//=================================================================
// DSPTestForm.cs
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
	unsafe public class DSPTestForm : System.Windows.Forms.Form
	{
		private Console console;
		private System.Windows.Forms.CheckBox chkMu;
		private System.Windows.Forms.GroupBox grpWBIQ;
		private System.Windows.Forms.NumericUpDown udMu;
		private System.Windows.Forms.GroupBoxTS grpDSPLMSNR;
		private System.Windows.Forms.LabelTS lblLMSNRLeak;
		private System.Windows.Forms.NumericUpDownTS udLMSNRLeak;
		private System.Windows.Forms.LabelTS lblLMSNRgain;
		private System.Windows.Forms.NumericUpDownTS udLMSNRgain;
		private System.Windows.Forms.NumericUpDownTS udLMSNRdelay;
		private System.Windows.Forms.LabelTS lblLMSNRdelay;
		private System.Windows.Forms.NumericUpDownTS udLMSNRtaps;
		private System.Windows.Forms.LabelTS lblLMSNRtaps;
		private System.Windows.Forms.GroupBoxTS grpDSPLMSANF;
		private System.Windows.Forms.LabelTS lblLMSANFLeak;
		private System.Windows.Forms.NumericUpDownTS udLMSANFLeak;
		private System.Windows.Forms.LabelTS lblLMSANFgain;
		private System.Windows.Forms.NumericUpDownTS udLMSANFgain;
		private System.Windows.Forms.LabelTS lblLMSANFdelay;
		private System.Windows.Forms.NumericUpDownTS udLMSANFdelay;
		private System.Windows.Forms.LabelTS lblLMSANFTaps;
		private System.Windows.Forms.NumericUpDownTS udLMSANFtaps;
		private System.Windows.Forms.CheckBox checkBoxIQEnable;
		private System.Windows.Forms.Button btnSAMPLL;
		private System.Windows.Forms.TextBox txtSAMPLL;
		private System.Windows.Forms.TextBox txtIQWReal;
		private System.Windows.Forms.Button btnIQW;
        private System.Windows.Forms.TextBox txtIQWImag;
        private GroupBox grpRXDCBlock;
        private NumericUpDown udDCBlock;
        private CheckBox checkBoxRXDCBlockEnable;
        private CheckBox chkAudioMox;
        private GroupBoxTS grpMN;
        private NumericUpDownTS udMNFreq;
        private CheckBox checkBoxMNEnable;
        private Label NotchFreq;
		private System.ComponentModel.Container components = null;

		public DSPTestForm(Console c)
		{
			InitializeComponent();
			console = c;
		}

		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DSPTestForm));
            this.chkMu = new System.Windows.Forms.CheckBox();
            this.grpWBIQ = new System.Windows.Forms.GroupBox();
            this.checkBoxIQEnable = new System.Windows.Forms.CheckBox();
            this.udMu = new System.Windows.Forms.NumericUpDown();
            this.btnSAMPLL = new System.Windows.Forms.Button();
            this.txtSAMPLL = new System.Windows.Forms.TextBox();
            this.txtIQWReal = new System.Windows.Forms.TextBox();
            this.btnIQW = new System.Windows.Forms.Button();
            this.txtIQWImag = new System.Windows.Forms.TextBox();
            this.grpRXDCBlock = new System.Windows.Forms.GroupBox();
            this.checkBoxRXDCBlockEnable = new System.Windows.Forms.CheckBox();
            this.udDCBlock = new System.Windows.Forms.NumericUpDown();
            this.chkAudioMox = new System.Windows.Forms.CheckBox();
            this.grpMN = new System.Windows.Forms.GroupBoxTS();
            this.NotchFreq = new System.Windows.Forms.Label();
            this.udMNFreq = new System.Windows.Forms.NumericUpDownTS();
            this.checkBoxMNEnable = new System.Windows.Forms.CheckBox();
            this.grpDSPLMSNR = new System.Windows.Forms.GroupBoxTS();
            this.lblLMSNRLeak = new System.Windows.Forms.LabelTS();
            this.udLMSNRLeak = new System.Windows.Forms.NumericUpDownTS();
            this.lblLMSNRgain = new System.Windows.Forms.LabelTS();
            this.udLMSNRgain = new System.Windows.Forms.NumericUpDownTS();
            this.udLMSNRdelay = new System.Windows.Forms.NumericUpDownTS();
            this.lblLMSNRdelay = new System.Windows.Forms.LabelTS();
            this.udLMSNRtaps = new System.Windows.Forms.NumericUpDownTS();
            this.lblLMSNRtaps = new System.Windows.Forms.LabelTS();
            this.grpDSPLMSANF = new System.Windows.Forms.GroupBoxTS();
            this.lblLMSANFLeak = new System.Windows.Forms.LabelTS();
            this.udLMSANFLeak = new System.Windows.Forms.NumericUpDownTS();
            this.lblLMSANFgain = new System.Windows.Forms.LabelTS();
            this.udLMSANFgain = new System.Windows.Forms.NumericUpDownTS();
            this.lblLMSANFdelay = new System.Windows.Forms.LabelTS();
            this.udLMSANFdelay = new System.Windows.Forms.NumericUpDownTS();
            this.lblLMSANFTaps = new System.Windows.Forms.LabelTS();
            this.udLMSANFtaps = new System.Windows.Forms.NumericUpDownTS();
            this.grpWBIQ.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udMu)).BeginInit();
            this.grpRXDCBlock.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udDCBlock)).BeginInit();
            this.grpMN.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udMNFreq)).BeginInit();
            this.grpDSPLMSNR.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udLMSNRLeak)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udLMSNRgain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udLMSNRdelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udLMSNRtaps)).BeginInit();
            this.grpDSPLMSANF.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udLMSANFLeak)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udLMSANFgain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udLMSANFdelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udLMSANFtaps)).BeginInit();
            this.SuspendLayout();
            // 
            // chkMu
            // 
            this.chkMu.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkMu.Location = new System.Drawing.Point(8, 24);
            this.chkMu.Name = "chkMu";
            this.chkMu.Size = new System.Drawing.Size(48, 24);
            this.chkMu.TabIndex = 0;
            this.chkMu.Text = "Mu";
            this.chkMu.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkMu.CheckedChanged += new System.EventHandler(this.chkMu_CheckedChanged);
            // 
            // grpWBIQ
            // 
            this.grpWBIQ.Controls.Add(this.checkBoxIQEnable);
            this.grpWBIQ.Controls.Add(this.udMu);
            this.grpWBIQ.Controls.Add(this.chkMu);
            this.grpWBIQ.Location = new System.Drawing.Point(8, 8);
            this.grpWBIQ.Name = "grpWBIQ";
            this.grpWBIQ.Size = new System.Drawing.Size(272, 64);
            this.grpWBIQ.TabIndex = 1;
            this.grpWBIQ.TabStop = false;
            this.grpWBIQ.Text = "WBIQ";
            // 
            // checkBoxIQEnable
            // 
            this.checkBoxIQEnable.Location = new System.Drawing.Point(176, 23);
            this.checkBoxIQEnable.Name = "checkBoxIQEnable";
            this.checkBoxIQEnable.Size = new System.Drawing.Size(80, 24);
            this.checkBoxIQEnable.TabIndex = 2;
            this.checkBoxIQEnable.Text = "IQ disable";
            this.checkBoxIQEnable.CheckedChanged += new System.EventHandler(this.checkBoxIQEnable_CheckedChanged);
            // 
            // udMu
            // 
            this.udMu.DecimalPlaces = 8;
            this.udMu.Increment = new decimal(new int[] {
            1,
            0,
            0,
            327680});
            this.udMu.Location = new System.Drawing.Point(80, 24);
            this.udMu.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udMu.Name = "udMu";
            this.udMu.Size = new System.Drawing.Size(80, 20);
            this.udMu.TabIndex = 1;
            this.udMu.Value = new decimal(new int[] {
            5,
            0,
            0,
            196608});
            this.udMu.ValueChanged += new System.EventHandler(this.udMu_ValueChanged);
            // 
            // btnSAMPLL
            // 
            this.btnSAMPLL.Location = new System.Drawing.Point(8, 224);
            this.btnSAMPLL.Name = "btnSAMPLL";
            this.btnSAMPLL.Size = new System.Drawing.Size(75, 23);
            this.btnSAMPLL.TabIndex = 36;
            this.btnSAMPLL.Text = "SAM PLL";
            this.btnSAMPLL.Click += new System.EventHandler(this.btnSAMPLL_Click);
            // 
            // txtSAMPLL
            // 
            this.txtSAMPLL.Location = new System.Drawing.Point(96, 224);
            this.txtSAMPLL.Name = "txtSAMPLL";
            this.txtSAMPLL.Size = new System.Drawing.Size(56, 20);
            this.txtSAMPLL.TabIndex = 38;
            // 
            // txtIQWReal
            // 
            this.txtIQWReal.Location = new System.Drawing.Point(248, 216);
            this.txtIQWReal.Name = "txtIQWReal";
            this.txtIQWReal.Size = new System.Drawing.Size(56, 20);
            this.txtIQWReal.TabIndex = 40;
            // 
            // btnIQW
            // 
            this.btnIQW.Location = new System.Drawing.Point(192, 216);
            this.btnIQW.Name = "btnIQW";
            this.btnIQW.Size = new System.Drawing.Size(48, 23);
            this.btnIQW.TabIndex = 39;
            this.btnIQW.Text = "IQ W";
            this.btnIQW.Click += new System.EventHandler(this.btnIQW_Click);
            // 
            // txtIQWImag
            // 
            this.txtIQWImag.Location = new System.Drawing.Point(248, 240);
            this.txtIQWImag.Name = "txtIQWImag";
            this.txtIQWImag.Size = new System.Drawing.Size(56, 20);
            this.txtIQWImag.TabIndex = 41;
            // 
            // grpRXDCBlock
            // 
            this.grpRXDCBlock.Controls.Add(this.checkBoxRXDCBlockEnable);
            this.grpRXDCBlock.Controls.Add(this.udDCBlock);
            this.grpRXDCBlock.Location = new System.Drawing.Point(257, 80);
            this.grpRXDCBlock.Name = "grpRXDCBlock";
            this.grpRXDCBlock.Size = new System.Drawing.Size(105, 128);
            this.grpRXDCBlock.TabIndex = 44;
            this.grpRXDCBlock.TabStop = false;
            this.grpRXDCBlock.Text = "RX DC Block";
            this.grpRXDCBlock.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // checkBoxRXDCBlockEnable
            // 
            this.checkBoxRXDCBlockEnable.AutoSize = true;
            this.checkBoxRXDCBlockEnable.Location = new System.Drawing.Point(8, 31);
            this.checkBoxRXDCBlockEnable.Name = "checkBoxRXDCBlockEnable";
            this.checkBoxRXDCBlockEnable.Size = new System.Drawing.Size(83, 17);
            this.checkBoxRXDCBlockEnable.TabIndex = 44;
            this.checkBoxRXDCBlockEnable.Text = "DCB enable";
            this.checkBoxRXDCBlockEnable.UseVisualStyleBackColor = true;
            this.checkBoxRXDCBlockEnable.CheckedChanged += new System.EventHandler(this.checkBoxRXDCBlockEnable_CheckedChanged);
            // 
            // udDCBlock
            // 
            this.udDCBlock.DecimalPlaces = 8;
            this.udDCBlock.Increment = new decimal(new int[] {
            1,
            0,
            0,
            327680});
            this.udDCBlock.Location = new System.Drawing.Point(9, 65);
            this.udDCBlock.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udDCBlock.Name = "udDCBlock";
            this.udDCBlock.Size = new System.Drawing.Size(80, 20);
            this.udDCBlock.TabIndex = 43;
            this.udDCBlock.Value = new decimal(new int[] {
            5,
            0,
            0,
            196608});
            this.udDCBlock.ValueChanged += new System.EventHandler(this.udDCBlock_ValueChanged);
            // 
            // chkAudioMox
            // 
            this.chkAudioMox.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkAudioMox.Location = new System.Drawing.Point(310, 216);
            this.chkAudioMox.Name = "chkAudioMox";
            this.chkAudioMox.Size = new System.Drawing.Size(48, 38);
            this.chkAudioMox.TabIndex = 45;
            this.chkAudioMox.Text = "Audio MOX";
            this.chkAudioMox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkAudioMox.CheckedChanged += new System.EventHandler(this.chkAudioMox_CheckedChanged);
            // 
            // grpMN
            // 
            this.grpMN.Controls.Add(this.NotchFreq);
            this.grpMN.Controls.Add(this.udMNFreq);
            this.grpMN.Controls.Add(this.checkBoxMNEnable);
            this.grpMN.Location = new System.Drawing.Point(286, 8);
            this.grpMN.Name = "grpMN";
            this.grpMN.Size = new System.Drawing.Size(278, 66);
            this.grpMN.TabIndex = 46;
            this.grpMN.TabStop = false;
            this.grpMN.Text = "ManualNotch";
            // 
            // NotchFreq
            // 
            this.NotchFreq.AutoSize = true;
            this.NotchFreq.Location = new System.Drawing.Point(21, 26);
            this.NotchFreq.Name = "NotchFreq";
            this.NotchFreq.Size = new System.Drawing.Size(47, 13);
            this.NotchFreq.TabIndex = 2;
            this.NotchFreq.Text = "Freq(Hz)";
            // 
            // udMNFreq
            // 
            this.udMNFreq.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udMNFreq.Location = new System.Drawing.Point(75, 23);
            this.udMNFreq.Maximum = new decimal(new int[] {
            4000,
            0,
            0,
            0});
            this.udMNFreq.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.udMNFreq.Name = "udMNFreq";
            this.udMNFreq.Size = new System.Drawing.Size(57, 20);
            this.udMNFreq.TabIndex = 1;
            this.udMNFreq.Value = new decimal(new int[] {
            400,
            0,
            0,
            0});
            this.udMNFreq.ValueChanged += new System.EventHandler(this.udMNFreq_ValueChanged);
            // 
            // checkBoxMNEnable
            // 
            this.checkBoxMNEnable.AutoSize = true;
            this.checkBoxMNEnable.Location = new System.Drawing.Point(152, 24);
            this.checkBoxMNEnable.Name = "checkBoxMNEnable";
            this.checkBoxMNEnable.Size = new System.Drawing.Size(82, 17);
            this.checkBoxMNEnable.TabIndex = 0;
            this.checkBoxMNEnable.Text = "Man. Notch";
            this.checkBoxMNEnable.UseVisualStyleBackColor = true;
            this.checkBoxMNEnable.CheckedChanged += new System.EventHandler(this.checkBoxMNEnable_CheckedChanged);
            // 
            // grpDSPLMSNR
            // 
            this.grpDSPLMSNR.Controls.Add(this.lblLMSNRLeak);
            this.grpDSPLMSNR.Controls.Add(this.udLMSNRLeak);
            this.grpDSPLMSNR.Controls.Add(this.lblLMSNRgain);
            this.grpDSPLMSNR.Controls.Add(this.udLMSNRgain);
            this.grpDSPLMSNR.Controls.Add(this.udLMSNRdelay);
            this.grpDSPLMSNR.Controls.Add(this.lblLMSNRdelay);
            this.grpDSPLMSNR.Controls.Add(this.udLMSNRtaps);
            this.grpDSPLMSNR.Controls.Add(this.lblLMSNRtaps);
            this.grpDSPLMSNR.Location = new System.Drawing.Point(8, 80);
            this.grpDSPLMSNR.Name = "grpDSPLMSNR";
            this.grpDSPLMSNR.Size = new System.Drawing.Size(112, 128);
            this.grpDSPLMSNR.TabIndex = 35;
            this.grpDSPLMSNR.TabStop = false;
            this.grpDSPLMSNR.Text = "NR";
            // 
            // lblLMSNRLeak
            // 
            this.lblLMSNRLeak.Image = null;
            this.lblLMSNRLeak.Location = new System.Drawing.Point(8, 96);
            this.lblLMSNRLeak.Name = "lblLMSNRLeak";
            this.lblLMSNRLeak.Size = new System.Drawing.Size(40, 16);
            this.lblLMSNRLeak.TabIndex = 11;
            this.lblLMSNRLeak.Text = "Leak:";
            // 
            // udLMSNRLeak
            // 
            this.udLMSNRLeak.Enabled = false;
            this.udLMSNRLeak.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.udLMSNRLeak.Location = new System.Drawing.Point(56, 96);
            this.udLMSNRLeak.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.udLMSNRLeak.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udLMSNRLeak.Name = "udLMSNRLeak";
            this.udLMSNRLeak.Size = new System.Drawing.Size(48, 20);
            this.udLMSNRLeak.TabIndex = 10;
            this.udLMSNRLeak.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.udLMSNRLeak.ValueChanged += new System.EventHandler(this.udLMSNR_ValueChanged);
            // 
            // lblLMSNRgain
            // 
            this.lblLMSNRgain.Image = null;
            this.lblLMSNRgain.Location = new System.Drawing.Point(8, 72);
            this.lblLMSNRgain.Name = "lblLMSNRgain";
            this.lblLMSNRgain.Size = new System.Drawing.Size(40, 16);
            this.lblLMSNRgain.TabIndex = 9;
            this.lblLMSNRgain.Text = "Gain:";
            // 
            // udLMSNRgain
            // 
            this.udLMSNRgain.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.udLMSNRgain.Location = new System.Drawing.Point(56, 72);
            this.udLMSNRgain.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.udLMSNRgain.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udLMSNRgain.Name = "udLMSNRgain";
            this.udLMSNRgain.Size = new System.Drawing.Size(48, 20);
            this.udLMSNRgain.TabIndex = 7;
            this.udLMSNRgain.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.udLMSNRgain.ValueChanged += new System.EventHandler(this.udLMSNR_ValueChanged);
            // 
            // udLMSNRdelay
            // 
            this.udLMSNRdelay.Enabled = false;
            this.udLMSNRdelay.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udLMSNRdelay.Location = new System.Drawing.Point(56, 48);
            this.udLMSNRdelay.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.udLMSNRdelay.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udLMSNRdelay.Name = "udLMSNRdelay";
            this.udLMSNRdelay.Size = new System.Drawing.Size(48, 20);
            this.udLMSNRdelay.TabIndex = 6;
            this.udLMSNRdelay.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.udLMSNRdelay.ValueChanged += new System.EventHandler(this.udLMSNR_ValueChanged);
            // 
            // lblLMSNRdelay
            // 
            this.lblLMSNRdelay.Image = null;
            this.lblLMSNRdelay.Location = new System.Drawing.Point(8, 48);
            this.lblLMSNRdelay.Name = "lblLMSNRdelay";
            this.lblLMSNRdelay.Size = new System.Drawing.Size(40, 16);
            this.lblLMSNRdelay.TabIndex = 5;
            this.lblLMSNRdelay.Text = "Delay:";
            // 
            // udLMSNRtaps
            // 
            this.udLMSNRtaps.Enabled = false;
            this.udLMSNRtaps.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udLMSNRtaps.Location = new System.Drawing.Point(56, 24);
            this.udLMSNRtaps.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.udLMSNRtaps.Minimum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.udLMSNRtaps.Name = "udLMSNRtaps";
            this.udLMSNRtaps.Size = new System.Drawing.Size(48, 20);
            this.udLMSNRtaps.TabIndex = 5;
            this.udLMSNRtaps.Value = new decimal(new int[] {
            65,
            0,
            0,
            0});
            this.udLMSNRtaps.ValueChanged += new System.EventHandler(this.udLMSNR_ValueChanged);
            // 
            // lblLMSNRtaps
            // 
            this.lblLMSNRtaps.Image = null;
            this.lblLMSNRtaps.Location = new System.Drawing.Point(8, 24);
            this.lblLMSNRtaps.Name = "lblLMSNRtaps";
            this.lblLMSNRtaps.Size = new System.Drawing.Size(40, 16);
            this.lblLMSNRtaps.TabIndex = 3;
            this.lblLMSNRtaps.Text = "Taps:";
            // 
            // grpDSPLMSANF
            // 
            this.grpDSPLMSANF.Controls.Add(this.lblLMSANFLeak);
            this.grpDSPLMSANF.Controls.Add(this.udLMSANFLeak);
            this.grpDSPLMSANF.Controls.Add(this.lblLMSANFgain);
            this.grpDSPLMSANF.Controls.Add(this.udLMSANFgain);
            this.grpDSPLMSANF.Controls.Add(this.lblLMSANFdelay);
            this.grpDSPLMSANF.Controls.Add(this.udLMSANFdelay);
            this.grpDSPLMSANF.Controls.Add(this.lblLMSANFTaps);
            this.grpDSPLMSANF.Controls.Add(this.udLMSANFtaps);
            this.grpDSPLMSANF.Location = new System.Drawing.Point(128, 80);
            this.grpDSPLMSANF.Name = "grpDSPLMSANF";
            this.grpDSPLMSANF.Size = new System.Drawing.Size(120, 128);
            this.grpDSPLMSANF.TabIndex = 34;
            this.grpDSPLMSANF.TabStop = false;
            this.grpDSPLMSANF.Text = "ANF";
            // 
            // lblLMSANFLeak
            // 
            this.lblLMSANFLeak.Image = null;
            this.lblLMSANFLeak.Location = new System.Drawing.Point(8, 96);
            this.lblLMSANFLeak.Name = "lblLMSANFLeak";
            this.lblLMSANFLeak.Size = new System.Drawing.Size(40, 16);
            this.lblLMSANFLeak.TabIndex = 9;
            this.lblLMSANFLeak.Text = "Leak:";
            // 
            // udLMSANFLeak
            // 
            this.udLMSANFLeak.Enabled = false;
            this.udLMSANFLeak.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.udLMSANFLeak.Location = new System.Drawing.Point(56, 96);
            this.udLMSANFLeak.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.udLMSANFLeak.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udLMSANFLeak.Name = "udLMSANFLeak";
            this.udLMSANFLeak.Size = new System.Drawing.Size(48, 20);
            this.udLMSANFLeak.TabIndex = 8;
            this.udLMSANFLeak.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.udLMSANFLeak.ValueChanged += new System.EventHandler(this.udLMSANF_ValueChanged);
            // 
            // lblLMSANFgain
            // 
            this.lblLMSANFgain.Image = null;
            this.lblLMSANFgain.Location = new System.Drawing.Point(8, 72);
            this.lblLMSANFgain.Name = "lblLMSANFgain";
            this.lblLMSANFgain.Size = new System.Drawing.Size(40, 16);
            this.lblLMSANFgain.TabIndex = 6;
            this.lblLMSANFgain.Text = "Gain:";
            // 
            // udLMSANFgain
            // 
            this.udLMSANFgain.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.udLMSANFgain.Location = new System.Drawing.Point(56, 72);
            this.udLMSANFgain.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.udLMSANFgain.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udLMSANFgain.Name = "udLMSANFgain";
            this.udLMSANFgain.Size = new System.Drawing.Size(48, 20);
            this.udLMSANFgain.TabIndex = 3;
            this.udLMSANFgain.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.udLMSANFgain.ValueChanged += new System.EventHandler(this.udLMSANF_ValueChanged);
            // 
            // lblLMSANFdelay
            // 
            this.lblLMSANFdelay.Image = null;
            this.lblLMSANFdelay.Location = new System.Drawing.Point(8, 48);
            this.lblLMSANFdelay.Name = "lblLMSANFdelay";
            this.lblLMSANFdelay.Size = new System.Drawing.Size(40, 16);
            this.lblLMSANFdelay.TabIndex = 4;
            this.lblLMSANFdelay.Text = "Delay:";
            // 
            // udLMSANFdelay
            // 
            this.udLMSANFdelay.Enabled = false;
            this.udLMSANFdelay.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udLMSANFdelay.Location = new System.Drawing.Point(56, 48);
            this.udLMSANFdelay.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.udLMSANFdelay.Minimum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.udLMSANFdelay.Name = "udLMSANFdelay";
            this.udLMSANFdelay.Size = new System.Drawing.Size(48, 20);
            this.udLMSANFdelay.TabIndex = 2;
            this.udLMSANFdelay.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.udLMSANFdelay.ValueChanged += new System.EventHandler(this.udLMSANF_ValueChanged);
            // 
            // lblLMSANFTaps
            // 
            this.lblLMSANFTaps.Image = null;
            this.lblLMSANFTaps.Location = new System.Drawing.Point(8, 24);
            this.lblLMSANFTaps.Name = "lblLMSANFTaps";
            this.lblLMSANFTaps.Size = new System.Drawing.Size(40, 16);
            this.lblLMSANFTaps.TabIndex = 2;
            this.lblLMSANFTaps.Text = "Taps:";
            // 
            // udLMSANFtaps
            // 
            this.udLMSANFtaps.Enabled = false;
            this.udLMSANFtaps.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udLMSANFtaps.Location = new System.Drawing.Point(56, 24);
            this.udLMSANFtaps.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.udLMSANFtaps.Minimum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.udLMSANFtaps.Name = "udLMSANFtaps";
            this.udLMSANFtaps.Size = new System.Drawing.Size(48, 20);
            this.udLMSANFtaps.TabIndex = 1;
            this.udLMSANFtaps.Value = new decimal(new int[] {
            65,
            0,
            0,
            0});
            this.udLMSANFtaps.ValueChanged += new System.EventHandler(this.udLMSANF_ValueChanged);
            // 
            // DSPTestForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(567, 266);
            this.Controls.Add(this.grpMN);
            this.Controls.Add(this.chkAudioMox);
            this.Controls.Add(this.grpRXDCBlock);
            this.Controls.Add(this.txtIQWImag);
            this.Controls.Add(this.txtIQWReal);
            this.Controls.Add(this.btnIQW);
            this.Controls.Add(this.txtSAMPLL);
            this.Controls.Add(this.btnSAMPLL);
            this.Controls.Add(this.grpDSPLMSNR);
            this.Controls.Add(this.grpDSPLMSANF);
            this.Controls.Add(this.grpWBIQ);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DSPTestForm";
            this.Text = "DSP Test Form";
            this.grpWBIQ.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.udMu)).EndInit();
            this.grpRXDCBlock.ResumeLayout(false);
            this.grpRXDCBlock.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udDCBlock)).EndInit();
            this.grpMN.ResumeLayout(false);
            this.grpMN.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udMNFreq)).EndInit();
            this.grpDSPLMSNR.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.udLMSNRLeak)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udLMSNRgain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udLMSNRdelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udLMSNRtaps)).EndInit();
            this.grpDSPLMSANF.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.udLMSANFLeak)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udLMSANFgain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udLMSANFdelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udLMSANFtaps)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void chkMu_CheckedChanged(object sender, System.EventArgs e)
		{
			if(chkMu.Checked)
			{
				chkMu.BackColor = console.ButtonSelectedColor;
				DttSP.SetCorrectIQMu(0, 0, (double)udMu.Value);
			}
			else
			{
				chkMu.BackColor = SystemColors.Control;
				DttSP.SetCorrectIQMu(0, 0, 0.000);
			}
		}

		private void udMu_ValueChanged(object sender, System.EventArgs e)
		{
			if(chkMu.Checked) DttSP.SetCorrectIQMu(0, 0, (double)udMu.Value);
		}

		private void udLMSNR_ValueChanged(object sender, System.EventArgs e)
		{
			console.dsp.GetDSPRX(0, 0).SetNRVals(
				(int)udLMSNRtaps.Value,
				(int)udLMSNRdelay.Value,
				0.00001*(double)udLMSNRgain.Value,
				0.0000001*(double)udLMSNRLeak.Value);
			console.dsp.GetDSPRX(0, 1).SetNRVals(
				(int)udLMSNRtaps.Value,
				(int)udLMSNRdelay.Value,
				0.00001*(double)udLMSNRgain.Value,
				0.0000001*(double)udLMSNRLeak.Value);
		}

		private void udLMSANF_ValueChanged(object sender, System.EventArgs e)
		{
			console.dsp.GetDSPRX(0, 0).SetANFVals(
				(int)udLMSANFtaps.Value,
				(int)udLMSANFdelay.Value,
				0.00001*(double)udLMSANFgain.Value,
				0.00005);
		}

		private void checkBoxIQEnable_CheckedChanged(object sender, System.EventArgs e)
		{
			if (checkBoxIQEnable.Checked)
				DttSP.SetCorrectIQEnable(0);
			else DttSP.SetCorrectIQEnable(1);
		}

		private void btnSAMPLL_Click(object sender, System.EventArgs e)
		{
			float freq;
			DttSP.GetSAMFreq(0, 0, &freq);
			freq = (float)(freq*console.SampleRate1/(2*Math.PI));
			txtSAMPLL.Text = freq.ToString("f0");
		}

		private void btnIQW_Click(object sender, System.EventArgs e)
		{
			float real, imag;
			DttSP.GetCorrectRXIQw(0, 0, &real, &imag, 0);
			txtIQWReal.Text = real.ToString("f6");
			txtIQWImag.Text = imag.ToString("f6");
		}

        private void udDCBlock_ValueChanged(object sender, EventArgs e)
        {
            // do something to affect DC Block params here
            DttSP.SetRXDCBlockGain(0, 0, (float)udDCBlock.Value);
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void checkBoxRXDCBlockEnable_CheckedChanged(object sender, EventArgs e)
        {
            DttSP.SetRXDCBlock(0,0,checkBoxRXDCBlockEnable.Checked);

        }

        private void chkAudioMox_CheckedChanged(object sender, EventArgs e)
        {
            Audio.MOX = chkAudioMox.Checked;
        }

        private void checkBoxMNEnable_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxMNEnable.Checked)
                DttSP.SetRXManualNotchEnable(0, 0, 0, true);
            else 
                DttSP.SetRXManualNotchEnable(0, 0, 0, false);
   
        }

        private void udMNFreq_ValueChanged(object sender, EventArgs e)
        {
            DttSP.SetRXManualNotchFreq(0, 0, 0, (double)udMNFreq.Value);
        }
	}
}
