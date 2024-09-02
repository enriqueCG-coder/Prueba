using System.ComponentModel.DataAnnotations;

namespace Prueba.API.Models
{
    public class Transacciones
    {
        [Key] 
        public int NumAutorizacion { get; set; }
        public DateTime Fecha { get; set; }
        public string? Descripcion { get; set; }
        public decimal Cargo { get; set; }
        public decimal? Abono { get; set; }
    }
}
