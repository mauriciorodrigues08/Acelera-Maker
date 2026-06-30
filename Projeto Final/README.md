# Projeto Final COBOL — Modernização Cooperativa Alfa

Solução de modernização do cadastro de clientes da Cooperativa Financeira Alfa, integrando uma aplicação .NET (camada de atendimento) com o sistema legado em COBOL (processamento e regras de negócio), preservando o componente COBOL como fonte de verdade dos dados.

> Documentação completa de arquitetura, estrutura de dados e testes em [`/docs`](./docs).

## Status do projeto

Em desenvolvimento. Última atualização: configuração de persistência (SQLite + ODBC) validada.

## Pré-requisitos

- WSL (Ubuntu 24.04) ou Linux equivalente
- GnuCOBOL (testado com 3.1.2.0 e 4.0-early)
- unixODBC + driver `libsqliteodbc`
- SQLite3
- .NET SDK (versão a definir na Fase 7)
- gcc (para compilar o wrapper de integração COBOL ↔ SQLite)

## Setup do ambiente

### 1. Instalar dependências (Ubuntu/WSL)

```bash
sudo apt-get update
sudo apt-get install -y gnucobol4 libsqliteodbc unixodbc unixodbc-dev odbcinst sqlite3 gcc
```

### 2. Criar o banco de dados local

```bash
sqlite3 clientes.db "CREATE TABLE clientes (codigo INTEGER PRIMARY KEY, nome TEXT, telefone TEXT, email TEXT);"
```

### 3. Configurar o DSN ODBC (arquivo `odbc.ini` na raiz do projeto)

```bash
echo "[clientesDB]" > odbc.ini
echo "Description = SQLite3 clientes" >> odbc.ini
echo "Driver = SQLite3" >> odbc.ini
echo "Database = $(pwd)/clientes.db" >> odbc.ini
```

> O caminho gerado deve ser absoluto. Se a pasta do projeto tiver espaços no nome (ex.: `Projeto Final`), teste a conexão antes de seguir (passo 5).

### 4. Definir a variável de ambiente do ODBC

Antes de compilar ou executar qualquer programa COBOL que acesse o banco, exporte:

```bash
export ODBCINI="$(pwd)/odbc.ini"
```

> Essa variável precisa estar definida em toda sessão de terminal nova. Considere adicioná-la ao `.bashrc` do ambiente de desenvolvimento, ou ao script de execução do projeto.

### 5. Validar a conexão ODBC

```bash
echo "SELECT * FROM clientes;" | isql clientesDB -v
```

Deve retornar `Connected!`. Se der erro `[IM002]` ou `connect failed`, revise o `odbc.ini` e confirme que o driver está registrado com `odbcinst -q -d`.

## Compilação

```bash
gcc -c src/sqlitebridge.c -o sqlitebridge.o
cobc -x -free src/<programa>.cob sqlitebridge.o -o <executavel> -lodbc
```

## Estrutura do repositório

```
/
├── README.md
├── odbc.ini
├── clientes.db
├── src/
│   ├── sqlitebridge.c        # wrapper ODBC chamado pelo COBOL via CALL
│   └── *.cob                 # programas COBOL
├── docs/
│   ├── arquitetura.md
│   ├── estrutura-compartilhada.md
│   ├── plano-de-testes.md
│   └── relatorio-ia.md
├── dotnet/                   # aplicação .NET (a definir — Fase 7)
└── tests/                    # testes automatizados (a definir — Fase 8)
```

## Como executar (a atualizar conforme o projeto avança)

Pendente: instruções de execução do fluxo completo .NET → COBOL → SQLite.