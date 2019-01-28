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

        public string labelKey0Status { get; set; } = "";
        public string labelKey1Status { get; set; } = "";

        public ICommand OnReadKeyButtonCommand { protected set; get; }
        public ICommand OnRandomKeyButtonCommand { protected set; get; }
        public ICommand OnWriteKeyButtonCommand { protected set; get; }
        public ICommand OnHideButtonCommand { protected set; get; }
        public ICommand OnUnhideButtonCommand { protected set; get; }
        public ICommand OnActivateKey0ButtonCommand { protected set; get; }
        public ICommand OnActivateKey1ButtonCommand { protected set; get; }
        public ICommand OnAuthenticateTAM0ButtonCommand { protected set; get; }
        public ICommand OnAuthenticateTAM1ButtonCommand { protected set; get; }

        uint accessPwd;

        enum CURRENTOPERATION
        {
            READKEY0,
            READKEY1,
            WRITEKEY0,
            WRITEKEY1,
            ACTIVEKEY0,
            ACTIVEKEY1,
            UNKNOWN
        }

        CURRENTOPERATION _currentOperation = CURRENTOPERATION.UNKNOWN;

        public ViewModelUCODEDNA(IAdapter adapter, IUserDialogs userDialogs) : base(adapter)
        {
            _userDialogs = userDialogs;

            OnReadKeyButtonCommand = new Command(OnReadKeyButtonButtonClick);
            OnRandomKeyButtonCommand = new Command(OnRandomKeyButtonButtonClick);
            OnWriteKeyButtonCommand = new Command(OnWriteKeyButtonButtonClick);
            OnHideButtonCommand = new Command(OnHideButtonButtonClick);
            OnUnhideButtonCommand = new Command(OnUnhideButtonButtonClick);
            OnActivateKey0ButtonCommand = new Command(OnActivateKey0ButtonButtonClick);
            OnActivateKey1ButtonCommand = new Command(OnActivateKey1ButtonButtonClick);
            OnAuthenticateTAM0ButtonCommand = new Command(OnAuthenticateTAM0ButtonButtonClick);
            OnAuthenticateTAM1ButtonCommand = new Command(OnAuthenticateTAM1ButtonButtonClick);
        }

        public override void Resume()
        {
            base.Resume();
            //BleMvxApplication._reader.rfid.OnAccessCompleted += new EventHandler<CSLibrary.Events.OnAccessCompletedEventArgs>(TagCompletedEvent);
        }

        public override void Suspend()
        {
            BleMvxApplication._reader.rfid.OnAccessCompleted -= new EventHandler<CSLibrary.Events.OnAccessCompletedEventArgs>(TagCompletedEvent);
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

            BleMvxApplication._reader.rfid.OnAccessCompleted += new EventHandler<CSLibrary.Events.OnAccessCompletedEventArgs>(TagCompletedEvent);
        }

        void OnRandomKeyButtonButtonClick()
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

        void OnReadKeyButtonButtonClick()
        {
            accessPwd = Convert.ToUInt32(entrySelectedPWD, 16);

            TagSelected();
            ReadKey0();
        }

        void OnWriteKeyButtonButtonClick()
        {
            accessPwd = Convert.ToUInt32(entrySelectedPWD, 16);

            TagSelected();
            WriteKey0();
        }

        void OnHideButtonButtonClick()
        {
            TagSelected();

            BleMvxApplication._reader.rfid.Options.TagUntraceable.EPCLength = 0; // NXP AN11778 only have EPCLength function 

            /* for Gen2V2 
            BleMvxApplication._reader.rfid.Options.TagUntraceable.Range = CSLibrary.Structures.UNTRACEABLE_RANGE.Normal;
            BleMvxApplication._reader.rfid.Options.TagUntraceable.User = CSLibrary.Structures.UNTRACEABLE_USER.View;
            BleMvxApplication._reader.rfid.Options.TagUntraceable.TID = CSLibrary.Structures.UNTRACEABLE_TID.HideNone;
            BleMvxApplication._reader.rfid.Options.TagUntraceable.EPC = CSLibrary.Structures.UNTRACEABLE_EPC.Show;
            BleMvxApplication._reader.rfid.Options.TagUntraceable.EPCLength = 0;
            BleMvxApplication._reader.rfid.Options.TagUntraceable.U = CSLibrary.Structures.UNTRACEABLE_U.AssertU;
            */

            BleMvxApplication._reader.rfid.StartOperation(CSLibrary.Constants.Operation.TAG_UNTRACEABLE);
        }

        void OnUnhideButtonButtonClick()
        {
            TagSelected();

            BleMvxApplication._reader.rfid.Options.TagUntraceable.EPCLength = 6; // NXP AN11778 only have EPCLength function
            BleMvxApplication._reader.rfid.StartOperation(CSLibrary.Constants.Operation.TAG_UNTRACEABLE);
        }

        void OnActivateKey0ButtonButtonClick()
        {
            _currentOperation = CURRENTOPERATION.ACTIVEKEY0;

            labelKey0Status = "A";
            RaisePropertyChanged(() => labelKey0Status);

            BleMvxApplication._reader.rfid.Options.TagWriteUser.accessPassword = accessPwd;
            BleMvxApplication._reader.rfid.Options.TagWriteUser.offset = 0xc8; // m_writeAllBank.OffsetUser;
            BleMvxApplication._reader.rfid.Options.TagWriteUser.count = 1; // m_writeAllBank.WordUser;
            BleMvxApplication._reader.rfid.Options.TagWriteUser.pData = CSLibrary.Tools.Hex.ToUshorts("E200");

            BleMvxApplication._reader.rfid.StartOperation(CSLibrary.Constants.Operation.TAG_WRITE_USER);
        }

        void OnActivateKey1ButtonButtonClick()
        {
            _currentOperation = CURRENTOPERATION.ACTIVEKEY1;

            labelKey1Status = "A";
            RaisePropertyChanged(() => labelKey1Status);

            BleMvxApplication._reader.rfid.Options.TagWriteUser.accessPassword = accessPwd;
            BleMvxApplication._reader.rfid.Options.TagWriteUser.offset = 0xd8; // m_writeAllBank.OffsetUser;
            BleMvxApplication._reader.rfid.Options.TagWriteUser.count = 1; // m_writeAllBank.WordUser;
            BleMvxApplication._reader.rfid.Options.TagWriteUser.pData = CSLibrary.Tools.Hex.ToUshorts("E200");

            BleMvxApplication._reader.rfid.StartOperation(CSLibrary.Constants.Operation.TAG_WRITE_USER);
        }

        void OnAuthenticateTAM0ButtonButtonClick()
        {

        }

        void OnAuthenticateTAM1ButtonButtonClick()
        {

        }

        void TagSelected()
        {
            BleMvxApplication._reader.rfid.Options.TagSelected.flags = CSLibrary.Constants.SelectMaskFlags.ENABLE_TOGGLE;
            BleMvxApplication._reader.rfid.Options.TagSelected.epcMaskOffset = 0;
            BleMvxApplication._reader.rfid.Options.TagSelected.epcMask = new CSLibrary.Structures.S_MASK(entrySelectedEPC);
            BleMvxApplication._reader.rfid.Options.TagSelected.epcMaskLength = (uint)BleMvxApplication._reader.rfid.Options.TagSelected.epcMask.Length * 8;
            BleMvxApplication._reader.rfid.StartOperation(CSLibrary.Constants.Operation.TAG_SELECTED);
        }

        void ReadKey0 ()
        {
            _currentOperation = CURRENTOPERATION.READKEY0;

            labelKey0Status = "R";
            RaisePropertyChanged(() => labelKey0Status);

            BleMvxApplication._reader.rfid.Options.TagReadUser.accessPassword = accessPwd;
            BleMvxApplication._reader.rfid.Options.TagReadUser.offset = 0xc0;
            BleMvxApplication._reader.rfid.Options.TagReadUser.count = 8;

            BleMvxApplication._reader.rfid.StartOperation(CSLibrary.Constants.Operation.TAG_READ_USER);
        }

        void ReadKey1 ()
        {
            _currentOperation = CURRENTOPERATION.READKEY1;

            labelKey1Status = "R";
            RaisePropertyChanged(() => labelKey1Status);

            BleMvxApplication._reader.rfid.Options.TagReadUser.accessPassword = accessPwd;
            BleMvxApplication._reader.rfid.Options.TagReadUser.offset = 0xd0;
            BleMvxApplication._reader.rfid.Options.TagReadUser.count = 8;

            BleMvxApplication._reader.rfid.StartOperation(CSLibrary.Constants.Operation.TAG_READ_USER);
        }

        void WriteKey0 ()
        {
            _currentOperation = CURRENTOPERATION.WRITEKEY0;

            labelKey0Status = "W";
            RaisePropertyChanged(() => labelKey0Status);

            BleMvxApplication._reader.rfid.Options.TagWriteUser.accessPassword = accessPwd;
            BleMvxApplication._reader.rfid.Options.TagWriteUser.offset = 0xc0; // m_writeAllBank.OffsetUser;
            BleMvxApplication._reader.rfid.Options.TagWriteUser.count = 8; // m_writeAllBank.WordUser;
            BleMvxApplication._reader.rfid.Options.TagWriteUser.pData = CSLibrary.Tools.Hex.ToUshorts(entrySelectedKey0);

            BleMvxApplication._reader.rfid.StartOperation(CSLibrary.Constants.Operation.TAG_WRITE_USER);
        }

        void WriteKey1 ()
        {
            _currentOperation = CURRENTOPERATION.WRITEKEY1;

            labelKey1Status = "W";
            RaisePropertyChanged(() => labelKey1Status);

            BleMvxApplication._reader.rfid.Options.TagWriteUser.accessPassword = accessPwd;
            BleMvxApplication._reader.rfid.Options.TagWriteUser.offset = 0xd0; // m_writeAllBank.OffsetUser;
            BleMvxApplication._reader.rfid.Options.TagWriteUser.count = 8; // m_writeAllBank.WordUser;
            BleMvxApplication._reader.rfid.Options.TagWriteUser.pData = CSLibrary.Tools.Hex.ToUshorts(entrySelectedKey1);

            BleMvxApplication._reader.rfid.StartOperation(CSLibrary.Constants.Operation.TAG_WRITE_USER);
        }

        void TagCompletedEvent(object sender, CSLibrary.Events.OnAccessCompletedEventArgs e)
        {
            if (e.access == CSLibrary.Constants.TagAccess.READ)
            {
                switch (e.bank)
                {
                    case CSLibrary.Constants.Bank.USER:
                        if (e.success)
                        {
                            switch (_currentOperation)
                            {
                                case CURRENTOPERATION.READKEY0:
                                    entrySelectedKey0 = BleMvxApplication._reader.rfid.Options.TagReadUser.pData.ToString();
                                    labelKey0Status = "O";
                                    RaisePropertyChanged(() => entrySelectedKey0);
                                    RaisePropertyChanged(() => labelKey0Status);
                                    break;

                                case CURRENTOPERATION.READKEY1:
                                    entrySelectedKey1 = BleMvxApplication._reader.rfid.Options.TagReadUser.pData.ToString();
                                    labelKey1Status = "O";
                                    RaisePropertyChanged(() => entrySelectedKey1);
                                    RaisePropertyChanged(() => labelKey1Status);
                                    break;
                            }
                        }
                        else
                        {
                            switch (_currentOperation)
                            {
                                case CURRENTOPERATION.READKEY0:
                                    labelKey0Status = "E";
                                    RaisePropertyChanged(() => labelKey0Status);
                                    break;

                                case CURRENTOPERATION.READKEY1:
                                    labelKey1Status = "E";
                                    RaisePropertyChanged(() => labelKey1Status);
                                    break;
                            }
                        }

                        if (_currentOperation == CURRENTOPERATION.READKEY0)
                            ReadKey1();

                        break;
                }
            }

            if (e.access == CSLibrary.Constants.TagAccess.WRITE)
            {
                switch (e.bank)
                {
                    case CSLibrary.Constants.Bank.UNTRACEABLE:
                        if (e.success)
                        {
                            switch (_currentOperation)
                            {
                                case CURRENTOPERATION.ACTIVEKEY0:
                                    labelKey0Status = "O";
                                    RaisePropertyChanged(() => labelKey0Status);
                                    break;

                                case CURRENTOPERATION.ACTIVEKEY1:
                                    labelKey1Status = "O";
                                    RaisePropertyChanged(() => labelKey1Status);
                                    break;
                            }
                        }
                        else
                        {
                            switch (_currentOperation)
                            {
                                case CURRENTOPERATION.ACTIVEKEY0:
                                    labelKey0Status = "E";
                                    RaisePropertyChanged(() => labelKey0Status);
                                    break;

                                case CURRENTOPERATION.ACTIVEKEY1:
                                    labelKey1Status = "E";
                                    RaisePropertyChanged(() => labelKey1Status);
                                    break;
                            }
                        }
                        break;

                    case CSLibrary.Constants.Bank.USER:
                        if (e.success)
                        {
                            switch (_currentOperation)
                            {
                                case CURRENTOPERATION.WRITEKEY0:
                                    labelKey0Status = "O";
                                    RaisePropertyChanged(() => labelKey0Status);
                                    break;

                                case CURRENTOPERATION.WRITEKEY1:
                                    labelKey1Status = "O";
                                    RaisePropertyChanged(() => labelKey1Status);
                                    break;
                            }
                        }
                        else
                        {
                            switch (_currentOperation)
                            {
                                case CURRENTOPERATION.WRITEKEY0:
                                    labelKey0Status = "E";
                                    RaisePropertyChanged(() => labelKey0Status);
                                    break;

                                case CURRENTOPERATION.WRITEKEY1:
                                    labelKey1Status = "E";
                                    RaisePropertyChanged(() => labelKey1Status);
                                    break;
                            }
                        }

                        if (_currentOperation == CURRENTOPERATION.WRITEKEY0)
                            WriteKey1();

                        break;
                }
            }
        }
    }
}
