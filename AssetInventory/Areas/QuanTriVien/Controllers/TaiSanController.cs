using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AssetInventory.Models;

namespace AssetInventory.Areas.QuanTriVien.Controllers
{
    public class TaiSanController : Controller
    {
        AIDataContext db = new AIDataContext();

        // GET: QuanTriVien/TaiSan
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
        public ActionResult XemDanhSachPhanBo()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PhanBoThietBi()
        {
            return View();
        }






        [HttpGet]
        public JsonResult Select_TongSoLuong_TaiSan()
        {
            var get_data = from s in db.TaiSans.OrderByDescending(a => a.MaTS)
                           join nts in db.NhomTaiSans on s.MaNhomTS equals nts.MaNhomTS
                           join lts in db.LoaiTaiSans on nts.MaLoaiTS equals lts.MaLoaiTS
                           select new { s.MaTS, s.MaNhomTS, nts.TenNhomTS, lts.MaLoaiTS, lts.TenLoaiTS, s.TenTS, s.GiaTri, s.SoLuong, s.HangSanXuat, s.NamSanXuat, s.NuocSanXuat, s.GhiChu, s.NgayCapNhat };
            return Json(new {code = true, SoLuong = get_data.Count() }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Select_TaiSan()
        {
            var get_data = from s in db.TaiSans.OrderByDescending(a => a.MaTS)
                           join nts in db.NhomTaiSans on s.MaNhomTS equals nts.MaNhomTS
                           join lts in db.LoaiTaiSans on nts.MaLoaiTS equals lts.MaLoaiTS
                           select new { s.MaTS, s.MaNhomTS, nts.TenNhomTS, lts.MaLoaiTS, lts.TenLoaiTS, s.TenTS, s.GiaTri, s.SoLuongChinh, s.SoLuong, s.HangSanXuat, s.NamSanXuat, s.NuocSanXuat, s.GhiChu, s.NgayCapNhat };
            return Json(new { data = get_data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Select_NhomTaiSan()
        {
            var get_data = from s in db.NhomTaiSans.OrderByDescending(a => a.MaNhomTS)
                           join lts in db.LoaiTaiSans on s.MaLoaiTS equals lts.MaLoaiTS
                           select new { s.MaLoaiTS, s.MaNhomTS, lts.TenLoaiTS, s.TenNhomTS, s.GhiChu, s.NgayCapNhat };
            return Json(new { data = get_data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Select_LoaiTaiSan()
        {
            var get_data = from s in db.LoaiTaiSans.OrderByDescending(a => a.MaLoaiTS)
                           select new { s.MaLoaiTS, s.TenLoaiTS, s.GhiChu, s.NgayCapNhat };
            return Json(new { data = get_data }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult Select_TaiSan_By_MaTS(int MaTS)
        {
            var get_data = from s in db.TaiSans.OrderByDescending(a => a.MaTS)
                           join nts in db.NhomTaiSans on s.MaNhomTS equals nts.MaNhomTS
                           join lts in db.LoaiTaiSans on nts.MaLoaiTS equals lts.MaLoaiTS
                           where s.MaTS == MaTS
                           select new { s.MaTS, s.MaNhomTS, nts.TenNhomTS, lts.MaLoaiTS, lts.TenLoaiTS, s.TenTS, s.GiaTri, s.SoLuongChinh, s.SoLuong, s.HangSanXuat, s.NamSanXuat, s.NuocSanXuat, s.GhiChu, s.NgayCapNhat };
            return Json(new { code = true, data = get_data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Select_NhomTaiSan_By_MaNhomTS(int MaNhomTS)
        {
            var get_data = from s in db.NhomTaiSans.OrderByDescending(a => a.MaNhomTS)
                           join lts in db.LoaiTaiSans on s.MaLoaiTS equals lts.MaLoaiTS
                           where s.MaNhomTS == MaNhomTS
                           select new { s.MaLoaiTS, s.MaNhomTS, lts.TenLoaiTS, s.TenNhomTS, s.GhiChu, s.NgayCapNhat };
            return Json(new { code = true, data = get_data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Select_LoaiTaiSan_By_MaLoaiTS(int MaLoaiTS)
        {
            var get_data = from s in db.LoaiTaiSans.OrderByDescending(a => a.MaLoaiTS)
                           where s.MaLoaiTS == MaLoaiTS
                           select new { s.MaLoaiTS, s.TenLoaiTS, s.GhiChu, s.NgayCapNhat };
            return Json(new { code = true, data = get_data }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult Insert_TaiSan(TaiSan ts)
        {
            var check_taisan = from s in db.TaiSans.OrderByDescending(a => a.MaTS)
                              where s.TenTS == ts.TenTS
                              select s;
            if (check_taisan.Count() >= 1)
            {
                return Json(new { Message = "Thêm mới tài sản thất bại, tên tài sản này đã tồn tại", code = false });
            }
            if (string.IsNullOrEmpty(ts.GhiChu))
            {
                ts.GhiChu = "Không có";
            }
            ts.SoLuong = ts.SoLuongChinh;
            ts.NgayTao = DateTime.Now;
            ts.NgayCapNhat = DateTime.Now;
            db.TaiSans.InsertOnSubmit(ts);
            db.SubmitChanges();

            NguoiDung kh_insert = (NguoiDung)Session["Admin"];
            NhatKyHoatDong nkhd = new NhatKyHoatDong();
            nkhd.TenDangNhap = kh_insert.TenDangNhap;
            nkhd.HoatDong = "Thêm";
            nkhd.ChiTietHoatDong = "Thêm mới tài sản";
            nkhd.NgayHoatDong = DateTime.Now;
            db.NhatKyHoatDongs.InsertOnSubmit(nkhd);
            db.SubmitChanges();
            return Json(new { Message = "Thêm mới thành công", code = true });
        }


        [HttpPost]
        public JsonResult Insert_NhomTaiSan(NhomTaiSan nts)
        {
            var check_nhomtaisan = from s in db.NhomTaiSans.OrderByDescending(a => a.MaNhomTS)
                               where s.TenNhomTS == nts.TenNhomTS
                               select s;
            if (check_nhomtaisan.Count() >= 1)
            {
                return Json(new { Message = "Thêm mới nhóm tài sản thất bại, tên nhóm tài sản này đã tồn tại", code = false });
            }
            if (string.IsNullOrEmpty(nts.GhiChu))
            {
                nts.GhiChu = "Không có";
            }
            nts.NgayTao = DateTime.Now;
            nts.NgayCapNhat = DateTime.Now;
            db.NhomTaiSans.InsertOnSubmit(nts);
            db.SubmitChanges();

            NguoiDung kh_insert = (NguoiDung)Session["Admin"];
            NhatKyHoatDong nkhd = new NhatKyHoatDong();
            nkhd.TenDangNhap = kh_insert.TenDangNhap;
            nkhd.HoatDong = "Thêm";
            nkhd.ChiTietHoatDong = "Thêm mới nhóm tài sản";
            nkhd.NgayHoatDong = DateTime.Now;
            db.NhatKyHoatDongs.InsertOnSubmit(nkhd);
            db.SubmitChanges();
            return Json(new { Message = "Thêm mới thành công", code = true });
        }


        [HttpPost]
        public JsonResult Insert_LoaiTaiSan(LoaiTaiSan lts)
        {
            var check_loaitaisan = from s in db.LoaiTaiSans.OrderByDescending(a => a.MaLoaiTS)
                                   where s.TenLoaiTS == lts.TenLoaiTS
                                   select s;
            if (check_loaitaisan.Count() >= 1)
            {
                return Json(new { Message = "Thêm mới loại tài sản thất bại, tên loại tài sản này đã tồn tại", code = false });
            }
            if (string.IsNullOrEmpty(lts.GhiChu))
            {
                lts.GhiChu = "Không có";
            }
            lts.NgayTao = DateTime.Now;
            lts.NgayCapNhat = DateTime.Now;
            db.LoaiTaiSans.InsertOnSubmit(lts);
            db.SubmitChanges();

            NguoiDung kh_insert = (NguoiDung)Session["Admin"];
            NhatKyHoatDong nkhd = new NhatKyHoatDong();
            nkhd.TenDangNhap = kh_insert.TenDangNhap;
            nkhd.HoatDong = "Thêm";
            nkhd.ChiTietHoatDong = "Thêm mới loại tài sản";
            nkhd.NgayHoatDong = DateTime.Now;
            db.NhatKyHoatDongs.InsertOnSubmit(nkhd);
            db.SubmitChanges();
            return Json(new { Message = "Thêm mới thành công", code = true });
        }





        [HttpPost]
        public JsonResult Update_TaiSan(TaiSan ts)
        {
            try
            {
                var get_data = db.TaiSans.SingleOrDefault(c => c.MaTS == ts.MaTS);
                get_data.TenTS = ts.TenTS;
                get_data.HangSanXuat = ts.HangSanXuat;
                get_data.NamSanXuat = ts.NamSanXuat;
                get_data.NuocSanXuat = ts.NuocSanXuat;
                if (string.IsNullOrEmpty(ts.GhiChu))
                {
                    get_data.GhiChu = "Không có";
                }
                else
                {
                    get_data.GhiChu = ts.GhiChu;
                }
                get_data.NgayCapNhat = DateTime.Now;
                get_data.NgayTao = DateTime.Now;
                db.SubmitChanges();

                NguoiDung kh_insert = (NguoiDung)Session["Admin"];
                NhatKyHoatDong nkhd = new NhatKyHoatDong();
                nkhd.TenDangNhap = kh_insert.TenDangNhap;
                nkhd.HoatDong = "Sửa";
                nkhd.ChiTietHoatDong = "Sửa tài sản có ID là: " + ts.MaTS;
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
        public JsonResult Update_NhomTaiSan(NhomTaiSan nts)
        {
            try
            {
                var get_data = db.NhomTaiSans.SingleOrDefault(c => c.MaNhomTS == nts.MaNhomTS);
                get_data.MaLoaiTS = nts.MaLoaiTS;
                get_data.TenNhomTS = nts.TenNhomTS;
                if (string.IsNullOrEmpty(nts.GhiChu))
                {
                    get_data.GhiChu = "Không có";
                }else
                {
                    get_data.GhiChu = nts.GhiChu;
                }
                get_data.NgayCapNhat = DateTime.Now;
                get_data.NgayTao = DateTime.Now;
                db.SubmitChanges();

                NguoiDung kh_insert = (NguoiDung)Session["Admin"];
                NhatKyHoatDong nkhd = new NhatKyHoatDong();
                nkhd.TenDangNhap = kh_insert.TenDangNhap;
                nkhd.HoatDong = "Sửa";
                nkhd.ChiTietHoatDong = "Sửa nhóm tài sản có ID là: " + nts.MaNhomTS;
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
        public JsonResult Update_LoaiTaiSan(LoaiTaiSan lts)
        {
            try
            {
                var get_data = db.LoaiTaiSans.SingleOrDefault(c => c.MaLoaiTS == lts.MaLoaiTS);
                get_data.TenLoaiTS = lts.TenLoaiTS;
                if (string.IsNullOrEmpty(lts.GhiChu))
                {
                    get_data.GhiChu = "Không có";
                }else
                {
                    get_data.GhiChu = lts.GhiChu;
                }
                get_data.NgayCapNhat = DateTime.Now;
                get_data.NgayTao = DateTime.Now;
                db.SubmitChanges();

                NguoiDung kh_insert = (NguoiDung)Session["Admin"];
                NhatKyHoatDong nkhd = new NhatKyHoatDong();
                nkhd.TenDangNhap = kh_insert.TenDangNhap;
                nkhd.HoatDong = "Sửa";
                nkhd.ChiTietHoatDong = "Sửa loại tài sản có ID là: " + lts.MaLoaiTS;
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
        public JsonResult Delete_TaiSan(TaiSan ts)
        {
            try
            {
                var get_data = db.TaiSans.SingleOrDefault(c => c.MaTS == ts.MaTS);
                db.TaiSans.DeleteOnSubmit(get_data);
                db.SubmitChanges();

                NguoiDung kh_insert = (NguoiDung)Session["Admin"];
                NhatKyHoatDong nkhd = new NhatKyHoatDong();
                nkhd.TenDangNhap = kh_insert.TenDangNhap;
                nkhd.HoatDong = "Xóa";
                nkhd.ChiTietHoatDong = "Xóa tài sản có ID là: " + ts.MaTS;
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
        public JsonResult Delete_NhomTaiSan(NhomTaiSan nts)
        {
            try
            {
                var get_data = db.NhomTaiSans.SingleOrDefault(c => c.MaNhomTS == nts.MaNhomTS);
                db.NhomTaiSans.DeleteOnSubmit(get_data);
                db.SubmitChanges();

                NguoiDung kh_insert = (NguoiDung)Session["Admin"];
                NhatKyHoatDong nkhd = new NhatKyHoatDong();
                nkhd.TenDangNhap = kh_insert.TenDangNhap;
                nkhd.HoatDong = "Xóa";
                nkhd.ChiTietHoatDong = "Xóa nhóm tài sản có ID là: " + nts.MaNhomTS;
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
        public JsonResult Delete_LoaiTaiSan(LoaiTaiSan lts)
        {
            try
            {
                var get_data = db.LoaiTaiSans.SingleOrDefault(c => c.MaLoaiTS == lts.MaLoaiTS);
                db.LoaiTaiSans.DeleteOnSubmit(get_data);
                db.SubmitChanges();

                NguoiDung kh_insert = (NguoiDung)Session["Admin"];
                NhatKyHoatDong nkhd = new NhatKyHoatDong();
                nkhd.TenDangNhap = kh_insert.TenDangNhap;
                nkhd.HoatDong = "Xóa";
                nkhd.ChiTietHoatDong = "Xóa loại tài sản có ID là: " + lts.MaLoaiTS;
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





        [HttpGet]
        public JsonResult Select_PhanBo()
        {
            var get_data = from s in db.PhanBos
                           group s by s.MaPhong into g
                           join p1 in db.Phongs on g.First().MaPhong equals p1.MaPhong

                           select new { MaPhong = g.First().MaPhong, SoLuong = g.Sum(pc => pc.SoLuong), TenPhong = p1.TenPhong, NgayCapNhat = g.First().NgayCapNhat, NgayTao = g.First().NgayTao };

            return Json(new { data = get_data.OrderByDescending(s => s.NgayCapNhat) }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Select_PhanBo_By_MaPhong(int MaPhong)
        {
            var get_data = from s in db.PhanBos
                           where s.MaPhong == MaPhong
                           join ts1 in db.TaiSans on s.MaTS equals ts1.MaTS
                           join nts in db.NhomTaiSans on ts1.MaNhomTS equals nts.MaNhomTS
                           select new
                           {
                               s.MaTS,
                               ts1.MaNhomTS,
                               nts.TenNhomTS,
                               ts1.TenTS,
                               ts1.GiaTri,
                               s.SoLuong,
                               ts1.HangSanXuat,
                               ts1.NamSanXuat,
                               ts1.NuocSanXuat,
                               s.GhiChu,
                               s.NgayCapNhat
                           };
            return Json(new { data = get_data.OrderByDescending(s => s.NgayCapNhat) }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Insert_PhanBo_SoLuong_One(PhanBo pb)
        {
            var check_is = from s in db.PhanBos
                           where s.MaTS == pb.MaTS
                           where s.MaPhong == pb.MaPhong
                           select s; // kiểm tra TS đã có trong phòng chưa

            if (check_is.Count() <= 0) // nếu chưa có 
            {
                try
                {
                    // update lại tài sản
                    var ts = db.TaiSans.SingleOrDefault(s => s.MaTS == pb.MaTS);
                    if (ts.SoLuong >= 1)
                    {
                        if (pb.SoLuong > ts.SoLuong)
                        {
                            return Json(new { Message = "Số lượng không hợp lệ", code = false });
                        }
                        ts.SoLuong -= pb.SoLuong;
                        db.SubmitChanges();

                        // insert vào phân bố
                        NguoiDung kh_insert = (NguoiDung)Session["Admin"];

                        pb.MaND = kh_insert.MaND;
                        pb.NgayTao = DateTime.Now;
                        pb.NgayCapNhat = DateTime.Now;
                        if (string.IsNullOrEmpty(pb.GhiChu))
                        {
                            pb.GhiChu = "Không có";
                        }
                        db.PhanBos.InsertOnSubmit(pb);
                        db.SubmitChanges();

                        NhatKyHoatDong nkhd = new NhatKyHoatDong();
                        nkhd.TenDangNhap = kh_insert.TenDangNhap;
                        nkhd.HoatDong = "Thêm";
                        nkhd.ChiTietHoatDong = "Vừa phân bổ thiết bị vào phòng: " + pb.MaPhong + " - Mã thiết bị là: " + pb.MaTS;
                        nkhd.NgayHoatDong = DateTime.Now;
                        db.NhatKyHoatDongs.InsertOnSubmit(nkhd);
                        db.SubmitChanges();
                        return Json(new { Message = "Thêm mới thành công", code = true });
                    }
                    else
                    {
                        return Json(new { Message = "Thêm mới thất bại, do tài sản không đủ số lượng", code = true });
                    }

                }
                catch (Exception ex)
                {
                    return Json(new { Message = ex.Message, code = false });
                }
            }
            else
            { // nếu có rồi
                try
                {
                    // update lại tài sản
                    var ts = db.TaiSans.SingleOrDefault(s => s.MaTS == pb.MaTS);
                    if (ts.SoLuong >= 1)
                    {
                        if (pb.SoLuong > ts.SoLuong)
                        {
                            return Json(new { Message = "Số lượng không hợp lệ", code = false });
                        }
                        ts.SoLuong -= pb.SoLuong;
                        db.SubmitChanges();

                        NguoiDung kh_insert = (NguoiDung)Session["Admin"];

                        // update vào phân bố
                        check_is.SingleOrDefault().MaND = kh_insert.MaND;
                        check_is.SingleOrDefault().NgayCapNhat = DateTime.Now;
                        check_is.SingleOrDefault().SoLuong += pb.SoLuong;
                        if (string.IsNullOrEmpty(pb.GhiChu))
                        {
                            check_is.SingleOrDefault().GhiChu = "Không có";
                        }

                        db.SubmitChanges();

                        NhatKyHoatDong nkhd = new NhatKyHoatDong();
                        nkhd.TenDangNhap = kh_insert.TenDangNhap;
                        nkhd.HoatDong = "Sửa";
                        nkhd.ChiTietHoatDong = "Vừa thêm số lượng thiết bị thiết bị vào phòng: " + pb.MaPhong + " - Mã thiết bị là: " + pb.MaTS;
                        nkhd.NgayHoatDong = DateTime.Now;
                        db.NhatKyHoatDongs.InsertOnSubmit(nkhd);
                        db.SubmitChanges();


                        return Json(new { Message = "Thêm mới thành công", code = true });
                    }
                    else
                    {
                        return Json(new { Message = "Thêm mới thất bại, do tài sản không đủ số lượng", code = true });
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { Message = ex.Message, code = false });
                }
            }

        }

    }
}