using Microsoft.AspNetCore.Mvc;
using PDFShelf.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace PDFShelf.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestDbController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TestDbController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                // Testa a conexão com o banco
                var canConnect = await _context.Database.CanConnectAsync();

                if (!canConnect)
                    return StatusCode(500, "❌ Não foi possível conectar ao banco de dados.");

                // Conta quantos planos existem (os seeds devem estar lá)
                var planCount = await _context.Plans.CountAsync();

                return Ok(new
                {
                    message = "✅ Conexão com o banco bem-sucedida!",
                    plansFound = planCount
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro: {ex.Message}");
            }
        }
    }
}
