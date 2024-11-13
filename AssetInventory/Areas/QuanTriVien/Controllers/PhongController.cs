using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AssetInventory.Models;
namespace AssetInventory.Areas.QuanTriVien.Controllers
{
    public class PhongController : Controller
    {
        AIDataContext db = new AIDataContext();


        // GET: QuanTriVien/Phong
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
        public JsonResult Select_TongSoLuong_Phong()
        {
            var get_data = from s in db.Phongs.OrderByDescending(a => a.MaKV)
                           join kvp in db.KhuVucPhongs on s.MaKV equals kvp.MaKV
                           join lp in db.LoaiPhongs on s.MaLP equals lp.MaLP
                           select new { s.MaPhong, s.MaKV, s.MaLP, s.TenPhong, kvp.TenKV, lp.TenLP, s.GhiChu, s.NgayCapNhat };
            return Json(new { code = true, SoLuong = get_data.Count() }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Select_Phong()
        {
            var get_data = from s in db.Phongs.OrderByDescending(a => a.MaKV)
                           join kvp in db.KhuVucPhongs on s.MaKV equals kvp.MaKV
                           join lp in db.LoaiPhongs on s.MaLP equals lp.MaLP
                           select new { s.MaPhong, s.MaKV, s.MaLP, s.TenPhong, kvp.TenKV, lp.TenLP, s.GhiChu, s.NgayCapNhat };
            return Json(new { data = get_data }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult Select_KhuVucPhong()
        {
            var get_data = from s in db.KhuVucPhongs.OrderByDescending(a => a.MaKV)
                           select new { s.MaKV, s.TenKV, s.GhiChu, s.NgayCapNhat };
            return Json(new { data = get_data }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult Select_LoaiPhong()
        {
            var get_data = from s in db.LoaiPhongs.OrderByDescending(a => a.MaLP)
                           select new { s.MaLP, s.TenLP, s.GhiChu, s.NgayCapNhat };
            return Json(new { data = get_data }, JsonRequestBehavior.AllowGet);
        }




        [HttpGet]
        public JsonResult Select_Phong_By_MaPhong(int MaPhong)
        {
            var get_data = from s in db.Phongs.OrderByDescending(a => a.MaKV)
                           where s.MaPhong == MaPhong
                           join kvp in db.KhuVucPhongs on s.MaKV equals kvp.MaKV
                           join lp in db.LoaiPhongs on s.MaLP equals lp.MaLP
                           select new { s.MaPhong, s.MaKV, s.MaLP, s.TenPhong, kvp.TenKV, lp.TenLP, s.GhiChu, s.NgayCapNhat };
            return Json(new { code = true, data = get_data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Select_KhuVucPhong_By_MaKV(int MaKV)
        {
            var get_data = from s in db.KhuVucPhongs.OrderByDescending(a => a.MaKV)
                           where s.MaKV == MaKV
                           select new { s.MaKV, s.TenKV, s.GhiChu, s.NgayCapNhat };
            return Json(new { code = true, data = get_data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Select_LoaiPhong_By_MaLP(int MaLP)
        {
            var get_data = from s in db.LoaiPhongs.OrderByDescending(a => a.MaLP)
                           where s.MaLP == MaLP
                           select new { s.MaLP, s.TenLP, s.GhiChu, s.NgayCapNhat };
            return Json(new { code = true, data = get_data }, JsonRequestBehavior.AllowGet);
        }

 

   



        [HttpPost]
        public JsonResult Insert_Phong(Phong p)
        {
            var check_phong = from s in db.Phongs.OrderByDescending(a => a.MaPhong)
                                  where s.TenPhong == p.TenPhong
                                  select s;
            if (check_phong.Count() >= 1)
            {
                return Json(new { Message = "Thêm mới phòng thất bại, tên phòng này đã tồn tại", code = false });
            }
            if (string.IsNullOrEmpty(p.GhiChu))
            {
                p.GhiChu = "Không có";
            }
            p.NgayTao = DateTime.Now;
            p.NgayCapNhat = DateTime.Now;
            db.Phongs.InsertOnSubmit(p);
            db.SubmitChanges();

            NguoiDung kh_insert = (NguoiDung)Session["Admin"];
            NhatKyHoatDong nkhd = new NhatKyHoatDong();
            nkhd.TenDangNhap = kh_insert.TenDangNhap;
            nkhd.HoatDong = "Thêm";
            nkhd.ChiTietHoatDong = "Thêm mới phòng";
            nkhd.NgayHoatDong = DateTime.Now;
            db.NhatKyHoatDongs.InsertOnSubmit(nkhd);
            db.SubmitChanges();
            return Json(new { Message = "Thêm mới thành công", code = true });
        }

        [HttpPost]
        public JsonResult Insert_KhuVucPhong(KhuVucPhong kvp)
        {
            var check_kvphong = from s in db.KhuVucPhongs.OrderByDescending(a => a.MaKV)
                              where s.TenKV == kvp.TenKV
                              select s;
            if (check_kvphong.Count() >= 1)
            {
                return Json(new { Message = "Thêm mới kv phòng thất bại, tên kv phòng này đã tồn tại", code = false });
            }
            if (string.IsNullOrEmpty(kvp.GhiChu))
            {
                kvp.GhiChu = "Không có";
            }
            kvp.NgayTao = DateTime.Now;
            kvp.NgayCapNhat = DateTime.Now;
            db.KhuVucPhongs.InsertOnSubmit(kvp);
            db.SubmitChanges();

            NguoiDung kh_insert = (NguoiDung)Session["Admin"];
            NhatKyHoatDong nkhd = new NhatKyHoatDong();
            nkhd.TenDangNhap = kh_insert.TenDangNhap;
            nkhd.HoatDong = "Thêm";
            nkhd.ChiTietHoatDong = "Thêm mới khu vực phòng";
            nkhd.NgayHoatDong = DateTime.Now;
            db.NhatKyHoatDongs.InsertOnSubmit(nkhd);
            db.SubmitChanges();
            return Json(new { Message = "Thêm mới thành công", code = true });
        }

        [HttpPost]
        public JsonResult Insert_LoaiPhong(LoaiPhong lp)
        {
            var check_loaiphong = from s in db.LoaiPhongs.OrderByDescending(a => a.MaLP)
                                where s.TenLP == lp.TenLP
                                select s;
            if (check_loaiphong.Count() >= 1)
            {
                return Json(new { Message = "Thêm mới loại phòng thất bại, tên loại phòng này đã tồn tại", code = false });
            }
            if (string.IsNullOrEmpty(lp.GhiChu))
            {
                lp.GhiChu = "Không có";
            }
            lp.NgayTao = DateTime.Now;
            lp.NgayCapNhat = DateTime.Now;
            db.LoaiPhongs.InsertOnSubmit(lp);
            db.SubmitChanges();

            NguoiDung kh_insert = (NguoiDung)Session["Admin"];
            NhatKyHoatDong nkhd = new NhatKyHoatDong();
            nkhd.TenDangNhap = kh_insert.TenDangNhap;
            nkhd.HoatDong = "Thêm";
            nkhd.ChiTietHoatDong = "Thêm mới loại phòng";
            nkhd.NgayHoatDong = DateTime.Now;
            db.NhatKyHoatDongs.InsertOnSubmit(nkhd);
            db.SubmitChanges();
            return Json(new { Message = "Thêm mới thành công", code = true });
        }



        [HttpPost]
        public JsonResult Update_Phong(Phong p)
        {
            try
            {
                var get_data = db.Phongs.SingleOrDefault(c => c.MaPhong == p.MaPhong);
                get_data.TenPhong = p.TenPhong;
                get_data.MaKV = p.MaKV;
                get_data.MaLP = p.MaLP;
                get_data.GhiChu = p.GhiChu;
                get_data.NgayCapNhat = DateTime.Now;
                db.SubmitChanges();

                NguoiDung kh_insert = (NguoiDung)Session["Admin"];
                NhatKyHoatDong nkhd = new NhatKyHoatDong();
                nkhd.TenDangNhap = kh_insert.TenDangNhap;
                nkhd.HoatDong = "Sửa";
                nkhd.ChiTietHoatDong = "Sửa phòng có ID là: " + p.MaPhong;
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
        public JsonResult Update_KhuVucPhong(KhuVucPhong kvp)
        {
            try
            {
                var get_data = db.KhuVucPhongs.SingleOrDefault(c => c.MaKV == kvp.MaKV);
                get_data.TenKV = kvp.TenKV;
                get_data.GhiChu = kvp.GhiChu;
                get_data.NgayCapNhat = DateTime.Now;
                db.SubmitChanges();

                NguoiDung kh_insert = (NguoiDung)Session["Admin"];
                NhatKyHoatDong nkhd = new NhatKyHoatDong();
                nkhd.TenDangNhap = kh_insert.TenDangNhap;
                nkhd.HoatDong = "Sửa";
                nkhd.ChiTietHoatDong = "Sửa khu vực phòng có ID là: " + kvp.MaKV;
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
        public JsonResult Update_LoaiPhong(LoaiPhong lp)
        {
            try
            {
                var get_data = db.LoaiPhongs.SingleOrDefault(c => c.MaLP == lp.MaLP);
                get_data.TenLP = lp.TenLP;
                get_data.GhiChu = lp.GhiChu;
                get_data.NgayCapNhat = DateTime.Now;
                db.SubmitChanges();

                NguoiDung kh_insert = (NguoiDung)Session["Admin"];
                NhatKyHoatDong nkhd = new NhatKyHoatDong();
                nkhd.TenDangNhap = kh_insert.TenDangNhap;
                nkhd.HoatDong = "Sửa";
                nkhd.ChiTietHoatDong = "Sửa loại phòng có ID là: " + lp.MaLP;
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
        public JsonResult Delete_Phong(Phong p)
        {
            try
            {
                var get_data = db.Phongs.SingleOrDefault(c => c.MaPhong == p.MaPhong);
                db.Phongs.DeleteOnSubmit(get_data);
                db.SubmitChanges();

                NguoiDung kh_insert = (NguoiDung)Session["Admin"];
                NhatKyHoatDong nkhd = new NhatKyHoatDong();
                nkhd.TenDangNhap = kh_insert.TenDangNhap;
                nkhd.HoatDong = "Xóa";
                nkhd.ChiTietHoatDong = "Xóa phòng có ID là: " + p.MaPhong;
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

        [HttpPost]
        public JsonResult Delete_KhuVucPhong(KhuVucPhong kvp)
        {
            try
            {
                var get_data = db.KhuVucPhongs.SingleOrDefault(c => c.MaKV == kvp.MaKV);
                db.KhuVucPhongs.DeleteOnSubmit(get_data);
                db.SubmitChanges();

                NguoiDung kh_insert = (NguoiDung)Session["Admin"];
                NhatKyHoatDong nkhd = new NhatKyHoatDong();
                nkhd.TenDangNhap = kh_insert.TenDangNhap;
                nkhd.HoatDong = "Xóa";
                nkhd.ChiTietHoatDong = "Xóa khu vực phòng có ID là: " + kvp.MaKV;
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


        [HttpPost]
        public JsonResult Delete_LoaiPhong(LoaiPhong lp)
        {
            try
            {
                var get_data = db.LoaiPhongs.SingleOrDefault(c => c.MaLP == lp.MaLP);
                db.LoaiPhongs.DeleteOnSubmit(get_data);
                db.SubmitChanges();

                NguoiDung kh_insert = (NguoiDung)Session["Admin"];
                NhatKyHoatDong nkhd = new NhatKyHoatDong();
                nkhd.TenDangNhap = kh_insert.TenDangNhap;
                nkhd.HoatDong = "Xóa";
                nkhd.ChiTietHoatDong = "Xóa loại phòng có ID là: " + lp.MaLP;
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