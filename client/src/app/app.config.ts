import { APP_INITIALIZER, ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { provideAuthentication } from './auth/auth.provider';
import { provideClientHydration, withEventReplay } from '@angular/platform-browser';
import { ServicoConfiguracaoTenant } from './core/views/configuracao/services/tenant-config.service';
import { inicializarTenant } from './core/views/configuracao/services/inicializarTenant.service';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient(withInterceptorsFromDi()),
    provideAuthentication(),
    provideClientHydration(withEventReplay()),
    {
      provide: APP_INITIALIZER,
      useFactory: inicializarTenant,
      deps: [ServicoConfiguracaoTenant],
      multi: true
    }
  ]
};
