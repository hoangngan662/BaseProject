using ClinicProject.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClinicProject.Areas.Admin.Controllers
{
    public class EmployeeController : Controller
    {
        Database db = new Database();
        // GET: Admin/Employee
        EmployeeSession empss = SessionHelper.GetEmployeeSession();
        [HttpGet, ActionName("Index")]
        public ActionResult Index()
        {
            if(empss == null)
            {
                return RedirectToAction("Login","Dashboard");
            }            
            
            var listEmp = db.NhanViens.Where(n => n.NgayThoiViec == null).ToList();
            return View(listEmp);
        }

        [HttpGet, ActionName("Doctor")]
        public ActionResult Doctor()
        {
            var listCm = db.ChuyenMons.ToList();
            return View(listCm);
        }

        [HttpGet, ActionName("AddEmployee")]
        public ActionResult AddEmployee()
        {
            List<QuyenSD> roles = db.QuyenSDs.ToList();
            ViewBag.roles = new SelectList(roles, "ID_QuyenSD", "Quyen");
            return View();
        }

        [HttpPost, ActionName("AddEmployee")]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult AddEmployee(NhanVien model)
        {
            model.MatKhau = "123456";
            db.NhanViens.Add(model);
            db.SaveChanges();
            if(model.ID_QuyenSD == 1)
            {
                ChuyenMon cm = new ChuyenMon();
                cm.ID_NhanVien = model.ID_NhanVien;
                db.ChuyenMons.Add(cm);
                db.SaveChanges();
                return RedirectToAction("DoctorSpecialize", new { id = cm.ID_NhanVien });
            }
            return RedirectToAction("Index");
        }

        [HttpGet, ActionName("DoctorSpecialize")]
        public ActionResult DoctorSpecialize(int id)
        {
            ChuyenMon cm = db.ChuyenMons.Where(n => n.ID_NhanVien == id).FirstOrDefault();
            return View(cm);
        }

        [HttpPost, ActionName("DoctorSpecialize")]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult DoctorSpecializeSave(int id)
        {
            ChuyenMon cm = db.ChuyenMons.Where(n => n.ID_NhanVien == id).FirstOrDefault(); ;
            TryUpdateModel(cm, "", new string[] { "ChuyenKhoa", "ChucDanh", "ThongTin"});
            db.Entry(cm).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Doctor");
        }

        [HttpGet, ActionName("EditEmployee")]
        public ActionResult EditEmployee(int id)
        {
            NhanVien nv = db.NhanViens.Where(n => n.ID_NhanVien == id).FirstOrDefault();
            return View(nv);
        }

        [HttpPost, ActionName("EditEmployee")]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult EditEmployeeSave(int id)
        {
            NhanVien nv = db.NhanViens.Where(n => n.ID_NhanVien == id).FirstOrDefault(); ;
            TryUpdateModel(nv, "", new string[] { "NgaySinh", "GioiTinh", "SoDienThoai", "DiaChi", "Email", "NgayThoiViec", "HinhAnh" });
            db.Entry(nv).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("DeleteEmployee")]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteEmployee(int id)
        {
            NhanVien nv = db.NhanViens.Where(n => n.ID_NhanVien == id).SingleOrDefault();
            nv.NgayThoiViec = DateTime.Today;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}