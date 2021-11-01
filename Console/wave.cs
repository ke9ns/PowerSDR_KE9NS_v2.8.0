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

using NAudio.Lame;
//reference Nuget Package NAudio.Lame
using NAudio.Wave;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Security.Permissions;
using System.Threading;
using System.Windows.Forms;

namespace PowerSDR
{
    sealed public partial class WaveControl : System.Windows.Forms.Form
    {
        #region Variable Declaration

        private static Bitmap ke9ns_bmp;                    // ke9ns add call sign waterfall tx id

        private Console console;

        private WaveOptions WaveOptions;

        private ArrayList file_list;
        private string wave_folder = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) + "\\PowerSDR";

      
        #endregion
      

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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #endregion

      

        #region Properties

        private int currently_playing;
        private int CurrentlyPlaying
        {
            get { return currently_playing; }
            set
            {
                if (value > lstPlaylist.Items.Count - 1)
                    value = lstPlaylist.Items.Count - 1;

                currently_playing = value;
                if (currently_playing == 0)
                    btnPrevious.Enabled = false;
                else
                    btnPrevious.Enabled = true;

                if (currently_playing == lstPlaylist.Items.Count - 1)
                {
                    if (!checkBoxLoop.Checked)
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
        public bool RECPLAY
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

        private static void ConvertMp3ToWav(string _inPath_, string _outPath_)
        {
            using (Mp3FileReader mp3 = new Mp3FileReader(_inPath_))
            {
                using (WaveStream pcm = WaveFormatConversionStream.CreatePcmStream(mp3))
                {
                    //   WaveFileWriter.CreateWaveFile(_outPath_, pcm);


                }
            }
        }

        #region Misc Routines
        //=============================================================================================================
        //=============================================================================================================
        //ke9ns   Check out wave file to see if it is correct for playing
        //=============================================================================================================
        //=============================================================================================================
        public bool OpenWaveFile(string filename, bool rx2)
        {
            RIFFChunk riff = null;
            fmtChunk fmt = null;
            dataChunk data_chunk = null;

            if (!File.Exists(filename))                                      // ke9ns check if file name works
            {
                if (chkQuickAudioFolder.Checked == true)
                {
                    MessageBox.Show(new Form { TopMost = true }, "QuickAudio Save Folder Filename doesn't exist. (" + filename + ")\n" +
                    "Close this message, and Right Click on the Play button, then select a file and Click Open,\n" +
                    "You most likely renamed a Quickaudio file which caused this. ", "Bad Filename",
                      MessageBoxButtons.OK,
                      MessageBoxIcon.Error);


                }
                else
                {
                    MessageBox.Show(new Form { TopMost = true }, "Filename doesn't exist. (" + filename + ")",
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
            catch (Exception)
            {
                MessageBox.Show(new Form { TopMost = true }, "File is already open.");
                return false;
            }

            if (reader.BaseStream.Length == 0 || reader.PeekChar() != 'R')  // ke9ns see if a RIFF type wave file
            {
                reader.Close();
                MessageBox.Show(new Form { TopMost = true }, "File is not in the correct format.",
                    "Wrong File Format",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                file_list.RemoveAt(currently_playing);
                return false;
            }

            while ((data_chunk == null || riff == null || fmt == null) && (reader.BaseStream.Position < reader.BaseStream.Length))
            {
                Chunk chunk = Chunk.ReadChunk(ref reader);
                if (chunk.GetType() == typeof(RIFFChunk))
                    riff = (RIFFChunk)chunk;
                else if (chunk.GetType() == typeof(fmtChunk))
                    fmt = (fmtChunk)chunk;
                else if (chunk.GetType() == typeof(dataChunk))
                    data_chunk = (dataChunk)chunk;
            }

            if (reader.BaseStream.Position == reader.BaseStream.Length)
            {
                reader.Close();
                MessageBox.Show(new Form { TopMost = true }, "File is not in the correct format.",
                    "Wrong File Format",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                file_list.RemoveAt(currently_playing);
                return false;
            }

            if (riff.riff_type != 0x45564157)
            {
                reader.Close();
                MessageBox.Show(new Form { TopMost = true }, "File is not an RIFF Wave file.",
                    "Wrong file format",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                file_list.RemoveAt(currently_playing);
                return false;
            }


            if (!TXIDBoxTS.Checked) // ke9ns add: dont check if doing a TX ID 
            {                                                                  // format =1 means PCM
                if (!CheckSampleRate(fmt.sample_rate) || (fmt.format == 1 && fmt.sample_rate != Audio.SampleRate1))  // ke9ns: needs to be on the list of sample rates BUT needs to match the current SR otherwise it fails
                {
                    reader.Close();
                    MessageBox.Show(new Form { TopMost = true }, "File has the wrong sample rate.> " + fmt.sample_rate + ", " + Audio.SampleRate1,
                        "Wrong Sample Rate",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    file_list.RemoveAt(currently_playing);
                    return false;
                }

                if (fmt.channels != 2)                                    // ke9ns must be stereo 
                {
                    reader.Close();
                    MessageBox.Show(new Form { TopMost = true }, "Wave File is not stereo.",
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
                   (int)fmt.format, // use floating point
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
            switch (rate)
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
            foreach (string s in file_list)
            {
                int i = s.LastIndexOf("\\") + 1;
                string file = s.Substring(i, s.IndexOf(".wav") - i);
                lstPlaylist.Items.Add(file);
            }

            if (index < 0 && lstPlaylist.Items.Count > 0)
                lstPlaylist.SelectedIndex = 0;
            else if (lstPlaylist.Items.Count > index)
                lstPlaylist.SelectedIndex = index;
            lstPlaylist.EndUpdate();

            if (lstPlaylist.Items.Count > 0)
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

            if (lstPlaylist.Items.Count > 1)
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
            if (checkBoxPlay.Checked)
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
            if (chkQuickPlay.Checked) // ke9ns this is false if you ended the PLAY by releasing the MOX yourself
            {
                chkQuickPlay.Checked = false;    // ke9ns only comes here is PLAY finishes on its own

                //   Debug.WriteLine("here DONE WITH PLAY0");
            }

            bool temp = console.chkSR.Checked;

            console.chkSR.Checked = !console.chkSR.Checked; // ke9ns add

            do
            {
                // wait
                Thread.Sleep(100);

            } while (temp == console.chkSR.Checked);


            //=========================================================================
            //ke9ns add comes here when playback has ended
            if (TXIDBoxTS.Checked)
            {
                TXIDBoxTS.Checked = false;  // tell Waterfall ID routine to turn off since the wave file is done
            }

            console.chkSR.Checked = !console.chkSR.Checked; // ke9ns add toggles SR to force the RX audio to work correctly 

            do
            {
                // wait
                Thread.Sleep(100);

            } while (temp != console.chkSR.Checked);


        } // next()

        #endregion

        #region Event Handlers

        private void openFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            foreach (string s in openFileDialog1.FileNames)
            {
                if (!file_list.Contains(s))
                    file_list.Add(s);
            }

            UpdatePlaylist();
        }

        //=========================================================================================
        // ke9ns PLAY button (for playback of standard recordings or IQ recordings

        private void checkBoxPlay_CheckedChanged(object sender, System.EventArgs e)
        {
            if (checkBoxPlay.Checked)
            {
                string filename = (string)file_list[currently_playing];

                if (!OpenWaveFile(filename, false))
                {
                    checkBoxPlay.Checked = false;
                    currently_playing = -1;
                    UpdatePlaylist();
                    return;
                }

                if ((console.CurrentModel == Model.FLEX5000) && (FWCEEPROM.RX2OK) && (console.RX2Enabled))
                {
                    string filename2 = filename + "-rx2";
                    if (File.Exists(filename2)) OpenWaveFile(filename2, true);
                }

                txtCurrentFile.Text = (string)lstPlaylist.Items[currently_playing];
                checkBoxPlay.BackColor = console.ButtonSelectedColor;
                checkBoxPause.Enabled = true;
            }
            else // if not playing audio
            {
                if (Audio.wave_file_reader != null) Audio.wave_file_reader.Stop();

                if (Audio.wave_file_reader2 != null) Audio.wave_file_reader2.Stop();

                Thread.Sleep(50); // wait for files to close

                if (checkBoxPause.Checked) checkBoxPause.Checked = false;
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
            if (checkBoxRecord.Checked)
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

                string temp = console.RX1DSPMode.ToString() + " ";
                temp += console.VFOAFreq.ToString("f6") + "MHz [";
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
                //MessageBox.Show(new Form { TopMost = true }, "The file has been written to the following location:\n"+file_name);
            }
        } // checkBoxRecord_CheckedChanged(

        private void btnAdd_Click(object sender, System.EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void btnRemove_Click(object sender, System.EventArgs e)
        {
            if (lstPlaylist.Items.Count == 0 ||
                lstPlaylist.SelectedIndices.Count == 0) return;

            ArrayList selections = new ArrayList();

            foreach (int i in lstPlaylist.SelectedIndices)
            {
                if (i == currently_playing && checkBoxPlay.Checked)
                {
                    Application.DoEvents();
                    DialogResult dr = MessageBox.Show(
                        (string)lstPlaylist.Items[i] +
                        " is currently playing.\n" +
                        "Stop playing and remove from Playlist?",
                        "Stop and Remove?",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (dr == DialogResult.Yes)
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
            for (int i = selections.Count - 1; i >= 0; i--)
                file_list.RemoveAt((int)selections[i]);
            UpdatePlaylist();
        }

        private void checkBoxLoop_CheckedChanged(object sender, System.EventArgs e)
        {
            if (checkBoxLoop.Checked)
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
            if (checkBoxRandom.Checked)
                checkBoxRandom.BackColor = console.ButtonSelectedColor;
            else
                checkBoxRandom.BackColor = SystemColors.Control;
        }

        private void lstPlaylist_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (lstPlaylist.SelectedIndex < 0)
            {
                btnPrevious.Enabled = false;
                btnNext.Enabled = false;
                return;
            }

            if (!checkBoxPlay.Checked)
            {
                CurrentlyPlaying = lstPlaylist.SelectedIndex;
            }
        }

        private void btnPrevious_Click(object sender, System.EventArgs e)
        {
            if (checkBoxPlay.Checked)
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
            if (checkBoxPlay.Checked)
            {
                checkBoxPlay.Checked = false;
                if (CurrentlyPlaying == lstPlaylist.Items.Count - 1)
                {
                    CurrentlyPlaying = 0;
                }
                else CurrentlyPlaying++;
                checkBoxPlay.Checked = true;
            }
            else
            {
                int temp = lstPlaylist.SelectedIndex + 1;
                if (temp == lstPlaylist.Items.Count) temp = 0;
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
            if (checkBoxPlay.Checked)
                Audio.wave_playback = !checkBoxPause.Checked;

            if (checkBoxPause.Checked)
                checkBoxPause.BackColor = console.ButtonSelectedColor;
            else
                checkBoxPause.BackColor = SystemColors.Control;
        }

        private void lstPlaylist_DoubleClick(object sender, System.EventArgs e)
        {
            if (checkBoxPlay.Checked)
            {
                CurrentlyPlaying = lstPlaylist.SelectedIndex;
                checkBoxPlay.Checked = false;
                checkBoxPlay.Checked = true;
            }
            else checkBoxPlay.Checked = true;
        }

        private void mnuWaveOptions_Click(object sender, System.EventArgs e)
        {
            if (WaveOptions == null || WaveOptions.IsDisposed)
                WaveOptions = new WaveOptions();

            WaveOptions.Show();
            WaveOptions.Focus();
        }

        public decimal WavPreamp = 0;
        private void udPreamp_ValueChanged(object sender, System.EventArgs e)
        {
            tbPreamp.Value = (int)udPreamp.Value;

            WavPreamp = udPreamp.Value; // ke9ns add
            Audio.WavePreamp = Math.Pow(10.0, (int)udPreamp.Value / 20.0); // convert to scalar
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
        private bool temp_dexp = false; // dexp

        public static int QAC = 0; // ke9ns add

        public decimal preampvalue = 0; // ke9na add
        //==================================================================================
        // ke9ns  mod needed since MON now toggle pre and post audio. quickplay should always be post 
        private void chkQuickPlay_CheckedChanged(object sender, System.EventArgs e)
        {
            Debug.WriteLine("WAVE: chkQuickPlay checkchanged");
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
                                              //  console.checkBoxID.Checked = false;
                    file_name = console.AppDataPath + "QuickAudio" + "\\CALL.wav";            // ke9ns 
                }
                else if (console.VK1CALL == true) // ke9ns add
                {
                    console.VK1CALL = false; // reset ID 
                                              //  console.checkBoxID.Checked = false;
                    file_name = console.AppDataPath + "QuickAudio" + "\\VK1.wav";            // ke9ns 
                }
                else if (console.VK2CALL == true) // ke9ns add
                {
                    console.VK2CALL = false; // reset ID 
                                              //  console.checkBoxID.Checked = false;
                    file_name = console.AppDataPath + "QuickAudio" + "\\VK2.wav";            // ke9ns 
                }
                else if (console.QuindarStart == true) // ke9ns add
                {
                   
                    file_name = console.AppDataPath + "QuickAudio" + "\\Quindar_tone_start.wav";            // ke9ns 
                }
                else if (console.QuindarEnd == true) // ke9ns add
                {
                    
                     file_name = console.AppDataPath + "QuickAudio" + "\\Quindar_tone_end.wav";            // ke9ns 
                  
                }



                else if (QPFile != null)
                {
                    file_name = QPFile; // ke9ns check file name passed from console play button

                }
                else if ((chkQuickPlay.Checked) && QAC > 0)
                {

                    // ke9ns add to keep a missing file name from crashing powersdr
                    try
                    {
                        //  file_name = console.AppDataPath + "QuickAudio" + "\\SDRQuickAudio" + QAC.ToString() + QAName() + ".wav";
                        string[] files = Directory.GetFiles(console.AppDataPath + "QuickAudio" + "\\", "SDRQuickAudio" + QAC.ToString() + "*.wav"); // ke9ns ignore extra part of name
                        file_name = files[0];
                    }
                    catch (Exception)
                    {
                        console.ckQuickPlay.Checked = false; // if not transmitting then dont do anything and return.

                        if (!Directory.Exists(console.AppDataPath)) // need to see the quickaudio folder
                        {
                            MessageBox.Show(new Form() { TopMost = true }, "WAVE: could not Find QuickAudio folder. " + console.AppDataPath);
                            if (console.QuindarEnd == true || console.QuindarStart == true)
                            {
                                console.WaveForm.udPreamp.Value = console.WaveForm.WavPreamp;
                                console.QuindarStart = false;
                                console.QuindarEnd = false;
                            }
                            return;
                        }
                        else
                        {
                            //   MessageBox.Show(new Form() { TopMost = true }, "WAVE: could not Find file " + QAC.ToString() + " in QuickAudio folder");
                            MessageBox.Show(new Form() { TopMost = true }, "WAVE: could not Find file " + QAC.ToString() + " in QuickAudio folder");
                            if (console.QuindarEnd == true || console.QuindarStart == true)
                            {
                                console.WaveForm.udPreamp.Value = console.WaveForm.WavPreamp;
                                console.QuindarStart = false;
                                console.QuindarEnd = false;
                            }
                            return;
                        }
                    }

                } // else 
                else
                {
                    file_name = console.AppDataPath + "SDRQuickAudio.wav";
                    console.ckQuickPlay.Checked = false; // if not transmitting then dont do anything and return.
                }

            } // quickqudiofolder enabled
            else
            {
                file_name = console.AppDataPath + "SDRQuickAudio.wav";
            }

            //--------------------------------------------------------------------

            if (chkQuickPlay.Checked == true)
            {
                if (Directory.Exists(console.AppDataPath)) // need to see the quickaudio folder
                {
                    if (File.Exists(file_name))
                    {
                        Debug.WriteLine("found file name " + file_name);
                    }
                    else
                    {
                        console.ckQuickPlay.Checked = false; // if not transmitting then dont do anything and return.
                        MessageBox.Show(new Form() { TopMost = true }, "Wave: Could not Find file: " + file_name + " in QuickAudio folder");
                        if (console.QuindarEnd == true || console.QuindarStart == true)
                        {
                            console.WaveForm.udPreamp.Value = console.WaveForm.WavPreamp;
                            console.QuindarStart = false;
                            console.QuindarEnd = false;
                        }

                        return;
                    }
                }
                else
                {
                    console.ckQuickPlay.Checked = false; // if not transmitting then dont do anything and return.
                    MessageBox.Show(new Form() { TopMost = true }, "Wave: could not Find QuickAudio folder.. " + console.AppDataPath);

                    if (console.QuindarEnd == true || console.QuindarStart == true)
                    {
                        console.WaveForm.udPreamp.Value = console.WaveForm.WavPreamp;
                        console.QuindarStart = false;
                        console.QuindarEnd = false;
                    }
                    return;
                }
            }

            //----------------------------------------------------------------------------------
            if (chkQuickPlay.Checked) // do at the start of playing audio file
            {

                temp_txeq = console.TXEQ;
                console.TXEQ = false;               // set TX Eq temporarily to OFF

                temp_cpdr = console.CPDR;
                console.CPDR = false;

                temp_dx = console.DX;
                console.DX = false;

                temp_dexp = console.DEXP; // ke9ns add: disable DEXP during quickplay.
                console.DEXP = false;

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

                    if (console.QuindarEnd == true || console.QuindarStart == true)
                    {
                        console.WaveForm.udPreamp.Value = preampvalue;
                        console.QuindarStart = false;
                        console.QuindarEnd = false;
                    }

                    return;
                }
                /*if (console.CurrentModel == Model.FLEX5000 && FWCEEPROM.RX2OK && console.RX2Enabled)
               {
                   string file_name2 = file_name+"-rx2";
                   OpenWaveFile(file_name2, true);
               }*/

                chkQuickPlay.BackColor = console.ButtonSelectedColor;

            } // quickplay checked and playing audio
            else // quickplay DONE, over, finished
            {

                if (Audio.wave_file_reader != null)
                {
                    //quindar delay
                    Audio.wave_file_reader.Stop();
                }

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
                console.DEXP = temp_dexp; // ke9ns add
                console.checkBoxID.Checked = false; // ke9ns add

                //  console.buttonCQ.BackColor = Color.Blue; // ke9ns add
                //   console.buttonCall.BackColor = Color.Blue; // kens add

                console.buttonCQ1.Image = global::PowerSDR.Properties.Resources.wideblue4;
                console.buttonCall1.Image = global::PowerSDR.Properties.Resources.wideblue3;
                console.buttonVK1.Image = global::PowerSDR.Properties.Resources.VK1;
                console.buttonVK2.Image = global::PowerSDR.Properties.Resources.VK2;


            }

            Audio.wave_playback = chkQuickPlay.Checked;  // PLAY AUDIO FILE HERE
            console.WavePlayback = chkQuickPlay.Checked;
           
            console.QuindarStart = false;
            console.QuindarEnd = false;


        }// chkQuickPlay changed




        public static string quickmp3SR; // ke9ns add

        public static string quickmp3; // ke9ns add
        //============================================================================================
        private void chkQuickRec_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkQuickRec.Checked)
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

                    file_name = console.AppDataPath + "QuickAudio" + "\\SDRQuickAudio" + QAC.ToString() + QAName() + ".wav";
                    quickmp3 = console.AppDataPath + "QuickAudioMP3" + "\\SDRQuickAudio" + QAC.ToString() + QAName() + ".mp3"; // ke9ns add mp3


                    //   Debug.WriteLine("qac" + QAC);

                }
                else // ke9ns old way quickaudio
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
                    MessageBox.Show(new Form { TopMost = true }, "The 'Over the Air' Quick audio recording has been successfully created.\n" +
                        "Key the radio with either PTT or MOX and click on the Play button to play back the Quick audio recording over the air.");

                    // MessageBox.Show(new Form { TopMost = true }, "The file has been written to the following location:\n"+file_name);
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
                            reader.CopyTo(writer);  // create MP3 file
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
            set { chkQuickRec.Checked = value; }
        }

        public bool QuickPlay
        {
            get
            {
                return chkQuickPlay.Checked;
            }
            set
            {
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

                Debug.WriteLine("playing ");

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

                Debug.WriteLine("DONE playing ");

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
        // ke9ns ADD:   this routine creates the waterfall ID (txidBoxTS plays it)
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

                if ((console.Callsign != console.LastCall) || (console.RX1DSPMode != BandL) || ((console.WIDEWATERID == true) && (console.TXFilterHigh != TxfhL)))  // check if we need to create a new wave file or use the old one.
                {
                    TxfhL = console.TXFilterHigh;

                    Thread t = new Thread(new ThreadStart(CreateWaterfallID));
                    t.Name = "Create Waterfall ID wave file Thread";
                    t.IsBackground = true;
                    t.Priority = ThreadPriority.Highest;
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
        // ke9ns add:   THREAD process to create wave file from text or bitmap image here  (TXWaterID)
        //============================================================================================
        //============================================================================================
        private void CreateWaterfallID()
        {


            string file_name = console.AppDataPath + "ke9ns.wav"; // TEXT to waterfall image only   16bit 48khz SR, 768 kbps
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

            int xm = 150; // width was 100

            if (console.Callsign.EndsWith(".") == true)
            {
                IMAGE = 1; // get real image file

                file_name1 = console.AppDataPath + console.Callsign + "bmp";    //    image file to waterfall

                xm = 200; // was 100
            }

            double bright = 500;      // ke9ns volume level (was 400 amplitude factor_

            int n1 = 0;   // used by USB/LSB routine
            int n2 = 0;
            int n3 = 0;
            int n4 = 0;
            SizeF cl = new SizeF();  // determine left or right side of bitmap

            int fontS = 30; // was 34

            int ym = 40; // height was 22

            if (IMAGE == 0)
            {
                ym = 36; // was 26
            }



            long xm4 = 4 * ((xm + 3) / 4);

            const float bw = .114F;   // factors to convert RGB color to grayscale
            const float gw = .587F;
            const float rw = .2989F;

            byte[,] ap = new byte[xm + 10, ym + 10];  // get bitmap data
           

            //=========================================================================================
            //=========================================================================================
            // Choose TEXT or BITMAP   txwaterid
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
                } while (((cl.Width > 150)|| (cl.Height > 40)) && (fontS > 9));  // 100 9 ke9ns reduce size of font until the string fits into bandpass
// while ((cl.Width > 100) && (fontS > 9));

             
                Debug.WriteLine("MEASUREMENT LENGTH " + cl + " , " + fontS + " , " + xm + " , " + ym + " , " + cl.Height + " , " + fontS);
                    
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
                g1.DrawString(temp1A, new Font("Arial", fontS, FontStyle.Regular), Brushes.White, cl.Width, cl.Height); // determine USB or LSB, then draw callsign into bitmap

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
                    MessageBox.Show(new Form { TopMost = true }, "Filename doesn't exist. (" + file_name1 + ")",
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

            const int RIFF = 0x46464952;                      // 0x46464952  (0x52494646 big-endian form). spell out "RIFF"
            // filesize chunksize
            const int WAVE = 0x45564157;                      // 0x45564157  (0x57415645 big-endian form). spellout "WAVE"
            const int format = 0x20746D66;                    // 0x20746D66   0x666d7420 spell out "fmt "
            const int formatChunkSize = 16;                   // 16 bytes to follow
            const short formatType = 1;                       // 1 PCM
            const short tracks = 1;                           // 1 Stereo = 2
            const int samplesPerSecond = 48000;               // console.SampleRate1;    (fixed flex routine to resample audio to correct SR)
            int bytesPerSecond = 48000;                       //  byterate = SampleRate * NumChannels * BitsPerSample/8
            short frameSize = 2;                              // blockalign = NumChannels * BitsPerSample/8   
            const short bitsPerSample = 16;                   // 16
            int data = 0x61746164;                            // (0x64617461 big-endian form). Spell out "data"
         
            
            const int headerSize = 8;                         // 8 bytes (
        
            int waveSize = ym; // how tall is the images

            const int lowtx = 150; // lowest frequency to display

            int bandpass = 2400; // typical width in freq

            if (console.WIDEWATERID == true)
            {
                if (console.TXFilterHigh > 2400) bandpass = (console.TXFilterHigh - 150);
                else bandpass = 2400;
            }
            else
            {
                bandpass = 2400;
            }

            int t2 = 2;     // was 3               // was 2 .178                                 // divide the time for each line (this controls how long each line takes)

            if (IMAGE == 1)
            {
                float t3 = ((float)xm / (float)ym);         // picture ratio to maintain in waterfall (80w x 80h = 1) (200w x 150h = 1.33) (80w x 20h = 4)
                                                            // float t4 = ((float)bandpass / (float)xm);  // number of freqs in width of images (200w = 13hz per pixel)longer time .076pix/hz,  (80w= 30hz per pixel)shorter time.033pix/hz
                t2 = (int)((float)xm * 2 / 20 / (t3 + 1));        // t2 small means long vertical , t2 big means short (20 is max)

                if (t2 > 20) t2 = 20;
            }

      
            int samples = (samplesPerSecond * 2) * waveSize / t2;            // file size 48000*2 * 40 / 2 = 1920000              //88200 * 4 = 352800

          int dataChunkSize = samples * frameSize / 2;                     //  = NumSamples * NumChannels * BitsPerSample/8
         //   int dataChunkSize = samples * 2;                     //  = NumSamples * NumChannels * BitsPerSample/8

            int fileSize = (waveSize) + (8 + formatChunkSize) + (8 + dataChunkSize);
         //   int fileSize = 4 + (8 + formatChunkSize) + (8 + dataChunkSize);

            // write header to file

            writer.Write(RIFF);                             // "RIFF" (actual text)
            writer.Write(fileSize);                         // overall file size - 8 bytes
            writer.Write(WAVE);                             // "WAVE" (actual text)
            writer.Write(format);                           // "fmt " (actual text)
            writer.Write(formatChunkSize);                  // 16bytes  (always)
            writer.Write(formatType);                       // 1=PCM
            writer.Write(tracks);                           // 1=channel mono
            writer.Write(samplesPerSecond);                 // 480000
            writer.Write(bytesPerSecond);                   // (48000SR * 16bits * 1channel) / 8 = 96000
            writer.Write(frameSize);                        // 2= NumChannels * BitsPerSample/8
            writer.Write(bitsPerSample);                    // 16bits
            writer.Write(data);                             // "data" (actual text)
            writer.Write(dataChunkSize);                    // file size - 44 bytes

            //==============================================================
            // used if you want the waterfall to be as wide as your transmission bandpass
            //  int lowtx = console.TXFilterLow;
            //  int hightx = console.TXFilterHigh;
            //   int tottx = hightx - lowtx; // band width

            // xm = 100 pixels wide // was 80
            int hzperpixel = (int)(((double)bandpass / (double)xm ) + 0.5);    //  2khz wide =  25hz/pix  6000 = 75 hz per pixel


            //  Debug.WriteLine("hzperpixel " + hzperpixel);

            int sample2 = (int)(samplesPerSecond / t2);      // samples2 = the amount of time spent on each y1 line 48000/2 = 24000
            int sample3 = (int)(sample2 / 2f);

            // ap[,] = vales between 0 and 1 (0=dark, 255=white)
          
            bright = bright / ((double)xm); // correct brightness level of PCM audio by how many freq points go into each pass

            //=========================================================================================
            // Create Digital PCM Stream
            // The call sign text is converted into a 100 pixel wide by 26 pixel tall internal bmp
            // Then I convert that into a data array of brightness values(100 across and 26 down).
            // I take each horizontal line, and create sin waves for 100 pixels, spanning equally across the bandpass,
            // then step and repeat for all 26 horizontal lines.With each line sampled at 48khz 16bit
            //=========================================================================================

            bool firstdata = false;  // true = value other than 0

            firstdata = false;

            int filesizewav = 0;
            int firstzero = 0;
            int lastzero = 0;
            List<short> s1 = new List<short>();   // (ym*sample2);

          

            Debug.WriteLine("PCM CREATION================");
            Debug.WriteLine("ym="+ym);
            Debug.WriteLine("xm=" + xm);
            Debug.WriteLine("sample2=" + sample2);
            Debug.WriteLine("lowtx=" + lowtx);
            Debug.WriteLine("bandpass=" + bandpass);
            Debug.WriteLine("hzpp=" + hzperpixel);
            Debug.WriteLine("sps=" + samplesPerSecond);


            Debug.WriteLine("PCM END================");
            double temp7 = 0.0;
            double temp8 = 0.0;
           
            int i = 0;
         
                for (int y1 = 0; y1 < ym; y1++)   // each scan line (bottom of bitmap is first line out) height = y1
                {
               
                    for (double n = -sample3; n < sample3 ; n++)           //  (generate tone)  sample3 = samplespersecond / 2         
                    {

                    temp7 = 0.0;

                    i = 0;

                        //===========================================================
                        // depending on width of bitmap, display within 150 hz to 2550hz
                       
                            for (double freq = (double)lowtx; freq < (double)(bandpass + lowtx); freq = freq + (double)hzperpixel, i++)             // add up all the frequencies from each line(row) of the bitmap into 1 signal
                            {

                                if (ap[i, y1] > 1)
                                {
                                    // i=xm, y1= ym
                                    // temp7 = add up all 100 sine waves in the bandpass for 1 hor line of 26 possible lines 
                                    // There are 48000 temp7 values per hor line (and temp7 16bits are captured)

                                    temp7 = temp7 + ((double)ap[i, y1] * (Math.Cos(n * freq * Math.PI / (double)samplesPerSecond)));  // generate individual tone 
                                   
                                }
                            }// freq loop

                       

                       //   if (temp7 > 0) Debug.WriteLine("ODD " +  temp7.ToString()  + " n=" +n);
                        //============================================================

                       
                        short s = (short)(temp7 * bright);

                       //  writer.Write(s);  // left 16bits

                        if ((firstdata == false))
                        {
                             if (s != 0) firstdata = true;  // ke9ns: .139 remove the starting dead space from the transmission
                            else firstzero++;           // count how many 16bit words until the first real data (non-zero)
                        }


                        s1.Add(s); // ke9ns: store wav in array so you can drop the dead space and the start and end of the file

                        if ((firstdata == true))
                        {
                           if (s == 0) lastzero++; // counts how many zeros before the end of the file
                        }
                       else
                       {
                           lastzero = 0;
                        }


                    } // n  X  1 line at a time

            } // Y

            Debug.WriteLine("WAVE=== First zero: " + firstzero + " , Last Zero: " + lastzero + " Arry size: " + s1.Count);

            filesizewav = s1.Count;

            if (IMAGE == 0)
            {
                if (lastzero > 40000) // ke9ns: .146 remove end of file
                {
                    lastzero = lastzero - 40000;
                    filesizewav = s1.Count - lastzero; // remove zeros at end of file
                }
            }

            if (firstzero > 30000) firstzero = firstzero - 30000;
            else firstzero = 0;

            Debug.WriteLine("1WAVE=== First zero: " + firstzero + " , Last Zero: " + lastzero + " Arry size: " + s1.Count);

            for (int q = firstzero; q < filesizewav; q++)
            {
                writer.Write(s1[q]);
            }

            writer.Flush();


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

            if (checkBoxVoice.Checked == true)
            {
                oldmode = console.RX1DSPMode;
                if ((oldmode == DSPMode.FM) || (oldmode == DSPMode.AM) || (oldmode == DSPMode.SAM) || (oldmode == DSPMode.DSB)) console.RX1DSPMode = DSPMode.USB; // record only in SSB mode
            }

            if (console.ckQuickRec.Checked == true) // recorder already running
            {
                console.ckQuickRec.Checked = false;
                string file_name1 = console.AppDataPath + "QuickAudio" + "\\IDTIMER.wav";

                string[] files = Directory.GetFiles(console.AppDataPath + "QuickAudio" + "\\", "SDRQuickAudio" + QAC.ToString() + "*.wav"); // ke9ns find file but ignore extra information about file

                MessageBox.Show(new Form() { TopMost = true }, "Done Recording.\nIDTIMER.wav file will be created.");

                console.RX1DSPMode = oldmode;

                if (!File.Exists((file_name1)))                                      // ke9ns check if file name works
                {
                    System.IO.File.Delete(file_name1);  // delete the IDTIMER file first
                }

                System.IO.File.Copy(files[0], file_name1, true); // copy quickaudio file to IDTIMER file
            }
            else
            {
                console.ckQuickRec.Checked = true; // start recording
            }


        } // checkBoxVoice_CheckedChanged


        //==================================================================================================================
        // ke9ns add
        private void checkBoxCW_CheckedChanged(object sender, EventArgs e)
        {
            chkQuickAudioFolder.Checked = true;
            console.checkBoxID.Checked = true;

            if (checkBoxCW.Checked == true)
            {
                oldmode = console.RX1DSPMode;
                if ((oldmode == DSPMode.FM) || (oldmode == DSPMode.AM) || (oldmode == DSPMode.SAM) || (oldmode == DSPMode.DSB)) console.RX1DSPMode = DSPMode.USB; // record only in SSB mode
            }

            if (console.ckQuickRec.Checked == true) // recorder already running
            {
                console.ckQuickRec.Checked = false;
                string file_name1 = console.AppDataPath + "QuickAudio" + "\\IDTIMERCW.wav";

                string[] files = Directory.GetFiles(console.AppDataPath + "QuickAudio" + "\\", "SDRQuickAudio" + QAC.ToString() + "*.wav"); // 

                MessageBox.Show(new Form() { TopMost = true }, "Done Recording.\nIDTIMERCW.wav file will be created.");

                console.RX1DSPMode = oldmode;

                if (!File.Exists((file_name1)))                                      // ke9ns check if file name works
                {
                    System.IO.File.Delete(file_name1);  // delete the IDTIMERCW file first
                }

                System.IO.File.Copy(files[0], file_name1, true); // copy quickaudio file to IDTIMERCW file
            }
            else
            {
                console.ckQuickRec.Checked = true; // start recording
            }


        } // checkBoxCW_CheckedChanged



        DSPMode oldmode = DSPMode.FIRST;

        //==================================================================================================================
        // ke9ns add IDVOICE
        private void checkBoxCQ_CheckedChanged(object sender, EventArgs e)
        {
            chkQuickAudioFolder.Checked = true;
            console.checkBoxID.Checked = true;  // REC/PLAY ID checkbox in console

            if (checkBoxCQ.Checked == true)
            {
                oldmode = console.RX1DSPMode;
                if ((oldmode == DSPMode.FM) || (oldmode == DSPMode.AM) || (oldmode == DSPMode.SAM) || (oldmode == DSPMode.DSB)) console.RX1DSPMode = DSPMode.USB; // record only in SSB mode
            }

            if (console.ckQuickRec.Checked == true) // recorder already running
            {
                console.ckQuickRec.Checked = false;
                string file_name1 = console.AppDataPath + "QuickAudio" + "\\CQCQ.wav";

                string[] files = Directory.GetFiles(console.AppDataPath + "QuickAudio" + "\\", "SDRQuickAudio" + QAC.ToString() + "*.wav"); // 

                MessageBox.Show(new Form() { TopMost = true }, "Done Recording.\nCQCQ.wav file will be created.");

                console.RX1DSPMode = oldmode;

                if (!File.Exists((file_name1)))                                      // ke9ns check if file name works
                {
                    System.IO.File.Delete(file_name1);  // delete the cqcq file first
                }

                System.IO.File.Copy(files[0], file_name1, true); // copy quickaudio file to cqcq file

            }
            else
            {
                console.ckQuickRec.Checked = true; // start recording
            }

        } //checkBoxCQ_CheckedChanged



        //==================================================================================
        // ke9ns add: REPLY
        private void checkBoxTS2_CheckedChanged(object sender, EventArgs e)
        {
            chkQuickAudioFolder.Checked = true;
            console.checkBoxID.Checked = true;

            if (checkBoxTS2.Checked == true)
            {
                oldmode = console.RX1DSPMode;
                if ((oldmode == DSPMode.FM) || (oldmode == DSPMode.AM) || (oldmode == DSPMode.SAM) || (oldmode == DSPMode.DSB))
                {
                    console.RX1DSPMode = DSPMode.USB; // record only in SSB mode
                }
            }

            if (console.ckQuickRec.Checked == true) // recorder already running
            {
                console.ckQuickRec.Checked = false;

                string file_name1 = console.AppDataPath + "QuickAudio" + "\\CALL.wav";

                string[] files = Directory.GetFiles(console.AppDataPath + "QuickAudio" + "\\", "SDRQuickAudio" + QAC.ToString() + "*.wav"); // 

                MessageBox.Show(new Form() { TopMost = true }, "Done Recording.\nCALL.wav file will be created.");

                console.RX1DSPMode = oldmode;

                if (!File.Exists((file_name1)))                                      // ke9ns check if file name works
                {
                    System.IO.File.Delete(file_name1);  // delete the call file first
                }

                System.IO.File.Copy(files[0], file_name1, true); // copy quickaudio file to call file
            }
            else
            {
                console.ckQuickRec.Checked = true; // start recording
            }

        } // checkBoxTS2_CheckedChanged



        //=================================================================================
        // ke9ns add  open MP3 folder
        private void checkBoxTS1_CheckedChanged(object sender, EventArgs e)
        {
            if ((chkQuickAudioFolder.Checked == true) && (chkBoxMP3.Checked == true))
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

        private void checkBoxVK1_CheckedChanged(object sender, EventArgs e)
        {
            chkQuickAudioFolder.Checked = true;
            console.checkBoxID.Checked = true;

            if (checkBoxVK1.Checked == true)
            {
                oldmode = console.RX1DSPMode;
                if ((oldmode == DSPMode.FM) || (oldmode == DSPMode.AM) || (oldmode == DSPMode.SAM) || (oldmode == DSPMode.DSB))
                {
                    console.RX1DSPMode = DSPMode.USB; // record only in SSB mode
                }
            }

            if (console.ckQuickRec.Checked == true) // recorder already running
            {
                console.ckQuickRec.Checked = false;

                string file_name1 = console.AppDataPath + "QuickAudio" + "\\VK1.wav";

                string[] files = Directory.GetFiles(console.AppDataPath + "QuickAudio" + "\\", "SDRQuickAudio" + QAC.ToString() + "*.wav"); // 

                MessageBox.Show(new Form() { TopMost = true }, "Done Recording.\nVK1.wav file will be created.");

                console.RX1DSPMode = oldmode;

                if (!File.Exists((file_name1)))                                      // ke9ns check if file name works
                {
                    System.IO.File.Delete(file_name1);  // delete the call file first
                }

                System.IO.File.Copy(files[0], file_name1, true); // copy quickaudio file to call file
            }
            else
            {
                console.ckQuickRec.Checked = true; // start recording
            }



        } // checkboxvk1

        private void checkBoxVK2_CheckedChanged(object sender, EventArgs e)
        {
            chkQuickAudioFolder.Checked = true;
            console.checkBoxID.Checked = true;

            if (checkBoxVK1.Checked == true)
            {
                oldmode = console.RX1DSPMode;
                if ((oldmode == DSPMode.FM) || (oldmode == DSPMode.AM) || (oldmode == DSPMode.SAM) || (oldmode == DSPMode.DSB))
                {
                    console.RX1DSPMode = DSPMode.USB; // record only in SSB mode
                }
            }

            if (console.ckQuickRec.Checked == true) // recorder already running
            {
                console.ckQuickRec.Checked = false;

                string file_name1 = console.AppDataPath + "QuickAudio" + "\\VK2.wav";

                string[] files = Directory.GetFiles(console.AppDataPath + "QuickAudio" + "\\", "SDRQuickAudio" + QAC.ToString() + "*.wav"); // 

                MessageBox.Show(new Form() { TopMost = true }, "Done Recording.\nVK2.wav file will be created.");

                console.RX1DSPMode = oldmode;

                if (!File.Exists((file_name1)))                                      // ke9ns check if file name works
                {
                    System.IO.File.Delete(file_name1);  // delete the call file first
                }

                System.IO.File.Copy(files[0], file_name1, true); // copy quickaudio file to call file
            }
            else
            {
                console.ckQuickRec.Checked = true; // start recording
            }

        } // checkboxvk2


    } // Class wavecontrol





    #region Wave File Header Helper Classes

    public class Chunk
    {
        public int chunk_id;

        public static Chunk ReadChunk(ref BinaryReader reader)
        {
            int data = reader.ReadInt32();
            if (data == 0x46464952) // RIFF chunk
            {
                RIFFChunk riff = new RIFFChunk();
                riff.chunk_id = data;
                riff.file_size = reader.ReadInt32();
                riff.riff_type = reader.ReadInt32();
                return riff;
            }
            else if (data == 0x20746D66)    // fmt chunk
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
            else if (data == 0x61746164) // data chunk
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
            dataChunk data_chunk = null;

            filename = file;
            if (!File.Exists(filename))
            {
                valid = false;
                return;
            }

            BinaryReader reader = null;
            try
            {
                reader = new BinaryReader(File.Open(filename, FileMode.Open, FileAccess.Read));
            }
            catch (Exception)
            {
                MessageBox.Show(new Form { TopMost = true }, "File is already open.");
                valid = false;
                return;
            }

            while ((data_chunk == null ||
                riff == null || fmt == null) &&
                reader.PeekChar() != -1)
            {
                Chunk chunk = Chunk.ReadChunk(ref reader);
                if (chunk.GetType() == typeof(RIFFChunk))
                    riff = (RIFFChunk)chunk;
                else if (chunk.GetType() == typeof(fmtChunk))
                    fmt = (fmtChunk)chunk;
                else if (chunk.GetType() == typeof(dataChunk))
                    data_chunk = (dataChunk)chunk;
            }

            reader.Close();

            format = fmt.format;
            sample_rate = fmt.sample_rate;
            channels = fmt.channels;

            if (fmt.bytes_per_sec == 0)
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

            int OUT_BLOCK = (int)Math.Ceiling(IN_BLOCK * (double)sample_rate / Audio.SampleRate1);

            rb_l = new RingBufferFloat(IN_BLOCK * 16);
            rb_r = new RingBufferFloat(IN_BLOCK * 16);
            in_buf_l = new float[IN_BLOCK];
            in_buf_r = new float[IN_BLOCK];
            out_buf_l = new float[OUT_BLOCK];
            out_buf_r = new float[OUT_BLOCK];
            out_buf = new float[OUT_BLOCK * 2];
            byte_buf = new byte[OUT_BLOCK * 2 * 4];

            length_counter = 0;
            record = true;

            if (sample_rate != Audio.SampleRate1)
            {
                resamp_l = DttSP.NewResamplerF(Audio.SampleRate1, sample_rate);
                if (channels > 1) resamp_r = DttSP.NewResamplerF(Audio.SampleRate1, sample_rate);
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

            while (record == true || rb_l.ReadSpace() > 0)
            {
                while (rb_l.ReadSpace() > IN_BLOCK ||
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

        unsafe public void AddWriteBuffer(float* left, float* right)
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
            if (sample_rate != Audio.SampleRate1)
            {
                fixed (float* in_ptr = &in_buf_l[0])
                fixed (float* out_ptr = &out_buf_l[0])
                    DttSP.DoResamplerF(in_ptr, out_ptr, cnt, &out_cnt, resamp_l);
                if (channels > 1)
                {
                    fixed (float* in_ptr = &in_buf_r[0])
                    fixed (float* out_ptr = &out_buf_r[0])
                        DttSP.DoResamplerF(in_ptr, out_ptr, cnt, &out_cnt, resamp_r);
                }
            }
            else
            {
                in_buf_l.CopyTo(out_buf_l, 0);
                in_buf_r.CopyTo(out_buf_r, 0);
            }

            if (channels > 1)
            {
                // interleave samples
                for (int i = 0; i < out_cnt; i++)
                {
                    out_buf[i * 2] = out_buf_l[i];
                    out_buf[i * 2 + 1] = out_buf_r[i];
                }
            }
            else
            {
                out_buf_l.CopyTo(out_buf, 0);
            }

            byte[] temp = new byte[4];
            int length = out_cnt;
            if (channels > 1) length *= 2;
            for (int i = 0; i < length; i++)
            {
                temp = BitConverter.GetBytes(out_buf[i]);
                for (int j = 0; j < 4; j++)
                    byte_buf[i * 4 + j] = temp[j];
            }

            writer.Write(byte_buf, 0, out_cnt * 2 * 4);
            count += out_cnt * 2 * 4;
        }

        // ke9ns: write a wav file (header)
        private void WriteWaveHeader(ref BinaryWriter writer, short channels, int sample_rate, short bit_depth, int data_length)
        {
            writer.Write(0x46464952);                               // "RIFF"		-- descriptor chunk ID
            writer.Write(data_length + 36);                         // size of whole file -- 1 for now
            writer.Write(0x45564157);                               // "WAVE"		-- descriptor type
            writer.Write(0x20746d66);                               // "fmt "		-- format chunk ID
            writer.Write((int)16);                                  // size of fmt chunk
            writer.Write((short)3);                                 // FormatTag	-- 3 for floats
            writer.Write(channels);                                 // wChannels
            writer.Write(sample_rate);                              // dwSamplesPerSec
            writer.Write((int)(channels * sample_rate * bit_depth / 8));    // dwAvgBytesPerSec
            writer.Write((short)(channels * bit_depth / 8));            // wBlockAlign
            writer.Write(bit_depth);                                // wBitsPerSample
            writer.Write(0x61746164);                               // "data" -- data chunk ID
            writer.Write(data_length);                              // chunkSize = length of data
            writer.Flush();                                         // write the file
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
            OUT_BLOCK = (int)Math.Ceiling(IN_BLOCK * Audio.SampleRate1 / (double)sample_rate); // SampleRate1 = SR of the Flex,  Sample_Rate = the wave file SR

            rb_l = new RingBufferFloat(16 * OUT_BLOCK);
            rb_r = new RingBufferFloat(16 * OUT_BLOCK);

            buf_l_in = new float[IN_BLOCK];  // floating arrays
            buf_r_in = new float[IN_BLOCK];
            buf_l_out = new float[OUT_BLOCK];
            buf_r_out = new float[OUT_BLOCK];

            if (format == 1)              // Format = 1 = PCM (2byte SINGLE)
            {
                io_buf_size = IN_BLOCK * 2 * 2;

                if (sample_rate != Audio.SampleRate1)  // ke9ns add   (this code was only in format==3, but copied it to format==1 If Flex and wave file SR dont match
                {
                    resamp_l = DttSP.NewResamplerF(sample_rate, Audio.SampleRate1);   // LEFT
                    if (channels > 1) resamp_r = DttSP.NewResamplerF(sample_rate, Audio.SampleRate1);   // RIGHT
                }

            }
            else if (format == 3)           // Format = 3 = ?? (float 4bytes, instead of SINGLE 2 bytes
            {
                io_buf_size = IN_BLOCK * 4 * 2;

                if (sample_rate != Audio.SampleRate1)  // If Flex and wave file SR dont match
                {
                    resamp_l = DttSP.NewResamplerF(sample_rate, Audio.SampleRate1);   // LEFT
                    if (channels > 1) resamp_r = DttSP.NewResamplerF(sample_rate, Audio.SampleRate1);   // RIGHT
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
            } while (rb_l.WriteSpace() > OUT_BLOCK && !eof); // do ReadBuffer to tak eout the leadin wave whitespace before handing it over to the Processbuffer to call the Readbuffer

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

                    if (playback == false) goto end;
                }

                if (playback == false)
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

            int i = 0, num_reads = IN_BLOCK;
            int val = reader.Read(io_buf, 0, io_buf_size);             // reader.  this is the open wave file binearystream

            if (val < io_buf_size)
            {
                eof = true;
                switch (format)
                {
                    case 1:     // ints
                        num_reads = val / 4;
                        break;
                    case 3:		// floats
                        num_reads = val / 8;
                        break;
                }
            } // if

            for (; i < num_reads; i++)
            {
                switch (format)
                {
                    case 1:     // FORMAT 1 = PCM = ints
                        buf_l_in[i] = (float)((double)BitConverter.ToInt16(io_buf, i * 4) / 32767.0);          // LEFT wave files are 2bytes (16bit) data saved as +/- 0 to 32767
                        if (channels > 1) buf_r_in[i] = (float)((double)BitConverter.ToInt16(io_buf, i * 4 + 2) / 32767.0);      // RIGHT buf_r_in[i] is now ke9ns MOD no channels if 



                        break;
                    case 3:     // FORMAT 3 = ???  floats
                        buf_l_in[i] = BitConverter.ToSingle(io_buf, i * 8);  // LEFT  wave files are 4 bytes (float) each data point
                        if (channels > 1) buf_r_in[i] = BitConverter.ToSingle(io_buf, i * 8 + 4);  // RIGHT  ke9ns MOD no channels if

                        break;
                }
            } // for

            if (num_reads < IN_BLOCK)  // BAD clear out buffer and STOP
            {
                for (int j = i; j < IN_BLOCK; j++)
                {
                    buf_l_in[j] = buf_r_in[j] = 0.0f;
                }
                playback = false;
                reader.Close();                                  // wave file is now closed
            }

            int out_cnt = IN_BLOCK;

            if (sample_rate != Audio.SampleRate1)
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
                    fixed (float* in_ptr = &buf_r_in[0])
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
        // ke9ns play wav files back
        unsafe public void GetPlayBuffer(float* left, float* right)
        {

            // Debug.WriteLine("GetPlayBuffer ("+rb_l.ReadSpace()+")");
            int count = rb_l.ReadSpace();

            if (count == 0) return;

            if (count > frames_per_buffer) count = frames_per_buffer;

            rb_l.ReadPtr(left, count); // LEFT

            rb_r.ReadPtr(right, count);  // RIGHT

            if (count < frames_per_buffer)  // BAD
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
