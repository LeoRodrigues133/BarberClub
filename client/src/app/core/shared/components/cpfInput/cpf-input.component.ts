// cpf-input.component.ts
import { Component, forwardRef } from '@angular/core';
import {
  AbstractControl,
  ControlValueAccessor,
  FormsModule,
  NG_VALIDATORS,
  NG_VALUE_ACCESSOR,
  ValidationErrors,
  Validator
} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { aplicarMascaraCPF, VerificarCadeiaCpf } from '../../validators/cpf.validators';

@Component({
  selector: 'app-cpf-input',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule
  ],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => CpfInputComponent),
      multi: true
    },
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => CpfInputComponent),
      multi: true
    }
  ],
  templateUrl: './cpf-input.component.html',
  styleUrl: './cpf-input.component.scss'
})
export class CpfInputComponent implements ControlValueAccessor, Validator {
  label: string = 'CPF';
  placeholder: string = '000.000.000-00';
  value: string = '';
  disabled: boolean = false;
  touched: boolean = false;

  private onChange: (value: string) => void = () => { };
  private onTouched: () => void = () => { };
  private onValidatorChange: () => void = () => { };

  // Instância do validator
  private cpfValidator = VerificarCadeiaCpf();

  writeValue(value: string): void {
    this.value = value ? aplicarMascaraCPF(value) : '';
  }

  registerOnChange(fn: (value: string) => void): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  validate(control: AbstractControl): ValidationErrors | null {
    // Usa o validator criado
    return this.cpfValidator(control);
  }

  registerOnValidatorChange(fn: () => void): void {
    this.onValidatorChange = fn;
  }

  onInputChange(event: any): void {
    const digitado = event.target.value;
    const mascarado = aplicarMascaraCPF(digitado);

    this.value = mascarado;
    this.onChange(mascarado);
    this.onValidatorChange();
  }

  onBlur(): void {
    if (!this.touched) {
      this.touched = true;
      this.onTouched();
    }
  }
}
