using Microsoft.AspNetCore.Mvc.Rendering;

namespace Honeywell_Production_Dashboard.Models
{
    
    public class DashBoardServices : Interface_DashBoard
    {
        private readonly DataManagement dataManagement;

        public DashBoardServices(DataManagement _datamanagement)
        {
            dataManagement = _datamanagement;
        }
        public List<SelectListItem> getCustomerName()
        {
            var cusname= dataManagement.getcustomername();
            return cusname;
        }

        public List<SelectListItem> getFgName(int customerid)
        {
            var fgName = dataManagement.getFgName(customerid);
            return fgName;
        }

        public List<ProductionDetails> getCustomerMasterModels()
        {
            var prodetails= dataManagement.getCustomerMasterModels();
            return prodetails;
        }

        public int insertManpower(CustomerMasterModel customermodel)
        {
            var result= dataManagement.insertManpower(customermodel);
            return result;
        }

        public List<Dashboard_HourlyOP> getHourlyOP(Dashboard_HourlyOP dashboard_HourlyOP)
        {
            var resulthourly=dataManagement.getHourlyOP(dashboard_HourlyOP);
            return resulthourly;
        }

        public loginmodel logindetails(loginmodel loginmodel)
        {
            var resultlogin= dataManagement.logindetails(loginmodel);
            return resultlogin;
        }
    }
}
