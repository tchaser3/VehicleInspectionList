/* Title:           Add Employee To Report List
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
using SafetyDLL;
using NewEventLogDLL;
using NewEmployeeDLL;

namespace VehicleInspectionList
{
    /// <summary>
    /// Interaction logic for AddEmployeeToReportList.xaml
    /// </summary>
    public partial class AddEmployeeToReportList : Window
    {
        //setting up the classes
        WPFMessagesClass TheMessagesClass = new WPFMessagesClass();
        SafetyClass TheSafetyClass = new SafetyClass();
        EventLogClass TheEventLogClass = new EventLogClass();
        EmployeeClass TheEmployeeClass = new EmployeeClass();

        ComboEmployeeDataSet TheComboBoxEmployeeDataSet = new ComboEmployeeDataSet();
        FindVehicleInspectionEmailListMembersDataSet TheFindVehicleInspectionEmailListMembersDataSet = new FindVehicleInspectionEmailListMembersDataSet();
        FindVehicleInspectionMembersByEmployeeIDDataSet TheFindVehicleInspectionMembersByEmployeeIDDataSet = new FindVehicleInspectionMembersByEmployeeIDDataSet();
        
        int gintEmployeeID;

        public AddEmployeeToReportList()
        {
            InitializeComponent();
        }

        private void btnMainMenu_Click(object sender, RoutedEventArgs e)
        {
            MainMenu MainMenu = new MainMenu();
            MainMenu.Show();
            Close();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            TheMessagesClass.CloseTheProgram();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            btnProcess.IsEnabled = false;

            TheFindVehicleInspectionEmailListMembersDataSet = TheSafetyClass.FindVehicleInspectionEmailListMembers();

            dgrMembers.ItemsSource = TheFindVehicleInspectionEmailListMembersDataSet.FindVehicleInspectionEmailListMembers;
        }

        private void txtLastName_TextChanged(object sender, TextChangedEventArgs e)
        {
            //setting local variables
            int intNumberOfRecords;
            string strLastName;
            int intLength;
            int intCounter;

            strLastName = txtLastName.Text;

            intLength = strLastName.Length;

            if(intLength >= 3)
            {
                TheComboBoxEmployeeDataSet = TheEmployeeClass.FillEmployeeComboBox(strLastName);

                cboSelectEmployee.Items.Clear();
                cboSelectEmployee.Items.Add("Select Employee");

                intNumberOfRecords = TheComboBoxEmployeeDataSet.employees.Rows.Count - 1;

                if(intNumberOfRecords == -1)
                {
                    TheMessagesClass.ErrorMessage("No Employees Found");
                    return;
                }
                else if (intNumberOfRecords > -1)
                {
                    for(intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
                    {
                        cboSelectEmployee.Items.Add(TheComboBoxEmployeeDataSet.employees[intCounter].FullName);
                    }

                    cboSelectEmployee.SelectedIndex = 0;
                }
                else
                {
                    TheMessagesClass.ErrorMessage("There Has Been A Problem, Contact IT");

                    TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Vehicle Inspection List // Last Name Change Event // The Number Of Records is Less Than -1");
                    
                }
            }
        }

        private void cboSelectEmployee_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //setting variables
            int intSelectedIndex;
            char[] chaFirstName;
            string strFirstName;
            string strLastName;
            string strEmailAddress;
            int intRecordsReturned;

            try
            {
                intSelectedIndex = cboSelectEmployee.SelectedIndex;

                if(intSelectedIndex > 0)
                {
                    gintEmployeeID = TheComboBoxEmployeeDataSet.employees[intSelectedIndex - 1].EmployeeID;

                    TheFindVehicleInspectionMembersByEmployeeIDDataSet = TheSafetyClass.FindVehicleInspectionMembersByEmployeeID(gintEmployeeID);

                    intRecordsReturned = TheFindVehicleInspectionMembersByEmployeeIDDataSet.FindVehicleInspectionMembersByEmployeeID.Rows.Count;

                    if(intRecordsReturned > 0)
                    {
                        TheMessagesClass.ErrorMessage("Employee Is Already on the List");
                        btnProcess.IsEnabled = false;
                        return;
                    }

                    strFirstName = TheComboBoxEmployeeDataSet.employees[intSelectedIndex - 1].FirstName;
                    strLastName = TheComboBoxEmployeeDataSet.employees[intSelectedIndex - 1].LastName;
                    chaFirstName = strFirstName.ToCharArray();
                    strEmailAddress = Convert.ToString(chaFirstName[0]) + strLastName + "@bluejaycommunications.com";

                    txtEmailAddress.Text = strEmailAddress.ToLower();

                    btnProcess.IsEnabled = true;
                }
                else
                {
                    txtEmailAddress.Text = "";
                }
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Vehicle Inspection List // Add Employee To Report List // cbo Selected Employee Selection Changed " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
        }

        private void btnProcess_Click(object sender, RoutedEventArgs e)
        {
            //setting local variable
            string strEmailAddress;
            bool blnFatalError = false;

            strEmailAddress = txtEmailAddress.Text;

            if (strEmailAddress == "")
            {
                TheMessagesClass.ErrorMessage("Employee Was Not Selected");
                return;
            }

            blnFatalError = TheSafetyClass.InsertNewVehicleInspectionEmailMember(gintEmployeeID, strEmailAddress);

            if(blnFatalError == true)
            {
                TheMessagesClass.ErrorMessage("There Was A Problem, Contact IT");
                return;
            }

            txtEmailAddress.Text = "";
            cboSelectEmployee.Items.Clear();
            txtLastName.Text = "";

            TheFindVehicleInspectionEmailListMembersDataSet = TheSafetyClass.FindVehicleInspectionEmailListMembers();

            dgrMembers.ItemsSource = TheFindVehicleInspectionEmailListMembersDataSet.FindVehicleInspectionEmailListMembers;
        }
    }
}
