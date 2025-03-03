using Microsoft.AspNetCore.Mvc;

namespace ForumAPI.Controllers
{
    public class GreenitorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
