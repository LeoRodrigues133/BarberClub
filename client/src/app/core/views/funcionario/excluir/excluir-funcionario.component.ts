import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { FuncionarioService } from '../services/funcionario.service';
import { SelecionarPorId } from '../models/funcionario.models';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatListItemIcon } from '@angular/material/list';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatDividerModule } from '@angular/material/divider';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { FormsModule } from '@angular/forms';
import { NotificacaoToastrService } from '../../../shared/components/notificacao/notificacao-toastr.service';

@Component({
  selector: 'app-excluir-funcionario',
  imports: [
    RouterLink,
    MatCardModule,
    MatIconModule,
    MatButtonModule,
    MatTooltipModule,
    MatDividerModule,
    MatCheckboxModule,
    FormsModule,
    RouterLink
  ],
  templateUrl: './excluir-funcionario.component.html',
  styleUrl: './excluir-funcionario.component.scss'
})
export class ExcluirFuncionarioComponent implements OnInit {
  funcionario!: SelecionarPorId;
  confirmacaoExclusao = false;


  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private funcionarioService: FuncionarioService,
    private toastr: NotificacaoToastrService

  ) { }

  ngOnInit(): void {
    this.funcionario = this.route.snapshot.data['funcionario'];

  }

  public excluir() {
    if (!this.confirmacaoExclusao) return;

    this.funcionarioService.Excluir(this.funcionario.id).subscribe({
      next: () => this.processarSucesso(),
      error: (erro) => this.processarFalha(erro),
    });
  }


  private processarSucesso(): void {
    this.toastr.sucesso(`Funcionario excluído(a) com sucesso!`);

    this.router.navigate(['/employees', 'listar']);
  }

  private processarFalha(erro: Error): any {
    this.toastr.erro(erro.message);
  }

}
