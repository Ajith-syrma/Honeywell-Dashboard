
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Data.SqlClient;

namespace Honeywell_Production_Dashboard.Models
{
   
    public class DataManagement 
    {
        private readonly String ConnectionString;
        public DataManagement(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("conn");
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
            }
            return FgName;
        }
    }
}
