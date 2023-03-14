using BotDetect.Web.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHangs.Context;
using WebsiteBanHangs.Models;

namespace WebsiteBanHangs.Controllers
{
    public class HomeController : Controller
    {
        MyData data = new MyData();
        public ActionResult Index(string name)
        {
            HomeModel objmodel = new HomeModel();
            ViewBag.Find = name;
            objmodel.ListLoaiSanPham = data.LoaiSanPhams.ToList();
            objmodel.ListSanPham = data.SanPhams.ToList();
            var all_SanPham = (from ele in data.SanPhams select ele).OrderBy(p => p.MaSP);
            if (!String.IsNullOrEmpty(name))
            {
                all_SanPham = (IOrderedQueryable<SanPham>)all_SanPham.Where(a => a.TenSP.Contains(name));
                return View(all_SanPham.ToList());
            }
            return View(objmodel);

        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        [CaptchaValidation("CaptChaCode","registerCapcha","Mã xác nhận không đúng!")]

        public ActionResult DangKy(FormCollection collection, TaiKhoan kh)
        {

            var SDT = collection["SDT"];
            var email = collection["Email"];
            var matkhau = collection["MatKhau"];
            var MatKhauXacNhan = collection["MatKhauXacNhan"];
            var HoTen = collection["HoTen"];
            var Diachi = collection["DiaChi"];
            var NgayTao = String.Format("{0:MM/dd/yyyy}", collection["NgayTao"]);

            if (String.IsNullOrEmpty(MatKhauXacNhan))
            {
                ViewData["NhapMKXN"] = "Phải nhập mật khẩu xác nhận";
            }

            else
            {
                if (!matkhau.Equals(MatKhauXacNhan))
                {
                    ViewData["MatKhauGiongNhau"] = "Mật khẩu và mật khẩu xác nhận giống nhau";
                }
                else
                {
                    var check = data.TaiKhoans.FirstOrDefault(s => s.Email == email);
                    if (check == null)
                    {
                        kh.SDT = SDT;
                        kh.Email = email;
                        kh.MatKhau = matkhau;

                        kh.HoTen = HoTen;
                        kh.NgayTao = DateTime.Parse(NgayTao);
                        data.TaiKhoans.Add(kh);
                        data.SaveChanges();
                        return RedirectToAction("DangNhap");
                    }
                    else
                    {
                        ViewBag.ThongBao = "Tài khoản đã tồn tại!!!";
                        return View();
                    }

                }
                
            }
            if (!ModelState.IsValid)
            {
                ViewBag.ThongBao = " lỗi";
            }
            else
            {
                ViewBag.ThongBao = "thành công";
                MvcCaptcha.ResetCaptcha("registerCapcha");
            }
            //return View();

            return this.DangKy();
        }
        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection collection)
        {
            var Email = collection["Email"];
            var MatKhau = collection["MatKhau"];
            TaiKhoan kh = data.TaiKhoans.SingleOrDefault(n => n.Email == Email && n.MatKhau == MatKhau);
            if (kh != null)
            {
                if (Email == "Admin@gmail.com")
                {
                    ViewBag.ThongBao = "Chúc mừng đăng nhập thành công";
                    Session["TaiKhoan"] = kh;
                    return RedirectToAction("Index", "Admin/Admin");
                }
                else
                {

                    Session["TaiKhoan"] = kh;
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ViewBag.ThongBao = "Email hoặc mật khẩu không đúng";
            }
            return View();
        }

        public ActionResult DangXuat()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        

    }
}