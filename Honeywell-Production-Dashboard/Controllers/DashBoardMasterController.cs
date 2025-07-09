using Honeywell_Production_Dashboard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Honeywell_Production_Dashboard.Controllers
{
    public class DashBoardMasterController : Controller
    {
        private readonly Interface_DashBoard interface_DashBoard;

        public DashBoardMasterController(Interface_DashBoard _interface_DashBoard)
        {
            interface_DashBoard= _interface_DashBoard;
        }
        public IActionResult ProdcutionMaster()
        {
            CustomerMasterModel customerMasterModel = new CustomerMasterModel();
            customerMasterModel.Customers = interface_DashBoard.getCustomerName();
            var getProdcutiondettails = interface_DashBoard.getCustomerMasterModels();
          //  var getdowntime = interface_DashBoard.getdowntime();    
            customerMasterModel.inputDetails = getProdcutiondettails;
            return View(customerMasterModel);
        }
        [HttpPost]
        public IActionResult ProdcutionMaster(CustomerMasterModel customermodel)
        {
            Dashboard_HourlyOP objDashboard=new Dashboard_HourlyOP ();
            var inputResult= interface_DashBoard.insertManpower(customermodel);
            customermodel.Customers = interface_DashBoard.getCustomerName();
            var getProdcutiondettails = interface_DashBoard.getCustomerMasterModels();
            customermodel.inputDetails = getProdcutiondettails;
            objDashboard.FGName= customermodel.FGName;
            objDashboard.TestType=customermodel.Type;
            var dashboardHourly = interface_DashBoard.getHourlyOP(objDashboard);
           // return View(customermodel);
           return RedirectToAction("DhasBoardInput", dashboardHourly);
        }
        public JsonResult getFGNameList(string cusid)
        {
            var lstFgName = interface_DashBoard.getFgName(Convert.ToInt32(cusid));
            return Json(lstFgName);
        }

        public IActionResult DhasBoardInput(List<Dashboard_HourlyOP> dashboardHourly)
        {
            return View();
        }
    }
}
