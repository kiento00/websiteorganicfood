using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebsiteBanHangs.Context;

namespace WebsiteBanHangs.Models
{
    public class GioHang
    {
        MyData data = new MyData();
        public int MaSP { get; set; }


        [Display(Name = "Tên Sản phẩm ")]
        public string TenSP { get; set; }


        [Display(Name = "Ảnh bìa ")]
        public string Hinh { get; set; }

        [Display(Name = "Giá bán ")]

        public Double GiaBan { get; set; }

        [Display(Name = "Số lượng ")]
        public int soluong { get; set; }
        [Display(Name = "Số lượng tồn ")]

        public int SoLuongTon { get; set; }

        [Display(Name = "Mã loại sản phẩm ")]
        public int MaLoaiSP { get; set; }
        [Display(Name = "Mã Nhà cung cấp ")]
        public int MaNCC { get; set; }

        [Display(Name = "Thành tiền ")]
        public Double dthanhtien
        {
            get { return soluong * GiaBan; }
        }

        public GioHang(int id)
        {
            MaSP = id;
            SanPham sp = data.SanPhams.Single(n => n.MaSP == MaSP);
            TenSP = sp.TenSP;
            Hinh = sp.Hinh;


            GiaBan = double.Parse(sp.GiaBan.ToString());
            soluong = 1;
            SoLuongTon = Convert.ToInt32(sp.SoLuongTon);
        }
    }
}