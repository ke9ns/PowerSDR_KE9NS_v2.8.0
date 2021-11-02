//=================================================================
// FLEX5000FinalTestForm.cs
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
    public partial class FLEX5000FinalTestForm : System.Windows.Forms.Form
    {
        

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FLEX5000FinalTestForm));
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.comboCOMPort = new System.Windows.Forms.ComboBoxTS();
            this.printPreviewDialog1 = new System.Windows.Forms.PrintPreviewDialog();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.udBiasDriverTarget = new System.Windows.Forms.NumericUpDown();
            this.udBiasFinalTarget = new System.Windows.Forms.NumericUpDown();
            this.lstDebug = new System.Windows.Forms.ListBox();
            this.grpATU = new System.Windows.Forms.GroupBoxTS();
            this.btnATUSWR = new System.Windows.Forms.ButtonTS();
            this.grpBridgeNull = new System.Windows.Forms.GroupBoxTS();
            this.picNullBridge = new System.Windows.Forms.PictureBox();
            this.grpIO = new System.Windows.Forms.GroupBoxTS();
            this.btnIORunAll = new System.Windows.Forms.ButtonTS();
            this.btnIORX1InOut = new System.Windows.Forms.ButtonTS();
            this.btnIOXVRX = new System.Windows.Forms.ButtonTS();
            this.btnIOTXMon = new System.Windows.Forms.ButtonTS();
            this.grpPA = new System.Windows.Forms.GroupBoxTS();
            this.btnPAVerify = new System.Windows.Forms.ButtonTS();
            this.btnPASWR = new System.Windows.Forms.ButtonTS();
            this.btnNullBridge = new System.Windows.Forms.ButtonTS();
            this.btnPABias = new System.Windows.Forms.ButtonTS();
            this.btnPAPower = new System.Windows.Forms.ButtonTS();
            this.btnRunPACal = new System.Windows.Forms.ButtonTS();
            this.btnPrintReport = new System.Windows.Forms.ButtonTS();
            this.txtTech = new System.Windows.Forms.TextBoxTS();
            this.lblTech = new System.Windows.Forms.LabelTS();
            this.grpComPort = new System.Windows.Forms.GroupBoxTS();
            this.grpBands = new System.Windows.Forms.GroupBoxTS();
            this.ck6 = new System.Windows.Forms.CheckBoxTS();
            this.ck10 = new System.Windows.Forms.CheckBoxTS();
            this.ck12 = new System.Windows.Forms.CheckBoxTS();
            this.ck15 = new System.Windows.Forms.CheckBoxTS();
            this.ck17 = new System.Windows.Forms.CheckBoxTS();
            this.ck20 = new System.Windows.Forms.CheckBoxTS();
            this.ck30 = new System.Windows.Forms.CheckBoxTS();
            this.ck40 = new System.Windows.Forms.CheckBoxTS();
            this.ck60 = new System.Windows.Forms.CheckBoxTS();
            this.ck80 = new System.Windows.Forms.CheckBoxTS();
            this.ck160 = new System.Windows.Forms.CheckBoxTS();
            this.btnClearAll = new System.Windows.Forms.ButtonTS();
            this.btnCheckAll = new System.Windows.Forms.ButtonTS();
            this.btnCheckEEPROM = new System.Windows.Forms.ButtonTS();
            this.ckPM2 = new System.Windows.Forms.CheckBoxTS();
            ((System.ComponentModel.ISupportInitialize)(this.udBiasDriverTarget)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udBiasFinalTarget)).BeginInit();
            this.grpATU.SuspendLayout();
            this.grpBridgeNull.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picNullBridge)).BeginInit();
            this.grpIO.SuspendLayout();
            this.grpPA.SuspendLayout();
            this.grpComPort.SuspendLayout();
            this.grpBands.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboCOMPort
            // 
            this.comboCOMPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboCOMPort.DropDownWidth = 72;
            this.comboCOMPort.Location = new System.Drawing.Point(16, 16);
            this.comboCOMPort.Name = "comboCOMPort";
            this.comboCOMPort.Size = new System.Drawing.Size(72, 21);
            this.comboCOMPort.TabIndex = 0;
            this.toolTip1.SetToolTip(this.comboCOMPort, "COM Port Power Master Wattmeter is connected to");
            // 
            // printPreviewDialog1
            // 
            this.printPreviewDialog1.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.ClientSize = new System.Drawing.Size(400, 300);
            this.printPreviewDialog1.Enabled = true;
            this.printPreviewDialog1.Icon = ((System.Drawing.Icon)(resources.GetObject("printPreviewDialog1.Icon")));
            this.printPreviewDialog1.Name = "printPreviewDialog1";
            this.printPreviewDialog1.Visible = false;
            // 
            // printDocument1
            // 
            this.printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument1_PrintPage);
            // 
            // udBiasDriverTarget
            // 
            this.udBiasDriverTarget.DecimalPlaces = 3;
            this.udBiasDriverTarget.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.udBiasDriverTarget.Location = new System.Drawing.Point(384, 336);
            this.udBiasDriverTarget.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.udBiasDriverTarget.Name = "udBiasDriverTarget";
            this.udBiasDriverTarget.Size = new System.Drawing.Size(56, 20);
            this.udBiasDriverTarget.TabIndex = 27;
            this.udBiasDriverTarget.Value = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            this.udBiasDriverTarget.Visible = false;
            // 
            // udBiasFinalTarget
            // 
            this.udBiasFinalTarget.DecimalPlaces = 3;
            this.udBiasFinalTarget.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.udBiasFinalTarget.Location = new System.Drawing.Point(384, 360);
            this.udBiasFinalTarget.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.udBiasFinalTarget.Name = "udBiasFinalTarget";
            this.udBiasFinalTarget.Size = new System.Drawing.Size(56, 20);
            this.udBiasFinalTarget.TabIndex = 28;
            this.udBiasFinalTarget.Value = new decimal(new int[] {
            20,
            0,
            0,
            65536});
            this.udBiasFinalTarget.Visible = false;
            // 
            // lstDebug
            // 
            this.lstDebug.HorizontalScrollbar = true;
            this.lstDebug.Location = new System.Drawing.Point(240, 8);
            this.lstDebug.Name = "lstDebug";
            this.lstDebug.Size = new System.Drawing.Size(256, 264);
            this.lstDebug.TabIndex = 29;
            // 
            // grpATU
            // 
            this.grpATU.Controls.Add(this.btnATUSWR);
            this.grpATU.Location = new System.Drawing.Point(120, 8);
            this.grpATU.Name = "grpATU";
            this.grpATU.Size = new System.Drawing.Size(112, 64);
            this.grpATU.TabIndex = 32;
            this.grpATU.TabStop = false;
            this.grpATU.Text = "ATU";
            this.grpATU.Visible = false;
            // 
            // btnATUSWR
            // 
            this.btnATUSWR.Image = null;
            this.btnATUSWR.Location = new System.Drawing.Point(16, 24);
            this.btnATUSWR.Name = "btnATUSWR";
            this.btnATUSWR.Size = new System.Drawing.Size(75, 23);
            this.btnATUSWR.TabIndex = 20;
            this.btnATUSWR.Text = "Cal ATU";
            this.btnATUSWR.Click += new System.EventHandler(this.btnATUCal_Click);
            // 
            // grpBridgeNull
            // 
            this.grpBridgeNull.Controls.Add(this.picNullBridge);
            this.grpBridgeNull.Location = new System.Drawing.Point(120, 216);
            this.grpBridgeNull.Name = "grpBridgeNull";
            this.grpBridgeNull.Size = new System.Drawing.Size(312, 56);
            this.grpBridgeNull.TabIndex = 26;
            this.grpBridgeNull.TabStop = false;
            this.grpBridgeNull.Visible = false;
            // 
            // picNullBridge
            // 
            this.picNullBridge.BackColor = System.Drawing.SystemColors.ControlText;
            this.picNullBridge.Location = new System.Drawing.Point(8, 16);
            this.picNullBridge.Name = "picNullBridge";
            this.picNullBridge.Size = new System.Drawing.Size(296, 32);
            this.picNullBridge.TabIndex = 0;
            this.picNullBridge.TabStop = false;
            this.picNullBridge.Paint += new System.Windows.Forms.PaintEventHandler(this.picNullBridge_Paint);
            // 
            // grpIO
            // 
            this.grpIO.Controls.Add(this.btnIORunAll);
            this.grpIO.Controls.Add(this.btnIORX1InOut);
            this.grpIO.Controls.Add(this.btnIOXVRX);
            this.grpIO.Controls.Add(this.btnIOTXMon);
            this.grpIO.Location = new System.Drawing.Point(120, 8);
            this.grpIO.Name = "grpIO";
            this.grpIO.Size = new System.Drawing.Size(112, 192);
            this.grpIO.TabIndex = 25;
            this.grpIO.TabStop = false;
            this.grpIO.Text = "Input/Output";
            // 
            // btnIORunAll
            // 
            this.btnIORunAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIORunAll.Image = null;
            this.btnIORunAll.Location = new System.Drawing.Point(16, 24);
            this.btnIORunAll.Name = "btnIORunAll";
            this.btnIORunAll.Size = new System.Drawing.Size(80, 32);
            this.btnIORunAll.TabIndex = 31;
            this.btnIORunAll.Text = "Run All";
            this.btnIORunAll.Click += new System.EventHandler(this.btnIORunAll_Click);
            // 
            // btnIORX1InOut
            // 
            this.btnIORX1InOut.Image = null;
            this.btnIORX1InOut.Location = new System.Drawing.Point(16, 112);
            this.btnIORX1InOut.Name = "btnIORX1InOut";
            this.btnIORX1InOut.Size = new System.Drawing.Size(75, 23);
            this.btnIORX1InOut.TabIndex = 21;
            this.btnIORX1InOut.Text = "RX1 In/Out";
            this.btnIORX1InOut.Click += new System.EventHandler(this.btnIORX1InOut_Click);
            // 
            // btnIOXVRX
            // 
            this.btnIOXVRX.Image = null;
            this.btnIOXVRX.Location = new System.Drawing.Point(16, 72);
            this.btnIOXVRX.Name = "btnIOXVRX";
            this.btnIOXVRX.Size = new System.Drawing.Size(75, 23);
            this.btnIOXVRX.TabIndex = 20;
            this.btnIOXVRX.Text = "XVRX";
            this.btnIOXVRX.Click += new System.EventHandler(this.btnIOXVRX_Click);
            // 
            // btnIOTXMon
            // 
            this.btnIOTXMon.Image = null;
            this.btnIOTXMon.Location = new System.Drawing.Point(16, 152);
            this.btnIOTXMon.Name = "btnIOTXMon";
            this.btnIOTXMon.Size = new System.Drawing.Size(75, 23);
            this.btnIOTXMon.TabIndex = 30;
            this.btnIOTXMon.Text = "TX MON";
            this.btnIOTXMon.Click += new System.EventHandler(this.btnIOTXMon_Click);
            // 
            // grpPA
            // 
            this.grpPA.Controls.Add(this.btnPAVerify);
            this.grpPA.Controls.Add(this.btnPASWR);
            this.grpPA.Controls.Add(this.btnNullBridge);
            this.grpPA.Controls.Add(this.btnPABias);
            this.grpPA.Controls.Add(this.btnPAPower);
            this.grpPA.Controls.Add(this.btnRunPACal);
            this.grpPA.Location = new System.Drawing.Point(8, 8);
            this.grpPA.Name = "grpPA";
            this.grpPA.Size = new System.Drawing.Size(104, 264);
            this.grpPA.TabIndex = 24;
            this.grpPA.TabStop = false;
            this.grpPA.Text = "Power Amplifier";
            // 
            // btnPAVerify
            // 
            this.btnPAVerify.Image = null;
            this.btnPAVerify.Location = new System.Drawing.Point(16, 224);
            this.btnPAVerify.Name = "btnPAVerify";
            this.btnPAVerify.Size = new System.Drawing.Size(75, 23);
            this.btnPAVerify.TabIndex = 22;
            this.btnPAVerify.Text = "Verify";
            this.btnPAVerify.Click += new System.EventHandler(this.btnPAVerify_Click);
            // 
            // btnPASWR
            // 
            this.btnPASWR.Image = null;
            this.btnPASWR.Location = new System.Drawing.Point(16, 184);
            this.btnPASWR.Name = "btnPASWR";
            this.btnPASWR.Size = new System.Drawing.Size(75, 23);
            this.btnPASWR.TabIndex = 21;
            this.btnPASWR.Text = "SWR";
            this.btnPASWR.Visible = false;
            this.btnPASWR.Click += new System.EventHandler(this.btnPASWR_Click);
            // 
            // btnNullBridge
            // 
            this.btnNullBridge.Image = null;
            this.btnNullBridge.Location = new System.Drawing.Point(16, 64);
            this.btnNullBridge.Name = "btnNullBridge";
            this.btnNullBridge.Size = new System.Drawing.Size(75, 23);
            this.btnNullBridge.TabIndex = 19;
            this.btnNullBridge.Text = "Null Bridge";
            this.btnNullBridge.Click += new System.EventHandler(this.btnNullBridge_Click);
            // 
            // btnPABias
            // 
            this.btnPABias.Image = null;
            this.btnPABias.Location = new System.Drawing.Point(16, 24);
            this.btnPABias.Name = "btnPABias";
            this.btnPABias.Size = new System.Drawing.Size(75, 23);
            this.btnPABias.TabIndex = 18;
            this.btnPABias.Text = "Bias";
            this.btnPABias.Click += new System.EventHandler(this.btnPABias_Click);
            // 
            // btnPAPower
            // 
            this.btnPAPower.Image = null;
            this.btnPAPower.Location = new System.Drawing.Point(16, 144);
            this.btnPAPower.Name = "btnPAPower";
            this.btnPAPower.Size = new System.Drawing.Size(75, 23);
            this.btnPAPower.TabIndex = 20;
            this.btnPAPower.Text = "Power";
            this.btnPAPower.Click += new System.EventHandler(this.btnPAPower_Click);
            // 
            // btnRunPACal
            // 
            this.btnRunPACal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRunPACal.Image = null;
            this.btnRunPACal.Location = new System.Drawing.Point(16, 96);
            this.btnRunPACal.Name = "btnRunPACal";
            this.btnRunPACal.Size = new System.Drawing.Size(80, 32);
            this.btnRunPACal.TabIndex = 31;
            this.btnRunPACal.Text = "Run PA Cal";
            this.btnRunPACal.Click += new System.EventHandler(this.btnRunPACal_Click);
            // 
            // btnPrintReport
            // 
            this.btnPrintReport.Image = null;
            this.btnPrintReport.Location = new System.Drawing.Point(395, 280);
            this.btnPrintReport.Name = "btnPrintReport";
            this.btnPrintReport.Size = new System.Drawing.Size(75, 24);
            this.btnPrintReport.TabIndex = 23;
            this.btnPrintReport.Text = "Print Report";
            this.btnPrintReport.Click += new System.EventHandler(this.btnPrintReport_Click);
            // 
            // txtTech
            // 
            this.txtTech.Location = new System.Drawing.Point(272, 304);
            this.txtTech.Name = "txtTech";
            this.txtTech.Size = new System.Drawing.Size(100, 20);
            this.txtTech.TabIndex = 22;
            // 
            // lblTech
            // 
            this.lblTech.Image = null;
            this.lblTech.Location = new System.Drawing.Point(272, 288);
            this.lblTech.Name = "lblTech";
            this.lblTech.Size = new System.Drawing.Size(64, 16);
            this.lblTech.TabIndex = 21;
            this.lblTech.Text = "Technician:";
            // 
            // grpComPort
            // 
            this.grpComPort.Controls.Add(this.comboCOMPort);
            this.grpComPort.Location = new System.Drawing.Point(272, 336);
            this.grpComPort.Name = "grpComPort";
            this.grpComPort.Size = new System.Drawing.Size(104, 48);
            this.grpComPort.TabIndex = 17;
            this.grpComPort.TabStop = false;
            this.grpComPort.Text = "COM Port";
            // 
            // grpBands
            // 
            this.grpBands.Controls.Add(this.ck6);
            this.grpBands.Controls.Add(this.ck10);
            this.grpBands.Controls.Add(this.ck12);
            this.grpBands.Controls.Add(this.ck15);
            this.grpBands.Controls.Add(this.ck17);
            this.grpBands.Controls.Add(this.ck20);
            this.grpBands.Controls.Add(this.ck30);
            this.grpBands.Controls.Add(this.ck40);
            this.grpBands.Controls.Add(this.ck60);
            this.grpBands.Controls.Add(this.ck80);
            this.grpBands.Controls.Add(this.ck160);
            this.grpBands.Controls.Add(this.btnClearAll);
            this.grpBands.Controls.Add(this.btnCheckAll);
            this.grpBands.Location = new System.Drawing.Point(8, 280);
            this.grpBands.Name = "grpBands";
            this.grpBands.Size = new System.Drawing.Size(256, 104);
            this.grpBands.TabIndex = 16;
            this.grpBands.TabStop = false;
            this.grpBands.Text = "Bands";
            // 
            // ck6
            // 
            this.ck6.Checked = true;
            this.ck6.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ck6.Image = null;
            this.ck6.Location = new System.Drawing.Point(216, 40);
            this.ck6.Name = "ck6";
            this.ck6.Size = new System.Drawing.Size(32, 24);
            this.ck6.TabIndex = 28;
            this.ck6.Text = "6";
            // 
            // ck10
            // 
            this.ck10.Checked = true;
            this.ck10.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ck10.Image = null;
            this.ck10.Location = new System.Drawing.Point(176, 40);
            this.ck10.Name = "ck10";
            this.ck10.Size = new System.Drawing.Size(40, 24);
            this.ck10.TabIndex = 27;
            this.ck10.Text = "10";
            // 
            // ck12
            // 
            this.ck12.Checked = true;
            this.ck12.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ck12.Image = null;
            this.ck12.Location = new System.Drawing.Point(136, 40);
            this.ck12.Name = "ck12";
            this.ck12.Size = new System.Drawing.Size(40, 24);
            this.ck12.TabIndex = 26;
            this.ck12.Text = "12";
            // 
            // ck15
            // 
            this.ck15.Checked = true;
            this.ck15.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ck15.Image = null;
            this.ck15.Location = new System.Drawing.Point(96, 40);
            this.ck15.Name = "ck15";
            this.ck15.Size = new System.Drawing.Size(40, 24);
            this.ck15.TabIndex = 25;
            this.ck15.Text = "15";
            // 
            // ck17
            // 
            this.ck17.Checked = true;
            this.ck17.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ck17.Image = null;
            this.ck17.Location = new System.Drawing.Point(56, 40);
            this.ck17.Name = "ck17";
            this.ck17.Size = new System.Drawing.Size(40, 24);
            this.ck17.TabIndex = 24;
            this.ck17.Text = "17";
            // 
            // ck20
            // 
            this.ck20.Checked = true;
            this.ck20.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ck20.Image = null;
            this.ck20.Location = new System.Drawing.Point(16, 40);
            this.ck20.Name = "ck20";
            this.ck20.Size = new System.Drawing.Size(40, 24);
            this.ck20.TabIndex = 23;
            this.ck20.Text = "20";
            // 
            // ck30
            // 
            this.ck30.Checked = true;
            this.ck30.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ck30.Image = null;
            this.ck30.Location = new System.Drawing.Point(184, 16);
            this.ck30.Name = "ck30";
            this.ck30.Size = new System.Drawing.Size(40, 24);
            this.ck30.TabIndex = 22;
            this.ck30.Text = "30";
            // 
            // ck40
            // 
            this.ck40.Checked = true;
            this.ck40.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ck40.Image = null;
            this.ck40.Location = new System.Drawing.Point(144, 16);
            this.ck40.Name = "ck40";
            this.ck40.Size = new System.Drawing.Size(40, 24);
            this.ck40.TabIndex = 21;
            this.ck40.Text = "40";
            // 
            // ck60
            // 
            this.ck60.Checked = true;
            this.ck60.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ck60.Image = null;
            this.ck60.Location = new System.Drawing.Point(104, 16);
            this.ck60.Name = "ck60";
            this.ck60.Size = new System.Drawing.Size(40, 24);
            this.ck60.TabIndex = 20;
            this.ck60.Text = "60";
            // 
            // ck80
            // 
            this.ck80.Checked = true;
            this.ck80.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ck80.Image = null;
            this.ck80.Location = new System.Drawing.Point(64, 16);
            this.ck80.Name = "ck80";
            this.ck80.Size = new System.Drawing.Size(40, 24);
            this.ck80.TabIndex = 19;
            this.ck80.Text = "80";
            // 
            // ck160
            // 
            this.ck160.Checked = true;
            this.ck160.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ck160.Image = null;
            this.ck160.Location = new System.Drawing.Point(16, 16);
            this.ck160.Name = "ck160";
            this.ck160.Size = new System.Drawing.Size(48, 24);
            this.ck160.TabIndex = 18;
            this.ck160.Text = "160";
            // 
            // btnClearAll
            // 
            this.btnClearAll.Image = null;
            this.btnClearAll.Location = new System.Drawing.Point(112, 64);
            this.btnClearAll.Name = "btnClearAll";
            this.btnClearAll.Size = new System.Drawing.Size(64, 20);
            this.btnClearAll.TabIndex = 30;
            this.btnClearAll.Text = "Clear All";
            this.btnClearAll.Click += new System.EventHandler(this.btnClearAll_Click);
            // 
            // btnCheckAll
            // 
            this.btnCheckAll.Image = null;
            this.btnCheckAll.Location = new System.Drawing.Point(24, 64);
            this.btnCheckAll.Name = "btnCheckAll";
            this.btnCheckAll.Size = new System.Drawing.Size(64, 20);
            this.btnCheckAll.TabIndex = 29;
            this.btnCheckAll.Text = "Check All";
            this.btnCheckAll.Click += new System.EventHandler(this.btnCheckAll_Click);
            // 
            // btnCheckEEPROM
            // 
            this.btnCheckEEPROM.Image = null;
            this.btnCheckEEPROM.Location = new System.Drawing.Point(395, 306);
            this.btnCheckEEPROM.Name = "btnCheckEEPROM";
            this.btnCheckEEPROM.Size = new System.Drawing.Size(97, 24);
            this.btnCheckEEPROM.TabIndex = 33;
            this.btnCheckEEPROM.Text = "Check EEPROM";
            this.btnCheckEEPROM.Click += new System.EventHandler(this.btnCheckEEPROM_Click);
            // 
            // ckPM2
            // 
            this.ckPM2.Image = null;
            this.ckPM2.Location = new System.Drawing.Point(446, 354);
            this.ckPM2.Name = "ckPM2";
            this.ckPM2.Size = new System.Drawing.Size(56, 24);
            this.ckPM2.TabIndex = 31;
            this.ckPM2.Text = "PM2";
            this.toolTip1.SetToolTip(this.ckPM2, "Check for PowerMaster II");
            this.ckPM2.CheckedChanged += new System.EventHandler(this.ckPM2_CheckedChanged);
            // 
            // FLEX5000FinalTestForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(504, 390);
            this.Controls.Add(this.ckPM2);
            this.Controls.Add(this.btnCheckEEPROM);
            this.Controls.Add(this.grpATU);
            this.Controls.Add(this.grpBridgeNull);
            this.Controls.Add(this.lstDebug);
            this.Controls.Add(this.udBiasFinalTarget);
            this.Controls.Add(this.udBiasDriverTarget);
            this.Controls.Add(this.grpIO);
            this.Controls.Add(this.grpPA);
            this.Controls.Add(this.btnPrintReport);
            this.Controls.Add(this.txtTech);
            this.Controls.Add(this.lblTech);
            this.Controls.Add(this.grpComPort);
            this.Controls.Add(this.grpBands);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "FLEX5000FinalTestForm";
            this.Text = "FLEX-5000 Production PA / IO Test";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.FLEX5000FinalTestForm_Closing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FLEX5000FinalTestForm_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.udBiasDriverTarget)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udBiasFinalTarget)).EndInit();
            this.grpATU.ResumeLayout(false);
            this.grpBridgeNull.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picNullBridge)).EndInit();
            this.grpIO.ResumeLayout(false);
            this.grpPA.ResumeLayout(false);
            this.grpComPort.ResumeLayout(false);
            this.grpBands.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

      

        private System.Windows.Forms.GroupBoxTS grpBands;
        private System.Windows.Forms.ButtonTS btnClearAll;
        private System.Windows.Forms.ButtonTS btnCheckAll;
        private System.Windows.Forms.GroupBoxTS grpComPort;
        private System.Windows.Forms.ComboBoxTS comboCOMPort;
        private System.Windows.Forms.ButtonTS btnPrintReport;
        private System.Windows.Forms.TextBoxTS txtTech;
        private System.Windows.Forms.LabelTS lblTech;
        private System.Windows.Forms.ButtonTS btnPABias;
        private System.Windows.Forms.ButtonTS btnNullBridge;
        private System.Windows.Forms.ButtonTS btnPAPower;
        private System.Windows.Forms.GroupBoxTS grpPA;
        private System.Windows.Forms.GroupBoxTS grpIO;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.PrintPreviewDialog printPreviewDialog1;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.GroupBoxTS grpBridgeNull;
        private System.Windows.Forms.PictureBox picNullBridge;
        private System.Windows.Forms.NumericUpDown udBiasDriverTarget;
        private System.Windows.Forms.NumericUpDown udBiasFinalTarget;
        private System.Windows.Forms.ListBox lstDebug;
        private System.Windows.Forms.ButtonTS btnIOXVRX;
        private System.Windows.Forms.ButtonTS btnIORX1InOut;
        private System.Windows.Forms.ButtonTS btnPASWR;
        private System.Windows.Forms.ButtonTS btnIOTXMon;
        private System.Windows.Forms.ButtonTS btnIORunAll;
        private System.Windows.Forms.ButtonTS btnPAVerify;
        private System.Windows.Forms.ButtonTS btnRunPACal;
        private System.Windows.Forms.CheckBoxTS ck6;
        private System.Windows.Forms.CheckBoxTS ck10;
        private System.Windows.Forms.CheckBoxTS ck12;
        private System.Windows.Forms.CheckBoxTS ck15;
        private System.Windows.Forms.CheckBoxTS ck17;
        private System.Windows.Forms.CheckBoxTS ck20;
        private System.Windows.Forms.CheckBoxTS ck30;
        private System.Windows.Forms.CheckBoxTS ck40;
        private System.Windows.Forms.CheckBoxTS ck60;
        private System.Windows.Forms.CheckBoxTS ck80;
        private System.Windows.Forms.CheckBoxTS ck160;
        private System.Windows.Forms.GroupBoxTS grpATU;
        private System.Windows.Forms.ButtonTS btnATUSWR;
        private System.Windows.Forms.ButtonTS btnCheckEEPROM;
        private System.ComponentModel.IContainer components;
        public System.Windows.Forms.CheckBoxTS ckPM2;
       
      

    }
}
