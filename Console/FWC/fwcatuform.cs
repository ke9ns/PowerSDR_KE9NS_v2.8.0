//=================================================================
// fwcatuform.cs
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
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;

namespace PowerSDR
{
	/// <summary>
	/// Summary description for fwcatuform.
	/// </summary>
	public class FWCATUForm : System.Windows.Forms.Form
	{
		#region Variable Declaration

		private Console console;
		private System.Windows.Forms.GroupBoxTS grpMode;
		private System.Windows.Forms.RadioButtonTS radModeBypass;
		private System.Windows.Forms.RadioButtonTS radModeSemiAuto;
		private System.Windows.Forms.RadioButtonTS radModeAuto;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.ButtonTS btnTuneMemory;
		private System.Windows.Forms.ButtonTS btnTuneFull;
		private System.Windows.Forms.GroupBoxTS grpTune;
		private System.Windows.Forms.GroupBoxTS grpFeedback;
		private System.Windows.Forms.LabelTS lblFreq;
		private System.Windows.Forms.LabelTS lblPower;
		private System.Windows.Forms.LabelTS lblReflected;
		private System.Windows.Forms.LabelTS lblSWR;
		private System.Windows.Forms.LabelTS lblForward;
		private System.Windows.Forms.LabelTS lblTuneComplete;
		private System.Windows.Forms.GroupBoxTS grpSWRThreshold;
		private System.Windows.Forms.ComboBoxTS comboSWRThresh;
		private System.Windows.Forms.CheckBoxTS chkUseTUN;
        private CheckBoxTS chkATUEnabledOnBandChange;
        private ButtonTS buttonTS1;
        private LabelTS lblIND;
        private LabelTS lblCAP;
        private ButtonTS capdown;
        private ButtonTS indup;
        private ButtonTS inddown;
        private ButtonTS capup;
        private System.ComponentModel.IContainer components;

		#endregion

		#region Constructor and Destructor

		public FWCATUForm(Console c)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			console = c;
			comboSWRThresh.Text = "3.0 : 1";
			Common.RestoreForm(this, "FWCATU", false);

			if(radModeSemiAuto.Checked)
			{
				radModeBypass.Checked = true;
			}
			else if(radModeBypass.Checked)
			{
				radModeBypass_CheckedChanged(this, EventArgs.Empty);
			}
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
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

		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FWCATUForm));
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.chkATUEnabledOnBandChange = new System.Windows.Forms.CheckBoxTS();
            this.grpSWRThreshold = new System.Windows.Forms.GroupBoxTS();
            this.comboSWRThresh = new System.Windows.Forms.ComboBoxTS();
            this.grpFeedback = new System.Windows.Forms.GroupBoxTS();
            this.indup = new System.Windows.Forms.ButtonTS();
            this.inddown = new System.Windows.Forms.ButtonTS();
            this.capup = new System.Windows.Forms.ButtonTS();
            this.capdown = new System.Windows.Forms.ButtonTS();
            this.lblIND = new System.Windows.Forms.LabelTS();
            this.lblCAP = new System.Windows.Forms.LabelTS();
            this.buttonTS1 = new System.Windows.Forms.ButtonTS();
            this.lblTuneComplete = new System.Windows.Forms.LabelTS();
            this.lblSWR = new System.Windows.Forms.LabelTS();
            this.lblReflected = new System.Windows.Forms.LabelTS();
            this.lblPower = new System.Windows.Forms.LabelTS();
            this.lblFreq = new System.Windows.Forms.LabelTS();
            this.lblForward = new System.Windows.Forms.LabelTS();
            this.grpTune = new System.Windows.Forms.GroupBoxTS();
            this.chkUseTUN = new System.Windows.Forms.CheckBoxTS();
            this.btnTuneFull = new System.Windows.Forms.ButtonTS();
            this.btnTuneMemory = new System.Windows.Forms.ButtonTS();
            this.grpMode = new System.Windows.Forms.GroupBoxTS();
            this.radModeAuto = new System.Windows.Forms.RadioButtonTS();
            this.radModeSemiAuto = new System.Windows.Forms.RadioButtonTS();
            this.radModeBypass = new System.Windows.Forms.RadioButtonTS();
            this.grpSWRThreshold.SuspendLayout();
            this.grpFeedback.SuspendLayout();
            this.grpTune.SuspendLayout();
            this.grpMode.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkATUEnabledOnBandChange
            // 
            this.chkATUEnabledOnBandChange.AutoSize = true;
            this.chkATUEnabledOnBandChange.Image = null;
            this.chkATUEnabledOnBandChange.Location = new System.Drawing.Point(12, 134);
            this.chkATUEnabledOnBandChange.Name = "chkATUEnabledOnBandChange";
            this.chkATUEnabledOnBandChange.Size = new System.Drawing.Size(202, 17);
            this.chkATUEnabledOnBandChange.TabIndex = 7;
            this.chkATUEnabledOnBandChange.Text = "ATU Stays Enabled on Band Change";
            this.toolTip1.SetToolTip(this.chkATUEnabledOnBandChange, "Check this option to initiate a memory tune on band changes if ATU is enabled");
            this.chkATUEnabledOnBandChange.UseVisualStyleBackColor = true;
            // 
            // grpSWRThreshold
            // 
            this.grpSWRThreshold.Controls.Add(this.comboSWRThresh);
            this.grpSWRThreshold.Location = new System.Drawing.Point(280, 8);
            this.grpSWRThreshold.Name = "grpSWRThreshold";
            this.grpSWRThreshold.Size = new System.Drawing.Size(104, 56);
            this.grpSWRThreshold.TabIndex = 6;
            this.grpSWRThreshold.TabStop = false;
            this.grpSWRThreshold.Text = "SWR Threshold";
            this.toolTip1.SetToolTip(this.grpSWRThreshold, "Sets the threshold below which constitutes a successful tune.");
            // 
            // comboSWRThresh
            // 
            this.comboSWRThresh.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboSWRThresh.DropDownWidth = 64;
            this.comboSWRThresh.Items.AddRange(new object[] {
            "1.1 : 1",
            "1.3 : 1",
            "1.5 : 1",
            "1.7 : 1",
            "2.0 : 1",
            "2.5 : 1",
            "3.0 : 1"});
            this.comboSWRThresh.Location = new System.Drawing.Point(16, 24);
            this.comboSWRThresh.Name = "comboSWRThresh";
            this.comboSWRThresh.Size = new System.Drawing.Size(64, 21);
            this.comboSWRThresh.TabIndex = 4;
            this.comboSWRThresh.SelectedIndexChanged += new System.EventHandler(this.comboSWRThresh_SelectedIndexChanged);
            // 
            // grpFeedback
            // 
            this.grpFeedback.BackColor = System.Drawing.SystemColors.Control;
            this.grpFeedback.Controls.Add(this.indup);
            this.grpFeedback.Controls.Add(this.inddown);
            this.grpFeedback.Controls.Add(this.capup);
            this.grpFeedback.Controls.Add(this.capdown);
            this.grpFeedback.Controls.Add(this.lblIND);
            this.grpFeedback.Controls.Add(this.lblCAP);
            this.grpFeedback.Controls.Add(this.buttonTS1);
            this.grpFeedback.Controls.Add(this.lblTuneComplete);
            this.grpFeedback.Controls.Add(this.lblSWR);
            this.grpFeedback.Controls.Add(this.lblReflected);
            this.grpFeedback.Controls.Add(this.lblPower);
            this.grpFeedback.Controls.Add(this.lblFreq);
            this.grpFeedback.Controls.Add(this.lblForward);
            this.grpFeedback.Location = new System.Drawing.Point(8, 166);
            this.grpFeedback.Name = "grpFeedback";
            this.grpFeedback.Size = new System.Drawing.Size(405, 96);
            this.grpFeedback.TabIndex = 5;
            this.grpFeedback.TabStop = false;
            this.grpFeedback.Text = "Tuner Feedback";
            this.toolTip1.SetToolTip(this.grpFeedback, "The information in this window is returned from the ATU after a tune sequence.");
            // 
            // indup
            // 
            this.indup.Image = null;
            this.indup.Location = new System.Drawing.Point(374, 71);
            this.indup.Name = "indup";
            this.indup.Size = new System.Drawing.Size(31, 23);
            this.indup.TabIndex = 13;
            this.indup.Text = ">";
            this.toolTip1.SetToolTip(this.indup, "IND up (0 to 127)");
            this.indup.Click += new System.EventHandler(this.indup_Click);
            // 
            // inddown
            // 
            this.inddown.Image = null;
            this.inddown.Location = new System.Drawing.Point(254, 67);
            this.inddown.Name = "inddown";
            this.inddown.Size = new System.Drawing.Size(31, 23);
            this.inddown.TabIndex = 12;
            this.inddown.Text = "<";
            this.toolTip1.SetToolTip(this.inddown, "IND down ( 0 to 127)");
            this.inddown.Click += new System.EventHandler(this.inddown_Click);
            // 
            // capup
            // 
            this.capup.Image = null;
            this.capup.Location = new System.Drawing.Point(374, 42);
            this.capup.Name = "capup";
            this.capup.Size = new System.Drawing.Size(31, 23);
            this.capup.TabIndex = 11;
            this.capup.Text = ">";
            this.toolTip1.SetToolTip(this.capup, "CAP UP (0 to 127)");
            this.capup.Click += new System.EventHandler(this.capup_Click);
            // 
            // capdown
            // 
            this.capdown.Image = null;
            this.capdown.Location = new System.Drawing.Point(254, 41);
            this.capdown.Name = "capdown";
            this.capdown.Size = new System.Drawing.Size(31, 23);
            this.capdown.TabIndex = 10;
            this.capdown.Text = "<";
            this.toolTip1.SetToolTip(this.capdown, "Cap down (0 to 127)");
            this.capdown.Click += new System.EventHandler(this.capdown_Click);
            // 
            // lblIND
            // 
            this.lblIND.Image = null;
            this.lblIND.Location = new System.Drawing.Point(291, 71);
            this.lblIND.Name = "lblIND";
            this.lblIND.Size = new System.Drawing.Size(72, 16);
            this.lblIND.TabIndex = 9;
            this.lblIND.Text = "IND:";
            this.toolTip1.SetToolTip(this.lblIND, "Values = 0 to 127");
            // 
            // lblCAP
            // 
            this.lblCAP.Image = null;
            this.lblCAP.Location = new System.Drawing.Point(291, 46);
            this.lblCAP.Name = "lblCAP";
            this.lblCAP.Size = new System.Drawing.Size(72, 16);
            this.lblCAP.TabIndex = 8;
            this.lblCAP.Text = "CAP:";
            this.toolTip1.SetToolTip(this.lblCAP, "Values = 0 to 127");
            // 
            // buttonTS1
            // 
            this.buttonTS1.Image = null;
            this.buttonTS1.Location = new System.Drawing.Point(244, 14);
            this.buttonTS1.Name = "buttonTS1";
            this.buttonTS1.Size = new System.Drawing.Size(77, 23);
            this.buttonTS1.TabIndex = 4;
            this.buttonTS1.Text = "TUN Check";
            this.toolTip1.SetToolTip(this.buttonTS1, "Read the current SWR  as seen by the ATU and not the Flex SWR");
            this.buttonTS1.Click += new System.EventHandler(this.buttonTS1_Click);
            // 
            // lblTuneComplete
            // 
            this.lblTuneComplete.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTuneComplete.ForeColor = System.Drawing.Color.Green;
            this.lblTuneComplete.Image = null;
            this.lblTuneComplete.Location = new System.Drawing.Point(1, 19);
            this.lblTuneComplete.Name = "lblTuneComplete";
            this.lblTuneComplete.Size = new System.Drawing.Size(232, 16);
            this.lblTuneComplete.TabIndex = 7;
            this.lblTuneComplete.Text = "Tune Completed Successfully";
            // 
            // lblSWR
            // 
            this.lblSWR.Image = null;
            this.lblSWR.Location = new System.Drawing.Point(327, 19);
            this.lblSWR.Name = "lblSWR";
            this.lblSWR.Size = new System.Drawing.Size(72, 16);
            this.lblSWR.TabIndex = 3;
            this.lblSWR.Text = "SWR: 1.0:1";
            // 
            // lblReflected
            // 
            this.lblReflected.Image = null;
            this.lblReflected.Location = new System.Drawing.Point(157, 72);
            this.lblReflected.Name = "lblReflected";
            this.lblReflected.Size = new System.Drawing.Size(80, 16);
            this.lblReflected.TabIndex = 2;
            this.lblReflected.Text = "Reflected: 0";
            // 
            // lblPower
            // 
            this.lblPower.Image = null;
            this.lblPower.Location = new System.Drawing.Point(1, 72);
            this.lblPower.Name = "lblPower";
            this.lblPower.Size = new System.Drawing.Size(64, 16);
            this.lblPower.TabIndex = 1;
            this.lblPower.Text = "Power (W):   ";
            // 
            // lblFreq
            // 
            this.lblFreq.Image = null;
            this.lblFreq.Location = new System.Drawing.Point(1, 49);
            this.lblFreq.Name = "lblFreq";
            this.lblFreq.Size = new System.Drawing.Size(152, 16);
            this.lblFreq.TabIndex = 0;
            this.lblFreq.Text = "Freq (MHz): ";
            // 
            // lblForward
            // 
            this.lblForward.Image = null;
            this.lblForward.Location = new System.Drawing.Point(78, 72);
            this.lblForward.Name = "lblForward";
            this.lblForward.Size = new System.Drawing.Size(80, 16);
            this.lblForward.TabIndex = 6;
            this.lblForward.Text = "Forward: 0";
            // 
            // grpTune
            // 
            this.grpTune.Controls.Add(this.chkUseTUN);
            this.grpTune.Controls.Add(this.btnTuneFull);
            this.grpTune.Controls.Add(this.btnTuneMemory);
            this.grpTune.Location = new System.Drawing.Point(144, 8);
            this.grpTune.Name = "grpTune";
            this.grpTune.Size = new System.Drawing.Size(128, 120);
            this.grpTune.TabIndex = 4;
            this.grpTune.TabStop = false;
            this.grpTune.Text = "Tuning Options";
            // 
            // chkUseTUN
            // 
            this.chkUseTUN.Checked = true;
            this.chkUseTUN.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkUseTUN.Image = null;
            this.chkUseTUN.Location = new System.Drawing.Point(24, 88);
            this.chkUseTUN.Name = "chkUseTUN";
            this.chkUseTUN.Size = new System.Drawing.Size(80, 24);
            this.chkUseTUN.TabIndex = 3;
            this.chkUseTUN.Text = "Use TUN";
            this.toolTip1.SetToolTip(this.chkUseTUN, "Checking this box will enable the front panel TUN function when using the Memory " +
        "or Full Tune functions above.");
            // 
            // btnTuneFull
            // 
            this.btnTuneFull.Image = null;
            this.btnTuneFull.Location = new System.Drawing.Point(16, 56);
            this.btnTuneFull.Name = "btnTuneFull";
            this.btnTuneFull.Size = new System.Drawing.Size(88, 23);
            this.btnTuneFull.TabIndex = 2;
            this.btnTuneFull.Text = "Full Tune";
            this.toolTip1.SetToolTip(this.btnTuneFull, "Perform Full Tune.  Ignores any previous saved tunes on the current frequency.");
            this.btnTuneFull.Click += new System.EventHandler(this.btnTuneFull_Click);
            // 
            // btnTuneMemory
            // 
            this.btnTuneMemory.Image = null;
            this.btnTuneMemory.Location = new System.Drawing.Point(16, 24);
            this.btnTuneMemory.Name = "btnTuneMemory";
            this.btnTuneMemory.Size = new System.Drawing.Size(88, 23);
            this.btnTuneMemory.TabIndex = 1;
            this.btnTuneMemory.Text = "Memory Tune";
            this.toolTip1.SetToolTip(this.btnTuneMemory, "Performs a Memory Tune.  Uses previously saved tune settings if found for the cur" +
        "rent frequency.  If a previous setting is not found, a Full Tune is performed.");
            this.btnTuneMemory.Click += new System.EventHandler(this.btnTuneMemory_Click);
            // 
            // grpMode
            // 
            this.grpMode.Controls.Add(this.radModeAuto);
            this.grpMode.Controls.Add(this.radModeSemiAuto);
            this.grpMode.Controls.Add(this.radModeBypass);
            this.grpMode.Location = new System.Drawing.Point(8, 8);
            this.grpMode.Name = "grpMode";
            this.grpMode.Size = new System.Drawing.Size(128, 120);
            this.grpMode.TabIndex = 0;
            this.grpMode.TabStop = false;
            this.grpMode.Text = "Operating Mode";
            // 
            // radModeAuto
            // 
            this.radModeAuto.Image = null;
            this.radModeAuto.Location = new System.Drawing.Point(16, 88);
            this.radModeAuto.Name = "radModeAuto";
            this.radModeAuto.Size = new System.Drawing.Size(104, 24);
            this.radModeAuto.TabIndex = 2;
            this.radModeAuto.Text = "Automatic";
            this.toolTip1.SetToolTip(this.radModeAuto, "Activates the ATU requiring just RF to automatically begin a tuning sequence.  Th" +
        "is works for all transmission modes.  Note that manually tuning is still possibl" +
        "e in this mode.");
            this.radModeAuto.CheckedChanged += new System.EventHandler(this.radModeAuto_CheckedChanged);
            // 
            // radModeSemiAuto
            // 
            this.radModeSemiAuto.Image = null;
            this.radModeSemiAuto.Location = new System.Drawing.Point(16, 56);
            this.radModeSemiAuto.Name = "radModeSemiAuto";
            this.radModeSemiAuto.Size = new System.Drawing.Size(104, 24);
            this.radModeSemiAuto.TabIndex = 1;
            this.radModeSemiAuto.Text = "Semi-Automatic";
            this.toolTip1.SetToolTip(this.radModeSemiAuto, "Enables the ATU requiring the user to activate a tune sequence.");
            this.radModeSemiAuto.CheckedChanged += new System.EventHandler(this.radModeSemiAuto_CheckedChanged);
            // 
            // radModeBypass
            // 
            this.radModeBypass.Checked = true;
            this.radModeBypass.Image = null;
            this.radModeBypass.Location = new System.Drawing.Point(16, 24);
            this.radModeBypass.Name = "radModeBypass";
            this.radModeBypass.Size = new System.Drawing.Size(88, 24);
            this.radModeBypass.TabIndex = 0;
            this.radModeBypass.TabStop = true;
            this.radModeBypass.Text = "Bypass";
            this.toolTip1.SetToolTip(this.radModeBypass, "Effectively deactivates the ATU by unlatching all relays.");
            this.radModeBypass.CheckedChanged += new System.EventHandler(this.radModeBypass_CheckedChanged);
            // 
            // FWCATUForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(425, 274);
            this.Controls.Add(this.chkATUEnabledOnBandChange);
            this.Controls.Add(this.grpSWRThreshold);
            this.Controls.Add(this.grpFeedback);
            this.Controls.Add(this.grpTune);
            this.Controls.Add(this.grpMode);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FWCATUForm";
            this.Text = "FLEX-5000 ATU Settings";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.FWCATUForm_Closing);
            this.grpSWRThreshold.ResumeLayout(false);
            this.grpFeedback.ResumeLayout(false);
            this.grpTune.ResumeLayout(false);
            this.grpMode.ResumeLayout(false);
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
				switch(value)
				{
					case FWCATUMode.Bypass:
						radModeBypass.Checked = true;
						break;
					case FWCATUMode.SemiAutomatic:
						radModeSemiAuto.Checked = true;
						break;
					case FWCATUMode.Automatic:
						radModeAuto.Checked = true;
						break;
				}
			}
		}
        public bool ATUEnabledOnBandChange()
        {
            if (chkATUEnabledOnBandChange.Checked) return true;
            else return false;
        }

		#endregion

		#region Event Handlers

		private void FWCATUForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			this.Hide();
			e.Cancel = true;
            Common.SaveForm(this, "FWCATU");
		}

		public void DoBypass()
		{
			radModeBypass.Checked = true;
		}

		private void radModeBypass_CheckedChanged(object sender, System.EventArgs e)
		{
            if (!console.fwc_init || console.CurrentModel != Model.FLEX5000) return;
			if(radModeBypass.Checked)
			{
				if(FWCATU.AutoStatus == 1)
					FWCATU.AutoTuning(false);
				if(FWCATU.Active)
					FWCATU.Activate(false);

                if (FWC.old_atu == true)
                {
                    FWC.ATUSendCmd(9, 0, 0);   //prevent
                    Thread.Sleep(200);
                }
                else
                {
                    //Debug.WriteLine("setting antenna 1...");
                    FWCATU.SelectAntenna1();
                }
				current_tune_mode = FWCATUMode.Bypass;
				console.FWCATUBypass();

			}
			grpTune.Enabled = !radModeBypass.Checked;
		}

		private void radModeSemiAuto_CheckedChanged(object sender, System.EventArgs e)
		{
            
            if (!console.fwc_init || console.CurrentModel != Model.FLEX5000) return;
			if(radModeSemiAuto.Checked)
			{
				if(!FWCATU.Active)
					FWCATU.Activate(true);
				if(FWCATU.AutoStatus == 1)
					FWCATU.AutoTuning(false);

               if (FWC.old_atu ==  false)  //if new model
               {
                   Debug.WriteLine("setting antenna 2...");
                   FWCATU.SelectAntenna2();
               }

				current_tune_mode = FWCATUMode.SemiAutomatic;
			}
		}

		private void radModeAuto_CheckedChanged(object sender, System.EventArgs e)
		{
            if (!console.fwc_init || console.CurrentModel != Model.FLEX5000) return;
			if(radModeAuto.Checked)
			{
				if(!FWCATU.Active)
					FWCATU.Activate(true);
				if(FWCATU.AutoStatus == 0)
					FWCATU.AutoTuning(true);

                if (FWC.old_atu == false)
                {
                    Debug.WriteLine("setting antenna 2...");
                    FWCATU.SelectAntenna2();
                }
				current_tune_mode = FWCATUMode.Automatic;
			}
		}

		public void DoTuneMemory()
		{
			radModeSemiAuto.Checked = true;
			chkUseTUN.Checked = true;
			btnTuneMemory_Click(this, EventArgs.Empty);
		}

		private void btnTuneMemory_Click(object sender, System.EventArgs e)
		{
			Thread t = new Thread(new ThreadStart(TuneMemory));
			t.Name = "Memory Tune Thread";
			t.IsBackground = true;
			t.Priority = ThreadPriority.Normal;
			t.Start();
		}

		private void TuneMemory()
		{
            if (!console.fwc_init || console.CurrentModel != Model.FLEX5000) return;
			int old_tun_pwr = 50;
			btnTuneMemory.BackColor = console.ButtonSelectedColor;
			if(chkUseTUN.Checked)
			{			
				console.TUN = true;
				old_tun_pwr = console.PWR;
				console.PWR = 10;
			}
			FWCATU.MemoryTune();
			if(chkUseTUN.Checked)
			{
				console.PWR = old_tun_pwr;
				console.TUN = false;				
			}
			btnTuneMemory.BackColor = SystemColors.Control;
			UpdateFeedback();
			console.FWCATUTuned();
		}

		public void DoTuneFull()
		{
			radModeSemiAuto.Checked = true;
			chkUseTUN.Checked = true;
			btnTuneFull_Click(this, EventArgs.Empty);
		}

		private void btnTuneFull_Click(object sender, System.EventArgs e)
		{
			Thread t = new Thread(new ThreadStart(TuneFull));
			t.Name = "Full Tune Thread";
			t.IsBackground = true;
			t.Priority = ThreadPriority.Normal;
			t.Start();
		}

		private void TuneFull()
		{
            if (!console.fwc_init || console.CurrentModel != Model.FLEX5000) return;
			int old_tun_pwr = 50;
			btnTuneFull.BackColor = console.ButtonSelectedColor;
			if(chkUseTUN.Checked)
			{
				console.TUN = true;
				old_tun_pwr = console.PWR;
				console.PWR = 10;
			}
			FWCATU.FullTune();
			if(chkUseTUN.Checked)
			{
				console.PWR = old_tun_pwr;
				console.TUN = false;				
			}
			btnTuneFull.BackColor = SystemColors.Control;
			UpdateFeedback();
			console.FWCATUTuned();
		}

		private void UpdateFeedback()
		{
			if(FWCATU.TunePass)
			{
				lblTuneComplete.ForeColor = Color.Green;
				lblTuneComplete.Text = "Tune Completed Successfully";
				lblFreq.Text = "Freq (MHz): "+FWCATU.TXFreq.ToString("f2");
				lblForward.Text = "Forward: "+FWCATU.ForwardPower.ToString("f0");
				lblReflected.Text = "Reflected: "+FWCATU.ReflectedPower.ToString("f0");

                lblSWR.Text = "SWR: "+FWCATU.SWR.ToString("f1");
                lblCAP.Text = "CAP: " + FWCATU.CapacitorValue.ToString();
                lblIND.Text = "IND: " + FWCATU.InductorValue.ToString();

            }
            else
			{
				lblTuneComplete.ForeColor = Color.Red;
				switch(FWCATU.TuneFail)
				{
					case 0:
						lblTuneComplete.Text = "Tune Failed: No RF Detected";
						break;
					case 1:
						lblTuneComplete.Text = "Tune Failed: RF Carrier Lost";
						break;
					case 2:
						lblTuneComplete.Text = "Tune Failed: Could Not Bring Down SWR";
						break;
				}
				lblFreq.Text = "Freq (MHz):";
				lblForward.Text = "Forward:";
				lblReflected.Text = "Reflected:";
				lblSWR.Text = "SWR:";
                lblCAP.Text = "CAP:";
                lblIND.Text = "IND:";
            }
		}

		private void comboSWRThresh_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			double swr_thresh = 0.0;
			switch((byte)comboSWRThresh.SelectedIndex)
			{
				case 0: swr_thresh = 1.1; break;
				case 1: swr_thresh = 1.3; break;
				case 2: swr_thresh = 1.5; break;
				case 3: swr_thresh = 1.7; break;
				case 4: swr_thresh = 2.0; break;
				case 5: swr_thresh = 2.5; break;
				case 6: swr_thresh = 3.0; break;
			}
			if(FWCATU.SWRThreshold != swr_thresh)
				FWCATU.SetSWRThreshold(swr_thresh);

       //     byte b1, b2, b3, b4;
       //     do
       //     {
       //         FWC.ATUGetResult(out b1, out b2, out b3, out b4, 200);
       //     } while (b4 > 0);
		}





        // ke9ns add to force a SWR read from the built in ATU tuner
        private void buttonTS1_Click(object sender, EventArgs e)
        {

            Thread t = new Thread(new ThreadStart(READSWR));
            t.Name = "SWR Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.Normal;
            t.Start();

        }

        public void READSWR()
        {
            SWRRUN = true;
            int old_tun_pwr = 50;

            do
            {
                if (ADJUST == 1) FWCATU.DecrementCapacitance(); // ADJUST set by <> buttons on FWC ATU screen
                else if (ADJUST == 2) FWCATU.IncrementCapacitance();
                else if (ADJUST == 3) FWCATU.DecrementInductance();
                else if (ADJUST == 4) FWCATU.IncrementInductance();


                ADJUST = 0; // reset back 

                if (!console.fwc_init || console.CurrentModel != Model.FLEX5000) return;
               
                btnTuneMemory.BackColor = console.ButtonSelectedColor;
                if (chkUseTUN.Checked)
                {
                    console.TUN = true;
                 //   old_tun_pwr = console.PWR;
                  //  console.PWR = 10;
                }
                FWCATU.ReadSWR(); // AT-200 sends all updated data to me
                UpdateFeedback();
            }
            while (ADJUST != 0); // if you push <> button before done, then keep going


            if (chkUseTUN.Checked)
            {
              //  console.PWR = old_tun_pwr;
                console.TUN = false;
            }
            btnTuneMemory.BackColor = SystemColors.Control;
            UpdateFeedback();

            SWRRUN = false; // reset to allow this thread to start again 

        } // buttonTS1_Click
        #endregion


        // ke9ns add

        public int ADJUST = 0; // 1=capdown, 2=capup, 3=ind down, 4=ind up
        public bool SWRRUN = false;

        private void capdown_Click(object sender, EventArgs e)
        {

            ADJUST = 1;

            if (SWRRUN == false)
            {
                Thread t = new Thread(new ThreadStart(READSWR));
                t.Name = "SWR Thread";
                t.IsBackground = true;
                t.Priority = ThreadPriority.Normal;
                t.Start();
            }


        }

        private void capup_Click(object sender, EventArgs e)
        {
            ADJUST = 2;

            if (SWRRUN == false)
            {
                Thread t = new Thread(new ThreadStart(READSWR));
                t.Name = "SWR Thread";
                t.IsBackground = true;
                t.Priority = ThreadPriority.Normal;
                t.Start();
            }
        }

        private void inddown_Click(object sender, EventArgs e)
        {
            ADJUST = 3;

            if (SWRRUN == false)
            {
                Thread t = new Thread(new ThreadStart(READSWR));
                t.Name = "SWR Thread";
                t.IsBackground = true;
                t.Priority = ThreadPriority.Normal;
                t.Start();
            }

        }

        private void indup_Click(object sender, EventArgs e)
        {

            ADJUST = 4;

            if (SWRRUN == false)
            {
                Thread t = new Thread(new ThreadStart(READSWR));
                t.Name = "SWR Thread";
                t.IsBackground = true;
                t.Priority = ThreadPriority.Normal;
                t.Start();
            }
        }
    } // class FWCATUForm

} // PowerSDR
