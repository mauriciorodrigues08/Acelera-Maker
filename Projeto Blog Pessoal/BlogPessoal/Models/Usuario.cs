using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BlogPessoal.Models;

/*
Classe Usuário:
    Entidade que representa um Usuário do Blog.
    Um Usuário pode criar e gerenciar Postagens.
    A senha é armazenada como hash (nunca em texto puro).
    Possui relacionamento 1:N com Postagem (um Usuário pode ter várias Postagens).
    Gera a tabela tb_usuarios no banco de dados.
*/

[Table("tb_usuarios")]
public class Usuario
{    
    // id
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    // nome
    [Required(ErrorMessage = "O Nome é obrigatório.")]
    public string Nome { get; set; } = string.Empty;

    // email
    [Required(ErrorMessage = "O Email é obrigatório.")]
    [EmailAddress(ErrorMessage = "Formato de email inválido.")]
    public string Email { get; set; } = string.Empty;

    // senha
    [Required(ErrorMessage = "A Senha é obrigatória.")]
    public string Senha { get; set; } = string.Empty;

    // foto (url da foto)
    [Column(TypeName = "varchar(5000)")]
    public string? Foto { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public virtual ICollection<Postagem>? Postagem { get; set; }
}