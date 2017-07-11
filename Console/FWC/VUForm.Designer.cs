//=================================================================
// VUForm.Designer.cs
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
    partial class VUForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VUForm));
            this.chkVUmodeVLG1 = new System.Windows.Forms.CheckBox();
            this.chkVUmodeVHGRX1 = new System.Windows.Forms.CheckBox();
            this.chkVUmodeVHG2 = new System.Windows.Forms.CheckBox();
            this.chkVUmodeVLG2 = new System.Windows.Forms.CheckBox();
            this.chkVUmodeUHG2 = new System.Windows.Forms.CheckBox();
            this.chkVUmodeULG2 = new System.Windows.Forms.CheckBox();
            this.chkVUmodeUHG1 = new System.Windows.Forms.CheckBox();
            this.chkVUmodeULG1 = new System.Windows.Forms.CheckBox();
            this.chkVUmodeTXV60W = new System.Windows.Forms.CheckBox();
            this.chkVUmodeTXVLP = new System.Windows.Forms.CheckBox();
            this.chkVUmodeTXU60W = new System.Windows.Forms.CheckBox();
            this.chkVUmodeTXULP = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chkVUFanHigh = new System.Windows.Forms.CheckBox();
            this.chkVUKeyV = new System.Windows.Forms.CheckBox();
            this.chkVUTXIFU = new System.Windows.Forms.CheckBox();
            this.chkVUKeyU = new System.Windows.Forms.CheckBox();
            this.chkVURXURX2 = new System.Windows.Forms.CheckBox();
            this.chkVURX2V = new System.Windows.Forms.CheckBox();
            this.chkVURXIFV = new System.Windows.Forms.CheckBox();
            this.chkVUK14 = new System.Windows.Forms.CheckBox();
            this.chkVUK16 = new System.Windows.Forms.CheckBox();
            this.chkVUK17 = new System.Windows.Forms.CheckBox();
            this.chkVUKeyVU = new System.Windows.Forms.CheckBox();
            this.chkVUDrvU = new System.Windows.Forms.CheckBox();
            this.chkVUDrvV = new System.Windows.Forms.CheckBox();
            this.chkVUK18 = new System.Windows.Forms.CheckBox();
            this.chkVUK15 = new System.Windows.Forms.CheckBox();
            this.chkVULPwrU = new System.Windows.Forms.CheckBox();
            this.chkVURXV = new System.Windows.Forms.CheckBox();
            this.chkVUTXU = new System.Windows.Forms.CheckBox();
            this.chkVUTXV = new System.Windows.Forms.CheckBox();
            this.chkVUUIFHG1 = new System.Windows.Forms.CheckBox();
            this.chkVUVIFHG1 = new System.Windows.Forms.CheckBox();
            this.chkVULPwrV = new System.Windows.Forms.CheckBox();
            this.chkVUUIFHG2 = new System.Windows.Forms.CheckBox();
            this.chkVUVIFHG2 = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkVU_VIFGain = new System.Windows.Forms.CheckBox();
            this.chkVU_VPAEnable = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.chkVRX2 = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.chkURX2 = new System.Windows.Forms.CheckBox();
            this.chkVU_UIFGain = new System.Windows.Forms.CheckBox();
            this.chkVU_UPAEnable = new System.Windows.Forms.CheckBox();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkVUmodeVLG1
            // 
            this.chkVUmodeVLG1.AutoSize = true;
            this.chkVUmodeVLG1.Location = new System.Drawing.Point(17, 25);
            this.chkVUmodeVLG1.Name = "chkVUmodeVLG1";
            this.chkVUmodeVLG1.Size = new System.Drawing.Size(74, 17);
            this.chkVUmodeVLG1.TabIndex = 24;
            this.chkVUmodeVLG1.Text = "RX1-LG-V";
            this.chkVUmodeVLG1.UseVisualStyleBackColor = true;
            this.chkVUmodeVLG1.CheckedChanged += new System.EventHandler(this.chkVUmodeVLG1_CheckedChanged);
            // 
            // chkVUmodeVHGRX1
            // 
            this.chkVUmodeVHGRX1.AutoSize = true;
            this.chkVUmodeVHGRX1.Location = new System.Drawing.Point(17, 48);
            this.chkVUmodeVHGRX1.Name = "chkVUmodeVHGRX1";
            this.chkVUmodeVHGRX1.Size = new System.Drawing.Size(76, 17);
            this.chkVUmodeVHGRX1.TabIndex = 25;
            this.chkVUmodeVHGRX1.Text = "RX1-HG-V";
            this.chkVUmodeVHGRX1.UseVisualStyleBackColor = true;
            this.chkVUmodeVHGRX1.CheckedChanged += new System.EventHandler(this.chkVUmodeVHGRX1_CheckedChanged);
            // 
            // chkVUmodeVHG2
            // 
            this.chkVUmodeVHG2.AutoSize = true;
            this.chkVUmodeVHG2.Location = new System.Drawing.Point(115, 48);
            this.chkVUmodeVHG2.Name = "chkVUmodeVHG2";
            this.chkVUmodeVHG2.Size = new System.Drawing.Size(76, 17);
            this.chkVUmodeVHG2.TabIndex = 28;
            this.chkVUmodeVHG2.Text = "RX2-HG-V";
            this.chkVUmodeVHG2.UseVisualStyleBackColor = true;
            this.chkVUmodeVHG2.CheckedChanged += new System.EventHandler(this.chkVUmodeVHG2_CheckedChanged);
            // 
            // chkVUmodeVLG2
            // 
            this.chkVUmodeVLG2.AutoSize = true;
            this.chkVUmodeVLG2.Location = new System.Drawing.Point(115, 25);
            this.chkVUmodeVLG2.Name = "chkVUmodeVLG2";
            this.chkVUmodeVLG2.Size = new System.Drawing.Size(74, 17);
            this.chkVUmodeVLG2.TabIndex = 27;
            this.chkVUmodeVLG2.Text = "RX2-LG-V";
            this.chkVUmodeVLG2.UseVisualStyleBackColor = true;
            this.chkVUmodeVLG2.CheckedChanged += new System.EventHandler(this.chkVUmodeVLG2_CheckedChanged);
            // 
            // chkVUmodeUHG2
            // 
            this.chkVUmodeUHG2.AutoSize = true;
            this.chkVUmodeUHG2.Location = new System.Drawing.Point(115, 42);
            this.chkVUmodeUHG2.Name = "chkVUmodeUHG2";
            this.chkVUmodeUHG2.Size = new System.Drawing.Size(77, 17);
            this.chkVUmodeUHG2.TabIndex = 32;
            this.chkVUmodeUHG2.Text = "RX2-HG-U";
            this.chkVUmodeUHG2.UseVisualStyleBackColor = true;
            this.chkVUmodeUHG2.CheckedChanged += new System.EventHandler(this.chkVUmodeUHG2_CheckedChanged);
            // 
            // chkVUmodeULG2
            // 
            this.chkVUmodeULG2.AutoSize = true;
            this.chkVUmodeULG2.Location = new System.Drawing.Point(115, 19);
            this.chkVUmodeULG2.Name = "chkVUmodeULG2";
            this.chkVUmodeULG2.Size = new System.Drawing.Size(75, 17);
            this.chkVUmodeULG2.TabIndex = 31;
            this.chkVUmodeULG2.Text = "RX2-LG-U";
            this.chkVUmodeULG2.UseVisualStyleBackColor = true;
            this.chkVUmodeULG2.CheckedChanged += new System.EventHandler(this.chkVUmodeULG2_CheckedChanged);
            // 
            // chkVUmodeUHG1
            // 
            this.chkVUmodeUHG1.AutoSize = true;
            this.chkVUmodeUHG1.Location = new System.Drawing.Point(17, 42);
            this.chkVUmodeUHG1.Name = "chkVUmodeUHG1";
            this.chkVUmodeUHG1.Size = new System.Drawing.Size(77, 17);
            this.chkVUmodeUHG1.TabIndex = 30;
            this.chkVUmodeUHG1.Text = "RX1-HG-U";
            this.chkVUmodeUHG1.UseVisualStyleBackColor = true;
            this.chkVUmodeUHG1.CheckedChanged += new System.EventHandler(this.chkVUmodeUHG1_CheckedChanged);
            // 
            // chkVUmodeULG1
            // 
            this.chkVUmodeULG1.AutoSize = true;
            this.chkVUmodeULG1.Location = new System.Drawing.Point(17, 19);
            this.chkVUmodeULG1.Name = "chkVUmodeULG1";
            this.chkVUmodeULG1.Size = new System.Drawing.Size(75, 17);
            this.chkVUmodeULG1.TabIndex = 29;
            this.chkVUmodeULG1.Text = "RX1-LG-U";
            this.chkVUmodeULG1.UseVisualStyleBackColor = true;
            this.chkVUmodeULG1.CheckedChanged += new System.EventHandler(this.chkVUmodeULG1_CheckedChanged);
            // 
            // chkVUmodeTXV60W
            // 
            this.chkVUmodeTXV60W.AutoSize = true;
            this.chkVUmodeTXV60W.Location = new System.Drawing.Point(17, 107);
            this.chkVUmodeTXV60W.Name = "chkVUmodeTXV60W";
            this.chkVUmodeTXV60W.Size = new System.Drawing.Size(76, 17);
            this.chkVUmodeTXV60W.TabIndex = 34;
            this.chkVUmodeTXV60W.Text = "TX-60W-V";
            this.chkVUmodeTXV60W.UseVisualStyleBackColor = true;
            this.chkVUmodeTXV60W.CheckedChanged += new System.EventHandler(this.chkVUmodeTXV60W_CheckedChanged);
            // 
            // chkVUmodeTXVLP
            // 
            this.chkVUmodeTXVLP.AutoSize = true;
            this.chkVUmodeTXVLP.Location = new System.Drawing.Point(17, 84);
            this.chkVUmodeTXVLP.Name = "chkVUmodeTXVLP";
            this.chkVUmodeTXVLP.Size = new System.Drawing.Size(85, 17);
            this.chkVUmodeTXVLP.TabIndex = 33;
            this.chkVUmodeTXVLP.Text = "TX-LPWR-V";
            this.chkVUmodeTXVLP.UseVisualStyleBackColor = true;
            this.chkVUmodeTXVLP.CheckedChanged += new System.EventHandler(this.chkVUmodeTXVLP_CheckedChanged);
            // 
            // chkVUmodeTXU60W
            // 
            this.chkVUmodeTXU60W.AutoSize = true;
            this.chkVUmodeTXU60W.Location = new System.Drawing.Point(17, 100);
            this.chkVUmodeTXU60W.Name = "chkVUmodeTXU60W";
            this.chkVUmodeTXU60W.Size = new System.Drawing.Size(77, 17);
            this.chkVUmodeTXU60W.TabIndex = 36;
            this.chkVUmodeTXU60W.Text = "TX-60W-U";
            this.chkVUmodeTXU60W.UseVisualStyleBackColor = true;
            this.chkVUmodeTXU60W.CheckedChanged += new System.EventHandler(this.chkVUmodeTXU60W_CheckedChanged);
            // 
            // chkVUmodeTXULP
            // 
            this.chkVUmodeTXULP.AutoSize = true;
            this.chkVUmodeTXULP.Location = new System.Drawing.Point(17, 77);
            this.chkVUmodeTXULP.Name = "chkVUmodeTXULP";
            this.chkVUmodeTXULP.Size = new System.Drawing.Size(86, 17);
            this.chkVUmodeTXULP.TabIndex = 35;
            this.chkVUmodeTXULP.Text = "TX-LPWR-U";
            this.chkVUmodeTXULP.UseVisualStyleBackColor = true;
            this.chkVUmodeTXULP.CheckedChanged += new System.EventHandler(this.chkVUmodeTXULP_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkVUmodeTXV60W);
            this.groupBox2.Controls.Add(this.chkVUmodeTXVLP);
            this.groupBox2.Controls.Add(this.chkVUmodeVHG2);
            this.groupBox2.Controls.Add(this.chkVUmodeVLG2);
            this.groupBox2.Controls.Add(this.chkVUmodeVHGRX1);
            this.groupBox2.Controls.Add(this.chkVUmodeVLG1);
            this.groupBox2.Enabled = false;
            this.groupBox2.Location = new System.Drawing.Point(236, 129);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 133);
            this.groupBox2.TabIndex = 37;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "VHF User Modes";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.chkVUmodeTXU60W);
            this.groupBox3.Controls.Add(this.chkVUmodeTXULP);
            this.groupBox3.Controls.Add(this.chkVUmodeUHG2);
            this.groupBox3.Controls.Add(this.chkVUmodeULG2);
            this.groupBox3.Controls.Add(this.chkVUmodeUHG1);
            this.groupBox3.Controls.Add(this.chkVUmodeULG1);
            this.groupBox3.Enabled = false;
            this.groupBox3.Location = new System.Drawing.Point(236, 263);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(199, 128);
            this.groupBox3.TabIndex = 38;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "UHF User Modes";
            // 
            // chkVUFanHigh
            // 
            this.chkVUFanHigh.AutoSize = true;
            this.chkVUFanHigh.Location = new System.Drawing.Point(97, 27);
            this.chkVUFanHigh.Name = "chkVUFanHigh";
            this.chkVUFanHigh.Size = new System.Drawing.Size(69, 17);
            this.chkVUFanHigh.TabIndex = 0;
            this.chkVUFanHigh.Text = "Fan High";
            this.chkVUFanHigh.UseVisualStyleBackColor = true;
            this.chkVUFanHigh.CheckedChanged += new System.EventHandler(this.chkFanHigh_CheckedChanged);
            // 
            // chkVUKeyV
            // 
            this.chkVUKeyV.AutoSize = true;
            this.chkVUKeyV.Location = new System.Drawing.Point(6, 27);
            this.chkVUKeyV.Name = "chkVUKeyV";
            this.chkVUKeyV.Size = new System.Drawing.Size(54, 17);
            this.chkVUKeyV.TabIndex = 1;
            this.chkVUKeyV.Text = "Key-V";
            this.chkVUKeyV.UseVisualStyleBackColor = true;
            this.chkVUKeyV.CheckedChanged += new System.EventHandler(this.chkVUKeyV_CheckedChanged);
            // 
            // chkVUTXIFU
            // 
            this.chkVUTXIFU.AutoSize = true;
            this.chkVUTXIFU.Location = new System.Drawing.Point(5, 178);
            this.chkVUTXIFU.Name = "chkVUTXIFU";
            this.chkVUTXIFU.Size = new System.Drawing.Size(79, 17);
            this.chkVUTXIFU.TabIndex = 2;
            this.chkVUTXIFU.Text = "K4 [TXIFU]";
            this.chkVUTXIFU.UseVisualStyleBackColor = true;
            this.chkVUTXIFU.CheckedChanged += new System.EventHandler(this.chkVUTXIFU_CheckedChanged);
            // 
            // chkVUKeyU
            // 
            this.chkVUKeyU.AutoSize = true;
            this.chkVUKeyU.Location = new System.Drawing.Point(6, 50);
            this.chkVUKeyU.Name = "chkVUKeyU";
            this.chkVUKeyU.Size = new System.Drawing.Size(55, 17);
            this.chkVUKeyU.TabIndex = 3;
            this.chkVUKeyU.Text = "Key-U";
            this.chkVUKeyU.UseVisualStyleBackColor = true;
            this.chkVUKeyU.CheckedChanged += new System.EventHandler(this.chkVUKeyU_CheckedChanged);
            // 
            // chkVURXURX2
            // 
            this.chkVURXURX2.AutoSize = true;
            this.chkVURXURX2.Location = new System.Drawing.Point(102, 132);
            this.chkVURXURX2.Name = "chkVURXURX2";
            this.chkVURXURX2.Size = new System.Drawing.Size(98, 17);
            this.chkVURXURX2.TabIndex = 4;
            this.chkVURXURX2.Text = "K11 [RXURX2]";
            this.chkVURXURX2.UseVisualStyleBackColor = true;
            this.chkVURXURX2.CheckedChanged += new System.EventHandler(this.chkVURXURX2_CheckedChanged);
            // 
            // chkVURX2V
            // 
            this.chkVURX2V.AutoSize = true;
            this.chkVURX2V.Location = new System.Drawing.Point(102, 179);
            this.chkVURX2V.Name = "chkVURX2V";
            this.chkVURX2V.Size = new System.Drawing.Size(82, 17);
            this.chkVURX2V.TabIndex = 5;
            this.chkVURX2V.Text = "K13 [RX2V]";
            this.chkVURX2V.UseVisualStyleBackColor = true;
            this.chkVURX2V.CheckedChanged += new System.EventHandler(this.chkVURX2V_CheckedChanged);
            // 
            // chkVURXIFV
            // 
            this.chkVURXIFV.AutoSize = true;
            this.chkVURXIFV.Location = new System.Drawing.Point(5, 294);
            this.chkVURXIFV.Name = "chkVURXIFV";
            this.chkVURXIFV.Size = new System.Drawing.Size(79, 17);
            this.chkVURXIFV.TabIndex = 6;
            this.chkVURXIFV.Text = "K9 [RXIFV]";
            this.chkVURXIFV.UseVisualStyleBackColor = true;
            this.chkVURXIFV.CheckedChanged += new System.EventHandler(this.chkVURXIFV_CheckedChanged);
            // 
            // chkVUK14
            // 
            this.chkVUK14.AutoSize = true;
            this.chkVUK14.Location = new System.Drawing.Point(102, 202);
            this.chkVUK14.Name = "chkVUK14";
            this.chkVUK14.Size = new System.Drawing.Size(77, 17);
            this.chkVUK14.TabIndex = 7;
            this.chkVUK14.Text = "K14 [V-LP]";
            this.chkVUK14.UseVisualStyleBackColor = true;
            this.chkVUK14.CheckedChanged += new System.EventHandler(this.chkVUK14_CheckedChanged);
            // 
            // chkVUK16
            // 
            this.chkVUK16.AutoSize = true;
            this.chkVUK16.Location = new System.Drawing.Point(102, 248);
            this.chkVUK16.Name = "chkVUK16";
            this.chkVUK16.Size = new System.Drawing.Size(83, 17);
            this.chkVUK16.TabIndex = 8;
            this.chkVUK16.Text = "K16 [RX2N]";
            this.chkVUK16.UseVisualStyleBackColor = true;
            this.chkVUK16.CheckedChanged += new System.EventHandler(this.chkVUK16_CheckedChanged);
            // 
            // chkVUK17
            // 
            this.chkVUK17.AutoSize = true;
            this.chkVUK17.Location = new System.Drawing.Point(102, 271);
            this.chkVUK17.Name = "chkVUK17";
            this.chkVUK17.Size = new System.Drawing.Size(78, 17);
            this.chkVUK17.TabIndex = 9;
            this.chkVUK17.Text = "K17 [U-LP]";
            this.chkVUK17.UseVisualStyleBackColor = true;
            this.chkVUK17.CheckedChanged += new System.EventHandler(this.chkVUK17_CheckedChanged);
            // 
            // chkVUKeyVU
            // 
            this.chkVUKeyVU.AutoSize = true;
            this.chkVUKeyVU.Location = new System.Drawing.Point(6, 73);
            this.chkVUKeyVU.Name = "chkVUKeyVU";
            this.chkVUKeyVU.Size = new System.Drawing.Size(62, 17);
            this.chkVUKeyVU.TabIndex = 10;
            this.chkVUKeyVU.Text = "Key VU";
            this.chkVUKeyVU.UseVisualStyleBackColor = true;
            this.chkVUKeyVU.CheckedChanged += new System.EventHandler(this.chkVUKeyVU_CheckedChanged);
            // 
            // chkVUDrvU
            // 
            this.chkVUDrvU.AutoSize = true;
            this.chkVUDrvU.Location = new System.Drawing.Point(6, 155);
            this.chkVUDrvU.Name = "chkVUDrvU";
            this.chkVUDrvU.Size = new System.Drawing.Size(82, 17);
            this.chkVUDrvU.TabIndex = 11;
            this.chkVUDrvU.Text = "K3 [DRV-U]";
            this.chkVUDrvU.UseVisualStyleBackColor = true;
            this.chkVUDrvU.CheckedChanged += new System.EventHandler(this.chkVUDrvU_CheckedChanged);
            // 
            // chkVUDrvV
            // 
            this.chkVUDrvV.AutoSize = true;
            this.chkVUDrvV.Location = new System.Drawing.Point(5, 132);
            this.chkVUDrvV.Name = "chkVUDrvV";
            this.chkVUDrvV.Size = new System.Drawing.Size(81, 17);
            this.chkVUDrvV.TabIndex = 12;
            this.chkVUDrvV.Text = "K2 [DRV-V]";
            this.chkVUDrvV.UseVisualStyleBackColor = true;
            this.chkVUDrvV.CheckedChanged += new System.EventHandler(this.chkVUDrvV_CheckedChanged);
            // 
            // chkVUK18
            // 
            this.chkVUK18.AutoSize = true;
            this.chkVUK18.Location = new System.Drawing.Point(102, 294);
            this.chkVUK18.Name = "chkVUK18";
            this.chkVUK18.Size = new System.Drawing.Size(85, 17);
            this.chkVUK18.TabIndex = 13;
            this.chkVUK18.Text = "K18 [U-R/T]";
            this.chkVUK18.UseVisualStyleBackColor = true;
            this.chkVUK18.CheckedChanged += new System.EventHandler(this.chkVUK18_CheckedChanged);
            // 
            // chkVUK15
            // 
            this.chkVUK15.AutoSize = true;
            this.chkVUK15.Location = new System.Drawing.Point(102, 225);
            this.chkVUK15.Name = "chkVUK15";
            this.chkVUK15.Size = new System.Drawing.Size(84, 17);
            this.chkVUK15.TabIndex = 14;
            this.chkVUK15.Text = "K15 [V-R/T]";
            this.chkVUK15.UseVisualStyleBackColor = true;
            this.chkVUK15.CheckedChanged += new System.EventHandler(this.chkVUK15_CheckedChanged);
            // 
            // chkVULPwrU
            // 
            this.chkVULPwrU.AutoSize = true;
            this.chkVULPwrU.Location = new System.Drawing.Point(5, 201);
            this.chkVULPwrU.Name = "chkVULPwrU";
            this.chkVULPwrU.Size = new System.Drawing.Size(91, 17);
            this.chkVULPwrU.TabIndex = 15;
            this.chkVULPwrU.Text = "K5 [U-LPWR]";
            this.chkVULPwrU.UseVisualStyleBackColor = true;
            this.chkVULPwrU.CheckedChanged += new System.EventHandler(this.chkVULPwrU_CheckedChanged);
            // 
            // chkVURXV
            // 
            this.chkVURXV.AutoSize = true;
            this.chkVURXV.Location = new System.Drawing.Point(5, 270);
            this.chkVURXV.Name = "chkVURXV";
            this.chkVURXV.Size = new System.Drawing.Size(70, 17);
            this.chkVURXV.TabIndex = 16;
            this.chkVURXV.Text = "K8 [RXV]";
            this.chkVURXV.UseVisualStyleBackColor = true;
            this.chkVURXV.CheckedChanged += new System.EventHandler(this.chkVURXV_CheckedChanged);
            // 
            // chkVUTXU
            // 
            this.chkVUTXU.AutoSize = true;
            this.chkVUTXU.Location = new System.Drawing.Point(97, 73);
            this.chkVUTXU.Name = "chkVUTXU";
            this.chkVUTXU.Size = new System.Drawing.Size(51, 17);
            this.chkVUTXU.TabIndex = 17;
            this.chkVUTXU.Text = "TX-U";
            this.chkVUTXU.UseVisualStyleBackColor = true;
            this.chkVUTXU.CheckedChanged += new System.EventHandler(this.chkVUTXU_CheckedChanged);
            // 
            // chkVUTXV
            // 
            this.chkVUTXV.AutoSize = true;
            this.chkVUTXV.Location = new System.Drawing.Point(97, 50);
            this.chkVUTXV.Name = "chkVUTXV";
            this.chkVUTXV.Size = new System.Drawing.Size(50, 17);
            this.chkVUTXV.TabIndex = 18;
            this.chkVUTXV.Text = "TX-V";
            this.chkVUTXV.UseVisualStyleBackColor = true;
            this.chkVUTXV.CheckedChanged += new System.EventHandler(this.chkVUTXV_CheckedChanged);
            // 
            // chkVUUIFHG1
            // 
            this.chkVUUIFHG1.AutoSize = true;
            this.chkVUUIFHG1.Location = new System.Drawing.Point(101, 109);
            this.chkVUUIFHG1.Name = "chkVUUIFHG1";
            this.chkVUUIFHG1.Size = new System.Drawing.Size(93, 17);
            this.chkVUUIFHG1.TabIndex = 19;
            this.chkVUUIFHG1.Text = "K10 [U-IF HG]";
            this.chkVUUIFHG1.UseVisualStyleBackColor = true;
            this.chkVUUIFHG1.CheckedChanged += new System.EventHandler(this.chkVUUIFHG1_CheckedChanged);
            // 
            // chkVUVIFHG1
            // 
            this.chkVUVIFHG1.AutoSize = true;
            this.chkVUVIFHG1.Location = new System.Drawing.Point(5, 247);
            this.chkVUVIFHG1.Name = "chkVUVIFHG1";
            this.chkVUVIFHG1.Size = new System.Drawing.Size(86, 17);
            this.chkVUVIFHG1.TabIndex = 20;
            this.chkVUVIFHG1.Text = "K7 [V-IF HG]";
            this.chkVUVIFHG1.UseVisualStyleBackColor = true;
            this.chkVUVIFHG1.CheckedChanged += new System.EventHandler(this.chkVUVIFHG1_CheckedChanged);
            // 
            // chkVULPwrV
            // 
            this.chkVULPwrV.AutoSize = true;
            this.chkVULPwrV.Location = new System.Drawing.Point(5, 109);
            this.chkVULPwrV.Name = "chkVULPwrV";
            this.chkVULPwrV.Size = new System.Drawing.Size(90, 17);
            this.chkVULPwrV.TabIndex = 21;
            this.chkVULPwrV.Text = "K1 [V-LPWR]";
            this.chkVULPwrV.UseVisualStyleBackColor = true;
            this.chkVULPwrV.CheckedChanged += new System.EventHandler(this.chkVULPwrV_CheckedChanged);
            // 
            // chkVUUIFHG2
            // 
            this.chkVUUIFHG2.AutoSize = true;
            this.chkVUUIFHG2.Location = new System.Drawing.Point(102, 155);
            this.chkVUUIFHG2.Name = "chkVUUIFHG2";
            this.chkVUUIFHG2.Size = new System.Drawing.Size(91, 17);
            this.chkVUUIFHG2.TabIndex = 22;
            this.chkVUUIFHG2.Text = "K12 [U-IF LG]";
            this.chkVUUIFHG2.UseVisualStyleBackColor = true;
            this.chkVUUIFHG2.CheckedChanged += new System.EventHandler(this.chkVUUIFHG2_CheckedChanged);
            // 
            // chkVUVIFHG2
            // 
            this.chkVUVIFHG2.AutoSize = true;
            this.chkVUVIFHG2.Location = new System.Drawing.Point(5, 224);
            this.chkVUVIFHG2.Name = "chkVUVIFHG2";
            this.chkVUVIFHG2.Size = new System.Drawing.Size(84, 17);
            this.chkVUVIFHG2.TabIndex = 23;
            this.chkVUVIFHG2.Text = "K6 [V-IF LG]";
            this.chkVUVIFHG2.UseVisualStyleBackColor = true;
            this.chkVUVIFHG2.CheckedChanged += new System.EventHandler(this.chkVUVIFHG2_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkVUVIFHG2);
            this.groupBox1.Controls.Add(this.chkVUUIFHG2);
            this.groupBox1.Controls.Add(this.chkVULPwrV);
            this.groupBox1.Controls.Add(this.chkVUVIFHG1);
            this.groupBox1.Controls.Add(this.chkVUUIFHG1);
            this.groupBox1.Controls.Add(this.chkVUTXV);
            this.groupBox1.Controls.Add(this.chkVUTXU);
            this.groupBox1.Controls.Add(this.chkVURXV);
            this.groupBox1.Controls.Add(this.chkVULPwrU);
            this.groupBox1.Controls.Add(this.chkVUK15);
            this.groupBox1.Controls.Add(this.chkVUK18);
            this.groupBox1.Controls.Add(this.chkVUDrvV);
            this.groupBox1.Controls.Add(this.chkVUDrvU);
            this.groupBox1.Controls.Add(this.chkVUKeyVU);
            this.groupBox1.Controls.Add(this.chkVUK17);
            this.groupBox1.Controls.Add(this.chkVUK16);
            this.groupBox1.Controls.Add(this.chkVUK14);
            this.groupBox1.Controls.Add(this.chkVURXIFV);
            this.groupBox1.Controls.Add(this.chkVURX2V);
            this.groupBox1.Controls.Add(this.chkVURXURX2);
            this.groupBox1.Controls.Add(this.chkVUKeyU);
            this.groupBox1.Controls.Add(this.chkVUTXIFU);
            this.groupBox1.Controls.Add(this.chkVUKeyV);
            this.groupBox1.Controls.Add(this.chkVUFanHigh);
            this.groupBox1.Location = new System.Drawing.Point(12, 22);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(207, 325);
            this.groupBox1.TabIndex = 26;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Manual Relay Control";
            // 
            // chkVU_VIFGain
            // 
            this.chkVU_VIFGain.AutoSize = true;
            this.chkVU_VIFGain.Location = new System.Drawing.Point(17, 22);
            this.chkVU_VIFGain.Name = "chkVU_VIFGain";
            this.chkVU_VIFGain.Size = new System.Drawing.Size(60, 17);
            this.chkVU_VIFGain.TabIndex = 39;
            this.chkVU_VIFGain.Text = "IF Gain";
            this.chkVU_VIFGain.UseVisualStyleBackColor = true;
            this.chkVU_VIFGain.CheckedChanged += new System.EventHandler(this.chkVU_VIFGain_CheckedChanged);
            // 
            // chkVU_VPAEnable
            // 
            this.chkVU_VPAEnable.AutoSize = true;
            this.chkVU_VPAEnable.Location = new System.Drawing.Point(113, 22);
            this.chkVU_VPAEnable.Name = "chkVU_VPAEnable";
            this.chkVU_VPAEnable.Size = new System.Drawing.Size(76, 17);
            this.chkVU_VPAEnable.TabIndex = 40;
            this.chkVU_VPAEnable.Text = "PA Enable";
            this.chkVU_VPAEnable.UseVisualStyleBackColor = true;
            this.chkVU_VPAEnable.CheckedChanged += new System.EventHandler(this.chkVU_VPAEnable_CheckedChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.chkVRX2);
            this.groupBox4.Controls.Add(this.chkVU_VIFGain);
            this.groupBox4.Controls.Add(this.chkVU_VPAEnable);
            this.groupBox4.Location = new System.Drawing.Point(236, 22);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(290, 45);
            this.groupBox4.TabIndex = 43;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "VHF";
            // 
            // chkVRX2
            // 
            this.chkVRX2.AutoSize = true;
            this.chkVRX2.Location = new System.Drawing.Point(195, 19);
            this.chkVRX2.Name = "chkVRX2";
            this.chkVRX2.Size = new System.Drawing.Size(47, 17);
            this.chkVRX2.TabIndex = 41;
            this.chkVRX2.Text = "RX2";
            this.chkVRX2.UseVisualStyleBackColor = true;
            this.chkVRX2.CheckedChanged += new System.EventHandler(this.chkVRX2_CheckedChanged);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.chkURX2);
            this.groupBox5.Controls.Add(this.chkVU_UIFGain);
            this.groupBox5.Controls.Add(this.chkVU_UPAEnable);
            this.groupBox5.Location = new System.Drawing.Point(236, 73);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(290, 50);
            this.groupBox5.TabIndex = 44;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "UHF";
            // 
            // chkURX2
            // 
            this.chkURX2.AutoSize = true;
            this.chkURX2.Location = new System.Drawing.Point(195, 19);
            this.chkURX2.Name = "chkURX2";
            this.chkURX2.Size = new System.Drawing.Size(47, 17);
            this.chkURX2.TabIndex = 41;
            this.chkURX2.Text = "RX2";
            this.chkURX2.UseVisualStyleBackColor = true;
            this.chkURX2.CheckedChanged += new System.EventHandler(this.chkURX2_CheckedChanged);
            // 
            // chkVU_UIFGain
            // 
            this.chkVU_UIFGain.AutoSize = true;
            this.chkVU_UIFGain.Location = new System.Drawing.Point(17, 22);
            this.chkVU_UIFGain.Name = "chkVU_UIFGain";
            this.chkVU_UIFGain.Size = new System.Drawing.Size(60, 17);
            this.chkVU_UIFGain.TabIndex = 39;
            this.chkVU_UIFGain.Text = "IF Gain";
            this.chkVU_UIFGain.UseVisualStyleBackColor = true;
            this.chkVU_UIFGain.CheckedChanged += new System.EventHandler(this.chkVU_UIFGain_CheckedChanged);
            // 
            // chkVU_UPAEnable
            // 
            this.chkVU_UPAEnable.AutoSize = true;
            this.chkVU_UPAEnable.Location = new System.Drawing.Point(113, 22);
            this.chkVU_UPAEnable.Name = "chkVU_UPAEnable";
            this.chkVU_UPAEnable.Size = new System.Drawing.Size(76, 17);
            this.chkVU_UPAEnable.TabIndex = 40;
            this.chkVU_UPAEnable.Text = "PA Enable";
            this.chkVU_UPAEnable.UseVisualStyleBackColor = true;
            this.chkVU_UPAEnable.CheckedChanged += new System.EventHandler(this.chkVU_UPAEnable_CheckedChanged);
            // 
            // VUForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(560, 403);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "VUForm";
            this.Text = "VUForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.VUForm_FormClosing);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox chkVUmodeVLG1;
        private System.Windows.Forms.CheckBox chkVUmodeVHGRX1;
        private System.Windows.Forms.CheckBox chkVUmodeVHG2;
        private System.Windows.Forms.CheckBox chkVUmodeVLG2;
        private System.Windows.Forms.CheckBox chkVUmodeUHG2;
        private System.Windows.Forms.CheckBox chkVUmodeULG2;
        private System.Windows.Forms.CheckBox chkVUmodeUHG1;
        private System.Windows.Forms.CheckBox chkVUmodeULG1;
        private System.Windows.Forms.CheckBox chkVUmodeTXV60W;
        private System.Windows.Forms.CheckBox chkVUmodeTXVLP;
        private System.Windows.Forms.CheckBox chkVUmodeTXU60W;
        private System.Windows.Forms.CheckBox chkVUmodeTXULP;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox chkVUFanHigh;
        private System.Windows.Forms.CheckBox chkVUKeyV;
        private System.Windows.Forms.CheckBox chkVUTXIFU;
        private System.Windows.Forms.CheckBox chkVUKeyU;
        private System.Windows.Forms.CheckBox chkVURXURX2;
        private System.Windows.Forms.CheckBox chkVURX2V;
        private System.Windows.Forms.CheckBox chkVURXIFV;
        private System.Windows.Forms.CheckBox chkVUK14;
        private System.Windows.Forms.CheckBox chkVUK16;
        private System.Windows.Forms.CheckBox chkVUK17;
        private System.Windows.Forms.CheckBox chkVUKeyVU;
        private System.Windows.Forms.CheckBox chkVUDrvU;
        private System.Windows.Forms.CheckBox chkVUDrvV;
        public System.Windows.Forms.CheckBox chkVUK18;
        public System.Windows.Forms.CheckBox chkVUK15;
        private System.Windows.Forms.CheckBox chkVULPwrU;
        private System.Windows.Forms.CheckBox chkVURXV;
        private System.Windows.Forms.CheckBox chkVUTXU;
        private System.Windows.Forms.CheckBox chkVUTXV;
        private System.Windows.Forms.CheckBox chkVUUIFHG1;
        private System.Windows.Forms.CheckBox chkVUVIFHG1;
        private System.Windows.Forms.CheckBox chkVULPwrV;
        private System.Windows.Forms.CheckBox chkVUUIFHG2;
        private System.Windows.Forms.CheckBox chkVUVIFHG2;
        private System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.CheckBox chkVU_VIFGain;
        public System.Windows.Forms.CheckBox chkVU_VPAEnable;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        public System.Windows.Forms.CheckBox chkVU_UIFGain;
        public System.Windows.Forms.CheckBox chkVU_UPAEnable;
        public System.Windows.Forms.CheckBox chkVRX2;
        public System.Windows.Forms.CheckBox chkURX2;
    }
}