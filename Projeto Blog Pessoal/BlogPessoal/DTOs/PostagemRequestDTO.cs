using System.ComponentModel.DataAnnotations;

namespace BlogPessoal.DTOs;

public class PostagemRequestDTO
{
    [Required, StringLength(255, MinimumLength = 3)]
    public string Titulo { get; set; } = string.Empty;

    [Required, StringLength(10000, MinimumLength = 10)]
    public string Texto { get; set; } = string.Empty;

    public long? TemaId { get; set; }    // ID do Tema
    public long? UsuarioId { get; set; } // ID do Usuário
}