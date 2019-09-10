using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepRap_Phone_Host.Communication;

namespace RepRap_Phone_Host.GCode
{
    /// <summary>
    /// This class contains all the functions related to handling
    /// responses to sent GCode.
    /// </summary>
    class GCodeResponseReciever
    {
        BluetoothIO bluetoothIO;//The bluetoothio that we will be recieving meesages from

        /// <summary>
        /// Intitialises a new class that can respond to messages recieved from a bluetooth
        /// device connected to a bluetoothio instance
        /// </summary>
        /// <param name="_bluetoothIO">The bluetoothio that is connected to the device that will be sending us a message</param>
        public GCodeResponseReciever(BluetoothIO _bluetoothIO)
        {
            bluetoothIO = _bluetoothIO;
            bluetoothIO.RecievedMessage += bluetoothIO_RecievedMessage;
        }

        /// <summary>
        /// The handler that parses the response from the deice
        /// </summary>
        /// <param name="message">The message that should be parsed</param>
        void bluetoothIO_RecievedMessage(string message)
        {
            //Parse the text and raise the appropriate event if needed
            if (message.Contains("wait"))//check if it is a wait message
            {
                //Debug.WriteLine("wait");
                return;
            }

            else if (message.Contains("T:"))//check if message contains temperature information
            {
                if (TemperatureRecieved != null)//Check if an eventhandler is attacthed
                {
                    //expect: T:-46.76 B:-1.00 @:0
                    string withoutSpace = message.Split(' ')[0];
                    string withoutT = withoutSpace.Split('T')[1];
                    string withoutColon = withoutT.Split(':')[1];
                    if (TemperatureRecieved != null)
                        TemperatureRecieved(withoutColon);
                }
            }


            else if (message.Contains("ok"))//This needs to be checked after the temperature because that also contains "ok"
            {
                if (OkRecieved != null)//Check if an eventhandler is attacthed
                {
                    int lineNumber;
                    if (int.TryParse(message.Replace(@"ok ", ""), out lineNumber))
                        OkRecieved(lineNumber);
                }
            }

            else if (message.Contains("Resend"))
            {
                //expect: Resend:28
                if (ResendRecieved != null)//Check if an eventhandler is attacthed
                {
                    int lineNumber;
                    if (int.TryParse(message.Replace(@"Resend:", ""), out lineNumber))//Try parsing only the number in the line
                        ResendRecieved(lineNumber);
                }
            }

            if (MessageRecieved != null && message.Length > 1)//Check if an eventhandler is attacthed and that the line is not empty
                MessageRecieved(message);
        }

        //This event is raised when we recieve an ok response
        public event OkRecievedDelegate OkRecieved;
        public delegate void OkRecievedDelegate(int okLine);

        //This event is raised when we recieve a resend response
        public event ResendRecievedDelegate ResendRecieved;
        public delegate void ResendRecievedDelegate(int resendLine);

        //This event is raised when we recieve a temperature
        public event TemperatureRecievedHandler TemperatureRecieved;
        public delegate void TemperatureRecievedHandler(string temperatureResponse);

        //This event is raised when we recieve a message and want to log it
        public event MessageRecievedHandler MessageRecieved;
        public delegate void MessageRecievedHandler(string message);
    }
}
