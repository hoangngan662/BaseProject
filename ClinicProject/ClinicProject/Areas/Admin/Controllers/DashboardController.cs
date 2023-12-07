using ClinicProject.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClinicProject.Areas.Admin.Controllers
{
    public class DashboardController : Controller
    {
        Database db = new Database();
        // GET: Admin/Home
        EmployeeSession empss = SessionHelper.GetEmployeeSession();
        public ActionResult Index()
        {
            if (empss == null)
            {
                return RedirectToAction("Login");
            }
            ViewBag.c_Patient = db.BenhNhans.Count();
            ViewBag.c_Doctor = db.NhanViens.Where(e => e.ID_QuyenSD == 1 && e.NgayThoiViec == null).Count();
            ViewBag.c_Treatment = db.PhieuKhams.Count();
            return View();
        }

        public ActionResult Login()
        {
            if (empss != null)
            {
                return RedirectToAction("Index");
            }
            return View();
        }


        [HttpPost, ActionName("Login")]
        public ActionResult Login(string username, string passwd)
        {
            if (username == "")
            {
                ModelState.AddModelError("username", "Tên đăng nhập không được để trống.");
                return View("Login");
            }
            if (passwd == "")
            {
                ModelState.AddModelError("password", "Mật khẩu không được để trống.");
                ViewBag.username = username;
                return View("Login");
            }


            var nv = db.NhanViens.SingleOrDefault(s => s.TaiKhoan == username && s.MatKhau == passwd);
            if (nv != null)
            {
                SessionHelper.SetSession(new EmployeeSession(nv.ID_NhanVien, nv.HoTen, nv.ID_QuyenSD.Value));
                Session["Id"] = nv.ID_NhanVien;
                Session["Name"] = nv.HoTen;
                Session["Right"] = nv.ID_QuyenSD;
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("password", "Tên đăng nhập hoặc mật khẩu không chính xác.");
                ViewBag.username = username;
            }
            return View("Login");
        }

        public ActionResult Logout()
        {
            Session.Remove("EmployeeSession");
            return RedirectToAction("Login");
        }

    }


}