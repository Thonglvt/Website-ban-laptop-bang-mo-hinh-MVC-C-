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
    
    public partial class tblTinhThanhPho
    {
        public tblTinhThanhPho()
        {
            this.tblQuanHuyens = new HashSet<tblQuanHuyen>();
        }
    
        public string MaTinhThanhPho { get; set; }
        public string TenTinhThanhPho { get; set; }
        public string GhiChu { get; set; }
    
        public virtual ICollection<tblQuanHuyen> tblQuanHuyens { get; set; }
    }
}