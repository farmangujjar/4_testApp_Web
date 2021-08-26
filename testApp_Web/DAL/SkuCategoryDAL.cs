using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using testApp_Web.Models;

namespace testApp_Web.DAL
{
    public class SkuCategoryDAL
    {
        public List<string> GetSkuCategoryList(int shipmentID)
        {
            try
            {
                testAppEntities entities = new testAppEntities();
                return entities.stp_Shipments_Shipment_GetSkuCategoryList(shipmentID).ToList();
            }
            catch (Exception)
            {
                return null;
            }


            ////RadioGroup1.Properties.Items.Clear();
            ////RadioGroupItem it = new RadioGroupItem();
            ////it.Description = "All Skus";
            ////it.Value = "All Skus";
            ////RadioGroup1.Properties.Items.Add(it);
            ////RadioGroup1.SelectedIndex = 0;
            ////try
            ////{
            //string sql = "stp_Shipments_Shipment_GetSkuCategoryList";

            //    string StrSql = "";
            //    using (SqlConnection cn = new SqlConnection(RunSql.GetConStr()))
            //    {
            //        SqlCommand SqlCn = new SqlCommand(sql, cn);
            //        SqlCn.CommandType = CommandType.StoredProcedure;

            //        SqlCn.Connection = cn;
            //        SqlCn.Connection.Open();
            //        SqlCn.CommandTimeout = 30;

            //        SqlCn.Parameters.Add("@ShipmentId", SqlDbType.Int);
            //        SqlCn.Parameters("@ShipmentId").Value = ShipmentID;

            //        SqlDataReader Daa = SqlCn.ExecuteReader;
            //        while (Daa.Read)
            //        {
            //            it = new RadioGroupItem();
            //            it.Description = Daa.Item(0)?.ToString;
            //            it.Value = Daa.Item(0)?.ToString;
            //            RadioGroup1.Properties.Items.Add(it);
            //        }
            //        Daa.Close();
            //    }
            //}
            //catch (Exception ex)
            //{
            //}

            finally
            {
            }
        }
    }
}