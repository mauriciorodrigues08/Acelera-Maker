# 💼 Salário Final — Projeto COBOL

Programa desenvolvido em **COBOL** que calcula o salário final de um funcionário com base no salário base e no tempo de empresa, aplicando regras de bônus por faixa de tempo.

---

## 📋 Sobre o Projeto

Este projeto foi desenvolvido com o objetivo de praticar o uso de parágrafos e `PERFORM` na `PROCEDURE DIVISION` para organizar a lógica de forma modular.

### Funcionalidades

- Menu interativo com opções de calcular e sair
- Entrada de dados do funcionário (nome, salário base, tempo de empresa)
- Validação dos dados informados
- Cálculo de bônus por faixa de tempo de empresa:

| Tempo de Empresa | Bônus |
|---|---|
| Até 1 ano (≤ 12 meses) | 5% |
| De 1 a 5 anos (13–60 meses) | 10% |
| Acima de 5 anos (> 60 meses) | 15% |

- Exibição formatada do resultado (nome, salário base, bônus e salário final)

---

## 🗂️ Estrutura do Programa

```
MAIN-PROCEDURE
├── IMPRIME-MENU
├── ENTRADA-DADOS
├── VALIDA-DADOS
├── CALCULA-BONUS
├── CALCULA-SALARIO
└── EXIBE-RESULTADO
```

---

## 🛠️ Pré-requisitos

Para compilar e executar o programa, é necessário ter instalado o compilador **GnuCOBOL** (também conhecido como `cobc`).

### Instalação do GnuCOBOL

**Ubuntu / Debian:**
```bash
sudo apt update
sudo apt install gnucobol
```

**Fedora / RHEL:**
```bash
sudo dnf install gnucobol
```

**macOS (via Homebrew):**
```bash
brew install gnu-cobol
```

**Windows:**
Baixe o instalador pelo site oficial: https://gnucobol.sourceforge.io

---

## ⚙️ Compilação e Execução

### 1. Compilar o programa

```bash
cobc -x -o SalarioFinal SalarioFinal.cbl
```

> `-x` indica que deve gerar um executável; `-o SalarioFinal` define o nome do arquivo de saída.

### 2. Executar o programa

**Linux / macOS:**
```bash
./SalarioFinal
```

**Windows:**
```bash
SalarioFinal.exe
```

---

## 🖥️ Exemplo de uso

```
============== MENU ==============
 1. CALCULAR BONUS
 2. SAIR
==================================
INFORME SUA OPCAO: 1
INFORME O NOME: João Silva
INFORME O SALARIO BASE: R$3500.00
INFORME O TEMPO DE EMPRESA (EM MESES): 36

=========== RESULTADO ============
 NOME: João Silva
 SALARIO BASE: R$  3500.00
 BONUS: R$ 350.00
 SALARIO FINAL: R$ 3850.00
==================================
```

---

## 📁 Arquivos

```
.
├── SalarioFinal.cbl   # Código-fonte COBOL
└── README.md          # Este arquivo
```
