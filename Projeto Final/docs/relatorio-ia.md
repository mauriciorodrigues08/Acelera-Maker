# Relatório de Utilização de Inteligência Artificial

## Ferramenta utilizada

**Claude (Anthropic)** — modelo de linguagem utilizado como assistente durante todo o desenvolvimento do projeto, desde o planejamento arquitetural até a implementação dos testes automatizados.

## Considerações gerais

O uso da IA foi intencional e crítico — não limitado a geração de código, mas também para discussão de alternativas arquiteturais, diagnóstico de erros, validação de decisões técnicas e documentação. Em todos os casos, as respostas foram analisadas antes de serem aplicadas, e em vários momentos a sugestão da IA foi adaptada ou substituída com base no contexto real do projeto.

---

## Prompt 1 — Planejamento arquitetural: comunicação entre .NET e COBOL

**Objetivo:** entender as opções disponíveis para fazer o .NET chamar o programa COBOL e escolher a mais adequada para o projeto.

**Prompt utilizado:**
> "Existe diferença de desempenho entre a comunicação via arquivo e via processo?"

**Resposta obtida:**
A IA detalhou três opções: comunicação via arquivo (lenta, risco de concorrência), chamada via `Process.Start` (custo de criação de processo a cada chamada, mas sem I/O de arquivo) e interop via DLL/biblioteca dinâmica (melhor desempenho, sem overhead de processo). Explicou que a opção de arquivo tem latência de I/O e exige mecanismo de lock, enquanto a via processo tem overhead de inicialização mas é mais simples e isolada.

**Análise crítica:**
A resposta foi tecnicamente precisa e bem estruturada. A comparação entre as três opções — não apenas as duas originalmente mencionadas — foi um valor agregado que não havia sido considerado. A terceira opção (DLL/interop) é de fato a mais performática, mas a IA também reconheceu que, para o escopo do projeto, o `Process.Start` é o melhor equilíbrio entre simplicidade e desempenho justificável.

**Impacto na solução:**
Adotou-se a chamada via `Process.Start`, com a opção de DLL documentada no `arquitetura.md` como possível evolução futura. Essa justificativa foi incorporada diretamente na seção 3.1 do documento de arquitetura.

---

## Prompt 2 — Viabilidade técnica: SQLite + ODBC no GnuCOBOL

**Objetivo:** verificar se o GnuCOBOL suportaria `EXEC SQL` com SQLite antes de comprometer a arquitetura com essa escolha.

**Prompt utilizado:**
> "Verifique a viabilidade do SQLite + ODBC no GnuCOBOL para realizar a persistência de dados"

**Resposta obtida:**
A IA configurou o ambiente, compilou o wrapper C, criou o banco SQLite e testou a conexão ODBC — e identificou que o `EXEC SQL` embutido do GnuCOBOL **não suporta SQLite** (apenas MySQL, MSSQL e Oracle). Propôs como alternativa um wrapper em C usando a API ODBC diretamente, chamado pelo COBOL via `CALL`.

**Análise crítica:**
Este foi o momento de maior valor do uso da IA no projeto. A limitação do `EXEC SQL` no GnuCOBOL não era documentada de forma clara e poderia ter causado retrabalho significativo se descoberta no meio da implementação. A IA não apenas identificou o problema, mas propôs e validou uma solução alternativa completa. O resultado foi testado de ponta a ponta antes de qualquer decisão ser tomada. Ponto de atenção: a solução via wrapper C adiciona uma dependência de compilação (gcc) que precisa estar documentada nos pré-requisitos — o que foi feito no `README.md`.

**Impacto na solução:**
A arquitetura de persistência foi definida com base nesse teste: wrapper C (`sqlitebridge.c`) com funções `CONSULTA_CLIENTE` e `ATUALIZA_CLIENTE`, chamadas via `CALL` do COBOL. Essa limitação foi documentada na seção 3.2 do `arquitetura.md` como decisão técnica justificada, não como restrição oculta.

---

## Prompt 3 — Decisão de ambiente: .NET no Windows vs. WSL

**Objetivo:** avaliar se seria possível usar o .NET já instalado no Windows para chamar o executável COBOL compilado no WSL.

**Prompt utilizado:**
> "Seria um problema utilizar o .NET no Windows e o programa COBOL ser compilado para Linux?"

**Resposta obtida:**
A IA explicou que o executável gerado pelo GnuCOBOL no WSL é um binário ELF (Linux) que o Windows não consegue executar diretamente via `Process.Start`. Apresentou três alternativas: instalar o .NET no WSL (recomendada), chamar o COBOL via `wsl.exe` a partir do .NET no Windows (frágil), ou compilar o COBOL para Windows com MinGW (trabalhoso).

**Análise crítica:**
A resposta foi direta e correta. A opção de usar `wsl.exe` como intermediário existe e funciona, mas a IA foi honesta ao qualificá-la como "frágil" — o que é verdade, pois dependeria de caminhos corretos entre os dois sistemas de arquivos e seria difícil de justificar arquiteturalmente. A recomendação de instalar o .NET no WSL é a mais limpa tecnicamente. A única ressalva é que o aluno já tinha o .NET instalado no Windows para outros projetos — o que tornaria essa mudança um custo de setup adicional, mas que foi considerado justificável.

**Impacto na solução:**
O .NET foi instalado no WSL, eliminando qualquer barreira entre os dois ambientes. Isso simplificou o `Process.Start` (sem intermediários), o gerenciamento de variáveis de ambiente (`ODBCINI`) e o acesso ao filesystem. A decisão foi documentada na seção 3.5 do `arquitetura.md`.

---

## Prompt 4 — Diagnóstico de erro: `ODBCINI` não definido

**Objetivo:** identificar por que o programa COBOL retornava `status "08"` (erro de conexão) no WSL, quando funcionava no ambiente de testes da IA.

**Prompt utilizado:**
> O que pode estar causando este erro? (print de tela mostrando `"status": "08"` em todas as operações)

**Resposta obtida:**
A IA identificou que o mais provável era a variável `ODBCINI` não estar definida na sessão atual do terminal, e que o `odbc.ini` provavelmente ainda apontava para o caminho antigo (`/caminho/completo/para/clientes.db`, placeholder que não foi substituído). Solicitou o resultado de `cat odbc.ini` e `echo $ODBCINI` para confirmar.

**Análise crítica:**
O diagnóstico foi correto — o `odbc.ini` ainda continha o caminho placeholder. A IA acertou a causa raiz sem acesso ao ambiente real, baseando-se nos prints de tela. Isso demonstra valor no uso da IA para diagnóstico de problemas de configuração de ambiente, onde a mensagem de erro por si só (`"Erro ao consultar o banco de dados"`) não é suficientemente descritiva. O processo de debug foi iterativo e educativo — cada passo foi validado antes do próximo.

**Impacto na solução:**
Além de resolver o problema imediato, esse diagnóstico motivou duas melhorias permanentes: (1) o `Makefile` passou a exportar `ODBCINI` automaticamente via `export ODBCINI = $(CURDIR)/odbc.ini`, eliminando a necessidade de export manual; (2) o `CobolBridge.cs` foi implementado para definir `ODBCINI` no `ProcessStartInfo.Environment`, garantindo que a API nunca dependa de variáveis de ambiente do terminal.

---

## Prompt 5 — Arquitetura de testes: mock do CobolBridge

**Objetivo:** decidir a estratégia de testes para a API integrada com COBOL, considerando que o executável COBOL não estará disponível em todos os ambientes.

**Prompt utilizado:**
> "Quais são as possibilidades de testes para a api integrada com cobol? É possível utilizar o SonarQube?"

**Resposta obtida:**
A IA apresentou três níveis de testes (unitário com mock, integração com WebApplicationFactory, e2e com todos os componentes), explicou que o SonarQube Community não suporta COBOL gratuitamente (confirmado via busca), e recomendou xUnit + Moq + WebApplicationFactory como abordagem principal para o escopo do projeto.

**Análise crítica:**
A resposta sobre o SonarQube foi verificada e confirmada — a limitação é real e está documentada pela própria Sonarsource. A recomendação de xUnit + WebApplicationFactory foi adequada ao escopo. Um ponto importante que surgiu durante a implementação e não foi antecipado na resposta inicial: os métodos do `CobolBridge` precisariam ser `virtual` (ou extrair uma interface) para o Moq conseguir mockálos. Isso gerou uma rodada extra de refatoração — o que, por outro lado, resultou em uma decisão arquitetural melhor (interface `ICobolBridge`), alinhada com o princípio de inversão de dependência.

**Impacto na solução:**
A estratégia de testes foi definida com base nessa resposta: 14 testes unitários + 9 de integração, todos independentes de infraestrutura. A extração da interface `ICobolBridge` — motivada pelo requisito de mockabilidade — foi documentada no `arquitetura.md` como decisão técnica com justificativa própria (não apenas como consequência dos testes).

---

## Resumo das interações

| # | Tema | Tipo de uso | Resposta adotada? |
|---|---|---|---|
| 1 | Comunicação .NET ↔ COBOL | Discussão arquitetural | ✅ Adotada (com documentação de evolução futura) |
| 2 | SQLite + ODBC no GnuCOBOL | Validação experimental | ✅ Adotada após confirmação nos testes |
| 3 | .NET no Windows vs. WSL | Decisão de ambiente | ✅ Adotada integralmente |
| 4 | Diagnóstico de erro ODBCINI | Debug de ambiente | ✅ Resolvido + melhorias permanentes |
| 5 | Estratégia de testes + SonarQube | Decisão técnica | ✅ Adotada com refatoração adicional |