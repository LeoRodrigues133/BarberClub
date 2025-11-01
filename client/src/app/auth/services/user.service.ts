import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { UsuarioAutenticadoDto } from '../models/auth.models';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private usuarioAutenticadoSubject: BehaviorSubject<UsuarioAutenticadoDto | undefined>;


  constructor() {
    this.usuarioAutenticadoSubject = new BehaviorSubject<UsuarioAutenticadoDto | undefined>(undefined);

  }

  get usuarioAutenticado() {
    return this.usuarioAutenticadoSubject.asObservable();
  }

  public logarUsuario(usuario: UsuarioAutenticadoDto): void {
    this.usuarioAutenticadoSubject.next(usuario);
  }

  public logout(): void {
    this.usuarioAutenticadoSubject.next(undefined);
  }


}
