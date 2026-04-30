using System;
using Projeto_Conta_Bancaria.Classes; // Importa as classes que estão na pasta 'Classes'

namespace Projeto_Conta_Bancaria
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== TESTE DO SISTEMA BANCÁRIO ===\n");

            // 1. Instanciando uma Conta Corrente
            // Parâmetros: numero, agencia, tipo, titular, saldo, limite
            ContaCorrente cc = new ContaCorrente(1001, 123, 1, "Mauricio", 500.0f, 200.0f);
            cc.visualizar();

            // Testando Saque na Corrente (usando parte do limite)
            Console.WriteLine("Ação: Sacando R$ 600,00...");
            cc.sacar(600.0f);
            cc.visualizar();

            Console.WriteLine("---------------------------------\n");

            // 2. Instanciando uma Conta Poupança
            // Parâmetros: numero, agencia, tipo, titular, saldo, aniversario
            ContaPoupanca cp = new ContaPoupanca(2001, 456, 2, "Maria", 1000.0f, 10);
            cp.visualizar();

            // Testando Saque na Poupança (sem limite disponível)
            Console.WriteLine("Ação: Tentando sacar R$ 1200,00...");
            cp.sacar(1200.0f);
            Console.WriteLine("---------------------------------\n");

            // Testando Depósito
            Console.WriteLine("Ação: Depositando R$ 300,00...");
            cp.depositar(300.0f);
            cp.visualizar();

            Console.WriteLine("=== FIM DOS TESTES ===");
        }
    }
}
