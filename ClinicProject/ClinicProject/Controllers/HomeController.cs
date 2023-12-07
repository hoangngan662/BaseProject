using ClinicProject.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClinicProject.Controllers
{
    public class HomeController : Controller
    {
        Database db = new Database();
        PatientSession patss = SessionHelper.GetPatientSession();
        public ActionResult Index()
        {
            ViewBag.doctor = db.NhanViens.Where(e => e.QuyenSD.ID_QuyenSD == 1 && e.NgayThoiViec == null).ToList();
            ViewBag.service = db.DichVus.ToList();
            ViewBag.c_ser = db.DichVus.ToList().Count;
            ViewBag.c_case = db.PhieuKhams.ToList().Count;
            ViewBag.c_pat = db.BenhNhans.ToList().Count;
            ViewBag.c_emp = db.NhanViens.Where(e => e.NgayThoiViec == null).ToList().Count;
            return View();
        }

        public ActionResult DoctorDetail(int id)
        {
            ChuyenMon cm = db.ChuyenMons.Where(c => c.ID_NhanVien == id).FirstOrDefault();
            return View(cm);
        } 

        public ActionResult Login()
        {
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


            var bn = db.BenhNhans.SingleOrDefault(s => s.TaiKhoan == username && s.MatKhau == passwd);
            if (bn != null)
            {
                SessionHelper.SetSession(new PatientSession(bn.ID_BenhNhan, bn.HoTen));
                Session["Name"] = bn.HoTen;
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("password", "Tên đăng nhập hoặc mật khẩu không chính xác.");
                ViewBag.username = username;
            }
            return View("Login");
        }

        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost, ActionName("Signup")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Signup(BenhNhan model)
        {
            
            //Kiem tra username
            var checkUsername = db.BenhNhans.SingleOrDefault(s => s.TaiKhoan == model.TaiKhoan);
            if (checkUsername != null)
            {
                ModelState.AddModelError("TaiKhoan", "Tài khoản đã tồn tại.");
                return View("Signup");
            }
            ViewBag.Username = model.TaiKhoan;
            ViewBag.Name = model.HoTen;

            //Kiem tra ngay sinh
            //kt ngay sinh
            if (model.NgaySinh > DateTime.Today)
            {
                ModelState.AddModelError("NgaySinh", "Ngày sinh phải nhỏ hơn hôm nay");
                return View("Signup");
            }
            ViewBag.NgaySinh = model.NgaySinh;

            

            //Kiem tra sdt
            var checkPhone = db.BenhNhans.SingleOrDefault(s => s.SoDienThoai == model.SoDienThoai);
            if (checkPhone != null)
            {
                ModelState.AddModelError("SoDienThoai", "Số điện thoại đã được sử dụng.");
                return View("Signup");
            }
            ViewBag.SoDienThoai = model.SoDienThoai;

            //Kiem tra Email
            var checkEmail = db.BenhNhans.SingleOrDefault(s => s.Email == model.Email);
            if (checkEmail != null)
            {
                ModelState.AddModelError("Email", "Email đã được sử dụng.");
                return View("Signup");
            }
            if (ModelState.IsValid)
            {
                db.BenhNhans.Add(model);
                db.SaveChanges();
                return View("Login");
            }
            else
            {
                ModelState.AddModelError("", "Đăng ký không thành công, vui lòng kiểm tra lại");
                return View("Signup");
            }

        }

        public ActionResult Logout()
        {
            Session.Remove("PatientSession");
            return RedirectToAction("Index");
        }

        public ActionResult PageNotFound()
        {
            Response.StatusCode = 404;
            return View();
        }

        public ActionResult AddAppointment()
        {
            if (patss == null)
            {
                return RedirectToAction("Login");
            }
            //if(id == 0)
            //{

            //    ViewBag.ca = 1;
            //}
            //else
            //{
            //    NhanVien bs = db.NhanViens.Where(e => e.ID_NhanVien == id).FirstOrDefault();
            //    ViewBag.doctor = bs;
            //    ViewBag.ca = 2;
            //}
            List<NhanVien> ls_bs = db.NhanViens.Where(e => e.ID_QuyenSD == 1).ToList();
            ViewBag.doctor = new SelectList(ls_bs, "ID_NhanVien", "Hoten");
            return View();
        }

        [HttpPost, ActionName("AddAppointment")]
        public ActionResult AddAppointment(LichKham lk)
        {
            lk.ID_BenhNhan = patss.getID();
            lk.TrangThai = "P";
            db.LichKhams.Add(lk);
            db.SaveChanges();      
            
            return RedirectToAction("Index");
        }

        public ActionResult UserProfile()
        {
            int id = patss.getID();
            BenhNhan bn = db.BenhNhans.Where(b => b.ID_BenhNhan == id ).FirstOrDefault();
            ViewBag.lk = db.LichKhams.Where(l => l.ID_BenhNhan == id).OrderBy(l => l.ThoiGian).ToList();
            ViewBag.pk = db.PhieuKhams.Where(l => l.LichKham.ID_BenhNhan == id && l.LichKham.TrangThai=="D").OrderBy(l => l.ThoiGian).ToList();
            return View(bn);
        }

        public ActionResult Lich()
        {
            List<LichKham> lk = db.LichKhams.Where(l => l.ID_BenhNhan == 4).OrderBy(l => l.ThoiGian).ToList();
            return View(lk);
        }

    }
}