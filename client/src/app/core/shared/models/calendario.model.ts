export interface DiaCalendario {
  numero: number;
  diaDaSemana: string;
  diaDaSemanaAbrev: string;
  mesAtual: boolean;
  dataCompleta: Date;
  fechado: boolean;
}

export interface ConfiguracaoHorarios {
  diasFechados: number[];
  datasFechadasEspecificas: string[];
  horarioAbertura?: string;
  horarioFechamento?: string;
}
