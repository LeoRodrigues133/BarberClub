import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ServicoService } from '../services/servico.service';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { Router } from '@angular/router';
import { NotificacaoToastrService } from '../../../shared/components/notificacao/notificacao-toastr.service';

@Component({
  selector: 'app-cadastrar-servico',
  imports: [
    MatIconModule,
    MatCardModule,
    MatFormFieldModule,
    MatSlideToggleModule,
    ReactiveFormsModule,
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatIconModule,
    MatCardModule,
  ],
  templateUrl: './cadastrar-servico.component.html',
  styleUrl: './cadastrar-servico.component.scss'
})
export class CadastrarServicoComponent {
  form: FormGroup

  constructor(
    private router: Router,
    private fb: FormBuilder,
    private servicoService: ServicoService,
    private toastr: NotificacaoToastrService

  ) {
    this.form = this.fb.group({
      titulo: ['',
        [
          Validators.required,
          Validators.minLength(3),
          Validators.maxLength(50)]
      ],
      valor: [
        '',
        [
          Validators.required,
          Validators.min(1)
        ]
      ],
      duracao: [
        '',
        [Validators.min(1)]
      ],
      isPromocao:
        [
          false
        ],
      porcentagemPromocao: [
        {
          value: '',
          disabled: true
        },
        [
          Validators.min(0),
          Validators.max(90)
        ]
      ]
    });

    this.setupPromocaoWatcher();
  }

  private setupPromocaoWatcher() {
    this.form.get('isPromocao')!.valueChanges.subscribe(isPromo => {
      const porcentagemField = this.form.get('porcentagemPromocao');

      if (isPromo) {
        porcentagemField?.enable();
        porcentagemField?.addValidators(Validators.required);
      } else {
        porcentagemField?.disable();
        porcentagemField?.removeValidators(Validators.required);
        porcentagemField?.setValue('');
      }

      porcentagemField?.updateValueAndValidity();
    });
  }

  public cadastrar() {
    if (this.form.invalid) return;

    const request = this.form.value;

    this.servicoService.Cadastrar(request).subscribe({
      next: (servicoCadastrado) => this.processarSucesso(),
      error: (erro) => this.processarFalha(erro)
    });

  }

  get titulo() {
    return this.form.get('titulo');
  }
  get valor() {
    return this.form.get('valor');
  }
  get duracao() {
    return this.form.get('duracao');
  }
  get isPromocao() {
    return this.form.get('isPromocao');
  }
  get porcentagemPromocao() {
    return this.form.get('porcentagemPromocao');
  }


  private processarSucesso(): void {
    this.toastr.sucesso(`Serviço cadastrado com sucesso!`);

    this.router.navigate(['/services', 'listar']);
  }

  private processarFalha(erro: Error): any {
    this.toastr.erro(erro.message);
  }
}
