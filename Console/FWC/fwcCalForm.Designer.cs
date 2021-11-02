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


namespace PowerSDR
{
    public partial class FWCCalForm : System.Windows.Forms.Form
    {
       

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FWCCalForm));
            this.grpTRX = new System.Windows.Forms.GroupBox();
            this.btnRestoreFromEEPROM = new System.Windows.Forms.ButtonTS();
            this.btnSaveToEEPROM = new System.Windows.Forms.ButtonTS();
            this.btnResetTRXChecksums = new System.Windows.Forms.Button();
            this.grpRX2 = new System.Windows.Forms.GroupBox();
            this.btnResetRX2Checksums = new System.Windows.Forms.Button();
            this.btnRestoreRX2FromEEPROM = new System.Windows.Forms.ButtonTS();
            this.btnSaveRX2ToEEPROM = new System.Windows.Forms.ButtonTS();
            this.grpTRX.SuspendLayout();
            this.grpRX2.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpTRX
            // 
            this.grpTRX.Controls.Add(this.btnRestoreFromEEPROM);
            this.grpTRX.Controls.Add(this.btnSaveToEEPROM);
            this.grpTRX.Controls.Add(this.btnResetTRXChecksums);
            this.grpTRX.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpTRX.Location = new System.Drawing.Point(8, 8);
            this.grpTRX.Name = "grpTRX";
            this.grpTRX.Size = new System.Drawing.Size(256, 136);
            this.grpTRX.TabIndex = 16;
            this.grpTRX.TabStop = false;
            this.grpTRX.Text = "RX1 / PA";
            // 
            // btnRestoreFromEEPROM
            // 
            this.btnRestoreFromEEPROM.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRestoreFromEEPROM.Image = null;
            this.btnRestoreFromEEPROM.Location = new System.Drawing.Point(8, 24);
            this.btnRestoreFromEEPROM.Name = "btnRestoreFromEEPROM";
            this.btnRestoreFromEEPROM.Size = new System.Drawing.Size(112, 48);
            this.btnRestoreFromEEPROM.TabIndex = 15;
            this.btnRestoreFromEEPROM.Text = "Restore Calibration Data To Database from EEPROM";
            this.btnRestoreFromEEPROM.Click += new System.EventHandler(this.btnRestoreFromEEPROM_Click);
            // 
            // btnSaveToEEPROM
            // 
            this.btnSaveToEEPROM.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveToEEPROM.Image = null;
            this.btnSaveToEEPROM.Location = new System.Drawing.Point(128, 24);
            this.btnSaveToEEPROM.Name = "btnSaveToEEPROM";
            this.btnSaveToEEPROM.Size = new System.Drawing.Size(112, 48);
            this.btnSaveToEEPROM.TabIndex = 0;
            this.btnSaveToEEPROM.Text = "Save Calibration Data To EEPROM";
            this.btnSaveToEEPROM.Click += new System.EventHandler(this.btnSaveToEEPROM_Click);
            // 
            // btnResetTRXChecksums
            // 
            this.btnResetTRXChecksums.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnResetTRXChecksums.Location = new System.Drawing.Point(8, 80);
            this.btnResetTRXChecksums.Name = "btnResetTRXChecksums";
            this.btnResetTRXChecksums.Size = new System.Drawing.Size(112, 48);
            this.btnResetTRXChecksums.TabIndex = 18;
            this.btnResetTRXChecksums.Text = "Reset RX1/PA Checksums";
            this.btnResetTRXChecksums.Click += new System.EventHandler(this.btnResetTRXChecksums_Click);
            // 
            // grpRX2
            // 
            this.grpRX2.Controls.Add(this.btnResetRX2Checksums);
            this.grpRX2.Controls.Add(this.btnRestoreRX2FromEEPROM);
            this.grpRX2.Controls.Add(this.btnSaveRX2ToEEPROM);
            this.grpRX2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpRX2.Location = new System.Drawing.Point(8, 152);
            this.grpRX2.Name = "grpRX2";
            this.grpRX2.Size = new System.Drawing.Size(256, 136);
            this.grpRX2.TabIndex = 17;
            this.grpRX2.TabStop = false;
            this.grpRX2.Text = "RX2";
            // 
            // btnResetRX2Checksums
            // 
            this.btnResetRX2Checksums.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnResetRX2Checksums.Location = new System.Drawing.Point(8, 80);
            this.btnResetRX2Checksums.Name = "btnResetRX2Checksums";
            this.btnResetRX2Checksums.Size = new System.Drawing.Size(112, 48);
            this.btnResetRX2Checksums.TabIndex = 19;
            this.btnResetRX2Checksums.Text = "Reset RX2 Checksums";
            this.btnResetRX2Checksums.Click += new System.EventHandler(this.btnResetRX2Checksums_Click);
            // 
            // btnRestoreRX2FromEEPROM
            // 
            this.btnRestoreRX2FromEEPROM.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRestoreRX2FromEEPROM.Image = null;
            this.btnRestoreRX2FromEEPROM.Location = new System.Drawing.Point(8, 24);
            this.btnRestoreRX2FromEEPROM.Name = "btnRestoreRX2FromEEPROM";
            this.btnRestoreRX2FromEEPROM.Size = new System.Drawing.Size(112, 48);
            this.btnRestoreRX2FromEEPROM.TabIndex = 15;
            this.btnRestoreRX2FromEEPROM.Text = "Restore Calibration Data To Database from EEPROM";
            this.btnRestoreRX2FromEEPROM.Click += new System.EventHandler(this.btnRestoreRX2FromEEPROM_Click);
            // 
            // btnSaveRX2ToEEPROM
            // 
            this.btnSaveRX2ToEEPROM.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveRX2ToEEPROM.Image = null;
            this.btnSaveRX2ToEEPROM.Location = new System.Drawing.Point(128, 24);
            this.btnSaveRX2ToEEPROM.Name = "btnSaveRX2ToEEPROM";
            this.btnSaveRX2ToEEPROM.Size = new System.Drawing.Size(112, 48);
            this.btnSaveRX2ToEEPROM.TabIndex = 0;
            this.btnSaveRX2ToEEPROM.Text = "Save Calibration Data To EEPROM";
            this.btnSaveRX2ToEEPROM.Click += new System.EventHandler(this.btnSaveRX2ToEEPROM_Click);
            // 
            // FWCCalForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(272, 296);
            this.Controls.Add(this.grpRX2);
            this.Controls.Add(this.grpTRX);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(288, 335);
            this.MinimumSize = new System.Drawing.Size(288, 335);
            this.Name = "FWCCalForm";
            this.Text = "FLEX-5000 EEPROM";
            this.grpTRX.ResumeLayout(false);
            this.grpRX2.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

       
        private System.Windows.Forms.ButtonTS btnSaveToEEPROM;
        private System.Windows.Forms.ButtonTS btnRestoreFromEEPROM;
        private System.Windows.Forms.GroupBox grpTRX;
        private System.Windows.Forms.GroupBox grpRX2;
        private System.Windows.Forms.ButtonTS btnSaveRX2ToEEPROM;
        private System.Windows.Forms.ButtonTS btnRestoreRX2FromEEPROM;
        private System.Windows.Forms.Button btnResetTRXChecksums;
        private System.Windows.Forms.Button btnResetRX2Checksums;
        private System.ComponentModel.Container components = null;

        
    }
}
