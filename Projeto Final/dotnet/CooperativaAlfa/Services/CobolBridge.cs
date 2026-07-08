using System.Diagnostics;
using System.Text;
using System.Text.Json;
using CooperativaAlfa.Models;

namespace CooperativaAlfa.Services;

/// <summary>
/// Responsável por chamar o executável COBOL como processo do sistema,
/// passar o JSON de entrada via stdin e ler o JSON de resposta via stdout.
///
/// Decisão arquitetural: o .NET nunca acessa o banco diretamente —
/// toda operação de dados passa pelo componente COBOL, que é a fonte
/// de verdade do sistema legado.
/// </summary>
public class CobolBridge : ICobolBridge
{
    private readonly string _executablePath;
    private readonly string _odbcIniPath;
    private readonly ILogger<CobolBridge> _logger;

    public CobolBridge(IConfiguration config, ILogger<CobolBridge> logger)
    {
        _executablePath = config["Cobol:ExecutablePath"]
            ?? throw new InvalidOperationException(
                "Configuração 'Cobol:ExecutablePath' não encontrada no appsettings.json.");

        _odbcIniPath = config["Cobol:OdbcIniPath"]
            ?? throw new InvalidOperationException(
                "Configuração 'Cobol:OdbcIniPath' não encontrada no appsettings.json.");

        _logger = logger;
    }

    /// <summary>
    /// Executa uma consulta de cliente pelo código.
    /// </summary>
    virtual public async Task<CobolResponse> ConsultarClienteAsync(int codigo)
    {
        var input = JsonSerializer.Serialize(new
        {
            operacao = "C",
            codigo
        });

        return await ExecutarAsync(input);
    }

    /// <summary>
    /// Executa a atualização de telefone e e-mail de um cliente.
    /// </summary>
    virtual public async Task<CobolResponse> AtualizarClienteAsync(
        int codigo, string telefone, string email)
    {
        var input = JsonSerializer.Serialize(new
        {
            operacao = "A",
            codigo,
            telefone,
            email
        });

        return await ExecutarAsync(input);
    }

    /// <summary>
    /// Inicia o processo COBOL, envia o JSON via stdin,
    /// aguarda a resposta no stdout e desserializa o retorno.
    /// </summary>
    private async Task<CobolResponse> ExecutarAsync(string jsonInput)
    {
        _logger.LogDebug("Chamando COBOL com entrada: {Input}", jsonInput);

        var psi = new ProcessStartInfo
        {
            FileName = _executablePath,
            RedirectStandardInput  = true,
            RedirectStandardOutput = true,
            RedirectStandardError  = true,
            UseShellExecute = false,
            StandardInputEncoding  = Encoding.UTF8,
            StandardOutputEncoding = Encoding.UTF8
        };

        // ODBCINI precisa estar definido no ambiente do processo COBOL
        // para que ele consiga localizar o DSN do SQLite
        psi.Environment["ODBCINI"] = _odbcIniPath;

        using var process = new Process { StartInfo = psi };

        try
        {
            process.Start();

            await process.StandardInput.WriteLineAsync(jsonInput);
            process.StandardInput.Close();

            var output = await process.StandardOutput.ReadToEndAsync();
            var erro   = await process.StandardError.ReadToEndAsync();

            await process.WaitForExitAsync();

            if (!string.IsNullOrWhiteSpace(erro))
                _logger.LogWarning("COBOL stderr: {Erro}", erro);

            _logger.LogDebug("COBOL stdout: {Output}", output);

            if (string.IsNullOrWhiteSpace(output))
                return ErroInterno("O processo COBOL não retornou nenhuma saída.");

            var response = JsonSerializer.Deserialize<CobolResponse>(output);
            return response ?? ErroInterno("Não foi possível interpretar a resposta do COBOL.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao executar o processo COBOL.");
            return ErroInterno($"Erro ao executar o processo COBOL: {ex.Message}");
        }
    }

    private static CobolResponse ErroInterno(string mensagem) => new()
    {
        Status   = "08",
        Mensagem = mensagem
    };
}