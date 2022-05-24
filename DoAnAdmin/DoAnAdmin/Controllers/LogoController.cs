using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnAdmin.Models;

namespace DoAnAdmin.Controllers
{
    public class LogoController : Controller
    {
        //
        // GET: /Logo/
        QL_LaptopEntities db = new QL_LaptopEntities();


        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LogoPartial()
        {
            var logo = db.Banners.SingleOrDefault(b => b.BannerType.Trim() == "Logo" && b.active == true);
            if(logo == null)
                ViewBag.logo = "NO Image";
            else
                ViewBag.logo = logo.image_;
            return View();
        }

        public ActionResult LogoManager()
        {
            return View(db.Banners.Where(b => b.BannerType.Trim() == "Logo"));
        }
        public ActionResult InsertLogo()
        {
            return View();
        }
        [HttpPost]
        public ActionResult InsertLogo(FormCollection f)
        {
            return View();
        }
    }
}
