namespace Projeto_Conta_Bancaria.Classes;

public static class Cores
{
    // ======== Definição de Cores ========
    // Códigos ANSI de cor de texto 
    private const string Reset   = "\u001b[0m";
    private const string Negrito = "\u001b[1m";
 
    // Cores de texto
    private const string TextoPreto    = "\u001b[30m";
    private const string TextoVermelho = "\u001b[31m";
    private const string TextoVerde    = "\u001b[32m";
    private const string TextoAmarelo  = "\u001b[33m";
    private const string TextoAzul     = "\u001b[34m";
    private const string TextoMagenta  = "\u001b[35m";
    private const string TextoCiano    = "\u001b[36m";
    private const string TextoBranco   = "\u001b[37m";
 
    // Cores de fundo
    private const string FundoPreto    = "\u001b[40m";
    private const string FundoVermelho = "\u001b[41m";
    private const string FundoVerde    = "\u001b[42m";
    private const string FundoAmarelo  = "\u001b[43m";
    private const string FundoAzul     = "\u001b[44m";
    private const string FundoMagenta  = "\u001b[45m";
    private const string FundoCiano    = "\u001b[46m";
    private const string FundoBranco   = "\u001b[47m";

    // ======== Impressão de textos ========
    // Imprime o texto na cor indicada, seguido de nova linha
    public static void WriteLine(string texto, string cor)
    {
        Console.WriteLine($"{cor}{texto}{Reset}");
    }
 
    // Imprime o texto na cor indicada, sem nova linha
    public static void Write(string texto, string cor)
    {
        Console.Write($"{cor}{texto}{Reset}");
    }

    // ======== Atalhos prontos para uso no Menu ========
    public static void Titulo(string texto)
        => Console.WriteLine($"{Negrito}{TextoCiano}{texto}{Reset}");
 
    public static void Sucesso(string texto)
        => Console.WriteLine($"{Negrito}{TextoVerde}{texto}{Reset}");
 
    public static void Erro(string texto)
        => Console.WriteLine($"{Negrito}{TextoVermelho}{texto}{Reset}");
 
    public static void Aviso(string texto)
        => Console.WriteLine($"{Negrito}{TextoAmarelo}{texto}{Reset}");
 
    public static void Info(string texto)
        => Console.WriteLine($"{TextoBranco}{texto}{Reset}");
 
    public static void Destaque(string texto)
        => Console.WriteLine($"{Negrito}{TextoMagenta}{texto}{Reset}");


    // ======== Utilitários de formatação do Menu ========
    // Imprime uma linha separadora colorida
    public static void Separador(char caractere = '*', int tamanho = 60, string? cor = null)
    {
        string linha = new string(caractere, tamanho);
        Console.WriteLine($"{cor ?? TextoCiano}{linha}{Reset}");
    }
 
    // Imprime o cabeçalho do menu centralizado com separadores
    public static void Cabecalho(string titulo, int largura = 60)
    {
        Separador('*', largura);
        string centralizado = titulo.PadLeft((largura + titulo.Length) / 2).PadRight(largura);
        Console.WriteLine($"{Negrito}{FundoAzul}{TextoBranco}{centralizado}{Reset}");
        Separador('*', largura);
        Console.WriteLine();
    }
 
    // Retorna a string de cor ANSI pelo nome (para uso externo)
    public static string ObterCor(string nomeCor)
    {
        return nomeCor.ToLower() switch
        {
            "preto"    => TextoPreto,
            "vermelho" => TextoVermelho,
            "verde"    => TextoVerde,
            "amarelo"  => TextoAmarelo,
            "azul"     => TextoAzul,
            "magenta"  => TextoMagenta,
            "ciano"    => TextoCiano,
            "branco"   => TextoBranco,
            "negrito"  => Negrito,
            _          => Reset
        };
    }
 
    // Reseta todas as cores do terminal
    public static void ResetarCores()
    {
        Console.Write(Reset);
    }

}