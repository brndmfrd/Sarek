using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using Sarek.FormController;

namespace Sarek
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {        


        private App()
        {
            bool everythingIsOK = true;

            
            everythingIsOK = CheckUniqueness();
            if (!everythingIsOK) { this.Shutdown(1); }       // maybe create a messagebox to alert people that nothing is going to happen.
            
            LaunchParentForm();
        }



        // We removed StartupUri="MainWindow.xaml from App.xaml
        // We needed some additoinal controls so now MainWindow.xaml (the project head) 
        // is launched from ParentForm.cs
        private void LaunchParentForm()
        {
            new ParentForm();
        }



        // There can be only one version of this application running at a time
        // At launch ensure the count of applications named the same name as this application is less than one(1) or kill this process.
        private bool CheckUniqueness()
        {
            Process[] pname = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName);
            if (pname.Length > 1){ return false;}
            else{return true;}
        }





    }// end App
}// end namespace Sarek
