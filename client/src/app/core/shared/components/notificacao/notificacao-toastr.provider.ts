import { EnvironmentProviders, makeEnvironmentProviders } from "@angular/core";
import { MAT_SNACK_BAR_DEFAULT_OPTIONS} from '@angular/material/snack-bar'
import { NotificacaoToastrService } from "./notificacao-toastr.service";

export const provideNotifications = (): EnvironmentProviders => {
  return makeEnvironmentProviders([
    {
      provide: MAT_SNACK_BAR_DEFAULT_OPTIONS,
      useValue: {
        duration: 3000,
        verticalPosition: 'bottom',
      },
    },

    NotificacaoToastrService,
  ]);
};
