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
          return View(customerMasterModel);
        }

        public JsonResult getFGNameList(string cusid)
        {
            var lstFgName = interface_DashBoard.getFgName(Convert.ToInt32(cusid));
            return Json(lstFgName);
        }
    }
}
