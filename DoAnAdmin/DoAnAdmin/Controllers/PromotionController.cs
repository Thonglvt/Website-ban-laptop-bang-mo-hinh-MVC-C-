using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnAdmin.Models;
using PagedList;

namespace DoAnAdmin.Controllers
{
    public class PromotionController : Controller
    {
        //
        // GET: /Promotion/
        QL_LaptopEntities db = new QL_LaptopEntities();
        HamXuLy xuLy = new HamXuLy();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult PromotionPrice(int ? page)
        {
            //1. Tham số int? dùng để thể hiện null và kiểu int
            //Nếu page == null thì gán là mặc định là trang 1
            if (page == null)
                page = 1;
            //Tạo kích thước cho trang (PageSize) hay là số lượng bài hát trên 1 trang
            int pageSize = 20;
            //Nếu page = null thì lấy giá trị 1 cho biến pageNumber
            int pageNumber = (page ?? 1);

            return View(db.promotions.OrderByDescending(t => t.date_end).ToPagedList(pageNumber, pageSize));
        }
        [HttpPost]
        public ActionResult PromotionPrice(int ? page, FormCollection f)
        {
            //1. Tham số int? dùng để thể hiện null và kiểu int
            //Nếu page == null thì gán là mặc định là trang 1
            if (page == null)
                page = 1;
            //Tạo kích thước cho trang (PageSize) hay là số lượng bài hát trên 1 trang
            int pageSize = 20;
            //Nếu page = null thì lấy giá trị 1 cho biến pageNumber
            int pageNumber = (page ?? 1);
            string strS = f["txtSearch"];
            return View(db.promotions.Where(t => t.product_id.Contains(strS)).OrderByDescending(t => t.date_start).ToPagedList(pageNumber, pageSize));
        }
        //Thêm mới khuyến mãi sản phẩm
        public ActionResult AddPromotionPrice ()
        {
            ViewBag.ProdID = new SelectList(db.Products, "id", "id");
            return View();
        }
        [HttpPost]
        public ActionResult AddPromotionPrice(promotion p, FormCollection f)
        {
            ViewBag.ProdID = new SelectList(db.Products, "id", "id");
            //Kiểm tra input
            if (string.IsNullOrEmpty(p.date_start.ToString()) == true)
            { 
                ViewBag.tbDateS = "Vui lòng nhập ngày bắt đầu khuyến mãi!";
                return View();
            }
            if (string.IsNullOrEmpty(p.date_end.ToString()) == true)
            { 
                ViewBag.tbDateE = "Vui lòng nhập ngày kết thúc!";
                return View();
            }
            if (p.price.ToString().Trim() == string.Empty)
            {
                ViewBag.tbPrice = "Giá khuyến mãi không được để trống";
                return View();
            }
            string dateS = String.Format("{0:MM/dd/yyyy}", p.date_start);
            string today = String.Format("{0:MM/dd/yyyy}", DateTime.Now);
            //Kiểm tra ngày bắt đầu phải >= today
            if (xuLy.checkDate1AndDate2(today, dateS) == false)
            {
                ViewBag.tbChkDateSByToday = "Ngày bắt đầu khuyến mãi phải từ ngày hôm nay trở đi!";
                return View();
            }
            if (p.date_end <= p.date_start)
            {
                ViewBag.tbChkDateEByDateS = "Ngày kết thúc phải lớn hơn ngày bắt đầu khuyến mãi!";
                return View();
            }
            //Kiểm tra giá khuyến mãi không được lớn hơn giá bán
            var prod = db.Products.Single(t => t.id == p.product_id);
            if(p.price > prod.price)
            {
                ViewBag.tbChkPriceByPrProd = "Giá khuyến mãi không được lớn hơn giá bán của sản phẩm";
                return View();
            }

            var pr = db.promotions.SingleOrDefault(t => t.product_id == p.product_id && (p.date_start >= t.date_start && p.date_start <= t.date_end));
            if (pr != null)
            {
                ViewBag.tbPriKey = "Sản phẩm này đang trong quá trình khuyến mãi!";
                return View();
            }
            if (ModelState.IsValid)
            {
                db.promotions.Add(p);
                db.SaveChanges();
                ViewBag.tb = "Thêm thành công";
                return RedirectToAction("AddPromotionPrice", "Promotion");
            }
            return View();
        }
        //Cập nhật khuyến mãi sản phẩm
        public ActionResult UpdatePromotionPrice(string maSP, DateTime dateS, DateTime dateE, int price)
        {
            Session["maSP"] = maSP;
            Session["dateS"] = dateS;
            Session["dateE"] = dateE;
            Session["price"] = price;

            return View();
        }
        [HttpPost]
        public ActionResult UpdatePromotionPrice(FormCollection f)
        {
            if (string.IsNullOrEmpty(f["txtDateE"]) == true)
            {
                ViewBag.tbDateE = "Vui lòng nhập ngày kết thúc!";
                return View();
            }
            if (f["txtPrice"].ToString().Trim() == string.Empty)
            {
                ViewBag.tbPrice = "Giá khuyến mãi không được để trống";
                return View();
            }
            string maSP = "";
            DateTime dateS = new DateTime();
            if (Session["maSP"] != null && Session["dateS"]!= null)
            { 
                maSP = Session["maSP"].ToString();
                dateS = (DateTime)Session["dateS"];
            }
            //Kiểm tra độ dài của giá
            if(f["txtPrice"].ToString().Length>19)
            {
                ViewBag.tbPrice = "Độ dài vượt quá yêu cầu! (Tối đa 19 kí tự)";
                return View();
            }
            //Kiểm tra giá khuyến mãi không được lớn hơn giá bán
            var prod = db.Products.Single(t => t.id == maSP);
            if (Convert.ToDecimal(f["txtPrice"]) > prod.price)
            {
                ViewBag.tbChkPriceByPrProd = "Giá khuyến mãi không được lớn hơn giá bán của sản phẩm! (" + string.Format("{0:0,0}", prod.price) + "đ)";
                return View();
            }

            promotion pr = db.promotions.Single(p => p.product_id == maSP && p.date_start == dateS);
            pr.date_end = Convert.ToDateTime(f["txtDateE"]);
            pr.price = Convert.ToDecimal(f["txtPrice"]);
            db.SaveChanges();

            ViewBag.tb = "Cập nhật thành công";
            return View();
        }
        public ActionResult DeletePromotionPrice(string maSP, DateTime dateS)
        {
            promotion prom = db.promotions.Single(t => t.product_id == maSP && t.date_start == dateS);
            db.promotions.Remove(prom);
            db.SaveChanges();
            return RedirectToAction("PromotionPrice", "Promotion", new { page = 1 });
        }
        //----Qùa khuyến mãi
        public ActionResult PromotionGift(int ? page)
        {
            //1. Tham số int? dùng để thể hiện null và kiểu int
            //Nếu page == null thì gán là mặc định là trang 1
            if (page == null)
                page = 1;
            //Tạo kích thước cho trang (PageSize) hay là số lượng bài hát trên 1 trang
            int pageSize = 20;
            //Nếu page = null thì lấy giá trị 1 cho biến pageNumber
            int pageNumber = (page ?? 1);

            return View(db.PromotionsGifts.OrderByDescending(t => t.date_start).ToPagedList(pageNumber, pageSize));
        }

    }
}
