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
    public class ViewModelUCODEDNA : BaseViewModel
    {
        private readonly IUserDialogs _userDialogs;

        public string entrySelectedEPC { get; set; }
        public string entrySelectedPWD { get; set; }
        public string entrySelectedKey0 { get; set; }       // 128 bits
        public string entrySelectedKey1 { get; set; }       // 16 bits

        public ICommand OnReadKeyButtonCommand { protected set; get; }
        public ICommand OnRandomKeyButtonCommand { protected set; get; }
        public ICommand OnWriteKeyButtonCommand { protected set; get; }
        public ICommand OnActivateKeyButtonCommand { protected set; get; }
        public ICommand OnUntraceableButtonCommand { protected set; get; }
        public ICommand OnAuthenticateButtonCommand { protected set; get; }

        uint accessPwd;

        public ViewModelUCODEDNA(IAdapter adapter, IUserDialogs userDialogs) : base(adapter)
        {
            _userDialogs = userDialogs;

            OnReadKeyButtonCommand = new Command(OnReadKeyButtonButtonClick);
            OnRandomKeyButtonCommand = new Command(OnRandomKeyButtonButtonClick);
            OnWriteKeyButtonCommand = new Command(OnWriteKeyButtonButtonClick);
            OnActivateKeyButtonCommand = new Command(OnActivateKeyButtonButtonClick);
            OnUntraceableButtonCommand = new Command(OnUntraceableButtonButtonClick);
            OnAuthenticateButtonCommand = new Command(OnAuthenticateButtonButtonClick);
        }

        public override void Resume()
        {
            base.Resume();
            //BleMvxApplication._reader.rfid.OnAccessCompleted += new EventHandler<CSLibrary.Events.OnAccessCompletedEventArgs>(TagCompletedEvent);
        }

        public override void Suspend()
        {
            //BleMvxApplication._reader.rfid.OnAccessCompleted -= new EventHandler<CSLibrary.Events.OnAccessCompletedEventArgs>(TagCompletedEvent);
            base.Suspend();
        }

        protected override void InitFromBundle(IMvxBundle parameters)
        {
            base.InitFromBundle(parameters);

            entrySelectedEPC = BleMvxApplication._SELECT_EPC;
            entrySelectedPWD = "00000000";
            entrySelectedKey0 = "";
            entrySelectedKey1 = "";

            RaisePropertyChanged(() => entrySelectedEPC);
            RaisePropertyChanged(() => entrySelectedPWD);
            RaisePropertyChanged(() => entrySelectedKey0);
            RaisePropertyChanged(() => entrySelectedKey1);
        }

        async void OnReadKeyButtonButtonClick()
        {
            accessPwd = Convert.ToUInt32(entrySelectedPWD, 16);

        }

        async void OnRandomKeyButtonButtonClick()
        {
            Random rnd = new Random();

            entrySelectedKey0 = "";
            entrySelectedKey1 = "";
            for (int cnt = 0; cnt < 8; cnt++)
            {
                entrySelectedKey0 += rnd.Next(0, 65535).ToString("X4");
                entrySelectedKey1 += rnd.Next(0, 65535).ToString("X4");
            }

            RaisePropertyChanged(() => entrySelectedKey0);
            RaisePropertyChanged(() => entrySelectedKey1);
        }

        async void OnWriteKeyButtonButtonClick()
        {
            accessPwd = Convert.ToUInt32(entrySelectedPWD, 16);

        }

        async void OnActivateKeyButtonButtonClick()
        {
        }

        async void OnUntraceableButtonButtonClick()
        {
        }

        async void OnAuthenticateButtonButtonClick()
        {
        }

        void ReadUSER()
        {
            BleMvxApplication._reader.rfid.Options.TagReadUser.accessPassword = accessPwd;
            BleMvxApplication._reader.rfid.Options.TagReadUser.offset = 0xD0; // m_readAllBank.OffsetUser;
            BleMvxApplication._reader.rfid.Options.TagReadUser.count = 9; // m_readAllBank.WordUser;

            BleMvxApplication._reader.rfid.StartOperation(CSLibrary.Constants.Operation.TAG_READ_USER);
        }

        void WriteUSER()
        {
            BleMvxApplication._reader.rfid.Options.TagWritePC.accessPassword = accessPwd;
            BleMvxApplication._reader.rfid.Options.TagWritePC.pc = CSLibrary.Tools.Hex.ToUshort(entrySelectedEPC);

            //BleMvxApplication._reader.rfid.StartOperation(CSLibrary.Constants.Operation.TAG_WRITEUSER);
        }




#if nouse
        void TagCompletedEvent(object sender, CSLibrary.Events.OnAccessCompletedEventArgs e)
        {
            if (e.access == CSLibrary.Constants.TagAccess.READ)
			{
                switch (e.bank)
                {
                    case CSLibrary.Constants.Bank.PC:
                        if (e.success)
                        {
                            entryPC = BleMvxApplication._reader.rfid.Options.TagReadPC.pc.ToString();
                            RaisePropertyChanged(() => entryPC);

							labelPCStatus = "O";
						}
						else
						{
                            if (--readPCRetryCnt == 0)
                                labelPCStatus = "E";
                            else
                                ReadPC();
						}
						RaisePropertyChanged(() => labelPCStatus);
						break;

                    case CSLibrary.Constants.Bank.EPC:
                        if (e.success)
                        {
                            entryEPC = BleMvxApplication._reader.rfid.Options.TagReadEPC.epc.ToString();
                            RaisePropertyChanged(() => entryEPC);
							labelEPCStatus = "O";
						}
						else
						{
                            if (--readEPCRetryCnt == 0)
							    labelEPCStatus = "E";
                            else
                                ReadEPC();
						}
						RaisePropertyChanged(() => labelEPCStatus);
						break;

                    case CSLibrary.Constants.Bank.ACC_PWD:
                        if (e.success)
                        {
                            entryACCPWD = BleMvxApplication._reader.rfid.Options.TagReadAccPwd.password.ToString();
                            RaisePropertyChanged(() => entryACCPWD);
							labelACCPWDStatus = "O";
						}
						else
						{
                            if (--readACCPWDRetryCnt == 0)
                                labelACCPWDStatus = "E";
                            else
                                ReadACCPWD();
						}
						RaisePropertyChanged(() => labelACCPWDStatus);
						break;

                    case CSLibrary.Constants.Bank.KILL_PWD:
                        if (e.success)
                        {
                            entryKILLPWD = BleMvxApplication._reader.rfid.Options.TagReadKillPwd.password.ToString();
                            RaisePropertyChanged(() => entryKILLPWD);
							labelKILLPWDStatus = "O";
						}
						else
						{
                            if (--readKILLPWDRetryCnt == 0)
                                labelKILLPWDStatus = "E";
                            else
                                ReadKILLPWD();
						}
						RaisePropertyChanged(() => labelKILLPWDStatus);
						break;

                    case CSLibrary.Constants.Bank.TID:
                        if (e.success)
                        {
                            entryTIDUID = BleMvxApplication._reader.rfid.Options.TagReadTid.tid.ToString();
                            RaisePropertyChanged(() => entryTIDUID);
							labelTIDUIDStatus = "O";
						}
						else
						{
                            if (--readTIDUIDRetryCnt == 0)
                                labelTIDUIDStatus = "E";
                            else
                                ReadTIDUID();
						}
						RaisePropertyChanged(() => labelTIDUIDStatus);
						break;

                    case CSLibrary.Constants.Bank.USER:
                        if (e.success)
                        {
                            entryUSER = BleMvxApplication._reader.rfid.Options.TagReadUser.pData.ToString();
                            RaisePropertyChanged(() => entryUSER);
							labelUSERStatus = "O";
						}
						else
						{
                            if (--readUSERRetryCnt == 0)
                                labelUSERStatus = "E";
                            else
                                ReadUSER();
						}
						RaisePropertyChanged(() => labelUSERStatus);
						break;
                }
            }

			if (e.access == CSLibrary.Constants.TagAccess.WRITE)
			{
				switch (e.bank)
				{
					case CSLibrary.Constants.Bank.PC:
						if (e.success)
						{
							labelPCStatus = "O";
						}
						else
						{
                            if (--writePCRetryCnt == 0)
                                labelPCStatus = "E";
                            else
                                WritePC ();
						}
						RaisePropertyChanged(() => labelPCStatus);
						break;

					case CSLibrary.Constants.Bank.EPC:
						if (e.success)
						{
							labelEPCStatus = "O";
						}
						else
						{
                            if (--writeEPCRetryCnt == 0)
                                labelEPCStatus = "E";
                            else
                                WriteEPC();
						}
						RaisePropertyChanged(() => labelEPCStatus);
						break;

					case CSLibrary.Constants.Bank.ACC_PWD:
						if (e.success)
						{
							labelACCPWDStatus = "O";
						}
						else
						{
                            if (--writeACCPWDRetryCnt == 0)
                                labelACCPWDStatus = "E";
                            else
                                WriteACCPWD();
						}
						RaisePropertyChanged(() => labelACCPWDStatus);
						break;

					case CSLibrary.Constants.Bank.KILL_PWD:
						if (e.success)
						{
							labelKILLPWDStatus = "O";
						}
						else
						{
                            if (--writeKILLPWDRetryCnt == 0)
                                labelKILLPWDStatus = "E";
                            else
                                WriteKILLPWD();
						}
						RaisePropertyChanged(() => labelKILLPWDStatus);
						break;

					case CSLibrary.Constants.Bank.USER:
						if (e.success)
						{
							labelUSERStatus = "O";
						}
						else
						{
                            if (--writeUSERRetryCnt == 0)
                                labelUSERStatus = "E";
                            else
                                WriteUSER();
						}
						RaisePropertyChanged(() => labelUSERStatus);
						break;
				}
			}
		}

        async void onlabelTIDOffsetTapped()
        {
            try
            {
                var msg = $"Enter a TID bank offset value (word)";
                this.cancelSrc?.CancelAfter(TimeSpan.FromSeconds(3));
                var r = await this._userDialogs.PromptAsync(msg, inputType: InputType.Number, placeholder: _labelTIDOffset.ToString(), cancelToken: this.cancelSrc?.Token);
                await System.Threading.Tasks.Task.Delay(500);

                _labelTIDOffset = UInt16.Parse(r.Text);

            }
            catch (Exception ex)
            {
            }

            RaisePropertyChanged(() => labelTIDOffset);
        }

        async void onlabelTIDWordTapped()
        {
            try
            {
                var msg = $"Enter a TID bank length value (word)";
                this.cancelSrc?.CancelAfter(TimeSpan.FromSeconds(3));
                var r = await this._userDialogs.PromptAsync(msg, inputType: InputType.Number, placeholder: _labelTIDWord.ToString(), cancelToken: this.cancelSrc?.Token);
                await System.Threading.Tasks.Task.Delay(500);

                _labelTIDWord = UInt16.Parse(r.Text);
            }
            catch (Exception ex)
            {
            }

            RaisePropertyChanged(() => labelTIDWord);
        }

        async void onlabelUSEROffsetTapped()
        {
            try
            {
                var msg = $"Enter a USER bank offset value (word)";
                this.cancelSrc?.CancelAfter(TimeSpan.FromSeconds(3));
                var r = await this._userDialogs.PromptAsync(msg, inputType: InputType.Number, placeholder: _labelUSEROffset.ToString(), cancelToken: this.cancelSrc?.Token);
                await System.Threading.Tasks.Task.Delay(500);

                _labelUSEROffset = UInt16.Parse(r.Text);
            }
            catch (Exception ex)
            {
            }

            RaisePropertyChanged(() => labelUSEROffset);
        }

        async void onlabelUSERWOordTapped()
        {
            try
            {
                var msg = $"Enter a read length value (word)";
                this.cancelSrc?.CancelAfter(TimeSpan.FromSeconds(3));
                var r = await this._userDialogs.PromptAsync(msg, inputType: InputType.Number, placeholder: _labelUSERWord.ToString(), cancelToken: this.cancelSrc?.Token);
                await System.Threading.Tasks.Task.Delay(500);

                _labelUSERWord = UInt16.Parse(r.Text);
            }
            catch (Exception ex)
            {
            }

            RaisePropertyChanged(() => labelUSERWord);
        }

        System.Threading.CancellationTokenSource cancelSrc;

        void OnReadButtonButtonClick()
        {
            Xamarin.Forms.DependencyService.Get<ISystemSound>().SystemSound(1);

            uint m_retry_cnt = 7;       // Max 7

			labelPCStatus = "";
			labelEPCStatus = "";
			labelACCPWDStatus = "";
			labelKILLPWDStatus = "";
			labelTIDUIDStatus = "";
			labelUSERStatus = "";

			RaisePropertyChanged(() => entrySelectedEPC);
            RaisePropertyChanged(() => entrySelectedPWD);

            RaisePropertyChanged(() => switchPCIsToggled);
            RaisePropertyChanged(() => switchEPCIsToggled);
            RaisePropertyChanged(() => switchACCPWDIsToggled);
            RaisePropertyChanged(() => switchKILLPWDIsToggled);
            RaisePropertyChanged(() => switchTIDUIDIsToggled);
            RaisePropertyChanged(() => switchUSERIsToggled);

            accessPwd = Convert.ToUInt32(entrySelectedPWD, 16);

            if (BleMvxApplication._reader.rfid.State != CSLibrary.Constants.RFState.IDLE)
            {
                //MessageBox.Show("Reader is busy now, please try later.");
                return;
            }

            //BleMvxApplication._reader.rfid.StartOperation(CSLibrary.Constants.Operation.TAG_RANGING);

            if (entrySelectedEPC.Length > 64)
            {
                //MessageBox.Show("EPC too long, only selecte first 256 bit");
                BleMvxApplication._reader.rfid.Options.TagSelected.epcMask = new CSLibrary.Structures.S_MASK(/*m_record.pc.ToString() + */entrySelectedEPC.Substring(0, 64));
            }
            else
                BleMvxApplication._reader.rfid.Options.TagSelected.epcMask = new CSLibrary.Structures.S_MASK(/*m_record.pc.ToString() + */entrySelectedEPC);

            BleMvxApplication._reader.rfid.Options.TagSelected.flags = CSLibrary.Constants.SelectMaskFlags.ENABLE_TOGGLE;
            BleMvxApplication._reader.rfid.Options.TagSelected.epcMaskOffset = 0;
            BleMvxApplication._reader.rfid.Options.TagSelected.epcMaskLength = (uint)BleMvxApplication._reader.rfid.Options.TagSelected.epcMask.Length * 8;
            BleMvxApplication._reader.rfid.StartOperation(CSLibrary.Constants.Operation.TAG_SELECTED);

            BleMvxApplication._reader.rfid.SetCurrentLinkProfile(BleMvxApplication._config.RFID_Profile);

            if (switchPCIsToggled)
            {
				labelPCStatus = "R";

                readPCRetryCnt = m_retry_cnt;
                ReadPC();
			}

			if (switchEPCIsToggled)
            {
				labelEPCStatus = "R";

                readEPCRetryCnt = m_retry_cnt;
                ReadEPC();
			}

			//if access bank is checked, read it.
			if (switchACCPWDIsToggled)
            {
				labelACCPWDStatus = "R";

                readACCPWDRetryCnt = m_retry_cnt;
                ReadACCPWD();
			}

			//if kill bank is checked, read it.
			if (switchKILLPWDIsToggled)
            {
				labelKILLPWDStatus = "R";

                readKILLPWDRetryCnt = m_retry_cnt;
                ReadKILLPWD();
            }

			//if TID-UID is checked, read it.
			if (switchTIDUIDIsToggled)
			{
				labelTIDUIDStatus = "R";

                readTIDUIDRetryCnt = m_retry_cnt;
                ReadTIDUID();
			}

			//if user bank is checked, read it.
			if (switchUSERIsToggled)
            {
				labelUSERStatus = "R";

                readUSERRetryCnt = m_retry_cnt;
                ReadUSER();
            }

			RaisePropertyChanged(() => labelPCStatus);
			RaisePropertyChanged(() => labelEPCStatus);
			RaisePropertyChanged(() => labelACCPWDStatus);
			RaisePropertyChanged(() => labelKILLPWDStatus);
			RaisePropertyChanged(() => labelTIDUIDStatus);
			RaisePropertyChanged(() => labelUSERStatus);
		}

		void OnWriteButtonButtonClick()
        {
            Xamarin.Forms.DependencyService.Get<ISystemSound>().SystemSound(1);

            RaisePropertyChanged(() => switchTIDUIDIsToggled);

            //if tid bank is checked, read it.
            if (switchTIDUIDIsToggled)
            {
                //_userDialogs.ShowError("TID only display, cannot write", 3000);

                ShowDialog("TID only display, cannot write");

                return;
            }

			uint m_retry_cnt = 7;       // Max 7

            RaisePropertyChanged(() => switchPCIsToggled);
            RaisePropertyChanged(() => switchEPCIsToggled);
            RaisePropertyChanged(() => switchACCPWDIsToggled);
            RaisePropertyChanged(() => switchKILLPWDIsToggled);
            RaisePropertyChanged(() => switchTIDUIDIsToggled);
            RaisePropertyChanged(() => switchUSERIsToggled);

            RaisePropertyChanged(() => entrySelectedEPC);
            RaisePropertyChanged(() => entrySelectedPWD);
            RaisePropertyChanged(() => entryPC);
            RaisePropertyChanged(() => entryEPC);
            RaisePropertyChanged(() => entryACCPWD);
            RaisePropertyChanged(() => entryKILLPWD);
            RaisePropertyChanged(() => entryTIDUID);
            RaisePropertyChanged(() => entryUSER);

            accessPwd = Convert.ToUInt32(entrySelectedPWD, 16);

            if (BleMvxApplication._reader.rfid.State != CSLibrary.Constants.RFState.IDLE)
            {
                //MessageBox.Show("Reader is busy now, please try later.");
                return;
            }

            // Can not write TID bank
            if (switchTIDUIDIsToggled)
            {
				return;
            }

            if (!(switchPCIsToggled | switchEPCIsToggled | switchACCPWDIsToggled | switchKILLPWDIsToggled | switchUSERIsToggled))
            {
                //All unchecked
                //MessageBox.Show("Please check at least one item to write", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                return;
            }

            if (entrySelectedEPC.Length > 64)
            {
                //MessageBox.Show("EPC too long, only selecte first 256 bit");
                BleMvxApplication._reader.rfid.Options.TagSelected.epcMask = new CSLibrary.Structures.S_MASK(/*m_record.pc.ToString() + */entrySelectedEPC.Substring(0, 64));
            }
            else
                BleMvxApplication._reader.rfid.Options.TagSelected.epcMask = new CSLibrary.Structures.S_MASK(/*m_record.pc.ToString() + */entrySelectedEPC);

            BleMvxApplication._reader.rfid.Options.TagSelected.flags = CSLibrary.Constants.SelectMaskFlags.ENABLE_TOGGLE;
            BleMvxApplication._reader.rfid.Options.TagSelected.epcMaskOffset = 0;
            BleMvxApplication._reader.rfid.Options.TagSelected.epcMaskLength = (uint)BleMvxApplication._reader.rfid.Options.TagSelected.epcMask.Length * 8;
            BleMvxApplication._reader.rfid.StartOperation(CSLibrary.Constants.Operation.TAG_SELECTED);

            //if access bank is checked, read it.
            if (switchACCPWDIsToggled)
            {
				labelACCPWDStatus = "W";

                writeACCPWDRetryCnt = m_retry_cnt;
                WriteACCPWD();
            }

            //if kill bank is checked, read it.
            if (switchKILLPWDIsToggled)
            {
				labelKILLPWDStatus = "W";

                writeKILLPWDRetryCnt = m_retry_cnt;
                WriteKILLPWD();
            }

            //if user bank is checked, read it.
            if (switchUSERIsToggled)
            {
				labelUSERStatus = "W";

                writeUSERRetryCnt = m_retry_cnt;
                WriteUSER();
            }

            if (switchPCIsToggled)
            {
				labelPCStatus = "W";

                writePCRetryCnt = m_retry_cnt;
                WritePC();
            }
            
            //Write EPC must put in last order to prevent it get lost
            if (switchEPCIsToggled)
            {
				labelEPCStatus = "W";

                writeEPCRetryCnt = m_retry_cnt;
                WriteEPC();
            }

			RaisePropertyChanged(() => labelPCStatus);
			RaisePropertyChanged(() => labelEPCStatus);
			RaisePropertyChanged(() => labelACCPWDStatus);
			RaisePropertyChanged(() => labelKILLPWDStatus);
            RaisePropertyChanged(() => labelUSERStatus);
        }


        async void ShowDialog(string Msg)
        {
            var config = new ProgressDialogConfig()
            {
                Title = Msg,
                IsDeterministic = true,
                MaskType = MaskType.Gradient,
            };

            using (var progress = _userDialogs.Progress(config))
            {
                progress.Show();
                await System.Threading.Tasks.Task.Delay(3000);
            }
        }
#endif

    }
}
