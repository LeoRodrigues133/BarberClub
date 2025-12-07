import { Routes } from "@angular/router";
import { ListarServicoComponent } from "./listar/listar-servico.component";
import { EditarServicoComponent } from "./editar/editar-servico.component";
import { ExcluirServicoComponent } from "./excluir/excluir-servico.component";
import { DetalharServicoResolver } from "./services/detalhar-servico.resolver";
import { ListarServicoResolver } from "./services/listar-servico.resolver";
import { CadastrarServicoComponent } from "./cadastrar/cadastrar-servico.component";
import { DetalharFuncionarioResolver } from "../funcionario/services/detalhar-funcionario.resolver";
import { listarFuncionarioResolver } from "../funcionario/services/listar-funcionario.resolver";

export const servicoRoutes: Routes = [
  {
    path: '',
    redirectTo: 'listar',
    pathMatch: 'full'
  },
  {
    path: 'listar',
    component: ListarServicoComponent,
    resolve: {
      servicos: ListarServicoResolver,
      funcionarios: listarFuncionarioResolver
    }
  },
  {
    path: 'cadastrar',
    component: CadastrarServicoComponent
  },
  {
    path: 'editar/:id',
    component: EditarServicoComponent,
    resolve: {
      servico: DetalharServicoResolver
    }
  },
  {
    path: 'excluir/:id',
    component: ExcluirServicoComponent,
    resolve: {
      servico: DetalharServicoResolver
    }
  },
  {
    path: 'visualizar/:id',
    component: ListarServicoComponent,
    resolve: {
      servico: DetalharServicoResolver
    }
  }
]
