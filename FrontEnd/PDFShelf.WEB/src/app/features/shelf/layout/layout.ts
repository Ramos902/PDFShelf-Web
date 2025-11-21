import { Component, inject } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router'; 
import { CommonModule } from '@angular/common';
import { AuthService } from '../../../core/services/auth'; // Certifique-se que o caminho para auth.service está correto

@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [
    CommonModule,
    RouterOutlet 
  ],
  templateUrl: './layout.html',
  styleUrls: ['./layout.scss']
})
export class LayoutComponent {
  
  // Injeta os serviços
  public authService = inject(AuthService);
  private router = inject(Router);

  // Pega o nome do usuário para o template
  public userName = this.authService.currentUser()?.name;

  // Método de logout
  logout(): void {
    this.authService.logout();
    this.router.navigate(['/auth/login']);
  }
}