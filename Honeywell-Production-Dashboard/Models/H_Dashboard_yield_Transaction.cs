using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace Honeywell_Production_Dashboard.Models
{
    public class H_Dashboard_yield_Transaction
    {
        public int YieldTransactionID { get; set; }
        public decimal FCT_1 { get; set; }
        public decimal FCT_2 { get; set; }
        public decimal FCT_3 { get; set; }
        public decimal LCD_1 { get; set; }
         public decimal LCD_2 { get; set; }
        public decimal RF_1 { get; set; }
        public decimal RF_2 { get; set; }
        public decimal RTC { get; set; }
        public decimal VOLT { get; set; }
        public string Honeywell_shift { get; set; }
        public string Createid { get; set; }
        public string Updateid { get; set; }
        public bool IsActive { get; set; }
    }
}
