using Microsoft.EntityFrameworkCore;
using PDFShelf.Api.Data;
using PDFShelf.Api.Models;  

namespace PDFShelf.Api.Endpoints
{
    public static class TestDbEndpoint
    {
        public static void MapTestDbEdnpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/testdb").WithTags("Database Test");

            group.MapGet("/", async (AppDbContext context) =>
            {
                try
                {
                    // Testa a conexão com o banco
                    var canConnect = await context.Database.CanConnectAsync();

                    if (!canConnect)
                        return Results.Problem("❌ Não foi possível conectar ao banco de dados.", statusCode: 500);

                    // Conta quantos planos existem (os seeds devem estar lá)
                    var planCount = await context.Plans.CountAsync();

                    return Results.Ok(new
                    {
                        message = "✅ Conexão com o banco bem-sucedida!",
                        plansFound = planCount
                    });
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Erro: {ex.Message}", statusCode: 500);
                }
            }).WithName("TestDatabaseConnection")
            .WithOpenApi(op => new(op)
            {
                Summary = "Testa a conexão com o banco de dados e retorna a contagem de planos.",
                Description = "Este endpoint verifica se a aplicação consegue se conectar ao banco de dados e retorna o número de planos existentes na tabela Plans."
            });
        }
    }
}