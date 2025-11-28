import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root'
})
export class NotificacaoToastrService {

  constructor(private snackbar : MatSnackBar) { }

  sucesso(mensagem: string): void {
    this.snackbar.open(mensagem, 'OK', {
      panelClass: ['notificacao-sucesso'],
    });
  }

  aviso(mensagem: string): void {
    this.snackbar.open(mensagem, 'OK');
  }

  erro(mensagem: string): void {
    this.snackbar.open(mensagem, 'OK', {
      panelClass: ['notificacao-erro'],
    });
  }
}
