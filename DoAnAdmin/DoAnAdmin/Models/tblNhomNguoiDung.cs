//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DoAnAdmin.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblNhomNguoiDung
    {
        public tblNhomNguoiDung()
        {
            this.tblNguoiDungNhomNguoiDungs = new HashSet<tblNguoiDungNhomNguoiDung>();
            this.tblPhanQuyens = new HashSet<tblPhanQuyen>();
        }
    
        public string MaNhom { get; set; }
        public string TenNhom { get; set; }
        public string GhiChu { get; set; }
    
        public virtual ICollection<tblNguoiDungNhomNguoiDung> tblNguoiDungNhomNguoiDungs { get; set; }
        public virtual ICollection<tblPhanQuyen> tblPhanQuyens { get; set; }
    }
}
