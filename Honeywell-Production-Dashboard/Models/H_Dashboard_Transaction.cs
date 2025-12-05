namespace Honeywell_Production_Dashboard.Models
{
    public class H_Dashboard_Transaction
    {
        public int TransactionID { get; set; }
        public string CustomerName { get; set; }
        public string FGName { get; set; }
        public decimal OEE_Availability { get; set; }
        public decimal OEE_Performance { get; set; }
        public decimal OEE_Quality { get; set; }
        public decimal OEE { get; set; }
        public decimal Labourloss { get; set; }
        public decimal LineUtililization { get; set; }
        public string Honeywell_shift { get; set; }
        public string Createid { get; set; }
        public string Updateid { get; set; }
        public bool IsActive { get; set; }
    }
}
