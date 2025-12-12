# BarberClub

> Sistema completo para gerenciamento de salões de beleza e barbearias

## Sobre o Projeto

BarberClub é uma solução fullstack para gestão de estabelecimentos de beleza, oferecendo controle completo de funcionários, serviços, agendamentos e configurações personalizadas por empresa (multi-tenant).

---

## Tecnologias Utilizadas

### Backend
- **C# / .NET** - Framework principal
- **Entity Framework Core** - ORM para acesso a dados
- **ASP.NET Core Identity** - Autenticação e autorização
- **JWT Bearer Authentication** - Tokens de acesso
- **Swagger/OpenAPI** - Documentação da API
- **Azure Blob Storage** - Armazenamento de imagens (logos/banners)

### Frontend
- **Angular 19** - Framework SPA
- **Angular Material** - Componentes UI
- **Bootstrap 5.3** - Estilização
- **RxJS** - Programação reativa
- **TypeScript** - Linguagem tipada

---

## Funcionalidades Implementadas

### Autenticação e Autorização
- Sistema de registro de usuários
- Login com JWT
- Logout seguro
- Controle de permissões baseado em roles (Admin/Funcionário)
- Guards para proteção de rotas
- Diretiva `*appHasPermission` para controle de UI

### Gestão de Funcionários
- Cadastro de funcionários
- Edição de dados
- Exclusão de funcionários
- Listagem completa
- Visualização detalhada por ID
- Configuração de horários de atendimento por funcionário
- Geração automática de horários disponíveis

### Gestão de Serviços
- Cadastro de serviços
- Edição de serviços
- Exclusão de serviços
- Listagem de serviços
- Visualização por ID
- Associação de serviços a funcionários

### Gestão de Horários
- Configuração de horários de funcionamento da empresa
- Controle de dias da semana
- Horários de abertura e fechamento
- Verificação de estabelecimento aberto/fechado
- Ativação/desativação de horários disponíveis
- Visualização de horários por data

### Configurações da Empresa (Multi-tenant)
- Cadastro de informações da empresa
- Upload de logo
- Upload de banner
- Edição do nome da empresa
- Geração de tokens SAS para imagens (Azure Blob)
- Cache inteligente de configurações (24h)
- Slug único por empresa
- Configuração de horários de expediente
- Sistema de tenant isolado por empresa

### Interface do Usuário
- Dashboard responsivo
- Sistema de notificações Toastr
- Validação avançada de formulários
- Validação de CPF
- Validação customizada de senhas (maiúsculas, minúsculas, números, especiais)
- Carregamento progressivo de dados
- Estados de loading e erro
- Design responsivo com Angular Material

---

## Arquitetura

### Backend - Camadas
```
BarberClub.WebApi/          # API e configurações
├── Controllers/            # Endpoints REST
└── Config/                 # Injeção de dependências, JWT, Swagger

BarberClub.Aplicacao/       # Lógica de aplicação
├── Commands/               # CQRS Commands
├── Services/               # Serviços de negócio
└── DTOs/                   # Data Transfer Objects

BarberClub.Dominio/         # Entidades e regras de negócio
├── ModuloAutenticacao/
├── ModuloFuncionario/
├── ModuloServico/
├── ModuloConfiguracao/
├── ModuloHorario/
└── Compartilhado/

BarberClub.Infraestrutura/  # Acesso a dados
├── Orm/                    # Entity Framework
└── Repositorios/
```

### Frontend - Estrutura
```
src/app/
├── auth/                   # Autenticação
│   ├── views/              # Login, Registro
│   ├── services/           # UserService, TokenService
│   └── guards/             # AuthGuard, AuthUserGuard
├── core/
│   └── views/
│       ├── dashboard/      # Dashboard principal
│       ├── funcionario/    # Gestão de funcionários
│       ├── servico/        # Gestão de serviços
│       └── configuracao/   # Configurações da empresa
├── tenant/                 # Sistema multi-tenant
│   ├── services/           # TenantConfigService, AzureBlobService
│   ├── constants/          # Permissions, Roles
│   └── guards/             # PermissionGuard
└── shared/                 # Componentes compartilhados
    ├── components/
    └── validators/         # Validações customizadas
```

---

## Sistema de Permissões

```typescript
// Roles disponíveis
enum Role {
  ADMIN = 0,          // Acesso total
  FUNCIONARIO = 1     // Acesso limitado
}

// Permissões por módulo
- View_Dashboard
- View_Funcionários
- View_Serviços
- View_Configurações
- View_Contas
- Permissão_de_Administrador
- Permissão_de_Funcionario
- Permissão_Pública
```

---

## API Endpoints

### Autenticação
- `POST /api/auth/registrar` - Criar conta
- `POST /api/auth/autenticar` - Login
- `POST /api/auth/sair` - Logout

### Configuração
- `GET /api/configuracao` - Obter configurações
- `POST /api/configuracao/gerar-token` - Token SAS para imagens
- `PUT /api/configuracao/banner` - Upload de banner
- `PUT /api/configuracao/logo` - Upload de logo
- `PUT /api/configuracao/nome` - Atualizar nome
- `PUT /api/configuracao/horario/{id}` - Atualizar horário

### Funcionários
- `POST /api/funcionario/cadastrar` - Criar funcionário
- `PUT /api/funcionario/editar/{id}` - Editar funcionário
- `DELETE /api/funcionario/excluir/{id}` - Excluir funcionário
- `GET /api/funcionario` - Listar todos
- `GET /api/funcionario/{id}` - Buscar por ID
- `PUT /api/funcionario/{id}/configurar-atendimento` - Configurar horários
- `POST /api/funcionario/{id}/gerar-horarios` - Gerar horários

### Serviços
- `POST /api/servico/cadastrar` - Criar serviço
- `PUT /api/servico/editar/{id}` - Editar serviço
- `DELETE /api/servico/excluir/{id}` - Excluir serviço
- `GET /api/servico` - Listar todos
- `GET /api/servico/{id}` - Buscar por ID

### Horários
- `PUT /api/horario/desativar/{horarioId}` - Desativar horário
- `PUT /api/horario/ativar/{horarioId}` - Ativar horário
- `POST /api/horario/visualizar-por-data/{id}` - Horários por data

---

## Instalação e Configuração

### Pré-requisitos
- .NET 8.0 SDK
- Node.js 18+ e npm
- SQL Server ou LocalDB
- Conta Azure (para Blob Storage)

### Backend

1. **Clone o repositório**
```bash
git clone https://github.com/LeoRodrigues133/BarberClub.git
cd BarberClub
```

2. **Configure os User Secrets**

Navegue até o diretório do projeto WebApi e inicialize os secrets:

```bash
cd BarberClub.WebApi
dotnet user-secrets init
```

Configure as seguintes chaves:

```bash
# Configurar SQL Server Connection String
dotnet user-secrets set "SQLSERVER_CONNECTION_STRING" "Server=(localdb)\\mssqllocaldb;Database=BarberClubDb;Trusted_Connection=True;MultipleActiveResultSets=true"

# Configurar JWT
dotnet user-secrets set "JWT_GENERATION_KEY" "sua_chave_jwt_super_secreta_de_32_caracteres_ou_mais"
dotnet user-secrets set "JWT_AUDIENCE_DOMAIN" "https://localhost"

# Configurar Azure Blob Storage
dotnet user-secrets set "AZURE_BLOB_STORAGE:ConnectionString" "DefaultEndpointsProtocol=https;AccountName=SUA_CONTA;AccountKey=SUA_CHAVE;EndpointSuffix=core.windows.net"
dotnet user-secrets set "AZURE_BLOB_STORAGE:ContainerName" "seu-container-name"
```

**Nota:** Para JWT_GENERATION_KEY, use uma string com pelo menos 32 caracteres aleatórios para maior segurança.

3. **Execute as migrações**
```bash
dotnet ef database update
```

4. **Inicie o backend**
```bash
dotnet run --project BarberClub.WebApi
```

Acesse: `https://localhost:7090/swagger`

### Frontend

1. **Instale as dependências**
```bash
cd client
npm install
```

2. **Configure o ambiente**

Crie ou edite o arquivo `src/environments/environment.development.ts`:

```typescript
export const environment = {
  production: false,
  urlApi: 'https://localhost:7090/api'
};
```

3. **Inicie o servidor de desenvolvimento**
```bash
ng serve
```

Acesse: `http://localhost:4200`

---

## Segurança Implementada

- **JWT Authentication** com expiração configurável
- **Password Hashing** com ASP.NET Identity
- **HTTPS** obrigatório
- **CORS** configurado
- **SQL Injection** prevenida (EF Core + parametrização)
- **XSS Protection** (Angular sanitization)
- **Role-based Authorization**
- **Query Filters** para isolamento multi-tenant
- **Validação de arquivos** (tipo e tamanho)
- **SAS Tokens** para acesso temporário a imagens
- **User Secrets** para dados sensíveis

---

## Principais Dependências

### Backend
```xml
- Microsoft.EntityFrameworkCore
- Microsoft.AspNetCore.Identity.EntityFrameworkCore
- Microsoft.AspNetCore.Authentication.JwtBearer
- Azure.Storage.Blobs
- FluentResults
- MediatR
- Swashbuckle.AspNetCore
```

### Frontend
```json
- @angular/core: 19.2.0
- @angular/material: 19.2.19
- @angular/cdk: 19.2.19
- bootstrap: 5.3.8
- rxjs: 7.8.0
```

---

## Próximos Passos / Roadmap

- [ ] Sistema de agendamentos completo
- [ ] Notificações em tempo real (SignalR)
- [ ] Relatórios e dashboard analítico
- [ ] Sistema de avaliações de clientes
- [ ] Integração com meios de pagamento
- [ ] App mobile (React Native/Flutter/Ionic)
- [ ] Notificações por email/SMS
- [ ] Sistema de fidelidade
- [ ] Agenda pública para clientes

---

## Autor

**Leonardo Rodrigues**

- GitHub: [@LeoRodrigues133](https://github.com/LeoRodrigues133)

---

## Licença

Este projeto é de uso pessoal e educacional.

---

## Contribuindo

Contribuições são bem-vindas! Sinta-se à vontade para abrir issues ou pull requests.

---

## Suporte

Para dúvidas ou sugestões, abra uma issue no repositório.

---
