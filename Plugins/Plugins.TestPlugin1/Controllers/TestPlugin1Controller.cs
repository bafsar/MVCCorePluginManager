using Microsoft.AspNetCore.Mvc;

namespace Plugins.TestPlugin1.Controllers
{
    public class TestPlugin1Controller : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult OtherPage()
        {
            return View();
        }
    }
}
