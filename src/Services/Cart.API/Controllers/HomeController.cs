using Microsoft.AspNetCore.Mvc;

namespace Cart.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Home")]
    public class HomeController : Controller
    {
        //GET /<controller>/
        public IActionResult Index()
        {
            return new RedirectResult("~/swagger/ui");
        }
    }
}