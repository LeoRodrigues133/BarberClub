import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { ActivatedRoute, Router } from '@angular/router';
import { ServicoService } from '../services/servico.service';
import { NotificacaoToastrService } from '../../../shared/components/notificacao/notificacao-toastr.service';

@Component({
  selector: 'app-editar-servico',
  imports: [
    MatIconModule,
    MatCardModule,
    MatFormFieldModule,
    MatSlideToggleModule,
    ReactiveFormsModule,
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatIconModule,
    MatCardModule,
  ],
  templateUrl: './editar-servico.component.html',
  styleUrl: './editar-servico.component.scss'
})
export class EditarServicoComponent implements OnInit {
  form: FormGroup

  constructor(
    private router: Router,
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private servicoService: ServicoService,
    private toastr:NotificacaoToastrService
  ) {
    this.form = this.fb.group({
      titulo: ['',
        [
          Validators.required,
          Validators.minLength(3),
          Validators.maxLength(50)]
      ],
      valor: [
        '',
        [
          Validators.required,
          Validators.min(1)
        ]
      ],
      duracao: [
        '',
        [Validators.min(1)]
      ],
      isPromocao:
        [
          false
        ],
      porcentagemPromocao: [
        {
          value: '',
          disabled: true
        },
        [
          Validators.min(0),
          Validators.max(90)
        ]
      ]
    });

    this.setupPromocaoWatcher();
  }

  ngOnInit(): void {
    const servico = this.route.snapshot.data['servico'];

    this.form.patchValue(servico);
  }


  private setupPromocaoWatcher() {
    this.form.get('isPromocao')!.valueChanges.subscribe(isPromo => {
      const porcentagemField = this.form.get('porcentagemPromocao');

      if (isPromo) {
        porcentagemField?.enable();
        porcentagemField?.addValidators(Validators.required);
      } else {
        porcentagemField?.disable();
        porcentagemField?.removeValidators(Validators.required);
        porcentagemField?.setValue('');
      }

      porcentagemField?.updateValueAndValidity();
    });
  }

  public editar() {
    if (this.form.invalid) return;

    const request = this.form.value;

    this.servicoService.Cadastrar(request).subscribe({
      next: (servicoCadastrado) => this.processarSucesso(),
      error: (erro) => this.processarFalha(erro)
    });

  }

  get titulo() {
    return this.form.get('titulo');
  }
  get valor() {
    return this.form.get('valor');
  }
  get duracao() {
    return this.form.get('duracao');
  }
  get isPromocao() {
    return this.form.get('isPromocao');
  }
  get porcentagemPromocao() {
    return this.form.get('porcentagemPromocao');
  }


  private processarSucesso(): void {
    this.toastr.sucesso(`Serviço editado com sucesso!`);

    this.router.navigate(['/services', 'listar']);
  }

  private processarFalha(erro: Error): any {
    this.toastr.erro(erro.message);
  }
}
