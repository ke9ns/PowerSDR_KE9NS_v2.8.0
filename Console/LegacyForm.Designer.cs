//=================================================================
// LegacyForm.Designer.cs
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
    partial class LegacyForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LegacyForm));
            this.txtSDR1000SN = new System.Windows.Forms.TextBoxTS();
            this.txtSoftRockSN = new System.Windows.Forms.TextBoxTS();
            this.lblSerialNumber = new System.Windows.Forms.LabelTS();
            this.lblInstructions = new System.Windows.Forms.LabelTS();
            this.btnOK = new System.Windows.Forms.ButtonTS();
            this.chkSoftRock = new System.Windows.Forms.CheckBoxTS();
            this.chkSDR1000 = new System.Windows.Forms.CheckBoxTS();
            this.SuspendLayout();
            // 
            // txtSDR1000SN
            // 
            this.txtSDR1000SN.Location = new System.Drawing.Point(119, 66);
            this.txtSDR1000SN.Name = "txtSDR1000SN";
            this.txtSDR1000SN.Size = new System.Drawing.Size(83, 20);
            this.txtSDR1000SN.TabIndex = 4;
            // 
            // txtSoftRockSN
            // 
            this.txtSoftRockSN.Location = new System.Drawing.Point(119, 88);
            this.txtSoftRockSN.Name = "txtSoftRockSN";
            this.txtSoftRockSN.Size = new System.Drawing.Size(83, 20);
            this.txtSoftRockSN.TabIndex = 6;
            // 
            // lblSerialNumber
            // 
            this.lblSerialNumber.AutoSize = true;
            this.lblSerialNumber.Image = null;
            this.lblSerialNumber.Location = new System.Drawing.Point(116, 50);
            this.lblSerialNumber.Name = "lblSerialNumber";
            this.lblSerialNumber.Size = new System.Drawing.Size(73, 13);
            this.lblSerialNumber.TabIndex = 5;
            this.lblSerialNumber.Text = "Serial Number";
            // 
            // lblInstructions
            // 
            this.lblInstructions.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInstructions.Image = null;
            this.lblInstructions.Location = new System.Drawing.Point(10, 9);
            this.lblInstructions.Name = "lblInstructions";
            this.lblInstructions.Size = new System.Drawing.Size(221, 31);
            this.lblInstructions.TabIndex = 3;
            this.lblInstructions.Text = "Please indicate below any legacy radios you use with PowerSDR.";
            // 
            // btnOK
            // 
            this.btnOK.Image = null;
            this.btnOK.Location = new System.Drawing.Point(80, 137);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // chkSoftRock
            // 
            this.chkSoftRock.AutoSize = true;
            this.chkSoftRock.Image = null;
            this.chkSoftRock.Location = new System.Drawing.Point(12, 90);
            this.chkSoftRock.Name = "chkSoftRock";
            this.chkSoftRock.Size = new System.Drawing.Size(86, 17);
            this.chkSoftRock.TabIndex = 1;
            this.chkSoftRock.Text = "SoftRock 40";
            this.chkSoftRock.UseVisualStyleBackColor = true;
            // 
            // chkSDR1000
            // 
            this.chkSDR1000.AutoSize = true;
            this.chkSDR1000.Image = null;
            this.chkSDR1000.Location = new System.Drawing.Point(12, 67);
            this.chkSDR1000.Name = "chkSDR1000";
            this.chkSDR1000.Size = new System.Drawing.Size(76, 17);
            this.chkSDR1000.TabIndex = 0;
            this.chkSDR1000.Text = "SDR-1000";
            this.chkSDR1000.UseVisualStyleBackColor = true;
            // 
            // LegacyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(243, 167);
            this.Controls.Add(this.txtSDR1000SN);
            this.Controls.Add(this.txtSoftRockSN);
            this.Controls.Add(this.lblSerialNumber);
            this.Controls.Add(this.lblInstructions);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.chkSoftRock);
            this.Controls.Add(this.chkSDR1000);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LegacyForm";
            this.Text = "Legacy Radios";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LegacyForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBoxTS chkSDR1000;
        private System.Windows.Forms.CheckBoxTS chkSoftRock;
        private System.Windows.Forms.ButtonTS btnOK;
        private System.Windows.Forms.LabelTS lblInstructions;
        private System.Windows.Forms.TextBoxTS txtSDR1000SN;
        private System.Windows.Forms.LabelTS lblSerialNumber;
        private System.Windows.Forms.TextBoxTS txtSoftRockSN;
    }
}