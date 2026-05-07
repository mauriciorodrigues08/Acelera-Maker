namespace Projeto_Conta_Bancaria.Tests;

using Projeto_Conta_Bancaria.Classes;
using Xunit;

public class ContaCorrenteTests
{
    // ================================================================
    // Fixture — conta padrão para os testes
    // Numero: 1 | Agencia: 11 | Saldo: 500 | Limite: 200
    // ================================================================
    private ContaCorrente CriarConta(float saldo = 500f, float limite = 200f)
        => new ContaCorrente(1, 11, 1, "Titular Teste", saldo, limite);

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
    public void Sacar_UsandoLimite_DeveRetornarTrue()
    {
        var conta = CriarConta(saldo: 100f, limite: 200f);
        bool resultado = conta.sacar(250f); // usa 100 do saldo + 150 do limite
        Assert.True(resultado);
    }

    [Fact]
    public void Sacar_UsandoLimite_DeveReduzirSaldoELimite()
    {
        var conta = CriarConta(saldo: 100f, limite: 200f);
        conta.sacar(250f); // usa 100 do saldo + 150 do limite
        Assert.Equal(0f, conta.getSaldo());
        Assert.Equal(50f, conta.getLimite());
    }

    [Fact]
    public void Sacar_ValorAcimaDoSaldoMaisLimite_DeveRetornarFalse()
    {
        var conta = CriarConta(saldo: 500f, limite: 200f);
        bool resultado = conta.sacar(800f); // 500 + 200 = 700, mas pede 800
        Assert.False(resultado);
    }

    [Fact]
    public void Sacar_ValorAcimaDoSaldoMaisLimite_NaoDeveAlterarSaldo()
    {
        var conta = CriarConta(saldo: 500f, limite: 200f);
        conta.sacar(800f);
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
    public void Sacar_ComoTransferencia_NaoDeveAlterarMensagem()
    {
        // transf = true não deve afetar o resultado do saque
        var conta = CriarConta();
        bool resultado = conta.sacar(100f, transf: true);
        Assert.True(resultado);
        Assert.Equal(400f, conta.getSaldo());
    }

    // Teste com múltiplos valores usando [Theory]
    [Theory]
    [InlineData(100f,  true)]   // dentro do saldo
    [InlineData(500f,  true)]   // exatamente o saldo
    [InlineData(600f,  true)]   // usando limite parcialmente
    [InlineData(700f,  true)]   // usando saldo + limite inteiro
    [InlineData(701f,  false)]  // acima do saldo + limite
    [InlineData(-10f,  false)]  // negativo
    [InlineData(0f,    false)]  // zero
    public void Sacar_DiversosValores_DeveRetornarEsperado(float valor, bool esperado)
    {
        var conta = CriarConta(saldo: 500f, limite: 200f);
        Assert.Equal(esperado, conta.sacar(valor));
    }

    // ================================================================
    // depositar()
    // ================================================================

    [Fact]
    public void Depositar_ValorPositivo_DeveAumentarSaldo()
    {
        var conta = CriarConta(saldo: 500f);
        conta.depositar(200f);
        Assert.Equal(700f, conta.getSaldo());
    }

    [Fact]
    public void Depositar_ValorNegativo_NaoDeveAlterarSaldo()
    {
        var conta = CriarConta(saldo: 500f);
        conta.depositar(-100f);
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
    // getLimite() / setLimite()
    // ================================================================

    [Fact]
    public void SetLimite_ValorPositivo_DeveAtualizarLimite()
    {
        var conta = CriarConta(limite: 200f);
        conta.setLimite(500f);
        Assert.Equal(500f, conta.getLimite());
    }

    [Fact]
    public void SetLimite_ValorNegativo_DeveDefinirZero()
    {
        var conta = CriarConta(limite: 200f);
        conta.setLimite(-100f);
        Assert.Equal(0f, conta.getLimite());
    }
}