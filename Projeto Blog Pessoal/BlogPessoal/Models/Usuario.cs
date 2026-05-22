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
    [Column(TypeName = "varchar(255)")]
    [StringLength(255, MinimumLength = 3,
        ErrorMessage = "O Nome deve ter entre 3 e 255 caracteres.")]
    public string? Nome { get; set; }

    // email 
    [Column(TypeName = "varchar(255)")]
    [StringLength(255, MinimumLength = 5,
        ErrorMessage = "O Email deve ter entre 5 e 255 caracteres.")]
    public string? Email { get; set; }

    // senha (hash da senha, e não string pura)
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [Column(TypeName = "varchar(255)")]
    [StringLength(255, MinimumLength = 8, ErrorMessage = "A Senha deve ter no mínimo 8 caracteres.")]
    public string? Senha { get; set; }

    // foto (url da foto)
    [Column(TypeName = "varchar(5000)")]
    public string? Foto { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public virtual ICollection<Postagem>? Postagem { get; set; }
}