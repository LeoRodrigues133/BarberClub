import { NgIf } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { LocalStorageService } from '../../services/local-storage.service';
import { RegistrarUsuarioRequest, TokenResponse } from '../../models/auth.models';
import { UserService } from '../../services/user.service';
import { VerificarCadeiaSenha } from '../../../core/shared/validators/senha.validators';
import { ServicoConfiguracaoTenant } from '../../../core/views/configuracao/services/tenant-config.service';
import { NotificacaoToastrService } from '../../../core/shared/components/notificacao/notificacao-toastr.service';
import { MatCardModule } from '@angular/material/card';

@Component({
  selector: 'app-registro',
  imports: [
    NgIf,
    RouterLink,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatCardModule
  ],
  templateUrl: './registro.component.html',
  styleUrl: './registro.component.scss'
})
export class RegistroComponent {
  form: FormGroup;

  constructor(
    private router: Router,
    private formBuilder: FormBuilder,
    private authService: AuthService,
    private userService: UserService,
    private localStorage: LocalStorageService,
    private tenantService: ServicoConfiguracaoTenant,
    private toastr: NotificacaoToastrService
  ) {
    this.form = this.formBuilder.group({
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
          VerificarCadeiaSenha()
        ],
      ],
      email: [
        '',
        [
          Validators.required,
          Validators.email
        ],
      ],
      nomeApresentacao: [
        '',
        [
          Validators.required,
          Validators.minLength(3),
          Validators.maxLength(100)
        ]
      ],
      nomeEmpresa: [
        '',
        [
          Validators.required,
          Validators.minLength(3),
          Validators.maxLength(100)
        ]
      ]
    });
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

  get nomeEmpresa() {
    return this.form.get('nomeEmpresa');
  }

  get nomeApresentacao() {
    return this.form.get('nomeApresentacao');
  }

  public registrar() {
    if (this.form.invalid) {
      this.toastr.erro('Não foi possível finalizar o cadastro.');
      return;
    }

    const registro: RegistrarUsuarioRequest = this.form.value;

    console.log(registro);
    this.authService.registrar(registro).subscribe({
      next: (res: TokenResponse) => this.processarSucesso(res),
      error: (erro) => this.processarFalha(erro),
    });

  }
  processarSucesso(res: TokenResponse) {
    this.userService.logarUsuario(res.usuario);
    this.localStorage.salvarTokenAutenticacao(res);

    this.tenantService.carregarPorAutenticacao().subscribe({
      next: () => this.router.navigate(['/dashboard']),
      error: () => this.router.navigate(['/registrar'])

    });

  }

  processarFalha(erro: any) {
    this.toastr.erro('Erro ao efetuar cadastro.');
    this.toastr.aviso(erro);
    this.localStorage.limparDadosLocais();
  }
}
