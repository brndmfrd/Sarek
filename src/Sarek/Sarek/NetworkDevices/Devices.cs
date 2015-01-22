using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sarek.NetworkDevices
{
    public class Devices
    {
        private string driverLetter;
        private string server_ip;
        private string description;
        private bool printflag;

        public string localDrive { get { return driverLetter; } set { driverLetter = value; } }
        public string server { get { return server_ip; } set { server_ip = value; } }
        public string comment { get { return description; } set { description = value; } }
        public bool isAPrinter { get { return printflag; } set { printflag = value; } }

        //----- Effectivly a struct for holding values needed for network device mapping -----
        // Called by MapNetworkDrive.Map_All_Drives()
        public Devices(string _localDrive, string _server, string _comment, bool _isAPrinter)
        {
            driverLetter = _localDrive;
            server_ip = _server;
            description = _comment;
            printflag = _isAPrinter;
        }//-- end Devices()



    }//-- end Class Devices
}
