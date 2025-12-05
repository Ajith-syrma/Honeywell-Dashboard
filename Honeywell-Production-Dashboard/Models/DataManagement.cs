
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using System.Data;
using System.Data.SqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Honeywell_Production_Dashboard.Models
{

    public class DataManagement
    {
        private readonly string ConnectionString;
        private readonly string Prod_ConnectionString;
        private readonly string A4;
        public DataManagement(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("conn");
            Prod_ConnectionString = configuration.GetConnectionString("connProdcurtion");
            A4 = configuration.GetConnectionString("connProdcurtionA4");
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
                string proceture = string.Empty;
                string connection = string.Empty;
                if (dashboard_HourlyOP.FGName == "ECH3HWI00001" || dashboard_HourlyOP.FGName == "ECH1HWI00002")
                {
                    connection = Prod_ConnectionString;
                    proceture = "DIGI_DASHBOARD_HOURLY_OP_by_stage";
                }
                else
                {
                    connection = A4;
                    proceture = "DIGI_DASHBOARD_HOURLY_OP_by_stage_H2";
                }

                using (SqlConnection sqlhorly = new SqlConnection(connection))
                {
                   

                    using (SqlCommand sqlcmdhourly = new SqlCommand(proceture, sqlhorly))
                    {
                        sqlcmdhourly.CommandType = CommandType.StoredProcedure;
                        sqlcmdhourly.Parameters.AddWithValue("@Type", dashboard_HourlyOP.TestType);
                        sqlcmdhourly.Parameters.AddWithValue("@fg", dashboard_HourlyOP.FGName);
                        sqlcmdhourly.Parameters.AddWithValue("@date", date.ToString("dd-MM-yyyy"));
                        sqlcmdhourly.Parameters.AddWithValue("@shift", shift);

                        //sqlcmdhourly.Parameters.AddWithValue("@Type", "V200");                       
                        //sqlcmdhourly.Parameters.AddWithValue("@fg", "ECH3HWI00001");                       
                        //sqlcmdhourly.Parameters.AddWithValue("@date", "25-09-2025");                      
                        //sqlcmdhourly.Parameters.AddWithValue("@shift", "SHIFT-A");

                        SqlDataAdapter dahourly = new SqlDataAdapter(sqlcmdhourly);
                        DataTable dthourly = new DataTable();
                        dahourly.Fill(dthourly);
                        if (dthourly.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dthourly.Rows)
                            {
                                objDashboard = new Dashboard_HourlyOP();
                                objDashboard.Target = Convert.ToInt32(dr["Target"]); //plan
                                objDashboard.HourIntervel = dr["HOURINTERVAL"].ToString(); //hour
                                objDashboard.LogCount = Convert.ToInt32(dr["LogCount"].ToString()); //actual
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
                string proc = string.Empty;
                string shift = GetShiftLabel(DateTime.Now);
                string conn = string.Empty;
                // A4
                if (dashboard_HourlyOP.FGName == "ECH3HWI00001" || dashboard_HourlyOP.FGName == "ECH1HWI00002")
                {
                    proc = "DIGI_DASHBOARD_YIELDBYSTAGE";
                    conn=Prod_ConnectionString;
                }
                else
                {
                    proc = "DIGI_DASHBOARD_YIELDBYSTAGE_H2";
                    conn = A4;
                }
                using (SqlConnection sqlhorly = new SqlConnection(conn))
                {
                    

                    using (SqlCommand sqlcmdyield = new SqlCommand(proc, sqlhorly))
                        {
                            sqlcmdyield.CommandType = CommandType.StoredProcedure;
                            sqlcmdyield.Parameters.AddWithValue("@Type", dashboard_HourlyOP.TestType);
                            sqlcmdyield.Parameters.AddWithValue("@fg", dashboard_HourlyOP.FGName);
                            sqlcmdyield.Parameters.AddWithValue("@date", date.ToString("dd-MM-yyyy"));
                            sqlcmdyield.Parameters.AddWithValue("@shift", shift);

                            //sqlcmdyield.Parameters.AddWithValue("@Type", "V200");                      
                            //sqlcmdyield.Parameters.AddWithValue("@fg", "ECH3HWI00001");                     
                            //sqlcmdyield.Parameters.AddWithValue("@date", "25-09-2025");                      
                            //sqlcmdyield.Parameters.AddWithValue("@shift", "SHIFT-A");

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

                            sqlcmdutil.Parameters.AddWithValue("@Type", dashboard_lineutildata_OP.TestType);
                            sqlcmdutil.Parameters.AddWithValue("@fg", dashboard_lineutildata_OP.FGName);
                            sqlcmdutil.Parameters.AddWithValue("@date", date.ToString("dd-MM-yyyy"));
                            sqlcmdutil.Parameters.AddWithValue("@shift", shift);

                            //sqlcmdutil.Parameters.AddWithValue("@Type", "V200");                       
                            //sqlcmdutil.Parameters.AddWithValue("@fg", "ECH3HWI00001");                       
                            //sqlcmdutil.Parameters.AddWithValue("@date", "25-09-2025");
                            //sqlcmdutil.Parameters.AddWithValue("@shift", "SHIFT-A");

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
                        sqlcmdlabr.Parameters.AddWithValue("@Type", dashboard_lablossper_OP.TestType);
                        sqlcmdlabr.Parameters.AddWithValue("@fg", dashboard_lablossper_OP.FGName);
                        sqlcmdlabr.Parameters.AddWithValue("@date", date.ToString("dd-MM-yyyy"));
                        sqlcmdlabr.Parameters.AddWithValue("@shift", shift);

                        //sqlcmdlabr.Parameters.AddWithValue("@Type", "V200");                        
                        //sqlcmdlabr.Parameters.AddWithValue("@fg", "ECH3HWI00001");                        
                        //string val = date.ToString("dd-MM-yyyy");
                        //sqlcmdlabr.Parameters.AddWithValue("@date", "25-09-2025");                       
                        //sqlcmdlabr.Parameters.AddWithValue("@shift", "SHIFT-A");

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


                        //cmd.Parameters.AddWithValue("@Type", "V200");
                        //cmd.Parameters.AddWithValue("@fg", "ECH3HWI00001");
                        //cmd.Parameters.AddWithValue("@date", "25-09-2025");
                        //cmd.Parameters.AddWithValue("@shift", "SHIFT-A");



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

        public int insertHoneywellTransaction(H_Dashboard_Transaction data)
        {
            int transResult = 0;
            try
            {
                using (SqlConnection sqltranscon = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand sqlcmdtrans = new SqlCommand("pro_InsertHoneywellDashboard_Transaction", sqltranscon))
                    {
                        sqlcmdtrans.CommandType = CommandType.StoredProcedure;
                        sqlcmdtrans.Parameters.AddWithValue("@CustomerName", data.CustomerName.ToString());
                        sqlcmdtrans.Parameters.AddWithValue("@FGName", data.FGName.ToString());
                        sqlcmdtrans.Parameters.AddWithValue("@OEE_Availability", data.OEE_Availability);
                        sqlcmdtrans.Parameters.AddWithValue("@OEE_Performance", data.OEE_Performance);
                        sqlcmdtrans.Parameters.AddWithValue("@OEE_Quality", data.OEE_Quality);
                        sqlcmdtrans.Parameters.AddWithValue("@OEE", data.OEE);
                        sqlcmdtrans.Parameters.AddWithValue("@Labourloss", data.Labourloss);
                        sqlcmdtrans.Parameters.AddWithValue("@LineUtililization", data.LineUtililization);
                        sqlcmdtrans.Parameters.AddWithValue("@Honeywell_shift", data.Honeywell_shift.ToString());
                        sqlcmdtrans.Parameters.AddWithValue("@Createid",string.IsNullOrEmpty(data.Createid.ToString()) ? "70192" : data.Createid.ToString());
                        sqlcmdtrans.Parameters.AddWithValue("@Updateid",string.IsNullOrEmpty(data.Updateid.ToString())? "70192" : data.Updateid.ToString());
                        sqltranscon.Open();
                        transResult = sqlcmdtrans.ExecuteNonQuery();
                        sqltranscon.Close();
                        return transResult;

                    }
                }
            }
            catch (Exception ex)
            {
                writeErrorMessage(ex.Message.ToString(), "insertHoneywellTransaction");
                return transResult; 
            }


        }

        public int insertHoneywelldashboard_yield_Transaction(H_Dashboard_yield_Transaction yielddata)
        {
            int yTransResult = 0;
            try
            {
                using (SqlConnection sqltranscon = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand sqlcmdtrans = new SqlCommand("pro_insertHoneywelldashboard_yield_Transaction", sqltranscon))
                    {
                        sqlcmdtrans.CommandType = CommandType.StoredProcedure;
                        sqlcmdtrans.Parameters.AddWithValue("@FCT_1", yielddata.FCT_1);
                        sqlcmdtrans.Parameters.AddWithValue("@FCT_2", yielddata.FCT_2);
                        sqlcmdtrans.Parameters.AddWithValue("@RF_1", yielddata.RF_1);
                        sqlcmdtrans.Parameters.AddWithValue("@RF_2",yielddata.RF_2);
                        sqlcmdtrans.Parameters.AddWithValue("@RTC", yielddata.RTC);
                        sqlcmdtrans.Parameters.AddWithValue("@VOLT",yielddata.VOLT);
                        sqlcmdtrans.Parameters.AddWithValue("@Honeywell_shift", yielddata.Honeywell_shift);
                        sqlcmdtrans.Parameters.AddWithValue("@Createid", yielddata.Createid.ToString());
                        sqlcmdtrans.Parameters.AddWithValue("@Updateid", yielddata.Updateid.ToString());
                        sqltranscon.Open();
                        yTransResult = sqlcmdtrans.ExecuteNonQuery();
                        sqltranscon.Close();
                        return yTransResult;

                    }
                }
            }
            catch (Exception ex)
            {
                writeErrorMessage(ex.Message.ToString(), "insertHoneywelldashboard_yield_Transaction");
                return yTransResult;
            }
        }

        public int insertHoneywelldashboard_hourly_Transaction(H_Dashboard_hourly_Transaction hourlyData)
        {
            int hTransResult = 0;
            try
            {
                using (SqlConnection sqltranscon = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand sqlcmdtrans = new SqlCommand("pro_insertHoneywelldashboard_hourlyoutput_Transaction", sqltranscon))
                    {
                        sqlcmdtrans.CommandType = CommandType.StoredProcedure;
                        sqlcmdtrans.Parameters.AddWithValue("@Honeywell_hour", hourlyData.Honeywell_hour.ToString());
                        sqlcmdtrans.Parameters.AddWithValue("@Honeywell_shift", hourlyData.Honeywell_shift.ToString());
                        sqlcmdtrans.Parameters.AddWithValue("@Honeywell_plan", hourlyData.Honeywell_plan);
                        sqlcmdtrans.Parameters.AddWithValue("@Honeywell_Actual", hourlyData.Honeywell_Actual);
                        sqlcmdtrans.Parameters.AddWithValue("@Createid", hourlyData.Createid.ToString());
                        sqlcmdtrans.Parameters.AddWithValue("@Updateid", hourlyData.Updateid.ToString());
                        sqltranscon.Open();
                        hTransResult = sqlcmdtrans.ExecuteNonQuery();
                        sqltranscon.Close();
                        return hTransResult;

                    }
                }
            }
            catch (Exception ex)
            {
                writeErrorMessage(ex.Message.ToString(), "insertHoneywelldashboard_hourly_Transaction");
                return hTransResult;
            }
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


                        //cmd.Parameters.AddWithValue("@type", "V200");
                        //cmd.Parameters.AddWithValue("@fg", "ECH3HWI00001");
                        //cmd.Parameters.AddWithValue("@date", "25-09-2025");
                        //cmd.Parameters.AddWithValue("@shift", "SHIFT-A");

                        cmd.Parameters.AddWithValue("@type", oeedowntime.TestType);
                        cmd.Parameters.AddWithValue("@fg", oeedowntime.FGName);
                        cmd.Parameters.AddWithValue("@date", date.ToString("dd-MM-yyyy"));
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



                        //cmd.Parameters.AddWithValue("@type", "V200");
                        //cmd.Parameters.AddWithValue("@fg", "ECH3HWI00001");
                        //cmd.Parameters.AddWithValue("@date", "25-09-2025");
                        //cmd.Parameters.AddWithValue("@shift", "SHIFT-A");


                        cmd.Parameters.AddWithValue("@type", dashboard_HourlyOP.TestType);
                        cmd.Parameters.AddWithValue("@fg", dashboard_HourlyOP.FGName);
                        cmd.Parameters.AddWithValue("@date", date.ToString("dd-MM-yyyy"));
                        cmd.Parameters.AddWithValue("@shift", shift);

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

        public List<Dashboard_HourlyOP> oee_data_upload(Dashboard_HourlyOP dashboard_HourlyOP)
        {
            List<Dashboard_HourlyOP> lstPerformance = new List<Dashboard_HourlyOP>();

            try
            {
                var date = DateTime.Now.Date;
                string shift = GetShiftLabel(DateTime.Now);

                using (SqlConnection sqlConn = new SqlConnection(Prod_ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("proc_lineeff_upload", sqlConn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;



                        //cmd.Parameters.AddWithValue("@type", "V200");
                        //cmd.Parameters.AddWithValue("@fg", "ECH3HWI00001");
                        //cmd.Parameters.AddWithValue("@date", "25-09-2025");
                        //cmd.Parameters.AddWithValue("@shift", "SHIFT-A");


                        cmd.Parameters.AddWithValue("@type", dashboard_HourlyOP.TestType);
                        cmd.Parameters.AddWithValue("@fg", dashboard_HourlyOP.FGName);
                        cmd.Parameters.AddWithValue("@date", date.ToString("dd-MM-yyyy"));
                        cmd.Parameters.AddWithValue("@shift", shift);

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


        public List<Dashboard_HourlyOP> gethourlyone(Dashboard_HourlyOP dashboard_HourlyOP)
        {
            List<Dashboard_HourlyOP> lstyieldDashboard = new List<Dashboard_HourlyOP>();
            Dashboard_HourlyOP objDashboard1;
            try
            {
                var date = DateTime.Now.Date;
                string proc = string.Empty;
                string shift = GetShiftLabel(DateTime.Now);
                string conn = string.Empty;
                proc = "DASHBOARD_A4GNICHNYTESTER";
                conn = A4;
              
                using (SqlConnection sqlhorly = new SqlConnection(conn))
                {

                    using (SqlCommand sqlcmdyield = new SqlCommand(proc, sqlhorly))
                    {
                        sqlcmdyield.CommandType = CommandType.StoredProcedure;
              
                        sqlcmdyield.Parameters.AddWithValue("@type", "HOURLY");
                        sqlcmdyield.Parameters.AddWithValue("@fg", dashboard_HourlyOP.FGName);
                        sqlcmdyield.Parameters.AddWithValue("@Stagevalue", "Burn-in Test");
                        sqlcmdyield.Parameters.AddWithValue("@dateval", "31-10-2025");
                        sqlcmdyield.Parameters.AddWithValue("@targetval", "85");


                        SqlDataAdapter dyhourly = new SqlDataAdapter(sqlcmdyield);
                        DataTable dtyhourly = new DataTable();
                        dyhourly.Fill(dtyhourly);
                        if (dtyhourly.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtyhourly.Rows)
                            {
                                objDashboard1 = new Dashboard_HourlyOP();
                                objDashboard1.Stage = "Burn-in Test";
                                objDashboard1.hour = Convert.ToInt32(dr["HOUR"].ToString());
                                objDashboard1.hourvalue = Convert.ToInt32(dr["VALUE"].ToString());
                                objDashboard1.Target = Convert.ToInt32(dr["TARGET"].ToString());
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
        public List<Dashboard_HourlyOP> gethourlytwo(Dashboard_HourlyOP dashboard_HourlyOP)
        {
            List<Dashboard_HourlyOP> lstyieldDashboard = new List<Dashboard_HourlyOP>();
            Dashboard_HourlyOP objDashboard1;
            try
            {
                var date = DateTime.Now.Date;
                string proc = string.Empty;
                string shift = GetShiftLabel(DateTime.Now);
                string conn = string.Empty;
                proc = "DASHBOARD_A4GNICHNYTESTER";
                conn = A4;

                using (SqlConnection sqlhorly = new SqlConnection(conn))
                {

                    using (SqlCommand sqlcmdyield = new SqlCommand(proc, sqlhorly))
                    {
                        sqlcmdyield.CommandType = CommandType.StoredProcedure;
                        
                        sqlcmdyield.Parameters.AddWithValue("@type", "HOURLY");
                        sqlcmdyield.Parameters.AddWithValue("@fg", dashboard_HourlyOP.FGName);
                        sqlcmdyield.Parameters.AddWithValue("@Stagevalue", "Case Test");
                        sqlcmdyield.Parameters.AddWithValue("@dateval", "31-10-2025");
                        sqlcmdyield.Parameters.AddWithValue("@targetval", "85");

                        SqlDataAdapter dyhourly = new SqlDataAdapter(sqlcmdyield);
                        DataTable dtyhourly = new DataTable();
                        dyhourly.Fill(dtyhourly);
                        if (dtyhourly.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtyhourly.Rows)
                            {
                                objDashboard1 = new Dashboard_HourlyOP();
                                objDashboard1.Stage = "Case Test";
                                objDashboard1.hour = Convert.ToInt32(dr["HOUR"].ToString());
                                objDashboard1.hourvalue = Convert.ToInt32(dr["VALUE"].ToString());
                                objDashboard1.Target = Convert.ToInt32(dr["TARGET"].ToString());
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

        public List<Dashboard_HourlyOP> gethourlythree(Dashboard_HourlyOP dashboard_HourlyOP)
        {
            List<Dashboard_HourlyOP> lstyieldDashboard = new List<Dashboard_HourlyOP>();
            Dashboard_HourlyOP objDashboard1;
            try
            {
                var date = DateTime.Now.Date;
                string proc = string.Empty;
                string shift = GetShiftLabel(DateTime.Now);
                string conn = string.Empty;
                proc = "DASHBOARD_A4GNICHNYTESTER";
                conn = A4;

                using (SqlConnection sqlhorly = new SqlConnection(conn))
                {

                    using (SqlCommand sqlcmdyield = new SqlCommand(proc, sqlhorly))
                    {
                        sqlcmdyield.CommandType = CommandType.StoredProcedure;
                       
                        sqlcmdyield.Parameters.AddWithValue("@type", "HOURLY");
                        sqlcmdyield.Parameters.AddWithValue("@fg", dashboard_HourlyOP.FGName);
                        sqlcmdyield.Parameters.AddWithValue("@Stagevalue", "FCT");
                        sqlcmdyield.Parameters.AddWithValue("@dateval", "31-10-2025");
                        sqlcmdyield.Parameters.AddWithValue("@targetval", "85");

                        SqlDataAdapter dyhourly = new SqlDataAdapter(sqlcmdyield);
                        DataTable dtyhourly = new DataTable();
                        dyhourly.Fill(dtyhourly);
                        if (dtyhourly.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtyhourly.Rows)
                            {
                                objDashboard1 = new Dashboard_HourlyOP();
                                objDashboard1.Stage = "FCT";
                                objDashboard1.hour = Convert.ToInt32(dr["HOUR"].ToString());
                                objDashboard1.hourvalue = Convert.ToInt32(dr["VALUE"].ToString());
                                objDashboard1.Target = Convert.ToInt32(dr["TARGET"].ToString());
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

        public List<Dashboard_HourlyOP> gethourlyfour(Dashboard_HourlyOP dashboard_HourlyOP)
        {
            List<Dashboard_HourlyOP> lstyieldDashboard = new List<Dashboard_HourlyOP>();
            Dashboard_HourlyOP objDashboard1;
            try
            {
                var date = DateTime.Now.Date;
                string proc = string.Empty;
                string shift = GetShiftLabel(DateTime.Now);
                string conn = string.Empty;
                proc = "DASHBOARD_A4GNICHNYTESTER";
                conn = A4;

                using (SqlConnection sqlhorly = new SqlConnection(conn))
                {

                    using (SqlCommand sqlcmdyield = new SqlCommand(proc, sqlhorly))
                    {
                        sqlcmdyield.CommandType = CommandType.StoredProcedure;
                      
                        sqlcmdyield.Parameters.AddWithValue("@type", "HOURLY");
                        sqlcmdyield.Parameters.AddWithValue("@fg", dashboard_HourlyOP.FGName);
                        sqlcmdyield.Parameters.AddWithValue("@Stagevalue", "Flash Test");
                        sqlcmdyield.Parameters.AddWithValue("@dateval", "31-10-2025");
                        sqlcmdyield.Parameters.AddWithValue("@targetval", "85");

                        SqlDataAdapter dyhourly = new SqlDataAdapter(sqlcmdyield);
                        DataTable dtyhourly = new DataTable();
                        dyhourly.Fill(dtyhourly);
                        if (dtyhourly.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtyhourly.Rows)
                            {
                                objDashboard1 = new Dashboard_HourlyOP();
                                objDashboard1.Stage = "Flash Test";
                                objDashboard1.hour = Convert.ToInt32(dr["HOUR"].ToString());
                                objDashboard1.hourvalue = Convert.ToInt32(dr["VALUE"].ToString());
                                objDashboard1.Target = Convert.ToInt32(dr["TARGET"].ToString());
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

        public List<Dashboard_HourlyOP> gethourlyfive(Dashboard_HourlyOP dashboard_HourlyOP)
        {
            List<Dashboard_HourlyOP> lstyieldDashboard = new List<Dashboard_HourlyOP>();
            Dashboard_HourlyOP objDashboard1;
            try
            {
                var date = DateTime.Now.Date;
                string proc = string.Empty;
                string shift = GetShiftLabel(DateTime.Now);
                string conn = string.Empty;
                proc = "DASHBOARD_A4GNICHNYTESTER";
                conn = A4;

                using (SqlConnection sqlhorly = new SqlConnection(conn))
                {

                    using (SqlCommand sqlcmdyield = new SqlCommand(proc, sqlhorly))
                    {
                        sqlcmdyield.CommandType = CommandType.StoredProcedure;
                     
                        sqlcmdyield.Parameters.AddWithValue("@type", "HOURLY");
                        sqlcmdyield.Parameters.AddWithValue("@fg", dashboard_HourlyOP.FGName);
                        sqlcmdyield.Parameters.AddWithValue("@Stagevalue", "Pre Burn-in Test");
                        sqlcmdyield.Parameters.AddWithValue("@dateval", "31-10-2025");
                        sqlcmdyield.Parameters.AddWithValue("@targetval", "85");

                        SqlDataAdapter dyhourly = new SqlDataAdapter(sqlcmdyield);
                        DataTable dtyhourly = new DataTable();
                        dyhourly.Fill(dtyhourly);
                        if (dtyhourly.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtyhourly.Rows)
                            {
                                objDashboard1 = new Dashboard_HourlyOP();
                                objDashboard1.Stage = "Pre Burn-in Test";
                                objDashboard1.hour = Convert.ToInt32(dr["HOUR"].ToString());
                                objDashboard1.hourvalue = Convert.ToInt32(dr["VALUE"].ToString());
                                objDashboard1.Target = Convert.ToInt32(dr["TARGET"].ToString());
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


        public List<Dashboard_HourlyOP> getyieldDataOne(Dashboard_HourlyOP dashboard_HourlyOP)
        {
            List<Dashboard_HourlyOP> lstyieldDashboard = new List<Dashboard_HourlyOP>();
            Dashboard_HourlyOP objDashboard1;
            try
            {
                var date = DateTime.Now.Date;
                string proc = string.Empty;
                string shift = GetShiftLabel(DateTime.Now);
                string conn = string.Empty;
                proc = "DASHBOARD_A4GNICHNYTESTER";
                conn = A4;

                using (SqlConnection sqlhorly = new SqlConnection(conn))
                {

                    using (SqlCommand sqlcmdyield = new SqlCommand(proc, sqlhorly))
                    {
                        sqlcmdyield.CommandType = CommandType.StoredProcedure;
                        sqlcmdyield.Parameters.AddWithValue("@type", "YIELD");
                        sqlcmdyield.Parameters.AddWithValue("@fg", dashboard_HourlyOP.FGName);
                        sqlcmdyield.Parameters.AddWithValue("@Stagevalue", "Burn-in Test");
                        sqlcmdyield.Parameters.AddWithValue("@dateval", "31-10-2025");
                        sqlcmdyield.Parameters.AddWithValue("@targetval", "85");

                        SqlDataAdapter dyhourly = new SqlDataAdapter(sqlcmdyield);
                        DataTable dtyhourly = new DataTable();
                        dyhourly.Fill(dtyhourly);
                        if (dtyhourly.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtyhourly.Rows)
                            {
                                objDashboard1 = new Dashboard_HourlyOP();
                                objDashboard1.Stage = "Burn-in Test";
                                objDashboard1.Passcountyield = Convert.ToInt32(dr["PASSCOUNT"].ToString());
                                objDashboard1.Failcountyield = Convert.ToInt32(dr["FAILCOUNT"].ToString());
                                objDashboard1.Yield = Convert.ToDecimal(dr["YIELD"].ToString());
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
        public List<Dashboard_HourlyOP> getyieldDatatwo(Dashboard_HourlyOP dashboard_HourlyOP)
        {
            List<Dashboard_HourlyOP> lstyieldDashboard = new List<Dashboard_HourlyOP>();
            Dashboard_HourlyOP objDashboard1;
            try
            {
                var date = DateTime.Now.Date;
                string proc = string.Empty;
                string shift = GetShiftLabel(DateTime.Now);
                string conn = string.Empty;
                proc = "DASHBOARD_A4GNICHNYTESTER";
                conn = A4;

                using (SqlConnection sqlhorly = new SqlConnection(conn))
                {

                    using (SqlCommand sqlcmdyield = new SqlCommand(proc, sqlhorly))
                    {
                        sqlcmdyield.CommandType = CommandType.StoredProcedure;

                        sqlcmdyield.Parameters.AddWithValue("@type", "YIELD");
                        sqlcmdyield.Parameters.AddWithValue("@fg", dashboard_HourlyOP.FGName);
                        sqlcmdyield.Parameters.AddWithValue("@Stagevalue", "Case Test");
                        sqlcmdyield.Parameters.AddWithValue("@dateval", "31-10-2025");
                        sqlcmdyield.Parameters.AddWithValue("@targetval", "85");

                        SqlDataAdapter dyhourly = new SqlDataAdapter(sqlcmdyield);
                        DataTable dtyhourly = new DataTable();
                        dyhourly.Fill(dtyhourly);
                        if (dtyhourly.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtyhourly.Rows)
                            {
                                objDashboard1 = new Dashboard_HourlyOP();
                                objDashboard1.Stage = "Case Test";
                                objDashboard1.Passcountyield = Convert.ToInt32(dr["PassCount"].ToString());
                                objDashboard1.Failcountyield = Convert.ToInt32(dr["FailCount"].ToString());
                                objDashboard1.Yield = Convert.ToDecimal(dr["YIELD"].ToString());
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

        public List<Dashboard_HourlyOP> getyieldDatathree(Dashboard_HourlyOP dashboard_HourlyOP)
        {
            List<Dashboard_HourlyOP> lstyieldDashboard = new List<Dashboard_HourlyOP>();
            Dashboard_HourlyOP objDashboard1;
            try
            {
                var date = DateTime.Now.Date;
                string proc = string.Empty;
                string shift = GetShiftLabel(DateTime.Now);
                string conn = string.Empty;
                proc = "DASHBOARD_A4GNICHNYTESTER";
                conn = A4;

                using (SqlConnection sqlhorly = new SqlConnection(conn))
                {

                    using (SqlCommand sqlcmdyield = new SqlCommand(proc, sqlhorly))
                    {
                        sqlcmdyield.CommandType = CommandType.StoredProcedure;                    

                        sqlcmdyield.Parameters.AddWithValue("@type", "YIELD");
                        sqlcmdyield.Parameters.AddWithValue("@fg", dashboard_HourlyOP.FGName);
                        sqlcmdyield.Parameters.AddWithValue("@Stagevalue", "FCT");
                        sqlcmdyield.Parameters.AddWithValue("@dateval", "31-10-2025");
                        sqlcmdyield.Parameters.AddWithValue("@targetval", "85");

                        SqlDataAdapter dyhourly = new SqlDataAdapter(sqlcmdyield);
                        DataTable dtyhourly = new DataTable();
                        dyhourly.Fill(dtyhourly);
                        if (dtyhourly.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtyhourly.Rows)
                            {
                                objDashboard1 = new Dashboard_HourlyOP();
                                objDashboard1.Stage = "FCT";
                                objDashboard1.Passcountyield = Convert.ToInt32(dr["PassCount"].ToString());
                                objDashboard1.Failcountyield = Convert.ToInt32(dr["FailCount"].ToString());
                                objDashboard1.Yield = Convert.ToDecimal(dr["YIELD"].ToString());
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

        public List<Dashboard_HourlyOP> getyieldDatafour(Dashboard_HourlyOP dashboard_HourlyOP)
        {
            List<Dashboard_HourlyOP> lstyieldDashboard = new List<Dashboard_HourlyOP>();
            Dashboard_HourlyOP objDashboard1;
            try
            {
                var date = DateTime.Now.Date;
                string proc = string.Empty;
                string shift = GetShiftLabel(DateTime.Now);
                string conn = string.Empty;
                proc = "DASHBOARD_A4GNICHNYTESTER";
                conn = A4;

                using (SqlConnection sqlhorly = new SqlConnection(conn))
                {

                    using (SqlCommand sqlcmdyield = new SqlCommand(proc, sqlhorly))
                    {
                        sqlcmdyield.CommandType = CommandType.StoredProcedure;

                        sqlcmdyield.Parameters.AddWithValue("@type", "YIELD");
                        sqlcmdyield.Parameters.AddWithValue("@fg", dashboard_HourlyOP.FGName);
                        sqlcmdyield.Parameters.AddWithValue("@Stagevalue", "Flash Test");
                        sqlcmdyield.Parameters.AddWithValue("@dateval", "31-10-2025");
                        sqlcmdyield.Parameters.AddWithValue("@targetval", "85");

                        SqlDataAdapter dyhourly = new SqlDataAdapter(sqlcmdyield);
                        DataTable dtyhourly = new DataTable();
                        dyhourly.Fill(dtyhourly);
                        if (dtyhourly.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtyhourly.Rows)
                            {
                                objDashboard1 = new Dashboard_HourlyOP();
                                objDashboard1.Stage = "Flash Test";
                                objDashboard1.Passcountyield = Convert.ToInt32(dr["PassCount"].ToString());
                                objDashboard1.Failcountyield = Convert.ToInt32(dr["FailCount"].ToString());
                                objDashboard1.Yield = Convert.ToDecimal(dr["YIELD"].ToString());
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

        public List<Dashboard_HourlyOP> getyieldDatafive(Dashboard_HourlyOP dashboard_HourlyOP)
        {
            List<Dashboard_HourlyOP> lstyieldDashboard = new List<Dashboard_HourlyOP>();
            Dashboard_HourlyOP objDashboard1;
            try
            {
                var date = DateTime.Now.Date;
                string proc = string.Empty;
                string shift = GetShiftLabel(DateTime.Now);
                string conn = string.Empty;
                proc = "DASHBOARD_A4GNICHNYTESTER";
                conn = A4;

                using (SqlConnection sqlhorly = new SqlConnection(conn))
                {

                    using (SqlCommand sqlcmdyield = new SqlCommand(proc, sqlhorly))
                    {
                        sqlcmdyield.CommandType = CommandType.StoredProcedure;
                       
                        sqlcmdyield.Parameters.AddWithValue("@type", "YIELD");
                        sqlcmdyield.Parameters.AddWithValue("@fg", dashboard_HourlyOP.FGName);
                        sqlcmdyield.Parameters.AddWithValue("@Stagevalue", "Pre Burn-in Test");
                        sqlcmdyield.Parameters.AddWithValue("@dateval", "31-10-2025");
                        sqlcmdyield.Parameters.AddWithValue("@targetval", "85");


                        SqlDataAdapter dyhourly = new SqlDataAdapter(sqlcmdyield);
                        DataTable dtyhourly = new DataTable();
                        dyhourly.Fill(dtyhourly);
                        if (dtyhourly.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtyhourly.Rows)
                            {
                                objDashboard1 = new Dashboard_HourlyOP();
                                objDashboard1.Stage = "Pre Burn-in Test";
                                objDashboard1.Passcountyield = Convert.ToInt32(dr["PassCount"].ToString());
                                objDashboard1.Failcountyield = Convert.ToInt32(dr["FailCount"].ToString());
                                objDashboard1.Yield = Convert.ToDecimal(dr["YIELD"].ToString());
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
        public void writeErrorMessage(string Message, string FuncationName)
        {
            // Ensure the directory exists
            string systemPath = "D:\\Honeywell\\Logs\"";
            if (!Directory.Exists(systemPath))
            {
                Directory.CreateDirectory(systemPath);
            }

            // Prepare log file path
            string currentDate = DateTime.Now.ToString("dd-MM-yyyy");
            string errorLogFileName = $"Log_{currentDate}.txt";
            string errorLogPath = Path.Combine(systemPath, errorLogFileName);

            // Write to log file
            using (StreamWriter errLogs = new StreamWriter(errorLogPath, true))
            {
                errLogs.WriteLine("--------------------------------------------------------------------------------------------------------------------" + Environment.NewLine);
                errLogs.WriteLine("---------------------------------------------------" + DateTime.Now + "----------------------------------------------" + Environment.NewLine);
                errLogs.WriteLine($"Log Message: {Message}" + Environment.NewLine);
                errLogs.WriteLine($"Function Name: {FuncationName}" + Environment.NewLine);
                errLogs.WriteLine($"Date Time:" + DateTime.Now.ToString());
                errLogs.Close();
            }
        }

    }

}

