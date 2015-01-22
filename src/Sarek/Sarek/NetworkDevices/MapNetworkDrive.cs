using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sarek.NetworkDevices.API;
using Sarek.AppLog;

namespace Sarek.NetworkDevices
{
    class MapNetworkDrive : NetConnect2API
    {
        private string uName;
        private string uPass;
        private NETRESOURCE resource;
        private MessageLogging log;
        private bool[] deviceStatus;    // Success of each device mapping
        private string[] mountReports;  // Status messages about each mount (successfull or not).


        public string userName { get { return uName; } }
        public string userPass { get { return uPass; } }
        public string[] status { get { return mountReports; } }
        public bool[] devices { get { return deviceStatus; } }


        public MapNetworkDrive() { }


        //----- Mount all network devices : authentication has been accepted at this point -----
        // Called by MainWindow.xaml.cs (Button click -> auth -> net devices)
        // Calls Map_Device() for each device this method has to mount
        public void Map_All_Drives(string _uName, string _uPass)
        {
            Configuration config = new Configuration();
            log = new MessageLogging("Map Network Devices"); // Log entries are in Map_Device()
            uName = _uName;
            uPass = _uPass;
            List<Devices> devices = new List<Devices>();

            devices.Add(new Devices("I:", config.server2, config.resourceName2, false));
            devices.Add(new Devices("H:", config.server3, config.resourceName3, false));
            devices.Add(new Devices("S:", config.server4, config.resourceName4, false));

            devices.Add(new Devices("lpt3:", config.server5, config.resourceName5, true));
            devices.Add(new Devices("lpt4:", config.server6, config.resourceName6, true));
            devices.Add(new Devices("lpt5:", config.server7, config.resourceName7, true));
            devices.Add(new Devices("lpt6:", config.server8, config.resourceName8, true));

            deviceStatus = new bool[devices.Count];     // define bool array now that devices are est.
            mountReports = new string[devices.Count];   // status report of each attempted mount

            // deviceStatus: mount all devices and report t/f if successfull
            // mountReports: array of status messages about each mount (successfull or not).
            for (int i = 0; i < devices.Count; i++)
            {
                deviceStatus[i] =
                    Map_Device(devices[i].localDrive, devices[i].server, devices[i].comment, devices[i].isAPrinter);
                mountReports[i] = devices[i].comment + " Mapping " + (deviceStatus[i] ? "Succeeded" : "Failed");
            }

            log.CloseLog();
        }//-- end Map_All_Drives()




        // Called by Map_All_Drives()
        private bool Map_Device(string localDrive, string server, string comment, bool isaPrinter)
        {
            string message = null;
            resource = new NETRESOURCE();           // Refrence the struct from API.NetConnect2API

            resource.lpLocalName = localDrive;
            resource.lpRemoteName = server;
            resource.lpComment = comment;
            resource.dwDisplayType = ResourceDisplayType.RESOURCEDISPLAYTYPE_GENERIC;
            resource.dwScope = ResourceScope.RESOURCE_GLOBALNET;
            resource.dwUsage = 0x0;
            resource.lpProvider = null;
            if (isaPrinter) { resource.dwType = ResourceType.RESOURCETYPE_PRINT; }
            else { resource.dwType = ResourceType.RESOURCETYPE_DISK; }

            // Send user name and password to create a connection at the 
            // Network Resource defined above. 
            uint syscode = WNetAddConnection2(resource, uPass, uName, 0x0);
            switch (syscode)
            {
                case 0: message = "Try connect - " + localDrive + " " + server + ": SUCCESS!";
                    break;
                case 5: message = "Try connect - " + localDrive + " " + server + ": ERROR " +
                syscode + "- Bad Authentication - Access Denied.";
                    break;
                case 53: message = "Try connect - " + localDrive + " " + server + ": ERROR " +
                 syscode + "- Bad network path.";
                    break;
                case 85: message = "Try connect - " + localDrive + " " + server + ": ERROR " +
                 syscode + "- Bad network path.";
                    break;
                case 86: message = "Try connect - " + localDrive + " " + server + ": ERROR " +
                 syscode + "- Bad Authentication.";
                    break;
                case 1229: message = "Try connect - " + localDrive + " " + server + ": ERROR " +
                   syscode + "- Credentials Conflict.";
                    break;
                case 1330: message = "Try connect - " + localDrive + " " + server + ": ERROR " +
                   syscode + "- Bad Authentication.";
                    break;
                case 1326: message = "Try connect - " + localDrive + " " + server + ": ERROR " +
                   syscode + "- Bad Authentication.";
                    break;
                default: message = "Try connect - " + localDrive + " " + server + ": ERROR " +
                 syscode + "- Undefined Error.  See Windows System error codes.";
                    break;
            }

            log.AddMessage(message);

            if (syscode == 0) return true;
            else return false;
        }//-- end Map_Device()


        // Cancel the network connections
        // If the connections do not exist, no exceptions are thrown
        public void Cancel_All_Drives()
        {
            log = new MessageLogging("Unmount all devices");

            // le network drives
            Cancel_Drive("I:", 0x1, true);
            Cancel_Drive("H:", 0x1, true);
            Cancel_Drive("S:", 0x1, true);

            // le network printers
            Cancel_Drive("lpt3:", 0x1, true);
            Cancel_Drive("lpt4:", 0x1, true);
            Cancel_Drive("lpt5:", 0x1, true);
            Cancel_Drive("lpt6:", 0x1, true);

            log.CloseLog();
        }//-- end Cancel_All_Drives()


        // Cancel a drive - called by Cancel_All_Drives()
        private void Cancel_Drive(string lpname, uint dwflag, bool bforce)
        {
            string message = null;

            uint syscode = WNetCancelConnection2(lpname, dwflag, bforce);   // cancel the connection

            switch (syscode)
            {
                case 0: message = "Try remove - " + lpname + ": SUCCESS!";
                    break;
                case 2250: message = "Try remove - " + lpname + ": ERROR " +
                   syscode + "- Connection does not exist";
                    break;
                default: message = "Try remove - " + lpname + ": ERROR " +
                 syscode + "- Undefined Error.  See Windows System error codes.";
                    break;
            }

            log.AddMessage(message);
        }//-- end Cancel_Drive()


    }//-- end class MapNetworkDrive
}
