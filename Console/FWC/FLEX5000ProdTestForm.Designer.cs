//=================================================================
// FLEX5000ProdTestForm.cs
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
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace PowerSDR
{
    unsafe public partial class FLEX5000ProdTestForm : System.Windows.Forms.Form
    {
              

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FLEX5000ProdTestForm));
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.comboCOMPort = new System.Windows.Forms.ComboBoxTS();
            this.btnGenATTN = new System.Windows.Forms.ButtonTS();
            this.btnGenPreamp = new System.Windows.Forms.ButtonTS();
            this.btnNoise = new System.Windows.Forms.ButtonTS();
            this.btnGenBal = new System.Windows.Forms.ButtonTS();
            this.btnPLL = new System.Windows.Forms.ButtonTS();
            this.btnImpulse = new System.Windows.Forms.ButtonTS();
            this.btnRXImage = new System.Windows.Forms.ButtonTS();
            this.btnRXFilter = new System.Windows.Forms.ButtonTS();
            this.btnRXLevel = new System.Windows.Forms.ButtonTS();
            this.checkGENBAL = new System.Windows.Forms.CheckBoxTS();
            this.lstDebug = new System.Windows.Forms.ListBox();
            this.printPreviewDialog1 = new System.Windows.Forms.PrintPreviewDialog();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.btnRunAll1500 = new System.Windows.Forms.ButtonTS();
            this.btnPostFence = new System.Windows.Forms.ButtonTS();
            this.grpIO = new System.Windows.Forms.GroupBoxTS();
            this.grpComPort = new System.Windows.Forms.GroupBoxTS();
            this.btnIOFWPTT = new System.Windows.Forms.ButtonTS();
            this.btnIOMicFast = new System.Windows.Forms.ButtonTS();
            this.btnIOMicDown = new System.Windows.Forms.ButtonTS();
            this.btnIOMicUp = new System.Windows.Forms.ButtonTS();
            this.btnIORunAll = new System.Windows.Forms.ButtonTS();
            this.btnIOExtRef = new System.Windows.Forms.ButtonTS();
            this.btnIOHeadphone = new System.Windows.Forms.ButtonTS();
            this.btnIOMicPTT = new System.Windows.Forms.ButtonTS();
            this.btnIORCAPTT = new System.Windows.Forms.ButtonTS();
            this.btnIODash = new System.Windows.Forms.ButtonTS();
            this.btnIODot = new System.Windows.Forms.ButtonTS();
            this.btnIOFWInOut = new System.Windows.Forms.ButtonTS();
            this.btnIORCAInOut = new System.Windows.Forms.ButtonTS();
            this.btnIOPwrSpkr = new System.Windows.Forms.ButtonTS();
            this.btnTestNone = new System.Windows.Forms.ButtonTS();
            this.btnTestAll = new System.Windows.Forms.ButtonTS();
            this.grpTestTransmitter = new System.Windows.Forms.GroupBoxTS();
            this.ckTestTXPA = new System.Windows.Forms.CheckBoxTS();
            this.ckTestTXGain = new System.Windows.Forms.CheckBoxTS();
            this.btnTestTXNone = new System.Windows.Forms.ButtonTS();
            this.btnTestTXAll = new System.Windows.Forms.ButtonTS();
            this.ckTestTXImage = new System.Windows.Forms.CheckBoxTS();
            this.ckTestTXCarrier = new System.Windows.Forms.CheckBoxTS();
            this.ckTestTXFilter = new System.Windows.Forms.CheckBoxTS();
            this.grpTestReceiver = new System.Windows.Forms.GroupBoxTS();
            this.btnTestRXNone = new System.Windows.Forms.ButtonTS();
            this.btnTestRXAll = new System.Windows.Forms.ButtonTS();
            this.ckTestRXMDS = new System.Windows.Forms.CheckBoxTS();
            this.ckTestRXImage = new System.Windows.Forms.CheckBoxTS();
            this.ckTestRXLevel = new System.Windows.Forms.CheckBoxTS();
            this.ckTestRXFilter = new System.Windows.Forms.CheckBoxTS();
            this.grpTestGeneral = new System.Windows.Forms.GroupBoxTS();
            this.btnTestGenAll = new System.Windows.Forms.ButtonTS();
            this.ckTestGenPreamp = new System.Windows.Forms.CheckBoxTS();
            this.btnTestGenNone = new System.Windows.Forms.ButtonTS();
            this.ckTestGenImpulse = new System.Windows.Forms.CheckBoxTS();
            this.ckTestGenNoise = new System.Windows.Forms.CheckBoxTS();
            this.ckTestGenBal = new System.Windows.Forms.CheckBoxTS();
            this.ckTestGenPLL = new System.Windows.Forms.CheckBoxTS();
            this.ckTestGenATTN = new System.Windows.Forms.CheckBoxTS();
            this.btnPrintReport = new System.Windows.Forms.ButtonTS();
            this.txtTech = new System.Windows.Forms.TextBoxTS();
            this.lblTech = new System.Windows.Forms.LabelTS();
            this.btnRunSelectedTests = new System.Windows.Forms.ButtonTS();
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
            this.grpTransmitter = new System.Windows.Forms.GroupBoxTS();
            this.btnTX1500PA = new System.Windows.Forms.ButtonTS();
            this.btnTXGain = new System.Windows.Forms.ButtonTS();
            this.btnTXFilter = new System.Windows.Forms.ButtonTS();
            this.btnTXCarrier = new System.Windows.Forms.ButtonTS();
            this.btnTXImage = new System.Windows.Forms.ButtonTS();
            this.grpGeneral = new System.Windows.Forms.GroupBoxTS();
            this.grpReceiver = new System.Windows.Forms.GroupBoxTS();
            this.udLevel = new System.Windows.Forms.NumericUpDown();
            this.ckPM2 = new System.Windows.Forms.CheckBoxTS();
            this.grpIO.SuspendLayout();
            this.grpComPort.SuspendLayout();
            this.grpTestTransmitter.SuspendLayout();
            this.grpTestReceiver.SuspendLayout();
            this.grpTestGeneral.SuspendLayout();
            this.grpBands.SuspendLayout();
            this.grpTransmitter.SuspendLayout();
            this.grpGeneral.SuspendLayout();
            this.grpReceiver.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udLevel)).BeginInit();
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
            // btnGenATTN
            // 
            this.btnGenATTN.Image = null;
            this.btnGenATTN.Location = new System.Drawing.Point(96, 80);
            this.btnGenATTN.Name = "btnGenATTN";
            this.btnGenATTN.Size = new System.Drawing.Size(75, 23);
            this.btnGenATTN.TabIndex = 8;
            this.btnGenATTN.Text = "Attenuator";
            this.toolTip1.SetToolTip(this.btnGenATTN, "Attenuator Test: Not Run");
            this.btnGenATTN.Visible = false;
            this.btnGenATTN.Click += new System.EventHandler(this.btnGenATTN_Click);
            // 
            // btnGenPreamp
            // 
            this.btnGenPreamp.Image = null;
            this.btnGenPreamp.Location = new System.Drawing.Point(8, 80);
            this.btnGenPreamp.Name = "btnGenPreamp";
            this.btnGenPreamp.Size = new System.Drawing.Size(75, 23);
            this.btnGenPreamp.TabIndex = 7;
            this.btnGenPreamp.Text = "Preamp";
            this.toolTip1.SetToolTip(this.btnGenPreamp, "Preamp Test: Not Run");
            this.btnGenPreamp.Click += new System.EventHandler(this.btnGenPreamp_Click);
            // 
            // btnNoise
            // 
            this.btnNoise.Image = null;
            this.btnNoise.Location = new System.Drawing.Point(8, 48);
            this.btnNoise.Name = "btnNoise";
            this.btnNoise.Size = new System.Drawing.Size(75, 23);
            this.btnNoise.TabIndex = 2;
            this.btnNoise.Text = "Noise";
            this.toolTip1.SetToolTip(this.btnNoise, "Noise Test: Not Run");
            this.btnNoise.Visible = false;
            this.btnNoise.Click += new System.EventHandler(this.btnNoise_Click);
            // 
            // btnGenBal
            // 
            this.btnGenBal.Image = null;
            this.btnGenBal.Location = new System.Drawing.Point(96, 16);
            this.btnGenBal.Name = "btnGenBal";
            this.btnGenBal.Size = new System.Drawing.Size(75, 23);
            this.btnGenBal.TabIndex = 1;
            this.btnGenBal.Text = "Gen/Bal";
            this.toolTip1.SetToolTip(this.btnGenBal, "Gen/Bal Test: Not Run");
            this.btnGenBal.Click += new System.EventHandler(this.btnGenBal_Click);
            // 
            // btnPLL
            // 
            this.btnPLL.Image = null;
            this.btnPLL.Location = new System.Drawing.Point(8, 16);
            this.btnPLL.Name = "btnPLL";
            this.btnPLL.Size = new System.Drawing.Size(75, 23);
            this.btnPLL.TabIndex = 0;
            this.btnPLL.Text = "PLL";
            this.toolTip1.SetToolTip(this.btnPLL, "PLL Test: Not Run");
            this.btnPLL.Click += new System.EventHandler(this.btnPLL_Click);
            // 
            // btnImpulse
            // 
            this.btnImpulse.Image = null;
            this.btnImpulse.Location = new System.Drawing.Point(96, 48);
            this.btnImpulse.Name = "btnImpulse";
            this.btnImpulse.Size = new System.Drawing.Size(75, 23);
            this.btnImpulse.TabIndex = 6;
            this.btnImpulse.Text = "Impulse";
            this.toolTip1.SetToolTip(this.btnImpulse, "Impulse Test: Not Run");
            this.btnImpulse.Click += new System.EventHandler(this.btnImpulse_Click);
            // 
            // btnRXImage
            // 
            this.btnRXImage.Image = null;
            this.btnRXImage.Location = new System.Drawing.Point(16, 64);
            this.btnRXImage.Name = "btnRXImage";
            this.btnRXImage.Size = new System.Drawing.Size(75, 23);
            this.btnRXImage.TabIndex = 4;
            this.btnRXImage.Text = "Image";
            this.toolTip1.SetToolTip(this.btnRXImage, "RX Image Test: Not Run");
            this.btnRXImage.Click += new System.EventHandler(this.btnRXImage_Click);
            // 
            // btnRXFilter
            // 
            this.btnRXFilter.Image = null;
            this.btnRXFilter.Location = new System.Drawing.Point(16, 24);
            this.btnRXFilter.Name = "btnRXFilter";
            this.btnRXFilter.Size = new System.Drawing.Size(75, 23);
            this.btnRXFilter.TabIndex = 1;
            this.btnRXFilter.Text = "Filter";
            this.toolTip1.SetToolTip(this.btnRXFilter, "RX Filter Test: Not Run");
            this.btnRXFilter.Click += new System.EventHandler(this.btnRXFilter_Click);
            // 
            // btnRXLevel
            // 
            this.btnRXLevel.Image = null;
            this.btnRXLevel.Location = new System.Drawing.Point(104, 24);
            this.btnRXLevel.Name = "btnRXLevel";
            this.btnRXLevel.Size = new System.Drawing.Size(75, 23);
            this.btnRXLevel.TabIndex = 3;
            this.btnRXLevel.Text = "Level";
            this.toolTip1.SetToolTip(this.btnRXLevel, "RX Level Test: Not Run");
            this.btnRXLevel.Click += new System.EventHandler(this.btnRXLevel_Click);
            // 
            // checkGENBAL
            // 
            this.checkGENBAL.Image = null;
            this.checkGENBAL.Location = new System.Drawing.Point(195, 23);
            this.checkGENBAL.Margin = new System.Windows.Forms.Padding(0);
            this.checkGENBAL.Name = "checkGENBAL";
            this.checkGENBAL.Size = new System.Drawing.Size(27, 24);
            this.checkGENBAL.TabIndex = 31;
            this.toolTip1.SetToolTip(this.checkGENBAL, "Automatic Repeat of the GEN/BAL test until unchecked.\r\nNOTE: Does not update the " +
        "LOG file");
            // 
            // lstDebug
            // 
            this.lstDebug.HorizontalScrollbar = true;
            this.lstDebug.Location = new System.Drawing.Point(216, 128);
            this.lstDebug.Name = "lstDebug";
            this.lstDebug.Size = new System.Drawing.Size(256, 199);
            this.lstDebug.TabIndex = 16;
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
            // btnRunAll1500
            // 
            this.btnRunAll1500.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRunAll1500.Image = null;
            this.btnRunAll1500.Location = new System.Drawing.Point(387, 343);
            this.btnRunAll1500.Name = "btnRunAll1500";
            this.btnRunAll1500.Size = new System.Drawing.Size(88, 32);
            this.btnRunAll1500.TabIndex = 31;
            this.btnRunAll1500.Text = "Run All 1500 Tests";
            this.btnRunAll1500.Visible = false;
            this.btnRunAll1500.Click += new System.EventHandler(this.btnRunAll1500_Click);
            // 
            // btnPostFence
            // 
            this.btnPostFence.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPostFence.Image = null;
            this.btnPostFence.Location = new System.Drawing.Point(152, 344);
            this.btnPostFence.Name = "btnPostFence";
            this.btnPostFence.Size = new System.Drawing.Size(56, 32);
            this.btnPostFence.TabIndex = 29;
            this.btnPostFence.Text = "Post Fence";
            this.btnPostFence.Click += new System.EventHandler(this.btnPostFence_Click);
            // 
            // grpIO
            // 
            this.grpIO.Controls.Add(this.grpComPort);
            this.grpIO.Controls.Add(this.btnIOFWPTT);
            this.grpIO.Controls.Add(this.btnIOMicFast);
            this.grpIO.Controls.Add(this.btnIOMicDown);
            this.grpIO.Controls.Add(this.btnIOMicUp);
            this.grpIO.Controls.Add(this.btnIORunAll);
            this.grpIO.Controls.Add(this.btnIOExtRef);
            this.grpIO.Controls.Add(this.btnIOHeadphone);
            this.grpIO.Controls.Add(this.btnIOMicPTT);
            this.grpIO.Controls.Add(this.btnIORCAPTT);
            this.grpIO.Controls.Add(this.btnIODash);
            this.grpIO.Controls.Add(this.btnIODot);
            this.grpIO.Controls.Add(this.btnIOFWInOut);
            this.grpIO.Controls.Add(this.btnIORCAInOut);
            this.grpIO.Controls.Add(this.btnIOPwrSpkr);
            this.grpIO.Location = new System.Drawing.Point(480, 8);
            this.grpIO.Name = "grpIO";
            this.grpIO.Size = new System.Drawing.Size(104, 472);
            this.grpIO.TabIndex = 28;
            this.grpIO.TabStop = false;
            this.grpIO.Text = "Input/Output";
            // 
            // grpComPort
            // 
            this.grpComPort.Controls.Add(this.comboCOMPort);
            this.grpComPort.Location = new System.Drawing.Point(5, 371);
            this.grpComPort.Name = "grpComPort";
            this.grpComPort.Size = new System.Drawing.Size(104, 48);
            this.grpComPort.TabIndex = 30;
            this.grpComPort.TabStop = false;
            this.grpComPort.Text = "COM Port";
            this.grpComPort.Visible = false;
            // 
            // btnIOFWPTT
            // 
            this.btnIOFWPTT.Image = null;
            this.btnIOFWPTT.Location = new System.Drawing.Point(16, 304);
            this.btnIOFWPTT.Name = "btnIOFWPTT";
            this.btnIOFWPTT.Size = new System.Drawing.Size(75, 23);
            this.btnIOFWPTT.TabIndex = 35;
            this.btnIOFWPTT.Text = "FW PTT";
            this.btnIOFWPTT.Visible = false;
            this.btnIOFWPTT.Click += new System.EventHandler(this.btnIOFWPTT_Click);
            // 
            // btnIOMicFast
            // 
            this.btnIOMicFast.Image = null;
            this.btnIOMicFast.Location = new System.Drawing.Point(16, 384);
            this.btnIOMicFast.Name = "btnIOMicFast";
            this.btnIOMicFast.Size = new System.Drawing.Size(75, 23);
            this.btnIOMicFast.TabIndex = 34;
            this.btnIOMicFast.Text = "Mic Fast";
            this.btnIOMicFast.Visible = false;
            this.btnIOMicFast.Click += new System.EventHandler(this.btnIOMicFast_Click);
            // 
            // btnIOMicDown
            // 
            this.btnIOMicDown.Image = null;
            this.btnIOMicDown.Location = new System.Drawing.Point(16, 104);
            this.btnIOMicDown.Name = "btnIOMicDown";
            this.btnIOMicDown.Size = new System.Drawing.Size(75, 23);
            this.btnIOMicDown.TabIndex = 33;
            this.btnIOMicDown.Text = "Mic Down";
            this.btnIOMicDown.Visible = false;
            this.btnIOMicDown.Click += new System.EventHandler(this.btnIOMicDown_Click);
            // 
            // btnIOMicUp
            // 
            this.btnIOMicUp.Image = null;
            this.btnIOMicUp.Location = new System.Drawing.Point(16, 24);
            this.btnIOMicUp.Name = "btnIOMicUp";
            this.btnIOMicUp.Size = new System.Drawing.Size(75, 23);
            this.btnIOMicUp.TabIndex = 32;
            this.btnIOMicUp.Text = "Mic Up";
            this.btnIOMicUp.Visible = false;
            this.btnIOMicUp.Click += new System.EventHandler(this.btnIOMicUp_Click);
            // 
            // btnIORunAll
            // 
            this.btnIORunAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIORunAll.Image = null;
            this.btnIORunAll.Location = new System.Drawing.Point(16, 424);
            this.btnIORunAll.Name = "btnIORunAll";
            this.btnIORunAll.Size = new System.Drawing.Size(75, 40);
            this.btnIORunAll.TabIndex = 31;
            this.btnIORunAll.Text = "Run All IO Tests";
            this.btnIORunAll.Click += new System.EventHandler(this.btnIORunAll_Click);
            // 
            // btnIOExtRef
            // 
            this.btnIOExtRef.Image = null;
            this.btnIOExtRef.Location = new System.Drawing.Point(16, 24);
            this.btnIOExtRef.Name = "btnIOExtRef";
            this.btnIOExtRef.Size = new System.Drawing.Size(75, 23);
            this.btnIOExtRef.TabIndex = 30;
            this.btnIOExtRef.Text = "Ext Ref";
            this.btnIOExtRef.Click += new System.EventHandler(this.btnIOExtRef_Click);
            // 
            // btnIOHeadphone
            // 
            this.btnIOHeadphone.Image = null;
            this.btnIOHeadphone.Location = new System.Drawing.Point(16, 185);
            this.btnIOHeadphone.Name = "btnIOHeadphone";
            this.btnIOHeadphone.Size = new System.Drawing.Size(75, 23);
            this.btnIOHeadphone.TabIndex = 29;
            this.btnIOHeadphone.Text = "Headphone";
            this.btnIOHeadphone.Click += new System.EventHandler(this.btnIOHeadphone_Click);
            // 
            // btnIOMicPTT
            // 
            this.btnIOMicPTT.Image = null;
            this.btnIOMicPTT.Location = new System.Drawing.Point(16, 344);
            this.btnIOMicPTT.Name = "btnIOMicPTT";
            this.btnIOMicPTT.Size = new System.Drawing.Size(75, 23);
            this.btnIOMicPTT.TabIndex = 28;
            this.btnIOMicPTT.Text = "Mic PTT";
            this.btnIOMicPTT.Click += new System.EventHandler(this.btnIOMicPTT_Click);
            // 
            // btnIORCAPTT
            // 
            this.btnIORCAPTT.Image = null;
            this.btnIORCAPTT.Location = new System.Drawing.Point(16, 304);
            this.btnIORCAPTT.Name = "btnIORCAPTT";
            this.btnIORCAPTT.Size = new System.Drawing.Size(75, 23);
            this.btnIORCAPTT.TabIndex = 27;
            this.btnIORCAPTT.Text = "RCA PTT";
            this.btnIORCAPTT.Click += new System.EventHandler(this.btnIORCAPTT_Click);
            // 
            // btnIODash
            // 
            this.btnIODash.Image = null;
            this.btnIODash.Location = new System.Drawing.Point(16, 264);
            this.btnIODash.Name = "btnIODash";
            this.btnIODash.Size = new System.Drawing.Size(75, 23);
            this.btnIODash.TabIndex = 26;
            this.btnIODash.Text = "Dash";
            this.btnIODash.Click += new System.EventHandler(this.btnIODash_Click);
            // 
            // btnIODot
            // 
            this.btnIODot.Image = null;
            this.btnIODot.Location = new System.Drawing.Point(16, 224);
            this.btnIODot.Name = "btnIODot";
            this.btnIODot.Size = new System.Drawing.Size(75, 23);
            this.btnIODot.TabIndex = 25;
            this.btnIODot.Text = "Dot";
            this.btnIODot.Click += new System.EventHandler(this.btnIODot_Click);
            // 
            // btnIOFWInOut
            // 
            this.btnIOFWInOut.Image = null;
            this.btnIOFWInOut.Location = new System.Drawing.Point(16, 145);
            this.btnIOFWInOut.Name = "btnIOFWInOut";
            this.btnIOFWInOut.Size = new System.Drawing.Size(75, 23);
            this.btnIOFWInOut.TabIndex = 24;
            this.btnIOFWInOut.Text = "FW In/Out";
            this.btnIOFWInOut.Click += new System.EventHandler(this.btnIOFWInOut_Click);
            // 
            // btnIORCAInOut
            // 
            this.btnIORCAInOut.Image = null;
            this.btnIORCAInOut.Location = new System.Drawing.Point(16, 104);
            this.btnIORCAInOut.Name = "btnIORCAInOut";
            this.btnIORCAInOut.Size = new System.Drawing.Size(75, 23);
            this.btnIORCAInOut.TabIndex = 23;
            this.btnIORCAInOut.Text = "RCA In/Out";
            this.btnIORCAInOut.Click += new System.EventHandler(this.btnIORCAInOut_Click);
            // 
            // btnIOPwrSpkr
            // 
            this.btnIOPwrSpkr.Image = null;
            this.btnIOPwrSpkr.Location = new System.Drawing.Point(16, 64);
            this.btnIOPwrSpkr.Name = "btnIOPwrSpkr";
            this.btnIOPwrSpkr.Size = new System.Drawing.Size(75, 23);
            this.btnIOPwrSpkr.TabIndex = 22;
            this.btnIOPwrSpkr.Text = "PWR.SPKR";
            this.btnIOPwrSpkr.Click += new System.EventHandler(this.btnIOPwrSpkr_Click);
            // 
            // btnTestNone
            // 
            this.btnTestNone.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTestNone.Image = null;
            this.btnTestNone.Location = new System.Drawing.Point(104, 360);
            this.btnTestNone.Name = "btnTestNone";
            this.btnTestNone.Size = new System.Drawing.Size(40, 16);
            this.btnTestNone.TabIndex = 27;
            this.btnTestNone.Text = "None";
            this.btnTestNone.Click += new System.EventHandler(this.btnTestNone_Click);
            // 
            // btnTestAll
            // 
            this.btnTestAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTestAll.Image = null;
            this.btnTestAll.Location = new System.Drawing.Point(104, 344);
            this.btnTestAll.Name = "btnTestAll";
            this.btnTestAll.Size = new System.Drawing.Size(40, 16);
            this.btnTestAll.TabIndex = 26;
            this.btnTestAll.Text = "All";
            this.btnTestAll.Click += new System.EventHandler(this.btnTestAll_Click);
            // 
            // grpTestTransmitter
            // 
            this.grpTestTransmitter.Controls.Add(this.ckTestTXPA);
            this.grpTestTransmitter.Controls.Add(this.ckTestTXGain);
            this.grpTestTransmitter.Controls.Add(this.btnTestTXNone);
            this.grpTestTransmitter.Controls.Add(this.btnTestTXAll);
            this.grpTestTransmitter.Controls.Add(this.ckTestTXImage);
            this.grpTestTransmitter.Controls.Add(this.ckTestTXCarrier);
            this.grpTestTransmitter.Controls.Add(this.ckTestTXFilter);
            this.grpTestTransmitter.Location = new System.Drawing.Point(320, 384);
            this.grpTestTransmitter.Name = "grpTestTransmitter";
            this.grpTestTransmitter.Size = new System.Drawing.Size(144, 96);
            this.grpTestTransmitter.TabIndex = 23;
            this.grpTestTransmitter.TabStop = false;
            this.grpTestTransmitter.Text = "Transmitter";
            // 
            // ckTestTXPA
            // 
            this.ckTestTXPA.Checked = true;
            this.ckTestTXPA.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckTestTXPA.Image = null;
            this.ckTestTXPA.Location = new System.Drawing.Point(72, 49);
            this.ckTestTXPA.Name = "ckTestTXPA";
            this.ckTestTXPA.Size = new System.Drawing.Size(56, 24);
            this.ckTestTXPA.TabIndex = 29;
            this.ckTestTXPA.Text = "PA";
            this.ckTestTXPA.Visible = false;
            // 
            // ckTestTXGain
            // 
            this.ckTestTXGain.Image = null;
            this.ckTestTXGain.Location = new System.Drawing.Point(72, 48);
            this.ckTestTXGain.Name = "ckTestTXGain";
            this.ckTestTXGain.Size = new System.Drawing.Size(56, 24);
            this.ckTestTXGain.TabIndex = 28;
            this.ckTestTXGain.Text = "Gain";
            this.ckTestTXGain.Visible = false;
            // 
            // btnTestTXNone
            // 
            this.btnTestTXNone.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTestTXNone.Image = null;
            this.btnTestTXNone.Location = new System.Drawing.Point(76, 72);
            this.btnTestTXNone.Name = "btnTestTXNone";
            this.btnTestTXNone.Size = new System.Drawing.Size(40, 16);
            this.btnTestTXNone.TabIndex = 27;
            this.btnTestTXNone.Text = "None";
            this.btnTestTXNone.Click += new System.EventHandler(this.btnTestTXNone_Click);
            // 
            // btnTestTXAll
            // 
            this.btnTestTXAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTestTXAll.Image = null;
            this.btnTestTXAll.Location = new System.Drawing.Point(28, 72);
            this.btnTestTXAll.Name = "btnTestTXAll";
            this.btnTestTXAll.Size = new System.Drawing.Size(40, 16);
            this.btnTestTXAll.TabIndex = 26;
            this.btnTestTXAll.Text = "All";
            this.btnTestTXAll.Click += new System.EventHandler(this.btnTestTXAll_Click);
            // 
            // ckTestTXImage
            // 
            this.ckTestTXImage.Checked = true;
            this.ckTestTXImage.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckTestTXImage.Image = null;
            this.ckTestTXImage.Location = new System.Drawing.Point(72, 24);
            this.ckTestTXImage.Name = "ckTestTXImage";
            this.ckTestTXImage.Size = new System.Drawing.Size(56, 24);
            this.ckTestTXImage.TabIndex = 21;
            this.ckTestTXImage.Text = "Image";
            // 
            // ckTestTXCarrier
            // 
            this.ckTestTXCarrier.Checked = true;
            this.ckTestTXCarrier.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckTestTXCarrier.Image = null;
            this.ckTestTXCarrier.Location = new System.Drawing.Point(16, 48);
            this.ckTestTXCarrier.Name = "ckTestTXCarrier";
            this.ckTestTXCarrier.Size = new System.Drawing.Size(64, 24);
            this.ckTestTXCarrier.TabIndex = 20;
            this.ckTestTXCarrier.Text = "Carrier";
            // 
            // ckTestTXFilter
            // 
            this.ckTestTXFilter.Checked = true;
            this.ckTestTXFilter.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckTestTXFilter.Image = null;
            this.ckTestTXFilter.Location = new System.Drawing.Point(16, 24);
            this.ckTestTXFilter.Name = "ckTestTXFilter";
            this.ckTestTXFilter.Size = new System.Drawing.Size(48, 24);
            this.ckTestTXFilter.TabIndex = 19;
            this.ckTestTXFilter.Text = "Filter";
            // 
            // grpTestReceiver
            // 
            this.grpTestReceiver.Controls.Add(this.btnTestRXNone);
            this.grpTestReceiver.Controls.Add(this.btnTestRXAll);
            this.grpTestReceiver.Controls.Add(this.ckTestRXMDS);
            this.grpTestReceiver.Controls.Add(this.ckTestRXImage);
            this.grpTestReceiver.Controls.Add(this.ckTestRXLevel);
            this.grpTestReceiver.Controls.Add(this.ckTestRXFilter);
            this.grpTestReceiver.Location = new System.Drawing.Point(168, 384);
            this.grpTestReceiver.Name = "grpTestReceiver";
            this.grpTestReceiver.Size = new System.Drawing.Size(144, 96);
            this.grpTestReceiver.TabIndex = 22;
            this.grpTestReceiver.TabStop = false;
            this.grpTestReceiver.Text = "Receiver";
            // 
            // btnTestRXNone
            // 
            this.btnTestRXNone.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTestRXNone.Image = null;
            this.btnTestRXNone.Location = new System.Drawing.Point(76, 72);
            this.btnTestRXNone.Name = "btnTestRXNone";
            this.btnTestRXNone.Size = new System.Drawing.Size(40, 16);
            this.btnTestRXNone.TabIndex = 27;
            this.btnTestRXNone.Text = "None";
            this.btnTestRXNone.Click += new System.EventHandler(this.btnTestRXNone_Click);
            // 
            // btnTestRXAll
            // 
            this.btnTestRXAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTestRXAll.Image = null;
            this.btnTestRXAll.Location = new System.Drawing.Point(28, 72);
            this.btnTestRXAll.Name = "btnTestRXAll";
            this.btnTestRXAll.Size = new System.Drawing.Size(40, 16);
            this.btnTestRXAll.TabIndex = 26;
            this.btnTestRXAll.Text = "All";
            this.btnTestRXAll.Click += new System.EventHandler(this.btnTestRXAll_Click);
            // 
            // ckTestRXMDS
            // 
            this.ckTestRXMDS.Checked = true;
            this.ckTestRXMDS.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckTestRXMDS.Image = null;
            this.ckTestRXMDS.Location = new System.Drawing.Point(80, 48);
            this.ckTestRXMDS.Name = "ckTestRXMDS";
            this.ckTestRXMDS.Size = new System.Drawing.Size(56, 24);
            this.ckTestRXMDS.TabIndex = 22;
            this.ckTestRXMDS.Text = "MDS";
            this.ckTestRXMDS.Visible = false;
            // 
            // ckTestRXImage
            // 
            this.ckTestRXImage.Checked = true;
            this.ckTestRXImage.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckTestRXImage.Image = null;
            this.ckTestRXImage.Location = new System.Drawing.Point(16, 48);
            this.ckTestRXImage.Name = "ckTestRXImage";
            this.ckTestRXImage.Size = new System.Drawing.Size(56, 24);
            this.ckTestRXImage.TabIndex = 21;
            this.ckTestRXImage.Text = "Image";
            // 
            // ckTestRXLevel
            // 
            this.ckTestRXLevel.Checked = true;
            this.ckTestRXLevel.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckTestRXLevel.Image = null;
            this.ckTestRXLevel.Location = new System.Drawing.Point(80, 24);
            this.ckTestRXLevel.Name = "ckTestRXLevel";
            this.ckTestRXLevel.Size = new System.Drawing.Size(56, 24);
            this.ckTestRXLevel.TabIndex = 20;
            this.ckTestRXLevel.Text = "Level";
            // 
            // ckTestRXFilter
            // 
            this.ckTestRXFilter.Checked = true;
            this.ckTestRXFilter.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckTestRXFilter.Image = null;
            this.ckTestRXFilter.Location = new System.Drawing.Point(16, 24);
            this.ckTestRXFilter.Name = "ckTestRXFilter";
            this.ckTestRXFilter.Size = new System.Drawing.Size(48, 24);
            this.ckTestRXFilter.TabIndex = 19;
            this.ckTestRXFilter.Text = "Filter";
            // 
            // grpTestGeneral
            // 
            this.grpTestGeneral.Controls.Add(this.btnTestGenAll);
            this.grpTestGeneral.Controls.Add(this.ckTestGenPreamp);
            this.grpTestGeneral.Controls.Add(this.btnTestGenNone);
            this.grpTestGeneral.Controls.Add(this.ckTestGenImpulse);
            this.grpTestGeneral.Controls.Add(this.ckTestGenNoise);
            this.grpTestGeneral.Controls.Add(this.ckTestGenBal);
            this.grpTestGeneral.Controls.Add(this.ckTestGenPLL);
            this.grpTestGeneral.Controls.Add(this.ckTestGenATTN);
            this.grpTestGeneral.Location = new System.Drawing.Point(8, 384);
            this.grpTestGeneral.Name = "grpTestGeneral";
            this.grpTestGeneral.Size = new System.Drawing.Size(152, 96);
            this.grpTestGeneral.TabIndex = 21;
            this.grpTestGeneral.TabStop = false;
            this.grpTestGeneral.Text = "General";
            // 
            // btnTestGenAll
            // 
            this.btnTestGenAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTestGenAll.Image = null;
            this.btnTestGenAll.Location = new System.Drawing.Point(64, 72);
            this.btnTestGenAll.Name = "btnTestGenAll";
            this.btnTestGenAll.Size = new System.Drawing.Size(40, 16);
            this.btnTestGenAll.TabIndex = 24;
            this.btnTestGenAll.Text = "All";
            this.btnTestGenAll.Click += new System.EventHandler(this.btnTestGenAll_Click);
            // 
            // ckTestGenPreamp
            // 
            this.ckTestGenPreamp.Checked = true;
            this.ckTestGenPreamp.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckTestGenPreamp.Image = null;
            this.ckTestGenPreamp.Location = new System.Drawing.Point(8, 72);
            this.ckTestGenPreamp.Name = "ckTestGenPreamp";
            this.ckTestGenPreamp.Size = new System.Drawing.Size(64, 16);
            this.ckTestGenPreamp.TabIndex = 26;
            this.ckTestGenPreamp.Text = "Preamp";
            // 
            // btnTestGenNone
            // 
            this.btnTestGenNone.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTestGenNone.Image = null;
            this.btnTestGenNone.Location = new System.Drawing.Point(104, 72);
            this.btnTestGenNone.Name = "btnTestGenNone";
            this.btnTestGenNone.Size = new System.Drawing.Size(40, 16);
            this.btnTestGenNone.TabIndex = 25;
            this.btnTestGenNone.Text = "None";
            this.btnTestGenNone.Click += new System.EventHandler(this.btnTestGenNone_Click);
            // 
            // ckTestGenImpulse
            // 
            this.ckTestGenImpulse.Checked = true;
            this.ckTestGenImpulse.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckTestGenImpulse.Image = null;
            this.ckTestGenImpulse.Location = new System.Drawing.Point(80, 48);
            this.ckTestGenImpulse.Name = "ckTestGenImpulse";
            this.ckTestGenImpulse.Size = new System.Drawing.Size(64, 24);
            this.ckTestGenImpulse.TabIndex = 22;
            this.ckTestGenImpulse.Text = "Impulse";
            // 
            // ckTestGenNoise
            // 
            this.ckTestGenNoise.Image = null;
            this.ckTestGenNoise.Location = new System.Drawing.Point(16, 48);
            this.ckTestGenNoise.Name = "ckTestGenNoise";
            this.ckTestGenNoise.Size = new System.Drawing.Size(56, 24);
            this.ckTestGenNoise.TabIndex = 21;
            this.ckTestGenNoise.Text = "Noise";
            this.ckTestGenNoise.Visible = false;
            // 
            // ckTestGenBal
            // 
            this.ckTestGenBal.Checked = true;
            this.ckTestGenBal.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckTestGenBal.Image = null;
            this.ckTestGenBal.Location = new System.Drawing.Point(80, 24);
            this.ckTestGenBal.Name = "ckTestGenBal";
            this.ckTestGenBal.Size = new System.Drawing.Size(64, 24);
            this.ckTestGenBal.TabIndex = 20;
            this.ckTestGenBal.Text = "Gen/Bal";
            // 
            // ckTestGenPLL
            // 
            this.ckTestGenPLL.Checked = true;
            this.ckTestGenPLL.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckTestGenPLL.Image = null;
            this.ckTestGenPLL.Location = new System.Drawing.Point(16, 24);
            this.ckTestGenPLL.Name = "ckTestGenPLL";
            this.ckTestGenPLL.Size = new System.Drawing.Size(48, 24);
            this.ckTestGenPLL.TabIndex = 19;
            this.ckTestGenPLL.Text = "PLL";
            // 
            // ckTestGenATTN
            // 
            this.ckTestGenATTN.Image = null;
            this.ckTestGenATTN.Location = new System.Drawing.Point(16, 24);
            this.ckTestGenATTN.Name = "ckTestGenATTN";
            this.ckTestGenATTN.Size = new System.Drawing.Size(56, 24);
            this.ckTestGenATTN.TabIndex = 27;
            this.ckTestGenATTN.Text = "ATTN";
            // 
            // btnPrintReport
            // 
            this.btnPrintReport.Image = null;
            this.btnPrintReport.Location = new System.Drawing.Point(400, 352);
            this.btnPrintReport.Name = "btnPrintReport";
            this.btnPrintReport.Size = new System.Drawing.Size(75, 24);
            this.btnPrintReport.TabIndex = 20;
            this.btnPrintReport.Text = "Print Report";
            this.btnPrintReport.Click += new System.EventHandler(this.btnPrintReport_Click);
            // 
            // txtTech
            // 
            this.txtTech.Location = new System.Drawing.Point(280, 352);
            this.txtTech.Name = "txtTech";
            this.txtTech.Size = new System.Drawing.Size(100, 20);
            this.txtTech.TabIndex = 19;
            // 
            // lblTech
            // 
            this.lblTech.Image = null;
            this.lblTech.Location = new System.Drawing.Point(216, 352);
            this.lblTech.Name = "lblTech";
            this.lblTech.Size = new System.Drawing.Size(64, 23);
            this.lblTech.TabIndex = 18;
            this.lblTech.Text = "Technician:";
            // 
            // btnRunSelectedTests
            // 
            this.btnRunSelectedTests.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRunSelectedTests.Image = null;
            this.btnRunSelectedTests.Location = new System.Drawing.Point(8, 344);
            this.btnRunSelectedTests.Name = "btnRunSelectedTests";
            this.btnRunSelectedTests.Size = new System.Drawing.Size(88, 32);
            this.btnRunSelectedTests.TabIndex = 17;
            this.btnRunSelectedTests.Text = "Run Selected Tests";
            this.btnRunSelectedTests.Click += new System.EventHandler(this.btnRunSelectedTests_Click);
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
            this.grpBands.Location = new System.Drawing.Point(216, 8);
            this.grpBands.Name = "grpBands";
            this.grpBands.Size = new System.Drawing.Size(256, 104);
            this.grpBands.TabIndex = 15;
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
            // grpTransmitter
            // 
            this.grpTransmitter.Controls.Add(this.btnTX1500PA);
            this.grpTransmitter.Controls.Add(this.btnTXGain);
            this.grpTransmitter.Controls.Add(this.btnTXFilter);
            this.grpTransmitter.Controls.Add(this.btnTXCarrier);
            this.grpTransmitter.Controls.Add(this.btnTXImage);
            this.grpTransmitter.Location = new System.Drawing.Point(8, 232);
            this.grpTransmitter.Name = "grpTransmitter";
            this.grpTransmitter.Size = new System.Drawing.Size(200, 100);
            this.grpTransmitter.TabIndex = 4;
            this.grpTransmitter.TabStop = false;
            this.grpTransmitter.Text = "Transmitter";
            // 
            // btnTX1500PA
            // 
            this.btnTX1500PA.Image = null;
            this.btnTX1500PA.Location = new System.Drawing.Point(104, 64);
            this.btnTX1500PA.Name = "btnTX1500PA";
            this.btnTX1500PA.Size = new System.Drawing.Size(75, 23);
            this.btnTX1500PA.TabIndex = 10;
            this.btnTX1500PA.Text = "PA";
            this.btnTX1500PA.Visible = false;
            this.btnTX1500PA.Click += new System.EventHandler(this.btnTX1500PA_Click);
            // 
            // btnTXGain
            // 
            this.btnTXGain.Image = null;
            this.btnTXGain.Location = new System.Drawing.Point(104, 64);
            this.btnTXGain.Name = "btnTXGain";
            this.btnTXGain.Size = new System.Drawing.Size(75, 23);
            this.btnTXGain.TabIndex = 8;
            this.btnTXGain.Text = "Gain";
            this.btnTXGain.Visible = false;
            this.btnTXGain.Click += new System.EventHandler(this.btnTXGain_Click);
            // 
            // btnTXFilter
            // 
            this.btnTXFilter.Image = null;
            this.btnTXFilter.Location = new System.Drawing.Point(16, 24);
            this.btnTXFilter.Name = "btnTXFilter";
            this.btnTXFilter.Size = new System.Drawing.Size(75, 23);
            this.btnTXFilter.TabIndex = 7;
            this.btnTXFilter.Text = "Filter";
            this.btnTXFilter.Click += new System.EventHandler(this.btnTXFilter_Click);
            // 
            // btnTXCarrier
            // 
            this.btnTXCarrier.Image = null;
            this.btnTXCarrier.Location = new System.Drawing.Point(16, 64);
            this.btnTXCarrier.Name = "btnTXCarrier";
            this.btnTXCarrier.Size = new System.Drawing.Size(75, 23);
            this.btnTXCarrier.TabIndex = 6;
            this.btnTXCarrier.Text = "Carrier";
            this.btnTXCarrier.Click += new System.EventHandler(this.btnTXCarrier_Click);
            // 
            // btnTXImage
            // 
            this.btnTXImage.Image = null;
            this.btnTXImage.Location = new System.Drawing.Point(104, 24);
            this.btnTXImage.Name = "btnTXImage";
            this.btnTXImage.Size = new System.Drawing.Size(75, 23);
            this.btnTXImage.TabIndex = 5;
            this.btnTXImage.Text = "Image";
            this.btnTXImage.Click += new System.EventHandler(this.btnTXImage_Click);
            // 
            // grpGeneral
            // 
            this.grpGeneral.Controls.Add(this.btnGenATTN);
            this.grpGeneral.Controls.Add(this.btnGenPreamp);
            this.grpGeneral.Controls.Add(this.btnNoise);
            this.grpGeneral.Controls.Add(this.btnGenBal);
            this.grpGeneral.Controls.Add(this.btnPLL);
            this.grpGeneral.Controls.Add(this.btnImpulse);
            this.grpGeneral.Location = new System.Drawing.Point(8, 8);
            this.grpGeneral.Name = "grpGeneral";
            this.grpGeneral.Size = new System.Drawing.Size(184, 112);
            this.grpGeneral.TabIndex = 3;
            this.grpGeneral.TabStop = false;
            this.grpGeneral.Text = "General Tests";
            // 
            // grpReceiver
            // 
            this.grpReceiver.Controls.Add(this.udLevel);
            this.grpReceiver.Controls.Add(this.btnRXImage);
            this.grpReceiver.Controls.Add(this.btnRXFilter);
            this.grpReceiver.Controls.Add(this.btnRXLevel);
            this.grpReceiver.Location = new System.Drawing.Point(8, 120);
            this.grpReceiver.Name = "grpReceiver";
            this.grpReceiver.Size = new System.Drawing.Size(200, 104);
            this.grpReceiver.TabIndex = 2;
            this.grpReceiver.TabStop = false;
            this.grpReceiver.Text = "Receiver Tests";
            // 
            // udLevel
            // 
            this.udLevel.DecimalPlaces = 1;
            this.udLevel.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.udLevel.Location = new System.Drawing.Point(120, 64);
            this.udLevel.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.udLevel.Name = "udLevel";
            this.udLevel.Size = new System.Drawing.Size(64, 20);
            this.udLevel.TabIndex = 5;
            this.udLevel.Value = new decimal(new int[] {
            240,
            0,
            0,
            -2147418112});
            this.udLevel.Visible = false;
            // 
            // ckPM2
            // 
            this.ckPM2.Image = null;
            this.ckPM2.Location = new System.Drawing.Point(219, 325);
            this.ckPM2.Name = "ckPM2";
            this.ckPM2.Size = new System.Drawing.Size(56, 24);
            this.ckPM2.TabIndex = 32;
            this.ckPM2.Text = "PM2";
            this.toolTip1.SetToolTip(this.ckPM2, "Check for PowerMaster II");
            // 
            // FLEX5000ProdTestForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(592, 486);
            this.Controls.Add(this.ckPM2);
            this.Controls.Add(this.btnRunAll1500);
            this.Controls.Add(this.btnPostFence);
            this.Controls.Add(this.grpIO);
            this.Controls.Add(this.btnTestNone);
            this.Controls.Add(this.btnTestAll);
            this.Controls.Add(this.grpTestTransmitter);
            this.Controls.Add(this.grpTestReceiver);
            this.Controls.Add(this.grpTestGeneral);
            this.Controls.Add(this.btnPrintReport);
            this.Controls.Add(this.txtTech);
            this.Controls.Add(this.lblTech);
            this.Controls.Add(this.btnRunSelectedTests);
            this.Controls.Add(this.lstDebug);
            this.Controls.Add(this.grpBands);
            this.Controls.Add(this.grpTransmitter);
            this.Controls.Add(this.grpGeneral);
            this.Controls.Add(this.grpReceiver);
            this.Controls.Add(this.checkGENBAL);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "FLEX5000ProdTestForm";
            this.Text = "FLEX-5000 Production TRX / IO Test";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.FLEX5000ProdTestForm_Closing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FLEX5000ProdTestForm_KeyDown);
            this.grpIO.ResumeLayout(false);
            this.grpComPort.ResumeLayout(false);
            this.grpTestTransmitter.ResumeLayout(false);
            this.grpTestReceiver.ResumeLayout(false);
            this.grpTestGeneral.ResumeLayout(false);
            this.grpBands.ResumeLayout(false);
            this.grpTransmitter.ResumeLayout(false);
            this.grpGeneral.ResumeLayout(false);
            this.grpReceiver.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.udLevel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

       
        private System.Windows.Forms.ButtonTS btnPLL;
        private System.Windows.Forms.ButtonTS btnRXFilter;
        private System.Windows.Forms.GroupBoxTS grpReceiver;
        private System.Windows.Forms.ButtonTS btnRXLevel;
        private System.Windows.Forms.GroupBoxTS grpGeneral;
        private System.Windows.Forms.GroupBoxTS grpTransmitter;
        private System.Windows.Forms.ButtonTS btnTXImage;
        private System.Windows.Forms.ButtonTS btnTXCarrier;
        private System.Windows.Forms.ButtonTS btnNoise;
        private System.Windows.Forms.ButtonTS btnTXFilter;
        private System.Windows.Forms.ButtonTS btnImpulse;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBoxTS grpBands;
        private System.Windows.Forms.ButtonTS btnClearAll;
        private System.Windows.Forms.ButtonTS btnCheckAll;
        private System.Windows.Forms.ButtonTS btnRXImage;
        private System.Windows.Forms.ListBox lstDebug;
        private System.Windows.Forms.LabelTS lblTech;
        private System.Windows.Forms.TextBoxTS txtTech;
        private System.Windows.Forms.ButtonTS btnPrintReport;
        private System.Windows.Forms.PrintPreviewDialog printPreviewDialog1;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.ButtonTS btnRunSelectedTests;
        private System.Windows.Forms.GroupBoxTS grpTestGeneral;
        private System.Windows.Forms.GroupBoxTS grpTestReceiver;
        private System.Windows.Forms.GroupBoxTS grpTestTransmitter;
        private System.Windows.Forms.ButtonTS btnTestGenAll;
        private System.Windows.Forms.ButtonTS btnTestGenNone;
        private System.Windows.Forms.ButtonTS btnTestRXNone;
        private System.Windows.Forms.ButtonTS btnTestRXAll;
        private System.Windows.Forms.ButtonTS btnTestTXNone;
        private System.Windows.Forms.ButtonTS btnTestTXAll;
        private System.Windows.Forms.ButtonTS btnTestNone;
        private System.Windows.Forms.ButtonTS btnTestAll;
        private System.Windows.Forms.GroupBoxTS grpIO;
        private System.Windows.Forms.ButtonTS btnIORunAll;
        private System.Windows.Forms.ButtonTS btnIOExtRef;
        private System.Windows.Forms.ButtonTS btnIOHeadphone;
        private System.Windows.Forms.ButtonTS btnIOMicPTT;
        private System.Windows.Forms.ButtonTS btnIORCAPTT;
        private System.Windows.Forms.ButtonTS btnIODash;
        private System.Windows.Forms.ButtonTS btnIODot;
        private System.Windows.Forms.ButtonTS btnIOFWInOut;
        private System.Windows.Forms.ButtonTS btnIORCAInOut;
        private System.Windows.Forms.ButtonTS btnIOPwrSpkr;
        private System.Windows.Forms.ButtonTS btnGenPreamp;
        private System.Windows.Forms.ButtonTS btnGenBal;
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
        private System.Windows.Forms.CheckBoxTS ckTestGenPreamp;
        private System.Windows.Forms.CheckBoxTS ckTestGenImpulse;
        private System.Windows.Forms.CheckBoxTS ckTestGenNoise;
        private System.Windows.Forms.CheckBoxTS ckTestGenBal;
        private System.Windows.Forms.CheckBoxTS ckTestGenPLL;
        private System.Windows.Forms.CheckBoxTS ckTestRXMDS;
        private System.Windows.Forms.CheckBoxTS ckTestRXImage;
        private System.Windows.Forms.CheckBoxTS ckTestRXLevel;
        private System.Windows.Forms.CheckBoxTS ckTestRXFilter;
        private System.Windows.Forms.CheckBoxTS ckTestTXImage;
        private System.Windows.Forms.CheckBoxTS ckTestTXCarrier;
        private System.Windows.Forms.CheckBoxTS ckTestTXFilter;
        private System.Windows.Forms.ButtonTS btnPostFence;
        private System.Windows.Forms.ButtonTS btnTXGain;
        private System.Windows.Forms.CheckBoxTS ckTestTXGain;
        private System.Windows.Forms.NumericUpDown udLevel;
        private System.Windows.Forms.ButtonTS btnGenATTN;
        private System.Windows.Forms.CheckBoxTS ckTestGenATTN;
        private System.Windows.Forms.ButtonTS btnIOMicUp;
        private System.Windows.Forms.ButtonTS btnIOMicDown;
        private System.Windows.Forms.ButtonTS btnIOMicFast;
        private System.Windows.Forms.ButtonTS btnTX1500PA;
        private System.Windows.Forms.GroupBoxTS grpComPort;
        private System.Windows.Forms.ComboBoxTS comboCOMPort;
        private System.Windows.Forms.ButtonTS btnIOFWPTT;
        private System.Windows.Forms.CheckBoxTS ckTestTXPA;
        private System.Windows.Forms.ButtonTS btnRunAll1500;
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.CheckBoxTS checkGENBAL;
        public System.Windows.Forms.CheckBoxTS ckPM2;
        
      

    }
}