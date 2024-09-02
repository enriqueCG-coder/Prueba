using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prueba.API.Context;
using Prueba.API.Models;
using FluentValidation;

namespace Prueba.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TitularesController : Controller
    {
        private readonly ApiDbContext _context;

        public TitularesController(ApiDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("GetTitulares")]
        public async Task<ActionResult<List<Titular>>> GetTitulares()
        {
            try
            {
                var result = await _context.Titulares.FromSqlRaw("sp_ListarTitulares").ToListAsync();
                if (result == null)
                {
                    return BadRequest("No existen registros actualmente.");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Manejo de errores
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("PostTitular")]
        public async Task<ActionResult<Titular>> PostTitular(Titular titular)
        {
            try
            {
                if (titular == null)
                {
                    return BadRequest("Datos del titular no proporcionados.");
                }

                var result = await _context.Database.ExecuteSqlRawAsync("sp_InsertarTitularTarjeta @Nombres = {0}, @Apellidos = {1}",
                    titular.Nombres, titular.Apellidos);

                return Ok(result);
            }
            catch (Exception ex)
            {
                // Manejo de errores
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("PutTitular/{idTitularTarjeta}")]
        public async Task<IActionResult> PutTitular(int id, Titular titular)
        {
            
            if (titular == null )
            {
                return BadRequest("Datos del titular no proporcionados");
            }

            try
            {
                var result = await _context.Database.ExecuteSqlRawAsync("sp_ModificarTitularTarjeta @IdTitularTarjeta = {0}, @Nombres = {1}, @Apellidos = {2}, @Estado = {3}",
                    titular.IdTitularTarjeta, titular.Nombres, titular.Apellidos, titular.Estado);

                return Ok(result);
            }
            catch (Exception ex)
            {
                // Manejo de errores
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }


        [HttpDelete]
        [Route("DeleteTitular/{idTitularTarjeta}")]
        public async Task<IActionResult> DeleteTitular([FromRoute(Name = "idTitularTarjeta")] int id)
        {
            try
            {
                var result = await _context.Database.ExecuteSqlRawAsync(
                    "sp_EliminarTitularTarjeta @IdTitularTarjeta = {0}", id);

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
