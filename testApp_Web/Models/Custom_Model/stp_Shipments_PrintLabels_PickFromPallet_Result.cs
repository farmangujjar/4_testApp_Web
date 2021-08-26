using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace testApp_Web.Models.Custom_Model
{
    public class stp_Shipments_PrintLabels_PickFromPallet_Result
    {

        public string ItemNum { get; set; }
        public int SeqPalletNo { get; set; }
        public string UOM { get; set; }
        public int qtyTake { get; set; }
        public int PickedForShip { get; set; }
        public string ExpDate { get; set; }
    }
}