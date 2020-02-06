using Microsoft.AspNetCore.Mvc;

namespace dl.wm.suite.auth.api.Controllers
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