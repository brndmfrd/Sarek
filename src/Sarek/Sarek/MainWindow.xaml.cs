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

using Sarek.NetworkDevices;
using Sarek.MessagePassing.Authentication;

namespace Sarek
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MapNetworkDrive mnd;
        private Authentication au;


        public MainWindow()
        {
            InitializeComponent();
            mnd = new MapNetworkDrive();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void txt_name_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txt_pass_TextChanged(object sender, TextChangedEventArgs e)
        {

        }



        // ---------- User attempting to sign in with name/pass credentials ----------
        // Called by 'User Action'
        // Calls MainWindow.SignInAttemp() A driver method that calls for authentication and mounting
        // During 'sign in' and 'signing in' process this button disables itself
        // Button re-enabled upon auth failure or always by pressing the cancel button
        private void bttn_sign_in_Click(object sender, RoutedEventArgs e)
        {
            bttn_sign_in.IsEnabled = false;          // Step 1) 'Sign In' disabled

            if (SignInAttempt())
            {
                bttn_sign_out.IsEnabled = true;
            }     // Step 2) 'Cancel' enabled
            else
            {
                bttn_sign_in.IsEnabled = true;
            }      // Failure, goback to Step 1
        }//-- end bttn_sign_in_Click()



        // ---------- User attempting to sign out ----------
        // Called by 'User Action'
        // Calls MapNetworkDrive.Cancel_All_Drives() - Unmounts all pre-defined network devices
        // Initially disabled.  Enabled after auth success.
        public void bttn_sign_out_Click(object sender, RoutedEventArgs e)
        {
            bttn_sign_out.IsEnabled = false;
            txtbx_report.Clear();
            txt_name.Clear();
            txt_pass.Clear();
            mnd.Cancel_All_Drives();
            bttn_sign_in.IsEnabled = true;
        }//-- end bttn_sign_out_Click()



        private bool SignInAttempt()
        {
            au = new Authentication();
            //mnd = new MapNetworkDrive();            
            bool keepGoing = true;

            // sanity checking name and password strings
            if (txt_name.Text.Length <= 0)
            {
                txtbx_report.Text += "Name field must not be blank.\n";
                return false;
            }
            if (txt_pass.Password.Length <= 0)
            {
                txtbx_report.Text += "Password field must not be blank.\n";
                return false;
            }

            // give sign-in an attempt
            keepGoing = au.Authenticate(txt_name.Text, txt_pass.Password);

            // auth passed
            if (keepGoing)
            {
                // Report remaining prints
                txtbx_report.Text += "Your remaining prints: " + au.remainingPrints + "\n";

                // Mount as many drives as possible
                mnd.Map_All_Drives(txt_name.Text, txt_pass.Password);

                foreach (string s in mnd.status)
                {
                    txtbx_report.Text += s + "\n";
                }
            }
            else { txtbx_report.Text += au.status + "\n"; }

            return keepGoing;
        }

        private void bttn_OK_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }



    }//-- end class MainWindow
}
