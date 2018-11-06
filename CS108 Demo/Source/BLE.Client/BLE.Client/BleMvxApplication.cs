using System;
using System.IO;
using System.Collections.Generic;

using System.Threading.Tasks;

using BLE.Client.ViewModels;
using MvvmCross.Core.ViewModels;

using PCLStorage;
using Newtonsoft.Json;

using CSLibrary;

namespace BLE.Client
{
	public class CONFIG
	{
        public class MAINMENUSHORTCUT
        {
            public enum FUNCTION
            {
                NONE,
                INVENTORY,
                BARCODE,
            }

            public FUNCTION Function = FUNCTION.NONE;
            public uint DurationMin = 0;
            public uint DurationMax = 0;
        }

        public string readerID = "";

        public int BatteryLevelIndicatorFormat = 1; // 0 = voltage, other = percentage 

        public int RFID_Power;
		public uint RFID_Profile;
		public int RFID_InventoryDelayTime;
        public UInt32 RFID_InventoryCycleDelayTime;
        public CSLibrary.Constants.RadioOperationMode RFID_OperationMode;
		public uint RFID_DWellTime;
        public uint RFID_TagPopulation;
        public CSLibrary.Structures.TagGroup RFID_TagGroup;
		public CSLibrary.Constants.SingulationAlgorithm RFID_Algorithm;
		public CSLibrary.Structures.DynamicQParms RFID_DynamicQParms;
		public CSLibrary.Structures.FixedQParms RFID_FixedQParms;

        public CSLibrary.Constants.RegionCode RFID_Region = CSLibrary.Constants.RegionCode.UNKNOWN;
        public int RFID_FrequenceSwitch = 0; // 0 = hopping, 1 = fixed, 2 = agile
        public uint RFID_FixedChannel = 0;

		// Multi Bank Inventory Setting
		public bool RFID_MBI_MultiBank1Enable;
		public CSLibrary.Constants.MemoryBank RFID_MBI_MultiBank1;
		public UInt16 RFID_MBI_MultiBank1Offset;
		public UInt16 RFID_MBI_MultiBank1Count;
		public bool RFID_MBI_MultiBank2Enable;
		public CSLibrary.Constants.MemoryBank RFID_MBI_MultiBank2;
		public UInt16 RFID_MBI_MultiBank2Offset;
		public UInt16 RFID_MBI_MultiBank2Count;

        // Main Menu Shortcut
        public MAINMENUSHORTCUT[] RFID_Shortcut = new MAINMENUSHORTCUT[6];

        public bool RFID_InventoryAlertSound = true;
        public bool RFID_QOverride = false;
        public bool RFID_DBm = true;

        // Backend Server
        public bool RFID_SavetoFile = false;
        public bool RFID_SavetoCloud = true;
        public int RFID_CloudProtocol = 0;
        public string RFID_IPAddress;

        public bool RFID_Vibration = false;
        public bool RFID_VibrationTag = false;      // false = New, true = All
        public uint RFID_VibrationWindow = 2;      // 2 seconds
        public uint RFID_VibrationTime = 300;       // 300 ms

        public CONFIG ()
		{
			RFID_Power = 300;
			RFID_DWellTime = 0;
            RFID_TagPopulation = 30;
            RFID_InventoryDelayTime = 0;
            RFID_InventoryCycleDelayTime = 0;

            RFID_OperationMode = CSLibrary.Constants.RadioOperationMode.CONTINUOUS;
			RFID_TagGroup = new CSLibrary.Structures.TagGroup(CSLibrary.Constants.Selected.ALL, CSLibrary.Constants.Session.S1, CSLibrary.Constants.SessionTarget.A);
			RFID_Algorithm = CSLibrary.Constants.SingulationAlgorithm.DYNAMICQ;
			RFID_Profile = 1;

			RFID_DynamicQParms = new CSLibrary.Structures.DynamicQParms ();
			RFID_DynamicQParms.minQValue = 0;
			RFID_DynamicQParms.startQValue = 6;
			RFID_DynamicQParms.maxQValue = 15;
			RFID_DynamicQParms.toggleTarget = 0;
			RFID_DynamicQParms.thresholdMultiplier = 4;
			RFID_DynamicQParms.retryCount = 0;

			RFID_FixedQParms = new CSLibrary.Structures.FixedQParms ();
			RFID_FixedQParms.qValue = 6;
			RFID_FixedQParms.retryCount = 0;
			RFID_FixedQParms.toggleTarget = 0;
			RFID_FixedQParms.repeatUntilNoTags = 0;

			RFID_MBI_MultiBank1Enable = false;
			RFID_MBI_MultiBank2Enable = false;
			RFID_MBI_MultiBank1 = CSLibrary.Constants.MemoryBank.TID;
			RFID_MBI_MultiBank1Offset = 0;
			RFID_MBI_MultiBank1Count = 1;
			RFID_MBI_MultiBank2 = CSLibrary.Constants.MemoryBank.USER;
			RFID_MBI_MultiBank2Offset = 0;
			RFID_MBI_MultiBank2Count = 1;

            RFID_InventoryAlertSound = true;
            RFID_QOverride = false;
            RFID_DBm = true;

            RFID_SavetoFile = false;
            RFID_SavetoCloud = true;
            RFID_CloudProtocol = 0;
            RFID_IPAddress = "";

            RFID_Vibration = false;
            RFID_VibrationTag = false;      // false = New, true = All
            RFID_VibrationWindow = 2;      // 2 seconds
            RFID_VibrationTime = 300;       // 500 ms

            for (int cnt = 0; cnt < RFID_Shortcut.Length; cnt++)
            {
                MAINMENUSHORTCUT item = new MAINMENUSHORTCUT();

                switch (cnt)
                {
                    case 0:
                        item.Function = MAINMENUSHORTCUT.FUNCTION.INVENTORY;
                        item.DurationMin = 0;
                        item.DurationMax = 500;
                        break;
                    case 1:
                        item.Function = MAINMENUSHORTCUT.FUNCTION.BARCODE;
                        item.DurationMin = 500;
                        item.DurationMax = 10000;
                        break;
                }

                RFID_Shortcut[cnt] = item;
            }
        }
	}

    public class BleMvxApplication : MvxApplication
    {
        static public HighLevelInterface _reader = new HighLevelInterface ();
        public static CONFIG _config;

        // for Geiger and Read/Write
        public static string _SELECT_EPC;
        public static UInt16 _SELECT_PC;

        // for PreFilter
        public static string _PREFILTER_MASK_EPC = "";
        public static uint _PREFILTER_MASK_Offset = 0;
        public static int _PREFILTER_MASK_Truncate = 0;
        public static bool _PREFILTER_Enable = false;

        // for Post Filter
        public static string _POSTFILTER_MASK_EPC = "";
        public static uint _POSTFILTER_MASK_Offset = 0;
        public static bool _POSTFILTER_MASK_MatchNot = false;
        public static bool _POSTFILTER_MASK_Enable = false;

        public static int _inventoryEntryPoint = 0;
        public static bool _settingPage1TagPopulationChanged = false;
        public static bool _settingPage3QvalueChanged = false;
        public static bool _settingPage4QvalueChanged = false;

        // for Cloud server
        public static UInt16 _sequenceNumber = 0;

        // for battery level display
        public static bool _batteryLow = false;

        public override void Initialize()
        {
            RegisterAppStart<ViewModelMainMenu>();
            //RegisterAppStart<DeviceListViewModel>();
        }

        //static async public void LoadConfig(string readerID)
        static public async Task<bool> LoadConfig(string readerID)
        {
            try
            {
                _config = new CONFIG();

                IFolder rootFolder = FileSystem.Current.LocalStorage;
                IFolder sourceFolder = await FileSystem.Current.LocalStorage.CreateFolderAsync("CSLReader", CreationCollisionOption.OpenIfExists);
                IFile sourceFile = await sourceFolder.CreateFileAsync(readerID + ".cfg", CreationCollisionOption.OpenIfExists);

                var contentJSON = await sourceFile.ReadAllTextAsync();
                var setting = JsonConvert.DeserializeObject<CONFIG>(contentJSON);

                if (setting != null)
                {
                    _config = setting;
                    return true;
                }
            }
            catch (Exception ex)
            {
            }

            return false;
        }

        static async public void SaveConfig()
        {
            IFolder rootFolder = FileSystem.Current.LocalStorage;
            IFolder sourceFolder = await FileSystem.Current.LocalStorage.CreateFolderAsync("CSLReader", CreationCollisionOption.OpenIfExists);
            IFile sourceFile = await sourceFolder.CreateFileAsync(_config.readerID + ".cfg", CreationCollisionOption.ReplaceExisting);

            string contentJSON = JsonConvert.SerializeObject(_config);
            await sourceFile.WriteAllTextAsync(contentJSON);
        }

        static public void ResetConfig()
        {
            var readerID = _config.readerID;
            _config = new CONFIG();
            _config.readerID = readerID;
        }

    }
}
