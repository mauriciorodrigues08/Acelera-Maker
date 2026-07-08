using CooperativaAlfa.Models;

namespace CooperativaAlfa.Tests.Unit;

/// <summary>
/// Testes unitários do modelo CobolResponse.
/// Valida os helpers de status (Sucesso, NaoEncontrado, Erro)
/// que são usados pelo controller para tomar decisões de roteamento.
/// </summary>
public class CobolResponseTests
{
    [Fact]
    [Trait("Categoria", "Unitario")]
    public void Sucesso_QuandoStatus00_RetornaTrue()
    {
        var response = new CobolResponse { Status = "00" };
        Assert.True(response.Sucesso);
        Assert.False(response.NaoEncontrado);
        Assert.False(response.Erro);
    }

    [Fact]
    [Trait("Categoria", "Unitario")]
    public void NaoEncontrado_QuandoStatus04_RetornaTrue()
    {
        var response = new CobolResponse { Status = "04" };
        Assert.False(response.Sucesso);
        Assert.True(response.NaoEncontrado);
        Assert.False(response.Erro);
    }

    [Fact]
    [Trait("Categoria", "Unitario")]
    public void Erro_QuandoStatus08_RetornaTrue()
    {
        var response = new CobolResponse { Status = "08" };
        Assert.False(response.Sucesso);
        Assert.False(response.NaoEncontrado);
        Assert.True(response.Erro);
    }
}