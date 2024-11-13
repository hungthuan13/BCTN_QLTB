using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AssetInventory.Models;

namespace AssetInventory.Areas.QuanTriVien.Controllers
{

    public class DonViController : Controller
    {
        AIDataContext db = new AIDataContext();

        // GET: QuanTriVien/DonVi
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
        public JsonResult Select_DonVi()
        {
            var get_data = from s in db.DonVis.OrderByDescending(a => a.MaDV)
                           select new { s.MaDV, s.TenDV, s.MoTa, s.NgayCapNhat, s.NgayTao };
            return Json(new { data = get_data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Select_DonVi_By_MaDonVi(int MaDV)
        {
            var get_data = from s in db.DonVis.OrderByDescending(a => a.MaDV)
                           where s.MaDV == MaDV
                           select new { s.MaDV, s.TenDV, s.MoTa, s.NgayCapNhat, s.NgayTao };
            return Json(new { code = true, data = get_data }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult Insert_DonVi(DonVi dv)
        {
            var check_loaitaisan = from s in db.DonVis.OrderByDescending(a => a.TenDV)
                                   where s.TenDV == dv.TenDV
                                   select s;
            if (check_loaitaisan.Count() >= 1)
            {
                return Json(new { Message = "Thêm mới đơn vị thất bại, tên đơn vị này đã tồn tại", code = false });
            }

            if (string.IsNullOrEmpty(dv.MoTa))
            {
                dv.MoTa = "Không có";
            }
            dv.NgayTao = DateTime.Now;
            dv.NgayCapNhat = DateTime.Now;
            db.DonVis.InsertOnSubmit(dv);
            db.SubmitChanges();

            NguoiDung kh_insert = (NguoiDung)Session["Admin"];
            NhatKyHoatDong nkhd = new NhatKyHoatDong();
            nkhd.TenDangNhap = kh_insert.TenDangNhap;
            nkhd.HoatDong = "Thêm";
            nkhd.ChiTietHoatDong = "Thêm mới đơn vị";
            nkhd.NgayHoatDong = DateTime.Now;
            db.NhatKyHoatDongs.InsertOnSubmit(nkhd);
            db.SubmitChanges();
            return Json(new { Message = "Thêm mới thành công", code = true });
        }

        [HttpPost]
        public JsonResult Update_DonVi(DonVi dv)
        {
            try
            {
                var get_data = db.DonVis.SingleOrDefault(c => c.MaDV == dv.MaDV);
                get_data.TenDV = dv.TenDV;
                if (string.IsNullOrEmpty(dv.MoTa))
                {
                    get_data.MoTa = "Không có";
                }
                else
                {
                    get_data.MoTa = dv.MoTa;
                }
                get_data.NgayCapNhat = DateTime.Now;
                get_data.NgayTao = DateTime.Now;
                db.SubmitChanges();

                NguoiDung kh_insert = (NguoiDung)Session["Admin"];
                NhatKyHoatDong nkhd = new NhatKyHoatDong();
                nkhd.TenDangNhap = kh_insert.TenDangNhap;
                nkhd.HoatDong = "Sửa";
                nkhd.ChiTietHoatDong = "Sửa đơn vị có ID là: " + dv.MaDV;
                nkhd.NgayHoatDong = DateTime.Now;
                db.NhatKyHoatDongs.InsertOnSubmit(nkhd);
                db.SubmitChanges();
                return Json(new { code = true, msg = "Sửa thành công nha :3" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = false, msg = "Sửa hổng được, hình như có lỗi á :3" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult Delete_DonVi(DonVi dv)
        {
            try
            {
                var get_data = db.DonVis.SingleOrDefault(c => c.MaDV == dv.MaDV);
                db.DonVis.DeleteOnSubmit(get_data);
                db.SubmitChanges();

                NguoiDung kh_insert = (NguoiDung)Session["Admin"];
                NhatKyHoatDong nkhd = new NhatKyHoatDong();
                nkhd.TenDangNhap = kh_insert.TenDangNhap;
                nkhd.HoatDong = "Xóa";
                nkhd.ChiTietHoatDong = "Xóa đơn vị có ID là: " + dv.MaDV;
                nkhd.NgayHoatDong = DateTime.Now;
                db.NhatKyHoatDongs.InsertOnSubmit(nkhd);
                db.SubmitChanges();

                return Json(new { code = true, msg = "Xóa thành công nha :3" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = false, msg = "Xóa hổng được, hình như có lỗi á :3 \n Chi tiết lỗi: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}