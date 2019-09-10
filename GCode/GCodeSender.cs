using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using System.Threading;
using System.Windows.Threading;
using RepRap_Phone_Host.Communication;
using RepRap_Phone_Host.GlobalValues;

//NOTE: we need to lock the dictionaries to avoid some strange errors when we start going through them at high speeds

namespace RepRap_Phone_Host.GCode
{
    /// <summary>
    /// This class does all the processing on a line of GCode needed for to be sent with a checksum.
    /// It also handles the resending of the line.
    /// </summary>
    class GCodeSender
    {
        BluetoothIO bluetoothIO;
        GCodeResponseReciever responseReciever;
        Int64 currentLineNumber = 0;
        Int64 lastOkLine = 0;
        Int64 currentResetLine = 0;
        string lastCommand = "";
        List<string> waitingCommandList;
        Dispatcher dispatcher;
        bool problemWithSending = false;
        int sizeOfCommandsInBuffer = 0;
        int bufferSize = 127;
        Dictionary<Int64, string> commandsInBuffer;
        Dictionary<Int64, int> bytesInBuffer;
        byte[] emptyBytes;
        bool haveToResendLines = false;
        bool someoneIsSendingTheList = false;//Indicates if an instance is already working on sending the list of waiting commands

        Timer commandSendingTimer;

        bool useRepetierprotocol = Settings.repetierProtocol; //Keep a cahced value of this setting because retrieving it every time would be to intensive

        public GCodeSender(BluetoothIO _bluetoothIO, GCodeResponseReciever _responseReciever, Dispatcher _dispatcher)
        {
            bluetoothIO = _bluetoothIO;
            responseReciever = _responseReciever;
            dispatcher = _dispatcher;

            waitingCommandList = new List<string>();
            commandsInBuffer = new Dictionary<Int64, string>();
            bytesInBuffer = new Dictionary<Int64, int>();

            emptyBytes = new byte[32];
            for (int i = 0; i < 32; i++)
                emptyBytes[i] = 0;

            responseReciever.OkRecieved += responseReciever_OkRecieved;
            responseReciever.ResendRecieved += responseReciever_ResendRecieved;
            bluetoothIO.ConnectedToDevice += bluetoothIO_ConnectedToDevice;

            commandSendingTimer = new Timer(new TimerCallback(this.sendWaitingCommands));
            commandSendingTimer.Change(0, 0);

            Settings.repetierProtocolChangedEvent += (o) => { useRepetierprotocol = (bool)o; };
        }

        void bluetoothIO_ConnectedToDevice()
        {
            trySendingGCode("M110 N0");
            responseReciever_OkRecieved(0);//remove this command from the buffer because it will never get an ok 0 automatically
        }

        static bool errorStateSent = false;

        async void responseReciever_ResendRecieved(int resendLine)
        {
            currentResetLine = resendLine;

            if (resendLine > currentLineNumber)
            {
                if (!errorStateSent)
                {
                    dispatcher.BeginInvoke(() => MessageBox.Show("Please reset your printer and the app because the printer is in an error state"));
                    errorStateSent = true;
                }

                problemWithSending = true;
                return;//Ignore previous error state
            }

            string lineToResend = null;

            lock (commandsInBuffer)
            {
                if (commandsInBuffer.ContainsKey(resendLine))
                {
                    haveToResendLines = true;//prevents the loop from sending new lines until the ones that should be in the buffer have been resent
                    sizeOfCommandsInBuffer = 0;//the buffer should now be empty and waiting for the lines to be resent'
                    currentLineNumber = resendLine + 1;//We need to send all commands after this line over

                    lineToResend = commandsInBuffer[resendLine];
                }
            }

            if (lineToResend != null)
            {
                haveToResendLines = true;//prevents the loop from sending new lines until the ones that should be in the buffer have been resent
                sizeOfCommandsInBuffer = 0;//the buffer should now be empty and waiting for the lines to be resent'
                currentLineNumber = resendLine + 1;//We need to send all commands after this line over

                lineToResend = commandsInBuffer[resendLine];

                GCode encodedCommand = new GCode(lineToResend);
                encodedCommand.N = resendLine;

                try
                {
                    await bluetoothIO.SendCommand(emptyBytes);//Send clean bytes to clean the buffer

                    var text = (useRepetierprotocol) ? encodedCommand.ToString() : encodedCommand.getAscii(true, false);

                    if (useRepetierprotocol)
                        await bluetoothIO.SendCommand(encodedCommand.getBinary(2));
                    else
                        await bluetoothIO.SendCommand(text + "\n");
                    
                    if (CommandSent != null)
                        CommandSent(text);
                }
                catch (Exception)
                {
                    dispatcher.BeginInvoke(() => MessageBox.Show("There was a problem resending the command"));
                }
            }
            else
            {
                //dispatcher.BeginInvoke(() => MessageBox.Show("There was an error resending a line and a blank one will be sent in its place"));

                GCode encodedCommand = new GCode("M119");//If we don't have the command in our list then just send this endstop checking line simply to move on to the next line
                encodedCommand.N = resendLine;

                try
                {
                    await bluetoothIO.SendCommand(emptyBytes);//Send clean bytes to clean the buffer

                    var text = (useRepetierprotocol) ? encodedCommand.ToString() : encodedCommand.getAscii(true, false);

                    if (useRepetierprotocol)
                        await bluetoothIO.SendCommand(encodedCommand.getBinary(2));
                    else
                        await bluetoothIO.SendCommand(text + "\n");

                    if (CommandSent != null)
                        CommandSent(text);
                }
                catch (Exception)
                {
                    dispatcher.BeginInvoke(() => MessageBox.Show("There was a problem resending the command"));
                }
            }
        }

        /// <summary>
        /// This method is used to resend the lines that should be in there buffer (this is the buffer seems to get formatted
        /// once a corrupt message is recieved)
        /// </summary>
        void resendOtherCommands()
        {
            for (int i = 1; i <= commandsInBuffer.Count; i++)
            {
                if (commandsInBuffer.ContainsKey(currentResetLine + i))
                    waitingCommandList.Insert(i - 1, commandsInBuffer[currentResetLine + i]);
            }

            haveToResendLines = false;//allows to loop to resume sothat it can resent the needed commands as well as the others that were queued
        }

        void responseReciever_OkRecieved(int okLine)
        {
            lastOkLine = okLine;

            if (okLine == currentResetLine)
                resendOtherCommands();

            int timesTried = 0;

            lock (commandsInBuffer)
            {
                //TODO: make sure the line is removed completely

                if (commandsInBuffer.ContainsKey(okLine))
                {
                    //If we recieved ok for a command and the command contains coordinates then it means that the printhead should be at the position
                    //specified by the command
                    GCode command = new GCode(commandsInBuffer[okLine]);

                    if (command.hasX)
                        Values.currentPos_X = command.X;

                    if (command.hasY)
                        Values.currentPos_Y = command.Y;

                    if (command.hasZ)
                        Values.currentPos_Z = command.Z;

                    commandsInBuffer.Remove(okLine);
                }
            }

            timesTried = 0;

            lock (bytesInBuffer)
            {
                //Sometimes the ok comes in just a few milis before the command was fully added to list
                while (!bytesInBuffer.ContainsKey(okLine) && timesTried < 3)
                {
                    Thread.Sleep(1);
                    timesTried++;
                }

                //If we have tried removing the command 100 times and it still is not there then for some reason it was probably never added
                if (bytesInBuffer.ContainsKey(okLine) && timesTried < 3)
                {
                    sizeOfCommandsInBuffer -= bytesInBuffer[okLine];
                    bytesInBuffer.Remove(okLine);
                }
                else if (timesTried >= 10)
                    sizeOfCommandsInBuffer -= 19;//If the command was never in the list then we assume 19 bytes were added in the buffer for it

                //Recalculate the size of bytes in the buffer
                int tempSize = 0;

                try
                {
                    Dictionary<long, int> tempList = new Dictionary<long, int>(bytesInBuffer);

                    //Determine if there are any lines thatare way to old and remove them
                    foreach (long lineNum in tempList.Keys)
                    {
                        if ((currentLineNumber - lineNum) > 10)
                            bytesInBuffer.Remove(lineNum);
                    }

                    //Recalculate the amount of bytes in the buffer
                    foreach (int ding in bytesInBuffer.Values)
                    {
                        tempSize += ding;
                    }

                    sizeOfCommandsInBuffer = tempSize;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                    Debug.WriteLine(e.Message);
                }
            }
        }

        public bool trySendingGCode(string GCodeToSend)
        {
            if (GCodeToSend == null)
                return true;//Move on to the next line because we cannot send null lines
            
            if (!bluetoothIO.isConnectedToDevice)
            {
                dispatcher.BeginInvoke(() => MessageBox.Show("You are not connected to a device yet"));
                return false;
            }
            
            waitingCommandList.Add(GCodeToSend);

            //Wait until this line has been recieved by the device before adding another one to the waiting list
            while (waitingCommandList.Count > 0 && !problemWithSending)
            {
                Thread.Sleep(0);
            }

            var returnValue = !problemWithSending;
            problemWithSending = false;
            return returnValue;
        }

        /// <summary>
        /// This method sends all the waiting commands once spcae becomes available on the recieving buffer
        /// </summary>
        /// <param name="state"></param>
        async void sendWaitingCommands(object state)
        {
            commandSendingTimer.Change(Timeout.Infinite, 0);
            int waitTime = 0;//amount of time we should wait before emptying the list again

            if (waitingCommandList.Count > 0 && !problemWithSending && !haveToResendLines)
            {
                while (waitingCommandList.Count > 0 && !problemWithSending && !haveToResendLines)
                {
                    if (await sendCommandWhenSpace(waitingCommandList[0]))//Try sending the top line
                    {
                        waitingCommandList.Remove(waitingCommandList[0]);//Only remove the command if it was sent correctly
                        currentLineNumber++;//increase the current line number
                    }
                    else if (waitingCommandList[0] == null)
                    {
                        waitingCommandList.Remove(waitingCommandList[0]);//Remove the empty command
                    }
                }
            }
            else
                waitTime = 1;//wait a second because we are probably not printing yet and should take some strain off off the cpu

            //CheckStatus(null);
            commandSendingTimer.Change(waitTime, 0);
        }

        async void sendWaitingCommands()
        {
            while (true)
            {
                if (waitingCommandList.Count > 0 && !problemWithSending && !haveToResendLines)
                {
                    if (await sendCommandWhenSpace(waitingCommandList[0]))//Try sending the top line
                    {
                        waitingCommandList.Remove(waitingCommandList[0]);//Only remove the command if it was sent correctly
                        currentLineNumber++;//increase the current line number
                    }
                    else if (waitingCommandList[0] == null)
                    {
                        waitingCommandList.Remove(waitingCommandList[0]);//Remove the empty command
                        //waitingCommandList[0] = lastCommand;//Resend the previous line as a last resort
                    }
                }
                else
                    Thread.Sleep(5);//Let the cpu relax if we do not currently have a lot of stuff int the waiting list
            }
        }

        //This event is raised once a full GCode command has been sent
        public event CommandSentHandler CommandSent;
        public delegate void CommandSentHandler(string command);

        async Task<bool> sendCommandWhenSpace(string command)
        {
            if (command == null)
                return false;

            lastCommand = command;

            GCode encodedCommand = new GCode(command);
            encodedCommand.N = (int)currentLineNumber;

            var text = (useRepetierprotocol) ? encodedCommand.ToString() : encodedCommand.getAscii(true, false);

            var commandBytes = encodedCommand.getBinary(2);
            if (!useRepetierprotocol)
                commandBytes = Encoding.UTF8.GetBytes(text);
            int commandSize = commandBytes.Length;

            if (useRepetierprotocol)
            {
                while (commandSize + sizeOfCommandsInBuffer > bufferSize && !haveToResendLines)//Check if the command will fit into the buffer
                {
                    Thread.Sleep(0);
                }
            }
            else
            {
                while (sizeOfCommandsInBuffer > 50)
                    Thread.Sleep(0);
            }

            if (haveToResendLines)
                return false;//Let the lines that have to be resent be sent first

            try
            {
                if (useRepetierprotocol)
                    await bluetoothIO.SendCommand(commandBytes);
                else
                    await bluetoothIO.SendCommand(text + "\n");

                if (CommandSent != null)
                    CommandSent(text);

                //Check if the command sets the temperature then we need to refelect that
                dispatcher.BeginInvoke(() =>
                {
                    if (encodedCommand.hasS)
                    {
                        if (encodedCommand.S == 0)
                            Values.isHeating = false;
                        else
                        {
                            Values.isHeating = true;
                            Settings.printingTemperature = (short)encodedCommand.S;
                        }
                    }
                });
            }
            catch (Exception)
            {
                dispatcher.BeginInvoke(() => MessageBox.Show("There was an error sending the command"));
                problemWithSending = true;
                return false;
            }

            int timesTried = 0;//If we have already tied adding the command for more than 100 times then it will probably never be added

            //For some reason they are sometimes not added to the list so we try until they are
            /*while (!commandsInBuffer.ContainsKey(currentLineNumber) && timesTried < 10)
            {
                try
                {
                    commandsInBuffer.Add(currentLineNumber, command);
                }
                catch (Exception) { Thread.Sleep(10); timesTried++; }//Give the list time to sort out its nonsense
            }*/
            try
            {
                lock (commandsInBuffer)
                {
                    commandsInBuffer.Add(currentLineNumber, command);
                }
            }
            catch (Exception e) { Debug.WriteLine(e); }

            timesTried = 0;

            /*while (!bytesInBuffer.ContainsKey(currentLineNumber) && timesTried < 10)
            {
                try
                {
                    bytesInBuffer.Add(currentLineNumber, commandSize);
                }
                catch (Exception) { Thread.Sleep(10); timesTried++; }//Give the list time to sort out its nonsense
            }*/
            try
            {
                lock (bytesInBuffer)
                {
                    bytesInBuffer.Add(currentLineNumber, commandSize);
                }
            }
            catch (Exception e) { Debug.WriteLine(e); }

            if (timesTried < 100)
                sizeOfCommandsInBuffer += commandSize;//If the bytes were added to the list then their real size can be added to the estimated buffer size
            else
                sizeOfCommandsInBuffer += 19;//Otherwise 19 is a very good average size

            return true;
        }
    }
}
