using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using MySql.Data.MySqlClient;
using UiApp.Properties;
using UiApp.Classes;

namespace UiApp
{
    public class Database
    {
        private readonly string _ConnectionString = Settings.Default.DbConnectionString;

        public MySqlConnection GetConnection => new MySqlConnection(_ConnectionString);

        Order Order_Details;

        Customer Customer_Details;

        Branch Branch_Details;

        public static void Submit(int order_number)
        {
            Database db = new Database();
            var dbConnection = new Database().GetConnection;
                dbConnection.Open();
            db.FetchOrderDetails(order_number, dbConnection);
            db.FetchCustomerDetails(db.Order_Details.Customer_number, dbConnection);
            db.FetchBranchDetails(db.Order_Details.Employee_number, dbConnection);
                dbConnection.Close();
        }

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

    }
}
