import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms'; 

import { AuthRoutingModule } from './auth-routing-module';
import { LoginComponent } from './pages/login/login';
import { RegisterComponent } from './pages/register/register';


@NgModule({
  declarations: [
    
  ],
  imports: [
    CommonModule,
    AuthRoutingModule,
    ReactiveFormsModule,
    LoginComponent,
    RegisterComponent
  ]
})
export class AuthModule { }