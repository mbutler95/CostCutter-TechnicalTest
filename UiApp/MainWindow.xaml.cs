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
            InitializeComponent();
            DatabaseModel.PopulateTotalOrders();
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
                    DatabaseModel.UpdateInfoLabel("Finding order number " + searchnum);
                    DatabaseModel.Find(searchnum);
                }
                else throw new FormatException();
            }
            catch (Exception)
            {
                DatabaseModel.UpdateInfoLabel("Invalid search");
            }
        }

        private void Filter_Reset_Click(object sender, RoutedEventArgs e)
        {
            After.IsChecked = false;
            Before.IsChecked = false;
            DateSelector.SelectedDate = null;
            OrderComboBox.Text = "";
            DatabaseModel.Filter = "";
            FilterExpander.IsExpanded = false;
            DatabaseModel.PopulateTotalOrders();
        }

        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            DateTime SelectedDate;
            if (DateSelector.SelectedDate != null)
            {
                SelectedDate = (DateTime)DateSelector.SelectedDate;
                DatabaseModel.ApplyFilters(Before.IsChecked ?? false, After.IsChecked ?? false, SelectedDate);
                FilterExpander.IsExpanded = false;
            }
            else
            {
                DatabaseModel.UpdateInfoLabel("Please enter a date");
            }
            
            
        }
        private void OrderComboBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            DatabaseModel.UpdateComboBox(OrderComboBox.Text);
            OrderComboBox.IsDropDownOpen = true;
        }

        private void OrderComboBox_GotFocus(object sender, RoutedEventArgs e)
        {
            FilterExpander.IsExpanded = false;
        }

        private void Button_GotFocus(object sender, RoutedEventArgs e)
        {
            FilterExpander.IsExpanded = false;
        }

        private void Reset_Button_Click(object sender, RoutedEventArgs e)
        {
            OrderComboBox.Text = "";
            OrderComboBox.IsDropDownOpen = false;
            DatabaseModel.ResetSearch();
        }

        private void Window_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            DatabaseModel.UpdateInfoLabel("");
        }

        private void OrderComboBox_LostFocus(object sender, RoutedEventArgs e)
        {
            OrderComboBox.IsDropDownOpen = false;
        }
    }
}
