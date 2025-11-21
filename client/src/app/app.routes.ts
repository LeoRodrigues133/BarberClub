import { inject } from '@angular/core';
import { Routes } from '@angular/router';
import { UserService } from './auth/services/user.service';
import { authGuard } from './auth/guards/auth.guard';
import { authUserGuard } from './auth/guards/auth-user.guard';

export const routes: Routes = [
  { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
  {
    path: 'dashboard',
    loadComponent: () =>
      import('./core/views/dashboard/dashboard.component').then(
        (c) => c.DashboardComponent
      ),
    resolve: { usuario: () => inject(UserService).usuarioAutenticado },
  },
  {
    path: 'registrar',
    loadComponent: () =>
      import('./auth/views/registro/registro.component').then(
        (c) => c.RegistroComponent
      ),
    canMatch: [authUserGuard],

  },
  {
    path: 'login',
    loadComponent: () =>
      import('./auth/views/login/login.component').then(
        (c) => c.LoginComponent
      ),
    canMatch: [authUserGuard],
  },
  {
    path: 'settings',
    loadComponent: () =>
      import('./core/views/configuracao/configuracao.component').then(
        (c) => c.ConfiguracaoComponent
      ),
    canMatch: [authGuard],
  },
  {
    path: 'employees',
    loadChildren: () =>
      import('./core/views/funcionario/funcionario.routes').then(
        (r) => r.funcionarioRoutes
      ),
    canMatch: [authGuard],
  },
  {
    path: 'services',
    loadChildren: () =>
      import('./core/views/servico/servico.routes').then(
        (r) => r.servicoRoutes
      ),
    canMatch: [authGuard],
  },
];
