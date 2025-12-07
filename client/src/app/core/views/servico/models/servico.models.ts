export interface CadastrarServicoRequest {
  titulo: string;
  valor: number;
  duracao?: number,
  isPromocao: boolean,
  porcentagemPromocao: number;
}

export interface EditarServicoRequest {
  titulo: string;
  valor: number;
  duracao?: number,
  isPromocao: boolean,
  porcentagemPromocao: number;
}

export interface SelecionarServicosRequest {
  id: string;
  funcionarioId: string;
  titulo: string;
  valorFinal: number;
  duracao?: number,
  ativo: boolean
  isPromocao: boolean,
  porcentagemPromocao: number;
}

export interface SelecionarServicoRequest {
  id: string;
  titulo: string;
  valor: number;
  ativo: boolean
  valorFinal: number;
  duracao?: number,
  isPromocao: boolean,
  porcentagemPromocao: number;
}
