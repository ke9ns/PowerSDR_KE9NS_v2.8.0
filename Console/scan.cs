//=================================================================
// scan.cs
// created by Darrin Kohn ke9ns
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
//
//=================================================================

//=================================================================

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;                    // ke9ns add for stringbuilder
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PowerSDR
{
    public partial class ScanControl : System.Windows.Forms.Form
    {

        public static Console console;   // ke9ns mod  to allow console to pass back values to setup screen
        public Setup setupForm;   // ke9ns communications with setupform  (i.e. allow combometertype.text update from inside console.cs) 

        //   private ArrayList file_list;
        private string wave_folder = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) + "\\PowerSDR";


        private IContainer components;


        #region Constructor and Destructor

        public ScanControl(Console c)
        {
            InitializeComponent();
            console = c;

            Common.RestoreForm(this, "ScanForm", true);



        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #endregion

        #region Properties



        #endregion

        #region Event Handlers







        #endregion




        //====================================================================================================
        int NamesTot = 0; // total number of unique Group names found in the memory list (no repeats)
        string[] Names = new string[1000]; // all the Group names found in the memory list (no repeats)

        StringBuilder sb = new StringBuilder();

        private void ScanControl_Load(object sender, EventArgs e)
        {

            comboMemGroupName.DataSource = console.MemoryList.List; // upon loading, load up the current memory listing into the combobox
            comboMemGroupName.DisplayMember = "Group";
            comboMemGroupName.ValueMember = "Group";

            dataGridView2.DataSource = console.MemoryList.List;   // ke9ns get list of memories from memorylist.cs is where the file is opened and saved
            Debug.WriteLine("Rows Count " + dataGridView2.Rows.Count);
            memcount = dataGridView2.Rows.Count;

            for (int i = 0; i < memcount; i++) // find all the memories with the same group name
            {

                for (int y = 0; y < NamesTot; y++) // recheck all prior names found 
                {
                    if (dataGridView2[0, i].Value.ToString() == Names[y]) // check if index matches name
                    {
                        goto rt1;
                    }
                }

                comboBoxTS1.Items.Add(dataGridView2[0, i].Value.ToString());  // accumulate a combobox list of Group Memory names (no repeats)

                Names[NamesTot] = dataGridView2[0, i].Value.ToString(); // save new name for list
                NamesTot++;
                continue;

            rt1:
                Debug.WriteLine("Scanner Load groups: Found repeat ");

            } // for i loop

            // comboBoxTS1.DataSource = Names;


        } // ScanControl_Load

        private int band_index;
        public static byte ScanStop = 1; // controlled from console 0=run, 1=stop

        public static byte ScanRST = 0; // 1= pick up where you left off, 0=reset back to low_freq
        private string last_band;                           // Used in bandstacking algorithm

        public static bool ScanPause = false; // pause = true
        public static bool ScanRun = false; // run = true
        //=======================================================================================
        // ke9ns add    scan just the Band stacking reg 
        private void btnBandstack_Click(object sender, EventArgs e)
        {
            ST3.Stop();
            ST3.Reset();

            ScanPause = false;

            if (ScanRun == false) // if stopped
            {
                try
                {
                    // check to see if you are on a band button
                    for (nnn = 0; nnn < 41; nnn++)   // total number of possible Bands
                    {
                        if (band_list[nnn] == console.last_band) break; // this is the current band_list index 
                        if (nnn >= 40)
                        {
                            Debug.WriteLine("console.last_Band doesnt match anything");
                            return;
                        }
                    }
                }
                catch (Exception q)
                {
                    Debug.WriteLine("console.last_Band " + q);
                    return;
                }

                ScanRun = true; // start up the scanner

                currFBox.Text = "";
                for (int y = 0; y < 100; y++)
                {
                    memsignal[y] = null;
                }



                UpdateText2();

                Thread t = new Thread(new ThreadStart(SCAN2));
                t.Name = "Bandstack memory Scanner Thread";
                t.IsBackground = true;
                t.Priority = ThreadPriority.Normal;
                t.Start();

            }
            else
            {
                ScanRun = false; // turn off scanner

            }

        } // btnBandstack_Click

        public string[] band_list = {"160M", "80M", "60M", "40M", "30M", "20M", "17M",
                                     "15M", "12M", "10M", "6M", "2M", "WWV", "GEN",
                                      "LMF","120M","90M","61M","49M","41M","31M","25M",
                                     "22M","19M","16M","14M","13M","11M",
                                     "VHF0", "VHF1", "VHF2", "VHF3", "VHF4", "VHF5",
                                     "VHF6", "VHF7", "VHF8", "VHF9", "VHF10", "VHF11",
                                     "VHF12", "VHF13" };



        public int nnn = 0; // 0-41 based on last_band
        public double[] freq2 = new double[20];

        public string[] filter1 = new string[20];
        public string[] filter2 = new string[20]; // ke9ns add   F4 would indicate a unlocked bandstack memory, but F4L would indicate its a locked bandstank memory
        public string[] mode1 = new string[20];

        //========================================================================================
        //  BANDSTACK CURRFBOX text update
        void UpdateText2()
        {
            string filter, mode;
            double freq;

            for (nnn = 0; nnn < 41; nnn++) // total number of possible Bands
            {
                if (band_list[nnn] == console.last_band) break; // this is the current band_list index 

            }

            string temp1 = "";

            memtotal = 0;

            for (int ii = 0; ii < console.band_stacks[nnn]; ii++)
            {

                if (DB.GetBandStack(band_list[nnn], ii, out mode, out filter, out freq))
                {

                    string freq3 = freq.ToString("###0.000000"); // was N6 4 less than having index numbers

                    string name = console.last_band.ToString();

                    string mm = "BandStack Memories";

                    if (memsignal[ii] == null) memsignal[ii] = " ";

                    //  temp1 = temp1 + (ii + 1).ToString().PadLeft(2)       + ": " + freq3.PadLeft(12).Substring(0, 12) + " , " + name.PadRight(20).Substring(0, 20) + " , " + memsignal[ii].PadRight(20).Substring(0,20) + "\r\n";
                    temp1 = temp1 + (memtotal + 1).ToString().PadLeft(2) + ": " + mm.PadRight(20).Substring(0, 20) + ", " + freq3.PadLeft(12).Substring(0, 12) + ", " + name.PadRight(20).Substring(0, 20) + ", " + memsignal[memtotal].PadRight(20).Substring(0, 20) + "\r\n"; // 74 char long

                    memIndex[memtotal] = ii;
                    memtotal++; // test1

                } // if bandstack available for band
                else
                {
                    Debug.WriteLine("no bandstack for band " + band_list[nnn]);
                    break;
                }

            } // for


            currFBox.Text = temp1;

        } // UpdateText2()  BANDSTACK 

        //===========================================================================
        // Thread bandstack scanner
        private void SCAN2()
        {

            scantype = 2;

            ST2.Reset();
            ST3.Reset();

            Debug.WriteLine("SCANSTOP = " + ScanStop);
            btnBandstack.BackColor = Color.LightGreen;

            int lastSIG = 0;
            int lastSQL = 0;

            Debug.WriteLine("CONSOLE LAST BAND " + console.last_band + " , " + console.RX1Band);


            last_band = console.last_band; // get current band stack you are viewing now

            do // ScanRun
            {

                for (; ; )
                {

                    Thread.Sleep(50);

                    try
                    {
                        speed = (int)udspeedBox.Value;  // Convert.ToInt16(udspeedBox.Text);
                        Debug.WriteLine("SPEED " + speed);
                    }
                    catch (Exception)
                    {
                        speed = 50; // 50msec
                    }


                    incbandstack(); // go to next bandstack memory

                    currFBox.SelectionStart = band_index * linelength; // i * linelength
                    currFBox.SelectionLength = linelength;
                    currFBox.ScrollToCaret(); // keep highlighted line visable

                    ST2.Restart(); // restart timer over again

                    ScanStop = 0; // reset squelch

                    lastSIG = -400;
                    lastSQL = -400;
                    //-------------------------------------------------------
                    // timer
                    do // scan speed and scanPause
                    {
                        Thread.Sleep(50);

                        if (ScanPause == true) pausebtn.BackColor = Color.Yellow;
                        else pausebtn.BackColor = SystemColors.ControlLight;

                        if (ScanRun == false)
                        {
                            ScanPause = false;
                            goto RT2;
                        }

                        if (SIG > lastSIG)
                        {
                            lastSIG = SIG;
                        }

                        if (SQL > lastSQL)
                        {
                            lastSQL = SQL;
                        }

                        if (scanstop == true)
                        {
                            scanstop = false;  // reset
                            goto RT2;

                        }
                        if ((ScanStop == 1)) // if console detected squelch open
                        {
                            memsignal[band_index] = lastSQL.ToString().PadLeft(4) + ", " + lastSIG.ToString().PadLeft(4) + ", Squelch Break";

                            if ((chkBoxSQLBRK.Checked == true) && (ScanPause == false)) // if stop on squelch break, then stop now
                            {
                                ScanPause = true;
                                UpdateText2(); // update currFBox text

                                currFBox.SelectionStart = band_index * linelength; // i * linelength
                                currFBox.SelectionLength = linelength;

                                if (udPauseLength.Value > 0)
                                {
                                    Debug.WriteLine("ST3 TIMER STARTED ");
                                    ST3.Restart(); // start the pause timer
                                }
                                else
                                {
                                    ScanPause = false;
                                    ScanRun = false;
                                    goto RT2;

                                }
                                break;

                            }
                            else if ((chkBoxSQLBRKWait.Checked == true) && (ScanPause == false)) // if stop on squelch break, then stop now
                            {
                                ScanPause = true;
                                UpdateText2(); // update currFBox text

                                currFBox.SelectionStart = band_index * linelength; // i * linelength
                                currFBox.SelectionLength = linelength;

                                if (udPauseLength.Value > 0)
                                {
                                    Debug.WriteLine("ST3 TIMER STARTED ");
                                    ST3.Restart(); // start the pause timer
                                }
                                else
                                {
                                    ScanPause = false;
                                    ScanRun = false;
                                    goto RT2;

                                }
                                break;

                            }
                        }
                        else
                        {
                            memsignal[band_index] = lastSQL.ToString().PadLeft(4) + ", " + lastSIG.ToString().PadLeft(4) + ", ";
                        }
                        if (scanstop == true)
                        {
                            scanstop = false;  // reset
                            goto RT2;

                        }
                    } while ((ST2.ElapsedMilliseconds < speed) || (ScanPause == true));

                    //-------------------------------------------------------

                    UpdateText2(); // update currFBox text

                    currFBox.SelectionStart = band_index * linelength; // i * linelength
                    currFBox.SelectionLength = linelength;

                    if (SP5_Active == 1)
                    {
                        ScanRun = false;
                        ScanPause = false;
                        Debug.WriteLine("SCANSTOP, another scanner started");
                        break;
                    }


                    //-------------------------------------------------------
                    // CHECK For PAUSE
                    while (ScanPause == true)  // wait here in in pause
                    {

                        Thread.Sleep(50);

                        if (ScanRun == false)
                        {
                            Debug.WriteLine("SCANSTOP, Group scanner turned back off");
                            ScanPause = false;
                            break;
                        }

                        if (ST3.ElapsedMilliseconds > ((long)udPauseLength.Value * 1000))
                        {
                            ST3.Stop(); // stop the pause timer

                            if (chkBoxSQLBRKWait.Checked == true && ScanStop == 1)
                            {
                                ScanPause = true;
                                ScanStop = 0;
                                ST3.Restart();

                            }
                            else ScanPause = false;

                            Debug.WriteLine("ST3 TIMER REACHED PAUSELENGTH ");
                        }

                        if (ScanPause == true) pausebtn.BackColor = Color.Yellow;
                        else pausebtn.BackColor = SystemColors.ControlLight;

                    };

                    pausebtn.BackColor = SystemColors.ControlLight;


                } // FOR ;; loop

            } while (ScanRun == true); // ScanStopped so leave thread


        RT2: ST2.Stop();
            ST3.Stop();
            Debug.WriteLine("SCANTOP0"); // scanner done
            btnBandstack.BackColor = SystemColors.ControlLight;
            pausebtn.BackColor = SystemColors.ControlLight;
            //   scantype = 0;

        } // SCAN2   BANDSTACK memory scanner





        //================================================================================================
        // increment through the bandstack (as though you were clicking on the same band button over and over
        public void incbandstack()
        {
            string filter, mode;
            double freq;


            if (last_band.Equals("160M"))
            {
                console.band_160m_index = (console.band_160m_index + 1) % console.band_160m_register;
                band_index = console.band_160m_index;
            }
            else if (last_band.Equals("80M"))
            {
                console.band_80m_index = (console.band_80m_index + 1) % console.band_80m_register;
                band_index = console.band_80m_index;
            }
            else if (last_band.Equals("60M"))
            {
                console.band_60m_index = (console.band_60m_index + 1) % console.band_60m_register;
                band_index = console.band_60m_index;
            }
            else if (last_band.Equals("40M"))
            {
                console.band_40m_index = (console.band_40m_index + 1) % console.band_40m_register;
                band_index = console.band_40m_index;
            }
            else if (last_band.Equals("30M"))
            {
                console.band_30m_index = (console.band_30m_index + 1) % console.band_30m_register;
                band_index = console.band_30m_index;
            }
            else if (last_band.Equals("20M"))
            {
                console.band_20m_index = (console.band_20m_index + 1) % console.band_20m_register;
                band_index = console.band_20m_index;
            }
            else if (last_band.Equals("17M"))
            {
                console.band_17m_index = (console.band_17m_index + 1) % console.band_17m_register;
                band_index = console.band_17m_index;
            }
            else if (last_band.Equals("15M"))
            {
                console.band_15m_index = (console.band_15m_index + 1) % console.band_15m_register;
                band_index = console.band_15m_index;
            }
            else if (last_band.Equals("12M"))
            {
                console.band_12m_index = (console.band_12m_index + 1) % console.band_12m_register;
                band_index = console.band_12m_index;
            }
            else if (last_band.Equals("10M"))
            {
                console.band_10m_index = (console.band_10m_index + 1) % console.band_10m_register;
                band_index = console.band_10m_index;
            }
            else if (last_band.Equals("6M"))
            {
                console.band_6m_index = (console.band_6m_index + 1) % console.band_6m_register;
                band_index = console.band_6m_index;
            }
            else if (last_band.Equals("2M"))
            {
                console.band_2m_index = (console.band_2m_index + 1) % console.band_2m_register;
                band_index = console.band_2m_index;
            }
            else if (last_band.Equals("WWV"))
            {
                console.band_wwv_index = (console.band_wwv_index + 1) % console.band_wwv_register;
                band_index = console.band_wwv_index;
            }
            else if (last_band.Equals("GEN"))
            {
                console.band_gen_index = (console.band_gen_index + 1) % console.band_gen_register;
                band_index = console.band_gen_index;
            }
            else if (last_band.Equals("VHF0"))
            {
                console.band_vhf0_index = (console.band_vhf0_index + 1) % console.band_vhf0_register;
                band_index = console.band_vhf0_index;
            }
            else if (last_band.Equals("VHF1"))
            {
                console.band_vhf1_index = (console.band_vhf1_index + 1) % console.band_vhf1_register;
                band_index = console.band_vhf1_index;
            }
            else if (last_band.Equals("VHF2"))
            {
                console.band_vhf2_index = (console.band_vhf2_index + 1) % console.band_vhf2_register;
                band_index = console.band_vhf2_index;
            }
            else if (last_band.Equals("VHF3"))
            {
                console.band_vhf3_index = (console.band_vhf3_index + 1) % console.band_vhf3_register;
                band_index = console.band_vhf3_index;
            }
            else if (last_band.Equals("VHF4"))
            {
                console.band_vhf4_index = (console.band_vhf4_index + 1) % console.band_vhf4_register;
                band_index = console.band_vhf4_index;
            }
            else if (last_band.Equals("VHF5"))
            {
                console.band_vhf5_index = (console.band_vhf5_index + 1) % console.band_vhf5_register;
                band_index = console.band_vhf5_index;
            }
            else if (last_band.Equals("VHF6"))
            {
                console.band_vhf6_index = (console.band_vhf6_index + 1) % console.band_vhf6_register;
                band_index = console.band_vhf6_index;
            }
            else if (last_band.Equals("VHF7"))
            {
                console.band_vhf7_index = (console.band_vhf7_index + 1) % console.band_vhf7_register;
                band_index = console.band_vhf7_index;
            }
            else if (last_band.Equals("VHF8"))
            {
                console.band_vhf8_index = (console.band_vhf8_index + 1) % console.band_vhf8_register;
                band_index = console.band_vhf8_index;
            }
            else if (last_band.Equals("VHF9"))
            {
                console.band_vhf9_index = (console.band_vhf9_index + 1) % console.band_vhf9_register;
                band_index = console.band_vhf9_index;
            }
            else if (last_band.Equals("VHF10"))
            {
                console.band_vhf10_index = (console.band_vhf10_index + 1) % console.band_vhf10_register;
                band_index = console.band_vhf10_index;
            }
            else if (last_band.Equals("VHF11"))
            {
                console.band_vhf11_index = (console.band_vhf11_index + 1) % console.band_vhf11_register;
                band_index = console.band_vhf11_index;
            }
            else if (last_band.Equals("VHF12"))
            {
                console.band_vhf12_index = (console.band_vhf12_index + 1) % console.band_vhf12_register;
                band_index = console.band_vhf12_index;
            }
            else if (last_band.Equals("VHF13"))
            {
                console.band_vhf13_index = (console.band_vhf13_index + 1) % console.band_vhf13_register;
                band_index = console.band_vhf13_index;
            }
            else if (last_band.Equals("LMF"))
            {
                console.band_LMF_index = (console.band_LMF_index + 1) % console.band_LMF_register;
                band_index = console.band_LMF_index;
            }
            else if (last_band.Equals("120M"))
            {
                console.band_120m_index = (console.band_120m_index + 1) % console.band_120m_register;
                band_index = console.band_120m_index;
            }
            else if (last_band.Equals("90M"))
            {
                console.band_90m_index = (console.band_90m_index + 1) % console.band_90m_register;
                band_index = console.band_90m_index;
            }
            else if (last_band.Equals("61M"))
            {
                console.band_61m_index = (console.band_61m_index + 1) % console.band_61m_register;
                band_index = console.band_61m_index;
            }
            else if (last_band.Equals("49M"))
            {
                console.band_49m_index = (console.band_49m_index + 1) % console.band_49m_register;
                band_index = console.band_49m_index;
            }
            else if (last_band.Equals("41M"))
            {
                console.band_41m_index = (console.band_41m_index + 1) % console.band_41m_register;
                band_index = console.band_41m_index;
            }
            else if (last_band.Equals("31M"))
            {
                console.band_31m_index = (console.band_31m_index + 1) % console.band_31m_register;
                band_index = console.band_31m_index;
            }
            else if (last_band.Equals("25M"))
            {
                console.band_25m_index = (console.band_25m_index + 1) % console.band_25m_register;
                band_index = console.band_25m_index;
            }
            else if (last_band.Equals("22M"))
            {
                console.band_22m_index = (console.band_22m_index + 1) % console.band_22m_register;
                band_index = console.band_22m_index;
            }
            else if (last_band.Equals("19M"))
            {
                console.band_19m_index = (console.band_19m_index + 1) % console.band_19m_register;
                band_index = console.band_19m_index;
            }
            else if (last_band.Equals("16M"))
            {
                console.band_16m_index = (console.band_16m_index + 1) % console.band_16m_register;
                band_index = console.band_16m_index;
            }
            else if (last_band.Equals("14M"))
            {
                console.band_14m_index = (console.band_14m_index + 1) % console.band_14m_register;
                band_index = console.band_16m_index;
            }
            else if (last_band.Equals("13M"))
            {
                console.band_13m_index = (console.band_13m_index + 1) % console.band_13m_register;
                band_index = console.band_13m_index;
            }
            else if (last_band.Equals("11M"))
            {
                console.band_11m_index = (console.band_11m_index + 1) % console.band_11m_register;
                band_index = console.band_11m_index;
            }
            else
            {
                return;
            }

            //---------------------------------------------------------------


            if (DB.GetBandStack(last_band, band_index, out mode, out filter, out freq))
            {
                if (filter.Contains("@"))
                {
                    filter = filter.Substring(0, (filter.Length) - 1); // ke9ns add for bandstack lockout
                }

                console.SetBand(mode, filter, freq);
            }


            console.UpdateWaterfallLevelValues();


        }  // incbandstack




        public int xxx = 0;
        //======================================================================== 
        public void updateindex()
        {
            switch (console.RX1Band)
            {
                case Band.B160M:
                    console.band_160m_index = xxx;

                    break;
                case Band.B80M:
                    console.band_80m_index = xxx;

                    break;
                case Band.B60M:
                    console.band_60m_index = xxx;

                    break;
                case Band.B40M:
                    console.band_40m_index = xxx;
                    break;
                case Band.B30M:
                    console.band_30m_index = xxx;
                    break;
                case Band.B20M:
                    console.band_20m_index = xxx;
                    break;
                case Band.B17M:
                    console.band_17m_index = xxx;
                    break;
                case Band.B15M:
                    console.band_15m_index = xxx;
                    break;
                case Band.B12M:
                    console.band_12m_index = xxx;
                    break;
                case Band.B10M:
                    console.band_10m_index = xxx;
                    break;
                case Band.B6M:
                    console.band_6m_index = xxx;
                    break;
                case Band.B2M:
                    console.band_2m_index = xxx;
                    break;
                case Band.WWV:
                    console.band_wwv_index = xxx;
                    break;
                case Band.GEN:
                    console.band_gen_index = xxx;
                    break;


                case Band.VHF0:
                    console.band_vhf0_index = xxx;
                    break;
                case Band.VHF1:
                    console.band_vhf1_index = xxx;
                    break;
                case Band.VHF2:
                    console.band_vhf2_index = xxx;
                    break;
                case Band.VHF3:
                    console.band_vhf3_index = xxx;
                    break;
                case Band.VHF4:
                    console.band_vhf4_index = xxx;
                    break;
                case Band.VHF5:
                    console.band_vhf5_index = xxx;
                    break;
                case Band.VHF6:
                    console.band_vhf6_index = xxx;
                    break;
                case Band.VHF7:
                    console.band_vhf7_index = xxx;
                    break;
                case Band.VHF8:
                    console.band_vhf8_index = xxx;
                    break;
                case Band.VHF9:
                    console.band_vhf9_index = xxx;
                    break;
                case Band.VHF10:
                    console.band_vhf10_index = xxx;
                    break;
                case Band.VHF11:
                    console.band_vhf11_index = xxx;
                    break;
                case Band.VHF12:
                    console.band_vhf12_index = xxx;
                    break;
                case Band.VHF13:
                    console.band_vhf13_index = xxx;
                    break;



                case Band.BLMF:                                                                     // ke9ns add down below vhf
                    console.band_LMF_index = xxx;
                    break;
                case Band.B120M:
                    console.band_120m_index = xxx;
                    break;
                case Band.B90M:
                    console.band_90m_index = xxx;
                    break;
                case Band.B61M:
                    console.band_61m_index = xxx;
                    break;
                case Band.B49M:
                    console.band_49m_index = xxx;
                    break;
                case Band.B41M:
                    console.band_41m_index = xxx;
                    break;
                case Band.B31M:
                    console.band_31m_index = xxx;
                    break;
                case Band.B25M:
                    console.band_25m_index = xxx;
                    break;
                case Band.B22M:
                    console.band_22m_index = xxx;
                    break;

                case Band.B19M:
                    console.band_19m_index = xxx;
                    break;

                case Band.B16M:
                    console.band_16m_index = xxx;
                    break;
                case Band.B14M:
                    console.band_14m_index = xxx;
                    break;

                case Band.B13M:
                    console.band_13m_index = xxx;
                    break;

                case Band.B11M:
                    console.band_11m_index = xxx;
                    break;

            } // switch rx1band

        } // updateindex


        //=======================================================================================================================
        private void ScanControl_FormClosing(object sender, FormClosingEventArgs e)
        {

            Debug.WriteLine("==========CLOSING SCANNER============");

            ScanRun = false;
            ScanPause = false;
            ScanStop = 1;

            this.Hide();
            e.Cancel = true;
            Common.SaveForm(this, "ScanForm");
            //  console.MemoryList.Save();



        } // ScanControl_FormClosing


        MemoryRecord[] m1 = new MemoryRecord[1000];

        int[] memIndex = new int[1000]; // holder for memories that match the group name
        int memtotal = 0; // total matching group name memories found
        int memcount = 0; // total memories found
        string[] memsignal = new string[1000]; // db signal and sql brk

        bool ScanVFOB = false; // .236

        //=======================================================================================================================
        // Group memory scanner. Scanning only frequencies in 1 group name
        private void btnGroupMemory_Click(object sender, EventArgs e)
        {
            ST3.Stop();
            ST3.Reset();

            ScanPause = false;

            if (console.MemoryList.List.Count == 0) return; // nothing in the list, exit
            if (comboMemGroupName.Items.Count == 0) return;
            memcount = comboMemGroupName.Items.Count; // total number of memories listed

            Debug.WriteLine("memory list7 " + memcount);

            if (ScanRun == false) // if stoppedchange
            {
                currFBox.Text = "";

                for (int y = 0; y < 100; y++)
                {
                    memsignal[y] = null;
                }

                UpdateText();  // upate currFBox text

                ScanRun = true; // start up the scanner

                Thread t = new Thread(new ThreadStart(SCAN1));
                t.Name = "Group memory Scanner Thread";
                t.IsBackground = true;
                t.Priority = ThreadPriority.Normal;
                t.Start();

            }
            else
            {
                ScanRun = false; // turn off scanner
            }

        } // button6_Click


        //=======================================================================================================================
        // Group memory scanner. Scanning only frequencies in 1 group name
        private void btnGroupMemory1_Click(object sender, EventArgs e)
        {
            ST3.Stop();
            ST3.Reset();

            ScanPause = false;
            bandSwlupdate1(); // determine if there are any SWL stations ON the air that contain the name you just typed int the combobox of the scanner 


            if (swl_number < 1) return; // did not find any stations matching so return

            memcount = swl_number; // total number of memories listed


            Debug.WriteLine("memory list77 " + memcount);


            if (ScanRun == false) // if stoppedchange
            {
                currFBox.Text = "";

                for (int y = 0; y < 200; y++)
                {
                    memsignal[y] = null;
                }

                UpdateText1();  // upate currFBox text

                ScanRun = true; // start up the scanner

                Thread t = new Thread(new ThreadStart(SCAN6));
                t.Name = "Group memory Scanner Thread";
                t.IsBackground = true;
                t.Priority = ThreadPriority.Normal;
                t.Start();

            }
            else
            {
                ScanRun = false; // turn off scanner

            }

        } // btnGroupMemory1_Click


        MemoryRecord recordToRestore; // holder to select group name
        string Gname; // name of group of memories to scan
        string Gname1; // name of group of SWL to scan

        //==========================================================================================
        // ke9ns combobox to display ALL the group names from the memory listing
        private void comboMemGroupName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboMemGroupName.Items.Count == 0 || comboMemGroupName.SelectedItem == null) return;

            recordToRestore = new MemoryRecord((MemoryRecord)comboMemGroupName.SelectedItem); // ke9ns   you select index in the combo pulldown list


        } //  comboMemGroupName_SelectedIndexChanged


        //==========================================================================================
        // ke9ns combobox to display all the unique (SUB) group names from the memory listing
        private void comboBoxTS1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScanPause = false;
            ScanRun = false;
            scantype = 1;


            if (comboBoxTS1.Text == "")
            {
                Gname = "";
                memcount = 0;
                return;
            }


            textBox1.Text = "";


            Debug.WriteLine("[[[[[[[[[[[[[[[[[[[COMBOBOX EVENT]]]]]]]]]]]]]]]");

            //  if (comboBoxTS1.Items.Count == 0 || comboBoxTS1.SelectedItem == null) return;
            if (console.initializing == true) return;

            //  Gname = comboBoxTS1.SelectedItem.ToString();
            Gname = comboBoxTS1.Text;

            Debug.WriteLine("SELECTED GROUP NAME " + Gname);

            if (console.MemoryList.List.Count == 0) return; // nothing in the list, exit
            if (comboMemGroupName.Items.Count == 0) return;

            memcount = comboMemGroupName.Items.Count; // total number of memories listed

            Debug.WriteLine("memory list8 " + memcount);


            UpdateText(); // upate currFBox text

            Debug.WriteLine("memory list8a " + memcount);

            btnGroupMemory.Enabled = true;
            btnGroupMemory1.Enabled = false;

            Debug.WriteLine("memory list8b " + memcount);

        } // comboBoxTS1_SelectedIndexChanged


        private void comboBoxTS2_SelectedIndexChanged(object sender, KeyEventArgs e)
        {
            ScanPause = false;
            ScanRun = false;
            scantype = 1;

            //  comboBoxTS1.SelectedIndexChanged -= comboBoxTS1_SelectedIndexChanged;  // ke9ns turn off checkchanged temporarily   
            comboBoxTS1.Text = "";
            //  comboBoxTS1.SelectedIndexChanged += comboBoxTS1_SelectedIndexChanged;  //


            //    Debug.WriteLine("SWL TEXT TYPED" + textBox1.Text);


            if (e.KeyCode != Keys.Enter) return;

            //    Debug.WriteLine("SWL TEXT TYPED and contains a CR");

            Gname1 = textBox1.Text;

            //   Debug.WriteLine("SELECTED SWL NAME " + Gname1);

            //     Debug.WriteLine("SWL checking SWL listing");

            bandSwlupdate1(); // determine if there are any SWL stations ON the air that contain the name you just typed int the combobox of the scanner 

            Debug.WriteLine("[[[[[[[[[[[[[[[[[[[SWL List compiled]]]]]]]]]]]]]]]");

            if (console.initializing == true) return;

            Debug.WriteLine("SWL found size " + swl_number);

            if (swl_number < 1) return; // did not find any stations matching so return

            memcount = swl_number; // total number of memories listed

            UpdateText1(); // upate currFBox text

            btnGroupMemory1.Enabled = true;
            btnGroupMemory.Enabled = false;
        }
        //==========================================================================================
        // ke9ns combobox to display all the unique (SUB) group names from the memory listing
        private void comboBoxTS2_SelectedIndexChanged(object sender, EventArgs e)
        {


        } // comboBoxTS1_SelectedIndexChanged

        //========================================================================================
        //  Lookup SWL table and match user input to the SWL list and update currFBox text screen
        void UpdateText1()
        {

            memtotal = 0;

            string temp1 = "";

            Debug.WriteLine("1UPDATE LIST " + Gname1);

            for (int i = 0; i < memcount; i++) // find all the memories with the same group name
            {
                int zz = swl_index[i];

                double hh = (double)SpotControl.SWL_Freq[zz] / 1000000.0;
                //     double hh = Convert.ToDouble();  // SWL "RXFREQ"  convert to hz
                string freq = hh.ToString("###0.000000");    //  freq of SWL

                string name = SpotControl.SWL_Loc[zz];    // name of SWL
                string mm = SpotControl.SWL_Station[zz];  // GROUP of SWL

                if (memsignal[memtotal] == null) memsignal[memtotal] = " ";

                temp1 = temp1 + (memtotal + 1).ToString().PadLeft(2) + ": " + mm.PadRight(20).Substring(0, 20) + ", " + freq.PadLeft(12).Substring(0, 12) + ", " + name.PadRight(20).Substring(0, 20) + ", " + memsignal[memtotal].PadRight(20).Substring(0, 20) + "\r\n"; // 74 char long

                memIndex[memtotal] = i;
                memtotal++;

            } // for i loop

            currFBox.Text = temp1;

        } // UpdateText1()  SWL 



        //===================================================================================
        //===================================================================================
        //===================================================================================

        int[] swl_index = new int[20000];
        int swl_number = 0;

        void bandSwlupdate1()
        {

            int iii = 0;

            //   Debug.WriteLine("swl index size= " + SpotControl.SWL_Index1);

            Gname1 = Gname1.TrimEnd('\r', '\n');

            DateTime UTCD = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);

            SpotControl.UTCNEW1 = Convert.ToInt16(UTCD.ToString("HHmm")); // convert 24hr UTC to int

            iii = 0;

            int ii = 0;
            try
            {

                for (ii = 0; ii < SpotControl.SWL_Index1; ii++) // check all spots to see which ones are on at this particular time and day
                {
                    // station check 

                    if (CultureInfo.InvariantCulture.CompareInfo.IndexOf(SpotControl.SWL_Station[ii], Gname1, CompareOptions.IgnoreCase) >= 0) // Gname must be contains in MEMORY (partial or full) and case insensitive)
                    {

                        //   Debug.WriteLine("SCAN SWL FREQ " + SpotControl.SWL_Freq[ii] + " , " + ii + " , " + SpotControl.SWL_Station[ii]);

                        // station check days on air and time on air
                        if (
                            ((SpotControl.SWL_Day1[ii] & SpotControl.UTCDD) > 0) && (((SpotControl.SWL_TimeN[ii] <= SpotControl.UTCNEW1) && (SpotControl.SWL_TimeF[ii] >= SpotControl.UTCNEW1)) ||
                            ((SpotControl.SWL_TimeN[ii] >= SpotControl.UTCNEW1) && (SpotControl.SWL_TimeF[ii] >= SpotControl.UTCNEW1) && (SpotControl.SWL_TimeF[ii] < SpotControl.SWL_TimeN[ii])))
                             )
                        {
                            //   Debug.WriteLine("station found " + SpotControl.SWL_Freq[ii] + " , "+ SpotControl.SWL_Day1[ii]);

                            swl_index[iii++] = ii; // keep track of frequencies on at the moment

                            if (SpotControl.SWL_Station[ii].Length > 20) SpotControl.SWL_Station[ii] = SpotControl.SWL_Station[ii].Substring(0, 20);
                            SpotControl.SWL_Mode[ii] = "USB";

                            //      bigmessage += (String.Format("{0:00.000000}", (double)(SpotControl.SWL_Freq[ii]) / 1000000.0) +
                            //           "  " + SpotControl.SWL_Station[ii].PadRight(25, ' ') + " " + SpotControl.SWL_Loc[ii].PadRight(3, ' ') +
                            //         " " + SpotControl.SWL_TimeN[ii].ToString().PadLeft(4, '0') + ":" + SpotControl.SWL_TimeF[ii].ToString().PadLeft(4, '0') +
                            //        " " + "\r\n");


                        } // check time
                    } // text search to narrow down


                } // for loop through SWL_Index
            }
            catch (Exception e)
            {

                Debug.WriteLine("SWL Load problem " + ii + " , " + e);
            }

            Debug.WriteLine("SWL DONE " + (iii - 1));


            swl_number = iii - 1; // total swl stations found that contain the Gname1 swl name



        } // bandSwlupdate







        int linelength = 84; // length of a line in the currFbox
        //========================================================================================
        //  Lookup MEMORY table and match user input to the MEMORY list and update currFBox text screen
        void UpdateText()
        {

            memtotal = 0;

            string temp1 = "";

            Debug.WriteLine("UPDATE LIST " + Gname);

            for (int i = 0; i < memcount; i++) // find all the memories with the same group name
            {

                //  if ( (dataGridView2[0, i].Value.ToString()).Equals(Gname, StringComparison.InvariantCultureIgnoreCase) == true) // check if index matches name
                //    if (dataGridView2[0, i].Value.ToString() == Gname) // check if index matches name

                if (CultureInfo.InvariantCulture.CompareInfo.IndexOf((dataGridView2[0, i].Value.ToString()), Gname, CompareOptions.IgnoreCase) >= 0) // Gname must be contains in MEMORY (partial or full) and case insensitive)
                {
                    bool scan = (bool)dataGridView2["Scan", i].Value; // ke9ns add .155  

                    double hh = Convert.ToDouble(dataGridView2[1, i].Value);  // MEMORY "RXFREQ"  convert to hz
                    string freq = hh.ToString("###0.000000");    //  freq of memory  dataGridView2[1, i].Value.ToString();
                    string name = dataGridView2[2, i].Value.ToString(); // name of memory
                    string mm = dataGridView2[0, i].Value.ToString();  // GROUP of MEMORY

                    //  string comment = dataGridView2["comments", i].Value.ToString(); // comments of memory
                    //  int hh = (int)(Convert.ToDouble(SpotForm.dataGridView2[1, ii].Value) * 1000000);  // MEMORY "RXFREQ"  convert to hz
                    // string ll = (string)SpotForm.dataGridView2[2, holder2[ii]].Value;  // Name of MEMORY
                    //  DSPMode nn = (DSPMode)SpotForm.dataGridView2[3, holder2[ii]].Value;  // DSPMODE of MEMORY

                    //  Debug.WriteLine("UPDATE LIST A ");

                    if (memsignal[memtotal] == null) memsignal[memtotal] = " ";

                    //   Debug.WriteLine("UPDATE LIST B " + memsignal[memtotal]);

                  //  bool scan = (bool)dataGridView2["Scan", i].Value; // ke9ns add .155  

                    string Y;
                    if (scan == true) Y = " "; // .155
                    else Y = "X";

                    temp1 = temp1 + (memtotal + 1).ToString().PadLeft(2) + ":" + Y + " " + mm.PadRight(20).Substring(0, 20) + ", " + freq.PadLeft(12).Substring(0, 12) + ", " + name.PadRight(20).Substring(0, 20) + ", " + memsignal[memtotal].PadRight(19).Substring(0, 19) + "\r\n"; // 74 char long

                    memIndex[memtotal] = i;
                    memtotal++;
                    //  Debug.WriteLine("Found Group name match at index " + i);
                }

            } // for i loop

            currFBox.Text = temp1;

        } // UpdateText()  MEMORY 


        public static int SQL = 0;  // updated by console routine picSquelch_Paint
        public static int SIG = 0;

        Stopwatch ST2 = new Stopwatch();
        Stopwatch ST3 = new Stopwatch();

        int scantype = 0;  // 1=memory, 2=band stack, 3= custom, 4= low-high, 5=SWL
        bool scanstop = false;

        //==========================================================================================
        // thread scans selected "Memory" Group name frequencies only
        private void SCAN1()
        {
            ST2.Reset();
            ST3.Reset();

            scantype = 1;

            Debug.WriteLine("SCANSTOP = " + ScanStop);
            btnGroupMemory.BackColor = Color.LightGreen;

            int lastSIG = 0;
            int lastSQL = 0;

            ST3.Reset();

            int x = 0;

            do // ScanRun
            {
                if (scanstop == true)
                {
                    scanstop = false;  // reset
                    goto RT2;

                }
                Debug.WriteLine("START OF LOOP");

                for (x = 0; x < memtotal; x++) // go through list of MEMORIES you found
                {
                    Thread.Sleep(50);

                    try
                    {
                        speed = (int)udspeedBox.Value;  // Convert.ToInt16(udspeedBox.Text);
                        Debug.WriteLine("SPEED " + speed);
                    }
                    catch (Exception)
                    {
                        speed = 50; // 50msec
                    }


                    comboMemGroupName.SelectedIndex = memIndex[x];
                    recordToRestore = new MemoryRecord((MemoryRecord)comboMemGroupName.SelectedItem); // ke9ns   you select index in the combo pulldown list

                    if (recordToRestore.Scan == false) continue; // ke9ns add .155

                    Debug.WriteLine("CHANGE MEMORY TO " + recordToRestore.RXFreq);
                    console.RecallMemory(recordToRestore);


                    currFBox.SelectionStart = x * linelength; // i * linelength
                    currFBox.SelectionLength = linelength;
                    currFBox.ScrollToCaret(); // keep highlighted line visable

                    ST2.Restart(); // restart timer over again

                    ScanStop = 0; // reset squelch

                    lastSIG = -400;
                    lastSQL = -400;


                    //-------------------------------------------------------
                    // SPEED TIMER and PAUSE
                    do
                    {
                        Thread.Sleep(50);
                        if (scanstop == true)
                        {
                            scanstop = false;  // reset
                            goto RT2;

                        }

                        if (ScanPause == true) pausebtn.BackColor = Color.Yellow;
                        else pausebtn.BackColor = SystemColors.ControlLight;

                        if (ScanRun == false)
                        {
                            ScanPause = false;
                            goto RT2;  // turn off this thread now
                        }

                        if (SIG > lastSIG)  // CHECK SQUELCH and SIGNAL levels
                        {
                            lastSIG = SIG;
                        }

                        if (SQL > lastSQL)
                        {
                            lastSQL = SQL;
                        }


                        if ((ScanStop == 1)) // if console detected squelch open
                        {
                            memsignal[x] = lastSQL.ToString().PadLeft(4) + ", " + lastSIG.ToString().PadLeft(4) + ", SQL BRK";

                            if ((chkBoxSQLBRK.Checked == true) && (ScanPause == false)) // if stop on squelch break, then stop now
                            {
                                ScanPause = true;

                                UpdateText(); // update currFBox text

                                currFBox.SelectionStart = x * linelength; // i * linelength
                                currFBox.SelectionLength = linelength;

                                if (udPauseLength.Value > 0)
                                {
                                    Debug.WriteLine("ST3 TIMER STARTED " + memtotal);
                                    ST3.Restart(); // start the pause timer
                                }
                                else // if 0
                                {
                                    ScanPause = false;
                                    ScanRun = false;
                                    goto RT2;
                                }

                                break; // break out of the while loop
                            }
                            else if ((chkBoxSQLBRKWait.Checked == true) && (ScanPause == false)) // if stop on squelch break, then stop now
                            {
                                ScanPause = true;

                                UpdateText(); // update currFBox text

                                currFBox.SelectionStart = x * linelength; // i * linelength
                                currFBox.SelectionLength = linelength;

                                if (udPauseLength.Value > 0)
                                {
                                    Debug.WriteLine("ST3 TIMER STARTED " + memtotal);
                                    ST3.Restart(); // start the pause timer

                                }
                                else // if 0
                                {
                                    ScanPause = false;
                                    ScanRun = false;
                                    goto RT2;
                                }
                                break; // break out of the while loop

                            }


                        } // ScanStop == 1
                        else
                        {
                            memsignal[x] = lastSQL.ToString().PadLeft(4) + ", " + lastSIG.ToString().PadLeft(4) + ", ";
                        }

                        if (scanstop == true)
                        {
                            scanstop = false;  // reset
                            goto RT2;

                        }

                    } while ((ST2.ElapsedMilliseconds < speed) || (ScanPause == true));

                    //-----------------------------------------------------BREAK comes here--

                    UpdateText(); // update currFBox text

                    currFBox.SelectionStart = x * linelength; // i * linelength
                    currFBox.SelectionLength = linelength;

                    if (scanstop == true)
                    {
                        scanstop = false;  // reset
                        goto RT2;

                    }

                    if (SP5_Active == 1)
                    {
                        ScanRun = false;
                        ScanPause = false;
                        Debug.WriteLine("SCANSTOP, another scanner started");
                        break;
                    }


                    //-------------------------------------------------------
                    // CHECK For PAUSE
                    while (ScanPause == true)  // wait here in in pause
                    {
                        Thread.Sleep(50);

                        if (scanstop == true)
                        {
                            scanstop = false;  // reset
                            goto RT2;

                        }

                        if (ScanRun == false)
                        {
                            Debug.WriteLine("SCANSTOP, Group scanner turned back off");
                            ScanPause = false; //.219
                            scanstop = true;
                            break;
                        }

                        if (ST3.ElapsedMilliseconds > ((long)udPauseLength.Value * 1000))
                        {
                            ST3.Stop(); // stop the pause timer
                            if (chkBoxSQLBRKWait.Checked == true && ScanStop == 1)
                            {
                                ScanPause = true;
                                ScanStop = 0;
                                ST3.Restart();

                            }
                            else ScanPause = false;

                            Debug.WriteLine("ST3 TIMER REACHED PAUSELENGTH " + memtotal);
                        }

                        if (ScanPause == true) pausebtn.BackColor = Color.Yellow;
                        else pausebtn.BackColor = SystemColors.ControlLight;

                    }; //  while (ScanPause == true)  // wait here in in pause


                    pausebtn.BackColor = SystemColors.ControlLight;

                    Debug.WriteLine("END OF LOOP " + memtotal + " , " + x);

                    if (scanstop == true)
                    {
                        scanstop = false;  // reset
                        goto RT2;

                    }

                } // for (x = 0; x < memtotal; x++)   FOR memtotal loop



                Debug.WriteLine("END OF LOOP1 " + memtotal + " , " + x);
                if (scanstop == true)
                {
                    scanstop = false;  // reset
                    goto RT2;

                }
            } while (ScanRun == true); // ScanStopped so leave thread

        RT2:
            Debug.WriteLine("SCANTOP0"); // scanner done
            ST2.Stop();
            ST3.Stop();

            btnGroupMemory.BackColor = SystemColors.ControlLight;

            ScanStop = 1;
            ScanRun = false;
            //   scantype = 0;

        } // SCAN1()


        //==========================================================================================
        // thread scans selected SWL Group name frequencies only
        private void SCAN6()
        {
            ST2.Reset();
            ST3.Reset();

            scantype = 1;

            Debug.WriteLine("6SCANSTOP = " + ScanStop);
            btnGroupMemory1.BackColor = Color.LightGreen;

            int lastSIG = 0;
            int lastSQL = 0;

            ST3.Reset();

            int x = 0;

            do // ScanRun
            {
                if (scanstop == true)
                {
                    scanstop = false;  // reset
                    goto RT2A;

                }
                Debug.WriteLine("START OF LOOP");

                for (x = 0; x < memtotal; x++) // go through list of MEMORIES you found
                {
                    Thread.Sleep(50);

                    try
                    {
                        speed = (int)udspeedBox.Value;  // Convert.ToInt16(udspeedBox.Text);
                        Debug.WriteLine("SPEED " + speed);
                    }
                    catch (Exception)
                    {
                        speed = 50; // 50msec
                    }

                    int zz = swl_index[x];

                    //   double hh = (double)SpotControl.SWL_Freq[zz];


                    //   string name = SpotControl.SWL_Loc[zz];    // name of SWL
                    //  string mm = SpotControl.SWL_Station[zz];  // GROUP of SWL

                    //    comboMemGroupName.SelectedIndex = memIndex[x];
                    //   recordToRestore = new MemoryRecord((MemoryRecord)comboMemGroupName.SelectedItem); // ke9ns   you select index in the combo pulldown list
                    //    Debug.WriteLine("CHANGE MEMORY TO " + recordToRestore.RXFreq);
                    //    console.RecallMemory(recordToRestore);

                    console.VFOAFreq = SpotControl.SWL_Freq[zz] / 1000000.0; // convert to mhz
                    console.tempVFOAFreq = console.VFOAFreq; // ke9ns add  CTUN operation changed freq so update temp value

                    currFBox.SelectionStart = x * linelength; // i * linelength
                    currFBox.SelectionLength = linelength;
                    currFBox.ScrollToCaret(); // keep highlighted line visable

                    Debug.WriteLine("SELECT FREQ: " + console.VFOAFreq);

                    ST2.Restart(); // restart timer over again

                    ScanStop = 0; // reset squelch

                    lastSIG = -400;
                    lastSQL = -400;


                    //-------------------------------------------------------
                    // SPEED TIMER and PAUSE
                    do
                    {
                        Thread.Sleep(50);
                        if (scanstop == true)
                        {
                            scanstop = false;  // reset
                            goto RT2A;

                        }
                        if (ScanPause == true) pausebtn.BackColor = Color.Yellow;
                        else pausebtn.BackColor = SystemColors.ControlLight;

                        if (ScanRun == false)
                        {
                            ScanPause = false;
                            goto RT2A;  // turn off this thread now
                        }

                        if (SIG > lastSIG)  // CHECK SQUELCH and SIGNAL levels
                        {
                            lastSIG = SIG;
                        }

                        if (SQL > lastSQL)
                        {
                            lastSQL = SQL;
                        }


                        if ((ScanStop == 1)) // if console detected squelch open
                        {
                            memsignal[x] = lastSQL.ToString().PadLeft(4) + ", " + lastSIG.ToString().PadLeft(4) + ", SQL BRK";

                            if ((chkBoxSQLBRK.Checked == true) && (ScanPause == false)) // if stop on squelch break, then stop now
                            {
                                ScanPause = true;

                                UpdateText(); // update currFBox text

                                currFBox.SelectionStart = x * linelength; // i * linelength
                                currFBox.SelectionLength = linelength;

                                if (udPauseLength.Value > 0)
                                {
                                    Debug.WriteLine("ST3 TIMER STARTED " + memtotal);
                                    ST3.Restart(); // start the pause timer
                                }
                                else
                                {
                                    ScanPause = false;
                                    ScanRun = false;
                                    goto RT2A;
                                }
                                break;
                            }
                            else if ((chkBoxSQLBRKWait.Checked == true) && (ScanPause == false)) // if stop on squelch break, then stop now
                            {
                                ScanPause = true;

                                UpdateText(); // update currFBox text

                                currFBox.SelectionStart = x * linelength; // i * linelength
                                currFBox.SelectionLength = linelength;

                                if (udPauseLength.Value > 0)
                                {
                                    Debug.WriteLine("ST3 TIMER STARTED " + memtotal);
                                    ST3.Restart(); // start the pause timer
                                }
                                else
                                {
                                    ScanPause = false;
                                    ScanRun = false;
                                    goto RT2A;
                                }

                                break;
                            }
                        }
                        else
                        {
                            memsignal[x] = lastSQL.ToString().PadLeft(4) + ", " + lastSIG.ToString().PadLeft(4) + ", ";
                        }

                        if (scanstop == true)
                        {
                            scanstop = false;  // reset
                            goto RT2A;

                        }
                    } while ((ST2.ElapsedMilliseconds < speed) || (ScanPause == true));

                    //-------------------------------------------------------

                    UpdateText1(); // update currFBox text

                    currFBox.SelectionStart = x * linelength; // i * linelength
                    currFBox.SelectionLength = linelength;

                    if (scanstop == true)
                    {
                        scanstop = false;  // reset
                        goto RT2A;

                    }

                    if (SP5_Active == 1)
                    {
                        ScanRun = false;
                        ScanPause = false;
                        Debug.WriteLine("SCANSTOP, another scanner started");
                        break;
                    }


                    //-------------------------------------------------------
                    // CHECK For PAUSE
                    while (ScanPause == true)  // wait here in in pause
                    {
                        Thread.Sleep(50);

                        if (scanstop == true)
                        {
                            scanstop = false;  // reset
                            goto RT2A;

                        }

                        
                        if (ScanRun == false)
                        {
                            Debug.WriteLine("SCANSTOP, Group scanner turned back off");
                            ScanPause = false;
                            scanstop = true;                       // .221
                            break;
                        }

                        if (ST3.ElapsedMilliseconds > ((long)udPauseLength.Value * 1000))
                        {
                            ST3.Stop(); // stop the pause timer
                            if (chkBoxSQLBRKWait.Checked == true && ScanStop == 1)
                            {
                                ScanPause = true;
                                ScanStop = 0;
                                ST3.Restart();

                            }
                            else ScanPause = false;

                            Debug.WriteLine("ST3 TIMER REACHED PAUSELENGTH " + memtotal);
                        }

                        if (ScanPause == true) pausebtn.BackColor = Color.Yellow;
                        else pausebtn.BackColor = SystemColors.ControlLight;

                    }; // while PAUSE

                    if (scanstop == true) //.221
                    {
                        scanstop = false;  // reset
                        goto RT2A;

                    }
                    pausebtn.BackColor = SystemColors.ControlLight;

                    Debug.WriteLine("END OF LOOP " + memtotal + " , " + x);

                } // FOR memtotal loop

                Debug.WriteLine("END OF LOOP1 " + memtotal + " , " + x);
                if (scanstop == true)
                {
                    scanstop = false;  // reset
                    goto RT2A;

                }
            } while (ScanRun == true); // ScanStopped so leave thread

        RT2A:
            Debug.WriteLine("SCANTOP0"); // scanner done
            ST2.Stop();
            ST3.Stop();

            btnGroupMemory1.BackColor = SystemColors.ControlLight;
            pausebtn.BackColor = SystemColors.ControlLight;

            //   scantype = 0;

        } // SCAN6()






        //===========================================================================================
       
        public static byte SP5_Active = 0; // ke9ns: 1= running, 0=off
        double freq1 = 0.0;
        public static double freq_Low = 0.0; // ke9ns low and high get automatically filled by the console.cs routine as the band changes
        public static double freq_High = 0.0;

        public static double freq_Low1 = 0.0; // ke9ns low and high get automatically filled by the console.cs routine as the band changes
        public static double freq_High1 = 0.0;// but these cannot be changed by the user

        public static double freq_Last = 0.0;

        public static bool ViewSWRPlot = false; // true = view SWR plot on display
        public static bool RunSWRPlot = false; // true = run a new plot
                                               //  public static double[,,] SWR_READ = new double[10, 40, 5000]; // Ant=1,2,3, band=1-30,freq slot = swr on band  (now found in console.cs)
        public static int SWR_STEP = 1; // 1k step for SWR
        public static int SWR_SLOT = 0; // slot in band (example 3.5 to 4.0  Slot0 = 3.500 Slot1 = 3.501 (1 SLOT = 1 KHZ)

        // public static double[] SWR_Freq = new double[1000];


        // SWR PLOT BUTTON2
        // read freq_low and Freq_high
        // turn off ATU
        // check for TX auth on freq, if not go to next freq, otherwise TUNE and check SWR, save in array
        private void button2_Click(object sender, EventArgs e)
        {
            ST3.Stop();
            ST3.Reset();

            switch (console.CurrentModel)
            {

                case Model.FLEX5000:
                case Model.FLEX3000:
                    if (console.PowerOn == false)
                    {
                        MessageBox.Show(new Form { TopMost = true }, "Your Flex Radio Must be Running, Turn ON first",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                        return;
                    }
                    if (console.VFOAFreq > 54.0)
                    {
                        MessageBox.Show(new Form { TopMost = true }, "No SWR detection above 6m.",
                               "Error",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Error);
                        return;
                    }
                    break;
                default:
                    MessageBox.Show(new Form { TopMost = true }, "Your Flex Radio does not have an SWR circuit",
                    " ERROR ",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                    return;
                    break;
            }


            Debug.WriteLine("RUN SWR PLOT click    ");

            ScanStop = 0; // reset scan

            if (SP5_Active == 0)
            {

                SP5_Active = 1;

                // see console routine  if (rx1_band != old_band || initializing) for setting low and high settings

                Thread t = new Thread(new ThreadStart(SWR_SCANNER));


                t.Name = "SWR Scanner Thread";
                t.IsBackground = true;
                t.Priority = ThreadPriority.Normal;
                t.Start();

                Debug.WriteLine("good    ");

            } // SP_active = 0;
            else
            {

                SP5_Active = 0;
                Debug.WriteLine("OFF   ");

            } // SP_Active = 1



        } // button2_Click

        //===================================================================================================
        private async void SWR_SCANNER()
        {

            Stopwatch x1 = new Stopwatch();
            // TEST
            int TestRun = 0; // numericSWRTest = 1- 5

            // Band
            Band BAND1 = console.RX1Band; // B160M = 1, B80M = 2,B60M = 3,

            // ANT
            FWCAnt ANT1 = console.GetRX1Ant(BAND1); //ANT1 = 1, ANT2 = 2, ANT3 = 3, RX1IN=4, RX2IN=5, RX1TAP=6, SIG_GEN=7, VHF=8, UHF=9,

            currFBox.Text = "Antenna = " + ANT1 + " , Band= " + BAND1 + "\r\n";



            scantype = 4; // low to high scan

            freq1 = freq_Low;

            //lblTUNE.Text = "Tune: " + ptbTune.Value.ToString();
            var OLD_TUNE_LEVEL = console.TunePower;
            console.TunePower = 11;
            // console.ptbTune.Value = 10; // set power level to 10watts

            try
            {
                TestRun = (int)numericSWRTest.Value;

            }
            catch (Exception)
            {
                TestRun = 1;
            }

            if (TestRun > 5)
            {
                TestRun = 1;
                numericSWRTest.Value = 1;
            }

            try
            {
                step = (int)udstepBox.Value;   // get just whole number. Convert.ToDouble(udstepBox.Text) / 1000;
                SWR_STEP = (int)udstepBox.Value;

                step = step / 1000; // convert to KHZ  (1khz : step= .001)
            }
            catch (Exception)
            {
                step = 0.002; // 2 khz
                SWR_STEP = 2;
            }

            try
            {
                speed = (int)udspeedBox1.Value;  // Convert.ToInt16(udspeedBox.Text);

            }
            catch (Exception)
            {
                speed = 300; // 300msec

            }

            Debug.WriteLine("SWR_Scanner STEP " + step + " ,SPEED " + speed + " ,LOW " + freq_Low + " ,HIGH " + freq_High + " ,RST " + ScanRST + " ,LAST " + freq_Last + ", Test: " + TestRun);


            double ii = freq_Low;


            if (ScanRST == 1)
            {
                ii = freq_Last;
            }


            console.SWR_READ[TestRun, (int)ANT1, (int)BAND1, 0] = Math.Round(freq_Low1, 3); // first entry is LOW freq of this SWR PLOT (SLOT size is always 1khz)
            console.SWR_READ[TestRun, (int)ANT1, (int)BAND1, 1] = Math.Round(freq_High1, 3); // 2nd entry is HIGH freq of this SWR PLOT (SLOT size is always 1khz)
            console.SWR_READ[TestRun, (int)ANT1, (int)BAND1, 2] = Math.Round(step, 3); // .001 = 1khz first entry is LOW freq of this SWR PLOT (SLOT size is always 1khz)

            console.SWR_READ[TestRun, (int)ANT1, (int)BAND1, 3] = TestRun;  //TEST#
            console.SWR_READ[TestRun, (int)ANT1, (int)BAND1, 4] = 4; // reserved
            console.SWR_READ[TestRun, (int)ANT1, (int)BAND1, 5] = 5; // reserved
            console.SWR_READ[TestRun, (int)ANT1, (int)BAND1, 6] = 6; // reserved
            console.SWR_READ[TestRun, (int)ANT1, (int)BAND1, 7] = 7; // reserved
            console.SWR_READ[TestRun, (int)ANT1, (int)BAND1, 8] = 8; // reserved
            console.SWR_READ[TestRun, (int)ANT1, (int)BAND1, 9] = 9; // reserved

            SWR_SLOT = (int)(((decimal)ii - (decimal)freq_Low1) * 1000) + 10; // find first SLOT in current LOW freq (example 3.5 - 4.0, but LOW = 3.6, so SLOT = 100 for 3.6 (1khz = 1 slot)

            //   Debug.WriteLine("SWR_SLOT SCAN " + SWR_SLOT);

            ScanStop = 0;

            for (int x = 10; x < SWR_SLOT; x++) // fill low end of SWR plot
            {
                if (console.SWR_READ[TestRun, (int)ANT1, (int)BAND1, x] < 1) console.SWR_READ[TestRun, (int)ANT1, (int)BAND1, x] = 0.5; // this is so the storage file will see the data gap and not ignore data after the gap
                                                                                                                                      //  Debug.WriteLine("SWR FILL " + x);

            }

            System.IO.Directory.CreateDirectory(console.AppDataPath + "SWR_PLOTS"); // ke9ns create sub directory

            string file_name = console.AppDataPath + "SWR_PLOTS\\SWR_TEST#" + TestRun.ToString() + "-" + ANT1.ToString() + "-" + BAND1.ToString() + "_" + SWRName() + ".csv"; // save data for my mods .166

            FileStream stream4 = new FileStream(file_name, FileMode.Create); // open   file .166
            BinaryWriter writer4 = new BinaryWriter(stream4);

            byte[] textb = Encoding.ASCII.GetBytes("Test Run=," + TestRun.ToString() + Environment.NewLine);
            writer4.Write(textb);

            textb = Encoding.ASCII.GetBytes("Antenna Source=," + ANT1.ToString() + Environment.NewLine);
            writer4.Write(textb);

            textb = Encoding.ASCII.GetBytes("Band=," + BAND1.ToString() + Environment.NewLine);
            writer4.Write(textb);

            textb = Encoding.ASCII.GetBytes(Environment.NewLine); // .166
            writer4.Write(textb);

            textb = Encoding.ASCII.GetBytes("SLOT," + "Frequency (Mhz)," + "SWR" + Environment.NewLine); // .166
            writer4.Write(textb);


            for (; ii <= freq_High; ii = ii + step, SWR_SLOT = SWR_SLOT + (int)(udstepBox.Value))
            {

                // find if you are authorized to TX on this freq
                if (console.CheckValidTXFreq(console.current_region, ii, console.dsp.GetDSPTX(0).CurrentDSPMode) == false) continue;

                console.chkTUN.Checked = true; // TX 

                if (SWR_SLOT == 10)
                {

                    for (int x9 = 0; x9 < 4; x9++)
                    {
                        if (ScanStop == 1 && chkBoxSQLBRK.Checked == true) break;
                        await Task.Delay(speed / 10).ConfigureAwait(false);

                    }

                }

                x1.Restart(); // restart timer


                console.VFOAFreq = ii; // convert to MHZ

                for (int x9 = 0; x9 < 10; x9++)
                {
                    if (ScanStop == 1 && chkBoxSQLBRK.Checked == true) break; // allow you to check if user stopped scanner
                    await Task.Delay(speed / 10).ConfigureAwait(false);
                }

                x1.Stop();
                double SWR1 = console.FWCSWR(console.pa_fwd_power, console.pa_rev_power);
                Debug.WriteLine("Freq & SWR " + ii + " , " + SWR1);

                if (SWR1 >= 25) // try 1 more time before displaying SWR
                {
                    for (int q = 0; q < 5; q++) // ke9ns .184
                    {
                        for (int x9 = 0; x9 < 3; x9++)
                        {
                            if (ScanStop == 1 && chkBoxSQLBRK.Checked == true) break;
                            await Task.Delay(speed / 10).ConfigureAwait(false);

                        }
                        SWR1 = console.FWCSWR(console.pa_fwd_power, console.pa_rev_power);
                        if (SWR1 < 25) break;
                    } // for loop

                } // if SWR1 >=25


                double temp9 = (Math.Round(SWR1, 1));

                Debug.WriteLine("SLOT= " + SWR_SLOT + " , " + ii.ToString("###0.000") + " , " + temp9.ToString() + " , " + (int)ANT1 + " , " + (int)BAND1);


                console.SWR_READ[TestRun, (int)ANT1, (int)BAND1, SWR_SLOT] = temp9;

                for (int i = 1; i < SWR_STEP; i++) // fill in empty slots with identical data to the last valid slot SWR
                {
                    console.SWR_READ[TestRun, (int)ANT1, (int)BAND1, SWR_SLOT + i] = temp9;

                } // if (SWR_STEP > 1)

                currFBox.AppendText(SWR_SLOT + " , " + ii.ToString("###0.000000") + " , " + temp9 + "\r\n");
                textb = Encoding.ASCII.GetBytes(SWR_SLOT.ToString() + " , " + ii.ToString("###0.000000") + " , " + temp9 + Environment.NewLine); // .166
                writer4.Write(textb);

                if (SP5_Active == 0) break;

            } // for loop

            console.chkTUN.Checked = false; // TX OFF 
            console.TunePower = OLD_TUNE_LEVEL;

            currFBox.AppendText("ABOVE ^ # " + TestRun + ", Antenna = " + ANT1 + " , Band= " + BAND1 + "\r\n");
            textb = Encoding.ASCII.GetBytes("ABOVE ^ # " + TestRun.ToString() + ", Antenna =," + ANT1.ToString() + " , Band= ," + BAND1.ToString() + Environment.NewLine); // .166
            writer4.Write(textb);
            Debug.WriteLine("SWR_Scanner STOPPED");

            if (ii >= freq_High)
            {
                ScanRST = 0; // reset back to start
                ii = freq_Low;

                Debug.WriteLine("SWR_SCANNER FINISHED ");

            }
            else
            {
                ScanRST = 1; // leave off where you left off
                freq_Last = ii + (step * 2); // need to jump past last signal that breaks squelch otherwise you cant move anymore
            }

            writer4.Close();    // close  file
            stream4.Close();   // close stream


        } // SWR_SCANNER (END Thread)


        public string SWRName()
        {
            string temp = DateTime.Now.ToString();                     // Date and time
            temp = temp.Replace("/", "-");
            temp = temp.Replace(":", "_");

            return temp;


        } // SWRName()


        // FREQ SCANNER BUTTON
        private void button5_Click(object sender, EventArgs e)
        {
            ST3.Stop();
            ST3.Reset(); // shut down memory scan


            Debug.WriteLine("click    ");

            ScanStop = 0; // reset scan

            if (SP5_Active == 0)
            {

                SP5_Active = 1;

                // see console routine  if (rx1_band != old_band || initializing) for setting low and high settings

                Thread t = new Thread(new ThreadStart(SCANNER));


                t.Name = "Scanner Thread";
                t.IsBackground = true;
                t.Priority = ThreadPriority.Normal;
                t.Start();

                Trace.WriteLine("good    ");

            } // SP_active = 0;
            else
            {

                SP5_Active = 0;
                Trace.WriteLine("OFF   ");

            } // SP_Active = 1




        } // button5_Click



        //===============================================================================
        //===============================================================================
        // ke9ns SCANNER thread from low to high frequency selected on the scanner panel

        double step = 0.0001;
        int speed = 50;


        private async void SCANNER()
        {

            Stopwatch x1 = new Stopwatch();

        LOOP1: scantype = 4;

            freq1 = freq_Low;

            try
            {
                step = (double)udstepBox.Value;   //Convert.ToDouble(udstepBox.Text) / 1000;
                step = step / 1000; // convert to KHZ
            }
            catch (Exception)
            {
                step = 0.0001; // 1 khz
            }

            try
            {
                speed = (int)udspeedBox1.Value;  // Convert.ToInt16(udspeedBox.Text);

            }
            catch (Exception)
            {
                speed = 50; // 50msec
            }

            Debug.WriteLine("Scanner STEP " + step + " ,SPEED " + speed + " ,LOW " + freq_Low + " ,HIGH " + freq_High + " ,RST " + ScanRST + " ,LAST " + freq_Last);



            //   Trace.WriteLine("good1   ");

            double ii = freq_Low;

            if (ScanRST == 1)
            {
                ii = freq_Last;
            }


            for (; ii <= freq_High; ii = ii + step)
            {

                currFBox.Text = ii.ToString("f6");

                x1.Restart();

                console.VFOAFreq = ii; // convert to MHZ

                for (int x9 = 0; x9 < 10; x9++) // divide up step time into 10 parts, so I can use await timer
                {
                    if (chkBoxSQLBRK.Checked == true)
                    {
                        if (ScanStop == 1) // if console says squelch break
                        {
                            if (udPauseLength.Value == 0)
                            {
                                break; // break out of the delay loop
                            }
                            else
                            {
                                if (SP5_Active == 0) break;
                                Debug.WriteLine("PAUSE " + x9);
                                pausebtn.BackColor = Color.Yellow;
                                await Task.Delay((int)udPauseLength.Value * 1000).ConfigureAwait(false); // pause
                                ScanStop = 0;
                                pausebtn.BackColor = SystemColors.ControlLight;
                                break;
                            }
                        }
                    }
                    else if (chkBoxSQLBRKWait.Checked == true)
                    {
                        if (ScanStop == 1) // if console says squelch break
                        {
                            if (udPauseLength.Value == 0)
                            {
                                break; // break out of the delay loop
                            }
                            else
                            {
                                if (SP5_Active == 0) break;
                                Debug.WriteLine("PAUSE " + x9);
                                pausebtn.BackColor = Color.Yellow;
                                await Task.Delay((int)udPauseLength.Value * 1000).ConfigureAwait(false); // pause
                                ScanStop = 0;
                                pausebtn.BackColor = SystemColors.ControlLight;

                            }
                        }
                    }
                    await Task.Delay(speed / 10).ConfigureAwait(false);

                } // 10x loop

                if (chkBoxSQLBRK.Checked == true)
                {
                    if (ScanStop == 1) // if console says squelch break
                    {
                        if (udPauseLength.Value == 0)
                        {
                            ScanStop = 0;
                            break; // break out of the freq scanner loop
                        }
                    }
                }
                else if (chkBoxSQLBRKWait.Checked == true)
                {
                    if (ScanStop == 1) // if console says squelch break
                    {
                        if (udPauseLength.Value == 0)
                        {
                            ScanStop = 0;
                            break; // break out of the freq scanner loop
                        }
                    }
                }


                x1.Stop();

                Debug.WriteLine("TIME " + x1.ElapsedMilliseconds);

                if (SP5_Active == 0) break;

            } // for loop


            Debug.WriteLine("Scanner1 ");

            if (ii >= freq_High)
            {
                ScanRST = 0; // reset back to start
                ii = freq_Low;

                Debug.WriteLine("SCANNER FINISHED ");

                if (chkBoxLoop.Checked == true)
                {
                    SP5_Active = 1;
                    goto LOOP1;
                }


            }
            else
            {
                ScanRST = 1; // leave off where you left off
                freq_Last = ii + (step * 2); // need to jump past last signal that breaks squelch otherwise you cant move anymore
            }


            //   scantype = 0;

        } // SCANNER


        //===============================================================================
        //===============================================================================


        // ke9ns override band edge setting 
        private void lowFBox_Click(object sender, EventArgs e)
        {
            double freq2 = 0.0;
            ScanRST = 0;
            try
            {
                freq2 = Convert.ToDouble(lowFBox.Text);

                if (freq2 < freq_Low1) freq2 = freq_Low1;

            }
            catch (Exception)
            {
                freq2 = freq_Low1;


            }

            freq_Low = freq2;
            lowFBox.Text = freq_Low.ToString("f6");

            if (console.RX1Band >= 0) console.SLowScan[(int)console.RX1Band] = lowFBox.Text; // ke9ns .186: save low/high of scanner for each band

        } // lowFBox_Click

        private void lowFBox_MouseLeave(object sender, EventArgs e)
        {
            double freq2 = 0.0;
            ScanRST = 0;
            try
            {
                freq2 = Convert.ToDouble(lowFBox.Text);

                if (freq2 < freq_Low1) freq2 = freq_Low1;

            }
            catch (Exception)
            {
                freq2 = freq_Low1;


            }


            freq_Low = freq2;
            lowFBox.Text = freq_Low.ToString("f6");

            if (console.RX1Band >= 0) console.SLowScan[(int)console.RX1Band] = lowFBox.Text; // ke9ns .186: save low/high of scanner for each band

        } // lowFBox_MouseLeave

        private void highFBox_Click(object sender, EventArgs e)
        {
            double freq3 = 0.0;
            ScanRST = 0;
            try
            {
                freq3 = Convert.ToDouble(highFBox.Text);
                if (freq3 > freq_High1) freq3 = freq_High1;
            }
            catch (Exception)
            {
                freq3 = freq_High1;


            }

            freq_High = freq3;
            highFBox.Text = freq_High.ToString("f6");

            if (console.RX1Band >= 0) console.SHighScan[(int)console.RX1Band] = highFBox.Text; // ke9ns .186: save low/high of scanner for each band


        } // highFBox_Click

        private void highFBox_MouseLeave(object sender, EventArgs e)
        {
            double freq3 = 0.0;
            ScanRST = 0;
            try
            {
                freq3 = Convert.ToDouble(highFBox.Text);
                if (freq3 > freq_High1) freq3 = freq_High1;
            }
            catch (Exception)
            {
                freq3 = freq_High1;


            }

            freq_High = freq3;
            highFBox.Text = freq_High.ToString("f6");

            if (console.RX1Band >= 0) console.SHighScan[(int)console.RX1Band] = highFBox.Text; // ke9ns .186: save low/high of scanner for each band

        } // highFBox_MouseLeave

        private void lowFBox_KeyDown(object sender, KeyEventArgs e)
        {
            double freq2 = 0.0;

            if (e.KeyData == Keys.Enter)
            {
                ScanRST = 0;
                try
                {
                    freq2 = Convert.ToDouble(lowFBox.Text);

                    if (freq2 < freq_Low1) freq2 = freq_Low1;

                }
                catch (Exception)
                {
                    freq2 = freq_Low1;


                }

                freq_Low = freq2;
                lowFBox.Text = freq_Low.ToString("f6");

                if (console.RX1Band >= 0) console.SLowScan[(int)console.RX1Band] = lowFBox.Text; // ke9ns .186: save low/high of scanner for each band

            } // wait for enter key

        } // lowFBox_KeyDown

        private void highFBox_KeyDown(object sender, KeyEventArgs e)
        {
            double freq3 = 0.0;

            if (e.KeyData == Keys.Enter)
            {
                ScanRST = 0;
                try
                {
                    freq3 = Convert.ToDouble(highFBox.Text);
                    if (freq3 > freq_High1) freq3 = freq_High1;
                }
                catch (Exception)
                {
                    freq3 = freq_High1;


                }

                freq_High = freq3;
                highFBox.Text = freq_High.ToString("f6");

                if (console.RX1Band >= 0) console.SHighScan[(int)console.RX1Band] = highFBox.Text; // ke9ns .186: save low/high of scanner for each band

            } // wait for enter key

        }// highFBox_KeyDown

        private void chkAlwaysOnTop_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = chkAlwaysOnTop.Checked;
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }



        string customscannerlist = ""; // ke9ns file name of customscannerlist


        //=========================================================================================
        // ke9ns add   select scanner file name (text file freq in hz, name)
        string[] customString = new string[200]; // name
        string[] customFilter = new string[200]; // filter
        string[] customMode = new string[200]; // mode
        double[] customMem = new double[200]; // list of custom memory frequency

        public static FileStream stream2;          // for reading SWL.csv file
        public static BinaryReader reader2;

        public static int custSize = 0; // size on custom freq list

        private void btnCustomList_Click(object sender, EventArgs e)
        {

            ScanPause = false;
           
            comboBoxTS1.Text = "";
            textBox1.Text = "";
            Gname = "";
            memcount = 0;
            memtotal = 0;  //.226
           

            ST3.Stop();
            ST3.Reset();
           

            if (ScanRun == false) // if stopped
            {
                string filePath = console.AppDataPath + "CustomScannerList\\";


                if (!Directory.Exists(filePath))
                {
                    //   Debug.WriteLine("no CustomScannerList folder file found");
                    System.IO.Directory.CreateDirectory(console.AppDataPath + "CustomScannerList"); // ke9ns create sub directory
                                                                                                    //  Debug.WriteLine("CustomScannerList created");

                }

                openFileDialog2.InitialDirectory = String.Empty;
                openFileDialog2.InitialDirectory = filePath; // ke9ns  file to quickplay subfolder but could also be wave_folder;


                DialogResult result1 = openFileDialog2.ShowDialog();

                if (result1 == DialogResult.OK) // Test result.
                {
                    //    Debug.WriteLine("file selected1 " + result);
                    //  Debug.WriteLine("file selected2 " + openFileDialog2.FileName);

                    customscannerlist = openFileDialog2.FileName; // pass file name to wave file
                }
                else
                {
                    customscannerlist = null;
                    return; // if you dont select a file then no scanning
                }

                stream2 = new FileStream(customscannerlist, FileMode.Open); // open  file
                reader2 = new BinaryReader(stream2, Encoding.ASCII);

                var result = new StringBuilder();

                Debug.WriteLine("OPEN THE FILE ");

                custSize = 0; // new size of custom freq list

                int x = 0;
                for (; ; )
                {

                    try
                    {
                        var newChar = (char)reader2.ReadChar();

                        if ((newChar == '\r'))  // 0x0d LF
                        {

                            newChar = (char)reader2.ReadChar(); // read \n char to finishline

                            string[] values = result.ToString().Split(','); // split line up into segments divided by ,

                            Debug.WriteLine("CUSTOM STRING " + values[0]);
                            Debug.WriteLine("CUSTOM MEM " + values[1]);

                            Debug.WriteLine("CUSTOM MEM " + values[2]);

                            Debug.WriteLine("CUSTOM MEM " + values[3]);

                            customString[x] = values[0];                 // name
                            customMem[x] = Convert.ToDouble(values[1]);  // freq
                            customMode[x] = values[2];                   // mode = LSB,  USB,DSB,CWL,CWU,FM,	AM,	DIGU,SPEC,	DIGL,	SAM, DRM
                            customFilter[x] = values[3];                 // filter =  F1,F2,F3,	F4,	F5,	F6,	F7,	F8,	F9,	F10,VAR1,VAR2



                            result.Clear();


                            x++; // get next line
                        }
                        else
                        {

                            result.Append(newChar);  // save char
                        }

                    }
                    catch (EndOfStreamException)
                    {
                        // x--;
                        Debug.WriteLine("END OF STREAM ");
                        break; // done with file
                    }
                    catch (Exception f)
                    {
                        Debug.WriteLine("GET CHAR EXCEPTION " + f);
                        // x--;
                        break;
                    }

                    if (x > 100) break; // only allow 100 freq in list


                } // for loop 
              
                reader2.Close();    // close  file
                stream2.Close();   // close stream
               
                custSize = x; // new size of custom freq list

                ScanRun = true; // start up the scanner

                currFBox.Text = "";
                for (int y = 0; y < 100; y++)
                {
                    memsignal[y] = null;
                }

                UpdateText3();

                Thread t = new Thread(new ThreadStart(SCAN3));
                t.Name = "Custom memory Scanner Thread";
                t.IsBackground = true;
                t.Priority = ThreadPriority.Normal;
                t.Start();

            }
            else
            {
                ScanRun = false; // turn off scanner

            }


        } // btnCustomList_Click

        //========================================================================================
        //  CUSTOM MEMORY LIST CURRFBOX text update
        void UpdateText3()
        {

            string temp1 = "";

           
            for (int ii = 0; ii < custSize; ii++)
            {
               
                string freq3 = customMem[ii].ToString("###0.000000"); // was N6 4 less than having index numbers
                string name = customString[ii];
                string mm = "Custom Memory List";

                if (memsignal[ii] == "") memsignal[ii] = " ";
                if (memsignal[ii] == null) memsignal[ii] = " ";

                temp1 = temp1 + (ii + 1).ToString().PadLeft(2) + ": " + mm.PadRight(20).Substring(0, 20) + ", " + freq3.PadLeft(12).Substring(0, 12) + ", " + name.PadRight(20).Substring(0, 20) + ", " + memsignal[ii].PadRight(20).Substring(0, 20) + "\r\n"; // 74 char long //.226 fix ii
              

            } // for


            currFBox.Text = temp1;

        } // UpdateText3()  CUSTOM MEMORY LIST


        //===========================================================================
        // Thread CUSTOM LIST MEMORY scanner
        private void SCAN3()
        {
            scantype = 3;

            ST2.Reset();
            ST3.Reset();

            Debug.WriteLine("SCANSTOP = " + ScanStop);
            btnCustomList.BackColor = Color.LightGreen;

            int lastSIG = 0;
            int lastSQL = 0;

            string filter, mode;
            double freq;


            do // ScanRun
            {

                for (int x = 0; x < custSize; x++)
                {

                    Thread.Sleep(50);
                    if (scanstop == true)
                    {
                        scanstop = false;  // reset
                        goto RT2;

                    }
                    try
                    {
                        speed = (int)udspeedBox.Value;  // Convert.ToInt16(udspeedBox.Text);
                        Debug.WriteLine("SPEED " + speed);
                    }
                    catch (Exception)
                    {
                        speed = 50; // 50msec
                    }


                    // go to next bandstack memory

                    freq = customMem[x];
                    filter = customFilter[x];   // "LAST";
                    mode = customMode[x];        // "LAST";

                    Debug.WriteLine("CUSTOM BAND: " + freq + " , " + filter + " , " + mode);

                    // filter = F1,F2,F3,F4,F5,F6,F7,F8,F9,F10,VAR1,VAR2
                    // mode = LSB,USB,DSB,CWL,CWU,FM,AM,DIGU,SPEC,DIGL,SAM,DRM


                    console.SetBand(mode, filter, freq);
                    console.UpdateWaterfallLevelValues();

                    currFBox.SelectionStart = x * linelength; // i * linelength
                    currFBox.SelectionLength = linelength;
                    currFBox.ScrollToCaret(); // keep highlighted line visable

                    ST2.Restart(); // restart timer over again

                    ScanStop = 0; // reset squelch

                    lastSIG = -400;
                    lastSQL = -400;
                    //-------------------------------------------------------
                    // timer
                    do // scan speed and scanPause
                    {
                        Thread.Sleep(50);
                        if (scanstop == true)
                        {
                            scanstop = false;  // reset
                            goto RT2;

                        }
                        if (ScanPause == true) pausebtn.BackColor = Color.Yellow;
                        else pausebtn.BackColor = SystemColors.ControlLight;

                        if (ScanRun == false)
                        {
                            ScanPause = false;
                            goto RT2;
                        }

                        if (SIG > lastSIG)
                        {
                            lastSIG = SIG;
                        }

                        if (SQL > lastSQL)
                        {
                            lastSQL = SQL;
                        }

                        if ((ScanStop == 1)) // if console detected squelch open
                        {
                            memsignal[x] = lastSQL.ToString().PadLeft(4) + ", " + lastSIG.ToString().PadLeft(4) + ", Squelch Break";

                            if ((chkBoxSQLBRK.Checked == true) && (ScanPause == false)) // if stop on squelch break, then stop now
                            {
                                ScanPause = true;
                                UpdateText3(); // update currFBox text

                                currFBox.SelectionStart = band_index * linelength; // i * linelength
                                currFBox.SelectionLength = linelength;

                                if (udPauseLength.Value > 0)
                                {
                                    Debug.WriteLine("ST3 TIMER STARTED ");
                                    ST3.Restart(); // start the pause timer
                                }
                                else
                                {
                                    ScanPause = false;
                                    ScanRun = false;
                                    goto RT2;
                                }
                                break;
                            }
                            else if ((chkBoxSQLBRKWait.Checked == true) && (ScanPause == false)) // if stop on squelch break, then stop now
                            {
                                ScanPause = true;
                                UpdateText3(); // update currFBox text

                                currFBox.SelectionStart = band_index * linelength; // i * linelength
                                currFBox.SelectionLength = linelength;

                                if (udPauseLength.Value > 0)
                                {
                                    Debug.WriteLine("ST3 TIMER STARTED ");
                                    ST3.Restart(); // start the pause timer
                                }
                                else
                                {
                                    ScanPause = false;
                                    ScanRun = false;
                                    goto RT2;
                                }
                                break;
                            }

                        }
                        else
                        {
                            memsignal[x] = lastSQL.ToString().PadLeft(4) + ", " + lastSIG.ToString().PadLeft(4) + ", ";
                        }
                        if (scanstop == true)
                        {
                            scanstop = false;  // reset
                            goto RT2;

                        }
                    } while ((ST2.ElapsedMilliseconds < speed) || (ScanPause == true));

                    //-------------------------------------------------------

                    UpdateText3(); // update currFBox text

                    currFBox.SelectionStart = x * linelength; // i * linelength
                    currFBox.SelectionLength = linelength;

                    if (SP5_Active == 1)
                    {
                        ScanRun = false;
                        ScanPause = false;
                        Debug.WriteLine("SCANSTOP, another scanner started");
                        break;
                    }


                    //-------------------------------------------------------
                    // CHECK For PAUSE
                    while (ScanPause == true)  // wait here in in pause
                    {

                        Thread.Sleep(50);

                        if (ScanRun == false)
                        {
                            Debug.WriteLine("SCANSTOP, Group scanner turned back off");
                            ScanPause = false;
                            break;
                        }

                        if (ST3.ElapsedMilliseconds > ((long)udPauseLength.Value * 1000))
                        {
                            ST3.Stop(); // stop the pause timer
                            if (chkBoxSQLBRKWait.Checked == true && ScanStop == 1)
                            {
                                ScanPause = true;
                                ScanStop = 0;
                                ST3.Restart();

                            }
                            else ScanPause = false;

                            Debug.WriteLine("ST3 TIMER REACHED PAUSELENGTH ");
                        }

                        if (ScanPause == true) pausebtn.BackColor = Color.Yellow;
                        else pausebtn.BackColor = SystemColors.ControlLight;

                    };

                    pausebtn.BackColor = SystemColors.ControlLight;

                } // FOR custSize loop

            } while (ScanRun == true); // ScanStopped so leave thread

        RT2: ST2.Stop();
            ST3.Stop();

            Debug.WriteLine("SCANTOP0"); // scanner done
            btnCustomList.BackColor = SystemColors.ControlLight;
            pausebtn.BackColor = SystemColors.ControlLight;
            //   scantype = 0;


        } // SCAN3  CUSTOMER LIST MEMORY SCANNER



        //==========================================================================================
        // Pause button
        private void pausebtn_Click(object sender, EventArgs e)
        {
            ST3.Stop();
            ST3.Reset();

            if (ScanRun == true)
            {
                if (ScanPause == false)
                {
                    ScanPause = true;

                }
                else
                {

                    ScanPause = false;
                    ScanStop = 0; // undo the squelch break if you unpause
                }
            }
        }

        int yyy = 0;
        int iii = 0;

        //==================================================================================
        // ke9ns left click to select this memory for vfoA
        private void currFBox_MouseUp(object sender, MouseEventArgs e)
        {
            string filter, mode;
            double freq;

            currFBox.ShortcutsEnabled = false;

            if (e.Button == MouseButtons.Left) // VFOA
            {
                Debug.WriteLine("LEFT CLICK: " );

                scanstop = true;

                if (scantype == 1)  // 1=memory, 2=band stack, 3= custom, 4= low-high, 5=SWL
                {
                    try
                    {
                        int ii = currFBox.GetCharIndexFromPosition(e.Location);

                        xxx = (ii / linelength); //find row 

                        // if (xxx >= console.band_stacks[nnn]) return; // if you click past the last index freq, then do nothing.

                        Debug.WriteLine("1xxx " + xxx + " , " + ii);


                        currFBox.SelectionStart = (xxx * linelength);
                        currFBox.SelectionLength = linelength;

                        currFBox.ScrollToCaret(); // keep highlighted line visable

                        Debug.WriteLine("index at start of click " + iii);


                        iii = xxx; // update new position in bandstack for checking if its locked

                        Debug.WriteLine("memcount " + memcount + " , " + memtotal);

                        yyy = 0;

                        if (textBox1.Text == "")  // MEMORY
                        {
                            if (iii > memIndex[memtotal])
                            {
                                Debug.WriteLine("clicked beyond index length " + memIndex[memtotal]);
                                return;
                            }
                        }
                        else // SWL
                        {
                            if (iii > memtotal)
                            {
                                Debug.WriteLine("1clicked beyond index length " + memtotal);
                                return;
                            }

                        }

                        if (comboBoxTS1.Text != "") //.221 add so clicking on memory in the scann screen will pull up all memory parameters
                        {
                            Debug.WriteLine("MEMORY CLICK. restore memory " + xxx);

                           comboMemGroupName.SelectedIndex = memIndex[xxx];
                            recordToRestore = new MemoryRecord((MemoryRecord)comboMemGroupName.SelectedItem); // ke9ns   you select index in the combo pulldown list
                          
                            Debug.WriteLine("CHANGE MEMORY TO " + recordToRestore.RXFreq);
                            console.RecallMemory(recordToRestore);

                            return;
                        }

                        int counter = 0;

                        for (int i = 0; i < memcount; i++) // find all the memories with the same group name
                        {

                            if (textBox1.Text == "")  // MEMORY
                            {
                              
                                if (CultureInfo.InvariantCulture.CompareInfo.IndexOf((dataGridView2[0, i].Value.ToString()), Gname, CompareOptions.IgnoreCase) >= 0) // Gname must be contains in MEMORY (partial or full) and case insensitive)
                                {
                                    freq = Convert.ToDouble(dataGridView2[1, i].Value);  // MEMORY "RXFREQ"  convert to hz

                                    mode = dataGridView2[3, i].Value.ToString();  // DSPMODE of MEMORY

                                    filter = dataGridView2[20, i].Value.ToString();

                                    // you got a match to your GROUP name so check the line #
                                    if (counter == iii)
                                    {
                                        console.SetBand(mode, filter, freq);
                                        return;
                                    }
                                    counter++;
                                }
                            }
                            else // SWL 
                            {
                                int zz = swl_index[iii];

                                double hh = (double)SpotControl.SWL_Freq[zz] / 1000000.0;

                                console.VFOAFreq = hh;
                                console.tempVFOAFreq = console.VFOAFreq; // ke9ns add  CTUN operation changed freq so update temp value

                                return;
                            }

                        } // for i loop

                    }
                    catch (Exception)
                    {


                    }

                    Debug.WriteLine(" did not find a match ");
                    return;

                } // scantype = 1
                else if (scantype == 2)  // 1=memory, 2=band stack, 3= custom, 4= low-high, 5=SWL
                {
                    try
                    {
                        int ii = currFBox.GetCharIndexFromPosition(e.Location);

                        xxx = (ii / linelength); //find row 

                        if (xxx >= console.band_stacks[nnn]) return; // if you click past the last index freq, then do nothing.

                        Debug.WriteLine("1xxx " + xxx + " , " + ii);


                        currFBox.SelectionStart = (xxx * linelength);
                        currFBox.SelectionLength = linelength;

                        Debug.WriteLine("index at start of click " + console.iii);


                        console.iii = xxx; // update new position in bandstack for checking if its locked

                        Debug.WriteLine("index after click " + console.iii);

                        yyy = 0;


                        if (xxx > console.band_stacks[nnn])
                        {
                            return;

                        }


                        DB.GetBandStack(band_list[nnn], xxx, out mode, out filter, out freq);

                        // updateindex();

                        console.SetBand(mode, filter, freq);

                        console.UpdateWaterfallLevelValues();
                    }
                    catch
                    {
                        Debug.WriteLine("Failed to determine index or cannot save bandstack because its locked");

                        if (yyy == 1)
                        {
                            updateindex();

                            //   console.SetBand(mode1[xxx], filter1[xxx], freq1[xxx]);

                            console.UpdateWaterfallLevelValues();
                        }

                    }


                } // scantype = 2 (bandstack)



            } // LEFT CLICK MOUSE VFOA
            else if (e.Button == MouseButtons.Middle) // .222 VFOB RX2
            {
                Debug.WriteLine("RIGHT CLICK: ");
               

                scanstop = true;

                if (scantype == 1)
                {
                    try
                    {
                        int ii = currFBox.GetCharIndexFromPosition(e.Location);

                        xxx = (ii / linelength); //find row 

                        // if (xxx >= console.band_stacks[nnn]) return; // if you click past the last index freq, then do nothing.

                        Debug.WriteLine("1xxx " + xxx + " , " + ii);


                        currFBox.SelectionStart = (xxx * linelength);
                        currFBox.SelectionLength = linelength;

                        currFBox.ScrollToCaret(); // keep highlighted line visable

                        Debug.WriteLine("index at start of click " + iii);


                        iii = xxx; // update new position in bandstack for checking if its locked

                        Debug.WriteLine("memcount " + memcount + " , " + memtotal);

                        yyy = 0;

                        if (textBox1.Text == "")  // MEMORY
                        {
                            if (iii > memIndex[memtotal])
                            {
                                Debug.WriteLine("clicked beyond index length " + memIndex[memtotal]);
                                return;
                            }

                        }
                        else // SWL
                        {
                            if (iii > memtotal)
                            {
                                Debug.WriteLine("1clicked beyond index length " + memtotal);
                                return;
                            }

                        }

                        if (comboBoxTS1.Text != "") //.221 add so clicking on memory in the scan screen will pull up all memory parameters
                        {
                            Debug.WriteLine("MEMORY CLICK. restore memory " + xxx);

                            comboMemGroupName.SelectedIndex = memIndex[xxx];
                            recordToRestore = new MemoryRecord((MemoryRecord)comboMemGroupName.SelectedItem); // ke9ns   you select index in the combo pulldown list

                            Debug.WriteLine("CHANGE MEMORY TO " + recordToRestore.RXFreq);
                            console.RecallMemoryB(recordToRestore); // vfob


                            if (console.setupForm != null && console.setupForm.chkRX2AutoOn.Checked == true && console.chkRX2.Checked == false && console.chkRX2.Visible) // .229
                            {
                                console.chkRX2.Checked = true;
                                if (console.setupForm.chkRX2AutoVAC2.Checked)
                                {
                                    console.setupForm.chkVAC2Enable.Checked = true; // .230
                                }
                            }


                            return;
                        }

                        int counter = 0;

                        for (int i = 0; i < memcount; i++) // find all the memories with the same group name
                        {

                            if (textBox1.Text == "")  // MEMORY
                            {

                                if (CultureInfo.InvariantCulture.CompareInfo.IndexOf((dataGridView2[0, i].Value.ToString()), Gname, CompareOptions.IgnoreCase) >= 0) // Gname must be contains in MEMORY (partial or full) and case insensitive)
                                {

                                    freq = Convert.ToDouble(dataGridView2[1, i].Value);  // MEMORY "RXFREQ"  convert to hz

                                    mode = dataGridView2[3, i].Value.ToString();  // DSPMODE of MEMORY

                                    filter = dataGridView2[20, i].Value.ToString();

                                    // you got a match to your GROUP name so check the line #
                                    if (counter == iii)
                                    {
                                        console.SetBand2(mode, filter, freq); // .222 rx2


                                        if (console.setupForm != null && console.setupForm.chkRX2AutoOn.Checked == true && console.chkRX2.Checked == false && console.chkRX2.Visible) // .229
                                        {
                                            console.chkRX2.Checked = true;
                                            if (console.setupForm.chkRX2AutoVAC2.Checked)
                                            {
                                                console.setupForm.chkVAC2Enable.Checked = true; // .230
                                            }
                                        }

                                        return;
                                    }
                                    counter++;
                                }
                            }
                            else // SWL 
                            {
                                int zz = swl_index[iii];

                                double hh = (double)SpotControl.SWL_Freq[zz] / 1000000.0;

                                console.VFOBFreq = hh;
                                //  console.tempVFOAFreq = console.VFOAFreq; // ke9ns add  CTUN operation changed freq so update temp value


                                if (console.setupForm != null && console.setupForm.chkRX2AutoOn.Checked == true && console.chkRX2.Checked == false && console.chkRX2.Visible) // .229
                                {
                                    console.chkRX2.Checked = true;
                                    if (console.setupForm.chkRX2AutoVAC2.Checked)
                                    {
                                        console.setupForm.chkVAC2Enable.Checked = true; // .230
                                    }
                                }

                                return;
                            }

                        } // for i loop

                    }
                    catch (Exception)
                    {


                    }

                    Debug.WriteLine(" did not find a match ");
                    return;

                } // scantype = 1
                else if (scantype == 2)
                {
                    try
                    {
                        int ii = currFBox.GetCharIndexFromPosition(e.Location);

                        xxx = (ii / linelength); //find row 

                        if (xxx >= console.band_stacks[nnn]) return; // if you click past the last index freq, then do nothing.

                        Debug.WriteLine("1xxx " + xxx + " , " + ii);


                        currFBox.SelectionStart = (xxx * linelength);
                        currFBox.SelectionLength = linelength;

                        Debug.WriteLine("index at start of click " + console.iii);


                        console.iii = xxx; // update new position in bandstack for checking if its locked

                        Debug.WriteLine("index after click " + console.iii);

                        yyy = 0;


                        if (xxx > console.band_stacks[nnn])
                        {
                            return;

                        }


                        DB.GetBandStack(band_list[nnn], xxx, out mode, out filter, out freq);

                        // updateindex();

                        console.SetBand2(mode, filter, freq); // .222 rX2


                        if (console.setupForm != null && console.setupForm.chkRX2AutoOn.Checked == true && console.chkRX2.Checked == false && console.chkRX2.Visible) // .229
                        {
                            console.chkRX2.Checked = true;
                            if (console.setupForm.chkRX2AutoVAC2.Checked)
                            {
                                console.setupForm.chkVAC2Enable.Checked = true; // .230
                            }
                        }

                        console.UpdateWaterfallLevelValues();
                    }
                    catch
                    {
                        Debug.WriteLine("Failed to determine index or cannot save bandstack because its locked");

                        if (yyy == 1)
                        {
                            updateindex();

                            //   console.SetBand(mode1[xxx], filter1[xxx], freq1[xxx]);

                            console.UpdateWaterfallLevelValues();
                        }

                    }


                } // scantype = 2 (bandstack)



            } // RIGHT CLICK MOUSE VFOB RX2  .230 VFOB = mouse wheel (so Memory, Scanner, Spotter are all the same)
            else if (e.Button == MouseButtons.Right) // .226 only for memory channel scan toggle ON/OFF
            {
              
                if (comboBoxTS1.Text == "") return;
                if (textBox1.Text != "") return; // only for memory channel scan toggle ON/OFF

                    scanstop = true;

                if (scantype == 1)  // 1=memory, 2=band stack, 3= custom, 4= low-high, 5=SWL
                {
                    try
                    {
                        int ii = currFBox.GetCharIndexFromPosition(e.Location);

                        xxx = (ii / linelength); //find row 

                        // if (xxx >= console.band_stacks[nnn]) return; // if you click past the last index freq, then do nothing.

                        Debug.WriteLine("1xxx " + xxx + " , " + ii);


                        currFBox.SelectionStart = (xxx * linelength);
                        currFBox.SelectionLength = linelength;

                        currFBox.ScrollToCaret(); // keep highlighted line visable

                        Debug.WriteLine("index at start of click " + iii);


                        iii = xxx; // update new position in bandstack for checking if its locked

                        Debug.WriteLine("memcount " + memcount + " , " + memtotal + " , " + memIndex[xxx]);

                        yyy = 0;

                        if (textBox1.Text == "")  // MEMORY
                        {
                            if (iii > memIndex[memtotal])
                            {
                                Debug.WriteLine("clicked beyond index length " + memIndex[memtotal]);
                                return;
                            }
                        }
                        else // SWL
                        {
                            if (iii > memtotal)
                            {
                                Debug.WriteLine("1clicked beyond index length " + memtotal);
                                return;
                            }

                        }

                        if (comboBoxTS1.Text != "") //.221 add so clicking on memory in the scan screen will pull up all memory parameters
                        {
                            Debug.WriteLine("MEMORY CLICK. restore memory " + xxx);

                            comboMemGroupName.SelectedIndex = memIndex[xxx];
                            recordToRestore = new MemoryRecord((MemoryRecord)comboMemGroupName.SelectedItem); // ke9ns   you select index in the combo pulldown list

                            Debug.WriteLine("CHANGE MEMORY TO " + recordToRestore.RXFreq);
                            console.RecallMemory(recordToRestore);

                         
                        }

                        int counter = 0;

                       
                        for (int i = 0; i < memcount; i++) // find all the memories with the same group name
                        {

                            if (textBox1.Text == "")  // MEMORY
                            {

                                if (CultureInfo.InvariantCulture.CompareInfo.IndexOf((dataGridView2[0, i].Value.ToString()), Gname, CompareOptions.IgnoreCase) >= 0) // Gname must be contains in MEMORY (partial or full) and case insensitive)
                                {
                                   
                                    freq = Convert.ToDouble(dataGridView2[1, i].Value);  // MEMORY "RXFREQ"  convert to hz

                                    mode = dataGridView2[3, i].Value.ToString();  // DSPMODE of MEMORY

                                    filter = dataGridView2[20, i].Value.ToString();

                                    // you got a match to your GROUP name so check the line #
                                    if (counter == iii)
                                    {
                                        bool scan = (bool)dataGridView2["Scan", i].Value; // ke9ns add .226 
                                       
                                        if (scan == true) dataGridView2["Scan", i].Value = false; // ke9ns add .226 change value here (in memoryForm.cs)
                                        else dataGridView2["Scan", i].Value = true;
                                        
                                        console.SetBand(mode, filter, freq);

                                        UpdateText(); // update memory currfbox.text .226

                                        
                                        return;
                                    }
                                    counter++;
                                }
                            }
                            else // SWL 
                            {
                                int zz = swl_index[iii];

                                double hh = (double)SpotControl.SWL_Freq[zz] / 1000000.0;

                                console.VFOAFreq = hh;
                                console.tempVFOAFreq = console.VFOAFreq; // ke9ns add  CTUN operation changed freq so update temp value

                                return;
                            }

                        } // for i loop

                    }
                    catch (Exception)
                    {


                    }

                    Debug.WriteLine(" did not find a match ");
                    return;

                } // scantype = 1
               


            } // Wheel CLICK MOUSE (disable in memory scan)



        } // currFBox_MouseUp

        private void currFBox_MouseDown(object sender, MouseEventArgs e)
        {
            currFBox.ShortcutsEnabled = false; // added to eliminate the contextmenu from popping up

        } // currFBox_MouseDown

        private void chkBoxIdent_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button2_MouseEnter(object sender, EventArgs e)
        {
            Console.HELPSWR = true;
        }

        private void checkBoxSWR_MouseEnter(object sender, EventArgs e)
        {
            Console.HELPSWR = true;
        }

        private void checkBoxSWR_MouseLeave(object sender, EventArgs e)
        {
            Console.HELPSWR = false;
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            Console.HELPSWR = false;
        }


        //=========================================
        // for F1 help sceen
        private void ScanControl_KeyDown(object sender, KeyEventArgs e) // ke9ns keypreview must be TRUE and use MouseIsOverControl(Control c) 
        {
            Debug.WriteLine("F1 key2");



            if (e.KeyCode == Keys.F1) // ke9ns add for help messages (F1 help screen)
            {


                Debug.WriteLine("F1 key");

                if (MouseIsOverControl(checkBoxSWR) == true)
                {
                    if (console.helpboxForm == null || console.helpboxForm.IsDisposed) console.helpboxForm = new helpbox(console);

                    console.helpboxForm.Show();
                    console.helpboxForm.Focus();
                    console.helpboxForm.youtube_embed = @"https://www.youtube.com/embed/w5j6jh6c0_g?rel=0&amp";
                    console.helpboxForm.WindowState = FormWindowState.Normal; // ke9ns add

                    console.helpboxForm.helpbox_message.Text = console.helpboxForm.SWRScanner.Text;
                }
                else if (MouseIsOverControl(button2) == true)
                {
                    if (console.helpboxForm == null || console.helpboxForm.IsDisposed) console.helpboxForm = new helpbox(console);

                    console.helpboxForm.Show();
                    console.helpboxForm.Focus();
                    console.helpboxForm.youtube_embed = @"https://www.youtube.com/embed/w5j6jh6c0_g?rel=0&amp";
                    console.helpboxForm.WindowState = FormWindowState.Normal; // ke9ns add

                    console.helpboxForm.helpbox_message.Text = console.helpboxForm.SWRScanner.Text;
                }



            } // if (e.KeyCode == Keys.F1)




        } // SpotControl_KeyDown


        public bool MouseIsOverControl(Control c) // ke9ns keypreview must be TRUE
        {
            return c.ClientRectangle.Contains(c.PointToClient(Control.MousePosition));
        }


        private void ScanControl_MouseEnter(object sender, EventArgs e)
        {
            Console.HELPSWR = true;
            if (console.setupForm.chkBoxAutoFocus.Checked == true && chkAlwaysOnTop.Checked == true) this.Activate();
        }

        private void ScanControl_MouseLeave(object sender, EventArgs e)
        {
            Console.HELPSWR = false;
        }

        private void numericSWRTest_ValueChanged(object sender, EventArgs e)
        {
            console.SWR_TESTRUN = (int)numericSWRTest.Value;

        } // numericSWRTest_ValueChanged

        private void checkBoxSWR_CheckedChanged(object sender, EventArgs e)
        {
            if (console.VFOAFreq > 54) checkBoxSWR.Checked = false; // .226 no swr above 6m
        }

        private void chkBoxSQLBRKWait_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBoxSQLBRKWait.Checked == true)
            {
                chkBoxSQLBRK.Checked = false;

            }
        }

        private void chkBoxSQLBRK_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBoxSQLBRK.Checked == true)
            {
                chkBoxSQLBRKWait.Checked = false;

            }
        }

        private void button2_MouseDown(object sender, MouseEventArgs e)
        {

            MouseEventArgs me = (MouseEventArgs)e;

            if (me.Button == System.Windows.Forms.MouseButtons.Right) // right mouse click
            {

                string filePath = console.AppDataPath + @"SWR_PLOTS\";

                Debug.WriteLine("filepath: " + filePath);

                if (!Directory.Exists(filePath))
                {

                    Debug.WriteLine("problem folder found");
                    return;

                }


                string argument = @filePath;                     //@"/select, " + filePath;

                System.Diagnostics.Process.Start("explorer.exe", argument);

            }// right mouse click

        }

        private void button_reset_Click(object sender, EventArgs e) // ke9ns add .186
        {
            if (console.RX1Band >= 0)
            {
                console.SLowScan[(int)console.RX1Band] = " ";
                console.SHighScan[(int)console.RX1Band] = " ";

                console.scanUpdate = true; // force a simulated band button click
                console.RX1Band = console.RX1Band;

                lowFBox.Invalidate();
                highFBox.Invalidate();
            }

        } // button_reset_Click

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            comboBoxTS1.Text = "";

        }

        private void btnGroupMemory_MouseDown(object sender, MouseEventArgs e) //.236
        {

            MouseEventArgs me = (MouseEventArgs)e;

            if (me.Button == System.Windows.Forms.MouseButtons.Middle) // 
            {
                ScanVFOB = true;
            }
            else
            {
                ScanVFOB = false;
            }

        } // btnGroupMemory_MouseDown
    } // scancontrol


} // powersdr
