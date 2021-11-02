//=================================================================
// hidAntForm.cs
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
    public partial class HIDAntForm : System.Windows.Forms.Form
    {
      
        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HIDAntForm));
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.radModeSimple = new System.Windows.Forms.RadioButtonTS();
            this.radModeExpert = new System.Windows.Forms.RadioButtonTS();
            this.chkLock = new System.Windows.Forms.CheckBoxTS();
            this.comboRXAnt = new System.Windows.Forms.ComboBoxTS();
            this.comboTXAnt = new System.Windows.Forms.ComboBoxTS();
            this.udTX1Delay = new System.Windows.Forms.NumericUpDownTS();
            this.chkTX1DelayEnable = new System.Windows.Forms.CheckBoxTS();
            this.chkPTTOutEnable = new System.Windows.Forms.CheckBoxTS();
            this.txtStatus = new System.Windows.Forms.TextBoxTS();
            this.grpMode = new System.Windows.Forms.GroupBoxTS();
            this.lblBand = new System.Windows.Forms.LabelTS();
            this.comboBand = new System.Windows.Forms.ComboBoxTS();
            this.grpAntenna = new System.Windows.Forms.GroupBoxTS();
            this.lblTX = new System.Windows.Forms.LabelTS();
            this.lblRX1 = new System.Windows.Forms.LabelTS();
            this.grpFlexWirePTTOut = new System.Windows.Forms.GroupBoxTS();
            this.textBoxTS1 = new System.Windows.Forms.TextBoxTS();
            ((System.ComponentModel.ISupportInitialize)(this.udTX1Delay)).BeginInit();
            this.grpMode.SuspendLayout();
            this.grpAntenna.SuspendLayout();
            this.grpFlexWirePTTOut.SuspendLayout();
            this.SuspendLayout();
            // 
            // radModeSimple
            // 
            this.radModeSimple.Checked = true;
            this.radModeSimple.Image = null;
            this.radModeSimple.Location = new System.Drawing.Point(16, 24);
            this.radModeSimple.Name = "radModeSimple";
            this.radModeSimple.Size = new System.Drawing.Size(64, 24);
            this.radModeSimple.TabIndex = 16;
            this.radModeSimple.TabStop = true;
            this.radModeSimple.Text = "Simple";
            this.toolTip1.SetToolTip(this.radModeSimple, "One setting for all bands");
            this.radModeSimple.CheckedChanged += new System.EventHandler(this.radModeSimple_CheckedChanged);
            // 
            // radModeExpert
            // 
            this.radModeExpert.Image = null;
            this.radModeExpert.Location = new System.Drawing.Point(96, 24);
            this.radModeExpert.Name = "radModeExpert";
            this.radModeExpert.Size = new System.Drawing.Size(56, 24);
            this.radModeExpert.TabIndex = 20;
            this.radModeExpert.Text = "Expert";
            this.toolTip1.SetToolTip(this.radModeExpert, "More settings for each individual band");
            this.radModeExpert.CheckedChanged += new System.EventHandler(this.radModeExpert_CheckedChanged);
            // 
            // chkLock
            // 
            this.chkLock.Checked = true;
            this.chkLock.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkLock.Image = null;
            this.chkLock.Location = new System.Drawing.Point(192, 38);
            this.chkLock.Name = "chkLock";
            this.chkLock.Size = new System.Drawing.Size(64, 24);
            this.chkLock.TabIndex = 16;
            this.chkLock.Text = "Lock";
            this.toolTip1.SetToolTip(this.chkLock, "Check this box to lock RX1 and TX antenna selections.");
            this.chkLock.CheckedChanged += new System.EventHandler(this.chkLock_CheckedChanged);
            // 
            // comboRXAnt
            // 
            this.comboRXAnt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboRXAnt.DropDownWidth = 64;
            this.comboRXAnt.Items.AddRange(new object[] {
            "PA",
            "XVTX/COM",
            "XVRX"});
            this.comboRXAnt.Location = new System.Drawing.Point(8, 40);
            this.comboRXAnt.Name = "comboRXAnt";
            this.comboRXAnt.Size = new System.Drawing.Size(80, 21);
            this.comboRXAnt.TabIndex = 10;
            this.toolTip1.SetToolTip(this.comboRXAnt, "Selects the Main Receiver Antenna");
            this.comboRXAnt.SelectedIndexChanged += new System.EventHandler(this.comboRXAnt_SelectedIndexChanged);
            // 
            // comboTXAnt
            // 
            this.comboTXAnt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboTXAnt.DropDownWidth = 64;
            this.comboTXAnt.Items.AddRange(new object[] {
            "PA",
            "XVTX/COM"});
            this.comboTXAnt.Location = new System.Drawing.Point(93, 40);
            this.comboTXAnt.Name = "comboTXAnt";
            this.comboTXAnt.Size = new System.Drawing.Size(80, 21);
            this.comboTXAnt.TabIndex = 12;
            this.toolTip1.SetToolTip(this.comboTXAnt, "Selects the Transmitter Antenna");
            this.comboTXAnt.SelectedIndexChanged += new System.EventHandler(this.comboTXAnt_SelectedIndexChanged);
            // 
            // udTX1Delay
            // 
            this.udTX1Delay.Enabled = false;
            this.udTX1Delay.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udTX1Delay.Location = new System.Drawing.Point(184, 23);
            this.udTX1Delay.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.udTX1Delay.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udTX1Delay.Name = "udTX1Delay";
            this.udTX1Delay.Size = new System.Drawing.Size(56, 20);
            this.udTX1Delay.TabIndex = 4;
            this.toolTip1.SetToolTip(this.udTX1Delay, resources.GetString("udTX1Delay.ToolTip"));
            this.udTX1Delay.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udTX1Delay.ValueChanged += new System.EventHandler(this.udTX1Delay_ValueChanged);
            // 
            // chkTX1DelayEnable
            // 
            this.chkTX1DelayEnable.Image = null;
            this.chkTX1DelayEnable.Location = new System.Drawing.Point(101, 16);
            this.chkTX1DelayEnable.Name = "chkTX1DelayEnable";
            this.chkTX1DelayEnable.Size = new System.Drawing.Size(77, 32);
            this.chkTX1DelayEnable.TabIndex = 3;
            this.chkTX1DelayEnable.Text = "Delay (ms)";
            this.toolTip1.SetToolTip(this.chkTX1DelayEnable, "When checked, Pin 3 on the FlexWire Connector will delay before switching on TR t" +
        "ransitions by the amount selected in milliseconds.");
            this.chkTX1DelayEnable.CheckedChanged += new System.EventHandler(this.chkTX1DelayEnable_CheckedChanged);
            // 
            // chkPTTOutEnable
            // 
            this.chkPTTOutEnable.Checked = true;
            this.chkPTTOutEnable.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPTTOutEnable.Image = null;
            this.chkPTTOutEnable.Location = new System.Drawing.Point(16, 16);
            this.chkPTTOutEnable.Name = "chkPTTOutEnable";
            this.chkPTTOutEnable.Size = new System.Drawing.Size(79, 32);
            this.chkPTTOutEnable.TabIndex = 0;
            this.chkPTTOutEnable.Text = "Enable";
            this.toolTip1.SetToolTip(this.chkPTTOutEnable, "When checked, Pin 3 on the FlexWire Connector will switch with TR transitions.  T" +
        "his can be used to switch an external linear, transverter, preselector, etc.");
            this.chkPTTOutEnable.CheckedChanged += new System.EventHandler(this.chkRCATX1_CheckedChanged);
            // 
            // txtStatus
            // 
            this.txtStatus.Location = new System.Drawing.Point(4, 255);
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.ReadOnly = true;
            this.txtStatus.Size = new System.Drawing.Size(264, 20);
            this.txtStatus.TabIndex = 23;
            this.txtStatus.Text = "Simple Mode: Settings are applied to all bands";
            // 
            // grpMode
            // 
            this.grpMode.Controls.Add(this.radModeSimple);
            this.grpMode.Controls.Add(this.radModeExpert);
            this.grpMode.Controls.Add(this.lblBand);
            this.grpMode.Controls.Add(this.comboBand);
            this.grpMode.Location = new System.Drawing.Point(8, 8);
            this.grpMode.Name = "grpMode";
            this.grpMode.Size = new System.Drawing.Size(264, 56);
            this.grpMode.TabIndex = 21;
            this.grpMode.TabStop = false;
            this.grpMode.Text = "Mode";
            // 
            // lblBand
            // 
            this.lblBand.Image = null;
            this.lblBand.Location = new System.Drawing.Point(160, 24);
            this.lblBand.Name = "lblBand";
            this.lblBand.Size = new System.Drawing.Size(40, 24);
            this.lblBand.TabIndex = 19;
            this.lblBand.Text = "Band:";
            this.lblBand.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblBand.Visible = false;
            // 
            // comboBand
            // 
            this.comboBand.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBand.DropDownWidth = 56;
            this.comboBand.Items.AddRange(new object[] {
            "GEN",
            "160m",
            "80m",
            "60m",
            "40m",
            "30m",
            "20m",
            "17m",
            "15m",
            "12m",
            "10m",
            "6m",
            "WWV",
            "VU 2m",
            "VU 70cm",
            "VHF2",
            "VHF3",
            "VHF4",
            "VHF5",
            "VHF6",
            "VHF7",
            "VHF8",
            "VHF9",
            "VHF10",
            "VHF11",
            "VHF12",
            "VHF13",
            "LMF",
            "120m",
            "90m",
            "61m",
            "49m",
            "41m",
            "31m",
            "25m",
            "22m",
            "19m",
            "16m",
            "14m",
            "13m",
            "11m"});
            this.comboBand.Location = new System.Drawing.Point(200, 24);
            this.comboBand.Name = "comboBand";
            this.comboBand.Size = new System.Drawing.Size(56, 21);
            this.comboBand.TabIndex = 18;
            this.comboBand.Visible = false;
            this.comboBand.SelectedIndexChanged += new System.EventHandler(this.comboBand_SelectedIndexChanged);
            // 
            // grpAntenna
            // 
            this.grpAntenna.Controls.Add(this.chkLock);
            this.grpAntenna.Controls.Add(this.comboRXAnt);
            this.grpAntenna.Controls.Add(this.comboTXAnt);
            this.grpAntenna.Controls.Add(this.lblTX);
            this.grpAntenna.Controls.Add(this.lblRX1);
            this.grpAntenna.Location = new System.Drawing.Point(8, 72);
            this.grpAntenna.Name = "grpAntenna";
            this.grpAntenna.Size = new System.Drawing.Size(264, 77);
            this.grpAntenna.TabIndex = 20;
            this.grpAntenna.TabStop = false;
            this.grpAntenna.Text = "Antenna";
            // 
            // lblTX
            // 
            this.lblTX.Image = null;
            this.lblTX.Location = new System.Drawing.Point(93, 24);
            this.lblTX.Name = "lblTX";
            this.lblTX.Size = new System.Drawing.Size(64, 16);
            this.lblTX.TabIndex = 13;
            this.lblTX.Text = "Transmit:";
            // 
            // lblRX1
            // 
            this.lblRX1.Image = null;
            this.lblRX1.Location = new System.Drawing.Point(8, 24);
            this.lblRX1.Name = "lblRX1";
            this.lblRX1.Size = new System.Drawing.Size(72, 16);
            this.lblRX1.TabIndex = 11;
            this.lblRX1.Text = "Receive:";
            // 
            // grpFlexWirePTTOut
            // 
            this.grpFlexWirePTTOut.Controls.Add(this.textBoxTS1);
            this.grpFlexWirePTTOut.Controls.Add(this.udTX1Delay);
            this.grpFlexWirePTTOut.Controls.Add(this.chkTX1DelayEnable);
            this.grpFlexWirePTTOut.Controls.Add(this.chkPTTOutEnable);
            this.grpFlexWirePTTOut.Location = new System.Drawing.Point(8, 155);
            this.grpFlexWirePTTOut.Name = "grpFlexWirePTTOut";
            this.grpFlexWirePTTOut.Size = new System.Drawing.Size(264, 94);
            this.grpFlexWirePTTOut.TabIndex = 21;
            this.grpFlexWirePTTOut.TabStop = false;
            this.grpFlexWirePTTOut.Text = "FlexWire PTT Out";
            this.grpFlexWirePTTOut.Visible = false;
            // 
            // textBoxTS1
            // 
            this.textBoxTS1.Location = new System.Drawing.Point(16, 54);
            this.textBoxTS1.Name = "textBoxTS1";
            this.textBoxTS1.ReadOnly = true;
            this.textBoxTS1.Size = new System.Drawing.Size(228, 20);
            this.textBoxTS1.TabIndex = 25;
            this.textBoxTS1.Text = "Hit F1 for Help on TR Delay";
            this.textBoxTS1.Click += new System.EventHandler(this.textBoxTS1_Click);
            // 
            // HIDAntForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(280, 287);
            this.Controls.Add(this.txtStatus);
            this.Controls.Add(this.grpMode);
            this.Controls.Add(this.grpAntenna);
            this.Controls.Add(this.grpFlexWirePTTOut);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "HIDAntForm";
            this.Text = "FLEX-1500 Antenna Selection";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.HIDAntForm_Closing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.HIDAntForm_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.udTX1Delay)).EndInit();
            this.grpMode.ResumeLayout(false);
            this.grpAntenna.ResumeLayout(false);
            this.grpFlexWirePTTOut.ResumeLayout(false);
            this.grpFlexWirePTTOut.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

       
        private System.Windows.Forms.ComboBoxTS comboRXAnt;
        private System.Windows.Forms.LabelTS lblTX;
        private System.Windows.Forms.ComboBoxTS comboTXAnt;
        private System.Windows.Forms.RadioButtonTS radModeSimple;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.LabelTS lblBand;
        private System.Windows.Forms.ComboBoxTS comboBand;
        private System.Windows.Forms.RadioButtonTS radModeExpert;
        private System.Windows.Forms.LabelTS lblRX1;
        private System.Windows.Forms.GroupBoxTS grpMode;
        private System.Windows.Forms.GroupBoxTS grpAntenna;
        private System.Windows.Forms.GroupBoxTS grpFlexWirePTTOut;
        private System.Windows.Forms.CheckBoxTS chkPTTOutEnable;
        private System.Windows.Forms.TextBoxTS txtStatus;
        private System.Windows.Forms.CheckBoxTS chkLock;
        private System.Windows.Forms.CheckBoxTS chkTX1DelayEnable;
        private System.Windows.Forms.NumericUpDownTS udTX1Delay;
        private System.Windows.Forms.TextBoxTS textBoxTS1;
        private System.ComponentModel.IContainer components;

        
    }
}
