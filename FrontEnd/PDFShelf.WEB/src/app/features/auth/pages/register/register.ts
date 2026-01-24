import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../../../core/services/auth/auth';
import { UserRegisterDto } from '../../../../core/models/user-model';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    ReactiveFormsModule // Essencial para [formGroup]
  ],
  templateUrl: './register.html',
  styleUrls: ['./register.scss']
})
export class RegisterComponent {
  // Injeção de dependências moderna
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);

  // Mensagem de erro da API
  apiError: string | null = null;

  // Definição do formulário
  registerForm = this.fb.group({
    name: ['', [Validators.required, Validators.minLength(3)]],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6)]]
  });

  // Método chamado no submit do formulário
  onSubmit() {
    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();
      return;
    }

    this.apiError = null;
    const credentials = this.registerForm.value as UserRegisterDto;

    this.authService.register(credentials).subscribe({
      next: () => {
        this.router.navigate(['/shelf']);
      },
      error: (err) => {
        this.apiError = err.error?.message || 'Ocorreu um erro. Tente novamente.';
      }
    });
  }

  isFieldInvalid(fieldName: string): boolean {
    const control = this.registerForm.get(fieldName);
    return !!control && control.invalid && control.touched;
  }

  getErrorMessage(fieldName: string): string {
    const control = this.registerForm.get(fieldName);
    if (control?.hasError('required')) {
      return 'Este campo é obrigatório.';
    }
    if (control?.hasError('email')) {
      return 'Por favor, insira um e-mail válido.';
    }
    if (control?.hasError('minlength')) {
      const requiredLength = control.errors?.['minlength'].requiredLength;
      return `O campo deve ter no mínimo ${requiredLength} caracteres.`;
    }
    return '';
  }
}

