using System;
/* Title:           Remove Employee
 * Date:            7-14-17
 * Author:          Terry Holmes */

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
using NewEventLogDLL;

namespace VehicleInspectionList
{
    /// <summary>
    /// Interaction logic for RemoveEmployee.xaml
    /// </summary>
    public partial class RemoveEmployee : Window
    {
        WPFMessagesClass TheMessagesClass = new WPFMessagesClass();
        SafetyClass TheSafetyClass = new SafetyClass();
        EventLogClass TheEventLogClass = new EventLogClass();

        FindVehicleInspectionMembersByEmployeeIDDataSet TheFindVehicleInspectionMembersByEmployeeIDDataSet = new FindVehicleInspectionMembersByEmployeeIDDataSet();

        int gintTransactionID;

        public RemoveEmployee()
        {
            InitializeComponent();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                TheFindVehicleInspectionMembersByEmployeeIDDataSet = TheSafetyClass.FindVehicleInspectionMembersByEmployeeID(MainWindow.gintEmployeeID);

                txtFirstName.Text = TheFindVehicleInspectionMembersByEmployeeIDDataSet.FindVehicleInspectionMembersByEmployeeID[0].FirstName;
                txtLastName.Text = TheFindVehicleInspectionMembersByEmployeeIDDataSet.FindVehicleInspectionMembersByEmployeeID[0].LastName;
                txtEmailAddress.Text = TheFindVehicleInspectionMembersByEmployeeIDDataSet.FindVehicleInspectionMembersByEmployeeID[0].EmailAddress;
                gintTransactionID = TheFindVehicleInspectionMembersByEmployeeIDDataSet.FindVehicleInspectionMembersByEmployeeID[0].TransactionID;
            }
            catch (Exception ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Vehicle Inspection List // Remove Employee // Window Loaded " + ex.Message);

                TheMessagesClass.ErrorMessage(ex.ToString());
            }
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            bool blnFatalError = false;

            blnFatalError = TheSafetyClass.RemoveVehicleEmailMember(gintTransactionID);

            if(blnFatalError == true)
            {
                TheMessagesClass.ErrorMessage("There Was a Problem, Contact ID");
            }
            else
            {
                Close();
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
