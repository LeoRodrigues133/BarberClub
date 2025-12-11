import { Component } from '@angular/core';
import { NomeConfiguracaoComponent } from "./_administrador/nome/nome-configuracao.component";
import { AvatarConfiguracaoComponent } from "./_administrador/avatar/avatar-configuracao.component";
import { BannerConfiguracaoComponent } from "./_administrador/banner/banner-configuracao.component";
import { HorarioConfiguracaoComponent } from "./_administrador/horario/horario-configuracao.component";
import { HasPermissionDirective } from '../../../tenant/directives/has-permission.directive';
import { Permission } from '../../../tenant/constants/permissions';
import { ConfigurarHorariosComponent } from "./_funcionario/configuracao-horarios/configuracao-horarios.component";
import { TempoAtendimentoComponent } from "./_funcionario/atendimento/tempo-atendimento.component";
@Component({
  selector: 'app-configuracao-empresa',
  standalone: true,
  imports: [
    NomeConfiguracaoComponent,
    AvatarConfiguracaoComponent,
    BannerConfiguracaoComponent,
    HorarioConfiguracaoComponent,
    HasPermissionDirective,
    ConfigurarHorariosComponent,
    TempoAtendimentoComponent
],
  templateUrl: './configuracao.component.html',
  styleUrl: './configuracao.component.scss'
})
export class ConfiguracaoComponent{
  Permission = Permission
}
