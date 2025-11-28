import { Component } from '@angular/core';
import { ServicoService } from '../services/servico.service';
import { SelecionarServicoRequest } from '../models/servico.models';
import { Router, ActivatedRoute, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { CommonModule, DecimalPipe } from '@angular/common';
import { NotificacaoToastrService } from '../../../shared/components/notificacao/notificacao-toastr.service';

@Component({
  selector: 'app-excluir-servico',
  imports: [
    RouterLink,
    MatCardModule,
    MatIconModule,
    MatButtonModule,
    MatTooltipModule,
    MatDividerModule,
    MatCheckboxModule,
    FormsModule,
    DecimalPipe,
    CommonModule
  ],
  templateUrl: './excluir-servico.component.html',
  styleUrl: './excluir-servico.component.scss'
})
export class ExcluirServicoComponent {
  servico!: SelecionarServicoRequest;
  confirmacaoExclusao = false;


  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private servicoService: ServicoService,
    private toastr: NotificacaoToastrService
  ) { }

  ngOnInit(): void {
    this.servico = this.route.snapshot.data['servico'];

  }

  public excluir() {
    if (!this.confirmacaoExclusao) return;

    this.servicoService.Excluir(this.servico.id).subscribe({
      next: () => this.processarSucesso(),
      error: (erro) => this.processarFalha(erro),
    });
  }


  private processarSucesso(): void {
    this.toastr.sucesso(`Serviço excluído com sucesso!`);

    this.router.navigate(['/services', 'listar']);
  }

  private processarFalha(erro: Error): any {
    this.toastr.erro(erro.message);
  }

}
