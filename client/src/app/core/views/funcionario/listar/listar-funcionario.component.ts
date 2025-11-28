import { Component, OnInit } from '@angular/core';
import { ListagemFuncionario } from '../models/funcionario.models';

import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatDividerModule } from '@angular/material/divider';
import { HasPermissionDirective } from '../../../../tenant/directives/has-permission.directive';
import { Permission } from '../../../../tenant/constants/permissions';
@Component({
  selector: 'app-listar-funcionario',
  imports: [
    CommonModule,
    RouterLink,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatDividerModule,
    HasPermissionDirective
  ],
  templateUrl: './listar-funcionario.component.html',
  styleUrl: './listar-funcionario.component.scss'
})
export class ListarFuncionarioComponent implements OnInit {
  funcionarios: ListagemFuncionario[] = [];
  Permission = Permission;

  constructor(private route: ActivatedRoute) { }

  ngOnInit(): void {

    this.funcionarios = this.route.snapshot.data['funcionarios'];

  }
}
