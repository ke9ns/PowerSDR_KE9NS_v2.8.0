//=================================================================
// fwcAntForm.cs
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
    public partial class FWCAntForm : System.Windows.Forms.Form
    {
       


        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FWCAntForm));
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.radModeSimple = new System.Windows.Forms.RadioButtonTS();
            this.radModeExpert = new System.Windows.Forms.RadioButtonTS();
            this.comboBand = new System.Windows.Forms.ComboBoxTS();
            this.comboBand2 = new System.Windows.Forms.ComboBoxTS();
            this.txtBoxAnt12 = new System.Windows.Forms.TextBoxTS();
            this.txtBoxAnt11 = new System.Windows.Forms.TextBoxTS();
            this.txtBoxAnt10 = new System.Windows.Forms.TextBoxTS();
            this.txtBoxAnt9 = new System.Windows.Forms.TextBoxTS();
            this.txtBoxAnt8 = new System.Windows.Forms.TextBoxTS();
            this.txtBoxAnt7 = new System.Windows.Forms.TextBoxTS();
            this.txtBoxAnt6 = new System.Windows.Forms.TextBoxTS();
            this.txtBoxAnt5 = new System.Windows.Forms.TextBoxTS();
            this.txtBoxAnt4 = new System.Windows.Forms.TextBoxTS();
            this.txtBoxAnt3 = new System.Windows.Forms.TextBoxTS();
            this.txtBoxAnt2 = new System.Windows.Forms.TextBoxTS();
            this.txtBoxAnt1 = new System.Windows.Forms.TextBoxTS();
            this.textBoxTX2Ant = new System.Windows.Forms.TextBoxTS();
            this.textBoxRX2Ant = new System.Windows.Forms.TextBoxTS();
            this.textBoxTX1Ant = new System.Windows.Forms.TextBoxTS();
            this.textBoxRX1Ant = new System.Windows.Forms.TextBoxTS();
            this.chkTX2Active = new System.Windows.Forms.CheckBoxTS();
            this.chkLock = new System.Windows.Forms.CheckBoxTS();
            this.comboTXAnt2 = new System.Windows.Forms.ComboBoxTS();
            this.chkEnable6mPreamp = new System.Windows.Forms.CheckBoxTS();
            this.comboRX1Ant = new System.Windows.Forms.ComboBoxTS();
            this.chkRX1Loop = new System.Windows.Forms.CheckBoxTS();
            this.comboRX2Ant = new System.Windows.Forms.ComboBoxTS();
            this.comboTXAnt = new System.Windows.Forms.ComboBoxTS();
            this.chkRX2TX3 = new System.Windows.Forms.CheckBoxTS();
            this.chkRX2TX2 = new System.Windows.Forms.CheckBoxTS();
            this.textBoxTS1 = new System.Windows.Forms.TextBoxTS();
            this.udTX3Delay = new System.Windows.Forms.NumericUpDownTS();
            this.chkTX3DelayEnable = new System.Windows.Forms.CheckBoxTS();
            this.udTX2Delay = new System.Windows.Forms.NumericUpDownTS();
            this.chkTX2DelayEnable = new System.Windows.Forms.CheckBoxTS();
            this.udTX1Delay = new System.Windows.Forms.NumericUpDownTS();
            this.chkTX1DelayEnable = new System.Windows.Forms.CheckBoxTS();
            this.chkRCATX3 = new System.Windows.Forms.CheckBoxTS();
            this.chkRCATX2 = new System.Windows.Forms.CheckBoxTS();
            this.chkRCATX1 = new System.Windows.Forms.CheckBoxTS();
            this.chkRX2TX1 = new System.Windows.Forms.CheckBoxTS();
            this.chkAlwaysOnTop1 = new System.Windows.Forms.CheckBoxTS();
            this.txtStatus = new System.Windows.Forms.TextBoxTS();
            this.grpComplexity = new System.Windows.Forms.GroupBoxTS();
            this.lblBand2 = new System.Windows.Forms.LabelTS();
            this.lblBand = new System.Windows.Forms.LabelTS();
            this.grpAntenna = new System.Windows.Forms.GroupBoxTS();
            this.lblLoopGain = new System.Windows.Forms.LabelTS();
            this.udLoopGain = new System.Windows.Forms.NumericUpDownTS();
            this.lblRX2 = new System.Windows.Forms.LabelTS();
            this.lblTX = new System.Windows.Forms.LabelTS();
            this.lblRX1 = new System.Windows.Forms.LabelTS();
            this.labelTS1 = new System.Windows.Forms.LabelTS();
            this.grpSwitchRelay = new System.Windows.Forms.GroupBoxTS();
            ((System.ComponentModel.ISupportInitialize)(this.udTX3Delay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udTX2Delay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udTX1Delay)).BeginInit();
            this.grpComplexity.SuspendLayout();
            this.grpAntenna.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udLoopGain)).BeginInit();
            this.grpSwitchRelay.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolTip1
            // 
            this.toolTip1.AutomaticDelay = 400;
            this.toolTip1.AutoPopDelay = 12000;
            this.toolTip1.InitialDelay = 400;
            this.toolTip1.IsBalloon = true;
            this.toolTip1.ReshowDelay = 80;
            this.toolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            // 
            // radModeSimple
            // 
            this.radModeSimple.Checked = true;
            this.radModeSimple.Image = null;
            this.radModeSimple.Location = new System.Drawing.Point(6, 24);
            this.radModeSimple.Name = "radModeSimple";
            this.radModeSimple.Size = new System.Drawing.Size(60, 24);
            this.radModeSimple.TabIndex = 16;
            this.radModeSimple.TabStop = true;
            this.radModeSimple.Text = "Simple";
            this.toolTip1.SetToolTip(this.radModeSimple, "One setting for all bands");
            this.radModeSimple.CheckedChanged += new System.EventHandler(this.radModeSimple_CheckedChanged);
            // 
            // radModeExpert
            // 
            this.radModeExpert.Image = null;
            this.radModeExpert.Location = new System.Drawing.Point(70, 24);
            this.radModeExpert.Name = "radModeExpert";
            this.radModeExpert.Size = new System.Drawing.Size(56, 24);
            this.radModeExpert.TabIndex = 20;
            this.radModeExpert.Text = "Expert";
            this.toolTip1.SetToolTip(this.radModeExpert, "More settings for each individual band");
            this.radModeExpert.CheckedChanged += new System.EventHandler(this.radModeExpert_CheckedChanged);
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
            this.comboBand.Location = new System.Drawing.Point(190, 15);
            this.comboBand.Name = "comboBand";
            this.comboBand.Size = new System.Drawing.Size(68, 21);
            this.comboBand.TabIndex = 18;
            this.toolTip1.SetToolTip(this.comboBand, "Shows Current RX1 Band (and Antenna selected below)\r\n\r\nIf you change this, it jus" +
        "t shows the Antenna for each band,\r\nbut does not change the RX1 band.");
            this.comboBand.Visible = false;
            this.comboBand.SelectedIndexChanged += new System.EventHandler(this.comboBand_SelectedIndexChanged);
            // 
            // comboBand2
            // 
            this.comboBand2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBand2.DropDownWidth = 56;
            this.comboBand2.Items.AddRange(new object[] {
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
            this.comboBand2.Location = new System.Drawing.Point(190, 42);
            this.comboBand2.Name = "comboBand2";
            this.comboBand2.Size = new System.Drawing.Size(68, 21);
            this.comboBand2.TabIndex = 18;
            this.toolTip1.SetToolTip(this.comboBand2, "Shows Current RX2 Band (and Antenna selected below)\r\n\r\nIf you change this, it jus" +
        "t shows the Antenna for each band,\r\nbut does not change the RX2 band.\r\n");
            this.comboBand2.Visible = false;
            this.comboBand2.SelectedIndexChanged += new System.EventHandler(this.comboBand_SelectedIndexChanged);
            // 
            // txtBoxAnt12
            // 
            this.txtBoxAnt12.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxAnt12.Location = new System.Drawing.Point(164, 29);
            this.txtBoxAnt12.MaxLength = 10;
            this.txtBoxAnt12.Name = "txtBoxAnt12";
            this.txtBoxAnt12.ShortcutsEnabled = false;
            this.txtBoxAnt12.Size = new System.Drawing.Size(29, 20);
            this.txtBoxAnt12.TabIndex = 38;
            this.txtBoxAnt12.Text = "PA";
            this.toolTip1.SetToolTip(this.txtBoxAnt12, "Assign a name to currently select RX1 antenna");
            this.txtBoxAnt12.Visible = false;
            this.txtBoxAnt12.WordWrap = false;
            // 
            // txtBoxAnt11
            // 
            this.txtBoxAnt11.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxAnt11.Location = new System.Drawing.Point(123, 29);
            this.txtBoxAnt11.MaxLength = 10;
            this.txtBoxAnt11.Name = "txtBoxAnt11";
            this.txtBoxAnt11.ShortcutsEnabled = false;
            this.txtBoxAnt11.Size = new System.Drawing.Size(29, 20);
            this.txtBoxAnt11.TabIndex = 37;
            this.txtBoxAnt11.Text = "XVTX/C";
            this.toolTip1.SetToolTip(this.txtBoxAnt11, "Assign a name to currently select RX1 antenna");
            this.txtBoxAnt11.Visible = false;
            this.txtBoxAnt11.WordWrap = false;
            // 
            // txtBoxAnt10
            // 
            this.txtBoxAnt10.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxAnt10.Location = new System.Drawing.Point(82, 33);
            this.txtBoxAnt10.MaxLength = 10;
            this.txtBoxAnt10.Name = "txtBoxAnt10";
            this.txtBoxAnt10.ShortcutsEnabled = false;
            this.txtBoxAnt10.Size = new System.Drawing.Size(29, 20);
            this.txtBoxAnt10.TabIndex = 36;
            this.txtBoxAnt10.Text = "XVTR";
            this.toolTip1.SetToolTip(this.txtBoxAnt10, "Assign a name to currently select RX1 antenna");
            this.txtBoxAnt10.Visible = false;
            this.txtBoxAnt10.WordWrap = false;
            // 
            // txtBoxAnt9
            // 
            this.txtBoxAnt9.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxAnt9.Location = new System.Drawing.Point(47, 29);
            this.txtBoxAnt9.MaxLength = 10;
            this.txtBoxAnt9.Name = "txtBoxAnt9";
            this.txtBoxAnt9.ShortcutsEnabled = false;
            this.txtBoxAnt9.Size = new System.Drawing.Size(29, 20);
            this.txtBoxAnt9.TabIndex = 35;
            this.txtBoxAnt9.Text = "UHF";
            this.toolTip1.SetToolTip(this.txtBoxAnt9, "Assign a name to currently select RX1 antenna");
            this.txtBoxAnt9.Visible = false;
            this.txtBoxAnt9.WordWrap = false;
            // 
            // txtBoxAnt8
            // 
            this.txtBoxAnt8.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxAnt8.Location = new System.Drawing.Point(12, 33);
            this.txtBoxAnt8.MaxLength = 10;
            this.txtBoxAnt8.Name = "txtBoxAnt8";
            this.txtBoxAnt8.ShortcutsEnabled = false;
            this.txtBoxAnt8.Size = new System.Drawing.Size(29, 20);
            this.txtBoxAnt8.TabIndex = 34;
            this.txtBoxAnt8.Text = "VHF";
            this.toolTip1.SetToolTip(this.txtBoxAnt8, "Assign a name to currently select RX1 antenna");
            this.txtBoxAnt8.Visible = false;
            this.txtBoxAnt8.WordWrap = false;
            // 
            // txtBoxAnt7
            // 
            this.txtBoxAnt7.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxAnt7.Location = new System.Drawing.Point(222, 2);
            this.txtBoxAnt7.MaxLength = 10;
            this.txtBoxAnt7.Name = "txtBoxAnt7";
            this.txtBoxAnt7.ShortcutsEnabled = false;
            this.txtBoxAnt7.Size = new System.Drawing.Size(29, 20);
            this.txtBoxAnt7.TabIndex = 33;
            this.txtBoxAnt7.Text = "Sig Gen";
            this.toolTip1.SetToolTip(this.txtBoxAnt7, "Assign a name to currently select RX1 antenna");
            this.txtBoxAnt7.Visible = false;
            this.txtBoxAnt7.WordWrap = false;
            // 
            // txtBoxAnt6
            // 
            this.txtBoxAnt6.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxAnt6.Location = new System.Drawing.Point(187, 2);
            this.txtBoxAnt6.MaxLength = 10;
            this.txtBoxAnt6.Name = "txtBoxAnt6";
            this.txtBoxAnt6.ShortcutsEnabled = false;
            this.txtBoxAnt6.Size = new System.Drawing.Size(29, 20);
            this.txtBoxAnt6.TabIndex = 32;
            this.txtBoxAnt6.Text = "TX1 Tap";
            this.toolTip1.SetToolTip(this.txtBoxAnt6, "Assign a name to currently select RX1 antenna");
            this.txtBoxAnt6.Visible = false;
            this.txtBoxAnt6.WordWrap = false;
            // 
            // txtBoxAnt5
            // 
            this.txtBoxAnt5.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxAnt5.Location = new System.Drawing.Point(152, 2);
            this.txtBoxAnt5.MaxLength = 10;
            this.txtBoxAnt5.Name = "txtBoxAnt5";
            this.txtBoxAnt5.ShortcutsEnabled = false;
            this.txtBoxAnt5.Size = new System.Drawing.Size(29, 20);
            this.txtBoxAnt5.TabIndex = 31;
            this.txtBoxAnt5.Text = "RX2 IN";
            this.toolTip1.SetToolTip(this.txtBoxAnt5, "Assign a name to currently select RX1 antenna");
            this.txtBoxAnt5.Visible = false;
            this.txtBoxAnt5.WordWrap = false;
            // 
            // txtBoxAnt4
            // 
            this.txtBoxAnt4.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxAnt4.Location = new System.Drawing.Point(117, 2);
            this.txtBoxAnt4.MaxLength = 10;
            this.txtBoxAnt4.Name = "txtBoxAnt4";
            this.txtBoxAnt4.ShortcutsEnabled = false;
            this.txtBoxAnt4.Size = new System.Drawing.Size(29, 20);
            this.txtBoxAnt4.TabIndex = 30;
            this.txtBoxAnt4.Text = "RX1 IN";
            this.toolTip1.SetToolTip(this.txtBoxAnt4, "Assign a name to currently select RX1 antenna");
            this.txtBoxAnt4.Visible = false;
            this.txtBoxAnt4.WordWrap = false;
            // 
            // txtBoxAnt3
            // 
            this.txtBoxAnt3.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxAnt3.Location = new System.Drawing.Point(82, 0);
            this.txtBoxAnt3.MaxLength = 10;
            this.txtBoxAnt3.Name = "txtBoxAnt3";
            this.txtBoxAnt3.ShortcutsEnabled = false;
            this.txtBoxAnt3.Size = new System.Drawing.Size(29, 20);
            this.txtBoxAnt3.TabIndex = 29;
            this.txtBoxAnt3.Text = "ANT3";
            this.toolTip1.SetToolTip(this.txtBoxAnt3, "Assign a name to currently select RX1 antenna");
            this.txtBoxAnt3.Visible = false;
            this.txtBoxAnt3.WordWrap = false;
            // 
            // txtBoxAnt2
            // 
            this.txtBoxAnt2.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxAnt2.Location = new System.Drawing.Point(47, 0);
            this.txtBoxAnt2.MaxLength = 10;
            this.txtBoxAnt2.Name = "txtBoxAnt2";
            this.txtBoxAnt2.ShortcutsEnabled = false;
            this.txtBoxAnt2.Size = new System.Drawing.Size(29, 20);
            this.txtBoxAnt2.TabIndex = 28;
            this.txtBoxAnt2.Text = "ANT2";
            this.toolTip1.SetToolTip(this.txtBoxAnt2, "Assign a name to currently select RX1 antenna");
            this.txtBoxAnt2.Visible = false;
            this.txtBoxAnt2.WordWrap = false;
            // 
            // txtBoxAnt1
            // 
            this.txtBoxAnt1.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxAnt1.Location = new System.Drawing.Point(12, 0);
            this.txtBoxAnt1.MaxLength = 10;
            this.txtBoxAnt1.Name = "txtBoxAnt1";
            this.txtBoxAnt1.ShortcutsEnabled = false;
            this.txtBoxAnt1.Size = new System.Drawing.Size(29, 20);
            this.txtBoxAnt1.TabIndex = 27;
            this.txtBoxAnt1.Text = "ANT1";
            this.toolTip1.SetToolTip(this.txtBoxAnt1, "Assign a name to currently select RX1 antenna");
            this.txtBoxAnt1.Visible = false;
            this.txtBoxAnt1.WordWrap = false;
            // 
            // textBoxTX2Ant
            // 
            this.textBoxTX2Ant.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxTX2Ant.Location = new System.Drawing.Point(199, 13);
            this.textBoxTX2Ant.MaxLength = 10;
            this.textBoxTX2Ant.Name = "textBoxTX2Ant";
            this.textBoxTX2Ant.ShortcutsEnabled = false;
            this.textBoxTX2Ant.Size = new System.Drawing.Size(60, 20);
            this.textBoxTX2Ant.TabIndex = 26;
            this.toolTip1.SetToolTip(this.textBoxTX2Ant, "Assign a name to currently select TX2 antenna\r\nRight Click to RESET back to DEFAU" +
        "LT names");
            this.textBoxTX2Ant.TextChanged += new System.EventHandler(this.textBoxTX2Ant_TextChanged);
            this.textBoxTX2Ant.Leave += new System.EventHandler(this.textBoxTX2Ant_Leave);
            this.textBoxTX2Ant.MouseUp += new System.Windows.Forms.MouseEventHandler(this.textBoxRX1Ant_MouseUp);
            // 
            // textBoxRX2Ant
            // 
            this.textBoxRX2Ant.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxRX2Ant.Location = new System.Drawing.Point(133, 13);
            this.textBoxRX2Ant.MaxLength = 10;
            this.textBoxRX2Ant.Name = "textBoxRX2Ant";
            this.textBoxRX2Ant.ShortcutsEnabled = false;
            this.textBoxRX2Ant.Size = new System.Drawing.Size(60, 20);
            this.textBoxRX2Ant.TabIndex = 25;
            this.toolTip1.SetToolTip(this.textBoxRX2Ant, "Assign a name to currently select RX2 antenna\r\nRight Click to RESET back to DEFAU" +
        "LT names");
            this.textBoxRX2Ant.TextChanged += new System.EventHandler(this.textBoxRX2Ant_TextChanged);
            this.textBoxRX2Ant.Leave += new System.EventHandler(this.textBoxRX2Ant_Leave);
            this.textBoxRX2Ant.MouseUp += new System.Windows.Forms.MouseEventHandler(this.textBoxRX1Ant_MouseUp);
            // 
            // textBoxTX1Ant
            // 
            this.textBoxTX1Ant.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxTX1Ant.Location = new System.Drawing.Point(69, 13);
            this.textBoxTX1Ant.MaxLength = 10;
            this.textBoxTX1Ant.Name = "textBoxTX1Ant";
            this.textBoxTX1Ant.ShortcutsEnabled = false;
            this.textBoxTX1Ant.Size = new System.Drawing.Size(60, 20);
            this.textBoxTX1Ant.TabIndex = 24;
            this.toolTip1.SetToolTip(this.textBoxTX1Ant, "Assign a name to currently select TX1 antenna\r\nRight Click to RESET back to DEFAU" +
        "LT names");
            this.textBoxTX1Ant.TextChanged += new System.EventHandler(this.textBoxTX1Ant_TextChanged);
            this.textBoxTX1Ant.Leave += new System.EventHandler(this.textBoxTX1Ant_Leave);
            this.textBoxTX1Ant.MouseUp += new System.Windows.Forms.MouseEventHandler(this.textBoxRX1Ant_MouseUp);
            // 
            // textBoxRX1Ant
            // 
            this.textBoxRX1Ant.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxRX1Ant.Location = new System.Drawing.Point(3, 13);
            this.textBoxRX1Ant.MaxLength = 10;
            this.textBoxRX1Ant.Name = "textBoxRX1Ant";
            this.textBoxRX1Ant.ShortcutsEnabled = false;
            this.textBoxRX1Ant.Size = new System.Drawing.Size(60, 20);
            this.textBoxRX1Ant.TabIndex = 23;
            this.toolTip1.SetToolTip(this.textBoxRX1Ant, "Assign a name to currently select RX1 antenna\r\nRight Click to RESET back to DEFAU" +
        "LT names");
            this.textBoxRX1Ant.WordWrap = false;
            this.textBoxRX1Ant.TextChanged += new System.EventHandler(this.textBoxRX1Ant_TextChanged);
            this.textBoxRX1Ant.Leave += new System.EventHandler(this.textBoxRX1Ant_Leave);
            this.textBoxRX1Ant.MouseUp += new System.Windows.Forms.MouseEventHandler(this.textBoxRX1Ant_MouseUp);
            // 
            // chkTX2Active
            // 
            this.chkTX2Active.Image = null;
            this.chkTX2Active.Location = new System.Drawing.Point(199, 73);
            this.chkTX2Active.Name = "chkTX2Active";
            this.chkTX2Active.Size = new System.Drawing.Size(63, 24);
            this.chkTX2Active.TabIndex = 22;
            this.chkTX2Active.Text = "SO2R";
            this.toolTip1.SetToolTip(this.chkTX2Active, "Check this box to activate Seperate Transmit Antenna selection for RX2 to fully e" +
        "nable SO2R\r\n\r\nYou must have RX2 and HRFIO Rev34c board installed");
            this.chkTX2Active.CheckedChanged += new System.EventHandler(this.chkTX2Active_CheckedChanged);
            // 
            // chkLock
            // 
            this.chkLock.Checked = true;
            this.chkLock.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkLock.Image = null;
            this.chkLock.Location = new System.Drawing.Point(143, 73);
            this.chkLock.Name = "chkLock";
            this.chkLock.Size = new System.Drawing.Size(50, 24);
            this.chkLock.TabIndex = 16;
            this.chkLock.Text = "Lock";
            this.toolTip1.SetToolTip(this.chkLock, "Check this box to lock RX1 and TX antenna selections.");
            this.chkLock.CheckedChanged += new System.EventHandler(this.chkLock_CheckedChanged);
            // 
            // comboTXAnt2
            // 
            this.comboTXAnt2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboTXAnt2.DropDownWidth = 64;
            this.comboTXAnt2.Items.AddRange(new object[] {
            "ANT 1",
            "ANT 2",
            "ANT 3"});
            this.comboTXAnt2.Location = new System.Drawing.Point(199, 49);
            this.comboTXAnt2.Name = "comboTXAnt2";
            this.comboTXAnt2.Size = new System.Drawing.Size(60, 21);
            this.comboTXAnt2.TabIndex = 20;
            this.toolTip1.SetToolTip(this.comboTXAnt2, "Selects the Transmitter 2 Antenna\r\nOnly for Flex-5000 units with 2nd receiver");
            this.comboTXAnt2.SelectedIndexChanged += new System.EventHandler(this.comboTXAnt2_SelectedIndexChanged);
            // 
            // chkEnable6mPreamp
            // 
            this.chkEnable6mPreamp.Image = null;
            this.chkEnable6mPreamp.Location = new System.Drawing.Point(7, 73);
            this.chkEnable6mPreamp.Name = "chkEnable6mPreamp";
            this.chkEnable6mPreamp.Size = new System.Drawing.Size(144, 24);
            this.chkEnable6mPreamp.TabIndex = 17;
            this.chkEnable6mPreamp.Text = "Preamp controls on 6m";
            this.toolTip1.SetToolTip(this.chkEnable6mPreamp, "Check this box to route the main receiver\'s RF path out RX1 Out and back in RX1 I" +
        "n.  For use with external preamps/filters/etc.");
            this.chkEnable6mPreamp.CheckedChanged += new System.EventHandler(this.chkEnable6mPreamp_CheckedChanged);
            // 
            // comboRX1Ant
            // 
            this.comboRX1Ant.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboRX1Ant.DropDownWidth = 64;
            this.comboRX1Ant.Items.AddRange(new object[] {
            "N/C",
            "ANT 1",
            "ANT 2",
            "ANT 3",
            "RX1 IN"});
            this.comboRX1Ant.Location = new System.Drawing.Point(3, 49);
            this.comboRX1Ant.Name = "comboRX1Ant";
            this.comboRX1Ant.Size = new System.Drawing.Size(60, 21);
            this.comboRX1Ant.TabIndex = 10;
            this.toolTip1.SetToolTip(this.comboRX1Ant, "Selects the Main Receiver Antenna");
            this.comboRX1Ant.SelectedIndexChanged += new System.EventHandler(this.comboRX1Ant_SelectedIndexChanged);
            // 
            // chkRX1Loop
            // 
            this.chkRX1Loop.Image = null;
            this.chkRX1Loop.Location = new System.Drawing.Point(7, 97);
            this.chkRX1Loop.Name = "chkRX1Loop";
            this.chkRX1Loop.Size = new System.Drawing.Size(168, 16);
            this.chkRX1Loop.TabIndex = 9;
            this.chkRX1Loop.Text = "Use RX1 Out to RX1 In Loop";
            this.toolTip1.SetToolTip(this.chkRX1Loop, "Check this box to route the main receiver\'s RF path out RX1 Out and back in RX1 I" +
        "n.  For use with external preamps/filters/etc.");
            this.chkRX1Loop.CheckedChanged += new System.EventHandler(this.chkRX1ExtAnt_CheckedChanged);
            // 
            // comboRX2Ant
            // 
            this.comboRX2Ant.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboRX2Ant.DropDownWidth = 72;
            this.comboRX2Ant.Items.AddRange(new object[] {
            "N/C",
            "ANT 1",
            "RX2 IN",
            "RX1 Tap"});
            this.comboRX2Ant.Location = new System.Drawing.Point(133, 49);
            this.comboRX2Ant.Name = "comboRX2Ant";
            this.comboRX2Ant.Size = new System.Drawing.Size(60, 21);
            this.comboRX2Ant.TabIndex = 14;
            this.toolTip1.SetToolTip(this.comboRX2Ant, resources.GetString("comboRX2Ant.ToolTip"));
            this.comboRX2Ant.SelectedIndexChanged += new System.EventHandler(this.comboRX2Ant_SelectedIndexChanged);
            // 
            // comboTXAnt
            // 
            this.comboTXAnt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboTXAnt.DropDownWidth = 64;
            this.comboTXAnt.Items.AddRange(new object[] {
            "ANT 1",
            "ANT 2",
            "ANT 3"});
            this.comboTXAnt.Location = new System.Drawing.Point(69, 49);
            this.comboTXAnt.Name = "comboTXAnt";
            this.comboTXAnt.Size = new System.Drawing.Size(60, 21);
            this.comboTXAnt.TabIndex = 12;
            this.toolTip1.SetToolTip(this.comboTXAnt, "Selects the Transmitter Antenna for RX1");
            this.comboTXAnt.SelectedIndexChanged += new System.EventHandler(this.comboTXAnt_SelectedIndexChanged);
            // 
            // chkRX2TX3
            // 
            this.chkRX2TX3.Image = null;
            this.chkRX2TX3.Location = new System.Drawing.Point(176, 13);
            this.chkRX2TX3.Name = "chkRX2TX3";
            this.chkRX2TX3.Size = new System.Drawing.Size(72, 22);
            this.chkRX2TX3.TabIndex = 27;
            this.chkRX2TX3.Text = "RX2-TX3";
            this.toolTip1.SetToolTip(this.chkRX2TX3, "Checked = Transmitter 2 (RX2 transmit)\r\nunChecked = Transmitter 1 (RX1 transmit)\r" +
        "\n\r\n");
            this.chkRX2TX3.CheckedChanged += new System.EventHandler(this.chkRX1TX3_CheckedChanged);
            // 
            // chkRX2TX2
            // 
            this.chkRX2TX2.Image = null;
            this.chkRX2TX2.Location = new System.Drawing.Point(96, 13);
            this.chkRX2TX2.Name = "chkRX2TX2";
            this.chkRX2TX2.Size = new System.Drawing.Size(72, 22);
            this.chkRX2TX2.TabIndex = 26;
            this.chkRX2TX2.Text = "RX2-TX2";
            this.toolTip1.SetToolTip(this.chkRX2TX2, "Checked = Transmitter 2 (RX2 transmit)\r\nunChecked = Transmitter 1 (RX1 transmit)\r" +
        "\n\r\n");
            this.chkRX2TX2.CheckedChanged += new System.EventHandler(this.chkRX1TX2_CheckedChanged);
            // 
            // textBoxTS1
            // 
            this.textBoxTS1.Location = new System.Drawing.Point(12, 116);
            this.textBoxTS1.Name = "textBoxTS1";
            this.textBoxTS1.ReadOnly = true;
            this.textBoxTS1.Size = new System.Drawing.Size(228, 20);
            this.textBoxTS1.TabIndex = 24;
            this.textBoxTS1.Text = "Hit F1 for Help on TR Delay Sequencing";
            this.toolTip1.SetToolTip(this.textBoxTS1, resources.GetString("textBoxTS1.ToolTip"));
            this.textBoxTS1.Click += new System.EventHandler(this.textBoxTS1_Click);
            // 
            // udTX3Delay
            // 
            this.udTX3Delay.Enabled = false;
            this.udTX3Delay.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udTX3Delay.Location = new System.Drawing.Point(176, 88);
            this.udTX3Delay.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.udTX3Delay.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.udTX3Delay.Name = "udTX3Delay";
            this.udTX3Delay.Size = new System.Drawing.Size(56, 20);
            this.udTX3Delay.TabIndex = 8;
            this.toolTip1.SetToolTip(this.udTX3Delay, resources.GetString("udTX3Delay.ToolTip"));
            this.udTX3Delay.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udTX3Delay.ValueChanged += new System.EventHandler(this.udTX3Delay_ValueChanged);
            // 
            // chkTX3DelayEnable
            // 
            this.chkTX3DelayEnable.Image = null;
            this.chkTX3DelayEnable.Location = new System.Drawing.Point(176, 60);
            this.chkTX3DelayEnable.Name = "chkTX3DelayEnable";
            this.chkTX3DelayEnable.Size = new System.Drawing.Size(80, 32);
            this.chkTX3DelayEnable.TabIndex = 7;
            this.chkTX3DelayEnable.Text = "Delay (ms)";
            this.toolTip1.SetToolTip(this.chkTX3DelayEnable, resources.GetString("chkTX3DelayEnable.ToolTip"));
            this.chkTX3DelayEnable.CheckedChanged += new System.EventHandler(this.chkTX3DelayEnable_CheckedChanged);
            // 
            // udTX2Delay
            // 
            this.udTX2Delay.Enabled = false;
            this.udTX2Delay.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udTX2Delay.Location = new System.Drawing.Point(96, 88);
            this.udTX2Delay.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.udTX2Delay.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.udTX2Delay.Name = "udTX2Delay";
            this.udTX2Delay.Size = new System.Drawing.Size(56, 20);
            this.udTX2Delay.TabIndex = 6;
            this.toolTip1.SetToolTip(this.udTX2Delay, resources.GetString("udTX2Delay.ToolTip"));
            this.udTX2Delay.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udTX2Delay.ValueChanged += new System.EventHandler(this.udTX2Delay_ValueChanged);
            // 
            // chkTX2DelayEnable
            // 
            this.chkTX2DelayEnable.Image = null;
            this.chkTX2DelayEnable.Location = new System.Drawing.Point(96, 60);
            this.chkTX2DelayEnable.Name = "chkTX2DelayEnable";
            this.chkTX2DelayEnable.Size = new System.Drawing.Size(80, 32);
            this.chkTX2DelayEnable.TabIndex = 5;
            this.chkTX2DelayEnable.Text = "Delay (ms)";
            this.toolTip1.SetToolTip(this.chkTX2DelayEnable, resources.GetString("chkTX2DelayEnable.ToolTip"));
            this.chkTX2DelayEnable.CheckedChanged += new System.EventHandler(this.chkTX2DelayEnable_CheckedChanged);
            // 
            // udTX1Delay
            // 
            this.udTX1Delay.Enabled = false;
            this.udTX1Delay.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udTX1Delay.Location = new System.Drawing.Point(16, 88);
            this.udTX1Delay.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.udTX1Delay.Minimum = new decimal(new int[] {
            0,
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
            this.chkTX1DelayEnable.Location = new System.Drawing.Point(16, 60);
            this.chkTX1DelayEnable.Name = "chkTX1DelayEnable";
            this.chkTX1DelayEnable.Size = new System.Drawing.Size(82, 32);
            this.chkTX1DelayEnable.TabIndex = 3;
            this.chkTX1DelayEnable.Text = "Delay (ms)";
            this.toolTip1.SetToolTip(this.chkTX1DelayEnable, resources.GetString("chkTX1DelayEnable.ToolTip"));
            this.chkTX1DelayEnable.CheckedChanged += new System.EventHandler(this.chkTX1DelayEnable_CheckedChanged);
            // 
            // chkRCATX3
            // 
            this.chkRCATX3.Checked = true;
            this.chkRCATX3.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRCATX3.Image = null;
            this.chkRCATX3.Location = new System.Drawing.Point(176, 32);
            this.chkRCATX3.Name = "chkRCATX3";
            this.chkRCATX3.Size = new System.Drawing.Size(64, 32);
            this.chkRCATX3.TabIndex = 2;
            this.chkRCATX3.Text = "TX3 (Yellow)";
            this.toolTip1.SetToolTip(this.chkRCATX3, resources.GetString("chkRCATX3.ToolTip"));
            this.chkRCATX3.CheckedChanged += new System.EventHandler(this.chkRCATX3_CheckedChanged);
            // 
            // chkRCATX2
            // 
            this.chkRCATX2.Checked = true;
            this.chkRCATX2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRCATX2.Image = null;
            this.chkRCATX2.Location = new System.Drawing.Point(96, 32);
            this.chkRCATX2.Name = "chkRCATX2";
            this.chkRCATX2.Size = new System.Drawing.Size(80, 32);
            this.chkRCATX2.TabIndex = 1;
            this.chkRCATX2.Text = "TX2 (White)";
            this.toolTip1.SetToolTip(this.chkRCATX2, resources.GetString("chkRCATX2.ToolTip"));
            this.chkRCATX2.CheckedChanged += new System.EventHandler(this.chkRCATX2_CheckedChanged);
            // 
            // chkRCATX1
            // 
            this.chkRCATX1.Checked = true;
            this.chkRCATX1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRCATX1.Image = null;
            this.chkRCATX1.Location = new System.Drawing.Point(16, 32);
            this.chkRCATX1.Name = "chkRCATX1";
            this.chkRCATX1.Size = new System.Drawing.Size(72, 32);
            this.chkRCATX1.TabIndex = 0;
            this.chkRCATX1.Text = "TX1 (Red)";
            this.toolTip1.SetToolTip(this.chkRCATX1, resources.GetString("chkRCATX1.ToolTip"));
            this.chkRCATX1.CheckedChanged += new System.EventHandler(this.chkRCATX1_CheckedChanged);
            // 
            // chkRX2TX1
            // 
            this.chkRX2TX1.Image = null;
            this.chkRX2TX1.Location = new System.Drawing.Point(16, 13);
            this.chkRX2TX1.Name = "chkRX2TX1";
            this.chkRX2TX1.Size = new System.Drawing.Size(72, 22);
            this.chkRX2TX1.TabIndex = 25;
            this.chkRX2TX1.Text = "RX2-TX1";
            this.toolTip1.SetToolTip(this.chkRX2TX1, "Checked = Transmitter 2 (RX2 transmit)\r\nunChecked = Transmitter 1 (RX1 transmit)\r" +
        "\n\r\n");
            this.chkRX2TX1.CheckedChanged += new System.EventHandler(this.chkRX1TX1_CheckedChanged);
            // 
            // chkAlwaysOnTop1
            // 
            this.chkAlwaysOnTop1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.chkAlwaysOnTop1.Image = null;
            this.chkAlwaysOnTop1.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkAlwaysOnTop1.Location = new System.Drawing.Point(169, 385);
            this.chkAlwaysOnTop1.Name = "chkAlwaysOnTop1";
            this.chkAlwaysOnTop1.Size = new System.Drawing.Size(103, 24);
            this.chkAlwaysOnTop1.TabIndex = 60;
            this.chkAlwaysOnTop1.Text = "Always On Top";
            this.chkAlwaysOnTop1.CheckedChanged += new System.EventHandler(this.ChkAlwaysOnTop1_CheckedChanged);
            // 
            // txtStatus
            // 
            this.txtStatus.Location = new System.Drawing.Point(8, 360);
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.ReadOnly = true;
            this.txtStatus.Size = new System.Drawing.Size(264, 20);
            this.txtStatus.TabIndex = 23;
            this.txtStatus.Text = "Simple Mode: Settings are applied to all bands";
            // 
            // grpComplexity
            // 
            this.grpComplexity.Controls.Add(this.lblBand2);
            this.grpComplexity.Controls.Add(this.radModeSimple);
            this.grpComplexity.Controls.Add(this.radModeExpert);
            this.grpComplexity.Controls.Add(this.comboBand);
            this.grpComplexity.Controls.Add(this.comboBand2);
            this.grpComplexity.Controls.Add(this.lblBand);
            this.grpComplexity.Location = new System.Drawing.Point(8, 6);
            this.grpComplexity.Name = "grpComplexity";
            this.grpComplexity.Size = new System.Drawing.Size(264, 72);
            this.grpComplexity.TabIndex = 21;
            this.grpComplexity.TabStop = false;
            this.grpComplexity.Text = "Complexity";
            // 
            // lblBand2
            // 
            this.lblBand2.Image = null;
            this.lblBand2.Location = new System.Drawing.Point(132, 40);
            this.lblBand2.Name = "lblBand2";
            this.lblBand2.Size = new System.Drawing.Size(57, 24);
            this.lblBand2.TabIndex = 21;
            this.lblBand2.Text = "RX2Band:";
            this.lblBand2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblBand2.Visible = false;
            // 
            // lblBand
            // 
            this.lblBand.Image = null;
            this.lblBand.Location = new System.Drawing.Point(132, 14);
            this.lblBand.Name = "lblBand";
            this.lblBand.Size = new System.Drawing.Size(59, 24);
            this.lblBand.TabIndex = 19;
            this.lblBand.Text = "RX1Band:";
            this.lblBand.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblBand.Visible = false;
            // 
            // grpAntenna
            // 
            this.grpAntenna.Controls.Add(this.txtBoxAnt12);
            this.grpAntenna.Controls.Add(this.txtBoxAnt11);
            this.grpAntenna.Controls.Add(this.txtBoxAnt10);
            this.grpAntenna.Controls.Add(this.txtBoxAnt9);
            this.grpAntenna.Controls.Add(this.txtBoxAnt8);
            this.grpAntenna.Controls.Add(this.txtBoxAnt7);
            this.grpAntenna.Controls.Add(this.txtBoxAnt6);
            this.grpAntenna.Controls.Add(this.txtBoxAnt5);
            this.grpAntenna.Controls.Add(this.txtBoxAnt4);
            this.grpAntenna.Controls.Add(this.txtBoxAnt3);
            this.grpAntenna.Controls.Add(this.txtBoxAnt2);
            this.grpAntenna.Controls.Add(this.txtBoxAnt1);
            this.grpAntenna.Controls.Add(this.textBoxTX2Ant);
            this.grpAntenna.Controls.Add(this.textBoxRX2Ant);
            this.grpAntenna.Controls.Add(this.textBoxTX1Ant);
            this.grpAntenna.Controls.Add(this.textBoxRX1Ant);
            this.grpAntenna.Controls.Add(this.chkTX2Active);
            this.grpAntenna.Controls.Add(this.chkLock);
            this.grpAntenna.Controls.Add(this.comboTXAnt2);
            this.grpAntenna.Controls.Add(this.lblLoopGain);
            this.grpAntenna.Controls.Add(this.udLoopGain);
            this.grpAntenna.Controls.Add(this.chkEnable6mPreamp);
            this.grpAntenna.Controls.Add(this.comboRX1Ant);
            this.grpAntenna.Controls.Add(this.chkRX1Loop);
            this.grpAntenna.Controls.Add(this.comboRX2Ant);
            this.grpAntenna.Controls.Add(this.lblRX2);
            this.grpAntenna.Controls.Add(this.comboTXAnt);
            this.grpAntenna.Controls.Add(this.lblTX);
            this.grpAntenna.Controls.Add(this.lblRX1);
            this.grpAntenna.Controls.Add(this.labelTS1);
            this.grpAntenna.Location = new System.Drawing.Point(8, 82);
            this.grpAntenna.Name = "grpAntenna";
            this.grpAntenna.Size = new System.Drawing.Size(264, 124);
            this.grpAntenna.TabIndex = 20;
            this.grpAntenna.TabStop = false;
            this.grpAntenna.Text = "Antenna";
            // 
            // lblLoopGain
            // 
            this.lblLoopGain.Image = null;
            this.lblLoopGain.Location = new System.Drawing.Point(175, 97);
            this.lblLoopGain.Name = "lblLoopGain";
            this.lblLoopGain.Size = new System.Drawing.Size(32, 16);
            this.lblLoopGain.TabIndex = 19;
            this.lblLoopGain.Text = "Gain:";
            this.lblLoopGain.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // udLoopGain
            // 
            this.udLoopGain.DecimalPlaces = 1;
            this.udLoopGain.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.udLoopGain.Location = new System.Drawing.Point(207, 97);
            this.udLoopGain.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.udLoopGain.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.udLoopGain.Name = "udLoopGain";
            this.udLoopGain.Size = new System.Drawing.Size(48, 20);
            this.udLoopGain.TabIndex = 18;
            this.udLoopGain.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.udLoopGain.ValueChanged += new System.EventHandler(this.udLoopGain_ValueChanged);
            // 
            // lblRX2
            // 
            this.lblRX2.Image = null;
            this.lblRX2.Location = new System.Drawing.Point(131, 33);
            this.lblRX2.Name = "lblRX2";
            this.lblRX2.Size = new System.Drawing.Size(62, 16);
            this.lblRX2.TabIndex = 15;
            this.lblRX2.Text = "Receiver 2:";
            // 
            // lblTX
            // 
            this.lblTX.Image = null;
            this.lblTX.Location = new System.Drawing.Point(66, 33);
            this.lblTX.Name = "lblTX";
            this.lblTX.Size = new System.Drawing.Size(64, 16);
            this.lblTX.TabIndex = 13;
            this.lblTX.Text = "Transmit 1:";
            // 
            // lblRX1
            // 
            this.lblRX1.Image = null;
            this.lblRX1.Location = new System.Drawing.Point(3, 33);
            this.lblRX1.Name = "lblRX1";
            this.lblRX1.Size = new System.Drawing.Size(72, 16);
            this.lblRX1.TabIndex = 11;
            this.lblRX1.Text = "Receiver 1:";
            // 
            // labelTS1
            // 
            this.labelTS1.Image = null;
            this.labelTS1.Location = new System.Drawing.Point(195, 33);
            this.labelTS1.Name = "labelTS1";
            this.labelTS1.Size = new System.Drawing.Size(64, 16);
            this.labelTS1.TabIndex = 21;
            this.labelTS1.Text = "Transmit 2:";
            // 
            // grpSwitchRelay
            // 
            this.grpSwitchRelay.Controls.Add(this.chkRX2TX3);
            this.grpSwitchRelay.Controls.Add(this.chkRX2TX2);
            this.grpSwitchRelay.Controls.Add(this.textBoxTS1);
            this.grpSwitchRelay.Controls.Add(this.udTX3Delay);
            this.grpSwitchRelay.Controls.Add(this.chkTX3DelayEnable);
            this.grpSwitchRelay.Controls.Add(this.udTX2Delay);
            this.grpSwitchRelay.Controls.Add(this.chkTX2DelayEnable);
            this.grpSwitchRelay.Controls.Add(this.udTX1Delay);
            this.grpSwitchRelay.Controls.Add(this.chkTX1DelayEnable);
            this.grpSwitchRelay.Controls.Add(this.chkRCATX3);
            this.grpSwitchRelay.Controls.Add(this.chkRCATX2);
            this.grpSwitchRelay.Controls.Add(this.chkRCATX1);
            this.grpSwitchRelay.Controls.Add(this.chkRX2TX1);
            this.grpSwitchRelay.Location = new System.Drawing.Point(8, 212);
            this.grpSwitchRelay.Name = "grpSwitchRelay";
            this.grpSwitchRelay.Size = new System.Drawing.Size(264, 142);
            this.grpSwitchRelay.TabIndex = 21;
            this.grpSwitchRelay.TabStop = false;
            this.grpSwitchRelay.Text = "Switch Relay with TR";
            this.grpSwitchRelay.Visible = false;
            // 
            // FWCAntForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(283, 414);
            this.Controls.Add(this.chkAlwaysOnTop1);
            this.Controls.Add(this.txtStatus);
            this.Controls.Add(this.grpComplexity);
            this.Controls.Add(this.grpAntenna);
            this.Controls.Add(this.grpSwitchRelay);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximumSize = new System.Drawing.Size(299, 453);
            this.MinimumSize = new System.Drawing.Size(299, 453);
            this.Name = "FWCAntForm";
            this.Text = "FLEX-5000 Antenna Selection";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.FWCAntForm_Closing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FWCAntForm_KeyDown);
            this.MouseEnter += new System.EventHandler(this.FWCAntForm_MouseEnter);
            ((System.ComponentModel.ISupportInitialize)(this.udTX3Delay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udTX2Delay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udTX1Delay)).EndInit();
            this.grpComplexity.ResumeLayout(false);
            this.grpAntenna.ResumeLayout(false);
            this.grpAntenna.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udLoopGain)).EndInit();
            this.grpSwitchRelay.ResumeLayout(false);
            this.grpSwitchRelay.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
        private System.Windows.Forms.LabelTS lblRX2;
        private System.Windows.Forms.RadioButtonTS radModeSimple;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.LabelTS lblBand;
        private System.Windows.Forms.ComboBoxTS comboBand;
        public System.Windows.Forms.ComboBoxTS comboBand2;
        public System.Windows.Forms.RadioButtonTS radModeExpert;
        private System.Windows.Forms.LabelTS lblRX1;
        private System.Windows.Forms.GroupBoxTS grpComplexity;
        private System.Windows.Forms.GroupBoxTS grpAntenna;
        private System.Windows.Forms.GroupBoxTS grpSwitchRelay;
        private System.Windows.Forms.CheckBoxTS chkRCATX1;
        private System.Windows.Forms.CheckBoxTS chkRCATX2;
        private System.Windows.Forms.CheckBoxTS chkRCATX3;
        private System.Windows.Forms.TextBoxTS txtStatus;
        private System.Windows.Forms.CheckBoxTS chkLock;
        private System.Windows.Forms.CheckBoxTS chkRX1Loop;
        private System.Windows.Forms.CheckBoxTS chkTX1DelayEnable;
        private System.Windows.Forms.NumericUpDownTS udTX1Delay;
        private System.Windows.Forms.NumericUpDownTS udTX2Delay;
        private System.Windows.Forms.CheckBoxTS chkTX2DelayEnable;
        private System.Windows.Forms.NumericUpDownTS udTX3Delay;
        private System.Windows.Forms.CheckBoxTS chkTX3DelayEnable;
        private System.Windows.Forms.CheckBoxTS chkEnable6mPreamp;
        private System.Windows.Forms.NumericUpDownTS udLoopGain;
        private System.Windows.Forms.LabelTS lblLoopGain;
        private System.Windows.Forms.TextBoxTS textBoxTS1;
        public System.Windows.Forms.CheckBoxTS chkAlwaysOnTop1;
        public System.Windows.Forms.LabelTS lblBand2;
        private System.Windows.Forms.LabelTS labelTS1;
        public System.Windows.Forms.ComboBoxTS comboTXAnt2;
        public System.Windows.Forms.CheckBoxTS chkTX2Active;
        public System.Windows.Forms.CheckBoxTS chkRX2TX3;
        public System.Windows.Forms.CheckBoxTS chkRX2TX2;
        public System.Windows.Forms.CheckBoxTS chkRX2TX1;
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.TextBoxTS textBoxTX2Ant;
        private System.Windows.Forms.TextBoxTS textBoxRX2Ant;
        private System.Windows.Forms.TextBoxTS textBoxTX1Ant;
        private System.Windows.Forms.TextBoxTS textBoxRX1Ant;
        public System.Windows.Forms.ComboBoxTS comboRX1Ant;
        public System.Windows.Forms.LabelTS lblTX;
        public System.Windows.Forms.ComboBoxTS comboTXAnt;
        public System.Windows.Forms.ComboBoxTS comboRX2Ant;
        public System.Windows.Forms.TextBoxTS txtBoxAnt12;
        public System.Windows.Forms.TextBoxTS txtBoxAnt11;
        public System.Windows.Forms.TextBoxTS txtBoxAnt10;
        public System.Windows.Forms.TextBoxTS txtBoxAnt9;
        public System.Windows.Forms.TextBoxTS txtBoxAnt8;
        public System.Windows.Forms.TextBoxTS txtBoxAnt7;
        public System.Windows.Forms.TextBoxTS txtBoxAnt6;
        public System.Windows.Forms.TextBoxTS txtBoxAnt5;
        public System.Windows.Forms.TextBoxTS txtBoxAnt4;
        public System.Windows.Forms.TextBoxTS txtBoxAnt3;
        public System.Windows.Forms.TextBoxTS txtBoxAnt2;
        public System.Windows.Forms.TextBoxTS txtBoxAnt1;
    }
}
