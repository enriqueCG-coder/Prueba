using System.ComponentModel.DataAnnotations;

namespace Prueba.API.Models
{
    public class Pago
    {
        [Key]
        public int IdPago { get; set; }
        public int IdTarjetaCredito { get; set; }
        public int NumAutoriza { get; set; }
        public DateTime FechaPago { get; set; }
        public decimal Monto { get; set; }

    }
}
