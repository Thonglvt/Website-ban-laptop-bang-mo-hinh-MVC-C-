using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnAdmin.Models;
using PagedList;

namespace DoAnAdmin.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /User/
        QL_LaptopEntities db = new QL_LaptopEntities();
        HamXuLy xuLy = new HamXuLy();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DangNhap()
        {
            return View();
        }
        public ActionResult LoginSuccessPartial()
        {
            return View();
        }
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(FormCollection f)
        {
            string phone = f["username"];
            //
            var acc = db.Accounts.SingleOrDefault(a => a.username == phone);
            if(acc!=null)
            {
                ViewBag.tbAcc = "Số điện thoại đã tồn tại";
                return View();
            }
            string name = f["name"];
            
            string pass = f["password"];
            string rePass = f["repassword"];
            string address = f["address"];
            string email = f["email"];
            string id = "";
            if(pass.Equals(rePass) == false)
            {
                ViewBag.tbPass = "Nhập lại mật khẩu không chính xác";
                return View();
            }
            Account accNew = new Account();
            accNew.username = phone;
            accNew.password = pass;
            accNew.account_type = 2;

            db.Accounts.Add(accNew);
            db.SaveChanges();
            //Kiểm tra nếu sđt khách hàng chưa có tên trong bảng KhachHang thì thêm mới KH
            var customer = db.Customers.SingleOrDefault(t => t.phone == phone);
            if (customer == null)
            {
                string IDKH = xuLy.createIDKH();
                string gender = "Nam";
                Customer cus = new Customer();
                cus.id = IDKH;
                cus.name = name;
                cus.phone = phone;
                cus.Address = address;
                cus.gender = gender;
                cus.email = email;
                cus.cmnd = "No";
                cus.username = phone;
                db.Customers.Add(cus);
                db.SaveChanges();
            }
            else
            {
                customer.username = phone;
                db.SaveChanges();
            }
            

            return RedirectToAction("DangNhap", "User");
        }
        public ActionResult DangXuat()
        {
            Session["user"] = null;
            return RedirectToAction("ShowAllProducts", "Product");
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection f)
        {
            var tendn = f["username"];
            var matkhau = f["password"];

            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "* Nhập sai tên tài khoản";
            }
            if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "* Nhập sai mật khẩu";
            }

            if (!String.IsNullOrEmpty(tendn) && !String.IsNullOrEmpty(matkhau))
            {
                Account acc = db.Accounts.SingleOrDefault(c => c.username.Trim() == tendn.Trim() && c.password.Trim() == matkhau.Trim());
                if (acc != null)
                {
                    //Kiểm tra account của nhân viên hay khách hàng
                    if (acc.account_type == 1)//Điều hướng đến trang admin
                    {
                        Session["userAdmin"] = acc;
                        return RedirectToAction("Home", "Admin");
                    }
                    else
                    {
                        Session["user"] = acc;
                        //Điều hướng về trnag chủ
                        return RedirectToAction("ShowAllProducts", "Product");
                    }
                }
                else
                {
                    ViewBag.TB = "X Sai tên đăng nhập hoặc mật khẩu, Vui lòng nhập lại !";
                }
            }
            return View();
        }
        public ActionResult ThongTin()
        {
            Account acc = Session["user"] as Account;
            var cust = db.Customers.SingleOrDefault(t => t.username == acc.username);

            return View(cust);
        }
        //Thanh toán
        public ActionResult ThanhToanNgay(string maSP)
        {//
            var product = db.Products.SingleOrDefault(t => t.id == maSP);
            var promPrice = db.promotions.SingleOrDefault(t => t.product_id == maSP && (DateTime.Now >= t.date_start && DateTime.Now <= t.date_end));
            if (promPrice != null)
            {
                ViewBag.IsKhuyenMai = promPrice.price_after;
            }
            else
            {
                ViewBag.IsKhuyenMai = product.price;
            }
            //ViewBag.IsKhuyenMai = (promPrice == null) ? promPrice.price_after : null;

            Session["maSPTT"] = maSP;
            promotion pro = db.promotions.SingleOrDefault(p => p.product_id == maSP);
            ViewBag.maSP = maSP;
            return View(product);
        }
        [HttpPost]
        public ActionResult ThanhToanNgay(FormCollection f)
        {
            string maSP = "";
            if (Session["maSPTT"] != null)
                maSP = Session["maSPTT"].ToString();
            var promPrice = db.promotions.SingleOrDefault(t => t.product_id == maSP && (DateTime.Now >= t.date_start && DateTime.Now <= t.date_end));
                
            promotion pro = db.promotions.SingleOrDefault(p => p.product_id == maSP);
            Product product = db.Products.FirstOrDefault(p => p.id == maSP);
            
            ViewBag.maSP = maSP;
            if (promPrice != null)
            {
                ViewBag.IsKhuyenMai = promPrice.price_after;
            }
            else
            {
                ViewBag.IsKhuyenMai = product.price;
            }
            ViewBag.hoTenDaNhap = (string.IsNullOrEmpty(f["txtHoTen"])) ? "" : f["txtHoTen"].ToString();
            ViewBag.sDTDaNhap = (string.IsNullOrEmpty(f["txtSdt"])) ? "" : f["txtSdt"].ToString();
            ViewBag.addressDaNhap = (string.IsNullOrEmpty(f["txtAddress"])) ? "" : f["txtAddress"].ToString();

            //Kiểm tra Input
            if (string.IsNullOrEmpty(f["txtHoTen"]))
            {
                ViewBag.tbHoTen = "Vui lòng cho biết họ tên của bạn";
                return View(product);
            }
            else if(string.IsNullOrEmpty(f["txtSdt"]))
            {
                ViewBag.tbSDT = "Vui lòng nhập số điện thoại";
                return View(product);
            }
            else if (string.IsNullOrEmpty(f["txtAddress"]))
            {
                ViewBag.tbDiaChi = "Vui lòng nhập địa chỉ giao hàng";
                return View(product);
            }
            string phone = f["txtSdt"].ToString();
            string name = f["txtHoTen"].ToString();
            string address = f["txtAddress"].ToString();
            //Kiểm tra khi khách hàng chưa đăng nhập

            string maPX = xuLy.createIDPhieuXuat();
            string emplID = "30102021NV000000";

            PhieuXuat px = new PhieuXuat();
            px.id = maPX;
            px.employee_id = emplID;
            px.date_ = DateTime.Now;
            px.Address_ = address;
            px.status = 0;
            //Kiểm tra nếu sđt khách hàng chưa có tên trong bảng KhachHang thì thêm mới KH
            var customer = db.Customers.SingleOrDefault(t => t.phone == phone);
            if (customer == null)
            {
                string IDKH = xuLy.createIDKH();
                string gender = f["optRadioGender"].ToString();
                Customer cus = new Customer();
                cus.id = IDKH;
                cus.name = name;
                cus.phone = phone;
                cus.Address = address;
                cus.gender = gender;
                cus.email = "No";
                cus.cmnd = "No";
                db.Customers.Add(cus);
                db.SaveChanges();

                px.customer_id = IDKH;

            }
            else//
            {
                string IDKH = customer.id;
                px.customer_id = IDKH;
            }
            //Thêm Phiếu xuất
            db.PhieuXuats.Add(px);
            db.SaveChanges();
            //Thêm chiTietPX
            ChiTietPhieuXuat ctpx = new ChiTietPhieuXuat();
            ctpx.phieuXuat_id = maPX;
            ctpx.product_id = maSP;
            ctpx.quanlity = 1;
            if(promPrice != null)
            {
                ViewBag.IsKhuyenMai = promPrice.price_after;
                ctpx.price = promPrice.price_after;
            }
            else
            {
                ctpx.price = product.price;
            }    
            db.ChiTietPhieuXuats.Add(ctpx);
            db.SaveChanges();

            return RedirectToAction("HoaDonKH", "PhieuNhapXuat", new { maHD = maPX});
        }
        //optRadioGender
        //Thanh toán
        public ActionResult ThanhToanGioHang(string maSP)
        {
            var promPrice = db.promotions.SingleOrDefault(t => t.product_id == maSP && (DateTime.Now >= t.date_start && DateTime.Now <= t.date_end));
            ViewBag.IsKhuyenMai = promPrice.price_after;

            Session["maSPTT"] = maSP;
            promotion pro = db.promotions.SingleOrDefault(p => p.product_id == maSP);
            ViewBag.maSP = maSP;
            return View(pro);
        }

        
    }
}
