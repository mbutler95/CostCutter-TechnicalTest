using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UiApp.Classes
{
    public class Branch
    {
        public Branch(String branch_name, String postcode)
        {
            Branch_name = branch_name;
            Postcode = postcode;
        }

        public string Branch_name { get; set; }
        public string Postcode { get; set; }
    }
}
