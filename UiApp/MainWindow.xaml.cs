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
            DatabaseModel.Find(1);
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            After.IsChecked = false;
            Before.IsChecked = false;
            DateSelector.SelectedDate = null;
            DatabaseModel.ResetComboBox();
        }

        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            DatabaseModel.ApplyFilters();
        }

        private void OrderComboBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            DatabaseModel.UpdateComboBox(OrderComboBox.Text);
        }

        private void OrderComboBox_TextInput(object sender, TextCompositionEventArgs e)
        {
            MessageBox.Show("Oi Dipshit Text Input");
            DatabaseModel.UpdateComboBox(OrderComboBox.Text);
        }
    }
}
