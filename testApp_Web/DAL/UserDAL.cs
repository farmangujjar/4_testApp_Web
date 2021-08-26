using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using testApp_Web.Models;

namespace testApp_Web.DAL
{
    public class UserDAL
    {
        public User Verify_UserByPin(int pin)
        {
            try
            {
                testAppEntities entities = new testAppEntities();
                return entities.Users.FirstOrDefault(p => p.PinLogin == pin && p.UserID == 3);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public List<string> Get_WarehouseUsers()
        {
            try
            {
                string connection = "Server=DESKTOP-QMBHE63;Database=testApp;Trusted_Connection=True;";
                List<string> usr = new List<string>();

                SqlConnection conn = new SqlConnection(connection);

                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter da = new SqlDataAdapter();
                DataTable dt = new DataTable();
                try
                {
                    conn.Open();
                    cmd = new SqlCommand("stp_GetWarehouseUsers", conn);
                    cmd.Parameters.Add(new SqlParameter("@Dept", ""));
                    cmd.Parameters.Add(new SqlParameter("@SubDept", ""));

                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);


                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        usr.Add(dt.Rows[i]["UserName"].ToString());
                    }
                    return usr;
                }
                catch (Exception x)
                {
                }
                finally
                {
                    cmd.Dispose();
                    conn.Close();
                }
                return usr;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}