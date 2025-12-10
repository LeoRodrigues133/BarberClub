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
  private destroy$ = new Subject<void>();
  carregando = true;

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
          console.log('Configuração carregada:', config);

          const diasFechados: number[] = [];
          config.horarioDeExpediente.forEach((horario, index) => {
            if (horario.fechado) {
              diasFechados.push(index);
            }
          });

          this.configuracaoHorarios = {
            diasFechados: diasFechados,
            datasFechadasEspecificas: config.datasEspecificasFechado || []
          };

          console.log('Configuração montada:', this.configuracaoHorarios);
          this.carregando = false;
        },
        error: (err) => {
          this.toastr.erro('Erro ao carregar configuração: ' + err);
          this.carregando = false;
        }
      });
  }
}
