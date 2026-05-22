// Testes unitários para o PostagemService.
// Valida as regras de negócio de criação e atualização de postagens,
// incluindo a validação de Tema e Usuario existentes.

using BlogPessoal.Models;
using BlogPessoal.Repositories;
using BlogPessoal.Services;
using BlogPessoal.Services.IA;
using FluentAssertions;
using Moq;

namespace BlogPessoal.Tests;

public class PostagemServiceTests
{
    private readonly Mock<IPostagemRepository> _mockPostagemRepo;
    private readonly Mock<ITemaRepository> _mockTemaRepo;
    private readonly Mock<IUsuarioRepository> _mockUsuarioRepo;
    private readonly PostagemService _service;

    public PostagemServiceTests()
    {
        _mockPostagemRepo = new Mock<IPostagemRepository>();
        _mockTemaRepo = new Mock<ITemaRepository>();
        _mockUsuarioRepo = new Mock<IUsuarioRepository>();

        _service = new PostagemService(
            _mockPostagemRepo.Object,
            _mockTemaRepo.Object,
            _mockUsuarioRepo.Object
        );
    }

    [Fact]
    public async Task GetAllAsync_DeveRetornarListaDePostagens()
    {
        // Arrange
        var postagens = new List<Postagem>
        {
            new() { Id = 1, Titulo = "Post 1" },
            new() { Id = 2, Titulo = "Post 2" }
        };
        _mockPostagemRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(postagens);

        // Act
        var resultado = await _service.GetAllAsync();

        // Assert
        resultado.Should().HaveCount(2);
    }

    [Fact]
    public async Task CreateAsync_ComTemaEUsuarioValidos_DeveCriarPostagem()
    {
        // Arrange
        var postagem = new Postagem
        {
            Titulo = "Nova postagem",
            Texto = "Conteúdo da postagem",
            Tema = new Tema { Id = 1 },
            Usuario = new Usuario { Id = 1 }
        };

        _mockTemaRepo.Setup(r => r.ExistsAsync(1)).ReturnsAsync(true);
        _mockUsuarioRepo.Setup(r => r.ExistsAsync(1)).ReturnsAsync(true);
        _mockPostagemRepo.Setup(r => r.CreateAsync(It.IsAny<Postagem>())).ReturnsAsync((Postagem p) => p);

        // Act
        var resultado = await _service.CreateAsync(postagem);

        // Assert
        resultado.Should().NotBeNull();
        resultado!.Titulo.Should().Be("Nova postagem");
    }

    [Fact]
    public async Task CreateAsync_ComTemaInvalido_DeveRetornarNull()
    {
        // Arrange — tema não existe
        var postagem = new Postagem
        {
            Titulo = "Nova postagem",
            Tema = new Tema { Id = 99 },
            Usuario = new Usuario { Id = 1 }
        };
        _mockTemaRepo.Setup(r => r.ExistsAsync(99)).ReturnsAsync(false);

        // Act
        var resultado = await _service.CreateAsync(postagem);

        // Assert
        resultado.Should().BeNull();
    }

    [Fact]
    public async Task CreateAsync_ComUsuarioInvalido_DeveRetornarNull()
    {
        // Arrange — usuario não existe
        var postagem = new Postagem
        {
            Titulo = "Nova postagem",
            Tema = new Tema { Id = 1 },
            Usuario = new Usuario { Id = 99 }
        };
        _mockTemaRepo.Setup(r => r.ExistsAsync(1)).ReturnsAsync(true);
        _mockUsuarioRepo.Setup(r => r.ExistsAsync(99)).ReturnsAsync(false);

        // Act
        var resultado = await _service.CreateAsync(postagem);

        // Assert
        resultado.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_ComPostagemInexistente_DeveRetornarNull()
    {
        // Arrange
        _mockPostagemRepo.Setup(r => r.ExistsAsync(99)).ReturnsAsync(false);

        // Act
        var resultado = await _service.UpdateAsync(new Postagem { Id = 99 });

        // Assert
        resultado.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_ComIdValido_DeveRetornarTrue()
    {
        // Arrange
        _mockPostagemRepo.Setup(r => r.ExistsAsync(1)).ReturnsAsync(true);
        _mockPostagemRepo.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

        // Act
        var resultado = await _service.DeleteAsync(1);

        // Assert
        resultado.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteAsync_ComIdInvalido_DeveRetornarFalse()
    {
        // Arrange
        _mockPostagemRepo.Setup(r => r.ExistsAsync(99)).ReturnsAsync(false);

        // Act
        var resultado = await _service.DeleteAsync(99);

        // Assert
        resultado.Should().BeFalse();
    }
}