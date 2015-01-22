using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;       // added for socket connections


namespace Sarek.MessagePassing.Authentication
{
    public class NetAuth
    {
        private string errMsg;
        private int nPrints;
        private byte[] in_buffer = new byte[32];        // input for network connection
        private System.Net.IPAddress serverAddy;        // our server friend

        public string status { get { return errMsg; } }
        public int remaining_prints { get { return nPrints; } }

        

        //----- Establish network connection and retrive replys from target server-----
        // Called by Authentication.cs
        public NetAuth()    // pass server name & port through constructor
        {
            Configuration config = new Configuration();

            serverAddy = System.Net.IPAddress.Parse(config.server1);  // the only auth server to negotiate with            
        }



        //----- This is the password gate -----
        // Called by Authentication.cs with the encrypted string name+password and the port#
        // Calls NegotiateWithServer() and evaluates what the server sent back (in_buffer)
        public bool Speak_Friend_And_Enter(byte[] cryp_credentials, int authPort)
        {
            NegotiateWithServer(cryp_credentials, authPort);
            if (in_buffer[0] == 49) { errMsg = "Bad Username or Password"; return false; }  // returned 1
            if (in_buffer[0] == 48) { errMsg = "Authentication Passed"; return true; }      // returned 0
            else { errMsg = "Failed connection to server"; return false; }                 // returned something weird
        }//-- end Speak_Friend...()



        //----- Back and Forth communication at Authentication through the net -----
        // Called by Speak_Friend...()
        // Attempted authentication server side returns "1" (failure) or "0" (success)
        // Actual success string is a 0 (zero) followed by a comma and the number of remaining prints.
        private bool NegotiateWithServer(byte[] message, int authPort)
        {
            Socket soc = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            byte[] out_buffer = message;
            soc.SendTimeout = 2000;                 // 2 seconds to live - send/receive attempts
            soc.ReceiveTimeout = 2000;

            try
            {
                soc.Connect(serverAddy, authPort);  // create persistant connection
                soc.Send(out_buffer);               // send the encrypted creds (name/address parsing done server side
                soc.Receive(in_buffer);             // block until server sends a responce ('0': pass '1': fail)
            }
            catch (SocketException)                 //soc.connect fails, soc.send/receive times out
            {
                soc.Close();                        // free the resource
                errMsg = "Bad connection to remote server";
                return false;
            }
            catch (Exception e)
            {
                soc.Close();                        // free the resource
                errMsg = "Undefined Socket error: " + e.ToString();
                return false;
            }

            soc.Close();                             // free the resource

            // Convert the rest of the byte string to it's intended integer value
            try
            {
                nPrints = Convert.ToInt32(System.Text.Encoding.ASCII.GetString(in_buffer, 2, 4));
            }
            catch (FormatException)
            {
                errMsg = "Server not sending back a 32bit integer value";
                return false;
            }

            return true;
        }//-- end NegotiateWithServer()




    }//-- end class NetAuth
}
