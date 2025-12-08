import { CommonModule } from '@angular/common';
import { Component, EventEmitter, OnInit, Output, Input, OnChanges, SimpleChanges } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatChipsModule } from '@angular/material/chips';
import { MatButton, MatMiniFabButton } from "@angular/material/button";
import { MatIconModule } from '@angular/material/icon';

interface DiaCalendario {
  numero: number;
  diaDaSemana: string;
  diaDaSemanaAbrev: string;
  mesAtual: boolean;
  dataCompleta: Date;
  fechado: boolean;
}

interface ConfiguracaoHorarios {
  diasFechados: number[]; // Array com os dias da semana fechados (0 = Domingo, 6 = Sábado)
  datasFechadasEspecificas: string[]; // Array com datas específicas fechadas no formato 'YYYY-MM-DD'
  horarioAbertura?: string; // Ex: "08:00"
  horarioFechamento?: string; // Ex: "18:00"
}

@Component({
  selector: 'app-calendario',
  imports: [
    CommonModule,
    MatChipsModule,
    FormsModule,
    MatIconModule,
    MatMiniFabButton
  ],
  templateUrl: './calendario.component.html',
  styleUrl: './calendario.component.scss'
})
export class CalendarioComponent implements OnInit, OnChanges {

  @Input() configuracao: ConfiguracaoHorarios = { diasFechados: [], datasFechadasEspecificas: [] };
  @Output() diaSelecionado = new EventEmitter<{ dia: number; mes: number; ano: number; diaDaSemana: string }>();


  meses: string[] = [];
  diasCalendario: DiaCalendario[] = [];
  mesSelecionado: number = 0;
  diaSelecionadoAtual: number | null = null;
  anoAtual: number = new Date().getFullYear();
  diasDaSemana: string[] = ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sáb'];

  ngOnInit(): void {
    this.meses = this.gerarListaDeMeses();
    this.mesSelecionado = new Date().getMonth();
    this.gerarCalendarioDoMes();
    console.log(this.configuracao.datasFechadasEspecificas)

  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['configuracao'] && !changes['configuracao'].firstChange) {
      this.gerarCalendarioDoMes();
    }
  }

  public onMesChange(mesIndex: number): void {
    this.mesSelecionado = mesIndex;
    this.diaSelecionadoAtual = null;
    this.gerarCalendarioDoMes();
  }

  public onDiaClick(dia: DiaCalendario): void {
    if (!dia.mesAtual || dia.fechado) return;

    this.diaSelecionadoAtual = dia.numero;
    this.diaSelecionado.emit({
      dia: dia.numero,
      mes: this.mesSelecionado + 1,
      ano: this.anoAtual,
      diaDaSemana: dia.diaDaSemana
    });
  }

  private gerarCalendarioDoMes(): void {
    const primeiroDia = new Date(this.anoAtual, this.mesSelecionado, 1);
    const ultimoDia = new Date(this.anoAtual, this.mesSelecionado + 1, 0);
    const totalDias = ultimoDia.getDate();
    const diaSemanaInicio = primeiroDia.getDay();

    this.diasCalendario = [];

    const mesAnterior = new Date(this.anoAtual, this.mesSelecionado, 0);
    const diasMesAnterior = mesAnterior.getDate();

    for (let i = diaSemanaInicio - 1; i >= 0; i--) {
      const numeroDia = diasMesAnterior - i;
      const data = new Date(this.anoAtual, this.mesSelecionado - 1, numeroDia);
      this.diasCalendario.push({
        numero: numeroDia,
        diaDaSemana: this.obterNomeDia(data),
        diaDaSemanaAbrev: this.diasDaSemana[data.getDay()],
        mesAtual: false,
        dataCompleta: data,
        fechado: false
      });
    }

    for (let dia = 1; dia <= totalDias; dia++) {
      const data = new Date(this.anoAtual, this.mesSelecionado, dia);
      this.diasCalendario.push({
        numero: dia,
        diaDaSemana: this.obterNomeDia(data),
        diaDaSemanaAbrev: this.diasDaSemana[data.getDay()],
        mesAtual: true,
        dataCompleta: data,
        fechado: this.verificarDiaFechado(data)
      });
    }

    const diasRestantes = 42 - this.diasCalendario.length;
    for (let dia = 1; dia <= diasRestantes; dia++) {
      const data = new Date(this.anoAtual, this.mesSelecionado + 1, dia);
      this.diasCalendario.push({
        numero: dia,
        diaDaSemana: this.obterNomeDia(data),
        diaDaSemanaAbrev: this.diasDaSemana[data.getDay()],
        mesAtual: false,
        dataCompleta: data,
        fechado: false
      });
    }
  }

  private verificarDiaFechado(data: Date): boolean {
    const diaDaSemana = data.getDay();
    if (this.configuracao.diasFechados.includes(diaDaSemana)) {
      return true;
    }

    const dataFormatada = this.formatarData(data);
    if (this.configuracao.datasFechadasEspecificas.includes(dataFormatada)) {
      return true;
    }

    return false;
  }

  private formatarData(data: Date): string {
    const ano = data.getFullYear();
    const mes = String(data.getMonth() + 1).padStart(2, '0');
    const dia = String(data.getDate()).padStart(2, '0');
    return `${ano}-${mes}-${dia}`;
  }

  private obterNomeDia(data: Date): string {
    return data.toLocaleDateString('pt-BR', { weekday: 'long' });
  }

  private gerarListaDeMeses(): string[] {
    const meses: string[] = [];
    for (let i = 0; i < 12; i++) {
      const data = new Date(2000, i, 1);
      const nomeMes = data.toLocaleDateString('pt-BR', { month: 'long' });
      meses.push(this.capitalize(nomeMes));
    }
    return meses;
  }

  private capitalize(texto: string): string {
    return texto.charAt(0).toUpperCase() + texto.slice(1);
  }

  public ehHoje(dia: DiaCalendario): boolean {
    const hoje = new Date();
    return dia.dataCompleta.getDate() === hoje.getDate() &&
      dia.dataCompleta.getMonth() === hoje.getMonth() &&
      dia.dataCompleta.getFullYear() === hoje.getFullYear();
  }
}
