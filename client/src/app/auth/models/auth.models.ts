import { Role } from "../../tenant/constants/permissions";

export interface RegistrarUsuarioRequest {
  userName: string;
  email: string;
  password: string;
}

export interface AutenticarUsuarioRequest {
  userName: string;
  password: string;
}

export interface TokenResponse {
  chave: string;
  expiracaoToken: Date;
  usuario: UsuarioAutenticadoDto;
}

export interface UsuarioAutenticadoDto {
  id: string;
  userName: string;
  email: string;
  role: Role;
}
