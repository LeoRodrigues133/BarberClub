import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root'
})
export class NotificacaoToastrService {

  constructor(private snackbar: MatSnackBar) { }

  sucesso(mensagem: string): void {
    this.snackbar.open(mensagem, 'OK', {
      panelClass: ['notificacao', 'sucesso'],
      announcementMessage: 'Notificação de Sucesso'
    });
  }

  aviso(mensagem: string): void {
    this.snackbar.open(mensagem, 'OK', {
      panelClass: ['notificacao', 'aviso'],
      announcementMessage: 'Notificação de Aviso'
    });
  }

  erro(mensagem: string): void {
    this.snackbar.open(mensagem, 'OK', {
      panelClass: ['notificacao', 'erro'],
      announcementMessage: 'Notificação de Erro'
    });
  }
}
