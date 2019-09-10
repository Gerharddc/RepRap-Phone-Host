﻿#pragma checksum "C:\Users\Gerhard\SkyDrive\Bananna3D\Host Software\RepRap Phone Host\RepRap Phone Host\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "3E2348AA8F5E7A3B7741EBEF610302F8"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace RepRap_Phone_Host {
    
    
    public partial class MainPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.MediaElement XnaMediaElement;
        
        internal Microsoft.Phone.Controls.Pivot MainPivot;
        
        internal Microsoft.Phone.Controls.PivotItem StlPivot;
        
        internal System.Windows.Controls.ScrollViewer stlpivot_Scroller;
        
        internal Microsoft.Phone.Controls.ListPicker stl_Filelist;
        
        internal System.Windows.Controls.DrawingSurface StlSurface;
        
        internal System.Windows.Controls.Button zoomButtonStl;
        
        internal System.Windows.Controls.Button unZoomButtonStl;
        
        internal System.Windows.Controls.Button moveUpButtonStl;
        
        internal System.Windows.Controls.Button moveLeftButtonStl;
        
        internal System.Windows.Controls.Button moveRightButtonStl;
        
        internal System.Windows.Controls.Button moveDownButtonStl;
        
        internal System.Windows.Controls.Button rotateUpButtonStl;
        
        internal System.Windows.Controls.Button rotateLeftButtonStl;
        
        internal System.Windows.Controls.Button rotateRightButtonStl;
        
        internal System.Windows.Controls.Button rotateDownButtonStl;
        
        internal System.Windows.Controls.TextBlock xRotation_TextBlock;
        
        internal System.Windows.Controls.TextBox xRotation_TextBox;
        
        internal System.Windows.Controls.Slider xRotation_Slider;
        
        internal System.Windows.Controls.TextBlock yRotation_TextBlock;
        
        internal System.Windows.Controls.TextBox yRotation_TextBox;
        
        internal System.Windows.Controls.Slider yRotation_Slider;
        
        internal System.Windows.Controls.TextBlock zRotation_TextBlock;
        
        internal System.Windows.Controls.TextBox zRotation_TextBox;
        
        internal System.Windows.Controls.Slider zRotation_Slider;
        
        internal System.Windows.Controls.TextBox scale_Textbox;
        
        internal System.Windows.Controls.TextBox xOffset_Textbox;
        
        internal System.Windows.Controls.TextBox yOffset_Textbox;
        
        internal System.Windows.Controls.Button saveButtonStl;
        
        internal Microsoft.Phone.Controls.PivotItem SlicerPivot;
        
        internal Microsoft.Phone.Controls.ListPicker slicer_Filelist;
        
        internal System.Windows.Controls.Button Slice_Button;
        
        internal System.Windows.Controls.Button CancelButton;
        
        internal System.Windows.Controls.StackPanel outputStackPanel;
        
        internal Microsoft.Phone.Controls.PivotItem GCodePivot;
        
        internal Microsoft.Phone.Controls.ListPicker gcode_Filelist;
        
        internal System.Windows.Controls.DrawingSurface GCodeSurface;
        
        internal System.Windows.Controls.Button zoomButtonGCode;
        
        internal System.Windows.Controls.Button unZoomButtonGCode;
        
        internal System.Windows.Controls.Button moveUpButtonGCode;
        
        internal System.Windows.Controls.Button moveLeftButtonGCode;
        
        internal System.Windows.Controls.Button moveRightButtonGCode;
        
        internal System.Windows.Controls.Button moveDownButtonGCode;
        
        internal System.Windows.Controls.Button rotateUpButtonGCode;
        
        internal System.Windows.Controls.Button rotateLeftButtonGCode;
        
        internal System.Windows.Controls.Button rotateRightButtonGCode;
        
        internal System.Windows.Controls.Button rotateDownButtonGCode;
        
        internal System.Windows.Controls.TextBlock bottomLayer_TextBlock;
        
        internal System.Windows.Controls.Slider min_Slider;
        
        internal System.Windows.Controls.TextBlock topLayer_TextBlock;
        
        internal System.Windows.Controls.Slider max_Slider;
        
        internal Microsoft.Phone.Controls.PivotItem GCode_TextPivot;
        
        internal Microsoft.Phone.Controls.ListPicker gcodetext_Filelist;
        
        internal System.Windows.Controls.ScrollViewer gcodeScroller;
        
        internal System.Windows.Controls.StackPanel gcodePanel;
        
        internal Microsoft.Phone.Controls.PivotItem ControlPivot;
        
        internal Microsoft.Phone.Controls.ListPicker gcodeprint_Filelist;
        
        internal System.Windows.Controls.TextBlock eta_TextBlock;
        
        internal System.Windows.Controls.Button print_Button;
        
        internal System.Windows.Controls.TextBlock printing_TextBlock;
        
        internal System.Windows.Controls.ProgressBar printing_ProgressBar;
        
        internal System.Windows.Controls.RadioButton x_Radiobutton;
        
        internal System.Windows.Controls.RadioButton y_Radiobutton;
        
        internal System.Windows.Controls.RadioButton z_Radiobutton;
        
        internal System.Windows.Controls.RadioButton e_Radiobutton;
        
        internal System.Windows.Controls.TextBlock distance_Textblock;
        
        internal System.Windows.Controls.TextBox distance_Textbox;
        
        internal System.Windows.Controls.TextBlock speed_Textblock;
        
        internal System.Windows.Controls.TextBox speed_Textbox;
        
        internal System.Windows.Controls.Button up_Button;
        
        internal System.Windows.Controls.Button down_Button;
        
        internal System.Windows.Controls.TextBlock home_Textblock;
        
        internal System.Windows.Controls.Button xhome_Button;
        
        internal System.Windows.Controls.Button yhome_Button;
        
        internal System.Windows.Controls.Button zhome_Button;
        
        internal System.Windows.Controls.Button allhome_Button;
        
        internal System.Windows.Controls.TextBlock temperatureTitle_Textblock;
        
        internal System.Windows.Controls.TextBlock temperature_Textblock;
        
        internal System.Windows.Controls.CheckBox heat_Checkbox;
        
        internal System.Windows.Controls.TextBox temperature_Textbox;
        
        internal System.Windows.Controls.TextBlock gcode_Textblock;
        
        internal System.Windows.Controls.TextBox gcode_Textbox;
        
        internal System.Windows.Controls.Button emergencystop_Button;
        
        internal Microsoft.Phone.Controls.PivotItem ConnectionPivot;
        
        internal Microsoft.Phone.Controls.ListPicker device_Listpicker;
        
        internal System.Windows.Controls.Button connect_Button;
        
        internal System.Windows.Controls.CheckBox repetier_CheckBox;
        
        internal System.Windows.Controls.StackPanel comlog_Stackpanel;
        
        internal Microsoft.Phone.Controls.PivotItem SettingsPivot;
        
        internal System.Windows.Controls.StackPanel SettingStackPanel;
        
        internal Microsoft.Phone.Controls.PivotItem FilesPivot;
        
        internal System.Windows.Controls.ScrollViewer FilesScroller;
        
        internal System.Windows.Controls.StackPanel stlFiles_StackPanel;
        
        internal System.Windows.Controls.StackPanel gcodeFiles_StackPanel;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/RepRap%20Phone%20Host;component/MainPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.XnaMediaElement = ((System.Windows.Controls.MediaElement)(this.FindName("XnaMediaElement")));
            this.MainPivot = ((Microsoft.Phone.Controls.Pivot)(this.FindName("MainPivot")));
            this.StlPivot = ((Microsoft.Phone.Controls.PivotItem)(this.FindName("StlPivot")));
            this.stlpivot_Scroller = ((System.Windows.Controls.ScrollViewer)(this.FindName("stlpivot_Scroller")));
            this.stl_Filelist = ((Microsoft.Phone.Controls.ListPicker)(this.FindName("stl_Filelist")));
            this.StlSurface = ((System.Windows.Controls.DrawingSurface)(this.FindName("StlSurface")));
            this.zoomButtonStl = ((System.Windows.Controls.Button)(this.FindName("zoomButtonStl")));
            this.unZoomButtonStl = ((System.Windows.Controls.Button)(this.FindName("unZoomButtonStl")));
            this.moveUpButtonStl = ((System.Windows.Controls.Button)(this.FindName("moveUpButtonStl")));
            this.moveLeftButtonStl = ((System.Windows.Controls.Button)(this.FindName("moveLeftButtonStl")));
            this.moveRightButtonStl = ((System.Windows.Controls.Button)(this.FindName("moveRightButtonStl")));
            this.moveDownButtonStl = ((System.Windows.Controls.Button)(this.FindName("moveDownButtonStl")));
            this.rotateUpButtonStl = ((System.Windows.Controls.Button)(this.FindName("rotateUpButtonStl")));
            this.rotateLeftButtonStl = ((System.Windows.Controls.Button)(this.FindName("rotateLeftButtonStl")));
            this.rotateRightButtonStl = ((System.Windows.Controls.Button)(this.FindName("rotateRightButtonStl")));
            this.rotateDownButtonStl = ((System.Windows.Controls.Button)(this.FindName("rotateDownButtonStl")));
            this.xRotation_TextBlock = ((System.Windows.Controls.TextBlock)(this.FindName("xRotation_TextBlock")));
            this.xRotation_TextBox = ((System.Windows.Controls.TextBox)(this.FindName("xRotation_TextBox")));
            this.xRotation_Slider = ((System.Windows.Controls.Slider)(this.FindName("xRotation_Slider")));
            this.yRotation_TextBlock = ((System.Windows.Controls.TextBlock)(this.FindName("yRotation_TextBlock")));
            this.yRotation_TextBox = ((System.Windows.Controls.TextBox)(this.FindName("yRotation_TextBox")));
            this.yRotation_Slider = ((System.Windows.Controls.Slider)(this.FindName("yRotation_Slider")));
            this.zRotation_TextBlock = ((System.Windows.Controls.TextBlock)(this.FindName("zRotation_TextBlock")));
            this.zRotation_TextBox = ((System.Windows.Controls.TextBox)(this.FindName("zRotation_TextBox")));
            this.zRotation_Slider = ((System.Windows.Controls.Slider)(this.FindName("zRotation_Slider")));
            this.scale_Textbox = ((System.Windows.Controls.TextBox)(this.FindName("scale_Textbox")));
            this.xOffset_Textbox = ((System.Windows.Controls.TextBox)(this.FindName("xOffset_Textbox")));
            this.yOffset_Textbox = ((System.Windows.Controls.TextBox)(this.FindName("yOffset_Textbox")));
            this.saveButtonStl = ((System.Windows.Controls.Button)(this.FindName("saveButtonStl")));
            this.SlicerPivot = ((Microsoft.Phone.Controls.PivotItem)(this.FindName("SlicerPivot")));
            this.slicer_Filelist = ((Microsoft.Phone.Controls.ListPicker)(this.FindName("slicer_Filelist")));
            this.Slice_Button = ((System.Windows.Controls.Button)(this.FindName("Slice_Button")));
            this.CancelButton = ((System.Windows.Controls.Button)(this.FindName("CancelButton")));
            this.outputStackPanel = ((System.Windows.Controls.StackPanel)(this.FindName("outputStackPanel")));
            this.GCodePivot = ((Microsoft.Phone.Controls.PivotItem)(this.FindName("GCodePivot")));
            this.gcode_Filelist = ((Microsoft.Phone.Controls.ListPicker)(this.FindName("gcode_Filelist")));
            this.GCodeSurface = ((System.Windows.Controls.DrawingSurface)(this.FindName("GCodeSurface")));
            this.zoomButtonGCode = ((System.Windows.Controls.Button)(this.FindName("zoomButtonGCode")));
            this.unZoomButtonGCode = ((System.Windows.Controls.Button)(this.FindName("unZoomButtonGCode")));
            this.moveUpButtonGCode = ((System.Windows.Controls.Button)(this.FindName("moveUpButtonGCode")));
            this.moveLeftButtonGCode = ((System.Windows.Controls.Button)(this.FindName("moveLeftButtonGCode")));
            this.moveRightButtonGCode = ((System.Windows.Controls.Button)(this.FindName("moveRightButtonGCode")));
            this.moveDownButtonGCode = ((System.Windows.Controls.Button)(this.FindName("moveDownButtonGCode")));
            this.rotateUpButtonGCode = ((System.Windows.Controls.Button)(this.FindName("rotateUpButtonGCode")));
            this.rotateLeftButtonGCode = ((System.Windows.Controls.Button)(this.FindName("rotateLeftButtonGCode")));
            this.rotateRightButtonGCode = ((System.Windows.Controls.Button)(this.FindName("rotateRightButtonGCode")));
            this.rotateDownButtonGCode = ((System.Windows.Controls.Button)(this.FindName("rotateDownButtonGCode")));
            this.bottomLayer_TextBlock = ((System.Windows.Controls.TextBlock)(this.FindName("bottomLayer_TextBlock")));
            this.min_Slider = ((System.Windows.Controls.Slider)(this.FindName("min_Slider")));
            this.topLayer_TextBlock = ((System.Windows.Controls.TextBlock)(this.FindName("topLayer_TextBlock")));
            this.max_Slider = ((System.Windows.Controls.Slider)(this.FindName("max_Slider")));
            this.GCode_TextPivot = ((Microsoft.Phone.Controls.PivotItem)(this.FindName("GCode_TextPivot")));
            this.gcodetext_Filelist = ((Microsoft.Phone.Controls.ListPicker)(this.FindName("gcodetext_Filelist")));
            this.gcodeScroller = ((System.Windows.Controls.ScrollViewer)(this.FindName("gcodeScroller")));
            this.gcodePanel = ((System.Windows.Controls.StackPanel)(this.FindName("gcodePanel")));
            this.ControlPivot = ((Microsoft.Phone.Controls.PivotItem)(this.FindName("ControlPivot")));
            this.gcodeprint_Filelist = ((Microsoft.Phone.Controls.ListPicker)(this.FindName("gcodeprint_Filelist")));
            this.eta_TextBlock = ((System.Windows.Controls.TextBlock)(this.FindName("eta_TextBlock")));
            this.print_Button = ((System.Windows.Controls.Button)(this.FindName("print_Button")));
            this.printing_TextBlock = ((System.Windows.Controls.TextBlock)(this.FindName("printing_TextBlock")));
            this.printing_ProgressBar = ((System.Windows.Controls.ProgressBar)(this.FindName("printing_ProgressBar")));
            this.x_Radiobutton = ((System.Windows.Controls.RadioButton)(this.FindName("x_Radiobutton")));
            this.y_Radiobutton = ((System.Windows.Controls.RadioButton)(this.FindName("y_Radiobutton")));
            this.z_Radiobutton = ((System.Windows.Controls.RadioButton)(this.FindName("z_Radiobutton")));
            this.e_Radiobutton = ((System.Windows.Controls.RadioButton)(this.FindName("e_Radiobutton")));
            this.distance_Textblock = ((System.Windows.Controls.TextBlock)(this.FindName("distance_Textblock")));
            this.distance_Textbox = ((System.Windows.Controls.TextBox)(this.FindName("distance_Textbox")));
            this.speed_Textblock = ((System.Windows.Controls.TextBlock)(this.FindName("speed_Textblock")));
            this.speed_Textbox = ((System.Windows.Controls.TextBox)(this.FindName("speed_Textbox")));
            this.up_Button = ((System.Windows.Controls.Button)(this.FindName("up_Button")));
            this.down_Button = ((System.Windows.Controls.Button)(this.FindName("down_Button")));
            this.home_Textblock = ((System.Windows.Controls.TextBlock)(this.FindName("home_Textblock")));
            this.xhome_Button = ((System.Windows.Controls.Button)(this.FindName("xhome_Button")));
            this.yhome_Button = ((System.Windows.Controls.Button)(this.FindName("yhome_Button")));
            this.zhome_Button = ((System.Windows.Controls.Button)(this.FindName("zhome_Button")));
            this.allhome_Button = ((System.Windows.Controls.Button)(this.FindName("allhome_Button")));
            this.temperatureTitle_Textblock = ((System.Windows.Controls.TextBlock)(this.FindName("temperatureTitle_Textblock")));
            this.temperature_Textblock = ((System.Windows.Controls.TextBlock)(this.FindName("temperature_Textblock")));
            this.heat_Checkbox = ((System.Windows.Controls.CheckBox)(this.FindName("heat_Checkbox")));
            this.temperature_Textbox = ((System.Windows.Controls.TextBox)(this.FindName("temperature_Textbox")));
            this.gcode_Textblock = ((System.Windows.Controls.TextBlock)(this.FindName("gcode_Textblock")));
            this.gcode_Textbox = ((System.Windows.Controls.TextBox)(this.FindName("gcode_Textbox")));
            this.emergencystop_Button = ((System.Windows.Controls.Button)(this.FindName("emergencystop_Button")));
            this.ConnectionPivot = ((Microsoft.Phone.Controls.PivotItem)(this.FindName("ConnectionPivot")));
            this.device_Listpicker = ((Microsoft.Phone.Controls.ListPicker)(this.FindName("device_Listpicker")));
            this.connect_Button = ((System.Windows.Controls.Button)(this.FindName("connect_Button")));
            this.repetier_CheckBox = ((System.Windows.Controls.CheckBox)(this.FindName("repetier_CheckBox")));
            this.comlog_Stackpanel = ((System.Windows.Controls.StackPanel)(this.FindName("comlog_Stackpanel")));
            this.SettingsPivot = ((Microsoft.Phone.Controls.PivotItem)(this.FindName("SettingsPivot")));
            this.SettingStackPanel = ((System.Windows.Controls.StackPanel)(this.FindName("SettingStackPanel")));
            this.FilesPivot = ((Microsoft.Phone.Controls.PivotItem)(this.FindName("FilesPivot")));
            this.FilesScroller = ((System.Windows.Controls.ScrollViewer)(this.FindName("FilesScroller")));
            this.stlFiles_StackPanel = ((System.Windows.Controls.StackPanel)(this.FindName("stlFiles_StackPanel")));
            this.gcodeFiles_StackPanel = ((System.Windows.Controls.StackPanel)(this.FindName("gcodeFiles_StackPanel")));
        }
    }
}

