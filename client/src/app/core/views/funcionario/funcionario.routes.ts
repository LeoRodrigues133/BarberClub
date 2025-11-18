import { Routes } from "@angular/router";
import { ListarFuncionarioComponent } from "./listar/listar-funcionario.component";
import { listarFuncionarioResolver } from "./services/listar-funcionario.resolver";
import { EditarFuncionarioComponent } from "./editar/editar-funcionario.component";
import { DetalharFuncionarioResolver } from "./services/detalhar-funcionario.resolver";
import { CadastrarFuncionarioComponent } from "./cadastro/cadastrar-funcionario.component";
import { ExcluirFuncionarioComponent } from "./excluir/excluir-funcionario.component";
import { VisualizarFuncionarioComponent } from "./visualizar/visualizar-funcionario.component";

export const funcionarioRoutes: Routes = [

  {
    path: '',
    redirectTo: 'listar',
    pathMatch: 'full'
  },
  {
    path: 'listar',
    component: ListarFuncionarioComponent,
    resolve: {
      funcionarios: listarFuncionarioResolver
    }
  },
  {
    path: 'cadastrar',
    component: CadastrarFuncionarioComponent,

  },
  {
    path: 'editar/:id',
    component: EditarFuncionarioComponent,
    resolve: {
      funcionario: DetalharFuncionarioResolver
    }
  },
  {
    path: 'excluir/:id',
    component: ExcluirFuncionarioComponent,
    resolve: {
      funcionario: DetalharFuncionarioResolver
    }
  },
  {
    path: 'visualizar/:id',
    component: VisualizarFuncionarioComponent,
    resolve: {
      funcionario: DetalharFuncionarioResolver
    }
  }
]
