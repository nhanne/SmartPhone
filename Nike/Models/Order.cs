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
    
    public partial class Order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            this.Order_Detail = new HashSet<Order_Detail>();
        }
        public int ID { get; set; }
        public Nullable<int> KhachHangID { get; set; }
        public string Status { get; set; }
        public string Address { get; set; }
        public Nullable<bool> Payment { get; set; }
        public Nullable<System.DateTime> NgayDat { get; set; }
        public Nullable<System.DateTime> NgayGiao { get; set; }
        public Nullable<double> ThanhTien { get; set; }
        public Nullable<int> TongSoLuong { get; set; }
        public Nullable<int> Id_NV { get; set; }
    
        public virtual KhachHang KhachHang { get; set; }
        public virtual Order_Status Order_Status { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order_Detail> Order_Detail { get; set; }
        public virtual NhanVien NhanVien { get; set; }
    }
    public class DoanhThuViewModel
    {
        public string Thang { get; set; }
        public double TongDoanhThu { get; set; }
    }
}
