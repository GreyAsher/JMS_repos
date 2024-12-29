using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Polished_Version.Classes
{
    public class CompanyUser 
    {
        public string CompanyName { get; set; }

        public string CompanyAddress { get; set; }

        public string CompanyPassword { get; set; }

        public string CompanyEmail { get; set; }

        public int CompanyId { get; set; }

        public string IsAccepted { get; set; }

        public string TimeCreates { get; set; }

        public string User_Type { get; set; }

        public string Photo { get; set; }

        public string Company_About { get; set; }

        public CompanyUser() { }

        public override string ToString()
        {
            return $"{CompanyName} {CompanyAddress} - {CompanyEmail}";
        }
    }
}
