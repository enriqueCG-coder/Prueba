using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prueba.API.Context;
using Prueba.API.Models;

namespace Prueba.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagoController : Controller
    {
        private readonly ApiDbContext _context;

        public PagoController(ApiDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("PostPago/{idTarjetaCredito}")]
        public async Task<ActionResult<Compra>> PostPago([FromRoute(Name = "idTarjetaCredito")] int id, Pago c)
        {
            try
            {
                if (c == null)
                {
                    return BadRequest("Monto no ingresado.");
                }

                var result = await _context.Database.ExecuteSqlRawAsync("sp_InsertarPago @IdTarjetaCredito = {0}, @Monto = {1} ",
                  id,
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
