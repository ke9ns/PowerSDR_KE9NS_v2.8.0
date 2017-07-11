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

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using System.IO;

namespace PowerSDR
{

    public enum TuneResult
    {
        TuneSuccessful = 0,
        TuneOK, // above thresh
        TuneLookup,
        TuneFailedHighSWR,
        TuneFailedLostRF,
        TunerNotNeeded, // good match on bypass
        TuneFailedNoRF,
        TuneAborted,
        TuneAutoModeTune,
        TuneAutoModeBypass
    }

    public class FLEX3000ATUForm : System.Windows.Forms.Form
    {
        #region Variable Declaration

        private Console console;
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
        private Label label1;
        private Label label2;
        private Label label3;
        private NumericUpDown udTunPower;
        private Label label4;
        private GroupBoxTS grpAntennaProfile;
        private ButtonTS btnAntProfileDelete;
        private ButtonTS btnAntProfileRename;
        private ComboBoxTS comboAntProfileName;
        private ButtonTS btnAntProfileAdd;
        private ButtonTS resetATUdatabase;
        private CheckBox chckForceRetune;
        private RadioButton rdStop;
        private NumericUpDown swrTargetUpDown;
        private Label label5;
        private NumericUpDown retuneSWRupdown;
        private Label label6;
        private CheckBox checkBox1;
        private Button button1;
        private Label MemoryTune;
        public CheckBoxTS chkAutoMode;
        private System.ComponentModel.IContainer components = null;

        #endregion

        #region Constructor and Destructor

        public FLEX3000ATUForm(Console c)
        {
            InitializeComponent();
            console = c;
            lblBypSWR.Text = "";
            lblTunSWR.Text = "";
            lblBypFwdPow.Text = "";
            lblTunFwdPow.Text = "";
            lblBypRefPow.Text = "";
            lblTunRefPow.Text = "";
            rdBypass.Checked = true;
            Common.RestoreForm(this, "FLEX3000ATUForm", false);
            this.ActiveControl = button1;
            rdTune.Focus();
        }

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
            this.ClientSize = new System.Drawing.Size(269, 461);
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
            this.Name = "FLEX3000ATUForm";
            this.Text = "FLEX-3000 ATU Settings";
            this.Load += new System.EventHandler(this.FLEX3000ATUForm_Load);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.FWCATUForm_Closing);
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

        #region Properties

        private FWCATUMode current_tune_mode = FWCATUMode.Bypass;
        public FWCATUMode CurrentTuneMode
        {
            get { return current_tune_mode; }
            set
            {
                switch (value)
                {
                    case FWCATUMode.Bypass:
                        rdBypass.Checked = true;
                        break;
                    default:
                        break;
                }
            }
        }
        private bool show_feedback_popup = false;



        public bool ShowFeedbackPopup
        {
            get { return show_feedback_popup; }
            set { show_feedback_popup = value; }
        }

        #endregion

        #region Event Handlers

        private void FWCATUForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
            Common.SaveForm(this, "FLEX3000ATUForm");
        }

        private bool abort = false;
        public void AbortTune()
        {
            if (rdTune.Checked)
                abort = true;
        }

        public void DoBypass()
        {
            rdBypass.Checked = true;
        }

        private void rdBypass_CheckedChanged(object sender, System.EventArgs e)
        {
            if (rdBypass.Checked)
            {
                rdBypass.BackColor = console.ButtonSelectedColor;
                rdTune.Checked = false;
                current_tune_mode = FWCATUMode.Bypass;
                if (console.MOX)
                {
                    int power = console.PWR;
                    console.PWR = 0;
                    Thread.Sleep(50);
                    FWC.SetATUEnable(false);
                    console.PWR = power;
                }
                else FWC.SetATUEnable(false);

                console.FWCATUBypass();
            }
            else
            {
                rdBypass.BackColor = SystemColors.Control;
            }
            abort = true;

        }

        public void DoTune()
        {
            rdTune.Checked = true;
        }

        private void rdTune_CheckedChanged(object sender, System.EventArgs e)
        {
            if (rdTune.Checked)
            {
                //want to start tune even if in Auto Mode
                rdStop.Checked = false;
                abort = false;
                rdTune.BackColor = console.ButtonSelectedColor;
                Thread t;

               
                if (File.Exists(Application.StartupPath + "\\" + "atu.dll")) // ke9ns fix  atu.dll\\ was a mistake not caught until I applied NET4.5.2
                {
                    console.oldATU = false;
                  
                }
                else
                {
                    console.oldATU = true;

                  
                }
                


                if (console.oldATU)
                {
                   
                    t = new Thread(new ThreadStart(Tune));

                }
                else
                {
                   
                    tuningDLL = true;
                    t = new Thread(new ThreadStart(TuneATU));
                }

                t.Name = "Tune Thread";
                t.IsBackground = true;
                t.Priority = ThreadPriority.Normal;
                t.Start();

                Thread t2 = new Thread(new ThreadStart(UpdateMeters));
                t2.Name = "Update Meters Thread";
                t2.IsBackground = true;
                t2.Priority = ThreadPriority.BelowNormal;
                t2.Start();

                lblTuneComplete.ForeColor = Color.Black;
                lblTuneComplete.Text = "Tuning...";
            }
            else
            {
                rdTune.BackColor = SystemColors.Control;
            }

        }

        private bool tuning = false;

        public TuneResult last_tune_result = TuneResult.TuneSuccessful;

        public ArrayList profileList = new ArrayList();
        public string currentProfileName;
        public bool force_retune = false;
        public double swr_thresh = 1.5;
        public double swr_retune_target = 2.10;
        public double high_swr = 999999;
        public bool high_swr_no_limit = true;
        public int progressBarATUValue = 0;
        public bool tuningDLL = false;
        public double Audio_RadioVolumeSaved = 0;
        public bool autoMode = false;


        private void Tune()
        {
            int fwd, rev;
            HiPerfTimer t1 = new HiPerfTimer();
            t1.Start();

            //const int SLEEP_TIME = 300;
            int SLEEP_TIME = (int)udSleepTime.Value;
            double HIGH_SWR = (double)udHighSWR.Value;
            double swr_thresh = 1.1; // start at 1.1, switch to 1.5 after 5 sec.
            int OFF_SLEEP = (int)udOffSleep.Value;
            int MaxL = 256;
            int MaxC = 128;
            int TUN_LVL = (int)udTunPower.Value;

            console.atu_tuning = true;

            // check in bypass
            FWC.SetATUEnable(false);
            FWC.SetHiZ(false);
            Thread.Sleep(50);
            lblHiZ.Visible = false;
            int old_power = console.TunePower;
            console.TunePower = TUN_LVL;
            console.TUN = true;
            tuning = true;
            Thread.Sleep(500);
            if (abort)
            {
                last_tune_result = TuneResult.TuneAborted;
                goto end;
            }

            // if the match is good enough, then just leave it bypassed
            FWC.ReadPAADC(5, out fwd); console.pa_fwd_power = fwd;
            FWC.ReadPAADC(4, out rev); console.pa_rev_power = rev;
            byp_fwd_pow = console.FWCPAPower(fwd);
            byp_ref_pow = console.FWCPAPower(rev) * console.atu_swr_table[(int)console.TXBand];
            byp_swr = SWR(fwd, rev);
            tun_swr = byp_swr;

            if (byp_fwd_pow < 1)
            {
                rdBypass.Checked = true;
                last_tune_result = TuneResult.TuneFailedNoRF;
                goto end;
            }

            if (byp_swr < swr_thresh)
            {
                rdBypass.Checked = true;
                last_tune_result = TuneResult.TunerNotNeeded;
                goto end;
            }
            else if (byp_swr > HIGH_SWR)
            {
                rdBypass.Checked = true;
                last_tune_result = TuneResult.TuneFailedHighSWR;
                goto end;
            }

            // check HiZ
            console.TunePower = 0;
            Thread.Sleep(OFF_SLEEP);
            FWC.SetATUEnable(true);
            Thread.Sleep(OFF_SLEEP);
            SetC(8);
            SetL(8);
            FWC.SetHiZ(false);
            console.TunePower = TUN_LVL;
            Thread.Sleep(SLEEP_TIME);
            if (abort)
            {
                last_tune_result = TuneResult.TuneAborted;
                goto end;
            }

            FWC.ReadPAADC(5, out fwd); console.pa_fwd_power = fwd;
            FWC.ReadPAADC(4, out rev); console.pa_rev_power = rev;
            double lo_z_swr = SWR(fwd, rev);
            tun_swr = lo_z_swr;

            console.TunePower = 0;
            Thread.Sleep(OFF_SLEEP);
            FWC.SetHiZ(true);
            Thread.Sleep(OFF_SLEEP);
            console.TunePower = TUN_LVL;
            Thread.Sleep(SLEEP_TIME);
            if (abort)
            {
                last_tune_result = TuneResult.TuneAborted;
                goto end;
            }

            FWC.ReadPAADC(5, out fwd); console.pa_fwd_power = fwd;
            FWC.ReadPAADC(4, out rev); console.pa_rev_power = rev;
            double hi_z_swr = SWR(fwd, rev);
            tun_swr = hi_z_swr;
            Debug.WriteLine("lo: " + lo_z_swr.ToString("f1") + "  hi: " + hi_z_swr.ToString("f1"));

            double min_swr = double.MaxValue;
            if (hi_z_swr < lo_z_swr)
            {
                lblHiZ.Visible = true;
                min_swr = hi_z_swr;
            }
            else
            {
                lblHiZ.Visible = false;
                console.TunePower = 0;
                Thread.Sleep(OFF_SLEEP);
                FWC.SetHiZ(false);
                Thread.Sleep(OFF_SLEEP);
                console.TunePower = TUN_LVL;
                min_swr = lo_z_swr;
            }

            tun_fwd_pow = console.FWCPAPower(console.pa_fwd_power);
            if (byp_fwd_pow < 1)
            {
                rdBypass.Checked = true;
                last_tune_result = TuneResult.TuneFailedLostRF;
                goto end;
            }

            if (min_swr < swr_thresh)
            {
                rdTune.Checked = false;
                last_tune_result = TuneResult.TuneSuccessful;
                goto end;
            }

            console.TunePower = 0;
            Thread.Sleep(OFF_SLEEP);
            SetL(0);
            SetC(0);
            Thread.Sleep(OFF_SLEEP);
            console.TunePower = TUN_LVL;
            Thread.Sleep(SLEEP_TIME);
            if (abort)
            {
                rdBypass.Checked = true;
                last_tune_result = TuneResult.TuneAborted;
                goto end;
            }

            int count = 0;
            int l_step = 8;
            int c_step = 8;
            int min_l = 0, L = 0;
            int min_c = 0, C = 0;
            int no_progress = 0;

            FWC.ReadPAADC(5, out fwd); console.pa_fwd_power = fwd;
            FWC.ReadPAADC(4, out rev); console.pa_rev_power = rev;
            min_swr = SWR(fwd, rev);
            bool first_time = true;

            while (rdTune.Checked)
            {
                double start_swr = min_swr;
                while (L >= 0 && L <= MaxL && rdTune.Checked)
                {
                    if (!first_time) Thread.Sleep(SLEEP_TIME);
                    else first_time = false;
                    if (abort)
                    {
                        rdBypass.Checked = true;
                        last_tune_result = TuneResult.TuneAborted;
                        goto end;
                    }

                    t1.Stop();
                    if (t1.Duration > 5.0 && swr_thresh < 1.5)
                        swr_thresh = 1.5;
                    if (t1.Duration > 15.0) no_progress = 100;

                    FWC.ReadPAADC(5, out fwd); console.pa_fwd_power = fwd;
                    FWC.ReadPAADC(4, out rev); console.pa_rev_power = rev;
                    tun_fwd_pow = console.FWCPAPower(fwd);
                    tun_ref_pow = console.FWCPAPower(rev) * console.atu_swr_table[(int)console.TXBand];
                    if (byp_fwd_pow < 1)
                    {
                        rdBypass.Checked = true;
                        last_tune_result = TuneResult.TuneFailedLostRF;
                        goto end;
                    }

                    tun_swr = SWR(fwd, rev);
                    Debug.WriteLine("swr (" + L + ", " + C + "): " + tun_swr.ToString("f1") + "  min: " + min_swr.ToString("f1") + "  start: " + start_swr.ToString("f1"));
                    if (tun_swr < swr_thresh)
                    {
                        rdTune.Checked = false;
                        last_tune_result = TuneResult.TuneSuccessful;
                        goto end;
                    }

                    if (tun_swr < min_swr)
                    {
                        min_swr = tun_swr;
                        min_l = L;
                    }

                    if (tun_swr > min_swr + 0.3)
                    {
                        l_step *= -1;
                        break;
                    }

                    if (count++ * Math.Abs(l_step) > 32 && min_swr >= start_swr - 0.05)
                        break;

                    if (!rdTune.Checked)
                        break;

                    console.TunePower = 0;
                    Thread.Sleep(OFF_SLEEP);
                    SetL(L += l_step);
                    Thread.Sleep(OFF_SLEEP);
                    console.TunePower = TUN_LVL;
                }

                console.TunePower = 0;
                Thread.Sleep(OFF_SLEEP);
                SetL(min_l);
                Thread.Sleep(OFF_SLEEP);
                console.TunePower = TUN_LVL;
                L = min_l;

                if (min_swr >= start_swr - 0.05)
                    no_progress++;

                if (no_progress > 6)
                {
                    rdTune.Checked = false;
                    last_tune_result = TuneResult.TuneOK;
                    goto end;
                }

                Debug.Write("C");

                count = 0;
                start_swr = min_swr;

                while (C >= 0 && C <= MaxC && rdTune.Checked)
                {
                    Thread.Sleep(SLEEP_TIME);
                    if (abort)
                    {
                        rdBypass.Checked = true;
                        last_tune_result = TuneResult.TuneAborted;
                        goto end;
                    }

                    t1.Stop();
                    if (t1.Duration > 5.0 && swr_thresh < 1.5)
                        swr_thresh = 1.5;
                    if (t1.Duration > 15.0) no_progress = 100;

                    FWC.ReadPAADC(5, out fwd); console.pa_fwd_power = fwd;
                    FWC.ReadPAADC(4, out rev); console.pa_rev_power = rev;
                    tun_fwd_pow = console.FWCPAPower(fwd);
                    tun_ref_pow = console.FWCPAPower(rev) * console.atu_swr_table[(int)console.TXBand];
                    if (byp_fwd_pow < 1)
                    {
                        rdBypass.Checked = true;
                        last_tune_result = TuneResult.TuneFailedLostRF;
                        goto end;
                    }

                    tun_swr = SWR(fwd, rev);
                    Debug.WriteLine("swr (" + L + ", " + C + "): " + tun_swr.ToString("f1") + "  min: " + min_swr.ToString("f1") + "  start: " + start_swr.ToString("f1"));
                    if (tun_swr < swr_thresh)
                    {
                        rdTune.Checked = false;
                        last_tune_result = TuneResult.TuneSuccessful;
                        goto end;
                    }

                    if (tun_swr < min_swr)
                    {
                        min_swr = tun_swr;
                        min_c = C;
                    }

                    if (tun_swr > min_swr + 0.3)
                    {
                        c_step *= -1;
                        break;
                    }

                    if (count++ * Math.Abs(c_step) > 32 && min_swr >= start_swr - 0.05)
                        break;

                    if (!rdTune.Checked)
                        break;

                    console.TunePower = 0;
                    Thread.Sleep(OFF_SLEEP);
                    SetC(C += c_step);
                    Thread.Sleep(OFF_SLEEP);
                    console.TunePower = TUN_LVL;
                }

                console.TunePower = 0;
                Thread.Sleep(OFF_SLEEP);
                SetC(min_c);
                Thread.Sleep(OFF_SLEEP);
                console.TunePower = TUN_LVL;
                C = min_c;
                count = 0;

                if (min_swr >= start_swr - 0.05)
                    no_progress++;

                if (no_progress > 6)
                {
                    rdTune.Checked = false;
                    if (byp_swr > tun_swr)
                    {
                        last_tune_result = TuneResult.TuneOK;
                    }
                    else
                    {
                        last_tune_result = TuneResult.TuneFailedHighSWR;
                        rdBypass.Checked = true;
                    }
                    goto end;
                }

                if (Math.Abs(l_step) > 1) l_step /= 2;
                if (Math.Abs(c_step) > 1) c_step /= 2;

                Debug.Write("L");
            }

        end:
            switch (last_tune_result)
            {
                case TuneResult.TuneSuccessful:
                case TuneResult.TuneOK:
                    Thread.Sleep(SLEEP_TIME);
                    FWC.ReadPAADC(5, out fwd); console.pa_fwd_power = fwd;
                    FWC.ReadPAADC(4, out rev); console.pa_rev_power = rev;
                    tun_fwd_pow = console.FWCPAPower(fwd);
                    tun_ref_pow = console.FWCPAPower(rev) * console.atu_swr_table[(int)console.TXBand];
                    tun_swr = SWR(fwd, rev);

                    if (tun_swr > byp_swr)
                    {
                        last_tune_result = TuneResult.TunerNotNeeded;
                        rdBypass.Checked = true;
                    }
                    Thread.Sleep(100);
                    break;
            }
            //Debug.WriteLine("swr: "+swr.ToString("f1"));
            // cleanup
            tuning = false;
            console.TUN = false;
            console.TunePower = old_power;
            rdTune.Checked = false;
            console.atu_tuning = false;
            abort = false;

            //Invoke(new MethodInvoker(UpdateFeedback));
            UpdateFeedback();

            t1.Stop();
            lblTime.Text = "Time: " + t1.Duration.ToString("f1");
        }

        private void TuneATU()
        {
            //console.FWCATUTuned();
            Debug.WriteLine("***Entering TuneATU()*****");
            HiPerfTimer t1 = new HiPerfTimer();
            t1.Start();

            Thread t2 = new Thread(new ThreadStart(progressBarTuning));
            t2.Name = "ATU Progress Bar Thread";
            t2.IsBackground = true;
            t2.Priority = ThreadPriority.Normal;
            t2.Start();

            double start_swr = 99;
            int SLEEP_TIME = (int)udSleepTime.Value;
            int OFF_SLEEP = (int)udOffSleep.Value;
            int TUN_LVL = (int)udTunPower.Value;
            int old_power = console.TunePower;
            if (abort)
            {
                last_tune_result = TuneResult.TuneAborted;
                goto end;
            }

            string last_tune_result_string = last_tune_result.ToString();
            string version = console.getVersion();
            button1.Focus();


            try
            {
#if(!NO_NEW_ATU)

                ATUClass obj = new ATUClass();
                MemoryTune.Hide();
                console.TunePower = 10;
                //Thread.Sleep(OFF_SLEEP);
                // Audio_RadioVolumeSaved = Audio.RadioVolume;

                version = console.getVersion();
                ////////////***load atu profiles if ATU button is pressed first
                if (comboAntProfileName.Items.Count == 0)
                {
                    profileList = obj.Init(version, ref currentProfileName, true, ref high_swr,
                                  ref  high_swr_no_limit, ref  swr_thresh, ref  swr_retune_target);
                }
                else
                {
                    profileList = obj.Init(version, ref currentProfileName, false, ref high_swr, ref high_swr_no_limit,
                                           ref swr_thresh, ref swr_retune_target);
                }
                if (profileList.Count == 0 && comboAntProfileName.Items.Count == 0)
                {
                    currentProfileName = "Default Profile";
                    comboAntProfileName.Items.Add(currentProfileName);
                    comboAntProfileName.SelectedItem = currentProfileName;
                }
                else
                {
                    foreach (string s in profileList)
                    {
                        if (!comboAntProfileName.Items.Contains(s))
                        {
                            comboAntProfileName.Items.Add(s);
                        }
                    }
                    comboAntProfileName.SelectedItem = currentProfileName;  //last saved profile (from database)
                }
                ///////***


                obj.DoWork( (int)console.RX1Band,
                           autoMode,
                           ref high_swr,
                           ref high_swr_no_limit,
                           ref swr_retune_target,
                           ref swr_thresh,
                           force_retune,
                           ref currentProfileName,
                           ref deletedProfileList,
                           ref start_swr,
                           console.atu_tuning,
                           ref version,
                           ref SLEEP_TIME,
                           ref OFF_SLEEP,
                           ref TUN_LVL,
                           ref byp_swr,
                           ref tun_swr,
                           ref byp_fwd_pow,
                           ref tun_fwd_pow,
                           ref byp_ref_pow,
                           ref tun_ref_pow,
                           ref abort,
                           ref tuning,
                           console.VFOAFreq,
                           ref last_tune_result_string,
                           FWC.SetATUEnable,
                           FWC.SetHiZ,
                           SetL,
                           SetC,
                           getSWR,
                           console_TunePower,
                    //console_TunePower, 
                           Audio_RadioVolume,
                           ref Audio_RadioVolumeSaved,
                           console_TUN,
                           console_TUN,
                           rdBypass_Checked,
                           rdBypass_Checked,
                           rdTune_Checked,
                           rdTune_Checked,
                           setlblHiZ_Visible);

                //autoMode = false;
                if (start_swr == 99 && !rdBypass.Checked)
                {
                    MemoryTune.Show();
                }
#endif
            }

            catch (Exception)
            {

            }

            if (last_tune_result_string == "TuneSuccessful")
                last_tune_result = TuneResult.TuneSuccessful;
            else if (last_tune_result_string == "TuneOK")
                last_tune_result = TuneResult.TuneOK;
            else if (last_tune_result_string == "TuneFailedHighSWR")
                last_tune_result = TuneResult.TuneFailedHighSWR;
            else if (last_tune_result_string == "TuneAborted")
                last_tune_result = TuneResult.TuneAborted;
            else if (last_tune_result_string == "TunerNotNeeded")
                last_tune_result = TuneResult.TunerNotNeeded;
            else if (last_tune_result_string == "TuneFailedNoRF")
                last_tune_result = TuneResult.TuneFailedNoRF;
            else if (last_tune_result_string == "TuneAutoModeTune")
                last_tune_result = TuneResult.TuneAutoModeTune;
            else if (last_tune_result_string == "TuneAutoModeBypass")
                last_tune_result = TuneResult.TuneAutoModeBypass;

        end:

            t1.Stop();
            Debug.WriteLine("The SWR has been reduced to " + tun_swr + " from " + byp_swr + "(" + start_swr + ") in " + t1.Duration.ToString("f1") + " seconds");
            LogMessageToFile("****The SWR at " + console.VFOAFreq + " MHz has been reduced to " + tun_swr + " from " + byp_swr + "(" + start_swr + ") in " + t1.Duration.ToString("f1") + " seconds***");
            tuning = false;
            console.TUN = false;
            console.TunePower = old_power;
            rdTune.Checked = false;
            console.atu_tuning = false;
            abort = false;

            lblTime.Text = "Time: " + t1.Duration.ToString("f1");
            UpdateFeedbackNew();
            tuningDLL = false;
            autoMode = false;


        }

        #endregion

        public bool setlblHiZ_Visible(bool b)
        {
            lblHiZ.Visible = b;
            return lblHiZ.Visible;
        }

        public int console_TunePower(int a)
        {
            console.TunePower = a;
            return console.TunePower;
        }

        public double Audio_RadioVolume(double a)
        {
            Audio.RadioVolume = a;
            return Audio.RadioVolume;
        }

        public bool console_TUN(bool b)
        {
            console.TUN = b;
            return console.TUN;
        }

        public bool console_TUN()
        {
            return console.TUN;
        }

        public bool rdBypass_Checked(bool b)
        {
            rdBypass.Checked = b;
            return rdBypass.Checked;
        }

        public bool rdBypass_Checked()
        {
            return rdBypass.Checked;
        }

        public bool rdTune_Checked(bool b)
        {
            rdTune.Checked = b;
            return rdBypass.Checked;
        }

        public bool rdTune_Checked()
        {
            return rdTune.Checked;
        }

        public double getSWR()
        {
            Thread.Sleep(50);
            int adc_fwd, adc_rev;
            FWC.ReadPAADC(5, out adc_fwd); console.pa_fwd_power = adc_fwd;
            FWC.ReadPAADC(4, out adc_rev); console.pa_rev_power = adc_rev;
            tun_fwd_pow = console.FWCPAPower(adc_fwd);
            tun_ref_pow = console.FWCPAPower(adc_rev) * console.atu_swr_table[(int)console.TXBand];

            if (adc_fwd == 0 && adc_rev == 0)
            {
                Debug.WriteLine("WARGNING: FWD and REV is 0");
                //return 1.0;  
            }
            if (adc_fwd <= 4 && adc_rev <= 4) Debug.WriteLine("WARNING: Possible invalid SWR reading");
            if (tun_ref_pow > tun_fwd_pow)
            {
                Debug.WriteLine("WARNING: Reflected Power > Forward Power!!");
                //return 50.0;
            }


            double sqrt_r_over_f = Math.Sqrt(tun_ref_pow / tun_fwd_pow);
            double swr = (1.0 + sqrt_r_over_f) / Math.Abs((1.0 - sqrt_r_over_f));
            if (swr < 1.0)
            {
                Debug.WriteLine("WARNING: SWR is < 1.0!!");
                // swr = -1.0;
            }

            Debug.WriteLine("tun_fwd_pow: " + tun_fwd_pow + " tun_ref_pow: " + tun_ref_pow + "                   SWR: " + swr + "\n");
            LogMessageToFile(" Reflected Power: " + tun_ref_pow + "             SWR: " + swr + "\n");

            return swr;

        }

        public double SWR(int adc_fwd, int adc_rev)
        {
            double f = console.FWCPAPower(adc_fwd);
            double r = console.FWCPAPower(adc_rev) * console.atu_swr_table[(int)console.TXBand];
            //Debug.WriteLine("FWCSWR: fwd:"+adc_fwd+" rev:"+adc_rev+" f:"+f.ToString("f2")+" r:"+r.ToString("f2"));

            if (adc_fwd == 0 && adc_rev == 0) return 1.0;
            if (r > f) return 50.0;

            double sqrt_r_over_f = Math.Sqrt(r / f);
            double swr = (1.0 + sqrt_r_over_f) / (1.0 - sqrt_r_over_f);
            if (swr < 1.0) swr = 1.0;
            if (swr > 50.0) swr = 50.0;
            return swr;
        }

        private int quantizeFreq_Hz(double freq_Hz)//, int qBandwidth)
        {
            return (int)Math.Round(freq_Hz / 10000) * 10000;
        }

        public int getBanddelete()
        {
            double freq = console.VFOAFreq;

            if (freq == 2.5 || freq == 5.0 || freq == 10.0 || freq == 15.0 || freq == 20.0)
            {
                return (int)Band.WWV;
            }

            else if (freq >= 1.8 && freq < 2.0)
            {
                return (int)Band.B160M;
            }


            else if (freq >= 3.5 && freq < 4.0)
            {
                return (int)Band.B80M;
            }


            else if (freq >= 5.0 && freq < 6.0)
            {
                return (int)Band.B60M;
            }

            else if (freq >= 7.0 && freq < 7.3)
            {
                return (int)Band.B40M;
            }

            else if (freq >= 10.1 && freq < 10.15)
            {
                return (int)Band.B30M;
            }

            else if (freq >= 14.0 && freq < 14.350)
            {
                return (int)Band.B20M;
            }

            else if (freq >= 18.068 && freq < 18.168)
            {
                return (int)Band.B17M;
            }

            else if (freq >= 21.0 && freq < 21.45)
            {
                return (int)Band.B15M;
            }

            else if (freq >= 24.890 && freq < 24.990)
            {
                return (int)Band.B12M;
            }

            else if (freq >= 28.0 && freq < 29.7)
            {
                return (int)Band.B10M;
            }

            else if (freq >= 50.0 && freq < 54.0)
            {
                return (int)Band.B6M;
            }

            else if (freq >= 144.0 && freq < 146.0)
            {
                return (int)Band.B2M;
            }

            else
                return -1;
        }

        private byte SwapBits(byte b)
        {
            byte temp = b, result = 0;

            for (int i = 0; i < 8; i++)
            {
                result <<= 1;
                if ((temp & 1) == 1) result += 1;
                temp >>= 1;
            }

            return result;
        }

        private void SetL(int val)
        {
            if (val > 255 || val < 0)
            {
                Debug.WriteLine("WARNING: L VALUE CANNOT BE SET! VALUE OUT OF RANGE!");
                val = 255;
                //return;
            }
            Thread.Sleep(50);
            FWC.I2C_Write2Value(0x4E, 0x02, SwapBits((byte)val));
            lblL.Text = "L: " + val.ToString();
            Debug.Write(" L(" + val + ") ");
        }

        private void SetC(int val)
        {
            if (val > 128 || val < 0)
            {
                Debug.WriteLine("WARNING: C VALUE CANNOT BE SET! VALUE OUT OF RANGE!");
                val = 128;
                //return;
            }
            Thread.Sleep(50);
            FWC.I2C_Write2Value(0x4E, 0x03, (byte)val);
            lblC.Text = "C: " + val.ToString();
            Debug.Write(" C(" + val + ") ");
        }

        private int[] StringToIntArray(string s)
        {
            int o = s.Length;
            int[] iArray = new int[o];
            for (int x = 0; x < s.Length; x++)
            {
                iArray[x] = Convert.ToInt32(s.Substring(x, 1));
            }
            return iArray;
        }

        private void UpdateMeters()
        {
            while (rdTune.Checked || tuning)
            {
                // get swr
                /*double swr = console.FWCSWR(console.pa_fwd_power, console.pa_rev_power);
                if(saved_swr == 999999.0)
                    saved_swr = swr;
                else saved_swr = saved_swr * 0.8 + swr * 0.2;*/
                txtSWR.Text = tun_swr.ToString("f1") + " : 1";

                // get fwd pow
                double fwd_pow = console.FWCPAPower(console.pa_fwd_power);
                if (saved_fwd_pow == 999999.0)
                    saved_fwd_pow = fwd_pow;
                else saved_fwd_pow = saved_fwd_pow * 0.8 + fwd_pow * 0.2;
                txtFwdPow.Text = saved_fwd_pow.ToString("f1") + " W";

                // get ref pow
                double ref_pow = console.FWCPAPower(console.pa_rev_power) * console.atu_swr_table[(int)console.TXBand];
                if (saved_ref_pow == 999999.0)
                    saved_ref_pow = ref_pow;
                else saved_ref_pow = saved_ref_pow * 0.8 + ref_pow * 0.2;
                txtRefPow.Text = saved_ref_pow.ToString("f1") + " W";

                picSWR.Invalidate();
                picFwdPow.Invalidate();
                picRefPow.Invalidate();

                Thread.Sleep(50);
            }
        }

        private double byp_swr = 1.0;
        private double tun_swr = 1.0;
        private double byp_fwd_pow = 0.0;
        private double tun_fwd_pow = 0.0;
        private double byp_ref_pow = 0.0;
        private double tun_ref_pow = 0.0;


        private void UpdateFeedbackNew()
        {
            string greaterThan = ">";
            if (byp_swr > 10)
            {
                byp_swr = 10;
                greaterThan = ">";
            }
            else
            {
                greaterThan = "";
            }

            switch (last_tune_result)
            {
                case TuneResult.TuneSuccessful:
                    console.FWCATUTuned();
                    lblTuneComplete.ForeColor = Color.Green;
                    lblTuneComplete.Text = "Tune Completed Successfully";

                    lblBypSWR.Text = greaterThan + byp_swr.ToString("f1") + " : 1";
                    lblTunSWR.Text = tun_swr.ToString("f1") + " : 1";
                    lblBypFwdPow.Text = byp_fwd_pow.ToString("f1");
                    lblTunFwdPow.Text = tun_fwd_pow.ToString("f1");
                    lblBypRefPow.Text = byp_ref_pow.ToString("f1");
                    lblTunRefPow.Text = tun_ref_pow.ToString("f1");

                    // if (show_feedback_popup)
                    //     MessageBox.Show("Tune Completed Successfully (" + tun_swr.ToString("f1") + ":1)", "ATU Feedback",
                    //         MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (show_feedback_popup)
                        MessageBox.Show("(" + console.VFOAFreq + " MHz) SWR reduced to " + tun_swr.ToString("f1") + ":1 from " + byp_swr.ToString("f1") + ":1", "ATU Feedback",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                    //("****The SWR at " + console.VFOAFreq + " MHz has been reduced to " + tun_swr + ":1 from " + byp_swr + "in " + t1.Duration.ToString("f1") + " seconds");


                    console.SetATUFeedback("Tune Completed Successfully (" + tun_swr.ToString("f1") + ":1)");
                    LogMessageToFile("(" + console.VFOAFreq + " MHz) SWR reduced to " + tun_swr.ToString("f1") + ":1 from " + byp_swr.ToString("f1") + ":1");

                    break;
                case TuneResult.TuneOK:
                    console.FWCATUTuned();
                    //if (tun_swr < 2.0)
                    if (tun_swr < 10.0)
                    {
                        lblTuneComplete.ForeColor = Color.Green;
                        lblTuneComplete.Text = "Tune OK: Above Threshold";
                        if (show_feedback_popup)
                            //MessageBox.Show("Tune OK (" + tun_swr.ToString("f1") + ":1)", "ATU Feedback",
                            //MessageBoxButtons.OK, MessageBoxIcon.Information);
                            MessageBox.Show("(" + console.VFOAFreq + " MHz) SWR reduced to " + tun_swr.ToString("f1") + ":1 from " + byp_swr.ToString("f1") + ":1", "ATU Feedback",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        console.SetATUFeedback("Tune OK (" + tun_swr.ToString("f1") + ":1)");
                        LogMessageToFile("(" + console.VFOAFreq + " MHz) SWR reduced to " + tun_swr.ToString("f1") + ":1 from " + byp_swr.ToString("f1") + ":1");
                    }
                    else
                    {
                        lblTuneComplete.ForeColor = Color.Red;
                        lblTuneComplete.Text = "Tune Failed: High SWR";
                        if (show_feedback_popup)
                        {
                            //MessageBox.Show("Tune Failed (" + tun_swr.ToString("f1") + ":1)", "ATU Feedback",
                            //    MessageBoxButtons.OK, MessageBoxIcon.Error);
                            MessageBox.Show("Tune Failed (SWR > 10:1)", "ATU Feedback",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        console.SetATUFeedback("Tune Failed (" + tun_swr.ToString("f1") + ":1)");
                        LogMessageToFile("Tune Failed (" + tun_swr.ToString("f1") + ":1)");
                    }

                    lblBypSWR.Text = greaterThan + byp_swr.ToString("f1") + " : 1";
                    lblTunSWR.Text = tun_swr.ToString("f1") + " : 1";
                    lblBypFwdPow.Text = byp_fwd_pow.ToString("f1");
                    lblTunFwdPow.Text = tun_fwd_pow.ToString("f1");
                    lblBypRefPow.Text = byp_ref_pow.ToString("f1");
                    lblTunRefPow.Text = tun_ref_pow.ToString("f1");
                    break;
                case TuneResult.TunerNotNeeded:
                    console.FWCATUBypass();
                    lblTuneComplete.ForeColor = Color.Green;
                    lblTuneComplete.Text = "Tuner Bypassed";

                    lblBypSWR.Text = greaterThan + byp_swr.ToString("f1") + " : 1";
                    lblTunSWR.Text = "";
                    lblBypFwdPow.Text = byp_fwd_pow.ToString("f1");
                    lblTunFwdPow.Text = "";
                    lblBypRefPow.Text = byp_ref_pow.ToString("f1");
                    lblTunRefPow.Text = "";
                    if (show_feedback_popup)
                    {
                        //            MessageBox.Show("Tuner Bypassed: Good match (" + tun_swr.ToString("f1") + ":1)", "ATU Feedback",
                        //                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //        console.SetATUFeedback("Tuner Bypassed: Good match (" + tun_swr.ToString("f1") + ":1)");
                        if (tun_swr < swr_thresh)
                        {
                            MessageBox.Show("Tuner Bypassed: Tuner Not Needed (" + tun_swr.ToString("f1") + ":1)", "ATU Feedback",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                            console.SetATUFeedback("Tuner Bypassed: Tuner Not Needed (" + tun_swr.ToString("f1") + ":1)");
                            LogMessageToFile("Output Message: Tuner Bypassed: Tuner Not Needed");
                        }
                        else
                        {
                            MessageBox.Show("Tuner Bypassed: No Improvement With Tuner (" + tun_swr.ToString("f1") + ":1)", "ATU Feedback",
                                     MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LogMessageToFile("Output Message: Tuner Bypassed: No Improvement With Tuner (" + tun_swr.ToString("f1") + ":1");
                        }
                    }
                    console.SetATUFeedback("Tuner Bypassed: No Improvement With Tuner (" + tun_swr.ToString("f1") + ":1)");
                    break;
                case TuneResult.TuneAborted:
                    if (tun_swr > swr_retune_target || byp_swr < tun_swr)  //set to bypass mode
                    {
                        console.FWCATUBypass();
                        lblTuneComplete.ForeColor = Color.Orange;
                        lblTuneComplete.Text = "Tune Aborted, Set to Bypass";
                        lblBypSWR.Text = greaterThan + byp_swr.ToString("f1") + " : 1";
                        lblTunSWR.Text = "";
                        lblBypFwdPow.Text = byp_fwd_pow.ToString("f1");
                        lblTunFwdPow.Text = "";
                        lblBypRefPow.Text = byp_ref_pow.ToString("f1");
                        lblTunRefPow.Text = "";
                        //    if (show_feedback_popup)
                        MessageBox.Show("Tune Aborted, Bypassed", "ATU Feedback",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        console.SetATUFeedback("Tune Aborted, Bypassed");
                        LogMessageToFile("Output Message: Tune Aborted, Bypassed");
                    }
                    else  //set to best found match
                    {
                        console.FWCATUTuned();
                        lblTuneComplete.ForeColor = Color.Orange;
                        lblTuneComplete.Text = "Tune Aborted, Set to Best Found Match";

                        lblTunSWR.Text = tun_swr.ToString("f1") + " : 1";
                        lblBypSWR.Text = greaterThan + byp_swr.ToString("f1") + " : 1";
                        //lblTunSWR.Text = "";
                        lblBypFwdPow.Text = byp_fwd_pow.ToString("f1");
                        lblTunFwdPow.Text = "";
                        lblBypRefPow.Text = byp_ref_pow.ToString("f1");
                        lblTunRefPow.Text = "";
                        //           if (show_feedback_popup)
                        //               MessageBox.Show("Tune Aborted, SWR "+tun_swr+" : 1", "ATU Feedback",
                        //                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                        console.SetATUFeedback("Tune Aborted, Set to Best Found Match");
                        LogMessageToFile("Output Message: Tune Aborted, Set to Best Found Match ");
                    }
                    break;
                case TuneResult.TuneFailedHighSWR:
                    console.FWCATUFailed();
                    lblTuneComplete.ForeColor = Color.Red;
                    //lblTuneComplete.Text = "Tune Failed: SWR Out of Range (16.7 - 150O)";
                    if (!checkBox1.Checked && byp_swr > (double)udHighSWR.Value)
                    {
                        lblTuneComplete.Text = "Tune Failed: SWR is greater than " + udHighSWR.Value.ToString("f1");
                        //      if (show_feedback_popup)
                        MessageBox.Show("Tune Failed: SWR is greater than " + udHighSWR.Value.ToString("f1"), "ATU Feedback",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        console.SetATUFeedback("Tune Failed: SWR is greater than " + udHighSWR.Value.ToString("f1"));
                        LogMessageToFile("Tune Failed: SWR is greater than " + udHighSWR.Value.ToString("f1"));
                    }
                    else
                    {
                        lblBypSWR.Text = greaterThan + byp_swr.ToString("f1") + " : 1";
                        lblTunSWR.Text = "";
                        lblBypFwdPow.Text = byp_fwd_pow.ToString("f1");
                        lblTunFwdPow.Text = "";
                        lblBypRefPow.Text = byp_ref_pow.ToString("f1");
                        lblTunRefPow.Text = "";

                        lblTuneComplete.Text = "Tune Failed";
                        //      if (show_feedback_popup)
                        MessageBox.Show("Tune Failed", "ATU Feedback",
                           MessageBoxButtons.OK, MessageBoxIcon.Error);

                        console.SetATUFeedback("Tune Failed");
                        LogMessageToFile("Tune Failed");
                    }
                    break;

                case TuneResult.TuneFailedLostRF:
                    console.FWCATUFailed();
                    lblTuneComplete.ForeColor = Color.Red;
                    lblTuneComplete.Text = "Tune Failed: RF Carrier Lost";

                    lblBypSWR.Text = greaterThan + byp_swr.ToString("f1") + " : 1";
                    lblTunSWR.Text = "";
                    lblBypFwdPow.Text = byp_fwd_pow.ToString("f1");
                    lblTunFwdPow.Text = "";
                    lblBypRefPow.Text = byp_ref_pow.ToString("f1");
                    lblTunRefPow.Text = "";
                    //         if (show_feedback_popup)
                    MessageBox.Show("Tune Failed: RF Carrier Lost", "ATU Feedback",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    console.SetATUFeedback("Tune Failed: RF Carrier Lost");
                    LogMessageToFile("Output Message: Tune Failed: RF Carrier Lost");
                    break;
                case TuneResult.TuneFailedNoRF:
                    console.FWCATUFailed();
                    lblTuneComplete.ForeColor = Color.Red;
                    lblTuneComplete.Text = "Tune Failed: No RF Detected";

                    lblBypSWR.Text = greaterThan + byp_swr.ToString("f1") + " : 1";
                    lblTunSWR.Text = "";
                    lblBypFwdPow.Text = byp_fwd_pow.ToString("f1");
                    lblTunFwdPow.Text = "";
                    lblBypRefPow.Text = byp_ref_pow.ToString("f1");
                    lblTunRefPow.Text = "";
                    //          if (show_feedback_popup)
                    MessageBox.Show("Tune Failed: No RF Detected", "ATU Feedback",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    console.SetATUFeedback("Tune Failed: No RF Detected");
                    LogMessageToFile("Output Message: Tune Failed: No RF Detected");
                    break;
                case TuneResult.TuneAutoModeTune:
                    console.FWCATUTuned();
                    lblTuneComplete.ForeColor = Color.Orange;
                    lblTuneComplete.Text = "Auto Tune Mode Set";
                    lblBypSWR.Text = "";
                    lblTunSWR.Text = "";
                    lblBypFwdPow.Text = "";
                    lblTunFwdPow.Text = "";
                    lblBypRefPow.Text = "";
                    lblTunRefPow.Text = "";
                    console.SetATUFeedback("Auto Tune Mode Set");
                    LogMessageToFile("Output Message: Auto Tune Mode Used");
                    break;
                case TuneResult.TuneAutoModeBypass:
                    console.FWCATUBypass();
                    lblTuneComplete.ForeColor = Color.Orange;
                    lblTuneComplete.Text = "Auto Mode Bypass";
                    lblBypSWR.Text = "";
                    lblTunSWR.Text = "";
                    lblBypFwdPow.Text = "";
                    lblTunFwdPow.Text = "";
                    lblBypRefPow.Text = "";
                    lblTunRefPow.Text = "";
                    console.SetATUFeedback("Auto Mode: ATU bypassed, no memory found on band");
                    LogMessageToFile("Output Message: Auto Tune Mode Used - No memory, bypass mode enabled");
                    break;
            }
        }

        private void UpdateFeedback()
        {
            show_feedback_popup = true;
            switch (last_tune_result)
            {
                case TuneResult.TuneSuccessful:
                    console.FWCATUTuned();
                    lblTuneComplete.ForeColor = Color.Green;
                    lblTuneComplete.Text = "Tune Completed Successfully";

                    lblBypSWR.Text = byp_swr.ToString("f1") + " : 1";
                    lblTunSWR.Text = tun_swr.ToString("f1") + " : 1";
                    lblBypFwdPow.Text = byp_fwd_pow.ToString("f1");
                    lblTunFwdPow.Text = tun_fwd_pow.ToString("f1");
                    lblBypRefPow.Text = byp_ref_pow.ToString("f1");
                    lblTunRefPow.Text = tun_ref_pow.ToString("f1");

                    if (show_feedback_popup)
                        MessageBox.Show("Tune Completed Successfully (" + tun_swr.ToString("f1") + ":1)", "ATU Feedback",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    console.SetATUFeedback("Tune Completed Successfully (" + tun_swr.ToString("f1") + ":1)");
                    break;
                case TuneResult.TuneOK:
                    console.FWCATUTuned();
                    if (tun_swr < 2.0)
                    {
                        lblTuneComplete.ForeColor = Color.Green;
                        lblTuneComplete.Text = "Tune OK: Above Threshold";
                        if (show_feedback_popup)
                            MessageBox.Show("Tune OK (" + tun_swr.ToString("f1") + ":1)", "ATU Feedback",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        console.SetATUFeedback("Tune OK (" + tun_swr.ToString("f1") + ":1)");
                    }
                    else
                    {
                        lblTuneComplete.ForeColor = Color.Red;
                        lblTuneComplete.Text = "Tune Failed: High SWR";
                        if (show_feedback_popup)
                            MessageBox.Show("Tune Failed (" + tun_swr.ToString("f1") + ":1)", "ATU Feedback",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        console.SetATUFeedback("Tune Failed (" + tun_swr.ToString("f1") + ":1)");
                    }

                    lblBypSWR.Text = byp_swr.ToString("f1") + " : 1";
                    lblTunSWR.Text = tun_swr.ToString("f1") + " : 1";
                    lblBypFwdPow.Text = byp_fwd_pow.ToString("f1");
                    lblTunFwdPow.Text = tun_fwd_pow.ToString("f1");
                    lblBypRefPow.Text = byp_ref_pow.ToString("f1");
                    lblTunRefPow.Text = tun_ref_pow.ToString("f1");
                    break;
                case TuneResult.TunerNotNeeded:
                    console.FWCATUBypass();
                    lblTuneComplete.ForeColor = Color.Green;
                    lblTuneComplete.Text = "Tuner Bypassed: Good match";

                    lblBypSWR.Text = byp_swr.ToString("f1") + " : 1";
                    lblTunSWR.Text = "";
                    lblBypFwdPow.Text = byp_fwd_pow.ToString("f1");
                    lblTunFwdPow.Text = "";
                    lblBypRefPow.Text = byp_ref_pow.ToString("f1");
                    lblTunRefPow.Text = "";
                    if (show_feedback_popup)
                        MessageBox.Show("Tuner Bypassed: Good match (" + tun_swr.ToString("f1") + ":1)", "ATU Feedback",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    console.SetATUFeedback("Tuner Bypassed: Good match (" + tun_swr.ToString("f1") + ":1)");
                    break;
                case TuneResult.TuneAborted:
                    console.FWCATUBypass();
                    lblTuneComplete.ForeColor = Color.Orange;
                    lblTuneComplete.Text = "Tune Aborted";

                    lblBypSWR.Text = byp_swr.ToString("f1") + " : 1";
                    lblTunSWR.Text = "";
                    lblBypFwdPow.Text = byp_fwd_pow.ToString("f1");
                    lblTunFwdPow.Text = "";
                    lblBypRefPow.Text = byp_ref_pow.ToString("f1");
                    lblTunRefPow.Text = "";
                    if (show_feedback_popup)
                        MessageBox.Show("Tune Aborted, Bypassed", "ATU Feedback",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    console.SetATUFeedback("Tune Aborted, Bypassed");
                    break;
                case TuneResult.TuneFailedHighSWR:
                    console.FWCATUFailed();
                    lblTuneComplete.ForeColor = Color.Red;
                    lblTuneComplete.Text = "Tune Failed: SWR Out of Range (16.7 - 150O)";

                    lblBypSWR.Text = byp_swr.ToString("f1") + " : 1";
                    lblTunSWR.Text = "";
                    lblBypFwdPow.Text = byp_fwd_pow.ToString("f1");
                    lblTunFwdPow.Text = "";
                    lblBypRefPow.Text = byp_ref_pow.ToString("f1");
                    lblTunRefPow.Text = "";
                    if (show_feedback_popup)
                        MessageBox.Show("Tune Failed: SWR Out of Range (16.7 - 150O)", "ATU Feedback",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    console.SetATUFeedback("Tune Failed: SWR Out of Range (16.7 - 150O)");
                    break;
                case TuneResult.TuneFailedLostRF:
                    console.FWCATUFailed();
                    lblTuneComplete.ForeColor = Color.Red;
                    lblTuneComplete.Text = "Tune Failed: RF Carrier Lost";

                    lblBypSWR.Text = byp_swr.ToString("f1") + " : 1";
                    lblTunSWR.Text = "";
                    lblBypFwdPow.Text = byp_fwd_pow.ToString("f1");
                    lblTunFwdPow.Text = "";
                    lblBypRefPow.Text = byp_ref_pow.ToString("f1");
                    lblTunRefPow.Text = "";
                    if (show_feedback_popup)
                        MessageBox.Show("Tune Failed: RF Carrier Lost", "ATU Feedback",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    console.SetATUFeedback("Tune Failed: RF Carrier Lost");
                    break;
                case TuneResult.TuneFailedNoRF:
                    console.FWCATUFailed();
                    lblTuneComplete.ForeColor = Color.Red;
                    lblTuneComplete.Text = "Tune Failed: No RF Detected";

                    lblBypSWR.Text = byp_swr.ToString("f1") + " : 1";
                    lblTunSWR.Text = "";
                    lblBypFwdPow.Text = byp_fwd_pow.ToString("f1");
                    lblTunFwdPow.Text = "";
                    lblBypRefPow.Text = byp_ref_pow.ToString("f1");
                    lblTunRefPow.Text = "";
                    if (show_feedback_popup)
                        MessageBox.Show("Tune Failed: No RF Detected", "ATU Feedback",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    console.SetATUFeedback("Tune Failed: No RF Detected");
                    break;
            }
        }

        //private double saved_swr = 999999.0;
        private void picSWR_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            int W = picSWR.Width;
            int H = picSWR.Height;
            if (tuning)
            {
                /*// draw threshold
                int x = (int)((swr_thresh-1) / 2.0 * W);
                e.Graphics.DrawLine(Pens.Red, x, 0, x, H);*/

                // draw current value
                int x = (int)((tun_swr - 1) / 9.0 * W);
                if (x < 1) x = 1;
                if (x > W - 1) x = W - 1;
                e.Graphics.DrawLine(Pens.Gold, x - 1, 1, x - 1, H - 1);
                e.Graphics.DrawLine(Pens.Yellow, x, 1, x, H - 1);
                e.Graphics.DrawLine(Pens.Gold, x + 1, 1, x + 1, H - 1);
            }
            else
            {
                /*// draw threshold
                int x = (int)((swr_thresh-1) / 2.0 * W);
                e.Graphics.DrawLine(Pens.Red, x, 0, x, H);*/
            }
        }

        private double saved_fwd_pow = 999999.0;
        private void picFwdPow_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            int W = picFwdPow.Width;
            int H = picFwdPow.Height;
            if (tuning)
            {
                int x = (int)((saved_fwd_pow) / 10.0 * W);
                if (x < 1) x = 1;
                if (x > W - 1) x = W - 1;
                e.Graphics.DrawLine(Pens.Gold, x - 1, 0, x - 1, H);
                e.Graphics.DrawLine(Pens.Yellow, x, 0, x, H);
                e.Graphics.DrawLine(Pens.Gold, x + 1, 0, x + 1, H);
            }
            else
            {
                e.Graphics.FillRectangle(Brushes.Black, 0, 0, W, H);
            }
        }

        private double saved_ref_pow = 999999.0;
        private void picRefPow_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            int W = picRefPow.Width;
            int H = picRefPow.Height;
            if (tuning)
            {
                int x = (int)((saved_ref_pow) / 10.0 * W);
                if (x < 1) x = 1;
                if (x > W - 1) x = W - 1;
                e.Graphics.DrawLine(Pens.Gold, x - 1, 0, x - 1, H);
                e.Graphics.DrawLine(Pens.Yellow, x, 0, x, H);
                e.Graphics.DrawLine(Pens.Gold, x + 1, 0, x + 1, H);
            }
            else
            {
                e.Graphics.FillRectangle(Brushes.Black, 0, 0, W, H);
            }
        }

        private void chkDoNotPress_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkDoNotPress.Checked)
            {
                chkDoNotPress.BackColor = console.ButtonSelectedColor;

                Thread t = new Thread(new ThreadStart(LongTest));
                t.Name = "Long Test Thread";
                t.IsBackground = true;
                t.Priority = ThreadPriority.Normal;
                t.Start();
            }
            else
            {
                chkDoNotPress.BackColor = SystemColors.Control;
            }
        }

        private void LongTest()
        {
            while (chkDoNotPress.Checked)
            {
                if (!rdTune.Checked)
                {
                    Thread.Sleep(10000);
                    rdTune.Checked = true;
                }
                else Thread.Sleep(1000);
            }
        }

        public void LogMessageToFile(string message)
        {
            System.IO.StreamWriter sw =
               System.IO.File.AppendText(
                  console.AppDataPath + "ATU.log"); // Change filename
            try
            {
                string logLine =
                   System.String.Format(
                      "{0:G}: {1}.", System.DateTime.Now, message);
                sw.WriteLine(logLine);
            }
            finally
            {
                sw.Close();
            }
        }

        private void txtSWR_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblTuneComplete_Click(object sender, EventArgs e)
        {

        }

        ArrayList deletedProfileList = new ArrayList();

        private void FLEX3000ATUForm_Load(object sender, EventArgs e)
        {
            if (console.oldATU)
            {
            
            }
            try
            {
#if(!NO_NEW_ATU)
                ATUClass obj = new ATUClass();

             

                string version = console.getVersion();
                MemoryTune.Hide();
                ////////////
                if (comboAntProfileName.Items.Count == 0)
                {
                    profileList = obj.Init(version, ref currentProfileName, true, ref high_swr,
                                  ref  high_swr_no_limit, ref  swr_thresh, ref  swr_retune_target);
                }
                else
                {
                    profileList = obj.Init(version, ref currentProfileName, false, ref high_swr, ref high_swr_no_limit,
                                           ref swr_thresh, ref swr_retune_target);
                }
                if (profileList.Count == 0 && comboAntProfileName.Items.Count == 0)
                {
                    currentProfileName = "Default Profile";
                    comboAntProfileName.Items.Add(currentProfileName);
                    comboAntProfileName.SelectedItem = currentProfileName;
                }
                else
                {
                    foreach (string s in profileList)
                    {
                        if (!comboAntProfileName.Items.Contains(s))
                        {
                            comboAntProfileName.Items.Add(s);
                        }
                    }
                    comboAntProfileName.SelectedItem = currentProfileName;  //last saved profile (from database)
                }
                ///////

                obj.Init(version, ref currentProfileName, true, ref high_swr, ref high_swr_no_limit,
                         ref swr_thresh, ref swr_retune_target);
#endif
                swrTargetUpDown.Maximum = retuneSWRupdown.Value;
                //swr_thresh = (double)swrTargetUpDown.Value;
                //swr_retune_target = (double)retuneSWRupdown.Value;
                //high_swr = (double)udHighSWR.Value;
                //high_swr_no_limit = checkBox1.Checked;
                swrTargetUpDown.Value = (decimal)swr_thresh;
                retuneSWRupdown.Value = (decimal)swr_retune_target;
                udHighSWR.Value = (decimal)high_swr;

#if(!NO_NEW_ATU)
                profileList = obj.Init(version, ref currentProfileName, false, ref high_swr, ref high_swr_no_limit,
                                        ref swr_thresh, ref swr_retune_target);
#endif
                if (profileList.Count == 0 && comboAntProfileName.Items.Count == 0)
                {
                    currentProfileName = "Default Profile";
                    comboAntProfileName.Items.Add(currentProfileName);
                    comboAntProfileName.SelectedItem = currentProfileName;
                }
                else
                {
                    foreach (string s in profileList)
                    {
                        comboAntProfileName.Items.Add(s);
                    }
                    comboAntProfileName.SelectedItem = currentProfileName;  //last saved profile (from database)
                }


                txtFwdPow.Focus();
                //button1.PerformClick();
                //button1.Focus();
                //button1.Visible = false;

                //this.AcceptButton = button1;
                //comboAntProfileName.DataSource = profileList;
            }
            catch (Exception)
            {

            }
        }

        private void grpTXProfile_Enter(object sender, EventArgs e)
        {

        }

        private void grpAntennaProfile_Enter(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void antennaProfileName_SelectedIndexChanged(object sender, EventArgs e)
        {
#if(!NO_NEW_ATU)
            if (comboAntProfileName.SelectedIndex < 0)
                return;
            ATUClass obj = new ATUClass();
            currentProfileName = comboAntProfileName.Text;

            obj.Init(console.getVersion(), ref currentProfileName, false, ref high_swr, ref high_swr_no_limit,
                 ref swr_thresh, ref swr_retune_target);
#endif

        }

        private void btnAntProfileAdd_Click(object sender, EventArgs e)
        {
            string name = InputBox.Show("Create Profile", "Please enter a profile name:",
            "AntennaProfile1");

            if (name == "" || name == null)
            {
                MessageBox.Show("Profile Save cancelled",
                    "Antenna Profile",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }


            if (comboAntProfileName.Items.Contains(name))
            {
                DialogResult result = MessageBox.Show(
                    "The profile " + name + " already exists.  Would you like to overwrite it? ",
                    "Overwrite Profile?",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.No)
                    return;
            }

            if (!comboAntProfileName.Items.Contains(name))
            {
                comboAntProfileName.Items.Add(name);

                //profileList.Add(name);
                //comboAntProfileName.DataSource = profileList;
                //comboAntProfileName.Text = name;
                //comboAntProfileName.SelectedItem = name;
                currentProfileName = name;
                comboAntProfileName.SelectedItem = name;
#if(!NO_NEW_ATU)
                ATUClass obj = new ATUClass();
                obj.Init(console.getVersion(), ref currentProfileName, false, ref high_swr, ref high_swr_no_limit,
                         ref swr_thresh, ref swr_retune_target);
#endif


            }
        }

        private void btnAntProfileSave_Click(object sender, EventArgs e)
        {
            try
            {
#if(!NO_NEW_ATU)
                ATUClass obj = new ATUClass();
                string name = InputBox.Show("Rename Profile", "Please enter a profile name:",
                comboAntProfileName.Text);

                string oldName = comboAntProfileName.Text;
                string newName = name;

                comboAntProfileName.Items.Remove(comboAntProfileName.Text);
                comboAntProfileName.Items.Add(name);
                comboAntProfileName.SelectedItem = name;
                currentProfileName = name;
                obj.Init(console.getVersion(), ref currentProfileName, false, ref high_swr, ref high_swr_no_limit,
                         ref swr_thresh, ref swr_retune_target);
                obj.Rename(oldName, newName, ref currentProfileName);
#endif
            }
            catch (Exception)
            {

            }

        }

        private void btnAntProfileDelete_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to delete the '" + comboAntProfileName.Text + "' antenna profile?",
                                    "Delete Profile?",
                                    MessageBoxButtons.YesNo,
                                    MessageBoxIcon.Warning);

            if (dr == DialogResult.No)
                return;


            int index = comboAntProfileName.SelectedIndex;
            deletedProfileList.Add(comboAntProfileName.Text);
            try
            {
#if(!NO_NEW_ATU)
                ATUClass obj = new ATUClass();
                profileList = obj.Init(console.getVersion(), ref currentProfileName, false, ref high_swr, ref high_swr_no_limit,
                                       ref swr_thresh, ref swr_retune_target);
                obj.Exit(deletedProfileList, ref currentProfileName, high_swr, high_swr_no_limit, swr_thresh, swr_retune_target);
                comboAntProfileName.Items.Remove(comboAntProfileName.Text);
                if (comboAntProfileName.Items.Count > 0)
                {
                    if (index > comboAntProfileName.Items.Count - 1)
                        index = comboAntProfileName.Items.Count - 1;
                    comboAntProfileName.SelectedIndex = index;
                }

                currentProfileName = comboAntProfileName.Text;
#endif
            }
            catch (Exception)
            {

            }
        }

        private void resetATUdatabase_Click(object sender, EventArgs e)
        {
            try
            {
#if(!NO_NEW_ATU)
                ATUClass obj = new ATUClass();
                if (obj.resetATUdatabase(console.getVersion()))
                {
                    comboAntProfileName.Items.Clear();
                    currentProfileName = "Default Profile";
                    comboAntProfileName.Items.Add(currentProfileName);
                    comboAntProfileName.SelectedItem = currentProfileName;
                    //obj.Init(console.getVersion(), ref currentProfileName, true);
                }
#endif
            }
            catch (Exception)
            {

            }
        }

        private void rdForceRetune_CheckedChanged(object sender, EventArgs e)
        {
            force_retune = true;

            DoTune();
            //rdTune.Checked = true;
            if (rdTune.Checked)
            {
                Thread t;
                rdTune.BackColor = console.ButtonSelectedColor;

                if (console.oldATU)
                {
                    t = new Thread(new ThreadStart(Tune));

                }
                else
                {
                    tuningDLL = true;
                    t = new Thread(new ThreadStart(TuneATU));

                }
                //Thread t = new Thread(new ThreadStart(QuickTest));
                t.Name = "Tune Thread";
                t.IsBackground = true;
                t.Priority = ThreadPriority.Normal;
                t.Start();

                Thread t2 = new Thread(new ThreadStart(UpdateMeters));
                t2.Name = "Update Meters Thread";
                t2.IsBackground = true;
                t2.Priority = ThreadPriority.BelowNormal;
                t2.Start();

                lblTuneComplete.ForeColor = Color.Black;
                lblTuneComplete.Text = "Tuning...";
            }
            else rdTune.BackColor = SystemColors.Control;


        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (chckForceRetune.Checked == true)
            {
                force_retune = true;
            }
            else
            {
                force_retune = false;
            }
        }



        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void control_swr_thresh_ValueChanged(object sender, EventArgs e)
        {
            try
            {
#if(!NO_NEW_ATU)
                ATUClass obj = new ATUClass();
                obj.Init(console.getVersion(), ref currentProfileName, false, ref high_swr, ref high_swr_no_limit,
                ref swr_thresh, ref swr_retune_target);
                retuneSWRupdown.Minimum = swrTargetUpDown.Value;
                swr_thresh = (double)swrTargetUpDown.Value;
                obj.Exit(deletedProfileList, ref currentProfileName, high_swr, high_swr_no_limit, swr_thresh, swr_retune_target);
#endif
            }
            catch (Exception)
            {

            }
        }

        private void retuneSWRupdown_ValueChanged(object sender, EventArgs e)
        {
            try
            {
#if(!NO_NEW_ATU)
                ATUClass obj = new ATUClass();
                obj.Init(console.getVersion(), ref currentProfileName, false, ref high_swr, ref high_swr_no_limit,
                        ref swr_thresh, ref swr_retune_target);
                swrTargetUpDown.Maximum = retuneSWRupdown.Value;
                swr_retune_target = (double)retuneSWRupdown.Value;
                obj.Exit(deletedProfileList, ref currentProfileName, high_swr, high_swr_no_limit, swr_thresh, swr_retune_target);
#endif
            }
            catch (Exception)
            {

            }
        }

        private void rdStop_CheckedChanged(object sender, EventArgs e)
        {
            if (rdStop.Checked == true)
            {
                abort = true;
                rdStop.Checked = false;
            }
        }

        private void udHighSWR_ValueChanged(object sender, EventArgs e)
        {
            try
            {
#if(!NO_NEW_ATU)
                ATUClass obj = new ATUClass();
                obj.Init(console.getVersion(), ref currentProfileName, false, ref high_swr, ref high_swr_no_limit,
                        ref swr_thresh, ref swr_retune_target);
#endif
                high_swr = (double)udHighSWR.Value;
#if(!NO_NEW_ATU)
                obj.Exit(deletedProfileList, ref currentProfileName, high_swr, high_swr_no_limit, swr_thresh, swr_retune_target);
#endif

            }
            catch (Exception)
            {

            }
        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                udHighSWR.Enabled = false;
                high_swr_no_limit = true;
            }
            else
            {
                udHighSWR.Enabled = true;
                high_swr_no_limit = false;
            }
        }

        private void progressBarTuning()
        {
            float maxTuningTime_ms = 25000;
            float offset = 0;
            float multiplier = 1;
            bool slowDown = false;
            float percent = 0;
            HiPerfTimer t3 = new HiPerfTimer();
            t3.Start();

            Progress p = new Progress("ATU Tuning Progress");
            p.SetPercent(0.0f);
            console.Invoke(new MethodInvoker(p.Show));

            while (tuningDLL)// && t3.DurationMsec< maxTuningTime_ms*2)
            {
                t3.Stop();
                if (t3.DurationMsec > maxTuningTime_ms / 2 && slowDown == false)
                {
                    t3.Start();
                    offset = 0.5f;
                    multiplier = 2;
                    slowDown = true;
                }
                percent = offset + (float)t3.DurationMsec / (maxTuningTime_ms * multiplier);
                if (percent < .98)
                {
                    p.SetPercent(percent);
                }
                else
                {
                    p.SetPercent(.98f);
                }
                if (!p.Visible)
                {
                    abort = true;
                }
                Thread.Sleep(400);

            }
            p.SetPercent(1);
            Thread.Sleep(500);
            p.Hide();


        }

        private void chkAutoMode_CheckedChanged(object sender, EventArgs e)
        {
            
        }
    }
}