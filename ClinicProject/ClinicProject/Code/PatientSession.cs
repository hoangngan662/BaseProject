using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClinicProject.Code
{
    public class PatientSession
    {
        private int id { get; set; }
        private string HoTen { get; set; }
        public PatientSession(int id, string HoTen)
        {
            this.id = id;
            this.HoTen = HoTen;
        }
        public int getID()
        {
            return id;
        }

        public string getHoTen()
        {
            return HoTen;
        }
    }
}