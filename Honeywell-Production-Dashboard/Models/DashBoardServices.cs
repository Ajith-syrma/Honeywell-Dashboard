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

    }
}
