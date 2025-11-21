import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth-guard';
import { loginGuard } from './core/guards/login-guard';
import { LandingComponent } from './features/landing/landing';

export const routes: Routes = [
  // 3. ROTA DA "HOME" PÚBLICA (Landing Page)
  {
        path: '', // A rota raiz
    component: LandingComponent
  },

  // 4. ROTA DE AUTENTICAÇÃO (Login/Registro)
  {
    path: 'auth',
    loadChildren: () => import('./features/auth/auth-module').then(m => m.AuthModule),
    canActivate: [loginGuard]
  },
  
  // 5. ROTA DA "PRATELEIRA" PRIVADA (O app em si)
  {
    path: 'shelf', 
    // CORREÇÃO IMPORTANTE: Mudado de 'm.DashboardModule' para 'm.ShelfModule'
    // para bater com o nome da classe que renomeamos no arquivo shelf-module.ts
    loadChildren: () => import('./features/shelf/shelf-module').then(m => m.ShelfModule), 
    canActivate: [authGuard]
  },

  // 6. CATCH-ALL
  {
    path: '**',
    redirectTo: '',
    pathMatch: 'full'
  }
];