//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace testApp_Web.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Item
    {
        public int ItemID { get; set; }
        public string ItemNumber { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string CreateUser { get; set; }
        public decimal ItemCost { get; set; }
        public string ItemDesc { get; set; }
        public Nullable<int> MfgCs { get; set; }
        public Nullable<int> PackQty { get; set; }
        public Nullable<int> ModifyUser { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
        public string StockSource { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<bool> OnHold { get; set; }
        public Nullable<int> HoldQtyAlert_Below { get; set; }
        public Nullable<int> HoldQtyAlert_Above { get; set; }
        public string Mapping { get; set; }
        public string UPC { get; set; }
    }
}
