using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AssetInventory.Models;

namespace AssetInventory.Areas.QuanTriVien.Controllers
{
    public class NhatKyHoatDongController : Controller
    {
        AIDataContext db = new AIDataContext();

        // GET: QuanTriVien/NhatKyHoatDong
        public ActionResult Index()
        {
            if (Session["Admin"] == null || Session["Admin"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "TrangChu");
            }
            else
            {
                return View();

            }
        }


        [HttpGet]
        public JsonResult Select_NKHD()
        {
            var get_data = from s in db.NhatKyHoatDongs.OrderByDescending(a => a.NgayHoatDong)
                           select new { s.MaNKHD, s.TenDangNhap, s.HoatDong, s.ChiTietHoatDong, s.GhiChu, s.NgayHoatDong};
            return Json(new { data = get_data }, JsonRequestBehavior.AllowGet);
        }
    }
}