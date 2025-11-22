import { Component } from '@angular/core';
import { NomeConfiguracaoComponent } from "./nome/nome-configuracao.component";
import { AvatarConfiguracaoComponent } from "./avatar/avatar-configuracao.component";
import { BannerConfiguracaoComponent } from "./banner/banner-configuracao.component";
import { HorarioConfiguracaoComponent } from "./horario/horario-configuracao.component";
import { HasPermissionDirective } from '../../../tenant/directives/has-permission.directive';
import { Permission } from '../../../tenant/constants/permissions';
@Component({
  selector: 'app-configuracao-empresa',
  standalone: true,
  imports: [
    NomeConfiguracaoComponent,
    AvatarConfiguracaoComponent,
    BannerConfiguracaoComponent,
    HorarioConfiguracaoComponent,
    HasPermissionDirective
],
  templateUrl: './configuracao.component.html',
  styleUrl: './configuracao.component.scss'
})
export class ConfiguracaoComponent{
  Permission = Permission
}
