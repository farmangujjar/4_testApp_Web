using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using testApp_Web.DAL;
using testApp_Web.Models;
using testApp_Web.Models.Custom_Model;

namespace testApp_Web.Controllers
{

    public class FBAShipments_PickController : Controller
    {
        
        FBAShipmentDAL dal = new FBAShipmentDAL();
        

        // GET: FBAShipments_Pick        
        public ActionResult Index(int shipmentID, string AmzShipmentID, string sku, string whseUser, int shipDetailID, int QtyInCase, string expDate,
            string split, int qty, int lbls, int boxLblsPrinted, int fnskuLabelsPrinted,
            int boxLabels, int fnskuLablelsNeed)
        {
            sku = sku.Replace(" ", "+");

            int palletNo = 0;
            int userID = 1;

            List<string> pallets = LookupDAL.Get_Pallets(shipmentID, shipDetailID);

            DateTime expiryDate = Convert.ToDateTime(expDate);
            palletNo = dal.GetPalletNo(shipmentID, sku, userID, whseUser, palletNo, expiryDate);


            Fba_Shipment_PrmCollection prms = new Fba_Shipment_PrmCollection();
            prms.shipmentID = shipmentID;
            prms.sku = sku;
            prms.whseUser = whseUser;
            prms.shipDetailID = shipDetailID;
            prms.QtyInCase = QtyInCase;
            prms.palletNo = palletNo;
            prms.expDate = expDate;
            prms.skuLablelsNeed = fnskuLablelsNeed;
            prms.skuLabelsPrinted = fnskuLabelsPrinted;
            prms.boxLabels = boxLabels;



            List<string> prepType = LookupDAL.stp_GetChoiceDropdown("Prepping", "");

            stp_Shipments_PrintLabels_GetConfirmInfo_Result confirmObj = Get_ConfirmInfo(shipDetailID).FirstOrDefault();

            List<string> prepSize = LookupDAL.stp_GetChoiceDropdown("Prepping", confirmObj.PrepType);

            List<string> boxType = LookupDAL.stp_GetChoiceDropdown("Boxing", "");





            //Dim confirm = RunSql.QueryDB("exec stp_Shipments_PrintLabels_GetConfirmInfo " & _shipDetailID)
            //If confirm IsNot Nothing Then
            //    If confirm.Rows.Count > 0 Then
            //        Dim prep = confirm.Rows(0).Item("PrepInfoConfirmed")
            //        cmbPrepType.EditValue = confirm.Rows(0).Item("PrepType")?.ToString
            //        cmbPrepSize.EditValue = confirm.Rows(0).Item("PrepSize")?.ToString
            //        cmbBox.EditValue = confirm.Rows(0).Item("BoxType")?.ToString
            //        chkSoldAsSetLbls.Checked = confirm.Rows(0).Item("SoldAsSetLabels")?.ToString
            //        txtSasLbls.EditValue = confirm.Rows(0).Item("SoldAsSetPerItem")

            //        Dim first As Boolean = True
            //        Dim d = RunSql.QueryDB("select distinct PrepInstructions from SkuPrepInfo where PrepInstructions is not null and PrepInstructions <> '' and sku='" & _sku & "'")
            //        If d IsNot Nothing Then
            //            For Each pr As DataRow In d.Rows
            //                If first Then
            //                    lblPrepInstr.Text = "• " & pr.Item(0)?.ToString
            //                    first = False
            //                Else
            //                    lblPrepInstr.Text += vbNewLine & "• " & pr.Item(0)?.ToString
            //                End If

            //            Next
            //        End If

            //        If prep Then
            //            lcgConfirmQty.Expanded = False
            //            lcgPrepInfo.Expanded = False
            //            lcgPrintLbls.Expanded = True
            //        ElseIf _QtyConfirmed = 1 Then
            //            lcgConfirmQty.Expanded = False
            //            lcgPrepInfo.Expanded = True
            //            lcgPrintLbls.Expanded = False
            //        End If

            //        If _QtyConfirmed = 1 Then
            //            lcgConfirmQty.CaptionImageOptions.Image = My.Resources.ok_24
            //        Else
            //            lcgConfirmQty.CaptionImageOptions.Image = My.Resources.info_24__1_
            //        End If

            //        If _lblsComplete = 1 Then
            //            lcgPrintLbls.CaptionImageOptions.Image = My.Resources.ok_24
            //        Else
            //            lcgPrintLbls.CaptionImageOptions.Image = My.Resources.info_24__1_
            //        End If

            //        If prep Then
            //            lcgPrepInfo.CaptionImageOptions.Image = My.Resources.ok_24
            //            btnConfirmPrem.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Success
            //            btnConfirmPrem.Text = "Confirmed"
            //            _PrepConfirmed = True
            //        Else
            //            lcgPrepInfo.CaptionImageOptions.Image = My.Resources.info_24__1_
            //        End If

            //        If prep And _QtyConfirmed = 1 Then
            //            lcgPrintLbls.Enabled = True
            //            SimpleButton4.Enabled = True
            //        Else
            //            lcgPrintLbls.Enabled = False
            //            SimpleButton4.Enabled = False
            //        End If
            //    End If
            //End If





            ViewBag.Pallets = pallets;
            ViewBag.PrepType = prepType;
            ViewBag.BoxType = boxType;
            ViewBag.ConfirmInfo = Get_ConfirmInfo(shipDetailID).FirstOrDefault();


            return View(prms);
        }



        public JsonResult GetItems(int shipmentID, string sku, string whseUser, int shipDetailID, int QtyInCase, int palletNo, string expDate)
        {
            int userID = 1;

            FBAShipmentDAL dal = new FBAShipmentDAL();

            using (testAppEntities dc = new testAppEntities())
            {
                DateTime expiryDate = Convert.ToDateTime(expDate);

                List<stp_Shipments_PrintLabels_PickFromPallet_Result> result = new List<stp_Shipments_PrintLabels_PickFromPallet_Result>();
                result = dal.LoadShipmentByPallet(shipmentID, sku, userID, whseUser, palletNo, expiryDate);
                return new JsonResult { Data = new { List = result }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        private List<stp_Shipments_PrintLabels_PickFromPallet_Result> LoadByPallet(int shipmentID, string sku, int userID, string whseUser, int palletNo, string expDate)
        {

            DateTime expiryDate = Convert.ToDateTime(expDate);


            palletNo = dal.GetPalletNo(shipmentID, sku, userID, whseUser, palletNo, expiryDate);
            return dal.LoadShipmentByPallet(shipmentID, sku, userID, whseUser, palletNo, expiryDate);
        }

        private List<stp_Shipments_PrintLabels_GetConfirmInfo_Result> Get_ConfirmInfo(int _shipDetailID)
        {
            return dal.GetConfirmInfo(_shipDetailID);
            //Dim confirm = RunSql.QueryDB("exec stp_Shipments_PrintLabels_GetConfirmInfo " & _shipDetailID)
        }

        public JsonResult Update_Pallets(string palletnumbers)
        {
            FBAShipmentDAL dal = new FBAShipmentDAL();

            using (testAppEntities dc = new testAppEntities())
            {
                //List<stp_Shipments_PrintLabels_Skus_Result> result = new List<stp_Shipments_PrintLabels_Skus_Result>();

                //result = dal.Get_Shipment_PrintLabels_Sku(shipmentID, searchStr);

                return new JsonResult { Data = new { List = 1 }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        public JsonResult GetChoiceDropDown(string prepType, string description)
        {
            FBAShipmentDAL dal = new FBAShipmentDAL();

            using (testAppEntities dc = new testAppEntities())
            {
                List<string> result = LookupDAL.stp_GetChoiceDropdown(prepType, description);
                return new JsonResult { Data = new { List = result }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }
    }
}