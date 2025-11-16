import { Injectable } from '@angular/core';
import { TokenResponse } from '../models/auth.models';

@Injectable()
export class LocalStorageService {
  private readonly chave: string = 'BarberClub.token';

  public salvarTokenAutenticacao(token: TokenResponse): void {
    const jsonString = JSON.stringify(token);

    localStorage.setItem(this.chave, jsonString)
  }

  obterTokenAutenticacao(): TokenResponse | undefined {
    const jsonString = localStorage.getItem(this.chave);

    if (!jsonString) return undefined;

    return JSON.parse(jsonString);
  }

  public limparDadosLocais(): void {
    localStorage.removeItem(this.chave)
  }

public estaAutenticado(): boolean {
  const token = this.obterTokenAutenticacao();
  if (!token) return false;

  const dataExpiracao = new Date(token.expiracaoToken);
  return dataExpiracao > new Date();
}
}
