import { Component, OnInit, OnDestroy } from '@angular/core';
import { FiltroChipsComponent } from "../../../../shared/components/filtroChips/filtro-chips.component";
import { CommonModule } from '@angular/common';
import { AzureBlobService } from '../../services/azure-blob.service';
import { Subject, takeUntil } from 'rxjs';
import { HorarioFuncionamento } from '../../models/service.models';
import { NotificacaoToastrService } from '../../../../shared/components/notificacao/notificacao-toastr.service';

@Component({
  selector: 'app-gerar-horarios',
  standalone: true,
  imports: [
    FiltroChipsComponent,
    CommonModule
  ],
  templateUrl: './gerar-horarios.component.html',
  styleUrls: ['./gerar-horarios.component.scss']
})
export class GerarHorariosComponent implements OnInit, OnDestroy {
  dataSelecionada: { dia: number; mes: number; ano: number; diaDaSemana: string } | null = null;
  private destroy$ = new Subject<void>();
  carregando = true;
  horariosOriginais: HorarioFuncionamento[] = [];

  configuracaoHorarios = {
    diasFechados: [] as number[],
    datasFechadasEspecificas: [] as string[],
  };

  constructor(
    private azureBlobService: AzureBlobService,
    private toastr: NotificacaoToastrService,
  ) {}

  ngOnInit(): void {
    this.carregarConfiguracaoHorarios();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private carregarConfiguracaoHorarios(): void {
    this.carregando = true;

    this.azureBlobService.obterConfiguracao()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (config) => {
          this.horariosOriginais = config.horarioDeExpediente;
          this.montarConfiguracao(config.horarioDeExpediente);
          this.carregando = false;
        },
        error: (err) => {
          this.toastr.erro('Erro ao carregar configuração: '+ err);
          this.carregando = false;
        }
      });
  }

  private montarConfiguracao(horarios: HorarioFuncionamento[]): void {
    const diasFechados: number[] = [];

    horarios.forEach((horario, index) => {
      if (horario.fechado) {
        diasFechados.push(index);
      }
    });

    this.configuracaoHorarios = {
      diasFechados: diasFechados,
      datasFechadasEspecificas: [], // Inicialmente vazio, pode ser populado via backend futuramente
    };
  }

  onDataSelecionada(data: { dia: number; mes: number; ano: number; diaDaSemana: string }): void {
    this.dataSelecionada = data;
  }

  obterNomeDiaSemana(numeroDia: number): string {
    const dias = [
      'Domingo',
      'Segunda-feira',
      'Terça-feira',
      'Quarta-feira',
      'Quinta-feira',
      'Sexta-feira',
      'Sábado'];
    return dias[numeroDia];
  }
}
