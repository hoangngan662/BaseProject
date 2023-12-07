using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClinicProject.Areas.Admin.Controllers
{
    public class PatientController : Controller
    {
        Database db = new Database();
        // GET: Admin/Patient
        public ActionResult Index()
        {
            var listPat = db.BenhNhans.ToList(); 
            return View(listPat);
        }

        [HttpGet, ActionName("AddPatient")]
        public ActionResult AddPatient()
        {
            return View();
        }

        [HttpPost, ActionName("AddPatient")]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult AddPatient(BenhNhan model)
        {
            model.MatKhau = "123456";
            db.BenhNhans.Add(model);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet, ActionName("PatientProfile")]
        public ActionResult PatientProfile(int id) {
            var model = db.BenhNhans.Where(b => b.ID_BenhNhan == id).FirstOrDefault();
            ViewBag.lichKham = db.LichKhams.Where(b => b.TrangThai == "P" && b.ID_BenhNhan == id).ToList();
            ViewBag.hoSo = db.PhieuKhams.Where(b => b.LichKham.ID_BenhNhan == id).ToList();
            return View(model);
        }

        public ActionResult FindByName (string tuKhoa)
        {
            var result = db.BenhNhans.Where(p => p.HoTen.ToLower().Contains(tuKhoa.ToLower())).ToList();
            return PartialView(result);
        }
    }
}