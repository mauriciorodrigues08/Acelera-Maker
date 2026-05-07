namespace Projeto_Conta_Bancaria.Classes;

// imports
using System;
using System.Text.Json.Serialization;

public class ContaCorrente : Conta
{
    // atributos
    [JsonInclude] internal float limite;
    
    // construtores
    [JsonConstructor]
    protected ContaCorrente() : base() { }
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
    public override bool sacar(float _valor, bool transf = false)
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
            
            // verifica se o saque é parte de uma transação
            if (!transf)
            {
                // se não for, retorna mensagem de confirmação
                Cores.Sucesso($"Saque de R${_valor} realizado com sucesso!");
            }

            return true;
        }

        // mensagem de erro
        Cores.Erro($"Não foi possível realizar o saque de R${_valor}.\n Saldo/Limite insuficiente!");

        return false;
    }

    // visualizar
    public override void visualizar(){
        Cores.Titulo("- EXIBINDO DADOS DO CLIENTE -");
        Cores.Info($"Titular: {this.getTitular()}");
        Cores.Info($"Tipo: Corrente");
        Cores.Info($"Número: {this.getNumero()}");
        Cores.Info($"Agência: {this.getAgencia()}");
        Cores.Info($"Saldo: R${this.getSaldo()}");
        Cores.Info($"Limite: R${this.getLimite()}\n");
    }
}
