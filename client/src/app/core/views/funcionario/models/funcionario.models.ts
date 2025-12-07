import { SelecionarServicosRequest } from "../../servico/models/servico.models";

export interface ListagemFuncionario {
  id: string;
  nome: string;
  cpf: String;
  email: string;
  avatar: string;
  cargo: number;
}

export interface CadastrarFuncionario {
  nome: string;
  cpf: String;

  login: string;
  senha: string;

  avatar: string;
  cargo: number;
}

export interface EditarFuncionario {
  id: string;
  nome?: string;
  cpf?: string;
  avatar?: string;
  cargo?: number | null;
}

export interface SelecionarPorId {
  id: string;
  nome: string;
  cpf: String;

  userName: string;
  email?: string;

  avatar: string;
  cargo: number;
}

export interface ConfigurarAtendimentoRequest {
  id: string;
  tempoAtendimento: number;
  tempoIntervalo: number;
}

export interface CadastrarVariosHorariosRequest {
  id: string;
  mes: number;
  ano: number;
}

export interface FuncionarioComServicos{
  funcionario: SelecionarPorId;
  servicos: SelecionarServicosRequest[];
}
