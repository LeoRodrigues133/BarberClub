import { Component, EventEmitter, inject, Input, OnInit, Output } from '@angular/core';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { AsyncPipe, NgIf, NgForOf } from '@angular/common';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { Observable } from 'rxjs';
import { map, shareReplay } from 'rxjs/operators';
import { UsuarioAutenticadoDto } from '../../auth/models/auth.models';
import { Title } from '@angular/platform-browser';
import { RouterLink } from '@angular/router';
import { ServicoConfiguracaoTenant} from '../views/configuracao/services/tenant-config.service';

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

  constructor(
    private title: Title,
    private tenantConfigService: ServicoConfiguracaoTenant
  ) {
    this.logout = new EventEmitter();
  }

  ngOnInit() {
    if (this.usuarioAutenticado) {
      this.title.setTitle(`BarberClub | ${this.usuarioAutenticado.userName}`);
    }

    this.tenantConfigService.obterConfiguracao().subscribe();
  }

  linksPublicos: LinkNavegacao[] = [
    {
      titulo: 'Início',
      icone: 'home',
      rota: '/dashboard'
    },
    {
      titulo: 'Login',
      icone: 'login',
      rota: '/login'
    }
  ];
  linksAutenticados: LinkNavegacao[] = [
    {
      titulo: 'Início',
      icone: 'home',
      rota: '/dashboard'
    },
    {
      titulo: 'Funcionarios',
      icone: 'people',
      rota: '/employees'
    },
    {
      titulo: 'Gerenciar',
      icone: 'settings',
      rota: '/settings'
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
}
