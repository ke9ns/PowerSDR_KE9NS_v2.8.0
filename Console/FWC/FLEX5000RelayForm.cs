//=================================================================
// FLEX5000RelayForm.cs
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

using System.Drawing;

namespace PowerSDR
{
    public partial class FLEX5000RelayForm : System.Windows.Forms.Form
    {
        #region Variable Declaration

        private Console console;
       

        #endregion

        #region Constructor and Destructor

        public FLEX5000RelayForm(Console c)
        {
            InitializeComponent();
            console = c;

            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
            comboBox4.SelectedIndex = 0;
            comboBox6.SelectedIndex = 0;
            comboBox7.SelectedIndex = 0;
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

        #region Misc Routines

        private string BandToString(Band b)
        {
            string ret_val = "";
            switch (b)
            {
                case Band.GEN: ret_val = "GEN"; break;
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
                case Band.B2M: ret_val = "2m"; break;
                case Band.WWV: ret_val = "WWV"; break;
                case Band.VHF0: ret_val = "VHF0"; break;
                case Band.VHF1: ret_val = "VHF1"; break;
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
            }
            return ret_val;
        }

        private Band StringToBand(string s)
        {
            Band b = Band.GEN;
            switch (s)
            {
                case "GEN": b = Band.GEN; break;
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
                case "2m": b = Band.B2M; break;
                case "WWV": b = Band.WWV; break;
                case "VFO0": b = Band.VHF0; break;
                case "VFO1": b = Band.VHF1; break;
                case "VFO2": b = Band.VHF2; break;
                case "VFO3": b = Band.VHF3; break;
                case "VFO4": b = Band.VHF4; break;
                case "VFO5": b = Band.VHF5; break;
                case "VFO6": b = Band.VHF6; break;
                case "VFO7": b = Band.VHF7; break;
                case "VFO8": b = Band.VHF8; break;
                case "VFO9": b = Band.VHF9; break;
                case "VFO10": b = Band.VHF10; break;
                case "VFO11": b = Band.VHF11; break;
                case "VFO12": b = Band.VHF12; break;
                case "VFO13": b = Band.VHF13; break;
            }
            return b;
        }

        public void UpdateRelayState(out bool tx1, out bool tx2, out bool tx3)
        {
            bool t1 = false, t2 = false, t3 = false;
            bool power = console.PowerOn;
            Band band = console.TXBand;
            DSPMode mode = console.RX1DSPMode;

            for (int i = lstRules.Items.Count - 1; i >= 0; i--)
            {
                string s = (string)lstRules.Items[i];
                string[] words = s.Split(' ');

                bool c1 = false, c2 = false;
                switch (words[4])
                {
                    case "Power":
                        if ((words[5] == "Is" &&
                            ((words[6] == "On" && power) ||
                            (words[6] == "Off" && !power))) ||
                            (words[5] == "Is_Not" &&
                            ((words[6] == "On" && !power) ||
                            (words[6] == "Off" && power))))
                        {
                            c1 = true;
                        }
                        break;
                    case "Band":
                        if ((words[5] == "Is" && StringToBand(words[6]) == band) ||
                            (words[5] == "Is_Not" && StringToBand(words[6]) != band))
                        {
                            c1 = true;
                        }
                        break;
                    case "Mode":
                        if ((words[5] == "Is" && words[6] == mode.ToString()) ||
                            (words[5] == "Is_Not" && words[6] != mode.ToString()))
                        {
                            c1 = true;
                        }
                        break;
                }

                if (words.Length == 7) c2 = true;
                else if (words.Length == 11)
                {
                    switch (words[8])
                    {
                        case "Power":
                            if ((words[9] == "Is" &&
                                ((words[10] == "On" && power) ||
                                (words[10] == "Off" && !power))) ||
                                (words[9] == "Is_Not" &&
                                ((words[10] == "On" && !power) ||
                                (words[10] == "Off" && power))))
                            {
                                c2 = true;
                            }
                            break;
                        case "Band":
                            if ((words[9] == "Is" && StringToBand(words[10]) == band) ||
                                (words[9] == "Is_Not" && StringToBand(words[10]) != band))
                            {
                                c2 = true;
                            }
                            break;
                        case "Mode":
                            if ((words[9] == "Is" && words[10] == mode.ToString()) ||
                                (words[9] == "Is_Not" && words[10] != mode.ToString()))
                            {
                                c2 = true;
                            }
                            break;
                    }
                }

                if (c1 && c2)
                {
                    if (words[1] == "On")
                    {
                        switch (words[2])
                        {
                            case "TX1": t1 = true; break;
                            case "TX2": t2 = true; break;
                            case "TX3": t3 = true; break;
                        }
                    }
                    else // words[1] == "Off"
                    {
                        switch (words[2])
                        {
                            case "TX1": t1 = false; break;
                            case "TX2": t2 = false; break;
                            case "TX3": t3 = false; break;
                        }
                    }
                }
                else
                {
                    if (words[1] == "On")
                    {
                        switch (words[2])
                        {
                            case "TX1": t1 = false; break;
                            case "TX2": t2 = false; break;
                            case "TX3": t3 = false; break;
                        }
                    }
                    else // words[1] == "Off"
                    {
                        switch (words[2])
                        {
                            case "TX1": t1 = true; break;
                            case "TX2": t2 = true; break;
                            case "TX3": t3 = true; break;
                        }
                    }
                }
            }

            tx1 = t1;
            tx2 = t2;
            tx3 = t3;

            if (tx1) btnTX1.BackColor = Color.Green;
            else btnTX1.BackColor = Color.Red;

            if (tx2) btnTX2.BackColor = Color.Green;
            else btnTX2.BackColor = Color.Red;

            if (tx3) btnTX3.BackColor = Color.Green;
            else btnTX3.BackColor = Color.Red;
        }

        #endregion

        #region Event Handlers

        private void comboBox3_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            comboBox5.Items.Clear();
            switch (comboBox3.Text)
            {
                case "Power":
                    comboBox5.Items.Add("On");
                    comboBox5.Items.Add("Off");
                    break;
                case "Band":
                    for (int i = (int)Band.FIRST + 1; i < (int)Band.LAST; i++)
                        comboBox5.Items.Add(BandToString((Band)i));
                    break;
                case "Mode":
                    for (int i = (int)DSPMode.FIRST + 1; i < (int)DSPMode.LAST; i++)
                        comboBox5.Items.Add(((DSPMode)i).ToString());
                    break;
            }
            comboBox5.SelectedIndex = 0;
        }

        private void comboBox6_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            comboBox8.Items.Clear();
            switch (comboBox6.Text)
            {
                case "Power":
                    comboBox8.Items.Add("On");
                    comboBox8.Items.Add("Off");
                    break;
                case "Band":
                    for (int i = (int)Band.FIRST + 1; i < (int)Band.LAST; i++)
                        comboBox8.Items.Add(BandToString((Band)i));
                    break;
                case "Mode":
                    for (int i = (int)DSPMode.FIRST + 1; i < (int)DSPMode.LAST; i++)
                        comboBox8.Items.Add(((DSPMode)i).ToString());
                    break;
            }
            comboBox8.SelectedIndex = 0;

        }

        private void btnMoreLess_Click(object sender, System.EventArgs e)
        {
            if (btnMoreLess.Text == "More")
            {
                lblAnd.Visible = true;
                comboBox6.Visible = true;
                comboBox7.Visible = true;
                comboBox8.Visible = true;
                btnMoreLess.Text = "Less";
            }
            else if (btnMoreLess.Text == "Less")
            {
                lblAnd.Visible = false;
                comboBox6.Visible = false;
                comboBox7.Visible = false;
                comboBox8.Visible = false;
                btnMoreLess.Text = "More";
            }
        }

        private void btnAdd_Click(object sender, System.EventArgs e)
        {
            string s = "Turn " + comboBox1.Text + " " + comboBox2.Text + " when " + comboBox3.Text +
                " " + comboBox4.Text + " " + comboBox5.Text;
            if (btnMoreLess.Text == "Less")
                s += " and " + comboBox6.Text + " " + comboBox7.Text + " " + comboBox8.Text;
            lstRules.Items.Add(s);
        }

        private void btnRemove_Click(object sender, System.EventArgs e)
        {
            if (lstRules.SelectedIndex >= 0)
                lstRules.Items.RemoveAt(lstRules.SelectedIndex);
        }

        private void btnMoveToTop_Click(object sender, System.EventArgs e)
        {
            int index = lstRules.SelectedIndex;
            if (index >= 1 && lstRules.Items.Count > 1)
            {
                object temp = lstRules.Items[index];
                lstRules.Items.RemoveAt(index);
                lstRules.Items.Insert(0, temp);
                lstRules.SelectedIndex = 0;
            }
        }

        private void btnMoveUp_Click(object sender, System.EventArgs e)
        {
            int index = lstRules.SelectedIndex;
            if (index >= 1 && lstRules.Items.Count > 1)
            {
                object temp = lstRules.Items[index];
                lstRules.Items.RemoveAt(index);
                lstRules.Items.Insert(index - 1, temp);
                lstRules.SelectedIndex = index - 1;
            }
        }

        private void btnMoveDown_Click(object sender, System.EventArgs e)
        {
            int index = lstRules.SelectedIndex;
            if (index >= 0 && index < lstRules.Items.Count - 1 && lstRules.Items.Count > 1)
            {
                object temp = lstRules.Items[index];
                lstRules.Items.RemoveAt(index);
                lstRules.Items.Insert(index + 1, temp);
                lstRules.SelectedIndex = index + 1;
            }
        }

        private void btnMoveToEnd_Click(object sender, System.EventArgs e)
        {
            int index = lstRules.SelectedIndex;
            if (index >= 0 && index < lstRules.Items.Count - 1 && lstRules.Items.Count > 1)
            {
                object temp = lstRules.Items[index];
                lstRules.Items.RemoveAt(index);
                lstRules.Items.Add(temp);
                lstRules.SelectedIndex = lstRules.Items.Count - 1;
            }
        }

        private void FLEX5000RelayForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        private void btnClear_Click(object sender, System.EventArgs e)
        {
            lstRules.Items.Clear();
        }

        #endregion

        private void btnUpdate_Click(object sender, System.EventArgs e)
        {
            bool b1, b2, b3;
            UpdateRelayState(out b1, out b2, out b3);
        }
    }
}
