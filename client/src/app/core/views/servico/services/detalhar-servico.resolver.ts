import { inject } from '@angular/core';
import { ActivatedRouteSnapshot, ResolveFn } from '@angular/router';
import { ServicoService } from './servico.service';
import { SelecionarServicoRequest } from '../models/servico.models';

export const DetalharServicoResolver: ResolveFn<
  SelecionarServicoRequest> = (route: ActivatedRouteSnapshot) => {
    const id = route.params['id'];

    return inject(ServicoService).SelecionarPorId(id);
  };
