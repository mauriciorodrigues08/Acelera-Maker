using System.ComponentModel.DataAnnotations;

namespace CooperativaAlfa.Models;

/// <summary>
/// Payload recebido no endpoint PUT /clientes/{codigo}.
/// Contém apenas os campos que o atendente pode alterar.
/// </summary>
public class AtualizaClienteRequest
{
    [Required(ErrorMessage = "Telefone é obrigatório.")]
    [StringLength(15, MinimumLength = 10, ErrorMessage = "Telefone deve ter entre 10 e 15 caracteres.")]
    [RegularExpression(@"^\d+$", ErrorMessage = "Telefone deve conter apenas dígitos.")]
    public string Telefone { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email é obrigatório.")]
    [EmailAddress(ErrorMessage = "Email inválido.")]
    [StringLength(60, ErrorMessage = "Email deve ter no máximo 60 caracteres.")]
    public string Email { get; set; } = string.Empty;
}