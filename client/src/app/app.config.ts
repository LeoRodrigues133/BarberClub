import { APP_INITIALIZER, ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { provideAuthentication } from './auth/auth.provider';
import { provideClientHydration, withEventReplay } from '@angular/platform-browser';
import { ServicoConfiguracaoTenant } from './core/views/configuracao/services/tenant-config.service';
import { inicializarTenant } from './core/views/configuracao/services/inicializarTenant.service';
import { provideInitializer } from './core/views/configuracao/services/provideInitializer.provider';
import { provideNotifications } from './core/shared/components/notificacao/notificacao-toastr.provider';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient(withInterceptorsFromDi()),
    provideAuthentication(),
    provideClientHydration(withEventReplay()),
    provideInitializer(),
    provideNotifications()
  ]
};
