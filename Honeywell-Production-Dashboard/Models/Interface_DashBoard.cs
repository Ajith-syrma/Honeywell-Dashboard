using Microsoft.AspNetCore.Mvc.Rendering;

namespace Honeywell_Production_Dashboard.Models
{
    public interface Interface_DashBoard
    {
        List<SelectListItem> getCustomerName();
        List<SelectListItem> getFgName(int customer);
        int insertManpower(CustomerMasterModel customermodel);
        List<ProductionDetails> getCustomerMasterModels();

        List<Dashboard_HourlyOP> getHourlyOP(Dashboard_HourlyOP dashboard_HourlyOP);

        loginmodel logindetails(loginmodel loginmodel);

    }
}
