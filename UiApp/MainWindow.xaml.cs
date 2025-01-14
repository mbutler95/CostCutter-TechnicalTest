﻿using System;
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
using System.Threading;

namespace UiApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = DatabaseModel;
        }

        private DatabaseConnector _databaseModel = new();

        public DatabaseConnector DatabaseModel { get => _databaseModel; set => _databaseModel = value; }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.BeginInvoke((Action)(() => DatabaseModel.PopulateTotalOrders()));
        }

        public static void InitConnectionError()
        {
            InitializationConnectErrorWindow InitError = new();
            InitError.Owner = Application.Current.MainWindow;
            InitError.ShowDialog();
        }

        public static void FindConnectionError(int searchnum)
        {
            FindConnectErrorWindow FindError = new(searchnum);
            FindError.Owner = Application.Current.MainWindow;
            FindError.ShowDialog();
        }

        private void Window_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            DatabaseModel.InvalidSearch("");
            DatabaseModel.ShowFilterError("");
            DatabaseModel.ShowFilterApplied("");
            
        }
        private void Find_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int searchnum = Int32.Parse(OrderComboBox.Text);
                this.Dispatcher.BeginInvoke((Action)(() => DatabaseModel.Find(searchnum)));
            }
            catch (Exception)
            {
                DatabaseModel.InvalidSearch("Invalid Search");
            }
        }

        private void Reset_Button_Click(object sender, RoutedEventArgs e)
        {
            OrderComboBox.Text = "";
            OrderComboBox.IsDropDownOpen = false;
            DatabaseModel.UpdateSearchLabel("");
            DatabaseModel.ResetSearch();
        }

        private void Filter_Reset_Click(object sender, RoutedEventArgs e)
        {
            After.IsChecked = false;
            Before.IsChecked = false;
            On.IsChecked = false;
            DateSelector.SelectedDate = null;
            OrderComboBox.Text = "";
            DatabaseModel.Filter = null;
            this.Dispatcher.BeginInvoke((Action)(() => DatabaseModel.PopulateTotalOrders()));
        }

        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            DateTime SelectedDate;
            if (DateSelector.SelectedDate != null)
            {
                SelectedDate = (DateTime)DateSelector.SelectedDate;
                this.Dispatcher.BeginInvoke((Action)(() => DatabaseModel.ApplyFilters(Before.IsChecked ?? false, On.IsChecked ?? false, After.IsChecked ?? false, SelectedDate)));
            }
            else
            {
                DatabaseModel.ShowFilterError("Please enter a date");
            }  
        }

        private void OrderComboBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.Dispatcher.BeginInvoke((Action)(() => DatabaseModel.UpdateComboBox(OrderComboBox.Text)));
            OrderComboBox.IsDropDownOpen = true;
        }

        private void OrderComboBox_GotFocus(object sender, RoutedEventArgs e)
        {
            OrderComboBox.IsDropDownOpen = true;
            FilterExpander.IsExpanded = false;
        }

        private void Button_GotFocus(object sender, RoutedEventArgs e)
        {
            FilterExpander.IsExpanded = false;
        }
   
        private void OrderComboBox_LostFocus(object sender, RoutedEventArgs e)
        {
            OrderComboBox.IsDropDownOpen = false;
        }

    }
}
