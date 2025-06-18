using Microsoft.AspNetCore.Mvc;

namespace TM.Models.ViewModels
{
    public class TourViewModel : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
