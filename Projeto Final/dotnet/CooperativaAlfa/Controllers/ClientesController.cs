using Microsoft.AspNetCore.Mvc;
using CooperativaAlfa.Models;
using CooperativaAlfa.Services;

namespace CooperativaAlfa.Controllers;

/// <summary>
/// Endpoints para consulta e atualização de clientes.
/// Toda operação de dados é delegada ao componente COBOL via CobolBridge.
/// </summary>
[ApiController]
[Route("clientes")]
[Produces("application/json")]
public class ClientesController : ControllerBase
{
    private readonly ICobolBridge _cobol;
    private readonly ILogger<ClientesController> _logger;

    public ClientesController(ICobolBridge cobol, ILogger<ClientesController> logger)
    {
        _cobol  = cobol;
        _logger = logger;
    }

    /// <summary>
    /// Consulta um cliente pelo código.
    /// </summary>
    /// <param name="codigo">Código do cliente (inteiro positivo).</param>
    /// <returns>Dados cadastrais do cliente.</returns>
    /// <response code="200">Cliente encontrado.</response>
    /// <response code="400">Código inválido.</response>
    /// <response code="404">Cliente não encontrado.</response>
    /// <response code="500">Erro interno ao acessar o sistema legado.</response>
    [HttpGet("{codigo:int}")]
    [ProducesResponseType(typeof(ClienteDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Consultar(int codigo)
    {
        if (codigo <= 0)
            return BadRequest(new { mensagem = "Código deve ser um número inteiro positivo." });

        _logger.LogInformation("Consultando cliente {Codigo}", codigo);

        var resposta = await _cobol.ConsultarClienteAsync(codigo);

        if (resposta.NaoEncontrado)
            return NotFound(new { mensagem = resposta.Mensagem });

        if (resposta.Erro)
            return StatusCode(500, new { mensagem = resposta.Mensagem });

        var cliente = new ClienteDto
        {
            Codigo   = resposta.Codigo ?? codigo,
            Nome     = resposta.Nome     ?? string.Empty,
            Telefone = resposta.Telefone ?? string.Empty,
            Email    = resposta.Email    ?? string.Empty
        };

        return Ok(cliente);
    }

    /// <summary>
    /// Atualiza o telefone e o e-mail de um cliente.
    /// </summary>
    /// <param name="codigo">Código do cliente.</param>
    /// <param name="request">Novos dados de contato.</param>
    /// <response code="200">Dados atualizados com sucesso.</response>
    /// <response code="400">Dados inválidos.</response>
    /// <response code="404">Cliente não encontrado.</response>
    /// <response code="500">Erro interno ao acessar o sistema legado.</response>
    [HttpPut("{codigo:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Atualizar(
        int codigo, [FromBody] AtualizaClienteRequest request)
    {
        if (codigo <= 0)
            return BadRequest(new { mensagem = "Código deve ser um número inteiro positivo." });

        _logger.LogInformation("Atualizando cliente {Codigo}", codigo);

        var resposta = await _cobol.AtualizarClienteAsync(
            codigo, request.Telefone, request.Email);

        if (resposta.NaoEncontrado)
            return NotFound(new { mensagem = resposta.Mensagem });

        if (resposta.Erro)
            return StatusCode(500, new { mensagem = resposta.Mensagem });

        return Ok(new { mensagem = resposta.Mensagem });
    }
}