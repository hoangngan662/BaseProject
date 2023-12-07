using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClinicProject.Code
{
    public class SessionHelper
    {
        public static void SetSession(PatientSession session)
        {
            HttpContext.Current.Session["PatientSession"] = session;
        }
        public static void SetSession(EmployeeSession session)
        {
            HttpContext.Current.Session["EmployeeSession"] = session;
        }

        public static PatientSession GetPatientSession()
        {
            var session = HttpContext.Current.Session["PatientSession"];
            if (session == null)
            {
                return null;
            }
            else return session as PatientSession;
        }

        public static EmployeeSession GetEmployeeSession()
        {
            var session = HttpContext.Current.Session["EmployeeSession"];
            if (session == null)
            {
                return null;
            }
            else return session as EmployeeSession;
        }
    }
}