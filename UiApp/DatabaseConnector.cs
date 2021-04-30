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
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #region Database modelling class variables & properties
        private Order _order_details;
        public Order Order_Details { get => _order_details; set { _order_details = value; OnPropertyChanged("Order_Details"); } }

        private Customer _customer_details;

        public Customer Customer_Details { get => _customer_details; set { _customer_details = value; OnPropertyChanged("Customer_Details"); } }

        private Branch _branch_details;
        public Branch Branch_Details { get => _branch_details; set { _branch_details = value; OnPropertyChanged("Branch_Details"); } }

        #endregion

        private string _filter;
        public string Filter { get => _filter; set => _filter = value; }

        private string _combobox_tooltip;
        public string ComboBox_Tooltip
        {
            get { return _combobox_tooltip; }
            set
            {
                _combobox_tooltip = value;
                OnPropertyChanged("ComboBox_ToolTip");
            }
        }

        private string _infomessage; 
        public string InfoMessage
        {
            get { return _infomessage; }
            set
            {
                _infomessage = value;
                OnPropertyChanged("InfoMessage");
            }
        }

        internal void UpdateInfoLabel(string v)
        {
            InfoMessage = v;
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
            TotalOrderList = new();
            var dbConnection = new DatabaseConnector().GetConnection;
            IEnumerable<dynamic> result;
            try
            {
                dbConnection.Open();
                var sql = "SELECT order_number from orders "+ Filter + " ORDER BY order_number";
                result = dbConnection.Query(sql).AsList();
                foreach (var row in result)
                {
                    TotalOrderList.Add((int)row.order_number);
                }
            }
            catch (MySqlException)
            {
                InfoMessage = "Unable to esablish a Connection with the Server.";
            }
            finally
            {
                dbConnection.Close();
            }
            DefaultComboBox();
            UpdateComboBoxTooltip();
        }

        internal void ResetSearch()
        {
            Order_Details = null;
            Customer_Details = null;
            Branch_Details = null;
        }

        internal void DefaultComboBox()
        {
            ObservableCollection<int> tempstorage = new();
            if (TotalOrderList.Count > 200)
            {
                for (int i = 0; i < 200; i++) tempstorage.Add(TotalOrderList[i]);
            }
            else
            {
                for (int i = 0; i < TotalOrderList.Count; i++) tempstorage.Add(TotalOrderList[i]);
            }
             ComboBoxEntries = tempstorage;
        }
        internal void UpdateComboBox(string text)
        {
            ObservableCollection<int> tempstorage = new();
            int i = 0;
            foreach (int num in TotalOrderList)
            {    
                if(i < 200) 
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
            UpdateComboBoxTooltip();
        }
        internal void UpdateComboBoxTooltip()
        {
            ComboBox_Tooltip = $"Select an order number, displaying {ComboBoxEntries.Count} of the total {TotalOrderList.Count} results";
        }
        #endregion
        
        public void Find(int order_number)
        {
            var dbConnection = new DatabaseConnector().GetConnection;
            try
            {
                dbConnection.Open();
                FetchOrderDetails(order_number, dbConnection);
                FetchCustomerDetails(Order_Details.Customer_number, dbConnection);
                FetchBranchDetails(Order_Details.Employee_number, dbConnection);
            }
            catch (MySqlException)
            {
                InfoMessage = "Unable to esablish a Connection with the Server.";
            }
            finally
            {
                dbConnection.Close();
            }
            
        }

        internal void ApplyFilters(bool before, bool after, DateTime selected_date)
        {
            if(before)
            {
                Filter = "WHERE order_date <= '" + selected_date.ToString("yyyy-MM-dd") +"'";
            }
            else if(after)
            {
                Filter = "WHERE order_date >= '" + selected_date.ToString("yyyy-MM-dd") + "'";
            }
            else
            {
                Filter = "WHERE order_date = '" + selected_date.ToString("yyyy-MM-dd") + "'";
            }
            PopulateTotalOrders();
            DefaultComboBox();
            InfoMessage = "Found " + TotalOrderList.Count + " results matching filter!";
        }
        #region Database Query Logic
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
