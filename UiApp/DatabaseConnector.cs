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

        #region Property Changed Event Handling
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion

        #region Database modelling class variables & properties
        private Order _order_details;
        public Order Order_Details { get => _order_details; set { _order_details = value; OnPropertyChanged("Order_Details"); } }

        private Customer _customer_details;

        

        public Customer Customer_Details { get => _customer_details; set { _customer_details = value; OnPropertyChanged("Customer_Details"); } }

        private Branch _branch_details;
        public Branch Branch_Details { get => _branch_details; set { _branch_details = value; OnPropertyChanged("Branch_Details"); } }

        #endregion

        #region Updating Ui Binding Variables

        private string _resultsmessage;
        public string ResultsMessage { get => _resultsmessage; set { _resultsmessage = value; OnPropertyChanged("ResultsMessage"); } }
        
        private string _combobox_tooltip;
        public string ComboBox_Tooltip { get => _combobox_tooltip; set { _combobox_tooltip = value; OnPropertyChanged("ComboBox_ToolTip"); } }
        
        #region SearchErrorLabel
        private string _invalidSearchLabel;
        public string InvalidSearchLabel { get => _invalidSearchLabel; set { _invalidSearchLabel = value; OnPropertyChanged("InvalidSearchLabel"); } }

        private bool _invalidSearchImage;
        public bool InvalidSearchImage { get => _invalidSearchImage; set { _invalidSearchImage = value; OnPropertyChanged("InvalidSearchImage"); } }

        internal void InvalidSearch(string v)
        {
            InvalidSearchLabel = v;
            if (v.Length > 2)
            {
                InvalidSearchImage = true;
            }
            else InvalidSearchImage = false;
        }
        #endregion

        private string _searchLabel;
        public string SearchLabel { get => _searchLabel; set { _searchLabel = value; OnPropertyChanged("SearchLabel"); } }
        internal void UpdateSearchLabel(string v)
        {
            SearchLabel = v;
        }

        #endregion

        internal void ResetSearch()
        {
            Order_Details = null;
            Customer_Details = null;
            Branch_Details = null;
        }
        #region Combobox Updating and Populating

        private List<int> _totalOrderList;
        public List<int> TotalOrderList { get => _totalOrderList; set => _totalOrderList = value; }

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

        public void PopulateTotalOrders()
        {
            var dbConnection = new DatabaseConnector().GetConnection;
            IEnumerable<dynamic> result;
            try
            {
                dbConnection.Open();
                var sql = "SELECT order_number from orders " + Filter + " ORDER BY order_number";
                result = dbConnection.Query(sql).AsList();
                TotalOrderList = new();
                foreach (var row in result)
                {
                    TotalOrderList.Add((int)row.order_number);
                }
                if (Filter == null) ResultsMessage = $"{TotalOrderList.Count} results";
            }
            catch (MySqlException)
            {
                MainWindow.InitConnectionError();
            }
            finally
            {
                dbConnection.Close();
            }
            DefaultComboBox();
            UpdateComboBoxTooltip();
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
            if (text == "")
            {
                DefaultComboBox();
            }
            else
            {
                List<int> results = TotalOrderList.FindAll(num => num.ToString().StartsWith(text));
               foreach (int num in results)
                {
                    if (i < 200)
                    {
                        tempstorage.Add(num);
                        i++;
                    }
                    else
                    {
                        break;
                    }
                }
                ComboBoxEntries = tempstorage;
            }
            UpdateComboBoxTooltip();
            
        }
        internal void UpdateComboBoxTooltip()
        {
            ComboBox_Tooltip = $"Select an order number, displaying {ComboBoxEntries.Count} of the total {TotalOrderList.Count} results";
        }
        #endregion

        public void Find(int order_number)
        {
            if (TotalOrderList == null) PopulateTotalOrders();
            if (order_number > 0 && TotalOrderList.Contains(order_number))
            {
                var dbConnection = new DatabaseConnector().GetConnection;
                try
                {
                    dbConnection.Open();
                    FetchOrderDetails(order_number, dbConnection);
                    FetchCustomerDetails(Order_Details.Customer_number, dbConnection);
                    FetchBranchDetails(Order_Details.Employee_number, dbConnection);
                    UpdateSearchLabel("Displaying details for order number " + order_number);
                }
                catch (MySqlException)
                {
                    MainWindow.FindConnectionError(order_number);
                }
                finally
                {
                    dbConnection.Close();
                }
            }
            else
            {
                InvalidSearch("Invalid Search");
            }

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

        private string _filter;
        public string Filter { get => _filter; set => _filter = value; }
        public void ApplyFilters(bool before, bool on, bool after, DateTime selected_date)
        {
            string DateString = selected_date.ToString("yyyy-MM-dd");
            string ShortDate = selected_date.ToShortDateString();
            if (before)
            {
                Filter = $"WHERE order_date <= '{DateString}'";
                PopulateTotalOrders();
                ResultsMessage = $"{TotalOrderList.Count} results before {ShortDate}";
                ShowFilterApplied("Filters Applied");
            }
            else if (on)
            {
                Filter = $"WHERE order_date = '{DateString}'";
                PopulateTotalOrders();
                ResultsMessage = $"{TotalOrderList.Count} results on {ShortDate}";
                ShowFilterApplied("Filters Applied");
            }
            else if (after)
            {
                Filter = $"WHERE order_date >= '{DateString}'";
                PopulateTotalOrders();
                ResultsMessage = $"{TotalOrderList.Count} results after {ShortDate}";
                ShowFilterApplied("Filters Applied");
            }
            else
            {
                ShowFilterError("Please select a time filter");
            }
        }

        #region Updating Filtering Labels
        private string _filterError;
        public string FilterError { get => _filterError; set { _filterError = value; OnPropertyChanged("FilterError"); } }

        private bool _filterErrorImage;
        public bool FilterErrorImage { get => _filterErrorImage; set { _filterErrorImage = value; OnPropertyChanged("FilterErrorImage"); } }

        internal void ShowFilterError(string v)
        {
            FilterError = v;
            if (v.Length > 2)
            {
                FilterErrorImage = true;
            }
            else FilterErrorImage = false;
        }
        private string _filtersApplied;
        public string FiltersApplied { get => _filtersApplied; set { _filtersApplied = value; OnPropertyChanged("FiltersApplied"); } }

        private bool _filterSuccessImage;
        public bool FilterSuccessImage { get => _filterSuccessImage; set { _filterSuccessImage = value; OnPropertyChanged("FilterSuccessImage"); } }

        internal void ShowFilterApplied(string v)
        {
            FiltersApplied = v;
            if (v.Length > 1)
            {
                FilterSuccessImage = true;
            }
            else FilterSuccessImage = false;
        }
        #endregion



    }
}
