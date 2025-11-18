import { inject } from "@angular/core";
import { ResolveFn } from "@angular/router";
import { FuncionarioService } from "./funcionario.service";
import { ListagemFuncionario } from "../models/funcionario.models";

export const listarFuncionarioResolver: ResolveFn<
  ListagemFuncionario[]> = () => {
    return inject(FuncionarioService).SelecionarTodos();
  }
