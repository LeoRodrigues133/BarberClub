import { Injectable } from '@angular/core';
import { UserService } from '../../auth/services/user.service';
import { Permission, Role, ROLE_PERMISSIONS } from '../constants/permissions';
import { map, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PermissionService {

  constructor(private userService: UserService) { }

  hasPermission(permission: Permission): Observable<boolean> {
    return this.userService.usuarioAutenticado.pipe(
      map(usuario => {
        if (!usuario || usuario.role === undefined)
          return false;


        const usuarioRole = usuario.role as Role;
        const usuarioPermission = ROLE_PERMISSIONS[usuarioRole] || [];

        return usuarioPermission.includes(permission);
      })
    );
  }

  hasAnyPermission(permissions: Permission[]): Observable<boolean> {
    return this.userService.usuarioAutenticado.pipe(
      map(usuario => {
        if (!usuario || usuario.role === undefined)
          return false;

        const usuarioRole = usuario.role as Role;
        const usuarioPermission = ROLE_PERMISSIONS[usuarioRole] || [];

        return permissions.some(permission => usuarioPermission.includes(permission));
      })
    );
  }

  hasAllPermission(permissions: Permission[]): Observable<boolean> {
    return this.userService.usuarioAutenticado.pipe(
      map(usuario => {
        if (!usuario || usuario.role === undefined)
          return false;

        const usuarioRole = usuario.role as Role;
        const usuarioPermission = ROLE_PERMISSIONS[usuarioRole] || [];

        return permissions.every(permission => usuarioPermission.includes(permission));
      })
    )
  }
}
