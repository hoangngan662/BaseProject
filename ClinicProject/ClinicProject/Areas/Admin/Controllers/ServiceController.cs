using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClinicProject.Areas.Admin.Controllers
{
    public class ServiceController : Controller
    {
        // GET: Admin/Service
        Database db = new Database();
        public ActionResult Index()
        {
            var lsser = db.DichVus.ToList();
            return View(lsser);
        }

        public ActionResult Medicine()
        {
            var lsme = db.Thuocs.ToList();
            return View(lsme);
        }



        public ActionResult ServiceHandle() {
            var lsser = db.ChiTietDVs.Where(s => s.KetQua == null).OrderBy(s => s.PhieuKham.ThoiGian).ToList();
            return View(lsser);
        }

        public ActionResult AddResult(int id, int id_pk)
        {
            var result = db.ChiTietDVs.Where(c => c.ID_DichVu == id && c.ID_PhieuKham == id_pk).FirstOrDefault();
            return View(result);
        }

        [HttpPost, ActionName("AddResult")]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult AddResultSave(int id, int id_pk)
        {
            var result = db.ChiTietDVs.Where(c => c.ID_DichVu == id && c.ID_PhieuKham == id_pk).FirstOrDefault();
            TryUpdateModel(result, "", new string[] { "KetQua" });
            db.Entry(result).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("ServiceHandle");
        }

        public ActionResult AddService()
        {
            return View();
        }

        [HttpPost, ActionName("AddService")]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult AddService(DichVu model)
        {
            db.DichVus.Add(model);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult AddMedicine()
        {
            return View();
        }

        [HttpPost, ActionName("AddMedicine")]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult AddMedicine(Thuoc model)
        {
            db.Thuocs.Add(model);
            db.SaveChanges();
            return RedirectToAction("Medicine");
        }


    }
}