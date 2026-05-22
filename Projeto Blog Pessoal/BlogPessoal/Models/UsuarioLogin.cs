using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogPessoal.Models;


/*
Classe UsuarioLogin:
    Classe auxiliar usada apenas para autenticação.
    Recebe email e senha no endpoint POST /api/usuarios/login.
    Não gera tabela no banco de dados.
*/
public class UsuarioLogin
{
    // email do usuario
    [StringLength(255, MinimumLength = 5)]
    [EmailAddress(ErrorMessage = "Formato de email inválido.")]
    public string? Email { get; set; }

    // senha do usuario
    [Required(ErrorMessage = "A Senha é obrigatória.")]
    [Column(TypeName = "varchar(255)")]
    public string? Senha { get; set; }
}