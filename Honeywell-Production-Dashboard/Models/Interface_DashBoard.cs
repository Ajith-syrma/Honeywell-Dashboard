using Microsoft.AspNetCore.Mvc.Rendering;

namespace Honeywell_Production_Dashboard.Models
{
    public interface Interface_DashBoard
    {
        List<SelectListItem> getCustomerName();
        List<SelectListItem> getFgName(int customer);
        int insertManpower(CustomerMasterModel customermodel);
        List<ProductionDetails> getCustomerMasterModels();
        List<Dashboard_HourlyOP> getoee(Dashboard_HourlyOP dashboard_HourlyOP);

        List<Dashboard_HourlyOP> getHourlyOP(Dashboard_HourlyOP dashboard_HourlyOP);
        List<Dashboard_HourlyOP> getHourlyyield(Dashboard_HourlyOP dashboard_yield_OP);
        List<Lineutilization> getlineutildata(Lineutilization dashboard_lineutildata_OP);
        List<labrlosspercentage> getlablosData(labrlosspercentage dashboard_lablossper_OP);


        List<Dashboard_HourlyOP> gethourlyone(Dashboard_HourlyOP dashboard_HourlyOP);
        List<Dashboard_HourlyOP> gethourlytwo(Dashboard_HourlyOP dashboard_HourlyOP);
        List<Dashboard_HourlyOP> gethourlythree(Dashboard_HourlyOP dashboard_HourlyOP);
        List<Dashboard_HourlyOP> gethourlyfour(Dashboard_HourlyOP dashboard_HourlyOP);
        List<Dashboard_HourlyOP> gethourlyfive(Dashboard_HourlyOP dashboard_HourlyOP);

        List<Dashboard_HourlyOP> getyieldDataOne(Dashboard_HourlyOP dashboard_yield);
        List<Dashboard_HourlyOP> getyieldDatatwo(Dashboard_HourlyOP dashboard_yield);
        List<Dashboard_HourlyOP> getyieldDatathree(Dashboard_HourlyOP dashboard_yield);
        List<Dashboard_HourlyOP> getyieldDatafour(Dashboard_HourlyOP dashboard_yield);
        List<Dashboard_HourlyOP> getyieldDatafive(Dashboard_HourlyOP dashboard_yield);
        loginmodel logindetails(loginmodel loginmodel);
        int insertHoneywellTransaction(H_Dashboard_Transaction transaction);
        int insertHoneywelldashboard_yield_Transaction(H_Dashboard_yield_Transaction h_Dashboard_Yield);
       int insertHoneywelldashboard_HourlyTransaction(H_Dashboard_hourly_Transaction hourlyData);
    }
}
