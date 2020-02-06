using Microsoft.AspNetCore.Mvc;

namespace dl.wm.suite.fleet.api.Controllers
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