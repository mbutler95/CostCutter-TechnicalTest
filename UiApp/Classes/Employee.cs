using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UiApp.Classes
{
    public class Employee
    {
        public Employee(int employee_number, int line_manager_number, string branch_name, string forename, string surname)
        {
            Employee_number = employee_number;
            Line_manager_number = line_manager_number;
            Branch_name = branch_name;
            Forename = forename;
            Surname = surname;
        }

        public int Employee_number { get; set; }
        public int Line_manager_number { get; set; }
        public string Branch_name { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }

    }
}
