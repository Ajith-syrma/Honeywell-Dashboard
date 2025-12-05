using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace Honeywell_Production_Dashboard.Models
{
    public class H_Dashboard_hourly_Transaction
    {
        public int HourlyTransactionid { get; set; }
        public string Honeywell_hour { get; set; }
        public string Honeywell_shift { get; set; }
        public int Honeywell_plan { get; set; }
        public int Honeywell_Actual { get; set; }
        public string Createid { get; set; }
        public string Updateid { get; set; }
        public bool IsActive { get; set; }
    }
}
