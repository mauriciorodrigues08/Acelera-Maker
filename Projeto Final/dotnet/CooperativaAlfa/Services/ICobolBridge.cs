using CooperativaAlfa.Models;

namespace CooperativaAlfa.Services;

public interface ICobolBridge
{
    Task<CobolResponse> ConsultarClienteAsync(int codigo);
    Task<CobolResponse> AtualizarClienteAsync(int codigo, string telefone, string email);
}
