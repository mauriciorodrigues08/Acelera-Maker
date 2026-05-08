# Projeto Conta Bancária

Este é um sistema de console robusto desenvolvido em **C#** e **.NET**, focado nos pilares da Programação Orientada a Objetos (POO). O sistema permite o gerenciamento completo de contas bancárias (Corrente e Poupança), incluindo operações financeiras, histórico de transações e persistência de dados em arquivos JSON.

## Funcionalidades

* **Gerenciamento de Contas:** Cadastro, listagem, busca, atualização e exclusão de contas.
* **Tipos de Conta:** 
    * **Conta Corrente:** Possui limite de crédito adicional ao saldo.
    * **Conta Poupança:** Requer a definição do ano de aniversário da conta.
* **Operações Financeiras:**
    * Depósitos e Saques com validação de saldo/limite.
    * Transferências entre contas com confirmação de segurança.
* **Histórico de Transações:** Registro automático das últimas 10 movimentações de cada conta, detalhando valores e contrapartes.
* **Persistência de Dados:** Salvamento e carregamento automático via `contas.json` utilizando a biblioteca `System.Text.Json`.
* **Interface Colorida:** Menu interativo no console com feedback visual (cores ANSI) para erros, sucessos e avisos.

## Tecnologias Utilizadas

* **Linguagem:** C#
* **Framework:** .NET 10.0 (ou superior)
* **Formato de Dados:** JSON
* **Padrões de Projeto:** Repository Pattern (IContaRepository) e Controller Pattern.

## Estrutura do Projeto

O projeto está organizado no namespace `Projeto_Conta_Bancaria.Classes`:

* `Program.cs`: Ponto de entrada que inicializa o menu.
* `Menu.cs`: Gerencia a interação com o usuário e fluxos de entrada.
* `Conta.cs`: Classe abstrata base com lógica comum de saldo e transações.
* `ContaCorrente.cs` / `ContaPoupanca.cs`: Implementações específicas de regras de negócio.
* `ContaController.cs`: Centraliza a lógica de negócios, manipulação da coleção e persistência.
* `Transacao.cs`: Modelo para o registro de movimentações financeiras.
* `Cores.cs`: Utilitário para formatação estética do console (cores ANSI).

## Pré-requisitos

* [.NET SDK](https://dotnet.microsoft.com/download) instalado em sua máquina.
* Um terminal (PowerShell, CMD ou terminal do VS Code).

## Como Rodar o Projeto

1.  **Clone ou baixe** os arquivos do projeto para uma pasta local.
2.  Abra o terminal na pasta raiz do projeto (onde está o arquivo `.csproj` ou `Program.cs`).
3.  **Restaure as dependências**:
    ```bash
    dotnet restore
    ```
4.  **Execute a aplicação:**
    ```bash
    dotnet run
    ```

## Persistência

Ao iniciar, o sistema procura por um arquivo chamado `contas.json` na raiz do projeto. 
* Se o arquivo existir, os dados serão carregados automaticamente.
* A cada operação de cadastro, alteração ou transação financeira, o arquivo é atualizado de forma automática.

---
Desenvolvido por Maurício de Oliveira Santos Rodrigues.