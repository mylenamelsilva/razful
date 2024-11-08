using Microsoft.AspNetCore.Mvc;

namespace Front.Controllers
{
    public class TurmasController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
