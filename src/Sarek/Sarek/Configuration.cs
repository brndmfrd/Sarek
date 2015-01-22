using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sarek
{
    public class Configuration
    {
        public string server1;
        public string server2;
        public string server3;
        public string server4;
        public string server5;
        public string server6;
        public string server7;
        public string server8;

        public string resourceName2;
        public string resourceName3;
        public string resourceName4;
        public string resourceName5;
        public string resourceName6;
        public string resourceName7;
        public string resourceName8;

 

        public Configuration()
        {
            server1 = "192.168.1.1"; 
            server2 = "\\\\192.168.1.1\\share1";
            server3 = "\\\\sfs.place.com\\share2";
            server4 = "\\\\192.168.1.1\\share3";
            server5 = "\\\\192.168.1.100\\printer1";
            server6 = "\\\\192.168.1.100\\printer2";
            server7 = "\\\\192.168.1.100\\printer3";
            server8 = "\\\\192.168.1.100\\printer4";

            resourceName2 = "network location 1";
            resourceName3 = "network location 2";
            resourceName4 = "network location 3";
            resourceName5 = "cups printer 1";
            resourceName6 = "cups printer 2";
            resourceName7 = "cups printer 3";
            resourceName8 = "cups printer 4";
        }



    }
}
