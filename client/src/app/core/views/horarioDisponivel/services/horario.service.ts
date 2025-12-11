import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../../../environments/environment.development';
import { BaseHttpService } from '../../../shared/services/http-response.service';
import { catchError, map, Observable } from 'rxjs';
import { HttpResponse } from '../../../shared/models/http-response.model';

@Injectable({
  providedIn: 'root'
})
export class HorarioService extends BaseHttpService {
  readonly API_URL = `${environment.urlApi}/horario`

  constructor(http: HttpClient) {
    super(http)
  }

  SelecionarHorarioPorData(funcionarioId: string, dataSelecionada: string): Observable<any> {
    const urlCompleto = `${this.API_URL}/visualizar-por-data`;

    const body = {
      funcionarioId: funcionarioId,
      dataSelecionada: dataSelecionada
    };

    return this.http.post<HttpResponse>(urlCompleto, body)
      .pipe(
        map(res => this.processarDadosAninhados(res, 'horarios')),
        catchError(this.processarFalha)
      );
  }

  DesativarHorario(id: string): Observable<any> {
    const urlCompleto = `${this.API_URL}/desativar/${id}`;
    return this.http.put<HttpResponse>(urlCompleto, {})
      .pipe(
        map(res => this.processarDadosAninhados(res, 'horarios')),
        catchError(this.processarFalha)
      );
  }

  AtivarHorario(id: string): Observable<any> {
    const urlCompleto = `${this.API_URL}/ativar/${id}`;
    return this.http.put<HttpResponse>(urlCompleto, {})
      .pipe(
        map(res => this.processarDadosAninhados(res, 'horarios')),
        catchError(this.processarFalha)
      );
  }
}
