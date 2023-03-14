using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebsiteBanHangs.Context;
using WebsiteBanHangs.Models;
using System.Configuration;
using SendMail;
using Newtonsoft.Json.Linq;

namespace WebsiteBanHangs.Controllers
{
    public class GioHangController : Controller
    {
        // GET: GioHang
        MyData data = new MyData();
        public List<GioHang> Laygiohang()
        {
            List<GioHang> listGiohang = Session["GioHang"] as List<GioHang>;
            if (listGiohang == null)
            {
                listGiohang = new List<GioHang>();
                Session["GioHang"] = listGiohang;
            }
            return listGiohang;
        }

        public ActionResult ThemGiohang(int Id, string strURL)
        {
            List<GioHang> listGiohang = Laygiohang();
            GioHang sanpham = listGiohang.Find(n => n.MaSP == Id);
            if (sanpham == null)
            {
                sanpham = new GioHang(Id);
                listGiohang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.soluong++;
                return Redirect(strURL);
            }
        }

        private int TongSoLuong()
        {
            int tsl = 0;
            List<GioHang> listGiohang = Session["GioHang"] as List<GioHang>;
            if (listGiohang != null)
            {
                tsl = listGiohang.Sum(n => n.soluong);
            }
            return tsl;
        }

        private int TongSoLuongSanPham()
        {
            int tsl = 0;
            List<GioHang> listGiohang = Session["GioHang"] as List<GioHang>;
            if (listGiohang != null)
            {
                tsl = listGiohang.Count;
            }
            return tsl;
        }

        private double TongTien()
        {
            double tt = 0;
            List<GioHang> listGiohang = Session["GioHang"] as List<GioHang>;
            if (listGiohang != null)
            {
                tt = listGiohang.Sum(n => n.dthanhtien);
            }
            return tt;

        }

        public ActionResult Giohang()
        {
            List<GioHang> listGiohang = Laygiohang();
            //GioHang sanpham = listGiohang.SingleOrDefault(n => n.maxe == id);
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();
            return View(listGiohang);
        }

        public ActionResult GiohangPartial()
        {
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();
            return PartialView();
        }

        public ActionResult XoaGiohang(int id)
        {
            List<GioHang> listGiohang = Laygiohang();
            GioHang sanpham = listGiohang.SingleOrDefault(n => n.MaSP == id);
            if (sanpham != null)
            {
                listGiohang.RemoveAll(n => n.MaSP == id);
                return RedirectToAction("GioHang");
            }
            return RedirectToAction("GioHang");
        }

        public ActionResult CapNhatGioHang(int id, FormCollection collection)
        {
            List<GioHang> listGiohang = Laygiohang();

            GioHang sanpham = listGiohang.SingleOrDefault(n => n.MaSP == id);
            SanPham sp = data.SanPhams.Where(n => n.MaSP == sanpham.MaSP).FirstOrDefault();
            if (sanpham != null)
            {
                if (Convert.ToInt32(collection["txtSoLuong"]) <= sp.SoLuongTon)
                {
                    sanpham.soluong = Convert.ToInt32(collection["txtSoLuong"].ToString());
                }
                else if ((Convert.ToInt32(collection["txtSoLuong"]) > sp.SoLuongTon))
                {
                    ViewBag.ThongBao = "Sản phẩm vượt quá số lượng tồn";
                }
            }
            return RedirectToAction("GioHang");
        }

        public ActionResult XoaTatCaGioHang()
        {
            List<GioHang> listGiohang = Laygiohang();
            listGiohang.Clear();
            return RedirectToAction("GioHang");
        }

        [HttpGet]
        public ActionResult DatHang()
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "Home");
            }
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<GioHang> listGioHang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();
            return View(listGioHang);
        }
        public ActionResult DatHang(FormCollection collection)
        {
            DonHang dh = new DonHang();
            TaiKhoan kh = (TaiKhoan)Session["TaiKhoan"];
            SanPham s = new SanPham();

            List<GioHang> gh = Laygiohang();
            var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["NgayGiao"]);

            dh.MaTK = kh.MaTK;
            dh.NgayDat = DateTime.Now;
            dh.NgayGiao = DateTime.Parse(ngaygiao);
            //dh.HuyDH = false;
            dh.ThanhToan = false;

            data.DonHangs.Add(dh);
            data.SaveChanges();
            foreach (var item in gh)
            {
                ChiTietDonHang ctdh = new ChiTietDonHang();
                ctdh.MaDH = dh.MaDH;
                ctdh.MaSP = item.MaSP;
                ctdh.SoLuong = item.soluong;
                ctdh.Gia = (decimal)item.GiaBan;
                s = data.SanPhams.Single(n => n.MaSP == item.MaSP);
                s.SoLuongTon -= ctdh.SoLuong;
                data.SaveChanges();
                data.ChiTietDonHangs.Add(ctdh);
            }
            data.SaveChanges();
            //SendEmail();
           // Session["GioHang"] = null;
            return RedirectToAction("Payment", "GioHang");

        }
        public void SendEmail()
        { /*SanPham sp = (SanPham)Session["SanPham"]*/;
            TaiKhoan kh = (TaiKhoan)Session["TaiKhoan"];
            string content = System.IO.File.ReadAllText(Server.MapPath("~/SendMail/ThongTinDonHang.html"));
            content = content.Replace("{{CustomerName}}", kh.HoTen);
            content = content.Replace("{{Phone}}", kh.SDT);
            content = content.Replace("{{Email}}", kh.Email);
            content = content.Replace("{{Address}}", kh.DiaChi);
            //content = content.Replace("{{Picture}}", sp.Hinh);
            content = content.Replace("{{Total}}", TongTien().ToString("N0"));

            var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();

            new MailHelper().SendMail(kh.Email, "Đơn hàng mới từ OrganicFood", content);
            new MailHelper().SendMail(toEmail, "Đơn hàng mới từ OrganicFood", content);
        }

        public ActionResult XacnhanDonHang()
        {
            return View();

        }

        public ActionResult Payment()
        {
            //request params need to request to MoMo system
            string endpoint = "https://test-payment.momo.vn/gw_payment/transactionProcessor";
            string partnerCode = "MOMO0AOF20220530";
            string accessKey = "TvCzPCFxIRIsdBLa";
            string serectkey = "kHM3KpmXObLYuCvg8noVNGnWRUIWdwlM";
            string orderInfo = "test";
            string returnUrl = "https://localhost:44344/GioHang/XacnhanDonHang";
            string notifyurl = "http://ba1adf48beba.ngrok.io/Home/SavePayment"; //lưu ý: notifyurl không được sử dụng localhost, có thể sử dụng ngrok để public localhost trong quá trình test

            string amount = TongTien().ToString();
            string orderid = DateTime.Now.Ticks.ToString();
            string requestId = DateTime.Now.Ticks.ToString();
            string extraData = "";

            //Before sign HMAC SHA256 signature
            string rawHash = "partnerCode=" +
                partnerCode + "&accessKey=" +
                accessKey + "&requestId=" +
                requestId + "&amount=" +
                amount + "&orderId=" +
                orderid + "&orderInfo=" +
                orderInfo + "&returnUrl=" +
                returnUrl + "&notifyUrl=" +
                notifyurl + "&extraData=" +
                extraData;

            MoMoSecurity crypto = new MoMoSecurity();
            //sign signature SHA256
            string signature = crypto.signSHA256(rawHash, serectkey);

            //build body json request
            JObject message = new JObject
            {
                { "partnerCode", partnerCode },
                { "accessKey", accessKey },
                { "requestId", requestId },
                { "amount", amount },
                { "orderId", orderid },
                { "orderInfo", orderInfo },
                { "returnUrl", returnUrl },
                { "notifyUrl", notifyurl },
                { "extraData", extraData },
                { "requestType", "captureMoMoWallet" },
                { "signature", signature }

            };

            string responseFromMomo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());

            JObject jmessage = JObject.Parse(responseFromMomo);

            return Redirect(jmessage.GetValue("payUrl").ToString());


        }

        //Khi thanh toán xong ở cổng thanh toán Momo, Momo sẽ trả về một số thông tin, trong đó có errorCode để check thông tin thanh toán
        //errorCode = 0 : thanh toán thành công (Request.QueryString["errorCode"])
        //Tham khảo bảng mã lỗi tại: https://developers.momo.vn/#/docs/aio/?id=b%e1%ba%a3ng-m%c3%a3-l%e1%bb%97i
        public ActionResult ConfirmPaymentClient()
        {
            //hiển thị thông báo cho người dùng
            return View();
        }

        [HttpPost]
        public void SavePayment()
        {
            //cập nhật dữ liệu vào db
        }

    }
}