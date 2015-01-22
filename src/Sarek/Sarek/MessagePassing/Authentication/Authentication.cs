using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Sarek.AppLog;
using Sarek.MessagePassing.Encryption;


namespace Sarek.MessagePassing.Authentication
{
    public class Authentication
    {
        private string uName;
        private string uPass;
        private string errorMsg;
        private MessageLogging log;
        private int nPrints;


        public string userName { get { return uName; } }
        public string userPass { get { return uPass; } }
        public string status { get { return errorMsg; } }
        public int remainingPrints { get { return nPrints; } }

        public Authentication()
        {
            uName = "noname";
            uPass = "nopass";
            errorMsg = "";
            MessageLogging log_object = new MessageLogging();     // no params - does not open file, just refrence object
            log_object.Clear();                    // Assumed to be first step in execution - clear the old log on every new attempt to 'log in'
        }


        //----- Authenticate user with name/pass credentials-----
        // Called by MainWindow.xaml.cs (button click)
        // Calls Net.NetAuth.Speak_Friend_And_Enter() to transmit encrypted name/pass 
        // Return t/f : Successfull authentication
        public bool Authenticate(string _uName, string _uPass)
        {
            log = new MessageLogging("Authenticate");
            Crypt crypt = new Crypt();
            Aes myAes = Aes.Create();
            NetAuth netauth = new NetAuth();
            uName = _uName;
            uPass = _uPass;
            string namepass = "";
            bool authenticated = false;

            namepass = uName + uPass;

            byte[] encrypted_creds = crypt.EncryptStringToBytes_Aes(uName + uPass, myAes.Key, myAes.IV);
            
            // Send to Server to test our credentials 
            authenticated = netauth.Speak_Friend_And_Enter(encrypted_creds, 723);

            // If network authentication failed, log netauth's error message
            if (authenticated == false)
            {
                errorMsg = netauth.status;
                log.AddMessage(errorMsg);   // report to the log
                log.CloseLog();
                return false;
            }

            // collect the number of prints remaining 
            nPrints = netauth.remaining_prints;

            log.CloseLog();
            return true;
        }//-- end Authenticate() --
    }//----- end Class Authentication -----
}
