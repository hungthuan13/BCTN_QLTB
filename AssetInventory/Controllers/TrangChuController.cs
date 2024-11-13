using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AssetInventory.Models;

namespace AssetInventory.Controllers
{
    public class TrangChuController : Controller
    {
        // GET: TrangChu
        AIDataContext db = new AIDataContext();

        public ActionResult Index()
        {
            if (Session["User"] == null || Session["User"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "TrangChu");
            }
            else
            {
                return View();

            }
        }

        [HttpGet]
        public ActionResult DangNhap()
        {
            if (Session["User"] == null || Session["User"].ToString() == "")
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
            var sTDN = collection["TenDangNhap"];
            var sMK = collection["MatKhau"];
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
                    Session["User"] = kh;
                    return RedirectToAction("Index", "TrangChu");   
                }
                else
                {
                    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu hông đúng.";
                }
            }

            return View();
        }




        public ActionResult NavPartial()
        {
            return PartialView();
        }

        public ActionResult ThietBiPartial()
        {
            return PartialView();
        }

        public ActionResult FooterPartial()
        {
            return PartialView();
        }



        public ActionResult DangXuat()
        {
            FormsAuthentication.SignOut();
            //Session.Abandon();
            Session.Remove("User");
            return RedirectToAction("Index", "TrangChu");
        }

    }
}