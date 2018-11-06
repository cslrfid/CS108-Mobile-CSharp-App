using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Plugin.BLE.Abstractions;
using CSLibrary.Events;
using CSLibrary.Constants;

namespace CSLibrary
{
	public partial class RFIDReader
	{
#if del
		private enum RFIDREADERCMDSTATUS
		{
			IDLE,           // Can send (SetRegister, GetRegister, ExecCmd, Abort), Can not receive data
			GETREGISTER,    // Can not send data, Can receive (GetRegister) 
			EXECCMD,        // Can send (Abort), Can receive (CMDBegin, CMDEnd, Inventory data, Abort)
			INVENTORY,      // Can send (Abort)
			ABORT,          // Can not send
		}

		private enum SENDREMARK
		{
			GENERAL,
			GETREGISTER,
			INVENTORY,
			EXECCMD,
		}

		// RFID event code
		private class DOWNLINKCMD
		{
			public static readonly byte[] RFIDPOWERON = { 0x80, 0x00 };
			public static readonly byte[] RFIDPOWEROFF = { 0x80, 0x01 };
			public static readonly byte[] RFIDCMD = { 0x80, 0x02 };
		}

		/// <summary>
		/// REGISTER NAME/ADDRESS CONSTANTS
		/// </summary>
		private enum MACREGISTER : UInt16
		{
			MAC_VER = 0x0000,
			MAC_INFO = 0x0001,
			MAC_RFTRANSINFO = 0x0002,
			MAC_DBG1 = 0x0003,
			MAC_DBG2 = 0x0004,
			MAC_ERROR = 0x0005,
			HST_ENGTST_ARG0 = 0x0100,
			HST_ENGTST_ARG1 = 0x0101,
			HST_DBG1 = 0x0102,
			HST_EMU = 0x0103,
			HST_PWRMGMT = 0x0200,
			HST_CMNDIAGS = 0x0201,
			MAC_BLK02RES1 = 0x0202,
			HST_CTR_GCFG = 0x0203,
			HST_CTR1_CFG = 0x0204,
			MAC_CTR1_VAL = 0x0205,
			HST_CTR2_CFG = 0x0206,
			MAC_CTR2_VAL = 0x0207,
			HST_CTR3_CFG = 0x0208,
			MAC_CTR3_VAL = 0x0209,
			HST_CTR4_CFG = 0x020A,
			MAC_CTR4_VAL = 0x020B,
			HST_PROTSCH_SMIDX = 0x0300,
			HST_PROTSCH_SMCFG = 0x0301,
			HST_PROTSCH_FTIME_SEL = 0x0302,
			HST_PROTSCH_FTIME = 0x0303,
			HST_PROTSCH_SMCFG_SEL = 0x0304,
			HST_PROTSCH_TXTIME_SEL = 0x0305,
			HST_PROTSCH_TXTIME_ON = 0x0306,
			HST_PROTSCH_TXTIME_OFF = 0x0307,
			HST_PROTSCH_CYCCFG_SEL = 0x0308,
			HST_PROTSCH_CYCCFG_DESC_ADJ1 = 0x0309,
			HST_PROTSCH_ADJCW = 0x030A,
			HST_MBP_ADDR = 0x0400,
			HST_MBP_DATA = 0x0401,
			HST_MBP_RFU_0x0402 = 0x0402,
			HST_MBP_RFU_0x0403 = 0x0403,
			HST_MBP_RFU_0x0404 = 0x0404,
			HST_MBP_RFU_0x0405 = 0x0405,
			HST_MBP_RFU_0x0406 = 0x0406,
			HST_MBP_RFU_0x0407 = 0x0407,
			HST_LPROF_SEL = 0x0408,
			HST_LPROF_ADDR = 0x0409,
			HST_LPROF_DATA = 0x040A,
			HST_OEM_ADDR = 0x0500,
			HST_OEM_DATA = 0x0501,
			HST_GPIO_INMSK = 0x0600,
			HST_GPIO_OUTMSK = 0x0601,
			HST_GPIO_OUTVAL = 0x0602,
			HST_GPIO_CFG = 0x0603,
			HST_ANT_CYCLES = 0x0700,
			HST_ANT_DESC_SEL = 0x0701,
			HST_ANT_DESC_CFG = 0x0702,
			MAC_ANT_DESC_STAT = 0x0703,
			HST_ANT_DESC_PORTDEF = 0x0704,
			HST_ANT_DESC_DWELL = 0x0705,
			HST_ANT_DESC_RFPOWER = 0x0706,
			HST_ANT_DESC_INV_CNT = 0x0707,
			HST_TAGMSK_DESC_SEL = 0x0800,
			HST_TAGMSK_DESC_CFG = 0x0801,
			HST_TAGMSK_BANK = 0x0802,
			HST_TAGMSK_PTR = 0x0803,
			HST_TAGMSK_LEN = 0x0804,
			HST_TAGMSK_0_3 = 0x0805,
			HST_TAGMSK_4_7 = 0x0806,
			HST_TAGMSK_8_11 = 0x0807,
			HST_TAGMSK_12_15 = 0x0808,
			HST_TAGMSK_16_19 = 0x0809,
			HST_TAGMSK_20_23 = 0x080A,
			HST_TAGMSK_24_27 = 0x080B,
			HST_TAGMSK_28_31 = 0x080C,
			HST_QUERY_CFG = 0x0900,
			HST_INV_CFG = 0x0901,
			HST_INV_SEL = 0x0902,
			HST_INV_ALG_PARM_0 = 0x0903,
			HST_INV_ALG_PARM_1 = 0x0904,
			HST_INV_ALG_PARM_2 = 0x0905,
			HST_INV_ALG_PARM_3 = 0x0906,
			HST_INV_RFU_0x0907 = 0x0907,
			HST_INV_RFU_0x0908 = 0x0908,
			HST_INV_RFU_0x0909 = 0x0909,
			HST_INV_RFU_0x090A = 0x090A,
			HST_INV_RFU_0x090B = 0x090B,
			HST_INV_RFU_0x090C = 0x090C,
			HST_INV_RFU_0x090D = 0x090D,
			HST_INV_RFU_0x090E = 0x090E,
			HST_INV_RFU_0x090F = 0x090F,
			HST_INV_EPC_MATCH_SEL = 0x0910,
			HST_INV_EPC_MATCH_CFG = 0x0911,
			HST_INV_EPCDAT_0_3 = 0x0912,
			HST_INV_EPCDAT_4_7 = 0x0913,
			HST_INV_EPCDAT_8_11 = 0x0914,
			HST_INV_EPCDAT_12_15 = 0x0915,
			HST_INV_EPCDAT_16_19 = 0x0916,
			HST_INV_EPCDAT_20_23 = 0x0917,
			HST_INV_EPCDAT_24_27 = 0x0918,
			HST_INV_EPCDAT_28_31 = 0x0919,
			HST_INV_EPCDAT_32_35 = 0x091A,
			HST_INV_EPCDAT_36_39 = 0x091B,
			HST_INV_EPCDAT_40_43 = 0x091C,
			HST_INV_EPCDAT_44_47 = 0x091D,
			HST_INV_EPCDAT_48_51 = 0x091E,
			HST_INV_EPCDAT_52_55 = 0x091F,
			HST_INV_EPCDAT_56_59 = 0x0920,
			HST_INV_EPCDAT_60_63 = 0x0921,
			HST_TAGACC_DESC_SEL = 0x0A00,
			HST_TAGACC_DESC_CFG = 0x0A01,
			HST_TAGACC_BANK = 0x0A02,
			HST_TAGACC_PTR = 0x0A03,
			HST_TAGACC_CNT = 0x0A04,
			HST_TAGACC_LOCKCFG = 0x0A05,
			HST_TAGACC_ACCPWD = 0x0A06,
			HST_TAGACC_KILLPWD = 0x0A07,
			HST_TAGWRDAT_SEL = 0x0A08,
			HST_TAGWRDAT_0 = 0x0A09,
			HST_TAGWRDAT_1 = 0x0A0A,
			HST_TAGWRDAT_2 = 0x0A0B,
			HST_TAGWRDAT_3 = 0x0A0C,
			HST_TAGWRDAT_4 = 0x0A0D,
			HST_TAGWRDAT_5 = 0x0A0E,
			HST_TAGWRDAT_6 = 0x0A0F,
			HST_TAGWRDAT_7 = 0x0A10,
			HST_TAGWRDAT_8 = 0x0A11,
			HST_TAGWRDAT_9 = 0x0A12,
			HST_TAGWRDAT_10 = 0x0A13,
			HST_TAGWRDAT_11 = 0x0A14,
			HST_TAGWRDAT_12 = 0x0A15,
			HST_TAGWRDAT_13 = 0x0A16,
			HST_TAGWRDAT_14 = 0x0A17,
			HST_TAGWRDAT_15 = 0x0A18,
			MAC_RFTC_PAPWRLEV = 0x0B00,
			HST_RFTC_PAPWRCTL_PGAIN = 0x0B01,
			HST_RFTC_PAPWRCTL_IGAIN = 0x0B02,
			HST_RFTC_PAPWRCTL_DGAIN = 0x0B03,
			MAC_RFTC_REVPWRLEV = 0x0B04,
			HST_RFTC_REVPWRTHRSH = 0x0B05,
			MAC_RFTC_AMBIENTTEMP = 0x0B06,
			HST_RFTC_AMBIENTTEMPTHRSH = 0x0B07,
			MAC_RFTC_XCVRTEMP = 0x0B08,
			HST_RFTC_XCVRTEMPTHRSH = 0x0B09,
			MAC_RFTC_PATEMP = 0x0B0A,
			HST_RFTC_PATEMPTHRSH = 0x0B0B,
			HST_RFTC_PADELTATEMPTHRSH = 0x0B0C,
			HST_RFTC_PAPWRCTL_AIWDELAY = 0x0B0D,
			MAC_RFTC_PAPWRCTL_STAT0 = 0x0B0E,
			MAC_RFTC_PAPWRCTL_STAT1 = 0x0B0F,
			MAC_RFTC_PAPWRCTL_STAT2 = 0x0B10,
			MAC_RFTC_PAPWRCTL_STAT3 = 0x0B11,
			HST_RFTC_ANTSENSRESTHRSH = 0x0B12,
			HST_RFTC_IFLNAAGCRANGE = 0x0B13,
			MAC_RFTC_LAST_ANACTRL1 = 0x0B14,
			HST_RFTC_OPENLOOPPWRCTRL = 0x0B15,
			HST_RFTC_RFU_0x0B16 = 0x0B16,
			HST_RFTC_RFU_0x0B17 = 0x0B17,
			HST_RFTC_RFU_0x0B18 = 0x0B18,
			HST_RFTC_RFU_0x0B19 = 0x0B19,
			HST_RFTC_PREDIST_COEFF0 = 0x0B1A,
			HST_RFTC_RFU_0x0B1B = 0x0B1B,
			HST_RFTC_RFU_0x0B1C = 0x0B1C,
			HST_RFTC_RFU_0x0B1D = 0x0B1D,
			HST_RFTC_RFU_0x0B1E = 0x0B1E,
			HST_RFTC_RFU_0x0B1F = 0x0B1F,
			HST_RFTC_CAL_GGNEG7 = 0x0B20,
			HST_RFTC_CAL_GGNEG5 = 0x0B21,
			HST_RFTC_CAL_GGNEG3 = 0x0B22,
			HST_RFTC_CAL_GGNEG1 = 0x0B23,
			HST_RFTC_CAL_GGPLUS1 = 0x0B24,
			HST_RFTC_CAL_GGPLUS3 = 0x0B25,
			HST_RFTC_CAL_GGPLUS5 = 0x0B26,
			HST_RFTC_CAL_GGPLUS7 = 0x0B27,
			HST_RFTC_CAL_MACADCREFV = 0x0B28,
			HST_RFTC_CAL_RFFWDPWR_C0 = 0x0B29,
			HST_RFTC_CAL_RFFWDPWR_C1 = 0x0B2A,
			HST_RFTC_CAL_RFFWDPWR_C2 = 0x0B2B,
			HST_RFTC_RFU_0x0B2C = 0x0B2C,
			HST_RFTC_RFU_0x0B2D = 0x0B2D,
			HST_RFTC_RFU_0x0B2E = 0x0B2E,
			HST_RFTC_RFU_0x0B2F = 0x0B2F,
			HST_RFTC_CLKDBLR_CFG = 0x0B30,
			HST_RFTC_CLKDBLR_SEL = 0x0B31,
			HST_RFTC_CLKDBLR_LUTENTRY = 0x0B32,
			HST_RFTC_RFU_0x0B33 = 0x0B33,
			HST_RFTC_RFU_0x0B34 = 0x0B34,
			HST_RFTC_RFU_0x0B35 = 0x0B35,
			HST_RFTC_RFU_0x0B36 = 0x0B36,
			HST_RFTC_RFU_0x0B37 = 0x0B37,
			HST_RFTC_RFU_0x0B38 = 0x0B38,
			HST_RFTC_RFU_0x0B39 = 0x0B39,
			HST_RFTC_RFU_0x0B3A = 0x0B3A,
			HST_RFTC_RFU_0x0B3B = 0x0B3B,
			HST_RFTC_RFU_0x0B3C = 0x0B3C,
			HST_RFTC_RFU_0x0B3D = 0x0B3D,
			HST_RFTC_RFU_0x0B3E = 0x0B3E,
			HST_RFTC_RFU_0x0B3F = 0x0B3F,
			HST_RFTC_FRQHOPMODE = 0x0B40,
			HST_RFTC_FRQHOPENTRYCNT = 0x0B41,
			HST_RFTC_FRQHOPTABLEINDEX = 0x0B42,
			MAC_RFTC_HOPCNT = 0x0B43,
			HST_RFTC_MINHOPDUR = 0x0B44,
			HST_RFTC_MAXHOPDUR = 0x0B45,
			HST_RFTC_FRQHOPRANDSEED = 0x0B46,
			MAC_RFTC_FRQHOPSHFTREGVAL = 0x0B47,
			MAC_RFTC_FRQHOPRANDNUMCNT = 0x0B48,
			HST_RFTC_FRQCHINDEX = 0x0B49,
			HST_RFTC_PLLLOCKTIMEOUT = 0x0B4A,
			HST_RFTC_PLLLOCK_DET_THRSH = 0x0B4B,
			HST_RFTC_PLLLOCK_DET_CNT = 0x0B4C,
			HST_RFTC_PLLLOCK_TO = 0x0B4D,
			HST_RFTC_BERREADDELAY = 0x0B4E,
			HST_RFTC_RFU_0x0B4F = 0x0B4F,
			MAC_RFTC_FWDRFPWRRAWADC = 0x0B50,
			MAC_RFTC_REVRFPWRRAWADC = 0x0B51,
			MAC_RFTC_ANTSENSERAWADC = 0x0B52,
			MAC_RFTC_AMBTEMPRAWADC = 0x0B53,
			MAC_RFTC_PATEMPRAWADC = 0x0B54,
			MAC_RFTC_XCVRTEMPRAWADC = 0x0B55,
			HST_RFTC_RFU_0x0B56 = 0x0B56,
			HST_RFTC_RFU_0x0B57 = 0x0B57,
			HST_RFTC_RFU_0x0B58 = 0x0B58,
			HST_RFTC_RFU_0x0B59 = 0x0B59,
			HST_RFTC_RFU_0x0B5A = 0x0B5A,
			HST_RFTC_RFU_0x0B5B = 0x0B5B,
			HST_RFTC_RFU_0x0B5C = 0x0B5C,
			HST_RFTC_RFU_0x0B5D = 0x0B5D,
			HST_RFTC_RFU_0x0B5E = 0x0B5E,
			HST_RFTC_RFU_0x0B5F = 0x0B5F,
			HST_RFTC_CURRENT_PROFILE = 0x0B60,
			HST_RFTC_PROF_SEL = 0x0B61,
			MAC_RFTC_PROF_CFG = 0x0B62,
			MAC_RFTC_PROF_ID_HIGH = 0x0B63,
			MAC_RFTC_PROF_ID_LOW = 0x0B64,
			MAC_RFTC_PROF_IDVER = 0x0B65,
			MAC_RFTC_PROF_PROTOCOL = 0x0B66,
			MAC_RFTC_PROF_R2TMODTYPE = 0x0B67,
			MAC_RFTC_PROF_TARI = 0x0B68,
			MAC_RFTC_PROF_X = 0x0B69,
			MAC_RFTC_PROF_PW = 0x0B6A,
			MAC_RFTC_PROF_RTCAL = 0x0B6B,
			MAC_RFTC_PROF_TRCAL = 0x0B6C,
			MAC_RFTC_PROF_DIVIDERATIO = 0x0B6D,
			MAC_RFTC_PROF_MILLERNUM = 0x0B6E,
			MAC_RFTC_PROF_T2RLINKFREQ = 0x0B6F,
			MAC_RFTC_PROF_VART2DELAY = 0x0B70,
			MAC_RFTC_PROF_RXDELAY = 0x0B71,
			MAC_RFTC_PROF_MINTOTT2DELAY = 0x0B72,
			MAC_RFTC_PROF_TXPROPDELAY = 0x0B73,
			MAC_RFTC_PROF_RSSIAVECFG = 0x0B74,
			MAC_RFTC_PROF_PREAMCMD = 0x0B75,
			MAC_RFTC_PROF_FSYNCCMD = 0x0B76,
			MAC_RFTC_PROF_T2WAITCMD = 0x0B77,
			HST_RFTC_RFU_0x0B78 = 0x0B78,
			HST_RFTC_RFU_0x0B79 = 0x0B79,
			HST_RFTC_RFU_0x0B7A = 0x0B7A,
			HST_RFTC_RFU_0x0B7B = 0x0B7B,
			HST_RFTC_RFU_0x0B7C = 0x0B7C,
			HST_RFTC_RFU_0x0B7D = 0x0B7D,
			HST_RFTC_RFU_0x0B7E = 0x0B7E,
			HST_RFTC_RFU_0x0B7F = 0x0B7F,
			HST_RFTC_RFU_0x0B80 = 0x0B80,
			HST_RFTC_RFU_0x0B81 = 0x0B81,
			HST_RFTC_RFU_0x0B82 = 0x0B82,
			HST_RFTC_RFU_0x0B83 = 0x0B83,
			HST_RFTC_RFU_0x0B84 = 0x0B84,
			HST_RFTC_FRQCH_ENTRYCNT = 0x0C00,
			HST_RFTC_FRQCH_SEL = 0x0C01,
			HST_RFTC_FRQCH_CFG = 0x0C02,
			HST_RFTC_FRQCH_DESC_PLLDIVMULT = 0x0C03,
			HST_RFTC_FRQCH_DESC_PLLDACCTL = 0x0C04,
			MAC_RFTC_FRQCH_DESC_PLLLOCKSTAT0 = 0x0C05,
			MAC_RFTC_FRQCH_DESC_PLLLOCKSTAT1 = 0x0C06,
			HST_RFTC_FRQCH_DESC_PARFU3 = 0x0C07,
			HST_RFTC_FRQCH_CMDSTART = 0x0C08,
			HST_CMD = 0xF000
		}
#endif

#region public variable
#region ====================== Callback Event Handler ======================
		/// <summary>
		/// Reader Operation State Event
		/// </summary>
		public event EventHandler<CSLibrary.Events.OnStateChangedEventArgs> OnStateChanged;

		/// <summary>
		/// Tag Inventory(including Inventory and search) callback event
		/// </summary>
		public event EventHandler<CSLibrary.Events.OnAsyncCallbackEventArgs> OnAsyncCallback;

		/// <summary>
		/// Tag Access (including Tag read/write/kill/lock) completed event
		/// </summary>
		public event EventHandler<CSLibrary.Events.OnAccessCompletedEventArgs> OnAccessCompleted;

#endregion
#endregion

		private HighLevelInterface _deviceHandler;
		private CSLibrary.Tools.Queue _dataBuffer = new Tools.Queue(409600);
		RFIDREADERCMDSTATUS _readerStatus = RFIDREADERCMDSTATUS.IDLE;

		/// <summary>
		/// Transfer BT API packet to R2000 packet
		/// </summary>
		/// <param name="recvData"></param>
		/// <param name="offset"></param>
		/// <param name="size"></param>
		public void DeviceRecvData(byte[] recvData)
		{
			if (_dataBuffer.length == 0)
			{
				// first packet
				byte header = recvData[10];

				switch (header)
				{
					case 0x70:  // register read packet
						{
							UInt16 add = (UInt16)(recvData[10 + 3] << 8 | recvData[10 + 2]);
							UInt32 data = (UInt16)(recvData[10 + 7] << 24 | recvData[10 + 6] << 16 | recvData[10 + 5] << 8 | recvData[10 + 4]);

							_registerData[add] = data;
						}
						break;

					case 0x00:
						{
							if (recvData[8] == 0x70)
							{
								//cycle end
							}
						}
						break;

					case 0x01:  //
						{
							if (recvData[8] == 0x70)
							{
								//cycle end
							}
						}
						break;

					case 0x02:  //
						{
							if (recvData[10 + 3] == 0x80)   // Command packet
							{
							}
							else
							{
								break;
							}
							switch (recvData[10 + 3])
							{
								case 0x00:      // Command Begin
									break;

								case 0x01:      // Command End
									break;

								default:        // Error Code
									break;
							}
						}
						break;

					case 0x03:  // inventory 
						{
							var pkt_ver = recvData[10 + 0];
							var flags = recvData[10 + 1];
							var pkt_type = (UInt16)(recvData[10 + 2] | (recvData[10 + 3] << 8));
							var pkt_len = (UInt16)(recvData[10 + 4] | (recvData[10 + 5] << 8));
							var extdatalen = (pkt_len) * 4 - ((flags >> 6) & 3);

							switch (pkt_type)
							{
								case 0x8005:    /// inventory
									{
										if (OnAsyncCallback != null)
										{
											CSLibrary.Events.TagCallbackInfo info = new CSLibrary.Events.TagCallbackInfo();
											CSLibrary.Events.CallbackType type = CSLibrary.Events.CallbackType.TAG_RANGING;

											info.ms_ctr = (UInt32)(recvData[18 + 0] | recvData[18 + 1] << 8 | recvData[18 + 2] << 16 | recvData[18 + 3] << 24);
											info.rssi = (Single)(recvData[18 + 5] * 0.8);
											info.freqChannel = recvData[18 + 10];
											info.antennaPort = (UInt16)(recvData[18 + 10] | recvData[18 + 11] << 8);
											info.pc = (UInt16)(recvData[18 + 12] << 8 | recvData[18 + 13]);
											info.epcstrlen = (UInt16)((((pkt_len - 3) * 4) - ((flags >> 6) & 3) - 4));
											info.crc16 = (UInt16)(recvData[18 + 14 + info.epcstrlen] << 8 | recvData[18 + 15 + info.epcstrlen]);

											byte[] byteEpc = new byte[info.epcstrlen];
											Array.Copy(recvData, 18 + 14, byteEpc, 0, (int)info.epcstrlen);

											info.epc = (ushort[])CSLibrary.Tools.Hex.ToUshorts(byteEpc).Clone();

											CSLibrary.Events.OnAsyncCallbackEventArgs callBackData = new Events.OnAsyncCallbackEventArgs(info, type);
											OnAsyncCallback(_deviceHandler, callBackData);
										}
									}
									break;

								default:        // unknown packet
									break;
							}
						}
						break;

					case 0x40:  // Abort packet
						break;

					case 0x34:  // Unknow command
					default:    // skip invalid packet
						break;
				}
			}
			else
			{
				_dataBuffer.DataIn(recvData, 10, recvData[2]);
			}
		}


#if oldcode
        public void DeviceRecvData (byte [] recvData, int offset, int size)
        {
            _dataBuffer.DataIn(recvData, offset, size);

            // try to analysis first packet
            if (_dataBuffer.length >= 8)
            {
                byte[] header = _dataBuffer.DataPreOut();

                switch (header[0])
                {
                    case 0x70:  // register read packet
                        {
                            byte[] registerPacket = _dataBuffer.DataOut(8);

                            UInt16 add = (UInt16)(registerPacket[3] << 8 | registerPacket[2]);
                            UInt32 data = (UInt16)(registerPacket[7] << 24 | registerPacket[6] << 16 | registerPacket[5] << 8 | registerPacket[4]);

                            _registerData[add] = data;
                        }
                        break;

                    case 0x00:
                        {
                            byte[] registerPacket = _dataBuffer.DataOut(8);

                            if (registerPacket[1] == 0x70)
                            {
                                //cycle end
                            }
                        }
                        break;

                    case 0x01:  //
                        {
                            byte[] CommandPacket = _dataBuffer.DataOut(8);
                        }
                        break;

                    case 0x02:  //
                        {
                            byte[] CommandPacket = _dataBuffer.DataOut(8);

                            if (CommandPacket[3] == 0x80)   // Command packet
                            {
                                _dataBuffer.DataOut(8);
                            }
                            else
                            {
                                break;
                            }
                            switch (CommandPacket[2])
                            {
                                case 0x00:      // Command Begin
                                    break;

                                case 0x01:      // Command End
                                    break;
                                       
                                default:        // Error Code
                                    break;
                            }
                        }
                        break;

                    case 0x03:  // inventory 
                        {
                            byte[] tagPacketHeader = _dataBuffer.DataOut(8);

                            var pkt_ver = tagPacketHeader[0];
                            var flags = tagPacketHeader[1];
                            var pkt_type = (UInt16)(tagPacketHeader[2] | (tagPacketHeader[3] << 8));
                            var pkt_len = (UInt16)(tagPacketHeader[4] | (tagPacketHeader[5] << 8));
                            var extdatalen = (pkt_len) * 4 - ((flags >> 6) & 3);

                            switch (pkt_type)
                            {
                                case 0x8005:    /// inventory
                                    {
                                        int pktByteLen = pkt_len * 4;

                                        if (_dataBuffer.length < pktByteLen)
                                        {
                                            _dataBuffer.Clear();
                                            return;
                                        }

                                        byte[] tagPacketBody = _dataBuffer.DataOut(pktByteLen);

                                        if (OnAsyncCallback != null)
                                        {
                                            CSLibrary.Events.TagCallbackInfo info = new CSLibrary.Events.TagCallbackInfo();
                                            CSLibrary.Events.CallbackType type = CSLibrary.Events.CallbackType.TAG_RANGING;

                                            info.ms_ctr = (UInt32)(tagPacketBody[0] | tagPacketBody[1] << 8 | tagPacketBody[2] << 16 | tagPacketBody[3] << 24);
                                            info.rssi = (Single)(tagPacketBody[5] * 0.8);
                                            info.freqChannel = tagPacketBody[10];
                                            info.antennaPort = (UInt16)(tagPacketBody[10] | tagPacketBody[11] << 8);
                                            info.pc = (UInt16)(tagPacketBody[12] << 8 | tagPacketBody[13]);
                                            info.epcstrlen = (UInt16)((((pkt_len - 3) * 4) - ((flags >> 6) & 3) - 4));
                                            info.crc16 = (UInt16)(tagPacketBody[14 + info.epcstrlen] << 8 | tagPacketBody[15 + info.epcstrlen]);

                                            byte[] byteEpc = new byte[info.epcstrlen];
                                            Array.Copy(tagPacketBody, 14, byteEpc, 0, (int)info.epcstrlen);

                                            info.epc = (ushort[])CSLibrary.Tools.Hex.ToUshorts(byteEpc).Clone();

                                            CSLibrary.Events.OnAsyncCallbackEventArgs callBackData = new Events.OnAsyncCallbackEventArgs(info, type);
                                            OnAsyncCallback(_deviceHandler, callBackData);
                                        }
                                    }
                                    break;

                                default:        // unknown packet
                                    break;
                            }


                        }
                        break;

                    default:    // skip invalid packet
                        _dataBuffer.Clear();
                        break;
                }
            }


            /*
                        switch (_readerStatus)
                        {
                            case RFIDREADERCMDSTATUS.IDLE:
                                break;

                            case RFIDREADERCMDSTATUS.GETREGISTER:
                                if (_dataBuffer.length >= 8)
                                {
                                    byte [] header = _dataBuffer.DataPreOut(4);
                                    if (Array.Equals(header, new byte []{0x01, 0x02, 0x03, 0x04 }))
                                    {
                                        byte [] getRegiterPacket = _dataBuffer.DataOut(8);
                                    }
                                }
                                _readerStatus = RFIDREADERCMDSTATUS.IDLE;
                                break;

                            case RFIDREADERCMDSTATUS.EXECCMD:  // Receive command begin packet
                                if (_dataBuffer.length >= 16)
                                {

                                }
                                break;

                            case RFIDREADERCMDSTATUS.INVENTORY:  // Receive inventory packet
                                if (_dataBuffer.length >= 16)
                                {

                                }
                                break;

                            case RFIDREADERCMDSTATUS.ABORT: // Receive bbort response
                                if (_dataBuffer.length >= 8)
                                {
                                    byte[] packetData = _dataBuffer.DataPreOut(8);
                                    if (Array.Equals(packetData, new byte[] { 0x43, 0x02, 0x03, 0x04 }))
                                    {
                                        _dataBuffer.Skip (8);
                                    }
                                }
                                _readerStatus = RFIDREADERCMDSTATUS.IDLE;
                                break;
                        }

                */

        }
#endif

		// public RFID function
		public void PowerOn()
		{
			Trace.Message("DateTime {0}", DateTime.Now);

			_deviceHandler.SendAsync(0, 0, DOWNLINKCMD.RFIDPOWERON);
		}

		public void PowerOff()
		{
			Trace.Message("DateTime {0}", DateTime.Now);

			_deviceHandler.SendAsync(0, 0, DOWNLINKCMD.RFIDPOWEROFF);
		}

		public void SetPowerLevel(UInt32 pwrlevel)
		{
			MacWriteRegister(0x701, 0);
			MacWriteRegister(0x706, pwrlevel);
		}

        public uint[] GetActiveLinkProfile(uint region = 0)
		{
			switch (region)
			{
//				case RegionCode.JP2012:
//					return new uint[] { 2, 3, 5 };

//				case RegionCode.UNKNOWN:
//					return new uint[0];

				default:
					return new uint[] { 0, 1, 2, 3, 4, 5 };
			}
		}

		/// <summary>
		/// 0 = Success
		/// </summary>
		/// <param name="profile"></param>
		/// <returns></returns>
		public int SetCurrentLinkProfile(uint profile)
		{
			uint[] prof = GetActiveLinkProfile();



/*
			if (prof.ToList(() ==> profile) == null)
				return true;


			foreach (uint link in prof)
			{
				if (link == profile)
					return true;
			}
			return false;

*/
			//			MacReadRegister(MACREGISTER.HST_RFTC_CURRENT_PROFILE, ref currentProfile);
			//MacWriteRegister((UInt16)HST_RFTC_CURRENT_PROFILE, profile);
			MacWriteRegister((UInt16)0x901, profile);
			MacWriteRegister((UInt16)0xf000, 0x000f);

			return 0;
		}

		public UInt32 GetPowerLevel()
        {

			return 0;
        }

        public bool SetInventoryTimeDelay(UInt32 ms)
        {
            if (ms > 0x3f)
                return false;

			/*            const UInt16 reg = 0x901;

						_registerData[reg] &= ~(0x03f00000U);
						_registerData[reg] |= (ms << 20);
			*/

			UInt32 value;

			MacReadRegister(MACREGISTER.HST_INV_CFG, ref value);

			value &= ~(0x03f00000U);
			value |= (ms << 20);

			MacWriteRegister(MACREGISTER.HST_INV_CFG, value);

            return true;
        }

#if del
		private void TagSelected()
		{
			try
			{
				UInt32 value = 0;

				MacReadRegister(MACREGISTER.HST_TAGACC_DESC_CFG, ref value);
				value |= 0x0001U; // Enable Verify after write
				MacWriteRegister(MACREGISTER.HST_TAGACC_DESC_CFG, value);

				MacReadRegister(MACREGISTER.HST_QUERY_CFG, ref value);
				value &= ~0x0200U; // Enable Ucode Parallel encoding
				MacWriteRegister(MACREGISTER.HST_QUERY_CFG, value);

				if ((m_Result = SetOperationMode(RadioOperationMode.NONCONTINUOUS)) != Result.OK)
				{
					goto EXIT;
				}

				if ((m_Result = SetTagGroup(Selected.ASSERTED, Session.S0, SessionTarget.A)) != Result.OK)
				{
					goto EXIT;
				}

				if ((m_Result = SetSingulationAlgorithmParms(SingulationAlgorithm.FIXEDQ, new FixedQParms
					(
					m_rdr_opt_parms.TagSelected.Qvalue,//QValue
					0x5, //Retry
					(uint)((m_rdr_opt_parms.TagSelected.flags & SelectMaskFlags.ENABLE_TOGGLE) == SelectMaskFlags.ENABLE_TOGGLE ? 1 : 0),//toggle
					0)//repeatUntilNoUnit
					)) != Result.OK)
				{
					goto EXIT;
				}

				SelectCriterion[] sel = new SelectCriterion[1];
				sel[0] = new SelectCriterion();
				sel[0].action = new SelectAction(CSLibrary.Constants.Target.SELECTED,
					(m_rdr_opt_parms.TagSelected.flags & SelectMaskFlags.ENABLE_NON_MATCH) == SelectMaskFlags.ENABLE_NON_MATCH ?
					CSLibrary.Constants.Action.DSLINVB_ASLINVA : CSLibrary.Constants.Action.ASLINVA_DSLINVB, 0);


				if (m_rdr_opt_parms.TagSelected.bank == MemoryBank.EPC)
				{
					sel[0].mask = new SelectMask(
						m_rdr_opt_parms.TagSelected.bank,
						(uint)((m_rdr_opt_parms.TagSelected.flags & SelectMaskFlags.ENABLE_PC_MASK) == SelectMaskFlags.ENABLE_PC_MASK ? 16 : 32 + m_rdr_opt_parms.TagSelected.epcMaskOffset),
						m_rdr_opt_parms.TagSelected.epcMaskLength,
						m_rdr_opt_parms.TagSelected.epcMask.ToBytes());
				}
				else
				{
					sel[0].mask = new SelectMask(
						m_rdr_opt_parms.TagSelected.bank,
						m_rdr_opt_parms.TagSelected.MaskOffset,
						m_rdr_opt_parms.TagSelected.MaskLength,
						m_rdr_opt_parms.TagSelected.Mask);
				}
				if ((m_Result = SetSelectCriteria(sel)) != Result.OK)
				{
					goto EXIT;
				}
			}
			catch (System.Exception ex)
			{
#if DEBUG
				CSLibrary.Diagnostics.CoreDebug.Logger.ErrorException("HighLevelInterface.TagSelected()", ex);
#endif
				m_Result = Result.SYSTEM_CATCH_EXCEPTION;
			}

		}

		void Start18K6CRequest(uint tagStopCount, SelectFlags flags)
		{
			// Set up the rest of the HST_INV_CFG register.  First, we have to read its
			// current value
			UInt32 registerValue = 0;

			MacReadRegister(MACREGISTER.HST_INV_CFG, ref registerValue);
			registerValue &= ~0x0000FFC0U;  // reserver bit 0:5 ~ 16:31

			// TBD - an optimization could be to only write back the register if
			// the value changes

			// Set the tag stop count and enabled flags and then write the register
			// back
			if ((flags & SelectFlags.SELECT) != 0)
			{
				registerValue |= (1 << 14);
			}
			if ((flags & SelectFlags.DISABLE_INVENTORY) != 0)
			{
				registerValue |= (1 << 15);
			}
			registerValue |= tagStopCount << 6;
			MacWriteRegister(MACREGISTER.HST_INV_CFG, registerValue);

			// Set the enabled flag in the HST_INV_EPC_MATCH_CFG register properly.  To
			// do so, have to read the register value first.  Then set the bit properly
			// and then write the register value back to the MAC.
			MacReadRegister(MACREGISTER.HST_INV_EPC_MATCH_CFG, ref registerValue);
			if ((flags & SelectFlags.POST_MATCH) != 0)
			{
				registerValue |= 0x01;
			}
			else
			{
				registerValue &= ~(uint)0x01; ;
			}

			MacWriteRegister(MACREGISTER.HST_INV_EPC_MATCH_CFG, registerValue);
		} // Radio::Start18K6CRequest

		void Setup18K6CReadRegisters(UInt32 bank, UInt32 offset, UInt32 count)
		{
			// Set up the access bank register
			MacWriteRegister(MACREGISTER.HST_TAGACC_BANK, bank);

			// Set up the access pointer register (tells the offset)
			MacWriteRegister(MACREGISTER.HST_TAGACC_PTR, offset);

			// Set up the access count register (i.e., number values to read)
			MacWriteRegister(MACREGISTER.HST_TAGACC_CNT, count);
		}

		public int Start18K6CRead(uint bank, uint offset, uint count, UInt16[] data, uint accessPassword, uint retry, UInt16 flags)
		{
			// Perform the common 18K6C tag operation setup
			Start18K6CRequest(retry, flags);

			Setup18K6CReadRegisters(bank, offset, count);

			// Set up the access password register
			MacWriteRegister(MACREGISTER.HST_TAGACC_ACCPWD, accessPassword);

			// Issue the read command
			_deviceHandler.SendAsync(0, 0, DOWNLINKCMD.RFIDCMD, PacketData(0xf000, 0x000f), (UInt32)SENDREMARK.INVENTORY);

			return 0;
		} //  Start18K6CRead

		public int Start18K6CWrite(uint bank, uint offset, uint count, UInt16[] data, uint accessPassword, uint retry, UInt16 flags)
		{
			// Perform the common 18K6C tag operation setup
			Start18K6CRequest(retry, flags);

			Setup18K6CReadRegisters(bank, offset, count);

			// Set up the access password register
			MacWriteRegister(MACREGISTER.HST_TAGACC_ACCPWD, accessPassword);

			// Issue the read command
			_deviceHandler.SendAsync(0, 0, DOWNLINKCMD.RFIDCMD, PacketData(0xf000, 0x000f), (UInt32)SENDREMARK.INVENTORY);

			return 0;
		}
#endif

		public void StartOperation()
        {
            _deviceHandler.SendAsync(0, 0, DOWNLINKCMD.RFIDPOWERON);
        }

#region Public Functions

		public RFIDReader(HighLevelInterface deviceHandler)
		{
			_deviceHandler = deviceHandler;
		}

		~RFIDReader()
		{
		}

		public void Connect()
		{
			// Get and backup registers value
			MacReadRegister(0x901);
		}

		public void Reconnect()
		{
			// Restore registers value
			MacWriteRegister(0x901, _registerData[0x901]);
		}

		public void GetFirmwareVersion()
		{
			MacReadRegister(MACREGISTER.MAC_VER);
		}

		public UInt32 GetInventoryDelayTime()
		{
			return ((_registerData[0x901] >> 20) & 0X3F);
		}

		/// <summary>
		/// Retrieves the operation mode for the RFID radio module.  The 
		/// operation mode cannot be retrieved while a radio module is 
		/// executing a tag-protocol operation. 
		/// </summary>
		/// <param name="mode"> return will receive the current operation mode.</param>
		/// <returns></returns>
		public void GetOperationMode(ref RadioOperationMode mode)
		{
			uint value = 0;

			MacReadRegister(MACREGISTER.HST_ANT_CYCLES /*0x700 HST_ANT_CYCLES*/, ref value);

			if ((value & 0xffff)== 0xffff)
				mode = RadioOperationMode.CONTINUOUS;
			else
				mode = RadioOperationMode.NONCONTINUOUS;
		}

		/// <summary>
		/// Sets the operation mode of RFID radio module.  By default, when 
		/// an application opens a radio, the RFID Reader Library sets the 
		/// reporting mode to non-continuous.  An RFID radio module's 
		/// operation mode will remain in effect until it is explicitly changed 
		/// via RFID_RadioSetOperationMode, or the radio is closed and re-
		/// opened (at which point it will be set to non-continuous mode).  
		/// The operation mode may not be changed while a radio module is 
		/// executing a tag-protocol operation. 
		/// </summary>
		/// <param name="mode">The operation mode for the radio module.</param>
		/// <returns></returns>
		public void SetOperationMode(RadioOperationMode mode)
		{
			ushort cycles = 0;
			AntennaSequenceMode smode = AntennaSequenceMode.UNKNOWN;
			uint sequenceSize = 0;

			if (RadioOperationMode.UNKNOWN == mode)
			{
				return Result.INVALID_PARAMETER;
			}

			if ((m_Result = GetOperationMode(ref cycles, ref smode, ref sequenceSize)) == Result.OK)
			{
				m_Result = SetOperationMode((ushort)(mode == RadioOperationMode.CONTINUOUS ? 0xFFFF : 1), smode, sequenceSize);
			}
		}

		/// <summary>
		/// Sets the operation mode of RFID radio module.  By default, when 
		/// an application opens a radio, the RFID Reader Library sets the 
		/// reporting mode to non-continuous.  An RFID radio module's 
		/// operation mode will remain in effect until it is explicitly changed 
		/// via RFID_RadioSetOperationMode, or the radio is closed and re-
		/// opened (at which point it will be set to non-continuous mode).  
		/// The operation mode may not be changed while a radio module is 
		/// executing a tag-protocol operation. 
		/// </summary>
		/// <param name="cycles">The number of antenna cycles to be completed for command execution.
		/// <para>0x0001 = once cycle through</para>
		/// <para>0xFFFF = cycle forever until a CANCEL is received.</para></param>
		/// <param name="mode">Antenna Sequence mode.</param>
		/// <param name="sequenceSize">Sequence size. Maximum value is 48</param>
		/// <returns></returns>
		public void SetOperationMode(ushort cycles, Events.AntennaSequenceMode mode, uint sequenceSize)
		{
			uint value = 0;

			if (sequenceSize > 48)
				return;

			value = (cycles | ((uint)mode & 0x3) << 16 | (sequenceSize & 0x3F) << 18);
			MacWriteRegister(0x700, value);
		}

		public void StartInventory()
		{
			//_deviceHandler.rfid._dataBuffer.Clear();

			_deviceHandler.SendAsync(0, 0, DOWNLINKCMD.RFIDCMD, PacketData(0xf000, 0x000f), (UInt32)SENDREMARK.INVENTORY);

			/*
            // Create a timer that waits one second, then invokes every second.
            Xamarin.Forms.Device.StartTimer(TimeSpan.FromMilliseconds(2000), () => {
                _deviceHandler.SendAsync(0, 0, DOWNLINKCMD.RFIDCMD, PacketData(0, 0xf000, 0x000f), (UInt32)SENDREMARK.INVENTORY);
                return true;
            });

            */
		}

		public void StopOperation()
		{
			byte[] cmd = { 0x40, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

			_deviceHandler.SendAsync(0, 0, DOWNLINKCMD.RFIDCMD, cmd, (UInt32)SENDREMARK.GENERAL);
		}


#endregion

#if old
#region Basic Functions

		/// <summary>
		/// rfid reader packet
		/// </summary>
		/// <param name="RW"></param>
		/// <param name="add"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		byte[] PacketData(UInt16 add, UInt32? value = null)
		{
			byte[] CMDBuf = new byte[8];

			if (value == null)
			{
				CMDBuf[1] = 0x00;
				CMDBuf[4] = 0x00;
				CMDBuf[5] = 0x00;
				CMDBuf[6] = 0x00;
				CMDBuf[7] = 0x00;
			}
			else
			{
				CMDBuf[1] = 0x01;
				CMDBuf[4] = (byte)value;
				CMDBuf[5] = (byte)(value >> 8);
				CMDBuf[6] = (byte)(value >> 16);
				CMDBuf[7] = (byte)(value >> 24);
			}

			CMDBuf[0] = 0x70;
			CMDBuf[2] = (byte)add;
			CMDBuf[3] = (byte)((uint)add >> 8);

			return CMDBuf;
		}

		private void MacWriteRegister(MACREGISTER add, UInt32 value)
		{
			MacWriteRegister((UInt16)add, value);
		}

		public void MacReadRegister(MACREGISTER add)
		{
			MacReadRegister((UInt16)add);
		}

		bool MacReadRegister(MACREGISTER add, ref UInt32 data)
		{
			data = _registerData[(UInt16)add];
			return true;
		}

		private async void MacWriteRegister(UInt16 add, UInt32 value)
		{
			_registerData[add] = value;
			_deviceHandler.SendAsync(0, 0, DOWNLINKCMD.RFIDCMD, PacketData(add, value));
		}

		public async void MacReadRegister(UInt16 add)
		{
			_deviceHandler.SendAsync(0, 0, DOWNLINKCMD.RFIDCMD, PacketData(add), (UInt32)SENDREMARK.GETREGISTER);
		}
#endregion
#endif
		public void test()
		{
			byte[] data = new byte[11] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
			byte[] getData;

			_dataBuffer.DataIn(data);

			getData = _dataBuffer.DataOut(5);

			getData = _dataBuffer.DataOut(5);

			_dataBuffer.DataIn(data);

			getData = _dataBuffer.DataOut(5);

			getData = _dataBuffer.DataOut(5);

			getData = _dataBuffer.DataOut(5);
		}
	}
}
