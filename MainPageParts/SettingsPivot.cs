using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using RepRap_Phone_Host.GlobalValues;
using System.Globalization;

namespace RepRap_Phone_Host
{
    public partial class MainPage : PhoneApplicationPage
    {
        IApplicationBar settingsBar = new ApplicationBar();

        //This is the constructor for the pivot which gets called from the main constructor
        public void Construct_SettingsPivot()
        {
            buildSettingsApplicationBar();

            TextBox bedWidth_TextBox = new TextBox();
            bedWidth_TextBox.TextChanged += bedWidth_TextBox_TextChanged;
            bedWidth_TextBox.Text = Settings.bedWidth.ToString(CultureInfo.InvariantCulture);
            bedWidth_TextBox.InputScope = new InputScope() { Names = { new InputScopeName() { NameValue = InputScopeNameValue.Number } } };
            SettingStackPanel.Children.Add(new TextBlock() { Text = "bedWidth" });
            SettingStackPanel.Children.Add(bedWidth_TextBox);

            TextBox bedLength_TextBox = new TextBox();
            bedLength_TextBox.TextChanged += bedLength_TextBox_TextChanged;
            bedLength_TextBox.Text = Settings.bedLength.ToString(CultureInfo.InvariantCulture);
            bedLength_TextBox.InputScope = new InputScope() { Names = { new InputScopeName() { NameValue = InputScopeNameValue.Number } } };
            SettingStackPanel.Children.Add(new TextBlock() { Text = "bedLength" });
            SettingStackPanel.Children.Add(bedLength_TextBox);

            TextBox bedHeigth_TextBox = new TextBox();
            bedHeigth_TextBox.TextChanged += bedHeigth_TextBox_TextChanged;
            bedHeigth_TextBox.Text = Settings.bedHeigth.ToString(CultureInfo.InvariantCulture);
            bedHeigth_TextBox.InputScope = new InputScope() { Names = { new InputScopeName() { NameValue = InputScopeNameValue.Number } } };
            SettingStackPanel.Children.Add(new TextBlock() { Text = "bedHeigth" });
            SettingStackPanel.Children.Add(bedHeigth_TextBox);

            TextBox printingTemperature_TextBox = new TextBox();
            printingTemperature_TextBox.TextChanged += printingTemperature_TextBox_TextChanged;
            printingTemperature_TextBox.Text = Settings.printingTemperature.ToString(CultureInfo.InvariantCulture);
            printingTemperature_TextBox.InputScope = new InputScope() { Names = { new InputScopeName() { NameValue = InputScopeNameValue.Number } } };
            SettingStackPanel.Children.Add(new TextBlock() { Text = "printingTemperature" });
            SettingStackPanel.Children.Add(printingTemperature_TextBox);

            TextBox bottomLayerCount_TextBox = new TextBox();
            bottomLayerCount_TextBox.TextChanged += bottomLayerCount_TextBox_TextChanged;
            bottomLayerCount_TextBox.Text = Settings.bottomLayerCount.ToString(CultureInfo.InvariantCulture);
            bottomLayerCount_TextBox.InputScope = new InputScope() { Names = { new InputScopeName() { NameValue = InputScopeNameValue.Number } } };
            SettingStackPanel.Children.Add(new TextBlock() { Text = "bottomLayerCount" });
            SettingStackPanel.Children.Add(bottomLayerCount_TextBox);

            TextBox topLayerCount_TextBox = new TextBox();
            topLayerCount_TextBox.TextChanged += topLayerCount_TextBox_TextChanged;
            topLayerCount_TextBox.Text = Settings.topLayerCount.ToString(CultureInfo.InvariantCulture);
            topLayerCount_TextBox.InputScope = new InputScope() { Names = { new InputScopeName() { NameValue = InputScopeNameValue.Number } } };
            SettingStackPanel.Children.Add(new TextBlock() { Text = "topLayerCount" });
            SettingStackPanel.Children.Add(topLayerCount_TextBox);

            TextBox flowPercentage_TextBox = new TextBox();
            flowPercentage_TextBox.TextChanged += flowPercentage_TextBox_TextChanged;
            flowPercentage_TextBox.Text = Settings.flowPercentage.ToString(CultureInfo.InvariantCulture);
            flowPercentage_TextBox.InputScope = new InputScope() { Names = { new InputScopeName() { NameValue = InputScopeNameValue.Number } } };
            SettingStackPanel.Children.Add(new TextBlock() { Text = "flowPercentage" });
            SettingStackPanel.Children.Add(flowPercentage_TextBox);

            TextBox initialLayerSpeed_TextBox = new TextBox();
            initialLayerSpeed_TextBox.TextChanged += initialLayerSpeed_TextBox_TextChanged;
            initialLayerSpeed_TextBox.Text = Settings.initialLayerSpeed.ToString(CultureInfo.InvariantCulture);
            initialLayerSpeed_TextBox.InputScope = new InputScope() { Names = { new InputScopeName() { NameValue = InputScopeNameValue.Number } } };
            SettingStackPanel.Children.Add(new TextBlock() { Text = "initialLayerSpeed" });
            SettingStackPanel.Children.Add(initialLayerSpeed_TextBox);

            TextBox normalLayerSpeed_TextBox = new TextBox();
            normalLayerSpeed_TextBox.TextChanged += normalLayerSpeed_TextBox_TextChanged;
            normalLayerSpeed_TextBox.Text = Settings.normalLayerSpeed.ToString(CultureInfo.InvariantCulture);
            normalLayerSpeed_TextBox.InputScope = new InputScope() { Names = { new InputScopeName() { NameValue = InputScopeNameValue.Number } } };
            SettingStackPanel.Children.Add(new TextBlock() { Text = "normalLayerSpeed" });
            SettingStackPanel.Children.Add(normalLayerSpeed_TextBox);

            TextBox normalInfillSpeed_TextBox = new TextBox();
            normalInfillSpeed_TextBox.TextChanged += normalInfillSpeed_TextBox_TextChanged;
            normalInfillSpeed_TextBox.Text = Settings.normalInfillSpeed.ToString(CultureInfo.InvariantCulture);
            normalInfillSpeed_TextBox.InputScope = new InputScope() { Names = { new InputScopeName() { NameValue = InputScopeNameValue.Number } } };
            SettingStackPanel.Children.Add(new TextBlock() { Text = "normalInfillSpeed" });
            SettingStackPanel.Children.Add(normalInfillSpeed_TextBox);

            TextBox bridgeingSpeed_TextBox = new TextBox();
            bridgeingSpeed_TextBox.TextChanged += bridgeingSpeed_TextBox_TextChanged;
            bridgeingSpeed_TextBox.Text = Settings.bridgeingSpeed.ToString(CultureInfo.InvariantCulture);
            bridgeingSpeed_TextBox.InputScope = new InputScope() { Names = { new InputScopeName() { NameValue = InputScopeNameValue.Number } } };
            SettingStackPanel.Children.Add(new TextBlock() { Text = "bridgeingSpeed" });
            SettingStackPanel.Children.Add(bridgeingSpeed_TextBox);

            TextBox moveSpeed_TextBox = new TextBox();
            moveSpeed_TextBox.TextChanged += moveSpeed_TextBox_TextChanged;
            moveSpeed_TextBox.Text = Settings.moveSpeed.ToString(CultureInfo.InvariantCulture);
            moveSpeed_TextBox.InputScope = new InputScope() { Names = { new InputScopeName() { NameValue = InputScopeNameValue.Number } } };
            SettingStackPanel.Children.Add(new TextBlock() { Text = "moveSpeed" });
            SettingStackPanel.Children.Add(moveSpeed_TextBox);

            TextBox normalInfillDensity_TextBox = new TextBox();
            normalInfillDensity_TextBox.TextChanged += normalInfillDensity_TextBox_TextChanged;
            normalInfillDensity_TextBox.Text = Settings.normalInfillDensity.ToString(CultureInfo.InvariantCulture);
            normalInfillDensity_TextBox.InputScope = new InputScope() { Names = { new InputScopeName() { NameValue = InputScopeNameValue.Number } } };
            SettingStackPanel.Children.Add(new TextBlock() { Text = "normalInfillDensity" });
            SettingStackPanel.Children.Add(normalInfillDensity_TextBox);
            /*
            TextBox specialLayerSpeeds_TextBox = new TextBox();
            specialLayerSpeeds_TextBox.TextChanged += specialLayerSpeeds_TextBox_TextChanged;
            specialLayerSpeeds_TextBox.Text = Settings.specialLayerSpeeds.ToString();
            specialLayerSpeeds_TextBox.InputScope = new InputScope() { Names = { new InputScopeName() { NameValue = InputScopeNameValue.Number } } };
            SettingStackPanel.Children.Add(new TextBlock() { Text = "specialLayerSpeeds" });
            SettingStackPanel.Children.Add(specialLayerSpeeds_TextBox);

            TextBox specialInfillSpeeds_TextBox = new TextBox();
            specialInfillSpeeds_TextBox.TextChanged += specialInfillSpeeds_TextBox_TextChanged;
            specialInfillSpeeds_TextBox.Text = Settings.specialInfillSpeeds.ToString();
            specialInfillSpeeds_TextBox.InputScope = new InputScope() { Names = { new InputScopeName() { NameValue = InputScopeNameValue.Number } } };
            SettingStackPanel.Children.Add(new TextBlock() { Text = "specialInfillSpeeds" });
            SettingStackPanel.Children.Add(specialInfillSpeeds_TextBox);

            TextBox specialLayerDensities_TextBox = new TextBox();
            specialLayerDensities_TextBox.TextChanged += specialLayerDensities_TextBox_TextChanged;
            specialLayerDensities_TextBox.Text = Settings.specialLayerDensities.ToString();
            specialLayerDensities_TextBox.InputScope = new InputScope() { Names = { new InputScopeName() { NameValue = InputScopeNameValue.Number } } };
            SettingStackPanel.Children.Add(new TextBlock() { Text = "specialLayerDensities" });
            SettingStackPanel.Children.Add(specialLayerDensities_TextBox);
            */
            TextBox nozzleWidth_TextBox = new TextBox();
            nozzleWidth_TextBox.TextChanged += nozzleWidth_TextBox_TextChanged;
            nozzleWidth_TextBox.Text = Settings.nozzleWidth.ToString(CultureInfo.InvariantCulture);
            nozzleWidth_TextBox.InputScope = new InputScope() { Names = { new InputScopeName() { NameValue = InputScopeNameValue.Number } } };
            SettingStackPanel.Children.Add(new TextBlock() { Text = "nozzleWidth" });
            SettingStackPanel.Children.Add(nozzleWidth_TextBox);

            TextBox filamentWidth_TextBox = new TextBox();
            filamentWidth_TextBox.TextChanged += filamentWidth_TextBox_TextChanged;
            filamentWidth_TextBox.Text = Settings.filamentWidth.ToString(CultureInfo.InvariantCulture);
            filamentWidth_TextBox.InputScope = new InputScope() { Names = { new InputScopeName() { NameValue = InputScopeNameValue.Number } } };
            SettingStackPanel.Children.Add(new TextBlock() { Text = "filamentWidth" });
            SettingStackPanel.Children.Add(filamentWidth_TextBox);

            TextBox layerHeight_TextBox = new TextBox();
            layerHeight_TextBox.TextChanged += layerHeight_TextBox_TextChanged;
            layerHeight_TextBox.Text = Settings.layerHeight.ToString(CultureInfo.InvariantCulture);
            layerHeight_TextBox.InputScope = new InputScope() { Names = { new InputScopeName() { NameValue = InputScopeNameValue.Number } } };
            SettingStackPanel.Children.Add(new TextBlock() { Text = "layerHeight" });
            SettingStackPanel.Children.Add(layerHeight_TextBox);

            //TextBox materialType_TextBox = new TextBox();
            //materialType_TextBox.TextChanged += materialType_TextBox_TextChanged;
            //materialType_TextBox.Text = Settings.materialType.ToString();
            //materialType_TextBox.InputScope = new InputScope() { Names = { new InputScopeName() { NameValue = InputScopeNameValue.Text } } };
            RadioButton pla_RadioButton = new RadioButton();
            pla_RadioButton.Content = "PLA";
            pla_RadioButton.GroupName = "filament";
            pla_RadioButton.Checked += pla_RadioButton_Checked;
            pla_RadioButton.IsChecked = (Settings.materialType == "PLA");
            RadioButton abs_RadioButton = new RadioButton();
            abs_RadioButton.Content = "ABS";
            abs_RadioButton.GroupName = "filament";
            abs_RadioButton.Checked += abs_RadioButton_Checked;
            abs_RadioButton.IsChecked = (Settings.materialType == "ABS");
            SettingStackPanel.Children.Add(new TextBlock() { Text = "materialType" });
            //SettingStackPanel.Children.Add(materialType_TextBox);
            SettingStackPanel.Children.Add(pla_RadioButton);
            SettingStackPanel.Children.Add(abs_RadioButton);

            TextBox costPerKg_TextBox = new TextBox();
            costPerKg_TextBox.TextChanged += costPerKg_TextBox_TextChanged;
            costPerKg_TextBox.Text = Settings.costPerKg.ToString(CultureInfo.InvariantCulture);
            costPerKg_TextBox.InputScope = new InputScope() { Names = { new InputScopeName() { NameValue = InputScopeNameValue.Number } } };
            SettingStackPanel.Children.Add(new TextBlock() { Text = "costPerKg" });
            SettingStackPanel.Children.Add(costPerKg_TextBox);

            TextBox costPerMinute_TextBox = new TextBox();
            costPerMinute_TextBox.TextChanged += costPerMinute_TextBox_TextChanged;
            costPerMinute_TextBox.Text = Settings.costPerMinute.ToString(CultureInfo.InvariantCulture);
            costPerMinute_TextBox.InputScope = new InputScope() { Names = { new InputScopeName() { NameValue = InputScopeNameValue.Number } } };
            SettingStackPanel.Children.Add(new TextBlock() { Text = "costPerMinute" });
            SettingStackPanel.Children.Add(costPerMinute_TextBox);

            TextBox skirtSpeed_TextBox = new TextBox();
            skirtSpeed_TextBox.TextChanged += skirtSpeed_TextBox_TextChanged;
            skirtSpeed_TextBox.Text = Settings.skirtSpeed.ToString(CultureInfo.InvariantCulture);
            skirtSpeed_TextBox.InputScope = new InputScope() { Names = { new InputScopeName() { NameValue = InputScopeNameValue.Number } } };
            SettingStackPanel.Children.Add(new TextBlock() { Text = "skirtSpeed" });
            SettingStackPanel.Children.Add(skirtSpeed_TextBox);

            TextBox skirtCount_TextBox = new TextBox();
            skirtCount_TextBox.TextChanged += skirtCount_TextBox_TextChanged;
            skirtCount_TextBox.Text = Settings.skirtCount.ToString(CultureInfo.InvariantCulture);
            skirtCount_TextBox.InputScope = new InputScope() { Names = { new InputScopeName() { NameValue = InputScopeNameValue.Number } } };
            SettingStackPanel.Children.Add(new TextBlock() { Text = "skirtCount" });
            SettingStackPanel.Children.Add(skirtCount_TextBox);

            TextBox skirtDistance_TextBox = new TextBox();
            skirtDistance_TextBox.TextChanged += skirtDistance_TextBox_TextChanged;
            skirtDistance_TextBox.Text = Settings.skirtDistance.ToString(CultureInfo.InvariantCulture);
            skirtDistance_TextBox.InputScope = new InputScope() { Names = { new InputScopeName() { NameValue = InputScopeNameValue.Number } } };
            SettingStackPanel.Children.Add(new TextBlock() { Text = "skirtDistance" });
            SettingStackPanel.Children.Add(skirtDistance_TextBox);

            CheckBox shouldSkirt_CheckBox = new CheckBox();
            shouldSkirt_CheckBox.Click += shouldSkirt_CheckBox_Click;
            shouldSkirt_CheckBox.IsChecked = Settings.shouldSkirt;
            SettingStackPanel.Children.Add(new TextBlock() { Text = "shouldSkirt" });
            SettingStackPanel.Children.Add(shouldSkirt_CheckBox);

            TextBox shellThickness_TextBox = new TextBox();
            shellThickness_TextBox.TextChanged += shellThickness_TextBox_TextChanged;
            shellThickness_TextBox.Text = Settings.shellThickness.ToString(CultureInfo.InvariantCulture);
            shellThickness_TextBox.InputScope = new InputScope() { Names = { new InputScopeName() { NameValue = InputScopeNameValue.Number } } };
            SettingStackPanel.Children.Add(new TextBlock() { Text = "shellThickness" });
            SettingStackPanel.Children.Add(shellThickness_TextBox);

            CheckBox shouldSupportMaterial_CheckBox = new CheckBox();
            shouldSupportMaterial_CheckBox.Click += shouldSupportMaterial_CheckBox_Click;
            shouldSupportMaterial_CheckBox.IsChecked = Settings.shouldSupportMaterial;
            SettingStackPanel.Children.Add(new TextBlock() { Text = "shouldSupportMaterial" });
            SettingStackPanel.Children.Add(shouldSupportMaterial_CheckBox);

            TextBox supportMaterialDensity_TextBox = new TextBox();
            supportMaterialDensity_TextBox.TextChanged += supportMaterialDensity_TextBox_TextChanged;
            supportMaterialDensity_TextBox.Text = Settings.supportMaterialDensity.ToString(CultureInfo.InvariantCulture);
            supportMaterialDensity_TextBox.InputScope = new InputScope() { Names = { new InputScopeName() { NameValue = InputScopeNameValue.Number } } };
            SettingStackPanel.Children.Add(new TextBlock() { Text = "supportMaterialDensity" });
            SettingStackPanel.Children.Add(supportMaterialDensity_TextBox);

            TextBox supportSpeed_TextBox = new TextBox();
            supportSpeed_TextBox.TextChanged += supportSpeed_TextBox_TextChanged;
            supportSpeed_TextBox.Text = Settings.supportSpeed.ToString(CultureInfo.InvariantCulture);
            supportSpeed_TextBox.InputScope = new InputScope() { Names = { new InputScopeName() { NameValue = InputScopeNameValue.Number } } };
            SettingStackPanel.Children.Add(new TextBlock() { Text = "supportSpeed" });
            SettingStackPanel.Children.Add(supportSpeed_TextBox);

            TextBox retractionSpeed_TextBox = new TextBox();
            retractionSpeed_TextBox.TextChanged += retractionSpeed_TextBox_TextChanged;
            retractionSpeed_TextBox.Text = Settings.retractionSpeed.ToString(CultureInfo.InvariantCulture);
            retractionSpeed_TextBox.InputScope = new InputScope() { Names = { new InputScopeName() { NameValue = InputScopeNameValue.Number } } };
            SettingStackPanel.Children.Add(new TextBlock() { Text = "retractionSpeed" });
            SettingStackPanel.Children.Add(retractionSpeed_TextBox);

            TextBox retractionDistance_TextBox = new TextBox();
            retractionDistance_TextBox.TextChanged += retractionDistance_TextBox_TextChanged;
            retractionDistance_TextBox.Text = Settings.retractionDistance.ToString(CultureInfo.InvariantCulture);
            retractionDistance_TextBox.InputScope = new InputScope() { Names = { new InputScopeName() { NameValue = InputScopeNameValue.Number } } };
            SettingStackPanel.Children.Add(new TextBlock() { Text = "retractionDistance" });
            SettingStackPanel.Children.Add(retractionDistance_TextBox);

            CheckBox shouldRaft_CheckBox = new CheckBox();
            shouldRaft_CheckBox.Click += shouldRaft_CheckBox_Click;
            shouldRaft_CheckBox.IsChecked = Settings.shouldRaft;
            SettingStackPanel.Children.Add(new TextBlock() { Text = "shouldRaft" });
            SettingStackPanel.Children.Add(shouldRaft_CheckBox);

            TextBox raftDensity_TextBox = new TextBox();
            raftDensity_TextBox.TextChanged += raftDensity_TextBox_TextChanged;
            raftDensity_TextBox.Text = Settings.raftDensity.ToString(CultureInfo.InvariantCulture);
            raftDensity_TextBox.InputScope = new InputScope() { Names = { new InputScopeName() { NameValue = InputScopeNameValue.Number } } };
            SettingStackPanel.Children.Add(new TextBlock() { Text = "raftDensity" });
            SettingStackPanel.Children.Add(raftDensity_TextBox);

            TextBox raftDistance_TextBox = new TextBox();
            raftDistance_TextBox.TextChanged += raftDistance_TextBox_TextChanged;
            raftDistance_TextBox.Text = Settings.raftDistance.ToString(CultureInfo.InvariantCulture);
            raftDistance_TextBox.InputScope = new InputScope() { Names = { new InputScopeName() { NameValue = InputScopeNameValue.Number } } };
            SettingStackPanel.Children.Add(new TextBlock() { Text = "raftDistance" });
            SettingStackPanel.Children.Add(raftDistance_TextBox);

            TextBox raftCount_TextBox = new TextBox();
            raftCount_TextBox.TextChanged += raftCount_TextBox_TextChanged;
            raftCount_TextBox.Text = Settings.raftCount.ToString(CultureInfo.InvariantCulture);
            raftCount_TextBox.InputScope = new InputScope() { Names = { new InputScopeName() { NameValue = InputScopeNameValue.Number } } };
            SettingStackPanel.Children.Add(new TextBlock() { Text = "raftCount" });
            SettingStackPanel.Children.Add(raftCount_TextBox);

            TextBox raftSpeed_TextBox = new TextBox();
            raftSpeed_TextBox.TextChanged += raftSpeed_TextBox_TextChanged;
            raftSpeed_TextBox.Text = Settings.raftSpeed.ToString(CultureInfo.InvariantCulture);
            raftSpeed_TextBox.InputScope = new InputScope() { Names = { new InputScopeName() { NameValue = InputScopeNameValue.Number } } };
            SettingStackPanel.Children.Add(new TextBlock() { Text = "raftSpeed" });
            SettingStackPanel.Children.Add(raftSpeed_TextBox);

            TextBox infillCombinationCount_TextBox = new TextBox();
            infillCombinationCount_TextBox.TextChanged += infillCombinationCount_TextBox_TextChanged;
            infillCombinationCount_TextBox.Text = Settings.infillCombinationCount.ToString();
            infillCombinationCount_TextBox.InputScope = new InputScope() { Names = { new InputScopeName() { NameValue = InputScopeNameValue.Number } } };
            SettingStackPanel.Children.Add(new TextBlock() { Text = "infillCombinationCount" });
            SettingStackPanel.Children.Add(infillCombinationCount_TextBox);
            /*
            TextBox specialFlowrates_TextBox = new TextBox();
            specialFlowrates_TextBox.TextChanged += specialFlowrates_TextBox_TextChanged;
            specialFlowrates_TextBox.Text = Settings.specialFlowrates.ToString();
            specialFlowrates_TextBox.InputScope = new InputScope() { Names = { new InputScopeName() { NameValue = InputScopeNameValue.Number } } };
            SettingStackPanel.Children.Add(new TextBlock() { Text = "specialFlowrates" });
            SettingStackPanel.Children.Add(specialFlowrates_TextBox);

            TextBox specialMoveSpeeds_TextBox = new TextBox();
            specialMoveSpeeds_TextBox.TextChanged += specialMoveSpeeds_TextBox_TextChanged;
            specialMoveSpeeds_TextBox.Text = Settings.specialMoveSpeeds.ToString();
            specialMoveSpeeds_TextBox.InputScope = new InputScope() { Names = { new InputScopeName() { NameValue = InputScopeNameValue.Number } } };
            SettingStackPanel.Children.Add(new TextBlock() { Text = "specialMoveSpeeds" });
            SettingStackPanel.Children.Add(specialMoveSpeeds_TextBox);

            TextBox specialBridgeingSpeeds_TextBox = new TextBox();
            specialBridgeingSpeeds_TextBox.TextChanged += specialBridgeingSpeeds_TextBox_TextChanged;
            specialBridgeingSpeeds_TextBox.Text = Settings.specialBridgeingSpeeds.ToString();
            specialBridgeingSpeeds_TextBox.InputScope = new InputScope() { Names = { new InputScopeName() { NameValue = InputScopeNameValue.Number } } };
            SettingStackPanel.Children.Add(new TextBlock() { Text = "specialBridgeingSpeeds" });
            SettingStackPanel.Children.Add(specialBridgeingSpeeds_TextBox);

            TextBox specialSupportDensities_TextBox = new TextBox();
            specialSupportDensities_TextBox.TextChanged += specialSupportDensities_TextBox_TextChanged;
            specialSupportDensities_TextBox.Text = Settings.specialSupportDensities.ToString();
            specialSupportDensities_TextBox.InputScope = new InputScope() { Names = { new InputScopeName() { NameValue = InputScopeNameValue.Number } } };
            SettingStackPanel.Children.Add(new TextBlock() { Text = "specialSupportDensities" });
            SettingStackPanel.Children.Add(specialSupportDensities_TextBox);

            TextBox specialSupportSpeeds_TextBox = new TextBox();
            specialSupportSpeeds_TextBox.TextChanged += specialSupportSpeeds_TextBox_TextChanged;
            specialSupportSpeeds_TextBox.Text = Settings.specialSupportSpeeds.ToString();
            specialSupportSpeeds_TextBox.InputScope = new InputScope() { Names = { new InputScopeName() { NameValue = InputScopeNameValue.Number } } };
            SettingStackPanel.Children.Add(new TextBlock() { Text = "specialSupportSpeeds" });
            SettingStackPanel.Children.Add(specialSupportSpeeds_TextBox);
            */
            TextBox minimumLayerTime_TextBox = new TextBox();
            minimumLayerTime_TextBox.TextChanged += minimumLayerTime_TextBox_TextChanged;
            minimumLayerTime_TextBox.Text = Settings.minimumLayerTime.ToString(CultureInfo.InvariantCulture);
            minimumLayerTime_TextBox.InputScope = new InputScope() { Names = { new InputScopeName() { NameValue = InputScopeNameValue.Number } } };
            SettingStackPanel.Children.Add(new TextBlock() { Text = "minimumLayerTime" });
            SettingStackPanel.Children.Add(minimumLayerTime_TextBox);

            TextBox minimumLayerSpeed_TextBox = new TextBox();
            minimumLayerSpeed_TextBox.TextChanged += minimumLayerSpeed_TextBox_TextChanged;
            minimumLayerSpeed_TextBox.Text = Settings.minimumLayerSpeed.ToString(CultureInfo.InvariantCulture);
            minimumLayerSpeed_TextBox.InputScope = new InputScope() { Names = { new InputScopeName() { NameValue = InputScopeNameValue.Number } } };
            SettingStackPanel.Children.Add(new TextBlock() { Text = "minimumLayerSpeed" });
            SettingStackPanel.Children.Add(minimumLayerSpeed_TextBox);
        }

        #region ui events
        void abs_RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            Settings.materialType = "ABS";
        }

        void pla_RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            Settings.materialType = "PLA";
        }

        void shouldRaft_CheckBox_Click(object sender, RoutedEventArgs e)
        {
            Settings.shouldRaft = (sender as CheckBox).IsChecked.Value;
        }

        void shouldSupportMaterial_CheckBox_Click(object sender, RoutedEventArgs e)
        {
            Settings.shouldSupportMaterial = (sender as CheckBox).IsChecked.Value;
        }

        void shouldSkirt_CheckBox_Click(object sender, RoutedEventArgs e)
        {
            Settings.shouldSkirt = (sender as CheckBox).IsChecked.Value;
        }

        private void printingTemperature_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            short num;
            if (short.TryParse((sender as TextBox).Text, NumberStyles.Number, CultureInfo.InvariantCulture, out num))
                Settings.printingTemperature = num;
        }

        private void minimumLayerSpeed_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            float num;
            if (float.TryParse((sender as TextBox).Text, NumberStyles.Number, CultureInfo.InvariantCulture, out num))
                Settings.minimumLayerSpeed = num;
        }

        private void minimumLayerTime_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            float num;
            if (float.TryParse((sender as TextBox).Text, NumberStyles.Number, CultureInfo.InvariantCulture, out num))
                Settings.minimumLayerTime = num;
        }

        private void specialSupportSpeeds_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Settings.specialSupportSpeeds = (sender as TextBox).Text;
        }

        private void specialSupportDensities_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Settings.specialLayerDensities = (sender as TextBox).Text;
        }

        private void specialBridgeingSpeeds_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Settings.specialBridgeingSpeeds = (sender as TextBox).Text;
        }

        private void specialMoveSpeeds_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Settings.specialMoveSpeeds = (sender as TextBox).Text;
        }

        private void specialFlowrates_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Settings.specialFlowrates = (sender as TextBox).Text;
        }

        private void infillCombinationCount_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int num;
            if (int.TryParse((sender as TextBox).Text, NumberStyles.Number, CultureInfo.InvariantCulture, out num))
                Settings.infillCombinationCount = num;
        }

        private void raftSpeed_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            float num;
            if (float.TryParse((sender as TextBox).Text, NumberStyles.Number, CultureInfo.InvariantCulture, out num))
                Settings.raftSpeed = num;
        }

        private void raftCount_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int num;
            if (int.TryParse((sender as TextBox).Text, NumberStyles.Number, CultureInfo.InvariantCulture, out num))
                Settings.raftCount = num;
        }

        private void raftDistance_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            float num;
            if (float.TryParse((sender as TextBox).Text, NumberStyles.Number, CultureInfo.InvariantCulture, out num))
                Settings.raftDistance = num;
        }

        private void raftDensity_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            float num;
            if (float.TryParse((sender as TextBox).Text, NumberStyles.Number, CultureInfo.InvariantCulture, out num))
                Settings.raftDensity = num;
        }

        private void retractionDistance_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            float num;
            if (float.TryParse((sender as TextBox).Text, NumberStyles.Number, CultureInfo.InvariantCulture, out num))
                Settings.retractionDistance = num;
        }

        private void retractionSpeed_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            float num;
            if (float.TryParse((sender as TextBox).Text, NumberStyles.Number, CultureInfo.InvariantCulture, out num))
                Settings.retractionSpeed = num;
        }

        private void supportSpeed_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            float num;
            if (float.TryParse((sender as TextBox).Text, NumberStyles.Number, CultureInfo.InvariantCulture, out num))
                Settings.supportSpeed = num;
        }

        private void supportMaterialDensity_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            float num;
            if (float.TryParse((sender as TextBox).Text, NumberStyles.Number, CultureInfo.InvariantCulture, out num))
                Settings.supportMaterialDensity = num;
        }

        private void shellThickness_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int num;
            if (int.TryParse((sender as TextBox).Text, NumberStyles.Number, CultureInfo.InvariantCulture, out num))
                Settings.shellThickness = num;
        }

        private void skirtDistance_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            float num;
            if (float.TryParse((sender as TextBox).Text, NumberStyles.Number, CultureInfo.InvariantCulture, out num))
                Settings.skirtDistance = num;
        }

        private void skirtCount_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int num;
            if (int.TryParse((sender as TextBox).Text, NumberStyles.Number, CultureInfo.InvariantCulture, out num))
                Settings.skirtCount = num;
        }

        private void skirtSpeed_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            float num;
            if (float.TryParse((sender as TextBox).Text, NumberStyles.Number, CultureInfo.InvariantCulture, out num))
                Settings.skirtSpeed = num;
        }

        private void costPerMinute_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            float num;
            if (float.TryParse((sender as TextBox).Text, NumberStyles.Number, CultureInfo.InvariantCulture, out num))
                Settings.costPerMinute = num;
        }

        private void costPerKg_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            float num;
            if (float.TryParse((sender as TextBox).Text, NumberStyles.Number, CultureInfo.InvariantCulture, out num))
                Settings.costPerKg = num;
        }

        private void layerHeight_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            float num;
            if (float.TryParse((sender as TextBox).Text, NumberStyles.Number, CultureInfo.InvariantCulture, out num))
                Settings.layerHeight = num;
        }

        private void filamentWidth_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            float num;
            if (float.TryParse((sender as TextBox).Text, NumberStyles.Number, CultureInfo.InvariantCulture, out num))
                Settings.filamentWidth = num;
        }

        private void nozzleWidth_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            float num;
            if (float.TryParse((sender as TextBox).Text, NumberStyles.Number, CultureInfo.InvariantCulture, out num))
                Settings.nozzleWidth = num;
        }

        private void specialLayerDensities_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Settings.specialLayerDensities = (sender as TextBox).Text;
        }

        private void specialInfillSpeeds_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Settings.specialInfillSpeeds = (sender as TextBox).Text;
        }

        private void specialLayerSpeeds_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Settings.specialLayerSpeeds = (sender as TextBox).Text;
        }

        private void normalInfillDensity_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            float num;
            if (float.TryParse((sender as TextBox).Text, NumberStyles.Number, CultureInfo.InvariantCulture, out num))
                Settings.normalInfillDensity = num;
        }

        private void moveSpeed_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            float num;
            if (float.TryParse((sender as TextBox).Text, NumberStyles.Number, CultureInfo.InvariantCulture, out num))
                Settings.moveSpeed = num;
        }

        private void bridgeingSpeed_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            float num;
            if (float.TryParse((sender as TextBox).Text, NumberStyles.Number, CultureInfo.InvariantCulture, out num))
                Settings.bridgeingSpeed = num;
        }

        private void normalInfillSpeed_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            float num;
            if (float.TryParse((sender as TextBox).Text, NumberStyles.Number, CultureInfo.InvariantCulture, out num))
                Settings.normalInfillSpeed = num;
        }

        private void normalLayerSpeed_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            float num;
            if (float.TryParse((sender as TextBox).Text, NumberStyles.Number, CultureInfo.InvariantCulture, out num))
                Settings.normalLayerSpeed = num;
        }

        private void initialLayerSpeed_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            float num;
            if (float.TryParse((sender as TextBox).Text, NumberStyles.Number, CultureInfo.InvariantCulture, out num))
                Settings.initialLayerSpeed = num;
        }

        private void flowPercentage_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            float num;
            if (float.TryParse((sender as TextBox).Text, NumberStyles.Number, CultureInfo.InvariantCulture, out num))
                Settings.flowPercentage = num;
        }

        private void topLayerCount_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int num;
            if (int.TryParse((sender as TextBox).Text, NumberStyles.Number, CultureInfo.InvariantCulture, out num))
                Settings.topLayerCount = num;
        }

        private void bottomLayerCount_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int num;
            if (int.TryParse((sender as TextBox).Text, NumberStyles.Number, CultureInfo.InvariantCulture, out num))
                Settings.bottomLayerCount = num;
        }

        private void bedHeigth_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int num;
            if (int.TryParse((sender as TextBox).Text, NumberStyles.Number, CultureInfo.InvariantCulture, out num))
                Settings.bedHeigth = num;
        }

        private void bedLength_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int num;
            if (int.TryParse((sender as TextBox).Text, NumberStyles.Number, CultureInfo.InvariantCulture, out num))
                Settings.bedLength = num;
        }

        private void bedWidth_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int num;
            if (int.TryParse((sender as TextBox).Text, NumberStyles.Number, CultureInfo.InvariantCulture, out num))
                Settings.bedWidth = num;
        }
        #endregion

        //This gets called when the settings pivot gets focus
        public void SettingsPivot_Load()
        {
            ApplicationBar = settingsBar;
            ApplicationBar.IsVisible = true;
        }

        //This gets called when the settings pivot loses focus
        public void SettingsPivot_Unload()
        {
            ApplicationBar.IsVisible = false;
        }

        private void buildSettingsApplicationBar()
        {
            // Set the page's ApplicationBar to a new instance of ApplicationBar.
            //ApplicationBar = new ApplicationBar();
            settingsBar.Mode = ApplicationBarMode.Minimized;

            // Create a new menu item
            ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem("About");
            settingsBar.MenuItems.Add(appBarMenuItem);
            appBarMenuItem.Click += about_Click;

            ApplicationBarMenuItem appBarMenuItem2 = new ApplicationBarMenuItem("Help");
            settingsBar.MenuItems.Add(appBarMenuItem2);
            appBarMenuItem2.Click += help_Click;
        }

        private void about_Click(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/AboutPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void help_Click(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/HelpPage.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}
