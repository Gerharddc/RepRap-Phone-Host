using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using PolyChopper;
using RepRap_Phone_Host;
using RepRap_Phone_Host.GlobalValues;
using System.Diagnostics;

namespace RepRap_Phone_Host.Slicer
{
    class PolyChopperSlicer
    {
        SlicerController slicerController;
        PolyChopper.PolyChopper polyChopper;

        public PolyChopperSlicer(SlicerController controller, ref Thread thread)
        {
            slicerController = controller;
            polyChopper = new PolyChopper.PolyChopper();

            polyChopper.logEvent += slicerController.handleSlicerOutput;
        }

        private void logevent(string message)
        {
            Debug.WriteLine("Log: " + message);
        }

        public void startSlicer(ref Thread thread, string input, string output)
        {
            //First write the config file
            ConfigWriter.writeConfigFile();

            thread = new Thread(new ThreadStart(() =>
                {
                    try
                    {
                        polyChopper.sliceFile(input, output, "chopperconf.txt");                        
                    }
                    catch (Exception e)
                    {
                        App.RootFrame.Dispatcher.BeginInvoke(() =>
                        {
                            System.Windows.MessageBox.Show(e.ToString());
                        });    
                    }
                    finally
                    {
                        slicerController.handleSlicerFinished();
                    }
                }));

            thread.Start();
        }

        public void stopSlicer(ref Thread thread)
        {
            thread.Abort();
        }
    }
}
