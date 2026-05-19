using BlogPessoal.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogPessoal.Data;

/*
Classe AppDbContext:
    Classe responsável por configurar a conexão com o banco de dados.
    É o ponto central do Entity Framework Core na aplicação.
    Mapeia as entidades para as tabelas do banco e configura os relacionamentos.
*/

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // representa a tabela tb_temas no banco de dados
    public DbSet<Tema> Temas { get; set; } = null!;

    // representa a tabela tb_usuarios no banco de dados
    public DbSet<Usuario> Usuarios { get; set; } = null!;

    // representa a tabela tb_postagens no banco de dados
    public DbSet<Postagem> Postagens { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // configura os relacionamentos explicitamente com chaves estrangeiras (TemaId e UsuarioId)
        base.OnModelCreating(modelBuilder);

        // Configura o relacionamento Tema 1:N Postagem
        modelBuilder.Entity<Postagem>()
            .HasOne(p => p.Tema)
            .WithMany(t => t.Postagem)
            .HasForeignKey("TemaId")
            .OnDelete(DeleteBehavior.Restrict); // impede deletar um tema que ainda possua postagens

        // Configura o relacionamento Usuario 1:N Postagem
        modelBuilder.Entity<Postagem>()
            .HasOne(p => p.Usuario)
            .WithMany(u => u.Postagem)
            .HasForeignKey("UsuarioId")
            .OnDelete(DeleteBehavior.Restrict); // impede deletar um usuario que ainda possua postagens
    }
}