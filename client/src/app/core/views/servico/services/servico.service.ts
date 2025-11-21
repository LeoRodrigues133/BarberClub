import { Injectable } from '@angular/core';
import { environment } from '../../../../../environments/environment.development';
import { BaseHttpService } from '../../../shared/services/http-response.service';
import { HttpClient } from '@angular/common/http';
import { catchError, map, Observable } from 'rxjs';
import { HttpResponse } from '../../../shared/models/http-response.model';
import { CadastrarServicoRequest, EditarServicoRequest, SelecionarServicoRequest, SelecionarServicosRequest } from '../models/servico.models';

@Injectable({
  providedIn: 'root'
})
export class ServicoService extends BaseHttpService {
  readonly API_URL = `${environment.urlApi}/servico`;

  constructor(http: HttpClient) {
    super(http)
  }

  public SelecionarTodos(): Observable<SelecionarServicosRequest> {
    return this.http.get<HttpResponse>(this.API_URL)
      .pipe(map(res => this.processarDadosAninhados(res, 'servicos')),
        catchError(this.processarFalha));
  }

  public SelecionarPorId(id: string): Observable<SelecionarServicoRequest> {
    const urlCompleto = `${this.API_URL}/${id}`;

    return this.http.get<HttpResponse>(urlCompleto)
      .pipe(map(this.processarDados),
        catchError(this.processarFalha));
  }

  public Cadastrar(registro: CadastrarServicoRequest): Observable<CadastrarServicoRequest> {
    const urlCompleto = `${this.API_URL}/cadastrar`;

    return this.http.post<HttpResponse>(urlCompleto, registro)
      .pipe(map(this.processarDados),
        catchError(this.processarFalha));
  }

  public Editar(id: string, registroEditado: EditarServicoRequest): Observable<EditarServicoRequest> {
    const urlCompleto = `${this.API_URL}/editar/${id}`;

    return this.http.put<HttpResponse>(urlCompleto, registroEditado)
      .pipe(map(this.processarDados),
        catchError(this.processarFalha));
  }

  public Excluir(id: string): Observable<any> {
    const urlCompleto = `${this.API_URL}/excluir/${id}`;

    return this.http.delete<HttpResponse>(urlCompleto)
      .pipe(map(this.processarDados),
        catchError(this.processarFalha));
  }
}
