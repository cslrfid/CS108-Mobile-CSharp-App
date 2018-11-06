using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSLibrary
{
    public partial class RFIDReader
    {
        UInt32[] _0000 = null;             // 0X0000~0X0002
        UInt32[] _0100 = null;
        UInt32[] _0200 = null;
        UInt32[] _0300 = null;
        UInt32[] _0400 = null;
        UInt32[] _0500 = null;             // 0x0500 ~ 0x0501
        UInt32[] _0600 = null;             // 0x0600 ~ 0x0603
        UInt32[] _0700 = null;             // 0x0700 ~ 0x0701
        UInt32[,] _0702_707 = null;     // 0x0702 ~ 0x0707
        UInt32[] _0800 = null;             // 0x0800
        UInt32[,] _0801_80c = null;    // 0x0800 ~ 0x080c
        UInt32[] _0900 = null;             // 0X0900 ~ 0X0902
        UInt32[,] _0903_906 = null;     // 0X0903 ~ 0X0906
        UInt32[] _0910_921 = null;        // 0X0910 ~ 0X0921
        UInt32[] _0a00 = null;            // 0X0a00 ~ 0x0a0f
        UInt32[] _0b00 = null;          // 0x0b00 ~ 0x0b84
        UInt32[] _0c01 = null;             // 0X0c01
        UInt32[,] _0c02_c07 = null;    // 0X0c02 ~ 0x0c07
        UInt32[] _0c08 = null;             // 0X0c08
        UInt32[] _0d00 = null;
        UInt32[] _0e00 = null;
        UInt32[] _0f00 = null;

        public bool MacRegisterInitialize()
        {
            /*
            _0000 = new UInt32[3];             // 0X0000~0X0002
            _0100 = new UInt32[0];
            _0200 = new UInt32[0];
            _0300 = new UInt32[0];              // 302, 304, 308 (Selector?)
            _0400 = new UInt32[0];              // 408 (Selector?)
            _0500 = new UInt32[2];             // 0x0500 ~ 0x0501
            _0600 = new UInt32[4];             // 0x0600 ~ 0x0603
            _0700 = new UInt32[2];             // 0x0700 ~ 0x0701 (Selector)
            _0702_707 = new UInt32[16, 6];     // 0x0702 ~ 0x0707
            _0800 = new UInt32[1];             // 0x0800 (Selector)
            _0801_80c = new UInt32[8, 12];     // 0x0800 ~ 0x080c
            _0900 = new UInt32[3];             // 0X0900 ~ 0X0902 (Selector)
            _0903_906 = new UInt32[4, 4];      // 0X0903 ~ 0X0906
            _0910_921 = new UInt32[12];        // 0X0910 ~ 0X0921 (Selector)
            _0a00 = new UInt32[16];            // 0X0a00 ~ 0x0a0f (Selector)
            _0b00 = new UInt32[0x85];          // 0x0b00 ~ 0x0b84
            _0c01 = new UInt32[2];             // 0X0c01 (Selector)
            _0c02_c07 = new UInt32[50, 6];     // 0X0c02 ~ 0x0c07
            _0c08 = new UInt32[1];             // 0X0c08
            _0d00 = new UInt32[0];
            _0e00 = new UInt32[0];
            _0f00 = new UInt32[0];

            _0700[0x00] = 0xffff;
            //_0700[0x02] = 0x0001;
            //_0700[0x05] = 0x07d0;
            //_0700[0x06] = 0x012c;
            _0900[0x00] = 0x00c0;
            _0900[0x01] = 0x0003;
            _0900[0x02] = 0x0003;
            //_0900[0x03] = 0x40f4;
            //_0900[0x05] = 0x0001;
            _0a00[0x01] = 0x0006;
            _0a00[0x02] = 0x0001;
            _0a00[0x03] = 0x0002;
            _0a00[0x04] = 0x0001;

            ReadReaderRegister(0x0000); // Get RFID Reader Firmware version
            //ReadReaderRegister(0x0800);
            */



            _0000 = new UInt32[3];             // 0X0000~0X0002
            _0100 = new UInt32[0];
            _0200 = new UInt32[0];
            _0300 = new UInt32[0];              // 302, 304, 308 (Selector?)

            _0302 = new UInt32[2];              // (Selector)
            _0303 = new UInt32[11];

            _0304 = new UInt32[2];              // (Selector)

            _0305 = new UInt32[2];              // (Selector)

            _0308 = new UInt32[2];              // (Selector)
            _0309 = new UInt32[8];              

            _0400 = new UInt32[0];              // 408 (Selector?)

            _0408 = new UInt32[2];              // (Selector)


            _0500 = new UInt32[2];             // 0x0500 ~ 0x0501
            _0600 = new UInt32[4];             // 0x0600 ~ 0x0603
            _0700 = new UInt32[1];             // 0x0700 

            _0701 = new UInt32[2];              // (Selector)
            _0702_707 = new UInt32[16, 6];     // 0x0702 ~ 0x0707

            _0800 = new UInt32[2];             // (Selector)
            _0801_80c = new UInt32[8, 12];     // 0x0800 ~ 0x080c

            _0900 = new UInt32[3];             // 0X0900 ~ 0X0901

            _0902 = new UInt32[2];              // Selector
            _0903_906 = new UInt32[4, 4];      // 0X0903 ~ 0X0906

            _0910_921 = new UInt32[12];        // 0X0910 ~ 0X0921

            _0a00 = new UInt32[16];            // 0X0a00 ~ 0x0a0f

            _0a08 = new UInt32[2];              // Selector
            _0a09_a18 = new UInt32[8];        // 0X0a09 ~ 0X0a18

            _0b00 = new UInt32[0x85];          // 0x0b00 ~ 0x0b84
            _0c01 = new UInt32[2];             // 0X0c01 (Selector)
            _0c02_c07 = new UInt32[50, 6];     // 0X0c02 ~ 0x0c07
            _0c08 = new UInt32[1];             // 0X0c08
            _0d00 = new UInt32[0];
            _0e00 = new UInt32[0];
            _0f00 = new UInt32[0];

            _0700[0x00] = 0xffff;
            //_0700[0x02] = 0x0001;
            //_0700[0x05] = 0x07d0;
            //_0700[0x06] = 0x012c;
            _0900[0x00] = 0x00c0;
            _0900[0x01] = 0x0003;
            _0900[0x02] = 0x0003;
            //_0900[0x03] = 0x40f4;
            //_0900[0x05] = 0x0001;
            _0a00[0x01] = 0x0006;
            _0a00[0x02] = 0x0001;
            _0a00[0x03] = 0x0002;
            _0a00[0x04] = 0x0001;

            ReadReaderRegister(0x0000); // Get RFID Reader Firmware version
            //ReadReaderRegister(0x0800);



            return true;
        }

/*            public bool MacRegisterInitialize (uint model, uint country, uint specialVersion, uint oemVersion)
        {
            switch (model)
            {
                case 1:
                    break;
                case 2:
                    break;
                case 4:
                    break;
                case 7:
                    break;
                case 8:
                    break;
                case 9:
                    break;

                default:
                    break;
            }

            ReadReaderRegister(0x000);                                          // Get RFID Reader Firmware version

            //ReadReaderRegister((UInt16)MACREGISTER.HST_ANT_DESC_RFPOWER);      // Get Antenna 0 Power Level            
            // ReadReaderRegister((UInt16)MACREGISTER.HST_ANT_CYCLES);
            // MacWriteRegister(MACREGISTER.HST_ANT_DESC_SEL, 0);  // Set Antenna 0 
            // ReadReaderRegister((UInt16)MACREGISTER.HST_ANT_DESC_RFPOWER);   // Get Antenna 0 Power Level
            // ReadReaderRegister((UInt16)MACREGISTER.HST_ANT_DESC_DWELL);
            // ReadReaderRegister((UInt16)MACREGISTER.HST_QUERY_CFG);
            // ReadReaderRegister((UInt16)MACREGISTER.HST_INV_CFG);
            // ReadReaderRegister((UInt16)MACREGISTER.HST_INV_EPC_MATCH_CFG);
            // ReadReaderRegister((UInt16)MACREGISTER.HST_TAGACC_DESC_CFG);
            // ReadReaderRegister(0x005); // reader mac error register

            return false;
        }
*/

        private void ReadReaderRegister(UInt16 add)
        {
            _deviceHandler.SendAsync(0, 0, DOWNLINKCMD.RFIDCMD, PacketData(add), HighLevelInterface.BTWAITCOMMANDRESPONSETYPE.WAIT_BTAPIRESPONSE_DATA1);
        }

        /// <summary>
        /// for compatible with old library
        /// </summary>
        /// <param name="add"></param>
        /// <param name="value"></param>
        private void MacWriteRegister(MACREGISTER add, UInt32 value)
        {
            Plugin.BLE.Abstractions.Trace.Message("MAC Write {0:X}:{1:X}", add, value);
            WriteMacRegister((UInt16)add, value);
            //MacWriteRegister((UInt16)add, value);
        }

        private bool MacReadRegister(MACREGISTER add, ref UInt32 data)
        {
            data = ReadMacRegister ((UInt16)add);
            Plugin.BLE.Abstractions.Trace.Message("MAC Read {0:X}:{1:X}", add, data);
            return true;
        }

        public UInt32 ReadMacRegister(UInt16 address)
        {
            UInt16 addressBench = (UInt16)(address & 0x0f00U);
            UInt16 addressoffset = (UInt16)(address & 0x00ffU);

            try
            {   switch (addressBench)
                {
                    case 0x0000:
                        if (addressoffset == 0x0000)
                        {
                            return _0000[addressoffset];
                        }
                        else
                        {
                            ReadReaderRegister(address);
                        }
                        break;

                    case 0x0100:
                        return _0100[addressoffset];
                        break;

                    case 0x0200:
                        return _0200[addressoffset];
                        break;

                    case 0x0300:
                        return _0300[addressoffset];
                        break;

                    case 0x0400:
                        return _0400[addressoffset];
                        break;

                    case 0x0500:
                        return _0500[addressoffset];
                        break;

                    case 0x0600:
                        return _0600[addressoffset];
                        break;

                    case 0x0700:
                        if (addressoffset >= 0x0002 && addressoffset <= 0x0007)
                        {
                            addressoffset -= 2;
                            return _0702_707[_0700[0x0001], addressoffset];
                        }
                        else
                        {
                            return _0700[addressoffset];
                        }
                        break;

                    case 0x0800:
                        if (addressoffset >= 0x0001 && addressoffset <= 0x000c)
                        {
                            addressoffset -= 1;
                            return _0801_80c[_0800[0x0000], addressoffset];
                        }
                        else
                        {
                            return _0800[addressoffset];
                        }
                        break;

                    case 0x0900:
                        if (addressoffset >= 0x0003 && addressoffset <= 0x0006)
                        {
                            addressoffset -= 3;
                            return _0903_906[_0900[0x0002], addressoffset];
                        }
                        else if (addressoffset >= 0x0010 && addressoffset <= 0x0021)
                        {
                            addressoffset -= 0x0010;
                            return _0910_921 [addressoffset];
                        }
                        else
                        {
                            return _0900[addressoffset];
                        }
                        break;

                    case 0x0a00:
                        return _0a00[addressoffset];
                        break;

                    case 0x0b00:
                        return _0b00[addressoffset];
                        break;

                    case 0x0c00:
                        if (addressoffset >= 0x0002 && addressoffset <= 0x0007)
                        {
                            addressoffset -= 2;
                            return _0c02_c07[_0c01[0x0000], addressoffset];
                        }
                        else
                        {
                            switch (addressoffset)
                            {
                                case 0x0001:
                                    return _0c01[0x00];
                                    break;

                                case 0x0008:
                                    return _0c08[0x00];
                                    break;
                            }
                        }
                        break;

                    case 0x0d00:
                        return _0d00[addressoffset];
                        break;

                    case 0x0e00:
                        return _0e00[addressoffset];
                        break;

                    case 0x0f00:
                        return _0f00[addressoffset];
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Plugin.BLE.Abstractions.Trace.Message(ex.Message);
            }

            return 0;
        }

        public bool SaveMacRegister (UInt16 address, UInt32 data)
        {
            if (address > 0x0001)
                return false;

            UInt16 addressoffset = (UInt16)(address & 0x00ffU);

            _0000[addressoffset] = data;
            return true;
        }

        public void WriteMacRegister(UInt16 address, UInt32 data)
        {
            UInt16 addressBench = (UInt16)(address & 0x0f00U);
            UInt16 addressoffset = (UInt16)(address & 0x00ffU);

            try
            {
                switch (addressBench)
                {
                    case 0x0000:
                        if (data != _0000[addressoffset])
                        {
                            _0000[addressoffset] = data;
                            _deviceHandler.SendAsync(0, 0, DOWNLINKCMD.RFIDCMD, PacketData(address, data), HighLevelInterface.BTWAITCOMMANDRESPONSETYPE.BTAPIRESPONSE);
                        }
                        break;

                    case 0x0100:
                        if (data != _0100[addressoffset])
                        {
                            _0100[addressoffset] = data;
                            _deviceHandler.SendAsync(0, 0, DOWNLINKCMD.RFIDCMD, PacketData(address, data), HighLevelInterface.BTWAITCOMMANDRESPONSETYPE.BTAPIRESPONSE);
                        }
                        break;

                    case 0x0200:
                        if (data != _0200[addressoffset])
                        {
                            _0200[addressoffset] = data;
                            _deviceHandler.SendAsync(0, 0, DOWNLINKCMD.RFIDCMD, PacketData(address, data), HighLevelInterface.BTWAITCOMMANDRESPONSETYPE.BTAPIRESPONSE);
                        }
                        break;

                    case 0x0300:
                        if (data != _0300[addressoffset])
                        {
                            _0300[addressoffset] = data;
                            _deviceHandler.SendAsync(0, 0, DOWNLINKCMD.RFIDCMD, PacketData(address, data), HighLevelInterface.BTWAITCOMMANDRESPONSETYPE.BTAPIRESPONSE);
                        }
                        break;

                    case 0x0400:
                        if (data != _0400[addressoffset])
                        {
                            _0400[addressoffset] = data;
                            _deviceHandler.SendAsync(0, 0, DOWNLINKCMD.RFIDCMD, PacketData(address, data), HighLevelInterface.BTWAITCOMMANDRESPONSETYPE.BTAPIRESPONSE);
                        }
                        break;

                    case 0x0500:
                        if (data != _0500[addressoffset])
                        {
                            _0500[addressoffset] = data;
                            _deviceHandler.SendAsync(0, 0, DOWNLINKCMD.RFIDCMD, PacketData(address, data), HighLevelInterface.BTWAITCOMMANDRESPONSETYPE.BTAPIRESPONSE);
                        }
                        break;

                    case 0x0600:
                        if (data != _0600[addressoffset])
                        {
                            _0600[addressoffset] = data;
                            _deviceHandler.SendAsync(0, 0, DOWNLINKCMD.RFIDCMD, PacketData(address, data), HighLevelInterface.BTWAITCOMMANDRESPONSETYPE.BTAPIRESPONSE);
                        }
                        break;

                    case 0x0700:
                        if (addressoffset >= 0x0002 && addressoffset <= 0x0007)
                        {
                            addressoffset -= 2;
                            if (data != _0702_707[_0700[0x0001], addressoffset])
                            {
                                _0702_707[_0700[0x0001], addressoffset] = data;
                                _deviceHandler.SendAsync(0, 0, DOWNLINKCMD.RFIDCMD, PacketData(address, data), HighLevelInterface.BTWAITCOMMANDRESPONSETYPE.BTAPIRESPONSE);
                            }
                        }
                        else if (data != _0700[addressoffset] || addressoffset == 0x0001)
                        {
                            _0700[addressoffset] = data;
                            _deviceHandler.SendAsync(0, 0, DOWNLINKCMD.RFIDCMD, PacketData(address, data), HighLevelInterface.BTWAITCOMMANDRESPONSETYPE.BTAPIRESPONSE);
                        }
                        break;

                    case 0x0800:
                        if (addressoffset >= 0x0001 && addressoffset <= 0x000c)
                        {
                            addressoffset -= 1;
                            if (data != _0801_80c[_0800[0x0000], addressoffset])
                            {
                                _0801_80c[_0800[0x0000], addressoffset] = data;
                                _deviceHandler.SendAsync(0, 0, DOWNLINKCMD.RFIDCMD, PacketData(address, data), HighLevelInterface.BTWAITCOMMANDRESPONSETYPE.BTAPIRESPONSE);
                            }
                        }
                        else 
                        {
                            _0800[addressoffset] = data;
                            _deviceHandler.SendAsync(0, 0, DOWNLINKCMD.RFIDCMD, PacketData(address, data), HighLevelInterface.BTWAITCOMMANDRESPONSETYPE.BTAPIRESPONSE);
                        }
                        break;

                    case 0x0900:
                        if (addressoffset >= 0x0003 && addressoffset <= 0x0006)
                        {
                            addressoffset -= 3;
                            if (data != _0903_906[_0900[0x0002], addressoffset])
                            {
                                _0903_906[_0900[0x0002], addressoffset] = data;
                                _deviceHandler.SendAsync(0, 0, DOWNLINKCMD.RFIDCMD, PacketData(address, data), HighLevelInterface.BTWAITCOMMANDRESPONSETYPE.BTAPIRESPONSE);
                            }
                        }
                        else if (addressoffset >= 0x0010 && addressoffset <= 0x0021)
                        {
                            addressoffset -= 0x0010;
                            if (data != _0910_921[addressoffset])
                            {
                                _0910_921[addressoffset] = data;
                                _deviceHandler.SendAsync(0, 0, DOWNLINKCMD.RFIDCMD, PacketData(address, data), HighLevelInterface.BTWAITCOMMANDRESPONSETYPE.BTAPIRESPONSE);
                            }
                        }
                        else if (data != _0900[addressoffset] || addressoffset == 0x0002)
                        {
                            _0900[addressoffset] = data;
                            _deviceHandler.SendAsync(0, 0, DOWNLINKCMD.RFIDCMD, PacketData(address, data), HighLevelInterface.BTWAITCOMMANDRESPONSETYPE.BTAPIRESPONSE);
                        }
                        break;

                    case 0x0a00:
                        if (data != _0a00[addressoffset])
                        {
                            _0a00[addressoffset] = data;
                            _deviceHandler.SendAsync(0, 0, DOWNLINKCMD.RFIDCMD, PacketData(address, data), HighLevelInterface.BTWAITCOMMANDRESPONSETYPE.BTAPIRESPONSE);
                        }
                        break;

                    case 0x0b00:
                        if (data != _0b00[addressoffset])
                        {
                            _0b00[addressoffset] = data;
                            _deviceHandler.SendAsync(0, 0, DOWNLINKCMD.RFIDCMD, PacketData(address, data), HighLevelInterface.BTWAITCOMMANDRESPONSETYPE.BTAPIRESPONSE);
                        }
                        break;

                    case 0x0c00:
                        if (addressoffset >= 0x0002 && addressoffset <= 0x0007)
                        {
                            addressoffset -= 2;
                            if (data != _0c02_c07[_0c01[0x00], addressoffset])
                            {
                                _0c02_c07[_0c01[0x0000], addressoffset] = data;
                                _deviceHandler.SendAsync(0, 0, DOWNLINKCMD.RFIDCMD, PacketData(address, data), HighLevelInterface.BTWAITCOMMANDRESPONSETYPE.BTAPIRESPONSE);
                            }
                        }
                        else
                        {
                            switch (addressoffset)
                            {
                                case 0x0001:
                                    _0c01[0x00] = data;
                                    _deviceHandler.SendAsync(0, 0, DOWNLINKCMD.RFIDCMD, PacketData(address, data), HighLevelInterface.BTWAITCOMMANDRESPONSETYPE.BTAPIRESPONSE);
                                    break;

                                case 0x0008:
                                    if (data != _0c08[0x00])
                                    {
                                        _0c08[0x00] = data;
                                        _deviceHandler.SendAsync(0, 0, DOWNLINKCMD.RFIDCMD, PacketData(address, data), HighLevelInterface.BTWAITCOMMANDRESPONSETYPE.BTAPIRESPONSE);
                                    }
                                    break;
                            }
                        }
                        break;

                    case 0x0d00:
                        if (data != _0d00[addressoffset])
                        {
                            _0d00[addressoffset] = data;
                            _deviceHandler.SendAsync(0, 0, DOWNLINKCMD.RFIDCMD, PacketData(address, data), HighLevelInterface.BTWAITCOMMANDRESPONSETYPE.BTAPIRESPONSE);
                        }
                        break;

                    case 0x0e00:
                        if (data != _0e00[addressoffset])
                        {
                            _0e00[addressoffset] = data;
                            _deviceHandler.SendAsync(0, 0, DOWNLINKCMD.RFIDCMD, PacketData(address, data), HighLevelInterface.BTWAITCOMMANDRESPONSETYPE.BTAPIRESPONSE);
                        }
                        break;

                    case 0x0f00:
                        _0f00[addressoffset] = data;
                        _deviceHandler.SendAsync(0, 0, DOWNLINKCMD.RFIDCMD, PacketData(address, data), HighLevelInterface.BTWAITCOMMANDRESPONSETYPE.BTAPIRESPONSE);
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Plugin.BLE.Abstractions.Trace.Message(ex.Message);
            }
        }


    }
}
