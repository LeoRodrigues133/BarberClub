
export interface HorarioFuncionamento {
  id: string;
  diaSemana: string;
  horaAbertura: string | null;
  horaFechamento: string | null;
  fechado: boolean;
}

export interface ConfiguracaoEmpresa {
  id: string;
  nomeEmpresa: string;
  logoUrl: string;
  bannerUrl: string;
  ativo: boolean;
  dataCriacao: Date;
  horarioDeExpediente: HorarioFuncionamento[];
  datasEspecificasFechado: any[];
}

export interface ImagemComToken {
  urlOriginal: string;
  urlComToken: string;
  expiraEm: Date;
}


export interface ConfiguracaoTenant {
  empresaId: string;
  nomeEmpresa: string;
  slugEmpresa: string;
  logoUrl: string | null;
  bannerUrl: string | null;
  ativo: boolean;
  dataCriacao: Date;
}

export interface InformacaoEmpresa {
  descricao?: string;
  telefone?: string;
  endereco?: string;
  redesSociais: RedeSocial[];
}

export interface RedeSocial {
  logo: string;
  link: string;
}
