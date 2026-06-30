# Documento de Arquitetura

> Documento atualizado incrementalmente ao longo do desenvolvimento, registrando cada decisão técnica tomada.

## 1. Contexto e objetivo

A Cooperativa Financeira Alfa mantém um sistema legado em COBOL responsável pelo processamento e armazenamento dos dados cadastrais de clientes. O objetivo deste projeto é construir uma nova camada de atendimento (.NET) que consulte e atualize esses dados **sem substituir** o componente legado, preservando-o como responsável pelo processamento e persistência.

## 2. Visão geral da arquitetura

```
[ Atendente ]
      │
      ▼
[ Aplicação .NET ]  ── chama via Process.Start ──▶  [ Executável COBOL ]
                                                            │
                                                            ▼
                                                   [ Wrapper C / ODBC ]
                                                            │
                                                            ▼
                                                     [ Banco SQLite ]
```

A aplicação .NET nunca acessa o banco de dados diretamente — toda leitura e escrita passa pelo componente COBOL, que continua sendo a única fonte de verdade dos dados cadastrais. Isso preserva o requisito de negócio de manter o processamento existente.

## 3. Decisões de arquitetura

### 3.1 Comunicação entre .NET e COBOL

**Decisão:** chamada do executável COBOL como processo do sistema operacional, via `Process.Start` no lado .NET.

**Alternativas consideradas:**
- *Comunicação via arquivo compartilhado* (.NET escreve um arquivo de entrada, COBOL lê e escreve a saída): descartada por introduzir latência de I/O em disco a cada operação e exigir mecanismo de lock para evitar concorrência entre requisições simultâneas.
- *Interop via DLL/biblioteca compartilhada* (compilar o COBOL como `.so` e chamar via P/Invoke): seria a opção de melhor desempenho, por eliminar o custo de criação de processo, mas tem maior complexidade de configuração. Registrada como possível evolução futura.

**Justificativa da escolha:** a chamada via processo equilibra simplicidade de implementação com desempenho aceitável para o volume de operações esperado neste projeto (consulta e atualização pontuais, não em lote), além de manter um isolamento claro entre os dois mundos tecnológicos (.NET e COBOL), facilitando depuração e manutenção.

### 3.2 Persistência de dados

**Decisão:** SQLite, acessado pelo componente COBOL através de uma rotina externa em C que utiliza a API ODBC (unixODBC + driver `libsqliteodbc`).

**Alternativas consideradas:**
- *DB2 real*: representaria com mais fidelidade o cenário de mainframe estudado (z/OS Connect + DB2), mas exigiria provisionamento de uma instância DB2 (licenciada ou via container), o que adiciona uma dependência de infraestrutura desproporcional ao escopo e ao tempo disponível para o projeto.
- *Arquivo sequencial/indexado nativo do COBOL* (`ORGANIZATION INDEXED`, simulando VSAM): opção mais simples e totalmente nativa do COBOL, mas sem transações ACID nem consultas diretas por índice secundário — ficou registrada como plano B caso a configuração ODBC não fosse viável a tempo.
- *`EXEC SQL` embutido do GnuCOBOL*: **testado e descartado**. O pré-compilador SQL do GnuCOBOL (`-fsqldb`) suporta apenas MySQL, MSSQL e Oracle — não há suporte nativo a SQLite. Essa limitação foi confirmada experimentalmente antes de se prosseguir com a alternativa de wrapper C/ODBC.

**Justificativa da escolha:** SQLite oferece transações ACID (atendendo ao requisito de "persistir as alterações realizadas" de forma confiável) e consulta direta por chave, sem a complexidade de provisionar um SGBD completo. O acesso via wrapper C/ODBC foi validado experimentalmente (ver seção 4) e reproduz, em escala reduzida, o padrão real usado em integrações mainframe-banco de dados (rotina externa chamada via `CALL`, análoga a como um programa COBOL chamaria uma stored procedure ou um módulo de acesso a dados em ambiente z/OS).

### 3.3 Padrão de integração COBOL ↔ banco de dados

**Decisão:** rotinas em C (`sqlitebridge.c`) compiladas como objeto e linkadas ao executável COBOL, expondo funções (`CONSULTA_CLIENTE`, `ATUALIZA_CLIENTE`) chamadas via `CALL ... USING` com parâmetros passados por referência.

**Detalhes técnicos:**
- Cada função abre sua própria conexão ODBC (`SQLConnect` ao DSN `clientesDB`), executa a operação e libera os handles ao final — sem manter conexão persistente entre chamadas.
- `ATUALIZA_CLIENTE` desativa o autocommit (`SQL_ATTR_AUTOCOMMIT = SQL_AUTOCOMMIT_OFF`) e controla a transação manualmente, fazendo `COMMIT` apenas se a atualização afetar pelo menos uma linha (`SQLRowCount`), e `ROLLBACK` em caso de cliente não encontrado ou erro.
- Códigos de retorno padronizados em `PIC X(2)`, inspirados nos return codes COBOL tradicionais: `"00"` = sucesso, `"04"` = cliente não encontrado, `"08"` = erro de conexão/execução.

**Justificativa:** esse padrão isola toda a complexidade de acesso a dados em uma camada própria, deixando os programas COBOL "de aplicação" simples (apenas chamam a rotina e tratam o status de retorno), o que atende ao requisito não funcional de estrutura organizada e de fácil manutenção.

## 4. Validação experimental (testes de viabilidade técnica)

Antes de assumir essa arquitetura como definitiva, foram realizados testes isolados para confirmar viabilidade:

| Teste | Resultado |
|---|---|
| `EXEC SQL` com SQLite no GnuCOBOL | ❌ Não suportado (`-fsqldb` aceita apenas MySQL/MSSQL/Oracle) |
| Conexão ODBC ao SQLite via `isql` | ✅ Conectado e consulta retornada com sucesso |
| COBOL chamando wrapper C (`CONSULTA_CLIENTE`) | ✅ Dados retornados corretamente |
| COBOL chamando wrapper C (`ATUALIZA_CLIENTE`) | ✅ Persistência confirmada via consulta posterior ao banco |
| Cenário cliente não encontrado | ✅ Status `"04"` retornado corretamente, sem falso positivo |

Esses testes foram conduzidos no ambiente de desenvolvimento (WSL Ubuntu 24.04, GnuCOBOL 3.1.2.0) antes da implementação dos programas finais, reduzindo o risco de retrabalho arquitetural nas fases seguintes.

## 5. Estrutura de componentes (a atualizar)

| Componente | Responsabilidade | Status |
|---|---|---|
| `sqlitebridge.c` | Ponte ODBC entre COBOL e SQLite | ✅ Implementado e testado |
| Programa COBOL principal | Receber parâmetros do processo .NET e orquestrar consulta/atualização | ⏳ Pendente (Fase 5) |
| Estrutura compartilhada | Contrato de dados entre .NET e COBOL | ⏳ Pendente (Fase 4) |
| Aplicação .NET | Interface de atendimento, chamada do processo COBOL | ⏳ Pendente (Fase 6/7) |

## 6. Fluxo de execução (a detalhar conforme Fase 6)

Pendente: descrever passo a passo como uma requisição do atendente percorre .NET → processo COBOL → wrapper C → SQLite e retorna.

## 7. Riscos e mitigações registrados

- **Risco:** configuração ODBC/SQLite poderia não funcionar a tempo. **Mitigação planejada:** plano B com arquivo indexado COBOL (`ORGANIZATION INDEXED`), simulando VSAM. **Status:** mitigação não foi necessária — configuração ODBC validada com sucesso.