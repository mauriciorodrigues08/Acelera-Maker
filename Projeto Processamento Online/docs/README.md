# Projeto COBOL/CICS — Sistema de Consulta e Atualização de Clientes

## Contexto

Transação CICS chamada `CLIE` que executa o programa `CLIPGM`.
Ambiente acadêmico — código fiel à estrutura mainframe, **não executável em CICS real**
(sem tradutor CICS/BMS disponível). Apresentado como artefato de design.

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

---

## Mensagens Fixas do Sistema

```cobol
01  WS-MENSAGENS.
    05  MSG-ENCONTRADO     PIC X(30) VALUE 'CLIENTE ENCONTRADO'.
    05  MSG-NAO-ENCONTRADO PIC X(30) VALUE 'CLIENTE NAO ENCONTRADO'.
    05  MSG-ALTERADO       PIC X(30) VALUE 'ALTERACAO REALIZADA'.
    05  MSG-ERRO-VSAM      PIC X(30) VALUE 'ERRO AO ACESSAR ARQUIVO'.
    05  MSG-CAMPO-OBRIG    PIC X(30) VALUE 'CODIGO OBRIGATORIO'.
```

---

## Regras de Negócio

### PF5 — Consultar
1. Verificar se CODCLI foi informado — senão: `CODIGO OBRIGATORIO`
2. `EXEC CICS READ FILE('CLIENTES') INTO(WS-REG-CLIENTE) RIDFLD(WS-CODCLI)`
3. Se NOTFND → exibir `CLIENTE NAO ENCONTRADO`
4. Se OK → preencher NOME, TELEFONE, CIDADE na tela → exibir `CLIENTE ENCONTRADO`

### PF6 — Salvar
1. Verificar se CODCLI foi informado — senão: `CODIGO OBRIGATORIO`
2. `EXEC CICS READ UPDATE FILE('CLIENTES') INTO(WS-REG-CLIENTE) RIDFLD(WS-CODCLI)`
3. Mover TELEFONE e CIDADE da tela para o registro (apenas esses dois campos)
4. `EXEC CICS REWRITE FILE('CLIENTES') FROM(WS-REG-CLIENTE)`
5. Se OK → exibir `ALTERACAO REALIZADA`
6. Se NOTFND → exibir `CLIENTE NAO ENCONTRADO`

### PF3 — Sair
- `EXEC CICS RETURN`

---

## Estratégia de Simulação CICS

O professor informou que **não há ambiente CICS disponível** para execução real.
Estratégia adotada:

| Artefato              | Abordagem                                                  |
|-----------------------|------------------------------------------------------------|
| `CLIEMAP.bms`         | Código BMS completo como **artefato de design** (não compilável) |
| `CLIPGM.cbl`          | COBOL com `EXEC CICS` nos comentários e lógica equivalente |
| `CLIPGM_CICS.cbl`     | Copybook com estruturas de COMMAREA e área de mapa simulada |

Modelo **Pseudo-Conversational**: a cada tecla PF, o programa termina com
`EXEC CICS RETURN TRANSID('CLIE') COMMAREA(WS-COMMAREA)` passando o estado
para a próxima execução via DFHCOMMAREA.

---

## Estrutura de Pastas

```
Projeto Processamento/
├── data/
│   └── clientes_exemplo.txt      # Massa de dados de referência
├── docs/
│   ├── fluxograma.png            # Fluxo PF5/PF6
│   └── README.md                 # Este arquivo
└── src/
    ├── CLIEMAP.bms               # Mapa BMS (artefato de design)
    ├── CLIPGM.cbl                # Programa principal COBOL/CICS
    └── CLIPGM_CICS.cbl           # Copybooks e estruturas CICS simuladas
```

---

## Ambiente

- **Compilador:** GnuCOBOL 3.1.2 (`cobc`) — testado com Hello World ✅
- **Editor:** VS Code com extensão COBOL
- **Simulação CICS:** sem ambiente real (conforme orientação do professor)
