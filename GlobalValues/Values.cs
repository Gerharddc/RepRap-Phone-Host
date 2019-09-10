using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepRap_Phone_Host.ListItems;

namespace RepRap_Phone_Host.GlobalValues
{
    public static class Values
    {
        public delegate void valueChangedEvent(object value);

        #region misc
        private static string _startFileType = "";
        public static valueChangedEvent startFileTypeChangedEvent;
        /// <summary>
        /// This value indicates the type of startup file
        /// </summary>
        public static string startFileType
        {
            get
            {
                return _startFileType;
            }
            set
            {
                if (_startFileType == value)
                    return;

                _startFileType = value;

                if (startFileTypeChangedEvent != null)
                    startFileTypeChangedEvent(value);
            }
        }   
        #endregion

        #region printer state
        private static double _progress_SecondsLeft = 0;
        public static valueChangedEvent progress_SecondsLeftChangedEvent;
        /// <summary>
        /// This value indicates the amount of seconds left in the current print job
        /// </summary>
        public static double progress_SecondsLeft
        {
            get
            {
                return _progress_SecondsLeft;
            }
            set
            {
                if (_progress_SecondsLeft == value)
                    return;

                _progress_SecondsLeft = value;

                if (progress_SecondsLeftChangedEvent != null)
                    progress_SecondsLeftChangedEvent(value);
            }
        }

        private static int _progress_PercentageDone = 0;
        public static valueChangedEvent progress_PercentageDoneChangedEvent;
        /// <summary>
        /// This value indicates the percentage of the current job that has been completed
        /// </summary>
        public static int progress_PercentageDone
        {
            get
            {
                return _progress_PercentageDone;
            }
            set
            {
                if (_progress_PercentageDone == value)
                    return;

                _progress_PercentageDone = value;

                if (progress_PercentageDoneChangedEvent != null)
                    progress_PercentageDoneChangedEvent(value);
            }
        }

        private static bool _isHeating = false;
        public static valueChangedEvent isHeatingChangedEvent;
        /// <summary>
        /// This value indicates if the printer is currently heating
        /// </summary>
        public static bool isHeating
        {
            get
            {
                return _isHeating;
            }
            set
            {
                if (_isHeating == value)
                    return;

                _isHeating = value;

                if (isHeatingChangedEvent != null)
                    isHeatingChangedEvent(value);
            }
        }

        private static float _currentPos_X = 0;
        public static valueChangedEvent currentPos_XChangedEvent;
        /// <summary>
        /// This value conatins the current x position of the printhead
        /// </summary>
        public static float currentPos_X
        {
            get
            {
                return _currentPos_X;
            }
            set
            {
                if (_currentPos_X == value)
                    return;

                _currentPos_X = value;

                if (currentPos_XChangedEvent != null)
                    currentPos_XChangedEvent(value);
            }
        }

        private static float _currentPos_Y = 0;
        public static valueChangedEvent currentPos_YChangedEvent;
        /// <summary>
        /// This value conatins the current y position of the printhead
        /// </summary>
        public static float currentPos_Y
        {
            get
            {
                return _currentPos_Y;
            }
            set
            {
                if (_currentPos_Y == value)
                    return;

                _currentPos_Y = value;

                if (currentPos_YChangedEvent != null)
                    currentPos_YChangedEvent(value);
            }
        }

        private static float _currentPos_Z = 0;
        public static valueChangedEvent currentPos_ZChangedEvent;
        /// <summary>
        /// This value conatins the current z position of the printhead
        /// </summary>
        public static float currentPos_Z
        {
            get
            {
                return _currentPos_Z;
            }
            set
            {
                if (_currentPos_Z == value)
                    return;

                _currentPos_Z = value;

                if (currentPos_ZChangedEvent != null)
                    currentPos_ZChangedEvent(value);
            }
        }
        #endregion

        #region GCode values
        private static string _startGCodeFile = "";
        public static valueChangedEvent startGCodeFileChangedEvent;
        /// <summary>
        /// This value indicates the name of the GCode file to load on startup
        /// </summary>
        public static string startGCodeFile
        {
            get
            {
                return _startGCodeFile;
            }
            set
            {
                if (_startGCodeFile == value)
                    return;

                _startGCodeFile = value;

                if (startGCodeFileChangedEvent != null)
                    startGCodeFileChangedEvent(value);
            }
        }

        private static string _currentGCodeFile = "";
        public static valueChangedEvent currentGCodeFileChangedEvent;
        /// <summary>
        /// This value indicates the name of the currently loaded GCode file
        /// </summary>
        public static string currentGCodeFile
        {
            get
            {
                return _currentGCodeFile;
            }
            set
            {
                if (_currentGCodeFile == value)
                    return;

                _currentGCodeFile = value;

                if (currentGCodeFileChangedEvent != null)
                    currentGCodeFileChangedEvent(value);
            }
        }

        private static bool _GCode_IsBusy = false;
        public static valueChangedEvent GCode_IsBusyChangedEvent;
        /// <summary>
        /// This value determines if the current GCode file is busy or not
        /// </summary>
        public static bool GCode_IsBusy
        {
            get
            {
                return _GCode_IsBusy;
            }
            set
            {
                if (_GCode_IsBusy == value)
                    return;

                _GCode_IsBusy = value;

                if (GCode_IsBusyChangedEvent != null)
                    GCode_IsBusyChangedEvent(value);
            }
        }

        private static List<FileItems> _GCode_Items = new List<FileItems>();
        public static valueChangedEvent GCode_ItemsChangedEvent;
        /// <summary>
        /// This value contains the list of gcode files used to fill a listpicker
        /// </summary>
        public static List<FileItems> GCode_Items
        {
            get
            {
                return _GCode_Items;
            }
            set
            {
                if (_GCode_Items == value)
                    return;

                _GCode_Items = value;

                if (GCode_ItemsChangedEvent != null)
                {
                    GCode_ItemsChangedEvent(value);
                    _GCode_ListIndex = 0;
                }
            }
        }

        private static int _GCode_ListIndex = 0;
        public static valueChangedEvent GCode_ListIndexChangedEvent;
        /// <summary>
        /// This value contains the index of the current gcode list item
        /// </summary>
        public static int GCode_ListIndex
        {
            get
            {
                return _GCode_ListIndex;
            }
            set
            {
                if (_GCode_ListIndex == value)
                    return;

                _GCode_ListIndex = value;

                if (GCode_ListIndexChangedEvent != null)
                    GCode_ListIndexChangedEvent(value);
            }
        }
        #endregion

        #region stl values
        private static string _startStlFile = "";
        public static valueChangedEvent startStlFileChangedEvent;
        /// <summary>
        /// This value indicates the name of the Stl file to load on startup
        /// </summary>
        public static string startStlFile
        {
            get
            {
                return _startStlFile;
            }
            set
            {
                if (_startStlFile == value)
                    return;

                _startStlFile = value;

                if (startStlFileChangedEvent != null)
                    startStlFileChangedEvent(value);
            }
        }

        private static string _currentStlFile = "";
        public static valueChangedEvent currentStlFileChangedEvent;
        /// <summary>
        /// This value indicates the name of the currently loaded Stl file
        /// </summary>
        public static string currentStlFile
        {
            get
            {
                return _currentStlFile;
            }
            set
            {
                if (_currentStlFile == value)
                    return;

                _currentStlFile = value;

                if (currentStlFileChangedEvent != null)
                    currentStlFileChangedEvent(value);
            }
        }

        private static bool _stl_IsBusy = false;
        public static valueChangedEvent stl_IsBusyChangedEvent;
        /// <summary>
        /// This value determines if the current stl file is busy or not
        /// </summary>
        public static bool stl_IsBusy
        {
            get
            {
                return _stl_IsBusy;
            }
            set
            {
                if (_stl_IsBusy == value)
                    return;

                _stl_IsBusy = value;

                if (stl_IsBusyChangedEvent != null)
                    stl_IsBusyChangedEvent(value);
            }
        }

        private static List<FileItems> _Stl_Items = new List<FileItems>();
        public static valueChangedEvent Stl_ItemsChangedEvent;
        /// <summary>
        /// This value contains the list of stl files used to fill a listpicker
        /// </summary>
        public static List<FileItems> Stl_Items
        {
            get
            {
                return _Stl_Items;
            }
            set
            {
                if (_Stl_Items == value)
                    return;

                _Stl_Items = value;

                if (Stl_ItemsChangedEvent != null)
                {
                    Stl_ItemsChangedEvent(value);
                    _Stl_ListIndex = 0;
                }
            }
        }

        private static int _Stl_ListIndex = 0;
        public static valueChangedEvent Stl_ListIndexChangedEvent;
        /// <summary>
        /// This value contains the index of the current stl list item
        /// </summary>
        public static int Stl_ListIndex
        {
            get
            {
                return _Stl_ListIndex;
            }
            set
            {
                if (_Stl_ListIndex == value)
                    return;

                _Stl_ListIndex = value;

                if (Stl_ListIndexChangedEvent != null)
                    Stl_ListIndexChangedEvent(value);
            }
        }
        #endregion

        #region GCode index values
        /// <summary>
        /// This dictionary stores the starting index of each layer
        /// </summary>
        public static Dictionary<int, int> layerStartIndices = new Dictionary<int, int>();

        private static int _layerCount = 0;
        public static valueChangedEvent layerCountChangedEvent;
        /// <summary>
        /// This value defines the total amount of layers in the GCode model
        /// </summary>
        public static int layerCount
        {
            get
            {
                return _layerCount;
            }
            set
            {
                if (_layerCount == value)
                    return;

                _layerCount = value;

                if (layerCountChangedEvent != null)
                    layerCountChangedEvent(value);

                //If the layer count has ChangedEvent then we should set the min and max to defaults
                minLayer = 0;
                maxLayer = value - 1;
            }
        }

        private static int _minLayer = 0;
        public static valueChangedEvent minLayerChangedEvent;
        /// <summary>
        /// This value indicates the number of the lowest layer currently being rendered
        /// </summary>
        public static int minLayer
        {
            get
            {
                return _minLayer;
            }
            set
            {
                if (_minLayer == value)
                    return;

                _minLayer = value;

                if (minLayerChangedEvent != null)
                    minLayerChangedEvent(value);

                //Move the maxLayer up if the min is larger
                if (value > _maxLayer)
                    maxLayer = value;
            }
        }

        private static int _maxLayer = 0;
        public static valueChangedEvent maxLayerChangedEvent;
        /// <summary>
        /// This value indicates the number of the highest layer currently being rendered
        /// </summary>
        public static int maxLayer
        {
            get
            {
                return _maxLayer;
            }
            set
            {
                if (_maxLayer == value)
                    return;

                _maxLayer = value;

                if (maxLayerChangedEvent != null)
                    maxLayerChangedEvent(value);

                //Move the min layer down if the max is smaller
                if (value < _minLayer)
                    minLayer = value;
            }
        }
        #endregion
    }
}
