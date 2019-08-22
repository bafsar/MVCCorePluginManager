using Microsoft.AspNetCore.Mvc;
using Plugins.TestPlugin2.Models;

namespace Plugins.TestPlugin2.Controllers
{
    public class TestPlugin2Controller : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public string StringTest()
        {
            return "Test Value";
        }

        public IActionResult JsonTest()
        {
            return Json(new SampleModelList());
        }
    }
}
