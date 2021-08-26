using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using testApp_Web.Models;
using testApp_Web.Models.Custom_Model;

namespace testApp_Web.DAL
{
    public class FBAShipmentDAL
    {
        string connection = "Server=DESKTOP-QMBHE63;Database=testApp;Trusted_Connection=True;";
        public List<stp_Shipments_GetWorkingShipments_Result> LoadShipments(bool? whse, int? account, int? user)
        {
            try
            {
                testAppEntities entities = new testAppEntities();
                return entities.stp_Shipments_GetWorkingShipments(whse, account, user).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public int GetPalletNo(int ShipmentID, string Sku, int user,
            string WhseUser, int PalletsNo, DateTime expDate)
        {
            try
            {
                testAppEntities entities = new testAppEntities();
                return entities.stp_Shipments_PrintLabels_PickFromPallet(ShipmentID, Sku, user, WhseUser, PalletsNo, expDate).FirstOrDefault().Value;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public List<stp_Shipments_PrintLabels_GetConfirmInfo_Result> GetConfirmInfo(int _shpDetailID)
        {
            try
            {
                testAppEntities entities = new testAppEntities();
                return entities.stp_Shipments_PrintLabels_GetConfirmInfo(_shpDetailID).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }
        public List<stp_Shipments_PrintLabels_PickFromPallet_Result> LoadShipmentByPallet(int ShipmentID, string Sku, int user,
            string WhseUser,int PalletsNo, DateTime expDate)
        {
            try
            {
                //testAppEntities entities = new testAppEntities();

                //string ss = Sku;

                List<stp_Shipments_PrintLabels_PickFromPallet_Result> result = new List<stp_Shipments_PrintLabels_PickFromPallet_Result>();
                
                //    entities.stp_Shipments_PrintLabels_PickFromPallet(ShipmentID, "3M-654-5UC-4a", 1, WhseUser, PalletsNo, null).Cast<stp_Shipments_PrintLabels_PickFromPallet_Result>().ToList();

                SqlConnection conn = new SqlConnection(connection);

                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter da = new SqlDataAdapter();
                DataTable dt = new DataTable();
                try
                {
                    conn.Open();
                    cmd = new SqlCommand("stp_Shipments_PrintLabels_PickFromPallet", conn);

                    //exec stp_Shipments_PrintLabels_PickFromPallet 87, 'CR-51-0320-Kit-6+6',1, 'Pinchas',1, '2024-02-06'

                    cmd.Parameters.Add(new SqlParameter("@ShipmentID", ShipmentID));
                    cmd.Parameters.Add(new SqlParameter("@Sku", Sku));
                    cmd.Parameters.Add(new SqlParameter("@User", 1));
                    cmd.Parameters.Add(new SqlParameter("@WhseUser", WhseUser));
                    cmd.Parameters.Add(new SqlParameter("@Pallets", PalletsNo));
                    cmd.Parameters.Add(new SqlParameter("@ExpDate", expDate));

                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);


                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        stp_Shipments_PrintLabels_PickFromPallet_Result obj = new stp_Shipments_PrintLabels_PickFromPallet_Result();

                        obj.ItemNum = dt.Rows[i]["ItemNum"].ToString();
                        obj.SeqPalletNo = Convert.ToInt16(dt.Rows[i]["SeqPalletNo"].ToString());
                        obj.UOM = dt.Rows[i]["UOM"].ToString();
                        obj.qtyTake = Convert.ToInt16(dt.Rows[i]["qtyTake"].ToString());
                        obj.PickedForShip = Convert.ToInt16(dt.Rows[i]["PickedForShip"].ToString());
                        obj.ExpDate = dt.Rows[i]["ExpDate"].ToString();


                        result.Add(obj);

                    }
                    return result;
                }
                catch (Exception x)
                {
                }
                finally
                {
                    cmd.Dispose();
                    conn.Close();
                }
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        

        public List<stp_Shipments_PrintLabels_Skus_Result> Get_Shipment_PrintLabels_Sku(int shipmentID, string searchStr)
        {
            try
            {
                testAppEntities entities = new testAppEntities();
                return entities.stp_Shipments_PrintLabels_Skus(shipmentID, searchStr).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<stp_Shipments_GetItemsInSku_Result> Get_Shipment_Detail(int shipmentDetailID)
        {
            try
            {
                testAppEntities entities = new testAppEntities();
                return entities.stp_Shipments_GetItemsInSku(shipmentDetailID).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }


        public bool Update_Shipment_Wise(int shipmentid, int showWhse)
        {
            try
            {
                testAppEntities entities = new testAppEntities();
                var result = entities.FBAShipments.FirstOrDefault(b => b.ShipmentID == shipmentid);
                if (result != null)
                {
                    result.ShowWhse = showWhse;
                    entities.SaveChanges();
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public int Delete_Shipments_By_DeleteSku(int pin, int shipmentID, string sku)
        {
            try
            {
                UserDAL uDal = new UserDAL();
                User u = uDal.Verify_UserByPin(pin);


                if (u != null)
                {
                    string whseUser = u.UserName;

                    testAppEntities entities = new testAppEntities();
                    entities.stp_Shipments_PrintLabels_DeleteSku(shipmentID, sku);

                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception)
            {
                return -1;
            }
        }
    }
}