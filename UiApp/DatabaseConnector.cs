using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using Dapper;
using MySql.Data.MySqlClient;
using UiApp.Properties;
using UiApp.Classes;



namespace UiApp
{
    public class DatabaseConnector : INotifyPropertyChanged
    {
        private readonly string _ConnectionString = Settings.Default.DbConnectionString;

        public MySqlConnection GetConnection => new MySqlConnection(_ConnectionString);

        public event PropertyChangedEventHandler PropertyChanged;

        private string errormessage; 
        public string Errormessage
        {
            get { return errormessage; }
            set
            {
                errormessage = value;
                OnPropertyChanged("Errormessage");
            }
        }
        #region Combobox Updating and Populating

        private List<int> TotalOrderList = new();

        private ObservableCollection<int> _comboboxentries;
        public ObservableCollection<int> ComboBoxEntries
        {
            get { return _comboboxentries; }
            set
            {
                _comboboxentries = value;
                OnPropertyChanged("ComboBoxEntries");
            }
        }

        internal void PopulateTotalOrders()
        {
            var dbConnection = new DatabaseConnector().GetConnection;
            IEnumerable<dynamic> result;
            try
            {
                dbConnection.Open();
                var sql = "SELECT order_number from orders ORDER BY order_number";
                result = dbConnection.Query(sql).AsList();
                foreach (var row in result)
                {
                    TotalOrderList.Add((int)row.order_number);
                }
            }
            catch (MySqlException)
            {
                this.Errormessage = "Unable to esablish a Connection with the Server.";
            }
            finally
            {
                dbConnection.Close();
            }
            ////First Initialization of ComboBox////
            this.ResetComboBox();
        }

        internal void ResetComboBox()
        {
            ObservableCollection<int> tempstorage = new();
            for (int i = 0; i < 100; i++) tempstorage.Add(TotalOrderList[i]);
            ComboBoxEntries = tempstorage;
        }

        internal void UpdateComboBox(string text)
        {
            ObservableCollection<int> tempstorage = new();
            int i = 0;
            foreach (int num in TotalOrderList)
            {    
                if(i < 100) 
                { 
                    string tester = "" + num;
                    if (tester.StartsWith(text))
                    {
                        tempstorage.Add(num);
                        i++;
                    }
                }
                else
                {
                    break;
                }
            }
            ComboBoxEntries = tempstorage;
        }
        #endregion

        Order Order_Details;

        Customer Customer_Details;

        Branch Branch_Details;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public void Find(int order_number)
        {
            this.Errormessage = "";
            var dbConnection = new DatabaseConnector().GetConnection;
            try
            {
                dbConnection.Open();
                this.FetchOrderDetails(order_number, dbConnection);
                this.FetchCustomerDetails(this.Order_Details.Customer_number, dbConnection);
                this.FetchBranchDetails(this.Order_Details.Employee_number, dbConnection);
            }
            catch (MySqlException)
            {
                this.Errormessage = "Unable to esablish a Connection with the Server.";
            }
            finally
            {
                dbConnection.Close();
            }
            
        }

        internal void ApplyFilters()
        {
            throw new NotImplementedException();
        }
        #region Database Fetch Logic
        public void FetchOrderDetails(int order_number, MySqlConnection dbConnection)
        {
            var sql = "SELECT * FROM orders WHERE order_number = " + order_number;
            Order_Details = dbConnection.QuerySingle<Order>(sql);
        }

        public void FetchCustomerDetails(int customer_number, MySqlConnection dbConnection)
        {
            var sql = "SELECT * FROM customers WHERE customer_number = " + customer_number;
            Customer_Details = dbConnection.QuerySingle<Customer>(sql);
        }

        public void FetchBranchDetails(int employee_number, MySqlConnection dbConnection)
        {
            var sql = "SELECT * FROM employees WHERE employee_number = " + employee_number;
            Employee Employee_Details = dbConnection.QuerySingle<Employee>(sql);
            sql = "select * from branches where branch_name = '" + Employee_Details.Branch_name + "'";
            Branch_Details = dbConnection.QuerySingle<Branch>(sql);
        }
        #endregion

    }
}
