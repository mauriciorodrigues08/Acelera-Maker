namespace Projeto_Conta_Bancaria.Classes;

// import
using System.Text.Json.Serialization;

// Atributos json para a classe Conta
[JsonPolymorphic(TypeDiscriminatorPropertyName = "tipoConta")]
[JsonDerivedType(typeof(ContaCorrente), "corrente")]
[JsonDerivedType(typeof(ContaPoupanca), "poupanca")]
public abstract class Conta
{
    //atributos (liberados para acesso do json)
    [JsonInclude] internal string titular;
    [JsonInclude] internal int tipo;
    [JsonInclude] internal int numero;
    [JsonInclude] internal int agencia;
    [JsonInclude] internal float saldo;

    // construtores

    public Conta()
    {
        this.titular = "";
    }
    public Conta(int _numero = 0, int _agencia = 0, int _tipo = 0, string _titular = "", float _saldo = 0)
    {
        numero = _numero;
        agencia = _agencia;
        tipo = _tipo;
        titular = _titular;
        setSaldo(_saldo);
    }

    // getters
    public int getNumero()
    {
        return this.numero;
    }
    
    public int getAgencia()
    {
        return this.agencia;
    }
    
    public int getTipo()
    {
        return this.tipo;
    }
    
    public string getTitular()
    {
        return this.titular;
    }

    public float getSaldo()
    {
        return this.saldo;
    }

    // setters
    public void setNumero(int _numero)
    {
        this.numero = _numero;
    }
    
    public void setAgencia(int _agencia)
    {
        this.agencia = _agencia;
    }
    
    public void setTipo(int _tipo)
    {
        this.tipo = _tipo;
    }
    
    public void setTitular(string _titular)
    {
        this.titular = _titular;
    }

    public void setSaldo(float _saldo)
    {
        if (_saldo > 0)
        {   
            this.saldo = _saldo;
        }
        else 
        {
            this.saldo = 0;
        }
    }

    // sacar
    public abstract bool sacar(float _valor, bool transf = false);

    //depositar
    public void depositar(float _valor, bool transf = false)
    {
        if (_valor > 0)
        {
            setSaldo(getSaldo() + _valor);
            
            // verifica se o depósit é parte de uma transferência
            if (!transf)
            {
                // caso não for, notifica o sucesso
                Cores.Sucesso($"Depósito de R${_valor} realizado com sucesso!");
            }
        }
        else
        {
            Cores.Erro("Erro! Valor inválido!");
        }
    }

    // visualizar informações
    public abstract void visualizar();
}