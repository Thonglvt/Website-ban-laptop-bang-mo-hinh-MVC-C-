using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAnAdmin.Models
{
    public class Cart
    {
        QL_LaptopEntities db = new QL_LaptopEntities();

        public string sIdSP { set; get; }
        public string sNameSP { set; get; }
        public string sImageSP { set; get; }
        public decimal dDetailPrice { set; get; }
        public int iSoLuong { set; get; }
        public decimal dThanhTien
        {
            get { return iSoLuong * dDetailPrice; }
        }
        public Cart(string ID_SP)
        {
            sIdSP = ID_SP;
            Product sp = db.Products.Single(s => s.id == sIdSP);
            sNameSP = sp.Name;
            sImageSP = sp.avatar;

            var prom = db.promotions.SingleOrDefault(t => t.product_id == ID_SP && t.date_end >= DateTime.Now);
            //Nếu ko có khuyến mãi thì lấy giá gốc
            dDetailPrice = (prom == null) ? Convert.ToDecimal(sp.price) : Convert.ToDecimal(prom.price_after);
            iSoLuong = 1;
        }
    }
}