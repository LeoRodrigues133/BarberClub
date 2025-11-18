import { HttpClient, HttpErrorResponse } from "@angular/common/http";
import { HttpResponse } from "../models/http-response.model";
import { Observable, throwError } from "rxjs";

export abstract class BaseHttpService {
  protected constructor(protected http: HttpClient) {
  }

  protected processarDados(resposta: HttpResponse): any {
    if (resposta.success) return resposta.data;

    throw new Error('Erro ao mapear dados requisitados.', {
      cause: resposta.errors,
    });
  }

  protected processarDadosAninhados(resposta: HttpResponse, entidade: string): any {
    if (resposta.success && resposta.data) return resposta.data[entidade];

    throw new Error('Erro ao mapear dados requisitados.', {
      cause: resposta.errors,
    });
  }

  protected processarFalha(resposta: HttpErrorResponse): Observable<never> {
    return throwError(() => new Error(resposta.error.Errors[0]));
  }
}
