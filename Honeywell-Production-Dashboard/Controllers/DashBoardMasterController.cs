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
            customerMasterModel.inputDetails = getProdcutiondettails;
            return View(customerMasterModel);
        }
        [HttpPost]
        public IActionResult ProdcutionMaster(CustomerMasterModel customermodel)
        {
            var inputResult= interface_DashBoard.insertManpower(customermodel);
            customermodel.Customers = interface_DashBoard.getCustomerName();

            var getProdcutiondettails = interface_DashBoard.getCustomerMasterModels();
            // return View(customermodel);
            return RedirectToAction("DhasBoardInput", customermodel);
        }
        public JsonResult getFGNameList(string cusid)
        {
            var lstFgName = interface_DashBoard.getFgName(Convert.ToInt32(cusid));
            return Json(lstFgName);
        }

        public IActionResult DhasBoardInput(CustomerMasterModel dashboardHourly)
        {
            CustomerMasterModel obj=new CustomerMasterModel ();
            obj.FGName = dashboardHourly.FGNameText;
            obj.Type = dashboardHourly.Type;
            return View(obj);
        }

        [HttpGet]
        public JsonResult GetChartData(string Fgname,string type)
        {
            // var data = new List<Dashboard_HourlyOP>
            Dashboard_HourlyOP objDashboard = new Dashboard_HourlyOP();
            objDashboard.FGName = Fgname.ToString();
            objDashboard.TestType = type.ToString(); 
            var dashboardHourly = interface_DashBoard.getHourlyOP(objDashboard);
            return Json(dashboardHourly);
        }
    }
}
