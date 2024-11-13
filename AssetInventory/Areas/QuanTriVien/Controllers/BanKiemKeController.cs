using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AssetInventory.Models;

namespace AssetInventory.Areas.QuanTriVien.Controllers
{
    public class BanKiemKeController : Controller
    {
        AIDataContext db = new AIDataContext();

        // GET: QuanTriVien/BanKiemKe
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
        public JsonResult Select_BanKiemKe(int MaPhieu)
        {
            var get_data = from s in db.BanKiemKes.OrderByDescending(a => a.MaPhieu)
                           where MaPhieu == s.MaPhieu
                           join p in db.DonVis on s.MaDV equals p.MaDV
                           select new 
                           { 
                               s.MaBanKiemKe,
                                s.MaPhieu,
                                p.TenDV,
                                s.HoVaTen,
                                s.ChucVu,
                           };
            return Json(new { data = get_data }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Insert_BanKiemKe(BanKiemKe bkk)
        {
           
                try
                {

                    bkk.TrangThai = 0;
                    bkk.NgayTao = DateTime.Now;
                    bkk.NgayCapNhat = DateTime.Now;
                    if (string.IsNullOrEmpty(bkk.GhiChu))
                    {
                        bkk.GhiChu = "Không có";
                    }
                    db.BanKiemKes.InsertOnSubmit(bkk);
                    db.SubmitChanges();

                    NguoiDung kh_insert = (NguoiDung)Session["Admin"];
                    NhatKyHoatDong nkhd = new NhatKyHoatDong();
                    nkhd.TenDangNhap = kh_insert.TenDangNhap;
                    nkhd.HoatDong = "Thêm";
                    nkhd.ChiTietHoatDong = "Thêm ban kiểm kê vào Mã Phiếu: " + bkk.MaPhieu;
                    nkhd.NgayHoatDong = DateTime.Now;
                    db.NhatKyHoatDongs.InsertOnSubmit(nkhd);
                    db.SubmitChanges();
                return Json(new { Message = "Thêm mới thành công", code = true });
                    

                }
                catch (Exception ex)
                {
                    return Json(new { Message = ex.Message, code = false });
                }
            

        }





        [HttpPost]
        public JsonResult Delete_BanKiemKe(int MaBanKiemKe)
        {
            try
            {
                var cd = db.BanKiemKes.SingleOrDefault(c => c.MaBanKiemKe == MaBanKiemKe);

                NguoiDung kh_insert = (NguoiDung)Session["Admin"];
                NhatKyHoatDong nkhd = new NhatKyHoatDong();
                nkhd.TenDangNhap = kh_insert.TenDangNhap;
                nkhd.HoatDong = "Xóa";
                nkhd.ChiTietHoatDong = "Xóa ban kiểm kê ở Mã Phiếu: " + cd.MaPhieu;
                nkhd.NgayHoatDong = DateTime.Now;
                db.NhatKyHoatDongs.InsertOnSubmit(nkhd);
                db.SubmitChanges();


                db.BanKiemKes.DeleteOnSubmit(cd);
                db.SubmitChanges();

                
                return Json(new { code = 1, msg = "Xóa thành công nha :3" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 0, msg = "Xóa hổng được, hình như có lỗi á :3 \nChi tiết lỗi: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }



    }
}