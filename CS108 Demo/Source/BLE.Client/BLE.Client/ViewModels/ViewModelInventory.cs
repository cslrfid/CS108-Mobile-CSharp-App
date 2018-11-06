using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Acr.UserDialogs;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;

using System.Windows.Input;
using Xamarin.Forms;


using Plugin.BLE.Abstractions.Contracts;

using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Extensions;

namespace BLE.Client.ViewModels
{
    public class ViewModelInventory : BaseViewModel
    {
        private readonly IUserDialogs _userDialogs;

        public ICommand OnStartInventoryButtonCommand { protected set; get; }
        public ICommand OnClearButtonCommand { protected set; get; }

		public List<CSLibrary.Structures.TagCallbackInfo> EPCData = new List<CSLibrary.Structures.TagCallbackInfo>();
		public ObservableCollection<string[]> TagData { get; } = new ObservableCollection<string[]>();

		public int tagsCount = 0;

		public bool _startInventory = true;

        private string _startInventoryButtonText = "Start Inventory";
        public string startInventoryButtonText { get { return _startInventoryButtonText; } }

        bool _tagCount = false;

        private string _tagPerSecondText = "0 tags/s";
        public string tagPerSecondText { get { return _tagPerSecondText; } }
		private string _numberOfTagsText = "0 tags";
		public string numberOfTagsText { get { return _numberOfTagsText; } }


		public ViewModelInventory(IAdapter adapter, IUserDialogs userDialogs) : base(adapter)
        {
            _userDialogs = userDialogs;

            OnStartInventoryButtonCommand = new Command(StartInventoryClick);
            OnClearButtonCommand = new Command(ClearClick);
        }

        public override void Resume()
        {
            base.Resume();
            BleMvxApplication._reader.rfid.OnAsyncCallback += new EventHandler<CSLibrary.Events.OnAsyncCallbackEventArgs>(TagInventoryEvent);
        }

        public override void Suspend()
        {
            BleMvxApplication._reader.rfid.OnAsyncCallback -= new EventHandler<CSLibrary.Events.OnAsyncCallbackEventArgs>(TagInventoryEvent);
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

                StartTagCount();
				if (BleMvxApplication._config.RFID_OperationMode == CSLibrary.Constants.RadioOperationMode.CONTINUOUS)
				{
					_startInventory = false;
					_startInventoryButtonText = "Stop Inventory";
				}

				// Setting 1
				BleMvxApplication._reader.rfid.SetInventoryTimeDelay((uint)BleMvxApplication._config.RFID_InventoryDelayTime);
				BleMvxApplication._reader.rfid.SetInventoryDuration((uint)BleMvxApplication._config.RFID_DWellTime);
				BleMvxApplication._reader.rfid.SetPowerLevel((uint)BleMvxApplication._config.RFID_Power);

				// Setting 2
				BleMvxApplication._reader.rfid.SetOperationMode(BleMvxApplication._config.RFID_OperationMode);
				BleMvxApplication._reader.rfid.SetTagGroup(BleMvxApplication._config.RFID_TagGroup);
				BleMvxApplication._reader.rfid.SetCurrentSingulationAlgorithm(BleMvxApplication._config.RFID_Algorithm);
				BleMvxApplication._reader.rfid.SetCurrentLinkProfile(BleMvxApplication._config.RFID_Profile);

				// Setting 3
				BleMvxApplication._reader.rfid.SetDynamicQParms(BleMvxApplication._config.RFID_DynamicQParms);

				// Setting 4
				BleMvxApplication._reader.rfid.SetFixedQParms(BleMvxApplication._config.RFID_FixedQParms);

				// Select Criteria filter
				/*
				BleMvxApplication._reader.rfid.Options.TagSelected.epcMask = new CSLibrary.Structures.S_MASK("709999");

				CSLibrary.Structures.SelectCriterion[] critlist = new CSLibrary.Structures.SelectCriterion[1];
				critlist[0] = new CSLibrary.Structures.SelectCriterion();
				critlist[0].mask = new CSLibrary.Structures.SelectMask(CSLibrary.Constants.MemoryBank.EPC, 0x20, 24, BleMvxApplication._reader.rfid.Options.TagSelected.epcMask.ToBytes());
				critlist[0].action = new CSLibrary.Structures.SelectAction(CSLibrary.Constants.Target.SELECTED, CSLibrary.Constants.Action.ASLINVA_DSLINVB, 0);

				BleMvxApplication._reader.rfid.SetSelectCriteria(critlist);
				*/

				// Post Match Criteria filter
				BleMvxApplication._reader.rfid.Options.TagSelected.epcMask = new CSLibrary.Structures.S_MASK(BleMvxApplication._config.MASK_EPC);

				CSLibrary.Structures.SingulationCriterion[] sel = new CSLibrary.Structures.SingulationCriterion[1];
				sel[0] = new CSLibrary.Structures.SingulationCriterion();
				sel[0].match = BleMvxApplication._config.MASK_Enable ? 0U : 1U;
				sel[0].mask = new CSLibrary.Structures.SingulationMask(BleMvxApplication._config.MASK_Offset, (uint)(BleMvxApplication._config.MASK_EPC.Length * 4), BleMvxApplication._reader.rfid.Options.TagSelected.epcMask.ToBytes());
				BleMvxApplication._reader.rfid.SetPostMatchCriteria(sel);

				// Start Inventory
				BleMvxApplication._reader.rfid.StartOperation(CSLibrary.Constants.Operation.TAG_RANGING);
            }
            else
            {
                _startInventory = true;
                _startInventoryButtonText = "Start Inventory";

                _tagCount = false;
                BleMvxApplication._reader.rfid.StopOperation();
            }

            RaisePropertyChanged(() => startInventoryButtonText);
        }

        void StartTagCount()
        {
            tagsCount = 0;
            _tagCount = true;

            // Create a timer that waits one second, then invokes every second.
            Xamarin.Forms.Device.StartTimer(TimeSpan.FromMilliseconds(1000), () =>
            {
				_numberOfTagsText = TagData.Count.ToString () + " tags";
				RaisePropertyChanged(() => numberOfTagsText);

				_tagPerSecondText = tagsCount.ToString() + " tags/s";
                RaisePropertyChanged(() => tagPerSecondText);
                tagsCount = 0;

                if (_tagCount)
                    return true;

                return false;
            });
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
            tagsCount++;
            AddOrUpdateTagData(e.info);
        }

        private void AddOrUpdateTagData(CSLibrary.Structures.TagCallbackInfo info)
        {
            InvokeOnMainThread(() =>
            {
                //string epc = CSLibrary.Tools.Hex.ToString(e.info.epc);
                //TagData.Add(epc);
                //Trace.Message("EPC = {0}", epc);

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

				/*
				foreach (CSLibrary.Events.TagCallbackInfo item in EPCData)
                {
                    int cnt = 0;

                    for (; cnt < info.epc.Length; cnt ++)
                    {
                        if (item.epc[cnt] != info.epc[cnt])
                            break;
                    }

                    if (cnt == info.epc.Length)
                    {
						item.rssi = info.rssi;
						TagData[cnt][1] = item.rssi.ToString();
						found = true;
                        break;
                    }
                }

				if (!found)
                {
                    string epcString = CSLibrary.Tools.Hex.ToString(info.epc);

                    EPCData.Add(info);
					//TagData.Add(CSLibrary.Tools.Hex.ToString(info.epc));

					string[] newItem = new string[] {epcString, info.rssi.ToString () };

					TagData.Add(newItem);
					Trace.Message("EPC Data = {0}", epcString);
                }
				*/

				//RaisePropertyChanged();
                //RaisePropertyChanged(() => TagData);
				//RaisePropertyChanged(() => TagData[1]);
			});
        }
    }
}
