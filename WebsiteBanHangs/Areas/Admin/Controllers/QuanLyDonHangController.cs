using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHangs.Context;

namespace WebsiteBanHangs.Areas.Admin.Controllers
{
    public class QuanLyDonHangController : Controller
    {
        // GET: Admin/QuanLyDonHang
        MyData data = new MyData();
        public ActionResult Index()
        {

            var lstDH = data.DonHangs.ToList();
            return View(lstDH);
            
        }

        public ActionResult Delete(int id)
        {
            var D_DonHang = data.DonHangs.First(m => m.MaDH == id);
            return View(D_DonHang);
        }
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var D_DonHang = data.DonHangs.Where(m => m.MaDH == id).First();
            data.DonHangs.Remove(D_DonHang);
            data.SaveChanges();
            return RedirectToAction("Index");
        }

        //public ActionResult Edit(int id)
        //{
        //    var E_DH = data.DonHangs.First(m => m.MaDH == id);
        //    return View(E_DH);
        //}
        //[HttpPost]
        //public ActionResult Edit(int id, FormCollection collection)
        //{
        //    var E_SanPham = data.DonHangs.First(m => m.MaDH == id);
        //    var E_MaTK = Convert.ToInt32(collection["Mã tài khoản"]); ;
           
        //    var E_TrangThai = Convert.ToBoolean(collection["Trạng thái"]);
           
        //    var E_NgayGiao = Convert.ToDateTime(collection["SoLuongTon"]);
        //    var E_NgayDat = Convert.ToDateTime(collection["MaLoaiSP"]);
        //    var E_ThanhToan = Convert.ToBoolean(collection["MaNCC"]);
        //    E_SanPham.MaDH = id;
        //    if (string.IsNullOrEmpty(E_MaTK))
        //    {
        //        ViewData["Error"] = "Don't empty!";
        //    }
        //    else
        //    {
        //        E_SanPham.MaTK = E_MaTK;
        //        E_SanPham.GiaoHang = E_TrangThai;
        //        E_SanPham.NgayGiao = E_NgayGiao;
        //        E_SanPham.NgayDat = E_NgayDat;
        //        E_SanPham.ThanhToan = E_ThanhToan;
               
               
        //        UpdateModel(E_SanPham);
        //        data.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return this.Edit(id);
        //}
    }
}