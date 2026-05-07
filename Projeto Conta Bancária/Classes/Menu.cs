namespace Projeto_Conta_Bancaria.Classes;

// imports
using System;
public class Menu
{
    // inicia um Menu
    public void Iniciar()
    {
        Console.Clear();
        
        // instancia a variável de controle
        ContaController controller = new ContaController();
        
        // tenta carregar o arquivo json
        if (controller.carregar())
        {
            // caso o arquivo exita, mostra a mensagem de retorno da função
            Cores.Write("\nPressione enter para continuar...");
            Console.ReadLine();
        }
        
        // variável auxiliar para opção
        int op;

        // loop principal
        do
        {
            Console.Clear();
            // exibe o menu e recebe a opção
            printMenu();
            while(!int.TryParse(Console.ReadLine(), out op) || op < 0 || op > 7)
            {
                Cores.Erro("Opção inválida!");
                Cores.Write("Digite novamente: ");
            }

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
                    buscar(controller);
                    break;

                // saque
                case 6:
                    sacar(controller);
                    break;

                // depósito
                case 7:
                    depositar(controller);
                    break;

                // transferência
                case 8:
                    transferir(controller);
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
        Cores.Info("5. BUSCAR CONTA");
        Cores.Info("6. REALIZAR SAQUE");
        Cores.Info("7. REALIZAR DEPÓSITO");
        Cores.Info("8. REALIZAR TRANSFERÊNCIA");
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
        
        // verifica se a string é nula ou apenas espaços
        while (string.IsNullOrWhiteSpace(titular))
        {
            Cores.Erro("Nome inválido!");
            Cores.Write("Insira o nome do Titular: ");
            titular = Console.ReadLine();
        }

        // gera o Número
        int numero = controller.gerarNumero();

        // gera a Agência
        int agencia = numero * 11;
        
        // recebe o saldo inicial
        float saldo;
        Cores.Write("Informe o saldo inicial: ");
        while (!float.TryParse(Console.ReadLine(), out saldo))
        {   
            Cores.Erro("Valor inválido! Digite novamente: ");
        }        

        // recebe o Tipo
        Cores.Write("Informe o Tipo da Nova Conta (1. Corrente ou 2. Poupança): ");
        int tipo;
        while (!int.TryParse(Console.ReadLine(), out tipo) || (tipo != 1 && tipo != 2))
        {
            Cores.Erro("Tipo inválido!");
            Cores.Write("Digite novamente: ");
        }

        // verifica o tipo fornceido
        if(tipo == 1) // corrente
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
        else // poupança
        {
            // cria os atributos de Conta Poupança
            Cores.Write("Informe o seu ano de Nascimento: ");
            int aniversario;
            while(!int.TryParse(Console.ReadLine(), out aniversario) || aniversario < 1900)
            {
                Cores.Erro("Número inválido!");
                Cores.Write("Digite novamente: ");        
            }


            // instancia a Nova Conta
            novaConta = new ContaPoupanca(numero, agencia, tipo, titular, saldo, aniversario);
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

    // recebe o número
    Cores.Write("Informe o número da conta: ");
    int numero;
    while(!int.TryParse(Console.ReadLine(), out numero))
    {
        Cores.Erro("Número inválido!");
        Cores.Write("Digite novamente: ");        
    }

    // busca a conta para saber o tipo e exibir os dados atuais
    Conta? conta = controller.buscarNaCollection(numero);
    if (conta == null)
    {
        Cores.Erro("\nConta não encontrada!");
        return;
    }

    // exibe os dados atuais
    conta.visualizar();

    // coleta o novo titular
    Cores.Write("Novo titular: ");
    string? titular = Console.ReadLine();
    
    // verifica se a string é nula ou apenas espaços
    while (string.IsNullOrWhiteSpace(titular)) 
    {
        Cores.Erro("Nome inválido!");
        Cores.Write("Insira o nome do Titular: ");
        titular = Console.ReadLine();
    }

    // coleta dados específicos por tipo
    if (conta.getTipo() == 1)
    {
        Cores.Write("Novo limite: ");
        float limite;
        while (!float.TryParse(Console.ReadLine(), out limite))
            Cores.Erro("Valor inválido! Digite novamente: ");

        // passa tudo pronto para o controller
        controller.atualizar(numero, titular, limite);
    }
    else
    {
        Cores.Write("Novo aniversário: ");
        int aniversario;
        while (!int.TryParse(Console.ReadLine(), out aniversario) || aniversario < 1900)
            Cores.Erro("Valor inválido! Digite novamente: ");

        // passa tudo pronto para o controller
        controller.atualizar(numero, titular, aniversario);
    }

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
        int numero;
        while(!int.TryParse(Console.ReadLine(), out numero))
        {
            Cores.Erro("Número inválido!");
            Cores.Write("Digite novamente: ");        
        }

        // passa para o controller
        controller.deletar(numero);

        Cores.Separador();
        Cores.Write("\nPressione enter para voltar ao menu...");
        Console.ReadLine();
    }

    private void buscar(ContaController controller)
    {
        Console.Clear();

        Cores.Cabecalho("- BUSCANDO CONTA -");

        // recebe o número da conta
        int numero;

        Cores.Write("Informe o número da conta: ");
        while(!int.TryParse(Console.ReadLine(), out numero))
        {
            Cores.Erro("Número inválido!");
            Cores.Write("Digite novamente: ");        
        }

        // chama a função passando o número        
        controller.procurarPorNumero(numero);

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
        int numero;
        while(!int.TryParse(Console.ReadLine(), out numero))
        {
            Cores.Erro("Número inválido!");
            Cores.Write("Digite novamente: ");        
        }
        
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
        int numero;
        while(!int.TryParse(Console.ReadLine(), out numero))
        {
            Cores.Erro("Número inválido!");
            Cores.Write("Digite novamente: ");        
        }

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
        int numeroOrigem;
        while(!int.TryParse(Console.ReadLine(), out numeroOrigem))
        {
            Cores.Erro("Número inválido!");
            Cores.Write("Digite novamente: ");        
        }


        // recebe a conta de destino
        Cores.Write("Informe o número da conta de Destino: ");
        int numeroDestino;
        while(!int.TryParse(Console.ReadLine(), out numeroDestino))
        {
            Cores.Erro("Número inválido!");
            Cores.Write("Digite novamente: ");        
        }

        
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