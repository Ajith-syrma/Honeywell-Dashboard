
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Data.SqlClient;

namespace Honeywell_Production_Dashboard.Models
{
   
    public class DataManagement 
    {
        private readonly String ConnectionString;
        private readonly String Prod_ConnectionString;
        public DataManagement(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("conn");
            Prod_ConnectionString = configuration.GetConnectionString("connProdcurtion");
        }

        public List<SelectListItem> getcustomername()
        {
            List<SelectListItem> customername = new List<SelectListItem>();
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("pro_getCustomerName", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dataTable = new DataTable();
                        da.Fill(dataTable);
                        if (dataTable != null)
                        {
                            foreach (DataRow dr in dataTable.Rows)
                            {
                                customername.Add(new SelectListItem
                                {
                                    Text = dr["customer_name"].ToString(),
                                    Value = dr["customerid"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return customername;
            }
            return customername;
        }

        public List<SelectListItem> getFgName(int cusid)
        {
            List<SelectListItem> FgName= new List<SelectListItem>();
            try
            {
                using (SqlConnection sqlcon = new SqlConnection(ConnectionString)) {
                    using (SqlCommand sqlCommand = new SqlCommand("pro_getFGName", sqlcon)) {
                        sqlCommand.CommandType=CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@cusid", cusid);
                        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                        DataTable dt = new DataTable();
                        sqlDataAdapter.Fill(dt);
                        if (dt != null) { 
                            foreach (DataRow drnew in dt.Rows)
                            {
                                FgName.Add(new SelectListItem
                                {
                                    Text = drnew["Fg_Name"].ToString(),
                                    Value = drnew["id"].ToString()
                                });
                            }
                        }
                    }
                }

            }
            catch (Exception ex) {
                return FgName;
            }
            return FgName;
        }

        public int insertManpower(CustomerMasterModel customermodel)
        {
            int manpowerResult = 0;
            try
            {
                using (SqlConnection sqlinsert = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand cmdinsert = new SqlCommand("InsertTargetop", sqlinsert))
                    {
                        cmdinsert.CommandType = CommandType.StoredProcedure;
                        cmdinsert.Parameters.AddWithValue("@Op_count", customermodel.Manpower);
                        cmdinsert.Parameters.AddWithValue("@Fg_Id", customermodel.FGName);
                        sqlinsert.Open();
                        manpowerResult = cmdinsert.ExecuteNonQuery();
                        sqlinsert.Close();

                    }
                }
            }
            catch (Exception ex)
            {

                return manpowerResult;
            }
            return manpowerResult;
        }

        public List<ProductionDetails> getCustomerMasterModels() {
            var getOutTransaction=new List<ProductionDetails>();
            ProductionDetails objCustomerModel;
            try
            {
                using (SqlConnection sqlselect = new SqlConnection(ConnectionString)) {
                    using (SqlCommand sqlcmdselect = new SqlCommand("pro_selectTransactionOutput", sqlselect)) { 
                        sqlcmdselect.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter sqlDataAdapter=new SqlDataAdapter (sqlcmdselect);
                        DataTable dtManpower = new DataTable();
                        sqlDataAdapter.Fill (dtManpower);
                        if (dtManpower != null && dtManpower.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtManpower.Rows)
                            {
                                objCustomerModel =new ProductionDetails();
                                objCustomerModel.TransactionId =Convert.ToInt32(dr["Id"].ToString());
                                objCustomerModel.station = dr["station"].ToString();
                                objCustomerModel.cycle_time = dr["cycle_time"].ToString();
                                objCustomerModel.Hourly_output = dr["Hourly_output"].ToString();
                                objCustomerModel.Op_count =Convert.ToInt32(dr["Op_count"].ToString());
                                objCustomerModel.Man_output = dr["Man_output"].ToString();
                                objCustomerModel.FgId =Convert.ToInt32(dr["Fg_Id"].ToString());
                                getOutTransaction.Add(objCustomerModel);
                            }
                        }
                        
                    }
                }
                return getOutTransaction;

            }
            catch (Exception ex) {
                return getOutTransaction;
            }
        }

        public List<Dashboard_HourlyOP> getHourlyOP(Dashboard_HourlyOP dashboard_HourlyOP)
        {
            List<Dashboard_HourlyOP> lstDashboard = new List<Dashboard_HourlyOP>();
            Dashboard_HourlyOP objDashboard;
            try
            {
                var date=DateTime.Now.Date;
                string shift = GetShiftLabel(DateTime.Now);

                using (SqlConnection sqlhorly = new SqlConnection(Prod_ConnectionString)) { 
                    using(SqlCommand sqlcmdhourly=new SqlCommand("DIGI_DASHBOARD_HOURLY_OP_by_stage",sqlhorly))
                    {
                        sqlcmdhourly.CommandType = CommandType.StoredProcedure;
                        sqlcmdhourly.Parameters.AddWithValue("@Type", dashboard_HourlyOP.TestType);
                        // sqlcmdhourly.Parameters.AddWithValue("@fg",dashboard_HourlyOP.FGName);
                        sqlcmdhourly.Parameters.AddWithValue("@fg", "ECH1HML00002");
                        //sqlcmdhourly.Parameters.AddWithValue("@date", date.ToString("dd-MM-yyyy"));
                        sqlcmdhourly.Parameters.AddWithValue("@date", "29-11-2024");
                        // sqlcmdhourly.Parameters.AddWithValue("@shift", shift);
                        sqlcmdhourly.Parameters.AddWithValue("@shift", "SHIFT-B");
                        SqlDataAdapter dahourly=new SqlDataAdapter (sqlcmdhourly);
                        DataTable dthourly = new DataTable();
                        dahourly.Fill(dthourly);
                        if (dthourly.Rows.Count > 0) {
                            foreach (DataRow dr in dthourly.Rows)
                            {
                                objDashboard = new Dashboard_HourlyOP();
                                objDashboard.TestType = dr["TESTTYPE"].ToString();
                                objDashboard.HourIntervel = dr["HOURINTERVAL"].ToString();
                                objDashboard.LogCount =Convert.ToInt32(dr["LogCount"].ToString());
                                lstDashboard.Add(objDashboard);
                            }
                        }
                    }

                }
            }
            catch (Exception ex) {
                return lstDashboard;
            }

            return lstDashboard;
        }

        public loginmodel logindetails(loginmodel loginmodel)
        {
            var logindetails = new loginmodel();
            try
            {
                using (SqlConnection sqllogin = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand sqlcmdlogin = new SqlCommand("pro_login", sqllogin))
                    {
                        sqlcmdlogin.CommandType = CommandType.StoredProcedure;
                        sqlcmdlogin.Parameters.AddWithValue("@emloyeeid", loginmodel.employeeid);
                        sqlcmdlogin.Parameters.AddWithValue("@password", loginmodel.password);
                        SqlDataAdapter da = new SqlDataAdapter(sqlcmdlogin);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            logindetails.employeeid = dt.Rows[0][0].ToString();
                            logindetails.password = dt.Rows[0][1].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return logindetails;
            }

            return logindetails;
        }

        string GetShiftLabel(DateTime time)
        {
            TimeSpan t = time.TimeOfDay;

            if (t >= TimeSpan.FromHours(8) && t < TimeSpan.FromHours(16))
            {
                return "SHIFT-A"; // 8:00 AM - 3:59 PM
            }
            else if (t >= TimeSpan.FromHours(16) && t < TimeSpan.FromHours(24))
            {
                return "SHIFT-B"; // 4:00 PM - 11:59 PM
            }
            else
            {
                return "SHIFT-C"; // 12:00 AM - 7:59 AM
            }
        }

    }
}

