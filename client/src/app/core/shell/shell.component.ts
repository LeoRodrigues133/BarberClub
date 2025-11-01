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

  constructor(private title: Title) {
    this.logout = new EventEmitter();
  }

  ngOnInit() {
    if (this.usuarioAutenticado) {
      this.title.setTitle(`BarberClub | ${this.usuarioAutenticado.userName}`);
    }
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
      titulo: 'Gerenciar',
      icone: 'settings',
      rota: '/settings'
    }
  ];

  foto: string = 'https://media.istockphoto.com/id/1007175486/pt/foto/modern-empty-barbershop-interior-with-chairs-mirrors-and-lamps.webp?s=1024x1024&w=is&k=20&c=MXcGa1SVLLGphHt3gMLAbAU2O8EThiDIIy42UaRbL7A='


  logoutEfetuado() {
    this.title.setTitle('BarberClub');
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
