namespace Projeto_Conta_Bancaria.Classes;

// imports
using System.Collections.Generic;

public class ContaController : IContaRepository
{
    // lista para armazenar todas as Contas
    private List<Conta> contasCadastradas = new();

    // CADASTRAR NOVA CONTA
    public void cadastrar (Conta _conta)
    {
        contasCadastradas.Add(_conta);
        
        Cores.Sucesso($"Conta de número {_conta.getNumero()} cadastrada com sucesso!\n");
    }

    // GERA UM NÚMERO AUTOMÁTICO PARA A CONTA
    public int gerarNumero()
    {
        return contasCadastradas.Count + 1;
    }

    // ATUALIZAR UMA CONTA
    public void atualizar (Conta _conta)
    {
        // verifica se a conta está cadastrada
        Conta? contaExistente = buscarNaCollection(_conta.getNumero());

        // caso exista, atualiza seus valores
        if (contaExistente != null)
        {
            // atualiza o índice da conta na coleção
            contasCadastradas[contasCadastradas.IndexOf(contaExistente)] = _conta;

            // notifica sucesso
            Cores.Sucesso($"Conta de número {_conta.getNumero()} atualizada com sucesso!\n");
        }
        else
        {
            // notifica o erro
            Cores.Erro($"Erro! Conta de número {_conta.getNumero()} não está cadastrada.\n");
        }
    }

    // PROCURAR POR NÚMERO
    public void procurarPorNumero(int _numero)
    {
        // verifica se a conta está cadastrada
        Conta? contaExistente = buscarNaCollection(_numero);

        // caso exista, verifica o tipo de conta e visualiza
        if (contaExistente != null)
        {
            contaExistente.visualizar();
        }
        else
        {
            // notifica o erro
            Cores.Erro($"Conta de número {_numero} não encontrada!\n");
        }
    }

    // LISTAR TODAS AS CONTAS CADASTRADAS
    public void listarTodas()
    {
        // verifica se não existem contas cadastradas
        if (contasCadastradas.Count == 0)
        {
            Cores.Info("Não existem contas cadastradas até o momento!\n");
            
            return;
        }

        // caso existam, percorre a coleção imprimindo as contas
        foreach (Conta conta in contasCadastradas)
        {
            conta.visualizar();
        }
    }

    // DELETAR UMA CONTA
    public void deletar (int _numero)
    {
        // verifica se a conta está cadastrada
        Conta? contaExistente = buscarNaCollection(_numero);

        // caso não exista, notifica
        if (contaExistente == null)
        {
            Cores.Erro($"Conta de número {_numero} não está cadastrada!\n");

            return;
        }

        // caso exista, pede confirmação
        contaExistente.visualizar();
        Cores.Aviso("Tem certeza que deseja deletar essa conta? (S/N): ");
        string? confirmacao = Console.ReadLine();

        // se confirmar, exclui
        if (confirmacao == "S")
        {
            contasCadastradas.RemoveAt(contasCadastradas.IndexOf(contaExistente));
            Cores.Sucesso($"Conta de número {_numero} deletada com sucesso!\n");
        }
        // caso não confirmar, ou digitar chave inválida, cancela
        else if (confirmacao == "N")
        {
            Cores.Erro("Operação Cancelada!");
        }
        else
        {
            Cores.Erro("Chave inválida! Operação Cancelada!");
        }

    }

    // CHAMA O MÉTODO SACAR
    public void sacar(int _numero, float _valor)
    {
        // verifica se a conta está cadastrada
        Conta? contaExistente = buscarNaCollection(_numero);

        // caso não exista
        if (contaExistente == null)
        {
            Cores.Erro($"Conta de número {_numero} não está cadastrada!\n");

            return;
        }

        // caso exista, chama o método sacar
        contaExistente.sacar(_valor);
    }

    // CHAMA O MÉTODO DEPOSITAR
    public void depositar(int _numero, float _valor)
    {
        // verifica se a conta está cadastrada
        Conta? contaExistente = buscarNaCollection(_numero);

        // caso não exista
        if (contaExistente == null)
        {
            Cores.Erro($"Conta de número {_numero} não está cadastrada!\n");

            return;
        }

        // caso exista, chama o método depositar
        contaExistente.depositar(_valor);
    }

    // CHAMA O MÉTODO DE TRANSFERÊNCIA
    public void transferir(int _numeroOrigem, int _numeroDestino, float _valor)
    {
        // verifica se as duas contas estão cadastradas
        Conta? contaOrigem = buscarNaCollection(_numeroOrigem);
        if (contaOrigem == null)
        {
            Cores.Erro($"Conta de número {_numeroOrigem} não está cadastrada!\n");

            return;
        }

        Conta? contaDestino = buscarNaCollection(_numeroDestino);
        if (contaDestino == null)
        {
            Cores.Erro($"Conta de número {_numeroDestino} não está cadastrada!\n");

            return;
        }

        // verifica se a Conta de Origem possui o valor desejado
        bool saqueRealizado = contaOrigem.sacar(_valor);
        if (!saqueRealizado) return;

        // caso o saque seja feito, credita na conta de destino
        contaDestino.depositar(_valor);
        Cores.Sucesso($"Transferência de R${_valor} concluída com sucesso!");
    }

    // BUSCA UMA CONTA NA BASE DE DADOS
    public Conta? buscarNaCollection(int _numero)
    {
        foreach (var conta in contasCadastradas)
        {
            if (conta.getNumero() == _numero)
            {
                return conta;
            }
        }

        return null;
    }

}