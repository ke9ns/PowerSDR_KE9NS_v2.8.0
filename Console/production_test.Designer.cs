//=================================================================
// production_test.cs
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
    /// Summary description for production_test.
    /// </summary>
    public partial class ProductionTest : System.Windows.Forms.Form
    {
        

        

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProductionTest));
            this.txtRefSig160 = new System.Windows.Forms.TextBoxTS();
            this.txtSignal160H = new System.Windows.Forms.TextBoxTS();
            this.lblBand160 = new System.Windows.Forms.LabelTS();
            this.lblBand80 = new System.Windows.Forms.LabelTS();
            this.txtSignal80H = new System.Windows.Forms.TextBoxTS();
            this.txtRefSig80 = new System.Windows.Forms.TextBoxTS();
            this.lblBand60 = new System.Windows.Forms.LabelTS();
            this.txtSignal60H = new System.Windows.Forms.TextBoxTS();
            this.txtRefSig60 = new System.Windows.Forms.TextBoxTS();
            this.lblBand40 = new System.Windows.Forms.LabelTS();
            this.txtSignal40H = new System.Windows.Forms.TextBoxTS();
            this.txtRefSig40 = new System.Windows.Forms.TextBoxTS();
            this.lblBand30 = new System.Windows.Forms.LabelTS();
            this.txtSignal30H = new System.Windows.Forms.TextBoxTS();
            this.txtRefSig30 = new System.Windows.Forms.TextBoxTS();
            this.lblBand20 = new System.Windows.Forms.LabelTS();
            this.txtNoise20 = new System.Windows.Forms.TextBoxTS();
            this.txtSignal20H = new System.Windows.Forms.TextBoxTS();
            this.txtRefNoise20 = new System.Windows.Forms.TextBoxTS();
            this.txtRefSig20 = new System.Windows.Forms.TextBoxTS();
            this.lblBand17 = new System.Windows.Forms.LabelTS();
            this.txtSignal17H = new System.Windows.Forms.TextBoxTS();
            this.txtRefSig17 = new System.Windows.Forms.TextBoxTS();
            this.lblBand15 = new System.Windows.Forms.LabelTS();
            this.txtSignal15H = new System.Windows.Forms.TextBoxTS();
            this.txtRefSig15 = new System.Windows.Forms.TextBoxTS();
            this.lblBand12 = new System.Windows.Forms.LabelTS();
            this.txtSignal12H = new System.Windows.Forms.TextBoxTS();
            this.txtRefSig12 = new System.Windows.Forms.TextBoxTS();
            this.lblBand10 = new System.Windows.Forms.LabelTS();
            this.txtSignal10H = new System.Windows.Forms.TextBoxTS();
            this.txtRefSig10 = new System.Windows.Forms.TextBoxTS();
            this.lblBand6 = new System.Windows.Forms.LabelTS();
            this.txtSignal6H = new System.Windows.Forms.TextBoxTS();
            this.txtRefSig6 = new System.Windows.Forms.TextBoxTS();
            this.lblNoise = new System.Windows.Forms.LabelTS();
            this.lblSigRef = new System.Windows.Forms.LabelTS();
            this.grpSignal = new System.Windows.Forms.GroupBoxTS();
            this.labelTS1 = new System.Windows.Forms.LabelTS();
            this.txtSigDelta60 = new System.Windows.Forms.TextBoxTS();
            this.txtSigDelta17 = new System.Windows.Forms.TextBoxTS();
            this.txtSigDelta6 = new System.Windows.Forms.TextBoxTS();
            this.txtSigDelta40 = new System.Windows.Forms.TextBoxTS();
            this.txtSigDelta160 = new System.Windows.Forms.TextBoxTS();
            this.txtSigDelta80 = new System.Windows.Forms.TextBoxTS();
            this.txtSigDelta12 = new System.Windows.Forms.TextBoxTS();
            this.txtSigDelta20 = new System.Windows.Forms.TextBoxTS();
            this.txtSigDelta10 = new System.Windows.Forms.TextBoxTS();
            this.txtSigDelta15 = new System.Windows.Forms.TextBoxTS();
            this.txtSigDelta30 = new System.Windows.Forms.TextBoxTS();
            this.lblSignalActual = new System.Windows.Forms.LabelTS();
            this.grpNoise = new System.Windows.Forms.GroupBoxTS();
            this.lblNoiseRef = new System.Windows.Forms.LabelTS();
            this.lblBand = new System.Windows.Forms.LabelTS();
            this.grpPreamp = new System.Windows.Forms.GroupBoxTS();
            this.txtPreAtt = new System.Windows.Forms.TextBoxTS();
            this.lblPreNoise = new System.Windows.Forms.LabelTS();
            this.lblPreSignal = new System.Windows.Forms.LabelTS();
            this.txtPreGain = new System.Windows.Forms.TextBoxTS();
            this.grpTests = new System.Windows.Forms.GroupBoxTS();
            this.btnRunPTT = new System.Windows.Forms.ButtonTS();
            this.btnRunMic = new System.Windows.Forms.ButtonTS();
            this.btnRunBalance = new System.Windows.Forms.ButtonTS();
            this.btnRunRFE = new System.Windows.Forms.ButtonTS();
            this.btnRunTX = new System.Windows.Forms.ButtonTS();
            this.chkRunDash = new System.Windows.Forms.CheckBox();
            this.chkRunDot = new System.Windows.Forms.CheckBox();
            this.btnRunImpulse = new System.Windows.Forms.ButtonTS();
            this.btnClearAll = new System.Windows.Forms.ButtonTS();
            this.btnCheckAll = new System.Windows.Forms.ButtonTS();
            this.btnRunPreamp = new System.Windows.Forms.ButtonTS();
            this.btnRunNoise = new System.Windows.Forms.ButtonTS();
            this.btnRunSignal = new System.Windows.Forms.ButtonTS();
            this.lblSkipCheckedBands = new System.Windows.Forms.LabelTS();
            this.chk6 = new System.Windows.Forms.CheckBoxTS();
            this.chk10 = new System.Windows.Forms.CheckBoxTS();
            this.chk12 = new System.Windows.Forms.CheckBoxTS();
            this.chk15 = new System.Windows.Forms.CheckBoxTS();
            this.chk17 = new System.Windows.Forms.CheckBoxTS();
            this.chk20 = new System.Windows.Forms.CheckBoxTS();
            this.chk30 = new System.Windows.Forms.CheckBoxTS();
            this.chk40 = new System.Windows.Forms.CheckBoxTS();
            this.chk60 = new System.Windows.Forms.CheckBoxTS();
            this.chk80 = new System.Windows.Forms.CheckBoxTS();
            this.chk160 = new System.Windows.Forms.CheckBoxTS();
            this.btnRunAllTests = new System.Windows.Forms.ButtonTS();
            this.grpGenerator = new System.Windows.Forms.GroupBoxTS();
            this.udGenClockCorr = new System.Windows.Forms.NumericUpDownTS();
            this.label1 = new System.Windows.Forms.LabelTS();
            this.btnGenReset = new System.Windows.Forms.ButtonTS();
            this.udGenLevel = new System.Windows.Forms.NumericUpDownTS();
            this.lblGenLevel = new System.Windows.Forms.LabelTS();
            this.udGenFreq = new System.Windows.Forms.NumericUpDownTS();
            this.lblGenFreq = new System.Windows.Forms.LabelTS();
            this.grpTolerance = new System.Windows.Forms.GroupBoxTS();
            this.txtTolSigHigh = new System.Windows.Forms.TextBoxTS();
            this.labelTS3 = new System.Windows.Forms.LabelTS();
            this.txtTolSigLow = new System.Windows.Forms.TextBoxTS();
            this.lblTolBalance = new System.Windows.Forms.LabelTS();
            this.txtTolBalance = new System.Windows.Forms.TextBoxTS();
            this.lblTolImpulse = new System.Windows.Forms.LabelTS();
            this.txtTolImpulse = new System.Windows.Forms.TextBoxTS();
            this.lblTolPreamp = new System.Windows.Forms.LabelTS();
            this.txtTolPreamp = new System.Windows.Forms.TextBoxTS();
            this.txtTolNoise = new System.Windows.Forms.TextBoxTS();
            this.lblTolNoise = new System.Windows.Forms.LabelTS();
            this.lblTolSigHigh = new System.Windows.Forms.LabelTS();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.mnuDebug = new System.Windows.Forms.MenuItem();
            this.groupBoxTS1 = new System.Windows.Forms.GroupBoxTS();
            this.udTXTestOffTime = new System.Windows.Forms.NumericUpDownTS();
            this.udTXTestOnTime = new System.Windows.Forms.NumericUpDownTS();
            this.lblTXTestOnTime = new System.Windows.Forms.LabelTS();
            this.labelTS2 = new System.Windows.Forms.LabelTS();
            this.btnPrintResults = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSerialNum = new System.Windows.Forms.TextBoxTS();
            this.txtComments = new System.Windows.Forms.TextBoxTS();
            this.label3 = new System.Windows.Forms.Label();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            this.printPreviewDialog1 = new System.Windows.Forms.PrintPreviewDialog();
            this.labelTS4 = new System.Windows.Forms.LabelTS();
            this.txtTolMic = new System.Windows.Forms.TextBoxTS();
            this.grpSignal.SuspendLayout();
            this.grpNoise.SuspendLayout();
            this.grpPreamp.SuspendLayout();
            this.grpTests.SuspendLayout();
            this.grpGenerator.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udGenClockCorr)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udGenLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udGenFreq)).BeginInit();
            this.grpTolerance.SuspendLayout();
            this.groupBoxTS1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udTXTestOffTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udTXTestOnTime)).BeginInit();
            this.SuspendLayout();
            // 
            // txtRefSig160
            // 
            this.txtRefSig160.Location = new System.Drawing.Point(64, 24);
            this.txtRefSig160.Name = "txtRefSig160";
            this.txtRefSig160.Size = new System.Drawing.Size(48, 20);
            this.txtRefSig160.TabIndex = 0;
            this.txtRefSig160.Text = "-65.8";
            // 
            // txtSignal160H
            // 
            this.txtSignal160H.Location = new System.Drawing.Point(64, 48);
            this.txtSignal160H.Name = "txtSignal160H";
            this.txtSignal160H.ReadOnly = true;
            this.txtSignal160H.Size = new System.Drawing.Size(48, 20);
            this.txtSignal160H.TabIndex = 2;
            // 
            // lblBand160
            // 
            this.lblBand160.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBand160.Image = null;
            this.lblBand160.Location = new System.Drawing.Point(80, 112);
            this.lblBand160.Name = "lblBand160";
            this.lblBand160.Size = new System.Drawing.Size(48, 16);
            this.lblBand160.TabIndex = 6;
            this.lblBand160.Text = "160";
            this.lblBand160.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblBand80
            // 
            this.lblBand80.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBand80.Image = null;
            this.lblBand80.Location = new System.Drawing.Point(136, 112);
            this.lblBand80.Name = "lblBand80";
            this.lblBand80.Size = new System.Drawing.Size(48, 16);
            this.lblBand80.TabIndex = 13;
            this.lblBand80.Text = "80";
            this.lblBand80.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // txtSignal80H
            // 
            this.txtSignal80H.Location = new System.Drawing.Point(120, 48);
            this.txtSignal80H.Name = "txtSignal80H";
            this.txtSignal80H.ReadOnly = true;
            this.txtSignal80H.Size = new System.Drawing.Size(48, 20);
            this.txtSignal80H.TabIndex = 9;
            // 
            // txtRefSig80
            // 
            this.txtRefSig80.Location = new System.Drawing.Point(120, 24);
            this.txtRefSig80.Name = "txtRefSig80";
            this.txtRefSig80.Size = new System.Drawing.Size(48, 20);
            this.txtRefSig80.TabIndex = 7;
            this.txtRefSig80.Text = "-64.3";
            // 
            // lblBand60
            // 
            this.lblBand60.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBand60.Image = null;
            this.lblBand60.Location = new System.Drawing.Point(192, 112);
            this.lblBand60.Name = "lblBand60";
            this.lblBand60.Size = new System.Drawing.Size(48, 16);
            this.lblBand60.TabIndex = 20;
            this.lblBand60.Text = "60";
            this.lblBand60.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // txtSignal60H
            // 
            this.txtSignal60H.Location = new System.Drawing.Point(176, 48);
            this.txtSignal60H.Name = "txtSignal60H";
            this.txtSignal60H.ReadOnly = true;
            this.txtSignal60H.Size = new System.Drawing.Size(48, 20);
            this.txtSignal60H.TabIndex = 16;
            // 
            // txtRefSig60
            // 
            this.txtRefSig60.Location = new System.Drawing.Point(176, 24);
            this.txtRefSig60.Name = "txtRefSig60";
            this.txtRefSig60.Size = new System.Drawing.Size(48, 20);
            this.txtRefSig60.TabIndex = 14;
            this.txtRefSig60.Text = "-65.1";
            // 
            // lblBand40
            // 
            this.lblBand40.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBand40.Image = null;
            this.lblBand40.Location = new System.Drawing.Point(248, 112);
            this.lblBand40.Name = "lblBand40";
            this.lblBand40.Size = new System.Drawing.Size(48, 16);
            this.lblBand40.TabIndex = 27;
            this.lblBand40.Text = "40";
            this.lblBand40.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // txtSignal40H
            // 
            this.txtSignal40H.Location = new System.Drawing.Point(232, 48);
            this.txtSignal40H.Name = "txtSignal40H";
            this.txtSignal40H.ReadOnly = true;
            this.txtSignal40H.Size = new System.Drawing.Size(48, 20);
            this.txtSignal40H.TabIndex = 23;
            // 
            // txtRefSig40
            // 
            this.txtRefSig40.Location = new System.Drawing.Point(232, 24);
            this.txtRefSig40.Name = "txtRefSig40";
            this.txtRefSig40.Size = new System.Drawing.Size(48, 20);
            this.txtRefSig40.TabIndex = 21;
            this.txtRefSig40.Text = "-64.2";
            // 
            // lblBand30
            // 
            this.lblBand30.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBand30.Image = null;
            this.lblBand30.Location = new System.Drawing.Point(304, 112);
            this.lblBand30.Name = "lblBand30";
            this.lblBand30.Size = new System.Drawing.Size(48, 16);
            this.lblBand30.TabIndex = 34;
            this.lblBand30.Text = "30";
            this.lblBand30.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // txtSignal30H
            // 
            this.txtSignal30H.Location = new System.Drawing.Point(288, 48);
            this.txtSignal30H.Name = "txtSignal30H";
            this.txtSignal30H.ReadOnly = true;
            this.txtSignal30H.Size = new System.Drawing.Size(48, 20);
            this.txtSignal30H.TabIndex = 30;
            // 
            // txtRefSig30
            // 
            this.txtRefSig30.Location = new System.Drawing.Point(288, 24);
            this.txtRefSig30.Name = "txtRefSig30";
            this.txtRefSig30.Size = new System.Drawing.Size(48, 20);
            this.txtRefSig30.TabIndex = 28;
            this.txtRefSig30.Text = "-65.0";
            // 
            // lblBand20
            // 
            this.lblBand20.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBand20.Image = null;
            this.lblBand20.Location = new System.Drawing.Point(360, 112);
            this.lblBand20.Name = "lblBand20";
            this.lblBand20.Size = new System.Drawing.Size(48, 16);
            this.lblBand20.TabIndex = 41;
            this.lblBand20.Text = "20";
            this.lblBand20.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // txtNoise20
            // 
            this.txtNoise20.Location = new System.Drawing.Point(56, 48);
            this.txtNoise20.Name = "txtNoise20";
            this.txtNoise20.ReadOnly = true;
            this.txtNoise20.Size = new System.Drawing.Size(48, 20);
            this.txtNoise20.TabIndex = 40;
            // 
            // txtSignal20H
            // 
            this.txtSignal20H.Location = new System.Drawing.Point(344, 48);
            this.txtSignal20H.Name = "txtSignal20H";
            this.txtSignal20H.ReadOnly = true;
            this.txtSignal20H.Size = new System.Drawing.Size(48, 20);
            this.txtSignal20H.TabIndex = 37;
            // 
            // txtRefNoise20
            // 
            this.txtRefNoise20.Location = new System.Drawing.Point(56, 24);
            this.txtRefNoise20.Name = "txtRefNoise20";
            this.txtRefNoise20.Size = new System.Drawing.Size(48, 20);
            this.txtRefNoise20.TabIndex = 36;
            this.txtRefNoise20.Text = "-125.0";
            // 
            // txtRefSig20
            // 
            this.txtRefSig20.Location = new System.Drawing.Point(344, 24);
            this.txtRefSig20.Name = "txtRefSig20";
            this.txtRefSig20.Size = new System.Drawing.Size(48, 20);
            this.txtRefSig20.TabIndex = 35;
            this.txtRefSig20.Text = "-64.5";
            // 
            // lblBand17
            // 
            this.lblBand17.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBand17.Image = null;
            this.lblBand17.Location = new System.Drawing.Point(416, 112);
            this.lblBand17.Name = "lblBand17";
            this.lblBand17.Size = new System.Drawing.Size(48, 16);
            this.lblBand17.TabIndex = 48;
            this.lblBand17.Text = "17";
            this.lblBand17.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // txtSignal17H
            // 
            this.txtSignal17H.Location = new System.Drawing.Point(400, 48);
            this.txtSignal17H.Name = "txtSignal17H";
            this.txtSignal17H.ReadOnly = true;
            this.txtSignal17H.Size = new System.Drawing.Size(48, 20);
            this.txtSignal17H.TabIndex = 44;
            // 
            // txtRefSig17
            // 
            this.txtRefSig17.Location = new System.Drawing.Point(400, 24);
            this.txtRefSig17.Name = "txtRefSig17";
            this.txtRefSig17.Size = new System.Drawing.Size(48, 20);
            this.txtRefSig17.TabIndex = 42;
            this.txtRefSig17.Text = "-64.8";
            // 
            // lblBand15
            // 
            this.lblBand15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBand15.Image = null;
            this.lblBand15.Location = new System.Drawing.Point(472, 112);
            this.lblBand15.Name = "lblBand15";
            this.lblBand15.Size = new System.Drawing.Size(48, 16);
            this.lblBand15.TabIndex = 55;
            this.lblBand15.Text = "15";
            this.lblBand15.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // txtSignal15H
            // 
            this.txtSignal15H.Location = new System.Drawing.Point(456, 48);
            this.txtSignal15H.Name = "txtSignal15H";
            this.txtSignal15H.ReadOnly = true;
            this.txtSignal15H.Size = new System.Drawing.Size(48, 20);
            this.txtSignal15H.TabIndex = 51;
            // 
            // txtRefSig15
            // 
            this.txtRefSig15.Location = new System.Drawing.Point(456, 24);
            this.txtRefSig15.Name = "txtRefSig15";
            this.txtRefSig15.Size = new System.Drawing.Size(48, 20);
            this.txtRefSig15.TabIndex = 49;
            this.txtRefSig15.Text = "-66.2";
            // 
            // lblBand12
            // 
            this.lblBand12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBand12.Image = null;
            this.lblBand12.Location = new System.Drawing.Point(528, 112);
            this.lblBand12.Name = "lblBand12";
            this.lblBand12.Size = new System.Drawing.Size(48, 16);
            this.lblBand12.TabIndex = 62;
            this.lblBand12.Text = "12";
            this.lblBand12.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // txtSignal12H
            // 
            this.txtSignal12H.Location = new System.Drawing.Point(512, 48);
            this.txtSignal12H.Name = "txtSignal12H";
            this.txtSignal12H.ReadOnly = true;
            this.txtSignal12H.Size = new System.Drawing.Size(48, 20);
            this.txtSignal12H.TabIndex = 58;
            // 
            // txtRefSig12
            // 
            this.txtRefSig12.Location = new System.Drawing.Point(512, 24);
            this.txtRefSig12.Name = "txtRefSig12";
            this.txtRefSig12.Size = new System.Drawing.Size(48, 20);
            this.txtRefSig12.TabIndex = 56;
            this.txtRefSig12.Text = "-65.9";
            // 
            // lblBand10
            // 
            this.lblBand10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBand10.Image = null;
            this.lblBand10.Location = new System.Drawing.Point(584, 112);
            this.lblBand10.Name = "lblBand10";
            this.lblBand10.Size = new System.Drawing.Size(48, 16);
            this.lblBand10.TabIndex = 69;
            this.lblBand10.Text = "10";
            this.lblBand10.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // txtSignal10H
            // 
            this.txtSignal10H.Location = new System.Drawing.Point(568, 48);
            this.txtSignal10H.Name = "txtSignal10H";
            this.txtSignal10H.ReadOnly = true;
            this.txtSignal10H.Size = new System.Drawing.Size(48, 20);
            this.txtSignal10H.TabIndex = 65;
            // 
            // txtRefSig10
            // 
            this.txtRefSig10.Location = new System.Drawing.Point(568, 24);
            this.txtRefSig10.Name = "txtRefSig10";
            this.txtRefSig10.Size = new System.Drawing.Size(48, 20);
            this.txtRefSig10.TabIndex = 63;
            this.txtRefSig10.Text = "-66.7";
            // 
            // lblBand6
            // 
            this.lblBand6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBand6.Image = null;
            this.lblBand6.Location = new System.Drawing.Point(640, 112);
            this.lblBand6.Name = "lblBand6";
            this.lblBand6.Size = new System.Drawing.Size(48, 16);
            this.lblBand6.TabIndex = 76;
            this.lblBand6.Text = "6";
            this.lblBand6.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // txtSignal6H
            // 
            this.txtSignal6H.Location = new System.Drawing.Point(624, 48);
            this.txtSignal6H.Name = "txtSignal6H";
            this.txtSignal6H.ReadOnly = true;
            this.txtSignal6H.Size = new System.Drawing.Size(48, 20);
            this.txtSignal6H.TabIndex = 72;
            // 
            // txtRefSig6
            // 
            this.txtRefSig6.Location = new System.Drawing.Point(624, 24);
            this.txtRefSig6.Name = "txtRefSig6";
            this.txtRefSig6.Size = new System.Drawing.Size(48, 20);
            this.txtRefSig6.TabIndex = 70;
            this.txtRefSig6.Text = "-67.6";
            // 
            // lblNoise
            // 
            this.lblNoise.Image = null;
            this.lblNoise.Location = new System.Drawing.Point(16, 48);
            this.lblNoise.Name = "lblNoise";
            this.lblNoise.Size = new System.Drawing.Size(40, 23);
            this.lblNoise.TabIndex = 73;
            this.lblNoise.Text = "Noise:";
            // 
            // lblSigRef
            // 
            this.lblSigRef.Image = null;
            this.lblSigRef.Location = new System.Drawing.Point(16, 24);
            this.lblSigRef.Name = "lblSigRef";
            this.lblSigRef.Size = new System.Drawing.Size(40, 23);
            this.lblSigRef.TabIndex = 72;
            this.lblSigRef.Text = "Ref:";
            // 
            // grpSignal
            // 
            this.grpSignal.Controls.Add(this.labelTS1);
            this.grpSignal.Controls.Add(this.txtSigDelta60);
            this.grpSignal.Controls.Add(this.txtSigDelta17);
            this.grpSignal.Controls.Add(this.txtSigDelta6);
            this.grpSignal.Controls.Add(this.txtSigDelta40);
            this.grpSignal.Controls.Add(this.txtSigDelta160);
            this.grpSignal.Controls.Add(this.txtSigDelta80);
            this.grpSignal.Controls.Add(this.txtSigDelta12);
            this.grpSignal.Controls.Add(this.txtSigDelta20);
            this.grpSignal.Controls.Add(this.txtSigDelta10);
            this.grpSignal.Controls.Add(this.txtSigDelta15);
            this.grpSignal.Controls.Add(this.txtSigDelta30);
            this.grpSignal.Controls.Add(this.lblSignalActual);
            this.grpSignal.Controls.Add(this.txtSignal60H);
            this.grpSignal.Controls.Add(this.txtSignal17H);
            this.grpSignal.Controls.Add(this.txtSignal6H);
            this.grpSignal.Controls.Add(this.txtSignal40H);
            this.grpSignal.Controls.Add(this.txtSignal160H);
            this.grpSignal.Controls.Add(this.txtSignal80H);
            this.grpSignal.Controls.Add(this.txtSignal12H);
            this.grpSignal.Controls.Add(this.txtSignal20H);
            this.grpSignal.Controls.Add(this.txtSignal10H);
            this.grpSignal.Controls.Add(this.txtSignal15H);
            this.grpSignal.Controls.Add(this.txtSignal30H);
            this.grpSignal.Controls.Add(this.txtRefSig40);
            this.grpSignal.Controls.Add(this.txtRefSig6);
            this.grpSignal.Controls.Add(this.lblSigRef);
            this.grpSignal.Controls.Add(this.txtRefSig30);
            this.grpSignal.Controls.Add(this.txtRefSig160);
            this.grpSignal.Controls.Add(this.txtRefSig20);
            this.grpSignal.Controls.Add(this.txtRefSig17);
            this.grpSignal.Controls.Add(this.txtRefSig15);
            this.grpSignal.Controls.Add(this.txtRefSig80);
            this.grpSignal.Controls.Add(this.txtRefSig12);
            this.grpSignal.Controls.Add(this.txtRefSig60);
            this.grpSignal.Controls.Add(this.txtRefSig10);
            this.grpSignal.Location = new System.Drawing.Point(16, 136);
            this.grpSignal.Name = "grpSignal";
            this.grpSignal.Size = new System.Drawing.Size(688, 104);
            this.grpSignal.TabIndex = 78;
            this.grpSignal.TabStop = false;
            this.grpSignal.Text = "Measured Signal (dBm)";
            // 
            // labelTS1
            // 
            this.labelTS1.Image = null;
            this.labelTS1.Location = new System.Drawing.Point(16, 72);
            this.labelTS1.Name = "labelTS1";
            this.labelTS1.Size = new System.Drawing.Size(40, 23);
            this.labelTS1.TabIndex = 87;
            this.labelTS1.Text = "Delta:";
            // 
            // txtSigDelta60
            // 
            this.txtSigDelta60.Location = new System.Drawing.Point(176, 72);
            this.txtSigDelta60.Name = "txtSigDelta60";
            this.txtSigDelta60.ReadOnly = true;
            this.txtSigDelta60.Size = new System.Drawing.Size(48, 20);
            this.txtSigDelta60.TabIndex = 78;
            // 
            // txtSigDelta17
            // 
            this.txtSigDelta17.Location = new System.Drawing.Point(400, 72);
            this.txtSigDelta17.Name = "txtSigDelta17";
            this.txtSigDelta17.ReadOnly = true;
            this.txtSigDelta17.Size = new System.Drawing.Size(48, 20);
            this.txtSigDelta17.TabIndex = 82;
            // 
            // txtSigDelta6
            // 
            this.txtSigDelta6.Location = new System.Drawing.Point(624, 72);
            this.txtSigDelta6.Name = "txtSigDelta6";
            this.txtSigDelta6.ReadOnly = true;
            this.txtSigDelta6.Size = new System.Drawing.Size(48, 20);
            this.txtSigDelta6.TabIndex = 86;
            // 
            // txtSigDelta40
            // 
            this.txtSigDelta40.Location = new System.Drawing.Point(232, 72);
            this.txtSigDelta40.Name = "txtSigDelta40";
            this.txtSigDelta40.ReadOnly = true;
            this.txtSigDelta40.Size = new System.Drawing.Size(48, 20);
            this.txtSigDelta40.TabIndex = 79;
            // 
            // txtSigDelta160
            // 
            this.txtSigDelta160.Location = new System.Drawing.Point(64, 72);
            this.txtSigDelta160.Name = "txtSigDelta160";
            this.txtSigDelta160.ReadOnly = true;
            this.txtSigDelta160.Size = new System.Drawing.Size(48, 20);
            this.txtSigDelta160.TabIndex = 76;
            // 
            // txtSigDelta80
            // 
            this.txtSigDelta80.Location = new System.Drawing.Point(120, 72);
            this.txtSigDelta80.Name = "txtSigDelta80";
            this.txtSigDelta80.ReadOnly = true;
            this.txtSigDelta80.Size = new System.Drawing.Size(48, 20);
            this.txtSigDelta80.TabIndex = 77;
            // 
            // txtSigDelta12
            // 
            this.txtSigDelta12.Location = new System.Drawing.Point(512, 72);
            this.txtSigDelta12.Name = "txtSigDelta12";
            this.txtSigDelta12.ReadOnly = true;
            this.txtSigDelta12.Size = new System.Drawing.Size(48, 20);
            this.txtSigDelta12.TabIndex = 84;
            // 
            // txtSigDelta20
            // 
            this.txtSigDelta20.Location = new System.Drawing.Point(344, 72);
            this.txtSigDelta20.Name = "txtSigDelta20";
            this.txtSigDelta20.ReadOnly = true;
            this.txtSigDelta20.Size = new System.Drawing.Size(48, 20);
            this.txtSigDelta20.TabIndex = 81;
            // 
            // txtSigDelta10
            // 
            this.txtSigDelta10.Location = new System.Drawing.Point(568, 72);
            this.txtSigDelta10.Name = "txtSigDelta10";
            this.txtSigDelta10.ReadOnly = true;
            this.txtSigDelta10.Size = new System.Drawing.Size(48, 20);
            this.txtSigDelta10.TabIndex = 85;
            // 
            // txtSigDelta15
            // 
            this.txtSigDelta15.Location = new System.Drawing.Point(456, 72);
            this.txtSigDelta15.Name = "txtSigDelta15";
            this.txtSigDelta15.ReadOnly = true;
            this.txtSigDelta15.Size = new System.Drawing.Size(48, 20);
            this.txtSigDelta15.TabIndex = 83;
            // 
            // txtSigDelta30
            // 
            this.txtSigDelta30.Location = new System.Drawing.Point(288, 72);
            this.txtSigDelta30.Name = "txtSigDelta30";
            this.txtSigDelta30.ReadOnly = true;
            this.txtSigDelta30.Size = new System.Drawing.Size(48, 20);
            this.txtSigDelta30.TabIndex = 80;
            // 
            // lblSignalActual
            // 
            this.lblSignalActual.Image = null;
            this.lblSignalActual.Location = new System.Drawing.Point(16, 48);
            this.lblSignalActual.Name = "lblSignalActual";
            this.lblSignalActual.Size = new System.Drawing.Size(40, 23);
            this.lblSignalActual.TabIndex = 75;
            this.lblSignalActual.Text = "Actual:";
            // 
            // grpNoise
            // 
            this.grpNoise.Controls.Add(this.lblNoiseRef);
            this.grpNoise.Controls.Add(this.txtNoise20);
            this.grpNoise.Controls.Add(this.lblNoise);
            this.grpNoise.Controls.Add(this.txtRefNoise20);
            this.grpNoise.Location = new System.Drawing.Point(16, 248);
            this.grpNoise.Name = "grpNoise";
            this.grpNoise.Size = new System.Drawing.Size(144, 80);
            this.grpNoise.TabIndex = 79;
            this.grpNoise.TabStop = false;
            this.grpNoise.Text = "Measured Noise (dBm)";
            // 
            // lblNoiseRef
            // 
            this.lblNoiseRef.Image = null;
            this.lblNoiseRef.Location = new System.Drawing.Point(16, 25);
            this.lblNoiseRef.Name = "lblNoiseRef";
            this.lblNoiseRef.Size = new System.Drawing.Size(40, 23);
            this.lblNoiseRef.TabIndex = 76;
            this.lblNoiseRef.Text = "Ref:";
            // 
            // lblBand
            // 
            this.lblBand.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBand.Image = null;
            this.lblBand.Location = new System.Drawing.Point(32, 112);
            this.lblBand.Name = "lblBand";
            this.lblBand.Size = new System.Drawing.Size(40, 16);
            this.lblBand.TabIndex = 80;
            this.lblBand.Text = "Band";
            // 
            // grpPreamp
            // 
            this.grpPreamp.Controls.Add(this.txtPreAtt);
            this.grpPreamp.Controls.Add(this.lblPreNoise);
            this.grpPreamp.Controls.Add(this.lblPreSignal);
            this.grpPreamp.Controls.Add(this.txtPreGain);
            this.grpPreamp.Location = new System.Drawing.Point(168, 248);
            this.grpPreamp.Name = "grpPreamp";
            this.grpPreamp.Size = new System.Drawing.Size(128, 80);
            this.grpPreamp.TabIndex = 81;
            this.grpPreamp.TabStop = false;
            this.grpPreamp.Text = "Preamp Test (dBm)";
            // 
            // txtPreAtt
            // 
            this.txtPreAtt.Location = new System.Drawing.Point(64, 48);
            this.txtPreAtt.Name = "txtPreAtt";
            this.txtPreAtt.ReadOnly = true;
            this.txtPreAtt.Size = new System.Drawing.Size(48, 20);
            this.txtPreAtt.TabIndex = 4;
            // 
            // lblPreNoise
            // 
            this.lblPreNoise.Image = null;
            this.lblPreNoise.Location = new System.Drawing.Point(16, 48);
            this.lblPreNoise.Name = "lblPreNoise";
            this.lblPreNoise.Size = new System.Drawing.Size(48, 16);
            this.lblPreNoise.TabIndex = 2;
            this.lblPreNoise.Text = "Atten:";
            // 
            // lblPreSignal
            // 
            this.lblPreSignal.Image = null;
            this.lblPreSignal.Location = new System.Drawing.Point(16, 24);
            this.lblPreSignal.Name = "lblPreSignal";
            this.lblPreSignal.Size = new System.Drawing.Size(48, 16);
            this.lblPreSignal.TabIndex = 1;
            this.lblPreSignal.Text = "Gain:";
            // 
            // txtPreGain
            // 
            this.txtPreGain.Location = new System.Drawing.Point(64, 24);
            this.txtPreGain.Name = "txtPreGain";
            this.txtPreGain.ReadOnly = true;
            this.txtPreGain.Size = new System.Drawing.Size(48, 20);
            this.txtPreGain.TabIndex = 0;
            // 
            // grpTests
            // 
            this.grpTests.Controls.Add(this.btnRunPTT);
            this.grpTests.Controls.Add(this.btnRunMic);
            this.grpTests.Controls.Add(this.btnRunBalance);
            this.grpTests.Controls.Add(this.btnRunRFE);
            this.grpTests.Controls.Add(this.btnRunTX);
            this.grpTests.Controls.Add(this.chkRunDash);
            this.grpTests.Controls.Add(this.chkRunDot);
            this.grpTests.Controls.Add(this.btnRunImpulse);
            this.grpTests.Controls.Add(this.btnClearAll);
            this.grpTests.Controls.Add(this.btnCheckAll);
            this.grpTests.Controls.Add(this.btnRunPreamp);
            this.grpTests.Controls.Add(this.btnRunNoise);
            this.grpTests.Controls.Add(this.btnRunSignal);
            this.grpTests.Controls.Add(this.lblSkipCheckedBands);
            this.grpTests.Controls.Add(this.chk6);
            this.grpTests.Controls.Add(this.chk10);
            this.grpTests.Controls.Add(this.chk12);
            this.grpTests.Controls.Add(this.chk15);
            this.grpTests.Controls.Add(this.chk17);
            this.grpTests.Controls.Add(this.chk20);
            this.grpTests.Controls.Add(this.chk30);
            this.grpTests.Controls.Add(this.chk40);
            this.grpTests.Controls.Add(this.chk60);
            this.grpTests.Controls.Add(this.chk80);
            this.grpTests.Controls.Add(this.chk160);
            this.grpTests.Controls.Add(this.btnRunAllTests);
            this.grpTests.Location = new System.Drawing.Point(16, 8);
            this.grpTests.Name = "grpTests";
            this.grpTests.Size = new System.Drawing.Size(688, 88);
            this.grpTests.TabIndex = 82;
            this.grpTests.TabStop = false;
            this.grpTests.Text = "Test Options";
            // 
            // btnRunPTT
            // 
            this.btnRunPTT.Image = null;
            this.btnRunPTT.Location = new System.Drawing.Point(88, 56);
            this.btnRunPTT.Name = "btnRunPTT";
            this.btnRunPTT.Size = new System.Drawing.Size(40, 23);
            this.btnRunPTT.TabIndex = 27;
            this.btnRunPTT.Text = "PTT";
            this.btnRunPTT.Click += new System.EventHandler(this.btnRunPTT_Click);
            // 
            // btnRunMic
            // 
            this.btnRunMic.Image = null;
            this.btnRunMic.Location = new System.Drawing.Point(40, 56);
            this.btnRunMic.Name = "btnRunMic";
            this.btnRunMic.Size = new System.Drawing.Size(40, 23);
            this.btnRunMic.TabIndex = 26;
            this.btnRunMic.Text = "Mic";
            this.btnRunMic.Click += new System.EventHandler(this.btnRunMic_Click);
            // 
            // btnRunBalance
            // 
            this.btnRunBalance.Image = null;
            this.btnRunBalance.Location = new System.Drawing.Point(328, 56);
            this.btnRunBalance.Name = "btnRunBalance";
            this.btnRunBalance.Size = new System.Drawing.Size(56, 23);
            this.btnRunBalance.TabIndex = 25;
            this.btnRunBalance.Text = "Balance";
            this.btnRunBalance.Click += new System.EventHandler(this.btnRunBalance_Click);
            // 
            // btnRunRFE
            // 
            this.btnRunRFE.Image = null;
            this.btnRunRFE.Location = new System.Drawing.Point(280, 56);
            this.btnRunRFE.Name = "btnRunRFE";
            this.btnRunRFE.Size = new System.Drawing.Size(40, 23);
            this.btnRunRFE.TabIndex = 24;
            this.btnRunRFE.Text = "RFE";
            this.btnRunRFE.Click += new System.EventHandler(this.btnRunRFE_Click);
            // 
            // btnRunTX
            // 
            this.btnRunTX.Image = null;
            this.btnRunTX.Location = new System.Drawing.Point(232, 56);
            this.btnRunTX.Name = "btnRunTX";
            this.btnRunTX.Size = new System.Drawing.Size(40, 23);
            this.btnRunTX.TabIndex = 23;
            this.btnRunTX.Text = "TX";
            this.btnRunTX.Click += new System.EventHandler(this.btnRunTX_Click);
            // 
            // chkRunDash
            // 
            this.chkRunDash.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkRunDash.Location = new System.Drawing.Point(184, 56);
            this.chkRunDash.Name = "chkRunDash";
            this.chkRunDash.Size = new System.Drawing.Size(40, 23);
            this.chkRunDash.TabIndex = 22;
            this.chkRunDash.Text = "Dash";
            this.chkRunDash.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkRunDash.CheckedChanged += new System.EventHandler(this.chkRunDash_CheckedChanged);
            // 
            // chkRunDot
            // 
            this.chkRunDot.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkRunDot.Location = new System.Drawing.Point(136, 56);
            this.chkRunDot.Name = "chkRunDot";
            this.chkRunDot.Size = new System.Drawing.Size(40, 23);
            this.chkRunDot.TabIndex = 21;
            this.chkRunDot.Text = "Dot";
            this.chkRunDot.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkRunDot.CheckedChanged += new System.EventHandler(this.chkRunDot_CheckedChanged);
            // 
            // btnRunImpulse
            // 
            this.btnRunImpulse.Image = null;
            this.btnRunImpulse.Location = new System.Drawing.Point(312, 24);
            this.btnRunImpulse.Name = "btnRunImpulse";
            this.btnRunImpulse.Size = new System.Drawing.Size(56, 23);
            this.btnRunImpulse.TabIndex = 18;
            this.btnRunImpulse.Text = "Impulse";
            this.btnRunImpulse.Click += new System.EventHandler(this.btnRunImpulse_Click);
            // 
            // btnClearAll
            // 
            this.btnClearAll.Image = null;
            this.btnClearAll.Location = new System.Drawing.Point(544, 64);
            this.btnClearAll.Name = "btnClearAll";
            this.btnClearAll.Size = new System.Drawing.Size(64, 20);
            this.btnClearAll.TabIndex = 17;
            this.btnClearAll.Text = "Clear All";
            this.btnClearAll.Click += new System.EventHandler(this.btnClearAll_Click);
            // 
            // btnCheckAll
            // 
            this.btnCheckAll.Image = null;
            this.btnCheckAll.Location = new System.Drawing.Point(456, 64);
            this.btnCheckAll.Name = "btnCheckAll";
            this.btnCheckAll.Size = new System.Drawing.Size(64, 20);
            this.btnCheckAll.TabIndex = 16;
            this.btnCheckAll.Text = "Check All";
            this.btnCheckAll.Click += new System.EventHandler(this.btnCheckAll_Click);
            // 
            // btnRunPreamp
            // 
            this.btnRunPreamp.Image = null;
            this.btnRunPreamp.Location = new System.Drawing.Point(248, 24);
            this.btnRunPreamp.Name = "btnRunPreamp";
            this.btnRunPreamp.Size = new System.Drawing.Size(56, 23);
            this.btnRunPreamp.TabIndex = 15;
            this.btnRunPreamp.Text = "Preamp";
            this.btnRunPreamp.Click += new System.EventHandler(this.btnRunPreamp_Click);
            // 
            // btnRunNoise
            // 
            this.btnRunNoise.Image = null;
            this.btnRunNoise.Location = new System.Drawing.Point(192, 24);
            this.btnRunNoise.Name = "btnRunNoise";
            this.btnRunNoise.Size = new System.Drawing.Size(48, 23);
            this.btnRunNoise.TabIndex = 14;
            this.btnRunNoise.Text = "Noise";
            this.btnRunNoise.Click += new System.EventHandler(this.btnRunNoise_Click);
            // 
            // btnRunSignal
            // 
            this.btnRunSignal.Image = null;
            this.btnRunSignal.Location = new System.Drawing.Point(136, 24);
            this.btnRunSignal.Name = "btnRunSignal";
            this.btnRunSignal.Size = new System.Drawing.Size(48, 23);
            this.btnRunSignal.TabIndex = 13;
            this.btnRunSignal.Text = "Signal";
            this.btnRunSignal.Click += new System.EventHandler(this.btnRunSignal_Click);
            // 
            // lblSkipCheckedBands
            // 
            this.lblSkipCheckedBands.Image = null;
            this.lblSkipCheckedBands.Location = new System.Drawing.Point(384, 24);
            this.lblSkipCheckedBands.Name = "lblSkipCheckedBands";
            this.lblSkipCheckedBands.Size = new System.Drawing.Size(56, 40);
            this.lblSkipCheckedBands.TabIndex = 12;
            this.lblSkipCheckedBands.Text = "Skip Checked Bands:";
            // 
            // chk6
            // 
            this.chk6.Image = null;
            this.chk6.Location = new System.Drawing.Point(648, 40);
            this.chk6.Name = "chk6";
            this.chk6.Size = new System.Drawing.Size(32, 24);
            this.chk6.TabIndex = 11;
            this.chk6.Text = "6";
            // 
            // chk10
            // 
            this.chk10.Image = null;
            this.chk10.Location = new System.Drawing.Point(608, 40);
            this.chk10.Name = "chk10";
            this.chk10.Size = new System.Drawing.Size(40, 24);
            this.chk10.TabIndex = 10;
            this.chk10.Text = "10";
            // 
            // chk12
            // 
            this.chk12.Image = null;
            this.chk12.Location = new System.Drawing.Point(568, 40);
            this.chk12.Name = "chk12";
            this.chk12.Size = new System.Drawing.Size(40, 24);
            this.chk12.TabIndex = 9;
            this.chk12.Text = "12";
            // 
            // chk15
            // 
            this.chk15.Image = null;
            this.chk15.Location = new System.Drawing.Point(528, 40);
            this.chk15.Name = "chk15";
            this.chk15.Size = new System.Drawing.Size(40, 24);
            this.chk15.TabIndex = 8;
            this.chk15.Text = "15";
            // 
            // chk17
            // 
            this.chk17.Image = null;
            this.chk17.Location = new System.Drawing.Point(488, 40);
            this.chk17.Name = "chk17";
            this.chk17.Size = new System.Drawing.Size(40, 24);
            this.chk17.TabIndex = 7;
            this.chk17.Text = "17";
            // 
            // chk20
            // 
            this.chk20.Image = null;
            this.chk20.Location = new System.Drawing.Point(448, 40);
            this.chk20.Name = "chk20";
            this.chk20.Size = new System.Drawing.Size(40, 24);
            this.chk20.TabIndex = 6;
            this.chk20.Text = "20";
            // 
            // chk30
            // 
            this.chk30.Image = null;
            this.chk30.Location = new System.Drawing.Point(616, 16);
            this.chk30.Name = "chk30";
            this.chk30.Size = new System.Drawing.Size(40, 24);
            this.chk30.TabIndex = 5;
            this.chk30.Text = "30";
            // 
            // chk40
            // 
            this.chk40.Image = null;
            this.chk40.Location = new System.Drawing.Point(576, 16);
            this.chk40.Name = "chk40";
            this.chk40.Size = new System.Drawing.Size(40, 24);
            this.chk40.TabIndex = 4;
            this.chk40.Text = "40";
            // 
            // chk60
            // 
            this.chk60.Image = null;
            this.chk60.Location = new System.Drawing.Point(536, 16);
            this.chk60.Name = "chk60";
            this.chk60.Size = new System.Drawing.Size(40, 24);
            this.chk60.TabIndex = 3;
            this.chk60.Text = "60";
            // 
            // chk80
            // 
            this.chk80.Image = null;
            this.chk80.Location = new System.Drawing.Point(496, 16);
            this.chk80.Name = "chk80";
            this.chk80.Size = new System.Drawing.Size(40, 24);
            this.chk80.TabIndex = 2;
            this.chk80.Text = "80";
            // 
            // chk160
            // 
            this.chk160.Image = null;
            this.chk160.Location = new System.Drawing.Point(448, 16);
            this.chk160.Name = "chk160";
            this.chk160.Size = new System.Drawing.Size(48, 24);
            this.chk160.TabIndex = 1;
            this.chk160.Text = "160";
            // 
            // btnRunAllTests
            // 
            this.btnRunAllTests.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRunAllTests.Image = null;
            this.btnRunAllTests.Location = new System.Drawing.Point(16, 24);
            this.btnRunAllTests.Name = "btnRunAllTests";
            this.btnRunAllTests.Size = new System.Drawing.Size(104, 24);
            this.btnRunAllTests.TabIndex = 0;
            this.btnRunAllTests.Text = "Run All Tests";
            this.toolTip1.SetToolTip(this.btnRunAllTests, "Run all automatic tests.");
            this.btnRunAllTests.Click += new System.EventHandler(this.btnRunAllTests_Click);
            // 
            // grpGenerator
            // 
            this.grpGenerator.Controls.Add(this.udGenClockCorr);
            this.grpGenerator.Controls.Add(this.label1);
            this.grpGenerator.Controls.Add(this.btnGenReset);
            this.grpGenerator.Controls.Add(this.udGenLevel);
            this.grpGenerator.Controls.Add(this.lblGenLevel);
            this.grpGenerator.Controls.Add(this.udGenFreq);
            this.grpGenerator.Controls.Add(this.lblGenFreq);
            this.grpGenerator.Location = new System.Drawing.Point(544, 248);
            this.grpGenerator.Name = "grpGenerator";
            this.grpGenerator.Size = new System.Drawing.Size(152, 144);
            this.grpGenerator.TabIndex = 83;
            this.grpGenerator.TabStop = false;
            this.grpGenerator.Text = "Generator";
            // 
            // udGenClockCorr
            // 
            this.udGenClockCorr.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udGenClockCorr.Location = new System.Drawing.Point(80, 96);
            this.udGenClockCorr.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.udGenClockCorr.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.udGenClockCorr.Name = "udGenClockCorr";
            this.udGenClockCorr.Size = new System.Drawing.Size(56, 20);
            this.udGenClockCorr.TabIndex = 6;
            this.udGenClockCorr.Value = new decimal(new int[] {
            477,
            0,
            0,
            0});
            this.udGenClockCorr.ValueChanged += new System.EventHandler(this.udGenClockCorr_ValueChanged);
            // 
            // label1
            // 
            this.label1.Image = null;
            this.label1.Location = new System.Drawing.Point(16, 96);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 32);
            this.label1.TabIndex = 5;
            this.label1.Text = "Clock Correction:";
            // 
            // btnGenReset
            // 
            this.btnGenReset.Image = null;
            this.btnGenReset.Location = new System.Drawing.Point(16, 72);
            this.btnGenReset.Name = "btnGenReset";
            this.btnGenReset.Size = new System.Drawing.Size(56, 20);
            this.btnGenReset.TabIndex = 4;
            this.btnGenReset.Text = "Reset";
            this.btnGenReset.Click += new System.EventHandler(this.btnGenReset_Click);
            // 
            // udGenLevel
            // 
            this.udGenLevel.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udGenLevel.Location = new System.Drawing.Point(64, 48);
            this.udGenLevel.Maximum = new decimal(new int[] {
            4095,
            0,
            0,
            0});
            this.udGenLevel.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.udGenLevel.Name = "udGenLevel";
            this.udGenLevel.Size = new System.Drawing.Size(56, 20);
            this.udGenLevel.TabIndex = 3;
            this.udGenLevel.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udGenLevel.ValueChanged += new System.EventHandler(this.udGenLevel_ValueChanged);
            // 
            // lblGenLevel
            // 
            this.lblGenLevel.Image = null;
            this.lblGenLevel.Location = new System.Drawing.Point(16, 48);
            this.lblGenLevel.Name = "lblGenLevel";
            this.lblGenLevel.Size = new System.Drawing.Size(40, 23);
            this.lblGenLevel.TabIndex = 2;
            this.lblGenLevel.Text = "Level:";
            // 
            // udGenFreq
            // 
            this.udGenFreq.DecimalPlaces = 6;
            this.udGenFreq.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udGenFreq.Location = new System.Drawing.Point(64, 24);
            this.udGenFreq.Maximum = new decimal(new int[] {
            65,
            0,
            0,
            0});
            this.udGenFreq.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.udGenFreq.Name = "udGenFreq";
            this.udGenFreq.Size = new System.Drawing.Size(72, 20);
            this.udGenFreq.TabIndex = 1;
            this.udGenFreq.Value = new decimal(new int[] {
            70,
            0,
            0,
            65536});
            this.udGenFreq.ValueChanged += new System.EventHandler(this.udGenFreq_ValueChanged);
            // 
            // lblGenFreq
            // 
            this.lblGenFreq.Image = null;
            this.lblGenFreq.Location = new System.Drawing.Point(16, 24);
            this.lblGenFreq.Name = "lblGenFreq";
            this.lblGenFreq.Size = new System.Drawing.Size(32, 23);
            this.lblGenFreq.TabIndex = 0;
            this.lblGenFreq.Text = "Freq:";
            // 
            // grpTolerance
            // 
            this.grpTolerance.Controls.Add(this.txtTolSigHigh);
            this.grpTolerance.Controls.Add(this.labelTS3);
            this.grpTolerance.Controls.Add(this.txtTolSigLow);
            this.grpTolerance.Controls.Add(this.lblTolBalance);
            this.grpTolerance.Controls.Add(this.txtTolBalance);
            this.grpTolerance.Controls.Add(this.lblTolImpulse);
            this.grpTolerance.Controls.Add(this.txtTolImpulse);
            this.grpTolerance.Controls.Add(this.lblTolPreamp);
            this.grpTolerance.Controls.Add(this.txtTolPreamp);
            this.grpTolerance.Controls.Add(this.txtTolNoise);
            this.grpTolerance.Controls.Add(this.lblTolNoise);
            this.grpTolerance.Controls.Add(this.lblTolSigHigh);
            this.grpTolerance.Location = new System.Drawing.Point(304, 248);
            this.grpTolerance.Name = "grpTolerance";
            this.grpTolerance.Size = new System.Drawing.Size(232, 104);
            this.grpTolerance.TabIndex = 84;
            this.grpTolerance.TabStop = false;
            this.grpTolerance.Text = "Tolerance (dBm)";
            // 
            // txtTolSigHigh
            // 
            this.txtTolSigHigh.Location = new System.Drawing.Point(64, 24);
            this.txtTolSigHigh.Name = "txtTolSigHigh";
            this.txtTolSigHigh.Size = new System.Drawing.Size(48, 20);
            this.txtTolSigHigh.TabIndex = 0;
            this.txtTolSigHigh.Text = "3.0";
            // 
            // labelTS3
            // 
            this.labelTS3.Image = null;
            this.labelTS3.Location = new System.Drawing.Point(120, 24);
            this.labelTS3.Name = "labelTS3";
            this.labelTS3.Size = new System.Drawing.Size(48, 16);
            this.labelTS3.TabIndex = 12;
            this.labelTS3.Text = "Sig Low:";
            // 
            // txtTolSigLow
            // 
            this.txtTolSigLow.Location = new System.Drawing.Point(168, 24);
            this.txtTolSigLow.Name = "txtTolSigLow";
            this.txtTolSigLow.Size = new System.Drawing.Size(48, 20);
            this.txtTolSigLow.TabIndex = 11;
            this.txtTolSigLow.Text = "1.5";
            // 
            // lblTolBalance
            // 
            this.lblTolBalance.Image = null;
            this.lblTolBalance.Location = new System.Drawing.Point(120, 72);
            this.lblTolBalance.Name = "lblTolBalance";
            this.lblTolBalance.Size = new System.Drawing.Size(48, 16);
            this.lblTolBalance.TabIndex = 10;
            this.lblTolBalance.Text = "Balance:";
            // 
            // txtTolBalance
            // 
            this.txtTolBalance.Location = new System.Drawing.Point(168, 72);
            this.txtTolBalance.Name = "txtTolBalance";
            this.txtTolBalance.Size = new System.Drawing.Size(48, 20);
            this.txtTolBalance.TabIndex = 9;
            this.txtTolBalance.Text = "0.5";
            // 
            // lblTolImpulse
            // 
            this.lblTolImpulse.Image = null;
            this.lblTolImpulse.Location = new System.Drawing.Point(120, 48);
            this.lblTolImpulse.Name = "lblTolImpulse";
            this.lblTolImpulse.Size = new System.Drawing.Size(48, 16);
            this.lblTolImpulse.TabIndex = 8;
            this.lblTolImpulse.Text = "Impulse:";
            // 
            // txtTolImpulse
            // 
            this.txtTolImpulse.Location = new System.Drawing.Point(168, 48);
            this.txtTolImpulse.Name = "txtTolImpulse";
            this.txtTolImpulse.Size = new System.Drawing.Size(48, 20);
            this.txtTolImpulse.TabIndex = 7;
            this.txtTolImpulse.Text = "3.0";
            // 
            // lblTolPreamp
            // 
            this.lblTolPreamp.Image = null;
            this.lblTolPreamp.Location = new System.Drawing.Point(16, 72);
            this.lblTolPreamp.Name = "lblTolPreamp";
            this.lblTolPreamp.Size = new System.Drawing.Size(48, 16);
            this.lblTolPreamp.TabIndex = 6;
            this.lblTolPreamp.Text = "Preamp:";
            // 
            // txtTolPreamp
            // 
            this.txtTolPreamp.Location = new System.Drawing.Point(64, 72);
            this.txtTolPreamp.Name = "txtTolPreamp";
            this.txtTolPreamp.Size = new System.Drawing.Size(48, 20);
            this.txtTolPreamp.TabIndex = 5;
            this.txtTolPreamp.Text = "0.5";
            // 
            // txtTolNoise
            // 
            this.txtTolNoise.Location = new System.Drawing.Point(64, 48);
            this.txtTolNoise.Name = "txtTolNoise";
            this.txtTolNoise.Size = new System.Drawing.Size(48, 20);
            this.txtTolNoise.TabIndex = 4;
            this.txtTolNoise.Text = "1.5";
            // 
            // lblTolNoise
            // 
            this.lblTolNoise.Image = null;
            this.lblTolNoise.Location = new System.Drawing.Point(16, 48);
            this.lblTolNoise.Name = "lblTolNoise";
            this.lblTolNoise.Size = new System.Drawing.Size(48, 16);
            this.lblTolNoise.TabIndex = 2;
            this.lblTolNoise.Text = "Noise:";
            // 
            // lblTolSigHigh
            // 
            this.lblTolSigHigh.Image = null;
            this.lblTolSigHigh.Location = new System.Drawing.Point(16, 24);
            this.lblTolSigHigh.Name = "lblTolSigHigh";
            this.lblTolSigHigh.Size = new System.Drawing.Size(56, 16);
            this.lblTolSigHigh.TabIndex = 1;
            this.lblTolSigHigh.Text = "Sig High:";
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuDebug});
            // 
            // mnuDebug
            // 
            this.mnuDebug.Index = 0;
            this.mnuDebug.Text = "Debug";
            this.mnuDebug.Click += new System.EventHandler(this.mnuDebug_Click);
            // 
            // groupBoxTS1
            // 
            this.groupBoxTS1.Controls.Add(this.udTXTestOffTime);
            this.groupBoxTS1.Controls.Add(this.udTXTestOnTime);
            this.groupBoxTS1.Controls.Add(this.lblTXTestOnTime);
            this.groupBoxTS1.Controls.Add(this.labelTS2);
            this.groupBoxTS1.Location = new System.Drawing.Point(16, 336);
            this.groupBoxTS1.Name = "groupBoxTS1";
            this.groupBoxTS1.Size = new System.Drawing.Size(144, 80);
            this.groupBoxTS1.TabIndex = 85;
            this.groupBoxTS1.TabStop = false;
            this.groupBoxTS1.Text = "Transmit Test";
            // 
            // udTXTestOffTime
            // 
            this.udTXTestOffTime.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udTXTestOffTime.Location = new System.Drawing.Point(88, 48);
            this.udTXTestOffTime.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.udTXTestOffTime.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.udTXTestOffTime.Name = "udTXTestOffTime";
            this.udTXTestOffTime.Size = new System.Drawing.Size(48, 20);
            this.udTXTestOffTime.TabIndex = 78;
            this.udTXTestOffTime.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // udTXTestOnTime
            // 
            this.udTXTestOnTime.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udTXTestOnTime.Location = new System.Drawing.Point(88, 24);
            this.udTXTestOnTime.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.udTXTestOnTime.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.udTXTestOnTime.Name = "udTXTestOnTime";
            this.udTXTestOnTime.Size = new System.Drawing.Size(48, 20);
            this.udTXTestOnTime.TabIndex = 77;
            this.udTXTestOnTime.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // lblTXTestOnTime
            // 
            this.lblTXTestOnTime.Image = null;
            this.lblTXTestOnTime.Location = new System.Drawing.Point(16, 24);
            this.lblTXTestOnTime.Name = "lblTXTestOnTime";
            this.lblTXTestOnTime.Size = new System.Drawing.Size(80, 16);
            this.lblTXTestOnTime.TabIndex = 76;
            this.lblTXTestOnTime.Text = "On Time (ms):";
            // 
            // labelTS2
            // 
            this.labelTS2.Image = null;
            this.labelTS2.Location = new System.Drawing.Point(16, 48);
            this.labelTS2.Name = "labelTS2";
            this.labelTS2.Size = new System.Drawing.Size(80, 16);
            this.labelTS2.TabIndex = 73;
            this.labelTS2.Text = "Off Time (ms):";
            // 
            // btnPrintResults
            // 
            this.btnPrintResults.Location = new System.Drawing.Point(184, 336);
            this.btnPrintResults.Name = "btnPrintResults";
            this.btnPrintResults.Size = new System.Drawing.Size(88, 23);
            this.btnPrintResults.TabIndex = 86;
            this.btnPrintResults.Text = "Print Results";
            this.btnPrintResults.Click += new System.EventHandler(this.btnPrintResults_Click);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(176, 368);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 16);
            this.label2.TabIndex = 87;
            this.label2.Text = "Serial Num:";
            // 
            // txtSerialNum
            // 
            this.txtSerialNum.Location = new System.Drawing.Point(240, 368);
            this.txtSerialNum.Name = "txtSerialNum";
            this.txtSerialNum.Size = new System.Drawing.Size(72, 20);
            this.txtSerialNum.TabIndex = 88;
            // 
            // txtComments
            // 
            this.txtComments.Location = new System.Drawing.Point(240, 392);
            this.txtComments.Name = "txtComments";
            this.txtComments.Size = new System.Drawing.Size(288, 20);
            this.txtComments.TabIndex = 89;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(176, 392);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 16);
            this.label3.TabIndex = 90;
            this.label3.Text = "Comments:";
            // 
            // printDocument1
            // 
            this.printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument1_PrintPage);
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
            // labelTS4
            // 
            this.labelTS4.Image = null;
            this.labelTS4.Location = new System.Drawing.Point(328, 360);
            this.labelTS4.Name = "labelTS4";
            this.labelTS4.Size = new System.Drawing.Size(32, 16);
            this.labelTS4.TabIndex = 92;
            this.labelTS4.Text = "Mic:";
            // 
            // txtTolMic
            // 
            this.txtTolMic.Location = new System.Drawing.Point(368, 360);
            this.txtTolMic.Name = "txtTolMic";
            this.txtTolMic.Size = new System.Drawing.Size(48, 20);
            this.txtTolMic.TabIndex = 91;
            this.txtTolMic.Text = "1.0";
            // 
            // ProductionTest
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(712, 425);
            this.Controls.Add(this.labelTS4);
            this.Controls.Add(this.txtTolMic);
            this.Controls.Add(this.txtComments);
            this.Controls.Add(this.txtSerialNum);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnPrintResults);
            this.Controls.Add(this.groupBoxTS1);
            this.Controls.Add(this.grpTolerance);
            this.Controls.Add(this.grpGenerator);
            this.Controls.Add(this.grpTests);
            this.Controls.Add(this.grpPreamp);
            this.Controls.Add(this.lblBand);
            this.Controls.Add(this.lblBand6);
            this.Controls.Add(this.lblBand10);
            this.Controls.Add(this.lblBand12);
            this.Controls.Add(this.lblBand15);
            this.Controls.Add(this.lblBand17);
            this.Controls.Add(this.lblBand20);
            this.Controls.Add(this.lblBand30);
            this.Controls.Add(this.lblBand40);
            this.Controls.Add(this.lblBand60);
            this.Controls.Add(this.lblBand80);
            this.Controls.Add(this.lblBand160);
            this.Controls.Add(this.grpNoise);
            this.Controls.Add(this.grpSignal);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Menu = this.mainMenu1;
            this.Name = "ProductionTest";
            this.Text = "Production Tests";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.ProductionTest_Closing);
            this.grpSignal.ResumeLayout(false);
            this.grpSignal.PerformLayout();
            this.grpNoise.ResumeLayout(false);
            this.grpNoise.PerformLayout();
            this.grpPreamp.ResumeLayout(false);
            this.grpPreamp.PerformLayout();
            this.grpTests.ResumeLayout(false);
            this.grpGenerator.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.udGenClockCorr)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udGenLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udGenFreq)).EndInit();
            this.grpTolerance.ResumeLayout(false);
            this.grpTolerance.PerformLayout();
            this.groupBoxTS1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.udTXTestOffTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udTXTestOnTime)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        #region Variable Declaration


        private System.Windows.Forms.TextBoxTS txtRefSig160;
        private System.Windows.Forms.TextBoxTS txtSignal160H;
        private System.Windows.Forms.LabelTS lblBand160;
        private System.Windows.Forms.LabelTS lblBand80;
        private System.Windows.Forms.TextBoxTS txtSignal80H;
        private System.Windows.Forms.TextBoxTS txtRefSig80;
        private System.Windows.Forms.LabelTS lblBand60;
        private System.Windows.Forms.TextBoxTS txtSignal60H;
        private System.Windows.Forms.TextBoxTS txtRefSig60;
        private System.Windows.Forms.LabelTS lblBand40;
        private System.Windows.Forms.TextBoxTS txtSignal40H;
        private System.Windows.Forms.TextBoxTS txtRefSig40;
        private System.Windows.Forms.LabelTS lblBand30;
        private System.Windows.Forms.TextBoxTS txtSignal30H;
        private System.Windows.Forms.TextBoxTS txtRefSig30;
        private System.Windows.Forms.LabelTS lblBand20;
        private System.Windows.Forms.TextBoxTS txtNoise20;
        private System.Windows.Forms.TextBoxTS txtSignal20H;
        private System.Windows.Forms.TextBoxTS txtRefNoise20;
        private System.Windows.Forms.TextBoxTS txtRefSig20;
        private System.Windows.Forms.LabelTS lblBand17;
        private System.Windows.Forms.TextBoxTS txtSignal17H;
        private System.Windows.Forms.TextBoxTS txtRefSig17;
        private System.Windows.Forms.LabelTS lblBand15;
        private System.Windows.Forms.TextBoxTS txtSignal15H;
        private System.Windows.Forms.TextBoxTS txtRefSig15;
        private System.Windows.Forms.LabelTS lblBand12;
        private System.Windows.Forms.TextBoxTS txtSignal12H;
        private System.Windows.Forms.TextBoxTS txtRefSig12;
        private System.Windows.Forms.LabelTS lblBand10;
        private System.Windows.Forms.TextBoxTS txtSignal10H;
        private System.Windows.Forms.TextBoxTS txtRefSig10;
        private System.Windows.Forms.LabelTS lblBand6;
        private System.Windows.Forms.TextBoxTS txtSignal6H;
        private System.Windows.Forms.TextBoxTS txtRefSig6;
        private System.Windows.Forms.GroupBoxTS grpSignal;
        private System.Windows.Forms.GroupBoxTS grpNoise;
        private System.Windows.Forms.LabelTS lblBand;
        private System.Windows.Forms.GroupBoxTS grpPreamp;
        private System.Windows.Forms.ButtonTS btnRunPreamp;
        private System.Windows.Forms.ButtonTS btnRunNoise;
        private System.Windows.Forms.ButtonTS btnRunSignal;
        private System.Windows.Forms.LabelTS lblSkipCheckedBands;
        private System.Windows.Forms.CheckBoxTS chk6;
        private System.Windows.Forms.CheckBoxTS chk10;
        private System.Windows.Forms.CheckBoxTS chk12;
        private System.Windows.Forms.CheckBoxTS chk15;
        private System.Windows.Forms.CheckBoxTS chk17;
        private System.Windows.Forms.CheckBoxTS chk20;
        private System.Windows.Forms.CheckBoxTS chk30;
        private System.Windows.Forms.CheckBoxTS chk40;
        private System.Windows.Forms.CheckBoxTS chk60;
        private System.Windows.Forms.CheckBoxTS chk80;
        private System.Windows.Forms.CheckBoxTS chk160;
        private System.Windows.Forms.ButtonTS btnRunAllTests;
        private System.Windows.Forms.LabelTS lblPreNoise;
        private System.Windows.Forms.LabelTS lblPreSignal;
        private System.Windows.Forms.ButtonTS btnCheckAll;
        private System.Windows.Forms.GroupBoxTS grpGenerator;
        private System.Windows.Forms.LabelTS lblGenFreq;
        private System.Windows.Forms.NumericUpDownTS udGenFreq;
        private System.Windows.Forms.LabelTS lblGenLevel;
        private System.Windows.Forms.NumericUpDownTS udGenLevel;
        private System.Windows.Forms.ButtonTS btnGenReset;
        private System.Windows.Forms.LabelTS label1;
        private System.Windows.Forms.NumericUpDownTS udGenClockCorr;
        private System.Windows.Forms.ButtonTS btnClearAll;
        private System.Windows.Forms.TextBoxTS txtPreAtt;
        private System.Windows.Forms.TextBoxTS txtPreGain;
        private System.Windows.Forms.LabelTS lblNoise;
        private System.Windows.Forms.LabelTS lblSigRef;
        private System.Windows.Forms.LabelTS lblNoiseRef;
        private System.Windows.Forms.GroupBoxTS grpTolerance;
        private System.Windows.Forms.TextBoxTS txtTolNoise;
        private System.Windows.Forms.LabelTS lblTolNoise;
        private System.Windows.Forms.LabelTS lblTolPreamp;
        private System.Windows.Forms.TextBoxTS txtTolPreamp;
        private System.Windows.Forms.ButtonTS btnRunImpulse;
        private System.Windows.Forms.GroupBoxTS grpTests;
        private System.Windows.Forms.CheckBox chkRunDot;
        private System.Windows.Forms.CheckBox chkRunDash;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem mnuDebug;
        private System.Windows.Forms.ButtonTS btnRunRFE;
        private System.Windows.Forms.ButtonTS btnRunBalance;
        private System.Windows.Forms.LabelTS lblTolImpulse;
        private System.Windows.Forms.TextBoxTS txtTolImpulse;
        private System.Windows.Forms.LabelTS lblTolBalance;
        private System.Windows.Forms.TextBoxTS txtTolBalance;
        private System.Windows.Forms.ButtonTS btnRunTX;
        private System.Windows.Forms.LabelTS lblSignalActual;
        private System.Windows.Forms.GroupBoxTS groupBoxTS1;
        private System.Windows.Forms.LabelTS labelTS2;
        private System.Windows.Forms.LabelTS lblTXTestOnTime;
        private System.Windows.Forms.NumericUpDownTS udTXTestOffTime;
        private System.Windows.Forms.NumericUpDownTS udTXTestOnTime;
        private System.Windows.Forms.Button btnPrintResults;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBoxTS txtSerialNum;
        private System.Windows.Forms.TextBoxTS txtComments;
        private System.Windows.Forms.Label label3;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.PrintDialog printDialog1;
        private System.Windows.Forms.PrintPreviewDialog printPreviewDialog1;
        private System.Windows.Forms.LabelTS labelTS1;
        private System.Windows.Forms.TextBoxTS txtSigDelta60;
        private System.Windows.Forms.TextBoxTS txtSigDelta17;
        private System.Windows.Forms.TextBoxTS txtSigDelta40;
        private System.Windows.Forms.TextBoxTS txtSigDelta160;
        private System.Windows.Forms.TextBoxTS txtSigDelta80;
        private System.Windows.Forms.TextBoxTS txtSigDelta12;
        private System.Windows.Forms.TextBoxTS txtSigDelta20;
        private System.Windows.Forms.TextBoxTS txtSigDelta15;
        private System.Windows.Forms.TextBoxTS txtSigDelta30;
        private System.Windows.Forms.TextBoxTS txtSigDelta6;
        private System.Windows.Forms.TextBoxTS txtSigDelta10;
        private System.Windows.Forms.LabelTS lblTolSigHigh;
        private System.Windows.Forms.TextBoxTS txtTolSigHigh;
        private System.Windows.Forms.LabelTS labelTS3;
        private System.Windows.Forms.TextBoxTS txtTolSigLow;
        private System.Windows.Forms.ButtonTS btnRunPTT;
        private System.Windows.Forms.ButtonTS btnRunMic;
        private System.Windows.Forms.LabelTS labelTS4;
        private System.Windows.Forms.TextBoxTS txtTolMic;
        private System.ComponentModel.IContainer components;

        #endregion
    }
}
