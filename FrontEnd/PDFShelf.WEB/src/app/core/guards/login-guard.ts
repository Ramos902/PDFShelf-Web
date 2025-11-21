import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth';

export const loginGuard: CanActivateFn = (route, state) => {

  const authService = inject(AuthService);
  const router = inject(Router);

  // Verifica se o usuário NÃO está autenticado
  if (!authService.isAuthenticated()) {
    return true; // Permite o acesso (mostra a tela de login)
  }

  // Se JÁ ESTIVER logado, redireciona para a prateleira (/shelf)
  // Isso evita que o usuário logado veja a tela de login de novo
  router.navigate(['/shelf']);
  return false; // Bloqueia o acesso à tela de login
};