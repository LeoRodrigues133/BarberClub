import { NgForOf } from '@angular/common';
import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatListModule } from "@angular/material/list";

interface Servico {
  nome: string;
  icone: string;
}

interface Comodidade {
  nome: string;
  icone: string;
}

@Component({
  selector: 'app-dashboard',
  imports: [
    NgForOf,
    MatButtonModule,
    MatIconModule,
    MatCardModule,
    MatFormFieldModule,
    MatListModule,
],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent {
  servicos: Servico[] = [
    { nome: 'Corte Masculino', icone: 'content_cut' },
    { nome: 'Barba', icone: 'face' },
    { nome: 'Corte + Barba', icone: 'checkroom' },
    { nome: 'Corte Feminino', icone: 'content_cut' },
    { nome: 'Coloração', icone: 'palette' },
    { nome: 'Hidratação', icone: 'water_drop' },
    { nome: 'Escova', icone: 'air' },
    { nome: 'Manicure', icone: 'back_hand' }
  ];

  comodidades: Comodidade[] = [
    { nome: 'Wi-Fi Gratuito', icone: 'wifi' },
    { nome: 'Estacionamento', icone: 'local_parking' },
    { nome: 'Café e Água', icone: 'local_cafe' },
    { nome: 'TV e Entretenimento', icone: 'tv' },
    { nome: 'Ar Condicionado', icone: 'ac_unit' },
    { nome: 'Produtos Premium', icone: 'star' }
  ];

}
