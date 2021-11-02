//=================================================================
// FLEX3000ATUForm.cs
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

   

    public partial class FLEX3000ATUForm : System.Windows.Forms.Form
    {
        

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FLEX3000ATUForm));
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.chckForceRetune = new System.Windows.Forms.CheckBox();
            this.swrTargetUpDown = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.retuneSWRupdown = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.udHighSWR = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.chkAutoMode = new System.Windows.Forms.CheckBoxTS();
            this.resetATUdatabase = new System.Windows.Forms.ButtonTS();
            this.btnAntProfileAdd = new System.Windows.Forms.ButtonTS();
            this.comboAntProfileName = new System.Windows.Forms.ComboBoxTS();
            this.btnAntProfileDelete = new System.Windows.Forms.ButtonTS();
            this.btnAntProfileRename = new System.Windows.Forms.ButtonTS();
            this.grpFeedback = new System.Windows.Forms.GroupBoxTS();
            this.lblHiZ = new System.Windows.Forms.Label();
            this.lblTunRefPow = new System.Windows.Forms.LabelTS();
            this.lblBypRefPow = new System.Windows.Forms.LabelTS();
            this.lblTunFwdPow = new System.Windows.Forms.LabelTS();
            this.lblBypFwdPow = new System.Windows.Forms.LabelTS();
            this.lblTunSWR = new System.Windows.Forms.LabelTS();
            this.lblBypSWR = new System.Windows.Forms.LabelTS();
            this.labelTS1 = new System.Windows.Forms.LabelTS();
            this.lblByp = new System.Windows.Forms.LabelTS();
            this.lblSWR = new System.Windows.Forms.LabelTS();
            this.lblReflected = new System.Windows.Forms.LabelTS();
            this.lblForward = new System.Windows.Forms.LabelTS();
            this.lblTuneComplete = new System.Windows.Forms.LabelTS();
            this.rdBypass = new System.Windows.Forms.RadioButton();
            this.rdTune = new System.Windows.Forms.RadioButton();
            this.lblC = new System.Windows.Forms.Label();
            this.lblL = new System.Windows.Forms.Label();
            this.lblTime = new System.Windows.Forms.Label();
            this.udSleepTime = new System.Windows.Forms.NumericUpDown();
            this.udOffSleep = new System.Windows.Forms.NumericUpDown();
            this.chkDoNotPress = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.udTunPower = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.rdStop = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.MemoryTune = new System.Windows.Forms.Label();
            this.grpAntennaProfile = new System.Windows.Forms.GroupBoxTS();
            this.grpRefPow = new System.Windows.Forms.GroupBoxTS();
            this.picRefPow = new System.Windows.Forms.PictureBox();
            this.txtRefPow = new System.Windows.Forms.TextBox();
            this.grpFwdPow = new System.Windows.Forms.GroupBoxTS();
            this.picFwdPow = new System.Windows.Forms.PictureBox();
            this.txtFwdPow = new System.Windows.Forms.TextBox();
            this.grpSWR = new System.Windows.Forms.GroupBoxTS();
            this.picSWR = new System.Windows.Forms.PictureBox();
            this.txtSWR = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.swrTargetUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.retuneSWRupdown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udHighSWR)).BeginInit();
            this.grpFeedback.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udSleepTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udOffSleep)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udTunPower)).BeginInit();
            this.grpAntennaProfile.SuspendLayout();
            this.grpRefPow.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picRefPow)).BeginInit();
            this.grpFwdPow.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picFwdPow)).BeginInit();
            this.grpSWR.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSWR)).BeginInit();
            this.SuspendLayout();
            // 
            // chckForceRetune
            // 
            this.chckForceRetune.AutoSize = true;
            this.chckForceRetune.Location = new System.Drawing.Point(143, 10);
            this.chckForceRetune.Name = "chckForceRetune";
            this.chckForceRetune.Size = new System.Drawing.Size(91, 17);
            this.chckForceRetune.TabIndex = 97;
            this.chckForceRetune.Text = "Force Retune";
            this.toolTip1.SetToolTip(this.chckForceRetune, "The ATU will ignore past tune memory and will do a full tune when the \"Tune\" butt" +
        "on is pressed ");
            this.chckForceRetune.UseVisualStyleBackColor = true;
            this.chckForceRetune.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // swrTargetUpDown
            // 
            this.swrTargetUpDown.DecimalPlaces = 1;
            this.swrTargetUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.swrTargetUpDown.Location = new System.Drawing.Point(125, 279);
            this.swrTargetUpDown.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            65536});
            this.swrTargetUpDown.Minimum = new decimal(new int[] {
            11,
            0,
            0,
            65536});
            this.swrTargetUpDown.Name = "swrTargetUpDown";
            this.swrTargetUpDown.Size = new System.Drawing.Size(51, 20);
            this.swrTargetUpDown.TabIndex = 99;
            this.toolTip1.SetToolTip(this.swrTargetUpDown, "The ATU stop tuning when an SWR less than this number is found.  Higher numbers m" +
        "ay speed up tuning time.");
            this.swrTargetUpDown.Value = new decimal(new int[] {
            11,
            0,
            0,
            65536});
            this.swrTargetUpDown.ValueChanged += new System.EventHandler(this.control_swr_thresh_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(24, 281);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 13);
            this.label5.TabIndex = 100;
            this.label5.Text = "SWR Target";
            this.toolTip1.SetToolTip(this.label5, "The ATU will stop tuning when an SWR less than this number is found.  Higher numb" +
        "ers may speed up tuning time.");
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // retuneSWRupdown
            // 
            this.retuneSWRupdown.AccessibleName = "";
            this.retuneSWRupdown.DecimalPlaces = 1;
            this.retuneSWRupdown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.retuneSWRupdown.Location = new System.Drawing.Point(125, 314);
            this.retuneSWRupdown.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            65536});
            this.retuneSWRupdown.Minimum = new decimal(new int[] {
            11,
            0,
            0,
            65536});
            this.retuneSWRupdown.Name = "retuneSWRupdown";
            this.retuneSWRupdown.Size = new System.Drawing.Size(51, 20);
            this.retuneSWRupdown.TabIndex = 101;
            this.toolTip1.SetToolTip(this.retuneSWRupdown, "The tune will be considered successful and will be stored in memory if the tuned " +
        "SWR is less than this number");
            this.retuneSWRupdown.Value = new decimal(new int[] {
            210,
            0,
            0,
            131072});
            this.retuneSWRupdown.ValueChanged += new System.EventHandler(this.retuneSWRupdown_ValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(24, 316);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(88, 13);
            this.label6.TabIndex = 102;
            this.label6.Text = "SWR Successful";
            this.toolTip1.SetToolTip(this.label6, "The tune will be considered successful and will be stored in memory if the tuned " +
        "SWR is less than this number");
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // udHighSWR
            // 
            this.udHighSWR.DecimalPlaces = 1;
            this.udHighSWR.Enabled = false;
            this.udHighSWR.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.udHighSWR.Location = new System.Drawing.Point(125, 247);
            this.udHighSWR.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            65536});
            this.udHighSWR.Minimum = new decimal(new int[] {
            20,
            0,
            0,
            65536});
            this.udHighSWR.Name = "udHighSWR";
            this.udHighSWR.Size = new System.Drawing.Size(51, 20);
            this.udHighSWR.TabIndex = 85;
            this.toolTip1.SetToolTip(this.udHighSWR, "The ATU will not attempt to tune when the SWR is greater than this number");
            this.udHighSWR.Value = new decimal(new int[] {
            70,
            0,
            0,
            65536});
            this.udHighSWR.ValueChanged += new System.EventHandler(this.udHighSWR_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 249);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 13);
            this.label3.TabIndex = 90;
            this.label3.Text = "SWR Attempt Limit";
            this.toolTip1.SetToolTip(this.label3, "The ATU will not attempt to tune when the SWR is greater than this number");
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(187, 250);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(64, 17);
            this.checkBox1.TabIndex = 103;
            this.checkBox1.Text = "No Limit";
            this.toolTip1.SetToolTip(this.checkBox1, "The ATU will attempt to tune any SWR");
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged_1);
            // 
            // chkAutoMode
            // 
            this.chkAutoMode.AutoSize = true;
            this.chkAutoMode.Image = null;
            this.chkAutoMode.Location = new System.Drawing.Point(143, 33);
            this.chkAutoMode.Name = "chkAutoMode";
            this.chkAutoMode.Size = new System.Drawing.Size(78, 17);
            this.chkAutoMode.TabIndex = 106;
            this.chkAutoMode.Text = "Auto Mode";
            this.toolTip1.SetToolTip(this.chkAutoMode, "Enable automatic tuning when switching to a band to a different band");
            this.chkAutoMode.UseVisualStyleBackColor = true;
            this.chkAutoMode.CheckedChanged += new System.EventHandler(this.chkAutoMode_CheckedChanged);
            // 
            // resetATUdatabase
            // 
            this.resetATUdatabase.Image = null;
            this.resetATUdatabase.Location = new System.Drawing.Point(18, 428);
            this.resetATUdatabase.Name = "resetATUdatabase";
            this.resetATUdatabase.Size = new System.Drawing.Size(233, 21);
            this.resetATUdatabase.TabIndex = 96;
            this.resetATUdatabase.Text = "Reset ATU Database";
            this.toolTip1.SetToolTip(this.resetATUdatabase, "Click to reset the ATU Databsase");
            this.resetATUdatabase.Click += new System.EventHandler(this.resetATUdatabase_Click);
            // 
            // btnAntProfileAdd
            // 
            this.btnAntProfileAdd.Image = null;
            this.btnAntProfileAdd.Location = new System.Drawing.Point(12, 46);
            this.btnAntProfileAdd.Name = "btnAntProfileAdd";
            this.btnAntProfileAdd.Size = new System.Drawing.Size(66, 21);
            this.btnAntProfileAdd.TabIndex = 95;
            this.btnAntProfileAdd.Text = "Add";
            this.toolTip1.SetToolTip(this.btnAntProfileAdd, "Click to save the current settings to an Antenna Profile.");
            this.btnAntProfileAdd.Click += new System.EventHandler(this.btnAntProfileAdd_Click);
            // 
            // comboAntProfileName
            // 
            this.comboAntProfileName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboAntProfileName.DropDownWidth = 104;
            this.comboAntProfileName.Location = new System.Drawing.Point(12, 16);
            this.comboAntProfileName.Name = "comboAntProfileName";
            this.comboAntProfileName.Size = new System.Drawing.Size(209, 21);
            this.comboAntProfileName.TabIndex = 0;
            this.toolTip1.SetToolTip(this.comboAntProfileName, "Sets the current Antenna Profile to be used.");
            this.comboAntProfileName.SelectedIndexChanged += new System.EventHandler(this.antennaProfileName_SelectedIndexChanged);
            // 
            // btnAntProfileDelete
            // 
            this.btnAntProfileDelete.Image = null;
            this.btnAntProfileDelete.Location = new System.Drawing.Point(83, 46);
            this.btnAntProfileDelete.Name = "btnAntProfileDelete";
            this.btnAntProfileDelete.Size = new System.Drawing.Size(66, 21);
            this.btnAntProfileDelete.TabIndex = 2;
            this.btnAntProfileDelete.Text = "Delete";
            this.toolTip1.SetToolTip(this.btnAntProfileDelete, "Click to delete the currently selected Antenna Profile.");
            this.btnAntProfileDelete.Click += new System.EventHandler(this.btnAntProfileDelete_Click);
            // 
            // btnAntProfileRename
            // 
            this.btnAntProfileRename.Image = null;
            this.btnAntProfileRename.Location = new System.Drawing.Point(155, 46);
            this.btnAntProfileRename.Name = "btnAntProfileRename";
            this.btnAntProfileRename.Size = new System.Drawing.Size(66, 21);
            this.btnAntProfileRename.TabIndex = 1;
            this.btnAntProfileRename.Text = "Rename";
            this.toolTip1.SetToolTip(this.btnAntProfileRename, "Click to rename the selected profile");
            this.btnAntProfileRename.Click += new System.EventHandler(this.btnAntProfileSave_Click);
            // 
            // grpFeedback
            // 
            this.grpFeedback.BackColor = System.Drawing.SystemColors.Control;
            this.grpFeedback.Controls.Add(this.lblHiZ);
            this.grpFeedback.Controls.Add(this.lblTunRefPow);
            this.grpFeedback.Controls.Add(this.lblBypRefPow);
            this.grpFeedback.Controls.Add(this.lblTunFwdPow);
            this.grpFeedback.Controls.Add(this.lblBypFwdPow);
            this.grpFeedback.Controls.Add(this.lblTunSWR);
            this.grpFeedback.Controls.Add(this.lblBypSWR);
            this.grpFeedback.Controls.Add(this.labelTS1);
            this.grpFeedback.Controls.Add(this.lblByp);
            this.grpFeedback.Controls.Add(this.lblSWR);
            this.grpFeedback.Controls.Add(this.lblReflected);
            this.grpFeedback.Controls.Add(this.lblForward);
            this.grpFeedback.Controls.Add(this.lblTuneComplete);
            this.grpFeedback.Location = new System.Drawing.Point(18, 144);
            this.grpFeedback.Name = "grpFeedback";
            this.grpFeedback.Size = new System.Drawing.Size(233, 90);
            this.grpFeedback.TabIndex = 5;
            this.grpFeedback.TabStop = false;
            this.grpFeedback.Text = "Tuner Results";
            this.toolTip1.SetToolTip(this.grpFeedback, "The information in this window is returned from the ATU after a tune sequence.");
            // 
            // lblHiZ
            // 
            this.lblHiZ.BackColor = System.Drawing.SystemColors.ControlText;
            this.lblHiZ.Enabled = false;
            this.lblHiZ.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHiZ.ForeColor = System.Drawing.Color.White;
            this.lblHiZ.Location = new System.Drawing.Point(29, 38);
            this.lblHiZ.Name = "lblHiZ";
            this.lblHiZ.Size = new System.Drawing.Size(32, 16);
            this.lblHiZ.TabIndex = 16;
            this.lblHiZ.Text = "Hi Z";
            this.lblHiZ.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblHiZ.Visible = false;
            // 
            // lblTunRefPow
            // 
            this.lblTunRefPow.Image = null;
            this.lblTunRefPow.Location = new System.Drawing.Point(136, 113);
            this.lblTunRefPow.Name = "lblTunRefPow";
            this.lblTunRefPow.Size = new System.Drawing.Size(56, 16);
            this.lblTunRefPow.TabIndex = 15;
            this.lblTunRefPow.Text = "2.3";
            this.lblTunRefPow.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblBypRefPow
            // 
            this.lblBypRefPow.Image = null;
            this.lblBypRefPow.Location = new System.Drawing.Point(72, 113);
            this.lblBypRefPow.Name = "lblBypRefPow";
            this.lblBypRefPow.Size = new System.Drawing.Size(56, 16);
            this.lblBypRefPow.TabIndex = 14;
            this.lblBypRefPow.Text = "2.3";
            this.lblBypRefPow.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblTunFwdPow
            // 
            this.lblTunFwdPow.Image = null;
            this.lblTunFwdPow.Location = new System.Drawing.Point(136, 89);
            this.lblTunFwdPow.Name = "lblTunFwdPow";
            this.lblTunFwdPow.Size = new System.Drawing.Size(56, 16);
            this.lblTunFwdPow.TabIndex = 13;
            this.lblTunFwdPow.Text = "2.3";
            this.lblTunFwdPow.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblBypFwdPow
            // 
            this.lblBypFwdPow.Image = null;
            this.lblBypFwdPow.Location = new System.Drawing.Point(72, 89);
            this.lblBypFwdPow.Name = "lblBypFwdPow";
            this.lblBypFwdPow.Size = new System.Drawing.Size(56, 16);
            this.lblBypFwdPow.TabIndex = 12;
            this.lblBypFwdPow.Text = "2.3";
            this.lblBypFwdPow.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblTunSWR
            // 
            this.lblTunSWR.Image = null;
            this.lblTunSWR.Location = new System.Drawing.Point(136, 65);
            this.lblTunSWR.Name = "lblTunSWR";
            this.lblTunSWR.Size = new System.Drawing.Size(56, 16);
            this.lblTunSWR.TabIndex = 11;
            this.lblTunSWR.Text = "1.3 : 1";
            this.lblTunSWR.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblBypSWR
            // 
            this.lblBypSWR.Image = null;
            this.lblBypSWR.Location = new System.Drawing.Point(72, 65);
            this.lblBypSWR.Name = "lblBypSWR";
            this.lblBypSWR.Size = new System.Drawing.Size(56, 16);
            this.lblBypSWR.TabIndex = 10;
            this.lblBypSWR.Text = "1.3 : 1";
            this.lblBypSWR.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // labelTS1
            // 
            this.labelTS1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTS1.Image = null;
            this.labelTS1.Location = new System.Drawing.Point(136, 41);
            this.labelTS1.Name = "labelTS1";
            this.labelTS1.Size = new System.Drawing.Size(56, 16);
            this.labelTS1.TabIndex = 9;
            this.labelTS1.Text = "Tuned";
            this.labelTS1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblByp
            // 
            this.lblByp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblByp.Image = null;
            this.lblByp.Location = new System.Drawing.Point(67, 41);
            this.lblByp.Name = "lblByp";
            this.lblByp.Size = new System.Drawing.Size(63, 16);
            this.lblByp.TabIndex = 8;
            this.lblByp.Text = "Bypassed";
            this.lblByp.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblSWR
            // 
            this.lblSWR.Image = null;
            this.lblSWR.Location = new System.Drawing.Point(8, 65);
            this.lblSWR.Name = "lblSWR";
            this.lblSWR.Size = new System.Drawing.Size(56, 16);
            this.lblSWR.TabIndex = 3;
            this.lblSWR.Text = "SWR:";
            this.lblSWR.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblReflected
            // 
            this.lblReflected.Image = null;
            this.lblReflected.Location = new System.Drawing.Point(8, 113);
            this.lblReflected.Name = "lblReflected";
            this.lblReflected.Size = new System.Drawing.Size(56, 16);
            this.lblReflected.TabIndex = 2;
            this.lblReflected.Text = "Ref Pow:";
            this.lblReflected.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblForward
            // 
            this.lblForward.Image = null;
            this.lblForward.Location = new System.Drawing.Point(8, 89);
            this.lblForward.Name = "lblForward";
            this.lblForward.Size = new System.Drawing.Size(56, 16);
            this.lblForward.TabIndex = 6;
            this.lblForward.Text = "Fwd Pow:";
            this.lblForward.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblTuneComplete
            // 
            this.lblTuneComplete.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTuneComplete.ForeColor = System.Drawing.Color.Black;
            this.lblTuneComplete.Image = null;
            this.lblTuneComplete.Location = new System.Drawing.Point(6, 16);
            this.lblTuneComplete.Name = "lblTuneComplete";
            this.lblTuneComplete.Size = new System.Drawing.Size(224, 16);
            this.lblTuneComplete.TabIndex = 7;
            this.lblTuneComplete.Text = "Tuner Bypassed";
            this.lblTuneComplete.Click += new System.EventHandler(this.lblTuneComplete_Click);
            // 
            // rdBypass
            // 
            this.rdBypass.Appearance = System.Windows.Forms.Appearance.Button;
            this.rdBypass.Location = new System.Drawing.Point(17, 46);
            this.rdBypass.Name = "rdBypass";
            this.rdBypass.Size = new System.Drawing.Size(104, 24);
            this.rdBypass.TabIndex = 79;
            this.rdBypass.Text = "Bypass";
            this.rdBypass.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rdBypass.CheckedChanged += new System.EventHandler(this.rdBypass_CheckedChanged);
            // 
            // rdTune
            // 
            this.rdTune.Appearance = System.Windows.Forms.Appearance.Button;
            this.rdTune.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdTune.Location = new System.Drawing.Point(17, 14);
            this.rdTune.Name = "rdTune";
            this.rdTune.Size = new System.Drawing.Size(104, 24);
            this.rdTune.TabIndex = 80;
            this.rdTune.Text = "Tune";
            this.rdTune.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rdTune.CheckedChanged += new System.EventHandler(this.rdTune_CheckedChanged);
            // 
            // lblC
            // 
            this.lblC.Location = new System.Drawing.Point(557, 258);
            this.lblC.Name = "lblC";
            this.lblC.Size = new System.Drawing.Size(40, 23);
            this.lblC.TabIndex = 81;
            this.lblC.Text = "C: ";
            // 
            // lblL
            // 
            this.lblL.Location = new System.Drawing.Point(509, 258);
            this.lblL.Name = "lblL";
            this.lblL.Size = new System.Drawing.Size(40, 23);
            this.lblL.TabIndex = 82;
            this.lblL.Text = "L: ";
            // 
            // lblTime
            // 
            this.lblTime.Location = new System.Drawing.Point(460, 111);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(64, 16);
            this.lblTime.TabIndex = 83;
            this.lblTime.Text = "Time: ";
            // 
            // udSleepTime
            // 
            this.udSleepTime.Location = new System.Drawing.Point(524, 111);
            this.udSleepTime.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.udSleepTime.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.udSleepTime.Name = "udSleepTime";
            this.udSleepTime.Size = new System.Drawing.Size(56, 20);
            this.udSleepTime.TabIndex = 84;
            this.udSleepTime.Value = new decimal(new int[] {
            200,
            0,
            0,
            0});
            // 
            // udOffSleep
            // 
            this.udOffSleep.Location = new System.Drawing.Point(644, 111);
            this.udOffSleep.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.udOffSleep.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.udOffSleep.Name = "udOffSleep";
            this.udOffSleep.Size = new System.Drawing.Size(48, 20);
            this.udOffSleep.TabIndex = 86;
            this.udOffSleep.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // chkDoNotPress
            // 
            this.chkDoNotPress.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkDoNotPress.Location = new System.Drawing.Point(509, 282);
            this.chkDoNotPress.Name = "chkDoNotPress";
            this.chkDoNotPress.Size = new System.Drawing.Size(104, 24);
            this.chkDoNotPress.TabIndex = 87;
            this.chkDoNotPress.Text = "Long Test";
            this.chkDoNotPress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkDoNotPress.CheckedChanged += new System.EventHandler(this.chkDoNotPress_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(641, 141);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 88;
            this.label1.Text = "Off Sleep";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(521, 141);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 89;
            this.label2.Text = "Settle Time";
            // 
            // udTunPower
            // 
            this.udTunPower.Location = new System.Drawing.Point(524, 171);
            this.udTunPower.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udTunPower.Name = "udTunPower";
            this.udTunPower.Size = new System.Drawing.Size(48, 20);
            this.udTunPower.TabIndex = 91;
            this.udTunPower.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(525, 201);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 13);
            this.label4.TabIndex = 92;
            this.label4.Text = "Tune Power";
            // 
            // rdStop
            // 
            this.rdStop.Appearance = System.Windows.Forms.Appearance.Button;
            this.rdStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdStop.ForeColor = System.Drawing.Color.Maroon;
            this.rdStop.Location = new System.Drawing.Point(17, 79);
            this.rdStop.Name = "rdStop";
            this.rdStop.Size = new System.Drawing.Size(104, 24);
            this.rdStop.TabIndex = 98;
            this.rdStop.Text = "Stop";
            this.rdStop.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rdStop.CheckedChanged += new System.EventHandler(this.rdStop_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(509, 321);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 104;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            // 
            // MemoryTune
            // 
            this.MemoryTune.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.MemoryTune.AutoSize = true;
            this.MemoryTune.ForeColor = System.Drawing.Color.DarkGreen;
            this.MemoryTune.Location = new System.Drawing.Point(146, 57);
            this.MemoryTune.Name = "MemoryTune";
            this.MemoryTune.Size = new System.Drawing.Size(78, 13);
            this.MemoryTune.TabIndex = 105;
            this.MemoryTune.Text = "-Memory Tune-";
            // 
            // grpAntennaProfile
            // 
            this.grpAntennaProfile.Controls.Add(this.btnAntProfileAdd);
            this.grpAntennaProfile.Controls.Add(this.comboAntProfileName);
            this.grpAntennaProfile.Controls.Add(this.btnAntProfileDelete);
            this.grpAntennaProfile.Controls.Add(this.btnAntProfileRename);
            this.grpAntennaProfile.Location = new System.Drawing.Point(18, 344);
            this.grpAntennaProfile.Name = "grpAntennaProfile";
            this.grpAntennaProfile.Size = new System.Drawing.Size(233, 75);
            this.grpAntennaProfile.TabIndex = 93;
            this.grpAntennaProfile.TabStop = false;
            this.grpAntennaProfile.Text = "Antenna Profiles";
            this.grpAntennaProfile.Enter += new System.EventHandler(this.grpAntennaProfile_Enter);
            // 
            // grpRefPow
            // 
            this.grpRefPow.Controls.Add(this.picRefPow);
            this.grpRefPow.Controls.Add(this.txtRefPow);
            this.grpRefPow.Location = new System.Drawing.Point(626, 298);
            this.grpRefPow.Name = "grpRefPow";
            this.grpRefPow.Size = new System.Drawing.Size(88, 64);
            this.grpRefPow.TabIndex = 78;
            this.grpRefPow.TabStop = false;
            this.grpRefPow.Text = "Ref Pow";
            // 
            // picRefPow
            // 
            this.picRefPow.BackColor = System.Drawing.Color.Black;
            this.picRefPow.Location = new System.Drawing.Point(9, 43);
            this.picRefPow.Name = "picRefPow";
            this.picRefPow.Size = new System.Drawing.Size(69, 16);
            this.picRefPow.TabIndex = 77;
            this.picRefPow.TabStop = false;
            this.picRefPow.Paint += new System.Windows.Forms.PaintEventHandler(this.picRefPow_Paint);
            // 
            // txtRefPow
            // 
            this.txtRefPow.BackColor = System.Drawing.Color.Black;
            this.txtRefPow.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRefPow.ForeColor = System.Drawing.Color.White;
            this.txtRefPow.Location = new System.Drawing.Point(8, 16);
            this.txtRefPow.Name = "txtRefPow";
            this.txtRefPow.Size = new System.Drawing.Size(72, 26);
            this.txtRefPow.TabIndex = 75;
            // 
            // grpFwdPow
            // 
            this.grpFwdPow.Controls.Add(this.picFwdPow);
            this.grpFwdPow.Controls.Add(this.txtFwdPow);
            this.grpFwdPow.Location = new System.Drawing.Point(626, 222);
            this.grpFwdPow.Name = "grpFwdPow";
            this.grpFwdPow.Size = new System.Drawing.Size(88, 64);
            this.grpFwdPow.TabIndex = 75;
            this.grpFwdPow.TabStop = false;
            this.grpFwdPow.Text = "Fwd Pow";
            // 
            // picFwdPow
            // 
            this.picFwdPow.BackColor = System.Drawing.Color.Black;
            this.picFwdPow.Location = new System.Drawing.Point(9, 43);
            this.picFwdPow.Name = "picFwdPow";
            this.picFwdPow.Size = new System.Drawing.Size(69, 16);
            this.picFwdPow.TabIndex = 77;
            this.picFwdPow.TabStop = false;
            this.picFwdPow.Paint += new System.Windows.Forms.PaintEventHandler(this.picFwdPow_Paint);
            // 
            // txtFwdPow
            // 
            this.txtFwdPow.BackColor = System.Drawing.Color.Black;
            this.txtFwdPow.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFwdPow.ForeColor = System.Drawing.Color.White;
            this.txtFwdPow.Location = new System.Drawing.Point(8, 16);
            this.txtFwdPow.Name = "txtFwdPow";
            this.txtFwdPow.Size = new System.Drawing.Size(72, 26);
            this.txtFwdPow.TabIndex = 75;
            // 
            // grpSWR
            // 
            this.grpSWR.Controls.Add(this.picSWR);
            this.grpSWR.Controls.Add(this.txtSWR);
            this.grpSWR.Location = new System.Drawing.Point(142, 73);
            this.grpSWR.Name = "grpSWR";
            this.grpSWR.Size = new System.Drawing.Size(88, 64);
            this.grpSWR.TabIndex = 7;
            this.grpSWR.TabStop = false;
            this.grpSWR.Text = "SWR";
            // 
            // picSWR
            // 
            this.picSWR.BackColor = System.Drawing.Color.Black;
            this.picSWR.Location = new System.Drawing.Point(9, 43);
            this.picSWR.Name = "picSWR";
            this.picSWR.Size = new System.Drawing.Size(69, 16);
            this.picSWR.TabIndex = 74;
            this.picSWR.TabStop = false;
            this.picSWR.Paint += new System.Windows.Forms.PaintEventHandler(this.picSWR_Paint);
            // 
            // txtSWR
            // 
            this.txtSWR.BackColor = System.Drawing.Color.Black;
            this.txtSWR.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSWR.ForeColor = System.Drawing.Color.White;
            this.txtSWR.Location = new System.Drawing.Point(8, 16);
            this.txtSWR.Name = "txtSWR";
            this.txtSWR.ReadOnly = true;
            this.txtSWR.Size = new System.Drawing.Size(72, 26);
            this.txtSWR.TabIndex = 99999999;
            this.txtSWR.TextChanged += new System.EventHandler(this.txtSWR_TextChanged);
            // 
            // FLEX3000ATUForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(267, 463);
            this.Controls.Add(this.chkAutoMode);
            this.Controls.Add(this.MemoryTune);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.retuneSWRupdown);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.swrTargetUpDown);
            this.Controls.Add(this.rdStop);
            this.Controls.Add(this.chckForceRetune);
            this.Controls.Add(this.resetATUdatabase);
            this.Controls.Add(this.grpAntennaProfile);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.udTunPower);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkDoNotPress);
            this.Controls.Add(this.udOffSleep);
            this.Controls.Add(this.udHighSWR);
            this.Controls.Add(this.udSleepTime);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.lblL);
            this.Controls.Add(this.lblC);
            this.Controls.Add(this.rdTune);
            this.Controls.Add(this.rdBypass);
            this.Controls.Add(this.grpRefPow);
            this.Controls.Add(this.grpFwdPow);
            this.Controls.Add(this.grpSWR);
            this.Controls.Add(this.grpFeedback);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(283, 502);
            this.MinimumSize = new System.Drawing.Size(283, 502);
            this.Name = "FLEX3000ATUForm";
            this.Text = "FLEX-3000 ATU Settings";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.FWCATUForm_Closing);
            this.Load += new System.EventHandler(this.FLEX3000ATUForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.swrTargetUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.retuneSWRupdown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udHighSWR)).EndInit();
            this.grpFeedback.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.udSleepTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udOffSleep)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udTunPower)).EndInit();
            this.grpAntennaProfile.ResumeLayout(false);
            this.grpRefPow.ResumeLayout(false);
            this.grpRefPow.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picRefPow)).EndInit();
            this.grpFwdPow.ResumeLayout(false);
            this.grpFwdPow.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picFwdPow)).EndInit();
            this.grpSWR.ResumeLayout(false);
            this.grpSWR.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSWR)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

     
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBoxTS grpFeedback;
        private System.Windows.Forms.LabelTS lblReflected;
        private System.Windows.Forms.LabelTS lblSWR;
        private System.Windows.Forms.LabelTS lblForward;
        private System.Windows.Forms.LabelTS lblTuneComplete;
        private System.Windows.Forms.LabelTS lblByp;
        private System.Windows.Forms.LabelTS labelTS1;
        private System.Windows.Forms.LabelTS lblBypSWR;
        private System.Windows.Forms.LabelTS lblTunSWR;
        private System.Windows.Forms.LabelTS lblBypFwdPow;
        private System.Windows.Forms.LabelTS lblTunFwdPow;
        private System.Windows.Forms.LabelTS lblBypRefPow;
        private System.Windows.Forms.LabelTS lblTunRefPow;
        private System.Windows.Forms.PictureBox picSWR;
        private System.Windows.Forms.TextBox txtSWR;
        private System.Windows.Forms.PictureBox picFwdPow;
        private System.Windows.Forms.TextBox txtFwdPow;
        private System.Windows.Forms.GroupBoxTS grpSWR;
        private System.Windows.Forms.GroupBoxTS grpFwdPow;
        private System.Windows.Forms.GroupBoxTS grpRefPow;
        private System.Windows.Forms.PictureBox picRefPow;
        private System.Windows.Forms.TextBox txtRefPow;
        private System.Windows.Forms.RadioButton rdBypass;
        private System.Windows.Forms.RadioButton rdTune;
        private System.Windows.Forms.Label lblC;
        private System.Windows.Forms.Label lblL;
        private System.Windows.Forms.Label lblHiZ;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.NumericUpDown udSleepTime;
        private System.Windows.Forms.NumericUpDown udHighSWR;
        private System.Windows.Forms.NumericUpDown udOffSleep;
        private System.Windows.Forms.CheckBox chkDoNotPress;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown udTunPower;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBoxTS grpAntennaProfile;
        private System.Windows.Forms.ButtonTS btnAntProfileDelete;
        private System.Windows.Forms.ButtonTS btnAntProfileRename;
        private System.Windows.Forms.ComboBoxTS comboAntProfileName;
        private System.Windows.Forms.ButtonTS btnAntProfileAdd;
        private System.Windows.Forms.ButtonTS resetATUdatabase;
        private System.Windows.Forms.CheckBox chckForceRetune;
        private System.Windows.Forms.RadioButton rdStop;
        private System.Windows.Forms.NumericUpDown swrTargetUpDown;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown retuneSWRupdown;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label MemoryTune;
        public System.Windows.Forms.CheckBoxTS chkAutoMode;
        private System.ComponentModel.IContainer components = null;

        
    }
}