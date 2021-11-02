//=================================================================
// fwcAntForm.cs
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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace PowerSDR
{
    public partial class FWCAntForm : System.Windows.Forms.Form
    {
        #region Variable Declaration

        private Console console;
       
        #endregion

        #region Constructor and Destructor

        public FWCAntForm(Console c)
        {
            InitializeComponent();
            console = c;
            rx2_ok = FWCEEPROM.RX2OK;


            if (rx2_ok)
            {
                uint temp;
                FWC.GetRFIORev(out temp);
                if ((temp & 0xFF) < 34)
                {
                    comboRX2Ant.Items.Remove("ANT 1");
                    comboTXAnt2.Visible = false; // ke9ns add .205
                    labelTS1.Visible = false;
                    chkTX2Active.Visible = false;
                  

                    chkRX2TX1.Visible = false;
                    chkRX2TX2.Visible = false;
                    chkRX2TX3.Visible = false;
                   

                }
                if (chkTX2Active.Checked == false)
                {
                    comboTXAnt2.Visible = false; // ke9ns add .205
                    labelTS1.Visible = false;
                   

                    chkRX2TX1.Visible = false;
                    chkRX2TX2.Visible = false;
                    chkRX2TX3.Visible = false;
                }
                else
                {
                    chkRX2TX1.Visible = true;
                    chkRX2TX2.Visible = true;
                    chkRX2TX3.Visible = true;
                }

            }
            else
            {
                comboRX2Ant.Enabled = false;
                comboTXAnt2.Visible = false; // ke9ns add .205
                labelTS1.Visible = false;
                chkTX2Active.Visible = false;
              

                chkRX2TX1.Visible = false;
                chkRX2TX2.Visible = false;
                chkRX2TX3.Visible = false;
            }

            // Set mode first
            ArrayList a = DB.GetVars("FWCAnt");
            a.Sort();

            foreach (string s in a)
            {
                if (s.StartsWith("radModeExpert") && s.IndexOf("True") >= 0)
                {
                    radModeExpert.Checked = true;
                    break;
                }
            }

            Common.RestoreForm(this, "FWCAnt", false);

            if (radModeSimple.Checked)
                radModeSimple_CheckedChanged(this, EventArgs.Empty);

        } // FWCAntForm(Console c)

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

        private bool rx2_ok = false;
        public bool RX2OK
        {
            get { return rx2_ok; }
            set
            {
                rx2_ok = value;
                comboRX2Ant.Enabled = value;
            }
        }

        public AntMode CurrentAntMode
        {
            get
            {
                if (radModeSimple.Checked) return AntMode.Simple;
                else /*if(radModeExpert.Checked)*/ return AntMode.Expert;
            }
            set
            {
                switch (value)
                {
                    case AntMode.Simple:
                        radModeSimple.Checked = true;
                        break;
                    case AntMode.Expert:
                        radModeExpert.Checked = true;
                        break;
                }
            }
        }

        public FWCAnt RX1Ant
        {
            get { return StringToAnt(comboRX1Ant.Text); }
            set { comboRX1Ant.Text = AntToString(value); }
        }

        public FWCAnt RX2Ant
        {
            get { return StringToAnt(comboRX2Ant.Text); }
            set { comboRX2Ant.Text = AntToString(value); }
        }

        public FWCAnt TXAnt
        {
            get { return StringToAnt(comboTXAnt.Text); }
            set { comboTXAnt.Text = AntToString(value); }
        }

        // ke9ns add .205
        public FWCAnt TXAnt2
        {
            get { return StringToAnt(comboTXAnt2.Text); }
            set { comboTXAnt2.Text = AntToString(value); }
        }

        public bool RX1Loop
        {
            get { return chkRX1Loop.Checked; }
            set { chkRX1Loop.Checked = value; }
        }

        public bool RCATX1
        {
            get { return chkRCATX1.Checked; }
            set { chkRCATX1.Checked = value; }
        }

        public bool RCATX2
        {
            get { return chkRCATX2.Checked; }
            set { chkRCATX2.Checked = value; }
        }

        public bool RCATX3
        {
            get { return chkRCATX3.Checked; }
            set { chkRCATX3.Checked = value; }
        }

        public bool TX1DelayEnable
        {
            get { return chkTX1DelayEnable.Checked; }
            set { chkTX1DelayEnable.Checked = value; }
        }

        public bool TX2DelayEnable
        {
            get { return chkTX2DelayEnable.Checked; }
            set { chkTX2DelayEnable.Checked = value; }
        }

        public bool TX3DelayEnable
        {
            get { return chkTX3DelayEnable.Checked; }
            set { chkTX3DelayEnable.Checked = value; }
        }

        public bool AntLock
        {
            get { return chkLock.Checked; }
            set { chkLock.Checked = value; }
        }

        public uint TX1Delay
        {
            get { return (uint)udTX1Delay.Value; }
            set { udTX1Delay.Value = value; }
        }

        public uint TX2Delay
        {
            get { return (uint)udTX2Delay.Value; }
            set { udTX2Delay.Value = value; }
        }

        public uint TX3Delay
        {
            get { return (uint)udTX3Delay.Value; }
            set { udTX3Delay.Value = value; }
        }


        #endregion

        #region Misc Routines

        public void SetBand(Band b)
        {
            comboBand.Text = BandToString(b);
        }

        // ke9ns add for RX2
        public void SetBand2(Band b)
        {
            comboBand2.Text = BandToString(b);
        }
        private string BandToString(Band b)
        {
            string ret_val = "";
            switch (b)
            {
                case Band.GEN: ret_val = "GEN"; break; // ke9ns mod
                case Band.B160M: ret_val = "160m"; break;
                case Band.B80M: ret_val = "80m"; break;
                case Band.B60M: ret_val = "60m"; break;
                case Band.B40M: ret_val = "40m"; break;
                case Band.B30M: ret_val = "30m"; break;
                case Band.B20M: ret_val = "20m"; break;
                case Band.B17M: ret_val = "17m"; break;
                case Band.B15M: ret_val = "15m"; break;
                case Band.B12M: ret_val = "12m"; break;
                case Band.B10M: ret_val = "10m"; break;
                case Band.B6M: ret_val = "6m"; break;
                case Band.WWV: ret_val = "WWV"; break;

                case Band.VHF0: ret_val = "VU 2m"; break;
                case Band.VHF1: ret_val = "VU 70cm"; break;
                case Band.VHF2: ret_val = "VHF2"; break;
                case Band.VHF3: ret_val = "VHF3"; break;
                case Band.VHF4: ret_val = "VHF4"; break;
                case Band.VHF5: ret_val = "VHF5"; break;
                case Band.VHF6: ret_val = "VHF6"; break;
                case Band.VHF7: ret_val = "VHF7"; break;
                case Band.VHF8: ret_val = "VHF8"; break;
                case Band.VHF9: ret_val = "VHF9"; break;
                case Band.VHF10: ret_val = "VHF10"; break;
                case Band.VHF11: ret_val = "VHF11"; break;
                case Band.VHF12: ret_val = "VHF12"; break;
                case Band.VHF13: ret_val = "VHF13"; break;

                case Band.BLMF: ret_val = "LMF"; break;  // ke9ns add
                case Band.B120M: ret_val = "120m"; break;
                case Band.B90M: ret_val = "90m"; break;
                case Band.B61M: ret_val = "61m"; break;
                case Band.B49M: ret_val = "49m"; break;
                case Band.B41M: ret_val = "41m"; break;
                case Band.B31M: ret_val = "31m"; break;
                case Band.B25M: ret_val = "25m"; break;
                case Band.B22M: ret_val = "22m"; break;
                case Band.B19M: ret_val = "19m"; break;
                case Band.B16M: ret_val = "16m"; break;
                case Band.B14M: ret_val = "14m"; break;
                case Band.B13M: ret_val = "13m"; break;
                case Band.B11M: ret_val = "11m"; break;



            }
            return ret_val;

        } // BandToString

        private Band StringToBand(string s)
        {
            Band b = Band.GEN;
            switch (s)
            {
                case "GEN": b = Band.GEN; break;  // ke9ns mod
                case "160m": b = Band.B160M; break;
                case "80m": b = Band.B80M; break;
                case "60m": b = Band.B60M; break;
                case "40m": b = Band.B40M; break;
                case "30m": b = Band.B30M; break;
                case "20m": b = Band.B20M; break;
                case "17m": b = Band.B17M; break;
                case "15m": b = Band.B15M; break;
                case "12m": b = Band.B12M; break;
                case "10m": b = Band.B10M; break;
                case "6m": b = Band.B6M; break;
                case "WWV": b = Band.WWV; break;

                case "VU 2m": b = Band.VHF0; break;
                case "VU 70cm": b = Band.VHF1; break;
                case "VHF2": b = Band.VHF2; break;
                case "VHF3": b = Band.VHF3; break;
                case "VHF4": b = Band.VHF4; break;
                case "VHF5": b = Band.VHF5; break;
                case "VHF6": b = Band.VHF6; break;
                case "VHF7": b = Band.VHF7; break;
                case "VHF8": b = Band.VHF8; break;
                case "VHF9": b = Band.VHF9; break;
                case "VHF10": b = Band.VHF10; break;
                case "VHF11": b = Band.VHF11; break;
                case "VHF12": b = Band.VHF12; break;
                case "VHF13": b = Band.VHF13; break;

                case "LMF": b = Band.BLMF; break; // ke9ns add
                case "120m": b = Band.B120M; break;
                case "90m": b = Band.B90M; break;
                case "61m": b = Band.B61M; break;
                case "49m": b = Band.B49M; break;
                case "41m": b = Band.B41M; break;
                case "31m": b = Band.B31M; break;
                case "25m": b = Band.B25M; break;
                case "22m": b = Band.B22M; break;
                case "19m": b = Band.B19M; break;
                case "16m": b = Band.B16M; break;
                case "14m": b = Band.B14M; break;
                case "13m": b = Band.B13M; break;
                case "11m": b = Band.B11M; break;

            }
            return b;
        } //StringToBand

        private string AntToString(FWCAnt ant)
        {
            string ret_val = "";
            switch (ant)
            {
                case FWCAnt.NC: ret_val = "N/C"; break;
                case FWCAnt.ANT1: ret_val = "ANT 1"; break;
                case FWCAnt.ANT2: ret_val = "ANT 2"; break;
                case FWCAnt.ANT3: ret_val = "ANT 3"; break;
                case FWCAnt.RX1IN: ret_val = "RX1 IN"; break;
                case FWCAnt.RX2IN: ret_val = "RX2 IN"; break;
                case FWCAnt.RX1TAP: ret_val = "RX1 Tap"; break;
            }
            return ret_val;
        }//AntToString

        public FWCAnt StringToAnt(string s)
        {
            FWCAnt ant = FWCAnt.ANT1;
            switch (s)
            {
                case "N/C": ant = FWCAnt.NC; break;
                case "ANT 1": ant = FWCAnt.ANT1; break;
                case "ANT 2": ant = FWCAnt.ANT2; break;
                case "ANT 3": ant = FWCAnt.ANT3; break;
                case "RX1 IN": ant = FWCAnt.RX1IN; break;
                case "RX2 IN": ant = FWCAnt.RX2IN; break;
                case "RX1 Tap": ant = FWCAnt.RX1TAP; break;
            }
            return ant;
        }

        #endregion

        #region Event Handlers

        private void FWCAntForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
            Common.SaveForm(this, "FWCAnt");
        }

        private void chkRX1ExtAnt_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radModeSimple.Checked)
            {
                for (int i = 0; i < (int)Band.LAST; i++)
                    console.SetRX1Loop((Band)i, chkRX1Loop.Checked);
            }
            else // advanced
            {
                console.SetRX1Loop(StringToBand(comboBand.Text), chkRX1Loop.Checked);
            }
        }

        private void radModeSimple_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radModeSimple.Checked)
            {
                console.CurrentAntMode = AntMode.Simple;

                lblBand.Visible = false;
                lblBand2.Visible = false;

                comboBand.Visible = false;
                comboBand2.Visible = false; // ke9ns add
                grpSwitchRelay.Visible = true;

                comboRX1Ant.Text = AntToString(console.RX1Ant);
                comboRX2Ant.Text = AntToString(console.RX2Ant);
                comboTXAnt.Text = AntToString(console.TXAnt);
                chkRX1Loop.Checked = console.RX1Loop;

                if (console.TXBand == Band.B6M)
                    comboTXAnt.Enabled = false;

                chkRCATX1_CheckedChanged(this, EventArgs.Empty);
                chkRCATX2_CheckedChanged(this, EventArgs.Empty);
                chkRCATX3_CheckedChanged(this, EventArgs.Empty);

                txtStatus.Text = "Simple Mode: Settings are applied to all bands";
            }
        }



        private void radModeExpert_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radModeExpert.Checked)
            {
                console.CurrentAntMode = AntMode.Expert;

                lblBand.Visible = true;
                comboBand.Visible = true;


                if ((rx2_ok) && (console.CurrentModel == Model.FLEX5000) && (console.chkRX2.Checked == true)) // ke9ns add: also checked in console chkRX2 checkchanged
                {
                    comboBand2.Visible = true; // ke9ns add
                    lblBand2.Visible = true;
                }


                grpSwitchRelay.Visible = true;
                if (comboBand.Text != "6m" || (byte)FWCEEPROM.RFIORev >= 34) comboTXAnt.Enabled = true;

                comboBand.Text = BandToString(console.RX1Band);
                comboRX1Ant.Text = AntToString(console.RX1Ant);

                if (rx2_ok)
                {
                    comboRX2Ant.Text = AntToString(console.RX2Ant);
                    comboTXAnt2.Text = AntToString(console.TXAnt2); // ke9ns add .205
                }
                comboTXAnt.Text = AntToString(console.TXAnt);

                txtStatus.Text = "Expert Mode: Settings applied only to selected band";
            }

        } // radModeExpert_CheckedChanged

        //========================================================================================================
        private void comboBand_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            Band band = StringToBand(comboBand.Text);
            Band band2 = StringToBand(comboBand2.Text);

            if (!radModeSimple.Checked) // advanced
            {
                if ((byte)(FWCEEPROM.RFIORev) < 34)
                {
                    switch (band)
                    {
                        case Band.B6M:
                            if (comboTXAnt.Items.Contains("ANT 1")) comboTXAnt.Items.Remove("ANT 1");
                            if (comboTXAnt.Items.Contains("ANT 2")) comboTXAnt.Items.Remove("ANT 2");
                            break;
                        default:
                            if (!comboTXAnt.Items.Contains("ANT 2")) comboTXAnt.Items.Insert(0, "ANT 2");
                            if (!comboTXAnt.Items.Contains("ANT 1")) comboTXAnt.Items.Insert(0, "ANT 1");
                            break;
                    }
                }
                comboRX1Ant.Text = AntToString(console.GetRX1Ant(band));
                comboRX2Ant.Text = AntToString(console.GetRX2Ant(band2)); // ke9ns mod: was band

                if ((byte)(FWCEEPROM.RFIORev) < 34)
                {
                    if (band == Band.B6M)
                    {
                        comboTXAnt.Text = "ANT 3";
                        comboTXAnt.Enabled = false;
                    }
                    else
                    {
                        comboTXAnt.Text = AntToString(console.GetTXAnt(band));
                        comboTXAnt.Enabled = true;
                    }
                }
                else
                {
                    comboTXAnt.Text = AntToString(console.GetTXAnt(band));
                }


                chkRX1Loop.Checked = console.GetRX1Loop(band);
                chkRCATX1.Checked = console.GetTX1(band);
                chkRCATX2.Checked = console.GetTX2(band);
                chkRCATX3.Checked = console.GetTX3(band);
            }
            else // simple
            {
                if ((byte)(FWCEEPROM.RFIORev) < 34)
                {
                    switch (band)
                    {
                        case Band.B6M:
                            comboTXAnt.Text = "ANT 3";
                            comboTXAnt.Enabled = false;
                            break;
                        default:
                            comboTXAnt.Text = AntToString(console.TXAnt);
                            comboTXAnt.Enabled = true;
                            break;
                    }
                }
                else
                {
                    comboTXAnt.Text = AntToString(console.TXAnt);
                }
            }

        } //comboBand_SelectedIndexChanged



        private void comboRX1Ant_SelectedIndexChanged(object sender, System.EventArgs e)
        {

            
            if ((comboRX1Ant.Focused || comboTXAnt.Focused) && rx2_ok && comboRX1Ant.Text == "ANT 1" && comboRX2Ant.Text == "ANT 1")
            {
                MessageBox.Show("Antenna 1 is currently in use by RX2.  Please change that " +
                    "setting in order to use Antenna 1 with RX1.",
                    "Antenna 1 in use",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                comboRX1Ant.Text = AntToString(console.RX1Ant);
                return;
            }

            if (comboRX1Ant.Text == "RX1 IN")
                chkRX1Loop.Enabled = false;
            else
                chkRX1Loop.Enabled = true;

            console.SetRX1Ant(StringToBand(comboBand.Text), StringToAnt(comboRX1Ant.Text));


            if (chkLock.Checked)
            {
                switch (comboRX1Ant.Text)
                {
                    case "ANT 1":
                    case "ANT 2":
                    case "ANT 3":
                        comboTXAnt.Text = comboRX1Ant.Text;
                        break;
                }
            }

            if (radModeExpert.Checked) console.CurrentAntMode = AntMode.Expert; // ke9ns add: update console Ant display .119
            else console.CurrentAntMode = AntMode.Simple;

        } //comboRX1Ant_SelectedIndexChanged

        private void comboRX2Ant_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if ((comboRX2Ant.Focused || comboTXAnt.Focused) && rx2_ok && comboRX1Ant.Text == "ANT 1" && comboRX2Ant.Text == "ANT 1")
            {
                MessageBox.Show("Antenna 1 is currently in use by RX1.  Please change that " +
                    "setting in order to use Antenna 1 with RX2.",
                    "Antenna 1 in use",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                comboRX2Ant.Text = AntToString(console.RX2Ant);
                return;
            }
            if (rx2_ok) console.SetRX2Ant(StringToBand(comboBand.Text), StringToAnt(comboRX2Ant.Text));


            if (chkLock.Checked)
            {
                if (chkTX2Active.Checked)
                {
                    switch (comboRX2Ant.Text)
                    {
                        case "ANT 1":
                        case "ANT 2":
                        case "ANT 3":
                            comboTXAnt2.Text = comboRX2Ant.Text;
                            break;
                    }
                }
            }

            if (radModeExpert.Checked) console.CurrentAntMode = AntMode.Expert; // ke9ns add: update console Ant display .119
            else console.CurrentAntMode = AntMode.Simple;

        } // comboRX2Ant_SelectedIndexChanged

        //==========================================================================================
        private void comboTXAnt_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if ((byte)(FWCEEPROM.RFIORev) < 34)
            {
                switch (StringToBand(comboBand.Text))
                {
                    case Band.B6M: // do nothing
                        break;
                    default:
                        console.SetTXAnt(StringToBand(comboBand.Text), StringToAnt(comboTXAnt.Text));
                        break;
                }
            }
            else
            {
                console.SetTXAnt(StringToBand(comboBand.Text), StringToAnt(comboTXAnt.Text));
            }

            if (chkLock.Checked) comboRX1Ant.Text = comboTXAnt.Text;

            if (radModeExpert.Checked) console.CurrentAntMode = AntMode.Expert; // ke9ns add: update console Ant display .119
            else console.CurrentAntMode = AntMode.Simple;

        } // comboTXAnt_SelectedIndexChanged(

        // ke9ns add .205
        private void comboTXAnt2_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if ((byte)(FWCEEPROM.RFIORev) < 34)
            {
                switch (StringToBand(comboBand.Text))
                {
                    case Band.B6M: // do nothing
                        break;
                    default:
                        console.SetTXAnt2(StringToBand(comboBand2.Text), StringToAnt(comboTXAnt.Text));
                        break;
                }
            }
            else
            {
                
                console.SetTXAnt2(StringToBand(comboBand2.Text), StringToAnt(comboTXAnt2.Text));
            }

         //   if (chkLock.Checked)
          //  {

           //     comboRX2Ant.Text = comboTXAnt2.Text;
           // }

            if (radModeExpert.Checked) console.CurrentAntMode = AntMode.Expert; // ke9ns add: update console Ant display .119
            else console.CurrentAntMode = AntMode.Simple;

        } // comboTXAnt2_SelectedIndexChanged(





        private void chkRCATX1_CheckedChanged(object sender, System.EventArgs e)
        {
            console.SetTX1(StringToBand(comboBand.Text), chkRCATX1.Checked);
            chkTX1DelayEnable.Enabled = chkRCATX1.Checked;
        }

        private void chkRCATX2_CheckedChanged(object sender, System.EventArgs e)
        {
            console.SetTX2(StringToBand(comboBand.Text), chkRCATX2.Checked);
            chkTX2DelayEnable.Enabled = chkRCATX2.Checked;
        }

        private void chkRCATX3_CheckedChanged(object sender, System.EventArgs e)
        {
            console.SetTX3(StringToBand(comboBand.Text), chkRCATX3.Checked);
            chkTX3DelayEnable.Enabled = chkRCATX3.Checked;
        }

        private void chkRX1TX1_CheckedChanged(object sender, System.EventArgs e)
        {
            console.SetRX2TX1(StringToBand(comboBand.Text), chkRX2TX1.Checked, chkRCATX1.Checked);
           
        }

        private void chkRX1TX2_CheckedChanged(object sender, System.EventArgs e)
        {
            console.SetRX2TX2(StringToBand(comboBand.Text), chkRX2TX2.Checked, chkRCATX1.Checked);

        }

        private void chkRX1TX3_CheckedChanged(object sender, System.EventArgs e)
        {
            console.SetRX2TX3(StringToBand(comboBand.Text), chkRX2TX3.Checked, chkRCATX1.Checked);

        }

        private void chkLock_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkLock.Checked)
            {
                switch (comboRX1Ant.Text)
                {
                    case "ANT 1":
                    case "ANT 2":
                    case "ANT 3":
                        comboTXAnt.Text = comboRX1Ant.Text;
                        break;
                }

                if (chkTX2Active.Checked) // ke9ns add .207
                {
                    switch (comboRX2Ant.Text)
                    {
                        case "ANT 1":
                        case "ANT 2":
                        case "ANT 3":
                            comboTXAnt2.Text = comboRX2Ant.Text;
                            break;
                        case "RX2 IN":
                            break;
                        case "RX1 Tap":
                            comboTXAnt.Text = comboRX1Ant.Text; // ke9ns add .205 use RX1 TX ant
                            break;

                    }
                }
            }
        }

        private void chkTX1DelayEnable_CheckedChanged(object sender, System.EventArgs e)
        {
            FWC.SetAmpTX1DelayEnable(chkTX1DelayEnable.Checked);
            udTX1Delay.Enabled = chkTX1DelayEnable.Checked;
        }

        private void chkTX2DelayEnable_CheckedChanged(object sender, System.EventArgs e)
        {
            FWC.SetAmpTX2DelayEnable(chkTX2DelayEnable.Checked);
            udTX2Delay.Enabled = chkTX2DelayEnable.Checked;
        }

        private void chkTX3DelayEnable_CheckedChanged(object sender, System.EventArgs e)
        {
            FWC.SetAmpTX3DelayEnable(chkTX3DelayEnable.Checked);
            udTX3Delay.Enabled = chkTX3DelayEnable.Checked;
        }

        private void udTX1Delay_ValueChanged(object sender, System.EventArgs e)
        {
            FWC.SetAmpTX1Delay((uint)udTX1Delay.Value);
        }

        private void udTX2Delay_ValueChanged(object sender, System.EventArgs e)
        {
            FWC.SetAmpTX2Delay((uint)udTX2Delay.Value);
        }

        private void udTX3Delay_ValueChanged(object sender, System.EventArgs e)
        {
            FWC.SetAmpTX3Delay((uint)udTX3Delay.Value);
        }

        private void chkEnable6mPreamp_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkEnable6mPreamp.Checked && chkEnable6mPreamp.Focused)
            {
                DialogResult dr = MessageBox.Show(
                    "Warning: External Preamp is required above 28MHz to maintain CE compliance " +
                    "with the internal preamp turned off.  By selecting this option, user accepts " +
                    "responsibility for maintaining compliance.",
                    "Warning: External Preamp Required",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Warning);

                if (dr == DialogResult.Cancel)
                {
                    chkEnable6mPreamp.Checked = false;
                    return;
                }
            }
            console.Enable6mPreamp = chkEnable6mPreamp.Checked;
        }

        #endregion

        private void udLoopGain_ValueChanged(object sender, System.EventArgs e)
        {
            console.LoopGain = -(float)udLoopGain.Value;
        }

        // ke9ns add for help
        private void textBoxTS1_Click(object sender, EventArgs e)
        {
            if (console.helpboxForm == null || console.helpboxForm.IsDisposed) console.helpboxForm = new helpbox(console);

            console.helpboxForm.Show();
            console.helpboxForm.Focus();
            console.helpboxForm.WindowState = FormWindowState.Normal; // ke9ns add
            console.helpboxForm.youtube_embed = @"https://ke9ns.com/flexpage.html";
            console.helpboxForm.helpbox_message.Text = console.helpboxForm.AntennaDelay.Text;
        }

        private void FWCAntForm_KeyDown(object sender, KeyEventArgs e)
        {


            if (e.KeyCode == Keys.F1) // ke9ns add for help messages (F1 help screen)
            {


                if (console.helpboxForm == null || console.helpboxForm.IsDisposed) console.helpboxForm = new helpbox(console);

                console.helpboxForm.Show();
                console.helpboxForm.Focus();
                console.helpboxForm.WindowState = FormWindowState.Normal; // ke9ns add
                console.helpboxForm.youtube_embed = @"https://ke9ns.com/flexpage.html";
                console.helpboxForm.helpbox_message.Text = console.helpboxForm.AntennaDelay.Text;




            } // if (e.KeyCode == Keys.F1)


        }

        private void checkBoxTS1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void ChkAlwaysOnTop1_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = chkAlwaysOnTop1.Checked;
        }

        private void FWCAntForm_MouseEnter(object sender, EventArgs e)
        {
            if (console.setupForm.chkBoxAutoFocus.Checked == true && chkAlwaysOnTop1.Checked == true) this.Activate();
        }

        private void chkTX2Active_CheckedChanged(object sender, EventArgs e) // ke9ns add .205
        {
            rx2_ok = FWCEEPROM.RX2OK;

            if (rx2_ok)
            {
                uint temp;
                FWC.GetRFIORev(out temp);
                if ((temp & 0xFF) < 34)
                {
                   
                    comboTXAnt2.Visible = false; // ke9ns add .205
                    labelTS1.Visible = false;
                    console.panelAntenna.Size = new Size(115, 58);

                }
                else if (chkTX2Active.Checked)
                {
                    comboTXAnt2.Visible = true; // ke9ns add .205
                    labelTS1.Visible = true;
                    chkRX2TX1.Visible = true;
                    chkRX2TX2.Visible = true;
                    chkRX2TX3.Visible = true;
                    console.panelAntenna.Size = new Size(115, 78);

                }
                else
                {
                    comboTXAnt2.Visible = false; // ke9ns add .205
                    labelTS1.Visible = false;
                   

                    chkRX2TX1.Visible = false;
                    chkRX2TX2.Visible = false;
                    chkRX2TX3.Visible = false;

                    console.panelAntenna.Size = new Size(115, 58);
                   


                }

                
            }
            else
            {
                comboTXAnt2.Visible = false; // ke9ns add .205
                labelTS1.Visible = false;
                chkRX2TX1.Checked = false;
                chkRX2TX2.Checked = false;
                chkRX2TX3.Checked = false;

                chkRX2TX1.Visible = false;
                chkRX2TX2.Visible = false;
                chkRX2TX3.Visible = false;

                console.panelAntenna.Size = new Size(115, 58);
            }

            console.panelAntenna.Invalidate();

        } // chkTX2Active_CheckedChanged
    }
}
