import { CommonModule } from '@angular/common';
import {
  Component,
  EventEmitter,
  OnInit,
  Output,
  Input, OnChanges,
  SimpleChanges,
  OnDestroy
} from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatChipsModule } from '@angular/material/chips';
import { MatMiniFabButton } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatDivider } from '@angular/material/divider';
import { Subject, takeUntil } from 'rxjs';
import { AzureBlobService } from '../../../views/configuracao/services/azure-blob.service';
import { NotificacaoToastrService } from '../notificacao/notificacao-toastr.service';
import { Router } from '@angular/router';
import { ConfiguracaoHorarios, DiaCalendario } from '../../models/calendario.model';

@Component({
  selector: 'app-calendario',
  standalone: true,
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

  @Input() configuracao: ConfiguracaoHorarios = {
    diasFechados: [],
    datasFechadasEspecificas: []
  };

  @Output() diaSelecionado = new EventEmitter<{
    dia: number;
    mes: number;
    ano: number;
    diaDaSemana: string;
  }>();

  @Output() gerarHorarios = new EventEmitter<Date>();
  @Output() visualizarHorarios = new EventEmitter<Date>();

  meses: string[] = [];
  diasCalendario: DiaCalendario[] = [];
  mesSelecionado: number = 0;
  diaSelecionadoAtual: number | null = null;
  anoAtual: number = new Date().getFullYear();
  diasDaSemana: string[] = ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sáb'];

  private readonly destroy$ = new Subject<void>();

  constructor(
    private readonly azureService: AzureBlobService,
    private readonly toastr: NotificacaoToastrService,
    private readonly router: Router
  ) { }


  ngOnInit(): void {
    this.inicializarCalendario();
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
    if (!this.isDiaClicavel(dia)) return;

    this.diaSelecionadoAtual = dia.numero;
    this.emitirDiaSelecionado(dia);
  }

  public onGerarHorarios(data: Date, event: Event): void {
    event.stopPropagation();
    this.gerarHorarios.emit(data);
  }

  public onVisualizarHorarios(data: Date, event: Event): void {
    event.stopPropagation();
    this.visualizarHorarios.emit(data);
  }

  public ehHoje(dia: DiaCalendario): boolean {
    const hoje = new Date();
    return (
      dia.dataCompleta.getDate() === hoje.getDate() &&
      dia.dataCompleta.getMonth() === hoje.getMonth() &&
      dia.dataCompleta.getFullYear() === hoje.getFullYear()
    );
  }

  public fecharData(data: Date): void {
    const dataFormatada = this.formatarDataISO(data);

    this.azureService
      .fecharDataEspecifica(dataFormatada)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: () => this.handleFecharDataSucesso(dataFormatada),
        error: (erro) => this.handleErro(erro)
      });
  }

  public abrirData(data: Date): void {
    const dataFormatada = this.formatarDataISO(data);

    this.azureService
      .abrirDataEspecifica(dataFormatada)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: () => this.handleAbrirDataSucesso(dataFormatada),
        error: (erro) => this.handleErro(erro)
      });
  }

  private inicializarCalendario(): void {
    this.meses = this.gerarListaDeMeses();
    this.mesSelecionado = new Date().getMonth();
    this.gerarCalendarioDoMes();
  }

  private gerarListaDeMeses(): string[] {
    return Array.from({ length: 12 }, (_, i) => {
      const data = new Date(2000, i, 1);
      const nomeMes = data.toLocaleDateString('pt-BR', { month: 'long' });
      return this.capitalize(nomeMes);
    });
  }

  private gerarCalendarioDoMes(): void {
    this.diasCalendario = [
      ...this.gerarDiasMesAnterior(),
      ...this.gerarDiasMesAtual(),
      ...this.gerarDiasMesSeguinte()
    ];
  }


  //Completa os dias do mês anterior para manter o grid de 42 dias (6 semanas)

  private gerarDiasMesAnterior(): DiaCalendario[] {
    const primeiroDia = new Date(this.anoAtual, this.mesSelecionado, 1);
    const diaSemanaInicio = primeiroDia.getDay();

    if (diaSemanaInicio === 0) return []; // Sem dias anteriores necessários

    const mesAnterior = new Date(this.anoAtual, this.mesSelecionado, 0);
    const diasMesAnterior = mesAnterior.getDate();
    const dias: DiaCalendario[] = [];

    for (let i = diaSemanaInicio - 1; i >= 0; i--) {
      const numeroDia = diasMesAnterior - i;
      const data = new Date(this.anoAtual, this.mesSelecionado - 1, numeroDia);
      dias.push(this.criarDiaCalendario(data, numeroDia, false));
    }

    return dias;
  }


  // Gera todos os dias do mês atual com status de fechado/aberto

  private gerarDiasMesAtual(): DiaCalendario[] {
    const ultimoDia = new Date(this.anoAtual, this.mesSelecionado + 1, 0);
    const totalDias = ultimoDia.getDate();
    const dias: DiaCalendario[] = [];

    for (let dia = 1; dia <= totalDias; dia++) {
      const data = new Date(this.anoAtual, this.mesSelecionado, dia);
      const fechado = this.verificarDiaFechado(data);
      dias.push(this.criarDiaCalendario(data, dia, true, fechado));
    }

    return dias;
  }


  // Completa os dias do próximo mês para manter o grid de 42 dias (6 semanas)

  private gerarDiasMesSeguinte(): DiaCalendario[] {
    const diasJaGerados = this.countDiasGerados();
    const diasRestantes = 42 - diasJaGerados;

    if (diasRestantes <= 0) return [];

    const dias: DiaCalendario[] = [];

    for (let dia = 1; dia <= diasRestantes; dia++) {
      const data = new Date(this.anoAtual, this.mesSelecionado + 1, dia);
      dias.push(this.criarDiaCalendario(data, dia, false));
    }

    return dias;
  }

  private countDiasGerados(): number {
    const primeiroDia = new Date(this.anoAtual, this.mesSelecionado, 1);
    const ultimoDia = new Date(this.anoAtual, this.mesSelecionado + 1, 0);
    const diaSemanaInicio = primeiroDia.getDay();
    const totalDiasMesAtual = ultimoDia.getDate();

    return diaSemanaInicio + totalDiasMesAtual;
  }

  private criarDiaCalendario(
    data: Date,
    numero: number,
    mesAtual: boolean,
    fechado: boolean = false
  ): DiaCalendario {
    return {
      numero,
      diaDaSemana: this.obterNomeDia(data),
      diaDaSemanaAbrev: this.diasDaSemana[data.getDay()],
      mesAtual,
      dataCompleta: data,
      fechado
    };
  }



  //Verifica se um dia específico está fechado
  //Pode ser fechado por ser dia da semana não útil OU data específica bloqueada
  private verificarDiaFechado(data: Date): boolean {
    return this.isDiaSemanaNaoUtil(data) || this.isDataEspecificaFechada(data);
  }

  //Verifica se o dia da semana está configurado como fechado
  //Ex: Se domingo (0) está em diasFechados, todos os domingos serão fechados
  private isDiaSemanaNaoUtil(data: Date): boolean {
    const diaDaSemana = data.getDay();
    return this.configuracao.diasFechados.includes(diaDaSemana);
  }

  private isDataEspecificaFechada(data: Date): boolean {
    const dataFormatada = this.formatarDataSimples(data);

    return this.configuracao.datasFechadasEspecificas.some(dataFechada => {
      const dataSemTempo = dataFechada.split('T')[0];
      return dataSemTempo === dataFormatada;
    });
  }

  private isDiaClicavel(dia: DiaCalendario): boolean {
    return dia.mesAtual && !dia.fechado;
  }


  private formatarDataSimples(data: Date): string {
    const ano = data.getFullYear();
    const mes = String(data.getMonth() + 1).padStart(2, '0');
    const dia = String(data.getDate()).padStart(2, '0');
    return `${ano}-${mes}-${dia}`;
  }

  private formatarDataISO(data: Date): string {
    return `${this.formatarDataSimples(data)}T00:00:00.000Z`;
  }

  private obterNomeDia(data: Date): string {
    return data.toLocaleDateString('pt-BR', { weekday: 'long' });
  }

  private capitalize(texto: string): string {
    return texto.charAt(0).toUpperCase() + texto.slice(1);
  }


  private emitirDiaSelecionado(dia: DiaCalendario): void {
    this.diaSelecionado.emit({
      dia: dia.numero,
      mes: this.mesSelecionado + 1,
      ano: this.anoAtual,
      diaDaSemana: dia.diaDaSemana
    });
  }

  private handleFecharDataSucesso(dataFormatada: string): void {
    this.toastr.sucesso('Data fechada com sucesso!');
    this.configuracao.datasFechadasEspecificas.push(dataFormatada);
    this.gerarCalendarioDoMes();
  }

  private handleAbrirDataSucesso(dataFormatada: string): void {
    this.toastr.sucesso('Data aberta com sucesso!');
    this.removerDataFechadaDaLista(dataFormatada);
    this.gerarCalendarioDoMes();
  }

  private removerDataFechadaDaLista(dataFormatada: string): void {
    const dataSemTempo = dataFormatada.split('T')[0];

    this.configuracao.datasFechadasEspecificas =
      this.configuracao.datasFechadasEspecificas.filter(d => {
        const dataComparacao = d.split('T')[0];
        return dataComparacao !== dataSemTempo;
      });
  }

  private handleErro(erro: Error): void {
    this.toastr.erro(erro.message);
  }
}
