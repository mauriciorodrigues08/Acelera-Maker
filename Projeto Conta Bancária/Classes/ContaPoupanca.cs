namespace Projeto_Conta_Bancaria.Classes;
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
    public void visualizar()
    {
        Console.WriteLine("# EXIBINDO DADOS DO CLIENTE #");
        Console.WriteLine($"Titular: {this.getTitular()}");
        Console.WriteLine($"Tipo: {this.getTipo()}");
        Console.WriteLine($"Número: {this.getNumero()}");
        Console.WriteLine($"Agência: {this.getAgencia()}");
        Console.WriteLine($"Saldo: R${this.getSaldo()}");
        Console.WriteLine($"Aniversário: {this.getAniversario()}");
        Console.WriteLine();
    }

    // sacar
    public override bool sacar(float _valor)
    {
        if (_valor > 0 && _valor <= getSaldo())
        {
            setSaldo(getSaldo() - _valor);
            Console.WriteLine($"Saque de R${_valor} realizado com sucesso!\n");
            return true;
        }
        Console.WriteLine($"Não foi possível realizar o saque de R${_valor}. Saldo insuficiente!\n");
        return false;
    }
}
