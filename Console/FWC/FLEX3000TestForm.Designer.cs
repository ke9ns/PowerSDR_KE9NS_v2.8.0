//=================================================================
// FLEX3000TestForm.cs
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
    public partial class FLEX3000TestForm : System.Windows.Forms.Form
    {
   

  
        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FLEX3000TestForm));
            this.chkPollPhaseMag = new System.Windows.Forms.CheckBox();
            this.txtPhase = new System.Windows.Forms.TextBox();
            this.lblPhase = new System.Windows.Forms.Label();
            this.lblMag = new System.Windows.Forms.Label();
            this.txtMag = new System.Windows.Forms.TextBox();
            this.picPhase = new System.Windows.Forms.PictureBox();
            this.picMag = new System.Windows.Forms.PictureBox();
            this.udFanPWMOn = new System.Windows.Forms.NumericUpDown();
            this.udFanPWMOff = new System.Windows.Forms.NumericUpDown();
            this.grpFanPWM = new System.Windows.Forms.GroupBox();
            this.udFanSpeed = new System.Windows.Forms.NumericUpDown();
            this.labelTS1 = new System.Windows.Forms.LabelTS();
            this.lblFanPWMOff = new System.Windows.Forms.LabelTS();
            this.lblFanPWMOn = new System.Windows.Forms.LabelTS();
            this.picSWR = new System.Windows.Forms.PictureBox();
            this.lblSWR = new System.Windows.Forms.Label();
            this.txtSWR = new System.Windows.Forms.TextBox();
            this.chkPollSWR = new System.Windows.Forms.CheckBox();
            this.grpATURelays = new System.Windows.Forms.GroupBoxTS();
            this.udATUC = new System.Windows.Forms.NumericUpDown();
            this.udATUL = new System.Windows.Forms.NumericUpDown();
            this.lblHiZ = new System.Windows.Forms.LabelTS();
            this.chkHiZ = new System.Windows.Forms.CheckBoxTS();
            this.lblC6 = new System.Windows.Forms.LabelTS();
            this.lblC5 = new System.Windows.Forms.LabelTS();
            this.lblC4 = new System.Windows.Forms.LabelTS();
            this.lblC3 = new System.Windows.Forms.LabelTS();
            this.lblC2 = new System.Windows.Forms.LabelTS();
            this.lblC1 = new System.Windows.Forms.LabelTS();
            this.lblC0 = new System.Windows.Forms.LabelTS();
            this.chkC3 = new System.Windows.Forms.CheckBoxTS();
            this.chkC6 = new System.Windows.Forms.CheckBoxTS();
            this.chkC1 = new System.Windows.Forms.CheckBoxTS();
            this.chkC0 = new System.Windows.Forms.CheckBoxTS();
            this.chkC5 = new System.Windows.Forms.CheckBoxTS();
            this.chkC4 = new System.Windows.Forms.CheckBoxTS();
            this.chkC2 = new System.Windows.Forms.CheckBoxTS();
            this.lblL8 = new System.Windows.Forms.LabelTS();
            this.lblL7 = new System.Windows.Forms.LabelTS();
            this.lblL6 = new System.Windows.Forms.LabelTS();
            this.lblL2 = new System.Windows.Forms.LabelTS();
            this.chkL8 = new System.Windows.Forms.CheckBoxTS();
            this.chkL2 = new System.Windows.Forms.CheckBoxTS();
            this.chkL7 = new System.Windows.Forms.CheckBoxTS();
            this.chkL6 = new System.Windows.Forms.CheckBoxTS();
            this.lblL9 = new System.Windows.Forms.LabelTS();
            this.chkL9 = new System.Windows.Forms.CheckBoxTS();
            this.chkL3 = new System.Windows.Forms.CheckBoxTS();
            this.lblL3 = new System.Windows.Forms.LabelTS();
            this.lblL4 = new System.Windows.Forms.LabelTS();
            this.chkL4 = new System.Windows.Forms.CheckBoxTS();
            this.lblL5 = new System.Windows.Forms.LabelTS();
            this.chkL5 = new System.Windows.Forms.CheckBoxTS();
            this.chkATUEnable = new System.Windows.Forms.CheckBoxTS();
            this.chkATUATTN = new System.Windows.Forms.CheckBoxTS();
            this.btnTune = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picPhase)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMag)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udFanPWMOn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udFanPWMOff)).BeginInit();
            this.grpFanPWM.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udFanSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSWR)).BeginInit();
            this.grpATURelays.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udATUC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udATUL)).BeginInit();
            this.SuspendLayout();
            // 
            // chkPollPhaseMag
            // 
            this.chkPollPhaseMag.Location = new System.Drawing.Point(192, 184);
            this.chkPollPhaseMag.Name = "chkPollPhaseMag";
            this.chkPollPhaseMag.Size = new System.Drawing.Size(48, 24);
            this.chkPollPhaseMag.TabIndex = 40;
            this.chkPollPhaseMag.Text = "Poll";
            this.chkPollPhaseMag.CheckedChanged += new System.EventHandler(this.chkPollPhaseMag_CheckedChanged);
            // 
            // txtPhase
            // 
            this.txtPhase.BackColor = System.Drawing.Color.Black;
            this.txtPhase.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPhase.ForeColor = System.Drawing.Color.White;
            this.txtPhase.Location = new System.Drawing.Point(16, 184);
            this.txtPhase.Name = "txtPhase";
            this.txtPhase.Size = new System.Drawing.Size(72, 26);
            this.txtPhase.TabIndex = 41;
            // 
            // lblPhase
            // 
            this.lblPhase.Location = new System.Drawing.Point(16, 168);
            this.lblPhase.Name = "lblPhase";
            this.lblPhase.Size = new System.Drawing.Size(72, 16);
            this.lblPhase.TabIndex = 42;
            this.lblPhase.Text = "Phase";
            this.lblPhase.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblMag
            // 
            this.lblMag.Location = new System.Drawing.Point(104, 168);
            this.lblMag.Name = "lblMag";
            this.lblMag.Size = new System.Drawing.Size(72, 16);
            this.lblMag.TabIndex = 44;
            this.lblMag.Text = "Mag";
            this.lblMag.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // txtMag
            // 
            this.txtMag.BackColor = System.Drawing.Color.Black;
            this.txtMag.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMag.ForeColor = System.Drawing.Color.White;
            this.txtMag.Location = new System.Drawing.Point(104, 184);
            this.txtMag.Name = "txtMag";
            this.txtMag.Size = new System.Drawing.Size(72, 26);
            this.txtMag.TabIndex = 43;
            // 
            // picPhase
            // 
            this.picPhase.BackColor = System.Drawing.Color.Black;
            this.picPhase.Location = new System.Drawing.Point(16, 216);
            this.picPhase.Name = "picPhase";
            this.picPhase.Size = new System.Drawing.Size(72, 16);
            this.picPhase.TabIndex = 45;
            this.picPhase.TabStop = false;
            this.picPhase.Paint += new System.Windows.Forms.PaintEventHandler(this.picPhase_Paint);
            // 
            // picMag
            // 
            this.picMag.BackColor = System.Drawing.Color.Black;
            this.picMag.Location = new System.Drawing.Point(104, 216);
            this.picMag.Name = "picMag";
            this.picMag.Size = new System.Drawing.Size(72, 16);
            this.picMag.TabIndex = 46;
            this.picMag.TabStop = false;
            this.picMag.Paint += new System.Windows.Forms.PaintEventHandler(this.picMag_Paint);
            // 
            // udFanPWMOn
            // 
            this.udFanPWMOn.Location = new System.Drawing.Point(40, 16);
            this.udFanPWMOn.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.udFanPWMOn.Name = "udFanPWMOn";
            this.udFanPWMOn.Size = new System.Drawing.Size(48, 20);
            this.udFanPWMOn.TabIndex = 66;
            this.udFanPWMOn.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.udFanPWMOn.ValueChanged += new System.EventHandler(this.udFanPWM_ValueChanged);
            // 
            // udFanPWMOff
            // 
            this.udFanPWMOff.Location = new System.Drawing.Point(40, 40);
            this.udFanPWMOff.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.udFanPWMOff.Name = "udFanPWMOff";
            this.udFanPWMOff.Size = new System.Drawing.Size(48, 20);
            this.udFanPWMOff.TabIndex = 67;
            this.udFanPWMOff.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.udFanPWMOff.ValueChanged += new System.EventHandler(this.udFanPWM_ValueChanged);
            // 
            // grpFanPWM
            // 
            this.grpFanPWM.Controls.Add(this.udFanSpeed);
            this.grpFanPWM.Controls.Add(this.labelTS1);
            this.grpFanPWM.Controls.Add(this.lblFanPWMOff);
            this.grpFanPWM.Controls.Add(this.lblFanPWMOn);
            this.grpFanPWM.Controls.Add(this.udFanPWMOn);
            this.grpFanPWM.Controls.Add(this.udFanPWMOff);
            this.grpFanPWM.Location = new System.Drawing.Point(296, 8);
            this.grpFanPWM.Name = "grpFanPWM";
            this.grpFanPWM.Size = new System.Drawing.Size(104, 96);
            this.grpFanPWM.TabIndex = 68;
            this.grpFanPWM.TabStop = false;
            this.grpFanPWM.Text = "Fan PWM";
            // 
            // udFanSpeed
            // 
            this.udFanSpeed.DecimalPlaces = 2;
            this.udFanSpeed.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.udFanSpeed.Location = new System.Drawing.Point(48, 64);
            this.udFanSpeed.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            this.udFanSpeed.Name = "udFanSpeed";
            this.udFanSpeed.Size = new System.Drawing.Size(48, 20);
            this.udFanSpeed.TabIndex = 70;
            this.udFanSpeed.ValueChanged += new System.EventHandler(this.udFanSpeed_ValueChanged);
            // 
            // labelTS1
            // 
            this.labelTS1.Image = null;
            this.labelTS1.Location = new System.Drawing.Point(8, 64);
            this.labelTS1.Name = "labelTS1";
            this.labelTS1.Size = new System.Drawing.Size(40, 16);
            this.labelTS1.TabIndex = 71;
            this.labelTS1.Text = "Speed:";
            // 
            // lblFanPWMOff
            // 
            this.lblFanPWMOff.Image = null;
            this.lblFanPWMOff.Location = new System.Drawing.Point(8, 40);
            this.lblFanPWMOff.Name = "lblFanPWMOff";
            this.lblFanPWMOff.Size = new System.Drawing.Size(24, 16);
            this.lblFanPWMOff.TabIndex = 69;
            this.lblFanPWMOff.Text = "Off:";
            // 
            // lblFanPWMOn
            // 
            this.lblFanPWMOn.Image = null;
            this.lblFanPWMOn.Location = new System.Drawing.Point(8, 16);
            this.lblFanPWMOn.Name = "lblFanPWMOn";
            this.lblFanPWMOn.Size = new System.Drawing.Size(24, 16);
            this.lblFanPWMOn.TabIndex = 68;
            this.lblFanPWMOn.Text = "On:";
            // 
            // picSWR
            // 
            this.picSWR.BackColor = System.Drawing.Color.Black;
            this.picSWR.Location = new System.Drawing.Point(264, 224);
            this.picSWR.Name = "picSWR";
            this.picSWR.Size = new System.Drawing.Size(72, 16);
            this.picSWR.TabIndex = 71;
            this.picSWR.TabStop = false;
            this.picSWR.Paint += new System.Windows.Forms.PaintEventHandler(this.picSWR_Paint);
            // 
            // lblSWR
            // 
            this.lblSWR.Location = new System.Drawing.Point(264, 176);
            this.lblSWR.Name = "lblSWR";
            this.lblSWR.Size = new System.Drawing.Size(72, 16);
            this.lblSWR.TabIndex = 70;
            this.lblSWR.Text = "SWR";
            this.lblSWR.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // txtSWR
            // 
            this.txtSWR.BackColor = System.Drawing.Color.Black;
            this.txtSWR.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSWR.ForeColor = System.Drawing.Color.White;
            this.txtSWR.Location = new System.Drawing.Point(264, 192);
            this.txtSWR.Name = "txtSWR";
            this.txtSWR.Size = new System.Drawing.Size(72, 26);
            this.txtSWR.TabIndex = 69;
            // 
            // chkPollSWR
            // 
            this.chkPollSWR.Location = new System.Drawing.Point(344, 184);
            this.chkPollSWR.Name = "chkPollSWR";
            this.chkPollSWR.Size = new System.Drawing.Size(48, 24);
            this.chkPollSWR.TabIndex = 72;
            this.chkPollSWR.Text = "Poll";
            this.chkPollSWR.CheckedChanged += new System.EventHandler(this.chkPollSWR_CheckedChanged);
            // 
            // grpATURelays
            // 
            this.grpATURelays.Controls.Add(this.udATUC);
            this.grpATURelays.Controls.Add(this.udATUL);
            this.grpATURelays.Controls.Add(this.lblHiZ);
            this.grpATURelays.Controls.Add(this.chkHiZ);
            this.grpATURelays.Controls.Add(this.lblC6);
            this.grpATURelays.Controls.Add(this.lblC5);
            this.grpATURelays.Controls.Add(this.lblC4);
            this.grpATURelays.Controls.Add(this.lblC3);
            this.grpATURelays.Controls.Add(this.lblC2);
            this.grpATURelays.Controls.Add(this.lblC1);
            this.grpATURelays.Controls.Add(this.lblC0);
            this.grpATURelays.Controls.Add(this.chkC3);
            this.grpATURelays.Controls.Add(this.chkC6);
            this.grpATURelays.Controls.Add(this.chkC1);
            this.grpATURelays.Controls.Add(this.chkC0);
            this.grpATURelays.Controls.Add(this.chkC5);
            this.grpATURelays.Controls.Add(this.chkC4);
            this.grpATURelays.Controls.Add(this.chkC2);
            this.grpATURelays.Controls.Add(this.lblL8);
            this.grpATURelays.Controls.Add(this.lblL7);
            this.grpATURelays.Controls.Add(this.lblL6);
            this.grpATURelays.Controls.Add(this.lblL2);
            this.grpATURelays.Controls.Add(this.chkL8);
            this.grpATURelays.Controls.Add(this.chkL2);
            this.grpATURelays.Controls.Add(this.chkL7);
            this.grpATURelays.Controls.Add(this.chkL6);
            this.grpATURelays.Controls.Add(this.lblL9);
            this.grpATURelays.Controls.Add(this.chkL9);
            this.grpATURelays.Controls.Add(this.chkL3);
            this.grpATURelays.Controls.Add(this.lblL3);
            this.grpATURelays.Controls.Add(this.lblL4);
            this.grpATURelays.Controls.Add(this.chkL4);
            this.grpATURelays.Controls.Add(this.lblL5);
            this.grpATURelays.Controls.Add(this.chkL5);
            this.grpATURelays.Controls.Add(this.chkATUEnable);
            this.grpATURelays.Controls.Add(this.chkATUATTN);
            this.grpATURelays.Controls.Add(this.btnTune);
            this.grpATURelays.Location = new System.Drawing.Point(8, 8);
            this.grpATURelays.Name = "grpATURelays";
            this.grpATURelays.Size = new System.Drawing.Size(280, 152);
            this.grpATURelays.TabIndex = 39;
            this.grpATURelays.TabStop = false;
            this.grpATURelays.Text = "ATU";
            // 
            // udATUC
            // 
            this.udATUC.Location = new System.Drawing.Point(216, 112);
            this.udATUC.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.udATUC.Name = "udATUC";
            this.udATUC.Size = new System.Drawing.Size(48, 20);
            this.udATUC.TabIndex = 67;
            this.udATUC.ValueChanged += new System.EventHandler(this.udATUC_ValueChanged);
            // 
            // udATUL
            // 
            this.udATUL.Location = new System.Drawing.Point(216, 64);
            this.udATUL.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.udATUL.Name = "udATUL";
            this.udATUL.Size = new System.Drawing.Size(48, 20);
            this.udATUL.TabIndex = 65;
            this.udATUL.ValueChanged += new System.EventHandler(this.udATUL_ValueChanged);
            // 
            // lblHiZ
            // 
            this.lblHiZ.Image = null;
            this.lblHiZ.Location = new System.Drawing.Point(16, 96);
            this.lblHiZ.Name = "lblHiZ";
            this.lblHiZ.Size = new System.Drawing.Size(24, 16);
            this.lblHiZ.TabIndex = 62;
            this.lblHiZ.Text = "HiZ";
            // 
            // chkHiZ
            // 
            this.chkHiZ.Image = null;
            this.chkHiZ.Location = new System.Drawing.Point(16, 112);
            this.chkHiZ.Name = "chkHiZ";
            this.chkHiZ.Size = new System.Drawing.Size(16, 24);
            this.chkHiZ.TabIndex = 61;
            this.chkHiZ.CheckedChanged += new System.EventHandler(this.chkHiZ_CheckedChanged);
            // 
            // lblC6
            // 
            this.lblC6.Image = null;
            this.lblC6.Location = new System.Drawing.Point(40, 96);
            this.lblC6.Name = "lblC6";
            this.lblC6.Size = new System.Drawing.Size(24, 16);
            this.lblC6.TabIndex = 60;
            this.lblC6.Text = "C6";
            // 
            // lblC5
            // 
            this.lblC5.Image = null;
            this.lblC5.Location = new System.Drawing.Point(64, 96);
            this.lblC5.Name = "lblC5";
            this.lblC5.Size = new System.Drawing.Size(24, 16);
            this.lblC5.TabIndex = 59;
            this.lblC5.Text = "C5";
            // 
            // lblC4
            // 
            this.lblC4.Image = null;
            this.lblC4.Location = new System.Drawing.Point(88, 96);
            this.lblC4.Name = "lblC4";
            this.lblC4.Size = new System.Drawing.Size(24, 16);
            this.lblC4.TabIndex = 58;
            this.lblC4.Text = "C4";
            // 
            // lblC3
            // 
            this.lblC3.Image = null;
            this.lblC3.Location = new System.Drawing.Point(112, 96);
            this.lblC3.Name = "lblC3";
            this.lblC3.Size = new System.Drawing.Size(24, 16);
            this.lblC3.TabIndex = 57;
            this.lblC3.Text = "C3";
            // 
            // lblC2
            // 
            this.lblC2.Image = null;
            this.lblC2.Location = new System.Drawing.Point(136, 96);
            this.lblC2.Name = "lblC2";
            this.lblC2.Size = new System.Drawing.Size(24, 16);
            this.lblC2.TabIndex = 56;
            this.lblC2.Text = "C2";
            // 
            // lblC1
            // 
            this.lblC1.Image = null;
            this.lblC1.Location = new System.Drawing.Point(160, 96);
            this.lblC1.Name = "lblC1";
            this.lblC1.Size = new System.Drawing.Size(24, 16);
            this.lblC1.TabIndex = 55;
            this.lblC1.Text = "C1";
            // 
            // lblC0
            // 
            this.lblC0.Image = null;
            this.lblC0.Location = new System.Drawing.Point(184, 96);
            this.lblC0.Name = "lblC0";
            this.lblC0.Size = new System.Drawing.Size(24, 16);
            this.lblC0.TabIndex = 54;
            this.lblC0.Text = "C0";
            // 
            // chkC3
            // 
            this.chkC3.Image = null;
            this.chkC3.Location = new System.Drawing.Point(112, 112);
            this.chkC3.Name = "chkC3";
            this.chkC3.Size = new System.Drawing.Size(16, 24);
            this.chkC3.TabIndex = 52;
            this.chkC3.CheckedChanged += new System.EventHandler(this.chkC_CheckedChanged);
            // 
            // chkC6
            // 
            this.chkC6.Image = null;
            this.chkC6.Location = new System.Drawing.Point(40, 112);
            this.chkC6.Name = "chkC6";
            this.chkC6.Size = new System.Drawing.Size(16, 24);
            this.chkC6.TabIndex = 47;
            this.chkC6.CheckedChanged += new System.EventHandler(this.chkC_CheckedChanged);
            // 
            // chkC1
            // 
            this.chkC1.Image = null;
            this.chkC1.Location = new System.Drawing.Point(160, 112);
            this.chkC1.Name = "chkC1";
            this.chkC1.Size = new System.Drawing.Size(16, 24);
            this.chkC1.TabIndex = 51;
            this.chkC1.CheckedChanged += new System.EventHandler(this.chkC_CheckedChanged);
            // 
            // chkC0
            // 
            this.chkC0.Image = null;
            this.chkC0.Location = new System.Drawing.Point(184, 112);
            this.chkC0.Name = "chkC0";
            this.chkC0.Size = new System.Drawing.Size(16, 24);
            this.chkC0.TabIndex = 50;
            this.chkC0.CheckedChanged += new System.EventHandler(this.chkC_CheckedChanged);
            // 
            // chkC5
            // 
            this.chkC5.Image = null;
            this.chkC5.Location = new System.Drawing.Point(64, 112);
            this.chkC5.Name = "chkC5";
            this.chkC5.Size = new System.Drawing.Size(16, 24);
            this.chkC5.TabIndex = 49;
            this.chkC5.CheckedChanged += new System.EventHandler(this.chkC_CheckedChanged);
            // 
            // chkC4
            // 
            this.chkC4.Image = null;
            this.chkC4.Location = new System.Drawing.Point(88, 112);
            this.chkC4.Name = "chkC4";
            this.chkC4.Size = new System.Drawing.Size(16, 24);
            this.chkC4.TabIndex = 48;
            this.chkC4.CheckedChanged += new System.EventHandler(this.chkC_CheckedChanged);
            // 
            // chkC2
            // 
            this.chkC2.Image = null;
            this.chkC2.Location = new System.Drawing.Point(136, 112);
            this.chkC2.Name = "chkC2";
            this.chkC2.Size = new System.Drawing.Size(16, 24);
            this.chkC2.TabIndex = 53;
            this.chkC2.CheckedChanged += new System.EventHandler(this.chkC_CheckedChanged);
            // 
            // lblL8
            // 
            this.lblL8.Image = null;
            this.lblL8.Location = new System.Drawing.Point(160, 48);
            this.lblL8.Name = "lblL8";
            this.lblL8.Size = new System.Drawing.Size(24, 16);
            this.lblL8.TabIndex = 44;
            this.lblL8.Text = "L8";
            // 
            // lblL7
            // 
            this.lblL7.Image = null;
            this.lblL7.Location = new System.Drawing.Point(136, 48);
            this.lblL7.Name = "lblL7";
            this.lblL7.Size = new System.Drawing.Size(24, 16);
            this.lblL7.TabIndex = 43;
            this.lblL7.Text = "L7";
            // 
            // lblL6
            // 
            this.lblL6.Image = null;
            this.lblL6.Location = new System.Drawing.Point(112, 48);
            this.lblL6.Name = "lblL6";
            this.lblL6.Size = new System.Drawing.Size(24, 16);
            this.lblL6.TabIndex = 42;
            this.lblL6.Text = "L6";
            // 
            // lblL2
            // 
            this.lblL2.Image = null;
            this.lblL2.Location = new System.Drawing.Point(16, 48);
            this.lblL2.Name = "lblL2";
            this.lblL2.Size = new System.Drawing.Size(24, 16);
            this.lblL2.TabIndex = 30;
            this.lblL2.Text = "L2";
            // 
            // chkL8
            // 
            this.chkL8.Image = null;
            this.chkL8.Location = new System.Drawing.Point(160, 64);
            this.chkL8.Name = "chkL8";
            this.chkL8.Size = new System.Drawing.Size(16, 24);
            this.chkL8.TabIndex = 23;
            this.chkL8.CheckedChanged += new System.EventHandler(this.chkL_CheckedChanged);
            // 
            // chkL2
            // 
            this.chkL2.Image = null;
            this.chkL2.Location = new System.Drawing.Point(16, 64);
            this.chkL2.Name = "chkL2";
            this.chkL2.Size = new System.Drawing.Size(16, 24);
            this.chkL2.TabIndex = 26;
            this.chkL2.CheckedChanged += new System.EventHandler(this.chkL_CheckedChanged);
            // 
            // chkL7
            // 
            this.chkL7.Image = null;
            this.chkL7.Location = new System.Drawing.Point(136, 64);
            this.chkL7.Name = "chkL7";
            this.chkL7.Size = new System.Drawing.Size(16, 24);
            this.chkL7.TabIndex = 25;
            this.chkL7.CheckedChanged += new System.EventHandler(this.chkL_CheckedChanged);
            // 
            // chkL6
            // 
            this.chkL6.Image = null;
            this.chkL6.Location = new System.Drawing.Point(112, 64);
            this.chkL6.Name = "chkL6";
            this.chkL6.Size = new System.Drawing.Size(16, 24);
            this.chkL6.TabIndex = 24;
            this.chkL6.CheckedChanged += new System.EventHandler(this.chkL_CheckedChanged);
            // 
            // lblL9
            // 
            this.lblL9.Image = null;
            this.lblL9.Location = new System.Drawing.Point(184, 48);
            this.lblL9.Name = "lblL9";
            this.lblL9.Size = new System.Drawing.Size(24, 16);
            this.lblL9.TabIndex = 46;
            this.lblL9.Text = "L9";
            // 
            // chkL9
            // 
            this.chkL9.Image = null;
            this.chkL9.Location = new System.Drawing.Point(184, 64);
            this.chkL9.Name = "chkL9";
            this.chkL9.Size = new System.Drawing.Size(16, 24);
            this.chkL9.TabIndex = 45;
            this.chkL9.CheckedChanged += new System.EventHandler(this.chkL_CheckedChanged);
            // 
            // chkL3
            // 
            this.chkL3.Image = null;
            this.chkL3.Location = new System.Drawing.Point(40, 64);
            this.chkL3.Name = "chkL3";
            this.chkL3.Size = new System.Drawing.Size(16, 24);
            this.chkL3.TabIndex = 27;
            this.chkL3.CheckedChanged += new System.EventHandler(this.chkL_CheckedChanged);
            // 
            // lblL3
            // 
            this.lblL3.Image = null;
            this.lblL3.Location = new System.Drawing.Point(40, 48);
            this.lblL3.Name = "lblL3";
            this.lblL3.Size = new System.Drawing.Size(24, 16);
            this.lblL3.TabIndex = 31;
            this.lblL3.Text = "L3";
            // 
            // lblL4
            // 
            this.lblL4.Image = null;
            this.lblL4.Location = new System.Drawing.Point(64, 48);
            this.lblL4.Name = "lblL4";
            this.lblL4.Size = new System.Drawing.Size(24, 16);
            this.lblL4.TabIndex = 32;
            this.lblL4.Text = "L4";
            // 
            // chkL4
            // 
            this.chkL4.Image = null;
            this.chkL4.Location = new System.Drawing.Point(64, 64);
            this.chkL4.Name = "chkL4";
            this.chkL4.Size = new System.Drawing.Size(16, 24);
            this.chkL4.TabIndex = 29;
            this.chkL4.CheckedChanged += new System.EventHandler(this.chkL_CheckedChanged);
            // 
            // lblL5
            // 
            this.lblL5.Image = null;
            this.lblL5.Location = new System.Drawing.Point(88, 48);
            this.lblL5.Name = "lblL5";
            this.lblL5.Size = new System.Drawing.Size(24, 16);
            this.lblL5.TabIndex = 33;
            this.lblL5.Text = "L5";
            // 
            // chkL5
            // 
            this.chkL5.Image = null;
            this.chkL5.Location = new System.Drawing.Point(88, 64);
            this.chkL5.Name = "chkL5";
            this.chkL5.Size = new System.Drawing.Size(16, 24);
            this.chkL5.TabIndex = 28;
            this.chkL5.CheckedChanged += new System.EventHandler(this.chkL_CheckedChanged);
            // 
            // chkATUEnable
            // 
            this.chkATUEnable.Image = null;
            this.chkATUEnable.Location = new System.Drawing.Point(16, 24);
            this.chkATUEnable.Name = "chkATUEnable";
            this.chkATUEnable.Size = new System.Drawing.Size(64, 16);
            this.chkATUEnable.TabIndex = 63;
            this.chkATUEnable.Text = "Enable";
            this.chkATUEnable.CheckedChanged += new System.EventHandler(this.chkATUEnable_CheckedChanged);
            // 
            // chkATUATTN
            // 
            this.chkATUATTN.Image = null;
            this.chkATUATTN.Location = new System.Drawing.Point(88, 24);
            this.chkATUATTN.Name = "chkATUATTN";
            this.chkATUATTN.Size = new System.Drawing.Size(56, 16);
            this.chkATUATTN.TabIndex = 64;
            this.chkATUATTN.Text = "ATTN";
            this.chkATUATTN.CheckedChanged += new System.EventHandler(this.chkATUATTN_CheckedChanged);
            // 
            // btnTune
            // 
            this.btnTune.Location = new System.Drawing.Point(168, 16);
            this.btnTune.Name = "btnTune";
            this.btnTune.Size = new System.Drawing.Size(75, 23);
            this.btnTune.TabIndex = 73;
            this.btnTune.Text = "Tune";
            this.btnTune.Click += new System.EventHandler(this.btnTune_Click);
            // 
            // FLEX3000TestForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(400, 257);
            this.Controls.Add(this.chkPollSWR);
            this.Controls.Add(this.picSWR);
            this.Controls.Add(this.lblSWR);
            this.Controls.Add(this.txtSWR);
            this.Controls.Add(this.grpFanPWM);
            this.Controls.Add(this.picMag);
            this.Controls.Add(this.picPhase);
            this.Controls.Add(this.lblMag);
            this.Controls.Add(this.txtMag);
            this.Controls.Add(this.txtPhase);
            this.Controls.Add(this.lblPhase);
            this.Controls.Add(this.chkPollPhaseMag);
            this.Controls.Add(this.grpATURelays);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FLEX3000TestForm";
            this.Text = "FLEX-3000 Test Form";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.FLEX3000TestForm_Closing);
            ((System.ComponentModel.ISupportInitialize)(this.picPhase)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMag)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udFanPWMOn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udFanPWMOff)).EndInit();
            this.grpFanPWM.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.udFanSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSWR)).EndInit();
            this.grpATURelays.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.udATUC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udATUL)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        #region Variable Declaration


        private System.Windows.Forms.GroupBoxTS grpATURelays;
        private System.Windows.Forms.CheckBoxTS chkATUATTN;
        private System.Windows.Forms.CheckBoxTS chkATUEnable;
        private System.Windows.Forms.LabelTS lblHiZ;
        private System.Windows.Forms.CheckBoxTS chkHiZ;
        private System.Windows.Forms.LabelTS lblC6;
        private System.Windows.Forms.LabelTS lblC5;
        private System.Windows.Forms.LabelTS lblC4;
        private System.Windows.Forms.LabelTS lblC3;
        private System.Windows.Forms.LabelTS lblC2;
        private System.Windows.Forms.LabelTS lblC1;
        private System.Windows.Forms.LabelTS lblC0;
        private System.Windows.Forms.CheckBoxTS chkC3;
        private System.Windows.Forms.CheckBoxTS chkC6;
        private System.Windows.Forms.CheckBoxTS chkC1;
        private System.Windows.Forms.CheckBoxTS chkC0;
        private System.Windows.Forms.CheckBoxTS chkC5;
        private System.Windows.Forms.CheckBoxTS chkC4;
        private System.Windows.Forms.CheckBoxTS chkC2;
        private System.Windows.Forms.LabelTS lblL8;
        private System.Windows.Forms.LabelTS lblL7;
        private System.Windows.Forms.LabelTS lblL6;
        private System.Windows.Forms.LabelTS lblL2;
        private System.Windows.Forms.CheckBoxTS chkL8;
        private System.Windows.Forms.CheckBoxTS chkL2;
        private System.Windows.Forms.CheckBoxTS chkL7;
        private System.Windows.Forms.CheckBoxTS chkL6;
        private System.Windows.Forms.LabelTS lblL9;
        private System.Windows.Forms.CheckBoxTS chkL9;
        private System.Windows.Forms.CheckBoxTS chkL3;
        private System.Windows.Forms.LabelTS lblL3;
        private System.Windows.Forms.LabelTS lblL4;
        private System.Windows.Forms.CheckBoxTS chkL4;
        private System.Windows.Forms.LabelTS lblL5;
        private System.Windows.Forms.CheckBoxTS chkL5;
        private System.Windows.Forms.NumericUpDown udATUC;
        private System.Windows.Forms.NumericUpDown udATUL;
        private System.Windows.Forms.CheckBox chkPollPhaseMag;
        private System.Windows.Forms.TextBox txtPhase;
        private System.Windows.Forms.Label lblPhase;
        private System.Windows.Forms.Label lblMag;
        private System.Windows.Forms.TextBox txtMag;
        private System.Windows.Forms.PictureBox picPhase;
        private System.Windows.Forms.PictureBox picMag;
        private System.Windows.Forms.NumericUpDown udFanPWMOn;
        private System.Windows.Forms.NumericUpDown udFanPWMOff;
        private System.Windows.Forms.GroupBox grpFanPWM;
        private System.Windows.Forms.LabelTS lblFanPWMOn;
        private System.Windows.Forms.LabelTS lblFanPWMOff;
        private System.Windows.Forms.NumericUpDown udFanSpeed;
        private System.Windows.Forms.LabelTS labelTS1;
        private System.Windows.Forms.PictureBox picSWR;
        private System.Windows.Forms.Label lblSWR;
        private System.Windows.Forms.TextBox txtSWR;
        private System.Windows.Forms.CheckBox chkPollSWR;
        private System.Windows.Forms.Button btnTune;
        private System.ComponentModel.Container components = null;

        #endregion
    }
}
