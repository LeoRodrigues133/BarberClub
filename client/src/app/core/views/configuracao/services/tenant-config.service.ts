import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, of, throwError } from 'rxjs';
import { tap, catchError, shareReplay, filter, take } from 'rxjs/operators';
import { environment } from '../../../../../environments/environment.development';
import { ConfiguracaoTenant } from '../models/service.models';

@Injectable({
  providedIn: 'root'
})
export class ServicoConfiguracaoTenant {
  private readonly API_URL = `${environment.urlApi}/configuracao`;
  private readonly DURACAO_CACHE_MS = 24 * 60 * 60 * 1000; // 1 dia (igual ao token SAS)

  private configuracao$ = new BehaviorSubject<ConfiguracaoTenant | null>(null);
  private erro$ = new BehaviorSubject<string | null>(null);
  private carregando$ = new BehaviorSubject<boolean>(false);
  private cacheObservable$: Observable<ConfiguracaoTenant> | null = null;

  constructor(private http: HttpClient) { }


  carregarPorAutenticacao(): Observable<ConfiguracaoTenant> {
    return this.executarCarregamento(`${this.API_URL}`);
  }


  carregarPorSlug(slug: string): Observable<ConfiguracaoTenant> {
    return this.executarCarregamento(`${this.API_URL}/public/${slug}`);
  }

  carregarAutomaticamente(slug?: string): Observable<ConfiguracaoTenant> {
    if (this.cacheObservable$) {
      return this.cacheObservable$;
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

    if (this.possuiTokenValido()) {
      const config = await this.carregarPorAutenticacao().toPromise();
      if (!config) throw new Error('Falha ao recarregar configuração');
      return config;
    } else if (slug) {
      const config = await this.carregarPorSlug(slug).toPromise();
      if (!config) throw new Error('Falha ao recarregar configuração');
      return config;
    }

    throw new Error('Não foi possível recarregar: sem autenticação ou slug');
  }

  limparConfiguracao(): void {
    this.invalidarCache();
    this.configuracao$.next(null);
    this.erro$.next(null);
    this.carregando$.next(false);
  }

  invalidarCache(): void {
    this.cacheObservable$ = null;
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

  private executarCarregamento(url: string): Observable<ConfiguracaoTenant> {
    if (this.cacheObservable$) {
      return this.cacheObservable$;
    }

    if (this.carregando$.value) {
      return new Observable<ConfiguracaoTenant>(observer => {
        const subscription = this.configuracao$.subscribe(config => {
          if (config) {
            observer.next(config);
            observer.complete();
          }
        });
        return () => subscription.unsubscribe();
      });
    }

    this.erro$.next(null);
    this.carregando$.next(true);

    this.cacheObservable$ = this.http.get<any>(url).pipe(
      tap((resposta) => {
        const dados = this.extrairDados(resposta);

        const configuracao: ConfiguracaoTenant = {
          empresaId: dados.id,
          nomeEmpresa: dados.nomeEmpresa,
          slugEmpresa: dados.slugEmpresa || '',
          logoUrl: dados.logoUrl || null,
          bannerUrl: dados.bannerUrl || null,
          ativo: dados.ativo,
          dataCriacao: new Date(dados.dataCriacao),
        };

        this.configuracao$.next(configuracao);
        this.carregando$.next(false);

        setTimeout(() => this.invalidarCache(), this.DURACAO_CACHE_MS);
      }),
      catchError((erro) => {
        this.erro$.next(this.tratarErro(erro));
        this.carregando$.next(false);
        this.invalidarCache();
        throw erro;
      }),
      shareReplay(1)
    );

    return this.cacheObservable$;
  }

  private possuiTokenValido(): boolean {
    const tokenString = localStorage.getItem('BarberClub.token');
    if (!tokenString) return false;

    try {
      const token = JSON.parse(tokenString);
      const dataExpiracao = new Date(token.expiracaoToken);
      return dataExpiracao > new Date();
    } catch {
      return false;
    }
  }

  private extrairDados(resposta: any): any {
    return resposta.value || resposta.data || resposta;
  }

  private tratarErro(erro: any): string {
    if (erro.status === 404) {
      return 'Empresa não encontrada';
    } else if (erro.status === 401) {
      return 'Não autorizado';
    } else if (erro.status === 0) {
      return 'Erro de conexão';
    }
    return 'Erro ao carregar configuração';
  }
}
