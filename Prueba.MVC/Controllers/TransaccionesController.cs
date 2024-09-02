using Microsoft.AspNetCore.Mvc;

namespace Prueba.MVC.Controllers
{
    
    public class TransaccionesController : Controller
    {
        
        [HttpGet]
        [Route("Transacciones/Index")]
        public IActionResult Index(int idTarjetaCredito)
        {
            ViewBag.IdTarjetaCredito = idTarjetaCredito;

            return View();
        }

        [HttpGet]
        [Route("Transacciones/EstadoCuenta")]
        public IActionResult EstadoCuenta(int idTarjetaCredito)
        {
            ViewBag.IdTarjetaCredito = idTarjetaCredito;

            return View();
        }
    }
}
