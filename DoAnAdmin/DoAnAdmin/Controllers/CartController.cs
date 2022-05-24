using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnAdmin.Models;

namespace DoAnAdmin.Controllers
{
    public class CartController : Controller
    {
        //
        // GET: /Cart/
        HamXuLy xuLy = new HamXuLy();
        QL_LaptopEntities db = new QL_LaptopEntities();

        public ActionResult EmptyCart()
        {
            return View();
        }

        public List<Cart> LayGioHang()
        {
            List<Cart> lstGioHang = Session["GioHang"] as List<Cart>;
            if (lstGioHang == null)
            {
                //neu lstgiohang chua ton tai thi khoi tao
                lstGioHang = new List<Cart>();
                Session["GioHang"] = lstGioHang;
            }
            return lstGioHang;
        }
        //them vao gio hang
        public ActionResult ThemGioHang(string idsp, string strURL)
        {
            ///Lay ra gio hang
            List<Cart> lstCart = LayGioHang();
            //kiem tra sp nay da ton tai trong session[GioHang] chuaw
            Cart SanPham = lstCart.Find(sp => sp.sIdSP == idsp);
            if (SanPham == null)//chua co trong gio
            {
                SanPham = new Cart(idsp);
                lstCart.Add(SanPham);
                return Redirect(strURL);
            }
            else  ///da co san pham nay trong gio
            {
                SanPham.iSoLuong++;
                return Redirect(strURL);
            }
        }
        ///Tong so luong
        private int TongSoLuong()
        {
            int tsl = 0;
            List<Cart> lstGioHang = Session["GioHang"] as List<Cart>;
            if (lstGioHang != null)
            {
                tsl = lstGioHang.Sum(sp => sp.iSoLuong);
            }
            return tsl;
        }

        ///Tong thanh tien
        private decimal TongThanhTien()
        {
            decimal ttt = 0;
            List<Cart> lstGioHang = Session["GioHang"] as List<Cart>;
            if (lstGioHang != null)
            {
                ttt += lstGioHang.Sum(sp => sp.dThanhTien);
            }
            return ttt;

        }
        //trang gio hang
        public ActionResult Cart()
        {
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("EmptyCart", "Cart");
            }
            List<Cart> lstGioHang = LayGioHang();

            //neu gio hang rong
            if (lstGioHang.Count == 0)
            {
                return RedirectToAction("EmptyCart", "Cart");
            }

            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongThanhTien = TongThanhTien();

            return View(lstGioHang);
        }
        [HttpPost]//Khi KH Đặt hàng
        public ActionResult Cart(FormCollection f)
        {
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("EmptyCart", "Cart");
            }
            List<Cart> lstGioHang = LayGioHang();

            //neu gio hang rong
            if (lstGioHang.Count == 0)
            {
                return RedirectToAction("EmptyCart", "Cart");
            }

            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongThanhTien = TongThanhTien();
            try
            {
                //Kiểm tra Input
                if (string.IsNullOrEmpty(f["txtHoTen"]))
                {
                    ViewBag.tbHoTen = "Vui lòng cho biết họ tên của bạn";
                    return View(lstGioHang);
                }
                if (string.IsNullOrEmpty(f["txtSdt"]))
                {
                    ViewBag.tbSDT = "Vui lòng nhập số điện thoại";
                    return View(lstGioHang);
                }
                if (string.IsNullOrEmpty(f["txtAddress"]))
                {
                    ViewBag.tbDiaChi = "Vui lòng nhập địa chỉ khi nhận hàng";
                    return View(lstGioHang);
                }
                //kiểm tra số lượng tồn
                if (Session["GioHang"] != null)
                {
                    List<Cart> lstCart = Session["GioHang"] as List<Cart>;
                    string phone = f["txtSdt"].ToString();
                    string name = f["txtHoTen"].ToString();
                    string address = f["txtAddress"].ToString();
                    string maPX = xuLy.createIDPhieuXuat();
                    string emplID = "30102021NV000000";
                    PhieuXuat px = new PhieuXuat();
                    px.id = maPX;
                    px.employee_id = emplID;
                    px.date_ = DateTime.Now;
                    px.Address_ = address;
                    px.total = 0;
                    //Kiểm tra nếu sđt khách hàng chưa có trong bảng KhachHang thì thêm mới KH
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
                    foreach (Cart c in lstCart)
                    {
                        ChiTietPhieuXuat ctpx = new ChiTietPhieuXuat();
                        ctpx.phieuXuat_id = maPX;
                        ctpx.product_id = c.sIdSP;
                        ctpx.quanlity = c.iSoLuong;
                        ctpx.price = c.dThanhTien;
                        db.ChiTietPhieuXuats.Add(ctpx);
                        db.SaveChanges();
                    }
                    return RedirectToAction("HoaDonKH", "PhieuNhapXuat", new { maHD = maPX });
                }
            }
            catch (Exception)
            {
                return View(lstGioHang);
            }
            return View(lstGioHang);
        }
        public ActionResult CartPartial()
        {
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongThanhTien = TongThanhTien();
            return PartialView();
        }
        ///xoa gio hang
        public ActionResult XoaGiohang(string MaSP)
        {
            //Lay gio hang
            List<Cart> lstGioHang = LayGioHang();
            ///kiem tra xem sach can xoa co trong gio hang khong
            Cart sp = lstGioHang.Single(s => s.sIdSP == MaSP);

            //neu co thi tien hanh xoa
            if (sp != null)
            {
                lstGioHang.RemoveAll(s => s.sIdSP == MaSP);
                //neu gio hang rong
                if (lstGioHang.Count == 0)
                {
                    return RedirectToAction("EmptyCart", "Cart");
                }
                return RedirectToAction("Cart", "Cart");
            }
            return RedirectToAction("Cart", "Cart");
        }

        //xoa toan bo gio 
        public ActionResult XoaGioHang_All()
        {
            //Lay gio hang
            List<Cart> lstGioHang = LayGioHang();
            lstGioHang.Clear();
            return RedirectToAction("EmptyCart", "Cart");
        }

        ///cap nhat gio hang 
        public ActionResult CapNhatGioHang(string
 MaSP, FormCollection f)
        {
            List<Cart> lstGioHang = LayGioHang();
            ///kiem tra sach can cap nhat co trong gio hang hay khong
            Cart sp = lstGioHang.Single(s => s.sIdSP == MaSP);
            //Neu co tien hanh cap nhat
            
            if (sp != null)
            {
                try
                {
                    sp.iSoLuong = int.Parse(f["txtSoLuong"].ToString());
                    if (Session["tbTxtSoLuong"] != null)
                    {
                        Session["tbTxtSoLuong"] = null;
                    }
                }
                catch (Exception)
                {
                    Session["tbTxtSoLuong"] = "Vui lòng nhập số";
                    return RedirectToAction("Cart", "Cart");
                }
               
            }
            return RedirectToAction("Cart", "Cart");
        }


        public ActionResult DatHang(FormCollection f)
        {
            try
            {
                //Kiểm tra Input
                if (string.IsNullOrEmpty(f["txtHoTen"]))
                {
                    ViewBag.tbHoTen = "Vui lòng cho biết họ tên của bạn";
                    return RedirectToAction("Cart", "Cart");
                }
                if (string.IsNullOrEmpty(f["txtSdt"]))
                {
                    ViewBag.tbSDT = "Vui lòng nhập số điện thoại";
                    return RedirectToAction("Cart", "Cart");
                }
                if (string.IsNullOrEmpty(f["txtAddress"]))
                {
                    ViewBag.tbDiaChi = "Vui lòng nhập địa chỉ khi nhận hàng";
                    return RedirectToAction("Cart", "Cart");
                }

                if (Session["GioHang"] != null)
                {
                    List<Cart> lstCart = Session["GioHang"] as List<Cart>;
                    string phone = f["txtSdt"].ToString();
                    string name = f["txtHoTen"].ToString();
                    string address = f["txtAddress"].ToString();
                    string maPX = xuLy.createIDPhieuXuat();
                    string emplID = "30102021NV000000";
                    PhieuXuat px = new PhieuXuat();
                    px.id = maPX;
                    px.employee_id = emplID;
                    px.date_ = DateTime.Now;
                    px.Address_ = address;

                    //Kiểm tra nếu sđt khách hàng chưa có trong bảng KhachHang thì thêm mới KH
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
                    foreach (Cart c in lstCart)
                    {
                        ChiTietPhieuXuat ctpx = new ChiTietPhieuXuat();
                        ctpx.phieuXuat_id = maPX;
                        ctpx.product_id = c.sIdSP;
                        ctpx.quanlity = c.iSoLuong;
                        ctpx.price = c.dThanhTien;
                        db.ChiTietPhieuXuats.Add(ctpx);
                        db.SaveChanges();
                    }
                    return RedirectToAction("HoaDonKH", "PhieuNhapXuat", new { maHD = maPX });
                }  
            }
            catch (Exception)
            {
                return RedirectToAction("Cart", "Cart");
            }
            return RedirectToAction("Cart", "Cart");
        }
    }
}
