#region Assembly Flex1500USB, Version=2.3.2.1, Culture=neutral, PublicKeyToken=null
// C:\Users\RADIO\source\PowerSDR_v2.8.0\Source\bin\Release\Flex1500USB.dll
// Decompiled with ICSharpCode.Decompiler 6.1.0.5902
#endregion

using Jungo.flex1500_lib;
using Jungo.wdapi_dotnet;
using LockFreeLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace PowerSDR
{
    public class Flex1500USB
    {
        public class radio
        {
            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            private struct message
            {
                public byte index;

                public uint opcode;

                public uint param1;

                public uint param2;

                public uint result;

                public byte[] barray()
                {
                    return new byte[20]
                    {
                        index,
                        0,
                        0,
                        0,
                        BitConverter.GetBytes(opcode)[3],
                        BitConverter.GetBytes(opcode)[2],
                        BitConverter.GetBytes(opcode)[1],
                        BitConverter.GetBytes(opcode)[0],
                        BitConverter.GetBytes(param1)[3],
                        BitConverter.GetBytes(param1)[2],
                        BitConverter.GetBytes(param1)[1],
                        BitConverter.GetBytes(param1)[0],
                        BitConverter.GetBytes(param2)[3],
                        BitConverter.GetBytes(param2)[2],
                        BitConverter.GetBytes(param2)[1],
                        BitConverter.GetBytes(param2)[0],
                        0,
                        0,
                        0,
                        0
                    };
                }
            }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            private struct EEPROMmessage
            {
                public byte index;

                public ushort offset;

                public uint opcode;

                public byte num_bytes;

                public byte[] bytes;

                public byte[] b()
                {
                    int num = (bytes != null) ? (8 + bytes.Length) : 8;
                    byte[] array = new byte[num];
                    array[0] = index;
                    array[2] = (byte)(offset & 0xFF);
                    array[1] = (byte)((offset & 0xFF00) >> 8);
                    if (bytes != null)
                    {
                        array[3] = (byte)bytes.Length;
                    }
                    else
                    {
                        array[3] = num_bytes;
                    }

                    array[4] = BitConverter.GetBytes(opcode)[3];
                    array[5] = BitConverter.GetBytes(opcode)[2];
                    array[6] = BitConverter.GetBytes(opcode)[1];
                    array[7] = BitConverter.GetBytes(opcode)[0];
                    if (bytes != null)
                    {
                        Array.Copy(bytes, 0, array, 8, bytes.Length);
                    }

                    return array;
                }
            }

            public enum Opcode
            {
                USB_OP_CREATE_INTERFACE = 1000,
                USB_OP_START_INTERFACE = 1001,
                USB_OP_SET_CLOCK_SOURCE = 1002,
                USB_OP_SET_ROUTE = 1003,
                USB_OP_GET_CURRENT_STATUS = 1004,
                USB_OP_IS_LOCKED = 1005,
                USB_OP_GET_CURRENT_CONFIG = 1006,
                USB_OP_INSTALL_CALLBACK = 1007,
                USB_OP_UNINSTALL_CALLBACK = 1008,
                USB_OP_GLOBAL_MUTE = 1009,
                USB_OP_TOGGLE_LED = 1010,
                USB_OP_GET_EVM_UI = 1011,
                USB_OP_SET_OUT_CHANNEL_VOL = 1012,
                USB_OP_SET_IN_CHANNEL_VOL = 1013,
                USB_OP_SET_OUT_CHANNEL_MUTE = 1014,
                USB_OP_SET_IN_CHANNEL_MUTE = 1015,
                USB_OP_GET_IN_CHANNEL_VOL = 1016,
                USB_OP_GET_OUT_CHANNEL_VOL = 1017,
                USB_OP_NOOP = 1018,
                USB_OP_I2C_WRITE_VALUE = 1019,
                USB_OP_I2C_WRITE_2_VALUE = 1020,
                USB_OP_I2C_READ_VALUE = 1021,
                USB_OP_REG_GET = 1022,
                USB_OP_REG_SET = 0x3FF,
                USB_OP_SET_SAMPLE_RATE = 0x400,
                USB_OP_GET_RX_NUM = 1025,
                USB_OP_GET_RX_PRESENT_MASK = 1026,
                USB_OP_GET_RX_ENABLE_MASK = 1027,
                USB_OP_SET_RX_ENABLE_MASK = 1028,
                USB_OP_GET_TX_NUM = 1029,
                USB_OP_GET_TX_PRESENT_MASK = 1030,
                USB_OP_GET_TX_ENABLE_MASK = 1031,
                USB_OP_SET_TX_ENABLE_MASK = 1032,
                USB_OP_SET_DDS_CHAN = 1033,
                USB_OP_SET_DDS_FREQ = 1034,
                USB_OP_SET_DDS_AMPLITUDE = 1035,
                USB_OP_SET_PREAMP_ON = 1036,
                USB_OP_SET_ATT_ON = 1037,
                USB_OP_SET_ATT_VAL = 1038,
                USB_OP_SET_LED = 1039,
                USB_OP_SET_FILTER = 1040,
                USB_OP_READ_CLOCK_GEN = 1041,
                USB_OP_WRITE_CLOCK_GEN = 1042,
                USB_OP_READ_DDS = 1043,
                USB_OP_WRITE_DDS = 1044,
                USB_OP_UPDATE_DDS = 1045,
                USB_OP_READ_GPIO = 1046,
                USB_OP_WRITE_GPIO = 1047,
                USB_OP_READ_GPIO_DDR = 1048,
                USB_OP_WRITE_GPIO_DDR = 1049,
                USB_OP_SET_TEST_RELAY = 1050,
                USB_OP_SET_SIG_GEN_RELAY = 1051,
                USB_OP_SET_ADC_MASTER = 1052,
                USB_OP_GET_ADC_CAL_STATE = 1053,
                USB_OP_SET_ADC_HIGH_PASS_FILTER = 1054,
                USB_OP_SET_ADC_ZCAL = 1055,
                USB_OP_SET_ADC_RESET = 1056,
                USB_OP_SET_ENABLE_QSD = 1057,
                USB_OP_SET_POWER_DOWN1 = 1058,
                USB_OP_SET_POWER_DOWN2 = 1059,
                USB_OP_GET_PLL_STATUS = 1060,
                USB_OP_SET_DDS_MASTER = 1061,
                USB_OP_SET_PLL_ON = 1062,
                USB_OP_SET_IMPULSE_ON = 1063,
                USB_OP_SET_PORT = 1064,
                USB_OP_WRITE_SPI = 1065,
                USB_OP_REBOOT = 1066,
                USB_OP_GET_FIRMWARE_REV = 1200,
                USB_OP_GET_TRX_OK = 1201,
                USB_OP_GET_TRX_REV = 1202,
                USB_OP_GET_TRX_SN = 1203,
                USB_OP_GET_PA_OK = 1204,
                USB_OP_GET_PA_REV = 1205,
                USB_OP_GET_PA_SN = 1206,
                USB_OP_GET_RFIO_OK = 1207,
                USB_OP_GET_RFIO_REV = 1208,
                USB_OP_GET_RFIO_SN = 1209,
                USB_OP_GET_ATU_OK = 1210,
                USB_OP_GET_ATU_REV = 1211,
                USB_OP_GET_ATU_SN = 1212,
                USB_OP_GET_RX2_OK = 1213,
                USB_OP_GET_RX2_REV = 1214,
                USB_OP_GET_RX2_SN = 1215,
                USB_OP_GET_VU_OK = 1216,
                USB_OP_GET_VU_REV = 1217,
                USB_OP_GET_VU_SN = 1218,
                USB_OP_INITIALIZE = 1219,
                USB_OP_GET_SERIAL_NUM = 1220,
                USB_OP_GET_MODEL = 1221,
                USB_OP_READ_TRX_EEPROM_UINT8 = 1222,
                USB_OP_WRITE_TRX_EEPROM_UINT8 = 1223,
                USB_OP_READ_TRX_EEPROM_UINT16 = 1224,
                USB_OP_WRITE_TRX_EEPROM_UINT16 = 1225,
                USB_OP_READ_TRX_EEPROM_UINT32 = 1226,
                USB_OP_WRITE_TRX_EEPROM_UINT32 = 1227,
                USB_OP_READ_CLOCK_REG = 1228,
                USB_OP_WRITE_CLOCK_REG = 1229,
                USB_OP_READ_TRX_DDS_REG = 1230,
                USB_OP_WRITE_TRX_DDS_REG = 1231,
                USB_OP_READ_PIO_REG = 1232,
                USB_OP_WRITE_PIO_REG = 1233,
                USB_OP_TRX_POT_GET_RDAC = 1234,
                USB_OP_TRX_POT_SET_RDAC = 1235,
                USB_OP_PA_POT_GET_RDAC = 1236,
                USB_OP_PA_POT_SET_RDAC = 1237,
                USB_OP_SET_MUX = 1241,
                USB_OP_READ_CODEC_REG = 1242,
                USB_OP_WRITE_CODEC_REG = 1243,
                USB_OP_SET_RX1_FREQ = 1244,
                USB_OP_SET_RX2_FREQ = 1245,
                USB_OP_SET_TX_FREQ = 1246,
                USB_OP_SET_TRX_PREAMP = 1247,
                USB_OP_SET_TEST = 1248,
                USB_OP_SET_GEN = 1249,
                USB_OP_SET_SIG = 1250,
                USB_OP_SET_IMPULSE = 1251,
                USB_OP_SET_XVEN = 1252,
                USB_OP_SET_XVTXEN = 1253,
                USB_OP_SET_QSD = 1254,
                USB_OP_SET_QSE = 1255,
                USB_OP_SET_XREF = 1256,
                USB_OP_SET_RX1_FILTER = 1257,
                USB_OP_SET_RX2_FILTER = 1258,
                USB_OP_SET_TX_FILTER = 1259,
                USB_OP_SET_PA_FILTER = 1260,
                USB_OP_SET_INT_SPKR = 1261,
                USB_OP_SET_RX1_TAP = 1262,
                USB_OP_BYPASS_RX1_FILTER = 1263,
                USB_OP_BYPASS_TX_FILTER = 1264,
                USB_OP_BYPASS_PA_FILTER = 1265,
                USB_OP_READ_PTT = 1266,
                USB_OP_SET_HEADPHONE = 1267,
                USB_OP_SET_PLL = 1268,
                USB_OP_SET_RCA_TX1 = 1269,
                USB_OP_SET_RCA_TX2 = 1270,
                USB_OP_SET_RCA_TX3 = 1271,
                USB_OP_SET_FAN = 1272,
                USB_OP_GET_PLL_STATUS2 = 1273,
                USB_OP_READ_PA_ADC = 1274,
                USB_OP_SET_RX1_LOOP = 1275,
                USB_OP_SET_TR = 1276,
                USB_OP_SET_ANT = 1277,
                USB_OP_SET_RX1_ANT = 1278,
                USB_OP_SET_TX_ANT = 1279,
                USB_OP_SET_TXMON = 1280,
                USB_OP_SET_XVCOM = 1281,
                USB_OP_SET_EN_2M = 1282,
                USB_OP_SET_KEY_2M = 1283,
                USB_OP_SET_XVTR = 1284,
                USB_OP_SET_PA_BIAS = 1285,
                USB_OP_SET_POWER_OFF = 1286,
                USB_OP_SET_FPLED = 1287,
                USB_OP_SET_CTS = 1288,
                USB_OP_SET_RTS = 1289,
                USB_OP_SET_PC_RESET = 1290,
                USB_OP_SET_PC_PWRBT = 1291,
                USB_OP_SET_MOX = 1292,
                USB_OP_SET_INT_LED = 1293,
                USB_OP_ATU_SEND_CMD = 1294,
                USB_OP_ATU_GET_RESULT = 1295,
                USB_OP_SET_FULL_DUPLEX = 1296,
                USB_OP_SET_TX_DAC = 1297,
                USB_OP_SET_AMP_TX1 = 1298,
                USB_OP_SET_AMP_TX2 = 1299,
                USB_OP_SET_AMP_TX3 = 1300,
                USB_OP_SET_XVTR_ACTIVE = 1301,
                USB_OP_SET_XVTR_SPLIT = 1302,
                USB_OP_SET_RX1OUT = 1303,
                USB_OP_FLEXWIRE_WRITE_VALUE = 1304,
                USB_OP_FLEXWIRE_WRITE_2_VALUE = 1305,
                USB_OP_SET_RX1_DSP_MODE = 1306,
                USB_OP_SET_RX2_ANT = 1307,
                USB_OP_READ_RX2_EEPROM_UINT8 = 1308,
                USB_OP_WRITE_RX2_EEPROM_UINT8 = 1309,
                USB_OP_READ_RX2_EEPROM_UINT16 = 1310,
                USB_OP_WRITE_RX2_EEPROM_UINT16 = 1311,
                USB_OP_READ_RX2_EEPROM_UINT32 = 1312,
                USB_OP_WRITE_RX2_EEPROM_UINT32 = 1313,
                USB_OP_READ_RX2_DDS_REG = 1314,
                USB_OP_WRITE_RX2_DDS_REG = 1315,
                USB_OP_SET_RX2_ON = 1319,
                USB_OP_SET_STANDBY = 1320,
                USB_OP_BYPASS_RX2_FILTER = 1321,
                USB_OP_SET_AMP_TX1_DELAY_ENABLE = 1322,
                USB_OP_SET_AMP_TX2_DELAY_ENABLE = 1323,
                USB_OP_SET_AMP_TX3_DELAY_ENABLE = 1324,
                USB_OP_SET_AMP_TX1_DELAY = 1325,
                USB_OP_SET_AMP_TX2_DELAY = 1326,
                USB_OP_SET_AMP_TX3_DELAY = 1327,
                USB_OP_RESET_RX2_DDS = 1328,
                USB_OP_SET_RX2_PREAMP = 1329,
                USB_OP_SET_RX2_DSP_MODE = 1330,
                USB_OP_SET_TRX_POT = 1331,
                USB_OP_SET_IAMBIC = 1332,
                USB_OP_SET_BREAK_IN = 1333,
                USB_OP_SET_MANUAL_RX1_FILTER = 1334,
                USB_OP_SET_MANUAL_RX2_FILTER = 1335,
                USB_OP_SET_EEPROM_WC = 1336,
                USB_OP_SET_HIZ = 1337,
                USB_OP_ENABLE_ATU = 1338,
                USB_OP_SET_ATU_ATTN = 1339,
                USB_OP_SET_PDRVMON = 1340,
                USB_OP_SET_RX_ATTN = 1341,
                USB_OP_FLEXWIRE_READ_VALUE = 1342,
                USB_OP_FLEXWIRE_READ_2_VALUE = 1343,
                USB_OP_SET_FAN_PWM = 1344,
                USB_OP_SET_FAN_SPEED = 1345,
                USB_OP_SYNC_PHASE = 1346,
                USB_OP_SET_RX1_FREQ_TW = 1347,
                USB_OP_SET_RX2_FREQ_TW = 1348,
                USB_OP_SET_TX_FREQ_TW = 1349,
                USB_OP_GET_REGION = 1350,
                USB_OP_SET_TX_DSP_FILTER = 1351,
                USB_OP_SET_TX_OFFSET = 1352,
                USB_OP_SET_TX_DSP_MODE = 1353,
                USB_OP_SET_CW_PITCH = 1354,
                USB_OP_GET_STATUS = 1355,
                USB_OP_SET_VU_FAN_HIGH = 1356,
                USB_OP_SET_VU_KEY_V = 1357,
                USB_OP_SET_VU_TXIFU = 1358,
                USB_OP_SET_VU_KEY_U = 1359,
                USB_OP_SET_VU_RXURX2 = 1360,
                USB_OP_SET_VU_RX2U = 1361,
                USB_OP_SET_VU_RXIFU = 1362,
                USB_OP_SET_VU_K6 = 1363,
                USB_OP_SET_VU_K7 = 1364,
                USB_OP_SET_VU_K8 = 1365,
                USB_OP_SET_VU_K9 = 1366,
                USB_OP_SET_VU_K10 = 1367,
                USB_OP_SET_VU_K11 = 1368,
                USB_OP_SET_VU_K12 = 1369,
                USB_OP_SET_VU_K13 = 1370,
                USB_OP_SET_VU_K14 = 1371,
                USB_OP_SET_VU_RX2V = 1372,
                USB_OP_SET_VU_TX_U = 1373,
                USB_OP_SET_VU_TX_V = 1374
            }

            public FLEX1500_Device m_device;

            public FLEX1500_Pipe outUsbPipe;

            public FLEX1500_Pipe inUsbPipe;

            public FLEX1500_Pipe inAudioPipe;

            public FLEX1500_Pipe outAudioPipe;

            public bool m_PTT;

            public bool m_FlexwirePTT;

            public bool m_DOT;

            public bool m_DASH;

            private RingBufferByte m_rawInRB;

            private RingBufferByte m_rawOutRB;

            private RingBufferFloat m_RBInI;

            private RingBufferFloat m_RBInQ;

            private bool m_dither;

            private bool m_clip;

            private int m_applicationBufferSize;

            private int m_updatedApplicationBufferSize;

            private bool dataListening;

            private bool dataEnabled;

            private D_STATE_CHANGE_CALLBACK m_PTTCallback;

            private D_STATE_CHANGE_CALLBACK m_FlexwirePTTCallback;

            private D_STATE_CHANGE_CALLBACK m_DashChangeCallback;

            private D_STATE_CHANGE_CALLBACK m_DotChangeCallback;

            private D_INTERCHANGE_BUFFER_CALLBACK m_AudioInterchangeCallback;

            private IntPtr pInStream;

            private IntPtr pOutStream;

            public byte m_index;

            private bool inStreamOpen;

            private bool outStreamOpen;

            private StringBuilder x = new StringBuilder();

            private bool audio_buffer_flush_request;

            private int last_out_buf_size;

            private bool audioAbort;

            private AutoResetEvent audioHeartbeat;

            private static bool init_packet_size = true;

            private uint final_packet_size = 8u;

            private ArrayList elapsedTimeArray = new ArrayList();

            private ArrayList bufferInReadSpace = new ArrayList();

            private ArrayList bufferOutReadSpace = new ArrayList();

            private static Random rand = new Random();

            public uint FinalPacketSize
            {
                get
                {
                    return final_packet_size;
                }
                set
                {
                    uint num = final_packet_size;
                    final_packet_size = value;
                    if (!init_packet_size && final_packet_size != num)
                    {
                        updatePacketSize();
                    }
                }
            }

            public radio(FLEX1500_Device dev)
            {
                m_device = dev;
            }

            public int Init(D_STATE_CHANGE_CALLBACK tPTTCallback, D_STATE_CHANGE_CALLBACK tFlexwirePTTCallback, D_STATE_CHANGE_CALLBACK tDashChangeCallback, D_STATE_CHANGE_CALLBACK tDotChangeCallback, D_INTERCHANGE_BUFFER_CALLBACK tAudioInterchangeCallback, bool streamAudio, bool dither, bool clip, uint bufferSize)
            {
                m_dither = dither;
                m_clip = clip;
                m_PTTCallback = tPTTCallback;
                m_FlexwirePTTCallback = tFlexwirePTTCallback;
                m_DashChangeCallback = tDashChangeCallback;
                m_DotChangeCallback = tDotChangeCallback;
                m_AudioInterchangeCallback = tAudioInterchangeCallback;
                m_RBInI = new RingBufferFloat(8192);
                m_RBInQ = new RingBufferFloat(8192);
                m_rawInRB = new RingBufferByte(32768);
                m_rawOutRB = new RingBufferByte(32768);
                inStreamOpen = false;
                outStreamOpen = false;
                m_applicationBufferSize = (int)bufferSize;
                m_updatedApplicationBufferSize = (int)bufferSize;
                Logger.LogLine("Flex1500USB Init ---------");
                return 0;
            }

            public void changeApplicationBufferSize(uint bufferSize)
            {
                m_updatedApplicationBufferSize = (int)bufferSize;
                Logger.LogLine("changeApplicationBufferSize, " + bufferSize);
            }

            public void FlushAudioBuffers()
            {
                audio_buffer_flush_request = true;
            }

            public int GetCurrentLatency()
            {
                return last_out_buf_size + m_rawOutRB.ReadSpace();
            }

            public int StartAudioListener()
            {
                Logger.LogLine("Starting Audio Listener threads");
                audioAbort = false;
                audioHeartbeat = new AutoResetEvent(initialState: false);
                Thread thread = new Thread(HardwareIO);
                thread.Name = "HardwareIO";
                thread.IsBackground = true;
                thread.Priority = ThreadPriority.Highest;
                Thread thread2 = new Thread(AudioProcess);
                thread2.Name = "AudioProcess";
                thread2.IsBackground = true;
                thread2.Priority = ThreadPriority.Highest;
                thread2.Start();
                thread.Start();
                return 0;
            }

            public int StopAudioListener()
            {
                Logger.LogLine("StopAudioListener");
                audioAbort = true;
                return 0;
            }

            private void openInStream()
            {
                if (inStreamOpen)
                {
                    closeInStream();
                }

                Logger.LogLine("openInStream");
                uint dwOptions = 128u;
                pInStream = inAudioPipe.IsocStreamOpen(32768u, audioPacketSize, fBlocking: true, dwOptions, 0u);
                inStreamOpen = true;
            }

            private void openOutStream()
            {
                if (outStreamOpen)
                {
                    closeOutStream();
                }

                Logger.LogLine("openOutStream");
                uint dwOptions = 128u;
                pOutStream = outAudioPipe.IsocStreamOpen(32768u, audioPacketSize, fBlocking: false, dwOptions, 0u);
                outStreamOpen = true;
            }

            private void closeInStream()
            {
                if (pInStream != (IntPtr)0)
                {
                    inAudioPipe.IsocStreamClose(pInStream);
                    inStreamOpen = false;
                }

                Logger.LogLine("closeInStream");
                m_rawInRB.Reset();
                m_RBInI.Reset();
                m_RBInQ.Reset();
            }

            private void closeOutStream()
            {
                if (pOutStream != (IntPtr)0)
                {
                    outAudioPipe.IsocStreamClose(pOutStream);
                    outStreamOpen = false;
                }

                Logger.LogLine("closeOutStream");
                m_rawOutRB.Reset();
            }

            private void HardwareIO()
            {
                init_packet_size = true;
                openInStream();
                openOutStream();
                uint num = 0u;
                bool flag = false;
                int num2 = 0;
                int num3 = 0;
                byte[] array = new byte[audioPacketSize];
                byte[] array2 = new byte[audioPacketSize];
                int num4 = (int)Math.Ceiling((double)audioPacketSize / 192.0 / 50.0);
                for (int i = 0; i < num4; i++)
                {
                    uint bytesRead = 0u;
                    uint num5 = inAudioPipe.IsocStreamRead(pInStream, array, audioPacketSize, ref bytesRead);
                    uint bytesRead2 = 0u;
                    if (bytesRead == audioPacketSize && num5 == 0)
                    {
                        outAudioPipe.IsocStreamWrite(pOutStream, array2, bytesRead, ref bytesRead2);
                    }
                }

                updatePacketSize();
                init_packet_size = false;
                while (!audioAbort && !flag)
                {
                    if (audio_buffer_flush_request)
                    {
                        audio_buffer_flush_request = false;
                    }

                    uint bytesRead3 = 0u;
                    uint num6 = inAudioPipe.IsocStreamRead(pInStream, array, audioPacketSize, ref bytesRead3);
                    Logger.timer += (int)(audioPacketSize / 192u);
                    if (bytesRead3 == audioPacketSize && num6 == 0)
                    {
                        num2++;
                        if (num != 0)
                        {
                            num--;
                        }

                        if (m_rawInRB.WriteSpace() >= audioPacketSize)
                        {
                            m_rawInRB.Write(array, (int)audioPacketSize);
                        }

                        audioHeartbeat.Set();
                        if (m_rawOutRB.ReadSpace() > 4 * m_applicationBufferSize * 4)
                        {
                            while (m_rawOutRB.ReadSpace() > 2 * audioPacketSize)
                            {
                                m_rawOutRB.Read(array2, (int)audioPacketSize);
                                num3++;
                            }
                        }

                        if (m_rawOutRB.ReadSpace() % 2 != 0)
                        {
                            Logger.LogLine("m_rawOutRB Exception: Odd number of bytes available (" + m_rawOutRB.ReadSpace());
                            m_rawOutRB.Reset();
                        }

                        if (m_rawOutRB.ReadSpace() >= audioPacketSize)
                        {
                            int num7 = m_rawOutRB.Read(array2, (int)audioPacketSize);
                            if (num7 != audioPacketSize)
                            {
                                Logger.LogLine("m_rawOutRB Read Exception: tried to read " + audioPacketSize + ", but only got " + num7);
                            }
                        }
                        else
                        {
                            Array.Clear(array2, 0, (int)audioPacketSize);
                        }

                        uint bytesRead4 = 0u;
                        uint num8 = outAudioPipe.IsocStreamWrite(pOutStream, array2, audioPacketSize, ref bytesRead4);
                        if (num8 != 0 || bytesRead4 != audioPacketSize)
                        {
                            Logger.LogLine("IsocStreamWrite Exception, dwStatus=" + num8 + ", bytesTransferred=" + bytesRead4);
                            closeOutStream();
                            openOutStream();
                        }
                        else
                        {
                            num2--;
                        }
                    }
                    else
                    {
                        Logger.LogLine("IsocStreamRead Exception, dwStatus=" + num6 + ", bytesRead=" + bytesRead3);
                        num++;
                        closeInStream();
                        openInStream();
                        Thread.Sleep(1);
                        if (num > 100)
                        {
                            flag = true;
                        }
                    }
                }

                closeOutStream();
                closeInStream();
            }

            private void updatePacketSize()
            {
                lock (this)
                {
                    closeOutStream();
                    closeInStream();
                    audioPacketSize = final_packet_size * 192;
                    openInStream();
                    openOutStream();
                    m_rawOutRB.Reset();
                    Logger.LogLine("Updated packet size to " + final_packet_size * 192);
                }
            }

            private void AudioProcess()
            {
                bool flag = true;
                while (!audioAbort)
                {
                    Logger.LogLine("");
                    audioHeartbeat.WaitOne();
                    m_applicationBufferSize = m_updatedApplicationBufferSize;
                    int num = m_rawInRB.ReadSpace();
                    if (num >= 0)
                    {
                        byte[] array = new byte[num];
                        int num2 = m_rawInRB.Read(array, num);
                        if (num2 != num)
                        {
                            Logger.LogLine("m_rawInRB Read Exception, readLen=" + num2 + ", bufferLen=" + num);
                        }

                        float[] array2 = new float[num / 2];
                        float[] array3 = new float[num / 4];
                        float[] array4 = new float[num / 4];
                        Int16_To_Float32(array2, array, (uint)num);
                        bufferSplit(array2, array3, array4);
                        int num3 = m_RBInI.Write(array3, array3.Length);
                        int num4 = m_RBInQ.Write(array4, array4.Length);
                        if (num3 != num4)
                        {
                            Logger.LogLine("IQ Write Exception, samplesWrittenI=" + num3 + ", samplesWrittenQ=" + num4);
                        }

                        if (m_RBInI.ReadSpace() != m_RBInQ.ReadSpace())
                        {
                            Logger.LogLine("IQ Buffer Imbalance detected, resetting ringbuffers");
                            m_RBInI.Reset();
                            m_RBInQ.Reset();
                        }
                    }

                    if (flag)
                    {
                        flag = false;
                    }

                    while (m_RBInI.ReadSpace() >= m_applicationBufferSize)
                    {
                        float[] array5 = new float[m_applicationBufferSize];
                        float[] array6 = new float[m_applicationBufferSize];
                        float[] array7 = new float[m_applicationBufferSize];
                        float[] array8 = new float[m_applicationBufferSize];
                        int num5 = m_RBInI.Read(array7, m_applicationBufferSize);
                        int num6 = m_RBInQ.Read(array8, m_applicationBufferSize);
                        if (num5 != num6)
                        {
                            Logger.LogLine("IQ Read Exception, samplesReadI=" + num5 + ", samplesReadQ=" + num6);
                        }

                        m_AudioInterchangeCallback(array7, array8, array5, array6, 0u);
                        byte[] array9 = new byte[m_applicationBufferSize * 4];
                        for (int i = 0; i < m_applicationBufferSize; i++)
                        {
                            short num7 = Float32_To_Int16(array5[i]);
                            array9[i * 4] = (byte)(num7 & 0xFF);
                            array9[i * 4 + 1] = (byte)((num7 & 0xFF00) >> 8);
                            num7 = Float32_To_Int16(array6[i]);
                            array9[i * 4 + 2] = (byte)(num7 & 0xFF);
                            array9[i * 4 + 3] = (byte)((num7 & 0xFF00) >> 8);
                        }

                        int num8 = m_rawOutRB.Write(array9, array9.Length);
                        if (num8 != array9.Length)
                        {
                            Logger.LogLine("Real RB Write Exception, samplesWrittenRaw=" + num8 + ", temp_out_buf.Length=" + array9.Length);
                        }
                    }
                }
            }

            private int CheckStreamingStatus(bool printAnyway)
            {
                bool flag = false;
                bool flag2 = false;
                uint num = 0u;
                uint num2 = 0u;
                uint num3 = 0u;
                uint num4 = 0u;
                bool flag3 = num != 0 || num2 != 0 || !flag2 || !flag;
                if (!printAnyway && !flag3 && !((double)num4 > 29491.2))
                {
                    _ = (double)num3;
                    _ = 29491.2;
                }

                if (!flag2 || !flag)
                {
                    openOutStream();
                    openInStream();
                }

                return (int)num3;
            }

            private void sendComplete()
            {
            }

            private void RingBufferFloatDebug(RingBufferFloat ringBufIn, RingBufferFloat ringBufOut)
            {
                string str = "";
                int num = ringBufIn.ReadSpace() + ringBufIn.WriteSpace();
                int num2 = ringBufIn.ReadSpace() * 20 / num;
                for (int i = 1; i < 20; i++)
                {
                    str = ((i >= num2) ? (str + " ") : (str + "*"));
                }

                string str2 = "";
                num = ringBufIn.ReadSpace() + ringBufIn.WriteSpace();
                num2 = ringBufIn.ReadSpace() * 20 / num;
                for (int j = 1; j < 20; j++)
                {
                    str2 = ((j >= num2) ? (str2 + " ") : (str2 + "*"));
                }
            }

            private void RingBufferByteDebug(RingBufferByte ringBufIn, RingBufferByte ringBufOut)
            {
                string str = "";
                int num = ringBufIn.ReadSpace() + ringBufIn.WriteSpace();
                int num2 = ringBufIn.ReadSpace() * 20 / num;
                for (int i = 1; i < 20; i++)
                {
                    str = ((i >= num2) ? (str + " ") : (str + "*"));
                }

                string str2 = "";
                num = ringBufIn.ReadSpace() + ringBufIn.WriteSpace();
                num2 = ringBufIn.ReadSpace() * 20 / num;
                for (int j = 1; j < 20; j++)
                {
                    str2 = ((j >= num2) ? (str2 + " ") : (str2 + "*"));
                }
            }

            private void RingBufferByteDebugSimple(RingBufferByte ringBufIn, RingBufferByte ringBufOut)
            {
                bufferInReadSpace.Add(ringBufIn.ReadSpace());
                bufferOutReadSpace.Add(ringBufOut.ReadSpace());
            }

            private void bufferSplit(float[] sourceBuffer, float[] bufferI, float[] bufferQ)
            {
                for (int i = 0; i < sourceBuffer.Length; i += 2)
                {
                    bufferI[i / 2] = sourceBuffer[i];
                    bufferQ[i / 2] = sourceBuffer[i + 1];
                }
            }

            public int StartListener()
            {
                if (outstandingRequests == null)
                {
                    outstandingRequests = new Hashtable();
                }

                dataEnabled = true;
                return reStartListener();
            }

            private int reStartListener()
            {
                if (outstandingRequests == null)
                {
                    outstandingRequests = new Hashtable();
                }

                if (dataEnabled && !dataListening)
                {
                    uint dwOptions = 0u;
                    inUsbPipe.SetContiguous(bIsContiguous: true);
                    try
                    {
                        inUsbPipe.UsbPipeTransferAsync(fRead: true, dwOptions, 0u, DataListenCompletion);
                        dataListening = true;
                    }
                    catch (Exception ex)
                    {
                        Logger.LogLine("StartListener: USB fault" + ex.Message + ex.InnerException);
                    }

                    return 0;
                }

                return 268435475;
            }

            public int StopListener()
            {
                Logger.LogLine("StopListener");
                bool flag = dataListening;
                dataEnabled = false;
                inUsbPipe.SetContiguous(bIsContiguous: false);
                if (flag)
                {
                    GetFirmwareRev(out uint _);
                    return 0;
                }

                return 268435476;
            }

            private void WriteListenCompletion(FLEX1500_Pipe pipe)
            {
            }

            private void DataListenCompletion(FLEX1500_Pipe pipe)
            {
                if (pipe.GetPipeNum() != inUsbPipe.GetPipeNum())
                {
                    return;
                }

                uint transferStatus = pipe.GetTransferStatus();
                switch (transferStatus)
                {
                    case 536870933u:
                        Logger.LogLine("WD_TIME_OUT_EXPIRED on interrupt channel (this is normal if not CW/PTT keys pressed recently)");
                        dataListening = false;
                        if (dataEnabled)
                        {
                            reStartListener();
                        }

                        break;
                    default:
                        Logger.LogLine("Interrupt transfer failed: " + transferStatus.ToString("X") + utils.Stat2Str(transferStatus));
                        dataListening = false;
                        if (dataEnabled)
                        {
                            reStartListener();
                        }

                        break;
                    case 0u:
                        {
                            uint bytesTransferred = pipe.GetBytesTransferred();
                            byte[] array = new byte[bytesTransferred];
                            Array.Copy(pipe.GetBuffer(), array, bytesTransferred);
                            dumpMessage(array);
                            if (bytesTransferred >= 1)
                            {
                                CallbackCheck(array[0], 1, ref m_PTT, m_PTTCallback);
                                CallbackCheck(array[0], 8, ref m_FlexwirePTT, m_FlexwirePTTCallback);
                                CallbackCheck(array[0], 16, ref m_DASH, m_DashChangeCallback);
                                CallbackCheck(array[0], 32, ref m_DOT, m_DotChangeCallback);
                            }

                            if (bytesTransferred <= 1)
                            {
                                break;
                            }

                            if (array[1] == 1)
                            {
                                if (outstandingRequests.ContainsKey(array[2]))
                                {
                                    outstandingRequests[array[2]] = convertToUint(array[4], array[6], array[6], array[7]);
                                }
                                else
                                {
                                    outstandingRequests.Add(array[2], convertToUint(array[4], array[5], array[6], array[7]));
                                }
                            }
                            else if (array[1] == 2)
                            {
                                byte[] array2 = new byte[bytesTransferred];
                                Array.Copy(array, 4L, array2, 0L, bytesTransferred - 4);
                                if (outstandingRequests.ContainsKey(array[2]))
                                {
                                    outstandingRequests[array[2]] = array2;
                                }
                                else
                                {
                                    outstandingRequests.Add(array[2], array2);
                                }

                                dumpResult(array2);
                            }

                            break;
                        }
                }
            }

            private static uint convertToUint(byte one, byte two, byte three, byte four)
            {
                return (uint)((one << 24) + (two << 16) + (three << 8) + four);
            }

            private void CallbackCheck(byte status_byte, byte mask, ref bool saved_state, D_STATE_CHANGE_CALLBACK callback)
            {
                bool flag = checkBit(status_byte, mask);
                if (flag != saved_state)
                {
                    saved_state = flag;
                    callback?.Invoke(saved_state);
                }
            }

            private static void dumpMessage(byte[] buffer)
            {
                if (!Logger.exceptionLogging)
                {
                    return;
                }

                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("Int Buffer: ");
                for (int i = 0; i < buffer.Length; i++)
                {
                    stringBuilder.Append(buffer[i].ToString("X").PadLeft(2, '0') + " ");
                }

                if (checkBit(buffer[0], 1))
                {
                    stringBuilder.Append("PTT ");
                }

                if (checkBit(buffer[0], 8))
                {
                    stringBuilder.Append("FlexWirePTT ");
                }

                if (checkBit(buffer[0], 16))
                {
                    stringBuilder.Append("Dash ");
                }

                if (checkBit(buffer[0], 32))
                {
                    stringBuilder.Append("Dot ");
                }

                if (buffer.Length > 1)
                {
                    if (buffer[1] == 1)
                    {
                        stringBuilder.Append("type=response ");
                    }
                    else if (buffer[1] == 2)
                    {
                        stringBuilder.Append("type=EEPROM_data ");
                    }
                    else
                    {
                        stringBuilder.Append("type=unknown ");
                    }
                }

                Logger.LogLine("Interrupt: " + stringBuilder.ToString());
            }

            private static void dumpCommand(byte[] buffer)
            {
                if (Logger.exceptionLogging)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Append("Int Command: ");
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        stringBuilder.Append(buffer[i].ToString("X").PadLeft(2, '0') + " ");
                    }

                    stringBuilder.Append("index=" + buffer[0].ToString("X").PadLeft(2, '0') + " ");
                    Logger.LogLine("Interrupt: " + stringBuilder.ToString());
                }
            }

            private static void dumpResult(byte[] buffer)
            {
                if (Logger.exceptionLogging)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Append("Int Result: ");
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        stringBuilder.Append(buffer[i].ToString("X").PadLeft(2, '0') + " ");
                    }

                    Logger.LogLine("Interrupt: " + stringBuilder.ToString());
                }
            }

            private static bool checkBit(byte status_byte, byte mask)
            {
                return (byte)((status_byte & mask) ^ mask) > 0;
            }

            private static void Int16_To_Float32(float[] destinationBuffer, byte[] sourceBuffer, uint count)
            {
                for (int i = 0; i < count; i += 2)
                {
                    destinationBuffer[i / 2] = (float)BitConverter.ToInt16(sourceBuffer, i) * 3.05175781E-05f;
                }
            }

            private void BufferCheck(RingBufferFloat I, RingBufferFloat Q)
            {
                int wptrout = 0;
                int rptrout = 0;
                int wptrout2 = 0;
                int rptrout2 = 0;
                I.getPointers(ref wptrout, ref rptrout);
                Q.getPointers(ref wptrout2, ref rptrout2);
            }

            private static void Int24_To_Float32(float[] destinationBuffer, byte[] sourceBuffer, uint count)
            {
                for (int i = 0; i < count; i += 2)
                {
                    destinationBuffer[i / 2] = (float)BitConverter.ToInt32(sourceBuffer, i) * 1.1920929E-07f;
                }
            }

            private short Float32_To_Int16(float inNum)
            {
                int num = 0;
                if (m_dither)
                {
                    num = rand.Next(2);
                }

                if (!m_clip)
                {
                    return (short)(inNum * 32768f - (float)num);
                }

                return (short)Math.Min(Math.Max(inNum * 32768f + (float)num, -32768f), 32767f);
            }

            private int Float32_To_Int24(float inNum)
            {
                int num = 1;
                if (m_dither)
                {
                    num = rand.Next(1);
                }

                if (!m_clip)
                {
                    return (int)(inNum * 8388606f + (float)num);
                }

                return (int)Math.Min(Math.Max(inNum * 8388606f + (float)num, -8388608f), 8388607f);
            }

            private void ConvertBufferFloatToInt(float[] workingBufferI, float[] workingBufferQ, byte[] outputBuffer, int samplesRead, int bits)
            {
                switch (bits)
                {
                    case 16:
                        {
                            for (int j = 0; j < samplesRead; j++)
                            {
                                short num2 = Float32_To_Int16(workingBufferI[j]);
                                outputBuffer[j * 4] = (byte)(num2 & 0xFF);
                                outputBuffer[j * 4 + 1] = (byte)((num2 & 0xFF00) >> 8);
                                num2 = Float32_To_Int16(workingBufferQ[j]);
                                outputBuffer[j * 4 + 2] = (byte)(num2 & 0xFF);
                                outputBuffer[j * 4 + 3] = (byte)((num2 & 0xFF00) >> 8);
                            }

                            break;
                        }
                    case 24:
                        {
                            for (int i = 0; i < samplesRead; i++)
                            {
                                int num = Float32_To_Int24(workingBufferI[i]);
                                outputBuffer[i * 6] = (byte)(num & 0xFF);
                                outputBuffer[i * 6 + 1] = (byte)((num & 0xFF00) >> 8);
                                outputBuffer[i * 6 + 2] = (byte)((num & 0xFF0000) >> 16);
                                num = Float32_To_Int24(workingBufferQ[i]);
                                outputBuffer[i * 6 + 3] = (byte)(num & 0xFF);
                                outputBuffer[i * 6 + 4] = (byte)((num & 0xFF00) >> 8);
                                outputBuffer[i * 6 + 5] = (byte)((num & 0xFF0000) >> 16);
                            }

                            break;
                        }
                }
            }

            public int ReadOp(uint opcode, uint param1, uint param2, out uint val)
            {
                ensureInit();
                message message = default(message);
                uint dwBuffSize = 20u;
                uint dwOptions = 0u;
                byte b = message.index = m_index++;
                message.opcode = opcode;
                message.param1 = param1;
                message.param2 = param2;
                dumpCommand(message.barray());
                outUsbPipe.UsbPipeTransferAsync(fRead: false, dwOptions, message.barray(), dwBuffSize, 0u, WriteListenCompletion);
                for (int i = 1; i < 200; i++)
                {
                    Thread.Sleep(2);
                    if (outstandingRequests.ContainsKey(b))
                    {
                        val = (uint)outstandingRequests[b];
                        outstandingRequests.Remove(b);
                        return 0;
                    }
                }

                val = 0u;
                return 268435472;
            }

            public int WriteOp(uint opcode, uint param1, uint param2)
            {
                ensureInit();
                message message = default(message);
                uint dwBuffSize = 20u;
                uint dwOptions = 0u;
                byte b = message.index = m_index++;
                message.opcode = opcode;
                message.param1 = param1;
                message.param2 = param2;
                uint dwBytesTransfered = 0u;
                dumpCommand(message.barray());
                outUsbPipe.UsbPipeTransfer(fRead: false, dwOptions, message.barray(), dwBuffSize, ref dwBytesTransfered, null, 0u);
                return 0;
            }

            public int WriteEEPROM(ushort offset, byte[] buf)
            {
                if ((offset & 0xFFC0) == ((offset + buf.Length) & 0xFFC0))
                {
                    lowLevelWriteEEPROM(offset, buf);
                }
                else
                {
                    ushort num = (ushort)((offset + buf.Length) & 0xFFC0);
                    byte[] array = new byte[num - offset];
                    byte[] array2 = new byte[buf.Length - array.Length];
                    Array.Copy(buf, array, array.Length);
                    Array.Copy(buf, array.Length, array2, 0, array2.Length);
                    lowLevelWriteEEPROM(offset, array);
                    lowLevelWriteEEPROM((ushort)(offset + array.Length), array2);
                }

                return 0;
            }

            private int lowLevelWriteEEPROM(ushort offset, byte[] buf)
            {
                ensureInit();
                EEPROMmessage eEPROMmessage = default(EEPROMmessage);
                uint dwBuffSize = (uint)(8 + buf.Length);
                uint dwOptions = 0u;
                if (buf.Length == 0)
                {
                    return 0;
                }

                byte b = eEPROMmessage.index = m_index++;
                eEPROMmessage.opcode = 1383u;
                eEPROMmessage.offset = offset;
                eEPROMmessage.bytes = buf;
                uint dwBytesTransfered = 0u;
                dumpCommand(eEPROMmessage.b());
                outUsbPipe.UsbPipeTransfer(fRead: false, dwOptions, eEPROMmessage.b(), dwBuffSize, ref dwBytesTransfered, null, 0u);
                Thread.Sleep(10);
                return 0;
            }

            public int ReadEEPROM(ushort offset, byte num_bytes, out byte[] buf)
            {
                ensureInit();
                EEPROMmessage eEPROMmessage = default(EEPROMmessage);
                uint dwBuffSize = 8u;
                uint dwOptions = 0u;
                if (num_bytes <= 0)
                {
                    buf = new byte[0];
                    return 0;
                }

                byte b = eEPROMmessage.index = m_index++;
                eEPROMmessage.opcode = 1382u;
                eEPROMmessage.offset = offset;
                eEPROMmessage.num_bytes = num_bytes;
                dumpCommand(eEPROMmessage.b());
                int num = 0;
                do
                {
                    outUsbPipe.UsbPipeTransferAsync(fRead: false, dwOptions, eEPROMmessage.b(), dwBuffSize, 0u, WriteListenCompletion);
                    for (int i = 0; i < 25; i++)
                    {
                        Thread.Sleep(5);
                        if (outstandingRequests.ContainsKey(b))
                        {
                            try
                            {
                                buf = new byte[num_bytes];
                                Array.Copy((byte[])outstandingRequests[b], buf, num_bytes);
                                outstandingRequests.Remove(b);
                                return 0;
                            }
                            catch (Exception ex)
                            {
                                Logger.LogLine(string.Concat("ReadEEPROM failure: ", ex.Message, ex.InnerException, " num_bytes=", num_bytes, " key=", b));
                            }

                            break;
                        }
                    }
                }
                while (num++ < 5);
                Logger.LogLine("ReadEEPROM key not found, num_bytes=" + num_bytes + " index=" + b);
                buf = null;
                return 268435472;
            }

            private void ensureInit()
            {
            }

            public int Exit()
            {
                return 0;
            }

            private static void TransferAudioOutCompletion(FLEX1500_Pipe pipe)
            {
                uint transferStatus = pipe.GetTransferStatus();
            }

            public int GetFirmwareRev(out uint rev)
            {
                return ReadOp(1200u, 0u, 0u, out rev);
            }

            public int WriteI2CValue(byte addr, byte data)
            {
                return WriteOp(1019u, addr, data);
            }

            public int WriteI2C2Value(byte addr, byte data1, byte data2)
            {
                return WriteOp(1020u, addr, (uint)((data1 << 8) + data2));
            }

            public int ReadI2CValue(byte addr, byte data1, out byte data2)
            {
                uint val;
                int result = ReadOp(1021u, addr, data1, out val);
                data2 = (byte)val;
                return result;
            }

            public int WriteGPIO(byte data)
            {
                return WriteOp(1047u, data, 0u);
            }

            public int ReadGPIO(out byte data)
            {
                uint val;
                int result = ReadOp(1046u, 0u, 0u, out val);
                data = (byte)val;
                return result;
            }

            public int WriteSPI(byte num_bytes, byte data)
            {
                return WriteOp(1065u, num_bytes, data);
            }

            public int SetLED(bool b)
            {
                return WriteOp(1287u, Convert.ToUInt32(b), 0u);
            }

            public int SetFreqTW(uint tw)
            {
                return WriteOp(1347u, tw, 0u);
            }

            public int GetSerialNumber(out string serial)
            {
                uint val;
                int result = ReadOp(1220u, 0u, 0u, out val);
                val = SwapBytes(val);
                serial = SerialToString(val);
                return result;
            }

            private uint SwapBytes(uint x)
            {
                return ((x & 0xFF) << 24) | ((x & 0xFF00) << 8) | ((x & 0xFF0000) >> 8) | ((uint)((int)x & -16777216) >> 24);
            }

            public string SerialToString(uint serial)
            {
                string str = "";
                str += ((byte)(serial >> 24)).ToString("00");
                str = str + ((byte)(serial >> 16)).ToString("00") + "-";
                return str + ((ushort)serial).ToString("0000");
            }
        }

        private delegate void D_ATTACH_GUI_CALLBACK(FLEX1500_Device pDev);

        private delegate void D_DETACH_GUI_CALLBACK(FLEX1500_Device pDev);

        public delegate void D_ATTACH_DETACH_CALLBACK(IntPtr hDevice);

        public delegate void D_STATE_CHANGE_CALLBACK(bool status);

        public delegate int D_INTERCHANGE_BUFFER_CALLBACK(float[] AudioInBuf1, float[] AudioInBuf2, float[] AudioOutBuf1, float[] AudioOutBuf2, uint paFlags);

        private const uint USB_ISOCH_FULL_PACKETS_ONLY = 32u;

        private const uint USB_ABORT_PIPE = 64u;

        private const uint USB_ISOCH_NOASAP = 128u;

        private const uint USB_BULK_INT_URB_SIZE_OVERRIDE_128K = 256u;

        private const uint USB_STREAM_OVERWRITE_BUFFER_WHEN_FULL = 512u;

        private const uint USB_STREAM_MAX_TRANSFER_SIZE_OVERRIDE = 1024u;

        private const string APP_LNAME = "FLEX-1500 USB Driver";

        private const string DEFAULT_LICENSE_STRING = "6f1ea0635af8a12084842dc3b0de4206522373b8fe1810.WD1040_NL_FlexRadio_Systems";

        private const string DEFAULT_DRIVER_NAME = "flex1500";

        private const ushort DEFAULT_PRODUCT_ID = 5378;

        private const ushort DEFAULT_VENDOR_ID = 8594;

        public const uint TIME_OUT = 0u;

        private const int FLEX1500_IN_PIPE = 3;

        private const int FLEX1500_OUT_PIPE = 4;

        private const int FLEX1502_IN_PIPE = 1;

        private const int FLEX1502_OUT_PIPE = 2;

        private const int FLEX1500_AUDIO_IN_PIPE = 2;

        private const int FLEX1500_AUDIO_OUT_PIPE = 1;

        public const byte FLEX1500_PTT_MASK = 1;

        public const byte FLEX1500_PA_INPUT_UNDERFLOW = 1;

        public const byte FLEX1500_PA_INPUT_OVERFLOW = 2;

        public const byte FLEX1500_PA_OUTPUT_UNDERFLOW = 4;

        public const byte FLEX1500_PA_OUTPUT_OVERFLOW = 8;

        public const byte FLEX1500_FLEXWIRE_PTT_MASK = 8;

        public const byte FLEX1500_DASH_MASK = 16;

        public const byte FLEX1500_DOT_MASK = 32;

        public const int FLEX1500_MAX_RING_BUFFER_SIZE = 8192;

        public const int FLEX1500_IN_RING_BUFFER_SIZE = 32768;

        public const int FLEX1500_OUT_RING_BUFFER_SIZE = 32768;

        public const int FLEX1500_BITS_PER_SAMPLE = 16;

        public const int FLEX1500_SAMPLING_RATE = 48000;

        public const int FLEX1500_USB_PACKETS_PER_SECOND = 1000;

        public const int FLEX1500_SAMPLES_PER_PACKET = 48;

        public const int FLEX1500_AUDIO_PACKET_SIZE = 192;

        public const int FLEX1500_RECEIVE_BUFFER_SIZE = 384;

        public const int FLEX1500_HW_BUFFER_SIZE = 32768;

        public const byte FLEX1500_EITHER_PTT_MASK = 9;

        public const int FLEX1500_ALREADY_LISTENING = 268435475;

        public const int FLEX1500_STREAM_NOT_OPEN = 268435477;

        public const int FLEX1500_NOT_LISTENING = 268435476;

        public const int FLEX1500_READ_TIMEOUT = 268435472;

        public const int FLEX1500_NO_DEVICE_ATTACHED = 268435473;

        public const uint FLEX1500_WRITE_EEPROM = 1383u;

        public const uint FLEX1500_READ_EEPROM = 1382u;

        public static uint audioPacketSize = 3072u;

        private static FLEX1500_DeviceManager uDevManager;

        private static D_ATTACH_DETACH_CALLBACK deviceAttachCallback;

        private static D_ATTACH_DETACH_CALLBACK deviceDetachCallback;

        private static Hashtable outstandingRequests;

        public static uint number_devices = 0u;

        public static Dictionary<IntPtr, radio> radioArray;

        private static IEnumerator ienum;

        public Flex1500USB(D_ATTACH_DETACH_CALLBACK dDeviceAttachApplication, D_ATTACH_DETACH_CALLBACK deviceDetachApplication)
        {
            D_USER_ATTACH_CALLBACK dAttachCb = UserDeviceAttach;
            D_USER_DETACH_CALLBACK dDetachCb = UserDeviceDetach;
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\FlexRadio Systems\\flex1500_debug";
            Logger.exceptionLogging = File.Exists(path);
            if (Logger.exceptionLogging)
            {
                Thread thread = new Thread(Logger.Flusher);
                thread.Name = "Logger Flusher";
                thread.IsBackground = true;
                thread.Priority = ThreadPriority.Lowest;
                thread.Start();
            }

            Logger.LogLine("Flex1500USB Start -----------------");
            deviceAttachCallback = dDeviceAttachApplication;
            deviceDetachCallback = deviceDetachApplication;
            try
            {
                uDevManager = new FLEX1500_DeviceManager(dAttachCb, dDetachCb, 8594, 5378, "flex1500", "6f1ea0635af8a12084842dc3b0de4206522373b8fe1810.WD1040_NL_FlexRadio_Systems");
            }
            catch (Exception ex)
            {
                Logger.LogLine("FLEX1500_DeviceManager failure: " + ex.Message + ex.InnerException);
            }

            radioArray = new Dictionary<IntPtr, radio>();
        }

        private static void UserDeviceAttach(FLEX1500_Device pDev)
        {
            IntPtr key = pDev.GethDevice();
            radio value = new radio(pDev);
            bool flag = true;
            Logger.LogLine("Attach PID=" + pDev.GetPid() + " count=" + pDev.GetpPipesList().Count + " ");
            try
            {
                if (pDev.GetPid() == 5377)
                {
                    radioArray.Add(key, value);
                    radioArray[key].inUsbPipe = (FLEX1500_Pipe)pDev.GetpPipesList()[3];
                    radioArray[key].outUsbPipe = (FLEX1500_Pipe)pDev.GetpPipesList()[4];
                    radioArray[key].inAudioPipe = (FLEX1500_Pipe)pDev.GetpPipesList()[2];
                    radioArray[key].outAudioPipe = (FLEX1500_Pipe)pDev.GetpPipesList()[1];
                    radioArray[key].inAudioPipe.SetContiguous(bIsContiguous: true);
                }
                else if (pDev.GetPid() == 5378)
                {
                    if (pDev.GetpPipesList().Count != 1)
                    {
                        radioArray.Add(key, value);
                        for (int i = 0; i < pDev.GetpPipesList().Count; i++)
                        {
                            FLEX1500_Pipe fLEX1500_Pipe = (FLEX1500_Pipe)pDev.GetpPipesList()[i];
                            if (fLEX1500_Pipe.GetPipeDirection() == 1 && fLEX1500_Pipe.GetPipeType() == 3)
                            {
                                radioArray[key].inUsbPipe = (FLEX1500_Pipe)pDev.GetpPipesList()[i];
                                Logger.LogLine("...inUsbPipe assigned");
                            }
                            else if (fLEX1500_Pipe.GetPipeDirection() == 2 && fLEX1500_Pipe.GetPipeType() == 3)
                            {
                                radioArray[key].outUsbPipe = (FLEX1500_Pipe)pDev.GetpPipesList()[i];
                                Logger.LogLine("...outUsbPipe assigned");
                            }
                            else if (fLEX1500_Pipe.GetPipeDirection() == 1 && fLEX1500_Pipe.GetPipeType() == 1)
                            {
                                radioArray[key].inAudioPipe = (FLEX1500_Pipe)pDev.GetpPipesList()[i];
                                Logger.LogLine("...inAudioPipe assigned");
                            }
                            else if (fLEX1500_Pipe.GetPipeDirection() == 2 && fLEX1500_Pipe.GetPipeType() == 1)
                            {
                                radioArray[key].outAudioPipe = (FLEX1500_Pipe)pDev.GetpPipesList()[i];
                                Logger.LogLine("...outAudioPipe assigned");
                            }
                        }
                    }
                    else
                    {
                        flag = false;
                        Logger.LogLine("...deemed bad device due to incorrect pipe count");
                    }
                }

                if (flag)
                {
                    radioArray[key].StartListener();
                    Logger.LogLine("...starting interrupt listener");
                }
            }
            catch (Exception ex)
            {
                Logger.LogLine("UserDeviceAttach failure: " + ex.Message + ex.InnerException);
            }

            number_devices = (uint)radioArray.Count;
            ienum = radioArray.GetEnumerator();
            ienum.Reset();
            if (deviceAttachCallback != null && flag)
            {
                deviceAttachCallback(pDev.GethDevice());
            }
        }

        private static void UserDeviceDetach(FLEX1500_Device pDev)
        {
            try
            {
                Logger.LogLine("Detach");
                foreach (KeyValuePair<IntPtr, radio> item in radioArray)
                {
                    if (item.Key == pDev.GethDevice())
                    {
                        Logger.LogLine("...located device, removing");
                        radioArray.Remove(item.Key);
                        number_devices = (uint)radioArray.Count;
                        ienum = radioArray.GetEnumerator();
                        ienum.Reset();
                        if (deviceDetachCallback != null)
                        {
                            deviceDetachCallback(pDev.GethDevice());
                        }

                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogLine("UserDeviceDetach failure: " + ex.Message + ex.InnerException);
            }
        }
    }
}
#if false // Decompilation log
'39' items in cache
------------------
Resolve: 'mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
WARN: Version mismatch. Expected: '2.0.0.0', Got: '4.0.0.0'
Load from: 'C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5.2\mscorlib.dll'
------------------
Resolve: 'wdapi_dotnet1040, Version=10.4.0.0, Culture=neutral, PublicKeyToken=null'
Could not find by name: 'wdapi_dotnet1040, Version=10.4.0.0, Culture=neutral, PublicKeyToken=null'
#endif
