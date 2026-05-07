namespace Projeto_Conta_Bancaria.Tests;

using Projeto_Conta_Bancaria.Classes;
using Xunit;

public class ContaPoupancaTests
{
    // ================================================================
    // Fixture — conta padrão para os testes
    // Numero: 1 | Agencia: 11 | Saldo: 500 | Aniversario: 1990
    // ================================================================
    private ContaPoupanca CriarConta(float saldo = 500f, int aniversario = 1990)
        => new ContaPoupanca(1, 11, 2, "Titular Teste", saldo, aniversario);

    // ================================================================
    // sacar()
    // ================================================================

    [Fact]
    public void Sacar_ValorDentroDoSaldo_DeveRetornarTrue()
    {
        var conta = CriarConta();
        bool resultado = conta.sacar(300f);
        Assert.True(resultado);
    }

    [Fact]
    public void Sacar_ValorDentroDoSaldo_DeveReduzirSaldo()
    {
        var conta = CriarConta();
        conta.sacar(300f);
        Assert.Equal(200f, conta.getSaldo());
    }

    [Fact]
    public void Sacar_ValorExatoDoSaldo_DeveRetornarTrue()
    {
        var conta = CriarConta(saldo: 500f);
        bool resultado = conta.sacar(500f);
        Assert.True(resultado);
        Assert.Equal(0f, conta.getSaldo());
    }

    [Fact]
    public void Sacar_ValorAcimaDoSaldo_DeveRetornarFalse()
    {
        var conta = CriarConta(saldo: 500f);
        bool resultado = conta.sacar(600f);
        Assert.False(resultado);
    }

    [Fact]
    public void Sacar_ValorAcimaDoSaldo_NaoDeveAlterarSaldo()
    {
        var conta = CriarConta(saldo: 500f);
        conta.sacar(600f);
        Assert.Equal(500f, conta.getSaldo());
    }

    [Fact]
    public void Sacar_ValorNegativo_DeveRetornarFalse()
    {
        var conta = CriarConta();
        bool resultado = conta.sacar(-100f);
        Assert.False(resultado);
    }

    [Fact]
    public void Sacar_ValorZero_DeveRetornarFalse()
    {
        var conta = CriarConta();
        bool resultado = conta.sacar(0f);
        Assert.False(resultado);
    }

    [Fact]
    public void Sacar_NaoTemLimite_NaoDevePermitirSaldoNegativo()
    {
        // diferente da ContaCorrente, poupança não tem limite
        var conta = CriarConta(saldo: 100f);
        bool resultado = conta.sacar(150f);
        Assert.False(resultado);
        Assert.Equal(100f, conta.getSaldo()); // saldo inalterado
    }

    [Fact]
    public void Sacar_ComoTransferencia_DeveRetornarTrue()
    {
        var conta = CriarConta();
        bool resultado = conta.sacar(100f, transf: true);
        Assert.True(resultado);
        Assert.Equal(400f, conta.getSaldo());
    }

    [Theory]
    [InlineData(100f,  true)]   // dentro do saldo
    [InlineData(500f,  true)]   // exatamente o saldo
    [InlineData(501f,  false)]  // acima do saldo — sem limite
    [InlineData(-10f,  false)]  // negativo
    [InlineData(0f,    false)]  // zero
    public void Sacar_DiversosValores_DeveRetornarEsperado(float valor, bool esperado)
    {
        var conta = CriarConta(saldo: 500f);
        Assert.Equal(esperado, conta.sacar(valor));
    }

    // ================================================================
    // depositar()
    // ================================================================

    [Fact]
    public void Depositar_ValorPositivo_DeveAumentarSaldo()
    {
        var conta = CriarConta(saldo: 500f);
        conta.depositar(300f);
        Assert.Equal(800f, conta.getSaldo());
    }

    [Fact]
    public void Depositar_ValorNegativo_NaoDeveAlterarSaldo()
    {
        var conta = CriarConta(saldo: 500f);
        conta.depositar(-200f);
        Assert.Equal(500f, conta.getSaldo());
    }

    [Fact]
    public void Depositar_ValorZero_NaoDeveAlterarSaldo()
    {
        var conta = CriarConta(saldo: 500f);
        conta.depositar(0f);
        Assert.Equal(500f, conta.getSaldo());
    }

    // ================================================================
    // getAniversario() / setAniversario()
    // ================================================================

    [Fact]
    public void GetAniversario_DeveRetornarValorCorreto()
    {
        var conta = CriarConta(aniversario: 1995);
        Assert.Equal(1995, conta.getAniversario());
    }

    [Fact]
    public void SetAniversario_DeveAtualizarValor()
    {
        var conta = CriarConta(aniversario: 1990);
        conta.setAniversario(2000);
        Assert.Equal(2000, conta.getAniversario());
    }
}