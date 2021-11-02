//=================================================================
// FLEX5000LLHWForm.cs
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
    public partial class FLEX5000LLHWForm : System.Windows.Forms.Form
    {
       


        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FLEX5000LLHWForm));
            this.udATUL = new System.Windows.Forms.NumericUpDown();
            this.udATUC = new System.Windows.Forms.NumericUpDown();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.buttonTS3 = new System.Windows.Forms.ButtonTS();
            this.labelTS1 = new System.Windows.Forms.LabelTS();
            this.buttonTS1 = new System.Windows.Forms.ButtonTS();
            this.buttonTS2 = new System.Windows.Forms.ButtonTS();
            this.btnEEPROMRead1 = new System.Windows.Forms.ButtonTS();
            this.lblATUC = new System.Windows.Forms.LabelTS();
            this.lblATUL = new System.Windows.Forms.LabelTS();
            this.grpATURelays = new System.Windows.Forms.GroupBoxTS();
            this.chkATUATTN = new System.Windows.Forms.CheckBoxTS();
            this.chkATUEnable = new System.Windows.Forms.CheckBoxTS();
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
            this.grpFlexWire = new System.Windows.Forms.GroupBoxTS();
            this.btnFlexWireRead2Val = new System.Windows.Forms.ButtonTS();
            this.btnFlexWireReadVal = new System.Windows.Forms.ButtonTS();
            this.lblFlexWireVal2 = new System.Windows.Forms.LabelTS();
            this.lblFlexWireVal1 = new System.Windows.Forms.LabelTS();
            this.lblFlexWireAddr = new System.Windows.Forms.LabelTS();
            this.btnFlexWireWriteVal = new System.Windows.Forms.ButtonTS();
            this.txtFlexWireVal2 = new System.Windows.Forms.TextBox();
            this.txtFlexWireVal1 = new System.Windows.Forms.TextBox();
            this.txtFlexWireAddr = new System.Windows.Forms.TextBox();
            this.btnFlexWireWrite2Val = new System.Windows.Forms.ButtonTS();
            this.grpATU = new System.Windows.Forms.GroupBoxTS();
            this.btnATUFull = new System.Windows.Forms.ButtonTS();
            this.txtATU3 = new System.Windows.Forms.TextBox();
            this.txtATU2 = new System.Windows.Forms.TextBox();
            this.txtATU1 = new System.Windows.Forms.TextBox();
            this.btnATUSendCmd = new System.Windows.Forms.ButtonTS();
            this.grpPAPot = new System.Windows.Forms.GroupBoxTS();
            this.txtPAPotRead = new System.Windows.Forms.TextBox();
            this.txtPAPotWrite = new System.Windows.Forms.TextBox();
            this.btnPAPotWrite = new System.Windows.Forms.ButtonTS();
            this.btnPAPotRead = new System.Windows.Forms.ButtonTS();
            this.comboPAPotIndex = new System.Windows.Forms.ComboBoxTS();
            this.lblPAPotIndex = new System.Windows.Forms.LabelTS();
            this.grpGPIODDR = new System.Windows.Forms.GroupBoxTS();
            this.label9 = new System.Windows.Forms.LabelTS();
            this.label10 = new System.Windows.Forms.LabelTS();
            this.label11 = new System.Windows.Forms.LabelTS();
            this.label12 = new System.Windows.Forms.LabelTS();
            this.label13 = new System.Windows.Forms.LabelTS();
            this.label14 = new System.Windows.Forms.LabelTS();
            this.label15 = new System.Windows.Forms.LabelTS();
            this.label16 = new System.Windows.Forms.LabelTS();
            this.txtGPIODDRRead = new System.Windows.Forms.TextBox();
            this.txtGPIODDRWrite = new System.Windows.Forms.TextBox();
            this.btnGPIODDRWrite = new System.Windows.Forms.ButtonTS();
            this.btnGPIODDRRead = new System.Windows.Forms.ButtonTS();
            this.chkGPIODDR4 = new System.Windows.Forms.CheckBoxTS();
            this.chkGPIODDR7 = new System.Windows.Forms.CheckBoxTS();
            this.chkGPIODDR2 = new System.Windows.Forms.CheckBoxTS();
            this.chkGPIODDR1 = new System.Windows.Forms.CheckBoxTS();
            this.chkGPIODDR6 = new System.Windows.Forms.CheckBoxTS();
            this.chkGPIODDR5 = new System.Windows.Forms.CheckBoxTS();
            this.chkGPIODDR3 = new System.Windows.Forms.CheckBoxTS();
            this.chkGPIODDR8 = new System.Windows.Forms.CheckBoxTS();
            this.grpGPIO = new System.Windows.Forms.GroupBoxTS();
            this.label8 = new System.Windows.Forms.LabelTS();
            this.label7 = new System.Windows.Forms.LabelTS();
            this.label6 = new System.Windows.Forms.LabelTS();
            this.label1 = new System.Windows.Forms.LabelTS();
            this.txtGPIORead = new System.Windows.Forms.TextBox();
            this.txtGPIOWrite = new System.Windows.Forms.TextBox();
            this.btnGPIOWriteVal = new System.Windows.Forms.ButtonTS();
            this.btnGPIORead = new System.Windows.Forms.ButtonTS();
            this.label5 = new System.Windows.Forms.LabelTS();
            this.label4 = new System.Windows.Forms.LabelTS();
            this.label3 = new System.Windows.Forms.LabelTS();
            this.label2 = new System.Windows.Forms.LabelTS();
            this.chkGPIO4 = new System.Windows.Forms.CheckBoxTS();
            this.chkGPIO7 = new System.Windows.Forms.CheckBoxTS();
            this.chkGPIO2 = new System.Windows.Forms.CheckBoxTS();
            this.chkGPIO1 = new System.Windows.Forms.CheckBoxTS();
            this.chkGPIO6 = new System.Windows.Forms.CheckBoxTS();
            this.chkGPIO5 = new System.Windows.Forms.CheckBoxTS();
            this.chkGPIO3 = new System.Windows.Forms.CheckBoxTS();
            this.chkGPIO8 = new System.Windows.Forms.CheckBoxTS();
            this.grpCodec = new System.Windows.Forms.GroupBoxTS();
            this.comboCodecReg = new System.Windows.Forms.ComboBoxTS();
            this.txtCodecRead = new System.Windows.Forms.TextBox();
            this.txtCodecWrite = new System.Windows.Forms.TextBox();
            this.lblCodecRegister = new System.Windows.Forms.LabelTS();
            this.btnCodecWrite = new System.Windows.Forms.ButtonTS();
            this.btnCodecRead = new System.Windows.Forms.ButtonTS();
            this.grpTRXPot = new System.Windows.Forms.GroupBoxTS();
            this.txtTRXPotRead = new System.Windows.Forms.TextBox();
            this.txtTRXPotWrite = new System.Windows.Forms.TextBox();
            this.btnTRXPotWrite = new System.Windows.Forms.ButtonTS();
            this.btnTRXPotRead = new System.Windows.Forms.ButtonTS();
            this.comboTRXPotIndex = new System.Windows.Forms.ComboBoxTS();
            this.lblTRXPotIndex = new System.Windows.Forms.LabelTS();
            this.grpEEPROM = new System.Windows.Forms.GroupBoxTS();
            this.chkRX2EEPROM = new System.Windows.Forms.CheckBoxTS();
            this.txtEEPROMReadFloat = new System.Windows.Forms.TextBox();
            this.txtEEPROMWriteFloat = new System.Windows.Forms.TextBox();
            this.btnEEPROMWriteFloat = new System.Windows.Forms.ButtonTS();
            this.btnEEPROMReadFloat = new System.Windows.Forms.ButtonTS();
            this.txtEEPROMOffset = new System.Windows.Forms.TextBox();
            this.txtEEPROMRead = new System.Windows.Forms.TextBox();
            this.txtEEPROMWrite = new System.Windows.Forms.TextBox();
            this.lblEEPROMOffset = new System.Windows.Forms.LabelTS();
            this.btnEEPROMWrite = new System.Windows.Forms.ButtonTS();
            this.btnEEPROMRead = new System.Windows.Forms.ButtonTS();
            this.grpPIO = new System.Windows.Forms.GroupBoxTS();
            this.comboPIOReg = new System.Windows.Forms.ComboBoxTS();
            this.txtPIORead = new System.Windows.Forms.TextBox();
            this.txtPIOWrite = new System.Windows.Forms.TextBox();
            this.lblPIORegister = new System.Windows.Forms.LabelTS();
            this.btnPIOWrite = new System.Windows.Forms.ButtonTS();
            this.btnPIORead = new System.Windows.Forms.ButtonTS();
            this.lblPIOChip = new System.Windows.Forms.LabelTS();
            this.comboPIOChip = new System.Windows.Forms.ComboBoxTS();
            this.grpClockGen = new System.Windows.Forms.GroupBoxTS();
            this.chkClockGenCS = new System.Windows.Forms.CheckBoxTS();
            this.chkClockGenReset = new System.Windows.Forms.CheckBoxTS();
            this.comboClockGenReg = new System.Windows.Forms.ComboBoxTS();
            this.txtClockGenReadVal = new System.Windows.Forms.TextBox();
            this.txtClockGenWriteVal = new System.Windows.Forms.TextBox();
            this.lblClockGenReg = new System.Windows.Forms.LabelTS();
            this.btnClockGenWrite = new System.Windows.Forms.ButtonTS();
            this.btnClockGenRead = new System.Windows.Forms.ButtonTS();
            this.grpDDS = new System.Windows.Forms.GroupBoxTS();
            this.btnIOUpdate = new System.Windows.Forms.ButtonTS();
            this.chkManualIOUpdate = new System.Windows.Forms.CheckBoxTS();
            this.chkRX2DDS = new System.Windows.Forms.CheckBoxTS();
            this.chkDDSCS = new System.Windows.Forms.CheckBoxTS();
            this.chkDDSReset = new System.Windows.Forms.CheckBoxTS();
            this.comboDDSChan = new System.Windows.Forms.ComboBoxTS();
            this.lblDDSChan = new System.Windows.Forms.LabelTS();
            this.comboDDSReg = new System.Windows.Forms.ComboBoxTS();
            this.txtDDSWrite = new System.Windows.Forms.TextBox();
            this.lblDDSReg = new System.Windows.Forms.LabelTS();
            this.btnDDSWrite = new System.Windows.Forms.ButtonTS();
            this.txtDDSReadVal = new System.Windows.Forms.TextBox();
            this.btnDDSRead = new System.Windows.Forms.ButtonTS();
            this.comboMuxChan = new System.Windows.Forms.ComboBoxTS();
            this.lblMuxChannel = new System.Windows.Forms.LabelTS();
            ((System.ComponentModel.ISupportInitialize)(this.udATUL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udATUC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.grpATURelays.SuspendLayout();
            this.grpFlexWire.SuspendLayout();
            this.grpATU.SuspendLayout();
            this.grpPAPot.SuspendLayout();
            this.grpGPIODDR.SuspendLayout();
            this.grpGPIO.SuspendLayout();
            this.grpCodec.SuspendLayout();
            this.grpTRXPot.SuspendLayout();
            this.grpEEPROM.SuspendLayout();
            this.grpPIO.SuspendLayout();
            this.grpClockGen.SuspendLayout();
            this.grpDDS.SuspendLayout();
            this.SuspendLayout();
            // 
            // udATUL
            // 
            this.udATUL.Location = new System.Drawing.Point(576, 408);
            this.udATUL.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.udATUL.Name = "udATUL";
            this.udATUL.Size = new System.Drawing.Size(48, 20);
            this.udATUL.TabIndex = 39;
            this.udATUL.ValueChanged += new System.EventHandler(this.udATUL_ValueChanged);
            // 
            // udATUC
            // 
            this.udATUC.Location = new System.Drawing.Point(576, 432);
            this.udATUC.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.udATUC.Name = "udATUC";
            this.udATUC.Size = new System.Drawing.Size(48, 20);
            this.udATUC.TabIndex = 41;
            this.udATUC.ValueChanged += new System.EventHandler(this.udATUC_ValueChanged);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(272, 508);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(48, 20);
            this.numericUpDown1.TabIndex = 47;
            this.toolTip1.SetToolTip(this.numericUpDown1, resources.GetString("numericUpDown1.ToolTip"));
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // buttonTS3
            // 
            this.buttonTS3.Image = null;
            this.buttonTS3.Location = new System.Drawing.Point(336, 507);
            this.buttonTS3.Name = "buttonTS3";
            this.buttonTS3.Size = new System.Drawing.Size(88, 23);
            this.buttonTS3.TabIndex = 48;
            this.buttonTS3.Text = "Update Turf";
            this.toolTip1.SetToolTip(this.buttonTS3, resources.GetString("buttonTS3.ToolTip"));
            this.buttonTS3.Click += new System.EventHandler(this.buttonTS3_Click);
            // 
            // labelTS1
            // 
            this.labelTS1.Image = null;
            this.labelTS1.Location = new System.Drawing.Point(157, 512);
            this.labelTS1.Name = "labelTS1";
            this.labelTS1.Size = new System.Drawing.Size(112, 16);
            this.labelTS1.TabIndex = 45;
            this.labelTS1.Text = "Select Turf Region:";
            this.toolTip1.SetToolTip(this.labelTS1, resources.GetString("labelTS1.ToolTip"));
            // 
            // buttonTS1
            // 
            this.buttonTS1.Image = null;
            this.buttonTS1.Location = new System.Drawing.Point(187, 553);
            this.buttonTS1.Name = "buttonTS1";
            this.buttonTS1.Size = new System.Drawing.Size(141, 23);
            this.buttonTS1.TabIndex = 45;
            this.buttonTS1.Text = "MARS/CAP/SHARES";
            this.toolTip1.SetToolTip(this.buttonTS1, resources.GetString("buttonTS1.ToolTip"));
            this.buttonTS1.Click += new System.EventHandler(this.buttonTS1_Click);
            // 
            // buttonTS2
            // 
            this.buttonTS2.Image = null;
            this.buttonTS2.Location = new System.Drawing.Point(358, 553);
            this.buttonTS2.Name = "buttonTS2";
            this.buttonTS2.Size = new System.Drawing.Size(58, 23);
            this.buttonTS2.TabIndex = 46;
            this.buttonTS2.Text = "Normal";
            this.buttonTS2.Click += new System.EventHandler(this.buttonTS2_Click);
            // 
            // btnEEPROMRead1
            // 
            this.btnEEPROMRead1.Image = null;
            this.btnEEPROMRead1.Location = new System.Drawing.Point(476, 595);
            this.btnEEPROMRead1.Name = "btnEEPROMRead1";
            this.btnEEPROMRead1.Size = new System.Drawing.Size(152, 23);
            this.btnEEPROMRead1.TabIndex = 45;
            this.btnEEPROMRead1.Text = "Record ALL EEPROM";
            this.btnEEPROMRead1.Click += new System.EventHandler(this.btnEEPROMRead1_Click);
            // 
            // lblATUC
            // 
            this.lblATUC.Image = null;
            this.lblATUC.Location = new System.Drawing.Point(520, 432);
            this.lblATUC.Name = "lblATUC";
            this.lblATUC.Size = new System.Drawing.Size(48, 16);
            this.lblATUC.TabIndex = 42;
            this.lblATUC.Text = "ATU - C:";
            // 
            // lblATUL
            // 
            this.lblATUL.Image = null;
            this.lblATUL.Location = new System.Drawing.Point(520, 408);
            this.lblATUL.Name = "lblATUL";
            this.lblATUL.Size = new System.Drawing.Size(48, 16);
            this.lblATUL.TabIndex = 40;
            this.lblATUL.Text = "ATU - L:";
            // 
            // grpATURelays
            // 
            this.grpATURelays.Controls.Add(this.chkATUATTN);
            this.grpATURelays.Controls.Add(this.chkATUEnable);
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
            this.grpATURelays.Location = new System.Drawing.Point(184, 128);
            this.grpATURelays.Name = "grpATURelays";
            this.grpATURelays.Size = new System.Drawing.Size(216, 112);
            this.grpATURelays.TabIndex = 38;
            this.grpATURelays.TabStop = false;
            this.grpATURelays.Text = "ATU";
            // 
            // chkATUATTN
            // 
            this.chkATUATTN.Image = null;
            this.chkATUATTN.Location = new System.Drawing.Point(72, 50);
            this.chkATUATTN.Name = "chkATUATTN";
            this.chkATUATTN.Size = new System.Drawing.Size(56, 16);
            this.chkATUATTN.TabIndex = 64;
            this.chkATUATTN.Text = "ATTN";
            this.chkATUATTN.CheckedChanged += new System.EventHandler(this.chkATUATTN_CheckedChanged);
            // 
            // chkATUEnable
            // 
            this.chkATUEnable.Image = null;
            this.chkATUEnable.Location = new System.Drawing.Point(8, 50);
            this.chkATUEnable.Name = "chkATUEnable";
            this.chkATUEnable.Size = new System.Drawing.Size(64, 16);
            this.chkATUEnable.TabIndex = 63;
            this.chkATUEnable.Text = "Enable";
            this.chkATUEnable.CheckedChanged += new System.EventHandler(this.chkATUEnable_CheckedChanged);
            // 
            // lblHiZ
            // 
            this.lblHiZ.Image = null;
            this.lblHiZ.Location = new System.Drawing.Point(16, 64);
            this.lblHiZ.Name = "lblHiZ";
            this.lblHiZ.Size = new System.Drawing.Size(24, 16);
            this.lblHiZ.TabIndex = 62;
            this.lblHiZ.Text = "HiZ";
            // 
            // chkHiZ
            // 
            this.chkHiZ.Image = null;
            this.chkHiZ.Location = new System.Drawing.Point(16, 80);
            this.chkHiZ.Name = "chkHiZ";
            this.chkHiZ.Size = new System.Drawing.Size(16, 24);
            this.chkHiZ.TabIndex = 61;
            this.chkHiZ.CheckedChanged += new System.EventHandler(this.chkHiZ_CheckedChanged);
            // 
            // lblC6
            // 
            this.lblC6.Image = null;
            this.lblC6.Location = new System.Drawing.Point(40, 64);
            this.lblC6.Name = "lblC6";
            this.lblC6.Size = new System.Drawing.Size(24, 16);
            this.lblC6.TabIndex = 60;
            this.lblC6.Text = "C6";
            // 
            // lblC5
            // 
            this.lblC5.Image = null;
            this.lblC5.Location = new System.Drawing.Point(64, 64);
            this.lblC5.Name = "lblC5";
            this.lblC5.Size = new System.Drawing.Size(24, 16);
            this.lblC5.TabIndex = 59;
            this.lblC5.Text = "C5";
            // 
            // lblC4
            // 
            this.lblC4.Image = null;
            this.lblC4.Location = new System.Drawing.Point(88, 64);
            this.lblC4.Name = "lblC4";
            this.lblC4.Size = new System.Drawing.Size(24, 16);
            this.lblC4.TabIndex = 58;
            this.lblC4.Text = "C4";
            // 
            // lblC3
            // 
            this.lblC3.Image = null;
            this.lblC3.Location = new System.Drawing.Point(112, 64);
            this.lblC3.Name = "lblC3";
            this.lblC3.Size = new System.Drawing.Size(24, 16);
            this.lblC3.TabIndex = 57;
            this.lblC3.Text = "C3";
            // 
            // lblC2
            // 
            this.lblC2.Image = null;
            this.lblC2.Location = new System.Drawing.Point(136, 64);
            this.lblC2.Name = "lblC2";
            this.lblC2.Size = new System.Drawing.Size(24, 16);
            this.lblC2.TabIndex = 56;
            this.lblC2.Text = "C2";
            // 
            // lblC1
            // 
            this.lblC1.Image = null;
            this.lblC1.Location = new System.Drawing.Point(160, 64);
            this.lblC1.Name = "lblC1";
            this.lblC1.Size = new System.Drawing.Size(24, 16);
            this.lblC1.TabIndex = 55;
            this.lblC1.Text = "C1";
            // 
            // lblC0
            // 
            this.lblC0.Image = null;
            this.lblC0.Location = new System.Drawing.Point(184, 64);
            this.lblC0.Name = "lblC0";
            this.lblC0.Size = new System.Drawing.Size(24, 16);
            this.lblC0.TabIndex = 54;
            this.lblC0.Text = "C0";
            // 
            // chkC3
            // 
            this.chkC3.Image = null;
            this.chkC3.Location = new System.Drawing.Point(112, 80);
            this.chkC3.Name = "chkC3";
            this.chkC3.Size = new System.Drawing.Size(16, 24);
            this.chkC3.TabIndex = 52;
            this.chkC3.CheckedChanged += new System.EventHandler(this.chkC_CheckedChanged);
            // 
            // chkC6
            // 
            this.chkC6.Image = null;
            this.chkC6.Location = new System.Drawing.Point(40, 80);
            this.chkC6.Name = "chkC6";
            this.chkC6.Size = new System.Drawing.Size(16, 24);
            this.chkC6.TabIndex = 47;
            this.chkC6.CheckedChanged += new System.EventHandler(this.chkC_CheckedChanged);
            // 
            // chkC1
            // 
            this.chkC1.Image = null;
            this.chkC1.Location = new System.Drawing.Point(160, 80);
            this.chkC1.Name = "chkC1";
            this.chkC1.Size = new System.Drawing.Size(16, 24);
            this.chkC1.TabIndex = 51;
            this.chkC1.CheckedChanged += new System.EventHandler(this.chkC_CheckedChanged);
            // 
            // chkC0
            // 
            this.chkC0.Image = null;
            this.chkC0.Location = new System.Drawing.Point(184, 80);
            this.chkC0.Name = "chkC0";
            this.chkC0.Size = new System.Drawing.Size(16, 24);
            this.chkC0.TabIndex = 50;
            this.chkC0.CheckedChanged += new System.EventHandler(this.chkC_CheckedChanged);
            // 
            // chkC5
            // 
            this.chkC5.Image = null;
            this.chkC5.Location = new System.Drawing.Point(64, 80);
            this.chkC5.Name = "chkC5";
            this.chkC5.Size = new System.Drawing.Size(16, 24);
            this.chkC5.TabIndex = 49;
            this.chkC5.CheckedChanged += new System.EventHandler(this.chkC_CheckedChanged);
            // 
            // chkC4
            // 
            this.chkC4.Image = null;
            this.chkC4.Location = new System.Drawing.Point(88, 80);
            this.chkC4.Name = "chkC4";
            this.chkC4.Size = new System.Drawing.Size(16, 24);
            this.chkC4.TabIndex = 48;
            this.chkC4.CheckedChanged += new System.EventHandler(this.chkC_CheckedChanged);
            // 
            // chkC2
            // 
            this.chkC2.Image = null;
            this.chkC2.Location = new System.Drawing.Point(136, 80);
            this.chkC2.Name = "chkC2";
            this.chkC2.Size = new System.Drawing.Size(16, 24);
            this.chkC2.TabIndex = 53;
            this.chkC2.CheckedChanged += new System.EventHandler(this.chkC_CheckedChanged);
            // 
            // lblL8
            // 
            this.lblL8.Image = null;
            this.lblL8.Location = new System.Drawing.Point(160, 16);
            this.lblL8.Name = "lblL8";
            this.lblL8.Size = new System.Drawing.Size(24, 16);
            this.lblL8.TabIndex = 44;
            this.lblL8.Text = "L8";
            // 
            // lblL7
            // 
            this.lblL7.Image = null;
            this.lblL7.Location = new System.Drawing.Point(136, 16);
            this.lblL7.Name = "lblL7";
            this.lblL7.Size = new System.Drawing.Size(24, 16);
            this.lblL7.TabIndex = 43;
            this.lblL7.Text = "L7";
            // 
            // lblL6
            // 
            this.lblL6.Image = null;
            this.lblL6.Location = new System.Drawing.Point(112, 16);
            this.lblL6.Name = "lblL6";
            this.lblL6.Size = new System.Drawing.Size(24, 16);
            this.lblL6.TabIndex = 42;
            this.lblL6.Text = "L6";
            // 
            // lblL2
            // 
            this.lblL2.Image = null;
            this.lblL2.Location = new System.Drawing.Point(16, 16);
            this.lblL2.Name = "lblL2";
            this.lblL2.Size = new System.Drawing.Size(24, 16);
            this.lblL2.TabIndex = 30;
            this.lblL2.Text = "L2";
            // 
            // chkL8
            // 
            this.chkL8.Image = null;
            this.chkL8.Location = new System.Drawing.Point(160, 32);
            this.chkL8.Name = "chkL8";
            this.chkL8.Size = new System.Drawing.Size(16, 24);
            this.chkL8.TabIndex = 23;
            this.chkL8.CheckedChanged += new System.EventHandler(this.chkL_CheckedChanged);
            // 
            // chkL2
            // 
            this.chkL2.Image = null;
            this.chkL2.Location = new System.Drawing.Point(16, 32);
            this.chkL2.Name = "chkL2";
            this.chkL2.Size = new System.Drawing.Size(16, 24);
            this.chkL2.TabIndex = 26;
            this.chkL2.CheckedChanged += new System.EventHandler(this.chkL_CheckedChanged);
            // 
            // chkL7
            // 
            this.chkL7.Image = null;
            this.chkL7.Location = new System.Drawing.Point(136, 32);
            this.chkL7.Name = "chkL7";
            this.chkL7.Size = new System.Drawing.Size(16, 24);
            this.chkL7.TabIndex = 25;
            this.chkL7.CheckedChanged += new System.EventHandler(this.chkL_CheckedChanged);
            // 
            // chkL6
            // 
            this.chkL6.Image = null;
            this.chkL6.Location = new System.Drawing.Point(112, 32);
            this.chkL6.Name = "chkL6";
            this.chkL6.Size = new System.Drawing.Size(16, 24);
            this.chkL6.TabIndex = 24;
            this.chkL6.CheckedChanged += new System.EventHandler(this.chkL_CheckedChanged);
            // 
            // lblL9
            // 
            this.lblL9.Image = null;
            this.lblL9.Location = new System.Drawing.Point(184, 16);
            this.lblL9.Name = "lblL9";
            this.lblL9.Size = new System.Drawing.Size(24, 16);
            this.lblL9.TabIndex = 46;
            this.lblL9.Text = "L9";
            // 
            // chkL9
            // 
            this.chkL9.Image = null;
            this.chkL9.Location = new System.Drawing.Point(184, 32);
            this.chkL9.Name = "chkL9";
            this.chkL9.Size = new System.Drawing.Size(16, 24);
            this.chkL9.TabIndex = 45;
            this.chkL9.CheckedChanged += new System.EventHandler(this.chkL_CheckedChanged);
            // 
            // chkL3
            // 
            this.chkL3.Image = null;
            this.chkL3.Location = new System.Drawing.Point(40, 32);
            this.chkL3.Name = "chkL3";
            this.chkL3.Size = new System.Drawing.Size(16, 24);
            this.chkL3.TabIndex = 27;
            this.chkL3.CheckedChanged += new System.EventHandler(this.chkL_CheckedChanged);
            // 
            // lblL3
            // 
            this.lblL3.Image = null;
            this.lblL3.Location = new System.Drawing.Point(40, 16);
            this.lblL3.Name = "lblL3";
            this.lblL3.Size = new System.Drawing.Size(24, 16);
            this.lblL3.TabIndex = 31;
            this.lblL3.Text = "L3";
            // 
            // lblL4
            // 
            this.lblL4.Image = null;
            this.lblL4.Location = new System.Drawing.Point(64, 16);
            this.lblL4.Name = "lblL4";
            this.lblL4.Size = new System.Drawing.Size(24, 16);
            this.lblL4.TabIndex = 32;
            this.lblL4.Text = "L4";
            // 
            // chkL4
            // 
            this.chkL4.Image = null;
            this.chkL4.Location = new System.Drawing.Point(64, 32);
            this.chkL4.Name = "chkL4";
            this.chkL4.Size = new System.Drawing.Size(16, 24);
            this.chkL4.TabIndex = 29;
            this.chkL4.CheckedChanged += new System.EventHandler(this.chkL_CheckedChanged);
            // 
            // lblL5
            // 
            this.lblL5.Image = null;
            this.lblL5.Location = new System.Drawing.Point(88, 16);
            this.lblL5.Name = "lblL5";
            this.lblL5.Size = new System.Drawing.Size(24, 16);
            this.lblL5.TabIndex = 33;
            this.lblL5.Text = "L5";
            // 
            // chkL5
            // 
            this.chkL5.Image = null;
            this.chkL5.Location = new System.Drawing.Point(88, 32);
            this.chkL5.Name = "chkL5";
            this.chkL5.Size = new System.Drawing.Size(16, 24);
            this.chkL5.TabIndex = 28;
            this.chkL5.CheckedChanged += new System.EventHandler(this.chkL_CheckedChanged);
            // 
            // grpFlexWire
            // 
            this.grpFlexWire.Controls.Add(this.btnFlexWireRead2Val);
            this.grpFlexWire.Controls.Add(this.btnFlexWireReadVal);
            this.grpFlexWire.Controls.Add(this.lblFlexWireVal2);
            this.grpFlexWire.Controls.Add(this.lblFlexWireVal1);
            this.grpFlexWire.Controls.Add(this.lblFlexWireAddr);
            this.grpFlexWire.Controls.Add(this.btnFlexWireWriteVal);
            this.grpFlexWire.Controls.Add(this.txtFlexWireVal2);
            this.grpFlexWire.Controls.Add(this.txtFlexWireVal1);
            this.grpFlexWire.Controls.Add(this.txtFlexWireAddr);
            this.grpFlexWire.Controls.Add(this.btnFlexWireWrite2Val);
            this.grpFlexWire.Location = new System.Drawing.Point(336, 368);
            this.grpFlexWire.Name = "grpFlexWire";
            this.grpFlexWire.Size = new System.Drawing.Size(168, 120);
            this.grpFlexWire.TabIndex = 37;
            this.grpFlexWire.TabStop = false;
            this.grpFlexWire.Text = "FlexWire";
            // 
            // btnFlexWireRead2Val
            // 
            this.btnFlexWireRead2Val.Image = null;
            this.btnFlexWireRead2Val.Location = new System.Drawing.Point(128, 72);
            this.btnFlexWireRead2Val.Name = "btnFlexWireRead2Val";
            this.btnFlexWireRead2Val.Size = new System.Drawing.Size(32, 23);
            this.btnFlexWireRead2Val.TabIndex = 50;
            this.btnFlexWireRead2Val.Text = "R2";
            this.btnFlexWireRead2Val.Click += new System.EventHandler(this.btnFlexWireRead2Val_Click);
            // 
            // btnFlexWireReadVal
            // 
            this.btnFlexWireReadVal.Image = null;
            this.btnFlexWireReadVal.Location = new System.Drawing.Point(88, 72);
            this.btnFlexWireReadVal.Name = "btnFlexWireReadVal";
            this.btnFlexWireReadVal.Size = new System.Drawing.Size(32, 23);
            this.btnFlexWireReadVal.TabIndex = 49;
            this.btnFlexWireReadVal.Text = "R1";
            this.btnFlexWireReadVal.Click += new System.EventHandler(this.btnFlexWireReadVal_Click);
            // 
            // lblFlexWireVal2
            // 
            this.lblFlexWireVal2.Image = null;
            this.lblFlexWireVal2.Location = new System.Drawing.Point(96, 24);
            this.lblFlexWireVal2.Name = "lblFlexWireVal2";
            this.lblFlexWireVal2.Size = new System.Drawing.Size(32, 16);
            this.lblFlexWireVal2.TabIndex = 48;
            this.lblFlexWireVal2.Text = "Val2:";
            // 
            // lblFlexWireVal1
            // 
            this.lblFlexWireVal1.Image = null;
            this.lblFlexWireVal1.Location = new System.Drawing.Point(56, 24);
            this.lblFlexWireVal1.Name = "lblFlexWireVal1";
            this.lblFlexWireVal1.Size = new System.Drawing.Size(32, 16);
            this.lblFlexWireVal1.TabIndex = 47;
            this.lblFlexWireVal1.Text = "Val1:";
            // 
            // lblFlexWireAddr
            // 
            this.lblFlexWireAddr.Image = null;
            this.lblFlexWireAddr.Location = new System.Drawing.Point(16, 24);
            this.lblFlexWireAddr.Name = "lblFlexWireAddr";
            this.lblFlexWireAddr.Size = new System.Drawing.Size(32, 16);
            this.lblFlexWireAddr.TabIndex = 46;
            this.lblFlexWireAddr.Text = "Addr:";
            // 
            // btnFlexWireWriteVal
            // 
            this.btnFlexWireWriteVal.Image = null;
            this.btnFlexWireWriteVal.Location = new System.Drawing.Point(8, 72);
            this.btnFlexWireWriteVal.Name = "btnFlexWireWriteVal";
            this.btnFlexWireWriteVal.Size = new System.Drawing.Size(32, 23);
            this.btnFlexWireWriteVal.TabIndex = 45;
            this.btnFlexWireWriteVal.Text = "W1";
            this.btnFlexWireWriteVal.Click += new System.EventHandler(this.btnFlexWireWriteVal_Click);
            // 
            // txtFlexWireVal2
            // 
            this.txtFlexWireVal2.Location = new System.Drawing.Point(96, 40);
            this.txtFlexWireVal2.Name = "txtFlexWireVal2";
            this.txtFlexWireVal2.Size = new System.Drawing.Size(32, 20);
            this.txtFlexWireVal2.TabIndex = 44;
            this.txtFlexWireVal2.Text = "0";
            // 
            // txtFlexWireVal1
            // 
            this.txtFlexWireVal1.Location = new System.Drawing.Point(56, 40);
            this.txtFlexWireVal1.Name = "txtFlexWireVal1";
            this.txtFlexWireVal1.Size = new System.Drawing.Size(32, 20);
            this.txtFlexWireVal1.TabIndex = 43;
            this.txtFlexWireVal1.Text = "0";
            // 
            // txtFlexWireAddr
            // 
            this.txtFlexWireAddr.Location = new System.Drawing.Point(16, 40);
            this.txtFlexWireAddr.Name = "txtFlexWireAddr";
            this.txtFlexWireAddr.Size = new System.Drawing.Size(32, 20);
            this.txtFlexWireAddr.TabIndex = 41;
            this.txtFlexWireAddr.Text = "0";
            // 
            // btnFlexWireWrite2Val
            // 
            this.btnFlexWireWrite2Val.Image = null;
            this.btnFlexWireWrite2Val.Location = new System.Drawing.Point(48, 72);
            this.btnFlexWireWrite2Val.Name = "btnFlexWireWrite2Val";
            this.btnFlexWireWrite2Val.Size = new System.Drawing.Size(32, 23);
            this.btnFlexWireWrite2Val.TabIndex = 42;
            this.btnFlexWireWrite2Val.Text = "W2";
            this.btnFlexWireWrite2Val.Click += new System.EventHandler(this.btnFlexWireWrite2Val_Click);
            // 
            // grpATU
            // 
            this.grpATU.Controls.Add(this.btnATUFull);
            this.grpATU.Controls.Add(this.txtATU3);
            this.grpATU.Controls.Add(this.txtATU2);
            this.grpATU.Controls.Add(this.txtATU1);
            this.grpATU.Controls.Add(this.btnATUSendCmd);
            this.grpATU.Location = new System.Drawing.Point(184, 368);
            this.grpATU.Name = "grpATU";
            this.grpATU.Size = new System.Drawing.Size(144, 120);
            this.grpATU.TabIndex = 36;
            this.grpATU.TabStop = false;
            this.grpATU.Text = "ATU";
            // 
            // btnATUFull
            // 
            this.btnATUFull.Image = null;
            this.btnATUFull.Location = new System.Drawing.Point(16, 88);
            this.btnATUFull.Name = "btnATUFull";
            this.btnATUFull.Size = new System.Drawing.Size(40, 23);
            this.btnATUFull.TabIndex = 45;
            this.btnATUFull.Text = "Full";
            this.btnATUFull.Click += new System.EventHandler(this.btnATUFull_Click);
            // 
            // txtATU3
            // 
            this.txtATU3.Location = new System.Drawing.Point(96, 24);
            this.txtATU3.Name = "txtATU3";
            this.txtATU3.Size = new System.Drawing.Size(32, 20);
            this.txtATU3.TabIndex = 44;
            this.txtATU3.Text = "0";
            // 
            // txtATU2
            // 
            this.txtATU2.Location = new System.Drawing.Point(56, 24);
            this.txtATU2.Name = "txtATU2";
            this.txtATU2.Size = new System.Drawing.Size(32, 20);
            this.txtATU2.TabIndex = 43;
            this.txtATU2.Text = "0";
            // 
            // txtATU1
            // 
            this.txtATU1.Location = new System.Drawing.Point(16, 24);
            this.txtATU1.Name = "txtATU1";
            this.txtATU1.Size = new System.Drawing.Size(32, 20);
            this.txtATU1.TabIndex = 41;
            this.txtATU1.Text = "0";
            // 
            // btnATUSendCmd
            // 
            this.btnATUSendCmd.Image = null;
            this.btnATUSendCmd.Location = new System.Drawing.Point(16, 56);
            this.btnATUSendCmd.Name = "btnATUSendCmd";
            this.btnATUSendCmd.Size = new System.Drawing.Size(112, 23);
            this.btnATUSendCmd.TabIndex = 42;
            this.btnATUSendCmd.Text = "Send Command";
            this.btnATUSendCmd.Click += new System.EventHandler(this.btnATUSendCmd_Click);
            // 
            // grpPAPot
            // 
            this.grpPAPot.Controls.Add(this.txtPAPotRead);
            this.grpPAPot.Controls.Add(this.txtPAPotWrite);
            this.grpPAPot.Controls.Add(this.btnPAPotWrite);
            this.grpPAPot.Controls.Add(this.btnPAPotRead);
            this.grpPAPot.Controls.Add(this.comboPAPotIndex);
            this.grpPAPot.Controls.Add(this.lblPAPotIndex);
            this.grpPAPot.Location = new System.Drawing.Point(8, 400);
            this.grpPAPot.Name = "grpPAPot";
            this.grpPAPot.Size = new System.Drawing.Size(168, 88);
            this.grpPAPot.TabIndex = 35;
            this.grpPAPot.TabStop = false;
            this.grpPAPot.Text = "PA Pot";
            // 
            // txtPAPotRead
            // 
            this.txtPAPotRead.Location = new System.Drawing.Point(64, 56);
            this.txtPAPotRead.Name = "txtPAPotRead";
            this.txtPAPotRead.ReadOnly = true;
            this.txtPAPotRead.Size = new System.Drawing.Size(40, 20);
            this.txtPAPotRead.TabIndex = 27;
            this.txtPAPotRead.Text = "0";
            // 
            // txtPAPotWrite
            // 
            this.txtPAPotWrite.Location = new System.Drawing.Point(112, 24);
            this.txtPAPotWrite.Name = "txtPAPotWrite";
            this.txtPAPotWrite.Size = new System.Drawing.Size(40, 20);
            this.txtPAPotWrite.TabIndex = 26;
            this.txtPAPotWrite.Text = "0";
            // 
            // btnPAPotWrite
            // 
            this.btnPAPotWrite.Image = null;
            this.btnPAPotWrite.Location = new System.Drawing.Point(112, 56);
            this.btnPAPotWrite.Name = "btnPAPotWrite";
            this.btnPAPotWrite.Size = new System.Drawing.Size(40, 23);
            this.btnPAPotWrite.TabIndex = 28;
            this.btnPAPotWrite.Text = "Write";
            this.btnPAPotWrite.Click += new System.EventHandler(this.btnPAPotWrite_Click);
            // 
            // btnPAPotRead
            // 
            this.btnPAPotRead.Image = null;
            this.btnPAPotRead.Location = new System.Drawing.Point(16, 56);
            this.btnPAPotRead.Name = "btnPAPotRead";
            this.btnPAPotRead.Size = new System.Drawing.Size(40, 23);
            this.btnPAPotRead.TabIndex = 29;
            this.btnPAPotRead.Text = "Read";
            this.btnPAPotRead.Click += new System.EventHandler(this.btnPAPotRead_Click);
            // 
            // comboPAPotIndex
            // 
            this.comboPAPotIndex.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboPAPotIndex.DropDownWidth = 40;
            this.comboPAPotIndex.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7"});
            this.comboPAPotIndex.Location = new System.Drawing.Point(56, 24);
            this.comboPAPotIndex.Name = "comboPAPotIndex";
            this.comboPAPotIndex.Size = new System.Drawing.Size(40, 21);
            this.comboPAPotIndex.TabIndex = 2;
            this.comboPAPotIndex.SelectedIndexChanged += new System.EventHandler(this.comboPAPotIndex_SelectedIndexChanged);
            // 
            // lblPAPotIndex
            // 
            this.lblPAPotIndex.Image = null;
            this.lblPAPotIndex.Location = new System.Drawing.Point(16, 24);
            this.lblPAPotIndex.Name = "lblPAPotIndex";
            this.lblPAPotIndex.Size = new System.Drawing.Size(40, 23);
            this.lblPAPotIndex.TabIndex = 3;
            this.lblPAPotIndex.Text = "Index:";
            // 
            // grpGPIODDR
            // 
            this.grpGPIODDR.Controls.Add(this.label9);
            this.grpGPIODDR.Controls.Add(this.label10);
            this.grpGPIODDR.Controls.Add(this.label11);
            this.grpGPIODDR.Controls.Add(this.label12);
            this.grpGPIODDR.Controls.Add(this.label13);
            this.grpGPIODDR.Controls.Add(this.label14);
            this.grpGPIODDR.Controls.Add(this.label15);
            this.grpGPIODDR.Controls.Add(this.label16);
            this.grpGPIODDR.Controls.Add(this.txtGPIODDRRead);
            this.grpGPIODDR.Controls.Add(this.txtGPIODDRWrite);
            this.grpGPIODDR.Controls.Add(this.btnGPIODDRWrite);
            this.grpGPIODDR.Controls.Add(this.btnGPIODDRRead);
            this.grpGPIODDR.Controls.Add(this.chkGPIODDR4);
            this.grpGPIODDR.Controls.Add(this.chkGPIODDR7);
            this.grpGPIODDR.Controls.Add(this.chkGPIODDR2);
            this.grpGPIODDR.Controls.Add(this.chkGPIODDR1);
            this.grpGPIODDR.Controls.Add(this.chkGPIODDR6);
            this.grpGPIODDR.Controls.Add(this.chkGPIODDR5);
            this.grpGPIODDR.Controls.Add(this.chkGPIODDR3);
            this.grpGPIODDR.Controls.Add(this.chkGPIODDR8);
            this.grpGPIODDR.Location = new System.Drawing.Point(408, 248);
            this.grpGPIODDR.Name = "grpGPIODDR";
            this.grpGPIODDR.Size = new System.Drawing.Size(216, 112);
            this.grpGPIODDR.TabIndex = 33;
            this.grpGPIODDR.TabStop = false;
            this.grpGPIODDR.Text = "GPIO DDR";
            // 
            // label9
            // 
            this.label9.Image = null;
            this.label9.Location = new System.Drawing.Point(16, 16);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(24, 16);
            this.label9.TabIndex = 53;
            this.label9.Text = "8";
            // 
            // label10
            // 
            this.label10.Image = null;
            this.label10.Location = new System.Drawing.Point(40, 16);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(24, 16);
            this.label10.TabIndex = 52;
            this.label10.Text = "7";
            // 
            // label11
            // 
            this.label11.Image = null;
            this.label11.Location = new System.Drawing.Point(64, 16);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(24, 16);
            this.label11.TabIndex = 51;
            this.label11.Text = "6";
            // 
            // label12
            // 
            this.label12.Image = null;
            this.label12.Location = new System.Drawing.Point(88, 16);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(24, 16);
            this.label12.TabIndex = 50;
            this.label12.Text = "5";
            // 
            // label13
            // 
            this.label13.Image = null;
            this.label13.Location = new System.Drawing.Point(112, 16);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(24, 16);
            this.label13.TabIndex = 49;
            this.label13.Text = "4";
            // 
            // label14
            // 
            this.label14.Image = null;
            this.label14.Location = new System.Drawing.Point(136, 16);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(24, 16);
            this.label14.TabIndex = 48;
            this.label14.Text = "3";
            // 
            // label15
            // 
            this.label15.Image = null;
            this.label15.Location = new System.Drawing.Point(160, 16);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(24, 16);
            this.label15.TabIndex = 47;
            this.label15.Text = "2";
            // 
            // label16
            // 
            this.label16.Image = null;
            this.label16.Location = new System.Drawing.Point(184, 16);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(24, 16);
            this.label16.TabIndex = 46;
            this.label16.Text = "1";
            // 
            // txtGPIODDRRead
            // 
            this.txtGPIODDRRead.Location = new System.Drawing.Point(64, 80);
            this.txtGPIODDRRead.Name = "txtGPIODDRRead";
            this.txtGPIODDRRead.ReadOnly = true;
            this.txtGPIODDRRead.Size = new System.Drawing.Size(40, 20);
            this.txtGPIODDRRead.TabIndex = 39;
            this.txtGPIODDRRead.Text = "0";
            // 
            // txtGPIODDRWrite
            // 
            this.txtGPIODDRWrite.Location = new System.Drawing.Point(160, 80);
            this.txtGPIODDRWrite.Name = "txtGPIODDRWrite";
            this.txtGPIODDRWrite.Size = new System.Drawing.Size(40, 20);
            this.txtGPIODDRWrite.TabIndex = 38;
            this.txtGPIODDRWrite.Text = "0";
            // 
            // btnGPIODDRWrite
            // 
            this.btnGPIODDRWrite.Image = null;
            this.btnGPIODDRWrite.Location = new System.Drawing.Point(112, 80);
            this.btnGPIODDRWrite.Name = "btnGPIODDRWrite";
            this.btnGPIODDRWrite.Size = new System.Drawing.Size(40, 23);
            this.btnGPIODDRWrite.TabIndex = 40;
            this.btnGPIODDRWrite.Text = "Write";
            this.btnGPIODDRWrite.Click += new System.EventHandler(this.btnGPIODDRWrite_Click);
            // 
            // btnGPIODDRRead
            // 
            this.btnGPIODDRRead.Image = null;
            this.btnGPIODDRRead.Location = new System.Drawing.Point(16, 80);
            this.btnGPIODDRRead.Name = "btnGPIODDRRead";
            this.btnGPIODDRRead.Size = new System.Drawing.Size(40, 23);
            this.btnGPIODDRRead.TabIndex = 41;
            this.btnGPIODDRRead.Text = "Read";
            this.btnGPIODDRRead.Click += new System.EventHandler(this.btnGPIODDRRead_Click);
            // 
            // chkGPIODDR4
            // 
            this.chkGPIODDR4.Image = null;
            this.chkGPIODDR4.Location = new System.Drawing.Point(112, 32);
            this.chkGPIODDR4.Name = "chkGPIODDR4";
            this.chkGPIODDR4.Size = new System.Drawing.Size(16, 24);
            this.chkGPIODDR4.TabIndex = 28;
            this.chkGPIODDR4.CheckedChanged += new System.EventHandler(this.chkGPIODDR_CheckedChanged);
            // 
            // chkGPIODDR7
            // 
            this.chkGPIODDR7.Image = null;
            this.chkGPIODDR7.Location = new System.Drawing.Point(40, 32);
            this.chkGPIODDR7.Name = "chkGPIODDR7";
            this.chkGPIODDR7.Size = new System.Drawing.Size(16, 24);
            this.chkGPIODDR7.TabIndex = 23;
            this.chkGPIODDR7.CheckedChanged += new System.EventHandler(this.chkGPIODDR_CheckedChanged);
            // 
            // chkGPIODDR2
            // 
            this.chkGPIODDR2.Image = null;
            this.chkGPIODDR2.Location = new System.Drawing.Point(160, 32);
            this.chkGPIODDR2.Name = "chkGPIODDR2";
            this.chkGPIODDR2.Size = new System.Drawing.Size(16, 24);
            this.chkGPIODDR2.TabIndex = 27;
            this.chkGPIODDR2.CheckedChanged += new System.EventHandler(this.chkGPIODDR_CheckedChanged);
            // 
            // chkGPIODDR1
            // 
            this.chkGPIODDR1.Image = null;
            this.chkGPIODDR1.Location = new System.Drawing.Point(184, 32);
            this.chkGPIODDR1.Name = "chkGPIODDR1";
            this.chkGPIODDR1.Size = new System.Drawing.Size(16, 24);
            this.chkGPIODDR1.TabIndex = 26;
            this.chkGPIODDR1.CheckedChanged += new System.EventHandler(this.chkGPIODDR_CheckedChanged);
            // 
            // chkGPIODDR6
            // 
            this.chkGPIODDR6.Image = null;
            this.chkGPIODDR6.Location = new System.Drawing.Point(64, 32);
            this.chkGPIODDR6.Name = "chkGPIODDR6";
            this.chkGPIODDR6.Size = new System.Drawing.Size(16, 24);
            this.chkGPIODDR6.TabIndex = 25;
            this.chkGPIODDR6.CheckedChanged += new System.EventHandler(this.chkGPIODDR_CheckedChanged);
            // 
            // chkGPIODDR5
            // 
            this.chkGPIODDR5.Image = null;
            this.chkGPIODDR5.Location = new System.Drawing.Point(88, 32);
            this.chkGPIODDR5.Name = "chkGPIODDR5";
            this.chkGPIODDR5.Size = new System.Drawing.Size(16, 24);
            this.chkGPIODDR5.TabIndex = 24;
            this.chkGPIODDR5.CheckedChanged += new System.EventHandler(this.chkGPIODDR_CheckedChanged);
            // 
            // chkGPIODDR3
            // 
            this.chkGPIODDR3.Image = null;
            this.chkGPIODDR3.Location = new System.Drawing.Point(136, 32);
            this.chkGPIODDR3.Name = "chkGPIODDR3";
            this.chkGPIODDR3.Size = new System.Drawing.Size(16, 24);
            this.chkGPIODDR3.TabIndex = 29;
            this.chkGPIODDR3.CheckedChanged += new System.EventHandler(this.chkGPIODDR_CheckedChanged);
            // 
            // chkGPIODDR8
            // 
            this.chkGPIODDR8.Image = null;
            this.chkGPIODDR8.Location = new System.Drawing.Point(16, 32);
            this.chkGPIODDR8.Name = "chkGPIODDR8";
            this.chkGPIODDR8.Size = new System.Drawing.Size(16, 24);
            this.chkGPIODDR8.TabIndex = 22;
            this.chkGPIODDR8.CheckedChanged += new System.EventHandler(this.chkGPIODDR_CheckedChanged);
            // 
            // grpGPIO
            // 
            this.grpGPIO.Controls.Add(this.label8);
            this.grpGPIO.Controls.Add(this.label7);
            this.grpGPIO.Controls.Add(this.label6);
            this.grpGPIO.Controls.Add(this.label1);
            this.grpGPIO.Controls.Add(this.txtGPIORead);
            this.grpGPIO.Controls.Add(this.txtGPIOWrite);
            this.grpGPIO.Controls.Add(this.btnGPIOWriteVal);
            this.grpGPIO.Controls.Add(this.btnGPIORead);
            this.grpGPIO.Controls.Add(this.label5);
            this.grpGPIO.Controls.Add(this.label4);
            this.grpGPIO.Controls.Add(this.label3);
            this.grpGPIO.Controls.Add(this.label2);
            this.grpGPIO.Controls.Add(this.chkGPIO4);
            this.grpGPIO.Controls.Add(this.chkGPIO7);
            this.grpGPIO.Controls.Add(this.chkGPIO2);
            this.grpGPIO.Controls.Add(this.chkGPIO1);
            this.grpGPIO.Controls.Add(this.chkGPIO6);
            this.grpGPIO.Controls.Add(this.chkGPIO5);
            this.grpGPIO.Controls.Add(this.chkGPIO3);
            this.grpGPIO.Controls.Add(this.chkGPIO8);
            this.grpGPIO.Location = new System.Drawing.Point(184, 248);
            this.grpGPIO.Name = "grpGPIO";
            this.grpGPIO.Size = new System.Drawing.Size(216, 112);
            this.grpGPIO.TabIndex = 32;
            this.grpGPIO.TabStop = false;
            this.grpGPIO.Text = "GPIO";
            // 
            // label8
            // 
            this.label8.Image = null;
            this.label8.Location = new System.Drawing.Point(16, 16);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(24, 16);
            this.label8.TabIndex = 45;
            this.label8.Text = "8";
            // 
            // label7
            // 
            this.label7.Image = null;
            this.label7.Location = new System.Drawing.Point(40, 16);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(24, 16);
            this.label7.TabIndex = 44;
            this.label7.Text = "7";
            // 
            // label6
            // 
            this.label6.Image = null;
            this.label6.Location = new System.Drawing.Point(64, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(24, 16);
            this.label6.TabIndex = 43;
            this.label6.Text = "6";
            // 
            // label1
            // 
            this.label1.Image = null;
            this.label1.Location = new System.Drawing.Point(88, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 16);
            this.label1.TabIndex = 42;
            this.label1.Text = "5";
            // 
            // txtGPIORead
            // 
            this.txtGPIORead.Location = new System.Drawing.Point(64, 80);
            this.txtGPIORead.Name = "txtGPIORead";
            this.txtGPIORead.ReadOnly = true;
            this.txtGPIORead.Size = new System.Drawing.Size(40, 20);
            this.txtGPIORead.TabIndex = 39;
            this.txtGPIORead.Text = "0";
            // 
            // txtGPIOWrite
            // 
            this.txtGPIOWrite.Location = new System.Drawing.Point(160, 80);
            this.txtGPIOWrite.Name = "txtGPIOWrite";
            this.txtGPIOWrite.Size = new System.Drawing.Size(40, 20);
            this.txtGPIOWrite.TabIndex = 38;
            this.txtGPIOWrite.Text = "0";
            // 
            // btnGPIOWriteVal
            // 
            this.btnGPIOWriteVal.Image = null;
            this.btnGPIOWriteVal.Location = new System.Drawing.Point(112, 80);
            this.btnGPIOWriteVal.Name = "btnGPIOWriteVal";
            this.btnGPIOWriteVal.Size = new System.Drawing.Size(40, 23);
            this.btnGPIOWriteVal.TabIndex = 40;
            this.btnGPIOWriteVal.Text = "Write";
            this.btnGPIOWriteVal.Click += new System.EventHandler(this.btnGPIOWriteVal_Click);
            // 
            // btnGPIORead
            // 
            this.btnGPIORead.Image = null;
            this.btnGPIORead.Location = new System.Drawing.Point(16, 80);
            this.btnGPIORead.Name = "btnGPIORead";
            this.btnGPIORead.Size = new System.Drawing.Size(40, 23);
            this.btnGPIORead.TabIndex = 41;
            this.btnGPIORead.Text = "Read";
            this.btnGPIORead.Click += new System.EventHandler(this.btnGPIORead_Click);
            // 
            // label5
            // 
            this.label5.Image = null;
            this.label5.Location = new System.Drawing.Point(112, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(24, 16);
            this.label5.TabIndex = 33;
            this.label5.Text = "4";
            // 
            // label4
            // 
            this.label4.Image = null;
            this.label4.Location = new System.Drawing.Point(136, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(24, 16);
            this.label4.TabIndex = 32;
            this.label4.Text = "3";
            // 
            // label3
            // 
            this.label3.Image = null;
            this.label3.Location = new System.Drawing.Point(160, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(24, 16);
            this.label3.TabIndex = 31;
            this.label3.Text = "2";
            // 
            // label2
            // 
            this.label2.Image = null;
            this.label2.Location = new System.Drawing.Point(184, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(24, 16);
            this.label2.TabIndex = 30;
            this.label2.Text = "1";
            // 
            // chkGPIO4
            // 
            this.chkGPIO4.Image = null;
            this.chkGPIO4.Location = new System.Drawing.Point(112, 32);
            this.chkGPIO4.Name = "chkGPIO4";
            this.chkGPIO4.Size = new System.Drawing.Size(16, 24);
            this.chkGPIO4.TabIndex = 28;
            this.chkGPIO4.CheckedChanged += new System.EventHandler(this.chkGPIO_CheckedChanged);
            // 
            // chkGPIO7
            // 
            this.chkGPIO7.Image = null;
            this.chkGPIO7.Location = new System.Drawing.Point(40, 32);
            this.chkGPIO7.Name = "chkGPIO7";
            this.chkGPIO7.Size = new System.Drawing.Size(16, 24);
            this.chkGPIO7.TabIndex = 23;
            this.chkGPIO7.CheckedChanged += new System.EventHandler(this.chkGPIO_CheckedChanged);
            // 
            // chkGPIO2
            // 
            this.chkGPIO2.Image = null;
            this.chkGPIO2.Location = new System.Drawing.Point(160, 32);
            this.chkGPIO2.Name = "chkGPIO2";
            this.chkGPIO2.Size = new System.Drawing.Size(16, 24);
            this.chkGPIO2.TabIndex = 27;
            this.chkGPIO2.CheckedChanged += new System.EventHandler(this.chkGPIO_CheckedChanged);
            // 
            // chkGPIO1
            // 
            this.chkGPIO1.Image = null;
            this.chkGPIO1.Location = new System.Drawing.Point(184, 32);
            this.chkGPIO1.Name = "chkGPIO1";
            this.chkGPIO1.Size = new System.Drawing.Size(16, 24);
            this.chkGPIO1.TabIndex = 26;
            this.chkGPIO1.CheckedChanged += new System.EventHandler(this.chkGPIO_CheckedChanged);
            // 
            // chkGPIO6
            // 
            this.chkGPIO6.Image = null;
            this.chkGPIO6.Location = new System.Drawing.Point(64, 32);
            this.chkGPIO6.Name = "chkGPIO6";
            this.chkGPIO6.Size = new System.Drawing.Size(16, 24);
            this.chkGPIO6.TabIndex = 25;
            this.chkGPIO6.CheckedChanged += new System.EventHandler(this.chkGPIO_CheckedChanged);
            // 
            // chkGPIO5
            // 
            this.chkGPIO5.Image = null;
            this.chkGPIO5.Location = new System.Drawing.Point(88, 32);
            this.chkGPIO5.Name = "chkGPIO5";
            this.chkGPIO5.Size = new System.Drawing.Size(16, 24);
            this.chkGPIO5.TabIndex = 24;
            this.chkGPIO5.CheckedChanged += new System.EventHandler(this.chkGPIO_CheckedChanged);
            // 
            // chkGPIO3
            // 
            this.chkGPIO3.Image = null;
            this.chkGPIO3.Location = new System.Drawing.Point(136, 32);
            this.chkGPIO3.Name = "chkGPIO3";
            this.chkGPIO3.Size = new System.Drawing.Size(16, 24);
            this.chkGPIO3.TabIndex = 29;
            this.chkGPIO3.CheckedChanged += new System.EventHandler(this.chkGPIO_CheckedChanged);
            // 
            // chkGPIO8
            // 
            this.chkGPIO8.Image = null;
            this.chkGPIO8.Location = new System.Drawing.Point(16, 32);
            this.chkGPIO8.Name = "chkGPIO8";
            this.chkGPIO8.Size = new System.Drawing.Size(16, 24);
            this.chkGPIO8.TabIndex = 22;
            this.chkGPIO8.CheckedChanged += new System.EventHandler(this.chkGPIO_CheckedChanged);
            // 
            // grpCodec
            // 
            this.grpCodec.Controls.Add(this.comboCodecReg);
            this.grpCodec.Controls.Add(this.txtCodecRead);
            this.grpCodec.Controls.Add(this.txtCodecWrite);
            this.grpCodec.Controls.Add(this.lblCodecRegister);
            this.grpCodec.Controls.Add(this.btnCodecWrite);
            this.grpCodec.Controls.Add(this.btnCodecRead);
            this.grpCodec.Location = new System.Drawing.Point(408, 128);
            this.grpCodec.Name = "grpCodec";
            this.grpCodec.Size = new System.Drawing.Size(192, 112);
            this.grpCodec.TabIndex = 28;
            this.grpCodec.TabStop = false;
            this.grpCodec.Text = "Codec";
            // 
            // comboCodecReg
            // 
            this.comboCodecReg.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboCodecReg.DropDownWidth = 40;
            this.comboCodecReg.Location = new System.Drawing.Point(64, 24);
            this.comboCodecReg.Name = "comboCodecReg";
            this.comboCodecReg.Size = new System.Drawing.Size(40, 21);
            this.comboCodecReg.TabIndex = 32;
            this.comboCodecReg.SelectedIndexChanged += new System.EventHandler(this.comboCodecReg_SelectedIndexChanged);
            // 
            // txtCodecRead
            // 
            this.txtCodecRead.Location = new System.Drawing.Point(64, 48);
            this.txtCodecRead.Name = "txtCodecRead";
            this.txtCodecRead.ReadOnly = true;
            this.txtCodecRead.Size = new System.Drawing.Size(40, 20);
            this.txtCodecRead.TabIndex = 29;
            this.txtCodecRead.Text = "0";
            // 
            // txtCodecWrite
            // 
            this.txtCodecWrite.Location = new System.Drawing.Point(112, 24);
            this.txtCodecWrite.Name = "txtCodecWrite";
            this.txtCodecWrite.Size = new System.Drawing.Size(40, 20);
            this.txtCodecWrite.TabIndex = 28;
            this.txtCodecWrite.Text = "0";
            // 
            // lblCodecRegister
            // 
            this.lblCodecRegister.Image = null;
            this.lblCodecRegister.Location = new System.Drawing.Point(16, 24);
            this.lblCodecRegister.Name = "lblCodecRegister";
            this.lblCodecRegister.Size = new System.Drawing.Size(56, 16);
            this.lblCodecRegister.TabIndex = 27;
            this.lblCodecRegister.Text = "Register:";
            // 
            // btnCodecWrite
            // 
            this.btnCodecWrite.Image = null;
            this.btnCodecWrite.Location = new System.Drawing.Point(112, 48);
            this.btnCodecWrite.Name = "btnCodecWrite";
            this.btnCodecWrite.Size = new System.Drawing.Size(40, 23);
            this.btnCodecWrite.TabIndex = 30;
            this.btnCodecWrite.Text = "Write";
            this.btnCodecWrite.Click += new System.EventHandler(this.btnCodecWrite_Click);
            // 
            // btnCodecRead
            // 
            this.btnCodecRead.Image = null;
            this.btnCodecRead.Location = new System.Drawing.Point(16, 48);
            this.btnCodecRead.Name = "btnCodecRead";
            this.btnCodecRead.Size = new System.Drawing.Size(40, 23);
            this.btnCodecRead.TabIndex = 31;
            this.btnCodecRead.Text = "Read";
            this.btnCodecRead.Click += new System.EventHandler(this.btnCodecRead_Click);
            // 
            // grpTRXPot
            // 
            this.grpTRXPot.Controls.Add(this.txtTRXPotRead);
            this.grpTRXPot.Controls.Add(this.txtTRXPotWrite);
            this.grpTRXPot.Controls.Add(this.btnTRXPotWrite);
            this.grpTRXPot.Controls.Add(this.btnTRXPotRead);
            this.grpTRXPot.Controls.Add(this.comboTRXPotIndex);
            this.grpTRXPot.Controls.Add(this.lblTRXPotIndex);
            this.grpTRXPot.Location = new System.Drawing.Point(8, 304);
            this.grpTRXPot.Name = "grpTRXPot";
            this.grpTRXPot.Size = new System.Drawing.Size(168, 88);
            this.grpTRXPot.TabIndex = 26;
            this.grpTRXPot.TabStop = false;
            this.grpTRXPot.Text = "TRX Pot";
            // 
            // txtTRXPotRead
            // 
            this.txtTRXPotRead.Location = new System.Drawing.Point(64, 56);
            this.txtTRXPotRead.Name = "txtTRXPotRead";
            this.txtTRXPotRead.ReadOnly = true;
            this.txtTRXPotRead.Size = new System.Drawing.Size(40, 20);
            this.txtTRXPotRead.TabIndex = 27;
            this.txtTRXPotRead.Text = "0";
            // 
            // txtTRXPotWrite
            // 
            this.txtTRXPotWrite.Location = new System.Drawing.Point(112, 24);
            this.txtTRXPotWrite.Name = "txtTRXPotWrite";
            this.txtTRXPotWrite.Size = new System.Drawing.Size(40, 20);
            this.txtTRXPotWrite.TabIndex = 26;
            this.txtTRXPotWrite.Text = "0";
            // 
            // btnTRXPotWrite
            // 
            this.btnTRXPotWrite.Image = null;
            this.btnTRXPotWrite.Location = new System.Drawing.Point(112, 56);
            this.btnTRXPotWrite.Name = "btnTRXPotWrite";
            this.btnTRXPotWrite.Size = new System.Drawing.Size(40, 23);
            this.btnTRXPotWrite.TabIndex = 28;
            this.btnTRXPotWrite.Text = "Write";
            this.btnTRXPotWrite.Click += new System.EventHandler(this.btnTRXPotWrite_Click);
            // 
            // btnTRXPotRead
            // 
            this.btnTRXPotRead.Image = null;
            this.btnTRXPotRead.Location = new System.Drawing.Point(16, 56);
            this.btnTRXPotRead.Name = "btnTRXPotRead";
            this.btnTRXPotRead.Size = new System.Drawing.Size(40, 23);
            this.btnTRXPotRead.TabIndex = 29;
            this.btnTRXPotRead.Text = "Read";
            this.btnTRXPotRead.Click += new System.EventHandler(this.btnTRXPotRead_Click);
            // 
            // comboTRXPotIndex
            // 
            this.comboTRXPotIndex.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboTRXPotIndex.DropDownWidth = 40;
            this.comboTRXPotIndex.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3"});
            this.comboTRXPotIndex.Location = new System.Drawing.Point(56, 24);
            this.comboTRXPotIndex.Name = "comboTRXPotIndex";
            this.comboTRXPotIndex.Size = new System.Drawing.Size(40, 21);
            this.comboTRXPotIndex.TabIndex = 2;
            this.comboTRXPotIndex.SelectedIndexChanged += new System.EventHandler(this.comboTRXPotIndex_SelectedIndexChanged);
            // 
            // lblTRXPotIndex
            // 
            this.lblTRXPotIndex.Image = null;
            this.lblTRXPotIndex.Location = new System.Drawing.Point(16, 24);
            this.lblTRXPotIndex.Name = "lblTRXPotIndex";
            this.lblTRXPotIndex.Size = new System.Drawing.Size(40, 23);
            this.lblTRXPotIndex.TabIndex = 3;
            this.lblTRXPotIndex.Text = "Index:";
            // 
            // grpEEPROM
            // 
            this.grpEEPROM.Controls.Add(this.chkRX2EEPROM);
            this.grpEEPROM.Controls.Add(this.txtEEPROMReadFloat);
            this.grpEEPROM.Controls.Add(this.txtEEPROMWriteFloat);
            this.grpEEPROM.Controls.Add(this.btnEEPROMWriteFloat);
            this.grpEEPROM.Controls.Add(this.btnEEPROMReadFloat);
            this.grpEEPROM.Controls.Add(this.txtEEPROMOffset);
            this.grpEEPROM.Controls.Add(this.txtEEPROMRead);
            this.grpEEPROM.Controls.Add(this.txtEEPROMWrite);
            this.grpEEPROM.Controls.Add(this.lblEEPROMOffset);
            this.grpEEPROM.Controls.Add(this.btnEEPROMWrite);
            this.grpEEPROM.Controls.Add(this.btnEEPROMRead);
            this.grpEEPROM.Location = new System.Drawing.Point(8, 128);
            this.grpEEPROM.Name = "grpEEPROM";
            this.grpEEPROM.Size = new System.Drawing.Size(168, 168);
            this.grpEEPROM.TabIndex = 25;
            this.grpEEPROM.TabStop = false;
            this.grpEEPROM.Text = "EEPROM";
            // 
            // chkRX2EEPROM
            // 
            this.chkRX2EEPROM.Image = null;
            this.chkRX2EEPROM.Location = new System.Drawing.Point(120, 16);
            this.chkRX2EEPROM.Name = "chkRX2EEPROM";
            this.chkRX2EEPROM.Size = new System.Drawing.Size(50, 24);
            this.chkRX2EEPROM.TabIndex = 44;
            this.chkRX2EEPROM.Text = "rx2";
            this.chkRX2EEPROM.CheckedChanged += new System.EventHandler(this.chkRX2EEPROM_CheckedChanged);
            // 
            // txtEEPROMReadFloat
            // 
            this.txtEEPROMReadFloat.Location = new System.Drawing.Point(64, 128);
            this.txtEEPROMReadFloat.Name = "txtEEPROMReadFloat";
            this.txtEEPROMReadFloat.ReadOnly = true;
            this.txtEEPROMReadFloat.Size = new System.Drawing.Size(64, 20);
            this.txtEEPROMReadFloat.TabIndex = 28;
            this.txtEEPROMReadFloat.Text = "0";
            // 
            // txtEEPROMWriteFloat
            // 
            this.txtEEPROMWriteFloat.Location = new System.Drawing.Point(64, 104);
            this.txtEEPROMWriteFloat.Name = "txtEEPROMWriteFloat";
            this.txtEEPROMWriteFloat.Size = new System.Drawing.Size(64, 20);
            this.txtEEPROMWriteFloat.TabIndex = 27;
            this.txtEEPROMWriteFloat.Text = "0";
            // 
            // btnEEPROMWriteFloat
            // 
            this.btnEEPROMWriteFloat.Image = null;
            this.btnEEPROMWriteFloat.Location = new System.Drawing.Point(6, 104);
            this.btnEEPROMWriteFloat.Name = "btnEEPROMWriteFloat";
            this.btnEEPROMWriteFloat.Size = new System.Drawing.Size(50, 23);
            this.btnEEPROMWriteFloat.TabIndex = 29;
            this.btnEEPROMWriteFloat.Text = "WriteF";
            this.btnEEPROMWriteFloat.Click += new System.EventHandler(this.btnEEPROMWriteFloat_Click);
            // 
            // btnEEPROMReadFloat
            // 
            this.btnEEPROMReadFloat.Image = null;
            this.btnEEPROMReadFloat.Location = new System.Drawing.Point(6, 128);
            this.btnEEPROMReadFloat.Name = "btnEEPROMReadFloat";
            this.btnEEPROMReadFloat.Size = new System.Drawing.Size(50, 23);
            this.btnEEPROMReadFloat.TabIndex = 30;
            this.btnEEPROMReadFloat.Text = "ReadF";
            this.btnEEPROMReadFloat.Click += new System.EventHandler(this.btnEEPROMReadFloat_Click);
            // 
            // txtEEPROMOffset
            // 
            this.txtEEPROMOffset.Location = new System.Drawing.Point(64, 24);
            this.txtEEPROMOffset.Name = "txtEEPROMOffset";
            this.txtEEPROMOffset.Size = new System.Drawing.Size(40, 20);
            this.txtEEPROMOffset.TabIndex = 26;
            this.txtEEPROMOffset.Text = "0";
            this.txtEEPROMOffset.TextChanged += new System.EventHandler(this.txtEEPROMOffset_TextChanged);
            // 
            // txtEEPROMRead
            // 
            this.txtEEPROMRead.Location = new System.Drawing.Point(64, 72);
            this.txtEEPROMRead.Name = "txtEEPROMRead";
            this.txtEEPROMRead.ReadOnly = true;
            this.txtEEPROMRead.Size = new System.Drawing.Size(64, 20);
            this.txtEEPROMRead.TabIndex = 4;
            this.txtEEPROMRead.Text = "0";
            // 
            // txtEEPROMWrite
            // 
            this.txtEEPROMWrite.Location = new System.Drawing.Point(64, 48);
            this.txtEEPROMWrite.Name = "txtEEPROMWrite";
            this.txtEEPROMWrite.Size = new System.Drawing.Size(64, 20);
            this.txtEEPROMWrite.TabIndex = 3;
            this.txtEEPROMWrite.Text = "0";
            // 
            // lblEEPROMOffset
            // 
            this.lblEEPROMOffset.Image = null;
            this.lblEEPROMOffset.Location = new System.Drawing.Point(16, 24);
            this.lblEEPROMOffset.Name = "lblEEPROMOffset";
            this.lblEEPROMOffset.Size = new System.Drawing.Size(56, 16);
            this.lblEEPROMOffset.TabIndex = 1;
            this.lblEEPROMOffset.Text = "Offset:";
            // 
            // btnEEPROMWrite
            // 
            this.btnEEPROMWrite.Image = null;
            this.btnEEPROMWrite.Location = new System.Drawing.Point(6, 48);
            this.btnEEPROMWrite.Name = "btnEEPROMWrite";
            this.btnEEPROMWrite.Size = new System.Drawing.Size(50, 23);
            this.btnEEPROMWrite.TabIndex = 24;
            this.btnEEPROMWrite.Text = "Write";
            this.btnEEPROMWrite.Click += new System.EventHandler(this.btnEEPROMWrite_Click);
            // 
            // btnEEPROMRead
            // 
            this.btnEEPROMRead.Image = null;
            this.btnEEPROMRead.Location = new System.Drawing.Point(6, 72);
            this.btnEEPROMRead.Name = "btnEEPROMRead";
            this.btnEEPROMRead.Size = new System.Drawing.Size(50, 23);
            this.btnEEPROMRead.TabIndex = 25;
            this.btnEEPROMRead.Text = "Read";
            this.btnEEPROMRead.Click += new System.EventHandler(this.btnEEPROMRead_Click);
            // 
            // grpPIO
            // 
            this.grpPIO.Controls.Add(this.comboPIOReg);
            this.grpPIO.Controls.Add(this.txtPIORead);
            this.grpPIO.Controls.Add(this.txtPIOWrite);
            this.grpPIO.Controls.Add(this.lblPIORegister);
            this.grpPIO.Controls.Add(this.btnPIOWrite);
            this.grpPIO.Controls.Add(this.btnPIORead);
            this.grpPIO.Controls.Add(this.lblPIOChip);
            this.grpPIO.Controls.Add(this.comboPIOChip);
            this.grpPIO.Location = new System.Drawing.Point(408, 8);
            this.grpPIO.Name = "grpPIO";
            this.grpPIO.Size = new System.Drawing.Size(192, 112);
            this.grpPIO.TabIndex = 24;
            this.grpPIO.TabStop = false;
            this.grpPIO.Text = "PIO";
            // 
            // comboPIOReg
            // 
            this.comboPIOReg.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboPIOReg.DropDownWidth = 80;
            this.comboPIOReg.Items.AddRange(new object[] {
            "0 (Input 0)",
            "1 (Input 1)",
            "2 (Output 0)",
            "3 (Output 1)",
            "4 (Pol Inv 0)",
            "5 (Pol Inv 1)",
            "6 (DDR 0)",
            "7 (DDR 1)"});
            this.comboPIOReg.Location = new System.Drawing.Point(64, 48);
            this.comboPIOReg.Name = "comboPIOReg";
            this.comboPIOReg.Size = new System.Drawing.Size(32, 21);
            this.comboPIOReg.TabIndex = 32;
            this.comboPIOReg.SelectedIndexChanged += new System.EventHandler(this.comboPIOReg_SelectedIndexChanged);
            // 
            // txtPIORead
            // 
            this.txtPIORead.Location = new System.Drawing.Point(64, 72);
            this.txtPIORead.Name = "txtPIORead";
            this.txtPIORead.ReadOnly = true;
            this.txtPIORead.Size = new System.Drawing.Size(40, 20);
            this.txtPIORead.TabIndex = 29;
            this.txtPIORead.Text = "0";
            // 
            // txtPIOWrite
            // 
            this.txtPIOWrite.Location = new System.Drawing.Point(112, 48);
            this.txtPIOWrite.Name = "txtPIOWrite";
            this.txtPIOWrite.Size = new System.Drawing.Size(40, 20);
            this.txtPIOWrite.TabIndex = 28;
            this.txtPIOWrite.Text = "0";
            // 
            // lblPIORegister
            // 
            this.lblPIORegister.Image = null;
            this.lblPIORegister.Location = new System.Drawing.Point(16, 48);
            this.lblPIORegister.Name = "lblPIORegister";
            this.lblPIORegister.Size = new System.Drawing.Size(56, 16);
            this.lblPIORegister.TabIndex = 27;
            this.lblPIORegister.Text = "Register:";
            // 
            // btnPIOWrite
            // 
            this.btnPIOWrite.Image = null;
            this.btnPIOWrite.Location = new System.Drawing.Point(112, 72);
            this.btnPIOWrite.Name = "btnPIOWrite";
            this.btnPIOWrite.Size = new System.Drawing.Size(40, 23);
            this.btnPIOWrite.TabIndex = 30;
            this.btnPIOWrite.Text = "Write";
            this.btnPIOWrite.Click += new System.EventHandler(this.btnPIOWrite_Click);
            // 
            // btnPIORead
            // 
            this.btnPIORead.Image = null;
            this.btnPIORead.Location = new System.Drawing.Point(16, 72);
            this.btnPIORead.Name = "btnPIORead";
            this.btnPIORead.Size = new System.Drawing.Size(40, 23);
            this.btnPIORead.TabIndex = 31;
            this.btnPIORead.Text = "Read";
            this.btnPIORead.Click += new System.EventHandler(this.btnPIORead_Click);
            // 
            // lblPIOChip
            // 
            this.lblPIOChip.Image = null;
            this.lblPIOChip.Location = new System.Drawing.Point(16, 16);
            this.lblPIOChip.Name = "lblPIOChip";
            this.lblPIOChip.Size = new System.Drawing.Size(32, 23);
            this.lblPIOChip.TabIndex = 1;
            this.lblPIOChip.Text = "Chip:";
            // 
            // comboPIOChip
            // 
            this.comboPIOChip.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboPIOChip.DropDownWidth = 80;
            this.comboPIOChip.Items.AddRange(new object[] {
            "TRX IC14",
            "TRX IC26",
            "TRX IC30",
            "TRX IC31",
            "RFIO IC1",
            "PA IC9",
            "RX2 IC30",
            "RX2 IC31",
            "VU IC16",
            "VU IC17"});
            this.comboPIOChip.Location = new System.Drawing.Point(48, 16);
            this.comboPIOChip.Name = "comboPIOChip";
            this.comboPIOChip.Size = new System.Drawing.Size(80, 21);
            this.comboPIOChip.TabIndex = 0;
            this.comboPIOChip.SelectedIndexChanged += new System.EventHandler(this.comboPIOChip_SelectedIndexChanged);
            // 
            // grpClockGen
            // 
            this.grpClockGen.Controls.Add(this.chkClockGenCS);
            this.grpClockGen.Controls.Add(this.chkClockGenReset);
            this.grpClockGen.Controls.Add(this.comboClockGenReg);
            this.grpClockGen.Controls.Add(this.txtClockGenReadVal);
            this.grpClockGen.Controls.Add(this.txtClockGenWriteVal);
            this.grpClockGen.Controls.Add(this.lblClockGenReg);
            this.grpClockGen.Controls.Add(this.btnClockGenWrite);
            this.grpClockGen.Controls.Add(this.btnClockGenRead);
            this.grpClockGen.Location = new System.Drawing.Point(8, 8);
            this.grpClockGen.Name = "grpClockGen";
            this.grpClockGen.Size = new System.Drawing.Size(168, 112);
            this.grpClockGen.TabIndex = 22;
            this.grpClockGen.TabStop = false;
            this.grpClockGen.Text = "ClockGen";
            // 
            // chkClockGenCS
            // 
            this.chkClockGenCS.Image = null;
            this.chkClockGenCS.Location = new System.Drawing.Point(88, 80);
            this.chkClockGenCS.Name = "chkClockGenCS";
            this.chkClockGenCS.Size = new System.Drawing.Size(40, 24);
            this.chkClockGenCS.TabIndex = 28;
            this.chkClockGenCS.Text = "CS";
            this.chkClockGenCS.CheckedChanged += new System.EventHandler(this.chkClockGenCS_CheckedChanged);
            // 
            // chkClockGenReset
            // 
            this.chkClockGenReset.Image = null;
            this.chkClockGenReset.Location = new System.Drawing.Point(16, 80);
            this.chkClockGenReset.Name = "chkClockGenReset";
            this.chkClockGenReset.Size = new System.Drawing.Size(56, 24);
            this.chkClockGenReset.TabIndex = 27;
            this.chkClockGenReset.Text = "Reset";
            this.chkClockGenReset.CheckedChanged += new System.EventHandler(this.chkClockGenReset_CheckedChanged);
            // 
            // comboClockGenReg
            // 
            this.comboClockGenReg.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboClockGenReg.DropDownWidth = 40;
            this.comboClockGenReg.Location = new System.Drawing.Point(64, 24);
            this.comboClockGenReg.Name = "comboClockGenReg";
            this.comboClockGenReg.Size = new System.Drawing.Size(40, 21);
            this.comboClockGenReg.TabIndex = 26;
            this.comboClockGenReg.SelectedIndexChanged += new System.EventHandler(this.comboClockGenReg_SelectedIndexChanged);
            // 
            // txtClockGenReadVal
            // 
            this.txtClockGenReadVal.Location = new System.Drawing.Point(64, 48);
            this.txtClockGenReadVal.Name = "txtClockGenReadVal";
            this.txtClockGenReadVal.ReadOnly = true;
            this.txtClockGenReadVal.Size = new System.Drawing.Size(40, 20);
            this.txtClockGenReadVal.TabIndex = 4;
            this.txtClockGenReadVal.Text = "0";
            // 
            // txtClockGenWriteVal
            // 
            this.txtClockGenWriteVal.Location = new System.Drawing.Point(112, 24);
            this.txtClockGenWriteVal.Name = "txtClockGenWriteVal";
            this.txtClockGenWriteVal.Size = new System.Drawing.Size(40, 20);
            this.txtClockGenWriteVal.TabIndex = 3;
            this.txtClockGenWriteVal.Text = "0";
            // 
            // lblClockGenReg
            // 
            this.lblClockGenReg.Image = null;
            this.lblClockGenReg.Location = new System.Drawing.Point(16, 24);
            this.lblClockGenReg.Name = "lblClockGenReg";
            this.lblClockGenReg.Size = new System.Drawing.Size(56, 16);
            this.lblClockGenReg.TabIndex = 1;
            this.lblClockGenReg.Text = "Register:";
            // 
            // btnClockGenWrite
            // 
            this.btnClockGenWrite.Image = null;
            this.btnClockGenWrite.Location = new System.Drawing.Point(112, 48);
            this.btnClockGenWrite.Name = "btnClockGenWrite";
            this.btnClockGenWrite.Size = new System.Drawing.Size(40, 23);
            this.btnClockGenWrite.TabIndex = 24;
            this.btnClockGenWrite.Text = "Write";
            this.btnClockGenWrite.Click += new System.EventHandler(this.btnClockGenWrite_Click);
            // 
            // btnClockGenRead
            // 
            this.btnClockGenRead.Image = null;
            this.btnClockGenRead.Location = new System.Drawing.Point(16, 48);
            this.btnClockGenRead.Name = "btnClockGenRead";
            this.btnClockGenRead.Size = new System.Drawing.Size(40, 23);
            this.btnClockGenRead.TabIndex = 25;
            this.btnClockGenRead.Text = "Read";
            this.btnClockGenRead.Click += new System.EventHandler(this.btnClockGenRead_Click);
            // 
            // grpDDS
            // 
            this.grpDDS.Controls.Add(this.btnIOUpdate);
            this.grpDDS.Controls.Add(this.chkManualIOUpdate);
            this.grpDDS.Controls.Add(this.chkRX2DDS);
            this.grpDDS.Controls.Add(this.chkDDSCS);
            this.grpDDS.Controls.Add(this.chkDDSReset);
            this.grpDDS.Controls.Add(this.comboDDSChan);
            this.grpDDS.Controls.Add(this.lblDDSChan);
            this.grpDDS.Controls.Add(this.comboDDSReg);
            this.grpDDS.Controls.Add(this.txtDDSWrite);
            this.grpDDS.Controls.Add(this.lblDDSReg);
            this.grpDDS.Controls.Add(this.btnDDSWrite);
            this.grpDDS.Controls.Add(this.txtDDSReadVal);
            this.grpDDS.Controls.Add(this.btnDDSRead);
            this.grpDDS.Location = new System.Drawing.Point(184, 8);
            this.grpDDS.Name = "grpDDS";
            this.grpDDS.Size = new System.Drawing.Size(208, 112);
            this.grpDDS.TabIndex = 23;
            this.grpDDS.TabStop = false;
            this.grpDDS.Text = "DDS";
            // 
            // btnIOUpdate
            // 
            this.btnIOUpdate.Image = null;
            this.btnIOUpdate.Location = new System.Drawing.Point(136, 79);
            this.btnIOUpdate.Name = "btnIOUpdate";
            this.btnIOUpdate.Size = new System.Drawing.Size(64, 23);
            this.btnIOUpdate.TabIndex = 45;
            this.btnIOUpdate.Text = "IO Update";
            this.btnIOUpdate.Click += new System.EventHandler(this.btnIOUpdate_Click);
            // 
            // chkManualIOUpdate
            // 
            this.chkManualIOUpdate.Image = null;
            this.chkManualIOUpdate.Location = new System.Drawing.Point(116, 80);
            this.chkManualIOUpdate.Name = "chkManualIOUpdate";
            this.chkManualIOUpdate.Size = new System.Drawing.Size(20, 24);
            this.chkManualIOUpdate.TabIndex = 44;
            this.chkManualIOUpdate.CheckedChanged += new System.EventHandler(this.chkManualIOUpdate_CheckedChanged);
            // 
            // chkRX2DDS
            // 
            this.chkRX2DDS.Image = null;
            this.chkRX2DDS.Location = new System.Drawing.Point(184, 8);
            this.chkRX2DDS.Name = "chkRX2DDS";
            this.chkRX2DDS.Size = new System.Drawing.Size(20, 24);
            this.chkRX2DDS.TabIndex = 43;
            this.chkRX2DDS.Text = "RX2";
            // 
            // chkDDSCS
            // 
            this.chkDDSCS.Image = null;
            this.chkDDSCS.Location = new System.Drawing.Point(6, 77);
            this.chkDDSCS.Name = "chkDDSCS";
            this.chkDDSCS.Size = new System.Drawing.Size(40, 24);
            this.chkDDSCS.TabIndex = 42;
            this.chkDDSCS.Text = "CS";
            this.chkDDSCS.Visible = false;
            this.chkDDSCS.CheckedChanged += new System.EventHandler(this.chkDDSCS_CheckedChanged);
            // 
            // chkDDSReset
            // 
            this.chkDDSReset.Image = null;
            this.chkDDSReset.Location = new System.Drawing.Point(6, 78);
            this.chkDDSReset.Name = "chkDDSReset";
            this.chkDDSReset.Size = new System.Drawing.Size(56, 24);
            this.chkDDSReset.TabIndex = 41;
            this.chkDDSReset.Text = "Reset";
            this.chkDDSReset.Visible = false;
            this.chkDDSReset.CheckedChanged += new System.EventHandler(this.chkDDSReset_CheckedChanged);
            // 
            // comboDDSChan
            // 
            this.comboDDSChan.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboDDSChan.DropDownWidth = 40;
            this.comboDDSChan.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3"});
            this.comboDDSChan.Location = new System.Drawing.Point(64, 80);
            this.comboDDSChan.Name = "comboDDSChan";
            this.comboDDSChan.Size = new System.Drawing.Size(40, 21);
            this.comboDDSChan.TabIndex = 40;
            this.comboDDSChan.SelectedIndexChanged += new System.EventHandler(this.comboDDSChan_SelectedIndexChanged);
            // 
            // lblDDSChan
            // 
            this.lblDDSChan.Image = null;
            this.lblDDSChan.Location = new System.Drawing.Point(16, 80);
            this.lblDDSChan.Name = "lblDDSChan";
            this.lblDDSChan.Size = new System.Drawing.Size(56, 16);
            this.lblDDSChan.TabIndex = 39;
            this.lblDDSChan.Text = "Channel:";
            // 
            // comboDDSReg
            // 
            this.comboDDSReg.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboDDSReg.DropDownWidth = 40;
            this.comboDDSReg.Location = new System.Drawing.Point(64, 24);
            this.comboDDSReg.Name = "comboDDSReg";
            this.comboDDSReg.Size = new System.Drawing.Size(40, 21);
            this.comboDDSReg.TabIndex = 38;
            this.comboDDSReg.SelectedIndexChanged += new System.EventHandler(this.comboDDSReg_SelectedIndexChanged);
            // 
            // txtDDSWrite
            // 
            this.txtDDSWrite.Location = new System.Drawing.Point(112, 24);
            this.txtDDSWrite.Name = "txtDDSWrite";
            this.txtDDSWrite.Size = new System.Drawing.Size(64, 20);
            this.txtDDSWrite.TabIndex = 34;
            this.txtDDSWrite.Text = "0";
            // 
            // lblDDSReg
            // 
            this.lblDDSReg.Image = null;
            this.lblDDSReg.Location = new System.Drawing.Point(16, 24);
            this.lblDDSReg.Name = "lblDDSReg";
            this.lblDDSReg.Size = new System.Drawing.Size(56, 16);
            this.lblDDSReg.TabIndex = 33;
            this.lblDDSReg.Text = "Register:";
            // 
            // btnDDSWrite
            // 
            this.btnDDSWrite.Image = null;
            this.btnDDSWrite.Location = new System.Drawing.Point(136, 48);
            this.btnDDSWrite.Name = "btnDDSWrite";
            this.btnDDSWrite.Size = new System.Drawing.Size(40, 23);
            this.btnDDSWrite.TabIndex = 36;
            this.btnDDSWrite.Text = "Write";
            this.btnDDSWrite.Click += new System.EventHandler(this.btnDDSWrite_Click);
            // 
            // txtDDSReadVal
            // 
            this.txtDDSReadVal.Location = new System.Drawing.Point(64, 48);
            this.txtDDSReadVal.Name = "txtDDSReadVal";
            this.txtDDSReadVal.ReadOnly = true;
            this.txtDDSReadVal.Size = new System.Drawing.Size(64, 20);
            this.txtDDSReadVal.TabIndex = 26;
            this.txtDDSReadVal.Text = "0";
            // 
            // btnDDSRead
            // 
            this.btnDDSRead.Image = null;
            this.btnDDSRead.Location = new System.Drawing.Point(16, 48);
            this.btnDDSRead.Name = "btnDDSRead";
            this.btnDDSRead.Size = new System.Drawing.Size(40, 23);
            this.btnDDSRead.TabIndex = 27;
            this.btnDDSRead.Text = "Read";
            this.btnDDSRead.Click += new System.EventHandler(this.btnDDSRead_Click);
            // 
            // comboMuxChan
            // 
            this.comboMuxChan.DropDownWidth = 56;
            this.comboMuxChan.Items.AddRange(new object[] {
            "None",
            "Chan0",
            "Chan1"});
            this.comboMuxChan.Location = new System.Drawing.Point(568, 368);
            this.comboMuxChan.Name = "comboMuxChan";
            this.comboMuxChan.Size = new System.Drawing.Size(56, 21);
            this.comboMuxChan.TabIndex = 0;
            this.comboMuxChan.SelectedIndexChanged += new System.EventHandler(this.comboMuxChan_SelectedIndexChanged);
            // 
            // lblMuxChannel
            // 
            this.lblMuxChannel.Image = null;
            this.lblMuxChannel.Location = new System.Drawing.Point(512, 368);
            this.lblMuxChannel.Name = "lblMuxChannel";
            this.lblMuxChannel.Size = new System.Drawing.Size(56, 23);
            this.lblMuxChannel.TabIndex = 1;
            this.lblMuxChannel.Text = "Mux:";
            // 
            // FLEX5000LLHWForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(640, 588);
            this.Controls.Add(this.buttonTS3);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.labelTS1);
            this.Controls.Add(this.buttonTS2);
            this.Controls.Add(this.buttonTS1);
            this.Controls.Add(this.btnEEPROMRead1);
            this.Controls.Add(this.lblATUC);
            this.Controls.Add(this.udATUC);
            this.Controls.Add(this.lblATUL);
            this.Controls.Add(this.udATUL);
            this.Controls.Add(this.grpATURelays);
            this.Controls.Add(this.grpFlexWire);
            this.Controls.Add(this.grpATU);
            this.Controls.Add(this.grpPAPot);
            this.Controls.Add(this.grpGPIODDR);
            this.Controls.Add(this.grpGPIO);
            this.Controls.Add(this.grpCodec);
            this.Controls.Add(this.grpTRXPot);
            this.Controls.Add(this.grpEEPROM);
            this.Controls.Add(this.grpPIO);
            this.Controls.Add(this.grpClockGen);
            this.Controls.Add(this.grpDDS);
            this.Controls.Add(this.comboMuxChan);
            this.Controls.Add(this.lblMuxChannel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FLEX5000LLHWForm";
            this.Text = "FLEX-5000 Low Level Hardware Control";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.FLEX5000LLHWForm_Closing);
            this.Load += new System.EventHandler(this.FLEX5000LLHWForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.udATUL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udATUC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.grpATURelays.ResumeLayout(false);
            this.grpFlexWire.ResumeLayout(false);
            this.grpFlexWire.PerformLayout();
            this.grpATU.ResumeLayout(false);
            this.grpATU.PerformLayout();
            this.grpPAPot.ResumeLayout(false);
            this.grpPAPot.PerformLayout();
            this.grpGPIODDR.ResumeLayout(false);
            this.grpGPIODDR.PerformLayout();
            this.grpGPIO.ResumeLayout(false);
            this.grpGPIO.PerformLayout();
            this.grpCodec.ResumeLayout(false);
            this.grpCodec.PerformLayout();
            this.grpTRXPot.ResumeLayout(false);
            this.grpTRXPot.PerformLayout();
            this.grpEEPROM.ResumeLayout(false);
            this.grpEEPROM.PerformLayout();
            this.grpPIO.ResumeLayout(false);
            this.grpPIO.PerformLayout();
            this.grpClockGen.ResumeLayout(false);
            this.grpClockGen.PerformLayout();
            this.grpDDS.ResumeLayout(false);
            this.grpDDS.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

       

        private System.Windows.Forms.GroupBoxTS grpClockGen;
        private System.Windows.Forms.ComboBoxTS comboClockGenReg;
        private System.Windows.Forms.TextBox txtClockGenReadVal;
        private System.Windows.Forms.TextBox txtClockGenWriteVal;
        private System.Windows.Forms.LabelTS lblClockGenReg;
        private System.Windows.Forms.ButtonTS btnClockGenWrite;
        private System.Windows.Forms.ButtonTS btnClockGenRead;
        private System.Windows.Forms.GroupBoxTS grpDDS;
        private System.Windows.Forms.ComboBoxTS comboDDSChan;
        private System.Windows.Forms.LabelTS lblDDSChan;
        private System.Windows.Forms.ComboBoxTS comboDDSReg;
        private System.Windows.Forms.TextBox txtDDSWrite;
        private System.Windows.Forms.LabelTS lblDDSReg;
        private System.Windows.Forms.ButtonTS btnDDSWrite;
        private System.Windows.Forms.TextBox txtDDSReadVal;
        private System.Windows.Forms.ButtonTS btnDDSRead;
        private System.Windows.Forms.GroupBoxTS grpPIO;
        private System.Windows.Forms.ComboBoxTS comboPIOChip;
        private System.Windows.Forms.LabelTS lblPIOChip;
        private System.Windows.Forms.ComboBoxTS comboPIOReg;
        private System.Windows.Forms.TextBox txtPIORead;
        private System.Windows.Forms.TextBox txtPIOWrite;
        private System.Windows.Forms.LabelTS lblPIORegister;
        private System.Windows.Forms.ButtonTS btnPIOWrite;
        private System.Windows.Forms.ButtonTS btnPIORead;
        private System.Windows.Forms.GroupBoxTS grpEEPROM;
        private System.Windows.Forms.TextBox txtEEPROMRead;
        private System.Windows.Forms.TextBox txtEEPROMWrite;
        private System.Windows.Forms.LabelTS lblEEPROMOffset;
        private System.Windows.Forms.ButtonTS btnEEPROMWrite;
        private System.Windows.Forms.ButtonTS btnEEPROMRead;
        private System.Windows.Forms.TextBox txtEEPROMOffset;
        private System.Windows.Forms.ComboBoxTS comboMuxChan;
        private System.Windows.Forms.LabelTS lblMuxChannel;
        private System.Windows.Forms.GroupBoxTS grpGPIODDR;
        private System.Windows.Forms.ButtonTS btnGPIODDRWrite;
        private System.Windows.Forms.ButtonTS btnGPIODDRRead;
        private System.Windows.Forms.CheckBoxTS chkGPIODDR4;
        private System.Windows.Forms.CheckBoxTS chkGPIODDR7;
        private System.Windows.Forms.CheckBoxTS chkGPIODDR2;
        private System.Windows.Forms.CheckBoxTS chkGPIODDR1;
        private System.Windows.Forms.CheckBoxTS chkGPIODDR6;
        private System.Windows.Forms.CheckBoxTS chkGPIODDR5;
        private System.Windows.Forms.CheckBoxTS chkGPIODDR3;
        private System.Windows.Forms.CheckBoxTS chkGPIODDR8;
        private System.Windows.Forms.GroupBoxTS grpGPIO;
        private System.Windows.Forms.ButtonTS btnGPIOWriteVal;
        private System.Windows.Forms.ButtonTS btnGPIORead;
        private System.Windows.Forms.LabelTS label5;
        private System.Windows.Forms.LabelTS label4;
        private System.Windows.Forms.LabelTS label3;
        private System.Windows.Forms.LabelTS label2;
        private System.Windows.Forms.CheckBoxTS chkGPIO4;
        private System.Windows.Forms.CheckBoxTS chkGPIO7;
        private System.Windows.Forms.CheckBoxTS chkGPIO2;
        private System.Windows.Forms.CheckBoxTS chkGPIO1;
        private System.Windows.Forms.CheckBoxTS chkGPIO6;
        private System.Windows.Forms.CheckBoxTS chkGPIO5;
        private System.Windows.Forms.CheckBoxTS chkGPIO3;
        private System.Windows.Forms.CheckBoxTS chkGPIO8;
        private System.Windows.Forms.LabelTS label1;
        private System.Windows.Forms.LabelTS label6;
        private System.Windows.Forms.LabelTS label7;
        private System.Windows.Forms.LabelTS label8;
        private System.Windows.Forms.LabelTS label9;
        private System.Windows.Forms.LabelTS label10;
        private System.Windows.Forms.LabelTS label11;
        private System.Windows.Forms.LabelTS label12;
        private System.Windows.Forms.LabelTS label13;
        private System.Windows.Forms.LabelTS label14;
        private System.Windows.Forms.LabelTS label15;
        private System.Windows.Forms.LabelTS label16;
        private System.Windows.Forms.TextBox txtGPIORead;
        private System.Windows.Forms.TextBox txtGPIOWrite;
        private System.Windows.Forms.TextBox txtGPIODDRRead;
        private System.Windows.Forms.TextBox txtGPIODDRWrite;
        private System.Windows.Forms.CheckBoxTS chkClockGenReset;
        private System.Windows.Forms.CheckBoxTS chkDDSReset;
        private System.Windows.Forms.CheckBoxTS chkClockGenCS;
        private System.Windows.Forms.CheckBoxTS chkDDSCS;
        private System.Windows.Forms.GroupBoxTS grpCodec;
        private System.Windows.Forms.ComboBoxTS comboCodecReg;
        private System.Windows.Forms.TextBox txtCodecRead;
        private System.Windows.Forms.TextBox txtCodecWrite;
        private System.Windows.Forms.LabelTS lblCodecRegister;
        private System.Windows.Forms.ButtonTS btnCodecWrite;
        private System.Windows.Forms.ButtonTS btnCodecRead;
        private System.Windows.Forms.GroupBoxTS grpTRXPot;
        private System.Windows.Forms.TextBox txtTRXPotRead;
        private System.Windows.Forms.TextBox txtTRXPotWrite;
        private System.Windows.Forms.ButtonTS btnTRXPotWrite;
        private System.Windows.Forms.ButtonTS btnTRXPotRead;
        private System.Windows.Forms.ComboBoxTS comboTRXPotIndex;
        private System.Windows.Forms.LabelTS lblTRXPotIndex;
        private System.Windows.Forms.GroupBoxTS grpPAPot;
        private System.Windows.Forms.TextBox txtPAPotRead;
        private System.Windows.Forms.TextBox txtPAPotWrite;
        private System.Windows.Forms.ButtonTS btnPAPotWrite;
        private System.Windows.Forms.ButtonTS btnPAPotRead;
        private System.Windows.Forms.ComboBoxTS comboPAPotIndex;
        private System.Windows.Forms.LabelTS lblPAPotIndex;
        private System.Windows.Forms.GroupBoxTS grpATU;
        private System.Windows.Forms.TextBox txtATU1;
        private System.Windows.Forms.ButtonTS btnATUSendCmd;
        private System.Windows.Forms.TextBox txtATU2;
        private System.Windows.Forms.TextBox txtATU3;
        private System.Windows.Forms.ButtonTS btnATUFull;
        private System.Windows.Forms.GroupBoxTS grpFlexWire;
        private System.Windows.Forms.ButtonTS btnFlexWireWriteVal;
        private System.Windows.Forms.TextBox txtFlexWireVal2;
        private System.Windows.Forms.TextBox txtFlexWireVal1;
        private System.Windows.Forms.TextBox txtFlexWireAddr;
        private System.Windows.Forms.ButtonTS btnFlexWireWrite2Val;
        private System.Windows.Forms.LabelTS lblFlexWireAddr;
        private System.Windows.Forms.LabelTS lblFlexWireVal1;
        private System.Windows.Forms.LabelTS lblFlexWireVal2;
        private System.Windows.Forms.CheckBoxTS chkRX2DDS;
        private System.Windows.Forms.CheckBoxTS chkRX2EEPROM;
        private System.Windows.Forms.ButtonTS btnEEPROMWriteFloat;
        private System.Windows.Forms.ButtonTS btnEEPROMReadFloat;
        private System.Windows.Forms.TextBox txtEEPROMReadFloat;
        private System.Windows.Forms.TextBox txtEEPROMWriteFloat;
        private System.Windows.Forms.GroupBoxTS grpATURelays;
        private System.Windows.Forms.CheckBoxTS chkL5;
        private System.Windows.Forms.CheckBoxTS chkL8;
        private System.Windows.Forms.CheckBoxTS chkL3;
        private System.Windows.Forms.CheckBoxTS chkL2;
        private System.Windows.Forms.CheckBoxTS chkL7;
        private System.Windows.Forms.CheckBoxTS chkL6;
        private System.Windows.Forms.CheckBoxTS chkL4;
        private System.Windows.Forms.CheckBoxTS chkL9;
        private System.Windows.Forms.LabelTS lblL8;
        private System.Windows.Forms.LabelTS lblL7;
        private System.Windows.Forms.LabelTS lblL6;
        private System.Windows.Forms.LabelTS lblL5;
        private System.Windows.Forms.LabelTS lblL4;
        private System.Windows.Forms.LabelTS lblL3;
        private System.Windows.Forms.LabelTS lblL2;
        private System.Windows.Forms.LabelTS lblL9;
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
        private System.Windows.Forms.CheckBoxTS chkATUEnable;
        private System.Windows.Forms.CheckBoxTS chkATUATTN;
        private System.Windows.Forms.LabelTS lblHiZ;
        private System.Windows.Forms.CheckBoxTS chkHiZ;
        private System.Windows.Forms.NumericUpDown udATUL;
        private System.Windows.Forms.LabelTS lblATUL;
        private System.Windows.Forms.LabelTS lblATUC;
        private System.Windows.Forms.NumericUpDown udATUC;
        private System.Windows.Forms.ButtonTS btnFlexWireReadVal;
        private System.Windows.Forms.ButtonTS btnFlexWireRead2Val;
        private System.Windows.Forms.CheckBoxTS chkManualIOUpdate;
        private System.Windows.Forms.ButtonTS btnIOUpdate;
        private System.Windows.Forms.ButtonTS btnEEPROMRead1;
        private System.Windows.Forms.ButtonTS buttonTS1;
        private System.Windows.Forms.ButtonTS buttonTS2;
        private System.Windows.Forms.LabelTS labelTS1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.ButtonTS buttonTS3;
        private System.ComponentModel.IContainer components;

       
    }
}
