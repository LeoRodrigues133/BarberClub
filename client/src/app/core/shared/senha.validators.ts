import { AbstractControl, ValidationErrors, ValidatorFn } from "@angular/forms";

export function VerificarCadeiaSenha(): ValidatorFn {

  return (constrol: AbstractControl): ValidationErrors | null => {

    const value = constrol.value;

    if (!value) return null;

    const temCaracteresMaiusculo = /[A-Z]+/.test(value);
    const temCaracteresMinusculo = /[a-z]+/.test(value);
    const temCaracteresEspeciais = /[^A-Za-z0-9]+/.test(value);
    const temCaracteresNumericos = /[0-9]+/.test(value);

    const erros: ValidationErrors = {};

    if (!temCaracteresMaiusculo) erros['semMaiuscula'] = true;
    if (!temCaracteresMinusculo) erros['semMinuscula'] = true;
    if (!temCaracteresNumericos) erros['semNumero'] = true;
    if (!temCaracteresEspeciais) erros['semEspecial'] = true;

    return Object.keys(erros).length > 0 ? erros : null
  }
}
