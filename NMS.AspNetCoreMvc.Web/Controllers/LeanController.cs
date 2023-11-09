using Microsoft.AspNetCore.Mvc;

namespace NMS.AspNetCoreMvc.Web.Controllers
{
    public class LeanController : Controller
    {
        public IActionResult Memorize()
        {
            return View();
        }
    }
}
