using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UiApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.DataContext = DatabaseModel;
            DatabaseModel.PopulateTotalOrders();
            InitializeComponent();
            
        }

        private DatabaseConnector _databaseModel = new();

        public DatabaseConnector DatabaseModel { get => _databaseModel; set => _databaseModel = value; }

        private void Find_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int searchnum = Int32.Parse(OrderComboBox.Text);
                if (searchnum > 0 && searchnum < 30_000)
                {
                    DatabaseModel.Find(searchnum);
                    InfoBox.Text = "Finding Order Number "+ searchnum;
                }
                else throw new FormatException();
            }
            catch (Exception)
            {
                InfoBox.Text = "Invalid Search";
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            After.IsChecked = false;
            Before.IsChecked = false;
            DateSelector.SelectedDate = null;
            InfoBox.Text = "";
            OrderComboBox.Text = "";
            DatabaseModel.Filter = "";
            DatabaseModel.PopulateTotalOrders();
            DatabaseModel.DefaultComboBox();
        }

        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            DateTime SelectedDate;
            try
            {
                if (DateSelector.SelectedDate == null) throw new NullReferenceException();
                SelectedDate = (DateTime)DateSelector.SelectedDate;
                DatabaseModel.ApplyFilters(Before.IsChecked??false, After.IsChecked??false, SelectedDate);
            }
            catch (NullReferenceException)
            {
                InfoBox.Text = "Enter a Date";
            }
            
        }

        private void OrderComboBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            DatabaseModel.UpdateComboBox(OrderComboBox.Text);
        }
    }
}
