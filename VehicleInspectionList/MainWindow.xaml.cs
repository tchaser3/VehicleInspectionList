/* Title:           Main Window Vehicle Inspection Lists
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using NewEmployeeDLL;
using NewEventLogDLL;
using DataValidationDLL;

namespace VehicleInspectionList
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //setting up the classes
        WPFMessagesClass TheMessagesClass = new WPFMessagesClass();
        EmployeeClass TheEmployeeClass = new EmployeeClass();
        EventLogClass TheEventLogClass = new EventLogClass();
        DataValidationClass TheDataValidationClass = new DataValidationClass();

        //logon data
        public static VerifyLogonDataSet TheVerifyLogonDataSet = new VerifyLogonDataSet();

        //global variables
        int gintNoOfMisses;
        public static int gintEmployeeID;

        public MainWindow()
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            gintNoOfMisses = 0;

            pbxPassword.Focus();
        }
        private void LogonFailed()
        {
            gintNoOfMisses++;

            if(gintNoOfMisses == 3)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "There Have Been Three Attempts Sign Into Vehicle Inspection Lists Program");

                TheMessagesClass.ErrorMessage("You Have Attempted Three Times to Sign In\nThe Program Will Not Close");

                Application.Current.Shutdown();
            }
            else
            {
                TheMessagesClass.InformationMessage("You Have Failed the Sign In Process");
                return;
            }
        }

        private void btnSignIn_Click(object sender, RoutedEventArgs e)
        {
            //setting local variables;
            int intEmployeeID = 0;
            string strValueForValidation;
            string strLastName;
            string strErrorMessage = "";
            int intRecordsReturned;
            bool blnFatalError = false;

            //beginning Data Validation
            strValueForValidation = pbxPassword.Password.ToString();
            blnFatalError = TheDataValidationClass.VerifyIntegerData(strValueForValidation);
            if(blnFatalError == true)
            {
                strErrorMessage += "The Employee ID Is not an Integer\n";
            }
            else
            {
                intEmployeeID = Convert.ToInt32(strValueForValidation);
            }
            strLastName = txtLastName.Text;
            if(strLastName == "")
            {
                blnFatalError = true;
                strErrorMessage += "The Last Name Was Not Entered\n";
            }
            if(blnFatalError == true)
            {
                TheMessagesClass.ErrorMessage(strErrorMessage);
                return;
            }

            TheVerifyLogonDataSet = TheEmployeeClass.VerifyLogon(intEmployeeID, strLastName);

            intRecordsReturned = TheVerifyLogonDataSet.VerifyLogon.Rows.Count;

            if(intRecordsReturned == 0)
            {
                LogonFailed();
            }
            else
            {
                if((TheVerifyLogonDataSet.VerifyLogon[0].EmployeeGroup != "ADMIN") && (TheVerifyLogonDataSet.VerifyLogon[0].EmployeeGroup != "MANAGERS"))
                {
                    LogonFailed();
                }
                else
                {
                    MainMenu MainMenu = new MainMenu();
                    MainMenu.Show();
                    Hide();
                }
            }
        }
    }
}
