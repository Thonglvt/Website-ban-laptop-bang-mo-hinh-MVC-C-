using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnAdmin.Models;
using PagedList;

namespace DoAnAdmin.Controllers
{
    public class ImageController : Controller
    {
        //
        // GET: /Image/
        QL_LaptopEntities db = new QL_LaptopEntities();
        HamXuLy xuLy = new HamXuLy();

        public ActionResult Index()
        {
            return View();
        }


        //Hình ảnh
        public ActionResult ImagesProduct(int? page)
        {
            //1. Tham số int? dùng để thể hiện null và kiểu int
            //Nếu page == null thì gán là mặc định là trang 1
            if (page == null)
                page = 1;
            //Tạo kích thước cho trang (PageSize) hay là số lượng bài hát trên 1 trang
            int pageSize = 8;
            //Nếu page = null thì lấy giá trị 1 cho biến pageNumber
            int pageNumber = (page ?? 1);

            return View(db.Images.OrderBy(t => t.product_id).ToPagedList(pageNumber, pageSize));
        }
        //Hình ảnh
        public ActionResult AddImage()
        {
            ViewBag.ProdID = new SelectList(db.Products, "id", "id");
            return View();
        }
        [HttpPost]
        public ActionResult AddImage(Image img)
        {
            ViewBag.ProdID = new SelectList(db.Products, "id", "id");
            img.dateUpdate = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.Images.Add(img);
                db.SaveChanges();
                ViewBag.tb = 1;
                //return RedirectToAction("Images", "Image");
                return View();
            }
            ViewBag.tb = 0;
            return View();
        }

        public ActionResult UpdateImage(string prodID, string Url)
        {
            ViewBag.ProdID = new SelectList(db.Products, "id", "id", prodID);
            Session["ProdID"] = prodID;
            Session["Url"] = Url;
            try
            {
                Image img = db.Images.Single(i => i.product_id == prodID && i.image_ == Url);
                //ViewBag.prodID = img.product_id;
                ViewBag.url = img.image_;
                ViewBag.Active = new SelectList(db.Images, "active", "active", img.active);
                ViewBag.dateUp = img.dateUpdate;
            }
            catch (Exception)
            {
                return RedirectToAction("ImagesProduct", "Image");
            }
            return View();
        }
        [HttpPost]
        public ActionResult UpdateImage(Image img, FormCollection f)
        {
            try
            {
                Image image = db.Images.Single(i => i.product_id == img.product_id && i.image_ == img.image_);
                //ViewBag.prodID = img.product_id;

                image.active = img.active;
                image.dateUpdate = DateTime.Now;
            }
            catch (Exception)
            {
                return RedirectToAction("ImagesProduct", "Image");
            }
            return View();
        }
        public ActionResult DeleteImageProduct(string maSP, string url)
        {
            try
            {
                Image image = db.Images.Single(i => i.product_id == maSP && i.image_ == url);
                db.Images.Remove(image);
                db.SaveChanges();
                RedirectToAction("ImagesProduct", "Image");
            }
            catch (Exception)
            {
                return RedirectToAction("ImagesProduct", "Image");
            }
            return RedirectToAction("ImagesProduct", "Image");
        }
    }
}
