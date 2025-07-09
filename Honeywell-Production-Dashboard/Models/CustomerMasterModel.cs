using Microsoft.AspNetCore.Mvc.Rendering;

namespace Honeywell_Production_Dashboard.Models
{
    public class CustomerMasterModel
    {
        public string Customer { get; set; }
        public string FGName { get; set; }
        public int Manpower { get; set; }
        public string Type {  get; set; }
        public decimal downtime { get; set; }

        public List<SelectListItem> Customers { get; set; }
        public List<SelectListItem> FGNames { get; set; }

        public List<ProductionDetails> inputDetails { get; set; }
    }
}
