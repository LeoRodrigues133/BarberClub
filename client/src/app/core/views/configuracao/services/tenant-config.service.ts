import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, timer, of, throwError } from 'rxjs';
import { tap, catchError, shareReplay, switchMap } from 'rxjs/operators';
import { environment } from '../../../../../environments/environment.development';

export interface ConfiguracaoTenant {
  empresaId: string;
  nomeEmpresa: string;
  slugEmpresa: string;
  logoUrl: string | null;
  bannerUrl: string | null;
  logoUrlComToken: string | null;
  bannerUrlComToken: string | null;
  ativo: boolean;
  dataCriacao: Date;
}

export interface InformacaoEmpresa {
  descricao?: string;
  telefone?: string;
  endereco?: string;
  redesSociais: RedeSocial[];
}

export interface RedeSocial {
  logo: string;
  link: string;
}

export enum EstrategiaCarregamento {
  POR_AUTENTICACAO = 'autenticacao',
  POR_SLUG = 'slug'
}

@Injectable({
  providedIn: 'root'
})
export class ServicoConfiguracaoTenant {
  private readonly urlBase = `${environment.urlApi}/configuracao`;
  private readonly duracaoToken = 50 * 60 * 1000;
  private readonly duracaoCache = 15 * 60 * 1000;

  private configuracao$ = new BehaviorSubject<ConfiguracaoTenant | null>(null);
  private erro$ = new BehaviorSubject<string | null>(null);
  private carregando$ = new BehaviorSubject<boolean>(false);
  private momentoUltimoCarregamento: number = 0;
  private temporizadorRenovacao: any = null;

  constructor(private http: HttpClient) {
    this.inicializar();
  }

  private inicializar(): void {
    if (this.possuiTokenValido()) {
      this.carregarPorAutenticacao().subscribe();
    }
  }

  carregarPorAutenticacao(): Observable<ConfiguracaoTenant> {
    return this.carregarConfiguracao(`${this.urlBase}`);
  }

  carregarPorSlug(slug: string): Observable<ConfiguracaoTenant> {
    return this.carregarConfiguracao(`${this.urlBase}/public/${slug}`);
  }

  carregarAutomaticamente(slug?: string): Observable<ConfiguracaoTenant> {
    if (this.cacheEstaValido()) {
      return of(this.configuracao$.value!);
    }

    if (this.possuiTokenValido()) {
      return this.carregarPorAutenticacao();
    } else if (slug) {
      return this.carregarPorSlug(slug);
    } else {
      return throwError(() => new Error('Não há token nem slug para carregar configuração'));
    }
  }

  private carregarConfiguracao(url: string): Observable<ConfiguracaoTenant> {
    if (this.carregando$.value) {
      return this.configuracao$.asObservable().pipe(
        switchMap(config => config ? of(config) : throwError(() => new Error('Configuração não disponível')))
      );
    }

    this.erro$.next(null);
    this.carregando$.next(true);

    return this.http.get<any>(url).pipe(
      switchMap(async (resposta) => {
        const dados = this.extrairDados(resposta);

        const [tokenLogo, tokenBanner] = await Promise.all([
          dados.logoUrl ? this.gerarToken(dados.logoUrl) : Promise.resolve(null),
          dados.bannerUrl ? this.gerarToken(dados.bannerUrl) : Promise.resolve(null)
        ]);

        const configuracao: ConfiguracaoTenant = {
          empresaId: dados.id,
          nomeEmpresa: dados.nomeEmpresa,
          slugEmpresa: dados.slugEmpresa || this.gerarSlug(dados.nomeEmpresa),
          logoUrl: dados.logoUrl,
          bannerUrl: dados.bannerUrl,
          logoUrlComToken: tokenLogo,
          bannerUrlComToken: tokenBanner,
          ativo: dados.ativo,
          dataCriacao: new Date(dados.dataCriacao),
        };

        this.configuracao$.next(configuracao);
        this.momentoUltimoCarregamento = Date.now();
        this.carregando$.next(false);

        this.agendarRenovacaoTokens();

        return configuracao;
      }),
      catchError((erro) => {
        this.erro$.next(this.tratarErro(erro));
        this.carregando$.next(false);
        throw erro;
      }),
      shareReplay(1)
    );
  }

  async recarregarConfiguracao(): Promise<void> {
    this.momentoUltimoCarregamento = 0;

    try {
      const config = this.configuracao$.value;

      if (this.possuiTokenValido()) {
        await this.carregarPorAutenticacao().toPromise();
      } else if (config?.slugEmpresa) {
        await this.carregarPorSlug(config.slugEmpresa).toPromise();
      }
    } catch (erro) {
      throw erro;
    }
  }

  private async gerarToken(urlBlob: string): Promise<string> {
    try {
      const resposta = await this.http.post<any>(
        `${this.urlBase}/gerar-token`,
        { url: urlBlob }
      ).toPromise();

      return resposta.urlComToken || resposta.value?.urlComToken || resposta.data?.urlComToken;
    } catch (erro) {
      return urlBlob;
    }
  }

  private agendarRenovacaoTokens(): void {
    if (this.temporizadorRenovacao) {
      this.temporizadorRenovacao.unsubscribe();
    }

    const tempoParaRenovar = this.duracaoToken - (5 * 60 * 1000);

    this.temporizadorRenovacao = timer(tempoParaRenovar).subscribe(async () => {
      const config = this.configuracao$.value;
      if (!config) return;

      try {
        const [tokenLogo, tokenBanner] = await Promise.all([
          config.logoUrl ? this.gerarToken(config.logoUrl) : Promise.resolve(null),
          config.bannerUrl ? this.gerarToken(config.bannerUrl) : Promise.resolve(null)
        ]);

        config.logoUrlComToken = tokenLogo;
        config.bannerUrlComToken = tokenBanner;

        this.configuracao$.next({ ...config });
        this.agendarRenovacaoTokens();
      } catch (erro) {
        throw erro;
      }
    });
  }

  limparConfiguracao(): void {
    if (this.temporizadorRenovacao) {
      this.temporizadorRenovacao.unsubscribe();
      this.temporizadorRenovacao = null;
    }

    this.configuracao$.next(null);
    this.erro$.next(null);
    this.carregando$.next(false);
    this.momentoUltimoCarregamento = 0;
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
    return this.configuracao$.value?.nomeEmpresa || 'Minha Empresa';
  }

  get slugEmpresa(): string | null {
    return this.configuracao$.value?.slugEmpresa || null;
  }

  get urlLogo(): string | null {
    return this.configuracao$.value?.logoUrlComToken || null;
  }

  get urlBanner(): string | null {
    return this.configuracao$.value?.bannerUrlComToken || null;
  }

  get estaAtivo(): boolean {
    return this.configuracao$.value?.ativo || false;
  }

  get configuracaoAtual(): ConfiguracaoTenant | null {
    return this.configuracao$.value;
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

  private cacheEstaValido(): boolean {
    if (!this.configuracao$.value) return false;

    const tempoDecorrido = Date.now() - this.momentoUltimoCarregamento;
    return tempoDecorrido < this.duracaoCache;
  }

  private extrairDados(resposta: any): any {
    return resposta.value || resposta.data || resposta;
  }

  private gerarSlug(nomeEmpresa: string): string {
    return nomeEmpresa
      .toLowerCase()
      .normalize('NFD')
      .replace(/[\u0300-\u036f]/g, '')
      .replace(/[^a-z0-9\s-]/g, '')
      .trim()
      .replace(/\s+/g, '-');
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
