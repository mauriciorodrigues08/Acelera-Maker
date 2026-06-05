# Processamento de Contas Bancárias - Projeto 4 (Semana 6)

Este projeto foi desenvolvido como parte dos desafios práticos do programa **Acelera Maker (Montreal)**. O objetivo consiste em construir uma rotina completa de processamento em lote (Batch) no ambiente Mainframe (MVS 3.8 / TK5) utilizando **COBOL** e **JCL**.

A aplicação simula a rotina de um banco, lendo arquivos de clientes, ordenando os registros, processando as quebras de controle (Control Break) e gerando um relatório consolidado de estatísticas bancárias com validações de saldos.

---

## 🚀 Funcionalidades e Desafios Extras Concluídos

Todos os requisitos obrigatórios foram implementados, além do cumprimento com sucesso de todos os desafios extras propostos para a semana:

*   **Concatenação de Arquivos:** O sistema recebe dois arquivos de entrada (`ARQCLIEN.TXT` e `ARQNOVOS.TXT`) e os une no momento da execução.
*   **Ordenação Dinâmica (SORT):** Utilização do utilitário `SORT` via JCL para organizar todas as contas em ordem crescente de Agência antes do processamento.
*   **Processamento e Quebra de Controle (Control Break):** O programa COBOL identifica mudanças de agência na leitura sequencial e gera subtotais automaticamente.
*   **Máscaras e Tratamento de Sinais:** Tratamento de valores monetários com variáveis `S9(09)V99` e impressão de saldos negativos com a máscara `-ZZZ,ZZZ,ZZ9.99`.
*   **Geração de Relatório Físico:** Em vez de utilizar apenas o comando `DISPLAY`, a saída foi roteada para a criação de um arquivo de relatório formatado (`SAIDAREL.TXT`).
*   **Estatísticas Gerais:** Cálculo e impressão no rodapé do total de contas processadas e do saldo total geral da instituição.

---

## 🛠️ Tecnologias Utilizadas

*   **Linguagem:** COBOL (Compilador IKFCBL00)
*   **Controle de Job:** JCL (Job Control Language)
*   **Ambiente:** Mainframe MVS 3.8 / Emulador TK5
*   **Utilitários Mainframe:** IEWL (Linkage Editor), SORT

---

## 📂 Estrutura de Arquivos

Abaixo está o mapeamento dos Data Sets (arquivos) criados no Mainframe:

| Data Set / Arquivo | Descrição |
| :--- | :--- |
| `HERC01.ARQCLIEN.TXT` | Arquivo texto principal contendo a base de contas correntes e poupanças. |
| `HERC01.ARQNOVOS.TXT` | Arquivo texto extra para teste de concatenação. |
| `HERC01.COBOL.SOURCE(GERASALD)` | Código fonte do programa COBOL principal. |
| `HERC01.JCL(COMPILAR)` | Script JCL responsável por compilar o COBOL e gerar o *Load Module* (Executável). |
| `HERC01.JCL(EXECUTA)` | Script JCL que executa o `SORT`, passa os dados para o programa COBOL e gera o relatório. |
| `HERC01.LOAD(GERASALD)` | Biblioteca de executáveis onde o programa compilado reside. |
| `HERC01.SAIDAREL.TXT` | Arquivo gerado automaticamente contendo o relatório final formatado. |

---

## 📸 Execução do Projeto

Abaixo estão as imagend da execução com sucesso do programa no ambiente TSO, incluindo a compilação, o retorno dos Jobs e o relatório gerado.

*(Insira seus prints abaixo substituindo os textos e links)*

**1. Arquivos de Entrada Criados e Preenchidos:**
![Print dos arquivos de texto no ISPF](img/arquivo%20de%20entrada%20de%20dados%20principal.png)
![Print dos arquivos de texto no ISPF](img/arquivo%20de%20entrada%20de%20dados%20extra.png)

**2. Sucesso na Compilação (RC=0000 / RC=0004):**
![Print do JOB de compilação no Outlist](img/compilacao%20sem%20erros.png)

**3. Execução do SORT e Programa COBOL (RC=0000):**
![Print do JOB de execução no Outlist mostrando STEP01 e STEP02](img/execucao%20sem%20erros.png)

**4. Relatório Final Gerado e Formatado:**
![Print do arquivo SAIDAREL.TXT aberto no Browse](img/resultado%20parte%201.png)
![Print do arquivo SAIDAREL.TXT aberto no Browse](img/resultado%20parte%202.png)

---

## 👤 Autor

*   **Maurício de Oliveira Santos Rodrigues**