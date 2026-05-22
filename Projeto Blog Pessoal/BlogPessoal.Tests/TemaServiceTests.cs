// Testes unitários para o TemaService.
// Usa Moq para simular o repositório sem precisar do banco de dados.
// Usa FluentAssertions para asserções mais legíveis.

using BlogPessoal.Models;
using BlogPessoal.Repositories;
using BlogPessoal.Services;
using FluentAssertions;
using Moq;

namespace BlogPessoal.Tests;

public class TemaServiceTests
{
    // mock do repositório — simula o banco sem precisar de conexão real
    private readonly Mock<ITemaRepository> _mockRepo;
    private readonly TemaService _service;

    public TemaServiceTests()
    {
        _mockRepo = new Mock<ITemaRepository>();
        _service = new TemaService(_mockRepo.Object);
    }

    [Fact]
    public async Task GetAllAsync_DeveRetornarListaDeTemas()
    {
        // Arrange — configura o mock para retornar uma lista de temas
        var temas = new List<Tema>
        {
            new() { Id = 1, Descricao = "Tecnologia" },
            new() { Id = 2, Descricao = "Programação" }
        };
        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(temas);

        // Act — chama o método que está sendo testado
        var resultado = await _service.GetAllAsync();

        // Assert — verifica se o resultado é o esperado
        resultado.Should().HaveCount(2);
        resultado.Should().Contain(t => t.Descricao == "Tecnologia");
    }

    [Fact]
    public async Task GetByIdAsync_ComIdValido_DeveRetornarTema()
    {
        // Arrange
        var tema = new Tema { Id = 1, Descricao = "Tecnologia" };
        _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(tema);

        // Act
        var resultado = await _service.GetByIdAsync(1);

        // Assert
        resultado.Should().NotBeNull();
        resultado!.Descricao.Should().Be("Tecnologia");
    }

    [Fact]
    public async Task GetByIdAsync_ComIdInvalido_DeveRetornarNull()
    {
        // Arrange — mock retorna null para ID inexistente
        _mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Tema?)null);

        // Act
        var resultado = await _service.GetByIdAsync(99);

        // Assert
        resultado.Should().BeNull();
    }

    [Fact]
    public async Task CreateAsync_DeveCriarTema()
    {
        // Arrange
        var tema = new Tema { Descricao = "Novo Tema" };
        var temaCriado = new Tema { Id = 3, Descricao = "Novo Tema" };
        _mockRepo.Setup(r => r.CreateAsync(tema)).ReturnsAsync(temaCriado);

        // Act
        var resultado = await _service.CreateAsync(tema);

        // Assert
        resultado.Should().NotBeNull();
        resultado.Id.Should().Be(3);
        resultado.Descricao.Should().Be("Novo Tema");
    }

    [Fact]
    public async Task UpdateAsync_ComIdInvalido_DeveRetornarNull()
    {
        // Arrange — tema não existe no banco
        _mockRepo.Setup(r => r.ExistsAsync(99)).ReturnsAsync(false);

        // Act
        var resultado = await _service.UpdateAsync(new Tema { Id = 99 });

        // Assert
        resultado.Should().BeNull();
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
        // Arrange — tema não existe
        _mockRepo.Setup(r => r.ExistsAsync(99)).ReturnsAsync(false);

        // Act
        var resultado = await _service.DeleteAsync(99);

        // Assert
        resultado.Should().BeFalse();
    }
}