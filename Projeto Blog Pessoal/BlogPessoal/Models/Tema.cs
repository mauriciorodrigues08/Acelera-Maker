using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BlogPessoal.Models;

/*
Classe Tema:
    Entidade que representa um Tema do Blog.
    Um Tema é usado para classificar as Postagens.
    Possui relacionamento 1:N com Postagem (um Tema pode ter várias Postagens).
    Gera a tabela tb_temas no banco de dados.
*/

[Table("tb_temas")] // define o nome da tabela no banco
public class Tema
{
    [Key] // marca o campo como chave primária
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // gera id automatico com autoincrement
    public long Id { get; set; }

    // descrição do tema
    [Required(ErrorMessage = "A Descrição é obrigatória.")]
    public string Descricao { get; set; } = string.Empty;

    // postagem do tema
    // evita loop infinito na serialização JSON quando Tema tentar trazer todas as Postagens
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] 
    public virtual ICollection<Postagem>? Postagem { get; set; } // representa o relacionamento 1:N com Postagem
}