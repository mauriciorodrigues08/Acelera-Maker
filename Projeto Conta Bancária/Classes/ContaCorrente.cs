namespace Projeto_Conta_Bancaria.Classes;
using System;

public class ContaCorrente : Conta
{
    // atributos
    private float limite;
    
    // construtor
    public ContaCorrente(int _numero, int _agencia, int _tipo, string _titular, float _saldo, float _limite)
        : base(_numero, _agencia, _tipo, _titular, _saldo)
    {
        this.setLimite(_limite);
    }

    // getters
    public float getLimite()
    {
        return this.limite;
    }

    // setters
    public void setLimite(float _limite)
    {
        if (_limite > 0)
        {
            this.limite = _limite;
        }
        else
        {
            this.limite = 0;
        }
    }

    // sacar
    public override bool sacar(float _valor)
    {
        // verifica se é possível realizar o saque
        if ( (_valor > 0) && (_valor <= this.getSaldo() + getLimite()) )
        {
            // verifica se valor a sacar é maior que saldo disponível
            if (_valor > this.getSaldo())
            {
                // saldo recebe 0 e retira o valor restante do limite
                float newValor = _valor - this.getSaldo();
                this.setSaldo(0);
                this.setLimite(this.getLimite() - newValor);
            }
            else
            {
                // apenas debita do saldo atual
                this.setSaldo(this.getSaldo() - _valor);
            }
            
            // mensagem de confirmação
            Console.WriteLine($"Saque de R${_valor} realizado com sucesso!\n");

            return true;
        }

        // mensagem de erro
        Console.WriteLine($"Não foi possível realizar o saque de R${_valor}. Saldo/Limite insuficiente!\n");

        return false;
    }

    // vizualizar
    public void visualizar(){
        Console.WriteLine("# EXIBINDO DADOS DO CLIENTE #");
        Console.WriteLine($"Titular: {this.getTitular()}");
        Console.WriteLine($"Tipo: {this.getTipo()}");
        Console.WriteLine($"Número: {this.getNumero()}");
        Console.WriteLine($"Agência: {this.getAgencia()}");
        Console.WriteLine($"Saldo: R${this.getSaldo()}");
        Console.WriteLine($"Limite: R${this.getLimite()}");
        Console.WriteLine();
    }
}
