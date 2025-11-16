import { Subject } from 'rxjs';
import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { AzureBlobService, ConfiguracaoEmpresa, HorarioFuncionamento } from '../services/azure-blob.service';
import { ServicoConfiguracaoTenant } from '../services/tenant-config.service';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';

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

  constructor(private fb: FormBuilder,
    private azureBlobService: AzureBlobService,
    private tenantProvider: ServicoConfiguracaoTenant
  ) {
    this.formHorarios = this.fb.group({
      horarios: this.fb.array([])
    });

  }
  ngOnInit(): void {
    this.azureBlobService.obterConfiguracao().subscribe(res =>
      this.carregarHorarios(res.horarioDeExpediente)
    )
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


    horarios.forEach(horario => {
    horario.horaAbertura = this.formatarHoraParaApi(horario.horaAbertura);
    horario.horaFechamento = this.formatarHoraParaApi(horario.horaFechamento);


      this.azureBlobService.atualizarHorario(horario).subscribe();
    });

    this.salvandoHorarios = false;
  }


  get horarios(): FormArray {
    return this.formHorarios.get('horarios') as FormArray;
  }

}
