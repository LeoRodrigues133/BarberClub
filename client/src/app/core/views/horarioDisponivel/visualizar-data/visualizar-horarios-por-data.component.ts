import { CommonModule } from '@angular/common';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router, ɵEmptyOutletComponent } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatMenuModule } from '@angular/material/menu';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { Subject, takeUntil } from 'rxjs';
import { HorarioService } from '../services/horario.service';
import { Permission } from '../../../../tenant/constants/permissions';
import { HasPermissionDirective } from '../../../../tenant/directives/has-permission.directive';
import { NotificacaoToastrService } from '../../../shared/components/notificacao/notificacao-toastr.service';
import { MatDividerModule } from '@angular/material/divider';

interface HorarioDto {
  horario: string;
  status: boolean;
  id?: string;
}

@Component({
  selector: 'app-visualizar-horarios-por-data',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatIconModule,
    MatButtonModule,
    MatMenuModule,
    MatTooltipModule,
    MatProgressSpinnerModule,
    HasPermissionDirective,
    MatDividerModule,
    ɵEmptyOutletComponent
],
  templateUrl: './visualizar-horarios-por-data.component.html',
  styleUrl: './visualizar-horarios-por-data.component.scss'
})
export class VisualizarHorariosPorDataComponent implements OnInit, OnDestroy {

  horarios: HorarioDto[] = [];
  carregando = false;
  dataFormatada = '';
  Permission = Permission;

  private readonly destroy$ = new Subject<void>();
  private funcionarioId = '';
  private dataSelecionada = '';

  constructor(
    private readonly route: ActivatedRoute,
    private readonly router: Router,
    private readonly horarioService: HorarioService,
    private toastr: NotificacaoToastrService
  ) {}

  ngOnInit(): void {
    this.obterParametros();
    this.carregarHorarios();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  public voltar(): void {
    this.router.navigate(['/settings']);
  }

  public desativarHorario(horario: HorarioDto): void {
    if (!horario.id) {
      this.toastr.erro('ID do horário não encontrado');
      return;
    }

    this.horarioService
      .DesativarHorario(horario.id)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: () => {
          this.toastr.sucesso('Horário desativado com sucesso!');
          horario.status = false;
        },
        error: (erro) => this.toastr.erro(erro.message)
      });
  }

  public ativarHorario(horario: HorarioDto): void {
    if (!horario.id) {
      this.toastr.erro('ID do horário não encontrado');
      return;
    }

    this.horarioService
      .AtivarHorario(horario.id)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: () => {
          this.toastr.sucesso('Horário ativado com sucesso!');
          horario.status = true;
        },
        error: (erro) => this.toastr.erro(erro.message)
      });
  }

  public contarDisponiveis(): number {
    return this.horarios.filter(h => h.status).length;
  }

  public contarIndisponiveis(): number {
    return this.horarios.filter(h => !h.status).length;
  }

  private obterParametros(): void {
    this.funcionarioId = this.route.snapshot.paramMap.get('funcionarioId') || '';
    this.dataSelecionada = this.route.snapshot.queryParamMap.get('data') || '';
      console.log(this.dataSelecionada)
      console.log(this.dataFormatada + 'dataForm antes')

    if (this.dataSelecionada) {
      this.dataFormatada = this.formatarDataParaExibicao(this.dataSelecionada);
      console.log(this.dataFormatada + 'dataForm depois')

    }
  }

  private carregarHorarios(): void {
    const horariosDoResolver = this.route.snapshot.data['horarios'];

    if (horariosDoResolver) {
      this.horarios = horariosDoResolver;
      return;
    }
    this.carregando = true;

    this.horarioService
      .SelecionarHorarioPorData(this.funcionarioId, this.dataSelecionada)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (dados) => {
          this.horarios = dados;
          this.carregando = false;
        },
        error: (erro) => {
          this.toastr.erro(erro.message);
          this.carregando = false;
        }
      });
  }

  private formatarDataParaExibicao(dataISO: string): string {
    try {
    const [ano, mes, dia] = dataISO.split('T')[0].split('-');

    const data = new Date(parseInt(ano), parseInt(mes) - 1, parseInt(dia));

      return data.toLocaleDateString('pt-BR', {
        weekday: 'long',
        year: 'numeric',
        month: 'long',
        day: 'numeric',

      });

    } catch {
      return dataISO;
    }
  }
}
