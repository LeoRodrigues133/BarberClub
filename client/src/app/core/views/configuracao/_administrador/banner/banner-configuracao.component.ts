import { Component, OnInit, OnDestroy } from '@angular/core';
import { Observable, Subject, takeUntil } from 'rxjs';
import { ServicoConfiguracaoTenant } from '../../services/tenant-config.service';
import { AsyncPipe, CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { ConfiguracaoEmpresa } from '../../models/service.models';
import { AzureBlobService } from '../../services/azure-blob.service';
import { NotificacaoToastrService } from '../../../../shared/components/notificacao/notificacao-toastr.service';

@Component({
  selector: 'app-banner-configuracao',
  standalone: true,
  imports: [
    MatIconModule,
    MatCardModule,
    MatSlideToggleModule,
    AsyncPipe,
    MatInputModule,
    MatFormFieldModule,
    ReactiveFormsModule,
    CommonModule,
    MatButtonModule,
  ],
  templateUrl: './banner-configuracao.component.html',
  styleUrl: './banner-configuracao.component.scss'
})
export class BannerConfiguracaoComponent implements OnInit, OnDestroy {
  private destroy$ = new Subject<void>();

  configuracao: ConfiguracaoEmpresa | null = null;
  bannerUrl$: Observable<string> | null = null;
  bannerPreview: string | null = null;
  bannerFile: File | null = null;
  uploadandoBanner = false;

  constructor(
    private azureBlobService: AzureBlobService,
    private servicoTenant: ServicoConfiguracaoTenant,
    private toastr: NotificacaoToastrService

  ) { }

  ngOnInit(): void {
    this.carregarBannerAtual();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private carregarBannerAtual(): void {
    this.servicoTenant.obterConfiguracao()
      .pipe(takeUntil(this.destroy$))
      .subscribe(config => {
        if (config?.bannerUrl) {
          this.bannerUrl$ = new Observable(observer => {
            observer.next(config.bannerUrl!);
            observer.complete();
          });
        }
      });
  }

  selecionarBanner(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (!input.files?.length) return;

    const arquivo = input.files[0];
    if (!this.validarArquivo(arquivo)) return;

    this.bannerFile = arquivo;

    const reader = new FileReader();
    reader.onload = e => this.bannerPreview = e.target?.result as string;
    reader.readAsDataURL(arquivo);
  }

  uploadBanner(): void {
    if (!this.bannerFile) return;

    this.uploadandoBanner = true;

    this.azureBlobService.uploadBanner(this.bannerFile)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: async (config) => {
          this.configuracao = config;
          await this.servicoTenant.recarregarConfiguracao();


          this.bannerPreview = null;
          this.bannerFile = null;
          this.uploadandoBanner = false;

          this.toastr.sucesso('Banner atualizado com sucesso');
        },
        error: () => {
          this.uploadandoBanner = false;
          this.toastr.erro('Erro ao enviar banner');
        }
      });
  }

  cancelarBanner(): void {
    this.bannerFile = null;
    this.bannerPreview = null;
  }

  private validarArquivo(arquivo: File): boolean {
    const tiposPermitidos = ['image/jpeg', 'image/jpg', 'image/png'];

    if (!tiposPermitidos.includes(arquivo.type)) {
      this.toastr.aviso('Apenas arquivos JPG, JPEG e PNG são permitidos');
      return false;
    }

    if (arquivo.size > 5 * 1024 * 1024) {
      this.toastr.aviso('O arquivo deve ter no máximo 5MB');
      return false;
    }

    return true;
  }
}
