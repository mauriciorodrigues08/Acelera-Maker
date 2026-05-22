// DTOs/UsuarioRequestDTO.cs
using System.ComponentModel.DataAnnotations;

namespace BlogPessoal.DTOs
{
    public class UsuarioRequestDTO
    {
        [Required]
        public string Nome { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, MinLength(8)]
        public string Senha { get; set; } = string.Empty;

        public string? Foto { get; set; }
    }
}