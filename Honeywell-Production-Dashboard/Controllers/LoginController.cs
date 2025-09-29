using Honeywell_Production_Dashboard.Models;
using Microsoft.AspNetCore.Mvc;

namespace Honeywell_Production_Dashboard.Controllers
{
 
      
    public class LoginController : Controller
    {
        private readonly Interface_DashBoard _interface_DashBoard;
        public LoginController(Interface_DashBoard interface_DashBoard)
        {
            _interface_DashBoard = interface_DashBoard;
        }
        public IActionResult login()
        {
            ViewBag.login = "login";
            return View();
        }

        [HttpPost]
        public IActionResult login(loginmodel model)
        {
            if (ModelState.IsValid)
            {
                
                var login=_interface_DashBoard.logindetails(model);
                if (login != null)
                {
                    if (model.employeeid == login.employeeid && model.password == login.password)
                    {
                        if (login.usertype == "Admin")
                        {
                            return RedirectToAction("ProdcutionMaster", "DashBoardMaster");
                        }
                        else
                        {
                            return RedirectToAction("DhasBoardInput", "DashBoardMaster");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid employee ID or password.");
                        return View();
                    }

                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Login failed. User not found.");
                    return View();
                }
               
            }
            return View();
        }
    }
}
