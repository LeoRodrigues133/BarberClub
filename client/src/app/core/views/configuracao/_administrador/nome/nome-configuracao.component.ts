import { Component, OnInit } from '@angular/core';
import { AzureBlobService } from '../../services/azure-blob.service';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Subject, takeUntil } from 'rxjs';
import { ServicoConfiguracaoTenant } from '../../services/tenant-config.service';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { ConfiguracaoEmpresa } from '../../models/service.models';
import { ActivatedRoute } from '@angular/router';
import { NotificacaoToastrService } from '../../../../shared/components/notificacao/notificacao-toastr.service';

@Component({
  selector: 'app-nome-configuracao',
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
  templateUrl: './nome-configuracao.component.html',
  styleUrl: './nome-configuracao.component.scss'
})
export class NomeConfiguracaoComponent implements OnInit {
  private destroy$ = new Subject<void>();

  formEmpresa: FormGroup;
  salvandoNome = false;
  configuracao: ConfiguracaoEmpresa | null = null;


  constructor(
    private azureBlobService: AzureBlobService,
    private tenantProvider: ServicoConfiguracaoTenant,
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private toastr: NotificacaoToastrService

  ) {
    this.formEmpresa = this.fb.group({
      nomeEmpresa: ['', [Validators.required, Validators.minLength(3)]]
    });
  }
  ngOnInit(): void {
    const empresa = this.route.snapshot.data['funcionario'];

    this.formEmpresa.patchValue(empresa);
  }

  salvarNome(): void {
    if (this.formEmpresa.invalid) return;

    this.salvandoNome = true;
    const nomeEmpresa = this.formEmpresa.value.nomeEmpresa;

    this.azureBlobService.atualizarNomeEmpresa(nomeEmpresa)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: async (config) => {
          this.configuracao = config;
          await this.tenantProvider.recarregarConfiguracao();

          this.salvandoNome = false;
          this.toastr.sucesso('Nome atualizado! Visível em toda aplicação.');
        },
        error: (err) => {
          this.salvandoNome = false;
          this.toastr.erro('Erro ao atualizar nome da empresa.');
        }
      });
  }

  get nomeEmpresa() {
    return this.formEmpresa.get('nomeEmpresa');
  }
}
