import { NgIf } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { AuthService } from '../../services/auth.service';
import { UserService } from '../../services/user.service';
import { LocalStorageService } from '../../services/local-storage.service';
import { AutenticarUsuarioRequest, TokenResponse } from '../../models/auth.models';
import { ServicoConfiguracaoTenant } from '../../../core/views/configuracao/services/tenant-config.service';

@Component({
  selector: 'app-login',
  imports: [
    NgIf,
    RouterLink,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    MatButtonModule,
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  form: FormGroup;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private authService: AuthService,
    private usuarioService: UserService,
    private localStorage: LocalStorageService,
    private tenantService: ServicoConfiguracaoTenant
  ) {
    this.form = this.fb.group({
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

  public entrar() {
    if (this.form.invalid) {
      alert('Usuário ou senha inválidos'); // Corrigir para um toastr dps

      return;
    };

    const loginUsuario: AutenticarUsuarioRequest = this.form.value;

    this.authService.login(loginUsuario).subscribe({
      next: (res: TokenResponse) => this.processarSucesso(res),
      error: (erro: Error) => this.processarFalha(erro),
    });
  }

  private processarSucesso(res: TokenResponse) {

  this.localStorage.salvarTokenAutenticacao(res);
  this.usuarioService.logarUsuario(res.usuario);

  this.tenantService.carregarPorAutenticacao().subscribe({
    next: (config) => {
      this.router.navigate(['/dashboard']);
    },
    error: (erro) => {
      this.router.navigate(['/dashboard']);
    }
  });

}

  private processarFalha(erro: Error) {
    alert(erro.message);

  }
}
