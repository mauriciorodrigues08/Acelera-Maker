# Documento de Arquitetura

> Documento vivo — atualizado incrementalmente ao longo do desenvolvimento, registrando cada decisão técnica no momento em que foi tomada.

## 1. Contexto e objetivo

A Cooperativa Financeira Alfa mantém um sistema legado em COBOL responsável pelo processamento e armazenamento dos dados cadastrais de clientes. O objetivo deste projeto é construir uma nova camada de atendimento (.NET) que consulte e atualize esses dados **sem substituir** o componente legado, preservando-o como responsável pelo processamento e persistência.

## 2. Visão geral da arquitetura

```
[ Atendente ]
      │
      ▼
[ Aplicação .NET ]  ── chama via Process.Start ──▶  [ Executável COBOL ]
                             JSON (stdin)                    │
                             JSON (stdout)                   ▼
                                              [ LER_ENTRADA / ESCREVER_SAIDA ]
                                                             │
                                                             ▼
                                          [ CONSULTA_CLIENTE / ATUALIZA_CLIENTE ]
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
- *Arquivo sequencial/indexado nativo do COBOL* (`ORGANIZATION INDEXED`, simulando VSAM): opção mais simples e totalmente nativa do COBOL, mas sem transações ACID nem consultas diretas por índice — ficou registrada como plano B caso a configuração ODBC não fosse viável a tempo.
- *`EXEC SQL` embutido do GnuCOBOL*: **testado e descartado**. O pré-compilador SQL do GnuCOBOL (`-fsqldb`) suporta apenas MySQL, MSSQL e Oracle — não há suporte nativo a SQLite. Essa limitação foi confirmada experimentalmente antes de se prosseguir com a alternativa de wrapper C/ODBC.

**Justificativa da escolha:** SQLite oferece transações ACID (atendendo ao requisito de "persistir as alterações realizadas" de forma confiável) e consulta direta por chave, sem a complexidade de provisionar um SGBD completo. O acesso via wrapper C/ODBC reproduz, em escala reduzida, o padrão real usado em integrações mainframe-banco de dados (rotina externa chamada via `CALL`, análoga a como um programa COBOL chamaria um módulo de acesso a dados em ambiente z/OS).

### 3.3 Padrão de integração COBOL ↔ banco de dados

**Decisão:** rotinas em C (`sqlitebridge.c`) compiladas como objeto e linkadas ao executável COBOL, expondo funções chamadas via `CALL ... USING` com parâmetros passados por referência.

**Funções implementadas:**

| Função C | Chamada COBOL | Responsabilidade |
|---|---|---|
| `LER_ENTRADA` | `CALL 'LER_ENTRADA' USING ...` | Lê JSON do `stdin` e preenche campos COBOL |
| `ESCREVER_SAIDA` | `CALL 'ESCREVER_SAIDA' USING ...` | Monta JSON com os campos COBOL e escreve no `stdout` |
| `CONSULTA_CLIENTE` | `CALL 'CONSULTA_CLIENTE' USING ...` | Busca cliente por código via ODBC |
| `ATUALIZA_CLIENTE` | `CALL 'ATUALIZA_CLIENTE' USING ...` | Atualiza telefone e e-mail via ODBC com commit/rollback |

**Detalhes técnicos:**
- Cada função de banco abre sua própria conexão ODBC, executa a operação e libera os handles ao final — sem manter conexão persistente entre chamadas.
- `ATUALIZA_CLIENTE` desativa o autocommit (`SQL_ATTR_AUTOCOMMIT = SQL_AUTOCOMMIT_OFF`) e controla a transação manualmente, fazendo `COMMIT` apenas se a atualização afetar pelo menos uma linha (`SQLRowCount`), e `ROLLBACK` em caso de cliente não encontrado ou erro.
- Códigos de retorno padronizados em `PIC X(2)`, inspirados nos return codes COBOL tradicionais: `"00"` = sucesso, `"04"` = cliente não encontrado, `"08"` = erro de conexão/execução.

**Justificativa:** esse padrão isola toda a complexidade de acesso a dados e de I/O JSON em uma camada própria (o `sqlitebridge.c`), deixando o programa COBOL principal (`clientes.cob`) focado apenas na lógica de negócio — atende ao requisito não funcional de estrutura organizada e de fácil manutenção.

### 3.4 Formato de troca de dados entre .NET e COBOL

**Decisão:** JSON via `stdin`/`stdout` do processo COBOL.

**Alternativas consideradas:**
- *Argumentos de linha de comando*: simples de implementar, mas limitado em tamanho e não extensível — adicionar um campo novo exigiria alterar a assinatura do processo e recompilar ambos os lados.
- *Arquivo temporário*: mais fácil de depurar (o arquivo fica em disco e pode ser inspecionado), mas reintroduz I/O de disco a cada operação — justamente o problema que levou à rejeição da comunicação via arquivo na decisão 3.1.

**Justificativa da escolha:** JSON via `stdin`/`stdout` é o formato mais alinhado com o conceito de API REST estudado no curso (JSON como padrão de troca de dados), extensível sem quebrar versões anteriores, e não adiciona I/O de disco. A estrutura completa dos JSONs de entrada e saída está definida em [`estrutura-compartilhada.md`](./estrutura-compartilhada.md).

### 3.5 Tipo de projeto .NET e ambiente de execução

**Decisão:** Web API ASP.NET Core 8, rodando no WSL (mesmo ambiente do componente COBOL).

**Alternativas consideradas:**
- *Console App*: simples de implementar, mas não atende ao requisito de "interface que facilite a utilização pelo usuário" nem ao de "garantir que a solução possa ser utilizada futuramente por outras aplicações".
- *.NET no Windows chamando o COBOL via WSL (`wsl.exe ...`)*: possível, mas frágil — depende de caminhos corretos entre os dois sistemas de arquivos e adiciona uma camada extra sem benefício técnico.
- *Compilar o COBOL para Windows (GnuCOBOL + MinGW)*: viável, mas repetiria todo o trabalho de configuração do ODBC já validado no Linux.

**Justificativa da escolha:** rodar o .NET no WSL elimina a barreira entre os dois ambientes — o `Process.Start` chama o executável COBOL diretamente, sem intermediários, com o mesmo filesystem e as mesmas variáveis de ambiente. A Web API expõe os endpoints via HTTP, tornando a solução consumível por qualquer cliente (browser, mobile, outro sistema), atendendo diretamente ao requisito de "futura integração com outros sistemas".

### 3.6 Estrutura da Web API (.NET)

**Decisão:** organização em três camadas dentro do projeto `CooperativaAlfa`:

| Camada | Arquivo | Responsabilidade |
|---|---|---|
| Models | `ClienteDto.cs` | Representa os dados do cliente na resposta HTTP |
| Models | `AtualizaClienteRequest.cs` | Payload do `PUT` com validações via DataAnnotations |
| Models | `CobolResponse.cs` | Desserializa o JSON retornado pelo processo COBOL |
| Services | `CobolBridge.cs` | Encapsula toda a lógica de `Process.Start`, stdin/stdout e erros |
| Controllers | `ClientesController.cs` | Mapeia rotas HTTP para chamadas ao `CobolBridge` |

**Detalhe crítico — variável `ODBCINI`:** o processo COBOL filho não herda automaticamente as variáveis de ambiente da sessão do terminal. Por isso, o `CobolBridge` define explicitamente `ODBCINI` no `ProcessStartInfo.Environment` antes de iniciar o processo, garantindo que o driver ODBC consiga localizar o DSN independentemente de como a API foi iniciada.

**Mapeamento de status COBOL → HTTP:**

| Status COBOL | HTTP Status | Significado |
|---|---|---|
| `"00"` | 200 OK | Operação realizada com sucesso |
| `"04"` | 404 Not Found | Cliente não encontrado |
| `"08"` | 500 Internal Server Error | Erro interno no sistema legado |

### 3.7 Automação de build (Makefile)

**Decisão:** uso de `Makefile` para automatizar compilação, inicialização do banco e execução de testes.

**Justificativa:** o processo de compilação envolve dois passos distintos (`gcc` para o wrapper C e `cobc` para o COBOL) com flags e dependências específicas. Sem automação, qualquer erro nos comandos ou esquecimento da ordem correta quebraria o build. O `Makefile` resolve isso e também exporta automaticamente a variável `ODBCINI`, eliminando uma fonte comum de erro durante o desenvolvimento. A adoção de `Makefile` está alinhada com os conceitos de DevOps e automação de build estudados no curso (Semana 10, Dia 4).

**Alvos disponíveis:**

| Alvo | Descrição |
|---|---|
| `make` / `make all` | Compila o wrapper C e o programa COBOL, gerando `build/clientes` |
| `make clean` | Remove todos os artefatos de `build/` |
| `make db-init` | Cria a tabela e popula o banco com 3 clientes de exemplo |
| `make run-consulta` | Testa consulta de cliente existente (código 1) |
| `make run-atualiza` | Testa atualização de telefone/e-mail e confirma com consulta |
| `make run-nao-encontrado` | Testa resposta para cliente inexistente (código 99) |
| `make test` | Executa todos os testes acima em sequência |

**Comportamento de recompilação incremental:** o `make` recompila apenas o arquivo modificado — se só o `clientes.cob` for alterado, o `sqlitebridge.c` não é recompilado, e vice-versa.

## 4. Programa COBOL principal (clientes.cob)

O programa `clientes.cob` é o ponto de entrada do componente legado. Ele é responsável por:

1. Chamar `LER_ENTRADA` para ler e interpretar o JSON recebido via `stdin`
2. Identificar a operação solicitada (`"C"` = consulta, `"A"` = atualização) usando level-88 (`88 OP-CONSULTA VALUE 'C'`)
3. Direcionar para o parágrafo correspondente (`EXECUTAR-CONSULTA` ou `EXECUTAR-ATUALIZA`) via `EVALUATE TRUE`
4. Chamar `ESCREVER_SAIDA` para formatar e emitir o JSON de resposta via `stdout`

**Estrutura de parágrafos:**

| Parágrafo | Responsabilidade |
|---|---|
| `PROCEDURE DIVISION` principal | Orquestração geral: leitura → decisão → escrita |
| `EXECUTAR-CONSULTA` | Chama `CONSULTA_CLIENTE` e define mensagem/flag de retorno |
| `EXECUTAR-ATUALIZA` | Chama `ATUALIZA_CLIENTE` e define mensagem/flag de retorno |
| `ESCREVER-E-SAIR` | Trata erros críticos (leitura inválida, operação desconhecida) e encerra |

**Uso de level-88 para legibilidade:** em vez de comparar strings diretamente (`IF WS-STATUS = "00"`), o programa declara condições nomeadas (`88 STATUS-OK VALUE '00'`) e usa `EVALUATE TRUE / WHEN STATUS-OK`, tornando o código mais próximo da linguagem de negócio e mais fácil de manter.

## 5. Validação experimental

| Teste | Ambiente | Resultado |
|---|---|---|
| `EXEC SQL` com SQLite no GnuCOBOL | WSL Ubuntu 24.04, GnuCOBOL 3.1.2.0 | ❌ Não suportado |
| Conexão ODBC ao SQLite via `isql` | WSL Ubuntu 24.04 | ✅ Sucesso |
| COBOL chamando `CONSULTA_CLIENTE` | WSL Ubuntu 24.04 | ✅ Dados retornados corretamente |
| COBOL chamando `ATUALIZA_CLIENTE` | WSL Ubuntu 24.04 | ✅ Persistência confirmada |
| Cenário cliente não encontrado | WSL Ubuntu 24.04 | ✅ Status `"04"` retornado corretamente |
| Programa principal lendo JSON do `stdin` | WSL Ubuntu 24.04 | ✅ Todos os cenários validados |
| Programa principal escrevendo JSON no `stdout` | WSL Ubuntu 24.04 | ✅ JSON bem formado em todos os cenários |
| `make test` completo | WSL Ubuntu 24.04 | ✅ Todos os alvos executados com sucesso |
| `GET /clientes/1` via Swagger | WSL .NET 8.0.422 + browser Windows | ✅ HTTP 200, dados do cliente retornados |
| `PUT /clientes/1` via Swagger | WSL .NET 8.0.422 + browser Windows | ✅ HTTP 200, "Dados atualizados com sucesso." |
| `GET /clientes/99` via Swagger | WSL .NET 8.0.422 + browser Windows | ✅ HTTP 404, "Cliente nao encontrado." |
| `ODBCINI` definido pelo `CobolBridge` no `ProcessStartInfo` | WSL | ✅ Processo COBOL conectou ao SQLite sem export manual |

## 6. Estrutura de componentes

| Componente | Arquivo | Responsabilidade | Status |
|---|---|---|---|
| Wrapper ODBC + I/O JSON | `cobol/sqlitebridge.c` | Leitura/escrita JSON + acesso ao SQLite via ODBC | ✅ Implementado e testado |
| Programa COBOL principal | `cobol/clientes.cob` | Orquestrar consulta/atualização conforme operação recebida | ✅ Implementado e testado |
| Banco de dados | `data/clientes.db` | Persistência dos dados cadastrais | ✅ Criado via `make db-init` |
| Automação de build | `Makefile` | Compilação, inicialização do banco e testes | ✅ Implementado e testado |
| Estrutura compartilhada | `docs/estrutura-compartilhada.md` | Contrato de dados entre .NET e COBOL | ✅ Documentado |
| Web API .NET | `dotnet/CooperativaAlfa/` | Interface HTTP, `CobolBridge`, endpoints GET e PUT | ✅ Implementado e testado |

## 8. Riscos e mitigações

| Risco | Mitigação planejada | Status |
|---|---|---|
| Configuração ODBC/SQLite inviável a tempo | Plano B: arquivo indexado COBOL (`ORGANIZATION INDEXED`) | ✅ Não foi necessário — ODBC validado |
| `EXEC SQL` sem suporte a SQLite no GnuCOBOL | Wrapper C/ODBC como camada de acesso | ✅ Implementado e funcionando |
| Variável `ODBCINI` não definida em sessão nova | `Makefile` exporta automaticamente antes de cada execução | ✅ Resolvido no Makefile |
| .NET precisa definir `ODBCINI` ao chamar o processo COBOL | Setar variável de ambiente no `ProcessStartInfo` | ✅ Implementado no CobolBridge |

## 7. Fluxo de execução completo

Sequência de uma requisição de consulta (`GET /clientes/1`) percorrendo toda a solução:

```
1. Atendente abre o browser e acessa http://localhost:5210
2. Swagger renderiza a interface — atendente clica em GET /clientes/{codigo}
3. HTTP GET /clientes/1 chega ao ClientesController (.NET)
4. Controller valida o parâmetro (codigo > 0) e chama CobolBridge.ConsultarClienteAsync(1)
5. CobolBridge serializa o JSON de entrada: {"operacao":"C","codigo":1}
6. CobolBridge cria um ProcessStartInfo apontando para build/clientes
   - RedirectStandardInput = true
   - RedirectStandardOutput = true
   - Environment["ODBCINI"] = caminho do odbc.ini
7. Process.Start() inicia o executável COBOL como processo filho
8. CobolBridge escreve o JSON no stdin do processo e fecha o stream
9. clientes.cob recebe o JSON via LER_ENTRADA (função C)
   - Extrai operacao = "C" e codigo = 000000001
10. EVALUATE TRUE → WHEN OP-CONSULTA → PERFORM EXECUTAR-CONSULTA
11. EXECUTAR-CONSULTA chama CONSULTA_CLIENTE (função C via CALL)
12. CONSULTA_CLIENTE abre conexão ODBC com o DSN "clientesDB"
    - SQLConnect → driver SQLite → abre data/clientes.db
    - SELECT nome, telefone, email WHERE codigo = 1
    - SQLFetch → retorna os dados do cliente
    - SQLDisconnect → libera handles
    - Status retornado: "00"
13. clientes.cob preenche WS-MENSAGEM = "Cliente encontrado." e WS-INCLUIR-DADOS = "S"
14. Chama ESCREVER_SAIDA (função C) que monta e imprime o JSON no stdout
15. CobolBridge lê o stdout do processo e aguarda o encerramento (WaitForExitAsync)
16. CobolBridge desserializa o JSON em CobolResponse
17. ClientesController mapeia CobolResponse para ClienteDto e retorna HTTP 200
18. Swagger exibe o JSON de resposta para o atendente
```

O fluxo de atualização (`PUT /clientes/1`) segue o mesmo caminho, com `operacao = "A"` e a chamada sendo direcionada para `ATUALIZA_CLIENTE`, que executa um `UPDATE` transacional com commit explícito.

## 8. Testes automatizados
 
### Estratégia adotada
 
O projeto adota dois níveis de testes automatizados com xUnit, sem dependência de infraestrutura (COBOL, SQLite ou ODBC):
 
**Testes unitários (xUnit + Moq):** testam `ClientesController` e `CobolResponse` isoladamente. O `ICobolBridge` é mockado, permitindo controlar exatamente o que o COBOL "retornaria" em cada cenário sem precisar do executável real.
 
**Testes de integração (xUnit + WebApplicationFactory):** sobem a API ASP.NET Core em memória e fazem requisições HTTP reais. O `ICobolBridge` é substituído no container de DI por um mock, testando o pipeline HTTP completo (roteamento, validação de ModelState, serialização JSON) sem dependência externa.
 
### Decisão: interface `ICobolBridge`
 
Para permitir o mock do `CobolBridge` nos testes, foi extraída a interface `ICobolBridge` com os métodos `ConsultarClienteAsync` e `AtualizarClienteAsync`. O controller passou a depender da interface em vez da classe concreta, seguindo o princípio de inversão de dependência. Isso também facilita futura substituição da implementação (ex.: trocar `Process.Start` por chamada direta a uma DLL COBOL).
 
### Resultado
 
| Nível | Testes | Passando |
|---|---|---|
| Unitário | 14 | ✅ 14 |
| Integração | 9 | ✅ 9 |
| **Total** | **23** | **✅ 23** |