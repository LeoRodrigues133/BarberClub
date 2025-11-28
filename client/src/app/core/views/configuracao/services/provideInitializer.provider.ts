import { APP_INITIALIZER, EnvironmentProviders, makeEnvironmentProviders } from "@angular/core";
import { inicializarTenant } from "./inicializarTenant.service";
import { ServicoConfiguracaoTenant } from "./tenant-config.service";

export const provideInitializer = (): EnvironmentProviders => {
  return makeEnvironmentProviders([
        {
      provide: APP_INITIALIZER,
      useFactory: inicializarTenant,
      deps: [ServicoConfiguracaoTenant],
      multi: true
    }
  ])
}
