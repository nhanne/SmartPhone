//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Nike.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Order_Detail
    {
        public int ID_Order { get; set; }
        public int ID_Product { get; set; }
        public Nullable<int> SoLuong { get; set; }
        public Nullable<double> Price { get; set; }
    
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
