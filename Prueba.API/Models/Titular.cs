using System.ComponentModel.DataAnnotations;

namespace Prueba.API.Models
{
    public class Titular
    {
        [Key]
        public int IdTitularTarjeta { get; set; }  
        public string Nombres { get; set; }      
        public string Apellidos { get; set; }
        public bool Estado { get; set; }

    }
}
