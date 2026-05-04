namespace Projeto_Conta_Bancaria.Classes;

//imports
using System;

public class ContaPoupanca : Conta
{
    // atributos
    private int aniversario;
    
    // construtor
    public ContaPoupanca(int _numero, int _agencia, int _tipo, string _titular, float _saldo, int _aniversario)
        : base(_numero, _agencia, _tipo, _titular, _saldo)
    {
        setAniversario(_aniversario);
    }
    
    // getters
    public int getAniversario()
    {
        return this.aniversario;
    }
    
    // setters
    public void setAniversario(int _aniversario)
    {
        this.aniversario = _aniversario;
    }
    
    // vizualizar
    public override void visualizar()
    {
        Cores.Titulo("- EXIBINDO DADOS DO CLIENTE -");
        Cores.Info($"Titular: {this.getTitular()}");
        Cores.Info($"Tipo: Poupança");
        Cores.Info($"Número: {this.getNumero()}");
        Cores.Info($"Agência: {this.getAgencia()}");
        Cores.Info($"Saldo: R${this.getSaldo()}");
        Cores.Info($"Aniversário: {this.getAniversario()}\n");
    }

    // sacar
    public override bool sacar(float _valor, bool transf = false)
    {
        // verifica se é possível realizar o saque
        if (_valor > 0 && _valor <= getSaldo())
        {
            // realiza o saque
            setSaldo(getSaldo() - _valor);

            // verifica se o saque é parte de uma transação
            if (!transf)
            {   
                // se não for, retorna mensagem de sucesso
                Cores.Sucesso($"Saque de R${_valor} realizado com sucesso!\n");
                return true;
            }
        }

        // retorna mensagem de erro
        Cores.Erro($"Não foi possível realizar o saque de R${_valor}. Saldo insuficiente!\n");
        return false;
    }
}
