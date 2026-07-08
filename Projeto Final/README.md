# Projeto Final COBOL — Modernização Cooperativa Alfa

Solução de modernização do cadastro de clientes da Cooperativa Financeira Alfa, integrando uma aplicação .NET (camada de atendimento) com o sistema legado em COBOL (processamento e regras de negócio), preservando o componente COBOL como fonte de verdade dos dados.

> Documentação completa de arquitetura, estrutura de dados e testes em [`/docs`](./docs).

---

## Visão geral da solução

```
[ Atendente ]
      │
      ▼
[ Interface Web / Swagger ]   http://localhost:5210
      │
      ▼
[ Web API .NET 8 ]            GET /clientes/{codigo}
                              PUT /clientes/{codigo}
      │
      ▼  Process.Start + stdin/stdout (JSON)
      │
[ Programa COBOL ]            build/clientes
      │
      ▼  CALL via wrapper C/ODBC
      │
[ SQLite ]                    data/clientes.db
```

---

## Pré-requisitos

- WSL (Ubuntu 24.04) ou Linux equivalente
- GnuCOBOL 3.1.2.0 ou superior
- unixODBC + driver `libsqliteodbc`
- SQLite3
- gcc + make
- .NET SDK 8.0 ou superior

---

## Setup do ambiente

### 1. Instalar dependências (Ubuntu/WSL)

```bash
sudo apt-get update
sudo apt-get install -y gnucobol libsqliteodbc unixodbc unixodbc-dev odbcinst sqlite3 gcc make
```

### 2. Instalar o .NET SDK no WSL

```bash
wget https://dot.net/v1/dotnet-install.sh -O dotnet-install.sh
chmod +x dotnet-install.sh
./dotnet-install.sh --channel 8.0
echo 'export DOTNET_ROOT=$HOME/.dotnet' >> ~/.bashrc
echo 'export PATH=$PATH:$HOME/.dotnet:$HOME/.dotnet/tools' >> ~/.bashrc
source ~/.bashrc
dotnet --version
```

### 3. Configurar o DSN ODBC

```bash
echo "[clientesDB]" > odbc.ini
echo "Description = SQLite3 clientes" >> odbc.ini
echo "Driver = SQLite3" >> odbc.ini
echo "Database = $(pwd)/data/clientes.db" >> odbc.ini
```

> Confirme com `cat odbc.ini` que o caminho está correto antes de prosseguir.

### 4. Validar a conexão ODBC (recomendado na primeira vez)

```bash
export ODBCINI="$(pwd)/odbc.ini"
echo "SELECT 1;" | isql clientesDB -v
```

Deve retornar `Connected!`. Se der erro `[IM002]` ou `connect failed`, revise o `odbc.ini` e confirme que o driver está registrado com `odbcinst -q -d`.

---

## Compilação e execução

### Componente COBOL (via Makefile)

> O `Makefile` exporta `ODBCINI` automaticamente — não é necessário exportar manualmente.

```bash
make db-init   # cria o banco e popula com 3 clientes de exemplo
make           # compila o wrapper C e o programa COBOL
make test      # roda todos os cenários de teste em sequência
```

| Alvo | Descrição |
|---|---|
| `make` | Compila tudo, gera `build/clientes` |
| `make clean` | Remove artefatos de `build/` |
| `make db-init` | Cria tabela e insere 3 clientes de exemplo |
| `make run-consulta` | Testa consulta de cliente existente (código 1) |
| `make run-atualiza` | Testa atualização de telefone/e-mail |
| `make run-nao-encontrado` | Testa resposta para cliente inexistente (código 99) |
| `make test` | Executa todos os testes acima em sequência |

### Web API .NET

```bash
cd dotnet/CooperativaAlfa
dotnet run
```

A API sobe em `http://localhost:5210`.

| Endereço | Descrição |
|---|---|
| `http://localhost:5210` | Interface web para atendentes |
| `http://localhost:5210/swagger` | Documentação interativa da API |
| `GET /clientes/{codigo}` | Consulta cliente pelo código |
| `PUT /clientes/{codigo}` | Atualiza telefone e e-mail |

### Testes automatizados (.NET)

```bash
cd dotnet/CooperativaAlfa.Tests
dotnet test --verbosity normal
```

Para gerar relatório de cobertura:

```bash
dotnet test --collect:"XPlat Code Coverage"
```

---

## Estrutura do repositório

```
Projeto Final/
├── Makefile                        # automação de build e testes COBOL
├── README.md
├── .gitignore
├── odbc.ini                        # DSN ODBC (gerado no setup)
├── cobol/
│   ├── clientes.cob                # programa COBOL principal
│   ├── sqlitebridge.c              # wrapper C: ODBC + I/O JSON
│   ├── teste-consulta.cob          # programa de teste isolado
│   └── teste-atualiza.cob          # programa de teste isolado
├── build/                          # gerado pelo make — não versionado
│   ├── sqlitebridge.o
│   └── clientes
├── data/
│   └── clientes.db                 # banco SQLite
├── dotnet/
│   ├── CooperativaAlfa.sln
│   ├── CooperativaAlfa/            # Web API .NET 8
│   │   ├── Controllers/
│   │   │   └── ClientesController.cs
│   │   ├── Models/
│   │   │   ├── ClienteDto.cs
│   │   │   ├── AtualizaClienteRequest.cs
│   │   │   └── CobolResponse.cs
│   │   ├── Services/
│   │   │   ├── ICobolBridge.cs
│   │   │   └── CobolBridge.cs
│   │   ├── wwwroot/                # interface web estática
│   │   │   ├── index.html
│   │   │   ├── css/style.css
│   │   │   └── js/app.js
│   │   ├── Program.cs
│   │   └── appsettings.json
│   └── CooperativaAlfa.Tests/      # testes automatizados xUnit
│       ├── Helpers/
│       │   └── CobolResponseFactory.cs
│       ├── Unit/
│       │   ├── CobolResponseTests.cs
│       │   └── ClientesControllerTests.cs
│       └── Integration/
│           └── ClientesApiIntegrationTests.cs
└── docs/
    ├── arquitetura.md
    ├── estrutura-compartilhada.md
    ├── plano-de-testes.md
    ├── relatorio-ia.md
    └── Projeto Final Cobol.pdf     # enunciado do projeto
```

---

## Documentação

| Documento | Descrição |
|---|---|
| [`docs/arquitetura.md`](./docs/arquitetura.md) | Decisões arquiteturais, justificativas e fluxo de execução |
| [`docs/estrutura-compartilhada.md`](./docs/estrutura-compartilhada.md) | Contrato de dados entre COBOL e .NET |
| [`docs/plano-de-testes.md`](./docs/plano-de-testes.md) | Casos de teste, critérios de aceitação e evidências |
| [`docs/relatorio-ia.md`](./docs/relatorio-ia.md) | Utilização de IA durante o desenvolvimento |

---

## Autor
Maurício Rodrigues