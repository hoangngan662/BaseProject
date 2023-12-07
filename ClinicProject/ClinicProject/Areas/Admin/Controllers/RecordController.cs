using ClinicProject.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ClinicProject.Areas.Admin.Controllers
{
    public class RecordController : Controller
    {
        // GET: Admin/Record
        Database db = new Database();
        EmployeeSession empss = SessionHelper.GetEmployeeSession();
        public ActionResult Index()
        {
            if (empss == null)
            {
                return RedirectToAction("Login","Dashboard");
            }
            int id = empss.GetID();
            var ls_rc = db.PhieuKhams.Where(r => r.LichKham.NhanVien.ID_NhanVien == id && r.LichKham.TrangThai=="PD").ToList();
            return View(ls_rc);
        }

        public ActionResult Edit(int id) {
            PhieuKham pk = db.PhieuKhams.Where(p => p.ID_PhieuKham == id).FirstOrDefault();
            List<Thuoc> thuoc =  db.Thuocs.ToList();
            ViewBag.Toa = db.ChiTietToaThuocs.Where(c => c.ID_PhieuKham == id).ToList();
            ViewBag.Thuoc = new SelectList(thuoc, "ID_Thuoc", "TenThuoc");

            List<DichVu> dv = db.DichVus.ToList();
            ViewBag.CTDV = db.ChiTietDVs.Where(c => c.ID_PhieuKham == id).ToList();
            ViewBag.DVs = new SelectList(dv, "ID_DichVu", "TenDichVu");
            return View(pk);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult EditSave(int id)
        {
            PhieuKham pk = db.PhieuKhams.Where(p => p.ID_PhieuKham == id).FirstOrDefault();
            TryUpdateModel(pk, "", new string[] { "TrieuChung", "ChanDoan", "GhiChu" });
            db.Entry(pk).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpPost, ActionName("FinishRecord")]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult FinishRecord(int id)
        {
            LichKham lk = db.LichKhams.Where(n => n.PhieuKhams.FirstOrDefault().ID_PhieuKham == id).SingleOrDefault();
            lk.TrangThai = "D";
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet, ActionName("ChiTietToa")]
        public ActionResult ChiTietToa(int id, int id_pk, int soluong, string lieudung)
        {
            ChiTietToaThuoc ct = new ChiTietToaThuoc()
            {
                ID_PhieuKham = id_pk,
                ID_Thuoc = id,
                SoLuong = soluong,
                LieuDung = lieudung
            };
            db.ChiTietToaThuocs.Add(ct);
            db.SaveChanges();
            return RedirectToAction("Edit", new { id = id_pk });
        }

        [HttpGet, ActionName("ChiTietDV")]
        public ActionResult ChiTietDV(int id, int id_pk)
        {
            ChiTietDV ct = new ChiTietDV()
            {
                ID_PhieuKham = id_pk,
                ID_DichVu = id
            };
            db.ChiTietDVs.Add(ct);
            db.SaveChanges();
            return RedirectToAction("Edit", new { id = id_pk });
        }
    }
}