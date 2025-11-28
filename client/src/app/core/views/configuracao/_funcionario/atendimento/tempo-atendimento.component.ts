import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { FuncionarioService } from '../../../funcionario/services/funcionario.service';
import { ActivatedRoute, Router } from '@angular/router';
import { SelecionarPorId } from '../../../funcionario/models/funcionario.models';
import { NotificacaoToastrService } from '../../../../shared/components/notificacao/notificacao-toastr.service';

@Component({
  selector: 'app-tempo-atendimento',
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
  templateUrl: './tempo-atendimento.component.html',
  styleUrl: './tempo-atendimento.component.scss'
})
export class TempoAtendimentoComponent implements OnInit {
  form: FormGroup;
  salvandoAtendimento = false;
  funcionario?:SelecionarPorId;
  constructor(
    private funcionarioService: FuncionarioService,
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router:Router,
    private toastr: NotificacaoToastrService
  ) {
    this.form = this.fb.group({
      tempoAtendimento: [
        '',
        [
          Validators.required,
          Validators.min(1)
        ]
      ],
      tempoIntervalo: [
        '',
        Validators.required
      ]
    })
  }
  ngOnInit(): void {
    this.funcionario = this.route.snapshot.data['funcionario']

    this.form.patchValue(this.funcionario!);
  }

  atualizarAtendimento() {
    if(this.form.invalid) return;

    const request = this.form.value;

    this.funcionarioService.AtualizarTempoAtendimento(this.funcionario!.id,request).subscribe({
      next: () => this.processarSucesso(),
      error: (erro) => this.processarFalha(erro)
    })
  }

  private processarSucesso(): void {
    this.toastr.sucesso(`Tempo de atendimento editado(a) com sucesso!`);

    this.router.navigate(['/settings'])
  }

  private processarFalha(erro: Error): any {
    this.toastr.erro(erro.message);
  }

  get tempoAtendimento() {
    return this.form.get('tempoAtendimento');
  }

  get tempoIntervalo() {
    return this.form.get('tempoIntervalo');
  }
}
