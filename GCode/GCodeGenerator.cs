using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RepRap_Phone_Host.GCode
{
    /// <summary>
    /// This class contains all the functions needed to generate and send GCode
    /// for a particalur single line command
    /// </summary>
    class GCodeGenerator
    {
        GCodeSender GCodeSender;//The GCode sender will be used to prepare the GCode for sending and resending if needed

        /// <summary>
        /// This class generates the GCode for a given commands and automatically puts
        /// it through the sending process.
        /// </summary>
        /// <param name="_GCodeSender">The fully initialised GCodeSender that should be used to send  the GCode.</param>
        public GCodeGenerator(GCodeSender _GCodeSender)
        {
            GCodeSender = _GCodeSender;
        }

        /// <summary>
        /// Move the x axis with the given parameters.
        /// </summary>
        /// <param name="mm">The distance to move.</param>
        /// <param name="forward">Is the direction of the move forward?</param>
        /// <param name="feedrate">The speed(feedrate) at which the move should happen.</param>
        public void moveXaxis(float mm, bool forward, float feedrate)
        {
            string dir = (forward) ? "" : "-";
            var GCode = "G1 X" + dir + mm + " F" + feedrate;

            //Only send the following line if the previous was successful
            if (GCodeSender.trySendingGCode("G91"))
                if (GCodeSender.trySendingGCode(GCode))
                    GCodeSender.trySendingGCode("G90");
        }

        /// <summary>
        /// Move the y axis with the given parameters.
        /// </summary>
        /// <param name="mm">The distance to move.</param>
        /// <param name="forward">Is the direction of the move forward?</param>
        /// <param name="feedrate">The speed(feedrate) at which the move should happen.</param>
        public void moveYaxis(float mm, bool forward, float feedrate)
        {
            var distance = mm.ToString();
            if (!distance.Contains('.'))
                distance += ".0";

            var speed = feedrate.ToString();
            if (!speed.Contains('.'))
                speed += ".0";

            string dir = (forward) ? "" : "-";
            var GCode = "G1 Y" + dir + distance + " F" + speed;

            //Only send the following line if the previous was successful
            if (GCodeSender.trySendingGCode("G91"))
                if (GCodeSender.trySendingGCode(GCode))
                    GCodeSender.trySendingGCode("G90");
        }

        /// <summary>
        /// Move the z axis with the given parameters.
        /// </summary>
        /// <param name="mm">The distance to move.</param>
        /// <param name="forward">Is the direction of the move forward?</param>
        /// <param name="feedrate">The speed(feedrate) at which the move should happen.</param>
        public void moveZaxis(float mm, bool forward, float feedrate)
        {
            var distance = mm.ToString();
            if (!distance.Contains('.'))
                distance += ".0";

            var speed = feedrate.ToString();
            if (!speed.Contains('.'))
                speed += ".0";

            string dir = (forward) ? "" : "-";
            var GCode = "G1 Z" + dir + distance + " F" + speed;

            //Only send the following line if the previous was successful
            if (GCodeSender.trySendingGCode("G91"))
                if (GCodeSender.trySendingGCode(GCode))
                    GCodeSender.trySendingGCode("G90");
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
            switch (axis)
            {
                case "X":
                    moveXaxis(mm, forward, feedrate);
                    break;

                case "Y":
                    moveYaxis(mm, forward, feedrate);
                    break;

                case "Z":
                    moveZaxis(mm, forward, feedrate);
                    break;

                case "E":
                    moveExtruder(mm, forward, feedrate);
                    break;
            }
        }

        /// <summary>
        /// Move the extruder with the given parameters.
        /// </summary>
        /// <param name="mm">The distance to move.</param>
        /// <param name="forward">Is the direction of the move forward?</param>
        /// <param name="feedrate">The speed(feedrate) at which the move should happen.</param>
        public void moveExtruder(float mm, bool forward, float feedrate)
        {
            var distance = mm.ToString();
            if (!distance.Contains('.'))
                distance += ".0";

            var speed = feedrate.ToString();
            if (!speed.Contains('.'))
                speed += ".0";

            string dir = (forward) ? "" : "-";
            var GCode = "G1 E" + dir + distance + " F" + speed;

            if (GCodeSender.trySendingGCode("G91"))
                if (GCodeSender.trySendingGCode(GCode))
                    GCodeSender.trySendingGCode("G90");
        }

        /// <summary>
        /// Home the x axis.
        /// </summary>
        public void homeXaxis()
        {
            GCodeSender.trySendingGCode("G28 X0");
        }

        /// <summary>
        /// Home the y axis.
        /// </summary>
        public void homeYaxis()
        {
            GCodeSender.trySendingGCode("G28 Y0");
        }

        /// <summary>
        /// Home the z axis.
        /// </summary>
        public void homeZaxis()
        {
            GCodeSender.trySendingGCode("G28 Z0");
        }

        /// <summary>
        /// Home all axis.
        /// </summary>
        public void homeAll()
        {
            GCodeSender.trySendingGCode("G28 X0 Y0 Z0");
        }

        /// <summary>
        /// Start heating the extruder to the given temperature.
        /// </summary>
        /// <param name="temperature">The target temperature.</param>
        public void heatExtruder(int temperature)
        {
            GCodeSender.trySendingGCode("M104 S" + temperature);
        }

        /// <summary>
        /// Stop heating the extruder.
        /// </summary>
        public void coolExtruder()
        {
            GCodeSender.trySendingGCode("M104 S0");
        }

        /// <summary>
        /// Perform an emergency stop on the printer.
        /// </summary>
        public void emergencyStop()
        {
            GCodeSender.trySendingGCode("M112");
        }

        /// <summary>
        /// Prope the printer for its current hotend temperature.
        /// </summary>
        public void probeTemperature()
        {
            GCodeSender.trySendingGCode("M105");
        }
    }
}
