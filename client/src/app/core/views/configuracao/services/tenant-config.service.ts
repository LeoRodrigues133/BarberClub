import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { tap, catchError, shareReplay } from 'rxjs/operators';
import { environment } from '../../../../../environments/environment.development';
import { ConfiguracaoTenant } from '../models/service.models';

@Injectable({
  providedIn: 'root'
})
export class ServicoConfiguracaoTenant {
  private readonly API_URL = `${environment.urlApi}/configuracao`;
  private readonly DURACAO_CACHE_MS = 24 * 60 * 60 * 1000; // 1 dia

  private configuracao$ = new BehaviorSubject<ConfiguracaoTenant | null>(null);
  private erro$ = new BehaviorSubject<string | null>(null);
  private carregando$ = new BehaviorSubject<boolean>(false);
  private cacheObservable$: Observable<ConfiguracaoTenant> | null = null;
  private timestampCache: number = 0;

  constructor(private http: HttpClient) { }

  carregarPorAutenticacao(): Observable<ConfiguracaoTenant> {
    return this.executarCarregamento(`${this.API_URL}`, true);
  }

  carregarPorSlug(slug: string): Observable<ConfiguracaoTenant> {
    return this.executarCarregamento(`${this.API_URL}/public/${slug}`, false);
  }

  carregarAutomaticamente(slug?: string): Observable<ConfiguracaoTenant> {
    if (this.cacheValido()) {
      return this.cacheObservable$!;
    }

    if (this.possuiTokenValido()) {
      return this.carregarPorAutenticacao();
    }

    if (slug) {
      return this.carregarPorSlug(slug);
    }

    return throwError(() => new Error('Não há autenticação nem slug para carregar configuração'));
  }

  async recarregarConfiguracao(slug?: string): Promise<ConfiguracaoTenant> {
    this.invalidarCache();

    try {
      if (this.possuiTokenValido()) {
        return await this.carregarPorAutenticacao().toPromise() as ConfiguracaoTenant;
      } else if (slug) {
        return await this.carregarPorSlug(slug).toPromise() as ConfiguracaoTenant;
      }
      throw new Error('Não foi possível recarregar: sem autenticação ou slug');
    } catch (error) {
      throw new Error('Falha ao recarregar configuração: ' + (error as Error).message);
    }
  }

  limparConfiguracao(): void {
    this.invalidarCache();
    this.configuracao$.next(null);
    this.erro$.next(null);
    this.carregando$.next(false);
  }

  invalidarCache(): void {
    this.cacheObservable$ = null;
    this.timestampCache = 0;
  }

  obterConfiguracao(): Observable<ConfiguracaoTenant | null> {
    return this.configuracao$.asObservable();
  }

  obterEstadoErro(): Observable<string | null> {
    return this.erro$.asObservable();
  }

  obterEstadoCarregamento(): Observable<boolean> {
    return this.carregando$.asObservable();
  }

  get nomeEmpresa(): string {
    return this.configuracao$.value?.nomeEmpresa || 'BarberClub';
  }

  get slugEmpresa(): string | null {
    return this.configuracao$.value?.slugEmpresa || null;
  }

  get urlLogo(): string | null {
    return this.configuracao$.value?.logoUrl || null;
  }

  get urlBanner(): string | null {
    return this.configuracao$.value?.bannerUrl || null;
  }

  get estaAtivo(): boolean {
    return this.configuracao$.value?.ativo || false;
  }

  get configuracaoAtual(): ConfiguracaoTenant | null {
    return this.configuracao$.value;
  }

  private cacheValido(): boolean {
    if (!this.cacheObservable$ || !this.timestampCache) {
      return false;
    }

    const tempoDecorrido = Date.now() - this.timestampCache;
    return tempoDecorrido < this.DURACAO_CACHE_MS;
  }

  private executarCarregamento(url: string, requerAutenticacao: boolean): Observable<ConfiguracaoTenant> {
    if (this.cacheValido()) {
      return this.cacheObservable$!;
    }

    if (this.carregando$.value && this.cacheObservable$) {
      return this.cacheObservable$;
    }

    if (requerAutenticacao && !this.possuiTokenValido()) {
      return throwError(() => new Error('Token de autenticação inválido ou expirado'));
    }

    // Inicia novo carregamento
    this.erro$.next(null);
    this.carregando$.next(true);

    this.cacheObservable$ = this.http.get<any>(url).pipe(
      tap((resposta) => {
        const dados = this.extrairDados(resposta);
        const configuracao = this.mapearConfiguracao(dados);

        this.configuracao$.next(configuracao);
        this.carregando$.next(false);
        this.timestampCache = Date.now();

        this.agendarInvalidacaoCache();
      }),
      catchError((erro) => {
        const mensagemErro = this.tratarErro(erro);

        this.erro$.next(mensagemErro);
        this.carregando$.next(false);
        this.configuracao$.next(null);
        this.invalidarCache();

        return throwError(() => new Error(mensagemErro));
      }),
      shareReplay({ bufferSize: 1, refCount: false })
    );

    return this.cacheObservable$;
  }

  private agendarInvalidacaoCache(): void {
    setTimeout(() => {
      if (this.cacheValido()) {
        this.invalidarCache();
      }
    }, this.DURACAO_CACHE_MS);
  }

  private mapearConfiguracao(dados: any): ConfiguracaoTenant {
    return {
      empresaId: dados.id || dados.empresaId,
      nomeEmpresa: dados.nomeEmpresa || '',
      slugEmpresa: dados.slugEmpresa || '',
      logoUrl: dados.logoUrl || null,
      bannerUrl: dados.bannerUrl || null,
      ativo: dados.ativo ?? true,
      dataCriacao: dados.dataCriacao ? new Date(dados.dataCriacao) : new Date(),
    };
  }

  private possuiTokenValido(): boolean {
    try {
      const tokenString = localStorage.getItem('BarberClub.token');
      if (!tokenString) return false;

      const token = JSON.parse(tokenString);

      if (!token.expiracaoToken) return false;

      const dataExpiracao = new Date(token.expiracaoToken);
      const agora = new Date();

      const margemSeguranca = 5 * 60 * 1000;
      return dataExpiracao.getTime() > (agora.getTime() + margemSeguranca);
    } catch (erro) {
      console.error('Erro ao validar token:', erro);
      return false;
    }
  }

  private extrairDados(resposta: any): any {
    return resposta?.value || resposta?.data || resposta;
  }

  private tratarErro(erro: any): string {
    console.error('Erro no ServicoConfiguracaoTenant:', erro);

    if (erro.status === 404) {
      return 'Empresa não encontrada';
    }

    if (erro.status === 401 || erro.status === 403) {
      return 'Não autorizado a acessar as configurações';
    }

    if (erro.status === 0) {
      return 'Erro de conexão com o servidor';
    }

    if (erro.status >= 500) {
      return 'Erro interno do servidor';
    }

    return erro.error?.message || erro.message || 'Erro ao carregar configuração';
  }
}
