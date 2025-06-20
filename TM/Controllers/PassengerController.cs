using Microsoft.AspNetCore.Mvc;

namespace TM.Controllers
{
    public class PassengerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
