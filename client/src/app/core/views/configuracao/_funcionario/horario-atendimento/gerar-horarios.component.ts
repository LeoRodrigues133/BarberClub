import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AzureBlobService } from '../../services/azure-blob.service';
import { Subject, takeUntil } from 'rxjs';
import { HorarioFuncionamento } from '../../models/service.models';
import { NotificacaoToastrService } from '../../../../shared/components/notificacao/notificacao-toastr.service';
import { CalendarioComponent } from "../../../../shared/components/calendario/calendario.component";

@Component({
  selector: 'app-gerar-horarios',
  standalone: true,
  imports: [
    CommonModule,
    CalendarioComponent
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
  ) { }

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
          this.toastr.erro('Erro ao carregar configuração: ' + err);
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
      datasFechadasEspecificas: ['2025-12-25'], // Inicialmente vazio, pode ser populado via backend futuramente
                                                //formato 'ano-mes-dia' = '2025-12-25'
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
