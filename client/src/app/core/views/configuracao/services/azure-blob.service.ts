import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { environment } from '../../../../../environments/environment.development';
import { ConfiguracaoEmpresa, HorarioFuncionamento } from '../models/service.models';
import { ServicoConfiguracaoTenant } from './tenant-config.service';

@Injectable({
  providedIn: 'root'
})
export class AzureBlobService {
  private readonly API_URL = `${environment.urlApi}/configuracao`;

  constructor(
    private http: HttpClient,
    private servicoTenant: ServicoConfiguracaoTenant
  ) { }

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
      tap(() => {
        this.servicoTenant.invalidarCache();
      }),
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
      tap(() => {
        this.servicoTenant.invalidarCache();
      }),
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
      tap(() => {
        this.servicoTenant.invalidarCache();
      }),
      map(response => response.value || response.data || response)
    );
  }

  atualizarHorario(horario: HorarioFuncionamento): Observable<ConfiguracaoEmpresa> {
    const urlCompleto = `${this.API_URL}/horario/${horario.id}`;

    return this.http.put<any>(urlCompleto, horario).pipe(
      tap(() => {
        this.servicoTenant.invalidarCache();
      }),
      map(response => response.value || response.data || response)
    );
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
