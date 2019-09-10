using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RepRap_Phone_Host.GCode
{
    /// <summary>
    /// This class contains the function needed to probe
    /// printer temperature with GCode.
    /// </summary>
    static class GCodeTemperatureProber
    {
        /// <summary>
        /// A loop that sends GCode to probe the hotend temperature every given amount of milisenconds
        /// </summary>
        /// <param name="waitTime">Frequency to probe temperature</param>
        /// <param name="GCodeGenerator">The GCodegenrator to use for generating the GCode</param>
        public static void probeTemperature(int waitTime, GCodeGenerator GCodeGenerator)
        {
            while (true)
            {
                GCodeGenerator.probeTemperature();
                Thread.Sleep(waitTime);
            }
        }
    }
}
