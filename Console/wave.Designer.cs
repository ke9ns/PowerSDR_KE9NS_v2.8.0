//=================================================================
// wave.cs
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
    sealed public partial class WaveControl : System.Windows.Forms.Form
    {
       

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WaveControl));
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.grpPlaylist = new System.Windows.Forms.GroupBox();
            this.checkBoxRandom = new System.Windows.Forms.CheckBoxTS();
            this.checkBoxLoop = new System.Windows.Forms.CheckBoxTS();
            this.btnAdd = new System.Windows.Forms.ButtonTS();
            this.lstPlaylist = new System.Windows.Forms.ListBox();
            this.btnRemove = new System.Windows.Forms.ButtonTS();
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.mnuWaveOptions = new System.Windows.Forms.MenuItem();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.checkBoxTS1 = new System.Windows.Forms.CheckBoxTS();
            this.checkBoxTS2 = new System.Windows.Forms.CheckBoxTS();
            this.checkBoxVoice = new System.Windows.Forms.CheckBoxTS();
            this.checkBoxCQ = new System.Windows.Forms.CheckBoxTS();
            this.checkBoxCW = new System.Windows.Forms.CheckBoxTS();
            this.chkQuickAudioFolder = new System.Windows.Forms.CheckBoxTS();
            this.checkBoxVK1 = new System.Windows.Forms.CheckBoxTS();
            this.checkBoxVK2 = new System.Windows.Forms.CheckBoxTS();
            this.groupBoxTS2 = new System.Windows.Forms.GroupBoxTS();
            this.chkBoxMP3 = new System.Windows.Forms.CheckBoxTS();
            this.createBoxTS = new System.Windows.Forms.CheckBoxTS();
            this.TXIDBoxTS = new System.Windows.Forms.CheckBoxTS();
            this.chkQuickPlay = new System.Windows.Forms.CheckBoxTS();
            this.chkQuickRec = new System.Windows.Forms.CheckBoxTS();
            this.groupBoxTS1 = new System.Windows.Forms.GroupBoxTS();
            this.tbPreamp = new System.Windows.Forms.TrackBar();
            this.udPreamp = new System.Windows.Forms.NumericUpDownTS();
            this.groupBox2 = new System.Windows.Forms.GroupBoxTS();
            this.checkBoxRecord = new System.Windows.Forms.CheckBoxTS();
            this.grpPlayback = new System.Windows.Forms.GroupBoxTS();
            this.txtCurrentFile = new System.Windows.Forms.TextBoxTS();
            this.lblCurrentlyPlaying = new System.Windows.Forms.LabelTS();
            this.btnNext = new System.Windows.Forms.ButtonTS();
            this.btnPrevious = new System.Windows.Forms.ButtonTS();
            this.checkBoxPause = new System.Windows.Forms.CheckBoxTS();
            this.btnStop = new System.Windows.Forms.ButtonTS();
            this.checkBoxPlay = new System.Windows.Forms.CheckBoxTS();
            this.chkAlwaysOnTop = new System.Windows.Forms.CheckBoxTS();
            this.grpPlaylist.SuspendLayout();
            this.groupBoxTS2.SuspendLayout();
            this.groupBoxTS1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbPreamp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPreamp)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.grpPlayback.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "WAV files (*.wav)|*.wav|All files (*.*)|*.*";
            this.openFileDialog1.Multiselect = true;
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // grpPlaylist
            // 
            this.grpPlaylist.Controls.Add(this.checkBoxRandom);
            this.grpPlaylist.Controls.Add(this.checkBoxLoop);
            this.grpPlaylist.Controls.Add(this.btnAdd);
            this.grpPlaylist.Controls.Add(this.lstPlaylist);
            this.grpPlaylist.Controls.Add(this.btnRemove);
            this.grpPlaylist.Location = new System.Drawing.Point(8, 104);
            this.grpPlaylist.Name = "grpPlaylist";
            this.grpPlaylist.Size = new System.Drawing.Size(304, 184);
            this.grpPlaylist.TabIndex = 6;
            this.grpPlaylist.TabStop = false;
            this.grpPlaylist.Text = "Playlist";
            // 
            // checkBoxRandom
            // 
            this.checkBoxRandom.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxRandom.Enabled = false;
            this.checkBoxRandom.Image = null;
            this.checkBoxRandom.Location = new System.Drawing.Point(224, 24);
            this.checkBoxRandom.Name = "checkBoxRandom";
            this.checkBoxRandom.Size = new System.Drawing.Size(56, 23);
            this.checkBoxRandom.TabIndex = 13;
            this.checkBoxRandom.Text = "Random";
            this.checkBoxRandom.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxRandom.Visible = false;
            this.checkBoxRandom.CheckedChanged += new System.EventHandler(this.checkBoxRandom_CheckedChanged);
            // 
            // checkBoxLoop
            // 
            this.checkBoxLoop.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxLoop.Enabled = false;
            this.checkBoxLoop.Image = null;
            this.checkBoxLoop.Location = new System.Drawing.Point(176, 24);
            this.checkBoxLoop.Name = "checkBoxLoop";
            this.checkBoxLoop.Size = new System.Drawing.Size(40, 23);
            this.checkBoxLoop.TabIndex = 12;
            this.checkBoxLoop.Text = "Loop";
            this.checkBoxLoop.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxLoop.CheckedChanged += new System.EventHandler(this.checkBoxLoop_CheckedChanged);
            // 
            // btnAdd
            // 
            this.btnAdd.Image = null;
            this.btnAdd.Location = new System.Drawing.Point(24, 24);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(48, 23);
            this.btnAdd.TabIndex = 6;
            this.btnAdd.Text = "Add...";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // lstPlaylist
            // 
            this.lstPlaylist.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lstPlaylist.Location = new System.Drawing.Point(16, 56);
            this.lstPlaylist.Name = "lstPlaylist";
            this.lstPlaylist.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstPlaylist.Size = new System.Drawing.Size(272, 108);
            this.lstPlaylist.TabIndex = 0;
            this.lstPlaylist.SelectedIndexChanged += new System.EventHandler(this.lstPlaylist_SelectedIndexChanged);
            this.lstPlaylist.DoubleClick += new System.EventHandler(this.lstPlaylist_DoubleClick);
            // 
            // btnRemove
            // 
            this.btnRemove.Enabled = false;
            this.btnRemove.Image = null;
            this.btnRemove.Location = new System.Drawing.Point(80, 24);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(56, 23);
            this.btnRemove.TabIndex = 11;
            this.btnRemove.Text = "Remove";
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuWaveOptions});
            // 
            // mnuWaveOptions
            // 
            this.mnuWaveOptions.Index = 0;
            this.mnuWaveOptions.Text = "Options";
            this.mnuWaveOptions.Click += new System.EventHandler(this.mnuWaveOptions_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(8, 294);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(392, 62);
            this.textBox1.TabIndex = 59;
            // 
            // toolTip1
            // 
            this.toolTip1.AutomaticDelay = 200;
            this.toolTip1.AutoPopDelay = 12000;
            this.toolTip1.InitialDelay = 500;
            this.toolTip1.ReshowDelay = 40;
            // 
            // checkBoxTS1
            // 
            this.checkBoxTS1.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxTS1.Image = null;
            this.checkBoxTS1.Location = new System.Drawing.Point(318, 431);
            this.checkBoxTS1.Name = "checkBoxTS1";
            this.checkBoxTS1.Size = new System.Drawing.Size(72, 24);
            this.checkBoxTS1.TabIndex = 64;
            this.checkBoxTS1.Text = "MP3 Folder";
            this.checkBoxTS1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.checkBoxTS1, "Opens QuickAudio folder containing MP3 files");
            this.checkBoxTS1.CheckedChanged += new System.EventHandler(this.checkBoxTS1_CheckedChanged);
            // 
            // checkBoxTS2
            // 
            this.checkBoxTS2.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxTS2.Image = null;
            this.checkBoxTS2.Location = new System.Drawing.Point(140, 25);
            this.checkBoxTS2.Name = "checkBoxTS2";
            this.checkBoxTS2.Size = new System.Drawing.Size(57, 24);
            this.checkBoxTS2.TabIndex = 64;
            this.checkBoxTS2.Text = "Reply";
            this.checkBoxTS2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.checkBoxTS2, resources.GetString("checkBoxTS2.ToolTip"));
            this.checkBoxTS2.CheckedChanged += new System.EventHandler(this.checkBoxTS2_CheckedChanged);
            // 
            // checkBoxVoice
            // 
            this.checkBoxVoice.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxVoice.Image = null;
            this.checkBoxVoice.Location = new System.Drawing.Point(6, 25);
            this.checkBoxVoice.Name = "checkBoxVoice";
            this.checkBoxVoice.Size = new System.Drawing.Size(63, 24);
            this.checkBoxVoice.TabIndex = 61;
            this.checkBoxVoice.Text = "Voice ID";
            this.checkBoxVoice.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.checkBoxVoice, resources.GetString("checkBoxVoice.ToolTip"));
            this.checkBoxVoice.CheckedChanged += new System.EventHandler(this.checkBoxVoice_CheckedChanged);
            // 
            // checkBoxCQ
            // 
            this.checkBoxCQ.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxCQ.Image = null;
            this.checkBoxCQ.Location = new System.Drawing.Point(204, 25);
            this.checkBoxCQ.Name = "checkBoxCQ";
            this.checkBoxCQ.Size = new System.Drawing.Size(51, 24);
            this.checkBoxCQ.TabIndex = 63;
            this.checkBoxCQ.Text = "CQ CQ";
            this.checkBoxCQ.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.checkBoxCQ, resources.GetString("checkBoxCQ.ToolTip"));
            this.checkBoxCQ.CheckedChanged += new System.EventHandler(this.checkBoxCQ_CheckedChanged);
            // 
            // checkBoxCW
            // 
            this.checkBoxCW.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxCW.Image = null;
            this.checkBoxCW.Location = new System.Drawing.Point(76, 25);
            this.checkBoxCW.Name = "checkBoxCW";
            this.checkBoxCW.Size = new System.Drawing.Size(56, 24);
            this.checkBoxCW.TabIndex = 62;
            this.checkBoxCW.Text = "CW ID";
            this.checkBoxCW.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.checkBoxCW, resources.GetString("checkBoxCW.ToolTip"));
            this.checkBoxCW.CheckedChanged += new System.EventHandler(this.checkBoxCW_CheckedChanged);
            // 
            // chkQuickAudioFolder
            // 
            this.chkQuickAudioFolder.Checked = true;
            this.chkQuickAudioFolder.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkQuickAudioFolder.Image = null;
            this.chkQuickAudioFolder.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.chkQuickAudioFolder.Location = new System.Drawing.Point(8, 431);
            this.chkQuickAudioFolder.Name = "chkQuickAudioFolder";
            this.chkQuickAudioFolder.Size = new System.Drawing.Size(148, 26);
            this.chkQuickAudioFolder.TabIndex = 58;
            this.chkQuickAudioFolder.Text = "QuickAudio Save Folder";
            this.chkQuickAudioFolder.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.chkQuickAudioFolder, "This box should remain Check.\r\nThis allows multiple QuickAudio files to be stored" +
        "\r\nand puts them into a folder called \"QuickAudio\"");
            this.chkQuickAudioFolder.CheckedChanged += new System.EventHandler(this.chkQuickAudioFolder_CheckedChanged);
            // 
            // checkBoxVK1
            // 
            this.checkBoxVK1.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxVK1.Image = null;
            this.checkBoxVK1.Location = new System.Drawing.Point(261, 25);
            this.checkBoxVK1.Name = "checkBoxVK1";
            this.checkBoxVK1.Size = new System.Drawing.Size(51, 24);
            this.checkBoxVK1.TabIndex = 65;
            this.checkBoxVK1.Text = "VK1";
            this.checkBoxVK1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.checkBoxVK1, resources.GetString("checkBoxVK1.ToolTip"));
            this.checkBoxVK1.CheckedChanged += new System.EventHandler(this.checkBoxVK1_CheckedChanged);
            // 
            // checkBoxVK2
            // 
            this.checkBoxVK2.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxVK2.Image = null;
            this.checkBoxVK2.Location = new System.Drawing.Point(318, 25);
            this.checkBoxVK2.Name = "checkBoxVK2";
            this.checkBoxVK2.Size = new System.Drawing.Size(51, 24);
            this.checkBoxVK2.TabIndex = 66;
            this.checkBoxVK2.Text = "VK2";
            this.checkBoxVK2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.checkBoxVK2, resources.GetString("checkBoxVK2.ToolTip"));
            this.checkBoxVK2.CheckedChanged += new System.EventHandler(this.checkBoxVK2_CheckedChanged);
            // 
            // groupBoxTS2
            // 
            this.groupBoxTS2.Controls.Add(this.checkBoxVK2);
            this.groupBoxTS2.Controls.Add(this.checkBoxVK1);
            this.groupBoxTS2.Controls.Add(this.checkBoxTS2);
            this.groupBoxTS2.Controls.Add(this.checkBoxVoice);
            this.groupBoxTS2.Controls.Add(this.checkBoxCQ);
            this.groupBoxTS2.Controls.Add(this.checkBoxCW);
            this.groupBoxTS2.Location = new System.Drawing.Point(12, 362);
            this.groupBoxTS2.Name = "groupBoxTS2";
            this.groupBoxTS2.Size = new System.Drawing.Size(388, 59);
            this.groupBoxTS2.TabIndex = 11;
            this.groupBoxTS2.TabStop = false;
            this.groupBoxTS2.Text = "Create Special QuickAudio Files";
            // 
            // chkBoxMP3
            // 
            this.chkBoxMP3.Checked = true;
            this.chkBoxMP3.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBoxMP3.Image = null;
            this.chkBoxMP3.Location = new System.Drawing.Point(162, 431);
            this.chkBoxMP3.Name = "chkBoxMP3";
            this.chkBoxMP3.Size = new System.Drawing.Size(150, 26);
            this.chkBoxMP3.TabIndex = 60;
            this.chkBoxMP3.Text = "QuickAudio Create MP3\r\n";
            this.chkBoxMP3.CheckedChanged += new System.EventHandler(this.chkBoxMP3_CheckedChanged);
            // 
            // createBoxTS
            // 
            this.createBoxTS.Appearance = System.Windows.Forms.Appearance.Button;
            this.createBoxTS.Enabled = false;
            this.createBoxTS.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.createBoxTS.Image = null;
            this.createBoxTS.Location = new System.Drawing.Point(328, 166);
            this.createBoxTS.Name = "createBoxTS";
            this.createBoxTS.Size = new System.Drawing.Size(72, 24);
            this.createBoxTS.TabIndex = 57;
            this.createBoxTS.Text = "Create Wtr ID";
            this.createBoxTS.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.createBoxTS.Visible = false;
            this.createBoxTS.CheckedChanged += new System.EventHandler(this.createBoxTS_CheckedChanged);
            // 
            // TXIDBoxTS
            // 
            this.TXIDBoxTS.Appearance = System.Windows.Forms.Appearance.Button;
            this.TXIDBoxTS.Enabled = false;
            this.TXIDBoxTS.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TXIDBoxTS.Image = null;
            this.TXIDBoxTS.Location = new System.Drawing.Point(328, 194);
            this.TXIDBoxTS.Name = "TXIDBoxTS";
            this.TXIDBoxTS.Size = new System.Drawing.Size(72, 24);
            this.TXIDBoxTS.TabIndex = 56;
            this.TXIDBoxTS.Text = "WaterID Play";
            this.TXIDBoxTS.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.TXIDBoxTS.Visible = false;
            this.TXIDBoxTS.CheckedChanged += new System.EventHandler(this.TXIDBoxTS_CheckedChanged);
            // 
            // chkQuickPlay
            // 
            this.chkQuickPlay.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkQuickPlay.Enabled = false;
            this.chkQuickPlay.Image = null;
            this.chkQuickPlay.Location = new System.Drawing.Point(328, 256);
            this.chkQuickPlay.Name = "chkQuickPlay";
            this.chkQuickPlay.Size = new System.Drawing.Size(72, 24);
            this.chkQuickPlay.TabIndex = 55;
            this.chkQuickPlay.Text = "Quick Play";
            this.chkQuickPlay.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkQuickPlay.Visible = false;
            this.chkQuickPlay.CheckedChanged += new System.EventHandler(this.chkQuickPlay_CheckedChanged);
            // 
            // chkQuickRec
            // 
            this.chkQuickRec.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkQuickRec.Image = null;
            this.chkQuickRec.Location = new System.Drawing.Point(328, 224);
            this.chkQuickRec.Name = "chkQuickRec";
            this.chkQuickRec.Size = new System.Drawing.Size(72, 24);
            this.chkQuickRec.TabIndex = 54;
            this.chkQuickRec.Text = "Quick Rec";
            this.chkQuickRec.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkQuickRec.Visible = false;
            this.chkQuickRec.CheckedChanged += new System.EventHandler(this.chkQuickRec_CheckedChanged);
            // 
            // groupBoxTS1
            // 
            this.groupBoxTS1.Controls.Add(this.tbPreamp);
            this.groupBoxTS1.Controls.Add(this.udPreamp);
            this.groupBoxTS1.Location = new System.Drawing.Point(320, 80);
            this.groupBoxTS1.Name = "groupBoxTS1";
            this.groupBoxTS1.Size = new System.Drawing.Size(88, 80);
            this.groupBoxTS1.TabIndex = 53;
            this.groupBoxTS1.TabStop = false;
            this.groupBoxTS1.Text = "TX Gain (dB)";
            // 
            // tbPreamp
            // 
            this.tbPreamp.AutoSize = false;
            this.tbPreamp.Location = new System.Drawing.Point(8, 48);
            this.tbPreamp.Maximum = 70;
            this.tbPreamp.Minimum = -70;
            this.tbPreamp.Name = "tbPreamp";
            this.tbPreamp.Size = new System.Drawing.Size(72, 16);
            this.tbPreamp.TabIndex = 53;
            this.tbPreamp.TickFrequency = 35;
            this.tbPreamp.Scroll += new System.EventHandler(this.tbPreamp_Scroll);
            // 
            // udPreamp
            // 
            this.udPreamp.BackColor = System.Drawing.SystemColors.Window;
            this.udPreamp.ForeColor = System.Drawing.SystemColors.ControlText;
            this.udPreamp.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udPreamp.Location = new System.Drawing.Point(16, 24);
            this.udPreamp.Maximum = new decimal(new int[] {
            70,
            0,
            0,
            0});
            this.udPreamp.Minimum = new decimal(new int[] {
            70,
            0,
            0,
            -2147483648});
            this.udPreamp.Name = "udPreamp";
            this.udPreamp.Size = new System.Drawing.Size(40, 20);
            this.udPreamp.TabIndex = 52;
            this.udPreamp.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.udPreamp.ValueChanged += new System.EventHandler(this.udPreamp_ValueChanged);
            this.udPreamp.LostFocus += new System.EventHandler(this.udPreamp_LostFocus);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBoxRecord);
            this.groupBox2.Location = new System.Drawing.Point(320, 8);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(88, 64);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Record";
            // 
            // checkBoxRecord
            // 
            this.checkBoxRecord.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxRecord.Image = null;
            this.checkBoxRecord.Location = new System.Drawing.Point(16, 24);
            this.checkBoxRecord.Name = "checkBoxRecord";
            this.checkBoxRecord.Size = new System.Drawing.Size(56, 24);
            this.checkBoxRecord.TabIndex = 0;
            this.checkBoxRecord.Text = "Record";
            this.checkBoxRecord.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxRecord.CheckedChanged += new System.EventHandler(this.checkBoxRecord_CheckedChanged);
            // 
            // grpPlayback
            // 
            this.grpPlayback.Controls.Add(this.txtCurrentFile);
            this.grpPlayback.Controls.Add(this.lblCurrentlyPlaying);
            this.grpPlayback.Controls.Add(this.btnNext);
            this.grpPlayback.Controls.Add(this.btnPrevious);
            this.grpPlayback.Controls.Add(this.checkBoxPause);
            this.grpPlayback.Controls.Add(this.btnStop);
            this.grpPlayback.Controls.Add(this.checkBoxPlay);
            this.grpPlayback.Location = new System.Drawing.Point(8, 8);
            this.grpPlayback.Name = "grpPlayback";
            this.grpPlayback.Size = new System.Drawing.Size(304, 88);
            this.grpPlayback.TabIndex = 4;
            this.grpPlayback.TabStop = false;
            this.grpPlayback.Text = "Playback";
            // 
            // txtCurrentFile
            // 
            this.txtCurrentFile.Location = new System.Drawing.Point(104, 24);
            this.txtCurrentFile.Name = "txtCurrentFile";
            this.txtCurrentFile.ReadOnly = true;
            this.txtCurrentFile.Size = new System.Drawing.Size(184, 20);
            this.txtCurrentFile.TabIndex = 9;
            // 
            // lblCurrentlyPlaying
            // 
            this.lblCurrentlyPlaying.Image = null;
            this.lblCurrentlyPlaying.Location = new System.Drawing.Point(16, 24);
            this.lblCurrentlyPlaying.Name = "lblCurrentlyPlaying";
            this.lblCurrentlyPlaying.Size = new System.Drawing.Size(96, 23);
            this.lblCurrentlyPlaying.TabIndex = 10;
            this.lblCurrentlyPlaying.Text = "Currently Playing:";
            // 
            // btnNext
            // 
            this.btnNext.Enabled = false;
            this.btnNext.Image = null;
            this.btnNext.Location = new System.Drawing.Point(232, 56);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(40, 23);
            this.btnNext.TabIndex = 8;
            this.btnNext.Text = "Next";
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnPrevious
            // 
            this.btnPrevious.Enabled = false;
            this.btnPrevious.Image = null;
            this.btnPrevious.Location = new System.Drawing.Point(184, 56);
            this.btnPrevious.Name = "btnPrevious";
            this.btnPrevious.Size = new System.Drawing.Size(40, 23);
            this.btnPrevious.TabIndex = 7;
            this.btnPrevious.Text = "Prev";
            this.btnPrevious.Click += new System.EventHandler(this.btnPrevious_Click);
            // 
            // checkBoxPause
            // 
            this.checkBoxPause.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxPause.Enabled = false;
            this.checkBoxPause.Image = null;
            this.checkBoxPause.Location = new System.Drawing.Point(128, 56);
            this.checkBoxPause.Name = "checkBoxPause";
            this.checkBoxPause.Size = new System.Drawing.Size(48, 23);
            this.checkBoxPause.TabIndex = 5;
            this.checkBoxPause.Text = "Pause";
            this.checkBoxPause.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxPause.CheckedChanged += new System.EventHandler(this.checkBoxPause_CheckedChanged);
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Image = null;
            this.btnStop.Location = new System.Drawing.Point(32, 56);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(40, 23);
            this.btnStop.TabIndex = 4;
            this.btnStop.Text = "Stop";
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // checkBoxPlay
            // 
            this.checkBoxPlay.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxPlay.Enabled = false;
            this.checkBoxPlay.Image = null;
            this.checkBoxPlay.Location = new System.Drawing.Point(80, 56);
            this.checkBoxPlay.Name = "checkBoxPlay";
            this.checkBoxPlay.Size = new System.Drawing.Size(40, 23);
            this.checkBoxPlay.TabIndex = 3;
            this.checkBoxPlay.Text = "Play";
            this.checkBoxPlay.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxPlay.CheckedChanged += new System.EventHandler(this.checkBoxPlay_CheckedChanged);
            // 
            // chkAlwaysOnTop
            // 
            this.chkAlwaysOnTop.Image = null;
            this.chkAlwaysOnTop.Location = new System.Drawing.Point(289, 469);
            this.chkAlwaysOnTop.Name = "chkAlwaysOnTop";
            this.chkAlwaysOnTop.Size = new System.Drawing.Size(111, 19);
            this.chkAlwaysOnTop.TabIndex = 65;
            this.chkAlwaysOnTop.Text = "Always On Top";
            this.chkAlwaysOnTop.CheckedChanged += new System.EventHandler(this.chkAlwaysOnTop_CheckedChanged);
            // 
            // WaveControl
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(416, 500);
            this.Controls.Add(this.chkAlwaysOnTop);
            this.Controls.Add(this.checkBoxTS1);
            this.Controls.Add(this.groupBoxTS2);
            this.Controls.Add(this.chkBoxMP3);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.chkQuickAudioFolder);
            this.Controls.Add(this.createBoxTS);
            this.Controls.Add(this.TXIDBoxTS);
            this.Controls.Add(this.chkQuickPlay);
            this.Controls.Add(this.chkQuickRec);
            this.Controls.Add(this.groupBoxTS1);
            this.Controls.Add(this.grpPlaylist);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.grpPlayback);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Menu = this.mainMenu1;
            this.MinimumSize = new System.Drawing.Size(400, 200);
            this.Name = "WaveControl";
            this.Text = "Wave File Controls";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.WaveControl_Closing);
            this.grpPlaylist.ResumeLayout(false);
            this.groupBoxTS2.ResumeLayout(false);
            this.groupBoxTS1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tbPreamp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPreamp)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.grpPlayback.ResumeLayout(false);
            this.grpPlayback.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

       
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.CheckBoxTS checkBoxPlay;
        private System.Windows.Forms.GroupBoxTS groupBox2;
        public System.Windows.Forms.CheckBoxTS checkBoxRecord;
        private System.Windows.Forms.GroupBoxTS grpPlayback;
        private System.Windows.Forms.ButtonTS btnStop;
        private System.Windows.Forms.CheckBoxTS checkBoxPause;
        private System.Windows.Forms.ButtonTS btnPrevious;
        private System.Windows.Forms.ButtonTS btnNext;
        public System.Windows.Forms.ListBox lstPlaylist;
        private System.Windows.Forms.ButtonTS btnAdd;
        private System.Windows.Forms.ButtonTS btnRemove;
        private System.Windows.Forms.CheckBoxTS checkBoxRandom;
        private System.Windows.Forms.GroupBox grpPlaylist;
        private System.Windows.Forms.TextBoxTS txtCurrentFile;
        private System.Windows.Forms.LabelTS lblCurrentlyPlaying;
        private System.Windows.Forms.CheckBoxTS checkBoxLoop;
        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem mnuWaveOptions;
        public System.Windows.Forms.NumericUpDownTS udPreamp;
        private System.Windows.Forms.GroupBoxTS groupBoxTS1;
        private System.Windows.Forms.CheckBoxTS chkQuickRec;
        private System.Windows.Forms.CheckBoxTS chkQuickPlay;

        public System.Windows.Forms.TrackBar tbPreamp;
        public System.Windows.Forms.CheckBoxTS TXIDBoxTS; // ke9ns add
        public System.Windows.Forms.CheckBoxTS createBoxTS; // ke9ns add
        public System.Windows.Forms.CheckBoxTS chkQuickAudioFolder; // ke9ns add
        private System.Windows.Forms.TextBox textBox1;
        public System.Windows.Forms.CheckBoxTS chkBoxMP3;
        public System.Windows.Forms.CheckBoxTS checkBoxVoice;
        public System.Windows.Forms.CheckBoxTS checkBoxCW;
        public System.Windows.Forms.CheckBoxTS checkBoxCQ;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBoxTS groupBoxTS2;
        public System.Windows.Forms.CheckBoxTS checkBoxTS1;
        public System.Windows.Forms.CheckBoxTS checkBoxTS2;
        public System.Windows.Forms.CheckBoxTS chkAlwaysOnTop;
        public System.Windows.Forms.CheckBoxTS checkBoxVK2;
        public System.Windows.Forms.CheckBoxTS checkBoxVK1;

        private System.ComponentModel.IContainer components;

       

    }

}
