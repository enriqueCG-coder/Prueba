﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prueba.API.Context;
using Prueba.API.Models;


namespace Prueba.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TarjetaCredController : Controller
    {
        private readonly ApiDbContext _context;

        public TarjetaCredController(ApiDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("GetTarjetas")]
        public async Task<ActionResult<List<TarjetaCredito>>> GetTarjetas()
        {
            try
            {
                var result = await _context.TarjetaCred.FromSqlRaw("sp_ListarTC").ToListAsync();
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
        [Route("PostTarjetasCred")]
        public async Task<ActionResult<TarjetaCredito>> PostTarjetasCred(TarjetaCredito tc)
        {
            try
            {
                if (string.IsNullOrEmpty(tc.NumeroTarjeta) || tc.LimiteCredito == 0 || tc.PorcentInteres == 0)
                {
                    return BadRequest("Datos de tarjeta no proporcionados.");
                }

                if (tc.NumeroTarjeta.Length != 16 || !tc.NumeroTarjeta.All(char.IsDigit))
                {
                    return BadRequest("El número de la tarjeta debe tener 16 dígitos.");
                }

                var result = await _context.Database.ExecuteSqlRawAsync("sp_InsertarTC @IdTitularTarjeta = {0}, @NumeroTarjeta = {1}, @SaldoActual = {2}, @LimiteCredito = {3}, @PorcentInteres = {4}, @PorcentIntSaldoMin = {5}, @Estado = {6}",
                    tc.IdTitularTarjeta,
                    tc.NumeroTarjeta,
                    tc.SaldoActual,
                    tc.LimiteCredito,
                    tc.PorcentInteres,
                    tc.PorcentIntSaldoMin,
                    tc.Estado ? 1 : 0 
                );

                return Ok(result);
            }
            catch (Exception ex)
            {
                // Manejo de errores
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("PutTarjeta/{idTarjetaCredito}")]
        public async Task<IActionResult> PutTarjeta(int idTarjetaCredito, TarjetaCredito tc)
        {
            if (string.IsNullOrEmpty(tc.NumeroTarjeta) || tc.LimiteCredito == 0 || tc.PorcentInteres == 0)
            {
                return BadRequest("Datos de tarjeta no proporcionados.");
            }

            if (tc.NumeroTarjeta.Length != 16 || !tc.NumeroTarjeta.All(char.IsDigit))
            {
                return BadRequest("El número de la tarjeta debe tener 16 dígitos.");
            }

            try
            {
                var result = await _context.Database.ExecuteSqlRawAsync(
                    "EXEC sp_ModificarTC @IdTarjetaCredito = {0}, @IdTitularTarjeta = {1}, @NumeroTarjeta = {2}, @SaldoActual = {3}, @LimiteCredito = {4}, @PorcentInteres = {5}, @PorcentIntSaldoMin = {6}, @Estado = {7}",
                    tc.IdTarjetaCredito,
                    tc.IdTitularTarjeta,
                    tc.NumeroTarjeta,
                    tc.SaldoActual,
                    tc.LimiteCredito,
                    tc.PorcentInteres,
                    tc.PorcentIntSaldoMin,
                    tc.Estado ? 1 : 0 
                );

                return Ok(result);
            }
            catch (Exception ex)
            {
                // Manejo de errores
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }



        [HttpDelete]
        [Route("DeleteTarjeta/{idTarjetaCredito}")]
        public async Task<IActionResult> DeleteTitular([FromRoute(Name = "idTarjetaCredito")] int id)
        {
            try
            {
                var result = await _context.Database.ExecuteSqlRawAsync(
                    "sp_EliminarTC @IdTarjetaCredito = {0}", id);

                return Ok(result);
            }
            catch (Exception ex)
            {
                // Manejo de errores
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }


        [HttpGet]
        [Route("GetById/{idTarjetaCredito}")]
        public async Task<IActionResult> GetById([FromRoute(Name = "idTarjetaCredito")] int id)
        {
            try
            {
                var result = await _context.TarjetaCred.FromSqlRaw("sp_ListarTCbyID @IdTarjetaCredito = {0} ", id).ToListAsync();

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
