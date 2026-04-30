namespace Projeto_Conta_Bancaria.Classes;

public abstract class Conta
{
    //atributos
    private int numero;
    private int agencia;
    private int tipo;
    private string titular;
    private float saldo;

    // construtor
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
    public abstract bool sacar(float _valor);

    //depositar
    public void depositar(float _valor)
    {
        if (_valor > 0)
        {
            setSaldo(getSaldo() + _valor);
        }
    }
}
