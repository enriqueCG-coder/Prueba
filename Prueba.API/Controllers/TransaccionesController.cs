using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prueba.API.Context;
using Prueba.API.Models;
using System.Linq;

namespace Prueba.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransaccionesController : Controller
    {
        private readonly ApiDbContext _context;

        public TransaccionesController(ApiDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("GetTransaccionesById/{idTarjetaCredito}")]
        public async Task<IActionResult> GetTransaccionesById([FromRoute(Name = "idTarjetaCredito")] int id)
        {
            try
            {
                var result = await _context.Transacciones.FromSqlRaw("sp_TransaccionesTC_ID @IdTarjetaCredito = {0} ", id).ToListAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                // Manejo de errores
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }


        [HttpGet]
        [Route("GetEstadoCuentaById/{idTarjetaCredito}")]
        public async Task<IActionResult> GetEstadoCuentaById([FromRoute(Name = "idTarjetaCredito")] int id)
        {
            try
            {
                var result = await _context.Transacciones.FromSqlRaw("sp_EstadoCuentaTC_ID @IdTarjetaCredito = {0} ", id).ToListAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                // Manejo de errores
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }


        [HttpGet]
        [Route("GetMontoByMes/{idTarjetaCredito}")]
        public async Task<IActionResult> GetMontoByMes([FromRoute(Name = "idTarjetaCredito")] int id, int mes, int anio)
        {
            try
            {
                var resultados = await _context.Set<TransaccionCargo>()
                    .FromSqlInterpolated($"EXEC sp_TotalEstadoCuentaTC_ID @IdTarjetaCredito = {id}, @Mes = {mes}, @Anio = {anio}")
                    .ToListAsync();

                decimal montoTotal = resultados.Sum(r => r.TotalCargo);

                return Ok(montoTotal); // Retornamos el monto total.
            }
            catch (Exception ex)
            {
                // Manejo de errores
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }




    }
}
