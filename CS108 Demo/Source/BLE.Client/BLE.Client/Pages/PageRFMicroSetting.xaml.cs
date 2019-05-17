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
        string[] _sensorTypes = { "Sensor Code", "Temperature" };
        int [] _minOCRSSIs = { 0, 8 };
        int [] _maxOCRSSIs = { 21, 18 };

        public PageRFMicroSetting()
        {
            InitializeComponent();

            SetParas(1);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        public async void buttonTagTypeClicked(object sender, EventArgs e)
        {

        }

        public async void buttonSensorTypeClicked(object sender, EventArgs e)
        {
            var answer = await DisplayActionSheet("Sensor Type", "Cancel", null, _sensorTypes);

            if (answer != "Cancel")
            {
                for (uint index = 0; index < _sensorTypes.Length; index ++)
                {
                    if (answer == _sensorTypes[index])
                    {
                        SetParas(index);
                        break;
                    }
                }
            }
        }

        bool SetParas(uint index)
        {
            if (index >= _sensorTypes.Length)
                return false;

            buttonSensorType.Text = _sensorTypes[index];
            entryMinOCRSSI.Text = _minOCRSSIs[index].ToString();
            entryMaxOCRSSI.Text = _maxOCRSSIs[index].ToString();

            return true;
        }
    }
}
