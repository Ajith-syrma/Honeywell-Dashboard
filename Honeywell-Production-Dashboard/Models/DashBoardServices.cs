using Microsoft.AspNetCore.Mvc.Diagnostics;
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
            var resulthourly = dataManagement.getHourlyOP(dashboard_HourlyOP);
            return resulthourly;
        }

        public List<Dashboard_HourlyOP> getHourlyyield(Dashboard_HourlyOP dashboard_HourlyOP)
        {
            var resulthourly = dataManagement.getHourlyyield(dashboard_HourlyOP);

            foreach (var value in resulthourly)
            {
                var passcount = value.Passcountyield;
                var failcount = value.Failcountyield;
                var total_count = passcount + failcount;

                decimal yield = 0;
                if (total_count > 0)
                {
                    yield = ((decimal)(passcount) / total_count) * 100;
                }

                value.Yield = yield; //  Store the result directly in the object
            }

            return resulthourly;
        }


        public List<Lineutilization> getlineutildata(Lineutilization dashboard_lineutildata_OP)
        {
            var resultutildata = dataManagement.getlineutildata(dashboard_lineutildata_OP);

            foreach (var value in resultutildata)
            {
                var planned_qty = value.planned_qty;
                var Produced_qty = value.Produced_qty;
                //var lineutilization = (Produced_qty/planned_qty)*100;

                decimal lin_util = 0;
                if (planned_qty > 0 && Produced_qty >0)
                {
                    lin_util = ((decimal)(Produced_qty) / planned_qty) * 100;
                }

                value.line_util = lin_util; //  Store the result directly in the object
            }

            return resultutildata;
        }


        public List<labrlosspercentage> getlablosData(labrlosspercentage dashboard_lablossper_OP)
        {
            var resultlablosdata = dataManagement.getlablosData(dashboard_lablossper_OP);
            string fg = dashboard_lablossper_OP.FGName;
            string type = dashboard_lablossper_OP.TestType;
            var manpower = dataManagement.getmanpowerdata(fg, type);
            manpower = 2;

            foreach (var value in resultlablosdata)
            {

                var actualworkdhrs1 = ((decimal)value.Actual_work_hrs - 60);
                var actualworkdhrs = actualworkdhrs1 / 60;
                var Produced_qty = value.Produced_qty;
                var presenthrs = manpower* (actualworkdhrs) ;
                var gd_hrs = 1.347 * (Produced_qty);
                var val1 = (decimal)presenthrs - (decimal)gd_hrs;
                //var lineutilization = (Produced_qty/planned_qty)*100;

                decimal lab_los = 0;
                if (actualworkdhrs > 0 && Produced_qty > 0)
                {
                    lab_los = ((decimal)(val1) / (decimal)gd_hrs) * 100;
                    lab_los = lab_los+(decimal)(100);
                }

                value.labr_loss = lab_los; //  Store the result directly in the object
            }

            return resultlablosdata;
        }

        public loginmodel logindetails(loginmodel loginmodel)
        {
            var resultlogin= dataManagement.logindetails(loginmodel);
            return resultlogin;
        }

        public List<Dashboard_HourlyOP> getoee(Dashboard_HourlyOP dashboard_HourlyOP)
        {
            List<Dashboard_HourlyOP> oeeResults = new List<Dashboard_HourlyOP>();

            // 1. Availability
            int downtime_seconds = dataManagement.getdowntime(dashboard_HourlyOP);
            var date = DateTime.Now.TimeOfDay;

            int downtime = downtime_seconds - 2400;

            //int runtimeValue = 26400 - downtime_seconds;

            // Get current hour
            DateTime now = DateTime.Now;

            // 2. Format current time like "25-09-2025 08:12:23"
            string currentFormatted = now.ToString("dd-MM-yyyy HH:mm:ss");
            Console.WriteLine($"Current Time: {currentFormatted}");

            // 3. Determine shift start time
            DateTime shiftStart;

            TimeSpan currentTime = now.TimeOfDay;

            if (currentTime >= TimeSpan.FromHours(8) && currentTime < TimeSpan.FromHours(16))
            {
                // Shift A
                shiftStart = new DateTime(now.Year, now.Month, now.Day, 8, 0, 0);
            }
            else if (currentTime >= TimeSpan.FromHours(16) && currentTime < TimeSpan.FromHours(24))
            {
                // Shift B
                shiftStart = new DateTime(now.Year, now.Month, now.Day, 16, 0, 0);
            }
            else
            {
                // Shift C (between 00:00 and 08:00)
                // If before 08:00, shift started today at 00:00
                shiftStart = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
            }

            // 4. Format shift start time
            string shiftFormatted = shiftStart.ToString("dd-MM-yyyy HH:mm:ss");
            Console.WriteLine($"Shift Start Time: {shiftFormatted}");

            // 5. Calculate the difference
            TimeSpan difference = now - shiftStart;
            Console.WriteLine($"Time since shift started: {difference.Hours} hours, {difference.Minutes} minutes, {difference.Seconds} seconds");


            int minutesSinceShiftStart = (int)difference.TotalMinutes;
            // Get planned production time for current hour

            // int planned_production_time = minutesSinceShiftStart*60;
            int planned_production_time = 26400 ;

            // Calculate runtime
            int runtimeValue = 26400 - downtime_seconds;

           // Output


            decimal availability = (runtimeValue > 0)
                ? ((decimal)runtimeValue / planned_production_time) * 100
                : 0;

            // 2. Get Performance Metrics
            var performanceList = dataManagement.getperf(dashboard_HourlyOP);

            int passCount = 0, failCount = 0, totalCount = 0;

            if (performanceList != null && performanceList.Count > 0)
            {
                var item = performanceList.First();
                int.TryParse(item.Passcount, out passCount);
                int.TryParse(item.Failcount, out failCount);
                totalCount = item.Totalcount;
            }

            // 3. Quality
            decimal quality = totalCount > 0
                ? ((decimal)passCount / totalCount) * 100
                : 0;

            // 4. Performance
            // decimal idealCycleTime = dataManagement.getidealcycletime(dashboard_HourlyOP); // in seconds
            decimal idealCycleTime = (decimal)200.00;
            decimal performance = (totalCount > 0 && runtimeValue > 0)
                ? ((decimal)totalCount * idealCycleTime / runtimeValue) * 100
                : 0;


            // 5. Add to result list
            oeeResults.Add(new Dashboard_HourlyOP { Label = "Availability", Value = Math.Round(availability, 2) });
            oeeResults.Add(new Dashboard_HourlyOP { Label = "Performance", Value = Math.Round(performance, 2) });
            oeeResults.Add(new Dashboard_HourlyOP { Label = "Quality", Value = Math.Round(quality, 2) });

            return oeeResults;
        }





    }
}
