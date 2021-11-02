//=================================================================
// FLEX5000RX2CalForm.cs
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
    public partial class FLEX5000RX2CalForm : System.Windows.Forms.Form
    {
        

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FLEX5000RX2CalForm));
            this.grpTestGeneral = new System.Windows.Forms.GroupBoxTS();
            this.btnTestAll = new System.Windows.Forms.ButtonTS();
            this.ckRunPreamp = new System.Windows.Forms.CheckBoxTS();
            this.btnTestNone = new System.Windows.Forms.ButtonTS();
            this.ckRunNoise = new System.Windows.Forms.CheckBoxTS();
            this.ckRunGenBal = new System.Windows.Forms.CheckBoxTS();
            this.ckRunFilters = new System.Windows.Forms.CheckBoxTS();
            this.printPreviewDialog1 = new System.Windows.Forms.PrintPreviewDialog();
            this.txtTech = new System.Windows.Forms.TextBoxTS();
            this.lblTech = new System.Windows.Forms.LabelTS();
            this.btnSelNone = new System.Windows.Forms.ButtonTS();
            this.btnPostFence = new System.Windows.Forms.ButtonTS();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.grpTestReceiver = new System.Windows.Forms.GroupBoxTS();
            this.btnCalNone = new System.Windows.Forms.ButtonTS();
            this.btnCalAll = new System.Windows.Forms.ButtonTS();
            this.ckCalImage = new System.Windows.Forms.CheckBoxTS();
            this.ckCalLevel = new System.Windows.Forms.CheckBoxTS();
            this.btnSelAll = new System.Windows.Forms.ButtonTS();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnTestPreamp = new System.Windows.Forms.ButtonTS();
            this.btnTestNoise = new System.Windows.Forms.ButtonTS();
            this.btnTestGenBal = new System.Windows.Forms.ButtonTS();
            this.btnCalImage = new System.Windows.Forms.ButtonTS();
            this.btnTestFilters = new System.Windows.Forms.ButtonTS();
            this.btnCalLevel = new System.Windows.Forms.ButtonTS();
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
            this.btnRunSel = new System.Windows.Forms.ButtonTS();
            this.lstDebug = new System.Windows.Forms.ListBox();
            this.grpTests = new System.Windows.Forms.GroupBoxTS();
            this.udLevel = new System.Windows.Forms.NumericUpDown();
            this.grpCal = new System.Windows.Forms.GroupBoxTS();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBoxTS();
            this.checkGENBAL = new System.Windows.Forms.CheckBoxTS();
            this.grpTestGeneral.SuspendLayout();
            this.grpTestReceiver.SuspendLayout();
            this.grpBands.SuspendLayout();
            this.grpTests.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udLevel)).BeginInit();
            this.grpCal.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpTestGeneral
            // 
            this.grpTestGeneral.Controls.Add(this.btnTestAll);
            this.grpTestGeneral.Controls.Add(this.ckRunPreamp);
            this.grpTestGeneral.Controls.Add(this.btnTestNone);
            this.grpTestGeneral.Controls.Add(this.ckRunNoise);
            this.grpTestGeneral.Controls.Add(this.ckRunGenBal);
            this.grpTestGeneral.Controls.Add(this.ckRunFilters);
            this.grpTestGeneral.Location = new System.Drawing.Point(8, 379);
            this.grpTestGeneral.Name = "grpTestGeneral";
            this.grpTestGeneral.Size = new System.Drawing.Size(376, 48);
            this.grpTestGeneral.TabIndex = 39;
            this.grpTestGeneral.TabStop = false;
            this.grpTestGeneral.Text = "Tests";
            // 
            // btnTestAll
            // 
            this.btnTestAll.Image = null;
            this.btnTestAll.Location = new System.Drawing.Point(280, 16);
            this.btnTestAll.Name = "btnTestAll";
            this.btnTestAll.Size = new System.Drawing.Size(40, 24);
            this.btnTestAll.TabIndex = 24;
            this.btnTestAll.Text = "All";
            this.btnTestAll.Click += new System.EventHandler(this.btnTestAll_Click);
            // 
            // ckRunPreamp
            // 
            this.ckRunPreamp.Checked = true;
            this.ckRunPreamp.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckRunPreamp.Image = null;
            this.ckRunPreamp.Location = new System.Drawing.Point(152, 16);
            this.ckRunPreamp.Name = "ckRunPreamp";
            this.ckRunPreamp.Size = new System.Drawing.Size(64, 24);
            this.ckRunPreamp.TabIndex = 26;
            this.ckRunPreamp.Text = "Preamp";
            // 
            // btnTestNone
            // 
            this.btnTestNone.Image = null;
            this.btnTestNone.Location = new System.Drawing.Point(328, 16);
            this.btnTestNone.Name = "btnTestNone";
            this.btnTestNone.Size = new System.Drawing.Size(40, 24);
            this.btnTestNone.TabIndex = 25;
            this.btnTestNone.Text = "None";
            this.btnTestNone.Click += new System.EventHandler(this.btnTestNone_Click);
            // 
            // ckRunNoise
            // 
            this.ckRunNoise.Checked = true;
            this.ckRunNoise.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckRunNoise.Image = null;
            this.ckRunNoise.Location = new System.Drawing.Point(88, 16);
            this.ckRunNoise.Name = "ckRunNoise";
            this.ckRunNoise.Size = new System.Drawing.Size(56, 24);
            this.ckRunNoise.TabIndex = 21;
            this.ckRunNoise.Text = "Noise";
            // 
            // ckRunGenBal
            // 
            this.ckRunGenBal.Checked = true;
            this.ckRunGenBal.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckRunGenBal.Image = null;
            this.ckRunGenBal.Location = new System.Drawing.Point(16, 16);
            this.ckRunGenBal.Name = "ckRunGenBal";
            this.ckRunGenBal.Size = new System.Drawing.Size(64, 24);
            this.ckRunGenBal.TabIndex = 20;
            this.ckRunGenBal.Text = "Gen/Bal";
            // 
            // ckRunFilters
            // 
            this.ckRunFilters.Checked = true;
            this.ckRunFilters.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckRunFilters.Image = null;
            this.ckRunFilters.Location = new System.Drawing.Point(224, 16);
            this.ckRunFilters.Name = "ckRunFilters";
            this.ckRunFilters.Size = new System.Drawing.Size(56, 24);
            this.ckRunFilters.TabIndex = 19;
            this.ckRunFilters.Text = "Filters";
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
            // txtTech
            // 
            this.txtTech.Location = new System.Drawing.Point(280, 448);
            this.txtTech.Name = "txtTech";
            this.txtTech.Size = new System.Drawing.Size(100, 20);
            this.txtTech.TabIndex = 37;
            // 
            // lblTech
            // 
            this.lblTech.Image = null;
            this.lblTech.Location = new System.Drawing.Point(280, 432);
            this.lblTech.Name = "lblTech";
            this.lblTech.Size = new System.Drawing.Size(64, 16);
            this.lblTech.TabIndex = 36;
            this.lblTech.Text = "Technician:";
            // 
            // btnSelNone
            // 
            this.btnSelNone.Image = null;
            this.btnSelNone.Location = new System.Drawing.Point(104, 360);
            this.btnSelNone.Name = "btnSelNone";
            this.btnSelNone.Size = new System.Drawing.Size(48, 20);
            this.btnSelNone.TabIndex = 43;
            this.btnSelNone.Text = "None";
            this.btnSelNone.Click += new System.EventHandler(this.btnSelNone_Click);
            // 
            // btnPostFence
            // 
            this.btnPostFence.Image = null;
            this.btnPostFence.Location = new System.Drawing.Point(168, 339);
            this.btnPostFence.Name = "btnPostFence";
            this.btnPostFence.Size = new System.Drawing.Size(56, 32);
            this.btnPostFence.TabIndex = 45;
            this.btnPostFence.Text = "Post Fence";
            // 
            // grpTestReceiver
            // 
            this.grpTestReceiver.Controls.Add(this.btnCalNone);
            this.grpTestReceiver.Controls.Add(this.btnCalAll);
            this.grpTestReceiver.Controls.Add(this.ckCalImage);
            this.grpTestReceiver.Controls.Add(this.ckCalLevel);
            this.grpTestReceiver.Location = new System.Drawing.Point(8, 432);
            this.grpTestReceiver.Name = "grpTestReceiver";
            this.grpTestReceiver.Size = new System.Drawing.Size(256, 48);
            this.grpTestReceiver.TabIndex = 40;
            this.grpTestReceiver.TabStop = false;
            this.grpTestReceiver.Text = "Calibrations";
            // 
            // btnCalNone
            // 
            this.btnCalNone.Image = null;
            this.btnCalNone.Location = new System.Drawing.Point(208, 16);
            this.btnCalNone.Name = "btnCalNone";
            this.btnCalNone.Size = new System.Drawing.Size(42, 24);
            this.btnCalNone.TabIndex = 27;
            this.btnCalNone.Text = "None";
            this.btnCalNone.Click += new System.EventHandler(this.btnCalNone_Click);
            // 
            // btnCalAll
            // 
            this.btnCalAll.Image = null;
            this.btnCalAll.Location = new System.Drawing.Point(160, 16);
            this.btnCalAll.Name = "btnCalAll";
            this.btnCalAll.Size = new System.Drawing.Size(40, 24);
            this.btnCalAll.TabIndex = 26;
            this.btnCalAll.Text = "All";
            this.btnCalAll.Click += new System.EventHandler(this.btnCalAll_Click);
            // 
            // ckCalImage
            // 
            this.ckCalImage.Checked = true;
            this.ckCalImage.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckCalImage.Image = null;
            this.ckCalImage.Location = new System.Drawing.Point(80, 16);
            this.ckCalImage.Name = "ckCalImage";
            this.ckCalImage.Size = new System.Drawing.Size(56, 24);
            this.ckCalImage.TabIndex = 21;
            this.ckCalImage.Text = "Image";
            // 
            // ckCalLevel
            // 
            this.ckCalLevel.Checked = true;
            this.ckCalLevel.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckCalLevel.Image = null;
            this.ckCalLevel.Location = new System.Drawing.Point(16, 16);
            this.ckCalLevel.Name = "ckCalLevel";
            this.ckCalLevel.Size = new System.Drawing.Size(56, 24);
            this.ckCalLevel.TabIndex = 20;
            this.ckCalLevel.Text = "Level";
            // 
            // btnSelAll
            // 
            this.btnSelAll.Image = null;
            this.btnSelAll.Location = new System.Drawing.Point(104, 339);
            this.btnSelAll.Name = "btnSelAll";
            this.btnSelAll.Size = new System.Drawing.Size(48, 20);
            this.btnSelAll.TabIndex = 42;
            this.btnSelAll.Text = "All";
            this.btnSelAll.Click += new System.EventHandler(this.btnSelAll_Click);
            // 
            // btnTestPreamp
            // 
            this.btnTestPreamp.Image = null;
            this.btnTestPreamp.Location = new System.Drawing.Point(16, 104);
            this.btnTestPreamp.Name = "btnTestPreamp";
            this.btnTestPreamp.Size = new System.Drawing.Size(75, 23);
            this.btnTestPreamp.TabIndex = 7;
            this.btnTestPreamp.Text = "Preamp";
            this.toolTip1.SetToolTip(this.btnTestPreamp, "Preamp Test: Not Run");
            this.btnTestPreamp.Click += new System.EventHandler(this.btnTestPreamp_Click);
            // 
            // btnTestNoise
            // 
            this.btnTestNoise.Image = null;
            this.btnTestNoise.Location = new System.Drawing.Point(16, 64);
            this.btnTestNoise.Name = "btnTestNoise";
            this.btnTestNoise.Size = new System.Drawing.Size(75, 23);
            this.btnTestNoise.TabIndex = 2;
            this.btnTestNoise.Text = "Noise";
            this.toolTip1.SetToolTip(this.btnTestNoise, "Noise Test: Not Run");
            this.btnTestNoise.Click += new System.EventHandler(this.btnTestNoise_Click);
            // 
            // btnTestGenBal
            // 
            this.btnTestGenBal.Image = null;
            this.btnTestGenBal.Location = new System.Drawing.Point(16, 24);
            this.btnTestGenBal.Name = "btnTestGenBal";
            this.btnTestGenBal.Size = new System.Drawing.Size(56, 23);
            this.btnTestGenBal.TabIndex = 1;
            this.btnTestGenBal.Text = "Gen/Bal";
            this.toolTip1.SetToolTip(this.btnTestGenBal, "Gen/Bal Test: Not Run");
            this.btnTestGenBal.Click += new System.EventHandler(this.btnTestGenBal_Click);
            // 
            // btnCalImage
            // 
            this.btnCalImage.Image = null;
            this.btnCalImage.Location = new System.Drawing.Point(16, 64);
            this.btnCalImage.Name = "btnCalImage";
            this.btnCalImage.Size = new System.Drawing.Size(75, 23);
            this.btnCalImage.TabIndex = 4;
            this.btnCalImage.Text = "Image";
            this.toolTip1.SetToolTip(this.btnCalImage, "RX Image Test: Not Run");
            this.btnCalImage.Click += new System.EventHandler(this.btnCalImage_Click);
            // 
            // btnTestFilters
            // 
            this.btnTestFilters.Image = null;
            this.btnTestFilters.Location = new System.Drawing.Point(16, 144);
            this.btnTestFilters.Name = "btnTestFilters";
            this.btnTestFilters.Size = new System.Drawing.Size(75, 23);
            this.btnTestFilters.TabIndex = 1;
            this.btnTestFilters.Text = "Filters";
            this.toolTip1.SetToolTip(this.btnTestFilters, "RX Filter Test: Not Run");
            this.btnTestFilters.Click += new System.EventHandler(this.btnTestFilters_Click);
            // 
            // btnCalLevel
            // 
            this.btnCalLevel.Image = null;
            this.btnCalLevel.Location = new System.Drawing.Point(16, 24);
            this.btnCalLevel.Name = "btnCalLevel";
            this.btnCalLevel.Size = new System.Drawing.Size(75, 23);
            this.btnCalLevel.TabIndex = 3;
            this.btnCalLevel.Text = "Level";
            this.toolTip1.SetToolTip(this.btnCalLevel, "RX Level Test: Not Run");
            this.btnCalLevel.Click += new System.EventHandler(this.btnCalLevel_Click);
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
            this.grpBands.Location = new System.Drawing.Point(128, 232);
            this.grpBands.Name = "grpBands";
            this.grpBands.Size = new System.Drawing.Size(256, 96);
            this.grpBands.TabIndex = 33;
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
            // btnRunSel
            // 
            this.btnRunSel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRunSel.Image = null;
            this.btnRunSel.Location = new System.Drawing.Point(8, 339);
            this.btnRunSel.Name = "btnRunSel";
            this.btnRunSel.Size = new System.Drawing.Size(88, 32);
            this.btnRunSel.TabIndex = 35;
            this.btnRunSel.Text = "Run Selected";
            this.btnRunSel.Click += new System.EventHandler(this.btnRunSel_Click);
            // 
            // lstDebug
            // 
            this.lstDebug.HorizontalScrollbar = true;
            this.lstDebug.Location = new System.Drawing.Point(128, 8);
            this.lstDebug.Name = "lstDebug";
            this.lstDebug.Size = new System.Drawing.Size(256, 212);
            this.lstDebug.TabIndex = 34;
            // 
            // grpTests
            // 
            this.grpTests.Controls.Add(this.checkGENBAL);
            this.grpTests.Controls.Add(this.udLevel);
            this.grpTests.Controls.Add(this.btnTestPreamp);
            this.grpTests.Controls.Add(this.btnTestNoise);
            this.grpTests.Controls.Add(this.btnTestGenBal);
            this.grpTests.Controls.Add(this.btnTestFilters);
            this.grpTests.Location = new System.Drawing.Point(8, 3);
            this.grpTests.Name = "grpTests";
            this.grpTests.Size = new System.Drawing.Size(112, 221);
            this.grpTests.TabIndex = 31;
            this.grpTests.TabStop = false;
            this.grpTests.Text = "Tests";
            // 
            // udLevel
            // 
            this.udLevel.DecimalPlaces = 1;
            this.udLevel.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.udLevel.Location = new System.Drawing.Point(17, 182);
            this.udLevel.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.udLevel.Name = "udLevel";
            this.udLevel.Size = new System.Drawing.Size(74, 20);
            this.udLevel.TabIndex = 8;
            this.udLevel.Value = new decimal(new int[] {
            240,
            0,
            0,
            -2147418112});
            this.udLevel.Visible = false;
            // 
            // grpCal
            // 
            this.grpCal.Controls.Add(this.btnCalImage);
            this.grpCal.Controls.Add(this.btnCalLevel);
            this.grpCal.Location = new System.Drawing.Point(8, 232);
            this.grpCal.Name = "grpCal";
            this.grpCal.Size = new System.Drawing.Size(112, 96);
            this.grpCal.TabIndex = 30;
            this.grpCal.TabStop = false;
            this.grpCal.Text = "Calibration";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(272, 336);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 40);
            this.label1.TabIndex = 46;
            this.label1.Text = "RX2";
            // 
            // checkBox1
            // 
            this.checkBox1.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBox1.Image = null;
            this.checkBox1.Location = new System.Drawing.Point(368, 344);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(24, 24);
            this.checkBox1.TabIndex = 47;
            this.checkBox1.Visible = false;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // checkGENBAL
            // 
            this.checkGENBAL.Image = null;
            this.checkGENBAL.Location = new System.Drawing.Point(82, 24);
            this.checkGENBAL.Margin = new System.Windows.Forms.Padding(0);
            this.checkGENBAL.Name = "checkGENBAL";
            this.checkGENBAL.Size = new System.Drawing.Size(27, 24);
            this.checkGENBAL.TabIndex = 32;
            this.toolTip1.SetToolTip(this.checkGENBAL, "Automatic Repeat of the GEN/BAL RX2 test until unchecked.\r\nNOTE: Does not update " +
        "the LOG file");
            // 
            // FLEX5000RX2CalForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(392, 486);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.grpTestReceiver);
            this.Controls.Add(this.txtTech);
            this.Controls.Add(this.lblTech);
            this.Controls.Add(this.btnSelNone);
            this.Controls.Add(this.btnPostFence);
            this.Controls.Add(this.btnSelAll);
            this.Controls.Add(this.grpBands);
            this.Controls.Add(this.btnRunSel);
            this.Controls.Add(this.lstDebug);
            this.Controls.Add(this.grpTests);
            this.Controls.Add(this.grpCal);
            this.Controls.Add(this.grpTestGeneral);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "FLEX5000RX2CalForm";
            this.Text = "FLEX-5000 RX2 Calibration";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.FLEX5000RX2CalForm_Closing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FLEX5000RX2CalForm_KeyDown);
            this.grpTestGeneral.ResumeLayout(false);
            this.grpTestReceiver.ResumeLayout(false);
            this.grpBands.ResumeLayout(false);
            this.grpTests.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.udLevel)).EndInit();
            this.grpCal.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

    
        private System.Windows.Forms.GroupBoxTS grpTestGeneral;
        private System.Windows.Forms.ButtonTS btnTestAll;
        private System.Windows.Forms.CheckBoxTS ckRunPreamp;
        private System.Windows.Forms.ButtonTS btnTestNone;
        private System.Windows.Forms.CheckBoxTS ckRunNoise;
        private System.Windows.Forms.CheckBoxTS ckRunGenBal;
        private System.Windows.Forms.PrintPreviewDialog printPreviewDialog1;
        private System.Windows.Forms.TextBoxTS txtTech;
        private System.Windows.Forms.LabelTS lblTech;
        private System.Windows.Forms.ButtonTS btnSelNone;
        private System.Windows.Forms.ButtonTS btnPostFence;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.GroupBoxTS grpTestReceiver;
        private System.Windows.Forms.ButtonTS btnCalNone;
        private System.Windows.Forms.ButtonTS btnCalAll;
        private System.Windows.Forms.CheckBoxTS ckCalImage;
        private System.Windows.Forms.CheckBoxTS ckCalLevel;
        private System.Windows.Forms.ButtonTS btnSelAll;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBoxTS grpBands;
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
        private System.Windows.Forms.ButtonTS btnClearAll;
        private System.Windows.Forms.ButtonTS btnCheckAll;
        private System.Windows.Forms.ButtonTS btnRunSel;
        private System.Windows.Forms.ListBox lstDebug;
        private System.Windows.Forms.GroupBoxTS grpTests;
        private System.Windows.Forms.ButtonTS btnTestPreamp;
        private System.Windows.Forms.ButtonTS btnTestNoise;
        private System.Windows.Forms.ButtonTS btnTestGenBal;
        private System.Windows.Forms.GroupBoxTS grpCal;
        private System.Windows.Forms.ButtonTS btnCalImage;
        private System.Windows.Forms.ButtonTS btnTestFilters;
        private System.Windows.Forms.ButtonTS btnCalLevel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBoxTS ckRunFilters;
        private System.Windows.Forms.CheckBoxTS checkBox1;
        private System.Windows.Forms.NumericUpDown udLevel;
        private System.Windows.Forms.CheckBoxTS checkGENBAL;
        private System.ComponentModel.IContainer components;

      
    }
}
