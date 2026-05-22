using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogPessoal.Models;

/*
Classe Postagem:
    Entidade que representa uma Postagem do Blog.
    Uma Postagem pertence a um Usuário e a um Tema.
    Possui relacionamento N:1 com Tema e N:1 com Usuario.
    Contém campos opcionais gerados por IA (ResumoIA, TagsIA, CategoriaIA).
    Gera a tabela tb_postagens no banco de dados.
*/

[Table("tb_postagens")]
public class Postagem
{
    // id da postagem
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    // título da postagem
    [Required(ErrorMessage = "O Título é obrigatório.")]
    [StringLength(255, MinimumLength = 3, ErrorMessage = "...")]
    public string Titulo { get; set; } = string.Empty;

    // texto da postagem
    [Required(ErrorMessage = "O Texto é obrigatório.")]
    [StringLength(10000, MinimumLength = 10, ErrorMessage = "...")]
    public string Texto { get; set; } = string.Empty;

    // data da postagem (preenchida automaticamente)
    public DateTime? Data { get; set; } = DateTime.Now;

    // Campos gerados pela IA
    public string? ResumoIA { get; set; }
    public string? TagsIA { get; set; }
    public string? CategoriaIA { get; set; }

    // Relacionamento com Tema (N:1)
    public virtual Tema? Tema { get; set; }

    // Relacionamento com Usuario (N:1)
    public virtual Usuario? Usuario { get; set; }
}