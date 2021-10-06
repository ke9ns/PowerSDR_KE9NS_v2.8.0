//=================================================================
// checksum.cs
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
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace PowerSDR
{
    /// <summary>
    /// Summary description for checksum.
    /// </summary>
    public class Checksum
    {
        public static bool Match(SortedDictionary<double, uint> dict, byte sum)
        {
            return (Calc(dict) == sum);
        }

        public static bool Match(int[][] table, byte sum, bool only_band_indexes)
        {
            return (Calc(table, only_band_indexes) == sum);
        }

        public static bool Match(float[] table1, float[] table2, byte sum)
        {
            return ((byte)(Calc(table1, table2)) == sum);
        }

        public static bool Match(float[] table, byte sum)
        {
            return (Calc(table) == sum);
        }

        public static bool MatchHF(float[] table, byte sum)
        {
            return (CalcHF(table) == sum);
        }

        public static bool Match(float[][] table, byte sum)
        {
            return (Calc(table) == sum);
        }

        public static byte Calc(int[][] table, bool only_band_indexes) // = false for pa bias, true for tx carrier
        {
            Band[] bands = { Band.B160M, Band.B80M, Band.B60M, Band.B40M, Band.B30M, Band.B20M,
                               Band.B17M, Band.B15M, Band.B12M, Band.B10M, Band.B6M };

            if (!only_band_indexes)
            {
                byte[] data = new byte[table.Length * table[0].Length];
                for (int i = 0; i < table.Length; i++)
                    for (int j = 0; j < table[0].Length; j++)
                        data[i * table[0].Length + j] = (byte)table[i][j];
                return Calc(data);
            }
            else
            {
                byte[] data = new byte[bands.Length * table[0].Length];
                for (int i = 0; i < bands.Length; i++)
                    for (int j = 0; j < table[0].Length; j++)
                        data[i * table[0].Length + j] = (byte)table[(int)bands[i]][j];
                return Calc(data);
            }
        }

        public static byte Calc(float[] table)
        {
            byte[] data = new byte[table.Length * 4];
            for (int i = 0; i < table.Length; i++)
                Array.Copy(BitConverter.GetBytes(table[i]), 0, data, i * 4, 4);

            return Calc(data);
        }

        public static byte Calc(float[] table1, float[] table2)
        {
            return (byte)(Calc(table1) + Calc(table2));
        }

        public static byte CalcHF(float[] table)
        {
            Band[] bands = { Band.B160M, Band.B80M, Band.B60M, Band.B40M, Band.B30M, Band.B20M,
                               Band.B17M, Band.B15M, Band.B12M, Band.B10M, Band.B6M };

            byte[] data = new byte[bands.Length * 4];
            for (int i = 0; i < bands.Length; i++)
            {
                byte[] temp = BitConverter.GetBytes(table[(int)bands[i]]);
                for (int j = 0; j < 4; j++)
                    data[i * 4 + j] = temp[j];
            }
            return Calc(data);
        }

        public static byte Calc(float[][] table)
        {
            Band[] bands = { Band.B160M, Band.B80M, Band.B60M, Band.B40M, Band.B30M, Band.B20M,
                               Band.B17M, Band.B15M, Band.B12M, Band.B10M, Band.B6M };

            byte[] data = new byte[bands.Length * table[0].Length * 4];
            for (int i = 0; i < bands.Length; i++)
            {
                for (int j = 0; j < table[0].Length; j++)
                {
                    byte[] temp = BitConverter.GetBytes(table[(int)bands[i]][j]);
                    for (int k = 0; k < 4; k++)
                    {
                        data[i * table[0].Length * 4 + j * 4 + k] = temp[k];
                    }
                }
            }
            return Calc(data);
        }

        public static byte Calc(byte[] data)
        {
            byte sum = 0;
            for (int i = 0; i < data.Length; i++)
            {
                sum ^= data[i];
            }
            return sum;
        }

        public static byte Calc(SortedDictionary<double, uint> dict)
        {
            byte sum = 0;
            foreach (KeyValuePair<double, uint> pair in dict)
            {
                ulong freq = FWCEEPROM.ToVitaFreq(Math.Round(pair.Key, 3));
                for (int i = 0; i < 8; i++)
                    sum += (byte)(freq >> i * 8);

                for (int i = 0; i < 4; i++)
                    sum += (byte)(pair.Value >> i * 4);
            }

            return sum;
        }
    }
}
