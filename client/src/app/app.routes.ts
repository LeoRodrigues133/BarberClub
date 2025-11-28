import { inject } from '@angular/core';
import { Routes } from '@angular/router';
import { UserService } from './auth/services/user.service';
import { authGuard } from './auth/guards/auth.guard';
import { authUserGuard } from './auth/guards/auth-user.guard';
import { permissionGuard } from './tenant/guard/permission.guard';
import { Permission } from './tenant/constants/permissions';
import { DetalharFuncionarioResolver } from './core/views/funcionario/services/detalhar-funcionario.resolver';
import { tenantResolver } from './core/views/configuracao/services/tenant.resolver';

export const routes: Routes = [
  { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
  {
    path: 'dashboard',
    loadComponent: () =>
      import('./core/views/dashboard/dashboard.component').then(
        (c) => c.DashboardComponent
      ),
    resolve: { usuario: () => inject(UserService).usuarioAutenticado },
    canActivate: [permissionGuard],
    data: { permission: Permission.VIEW_HOME }

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
    canActivate: [permissionGuard],
    data: { permission: Permission.VIEW_SETTINGS },
    resolve:{
      funcionario: tenantResolver
    }
  },
  {
    path: 'employees',
    loadChildren: () =>
      import('./core/views/funcionario/funcionario.routes').then(
        (r) => r.funcionarioRoutes
      ),
    canMatch: [authGuard],
    canActivate: [permissionGuard],
    data: { permission: Permission.VIEW_EMPLOYEES }
  },
  {
    path: 'services',
    loadChildren: () =>
      import('./core/views/servico/servico.routes').then(
        (r) => r.servicoRoutes
      ),
    canMatch: [authGuard],
    canActivate: [permissionGuard],
    data: { permission: Permission.VIEW_SERVICES }
  },
];
