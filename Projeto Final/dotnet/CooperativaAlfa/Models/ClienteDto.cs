namespace CooperativaAlfa.Models;

/// <summary>
/// Representa os dados cadastrais de um cliente.
/// Utilizado tanto na resposta da consulta quanto na requisição de atualização.
/// </summary>
public class ClienteDto
{
    public int Codigo { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}