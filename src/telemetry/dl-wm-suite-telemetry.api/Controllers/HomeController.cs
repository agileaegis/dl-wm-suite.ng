using Microsoft.AspNetCore.Mvc;

namespace dl.wm.suite.telemetry.api.Controllers
{
  public class HomeController : Controller
  {

    // GET: /<controller>/
    public IActionResult Index()
    {
      return new RedirectResult("~/swagger");
    }
  }
}