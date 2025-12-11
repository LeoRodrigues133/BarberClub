import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AzureBlobService } from '../../services/azure-blob.service';
import { Subject, takeUntil } from 'rxjs';
import { NotificacaoToastrService } from '../../../../shared/components/notificacao/notificacao-toastr.service';
import { CalendarioComponent } from "../../../../shared/components/calendario/calendario.component";
import { FuncionarioService } from '../../../funcionario/services/funcionario.service';
import { ActivatedRoute } from '@angular/router';
import { CadastrarVariosHorariosRequest, CadastrarVariosHorariosResponse } from '../../../funcionario/models/funcionario.models';
import { HorarioService } from '../../../horarioDisponivel/services/horario.service';

@Component({
  selector: 'app-configuracao-horarios',
  standalone: true,
  imports: [
    CommonModule,
    CalendarioComponent
  ],
  templateUrl: './configuracao-horarios.component.html',
  styleUrls: ['./configuracao-horarios.component.scss']
})
export class ConfigurarHorariosComponent implements OnInit, OnDestroy {

  private destroy$ = new Subject<void>();
  carregando = true;

  configuracaoHorarios = {
    diasFechados: [] as number[],
    datasFechadasEspecificas: [] as string[],
  };

  constructor(
    private azureBlobService: AzureBlobService,
    private funcionarioService: FuncionarioService,
    private horarioService:HorarioService,
    private route: ActivatedRoute,
    private toastr: NotificacaoToastrService,
  ) { }

  ngOnInit(): void {
    this.carregarConfiguracaoHorarios();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  GerarHorarios(data: Date) {
    const mes = data.getMonth() + 1;
    const ano = data.getFullYear();

    const funcionario = this.route.snapshot.data['funcionario'];

    const request: CadastrarVariosHorariosRequest = { mes: mes, ano: ano }

    this.funcionarioService.GerarHorarios(funcionario.id, request).subscribe({
      next: (res:any) => this.processarSucesso(res)      ,
      error: (erro) => this.processarFalha(erro)
    })
  }

    VisualizarHorarios(data: Date) {
    const mes = data.getMonth() + 1;
    const ano = data.getFullYear();

    const funcionario = this.route.snapshot.data['funcionario'];

    const request: any = `${mes}-${ano}-10T00:00:00.000Z`

    this.horarioService.SelecionarHorarioPorData(funcionario.id, request).subscribe({
      next: (res:any) => this.processarSucesso(res)      ,
      error: (erro) => this.processarFalha(erro)
    })
  }


  private carregarConfiguracaoHorarios(): void {
    this.carregando = true;

    this.azureBlobService.obterConfiguracao()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (config) => {
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

          this.carregando = false;
        },
        error: (err) => {
          this.toastr.erro('Erro ao carregar configuração: ' + err);
          this.carregando = false;
        }
      });
  }

  private processarSucesso(response: CadastrarVariosHorariosResponse): void {
    if (!response) {
      this.toastr.erro('Resposta inválida do servidor');
      return;
    }

    const { qtHorariosGerados, qtHorariosRemovidos } = response;

    if (qtHorariosGerados === 0) {
      this.toastr.aviso('Nenhum horário foi gerado.');
      return;
    }

    let mensagem = `${qtHorariosGerados} horário(s) gerado(s) e ${qtHorariosRemovidos} removido(s) com sucesso!\n\n`;

    this.toastr.sucesso(mensagem);
  }

  private processarFalha(erro: Error): any {
    this.toastr.erro(erro.message);
  }
}
