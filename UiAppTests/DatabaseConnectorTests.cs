using Microsoft.VisualStudio.TestTools.UnitTesting;
using UiApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace UiApp.Tests
{
    [TestClass()]
    public class DatabaseConnectorTests
    {
        #region Initial Population of Total List Tests
        [TestMethod()]
        public void FindPopulationTest()
        {
            DatabaseConnector db = new DatabaseConnector();
            db.Find(1);

            var dbConnection = new DatabaseConnector().GetConnection;
            dbConnection.Open();
            var sql = "SELECT * FROM orders";
            var expectedResult = dbConnection.Query(sql);
            dbConnection.Close();

            Assert.AreEqual(db.TotalOrderList.Count, expectedResult.Count());

        }

        [TestMethod()]
        public void PopulationTest()
        {
            DatabaseConnector db = new DatabaseConnector();
            db.PopulateTotalOrders();

            var dbConnection = new DatabaseConnector().GetConnection;
            dbConnection.Open();
            var sql = "SELECT * FROM orders";
            var expectedResult = dbConnection.Query(sql);
            dbConnection.Close();

            Assert.AreEqual(db.TotalOrderList.Count, expectedResult.Count());

        }
        #endregion

        #region Find Tests
        [TestMethod()]
        public void FindTest1()
        {
            DatabaseConnector db = new DatabaseConnector();
            db.Find(12238);

            var dbConnection = new DatabaseConnector().GetConnection;
            dbConnection.Open();
            var sql = @"SELECT orders.*, customers.*, employees.*, branches.*
                        From orders
                        INNER JOIN customers on orders.customer_number = customers.customer_number
                        INNER JOIN employees on orders.employee_number = employees.employee_number
                        INNER JOIN branches on employees.branch_name = branches.branch_name
                        WHERE order_number = 12238";
            dynamic expectedResult = dbConnection.QuerySingle(sql);
            dbConnection.Close();

            Assert.AreEqual(db.Order_Details.Sale_price, expectedResult.sale_price);
            Assert.AreEqual(db.Branch_Details.Branch_name, expectedResult.branch_name);
            Assert.AreEqual(db.Branch_Details.Postcode, expectedResult.postcode);

        }

        [TestMethod()]
        public void FindTest2()
        {
            DatabaseConnector db = new DatabaseConnector();
            db.Find(10515);

            var dbConnection = new DatabaseConnector().GetConnection;
            dbConnection.Open();
            var sql = @"SELECT orders.*, customers.*, employees.*, branches.*
                        From orders
                        INNER JOIN customers on orders.customer_number = customers.customer_number
                        INNER JOIN employees on orders.employee_number = employees.employee_number
                        INNER JOIN branches on employees.branch_name = branches.branch_name
                        WHERE order_number = 10515";
            dynamic expectedResult = dbConnection.QuerySingle(sql);
            dbConnection.Close();

            Assert.AreEqual(db.Customer_Details.Customer_number, expectedResult.customer_number);
            Assert.AreEqual(db.Customer_Details.Forename, expectedResult.forename);
            Assert.AreEqual(db.Customer_Details.Surname, expectedResult.surname);
        }

        [TestMethod()]
        public void FindTest3()
        {
            DatabaseConnector db = new DatabaseConnector();
            db.Find(2395);

            var dbConnection = new DatabaseConnector().GetConnection;
            dbConnection.Open();
            var sql = @"SELECT orders.*, customers.*, employees.*, branches.*
                        From orders
                        INNER JOIN customers on orders.customer_number = customers.customer_number
                        INNER JOIN employees on orders.employee_number = employees.employee_number
                        INNER JOIN branches on employees.branch_name = branches.branch_name
                        WHERE order_number = 2395";
            dynamic expectedResult = dbConnection.QuerySingle(sql);
            dbConnection.Close();

            Assert.AreEqual(db.Order_Details.Order_number, expectedResult.order_number);
            Assert.AreEqual(db.Customer_Details.Telephone_number, expectedResult.telephone_number);
            Assert.AreEqual(db.Order_Details.Deposit, expectedResult.deposit);
        }

        [TestMethod()]
        public void FindTest4()
        {
            DatabaseConnector db = new DatabaseConnector();
            db.Find(22614);

            var dbConnection = new DatabaseConnector().GetConnection;
            dbConnection.Open();
            var sql = @"SELECT orders.*, customers.*, employees.*, branches.*
                        From orders
                        INNER JOIN customers on orders.customer_number = customers.customer_number
                        INNER JOIN employees on orders.employee_number = employees.employee_number
                        INNER JOIN branches on employees.branch_name = branches.branch_name
                        WHERE order_number = 22614";
            dynamic expectedResult = dbConnection.QuerySingle(sql);
            dbConnection.Close();

            Assert.AreEqual(db.Branch_Details.Branch_name, expectedResult.branch_name);
            Assert.AreEqual(db.Customer_Details.Forename, expectedResult.forename);
            Assert.AreEqual(db.Order_Details.Deposit, expectedResult.deposit);
        }

        [TestMethod()]
        public void FindTest5()
        {
            DatabaseConnector db = new DatabaseConnector();
            db.Find(666);

            var dbConnection = new DatabaseConnector().GetConnection;
            dbConnection.Open();
            var sql = @"SELECT orders.*, customers.*, employees.*, branches.*
                        From orders
                        INNER JOIN customers on orders.customer_number = customers.customer_number
                        INNER JOIN employees on orders.employee_number = employees.employee_number
                        INNER JOIN branches on employees.branch_name = branches.branch_name
                        WHERE order_number = 666";
            dynamic expectedResult = dbConnection.QuerySingle(sql);
            dbConnection.Close();

            Assert.AreEqual(db.Order_Details.Sale_price, expectedResult.sale_price);
            Assert.AreEqual(db.Customer_Details.Customer_number, expectedResult.customer_number);
            Assert.AreEqual(db.Customer_Details.Telephone_number, expectedResult.telephone_number);
        }

        [TestMethod()]
        public void FindTest6()
        {
            DatabaseConnector db = new DatabaseConnector();
            db.Find(26111);

            var dbConnection = new DatabaseConnector().GetConnection;
            dbConnection.Open();
            var sql = @"SELECT orders.*, customers.*, employees.*, branches.*
                        From orders
                        INNER JOIN customers on orders.customer_number = customers.customer_number
                        INNER JOIN employees on orders.employee_number = employees.employee_number
                        INNER JOIN branches on employees.branch_name = branches.branch_name
                        WHERE order_number = 26111";
            dynamic expectedResult = dbConnection.QuerySingle(sql);
            dbConnection.Close();

            Assert.AreEqual(db.Branch_Details.Postcode, expectedResult.postcode);
            Assert.AreEqual(db.Order_Details.Order_number, expectedResult.order_number);
            Assert.AreEqual(db.Order_Details.Deposit, expectedResult.deposit);
        }

        #endregion

        #region Before Tests
        [TestMethod()]
        public void BeforeTest1()
        {
            DatabaseConnector db = new DatabaseConnector();
            DateTime dt = new DateTime(2013, 04, 11);
            db.ApplyFilters(true, false, false, dt);

            var dbConnection = new DatabaseConnector().GetConnection;
            dbConnection.Open();
            var sql = "SELECT * FROM orders WHERE order_date <= '2013-04-11'";
            var expectedResult = dbConnection.Query(sql);
            dbConnection.Close();

            Assert.AreEqual(db.TotalOrderList.Count, expectedResult.Count());
        }

        [TestMethod()]
        public void BeforeTest2()
        {
            DatabaseConnector db = new DatabaseConnector();
            DateTime dt = new DateTime(2014, 12, 23);
            db.ApplyFilters(true, false, false, dt);

            var dbConnection = new DatabaseConnector().GetConnection;
            dbConnection.Open();
            var sql = "SELECT * FROM orders WHERE order_date <= '2014-12-23'";
            var expectedResult = dbConnection.Query(sql);
            dbConnection.Close();

            Assert.AreEqual(db.TotalOrderList.Count, expectedResult.Count());
        }

        [TestMethod()]
        public void BeforeTest3()
        {
            DatabaseConnector db = new DatabaseConnector();
            DateTime dt = new DateTime(2010, 12, 17);
            db.ApplyFilters(true, false, false, dt);

            var dbConnection = new DatabaseConnector().GetConnection;
            dbConnection.Open();
            var sql = "SELECT * FROM orders WHERE order_date <= '2010-12-17'";
            var expectedResult = dbConnection.Query(sql);
            dbConnection.Close();

            Assert.AreEqual(db.TotalOrderList.Count, expectedResult.Count());
        }

        [TestMethod()]
        public void BeforeTest4()
        {
            DatabaseConnector db = new DatabaseConnector();
            DateTime dt = new DateTime(2011, 02, 13);
            db.ApplyFilters(true, false, false, dt);

            var dbConnection = new DatabaseConnector().GetConnection;
            dbConnection.Open();
            var sql = "SELECT * FROM orders WHERE order_date <= '2011-02-13'";
            var expectedResult = dbConnection.Query(sql);
            dbConnection.Close();

            Assert.AreEqual(db.TotalOrderList.Count, expectedResult.Count());
        }

        #endregion

        #region On tests
        [TestMethod()]
        public void OnTest1()
        {
            DatabaseConnector db = new DatabaseConnector();
            DateTime dt = new DateTime(2010, 03, 02);
            db.ApplyFilters(false, true, false, dt);

            var dbConnection = new DatabaseConnector().GetConnection;
            dbConnection.Open();
            var sql = "SELECT * FROM orders WHERE order_date = '2010-03-02'";
            var expectedResult = dbConnection.Query(sql);
            dbConnection.Close();

            Assert.AreEqual(db.TotalOrderList.Count, expectedResult.Count());
        }

        [TestMethod()]
        public void OnTest2()
        {
            DatabaseConnector db = new DatabaseConnector();
            DateTime dt = new DateTime(2012, 11, 13);
            db.ApplyFilters(false, true, false, dt);

            var dbConnection = new DatabaseConnector().GetConnection;
            dbConnection.Open();
            var sql = "SELECT * FROM orders WHERE order_date = '2012-11-13'";
            var expectedResult = dbConnection.Query(sql);
            dbConnection.Close();

            Assert.AreEqual(db.TotalOrderList.Count, expectedResult.Count());
        }

        [TestMethod()]
        public void OnTest3()
        {
            DatabaseConnector db = new DatabaseConnector();
            DateTime dt = new DateTime(2013, 08, 12);
            db.ApplyFilters(false, true, false, dt);

            var dbConnection = new DatabaseConnector().GetConnection;
            dbConnection.Open();
            var sql = "SELECT * FROM orders WHERE order_date = '2013-08-12'";
            var expectedResult = dbConnection.Query(sql);
            dbConnection.Close();

            Assert.AreEqual(db.TotalOrderList.Count, expectedResult.Count());
        }

        [TestMethod()]
        public void OnTest4()
        {
            DatabaseConnector db = new DatabaseConnector();
            DateTime dt = new DateTime(2018, 01, 11);
            db.ApplyFilters(false, true, false, dt);

            var dbConnection = new DatabaseConnector().GetConnection;
            dbConnection.Open();
            var sql = "SELECT * FROM orders WHERE order_date = '2018-01-11'";
            var expectedResult = dbConnection.Query(sql);
            dbConnection.Close();

            Assert.AreEqual(db.TotalOrderList.Count, expectedResult.Count());
        }

        #endregion

        #region After Tests
        [TestMethod()]
        public void AfterTest1()
        {
            DatabaseConnector db = new DatabaseConnector();
            DateTime dt = new DateTime(2014, 05, 19);
            db.ApplyFilters(false, false, true, dt);

            var dbConnection = new DatabaseConnector().GetConnection;
            dbConnection.Open();
            var sql = "SELECT * FROM orders WHERE order_date >= '2014-05-19'";
            var expectedResult = dbConnection.Query(sql);
            dbConnection.Close();

            Assert.AreEqual(db.TotalOrderList.Count, expectedResult.Count());
        }

        [TestMethod()]
        public void AfterTest2()
        {
            DatabaseConnector db = new DatabaseConnector();
            DateTime dt = new DateTime(2019, 08, 10);
            db.ApplyFilters(false, false, true, dt);

            var dbConnection = new DatabaseConnector().GetConnection;
            dbConnection.Open();
            var sql = "SELECT * FROM orders WHERE order_date >= '2019-08-10'";
            var expectedResult = dbConnection.Query(sql);
            dbConnection.Close();

            Assert.AreEqual(db.TotalOrderList.Count, expectedResult.Count());
        }

        [TestMethod()]
        public void AfterTest3()
        {
            DatabaseConnector db = new DatabaseConnector();
            DateTime dt = new DateTime(2019, 12, 12);
            db.ApplyFilters(false, false, true, dt);

            var dbConnection = new DatabaseConnector().GetConnection;
            dbConnection.Open();
            var sql = "SELECT * FROM orders WHERE order_date >= '2019-12-12'";
            var expectedResult = dbConnection.Query(sql);
            dbConnection.Close();

            Assert.AreEqual(db.TotalOrderList.Count, expectedResult.Count());
        }

        [TestMethod()]
        public void AfterTest4()
        {
            DatabaseConnector db = new DatabaseConnector();
            DateTime dt = new DateTime(2011, 09, 30);
            db.ApplyFilters(false, false, true, dt);

            var dbConnection = new DatabaseConnector().GetConnection;
            dbConnection.Open();
            var sql = "SELECT * FROM orders WHERE order_date >= '2011-09-30'";
            var expectedResult = dbConnection.Query(sql);
            dbConnection.Close();

            Assert.AreEqual(db.TotalOrderList.Count, expectedResult.Count());
        }
        #endregion
    }
}