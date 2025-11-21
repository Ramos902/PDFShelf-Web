import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LayoutComponent } from './layout/layout';
import { HomeComponent } from './pages/home/home';

const routes: Routes = [
  {
    path: '', // A rota "raiz" do módulo (/shelf)
    component: LayoutComponent, // Carrega o Layout (com a navbar e o botão Sair)
    children: [
      {
        path: '', // O caminho vazio dentro de /shelf
        component: HomeComponent // Carrega a Home (lista de PDFs) DENTRO do router-outlet do Layout
      }
      // No futuro, você pode adicionar mais rotas filhas aqui, como:
      // { path: 'profile', component: ProfileComponent }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ShelfRoutingModule { }