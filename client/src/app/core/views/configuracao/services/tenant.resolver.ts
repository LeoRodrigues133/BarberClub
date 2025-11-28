import { ResolveFn } from '@angular/router';
import { SelecionarPorId } from '../../funcionario/models/funcionario.models';
import { inject } from '@angular/core';
import { UserService } from '../../../../auth/services/user.service';
import { FuncionarioService } from '../../funcionario/services/funcionario.service';
import { switchMap, take } from 'rxjs';

export const tenantResolver: ResolveFn<SelecionarPorId | null> = () => {
  const userService = inject(UserService);
  const funcionarioService = inject(FuncionarioService)

  return userService.usuarioAutenticado.pipe(
    take(1),
    switchMap(usuario => {
      if (!usuario || !usuario.funcionarioId)
        return [null];

      return funcionarioService.SelecionarPorId(usuario.funcionarioId);
    })
  );
};
