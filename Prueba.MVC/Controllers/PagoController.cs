using Microsoft.AspNetCore.Mvc;

namespace Prueba.MVC.Controllers
{
    public class PagoController : Controller
    {

        [HttpGet]
        [Route("Pago/Index")]
        public IActionResult Index(int idTarjetaCredito)
        {
            ViewBag.IdTarjetaCredito = idTarjetaCredito;

            return View();
        }
    }
}