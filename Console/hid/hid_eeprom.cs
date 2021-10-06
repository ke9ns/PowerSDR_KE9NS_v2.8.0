//=================================================================
// hid_eeprom.cs
//=================================================================
// PowerSDR is a C# implementation of a Software Defined Radio.
// Copyright (C) 2003-2013  FlexRadio Systems
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
//
// You may contact us via email at: gpl@flexradio.com.
// Paper mail may be sent to: 
//    FlexRadio Systems
//    4616 W. Howard Lane  Suite 1-150
//    Austin, TX 78728
//    USA
//=================================================================

// EEPROM 0x0000 - 0x3FFF

// 0x3000-0x310F = RX/TX calibration and checksum bytes


using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace PowerSDR
{
    public class HIDEEPROM
    {
        #region Misc Routines

        public static void Init()
        {
            USBHID.GetSerialNum(out serial_number); // get serial number 

            USBHID.GetTRXSN(out trx_serial);
            USBHID.GetTRXRev(out trx_rev);

            USBHID.GetPASN(out pa_serial);
            USBHID.GetPARev(out pa_rev);


            byte[] data; // get last cal date/time
            USBHID.ReadEEPROM(0x1820, 8, out data);
            if (data != null)
                last_cal_date_time = BitConverter.ToInt64(data, 0);

            /* USBHID.ReadTRXEEPROMByte(0x1BE, out temp);
			rx1_image_ver = temp;*/

            USBHID.GetRegion(out region); // check TURF in EEPROM
            if (region >= FRSRegion.LAST) region = FRSRegion.US; // use US REGION if value is messed up

        } // Init()

        public static bool NeedDump()
        {
            if (File.Exists(Application.StartupPath + "\\nobackup")) return false;  // for production
            uint data;

            StringBuilder s = new StringBuilder("F1.5K_");

            USBHID.GetSerialNum(out data);
            s.Append(SerialToString(data));
            if (File.Exists(app_data_path + "Backup\\" + s + " backup.csv"))
                return false;
            return true;
        }

        public static void StartDump()
        {
            Thread t = new Thread(new ThreadStart(Dump));
            t.Name = "EEPROM Dump Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.Normal;
            t.Start();
            while (t.IsAlive)
            {
                Thread.Sleep(5);
            }
        }

        private static void Dump()
        {
            if (File.Exists(Application.StartupPath + "\\nobackup")) return;  // for production
            uint data;

            StringBuilder s = new StringBuilder("F1.5K_");

            USBHID.GetSerialNum(out data);
            s.Append(SerialToString(data));
            if (!Directory.Exists(app_data_path + "\\Backup\\"))
                Directory.CreateDirectory(app_data_path + "\\Backup\\");
            if (File.Exists(app_data_path + "\\Backup\\" + s + " backup.csv"))
            {
                return;
            }
            StreamWriter writer = new StreamWriter(app_data_path + "\\Backup\\" + s + " backup.csv");

            s = new StringBuilder(",");
            for (int i = 0; i < 16; i++)
                s.Append(i.ToString("X") + ",");
            writer.WriteLine(s);

            byte[] buf = new byte[32];

            USBHID.ReadEEPROM(0x1800, 32, out buf);
            s = new StringBuilder("1800,");

            for (int j = 0; j < 16; j++)
                s.Append(buf[j].ToString("X") + ",");

            writer.WriteLine(s);
            Application.DoEvents();

            s = new StringBuilder("1810,");

            for (int j = 0; j < 16; j++)
                s.Append(buf[j + 16].ToString("X") + ",");

            writer.WriteLine(s);

            USBHID.ReadEEPROM(0x1820, 16, out buf);
            s = new StringBuilder("1820,");

            for (int j = 0; j < 16; j++)
                s.Append(buf[j].ToString("X") + ",");

            writer.WriteLine(s);
            Application.DoEvents();

            for (int i = 0; i < 9; i++) // 32*9 = 288
            {
                USBHID.ReadEEPROM((ushort)(0x3000 + i * 32), 32, out buf);

                s = new StringBuilder((0x300 + i * 2).ToString("X") + "0,");

                for (int j = 0; j < 16; j++)
                    s.Append(buf[j].ToString("X") + ",");

                writer.WriteLine(s);
                Application.DoEvents();

                s = new StringBuilder((0x300 + i * 2 + 1).ToString("X") + "0,");

                for (int j = 0; j < 16; j++)
                    s.Append(buf[j + 16].ToString("X") + ",");

                writer.WriteLine(s);
            }

            writer.Close();
        }

        public static string RevToString(uint rev)
        {
            string s = "";
            s += ((byte)(rev >> 24)).ToString() + ".";
            s += ((byte)(rev >> 16)).ToString() + ".";
            s += ((byte)(rev >> 8)).ToString() + ".";
            s += ((byte)(rev >> 0)).ToString();
            return s;
        }

        public static string SerialToString(uint serial)
        {
            string s = "";
            s += ((byte)(serial >> 24)).ToString("00");
            s += ((byte)(serial >> 16)).ToString("00") + "-";
            s += ((ushort)(serial)).ToString("0000");
            return s;
        }

        private static void WriteCalDateTime()
        {
            long l = DateTime.Now.ToFileTimeUtc();
            byte[] buf = BitConverter.GetBytes(l);
            CheckedWrite(0x1820, buf, 8);
            last_cal_date_time = l;
        }

        /*public static bool CheckAll()
		{
			bool b = true;
			b = CheckRXLevel(); if(!b) return b;
			b = CheckRXImage(); if(!b) return b;
			b = CheckTXImage(); if(!b) return b;
			
			return b;
		}*/

        private static bool CheckedWrite(ushort addr, byte val)
        {
            byte[] buf = new byte[1];
            buf[0] = val;
            return CheckedWrite(addr, buf, 1);
        }

        private static bool CheckedWrite(ushort addr, byte[] buf, byte num_bytes)
        {
            int error_count = 0;
            bool validated = true;
            byte[] test = new byte[num_bytes];

            do
            {
                int val = USBHID.WriteEEPROM(addr, num_bytes, buf);

                val = USBHID.ReadEEPROM(addr, num_bytes, out test);

                validated = true;
                for (int j = 0; j < num_bytes; j++)
                {
                    if (test[j] != buf[j])
                    {
                        validated = false;
                        break;
                    }
                }

                if (!validated) error_count++;
                if (error_count > NUM_WRITES_TO_TRY)
                    return false;

            } while (!validated);

            return true;
        }

        #endregion

        #region Properties

        private static string app_data_path = "";
        public static string AppDataPath
        {
            set { app_data_path = value; }
        }

        private static FRSRegion region = FRSRegion.US;
        public static FRSRegion Region
        {
            get { return region; }
        }

        private static long last_cal_date_time = 0;
        public static long LastCalDateTime
        {
            get { return last_cal_date_time; }
            set { last_cal_date_time = value; }
        }

        private static uint serial_number;
        public static uint SerialNumber
        {
            get { return serial_number; }
            //set { serial_number = value; }
        }

        private static uint trx_serial;
        public static uint TRXSerial
        {
            get { return trx_serial; }
            //set { trx_serial = value; }
        }

        private static uint trx_rev;
        public static uint TRXRev
        {
            get { return trx_rev; }
        }

        private static uint pa_serial;
        public static uint PASerial
        {
            get { return pa_serial; }
            //set { pa_serial = value; }
        }





        private static uint pa_rev;
        public static uint PARev
        {
            get { return pa_rev; }
            //set { pa_rev = value; }
        }

        private static int rx1_image_ver = 0;
        public static int RX1ImageVer
        {
            get { return rx1_image_ver; }
        }

        #endregion

        #region RX

        #region RX Level

        private const int NUM_WRITES_TO_TRY = 5;
        public static bool CheckRXLevel()
        {
            Random rand = new Random();
            Band[] bands = { Band.B160M, Band.B80M, Band.B60M, Band.B40M, Band.B30M, Band.B20M,
                               Band.B17M, Band.B15M, Band.B12M, Band.B10M, Band.B6M };

            float[][] rx_level_table = new float[(int)Band.LAST][];
            for (int i = 0; i < (int)Band.LAST; i++)
                rx_level_table[i] = new float[3];

            for (int i = 0; i < bands.Length; i++)
                for (int j = 0; j < 3; j++)
                    rx_level_table[(int)bands[i]][j] = (float)rand.NextDouble();

            byte temp;
            WriteRXLevel(rx_level_table, out temp);

            float[][] rx_level_check = new float[(int)Band.LAST][];
            for (int i = 0; i < (int)Band.LAST; i++)
                rx_level_check[i] = new float[3];

            ReadRXLevel(rx_level_check);

            for (int i = 0; i < bands.Length; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (rx_level_table[(int)bands[i]][j] != rx_level_check[(int)bands[i]][j])
                        return false;
                }
            }
            return true;
        }

        public static void WriteRXLevel(float[][] table, out byte checksum)
        {
            WriteCalDateTime();
            Band[] bands = { Band.B160M, Band.B80M, Band.B60M, Band.B40M, Band.B30M, Band.B20M,
                               Band.B17M, Band.B15M, Band.B12M, Band.B10M, Band.B6M };

            ushort addr = 0x3000;
            byte[] buf = new byte[32];
            int length = 0;

            for (int i = 0; i < bands.Length; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    short val = (short)(Math.Round(table[(int)bands[i]][j], 1) * 10);
                    BitConverter.GetBytes(val).CopyTo(buf, length);
                    length += 2;
                }

                if (length == 32)
                {
                    if (!CheckedWrite(addr, buf, (byte)length))
                    {
                        MessageBox.Show("Error writing RX Level value to EEPROM.");
                        checksum = 0xFF;
                        return;
                    }

                    length = 0;
                    addr += 32;
                }
            }

            if (length > 0)
            {
                if (!CheckedWrite(addr, buf, (byte)length))
                {
                    MessageBox.Show("Error writing RX Level value to EEPROM.");
                    checksum = 0xFF;
                    return;
                }
            }

            // calculate and write checksum
            byte sum = Checksum.Calc(table);
            WriteRXLevelChecksum(sum);
            checksum = sum;
        }

        public static void WriteRXLevelChecksum(byte sum)
        {
            if (!CheckedWrite(0x302F, sum))
                MessageBox.Show("Error writing RX Level checksum to EEPROM.");
        }

        public static void ReadRXLevel(float[][] table)
        {
            Band[] bands = { Band.B160M, Band.B80M, Band.B60M, Band.B40M, Band.B30M, Band.B20M,
                               Band.B17M, Band.B15M, Band.B12M, Band.B10M, Band.B6M };

            ushort addr = 0x3000;
            byte[] buf1 = new byte[32];
            byte[] buf2 = new byte[32];
            USBHID.ReadEEPROM(addr, 32, out buf1);
            addr += 32;
            USBHID.ReadEEPROM(addr, 32, out buf2);
            int index = 0;

            for (int i = 0; i < 8; i++)
            {
                if (buf1[i] != 0xFF)
                    break;

                if (i == 7) return; // data is all defaults -- nothing to read
            }

            for (int i = 0; i < bands.Length; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    short val = 0;
                    if (index < 32)
                        val = BitConverter.ToInt16(buf1, index);
                    else val = BitConverter.ToInt16(buf2, index - 32);
                    index += 2;

                    table[(int)bands[i]][j] = (float)(val / 10.0);
                }
            }
        }

        public static byte ReadRXLevelChecksum()
        {
            byte read;
            USBHID.ReadTRXEEPROMByte(0x302F, out read);
            return read;
        }

        #endregion

        #region RX Image

        public static bool CheckRXImage()
        {
            Random rand = new Random();
            Band[] bands = { Band.B160M, Band.B80M, Band.B60M, Band.B40M, Band.B30M, Band.B20M,
                               Band.B17M, Band.B15M, Band.B12M, Band.B10M, Band.B6M };

            float[] gain_table = new float[(int)Band.LAST];
            float[] phase_table = new float[(int)Band.LAST];

            for (int i = 0; i < bands.Length; i++)
            {
                gain_table[(int)bands[i]] = (float)rand.NextDouble();
                phase_table[(int)bands[i]] = (float)rand.NextDouble();
            }

            byte gain_sum = Checksum.CalcHF(gain_table);
            byte phase_sum = Checksum.CalcHF(phase_table);

            byte temp;
            WriteRXImage(gain_table, phase_table, out temp, out temp);

            float[] gain_check = new float[(int)Band.LAST];
            float[] phase_check = new float[(int)Band.LAST];

            ReadRXImage(gain_check, phase_check);

            byte gain_sum_check = Checksum.CalcHF(gain_check);
            byte phase_sum_check = Checksum.CalcHF(phase_check);

            if ((gain_sum_check != gain_sum) ||
                (phase_sum_check != phase_sum))
                return false;

            for (int i = 0; i < bands.Length; i++)
            {
                if (gain_table[(int)bands[i]] != gain_check[(int)bands[i]] ||
                    phase_table[(int)bands[i]] != phase_check[(int)bands[i]])
                    return false;
            }
            return true;
        }

        public static void WriteRXImage(float[] gain_table, float[] phase_table, out byte gain_sum, out byte phase_sum)
        {
            WriteCalDateTime();
            Band[] bands = { Band.B160M, Band.B80M, Band.B60M, Band.B40M, Band.B30M, Band.B20M,
                               Band.B17M, Band.B15M, Band.B12M, Band.B10M, Band.B6M };

            ushort addr = 0x3050;
            byte[] buf = new byte[32];
            int length = 0;

            for (uint i = 0; i < bands.Length; i++)
            {
                float val = gain_table[(int)bands[i]];
                if (val > 500.0f || val < -500.0f)
                {
                    TextWriter writer = new StreamWriter(app_data_path + "\\eeprom_error.log", true);
                    writer.WriteLine(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + " " +
                        "Error writing RX Image Gain value to EEPROM  -- sn: " + SerialToString(serial_number) + " > " +
                        bands[i].ToString() + " - Value out of range [-500.0, 500.0] (" + val.ToString("f4") + ").");
                    writer.Close();
                    gain_table[(int)bands[i]] = val = 0.0f;
                }

                BitConverter.GetBytes(val).CopyTo(buf, length);
                length += 4;

                if (length == 32)
                {
                    if (!CheckedWrite(addr, buf, (byte)length))
                    {
                        MessageBox.Show("Error writing RX Image Gain value to EEPROM.");
                        gain_sum = 0xFF;
                        phase_sum = 0xFF;
                        return;
                    }

                    length = 0;
                    addr += 32;
                }
            }

            if (length > 0)
            {
                if (!CheckedWrite(addr, buf, (byte)length))
                {
                    MessageBox.Show("Error writing RX Image Gain value to EEPROM.");
                    gain_sum = 0xFF;
                    phase_sum = 0xFF;
                    return;
                }

                length = 0;
            }

            addr = 0x3080;
            for (int i = 0; i < bands.Length; i++)
            {
                float val = phase_table[(int)bands[i]];
                if (val > 400.0f || val < -400.0f)
                {
                    TextWriter writer = new StreamWriter(app_data_path + "\\eeprom_error.log", true);
                    writer.WriteLine(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + " " +
                        "Error writing RX Image Phase value to EEPROM -- sn: " + SerialToString(serial_number) + " > " +
                        bands[i].ToString() + " - Value out of range [-400.0, 400.0] (" + val.ToString("f4") + ").");
                    writer.Close();
                    gain_table[(int)bands[i]] = val = 0.0f;
                }

                BitConverter.GetBytes(val).CopyTo(buf, length);
                length += 4;

                if (length == 32)
                {
                    if (!CheckedWrite(addr, buf, (byte)length))
                    {
                        MessageBox.Show("Error writing RX Image Phase value to EEPROM.");
                        gain_sum = 0xFF;
                        phase_sum = 0xFF;
                        return;
                    }

                    length = 0;
                    addr += 32;
                }
            }

            if (length > 0)
            {
                if (!CheckedWrite(addr, buf, (byte)length))
                {
                    MessageBox.Show("Error writing RX Image Phase value to EEPROM.");
                    gain_sum = 0xFF;
                    phase_sum = 0xFF;
                    return;
                }
            }

            // calculate and write checksums
            byte sum = Checksum.CalcHF(gain_table);
            WriteRXImageGainChecksum(sum);
            gain_sum = sum;

            sum = Checksum.CalcHF(phase_table);
            WriteRXImagePhaseChecksum(sum);
            phase_sum = sum;
        }

        public static void WriteRXImageGainChecksum(byte sum)
        {
            if (!CheckedWrite(0x307F, sum))
                MessageBox.Show("Error writing RX Image Gain checksum to EEPROM.");
        }

        public static void WriteRXImagePhaseChecksum(byte sum)
        {
            if (!CheckedWrite(0x30AF, sum))
                MessageBox.Show("Error writing RX Image Phase checksum to EEPROM.");
        }

        public static void ReadRXImage(float[] gain_table, float[] phase_table)
        {
            Band[] bands = { Band.B160M, Band.B80M, Band.B60M, Band.B40M, Band.B30M, Band.B20M,
                               Band.B17M, Band.B15M, Band.B12M, Band.B10M, Band.B6M };

            ushort addr = 0x3050;
            byte[] buf1 = new byte[32];
            byte[] buf2 = new byte[32];
            USBHID.ReadEEPROM(addr, 32, out buf1);
            addr += 32;
            USBHID.ReadEEPROM(addr, 32, out buf2);
            int index = 0;

            for (int i = 0; i < 8; i++)
            {
                if (buf1[i] != 0xFF)
                    break;

                if (i == 7) return; // data is all defaults -- nothing to read
            }

            for (int i = 0; i < bands.Length; i++)
            {
                float val = 0.0f;
                if (index < 32)
                    val = BitConverter.ToSingle(buf1, index);
                else val = BitConverter.ToSingle(buf2, index - 32);
                index += 4;

                if (val > 500.0f || val < -500.0f)
                {
                    TextWriter writer = new StreamWriter(app_data_path + "\\eeprom_error.log", true);
                    writer.WriteLine(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + " " +
                        "Error reading RX Image Gain value from EEPROM -- sn: " + SerialToString(serial_number) + " > " +
                        bands[i].ToString() + " - Value out of range [-400.0, 400.0] (" + val.ToString("f4") + ").");
                    writer.Close();
                    val = 0.0f;
                }

                gain_table[(int)bands[i]] = val;
            }

            addr = 0x3080;
            USBHID.ReadEEPROM(addr, 32, out buf1);
            addr += 32;
            USBHID.ReadEEPROM(addr, 32, out buf2);
            index = 0;

            for (int i = 0; i < 8; i++)
            {
                if (buf1[i] != 0xFF)
                    break;

                if (i == 7) return; // data is all defaults -- nothing to read
            }

            for (int i = 0; i < bands.Length; i++)
            {
                float val = 0.0f;
                if (index < 32)
                    val = BitConverter.ToSingle(buf1, index);
                else val = BitConverter.ToSingle(buf2, index - 32);
                index += 4;

                if (val > 400.0f || val < -400.0f)
                {
                    TextWriter writer = new StreamWriter(app_data_path + "\\eeprom_error.log", true);
                    writer.WriteLine(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + " " +
                        "Error reading RX Image Phase value from EEPROM -- sn: " + SerialToString(serial_number) + " > " +
                        bands[i].ToString() + " - Value out of range [-400.0, 400.0] (" + val.ToString("f4") + ").");
                    writer.Close();
                    val = 0.0f;
                }

                phase_table[(int)bands[i]] = val;
            }
        }

        public static byte ReadRXImageGainChecksum()
        {
            byte read;
            USBHID.ReadTRXEEPROMByte(0x307F, out read);
            return read;
        }

        public static byte ReadRXImagePhaseChecksum()
        {
            byte read;
            USBHID.ReadTRXEEPROMByte(0x30AF, out read);
            return read;
        }

        #endregion

        #endregion

        #region TX

        #region TX Image

        public static bool CheckTXImage()
        {
            Random rand = new Random();
            Band[] bands = { Band.B160M, Band.B80M, Band.B60M, Band.B40M, Band.B30M, Band.B20M,
                               Band.B17M, Band.B15M, Band.B12M, Band.B10M, Band.B6M };

            float[] gain_table = new float[(int)Band.LAST];
            float[] phase_table = new float[(int)Band.LAST];

            for (int i = 0; i < bands.Length; i++)
            {
                gain_table[(int)bands[i]] = (float)rand.NextDouble();
                phase_table[(int)bands[i]] = (float)rand.NextDouble();
            }

            byte temp;
            WriteTXImage(gain_table, phase_table, out temp, out temp);

            float[] gain_check = new float[(int)Band.LAST];
            float[] phase_check = new float[(int)Band.LAST];

            ReadTXImage(gain_check, phase_check);

            for (int i = 0; i < bands.Length; i++)
            {
                if (gain_table[(int)bands[i]] != gain_check[(int)bands[i]] ||
                    phase_table[(int)bands[i]] != phase_check[(int)bands[i]])
                    return false;
            }
            return true;
        }

        public static void WriteTXImage(float[] gain_table, float[] phase_table, out byte gain_sum, out byte phase_sum)
        {
            WriteCalDateTime();
            Band[] bands = { Band.B160M, Band.B80M, Band.B60M, Band.B40M, Band.B30M, Band.B20M,
                               Band.B17M, Band.B15M, Band.B12M, Band.B10M, Band.B6M };

            ushort addr = 0x30B0;
            byte[] buf = new byte[32];
            int length = 0;

            for (uint i = 0; i < bands.Length; i++)
            {
                float val = gain_table[(int)bands[i]];
                if (val > 500.0f || val < -500.0f)
                {
                    TextWriter writer = new StreamWriter(app_data_path + "\\eeprom_error.log", true);
                    writer.WriteLine(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + " " +
                        "Error writing TX Image Gain value to EEPROM -- sn: " + SerialToString(serial_number) + " > " +
                        bands[i].ToString() + " - Value out of range [-500.0, 500.0] (" + val.ToString("f4") + ").");
                    writer.Close();
                    gain_table[(int)bands[i]] = val = 0.0f;
                }

                BitConverter.GetBytes(val).CopyTo(buf, length);
                length += 4;

                if (length == 32)
                {
                    if (!CheckedWrite(addr, buf, (byte)length))
                    {
                        MessageBox.Show("Error writing TX Image Gain value to EEPROM.");
                        gain_sum = 0xFF;
                        phase_sum = 0xFF;
                        return;
                    }

                    length = 0;
                    addr += 32;
                }
            }

            if (length > 0)
            {
                if (!CheckedWrite(addr, buf, (byte)length))
                {
                    MessageBox.Show("Error writing TX Image Gain value to EEPROM.");
                    gain_sum = 0xFF;
                    phase_sum = 0xFF;
                    return;
                }

                length = 0;
            }

            addr = 0x30E0;
            for (int i = 0; i < bands.Length; i++)
            {
                float val = phase_table[(int)bands[i]];
                if (val > 400.0f || val < -400.0f)
                {
                    TextWriter writer = new StreamWriter(app_data_path + "\\eeprom_error.log", true);
                    writer.WriteLine(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + " " +
                        "Error writing TX Image Phase value to EEPROM -- sn: " + SerialToString(serial_number) + " > " +
                        bands[i].ToString() + " - Value out of range [-400.0, 400.0] (" + val.ToString("f4") + ").");
                    writer.Close();
                    gain_table[(int)bands[i]] = val = 0.0f;
                }

                BitConverter.GetBytes(val).CopyTo(buf, length);
                length += 4;

                if (length == 32)
                {
                    if (!CheckedWrite(addr, buf, (byte)length))
                    {
                        MessageBox.Show("Error writing TX Image Phase value to EEPROM.");
                        gain_sum = 0xFF;
                        phase_sum = 0xFF;
                        return;
                    }

                    length = 0;
                    addr += 32;
                }
            }

            if (length > 0)
            {
                if (!CheckedWrite(addr, buf, (byte)length))
                {
                    MessageBox.Show("Error writing TX Image Phase value to EEPROM.");
                    gain_sum = 0xFF;
                    phase_sum = 0xFF;
                    return;
                }
            }

            // calculate and write checksums
            byte sum = Checksum.CalcHF(gain_table);
            WriteTXImageGainChecksum(sum);
            gain_sum = sum;

            sum = Checksum.CalcHF(phase_table);
            WriteTXImagePhaseChecksum(sum);
            phase_sum = sum;
        }

        public static void WriteTXImageGainChecksum(byte sum)
        {
            if (!CheckedWrite(0x30DF, sum))
                MessageBox.Show("Error writing TX Image Gain checksum to EEPROM.");
        }

        public static void WriteTXImagePhaseChecksum(byte sum)
        {
            if (!CheckedWrite(0x310F, sum))
                MessageBox.Show("Error writing TX Image Phase checksum to EEPROM.");
        }

        public static void ReadTXImage(float[] gain_table, float[] phase_table)
        {
            Band[] bands = { Band.B160M, Band.B80M, Band.B60M, Band.B40M, Band.B30M, Band.B20M,
                               Band.B17M, Band.B15M, Band.B12M, Band.B10M, Band.B6M };

            ushort addr = 0x30B0;
            byte[] buf1 = new byte[32];
            byte[] buf2 = new byte[32];
            USBHID.ReadEEPROM(addr, 32, out buf1);
            addr += 32;
            USBHID.ReadEEPROM(addr, 32, out buf2);
            int index = 0;

            for (int i = 0; i < 8; i++)
            {
                if (buf1[i] != 0xFF)
                    break;

                if (i == 7) return; // data is all defaults -- nothing to read
            }

            for (int i = 0; i < bands.Length; i++)
            {
                float val = 0.0f;
                if (index < 32)
                    val = BitConverter.ToSingle(buf1, index);
                else val = BitConverter.ToSingle(buf2, index - 32);
                index += 4;

                if (val > 500.0f || val < -500.0f)
                {
                    TextWriter writer = new StreamWriter(app_data_path + "\\eeprom_error.log", true);
                    writer.WriteLine(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + " " +
                        "Error reading TX Image Gain value from EEPROM -- sn: " + SerialToString(serial_number) + " > " +
                        bands[i].ToString() + " - Value out of range [-400.0, 400.0] (" + val.ToString("f4") + ").");
                    writer.Close();
                    val = 0.0f;
                }

                gain_table[(int)bands[i]] = val;
            }

            addr = 0x30E0;
            USBHID.ReadEEPROM(addr, 32, out buf1);
            addr += 32;
            USBHID.ReadEEPROM(addr, 32, out buf2);
            index = 0;

            for (int i = 0; i < 8; i++)
            {
                if (buf1[i] != 0xFF)
                    break;

                if (i == 7) return; // data is all defaults -- nothing to read
            }

            for (int i = 0; i < bands.Length; i++)
            {
                float val = 0.0f;
                if (index < 32)
                    val = BitConverter.ToSingle(buf1, index);
                else val = BitConverter.ToSingle(buf2, index - 32);
                index += 4;

                if (val > 400.0f || val < -400.0f)
                {
                    TextWriter writer = new StreamWriter(app_data_path + "\\eeprom_error.log", true);
                    writer.WriteLine(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + " " +
                        "Error reading TX Image Phase value from EEPROM -- " + serial_number + " -- " +
                        bands[i].ToString() + " - Value out of range [-400.0, 400.0] (" + val.ToString("f4") + ").");
                    writer.Close();
                    val = 0.0f;
                }

                phase_table[(int)bands[i]] = val;
            }
        }

        public static byte ReadTXImageGainChecksum()
        {
            byte read;
            USBHID.ReadTRXEEPROMByte(0x30DF, out read);
            return read;
        }

        public static byte ReadTXImagePhaseChecksum()
        {
            byte read;
            USBHID.ReadTRXEEPROMByte(0x310F, out read);
            return read;
        }

        #endregion

        #endregion

        #region PA

        #region PA Power

        public static bool CheckPAPower()
        {
            Random rand = new Random();
            Band[] bands = { Band.B160M, Band.B80M, Band.B60M, Band.B40M, Band.B30M, Band.B20M,
                               Band.B17M, Band.B15M, Band.B12M, Band.B10M, Band.B6M };

            float[][] pa_power_table = new float[(int)Band.LAST][];
            for (int i = 0; i < (int)Band.LAST; i++)
                pa_power_table[i] = new float[13];

            for (int i = 0; i < bands.Length; i++)
                pa_power_table[(int)bands[i]][0] = (float)Math.Round((float)rand.NextDouble(), 4);

            byte temp;
            WritePAPower(pa_power_table, out temp);

            float[][] pa_power_check = new float[(int)Band.LAST][];
            for (int i = 0; i < (int)Band.LAST; i++)
                pa_power_check[i] = new float[13];

            ReadPAPower(pa_power_check);

            for (int i = 0; i < bands.Length; i++)
            {
                if (pa_power_table[(int)bands[i]][0] != pa_power_check[(int)bands[i]][0])
                    return false;
            }
            return true;
        }

        public static void WritePAPower(float[][] table, out byte checksum)
        {
            WriteCalDateTime();
            Band[] bands = { Band.B160M, Band.B80M, Band.B60M, Band.B40M, Band.B30M, Band.B20M,
                               Band.B17M, Band.B15M, Band.B12M, Band.B10M, Band.B6M };

            ushort addr = 0x3030;
            byte[] buf = new byte[32];
            int length = 0;

            for (int i = 0; i < bands.Length; i++)
            {
                short val = (short)(Math.Round(table[(int)bands[i]][0], 4) * 10000);
                BitConverter.GetBytes(val).CopyTo(buf, length);
                length += 2;
            }

            if (length > 0)
            {
                if (!CheckedWrite(addr, buf, (byte)length))
                {
                    MessageBox.Show("Error writing PA Power value to EEPROM.");
                    checksum = 0xFF;
                    return;
                }
            }

            // calculate and write checksum
            byte sum = Checksum.Calc(table);
            WritePAPowerChecksum(sum);
            checksum = sum;
        }

        public static void WritePAPowerChecksum(byte sum)
        {
            if (!CheckedWrite(0x304F, sum))
                MessageBox.Show("Error writing PA Power checksum to EEPROM.");
        }

        public static void ReadPAPower(float[][] table)
        {
            Band[] bands = { Band.B160M, Band.B80M, Band.B60M, Band.B40M, Band.B30M, Band.B20M,
                               Band.B17M, Band.B15M, Band.B12M, Band.B10M, Band.B6M };

            ushort addr = 0x3030;
            byte[] buf = new byte[32];
            USBHID.ReadEEPROM(addr, 32, out buf);
            int index = 0;

            if (buf[0] == 0xFF && buf[1] == 0xFF) return;

            // check for shifted data
            /*bool bad_early_data = false;
            for (int i = 0; i < 8; i++)
            {
                short val = BitConverter.ToInt16(buf, i * 2);
                float f = (float)(val / 10000.0);
                if (f > 10.0 || f < 0.0)
                {
                    bad_early_data = true;
                    break;
                }
            }

            bool bad_late_data = false;
            if (bad_early_data)
            {
                byte[] buf1 = new byte[6];
                USBHID.ReadEEPROM(0x3050, 6, out buf1);

                for (int i = 0; i < 3; i++)
                {
                    short val = BitConverter.ToInt16(buf1, i * 2);
                    float f = (float)(val / 10000.0);
                    if (f > 10.0 || f < 0.0)
                    {
                        bad_late_data = true;
                        break;
                    }
                }
            }

            // verifies that the middle of the RX Image Gain table is unwritten
            bool blank_image_data = true;
            if (bad_early_data)
            {
                byte[] buf2 = new byte[16];
                USBHID.ReadEEPROM(0x3060, 16, out buf2);

                for (int i = 0; i < 16; i++)
                {
                    if (buf2[i] != 0xFF)
                    {
                        blank_image_data = false;
                        break;
                    }
                }
            }

            if (bad_early_data && bad_late_data) // data is invalid -- how to handle this??
            {
                MessageBox.Show("Error E8390: Please contact technical support with this number.\n"+
                "You may experience low output power until this is corrected.",
                    "Error E8390",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                for (int i = 0; i < bands.Length; i++)
                    table[(int)bands[i]][0] = 0.1f;
                return;
            }
            else if (bad_early_data && !bad_late_data && blank_image_data) // data is shifted -- move it to the appropriate place
            {
                StreamWriter sw = new StreamWriter(app_data_path + "error.log", true);
                sw.WriteLine(DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + ": " +
                    "FLEX-1500 Power Cal Data shifted -- corrective measures taken.");
                sw.WriteLine("Before:");
                sw.Write("\t0x3030: ");
                for(int i=0; i<16; i++)
                    sw.Write(buf[i].ToString("X").PadLeft(2, '0')+" ");
                sw.WriteLine("");
                sw.Write("\t0x3040: ");
                for (int i = 0; i < 16; i++)
                    sw.Write(buf[i + 16].ToString("X").PadLeft(2, '0') + " ");
                sw.WriteLine("");

                byte[] buf1 = new byte[6];
                USBHID.ReadEEPROM(0x3050, 6, out buf1);

                sw.Write("\t0x3050: ");
                for (int i = 0; i < 6; i++)
                    sw.Write(buf1[i].ToString("X").PadLeft(2, '0') + " ");
                sw.WriteLine("");

                // shift data back 16 bytes
                Array.Copy(buf, 16, buf, 0, 16);                
                Array.Copy(buf1, 0, buf, 16, 6);

                // reset remaining bytes to the default 0xFF
                byte[] buf2 = new byte[10];
                for(int i=0; i<10; i++)
                    buf2[i] = 0xFF;
                Array.Copy(buf2, 0, buf, 22, 10);

                // handle 15m getting overwritten by the checksum
                Array.Copy(buf, 12, buf, 14, 2); // copy 15m from 17m

                // write data back to EEPROM
                USBHID.WriteEEPROM(0x3030, buf);

                // read corrected data back into buffer for following work
                USBHID.ReadEEPROM(addr, 32, out buf);

                // write 0xFF to 0x3050 row
                buf1 = new byte[16];
                for (int i = 0; i < 16; i++)
                    buf1[i] = 0xFF;
                USBHID.WriteEEPROM(0x3050, buf1);

                sw.WriteLine("After:");
                sw.Write("\t0x3030: ");
                for (int i = 0; i < 16; i++)
                    sw.Write(buf[i].ToString("X").PadLeft(2, '0') + " ");
                sw.WriteLine("");
                sw.Write("\t0x3040: ");
                for (int i = 0; i < 16; i++)
                    sw.Write(buf[i + 16].ToString("X").PadLeft(2, '0') + " ");
                sw.WriteLine("");
                sw.Write("\t0x3050: ");
                for (int i = 0; i < 6; i++)
                    sw.Write(buf1[i].ToString("X").PadLeft(2, '0') + " ");
                sw.WriteLine("");
                sw.Close();
            }*/

            // read the data from the EEPROM into the table
            for (int i = 0; i < bands.Length; i++)
            {
                short val = 0;
                val = BitConverter.ToInt16(buf, index);
                index += 2;

                table[(int)bands[i]][0] = (float)(val / 10000.0);
            }

            /*if(bad_early_data && !bad_late_data)
            {
                // recalculate and write checksum
                WritePAPowerChecksum(Checksum.Calc(table));
            }*/
        }

        public static byte ReadPAPowerChecksum()
        {
            byte read;
            USBHID.ReadTRXEEPROMByte(0x304F, out read);
            return read;
        }

        #endregion

        #endregion
    }
}