import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth'; // Ou '../services/auth' se seu arquivo for auth.ts
import { catchError, throwError } from 'rxjs';
import { Router } from '@angular/router';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const router = inject(Router);
  const token = authService.getToken();

  if (token) {
    console.log('Interceptor: Adicionando token na requisição para', req.url); // <-- Log para debug
        const cloned = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
    
    return next(cloned).pipe(
      catchError((error: HttpErrorResponse) => {
        // Se o erro for 401 (Não Autorizado), o token é inválido/expirado
        if (error.status === 401) {
          console.error('Interceptor: Token inválido ou expirado. Deslogando...');
          authService.logout(); // Limpa o localStorage e os signals
          router.navigate(['/auth/login']); // Redireciona para o login
        }
        return throwError(() => error); // Propaga o erro para o serviço que fez a chamada
      })
    );
  }

  console.log('Interceptor: Requisição sem token (Login/Public)');
  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      // Mesmo para requisições públicas, é bom logar os erros
      console.error(`Erro na requisição para ${req.url}:`, error.message);
      return throwError(() => error);
    })
  );
};