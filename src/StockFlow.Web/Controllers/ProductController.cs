using Microsoft.AspNetCore.Mvc;

namespace StockFlow.Web.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
