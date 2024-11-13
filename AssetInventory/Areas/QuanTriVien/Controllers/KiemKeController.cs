using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AssetInventory.Models;

namespace AssetInventory.Areas.QuanTriVien.Controllers
{
    public class KiemKeController : Controller
    {
        AIDataContext db = new AIDataContext();

        // GET: QuanTriVien/KiemKe
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

        public ActionResult ChiTietPhieuKiemKe(int MaPhieu)
        {
            if (Session["Admin"] == null || Session["Admin"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "TrangChu");
            }
            else
            {

                var check_phieukiemke = from s in db.PhieuKiemKes
                                        where s.MaPhieu == MaPhieu
                                        select s;
                if (check_phieukiemke.Count() <= 0)
                {
                    return RedirectToAction("Index");
                }else
                {
                    var get = db.PhieuKiemKes.SingleOrDefault(c => c.MaPhieu == MaPhieu);
                    if (get.TrangThai == 0)
                    {
                        return View();
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }

                }
            }
        }

        public ActionResult Invoice(int MaPhieu)
        {
            if (Session["Admin"] == null || Session["Admin"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "TrangChu");
            }
            else
            {

                var check_phieukiemke = from s in db.PhieuKiemKes
                                        where s.MaPhieu == MaPhieu
                                        select s;
                if (check_phieukiemke.Count() <= 0)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    var get = db.PhieuKiemKes.SingleOrDefault(c => c.MaPhieu == MaPhieu);
                    if (get.TrangThai == 1)
                    {
                        return View();
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }

                }
            }
        }

        public ActionResult Invoice_2(int MaPhieu)
        {
            if (Session["Admin"] == null || Session["Admin"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "TrangChu");
            }
            else
            {

                var check_phieukiemke = from s in db.PhieuKiemKes
                                        where s.MaPhieu == MaPhieu
                                        select s;
                if (check_phieukiemke.Count() <= 0)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    var get = db.PhieuKiemKes.SingleOrDefault(c => c.MaPhieu == MaPhieu);
                    if (get.TrangThai == 1)
                    {
                        return View();
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }

                }
            }
        }



        [HttpGet]
        public JsonResult Select_PhieuKiemKe()
        {
            var get_data = from s in db.PhieuKiemKes.OrderByDescending(a => a.MaPhieu)
                           join p in db.Phongs on s.MaPhong equals p.MaPhong
                           select new { s.MaPhieu, s.MaPhong, s.GhiChu, s.TrangThai, s.NgayCapNhat, s.NgayTao, p.TenPhong};
            return Json(new { data = get_data }, JsonRequestBehavior.AllowGet);
        }

      




        [HttpPost]
        public JsonResult Insert_PhieuKiemKe(PhieuKiemKe kk)
        {
            var check_phieukiemke = from s in db.PhieuKiemKes
                           where s.MaPhong == kk.MaPhong
                           select s;
            var check_phong = from s in db.Phongs
                           where s.MaPhong == kk.MaPhong
                           select s;
            var check_phanbo = from s in db.PhanBos
                              where s.MaPhong == kk.MaPhong
                              select s;
            if (check_phong.Count() <= 0)
            {
                return Json(new { Message = "Không có Phòng này!!!", code = -1 });
            }else if (check_phanbo.Count() <= 0)
            {
                return Json(new { Message = "Phòng này hiện chưa có tài sản, nên không thể kiểm kê!!!", code = -1 });
            }
            else if (check_phieukiemke.Count() <= 0) // nếu chưa có 
            {
                try
                {
                    // insert vào phân bố
                    kk.TrangThai = 0;
                    kk.NgayTao = DateTime.Now;
                    kk.NgayCapNhat = DateTime.Now;
                    if (string.IsNullOrEmpty(kk.GhiChu))
                    {
                        kk.GhiChu = "Không có";
                    }
                    db.PhieuKiemKes.InsertOnSubmit(kk);
                    db.SubmitChanges();
                    Insert_ChiTietPhieuKiem(kk.MaPhieu, kk.MaPhong);

                    NguoiDung kh_insert = (NguoiDung)Session["Admin"];
                    NhatKyHoatDong nkhd = new NhatKyHoatDong();
                    nkhd.TenDangNhap = kh_insert.TenDangNhap;
                    nkhd.HoatDong = "Thêm";
                    nkhd.ChiTietHoatDong = "Thêm mới phiếu kiểm kê";
                    nkhd.NgayHoatDong = DateTime.Now;
                    db.NhatKyHoatDongs.InsertOnSubmit(nkhd);
                    db.SubmitChanges();

                    return Json(new { Message = "Thêm mới thành công", code = 200 });
                }
                catch (Exception ex)
                {
                    return Json(new { Message = ex.Message, code = -1 });
                }
            }else
            {

                if (check_phieukiemke.FirstOrDefault().TrangThai == 0)
                {
                    return Json(new { Message = "Phòng này có phiếu kiểm kê chưa hoàn thành", code = -1 });
                }
                else
                {
                    // insert vào phiếu kiểm
                    kk.TrangThai = 0;
                    kk.NgayTao = DateTime.Now;
                    kk.NgayCapNhat = DateTime.Now;
                    if (string.IsNullOrEmpty(kk.GhiChu))
                    {
                        kk.GhiChu = "Không có";
                    }
                    db.PhieuKiemKes.InsertOnSubmit(kk);
                    db.SubmitChanges();
                    // insert vào phiếu kiểm


                    Insert_ChiTietPhieuKiem(kk.MaPhieu, kk.MaPhong);
                    return Json(new { Message = "Thêm mới thành công", code = 200 });
                }


            }
             
        }


        public bool Insert_ChiTietPhieuKiem(int MaPhieu, int MaPhong)
        {
            var select_PhanBo_By_MaPhong = from s in db.PhanBos
                                    where s.MaPhong == MaPhong
                                    join p in db.TaiSans on s.MaTS equals p.MaTS
                                    join p1 in db.NhomTaiSans on p.MaNhomTS equals p1.MaNhomTS 
                                    select new
                                    {
                                        p.MaNhomTS,
                                        p1.TenNhomTS,
                                        p.TenTS,
                                        p.GiaTri,
                                        s.SoLuong,
                                        p.HangSanXuat,
                                        p.NuocSanXuat,
                                        p.NamSanXuat,
                                    };
            var check_phieukiemke = from s in db.PhieuKiemKes
                                    where s.MaPhieu == MaPhieu
                                    select s;
            var check_chitiet_phieukiemke = from s in db.ChiTietPhieuKiemKes
                                    where s.MaPhieu == MaPhieu
                                    select s;
            if (check_phieukiemke.Count() >= 1)
            {
                if (check_phieukiemke.FirstOrDefault().TrangThai != 0)
                {
                    return true;
                }else if (check_phieukiemke.FirstOrDefault().TrangThai == 0 && check_chitiet_phieukiemke.Count() >= 1)
                {
                    return true;
                }
                else if (check_phieukiemke.FirstOrDefault().TrangThai == 0)
                {
                    for (int i = 0; i < select_PhanBo_By_MaPhong.Count(); i++)
                    {
                        ChiTietPhieuKiemKe inset_ctpkk = new ChiTietPhieuKiemKe();
                        inset_ctpkk.MaPhieu = MaPhieu;
                        inset_ctpkk.TenTS = select_PhanBo_By_MaPhong.Skip(i).FirstOrDefault().TenTS;
                        inset_ctpkk.TenNhomTS = select_PhanBo_By_MaPhong.Skip(i).FirstOrDefault().TenNhomTS;
                        inset_ctpkk.GiaTri = select_PhanBo_By_MaPhong.Skip(i).FirstOrDefault().GiaTri;
                        inset_ctpkk.SoLuong = select_PhanBo_By_MaPhong.Skip(i).FirstOrDefault().SoLuong;
                        inset_ctpkk.SoLuongThucTe = 0;
                        inset_ctpkk.ConTot = 0;
                        inset_ctpkk.KemPC = 0;
                        inset_ctpkk.MatPC = 0;
                        inset_ctpkk.HangSanXuat = select_PhanBo_By_MaPhong.Skip(i).FirstOrDefault().HangSanXuat;
                        inset_ctpkk.NamSanXuat = select_PhanBo_By_MaPhong.Skip(i).FirstOrDefault().NamSanXuat;
                        inset_ctpkk.NuocSanXuat = select_PhanBo_By_MaPhong.Skip(i).FirstOrDefault().NuocSanXuat;
                        inset_ctpkk.GhiChu = "Không có";
                        inset_ctpkk.NgayCapNhat = DateTime.Now;
                        inset_ctpkk.NgayTao = DateTime.Now;
                        db.ChiTietPhieuKiemKes.InsertOnSubmit(inset_ctpkk);
                        db.SubmitChanges();
                    }
                    return true;
                }
                else
                {
                    return true;
                }
                
            }
            else
            {
                return false;
            }
            
            

        }


        [HttpGet]
        public JsonResult Select_ChiTietPhieuKiemKe(int MaPhieu)
        {
            var get_data = from s in db.ChiTietPhieuKiemKes.OrderByDescending(a => a.TenNhomTS)
                           where s.MaPhieu == MaPhieu
                           select new { 
                               s.MaCTPKK,
                               s.TenNhomTS,
                               s.TenTS,
                               s.SoLuong,
                               s.SoLuongThucTe,
                               s.MatPC,
                               s.ConTot,
                               s.KemPC,
                               s.GhiChu,
                               s.NgayCapNhat,
                               s.NgayTao
                           };
            return Json(new { data = get_data }, JsonRequestBehavior.AllowGet);
        }






        [HttpPost]
        public JsonResult Update_ChiTiet_PhieuKiemKe(ChiTietPhieuKiemKe ctpkk)
        {
            var chitiet_phieukiemke = from s in db.ChiTietPhieuKiemKes
                                    where s.MaPhieu == ctpkk.MaPhieu
                                    where s.MaCTPKK == ctpkk.MaCTPKK
                                    select s;
            if (ctpkk.SoLuongThucTe < 0 || ctpkk.ConTot < 0 || ctpkk.KemPC < 0 || ctpkk.MatPC < 0)
            {
                return Json(new { Message = "Update thất bại", code = false });

            }
            else
            {
                chitiet_phieukiemke.FirstOrDefault().SoLuongThucTe = ctpkk.SoLuongThucTe;
                chitiet_phieukiemke.FirstOrDefault().ConTot = ctpkk.ConTot;
                chitiet_phieukiemke.FirstOrDefault().KemPC = ctpkk.KemPC;
                chitiet_phieukiemke.FirstOrDefault().MatPC = ctpkk.MatPC;
                chitiet_phieukiemke.FirstOrDefault().GhiChu = ctpkk.GhiChu;
                db.SubmitChanges();

                NguoiDung kh_insert = (NguoiDung)Session["Admin"];
                NhatKyHoatDong nkhd = new NhatKyHoatDong();
                nkhd.TenDangNhap = kh_insert.TenDangNhap;
                nkhd.HoatDong = "Sửa";
                nkhd.ChiTietHoatDong = "Lưu tạm phiếu kiểm kê có ID là: " + ctpkk.MaPhieu;
                nkhd.NgayHoatDong = DateTime.Now;
                db.NhatKyHoatDongs.InsertOnSubmit(nkhd);
                db.SubmitChanges();
                return Json(new { Message = "Update thành công", code = true });

            }





        }

        [HttpPost]
        public JsonResult Update_PhieuKiemKe_HoanTat(PhieuKiemKe ctpkk)
        {
            try
            {
                var cd = db.PhieuKiemKes.SingleOrDefault(c => c.MaPhieu == ctpkk.MaPhieu);
                cd.TrangThai = 1;
                db.SubmitChanges();

                NguoiDung kh_insert = (NguoiDung)Session["Admin"];
                NhatKyHoatDong nkhd = new NhatKyHoatDong();
                nkhd.TenDangNhap = kh_insert.TenDangNhap;
                nkhd.HoatDong = "Sửa";
                nkhd.ChiTietHoatDong = "Hoàn tất phiếu kiểm kê có ID là: " + ctpkk.MaPhieu;
                nkhd.NgayHoatDong = DateTime.Now;
                db.NhatKyHoatDongs.InsertOnSubmit(nkhd);
                db.SubmitChanges();
                return Json(new { code = 1, msg = "Sửa thành công nha :3" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 0, msg = "Sửa hổng được, hình như có lỗi á :3" + ex.Message }, JsonRequestBehavior.AllowGet);
            }





        }




    }
}