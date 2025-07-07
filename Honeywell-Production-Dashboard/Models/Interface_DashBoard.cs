using Microsoft.AspNetCore.Mvc.Rendering;

namespace Honeywell_Production_Dashboard.Models
{
    public interface Interface_DashBoard
    {
        List<SelectListItem> getCustomerName();
        List<SelectListItem> getFgName(int customer);

    }
}
