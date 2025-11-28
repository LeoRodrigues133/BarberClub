import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { PermissionService } from '../services/permission.service';
import { Permission } from '../constants/permissions';
import { map, take } from 'rxjs';
import { NotificacaoToastrService } from '../../core/shared/components/notificacao/notificacao-toastr.service';

export const permissionGuard: CanActivateFn = (route, state) => {
  const permissionService = inject(PermissionService);
  const router = inject(Router);
  const toastr = inject(NotificacaoToastrService)

  const requiredPermission = route.data['permission'] as Permission;

  return permissionService.hasPermission(requiredPermission).pipe(
    take(1),
    map(hasPermission => {
      if (hasPermission)
        return true;

      toastr.aviso(`Acesso negado. Permissão necessária: ${requiredPermission}`);
      if (hasPermission)
        router.navigate(['/dashboard']);
      else
        router.navigate(['/settings']);

      return false;
    })
  );
};
