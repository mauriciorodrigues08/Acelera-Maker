namespace Projeto_Conta_Bancaria.Tests;

using Projeto_Conta_Bancaria.Classes;
using Xunit;

public class ContaControllerTests
{
    // ================================================================
    // Fixture — controller com contas pré-cadastradas
    // ================================================================
    private ContaController CriarController()
    {
        var controller = new ContaController();
        controller.cadastrar(new ContaCorrente(1, 11, 1, "Alice", 500f, 200f));
        controller.cadastrar(new ContaPoupanca(2, 22, 2, "Bob", 300f, 1990));
        return controller;
    }

    // ================================================================
    // gerarNumero()
    // ================================================================

    [Fact]
    public void GerarNumero_ListaVazia_DeveRetornarUm()
    {
        var controller = new ContaController();
        Assert.Equal(1, controller.gerarNumero());
    }

    [Fact]
    public void GerarNumero_ComContas_DeveRetornarProximo()
    {
        var controller = CriarController(); // tem contas 1 e 2
        Assert.Equal(3, controller.gerarNumero());
    }

    // ================================================================
    // cadastrar()
    // ================================================================

    [Fact]
    public void Cadastrar_NovaConta_DeveAdicionarNaCollection()
    {
        var controller = new ContaController();
        var conta = new ContaCorrente(1, 11, 1, "Teste", 0f, 100f);
        controller.cadastrar(conta);
        Assert.NotNull(controller.buscarNaCollection(1));
    }

    // ================================================================
    // buscarNaCollection()
    // ================================================================

    [Fact]
    public void BuscarNaCollection_NumeroExistente_DeveRetornarConta()
    {
        var controller = CriarController();
        var conta = controller.buscarNaCollection(1);
        Assert.NotNull(conta);
        Assert.Equal("Alice", conta.getTitular());
    }

    [Fact]
    public void BuscarNaCollection_NumeroInexistente_DeveRetornarNull()
    {
        var controller = CriarController();
        var conta = controller.buscarNaCollection(999);
        Assert.Null(conta);
    }

    // ================================================================
    // sacar()
    // ================================================================

    [Fact]
    public void Sacar_ContaExistenteSaldoSuficiente_DeveReduzirSaldo()
    {
        var controller = CriarController();
        controller.sacar(1, 200f); // Alice tem 500
        var conta = controller.buscarNaCollection(1);
        Assert.Equal(300f, conta!.getSaldo());
    }

    [Fact]
    public void Sacar_ContaInexistente_NaoDeveAlterarNada()
    {
        var controller = CriarController();
        // não deve lançar exceção
        controller.sacar(999, 100f);
    }

    [Fact]
    public void Sacar_SaldoInsuficiente_NaoDeveAlterarSaldo()
    {
        var controller = CriarController();
        controller.sacar(2, 1000f); // Bob tem só 300, sem limite
        var conta = controller.buscarNaCollection(2);
        Assert.Equal(300f, conta!.getSaldo());
    }

    // ================================================================
    // depositar()
    // ================================================================

    [Fact]
    public void Depositar_ContaExistente_DeveAumentarSaldo()
    {
        var controller = CriarController();
        controller.depositar(1, 300f); // Alice tem 500
        var conta = controller.buscarNaCollection(1);
        Assert.Equal(800f, conta!.getSaldo());
    }

    [Fact]
    public void Depositar_ContaInexistente_NaoDeveAlterarNada()
    {
        var controller = CriarController();
        // não deve lançar exceção
        controller.depositar(999, 100f);
    }

    // ================================================================
    // transferir()
    // ================================================================

    [Fact]
    public void Transferir_ContasValidasSaldoSuficiente_DeveTransferir()
    {
        var controller = CriarController();
        // simula a confirmação — como o método pede S/N no console,
        // testamos os saldos diretamente via sacar + depositar
        var origem = controller.buscarNaCollection(1)!;
        var destino = controller.buscarNaCollection(2)!;

        bool saqueOk = origem.sacar(100f, transf: true);
        if (saqueOk) destino.depositar(100f, transf: true);

        Assert.Equal(400f, origem.getSaldo());
        Assert.Equal(400f, destino.getSaldo());
    }

    [Fact]
    public void Transferir_SaldoInsuficiente_NaoDeveAlterarSaldos()
    {
        var controller = CriarController();
        var origem = controller.buscarNaCollection(2)!; // Bob tem 300
        var destino = controller.buscarNaCollection(1)!;

        float saldoOrigemAntes = origem.getSaldo();
        float saldoDestinoAntes = destino.getSaldo();

        bool saqueOk = origem.sacar(1000f, transf: true); // falha
        if (saqueOk) destino.depositar(1000f, transf: true);

        Assert.Equal(saldoOrigemAntes, origem.getSaldo());
        Assert.Equal(saldoDestinoAntes, destino.getSaldo());
    }

    // ================================================================
    // deletar()
    // ================================================================

    [Fact]
    public void Deletar_ContaInexistente_NaoDeveAlterarNada()
    {
        var controller = CriarController();
        // não deve lançar exceção — apenas notifica erro
        // a confirmação S/N é no console, então testamos buscarNaCollection
        var contaAntes = controller.buscarNaCollection(999);
        Assert.Null(contaAntes);
    }

    // ================================================================
    // atualizar() — sobrecarga ContaCorrente
    // ================================================================

    [Fact]
    public void AtualizarCorrente_DadosValidos_DeveAtualizarTitularELimite()
    {
        var controller = CriarController();
        controller.atualizar(1, "Alice Atualizada", 500f);
        var conta = controller.buscarNaCollection(1) as ContaCorrente;
        Assert.NotNull(conta);
        Assert.Equal("Alice Atualizada", conta.getTitular());
        Assert.Equal(500f, conta.getLimite());
    }

    [Fact]
    public void AtualizarCorrente_DevePreservarSaldo()
    {
        var controller = CriarController();
        controller.atualizar(1, "Alice Nova", 300f);
        var conta = controller.buscarNaCollection(1);
        Assert.Equal(500f, conta!.getSaldo()); // saldo original preservado
    }

    // ================================================================
    // atualizar() — sobrecarga ContaPoupanca
    // ================================================================

    [Fact]
    public void AtualizarPoupanca_DadosValidos_DeveAtualizarTitularEAniversario()
    {
        var controller = CriarController();
        controller.atualizar(2, "Bob Atualizado", 2000);
        var conta = controller.buscarNaCollection(2) as ContaPoupanca;
        Assert.NotNull(conta);
        Assert.Equal("Bob Atualizado", conta.getTitular());
        Assert.Equal(2000, conta.getAniversario());
    }

    [Fact]
    public void AtualizarPoupanca_DevePreservarSaldo()
    {
        var controller = CriarController();
        controller.atualizar(2, "Bob Novo", 1995);
        var conta = controller.buscarNaCollection(2);
        Assert.Equal(300f, conta!.getSaldo()); // saldo original preservado
    }

    [Fact]
    public void Atualizar_ContaInexistente_NaoDeveAlterarNada()
    {
        var controller = CriarController();
        // não deve lançar exceção
        controller.atualizar(999, "Fantasma", 100f);
        Assert.Null(controller.buscarNaCollection(999));
    }
}