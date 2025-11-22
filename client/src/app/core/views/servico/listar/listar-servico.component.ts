import { Component, OnInit } from '@angular/core';
import { SelecionarServicosRequest } from '../models/servico.models';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatSelectModule } from '@angular/material/select';
import { MatDividerModule } from '@angular/material/divider';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatListModule } from "@angular/material/list";
import { HasPermissionDirective } from "../../../../tenant/directives/has-permission.directive";
import { Permission } from '../../../../tenant/constants/permissions';

@Component({
  selector: 'app-listar-servico',
  imports: [
    MatCardModule,
    MatIconModule,
    MatChipsModule,
    MatSelectModule,
    MatDividerModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    FormsModule,
    CommonModule,
    RouterLink,
    MatButtonModule,
    MatListModule,
    HasPermissionDirective
],
  templateUrl: './listar-servico.component.html',
  styleUrl: './listar-servico.component.scss'
})
export class ListarServicoComponent implements OnInit {
  Servicos?: SelecionarServicosRequest[];
  carregando = false;
  termoPesquisa = '';
  categoriaFiltro: string | null = null;
  statusFiltro: boolean | null = null;
  Permission = Permission

  constructor(private route: ActivatedRoute) {
  }

  ngOnInit(): void {
    this.Servicos = this.route.snapshot.data['servicos']

    console.log(this.Servicos)
  }

}
