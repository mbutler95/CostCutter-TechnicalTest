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
using System.Windows.Shapes;

namespace UiApp
{
    /// <summary>
    /// Interaction logic for FindConnectErrorWindow.xaml
    /// </summary>
    public partial class FindConnectErrorWindow : Window
    {
        public FindConnectErrorWindow(int searchnum)
        {
            InitializeComponent();
            searchNum = searchnum;
        }
        int searchNum;

        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            Application.Current.MainWindow.Close();
        }

        private void Retry_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            MainWindow win = (MainWindow)Application.Current.MainWindow;
            win.DatabaseModel.Find(searchNum);
        }
    }
}
