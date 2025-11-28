import { Subject, forkJoin, takeUntil } from 'rxjs';
import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { ServicoConfiguracaoTenant } from '../../services/tenant-config.service';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { ConfiguracaoEmpresa, HorarioFuncionamento } from '../../models/service.models';
import { AzureBlobService } from '../../services/azure-blob.service';
import { NotificacaoToastrService } from '../../../../shared/components/notificacao/notificacao-toastr.service';

@Component({
  selector: 'app-horario-configuracao',
  imports: [
    MatIconModule,
    MatCardModule,
    MatSlideToggleModule,
    MatInputModule,
    MatFormFieldModule,
    ReactiveFormsModule,
    CommonModule,
    MatButtonModule
  ],
  templateUrl: './horario-configuracao.component.html',
  styleUrl: './horario-configuracao.component.scss'
})
export class HorarioConfiguracaoComponent implements OnInit {
  private destroy$ = new Subject<void>();
  formHorarios: FormGroup;
  configuracao: ConfiguracaoEmpresa | null = null;
  carregando = false;
  salvandoHorarios = false;

  constructor(
    private fb: FormBuilder,
    private azureBlobService: AzureBlobService,
    private tenantProvider: ServicoConfiguracaoTenant,
    private toastr: NotificacaoToastrService

  ) {
    this.formHorarios = this.fb.group({
      horarios: this.fb.array([])
    });
  }

  ngOnInit(): void {
    this.azureBlobService.obterConfiguracao()
      .pipe(takeUntil(this.destroy$))
      .subscribe(res => {
        this.carregarHorarios(res.horarioDeExpediente);
      });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  carregarHorarios(horarios: HorarioFuncionamento[]): void {
    this.horarios.clear();
    horarios.forEach(h => {
      this.horarios.push(
        this.fb.group({
          id: [h.id],
          diaSemana: [{ value: h.diaSemana, disabled: true }],
          horaAbertura: [this.formatarHoraParaSite(h.horaAbertura)],
          horaFechamento: [this.formatarHoraParaSite(h.horaFechamento)],
          fechado: [h.fechado]
        })
      );
    });
  }

  private formatarHoraParaSite(timeSpan: string | null): string {
    if (!timeSpan) return '';
    const parts = timeSpan.split(':');
    return `${parts[0]}:${parts[1]}`;
  }

  private formatarHoraParaApi(timeSpan: string | null): string {
    if (!timeSpan) return '';
    const parts = timeSpan.split(':');
    if (parts.length === 2) {
      return `${parts[0]}:${parts[1]}:00`;
    }
    if (parts.length === 3) {
      return `${parts[0]}:${parts[1]}:${parts[2]}`;
    }
    return timeSpan;
  }

  salvarHorarios(): void {
    this.salvandoHorarios = true;
    const horarios = this.horarios.getRawValue();

    const requests = horarios.map(horario => {
      const horarioFormatado = {
        ...horario,
        horaAbertura: this.formatarHoraParaApi(horario.horaAbertura),
        horaFechamento: this.formatarHoraParaApi(horario.horaFechamento)
      };
      return this.azureBlobService.atualizarHorario(horarioFormatado);
    });

    forkJoin(requests)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: async () => {
          await this.tenantProvider.recarregarConfiguracao();
          this.salvandoHorarios = false;
          this.toastr.sucesso('Horários atualizados com sucesso!');
        },
        error: (err) => {
          this.toastr.aviso('Erro ao salvar horários: ' + err);
          this.salvandoHorarios = false;
          this.toastr.erro('Erro ao atualizar horários.');
        }
      });
  }

  get horarios(): FormArray {
    return this.formHorarios.get('horarios') as FormArray;
  }
}
