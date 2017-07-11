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

using System;
using System.Diagnostics;
using System.Drawing;

using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

//reference Nuget Package NAudio.Lame
using NAudio;
using NAudio.Wave;
using NAudio.Lame;



namespace PowerSDR
{
	public class WaveControl : System.Windows.Forms.Form
	{
        #region Variable Declaration


        private static Bitmap ke9ns_bmp;                    // ke9ns add call sign waterfall tx id

        private Console console;

		private WaveOptions WaveOptions;

		private ArrayList file_list;
        private string wave_folder = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) + "\\PowerSDR";

		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.CheckBoxTS checkBoxPlay;
		private System.Windows.Forms.GroupBoxTS groupBox2;
        public CheckBoxTS checkBoxRecord;
        private System.Windows.Forms.GroupBoxTS grpPlayback;
		private System.Windows.Forms.ButtonTS btnStop;
		private System.Windows.Forms.CheckBoxTS checkBoxPause;
		private System.Windows.Forms.ButtonTS btnPrevious;
		private System.Windows.Forms.ButtonTS btnNext;
        public ListBox lstPlaylist;
        private System.Windows.Forms.ButtonTS btnAdd;
		private System.Windows.Forms.ButtonTS btnRemove;
		private System.Windows.Forms.CheckBoxTS checkBoxRandom;
		private System.Windows.Forms.GroupBox grpPlaylist;
		private System.Windows.Forms.TextBoxTS txtCurrentFile;
		private System.Windows.Forms.LabelTS lblCurrentlyPlaying;
		private System.Windows.Forms.CheckBoxTS checkBoxLoop;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem mnuWaveOptions;
		private System.Windows.Forms.NumericUpDownTS udPreamp;
		private System.Windows.Forms.GroupBoxTS groupBoxTS1;
		private System.Windows.Forms.CheckBoxTS chkQuickRec;
		private System.Windows.Forms.CheckBoxTS chkQuickPlay;
        private System.Windows.Forms.TrackBar tbPreamp;

        #endregion
        public CheckBoxTS TXIDBoxTS; // ke9ns add
        public CheckBoxTS createBoxTS; // ke9ns add
        public CheckBoxTS chkQuickAudioFolder; // ke9ns add
        private TextBox textBox1;
        public CheckBoxTS chkBoxMP3;
        public CheckBoxTS checkBoxVoice;
        public CheckBoxTS checkBoxCW;
        public CheckBoxTS checkBoxCQ;
        private ToolTip toolTip1;
        private GroupBoxTS groupBoxTS2;
        public CheckBoxTS checkBoxTS1;
        public CheckBoxTS checkBoxTS2;
        private CheckBoxTS chkAlwaysOnTop;
        private IContainer components;

		#region Constructor and Destructor

		public WaveControl(Console c)
		{            
			InitializeComponent();
            console = c;

            if (!Directory.Exists(wave_folder))
            {
                // create PowerSDR audio folder if it does not exist
                Directory.CreateDirectory(wave_folder);
            }
            // openFileDialog1.InitialDirectory = console.AppDataPath;
            openFileDialog1.InitialDirectory = String.Empty;
            openFileDialog1.InitialDirectory = wave_folder;
			
			file_list = new ArrayList();
			currently_playing = -1;
			WaveOptions = new WaveOptions();
			this.ActiveControl = btnAdd;
			Common.RestoreForm(this, "WaveOptions", false);
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
            this.checkBoxTS2.Location = new System.Drawing.Point(204, 25);
            this.checkBoxTS2.Name = "checkBoxTS2";
            this.checkBoxTS2.Size = new System.Drawing.Size(72, 24);
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
            this.checkBoxVoice.Location = new System.Drawing.Point(21, 25);
            this.checkBoxVoice.Name = "checkBoxVoice";
            this.checkBoxVoice.Size = new System.Drawing.Size(72, 24);
            this.checkBoxVoice.TabIndex = 61;
            this.checkBoxVoice.Text = "Voice ID";
            this.checkBoxVoice.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.checkBoxVoice, "Record a Voice ID of your callsign.\r\nUsed by the voID Timer feature on the main c" +
        "onsole.\r\n\r\nClick to Start/Stop record a CW ID File: IDTIMER.wav\r\n");
            this.checkBoxVoice.CheckedChanged += new System.EventHandler(this.checkBoxVoice_CheckedChanged);
            // 
            // checkBoxCQ
            // 
            this.checkBoxCQ.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxCQ.Image = null;
            this.checkBoxCQ.Location = new System.Drawing.Point(292, 25);
            this.checkBoxCQ.Name = "checkBoxCQ";
            this.checkBoxCQ.Size = new System.Drawing.Size(72, 24);
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
            this.checkBoxCW.Location = new System.Drawing.Point(110, 25);
            this.checkBoxCW.Name = "checkBoxCW";
            this.checkBoxCW.Size = new System.Drawing.Size(72, 24);
            this.checkBoxCW.TabIndex = 62;
            this.checkBoxCW.Text = "CW ID";
            this.checkBoxCW.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.checkBoxCW, "Record a CW ID of your callsign.\r\nUsed by the cwID Timer feature on the main coso" +
        "le.\r\n\r\nClick to Start/Stop record a CW ID File: IDTIMERCW.wav\r\n\r\nMake sure to be" +
        " in CW mode and open CWX panel.");
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
            // groupBoxTS2
            // 
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
            this.chkAlwaysOnTop.Location = new System.Drawing.Point(301, 469);
            this.chkAlwaysOnTop.Name = "chkAlwaysOnTop";
            this.chkAlwaysOnTop.Size = new System.Drawing.Size(99, 19);
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

		#region Properties

		private int currently_playing;
		private int CurrentlyPlaying
		{
			get { return currently_playing; }
			set
			{
				if(value > lstPlaylist.Items.Count-1) 
					value = lstPlaylist.Items.Count-1;
				
				currently_playing = value;
				if(currently_playing == 0)
					btnPrevious.Enabled = false;
				else 
					btnPrevious.Enabled = true;

				if(currently_playing == lstPlaylist.Items.Count-1)
				{
					if(!checkBoxLoop.Checked)
						btnNext.Enabled = false;
				}
				else
					btnNext.Enabled = true;
			}
		}

        #endregion


        // ke9ns add pass string from console play button right click
        private static string QPFile = null;

        public static string QPFILE
        {
            get { return QPFile; }
            set { QPFile = value; }
        }


        //=======================================================
        // ke9ns add   rec/playid & waterID &  scheduler turn on post audio (called from console)
        public  bool RECPLAY
        {
            get { return false; }

            set
            {
                WaveOptions.RECPLAY1 = value;
               
            }
        } // RECPLAY

        //=======================================================
        // ke9ns add scheduler reduce file size by SR wav file reduction 
        public bool RECPLAY2
        {
            get { return false; }

            set
            {
              
                quickmp3SR = WaveOptions.comboSampleRate.Text; // save SR value before reducing
                WaveOptions.comboSampleRate.Text = "48000"; // reduce file size
            }
        } // RECPLAY2


        // ke9ns add  restore original SR back when done with scheduled recording
        public bool RECPLAY3
        {
            get { return false; }

            set
            {

                WaveOptions.radRXPreProcessed.Checked = WaveOptions.temp_record;
                WaveOptions.radTXPreProcessed.Checked = WaveOptions.temp_record;
               
                WaveOptions.comboSampleRate.Text = quickmp3SR; // restore file size SR
                
            }
        } // RECPLAY3



        #region Misc Routines
        //=============================================================================================================
        //=============================================================================================================
        //ke9ns   Check out wave file to see if it is correct for playing
        //=============================================================================================================
        //=============================================================================================================
        public bool OpenWaveFile(string filename, bool rx2)
		{
			RIFFChunk riff = null;
			fmtChunk fmt  = null;
			dataChunk data_chunk  = null;

			if(!File.Exists(filename))                                      // ke9ns check if file name works
			{
                if (chkQuickAudioFolder.Checked == true)
                {
                    MessageBox.Show("QuickAudio Save Folder Filename doesn't exist. (" + filename + ")\n" +
                    "Close this message, and Right Click on the Play button, then select a file and Click Open,\n" +
                    "You most likely renamed a Quickaudio file which caused this. " , "Bad Filename",
                      MessageBoxButtons.OK,
                      MessageBoxIcon.Error);

                  
                }
                else
                {
                    MessageBox.Show("Filename doesn't exist. (" + filename + ")",
                        "Bad Filename",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    file_list.RemoveAt(currently_playing);
                    
                }
                return false;
            }

			BinaryReader reader = null;
			try
			{
				reader = new BinaryReader(File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.Read));  // ke9ns see if the file can be opened
            }
			catch(Exception)
			{
				MessageBox.Show("File is already open.");
				return false;
			}
            
            if(reader.BaseStream.Length == 0 ||  reader.PeekChar() != 'R')  // ke9ns see if a RIFF type wave file
			{
				reader.Close();
				MessageBox.Show("File is not in the correct format.",
					"Wrong File Format",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error);
				file_list.RemoveAt(currently_playing);
				return false;
			}

			while( (data_chunk == null || riff == null || fmt == null) && (reader.BaseStream.Position < reader.BaseStream.Length) )
			{
				Chunk chunk = Chunk.ReadChunk(ref reader);
				if(chunk.GetType() == typeof(RIFFChunk))
					riff = (RIFFChunk)chunk;
				else if(chunk.GetType() == typeof(fmtChunk))
					fmt = (fmtChunk)chunk;
				else if(chunk.GetType() == typeof(dataChunk))
					data_chunk = (dataChunk)chunk;
			}

            if (reader.BaseStream.Position == reader.BaseStream.Length)
			{
				reader.Close();
				MessageBox.Show("File is not in the correct format.",
					"Wrong File Format",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error);
				file_list.RemoveAt(currently_playing);
				return false;
			}

			if(riff.riff_type != 0x45564157)
			{
				reader.Close();	
				MessageBox.Show("File is not an RIFF Wave file.",
					"Wrong file format",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error);
				file_list.RemoveAt(currently_playing);
				return false;
			}

            if (!TXIDBoxTS.Checked) // ke9ns add
            {                                                                  // format =1 means PCM
                if (!CheckSampleRate(fmt.sample_rate) || (fmt.format == 1 && fmt.sample_rate != Audio.SampleRate1))  // ke9ns needs to be on the list of sample rates BUT needs to match the current SR otherwise it fails
                {
                    reader.Close();
                    MessageBox.Show("File has the wrong sample rate.",
                        "Wrong Sample Rate",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    file_list.RemoveAt(currently_playing);
                    return false;
                }

                if (fmt.channels != 2)                                    // ke9ns must be stereo 
                {
                    reader.Close();
                    MessageBox.Show("Wave File is not stereo.",
                        "Wrong Number of Channels",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    file_list.RemoveAt(currently_playing);
                    return false;
                }

             //  Debug.WriteLine("audio go here"); // here at start of playback

            } // do above if not a WatefallID TX



            //----------------------------------------------------------------------
            // this is where the audio is loaded up into the proper reader.
            //----------------------------------------------------------------------
           
            if (!rx2)
            {
                 Audio.wave_file_reader = new WaveFileReader1(             // ke9ns   RX1 format of RIFF wave file to read
                    this,
                    console.BlockSize1,
                    (int)fmt.format,	// use floating point
                    (int)fmt.sample_rate,
                    (int)fmt.channels,
                    ref reader);
            }
            else
            {
                Audio.wave_file_reader2 = new WaveFileReader1(            // ke9ns   RX2 format of RIFF wave file to read
                    this,
                    console.BlockSize1,
                    (int)fmt.format,	// use floating point
                    (int)fmt.sample_rate,
                    (int)fmt.channels,
                    ref reader);
            }

			return true;

		} // openwavefile





        //=========================================================================================
        // ke9ns  
        private bool CheckSampleRate(int rate)
		{
			bool retval = false;
			switch(rate)
			{
				case 6000:
				case 12000:
				case 24000:
				case 48000:
				case 96000:
				case 192000:
					retval = true; break;
			}
			return retval;
		}


//================================================================
		private void UpdatePlaylist()
		{
			lstPlaylist.BeginUpdate();
			lstPlaylist.Items.Clear();
			int index = lstPlaylist.SelectedIndex;
			foreach(string s in file_list)
			{
				int i = s.LastIndexOf("\\")+1;
				string file = s.Substring(i, s.IndexOf(".wav")-i);
				lstPlaylist.Items.Add(file);
			}

			if(index < 0 && lstPlaylist.Items.Count > 0)
				lstPlaylist.SelectedIndex = 0;
			else if(lstPlaylist.Items.Count > index)
				lstPlaylist.SelectedIndex = index;
			lstPlaylist.EndUpdate();

			if(lstPlaylist.Items.Count > 0)
			{
				checkBoxPlay.Enabled = true;
				btnRemove.Enabled = true;
				checkBoxLoop.Enabled = true;
			}
			else
			{

				checkBoxPlay.Enabled = false;
				checkBoxPlay.Checked = false;
				btnRemove.Enabled = false;
				checkBoxLoop.Enabled = false;
			}

			if(lstPlaylist.Items.Count > 1)
				checkBoxRandom.Enabled = true;
			else
				checkBoxRandom.Enabled = false;				
		}


        //=========================================================================
        //=========================================================================
        // ke9ns   This is what shuts the audio off when end of wave file reached
        //=========================================================================
        //=========================================================================
        public void Next()
		{
			if(checkBoxPlay.Checked)
			{
                if (btnNext.Enabled)
                {
                    btnNext_Click(this, EventArgs.Empty);
                }
                else if (checkBoxLoop.Checked && lstPlaylist.Items.Count == 1)
                {
                    checkBoxPlay_CheckedChanged(this, EventArgs.Empty);
                }
                else
                {
                    checkBoxPlay.Checked = false;
                }
			}
            //k6jca added code...
            if (chkQuickPlay.Checked)
            {
                 chkQuickPlay.Checked = false;
            }

            //=========================================================================
            //ke9ns add comes here when playback has ended
            if (TXIDBoxTS.Checked)
            {
                 TXIDBoxTS.Checked = false;  // tell Waterfall ID routine to turn off since the wave file is done
            }

        } // next()

		#endregion

		#region Event Handlers

		private void openFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
		{
			foreach(string s in openFileDialog1.FileNames)
			{
				if(!file_list.Contains(s))
					file_list.Add(s);
			}

			UpdatePlaylist();
		}

//=========================================================================================
		private void checkBoxPlay_CheckedChanged(object sender, System.EventArgs e)
		{
			if(checkBoxPlay.Checked)
			{
				string filename = (string)file_list[currently_playing];

				if(!OpenWaveFile(filename, false))
				{
					checkBoxPlay.Checked = false;
					currently_playing = -1;
					UpdatePlaylist();
					return;
				}

                if (console.CurrentModel == Model.FLEX5000 && FWCEEPROM.RX2OK && console.RX2Enabled)
                {
                    string filename2 = filename+"-rx2";
                    if(File.Exists(filename2))  OpenWaveFile(filename2, true);
                }

				txtCurrentFile.Text = (string)lstPlaylist.Items[currently_playing];
				checkBoxPlay.BackColor = console.ButtonSelectedColor;
				checkBoxPause.Enabled = true;
			}
			else // if not playing audio
			{
				if(Audio.wave_file_reader != null)	Audio.wave_file_reader.Stop();

                if (Audio.wave_file_reader2 != null)   Audio.wave_file_reader2.Stop();

                Thread.Sleep(50); // wait for files to close

				if(checkBoxPause.Checked) checkBoxPause.Checked = false;
				checkBoxPause.Enabled = false;
				txtCurrentFile.Text = "";
				checkBoxPlay.BackColor = SystemColors.Control;

			}

			Audio.wave_playback = checkBoxPlay.Checked;
			console.WavePlayback = checkBoxPlay.Checked;
         
            //  Debug.WriteLine("audio done here");

        } //checkBoxPlay_CheckedChanged



        public static string scheduleName; // ke9ns add for saving file name of recording
        public static string scheduleName1; // ke9ns add for saving file name of recording
        public static string scheduleName2; // ke9ns add for saving file name of recording

        private void checkBoxRecord_CheckedChanged(object sender, System.EventArgs e)
		{
			if(checkBoxRecord.Checked)
			{
                string rec_type = "";
                int short_sample_rate = 48;
                int long_sample_rate = 4800;
                
                checkBoxRecord.BackColor = console.ButtonSelectedColor;
                
                if (Audio.RecordRXPreProcessed)
                {
                    rec_type = " IQ";
                    long_sample_rate = console.SampleRate1;
                    short_sample_rate = long_sample_rate / 1000;
                }
                else
                {
                    long_sample_rate = WaveOptions.SampleRate;
                    short_sample_rate = long_sample_rate / 1000;
                }
                
				string temp = console.RX1DSPMode.ToString()+" ";
				temp += console.VFOAFreq.ToString("f6")+"MHz [";
                temp += short_sample_rate.ToString() + "k";
                temp += rec_type + "] ";
                temp += DateTime.Now.ToString();
                temp = temp.Replace("/", "-");
                temp = temp.Replace(":", " ");
                // temp = console.AppDataPath + temp;

                temp = wave_folder + "\\" + temp;

                scheduleName2 = temp;

                scheduleName1 = temp + ".mp3"; // ke9ns add 

                string file_name = temp + ".wav";  // ke9ns this is the file created

                scheduleName = file_name; // ke9ns add

                string file_name2 = file_name + "-rx2";
				
				Audio.wave_file_writer = new WaveFileWriter(console.BlockSize1, 2, long_sample_rate, file_name);

                if (console.CurrentModel == Model.FLEX5000 && FWCEEPROM.RX2OK && console.RX2Enabled)
                    Audio.wave_file_writer2 = new WaveFileWriter(console.BlockSize1, 2, long_sample_rate, file_name2);

			} // checkbox record

			
			Audio.wave_record = checkBoxRecord.Checked;

            if (!checkBoxRecord.Checked)
            {
                if (console.CurrentModel == Model.FLEX5000 && FWCEEPROM.RX2OK && console.RX2Enabled && 
                    Audio.wave_file_writer != null && Audio.wave_file_writer2 != null)
                {
                    Thread.Sleep(100);
                    Audio.wave_file_writer2.Stop();
                }

             	Audio.wave_file_writer.Stop();
				checkBoxRecord.BackColor = SystemColors.Control;
				//MessageBox.Show("The file has been written to the following location:\n"+file_name);
			}
        } // checkBoxRecord_CheckedChanged(

        private void btnAdd_Click(object sender, System.EventArgs e)
		{
			openFileDialog1.ShowDialog();
		}

		private void btnRemove_Click(object sender, System.EventArgs e)
		{
			if(lstPlaylist.Items.Count == 0 ||
				lstPlaylist.SelectedIndices.Count == 0) return;
			
			ArrayList selections = new ArrayList();
			
			foreach(int i in lstPlaylist.SelectedIndices)
			{
				if(i == currently_playing && checkBoxPlay.Checked)
				{
                    Application.DoEvents();
					DialogResult dr = MessageBox.Show(
						(string)lstPlaylist.Items[i]+
						" is currently playing.\n"+
						"Stop playing and remove from Playlist?",
						"Stop and Remove?",
						MessageBoxButtons.YesNo,
						MessageBoxIcon.Question);

					if(dr == DialogResult.Yes)
					{
						selections.Add(i);
						checkBoxPlay.Checked = false;

					}
				}
				else
					selections.Add(i);
			}
			
			selections.Sort();

            Application.DoEvents();
			for(int i=selections.Count-1; i>=0; i--)
				file_list.RemoveAt((int)selections[i]);
			UpdatePlaylist();
		}

		private void checkBoxLoop_CheckedChanged(object sender, System.EventArgs e)
		{
			if(checkBoxLoop.Checked)
				checkBoxLoop.BackColor = console.ButtonSelectedColor;
			else
				checkBoxLoop.BackColor = SystemColors.Control;
		}

		private void btnStop_Click(object sender, System.EventArgs e)
		{
			checkBoxPlay.Checked = false;
		}

		private void checkBoxRandom_CheckedChanged(object sender, System.EventArgs e)
		{
			if(checkBoxRandom.Checked)
				checkBoxRandom.BackColor = console.ButtonSelectedColor;
			else
				checkBoxRandom.BackColor = SystemColors.Control;
		}

		private void lstPlaylist_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(lstPlaylist.SelectedIndex < 0)
			{
				btnPrevious.Enabled = false;
				btnNext.Enabled = false;
				return;
			}

			if(!checkBoxPlay.Checked)
			{
				CurrentlyPlaying = lstPlaylist.SelectedIndex;
			}
		}

		private void btnPrevious_Click(object sender, System.EventArgs e)
		{
			if(checkBoxPlay.Checked)
			{
				checkBoxPlay.Checked = false;
				CurrentlyPlaying--;
				checkBoxPlay.Checked = true;
			}
			else
				lstPlaylist.SelectedIndex--;
		}

		private void btnNext_Click(object sender, System.EventArgs e)
		{
			if(checkBoxPlay.Checked)
			{
				checkBoxPlay.Checked = false;
				if(CurrentlyPlaying == lstPlaylist.Items.Count-1)
				{
					CurrentlyPlaying = 0;
				}
				else CurrentlyPlaying++;
				checkBoxPlay.Checked = true;
			}
			else
			{
				int temp = lstPlaylist.SelectedIndex+1;
				if(temp == lstPlaylist.Items.Count) temp = 0;
				lstPlaylist.SelectedIndex = -1;
				lstPlaylist.SelectedIndex = temp;
			}
		}

		private void WaveControl_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			e.Cancel = true;
			this.Hide();
			Common.SaveForm(this, "WaveOptions");
		}

		private void checkBoxPause_CheckedChanged(object sender, System.EventArgs e)
		{
			if(checkBoxPlay.Checked)
				Audio.wave_playback = !checkBoxPause.Checked;

			if(checkBoxPause.Checked)
				checkBoxPause.BackColor = console.ButtonSelectedColor;
			else
				checkBoxPause.BackColor = SystemColors.Control;
		}

		private void lstPlaylist_DoubleClick(object sender, System.EventArgs e)
		{
			if(checkBoxPlay.Checked)
			{
				CurrentlyPlaying = lstPlaylist.SelectedIndex;
				checkBoxPlay.Checked = false;
				checkBoxPlay.Checked = true;
			}
			else checkBoxPlay.Checked = true;
		}

		private void mnuWaveOptions_Click(object sender, System.EventArgs e)
		{
			if(WaveOptions == null || WaveOptions.IsDisposed)
				WaveOptions = new WaveOptions();

			WaveOptions.Show();
			WaveOptions.Focus();
		}

		private void udPreamp_ValueChanged(object sender, System.EventArgs e)
		{
			tbPreamp.Value = (int)udPreamp.Value;
			Audio.WavePreamp = Math.Pow(10.0, (int)udPreamp.Value/20.0); // convert to scalar
		}

		private void udPreamp_LostFocus(object sender, System.EventArgs e)
		{
			udPreamp_ValueChanged(sender, e);
		}

		private void tbPreamp_Scroll(object sender, System.EventArgs e)
		{
			udPreamp.Value = (decimal)tbPreamp.Value;
		}

		#endregion

		//k6jca
		//
		//  Add two buttons for quick record & play without worrying about
		//  all of the other stuff...
		//
		//  Note that these routines automatically set the TX/RX Pre/Post variables
		//	to the proper value for recording off the air (and playing back over the air)
		//	and then return these back to their original values at the end of the invocation
		//  of these functions.
		//
		//	Also - turn off TX EQ (during playback) so that this doesn't modify 
		//  the playback audio.
		//
		//	Record & Playback are always to the same file:  SDRQuickAudio.wav
		//
		private bool temp_record = false;
		private bool temp_play = false;
		private bool temp_mon = false;
        private byte temp_pre = 0; // ke9ns add for quickplay function
		private bool temp_txeq = false;
		private bool temp_cpdr = false;
		private bool temp_dx = false;

        public static int QAC = 0; // ke9ns add
       
        //==================================================================================
        // ke9ns  mod needed since MON now toggle pre and post audio. quickplay should always be post 
        private void chkQuickPlay_CheckedChanged(object sender, System.EventArgs e)
		{
            string file_name;
          
            if (chkQuickAudioFolder.Checked == true) // ke9ns add to allow subfolder with different names to play
            {
                System.IO.Directory.CreateDirectory(console.AppDataPath + "QuickAudio"); // ke9ns create sub directory


                if (console.TIMETOID == true) // ke9ns add
                {
                    console.TIMETOID = false; // reset ID 

                    file_name = console.AppDataPath + "QuickAudio" + "\\IDTIMER.wav";            // ke9ns 

                }
                else if (console.TIMETOID1 == true) // ke9ns add
                {
                    console.TIMETOID1 = false; // reset ID 

                    file_name = console.AppDataPath + "QuickAudio" + "\\IDTIMERCW.wav";            // ke9ns 

                }
                else if (console.CQCQCALL == true) // ke9ns add
                {
                    console.CQCQCALL = false; // reset ID 

                    file_name = console.AppDataPath + "QuickAudio" + "\\CQCQ.wav";            // ke9ns 

                }
                else if (console.CALLCALL == true) // ke9ns add
                {
                    console.CALLCALL = false; // reset ID 

                    file_name = console.AppDataPath + "QuickAudio" + "\\CALL.wav";            // ke9ns 

                }
                else if (QPFile != null)
                {
                    file_name = QPFile; // ke9ns check file name passed from console play button
                    
                }
                else
                {

                  //  file_name = console.AppDataPath + "QuickAudio" + "\\SDRQuickAudio" + QAC.ToString() + QAName() + ".wav";
                    string[] files = Directory.GetFiles(console.AppDataPath + "QuickAudio" + "\\", "SDRQuickAudio" + QAC.ToString() + "*.wav"); // ke9ns ignore extra part of name

                    file_name = files[0];

                }


            }
            else
            {
                file_name = console.AppDataPath + "SDRQuickAudio.wav";
            }
        
           
            if (chkQuickPlay.Checked)
			{
              
                temp_txeq = console.TXEQ;
				console.TXEQ = false;               // set TX Eq temporarily to OFF

				temp_cpdr = console.CPDR;
				console.CPDR = false;

				temp_dx = console.DX;
				console.DX = false;

				temp_play = Audio.RecordTXPreProcessed;
				Audio.RecordTXPreProcessed = true;  // set TRUE temporarily

             //   temp_pre = Audio.MON_PRE; // ke9ns add

                temp_mon = console.MON; // record original MON setting

#if NO_MON
                if (console.RX1DSPMode == DSPMode.AM || console.RX1DSPMode == DSPMode.FM || console.RX1DSPMode == DSPMode.SAM)
                {
                    console.MON = false;
                }
                else
                {
                    console.MON = true;
                }
#else
                if (temp_mon == false) // if original MON was OFF
                {
                    Audio.MON_PRE = 0;

                    console.MONINIT = 1; // ke9ns add

                    console.MON = true; // turn post MON on
                 //   Debug.WriteLine("1mon == false");
                }
                else
                {
                  //  console.MONINIT = 1;
                 //   console.MON = true;
                }
#endif
              
                if (!OpenWaveFile(file_name, false))
				{
                    chkQuickPlay.Checked = false;
                    Audio.MON_PRE = temp_pre;  // ke9ns add

                    if (temp_mon == false)
                    {
                        Audio.MON_PRE = 0;
                        console.MONINIT = 1;
                        console.MON = temp_mon;// turn off MON

                    }
                    else
                    {
                        // just leave it alone
                    }
                  
					Audio.RecordTXPreProcessed = temp_play; //return to original state
					console.TXEQ = temp_txeq;               // set TX Eq back to original state
					return;
				}
                 /*if (console.CurrentModel == Model.FLEX5000 && FWCEEPROM.RX2OK && console.RX2Enabled)
                {
                    string file_name2 = file_name+"-rx2";
                    OpenWaveFile(file_name2, true);
                }*/

                chkQuickPlay.BackColor = console.ButtonSelectedColor;

			} // quickplay checked
			else
			{
				if(Audio.wave_file_reader != null)	Audio.wave_file_reader.Stop();

                /*if (Audio.wave_file_reader2 != null)
                    Audio.wave_file_reader2.Stop();*/

                chkQuickPlay.BackColor = SystemColors.Control;
				console.QuickPlay = false;

                if (temp_mon == false)
                {
                    Audio.MON_PRE = 0;
                    console.MONINIT = 1;
                    console.MON = false;// turn off MON
                  //  Debug.WriteLine("mon == false");

                }
                else
                {
                    // just leave it alone
                }

                Audio.RecordTXPreProcessed = temp_play; //return to original state
				console.TXEQ = temp_txeq;               // set TX Eq back to original state
				console.CPDR = temp_cpdr;
				console.DX = temp_dx;
			}

			Audio.wave_playback = chkQuickPlay.Checked;  // PLAY AUDIO FILE HERE

			console.WavePlayback = chkQuickPlay.Checked;

         
        }// quickplay changed


        public static string quickmp3SR; // ke9ns add

        public static string quickmp3; // ke9ns add
        //============================================================================================
        private void chkQuickRec_CheckedChanged(object sender, System.EventArgs e)
		{
			if(chkQuickRec.Checked)
			{

                temp_record = Audio.RecordRXPreProcessed;
                quickmp3SR = WaveOptions.comboSampleRate.Text;

                if (chkBoxMP3.Checked == true)
                    WaveOptions.comboSampleRate.Text = "48000"; // reduce file size

                Audio.RecordRXPreProcessed = false;                            //ke9ns add  set this FALSE temporarily

				chkQuickRec.BackColor = console.ButtonSelectedColor;

				chkQuickPlay.Enabled = true;

                string file_name;

                if (chkQuickAudioFolder.Checked == true)
                {
                    QAC++;
                    System.IO.Directory.CreateDirectory(console.AppDataPath + "QuickAudio"); // ke9ns add create sub directory
                    System.IO.Directory.CreateDirectory(console.AppDataPath + "QuickAudioMP3"); // ke9ns add create sub directory

                    file_name = console.AppDataPath + "QuickAudio"+ "\\SDRQuickAudio" + QAC.ToString() + QAName() + ".wav";

                    quickmp3 = console.AppDataPath + "QuickAudioMP3" + "\\SDRQuickAudio" + QAC.ToString() + QAName() +".mp3"; // ke9ns add mp3
                    
                    //   Debug.WriteLine("qac" + QAC);

                }
                else
                {

                    file_name = console.AppDataPath + "SDRQuickAudio.wav";

                    quickmp3 = console.AppDataPath + "SDRQuickAudio.mp3"; // ke9ns add mp3

                }

				Audio.wave_file_writer = new WaveFileWriter(console.BlockSize1, 2, WaveOptions.SampleRate, file_name);

                /*if (console.CurrentModel == Model.FLEX5000 && FWCEEPROM.RX2OK && console.RX2Enabled)
                {
                    string file_name2 = file_name + "-rx2";
                    Audio.wave_file_writer2 = new WaveFileWriter(console.BlockSize1, 2, waveOptionsForm.SampleRate, file_name2);
                }*/

			} //chkQuickRec.checked
			
			Audio.wave_record = chkQuickRec.Checked;

            if (!chkQuickRec.Checked)
			{
                /*if (console.CurrentModel == Model.FLEX5000 && FWCEEPROM.RX2OK && console.RX2Enabled && Audio.wave_file_writer2 != null)
                {
                    Thread.Sleep(100);
                    Audio.wave_file_writer2.Stop();
                }*/

				string file_name = Audio.wave_file_writer.Stop();

				chkQuickRec.BackColor = SystemColors.Control;

                if (console.checkBoxID.Checked == false) // ke9ns add
                {
                    MessageBox.Show("The 'Over the Air' Quick audio recording has been successfully created.\n" +
                        "Key the radio with either PTT or MOX and click on the Play button to play back the Quick audio recording over the air.");
                    // MessageBox.Show("The file has been written to the following location:\n"+file_name);
                }

                Audio.RecordRXPreProcessed = temp_record; //return to original state
                WaveOptions.comboSampleRate.Text = quickmp3SR; // restore file size

                //---------------------------------------------------------
                // ke9ns add save an MP3 to go along with the WAV file (NAudio.Lame)
                if (chkBoxMP3.Checked == true)
                {
                 
                    
                    try
                    {
                        using (var reader = new WaveFileReader(file_name)) // closes reader when done using
                        using (var writer = new LameMP3FileWriter(quickmp3, reader.WaveFormat, LAMEPreset.VBR_90)) // closes writer when done using (90=90% quality variable bit rate)
                        {
                            reader.CopyTo(writer);
                        }
                    }
                    catch (Exception)
                    {
                       
                    }
                    Debug.WriteLine("DONE WITH MP3 CREATION" + quickmp3);

                }

            } //   if (!chkQuickRec.Checked)

        } //  chkQuickRec_CheckedChanged

        public bool QuickRec
		{
			get { return chkQuickRec.Checked; }
			set	{ chkQuickRec.Checked = value; }
		}

		public bool QuickPlay
		{
			get {
             
                return chkQuickPlay.Checked;
            }
			set	{
                   chkQuickPlay.Checked = value;
            }
		}

        //=========================================================================
        public bool TXIDPlay // ke9ns ADD for TX waterfall ID
        {
            get { return TXIDBoxTS.Checked; }
            set
            {
                TXIDBoxTS.Checked = value;
            }
        }

     


        //=========================================================================================
        //=========================================================================================
        //=========================================================================================
        // ke9ns add:  Play waterfall ID wave file here (below createBoxTS creates the wave file)
        //=========================================================================================
        //=========================================================================================
        //=========================================================================================
        public int samplesPerSecondL = 0;
       
        private void TXIDBoxTS_CheckedChanged(object sender, EventArgs e) // ke9ns ADD TX waterfall ID
        {


            string file_name = console.AppDataPath + "ke9ns.wav"; // TEXT to waterfall image only


            //=========================================================================================
            // Determine if Sending Waterfall or not
            //=========================================================================================

            if (TXIDBoxTS.Checked)  
            {

                 // the wave file will have been created if the callsign text box is green. The code should prevent you coming here if its not green.


                //=========================================================================================
                //=========================================================================================
                //=========================================================================================
                // Send Digital PCM to QuickPlay Audio stream
                //=========================================================================================
                //=========================================================================================
                //=========================================================================================

                console.TXIDMenuItem.Text = "Transmit";

            //-------------------------------------------------------------------------------
            // play ke9ns.wav file here
            //-------------------------------------------------------------------------------


                temp_txeq = console.TXEQ;
                console.TXEQ = false;               // set TX Eq temporarily to OFF

                temp_cpdr = console.CPDR;
                console.CPDR = false;

                temp_dx = console.DX;
                console.DX = false;

                temp_play = Audio.RecordTXPreProcessed;
                Audio.RecordTXPreProcessed = true;  // set TRUE temporarily

                temp_mon = console.MON;

                //  console.MON = false;

             //   Debug.WriteLine("playing ");

                if (!OpenWaveFile(file_name, false))
                {
                     TXIDBoxTS.Checked = false;
                   
                    console.TXIDMenuItem.Checked = false;
                    console.chkMOX.Checked = false;
                    console.MON = temp_mon;
                    Audio.RecordTXPreProcessed = temp_play; //return to original state
                    console.TXEQ = temp_txeq;               // set TX Eq back to original state
                    return;
                }

                

                TXIDBoxTS.BackColor = console.ButtonSelectedColor;
          
            } // txidboxts checked
            else// this is what signals the end of playback
            {
                if (Audio.wave_file_reader != null) Audio.wave_file_reader.Stop();
             
                TXIDBoxTS.BackColor = SystemColors.Control;
               
               //  Debug.WriteLine("DONE playing ");

                console.MON = temp_mon;
                Audio.RecordTXPreProcessed = temp_play; //return to original state
                console.TXEQ = temp_txeq;               // set TX Eq back to original state
                console.CPDR = temp_cpdr;
                console.DX = temp_dx;

                console.TXIDMenuItem.Checked = false;  // turn off TX waterfall ID here

            } //  txidboxts not checked


            Audio.wave_playback = TXIDBoxTS.Checked;  // this trigger audio.cs to play audio this is read in console.cs 
            console.WavePlayback = TXIDBoxTS.Checked; // this triggers the audio playback

                   

        } // TXIDBoxTS checkchanged


        //=========================================================================
        public bool CreatePlay // ke9ns ADD for TX waterfall ID  (comes from console.callsignTextBox)
        {
            get { return createBoxTS.Checked; }
            set
            {
                createBoxTS.Checked = value;


            }
        }

        //=========================================================================================
        //=========================================================================================
        //=========================================================================================
        // ke9ns ADD   this routine creates the waterfall ID (txidBoxTS plays it)
        //             when mouse moved in/out a thread routine starts which converts image to wav
        //=========================================================================================
        //=========================================================================================
        //=========================================================================================
        public DSPMode BandL = 0;
        public int TxfhL = 0;

        private void createBoxTS_CheckedChanged(object sender, EventArgs e)
        {
            if (createBoxTS.Checked)
            {
              //  Debug.WriteLine("check create");
               
                if ((console.Callsign != console.LastCall) || (console.RX1DSPMode != BandL) || ((console.WIDEWATERID == true) && (console.TXFilterHigh != TxfhL)) )  // check if we need to create a new wave file or use the old one.
                {
                    TxfhL = console.TXFilterHigh;

                    Thread t = new Thread(new ThreadStart(CreateWaterfallID));
                    t.Name = "Create Waterfall ID wave file Thread";
                    t.IsBackground = true;
                    t.Priority = ThreadPriority.Normal;
                    t.Start();
                } // console.Callsign != console.LastCall) || (console.RX1DSPMode != BandL))
                else
                {
                    console.callsignTextBox.BackColor = Color.MediumSpringGreen;  // green if your last callsign is still a valid wave
                    console.menuStrip1.Invalidate();
                    console.menuStrip1.Update();
                    createBoxTS.Checked = false;  // do only 1 time
                }
     

            } // createBoxTS.Checked)


        }// createBoxTS_CheckedChanged



        //============================================================================================
        //============================================================================================
        // ke9ns add   THREAD process to create wave file from text or bitmap image here
        //============================================================================================
        //============================================================================================
        private void CreateWaterfallID()
        {
           
          
                    string file_name = console.AppDataPath + "ke9ns.wav"; // TEXT to waterfall image only
                    string file_name1 = console.AppDataPath + "ke9ns.bmp"; // image file to waterfall

                
                    console.callsignTextBox.BackColor = Color.PaleVioletRed; // let user know your creating a new wave file
                    console.menuStrip1.Invalidate();
                    console.menuStrip1.Update();


                    console.LastCall = console.Callsign;        // check if changed callsign, mode, or sample rate
                    samplesPerSecondL = console.SampleRate1;
                    BandL = console.RX1DSPMode;

                    int IMAGE = 0;                      // 0=text, 1=image

          //  t.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
          //  t.CurrentUICulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");

                    if (console.Callsign.EndsWith(".") == true)
                    {
                        IMAGE = 1; // get real image file

                        file_name1 = console.AppDataPath + console.Callsign + "bmp";    //    image file to waterfall
               
                    }

                    double bright = 500;      // ke9ns volume level (was 400 amplitude factor_

                    int n1 = 0;   // used by USB/LSB routine
                    int n2 = 0;
                    int n3 = 0;
                    int n4 = 0;
                    SizeF cl = new SizeF();  // determine left or right side of bitmap

                    int fontS = 34; // was 12

                    int ym = 40; // height was 22

                    if (IMAGE == 0)
                    {
                         ym = 26; // was 22
                    }

                    int xm = 100; // width was 80

            

                    long xm4 = 4 * ((xm + 3) / 4);

                    const float bw = .114F;   // factors to convert RGB color to grayscale
                    const float gw = .587F;
                    const float rw = .2989F;

                    byte[,] ap = new byte[xm + 10, ym + 10];  // get bitmap data


                    //=========================================================================================
                    //=========================================================================================
                    // Choose TEXT or BITMAP
                    //=========================================================================================
                    //=========================================================================================
             
                    if (IMAGE == 0) // text = 0
                    {


                        //=========================================================================================
                        // 24bit TEXT Bitmap Generate and SCAN
                        //=========================================================================================
                   
                        ke9ns_bmp = new Bitmap(xm, ym, PixelFormat.Format24bppRgb);  // initialize bitmap for call sign insertion
                        Graphics g1 = Graphics.FromImage(ke9ns_bmp);

                        g1.SmoothingMode = SmoothingMode.AntiAlias;
                        g1.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        g1.PixelOffsetMode = PixelOffsetMode.HighQuality;

                     //   Pen p = new Pen(Color.AntiqueWhite, 1);                // pen color white
                     //   g1.DrawRectangle(p, 1, 4, xm - 1, ym - 4);      // draw box


                        n4 = 0;          //  USB 
                      string temp1A = console.Callsign;
                      int temp2 = console.Callsign.Length;


             

                    if (temp2 < 8)  //if the callsign text is less then 7 char, then add space between each character to make it more legible 
                    {
                        temp1A = "";
                        for (int z = 0; z < temp2; z++)
                        {
                            temp1A = temp1A + console.Callsign.Substring(z, 1);

                            if (z != (temp2 - 1)) temp1A = temp1A + " ";

                        }
                    }
                    

                Debug.WriteLine("CALLSIGN [" + temp1A + "]");

                do
                {
                    cl = g1.MeasureString(temp1A, new Font("Arial", fontS, FontStyle.Regular)); //  temp used to determine the size of the string when in LSB and you need to reserve a certain space
                    fontS--;
                } while ((cl.Width > 100) && (fontS > 9));  // 89 ke9ns reduce size of font until the string fits into bandpass


                Debug.WriteLine("MEASUREMENT LENGTH "+ cl + " , " + fontS + " , "+ xm);
               // ke9ns cl = 89 is max width in pixels

                        // cl = g1.MeasureString(console.Callsign, new Font("Arial", fontS,FontStyle.Regular)); //  temp used to determine the size of the string when in LSB and you need to reserve a certain space

                        //   cl = g1.MeasureString(console.Callsign, new Font(SystemFonts.DefaultFont,FontStyle.Regular)); //  temp used to determine the size of the string when in LSB and you need to reserve a certain space
                        cl.Width = (xm / 2) - (cl.Width / 2);
                        cl.Height = (ym / 2) - (cl.Height / 2) + 2;

                        //===========================================================================================
                        // find if USB or LSB on RX1 or RX2 ?

                        if (console.chkVFOATX.Checked == true)
                        {
                            if (console.RX1DSPMode == DSPMode.LSB) n4 = 1;

                        }
                        else // not vfoa
                        {
                            if (console.RX2DSPMode == DSPMode.LSB) n4 = 1;  //  for (int n = 1; n != xm; n=n+1)  // displays right to left  (LSB) 

                        } // vfoA

                     // new Font("Arial", fontS),
                      g1.DrawString(temp1A, new Font("Arial", fontS,FontStyle.Regular), Brushes.White, cl.Width, cl.Height); // determine USB or LSB, then draw callsign into bitmap

                      //  g1.DrawString(console.Callsign, new Font(SystemFonts.DefaultFont,FontStyle.Regular), Brushes.AntiqueWhite, cl.Width, cl.Height); // determine USB or LSB, then draw callsign into bitmap
                        g1.Flush();  // done with graphic function

             
                        //=======================================================================
                        // CONVERT BITMAP into ARRAY of float values for brightness x,y
                        // ap[,] = vales between 0 and 1 (0=dark, 255=white)


                       for (int y = ym - 1; y >= 0; y--) // image is saved in correct direction
                        {

                            for (int n = 0; n != xm; n++)  // 
                            {

                                Color pixel = ke9ns_bmp.GetPixel(n, y);                                  // bitmap is correct x direction but y is backwards
                                float temp1 = (((bw * (float)pixel.B) + (gw * (float)pixel.G) + (rw * (float)pixel.R)));

                                byte temp = (byte)(temp1 * 0.6);

                       
                                // look at picture data and pick color based on index value found, AND invert so white = BLACK (no signal) , white = full signal (thats the 255- part)
                                // the /255 is to convert down to a 

                                if (n4 == 0) // USB
                                {
                                     ap[n, ym - y - 1] = temp;
                                }
                                else // LSB  
                                {
                                     ap[xm - n, ym - y - 1] = temp;
                                }
                      

                            } // n

                        } // Y

               
                        ke9ns_bmp.Dispose(); // text image
                        g1.Dispose();


                        //===========================================================================================
                        // end TEXT  BITMAP 24bit

                    } // IMAGE 0=text here
                    else
                    {

                        //=========================================================================================
                        // Get 24bit or 8bpp Bitmap Image loaded up 
                        //=========================================================================================
                 
                        if (!File.Exists(file_name1))
                        {
                            MessageBox.Show("Filename doesn't exist. (" + file_name1 + ")",
                                "Bad Filename",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                            TXIDBoxTS.Checked = false;

                            console.TXIDMenuItem.Checked = false;  // turn off TX waterfall ID here

                            createBoxTS.Checked = false;  // do only 1 time

                            return;
                        }

                        FileStream stream1 = new FileStream(file_name1, FileMode.Open); // open BMP  file
                        BinaryReader reader = new BinaryReader(stream1);

                        reader.ReadChars(10);               // ignore BM characters to start bitmap header
                        long hrdlen = reader.ReadInt32();   // offset in bitmap to first byte of image data
                        reader.ReadChars(4);                // header size ignore
                        long xm1 = reader.ReadInt32();       // width in pixels
                        long ym1 = reader.ReadInt32();       // height in pixels
                        reader.ReadBytes(2);                // color plane assumed to be 1
                        int bpp = reader.ReadInt16();       // reader.ReadBytes(2);                // bpp assumed to be 8bpp
                        reader.ReadBytes(24);               // ignore rest of header to get to color values (255 in 8bpp set)

                        xm = (int)xm1;                      // convert to xm 
                        ym = (int)ym1;                      // convert to ym 


                        int color24 = 1;

                        if (bpp == 24) color24 = 1;
                        else color24 = 0;


                        //=========================================================================================
                        // 24 bit color Image bitmap scan
                        //=========================================================================================
                
                        if (color24 == 1)  // 24bpp color grayscale (no color index needed)
                        {

                            xm4 = xm;

                            ap = new Byte[xm + 10, ym + 10];  // convert to grayscale


                            //===========================================================================================
                            // find if USB or LSB on RX1 or RX2 ?

                            n4 = 0;  //  USB 

                            if (console.chkVFOATX.Checked == true)
                            {
                                if (console.RX1DSPMode == DSPMode.LSB) n4 = 1;

                            }
                            else // not vfoa
                            {
                                if (console.RX2DSPMode == DSPMode.LSB) n4 = 1;

                            } // vfoA


                    //=============================================================================================
                    // convert bitmap into grayscale bitmap corrected for sending
                    // ap[,] = vales between 0 and 1 (0=dark, 255=white)

                   
                            for (int y = 0; y < ym; y++) // image is saved in correct direction
                            {
                                for (int n = 0; n != xm; n++)  // 
                                {
                            // look at picture data and pick color based on index value found, AND invert so white = BLACK (no signal) , white = full signal (thats the 255- part)
                            // the /255 is to convert down to a 

                                    float temp1 = ((bw * (float)reader.ReadByte()) + (gw * (float)reader.ReadByte()) + (rw * (float)reader.ReadByte()));
                                    byte temp = (byte)(temp1 * 0.45); // reduce overall max value to prevent overdriving sine waves

                                    if (n4 == 0) // USB
                                    {
                                       ap[n, y] = (byte)(114 - temp);

                                       if (ap[n, y] < 10) ap[n, y] = 0;

                                 
                                    }
                                    else // LSB  
                                    {
                                       ap[xm - n, y] = (byte)(114 - temp);
                                       if (ap[xm - n, y] < 10) ap[xm - n, y] = 0;

                                    }


                                } // X
                       

                    } // Y

                   
                } // color24 ==1

                        //=========================================================================================
                        // 256 color 8bpp Image bitmap scan
                        //=========================================================================================
                
                        else   // 8bpp 256 color grayscale
                        {

                            byte[] col = new Byte[255 + 1];  // color map
                            xm4 = 4 * ((xm + 3) / 4);
                            byte[] ri = new byte[xm4 + 1];

                            ap = new byte[xm4 + 10, ym + 10];  // get bitmap data
                                                               // ap1 = new float[xm4 + 10, ym + 10];  // convert to grayscale

                            // color mapping
                            for (int n = 0; n < 256; n++)
                            {
                                col[n] = (byte)((bw * (float)reader.ReadByte()) + (gw * (float)reader.ReadByte()) + (rw * (float)reader.ReadByte()));
                                reader.ReadByte(); // ignore 4th byte
                            }

                            //===========================================================================================
                            // find if USB or LSB on RX1 or RX2 ?

                            if (console.chkVFOATX.Checked == true)
                            {
                                if (console.RX1DSPMode == DSPMode.LSB)
                                {
                                    n1 = 1;
                                    n2 = xm;
                                    n3 = 1;
                                    n4 = 1;
                                }
                                else
                                {
                                    n2 = 0;
                                    n1 = xm;
                                    n3 = -1;
                                    n4 = 0;  //  USB must flip around here
                                }
                            }
                            else // not vfoa
                            {
                                if (console.RX2DSPMode == DSPMode.LSB)   //  for (int n = 1; n != xm; n=n+1)  // displays right to left  (LSB) 
                                {
                                    n1 = 1;
                                    n2 = xm;
                                    n3 = 1;
                                    n4 = 1;
                                }
                                else //  for (int n = (int)xm; n != 0; n--)  // displays correctly left to right (USB)
                                {
                                    n2 = 0;
                                    n1 = xm;
                                    n3 = -1;
                                    n4 = 0;  //  USB must flip around here
                                }

                            } // vfoA

                            //=============================================================================================
                            // convert bitmap into grayscale bitmap corrected for sending
                            // float ap[,] = vales between 0 and 1 (0=dark, 255=white)


                            for (int y = 0; y < ym; y++) // image is saved in correct direction
                            {
                                ri = reader.ReadBytes((int)xm4);                // get entire line of bitmap X

                                for (int n = n1; n != n2; n = n + n3)  // 
                                {
                                    // look at picture data and pick color based on index value found, AND invert so white = BLACK (no signal), white = full signal

                                    if (n4 == 0) // USB
                                    {
                                        ap[n1 - n, y] = (byte)((byte)255 - col[ri[xm - n]]);  // flip
                                        if (ap[n1 - n, y] < 5) ap[n1 - n, y] = 0;

                                    }
                                    else // LSB
                                    {
                                        ap[n, y] = (byte)((byte)255 - col[ri[xm - n]]); // dont flip
                                        if (ap[n, y] < 5) ap[n, y] = 0;

                                    }

                                } // X

                            } // Y

                        } // color24 == 0 8bpp 


                        reader.Close();    // close wav file
                        stream1.Close();   // close stream

                        //===============================================================================
                        // end IMAGE bitmap

                    } // IMAGE == 1 image file



                    //=========================================================================================
                    // Digital PCM  setup and file creation
                    //=========================================================================================
              
                    FileStream stream = new FileStream(file_name, FileMode.Create); // create a wav file
                    BinaryWriter writer = new BinaryWriter(stream);                   // create a stream to write to the wav file

                    //=================================================
                    // .wav header
                    const int RIFF = 0x46464952;                      // 0x46464952  (0x52494646 big-endian form).
                    const int WAVE = 0x45564157;                      // 0x45564157  (0x57415645 big-endian form).
                    const int formatChunkSize = 16;                   // 16
                    const int headerSize = 8;                         // 8
                    const int format = 0x20746D66;                    // 0x20746D66   0x666d7420
                    const short formatType = 1;                       // 1 PCM
                    const short tracks = 1;                           // 1 Stereo = 2
                    const int samplesPerSecond = 48000;                     // console.SampleRate1;    (fixed flex routine to resample audio to correct SR)
                    const short bitsPerSample = 16;                         // 16

                    short frameSize = (short)(tracks * ((bitsPerSample + 7) / 8));   // tracks 2.875  @ 8000 sps

                    int bytesPerSecond = samplesPerSecond * frameSize;              // 23000 bytes per second

                    int waveSize = ym;

                    const int lowtx = 150;

                    int bandpass = 2400;

                    if (console.WIDEWATERID == true)
                    {
                        if (console.TXFilterHigh > 2400) bandpass = (console.TXFilterHigh - 150);
                        else bandpass = 2400;

                    }
                    else
                    {
                        bandpass = 2400;
                
                    }

                    int t2 = 2;                                                     // divide the time for each line (this controls how long each line takes)

                    if (IMAGE == 1)
                    {
                        float t3 = ((float)xm / (float)ym);         // picture ratio to maintain in waterfall (80w x 80h = 1) (200w x 150h = 1.33) (80w x 20h = 4)
                                                                    // float t4 = ((float)bandpass / (float)xm);  // number of freqs in width of images (200w = 13hz per pixel)longer time .076pix/hz,  (80w= 30hz per pixel)shorter time.033pix/hz
                        t2 = (int)((float)xm * 2 / 20 / (t3+ 1));        // t2 small means long vertical , t2 big means short (20 is max)

                        if (t2 > 20) t2 = 20;
                    }


                    int data = 0x61746164;                                          // (0x64617461 big-endian form).

                    int samples = (samplesPerSecond * 2) * waveSize / t2;            // file size               //88200 * 4 = 352800

                    int dataChunkSize = samples * frameSize / 2;                        // 46000 

                    int fileSize = (waveSize) + headerSize + formatChunkSize + headerSize + dataChunkSize;


                    writer.Write(RIFF);                            // write header to file
                    writer.Write(fileSize);
                    writer.Write(WAVE);
                    writer.Write(format);
                    writer.Write(formatChunkSize);
                    writer.Write(formatType);
                    writer.Write(tracks);
                    writer.Write(samplesPerSecond);
                    writer.Write(bytesPerSecond);
                    writer.Write(frameSize);
                    writer.Write(bitsPerSample);
                    writer.Write(data);
                    writer.Write(dataChunkSize);

                    //==============================================================
                    // used if you want the waterfall to be as wide as your transmission bandpass
                    //  int lowtx = console.TXFilterLow;
                    //  int hightx = console.TXFilterHigh;
                    //   int tottx = hightx - lowtx; // band width

                    // xm = 80 pixels wide
                    int hzperpixel = (int)(((float)bandpass / (float)xm ) + .5);    //  2khz wide =  25hz/pix  6000 = 75 hz per pixel
            
           
          //  Debug.WriteLine("hzperpixel " + hzperpixel);

                    int sample2 = samplesPerSecond / t2;      // samples2 = the amount of time spent on each y1 line

                    // ap[,] = vales between 0 and 1 (0=dark, 255=white)

                    bright = bright / (float)xm; // correct brightness level of PCM audio by how many freq points go into each pass

            //=========================================================================================
            // Create Digital PCM Stream
            //=========================================================================================

            
                    for (int y1 = 0; y1 < ym; y1++)   // each line (bottom of bitmap is first line out)
                    {

                        for (int n = 0; n < sample2; n++)           //  (generate tone) 
                        {
                            double t = (double)n / (double)(samplesPerSecond * 2);          // used to generate a tone

                            double temp7 = 0.0;
                            int i = 0;

                            //===========================================================
                            // depending on width of bitmap, display within 150 hz to 2550hz

                            for (float freq = lowtx; freq < (bandpass + lowtx); freq = freq + hzperpixel,i++)             // add up all the frequencies from each line(row) of the bitmap into 1 signal
                            {

                                if (ap[i, y1] > 1)
                                {
                                    temp7 = temp7 + ((double)ap[i, y1] * (Math.Sin(t * (double)freq * 2.0 * Math.PI) ) );  // generate individual tone 
                              
                                }




                            } // freq loop

                    //============================================================
                 

                           short s = (short)(temp7 * bright);

                            writer.Write(s);  // left 16bits
                                              //   writer.Write(s);  // right 16bits (this channel is needed by Flex Quickplay routine)

                        } // n  X  1 line at a time

                    } // Y


                    writer.Close();    // close wav file
                    stream.Close();   // close stream

                
       
                console.callsignTextBox.BackColor = Color.MediumSpringGreen;  // green if you created it or its still a valid wave
                console.menuStrip1.Invalidate();
                console.menuStrip1.Update();

  
            createBoxTS.Checked = false;  // do only 1 time

        } // CreateWaterfallID (this is run as a Thread)



        //=============================================================================================
        // ke9ns 
        public static byte QAF = 0;
        public void chkQuickAudioFolder_CheckedChanged(object sender, EventArgs e)
        {

            if (chkQuickAudioFolder.Checked)
            {
                System.IO.Directory.CreateDirectory(console.AppDataPath + "QuickAudio"); // ke9ns create sub directory
     
            }
            else
            {

            }

        } // chkQuickAudioFolder_CheckedChanged


        // ke9ns add
        private void chkBoxMP3_CheckedChanged(object sender, EventArgs e)
        {

        }


        //===========================================================================
        // ke9ns add
        private void checkBoxVoice_CheckedChanged(object sender, EventArgs e)
        {
            chkQuickAudioFolder.Checked = true;
            console.checkBoxID.Checked = true;

           
            if (console.ckQuickRec.Checked == true) // recorder already running
            {
                console.ckQuickRec.Checked = false;
              //  string file_name = console.AppDataPath + "QuickAudio" + "\\SDRQuickAudio" + QAC.ToString(); // + ".wav";
                string file_name1 = console.AppDataPath + "QuickAudio" + "\\IDTIMER.wav";

                string[] files = Directory.GetFiles(console.AppDataPath + "QuickAudio" + "\\", "SDRQuickAudio" + QAC.ToString() +"*.wav"); // ke9ns find file but ignore extra information about file

                MessageBox.Show("Done Recording.\nIDTIMER.wav file will be created." );

               // System.IO.File.Copy(file_name, file_name1,true);
                System.IO.File.Copy(files[0], file_name1, true);
            }
            else
            {
                console.ckQuickRec.Checked = true; // start recording
            }


        } // checkBoxVoice_CheckedChanged


        // ke9ns add
        private void checkBoxCW_CheckedChanged(object sender, EventArgs e)
        {
            chkQuickAudioFolder.Checked = true;
            console.checkBoxID.Checked = true;

            if (console.ckQuickRec.Checked == true) // recorder already running
            {
                console.ckQuickRec.Checked = false;
              //  string file_name = console.AppDataPath + "QuickAudio" + "\\SDRQuickAudio" + QAC.ToString() + ".wav";
                string file_name1 = console.AppDataPath + "QuickAudio" + "\\IDTIMERCW.wav";

                string[] files = Directory.GetFiles(console.AppDataPath + "QuickAudio" + "\\", "SDRQuickAudio" + QAC.ToString() + "*.wav"); // 

                MessageBox.Show("Done Recording.\nIDTIMERCW.wav file will be created.");

                System.IO.File.Copy(files[0], file_name1, true);

            }
            else
            {
                console.ckQuickRec.Checked = true; // start recording
            }


        } // checkBoxCW_CheckedChanged


        // ke9ns add
        private void checkBoxCQ_CheckedChanged(object sender, EventArgs e)
        {
            chkQuickAudioFolder.Checked = true;
            console.checkBoxID.Checked = true;

            if (console.ckQuickRec.Checked == true) // recorder already running
            {
                console.ckQuickRec.Checked = false;
              //  string file_name = console.AppDataPath + "QuickAudio" + "\\SDRQuickAudio" + QAC.ToString() + ".wav";
                string file_name1 = console.AppDataPath + "QuickAudio" + "\\CQCQ.wav";

                string[] files = Directory.GetFiles(console.AppDataPath + "QuickAudio" + "\\", "SDRQuickAudio" + QAC.ToString() + "*.wav"); // 

                MessageBox.Show("Done Recording.\nCQCQ.wav file will be created.");

                System.IO.File.Copy(files[0], file_name1, true);

            }
            else
            {
                console.ckQuickRec.Checked = true; // start recording
            }

        } //checkBoxCQ_CheckedChanged


        //=================================================================================
        // ke9ns add  open MP3 folder
        private void checkBoxTS1_CheckedChanged(object sender, EventArgs e)
        {
            if ((chkQuickAudioFolder.Checked == true)&& (chkBoxMP3.Checked == true))
            {

                string filePath = console.AppDataPath + "QuickAudioMP3";

                Debug.WriteLine("mp3 path: " + filePath);

                if (!Directory.Exists(filePath))
                {
                  
                    Debug.WriteLine("no mp3 folder found");
                    return;

                }

                string argument = @filePath;                     //@"/select, " + filePath;

                System.Diagnostics.Process.Start("explorer.exe", argument);
                

            } // 


        } // checkBoxTS1_CheckedChanged

        //==================================================================================
        // ke9ns add REPLY
        private void checkBoxTS2_CheckedChanged(object sender, EventArgs e)
        {
            chkQuickAudioFolder.Checked = true;
            console.checkBoxID.Checked = true;

            if (console.ckQuickRec.Checked == true) // recorder already running
            {
                console.ckQuickRec.Checked = false;

              //  string file_name = console.AppDataPath + "QuickAudio" + "\\SDRQuickAudio" + QAC.ToString() + ".wav";
                string file_name1 = console.AppDataPath + "QuickAudio" + "\\CALL.wav";

                string[] files = Directory.GetFiles(console.AppDataPath + "QuickAudio" + "\\", "SDRQuickAudio" + QAC.ToString() + "*.wav"); // 

                MessageBox.Show("Done Recording.\nCALL.wav file will be created.");

                System.IO.File.Copy(files[0], file_name1, true);

            }
            else
            {
                console.ckQuickRec.Checked = true; // start recording
            }

        } // checkBoxTS2_CheckedChanged

        private void chkAlwaysOnTop_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = chkAlwaysOnTop.Checked;
        }




        //================================================================================================================
        //================================================================================================================
        //================================================================================================================
        //================================================================================================================



        //
        //k6jca  End Quick Record & Play
        //
        ////////////////////////



        //===================================================================
        // ke9ns add
        public string QAName()
        {
            string temp = "__" + console.RX1DSPMode.ToString() + "_";   // DSP mode
            temp += console.VFOAFreq.ToString("f6") + "MHz_";    // Freq
            temp += DateTime.Now.ToString();                     // Date and time
            temp = temp.Replace("/", "-");
            temp = temp.Replace(":", "_");

            return temp;


        } // QAName()

    } // Class wavecontrol





    #region Wave File Header Helper Classes

    public class Chunk
	{
		public int chunk_id;

		public static Chunk ReadChunk(ref BinaryReader reader)
		{
			int data = reader.ReadInt32();
			if(data == 0x46464952)	// RIFF chunk
			{
				RIFFChunk riff = new RIFFChunk();
				riff.chunk_id = data;
				riff.file_size = reader.ReadInt32();
				riff.riff_type = reader.ReadInt32();
				return riff;
			}
			else if(data == 0x20746D66)	// fmt chunk
			{
				fmtChunk fmt = new fmtChunk();
				fmt.chunk_id = data;
				fmt.chunk_size = reader.ReadInt32();
				fmt.format = reader.ReadInt16();
				fmt.channels = reader.ReadInt16();
				fmt.sample_rate = reader.ReadInt32();
				fmt.bytes_per_sec = reader.ReadInt32();
				fmt.block_align = reader.ReadInt16();
				fmt.bits_per_sample = reader.ReadInt16();
				return fmt;
			}
			else if(data == 0x61746164) // data chunk
			{
				dataChunk data_chunk = new dataChunk();
				data_chunk.chunk_id = data;
				data_chunk.chunk_size = reader.ReadInt32();
				return data_chunk;
			}
			else
			{
				Chunk c = new Chunk();
				c.chunk_id = data;
				return c;
			}
		}
	}

	public class RIFFChunk : Chunk
	{
		public int file_size;
		public int riff_type;
	}

	public class fmtChunk : Chunk
	{
		public int chunk_size;
		public short format;
		public short channels;
		public int sample_rate;
		public int bytes_per_sec;
		public short block_align;
		public short bits_per_sample;
	}

	public class dataChunk : Chunk
	{
		public int chunk_size;
		public int[] data;
	}

#endregion

#region WaveFile Class

	public class WaveFile
	{
#region Variable Declaration

		private string filename;
		private int format;
		private int sample_rate;
		private int channels;
		private TimeSpan length;
		private bool valid = false;

#endregion

#region Constructor

		public WaveFile(string file)
		{
			RIFFChunk riff = null;
			fmtChunk fmt = null;
			dataChunk data_chunk  = null;

			filename = file;			
			if(!File.Exists(filename))
			{
				valid = false;
				return;
			}

			BinaryReader reader = null;
			try
			{
				reader = new BinaryReader(File.Open(filename, FileMode.Open, FileAccess.Read));
			}
			catch(Exception)
			{
				MessageBox.Show("File is already open.");
				valid = false;
				return;
			}

			while((data_chunk == null ||
				riff == null || fmt == null) &&
				reader.PeekChar() != -1)
			{
				Chunk chunk = Chunk.ReadChunk(ref reader);
				if(chunk.GetType() == typeof(RIFFChunk))
					riff = (RIFFChunk)chunk;
				else if(chunk.GetType() == typeof(fmtChunk))
					fmt = (fmtChunk)chunk;
				else if(chunk.GetType() == typeof(dataChunk))
					data_chunk = (dataChunk)chunk;
			}

			reader.Close();

			format = fmt.format;
			sample_rate = fmt.sample_rate;
			channels = fmt.channels;

			if(fmt.bytes_per_sec == 0)
			{
				valid = false;
				return;
			}

			length = new TimeSpan(0, 0, data_chunk.data.Length / fmt.bytes_per_sec);

			valid = true;
		}

#endregion

#region Properties

		public int Format
		{
			get { return format; }
		}

		public int SampleRate
		{
			get { return sample_rate; }
		}

		public int Channels
		{
			get { return channels; }
		}

		public TimeSpan Length
		{
			get { return length; }
		}

		public bool Valid
		{
			get { return valid; }
		}

#endregion

#region Misc Routines

		public new string ToString()
		{
			string s = filename.PadRight(20, ' ');
			s += length.Hours.ToString("10") + ":" +
				length.Minutes.ToString("nn") + ":" +
				length.Seconds.ToString("nn");
			return s;
		}

#endregion
	}

#endregion

#region Playlist

	public class Playlist
	{
		//ArrayList wave_files;
		
		public void Add(WaveFile w)
		{
			//wave_files.Add(w);
		}

		public void Remove(int i)
		{
			//wave_files.RemoveAt(i);
		}
	}

#endregion

#region Wave File Writer Class

    //=====================================================================================================
    //=====================================================================================================
    // ke9ns  create a wave file from receive audio on the flex
    //=====================================================================================================
    //=====================================================================================================

    unsafe public class WaveFileWriter
	{
		private BinaryWriter writer;
		private bool record;
		private int frames_per_buffer;
		private short channels;
		private int sample_rate;
		private int length_counter;
		private RingBufferFloat rb_l;
		private RingBufferFloat rb_r;
		private float[] in_buf_l;
		private float[] in_buf_r;
		private float[] out_buf_l;
		private float[] out_buf_r;
		private float[] out_buf;
		private byte[] byte_buf;
		private const int IN_BLOCK = 2048;
		private string filename;

		unsafe private void* resamp_l, resamp_r;

		public WaveFileWriter(int frames, short chan, int samp_rate, string file)
		{
			frames_per_buffer = frames;
			channels = chan;
			sample_rate = samp_rate;

			int OUT_BLOCK = (int)Math.Ceiling(IN_BLOCK*(double)sample_rate/Audio.SampleRate1);

            rb_l = new RingBufferFloat(IN_BLOCK*16);
			rb_r = new RingBufferFloat(IN_BLOCK*16);
			in_buf_l = new float[IN_BLOCK];
			in_buf_r = new float[IN_BLOCK];
			out_buf_l = new float[OUT_BLOCK];
			out_buf_r = new float[OUT_BLOCK];
			out_buf = new float[OUT_BLOCK*2];
			byte_buf = new byte[OUT_BLOCK*2*4];
			
			length_counter = 0;
			record = true;

			if(sample_rate != Audio.SampleRate1)
			{
               resamp_l = DttSP.NewResamplerF(Audio.SampleRate1, sample_rate);
				if(channels > 1) resamp_r = DttSP.NewResamplerF(Audio.SampleRate1, sample_rate);
			}

            try
            {
                writer = new BinaryWriter(File.Open(file, FileMode.Create));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

			filename = file;

			Thread t = new Thread(new ThreadStart(ProcessRecordBuffers));
			t.Name = "Wave File Write Thread";
			t.IsBackground = true;
			t.Priority = ThreadPriority.Normal;
			t.Start();

		} // wavefilewriter


		private void ProcessRecordBuffers()
		{
			WriteWaveHeader(ref writer, channels, sample_rate, 32, 0);
 
			while(record == true || rb_l.ReadSpace() > 0)
			{
				while(rb_l.ReadSpace() > IN_BLOCK || 
					(record == false && rb_l.ReadSpace() > 0))
				{
					WriteBuffer(ref writer, ref length_counter);
				}
				Thread.Sleep(3);
			}

			writer.Seek(0, SeekOrigin.Begin);
			WriteWaveHeader(ref writer, channels, sample_rate, 32, length_counter);
			writer.Flush();
			writer.Close();
		}

		unsafe public void AddWriteBuffer(float *left, float *right)
		{
			rb_l.WritePtr(left, frames_per_buffer);
			rb_r.WritePtr(right, frames_per_buffer);
			//Debug.WriteLine("ReadSpace: "+rb.ReadSpace());
		}

		public string Stop()
		{
			record = false;
			return filename;
		}

		private void WriteBuffer(ref BinaryWriter writer, ref int count)
		{
			int cnt = rb_l.Read(in_buf_l, IN_BLOCK);
			rb_r.Read(in_buf_r, IN_BLOCK);
			int out_cnt = IN_BLOCK;

			// resample
			if(sample_rate != Audio.SampleRate1)
			{
				fixed(float* in_ptr = &in_buf_l[0])
					fixed(float* out_ptr = &out_buf_l[0])
						DttSP.DoResamplerF(in_ptr, out_ptr, cnt, &out_cnt, resamp_l);
				if(channels > 1)
				{
					fixed(float* in_ptr = &in_buf_r[0])
						fixed(float* out_ptr = &out_buf_r[0])
							DttSP.DoResamplerF(in_ptr, out_ptr, cnt, &out_cnt, resamp_r);
				}
			}
			else
			{
				in_buf_l.CopyTo(out_buf_l, 0);
				in_buf_r.CopyTo(out_buf_r, 0);
			}

			if(channels > 1)
			{
				// interleave samples
				for(int i=0; i<out_cnt; i++)
				{
					out_buf[i*2] = out_buf_l[i];
					out_buf[i*2+1] = out_buf_r[i];
				}
			}
			else
			{
				out_buf_l.CopyTo(out_buf, 0);
			}

            byte[] temp = new byte[4];
			int length = out_cnt;
			if(channels > 1) length *= 2;
			for(int i=0; i<length; i++)
			{
				temp = BitConverter.GetBytes(out_buf[i]);
				for(int j=0; j<4; j++)
					byte_buf[i*4+j] = temp[j];
			}

			writer.Write(byte_buf, 0, out_cnt*2*4);
			count += out_cnt*2*4;
		}

		private void WriteWaveHeader(ref BinaryWriter writer, short channels, int sample_rate, short bit_depth, int data_length)
		{
			writer.Write(0x46464952);								// "RIFF"		-- descriptor chunk ID
			writer.Write(data_length + 36);							// size of whole file -- 1 for now
			writer.Write(0x45564157);								// "WAVE"		-- descriptor type
			writer.Write(0x20746d66);								// "fmt "		-- format chunk ID
			writer.Write((int)16);									// size of fmt chunk
			writer.Write((short)3);									// FormatTag	-- 3 for floats
			writer.Write(channels);									// wChannels
			writer.Write(sample_rate);								// dwSamplesPerSec
			writer.Write((int)(channels*sample_rate*bit_depth/8));	// dwAvgBytesPerSec
			writer.Write((short)(channels*bit_depth/8));			// wBlockAlign
			writer.Write(bit_depth);								// wBitsPerSample
			writer.Write(0x61746164);								// "data" -- data chunk ID
			writer.Write(data_length);								// chunkSize = length of data
			writer.Flush();											// write the file
		}
	}

#endregion

#region Wave File Reader Class


    //=============================================================================================
    //=============================================================================================
    // ke9ns  read in wave file data passed from openwavefile()
    //        start Thread (processbuffers) to play wave file audio while doing other things
    //=============================================================================================
    //=============================================================================================
    unsafe public class WaveFileReader1         
	{
		private WaveControl wave_form;
		private BinaryReader reader;
		private int format;
		private int sample_rate;
		private int channels;
		private bool playback;
		private int frames_per_buffer;
		private RingBufferFloat rb_l;
		private RingBufferFloat rb_r;
		private float[] buf_l_in;
		private float[] buf_r_in;
		private float[] buf_l_out;
		private float[] buf_r_out;
		private int IN_BLOCK;
		private int OUT_BLOCK;
		private byte[] io_buf;
		private int io_buf_size;
		private bool eof = false;

		unsafe private void* resamp_l, resamp_r;
        private MemoryStream ms;

        //=====================================================================================
        public WaveFileReader1(
			WaveControl form,
			int frames,
			int fmt,
			int samp_rate,
			int chan,
			ref BinaryReader binread)
		{
			wave_form = form;
			frames_per_buffer = frames;
			format = fmt;
			sample_rate = samp_rate;
			channels = chan;

            //OUT_BLOCK = 2048;
            //IN_BLOCK = (int)Math.Floor(OUT_BLOCK*(double)sample_rate/Audio.SampleRate1);
            //OUT_BLOCK = (int)Math.Ceiling(IN_BLOCK*Audio.SampleRate1/(double)sample_rate);

            IN_BLOCK = 2048;
			OUT_BLOCK = (int)Math.Ceiling(IN_BLOCK*Audio.SampleRate1/(double)sample_rate); // SampleRate1 = SR of the Flex,  Sample_Rate = the wave file SR

            rb_l = new RingBufferFloat(16*OUT_BLOCK);
			rb_r = new RingBufferFloat(16*OUT_BLOCK);

			buf_l_in = new float[IN_BLOCK];  // floating arrays
			buf_r_in = new float[IN_BLOCK];
			buf_l_out = new float[OUT_BLOCK];
			buf_r_out = new float[OUT_BLOCK];

			if(format == 1)              // Format = 1 = PCM (2byte SINGLE)
			{
				io_buf_size = IN_BLOCK*2*2;

                if (sample_rate != Audio.SampleRate1)  // ke9ns add   (this code was only in format==3, but copied it to format==1 If Flex and wave file SR dont match
                {
                    resamp_l = DttSP.NewResamplerF(sample_rate, Audio.SampleRate1);   // LEFT
                    if (channels > 1) resamp_r = DttSP.NewResamplerF(sample_rate, Audio.SampleRate1);   // RIGHT
                }

            }
			else if(format == 3)           // Format = 3 = ?? (float 4bytes, instead of SINGLE 2 bytes
			{
                io_buf_size = IN_BLOCK*4*2;

				if(sample_rate != Audio.SampleRate1)  // If Flex and wave file SR dont match
				{
                    resamp_l = DttSP.NewResamplerF(sample_rate, Audio.SampleRate1);   // LEFT
					if(channels > 1) resamp_r = DttSP.NewResamplerF(sample_rate, Audio.SampleRate1);   // RIGHT
                }
            } // format = 3

			io_buf = new byte[io_buf_size];

			playback = true;
			reader = binread;
          
            Thread t = new Thread(new ThreadStart(ProcessBuffers));  // thread to play audio "ProcessBuffers"  without halting the radio
			t.Name = "Wave File Read Thread";
			t.IsBackground = true;
			t.Priority = ThreadPriority.Normal;

			do
			{
				ReadBuffer(ref reader);
			} while(rb_l.WriteSpace() > OUT_BLOCK && !eof); // do ReadBuffer to tak eout the leadin wave whitespace before handing it over to the Processbuffer to call the Readbuffer

			t.Start();
           
        } // wavefilereader()

      



        //================================================================
        //================================================================
        // ke9ns   This is a THREAD called from wavefilereader() above
        // plays wave audio then 
        // this calls next() which signal end of wave file playback
        //================================================================
        //================================================================
        private void ProcessBuffers()
		{
            
            while (playback == true)
			{				
				while (rb_l.WriteSpace() >= OUT_BLOCK && !eof)   // check for end of wave file
				{
					//Debug.WriteLine("loop 2");

					ReadBuffer(ref reader);                    // ke9ns play audio here ?

					if(playback == false)	goto end;
				}

				if(playback == false)
					goto end;

				Thread.Sleep(10);				
			}

		end:  // end of playback of wave file

          
            reader.Close(); // close the  wave file now

            Thread.Sleep(50);

			wave_form.Next(); // tall everyone your done

		} // processbuffers


        //==========================================================================
        //==========================================================================
        // ke9ns    called from Processbuffers THREAD above 
        //          this 
        //==========================================================================
        //==========================================================================
        private void ReadBuffer(ref BinaryReader reader)
		{

            //	Debug.WriteLine("ReadBuffer ("+rb_l.ReadSpace()+")");

            int i=0, num_reads = IN_BLOCK;
			int val = reader.Read(io_buf, 0, io_buf_size);             // reader.  this is the open wave file binearystream

			if(val < io_buf_size)
			{
				eof = true;
				switch(format)
				{
					case 1:		// ints
						num_reads = val / 4;
						break;
					case 3:		// floats
                        num_reads = val / 8;
						break;
				}
			} // if

			for(; i < num_reads; i++)
			{
				switch(format)
				{
					case 1:		// FORMAT 1 = PCM = ints
						buf_l_in[i] = (float)((double)BitConverter.ToInt16(io_buf, i * 4) / 32767.0);          // LEFT wave files are 2bytes (16bit) data saved as +/- 0 to 32767
                        if (channels > 1) buf_r_in[i] = (float)((double)BitConverter.ToInt16(io_buf, i * 4 + 2) / 32767.0);      // RIGHT buf_r_in[i] is now ke9ns MOD no channels if 
                       


                        break;
					case 3:		// FORMAT 3 = ???  floats
						buf_l_in[i] = BitConverter.ToSingle(io_buf, i*8);  // LEFT  wave files are 4 bytes (float) each data point
                        if (channels > 1) buf_r_in[i] = BitConverter.ToSingle(io_buf, i * 8 + 4);  // RIGHT  ke9ns MOD no channels if
                      
                        break;
				}
			} // for

			if(num_reads < IN_BLOCK)  // BAD clear out buffer and STOP
			{
                for (int j = i; j < IN_BLOCK; j++)
                {
                    buf_l_in[j] = buf_r_in[j] = 0.0f;
                }
                playback = false;
				reader.Close();                                  // wave file is now closed
			}

			int out_cnt = IN_BLOCK;

			if(sample_rate != Audio.SampleRate1)
			{
                fixed (float* in_ptr = &buf_l_in[0])     // PLAY LEFT
                {
                    fixed (float* out_ptr = &buf_l_out[0])
                    {
                         DttSP.DoResamplerF(in_ptr, out_ptr, IN_BLOCK, &out_cnt, resamp_l);
                    }
                }

                if (channels > 1)  // play RIGHT
				{
					fixed(float* in_ptr = &buf_r_in[0])
                    {
                        fixed (float* out_ptr = &buf_r_out[0])
                        {
                            DttSP.DoResamplerF(in_ptr, out_ptr, IN_BLOCK, &out_cnt, resamp_r);
                        }
                    }
				}
                
               
            }
			else
			{
				buf_l_in.CopyTo(buf_l_out, 0); // copy to buffer

                if (channels > 1)
                {
                    buf_r_in.CopyTo(buf_r_out, 0); // ke9ns mod (no if channels)
                   // Debug.WriteLine("channels2==================");
                }
            }


            //-----------------------------------------------------------
			rb_l.Write(buf_l_out, out_cnt);    // play left channel
          
            if (channels > 1)
            {
                rb_r.Write(buf_r_out, out_cnt);  // play right channel
            }
            else
            {
                rb_r.Write(buf_l_out, out_cnt); // if only left channel, simply repeat right
            }

          //  Debug.WriteLine("readbuffer==================");


        }  // readbuffer




//=====================================================================================================
		unsafe public void GetPlayBuffer(float *left, float *right)
		{

           //  Debug.WriteLine("GetPlayBuffer ("+rb_l.ReadSpace()+")");
            int count = rb_l.ReadSpace();

			if(count == 0) return;

			if(count > frames_per_buffer)	count = frames_per_buffer;

			rb_l.ReadPtr(left, count); // LEFT

			rb_r.ReadPtr(right, count);  // RIGHT

			if(count < frames_per_buffer)  // BAD
			{
                for (int i = count; i < frames_per_buffer; i++)
                {
                    left[i] = right[i] = 0.0f;
                }
			}

		}

		// FIXME: implement interleaved version of Get Play Buffer
		
//======================================================================
		public void Stop()
		{
			playback = false;
		}



	} // class wavefilereader


#endregion
}
