import { ActivatedRouteSnapshot, ResolveFn } from "@angular/router";
import { FuncionarioService } from "./funcionario.service";
import { inject } from "@angular/core";

export const DetalharFuncionarioResolver:ResolveFn<
any/*DetalhesFuncionarioVm*/> = (route: ActivatedRouteSnapshot) => {
  const id = route.params['id'];

  return inject(FuncionarioService).SelecionarPorId(id);
}
