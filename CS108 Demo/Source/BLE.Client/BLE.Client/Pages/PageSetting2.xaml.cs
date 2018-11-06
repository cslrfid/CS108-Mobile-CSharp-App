using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BLE.Client.Pages
{
	public partial class PageSetting2
	{
		public PageSetting2()
		{
			InitializeComponent();

            if (Device.RuntimePlatform == Device.iOS)
            {
                this.Icon = new FileImageSource();
                this.Icon.File = "icons8-Settings-50-2-30x30.png";
            }

            buttonOperationMode.Text = BleMvxApplication._config.RFID_OperationMode.ToString();
			buttonSelected.Text = BleMvxApplication._config.RFID_TagGroup.selected.ToString();
            buttonSession.Text = BleMvxApplication._config.RFID_TagGroup.session.ToString();
            if (BleMvxApplication._config.RFID_DynamicQParms.toggleTarget != 0)
            {
                buttonTarget.Text = "Toggle A/B";
            }
            else
            {
                buttonTarget.Text = BleMvxApplication._config.RFID_TagGroup.target.ToString();
            }
            buttonAlgorithm.Text = BleMvxApplication._config.RFID_Algorithm.ToString();
            buttonProfile.Text = BleMvxApplication._config.RFID_Profile.ToString();
		}

		public async void buttonOperationModeClicked(object sender, EventArgs e)
		{
			var answer = await DisplayAlert("Operation Mode", "", "CONTINUOUS", "NON-CONTINUOUS");
            buttonOperationMode.Text = answer ? "CONTINUOUS" : "NONCONTINUOUS";
		}

		public async void buttonSelectedClicked(object sender, EventArgs e)
		{
			var answer = await DisplayActionSheet("Selected", "Cancel", null, "ALL", "ASSERTED", "DEASSERTED");

            if (answer != "Cancel")
				buttonSelected.Text = answer;
		}

		public async void buttonSessionClicked(object sender, EventArgs e)
		{
			var answer = await DisplayActionSheet("Session", "Cancel", null, "S0", "S1", "S2", "S3"); // S2 S3

			if (answer != "Cancel")
                buttonSession.Text = answer;
		}

        public async void buttonTargetClicked(object sender, EventArgs e)
        {
            var answer = await DisplayActionSheet(null, "Cancel", null, "A", "B", "Toggle A/B");

            if (answer != "Cancel")
                buttonTarget.Text = answer;
        }

        public async void buttonAlgorithmClicked(object sender, EventArgs e)
		{
			var answer = await DisplayAlert("Algorithm", "", "DYNAMICQ", "FIXEDQ");
            buttonAlgorithm.Text = answer ? "DYNAMICQ" : "FIXEDQ";
		}

        public async void buttonProfileClicked(object sender, EventArgs e)
        {
            var answer = await DisplayActionSheet(null, "Cancel", null, "0", "1", "2", "3");

            if (answer != "Cancel")
                buttonProfile.Text = answer;
        }

        public async void btnOKClicked(object sender, EventArgs e)
        {
            Xamarin.Forms.DependencyService.Get<ISystemSound>().SystemSound(1);

            if (BleMvxApplication._config.RFID_OperationMode == CSLibrary.Constants.RadioOperationMode.CONTINUOUS && buttonOperationMode.Text != "CONTINUOUS")
                BleMvxApplication._config.RFID_DWellTime = 2000;

            BleMvxApplication._config.RFID_OperationMode = buttonOperationMode.Text == "CONTINUOUS" ? CSLibrary.Constants.RadioOperationMode.CONTINUOUS : CSLibrary.Constants.RadioOperationMode.NONCONTINUOUS;

            switch (buttonSelected.Text)
			{
				case "ALL":
					BleMvxApplication._config.RFID_TagGroup.selected = CSLibrary.Constants.Selected.ALL;
					break;

				case "ASSERTED":
					BleMvxApplication._config.RFID_TagGroup.selected = CSLibrary.Constants.Selected.ASSERTED;
					break;

				case "DEASSERTED":
					BleMvxApplication._config.RFID_TagGroup.selected = CSLibrary.Constants.Selected.DEASSERTED;
					break;
			}

			switch (buttonSession.Text)
			{
				case "S0":
					BleMvxApplication._config.RFID_TagGroup.session = CSLibrary.Constants.Session.S0;
					break;

				case "S1":
					BleMvxApplication._config.RFID_TagGroup.session = CSLibrary.Constants.Session.S1;
					break;

				case "S2":
					BleMvxApplication._config.RFID_TagGroup.session = CSLibrary.Constants.Session.S2;
					break;

				case "S3":
					BleMvxApplication._config.RFID_TagGroup.session = CSLibrary.Constants.Session.S3;
					break;
			}

            switch (buttonTarget.Text)
            {
                case "A":
                    BleMvxApplication._config.RFID_TagGroup.target = CSLibrary.Constants.SessionTarget.A;
                    BleMvxApplication._config.RFID_FixedQParms.toggleTarget = 0;
                    BleMvxApplication._config.RFID_DynamicQParms.toggleTarget = 0;
                    break;
                case "B":
                    BleMvxApplication._config.RFID_TagGroup.target = CSLibrary.Constants.SessionTarget.B;
                    BleMvxApplication._config.RFID_FixedQParms.toggleTarget = 0;
                    BleMvxApplication._config.RFID_DynamicQParms.toggleTarget = 0;
                    break;
                default:
                    BleMvxApplication._config.RFID_DynamicQParms.toggleTarget = 1;
                    BleMvxApplication._config.RFID_FixedQParms.toggleTarget = 1;
                    break;
            }

			if (buttonAlgorithm.Text == "DYNAMICQ")
			{
				BleMvxApplication._config.RFID_Algorithm = CSLibrary.Constants.SingulationAlgorithm.DYNAMICQ;  
			}
			else
			{
				BleMvxApplication._config.RFID_Algorithm = CSLibrary.Constants.SingulationAlgorithm.FIXEDQ;
			}
			BleMvxApplication._config.RFID_Profile = UInt16.Parse(buttonProfile.Text);

            BleMvxApplication.SaveConfig();
        }
    }
}
