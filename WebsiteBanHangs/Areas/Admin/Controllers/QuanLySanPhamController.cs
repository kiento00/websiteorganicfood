using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHangs.Context;

namespace WebsiteBanHangs.Areas.Admin.Controllers
{
    public class QuanLySanPhamController : Controller
    {
        // GET: Admin/QuanLySanPham
        MyData data = new MyData();
        public ActionResult Index()
        {
            var lstProduct = data.SanPhams.ToList();
            return View(lstProduct);

        }

        public ActionResult Detail(int Id)
        {
            var lstProduct = data.SanPhams.Where(n => n.MaSP == Id).FirstOrDefault();
            return View(lstProduct);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection, SanPham s)
        {
            var E_TenSP = collection["TenSP"];
            var E_MoTa1 = collection["MoTa1"];
            var E_Mota2 = collection["Mota2"];
            var E_Hinh = collection["Hinh"];
            var E_GiaBan = Convert.ToInt32(collection["GiaBan"]);
            var E_NgayCapNhat = Convert.ToDateTime(collection["NgayCapNhat"]);

            var E_SoLuongTon = Convert.ToInt32(collection["SoLuongTon"]);

            var E_MaLoaiSP = Convert.ToInt32(collection["MaLoaiSP"]);
            var E_MaNCC = Convert.ToInt32(collection["MaNCC"]);
            if (string.IsNullOrEmpty(E_TenSP))
            {
                ViewData["Error"] = "Don't empty!";
            }
            else
            {
                s.TenSP = E_TenSP.ToString();
                s.MoTa1 = E_MoTa1.ToString();
                s.MoTa2 = E_Mota2.ToString();
                s.Hinh = E_Hinh.ToString();
                s.GiaBan = E_GiaBan;
                s.NgayCapNhat = E_NgayCapNhat;


                s.SoLuongTon = E_SoLuongTon;

                s.MaLoaiSP = E_MaLoaiSP;
                s.MaNCC = E_MaNCC;
                data.SanPhams.Add(s);
                data.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
        public string ProcessUpload(HttpPostedFileBase file)
        {
            if (file == null)
            {
                return "";
            }
            file.SaveAs(Server.MapPath("~/Areas/Admin/AssetsAD/images/" + file.FileName));
            return "/Areas/Admin/AssetsAD/images/" + file.FileName;
        }

        public ActionResult Delete(int id)
        {
            var D_SanPham = data.SanPhams.First(m => m.MaSP == id);
            return View(D_SanPham);
        }
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var D_SanPham = data.SanPhams.Where(m => m.MaSP == id).First();
            data.SanPhams.Remove(D_SanPham);
            data.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var E_SanPham = data.SanPhams.First(m => m.MaSP == id);
            return View(E_SanPham);
        }
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var E_SanPham = data.SanPhams.First(m => m.MaSP == id);
            var E_TenSP = collection["TenSP"];
            var E_Hinh = collection["Hinh"];
            var E_MoTa1 = collection["MoTa1"];
            var E_Mota2 = collection["Mota2"];
            var E_GiaBan = Convert.ToDecimal(collection["GiaBan"]);
            var E_NgayCapNhat = Convert.ToDateTime(collection["NgayCatNhat"]);
            var E_SoLuongTon = Convert.ToInt32(collection["SoLuongTon"]);
            var E_MaLoaiSP = Convert.ToInt32(collection["MaLoaiSP"]);
            var E_MaNCC = Convert.ToInt32(collection["MaNCC"]);
            E_SanPham.MaSP = id;
            if (string.IsNullOrEmpty(E_TenSP))
            {
                ViewData["Error"] = "Don't empty!";
            }
            else
            {
                E_SanPham.TenSP = E_TenSP;
                E_SanPham.Hinh = E_Hinh;
                E_SanPham.MoTa1 = E_MoTa1;
                E_SanPham.MoTa2 = E_Mota2;
                E_SanPham.GiaBan = E_GiaBan;
                E_SanPham.NgayCapNhat = E_NgayCapNhat;
                E_SanPham.SoLuongTon = E_SoLuongTon;
                E_SanPham.MaLoaiSP = E_MaLoaiSP;
                E_SanPham.MaNCC = E_MaNCC;
                UpdateModel(E_SanPham);
                data.SaveChanges();
                return RedirectToAction("Index");
            }
            return this.Edit(id);
        }

        public ActionResult ExportToExcel()
        {
            var doctors = from m in data.SanPhams
                          select m;

            byte[] fileContents;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage Ep = new ExcelPackage();
            ExcelWorksheet Sheet = Ep.Workbook.Worksheets.Add("DoctorsInfo");
            Sheet.Cells["A1"].Value = "Mã Sản Phẩm";
            Sheet.Cells["B1"].Value = "Tên Sản Phẩm";
            Sheet.Cells["C1"].Value = "Mô tả ngắn";
            Sheet.Cells["D1"].Value = "Mô tả dài";
            Sheet.Cells["E1"].Value = "Ngày cập nhật";
            Sheet.Cells["F1"].Value = "Giá bán";
            Sheet.Cells["G1"].Value = "Hình";
            Sheet.Cells["H1"].Value = "tồn kho";
            Sheet.Cells["I1"].Value = "Mã loại SP";
            Sheet.Cells["J1"].Value = "Mã Nhà CC";

            int row = 2;
            foreach (var item in doctors)
            {
                Sheet.Cells[string.Format("A{0}", row)].Value = item.MaSP;
                Sheet.Cells[string.Format("B{0}", row)].Value = item.TenSP;
                Sheet.Cells[string.Format("C{0}", row)].Value = item.MoTa1;
                Sheet.Cells[string.Format("D{0}", row)].Value = item.MoTa2;
                Sheet.Cells[string.Format("E{0}", row)].Value = item.NgayCapNhat;
                Sheet.Cells[string.Format("F{0}", row)].Value = item.GiaBan;
                Sheet.Cells[string.Format("G{0}", row)].Value = item.Hinh;
                Sheet.Cells[string.Format("H{0}", row)].Value = item.SoLuongTon;
                Sheet.Cells[string.Format("I{0}", row)].Value = item.MaLoaiSP;
                Sheet.Cells[string.Format("J{0}", row)].Value = item.MaNCC;
                row++;
            }


            Sheet.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            fileContents = Ep.GetAsByteArray();

            if (fileContents == null || fileContents.Length == 0)
            {
                return NotFound();
            }

            return File(
                fileContents: fileContents,
                contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileDownloadName: "SanPhamcuaOrganicFood.xlsx"
            );
        }

        private ActionResult NotFound()
        {
            throw new NotImplementedException();
        }
    }
}