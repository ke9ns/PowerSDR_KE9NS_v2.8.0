//=================================================================
// wave_options.cs
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
    /// <summary>
    /// Summary description for WaveOptions.
    /// </summary>
    public partial class WaveOptions : System.Windows.Forms.Form
    {
        

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WaveOptions));
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.comboSampleRate = new System.Windows.Forms.ComboBoxTS();
            this.radTXPostProcessed = new System.Windows.Forms.RadioButtonTS();
            this.radTXPreProcessed = new System.Windows.Forms.RadioButtonTS();
            this.radRXPostProcessed = new System.Windows.Forms.RadioButtonTS();
            this.radRXPreProcessed = new System.Windows.Forms.RadioButtonTS();
            this.txtWaveOptionsForm = new System.Windows.Forms.TextBox();
            this.grpAudioSampleRate1 = new System.Windows.Forms.GroupBoxTS();
            this.groupBox1 = new System.Windows.Forms.GroupBoxTS();
            this.grpReceive = new System.Windows.Forms.GroupBoxTS();
            this.grpAudioSampleRate1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.grpReceive.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboSampleRate
            // 
            this.comboSampleRate.Cursor = System.Windows.Forms.Cursors.Default;
            this.comboSampleRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboSampleRate.DropDownWidth = 64;
            this.comboSampleRate.Enabled = false;
            this.comboSampleRate.Items.AddRange(new object[] {
            "6000",
            "12000",
            "24000",
            "48000",
            "96000",
            "192000"});
            this.comboSampleRate.Location = new System.Drawing.Point(16, 24);
            this.comboSampleRate.Name = "comboSampleRate";
            this.comboSampleRate.Size = new System.Drawing.Size(64, 21);
            this.comboSampleRate.TabIndex = 4;
            this.toolTip1.SetToolTip(this.comboSampleRate, "Sample Rate -- Set the sampling rate for a post processed wave file ");
            // 
            // radTXPostProcessed
            // 
            this.radTXPostProcessed.Checked = true;
            this.radTXPostProcessed.Image = null;
            this.radTXPostProcessed.Location = new System.Drawing.Point(16, 48);
            this.radTXPostProcessed.Name = "radTXPostProcessed";
            this.radTXPostProcessed.Size = new System.Drawing.Size(144, 24);
            this.radTXPostProcessed.TabIndex = 1;
            this.radTXPostProcessed.TabStop = true;
            this.radTXPostProcessed.Text = "Post-Processed Audio";
            this.toolTip1.SetToolTip(this.radTXPostProcessed, "Filtered, modulated, EQ\'d, and AGC\'d Signal");
            this.radTXPostProcessed.CheckedChanged += new System.EventHandler(this.radTXPostProcessed_CheckedChanged);
            // 
            // radTXPreProcessed
            // 
            this.radTXPreProcessed.Image = null;
            this.radTXPreProcessed.Location = new System.Drawing.Point(16, 24);
            this.radTXPreProcessed.Name = "radTXPreProcessed";
            this.radTXPreProcessed.Size = new System.Drawing.Size(144, 24);
            this.radTXPreProcessed.TabIndex = 0;
            this.radTXPreProcessed.Text = "Pre-Processed Audio";
            this.toolTip1.SetToolTip(this.radTXPreProcessed, "Raw Input (Microphone, Digital, etc)");
            this.radTXPreProcessed.CheckedChanged += new System.EventHandler(this.radTXPreProcessed_CheckedChanged);
            // 
            // radRXPostProcessed
            // 
            this.radRXPostProcessed.Image = null;
            this.radRXPostProcessed.Location = new System.Drawing.Point(16, 48);
            this.radRXPostProcessed.Name = "radRXPostProcessed";
            this.radRXPostProcessed.Size = new System.Drawing.Size(144, 24);
            this.radRXPostProcessed.TabIndex = 1;
            this.radRXPostProcessed.Text = "Post-Processed Audio";
            this.toolTip1.SetToolTip(this.radRXPostProcessed, "The demodulated filtered audio you listen to.");
            this.radRXPostProcessed.CheckedChanged += new System.EventHandler(this.radRXPostProcessed_CheckedChanged);
            // 
            // radRXPreProcessed
            // 
            this.radRXPreProcessed.Checked = true;
            this.radRXPreProcessed.Image = null;
            this.radRXPreProcessed.Location = new System.Drawing.Point(16, 24);
            this.radRXPreProcessed.Name = "radRXPreProcessed";
            this.radRXPreProcessed.Size = new System.Drawing.Size(144, 24);
            this.radRXPreProcessed.TabIndex = 0;
            this.radRXPreProcessed.TabStop = true;
            this.radRXPreProcessed.Text = "Pre-Processed Audio";
            this.toolTip1.SetToolTip(this.radRXPreProcessed, "The raw audio coming out of the radio (11kHz IF)");
            this.radRXPreProcessed.CheckedChanged += new System.EventHandler(this.radRXPreProcessed_CheckedChanged);
            // 
            // txtWaveOptionsForm
            // 
            this.txtWaveOptionsForm.Location = new System.Drawing.Point(182, 12);
            this.txtWaveOptionsForm.Multiline = true;
            this.txtWaveOptionsForm.Name = "txtWaveOptionsForm";
            this.txtWaveOptionsForm.ReadOnly = true;
            this.txtWaveOptionsForm.Size = new System.Drawing.Size(175, 226);
            this.txtWaveOptionsForm.TabIndex = 37;
            this.txtWaveOptionsForm.Text = resources.GetString("txtWaveOptionsForm.Text");
            // 
            // grpAudioSampleRate1
            // 
            this.grpAudioSampleRate1.Controls.Add(this.comboSampleRate);
            this.grpAudioSampleRate1.Location = new System.Drawing.Point(8, 182);
            this.grpAudioSampleRate1.Name = "grpAudioSampleRate1";
            this.grpAudioSampleRate1.Size = new System.Drawing.Size(168, 56);
            this.grpAudioSampleRate1.TabIndex = 36;
            this.grpAudioSampleRate1.TabStop = false;
            this.grpAudioSampleRate1.Text = "Wave File Sample Rate";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radTXPostProcessed);
            this.groupBox1.Controls.Add(this.radTXPreProcessed);
            this.groupBox1.Location = new System.Drawing.Point(8, 96);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(168, 80);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Transmit";
            // 
            // grpReceive
            // 
            this.grpReceive.Controls.Add(this.radRXPostProcessed);
            this.grpReceive.Controls.Add(this.radRXPreProcessed);
            this.grpReceive.Location = new System.Drawing.Point(8, 8);
            this.grpReceive.Name = "grpReceive";
            this.grpReceive.Size = new System.Drawing.Size(168, 80);
            this.grpReceive.TabIndex = 0;
            this.grpReceive.TabStop = false;
            this.grpReceive.Text = "Receive";
            // 
            // WaveOptions
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(367, 246);
            this.Controls.Add(this.txtWaveOptionsForm);
            this.Controls.Add(this.grpAudioSampleRate1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.grpReceive);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "WaveOptions";
            this.Text = "Wave Record Options";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.WaveOptions_Closing);
            this.grpAudioSampleRate1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.grpReceive.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

      


        private System.Windows.Forms.GroupBoxTS grpReceive;
        public System.Windows.Forms.RadioButtonTS radRXPreProcessed;
        private System.Windows.Forms.GroupBoxTS groupBox1;
        private System.Windows.Forms.ToolTip toolTip1;
        public System.Windows.Forms.RadioButtonTS radRXPostProcessed;
        public System.Windows.Forms.RadioButtonTS radTXPostProcessed;
        public System.Windows.Forms.RadioButtonTS radTXPreProcessed;
        private System.Windows.Forms.GroupBoxTS grpAudioSampleRate1;
        public System.Windows.Forms.ComboBoxTS comboSampleRate;
        private System.Windows.Forms.TextBox txtWaveOptionsForm;
        private System.ComponentModel.IContainer components;

   
    }
}
