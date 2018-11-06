using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BLE.Client.Pages
{
    public partial class PageSetting4 : BasePage
    {
		public PageSetting4()
		{
			InitializeComponent();

            if (Device.RuntimePlatform == Device.iOS)
            {
                this.Icon = new FileImageSource();
                this.Icon.File = "icons8-Settings-50-4-30x30.png";
            }

            entryQValue.Text = BleMvxApplication._config.RFID_FixedQParms.qValue.ToString ();
			entryRetry.Text = BleMvxApplication._config.RFID_FixedQParms.retryCount.ToString();
			entryRepeat.Text = BleMvxApplication._config.RFID_FixedQParms.repeatUntilNoTags == 1 ? "Repeat": "Not Repeat";
		}

        protected override void OnAppearing()
        {
            if (BleMvxApplication._settingPage4QvalueChanged)
            {
                BleMvxApplication._settingPage4QvalueChanged = false;
                entryQValue.Text = BleMvxApplication._config.RFID_FixedQParms.qValue.ToString();
            }

            base.OnAppearing();
        }

        public async void entryRepeatFocused(object sender, EventArgs e)
		{
			var answer = await DisplayAlert("Toggle Mode", "", "Repeat", "Not Repeat");
			entryRepeat.Text = answer ? "Repeat" : "Not Repeat";
		}

        public async void btnOKClicked(object sender, EventArgs e)
        {
            Xamarin.Forms.DependencyService.Get<ISystemSound>().SystemSound(1);

            BleMvxApplication._config.RFID_FixedQParms.qValue = uint.Parse(entryQValue.Text);
			BleMvxApplication._config.RFID_FixedQParms.retryCount = uint.Parse(entryRetry.Text);
			BleMvxApplication._config.RFID_FixedQParms.repeatUntilNoTags = (uint)(entryRepeat.Text == "Repeat" ? 1 : 0);

            BleMvxApplication._config.RFID_DynamicQParms.startQValue = BleMvxApplication._config.RFID_FixedQParms.qValue;
            BleMvxApplication._config.RFID_TagPopulation = ((uint)1 << (int)BleMvxApplication._config.RFID_FixedQParms.qValue);

            BleMvxApplication._settingPage1TagPopulationChanged = true;
            BleMvxApplication._settingPage3QvalueChanged = true;

            BleMvxApplication.SaveConfig();
        }
    }
}
