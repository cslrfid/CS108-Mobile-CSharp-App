using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BLE.Client.Pages
{
	public partial class PageSetting1
	{
        List<CSLibrary.Constants.RegionCode> Regions;
        string[] ActiveRegionsTextList;
        double[] ActiveFrequencyList;
        string[] ActiveFrequencyTextList;

        CSLibrary.Constants.RegionCode [] _regionsCode = new CSLibrary.Constants.RegionCode[] {
            CSLibrary.Constants.RegionCode.FCC,
            CSLibrary.Constants.RegionCode.ETSI,
            CSLibrary.Constants.RegionCode.CN,
            CSLibrary.Constants.RegionCode.TW,
            CSLibrary.Constants.RegionCode.KR,
            CSLibrary.Constants.RegionCode.HK,
            CSLibrary.Constants.RegionCode.JP,
            CSLibrary.Constants.RegionCode.AU,
            CSLibrary.Constants.RegionCode.MY,
            CSLibrary.Constants.RegionCode.SG,
            CSLibrary.Constants.RegionCode.IN,
            CSLibrary.Constants.RegionCode.G800,
            CSLibrary.Constants.RegionCode.ZA,
            CSLibrary.Constants.RegionCode.BR1,
            CSLibrary.Constants.RegionCode.BR2,
            CSLibrary.Constants.RegionCode.BR3,
            CSLibrary.Constants.RegionCode.BR4,
            CSLibrary.Constants.RegionCode.BR5,
            CSLibrary.Constants.RegionCode.ID,
            CSLibrary.Constants.RegionCode.TH,
            CSLibrary.Constants.RegionCode.JE,
            CSLibrary.Constants.RegionCode.PH,
            CSLibrary.Constants.RegionCode.ETSIUPPERBAND,
            CSLibrary.Constants.RegionCode.NZ,
            CSLibrary.Constants.RegionCode.UH1,
            CSLibrary.Constants.RegionCode.UH2,
            CSLibrary.Constants.RegionCode.LH,
            CSLibrary.Constants.RegionCode.LH1,
            CSLibrary.Constants.RegionCode.LH2,

            CSLibrary.Constants.RegionCode.VE,
            CSLibrary.Constants.RegionCode.AR,
            CSLibrary.Constants.RegionCode.CL,
            CSLibrary.Constants.RegionCode.CO,
            CSLibrary.Constants.RegionCode.CR,
            CSLibrary.Constants.RegionCode.DO,
            CSLibrary.Constants.RegionCode.MX,
            CSLibrary.Constants.RegionCode.PA,
            CSLibrary.Constants.RegionCode.PE,
            CSLibrary.Constants.RegionCode.UY
        };
        string[] _regionsName = new string[] {
            "USACanada",
            "Europe",
            "China",
            "Taiwan",
            "Korea",
            "Hong Kong",
            "Japan",
            "Australia",
            "Malaysia",
            "Singapore",
            "India",
            "G800",
            "South Africa",
            "Brazil 915-927",
            "Brazil 902-906, 915-927",
            "Brazil 902-906",
            "Brazil 902-904",
            "Brazil 917-924",
            "Indonesia",
            "Thailand",
            "Israel",
            "Philippine",
            "ETSI Upper Band",
            "New Zealand",
            "UH1",
            "UH2",
            "LH",
            "LH1",
            "LH2",
            "Venezuela",
            "Argentina",
            "Chile",
            "Colombia",
            "Costa Rica",
            "Dominican Republic",
            "Mexico",
            "Panama",
            "Peru",
            "Uruguay"
        };


#if nouse
        CSLibrary.Constants.RegionCode[] _fccRegionsCodeList = new CSLibrary.Constants.RegionCode[]  {
            CSLibrary.Constants.RegionCode.FCC,
            CSLibrary.Constants.RegionCode.BR1,
            CSLibrary.Constants.RegionCode.BR2,
            CSLibrary.Constants.RegionCode.BR3,
            CSLibrary.Constants.RegionCode.BR4,
            CSLibrary.Constants.RegionCode.BR5,
            CSLibrary.Constants.RegionCode.FCC,
            CSLibrary.Constants.RegionCode.FCC,
            CSLibrary.Constants.RegionCode.FCC,
            CSLibrary.Constants.RegionCode.FCC,
            CSLibrary.Constants.RegionCode.JE,
            CSLibrary.Constants.RegionCode.FCC,
            CSLibrary.Constants.RegionCode.FCC,
            CSLibrary.Constants.RegionCode.BR1,
            CSLibrary.Constants.RegionCode.PH,
            CSLibrary.Constants.RegionCode.SG,
            CSLibrary.Constants.RegionCode.ZA,
            CSLibrary.Constants.RegionCode.FCC,
            CSLibrary.Constants.RegionCode.VE,//            "Venezuela",
            CSLibrary.Constants.RegionCode.AU,
            CSLibrary.Constants.RegionCode.HK,
            CSLibrary.Constants.RegionCode.MY,
            CSLibrary.Constants.RegionCode.TH,
            CSLibrary.Constants.RegionCode.ID,
            CSLibrary.Constants.RegionCode.FCC,
            CSLibrary.Constants.RegionCode.LH1,
            CSLibrary.Constants.RegionCode.LH2,
            CSLibrary.Constants.RegionCode.UH1,
            CSLibrary.Constants.RegionCode.UH2,
            };

        string[] _fccRegionsList = new string[] {
            "Argentina",
            "Brazil 915-927",
            "Brazil 902-906, 915-927",
            "Brazil 902-906",
            "Brazil 902-904",
            "Brazil 917-924",
            "Chile",
            "Colombia",
            "Costa Rica",
            "Dominican Republic",
            "Israel",
            "Mexico",
            "Panama",
            "Peru",
            "Philippines",
            "Singapore",
            "South Africa",
            "Uruguay",
            "Venezuela",
            "Australia",
            "Hong Kong",
            "Malaysia",
            "Thailand",
            "Indonesia",
            "USACanada",
            "LH1",
            "LH2",
            "UH1",
            "UH2",
            };
#endif

        public PageSetting1()
        {
            InitializeComponent();

            if (Device.RuntimePlatform == Device.iOS)
            {
                this.Icon = new FileImageSource();
                this.Icon.File = "icons8-Settings-50-1-30x30.png";
            }

            switch (BleMvxApplication._config.BatteryLevelIndicatorFormat)
            {
                case 0:
                    buttonBatteryLevelFormat.Text = "Voltage";
                    break;

                default:
                    buttonBatteryLevelFormat.Text = "Percentage";
                    break;
            }

            if (BleMvxApplication._config.RFID_Region == CSLibrary.Constants.RegionCode.UNKNOWN)
                BleMvxApplication._config.RFID_Region = CSLibrary.Constants.RegionCode.FCC;

            Regions = BleMvxApplication._reader.rfid.GetActiveRegionCode();
            ActiveRegionsTextList = Regions.OfType<object>().Select(o => _regionsName[(int)o - 1]).ToArray();

            ActiveFrequencyList = BleMvxApplication._reader.rfid.GetAvailableFrequencyTable(BleMvxApplication._config.RFID_Region);
            ActiveFrequencyTextList = ActiveFrequencyList.OfType<object>().Select(o => o.ToString()).ToArray();

            entryInventoryDelayTime.Text = BleMvxApplication._config.RFID_InventoryDelayTime.ToString();
            entryDwellTime.Text = BleMvxApplication._config.RFID_DWellTime.ToString();
            entryPower.Text = BleMvxApplication._config.RFID_Power.ToString();
            entryTagPopulation.Text = BleMvxApplication._config.RFID_TagPopulation.ToString();
            buttonRegion.Text = _regionsName[(int)BleMvxApplication._config.RFID_Region - 1];
            switch (BleMvxApplication._config.RFID_FrequenceSwitch)
            {
                case 0:
                    buttonFrequencyOrder.Text = "Hopping";
                    break;
                case 1:
                    buttonFrequencyOrder.Text = "Fixed";
                    break;
                case 2:
                    buttonFrequencyOrder.Text = "Agile";
                    break;
            }
            buttonFixedChannel.Text = ActiveFrequencyTextList[BleMvxApplication._config.RFID_FixedChannel];

            checkbuttonFixedChannel();
        }

        protected override void OnAppearing()
        {
            if (BleMvxApplication._settingPage1TagPopulationChanged)
            {
                BleMvxApplication._settingPage1TagPopulationChanged = false;
                entryTagPopulation.Text = BleMvxApplication._config.RFID_TagPopulation.ToString();
            }

            base.OnAppearing();
        }

        public async void btnOKClicked(object sender, EventArgs e)
		{
            int cnt;

            Xamarin.Forms.DependencyService.Get<ISystemSound>().SystemSound(1);

            switch (buttonBatteryLevelFormat.Text)
            {
                case "Voltage":
                    BleMvxApplication._config.BatteryLevelIndicatorFormat = 0;
                    break;

                default:
                    BleMvxApplication._config.BatteryLevelIndicatorFormat = 1;
                    break;
            }

            BleMvxApplication._config.RFID_InventoryDelayTime = UInt16.Parse(entryInventoryDelayTime.Text);
			BleMvxApplication._config.RFID_DWellTime = UInt16.Parse(entryDwellTime.Text);
			BleMvxApplication._config.RFID_Power = UInt16.Parse(entryPower.Text);
            BleMvxApplication._config.RFID_TagPopulation = UInt16.Parse(entryTagPopulation.Text);


            for (cnt = 0; cnt < _regionsName.Length; cnt++)
            {
                if (_regionsName[cnt] == buttonRegion.Text)
                {
                    BleMvxApplication._config.RFID_Region = _regionsCode[cnt];
                    break;
                }
            }

            if (cnt == _regionsName.Length)
                BleMvxApplication._config.RFID_Region = CSLibrary.Constants.RegionCode.UNKNOWN;

            switch (buttonFrequencyOrder.Text)
            {
                case "Hopping":
                    BleMvxApplication._config.RFID_FrequenceSwitch = 0;
                    break;
                case "Fixed":
                    BleMvxApplication._config.RFID_FrequenceSwitch = 1;
                    break;
                case "Agile":
                    BleMvxApplication._config.RFID_FrequenceSwitch = 2;
                    break;
            }

            for (cnt = 0; cnt < ActiveFrequencyTextList.Length; cnt++)
            {
                if (buttonFixedChannel.Text == ActiveFrequencyTextList[cnt])
                {
                    BleMvxApplication._config.RFID_FixedChannel = (uint)cnt;
                    break;
                }
            }
            if (cnt == ActiveFrequencyTextList.Length)
                BleMvxApplication._config.RFID_FixedChannel = 0;

            BleMvxApplication._config.RFID_DynamicQParms.startQValue = BleMvxApplication._config.RFID_FixedQParms.qValue = (uint)(Math.Log((BleMvxApplication._config.RFID_TagPopulation * 2) , 2)) + 1;
            BleMvxApplication._settingPage3QvalueChanged = true;
            BleMvxApplication._settingPage4QvalueChanged = true;

            BleMvxApplication.SaveConfig();

            switch (BleMvxApplication._config.RFID_FrequenceSwitch)
            {
                case 0:
                    BleMvxApplication._reader.rfid.SetHoppingChannels(BleMvxApplication._config.RFID_Region);
                    break;
                case 1:
                    BleMvxApplication._reader.rfid.SetFixedChannel(BleMvxApplication._config.RFID_Region, BleMvxApplication._config.RFID_FixedChannel);
                    break;
                case 2:
                    BleMvxApplication._reader.rfid.SetAgileChannels(BleMvxApplication._config.RFID_Region);
                    break;
            }
        }

        public async void buttonRegionClicked(object sender, EventArgs e)
        {
            var answer = await DisplayActionSheet("Regions", "Cancel", null, ActiveRegionsTextList);

            if (answer != "Cancel")
            {
                int cnt;

                buttonRegion.Text = answer;

                for (cnt = 0; cnt < _regionsName.Length; cnt++)
                {
                    if (_regionsName[cnt] == answer)
                    {
                        ActiveFrequencyList = BleMvxApplication._reader.rfid.GetAvailableFrequencyTable(_regionsCode[cnt]);
                        break;
                    }
                }
                if (cnt == _regionsName.Length)
                    ActiveFrequencyList = new double[1] { 0.0 };

                ActiveFrequencyTextList = ActiveFrequencyList.OfType<object>().Select(o => o.ToString()).ToArray();
                buttonFixedChannel.Text = ActiveFrequencyTextList[0];
            }
        }

        public async void buttonFrequencyOrderClicked(object sender, EventArgs e)
        {
            string answer;

            if (BleMvxApplication._reader.rfid.IsHoppingChannelOnly)
                answer = await DisplayActionSheet("Frequence Channel Order", "Cancel", null, "Hopping");
            else if (BleMvxApplication._reader.rfid.IsFixedChannelOnly)
                answer = await DisplayActionSheet("Frequence Channel Order", "Cancel", null, "Fixed", "Agile");
            else
                answer = await DisplayActionSheet("Frequence Channel Order", "Cancel", null, "Hopping", "Fixed");

            if (answer != "Cancel")
                buttonFrequencyOrder.Text = answer;

            checkbuttonFixedChannel();
        }

        public async void buttonFixedChannelClicked(object sender, EventArgs e)
        {
            var answer = await DisplayActionSheet("Frequence Channel Order", "Cancel", null, ActiveFrequencyTextList);

            if (answer != "Cancel")
                buttonFixedChannel.Text = answer;
        }

        public async void buttonBatteryLevelFormatClicked(object sender, EventArgs e)
        {
            var answer = await DisplayActionSheet("Frequence Channel Order", "Cancel", null, "Voltage", "Percentage");

            if (answer != "Cancel")
                buttonBatteryLevelFormat.Text = answer;
        }

        void checkbuttonFixedChannel()
        {
            if (buttonFrequencyOrder.Text == "Fixed")
                buttonFixedChannel.IsEnabled = true;
            else
                buttonFixedChannel.IsEnabled = false;
        }

        public async void entryDwellTimeCompleted(object sender, EventArgs e)
        {
            try 
            {
                uint DwellTime = uint.Parse(entryDwellTime.Text);
                if (DwellTime == 0 && BleMvxApplication._config.RFID_OperationMode == CSLibrary.Constants.RadioOperationMode.NONCONTINUOUS)
                {
                    entryDwellTime.Text = "2000";
                    await DisplayAlert("Can not set to 0 when in non-continuous mode", "", "OK");
                    return;
                }

                entryDwellTime.Text = DwellTime.ToString();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Value not accept", "", "OK");
                entryDwellTime.Text = "2000";
            }

        }

    }
}
