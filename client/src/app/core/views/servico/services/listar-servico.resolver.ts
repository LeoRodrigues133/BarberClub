import { ActivatedRouteSnapshot, ResolveFn } from '@angular/router';
import { ServicoService } from './servico.service';
import { inject } from '@angular/core';
import { SelecionarServicosRequest } from '../models/servico.models';

export const ListarServicoResolver: ResolveFn<
SelecionarServicosRequest> = (route: ActivatedRouteSnapshot) => {

  return inject(ServicoService).SelecionarTodos();
};
