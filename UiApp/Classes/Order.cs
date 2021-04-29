using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace UiApp.Classes
{
    public class Order : INotifyPropertyChanged
    { 
         public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public Order(int order_number, int customer_number, int employee_number, decimal sale_price, decimal deposit, DateTime order_date)
        {
            Order_number = order_number;
            Customer_number = customer_number;
            Employee_number = employee_number;
            Sale_price = sale_price;
            Deposit = deposit;
            Order_date = order_date.ToLongDateString();
        }
        public int Order_number { get; set; }
        public int Customer_number { get; set; }
        public int Employee_number { get; set; }
        public decimal Sale_price { get; set; }
        public decimal Deposit { get; set; }
        public string Order_date { get; set; }





    }
}
