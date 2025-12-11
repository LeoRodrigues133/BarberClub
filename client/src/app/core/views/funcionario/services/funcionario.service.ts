import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../../../environments/environment.development';
import { catchError, EMPTY, map, Observable, of, throwError } from 'rxjs';
import { CadastrarFuncionario, CadastrarVariosHorariosRequest, ConfigurarAtendimentoRequest, EditarFuncionario, ListagemFuncionario, SelecionarPorId } from '../models/funcionario.models';
import { BaseHttpService } from '../../../shared/services/http-response.service';
import { HttpResponse } from '../../../shared/models/http-response.model';

@Injectable({
  providedIn: 'root'
})
export class FuncionarioService extends BaseHttpService {
  readonly API_URL = `${environment.urlApi}/funcionario`

  constructor(http: HttpClient) {
    super(http)
  }
  public Cadastrar(registro: CadastrarFuncionario): Observable<CadastrarFuncionario> {
    const urlCompleto = `${this.API_URL}/cadastrar`;

    return this.http.post<HttpResponse>(urlCompleto, registro)
      .pipe(map(this.processarDados), catchError(this.processarFalha))
  }

  public Editar(id: string, registroEditado: EditarFuncionario): Observable<EditarFuncionario> {
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

  public SelecionarTodos(): Observable<ListagemFuncionario[]> {
    return this.http.get<HttpResponse>(this.API_URL)
      .pipe(map(res => this.processarDadosAninhados(res, 'funcionarios')),
        catchError(this.processarFalha));
  }

  public SelecionarPorId(id: string): Observable<SelecionarPorId> {
    const urlCompleto = `${this.API_URL}/${id}`;

    return this.http.get<HttpResponse>(urlCompleto)
      .pipe(map(this.processarDados), catchError(this.processarFalha));
  }

  public AtualizarTempoAtendimento(id: string, dados: ConfigurarAtendimentoRequest)
    : Observable<ConfigurarAtendimentoRequest> {
    const urlCompleto = `${this.API_URL}/${id}/configurar-atendimento`;

    return this.http.put<HttpResponse>(urlCompleto, dados)
      .pipe(map(res => this.processarDadosAninhados(res, 'funcionarios')),
        catchError(this.processarFalha));
  }

  public GerarHorarios(id: string, dados: CadastrarVariosHorariosRequest)
    : Observable<CadastrarVariosHorariosRequest> {
    const urlCompleto = `${this.API_URL}/${id}/gerar-horarios`;

    return this.http.post<HttpResponse>(urlCompleto, dados)
      .pipe(map(res => this.processarDados(res)),
        catchError(this.processarFalha))
  }

}
