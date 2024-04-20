/* Title:           Main Menu
 * Date:            7-13-17
 * Author:          Terry Holmes */

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

namespace VehicleInspectionList
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Window
    {
        //settup the classes
        WPFMessagesClass TheMessagesClass = new WPFMessagesClass();

        public MainMenu()
        {
            InitializeComponent();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            TheMessagesClass.CloseTheProgram();
        }

        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            About About = new About();
            About.ShowDialog();
        }

        private void btnAddEmployeeToReportList_Click(object sender, RoutedEventArgs e)
        {
            AddEmployeeToReportList AddEmployeeToReportList = new AddEmployeeToReportList();
            AddEmployeeToReportList.Show();
            Close();
        }

        private void btnRemoveEmployeeFromList_Click(object sender, RoutedEventArgs e)
        {
            RemoveEmployeeFromEmailList RemoveEmployeeFromEmailList = new RemoveEmployeeFromEmailList();
            RemoveEmployeeFromEmailList.Show();
            Close();
        }
    }
}
