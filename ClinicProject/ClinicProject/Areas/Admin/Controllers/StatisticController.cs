using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClinicProject.Areas.Admin.Controllers
{
    public class StatisticController : Controller
    {
        // GET: Admin/Statistic
        Database db = new Database();
        public ActionResult Index()
        {
            //thong ke so ca kham theo thang            
            int[] cnts = new int[12];
            for (int i = 0; i < 12; i++)
            {                
                cnts[i] = db.PhieuKhams.Where(p => p.LichKham.TrangThai == "D" && EntityFunctions.TruncateTime(p.ThoiGian).Value.Month == (i + 1)).ToList().Count;
            }
            ViewBag.cnts = cnts;
                        
            //thong ke trang thai cac lich hen
            int[] cntpie = new int[3];
            cntpie[0] = db.LichKhams.Where(p => p.TrangThai == "D").ToList().Count;
            cntpie[1] = db.LichKhams.Where(p => p.TrangThai == "C").ToList().Count;
            cntpie[2] = db.LichKhams.Where(p => p.TrangThai == "P" || p.TrangThai == "PD").ToList().Count;
            ViewBag.pie = cntpie;

            ViewBag.cntNV = db.NhanViens.Where(n => n.NgayThoiViec == null).ToList().Count;
            ViewBag.cntBS = db.NhanViens.Where(n => n.NgayThoiViec == null && n.ID_QuyenSD==1).ToList().Count;
            ViewBag.cntBN = db.BenhNhans.ToList().Count;
            ViewBag.cntDV = db.DichVus.ToList().Count;

            return View();
        }

        [HttpGet, ActionName("GetData")]
        public JsonResult GetData(string start, string end)
        {
            DateTime startDate = Convert.ToDateTime(start);
            DateTime endDate = Convert.ToDateTime(end);
            var totalDays = (endDate - startDate).TotalDays + 1;
            string[] label = new string[((int)totalDays)];
            int[] cnts = new int[((int)totalDays)];

            for (var i = 0; i < ((int)totalDays); i++)
            {                
                DateTime daysAfter = startDate.AddDays(i);
                var cnt = db.PhieuKhams.Where(pk => EntityFunctions.TruncateTime(pk.ThoiGian) == daysAfter).ToList().Count;                
                label[i] = startDate.AddDays(i).Day.ToString() +"/"+ startDate.AddDays(i).Month.ToString() + "/" + startDate.AddDays(i).Year.ToString();
                cnts[i] = cnt;
            }
            return Json(new { label, cnts }, JsonRequestBehavior.AllowGet);
        }

        

    }
}