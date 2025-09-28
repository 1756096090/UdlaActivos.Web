using Microsoft.AspNetCore.Mvc;

namespace UdlaActivos.Web.Controllers
{
    public class HomeController1 : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
