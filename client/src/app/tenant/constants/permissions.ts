export enum Permission {
  VIEW_HOME = 'View_Dashboard',
  VIEW_EMPLOYEES = 'View_Funcionários',
  VIEW_SERVICES = 'View_Serviços',
  VIEW_SETTINGS = 'View_Configurações',
  VIEW_ACCOUNTS = 'View_Contas',
  VIEW_AUTH = 'View_Autenticação',
  ADMIN_PERMISSION = "Permissão_de_Administrador",
  EMPLOYEE_PERMISSION = "Permissão_de_Funcionario"
}

export enum Role {
  ADMIN = 0,
  FUNCIONARIO = 1,
}

export const ROLE_PERMISSIONS: Record<Role, Permission[]> = {
  [Role.ADMIN]: [
    Permission.VIEW_HOME,
    Permission.VIEW_EMPLOYEES,
    Permission.VIEW_SERVICES,
    Permission.VIEW_SETTINGS,
    Permission.VIEW_ACCOUNTS,
    Permission.ADMIN_PERMISSION,
  ],
  [Role.FUNCIONARIO]: [
    Permission.EMPLOYEE_PERMISSION,
    Permission.VIEW_SERVICES,
    Permission.VIEW_SETTINGS,
  ],
};
