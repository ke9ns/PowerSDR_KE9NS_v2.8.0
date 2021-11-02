//=================================================================
// ucbform.cs
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
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace PowerSDR
{
    /// <summary>
    /// Summary description for UCBForm.
    /// </summary>
    public partial class UCBForm : System.Windows.Forms.Form
    {
        #region Variable Declaration

        private Console console;
        private bool ucb_busy = false;
        private HW hw;
        private byte address;
        private int val;
        private UCB.CMD cmd;


        #endregion

        #region Constructor and Destructor

        public UCBForm(Console c)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            console = c;
            hw = console.Hdw;
            comboDelay.SelectedIndex = 0;
            Common.RestoreForm(this, "UCB", false);

            switch (console.CurrentModel)
            {
                case Model.FLEX3000:
                case Model.FLEX5000:
                case Model.FLEX1500:
                    btnEnable.Visible = false;
                    btnDisable.Visible = false;
                    lblDelay.Visible = false;
                    comboDelay.Visible = false;
                    btnSetDelay.Visible = false;
                    btnDisableClear.Visible = false;
                    btnWriteAll.Visible = false;
                    btnWriteLine0.Visible = false;
                    btnWriteLine1.Visible = false;
                    btnWriteLine2.Visible = false;
                    btnWriteLine3.Visible = false;
                    btnWriteLine4.Visible = false;
                    btnWriteLine5.Visible = false;
                    btnWriteLine6.Visible = false;
                    btnWriteLine7.Visible = false;
                    btnWriteLine8.Visible = false;
                    btnWriteLine9.Visible = false;
                    btnWriteLine10.Visible = false;
                    btnWriteLine11.Visible = false;
                    btnWriteLine12.Visible = false;
                    btnWriteLine13.Visible = false;
                    btnWriteLine14.Visible = false;
                    btnWriteLine15.Visible = false;
                    break;
                default:
                    chkFlexWire.Visible = false;
                    break;
            }

            //			Thread t = new Thread(new ThreadStart(KeepAlive));
            //			t.IsBackground = true;
            //			t.Name = "UCB KeepAlive Thread";
            //			t.Priority = ThreadPriority.Lowest;
            //			t.Start();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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

        

        #region Thread Routines

        private void KeepAlive()
        {
            while (true)
            {
                if (!ucb_busy)
                {
                    UCB.KeepAlive(hw);
                    Thread.Sleep(4000);
                }
                else Thread.Sleep(1000);
            }
        }

        private void SendCommand()
        {
            ucb_busy = true;
            UCB.SendCommand(hw, cmd);
            ucb_busy = false;
            ControlsEnabled(true);
        }

        private void WriteReg()
        {
            ucb_busy = true;
            UCB.WriteReg(hw, address, val);
            ucb_busy = false;
            ControlsEnabled(true);
        }

        private void WriteAll()
        {
            btnWriteLine0_Click(this, EventArgs.Empty);
            while (!btnEnable.Enabled)
                Thread.Sleep(250);

            btnWriteLine1_Click(this, EventArgs.Empty);
            while (!btnEnable.Enabled)
                Thread.Sleep(250);

            btnWriteLine2_Click(this, EventArgs.Empty);
            while (!btnEnable.Enabled)
                Thread.Sleep(250);

            btnWriteLine3_Click(this, EventArgs.Empty);
            while (!btnEnable.Enabled)
                Thread.Sleep(250);

            btnWriteLine4_Click(this, EventArgs.Empty);
            while (!btnEnable.Enabled)
                Thread.Sleep(250);

            btnWriteLine5_Click(this, EventArgs.Empty);
            while (!btnEnable.Enabled)
                Thread.Sleep(250);

            btnWriteLine6_Click(this, EventArgs.Empty);
            while (!btnEnable.Enabled)
                Thread.Sleep(250);

            btnWriteLine7_Click(this, EventArgs.Empty);
            while (!btnEnable.Enabled)
                Thread.Sleep(250);

            btnWriteLine8_Click(this, EventArgs.Empty);
            while (!btnEnable.Enabled)
                Thread.Sleep(250);

            btnWriteLine9_Click(this, EventArgs.Empty);
            while (!btnEnable.Enabled)
                Thread.Sleep(250);

            btnWriteLine10_Click(this, EventArgs.Empty);
            while (!btnEnable.Enabled)
                Thread.Sleep(250);

            btnWriteLine11_Click(this, EventArgs.Empty);
            while (!btnEnable.Enabled)
                Thread.Sleep(250);

            btnWriteLine12_Click(this, EventArgs.Empty);
            while (!btnEnable.Enabled)
                Thread.Sleep(250);

            btnWriteLine13_Click(this, EventArgs.Empty);
            while (!btnEnable.Enabled)
                Thread.Sleep(250);

            btnWriteLine14_Click(this, EventArgs.Empty);
            while (!btnEnable.Enabled)
                Thread.Sleep(250);

            btnWriteLine15_Click(this, EventArgs.Empty);
            while (!btnEnable.Enabled)
                Thread.Sleep(250);

            MessageBox.Show("Writing to UCB is complete.",
                "Writing Complete",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void Resume()
        {
            ucb_busy = true;
            UCB.SendCommand(hw, UCB.CMD.RESUME);
            ucb_busy = false;
            ControlsEnabled(true);
        }

        private void Disable()
        {
            ucb_busy = true;
            UCB.SendCommand(hw, UCB.CMD.DISABLE_OUTPUTS);
            ucb_busy = false;
            ControlsEnabled(true);
        }

        private void DisableAndClear()
        {
            ucb_busy = true;
            UCB.SendCommand(hw, UCB.CMD.DISABLE_OUTPUTS_AND_CLEAR_MATRIX);
            ucb_busy = false;
            ControlsEnabled(true);
        }

        #endregion

        #region Properties

        private ushort[] line = new ushort[16];
        public ushort GetLine(int index)
        {
            return line[index];
        }

        #endregion

        #region Misc Routines

        private void ControlsEnabled(bool b)
        {
            radLine0.Enabled = b;
            radLine1.Enabled = b;
            radLine2.Enabled = b;
            radLine3.Enabled = b;
            radLine4.Enabled = b;
            radLine5.Enabled = b;
            radLine6.Enabled = b;
            radLine7.Enabled = b;
            radLine8.Enabled = b;
            radLine9.Enabled = b;
            radLine10.Enabled = b;
            radLine11.Enabled = b;
            radLine12.Enabled = b;
            radLine13.Enabled = b;
            radLine14.Enabled = b;
            radLine15.Enabled = b;
            btnEnable.Enabled = b;
            btnDisable.Enabled = b;
            btnDisableClear.Enabled = b;
            btnWriteAll.Enabled = b;
            btnWriteLine0.Enabled = b;
            btnWriteLine1.Enabled = b;
            btnWriteLine2.Enabled = b;
            btnWriteLine3.Enabled = b;
            btnWriteLine4.Enabled = b;
            btnWriteLine5.Enabled = b;
            btnWriteLine6.Enabled = b;
            btnWriteLine7.Enabled = b;
            btnWriteLine8.Enabled = b;
            btnWriteLine9.Enabled = b;
            btnWriteLine10.Enabled = b;
            btnWriteLine11.Enabled = b;
            btnWriteLine12.Enabled = b;
            btnWriteLine13.Enabled = b;
            btnWriteLine14.Enabled = b;
            btnWriteLine15.Enabled = b;
            btnSetDelay.Enabled = b;
        }

        #endregion

        #region Event Handlers

        private void label_DoubleClick(object sender, System.EventArgs e)
        {
            string s = InputBox.Show("New Label", "Enter a new Label and press OK.", "<new Label>");
            if (s == "" || s == null) return;
            ((LabelTS)sender).Text = s;
        }

        private void radLine_CheckedChanged(object sender, System.EventArgs e)
        {
            switch (console.CurrentModel)
            {
                case Model.SDR1000:
                    if (((RadioButtonTS)sender).Checked)
                    {
                        string temp = ((RadioButtonTS)sender).Name;
                        byte val = byte.Parse(temp.Substring(7));
                        hw.X2 = (byte)((hw.X2 & 0xF0) | val);
                    }
                    break;
            }
        }

        private void UCBForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
            Common.SaveForm(this, "UCB");
        }

        private void btnSetDelay_Click(object sender, System.EventArgs e)
        {
            switch (comboDelay.Text)
            {
                case "80ms":
                    cmd = UCB.CMD.DELAY_80;
                    break;
                case "40ms":
                    cmd = UCB.CMD.DELAY_40;
                    break;
                case "20ms":
                    cmd = UCB.CMD.DELAY_20;
                    break;
                case "10ms":
                    cmd = UCB.CMD.DELAY_10;
                    break;
                case "5ms":
                    cmd = UCB.CMD.DELAY_5;
                    break;
                case "2ms":
                    cmd = UCB.CMD.DELAY_2;
                    break;
                default:
                    cmd = UCB.CMD.DELAY_80;
                    break;
            }

            ControlsEnabled(false);

            Thread t = new Thread(new ThreadStart(SendCommand));
            t.Name = "UCB Control Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.BelowNormal;
            t.Start();
        }

        private void btnEnable_Click(object sender, System.EventArgs e)
        {
            ControlsEnabled(false);

            Thread t = new Thread(new ThreadStart(Resume));
            t.Name = "UCB Control Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.BelowNormal;
            t.Start();
        }

        private void btnDisable_Click(object sender, System.EventArgs e)
        {
            ControlsEnabled(false);

            Thread t = new Thread(new ThreadStart(Disable));
            t.Name = "UCB Control Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.BelowNormal;
            t.Start();
        }

        private void btnDisableClear_Click(object sender, System.EventArgs e)
        {
            DialogResult dr = MessageBox.Show(
                "Are you sure you want to erase the existing data stored in the PIC matrix?",
                "Erase matrix?",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (dr == DialogResult.No)
                return;

            ControlsEnabled(false);

            Thread t = new Thread(new ThreadStart(DisableAndClear));
            t.Name = "UCB Control Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.BelowNormal;
            t.Start();
        }

        private void btnWriteAll_Click(object sender, System.EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(WriteAll));
            t.Name = "UCB Control Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.BelowNormal;
            t.Start();
        }

        private void btnSetupXVTR_Click(object sender, System.EventArgs e)
        {
            if (console.xvtrForm == null)
                console.xvtrForm = new XVTRForm(console);

            console.xvtrForm.Show();
            console.xvtrForm.Focus();
        }

        #endregion

        #region Write Line Handlers

        private void btnWriteLine0_Click(object sender, System.EventArgs e)
        {
            address = 0;
            val = 0;
            if (chkL00R01.Checked) val += 1 << 0;
            if (chkL00R02.Checked) val += 1 << 1;
            if (chkL00R03.Checked) val += 1 << 2;
            if (chkL00R04.Checked) val += 1 << 3;
            if (chkL00R05.Checked) val += 1 << 4;
            if (chkL00R06.Checked) val += 1 << 5;
            if (chkL00R07.Checked) val += 1 << 6;
            if (chkL00R08.Checked) val += 1 << 7;
            if (chkL00R09.Checked) val += 1 << 8;
            if (chkL00R10.Checked) val += 1 << 9;
            if (chkL00R11.Checked) val += 1 << 10;
            if (chkL00R12.Checked) val += 1 << 11;
            if (chkL00R13.Checked) val += 1 << 12;
            if (chkL00R14.Checked) val += 1 << 13;
            if (chkL00R15.Checked) val += 1 << 14;
            if (chkL00R16.Checked) val += 1 << 15;

            ControlsEnabled(false);

            Thread t = new Thread(new ThreadStart(WriteReg));
            t.IsBackground = true;
            t.Name = "UCB WriteReg Thread";
            t.Priority = ThreadPriority.BelowNormal;
            t.Start();
        }

        private void btnWriteLine1_Click(object sender, System.EventArgs e)
        {
            address = 1;
            val = 0;
            if (chkL01R01.Checked) val += 1 << 0;
            if (chkL01R02.Checked) val += 1 << 1;
            if (chkL01R03.Checked) val += 1 << 2;
            if (chkL01R04.Checked) val += 1 << 3;
            if (chkL01R05.Checked) val += 1 << 4;
            if (chkL01R06.Checked) val += 1 << 5;
            if (chkL01R07.Checked) val += 1 << 6;
            if (chkL01R08.Checked) val += 1 << 7;
            if (chkL01R09.Checked) val += 1 << 8;
            if (chkL01R10.Checked) val += 1 << 9;
            if (chkL01R11.Checked) val += 1 << 10;
            if (chkL01R12.Checked) val += 1 << 11;
            if (chkL01R13.Checked) val += 1 << 12;
            if (chkL01R14.Checked) val += 1 << 13;
            if (chkL01R15.Checked) val += 1 << 14;
            if (chkL01R16.Checked) val += 1 << 15;

            line[address] = (ushort)val;

            ControlsEnabled(false);

            Thread t = new Thread(new ThreadStart(WriteReg));
            t.IsBackground = true;
            t.Name = "UCB WriteReg Thread";
            t.Priority = ThreadPriority.BelowNormal;
            t.Start();
        }

        private void btnWriteLine2_Click(object sender, System.EventArgs e)
        {
            address = 2;
            val = 0;
            if (chkL02R01.Checked) val += 1 << 0;
            if (chkL02R02.Checked) val += 1 << 1;
            if (chkL02R03.Checked) val += 1 << 2;
            if (chkL02R04.Checked) val += 1 << 3;
            if (chkL02R05.Checked) val += 1 << 4;
            if (chkL02R06.Checked) val += 1 << 5;
            if (chkL02R07.Checked) val += 1 << 6;
            if (chkL02R08.Checked) val += 1 << 7;
            if (chkL02R09.Checked) val += 1 << 8;
            if (chkL02R10.Checked) val += 1 << 9;
            if (chkL02R11.Checked) val += 1 << 10;
            if (chkL02R12.Checked) val += 1 << 11;
            if (chkL02R13.Checked) val += 1 << 12;
            if (chkL02R14.Checked) val += 1 << 13;
            if (chkL02R15.Checked) val += 1 << 14;
            if (chkL02R16.Checked) val += 1 << 15;

            line[address] = (ushort)val;

            ControlsEnabled(false);

            Thread t = new Thread(new ThreadStart(WriteReg));
            t.IsBackground = true;
            t.Name = "UCB WriteReg Thread";
            t.Priority = ThreadPriority.BelowNormal;
            t.Start();
        }

        private void btnWriteLine3_Click(object sender, System.EventArgs e)
        {
            address = 3;
            val = 0;
            if (chkL03R01.Checked) val += 1 << 0;
            if (chkL03R02.Checked) val += 1 << 1;
            if (chkL03R03.Checked) val += 1 << 2;
            if (chkL03R04.Checked) val += 1 << 3;
            if (chkL03R05.Checked) val += 1 << 4;
            if (chkL03R06.Checked) val += 1 << 5;
            if (chkL03R07.Checked) val += 1 << 6;
            if (chkL03R08.Checked) val += 1 << 7;
            if (chkL03R09.Checked) val += 1 << 8;
            if (chkL03R10.Checked) val += 1 << 9;
            if (chkL03R11.Checked) val += 1 << 10;
            if (chkL03R12.Checked) val += 1 << 11;
            if (chkL03R13.Checked) val += 1 << 12;
            if (chkL03R14.Checked) val += 1 << 13;
            if (chkL03R15.Checked) val += 1 << 14;
            if (chkL03R16.Checked) val += 1 << 15;

            line[address] = (ushort)val;

            ControlsEnabled(false);

            Thread t = new Thread(new ThreadStart(WriteReg));
            t.IsBackground = true;
            t.Name = "UCB WriteReg Thread";
            t.Priority = ThreadPriority.BelowNormal;
            t.Start();
        }

        private void btnWriteLine4_Click(object sender, System.EventArgs e)
        {
            address = 4;
            val = 0;
            if (chkL04R01.Checked) val += 1 << 0;
            if (chkL04R02.Checked) val += 1 << 1;
            if (chkL04R03.Checked) val += 1 << 2;
            if (chkL04R04.Checked) val += 1 << 3;
            if (chkL04R05.Checked) val += 1 << 4;
            if (chkL04R06.Checked) val += 1 << 5;
            if (chkL04R07.Checked) val += 1 << 6;
            if (chkL04R08.Checked) val += 1 << 7;
            if (chkL04R09.Checked) val += 1 << 8;
            if (chkL04R10.Checked) val += 1 << 9;
            if (chkL04R11.Checked) val += 1 << 10;
            if (chkL04R12.Checked) val += 1 << 11;
            if (chkL04R13.Checked) val += 1 << 12;
            if (chkL04R14.Checked) val += 1 << 13;
            if (chkL04R15.Checked) val += 1 << 14;
            if (chkL04R16.Checked) val += 1 << 15;

            line[address] = (ushort)val;

            ControlsEnabled(false);

            Thread t = new Thread(new ThreadStart(WriteReg));
            t.IsBackground = true;
            t.Name = "UCB WriteReg Thread";
            t.Priority = ThreadPriority.BelowNormal;
            t.Start();
        }

        private void btnWriteLine5_Click(object sender, System.EventArgs e)
        {
            address = 5;
            val = 0;
            if (chkL05R01.Checked) val += 1 << 0;
            if (chkL05R02.Checked) val += 1 << 1;
            if (chkL05R03.Checked) val += 1 << 2;
            if (chkL05R04.Checked) val += 1 << 3;
            if (chkL05R05.Checked) val += 1 << 4;
            if (chkL05R06.Checked) val += 1 << 5;
            if (chkL05R07.Checked) val += 1 << 6;
            if (chkL05R08.Checked) val += 1 << 7;
            if (chkL05R09.Checked) val += 1 << 8;
            if (chkL05R10.Checked) val += 1 << 9;
            if (chkL05R11.Checked) val += 1 << 10;
            if (chkL05R12.Checked) val += 1 << 11;
            if (chkL05R13.Checked) val += 1 << 12;
            if (chkL05R14.Checked) val += 1 << 13;
            if (chkL05R15.Checked) val += 1 << 14;
            if (chkL05R16.Checked) val += 1 << 15;

            line[address] = (ushort)val;

            ControlsEnabled(false);

            Thread t = new Thread(new ThreadStart(WriteReg));
            t.IsBackground = true;
            t.Name = "UCB WriteReg Thread";
            t.Priority = ThreadPriority.BelowNormal;
            t.Start();
        }

        private void btnWriteLine6_Click(object sender, System.EventArgs e)
        {
            address = 6;
            val = 0;
            if (chkL06R01.Checked) val += 1 << 0;
            if (chkL06R02.Checked) val += 1 << 1;
            if (chkL06R03.Checked) val += 1 << 2;
            if (chkL06R04.Checked) val += 1 << 3;
            if (chkL06R05.Checked) val += 1 << 4;
            if (chkL06R06.Checked) val += 1 << 5;
            if (chkL06R07.Checked) val += 1 << 6;
            if (chkL06R08.Checked) val += 1 << 7;
            if (chkL06R09.Checked) val += 1 << 8;
            if (chkL06R10.Checked) val += 1 << 9;
            if (chkL06R11.Checked) val += 1 << 10;
            if (chkL06R12.Checked) val += 1 << 11;
            if (chkL06R13.Checked) val += 1 << 12;
            if (chkL06R14.Checked) val += 1 << 13;
            if (chkL06R15.Checked) val += 1 << 14;
            if (chkL06R16.Checked) val += 1 << 15;

            line[address] = (ushort)val;

            ControlsEnabled(false);

            Thread t = new Thread(new ThreadStart(WriteReg));
            t.IsBackground = true;
            t.Name = "UCB WriteReg Thread";
            t.Priority = ThreadPriority.BelowNormal;
            t.Start();
        }

        private void btnWriteLine7_Click(object sender, System.EventArgs e)
        {
            address = 7;
            val = 0;
            if (chkL07R01.Checked) val += 1 << 0;
            if (chkL07R02.Checked) val += 1 << 1;
            if (chkL07R03.Checked) val += 1 << 2;
            if (chkL07R04.Checked) val += 1 << 3;
            if (chkL07R05.Checked) val += 1 << 4;
            if (chkL07R06.Checked) val += 1 << 5;
            if (chkL07R07.Checked) val += 1 << 6;
            if (chkL07R08.Checked) val += 1 << 7;
            if (chkL07R09.Checked) val += 1 << 8;
            if (chkL07R10.Checked) val += 1 << 9;
            if (chkL07R11.Checked) val += 1 << 10;
            if (chkL07R12.Checked) val += 1 << 11;
            if (chkL07R13.Checked) val += 1 << 12;
            if (chkL07R14.Checked) val += 1 << 13;
            if (chkL07R15.Checked) val += 1 << 14;
            if (chkL07R16.Checked) val += 1 << 15;

            line[address] = (ushort)val;

            ControlsEnabled(false);

            Thread t = new Thread(new ThreadStart(WriteReg));
            t.IsBackground = true;
            t.Name = "UCB WriteReg Thread";
            t.Priority = ThreadPriority.BelowNormal;
            t.Start();
        }

        private void btnWriteLine8_Click(object sender, System.EventArgs e)
        {
            address = 8;
            val = 0;
            if (chkL08R01.Checked) val += 1 << 0;
            if (chkL08R02.Checked) val += 1 << 1;
            if (chkL08R03.Checked) val += 1 << 2;
            if (chkL08R04.Checked) val += 1 << 3;
            if (chkL08R05.Checked) val += 1 << 4;
            if (chkL08R06.Checked) val += 1 << 5;
            if (chkL08R07.Checked) val += 1 << 6;
            if (chkL08R08.Checked) val += 1 << 7;
            if (chkL08R09.Checked) val += 1 << 8;
            if (chkL08R10.Checked) val += 1 << 9;
            if (chkL08R11.Checked) val += 1 << 10;
            if (chkL08R12.Checked) val += 1 << 11;
            if (chkL08R13.Checked) val += 1 << 12;
            if (chkL08R14.Checked) val += 1 << 13;
            if (chkL08R15.Checked) val += 1 << 14;
            if (chkL08R16.Checked) val += 1 << 15;

            line[address] = (ushort)val;

            ControlsEnabled(false);

            Thread t = new Thread(new ThreadStart(WriteReg));
            t.IsBackground = true;
            t.Name = "UCB WriteReg Thread";
            t.Priority = ThreadPriority.BelowNormal;
            t.Start();
        }

        private void btnWriteLine9_Click(object sender, System.EventArgs e)
        {
            address = 9;
            val = 0;
            if (chkL09R01.Checked) val += 1 << 0;
            if (chkL09R02.Checked) val += 1 << 1;
            if (chkL09R03.Checked) val += 1 << 2;
            if (chkL09R04.Checked) val += 1 << 3;
            if (chkL09R05.Checked) val += 1 << 4;
            if (chkL09R06.Checked) val += 1 << 5;
            if (chkL09R07.Checked) val += 1 << 6;
            if (chkL09R08.Checked) val += 1 << 7;
            if (chkL09R09.Checked) val += 1 << 8;
            if (chkL09R10.Checked) val += 1 << 9;
            if (chkL09R11.Checked) val += 1 << 10;
            if (chkL09R12.Checked) val += 1 << 11;
            if (chkL09R13.Checked) val += 1 << 12;
            if (chkL09R14.Checked) val += 1 << 13;
            if (chkL09R15.Checked) val += 1 << 14;
            if (chkL09R16.Checked) val += 1 << 15;

            line[address] = (ushort)val;

            ControlsEnabled(false);

            Thread t = new Thread(new ThreadStart(WriteReg));
            t.IsBackground = true;
            t.Name = "UCB WriteReg Thread";
            t.Priority = ThreadPriority.BelowNormal;
            t.Start();
        }

        private void btnWriteLine10_Click(object sender, System.EventArgs e)
        {
            address = 10;
            val = 0;
            if (chkL10R01.Checked) val += 1 << 0;
            if (chkL10R02.Checked) val += 1 << 1;
            if (chkL10R03.Checked) val += 1 << 2;
            if (chkL10R04.Checked) val += 1 << 3;
            if (chkL10R05.Checked) val += 1 << 4;
            if (chkL10R06.Checked) val += 1 << 5;
            if (chkL10R07.Checked) val += 1 << 6;
            if (chkL10R08.Checked) val += 1 << 7;
            if (chkL10R09.Checked) val += 1 << 8;
            if (chkL10R10.Checked) val += 1 << 9;
            if (chkL10R11.Checked) val += 1 << 10;
            if (chkL10R12.Checked) val += 1 << 11;
            if (chkL10R13.Checked) val += 1 << 12;
            if (chkL10R14.Checked) val += 1 << 13;
            if (chkL10R15.Checked) val += 1 << 14;
            if (chkL10R16.Checked) val += 1 << 15;

            line[address] = (ushort)val;

            ControlsEnabled(false);

            Thread t = new Thread(new ThreadStart(WriteReg));
            t.IsBackground = true;
            t.Name = "UCB WriteReg Thread";
            t.Priority = ThreadPriority.BelowNormal;
            t.Start();
        }

        private void btnWriteLine11_Click(object sender, System.EventArgs e)
        {
            address = 11;
            val = 0;
            if (chkL11R01.Checked) val += 1 << 0;
            if (chkL11R02.Checked) val += 1 << 1;
            if (chkL11R03.Checked) val += 1 << 2;
            if (chkL11R04.Checked) val += 1 << 3;
            if (chkL11R05.Checked) val += 1 << 4;
            if (chkL11R06.Checked) val += 1 << 5;
            if (chkL11R07.Checked) val += 1 << 6;
            if (chkL11R08.Checked) val += 1 << 7;
            if (chkL11R09.Checked) val += 1 << 8;
            if (chkL11R10.Checked) val += 1 << 9;
            if (chkL11R11.Checked) val += 1 << 10;
            if (chkL11R12.Checked) val += 1 << 11;
            if (chkL11R13.Checked) val += 1 << 12;
            if (chkL11R14.Checked) val += 1 << 13;
            if (chkL11R15.Checked) val += 1 << 14;
            if (chkL11R16.Checked) val += 1 << 15;

            line[address] = (ushort)val;

            ControlsEnabled(false);

            Thread t = new Thread(new ThreadStart(WriteReg));
            t.IsBackground = true;
            t.Name = "UCB WriteReg Thread";
            t.Priority = ThreadPriority.BelowNormal;
            t.Start();
        }

        private void btnWriteLine12_Click(object sender, System.EventArgs e)
        {
            address = 12;
            val = 0;
            if (chkL12R01.Checked) val += 1 << 0;
            if (chkL12R02.Checked) val += 1 << 1;
            if (chkL12R03.Checked) val += 1 << 2;
            if (chkL12R04.Checked) val += 1 << 3;
            if (chkL12R05.Checked) val += 1 << 4;
            if (chkL12R06.Checked) val += 1 << 5;
            if (chkL12R07.Checked) val += 1 << 6;
            if (chkL12R08.Checked) val += 1 << 7;
            if (chkL12R09.Checked) val += 1 << 8;
            if (chkL12R10.Checked) val += 1 << 9;
            if (chkL12R11.Checked) val += 1 << 10;
            if (chkL12R12.Checked) val += 1 << 11;
            if (chkL12R13.Checked) val += 1 << 12;
            if (chkL12R14.Checked) val += 1 << 13;
            if (chkL12R15.Checked) val += 1 << 14;
            if (chkL12R16.Checked) val += 1 << 15;

            line[address] = (ushort)val;

            ControlsEnabled(false);

            Thread t = new Thread(new ThreadStart(WriteReg));
            t.IsBackground = true;
            t.Name = "UCB WriteReg Thread";
            t.Priority = ThreadPriority.BelowNormal;
            t.Start();
        }

        private void btnWriteLine13_Click(object sender, System.EventArgs e)
        {
            address = 13;
            val = 0;
            if (chkL13R01.Checked) val += 1 << 0;
            if (chkL13R02.Checked) val += 1 << 1;
            if (chkL13R03.Checked) val += 1 << 2;
            if (chkL13R04.Checked) val += 1 << 3;
            if (chkL13R05.Checked) val += 1 << 4;
            if (chkL13R06.Checked) val += 1 << 5;
            if (chkL13R07.Checked) val += 1 << 6;
            if (chkL13R08.Checked) val += 1 << 7;
            if (chkL13R09.Checked) val += 1 << 8;
            if (chkL13R10.Checked) val += 1 << 9;
            if (chkL13R11.Checked) val += 1 << 10;
            if (chkL13R12.Checked) val += 1 << 11;
            if (chkL13R13.Checked) val += 1 << 12;
            if (chkL13R14.Checked) val += 1 << 13;
            if (chkL13R15.Checked) val += 1 << 14;
            if (chkL13R16.Checked) val += 1 << 15;

            line[address] = (ushort)val;

            ControlsEnabled(false);

            Thread t = new Thread(new ThreadStart(WriteReg));
            t.IsBackground = true;
            t.Name = "UCB WriteReg Thread";
            t.Priority = ThreadPriority.BelowNormal;
            t.Start();
        }

        private void btnWriteLine14_Click(object sender, System.EventArgs e)
        {
            address = 14;
            val = 0;
            if (chkL14R01.Checked) val += 1 << 0;
            if (chkL14R02.Checked) val += 1 << 1;
            if (chkL14R03.Checked) val += 1 << 2;
            if (chkL14R04.Checked) val += 1 << 3;
            if (chkL14R05.Checked) val += 1 << 4;
            if (chkL14R06.Checked) val += 1 << 5;
            if (chkL14R07.Checked) val += 1 << 6;
            if (chkL14R08.Checked) val += 1 << 7;
            if (chkL14R09.Checked) val += 1 << 8;
            if (chkL14R10.Checked) val += 1 << 9;
            if (chkL14R11.Checked) val += 1 << 10;
            if (chkL14R12.Checked) val += 1 << 11;
            if (chkL14R13.Checked) val += 1 << 12;
            if (chkL14R14.Checked) val += 1 << 13;
            if (chkL14R15.Checked) val += 1 << 14;
            if (chkL14R16.Checked) val += 1 << 15;

            line[address] = (ushort)val;

            ControlsEnabled(false);

            Thread t = new Thread(new ThreadStart(WriteReg));
            t.IsBackground = true;
            t.Name = "UCB WriteReg Thread";
            t.Priority = ThreadPriority.BelowNormal;
            t.Start();
        }

        private void btnWriteLine15_Click(object sender, System.EventArgs e)
        {
            address = 15;
            val = 0;
            if (chkL15R01.Checked) val += 1 << 0;
            if (chkL15R02.Checked) val += 1 << 1;
            if (chkL15R03.Checked) val += 1 << 2;
            if (chkL15R04.Checked) val += 1 << 3;
            if (chkL15R05.Checked) val += 1 << 4;
            if (chkL15R06.Checked) val += 1 << 5;
            if (chkL15R07.Checked) val += 1 << 6;
            if (chkL15R08.Checked) val += 1 << 7;
            if (chkL15R09.Checked) val += 1 << 8;
            if (chkL15R10.Checked) val += 1 << 9;
            if (chkL15R11.Checked) val += 1 << 10;
            if (chkL15R12.Checked) val += 1 << 11;
            if (chkL15R13.Checked) val += 1 << 12;
            if (chkL15R14.Checked) val += 1 << 13;
            if (chkL15R15.Checked) val += 1 << 14;
            if (chkL15R16.Checked) val += 1 << 15;

            line[address] = (ushort)val;

            ControlsEnabled(false);

            Thread t = new Thread(new ThreadStart(WriteReg));
            t.IsBackground = true;
            t.Name = "UCB WriteReg Thread";
            t.Priority = ThreadPriority.BelowNormal;
            t.Start();
        }

        #endregion

        private void chkRelay_CheckedChanged(object sender, System.EventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            if (chk == null) return;

            int index = int.Parse(chk.Name.Substring(4, 2)); // get the name of the item click on in the form  (chkL02R06: index = 2, relay = 6)
            int relay = int.Parse(chk.Name.Substring(7, 2));

            if (chk.Checked)
                line[index] |= (ushort)(1 << (relay - 1)); // add it to the line[2] = line[2] | 0001 0000 = 0x10 (for relay 6) relay1 has no shift
            else
                line[index] &= (ushort)(0xFFFF - (1 << (relay - 1))); // remove it from the line[index]

            //Debug.WriteLine("line[" + index + "]: " + line[index]);
        }

        private void chkFlexWire_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkFlexWire.Checked)
            {
                chkFlexWire.BackColor = console.ButtonSelectedColor;
            }
            else
            {
                chkFlexWire.BackColor = SystemColors.Control;
            }

            console.FlexWireUCB = chkFlexWire.Checked;
        }
    }
}
