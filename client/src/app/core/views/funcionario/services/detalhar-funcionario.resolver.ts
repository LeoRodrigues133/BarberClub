import { ActivatedRouteSnapshot, ResolveFn } from "@angular/router";
import { FuncionarioService } from "./funcionario.service";
import { inject } from "@angular/core";
import { SelecionarPorId } from "../models/funcionario.models";

export const DetalharFuncionarioResolver:ResolveFn<
SelecionarPorId> = (route: ActivatedRouteSnapshot) => {
  const id = route.params['id'];

  return inject(FuncionarioService).SelecionarPorId(id);
}
