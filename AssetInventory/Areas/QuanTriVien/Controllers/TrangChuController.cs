using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AssetInventory.Models;

namespace AssetInventory.Areas.QuanTriVien.Controllers
{
    public class TrangChuController : Controller
    {
        // GET: QuanTriVien/TrangChu
        AIDataContext db = new AIDataContext();


        [HttpGet]
        public ActionResult Index()
        {
            if (Session["Admin"] == null || Session["Admin"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "TrangChu");
            }else
            {
                return View();

            }
        }

        [HttpGet]
        public ActionResult DangNhap()
        {
            if (Session["Admin"] == null || Session["Admin"].ToString() == "")
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "TrangChu");

            }
            
        }

        [HttpPost]
        public ActionResult DangNhap(FormCollection collection)
        {
            var sTDN = collection["username"];
            var sMK = collection["password"];
            if (String.IsNullOrEmpty(sTDN))
            {
                ViewBag.ThongBao = "Chưa nhập tên đăng nhập kìa.";
            }
            else if (String.IsNullOrEmpty(sMK))
            {
                ViewBag.ThongBao = "Phải nhập mật khẩu.";
            }
            else
            {

                NguoiDung kh = db.NguoiDungs.SingleOrDefault(n => n.TenDangNhap == sTDN && n.MatKhau == sMK);
                if (kh != null)
                {
                    Session["Admin"] = kh;

                    NhatKyHoatDong nkhd = new NhatKyHoatDong();
                    nkhd.TenDangNhap = kh.TenDangNhap;
                    nkhd.HoatDong = "Đăng nhập";
                    nkhd.ChiTietHoatDong = kh.TenDangNhap + " đăng nhập vào hệ thống thành công";
                    nkhd.NgayHoatDong = DateTime.Now;
                    db.NhatKyHoatDongs.InsertOnSubmit(nkhd);
                    db.SubmitChanges();

                    return RedirectToAction("Index", "TrangChu");
                }
                else
                {
                    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu hông đúng.";
                }
            }

            return View();
        }



        public ActionResult DangXuat()
        {
            FormsAuthentication.SignOut();
            //Session.Abandon();
            Session.Remove("Admin");
            return RedirectToAction("DangNhap", "TrangChu");
        }


    }
}