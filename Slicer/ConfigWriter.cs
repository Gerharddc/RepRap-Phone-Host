using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using RepRap_Phone_Host.GlobalValues;
using System.IO.IsolatedStorage;

namespace RepRap_Phone_Host.Slicer
{
    /// <summary>
    /// This class is responsible for generating the config file for PolyChopper from the local settings
    /// </summary>
    static class ConfigWriter
    {
        /// <summary>
        /// This method generates the config file for PolyChopper from the local settings
        /// </summary>
        public static void writeConfigFile()
        {
            var isf = IsolatedStorageFile.GetUserStoreForApplication();

            IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream("chopperconf.txt", FileMode.Create, isf);

            StreamWriter streamWriter = new StreamWriter(isoStream);//"Confs/chopperconf.txt");

            //All settings need to multiplied in order to achieve the correct type of units
            streamWriter.WriteLine("bedWidth: " + Settings.bedWidth * 100000);
            streamWriter.WriteLine("bedLength: " + Settings.bedLength * 100000);
            streamWriter.WriteLine("bedHeigth: " + Settings.bedHeigth * 100000);
            streamWriter.WriteLine("printingTemperature: " + Settings.printingTemperature);
            streamWriter.WriteLine("bottomLayerCount: " + Settings.bottomLayerCount);
            streamWriter.WriteLine("topLayerCount: " + Settings.topLayerCount);
            streamWriter.WriteLine("flowPercentage: " + Settings.flowPercentage / 100);
            streamWriter.WriteLine("initialLayerSpeed: " + Settings.initialLayerSpeed * 60);
            streamWriter.WriteLine("normalLayerSpeed: " + Settings.normalLayerSpeed * 60);
            streamWriter.WriteLine("normalInfillSpeed: " + Settings.normalInfillSpeed * 60);
            streamWriter.WriteLine("bridgeingSpeed: " + Settings.bridgeingSpeed * 60);
            streamWriter.WriteLine("moveSpeed: " + Settings.moveSpeed * 60);
            streamWriter.WriteLine("normalInfillDensity: " + Settings.normalInfillDensity / 100);
            streamWriter.WriteLine("specialLayerSpeeds: " + Settings.specialLayerSpeeds);
            streamWriter.WriteLine("specialInfillSpeeds: " + Settings.specialInfillSpeeds);
            streamWriter.WriteLine("specialLayerDensities: " + Settings.specialLayerDensities);
            streamWriter.WriteLine("nozzleWidth: " + Settings.nozzleWidth * 100000);
            streamWriter.WriteLine("filamentWidth: " + Settings.filamentWidth * 100000);
            streamWriter.WriteLine("layerHeight: " + Settings.layerHeight * 100000);
            streamWriter.WriteLine("materialType: " + Settings.materialType);
            streamWriter.WriteLine("costPerKg: " + Settings.costPerKg);
            streamWriter.WriteLine("costPerMinute: " + Settings.costPerMinute);
            streamWriter.WriteLine("skirtSpeed: " + Settings.skirtSpeed * 60);
            streamWriter.WriteLine("skirtCount: " + Settings.skirtCount);
            streamWriter.WriteLine("skirtDistance: " + Settings.skirtDistance * 100000);
            streamWriter.WriteLine("shouldSkirt: " + Settings.shouldSkirt);
            streamWriter.WriteLine("shellThickness: " + Settings.shellThickness);
            streamWriter.WriteLine("shouldSupportMaterial: " + Settings.shouldSupportMaterial);
            streamWriter.WriteLine("supportMaterialDensity: " + Settings.supportMaterialDensity / 100);
            streamWriter.WriteLine("supportSpeed: " + Settings.supportSpeed * 60);
            streamWriter.WriteLine("retractionSpeed: " + Settings.retractionSpeed * 60);
            streamWriter.WriteLine("retractionDistance: " + Settings.retractionDistance * 100000);
            streamWriter.WriteLine("shouldRaft: " + Settings.shouldRaft);
            streamWriter.WriteLine("raftDensity: " + Settings.raftDensity / 100);
            streamWriter.WriteLine("raftDistance: " + Settings.raftDistance * 100000);
            streamWriter.WriteLine("raftCount: " + Settings.raftCount);
            streamWriter.WriteLine("raftSpeed: " + Settings.raftSpeed * 60);
            streamWriter.WriteLine("infillCombinationCount: " + Settings.infillCombinationCount);
            streamWriter.WriteLine("specialFlowrates: " + Settings.specialFlowrates);
            streamWriter.WriteLine("specialMoveSpeeds: " + Settings.specialMoveSpeeds);
            streamWriter.WriteLine("specialBridgeingSpeeds: " + Settings.specialBridgeingSpeeds);
            streamWriter.WriteLine("specialSupportDensities: " + Settings.specialSupportDensities);
            streamWriter.WriteLine("specialSupportSpeeds: " + Settings.specialSupportSpeeds);
            streamWriter.WriteLine("minimumLayerTime: " + Settings.minimumLayerTime * 60000);
            streamWriter.WriteLine("minimumLayerSpeed: " + Settings.minimumLayerSpeed * 60);

            streamWriter.Dispose();
            isoStream.Dispose();
        }
    }
}
