//=================================================================
// fwcCalForm.cs
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
using System.Diagnostics;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;

namespace PowerSDR
{
    public partial class FWCCalForm : System.Windows.Forms.Form
    {
        #region Variable Declaration

        private Console console;
       
        #endregion

        #region Constructor and Destructor

        public FWCCalForm(Console c)
        {
            InitializeComponent();
            console = c;
            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                    if (!FWCEEPROM.RX2OK)
                    {
                        grpRX2.Visible = false;
                        this.Height -= grpRX2.Height;
                    }
                    break; // do nothing
                case Model.FLEX3000:
                    this.Text = this.Text.Replace("FLEX-5000", "FLEX-3000");
                    grpRX2.Visible = false;
                    this.Height -= grpRX2.Height;
                    break;
                case Model.FLEX1500:
                    this.Text = this.Text.Replace("FLEX-5000", "FLEX-1500");
                    grpRX2.Visible = false;
                    btnResetTRXChecksums.Visible = false;
                    grpTRX.Height -= btnResetTRXChecksums.Height;
                    this.Height -= (grpRX2.Height + btnResetTRXChecksums.Height);
                    break;
            }
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

        

        #region Event Handlers

        private void btnRestoreFromEEPROM_Click(object sender, System.EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to read the current RX1/PA EEPROM data into\n" +
                "the database overwriting any current values?",
                "Overwrite database?",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if (dr == DialogResult.No) return;

            btnRestoreFromEEPROM.BackColor = console.ButtonSelectedColor;
            btnRestoreFromEEPROM.Enabled = false;
            Thread t = new Thread(new ThreadStart(RestoreFromEEPROM));
            t.Name = "Restore From EEPROM Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.Normal;
            t.Start();
        }

        private void RestoreFromEEPROM()
        {
            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                case Model.FLEX3000:
                    console.RestoreCalData();
                    break;
                case Model.FLEX1500:
                    console.Restore1500CalData();
                    break;
            }
            btnRestoreFromEEPROM.BackColor = SystemColors.Control;
            btnRestoreFromEEPROM.Enabled = true;
        }

        private void btnSaveToEEPROM_Click(object sender, System.EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to write the current RX1/PA database calibration values to the EEPROM?",
                "Overwrite EEPROM?",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if (dr == DialogResult.No) return;

            btnSaveToEEPROM.BackColor = console.ButtonSelectedColor;
            btnSaveToEEPROM.Enabled = false;
            Thread t = new Thread(new ThreadStart(WriteToEEPROM));
            t.Name = "Write To EEPROM Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.Normal;
            t.Start();
        }

        private void WriteToEEPROM()
        {
            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                case Model.FLEX3000:
                    console.WriteCalData();
                    break;
                case Model.FLEX1500:
                    console.Write1500CalData();
                    break;
            }
            btnSaveToEEPROM.BackColor = SystemColors.Control;
            btnSaveToEEPROM.Enabled = true;
        }

        #endregion

        private void btnRestoreRX2FromEEPROM_Click(object sender, System.EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to read the current RX2 EEPROM data into\n" +
                "the database overwriting any current values?",
                "Overwrite database?",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if (dr == DialogResult.No) return;

            btnRestoreRX2FromEEPROM.BackColor = console.ButtonSelectedColor;
            btnRestoreRX2FromEEPROM.Enabled = false;
            Thread t = new Thread(new ThreadStart(RestoreRX2FromEEPROM));
            t.Name = "Restore RX2 from EEPROM Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.Normal;
            t.Start();
        }

        private void RestoreRX2FromEEPROM()
        {
            console.RX2RestoreCalData();
            btnRestoreRX2FromEEPROM.BackColor = SystemColors.Control;
            btnRestoreRX2FromEEPROM.Enabled = true;
        }

        private void btnSaveRX2ToEEPROM_Click(object sender, System.EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to write the current RX2 database calibration values to the EEPROM?",
                "Overwrite EEPROM?",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if (dr == DialogResult.No) return;

            btnSaveRX2ToEEPROM.BackColor = console.ButtonSelectedColor;
            btnSaveRX2ToEEPROM.Enabled = false;
            Thread t = new Thread(new ThreadStart(WriteRX2ToEEPROM));
            t.Name = "Write RX2 To EEPROM Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.Normal;
            t.Start();
        }

        private void WriteRX2ToEEPROM()
        {
            console.WriteRX2CalData();
            btnSaveRX2ToEEPROM.BackColor = SystemColors.Control;
            btnSaveRX2ToEEPROM.Enabled = true;
        }

        private void btnResetTRXChecksums_Click(object sender, System.EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to reset the RX1/PA checksums?",
                "Reset Checksums?",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if (dr == DialogResult.No) return;

            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                case Model.FLEX3000:
                    FWCEEPROM.TRXChecksumPresent = false;
                    break;
            }
        }

        private void btnResetRX2Checksums_Click(object sender, System.EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to reset the RX2 checksums?",
                "Reset Checksums?",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if (dr == DialogResult.No) return;

            FWCEEPROM.RX2ChecksumPresent = false;
        }
    }
}
