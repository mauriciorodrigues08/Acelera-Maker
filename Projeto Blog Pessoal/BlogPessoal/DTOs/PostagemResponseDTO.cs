// DTO de resposta para Postagem.
// Evita retornar dados desnecessários e previne
// loops de serialização entre entidades relacionadas.

namespace BlogPessoal.DTOs;

public class PostagemResponseDTO
{
    public long Id { get; set; }
    public string? Titulo { get; set; }
    public string? Texto { get; set; }
    public DateTime? Data { get; set; }
    public string? ResumoIA { get; set; }
    public string? TagsIA { get; set; }
    public string? CategoriaIA { get; set; }

    // retorna apenas os dados essenciais do Tema
    public TemaResumoDTO? Tema { get; set; }

    // retorna apenas os dados essenciais do Usuario
    public UsuarioResumoDTO? Usuario { get; set; }
}

public class TemaResumoDTO
{
    public long Id { get; set; }
    public string? Descricao { get; set; }
}

public class UsuarioResumoDTO
{
    public long Id { get; set; }
    public string? Nome { get; set; }
    public string? Email { get; set; }
    public string? Foto { get; set; }
}