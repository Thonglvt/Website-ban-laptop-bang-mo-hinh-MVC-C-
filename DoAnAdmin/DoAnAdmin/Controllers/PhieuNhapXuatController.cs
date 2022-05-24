using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnAdmin.Models;
using PagedList;

namespace DoAnAdmin.Controllers
{
    public class PhieuNhapXuatController : Controller
    {
        //
        // GET: /PhieuNhapXuat/
        QL_LaptopEntities db = new QL_LaptopEntities();
        HamXuLy xuLy = new HamXuLy();

        public ActionResult Index()
        {
            return View();
        }


        //Phiếu nhập
        public ActionResult PhieuNhap(int? page)
        {
            //1. Tham số int? dùng để thể hiện null và kiểu int
            //Nếu page == null thì gán là mặc định là trang 1
            if (page == null)
                page = 1;
            //Tạo kích thước cho trang (PageSize) hay là số lượng bài hát trên 1 trang
            int pageSize = 15;
            //Nếu page = null thì lấy giá trị 1 cho biến pageNumber
            int pageNumber = (page ?? 1);

            return View(db.PhieuNhaps.OrderBy(t => t.id).ToPagedList(pageNumber, pageSize));
        }
        //Chi tiết Phiếu nhập
        public ActionResult ChiTietPhieuNhap(string id)
        {
            return View(db.ChiTietPhieuNhaps.Where(t => t.PhieuNhap_id == id).OrderBy(t => t.product_id).ToList());
        }
        //Thêm phiếu nhập
        public ActionResult ThemCTPhieuNhap()
        {
            string maPN = xuLy.createIDPhieuNhap();
            Session["id"] = maPN;
            ViewBag.id = maPN;
            //Thêm vào phiếu nhập
            PhieuNhap p = new PhieuNhap();
            p.id = ViewBag.id;
            p.employee_id = "30102021NV000000";//Admin
            p.date_ = DateTime.Now;

            db.PhieuNhaps.Add(p);
            db.SaveChanges();
            //Lấy dữ liệu cho dropdown List
            ViewBag.Prod = new SelectList(db.Products, "id", "Name");
            return View();
        }

        [HttpPost]
        //
        public ActionResult ThemCTPhieuNhap(ChiTietPhieuNhap c, FormCollection f)
        {
            ViewBag.Prod = new SelectList(db.Products, "id", "Name");
            if (xuLy.kiemTraKhoaChinhOnCTPN(f["txtID"].ToString(), c.product_id) == true)
            {
                ViewBag.tb = "Bạn đã nhập sản phẩm của phiếu nhập này rồi";
                return View();
            }
            try
            {
                c.PhieuNhap_id = f["txtID"].ToString();
                c.quanlity = int.Parse(f["txtQuanlity"]);
                c.price_Import = int.Parse(f["txtImport"]);
                c.ratio = int.Parse(f["txtRatio"]);
                db.ChiTietPhieuNhaps.Add(c);
                db.SaveChanges();

                ViewBag.tb = "Thêm thành công";
            }
            catch (Exception)
            {
                ViewBag.tb = "Thêm thất bại";
            }
            return View();
        }
        public ActionResult HoaDonKH(string maHD)
        {
            ViewBag.maHD = maHD;
            Session["MaHD"] = maHD;
            var px = db.PhieuXuats.Single(t => t.id == maHD);
            ViewBag.total = px.total;
            ViewBag.address = px.Address_;
            ViewBag.name = px.Customer.name;
            ViewBag.phone = px.Customer.phone;

            Session["total"] = px.total;
            Session["address"] = px.Address_;
            Session["name"] = px.Customer.name;
            Session["phone"] = px.Customer.phone;

            return View(db.ChiTietPhieuXuats.Where(p=>p.phieuXuat_id == maHD).ToList());
        }
        [HttpPost]
        public ActionResult HoaDonKH(FormCollection f)
        {
            string maHD = (Session["MaHD"] != null) ? Session["MaHD"].ToString() : "";
            ViewBag.maHD = maHD;
            var px = db.PhieuXuats.Single(t => t.id == maHD);
            ViewBag.total = px.total;

            var lstHD = db.ChiTietPhieuXuats.Where(p => p.phieuXuat_id == maHD).ToList();
            //Kiểm tra Input
            if (string.IsNullOrEmpty(f["txtHoTen"]))
            {
                ViewBag.tbHoTen = "Vui lòng cho biết họ tên của bạn";
                return View(db.ChiTietPhieuXuats.Where(p => p.phieuXuat_id == maHD).ToList());
            }
            if (string.IsNullOrEmpty(f["txtSdt"]))
            {
                ViewBag.tbSDT = "Vui lòng nhập số điện thoại";
                return View(lstHD);
            }
            if (string.IsNullOrEmpty(f["txtAddress"]))
            {
                ViewBag.tbDiaChi = "Vui lòng nhập địa chỉ khi nhận hàng";
                return View(lstHD);
            }
            string phoneNew = f["txtSdt"].ToString();
            string nameNew = f["txtHoTen"].ToString();
            string addressNew = f["txtAddress"].ToString();

            //Kiểm tra sdt mới có trùng với các khách hàng còn lại hay không
            string sdtOld = (Session["phone"] != null) ? Session["phone"].ToString() : "";
            var customerKT = db.Customers.SingleOrDefault(t => t.id != px.customer_id && t.phone == phoneNew);
            if (customerKT != null)
            {
                ViewBag.tbSDT = "Số điện thoại đã tồn tại!";
                return View(lstHD);
            }
            else
            {
                Customer cusUpd = db.Customers.Single(c => c.id == px.customer_id);
                cusUpd.phone = phoneNew;
                cusUpd.name = nameNew;
                cusUpd.Address = addressNew;

                db.SaveChanges();
                ViewBag.tbThanhCong = "Cập nhật thành công";
            }
            ViewBag.address = addressNew;
            ViewBag.name = nameNew;
            ViewBag.phone = phoneNew;
            //px.customer_id = IDKH;
            return View(db.ChiTietPhieuXuats.Where(p => p.phieuXuat_id == maHD).ToList());
        }

        ////Phiếu xuất
        public ActionResult PhieuXuat(int? page)
        {
            //1. Tham số int? dùng để thể hiện null và kiểu int
            //Nếu page == null thì gán là mặc định là trang 1
            if (page == null)
                page = 1;
            //Tạo kích thước cho trang (PageSize) hay là số lượng bài hát trên 1 trang
            int pageSize = 15;
            //Nếu page = null thì lấy giá trị 1 cho biến pageNumber
            int pageNumber = (page ?? 1);

            return View(db.PhieuXuats.OrderByDescending(t => t.date_).ToPagedList(pageNumber, pageSize));
        }
        [HttpPost]
        public ActionResult PhieuXuat(int? page, FormCollection f)
        {
            //1. Tham số int? dùng để thể hiện null và kiểu int
            //Nếu page == null thì gán là mặc định là trang 1
            if (page == null)
                page = 1;
            //Tạo kích thước cho trang (PageSize) hay là số lượng bài hát trên 1 trang
            int pageSize = 15;
            //Nếu page = null thì lấy giá trị 1 cho biến pageNumber
            int pageNumber = (page ?? 1);

            string str = f["txtDateS"].ToString();
            if (string.IsNullOrEmpty(f["txtSearch"]) && (string.IsNullOrEmpty(f["txtDateS"]) || string.IsNullOrEmpty(f["txtDateE"])))
            {
                return View(db.PhieuXuats.OrderByDescending(t => t.date_).ToPagedList(pageNumber, pageSize));
            }
            if (string.IsNullOrEmpty(f["txtSearch"]))
            {
                DateTime dateS = DateTime.Parse(f["txtDateS"]);
                DateTime dateE = DateTime.Parse(f["txtDateE"]);

                return RedirectToAction("SearchHoaDonByDate", "PhieuNhapXuat", new { dateS = dateS, dateE = dateE });
            }
            string strS = f["txtSearch"];
            return RedirectToAction("SearchHoaDon", "PhieuNhapXuat", new { strS = strS });
        }
        public ActionResult SearchHoaDon(string strS)
        {
            return View(db.PhieuXuats.Where(t => t.id.Contains(strS) || t.employee_id.Contains(strS)
                || t.Employee.name.Contains(strS) || t.customer_id.Contains(strS)
                || t.Customer.name.Contains(strS) || t.Customer.phone.Contains(strS)
                || t.employee_id.Contains(strS)).OrderByDescending(t => t.date_).ToList());
        }
        [HttpPost]
        public ActionResult SearchHoaDon(int? page, FormCollection f)
        {
            if (page == null)
                page = 1;

            string str = f["txtDateS"].ToString();
            if (string.IsNullOrEmpty(f["txtSearch"]) && (string.IsNullOrEmpty(f["txtDateS"]) || string.IsNullOrEmpty(f["txtDateE"])))
            {
                return RedirectToAction("PhieuXuat", "PhieuNhapXuat", new { page = page });
            }
            if (string.IsNullOrEmpty(f["txtSearch"]))
            {
                DateTime dateS = DateTime.Parse(f["txtDateS"]);
                DateTime dateE = DateTime.Parse(f["txtDateE"]);

                return View(db.PhieuXuats.Where(t => t.date_ >= dateS && t.date_ <= dateE).OrderByDescending(t => t.date_).ToList());
            }
            string strS = f["txtSearch"];
            return RedirectToAction("SearchHoaDon", "PhieuNhapXuat", new { strS = strS });
        }
        public ActionResult SearchHoaDonByDate(DateTime dateS, DateTime dateE)
        {
            return View(db.PhieuXuats.Where(t => t.date_ >= dateS && t.date_ <= dateE).OrderByDescending(t => t.date_));
        }
        [HttpPost]
        public ActionResult SearchHoaDonByDate(int? page, FormCollection f)
        {
            if (page == null)
                page = 1;

            string str = f["txtDateS"].ToString();
            if (string.IsNullOrEmpty(f["txtSearch"]) && (string.IsNullOrEmpty(f["txtDateS"]) || string.IsNullOrEmpty(f["txtDateE"])))
            {
                return RedirectToAction("PhieuXuat", "PhieuNhapXuat", new { page = page });
            }
            if (string.IsNullOrEmpty(f["txtSearch"]))
            {
                DateTime dateS = DateTime.Parse(f["txtDateS"]);
                DateTime dateE = DateTime.Parse(f["txtDateE"]);

                return View(db.PhieuXuats.Where(t => t.date_ >= dateS && t.date_ <= dateE).OrderByDescending(t => t.date_).ToList());
            }
            string strS = f["txtSearch"];
            return RedirectToAction("SearchHoaDon", "PhieuNhapXuat", new { strS = strS });
        }

        public ActionResult XemChiTietDonhang(string maHD)
        {
            return View(db.ChiTietPhieuXuats.Where(p => p.phieuXuat_id == maHD).OrderBy(t => t.price).ToList());
        }

        public ActionResult DaThanhToan(string maHD)
        {
            int page = 1;
            PhieuXuat px = db.PhieuXuats.SingleOrDefault(t => t.id == maHD);
            if(px!=null)
            {
                px.status = 1;
                db.SaveChanges();
            }
            return RedirectToAction("PhieuXuat", "PhieuNhapXuat", new { page = page });
        }

    }
}
