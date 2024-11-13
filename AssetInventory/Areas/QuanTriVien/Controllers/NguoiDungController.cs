using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AssetInventory.Models;

namespace AssetInventory.Areas.QuanTriVien.Controllers
{
    public class NguoiDungController : Controller
    {
        AIDataContext db = new AIDataContext();

        // GET: QuanTriVien/NguoiDung
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
        public JsonResult Select_TongSoLuong_NguoiDung()
        {
            var get_data = from s in db.NguoiDungs.OrderByDescending(a => a.MaND)
                           join dv in db.DonVis on s.MaDV equals dv.MaDV
                           select new { s.MaDV, s.MaND, s.TenDangNhap, s.MatKhau, s.HoVaTen, s.ChucDanh, s.PhanQuyen, s.SoDienThoai, s.Email, s.DiaChi, s.NgayCapNhat, s.NgayTao, dv.TenDV };
            return Json(new {code = true, SoLuong = get_data.Count() }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Select_NguoiDung()
        {
            var get_data = from s in db.NguoiDungs.OrderByDescending(a => a.MaND)
                           join dv in db.DonVis on s.MaDV equals dv.MaDV
                           select new { s.MaDV, s.MaND, s.TenDangNhap, s.MatKhau, s.HoVaTen, s.ChucDanh, s.PhanQuyen, s.SoDienThoai, s.Email, s.DiaChi, s.NgayCapNhat, s.NgayTao, dv.TenDV };
            return Json(new { data = get_data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Select_NguoiDung_By_MaND(int MaND)
        {
            var get_data = from s in db.NguoiDungs.OrderByDescending(a => a.MaND)
                           where s.MaND == MaND
                           join dv in db.DonVis on s.MaDV equals dv.MaDV
                           select new { s.MaDV, s.MaND, s.TenDangNhap, s.MatKhau, s.HoVaTen, s.ChucDanh, s.PhanQuyen, s.SoDienThoai, s.Email, s.DiaChi, s.NgayCapNhat, s.NgayTao, dv.TenDV };
            return Json(new { code = true, data = get_data }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Insert_NguoiDung(NguoiDung nd)
        {
            var check_nguoidung = from s in db.NguoiDungs.OrderByDescending(a => a.MaND)
                                  where s.TenDangNhap == nd.TenDangNhap
                                  select s;
            if (check_nguoidung.Count() >= 1)
            {
                return Json(new { Message = "Thêm mới người dùng thất bại, tên tài khoản này đã tồn tại", code = false });
            }
            if (nd.PhanQuyen != 0 && nd.PhanQuyen != 1)
            {
                return Json(new { Message = "Thêm mới người dùng thất bại, phân quyền không hợp lệ", code = false });
            }
            nd.NgayTao = DateTime.Now;
            nd.NgayCapNhat = DateTime.Now;
            db.NguoiDungs.InsertOnSubmit(nd);
            db.SubmitChanges();
            NguoiDung kh_insert = (NguoiDung)Session["Admin"];
            NhatKyHoatDong nkhd = new NhatKyHoatDong();
            nkhd.TenDangNhap = kh_insert.TenDangNhap;
            nkhd.HoatDong = "Thêm";
            nkhd.ChiTietHoatDong = "Thêm người dùng";
            nkhd.NgayHoatDong = DateTime.Now;
            db.NhatKyHoatDongs.InsertOnSubmit(nkhd);
            db.SubmitChanges();
            return Json(new { Message = "Thêm mới thành công", code = true });
        }

        [HttpPost]
        public JsonResult Update_NguoiDung(NguoiDung nd)
        {
            var check_nguoidung = from s in db.NguoiDungs.OrderByDescending(a => a.MaND)
                                  where s.TenDangNhap == nd.TenDangNhap
                                  select s;
            if (nd.PhanQuyen != 0 && nd.PhanQuyen != 1)
            {
                return Json(new { Message = "Sửa người dùng thất bại, phân quyền không hợp lệ", code = false });
            }
            try
            {
                var get_data = db.NguoiDungs.SingleOrDefault(c => c.MaND == nd.MaND);
                get_data.MaDV = nd.MaDV;
                get_data.TenDangNhap = nd.TenDangNhap;
                get_data.MatKhau = nd.MatKhau;
                get_data.HoVaTen = nd.HoVaTen;
                get_data.ChucDanh = nd.ChucDanh;
                get_data.PhanQuyen = nd.PhanQuyen;
                get_data.SoDienThoai = nd.SoDienThoai;
                get_data.Email = nd.Email;
                get_data.DiaChi = nd.DiaChi;
                get_data.NgayCapNhat = DateTime.Now;
                db.SubmitChanges();

                NguoiDung kh_insert = (NguoiDung)Session["Admin"];
                NhatKyHoatDong nkhd = new NhatKyHoatDong();
                nkhd.TenDangNhap = kh_insert.TenDangNhap;
                nkhd.HoatDong = "Sửa";
                nkhd.ChiTietHoatDong = "Sửa người dùng có Mã Người Dùng là: " + nd.MaND;
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
        public JsonResult Delete_NguoiDung(NguoiDung nd)
        {
            try
            {
                var get_data = db.NguoiDungs.SingleOrDefault(c => c.MaND == nd.MaND);
                db.NguoiDungs.DeleteOnSubmit(get_data);
                db.SubmitChanges();

                NguoiDung kh_insert = (NguoiDung)Session["Admin"];
                NhatKyHoatDong nkhd = new NhatKyHoatDong();
                nkhd.TenDangNhap = kh_insert.TenDangNhap;
                nkhd.HoatDong = "Xóa";
                nkhd.ChiTietHoatDong = "Xóa người dùng có Mã Người Dùng là: " + nd.MaND;
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