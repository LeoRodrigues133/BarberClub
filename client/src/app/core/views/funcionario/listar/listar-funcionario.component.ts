import { Component, OnInit } from '@angular/core';
import { ListagemFuncionario } from '../models/funcionario.models';
import { NgModule } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { MatTableModule } from '@angular/material/table';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { FuncionarioService } from '../services/funcionario.service';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { AsyncPipe, CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatDividerModule } from '@angular/material/divider';
@Component({
  selector: 'app-listar-funcionario',
  imports: [
    CommonModule,
    RouterLink,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatDividerModule
  ],
  templateUrl: './listar-funcionario.component.html',
  styleUrl: './listar-funcionario.component.scss'
})
export class ListarFuncionarioComponent implements OnInit {
  funcionarios: ListagemFuncionario[] = [];

  constructor(private route: ActivatedRoute) { }

  ngOnInit(): void {

    this.funcionarios = this.route.snapshot.data['funcionarios'];

  }
}
