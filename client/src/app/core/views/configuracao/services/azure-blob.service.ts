import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject, timer } from 'rxjs';
import { map, switchMap, shareReplay, tap } from 'rxjs/operators';
import { environment } from '../../../../../environments/environment.development';

export interface HorarioFuncionamento {
  id: string;
  diaSemana: string;
  horaAbertura: string | null;
  horaFechamento: string | null;
  fechado: boolean;
}

export interface ConfiguracaoEmpresa {
  id: string;
  nomeEmpresa: string;
  logoUrl: string;
  bannerUrl: string;
  ativo: boolean;
  dataCriacao: Date;
  horarioDeExpediente: HorarioFuncionamento[];
}

export interface ImagemComToken {
  urlOriginal: string;
  urlComToken: string;
  expiraEm: Date;
}

@Injectable({
  providedIn: 'root'
})
export class AzureBlobService {
  private readonly API_URL = `${environment.urlApi}/configuracao`;
  private readonly TOKEN_DURATION_MS = 50 * 60 * 1000;

  private logoCache$ = new BehaviorSubject<ImagemComToken | null>(null);
  private bannerCache$ = new BehaviorSubject<ImagemComToken | null>(null);

  constructor(private http: HttpClient) { }

  obterConfiguracao(): Observable<ConfiguracaoEmpresa> {
    return this.http.get<any>(`${this.API_URL}`).pipe(
      map(response => {
        const data = response.value || response.data || response;
        return data;
      })
    );
  }

  atualizarNomeEmpresa(nomeEmpresa: string): Observable<ConfiguracaoEmpresa> {
    return this.http.put<any>(`${this.API_URL}/nome`, { nomeEmpresa }).pipe(
      map(response => response.value || response.data || response)
    );
  }

  uploadLogo(arquivo: File): Observable<ConfiguracaoEmpresa> {
    const formData = new FormData();
    formData.append('arquivo', arquivo);

    return this.http.put<any>(
      `${this.API_URL}/logo`,
      formData
    ).pipe(
      tap(() => this.logoCache$.next(null)),
      map(response => response.value || response.data || response)
    );
  }

  uploadBanner(arquivo: File): Observable<ConfiguracaoEmpresa> {
    const formData = new FormData();
    formData.append('arquivo', arquivo);

    return this.http.put<any>(
      `${this.API_URL}/banner`,
      formData
    ).pipe(
      tap(() => this.bannerCache$.next(null)),
      map(response => response.value || response.data || response)
    );
  }

  atualizarHorario(horario: HorarioFuncionamento): Observable<any> {
    const urlCompleto = `${this.API_URL}/horario/${horario.id}`

    return this.http.put<HorarioFuncionamento>(urlCompleto, horario)
      .pipe(
        switchMap(() => this.obterConfiguracao())
      );
  }

  obterLogoComToken(urlOriginal: string): Observable<string> {
    return this.obterUrlComTokenCached(urlOriginal, this.logoCache$, 'logo');
  }

  obterBannerComToken(urlOriginal: string): Observable<string> {
    return this.obterUrlComTokenCached(urlOriginal, this.bannerCache$, 'banner');
  }

  private obterUrlComTokenCached(
    urlOriginal: string,
    cache$: BehaviorSubject<ImagemComToken | null>,
    tipo: 'logo' | 'banner'
  ): Observable<string> {
    const cached = cache$.value;

    if (cached && cached.urlOriginal === urlOriginal && new Date() < cached.expiraEm) {
      return new BehaviorSubject(cached.urlComToken).asObservable();
    }

    return this.gerarUrlComToken(urlOriginal).pipe(
      tap(urlComToken => {
        const expiraEm = new Date(Date.now() + this.TOKEN_DURATION_MS);
        cache$.next({ urlOriginal, urlComToken, expiraEm });
        this.agendarRenovacao(urlOriginal, cache$, tipo);
      }),
      shareReplay(1)
    );
  }

  private gerarUrlComToken(urlOriginal: string): Observable<string> {
    return this.http.post<any>(
      `${this.API_URL}/gerar-token`,
      { url: urlOriginal }
    ).pipe(
      map(response => {
        const urlComToken = response.urlComToken || response.value?.urlComToken || response.data?.urlComToken;
        return urlComToken;
      })
    );
  }

  private agendarRenovacao(
    urlOriginal: string,
    cache$: BehaviorSubject<ImagemComToken | null>,
    tipo: 'logo' | 'banner'
  ): void {
    const renovarEm = this.TOKEN_DURATION_MS - (5 * 60 * 1000);

    timer(renovarEm).pipe(
      switchMap(() => this.gerarUrlComToken(urlOriginal))
    ).subscribe(urlComToken => {
      const expiraEm = new Date(Date.now() + this.TOKEN_DURATION_MS);
      cache$.next({ urlOriginal, urlComToken, expiraEm });
      this.agendarRenovacao(urlOriginal, cache$, tipo);
    });
  }

  limparCache(): void {
    this.logoCache$.next(null);
    this.bannerCache$.next(null);
  }

  public validarArquivo(arquivo: File): boolean {
    const tiposPermitidos = ['image/jpeg', 'image/jpg', 'image/png'];

    if (!tiposPermitidos.includes(arquivo.type)) {
      alert('Apenas JPG, JPEG e PNG são permitidos.');
      return false;
    }

    if (arquivo.size > 5 * 1024 * 1024) {
      alert('O arquivo deve ter no máximo 5MB.');
      return false;
    }

    return true;
  }
}
