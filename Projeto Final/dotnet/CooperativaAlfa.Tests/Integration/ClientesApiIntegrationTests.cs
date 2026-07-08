using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using CooperativaAlfa.Models;
using CooperativaAlfa.Services;
using CooperativaAlfa.Tests.Helpers;
using Moq;

namespace CooperativaAlfa.Tests.Integration;

/// <summary>
/// Testes de integração da API.
/// A WebApplicationFactory sobe a API em memória — sem dotnet run.
/// O CobolBridge é substituído por um mock, sem depender do COBOL real.
/// Testa o pipeline HTTP completo: rota → validação → controller → resposta.
/// </summary>
public class ClientesApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly Mock<ICobolBridge> _cobolMock;

    public ClientesApiIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _cobolMock = new Mock<ICobolBridge>();
    
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(ICobolBridge));
                if (descriptor != null)
                    services.Remove(descriptor);
    
                services.AddTransient<ICobolBridge>(_ => _cobolMock.Object);
            });
        });
    }

    private HttpClient CriarCliente() => _factory.CreateClient();

    // ── GET /clientes/{codigo} ───────────────────────────────────

    [Fact]
    [Trait("Categoria", "Integracao")]
    public async Task GET_ClienteExistente_RetornaHttp200ComJSON()
    {
        // Arrange
        _cobolMock
            .Setup(c => c.ConsultarClienteAsync(1))
            .ReturnsAsync(CobolResponseFactory.ClienteEncontrado());

        // Act
        var response = await CriarCliente().GetAsync("/clientes/1");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("application/json", response.Content.Headers
            .ContentType?.MediaType);

        var cliente = await response.Content.ReadFromJsonAsync<ClienteDto>();
        Assert.NotNull(cliente);
        Assert.Equal(1, cliente.Codigo);
        Assert.Equal("Joao Silva", cliente.Nome);
        Assert.Equal("11999999999", cliente.Telefone);
        Assert.Equal("joao@teste.com", cliente.Email);
    }

    [Fact]
    [Trait("Categoria", "Integracao")]
    public async Task GET_ClienteNaoEncontrado_RetornaHttp404()
    {
        // Arrange
        _cobolMock
            .Setup(c => c.ConsultarClienteAsync(99))
            .ReturnsAsync(CobolResponseFactory.ClienteNaoEncontrado());

        // Act
        var response = await CriarCliente().GetAsync("/clientes/99");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        var body = await response.Content.ReadAsStringAsync();
        Assert.Contains("nao encontrado", body,
            StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    [Trait("Categoria", "Integracao")]
    public async Task GET_ErroInterno_RetornaHttp500()
    {
        // Arrange
        _cobolMock
            .Setup(c => c.ConsultarClienteAsync(1))
            .ReturnsAsync(CobolResponseFactory.ErroInterno());

        // Act
        var response = await CriarCliente().GetAsync("/clientes/1");

        // Assert
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
    }

    [Theory]
    [InlineData("/clientes/0")]
    [InlineData("/clientes/-1")]
    [Trait("Categoria", "Integracao")]
    public async Task GET_CodigoInvalido_RetornaHttp400(string url)
    {
        // Act
        var response = await CriarCliente().GetAsync(url);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    // ── PUT /clientes/{codigo} ───────────────────────────────────

    [Fact]
    [Trait("Categoria", "Integracao")]
    public async Task PUT_DadosValidos_RetornaHttp200()
    {
        // Arrange
        _cobolMock
            .Setup(c => c.AtualizarClienteAsync(
                1, "11988887777", "novo@email.com"))
            .ReturnsAsync(CobolResponseFactory.AtualizacaoSucesso());

        var payload = new { telefone = "11988887777", email = "novo@email.com" };

        // Act
        var response = await CriarCliente()
            .PutAsJsonAsync("/clientes/1", payload);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadAsStringAsync();
        Assert.Contains("sucesso", body, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    [Trait("Categoria", "Integracao")]
    public async Task PUT_ClienteNaoEncontrado_RetornaHttp404()
    {
        // Arrange
        _cobolMock
            .Setup(c => c.AtualizarClienteAsync(
                99, It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(CobolResponseFactory.ClienteNaoEncontrado());

        var payload = new { telefone = "11988887777", email = "novo@email.com" };

        // Act
        var response = await CriarCliente()
            .PutAsJsonAsync("/clientes/99", payload);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    [Trait("Categoria", "Integracao")]
    public async Task PUT_EmailInvalido_RetornaHttp400SemChamarCOBOL()
    {
        // Arrange
        var payload = new { telefone = "11988887777", email = "emailinvalido" };

        // Act
        var response = await CriarCliente()
            .PutAsJsonAsync("/clientes/1", payload);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        // Valida que o COBOL não foi chamado — a validação barrou antes
        _cobolMock.Verify(
            c => c.AtualizarClienteAsync(
                It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()),
            Times.Never);
    }

    [Fact]
    [Trait("Categoria", "Integracao")]
    public async Task PUT_TelefoneComLetras_RetornaHttp400SemChamarCOBOL()
    {
        // Arrange
        var payload = new { telefone = "1198888ABCD", email = "valido@email.com" };

        // Act
        var response = await CriarCliente()
            .PutAsJsonAsync("/clientes/1", payload);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        _cobolMock.Verify(
            c => c.AtualizarClienteAsync(
                It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()),
            Times.Never);
    }
}