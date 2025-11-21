import { Component, OnInit, OnDestroy } from '@angular/core';
import { Observable, Subject, takeUntil } from 'rxjs';
import { ServicoConfiguracaoTenant } from '../services/tenant-config.service';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { ConfiguracaoEmpresa } from '../models/service.models';
import { AzureBlobService } from '../services/azure-blob.service';

@Component({
  selector: 'app-avatar-configuracao',
  standalone: true,
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
  templateUrl: './avatar-configuracao.component.html',
  styleUrl: './avatar-configuracao.component.scss'
})
export class AvatarConfiguracaoComponent implements OnInit, OnDestroy {
  private destroy$ = new Subject<void>();

  configuracao: ConfiguracaoEmpresa | null = null;
  logoUrl$: string | null = null;

  uploadandoLogo = false;
  logoPreview: string | null = null;
  logoFile: File | null = null;

  constructor(
    private azureBlobService: AzureBlobService,
    private tenantProvider: ServicoConfiguracaoTenant
  ) {}

  ngOnInit(): void {
    this.carregarLogoAtual();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private carregarLogoAtual(): void {
    this.tenantProvider.obterConfiguracao()
      .pipe(takeUntil(this.destroy$))
      .subscribe(config => {
        // ✅ Backend já retorna URL completa com token SAS
        this.logoUrl$ = config?.logoUrl || null;
      });
  }


  selecionarLogo(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (!input.files?.length) return;

    const arquivo = input.files[0];

    if (!this.validarArquivo(arquivo)) return;

    this.logoFile = arquivo;

    const reader = new FileReader();
    reader.onload = e => this.logoPreview = e.target?.result as string;
    reader.readAsDataURL(arquivo);
  }

  uploadLogo(): void {
    if (!this.logoFile) return;

    this.uploadandoLogo = true;

    this.azureBlobService.uploadLogo(this.logoFile)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: async (config) => {

          await this.tenantProvider.recarregarConfiguracao();

          this.logoPreview = null;
          this.logoFile = null;
          this.uploadandoLogo = false;

          alert('Logo atualizado!');
        },
        error: (err) => {
          this.uploadandoLogo = false;
          alert('Erro ao fazer upload do logo');
        }
      });
  }

  cancelarLogo(): void {
    this.logoFile = null;
    this.logoPreview = null;
  }

  private validarArquivo(arquivo: File): boolean {
    const tiposPermitidos = ['image/jpeg', 'image/jpg', 'image/png'];

    if (!tiposPermitidos.includes(arquivo.type)) {
      alert('Apenas JPG, JPEG e PNG são permitidos.');
      return false;
    }

    if (arquivo.size > 5 * 1024 * 1024) {
      alert('O arquivo deve ter no máximo 5MB.');
      return false;
    }

    return true;
  }
}
