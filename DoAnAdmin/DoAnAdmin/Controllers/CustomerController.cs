using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnAdmin.Models;
using PagedList;

namespace DoAnAdmin.Controllers
{
    public class CustomerController : Controller
    {
        //
        // GET: /Customer/
        QL_LaptopEntities db = new QL_LaptopEntities();

        public ActionResult Index()
        {
            return View();
        }

        //Khách hàng
        public ActionResult Customer()
        {

            return View(db.ChiTietPhieuNhaps.ToList());
        }

        //Thanh toán
        public ActionResult ThanhToan(string maSP)
        {
            var promPrice = db.promotions.SingleOrDefault(t => t.product_id == maSP && (DateTime.Now >= t.date_start && DateTime.Now <= t.date_end));
            ViewBag.IsKhuyenMai = (promPrice == null) ? 0 : 1;

            promotion pro = db.promotions.SingleOrDefault(p => p.product_id == maSP);
            ViewBag.maSP = maSP;
            return View(pro);
        }

        //Phần Admin
        //Hiển thị khách hàng
        public ActionResult CustomerManager(int? page)
        {
            //1. Tham số int? dùng để thể hiện null và kiểu int
            //Nếu page == null thì gán là mặc định là trang 1
            if (page == null)
                page = 1;
            //Tạo kích thước cho trang (PageSize) hay là số lượng bài hát trên 1 trang
            int pageSize = 20;
            //Nếu page = null thì lấy giá trị 1 cho biến pageNumber
            int pageNumber = (page ?? 1);

            return View(db.Customers.OrderBy(t => t.id).ToPagedList(pageNumber, pageSize));
        }
    }
}

