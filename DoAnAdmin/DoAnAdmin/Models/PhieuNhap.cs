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
    
    public partial class PhieuNhap
    {
        public PhieuNhap()
        {
            this.ChiTietPhieuNhaps = new HashSet<ChiTietPhieuNhap>();
        }
    
        public string id { get; set; }
        public string employee_id { get; set; }
        public Nullable<System.DateTime> date_ { get; set; }
    
        public virtual ICollection<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
