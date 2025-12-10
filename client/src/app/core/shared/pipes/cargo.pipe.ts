import { Pipe, PipeTransform } from '@angular/core';

export enum Cargo {
  Administrador = 0,
  Funcionario = 1
}

@Pipe({
  name: 'cargo',
  standalone: true
})
export class CargoPipe implements PipeTransform {
  private cargoLabels: { [key: number]: string } = {
    [Cargo.Administrador]: 'Administrador',
    [Cargo.Funcionario]: 'Funcionário'
  };

  transform(value: number | null | undefined): string {
    if (value === null || value === undefined) {
      return 'Não definido';
    }

    return this.cargoLabels[value] || 'Cargo desconhecido';
  }
}
