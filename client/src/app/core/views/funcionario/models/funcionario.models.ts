export interface ListagemFuncionario {
  id: string;
  nome: string;
  cpf: String;
  avatar: string;
  cargo: string;
}

export interface CadastrarFuncionario {
  nome: string;
  cpf: String;

  login: string;
  senha: string;

  avatar: string;
  cargo: string;
}

export interface EditarFuncionario {
  id: string;
  nome: string;
  cpf: String;

  login: string;
  senha: string;

  avatar: string;
  cargo: string;
}

export interface SelecionarPorId {
  id: string;
  nome: string;
  cpf: String;

  userName: string;
  email?: string;

  avatar: string;
  cargo: string;
}
