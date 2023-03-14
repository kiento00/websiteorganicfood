using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHangs.Context;

namespace WebsiteBanHangs.Areas.Admin.Controllers
{
    public class QuanLyTaiKhoanController : Controller
    {
        // GET: Admin/QuanLyTaiKhoan
        MyData data = new MyData();
        public ActionResult Index()
        {
            var lstTaiKhoan = data.TaiKhoans.ToList();
            return View(lstTaiKhoan);
        }
    }
}