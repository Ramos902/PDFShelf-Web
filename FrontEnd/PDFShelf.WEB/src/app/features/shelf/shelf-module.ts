import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

// Importa o arquivo de rotas específico deste módulo
import { ShelfRoutingModule } from './shelf-routing-module';

// Importa os componentes standalone que serão usados nas rotas deste módulo
import { LayoutComponent } from './layout/layout';
import { HomeComponent } from './pages/home/home';

@NgModule({
  declarations: [], // Deixamos vazio pois os componentes são standalone
  imports: [
    CommonModule,
    ShelfRoutingModule, // Configuração das rotas /shelf
    LayoutComponent,    // A "casca" com a navbar
    HomeComponent       // A página com a lista de PDFs
  ]
})
export class ShelfModule { }