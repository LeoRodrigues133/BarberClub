import { APP_INITIALIZER } from '@angular/core';
import { ServicoConfiguracaoTenant } from './tenant-config.service';

export function inicializarTenant(servicoTenant: ServicoConfiguracaoTenant) {
  return (): Promise<any> => {

    const tokenString = localStorage.getItem('BarberClub.token');

    if (!tokenString) {
      console.log('Sem token - configuração não carregada');
      return Promise.resolve();
    }

    try {
      const token = JSON.parse(tokenString);
      const dataExpiracao = new Date(token.expiracaoToken);


      if (dataExpiracao > new Date()) {
        console.log('Carregando configuração do tenant...');

        return servicoTenant.carregarPorAutenticacao()
          .toPromise()
          .then((config:any) => {
            console.log('Configuração carregada:', config?.nomeEmpresa);
            return config;
          })
          .catch((erro:any) => {
            console.warn('Erro ao carregar configuração na inicialização:', erro);
            return null;
          });
      }
    } catch (erro) {
      console.warn('Token inválido no localStorage');
    }

    return Promise.resolve();
  };
}
