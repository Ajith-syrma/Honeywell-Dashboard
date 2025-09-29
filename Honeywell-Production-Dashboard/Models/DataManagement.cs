
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
            List<SelectListItem> FgName = new List<SelectListItem>();
            try
            {
                using (SqlConnection sqlcon = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("pro_getFGName", sqlcon))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@cusid", cusid);
                        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                        DataTable dt = new DataTable();
                        sqlDataAdapter.Fill(dt);
                        if (dt != null)
                        {
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
            catch (Exception ex)
            {
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
                        cmdinsert.Parameters.AddWithValue("@shift_val", customermodel.shift_val);
                        cmdinsert.Parameters.AddWithValue("@fg_type", customermodel.Type);

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

        public List<ProductionDetails> getCustomerMasterModels()
        {
            var getOutTransaction = new List<ProductionDetails>();
            ProductionDetails objCustomerModel;
            try
            {
                using (SqlConnection sqlselect = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand sqlcmdselect = new SqlCommand("pro_selectTransactionOutput", sqlselect))
                    {
                        sqlcmdselect.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlcmdselect);
                        DataTable dtManpower = new DataTable();
                        sqlDataAdapter.Fill(dtManpower);
                        if (dtManpower != null && dtManpower.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtManpower.Rows)
                            {
                                objCustomerModel = new ProductionDetails();
                                objCustomerModel.TransactionId = Convert.ToInt32(dr["Id"].ToString());
                                objCustomerModel.station = dr["station"].ToString();
                                objCustomerModel.cycle_time = dr["cycle_time"].ToString();
                                objCustomerModel.Hourly_output = dr["Hourly_output"].ToString();
                                objCustomerModel.Op_count = Convert.ToInt32(dr["Op_count"].ToString());
                                objCustomerModel.Man_output = dr["Man_output"].ToString();
                                objCustomerModel.FgId = Convert.ToInt32(dr["Fg_Id"].ToString());
                                getOutTransaction.Add(objCustomerModel);
                            }
                        }

                    }
                }
                return getOutTransaction;

            }
            catch (Exception ex)
            {
                return getOutTransaction;
            }
        }

        public List<Dashboard_HourlyOP> getHourlyOP(Dashboard_HourlyOP dashboard_HourlyOP)
        {
            List<Dashboard_HourlyOP> lstDashboard = new List<Dashboard_HourlyOP>();
            Dashboard_HourlyOP objDashboard;
            try
            {
                var date = DateTime.Now.Date;
                string shift = GetShiftLabel(DateTime.Now);

                using (SqlConnection sqlhorly = new SqlConnection(Prod_ConnectionString))
                {
                    using (SqlCommand sqlcmdhourly = new SqlCommand("DIGI_DASHBOARD_HOURLY_OP_by_stage", sqlhorly))
                    {
                        sqlcmdhourly.CommandType = CommandType.StoredProcedure;
                       //sqlcmdhourly.Parameters.AddWithValue("@Type", dashboard_HourlyOP.TestType);
                        sqlcmdhourly.Parameters.AddWithValue("@Type", "V200");
                       // sqlcmdhourly.Parameters.AddWithValue("@fg",dashboard_HourlyOP.FGName);
                        sqlcmdhourly.Parameters.AddWithValue("@fg", "ECH3HWI00001");
                       // sqlcmdhourly.Parameters.AddWithValue("@date", date.ToString("dd-MM-yyyy"));
                        sqlcmdhourly.Parameters.AddWithValue("@date", "25-09-2025");
                       // sqlcmdhourly.Parameters.AddWithValue("@shift", shift);
                        sqlcmdhourly.Parameters.AddWithValue("@shift", "SHIFT-A");
                        SqlDataAdapter dahourly = new SqlDataAdapter(sqlcmdhourly);
                        DataTable dthourly = new DataTable();
                        dahourly.Fill(dthourly);
                        if (dthourly.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dthourly.Rows)
                            {
                                objDashboard = new Dashboard_HourlyOP();
                                objDashboard.Target = Convert.ToInt32(dr["Target"]);
                                objDashboard.HourIntervel = dr["HOURINTERVAL"].ToString();
                                objDashboard.LogCount = Convert.ToInt32(dr["LogCount"].ToString());
                                lstDashboard.Add(objDashboard);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                return lstDashboard;
            }

            return lstDashboard; 
        }


        public List<Dashboard_HourlyOP> getHourlyyield(Dashboard_HourlyOP dashboard_HourlyOP)
        {
            List<Dashboard_HourlyOP> lstyieldDashboard = new List<Dashboard_HourlyOP>();
            Dashboard_HourlyOP objDashboard1;
            try
            {
                var date = DateTime.Now.Date;
                string shift = GetShiftLabel(DateTime.Now);

                using (SqlConnection sqlhorly = new SqlConnection(Prod_ConnectionString))
                {
                    using (SqlCommand sqlcmdyield = new SqlCommand("DIGI_DASHBOARD_YIELDBYSTAGE", sqlhorly))
                    {
                        sqlcmdyield.CommandType = CommandType.StoredProcedure;
                       // sqlcmdyield.Parameters.AddWithValue("@Type", dashboard_HourlyOP.TestType);
                        sqlcmdyield.Parameters.AddWithValue("@Type", "V200");
                       // sqlcmdyield.Parameters.AddWithValue("@fg",dashboard_HourlyOP.FGName);
                        sqlcmdyield.Parameters.AddWithValue("@fg", "ECH3HWI00001");
                      //  sqlcmdyield.Parameters.AddWithValue("@date", date.ToString("dd-MM-yyyy"));
                        sqlcmdyield.Parameters.AddWithValue("@date", "25-09-2025");
                      //  sqlcmdyield.Parameters.AddWithValue("@shift", shift);
                        sqlcmdyield.Parameters.AddWithValue("@shift", "SHIFT-A");
                        SqlDataAdapter dyhourly = new SqlDataAdapter(sqlcmdyield);
                        DataTable dtyhourly = new DataTable();
                        dyhourly.Fill(dtyhourly);
                        if (dtyhourly.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtyhourly.Rows)
                            {
                                objDashboard1 = new Dashboard_HourlyOP();
                                objDashboard1.Stage = dr["STAGE"].ToString();
                                //objDashboard1.HourIntervel = dr["HOURINTERVAL"].ToString();
                                objDashboard1.Passcountyield = Convert.ToInt32(dr["PassCount"].ToString());
                                objDashboard1.Failcountyield = Convert.ToInt32(dr["FailCount"].ToString());
                                lstyieldDashboard.Add(objDashboard1);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                return lstyieldDashboard;
            }

            return lstyieldDashboard; 
        }

        public List<Lineutilization> getlineutildata(Lineutilization dashboard_lineutildata_OP)
        {
            List<Lineutilization> lstutildataDashboard = new List<Lineutilization>();
            Lineutilization objDashboard2;
            try
            {
                var date = DateTime.Now.Date;
                string shift = GetShiftLabel(DateTime.Now);

                using (SqlConnection sqlhorly = new SqlConnection(Prod_ConnectionString))
                {
                    using (SqlCommand sqlcmdutil = new SqlCommand("DIGI_DASHBOARD_LINEUTILIAZTIONBYSTAGE", sqlhorly))
                    {
                        sqlcmdutil.CommandType = CommandType.StoredProcedure;
                        //sqlcmdutil.Parameters.AddWithValue("@Type", dashboard_lineutildata_OP.TestType);
                        sqlcmdutil.Parameters.AddWithValue("@Type", "V200");
                       // sqlcmdutil.Parameters.AddWithValue("@fg",dashboard_lineutildata_OP.FGName);
                        sqlcmdutil.Parameters.AddWithValue("@fg", "ECH3HWI00001");
                       // sqlcmdutil.Parameters.AddWithValue("@date", date.ToString("dd-MM-yyyy"));
                        sqlcmdutil.Parameters.AddWithValue("@date", "25-09-2025");
                       // sqlcmdutil.Parameters.AddWithValue("@shift", shift);
                        sqlcmdutil.Parameters.AddWithValue("@shift", "SHIFT-A");
                        SqlDataAdapter dyhourly = new SqlDataAdapter(sqlcmdutil);
                        DataTable dtyhourly = new DataTable();
                        dyhourly.Fill(dtyhourly);
                        if (dtyhourly.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtyhourly.Rows)
                            {
                                objDashboard2 = new Lineutilization();
                               // objDashboard2.stage = dr["STAGE"].ToString();
                                objDashboard2.HourIntervel = dr["ACTUALHOUR"].ToString();
                                objDashboard2.planned_qty = Convert.ToInt32(dr["planned_qty"].ToString());
                                objDashboard2.Produced_qty = Convert.ToInt32(dr["Produced_qty"].ToString());
                                lstutildataDashboard.Add(objDashboard2);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                return lstutildataDashboard;
            }

            return lstutildataDashboard;
        }

        public List<labrlosspercentage> getlablosData(labrlosspercentage dashboard_lablossper_OP)
        {
            List<labrlosspercentage> lstlabrDashboard = new List<labrlosspercentage>();
            labrlosspercentage objDashboard3;
            try
            {
                var date = DateTime.Now.Date;
                string shift = GetShiftLabel(DateTime.Now);

                using (SqlConnection sqlhorly = new SqlConnection(Prod_ConnectionString))
                {
                    using (SqlCommand sqlcmdlabr = new SqlCommand("DIGI_DASHBOARD_LABLOSSPERCENTAGE", sqlhorly))
                    {
                        sqlcmdlabr.CommandType = CommandType.StoredProcedure;
                       // sqlcmdlabr.Parameters.AddWithValue("@Type", dashboard_lablossper_OP.TestType);
                        sqlcmdlabr.Parameters.AddWithValue("@Type", "V200");
                        //sqlcmdlabr.Parameters.AddWithValue("@fg",dashboard_lablossper_OP.FGName);
                        sqlcmdlabr.Parameters.AddWithValue("@fg", "ECH3HWI00001");
                        //sqlcmdlabr.Parameters.AddWithValue("@date", date.ToString("dd-MM-yyyy"));
                        string val = date.ToString("dd-MM-yyyy");
                        sqlcmdlabr.Parameters.AddWithValue("@date", "25-09-2025");
                        //sqlcmdlabr.Parameters.AddWithValue("@shift", shift);
                        sqlcmdlabr.Parameters.AddWithValue("@shift", "SHIFT-A");
                        SqlDataAdapter dyhourly = new SqlDataAdapter(sqlcmdlabr);
                        DataTable dtyhourly = new DataTable();
                        dyhourly.Fill(dtyhourly);
                        if (dtyhourly.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtyhourly.Rows)
                            {
                                objDashboard3 = new labrlosspercentage();
                                // objDashboard2.stage = dr["STAGE"].ToString();
                                objDashboard3.HourIntervel = dr["ACTUALHOUR"].ToString();
                                objDashboard3.Actual_work_hrs = Convert.ToInt32(dr["Actualworkedhours"].ToString());
                                objDashboard3.Produced_qty = Convert.ToInt32(dr["Produced_qty"].ToString());
                                lstlabrDashboard.Add(objDashboard3);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                return lstlabrDashboard;
            }

            return lstlabrDashboard;
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
                            logindetails.usertype = dt.Rows[0][2].ToString();
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

        public int getmanpowerdata(string fg,string type)
        {
            int result = 0;
            var date = DateTime.Now.Date;
            string shift = GetShiftLabel(DateTime.Now);

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("pro_getmanpower_ct", sqlConnection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;


                        cmd.Parameters.AddWithValue("@Type", "V200");
                        cmd.Parameters.AddWithValue("@fg", "ECH3HWI00001");
                        cmd.Parameters.AddWithValue("@date", "25-09-2025");
                        cmd.Parameters.AddWithValue("@shift", "SHIFT-A");



                        cmd.Parameters.AddWithValue("@Type", type);
                        cmd.Parameters.AddWithValue("@fg", fg);
                        cmd.Parameters.AddWithValue("@date", date.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@shift", shift);

                        sqlConnection.Open();
                        var response = cmd.ExecuteScalar();
                        sqlConnection.Close();

                        if (response != null && int.TryParse(response.ToString(), out int output))
                        {
                            result = output;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Optionally log exception
                return 0;
            }

            return result;
        }

        public int getdowntime(Dashboard_HourlyOP oeedowntime)
        {
            int result = 0;
            var date = DateTime.Now.Date;
            string shift = GetShiftLabel(DateTime.Now);

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Prod_ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("DIGI_DASHBOARD_AVAILABILTY", sqlConnection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@input", "idletime");


                        cmd.Parameters.AddWithValue("@type", "V200");
                        cmd.Parameters.AddWithValue("@fg", "ECH3HWI00001");
                        cmd.Parameters.AddWithValue("@date", "25-09-2025");
                        cmd.Parameters.AddWithValue("@shift", "SHIFT-A");
                        //cmd.Parameters.AddWithValue("@type", oeedowntime.TestType);
                        //cmd.Parameters.AddWithValue("@fg", oeedowntime.FGName);
                        //cmd.Parameters.AddWithValue("@date", date.ToString("dd-MM-yyyy"));
                        //cmd.Parameters.AddWithValue("@shift", shift);

                        sqlConnection.Open();
                        var response = cmd.ExecuteScalar();
                        sqlConnection.Close();

                        if (response != null && int.TryParse(response.ToString(), out int output))
                        {
                            result = output;

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Optionally log exception
                return 0;
            }

            return result;
        }


        public List<Dashboard_HourlyOP> getperf(Dashboard_HourlyOP dashboard_HourlyOP)
        {
            List<Dashboard_HourlyOP> lstPerformance = new List<Dashboard_HourlyOP>();

            try
            {
                var date = DateTime.Now.Date;
                string shift = GetShiftLabel(DateTime.Now);

                using (SqlConnection sqlConn = new SqlConnection(Prod_ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("DIGI_DASHBOARD_QUALITY", sqlConn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;



                        cmd.Parameters.AddWithValue("@type", "V200");
                        cmd.Parameters.AddWithValue("@fg", "ECH3HWI00001");
                        cmd.Parameters.AddWithValue("@date", "25-09-2025");
                        cmd.Parameters.AddWithValue("@shift", "SHIFT-A");


                        //cmd.Parameters.AddWithValue("@type", dashboard_HourlyOP.TestType);            
                        //cmd.Parameters.AddWithValue("@fg", dashboard_HourlyOP.FGName);
                        //cmd.Parameters.AddWithValue("@date", date.ToString("dd-MM-yyyy"));
                        //cmd.Parameters.AddWithValue("@shift", shift);

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                Dashboard_HourlyOP objDashboard = new Dashboard_HourlyOP
                                {
                                    Passcount = dr["PASSCOUNT"].ToString(),
                                    Failcount = dr["FAILCOUNT"].ToString(),
                                    Totalcount = Convert.ToInt32(dr["TOTALCOUNT"])
                                };

                                lstPerformance.Add(objDashboard);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return lstPerformance; // Return empty list on error
            }

            return lstPerformance;
        }


        public int getidealcycletime(Dashboard_HourlyOP oeeidealcycletime)
        {
            int result = 0;
            var date = DateTime.Now.Date;
            string shift = GetShiftLabel(DateTime.Now);

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("Pro_get_idealcytime", sqlConnection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        //cmd.Parameters.AddWithValue("@input", "oeedowntime");
                        //cmd.Parameters.AddWithValue("@type", oeedowntime.TestType);
                        //cmd.Parameters.AddWithValue("@fg", oeedowntime.FGName);
                        //cmd.Parameters.AddWithValue("@date", date);
                        //cmd.Parameters.AddWithValue("@shift", shift);

                        sqlConnection.Open();
                        var response = cmd.ExecuteScalar();
                        sqlConnection.Close();

                        if (response != null && int.TryParse(response.ToString(), out int output))
                        {
                            result = output;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Optionally log exception
                return 0;
            }

            return result;
        }

    }

}

