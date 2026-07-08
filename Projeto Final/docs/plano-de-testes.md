# Plano de Testes

## 1. Objetivo

Verificar que as principais funcionalidades da solução estão implementadas corretamente e que futuras alterações não comprometam comportamentos já validados.

## 2. Estratégia de testes

A solução adota dois níveis de testes automatizados, complementados por testes manuais com evidências:

| Nível | Ferramenta | Escopo | Depende de infraestrutura? |
|---|---|---|---|
| Unitário | xUnit + Moq | Controller e Models isolados | Não |
| Integração | xUnit + WebApplicationFactory | Pipeline HTTP completo em memória | Não |
| Manual | Swagger UI | Fluxo real ponta a ponta | Sim (COBOL + SQLite) |

**Decisão arquitetural:** os testes automatizados mockam o `ICobolBridge`, isolando a lógica da API do processo COBOL. Isso permite rodar os testes em qualquer ambiente (incluindo CI) sem precisar do GnuCOBOL, ODBC ou SQLite instalados. Os testes manuais no Swagger cobrem o fluxo real com todos os componentes.

## 3. Casos de teste automatizados

### 3.1 Testes unitários — `CobolResponseTests`

| ID | Cenário | Entrada | Resultado esperado | Status |
|---|---|---|---|---|
| UT-01 | Status `"00"` → `Sucesso = true` | `Status = "00"` | `Sucesso=true`, `NaoEncontrado=false`, `Erro=false` | ✅ |
| UT-02 | Status `"04"` → `NaoEncontrado = true` | `Status = "04"` | `Sucesso=false`, `NaoEncontrado=true`, `Erro=false` | ✅ |
| UT-03 | Status `"08"` → `Erro = true` | `Status = "08"` | `Sucesso=false`, `NaoEncontrado=false`, `Erro=true` | ✅ |

### 3.2 Testes unitários — `ClientesControllerTests`

| ID | Cenário | Entrada | Resultado esperado | Status |
|---|---|---|---|---|
| UT-04 | Consulta cliente existente | Código 1, COBOL retorna dados | HTTP 200 + `ClienteDto` com dados corretos | ✅ |
| UT-05 | Consulta cliente não encontrado | Código 99, COBOL retorna status `"04"` | HTTP 404 | ✅ |
| UT-06 | Consulta com erro interno | Código 1, COBOL retorna status `"08"` | HTTP 500 | ✅ |
| UT-07 | Consulta com código zero | Código 0 | HTTP 400, COBOL não chamado | ✅ |
| UT-08 | Consulta com código negativo (-1) | Código -1 | HTTP 400, COBOL não chamado | ✅ |
| UT-09 | Consulta com código negativo (-99) | Código -99 | HTTP 400, COBOL não chamado | ✅ |
| UT-10 | Atualização com dados válidos | Código 1, telefone e e-mail válidos | HTTP 200 com mensagem de sucesso | ✅ |
| UT-11 | Atualização cliente não encontrado | Código 99, COBOL retorna status `"04"` | HTTP 404 | ✅ |
| UT-12 | Atualização com erro interno | Código 1, COBOL retorna status `"08"` | HTTP 500 | ✅ |
| UT-13 | Atualização com código zero | Código 0 | HTTP 400, COBOL não chamado | ✅ |
| UT-14 | Atualização com código negativo (-1) | Código -1 | HTTP 400, COBOL não chamado | ✅ |

### 3.3 Testes de integração — `ClientesApiIntegrationTests`

| ID | Cenário | Entrada | Resultado esperado | Status |
|---|---|---|---|---|
| IT-01 | GET cliente existente | `GET /clientes/1`, mock retorna dados | HTTP 200 + JSON com código, nome, telefone, e-mail | ✅ |
| IT-02 | GET cliente não encontrado | `GET /clientes/99`, mock retorna status `"04"` | HTTP 404 + mensagem "nao encontrado" | ✅ |
| IT-03 | GET erro interno | `GET /clientes/1`, mock retorna status `"08"` | HTTP 500 | ✅ |
| IT-04 | GET código zero | `GET /clientes/0` | HTTP 400 | ✅ |
| IT-05 | GET código negativo | `GET /clientes/-1` | HTTP 400 | ✅ |
| IT-06 | PUT dados válidos | `PUT /clientes/1` com telefone e e-mail válidos | HTTP 200 + mensagem "sucesso" | ✅ |
| IT-07 | PUT cliente não encontrado | `PUT /clientes/99`, mock retorna status `"04"` | HTTP 404 | ✅ |
| IT-08 | PUT e-mail inválido | `PUT /clientes/1` com e-mail sem `@` | HTTP 400, COBOL não chamado | ✅ |
| IT-09 | PUT telefone com letras | `PUT /clientes/1` com telefone contendo letras | HTTP 400, COBOL não chamado | ✅ |

**Total: 23 testes — 23 passando ✅**

## 4. Critérios de aceitação

| Critério | Descrição |
|---|---|
| Consulta bem-sucedida | `GET /clientes/{codigo}` retorna HTTP 200 com código, nome, telefone e e-mail |
| Cliente não encontrado | `GET /clientes/{codigo}` retorna HTTP 404 com mensagem descritiva |
| Atualização bem-sucedida | `PUT /clientes/{codigo}` retorna HTTP 200 e persiste os novos dados |
| Validação de entrada | Código inválido (≤0), e-mail sem `@` e telefone com letras retornam HTTP 400 sem chamar o COBOL |
| Erro interno tratado | Falha no sistema legado retorna HTTP 500 com mensagem, sem expor detalhes internos |
| Regressão | Todos os 23 testes automatizados devem passar após qualquer alteração |

## 5. Testes manuais — evidências

Testes executados via Swagger UI (`http://localhost:5210`) com todos os componentes rodando (WSL, GnuCOBOL 3.1.2.0, SQLite + ODBC).

### MT-01: Consulta de cliente existente
- **Entrada:** `GET /clientes/1`
- **Resultado esperado:** HTTP 200 com dados do cliente
- **Resultado obtido:** HTTP 200
```json
{
  "codigo": 1,
  "nome": "Joao Silva",
  "telefone": "11955554444",
  "email": "joao.atualizado@teste.com"
}
```
- **Status:** ✅

### MT-02: Atualização de telefone e e-mail
- **Entrada:** `PUT /clientes/1` com `{ "telefone": "11999990000", "email": "joao.novo@cooperativa.com" }`
- **Resultado esperado:** HTTP 200 com mensagem de sucesso
- **Resultado obtido:** HTTP 200
```json
{
  "mensagem": "Dados atualizados com sucesso."
}
```
- **Status:** ✅

### MT-03: Cliente não encontrado
- **Entrada:** `GET /clientes/99`
- **Resultado esperado:** HTTP 404 com mensagem descritiva
- **Resultado obtido:** HTTP 404
```json
{
  "mensagem": "Cliente nao encontrado."
}
```
- **Status:** ✅

## 6. Como executar os testes automatizados

```bash
cd dotnet/CooperativaAlfa.Tests
dotnet test --verbosity normal
```

Para ver a cobertura de código:

```bash
dotnet test --collect:"XPlat Code Coverage"
```

O relatório de cobertura é gerado em `TestResults/` no formato XML (Coverlet).