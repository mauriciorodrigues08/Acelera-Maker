// Testes unitários para o UsuarioService.
// Valida as regras de negócio de cadastro, atualização e autenticação.

using BlogPessoal.Models;
using BlogPessoal.Repositories;
using BlogPessoal.Services;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;

namespace BlogPessoal.Tests;

public class UsuarioServiceTests
{
    private readonly Mock<IUsuarioRepository> _mockRepo;
    private readonly Mock<IConfiguration> _mockConfig;
    private readonly UsuarioService _service;

    public UsuarioServiceTests()
    {
        _mockRepo = new Mock<IUsuarioRepository>();
        _mockConfig = new Mock<IConfiguration>();

        // configura a chave JWT para os testes
        _mockConfig.Setup(c => c["Jwt:Key"])
            .Returns("ChaveSuperSecretaParaTestesBlogPessoal123!");
        _mockConfig.Setup(c => c["Jwt:Issuer"]).Returns("BlogPessoal");
        _mockConfig.Setup(c => c["Jwt:Audience"]).Returns("BlogPessoal");

        var jwtService = new JwtService(_mockConfig.Object);
        _service = new UsuarioService(_mockRepo.Object, jwtService);
    }

    [Fact]
    public async Task CreateAsync_ComEmailNovo_DeveCriarUsuario()
    {
        // Arrange
        var usuario = new Usuario
        {
            Nome = "João",
            Email = "joao@email.com",
            Senha = "12345678"
        };

        // email não existe ainda
        _mockRepo.Setup(r => r.EmailExistsAsync("joao@email.com"))
            .ReturnsAsync(false);

        // simula o retorno após criar
        _mockRepo.Setup(r => r.CreateAsync(It.IsAny<Usuario>()))
            .ReturnsAsync((Usuario u) => u);

        // Act
        var resultado = await _service.CreateAsync(usuario);

        // Assert
        resultado.Should().NotBeNull();
        resultado!.Email.Should().Be("joao@email.com");
        // verifica que a senha foi hasheada (não é mais o texto puro)
        resultado.Senha.Should().NotBe("12345678");
    }

    [Fact]
    public async Task CreateAsync_ComEmailExistente_DeveRetornarNull()
    {
        // Arrange — email já está em uso
        var usuario = new Usuario { Email = "joao@email.com", Senha = "12345678" };
        _mockRepo.Setup(r => r.EmailExistsAsync("joao@email.com"))
            .ReturnsAsync(true);

        // Act
        var resultado = await _service.CreateAsync(usuario);

        // Assert
        resultado.Should().BeNull();
    }

    [Fact]
    public async Task LoginAsync_ComCredenciaisValidas_DeveRetornarToken()
    {
        // Arrange
        var senhaHash = BCrypt.Net.BCrypt.HashPassword("admin123");
        var usuario = new Usuario
        {
            Id = 1,
            Nome = "Maurício",
            Email = "mauricio@email.com",
            Senha = senhaHash
        };

        _mockRepo.Setup(r => r.GetByEmailAsync("mauricio@email.com"))
            .ReturnsAsync(usuario);

        var login = new UsuarioLogin
        {
            Email = "mauricio@email.com",
            Senha = "admin123"
        };

        // Act
        var token = await _service.LoginAsync(login);

        // Assert
        token.Should().NotBeNull();
        token.Should().StartWith("eyJ"); // tokens JWT sempre começam com eyJ
    }

    [Fact]
    public async Task LoginAsync_ComSenhaInvalida_DeveRetornarNull()
    {
        // Arrange
        var senhaHash = BCrypt.Net.BCrypt.HashPassword("senhaCorreta");
        var usuario = new Usuario
        {
            Email = "mauricio@email.com",
            Senha = senhaHash
        };

        _mockRepo.Setup(r => r.GetByEmailAsync("mauricio@email.com"))
            .ReturnsAsync(usuario);

        var login = new UsuarioLogin
        {
            Email = "mauricio@email.com",
            Senha = "senhaErrada"
        };

        // Act
        var token = await _service.LoginAsync(login);

        // Assert
        token.Should().BeNull();
    }

    [Fact]
    public async Task LoginAsync_ComEmailInexistente_DeveRetornarNull()
    {
        // Arrange — usuário não encontrado
        _mockRepo.Setup(r => r.GetByEmailAsync("inexistente@email.com"))
            .ReturnsAsync((Usuario?)null);

        var login = new UsuarioLogin
        {
            Email = "inexistente@email.com",
            Senha = "qualquersenha"
        };

        // Act
        var token = await _service.LoginAsync(login);

        // Assert
        token.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_ComIdValido_DeveRetornarTrue()
    {
        // Arrange
        _mockRepo.Setup(r => r.ExistsAsync(1)).ReturnsAsync(true);
        _mockRepo.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

        // Act
        var resultado = await _service.DeleteAsync(1);

        // Assert
        resultado.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteAsync_ComIdInvalido_DeveRetornarFalse()
    {
        // Arrange
        _mockRepo.Setup(r => r.ExistsAsync(99)).ReturnsAsync(false);

        // Act
        var resultado = await _service.DeleteAsync(99);

        // Assert
        resultado.Should().BeFalse();
    }
}