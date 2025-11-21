import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { AuthResponse } from '../models/auth-response.model';
import { User } from '../models/user-model';
import { UserLoginDto } from '../models/user-model';

// Chave para salvar o token no localStorage
const TOKEN_KEY = 'pdfshelf_token';
const USER_KEY = 'pdfshelf_user';

// ↓↓↓↓↓ ESTA É A CORREÇÃO PRINCIPAL ↓↓↓↓↓
//
// Ao adicionar 'providedIn: 'root'', este serviço
// se torna um "singleton" e ganha acesso
// aos providers do app.config.ts (como o provideHttpClient())
@Injectable({
  providedIn: 'root' 
})
export class AuthService {
  
  // URL da API de usuários
  private apiUrl = 'http://localhost:5212/api/users';

  // State Signals (Estado reativo)
  public currentUser = signal<User | null>(this.getUserFromStorage());
  public isAuthenticated = signal<boolean>(this.currentUser() !== null);

  constructor(private http: HttpClient) { }

  // Método de Login
  login(credentials: UserLoginDto): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/login`, credentials).pipe(
      tap(response => {
        this.saveStorage(response.token, response.user);
        this.currentUser.set(response.user);
        this.isAuthenticated.set(true);
      })
    );
  }

  // Método de Logout
  logout(): void {
    localStorage.removeItem(TOKEN_KEY);
    localStorage.removeItem(USER_KEY);
    this.currentUser.set(null);
    this.isAuthenticated.set(false);
  }

  // --- Métodos de Ajuda ---

  getToken(): string | null {
    return localStorage.getItem(TOKEN_KEY);
  }

  private saveStorage(token: string, user: User): void {
    localStorage.setItem(TOKEN_KEY, token);
    localStorage.setItem(USER_KEY, JSON.stringify(user));
  }

  private getUserFromStorage(): User | null {
    const userJson = localStorage.getItem(USER_KEY);
    if (userJson) {
      return JSON.parse(userJson) as User;
    }
    return null;
  }
}