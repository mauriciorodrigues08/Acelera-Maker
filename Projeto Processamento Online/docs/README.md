# Projeto COBOL/CICS — Sistema de Consulta e Atualização de Clientes

**Acelera Maker — Projeto 7 COBOL (Semana 8/9)**

---

## Contexto

Transação CICS chamada `CLIE`, que executa o programa `CLIPGM`, consultando
e atualizando clientes de uma instituição financeira fictícia, cadastrados
em um arquivo VSAM chamado `CLIENTES`.

Conforme orientação da mentora, **não há ambiente CICS real disponível**
para execução (o TK5 não suporta CICS). A entrega foi adaptada para chegar
"o mais próximo possível do código", treinando o mapa BMS mesmo sem
executá-lo, e produzindo uma versão executável equivalente em GnuCOBOL para
validar a lógica de negócio na prática.

---

## Decisões de Nomenclatura (fixadas pelo enunciado)

| Item         | Valor       |
|--------------|-------------|
| Transação    | `CLIE`      |
| Programa     | `CLIPGM`    |
| Arquivo VSAM | `CLIENTES`  |
| Mapa BMS     | `CLIEMAP`   |
| Mapset BMS   | `CLIESET`   |

---

## Layout do Arquivo VSAM — CLIENTES

| Campo    | Tipo          | Tamanho |
|----------|---------------|---------|
| CODCLI   | Numérico      | 6       |
| NOME     | Alfanumérico  | 30      |
| TELEFONE | Alfanumérico  | 15      |
| CIDADE   | Alfanumérico  | 20      |

**Tamanho total do registro:** 71 bytes
**Chave primária:** CODCLI (posição 1, tamanho 6)

---

## Tela Esperada (conforme enunciado)

```
Col: 1234567890123456789012345678901234567890
     ****************************************
     * CONSULTA DE CLIENTES                 *
     ****************************************

     Codigo Cliente: ______

     Nome.........: ______________________________

     Telefone......: _______________

     Cidade........: ____________________

     Mensagem......: ______________________________

     PF3=Sair
     PF5=Consultar
     PF6=Salvar
```

### Posicionamento exato na tela 3270 (80×24)

| Campo    | Linha | Col rótulo | Col campo | Tamanho | Atributo BMS          |
|----------|-------|------------|-----------|---------|------------------------|
| (borda)  | 1     | 1          | —         | 40      | `PROT`                 |
| (titulo) | 2     | 1–40       | —         | —       | `PROT`                 |
| (borda)  | 3     | 1          | —         | 40      | `PROT`                 |
| CODCLI   | 5     | 1          | 17        | 6       | `UNPROT,NUM`           |
| NOME     | 7     | 1          | 15        | 30      | `UNPROT`               |
| TELEFONE | 9     | 1          | 15        | 15      | `UNPROT`               |
| CIDADE   | 11    | 1          | 15        | 20      | `UNPROT`               |
| MENSAGEM | 13    | 1          | 15        | 30      | `PROT` (saída sistema) |
| PF3=Sair | 16    | 1          | —         | —       | `PROT`                 |
| PF5=...  | 17    | 1          | —         | —       | `PROT`                 |
| PF6=...  | 18    | 1          | —         | —       | `PROT`                 |

### Atributos dos campos

- **Rótulos e instruções:** `ATTRB=(NORM,PROT)` — usuário não digita
- **CODCLI:** `ATTRB=(NORM,UNPROT,NUM)` — editável, somente números
- **NOME:** `ATTRB=(NORM,UNPROT)` — editável, alfanumérico (somente exibição após PF5)
- **TELEFONE:** `ATTRB=(NORM,UNPROT)` — editável, alfanumérico
- **CIDADE:** `ATTRB=(NORM,UNPROT)` — editável, alfanumérico
- **MENSAGEM:** `ATTRB=(NORM,PROT)` — saída do sistema, somente leitura

> **Nota:** NOME é exibido como resultado de consulta. Apenas TELEFONE e CIDADE
> são atualizados pelo PF6, conforme regra do enunciado.

O mapa físico completo está em `src/CLIEMAP.bms` (macros `DFHMSD`, `DFHMDI`,
`DFHMDF`), e o mapa simbólico correspondente (normalmente gerado pelo
tradutor BMS) está em `src/CLIEMAP.cpy`.

---

## Mensagens Fixas do Sistema

```cobol
01  WS-MENSAGENS.
    05  MSG-ENCONTRADO      PIC X(30) VALUE 'CLIENTE ENCONTRADO'.
    05  MSG-NAO-ENCONTRADO  PIC X(30) VALUE 'CLIENTE NAO ENCONTRADO'.
    05  MSG-ALTERADO        PIC X(30) VALUE 'ALTERACAO REALIZADA'.
    05  MSG-CAMPO-OBRIG     PIC X(30) VALUE 'CODIGO OBRIGATORIO'.
    05  MSG-SEM-CONSULTA    PIC X(30) VALUE 'CONSULTE ANTES DE SALVAR'.
    05  MSG-OPCAO-INVALIDA  PIC X(30) VALUE 'OPCAO INVALIDA'.
    05  MSG-ERRO-VSAM       PIC X(30) VALUE 'ERRO AO ACESSAR ARQUIVO'.
```

As três primeiras (`ENCONTRADO`, `NAO ENCONTRADO`, `ALTERADO`) são exigidas
pelo enunciado. As demais (`CAMPO-OBRIG`, `SEM-CONSULTA`, `OPCAO-INVALIDA`,
`ERRO-VSAM`) são tratamentos de erro adicionados para tornar o programa mais
robusto.

---

## Regras de Negócio

### PF5 — Consultar
1. Verificar se CODCLI foi informado — senão: `CODIGO OBRIGATORIO`
2. `EXEC CICS READ FILE('CLIENTES') INTO(WS-REG-CLIENTE) RIDFLD(WS-CODCLI)`
3. Se não encontrado → exibir `CLIENTE NAO ENCONTRADO`
4. Se encontrado → preencher NOME, TELEFONE, CIDADE na tela → exibir `CLIENTE ENCONTRADO`

### PF6 — Salvar
1. Verificar se um cliente foi consultado antes (estado equivalente à
   COMMAREA) — senão: `CONSULTE ANTES DE SALVAR`
2. `EXEC CICS READ FILE('CLIENTES') ... UPDATE` (reserva o registro)
3. Mover TELEFONE e CIDADE digitados para o registro (apenas esses dois campos)
   — Enter em branco mantém o valor atual do campo (ver [bug corrigido](#bug-encontrado-e-corrigido))
4. `EXEC CICS REWRITE FILE('CLIENTES') FROM(WS-REG-CLIENTE)`
5. Se OK → exibir `ALTERACAO REALIZADA`
6. Se não encontrado → exibir `CLIENTE NAO ENCONTRADO`

### PF3 — Sair
- Encerra a transação. Na versão fiel ao CICS (`CLIPGM_CICS.cbl`), demonstra
  `EXEC CICS XCTL` transferindo controle a um programa de menu, em vez de um
  simples `RETURN`.

### Fluxograma
Ver `docs/fluxograma.svg` para o detalhamento visual dos fluxos PF5 e PF6.

---

## Estrutura de Pastas

```
Projeto Processamento Online/
├── Makefile                      # Compila, gera dados e roda os testes
├── testes.sh                     # Bateria de 8 testes automatizados
├── Projeto_7_Cobol_-_Semana_9.pdf
├── data/
│   ├── clientes_exemplo.txt      # Massa de dados de referência (legível)
│   └── clientes.dat              # Gerado pelo CARGA.cbl (não versionar manualmente)
├── docs/
│   ├── fluxograma.svg            # Fluxo PF5/PF6
│   └── README.md                 # Este arquivo
└── src/
    ├── CLIEMAP.bms               # Mapa BMS (artefato de design, não compilável)
    ├── CLIEMAP.cpy               # Mapa simbólico (equivalente ao gerado pelo tradutor BMS)
    ├── CLIPGM_CICS.cbl           # Versão "de papel", fiel ao CICS real (não compilável)
    ├── CARGA.cbl                 # Utilitário: cria data/clientes.dat com a massa de exemplo
    └── CLIPGM.cbl                # Versão EXECUTÁVEL no GnuCOBOL (entregável funcional)
```

---

## Estratégia de Simulação — Por que duas versões de CLIPGM?

O GnuCOBOL instalado neste ambiente **não tem suporte a arquivo indexado**
(ISAM/VBISAM/BDB), o que é comum em instalações padrão do open-cobol. Por
isso, foram criadas duas versões do programa, lado a lado:

| Arquivo            | O que é                                                        | Compila/Executa? |
|---------------------|------------------------------------------------------------------|-------------------|
| `CLIPGM_CICS.cbl`   | Código fiel ao CICS real (`EXEC CICS SEND/RECEIVE/READ/REWRITE/XCTL/RETURN`) | ❌ Não — sem tradutor CICS disponível. Artefato de estudo/comparação. |
| `CLIPGM.cbl`        | Mesma lógica de negócio, adaptada para rodar de fato no GnuCOBOL | ✅ Sim — testado e funcional |

**Equivalências entre as duas versões:**

| Conceito CICS (`CLIPGM_CICS.cbl`)        | Simulado em `CLIPGM.cbl` como                              |
|--------------------------------------------|--------------------------------------------------------------|
| `EXEC CICS SEND MAP`                       | Rotina `1000-EXIBIR-TELA` (`DISPLAY`)                        |
| `EXEC CICS RECEIVE MAP`                    | Rotina `2000-LER-OPCAO` (`ACCEPT`)                            |
| Arquivo VSAM `CLIENTES` (KSDS)              | Arquivo sequencial `data/clientes.dat` + tabela em memória   |
| `EXEC CICS READ FILE('CLIENTES')`           | Busca na tabela em memória por `CODCLI`                      |
| `EXEC CICS REWRITE FILE('CLIENTES')`        | Atualização da tabela (persistida no disco ao sair, PF3)     |
| `DFHCOMMAREA` (estado entre execuções)      | Variável `WS-POS-ENCONTRADA` (lembra o cliente "em tela")     |
| `EXEC CICS XCTL`                            | Não se aplica na versão executável; demonstrado apenas em `CLIPGM_CICS.cbl` (PF3 → `XCTL PROGRAM('MENUPGM')`) |

> O `CLIEMAP.cpy` (mapa simbólico) também é um artefato manual — em um
> ambiente real, ele seria *gerado* pelo tradutor BMS a partir do
> `CLIEMAP.bms`. Foi escrito à mão aqui apenas para permitir a leitura
> completa do `CLIPGM_CICS.cbl`.

---

## Como Compilar e Executar

### Opção 1 — Usando o Makefile (recomendado)

```bash
make            # compila, gera a massa de dados e roda os testes (testes.sh)
make build      # so compila CARGA e CLIPGM
make data       # gera/reseta data/clientes.dat
make run        # compila, gera dados e abre o CLIPGM interativo
make test       # roda a bateria de testes
make clean      # remove binarios e o arquivo de dados gerado
```

### Opção 2 — Manualmente

```bash
# A partir da raiz do projeto
# (o cobc gera o binario no diretorio ATUAL, mesmo apontando para src/*.cbl)

cobc -x src/CARGA.cbl
cobc -x src/CLIPGM.cbl
./CARGA
./CLIPGM
```

---

## Como Testar

```bash
bash testes.sh
# ou, via Makefile:
make test
```

O script `testes.sh` reseta a massa de dados e roda 8 cenários cobrindo as
regras de negócio do PF5, PF6 e PF3, comparando a saída com o resultado
esperado de cada um.

---

## Resultados dos Testes

Bateria executada e validada tanto em ambiente de desenvolvimento quanto no
ambiente local do aluno (WSL/VS Code):

| # | Cenário                                            | Esperado                                   | Resultado |
|---|------------------------------------------------------|-----------------------------------------------|:---:|
| 1 | Consultar cliente existente (000005)                  | `CLIENTE ENCONTRADO` + dados do cliente        | ✅ |
| 2 | Consultar código inexistente (000050)                  | `CLIENTE NAO ENCONTRADO`                        | ✅ |
| 3 | Salvar sem consultar antes (direto PF6)                | `CONSULTE ANTES DE SALVAR`                      | ✅ |
| 4 | Consultar → Salvar → Sair → reabrir → persistência     | `ALTERACAO REALIZADA` + dado batendo no arquivo | ✅ |
| 5 | Opção inválida (digitar "9")                            | `OPCAO INVALIDA`                                | ✅ |
| 6 | Código em branco no consultar                          | `CODIGO OBRIGATORIO`                            | ✅ |
| 7 | Consultar cliente A, depois B, salvar (só B deve mudar) | Apenas o último cliente consultado é alterado   | ✅ |
| 8 | Enter em branco no telefone/cidade do PF6               | Mantém os dados originais, sem apagar           | ✅ |

---

## Bug Encontrado e Corrigido

Durante os testes (cenário 8), identificou-se que apertar Enter em branco no
telefone/cidade do PF6 **apagava** o dado existente (sobrescrevia com
espaços) e ainda exibia `ALTERACAO REALIZADA` — uma perda de dados
silenciosa.

**Correção aplicada:** a digitação é capturada em variáveis auxiliares
(`WS-TELEFONE-DIGITADO` / `WS-CIDADE-DIGITADA`); se vier em branco, o valor
atual do campo é mantido em vez de sobrescrito. Reteste de regressão
completo confirmou que a correção não afetou os demais cenários.

---

## Conceitos do Enunciado e Onde Aparecem no Projeto

| Conceito pedido         | Onde aparece                                                       |
|---------------------------|------------------------------------------------------------------------|
| CICS / Transações          | `CLIPGM_CICS.cbl` (comentários introdutórios e estrutura geral)         |
| Programas online            | `CLIPGM_CICS.cbl` (modelo pseudo-conversacional) e `CLIPGM.cbl` (simulação) |
| BMS                          | `src/CLIEMAP.bms` (mapa físico) e `src/CLIEMAP.cpy` (mapa simbólico)     |
| COMMAREA                    | `WS-COMMAREA` / `DFHCOMMAREA` em `CLIPGM_CICS.cbl`; equivalente `WS-POS-ENCONTRADA` em `CLIPGM.cbl` |
| VSAM                         | Arquivo `CLIENTES`, layout documentado e simulado via `data/clientes.dat` |
| `EXEC CICS SEND`             | Rotina `1000-PRIMEIRA-EXECUCAO` / `2000-PROCESSAR-TECLA` em `CLIPGM_CICS.cbl` |
| `EXEC CICS RECEIVE`          | Rotina `2000-PROCESSAR-TECLA` em `CLIPGM_CICS.cbl`                       |
| `EXEC CICS RETURN`           | Rotinas `1000` e `2000` em `CLIPGM_CICS.cbl`                              |
| `EXEC CICS XCTL`             | Rotina `5000-SAIR` em `CLIPGM_CICS.cbl` (PF3)                             |
| `EXEC CICS READ`             | Rotinas `3000-CONSULTAR` e `4000-SALVAR` em `CLIPGM_CICS.cbl`             |
| `EXEC CICS REWRITE`          | Rotina `4000-SALVAR` em `CLIPGM_CICS.cbl`                                 |

---

## Ambiente Utilizado

- **Compilador:** GnuCOBOL (`cobc`) — sem suporte a ISAM, confirmado em testes
- **Editor:** VS Code com extensão COBOL
- **Sistema:** WSL (Windows Subsystem for Linux)
- **Simulação CICS:** sem ambiente real disponível (conforme orientação da mentora)

---

## Limitações Conhecidas

- O GnuCOBOL utilizado não compila comandos `EXEC CICS` nem o mapa BMS
  diretamente — por isso `CLIPGM_CICS.cbl` e `CLIEMAP.bms` são entregues
  como artefatos de design, não executáveis.
- A persistência das alterações do PF6 na versão executável (`CLIPGM.cbl`)
  só é gravada em disco ao sair da transação (PF3), já que o arquivo é
  tratado como sequencial + tabela em memória, e não como um arquivo
  indexado com gravação imediata por chave.

  ---

  ## Autor
  Maurício Rodrigues