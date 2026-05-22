# Blog Pessoal — ASP.NET Core Web API

API RESTful desenvolvida com ASP.NET Core 10 como projeto de aprendizado do ecossistema .NET. O projeto implementa um blog pessoal com gerenciamento de usuários, postagens e temas, autenticação JWT e integração com inteligência artificial via Gemini API.

---

## Tecnologias utilizadas

- **Linguagem:** C# 12+
- **Framework:** ASP.NET Core 10 Web API
- **Banco de dados:** MySQL
- **ORM:** Entity Framework Core 9
- **Segurança:** JWT Bearer + BCrypt
- **Documentação:** Scalar (OpenAPI 3.1)
- **IA:** Google Gemini API
- **Testes:** xUnit + Moq + FluentAssertions
- **Qualidade:** SonarQube

---

## Pré-requisitos

Antes de rodar o projeto, certifique-se de ter instalado:

- [.NET SDK 10](https://dotnet.microsoft.com/download)
- [MySQL Server](https://dev.mysql.com/downloads/mysql/)
- [MySQL Workbench](https://dev.mysql.com/downloads/workbench/) (opcional, para visualizar o banco)
- [VS Code](https://code.visualstudio.com/) com a extensão **C# Dev Kit**

---

## Configuração do banco de dados

1. Abra o MySQL Workbench e execute:

```sql
CREATE USER 'blogpessoal'@'localhost' IDENTIFIED BY 'sua_senha';
GRANT ALL PRIVILEGES ON db_blogpessoal.* TO 'blogpessoal'@'localhost';
FLUSH PRIVILEGES;
CREATE DATABASE db_blogpessoal;
```

---

## Configuração do projeto

1. Clone o repositório:

```bash
git clone https://github.com/seu-usuario/seu-repositorio.git
cd "Projeto Blog Pessoal/BlogPessoal"
```

2. Inicialize o User Secrets (as credenciais nunca são salvas no repositório):

```bash
dotnet user-secrets init
```

3. Configure as credenciais via User Secrets:

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Database=db_blogpessoal;Uid=blogpessoal;Pwd=sua_senha;"
dotnet user-secrets set "Jwt:Key" "SuaChaveJWTComPeloMenos32Caracteres!"
dotnet user-secrets set "Gemini:ApiKey" "sua_chave_gemini"
```

> A chave JWT deve ter no mínimo 32 caracteres. A chave Gemini pode ser gerada em [aistudio.google.com](https://aistudio.google.com).

4. Instale as dependências e aplique as migrations:

```bash
dotnet restore
dotnet ef database update
```

---

## Rodando o projeto

```bash
dotnet run
```

A API estará disponível em `http://localhost:5128`.

A documentação interativa pode ser acessada em:

```
http://localhost:5128/scalar/v1
```

---

## Rodando os testes

```bash
cd ../BlogPessoal.Tests
dotnet test
```

O projeto possui 35 testes unitários cobrindo as camadas de serviço de Tema, Usuario e Postagem.

---

## Endpoints da API

### Autenticação

| Método | Endpoint | Descrição | Auth |
|--------|----------|-----------|------|
| POST | `/api/usuarios/cadastrar` | Cadastrar novo usuário | Público |
| POST | `/api/usuarios/login` | Login e geração do token JWT | Público |

### Usuários

| Método | Endpoint | Descrição | Auth |
|--------|----------|-----------|------|
| GET | `/api/usuarios` | Listar todos os usuários | JWT |
| GET | `/api/usuarios/{id}` | Buscar usuário por ID | JWT |
| PUT | `/api/usuarios/{id}` | Atualizar usuário | JWT |
| DELETE | `/api/usuarios/{id}` | Excluir usuário | JWT |

### Temas

| Método | Endpoint | Descrição | Auth |
|--------|----------|-----------|------|
| GET | `/api/temas` | Listar todos os temas | JWT |
| GET | `/api/temas/{id}` | Buscar tema por ID | JWT |
| POST | `/api/temas` | Criar novo tema | JWT |
| PUT | `/api/temas/{id}` | Atualizar tema | JWT |
| DELETE | `/api/temas/{id}` | Excluir tema | JWT |

### Postagens

| Método | Endpoint | Descrição | Auth |
|--------|----------|-----------|------|
| GET | `/api/postagens` | Listar todas as postagens | JWT |
| GET | `/api/postagens/{id}` | Buscar postagem por ID | JWT |
| GET | `/api/postagens/filtro?autor={id}&tema={id}` | Filtrar por autor e/ou tema | JWT |
| POST | `/api/postagens` | Criar nova postagem | JWT |
| PUT | `/api/postagens/{id}` | Atualizar postagem | JWT |
| DELETE | `/api/postagens/{id}` | Excluir postagem | JWT |

### Inteligência Artificial

| Método | Endpoint | Descrição | Auth |
|--------|----------|-----------|------|
| POST | `/api/ia/resumir` | Gerar resumo, tags e categoria de um texto | JWT |

---

## Autenticação com JWT

Todos os endpoints protegidos exigem o token JWT no header da requisição:

```
Authorization: Bearer {seu_token}
```

O token é gerado no endpoint de login e tem validade de 8 horas.

---

## Arquitetura do projeto

O projeto segue arquitetura em camadas:

```
BlogPessoal/
├── Controllers/        # Recebe as requisições HTTP
│   └── IA/             # Controller do endpoint de IA
├── Services/           # Regras de negócio
│   └── IA/             # Serviço de integração com Gemini
├── Repositories/       # Acesso ao banco de dados
├── Models/             # Entidades do domínio
├── DTOs/               # Objetos de transferência de dados
├── Data/               # DbContext e configuração do EF Core
├── Config/             # Configurações auxiliares
└── Migrations/         # Migrations do banco de dados

BlogPessoal.Tests/
├── TemaServiceTests.cs
├── UsuarioServiceTests.cs
└── PostagemServiceTests.cs
```

---

## Funcionalidade de IA

Ao criar uma postagem, a API envia automaticamente o conteúdo para a Gemini API, que retorna:

- **Resumo** — síntese do conteúdo em até 2 frases
- **Tags** — palavras-chave relacionadas ao texto
- **Categoria** — classificação temática sugerida

Esses dados são salvos nos campos `ResumoIA`, `TagsIA` e `CategoriaIA` da postagem.

---

## Segurança

- Senhas armazenadas com hash BCrypt (nunca em texto puro)
- Autenticação via JWT com validação de issuer, audience e expiração
- Credenciais gerenciadas via .NET User Secrets (nunca no repositório)
- Endpoints protegidos com `[Authorize]`
- Dados sensíveis omitidos nos responses via DTOs

---

## Qualidade de código

O projeto foi analisado com SonarQube, obtendo:

- **Quality Gate:** Passed
- **Issues:** 0
- **Security Hotspots:** 0
- **Duplicações:** 0%