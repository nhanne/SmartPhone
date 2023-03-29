﻿//------------------------------------------------------------------------------
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
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    public partial class KhachHang
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public KhachHang()
        {
            this.Orders = new HashSet<Order>();
        }

        [Key, Column(Order = 1)]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int idUser { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Tên")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Vui lòng nhập tên thật")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Họ")]
        [StringLength(50, MinimumLength = 2,ErrorMessage = "Vui lòng nhập họ thật")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Email")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Mật khẩu")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$", ErrorMessage = "Mật khẩu tối thiêu 8 kí tự, bao gồm 1 in hoa, 1 số và 1 ký tự đặc biệt")]
        public string Password { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Vui lòng nhập nhập lại mật khẩu")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Mật khẩu không trùng khớp")]
        public string ConfirmPassword { get; set; }
        public string FullName()
        {
            return this.LastName + " " + this.FirstName;
        }

        public string Picture { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ")]
        public string Address { get; set; }
        //[Required(ErrorMessage = "Vui lòng nhập ngày sinh")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{mm/dd/yyyy}")]
        public Nullable<System.DateTime> NgaySinh { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập số chứng minh thư")]
        public string CMT { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        public string Sdt { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }
    }
    public partial class EditProFile
    {
        [Key, Column(Order = 1)]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int idUser { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Tên")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Vui lòng nhập tên thật")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Họ")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Vui lòng nhập họ thật")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Email")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}")]
        public string Email { get; set; }
        public string FullName()
        {
            return this.LastName + " " + this.FirstName;
        }

        public string Picture { get; set; }
        public string Address { get; set; }
        public Nullable<System.DateTime> NgaySinh { get; set; }
        public string CMT { get; set; }
        public string Sdt { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }
    }
}
