using Microsoft.AspNetCore.Mvc;

namespace aegis.wm.suite.dms.api.Controllers
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