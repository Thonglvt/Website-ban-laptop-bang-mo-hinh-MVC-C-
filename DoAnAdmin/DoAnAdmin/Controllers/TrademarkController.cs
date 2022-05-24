using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnAdmin.Controllers
{
    public class TrademarkController : Controller
    {
        //
        // GET: /Trademark/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult TrademarkPartial()
        {
            return View();
        }
    }
}
