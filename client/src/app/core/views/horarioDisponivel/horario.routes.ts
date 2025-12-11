import { Routes } from '@angular/router';
import { horarioResolver } from './services/horario.resolver';
import { permissionGuard } from '../../../tenant/guard/permission.guard';
import { Permission } from '../../../tenant/constants/permissions';

export const horarioRoutes: Routes = [
  {
    path: 'visualizar-por-data/:funcionarioId',
    loadComponent: () =>
      import('./visualizar-data/visualizar-horarios-por-data.component').then(
        (c) => c.VisualizarHorariosPorDataComponent
      ),
    canActivate: [permissionGuard],
    data: { permission: Permission.VIEW_APPOINTMENTS },
    resolve: {
      horarios: horarioResolver
    }
  }
];
