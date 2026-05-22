// Testes de integração para PostagemController.
// Usa WebApplicationFactory com banco InMemory e mocks de serviço
// para validar status codes, roteamento e regras de negócio dos endpoints.

using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using BlogPessoal.Models;
using BlogPessoal.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FluentAssertions;
using Moq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace BlogPessoal.Tests.Integration;

// ─────────────────────────────────────────────────────────────────
//  Factory customizada — substitui IPostagemService por um Mock
//  e configura JWT com chave fixa para os testes
// ─────────────────────────────────────────────────────────────────
public class PostagemWebFactory : WebApplicationFactory<Program>
{
    public Mock<IPostagemService> MockPostagemService { get; } = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Usa chave JWT conhecida para gerar tokens nos testes
        builder.UseSetting("Jwt:Key",    "chave-super-secreta-para-testes-123456");
        builder.UseSetting("Jwt:Issuer",   "BlogPessoal");
        builder.UseSetting("Jwt:Audience", "BlogPessoal");

        // Substitui o serviço real pelo mock
        builder.ConfigureTestServices(services =>
        {
            services.AddScoped<IPostagemService>(_ => MockPostagemService.Object);
        });
    }
}

// ─────────────────────────────────────────────────────────────────
//  Helper para gerar tokens JWT de teste
// ─────────────────────────────────────────────────────────────────
public static class JwtTestHelper
{
    private const string Key    = "chave-super-secreta-para-testes-123456";
    private const string Issuer   = "BlogPessoal";
    private const string Audience  = "BlogPessoal";

    public static string GerarToken(string email = "teste@email.com", string nome = "Usuário Teste")
    {
        var securityKey  = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
        var credentials  = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Name,  nome),
        };

        var token = new JwtSecurityToken(
            issuer:   Issuer,
            audience:  Audience,
            claims:   claims,
            expires:   DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

// ─────────────────────────────────────────────────────────────────
//  Testes de integração
// ─────────────────────────────────────────────────────────────────
public class PostagemControllerTests : IClassFixture<PostagemWebFactory>
{
    private readonly HttpClient _client;
    private readonly Mock<IPostagemService> _mockService;
    private readonly PostagemWebFactory _factory;

    // Dados reutilizáveis nos testes
    private static readonly Tema TemaExemplo = new() { Id = 1, Descricao = "Tecnologia" };
    private static readonly Usuario UsuarioExemplo = new() { Id = 1, Nome = "Ana", Email = "ana@email.com" };

    private static Postagem CriarPostagem(long id = 1) => new()
    {
        Id      = id,
        Titulo  = "Postagem Teste",
        Texto   = "Conteúdo da postagem de teste",
        Data    = DateTime.UtcNow,
        Tema    = TemaExemplo,
        Usuario = UsuarioExemplo
    };

    public PostagemControllerTests(PostagemWebFactory factory)
    {
        _factory = factory;
        _mockService = factory.MockPostagemService;
        _mockService.Reset(); // garante estado limpo entre testes

        _client = factory.CreateClient();

        // Adiciona token JWT válido a todas as requisições
        var token = JwtTestHelper.GerarToken();
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
    }

    // ── GET /api/postagens ──────────────────────────────────────

    [Fact]
    public async Task GetAll_Autenticado_DeveRetornar200ComListaDePostagens()
    {
        // Arrange
        var postagens = new List<Postagem> { CriarPostagem(1), CriarPostagem(2) };
        _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(postagens);

        // Act
        var response = await _client.GetAsync("/api/postagens");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await response.Content.ReadAsStringAsync();
        body.Should().Contain("Postagem Teste");
    }

    [Fact]
    public async Task GetAll_SemAutenticacao_DeveRetornar401()
    {
        // Arrange — cria cliente pela factory (sem adicionar token)
        var clienteSemAuth = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
        // Não adiciona Authorization header

        // Act
        var response = await clienteSemAuth.GetAsync("/api/postagens");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    // ── GET /api/postagens/{id} ─────────────────────────────────

    [Fact]
    public async Task GetById_PostagemExistente_DeveRetornar200()
    {
        // Arrange
        var postagem = CriarPostagem(1);
        _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(postagem);

        // Act
        var response = await _client.GetAsync("/api/postagens/1");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await response.Content.ReadAsStringAsync();
        body.Should().Contain("Postagem Teste");
    }

    [Fact]
    public async Task GetById_PostagemInexistente_DeveRetornar404()
    {
        // Arrange
        _mockService.Setup(s => s.GetByIdAsync(99)).ReturnsAsync((Postagem?)null);

        // Act
        var response = await _client.GetAsync("/api/postagens/99");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    // ── GET /api/postagens/filtro ───────────────────────────────

    [Fact]
    public async Task GetByFiltro_ComAutor_DeveRetornar200()
    {
        // Arrange
        var postagens = new List<Postagem> { CriarPostagem(1) };
        _mockService.Setup(s => s.GetByAutorAsync(1)).ReturnsAsync(postagens);

        // Act
        var response = await _client.GetAsync("/api/postagens/filtro?autor=1");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetByFiltro_ComTema_DeveRetornar200()
    {
        // Arrange
        var postagens = new List<Postagem> { CriarPostagem(1) };
        _mockService.Setup(s => s.GetByTemaAsync(1)).ReturnsAsync(postagens);

        // Act
        var response = await _client.GetAsync("/api/postagens/filtro?tema=1");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetByFiltro_SemFiltro_DeveRetornar400()
    {
        // Act — sem query params
        var response = await _client.GetAsync("/api/postagens/filtro");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    // ── POST /api/postagens ─────────────────────────────────────

    [Fact]
    public async Task Create_ComDadosValidos_DeveRetornar201()
    {
        // Arrange
        var postagem = CriarPostagem(1);
        _mockService.Setup(s => s.CreateAsync(It.IsAny<Postagem>()))
            .ReturnsAsync(postagem);

        var body = new
        {
            titulo  = "Postagem Teste",
            texto   = "Conteúdo da postagem de teste",
            tema    = new { id = 1 },
            usuario = new { id = 1 }
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/postagens", body);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();
    }

    [Fact]
    public async Task Create_ComTemaOuUsuarioInvalido_DeveRetornar400()
    {
        // Arrange — service retorna null (tema/usuário não existe)
        _mockService.Setup(s => s.CreateAsync(It.IsAny<Postagem>()))
            .ReturnsAsync((Postagem?)null);

        var body = new
        {
            titulo  = "Postagem Teste",
            texto   = "Conteúdo da postagem de teste",
            tema    = new { id = 99 },
            usuario = new { id = 99 }
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/postagens", body);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Create_ComTituloMuitoCurto_DeveRetornar400()
    {
        // Arrange — título com menos de 3 caracteres (viola StringLength)
        var body = new
        {
            titulo  = "AB",
            texto   = "Conteúdo da postagem de teste",
            tema    = new { id = 1 },
            usuario = new { id = 1 }
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/postagens", body);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    // ── PUT /api/postagens/{id} ─────────────────────────────────

    [Fact]
    public async Task Update_ComPostagamExistente_DeveRetornar200()
    {
        // Arrange
        var postagem = CriarPostagem(1);
        _mockService.Setup(s => s.UpdateAsync(It.IsAny<Postagem>()))
            .ReturnsAsync(postagem);

        var body = new
        {
            titulo  = "Postagem Atualizada",
            texto   = "Conteúdo atualizado da postagem",
            tema    = new { id = 1 },
            usuario = new { id = 1 }
        };

        // Act
        var response = await _client.PutAsJsonAsync("/api/postagens/1", body);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Update_ComPostagamInexistente_DeveRetornar404()
    {
        // Arrange — service retorna null (postagem não existe)
        _mockService.Setup(s => s.UpdateAsync(It.IsAny<Postagem>()))
            .ReturnsAsync((Postagem?)null);

        var body = new
        {
            titulo  = "Postagem Atualizada",
            texto   = "Conteúdo atualizado da postagem",
            tema    = new { id = 1 },
            usuario = new { id = 1 }
        };

        // Act
        var response = await _client.PutAsJsonAsync("/api/postagens/99", body);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    // ── DELETE /api/postagens/{id} ──────────────────────────────

    [Fact]
    public async Task Delete_ComIdValido_DeveRetornar204()
    {
        // Arrange
        _mockService.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);

        // Act
        var response = await _client.DeleteAsync("/api/postagens/1");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Delete_ComIdInvalido_DeveRetornar404()
    {
        // Arrange
        _mockService.Setup(s => s.DeleteAsync(99)).ReturnsAsync(false);

        // Act
        var response = await _client.DeleteAsync("/api/postagens/99");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
