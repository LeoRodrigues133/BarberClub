import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { AutenticarUsuarioRequest, RegistrarUsuarioRequest, TokenResponse } from '../models/auth.models';
import { catchError, map, Observable, throwError, tap } from 'rxjs';
import { ServicoConfiguracaoTenant } from '../../core/views/configuracao/services/tenant-config.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  readonly API_URL: string = environment.urlApi;

  constructor(
    private http: HttpClient,
    private servicoTenant: ServicoConfiguracaoTenant
  ) { }

  public registrar(
    registro: RegistrarUsuarioRequest
  ): Observable<TokenResponse> {
    const urlCompleto = `${this.API_URL}/auth/registrar`;
    return this.http
      .post<TokenResponse>(urlCompleto, registro)
      .pipe(
        map(this.processarDados),
        tap(() => {
          // Carrega configuração após registro bem-sucedido
          this.carregarConfiguracaoTenant();
        }),
        catchError(this.processarFalha)
      );
  }

  public login(loginUsuario: AutenticarUsuarioRequest): Observable<TokenResponse> {
    const urlCompleto = `${this.API_URL}/auth/autenticar`;
    return this.http
      .post<TokenResponse>(urlCompleto, loginUsuario)
      .pipe(
        map(this.processarDados),
        catchError(this.processarFalha)
      );
  }

  public logout(): Observable<any> {
    const urlCompleto = `${this.API_URL}/auth/sair`;
    return this.http.post(urlCompleto, {}).pipe(
      map(this.processarDados),
      catchError(this.processarFalha)
    );
  }

  public validarExpiracaoToken(dataExpiracaoToken: Date): boolean {
    return dataExpiracaoToken > new Date();
  }

  private carregarConfiguracaoTenant(): void {
    this.servicoTenant.carregarPorAutenticacao().subscribe({
      next: (config: any) => (this.processarDados(config)),
      error: (erro: any) => (this.processarFalha(erro))
    });
  }

  private processarDados(resposta: any): TokenResponse {
    if (resposta.success) return resposta.data;
    throw new Error('Erro ao mapear chave de autenticação.', {
      cause: resposta.errors,
    });
  }

  protected processarFalha(resposta: HttpErrorResponse): Observable<never> {
    return throwError(() => new Error(resposta.error.Errors[0]));
  }
}
