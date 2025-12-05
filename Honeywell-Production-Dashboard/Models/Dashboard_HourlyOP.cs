namespace Honeywell_Production_Dashboard.Models
{
    public class Dashboard_HourlyOP
    {

        public string Label { get; set; }   // e.g., "Availability", "Performance", "Quality"
        public decimal Value { get; set; }
        public string Passcount { get; set; }
        public string Failcount { get; set; }
        public int Totalcount { get; set; }
        public int Passcountyield { get; set; }
        public int Failcountyield { get; set; }

        public string Stage { get; set; }
        public decimal Yield { get; set; }

        public string TestType { get; set; } // Keep if still used elsewhere

        public int Target { get; set; } // Keep if still used elsewhere
        public string HourIntervel { get; set; }
        public int LogCount { get; set; }

        public string FGName { get; set; }

        public int hour { get; set; }

        public int hourvalue { get; set; }

    }


    public class Perforamce_value
    {
        public int Passcount { get; set; }
        public int Failcount { get; set; }
        public int Totalcount { get; set; }
    }

    public class Lineutilization 
    {
        public string FGName { get; set; }
        public string TestType { get; set; }
      //  public string stage { get; set; }
        public  int Produced_qty { get; set; }
        public int planned_qty { get; set; }
        public decimal line_util {  get; set; }
        public string HourIntervel { get; set; }

    }

    public class labrlosspercentage
    {
        public string FGName { get; set; }
        public string TestType { get; set; }
        //  public string stage { get; set; }
        public int Produced_qty { get; set; }
        public int Actual_work_hrs { get; set; }
        public decimal labr_loss { get; set; }
        public string HourIntervel { get; set; }

    }

    public class downtimecal
    {

        public int hour_count { get; set; }
        public int downtime { get; set; }


    }


}
