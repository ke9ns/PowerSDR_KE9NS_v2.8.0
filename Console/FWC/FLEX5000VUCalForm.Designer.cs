//=================================================================
// FLEX5000VUCalForm.Designer.cs
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
    partial class FLEX5000VUCalForm
    {
        private System.ComponentModel.IContainer components = null;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FLEX5000VUCalForm));
            this.lstDebug = new System.Windows.Forms.ListBox();
            this.lblPATarget = new System.Windows.Forms.Label();
            this.udPATarget = new System.Windows.Forms.NumericUpDown();
            this.udSensorPad = new System.Windows.Forms.NumericUpDown();
            this.lblSensorPad = new System.Windows.Forms.Label();
            this.lblVBoxLoss = new System.Windows.Forms.Label();
            this.udVBoxLoss = new System.Windows.Forms.NumericUpDown();
            this.lblUBoxLoss = new System.Windows.Forms.Label();
            this.udUBoxLoss = new System.Windows.Forms.NumericUpDown();
            this.udPAHPTargetTolHigh = new System.Windows.Forms.NumericUpDown();
            this.lblPAHPTargetTolHigh = new System.Windows.Forms.Label();
            this.udPAHPTargetTolLow = new System.Windows.Forms.NumericUpDown();
            this.lblPAHPTargetTolLow = new System.Windows.Forms.Label();
            this.udPAHPTarget = new System.Windows.Forms.NumericUpDown();
            this.lblPAHPTarget = new System.Windows.Forms.Label();
            this.udPALPTargetTolLow = new System.Windows.Forms.NumericUpDown();
            this.lblPALPTargetTolLow = new System.Windows.Forms.Label();
            this.udPALPTargetTolHigh = new System.Windows.Forms.NumericUpDown();
            this.lblPALPTargetTolHigh = new System.Windows.Forms.Label();
            this.lblMaxRadioVolume = new System.Windows.Forms.Label();
            this.udMaxRadioVolume = new System.Windows.Forms.NumericUpDown();
            this.lblLevelU = new System.Windows.Forms.Label();
            this.udLevelU = new System.Windows.Forms.NumericUpDown();
            this.lblLevelV = new System.Windows.Forms.Label();
            this.udLevelV = new System.Windows.Forms.NumericUpDown();
            this.udAttempts = new System.Windows.Forms.NumericUpDown();
            this.lblAttampts = new System.Windows.Forms.Label();
            this.lblRadioVolumeStarting = new System.Windows.Forms.Label();
            this.udRadioVolumeStarting = new System.Windows.Forms.NumericUpDown();
            this.ckPwrRemap = new System.Windows.Forms.CheckBoxTS();
            this.btnTXCarrier = new System.Windows.Forms.ButtonTS();
            this.chkHighPowerCal = new System.Windows.Forms.CheckBoxTS();
            this.btnPA = new System.Windows.Forms.ButtonTS();
            this.btnLevel = new System.Windows.Forms.ButtonTS();
            this.grpBands = new System.Windows.Forms.GroupBoxTS();
            this.ck70cm = new System.Windows.Forms.CheckBoxTS();
            this.ck2m = new System.Windows.Forms.CheckBoxTS();
            ((System.ComponentModel.ISupportInitialize)(this.udPATarget)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udSensorPad)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udVBoxLoss)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udUBoxLoss)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPAHPTargetTolHigh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPAHPTargetTolLow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPAHPTarget)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPALPTargetTolLow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPALPTargetTolHigh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udMaxRadioVolume)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udLevelU)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udLevelV)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udAttempts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udRadioVolumeStarting)).BeginInit();
            this.grpBands.SuspendLayout();
            this.SuspendLayout();
            // 
            // lstDebug
            // 
            this.lstDebug.HorizontalScrollbar = true;
            this.lstDebug.Location = new System.Drawing.Point(107, 79);
            this.lstDebug.Name = "lstDebug";
            this.lstDebug.Size = new System.Drawing.Size(289, 160);
            this.lstDebug.TabIndex = 20;
            // 
            // lblPATarget
            // 
            this.lblPATarget.AutoSize = true;
            this.lblPATarget.Location = new System.Drawing.Point(9, 252);
            this.lblPATarget.Name = "lblPATarget";
            this.lblPATarget.Size = new System.Drawing.Size(101, 13);
            this.lblPATarget.TabIndex = 22;
            this.lblPATarget.Text = "PA LP Target (dBm)";
            this.lblPATarget.Visible = false;
            // 
            // udPATarget
            // 
            this.udPATarget.DecimalPlaces = 1;
            this.udPATarget.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.udPATarget.Location = new System.Drawing.Point(134, 248);
            this.udPATarget.Name = "udPATarget";
            this.udPATarget.Size = new System.Drawing.Size(45, 20);
            this.udPATarget.TabIndex = 23;
            this.udPATarget.Value = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.udPATarget.Visible = false;
            // 
            // udSensorPad
            // 
            this.udSensorPad.DecimalPlaces = 1;
            this.udSensorPad.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.udSensorPad.Location = new System.Drawing.Point(134, 392);
            this.udSensorPad.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.udSensorPad.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.udSensorPad.Name = "udSensorPad";
            this.udSensorPad.Size = new System.Drawing.Size(45, 20);
            this.udSensorPad.TabIndex = 24;
            this.udSensorPad.Value = new decimal(new int[] {
            30,
            0,
            0,
            -2147483648});
            this.udSensorPad.Visible = false;
            // 
            // lblSensorPad
            // 
            this.lblSensorPad.AutoSize = true;
            this.lblSensorPad.Location = new System.Drawing.Point(46, 394);
            this.lblSensorPad.Name = "lblSensorPad";
            this.lblSensorPad.Size = new System.Drawing.Size(84, 13);
            this.lblSensorPad.TabIndex = 25;
            this.lblSensorPad.Text = "Sensor Pad (dB)";
            this.lblSensorPad.Visible = false;
            // 
            // lblVBoxLoss
            // 
            this.lblVBoxLoss.AutoSize = true;
            this.lblVBoxLoss.Location = new System.Drawing.Point(46, 346);
            this.lblVBoxLoss.Name = "lblVBoxLoss";
            this.lblVBoxLoss.Size = new System.Drawing.Size(82, 13);
            this.lblVBoxLoss.TabIndex = 29;
            this.lblVBoxLoss.Text = "V Box Loss (dB)";
            this.lblVBoxLoss.Visible = false;
            // 
            // udVBoxLoss
            // 
            this.udVBoxLoss.DecimalPlaces = 1;
            this.udVBoxLoss.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.udVBoxLoss.Location = new System.Drawing.Point(134, 342);
            this.udVBoxLoss.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.udVBoxLoss.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.udVBoxLoss.Name = "udVBoxLoss";
            this.udVBoxLoss.Size = new System.Drawing.Size(45, 20);
            this.udVBoxLoss.TabIndex = 28;
            this.udVBoxLoss.Value = new decimal(new int[] {
            2,
            0,
            0,
            -2147418112});
            this.udVBoxLoss.Visible = false;
            // 
            // lblUBoxLoss
            // 
            this.lblUBoxLoss.AutoSize = true;
            this.lblUBoxLoss.Location = new System.Drawing.Point(47, 370);
            this.lblUBoxLoss.Name = "lblUBoxLoss";
            this.lblUBoxLoss.Size = new System.Drawing.Size(83, 13);
            this.lblUBoxLoss.TabIndex = 31;
            this.lblUBoxLoss.Text = "U Box Loss (dB)";
            this.lblUBoxLoss.Visible = false;
            // 
            // udUBoxLoss
            // 
            this.udUBoxLoss.DecimalPlaces = 1;
            this.udUBoxLoss.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.udUBoxLoss.Location = new System.Drawing.Point(134, 366);
            this.udUBoxLoss.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.udUBoxLoss.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.udUBoxLoss.Name = "udUBoxLoss";
            this.udUBoxLoss.Size = new System.Drawing.Size(45, 20);
            this.udUBoxLoss.TabIndex = 30;
            this.udUBoxLoss.Value = new decimal(new int[] {
            4,
            0,
            0,
            -2147418112});
            this.udUBoxLoss.Visible = false;
            // 
            // udPAHPTargetTolHigh
            // 
            this.udPAHPTargetTolHigh.DecimalPlaces = 2;
            this.udPAHPTargetTolHigh.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.udPAHPTargetTolHigh.Location = new System.Drawing.Point(331, 272);
            this.udPAHPTargetTolHigh.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.udPAHPTargetTolHigh.Name = "udPAHPTargetTolHigh";
            this.udPAHPTargetTolHigh.Size = new System.Drawing.Size(45, 20);
            this.udPAHPTargetTolHigh.TabIndex = 33;
            this.udPAHPTargetTolHigh.Visible = false;
            // 
            // lblPAHPTargetTolHigh
            // 
            this.lblPAHPTargetTolHigh.AutoSize = true;
            this.lblPAHPTargetTolHigh.Location = new System.Drawing.Point(206, 276);
            this.lblPAHPTargetTolHigh.Name = "lblPAHPTargetTolHigh";
            this.lblPAHPTargetTolHigh.Size = new System.Drawing.Size(122, 13);
            this.lblPAHPTargetTolHigh.TabIndex = 32;
            this.lblPAHPTargetTolHigh.Text = "PA Target Tol High (HP)";
            this.lblPAHPTargetTolHigh.Visible = false;
            // 
            // udPAHPTargetTolLow
            // 
            this.udPAHPTargetTolLow.DecimalPlaces = 2;
            this.udPAHPTargetTolLow.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.udPAHPTargetTolLow.Location = new System.Drawing.Point(331, 295);
            this.udPAHPTargetTolLow.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.udPAHPTargetTolLow.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147418112});
            this.udPAHPTargetTolLow.Name = "udPAHPTargetTolLow";
            this.udPAHPTargetTolLow.Size = new System.Drawing.Size(45, 20);
            this.udPAHPTargetTolLow.TabIndex = 35;
            this.udPAHPTargetTolLow.Value = new decimal(new int[] {
            1,
            0,
            0,
            -2147418112});
            this.udPAHPTargetTolLow.Visible = false;
            // 
            // lblPAHPTargetTolLow
            // 
            this.lblPAHPTargetTolLow.AutoSize = true;
            this.lblPAHPTargetTolLow.Location = new System.Drawing.Point(206, 299);
            this.lblPAHPTargetTolLow.Name = "lblPAHPTargetTolLow";
            this.lblPAHPTargetTolLow.Size = new System.Drawing.Size(120, 13);
            this.lblPAHPTargetTolLow.TabIndex = 34;
            this.lblPAHPTargetTolLow.Text = "PA Target Tol Low (HP)";
            this.lblPAHPTargetTolLow.Visible = false;
            // 
            // udPAHPTarget
            // 
            this.udPAHPTarget.DecimalPlaces = 1;
            this.udPAHPTarget.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.udPAHPTarget.Location = new System.Drawing.Point(330, 248);
            this.udPAHPTarget.Name = "udPAHPTarget";
            this.udPAHPTarget.Size = new System.Drawing.Size(45, 20);
            this.udPAHPTarget.TabIndex = 38;
            this.udPAHPTarget.Value = new decimal(new int[] {
            484,
            0,
            0,
            65536});
            this.udPAHPTarget.Visible = false;
            // 
            // lblPAHPTarget
            // 
            this.lblPAHPTarget.AutoSize = true;
            this.lblPAHPTarget.Location = new System.Drawing.Point(205, 252);
            this.lblPAHPTarget.Name = "lblPAHPTarget";
            this.lblPAHPTarget.Size = new System.Drawing.Size(103, 13);
            this.lblPAHPTarget.TabIndex = 37;
            this.lblPAHPTarget.Text = "PA HP Target (dBm)";
            this.lblPAHPTarget.Visible = false;
            // 
            // udPALPTargetTolLow
            // 
            this.udPALPTargetTolLow.DecimalPlaces = 2;
            this.udPALPTargetTolLow.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.udPALPTargetTolLow.Location = new System.Drawing.Point(133, 295);
            this.udPALPTargetTolLow.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.udPALPTargetTolLow.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147418112});
            this.udPALPTargetTolLow.Name = "udPALPTargetTolLow";
            this.udPALPTargetTolLow.Size = new System.Drawing.Size(45, 20);
            this.udPALPTargetTolLow.TabIndex = 42;
            this.udPALPTargetTolLow.Value = new decimal(new int[] {
            2,
            0,
            0,
            -2147418112});
            this.udPALPTargetTolLow.Visible = false;
            // 
            // lblPALPTargetTolLow
            // 
            this.lblPALPTargetTolLow.AutoSize = true;
            this.lblPALPTargetTolLow.Location = new System.Drawing.Point(8, 299);
            this.lblPALPTargetTolLow.Name = "lblPALPTargetTolLow";
            this.lblPALPTargetTolLow.Size = new System.Drawing.Size(118, 13);
            this.lblPALPTargetTolLow.TabIndex = 41;
            this.lblPALPTargetTolLow.Text = "PA Target Tol Low (LP)";
            this.lblPALPTargetTolLow.Visible = false;
            // 
            // udPALPTargetTolHigh
            // 
            this.udPALPTargetTolHigh.DecimalPlaces = 2;
            this.udPALPTargetTolHigh.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.udPALPTargetTolHigh.Location = new System.Drawing.Point(133, 272);
            this.udPALPTargetTolHigh.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.udPALPTargetTolHigh.Name = "udPALPTargetTolHigh";
            this.udPALPTargetTolHigh.Size = new System.Drawing.Size(45, 20);
            this.udPALPTargetTolHigh.TabIndex = 40;
            this.udPALPTargetTolHigh.Value = new decimal(new int[] {
            2,
            0,
            0,
            65536});
            this.udPALPTargetTolHigh.Visible = false;
            // 
            // lblPALPTargetTolHigh
            // 
            this.lblPALPTargetTolHigh.AutoSize = true;
            this.lblPALPTargetTolHigh.Location = new System.Drawing.Point(8, 276);
            this.lblPALPTargetTolHigh.Name = "lblPALPTargetTolHigh";
            this.lblPALPTargetTolHigh.Size = new System.Drawing.Size(120, 13);
            this.lblPALPTargetTolHigh.TabIndex = 39;
            this.lblPALPTargetTolHigh.Text = "PA Target Tol High (LP)";
            this.lblPALPTargetTolHigh.Visible = false;
            // 
            // lblMaxRadioVolume
            // 
            this.lblMaxRadioVolume.AutoSize = true;
            this.lblMaxRadioVolume.Location = new System.Drawing.Point(32, 451);
            this.lblMaxRadioVolume.Name = "lblMaxRadioVolume";
            this.lblMaxRadioVolume.Size = new System.Drawing.Size(96, 13);
            this.lblMaxRadioVolume.TabIndex = 44;
            this.lblMaxRadioVolume.Text = "Max Radio Volume";
            this.lblMaxRadioVolume.Visible = false;
            // 
            // udMaxRadioVolume
            // 
            this.udMaxRadioVolume.DecimalPlaces = 2;
            this.udMaxRadioVolume.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.udMaxRadioVolume.Location = new System.Drawing.Point(134, 449);
            this.udMaxRadioVolume.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udMaxRadioVolume.Name = "udMaxRadioVolume";
            this.udMaxRadioVolume.Size = new System.Drawing.Size(45, 20);
            this.udMaxRadioVolume.TabIndex = 43;
            this.udMaxRadioVolume.Value = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            this.udMaxRadioVolume.Visible = false;
            // 
            // lblLevelU
            // 
            this.lblLevelU.AutoSize = true;
            this.lblLevelU.Location = new System.Drawing.Point(246, 415);
            this.lblLevelU.Name = "lblLevelU";
            this.lblLevelU.Size = new System.Drawing.Size(44, 13);
            this.lblLevelU.TabIndex = 48;
            this.lblLevelU.Text = "U Level";
            this.lblLevelU.Visible = false;
            // 
            // udLevelU
            // 
            this.udLevelU.DecimalPlaces = 1;
            this.udLevelU.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.udLevelU.Location = new System.Drawing.Point(296, 411);
            this.udLevelU.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.udLevelU.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.udLevelU.Name = "udLevelU";
            this.udLevelU.Size = new System.Drawing.Size(45, 20);
            this.udLevelU.TabIndex = 47;
            this.udLevelU.Value = new decimal(new int[] {
            610,
            0,
            0,
            -2147418112});
            this.udLevelU.Visible = false;
            // 
            // lblLevelV
            // 
            this.lblLevelV.AutoSize = true;
            this.lblLevelV.Location = new System.Drawing.Point(247, 390);
            this.lblLevelV.Name = "lblLevelV";
            this.lblLevelV.Size = new System.Drawing.Size(43, 13);
            this.lblLevelV.TabIndex = 46;
            this.lblLevelV.Text = "V Level";
            this.lblLevelV.Visible = false;
            // 
            // udLevelV
            // 
            this.udLevelV.DecimalPlaces = 1;
            this.udLevelV.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.udLevelV.Location = new System.Drawing.Point(296, 387);
            this.udLevelV.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.udLevelV.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.udLevelV.Name = "udLevelV";
            this.udLevelV.Size = new System.Drawing.Size(45, 20);
            this.udLevelV.TabIndex = 45;
            this.udLevelV.Value = new decimal(new int[] {
            640,
            0,
            0,
            -2147418112});
            this.udLevelV.Visible = false;
            // 
            // udAttempts
            // 
            this.udAttempts.Location = new System.Drawing.Point(333, 338);
            this.udAttempts.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.udAttempts.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udAttempts.Name = "udAttempts";
            this.udAttempts.Size = new System.Drawing.Size(45, 20);
            this.udAttempts.TabIndex = 51;
            this.udAttempts.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.udAttempts.Visible = false;
            // 
            // lblAttampts
            // 
            this.lblAttampts.AutoSize = true;
            this.lblAttampts.Location = new System.Drawing.Point(208, 342);
            this.lblAttampts.Name = "lblAttampts";
            this.lblAttampts.Size = new System.Drawing.Size(100, 13);
            this.lblAttampts.TabIndex = 50;
            this.lblAttampts.Text = "Calibration Attempts";
            this.lblAttampts.Visible = false;
            // 
            // lblRadioVolumeStarting
            // 
            this.lblRadioVolumeStarting.AutoSize = true;
            this.lblRadioVolumeStarting.Location = new System.Drawing.Point(15, 428);
            this.lblRadioVolumeStarting.Name = "lblRadioVolumeStarting";
            this.lblRadioVolumeStarting.Size = new System.Drawing.Size(112, 13);
            this.lblRadioVolumeStarting.TabIndex = 53;
            this.lblRadioVolumeStarting.Text = "Starting Radio Volume";
            this.lblRadioVolumeStarting.Visible = false;
            // 
            // udRadioVolumeStarting
            // 
            this.udRadioVolumeStarting.DecimalPlaces = 2;
            this.udRadioVolumeStarting.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.udRadioVolumeStarting.Location = new System.Drawing.Point(134, 426);
            this.udRadioVolumeStarting.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udRadioVolumeStarting.Name = "udRadioVolumeStarting";
            this.udRadioVolumeStarting.Size = new System.Drawing.Size(45, 20);
            this.udRadioVolumeStarting.TabIndex = 52;
            this.udRadioVolumeStarting.Value = new decimal(new int[] {
            2,
            0,
            0,
            131072});
            this.udRadioVolumeStarting.Visible = false;
            // 
            // ckPwrRemap
            // 
            this.ckPwrRemap.Checked = true;
            this.ckPwrRemap.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckPwrRemap.Image = null;
            this.ckPwrRemap.Location = new System.Drawing.Point(250, 440);
            this.ckPwrRemap.Name = "ckPwrRemap";
            this.ckPwrRemap.Size = new System.Drawing.Size(125, 37);
            this.ckPwrRemap.TabIndex = 54;
            this.ckPwrRemap.Text = "Power Remap Curve Enabled";
            this.ckPwrRemap.Visible = false;
            this.ckPwrRemap.CheckedChanged += new System.EventHandler(this.chkPwrRemap_CheckedChanged);
            // 
            // btnTXCarrier
            // 
            this.btnTXCarrier.Image = null;
            this.btnTXCarrier.Location = new System.Drawing.Point(12, 184);
            this.btnTXCarrier.Name = "btnTXCarrier";
            this.btnTXCarrier.Size = new System.Drawing.Size(75, 23);
            this.btnTXCarrier.TabIndex = 49;
            this.btnTXCarrier.Text = "TX Carrier";
            this.btnTXCarrier.UseVisualStyleBackColor = true;
            this.btnTXCarrier.Click += new System.EventHandler(this.btnTXCarrier_Click);
            // 
            // chkHighPowerCal
            // 
            this.chkHighPowerCal.Checked = true;
            this.chkHighPowerCal.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkHighPowerCal.Image = null;
            this.chkHighPowerCal.Location = new System.Drawing.Point(11, 141);
            this.chkHighPowerCal.Name = "chkHighPowerCal";
            this.chkHighPowerCal.Size = new System.Drawing.Size(82, 37);
            this.chkHighPowerCal.TabIndex = 36;
            this.chkHighPowerCal.Text = "High Power Calibration";
            // 
            // btnPA
            // 
            this.btnPA.Enabled = false;
            this.btnPA.Image = null;
            this.btnPA.Location = new System.Drawing.Point(12, 112);
            this.btnPA.Name = "btnPA";
            this.btnPA.Size = new System.Drawing.Size(75, 23);
            this.btnPA.TabIndex = 19;
            this.btnPA.Text = "PA";
            this.btnPA.UseVisualStyleBackColor = true;
            this.btnPA.Click += new System.EventHandler(this.btnPA_Click);
            // 
            // btnLevel
            // 
            this.btnLevel.Enabled = false;
            this.btnLevel.Image = null;
            this.btnLevel.Location = new System.Drawing.Point(12, 83);
            this.btnLevel.Name = "btnLevel";
            this.btnLevel.Size = new System.Drawing.Size(75, 23);
            this.btnLevel.TabIndex = 18;
            this.btnLevel.Text = "Level";
            this.btnLevel.UseVisualStyleBackColor = true;
            this.btnLevel.Click += new System.EventHandler(this.btnLevel_Click);
            // 
            // grpBands
            // 
            this.grpBands.Controls.Add(this.ck70cm);
            this.grpBands.Controls.Add(this.ck2m);
            this.grpBands.Location = new System.Drawing.Point(12, 12);
            this.grpBands.Name = "grpBands";
            this.grpBands.Size = new System.Drawing.Size(340, 43);
            this.grpBands.TabIndex = 17;
            this.grpBands.TabStop = false;
            this.grpBands.Text = "Bands";
            // 
            // ck70cm
            // 
            this.ck70cm.Checked = true;
            this.ck70cm.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ck70cm.Image = null;
            this.ck70cm.Location = new System.Drawing.Point(175, 16);
            this.ck70cm.Name = "ck70cm";
            this.ck70cm.Size = new System.Drawing.Size(143, 24);
            this.ck70cm.TabIndex = 19;
            this.ck70cm.Text = "70cm (430-450 MHz)";
            // 
            // ck2m
            // 
            this.ck2m.Checked = true;
            this.ck2m.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ck2m.Image = null;
            this.ck2m.Location = new System.Drawing.Point(16, 16);
            this.ck2m.Name = "ck2m";
            this.ck2m.Size = new System.Drawing.Size(143, 24);
            this.ck2m.TabIndex = 18;
            this.ck2m.Text = "2m (144-148 MHz)";
            // 
            // FLEX5000VUCalForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(408, 486);
            this.Controls.Add(this.ckPwrRemap);
            this.Controls.Add(this.lblRadioVolumeStarting);
            this.Controls.Add(this.udRadioVolumeStarting);
            this.Controls.Add(this.udAttempts);
            this.Controls.Add(this.lblAttampts);
            this.Controls.Add(this.btnTXCarrier);
            this.Controls.Add(this.lblLevelU);
            this.Controls.Add(this.udLevelU);
            this.Controls.Add(this.lblLevelV);
            this.Controls.Add(this.udLevelV);
            this.Controls.Add(this.lblMaxRadioVolume);
            this.Controls.Add(this.udMaxRadioVolume);
            this.Controls.Add(this.udPALPTargetTolLow);
            this.Controls.Add(this.lblPALPTargetTolLow);
            this.Controls.Add(this.udPALPTargetTolHigh);
            this.Controls.Add(this.lblPALPTargetTolHigh);
            this.Controls.Add(this.udPAHPTarget);
            this.Controls.Add(this.lblPAHPTarget);
            this.Controls.Add(this.chkHighPowerCal);
            this.Controls.Add(this.udPAHPTargetTolLow);
            this.Controls.Add(this.lblPAHPTargetTolLow);
            this.Controls.Add(this.udPAHPTargetTolHigh);
            this.Controls.Add(this.lblPAHPTargetTolHigh);
            this.Controls.Add(this.lblUBoxLoss);
            this.Controls.Add(this.udUBoxLoss);
            this.Controls.Add(this.lblVBoxLoss);
            this.Controls.Add(this.udVBoxLoss);
            this.Controls.Add(this.lblSensorPad);
            this.Controls.Add(this.udSensorPad);
            this.Controls.Add(this.udPATarget);
            this.Controls.Add(this.lblPATarget);
            this.Controls.Add(this.lstDebug);
            this.Controls.Add(this.btnPA);
            this.Controls.Add(this.btnLevel);
            this.Controls.Add(this.grpBands);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "FLEX5000VUCalForm";
            this.Text = "FLEX-5000 VU Test";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FLEX5000VUCalForm_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.udPATarget)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udSensorPad)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udVBoxLoss)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udUBoxLoss)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPAHPTargetTolHigh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPAHPTargetTolLow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPAHPTarget)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPALPTargetTolLow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPALPTargetTolHigh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udMaxRadioVolume)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udLevelU)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udLevelV)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udAttempts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udRadioVolumeStarting)).EndInit();
            this.grpBands.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBoxTS grpBands;
        private System.Windows.Forms.CheckBoxTS ck70cm;
        private System.Windows.Forms.CheckBoxTS ck2m;
        private System.Windows.Forms.ButtonTS btnLevel;
        private System.Windows.Forms.ButtonTS btnPA;
        private System.Windows.Forms.ListBox lstDebug;
        private System.Windows.Forms.Label lblPATarget;
        private System.Windows.Forms.NumericUpDown udPATarget;
        private System.Windows.Forms.NumericUpDown udSensorPad;
        private System.Windows.Forms.Label lblSensorPad;
        private System.Windows.Forms.Label lblVBoxLoss;
        private System.Windows.Forms.NumericUpDown udVBoxLoss;
        private System.Windows.Forms.Label lblUBoxLoss;
        private System.Windows.Forms.NumericUpDown udUBoxLoss;
        private System.Windows.Forms.NumericUpDown udPAHPTargetTolHigh;
        private System.Windows.Forms.Label lblPAHPTargetTolHigh;
        private System.Windows.Forms.NumericUpDown udPAHPTargetTolLow;
        private System.Windows.Forms.Label lblPAHPTargetTolLow;
        private System.Windows.Forms.CheckBoxTS chkHighPowerCal;
        private System.Windows.Forms.NumericUpDown udPAHPTarget;
        private System.Windows.Forms.Label lblPAHPTarget;
        private System.Windows.Forms.NumericUpDown udPALPTargetTolLow;
        private System.Windows.Forms.Label lblPALPTargetTolLow;
        private System.Windows.Forms.NumericUpDown udPALPTargetTolHigh;
        private System.Windows.Forms.Label lblPALPTargetTolHigh;
        private System.Windows.Forms.Label lblMaxRadioVolume;
        private System.Windows.Forms.NumericUpDown udMaxRadioVolume;
        private System.Windows.Forms.Label lblLevelU;
        private System.Windows.Forms.NumericUpDown udLevelU;
        private System.Windows.Forms.Label lblLevelV;
        private System.Windows.Forms.NumericUpDown udLevelV;
        private System.Windows.Forms.ButtonTS btnTXCarrier;
        private System.Windows.Forms.NumericUpDown udAttempts;
        private System.Windows.Forms.Label lblAttampts;
        private System.Windows.Forms.Label lblRadioVolumeStarting;
        private System.Windows.Forms.NumericUpDown udRadioVolumeStarting;
        private System.Windows.Forms.CheckBoxTS ckPwrRemap;
    }
}