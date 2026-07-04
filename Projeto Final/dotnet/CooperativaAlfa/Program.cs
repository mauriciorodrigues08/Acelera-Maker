using CooperativaAlfa.Services;

var builder = WebApplication.CreateBuilder(args);

// ── Serviços ────────────────────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title       = "Cooperativa Alfa — API de Clientes",
        Version     = "v1",
        Description = "API de modernização do cadastro de clientes. " +
                      "Todas as operações são processadas pelo sistema legado COBOL."
    });

    // Inclui os comentários XML dos controllers no Swagger
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        c.IncludeXmlComments(xmlPath);
});

// CobolBridge registrado como Transient:
// cada requisição cria sua própria instância e seu próprio processo COBOL
builder.Services.AddTransient<CobolBridge>();

// ── Pipeline ────────────────────────────────────────────────────
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cooperativa Alfa v1");
    c.RoutePrefix = string.Empty; // Swagger na raiz: http://localhost:5210
});

app.UseAuthorization();
app.MapControllers();
app.Run();