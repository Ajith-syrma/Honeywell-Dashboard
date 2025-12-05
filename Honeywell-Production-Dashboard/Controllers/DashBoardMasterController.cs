using Honeywell_Production_Dashboard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace Honeywell_Production_Dashboard.Controllers
{
    public class DashBoardMasterController : Controller
    {
        private readonly Interface_DashBoard interface_DashBoard;
        private readonly DataManagement _OEEData;


        public DashBoardMasterController(Interface_DashBoard _interface_DashBoard, DataManagement OEEData)
        {
            interface_DashBoard = _interface_DashBoard;
            _OEEData = OEEData;
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
            //var inputResult = interface_DashBoard.insertManpower(customermodel); 
            customermodel.Customers = interface_DashBoard.getCustomerName();

            var getProdcutiondettails = interface_DashBoard.getCustomerMasterModels();
            // return View(customermodel);
            return RedirectToAction("DhasBoardInput", customermodel);
        }

        public IActionResult Angenic()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Angenic(CustomerMasterModel customermodel)
        {
           // var inputResult = interface_DashBoard.insertManpower(customermodel); //Testing
            customermodel.Customers = interface_DashBoard.getCustomerName();

            var getProdcutiondettails = interface_DashBoard.getCustomerMasterModels();
            
            //return RedirectToAction("DhasBoardInputs", customermodel);
            return RedirectToAction("DhasBoardInput", customermodel);
            // return View(customermodel);
        }

        public JsonResult GetyieldDataOne(string Fgname, string type)
        {
            Dashboard_HourlyOP objDashboard3 = new Dashboard_HourlyOP();
            objDashboard3.FGName = Fgname.ToString();
            //objDashboard3.TestType = type.ToString();
            objDashboard3.TestType = string.Empty;
            var dashboardlablosper = interface_DashBoard.getyieldDataOne(objDashboard3);
            return Json(dashboardlablosper);
        }

        public JsonResult GetHourlyOne(string Fgname, string type)
        {
            Dashboard_HourlyOP objDashboard3 = new Dashboard_HourlyOP();
            objDashboard3.FGName = Fgname.ToString();
            //objDashboard3.TestType = type.ToString();
            objDashboard3.TestType = string.Empty;
            var dashboardlablosper = interface_DashBoard.gethourlyone(objDashboard3);
            return Json(dashboardlablosper);
        }

        public JsonResult GetyieldDatatwo(string Fgname, string type)
        {
            Dashboard_HourlyOP objDashboard3 = new Dashboard_HourlyOP();
            objDashboard3.FGName = Fgname.ToString();
            //objDashboard3.TestType = type.ToString();
            objDashboard3.TestType = string.Empty;
            var dashboardlablosper = interface_DashBoard.getyieldDatatwo(objDashboard3);
            return Json(dashboardlablosper);
        }

        public JsonResult GetHourlytwo(string Fgname, string type)
        {
            Dashboard_HourlyOP objDashboard3 = new Dashboard_HourlyOP();
            objDashboard3.FGName = Fgname.ToString();
            //objDashboard3.TestType = type.ToString();
            objDashboard3.TestType = string.Empty;
            var dashboardlablosper = interface_DashBoard.gethourlytwo(objDashboard3);
            return Json(dashboardlablosper);
        }


        public JsonResult GetyieldDatathree(string Fgname, string type)
        {
            Dashboard_HourlyOP objDashboard3 = new Dashboard_HourlyOP();
            objDashboard3.FGName = Fgname.ToString();
            //objDashboard3.TestType = type.ToString();
            objDashboard3.TestType = string.Empty;
            var dashboardlablosper = interface_DashBoard.getyieldDatathree(objDashboard3);
            return Json(dashboardlablosper);
        }

        public JsonResult GetHourlythree(string Fgname, string type)
        {
            Dashboard_HourlyOP objDashboard3 = new Dashboard_HourlyOP();
            objDashboard3.FGName = Fgname.ToString();
            //objDashboard3.TestType = type.ToString();
            objDashboard3.TestType = string.Empty;
            var dashboardlablosper = interface_DashBoard.gethourlythree(objDashboard3);
            return Json(dashboardlablosper);
        }

        public JsonResult GetyieldDatafour(string Fgname, string type)
        {
            Dashboard_HourlyOP objDashboard3 = new Dashboard_HourlyOP();
            objDashboard3.FGName = Fgname.ToString();
            //objDashboard3.TestType = type.ToString();
            objDashboard3.TestType = string.Empty;
            var dashboardlablosper = interface_DashBoard.getyieldDatafour(objDashboard3);
            return Json(dashboardlablosper);
        }

        public JsonResult GetHourlyfour(string Fgname, string type)
        {
            Dashboard_HourlyOP objDashboard3 = new Dashboard_HourlyOP();
            objDashboard3.FGName = Fgname.ToString();
            //objDashboard3.TestType = type.ToString();
            objDashboard3.TestType = string.Empty;
            var dashboardlablosper = interface_DashBoard.gethourlyfour(objDashboard3);
            return Json(dashboardlablosper);
        }

        public JsonResult GetyieldDatafive(string Fgname, string type)
        {
            Dashboard_HourlyOP objDashboard3 = new Dashboard_HourlyOP();
            objDashboard3.FGName = Fgname.ToString();
            //objDashboard3.TestType = type.ToString();
            objDashboard3.TestType = string.Empty;
            var dashboardlablosper = interface_DashBoard.getyieldDatafive(objDashboard3);
            return Json(dashboardlablosper);
        }

        public JsonResult GetHourlyfive(string Fgname, string type)
        {
            Dashboard_HourlyOP objDashboard3 = new Dashboard_HourlyOP();
            objDashboard3.FGName = Fgname.ToString();
            //objDashboard3.TestType = type.ToString();
            objDashboard3.TestType = string.Empty;
            var dashboardlablosper = interface_DashBoard.gethourlyfive(objDashboard3);
            return Json(dashboardlablosper);
        }
        public JsonResult getFGNameList(string cusid)
        {
            var lstFgName = interface_DashBoard.getFgName(Convert.ToInt32(cusid));
            return Json(lstFgName);
        }

        public IActionResult DhasBoardInput(CustomerMasterModel dashboardHourly)
        {
            CustomerMasterModel obj = new CustomerMasterModel();
            var objHtrans = new H_Dashboard_Transaction();
            var objDashboard1 = new Dashboard_HourlyOP();
            var objDashboard2 = new Lineutilization();
            var objDashboard3 = new labrlosspercentage();
            var objoeeDashboard = new Dashboard_HourlyOP();
            var objYield = new H_Dashboard_yield_Transaction();
            var objDashboardop = new H_Dashboard_hourly_Transaction();

            obj.FGName = dashboardHourly.FGNameText;
            obj.Type = dashboardHourly.Type;
            obj.Customer=dashboardHourly.Customer;

            objDashboard1.FGName = dashboardHourly.FGNameText;
            objDashboard1.TestType = dashboardHourly.Type;
            objDashboard2.FGName = dashboardHourly.FGNameText;
            objDashboard2.TestType = dashboardHourly.Type;
            objDashboard3.FGName = dashboardHourly.FGNameText;
            objDashboard3.TestType = dashboardHourly.Type;
            objoeeDashboard.FGName = dashboardHourly.FGNameText;
            objoeeDashboard.TestType = dashboardHourly.Type;


            var dashboardyield = interface_DashBoard.getHourlyyield(objDashboard1);
            var dashboardlineutil = interface_DashBoard.getlineutildata(objDashboard2);
            var dashboardlablosper = interface_DashBoard.getlablosData(objDashboard3);

            var OEE = interface_DashBoard.getoee(objoeeDashboard);
            if (OEE.Count > 0)
            {
                objHtrans.CustomerName = dashboardHourly.Customer;
                objHtrans.FGName = dashboardHourly.FGNameText;
                objHtrans.OEE_Performance = OEE.Where(a => a.Label == "Performance").Select(a => a.Value).FirstOrDefault();
                objHtrans.OEE_Quality = OEE.Where(a => a.Label == "Quality").Select(a => a.Value).FirstOrDefault();
                objHtrans.OEE_Availability = OEE.Where(a => a.Label == "Availability").Select(a => a.Value).FirstOrDefault();
                objHtrans.OEE = objHtrans.OEE_Performance * objHtrans.OEE_Quality * objHtrans.OEE_Availability;
                objHtrans.Labourloss = dashboardlablosper.Select(a => a.labr_loss).FirstOrDefault();
                objHtrans.LineUtililization = dashboardlineutil.Select(a => a.line_util).FirstOrDefault();
                objHtrans.Honeywell_shift = dashboardHourly.shift_val;
                objHtrans.Createid = "70192";
                objHtrans.Updateid = "70192";
                var resultHourlyDashBoard = interface_DashBoard.insertHoneywellTransaction(objHtrans);
                
            }
           
            if (dashboardyield.Count > 0)
            {
                objYield.FCT_1 = dashboardyield.Where(a => a.Stage == "FCT-1").Select(a => a.Yield).FirstOrDefault();
                objYield.FCT_2 = dashboardyield.Where(a => a.Stage == "FCT-2").Select(a => a.Yield).FirstOrDefault();
                objYield.FCT_3 = dashboardyield.Where(a => a.Stage == "FCT-3").Select(a => a.Yield).FirstOrDefault();
                objYield.LCD_1 = dashboardyield.Where(a => a.Stage == "LCD-1").Select(a => a.Yield).FirstOrDefault();
                objYield.LCD_2 = dashboardyield.Where(a => a.Stage == "LCD-2").Select(a => a.Yield).FirstOrDefault();
                objYield.RF_1 = dashboardyield.Where(a => a.Stage == "RF-1").Select(a => a.Yield).FirstOrDefault();
                objYield.RF_2 = dashboardyield.Where(a => a.Stage == "RF-2").Select(a => a.Yield).FirstOrDefault();
                objYield.RTC = dashboardyield.Where(a => a.Stage == "RTC").Select(a => a.Yield).FirstOrDefault();
                objYield.VOLT = dashboardyield.Where(a => a.Stage == "VOLT").Select(a => a.Yield).FirstOrDefault();
                objYield.Honeywell_shift = dashboardHourly.shift_val;
                objYield.Createid = "70192";
                objYield.Updateid = "70192";
                var resultYield = interface_DashBoard.insertHoneywelldashboard_yield_Transaction(objYield);
            }
            var dashboardHourlyop = interface_DashBoard.getHourlyOP(objoeeDashboard);
            if (dashboardHourlyop.Count > 0)
            {
                objDashboardop.Honeywell_hour = dashboardHourlyop.Select(a => a.hour).FirstOrDefault().ToString();
                objDashboardop.Honeywell_plan = dashboardHourlyop.Select(a => a.Target).FirstOrDefault();
                objDashboardop.Honeywell_Actual = dashboardHourlyop.Select(a => a.LogCount).FirstOrDefault();
                objDashboardop.Honeywell_shift = dashboardHourly.shift_val;
                objDashboardop.Createid = "70192";
                objDashboardop.Updateid = "70192";
                var resulthourly = interface_DashBoard.insertHoneywelldashboard_HourlyTransaction(objDashboardop);
            }
            return View(obj);
        }

        public IActionResult DhasBoardInputs(CustomerMasterModel dashboardHourly)
        {
            CustomerMasterModel obj = new CustomerMasterModel();
            obj.FGName = dashboardHourly.FGNameText;
            obj.Type = dashboardHourly.Type;
            return View(obj);
        }

        [HttpGet]
        public JsonResult GetChartData(string Fgname, string type)
        {
            // var data = new List<Dashboard_HourlyOP>
            Dashboard_HourlyOP objDashboard = new Dashboard_HourlyOP();
            objDashboard.FGName = Fgname.ToString();
            // objDashboard.TestType = type.ToString();
            objDashboard.TestType = string.Empty;
            var dashboardHourly = interface_DashBoard.getHourlyOP(objDashboard);
            return Json(dashboardHourly);
        }

        [HttpGet]
        public JsonResult GetyieldData(string Fgname, string type)
        {
            // var data = new List<Dashboard_HourlyOP>
            Dashboard_HourlyOP objDashboard1 = new Dashboard_HourlyOP();
            objDashboard1.FGName = Fgname.ToString();
            //objDashboard1.TestType = type.ToString();
            objDashboard1.TestType = string.Empty;
            var dashboardyield = interface_DashBoard.getHourlyyield(objDashboard1);
            return Json(dashboardyield);
        }

        [HttpGet]
        public JsonResult GetlineutilData(string Fgname, string type)
        {
            // var data = new List<Dashboard_HourlyOP>
            Lineutilization objDashboard2 = new Lineutilization();
            objDashboard2.FGName = Fgname.ToString();
            //objDashboard2.TestType = type.ToString();
            objDashboard2.TestType = string.Empty;
            var dashboardlineutil = interface_DashBoard.getlineutildata(objDashboard2);
            return Json(dashboardlineutil);
        }


        [HttpGet]
        public JsonResult GetlablosData(string Fgname, string type)
        {
            // var data = new List<Dashboard_HourlyOP>
            labrlosspercentage objDashboard3 = new labrlosspercentage();
            objDashboard3.FGName = Fgname.ToString();
            //objDashboard3.TestType = type.ToString();
            objDashboard3.TestType = string.Empty;
            var dashboardlablosper = interface_DashBoard.getlablosData(objDashboard3);
            return Json(dashboardlablosper);
        }

        [HttpGet]
        public JsonResult GetChartDataoee(string Fgname, string type)
        {
            Dashboard_HourlyOP objoeeDashboard = new Dashboard_HourlyOP
            {
                FGName = Fgname,
                TestType = type
            };

            // Call updated method that returns List<Dashboard_HourlyOP>
            List<Dashboard_HourlyOP> OEE = interface_DashBoard.getoee(objoeeDashboard);


            return Json(OEE); // Return as JSON to frontend
        }

    }
}
