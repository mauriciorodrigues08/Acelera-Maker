namespace Projeto_Conta_Bancaria.Classes;

// imports
using System.Collections.Generic;
using System.Text.Json;

public class ContaController : IContaRepository
{
    // caminho do arquivo json para Persistência de Dados
    internal static readonly string CAMINHO_ARQUIVO = Path.Combine(
        AppContext.BaseDirectory, // caminho atual (Projeto\bin\Debug\net10.0)
        "../../../contas.json" // volta 3 diretórios e acessa o arquivo na pasta base (Projeto)
    );

    // configurações para o json
    internal static readonly JsonSerializerOptions JSON_OPTIONS = new()
    {
        WriteIndented = true,               // adiciona quebras de linha ao arquivo json
        PropertyNameCaseInsensitive = true, // ignora maiúsculas e minúsculas (case insensitive)
        IncludeFields = true                // necessário para serializar campos internal com [JsonInclude]
    };

    // lista para armazenar todas as Contas
    internal List<Conta> contasCadastradas = new();

    // CONSTRUTOR
    public ContaController() { }

    // CADASTRAR NOVA CONTA
    public void cadastrar (Conta _conta)
    {
        // adiciona a nova conta à coleção
        contasCadastradas.Add(_conta);

        //notifica sucesso
        Cores.Sucesso($"\nConta de número {_conta.getNumero()} cadastrada com sucesso!\n");
        
        // salva no json
        salvar();
    }

    // GERA UM NÚMERO AUTOMÁTICO PARA A CONTA
    public int gerarNumero()
    {
        // retorna 1 se a coleção estiver vazia
        if (contasCadastradas.Count == 0) return 1;
        
        // retorna o número da última conta cadastrada somado de 1 
        return contasCadastradas[(contasCadastradas.Count - 1)].getNumero() + 1;
    }

    // ATUALIZAR UMA CONTA
    // sobrecarga para Conta Corrente
    public void atualizar(int _numero, string _titular, float _limite)
    {
        Conta? contaExistente = buscarNaCollection(_numero);
        if (contaExistente == null) {
            Cores.Erro("\nConta não encontrada!");
            return;
        }

        var contaAtt = new ContaCorrente(
            contaExistente.getNumero(),
            contaExistente.getAgencia(),
            1, _titular,
            contaExistente.getSaldo(),
            _limite
        );

        // atualiza a conta na coleção
        contasCadastradas[contasCadastradas.IndexOf(contaExistente)] = contaAtt;
        Cores.Sucesso($"\nConta de número {_numero} atualizada com sucesso!");

        // mostra a conta com os novos dados e salva
        contaAtt.visualizar();
        salvar();    
    }

    // sobrecarga para Conta Poupança
    public void atualizar(int _numero, string _titular, int _aniversario)
    {
        Conta? contaExistente = buscarNaCollection(_numero);
        if (contaExistente == null) {
            Cores.Erro("\nConta não encontrada!");
            return;
        }

        var contaAtt = new ContaPoupanca(
            contaExistente.getNumero(),
            contaExistente.getAgencia(),
            2, _titular,
            contaExistente.getSaldo(),
            _aniversario
        );

        // atualiza a conta na coleção
        contasCadastradas[contasCadastradas.IndexOf(contaExistente)] = contaAtt;
        Cores.Sucesso($"\nConta de número {_numero} atualizada com sucesso!");
        
        // mostra a conta com os novos dados e salva
        contaAtt.visualizar();
        salvar();
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
            Cores.Erro($"\nConta de número {_numero} não encontrada!\n");
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
            Cores.Erro($"\nConta de número {_numero} não está cadastrada!\n");

            return;
        }

        // caso exista, pede confirmação
        contaExistente.visualizar();
        Cores.Aviso("Tem certeza que deseja deletar essa conta? (S/N): ");
        string? confirmacao = Console.ReadLine();

        // se confirmar, exclui
        if (confirmacao?.ToUpper() == "S")
        {
            // deleta e notifica sucesso
            contasCadastradas.RemoveAt(contasCadastradas.IndexOf(contaExistente));
            Cores.Sucesso($"\nConta de número {_numero} deletada com sucesso!\n");
            
            // salva no json
            salvar();
        }
        // caso não confirmar, ou digitar chave inválida, cancela
        else if (confirmacao?.ToUpper() == "N")
        {
            Cores.Erro("\nOperação Cancelada!");
        }
        else
        {
            Cores.Erro("\nChave inválida! Operação Cancelada!");
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
            Cores.Erro($"\nConta de número {_numero} não está cadastrada!\n");

            return;
        }

        // caso exista, chama o método sacar
        bool saqueRealizado = contaExistente.sacar(_valor);

        // visualiza o novo saldo
        contaExistente.visualizar();

        // salva no json caso o saque tenha sucesso
        if (saqueRealizado) salvar();
    }

    // CHAMA O MÉTODO DEPOSITAR
    public void depositar(int _numero, float _valor)
    {
        // verifica se a conta está cadastrada
        Conta? contaExistente = buscarNaCollection(_numero);

        // caso não exista
        if (contaExistente == null)
        {
            Cores.Erro($"\nConta de número {_numero} não está cadastrada!\n");

            return;
        }

        // caso exista, chama o método depositar
        contaExistente.depositar(_valor);
        
        // visualiza o novo saldo
        contaExistente.visualizar();

        // salva no json
        salvar();
    }

    // CHAMA O MÉTODO DE TRANSFERÊNCIA
    public void transferir(int _numeroOrigem, int _numeroDestino, float _valor)
    {
        // verifica se as duas contas estão cadastradas
        Conta? contaOrigem = buscarNaCollection(_numeroOrigem);
        if (contaOrigem == null)
        {
            Cores.Erro($"\nConta de número {_numeroOrigem} não está cadastrada!\n");
            return;
        }

        Conta? contaDestino = buscarNaCollection(_numeroDestino);
        if (contaDestino == null)
        {
            Cores.Erro($"\nConta de número {_numeroDestino} não está cadastrada!\n");
            return;
        }

        // verificação de transação
        Cores.Info($"\nTransferindo R${_valor} de {contaOrigem.getTitular()} para {contaDestino.getTitular()}...");
        Cores.Write("Continuar? (S/N): ");
        string? confirmacao = Console.ReadLine();

        if (confirmacao?.ToUpper() == "S")
        {
            // verifica se a Conta de Origem possui o valor desejado
            bool saqueRealizado = contaOrigem.sacar(_valor, true);
            if (!saqueRealizado) return;

            // caso o saque seja feito, credita na conta de destino
            contaDestino.depositar(_valor, true);
            Cores.Sucesso($"\nTransferência de R${_valor} concluída com sucesso!");

            // salva no json
            salvar();
        }
        else if (confirmacao?.ToUpper() == "N")
        {
            Cores.Erro("\nOperação Cancelada!");
        }
        else
        {
            Cores.Erro("\nChave inválida! Operação Cancelada!");
        }

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

    // PERSISTÊNCIA DE DADOS
    // Carregar
    public bool carregar()
    {
        // retorno:
        // true  -> arquivo existe
        // false -> arquivo não existe

        // verifica se o arquivo existe
        if (!File.Exists(CAMINHO_ARQUIVO)) return false;

        // tenta carregar os dados
        try
        {
            // lê o json em uma string
            string json = File.ReadAllText(CAMINHO_ARQUIVO);

            // desserializa a string dentro de uma coleção
            var colecao = JsonSerializer.Deserialize<List<Conta>>(json, JSON_OPTIONS);

            // verifica se a coleção gerada é válida
            if (colecao != null)
            {
                // carrega os dados importados na coleção contasCadastradas
                contasCadastradas = colecao;
                
                // caso não tenha exeções, notifica sucesso e retorna true
                Cores.Sucesso("Dados carregados com sucesso!");
            }
        }
        // trata exceção (falha na coleta dos dados)
        catch (Exception ex)
        {
            Cores.Erro($"Erro ao carregar os dados: {ex.Message}");
        }

        return true;
    }

    // Salvar
    public void salvar()
    {
        try
        {    
            // salva as informações no arquivo
            var json = JsonSerializer.Serialize(contasCadastradas, JSON_OPTIONS);
            File.WriteAllText(CAMINHO_ARQUIVO, json);
        }
        catch (Exception ex)
        {
            // notifica erro
            Cores.Erro($"Erro ao salvar dados: {ex.Message}");
        }
    }

    // método que viabiliza limpar uma conta para realização de testes xUnit
    public void LimparContasParaTeste()
    {
        contasCadastradas.Clear();
    }
}