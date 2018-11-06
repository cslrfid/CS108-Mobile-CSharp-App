using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BLE.Client.Pages
{
	public partial class PageSetting3
	{
        public PageSetting3()
        {
            InitializeComponent();

            if (Device.RuntimePlatform == Device.iOS)
            {
                this.Icon = new FileImageSource();
                this.Icon.File = "icons8-Settings-50-3-30x30.png";
            }

            entryStartQ.Text = BleMvxApplication._config.RFID_DynamicQParms.startQValue.ToString();
            entryMinQ.Text = BleMvxApplication._config.RFID_DynamicQParms.minQValue.ToString();
            entryMaxQ.Text = BleMvxApplication._config.RFID_DynamicQParms.maxQValue.ToString();
            entryQueryRep.Text = BleMvxApplication._config.RFID_DynamicQParms.thresholdMultiplier.ToString();
            entryRetry.Text = BleMvxApplication._config.RFID_DynamicQParms.retryCount.ToString();
        }

        protected override void OnAppearing()
        {
            if (BleMvxApplication._settingPage3QvalueChanged)
            {
                BleMvxApplication._settingPage3QvalueChanged = false;
                entryStartQ.Text = BleMvxApplication._config.RFID_DynamicQParms.startQValue.ToString();
            }

            base.OnAppearing();
        }

        public async void btnOKClicked(object sender, EventArgs e)
        {
            Xamarin.Forms.DependencyService.Get<ISystemSound>().SystemSound(1);

            BleMvxApplication._config.RFID_DynamicQParms.startQValue = uint.Parse (entryStartQ.Text);
			BleMvxApplication._config.RFID_DynamicQParms.minQValue = uint.Parse(entryMinQ.Text);
			BleMvxApplication._config.RFID_DynamicQParms.maxQValue = uint.Parse(entryMaxQ.Text);
			BleMvxApplication._config.RFID_DynamicQParms.thresholdMultiplier = uint.Parse(entryQueryRep.Text);
			BleMvxApplication._config.RFID_DynamicQParms.retryCount = uint.Parse(entryRetry.Text);

            BleMvxApplication._config.RFID_FixedQParms.qValue = BleMvxApplication._config.RFID_DynamicQParms.startQValue;
            BleMvxApplication._config.RFID_TagPopulation = ((uint)1 << (int)BleMvxApplication._config.RFID_DynamicQParms.startQValue);

            BleMvxApplication._settingPage1TagPopulationChanged = true;
            BleMvxApplication._settingPage4QvalueChanged = true;

            BleMvxApplication.SaveConfig();
        }
    }
}
