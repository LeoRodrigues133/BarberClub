import { SelecionarServicosRequest } from "../../servico/models/servico.models";

export interface ListagemFuncionario {
  id: string;
  nome: string;
  nomeApresentacao: string;
  cpf: String;
  email: string;
  avatar: string;
  cargo: number;
}

export interface CadastrarFuncionario {
  nome: string;
  cpf: String;
  nomeApresentacao?: string;

  login: string;
  senha: string;

  avatar: string;
  cargo: number;
}

export interface EditarFuncionario {
  id: string;
  nome?: string;
  nomeApresentacao: string;
  cpf?: string;
  avatar?: string;
  cargo?: number | null;
}

export interface SelecionarPorId {
  id: string;
  nome: string;
  cpf: String;

  nomeApresentacao: string;
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
  mes: number;
  ano: number;
}
export interface CadastrarVariosHorariosResponse {
  qtHorariosGerados: number;
  qtHorariosRemovidos: number;
  horariosCadastrados: any[]
}

export interface FuncionarioComServicos {
  funcionario: SelecionarPorId;
  servicos: SelecionarServicosRequest[];
}
