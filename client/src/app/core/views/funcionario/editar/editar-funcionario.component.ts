import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { Cargo, CARGOS_OPTIONS } from '../cadastro/cadastrar-funcionario.component';
import { ActivatedRoute, Router } from '@angular/router';
import { VerificarCadeiaCpf } from '../../../shared/validators/cpf.validators';
import { VerificarCadeiaSenha } from '../../../shared/validators/senha.validators';
import { FuncionarioService } from '../services/funcionario.service';
import { CpfInputComponent } from "../../../shared/components/cpfInput/cpf-input.component";
import { EditarFuncionario } from '../models/funcionario.models';
import { NotificacaoToastrService } from '../../../shared/components/notificacao/notificacao-toastr.service';

@Component({
  selector: 'app-editar-funcionario',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatIconModule,
    MatCardModule,
    CpfInputComponent
  ],
  templateUrl: './editar-funcionario.component.html',
  styleUrl: './editar-funcionario.component.scss'
})
export class EditarFuncionarioComponent implements OnInit {
  form: FormGroup;
  cargosOptions = CARGOS_OPTIONS;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
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
      cpf: [
        '',
        Validators.required
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
          Validators.required,
          Validators.minLength(6),
          Validators.maxLength(30),
          VerificarCadeiaSenha(),
        ],
      ],
      email: [
        '',
        [
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
  ngOnInit(): void {
    const funcionario = this.route.snapshot.data['funcionario'];

    this.form.patchValue(funcionario);
  }

  public editar() {
    if (this.form.invalid) return;

    const id = this.route.snapshot.params['id'];

    const request = this.form.value as EditarFuncionario;

    this.funcionarioService.Editar(id, request).subscribe({
      next: (funcionarioEditado) => this.processarSucesso(),
      error: (erro) => this.processarFalha(erro)
    })
  }

  private processarSucesso(): void {
    this.toastr.sucesso(`Funcionario editado(a) com sucesso!`);

    this.router.navigate(['/employees', 'listar']);
  }

  private processarFalha(erro: Error): any {
    this.toastr.erro(erro.message);
  }


  get nome() {
    return this.form.get('nome');
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
}
