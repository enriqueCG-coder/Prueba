using Microsoft.AspNetCore.Mvc;

namespace Prueba.MVC.Controllers
{
    public class CompraController : Controller
    {

        [HttpGet]
        [Route("Compra/Index")]
        public IActionResult Index(int idTarjetaCredito)
        {
            ViewBag.IdTarjetaCredito = idTarjetaCredito;

            return View();
        }
    }
}
