import { ValidatorFn, AbstractControl, ValidationErrors } from "@angular/forms";

export function VerificarCadeiaCpf(): ValidatorFn {

  return (constrol: AbstractControl): ValidationErrors | null => {

    const value = constrol.value;

    if (!value) return null;

    const estaFormatado = /^\d{3}\.\d{3}\.\d{3}\-\d{2}$/.test(value);
    const cpfVazio = value.length == 0;
    const cpfCurto = value.length < 14;


    const erros: ValidationErrors = {};

    if (!estaFormatado) erros['semFormatacao'] = true;
    if (cpfVazio) erros['cpfVazio'] = true;
    if (cpfCurto) erros['cpfCurto'] = true;

    return Object.keys(erros).length > 0 ? erros : null
  }
}

export function aplicarMascaraCPF(value: string): string {
  if (!value) return '';

  value = value.replace(/\D/g, '');

  value = value.substring(0, 11);

  if (value.length > 10) {
    return value.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, '$1.$2.$3-$4');
  } else if (value.length > 6) {
    return value.replace(/(\d{3})(\d{3})(\d{1,3})/, '$1.$2.$3');
  } else if (value.length > 3) {
    return value.replace(/(\d{3})(\d{1,3})/, '$1.$2');
  }

  return value;
}
