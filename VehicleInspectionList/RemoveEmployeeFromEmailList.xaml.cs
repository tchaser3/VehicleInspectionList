/* Title:           Remove Employee From Email List
 * Date:            7-14-17
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
using SafetyDLL;

namespace VehicleInspectionList
{
    /// <summary>
    /// Interaction logic for RemoveEmployeeFromEmailList.xaml
    /// </summary>
    public partial class RemoveEmployeeFromEmailList : Window
    {
        //setting up the classes
        WPFMessagesClass TheMessagesClass = new WPFMessagesClass();
        SafetyClass TheSafetyClass = new SafetyClass();

        FindVehicleInspectionEmailListMembersDataSet TheFindVehicleInspectionEmailListMembersDataSet = new FindVehicleInspectionEmailListMembersDataSet();

        public RemoveEmployeeFromEmailList()
        {
            InitializeComponent();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btnMainMenu_Click(object sender, RoutedEventArgs e)
        {
            MainMenu MainMenu = new MainMenu();
            MainMenu.Show();
            Close();
        }

        private void dgrMembers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int intSelectedIndex;

            intSelectedIndex = dgrMembers.SelectedIndex;

            if(intSelectedIndex > -1)
            {
                MainWindow.gintEmployeeID = TheFindVehicleInspectionEmailListMembersDataSet.FindVehicleInspectionEmailListMembers[intSelectedIndex].EmployeeID;

                RemoveEmployee RemoveEmployee = new RemoveEmployee();
                RemoveEmployee.ShowDialog();

                TheFindVehicleInspectionEmailListMembersDataSet = TheSafetyClass.FindVehicleInspectionEmailListMembers();

                dgrMembers.ItemsSource = TheFindVehicleInspectionEmailListMembersDataSet.FindVehicleInspectionEmailListMembers;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TheFindVehicleInspectionEmailListMembersDataSet = TheSafetyClass.FindVehicleInspectionEmailListMembers();

            dgrMembers.ItemsSource = TheFindVehicleInspectionEmailListMembersDataSet.FindVehicleInspectionEmailListMembers;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            TheMessagesClass.CloseTheProgram();
        }
    }
}
