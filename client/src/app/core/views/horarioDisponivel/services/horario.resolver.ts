import { inject } from '@angular/core';
import { ResolveFn } from '@angular/router';
import { HorarioService } from './horario.service';

export const horarioResolver: ResolveFn<any> = (route, state) => {
  const funcionarioId = route.paramMap.get('funcionarioId');

  const dataSelecionada = route.queryParamMap.get('data');

  if (!funcionarioId || !dataSelecionada) {
    throw new Error('Parâmetros obrigatórios não fornecidos');
  }

  return inject(HorarioService).SelecionarHorarioPorData(funcionarioId, dataSelecionada);
};
