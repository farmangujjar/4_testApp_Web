using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using testApp_Web.DAL;
using testApp_Web.Models;

namespace testApp_Web.Controllers
{
    public class PrintLabelsController : Controller
    {
        // GET: PrintLabels
        public ActionResult Index()
        {
            SkuCategoryDAL cat_Dal = new SkuCategoryDAL();
            List<string> categories = cat_Dal.GetSkuCategoryList(87);

            UserDAL u_Dal = new UserDAL();
            //List<stp_GetWarehouseUsers_Entity_Result> users = u_Dal.Get_WarehouseUsers(null, null);


            ViewBag.Categories = categories;
            //ViewBag.User = users;

            return View();
        }


        public JsonResult LoadShipments()
        {
            FBAShipmentDAL dal = new FBAShipmentDAL();

            using (testAppEntities dc = new testAppEntities())
            {
                bool? whse = false;
                int? account = 1;
                int? user = 1;

                List<stp_Shipments_GetWorkingShipments_Result> result = new List<stp_Shipments_GetWorkingShipments_Result>();
                result = dal.LoadShipments(whse, account, user);
                return new JsonResult { Data = new { List = result }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }
        public JsonResult Get_ShipmentSkus(int shipmentID, string searchStr)
        {
            FBAShipmentDAL dal = new FBAShipmentDAL();

            using (testAppEntities dc = new testAppEntities())
            {
                List<stp_Shipments_PrintLabels_Skus_Result> result = new List<stp_Shipments_PrintLabels_Skus_Result>();

                result = dal.Get_Shipment_PrintLabels_Sku(shipmentID, searchStr);
                
                return new JsonResult { Data = new { List = result.ToList() }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }


        public JsonResult Get_Shipment_Detail(int shipmentDetailID)
        {
            FBAShipmentDAL dal = new FBAShipmentDAL();

            using (testAppEntities dc = new testAppEntities())
            {
                List<stp_Shipments_GetItemsInSku_Result> result = new List<stp_Shipments_GetItemsInSku_Result>();

                result = dal.Get_Shipment_Detail(shipmentDetailID);

                return new JsonResult { Data = new { List = result }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }
        public JsonResult Update_Shipment_Wise(int shipmentid, int showWhse)
        {
            FBAShipmentDAL dal = new FBAShipmentDAL();

            using (testAppEntities dc = new testAppEntities())
            {
                bool r = dal.Update_Shipment_Wise(shipmentid, showWhse);

                return new JsonResult { Data = new { List = r }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }


        public JsonResult Delete_Shipments_By_DeleteSku(int pin, int shipmentid, string sku)
        {
            FBAShipmentDAL dal = new FBAShipmentDAL();

            using (testAppEntities dc = new testAppEntities())
            {
                int result = dal.Delete_Shipments_By_DeleteSku(pin, shipmentid, sku);
                return new JsonResult { Data = new { status = result }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }
    }
}