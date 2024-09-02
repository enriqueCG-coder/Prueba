using System.ComponentModel.DataAnnotations;

namespace Prueba.API.Models
{
    public class Compra
    {
        [Key]
        public int? IdCompra { get; set; }               
        public int IdTarjetaCredito { get; set; }
        public int? NumAutoriza { get; set; }
        public DateTime? FechaCompra { get; set; }       
        public string Descripcion { get; set; }         
        public decimal Monto { get; set; }          
    }
}
