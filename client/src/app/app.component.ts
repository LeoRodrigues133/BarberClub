import { Component } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { AuthService } from './auth/services/auth.service';
import { LocalStorageService } from './auth/services/local-storage.service';
import { UserService } from './auth/services/user.service';
import { Observable } from 'rxjs';
import { UsuarioAutenticadoDto } from './auth/models/auth.models';
import { ShellComponent } from "./core/shell/shell.component";
import { AsyncPipe } from '@angular/common';

@Component({
  selector: 'app-root',
  imports: [
    RouterOutlet,
    ShellComponent,
    AsyncPipe],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'BarberClub App';

  usuarioAutenticado$?: Observable<UsuarioAutenticadoDto | undefined>;

  constructor(
    private router: Router,
    private authService: AuthService,
    private userService: UserService,
    private localStorageService: LocalStorageService
  ) { }

  ngOnInit(): void {
    this.usuarioAutenticado$ = this.userService.usuarioAutenticado;

    const token = this.localStorageService.obterTokenAutenticacao();

    if (!token) return;

    const usuarioPersistido = token.usuario;
    const dataExpiracaoToken = new Date(token.expiracaoToken);

    const tokenValido: boolean =
      this.authService.validarExpiracaoToken(dataExpiracaoToken);

    if (usuarioPersistido && tokenValido) {
      this.userService.logarUsuario(usuarioPersistido);
    } else {
      this.efetuarLogout();
    }
  }

  efetuarLogout() {
    this.userService.logout();
    this.authService.logout();
    this.localStorageService.limparDadosLocais();


    this.router.navigate(['/login']);
  }

}
