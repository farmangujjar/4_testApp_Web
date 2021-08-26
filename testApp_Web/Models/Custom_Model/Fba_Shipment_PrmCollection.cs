using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace testApp_Web.Models.Custom_Model
{
    public class Fba_Shipment_PrmCollection
    {
        public int shipmentID { get; set; }
        public string sku { get; set; }
        public string whseUser { get; set; }
        public int shipDetailID { get; set; }
        public int QtyInCase { get; set; }
        public int palletNo { get; set; }
        public string expDate { get; set; }
        public int skuLablelsNeed { get; set; }
        public int skuLabelsPrinted { get; set; }
        public int boxLabels { get; set; }
    }
}