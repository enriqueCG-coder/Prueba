using Microsoft.AspNetCore.Mvc;


namespace Prueba.MVC.Controllers
{
    public class TitularesController : Controller
    {
        
        private readonly HttpClient _httpClient;

        public TitularesController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("MyApiClient");
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }


    }
}
