import { inject } from '@angular/core';
import { CanMatchFn, Router, UrlTree } from '@angular/router';
import { Observable, map } from 'rxjs';
import { UserService } from '../services/user.service';

export const authUserGuard: CanMatchFn = (): Observable<boolean | UrlTree> => {
  const router = inject(Router);
  const userService = inject(UserService);

  return userService.usuarioAutenticado.pipe(
    map((user) => {

      if (user) return router.parseUrl('/dashboard');

      return true;
    })
  )
};
