//=================================================================
// CodecDebug.cs
//=================================================================
// PowerSDR is a C# implementation of a Software Defined Radio.
// Copyright (C) 2010-2012  FlexRadio Systems
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

using System.IO;
using System;
using System.Text;

namespace PowerSDR
{
    public class CodecDebug
    {
        public static void Dump(string filename)
        {
            openFile(filename);
            Regprint("Page Select Register", 0x00);
            Regprint("Software Reset Register", 0x01);
            Regprint("Codec Sample Rate Select Register", 0x02);
            Regprint("PLL Programming Register A", 0x03);
            Regprint("PLL Programming Register B", 0x4);
            Regprint("PLL Programming Register C", 0x5);
            Regprint("PLL Programming Register D", 0x6);
            Regprint("Codec Datapath Setup Register", 0x7);
            Regprint("Audio Serial Data Interface Control Register A", 0x8);
            Regprint("Audio Serial Data Interface Control Register B", 0x9);
            Regprint("Audio Serial Data Interface Control Register C", 0x0a);
            Regprint("Audio Codec Overflow Flag Register", 0x0b);
            Regprint("Audio Codec Digital Filter Control Register", 0x0c);
            Regprint("Headset / Button Press Detection Register A", 0x0d);
            Regprint("Headset / Button Press Detection Register B", 0xe);
            Regprint("Left ADC PGA Gain Control Register", 0xf);
            Regprint("Right ADC PGA Gain Control Register", 0x10);
            Regprint("MIC3L/R to Left ADC Control Register", 0x11);
            Regprint("MIC3L/R to Right ADC Control Register", 0x12);
            Regprint("LINE1L to Left ADC Control Register", 0x13);
            Regprint("LINE2L to Left(1) ADC Control Register", 0x14);
            Regprint("LINE1R to Left ADC Control Register", 0x15);
            Regprint("LINE1R to Right ADC Control Register", 0x16);
            Regprint("LINE2R to Right ADC Control Register", 0x17);
            Regprint("LINE1L to Right ADC Control Register", 0x18);
            Regprint("MICBIAS Control Register", 0x19);
            Regprint("Left AGC Control Register A", 0x1a);
            Regprint("Left AGC Control Register B", 0x1b);
            Regprint("Left AGC Control Register C", 0x1c);
            Regprint("Right AGC Control Register A", 0x1d);
            Regprint("Right AGC Control Register B", 0x1e);
            Regprint("Right AGC Control Register C", 0x1f);
            Regprint("Left AGC Gain Register", 0x20);
            Regprint("Right AGC Gain Register", 0x21);
            Regprint("Left AGC Noise Gate Debounce Register", 0x22);
            Regprint("Right AGC Noise Gate Debounce Register", 0x23);
            Regprint("ADC Flag Register", 0x24);
            Regprint("DAC Power and Output Driver Control Register", 0x25);
            Regprint("High Power Output Driver Control Register", 0x26);
            Regprint("Reserved Register", 0x27);
            Regprint("High Power Output Stage Control Register", 0x28);
            Regprint("DAC Output Switching Control Register", 0x29);
            Regprint("Output Driver Pop Reduction Register", 0x2a);
            Regprint("Left DAC Digital Volume Control Register", 0x2b);
            Regprint("Right DAC Digital Volume Control Register", 0x2c);
            Regprint("LINE2L to HPLOUT Volume Control Register", 0x2d);
            Regprint("PGA_L to HPLOUT Volume Control Register", 0x2e);
            Regprint("DAC_L1 to HPLOUT Volume Control Register", 0x2f);
            Regprint("LINE2R to HPLOUT Volume Control Register", 0x30);
            Regprint("PGA_R to HPLOUT Volume Control Register", 0x31);
            Regprint("DAC_R1 to HPLOUT Volume Control Register", 0x32);
            Regprint("HPLOUT Output Level Control Register", 0x33);
            Regprint("LINE2L to HPLCOM Volume Control Register", 0x34);
            Regprint("PGA_L to HPLCOM Volume Control Register", 0x35);
            Regprint("DAC_L1 to HPLCOM Volume Control Register", 0x36);
            Regprint("LINE2R to HPLCOM Volume Control Register", 0x37);
            Regprint("PGA_R to HPLCOM Volume Control Register", 0x38);
            Regprint("DAC_R1 to HPLCOM Volume Control Register", 0x39);
            Regprint("HPLCOM Output Level Control Register", 0x3a);
            Regprint("LINE2L to HPROUT Volume Control Register", 0x3b);
            Regprint("PGA_L to HPROUT Volume Control Register", 0x3c);
            Regprint("DAC_L1 to HPROUT Volume Control Register", 0x3d);
            Regprint("LINE2R to HPROUT Volume Control Register", 0x3e);
            Regprint("PGA_R to HPROUT Volume Control Register", 0x3f);
            Regprint("DAC_R1 to HPROUT Volume Control Register", 0x40);
            Regprint("HPROUT Output Level Control Register", 0x41);
            Regprint("LINE2L to HPRCOM Volume Control Register", 0x42);
            Regprint("PGA_L to HPRCOM Volume Control Register", 0x43);
            Regprint("DAC_L1 to HPRCOM Volume Control Register", 0x44);
            Regprint("LINE2R to HPRCOM Volume Control Register", 0x45);
            Regprint("PGA_R to HPRCOM Volume Control Register", 0x46);
            Regprint("DAC_R1 to HPRCOM Volume Control Register", 0x47);
            Regprint("HPRCOM Output Level Control Register", 0x48);
            Regprint("LINE2L to MONO_LOP/M Volume Control Register", 0x49);
            Regprint("PGA_L to MONO_LOP/M Volume Control Register", 0x4a);
            Regprint("DAC_L1 to MONO_LOP/M Volume Control Register", 0x4b);
            Regprint("LINE2R to MONO_LOP/M Volume Control Register", 0x4c);
            Regprint("PGA_R to MONO_LOP/M Volume Control Register", 0x4d);
            Regprint("DAC_R1 to MONO_LOP/M Volume Control Register", 0x4e);
            Regprint("MONO_LOP/M Output Level Control Register", 0x4f);
            Regprint("LINE2L to LEFT_LOP/M Volume Control Register", 0x50);
            Regprint("PGA_L to LEFT_LOP/M Volume Control Register", 0x51);
            Regprint("DAC_L1 to LEFT_LOP/M Volume Control Register", 0x52);
            Regprint("LINE2R to LEFT_LOP/M Volume Control Register", 0x53);
            Regprint("PGA_R to LEFT_LOP/M Volume Control Register", 0x54);
            Regprint("DAC_R1 to LEFT_LOP/M Volume Control Register", 0x55);
            Regprint("LEFT_LOP/M Output Level Control Register", 0x56);
            Regprint("LINE2L to RIGHT_LOP/M Volume Control Register", 0x57);
            Regprint("PGA_L to RIGHT_LOP/M Volume Control Register", 0x58);
            Regprint("DAC_L1 to RIGHT_LOP/M Volume Control Register", 0x59);
            Regprint("LINE2R to RIGHT_LOP/M Volume Control Register", 0x5a);
            Regprint("PGA_R to RIGHT_LOP/M Volume Control Register", 0x5b);
            Regprint("DAC_R1 to RIGHT_LOP/M Volume Control Register", 0x5c);
            Regprint("RIGHT_LOP/M Output Level Control Register", 0x5d);
            Regprint("Module Power Status Register", 0x5e);
            Regprint("Output Driver Short Circuit Detection Status Register", 0x5f);
            Regprint("Sticky Interrupt Flags Register", 0x60);
            Regprint("Real-time Interrupt Flags Register", 0x61);
            Regprint("GPIO1 Control Register", 0x62);
            Regprint("GPIO2 Control Register", 0x63);
            Regprint("Additional GPIO Control Register A", 0x64);
            Regprint("Additional GPIO Control Register B", 0x65);
            Regprint("Clock Generation Control Register", 0x66);
            Regprint("Reserved Registers", 0x67);
            closeFile();
        }

        private static TextWriter tw;

        private static void Regprint_old(string label, byte register)
        {
            byte return_value;
            USBHID.ReadI2CValue(0x30, register, out return_value);
            // write a line of text to the file
            tw.WriteLine(register.ToString("X").PadLeft(2, '0')+" 0x"+return_value.ToString("X").PadLeft(2, '0')+" "+Convert.ToString(return_value, 2).PadLeft(8, '0')+" "+label);
        }

        private static int[] regs = new int[0x70];
        private static StringBuilder sb;

        private static void Regprint(string label, byte register)
        {
            byte return_value;
            USBHID.ReadI2CValue(0x30, register, out return_value);
            string indicator = (return_value == regs[register] ? " " : "*");
            // write a line of text to the file
            sb.Append(indicator+register.ToString("X").PadLeft(2, '0') + " 0x" + return_value.ToString("X").PadLeft(2, '0') + " " + Convert.ToString(return_value, 2).PadLeft(8, '0') + " " + label + "\n");
            regs[register] = return_value;
        }

        private static void openFile(string filename)
        {
            tw = new StreamWriter(filename);
            sb = new StringBuilder();
        }

        private static void closeFile()
        {
            tw.WriteLine(sb.ToString());
            tw.Close();
        }
    }
}