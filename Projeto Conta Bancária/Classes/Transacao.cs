namespace Projeto_Conta_Bancaria.Classes;

using System.Text.Json.Serialization;

public class Transacao
{
    // atributos
    [JsonInclude] internal float valor;
    [JsonInclude] internal string titularOutraParte;

    // construtor json
    [JsonConstructor]
    protected Transacao()
    {
        this.titularOutraParte = "";
    }

    // construtor principal
    // valor positivo = recebido, negativo = enviado
    public Transacao(float _valor, string _titularOutraParte)
    {
        this.valor = _valor;
        this.titularOutraParte = _titularOutraParte;
    }

    // getters
    public float getValor()
    {
        return this.valor;
    }

    public string getTitularOutraParte()
    {
        return this.titularOutraParte;
    }
}