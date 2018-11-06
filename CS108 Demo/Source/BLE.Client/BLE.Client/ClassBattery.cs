using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLE.Client
{
#if oldMode

    14% Battery Life Left, Please Recharge CS108 or Replace with Freshly Charged CS108B
#else


    public static class ClassBattery
    {
        public enum BATTERYMODE
        {
            INVENTORY = 1,
            IDLE = 2,
        }

        public enum BATTERYLEVELSTATUS
        {
            NORMAL = 0,
            LOW = 1,
            LOW_17 = 2
        }

        // battery table for PCB version below of 1.7
        readonly static double voltageFirstOffset0 = 100.0 / 90 * 5;
        readonly static double[] voltageTable0 = new double[] { 3.4, 3.457, 3.468, 3.489, 3.494, 3.515, 3.541, 3.566, 3.578, 3.610, 3.615, 3.668, 3.7, 3.731, 3.753, 3.790, 3.842, 3.879, 4.0 };
        readonly static double[] voltageSlope0 = new double[voltageTable0.Length - 1];
        readonly static double voltagestep0 = (100.0 - voltageFirstOffset0) / (voltageTable0.Length - 2);

        // battery table for PCB version of 1.7 or above and inventory mode
        readonly static double voltageFirstOffset1 = 100.0 / 134 * 4;
        readonly static double[] voltageTable1 = new double[] { 2.789, 3.304, 3.452, 3.489, 3.515, 3.534, 3.554, 3.563, 3.578, 3.584, 3.594, 3.61, 3.625, 3.652, 3.652, 3.673, 3.7, 3.725, 3.747, 3.769, 3.8, 3.826, 3.858, 3.89, 3.972, 3.964, 4.001, 4.069 };
        readonly static double[] voltageSlope1 = new double[voltageTable1.Length - 1];
        readonly static double voltagestep1 = (100.0 - voltageFirstOffset1) / (voltageTable1.Length - 2);

        // battery table for PCB version of 1.7 or above and idle mode
        readonly static double voltageFirstOffset2 = 100.0 / 534 * 4;
        readonly static double[] voltageTable2 = new double[] { 2.322, 3.156, 3.452, 3.563, 3.605, 3.626, 3.631, 3.642, 3.652, 3.668, 3.679, 3.689, 3.700, 3.705, 3.710, 3.716, 3.721, 3.724, 3.726, 3.731, 3.737, 3.742, 3.747, 3.753, 3.758, 3.763, 3.774, 3.779, 3.784, 3.798, 3.805, 3.816, 3.826, 3.842, 3.853, 3.863, 3.879, 3.895, 3.906, 3.921, 3.937, 3.948, 3.964, 3.980, 4.001, 4.018, 4.032, 4.048, 4.064, 4.085, 4.097, 4.117, 4.138, 4.185, 4.190 };
        readonly static double[] voltageSlope2 = new double[voltageTable2.Length - 1];
        readonly static double voltagestep2 = (100.0 - voltageFirstOffset2) / (voltageTable2.Length - 2);

        static double voltageFirstOffset;
        static double[] voltageTable;
        static double[] voltageSlope;
        static double voltagestep;

        static BATTERYMODE _currentInventoryMode;

        static ClassBattery()
        {
            int cnt;

            for (cnt = 0; cnt < voltageTable0.Length - 1; cnt++)
                voltageSlope0[cnt] = voltagestep0 / (voltageTable0[cnt + 1] - voltageTable0[cnt]);

            for (cnt = 0; cnt < voltageTable1.Length - 1; cnt++)
                voltageSlope1[cnt] = voltagestep1 / (voltageTable1[cnt + 1] - voltageTable1[cnt]);

            for (cnt = 0; cnt < voltageTable2.Length - 1; cnt++)
                voltageSlope2[cnt] = voltagestep2 / (voltageTable2[cnt + 1] - voltageTable2[cnt]);

            SetBatteryMode(BATTERYMODE.IDLE);
        }

        public static void SetBatteryMode(BATTERYMODE bm)
        {
            _currentInventoryMode = bm;

            if (string.Compare (BleMvxApplication._reader.siliconlabIC.GetPCBVersion(), "180") < 0 )
            {
                voltageFirstOffset = voltageFirstOffset0;
                voltageTable = voltageTable0;
                voltageSlope = voltageSlope0;
                voltagestep = voltagestep0;
            }
            else
            {
                if (bm == BATTERYMODE.INVENTORY)
                {
                    voltageFirstOffset = voltageFirstOffset1;
                    voltageTable = voltageTable1;
                    voltageSlope = voltageSlope1;
                    voltagestep = voltagestep1;
                }
                else
                {
                    voltageFirstOffset = voltageFirstOffset2;
                    voltageTable = voltageTable2;
                    voltageSlope = voltageSlope2;
                    voltagestep = voltagestep2;
                }
            }
        }

        public static BATTERYLEVELSTATUS BatteryLow (double voltage)
        {
            if (string.Compare(BleMvxApplication._reader.siliconlabIC.GetPCBVersion(), "180") < 0)
            {
                if (_currentInventoryMode == BATTERYMODE.INVENTORY)
                {
                    if (voltage <= 3.45)
                    {
                        return BATTERYLEVELSTATUS.LOW_17;
                    }
                }
                else
                {
                    if (voltage <= 3.6)
                    {
                        return BATTERYLEVELSTATUS.LOW_17;
                    }
                }
            }
            else
            {
                if (_currentInventoryMode == BATTERYMODE.INVENTORY)
                {
                    if (voltage <= 3.515)
                    {
                        return BATTERYLEVELSTATUS.LOW;
                    }
                }
                else
                {
                    if (voltage <= 3.652)
                    {
                        return BATTERYLEVELSTATUS.LOW;
                    }
                }
            }

            return BATTERYLEVELSTATUS.NORMAL;
        }

        public static double Voltage2Percent(double voltage)
        {
            int cnt;

            if (voltage > voltageTable[0])
            {
                if (voltage > voltageTable[1])
                {
                    for (cnt = voltageTable.Length - 1; cnt >= 0; cnt--)
                    {
                        if (voltage > voltageTable[cnt])
                        {
                            if (cnt == voltageTable.Length - 1)
                                return 100;

                            double percent = 0;

                            percent = (voltagestep * (cnt - 1) + voltageFirstOffset) + ((voltage - voltageTable[cnt]) * voltageSlope[cnt]);

                            return percent;
                        }
                    }
                }
                else
                {
                    double percent = ((voltage - voltageTable[0]) * voltageSlope[0]);
                    return percent;
                }
            }

            return 0;
        }
    }
#endif
}
