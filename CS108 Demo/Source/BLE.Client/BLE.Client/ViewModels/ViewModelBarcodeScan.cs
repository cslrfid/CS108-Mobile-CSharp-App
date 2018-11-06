using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Acr.UserDialogs;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;

using System.Windows.Input;
using Xamarin.Forms;


using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;

using Plugin.BLE.Abstractions.Extensions;

namespace BLE.Client.ViewModels
{
    public class ViewModelBarcodeScan : BaseViewModel
    {
        private readonly IUserDialogs _userDialogs;

        public ICommand OnStartInventoryButtonCommand { protected set; get; }
        public ICommand OnClearButtonCommand { protected set; get; }

        public List<CSLibrary.Structures.TagCallbackInfo> EPCData = new List<CSLibrary.Structures.TagCallbackInfo>();
        public ObservableCollection<string[]> TagData { get; } = new ObservableCollection<string[]>();
        //public ObservableCollection<CSLibrary.Events.TagCallbackInfo> EPCData { get; } = new ObservableCollection<CSLibrary.Events.TagCallbackInfo>();

        public int tagsCount = 0;

        public bool _startInventory = true;

        private string _startInventoryButtonText = "Start Scan";
        public string startInventoryButtonText { get { return _startInventoryButtonText; } }


        public ViewModelBarcodeScan(IAdapter adapter, IUserDialogs userDialogs) : base(adapter)
        {
            _userDialogs = userDialogs;

            OnStartInventoryButtonCommand = new Command(StartInventoryClick);
            OnClearButtonCommand = new Command(ClearClick);
        }

        public override void Resume()
        {
            base.Resume();
            //BleMvxApplication._reader.barcode.m_captureCompleted += new EventHandler<CSLibrary.Events.OnAsyncCallbackEventArgs>(TagInventoryEvent);
        }

        public override void Suspend()
        {
            //BleMvxApplication._reader.rfid.OnAsyncCallback -= new EventHandler<CSLibrary.Events.OnAsyncCallbackEventArgs>(TagInventoryEvent);
            base.Suspend();
        }

        protected override void InitFromBundle(IMvxBundle parameters)
        {
            base.InitFromBundle(parameters);
        }

        private void ClearClick()
        {
            EPCData.Clear();
            TagData.Clear();
        }

        void StartInventoryClick()
        {
            if (_startInventory)
            {
                EPCData.Clear();
                TagData.Clear();

                _startInventoryButtonText = "Stop Scan";

                BleMvxApplication._reader.BARCODEPowerOn();
            }
            else
            {
                _startInventory = true;
                _startInventoryButtonText = "Start Scan";

                BleMvxApplication._reader.BARCODEPowerOff();
            }

            RaisePropertyChanged(() => startInventoryButtonText);
        }

        void StopInventoryClick()
        {
            BleMvxApplication._reader.rfid.StopOperation();
        }

        void OnItemSelected(object sender, CSLibrary.Events.OnAsyncCallbackEventArgs e)
        {
            var a = 10;
        }

        void TagInventoryEvent(object sender, CSLibrary.Events.OnAsyncCallbackEventArgs e)
        {
            AddOrUpdateTagData(e.info);
        }

        private void AddOrUpdateTagData(CSLibrary.Structures.TagCallbackInfo info)
        {
            InvokeOnMainThread(() =>
            {
                bool found = false;

                int cnt;

                for (cnt = 0; cnt < EPCData.Count; cnt++)
                {
                    if (EPCData[cnt].epc.ToString() == info.epc.ToString())
                    {
                        EPCData[cnt].rssi = info.rssi;
                        TagData[cnt][1] = info.rssi.ToString();
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    string epcString = info.epc.ToString();

                    EPCData.Add(info);
                    //TagData.Add(CSLibrary.Tools.Hex.ToString(info.epc));

                    string[] newItem = new string[] { epcString, info.rssi.ToString() };

                    TagData.Add(newItem);

                    Trace.Message("EPC Data = {0}", epcString);
                }
            });
        }
    }
}
