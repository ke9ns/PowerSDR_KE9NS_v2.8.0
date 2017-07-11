//=================================================================
// FLEX1500MixerForm.cs
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
	public class FLEX1500MixerForm : System.Windows.Forms.Form
	{
		#region Variable Declaration

		private Console console;
		private System.Windows.Forms.TrackBarTS tbMic;
		private System.Windows.Forms.GroupBoxTS grpInput;
		private System.Windows.Forms.LabelTS lblMic;
		private System.Windows.Forms.GroupBoxTS grpOutput;
		private System.Windows.Forms.CheckBoxTS chkInputMuteAll;
		private System.Windows.Forms.CheckBoxTS chkMicSel;
		private System.Windows.Forms.LabelTS lblFlexWireIn;
		private System.Windows.Forms.CheckBoxTS chkFlexWireInSel;
		private System.Windows.Forms.TrackBarTS tbFlexWireIn;
		private System.Windows.Forms.CheckBoxTS chkOutputMuteAll;
		private System.Windows.Forms.LabelTS lblPhones;
        public CheckBoxTS chkPhonesSel;
        private System.Windows.Forms.TrackBarTS tbPhones;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.LabelTS lblFlexWireOut;
        public CheckBoxTS chkFlexWireOutSel;
        private System.Windows.Forms.TrackBarTS tbFlexWireOut;
        private ButtonTS btnCodecDump;
		private System.ComponentModel.IContainer components;

		#endregion

		#region Constructor and Destructor

		public FLEX1500MixerForm(Console c)
		{
			InitializeComponent();
			console = c;
			
			Common.RestoreForm(this, "FLEX1500MixerForm", false);

			chkMicSel_CheckedChanged(this, EventArgs.Empty);
			chkFlexWireInSel_CheckedChanged(this, EventArgs.Empty);
			chkPhonesSel_CheckedChanged(this, EventArgs.Empty);
			chkFlexWireOutSel_CheckedChanged(this, EventArgs.Empty);
			tbMic_Scroll(this, EventArgs.Empty);
			tbFlexWireIn_Scroll(this, EventArgs.Empty);
			tbPhones_Scroll(this, EventArgs.Empty);
			tbFlexWireOut_Scroll(this, EventArgs.Empty);
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

		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FLEX1500MixerForm));
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.chkFlexWireOutSel = new System.Windows.Forms.CheckBoxTS();
            this.tbFlexWireOut = new System.Windows.Forms.TrackBarTS();
            this.chkPhonesSel = new System.Windows.Forms.CheckBoxTS();
            this.tbPhones = new System.Windows.Forms.TrackBarTS();
            this.chkOutputMuteAll = new System.Windows.Forms.CheckBoxTS();
            this.chkInputMuteAll = new System.Windows.Forms.CheckBoxTS();
            this.lblFlexWireIn = new System.Windows.Forms.LabelTS();
            this.chkFlexWireInSel = new System.Windows.Forms.CheckBoxTS();
            this.tbFlexWireIn = new System.Windows.Forms.TrackBarTS();
            this.lblMic = new System.Windows.Forms.LabelTS();
            this.chkMicSel = new System.Windows.Forms.CheckBoxTS();
            this.tbMic = new System.Windows.Forms.TrackBarTS();
            this.grpInput = new System.Windows.Forms.GroupBoxTS();
            this.btnCodecDump = new System.Windows.Forms.ButtonTS();
            this.grpOutput = new System.Windows.Forms.GroupBoxTS();
            this.lblFlexWireOut = new System.Windows.Forms.LabelTS();
            this.lblPhones = new System.Windows.Forms.LabelTS();
            ((System.ComponentModel.ISupportInitialize)(this.tbFlexWireOut)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbPhones)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbFlexWireIn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbMic)).BeginInit();
            this.grpInput.SuspendLayout();
            this.grpOutput.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkFlexWireOutSel
            // 
            this.chkFlexWireOutSel.Checked = true;
            this.chkFlexWireOutSel.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkFlexWireOutSel.Image = null;
            this.chkFlexWireOutSel.Location = new System.Drawing.Point(94, 144);
            this.chkFlexWireOutSel.Name = "chkFlexWireOutSel";
            this.chkFlexWireOutSel.Size = new System.Drawing.Size(16, 24);
            this.chkFlexWireOutSel.TabIndex = 21;
            this.chkFlexWireOutSel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.chkFlexWireOutSel, "Selects/Unselects the FlexWire Line Out Output");
            this.chkFlexWireOutSel.CheckedChanged += new System.EventHandler(this.chkFlexWireOutSel_CheckedChanged);
            // 
            // tbFlexWireOut
            // 
            this.tbFlexWireOut.Location = new System.Drawing.Point(82, 48);
            this.tbFlexWireOut.Maximum = 127;
            this.tbFlexWireOut.Name = "tbFlexWireOut";
            this.tbFlexWireOut.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbFlexWireOut.Size = new System.Drawing.Size(45, 104);
            this.tbFlexWireOut.TabIndex = 20;
            this.tbFlexWireOut.TickFrequency = 16;
            this.tbFlexWireOut.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.toolTip1.SetToolTip(this.tbFlexWireOut, "Adjusts the FlexWire Out Volume");
            this.tbFlexWireOut.Value = 95;
            this.tbFlexWireOut.Scroll += new System.EventHandler(this.tbFlexWireOut_Scroll);
            // 
            // chkPhonesSel
            // 
            this.chkPhonesSel.Checked = true;
            this.chkPhonesSel.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPhonesSel.Image = null;
            this.chkPhonesSel.Location = new System.Drawing.Point(40, 144);
            this.chkPhonesSel.Name = "chkPhonesSel";
            this.chkPhonesSel.Size = new System.Drawing.Size(16, 24);
            this.chkPhonesSel.TabIndex = 15;
            this.chkPhonesSel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.chkPhonesSel, "Selects/Unselects the Phones Output");
            this.chkPhonesSel.CheckedChanged += new System.EventHandler(this.chkPhonesSel_CheckedChanged);
            // 
            // tbPhones
            // 
            this.tbPhones.Location = new System.Drawing.Point(24, 48);
            this.tbPhones.Maximum = 127;
            this.tbPhones.Name = "tbPhones";
            this.tbPhones.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbPhones.Size = new System.Drawing.Size(45, 104);
            this.tbPhones.TabIndex = 14;
            this.tbPhones.TickFrequency = 16;
            this.tbPhones.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.toolTip1.SetToolTip(this.tbPhones, "Adjusts the Phones Volume");
            this.tbPhones.Value = 95;
            this.tbPhones.Scroll += new System.EventHandler(this.tbPhones_Scroll);
            // 
            // chkOutputMuteAll
            // 
            this.chkOutputMuteAll.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkOutputMuteAll.Image = null;
            this.chkOutputMuteAll.Location = new System.Drawing.Point(19, 176);
            this.chkOutputMuteAll.Name = "chkOutputMuteAll";
            this.chkOutputMuteAll.Size = new System.Drawing.Size(103, 24);
            this.chkOutputMuteAll.TabIndex = 13;
            this.chkOutputMuteAll.Text = "Mute All Outputs";
            this.chkOutputMuteAll.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.chkOutputMuteAll, "Mute All Output Lines");
            this.chkOutputMuteAll.CheckedChanged += new System.EventHandler(this.chkOutputMuteAll_CheckedChanged);
            // 
            // chkInputMuteAll
            // 
            this.chkInputMuteAll.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkInputMuteAll.Image = null;
            this.chkInputMuteAll.Location = new System.Drawing.Point(16, 176);
            this.chkInputMuteAll.Name = "chkInputMuteAll";
            this.chkInputMuteAll.Size = new System.Drawing.Size(88, 24);
            this.chkInputMuteAll.TabIndex = 12;
            this.chkInputMuteAll.Text = "Mute All Inputs";
            this.chkInputMuteAll.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.chkInputMuteAll, "Mute All Input Lines");
            this.chkInputMuteAll.CheckedChanged += new System.EventHandler(this.chkInputMuteAll_CheckedChanged);
            // 
            // lblFlexWireIn
            // 
            this.lblFlexWireIn.Image = null;
            this.lblFlexWireIn.Location = new System.Drawing.Point(64, 24);
            this.lblFlexWireIn.Name = "lblFlexWireIn";
            this.lblFlexWireIn.Size = new System.Drawing.Size(49, 24);
            this.lblFlexWireIn.TabIndex = 11;
            this.lblFlexWireIn.Text = "FlexWire In";
            this.lblFlexWireIn.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.toolTip1.SetToolTip(this.lblFlexWireIn, "FlexWire Audio Input (DB9 on back panel)");
            // 
            // chkFlexWireInSel
            // 
            this.chkFlexWireInSel.Image = null;
            this.chkFlexWireInSel.Location = new System.Drawing.Point(81, 144);
            this.chkFlexWireInSel.Name = "chkFlexWireInSel";
            this.chkFlexWireInSel.Size = new System.Drawing.Size(16, 24);
            this.chkFlexWireInSel.TabIndex = 10;
            this.chkFlexWireInSel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.chkFlexWireInSel, "Selects/Unselects the Line In DB9 Input");
            this.chkFlexWireInSel.CheckedChanged += new System.EventHandler(this.chkFlexWireInSel_CheckedChanged);
            // 
            // tbFlexWireIn
            // 
            this.tbFlexWireIn.Location = new System.Drawing.Point(68, 48);
            this.tbFlexWireIn.Maximum = 119;
            this.tbFlexWireIn.Name = "tbFlexWireIn";
            this.tbFlexWireIn.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbFlexWireIn.Size = new System.Drawing.Size(45, 104);
            this.tbFlexWireIn.TabIndex = 9;
            this.tbFlexWireIn.TickFrequency = 20;
            this.tbFlexWireIn.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.toolTip1.SetToolTip(this.tbFlexWireIn, resources.GetString("tbFlexWireIn.ToolTip"));
            this.tbFlexWireIn.Value = 60;
            this.tbFlexWireIn.Scroll += new System.EventHandler(this.tbFlexWireIn_Scroll);
            // 
            // lblMic
            // 
            this.lblMic.Image = null;
            this.lblMic.Location = new System.Drawing.Point(16, 24);
            this.lblMic.Name = "lblMic";
            this.lblMic.Size = new System.Drawing.Size(45, 24);
            this.lblMic.TabIndex = 2;
            this.lblMic.Text = "Mic";
            this.lblMic.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.toolTip1.SetToolTip(this.lblMic, "8 Pin front panel connector");
            // 
            // chkMicSel
            // 
            this.chkMicSel.Checked = true;
            this.chkMicSel.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMicSel.Image = null;
            this.chkMicSel.Location = new System.Drawing.Point(30, 144);
            this.chkMicSel.Name = "chkMicSel";
            this.chkMicSel.Size = new System.Drawing.Size(16, 24);
            this.chkMicSel.TabIndex = 1;
            this.chkMicSel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.chkMicSel, "Selects/Unselects the Mic Input");
            this.chkMicSel.CheckedChanged += new System.EventHandler(this.chkMicSel_CheckedChanged);
            // 
            // tbMic
            // 
            this.tbMic.Location = new System.Drawing.Point(16, 48);
            this.tbMic.Maximum = 119;
            this.tbMic.Name = "tbMic";
            this.tbMic.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbMic.Size = new System.Drawing.Size(45, 104);
            this.tbMic.TabIndex = 0;
            this.tbMic.TickFrequency = 20;
            this.tbMic.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.toolTip1.SetToolTip(this.tbMic, resources.GetString("tbMic.ToolTip"));
            this.tbMic.Value = 60;
            this.tbMic.Scroll += new System.EventHandler(this.tbMic_Scroll);
            // 
            // grpInput
            // 
            this.grpInput.Controls.Add(this.chkInputMuteAll);
            this.grpInput.Controls.Add(this.lblFlexWireIn);
            this.grpInput.Controls.Add(this.chkFlexWireInSel);
            this.grpInput.Controls.Add(this.tbFlexWireIn);
            this.grpInput.Controls.Add(this.lblMic);
            this.grpInput.Controls.Add(this.chkMicSel);
            this.grpInput.Controls.Add(this.tbMic);
            this.grpInput.Location = new System.Drawing.Point(8, 8);
            this.grpInput.Name = "grpInput";
            this.grpInput.Size = new System.Drawing.Size(120, 216);
            this.grpInput.TabIndex = 2;
            this.grpInput.TabStop = false;
            this.grpInput.Text = "Input (Stage 1)";
            this.toolTip1.SetToolTip(this.grpInput, resources.GetString("grpInput.ToolTip"));
            // 
            // btnCodecDump
            // 
            this.btnCodecDump.Image = null;
            this.btnCodecDump.Location = new System.Drawing.Point(76, -4);
            this.btnCodecDump.Name = "btnCodecDump";
            this.btnCodecDump.Size = new System.Drawing.Size(75, 23);
            this.btnCodecDump.TabIndex = 4;
            this.btnCodecDump.Text = "Dump";
            this.btnCodecDump.UseVisualStyleBackColor = true;
            this.btnCodecDump.Visible = false;
            this.btnCodecDump.Click += new System.EventHandler(this.btnCodecDump_Click);
            // 
            // grpOutput
            // 
            this.grpOutput.Controls.Add(this.lblFlexWireOut);
            this.grpOutput.Controls.Add(this.chkFlexWireOutSel);
            this.grpOutput.Controls.Add(this.tbFlexWireOut);
            this.grpOutput.Controls.Add(this.lblPhones);
            this.grpOutput.Controls.Add(this.chkPhonesSel);
            this.grpOutput.Controls.Add(this.tbPhones);
            this.grpOutput.Controls.Add(this.chkOutputMuteAll);
            this.grpOutput.Location = new System.Drawing.Point(136, 8);
            this.grpOutput.Name = "grpOutput";
            this.grpOutput.Size = new System.Drawing.Size(137, 216);
            this.grpOutput.TabIndex = 3;
            this.grpOutput.TabStop = false;
            this.grpOutput.Text = "Output";
            // 
            // lblFlexWireOut
            // 
            this.lblFlexWireOut.Image = null;
            this.lblFlexWireOut.Location = new System.Drawing.Point(78, 24);
            this.lblFlexWireOut.Name = "lblFlexWireOut";
            this.lblFlexWireOut.Size = new System.Drawing.Size(49, 32);
            this.lblFlexWireOut.TabIndex = 22;
            this.lblFlexWireOut.Text = "FlexWire Out";
            this.lblFlexWireOut.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblPhones
            // 
            this.lblPhones.Image = null;
            this.lblPhones.Location = new System.Drawing.Point(16, 24);
            this.lblPhones.Name = "lblPhones";
            this.lblPhones.Size = new System.Drawing.Size(56, 32);
            this.lblPhones.TabIndex = 16;
            this.lblPhones.Text = "Phones";
            this.lblPhones.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // FLEX1500MixerForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(281, 230);
            this.Controls.Add(this.btnCodecDump);
            this.Controls.Add(this.grpOutput);
            this.Controls.Add(this.grpInput);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "FLEX1500MixerForm";
            this.Text = "FLEX-1500 Audio Mixer";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.FLEX1500MixerForm_Closing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FLEX1500MixerForm_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.tbFlexWireOut)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbPhones)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbFlexWireIn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbMic)).EndInit();
            this.grpInput.ResumeLayout(false);
            this.grpInput.PerformLayout();
            this.grpOutput.ResumeLayout(false);
            this.grpOutput.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		#region Properties

		public int MicInput
		{
			get { return tbMic.Value; }
			set
			{
				tbMic.Value = value;
				tbMic_Scroll(this,EventArgs.Empty);
            }
		}

		public int FlexWireIn
		{
			get { return tbFlexWireIn.Value; }
			set
			{
				tbFlexWireIn.Value = value;
				tbFlexWireIn_Scroll(this,EventArgs.Empty);
            }
		}

        public bool MicInputSelected
        {
            get { return chkMicSel.Checked; }
            set
            {
                if (value) chkMicSel.Checked = true;
                else chkFlexWireInSel.Checked = true;
            }
        }

		public string MicInputSelectedStr
		{
			get
			{
				if(chkMicSel.Checked)
					return "1";
				else
					return "0";}

			set
			{
				if(value == "1")
					chkMicSel.Checked = true;
				else
					chkMicSel.Checked = false;
			}
		}

		public string LineInDB9Selected
		{
			get
			{
				if(chkFlexWireInSel.Checked)
					return "1";
				else
					return "0";}

			set
			{
				if(value == "1")
					chkFlexWireInSel.Checked = true;
				else
					chkFlexWireInSel.Checked = false;
			}
		}

		public string InputMuteAll
		{
			get
			{
				if(chkInputMuteAll.Checked)
					return "1";
				else
					return "0";}

			set
			{
				if(value == "1")
					chkInputMuteAll.Checked = true;
				else
					chkInputMuteAll.Checked = false;
			}
		}

		public int Phones
		{
			get{return tbPhones.Value;}
			set
			{
				tbPhones.Value = value;
				tbPhones_Scroll(this,EventArgs.Empty);}
		}

		public int FlexWireOut
		{
			get{return tbFlexWireOut.Value;}
			set
			{
				tbFlexWireOut.Value = value;
				tbFlexWireOut_Scroll(this,EventArgs.Empty);}
		}

        public bool PhonesSelected
        {
            get { return chkPhonesSel.Checked; }
            set { chkPhonesSel.Checked = value; }
        }

		public string PhonesSelectedStr
		{
			get
			{
				if(chkPhonesSel.Checked)
					return "1";
				else
					return "0";}

			set
			{
				if(value == "1")
					chkPhonesSel.Checked = true;
				else
					chkPhonesSel.Checked = false;
			}
		}

        public bool FlexWireOutSelected
        {
            get { return chkFlexWireOutSel.Checked; }
            set { chkFlexWireOutSel.Checked = value; }
        }

		public string FlexWireOutSelectedStr
		{
			get
			{
				if(chkFlexWireOutSel.Checked)
					return "1";
				else
					return "0";}

			set
			{
				if(value == "1")
					chkFlexWireOutSel.Checked = true;
				else
					chkFlexWireOutSel.Checked = false;
			}
		}

		public string OutputMuteAll
		{
			get
			{
				if(chkOutputMuteAll.Checked)
					return "1";
				else
					return "0";}

			set
			{
				if(value == "1")
					chkOutputMuteAll.Checked = true;
				else
					chkOutputMuteAll.Checked = false;
			}
		}

		#endregion Properties

		#region Input Event Handlers

		private void tbMic_Scroll(object sender, System.EventArgs e)
		{
            if (chkMicSel.Checked && !chkInputMuteAll.Checked)
            {
                //Debug.WriteLine("Mic Vol: 0x" + tbMic.Value.ToString("X").PadLeft(2, '0'));
                USBHID.SetTXGain(tbMic.Value);
            }
		}

		private void tbFlexWireIn_Scroll(object sender, System.EventArgs e)
		{
            if (chkFlexWireInSel.Checked && !chkInputMuteAll.Checked)
            {
                int val = tbFlexWireIn.Value;
                double percent = (val - tbFlexWireIn.Minimum) / (double)(tbFlexWireIn.Maximum - tbFlexWireIn.Minimum);
                int reg_val = (int)(0x2D * percent);
                USBHID.SetTXGain(reg_val);
                Debug.WriteLine("reg_val: " + reg_val);
                //USBHID.SetTXGain(tbFlexWireIn.Value);
            }
		}

		private void chkMicSel_CheckedChanged(object sender, System.EventArgs e)
		{
            if (chkMicSel.Checked)
            {
                chkFlexWireInSel.Checked = false;

                if (!chkInputMuteAll.Checked)
                {
                    USBHID.SetMicSel(true);
                    //Thread.Sleep(10);
                    USBHID.SetTXGain(tbMic.Value);
                }
            }
            else if (!chkFlexWireInSel.Checked)
                chkMicSel.Checked = true;
		}

		private void chkFlexWireInSel_CheckedChanged(object sender, System.EventArgs e)
		{
            if (chkFlexWireInSel.Checked)
            {
                chkMicSel.Checked = false;

                if (!chkInputMuteAll.Checked)
                {
                    USBHID.SetMicSel(false);
                    //Thread.Sleep(10);
                    USBHID.SetTXGain(tbFlexWireIn.Value);
                }
            }
            else if (!chkMicSel.Checked)
                chkFlexWireInSel.Checked = true;
		}

		private void chkInputMuteAll_CheckedChanged(object sender, System.EventArgs e)
		{
			if(chkInputMuteAll.Checked)
			{
				chkInputMuteAll.BackColor = console.ButtonSelectedColor;
                USBHID.SetTXGain(0x80);
			}
			else
			{
				chkInputMuteAll.BackColor = SystemColors.Control;
				if(chkMicSel.Checked) tbMic_Scroll(this, EventArgs.Empty);
				if(chkFlexWireInSel.Checked) tbFlexWireIn_Scroll(this, EventArgs.Empty);
			}
		}

		#endregion

		#region Output Event Handlers

		private void tbPhones_Scroll(object sender, System.EventArgs e)
		{
            USBHID.SetSpkGain(0x80 + 127 - tbPhones.Value);
		}

		private void tbFlexWireOut_Scroll(object sender, System.EventArgs e)
		{
			USBHID.SetLineOutGain(0x80 + 127 - tbFlexWireOut.Value);
		}

		private void chkPhonesSel_CheckedChanged(object sender, System.EventArgs e)
		{
            if (!chkOutputMuteAll.Checked)
                USBHID.SetSpkOn(chkPhonesSel.Checked);
            else USBHID.SetSpkOn(false);
		}

		private void chkFlexWireOutSel_CheckedChanged(object sender, System.EventArgs e)
		{
            if (!chkOutputMuteAll.Checked)
                USBHID.SetLineOutOn(chkFlexWireOutSel.Checked);
            else USBHID.SetLineOutOn(false);
		}

		private void chkOutputMuteAll_CheckedChanged(object sender, System.EventArgs e)
		{
			if(chkOutputMuteAll.Checked)
			{
				chkOutputMuteAll.BackColor = console.ButtonSelectedColor;
			}
			else
			{
				chkOutputMuteAll.BackColor = SystemColors.ControlLight;
            }

            chkPhonesSel_CheckedChanged(this, EventArgs.Empty); Thread.Sleep(20);
			chkFlexWireOutSel_CheckedChanged(this, EventArgs.Empty);			
		}

		#endregion

		#region Other Event Handlers

		private void FLEX1500MixerForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			this.Hide();
			e.Cancel = true;
			Common.SaveForm(this, "FLEX1500MixerForm");
		}

		#endregion

        private int dump_num = 0;
        private void btnCodecDump_Click(object sender, EventArgs e)
        {
            CodecDebug.Dump("dump" + (dump_num++).ToString() + ".txt");
        }

        private void FLEX1500MixerForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.Alt && e.KeyCode == Keys.D)
                btnCodecDump.Visible = true;
        }
	}
}
