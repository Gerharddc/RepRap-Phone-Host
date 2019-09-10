using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.IsolatedStorage;

namespace RepRap_Phone_Host.GlobalValues
{
    public static class Settings
    {
        public delegate void settingChangedEvent(object value);

        #region slicer settings
        public static settingChangedEvent printingTemperatureChangedEvent;
        /// <summary>
        /// This value determines the printing temperature
        /// </summary>
        public static short printingTemperature
        {
            get
            {
                if (!IsolatedStorageSettings.ApplicationSettings.Contains("printingTemperature"))
                {
                    IsolatedStorageSettings.ApplicationSettings["printingTemperature"] = (short)200; //Default value
                    IsolatedStorageSettings.ApplicationSettings.Save();
                }

                return (short)IsolatedStorageSettings.ApplicationSettings["printingTemperature"];
            }
            set
            {
                if (printingTemperature == value)
                    return;

                IsolatedStorageSettings.ApplicationSettings["printingTemperature"] = value;
                IsolatedStorageSettings.ApplicationSettings.Save();

                if (printingTemperatureChangedEvent != null)
                    printingTemperatureChangedEvent(value);
            }
        }

        public static settingChangedEvent bottomLayerCountChangedEvent;
        /// <summary>
        /// This value determines the amount of bottom layers that the slicer should create
        /// </summary>
        public static int bottomLayerCount
        {
            get
            {
                if (!IsolatedStorageSettings.ApplicationSettings.Contains("bottomLayerCount"))
                {
                    IsolatedStorageSettings.ApplicationSettings["bottomLayerCount"] = 4; //Default value
                    IsolatedStorageSettings.ApplicationSettings.Save();
                }

                return (int)IsolatedStorageSettings.ApplicationSettings["bottomLayerCount"];
            }
            set
            {
                if (bottomLayerCount == value)
                    return;

                IsolatedStorageSettings.ApplicationSettings["bottomLayerCount"] = value;
                IsolatedStorageSettings.ApplicationSettings.Save();

                if (bottomLayerCountChangedEvent != null)
                    bottomLayerCountChangedEvent(value);
            }
        }

        public static settingChangedEvent topLayerCountChangedEvent;
        /// <summary>
        /// This value determines the amount of top layers that the slicer should create
        /// </summary>
        public static int topLayerCount
        {
            get
            {
                if (!IsolatedStorageSettings.ApplicationSettings.Contains("topLayerCount"))
                {
                    IsolatedStorageSettings.ApplicationSettings["topLayerCount"] = 4; //Default value
                    IsolatedStorageSettings.ApplicationSettings.Save();
                }

                return (int)IsolatedStorageSettings.ApplicationSettings["topLayerCount"];
            }
            set
            {
                if (topLayerCount == value)
                    return;

                IsolatedStorageSettings.ApplicationSettings["topLayerCount"] = value;
                IsolatedStorageSettings.ApplicationSettings.Save();

                if (topLayerCountChangedEvent != null)
                    topLayerCountChangedEvent(value);
            }
        }

        public static settingChangedEvent flowPercentageChangedEvent;
        /// <summary>
        /// This value determines the filament flow multiplier
        /// </summary>
        public static float flowPercentage
        {
            get
            {
                if (!IsolatedStorageSettings.ApplicationSettings.Contains("flowPercentage"))
                {
                    IsolatedStorageSettings.ApplicationSettings["flowPercentage"] = 100.0f; //Default value
                    IsolatedStorageSettings.ApplicationSettings.Save();
                }

                return (float)IsolatedStorageSettings.ApplicationSettings["flowPercentage"];
            }
            set
            {
                if (flowPercentage == value)
                    return;

                IsolatedStorageSettings.ApplicationSettings["flowPercentage"] = value;
                IsolatedStorageSettings.ApplicationSettings.Save();

                if (flowPercentageChangedEvent != null)
                    flowPercentageChangedEvent(value);
            }
        }

        public static settingChangedEvent initialLayerSpeedChangedEvent;
        /// <summary>
        /// This value determines the initial layer speed
        /// </summary>
        public static float initialLayerSpeed
        {
            get
            {
                if (!IsolatedStorageSettings.ApplicationSettings.Contains("initialLayerSpeed"))
                {
                    IsolatedStorageSettings.ApplicationSettings["initialLayerSpeed"] = 10.0f; //Default value
                    IsolatedStorageSettings.ApplicationSettings.Save();
                }

                return (float)IsolatedStorageSettings.ApplicationSettings["initialLayerSpeed"];
            }
            set
            {
                if (initialLayerSpeed == value)
                    return;

                IsolatedStorageSettings.ApplicationSettings["initialLayerSpeed"] = value;
                IsolatedStorageSettings.ApplicationSettings.Save();

                if (initialLayerSpeedChangedEvent != null)
                    initialLayerSpeedChangedEvent(value);
            }
        }

        public static settingChangedEvent normalLayerSpeedChangedEvent;
        /// <summary>
        /// This value determines the normal layer speed
        /// </summary>
        public static float normalLayerSpeed
        {
            get
            {
                if (!IsolatedStorageSettings.ApplicationSettings.Contains("normalLayerSpeed"))
                {
                    IsolatedStorageSettings.ApplicationSettings["normalLayerSpeed"] = 20.0f; //Default value
                    IsolatedStorageSettings.ApplicationSettings.Save();
                }

                return (float)IsolatedStorageSettings.ApplicationSettings["normalLayerSpeed"];
            }
            set
            {
                if (normalLayerSpeed == value)
                    return;

                IsolatedStorageSettings.ApplicationSettings["normalLayerSpeed"] = value;
                IsolatedStorageSettings.ApplicationSettings.Save();

                if (normalLayerSpeedChangedEvent != null)
                    normalLayerSpeedChangedEvent(value);
            }
        }

        public static settingChangedEvent normalInfillSpeedChangedEvent;
        /// <summary>
        /// This value determines the normal infill speed
        /// </summary>
        public static float normalInfillSpeed
        {
            get
            {
                if (!IsolatedStorageSettings.ApplicationSettings.Contains("normalInfillSpeed"))
                {
                    IsolatedStorageSettings.ApplicationSettings["normalInfillSpeed"] = 34.0f; //Default value
                    IsolatedStorageSettings.ApplicationSettings.Save();
                }

                return (float)IsolatedStorageSettings.ApplicationSettings["normalInfillSpeed"];
            }
            set
            {
                if (normalInfillSpeed == value)
                    return;

                IsolatedStorageSettings.ApplicationSettings["normalInfillSpeed"] = value;
                IsolatedStorageSettings.ApplicationSettings.Save();

                if (normalInfillSpeedChangedEvent != null)
                    normalInfillSpeedChangedEvent(value);
            }
        }

        public static settingChangedEvent bridgeingSpeedChangedEvent;
        /// <summary>
        /// This value determines the bridgeing speed
        /// </summary>
        public static float bridgeingSpeed
        {
            get
            {
                if (!IsolatedStorageSettings.ApplicationSettings.Contains("bridgeingSpeed"))
                {
                    IsolatedStorageSettings.ApplicationSettings["bridgeingSpeed"] = 40.0f; //Default value
                    IsolatedStorageSettings.ApplicationSettings.Save();
                }

                return (float)IsolatedStorageSettings.ApplicationSettings["bridgeingSpeed"];
            }
            set
            {
                if (bridgeingSpeed == value)
                    return;

                IsolatedStorageSettings.ApplicationSettings["bridgeingSpeed"] = value;
                IsolatedStorageSettings.ApplicationSettings.Save();

                if (bridgeingSpeedChangedEvent != null)
                    bridgeingSpeedChangedEvent(value);
            }
        }

        public static settingChangedEvent moveSpeedChangedEvent;
        /// <summary>
        /// This value determines the move speed
        /// </summary>
        public static float moveSpeed
        {
            get
            {
                if (!IsolatedStorageSettings.ApplicationSettings.Contains("moveSpeed"))
                {
                    IsolatedStorageSettings.ApplicationSettings["moveSpeed"] = 36.0f; //Default value
                    IsolatedStorageSettings.ApplicationSettings.Save();
                }

                return (float)IsolatedStorageSettings.ApplicationSettings["moveSpeed"];
            }
            set
            {
                if (moveSpeed == value)
                    return;

                IsolatedStorageSettings.ApplicationSettings["moveSpeed"] = value;
                IsolatedStorageSettings.ApplicationSettings.Save();

                if (moveSpeedChangedEvent != null)
                    moveSpeedChangedEvent(value);
            }
        }

        public static settingChangedEvent normalInfillDensityChangedEvent;
        /// <summary>
        /// This value determines the normal infill density
        /// </summary>
        public static float normalInfillDensity
        {
            get
            {
                if (!IsolatedStorageSettings.ApplicationSettings.Contains("normalInfillDensity"))
                {
                    IsolatedStorageSettings.ApplicationSettings["normalInfillDensity"] = 10.0f; //Default value
                    IsolatedStorageSettings.ApplicationSettings.Save();
                }

                return (float)IsolatedStorageSettings.ApplicationSettings["normalInfillDensity"];
            }
            set
            {
                if (normalInfillDensity == value)
                    return;

                IsolatedStorageSettings.ApplicationSettings["normalInfillDensity"] = value;
                IsolatedStorageSettings.ApplicationSettings.Save();

                if (normalInfillDensityChangedEvent != null)
                    normalInfillDensityChangedEvent(value);
            }
        }

        public static settingChangedEvent specialLayerSpeedsChangedEvent;
        /// <summary>
        /// This value determines the special layer speeds
        /// </summary>
        public static string specialLayerSpeeds
        {
            get
            {
                if (!IsolatedStorageSettings.ApplicationSettings.Contains("specialLayerSpeeds"))
                {
                    IsolatedStorageSettings.ApplicationSettings["specialLayerSpeeds"] = ""; //Default value
                    IsolatedStorageSettings.ApplicationSettings.Save();
                }

                return (string)IsolatedStorageSettings.ApplicationSettings["specialLayerSpeeds"];
            }
            set
            {
                if (specialLayerSpeeds == value)
                    return;

                IsolatedStorageSettings.ApplicationSettings["specialLayerSpeeds"] = value;
                IsolatedStorageSettings.ApplicationSettings.Save();

                if (specialLayerSpeedsChangedEvent != null)
                    specialLayerSpeedsChangedEvent(value);
            }
        }

        public static settingChangedEvent specialInfillSpeedsChangedEvent;
        /// <summary>
        /// This value determines the special infill speeds
        /// </summary>
        public static string specialInfillSpeeds
        {
            get
            {
                if (!IsolatedStorageSettings.ApplicationSettings.Contains("specialInfillSpeeds"))
                {
                    IsolatedStorageSettings.ApplicationSettings["specialInfillSpeeds"] = ""; //Default value
                    IsolatedStorageSettings.ApplicationSettings.Save();
                }

                return (string)IsolatedStorageSettings.ApplicationSettings["specialInfillSpeeds"];
            }
            set
            {
                if (specialInfillSpeeds == value)
                    return;

                IsolatedStorageSettings.ApplicationSettings["specialInfillSpeeds"] = value;
                IsolatedStorageSettings.ApplicationSettings.Save();

                if (specialInfillSpeedsChangedEvent != null)
                    specialInfillSpeedsChangedEvent(value);
            }
        }

        public static settingChangedEvent specialLayerDensitiesChangedEvent;
        /// <summary>
        /// This value determines the special layer densities
        /// </summary>
        public static string specialLayerDensities
        {
            get
            {
                if (!IsolatedStorageSettings.ApplicationSettings.Contains("specialLayerDensities"))
                {
                    IsolatedStorageSettings.ApplicationSettings["specialLayerDensities"] = ""; //Default value
                    IsolatedStorageSettings.ApplicationSettings.Save();
                }

                return (string)IsolatedStorageSettings.ApplicationSettings["specialLayerDensities"];
            }
            set
            {
                if (specialLayerDensities == value)
                    return;

                IsolatedStorageSettings.ApplicationSettings["specialLayerDensities"] = value;
                IsolatedStorageSettings.ApplicationSettings.Save();

                if (specialLayerDensitiesChangedEvent != null)
                    specialLayerDensitiesChangedEvent(value);
            }
        }

        public static settingChangedEvent nozzleWidthChangedEvent;
        /// <summary>
        /// This value determines the nozzle width
        /// </summary>
        public static float nozzleWidth
        {
            get
            {
                if (!IsolatedStorageSettings.ApplicationSettings.Contains("nozzleWidth"))
                {
                    IsolatedStorageSettings.ApplicationSettings["nozzleWidth"] = 0.5f; //Default value
                    IsolatedStorageSettings.ApplicationSettings.Save();
                }

                return (float)IsolatedStorageSettings.ApplicationSettings["nozzleWidth"];
            }
            set
            {
                if (nozzleWidth == value)
                    return;

                IsolatedStorageSettings.ApplicationSettings["nozzleWidth"] = value;
                IsolatedStorageSettings.ApplicationSettings.Save();

                if (nozzleWidthChangedEvent != null)
                    nozzleWidthChangedEvent(value);
            }
        }

        public static settingChangedEvent filamentWidthChangedEvent;
        /// <summary>
        /// This value determines the filament width
        /// </summary>
        public static float filamentWidth
        {
            get
            {
                if (!IsolatedStorageSettings.ApplicationSettings.Contains("filamentWidth"))
                {
                    IsolatedStorageSettings.ApplicationSettings["filamentWidth"] = 2.8f; //Default value
                    IsolatedStorageSettings.ApplicationSettings.Save();
                }

                return (float)IsolatedStorageSettings.ApplicationSettings["filamentWidth"];
            }
            set
            {
                if (filamentWidth == value)
                    return;

                IsolatedStorageSettings.ApplicationSettings["filamentWidth"] = value;
                IsolatedStorageSettings.ApplicationSettings.Save();

                if (filamentWidthChangedEvent != null)
                    filamentWidthChangedEvent(value);
            }
        }

        public static settingChangedEvent layerHeightChangedEvent;
        /// <summary>
        /// This value determines the layer height
        /// </summary>
        public static float layerHeight
        {
            get
            {
                if (!IsolatedStorageSettings.ApplicationSettings.Contains("layerHeight"))
                {
                    IsolatedStorageSettings.ApplicationSettings["layerHeight"] = 0.2f; //Default value
                    IsolatedStorageSettings.ApplicationSettings.Save();
                }

                return (float)IsolatedStorageSettings.ApplicationSettings["layerHeight"];
            }
            set
            {
                if (layerHeight == value)
                    return;

                IsolatedStorageSettings.ApplicationSettings["layerHeight"] = value;
                IsolatedStorageSettings.ApplicationSettings.Save();

                if (layerHeightChangedEvent != null)
                    layerHeightChangedEvent(value);
            }
        }

        public static settingChangedEvent materialTypeChangedEvent;
        /// <summary>
        /// This value determines the type of material used
        /// </summary>
        public static string materialType
        {
            get
            {
                if (!IsolatedStorageSettings.ApplicationSettings.Contains("materialType"))
                {
                    IsolatedStorageSettings.ApplicationSettings["materialType"] = "PLA"; //Default value
                    IsolatedStorageSettings.ApplicationSettings.Save();
                }

                return (string)IsolatedStorageSettings.ApplicationSettings["materialType"];
            }
            set
            {
                if (materialType == value)
                    return;

                IsolatedStorageSettings.ApplicationSettings["materialType"] = value;
                IsolatedStorageSettings.ApplicationSettings.Save();

                if (materialTypeChangedEvent != null)
                    materialTypeChangedEvent(value);
            }
        }

        public static settingChangedEvent costPerKgChangedEvent;
        /// <summary>
        /// This value determines the cost of one kg of filament
        /// </summary>
        public static float costPerKg
        {
            get
            {
                if (!IsolatedStorageSettings.ApplicationSettings.Contains("costPerKg"))
                {
                    IsolatedStorageSettings.ApplicationSettings["costPerKg"] = 300.0f; //Default value
                    IsolatedStorageSettings.ApplicationSettings.Save();
                }

                return (float)IsolatedStorageSettings.ApplicationSettings["costPerKg"];
            }
            set
            {
                if (costPerKg == value)
                    return;

                IsolatedStorageSettings.ApplicationSettings["costPerKg"] = value;
                IsolatedStorageSettings.ApplicationSettings.Save();

                if (costPerKgChangedEvent != null)
                    costPerKgChangedEvent(value);
            }
        }

        public static settingChangedEvent costPerMinuteChangedEvent;
        /// <summary>
        /// This value determines the cost of one minute of printing
        /// </summary>
        public static float costPerMinute
        {
            get
            {
                if (!IsolatedStorageSettings.ApplicationSettings.Contains("costPerMinute"))
                {
                    IsolatedStorageSettings.ApplicationSettings["costPerMinute"] = 10.0f; //Default value
                    IsolatedStorageSettings.ApplicationSettings.Save();
                }

                return (float)IsolatedStorageSettings.ApplicationSettings["costPerMinute"];
            }
            set
            {
                if (costPerMinute == value)
                    return;

                IsolatedStorageSettings.ApplicationSettings["costPerMinute"] = value;
                IsolatedStorageSettings.ApplicationSettings.Save();

                if (costPerMinuteChangedEvent != null)
                    costPerMinuteChangedEvent(value);
            }
        }

        public static settingChangedEvent skirtSpeedChangedEvent;
        /// <summary>
        /// This value determines the speed at which to skirt
        /// </summary>
        public static float skirtSpeed
        {
            get
            {
                if (!IsolatedStorageSettings.ApplicationSettings.Contains("skirtSpeed"))
                {
                    IsolatedStorageSettings.ApplicationSettings["skirtSpeed"] = 10.0f; //Default value
                    IsolatedStorageSettings.ApplicationSettings.Save();
                }

                return (float)IsolatedStorageSettings.ApplicationSettings["skirtSpeed"];
            }
            set
            {
                if (skirtSpeed == value)
                    return;

                IsolatedStorageSettings.ApplicationSettings["skirtSpeed"] = value;
                IsolatedStorageSettings.ApplicationSettings.Save();

                if (skirtSpeedChangedEvent != null)
                    skirtSpeedChangedEvent(value);
            }
        }

        public static settingChangedEvent skirtCountChangedEvent;
        /// <summary>
        /// This value determines the amount of bottom of skirts to create
        /// </summary>
        public static int skirtCount
        {
            get
            {
                if (!IsolatedStorageSettings.ApplicationSettings.Contains("skirtCount"))
                {
                    IsolatedStorageSettings.ApplicationSettings["skirtCount"] = 4; //Default value
                    IsolatedStorageSettings.ApplicationSettings.Save();
                }

                return (int)IsolatedStorageSettings.ApplicationSettings["skirtCount"];
            }
            set
            {
                if (skirtCount == value)
                    return;

                IsolatedStorageSettings.ApplicationSettings["skirtCount"] = value;
                IsolatedStorageSettings.ApplicationSettings.Save();

                if (skirtCountChangedEvent != null)
                    skirtCountChangedEvent(value);
            }
        }

        public static settingChangedEvent skirtDistanceChangedEvent;
        /// <summary>
        /// This value determines the distance that the skirt should be offset from the object
        /// </summary>
        public static float skirtDistance
        {
            get
            {
                if (!IsolatedStorageSettings.ApplicationSettings.Contains("skirtDistance"))
                {
                    IsolatedStorageSettings.ApplicationSettings["skirtDistance"] = 3.0f; //Default value
                    IsolatedStorageSettings.ApplicationSettings.Save();
                }

                return (float)IsolatedStorageSettings.ApplicationSettings["skirtDistance"];
            }
            set
            {
                if (skirtDistance == value)
                    return;

                IsolatedStorageSettings.ApplicationSettings["skirtDistance"] = value;
                IsolatedStorageSettings.ApplicationSettings.Save();

                if (skirtDistanceChangedEvent != null)
                    skirtDistanceChangedEvent(value);
            }
        }

        public static settingChangedEvent shouldSkirtChangedEvent;
        /// <summary>
        /// This value determines if a skirt should be created
        /// </summary>
        public static bool shouldSkirt
        {
            get
            {
                if (!IsolatedStorageSettings.ApplicationSettings.Contains("shouldSkirt"))
                {
                    IsolatedStorageSettings.ApplicationSettings["shouldSkirt"] = true; //Default value
                    IsolatedStorageSettings.ApplicationSettings.Save();
                }

                return (bool)IsolatedStorageSettings.ApplicationSettings["shouldSkirt"];
            }
            set
            {
                if (shouldSkirt == value)
                    return;

                IsolatedStorageSettings.ApplicationSettings["shouldSkirt"] = value;
                IsolatedStorageSettings.ApplicationSettings.Save();

                if (shouldSkirtChangedEvent != null)
                    shouldSkirtChangedEvent(value);
            }
        }

        public static settingChangedEvent shellThicknessChangedEvent;
        /// <summary>
        /// This value determines the amount of shells that should be created
        /// </summary>
        public static int shellThickness
        {
            get
            {
                if (!IsolatedStorageSettings.ApplicationSettings.Contains("shellThickness"))
                {
                    IsolatedStorageSettings.ApplicationSettings["shellThickness"] = 2; //Default value
                    IsolatedStorageSettings.ApplicationSettings.Save();
                }

                return (int)IsolatedStorageSettings.ApplicationSettings["shellThickness"];
            }
            set
            {
                if (shellThickness == value)
                    return;

                IsolatedStorageSettings.ApplicationSettings["shellThickness"] = value;
                IsolatedStorageSettings.ApplicationSettings.Save();

                if (shellThicknessChangedEvent != null)
                    shellThicknessChangedEvent(value);
            }
        }

        public static settingChangedEvent shouldSupportMaterialChangedEvent;
        /// <summary>
        /// This value determines if support material should be created
        /// </summary>
        public static bool shouldSupportMaterial
        {
            get
            {
                if (!IsolatedStorageSettings.ApplicationSettings.Contains("shouldSupportMaterial"))
                {
                    IsolatedStorageSettings.ApplicationSettings["shouldSupportMaterial"] = false; //Default value
                    IsolatedStorageSettings.ApplicationSettings.Save();
                }

                return (bool)IsolatedStorageSettings.ApplicationSettings["shouldSupportMaterial"];
            }
            set
            {
                if (shouldSupportMaterial == value)
                    return;

                IsolatedStorageSettings.ApplicationSettings["shouldSupportMaterial"] = value;
                IsolatedStorageSettings.ApplicationSettings.Save();

                if (shouldSupportMaterialChangedEvent != null)
                    shouldSupportMaterialChangedEvent(value);
            }
        }

        public static settingChangedEvent supportMaterialDensityChangedEvent;
        /// <summary>
        /// This value determines the density of the support material
        /// </summary>
        public static float supportMaterialDensity
        {
            get
            {
                if (!IsolatedStorageSettings.ApplicationSettings.Contains("supportMaterialDensity"))
                {
                    IsolatedStorageSettings.ApplicationSettings["supportMaterialDensity"] = 10.0f; //Default value
                    IsolatedStorageSettings.ApplicationSettings.Save();
                }

                return (float)IsolatedStorageSettings.ApplicationSettings["supportMaterialDensity"];
            }
            set
            {
                if (skirtDistance == value)
                    return;

                IsolatedStorageSettings.ApplicationSettings["supportMaterialDensity"] = value;
                IsolatedStorageSettings.ApplicationSettings.Save();

                if (supportMaterialDensityChangedEvent != null)
                    supportMaterialDensityChangedEvent(value);
            }
        }

        public static settingChangedEvent supportSpeedChangedEvent;
        /// <summary>
        /// This value determines the speed at which support material should be printed
        /// </summary>
        public static float supportSpeed
        {
            get
            {
                if (!IsolatedStorageSettings.ApplicationSettings.Contains("supportSpeed"))
                {
                    IsolatedStorageSettings.ApplicationSettings["supportSpeed"] = 20.0f; //Default value
                    IsolatedStorageSettings.ApplicationSettings.Save();
                }

                return (float)IsolatedStorageSettings.ApplicationSettings["supportSpeed"];
            }
            set
            {
                if (supportSpeed == value)
                    return;

                IsolatedStorageSettings.ApplicationSettings["supportSpeed"] = value;
                IsolatedStorageSettings.ApplicationSettings.Save();

                if (supportSpeedChangedEvent != null)
                    supportSpeedChangedEvent(value);
            }
        }

        public static settingChangedEvent retractionSpeedChangedEvent;
        /// <summary>
        /// This value determines the speed at which filament should be retracted
        /// </summary>
        public static float retractionSpeed
        {
            get
            {
                if (!IsolatedStorageSettings.ApplicationSettings.Contains("retractionSpeed"))
                {
                    IsolatedStorageSettings.ApplicationSettings["retractionSpeed"] = 45.0f; //Default value
                    IsolatedStorageSettings.ApplicationSettings.Save();
                }

                return (float)IsolatedStorageSettings.ApplicationSettings["retractionSpeed"];
            }
            set
            {
                if (retractionSpeed == value)
                    return;

                IsolatedStorageSettings.ApplicationSettings["retractionSpeed"] = value;
                IsolatedStorageSettings.ApplicationSettings.Save();

                if (retractionSpeedChangedEvent != null)
                    retractionSpeedChangedEvent(value);
            }
        }

        public static settingChangedEvent retractionDistanceChangedEvent;
        /// <summary>
        /// This value determines the distance that filament should be retracted
        /// </summary>
        public static float retractionDistance
        {
            get
            {
                if (!IsolatedStorageSettings.ApplicationSettings.Contains("retractionDistance"))
                {
                    IsolatedStorageSettings.ApplicationSettings["retractionDistance"] = 45.0f; //Default value
                    IsolatedStorageSettings.ApplicationSettings.Save();
                }

                return (float)IsolatedStorageSettings.ApplicationSettings["retractionDistance"];
            }
            set
            {
                if (retractionDistance == value)
                    return;

                IsolatedStorageSettings.ApplicationSettings["retractionDistance"] = value;
                IsolatedStorageSettings.ApplicationSettings.Save();

                if (retractionDistanceChangedEvent != null)
                    retractionDistanceChangedEvent(value);
            }
        }

        public static settingChangedEvent shouldRaftChangedEvent;
        /// <summary>
        /// This value determines if a raft should be created
        /// </summary>
        public static bool shouldRaft
        {
            get
            {
                if (!IsolatedStorageSettings.ApplicationSettings.Contains("shouldRaft"))
                {
                    IsolatedStorageSettings.ApplicationSettings["shouldRaft"] = false; //Default value
                    IsolatedStorageSettings.ApplicationSettings.Save();
                }

                return (bool)IsolatedStorageSettings.ApplicationSettings["shouldRaft"];
            }
            set
            {
                if (shouldRaft == value)
                    return;

                IsolatedStorageSettings.ApplicationSettings["shouldRaft"] = value;
                IsolatedStorageSettings.ApplicationSettings.Save();

                if (shouldRaftChangedEvent != null)
                    shouldRaftChangedEvent(value);
            }
        }

        public static settingChangedEvent raftDensityChangedEvent;
        /// <summary>
        /// This value determines the fill density of the raft
        /// </summary>
        public static float raftDensity
        {
            get
            {
                if (!IsolatedStorageSettings.ApplicationSettings.Contains("raftDensity"))
                {
                    IsolatedStorageSettings.ApplicationSettings["raftDensity"] = 50.0f; //Default value
                    IsolatedStorageSettings.ApplicationSettings.Save();
                }

                return (float)IsolatedStorageSettings.ApplicationSettings["raftDensity"];
            }
            set
            {
                if (raftDensity == value)
                    return;

                IsolatedStorageSettings.ApplicationSettings["raftDensity"] = value;
                IsolatedStorageSettings.ApplicationSettings.Save();

                if (raftDensityChangedEvent != null)
                    raftDensityChangedEvent(value);
            }
        }

        public static settingChangedEvent raftDistanceChangedEvent;
        /// <summary>
        /// This value determines the fill distance that the raft should be offset from the object
        /// </summary>
        public static float raftDistance
        {
            get
            {
                if (!IsolatedStorageSettings.ApplicationSettings.Contains("raftDistance"))
                {
                    IsolatedStorageSettings.ApplicationSettings["raftDistance"] = 5.0f; //Default value
                    IsolatedStorageSettings.ApplicationSettings.Save();
                }

                return (float)IsolatedStorageSettings.ApplicationSettings["raftDistance"];
            }
            set
            {
                if (raftDistance == value)
                    return;

                IsolatedStorageSettings.ApplicationSettings["raftDistance"] = value;
                IsolatedStorageSettings.ApplicationSettings.Save();

                if (raftDistanceChangedEvent != null)
                    raftDistanceChangedEvent(value);
            }
        }

        public static settingChangedEvent raftCountChangedEvent;
        /// <summary>
        /// This value determines the amount of raft layers to create
        /// </summary>
        public static int raftCount
        {
            get
            {
                if (!IsolatedStorageSettings.ApplicationSettings.Contains("raftCount"))
                {
                    IsolatedStorageSettings.ApplicationSettings["raftCount"] = 2; //Default value
                    IsolatedStorageSettings.ApplicationSettings.Save();
                }

                return (int)IsolatedStorageSettings.ApplicationSettings["raftCount"];
            }
            set
            {
                if (raftCount == value)
                    return;

                IsolatedStorageSettings.ApplicationSettings["raftCount"] = value;
                IsolatedStorageSettings.ApplicationSettings.Save();

                if (raftCountChangedEvent != null)
                    raftCountChangedEvent(value);
            }
        }

        public static settingChangedEvent raftSpeedChangedEvent;
        /// <summary>
        /// This value determines the speed at which the raft should be printed
        /// </summary>
        public static float raftSpeed
        {
            get
            {
                if (!IsolatedStorageSettings.ApplicationSettings.Contains("raftSpeed"))
                {
                    IsolatedStorageSettings.ApplicationSettings["raftSpeed"] = 10.0f; //Default value
                    IsolatedStorageSettings.ApplicationSettings.Save();
                }

                return (float)IsolatedStorageSettings.ApplicationSettings["raftSpeed"];
            }
            set
            {
                if (raftSpeed == value)
                    return;

                IsolatedStorageSettings.ApplicationSettings["raftSpeed"] = value;
                IsolatedStorageSettings.ApplicationSettings.Save();

                if (raftSpeedChangedEvent != null)
                    raftSpeedChangedEvent(value);
            }
        }

        public static settingChangedEvent infillCombinationCountChangedEvent;
        /// <summary>
        /// This value determines the amount of layers' infill that should be combined
        /// </summary>
        public static int infillCombinationCount
        {
            get
            {
                if (!IsolatedStorageSettings.ApplicationSettings.Contains("infillCombinationCount"))
                {
                    IsolatedStorageSettings.ApplicationSettings["infillCombinationCount"] = 1; //Default value
                    IsolatedStorageSettings.ApplicationSettings.Save();
                }

                return (int)IsolatedStorageSettings.ApplicationSettings["infillCombinationCount"];
            }
            set
            {
                if (infillCombinationCount == value)
                    return;

                IsolatedStorageSettings.ApplicationSettings["infillCombinationCount"] = value;
                IsolatedStorageSettings.ApplicationSettings.Save();

                if (infillCombinationCountChangedEvent != null)
                    infillCombinationCountChangedEvent(value);
            }
        }

        public static settingChangedEvent specialFlowratesChangedEvent;
        /// <summary>
        /// This value determines the special flow rates
        /// </summary>
        public static string specialFlowrates
        {
            get
            {
                if (!IsolatedStorageSettings.ApplicationSettings.Contains("specialFlowrates"))
                {
                    IsolatedStorageSettings.ApplicationSettings["specialFlowrates"] = ""; //Default value
                    IsolatedStorageSettings.ApplicationSettings.Save();
                }

                return (string)IsolatedStorageSettings.ApplicationSettings["specialFlowrates"];
            }
            set
            {
                if (specialFlowrates == value)
                    return;

                IsolatedStorageSettings.ApplicationSettings["specialFlowrates"] = value;
                IsolatedStorageSettings.ApplicationSettings.Save();

                if (specialFlowratesChangedEvent != null)
                    specialFlowratesChangedEvent(value);
            }
        }

        public static settingChangedEvent specialMoveSpeedsChangedEvent;
        /// <summary>
        /// This value determines the special move speeds
        /// </summary>
        public static string specialMoveSpeeds
        {
            get
            {
                if (!IsolatedStorageSettings.ApplicationSettings.Contains("specialMoveSpeeds"))
                {
                    IsolatedStorageSettings.ApplicationSettings["specialMoveSpeeds"] = ""; //Default value
                    IsolatedStorageSettings.ApplicationSettings.Save();
                }

                return (string)IsolatedStorageSettings.ApplicationSettings["specialMoveSpeeds"];
            }
            set
            {
                if (specialMoveSpeeds == value)
                    return;

                IsolatedStorageSettings.ApplicationSettings["specialMoveSpeeds"] = value;
                IsolatedStorageSettings.ApplicationSettings.Save();

                if (specialMoveSpeedsChangedEvent != null)
                    specialMoveSpeedsChangedEvent(value);
            }
        }

        public static settingChangedEvent specialBridgeingSpeedsChangedEvent;
        /// <summary>
        /// This value determines the special bridgeing speeds
        /// </summary>
        public static string specialBridgeingSpeeds
        {
            get
            {
                if (!IsolatedStorageSettings.ApplicationSettings.Contains("specialBridgeingSpeeds"))
                {
                    IsolatedStorageSettings.ApplicationSettings["specialBridgeingSpeeds"] = ""; //Default value
                    IsolatedStorageSettings.ApplicationSettings.Save();
                }

                return (string)IsolatedStorageSettings.ApplicationSettings["specialBridgeingSpeeds"];
            }
            set
            {
                if (specialBridgeingSpeeds == value)
                    return;

                IsolatedStorageSettings.ApplicationSettings["specialBridgeingSpeeds"] = value;
                IsolatedStorageSettings.ApplicationSettings.Save();

                if (specialBridgeingSpeedsChangedEvent != null)
                    specialBridgeingSpeedsChangedEvent(value);
            }
        }

        public static settingChangedEvent specialSupportDensitiesChangedEvent;
        /// <summary>
        /// This value determines the special support densities
        /// </summary>
        public static string specialSupportDensities
        {
            get
            {
                if (!IsolatedStorageSettings.ApplicationSettings.Contains("specialSupportDensities"))
                {
                    IsolatedStorageSettings.ApplicationSettings["specialSupportDensities"] = ""; //Default value
                    IsolatedStorageSettings.ApplicationSettings.Save();
                }

                return (string)IsolatedStorageSettings.ApplicationSettings["specialSupportDensities"];
            }
            set
            {
                if (specialSupportDensities == value)
                    return;

                IsolatedStorageSettings.ApplicationSettings["specialSupportDensities"] = value;
                IsolatedStorageSettings.ApplicationSettings.Save();

                if (specialSupportDensitiesChangedEvent != null)
                    specialSupportDensitiesChangedEvent(value);
            }
        }

        public static settingChangedEvent specialSupportSpeedsChangedEvent;
        /// <summary>
        /// This value determines the special support speeds
        /// </summary>
        public static string specialSupportSpeeds
        {
            get
            {
                if (!IsolatedStorageSettings.ApplicationSettings.Contains("specialSupportSpeeds"))
                {
                    IsolatedStorageSettings.ApplicationSettings["specialSupportSpeeds"] = ""; //Default value
                    IsolatedStorageSettings.ApplicationSettings.Save();
                }

                return (string)IsolatedStorageSettings.ApplicationSettings["specialSupportSpeeds"];
            }
            set
            {
                if (specialSupportSpeeds == value)
                    return;

                IsolatedStorageSettings.ApplicationSettings["specialSupportSpeeds"] = value;
                IsolatedStorageSettings.ApplicationSettings.Save();

                if (specialSupportSpeedsChangedEvent != null)
                    specialSupportSpeedsChangedEvent(value);
            }
        }

        public static settingChangedEvent minimumLayerTimeChangedEvent;
        /// <summary>
        /// This value determines the minimum layer time
        /// </summary>
        public static float minimumLayerTime
        {
            get
            {
                if (!IsolatedStorageSettings.ApplicationSettings.Contains("minimumLayerTime"))
                {
                    IsolatedStorageSettings.ApplicationSettings["minimumLayerTime"] = 60.0f; //Default value
                    IsolatedStorageSettings.ApplicationSettings.Save();
                }

                return (float)IsolatedStorageSettings.ApplicationSettings["minimumLayerTime"];
            }
            set
            {
                if (minimumLayerTime == value)
                    return;

                IsolatedStorageSettings.ApplicationSettings["minimumLayerTime"] = value;
                IsolatedStorageSettings.ApplicationSettings.Save();

                if (minimumLayerTimeChangedEvent != null)
                    minimumLayerTimeChangedEvent(value);
            }
        }

        public static settingChangedEvent minimumLayerSpeedChangedEvent;
        /// <summary>
        /// This value determines the minimum layer speed
        /// </summary>
        public static float minimumLayerSpeed
        {
            get
            {
                if (!IsolatedStorageSettings.ApplicationSettings.Contains("minimumLayerSpeed"))
                {
                    IsolatedStorageSettings.ApplicationSettings["minimumLayerSpeed"] = 1.0f; //Default value
                    IsolatedStorageSettings.ApplicationSettings.Save();
                }

                return (float)IsolatedStorageSettings.ApplicationSettings["minimumLayerSpeed"];
            }
            set
            {
                if (minimumLayerSpeed == value)
                    return;

                IsolatedStorageSettings.ApplicationSettings["minimumLayerSpeed"] = value;
                IsolatedStorageSettings.ApplicationSettings.Save();

                if (minimumLayerSpeedChangedEvent != null)
                    minimumLayerSpeedChangedEvent(value);
            }
        }
        #endregion

        #region bed dimensions
        public static settingChangedEvent bedWithChangedEvent;
        /// <summary>
        /// This value determines the width of the printbed
        /// </summary>
        public static int bedWidth
        {
            get
            {
                if (!IsolatedStorageSettings.ApplicationSettings.Contains("bedWidth"))
                {
                    IsolatedStorageSettings.ApplicationSettings["bedWidth"] = 300; //Default value
                    IsolatedStorageSettings.ApplicationSettings.Save();
                }

                return (int)IsolatedStorageSettings.ApplicationSettings["bedWidth"];
            }
            set
            {
                if (bedWidth == value)
                    return;

                IsolatedStorageSettings.ApplicationSettings["bedWidth"] = value;
                IsolatedStorageSettings.ApplicationSettings.Save();

                if (bedWithChangedEvent != null)
                    bedWithChangedEvent(value);
            }
        }

        public static settingChangedEvent bedLengthChangedEvent;
        /// <summary>
        /// This value determines the length of the printbed
        /// </summary>
        public static int bedLength
        {
            get
            {
                if (!IsolatedStorageSettings.ApplicationSettings.Contains("bedLength"))
                {
                    IsolatedStorageSettings.ApplicationSettings["bedLength"] = 300; //Default value
                    IsolatedStorageSettings.ApplicationSettings.Save();
                }

                return (int)IsolatedStorageSettings.ApplicationSettings["bedLength"];
            }
            set
            {
                if (bedLength == value)
                    return;

                IsolatedStorageSettings.ApplicationSettings["bedLength"] = value;
                IsolatedStorageSettings.ApplicationSettings.Save();

                if (bedLengthChangedEvent != null)
                    bedLengthChangedEvent(value);
            }
        }

        public static settingChangedEvent bedHeigthChangedEvent;
        /// <summary>
        /// This value determines the heigth of the printbed
        /// </summary>
        public static int bedHeigth
        {
            get
            {
                if (!IsolatedStorageSettings.ApplicationSettings.Contains("bedHeigth"))
                {
                    IsolatedStorageSettings.ApplicationSettings["bedHeigth"] = 300; //Default value
                    IsolatedStorageSettings.ApplicationSettings.Save();
                }

                return (int)IsolatedStorageSettings.ApplicationSettings["bedHeigth"];
            }
            set
            {
                if (bedHeigth == value)
                    return;

                IsolatedStorageSettings.ApplicationSettings["bedHeigth"] = value;
                IsolatedStorageSettings.ApplicationSettings.Save();

                if (bedHeigthChangedEvent != null)
                    bedHeigthChangedEvent(value);
            }
        }
        #endregion

        #region other
        public static settingChangedEvent repetierProtocolChangedEvent;
        /// <summary>
        /// This value determines the width of the printbed
        /// </summary>
        public static bool repetierProtocol
        {
            get
            {
                if (!IsolatedStorageSettings.ApplicationSettings.Contains("repetierProtocol"))
                {
                    IsolatedStorageSettings.ApplicationSettings["repetierProtocol"] = false; //Default value
                    IsolatedStorageSettings.ApplicationSettings.Save();
                }

                return (bool)IsolatedStorageSettings.ApplicationSettings["repetierProtocol"];
            }
            set
            {
                if (repetierProtocol == value)
                    return;

                IsolatedStorageSettings.ApplicationSettings["repetierProtocol"] = value;
                IsolatedStorageSettings.ApplicationSettings.Save();

                if (repetierProtocolChangedEvent != null)
                    repetierProtocolChangedEvent(value);
            }
        }
        #endregion
    }
}
