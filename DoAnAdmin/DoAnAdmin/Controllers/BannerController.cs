using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnAdmin.Models;
using PagedList;

namespace DoAnAdmin.Controllers
{
    public class BannerController : Controller
    {
        //
        // GET: /Banner/
        QL_LaptopEntities db = new QL_LaptopEntities();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult BannerTopPartial()
        {
            return View(db.Banners.Where(b => b.BannerType.Trim() == "Top" && b.active == true).OrderBy(t=>t.id));
        }
        public ActionResult BannerMidPartial()
        {
            return View(db.Banners.Where(b => b.BannerType.Trim() == "Mid" && b.active == true).OrderBy(t => t.id));
        }
        public ActionResult BannerAdvPartial()
        {
            return View(db.Banners.Where(b => b.BannerType.Trim() == "Adv" && b.active == true).OrderBy(t => t.id));
        }


        //------------------------------Phần admin-----------------------------
        public ActionResult BannerTop(int ? page)
        {
            //1. Tham số int? dùng để thể hiện null và kiểu int
            //Nếu page == null thì gán là mặc định là trang 1
            if (page == null)
                page = 1;
            //Tạo kích thước cho trang (PageSize) hay là số lượng bài hát trên 1 trang
            int pageSize = 10;
            //Nếu page = null thì lấy giá trị 1 cho biến pageNumber
            int pageNumber = (page ?? 1);
            //Lấy dữ liệu từ 3 bảng: Products, Detail, Image
            return View(db.Banners.Where(b => b.BannerType.Trim() == "Top" && b.active == true).OrderBy(t => t.id).ToPagedList(pageNumber, pageSize));
        }
        public ActionResult BannerMid(int? page)
        {
            //1. Tham số int? dùng để thể hiện null và kiểu int
            //Nếu page == null thì gán là mặc định là trang 1
            if (page == null)
                page = 1;
            //Tạo kích thước cho trang (PageSize) hay là số lượng bài hát trên 1 trang
            int pageSize = 10;
            //Nếu page = null thì lấy giá trị 1 cho biến pageNumber
            int pageNumber = (page ?? 1);
            //Lấy dữ liệu từ 3 bảng: Products, Detail, Image
            return View(db.Banners.Where(b => b.BannerType.Trim() == "Mid" && b.active == true).OrderBy(t => t.id).ToPagedList(pageNumber, pageSize));
        }
        public ActionResult BannerAdv(int? page)
        {
            //1. Tham số int? dùng để thể hiện null và kiểu int
            //Nếu page == null thì gán là mặc định là trang 1
            if (page == null)
                page = 1;
            //Tạo kích thước cho trang (PageSize) hay là số lượng bài hát trên 1 trang
            int pageSize = 10;
            //Nếu page = null thì lấy giá trị 1 cho biến pageNumber
            int pageNumber = (page ?? 1);
            //Lấy dữ liệu từ 3 bảng: Products, Detail, Image
            return View(db.Banners.Where(b => b.BannerType.Trim() == "Adv" && b.active == true).OrderBy(t => t.id).ToPagedList(pageNumber, pageSize));
        }
    }
}
