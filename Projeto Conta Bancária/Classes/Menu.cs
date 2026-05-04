namespace Projeto_Conta_Bancaria.Classes;

// imports
using System;
public class Menu
{
    // inicia um Menu
    public void Iniciar()
    {
        // variável de controle
        ContaController controller = new ContaController();

        // variável auxiliar para opção
        int op;

        // loop principal
        do
        {
            Console.Clear();
            // exibe o menu e recebe a opção
            printMenu();
            op = Convert.ToInt32(Console.ReadLine());

            // realiza a ação selecionada
            switch (op)
            {
                // sair
                case 0:
                    Cores.Separador();
                    Cores.Info("\nPrograma finalizado...");
                    break;

                // cadastrar
                case 1:
                    cadastrar(controller);
                    break;

                // listar
                case 2:
                    listar(controller);
                    break;

                // atualizar
                case 3:
                    atualizar(controller);
                    break;

                // deletar
                case 4:
                    deletar(controller);
                    break;

                // saque
                case 5:
                    sacar(controller);
                    break;

                // depósito
                case 6:
                    depositar(controller);
                    break;

                // transferência
                case 7:
                    transferir(controller);
                    break;

                default:
                    Cores.Aviso("Opção Inválida!");
                    break;
            }

        } while(op != 0);
    }

    // exibe o menu e recebe a opção desejada
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
        // declara a Nova Conta
        Conta novaConta;

        Console.Clear();
        Cores.Cabecalho("CADASTRANDO NOVA CONTA");

        // recebe o nome do Titular
        Cores.Write("Insira o nome do Titular: ");
        string? titular = Console.ReadLine();
        if(titular == null)
        {
            Cores.Erro("Nome inválido!");
            return;
        }

        // gera o Número
        int numero = controller.gerarNumero();

        // gera a Agência
        int agencia = numero * 11;
        
        // inicia o saldo com 0
        float saldo = 0f;

        // recebe o Tipo
        Cores.Write("Informe o Tipo da Nova Conta (1. Corrente ou 2. Poupança): ");
        int tipo = Convert.ToInt32(Console.ReadLine());

        // verifica o tipo fornceido
        if(tipo == 1)
        {
            // cria os atributos de Conta Corrente
            Cores.Write("Informe o limite desejado: ");
            float limite;
            while (!float.TryParse(Console.ReadLine(), out limite))
            {   
                Cores.Erro("Valor inválido! Digite novamente: ");
            }

            // instancia a Nova Conta
            novaConta = new ContaCorrente(numero, agencia, tipo, titular, saldo, limite);
        }
        else if (tipo == 2)
        {
            // cria os atributos de Conta Poupança
            Cores.Write("Informe o seu ano de Nascimento: ");
            int aniversario = Convert.ToInt32(Console.ReadLine());

            // instancia a Nova Conta
            novaConta = new ContaPoupanca(numero, agencia, tipo, titular, saldo, aniversario);
        }
        else
        {
            Cores.Erro("Tipo de conta inválido!");
            return;
        }
        
        // adiciona a Nova Conta à lista de Contas Cadastradas
        controller.cadastrar(novaConta);

        // imprime os dados da conta criada
        novaConta.visualizar();

        Cores.Separador();
        Cores.Write("Pressione enter para voltar ao menu...");
        Console.ReadLine();
    }

    private void listar(ContaController controller) 
    {
        Console.Clear();
        Cores.Cabecalho("- LISTANDO CONTAS CADASTRADAS -");

        controller.listarTodas();

        Cores.Separador();
        Cores.Write("\nPressione enter para voltar ao menu...");
        Console.ReadLine();
    }

    private void atualizar(ContaController controller) 
    {
        Console.Clear();
        Cores.Cabecalho("- ATUALIZANDO CONTA -");

        // recebe o número da conta que será atualizada
        Cores.Write("Informe o número da conta: ");
        int numero = Convert.ToInt32(Console.ReadLine());

        // envia para o controller
        controller.atualizar(numero);
    
        Cores.Separador();
        Cores.Write("\nPressione enter para voltar ao menu...");
        Console.ReadLine();
    }

    private void deletar(ContaController controller) 
    {
        Console.Clear();
        Cores.Cabecalho("- DELETANDO CONTA -");

        // recebe numero da conta que será deletada
        Cores.Write("Informe o número da conta: ");
        int numero = Convert.ToInt32(Console.ReadLine());

        // passa para o controller
        controller.deletar(numero);

        Cores.Separador();
        Cores.Write("\nPressione enter para voltar ao menu...");
        Console.ReadLine();
    }

    private void sacar(ContaController controller) 
    {
        Console.Clear();
        Cores.Cabecalho("- REALIZANDO SAQUE -");

        // recebe o número da conta
        Cores.Write("Informe o número da conta: ");
        int numero = Convert.ToInt32(Console.ReadLine());

        // recebe o valor do saque
        Cores.Write("Informe o valor do saque: ");
        float valor;
        while (!float.TryParse(Console.ReadLine(), out valor))
        {   
            Cores.Erro("Valor inválido! Digite novamente: ");
        }

        // passa para o controller
        controller.sacar(numero, valor);

        Cores.Separador();
        Cores.Write("\nPressione enter para voltar ao menu...");
        Console.ReadLine();
    }

    private void depositar(ContaController controller) 
    {
        Console.Clear();
        Cores.Cabecalho("- REALIZANDO DEPÓSITO -");

        // recebe o número da conta
        Cores.Write("Informe o número da conta: ");
        int numero = Convert.ToInt32(Console.ReadLine());

        // recebe o valor do depósito
        Cores.Write("Informe o valor do depósito: ");
        float valor;
        while (!float.TryParse(Console.ReadLine(), out valor))
        {   
            Cores.Erro("Valor inválido! Digite novamente: ");
        }

        // passa para o controller
        controller.depositar(numero, valor);

        Cores.Separador();
        Cores.Write("\nPressione enter para voltar ao menu...");
        Console.ReadLine();
    }

    private void transferir(ContaController controller) 
    {
        Console.Clear();
        Cores.Cabecalho("- REALIZANDO TRANSFERÊNCIA -");

        // recebe a conta de origem
        Cores.Write("Informe o número da conta de Origem: ");
        int numeroOrigem = Convert.ToInt32(Console.ReadLine());

        // recebe a conta de destino
        Cores.Write("Informe o número da conta de Destino: ");
        int numeroDestino = Convert.ToInt32(Console.ReadLine());
        
        // recebe o valor da transação
        Cores.Write("Informe o valor da transação: ");
        float valor;
        while (!float.TryParse(Console.ReadLine(), out valor))
        {   
            Cores.Erro("Valor inválido! Digite novamente: ");
        }

        // passa para o controller
        controller.transferir(numeroOrigem, numeroDestino, valor);

        Cores.Separador();
        Cores.Write("\nPressione enter para voltar ao menu...");
        Console.ReadLine();
    }

}