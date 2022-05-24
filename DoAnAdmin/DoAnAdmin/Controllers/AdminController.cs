using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnAdmin.Models;
using System.Data;
using PagedList;
namespace DoAnAdmin.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        //dbQuanLyBanLaptopDataContext db = new dbQuanLyBanLaptopDataContext();

        QL_LaptopEntities db = new QL_LaptopEntities();

        HamXuLy xuLy = new HamXuLy();

        public ActionResult Home()
        {

            return View();
        }
        public ActionResult Admin()
        {
            return View();
        }

        public ActionResult NhanVien()
        {
            return View();
        }
        public ActionResult HoaDon()
        {
            return View();
        }
        public ActionResult ThuongHieu()
        {
            return View();
        }
        
        //-----------------------------------------------------------
        
    }
}
