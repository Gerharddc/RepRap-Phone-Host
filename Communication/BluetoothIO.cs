using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Windows.Networking.Sockets;
using Windows.Networking.Proximity;
using Windows.Storage.Streams;
using Windows.Networking;
using System.Diagnostics;
using System.Threading;

namespace RepRap_Phone_Host.Communication
{
    /// <summary>
    /// The class used to phusically setup a connection and communicate witha device.
    /// </summary>
    class BluetoothIO
    {
        private bool isConnected = false;//We need to know if we are currently connected

        private StreamSocket socket;//This is the socket we will use to communicate
        private DataWriter dataWriter;//We will use this to write data to the socket
        private DataReader dataReader;//We will use this to read data from the socket
        private BackgroundWorker dataReadWorker;//Reading of the socket will take place in the background though this backgroundworker
        private StringBuilder recievedLine;//This stringbuilder is used to read messages line for line
        private int amountOfCommandsInBuffer = 0;//This is the amount of commands that are currently in the buffer

        /// <summary>
        /// Initialize a new BluetoothIO
        /// </summary>
        public BluetoothIO()
        {
            //Create the needed elements and connect the needed eventhandlers
            socket = new StreamSocket();
            dataReadWorker = new BackgroundWorker();
            dataReadWorker.WorkerSupportsCancellation = true;
            dataReadWorker.DoWork += new DoWorkEventHandler(ReceiveMessages);
            recievedLine = new StringBuilder();

            //Timer sendingTimer = new Timer(tcb, autoEvent, 50, 50);
            Timer sendingTimer = new Timer(new TimerCallback(this.timerCallback), null, 25, 25);
        }

        //This method should send all waiting commands and clear the list of known commands
        private async void timerCallback(object state)
        {
            try
            {
                if (dataWriter != null && amountOfCommandsInBuffer != 0)
                {
                    await dataWriter.FlushAsync();
                    amountOfCommandsInBuffer = 0;
                }
            }
            catch (Exception) { }
        }

        //This event is raised when a connection to a device is established
        public event ConnectedToDeviceHandler ConnectedToDevice;
        public delegate void ConnectedToDeviceHandler();

        //This event is raised when a connection to a device fails
        public event ConnectFailedHandler ConnectionFailed;
        public delegate void ConnectFailedHandler();

        /// <summary>
        /// This method connects the BluetoothIO instance to a given bluetooth device
        /// </summary>
        /// <param name="deviceHostName">The hostname of the device to connect to.</param>
        public async void ConnectToDevice(HostName deviceHostName)
        {
            if (socket != null)
            {
                try
                {
                    await socket.ConnectAsync(deviceHostName, "1");
                    dataReader = new DataReader(socket.InputStream);
                    dataReadWorker.RunWorkerAsync();
                    dataWriter = new DataWriter(socket.OutputStream);

                    isConnected = true;

                    if (ConnectedToDevice != null)
                        ConnectedToDevice();
                }
                catch (Exception e)
                {
                    ConnectionFailed();
                }
            }
        }

        //This event is raised when we have finished disconnecting from the connected device
        public event DisconnectedFromDeviceHandler DiconnectedFromDevice;
        public delegate void DisconnectedFromDeviceHandler();

        /// <summary>
        /// Disconnects from the currently connected device.
        /// </summary>
        public void DisconnectFromDevice()
        {
            //We cannot disconnect if we are not connected yet
            if (!isConnected)
                return;

            //TODO: Implemet a way to disconnect from the device properly

            if (socket != null)
            {
                try
                {
                    dataWriter.Dispose();
                    dataReadWorker.CancelAsync();
                    dataReader.Dispose();
                    socket.Dispose();

                    if (DiconnectedFromDevice != null)
                        DiconnectedFromDevice();

                    isConnected = false;
                }
                catch (Exception e)
                {
                }
            }
        }

        //This event is raised when we recieve a line of text from the device
        public event RecievedMessageHandler RecievedMessage;
        public delegate void RecievedMessageHandler(string message);

        /// <summary>
        /// This is used to recieve bytes from the device and raise an event when we have an entire line of text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ReceiveMessages(object sender, DoWorkEventArgs e)
        {
            try
            {
                while (true)
                {
                    uint sizeFieldCount = await dataReader.LoadAsync(1);
                    if (sizeFieldCount != 1)
                    {
                        return;
                    }

                    string character = dataReader.ReadString(1);

                    //RecievedMessage(character);
                    if (character == "\n" || character == "\r")
                    {
                        if (RecievedMessage != null)
                            RecievedMessage(recievedLine.ToString());

                        recievedLine.Clear();
                    }
                    else
                        recievedLine.Append(character);
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// This is used to send a string to the device
        /// </summary>
        /// <param name="command">The string that should be sent.</param>
        /// <returns>The size of the command that was sent.</returns>
        public async Task<uint> SendCommand(string command)
        {
            if (!isConnected)
                throw new Exception("You are not connected yet");

            uint sentCommandSize = 0;
            if (dataWriter != null)
            {
                uint commandSize = dataWriter.MeasureString(command);
                dataWriter.WriteByte((byte)commandSize);
                sentCommandSize = dataWriter.WriteString(command);
                await dataWriter.StoreAsync();
                amountOfCommandsInBuffer += 1;

                if (amountOfCommandsInBuffer >= 3)
                {
                    await dataWriter.FlushAsync();
                    amountOfCommandsInBuffer = 0;
                }
            }
            return sentCommandSize;
        }
        /// <summary>
        /// This is used to send an array of bytes to the device
        /// </summary>
        /// <param name="command">The array of bytes which form the command</param>
        /// <returns>The size of the command that was sent</returns>
        public async Task<uint> SendCommand(byte[] command)
        {
            if (!isConnected)
                throw new Exception("You are not connected yet");

            uint sentCommandSize = 0;
            if (dataWriter != null)
            {
                dataWriter.WriteBytes(command);
                sentCommandSize = (uint)(command.Length * sizeof(byte));//Calculate the size of the command
                await dataWriter.StoreAsync();
                amountOfCommandsInBuffer += 1;

                if (amountOfCommandsInBuffer >= 3)
                {
                    await dataWriter.FlushAsync();
                    amountOfCommandsInBuffer = 0;
                }
            }
            return sentCommandSize;
        }

        /// <summary>
        /// Determine if the BluetoothIO instance is connected to a device
        /// </summary>
        public bool isConnectedToDevice
        {
            get
            {
                return isConnected;
            }
        }
    }
}
