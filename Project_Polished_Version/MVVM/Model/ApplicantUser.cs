using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Polished_Version.Classes
{
    public class ApplicantUser
    {
        public int Id { get; set; }

        public string Applicant_First_Name { get; set; }

        public string Applicant_Last_Name { get; set; }

        public string Applicant_Password { get; set; }

        public string Applicant_Email { get; set; }

        public string Applicant_JobTitle { get; set; }

        public string Applicant_PhoneNumber { get; set; }

        public string Applicant_Address { get; set; }

        public string Applicant_Gender { get; set; }

        public string Applicant_Photo { get; set; }

        public string IsAccepted { get; set; }

        public string User_Type { get; set; }

        public string Applicant_Education { get; set; }
        public string Applicant_About { get; set; }
        public string Applicant_Experience { get; set; }
        public string Applicant_InfoDatePosted { get; set; }

        public ApplicantUser() { }

        public override string ToString()
        {
            return $"{Applicant_First_Name} - {Applicant_First_Name}"; 
        }
    }
}
