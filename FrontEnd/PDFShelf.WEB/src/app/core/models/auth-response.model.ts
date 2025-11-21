import { User } from "./user-model";

// Esta interface define a resposta completa do seu endpoint /api/users/login
export interface AuthResponse {
  message: string;
  token: string;
  user: User;
}