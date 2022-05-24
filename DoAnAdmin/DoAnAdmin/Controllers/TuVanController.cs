using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnAdmin.Models;

namespace DoAnAdmin.Controllers
{
    public class TuVanController : Controller
    {
        //
        // GET: /TuVan/
        K_NNAlgorithm knn = new K_NNAlgorithm();
        Random r = new Random();
        public ActionResult TuVanKhachHang()
        {
            return View();
        }
        [HttpPost]
        public ActionResult TuVanKhachHang(FormCollection f)
        {
            LaptopDouble lapNew = new LaptopDouble();
            lapNew.TypeName =  knn.IntToString(f["TypeName"]);
            lapNew.Inches = knn.IntToString(f["Inches"]);
            lapNew.ScreenResolution = knn.IntToString(f["ScreenResolution"]);
            lapNew.Cpu = knn.IntToString(f["Cpu"]);
            lapNew.Ram = knn.IntToString(f["Ram"]);
            lapNew.Memory = knn.IntToString(f["Memory"]);
            lapNew.Gpu = knn.IntToString(f["Gpu"]);
            lapNew.OpSys = knn.IntToString(f["OpSys"]);
            //Ccân nặng
            if (f["Weight"] =="1")
            {
                lapNew.Weight = r.NextDouble();
            }
            else if (f["Weight"] == "2")
            {
                lapNew.Weight = r.NextDouble() + 1;
            }
            else if (f["Weight"] == "3")
            {
                lapNew.Weight = r.NextDouble() + 2;
            }
            else if (f["Weight"] == "4")
            {
                lapNew.Weight = r.NextDouble() + 3;
            }
            else
            {
                lapNew.Weight = r.NextDouble() + 4;
            }
            //Mức giá
            if (f["Price_euros"] == "1")//Dưới 10tr
            {
                lapNew.Price_euros = r.Next(260, 434);
            }
            else if (f["Price_euros"] == "2")//Từ 10tr - 15tr
            {
                lapNew.Price_euros = r.Next(434, 652);
            }
            else if (f["Price_euros"] == "3")//Từ 15tr - 20tr
            {
                lapNew.Price_euros = r.Next(652, 870);
            }
            else if (f["Price_euros"] == "4")//Từ 20tr - 25tr
            {
                lapNew.Price_euros = r.Next(870, 1086);
            }
            else if (f["Price_euros"] == "5")//Từ 25tr - 30tr   
            {
                lapNew.Price_euros = r.Next(1086, 1304);
            }
            else if (f["Price_euros"] == "6")//Từ 30tr - 40tr
            {
                lapNew.Price_euros = r.Next(1304, 1740);
            }
            else if (f["Price_euros"] == "7")//Từ 40tr - 50tr
            {
                lapNew.Price_euros = r.Next(1740, 2174);
            }
            else if (f["Price_euros"] == "8")//Từ 50tr - 60tr
            {
                lapNew.Price_euros = r.Next(2174, 2609);
            }
            else if (f["Price_euros"] == "9")//Từ 60tr - 70tr
            {
                lapNew.Price_euros = r.Next(2609, 3044);
            }
            else if (f["Price_euros"] == "10")//Từ 70tr - 80tr
            {
                lapNew.Price_euros = r.Next(3044, 3479);
            }
            else if (f["Price_euros"] == "11")//Từ 80tr - 100tr
            {
                lapNew.Price_euros = r.Next(3479, 4348);
            }
            else//Trên 100tr
            {
                lapNew.Price_euros = r.Next(4348, 6000);
            }
            var kq = knn.thuatToan(lapNew, 30);
            ViewBag.tuvan = (kq == 152) ? 3 : (kq == 385) ? 6 : (kq == 147) ? 8
                : (kq == 489) ? 1 : (kq == 412) ? 2 : (kq == 379) ? 5
                : (kq == 233) ? 7 : (kq == 627) ? 4 : 9;


            return View();
        }

    }
}
