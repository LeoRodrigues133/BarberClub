import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { FuncionarioService } from '../services/funcionario.service';
import { VerificarCadeiaSenha } from '../../../shared/validators/senha.validators';
import { CadastrarFuncionario } from '../models/funcionario.models';
import { CommonModule } from '@angular/common';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { CpfInputComponent } from "../../../shared/components/cpfInput/cpf-input.component";
import { NotificacaoToastrService } from '../../../shared/components/notificacao/notificacao-toastr.service';

export enum Cargo {
  Administrador = 0,
  Funcionario = 1
}

export const CARGOS_OPTIONS = [
  { valor: Cargo.Administrador, label: 'Administrador' },
  { valor: Cargo.Funcionario, label: 'Funcionário' }
];

@Component({
  selector: 'app-cadastrar-funcionario',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatIconModule,
    MatCardModule,
    CpfInputComponent,
    RouterLink
  ],
  templateUrl: './cadastrar-funcionario.component.html',
  styleUrl: './cadastrar-funcionario.component.scss'
})
export class CadastrarFuncionarioComponent {
  form: FormGroup;
  cargosOptions = CARGOS_OPTIONS;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private funcionarioService: FuncionarioService,
    private toastr: NotificacaoToastrService

  ) {
    this.form = this.fb.group({
      nome: [
        '',
        [
          Validators.required,
          Validators.minLength(3),
          Validators.maxLength(50),
        ],
      ],
      nomeApresentacao: [
        '',
        [
          Validators.minLength(1),
          Validators.maxLength(20),
        ],
      ],
      cpf: [
        '',
      ],
      userName: [
        '',
        [
          Validators.required,
          Validators.minLength(3),
          Validators.maxLength(30),
        ],
      ],
      password: [
        '',
        [
          VerificarCadeiaSenha(),
        ],
      ],
      email: [
        '',
        [
          Validators.required,
          Validators.email
        ],
      ],
      cargo: [
        Cargo.Funcionario,
        [
          Validators.required
        ]
      ]
    })
  }

  public cadastrar() {
    if (this.form.invalid)
      return;

    const request = this.form.value as CadastrarFuncionario;

    this.funcionarioService.Cadastrar(request).subscribe({
      next: (registro) => this.processarSucesso(),
      error: (erro) => this.processarFalha(erro),
    });
  }

  get nome() {
    return this.form.get('nome');
  }
  get nomeApresentacao() {
    return this.form.get('nomeApresentacao');
  }
  get cpf() {
    return this.form.get('cpf');
  }
  get userName() {
    return this.form.get('userName');
  }
  get password() {
    return this.form.get('password');
  }
  get email() {
    return this.form.get('email');
  }
  get cargo() {
    return this.form.get('cargo');
  }

  private processarSucesso(): void {
    this.toastr.sucesso(`Funcionario cadastrado(a) com sucesso!`);

    this.router.navigate(['/employees', 'listar']);
  }

  private processarFalha(erro: Error): any {
    this.toastr.erro(erro.message);
  }

}

