using System;
using System.Collections.Generic;
using Acr.UserDialogs;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using Plugin.BLE.Abstractions.Contracts;

using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace BLE.Client.ViewModels
{
    public class ViewModelRFMicroSetting : BaseViewModel
    {
        private readonly IUserDialogs _userDialogs;

        public string buttonSensorTypeText { get; set; }
        public string entryMinOCRSSIText { get; set; }
        public string entryMaxOCRSSIText { get; set; }
        public ICommand OnOKButtonCommand { protected set; get; }

        public ViewModelRFMicroSetting(IAdapter adapter, IUserDialogs userDialogs) : base(adapter)
        {
            _userDialogs = userDialogs;

            OnOKButtonCommand = new Command(OnOKButtonClicked);
        }

        public override void Resume()
        {
            base.Resume();
        }

        public override void Suspend()
        {
            base.Suspend();
        }

        protected override void InitFromBundle(IMvxBundle parameters)
        {
            base.InitFromBundle(parameters);
        }

        void OnOKButtonClicked()
        {
            RaisePropertyChanged(() => buttonSensorTypeText);
            RaisePropertyChanged(() => entryMinOCRSSIText);
            RaisePropertyChanged(() => entryMaxOCRSSIText);

            BleMvxApplication._sensorValueType = buttonSensorTypeText == "Sensor Code" ? 0 : 1;
            BleMvxApplication._minOCRSSI = uint.Parse(entryMinOCRSSIText);
            BleMvxApplication._maxOCRSSI = uint.Parse(entryMaxOCRSSIText);

            ShowViewModel<ViewModelRFMicroInventory>(new MvxBundle());
        }
    }
}
