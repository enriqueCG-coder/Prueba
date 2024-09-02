using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prueba.API.Context;
using Prueba.API.Models;

namespace Prueba.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompraController : Controller
    {
        private readonly ApiDbContext _context;

        public CompraController(ApiDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("PostCompra/{idTarjetaCredito}")]
        public async Task<ActionResult<Compra>> PostCompra([FromRoute(Name = "idTarjetaCredito")] int id, Compra c)
        {
            try
            {
                if (string.IsNullOrEmpty(c.Descripcion) || c.Monto == 0 )
                {
                    return BadRequest("Datos de compra no proporcionados.");
                }

                var result = await _context.Database.ExecuteSqlRawAsync("sp_InsertarCompra @IdTarjetaCredito = {0}, @Descripcion = {1}, @Monto = {2} ",
                  id,
                  c.Descripcion,
                  c.Monto
                );

                return Ok(result);
            }
            catch (Exception ex)
            {
                // Manejo de errores
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

    }
}
