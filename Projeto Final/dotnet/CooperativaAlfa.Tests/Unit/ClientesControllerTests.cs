using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using CooperativaAlfa.Controllers;
using CooperativaAlfa.Models;
using CooperativaAlfa.Services;
using CooperativaAlfa.Tests.Helpers;

namespace CooperativaAlfa.Tests.Unit;

/// <summary>
/// Testes unitários do ClientesController.
/// O CobolBridge é mockado — nenhum processo COBOL é iniciado.
/// Cada teste verifica um cenário de mapeamento HTTP isoladamente.
/// </summary>
public class ClientesControllerTests
{
    private readonly Mock<CobolBridge> _cobolMock;
    private readonly ClientesController _controller;

    public ClientesControllerTests()
    {
        // Configura o mock com valores mínimos para o construtor do CobolBridge
        var configMock = new Microsoft.Extensions.Configuration
            .ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Cobol:ExecutablePath"] = "/fake/clientes",
                ["Cobol:OdbcIniPath"]    = "/fake/odbc.ini"
            })
            .Build();

        _cobolMock = new Mock<CobolBridge>(
            configMock,
            NullLogger<CobolBridge>.Instance);

        _controller = new ClientesController(
            _cobolMock.Object,
            NullLogger<ClientesController>.Instance);
    }

    // ── GET /clientes/{codigo} ───────────────────────────────────

    [Fact]
    [Trait("Categoria", "Unitario")]
    public async Task Consultar_ClienteExistente_RetornaHttp200ComDados()
    {
        // Arrange
        var respostaCobol = CobolResponseFactory.ClienteEncontrado();
        _cobolMock
            .Setup(c => c.ConsultarClienteAsync(1))
            .ReturnsAsync(respostaCobol);

        // Act
        var resultado = await _controller.Consultar(1);

        // Assert
        var ok = Assert.IsType<OkObjectResult>(resultado);
        var cliente = Assert.IsType<ClienteDto>(ok.Value);
        Assert.Equal(1, cliente.Codigo);
        Assert.Equal("Joao Silva", cliente.Nome);
        Assert.Equal("11999999999", cliente.Telefone);
        Assert.Equal("joao@teste.com", cliente.Email);
    }

    [Fact]
    [Trait("Categoria", "Unitario")]
    public async Task Consultar_ClienteNaoEncontrado_RetornaHttp404()
    {
        // Arrange
        _cobolMock
            .Setup(c => c.ConsultarClienteAsync(99))
            .ReturnsAsync(CobolResponseFactory.ClienteNaoEncontrado());

        // Act
        var resultado = await _controller.Consultar(99);

        // Assert
        var notFound = Assert.IsType<NotFoundObjectResult>(resultado);
        Assert.NotNull(notFound.Value);
    }

    [Fact]
    [Trait("Categoria", "Unitario")]
    public async Task Consultar_ErroInterno_RetornaHttp500()
    {
        // Arrange
        _cobolMock
            .Setup(c => c.ConsultarClienteAsync(1))
            .ReturnsAsync(CobolResponseFactory.ErroInterno());

        // Act
        var resultado = await _controller.Consultar(1);

        // Assert
        var erro = Assert.IsType<ObjectResult>(resultado);
        Assert.Equal(500, erro.StatusCode);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-99)]
    [Trait("Categoria", "Unitario")]
    public async Task Consultar_CodigoInvalido_RetornaHttp400(int codigoInvalido)
    {
        // Act
        var resultado = await _controller.Consultar(codigoInvalido);

        // Assert
        Assert.IsType<BadRequestObjectResult>(resultado);
        // CobolBridge não deve ser chamado para código inválido
        _cobolMock.Verify(
            c => c.ConsultarClienteAsync(It.IsAny<int>()), Times.Never);
    }

    // ── PUT /clientes/{codigo} ───────────────────────────────────

    [Fact]
    [Trait("Categoria", "Unitario")]
    public async Task Atualizar_DadosValidos_RetornaHttp200()
    {
        // Arrange
        _cobolMock
            .Setup(c => c.AtualizarClienteAsync(1, "11988887777", "novo@email.com"))
            .ReturnsAsync(CobolResponseFactory.AtualizacaoSucesso());

        var request = new AtualizaClienteRequest
        {
            Telefone = "11988887777",
            Email    = "novo@email.com"
        };

        // Act
        var resultado = await _controller.Atualizar(1, request);

        // Assert
        var ok = Assert.IsType<OkObjectResult>(resultado);
        Assert.NotNull(ok.Value);
    }

    [Fact]
    [Trait("Categoria", "Unitario")]
    public async Task Atualizar_ClienteNaoEncontrado_RetornaHttp404()
    {
        // Arrange
        _cobolMock
            .Setup(c => c.AtualizarClienteAsync(99, It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(CobolResponseFactory.ClienteNaoEncontrado());

        var request = new AtualizaClienteRequest
        {
            Telefone = "11988887777",
            Email    = "novo@email.com"
        };

        // Act
        var resultado = await _controller.Atualizar(99, request);

        // Assert
        Assert.IsType<NotFoundObjectResult>(resultado);
    }

    [Fact]
    [Trait("Categoria", "Unitario")]
    public async Task Atualizar_ErroInterno_RetornaHttp500()
    {
        // Arrange
        _cobolMock
            .Setup(c => c.AtualizarClienteAsync(1, It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(CobolResponseFactory.ErroInterno());

        var request = new AtualizaClienteRequest
        {
            Telefone = "11988887777",
            Email    = "novo@email.com"
        };

        // Act
        var resultado = await _controller.Atualizar(1, request);

        // Assert
        var erro = Assert.IsType<ObjectResult>(resultado);
        Assert.Equal(500, erro.StatusCode);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [Trait("Categoria", "Unitario")]
    public async Task Atualizar_CodigoInvalido_RetornaHttp400(int codigoInvalido)
    {
        // Act
        var resultado = await _controller.Atualizar(
            codigoInvalido,
            new AtualizaClienteRequest
            {
                Telefone = "11988887777",
                Email    = "novo@email.com"
            });

        // Assert
        Assert.IsType<BadRequestObjectResult>(resultado);
        _cobolMock.Verify(
            c => c.AtualizarClienteAsync(
                It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()),
            Times.Never);
    }
}