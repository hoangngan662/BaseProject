using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ClinicProject.Areas.Admin.Controllers
{
    public class AppointmentController : Controller
    {
        // GET: Admin/Appointment
        Database db = new Database();
        public ActionResult Index()
        {
            List<LichKham> ls_lk = db.LichKhams.Where(l => l.TrangThai=="P").ToList();
            foreach(var l in ls_lk)
            {
                if(Convert.ToDateTime(l.ThoiGian).Date < DateTime.Now.Date)
                {
                    LichKham lk = db.LichKhams.Where(n => n.ID_LichKham == l.ID_LichKham).FirstOrDefault();
                    lk.TrangThai = "C";
                    TryUpdateModel(lk, "", new string[] { "TrangThai" });
                    db.Entry(lk).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
            List<LichKham> ls = db.LichKhams.Where(l => l.TrangThai == "P").OrderBy(l => l.ThoiGian).ToList();
            return View(ls);
        }

        [HttpPost]
        public ActionResult FilterByStatus(string status)
        {
            var filteredData = new List<LichKham>();
            if(status == "ALL")
            {
                filteredData = db.LichKhams.OrderBy(m => m.ThoiGian).ToList();
            }
            else
            {
                filteredData = db.LichKhams.Where(m => m.TrangThai == status).OrderBy(m => m.ThoiGian).ToList();
            }
            

            return PartialView("FilterByStatus", filteredData);
        }

        public ActionResult AddAppointment()
        {
            List<NhanVien> ls_bs = db.NhanViens.Where(e => e.ID_QuyenSD == 1).ToList();
            ViewBag.doctor = new SelectList(ls_bs, "ID_NhanVien", "Hoten");
            List<BenhNhan> ls_bn = db.BenhNhans.ToList();
            ViewBag.patient = new SelectList(ls_bn, "ID_BenhNhan", "Hoten");
            return View();
        }

        [HttpPost, ActionName("AddAppointment")]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult AddAppointment(LichKham model)
        {
            model.TrangThai = "P";
            db.LichKhams.Add(model);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Detail(int id)
        {
            PhieuKham pk = new PhieuKham();
            pk.ID_LichKham = id;
            pk.ThoiGian = DateTime.Now;
            db.PhieuKhams.Add(pk);

            LichKham lk = db.LichKhams.Where(n => n.ID_LichKham == id).FirstOrDefault();
            lk.TrangThai = "PD";
            TryUpdateModel(lk, "", new string[] { "TrangThai" });
            db.Entry(lk).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return View(pk);
        }
    }
}