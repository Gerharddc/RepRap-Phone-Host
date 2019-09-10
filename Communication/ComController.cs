using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Phone.Controls;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using Windows.Networking;
using RepRap_Phone_Host.GCode;
using RepRap_Phone_Host.FileSystem;
using RepRap_Phone_Host.Containers;
using RepRap_Phone_Host.ListItems;
using RepRap_Phone_Host.GlobalValues;

/* NOTE: At the moment the controller communicates in GCode
 * and therefore uses the GCode module. It has been designed
 * sothat other methods of communication will be possible in
 * the future. To do so other communication modules should
 * be made with the same public methods as the GCode module */

/* At the moment it is recommended for a class like the GCodeGenerator to be accessed
 * directly by the ui due to the sheer amount of functions that it has. This will change
 * once someone either adds those functions to the controller or finds anther way of implementing
 * the functionality
 */

namespace RepRap_Phone_Host.Communication
{
    /// <summary>
    /// This class contains al the function related to
    /// controlling communication with the printer.
    /// </summary>
    class ComController
    {
        //Keep local references of all the classes needed for communication with a device
        private BluetoothIO bluetoothIO;
        private GCodeSender GCodeSender;
        private GCodeResponseReciever responseReceiver;
        private GCodeGenerator GCodeGenerator;
        private GCodeFileReader GCodeFileReader;
        private Thread temperatureProbingThread;
        private LogBox logBox;
        private PhoneApplicationPage hostPage;
        private ListPicker device_Listpicker;
        private StackPanel log_Stackpanel;
        private TextBlock temperature_Textblock;

        //NOTE: at the moment temperature checking has only been enable for the #1 extruder, support for more extruders and heatbed(s) should be added later
        /// <summary>
        /// Initialise a Communication Controller which will handle all low level communication
        /// with the connected device
        /// </summary>
        /// <param name="_hostPage">The page that who's dispatcher will be used to display messageboxes with info regarding connection related events</param>
        /// <param name="_file_Listpicker">The listpicker tha will contain the list of GCode files that can be printed</param>
        /// <param name="_device_Listpicker">The listpicker that will contain the list of paired devices that can be connected to</param>
        /// <param name="_log_Stackpanel">The stackpanel that will be used to log communication</param>
        /// <param name="_temperature_Textblock">The textblock that will be used to display the current extruder temperature</param>
        public ComController(PhoneApplicationPage _hostPage, ListPicker _device_Listpicker, StackPanel _log_Stackpanel,
            TextBlock _temperature_Textblock)
        {
            //Store local references of the recieved instances
            hostPage = _hostPage;
            device_Listpicker = _device_Listpicker;
            log_Stackpanel = _log_Stackpanel;
            temperature_Textblock = _temperature_Textblock;

            //Initialise the needed classes
            bluetoothIO = new BluetoothIO();
            responseReceiver = new GCodeResponseReciever(bluetoothIO);
            GCodeSender = new GCodeSender(bluetoothIO, responseReceiver, hostPage.Dispatcher);
            GCodeGenerator = new GCodeGenerator(GCodeSender);

            //Connect the needed events to our local handlers
            bluetoothIO.ConnectedToDevice += bluetoothIO_ConnectedToDevice;
            bluetoothIO.ConnectionFailed += bluetoothIO_ConnectionFailed;
            bluetoothIO.DiconnectedFromDevice += bluetoothIO_DiconnectedFromDevice;

            //Setup the GCodefilereader
            GCodeFileReader = new GCodeFileReader(GCodeSender, GCodeGenerator);
            GCodeFileReader.FileReadingStarted += GCodeFileReader_FileReadingStarted;
            GCodeFileReader.FileReadingStopped += GCodeFileReader_FileReadingStopped;

            //Fill the listpicker that contains the list of connectable bluetooth with a list of all the currently paired bluetooth devices 
            Devicefinder.getListOfBluetoothDevices(device_Listpicker);

            //Link the external events to our local handlers
            GCodeSender.CommandSent += GCodeSender_CommandSent;
            responseReceiver.MessageRecieved += responseReceiver_MessageRecieved;
            responseReceiver.TemperatureRecieved += responseReceiver_TemperatureRecieved;

            //Start the probing thread with a function that probes the printer for the #1 extruders temperature every 5 second (5000 milliseconds)
            temperatureProbingThread = new Thread(new ThreadStart(() => GCodeTemperatureProber.probeTemperature(5000, GCodeGenerator)));

            //Initialise the logboxbox that will log all communication between the application and the connected device
            logBox = new LogBox(log_Stackpanel, hostPage, 25);
        }

        //When more communication protocols are added, this should probably become an overloaded function with an overload for each naming/identifying convention
        /// <summary>
        /// This function connects this communication controller instance to the
        /// bluetooth device with the specified HostName
        /// </summary>
        /// <param name="bluetoothDeviceHostName">The HostName of the device to connect to</param>
        public bool connectToDevice(HostName bluetoothDeviceHostName)
        {
            if (bluetoothIO == null)
            {
                MessageBox.Show("Your Bluetooth is not working (maybe it is off)");
                return false;
            }

            if (device_Listpicker.SelectedItem == null)
            {
                MessageBox.Show("You have not selected a Bluetooth device yet (maybe there isn't one)");
                return false;
            }

            bluetoothIO.ConnectToDevice(bluetoothDeviceHostName);

            return true;
        }

        /// <summary>
        /// This function disconnects from the curently connected device.
        /// </summary>
        public void disconnectFromDevice()
        {
            bluetoothIO.DisconnectFromDevice();

            if (temperatureProbingThread != null && temperatureProbingThread.ThreadState == ThreadState.Running)
                temperatureProbingThread.Abort();
        }

        /// <summary>
        /// This function tries to print from the file selected in
        /// the corresponding listpicker
        /// </summary>
        /// <param name="path">The file t print</param>
        public void tryPrintingFromFile()
        {
            GCodeFileReader.readAndtrySendingGCodeFromFile(Values.currentGCodeFile);
        }

        /// <summary>
        /// This function cancels the process of printing from a file
        /// </summary>
        public void cancelPrintingFromFile()
        {
            GCodeFileReader.cancelReadingFromFile();
        }

        /// <summary>
        /// This unction tries to send the specified command
        /// to the printer.
        /// </summary>
        /// <param name="command"></param>
        public void trySendingCommand(string command)
        {
            GCodeSender.trySendingGCode(command);
        }

        /// <summary>
        /// This function updates the textblock that displays the current #1 extruders temperature
        /// with the temperature that was recieved from the connected device
        /// </summary>
        /// <param name="temperatureResponse">The temperature string recieved from the printer</param>
        private void responseReceiver_TemperatureRecieved(string temperatureResponse)
        {
            //We need to use the dispatcher to update the ui because this functions gets called from a backgrond thread
            hostPage.Dispatcher.BeginInvoke(() => { temperature_Textblock.Text = temperatureResponse + "C"; });
        }

        /// <summary>
        /// This functions messages that are recieved from the printer
        /// </summary>
        /// <param name="message">The message that was recieved</param>
        private void responseReceiver_MessageRecieved(string message)
        {
            logBox.writeLineWithColor(message, Colors.Red);
        }

        /// <summary>
        /// This function logs messages that were sent by the application
        /// </summary>
        /// <param name="command"></param>
        private void GCodeSender_CommandSent(string command)
        {
            logBox.writeLineWithColor(command, Colors.Blue);
        }

        /// <summary>
        /// This event is called when we have stopped reading instructions from a file
        /// </summary>
        public event FileReadingStoppedEventHandler FileReadingStoppedEvent;
        public delegate void FileReadingStoppedEventHandler();
        /// <summary>
        /// This function is called when the GCodeFileReader has stopped reading instruction
        /// </summary>
        private void GCodeFileReader_FileReadingStopped()
        {
            if (FileReadingStartedEvent != null)
                FileReadingStoppedEvent();
        }
        //Other file reader should also use the above event in the demostarted way

        /// <summary>
        /// This event is called when we start reading instructions from a file
        /// </summary>
        public event FileReadingStartedEventHandler FileReadingStartedEvent;
        public delegate void FileReadingStartedEventHandler();
        /// <summary>
        /// This function is called when the GCodeFileReader has finsihed reading instructions from a file
        /// </summary>
        private void GCodeFileReader_FileReadingStarted()
        {
            if (FileReadingStartedEvent != null)
                FileReadingStartedEvent();
        }
        //Other file reader should also use the above event in the demostarted way

        /// <summary>
        /// This event is called when we have disconnected from a device
        /// </summary>
        public event DisconnectedEventHandler DisconnectedEvent;
        public delegate void DisconnectedEventHandler();
        /// <summary>
        /// This function is called when we have disconnected from a bluetoot device
        /// </summary>
        private void bluetoothIO_DiconnectedFromDevice()
        {
            //At the moment the application has to be restarted after the bluetoothio has diconnected, this should probably be fixed
            MessageBox.Show("You will have to restart the application to connect to another device");
            if (DisconnectedEvent != null)
                DisconnectedEvent();
        }
        //Other communicators should use the above event in the demonstarted way

        /// <summary>
        /// This event is raised when a connection to a device has failed
        /// </summary>
        public event ConnectionFailedEventHandler ConnectionFailedEvent;
        public delegate void ConnectionFailedEventHandler();
        /// <summary>
        /// This function is called when the cnnection to a bluetooth device fails
        /// </summary>
        private void bluetoothIO_ConnectionFailed()
        {
            MessageBox.Show("Sorry the connection failed");
            if (ConnectionFailedEvent != null)
                ConnectionFailedEvent();
        }
        //Other communicators should use the above event in the demonstarted way

        /// <summary>
        /// This event is called when we have connected to a printer
        /// </summary>
        public event ConnectedEventHandler ConnectedEvent;
        public delegate void ConnectedEventHandler();
        /// <summary>
        /// This function is called when we have connected to a bluetooth device
        /// </summary>
        private void bluetoothIO_ConnectedToDevice()
        {
            MessageBox.Show("You are now connected to a Bluetooth device");
            if (ConnectedEvent != null)
                ConnectedEvent();

            temperatureProbingThread.Start();
        }
        //Other communicators should use the above event in the demonstarted way


        //Below all command requests are merely routed through the controller. This is done sothat the communication protocol can be interchangeable
        //because other if parts of the application do not refernce the communicator directly then they won't break if it were to be changed

        /// <summary>
        /// Move the x axis with the given parameters.
        /// </summary>
        /// <param name="mm">The distance to move.</param>
        /// <param name="forward">Is the direction of the move forward?</param>
        /// <param name="feedrate">The speed(feedrate) at which the move should happen.</param>
        public void moveXaxis(float mm, bool forward, float feedrate)
        {
            GCodeGenerator.moveXaxis(mm, forward, feedrate);
        }

        /// <summary>
        /// Move the y axis with the given parameters.
        /// </summary>
        /// <param name="mm">The distance to move.</param>
        /// <param name="forward">Is the direction of the move forward?</param>
        /// <param name="feedrate">The speed(feedrate) at which the move should happen.</param>
        public void moveYaxis(float mm, bool forward, float feedrate)
        {
            GCodeGenerator.moveYaxis(mm, forward, feedrate);
        }

        /// <summary>
        /// Move the z axis with the given parameters.
        /// </summary>
        /// <param name="mm">The distance to move.</param>
        /// <param name="forward">Is the direction of the move forward?</param>
        /// <param name="feedrate">The speed(feedrate) at which the move should happen.</param>
        public void moveZaxis(float mm, bool forward, float feedrate)
        {
            GCodeGenerator.moveZaxis(mm, forward, feedrate);
        }

        /// <summary>
        /// This function moves the specified axis according to the specified parameters
        /// </summary>
        /// <param name="mm">The distance to move</param>
        /// <param name="forward">The direction to move in</param>
        /// <param name="feedrate">The speed to move at</param>
        /// <param name="axis">The axis that should be moved</param>
        public void moveAxis(float mm, bool forward, float feedrate, string axis)
        {
            //This should probably be changed to use an enumurator instead of string
            GCodeGenerator.moveAxis(mm, forward, feedrate, axis);
        }

        /// <summary>
        /// Move the extruder with the given parameters.
        /// </summary>
        /// <param name="mm">The distance to move.</param>
        /// <param name="forward">Is the direction of the move forward?</param>
        /// <param name="feedrate">The speed(feedrate) at which the move should happen.</param>
        public void moveExtruder(float mm, bool forward, float feedrate)
        {
            GCodeGenerator.moveExtruder(mm, forward, feedrate);
        }

        /// <summary>
        /// Home the x axis.
        /// </summary>
        public void homeXaxis()
        {
            GCodeGenerator.homeXaxis();
        }

        /// <summary>
        /// Home the y axis.
        /// </summary>
        public void homeYaxis()
        {
            GCodeGenerator.homeYaxis();
        }

        /// <summary>
        /// Home the z axis.
        /// </summary>
        public void homeZaxis()
        {
            GCodeGenerator.homeZaxis();
        }

        /// <summary>
        /// Home all axis.
        /// </summary>
        public void homeAll()
        {
            GCodeGenerator.homeAll();
        }

        /// <summary>
        /// Start heating the extruder to the given temperature.
        /// </summary>
        /// <param name="temperature">The target temperature.</param>
        public void heatExtruder(int temperature)
        {
            GCodeGenerator.heatExtruder(temperature);
        }

        /// <summary>
        /// Stop heating the extruder.
        /// </summary>
        public void coolExtruder()
        {
            GCodeGenerator.coolExtruder();
        }

        /// <summary>
        /// Prope the printer for its current hotend temperature.
        /// </summary>
        public void probeTemperature()
        {
            GCodeGenerator.probeTemperature();
        }

        /// <summary>
        /// Perform an emergency stop on the printer immediately
        /// </summary>
        public async void emergencyStop()
        {
            if (!bluetoothIO.isConnectedToDevice)
            {
                hostPage.Dispatcher.BeginInvoke(() => MessageBox.Show("You are not connected to a device yet"));
                return;
            }

            //Because this is an emergency stop which should happen immediately, we bypass the whole repetier buffer pipeline etc. and just send the command directly in ascii
            //TODO: resend checking is probably needed but will only be needed in cases of extremely bad communication
            await bluetoothIO.SendCommand("M112");
        }
    }
}
