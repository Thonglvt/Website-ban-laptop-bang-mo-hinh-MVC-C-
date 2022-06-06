using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnAdmin.Models;
namespace DoAnAdmin.Controllers
{
    public class DiaChiController : Controller
    {
        //
        // GET: /DiaChi/
        QL_LaptopEntities db = new QL_LaptopEntities();
        public ActionResult DiaChiPartial()
        {
            QL_LaptopEntities db = new QL_LaptopEntities();
            ViewBag.ThanhPhoList = new SelectList(getThanhPhoList(), "MaTinhThanhPho", "TenTinhThanhPho");
            return View();
        }
        public List<tblTinhThanhPho> getThanhPhoList()
        {
            QL_LaptopEntities db = new QL_LaptopEntities();

            List<tblTinhThanhPho> lst = db.tblTinhThanhPhoes.OrderByDescending(t => t.TenTinhThanhPho)
                .ToList();
            return lst;
        }
        public ActionResult getQuanHuyenList(string pMaTP)
        {
            QL_LaptopEntities db = new QL_LaptopEntities();
            List<tblQuanHuyen> lst = db.tblQuanHuyens.Where(t => t.MaTinhThanhPho == pMaTP)
                .OrderByDescending(t => t.TenQuanHuyen)
                .ToList();
            ViewBag.QuanHuyenList = new SelectList(lst, "MaQuanHuyen", "TenQuanHuyen");

            return PartialView("HienThiQuanHuyen");
        }
        public ActionResult getPhuongXaList(string pMaQH)
        {
            QL_LaptopEntities db = new QL_LaptopEntities();
            List<tblPhuongXa> lst = db.tblPhuongXas.Where(t => t.MaQuanHuyen == pMaQH)
                .OrderByDescending(t => t.TenPhuongXa)
                .ToList();
            ViewBag.PhuongXaList = new SelectList(lst, "MaPhuongXa", "TenPhuongXa");

            return PartialView("HienThiPhuongXa");
        }

    }
}
