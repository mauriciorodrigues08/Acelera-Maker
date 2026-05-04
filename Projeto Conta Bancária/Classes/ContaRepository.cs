namespace Projeto_Conta_Bancaria.Classes;

public interface IContaRepository
{
    public void procurarPorNumero(int _numero);
    public void listarTodas();
    public void cadastrar (Conta _conta);
    public void atualizar (int _numero);
    public void deletar (int _numero);
    public void sacar(int _numero, float _valor);
    public void depositar(int _numero, float _valor);
    public void transferir(int _numeroOrigem, int _numeroDestino, float _valor);
}