using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClinicProject.Code
{
    public class EmployeeSession
    {
        private int Id { get; set; }
        private string HoTen { get; set; }
        private int Quyen { get; set; }
        public EmployeeSession(int id, string hoten, int quyen)
        {
            this.Id = id;
            this.HoTen = hoten;
            this.Quyen = quyen;
        }

        public int GetRight()
        {
            return this.Quyen;
        }

        public int GetID()
        {
            return this.Id;
        }

        public string GetName()
        {
            return this.HoTen;
        }
    }
}