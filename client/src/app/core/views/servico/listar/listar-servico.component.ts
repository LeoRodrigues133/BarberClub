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
import { FuncionarioComServicos, SelecionarPorId } from '../../funcionario/models/funcionario.models';
import { MatExpansionModule } from '@angular/material/expansion'
import { MatTableDataSource, MatTableModule } from '@angular/material/table'
import { CargoPipe } from '../../../shared/pipes/cargo.pipe';

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
    MatExpansionModule,
    MatInputModule,
    FormsModule,
    CommonModule,
    RouterLink,
    MatButtonModule,
    MatListModule,
    MatTableModule,
    HasPermissionDirective,
    CargoPipe
  ],
  templateUrl: './listar-servico.component.html',
  styleUrl: './listar-servico.component.scss'
})
export class ListarServicoComponent implements OnInit {
  Permission = Permission
  funcionariosComServicos: FuncionarioComServicos[] = [];

  mockAvatarProvisorio: string =
  'https://media-gru2-1.cdn.whatsapp.net/v/t61.24694-24/457745570_1361301841514474_4871248558438884836_n.jpg?ccb=11-4&oh=01_Q5Aa3AHMhJIZfBstkgzFcYE9zc9yITIn3_ETh9Zn3k0vacSqWQ&oe=693A2E01&_nc_sid=5e03e0&_nc_cat=100'

  dataS:Record<string, MatTableDataSource<SelecionarServicosRequest>>= {};

  constructor(private route: ActivatedRoute) {
  }

  ngOnInit(): void {
   const funcionarios = this.route.snapshot.data['funcionarios'];
    const servicos = this.route.snapshot.data['servicos'];

    this.funcionariosComServicos = this.agruparServicosPorFuncionario(funcionarios, servicos);
  }

  private agruparServicosPorFuncionario(
    funcionarios: SelecionarPorId[],
    servicos: SelecionarServicosRequest[]
  ): FuncionarioComServicos[] {
    const servicosPorFuncionario = servicos.reduce((acc, servico) => {
      if (!acc[servico.funcionarioId]) {
        acc[servico.funcionarioId] = [];
      }
      acc[servico.funcionarioId].push(servico);
      return acc;
    }, {} as Record<string, SelecionarServicosRequest[]>);

    return funcionarios.map(funcionario => ({
      funcionario,
      servicos: servicosPorFuncionario[funcionario.id] || []
    }));
  }
}
