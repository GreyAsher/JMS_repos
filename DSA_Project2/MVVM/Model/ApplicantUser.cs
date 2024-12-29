using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSA_Project2.MVVM.Model
{
    public class ApplicantUser
    {
        public int Id { get; set; }

        public string First_Name { get; set; }

        public string Last_Name { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string JobTitle { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public string Gender { get; set; }

        public string Applicant_Photo { get; set; }

        public string IsAccepted { get; set; }
    }
}
