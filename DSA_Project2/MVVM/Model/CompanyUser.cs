using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSA_Project2.MVVM.Model
{
    public class CompanyUser
    {
        public string CompanyName { get; set; }

        public string CompanyAddress { get; set; }

        public string CompanyPassword { get; set; }

        public string CompanyEmail { get; set; }

        public int CompanyId { get; set; }

        public string IsAccepted { get; set; }
    }
}
