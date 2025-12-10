import { CommonModule } from '@angular/common';
import { Component, EventEmitter, OnInit, Output, Input, OnChanges, SimpleChanges, OnDestroy } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatChipsModule } from '@angular/material/chips';
import { MatMiniFabButton, MatFabButton } from "@angular/material/button";
import { MatIconModule } from '@angular/material/icon';
import { AzureBlobService } from '../../../views/configuracao/services/azure-blob.service';
import { NotificacaoToastrService } from '../notificacao/notificacao-toastr.service';
import { Router } from '@angular/router';
import { Subject, takeUntil } from 'rxjs';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatMenuModule } from '@angular/material/menu';
import { MatDivider } from '@angular/material/divider';

interface DiaCalendario {
  numero: number;
  diaDaSemana: string;
  diaDaSemanaAbrev: string;
  mesAtual: boolean;
  dataCompleta: Date;
  fechado: boolean;
}

interface ConfiguracaoHorarios {
  diasFechados: number[];
  datasFechadasEspecificas: string[];
  horarioAbertura?: string;
  horarioFechamento?: string;
}

@Component({
  selector: 'app-calendario',
  imports: [
    CommonModule,
    MatChipsModule,
    FormsModule,
    MatIconModule,
    MatMiniFabButton,
    MatMenuModule,
    MatTooltipModule,
    MatDivider
],
  templateUrl: './calendario.component.html',
  styleUrl: './calendario.component.scss'
})
export class CalendarioComponent implements OnInit, OnChanges, OnDestroy {

  @Input() configuracao: ConfiguracaoHorarios = { diasFechados: [], datasFechadasEspecificas: [] };
  @Output() diaSelecionado = new EventEmitter<{ dia: number; mes: number; ano: number; diaDaSemana: string }>();

  private destroy$ = new Subject<void>();

  meses: string[] = [];
  diasCalendario: DiaCalendario[] = [];
  mesSelecionado: number = 0;
  diaSelecionadoAtual: number | null = null;
  anoAtual: number = new Date().getFullYear();
  diasDaSemana: string[] = ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sáb'];

  constructor(
    private azureService: AzureBlobService,
    private toastr: NotificacaoToastrService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.meses = this.gerarListaDeMeses();
    this.mesSelecionado = new Date().getMonth();
    this.gerarCalendarioDoMes();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['configuracao'] && !changes['configuracao'].firstChange) {
      this.gerarCalendarioDoMes();
    }
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
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

    const estaFechada = this.configuracao.datasFechadasEspecificas.some(dataFechada => {
      const dataSemTempo = dataFechada.split('T')[0];
      return dataSemTempo === dataFormatada;
    });

    return estaFechada;
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

  FecharData(data: Date): void {
    const ano = data.getFullYear();
    const mes = String(data.getMonth() + 1).padStart(2, '0');
    const dia = String(data.getDate()).padStart(2, '0');

    const dataFormatada = `${ano}-${mes}-${dia}T00:00:00.000Z`;

    this.azureService.fecharDataEspecifica(dataFormatada)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: () => {
          this.processarSucesso();
          this.configuracao.datasFechadasEspecificas.push(dataFormatada);
          this.gerarCalendarioDoMes();
        },
        error: (erro) => this.processarFalha(erro)
      });
  }

  AbrirData(data: Date): void {
    const ano = data.getFullYear();
    const mes = String(data.getMonth() + 1).padStart(2, '0');
    const dia = String(data.getDate()).padStart(2, '0');

    const dataFormatada = `${ano}-${mes}-${dia}T00:00:00.000Z`;

    this.azureService.abrirDataEspecifica(dataFormatada)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: () => {
          this.toastr.sucesso('Data aberta com sucesso!');
          this.configuracao.datasFechadasEspecificas =
            this.configuracao.datasFechadasEspecificas.filter(d => {
              const dataSemTempo = d.split('T')[0];
              const dataComparacao = `${ano}-${mes}-${dia}`;
              return dataSemTempo !== dataComparacao;
            });
          // Regenera o calendário
          this.gerarCalendarioDoMes();
        },
        error: (erro) => this.processarFalha(erro)
      });
  }

  private processarSucesso(): void {
    this.toastr.sucesso('Data fechada com sucesso!');
  }

  private processarFalha(erro: Error): void {
    this.toastr.erro(erro.message);
  }
}
