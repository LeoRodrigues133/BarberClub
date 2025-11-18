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
import { VerificarCadeiaSenha } from '../../../core/shared/senha.validators';

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
    private localStorage: LocalStorageService
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
          Validators.required,
          Validators.minLength(6),
          Validators.maxLength(30),
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

  public registrar() {
    if (this.form.invalid) {
      alert('Não foi possível finalizar o cadastro.');
      return;
    }

    const registro: RegistrarUsuarioRequest = this.form.value;

    this.authService.registrar(registro).subscribe({
      next: (res: TokenResponse) => this.processarSucesso(res),
      error: (erro) => this.processarFalha(erro),
    });

  }
  processarSucesso(res: TokenResponse) {
    this.userService.logarUsuario(res.usuario);

    this.localStorage.salvarTokenAutenticacao(res);

    this.router.navigate(['/dashboard']);
  }

  processarFalha(erro: any) {
    alert('Erro ao efetuar cadastro.');

    this.localStorage.limparDadosLocais();
  }
}
