import { Component, EventEmitter, inject, Input, OnInit, Output } from '@angular/core';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { AsyncPipe, NgIf, NgForOf } from '@angular/common';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { combineLatest, Observable } from 'rxjs';
import { map, shareReplay } from 'rxjs/operators';
import { UsuarioAutenticadoDto } from '../../auth/models/auth.models';
import { Title } from '@angular/platform-browser';
import { RouterLink } from '@angular/router';
import { ServicoConfiguracaoTenant } from '../views/configuracao/services/tenant-config.service';
import { Permission } from '../../tenant/constants/permissions';
import { PermissionService } from '../../tenant/services/permission.service';

@Component({
  selector: 'app-shell',
  templateUrl: './shell.component.html',
  styleUrl: './shell.component.scss',
  imports: [
    RouterLink,
    MatTooltipModule,
    MatToolbarModule,
    MatButtonModule,
    MatSidenavModule,
    MatListModule,
    MatIconModule,
    AsyncPipe,
    NgIf,
    NgForOf,
  ]
})
export class ShellComponent implements OnInit {
  @Input() usuarioAutenticado: UsuarioAutenticadoDto | undefined;
  @Output() logout: EventEmitter<void>;

  visibleRoutesPublicos$!: Observable<LinkNavegacao[]>;
  visibleRoutesAutenticados$!: Observable<LinkNavegacao[]>;


  constructor(
    private title: Title,
    private tenantConfigService: ServicoConfiguracaoTenant,
    private permissionService: PermissionService
  ) {
    this.logout = new EventEmitter();
  }

  ngOnInit() {
    if (this.usuarioAutenticado) {
      this.title.setTitle(`BarberClub | ${this.usuarioAutenticado.userName}`);
    }

    this.tenantConfigService.obterConfiguracao().subscribe();

    this.visibleRoutesPublicos$ = this.filtrarLinksPorPermissao(this.linksPublicos);

    // Filtra links autenticados
    this.visibleRoutesAutenticados$ = this.filtrarLinksPorPermissao(this.linksAutenticados);
  }

private filtrarLinksPorPermissao(links: LinkNavegacao[]): Observable<LinkNavegacao[]> {
    const permissionChecks$ = links.map(item =>
      this.permissionService.hasPermission(item.permission).pipe(
        map(hasPermission => ({ item, hasPermission }))
      )
    );

    return combineLatest(permissionChecks$).pipe(
      map(results =>
        results
          .filter(result => result.hasPermission)
          .map(result => result.item)
      )
    );
  }

  linksPublicos: LinkNavegacao[] = [
    {
      titulo: 'Início',
      icone: 'home',
      rota: '/dashboard',
      permission: Permission.VIEW_HOME
    },
    {
      titulo: 'Login',
      icone: 'login',
      rota: '/login',
      permission: Permission.VIEW_AUTH
    }
  ];
  linksAutenticados: LinkNavegacao[] = [
    {
      titulo: 'Início',
      icone: 'home',
      rota: '/dashboard',
      permission: Permission.VIEW_HOME
    },
    {
      titulo: 'Funcionarios',
      icone: 'people',
      rota: '/employees',
      permission: Permission.VIEW_EMPLOYEES
    },
    {
      titulo: 'Servicos',
      icone: 'work',
      rota: '/services',
      permission: Permission.VIEW_SERVICES
    },
    {
      titulo: 'Gerenciar',
      icone: 'settings',
      rota: '/settings',
      permission: Permission.VIEW_SETTINGS
    }
  ];

  get nomeEmpresa(): string {
    return this.tenantConfigService.nomeEmpresa;
  }

  get bannerUrl(): string | null {
    return this.tenantConfigService.urlBanner;
  }

  get logoUrl(): string | null {
    return this.tenantConfigService.urlLogo;
  }

  logoutEfetuado() {
    this.title.setTitle('BarberClub');
    this.tenantConfigService.limparConfiguracao();
    this.logout.emit();
  }

  private breakpointObserver = inject(BreakpointObserver);
  isHandset$: Observable<boolean> = this.breakpointObserver.observe(Breakpoints.Handset)
    .pipe(
      map(result => result.matches),
      shareReplay()
    );
}

export interface LinkNavegacao {
  titulo: string;
  icone: string;
  rota: string;
  permission: Permission;
}
