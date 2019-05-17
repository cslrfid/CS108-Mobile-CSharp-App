using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BLE.Client.Pages
{
	public partial class PageRFMicroSetting
    {
        string[] _powerOptions = { "Low (16dBm)", "Mid (23dBm)", "High (30dBm)", "Auto (Trigger Cycle)", "Follow system Setting" };
        string[] _indicatorsProfileOptions = { "Hot temperature", "Cold temperature", "Moisture detection" };
        string[] _sensorTypeOptions = { "Sensor Code", "Temperature" };
        string[] _sensorCodeUnitOptions = { "code", "%" };
        string[] _temperatureUnitOptions = { "code", "ºF", "ºC" };
        int[] _minOCRSSIs = { 0, 8 };
        int [] _maxOCRSSIs = { 21, 18 };
        string[] _thresholdComparisonOptions = { ">", "<" };
        int[] _thresholdValueOptions = { 100, -1, 58 };
        string [] _thresholdColorOptions = { "Red", "Blue"};

        public PageRFMicroSetting()
        {
            InitializeComponent();

            buttonPower.Text = _powerOptions[2];
            SetIndicatorsProfile(0);
        }

        protected override void OnAppearing()
        {
            buttonOK.RemoveBinding(Button.CommandProperty);
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            buttonOK.RemoveBinding(Button.CommandProperty);
            base.OnDisappearing();
        }

        public async void buttonTagTypeClicked(object sender, EventArgs e)
        {
        }

        public async void buttonPowerClicked(object sender, EventArgs e)
        {
            var answer = await DisplayActionSheet("Power", "Cancel", null, _powerOptions);

            if (answer != "Cancel")
            {
                buttonPower.Text = answer;
            }
        }

        public async void buttonSensorTypeClicked(object sender, EventArgs e)
        {
            var answer = await DisplayActionSheet("Sensor Type", "Cancel", null, _sensorTypeOptions);

            if (answer != "Cancel")
            {
                SetSensorType((uint)Array.IndexOf(_sensorTypeOptions, answer));
            }
        }

        public async void buttonIndicatorsProfileClicked(object sender, EventArgs e)
        {
            var answer = await DisplayActionSheet("Indicators Profile", "Cancel", null, _indicatorsProfileOptions);

            if (answer != "Cancel")
            {
                SetIndicatorsProfile((uint)Array.IndexOf(_indicatorsProfileOptions, answer));
            }
        }

        public async void buttonSensorUnitClicked(object sender, EventArgs e)
        {
            string answer;

            if (buttonSensorType.Text == _sensorTypeOptions[0])
                answer = await DisplayActionSheet("Sensor Unit", "Cancel", null, _sensorCodeUnitOptions);
            else
                answer = await DisplayActionSheet("Sensor Unit", "Cancel", null, _temperatureUnitOptions);

            if (answer != "Cancel")
            {
                buttonSensorUnit.Text = answer;
            }
        }

        public async void buttonThresholdComparisonClicked(object sender, EventArgs e)
        {
            string answer = await DisplayActionSheet("Threshold Comparison", "Cancel", null, _thresholdComparisonOptions);

            if (answer != "Cancel")
            {
                buttonThresholdComparison.Text = answer;
            }
        }

        public async void buttonThresholdColorClicked(object sender, EventArgs e)
        {
            string answer = await DisplayActionSheet("Threshold Color", "Cancel", null, _thresholdColorOptions);

            if (answer != "Cancel")
            {
                buttonThresholdColor.Text = answer;
            }
        }

        public async void ButtonOK_Clicked(object sender, EventArgs e)
        {
            BleMvxApplication._rfMicro_Power = Array.IndexOf(_powerOptions, buttonPower.Text);
            BleMvxApplication._rfMicro_SensorType = Array.IndexOf(_sensorTypeOptions, buttonSensorType.Text);
            switch (BleMvxApplication._rfMicro_SensorType)
            {
                case 0:
                    BleMvxApplication._rfMicro_SensorUnit = Array.IndexOf(_sensorCodeUnitOptions, buttonSensorUnit.Text);
                    if (BleMvxApplication._rfMicro_SensorUnit == 1)
                        BleMvxApplication._rfMicro_SensorUnit = 3;
                    break;
                default:
                    BleMvxApplication._rfMicro_SensorUnit = Array.IndexOf(_temperatureUnitOptions, buttonSensorUnit.Text);
                    break;
            }
            BleMvxApplication._rfMicro_minOCRSSI = int.Parse(entryMinOCRSSI.Text);
            BleMvxApplication._rfMicro_maxOCRSSI = int.Parse(entryMaxOCRSSI.Text);
            BleMvxApplication._rfMicro_thresholdComparison = Array.IndexOf(_thresholdComparisonOptions, buttonThresholdComparison.Text);
            BleMvxApplication._rfMicro_thresholdValue = int.Parse(entryThresholdValue.Text);
            BleMvxApplication._rfMicro_thresholdColor = buttonThresholdColor.Text;

            buttonOK.SetBinding(Button.CommandProperty, new Binding("OnOKButtonCommand"));
            buttonOK.Command.Execute(1);
            buttonOK.RemoveBinding(Button.CommandProperty);
        }

        bool SetIndicatorsProfile(uint index)
        {
            switch (index)
            {
                case 0:
                    buttonIndicatorsProfile.Text = _indicatorsProfileOptions[0];
                    SetSensorType(1);
                    buttonSensorUnit.Text = _temperatureUnitOptions[1];
                    buttonThresholdComparison.Text = _thresholdComparisonOptions[0];
                    entryThresholdValue.Text = _thresholdValueOptions[0].ToString();
                    buttonThresholdColor.Text = _thresholdColorOptions[0];
                    break;
                case 1:
                    buttonIndicatorsProfile.Text = _indicatorsProfileOptions[1];
                    SetSensorType(1);
                    buttonSensorUnit.Text = _temperatureUnitOptions[2];
                    buttonThresholdComparison.Text = _thresholdComparisonOptions[1];
                    entryThresholdValue.Text = _thresholdValueOptions[1].ToString();
                    buttonThresholdColor.Text = _thresholdColorOptions[1];
                    break;
                case 2:
                    buttonIndicatorsProfile.Text = _indicatorsProfileOptions[2];
                    SetSensorType(0);
                    buttonSensorUnit.Text = _sensorCodeUnitOptions[1];
                    buttonThresholdComparison.Text = _thresholdComparisonOptions[0];
                    entryThresholdValue.Text = _thresholdValueOptions[2].ToString();
                    buttonThresholdColor.Text = _thresholdColorOptions[1];
                    break;
                default:
                    return false;
            }

            return true;
        }

        bool SetSensorType(uint index)
        {
            if (index >= _sensorTypeOptions.Length)
                return false;

            buttonSensorType.Text = _sensorTypeOptions[index];
            entryMinOCRSSI.Text = _minOCRSSIs[index].ToString();
            entryMaxOCRSSI.Text = _maxOCRSSIs[index].ToString();

            switch (index)
            {
                case 0:
                    buttonSensorUnit.Text = _sensorCodeUnitOptions[0];
                    break;
                default:
                    buttonSensorUnit.Text = _temperatureUnitOptions[2];
                    break;
            }

            return true;
        }
    }
}
