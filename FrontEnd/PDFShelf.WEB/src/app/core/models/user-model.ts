export interface User {
  id: string; // O .NET usa Guid, que no front vira string
  name: string;
  email: string;
  role: string;
} 

export interface UserLoginDto {
  email: string;
  password: string;
}