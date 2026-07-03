# Projeto Final COBOL — Modernização Cooperativa Alfa

Solução de modernização do cadastro de clientes da Cooperativa Financeira Alfa, integrando uma aplicação .NET (camada de atendimento) com o sistema legado em COBOL (processamento e regras de negócio), preservando o componente COBOL como fonte de verdade dos dados.

> Documentação completa de arquitetura, estrutura de dados e testes em [`/docs`](./docs).

## Status do projeto

Em desenvolvimento. Última atualização: componente COBOL principal implementado e testado.

## Pré-requisitos

- WSL (Ubuntu 24.04) ou Linux equivalente
- GnuCOBOL (testado com 3.1.2.0)
- unixODBC + driver `libsqliteodbc`
- SQLite3
- gcc + make
- .NET SDK (a instalar na Fase 7)

## Setup do ambiente

### 1. Instalar dependências (Ubuntu/WSL)

```bash
sudo apt-get update
sudo apt-get install -y gnucobol libsqliteodbc unixodbc unixodbc-dev odbcinst sqlite3 gcc make
```

### 2. Configurar o DSN ODBC (arquivo `odbc.ini` na raiz do projeto)

```bash
echo "[clientesDB]" > odbc.ini
echo "Description = SQLite3 clientes" >> odbc.ini
echo "Driver = SQLite3" >> odbc.ini
echo "Database = $(pwd)/data/clientes.db" >> odbc.ini
```

> O caminho deve ser absoluto. Confirme com `cat odbc.ini` antes de prosseguir.

### 3. Validar a conexão ODBC (recomendado na primeira vez)

```bash
export ODBCINI="$(pwd)/odbc.ini"
echo "SELECT 1;" | isql clientesDB -v
```

Deve retornar `Connected!`. Se der erro `[IM002]` ou `connect failed`, revise o `odbc.ini` e confirme que o driver está registrado com `odbcinst -q -d`.

## Compilação e execução (via Makefile)

> O `Makefile` exporta `ODBCINI` automaticamente — não é necessário exportar manualmente antes de rodar os alvos.

### Primeira vez

```bash
make db-init   # cria o banco e popula com dados de exemplo
make           # compila o wrapper C e o programa COBOL
```

### Testar

```bash
make test      # roda todos os cenários de teste em sequência
```

### Todos os alvos disponíveis

| Alvo | Descrição |
|---|---|
| `make` | Compila tudo, gera `build/clientes` |
| `make clean` | Remove artefatos de `build/` |
| `make db-init` | Cria tabela e insere 3 clientes de exemplo |
| `make run-consulta` | Testa consulta de cliente existente (código 1) |
| `make run-atualiza` | Testa atualização de telefone/e-mail e confirma com consulta |
| `make run-nao-encontrado` | Testa resposta para cliente inexistente (código 99) |
| `make test` | Executa todos os testes acima em sequência |

## Estrutura do repositório

```
Projeto Final/
├── Makefile                      # automação de build e testes
├── README.md
├── .gitignore
├── odbc.ini                      # DSN ODBC (gerado no setup)
├── src/
│   ├── clientes.cob              # programa COBOL principal
│   ├── sqlitebridge.c            # wrapper C: ODBC + I/O JSON
│   ├── teste-consulta.cob        # programa de teste isolado
│   └── teste-atualiza.cob        # programa de teste isolado
├── build/                        # gerado pelo make — não versionado
│   ├── sqlitebridge.o
│   └── clientes
├── data/
│   └── clientes.db               # banco SQLite
├── docs/
│   ├── arquitetura.md
│   ├── estrutura-compartilhada.md
│   ├── plano-de-testes.md
│   ├── relatorio-ia.md
│   └── Projeto Final Cobol.pdf   # enunciado do projeto
├── dotnet/                       # aplicação .NET (Fase 7)
└── tests/                        # testes automatizados (Fase 8)
```

## Como executar (a completar na Fase 6)

Pendente: instruções de execução do fluxo completo .NET → COBOL → SQLite.