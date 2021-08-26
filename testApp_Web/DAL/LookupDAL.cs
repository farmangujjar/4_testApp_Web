using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using testApp_Web.Models;

namespace testApp_Web.DAL
{
    public static class LookupDAL
    {
        static string connection = "Server=DESKTOP-QMBHE63;Database=testApp;Trusted_Connection=True;";

        public static List<string> Get_Pallets(int shiID, int shipDetailID)
        {
            try
            {
                List<string> pallets = new List<string>();

                SqlConnection conn = new SqlConnection(connection);

                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter da = new SqlDataAdapter();
                DataTable dt = new DataTable();
                try
                {

                    //stp_Shipments_PrintLabels_PickFromPallet


                    conn.Open();
                    cmd = new SqlCommand("stp_Shipments_PrintLabels_GetPallets", conn);
                    cmd.Parameters.Add(new SqlParameter("@ShipmentID", shiID));
                    //cmd.Parameters.Add(new SqlParameter("@ShipDetailID", ""));

                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);


                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        pallets.Add(dt.Rows[i]["PalletNum"].ToString());
                    }
                    return pallets;
                }
                catch (Exception x)
                {
                }
                finally
                {
                    cmd.Dispose();
                    conn.Close();
                }
                return pallets;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static List<string> stp_GetChoiceDropdown(string choiceName, string choiceDescription)
        {
            try
            {
                testAppEntities entities = new testAppEntities();
                return entities.stp_GetChoiceDropdown(choiceName, choiceDescription).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }
        
    }
}