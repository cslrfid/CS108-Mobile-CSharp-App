using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BLE.Client.Pages
{
	public partial class PageBlockWrite
	{
        string[] _bankOptions = new string []{ "Bank3 (User Bank)", "Bank1 (EPC Bank)" };
        string[] _sizeOptions = new string[] { "4K bit", "8K bit" };
        string[] _paddingOptions = new string[] { "repeart 55AA ", "repeart AA55", "repeart 0000", "repeart FFFF", "repeart 0001", "repeart 0002", "repeart 0004", "repeart 0008", "repeart 0010", "repeart 0020", "repeart 0040", "repeart 0080", "repeart 0100", "repeart 0200", "repeart 0400", "repeart 0800", "repeart 1000", "repeart 2000", "repeart 4000", "repeart 8000" };

        public PageBlockWrite()
		{
			InitializeComponent();
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();

            editorSelectedEPC.Text = BleMvxApplication._SELECT_EPC;
            buttonBank.Text = _bankOptions[0];
            buttonSize.Text = _sizeOptions[0];
            buttonPadding.Text = _paddingOptions[0];
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        public async void buttonBankClicked(object sender, EventArgs args)
        {
            var answer = await DisplayActionSheet("Bank", "Cancel", null, _bankOptions);

            if (answer != "Cancel")
            {
                buttonBank.Text = answer;
            }
        }

        public async void buttonSizeClicked(object sender, EventArgs args)
        {
            var answer = await DisplayActionSheet("Data Size", "Cancel", null, _sizeOptions);

            if (answer != "Cancel")
            {
                buttonSize.Text = answer;
            }
        }

        public async void buttonPaddingClicked(object sender, EventArgs args)
        {
            var answer = await DisplayActionSheet("Sensor Type", "Cancel", null, _paddingOptions);

            if (answer != "Cancel")
            {
                buttonPadding.Text = answer;
            }
        }

        void SelectTag()
        {
            BleMvxApplication._reader.rfid.Options.TagSelected.epcMask = new CSLibrary.Structures.S_MASK(/*m_record.pc.ToString() + */editorSelectedEPC.Text);

            BleMvxApplication._reader.rfid.Options.TagSelected.flags = CSLibrary.Constants.SelectMaskFlags.ENABLE_TOGGLE;
            BleMvxApplication._reader.rfid.Options.TagSelected.epcMaskOffset = 0;
            BleMvxApplication._reader.rfid.Options.TagSelected.epcMaskLength = (uint)BleMvxApplication._reader.rfid.Options.TagSelected.epcMask.Length * 8;
            BleMvxApplication._reader.rfid.StartOperation(CSLibrary.Constants.Operation.TAG_SELECTED);
        }

        void buttonBlockWriteClicked(object sender, EventArgs args)
        {
            int dataWordSize = (Array.IndexOf(_sizeOptions, buttonSize.Text) == 0 ? 256 : 512);
            UInt16[] data = new UInt16[dataWordSize];
            int paddingType = Array.IndexOf(_paddingOptions, buttonPadding.Text);
            UInt16 padding = 0;

            switch (paddingType)
            {
                case 0:
                    padding = 0x55AA;
                    break;
            }

            for (int i = 0; i < dataWordSize; i++)
                data [i] = padding;

            SelectTag();

            BleMvxApplication._reader.rfid.Options.TagBlockWrite.flags = CSLibrary.Constants.SelectFlags.SELECT;
            BleMvxApplication._reader.rfid.Options.TagBlockWrite.accessPassword = 0;
            BleMvxApplication._reader.rfid.Options.TagBlockWrite.bank = CSLibrary.Constants.MemoryBank.USER;
            BleMvxApplication._reader.rfid.Options.TagBlockWrite.offset = 0;
            BleMvxApplication._reader.rfid.Options.TagBlockWrite.count = 256;  // max 256
            BleMvxApplication._reader.rfid.Options.TagBlockWrite.data = data;

            CSLibrary.Debug.WriteLine("4K Block Write Test Start");
            BleMvxApplication._reader.rfid.StartOperation(CSLibrary.Constants.Operation.TAG_BLOCK_WRITE);
        }

        DateTime _startingTime;
        void buttonReadVerifyClicked(object sender, EventArgs args)
        {
            SelectTag();

            BleMvxApplication._reader.rfid.Options.TagReadUser.accessPassword = 0;
            BleMvxApplication._reader.rfid.Options.TagReadUser.offset = 0; // 0
            BleMvxApplication._reader.rfid.Options.TagReadUser.count = 32; // 32 word = 64 byte = 512 bit

            _startingTime = DateTime.Now;
            BleMvxApplication._reader.rfid.StartOperation(CSLibrary.Constants.Operation.TAG_READ_USER);
        }

    }
}
