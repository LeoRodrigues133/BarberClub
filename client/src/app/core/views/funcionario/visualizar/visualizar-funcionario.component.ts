import { Component, OnInit } from '@angular/core';
import { SelecionarPorId } from '../models/funcionario.models';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { FuncionarioService } from '../services/funcionario.service';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatChipsModule } from '@angular/material/chips'
import { CargoPipe } from '../../../shared/pipes/cargo.pipe';

@Component({
  selector: 'app-visualizar-funcionario',
  imports: [
    RouterLink,
    MatCardModule,
    MatIconModule,
    MatButtonModule,
    MatTooltipModule,
    MatDividerModule,
    MatCheckboxModule,
    FormsModule,
    MatCardModule,
    MatChipsModule,
    CargoPipe

  ],
  templateUrl: './visualizar-funcionario.component.html',
  styleUrl: './visualizar-funcionario.component.scss'
})
export class VisualizarFuncionarioComponent implements OnInit {
  funcionario?: SelecionarPorId;

  constructor(
    private route: ActivatedRoute,
    private funcionarioService: FuncionarioService
  ) { }
  ngOnInit(): void {
    this.funcionario = this.route.snapshot.data['funcionario'];
  }

}
