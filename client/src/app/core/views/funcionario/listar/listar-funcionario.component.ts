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
import { MatFormFieldModule } from "@angular/material/form-field";
import { CargoPipe } from '../../../shared/pipes/cargo.pipe';
@Component({
  selector: 'app-listar-funcionario',
  imports: [
    CommonModule,
    RouterLink,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatDividerModule,
    HasPermissionDirective,
    MatFormFieldModule,
    CargoPipe
],
  templateUrl: './listar-funcionario.component.html',
  styleUrl: './listar-funcionario.component.scss'
})
export class ListarFuncionarioComponent implements OnInit {
  funcionarios: ListagemFuncionario[] = [];
  Permission = Permission;
  mockAvatarProvisorio: string =
  'https://media-gru2-1.cdn.whatsapp.net/v/t61.24694-24/457745570_1361301841514474_4871248558438884836_n.jpg?ccb=11-4&oh=01_Q5Aa3AHMhJIZfBstkgzFcYE9zc9yITIn3_ETh9Zn3k0vacSqWQ&oe=693A2E01&_nc_sid=5e03e0&_nc_cat=100'


  constructor(private route: ActivatedRoute) { }

  ngOnInit(): void {

    this.funcionarios = this.route.snapshot.data['funcionarios'];

  }
}
