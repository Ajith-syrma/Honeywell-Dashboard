using Honeywell_Production_Dashboard.Models;
using Microsoft.AspNetCore.Mvc;

namespace Honeywell_Production_Dashboard.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult login(loginmodel model)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("ProdcutionMaster", "DashBoardMaster");
            }
            return View();
        }
    }
}
