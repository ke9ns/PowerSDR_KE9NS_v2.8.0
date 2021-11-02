//=================================================================
// FLEX5000LLHWForm.cs
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
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace PowerSDR
{
    public partial class FLEX5000LLHWForm : System.Windows.Forms.Form
    {
        #region Variable Declaration

        Console console;
        bool trx_ok = true;
        bool pa_ok = true;
        bool rfio_ok = true;
        bool rx2_ok = true;

       
        #endregion

        #region Constructor and Destructor

        public FLEX5000LLHWForm() // FLEX-5000 Low Level Hardware Control
        {
            InitializeComponent();
            //if(!FWC.Open1394Driver())
            //    MessageBox.Show("Error opening driver.");

            trx_ok = FWCEEPROM.TRXOK;
            pa_ok = FWCEEPROM.PAOK;
            rfio_ok = FWCEEPROM.RFIOOK;
            rx2_ok = FWCEEPROM.RX2OK;

            Init();

            this.TopMost = true; // ke9ns .174
        }

        public FLEX5000LLHWForm(Console c)
        {
            InitializeComponent();
            console = c;
            this.TopMost = true; // ke9ns .174
            trx_ok = FWCEEPROM.TRXOK;
            pa_ok = FWCEEPROM.PAOK;
            rfio_ok = FWCEEPROM.RFIOOK;
            rx2_ok = FWCEEPROM.RX2OK;

            grpPAPot.Enabled = pa_ok;

            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                    break;
                case Model.FLEX3000:
                    comboPIOChip.Items.Clear();
                    comboPIOChip.Items.Add("TRX IC27");
                    comboPIOChip.Items.Add("TRX IC37");
                    comboPIOChip.Items.Add("PA IC13");
                    comboPIOChip.Items.Add("PA IC16");
                    break;
            }

            Init();
            Common.RestoreForm(this, "FLEX5000LLHWForm", false);
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

      
        #region Main

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.Run(new FLEX5000LLHWForm());
        }

        #endregion

        #region Misc Routines

        public void Init()
        {
            for (int i = 0; i <= 0x5A; i++)
            {
                if ((i >= 1 && i <= 3) ||
                    (i >= 0x0E && i <= 0x33) ||
                    (i >= 0x37 && i <= 0x3C) ||
                    (i >= 0x42 && i <= 0x44) ||
                    (i >= 0x46 && i <= 0x49) ||
                    (i >= 0x54 && i <= 0x57) ||
                    i == 0x59)
                {

                }
                else
                {
                    comboClockGenReg.Items.Add(String.Format("{0:X2}", i));
                }
            }

            for (int i = 0; i <= 0x18; i++)
                comboDDSReg.Items.Add(String.Format("{0:X2}", i));

            for (int i = 1; i <= 0x1B; i++)
                comboCodecReg.Items.Add(String.Format("{0:X2}", i));

            comboClockGenReg.SelectedIndex = 0;
            comboDDSChan.SelectedIndex = 0;
            comboDDSReg.SelectedIndex = 0;
            comboPIOChip.SelectedIndex = 0;
            comboPIOReg.SelectedIndex = 2;
            comboTRXPotIndex.SelectedIndex = 0;
            comboPAPotIndex.SelectedIndex = 0;
            comboMuxChan.SelectedIndex = 1;
            comboCodecReg.SelectedIndex = 0;
        }

        #endregion

        #region Event Handlers

        private void btnClockGenRead_Click(object sender, System.EventArgs e)
        {
            if (!trx_ok) return;
            if (comboClockGenReg.Text == "") return;
            txtClockGenReadVal.BackColor = Color.Red;
            Application.DoEvents();
            int reg = int.Parse(comboClockGenReg.Text, System.Globalization.NumberStyles.HexNumber);
            int data = 0;
            if (FWC.ReadClockReg(reg, out data) == 0)
            {
                MessageBox.Show("Error in ReadClockReg.");
            }

            txtClockGenReadVal.BackColor = SystemColors.Control;
            txtClockGenReadVal.Text = String.Format("{0:X2}", data);
        }

        private void comboClockGenReg_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (comboClockGenReg.Text != "")
                btnClockGenRead_Click(this, EventArgs.Empty);
        }

        private void btnClockGenWrite_Click(object sender, System.EventArgs e)
        {
            if (!trx_ok) return;
            if (comboClockGenReg.Text == "") return;
            if (txtClockGenWriteVal.Text == "") return;
            txtClockGenWriteVal.BackColor = Color.Red;
            Application.DoEvents();
            int reg = int.Parse(comboClockGenReg.Text, System.Globalization.NumberStyles.HexNumber);
            int val = int.Parse(txtClockGenWriteVal.Text, System.Globalization.NumberStyles.HexNumber);

            if (FWC.WriteClockReg(reg, val) == 0)
                MessageBox.Show("Error in WriteClockReg.");

            txtClockGenWriteVal.BackColor = SystemColors.Window;
            btnClockGenRead_Click(this, EventArgs.Empty);
        }

        private void comboDDSReg_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (comboDDSReg.Text != "" && comboDDSChan.Text != "")
                btnDDSRead_Click(this, EventArgs.Empty);
        }

        private void btnDDSRead_Click(object sender, System.EventArgs e)
        {
            if (!trx_ok) return;
            if (comboDDSReg.Text == "") return;
            if (comboDDSChan.Text == "") return;
            txtDDSReadVal.BackColor = Color.Red;
            Application.DoEvents();
            int chan = int.Parse(comboDDSChan.Text);
            int reg = int.Parse(comboDDSReg.Text, System.Globalization.NumberStyles.HexNumber);
            uint data = 0;
            if (!chkRX2DDS.Checked)
            {
                if (FWC.ReadTRXDDSReg(chan, reg, out data) == 0)
                {
                    MessageBox.Show("Error in ReadTRXDDSReg.");
                }
            }
            else
            {
                if (FWC.ReadRX2DDSReg(chan, reg, out data) == 0)
                {
                    MessageBox.Show("Error in ReadRX2DDSReg.");
                }
            }

            txtDDSReadVal.BackColor = SystemColors.Control;
            txtDDSReadVal.Text = String.Format("{0:X8}", data);
        }

        private void btnDDSWrite_Click(object sender, System.EventArgs e)
        {
            if (!trx_ok) return;
            if (comboDDSReg.Text == "") return;
            if (txtDDSWrite.Text == "") return;
            txtDDSWrite.BackColor = Color.Red;
            Application.DoEvents();
            int chan = int.Parse(comboDDSChan.Text);
            int reg = int.Parse(comboDDSReg.Text, System.Globalization.NumberStyles.HexNumber);
            uint val = uint.Parse(txtDDSWrite.Text, System.Globalization.NumberStyles.HexNumber);

            if (!chkRX2DDS.Checked)
            {
                if (FWC.WriteTRXDDSReg(chan, reg, val) == 0)
                    MessageBox.Show("Error in WriteTRXDDSReg.");
            }
            else
            {
                if (FWC.WriteRX2DDSReg(chan, reg, val) == 0)
                    MessageBox.Show("Error in WriteRX2DDSReg.");
            }

            txtDDSWrite.BackColor = SystemColors.Window;
            btnDDSRead_Click(this, EventArgs.Empty);
        }

        private void comboDDSChan_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (comboDDSReg.Text != "" && comboDDSChan.Text != "")
                btnDDSRead_Click(this, EventArgs.Empty);
        }

        private void btnPIORead_Click(object sender, System.EventArgs e)
        {
            if (comboPIOReg.Text == "") return;
            if (comboPIOChip.Text == "") return;
            txtPIORead.BackColor = Color.Red;
            Application.DoEvents();
            int chip = comboPIOChip.SelectedIndex;
            int reg = comboPIOReg.SelectedIndex;
            uint data = 0;
            if (FWC.ReadPIOReg(chip, reg, out data) == 0)
            {
                MessageBox.Show("Error in ReadPIOReg.");
            }

            txtPIORead.BackColor = SystemColors.Control;
            txtPIORead.Text = String.Format("{0:X4}", data);
        }

        private void comboPIOReg_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (comboPIOReg.Text != "" && comboPIOChip.Text != "")
                btnPIORead_Click(this, EventArgs.Empty);
        }

        private void comboPIOChip_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (comboPIOReg.Text != "" && comboPIOChip.Text != "")
                btnPIORead_Click(this, EventArgs.Empty);
        }

        private void btnPIOWrite_Click(object sender, System.EventArgs e)
        {
            if (comboPIOReg.Text == "") return;
            if (comboPIOChip.Text == "") return;
            if (txtPIOWrite.Text == "") return;
            txtPIOWrite.BackColor = Color.Red;
            Application.DoEvents();
            int chip = comboPIOChip.SelectedIndex;
            int reg = comboPIOReg.SelectedIndex;
            int val = int.Parse(txtPIOWrite.Text, System.Globalization.NumberStyles.HexNumber);

            if (FWC.WritePIOReg(chip, reg, val) == 0)
                MessageBox.Show("Error in WriteDDSReg.");

            txtPIOWrite.BackColor = SystemColors.Window;
            btnPIORead_Click(this, EventArgs.Empty);
        }


        //=====================================================================================
        // ke9ns read offset hex value, get eeprom value from radio and display it.
        private void btnEEPROMRead_Click(object sender, System.EventArgs e)
        {
            if (txtEEPROMOffset.Text == "") return;
            txtEEPROMRead.BackColor = Color.Red;
            Application.DoEvents();
            uint offset = uint.Parse(txtEEPROMOffset.Text, System.Globalization.NumberStyles.HexNumber);

            byte data;
            if (!chkRX2EEPROM.Checked)
            {
                if (FWC.ReadTRXEEPROMByte(offset, out data) == 0)
                    MessageBox.Show("Error in ReadTRXEEPROM.");
            }
            else
            {
                if (FWC.ReadRX2EEPROMByte(offset, out data) == 0)
                    MessageBox.Show("Error in ReadRX2EEPROM.");
            }

            txtEEPROMRead.BackColor = SystemColors.Control;
            txtEEPROMRead.Text = String.Format("{0:X4}", data);
        }



        //=====================================================================================
        // ke9ns ADD  get all of eeprom and save it in a file called EEPROMDATA.txt in the database folder
        private void btnEEPROMRead1_Click(object sender, System.EventArgs e)
        {
            // if (txtEEPROMOffset.Text == "") return;
            txtEEPROMRead.BackColor = Color.Red;
            Application.DoEvents();
            //  uint offset = uint.Parse(txtEEPROMOffset.Text, System.Globalization.NumberStyles.HexNumber);


            Debug.WriteLine("TRYING TO OPEN EEPROM TXT FILE TO WRITE TO ");


            string file_name2 = console.AppDataPath + "EEEPROMDATA.txt"; // save data for my mods

            FileStream stream2 = new FileStream(file_name2, FileMode.Create); // open   file
            BinaryWriter writer2 = new BinaryWriter(stream2);

            Debug.WriteLine("OPENED EEPROM TXT FILE TO WRITE TO ");
            uint offset;

            byte data;
            string datastring;
            string final;
            string offsetstring;

            for (offset = 0; offset < 3000; offset++)
            {

                Debug.Write("   Reading offset-> " + offset);

                if (!chkRX2EEPROM.Checked)
                {
                    if (FWC.ReadTRXEEPROMByte(offset, out data) == 0)
                        MessageBox.Show("Error in ReadTRXEEPROM.");
                }
                else
                {
                    if (FWC.ReadRX2EEPROMByte(offset, out data) == 0)
                        MessageBox.Show("Error in ReadRX2EEPROM.");
                }

                Debug.WriteLine("   Data-> " + data);

                txtEEPROMRead.BackColor = SystemColors.Control;

                datastring = String.Format("{0:X4}", data);

                txtEEPROMRead.Text = datastring;

                offsetstring = String.Format("{0:X4}", offset);

                txtEEPROMOffset.Text = offsetstring;

                final = offsetstring + " , " + datastring + "\n";

                writer2.Write(final);


            } // for offset loop


            writer2.Close();    // close  file
            stream2.Close();   // close stream


        } // btnEEPROMRead1_Click


        //==========================================================================================
        // ke9ns add change 3000 or 5000 radio to Extended
        private void buttonTS1_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show(new Form { TopMost = true }, "Warning: You must have Authorization as a MARS,CAPS, or SHARES Licensed Operator.\n",
                                             "Do you have authorization?",
                           MessageBoxButtons.YesNo,
                           MessageBoxIcon.Question);

            if (dr == DialogResult.Yes)
            {

                if (FWC.WriteTRXEEPROMByte(0x0034, 0x2D) == 0) MessageBox.Show("Error in WriteEEPROM 34");
                txtEEPROMWrite.BackColor = SystemColors.Window;
                btnEEPROMRead_Click(this, EventArgs.Empty);
                if (FWC.WriteTRXEEPROMByte(0x0035, 0x05) == 0) MessageBox.Show("Error in WriteEEPROM 35");
                txtEEPROMWrite.BackColor = SystemColors.Window;
                btnEEPROMRead_Click(this, EventArgs.Empty);
                if (FWC.WriteTRXEEPROMByte(0x0036, 0x00) == 0) MessageBox.Show("Error in WriteEEPROM 36");
                txtEEPROMWrite.BackColor = SystemColors.Window;
                btnEEPROMRead_Click(this, EventArgs.Empty);
                if (FWC.WriteTRXEEPROMByte(0x0037, 0x00) == 0) MessageBox.Show("Error in WriteEEPROM 37");
                txtEEPROMWrite.BackColor = SystemColors.Window;
                btnEEPROMRead_Click(this, EventArgs.Empty);
                if (FWC.WriteTRXEEPROMByte(0x0038, 0x0D) == 0) MessageBox.Show("Error in WriteEEPROM 38");
                txtEEPROMWrite.BackColor = SystemColors.Window;
                btnEEPROMRead_Click(this, EventArgs.Empty);
                if (FWC.WriteTRXEEPROMByte(0x0039, 0x0B) == 0) MessageBox.Show("Error in WriteEEPROM 39");
                txtEEPROMWrite.BackColor = SystemColors.Window;
                btnEEPROMRead_Click(this, EventArgs.Empty);
                if (FWC.WriteTRXEEPROMByte(0x003A, 0x01) == 0) MessageBox.Show("Error in WriteEEPROM 3a");
                txtEEPROMWrite.BackColor = SystemColors.Window;
                btnEEPROMRead_Click(this, EventArgs.Empty);
                if (FWC.WriteTRXEEPROMByte(0x003B, 0x00) == 0) MessageBox.Show("Error in WriteEEPROM 3b");
                txtEEPROMWrite.BackColor = SystemColors.Window;
                btnEEPROMRead_Click(this, EventArgs.Empty);
                if (FWC.WriteTRXEEPROMByte(0x003C, 0x78) == 0) MessageBox.Show("Error in WriteEEPROM 3c");
                txtEEPROMWrite.BackColor = SystemColors.Window;
                btnEEPROMRead_Click(this, EventArgs.Empty);

            }




            MessageBox.Show("You must cycle power to the radio", "Cycle Power",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        } // buttonTS1_Click



        private void btnEEPROMWrite_Click(object sender, System.EventArgs e)
        {
            if (txtEEPROMOffset.Text == "") return;
            txtEEPROMWrite.BackColor = Color.Red;
            Application.DoEvents();
            uint offset = uint.Parse(txtEEPROMOffset.Text, System.Globalization.NumberStyles.HexNumber);
            byte val = byte.Parse(txtEEPROMWrite.Text, System.Globalization.NumberStyles.HexNumber);

            if (!chkRX2EEPROM.Checked)
            {
                if (FWC.WriteTRXEEPROMByte(offset, val) == 0)
                    MessageBox.Show("Error in WriteTRXEEPROM");
            }
            else
            {
                if (FWC.WriteRX2EEPROMByte(offset, val) == 0)
                    MessageBox.Show("Error in WriteRX2EEPROM");
            }

            txtEEPROMWrite.BackColor = SystemColors.Window;
            btnEEPROMRead_Click(this, EventArgs.Empty);
        }

        private void btnTRXPotRead_Click(object sender, System.EventArgs e)
        {
            txtTRXPotRead.BackColor = Color.Red;
            Application.DoEvents();
            uint val;
            if (FWC.TRXPotGetRDAC(out val) == 0)
                MessageBox.Show("Error in TRXPotGetRDAC");

            txtTRXPotRead.BackColor = SystemColors.Control;
            txtTRXPotRead.Text = String.Format("{0:X4}", val);
        }

        private void comboTRXPotIndex_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            btnTRXPotRead_Click(this, EventArgs.Empty);
        }

        private void btnTRXPotWrite_Click(object sender, System.EventArgs e)
        {
            if (comboTRXPotIndex.Text == "") return;
            txtTRXPotWrite.BackColor = Color.Red;
            Application.DoEvents();
            int index = comboTRXPotIndex.SelectedIndex;
            int val = int.Parse(txtTRXPotWrite.Text, System.Globalization.NumberStyles.HexNumber);

            if (FWC.TRXPotSetRDAC(index, val) == 0)
                MessageBox.Show("Error in TRXPotSetRDAC");

            txtTRXPotWrite.BackColor = SystemColors.Window;
            btnTRXPotRead_Click(this, EventArgs.Empty);
        }

        private void comboMuxChan_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (FWC.SetMux(comboMuxChan.SelectedIndex) == 0)
                MessageBox.Show("Error in SetMux");
        }

        private void chkGPIO_CheckedChanged(object sender, System.EventArgs e)
        {
            uint val = 0;
            if (chkGPIO1.Checked) val += 1 << 0;
            if (chkGPIO2.Checked) val += 1 << 1;
            if (chkGPIO3.Checked) val += 1 << 2;
            if (chkGPIO4.Checked) val += 1 << 3;
            if (chkGPIO5.Checked) val += 1 << 4;
            if (chkGPIO6.Checked) val += 1 << 5;
            if (chkGPIO7.Checked) val += 1 << 6;
            if (chkGPIO8.Checked) val += 1 << 7;

            if (FWC.GPIOWrite(val) == 0)
                MessageBox.Show("Error in GPIOWrite");

            btnGPIORead_Click(this, EventArgs.Empty);
        }

        private void btnGPIORead_Click(object sender, System.EventArgs e)
        {
            uint val;
            if (FWC.GPIORead(out val) == 0)
                MessageBox.Show("Error in GPIORead");

            txtGPIORead.Text = String.Format("{0:X4}", val);
        }

        private void btnGPIOWriteVal_Click(object sender, System.EventArgs e)
        {
            if (txtGPIOWrite.Text == "") return;
            uint val = uint.Parse(txtGPIOWrite.Text, System.Globalization.NumberStyles.HexNumber);

            if (FWC.GPIOWrite(val) == 0)
                MessageBox.Show("Error in GPIOWrite");

            btnGPIORead_Click(this, EventArgs.Empty);
        }

        private void chkGPIODDR_CheckedChanged(object sender, System.EventArgs e)
        {
            uint val = 0;
            if (chkGPIODDR1.Checked) val += 1 << 0;
            if (chkGPIODDR2.Checked) val += 1 << 1;
            if (chkGPIODDR3.Checked) val += 1 << 2;
            if (chkGPIODDR4.Checked) val += 1 << 3;
            if (chkGPIODDR5.Checked) val += 1 << 4;
            if (chkGPIODDR6.Checked) val += 1 << 5;
            if (chkGPIODDR7.Checked) val += 1 << 6;
            if (chkGPIODDR8.Checked) val += 1 << 7;

            if (FWC.GPIODDRWrite(val) == 0)
                MessageBox.Show("Error in GPIODDRWrite");

            btnGPIODDRRead_Click(this, EventArgs.Empty);
        }

        private void btnGPIODDRRead_Click(object sender, System.EventArgs e)
        {
            uint val;
            if (FWC.GPIODDRRead(out val) == 0)
                MessageBox.Show("Error in GPIODDRRead");

            txtGPIODDRRead.Text = String.Format("{0:X4}", val);
        }

        private void btnGPIODDRWrite_Click(object sender, System.EventArgs e)
        {
            if (txtGPIODDRWrite.Text == "") return;
            uint val = uint.Parse(txtGPIODDRWrite.Text, System.Globalization.NumberStyles.HexNumber);

            if (FWC.GPIODDRWrite(val) == 0)
                MessageBox.Show("Error in GPIODDRWrite");

            btnGPIODDRRead_Click(this, EventArgs.Empty);
        }

        private void chkClockGenReset_CheckedChanged(object sender, System.EventArgs e)
        {
            uint val;
            FWC.I2C_WriteValue(0x42, 0x3);
            FWC.I2C_ReadValue(0x42, out val);

            if (chkClockGenReset.Checked)
                val &= 0xFB;
            else
                val |= 0x04;

            FWC.I2C_Write2Value(0x42, 0x3, (byte)val);
        }

        private void chkDDSReset_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkRX2DDS.Checked)
            {
                FWC.ResetRX2DDS();
            }
            else
            {
                uint val;
                FWC.I2C_WriteValue(0x42, 0x3);
                FWC.I2C_ReadValue(0x42, out val);

                if (chkDDSReset.Checked)
                    val &= 0xDF;
                else
                    val |= 0x20;

                FWC.I2C_Write2Value(0x42, 0x3, (byte)val);
            }
        }

        private void chkClockGenCS_CheckedChanged(object sender, System.EventArgs e)
        {
            uint val;
            FWC.I2C_WriteValue(0x42, 0x3);
            FWC.I2C_ReadValue(0x42, out val);

            if (chkClockGenCS.Checked)
                val &= 0xFE;
            else
                val |= 0x01;

            FWC.I2C_Write2Value(0x42, 0x3, (byte)val);
        }

        private void chkDDSCS_CheckedChanged(object sender, System.EventArgs e)
        {
            uint val;
            FWC.I2C_WriteValue(0x42, 0x3);
            FWC.I2C_ReadValue(0x42, out val);

            if (chkDDSCS.Checked)
                val &= 0xBF;
            else
                val |= 0x40;

            FWC.I2C_Write2Value(0x42, 0x3, (byte)val);
        }

        private void btnCodecRead_Click(object sender, System.EventArgs e)
        {
            if (comboCodecReg.Text == "") return;
            byte reg = byte.Parse(comboCodecReg.Text, System.Globalization.NumberStyles.HexNumber);

            byte val;
            if (FWC.ReadCodecReg(reg, out val) == 0)
                MessageBox.Show("Error in ReadCodecReg");

            txtCodecRead.Text = String.Format("{0:X2}", val);
        }

        private void comboCodecReg_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            btnCodecRead_Click(this, EventArgs.Empty);
        }

        private void btnCodecWrite_Click(object sender, System.EventArgs e)
        {
            if (comboCodecReg.Text == "") return;
            byte reg = byte.Parse(comboCodecReg.Text, System.Globalization.NumberStyles.HexNumber);
            byte val = byte.Parse(txtCodecWrite.Text, System.Globalization.NumberStyles.HexNumber);

            if (FWC.WriteCodecReg(reg, val) == 0)
                MessageBox.Show("Error in WriteCodecReg");

            btnCodecRead_Click(this, EventArgs.Empty);
        }

        private void FLEX5000LLHWForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
            Common.SaveForm(this, "FLEX5000LLHWForm");
        }

        private void comboPAPotIndex_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            btnPAPotRead_Click(this, EventArgs.Empty);
        }

        private void btnPAPotRead_Click(object sender, System.EventArgs e)
        {
            if (!pa_ok) return;
            txtPAPotRead.BackColor = Color.Red;
            Application.DoEvents();
            uint val;
            if (FWC.PAPotGetRDAC(out val) == 0)
                MessageBox.Show("Error in PAPotGetRDAC");

            txtPAPotRead.BackColor = SystemColors.Control;
            txtPAPotRead.Text = String.Format("{0:X4}", val);
        }

        private void btnPAPotWrite_Click(object sender, System.EventArgs e)
        {
            if (!pa_ok) return;
            if (comboPAPotIndex.Text == "") return;
            txtPAPotWrite.BackColor = Color.Red;
            Application.DoEvents();
            int index = comboPAPotIndex.SelectedIndex;
            int val = int.Parse(txtPAPotWrite.Text, System.Globalization.NumberStyles.HexNumber);

            if (FWC.PAPotSetRDAC(index, val) == 0)
                MessageBox.Show("Error in PAPotSetRDAC");

            txtPAPotWrite.BackColor = SystemColors.Window;
            btnPAPotRead_Click(this, EventArgs.Empty);
        }

        private void btnATUSendCmd_Click(object sender, System.EventArgs e)
        {
            if (txtATU1.Text == "" || txtATU2.Text == "" || txtATU3.Text == "") return;
            byte b1 = byte.Parse(txtATU1.Text);
            byte b2 = byte.Parse(txtATU2.Text);
            byte b3 = byte.Parse(txtATU3.Text);
            uint timeout_ms = 200;

            FWC.ATUSendCmd(b1, b2, b3);

            switch (b1)
            {
                case 5:
                case 6:
                    timeout_ms = 6000;
                    break;
            }

            byte count;
            string s = "";
            Thread.Sleep((int)timeout_ms);
            do
            {
                FWC.ATUGetResult(out b1, out b2, out b3, out count, timeout_ms);
                s += b1.ToString() + " " + b2.ToString() + " " + b3.ToString() + " (" + count.ToString() + " left)\n";
            } while (count > 0);
            MessageBox.Show(s);
        }

        #endregion

        private void btnATUFull_Click(object sender, System.EventArgs e)
        {
            FWCATU.FullTune();
        }

        private void btnEEPROMWriteFloat_Click(object sender, System.EventArgs e)
        {
            if (txtEEPROMOffset.Text == "") return;
            txtEEPROMWriteFloat.BackColor = Color.Red;
            Application.DoEvents();
            uint offset = uint.Parse(txtEEPROMOffset.Text, System.Globalization.NumberStyles.HexNumber);
            float val = float.Parse(txtEEPROMWriteFloat.Text);

            if (!chkRX2EEPROM.Checked)
            {
                if (FWC.WriteTRXEEPROMFloat(offset, val) == 0)
                    MessageBox.Show("Error in WriteTRXEEPROMFloat");
            }
            else
            {
                if (FWC.WriteRX2EEPROMFloat(offset, val) == 0)
                    MessageBox.Show("Error in WriteRX2EEPROMFloat");
            }

            txtEEPROMWriteFloat.BackColor = SystemColors.Window;
            btnEEPROMReadFloat_Click(this, EventArgs.Empty);
        }

        //===========================================================================================
        // ke9ns 
        private void btnEEPROMReadFloat_Click(object sender, System.EventArgs e)
        {
            if (txtEEPROMOffset.Text == "") return;
            txtEEPROMReadFloat.BackColor = Color.Red;
            Application.DoEvents();
            uint offset = uint.Parse(txtEEPROMOffset.Text, System.Globalization.NumberStyles.HexNumber);

            float data;
            if (!chkRX2EEPROM.Checked)
            {
                if (FWC.ReadTRXEEPROMFloat(offset, out data) == 0)
                    MessageBox.Show("Error in ReadTRXEEPROMFloat");
            }
            else
            {
                if (FWC.ReadRX2EEPROMFloat(offset, out data) == 0)
                    MessageBox.Show("Error in ReadRX2EEPROMFloat");
            }

            txtEEPROMReadFloat.BackColor = SystemColors.Control;
            txtEEPROMReadFloat.Text = data.ToString("f4");
        }

        private void btnFlexWireWriteVal_Click(object sender, System.EventArgs e)
        {
            try
            {
                byte addr = byte.Parse(txtFlexWireAddr.Text, NumberStyles.HexNumber);
                byte val = byte.Parse(txtFlexWireVal1.Text, NumberStyles.HexNumber);

                FWC.FlexWire_WriteValue(addr, val);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.StackTrace);
            }
        }

        private void btnFlexWireWrite2Val_Click(object sender, System.EventArgs e)
        {
            try
            {
                byte addr = byte.Parse(txtFlexWireAddr.Text, NumberStyles.HexNumber);
                byte v1 = byte.Parse(txtFlexWireVal1.Text, NumberStyles.HexNumber);
                byte v2 = byte.Parse(txtFlexWireVal2.Text, NumberStyles.HexNumber);

                FWC.FlexWire_Write2Value(addr, v1, v2);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.StackTrace);
            }
        }

        private void btnFlexWireReadVal_Click(object sender, System.EventArgs e)
        {
            try
            {
                byte addr = byte.Parse(txtFlexWireAddr.Text, NumberStyles.HexNumber);
                uint val;

                FWC.FlexWire_ReadValue(addr, out val);
                txtFlexWireVal2.Text = String.Format("{0:X2}", val);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.StackTrace);
            }
        }

        private void btnFlexWireRead2Val_Click(object sender, System.EventArgs e)
        {
            try
            {
                byte addr = byte.Parse(txtFlexWireAddr.Text, NumberStyles.HexNumber);
                uint val;

                FWC.FlexWire_Read2Value(addr, out val);
                txtFlexWireVal1.Text = String.Format("{0:X2}", val >> 8);
                txtFlexWireVal2.Text = String.Format("{0:X2}", (byte)val);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.StackTrace);
            }
        }

        private void chkL_CheckedChanged(object sender, System.EventArgs e)
        {
            byte data = 0;

            if (chkL2.Checked) data |= 0x01;
            if (chkL3.Checked) data |= 0x02;
            if (chkL4.Checked) data |= 0x04;
            if (chkL5.Checked) data |= 0x08;
            if (chkL6.Checked) data |= 0x10;
            if (chkL7.Checked) data |= 0x20;
            if (chkL8.Checked) data |= 0x40;
            if (chkL9.Checked) data |= 0x80;

            FWC.I2C_Write2Value(0x4E, 0x02, data);
        }

        private void chkC_CheckedChanged(object sender, System.EventArgs e)
        {
            byte data = 0;

            if (chkC0.Checked) data |= 0x01;
            if (chkC1.Checked) data |= 0x02;
            if (chkC2.Checked) data |= 0x04;
            if (chkC3.Checked) data |= 0x08;
            if (chkC4.Checked) data |= 0x10;
            if (chkC5.Checked) data |= 0x20;
            if (chkC6.Checked) data |= 0x40;

            FWC.I2C_Write2Value(0x4E, 0x03, data);
        }

        private void chkHiZ_CheckedChanged(object sender, System.EventArgs e)
        {
            FWC.SetHiZ(chkHiZ.Checked);
        }

        private void chkATUEnable_CheckedChanged(object sender, System.EventArgs e)
        {
            FWC.SetATUEnable(chkATUEnable.Checked);
        }

        private void chkATUATTN_CheckedChanged(object sender, System.EventArgs e)
        {
            FWC.SetATUATTN(chkATUATTN.Checked);
        }

        private void udATUL_ValueChanged(object sender, System.EventArgs e)
        {
            int val = (int)udATUL.Value;
            chkL9.Checked = ((val & 0x01) == 0x01);
            chkL8.Checked = ((val & 0x02) == 0x02);
            chkL7.Checked = ((val & 0x04) == 0x04);
            chkL6.Checked = ((val & 0x08) == 0x08);
            chkL5.Checked = ((val & 0x10) == 0x10);
            chkL4.Checked = ((val & 0x20) == 0x20);
            chkL3.Checked = ((val & 0x40) == 0x40);
            chkL2.Checked = ((val & 0x80) == 0x80);
        }

        private void udATUC_ValueChanged(object sender, System.EventArgs e)
        {
            int val = (int)udATUC.Value;
            chkC0.Checked = ((val & 0x01) == 0x01);
            chkC1.Checked = ((val & 0x02) == 0x02);
            chkC2.Checked = ((val & 0x04) == 0x04);
            chkC3.Checked = ((val & 0x08) == 0x08);
            chkC4.Checked = ((val & 0x10) == 0x10);
            chkC5.Checked = ((val & 0x20) == 0x20);
            chkC6.Checked = ((val & 0x40) == 0x40);
        }

        private void chkManualIOUpdate_CheckedChanged(object sender, EventArgs e)
        {
            FWC.SetManualIOUpdate(chkManualIOUpdate.Checked);
        }

        private void btnIOUpdate_Click(object sender, EventArgs e)
        {
            FWC.DDSIOUpdate();
        }

        private void chkRX2EEPROM_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void txtEEPROMOffset_TextChanged(object sender, EventArgs e)
        {

        }

        private void buttonTS2_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("This will reset any MARS,CAP,SHARES operate back to Normal Standard Operation\n",

                         "Yes?",
                         MessageBoxButtons.YesNo,
                         MessageBoxIcon.Question);

            if (dr == DialogResult.Yes)
            {

                if (FWC.WriteTRXEEPROMByte(0x0034, 0xff) == 0) MessageBox.Show("Error in WriteEEPROM 34");
                txtEEPROMWrite.BackColor = SystemColors.Window;
                btnEEPROMRead_Click(this, EventArgs.Empty);
                if (FWC.WriteTRXEEPROMByte(0x0035, 0xff) == 0) MessageBox.Show("Error in WriteEEPROM 35");
                txtEEPROMWrite.BackColor = SystemColors.Window;
                btnEEPROMRead_Click(this, EventArgs.Empty);
                if (FWC.WriteTRXEEPROMByte(0x0036, 0xff) == 0) MessageBox.Show("Error in WriteEEPROM 36");
                txtEEPROMWrite.BackColor = SystemColors.Window;
                btnEEPROMRead_Click(this, EventArgs.Empty);
                if (FWC.WriteTRXEEPROMByte(0x0037, 0xff) == 0) MessageBox.Show("Error in WriteEEPROM 37");
                txtEEPROMWrite.BackColor = SystemColors.Window;
                btnEEPROMRead_Click(this, EventArgs.Empty);
                if (FWC.WriteTRXEEPROMByte(0x0038, 0xff) == 0) MessageBox.Show("Error in WriteEEPROM 38");
                txtEEPROMWrite.BackColor = SystemColors.Window;
                btnEEPROMRead_Click(this, EventArgs.Empty);
                if (FWC.WriteTRXEEPROMByte(0x0039, 0xff) == 0) MessageBox.Show("Error in WriteEEPROM 39");
                txtEEPROMWrite.BackColor = SystemColors.Window;
                btnEEPROMRead_Click(this, EventArgs.Empty);
                if (FWC.WriteTRXEEPROMByte(0x003A, 0xff) == 0) MessageBox.Show("Error in WriteEEPROM 3a");
                txtEEPROMWrite.BackColor = SystemColors.Window;
                btnEEPROMRead_Click(this, EventArgs.Empty);
                if (FWC.WriteTRXEEPROMByte(0x003B, 0xff) == 0) MessageBox.Show("Error in WriteEEPROM 3b");
                txtEEPROMWrite.BackColor = SystemColors.Window;
                btnEEPROMRead_Click(this, EventArgs.Empty);
                if (FWC.WriteTRXEEPROMByte(0x003C, 0xff) == 0) MessageBox.Show("Error in WriteEEPROM 3c");
                txtEEPROMWrite.BackColor = SystemColors.Window;
                btnEEPROMRead_Click(this, EventArgs.Empty);

            }

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {


        } // numericUpDown1_ValueChanged

        private void buttonTS3_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Warning: You are changing your Turf Region.\n",
                                          "Do you have authorization?",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

            if (dr == DialogResult.Yes)
            {
                Debug.WriteLine("Byte value " + (byte)numericUpDown1.Value);

                FWC.WriteTRXEEPROMByte(0x001D, (byte)numericUpDown1.Value);
                txtEEPROMWrite.BackColor = SystemColors.Window;
                btnEEPROMRead_Click(this, EventArgs.Empty);


            }

            MessageBox.Show("You must close PowerSDR and cycle power to the radio. Then go to setup->General->Options->BandText Udpate", "Cycle Power",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);

        } //  buttonTS3_Click

        private void FLEX5000LLHWForm_Load(object sender, EventArgs e)
        {
            byte data;
            this.TopMost = true; // ke9ns .174
            FWC.ReadTRXEEPROMByte(0x001D, out data);

            numericUpDown1.Value = data;
        }
    }
}
