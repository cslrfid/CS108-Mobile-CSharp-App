using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BLE.Client.Pages
{
    public partial class PageSettingShortcut : BasePage
    {
        public PageSettingShortcut()
        {
            InitializeComponent();

            if (Device.RuntimePlatform == Device.iOS)
            {
                this.Icon = new FileImageSource();
                this.Icon.File = "icons8-Settings-50-5-30x30.png";
            }

            F1.Text = BleMvxApplication._config.RFID_Shortcut[0].Function.ToString();
            F1MinTime.Text = BleMvxApplication._config.RFID_Shortcut[0].DurationMin.ToString();
            F1MaxTime.Text = BleMvxApplication._config.RFID_Shortcut[0].DurationMax.ToString();
            F2.Text = BleMvxApplication._config.RFID_Shortcut[1].Function.ToString();
            F2MinTime.Text = BleMvxApplication._config.RFID_Shortcut[1].DurationMin.ToString();
            F2MaxTime.Text = BleMvxApplication._config.RFID_Shortcut[1].DurationMax.ToString();

            switchInventoryAlertSound.IsToggled = BleMvxApplication._config.RFID_InventoryAlertSound;
            /*
                        F3.Text = BleMvxApplication._config.RFID_Shortcut[2].Function.ToString();
                        F3MinTime.Text = BleMvxApplication._config.RFID_Shortcut[2].DurationMin.ToString();
                        F3MaxTime.Text = BleMvxApplication._config.RFID_Shortcut[2].DurationMax.ToString();
                        F4.Text = BleMvxApplication._config.RFID_Shortcut[3].Function.ToString();
                        F4MinTime.Text = BleMvxApplication._config.RFID_Shortcut[3].DurationMin.ToString();
                        F4MaxTime.Text = BleMvxApplication._config.RFID_Shortcut[3].DurationMax.ToString();
                        F5.Text = BleMvxApplication._config.RFID_Shortcut[4].Function.ToString();
                        F5MinTime.Text = BleMvxApplication._config.RFID_Shortcut[4].DurationMin.ToString();
                        F5MaxTime.Text = BleMvxApplication._config.RFID_Shortcut[4].DurationMax.ToString();
                        F6.Text = BleMvxApplication._config.RFID_Shortcut[5].Function.ToString();
                        F6MinTime.Text = BleMvxApplication._config.RFID_Shortcut[5].DurationMin.ToString();
                        F6MaxTime.Text = BleMvxApplication._config.RFID_Shortcut[5].DurationMax.ToString();
            */
        }

        public async void btnFunctionSelectedClicked(object sender, EventArgs e)
        {
            var answer = await DisplayActionSheet(null, BLE.Client.CONFIG.MAINMENUSHORTCUT.FUNCTION.NONE.ToString(), null, BLE.Client.CONFIG.MAINMENUSHORTCUT.FUNCTION.INVENTORY.ToString(), BLE.Client.CONFIG.MAINMENUSHORTCUT.FUNCTION.BARCODE.ToString());

            Button b = (Button)sender;

            b.Text = answer;
        }

        public async void btnSaveClicked(object sender, EventArgs e)
        {
            Xamarin.Forms.DependencyService.Get<ISystemSound>().SystemSound(1);

            BleMvxApplication._config.RFID_Shortcut[0].Function = (CONFIG.MAINMENUSHORTCUT.FUNCTION)Enum.Parse(typeof(CONFIG.MAINMENUSHORTCUT.FUNCTION), F1.Text);
            BleMvxApplication._config.RFID_Shortcut[0].DurationMin = uint.Parse(F1MinTime.Text);
            BleMvxApplication._config.RFID_Shortcut[0].DurationMax = uint.Parse(F1MaxTime.Text);
            BleMvxApplication._config.RFID_Shortcut[1].Function = (CONFIG.MAINMENUSHORTCUT.FUNCTION)Enum.Parse(typeof(CONFIG.MAINMENUSHORTCUT.FUNCTION), F2.Text);
            BleMvxApplication._config.RFID_Shortcut[1].DurationMin = uint.Parse(F2MinTime.Text);
            BleMvxApplication._config.RFID_Shortcut[1].DurationMax = uint.Parse(F2MaxTime.Text);

            BleMvxApplication._config.RFID_InventoryAlertSound = switchInventoryAlertSound.IsToggled;

            /*
                        BleMvxApplication._config.RFID_Shortcut[2].Function = (CONFIG.MAINMENUSHORTCUT.FUNCTION)Enum.Parse(typeof(CONFIG.MAINMENUSHORTCUT.FUNCTION), F3.Text);
                        BleMvxApplication._config.RFID_Shortcut[2].DurationMin = uint.Parse(F3MinTime.Text);
                        BleMvxApplication._config.RFID_Shortcut[2].DurationMax = uint.Parse(F3MaxTime.Text);
                        BleMvxApplication._config.RFID_Shortcut[3].Function = (CONFIG.MAINMENUSHORTCUT.FUNCTION)Enum.Parse(typeof(CONFIG.MAINMENUSHORTCUT.FUNCTION), F4.Text);
                        BleMvxApplication._config.RFID_Shortcut[3].DurationMin = uint.Parse(F4MinTime.Text);
                        BleMvxApplication._config.RFID_Shortcut[3].DurationMax = uint.Parse(F4MaxTime.Text);
                        BleMvxApplication._config.RFID_Shortcut[4].Function = (CONFIG.MAINMENUSHORTCUT.FUNCTION)Enum.Parse(typeof(CONFIG.MAINMENUSHORTCUT.FUNCTION), F5.Text);
                        BleMvxApplication._config.RFID_Shortcut[4].DurationMin = uint.Parse(F5MinTime.Text);
                        BleMvxApplication._config.RFID_Shortcut[4].DurationMax = uint.Parse(F5MaxTime.Text);
                        BleMvxApplication._config.RFID_Shortcut[5].Function = (CONFIG.MAINMENUSHORTCUT.FUNCTION)Enum.Parse(typeof(CONFIG.MAINMENUSHORTCUT.FUNCTION), F6.Text);
                        BleMvxApplication._config.RFID_Shortcut[5].DurationMin = uint.Parse(F6MinTime.Text);
                        BleMvxApplication._config.RFID_Shortcut[5].DurationMax = uint.Parse(F6MaxTime.Text);
            */

            BleMvxApplication.SaveConfig();
        }

        public async void btnClearClicked(object sender, EventArgs e)
        {
            for (int cnt = 0; cnt < BleMvxApplication._config.RFID_Shortcut.Length; cnt++)
            {
                BleMvxApplication._config.RFID_Shortcut[cnt].Function = CONFIG.MAINMENUSHORTCUT.FUNCTION.NONE;
                BleMvxApplication._config.RFID_Shortcut[cnt].DurationMin = 0;
                BleMvxApplication._config.RFID_Shortcut[cnt].DurationMax = 0;
            }
        }

        public async void btnBarcodeResetClicked(object sender, EventArgs e)
        {
            BleMvxApplication._reader.barcode.FactoryReset();
        }

        public async void btnConfigResetClicked(object sender, EventArgs e)
        {
            BleMvxApplication.ResetConfig();
            BleMvxApplication._reader.rfid.SetDefaultChannel();

            BleMvxApplication._config.RFID_Region = BleMvxApplication._reader.rfid.SelectedRegionCode;

            if (BleMvxApplication._reader.rfid.IsFixedChannel)
            {
                BleMvxApplication._config.RFID_FrequenceSwitch = 1;
                BleMvxApplication._config.RFID_FixedChannel = BleMvxApplication._reader.rfid.SelectedChannel;
            }
            else
            {
                BleMvxApplication._config.RFID_FrequenceSwitch = 0; // Hopping
            }

            BleMvxApplication.SaveConfig();
        }
    }
}


#if backup
        <StackLayout Orientation="Horizontal">
            <Button x:Name="F3"
                Font="Large"
			    HorizontalOptions="FillAndExpand"
			    Clicked="btnFunctionSelectedClicked"
                WidthRequest="130"
            />

            <Entry
                x:Name="F3MinTime"
			    HorizontalOptions="FillAndExpand" 
			    Keyboard="Text"
                WidthRequest="90"
            />

            <Entry
                x:Name="F3MaxTime"
			    HorizontalOptions="FillAndExpand" 
			    Keyboard="Text"
                WidthRequest="90"
		    />
        </StackLayout>

        <StackLayout Orientation="Horizontal">
            <Button x:Name="F4"
                Font="Large"
			    HorizontalOptions="FillAndExpand"
			    Clicked="btnFunctionSelectedClicked"
                WidthRequest="130"
            />

            <Entry
                x:Name="F4MinTime"
			    HorizontalOptions="FillAndExpand" 
			    Keyboard="Text"
                WidthRequest="90"
            />

            <Entry
                x:Name="F4MaxTime"
			    HorizontalOptions="FillAndExpand" 
			    Keyboard="Text"
                WidthRequest="90"
		    />
        </StackLayout>

        <StackLayout Orientation="Horizontal">
            <Button x:Name="F5"
                Font="Large"
			    HorizontalOptions="FillAndExpand"
			    Clicked="btnFunctionSelectedClicked"
                WidthRequest="130"
            />

            <Entry
                x:Name="F5MinTime"
			    HorizontalOptions="FillAndExpand" 
			    Keyboard="Text"
                WidthRequest="90"
            />

            <Entry
                x:Name="F5MaxTime"
			    HorizontalOptions="FillAndExpand" 
			    Keyboard="Text"
                WidthRequest="90"
		    />
        </StackLayout>

        <StackLayout Orientation="Horizontal">
            <Button x:Name="F6"
                Font="Large"
			    HorizontalOptions="FillAndExpand"
			    Clicked="btnFunctionSelectedClicked"
                WidthRequest="130"
            />

            <Entry
                x:Name="F6MinTime"
			    HorizontalOptions="FillAndExpand" 
			    Keyboard="Text"
                WidthRequest="90"
            />

            <Entry
                x:Name="F6MaxTime"
			    HorizontalOptions="FillAndExpand" 
			    Keyboard="Text"
                WidthRequest="90"
		    />
        </StackLayout>
#endif