namespace Projeto_Conta_Bancaria.Classes;

// imports
using System;
public class Menu
{
    // atributos
    private int op;
    private ContaController controller = new ContaController();

    // Inicia um Menu
    public void Iniciar()
    {
        do
        {
            Console.Clear();
            // Exibe o menu e recebe a opção
            printMenu();
            op = Convert.ToInt32(Console.ReadLine());

            // Realiza a ação selecionada
            switch (op)
            {
                // Sair
                case 0:
                    Cores.Separador();
                    Cores.Info("Programa finalizado...");
                    break;

                // Cadastrar
                case 1:
                    cadastrar(controller);
                    break;

                // Listar
                case 2:
                    listar(controller);
                    break;

                // Atualizar
                case 3:
                    atualizar(controller);
                    break;

                // Deletar
                case 4:
                    deletar(controller);
                    break;

                // Saque
                case 5:
                    sacar(controller);
                    break;

                // Depósito
                case 6:
                    depositar(controller);
                    break;

                // Transferência
                case 7:
                    transferir(controller);
                    break;

                default:
                    Cores.Aviso("Opção Inválida!");
                    break;
            }

        } while(op != 0);
    }

    // Exibe o menu e recebe a opção desejada
    private void printMenu()
    {
        Cores.Cabecalho("MENU INICIAL");
        Cores.Info("1. CADASTRAR NOVA CONTA");
        Cores.Info("2. LISTAR TODAS AS CONTAS");
        Cores.Info("3. ATUALIZAR UMA CONTA");
        Cores.Info("4. DELETAR UMA CONTA");
        Cores.Info("5. REALIZAR SAQUE");
        Cores.Info("6. REALIZAR DEPÓSITO");
        Cores.Info("7. REALIZAR TRANSFERÊNCIA");
        Cores.Info("0. SAIR");
        Cores.Separador();
        Cores.Write("Escolha sua opção: ");
    }

    private void cadastrar(ContaController controller) 
    {
        // Declara a Nova Conta
        Conta novaConta;

        Console.Clear();
        Cores.Cabecalho("CADASTRANDO NOVA CONTA");

        // Recebe o nome do Titular
        Cores.Write("Insira o nome do Titular: ");
        string? titular = Console.ReadLine();
        if(titular == null)
        {
            Cores.Erro("Nome inválido!");
            return;
        }

        // Gera o Número
        int numero = controller.gerarNumero();

        // Gera a Agência
        int agencia = 0;
        
        // Inicia o saldo com 0
        float saldo = 0f;

        // Recebe o Tipo
        Cores.Write("Informe o Tipo da Nova Conta (1. Corrente ou 2. Poupança): ");
        int tipo = Convert.ToInt32(Console.ReadLine());

        // Verifica o tipo fornceido
        if(tipo == 1)
        {
            // Cria os atributos de Conta Corrente
            float limite = 100f;

            // Instancia a Nova Conta
            novaConta = new ContaCorrente(numero, agencia, tipo, titular, saldo, limite);
        }
        else if (tipo == 2)
        {
            // Cria os atributos de Conta Poupança
            Cores.Write("Informe o seu ano de Nascimento: ");
            int aniversario = Convert.ToInt32(Console.ReadLine());

            // Instancia a Nova Conta
            novaConta = new ContaPoupanca(numero, agencia, tipo, titular, saldo, aniversario);
        }
        else
        {
            Cores.Erro("Tipo de conta inválido!");
            return;
        }
        
        // Adiciona a Nova Conta à lista de Contas Cadastradas
        controller.cadastrar(novaConta);

        // Imprime os dados da conta criada
        novaConta.visualizar();

        Cores.Separador();
        Cores.Write("Pressione enter para voltar ao menu...");
        Console.ReadLine();
    }

    private void listar(ContaController controller) 
    {
        Console.Clear();
        Cores.Separador();
        Cores.Cabecalho("");
    }

    private void atualizar(ContaController controller) 
    {
        Console.Clear();
        Cores.Separador();
        Cores.Cabecalho("");
    }

    private void deletar(ContaController controller) 
    {
        Console.Clear();
        Cores.Separador();
        Cores.Cabecalho("");
    }

    private void sacar(ContaController controller) 
    {
        Console.Clear();
        Cores.Separador();
        Cores.Cabecalho("");
    }

    private void depositar(ContaController controller) 
    {
        Console.Clear();
        Cores.Separador();
        Cores.Cabecalho("");
    }

    private void transferir(ContaController controller) 
    {
        Console.Clear();
        Cores.Separador();
        Cores.Cabecalho("");
    }
}