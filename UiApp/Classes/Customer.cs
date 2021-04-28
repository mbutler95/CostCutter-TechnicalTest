using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UiApp.Classes
{
    public class Customer
    {
        public Customer(int customer_number, string forename, string surname, string telephone_number)
        {
            Customer_number = customer_number;
            Forename = forename;
            Surname = surname;
            Telephone_number = telephone_number;
        }

        public int Customer_number { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }
        public string Telephone_number { get; set; }

    }
}
