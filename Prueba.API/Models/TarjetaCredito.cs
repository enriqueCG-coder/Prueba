using System.ComponentModel.DataAnnotations;

namespace Prueba.API.Models
{
    public class TarjetaCredito
    {
        [Key]
        public int IdTarjetaCredito { get; set; }       
        public int IdTitularTarjeta { get; set; }       
        public string? Nombres { get; set; }       
        public string? Apellidos { get; set; }       
        public string NumeroTarjeta { get; set; }       
        public decimal? SaldoActual { get; set; }        
        public decimal LimiteCredito { get; set; }     
        public decimal SaldoDisponible { get; set; }     
        public decimal? PorcentInteres { get; set; }     
        public decimal? PorcentIntSaldoMin { get; set; }     
        public decimal CuotaMinima { get; set; }     
        public decimal MontoContado { get; set; }     
        public decimal IntBonificable { get; set; }     
        public bool Estado { get; set; }     
    }
}
