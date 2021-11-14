//=================================================================
// setup.cs
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

using Flex.Control;     // ke9ns add
using FlexCW;           //
using System;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using Microsoft.VisualBasic;


using System.Runtime.InteropServices;


namespace PowerSDR
{
    public partial class Setup : System.Windows.Forms.Form
    {
        #region Variable Declaration

        //	private Console console;  // was this
        public static Console console;   // ke9ns mod  to allow console to pass back values to setup screen
        public CATParser parser; // ke9ns add: for CAT URL support

        public static SpotControl SpotForm;                       // ke9ns add DX spotter function
        public static ScanControl ScanForm;                       // ke9ns add freq Scanner function
        public static FlexControlBasicForm FCBasicForm;

        //   public static Http httpFile;
       //   public static VolumeControl volumecontrol;

        HidDevice.PowerMate powerMate = new HidDevice.PowerMate();  // link back to PowerMate.cpp and PowerMate.h

        private Progress progress;
        private ArrayList KeyList;
        private int sound_card;
        private bool initializing;
        private bool dax_audio_setup_enum = false;

     

        #endregion

        #region Constructor and Destructor

        public Setup(Console c)
        {
            Debug.WriteLine("SETUP  FILE OPEN");

            InitializeComponent();

            console = c;
            openFileDialog1.InitialDirectory = String.Empty;
            openFileDialog1.InitialDirectory = console.AppDataPath;

            parser = new CATParser(console); // ke9ns add


#if (!DEBUG)
            comboGeneralProcessPriority.Items.Remove("Idle");
            comboGeneralProcessPriority.Items.Remove("Below Normal");
#endif

            grpGeneralModel.Visible = false;
            btnWizard.Visible = false;

            initializing = true;

            InitWindowTypes();

            GetMixerDevices();

            GetHosts();


            KeyList = new ArrayList();
            SetupKeyMap();

            GetTxProfiles();
            GetTxProfileDefs();

            RefreshCOMPortLists();

            RefreshSkinList();

            comboGeneralLPTAddr.SelectedIndex = -1;
            comboGeneralXVTR.SelectedIndex = (int)XVTRTRMode.POSITIVE;
            comboGeneralProcessPriority.Text = "Normal";
            comboOptFilterWidthMode.Text = "Linear";
            comboAudioSoundCard.Text = "Unsupported Card";
            comboAudioSampleRate1.SelectedIndex = 0;
            comboAudioSampleRate2.Text = "48000";
            comboAudioSampleRate3.Text = "48000";
            comboAudioBuffer1.Text = "2048";
            comboAudioBuffer2.Text = "512";
            comboAudioBuffer3.Text = "512";
            comboAudioChannels1.Text = "2";
            Audio.IN_RX1_L = 0;
            Audio.IN_RX1_R = 1;
            Audio.IN_RX2_L = 2;
            Audio.IN_RX2_R = 3;
            Audio.IN_TX_L = 4;
            Audio.IN_TX_R = 5;
            comboDisplayLabelAlign.Text = "Auto";
            comboDisplayDriver.Text = "GDI+";
            comboDSPPhoneRXBuf.Text = "2048";
            comboDSPPhoneTXBuf.Text = "2048";
            comboDSPCWRXBuf.Text = "2048";
            comboDSPCWTXBuf.Text = "2048";
            comboDSPDigRXBuf.Text = "2048";
            comboDSPDigTXBuf.Text = "2048";
            comboDSPWindow.SelectedIndex = (int)Window.BLKHARRIS;
            comboKeyerConnKeyLine.SelectedIndex = 0;
            comboKeyerConnSecondary.SelectedIndex = 0;
            comboKeyerConnPTTLine.SelectedIndex = 0;
            comboKeyerConnPrimary.SelectedIndex = 0;
            comboTXTUNMeter.SelectedIndex = 0;
            comboMeterType.Text = "Analog";
            if (comboCATPort.Items.Count > 0) comboCATPort.SelectedIndex = 0; 

            if (comboCATPort2.Items.Count > 0) comboCATPort2.SelectedIndex = 0; // ke9ns add .180
            if (comboCATPort3.Items.Count > 0) comboCATPort3.SelectedIndex = 0;
            if (comboCATPort4.Items.Count > 0) comboCATPort4.SelectedIndex = 0;
            if (comboCATPort5.Items.Count > 0) comboCATPort5.SelectedIndex = 0;
            if (comboCATPort6.Items.Count > 0) comboCATPort6.SelectedIndex = 0;

            if (comboROTORPort.Items.Count > 0) comboROTORPort.SelectedIndex = 0;  // ke9ns add

            if (comboCATPTTPort.Items.Count > 0) comboCATPTTPort.SelectedIndex = 0;

            comboCATbaud.Text = "1200";
            comboCATparity.Text = "none";
            comboCATdatabits.Text = "8";
            comboCATstopbits.Text = "1";
            comboCATRigType.Text = "PowerSDR";

            GetOptions();

            if (comboDSPPhoneRXBuf.SelectedIndex < 0 || comboDSPPhoneRXBuf.SelectedIndex >= comboDSPPhoneRXBuf.Items.Count)
                comboDSPPhoneRXBuf.SelectedIndex = comboDSPPhoneRXBuf.Items.Count - 1;
            if (comboDSPPhoneTXBuf.SelectedIndex < 0 || comboDSPPhoneTXBuf.SelectedIndex >= comboDSPPhoneTXBuf.Items.Count)
                comboDSPPhoneTXBuf.SelectedIndex = comboDSPPhoneTXBuf.Items.Count - 1;
            if (comboDSPCWRXBuf.SelectedIndex < 0 || comboDSPCWRXBuf.SelectedIndex >= comboDSPCWRXBuf.Items.Count)
                comboDSPCWRXBuf.SelectedIndex = comboDSPCWRXBuf.Items.Count - 1;
            if (comboDSPCWTXBuf.SelectedIndex < 0 || comboDSPCWTXBuf.SelectedIndex >= comboDSPCWTXBuf.Items.Count)
                comboDSPCWTXBuf.SelectedIndex = comboDSPCWTXBuf.Items.Count - 1;
            if (comboDSPDigRXBuf.SelectedIndex < 0 || comboDSPDigRXBuf.SelectedIndex >= comboDSPDigRXBuf.Items.Count)
                comboDSPDigRXBuf.SelectedIndex = comboDSPDigRXBuf.Items.Count - 1;
            if (comboDSPDigTXBuf.SelectedIndex < 0 || comboDSPDigTXBuf.SelectedIndex >= comboDSPDigTXBuf.Items.Count)
                comboDSPDigTXBuf.SelectedIndex = comboDSPDigTXBuf.Items.Count - 1;

            if (comboCATPort.SelectedIndex < 0)
            {
                if (comboCATPort.Items.Count > 0)  comboCATPort.SelectedIndex = 0;
                else
                {
                    chkCATEnable.Checked = false;
                    chkCATEnable.Enabled = false;
                }
            }

            // ke9ns add .180 
            if (comboCATPort2.SelectedIndex < 0)
            {
                if (comboCATPort2.Items.Count > 0) comboCATPort2.SelectedIndex = 0;
                else
                {
                    chkCATEnable2.Checked = false;
                    chkCATEnable2.Enabled = false;
                }
            }
            // ke9ns add .180 
            if (comboCATPort3.SelectedIndex < 0)
            {
                if (comboCATPort3.Items.Count > 0) comboCATPort3.SelectedIndex = 0;
                else
                {
                    chkCATEnable3.Checked = false;
                    chkCATEnable3.Enabled = false;
                }
            }
            // ke9ns add .180 
            if (comboCATPort4.SelectedIndex < 0)
            {
                if (comboCATPort4.Items.Count > 0) comboCATPort4.SelectedIndex = 0;
                else
                {
                    chkCATEnable4.Checked = false;
                    chkCATEnable4.Enabled = false;
                }
            }
            // ke9ns add .180 
            if (comboCATPort5.SelectedIndex < 0)
            {
                if (comboCATPort5.Items.Count > 0) comboCATPort5.SelectedIndex = 0;
                else
                {
                    chkCATEnable5.Checked = false;
                    chkCATEnable5.Enabled = false;
                }
            }
            // ke9ns add .200 SPOOF VFOA TO B
            if (comboCATPort6.SelectedIndex < 0)
            {
                if (comboCATPort6.Items.Count > 0) comboCATPort6.SelectedIndex = 0;
                else
                {
                    chkCATEnable6.Checked = false;
                    chkCATEnable6.Enabled = false;
                }
            }



            // ke9ns add
            if (comboROTORPort.SelectedIndex < 0)
            {
                if (comboROTORPort.Items.Count > 0)
                    comboROTORPort.SelectedIndex = 0;
                else
                {
                    chkROTOREnable.Checked = false;
                    chkROTOREnable.Enabled = false;
                }
            }



            cmboSigGenRXMode.Text = "Radio";
            cmboSigGenTXMode.Text = "Radio";

            if (comboAudioDriver1.SelectedIndex < 0 &&
                comboAudioDriver1.Items.Count > 0)
                comboAudioDriver1.SelectedIndex = 0;

            if (comboAudioDriver2.SelectedIndex < 0 &&
                comboAudioDriver2.Items.Count > 0)
                comboAudioDriver2.SelectedIndex = 0;

            if (comboAudioDriver2B.SelectedIndex < 0 &&
              comboAudioDriver2B.Items.Count > 0)
                comboAudioDriver2B.SelectedIndex = 0; // .204

            if (comboAudioDriver3.SelectedIndex < 0 &&
                comboAudioDriver3.Items.Count > 0)
                comboAudioDriver3.SelectedIndex = 0;

            /* ke9ns remove
			if(comboAudioMixer1.SelectedIndex < 0 &&
				comboAudioMixer1.Items.Count > 0)
				comboAudioMixer1.SelectedIndex = 0;
                */
            comboAudioBuffer1_SelectedIndexChanged(this, EventArgs.Empty);

            initializing = false;
            udDisplayScopeTime_ValueChanged(this, EventArgs.Empty);

            if (comboTXProfileName.SelectedIndex < 0 && comboTXProfileName.Items.Count > 0) comboTXProfileName.SelectedIndex = 0;


            current_profile = comboTXProfileName.Text;

            if (chkCATEnable.Checked)
            {
                chkCATEnable_CheckedChanged(this, EventArgs.Empty);
            }

            if (chkCATEnable2.Checked) // ke9ns add .180
            {
                chkCATEnable2_CheckedChanged(this, EventArgs.Empty);
            }

            if (chkCATEnable3.Checked) // ke9ns add .180
            {
                chkCATEnable3_CheckedChanged(this, EventArgs.Empty);
            }

            if (chkCATEnable4.Checked) // ke9ns add .180
            {
                chkCATEnable4_CheckedChanged(this, EventArgs.Empty);
            }

            if (chkCATEnable5.Checked) // ke9ns add .180
            {
                chkCATEnable5_CheckedChanged(this, EventArgs.Empty);
            }

            // ke9ns add
            if (chkROTOREnable.Checked)
            {
                chkROTOREnable_CheckedChanged(this, EventArgs.Empty);
            }

            if (chkCATPTTEnabled.Checked)
            {
                chkCATPTTEnabled_CheckedChanged(this, EventArgs.Empty);
            }

            comboKeyerConnSecondary_SelectedIndexChanged(this, EventArgs.Empty);

            if (radGenModelFLEX5000.Checked && DB.GetVars("Options").Count != 0)
                radGenModelFLEX5000_CheckedChanged(this, EventArgs.Empty);

            //ForceAllEvents();
            EventArgs e = EventArgs.Empty;
            tbOptUSBBuf_Scroll(this, e);
            comboGeneralLPTAddr_LostFocus(this, e);
            chkGeneralSpurRed_CheckedChanged(this, e);
            udDDSCorrection_ValueChanged(this, e);
            chkAudioLatencyManual1_CheckedChanged(this, e);
            udAudioLineIn1_ValueChanged(this, e);
            comboAudioReceive1_SelectedIndexChanged(this, e);
            udLMSANF_ValueChanged(this, e);
            udLMSNR_ValueChanged(this, e);
            udDSPImagePhaseTX_ValueChanged(this, e);
            udDSPImageGainTX_ValueChanged(this, e);
            udDSPCWPitch_ValueChanged(this, e);
            tbDSPAGCHangThreshold_Scroll(this, e);
            udTXFilterHigh_ValueChanged(this, e);
            udTXFilterLow_ValueChanged(this, e);
            tbRX1FilterAlpha_Scroll(this, e);
            tbMultiRXFilterAlpha_Scroll(this, e);

            tbPanAlpha_Scroll(this, e); // ke9ns add  fill alpha
            tbGridOffset_Scroll(this, e); // ke9ns add pan slider
            tbWaterOffset_Scroll(this, e); // ke9ns add water slider
            tbAGCTadj_Scroll(this, e); // ke9ns add agct slider
            tbPan3DAlpha_Scroll(this, e); // ke9ns add 3d slider
            chkBoxTitle_CheckedChanged_1(this, e); // ke9ns 
            chkBoxMax_CheckedChanged(this, e); // ke9ns 
            chkVFOLargeWindow_CheckedChanged(this, e); // ke9ns .228


            // ChkBoxPTTLatch_CheckedChanged(this, e); // ke9ns add


            for (int i = 0; i < 2; i++)
                for (int j = 0; j < 2; j++)
                    console.dsp.GetDSPRX(i, j).Update = true;
            comboDSPPhoneRXBuf_SelectedIndexChanged(this, EventArgs.Empty);
            comboDSPPhoneTXBuf_SelectedIndexChanged(this, EventArgs.Empty);

            openFileDialog1.Filter = "PowerSDR Database Files (*.xml) | *.xml";

            if (chkKWAI.Checked)
                AllowFreqBroadcast = true;
            else
                AllowFreqBroadcast = false;

            tkbarTestGenFreq.Value = console.CWPitch;
            FlexControlManager.DeviceCountChanged += new FlexControlManager.DeviceCountChangedHandler(FlexControlManager_DeviceCountChanged);

            // Mod DH1TW

            InitDJConsoles();

            if ( chkCatURLON.Checked  || Restart == true) // ke9ns add
            {

                chkCatURLON.Checked = false;
                
                chkCatURLON.Checked = true;
            }
            // End MOD


            if (console.CurrentModel == Model.FLEX5000 && FWCEEPROM.RX2OK) // ke9ns add .200
            {
                 chkCATEnable6.Enabled = true; // only enable if you have the 2nd RX unit
                Debug.WriteLine("SPOOF OK YOU HAVE 2nd RX");
            }
            else
            {
                chkCATEnable6.Enabled = false;
                Debug.WriteLine("SPOOF NOT ALLOWED");
            }

        } // setup

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {

                //  Debug.WriteLine("dispose");

                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #endregion

     
        #region Init Routines
        // ======================================================
        // Init Routines
        // ======================================================

        private void InitGeneralTab() // called 1 time at very start before setupForm is not considered null
        {
            if (console.Hdw != null) comboGeneralLPTAddr.Text = Convert.ToString(console.Hdw.LPTAddr, 16);
            udGeneralLPTDelay.Value = console.LatchDelay;

            chkGeneralRXOnly.Checked = console.RXOnly;

            chkGeneralUSBPresent.Checked = console.USBPresent;
            chkGeneralPAPresent.Checked = console.PAPresent;
            chkGeneralXVTRPresent.Checked = console.XVTRPresent;
            comboGeneralXVTR.SelectedItem = (int)console.CurrentXVTRTRMode;
            chkGeneralSpurRed.Checked = true;
            chkGeneralDisablePTT.Checked = console.DisablePTT;
            chkGeneralSoftwareGainCorr.Checked = console.NoHardwareOffset;
            chkGeneralEnableX2.Checked = console.X2Enabled;
            chkGeneralCustomFilter.Checked = console.EnableLPF0;
        }

        private void InitAudioTab()
        {

            // set driver type
            if (comboAudioDriver1.SelectedIndex < 0 && comboAudioDriver1.Items.Count > 0)
            {
                foreach (PADeviceInfo p in comboAudioDriver1.Items)
                {
                    if (p.Name == "ASIO")
                    {
                        comboAudioDriver1.SelectedItem = p;
                        break;
                    }
                }

                if (comboAudioDriver1.Text != "ASIO") comboAudioDriver1.Text = "MME";
            }


            // set Input device
            if (comboAudioInput1.Items.Count > 0) comboAudioInput1.SelectedIndex = 0;

            // set Output device
            if (comboAudioOutput1.Items.Count > 0) comboAudioOutput1.SelectedIndex = 0;

            // set sample rate
            comboAudioSampleRate1.Text = "96000";

            if (comboAudioReceive1.Enabled == true)
            {
                for (int i = 0; i < comboAudioReceive1.Items.Count; i++)
                {
                    if (((string)comboAudioReceive1.Items[i]).StartsWith("Line"))
                    {
                        comboAudioReceive1.SelectedIndex = i;
                        i = comboAudioReceive1.Items.Count;
                    }
                }
            }

            if (comboAudioTransmit1.Enabled == true)
            {
                for (int i = 0; i < comboAudioTransmit1.Items.Count; i++)
                {
                    if (((string)comboAudioTransmit1.Items[i]).StartsWith("Mic"))
                    {
                        comboAudioTransmit1.SelectedIndex = i;
                        i = comboAudioTransmit1.Items.Count;
                    }
                }
            }

            comboAudioBuffer1.Text = "2048";
            udAudioLatency1.Value = Audio.Latency1;
        }

        private void InitDisplayTab()
        {
            udDisplayGridMax.Value = Display.SpectrumGridMax;
            udDisplayGridMin.Value = Display.SpectrumGridMin;
            udDisplayGridStep.Value = Display.SpectrumGridStep;
            udDisplayFPS.Value = console.DisplayFPS;
            clrbtnWaterfallLow.Color = Display.WaterfallLowColor;
            udDisplayWaterfallLowLevel.Value = (decimal)Display.WaterfallLowThreshold;
            udDisplayWaterfallHighLevel.Value = (decimal)Display.WaterfallHighThreshold;
            udDisplayMeterDelay.Value = console.MeterDelay;
            udDisplayPeakText.Value = console.PeakTextDelay;
            udDisplayCPUMeter.Value = console.CPUMeterDelay;
            udDisplayPhasePts.Value = Display.PhaseNumPts;
            udDisplayMultiPeakHoldTime.Value = console.MultimeterPeakHoldTime;
            udDisplayMultiTextHoldTime.Value = console.MultimeterTextPeakTime;
        }

        private void InitDSPTab()
        {
            udDSPCWPitch.Value = console.CWPitch;
            comboDSPWindow.SelectedIndex = (int)console.dsp.GetDSPRX(0, 0).CurrentWindow;
        }

        private void InitKeyboardTab()
        {
            // set tune keys
            comboKBTuneUp1.Text = KeyToString(console.KeyTuneUp1);
            comboKBTuneUp2.Text = KeyToString(console.KeyTuneUp2);
            comboKBTuneUp3.Text = KeyToString(console.KeyTuneUp3);
            comboKBTuneUp4.Text = KeyToString(console.KeyTuneUp4);
            comboKBTuneUp5.Text = KeyToString(console.KeyTuneUp5);
            comboKBTuneUp6.Text = KeyToString(console.KeyTuneUp6);
            comboKBTuneUp7.Text = KeyToString(console.KeyTuneUp7);
            comboKBTuneDown1.Text = KeyToString(console.KeyTuneDown1);
            comboKBTuneDown2.Text = KeyToString(console.KeyTuneDown2);
            comboKBTuneDown3.Text = KeyToString(console.KeyTuneDown3);
            comboKBTuneDown4.Text = KeyToString(console.KeyTuneDown4);
            comboKBTuneDown5.Text = KeyToString(console.KeyTuneDown5);
            comboKBTuneDown6.Text = KeyToString(console.KeyTuneDown6);
            comboKBTuneDown7.Text = KeyToString(console.KeyTuneDown7);

            // set band keys
            comboKBBandDown.Text = KeyToString(console.KeyBandDown);
            comboKBBandUp.Text = KeyToString(console.KeyBandUp);

            // set filter keys
            comboKBFilterDown.Text = KeyToString(console.KeyFilterDown);
            comboKBFilterUp.Text = KeyToString(console.KeyFilterUp);

            // set mode keys
            comboKBModeDown.Text = KeyToString(console.KeyModeDown);
            comboKBModeUp.Text = KeyToString(console.KeyModeUp);

            // set RIT keys
            comboKBRITDown.Text = KeyToString(console.KeyRITDown);
            comboKBRITUp.Text = KeyToString(console.KeyRITUp);

            // set XIT keys
            comboKBXITDown.Text = KeyToString(console.KeyXITDown);
            comboKBXITUp.Text = KeyToString(console.KeyXITUp);

            // set CW keys
            comboKBCWDot.Text = KeyToString(console.KeyCWDot);
            comboKBCWDash.Text = KeyToString(console.KeyCWDash);
        }

        private void InitAppearanceTab()
        {
            clrbtnBackground.Color = Display.DisplayBackgroundColor;
            clrbtnGrid.Color = Display.GridColor;
            clrbtnZeroLine.Color = Display.GridZeroColor;
            clrbtnText.Color = Display.GridTextColor;
            clrbtnDataLine.Color = Display.DataLineColor;
            clrbtnFilter.Color = Display.DisplayFilterColor;
            clrbtnPan.Color = Display.DisplayPanFillColor; // ke9ns add

            tbWaterOffset.Value = Display.WATEROFFSET; // ke9ns add
            tbGridOffset.Value = Display.GRIDOFFSET; // ke9ns add


            clrbtnMeterLeft.Color = console.MeterLeftColor;
            clrbtnMeterRight.Color = console.MeterRightColor;
            clrbtnBtnSel.Color = console.ButtonSelectedColor;
            clrbtnVFODark.Color = console.VFOTextDarkColor;
            clrbtnVFOLight.Color = console.VFOTextLightColor;
            clrbtnBandDark.Color = console.BandTextDarkColor;
            clrbtnBandLight.Color = console.BandTextLightColor;
            clrbtnPeakText.Color = console.PeakTextColor;
            clrbtnOutOfBand.Color = console.OutOfBandColor;
        }

        #endregion

        #region Misc Routines
        // ======================================================
        // Misc Routines
        // ======================================================

        private void InitDelta44()
        {
            int retval = DeltaCP.Init();
            if (retval != 0) return;
            DeltaCP.SetLevels();
            DeltaCP.Close();
        }

        private void RefreshCOMPortLists()
        {
            string[] com_ports = Common.SortedComPorts();

            comboKeyerConnPrimary.Items.Clear();
            switch (console.CurrentModel)
            {
                case Model.FLEX3000:
                case Model.FLEX5000:
                case Model.FLEX1500:
                case Model.SDR1000:
                    comboKeyerConnPrimary.Items.Add("Radio");
                    break;
                default:
                    comboKeyerConnPrimary.Items.Add("None");
                    break;
            }

            comboKeyerConnSecondary.Items.Clear();
            comboKeyerConnSecondary.Items.Add("None");
            comboKeyerConnSecondary.Items.Add("CAT");

            comboCATPort.Items.Clear();

            comboCATPort2.Items.Clear(); // ke9ns add .180
            comboCATPort3.Items.Clear();
            comboCATPort4.Items.Clear();
            comboCATPort5.Items.Clear();
            comboCATPort6.Items.Clear(); // .200

            comboROTORPort.Items.Clear();  // ke9ns add

            comboCATPTTPort.Items.Clear();

            comboKeyerConnPrimary.Items.AddRange(com_ports);
            comboKeyerConnSecondary.Items.AddRange(com_ports);

            comboCATPort.Items.Add("None");
            comboCATPort.Items.AddRange(com_ports);

            comboCATPort2.Items.Add("None"); // ke9ns add .180
            comboCATPort2.Items.AddRange(com_ports);

            comboCATPort3.Items.Add("None"); // ke9ns add .180
            comboCATPort3.Items.AddRange(com_ports);

            comboCATPort4.Items.Add("None"); // ke9ns add .180
            comboCATPort4.Items.AddRange(com_ports);

            comboCATPort5.Items.Add("None"); // ke9ns add .180
            comboCATPort5.Items.AddRange(com_ports);

            comboCATPort6.Items.Add("None"); // ke9ns add .200
            comboCATPort6.Items.AddRange(com_ports);


            comboROTORPort.Items.Add("None");       // ke9ns add
            comboROTORPort.Items.AddRange(com_ports);

            comboCATPTTPort.Items.Add("None");
            comboCATPTTPort.Items.AddRange(com_ports);
        }

        private void RefreshSkinList()
        {
            string skin = comboAppSkin.Text;
            comboAppSkin.Items.Clear();
            string path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) +
                "\\FlexRadio Systems\\PowerSDR\\Skins";

            if (!Directory.Exists(path))
            {
                MessageBox.Show(new Form { TopMost = true }, "PowerSDR Skins were not found.\n" +
                    "Appearance will suffer until this is rectified.\n" +
                    "Please try running as Administrator. (Right-click on PowerSDR shortcut)",
                    "Skins files not found",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            foreach (string d in Directory.GetDirectories(path))
            {
                string s = d.Substring(d.LastIndexOf("\\") + 1);
                if (!s.StartsWith("."))
                    comboAppSkin.Items.Add(d.Substring(d.LastIndexOf("\\") + 1));
            }

            if (comboAppSkin.Items.Count == 0)
            {
                MessageBox.Show(new Form { TopMost = true }, "The console presentation files (skins) were not found.\n" +
                    "Appearance will suffer until this is rectified.\n" +
                    "Please visit www.flexradio.com to download the missing files.",
                    "Skins files not found",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            if (skin == "") comboAppSkin.Text = "KE9NS3_Spot";  // "Default 2012";
            else if (comboAppSkin.Items.Contains(skin)) comboAppSkin.Text = skin;
            else comboAppSkin.Text = "KE9NS3_Spot"; // "Default 2012";
        }

        private void InitWindowTypes()
        {
            for (Window w = Window.FIRST + 1; w < Window.LAST; w++)
            {
                string s = w.ToString().ToLower();
                s = s.Substring(0, 1).ToUpper() + s.Substring(1, s.Length - 1);
                comboDSPWindow.Items.Add(s);
            }
        }
        //===========================================================================
        private void GetHosts()  // do one time at startup
        {
            comboAudioDriver1.Items.Clear();
            comboAudioDriver2.Items.Clear();
            comboAudioDriver2B.Items.Clear(); // .204
            comboAudioDriver3.Items.Clear();

            int host_index = 0;


            foreach (string PAHostName in Audio.GetPAHosts())
            {
                if (Audio.GetPAInputDevices(host_index).Count > 0 || Audio.GetPAOutputDevices(host_index).Count > 0)
                {
                    comboAudioDriver1.Items.Add(new PADeviceInfo(PAHostName, host_index));


                    if (PAHostName != "Windows WASAPI" && PAHostName != "ASIO")
                    //  if (PAHostName != "Windows WASAPI") // ke9ns mod  this is the HPSDR version
                    {
                        PADeviceInfo devinfo = new PADeviceInfo(PAHostName, host_index);

                        comboAudioDriver2.Items.Add(new PADeviceInfo(PAHostName, host_index)); // ke9ns from hpsdr
                        comboAudioDriver2B.Items.Add(new PADeviceInfo(PAHostName, host_index)); // ke9ns .204

                        comboAudioDriver3.Items.Add(new PADeviceInfo(PAHostName, host_index)); // ke9ns from hpsdr

                        //  comboAudioDriver2.Items.Add(devinfo); // vac1 populate lists
                        //  comboAudioDriver3.Items.Add(devinfo); // vac2

                        //Debug.WriteLine("vac1 " + host_index + " devinfo "+ devinfo ); // SOUND DRIVERS: (0=MME, 1=Windows Direct Sound, 4=Windows WDM-KS

                        dax_audio_setup_enum = true;
                    }
                }
                host_index++; //Increment host index
            } // for

            if (!dax_audio_setup_enum)
            {
                chkAudioEnableVAC.Enabled = false; // VAC1
                chkVAC2Enable.Enabled = false;     // VAC2
                console.DisableDAX();
            }
            else
            {
                chkAudioEnableVAC.Enabled = true; // VAC1
                chkVAC2Enable.Enabled = true; // ke9ns add
                console.EnableDAX();
            }

        } // gethosts




        private void GetDevices1()  // PRIMARY
        {
            comboAudioInput1.Items.Clear();
            comboAudioOutput1.Items.Clear();

            int host = ((PADeviceInfo)comboAudioDriver1.SelectedItem).Index;

            ArrayList a = Audio.GetPAInputDevices(host);

            foreach (PADeviceInfo p in a)               // try each device and create list of inputs and outputs
            {
                comboAudioInput1.Items.Add(p);
                //  Debug.WriteLine("pri " + p + " in "); // SOUND input: ASIO FlexRadio 
            }
            a = Audio.GetPAOutputDevices(host);

            foreach (PADeviceInfo p in a)
            {
                comboAudioOutput1.Items.Add(p);

                //  Debug.WriteLine("pri " + p + " out "); // SOUND out: ASIO FlexRadio 
            }

        } // getdevices 1

        private void GetDevices2()  // VAC1
        {
            comboAudioInput2.Items.Clear();
            comboAudioOutput2.Items.Clear();


            int host = ((PADeviceInfo)comboAudioDriver2.SelectedItem).Index;


            ArrayList a = Audio.GetPAInputDevices(host);
            foreach (PADeviceInfo p in a)
            {
                comboAudioInput2.Items.Add(p);
               
                Debug.WriteLine("vac1 " + p + " in "); // SOUND input: (Microsoft Sound Mapper - Input,CABLE Output (VB-Audio Virtual, Mic in at front panel (Pink) (R

            }

            a = Audio.GetPAOutputDevices(host);
            foreach (PADeviceInfo p in a)
            {
                comboAudioOutput2.Items.Add(p);
                
                Debug.WriteLine("vac1 " + p + " out "); // SOUND output: (Microsoft Sound Mapper - Output,Speakers (Realtek High Definiti,CABLE Input (VB-Audio Virtual C )

            }
        } // GetDevices2() VAC1

        private void GetDevices2B()  // VAC1 .204
        {
            comboAudioInput2B.Items.Clear();
            comboAudioOutput2B.Items.Clear();


            int host = ((PADeviceInfo)comboAudioDriver2B.SelectedItem).Index;


            ArrayList a = Audio.GetPAInputDevices(host);
            foreach (PADeviceInfo p in a)
            {
                comboAudioInput2B.Items.Add(p);

                Debug.WriteLine("vac1 " + p + " in "); // SOUND input: (Microsoft Sound Mapper - Input,CABLE Output (VB-Audio Virtual, Mic in at front panel (Pink) (R

            }

            a = Audio.GetPAOutputDevices(host);
            foreach (PADeviceInfo p in a)
            {
                comboAudioOutput2B.Items.Add(p);

                Debug.WriteLine("vac1 " + p + " out "); // SOUND output: (Microsoft Sound Mapper - Output,Speakers (Realtek High Definiti,CABLE Input (VB-Audio Virtual C )

            }
        } // GetDevices2() VAC1

        private void GetDevices3()  // VAC2
        {
            comboAudioInput3.Items.Clear();
            comboAudioOutput3.Items.Clear();
            int host = ((PADeviceInfo)comboAudioDriver3.SelectedItem).Index;
            ArrayList a = Audio.GetPAInputDevices(host);
            foreach (PADeviceInfo p in a)
                comboAudioInput3.Items.Add(p);

            a = Audio.GetPAOutputDevices(host);
            foreach (PADeviceInfo p in a)
                comboAudioOutput3.Items.Add(p);
        }

        private void ControlList(Control c, ref ArrayList a)
        {
            if (c.Controls.Count > 0)
            {
                foreach (Control c2 in c.Controls)
                    ControlList(c2, ref a);
            }

            if (c.GetType() == typeof(CheckBoxTS) || c.GetType() == typeof(CheckBoxTS) ||
                c.GetType() == typeof(ComboBoxTS) || c.GetType() == typeof(ComboBox) ||
                c.GetType() == typeof(NumericUpDownTS) || c.GetType() == typeof(NumericUpDown) ||
                c.GetType() == typeof(RadioButtonTS) || c.GetType() == typeof(RadioButton) ||
                c.GetType() == typeof(TextBoxTS) || c.GetType() == typeof(TextBox) ||
                c.GetType() == typeof(TrackBarTS) || c.GetType() == typeof(TrackBar) ||
                c.GetType() == typeof(ColorButton))
                a.Add(c);
        }

        private static bool saving = false;

        public void SaveOptions()
        {
            // Automatically saves all control settings to the database in the tab
            // pages on this form of the following types: CheckBoxTS, ComboBox,
            // NumericUpDown, RadioButton, TextBox, and TrackBar (slider)

            saving = true;

            ArrayList a = new ArrayList();
            ArrayList temp = new ArrayList();

            ControlList(this, ref temp);

            foreach (Control c in temp)             // For each control
            {
                if (c.GetType() == typeof(CheckBoxTS)) a.Add(c.Name + "/" + ((CheckBoxTS)c).Checked.ToString());
                else if (c.GetType() == typeof(ComboBoxTS)) a.Add(c.Name + "/" + ((ComboBoxTS)c).Text);
                else if (c.GetType() == typeof(NumericUpDownTS)) a.Add(c.Name + "/" + ((NumericUpDownTS)c).Value.ToString());
                else if (c.GetType() == typeof(RadioButtonTS)) a.Add(c.Name + "/" + ((RadioButtonTS)c).Checked.ToString());
                else if (c.GetType() == typeof(TextBoxTS)) a.Add(c.Name + "/" + ((TextBoxTS)c).Text);
                else if (c.GetType() == typeof(TrackBarTS)) a.Add(c.Name + "/" + ((TrackBarTS)c).Value.ToString());
                else if (c.GetType() == typeof(ColorButton))
                {
                    Color clr = ((ColorButton)c).Color;
                    a.Add(c.Name + "/" + clr.R + "." + clr.G + "." + clr.B + "." + clr.A);
                }
#if (DEBUG)
				else if(c.GetType() == typeof(GroupBox) ||
					c.GetType() == typeof(CheckBoxTS) ||
					c.GetType() == typeof(ComboBox) ||
					c.GetType() == typeof(NumericUpDown) ||
					c.GetType() == typeof(RadioButton) ||
					c.GetType() == typeof(TextBox) ||
					c.GetType() == typeof(TrackBar))
					Debug.WriteLine(c.Name+" needs to be converted to a Thread Safe control.");
#endif
            } // foreach(Control c in temp)	

            DB.SaveVars("Options", ref a);      // save the values to the DB
            saving = false;

        } // SaveOptions()





        public void GetOptions()
        {
            // Automatically restores all controls from the database in the
            // tab pages on this form of the following types: CheckBoxTS, ComboBox,
            // NumericUpDown, RadioButton, TextBox, and TrackBar (slider)

            // get list of live controls
            ArrayList temp = new ArrayList();       // list of all first level controls
            ControlList(this, ref temp);

            ArrayList checkbox_list = new ArrayList();
            ArrayList combobox_list = new ArrayList();
            ArrayList numericupdown_list = new ArrayList();
            ArrayList radiobutton_list = new ArrayList();
            ArrayList textbox_list = new ArrayList();
            ArrayList trackbar_list = new ArrayList();
            ArrayList colorbutton_list = new ArrayList();

            //ArrayList controls = new ArrayList();	// list of controls to restore
            foreach (Control c in temp)
            {
                if (c.GetType() == typeof(CheckBoxTS))          // the control is a CheckBoxTS
                    checkbox_list.Add(c);
                else if (c.GetType() == typeof(ComboBoxTS))     // the control is a ComboBox
                    combobox_list.Add(c);
                else if (c.GetType() == typeof(NumericUpDownTS))    // the control is a NumericUpDown
                    numericupdown_list.Add(c);
                else if (c.GetType() == typeof(RadioButtonTS))  // the control is a RadioButton
                    radiobutton_list.Add(c);
                else if (c.GetType() == typeof(TextBoxTS))      // the control is a TextBox
                    textbox_list.Add(c);

                else if (c.GetType() == typeof(TrackBarTS))     // the control is a TrackBar (slider)
                    trackbar_list.Add(c);

                else if (c.GetType() == typeof(ColorButton))
                    colorbutton_list.Add(c);
            }
            temp.Clear();   // now that we have the controls we want, delete first list 

            ArrayList a = DB.GetVars("Options");                        // Get the saved list of controls
            a.Sort();
            int num_controls = checkbox_list.Count + combobox_list.Count +
                numericupdown_list.Count + radiobutton_list.Count +
                textbox_list.Count + trackbar_list.Count +
                colorbutton_list.Count;

            if (a.Count < num_controls)     // some control values are not in the database
            {                               // so set all of them to the defaults
                InitGeneralTab();
                InitAudioTab();
                InitDSPTab();
                InitDisplayTab();
                InitKeyboardTab();
                InitAppearanceTab();
            }

            // restore saved values to the controls
            foreach (string s in a)             // string is in the format "name,value"
            {
                string[] vals = s.Split('/');
                if (vals.Length > 2)
                {
                    for (int i = 2; i < vals.Length; i++)
                        vals[1] += "/" + vals[i];
                }

                string name = vals[0];
                string val = vals[1];

                if (s.StartsWith("chk"))            // control is a CheckBoxTS
                {
                    for (int i = 0; i < checkbox_list.Count; i++)
                    {   // look through each control to find the matching name
                        CheckBoxTS c = (CheckBoxTS)checkbox_list[i];
                        if (c.Name.Equals(name))        // name found
                        {
                            c.Checked = bool.Parse(val);    // restore value
                            i = checkbox_list.Count + 1;
                        }
                        if (i == checkbox_list.Count)
                            MessageBox.Show(new Form { TopMost = true }, "Control not found: " + name);
                    }
                }
                else if (s.StartsWith("combo")) // control is a ComboBox
                {
                    for (int i = 0; i < combobox_list.Count; i++)
                    {   // look through each control to find the matching name
                        ComboBoxTS c = (ComboBoxTS)combobox_list[i];
                        if (c.Name.Equals(name))        // name found
                        {
                            if (c.Items.Count > 0 && c.Items[0].GetType() == typeof(string))
                            {
                                c.Text = val;
                            }
                            else
                            {
                                foreach (object o in c.Items)
                                {
                                    if (o.ToString() == val)
                                        c.Text = val;   // restore value
                                }
                            }
                            i = combobox_list.Count + 1;
                        }
                        if (i == combobox_list.Count)
                            MessageBox.Show(new Form { TopMost = true }, "Control not found: " + name);
                    }
                }
                else if (s.StartsWith("ud"))
                {
                    for (int i = 0; i < numericupdown_list.Count; i++)
                    {   // look through each control to find the matching name
                        NumericUpDownTS c = (NumericUpDownTS)numericupdown_list[i];
                        if (c.Name.Equals(name))        // name found
                        {
                            decimal num = decimal.Parse(val);

                            if (num > c.Maximum) num = c.Maximum;       // check endpoints
                            else if (num < c.Minimum) num = c.Minimum;
                            c.Value = num;          // restore value
                            i = numericupdown_list.Count + 1;
                        }
                        if (i == numericupdown_list.Count)
                            MessageBox.Show(new Form { TopMost = true }, "Control not found: " + name);
                    }
                }
                else if (s.StartsWith("rad"))
                {   // look through each control to find the matching name
                    for (int i = 0; i < radiobutton_list.Count; i++)
                    {
                        RadioButtonTS c = (RadioButtonTS)radiobutton_list[i];
                        if (c.Name.Equals(name))        // name found
                        {
                            c.Checked = bool.Parse(val);    // restore value
                            i = radiobutton_list.Count + 1;
                        }
                        if (i == radiobutton_list.Count)
                            MessageBox.Show(new Form { TopMost = true }, "Control not found: " + name);
                    }
                }
                else if (s.StartsWith("txt"))
                {   // look through each control to find the matching name
                    for (int i = 0; i < textbox_list.Count; i++)
                    {
                        TextBoxTS c = (TextBoxTS)textbox_list[i];
                        if (c.Name.Equals(name))        // name found
                        {
                            c.Text = val;   // restore value
                            i = textbox_list.Count + 1;
                        }
                        if (i == textbox_list.Count)
                            MessageBox.Show(new Form { TopMost = true }, "Control not found: " + name);
                    }
                }
                else if (s.StartsWith("tb"))
                {
                    // look through each control to find the matching name
                    for (int i = 0; i < trackbar_list.Count; i++)
                    {
                        TrackBarTS c = (TrackBarTS)trackbar_list[i];
                        if (c.Name.Equals(name))        // name found
                        {
                            c.Value = Int32.Parse(val);
                            i = trackbar_list.Count + 1;
                        }
                        if (i == trackbar_list.Count)
                            MessageBox.Show(new Form { TopMost = true }, "Control not found: " + name);
                    }
                }
                else if (s.StartsWith("clrbtn"))
                {
                    string[] colors = val.Split('.');
                    if (colors.Length == 4)
                    {
                        int R, G, B, A;
                        R = Int32.Parse(colors[0]);
                        G = Int32.Parse(colors[1]);
                        B = Int32.Parse(colors[2]);
                        A = Int32.Parse(colors[3]);

                        for (int i = 0; i < colorbutton_list.Count; i++)
                        {
                            ColorButton c = (ColorButton)colorbutton_list[i];
                            if (c.Name.Equals(name))        // name found
                            {
                                c.Color = Color.FromArgb(A, R, G, B);
                                i = colorbutton_list.Count + 1;
                            }
                            if (i == colorbutton_list.Count)
                                MessageBox.Show(new Form { TopMost = true }, "Control not found: " + name);
                        }
                    }
                }
            }

            foreach (ColorButton c in colorbutton_list)
                c.Automatic = "";
        }

        private string KeyToString(Keys k)
        {
            if (!k.ToString().StartsWith("Oem"))
                return k.ToString();

            string s = "";
            switch (k)
            {
                case Keys.OemOpenBrackets:
                    s = "[";
                    break;
                case Keys.OemCloseBrackets:
                    s = "]";
                    break;
                case Keys.OemQuestion:
                    s = "/";
                    break;
                case Keys.OemPeriod:
                    s = ".";
                    break;
                case Keys.OemPipe:
                    if ((k & Keys.Shift) == 0)
                        s = "\\";
                    else s = "|";
                    break;
            }
            return s;
        }

        private void SetupKeyMap()
        {
            KeyList.Add(Keys.None);
            KeyList.Add(Keys.A);
            KeyList.Add(Keys.B);
            KeyList.Add(Keys.C);
            KeyList.Add(Keys.D);
            KeyList.Add(Keys.E);
            KeyList.Add(Keys.F);
            KeyList.Add(Keys.G);
            KeyList.Add(Keys.H);
            KeyList.Add(Keys.I);
            KeyList.Add(Keys.J);
            KeyList.Add(Keys.K);
            KeyList.Add(Keys.L);
            KeyList.Add(Keys.M);
            KeyList.Add(Keys.N);
            KeyList.Add(Keys.O);
            KeyList.Add(Keys.P);
            KeyList.Add(Keys.Q);
            KeyList.Add(Keys.R);
            KeyList.Add(Keys.S);
            KeyList.Add(Keys.T);
            KeyList.Add(Keys.U);
            KeyList.Add(Keys.V);
            KeyList.Add(Keys.W);
            KeyList.Add(Keys.X);
            KeyList.Add(Keys.Y);
            KeyList.Add(Keys.Z);
            KeyList.Add(Keys.F1);
            KeyList.Add(Keys.F2);
            KeyList.Add(Keys.F3);
            KeyList.Add(Keys.F4);
            KeyList.Add(Keys.F5);
            KeyList.Add(Keys.F6);
            KeyList.Add(Keys.F7);
            KeyList.Add(Keys.F8);
            KeyList.Add(Keys.F9);
            KeyList.Add(Keys.F10);
            KeyList.Add(Keys.Insert);
            KeyList.Add(Keys.Delete);
            KeyList.Add(Keys.Home);
            KeyList.Add(Keys.End);
            KeyList.Add(Keys.PageUp);
            KeyList.Add(Keys.PageDown);
            KeyList.Add(Keys.Up);
            KeyList.Add(Keys.Down);
            KeyList.Add(Keys.Left);
            KeyList.Add(Keys.Right);
            KeyList.Add(Keys.OemOpenBrackets);
            KeyList.Add(Keys.OemCloseBrackets);
            KeyList.Add(Keys.OemPeriod);
            KeyList.Add(Keys.OemQuestion);
            //			KeyList.Add(Keys.OemSemicolon);
            //			KeyList.Add(Keys.OemQuotes);
            //			KeyList.Add(Keys.Oemcomma);
            //			KeyList.Add(Keys.OemPeriod);
            //			KeyList.Add(Keys.OemBackslash);
            //			KeyList.Add(Keys.OemQuestion);

            foreach (Control c in tpKeyboard.Controls)
            {
                if (c.GetType() == typeof(GroupBoxTS))
                {
                    foreach (Control c2 in c.Controls)
                    {
                        if (c2.GetType() == typeof(ComboBoxTS))
                        {
                            ComboBoxTS combo = (ComboBoxTS)c2;
                            combo.Items.Clear();
                            foreach (Keys k in KeyList)
                            {
                                if (k.ToString().StartsWith("Oem"))
                                    combo.Items.Add(KeyToString(k));
                                else
                                    combo.Items.Add(k.ToString());
                            }
                        }
                    }
                }
                else if (c.GetType() == typeof(ComboBoxTS))
                {
                    ComboBoxTS combo = (ComboBoxTS)c;
                    combo.Items.Clear();
                    foreach (Keys k in KeyList)
                        combo.Items.Add(k.ToString());
                }
            }
        }

        private void UpdateMixerControls1()
        {
            if (comboAudioMixer1.SelectedIndex >= 0 &&
                comboAudioMixer1.Items.Count > 0)
            {
                int i = -1;

                i = Mixer.GetMux(comboAudioMixer1.SelectedIndex);
                if (i < 0 || i >= Mixer.MIXERR_BASE)
                {
                    comboAudioReceive1.Enabled = false;
                    comboAudioReceive1.Items.Clear();
                    comboAudioTransmit1.Enabled = false;
                    comboAudioTransmit1.Items.Clear();
                }
                else
                {
                    comboAudioReceive1.Enabled = true;
                    comboAudioTransmit1.Enabled = true;
                    GetMuxLineNames1();
                    for (int j = 0; j < comboAudioReceive1.Items.Count; j++)
                    {
                        if (((string)comboAudioReceive1.Items[j]).StartsWith("Line"))
                        {
                            comboAudioReceive1.SelectedIndex = j;
                            j = comboAudioReceive1.Items.Count;
                        }
                    }

                    if (comboAudioReceive1.SelectedIndex < 0)
                    {
                        for (int j = 0; j < comboAudioReceive1.Items.Count; j++)
                        {
                            if (((string)comboAudioReceive1.Items[j]).StartsWith("Analog"))
                            {
                                comboAudioReceive1.SelectedIndex = j;
                                j = comboAudioReceive1.Items.Count;
                            }
                        }
                    }

                    for (int j = 0; j < comboAudioTransmit1.Items.Count; j++)
                    {
                        if (((string)comboAudioTransmit1.Items[j]).StartsWith("Mic"))
                        {
                            comboAudioTransmit1.SelectedIndex = j;
                            j = comboAudioTransmit1.Items.Count;
                        }
                    }
                }
            }
        }

        private void GetMixerDevices()
        {
            comboAudioMixer1.Items.Clear();
            int num = Mixer.mixerGetNumDevs();
            for (int i = 0; i < num; i++)
            {
                comboAudioMixer1.Items.Add(Mixer.GetDevName(i));
            }
            comboAudioMixer1.Items.Add("None");
        }

        private void GetMuxLineNames1()
        {
            if (comboAudioMixer1.SelectedIndex >= 0 &&
                comboAudioMixer1.Items.Count > 0)
            {
                comboAudioReceive1.Items.Clear();
                comboAudioTransmit1.Items.Clear();

                ArrayList a;
                bool good = Mixer.GetMuxLineNames(comboAudioMixer1.SelectedIndex, out a);
                if (good)
                {
                    foreach (string s in a)
                    {
                        comboAudioReceive1.Items.Add(s);
                        comboAudioTransmit1.Items.Add(s);
                    }
                }
            }
        }

        private void ForceAllEvents()
        {
            EventArgs e = EventArgs.Empty;

            // General Tab
            comboGeneralLPTAddr_SelectedIndexChanged(this, e);
            udGeneralLPTDelay_ValueChanged(this, e);
            chkGeneralRXOnly_CheckedChanged(this, e);

            chkGeneralUSBPresent_CheckedChanged(this, e);
            chkGeneralPAPresent_CheckedChanged(this, e);
            chkGeneralATUPresent_CheckedChanged(this, e);
            chkXVTRPresent_CheckedChanged(this, e);
            comboGeneralXVTR_SelectedIndexChanged(this, e);
            udDDSCorrection_ValueChanged(this, e);
            udDDSPLLMult_ValueChanged(this, e);
            udDDSIFFreq_ValueChanged(this, e);
            chkGeneralSpurRed_CheckedChanged(this, e);
            chkGeneralDisablePTT_CheckedChanged(this, e);
            chkGeneralSoftwareGainCorr_CheckedChanged(this, e);
            chkGeneralEnableX2_CheckedChanged(this, e);
            udGeneralX2Delay_ValueChanged(this, e);
            chkGeneralCustomFilter_CheckedChanged(this, e);
            comboGeneralProcessPriority_SelectedIndexChanged(this, e);

            // Audio Tab
            comboAudioSoundCard_SelectedIndexChanged(this, e);
            comboAudioDriver1_SelectedIndexChanged(this, e);
            comboAudioInput1_SelectedIndexChanged(this, e);
            comboAudioOutput1_SelectedIndexChanged(this, e);
            comboAudioMixer1_SelectedIndexChanged(this, e);
            comboAudioReceive1_SelectedIndexChanged(this, e);
            comboAudioTransmit1_SelectedIndexChanged(this, e);
            //			comboAudioDriver2_SelectedIndexChanged(this, e);
            //			comboAudioInput2_SelectedIndexChanged(this, e);
            //			comboAudioOutput2_SelectedIndexChanged(this, e);
            //			comboAudioMixer2_SelectedIndexChanged(this, e);
            //			comboAudioReceive2_SelectedIndexChanged(this, e);
            //			comboAudioTransmit2_SelectedIndexChanged(this, e);
            comboAudioBuffer1_SelectedIndexChanged(this, e);
            comboAudioBuffer2_SelectedIndexChanged(this, e);
            comboAudioSampleRate1_SelectedIndexChanged(this, e);
            comboAudioSampleRate2_SelectedIndexChanged(this, e);
            udAudioLatency1_ValueChanged(this, e);
            udAudioLatency2_ValueChanged(this, e);
            udAudioLineIn1_ValueChanged(this, e);
            udAudioVoltage1_ValueChanged(this, e);
            chkAudioLatencyManual1_CheckedChanged(this, e);

            // Display Tab
            udDisplayGridMax_ValueChanged(this, e);
            udDisplayGridMin_ValueChanged(this, e);
            udDisplayGridStep_ValueChanged(this, e);
            udDisplayFPS_ValueChanged(this, e);
            udDisplayMeterDelay_ValueChanged(this, e);
            udDisplayPeakText_ValueChanged(this, e);
            udDisplayCPUMeter_ValueChanged(this, e);
            udDisplayPhasePts_ValueChanged(this, e);
            udDisplayAVGTime_ValueChanged(this, e);
            udDisplayWaterfallLowLevel_ValueChanged(this, e);
            udDisplayWaterfallHighLevel_ValueChanged(this, e);
            clrbtnWaterfallLow_Changed(this, e);
            udDisplayMultiPeakHoldTime_ValueChanged(this, e);
            udDisplayMultiTextHoldTime_ValueChanged(this, e);

            // DSP Tab
            udLMSANF_ValueChanged(this, e);
            udLMSNR_ValueChanged(this, e);
            udDSPImagePhaseTX_ValueChanged(this, e);
            udDSPImageGainTX_ValueChanged(this, e);
            udDSPAGCFixedGaindB_ValueChanged(this, e);
            udDSPAGCMaxGaindB_ValueChanged(this, e);
            udDSPCWPitch_ValueChanged(this, e);
            comboDSPWindow_SelectedIndexChanged(this, e);
            udDSPNB_ValueChanged(this, e);
            udDSPNB2_ValueChanged(this, e);

            // Transmit Tab
            udTXFilterHigh_ValueChanged(this, e);
            udTXFilterLow_ValueChanged(this, e);
            udTransmitTunePower_ValueChanged(this, e);
            udPAGain_ValueChanged(this, e);

            // Keyboard Tab
            comboKBTuneUp1_SelectedIndexChanged(this, e);
            comboKBTuneUp2_SelectedIndexChanged(this, e);
            comboKBTuneUp3_SelectedIndexChanged(this, e);
            comboKBTuneUp4_SelectedIndexChanged(this, e);
            comboKBTuneUp5_SelectedIndexChanged(this, e);
            comboKBTuneUp6_SelectedIndexChanged(this, e);
            comboKBTuneDown1_SelectedIndexChanged(this, e);
            comboKBTuneDown2_SelectedIndexChanged(this, e);
            comboKBTuneDown3_SelectedIndexChanged(this, e);
            comboKBTuneDown4_SelectedIndexChanged(this, e);
            comboKBTuneDown5_SelectedIndexChanged(this, e);
            comboKBTuneDown6_SelectedIndexChanged(this, e);
            comboKBBandUp_SelectedIndexChanged(this, e);
            comboKBBandDown_SelectedIndexChanged(this, e);
            comboKBFilterUp_SelectedIndexChanged(this, e);
            comboKBFilterDown_SelectedIndexChanged(this, e);
            comboKBModeUp_SelectedIndexChanged(this, e);
            comboKBModeDown_SelectedIndexChanged(this, e);

            // Appearance Tab
            clrbtnBtnSel_Changed(this, e);
            clrbtnVFODark_Changed(this, e);
            clrbtnVFOLight_Changed(this, e);
            clrbtnBandDark_Changed(this, e);
            clrbtnBandLight_Changed(this, e);
            clrbtnPeakText_Changed(this, e);
            clrbtnBackground_Changed(this, e);
            clrbtnGrid_Changed(this, e);
            clrbtnZeroLine_Changed(this, e);
            clrbtnFilter_Changed(this, e);
            clrbtnPan_Changed(this, e); // ke9ns add

            clrbtnText_Changed(this, e);
            clrbtnDataLine_Changed(this, e);
            udDisplayLineWidth_ValueChanged(this, e);
            udBandSegmentBoxLineWidth_ValueChanged(this, e);
            clrbtnMeterLeft_Changed(this, e);
            clrbtnMeterRight_Changed(this, e);


        } // force all events

        public string[] GetTXProfileStrings()
        {
            string[] s = new string[comboTXProfileName.Items.Count];
            for (int i = 0; i < comboTXProfileName.Items.Count; i++)
                s[i] = (string)comboTXProfileName.Items[i];
            return s;
        }


        public string TXProfile
        {
            get
            {
                if (comboTXProfileName != null) return comboTXProfileName.Text;
                else return "";
            }
            set { if (comboTXProfileName != null) comboTXProfileName.Text = value; }
        }


        //==========================================================
        //==========================================================
        // ke9ns add: used to pass meter type selection from console to setup.
        //==========================================================
        //==========================================================
        public string MTRSet // ke9ns add
        {
            get
            {
                if (comboMeterType != null) return comboMeterType.Text;
                else return "";
            }
            set
            {

                if (comboMeterType != null)
                {
                    comboMeterType.Text = value;
                    //   Debug.WriteLine("meter change via click " + value);
                }

            }
        }

        //========================================================================
        public void GetTxProfiles()
        {
            comboTXProfileName.Items.Clear();
            foreach (DataRow dr in DB.ds.Tables["TxProfile"].Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    if (!comboTXProfileName.Items.Contains(dr["Name"])) comboTXProfileName.Items.Add(dr["Name"]);
                }
            }
        }

        public void GetTxProfileDefs()
        {
            lstTXProfileDef.Items.Clear();
            foreach (DataRow dr in DB.ds.Tables["TxProfileDef"].Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    if (!lstTXProfileDef.Items.Contains(dr["Name"])) lstTXProfileDef.Items.Add(dr["name"]);
                }
            }
        }


        //=====================================================================
        // ke9ns click on another TXprofile on the list to change over to.
        //       this checks to see if anything was changed
        private bool CheckTXProfileChanged()
        {
            DB.ModTXProfileTable();
            DB.ModTXProfileDefTable();


            DataRow[] rows = DB.ds.Tables["TxProfile"].Select("'" + current_profile + "' = Name"); // find the current TXprofile before you change to new TXprofile


            if (rows.Length != 1) return false;

            int[] eq = console.eqForm.TXEQ; // ke9ns check eqForm radio button for 28band eq

            if (eq[0] != (int)rows[0]["TXEQPreamp"]) return true;

            if (console.eqForm.TXEQEnabled != (bool)rows[0]["TXEQEnabled"]) return true;



            for (int i = 1; i < eq.Length; i++)
            {
                Debug.WriteLine("2EQ " + i + " , " + eq[i] + "," + eq.Length);

                try
                {
                    if (eq[i] != (int)rows[0]["TXEQ" + i.ToString()]) return true;

                }
                catch (Exception)
                {
                    Debug.WriteLine("TXEQ bad at " + i);
                    break;
                }

            } // for loop
              //--------------------------------------------------
              //   Debug.WriteLine("6EQ ");

            int[] eq1 = console.eqForm.TXEQ28; // ke9ns check eqForm radio button for 28band eq


            Debug.WriteLine("9EQ " + rows[0]["TXEQ28Preamp"]);

            try
            {
                if (eq1[0] != (int)rows[0]["TXEQ28Preamp"]) return true;
            }
            catch (Exception)
            {
                return true;
            }

            Debug.WriteLine("8EQ ");

            for (int iq = 1; iq < eq1.Length; iq++)
            {
                Debug.WriteLine("3EQ " + iq + " , " + eq1[iq] + ", " + eq1.Length);

                try
                {
                    if (eq1[iq] != (int)rows[0]["TX28EQ" + iq.ToString()]) return true;
                }
                catch (Exception)
                {
                    Debug.WriteLine("TX28EQ bad at " + iq);
                    return true;

                }


            } // for loop

            Debug.WriteLine("5EQ ");

            int[] eq2 = console.eqForm.PEQ; // ke9ns check eqForm radio button for 28band eq

            try
            {
                if (eq2[0] != (int)rows[0]["TXEQ9Preamp"]) return true;
            }
            catch (Exception)
            {
                return true;
            }


            for (int i = 1; i < 10; i++)
            {
                Debug.WriteLine("4EQ " + i + " , " + eq2[i]);

                try
                {
                    if (eq2[i] != (int)rows[0]["PEQ" + i.ToString()]) return true;
                    if (eq2[i + 9] != (int)rows[0]["PEQoctave" + i.ToString()]) return true;
                    if (eq2[i + 18] != (int)rows[0]["PEQfreq" + i.ToString()]) return true;

                }
                catch (Exception)
                {
                    Debug.WriteLine("PEQ bad at " + i);
                    return true;

                    //  break;
                }


            } // for loop





            //==============================================================================


            if (udTXFilterLow.Value != (int)rows[0]["FilterLow"] || udTXFilterHigh.Value != (int)rows[0]["FilterHigh"] ||
                            console.CPDR != (bool)rows[0]["CompanderOn"] || console.CPDRLevel != (int)rows[0]["CompanderLevel"] ||
                            console.Mic != (int)rows[0]["MicGain"])
                return true;

            return false;

        } // private bool CheckTXProfileChanged()


        //================================================================================
        //
        public void SaveTXProfileData()
        {
            Debug.WriteLine("SAVETXPROFILE");

            if (profile_deleted == true)
            {
                profile_deleted = false;
                return;
            }

            string name = current_profile;

            DB.ModTXProfileTable(); // ke9ns add: to modify old databases
            DB.ModTXProfileDefTable();

            DataRow dr = null;       //  t.Columns.Add("TXEQ11", typeof(int)); // ke9ns add

            foreach (DataRow d in DB.ds.Tables["TxProfile"].Rows)
            {
                if ((string)d["Name"] == name)
                {

                    //   DataRow[] rows = DB.ds.Tables["TxProfile"].Select("'" + comboTXProfileName.Text + "' = Name");
                    //   if (rows.Length == 1) rows[0].Delete();



                    dr = d;
                    break;
                }
            }


            dr["FilterLow"] = (int)udTXFilterLow.Value;
            dr["FilterHigh"] = (int)udTXFilterHigh.Value;
            //-------------------------------------------------------------
            dr["TXEQEnabled"] = console.eqForm.TXEQEnabled;
            dr["TXEQNumBands"] = console.eqForm.NumBands;

            //-------------------------------------------------------------
            // ke9ns add:
            dr["TXEQ28Band"] = console.eqForm.rad28Band.Checked;
            dr["TXEQ10Band"] = console.eqForm.rad10Band.Checked;
            dr["TXEQBand"] = console.eqForm.rad3Band.Checked;
            dr["TXEQ9Band"] = console.eqForm.radPEQ.Checked;
            dr["TXEQ37Band"] = console.eqForm.chkBothEQ.Checked;

            //------------------------------------------------------------------
            // ke9ns: 3 or 10 band
            int[] eq = console.eqForm.TXEQ;
            dr["TXEQPreamp"] = eq[0];

            for (int i = 1; i < eq.Length; i++)
            {
                //  Debug.WriteLine("3EQ " + i);
                dr["TXEQ" + i.ToString()] = eq[i];
            }

            //------------------------------------------------------------------
            // ke9ns add: Save 28bandEQ values

            int[] eq1 = console.eqForm.TXEQ28;

            dr["TXEQ28Preamp"] = eq1[0];

            for (int i = 1; i < eq1.Length; i++)
            {

                try
                {
                    dr["TX28EQ" + i.ToString()] = eq1[i];
                }
                catch (Exception)
                {
                    Debug.WriteLine("TX28EQ bad at " + i);
                }
            }

            //------------------------------------------------------------------
            // ke9ns add: Save 9band peq


            int[] eq2 = console.eqForm.PEQ; // ke9ns: PEQ data from the eqform

            dr["TXEQ9Preamp"] = eq2[0];

            for (int i = 1; i < eq2.Length; i++)
            {

                try
                {
                    dr["PEQ" + i.ToString()] = eq2[i];
                }
                catch (Exception)
                {
                    Debug.WriteLine("PEQ bad at " + i);
                }

            }

            for (int i = 10; i < 19; i++)
            {

                try
                {
                    dr["PEQoctave" + (i - 9).ToString()] = eq2[i];
                }
                catch (Exception)
                {
                    Debug.WriteLine("PEQoctave bad at " + i);
                }

            }

            for (int i = 19; i < eq2.Length; i++)
            {

                try
                {
                    dr["PEQfreq" + (i - 18).ToString()] = eq2[i];
                }
                catch (Exception)
                {
                    Debug.WriteLine("1PEQfreq bad at " + i);
                }

            }

            //------------------------------------------------------------------

            dr["RX1DSPMODE"] = console.RX1DSPMODE; // .196
            dr["RX2DSPMODE"] = console.RX2DSPMODE; // .196


            //------------------------------------------------------------------
            dr["DXOn"] = console.DX;
            dr["DXLevel"] = console.DXLevel;
            dr["CompanderOn"] = console.CPDR;
            dr["CompanderLevel"] = console.CPDRLevel;

            dr["MicGain"] = console.Mic;
            dr["FMMicGain"] = console.FMMic;

            dr["Lev_On"] = chkDSPLevelerEnabled.Checked;
            dr["Lev_Slope"] = (int)udDSPLevelerSlope.Value;
            dr["Lev_MaxGain"] = (int)udDSPLevelerThreshold.Value;
            dr["Lev_Attack"] = (int)udDSPLevelerAttack.Value;
            dr["Lev_Decay"] = (int)udDSPLevelerDecay.Value;
            dr["Lev_Hang"] = (int)udDSPLevelerHangTime.Value;
            dr["Lev_HangThreshold"] = tbDSPLevelerHangThreshold.Value;

            dr["ALC_Slope"] = (int)udDSPALCSlope.Value;
            dr["ALC_MaxGain"] = (int)udDSPALCThreshold.Value;
            dr["ALC_Attack"] = (int)udDSPALCAttack.Value;
            dr["ALC_Decay"] = (int)udDSPALCDecay.Value;
            dr["ALC_Hang"] = (int)udDSPALCHangTime.Value;
            dr["ALC_HangThreshold"] = tbDSPALCHangThreshold.Value;

            dr["Power"] = console.PWR;

            dr["Dexp_On"] = chkTXNoiseGateEnabled.Checked;
            dr["Dexp_Threshold"] = (int)udTXNoiseGate.Value;
            dr["Dexp_Attenuate"] = (int)udTXNoiseGateAttenuate.Value;

            dr["VOX_On"] = chkTXVOXEnabled.Checked;
            dr["VOX_Threshold"] = (int)udTXVOXThreshold.Value;
            dr["VOX_HangTime"] = (int)udTXVOXHangTime.Value;
            dr["Tune_Power"] = (int)udTXTunePower.Value;
            dr["Tune_Meter_Type"] = (string)comboTXTUNMeter.Text;

            dr["TX_Limit_Slew"] = (bool)chkTXLimitSlew.Checked;
            dr["TXBlankingTime"] = (int)udTX1500PhoneBlanking.Value;
            dr["MicBoost"] = (bool)chkAudioMicBoost.Checked;

            dr["TX_AF_Level"] = console.TXAF;

            dr["AM_Carrier_Level"] = (int)udTXAMCarrierLevel.Value;

            dr["Show_TX_Filter"] = (bool)console.ShowTXFilter;

            dr["VAC1_On"] = (bool)chkAudioEnableVAC.Checked;
            dr["VAC1_Auto_On"] = (bool)chkAudioVACAutoEnable.Checked;
            dr["VAC1_RX_GAIN"] = (int)udAudioVACGainRX.Value;
            dr["VAC1_TX_GAIN"] = (int)udAudioVACGainTX.Value;
            dr["VAC1_Stereo_On"] = (bool)chkAudio2Stereo.Checked;
            dr["VAC1_Sample_Rate"] = (string)comboAudioSampleRate2.Text;
            dr["VAC1_Buffer_Size"] = (string)comboAudioBuffer2.Text;
            dr["VAC1_IQ_Output"] = (bool)chkAudioIQtoVAC.Checked;
            dr["VAC1_IQ_Correct"] = (bool)chkAudioCorrectIQ.Checked;
            dr["VAC1_PTT_OverRide"] = (bool)chkVACAllowBypass.Checked;
            dr["VAC1_Combine_Input_Channels"] = (bool)chkVACCombine.Checked;
            dr["VAC1_Latency_On"] = true;
            dr["VAC1_Latency_Duration"] = (int)udAudioLatency2.Value;

            dr["VAC2_On"] = (bool)chkVAC2Enable.Checked;
            dr["VAC2_Auto_On"] = (bool)chkVAC2AutoEnable.Checked;
            dr["VAC2_RX_GAIN"] = (int)udVAC2GainRX.Value;
            dr["VAC2_TX_GAIN"] = (int)udVAC2GainTX.Value;
            dr["VAC2_Stereo_On"] = (bool)chkAudioStereo3.Checked;
            dr["VAC2_Sample_Rate"] = (string)comboAudioSampleRate3.Text;
            dr["VAC2_Buffer_Size"] = (string)comboAudioBuffer3.Text;
            dr["VAC2_IQ_Output"] = (bool)chkVAC2DirectIQ.Checked;
            dr["VAC2_IQ_Correct"] = (bool)chkVAC2DirectIQCal.Checked;
            dr["VAC2_Combine_Input_Channels"] = (bool)chkVAC2Combine.Checked;
            dr["VAC2_Latency_On"] = true;
            dr["VAC2_Latency_Duration"] = (int)udVAC2Latency.Value;

            dr["Phone_RX_DSP_Buffer"] = (string)comboDSPPhoneRXBuf.Text;
            dr["Phone_TX_DSP_Buffer"] = (string)comboDSPPhoneTXBuf.Text;
            dr["Digi_RX_DSP_Buffer"] = (string)comboDSPDigRXBuf.Text;
            dr["Digi_TX_DSP_Buffer"] = (string)comboDSPDigTXBuf.Text;
            dr["CW_RX_DSP_Buffer"] = (string)comboDSPCWRXBuf.Text;

            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                    dr["Mic_Input_On"] = (string)console.fwcMixForm.MicInputSelected;
                    dr["Mic_Input_Level"] = (int)console.fwcMixForm.MicInput;
                    dr["Line_Input_On"] = (string)console.fwcMixForm.LineInRCASelected;
                    dr["Line_Input_Level"] = (int)console.fwcMixForm.LineInRCA;
                    dr["Balanced_Line_Input_On"] = (string)console.fwcMixForm.LineInPhonoSelected;
                    dr["Balanced_Line_Input_Level"] = (int)console.fwcMixForm.LineInPhono;
                    dr["FlexWire_Input_On"] = (string)console.fwcMixForm.LineInDB9Selected;
                    dr["FlexWire_Input_Level"] = (int)console.fwcMixForm.LineInDB9;
                    break;

                case Model.FLEX3000:
                    dr["Mic_Input_On"] = (string)console.flex3000MixerForm.MicInputSelected;
                    dr["Mic_Input_Level"] = (int)console.flex3000MixerForm.MicInput;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = (string)console.flex3000MixerForm.LineInDB9Selected;
                    dr["FlexWire_Input_Level"] = (int)console.flex3000MixerForm.LineInDB9;
                    break;

                case Model.FLEX1500:
                    dr["Mic_Input_On"] = (string)console.flex1500MixerForm.MicInputSelectedStr;
                    dr["Mic_Input_Level"] = (int)console.flex1500MixerForm.MicInput;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = (string)console.flex1500MixerForm.LineInDB9Selected;
                    dr["FlexWire_Input_Level"] = (int)console.flex1500MixerForm.FlexWireIn;
                    break;

                default:
                    dr["Mic_Input_On"] = "0";
                    dr["Mic_Input_Level"] = 0;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 0;
                    break;
            } //switch

        } // SaveTXProfileData()

        public void UpdateWaterfallBandInfo()
        {
            if (!initializing)
            {
                switch (console.RX1Band)
                {
                    case Band.B160M: txtWaterFallBandLevel.Text = "160 meters"; break;
                    case Band.B80M: txtWaterFallBandLevel.Text = "80 meters"; break;
                    case Band.B60M: txtWaterFallBandLevel.Text = "60 meters"; break;
                    case Band.B40M: txtWaterFallBandLevel.Text = "40 meters"; break;
                    case Band.B30M: txtWaterFallBandLevel.Text = "30 meters"; break;
                    case Band.B20M: txtWaterFallBandLevel.Text = "20 meters"; break;
                    case Band.B17M: txtWaterFallBandLevel.Text = "17 meters"; break;
                    case Band.B15M: txtWaterFallBandLevel.Text = "15 meters"; break;
                    case Band.B12M: txtWaterFallBandLevel.Text = "12 meters"; break;
                    case Band.B10M: txtWaterFallBandLevel.Text = "10 meters"; break;
                    case Band.B6M: txtWaterFallBandLevel.Text = "6 meters"; break;
                    case Band.WWV: txtWaterFallBandLevel.Text = "WWV"; break;

                    case Band.BLMF: txtWaterFallBandLevel.Text = "LW/MW bands"; break; // ke9ns add
                    case Band.B120M: txtWaterFallBandLevel.Text = "120 meters"; break;
                    case Band.B90M: txtWaterFallBandLevel.Text = "90 meters"; break;
                    case Band.B61M: txtWaterFallBandLevel.Text = "60 meters"; break;
                    case Band.B49M: txtWaterFallBandLevel.Text = "49 meters"; break;
                    case Band.B41M: txtWaterFallBandLevel.Text = "41 meters"; break;
                    case Band.B31M: txtWaterFallBandLevel.Text = "31 meters"; break;
                    case Band.B25M: txtWaterFallBandLevel.Text = "25 meters"; break;
                    case Band.B22M: txtWaterFallBandLevel.Text = "22 meters"; break;
                    case Band.B19M: txtWaterFallBandLevel.Text = "19 meters"; break;
                    case Band.B16M: txtWaterFallBandLevel.Text = "16 meters"; break;
                    case Band.B14M: txtWaterFallBandLevel.Text = "14 meters"; break;
                    case Band.B13M: txtWaterFallBandLevel.Text = "13 meters"; break;
                    case Band.B11M: txtWaterFallBandLevel.Text = "11 meters"; break;

                    case Band.GEN: txtWaterFallBandLevel.Text = "General"; break;

                    default: txtWaterFallBandLevel.Text = "2M & VHF+"; break;
                }
            }
        }
        #endregion

        #region Properties



        private bool flexcontrol_present = false;
        public bool FlexControlPresent
        {
            get { return flexcontrol_present; }
            set
            {
                flexcontrol_present = value;
                if (value)
                {
                    if (FlexControlManager.DeviceCount == 0)
                    {
                        FlexControlPresent = false;
                        return;
                    }

                    FlexControl fc = FlexControlManager.GetFlexControl(0);
                    string ver = Common.RevToString(fc.GetFirmwareVersion());
                    string temp = "FlexControl: v" + ver;
                    lblFlexControlRev.Text = temp;
                    lblFlexControlRev1500.Text = temp;
                    lblFlexControlRev1K.Text = temp;
                }

                lblFlexControlRev.Visible = value;
                lblFlexControlRev1500.Visible = value;
                lblFlexControlRev1K.Visible = value;
            }
        }

        private bool audio_stream_running = false;
        public bool AudioStreamRunning
        {
            get { return audio_stream_running; }
            set
            {
                audio_stream_running = value;
                tbOptUSBBuf.Enabled = !value;
            }
        }

        public int VACDriver
        {
            get
            {
                return comboAudioDriver2.SelectedIndex;
            }
            set
            {
                if (comboAudioDriver2.Items.Count - 1 >= value)
                    comboAudioDriver2.SelectedIndex = value;
            }
        }
        public int VACDriverB
        {
            get
            {
                return comboAudioDriver2B.SelectedIndex;
            }
            set
            {
                if (comboAudioDriver2B.Items.Count - 1 >= value)
                    comboAudioDriver2B.SelectedIndex = value;
            }
        }


        public int VAC2Driver
        {
            get
            {
                return comboAudioDriver3.SelectedIndex;
            }
            set
            {
                if (comboAudioDriver3.Items.Count - 1 >= value)
                    comboAudioDriver3.SelectedIndex = value;
            }
        }

        public int VACInputCable
        {
            get
            {
                return comboAudioInput2.SelectedIndex;
            }
            set
            {
                if (comboAudioInput2.Items.Count - 1 >= value)
                    comboAudioInput2.SelectedIndex = value;
            }
        }

        public int VAC2InputCable
        {
            get
            {
                return comboAudioInput3.SelectedIndex;
            }
            set
            {
                if (comboAudioInput3.Items.Count - 1 >= value)
                    comboAudioInput3.SelectedIndex = value;
            }
        }

        public int VACOutputCable
        {
            get
            {
                return comboAudioOutput2.SelectedIndex;
            }
            set
            {
                if (comboAudioOutput2.Items.Count - 1 >= value)
                    comboAudioOutput2.SelectedIndex = value;
            }
        }

        public int VAC2OutputCable
        {
            get
            {
                return comboAudioOutput3.SelectedIndex;
            }
            set
            {
                if (comboAudioOutput3.Items.Count - 1 >= value)
                    comboAudioOutput3.SelectedIndex = value;
            }
        }

        public bool VACUseRX2
        {
            get
            {
                if (chkAudioRX2toVAC != null && IQOutToVAC) return chkAudioRX2toVAC.Checked;
                else return false;
            }
            set
            {
                if (chkAudioRX2toVAC != null && IQOutToVAC) chkAudioRX2toVAC.Checked = value;
            }
        }

        public bool VAC2UseRX2
        {
            get
            {
                if (chkVAC2UseRX2 != null && IQOutToVAC) return chkVAC2UseRX2.Checked;
                else return false;
            }
            set
            {
                if (chkVAC2UseRX2 != null && IQOutToVAC) chkVAC2UseRX2.Checked = value;
            }
        }


        public bool CATEnabled
        {
            get
            {
                if (chkCATEnable != null) return chkCATEnable.Checked;
                else return false;
            }
            set
            {
                if (chkCATEnable != null) chkCATEnable.Checked = value;
            }
        } // CATEnabled
          //================================================================
          // ke9ns add .180 port 2 - 5

        public bool CATEnabled2
        {
            get
            {
                if (chkCATEnable2 != null) return chkCATEnable2.Checked;
                else return false;
            }
            set
            {
                if (chkCATEnable2 != null) chkCATEnable2.Checked = value;
            }
        } // CATEnabled2

        public bool CATEnabled3
        {
            get
            {
                if (chkCATEnable3 != null) return chkCATEnable3.Checked;
                else return false;
            }
            set
            {
                if (chkCATEnable3 != null) chkCATEnable3.Checked = value;
            }
        } // CATEnabled3

        public bool CATEnabled4
        {
            get
            {
                if (chkCATEnable4 != null) return chkCATEnable4.Checked;
                else return false;
            }
            set
            {
                if (chkCATEnable4 != null) chkCATEnable4.Checked = value;
            }
        } // CATEnabled4

        public bool CATEnabled5
        {
            get
            {
                if (chkCATEnable5 != null) return chkCATEnable5.Checked;
                else return false;
            }
            set
            {
                if (chkCATEnable5 != null) chkCATEnable5.Checked = value;
            }
        } // CATEnabled5

        public bool CATEnabled6 // .200
        {
            get
            {
                if (chkCATEnable6 != null) return chkCATEnable6.Checked;
                else return false;
            }
            set
            {
                if (chkCATEnable6 != null) chkCATEnable6.Checked = value;
            }
        } // CATEnabled6

        //================================================================
        // ke9ns add
        public bool ROTOREnabled
        {
            get
            {
                if (chkROTOREnable != null) return chkROTOREnable.Checked;
                else return false;
            }
            set
            {
                if (chkROTOREnable != null) chkROTOREnable.Checked = value;
            }
        } // ROTOREnabled


        public int RXAGCAttack
        {
            get
            {
                if (udDSPAGCAttack != null) return (int)udDSPAGCAttack.Value;
                else return 0;
            }
            set
            {
                if (udDSPAGCAttack != null) udDSPAGCAttack.Value = value;
            }
        }

        public int RXAGCHang
        {
            get
            {
                if (udDSPAGCHangTime != null) return (int)udDSPAGCHangTime.Value;
                else return 0;
            }
            set
            {
                if (udDSPAGCHangTime != null) udDSPAGCHangTime.Value = value;
            }
        }

        public int RXAGCDecay
        {
            get
            {
                if (udDSPAGCDecay != null) return (int)udDSPAGCDecay.Value;
                else return 0;
            }
            set
            {
                if (udDSPAGCDecay != null) udDSPAGCDecay.Value = value;
            }
        }

        public double IFFreq
        {
            get
            {
                if (udDDSIFFreq != null) return (double)udDDSIFFreq.Value * 1e-6;
                else return 0.0;
            }
            set
            {
                if (udDDSIFFreq != null) udDDSIFFreq.Value = (int)(value * 1e6);
            }
        }

        public bool X2TR
        {
            get
            {
                if (chkGeneralEnableX2 != null) return chkGeneralEnableX2.Checked;
                else return false;
            }
            set
            {
                if (chkGeneralEnableX2 != null) chkGeneralEnableX2.Checked = value;
            }
        }

        public int BreakInDelay
        {
            get
            {
                if (udCWBreakInDelay != null) return (int)udCWBreakInDelay.Value;
                else return -1;
            }
            set
            {
                if (udCWBreakInDelay != null) udCWBreakInDelay.Value = value;
            }
        }

        public int CWPitch
        {
            get
            {
                if (udDSPCWPitch != null) return (int)udDSPCWPitch.Value;
                else return -1;
            }
            set
            {
                if (udDSPCWPitch != null) udDSPCWPitch.Value = value;
            }
        }

        public bool CWDisableMonitor
        {
            get
            {
                if (chkDSPKeyerSidetone != null) return chkDSPKeyerSidetone.Checked;
                else return false;
            }
            set
            {
                if (chkDSPKeyerSidetone != null) chkDSPKeyerSidetone.Checked = value;
            }
        }

        public bool CWIambic
        {
            get
            {
                if (chkCWKeyerIambic != null) return chkCWKeyerIambic.Checked;
                else return false;
            }
            set
            {
                if (chkCWKeyerIambic != null) chkCWKeyerIambic.Checked = value;
            }
        }

        public string VACSampleRate
        {
            get
            {
                if (comboAudioSampleRate2 != null) return comboAudioSampleRate2.Text;
                else return "";
            }
            set
            {
                if (comboAudioSampleRate2 != null) comboAudioSampleRate2.Text = value;
            }
        }

        public string VAC2SampleRate
        {
            get
            {
                if (comboAudioSampleRate3 != null) return comboAudioSampleRate3.Text;
                else return "";
            }
            set
            {
                if (comboAudioSampleRate3 != null) comboAudioSampleRate3.Text = value;
            }
        }

        public string VAC1BufferSize
        {
            get
            {
                if (comboAudioBuffer2 != null) return comboAudioBuffer2.Text;
                else return "";
            }
            set
            {
                if (comboAudioBuffer2 != null) comboAudioBuffer2.Text = value;
            }
        }

        public string VAC2BufferSize
        {
            get
            {
                if (comboAudioBuffer3 != null) return comboAudioBuffer3.Text;
                else return "";
            }
            set
            {
                if (comboAudioBuffer3 != null) comboAudioBuffer3.Text = value;
            }
        }

        public bool IQOutToVAC
        {
            get
            {
                if (chkAudioIQtoVAC != null) return chkAudioIQtoVAC.Checked;
                else return false;
            }
            set
            {
                if (chkAudioIQtoVAC != null) chkAudioIQtoVAC.Checked = value;
            }
        }

        public bool VAC2DirectIQ
        {
            get
            {
                if (chkVAC2DirectIQ != null) return chkVAC2DirectIQ.Checked;
                else return false;
            }
            set
            {
                if (chkVAC2DirectIQ != null) chkVAC2DirectIQ.Checked = value;
            }
        }

        public bool VAC1Calibrate
        {
            get
            {
                if (chkAudioCorrectIQ != null && chkAudioIQtoVAC.Checked) return chkAudioCorrectIQ.Checked;
                else return false;
            }
            set
            {
                if (chkAudioCorrectIQ != null && chkAudioIQtoVAC.Checked) chkAudioCorrectIQ.Checked = value;
            }
        }

        public bool VAC2Calibrate
        {
            get
            {
                if (chkVAC2DirectIQCal != null && chkVAC2DirectIQ.Checked) return chkVAC2DirectIQCal.Checked;
                else return false;
            }
            set
            {
                if (chkVAC2DirectIQCal != null && chkVAC2DirectIQ.Checked) chkVAC2DirectIQCal.Checked = value;
            }
        }

        public bool VACStereo
        {
            get
            {
                if (chkAudio2Stereo != null) return chkAudio2Stereo.Checked;
                else return false;
            }
            set
            {
                if (chkAudio2Stereo != null) chkAudio2Stereo.Checked = value;
            }
        }

        public bool VAC2Stereo
        {
            get
            {
                if (chkAudioStereo3 != null) return chkAudioStereo3.Checked;
                else return false;
            }
            set
            {
                if (chkAudioStereo3 != null) chkAudioStereo3.Checked = value;
            }
        }

        public bool SpurReduction
        {
            get
            {
                if (chkGeneralSpurRed != null) return chkGeneralSpurRed.Checked;
                else return true;
            }
            set
            {
                if (chkGeneralSpurRed != null) chkGeneralSpurRed.Checked = value;
            }
        }

        public int NoiseGate
        {
            get
            {
                if (udTXNoiseGate != null) return (int)udTXNoiseGate.Value;
                else return -1;
            }
            set
            {
                if (udTXNoiseGate != null) udTXNoiseGate.Value = value;
            }
        }

        public int VOXSens
        {
            get
            {
                if (udTXVOXThreshold != null) return (int)udTXVOXThreshold.Value;
                else return -1;
            }
            set
            {
                if (udTXVOXThreshold != null) udTXVOXThreshold.Value = value;
            }
        }

        public bool NoiseGateEnabled
        {
            get
            {
                if (chkTXNoiseGateEnabled != null) return chkTXNoiseGateEnabled.Checked;
                else return false;
            }
            set
            {
                if (chkTXNoiseGateEnabled != null) chkTXNoiseGateEnabled.Checked = value;
            }
        }

        public int VACRXGain
        {
            get
            {
                if (udAudioVACGainRX != null) return (int)udAudioVACGainRX.Value;
                else return -99;
            }
            set
            {
                if (udAudioVACGainRX != null) udAudioVACGainRX.Value = value;
            }
        }

        public int VAC2RXGain
        {
            get
            {
                if (udVAC2GainRX != null) return (int)udVAC2GainRX.Value;
                else return -99;
            }
            set
            {
                if (udVAC2GainRX != null) udVAC2GainRX.Value = value;
            }
        }

        public int VACTXGain
        {
            get
            {
                if (udAudioVACGainTX != null) return (int)udAudioVACGainTX.Value;
                else return -99;
            }
            set
            {
                if (udAudioVACGainTX != null) udAudioVACGainTX.Value = value;
            }
        }

        // ke9ns add .190
        public int QuindarTXGain
        {
            get
            {
                if (udQuindarTonesVol != null) return (int)udQuindarTonesVol.Value;
                else return -99;
            }
            set
            {
                if (udQuindarTonesVol != null) udQuindarTonesVol.Value = value;
            }
        }

        public int VAC2TXGain
        {
            get
            {
                if (udVAC2GainTX != null) return (int)udVAC2GainTX.Value;
                else return -99;
            }
            set
            {
                if (udVAC2GainTX != null) udVAC2GainTX.Value = value;
            }
        }

        public bool BreakInEnabled
        {
            get
            {
                if (chkCWBreakInEnabled != null)
                    return chkCWBreakInEnabled.Checked;
                else return false;
            }
            set
            {
                if (chkCWBreakInEnabled != null)
                    chkCWBreakInEnabled.Checked = value;
            }
        }

        private SoundCard current_sound_card = SoundCard.UNSUPPORTED_CARD;
        public SoundCard CurrentSoundCard
        {
            get { return current_sound_card; }
            set
            {
                current_sound_card = value;
                switch (value)
                {
                    case SoundCard.DELTA_44:
                        comboAudioSoundCard.Text = "M-Audio Delta 44 (PCI)";
                        break;
                    case SoundCard.FIREBOX:
                        comboAudioSoundCard.Text = "PreSonus FireBox (FireWire)";
                        break;
                    case SoundCard.EDIROL_FA_66:
                        comboAudioSoundCard.Text = "Edirol FA-66 (FireWire)";
                        break;
                    case SoundCard.AUDIGY:
                        comboAudioSoundCard.Text = "SB Audigy (PCI)";
                        break;
                    case SoundCard.AUDIGY_2:
                        comboAudioSoundCard.Text = "SB Audigy 2 (PCI)";
                        break;
                    case SoundCard.AUDIGY_2_ZS:
                        comboAudioSoundCard.Text = "SB Audigy 2 ZS (PCI)";
                        break;
                    case SoundCard.EXTIGY:
                        comboAudioSoundCard.Text = "Sound Blaster Extigy (USB)";
                        break;
                    case SoundCard.MP3_PLUS:
                        comboAudioSoundCard.Text = "Sound Blaster MP3+ (USB)";
                        break;
                    case SoundCard.SANTA_CRUZ:
                        comboAudioSoundCard.Text = "Turtle Beach Santa Cruz (PCI)";
                        break;
                    case SoundCard.UNSUPPORTED_CARD:
                        comboAudioSoundCard.Text = "Unsupported Card";
                        break;
                }
            }
        }

        public bool VOXEnable
        {
            get
            {
                if (chkTXVOXEnabled != null) return chkTXVOXEnabled.Checked;
                else return false;
            }
            set
            {
                if (chkTXVOXEnabled != null) chkTXVOXEnabled.Checked = value;
            }
        }

        public int AGCMaxGain
        {
            get
            {
                if (udDSPAGCMaxGaindB != null) return (int)udDSPAGCMaxGaindB.Value;
                else return -1;
            }
            set
            {
                if (udDSPAGCMaxGaindB != null) udDSPAGCMaxGaindB.Value = value;
            }
        }

        public int AGCFixedGain
        {
            get
            {
                if (udDSPAGCFixedGaindB != null)
                {
                    udDSPAGCFixedGaindB_ValueChanged(this, EventArgs.Empty);
                    return (int)udDSPAGCFixedGaindB.Value;
                }
                else return -1;
            }
            set
            {
                if (udDSPAGCFixedGaindB != null) udDSPAGCFixedGaindB.Value = value;
            }
        }

        public int TXFilterHigh
        {
            get { return (int)udTXFilterHigh.Value; }
            set
            {
                if (value > udTXFilterHigh.Maximum) value = (int)udTXFilterHigh.Maximum;
                if (value < udTXFilterHigh.Minimum) value = (int)udTXFilterHigh.Minimum;
                udTXFilterHigh.Value = value;
            }
        }

        public int TXFilterLow
        {
            get { return (int)udTXFilterLow.Value; }
            set
            {
                if (value > udTXFilterLow.Maximum) value = (int)udTXFilterLow.Maximum;
                if (value < udTXFilterLow.Minimum) value = (int)udTXFilterLow.Minimum;
                udTXFilterLow.Value = value;
            }
        }

        public bool Polyphase
        {
            get { return chkSpectrumPolyphase.Checked; }
            set { chkSpectrumPolyphase.Checked = value; }
        }

        public bool CustomRXAGCEnabled
        {
            set
            {
                udDSPAGCAttack.Enabled = value;
                udDSPAGCDecay.Enabled = value;
                udDSPAGCHangTime.Enabled = value;

                if (value)
                {
                    udDSPAGCAttack_ValueChanged(this, EventArgs.Empty);
                    udDSPAGCDecay_ValueChanged(this, EventArgs.Empty);
                    udDSPAGCHangTime_ValueChanged(this, EventArgs.Empty);
                }
            }
        }

        public bool DirectX
        {
            set
            {
                if (value)
                {
                    if (!comboDisplayDriver.Items.Contains("DirectX"))
                        comboDisplayDriver.Items.Add("DirectX");
                }
                else
                {
                    if (comboDisplayDriver.Items.Contains("DirectX"))
                    {
                        comboDisplayDriver.Items.Remove("DirectX");
                        if (comboDisplayDriver.SelectedIndex < 0)
                            comboDisplayDriver.SelectedIndex = 0;
                    }
                }
            }
        }

        public bool VACEnable
        {
            get { return chkAudioEnableVAC.Checked; }
            set { chkAudioEnableVAC.Checked = value; }
        }

        public bool VAC2Enable
        {
            get { return chkVAC2Enable.Checked; }
            set
            {
               
                chkVAC2Enable.Checked = value; 
            }
        }

        public int SoundCardIndex
        {
            get { return comboAudioSoundCard.SelectedIndex; }
            set { comboAudioSoundCard.SelectedIndex = value; }
        }

        private bool force_model = false;
        public Model CurrentModel
        {
            set
            {
                switch (value)
                {
                    case Model.FLEX1500:
                        force_model = true;
                        radGenModelFLEX1500.Checked = true;
                        break;
                    case Model.SDR1000:
                        force_model = true;
                        radGenModelSDR1000.Checked = true;
                        break;
                    case Model.SOFTROCK40:
                        force_model = true;
                        radGenModelSoftRock40.Checked = true;
                        break;
                    case Model.DEMO:
                        force_model = true;
                        radGenModelDemoNone.Checked = true;
                        break;
                }
            }
        }

        public void ResetFLEX5000()
        {
            radGenModelFLEX5000_CheckedChanged(this, EventArgs.Empty);
        }

        public bool RXOnly
        {
            get { return chkGeneralRXOnly.Checked; }
            set
            {
                chkGeneralRXOnly.Checked = value;
            }
        }

        private bool mox;
        public bool MOX
        {
            get { return mox; }
            set
            {
                mox = value;
                grpGeneralHardwareSDR1000.Enabled = !mox;
                if (comboAudioSoundCard.SelectedIndex == (int)SoundCard.UNSUPPORTED_CARD)
                    grpAudioDetails1.Enabled = !mox;
                grpAudioCard.Enabled = !mox;
                grpAudioBufferSize1.Enabled = !mox;
                grpAudioVolts1.Enabled = !mox;
                grpAudioLatency1.Enabled = !mox;
                chkAudioEnableVAC.Enabled = !mox;

                if (chkAudioEnableVAC.Checked)
                {
                    grpAudioDetails2.Enabled = !mox;
                    grpAudioBuffer2.Enabled = !mox;
                    grpAudioLatency2.Enabled = !mox;
                    grpAudioSampleRate2.Enabled = !mox;
                    grpAudio2Stereo.Enabled = !mox;
                }
                else
                {
                    grpAudioDetails2.Enabled = true;
                    grpAudioBuffer2.Enabled = true;
                    grpAudioLatency2.Enabled = true;
                    grpAudioSampleRate2.Enabled = true;
                    grpAudio2Stereo.Enabled = true;
                }
                grpDSPBufferSize.Enabled = !mox;
                grpTestAudioBalance.Enabled = !mox;
                if (!mox && !chekTestIMD.Checked && !chkGeneralRXOnly.Checked)
                    grpTestTXIMD.Enabled = !mox;
            }
        }

        public int TXAF
        {
            get { return (int)udTXAF.Value; }
            set { udTXAF.Value = value; }
        }

        public int AudioReceiveMux1
        {
            get { return comboAudioReceive1.SelectedIndex; }
            set
            {
                comboAudioReceive1.SelectedIndex = value;
                comboAudioReceive1_SelectedIndexChanged(this, EventArgs.Empty);
            }
        }

        public bool USBPresent
        {
            get { return chkGeneralUSBPresent.Checked; }
            set { chkGeneralUSBPresent.Checked = value; }
        }

        public bool XVTRPresent
        {
            get { return chkGeneralXVTRPresent.Checked; }
            set { chkGeneralXVTRPresent.Checked = value; }
        }

        public int XVTRSelection
        {
            get { return comboGeneralXVTR.SelectedIndex; }
            set { comboGeneralXVTR.SelectedIndex = value; }
        }

        public bool PAPresent
        {
            get { return chkGeneralPAPresent.Checked; }
            set { chkGeneralPAPresent.Checked = value; }
        }

        public bool ATUPresent
        {
            get { return chkGeneralATUPresent.Checked; }
            set { chkGeneralATUPresent.Checked = value; }
        }

        public bool SpurRedEnabled
        {
            get { return chkGeneralSpurRed.Enabled; }
            set { chkGeneralSpurRed.Enabled = value; }
        }

        public int PllMult
        {
            get { return (int)udDDSPLLMult.Value; }
            set { udDDSPLLMult.Value = value; }
        }

        public int ClockOffset
        {
            get { return (int)udDDSCorrection.Value; }
            set { udDDSCorrection.Value = value; }
        }

        public float ImageGainTX
        {
            get { return (float)udDSPImageGainTX.Value; }
            set
            {
                try
                {
                    udDSPImageGainTX.Value = (decimal)value;
                }
                catch (Exception)
                {
                    MessageBox.Show(new Form { TopMost = true }, "Error setting TX Image Gain (" + value.ToString("f2") + ")");
                }
            }
        }

        public float ImagePhaseTX
        {
            get { return (float)udDSPImagePhaseTX.Value; }
            set
            {
                try
                {
                    udDSPImagePhaseTX.Value = (decimal)value;
                }
                catch (Exception)
                {
                    MessageBox.Show(new Form { TopMost = true }, "Error setting TX Image Phase (" + value.ToString("f2") + ")");
                }
            }
        }

        public float PAGain160
        {
            get { return (float)udPAGain160.Value; }
            set { udPAGain160.Value = (decimal)value; }
        }

        public float PAGain80
        {
            get { return (float)udPAGain80.Value; }
            set { udPAGain80.Value = (decimal)value; }
        }

        public float PAGain60
        {
            get { return (float)udPAGain60.Value; }
            set { udPAGain60.Value = (decimal)value; }
        }

        public float PAGain40
        {
            get { return (float)udPAGain40.Value; }
            set { udPAGain40.Value = (decimal)value; }
        }

        public float PAGain30
        {
            get { return (float)udPAGain30.Value; }
            set { udPAGain30.Value = (decimal)value; }
        }

        public float PAGain20
        {
            get { return (float)udPAGain20.Value; }
            set { udPAGain20.Value = (decimal)value; }
        }

        public float PAGain17
        {
            get { return (float)udPAGain17.Value; }
            set { udPAGain17.Value = (decimal)value; }
        }

        public float PAGain15
        {
            get { return (float)udPAGain15.Value; }
            set { udPAGain15.Value = (decimal)value; }
        }

        public float PAGain12
        {
            get { return (float)udPAGain12.Value; }
            set { udPAGain12.Value = (decimal)value; }
        }

        public float PAGain10
        {
            get { return (float)udPAGain10.Value; }
            set { udPAGain10.Value = (decimal)value; }
        }

        public float PAADC160
        {
            get { return (float)udPAADC160.Value; }
            set { udPAADC160.Value = (decimal)value; }
        }

        public float PAADC80
        {
            get { return (float)udPAADC80.Value; }
            set { udPAADC80.Value = (decimal)value; }
        }

        public float PAADC60
        {
            get { return (float)udPAADC60.Value; }
            set { udPAADC60.Value = (decimal)value; }
        }

        public float PAADC40
        {
            get { return (float)udPAADC40.Value; }
            set { udPAADC40.Value = (decimal)value; }
        }

        public float PAADC30
        {
            get { return (float)udPAADC30.Value; }
            set { udPAADC30.Value = (decimal)value; }
        }

        public float PAADC20
        {
            get { return (float)udPAADC20.Value; }
            set { udPAADC20.Value = (decimal)value; }
        }

        public float PAADC17
        {
            get { return (float)udPAADC17.Value; }
            set { udPAADC17.Value = (decimal)value; }
        }

        public float PAADC15
        {
            get { return (float)udPAADC15.Value; }
            set { udPAADC15.Value = (decimal)value; }
        }

        public float PAADC12
        {
            get { return (float)udPAADC12.Value; }
            set { udPAADC12.Value = (decimal)value; }
        }

        public float PAADC10
        {
            get { return (float)udPAADC10.Value; }
            set { udPAADC10.Value = (decimal)value; }
        }

        public int TunePower
        {
            get { return (int)udTXTunePower.Value; }
            set
            {

                udTXTunePower.Value = (decimal)value;

                if (udTXDriveMax.Value < udTXTunePower.Value) udTXTunePower.Value = udTXDriveMax.Value;

            }
        }

        public bool DigUIsUSB
        {
            get { return chkDigUIsUSB.Checked; }
        }

        public int DigU_CT_Offset
        {
            get { return (int)udOptClickTuneOffsetDIGU.Value; }
            set { udOptClickTuneOffsetDIGU.Value = value; }
        }

        public int DigL_CT_Offset
        {
            get { return (int)udOptClickTuneOffsetDIGL.Value; }
            set { udOptClickTuneOffsetDIGL.Value = value; }
        }

        public float WaterfallLowThreshold
        {
            get { return (float)udDisplayWaterfallLowLevel.Value; }
            set { udDisplayWaterfallLowLevel.Value = (decimal)value; }
        }

        //==================================================================================
        public float WaterfallLowRX2Threshold      // ke9ns ADD RX2
        {
            get { return (float)udDisplayWaterfallRX2Level.Value; }
            set { udDisplayWaterfallRX2Level.Value = (decimal)value; }
        }
        public float WaterfallHighThreshold
        {
            get { return (float)udDisplayWaterfallHighLevel.Value; }
            set { udDisplayWaterfallHighLevel.Value = (decimal)value; }
        }

        public bool WeakSignalWaterfallEnabled
        {
            get { return chkWeakSignalWaterfallSettings.Checked; }
            set { chkWeakSignalWaterfallSettings.Checked = value; }
        }


        // Added 06/21/05 BT for CAT commands
        public int CATNB1Threshold
        {
            get { return Convert.ToInt32(udDSPNB.Value); }
            set
            {
                value = (int)Math.Max(udDSPNB.Minimum, value);          // lower bound
                value = (int)Math.Min(udDSPNB.Maximum, value);          // upper bound
                udDSPNB.Value = value;
            }
        }

        // Added 06/21/05 BT for CAT commands
        public int CATNB2Threshold
        {
            get { return Convert.ToInt32(udDSPNB2.Value); }
            set
            {
                value = (int)Math.Max(udDSPNB2.Minimum, value);
                value = (int)Math.Min(udDSPNB2.Maximum, value);
                udDSPNB2.Value = value;
            }
        }

        // Added 06/21/05 BT for CAT commands
        /*public int CATCompThreshold
		{
			get{return Convert.ToInt32(udTXFFCompression.Value);}
			set
			{
				value = (int)Math.Max(udTXFFCompression.Minimum, value);
				value = (int)Math.Min(udTXFFCompression.Maximum, value);
				udTXFFCompression.Value = value;
			}
		}*/

        // Added 06/30/05 BT for CAT commands
        public int CATCWPitch
        {
            get { return (int)udDSPCWPitch.Value; }
            set
            {
                value = (int)Math.Max(udDSPCWPitch.Minimum, value);
                value = (int)Math.Min(udDSPCWPitch.Maximum, value);
                udDSPCWPitch.Value = value;
            }
        }

        // Added 07/07/05 BT for CAT commands
        public void CATSetRig(string rig)
        {
            comboCATRigType.Text = rig;
        }

        // Added 06/30/05 BT for CAT commands
        //		public int CATTXPreGain
        //		{
        //			get{return (int) udTXPreGain.Value;}
        //			set
        //			{
        //				value = Math.Max(-30, value);
        //				value = Math.Min(70, value);
        //				udTXPreGain.Value = value;
        //			}
        //		}

        //Reads or sets the setup form Spectrum Grid Display Maximum value.
        public int CATSGMax
        {
            get { return (int)udDisplayGridMax.Value; }
            set
            {
                value = (int)Math.Max(udDisplayGridMax.Minimum, value);
                value = (int)Math.Min(udDisplayGridMin.Maximum, value);
                udDisplayGridMax.Value = value;
            }
        }

        //Reads or sets the setup form Spectrum Grid Display Minimum value.
        public int CATSGMin
        {
            get { return (int)udDisplayGridMin.Value; }
            set
            {
                value = (int)Math.Max(udDisplayGridMax.Minimum, value);
                value = (int)Math.Min(udDisplayGridMin.Maximum, value);
                udDisplayGridMin.Value = value;
            }
        }

        //Reads or sets the setup form Spectrum Grid Display Step value.
        public int CATSGStep
        {
            get { return (int)udDisplayGridStep.Value; }
            set
            {
                value = (int)Math.Max(udDisplayGridStep.Minimum, value);
                value = (int)Math.Min(udDisplayGridStep.Maximum, value);
                udDisplayGridStep.Value = value;
            }
        }

        //Reads or sets the setup form Waterfall low level value.
        public int CATWFLo
        {
            get { return (int)udDisplayWaterfallLowLevel.Value; }
            set
            {
                value = (int)Math.Max(udDisplayWaterfallLowLevel.Minimum, value);
                value = (int)Math.Min(udDisplayWaterfallLowLevel.Maximum, value);
                udDisplayWaterfallLowLevel.Value = value;
            }
        }

        //Reads or sets the setup form Waterfall high level value.
        public int CATWFHi
        {
            get { return (int)udDisplayWaterfallHighLevel.Value; }
            set
            {
                value = (int)Math.Max(udDisplayWaterfallHighLevel.Minimum, value);
                value = (int)Math.Min(udDisplayWaterfallHighLevel.Maximum, value);
                udDisplayWaterfallHighLevel.Value = value;
            }
        }

        public int DSPPhoneRXBuffer
        {
            get { return Int32.Parse(comboDSPPhoneRXBuf.Text); }
            set
            {
                string temp = value.ToString();
                if (comboDSPPhoneRXBuf.Items.Contains(temp))
                    comboDSPPhoneRXBuf.SelectedItem = temp;
            }
        }

        public int DSPPhoneTXBuffer
        {
            get { return Int32.Parse(comboDSPPhoneTXBuf.Text); }
            set
            {
                string temp = value.ToString();
                if (comboDSPPhoneTXBuf.Items.Contains(temp))
                    comboDSPPhoneTXBuf.SelectedItem = temp;
            }
        }

        public int DSPCWRXBuffer
        {
            get { return Int32.Parse(comboDSPCWRXBuf.Text); }
            set
            {
                string temp = value.ToString();
                if (comboDSPCWRXBuf.Items.Contains(temp))
                    comboDSPCWRXBuf.SelectedItem = temp;
            }
        }

        public int DSPCWTXBuffer
        {
            get { return Int32.Parse(comboDSPCWTXBuf.Text); }
            set
            {
                string temp = value.ToString();
                if (comboDSPCWTXBuf.Items.Contains(temp))
                    comboDSPCWTXBuf.SelectedItem = temp;
            }
        }

        public int DSPDigRXBuffer
        {
            get { return Int32.Parse(comboDSPDigRXBuf.Text); }
            set
            {
                string temp = value.ToString();
                if (comboDSPDigRXBuf.Items.Contains(temp))
                    comboDSPDigRXBuf.SelectedItem = temp;
            }
        }

        public int DSPDigTXBuffer
        {
            get { return Int32.Parse(comboDSPDigTXBuf.Text); }
            set
            {
                string temp = value.ToString();
                if (comboDSPDigTXBuf.Items.Contains(temp))
                    comboDSPDigTXBuf.SelectedItem = temp;
            }
        }

        public int AudioBufferSize
        {
            get { return Int32.Parse(comboAudioBuffer1.Text); }
            set
            {
                string temp = value.ToString();
                if (comboAudioBuffer1.Items.Contains(temp))
                    comboAudioBuffer1.SelectedItem = temp;
            }
        }

        private bool flex_profiler_installed = false;
        public bool FlexProfilerInstalled
        {
            get { return flex_profiler_installed; }
            set { flex_profiler_installed = value; }
        }

        private bool allow_freq_broadcast = false;
        private bool allow_freq_broadcast2 = false; // .214
        private bool allow_freq_broadcast3 = false;
        private bool allow_freq_broadcast4 = false;
        private bool allow_freq_broadcast5 = false;
        private bool allow_freq_broadcast6 = false;
        private bool allow_freq_broadcast7 = false; // for TCP/IP CAT
        public bool AllowFreqBroadcast
        {
            get { return allow_freq_broadcast; }
            set
            {
                allow_freq_broadcast = value;
                if (value)
                {
                    console.KWAutoInformation = true;
                   
                }
                else
                {
                    console.KWAutoInformation = false;
                   
                }
            }
        }

        public bool AllowFreqBroadcast2  // ke9ns add .214
        {
            get { return allow_freq_broadcast2; }
            set
            {
                allow_freq_broadcast2 = value;
                if (value)
                {
                    console.KWAutoInformation2 = true;

                }
                else
                {
                    console.KWAutoInformation2 = false;

                }
            }
        } // AllowFreqBroadcast2  

        public bool AllowFreqBroadcast3  // ke9ns add .214
        {
            get { return allow_freq_broadcast3; }
            set
            {
                allow_freq_broadcast3 = value;
                if (value)
                {
                    console.KWAutoInformation3 = true;

                }
                else
                {
                    console.KWAutoInformation3 = false;

                }
            }
        } // AllowFreqBroadcast2  

        public bool AllowFreqBroadcast4  // ke9ns add .214
        {
            get { return allow_freq_broadcast4; }
            set
            {
                allow_freq_broadcast4 = value;
                if (value)
                {
                    console.KWAutoInformation4 = true;

                }
                else
                {
                    console.KWAutoInformation4 = false;

                }
            }
        } // AllowFreqBroadcast4  


        public bool AllowFreqBroadcast5  // ke9ns add .214
        {
            get { return allow_freq_broadcast5; }
            set
            {
                allow_freq_broadcast5 = value;
                if (value)
                {
                    console.KWAutoInformation5 = true;

                }
                else
                {
                    console.KWAutoInformation5 = false;

                }
            }
        } // AllowFreqBroadcast5  


        public bool AllowFreqBroadcast6  // ke9ns add .214
        {
            get { return allow_freq_broadcast6; }
            set
            {
                allow_freq_broadcast6 = value;
                if (value)
                {
                    console.KWAutoInformation6 = true;

                }
                else
                {
                    console.KWAutoInformation6 = false;

                }
            }
        } // AllowFreqBroadcast6  

        public bool AllowFreqBroadcast7  // ke9ns add .214
        {
            get { return allow_freq_broadcast7; }
            set
            {
                allow_freq_broadcast7 = value;
                if (value)
                {
                    console.KWAutoInformation7 = true;

                }
                else
                {
                    console.KWAutoInformation7 = false;

                }
            }
        } // AllowFreqBroadcast7  

        public bool AutoMuteRX2onVFOATX
        {
            get { return chkRX2AutoMuteRX2OnVFOATX.Checked; }
            set { chkRX2AutoMuteRX2OnVFOATX.Checked = value; }
        }

        public bool AutoMuteRX1onVFOBTX
        {
            get { return chkRX2AutoMuteRX1OnVFOBTX.Checked; }
            set { chkRX2AutoMuteRX1OnVFOBTX.Checked = value; }
        }

        private bool rtty_offset_enabled_a;
        public bool RttyOffsetEnabledA
        {
            get { return rtty_offset_enabled_a; }
            set { chkRTTYOffsetEnableA.Checked = value; }
        }

        private bool rtty_offset_enabled_b;
        public bool RttyOffsetEnabledB
        {
            get { return rtty_offset_enabled_b; }
            set { chkRTTYOffsetEnableB.Checked = value; }
        }

        private int rtty_offset_high = 2125;
        public int RttyOffsetHigh
        {
            get { return rtty_offset_high; }
            set
            {
                value = (int)Math.Max(udRTTYU.Minimum, value);
                value = (int)Math.Min(udRTTYU.Maximum, value);
                udRTTYU.Value = value;
            }
        }

        private int rtty_offset_low = 2125;
        public int RttyOffsetLow
        {
            get { return rtty_offset_low; }
            set
            {
                value = (int)Math.Max(udRTTYL.Minimum, value);
                value = (int)Math.Min(udRTTYL.Maximum, value);
                udRTTYL.Value = value;
            }
        }

        #endregion

        #region General Tab Event Handlers
        // ======================================================
        // General Tab Event Handlers
        // ======================================================

        private void radGenModelFLEX5000_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radGenModelFLEX5000.Checked)
            {
                if (!console.fwc_init)
                {
                    console.fwc_init = Pal.Init();
                    if (console.fwc_init)
                    {
                        FWCEEPROM.Init();
                        console.CurrentRegion = FWCEEPROM.Region;
                        FWC.SetPalCallback();
                    }
                }
                if (console.fwc_init)
                {
                    switch (FWCEEPROM.Model)
                    {
                        case 0:
                        case 1:
                        case 2:
                            console.CurrentModel = Model.FLEX5000;
                            radGenModelFLEX5000.Text = "FLEX-5000";
                            grpGeneralHardwareFLEX5000.Text = "FLEX-5000 Config";
                            lblRFIORev.Visible = true;
                            break;
                        case 3:
                            console.CurrentModel = Model.FLEX3000;
                            radGenModelFLEX5000.Text = "FLEX-3000";
                            grpGeneralHardwareFLEX5000.Text = "FLEX-3000 Config";
                            lblRFIORev.Visible = false;
                            chkGenFLEX5000ExtRef.Visible = false;
                            udTXFilterHigh.Maximum = 10000;     // ke9ns mod was 4500;

                            //   if (comboAudioSampleRate1.Items.Contains(192000))     comboAudioSampleRate1.Items.Remove(192000);

                            if (comboAudioSampleRate1.SelectedIndex == -1) comboAudioSampleRate1.SelectedIndex = 1;

                            lblF3KFanTempThresh.Visible = true;
                            udF3KFanTempThresh.Visible = true;
                            chkGenTX1Delay.Visible = true;
                            lblGenTX1Delay.Visible = true;
                            udGenTX1Delay.Visible = true;
                            chkSigGenRX2.Visible = false;
                            lblModel.Visible = false;
                            lblSerialNum.Left = 16;
                            chkGenOptionsShowATUPopup.Visible = true;
                            chkRX2AutoMuteRX2OnVFOATX.Visible = false;
                            chkRX2AutoMuteRX1OnVFOBTX.Visible = false;
                            chkRX2DisconnectOnTX.Visible = false;

                            chkRX2AutoOn.Visible = false; // .231
                            chkRX2AutoVAC2.Visible = false; // .231
                            richTextBox1.Visible = false; // .231
                            richTextBox2.Visible = false; // .231

                            break;
                    }
                }
                comboAudioSoundCard.Text = "Unsupported Card";
                comboAudioSampleRate1.Visible = true;
                //comboAudioSampleRate1.Text = "96000";

                foreach (PADeviceInfo p in comboAudioDriver1.Items)
                {
                    if (p.Name == "ASIO")
                    {
                        comboAudioDriver1.SelectedItem = p;
                        break;
                    }
                }

                foreach (PADeviceInfo dev in comboAudioInput1.Items)
                {
                    if (dev.Name.IndexOf("FlexRadio") >= 0)
                    {
                        comboAudioInput1.Text = dev.Name;
                        comboAudioOutput1.Text = dev.Name;
                        break;
                    }
                }

                //if (comboAudioBuffer1.Items.Contains("256"))
                //    comboAudioBuffer1.Items.Remove("256");

                udAudioVoltage1.Value = 1.0M;

                comboAudioMixer1.Text = "None";

                if (comboAudioInput1.Text.IndexOf("FlexRadio") < 0)
                {
                    /*MessageBox.Show(new Form { TopMost = true }, "FLEX-5000 hardware not found.  Please check " +
                        "the following:\n" +
                        "\t1. Verify that the unit has power and is running (note blue LED).\n" +
                        "\t2. Verify FireWire cable is securely plugged in on both ends.\n" +
                        "\t3. Verify that the driver is installed properly and the device shows up as FLEX 5000 in the device manager.\n" +
                        "Note that after correcting any of these issues, you must restart PowerSDR for the changes to take effect.\n" +
                        "For more support, see our website at www.flexradio.com or email support@flexradio.com.",
                        "Hardware Not Found",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);*/
                    console.PowerEnabled = false;
                }
                else
                {
                    bool trx_ok, pa_ok, rfio_ok, rx2_ok, atu_ok;
                    trx_ok = FWCEEPROM.TRXOK;
                    pa_ok = FWCEEPROM.PAOK;
                    rfio_ok = FWCEEPROM.RFIOOK;
                    rx2_ok = FWCEEPROM.RX2OK;

                    chkSigGenRX2.Visible = rx2_ok;

                    /* chkVAC2UseRX2.Visible = rx2_ok; W4TME */

                    if (!rx2_ok)
                    {
                        chkRX2AutoMuteRX2OnVFOATX.Enabled = false;
                        chkRX2AutoMuteRX1OnVFOBTX.Enabled = false;
                        chkRX2DisconnectOnTX.Enabled = false;
                    }

                    if (dax_audio_setup_enum)
                    {
                        chkVAC2Enable.Enabled = true; // rx2_ok;   //ke9ns  this turns on the enable checkbox
                        chkVAC2AutoEnable.Enabled = true; // rx2_ok;  //
                    }

                    if (console.CurrentModel == Model.FLEX5000)
                    {
                        FWC.GetATUOK(out atu_ok);
                    }
                    else
                    {
                        atu_ok = true;
                    }

                    switch (FWCEEPROM.Model)
                    {
                        case 0: lblModel.Text = "Model: A"; break;
                        case 1: lblModel.Text = "Model: C"; break;
                        case 2: lblModel.Text = "Model: D"; break;
                    }

                    lblSerialNum.Text = "S/N: " + FWCEEPROM.SerialToString(FWCEEPROM.SerialNumber);

                    Assembly assembly = Assembly.GetExecutingAssembly();
                    FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);

                    string y = "";

                    if (console.extended == true) y = "_E";

                    lblversion.Text = "v" + fvi.FileVersion.ToString() + "_" + console.CurrentRegion + y; // ke9ns add: display the software version v2.8.0.xxx




                    uint val;
                    Thread.Sleep(10);
                    FWC.GetFirmwareRev(out val);
                    string s = "Firmware: " + Common.RevToString(val);
                    lblFirmwareRev.Text = s;

                    s = "TRX: " + FWCEEPROM.SerialToString(FWCEEPROM.TRXSerial);
                    val = FWCEEPROM.TRXRev;
                    s += "  (" + ((byte)(val >> 0)).ToString();
                    s += ((char)(((byte)(val >> 8)) + 65)).ToString() + ")";
                    lblTRXRev.Text = s;
                    if (!trx_ok) lblTRXRev.ForeColor = Color.Red;

                    s = "PA: " + FWCEEPROM.SerialToString(FWCEEPROM.PASerial);
                    val = FWCEEPROM.PARev;
                    s += "  (" + ((byte)(val >> 0)).ToString();
                    s += ((char)(((byte)(val >> 8)) + 65)).ToString() + ")";
                    lblPARev.Text = s;
                    if (!pa_ok) lblPARev.ForeColor = Color.Red;

                    s = "RFIO: " + FWCEEPROM.SerialToString(FWCEEPROM.RFIOSerial);
                    val = FWCEEPROM.RFIORev;
                    s += "  (" + ((byte)(val >> 0)).ToString();
                    s += ((char)(((byte)(val >> 8)) + 65)).ToString() + ")";
                    lblRFIORev.Text = s;
                    if (!rfio_ok) lblRFIORev.ForeColor = Color.Red;

                    if (!atu_ok) lblATURev.Visible = false;
                    else
                    {
                        if (FWCEEPROM.ATURev > 0 && FWCEEPROM.ATURev < 0xFFFFFFFF)
                        {
                            if (FWCEEPROM.ATUSerial > 0 && FWCEEPROM.ATUSerial < 0xFFFFFFFF)
                                s = "ATU: " + FWCEEPROM.SerialToString(FWCEEPROM.ATUSerial);
                            else
                                s = "ATU: Present";
                            val = FWCEEPROM.ATURev;
                            s += "  (" + ((byte)(val >> 0)).ToString();
                            s += ((char)(((byte)(val >> 8)) + 65)).ToString() + ")";
                            lblATURev.Text = s;
                        }
                        else
                        {
                            lblATURev.Text = "ATU: Present";
                        }
                    }

                    if (!FWCEEPROM.VUOK) lblVURev.Visible = false;
                    else
                    {
                        s = "VU: " + FWCEEPROM.SerialToString(FWCEEPROM.VUSerial);
                        val = FWCEEPROM.VURev;
                        s += "  (" + ((byte)(val >> 0)).ToString();
                        s += ((char)(((byte)(val >> 8)) + 65)).ToString() + ")";
                        lblVURev.Text = s;
                    }

                    if (console.CurrentModel == Model.FLEX5000)
                    {
                        s = "RX2: " + FWCEEPROM.SerialToString(FWCEEPROM.RX2Serial);
                        val = FWCEEPROM.RX2Rev;
                        s += "  (" + ((byte)(val >> 0)).ToString();
                        s += ((char)(((byte)(val >> 8)) + 65)).ToString() + ")";
                    }
                    else s = "RX2: ";
                    lblRX2Rev.Text = s;
                    if (!rx2_ok) lblRX2Rev.Visible = false;


                    /*if(rx2_ok) 
                    {
                        console.dsp.GetDSPRX(1, 0).Active = true;
                        DttSP.SetThreadProcessingMode(2, 2);
                        DSP.SetThreadNumber(3);
                        Audio.RX2Enabled = true;
                    }
                    else*/

                    DSP.SetThreadNumber(2);

                    string key = comboKeyerConnPrimary.Text;
                    if (comboKeyerConnPrimary.Items.Contains("SDR"))
                        comboKeyerConnPrimary.Items.Remove("SDR");
                    if (!comboKeyerConnPrimary.Items.Contains("Radio"))
                        comboKeyerConnPrimary.Items.Insert(0, "Radio");
                    if (key == "SDR" || key == "5000") comboKeyerConnPrimary.Text = "Radio";
                    else comboKeyerConnPrimary.Text = key;
                    comboKeyerConnPrimary_SelectedIndexChanged(this, EventArgs.Empty);
                    chkPANewCal.Checked = true;

                    comboAudioBuffer1_SelectedIndexChanged(this, EventArgs.Empty);
                }
            }
            else
            {
                console.PowerEnabled = true;
                chkVAC2UseRX2.Visible = false;
                chkVAC2UseRX2.Checked = false;
            }

            bool b = radGenModelFLEX5000.Checked;

            if (radGenModelFLEX5000.Focused || force_model && b)
            {

                chkGeneralRXOnly.Checked = false;
                chkGeneralDisablePTT.Checked = false;
                force_model = false;
            }

            radPACalAllBands_CheckedChanged(this, EventArgs.Empty);
            grpGeneralHardwareSDR1000.Visible = false;
            grpGeneralHardwareFLEX5000.Visible = true;
            grpGeneralHardwareFLEX1500.Visible = false;
            // btnWizard.Visible = !b;
            grpGenAutoMute.Visible = !b;

            grpAudioDetails1.Visible = !b;
            grpAudioCard.Visible = !b;
            grpAudioLineInGain1.Visible = !b;
            grpAudioMicInGain1.Visible = !b;
            grpAudioChannels.Visible = !b;
            grpAudioVolts1.Visible = !b;

            chkGeneralSoftwareGainCorr.Visible = !b;
            chkGeneralEnableX2.Visible = !b;
            lblGeneralX2Delay.Visible = !b;
            udGeneralX2Delay.Visible = !b;
            chkGeneralCustomFilter.Visible = !b;

            grpGenCalLevel.Visible = !b;
            grpGenCalRXImage.Visible = !b;
            chkCalExpert.Visible = !b;

            //chkCalExpert.Visible = b;

            /* chkCalExpert_CheckedChanged(this, EventArgs.Empty);
            if (!b)
            {
                grpGeneralCalibration.Visible = true;
                grpGenCalLevel.Visible = true;
                grpGenCalRXImage.Visible = true;
            } */
            chkDSPImageExpert.Visible = b;
            chkDSPImageExpert_CheckedChanged(this, EventArgs.Empty);
            if (!b)
            {
                grpDSPImageRejectTX.Visible = true;
            }

            grpPAGainByBand.Visible = (!b && chkGeneralPAPresent.Checked);
            chkPANewCal.Visible = (!b && chkGeneralPAPresent.Checked);

            rtxtPACalReq.Visible = !b;

            if (b || radGenModelFLEX1500.Checked)
            {
                if (tcSetup.TabPages.Contains(tpExtCtrl))
                {
                    tcSetup.TabPages.Remove(tpExtCtrl);
                    tcSetup.SelectedIndex = 0;
                }
            }
            else
            {
                if (!tcSetup.TabPages.Contains(tpExtCtrl))
                    Common.TabControlInsert(tcSetup, tpExtCtrl, 6);
            }
            grpImpulseTest.Visible = !b;
            ckEnableSigGen.Visible = b;
            grpTestX2.Visible = !b;
        }

        private void radGenModelFLEX1500_CheckedChanged(object sender, EventArgs e)
        {
            if (radGenModelFLEX1500.Checked)
            {
                if (!console.hid_init)
                {
                    int count = 0;
                    try
                    {
                        Flex1500.Init();
                    }
                    catch (Exception)
                    {

                    }

                    while (!(console.hid_init = Flex1500.IsRadioPresent()))
                    {
                        if (count++ > 15)
                            break;
                        Thread.Sleep(1000);
                        Application.DoEvents();
                    }
                    if (console.hid_init)
                    {
                        HIDEEPROM.Init();
                        console.CurrentRegion = HIDEEPROM.Region;
                    }
                }

                if (console.hid_init)
                {
                    console.CurrentModel = Model.FLEX1500;

                    if (!console.TestEquip)
                    {
                        udTXFilterHigh.Maximum = 10000; //  ke9ns mod was 3650;
                    }
                    else
                    {
                        udTXFilterHigh.Maximum = 5000;
                    }
                    if (comboAudioSampleRate1.Items.Contains(192000))
                        comboAudioSampleRate1.Items.Remove(192000);
                    if (comboAudioSampleRate1.Items.Contains(96000))
                        comboAudioSampleRate1.Items.Remove(96000);

                    comboAudioSampleRate1.SelectedIndex = 0;

                    if (comboDSPPhoneRXBuf.Items.Contains("4096"))
                        comboDSPPhoneRXBuf.Items.Remove("4096");
                    if (comboDSPPhoneRXBuf.SelectedIndex < 0 || comboDSPPhoneRXBuf.SelectedIndex >= comboDSPPhoneRXBuf.Items.Count)
                        comboDSPPhoneRXBuf.SelectedIndex = comboDSPPhoneRXBuf.Items.Count - 1;

                    if (comboDSPPhoneTXBuf.Items.Contains("4096"))
                        comboDSPPhoneTXBuf.Items.Remove("4096");
                    if (comboDSPPhoneTXBuf.SelectedIndex < 0 || comboDSPPhoneTXBuf.SelectedIndex >= comboDSPPhoneTXBuf.Items.Count)
                        comboDSPPhoneTXBuf.SelectedIndex = comboDSPPhoneTXBuf.Items.Count - 1;

                    if (comboDSPCWRXBuf.Items.Contains("4096"))
                        comboDSPCWRXBuf.Items.Remove("4096");
                    if (comboDSPCWRXBuf.SelectedIndex < 0 || comboDSPCWRXBuf.SelectedIndex >= comboDSPCWRXBuf.Items.Count)
                        comboDSPCWRXBuf.SelectedIndex = comboDSPCWRXBuf.Items.Count - 1;

                    if (comboDSPCWTXBuf.Items.Contains("4096"))
                        comboDSPCWTXBuf.Items.Remove("4096");
                    if (comboDSPCWTXBuf.SelectedIndex < 0 || comboDSPCWTXBuf.SelectedIndex >= comboDSPCWTXBuf.Items.Count)
                        comboDSPCWTXBuf.SelectedIndex = comboDSPCWTXBuf.Items.Count - 1;

                    if (comboDSPDigRXBuf.Items.Contains("4096"))
                        comboDSPDigRXBuf.Items.Remove("4096");
                    if (comboDSPDigRXBuf.SelectedIndex < 0 || comboDSPDigRXBuf.SelectedIndex >= comboDSPDigRXBuf.Items.Count)
                        comboDSPDigRXBuf.SelectedIndex = comboDSPDigRXBuf.Items.Count - 1;

                    if (comboDSPDigTXBuf.Items.Contains("4096"))
                        comboDSPDigTXBuf.Items.Remove("4096");
                    if (comboDSPDigTXBuf.SelectedIndex < 0 || comboDSPDigTXBuf.SelectedIndex >= comboDSPDigTXBuf.Items.Count)
                        comboDSPDigTXBuf.SelectedIndex = comboDSPDigTXBuf.Items.Count - 1;

                    chkGenTX1Delay.Visible = false;
                    //chkGenTX1Delay_CheckedChanged(this, EventArgs.Empty);
                    lblGenTX1Delay.Visible = false;
                    udGenTX1Delay.Visible = false;
                    chkSigGenRX2.Visible = false;
                    lblModel.Visible = false;
                    grpTXVOX.Visible = false;

                    chkBoxTNTX3.Visible = false;    // ke9ns add
                    chkBoxTNTX3.Checked = false; // ke9ns add

                    chkTXLimitSlew.Visible = false;
                    chkTXLimitSlew.Checked = false;
                    chkRX2AutoMuteRX2OnVFOATX.Visible = false;
                    chkRX2AutoMuteRX1OnVFOBTX.Visible = false;
                    chkRX2DisconnectOnTX.Visible = false;

                    chkRX2AutoOn.Visible = false; // .231
                    chkRX2AutoVAC2.Visible = false; // .231
                    richTextBox1.Visible = false; // .231
                    richTextBox2.Visible = false; // .231


                    //comboAudioSoundCard.Text = "Unsupported Card";
                    comboAudioSampleRate1.Text = "48000";
                    comboAudioSampleRate1.Visible = false;

                    /*foreach (PADeviceInfo p in comboAudioDriver1.Items)
                    {
                        if (p.Name == "MME")
                        {
                            comboAudioDriver1.SelectedItem = p;
                            break;
                        }
                    }

                    foreach (PADeviceInfo dev in comboAudioInput1.Items)
                    {
                        if (dev.Name.IndexOf("FLEX-1500") >= 0)
                        {
                            comboAudioInput1.Text = dev.Name;
                            break;
                        }
                    }

                    foreach (PADeviceInfo dev in comboAudioOutput1.Items)
                    {
                        if (dev.Name.IndexOf("FLEX-1500") >= 0)
                        {
                            comboAudioOutput1.Text = dev.Name;
                            break;
                        }
                    }*/

                    udAudioVoltage1.Value = 1.0M;

                    comboAudioMixer1.Text = "None";

                    // setup buffers - if a new selection
                    if (radGenModelFLEX1500.Focused || force_model)
                    {
                        comboAudioBuffer1.Text = "1024";
                        comboDSPPhoneRXBuf.SelectedIndex = 2;
                        comboDSPPhoneTXBuf.SelectedIndex = 2;
                        comboDSPCWRXBuf.SelectedIndex = 2;
                        comboDSPCWTXBuf.SelectedIndex = 2;
                        comboDSPDigRXBuf.SelectedIndex = 2;
                        comboDSPDigTXBuf.SelectedIndex = 2;
                    }

                    /*if (comboAudioInput1.Text.IndexOf("FLEX-1500") < 0 ||
                        comboAudioOutput1.Text.IndexOf("FLEX-1500") < 0)
                    {                       
                        console.PowerEnabled = false;
                    }*/

                    // get 1500 config info (firmware rev, board rev/sn, radio sn)
                    lbl1500SN.Text = "S/N: " + HIDEEPROM.SerialToString(HIDEEPROM.SerialNumber);

                    uint val;
                    USBHID.GetFirmwareRev(out val);
                    string s = "Firmware: " + HIDEEPROM.RevToString(val);
                    lbl1500FWRev.Text = s;

                    s = "TRX: " + HIDEEPROM.SerialToString(HIDEEPROM.TRXSerial);
                    val = HIDEEPROM.TRXRev;
                    s += "  (" + ((byte)(val >> 0)).ToString();
                    s += ((char)(((byte)(val >> 8)) + 65)).ToString() + ")";
                    lbl1500TRXRev.Text = s;

                    s = "PA: " + HIDEEPROM.SerialToString(HIDEEPROM.PASerial);
                    val = HIDEEPROM.PARev;
                    s += "  (" + ((byte)(val >> 0)).ToString();
                    s += ((char)(((byte)(val >> 8)) + 65)).ToString() + ")";
                    lbl1500PARev.Text = s;

                    DSP.SetThreadNumber(1);

                    string key = comboKeyerConnPrimary.Text;
                    if (comboKeyerConnPrimary.Items.Contains("SDR"))
                        comboKeyerConnPrimary.Items.Remove("SDR");
                    if (!comboKeyerConnPrimary.Items.Contains("Radio"))
                        comboKeyerConnPrimary.Items.Insert(0, "Radio");
                    if (key == "SDR" || key == "5000") comboKeyerConnPrimary.Text = "Radio";
                    else comboKeyerConnPrimary.Text = key;
                    comboKeyerConnPrimary_SelectedIndexChanged(this, EventArgs.Empty);
                    chkPANewCal.Checked = true;
                    chkGenFLEX1500Xref_CheckedChanged(this, EventArgs.Empty);

                    comboAudioBuffer1_SelectedIndexChanged(this, EventArgs.Empty);
                }
                else console.PowerEnabled = false;

                bool b = radGenModelFLEX1500.Checked;

                if (radGenModelFLEX1500.Focused || force_model && b)
                {
                    chkGeneralRXOnly.Checked = false;
                    chkGeneralDisablePTT.Checked = false;
                    force_model = false;
                }

                radPACalAllBands_CheckedChanged(this, EventArgs.Empty);
                grpGeneralHardwareSDR1000.Visible = false;
                grpGeneralHardwareFLEX5000.Visible = false;
                grpGeneralHardwareFLEX1500.Visible = true;

                // btnWizard.Visible = !b;
                grpGenAutoMute.Visible = !b;
                grpOptUSBBuf.Visible = b;

                grpGenCalLevel.Visible = !b;
                grpGenCalRXImage.Visible = !b;
                chkCalExpert.Visible = !b;

                chkRX2AutoMuteRX2OnVFOATX.Visible = !b;
                chkRX2AutoMuteRX1OnVFOBTX.Visible = !b;

                chkRX2AutoOn.Visible = !b; // .231
                chkRX2AutoVAC2.Visible = !b; // .231
                richTextBox1.Visible = !b; // .231
                richTextBox2.Visible = !b; // .231

                grpAudioLineInGain1.Visible = !b;
                grpAudioMicInGain1.Visible = !b;
                grpAudioChannels.Visible = !b;
                grpAudioSampleRate1.Visible = !b;
                grpAudioVolts1.Visible = !b;
                chkAudioExpert.Visible = !b;

                comboAudioMixer1.Visible = !b;
                lblAudioMixer1.Visible = !b;
                comboAudioReceive1.Visible = !b;
                lblAudioReceive1.Visible = !b;
                comboAudioTransmit1.Visible = !b;
                lblAudioTransmit1.Visible = !b;

                grpAudioDetails1.Visible = !b;
                grpAudioCard.Visible = !b;


                chkGeneralSoftwareGainCorr.Visible = !b;
                chkGeneralEnableX2.Visible = !b;
                lblGeneralX2Delay.Visible = !b;
                udGeneralX2Delay.Visible = !b;
                chkGeneralCustomFilter.Visible = !b;

                chkDSPImageExpert.Visible = b;
                chkDSPImageExpert_CheckedChanged(this, EventArgs.Empty);
                if (!b)
                {
                    grpDSPImageRejectTX.Visible = true;
                }

                grpTX1500.Visible = b;

                grpPAGainByBand.Visible = (radGenModelSDR1000.Checked && chkGeneralPAPresent.Checked);
                chkPANewCal.Visible = (radGenModelSDR1000.Checked && chkGeneralPAPresent.Checked);

                rtxtPACalReq.Visible = !b;

                if (b || radGenModelFLEX5000.Checked)
                {
                    if (tcSetup.TabPages.Contains(tpExtCtrl))
                    {
                        tcSetup.TabPages.Remove(tpExtCtrl);
                        tcSetup.SelectedIndex = 0;
                    }
                }
                else
                {
                    if (!tcSetup.TabPages.Contains(tpExtCtrl))
                        Common.TabControlInsert(tcSetup, tpExtCtrl, 6);
                }

                grpImpulseTest.Visible = !b;
                ckEnableSigGen.Visible = b;
                chkSigGenRX2.Visible = !b;
                grpTestX2.Visible = !b;
            }
        }

        private void radGenModelSDR1000_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radGenModelSDR1000.Checked)
            {
                console.CurrentModel = Model.SDR1000;
                comboGeneralLPTAddr_SelectedIndexChanged(this, EventArgs.Empty);
                chkGeneralUSBPresent_CheckedChanged(this, EventArgs.Empty);
                chkGeneralPAPresent_CheckedChanged(this, EventArgs.Empty);
                chkGeneralATUPresent_CheckedChanged(this, EventArgs.Empty);
                chkXVTRPresent_CheckedChanged(this, EventArgs.Empty);
                comboGeneralXVTR_SelectedIndexChanged(this, EventArgs.Empty);

                if (radGenModelSDR1000.Focused || force_model)
                {
                    chkGeneralRXOnly.Checked = false;
                    chkGeneralDisablePTT.Checked = false;
                    force_model = false;
                }
                chkGeneralRXOnly.Enabled = true;

                string key = comboKeyerConnPrimary.Text;
                if (comboKeyerConnPrimary.Items.Contains("Radio"))
                    comboKeyerConnPrimary.Items.Remove("Radio");
                if (!comboKeyerConnPrimary.Items.Contains("SDR"))
                    comboKeyerConnPrimary.Items.Insert(0, "SDR");
                if (key == "Radio") comboKeyerConnPrimary.Text = "SDR";
                else comboKeyerConnPrimary.Text = key;
                comboKeyerConnPrimary_SelectedIndexChanged(this, EventArgs.Empty);
                lblF3KFanTempThresh.Visible = false;
                udF3KFanTempThresh.Visible = false;
                chkGenTX1Delay.Visible = false;
                lblGenTX1Delay.Visible = false;
                udGenTX1Delay.Visible = false;
                chkTXLimitSlew.Visible = false;
                chkTXLimitSlew.Checked = false;

                // grpGeneralHardwareSDR1000.Visible = true;
                grpGeneralHardwareFLEX5000.Visible = false;
                grpGeneralHardwareFLEX1500.Visible = false;
            }
            else console.XVTRPresent = false;
        }

        private void radGenModelSoftRock40_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radGenModelSoftRock40.Checked)
            {
                chkGeneralDisablePTT.Checked = true;
                console.CurrentModel = Model.SOFTROCK40;
                if (radGenModelSoftRock40.Focused || force_model)
                {
                    chkGeneralRXOnly.Checked = true;
                    chkGeneralDisablePTT.Checked = true;
                    force_model = false;
                }
                chkGeneralRXOnly.Enabled = false;
                lblF3KFanTempThresh.Visible = false;
                udF3KFanTempThresh.Visible = false;
                chkGenTX1Delay.Visible = false;
                lblGenTX1Delay.Visible = false;
                udGenTX1Delay.Visible = false;
                chkTXLimitSlew.Visible = false;
                chkTXLimitSlew.Checked = false;

                // grpGeneralHardwareSDR1000.Visible = true;
                grpGeneralHardwareFLEX5000.Visible = false;
                grpGeneralHardwareFLEX1500.Visible = false;
            }
            grpHWSoftRock.Visible = radGenModelSoftRock40.Checked;
        }

        private void radGenModelDemoNone_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radGenModelDemoNone.Checked)
            {
                console.CurrentModel = Model.DEMO;
                //if(radGenModelDemoNone.Focused || force_model)
                {
                    chkGeneralRXOnly.Checked = true;
                    chkGeneralDisablePTT.Checked = true;
                    MessageBox.Show(new Form { TopMost = true }, "Welcome to the Demo/Test mode of the PowerSDR software.\n\n" +
                        "Please contact us at support@flexradio.com with any questions.\n\n" +
                        "If you did not intend to be in Demo/Test mode, please open the Setup Form and change the model " +
                        "to the appropriate selection (FLEX-5000, FLEX-3000, etc) and then restart PowerSDR.",
                        "Welcome to the Demo/Test Mode",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    force_model = false;
                    lblF3KFanTempThresh.Visible = false;
                    udF3KFanTempThresh.Visible = false;
                    chkGenTX1Delay.Visible = false;
                    lblGenTX1Delay.Visible = false;
                    udGenTX1Delay.Visible = false;
                    chkTXLimitSlew.Visible = false;
                    chkTXLimitSlew.Checked = false;
                }
                chkGeneralRXOnly.Enabled = true;
                DSP.SetThreadNumber(1);

                // grpGeneralHardwareSDR1000.Visible = true;
                grpGeneralHardwareFLEX5000.Visible = false;
                grpGeneralHardwareFLEX1500.Visible = false;
            }
        }

        private void udSoftRockCenterFreq_ValueChanged(object sender, System.EventArgs e)
        {
            console.SoftRockCenterFreq = (double)udSoftRockCenterFreq.Value;
        }

        private void comboGeneralLPTAddr_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (comboGeneralLPTAddr.Text == "" || console.CurrentModel != Model.SDR1000)
                return;
            console.Hdw.LPTAddr = Convert.ToUInt16(comboGeneralLPTAddr.Text, 16);
        }

        private void comboGeneralLPTAddr_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (console.CurrentModel != Model.SDR1000) return;
            if (comboGeneralLPTAddr.Text == "") return;
            if (e.KeyData == Keys.Enter)
            {
                if (comboGeneralLPTAddr.Text.Length > 4)
                {
                    MessageBox.Show(new Form { TopMost = true }, "Invalid Parallel Port Address (" + comboGeneralLPTAddr.Text + ")");
                    comboGeneralLPTAddr.Text = "378";
                    return;
                }

                foreach (Char c in comboGeneralLPTAddr.Text)
                {
                    if (!Char.IsDigit(c) &&
                        Char.ToLower(c) < 'a' &&
                        Char.ToLower(c) > 'f')
                    {
                        MessageBox.Show(new Form { TopMost = true }, "Invalid Parallel Port Address (" + comboGeneralLPTAddr.Text + ")");
                        comboGeneralLPTAddr.Text = "378";
                        return;
                    }
                }

                console.Hdw.LPTAddr = Convert.ToUInt16(comboGeneralLPTAddr.Text, 16);
            }

        }

        private void comboGeneralLPTAddr_LostFocus(object sender, System.EventArgs e)
        {
            comboGeneralLPTAddr_KeyDown(sender, new KeyEventArgs(Keys.Enter));
        }

        // ke9ns (see  PollRXOnly() in console)
        private void chkGeneralRXOnly_CheckedChanged(object sender, System.EventArgs e)
        {
            bool rx_only = chkGeneralRXOnly.Checked;

            if (chkGeneralRXOnly.Focused && comboAudioSoundCard.Text == "Unsupported Card" && !rx_only && radGenModelSDR1000.Checked)
            {
                DialogResult dr = MessageBox.Show(
                    "Unchecking Receive Only while in Unsupported Card mode may \n" +
                    "cause damage to your SDR-1000 hardware.  Are you sure you want \n" +
                    "to enable transmit?",
                    "Warning: Enable Transmit?",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);
                if (dr == DialogResult.No)
                {
                    chkGeneralRXOnly.Checked = true;
                    return;
                }
            }

            if (console.RXOnly != rx_only) // GET CONSOLE RXONLY
            {
                console.RXOnly = rx_only;  // SET CONSOLE RXONLY

            }

            tpTransmit.Enabled = !rx_only;

            tpPowerAmplifier.Enabled = !rx_only;

            grpTestTXIMD.Enabled = !rx_only;

        } // chkGeneralRXOnly_CheckedChanged




        private void chkGeneralUSBPresent_CheckedChanged(object sender, System.EventArgs e)
        {
            try
            {
                console.USBPresent = chkGeneralUSBPresent.Checked;
                if (chkGeneralUSBPresent.Checked)
                {
                    if (!USB.Init(true, chkGeneralPAPresent.Checked))
                        chkGeneralUSBPresent.Checked = false;
                    else USB.Console = console;
                }
                else
                    USB.Exit();

                if (console.PowerOn)
                {
                    console.PowerOn = false;
                    Thread.Sleep(100);
                    console.PowerOn = true;
                }
            }
            catch (Exception)
            {
                MessageBox.Show(new Form { TopMost = true }, "A required DLL was not found (Sdr1kUsb.dll).  Please download the\n" +
                    "installer from the FlexRadio private download page and try again.",
                    "Error: Missing DLL",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                chkGeneralUSBPresent.Checked = false;
            }
        }

        private void chkGeneralPAPresent_CheckedChanged(object sender, System.EventArgs e)
        {
            console.PAPresent = chkGeneralPAPresent.Checked;
            chkGeneralATUPresent.Visible = chkGeneralPAPresent.Checked;
            grpPAGainByBand.Visible = chkGeneralPAPresent.Checked;
            rtxtPACalReq.Visible = chkGeneralPAPresent.Checked;

            if (!chkGeneralPAPresent.Checked)
                chkGeneralATUPresent.Checked = false;
            else if (console.PowerOn)
            {
                console.PowerOn = false;
                Thread.Sleep(100);
                console.PowerOn = true;
            }

            if (chkGeneralUSBPresent.Checked)
            {
                chkGeneralUSBPresent.Checked = false;
                chkGeneralUSBPresent.Checked = true;
            }
        }

        private void chkGeneralATUPresent_CheckedChanged(object sender, System.EventArgs e)
        {
            console.ATUPresent = chkGeneralATUPresent.Checked;
        }

        private void chkXVTRPresent_CheckedChanged(object sender, System.EventArgs e)
        {
            console.XVTRPresent = chkGeneralXVTRPresent.Checked;
            comboGeneralXVTR.Visible = chkGeneralXVTRPresent.Checked;
            if (chkGeneralXVTRPresent.Checked)
            {
                if (comboGeneralXVTR.SelectedIndex == (int)XVTRTRMode.POSITIVE)
                    comboGeneralXVTR_SelectedIndexChanged(this, EventArgs.Empty);
                else
                    comboGeneralXVTR.SelectedIndex = (int)XVTRTRMode.POSITIVE;
            }
        }

        private void chkGeneralSpurRed_CheckedChanged(object sender, System.EventArgs e)
        {
            console.SpurReduction = chkGeneralSpurRed.Checked;
        }

        private void udDDSCorrection_ValueChanged(object sender, System.EventArgs e)
        {
            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                case Model.FLEX3000:
                    console.FWCDDSClockCorrection = (double)((double)udDDSCorrection.Value * 1e-6);
                    break;
                case Model.FLEX1500:
                    console.HIDDDSClockCorrection = (double)((double)udDDSCorrection.Value * 1e-6);
                    break;
                default:
                    console.DDSClockCorrection = (double)((double)udDDSCorrection.Value * 1e-6);
                    break;
            }
        }

        private void udDDSPLLMult_ValueChanged(object sender, System.EventArgs e)
        {
            if (console.Hdw != null)
                console.Hdw.PLLMult = (int)udDDSPLLMult.Value;
        }

        private void udDDSIFFreq_ValueChanged(object sender, System.EventArgs e)
        {
            console.IFFreq = (double)udDDSIFFreq.Value * 1e-6;
        }

        private void btnGeneralCalFreqUseVFOA_Click(object sender, System.EventArgs e)
        {
            decimal calfreaq = Convert.ToDecimal(console.VFOAFreq);
            if (calfreaq <= udGeneralCalFreq1.Maximum) udGeneralCalFreq1.Value = calfreaq;
        }

        private void btnGeneralCalFreqStart_Click(object sender, System.EventArgs e)
        {
            btnGeneralCalFreqStart.Enabled = false;
            btnGeneralCalFreqUseVFOA.Enabled = false;

            progress = new Progress("Calibrate Frequency");

            Thread t = new Thread(new ThreadStart(CalibrateFreq));
            t.Name = "Freq Calibration Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.AboveNormal;
            t.Start();

            if (console.PowerOn) progress.Show();
        }

        private void btnGeneralCalLevelStart_Click(object sender, System.EventArgs e)
        {
            btnGeneralCalLevelStart.Enabled = false;
            progress = new Progress("Calibrate RX Level");

            Thread t = new Thread(new ThreadStart(CalibrateLevel));
            t.Name = "Level Calibration Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.AboveNormal;
            t.Start();

            if (console.PowerOn)
                progress.Show();
        }

        private void btnGeneralCalImageStart_Click(object sender, System.EventArgs e)
        {
            btnGeneralCalImageStart.Enabled = false;
            progress = new Progress("Calibrate RX Image Rejection");

            Thread t = new Thread(new ThreadStart(CalibrateRXImage));
            t.Name = "RX Image Calibration Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.AboveNormal;
            t.Start();

            if (console.PowerOn)
                progress.Show();
        }

        private void CalibrateFreq()
        {
            bool done = console.CalibrateFreq((float)udGeneralCalFreq1.Value, progress, false);
            if (done) MessageBox.Show(new Form { TopMost = true }, "Frequency Calibration complete.");
            btnGeneralCalFreqStart.Enabled = true;
            btnGeneralCalFreqUseVFOA.Enabled = true;
        }

        private void CalibrateLevel()
        {
            bool done = console.CalibrateLevel(
                (float)udGeneralCalLevel.Value,
                (float)udGeneralCalFreq2.Value,
                progress,
                false);
            if (done) MessageBox.Show(new Form { TopMost = true }, "Level Calibration complete.");
            btnGeneralCalLevelStart.Enabled = true;
        }

        private void CalibrateRXImage()
        {
            bool done = console.CalibrateRXImage((float)udGeneralCalFreq3.Value, progress, false);
            if (done) MessageBox.Show(new Form { TopMost = true }, "RX Image Rejection Calibration complete.");
            btnGeneralCalImageStart.Enabled = true;
        }

        private void chkGeneralDisablePTT_CheckedChanged(object sender, System.EventArgs e)
        {
            console.DisablePTT = chkGeneralDisablePTT.Checked;
        }

        private void comboGeneralXVTR_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            switch (comboGeneralXVTR.SelectedIndex)
            {
                case (int)XVTRTRMode.NEGATIVE:
                    if (comboGeneralXVTR.Focused)
                    {
                        MessageBox.Show(new Form { TopMost = true }, "The default TR Mode for the DEMI144-28FRS sold by FlexRadio Systems is\n" +
                            "Postive TR Logic.  Please use caution when using other TR modes.", "Warning");
                    }
                    break;
                case (int)XVTRTRMode.POSITIVE:
                case (int)XVTRTRMode.NONE:
                    break;
            }

            console.CurrentXVTRTRMode = (XVTRTRMode)comboGeneralXVTR.SelectedIndex;
        }

        private void chkGeneralSoftwareGainCorr_CheckedChanged(object sender, System.EventArgs e)
        {
            console.NoHardwareOffset = chkGeneralSoftwareGainCorr.Checked;
        }

        private void chkGeneralEnableX2_CheckedChanged(object sender, System.EventArgs e)
        {
            console.X2Enabled = chkGeneralEnableX2.Checked;
            udGeneralX2Delay.Enabled = chkGeneralEnableX2.Checked;
        }

        private void udGeneralX2Delay_ValueChanged(object sender, System.EventArgs e)
        {
            console.X2Delay = (int)udGeneralX2Delay.Value;
        }

        private void comboGeneralProcessPriority_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            Process p = Process.GetCurrentProcess();

            if (comboGeneralProcessPriority.Text == "Real Time" &&
                comboGeneralProcessPriority.Focused)
            {
                DialogResult dr = MessageBox.Show(
                    "Setting the Process Priority to Realtime can cause the system to become unresponsive.\n" +
                    "This setting is not recommended.\n" +
                    "Are you sure you want to change to Realtime?",
                    "Warning: Realtime Not Recommended",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);
                if (dr == DialogResult.No)
                {
                    switch (p.PriorityClass)
                    {
                        case ProcessPriorityClass.Idle:
                            comboGeneralProcessPriority.Text = "Idle";
                            break;
                        case ProcessPriorityClass.BelowNormal:
                            comboGeneralProcessPriority.Text = "Below Normal";
                            break;
                        case ProcessPriorityClass.AboveNormal:
                            comboGeneralProcessPriority.Text = "Above Normal";
                            break;
                        case ProcessPriorityClass.High:
                            comboGeneralProcessPriority.Text = "Highest";
                            break;
                        default:
                            comboGeneralProcessPriority.Text = "Normal";
                            break;
                    }
                    return;
                }
            }

            switch (comboGeneralProcessPriority.Text)
            {
                case "Idle":
                    p.PriorityClass = ProcessPriorityClass.Idle;
                    break;
                case "Below Normal":
                    p.PriorityClass = ProcessPriorityClass.BelowNormal;
                    break;
                case "Normal":
                    p.PriorityClass = ProcessPriorityClass.Normal;
                    break;
                case "Above Normal":
                    p.PriorityClass = ProcessPriorityClass.AboveNormal;
                    break;
                case "High":
                    p.PriorityClass = ProcessPriorityClass.High;
                    break;
                case "Real Time":
                    p.PriorityClass = ProcessPriorityClass.RealTime;
                    break;
            }
        }

        private void chkGeneralCustomFilter_CheckedChanged(object sender, System.EventArgs e)
        {
            console.EnableLPF0 = chkGeneralCustomFilter.Checked;
        }

        private void chkGenAutoMute_CheckedChanged(object sender, System.EventArgs e)
        {
            console.AutoMute = chkGenAutoMute.Checked;
        }

        private void chkOptQuickQSY_CheckedChanged(object sender, System.EventArgs e)
        {
            console.QuickQSY = chkOptQuickQSY.Checked;
        }

        private void chkOptAlwaysOnTop_CheckedChanged(object sender, System.EventArgs e)
        {
            console.AlwaysOnTop = chkOptAlwaysOnTop.Checked;
        }

        private void udOptClickTuneOffsetDIGL_ValueChanged(object sender, System.EventArgs e)
        {
            console.DIGLClickTuneOffset = (int)udOptClickTuneOffsetDIGL.Value;
        }

        private void udOptClickTuneOffsetDIGU_ValueChanged(object sender, System.EventArgs e)
        {
            console.DIGUClickTuneOffset = (int)udOptClickTuneOffsetDIGU.Value;
        }

        private void udOptMaxFilterWidth_ValueChanged(object sender, System.EventArgs e)
        {
            console.MaxFilterWidth = (int)udOptMaxFilterWidth.Value;
        }

        private void comboOptFilterWidthMode_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            switch (comboOptFilterWidthMode.Text)
            {
                case "Linear":
                    console.CurrentFilterWidthMode = FilterWidthMode.Linear;
                    break;
                case "Log":
                    console.CurrentFilterWidthMode = FilterWidthMode.Log;
                    break;
                case "Log10":
                    console.CurrentFilterWidthMode = FilterWidthMode.Log10;
                    break;
            }
        }

        private void udOptMaxFilterShift_ValueChanged(object sender, System.EventArgs e)
        {
            console.MaxFilterShift = (int)udOptMaxFilterShift.Value;
        }

        private void chkOptFilterSaveChanges_CheckedChanged(object sender, System.EventArgs e)
        {
            console.SaveFilterChanges = chkOptFilterSaveChanges.Checked;
        }

        private void chkOptEnableKBShortcuts_CheckedChanged(object sender, System.EventArgs e)
        {
            console.EnableKBShortcuts = chkOptEnableKBShortcuts.Checked;
            chkOptQuickQSY.Enabled = chkOptEnableKBShortcuts.Checked;
        }

        private void udFilterDefaultLowCut_ValueChanged(object sender, System.EventArgs e)
        {
            console.DefaultLowCut = (int)udFilterDefaultLowCut.Value;
        }



        #endregion

        #region Audio Tab Event Handlers
        // ======================================================
        // Audio Tab Event Handlers
        // ======================================================

        private void comboAudioDriver1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (comboAudioDriver1.SelectedIndex < 0) return;

            int old_host = Audio.Host1;
            int new_host = ((PADeviceInfo)comboAudioDriver1.SelectedItem).Index;
            bool power = console.PowerOn;

            if (power && old_host != new_host)
            {
                console.PowerOn = false;
                Thread.Sleep(500);
            }
            console.AudioDriverIndex1 = new_host;
            Audio.Host1 = new_host;
            GetDevices1();
            if (comboAudioInput1.Items.Count != 0)
                comboAudioInput1.SelectedIndex = 0;
            if (comboAudioOutput1.Items.Count != 0)
                comboAudioOutput1.SelectedIndex = 0;
            if (power && old_host != new_host) console.PowerOn = true;

            if (!chkAudioLatencyManual1.Checked)
            {
                if (comboAudioDriver1.Text == "MME" || comboAudioDriver1.Text == "DirectSound")
                    Audio.Latency1 = 200;
                else Audio.Latency1 = 0;
            }
        }


        //==========================================================================================
        // ke9ns primary audio input select (audio mixer in the flex itself)
        //       this would choose which to send over firewire to the pc for DSP
        //==========================================================================================
        private void comboAudioInput1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (comboAudioInput1.SelectedIndex < 0) return;

            int old_input = Audio.Input1;
            int new_input = ((PADeviceInfo)comboAudioInput1.SelectedItem).Index;
            bool power = console.PowerOn;

            if (power && old_input != new_input)
            {
                console.PowerOn = false;
                Thread.Sleep(500);
            }

            console.AudioInputIndex1 = new_input;
            Audio.Input1 = new_input;                      // ke9ns tell audio.cs your primary input device

            if (comboAudioInput1.SelectedIndex == 0 && comboAudioDriver1.SelectedIndex < 2)
            {
                comboAudioMixer1.SelectedIndex = 0;
            }
            else
            {
                for (int i = 0; i < comboAudioMixer1.Items.Count; i++)
                {
                    string s = (string)comboAudioMixer1.Items[i];
                    if (s.StartsWith(comboAudioInput1.Text.Substring(0, 5)))
                        comboAudioMixer1.Text = s;
                }
                comboAudioMixer1.Text = comboAudioInput1.Text;
            }

            if (power && old_input != new_input) console.PowerOn = true;

        } // comboAudioInput1_SelectedIndexChanged



        //==========================================================================================
        // ke9ns primary audio output select (audio mixer in the flex itself)
        //       this would choose which to send over firewire to the pc for DSP
        //==========================================================================================
        private void comboAudioOutput1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (comboAudioOutput1.SelectedIndex < 0) return;

            int old_output = Audio.Output1;
            int new_output = ((PADeviceInfo)comboAudioOutput1.SelectedItem).Index;
            bool power = console.PowerOn;

            if (power && new_output != old_output)
            {
                console.PowerOn = false;
                Thread.Sleep(500);
            }

            console.AudioOutputIndex1 = new_output;
            Audio.Output1 = new_output;                      // ke9ns tell audio.cs your primary output device

            if (power && new_output != old_output) console.PowerOn = true;
        }




        private void comboAudioMixer1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (comboAudioMixer1.SelectedIndex < 0) return;
            UpdateMixerControls1();
            console.MixerID1 = comboAudioMixer1.SelectedIndex;
        }

        //=============================================================================
        private void comboAudioReceive1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (comboAudioReceive1.SelectedIndex < 0) return;
            console.MixerRXMuxID1 = comboAudioReceive1.SelectedIndex;
            if (!initializing && console.PowerOn)
                Mixer.SetMux(comboAudioMixer1.SelectedIndex, comboAudioReceive1.SelectedIndex);
        }

        private void comboAudioTransmit1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (comboAudioTransmit1.SelectedIndex < 0) return;
            console.MixerTXMuxID1 = comboAudioTransmit1.SelectedIndex;
        }

        //================================================================================================
        private void chkAudioEnableVAC_CheckedChanged(object sender, System.EventArgs e)
        {
           if (radVAC1SelectA.Checked == false && radVAC1SelectB.Checked == false)
           {
                   radVAC1SelectA.CheckedChanged -= chkVAC1SelectA_CheckedChanged;
                   radVAC1SelectA.Checked = true;
                   radVAC1SelectA.CheckedChanged += chkVAC1SelectA_CheckedChanged;
           }

            bool val = chkAudioEnableVAC.Checked;
            bool old_val = console.VACEnabled;

            if (val)
            {
                
                   if ((comboAudioDriver2.SelectedIndex < 0) && (comboAudioDriver2.Items.Count > 0)) comboAudioDriver2.SelectedIndex = 0;
                   if ((comboAudioDriver2B.SelectedIndex < 0) && (comboAudioDriver2B.Items.Count > 0)) comboAudioDriver2B.SelectedIndex = 0;
                   if ((comboAudioInput2.SelectedIndex < 0) && (comboAudioInput2.Items.Count > 0)) comboAudioInput2.SelectedIndex = 0;
                   if ((comboAudioInput2B.SelectedIndex < 0) && (comboAudioInput2B.Items.Count > 0)) comboAudioInput2B.SelectedIndex = 0;
                   if ((comboAudioOutput2.SelectedIndex < 0) && (comboAudioOutput2.Items.Count > 0)) comboAudioOutput2.SelectedIndex = 0;
                   if ((comboAudioOutput2B.SelectedIndex < 0) && (comboAudioOutput2B.Items.Count > 0)) comboAudioOutput2B.SelectedIndex = 0;

            }

            bool power = console.PowerOn;

            if (power && val != old_val)
            {
                console.PowerOn = false;
                Thread.Sleep(200);
            }
                       

            if (val && radVAC1SelectA.Checked)
            {
                console.AudioDriverIndex2 =  ((PADeviceInfo)comboAudioDriver2.SelectedItem).Index;
                Audio.Host2 = ((PADeviceInfo)comboAudioDriver2.SelectedItem).Index;

                console.AudioInputIndex2 = ((PADeviceInfo)comboAudioInput2.SelectedItem).Index;
                Audio.Input2 = ((PADeviceInfo)comboAudioInput2.SelectedItem).Index;

                console.AudioOutputIndex2 =  ((PADeviceInfo)comboAudioOutput2.SelectedItem).Index;
                Audio.Output2 = ((PADeviceInfo)comboAudioOutput2.SelectedItem).Index;


            } // if driver, inpput, and output devices selected then update PowerSDR .204
            else if (val && radVAC1SelectB.Checked)
            {
                console.AudioDriverIndex2 = ((PADeviceInfo)comboAudioDriver2B.SelectedItem).Index;
                Audio.Host2 = ((PADeviceInfo)comboAudioDriver2B.SelectedItem).Index;

                console.AudioInputIndex2 = ((PADeviceInfo)comboAudioInput2B.SelectedItem).Index;
                Audio.Input2 = ((PADeviceInfo)comboAudioInput2B.SelectedItem).Index;

                console.AudioOutputIndex2 = ((PADeviceInfo)comboAudioOutput2B.SelectedItem).Index;
                Audio.Output2 = ((PADeviceInfo)comboAudioOutput2B.SelectedItem).Index;

            } 

            if (chkAudioIQtoVAC.Checked) console.SpurReduction = false;

            console.VACEnabled = val;

            if (power && val != old_val) console.PowerOn = true;


        } // VAC1 chkAudioEnableVAC_CheckedChanged

       
        // ke9ns add .204
        public void chkVAC1SelectA_CheckedChanged(object sender, EventArgs e)
        {
            Debug.WriteLine("SELECT A checkchagned " + radVAC1SelectA.Checked);

            if (radVAC1SelectA.Checked)
            {
                if (chkAudioEnableVAC.Checked)
                {
                    bool val = console.chkPower.Checked;

                    if (val)
                    {
                        console.PowerOn = false;
                        Thread.Sleep(200);
                    }
                    chkAudioEnableVAC.Checked = false;
                    Thread.Sleep(400);
                    chkAudioEnableVAC.Checked = true;

                    if (val)
                    {
                        console.PowerOn = true;
                        Thread.Sleep(200);
                    }
                }
            }

        } // chkVAC1SelectA_CheckedChanged

        public void chkVAC1SelectB_CheckedChanged(object sender, EventArgs e)
        {

            Debug.WriteLine("SELECT B checkchagned " + radVAC1SelectB.Checked);

            if (radVAC1SelectB.Checked)
            {

                if (chkAudioEnableVAC.Checked)
                {
                    bool val = console.chkPower.Checked;
                    if (val)
                    {
                        console.PowerOn = false;
                        Thread.Sleep(200);
                    }

                    chkAudioEnableVAC.Checked = false;
                    Thread.Sleep(400);
                    chkAudioEnableVAC.Checked = true;
                    if (val)
                    {
                        console.PowerOn = true;
                        Thread.Sleep(200);
                    }
                }
            }

        } //chkVAC1SelectB_CheckedChanged

        private void chkVAC1SelectB_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("SELECT B CLICKED " + radVAC1SelectB.Checked);

        }
        private void chkVAC1SelectA_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("SELECT A CLICKED " + radVAC1SelectA.Checked);

        }

        //=======================================================================================
        // ke9ns  select VAC2 enable
        //=======================================================================================
        private void chkVAC2Enable_CheckedChanged(object sender, EventArgs e)
        {
            bool val = chkVAC2Enable.Checked;
            bool old_val = console.VAC2Enabled;

            /* ke9ns mod
                if (!radGenModelFLEX5000.Checked || !FWCEEPROM.RX2OK)
                 {
                    if (chkVAC2Enable.Checked)   chkVAC2Enable.Checked = false;
                     console.VAC2Enabled = false;
                    return;
                }
     */
            if (val)
            {
                if (comboAudioDriver3.SelectedIndex < 0 && comboAudioDriver3.Items.Count > 0)
                {
                    comboAudioDriver3.SelectedIndex = 0;
                    Debug.WriteLine("7VAC2---");

                }
            }

            bool power = console.PowerOn;

            if (power && val != old_val)
            {

                console.PowerOn = false;
                Thread.Sleep(500);
            }

            if (chkVAC2DirectIQ.Checked) console.SpurReduction = false;

            console.VAC2Enabled = val;

            if (power && val != old_val) console.PowerOn = true;


        } // chkVAC2Enable_CheckedChanged



        //=======================================================================================
        // ke9ns  select primary
        //=======================================================================================
        private void comboAudioChannels1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (comboAudioChannels1.SelectedIndex < 0) return;

            int old_chan = Audio.NumChannels;
            int new_chan = Int32.Parse(comboAudioChannels1.Text);
            bool power = console.PowerOn;

            if (power && chkAudioEnableVAC.Checked && old_chan != new_chan)
            {
                console.PowerOn = false;
                Thread.Sleep(500);
            }

            console.NumChannels = new_chan;
            Audio.NumChannels = new_chan;

            //DSP.SetThreadNumber((uint)new_chan/2);
            if (power && chkAudioEnableVAC.Checked && old_chan != new_chan) console.PowerOn = true;
        }


        //=======================================================================================
        // ke9ns  select VAC1 host devices
        //=======================================================================================
        private void comboAudioDriver2_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (comboAudioDriver2.SelectedIndex < 0) return;

            int old_driver = Audio.Host2;
            int new_driver = ((PADeviceInfo)comboAudioDriver2.SelectedItem).Index;
            bool power = console.PowerOn;

            if (power && chkAudioEnableVAC.Checked && old_driver != new_driver)
            {
                console.PowerOn = false;
                Thread.Sleep(500);
            }

            string new_driver_name = ((PADeviceInfo)comboAudioDriver2.SelectedItem).Name;

            if (((new_driver_name != "Windows WDM-KS") && (new_driver_name != "ASIO")) && udAudioLatency2.Value < 50)
            {
                MessageBox.Show(new Form { TopMost = true }, "The VAC1 Driver type selected does not support a Buffer Latency value less than 120ms.  " +
                    "Buffer Latency values less than 120ms are only valid when using the WDM-KS VAC audio driver.\n\n" +
                    "The VAC1 Buffer Latency has been reset to the default of 120ms.  " +
                    "Make sure to save your Transmit profile to make the change persistent.",
                    "Invalid VAC1 Buffer Latency Value",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                udAudioLatency2.Value = 120;
            }

            if (radVAC1SelectA.Checked)
            {
                console.AudioDriverIndex2 = new_driver;
                Audio.Host2 = new_driver;
            }

            GetDevices2();  // vac1

            if (comboAudioInput2.Items.Count != 0) comboAudioInput2.SelectedIndex = 0;
            if (comboAudioOutput2.Items.Count != 0) comboAudioOutput2.SelectedIndex = 0;

            if (power && chkAudioEnableVAC.Checked && old_driver != new_driver)
                console.PowerOn = true;

        } //  VAC1 comboAudioDriver2_SelectedIndexChanged


        // ke9ns add .204
        private void comboAudioDriver2B_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboAudioDriver2B.SelectedIndex < 0) return;

            int old_driver = Audio.Host2;
            int new_driver = ((PADeviceInfo)comboAudioDriver2B.SelectedItem).Index;
            bool power = console.PowerOn;

            if (power && chkAudioEnableVAC.Checked && old_driver != new_driver && radVAC1SelectB.Checked)
            {
                console.PowerOn = false;
                Thread.Sleep(500);
            }

            string new_driver_name = ((PADeviceInfo)comboAudioDriver2B.SelectedItem).Name;

            if (((new_driver_name != "Windows WDM-KS") && (new_driver_name != "ASIO")) && udAudioLatency2.Value < 50)
            {
                MessageBox.Show(new Form { TopMost = true }, "The VAC1 Driver type selected does not support a Buffer Latency value less than 120ms.  " +
                    "Buffer Latency values less than 120ms are only valid when using the WDM-KS VAC audio driver.\n\n" +
                    "The VAC1 Buffer Latency has been reset to the default of 120ms.  " +
                    "Make sure to save your Transmit profile to make the change persistent.",
                    "Invalid VAC1 Buffer Latency Value",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                udAudioLatency2.Value = 120;
            }

            if (radVAC1SelectB.Checked)
            {
                console.AudioDriverIndex2 = new_driver;
                Audio.Host2 = new_driver;
            }
        

            GetDevices2B();  // vac1

            if (comboAudioInput2.Items.Count != 0) comboAudioInput2.SelectedIndex = 0;
            if (comboAudioOutput2.Items.Count != 0) comboAudioOutput2.SelectedIndex = 0;

            if (power && chkAudioEnableVAC.Checked && old_driver != new_driver)
                console.PowerOn = true;

        } // comboAudioDriver2B_SelectedIndexChanged



        //=======================================================================================
        // ke9ns  select VAC2 host devices
        //=======================================================================================
        private void comboAudioDriver3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboAudioDriver3.SelectedIndex < 0) return;

            int old_driver = Audio.Host3;
            int new_driver = ((PADeviceInfo)comboAudioDriver3.SelectedItem).Index;
            bool power = console.PowerOn;

            if (power && chkVAC2Enable.Checked && old_driver != new_driver)
            {
                console.PowerOn = false;
                Thread.Sleep(500);
            }

            string new_driver_name = ((PADeviceInfo)comboAudioDriver3.SelectedItem).Name;

            if (((new_driver_name != "Windows WDM-KS") && (new_driver_name != "ASIO")) && udVAC2Latency.Value < 50)
            {
                MessageBox.Show(new Form { TopMost = true }, "The VAC2 Driver type selected does not support a Buffer Latency value less than 120ms.  " +
                    "Buffer Latency values less than 120ms are only valid when using the WDM-KS VAC audio driver.\n\n" +
                    "The VAC2 Buffer Latency has been reset to the default of 120ms.  " +
                    "Make sure to save your Transmit profile to make the change persistent.",
                    "Invalid VAC2 Buffer Latency Value",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                udVAC2Latency.Value = 120;
            }

            console.AudioDriverIndex3 = new_driver;
            Audio.Host3 = new_driver;
            GetDevices3();
            if (comboAudioInput3.Items.Count != 0) comboAudioInput3.SelectedIndex = 0;
            if (comboAudioOutput3.Items.Count != 0) comboAudioOutput3.SelectedIndex = 0;

            if (power && chkVAC2Enable.Checked && old_driver != new_driver) console.PowerOn = true;
        }



        //=======================================================================================
        // ke9ns  select VAC1 in device
        //=======================================================================================
        private void comboAudioInput2_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            //  Debug.WriteLine("test1===============");

            if (comboAudioInput2.SelectedIndex < 0) return;
            //   Debug.WriteLine("test2===============");

            int old_input = Audio.Input2;
            int new_input = ((PADeviceInfo)comboAudioInput2.SelectedItem).Index;
            bool power = console.PowerOn;
            Debug.WriteLine("VAC1 A INPUT INDEX CHANGED=============== " + new_input);
            if (power && chkAudioEnableVAC.Checked && old_input != new_input)
            {
                console.PowerOn = false;
                Thread.Sleep(500);
            }

            //  Debug.WriteLine("test3===============");

            if (radVAC1SelectA.Checked)
            {
                console.AudioInputIndex2 = new_input;
                Audio.Input2 = new_input;
            }

            if (power && chkAudioEnableVAC.Checked && old_input != new_input) console.PowerOn = true;

        } //VAC1 comboAudioInput2_SelectedIndexChanged

        // ke9ns add .204
        private void comboAudioInput2B_SelectedIndexChanged(object sender, EventArgs e)
        {

            //  Debug.WriteLine("test1===============");

            if (comboAudioInput2B.SelectedIndex < 0) return;
           

            int old_input = Audio.Input2;
            int new_input = ((PADeviceInfo)comboAudioInput2B.SelectedItem).Index;
            bool power = console.PowerOn;
            Debug.WriteLine("VAC1 B INPUT INDEX CHANGED=============== "+ new_input);

            if (power && chkAudioEnableVAC.Checked && old_input != new_input && radVAC1SelectB.Checked)
            {
                console.PowerOn = false;
                Thread.Sleep(500);
            }

            //  Debug.WriteLine("test3===============");

            if (radVAC1SelectB.Checked)
            {
                console.AudioInputIndex2 = new_input;
                Audio.Input2 = new_input;
            }

            if (power && chkAudioEnableVAC.Checked && old_input != new_input && radVAC1SelectB.Checked) console.PowerOn = true;

        } // VAC1 comboAudioInput2B_SelectedIndexChanged



        //=======================================================================================
        // ke9ns  select VAC2 in device
        //=======================================================================================
        private void comboAudioInput3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboAudioInput3.SelectedIndex < 0) return;

            int old_input = Audio.Input3;
            int new_input = ((PADeviceInfo)comboAudioInput3.SelectedItem).Index;
            bool power = console.PowerOn;

            if (power && chkVAC2Enable.Checked && old_input != new_input)
            {
                console.PowerOn = false;
                Thread.Sleep(500);
            }

            console.AudioInputIndex3 = new_input;
            Audio.Input3 = new_input;

            if (power && chkVAC2Enable.Checked && old_input != new_input)
                console.PowerOn = true;
        }

        //=======================================================================================
        // ke9ns  select VAC1 out device
        //=======================================================================================
        private void comboAudioOutput2_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (comboAudioOutput2.SelectedIndex < 0) return;

            int old_output = Audio.Output2;
            int new_output = ((PADeviceInfo)comboAudioOutput2.SelectedItem).Index;
            bool power = console.PowerOn;
            if (power && chkAudioEnableVAC.Checked && old_output != new_output)
            {
                console.PowerOn = false;
                Thread.Sleep(500);
            }
            if (radVAC1SelectA.Checked)
            {
                console.AudioOutputIndex2 = new_output;
                Audio.Output2 = new_output;
            }

            if (power && chkAudioEnableVAC.Checked && old_output != new_output)
                console.PowerOn = true;

        } // VAC1 comboAudioOutput2_SelectedIndexChanged

        // ke9ns add .204
        private void comboAudioOutput2B_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboAudioOutput2B.SelectedIndex < 0) return;


            int old_output = Audio.Output2;
            int new_output = ((PADeviceInfo)comboAudioOutput2B.SelectedItem).Index;

            Debug.WriteLine("VAC1 B output " + new_output);


            bool power = console.PowerOn;
            if (power && chkAudioEnableVAC.Checked && old_output != new_output && radVAC1SelectB.Checked)
            {
                console.PowerOn = false;
                Thread.Sleep(500);
            }
            if (radVAC1SelectB.Checked)
            {
                console.AudioOutputIndex2 = new_output;
                Audio.Output2 = new_output;
            }
           
            if (power && chkAudioEnableVAC.Checked && old_output != new_output && radVAC1SelectB.Checked)
                console.PowerOn = true;


        } // VAC1 comboAudioOutput2B_SelectedIndexChanged


        //=======================================================================================
        // ke9ns  select VAC2 out device
        //=======================================================================================
        private void comboAudioOutput3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboAudioOutput3.SelectedIndex < 0) return;

            int old_output = Audio.Output3;
            int new_output = ((PADeviceInfo)comboAudioOutput3.SelectedItem).Index;
            bool power = console.PowerOn;
            if (power && chkVAC2Enable.Checked && old_output != new_output)
            {
                console.PowerOn = false;
                Thread.Sleep(500);
            }

            console.AudioOutputIndex3 = new_output;
            Audio.Output3 = new_output;

            if (power && chkVAC2Enable.Checked && old_output != new_output)
                console.PowerOn = true;
        }

        //=============================================================================================
        // ke9ns mod  can the 3000 use 192000 ? yes, but it does appear that the display does roll off early.

        private void comboAudioSampleRate1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            Display.lastvaluecount = 0;

            Debug.WriteLine("SR RATE CHANGE HERE:" + comboAudioSampleRate1.SelectedIndex);

            if (comboAudioSampleRate1.SelectedIndex < 0) return;

            int old_rate = console.SampleRate1;
            int new_rate = Int32.Parse(comboAudioSampleRate1.Text);


            if (console.CurrentModel == Model.FLEX3000 && new_rate == 192000)  // ke9ns this would normally limit the 3000 to SR=96000
            {
                //  comboAudioSampleRate1.Text = "96000";
                //  new_rate = 96000;
            }

            else if (console.CurrentModel == Model.FLEX1500 && new_rate > 48000)
            {
                comboAudioSampleRate1.Text = "48000";
                new_rate = 48000;
            }

            bool power = console.PowerOn;

            if (power && new_rate != old_rate)
            {
                console.PowerOn = false;
                Thread.Sleep(800);
            }
            Display.lastvaluecount = 0;

            console.SampleRate1 = new_rate;

            Display.DrawBackground();
            console.SoftRockCenterFreq = console.SoftRockCenterFreq; // warning -- this appears to do nothing - not true, these are
                                                                     // properties and the assignment is needed due to side effects!   
                                                                     // We need the soft rock  code to recalc  its tuning limits -- 
                                                                     // setting the center freq does this as a side effect
            udDisplayScopeTime_ValueChanged(this, EventArgs.Empty);

            if (!initializing)
            {
                DSP.SyncStatic();        // ke9ns: hires

                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        DSPRX dsp_rx = console.dsp.GetDSPRX(i, j);
                        dsp_rx.Update = false;
                        dsp_rx.Force = true;
                        dsp_rx.Update = true;
                        dsp_rx.Force = false;
                    }
                }

                for (int i = 0; i < 1; i++)
                {
                    DSPTX dsp_tx = console.dsp.GetDSPTX(i);
                    dsp_tx.Update = false;
                    dsp_tx.Force = true;
                    dsp_tx.Update = true;
                    dsp_tx.Force = false;
                }
            }
            Display.lastvaluecount = 0;

            if (power && new_rate != old_rate)
            {
                if (console.CurrentModel == Model.FLEX5000 || console.CurrentModel == Model.FLEX3000)
                {
                    console.PowerOn = true;
                    Thread.Sleep(100);
                    Display.lastvaluecount = 0;

                    console.PowerOn = false;
                    Thread.Sleep(100);
                    Display.lastvaluecount = 0;

                    console.PowerOn = true;

                 
                }
                else console.PowerOn = true;

                // console.PowerOn = true;
            }

        } // comboAudioSampleRate1_




        private void comboAudioSampleRate2_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (comboAudioSampleRate2.SelectedIndex < 0) return;

            int old_rate = console.SampleRate2;
            int new_rate = Int32.Parse(comboAudioSampleRate2.Text);
            bool poweron = console.PowerOn;

            if (poweron && chkAudioEnableVAC.Checked && new_rate != old_rate)
            {
                console.PowerOn = false;
                Thread.Sleep(400);
            }

            console.SampleRate2 = new_rate;
            console.VACSampleRate = comboAudioSampleRate2.Text;

            if (poweron && chkAudioEnableVAC.Checked && new_rate != old_rate)
                console.PowerOn = true;
        }

        private void comboAudioSampleRate3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboAudioSampleRate3.SelectedIndex < 0) return;

            int old_rate = console.SampleRate3;
            int new_rate = Int32.Parse(comboAudioSampleRate3.Text);
            bool poweron = console.PowerOn;

            if (poweron && chkVAC2Enable.Checked && new_rate != old_rate)
            {
                console.PowerOn = false;
                Thread.Sleep(400);
            }

            console.SampleRate3 = new_rate;
            console.VAC2SampleRate = comboAudioSampleRate3.Text;

            if (poweron && chkVAC2Enable.Checked && new_rate != old_rate)
                console.PowerOn = true;
        }

        private void comboAudioBuffer1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (comboAudioBuffer1.SelectedIndex < 0) return;

            int old_size = console.BlockSize1;
            int new_size = Int32.Parse(comboAudioBuffer1.Text);
            bool power = console.PowerOn;

            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                case Model.FLEX3000:
                    if (!console.fwc_init) return;

                    try
                    {
                        Pal.SetBufferSize((uint)new_size);
                    }
                    catch (Exception)
                    {
                        // ignore exceptions in case version of PAL dll doesn't support this function
                    }

                    CWKeyer.AudioLatency = Math.Max(10.0, new_size / (double)console.SampleRate1 * 1e3); // 2048 / 96000 * 1000 = 
                    break;
                case Model.FLEX1500:
                    Flex1500.SetAudioBuffer((uint)new_size);
                    CWKeyer.AudioLatency = Math.Max(12.0, new_size / (double)console.SampleRate1 * 1e3);
                    break;
            }

            if (power && old_size != new_size)
            {
                console.PowerOn = false;
                Thread.Sleep(500);
            }

            console.BlockSize1 = new_size;

            //DSP.KeyerResetSize = console.BlockSize1*3/2;

            if (power && old_size != new_size) console.PowerOn = true;
        }

        private void comboAudioBuffer2_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (comboAudioBuffer2.SelectedIndex < 0) return;

            int old_size = console.BlockSize2;
            int new_size = Int32.Parse(comboAudioBuffer2.Text);
            bool power = console.PowerOn;

            if (power && chkAudioEnableVAC.Checked && old_size != new_size)
            {
                console.PowerOn = false;
                Thread.Sleep(500);
            }

            console.BlockSize2 = new_size;

            if (power && chkAudioEnableVAC.Checked && old_size != new_size)
                console.PowerOn = true;
        }

        private void comboAudioBuffer3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboAudioBuffer3.SelectedIndex < 0) return;

            int old_size = console.BlockSize3;
            int new_size = Int32.Parse(comboAudioBuffer3.Text);
            bool power = console.PowerOn;

            if (power && chkVAC2Enable.Checked && old_size != new_size)
            {
                console.PowerOn = false;
                Thread.Sleep(500);
            }

            console.BlockSize3 = new_size;

            if (power && chkVAC2Enable.Checked && old_size != new_size)
                console.PowerOn = true;
        }

        private void udAudioLatency1_ValueChanged(object sender, System.EventArgs e)
        {
            bool power = console.PowerOn;
            if (power)
            {
                console.PowerOn = false;
                Thread.Sleep(500);
            }

            Audio.Latency1 = (int)udAudioLatency1.Value;

            if (power) console.PowerOn = true;
        }

        private void udAudioLatency2_ValueChanged(object sender, System.EventArgs e)
        {
            string vac_driver_name = ((PADeviceInfo)comboAudioDriver2.SelectedItem).Name;


            if (((vac_driver_name != "Windows WDM-KS") && (vac_driver_name != "ASIO")) && udAudioLatency2.Value < 50)
            {
                MessageBox.Show(new Form { TopMost = true }, "The VAC1 Buffer Latency value selected is less than 120ms which is too " +
                    "low for the MME and DirectSound VAC audio drivers.  Buffer Latency values less than " +
                    "120ms are only valid when using the WDM-KS VAC audio driver.\n\n" +
                    "The VAC1 Buffer Latency has been reset to the default of 120ms.  " +
                    "Make sure to save your Transmit profile to make the change persistent.",
                    "Invalid VAC1 Buffer Latency Value",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                udAudioLatency2.Value = 120;
            }

            if (udAudioLatency2.Value <= 15)
            {
                udAudioLatency2.Value = 15;
            }

            bool power = console.PowerOn;
            if (power && chkAudioEnableVAC.Checked)
            {
                console.PowerOn = false;
                Thread.Sleep(500);
            }

            Audio.Latency2 = (int)udAudioLatency2.Value;

            if (power && chkAudioEnableVAC.Checked)
                console.PowerOn = true;
        }

        private void udVAC2Latency_ValueChanged(object sender, System.EventArgs e)
        {
            string vac_driver_name = ((PADeviceInfo)comboAudioDriver3.SelectedItem).Name;

            if (chkVAC2Enable.Checked)
            {
                if (((vac_driver_name != "Windows WDM-KS") && (vac_driver_name != "ASIO")) && udVAC2Latency.Value < 50)
                {
                    MessageBox.Show(new Form { TopMost = true }, "The VAC2 Buffer Latency value selected is less than 120ms which is too " +
                        "low for the MME and DirectSound VAC audio drivers.  Buffer Latency values less than " +
                        "120ms are only valid when using the WDM-KS VAC audio driver.\n\n" +
                        "The VAC2 Buffer Latency has been reset to the default of 120ms.  " +
                        "Make sure to save your Transmit profile to make the change persistent.",
                        "Invalid VAC2 Buffer Latency Value",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    udVAC2Latency.Value = 120;
                }

                if (udVAC2Latency.Value <= 15)
                {
                    udVAC2Latency.Value = 15;
                }
            }


            bool power = console.PowerOn;
            if (power && chkVAC2Enable.Checked)
            {
                console.PowerOn = false;
                Thread.Sleep(500);
            }

            Audio.Latency3 = (int)udVAC2Latency.Value;

            if (power && chkVAC2Enable.Checked)
                console.PowerOn = true;
        }

        private void udAudioVoltage1_ValueChanged(object sender, System.EventArgs e)
        {
            if (udAudioVoltage1.Focused &&
                comboAudioSoundCard.SelectedIndex > 0 &&
                current_sound_card != SoundCard.UNSUPPORTED_CARD)
            {
                DialogResult dr = MessageBox.Show(new Form { TopMost = true }, "Are you sure you want to change the Max RMS Voltage for this \n" +
                    "supported sound card?  The largest measured difference in supported cards \n" +
                    "was 40mV.  Note that we will only allow a 100mV difference from our measured default.",
                    "Change Voltage?",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);
                if (dr == DialogResult.No)
                {
                    udAudioVoltage1.Value = (decimal)console.AudioVolts1;
                    return;
                }
            }
            /*double def_volt = 0.0;
			switch(current_sound_card)
			{
				case SoundCard.SANTA_CRUZ:
					def_volt = 1.27;
					break;
				case SoundCard.AUDIGY:
				case SoundCard.AUDIGY_2:
				case SoundCard.AUDIGY_2_ZS:
					def_volt = 2.23;
					break;
				case SoundCard.EXTIGY:
					def_volt = 1.96;
					break;
				case SoundCard.MP3_PLUS:
					def_volt = 0.98;
					break;
				case SoundCard.DELTA_44:
					def_volt = 0.98;
					break;
				case SoundCard.FIREBOX:
					def_volt = 6.39;
					break;
			}

			if(current_sound_card != SoundCard.UNSUPPORTED_CARD)
			{
				if(Math.Abs(def_volt - (double)udAudioVoltage1.Value) > 0.1)
				{
					udAudioVoltage1.Value = (decimal)def_volt;
					return;
				}
			}*/
            console.AudioVolts1 = (double)udAudioVoltage1.Value;
        }

        private void chkAudio2Stereo_CheckedChanged(object sender, System.EventArgs e)
        {
            bool power = console.PowerOn;
            if (power && chkAudioEnableVAC.Checked)
            {
                console.PowerOn = false;
                Thread.Sleep(500);
            }

            console.SecondSoundCardStereo = chkAudio2Stereo.Checked;
            console.VACStereo = chkAudio2Stereo.Checked;
            chkVACCombine.Enabled = chkAudio2Stereo.Checked;

            if (power && chkAudioEnableVAC.Checked)
                console.PowerOn = true;
        }

        private void chkAudioStereo3_CheckedChanged(object sender, EventArgs e)
        {
            bool power = console.PowerOn;
            if (power && chkVAC2Enable.Checked)
            {
                console.PowerOn = false;
                Thread.Sleep(500);
            }

            console.ThirdSoundCardStereo = chkAudioStereo3.Checked;
            console.VAC2Stereo = chkAudioStereo3.Checked;
            chkVAC2Combine.Enabled = chkAudioStereo3.Checked;

            if (power && chkVAC2Enable.Checked)
                console.PowerOn = true;
        }

        private void udAudioVACGainRX_ValueChanged(object sender, System.EventArgs e)
        {
            Audio.VACRXScale = Math.Pow(10.0, (int)udAudioVACGainRX.Value / 20.0);
            console.VACRXGain = (int)udAudioVACGainRX.Value;
        }

        private void udVAC2GainRX_ValueChanged(object sender, EventArgs e)
        {
            Audio.VAC2RXScale = Math.Pow(10.0, (int)udVAC2GainRX.Value / 20.0);
            console.VAC2RXGain = (int)udVAC2GainRX.Value;
        }

        private void udAudioVACGainTX_ValueChanged(object sender, System.EventArgs e)
        {
            Audio.VACPreamp = Math.Pow(10.0, (int)udAudioVACGainTX.Value / 20.0);
            console.VACTXGain = (int)udAudioVACGainTX.Value;
        }

        private void udVAC2GainTX_ValueChanged(object sender, EventArgs e)
        {
            Audio.VAC2TXScale = Math.Pow(10.0, (int)udVAC2GainTX.Value / 20.0);
            console.VAC2TXGain = (int)udVAC2GainTX.Value;
        }

        private void chkAudioVACAutoEnable_CheckedChanged(object sender, System.EventArgs e)
        {
            console.VACAutoEnable = chkAudioVACAutoEnable.Checked;
        }

        private void chkVAC2AutoEnable_CheckedChanged(object sender, EventArgs e)
        {
            console.VAC2AutoEnable = chkVAC2AutoEnable.Checked;
        }

        private void udAudioLineIn1_ValueChanged(object sender, System.EventArgs e)
        {
            Mixer.SetLineInRecordVolume(comboAudioMixer1.SelectedIndex, (int)udAudioLineIn1.Value);
        }

        private void udAudioMicGain1_ValueChanged(object sender, System.EventArgs e)
        {
            Mixer.SetMicRecordVolume(comboAudioMixer1.SelectedIndex, (int)udAudioMicGain1.Value);
        }

        private void chkAudioLatencyManual1_CheckedChanged(object sender, System.EventArgs e)
        {
            bool power = console.PowerOn;
            if (power)
            {
                console.PowerOn = false;
                Thread.Sleep(500);
            }

            udAudioLatency1.Enabled = chkAudioLatencyManual1.Checked;

            if (!chkAudioLatencyManual1.Checked)
            {
                if (comboAudioDriver1.Text == "MME" || comboAudioDriver1.Text == "DirectSound")
                    Audio.Latency1 = 50;
                else Audio.Latency1 = 0;
            }

            if (power) console.PowerOn = true;
        }

        private void chkAudioLatencyManual2_CheckedChanged(object sender, System.EventArgs e)
        {
            bool power = console.PowerOn;
            if (power && chkAudioEnableVAC.Checked)
            {
                console.PowerOn = false;
                Thread.Sleep(500);
            }

            udAudioLatency2.Enabled = chkAudioLatencyManual2.Checked;

            if (!chkAudioLatencyManual2.Checked)
                Audio.Latency2 = 120;

            if (power && chkAudioEnableVAC.Checked)
                console.PowerOn = true;
        }

        private void chkVAC2LatencyManual_CheckedChanged(object sender, EventArgs e)
        {
            bool power = console.PowerOn;
            if (power && chkVAC2Enable.Checked)
            {
                console.PowerOn = false;
                Thread.Sleep(500);
            }

            udVAC2Latency.Enabled = chkVAC2LatencyManual.Checked;

            if (!chkVAC2LatencyManual.Checked)
                Audio.Latency3 = 120;

            if (power && chkVAC2Enable.Checked)
                console.PowerOn = true;
        }

        private void chkAudioMicBoost_CheckedChanged(object sender, System.EventArgs e)
        {
            console.MicBoost = chkAudioMicBoost.Checked;
        }

        private void btnAudioVoltTest1_Click(object sender, System.EventArgs e)
        {
            sound_card = 1;

            DialogResult dr = MessageBox.Show(
                "Is the Line Out Cable unplugged?  Running this test with the plug in the \n" +
                "normal position plugged into the SDR-1000 could cause damage to the device.",
                "Warning: Cable unplugged from SDR-1000?",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (dr == DialogResult.No) return;

            progress = new Progress("Calibrate Sound Card");
            if (console.PowerOn)
                progress.Show();

            Thread t = new Thread(new ThreadStart(CalibrateSoundCard));
            t.Name = "Sound Card Calibration Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.AboveNormal;
            t.Start();
        }

        private void CalibrateSoundCard()
        {
            bool done = console.CalibrateSoundCard(progress, sound_card);
            if (done) MessageBox.Show(new Form { TopMost = true }, "Sound Card Calibration complete.");
        }

        private void FireBoxMixerFix()
        {
            try
            {
                Process p = Process.Start("c:\\Program Files\\PreSonus\\1394AudioDriver_FIREBox\\FireBox Mixer.exe");
                Thread.Sleep(2000);
                p.Kill();
            }
            catch (Exception)
            {

            }
        }

        private void comboAudioSoundCard_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            Debug.WriteLine("SOUND CARD TYPE " + comboAudioSoundCard.Text);

            if (comboAudioSoundCard.SelectedIndex < 0) return;

            Debug.WriteLine("SOUND CARD TYPE1 " + comboAudioSoundCard.Text);

            bool on = console.PowerOn;

            if (on)
            {
                console.PowerOn = false;
                Thread.Sleep(500);
            }

            SoundCard card = SoundCard.FIRST;
            switch (comboAudioSoundCard.Text)
            {
                case "M-Audio Delta 44 (PCI)":
                    card = SoundCard.DELTA_44;
                    break;
                case "PreSonus FireBox (FireWire)":
                    card = SoundCard.FIREBOX;
                    break;
                case "Edirol FA-66 (FireWire)":
                    card = SoundCard.EDIROL_FA_66;
                    break;
                case "SB Audigy (PCI)":
                    card = SoundCard.AUDIGY;
                    break;
                case "SB Audigy 2 (PCI)":
                    card = SoundCard.AUDIGY_2;
                    break;
                case "SB Audigy 2 ZS (PCI)":
                    card = SoundCard.AUDIGY_2_ZS;
                    break;
                case "Sound Blaster Extigy (USB)":
                    card = SoundCard.EXTIGY;
                    break;
                case "Sound Blaster MP3+ (USB)":
                    card = SoundCard.MP3_PLUS;
                    break;
                case "Turtle Beach Santa Cruz (PCI)":
                    card = SoundCard.SANTA_CRUZ;
                    break;
                case "Unsupported Card":
                    card = SoundCard.UNSUPPORTED_CARD;
                    break;
            }

            if (card == SoundCard.FIRST) return;

            console.CurrentSoundCard = card;
            current_sound_card = card;

            switch (card)
            {
                case SoundCard.SANTA_CRUZ:
                    grpAudioDetails1.Enabled = false;
                    grpAudioVolts1.Visible = chkAudioExpert.Checked;
                    udAudioVoltage1.Value = 1.274M;
                    if (comboAudioSampleRate1.Items.Contains(96000)) comboAudioSampleRate1.Items.Remove(96000);
                    if (comboAudioSampleRate1.Items.Contains(192000)) comboAudioSampleRate1.Items.Remove(192000);
                    comboAudioSampleRate1.Text = "48000";



                    foreach (PADeviceInfo p in comboAudioDriver1.Items)
                    {
                        if (p.Name == "ASIO")
                        {
                            comboAudioDriver1.SelectedItem = p;
                            break;
                        }
                    }

                    foreach (PADeviceInfo dev in comboAudioInput1.Items)
                    {
                        if (dev.Name == "Wuschel's ASIO4ALL")
                        {
                            comboAudioInput1.Text = "Wuschel's ASIO4ALL";
                            comboAudioOutput1.Text = "Wuschel's ASIO4ALL";
                        }
                    }
                    if (comboAudioInput1.Text != "Wuschel's ASIO4ALL")
                    {
                        foreach (PADeviceInfo dev in comboAudioInput1.Items)
                        {
                            if (dev.Name == "ASIO4ALL v2")
                            {
                                comboAudioInput1.Text = "ASIO4ALL v2";
                                comboAudioOutput1.Text = "ASIO4ALL v2";
                            }
                        }
                    }

                    comboAudioMixer1.Text = "Santa Cruz(tm)";
                    comboAudioReceive1.Text = "Line In";

                    for (int i = 0; i < comboAudioTransmit1.Items.Count; i++)
                    {
                        if (((string)comboAudioTransmit1.Items[i]).StartsWith("Mi"))
                        {
                            comboAudioTransmit1.SelectedIndex = i;
                            break;
                        }
                    }

                    if (comboAudioMixer1.SelectedIndex < 0 ||
                        comboAudioMixer1.Text != "Santa Cruz(tm)")
                    {
                        MessageBox.Show(comboAudioSoundCard.Text + " not found.\n " +
                            "Please verify that this specific sound card is installed " +
                            "and functioning and try again.  \nIf your sound card is not " +
                            "a " + comboAudioSoundCard.Text + " and your card is not in the " +
                            "list, use the Unsupported Card selection.  \nFor more support, " +
                            "email support@flexradio.com.",
                            comboAudioSoundCard.Text + " Not Found",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                        console.PowerEnabled = false;
                    }
                    else if (!Mixer.InitSantaCruz(console.MixerID1))
                    {
                        MessageBox.Show(new Form { TopMost = true }, "The " + comboAudioSoundCard.Text + " mixer initialization " +
                            "failed.  Please install the latest drivers from www.turtlebeach.com " +
                            " and try again.  For more support, email support@flexradio.com.",
                            comboAudioSoundCard.Text + " Mixer Initialization Failed",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                        console.PowerEnabled = false;
                    }
                    else if (comboAudioInput1.Text != "ASIO4ALL v2" && comboAudioInput1.Text != "Wuschel's ASIO4ALL")
                    {
                        MessageBox.Show(new Form { TopMost = true }, "ASIO4ALL driver not found.  Please visit " +
                            "www.asio4all.com, download and install the driver, " +
                            "and try again.  Alternatively, you can use the Unsupported " +
                            "Card selection and setup the sound interface manually.  For " +
                            "more support, email support@flexradio.com.",
                            "ASIO4ALL Not Found",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                        console.PowerEnabled = false;
                    }
                    else
                    {
                        udAudioLineIn1.Value = 20;
                        console.PowerEnabled = true;
                        grpAudioMicInGain1.Enabled = true;
                        grpAudioLineInGain1.Enabled = true;
                        comboAudioChannels1.Text = "2";
                        comboAudioChannels1.Enabled = false;
                        Audio.IN_RX1_L = 0;
                        Audio.IN_RX1_R = 1;
                        Audio.IN_TX_L = 0;
                        Audio.IN_TX_R = 1;
                    }
                    break;
                case SoundCard.AUDIGY:
                case SoundCard.AUDIGY_2:
                    grpAudioDetails1.Enabled = false;
                    grpAudioVolts1.Visible = chkAudioExpert.Checked;
                    udAudioVoltage1.Value = 2.23M;
                    if (comboAudioSampleRate1.Items.Contains(96000))
                        comboAudioSampleRate1.Items.Remove(96000);
                    if (comboAudioSampleRate1.Items.Contains(192000))
                        comboAudioSampleRate1.Items.Remove(192000);
                    comboAudioSampleRate1.Text = "48000";

                    /*
					foreach(PADeviceInfo p in comboAudioDriver1.Items)
					{
						if(p.Name == "ASIO")
						{
							comboAudioDriver1.SelectedItem = p;
							break;
						}
					}
					
					foreach(PADeviceInfo dev in comboAudioInput1.Items)
					{
						if(dev.Name == "Wuschel's ASIO4ALL")
						{
							comboAudioInput1.Text = "Wuschel's ASIO4ALL";
							comboAudioOutput1.Text = "Wuschel's ASIO4ALL";
						}
					}
					if(comboAudioInput1.Text != "Wuschel's ASIO4ALL")
					{
						foreach(PADeviceInfo dev in comboAudioInput1.Items)
						{
							if(dev.Name == "ASIO4ALL v2")
							{
								comboAudioInput1.Text = "ASIO4ALL v2";
								comboAudioOutput1.Text = "ASIO4ALL v2";
							}
						}
					}

                */
                    for (int i = 0; i < comboAudioMixer1.Items.Count; i++)
                    {
                        if (((string)comboAudioMixer1.Items[i]).StartsWith("SB Audigy"))
                        {
                            comboAudioMixer1.SelectedIndex = i;
                            break;
                        }
                    }

                    for (int i = 0; i < comboAudioReceive1.Items.Count; i++)
                    {
                        if (((string)comboAudioReceive1.Items[i]).StartsWith("Analog"))
                        {
                            comboAudioReceive1.SelectedIndex = i;
                            break;
                        }
                    }

                    if (comboAudioReceive1.SelectedIndex < 0 ||
                        !comboAudioReceive1.Text.StartsWith("Analog"))
                    {
                        for (int i = 0; i < comboAudioReceive1.Items.Count; i++)
                        {
                            if (((string)comboAudioReceive1.Items[i]).StartsWith("Mix ana"))
                            {
                                comboAudioReceive1.SelectedIndex = i;
                                break;
                            }
                        }
                    }

                    for (int i = 0; i < comboAudioTransmit1.Items.Count; i++)
                    {
                        if (((string)comboAudioTransmit1.Items[i]).StartsWith("Mi"))
                        {
                            comboAudioTransmit1.SelectedIndex = i;
                            break;
                        }
                    }

                    if (comboAudioMixer1.SelectedIndex < 0 ||
                        !comboAudioMixer1.Text.StartsWith("SB Audigy"))
                    {
                        MessageBox.Show(comboAudioSoundCard.Text + " not found.\n " +
                            "Please verify that this specific sound card is installed " +
                            "and functioning and try again.  \nIf your sound card is not " +
                            "a " + comboAudioSoundCard.Text + " and your card is not in the " +
                            "list, use the Unsupported Card selection.  \nFor more support, " +
                            "email support@flexradio.com.",
                            comboAudioSoundCard.Text + " Not Found",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                        console.PowerEnabled = false;
                    }
                    else if (!Mixer.InitAudigy2(console.MixerID1))
                    {
                        MessageBox.Show(new Form { TopMost = true }, "The " + comboAudioSoundCard.Text + " mixer initialization " +
                            "failed.  Please install the latest drivers from www.creativelabs.com " +
                            " and try again.  For more support, email support@flexradio.com.",
                            comboAudioSoundCard.Text + " Mixer Initialization Failed",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                        console.PowerEnabled = false;
                    }
                    else if (comboAudioInput1.Text != "ASIO4ALL v2" &&
                        comboAudioInput1.Text != "Wuschel's ASIO4ALL")
                    {
                        MessageBox.Show(new Form { TopMost = true }, "ASIO4ALL driver not found.  Please visit " +
                            "www.asio4all.com, download and install the driver, " +
                            "and try again.  Alternatively, you can use the Unsupported " +
                            "Card selection and setup the sound interface manually.  For " +
                            "more support, email support@flexradio.com.",
                            "ASIO4ALL Not Found",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                        console.PowerEnabled = false;
                    }
                    else
                    {
                        udAudioLineIn1.Value = 1;
                        console.PowerEnabled = true;
                        grpAudioMicInGain1.Enabled = true;
                        grpAudioLineInGain1.Enabled = true;
                        comboAudioChannels1.Text = "2";
                        comboAudioChannels1.Enabled = false;
                        Audio.IN_RX1_L = 0;
                        Audio.IN_RX1_R = 1;
                        Audio.IN_TX_L = 0;
                        Audio.IN_TX_R = 1;
                    }
                    break;
                case SoundCard.AUDIGY_2_ZS:
                    grpAudioDetails1.Enabled = false;
                    grpAudioVolts1.Visible = chkAudioExpert.Checked;
                    udAudioVoltage1.Value = 2.23M;
                    if (comboAudioSampleRate1.Items.Contains(96000))
                        comboAudioSampleRate1.Items.Remove(96000);
                    if (comboAudioSampleRate1.Items.Contains(192000))
                        comboAudioSampleRate1.Items.Remove(192000);
                    comboAudioSampleRate1.Text = "48000";

                    /*

					foreach(PADeviceInfo p in comboAudioDriver1.Items)
					{
						if(p.Name == "ASIO")
						{
							comboAudioDriver1.SelectedItem = p;
							break;
						}
					}
					
					foreach(PADeviceInfo dev in comboAudioInput1.Items)
					{
						if(dev.Name == "Wuschel's ASIO4ALL")
						{
							comboAudioInput1.Text = "Wuschel's ASIO4ALL";
							comboAudioOutput1.Text = "Wuschel's ASIO4ALL";
						}
					}
					if(comboAudioInput1.Text != "Wuschel's ASIO4ALL")
					{
						foreach(PADeviceInfo dev in comboAudioInput1.Items)
						{
							if(dev.Name == "ASIO4ALL v2")
							{
								comboAudioInput1.Text = "ASIO4ALL v2";
								comboAudioOutput1.Text = "ASIO4ALL v2";
							}
						}
					}

                */
                    for (int i = 0; i < comboAudioMixer1.Items.Count; i++)
                    {
                        if (((string)comboAudioMixer1.Items[i]).StartsWith("SB Audigy"))
                        {
                            comboAudioMixer1.SelectedIndex = i;
                            break;
                        }
                    }

                    for (int i = 0; i < comboAudioReceive1.Items.Count; i++)
                    {
                        if (((string)comboAudioReceive1.Items[i]).StartsWith("Analog"))
                        {
                            comboAudioReceive1.SelectedIndex = i;
                            break;
                        }
                    }

                    if (comboAudioReceive1.SelectedIndex < 0 ||
                        !comboAudioReceive1.Text.StartsWith("Analog"))
                    {
                        for (int i = 0; i < comboAudioReceive1.Items.Count; i++)
                        {
                            if (((string)comboAudioReceive1.Items[i]).StartsWith("Mix ana"))
                            {
                                comboAudioReceive1.SelectedIndex = i;
                                break;
                            }
                        }
                    }

                    for (int i = 0; i < comboAudioTransmit1.Items.Count; i++)
                    {
                        if (((string)comboAudioTransmit1.Items[i]).StartsWith("Mi"))
                        {
                            comboAudioTransmit1.SelectedIndex = i;
                            break;
                        }
                    }

                    if (comboAudioMixer1.SelectedIndex < 0 ||
                        !comboAudioMixer1.Text.StartsWith("SB Audigy"))
                    {
                        MessageBox.Show(comboAudioSoundCard.Text + " not found.\n " +
                            "Please verify that this specific sound card is installed " +
                            "and functioning and try again.  \nIf your sound card is not " +
                            "a " + comboAudioSoundCard.Text + " and your card is not in the " +
                            "list, use the Unsupported Card selection.  \nFor more support, " +
                            "email support@flexradio.com.",
                            comboAudioSoundCard.Text + " Not Found",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                        console.PowerEnabled = false;
                    }
                    else if (!Mixer.InitAudigy2ZS(console.MixerID1))
                    {
                        MessageBox.Show(new Form { TopMost = true }, "The " + comboAudioSoundCard.Text + " mixer initialization " +
                            "failed.  Please install the latest drivers from www.creativelabs.com " +
                            " and try again.  For more support, email support@flexradio.com.",
                            comboAudioSoundCard.Text + " Mixer Initialization Failed",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                        console.PowerEnabled = false;
                    }
                    else if (comboAudioInput1.Text != "ASIO4ALL v2" &&
                        comboAudioInput1.Text != "Wuschel's ASIO4ALL")
                    {
                        MessageBox.Show(new Form { TopMost = true }, "ASIO4ALL driver not found.  Please visit " +
                            "www.asio4all.com, download and install the driver, " +
                            "and try again.  Alternatively, you can use the Unsupported " +
                            "Card selection and setup the sound interface manually.  For " +
                            "more support, email support@flexradio.com.",
                            "ASIO4ALL Not Found",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                        console.PowerEnabled = false;
                    }
                    else
                    {
                        udAudioLineIn1.Value = 1;
                        console.PowerEnabled = true;
                        grpAudioMicInGain1.Enabled = true;
                        grpAudioLineInGain1.Enabled = true;
                        comboAudioChannels1.Text = "2";
                        comboAudioChannels1.Enabled = false;
                        Audio.IN_RX1_L = 0;
                        Audio.IN_RX1_R = 1;
                        Audio.IN_TX_L = 0;
                        Audio.IN_TX_R = 1;
                    }
                    break;
                case SoundCard.EXTIGY:
                    grpAudioDetails1.Enabled = false;
                    grpAudioVolts1.Visible = chkAudioExpert.Checked;
                    udAudioVoltage1.Value = 1.96M;
                    if (!comboAudioSampleRate1.Items.Contains(96000)) comboAudioSampleRate1.Items.Add(96000);
                    if (comboAudioSampleRate1.Items.Contains(192000)) comboAudioSampleRate1.Items.Remove(192000);

                    comboAudioSampleRate1.Text = "48000";
                    foreach (PADeviceInfo p in comboAudioDriver1.Items)
                    {
                        if (p.Name == "ASIO")
                        {
                            comboAudioDriver1.SelectedItem = p;
                            break;
                        }
                    }

                    foreach (PADeviceInfo dev in comboAudioInput1.Items)
                    {
                        if (dev.Name == "Wuschel's ASIO4ALL")
                        {
                            comboAudioInput1.Text = "Wuschel's ASIO4ALL";
                            comboAudioOutput1.Text = "Wuschel's ASIO4ALL";
                        }
                    }
                    if (comboAudioInput1.Text != "Wuschel's ASIO4ALL")
                    {
                        foreach (PADeviceInfo dev in comboAudioInput1.Items)
                        {
                            if (dev.Name == "ASIO4ALL v2")
                            {
                                comboAudioInput1.Text = "ASIO4ALL v2";
                                comboAudioOutput1.Text = "ASIO4ALL v2";
                            }
                        }
                    }

                    for (int i = 0; i < comboAudioMixer1.Items.Count; i++)
                    {
                        if (((string)comboAudioMixer1.Items[i]).StartsWith("Creative SB Extigy"))
                        {
                            comboAudioMixer1.SelectedIndex = i;
                            break;
                        }
                    }

                    comboAudioReceive1.Text = "Line In";
                    comboAudioTransmit1.Text = "Microphone";

                    if (comboAudioMixer1.SelectedIndex < 0 ||
                        comboAudioMixer1.Text != "Creative SB Extigy")
                    {
                        MessageBox.Show(comboAudioSoundCard.Text + " not found.\n " +
                            "Please verify that this specific sound card is installed " +
                            "and functioning and try again.  \nIf your sound card is not " +
                            "a " + comboAudioSoundCard.Text + " and your card is not in the " +
                            "list, use the Unsupported Card selection.  \nFor more support, " +
                            "email support@flexradio.com.",
                            comboAudioSoundCard.Text + " Not Found",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                        console.PowerEnabled = false;
                    }
                    else if (!Mixer.InitExtigy(console.MixerID1))
                    {
                        MessageBox.Show(new Form { TopMost = true }, "The " + comboAudioSoundCard.Text + " mixer initialization " +
                            "failed.  Please install the latest drivers from www.creativelabs.com " +
                            " and try again.  For more support, email support@flexradio.com.",
                            comboAudioSoundCard.Text + " Mixer Initialization Failed",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                        console.PowerEnabled = false;
                    }
                    else if (comboAudioInput1.Text != "ASIO4ALL v2" &&
                        comboAudioInput1.Text != "Wuschel's ASIO4ALL")
                    {
                        MessageBox.Show(new Form { TopMost = true }, "ASIO4ALL driver not found.  Please visit " +
                            "www.asio4all.com, download and install the driver, " +
                            "and try again.  Alternatively, you can use the Unsupported " +
                            "Card selection and setup the sound interface manually.  For " +
                            "more support, email support@flexradio.com.",
                            "ASIO4ALL Not Found",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                        console.PowerEnabled = false;
                    }
                    else
                    {
                        udAudioLineIn1.Value = 20;
                        console.PowerEnabled = true;
                        grpAudioMicInGain1.Enabled = true;
                        grpAudioLineInGain1.Enabled = true;
                        comboAudioChannels1.Text = "2";
                        comboAudioChannels1.Enabled = false;
                        Audio.IN_RX1_L = 0;
                        Audio.IN_RX1_R = 1;
                        Audio.IN_TX_L = 0;
                        Audio.IN_TX_R = 1;
                    }
                    break;
                case SoundCard.MP3_PLUS:
                    grpAudioDetails1.Enabled = false;
                    grpAudioVolts1.Visible = chkAudioExpert.Checked;
                    udAudioVoltage1.Value = 0.982M;
                    if (comboAudioSampleRate1.Items.Contains(96000)) comboAudioSampleRate1.Items.Remove(96000);
                    if (comboAudioSampleRate1.Items.Contains(192000)) comboAudioSampleRate1.Items.Remove(192000);
                    comboAudioSampleRate1.Text = "48000";
                    foreach (PADeviceInfo p in comboAudioDriver1.Items)
                    {
                        if (p.Name == "ASIO")
                        {
                            comboAudioDriver1.SelectedItem = p;
                            break;
                        }
                    }

                    for (int i = 0; i < comboAudioMixer1.Items.Count; i++)
                    {
                        if (((string)comboAudioMixer1.Items[i]).StartsWith("Sound Blaster"))
                        {
                            comboAudioMixer1.SelectedIndex = i;
                            break;
                        }
                    }

                    if (comboAudioMixer1.SelectedIndex < 0 ||
                        (string)comboAudioMixer1.SelectedItem != "Sound Blaster")
                    {
                        for (int i = 0; i < comboAudioMixer1.Items.Count; i++)
                        {
                            if (((string)comboAudioMixer1.Items[i]).StartsWith("USB Audio"))
                            {
                                comboAudioMixer1.SelectedIndex = i;
                                break;
                            }
                        }
                    }

                    foreach (PADeviceInfo dev in comboAudioInput1.Items)
                    {
                        if (dev.Name == "Wuschel's ASIO4ALL")
                        {
                            comboAudioInput1.Text = "Wuschel's ASIO4ALL";
                            comboAudioOutput1.Text = "Wuschel's ASIO4ALL";
                        }
                    }
                    if (comboAudioInput1.Text != "Wuschel's ASIO4ALL")
                    {
                        foreach (PADeviceInfo dev in comboAudioInput1.Items)
                        {
                            if (dev.Name == "ASIO4ALL v2")
                            {
                                comboAudioInput1.Text = "ASIO4ALL v2";
                                comboAudioOutput1.Text = "ASIO4ALL v2";
                            }
                        }
                    }

                    comboAudioReceive1.Text = "Line In";

                    for (int i = 0; i < comboAudioTransmit1.Items.Count; i++)
                    {
                        if (((string)comboAudioTransmit1.Items[i]).StartsWith("Mi"))
                        {
                            comboAudioTransmit1.SelectedIndex = i;
                            break;
                        }
                    }

                    if (comboAudioMixer1.SelectedIndex < 0 ||
                        (comboAudioMixer1.Text != "Sound Blaster" &&
                        comboAudioMixer1.Text != "USB Audio"))
                    {
                        MessageBox.Show(comboAudioSoundCard.Text + " not found.\n " +
                            "Please verify that this specific sound card is installed " +
                            "and functioning and try again.  \nIf your sound card is not " +
                            "a " + comboAudioSoundCard.Text + " and your card is not in the " +
                            "list, use the Unsupported Card selection.  \nFor more support, " +
                            "email support@flexradio.com.",
                            comboAudioSoundCard.Text + " Not Found",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                        console.PowerEnabled = false;
                    }
                    else if (!Mixer.InitMP3Plus(console.MixerID1))
                    {
                        MessageBox.Show(new Form { TopMost = true }, "The " + comboAudioSoundCard.Text + " mixer initialization " +
                            "failed.  Please install the latest drivers from www.creativelabs.com " +
                            " and try again.  For more support, email support@flexradio.com.",
                            comboAudioSoundCard.Text + " Mixer Initialization Failed",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                        console.PowerEnabled = false;
                    }
                    else if (comboAudioInput1.Text != "ASIO4ALL v2" &&
                        comboAudioInput1.Text != "Wuschel's ASIO4ALL")
                    {
                        MessageBox.Show(new Form { TopMost = true }, "ASIO4ALL driver not found.  Please visit " +
                            "www.asio4all.com, download and install the driver, " +
                            "and try again.  Alternatively, you can use the Unsupported " +
                            "Card selection and setup the sound interface manually.  For " +
                            "more support, email support@flexradio.com.",
                            "ASIO4ALL Not Found",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                        console.PowerEnabled = false;
                    }
                    else
                    {
                        udAudioLineIn1.Value = 6;
                        console.PowerEnabled = true;
                        grpAudioMicInGain1.Enabled = true;
                        grpAudioLineInGain1.Enabled = true;
                        comboAudioChannels1.Text = "2";
                        comboAudioChannels1.Enabled = false;
                        Audio.IN_RX1_L = 0;
                        Audio.IN_RX1_R = 1;
                        Audio.IN_TX_L = 0;
                        Audio.IN_TX_R = 1;
                    }
                    break;
                case SoundCard.DELTA_44:
                    grpAudioDetails1.Enabled = false;
                    grpAudioVolts1.Visible = chkAudioExpert.Checked;
                    udAudioVoltage1.Value = 0.98M;
                    if (!comboAudioSampleRate1.Items.Contains(96000)) comboAudioSampleRate1.Items.Add(96000);
                    if (comboAudioSampleRate1.Items.Contains(192000)) comboAudioSampleRate1.Items.Remove(192000);

                    if (comboAudioSoundCard.Focused || comboAudioSampleRate1.SelectedIndex < 0) comboAudioSampleRate1.Text = "48000";

                    foreach (PADeviceInfo p in comboAudioDriver1.Items)
                    {
                        if (p.Name == "ASIO")
                        {
                            comboAudioDriver1.SelectedItem = p;
                            break;
                        }
                    }

                    foreach (PADeviceInfo dev in comboAudioInput1.Items)
                    {
                        if (dev.Name == "M-Audio Delta ASIO")
                        {
                            comboAudioInput1.Text = "M-Audio Delta ASIO";
                            comboAudioOutput1.Text = "M-Audio Delta ASIO";
                        }
                    }

                    comboAudioMixer1.Text = "None";

                    if (comboAudioInput1.Text != "M-Audio Delta ASIO")
                    {
                        MessageBox.Show(new Form { TopMost = true }, "M-Audio Delta ASIO driver not found.  Please visit " +
                            "www.m-audio.com, download and install the latest driver, " +
                            "and try again.  For more support, email support@flexradio.com.",
                            "Delta 44 Driver Not Found",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                        console.PowerEnabled = false;
                    }
                    else
                    {
                        InitDelta44();
                        chkAudioEnableVAC.Enabled = true;
                        grpAudioMicInGain1.Enabled = false;
                        grpAudioLineInGain1.Enabled = false;
                        console.PowerEnabled = true;
                        comboAudioChannels1.Text = "4";
                        comboAudioChannels1.Enabled = false;
                        Audio.IN_RX1_L = 0;
                        Audio.IN_RX1_R = 1;
                        Audio.IN_TX_L = 2;
                        Audio.IN_TX_R = 3;
                    }
                    break;
                case SoundCard.FIREBOX:
                    grpAudioDetails1.Enabled = false;
                    grpAudioVolts1.Visible = chkAudioExpert.Checked;
                    udAudioVoltage1.Value = 6.39M;
                    if (!comboAudioSampleRate1.Items.Contains(96000)) comboAudioSampleRate1.Items.Add(96000);
                    if (comboAudioSampleRate1.Items.Contains(192000)) comboAudioSampleRate1.Items.Remove(192000);

                    if (comboAudioSoundCard.Focused || comboAudioSampleRate1.SelectedIndex < 0)
                        comboAudioSampleRate1.Text = "48000";

                    foreach (PADeviceInfo p in comboAudioDriver1.Items)
                    {
                        if (p.Name == "ASIO")
                        {
                            comboAudioDriver1.SelectedItem = p;
                            break;
                        }
                    }

                    foreach (PADeviceInfo dev in comboAudioInput1.Items)
                    {
                        if (dev.Name.IndexOf("FireBox") >= 0)
                        {
                            comboAudioInput1.Text = dev.Name;
                            comboAudioOutput1.Text = dev.Name;
                        }
                    }

                    comboAudioMixer1.Text = "None";

                    if (comboAudioInput1.Text.IndexOf("FireBox") < 0)
                    {
                        MessageBox.Show(new Form { TopMost = true }, "PreSonus FireBox ASIO driver not found.  Please visit " +
                            "www.presonus.com, download and install the latest driver, " +
                            "and try again.  For more support, email support@flexradio.com.",
                            "PreSonus FireBox Driver Not Found",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                        console.PowerEnabled = false;
                    }
                    else
                    {
                        chkAudioEnableVAC.Enabled = true;
                        grpAudioMicInGain1.Enabled = false;
                        grpAudioLineInGain1.Enabled = false;
                        console.PowerEnabled = true;
                        comboAudioChannels1.Text = "4";
                        comboAudioChannels1.Enabled = false;
                        Audio.IN_RX1_L = 2;
                        Audio.IN_RX1_R = 3;
                        Audio.IN_TX_L = 0;
                        Audio.IN_TX_R = 1;
                        Thread t = new Thread(new ThreadStart(FireBoxMixerFix));
                        t.Name = "FireBoxMixerFix";
                        t.Priority = ThreadPriority.Normal;
                        t.IsBackground = true;
                        t.Start();
                    }
                    break;
                case SoundCard.EDIROL_FA_66:
                    grpAudioDetails1.Enabled = false;
                    grpAudioVolts1.Visible = chkAudioExpert.Checked;
                    udAudioVoltage1.Value = 2.27M;
                    if (!comboAudioSampleRate1.Items.Contains(96000)) comboAudioSampleRate1.Items.Add(96000);
                    if (!comboAudioSampleRate1.Items.Contains(192000)) comboAudioSampleRate1.Items.Add(192000);

                    if (comboAudioSoundCard.Focused || comboAudioSampleRate1.SelectedIndex < 0) comboAudioSampleRate1.Text = "192000";

                    foreach (PADeviceInfo p in comboAudioDriver1.Items)
                    {
                        if (p.Name == "ASIO")
                        {
                            comboAudioDriver1.SelectedItem = p;
                            break;
                        }
                    }

                    foreach (PADeviceInfo dev in comboAudioInput1.Items)
                    {
                        if (dev.Name == "EDIROL FA-66")
                        {
                            comboAudioInput1.Text = "EDIROL FA-66";
                            comboAudioOutput1.Text = "EDIROL FA-66";
                        }
                    }

                    comboAudioMixer1.Text = "None";

                    if (comboAudioInput1.Text != "EDIROL FA-66")
                    {
                        MessageBox.Show(new Form { TopMost = true }, "Edirol FA-66 ASIO driver not found.  Please visit " +
                            "www.rolandus.com, download and install the latest driver, " +
                            "and try again.  For more support, email support@flexradio.com.",
                            "Edirol FA-66 Driver Not Found",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                        console.PowerEnabled = false;
                    }
                    else
                    {
                        chkAudioEnableVAC.Enabled = true;
                        grpAudioMicInGain1.Enabled = false;
                        grpAudioLineInGain1.Enabled = false;
                        console.PowerEnabled = true;
                        comboAudioChannels1.Text = "4";
                        comboAudioChannels1.Enabled = false;
                        Audio.IN_RX1_L = 2;
                        Audio.IN_RX1_R = 3;
                        Audio.IN_TX_L = 0;
                        Audio.IN_TX_R = 1;
                    }
                    break;
                case SoundCard.UNSUPPORTED_CARD:
                    if (comboAudioSoundCard.Focused)
                    {
                        MessageBox.Show(new Form { TopMost = true }, "Proper operation of the SDR-1000 depends on the use of a sound card that is\n" +
                            "officially recommended by FlexRadio Systems.  Refer to the Specifications page on\n" +
                            "www.flexradio.com to determine which sound cards are currently recommended.  Use only\n" +
                            "the specific model numbers stated on the website because other models within the same\n" +
                            "family may not work properly with the radio.  Officially supported sound cards may be\n" +
                            "updated on the website without notice.  If you have any question about the sound card\n" +
                            "you would like to use with the radio, please email support@flexradio.com or call us at\n" +
                            "+1 (512) 535-4713 ext 2.\n\n" +

                            "NO WARRANTY IS IMPLIED WHEN THE SDR-1000 IS USED WITH ANY SOUND CARD OTHER\n" +
                            "THAN THOSE CURRENTLY RECOMMENDED AS STATED ON THE FLEXRADIO SYSTEMS WEBSITE.\n" +
                            "UNSUPPORTED SOUND CARDS MAY OR MAY NOT WORK WITH THE SDR-1000.  USE OF\n" +
                            "UNSUPPORTED SOUND CARDS IS AT THE CUSTOMERS OWN RISK.",
                            "Warning: Unsupported Card",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                    }
                    grpAudioVolts1.Visible = true;

                    if (comboAudioSoundCard.Focused) chkGeneralRXOnly.Checked = true;

                    if (!comboAudioSampleRate1.Items.Contains(96000)) comboAudioSampleRate1.Items.Add(96000);
                    if (!comboAudioSampleRate1.Items.Contains(192000)) comboAudioSampleRate1.Items.Add(192000);
                    //---------------------------------------------------------


                    foreach (PADeviceInfo p in comboAudioDriver1.Items)
                    {
                        if (p.Name == "ASIO")
                        {
                            comboAudioDriver1.SelectedItem = p;
                            break;
                        }
                    }

                    foreach (PADeviceInfo dev in comboAudioInput1.Items)
                    {
                        if (dev.Name == "Wuschel's ASIO4ALL")
                        {
                            comboAudioInput1.Text = "Wuschel's ASIO4ALL";
                            comboAudioOutput1.Text = "Wuschel's ASIO4ALL";
                        }
                    }
                    if (comboAudioInput1.Text != "Wuschel's ASIO4ALL")
                    {
                        foreach (PADeviceInfo dev in comboAudioInput1.Items)
                        {
                            if (dev.Name == "ASIO4ALL v2")
                            {
                                comboAudioInput1.Text = "ASIO4ALL v2";
                                comboAudioOutput1.Text = "ASIO4ALL v2";
                            }
                        }
                    }


                    for (int i = 0; i < comboAudioTransmit1.Items.Count; i++)
                    {
                        if (((string)comboAudioTransmit1.Items[i]).StartsWith("Mi"))
                        {
                            comboAudioTransmit1.SelectedIndex = i;
                            break;
                        }
                    }











                    //-------------------------------------------------------

                    if (comboAudioSoundCard.Focused || comboAudioSampleRate1.SelectedIndex < 0) comboAudioSampleRate1.Text = "48000";
                    grpAudioDetails1.Enabled = true;
                    grpAudioMicInGain1.Enabled = true;
                    grpAudioLineInGain1.Enabled = true;
                    console.PowerEnabled = true;
                    comboAudioChannels1.Text = "2";
                    comboAudioChannels1.Enabled = true;
                    Audio.IN_RX1_L = 0;
                    Audio.IN_RX1_R = 1;
                    Audio.IN_TX_L = 0;
                    Audio.IN_TX_R = 1;
                    break;
            }

            console.PWR = console.PWR;
            console.AF = console.AF;
            if (on) console.PowerOn = true;
        }

        #endregion

        #region Display Tab Event Handlers
        // ======================================================
        // Display Tab Event Handlers
        // ======================================================

        private void udDisplayGridMax_LostFocus(object sender, System.EventArgs e)
        {
            Display.SpectrumGridMax = (int)udDisplayGridMax.Value;
        }

        private void udDisplayGridMax_Click(object sender, System.EventArgs e)
        {
            udDisplayGridMax_LostFocus(sender, e);
        }

        private void udDisplayGridMax_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            udDisplayGridMax_LostFocus(sender, new System.EventArgs());
        }

        private void udDisplayFPS_ValueChanged(object sender, System.EventArgs e)
        {
            console.DisplayFPS = (int)udDisplayFPS.Value;
            udDisplayAVGTime_ValueChanged(this, EventArgs.Empty);
        }

        private void udDisplayGridMax_ValueChanged(object sender, System.EventArgs e)
        {
            if (udDisplayGridMax.Value <= udDisplayGridMin.Value) udDisplayGridMax.Value = udDisplayGridMin.Value + 10;
            Display.SpectrumGridMax = (int)udDisplayGridMax.Value;

            Display.Gradient(Display.SpectrumGridMax, Display.SpectrumGridMin); // set new Gradient color scheme


            //  console.AutoPanScaleMax = (int)udDisplayGridMax.Value; // ke9ns add storage of original  value
        }

        private void udDisplayGridMin_ValueChanged(object sender, System.EventArgs e)
        {
            if (udDisplayGridMin.Value >= udDisplayGridMax.Value) udDisplayGridMin.Value = udDisplayGridMax.Value - 10;
            Display.SpectrumGridMin = (int)udDisplayGridMin.Value;

            Display.Gradient(Display.SpectrumGridMax, Display.SpectrumGridMin); // set new Gradient color scheme

            // console.AutoPanScaleMin = (int)udDisplayGridMin.Value; // ke9ns add storage of original  value
        }

        private void udDisplayGridStep_ValueChanged(object sender, System.EventArgs e)
        {
            Display.SpectrumGridStep = (int)udDisplayGridStep.Value;

            Display.Gradient(Display.SpectrumGridMax, Display.SpectrumGridMin); // set new Gradient color scheme

        }

        private void comboDisplayLabelAlign_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            switch (comboDisplayLabelAlign.Text)
            {
                case "Left":
                    Display.DisplayLabelAlign = DisplayLabelAlignment.LEFT;
                    break;
                case "Cntr":
                    Display.DisplayLabelAlign = DisplayLabelAlignment.CENTER;
                    break;
                case "Right":
                    Display.DisplayLabelAlign = DisplayLabelAlignment.RIGHT;
                    break;
                case "Auto":
                    Display.DisplayLabelAlign = DisplayLabelAlignment.AUTO;
                    break;
                case "S-Unit":
                    Display.DisplayLabelAlign = DisplayLabelAlignment.Sunit;
                    break;
                case "Off":
                    Display.DisplayLabelAlign = DisplayLabelAlignment.OFF;
                    break;
                default:
                    Display.DisplayLabelAlign = DisplayLabelAlignment.LEFT;
                    break;
            }
        }

        private void udDisplayPhasePts_ValueChanged(object sender, System.EventArgs e)
        {
            Display.PhaseNumPts = (int)udDisplayPhasePts.Value;
        }

        //=================================================================================
        // ke9ns  avg time value for both rx1 and rx2 panadapter and waterfall
        public void udDisplayAVGTime_ValueChanged(object sender, System.EventArgs e)
        {
            double display_time = 1 / (double)udDisplayFPS.Value;

            int buffersToAvg = (int)((float)udDisplayAVGTime.Value * 0.001 / display_time);

            Display.DisplayAvgBlocks = (int)Math.Max(2, buffersToAvg);
        }


        private void udDisplayMeterDelay_ValueChanged(object sender, System.EventArgs e)
        {
            console.MeterDelay = (int)udDisplayMeterDelay.Value;
        }

        private void udDisplayPeakText_ValueChanged(object sender, System.EventArgs e)
        {
            console.PeakTextDelay = (int)udDisplayPeakText.Value;
        }

        private void udDisplayCPUMeter_ValueChanged(object sender, System.EventArgs e)
        {
            console.CPUMeterDelay = (int)udDisplayCPUMeter.Value;
        }

        private void clrbtnWaterfallLow_Changed(object sender, System.EventArgs e)
        {
            Display.WaterfallLowColor = clrbtnWaterfallLow.Color;
        }

        public void udDisplayWaterfallLowLevel_ValueChanged(object sender, System.EventArgs e)
        {

            UpdateWaterfallBandInfo();

            switch (console.RX1Band)
            {
                case Band.B160M:
                    console.WaterfallLowThreshold160m = (float)udDisplayWaterfallLowLevel.Value;
                    Display.WaterfallLowThreshold = console.WaterfallLowThreshold160m;
                    break;
                case Band.B80M:
                    console.WaterfallLowThreshold80m = (float)udDisplayWaterfallLowLevel.Value;
                    Display.WaterfallLowThreshold = console.WaterfallLowThreshold80m;
                    break;
                case Band.B60M:
                    console.WaterfallLowThreshold60m = (float)udDisplayWaterfallLowLevel.Value;
                    Display.WaterfallLowThreshold = console.WaterfallLowThreshold60m;
                    break;
                case Band.B40M:
                    console.WaterfallLowThreshold40m = (float)udDisplayWaterfallLowLevel.Value;
                    Display.WaterfallLowThreshold = console.WaterfallLowThreshold40m;
                    break;
                case Band.B30M:
                    console.WaterfallLowThreshold30m = (float)udDisplayWaterfallLowLevel.Value;
                    Display.WaterfallLowThreshold = console.WaterfallLowThreshold30m;
                    break;
                case Band.B20M:
                    console.WaterfallLowThreshold20m = (float)udDisplayWaterfallLowLevel.Value;
                    Display.WaterfallLowThreshold = console.WaterfallLowThreshold20m;
                    break;
                case Band.B17M:
                    console.WaterfallLowThreshold17m = (float)udDisplayWaterfallLowLevel.Value;
                    Display.WaterfallLowThreshold = console.WaterfallLowThreshold17m;
                    break;
                case Band.B15M:
                    console.WaterfallLowThreshold15m = (float)udDisplayWaterfallLowLevel.Value;
                    Display.WaterfallLowThreshold = console.WaterfallLowThreshold15m;
                    break;
                case Band.B12M:
                    console.WaterfallLowThreshold12m = (float)udDisplayWaterfallLowLevel.Value;
                    Display.WaterfallLowThreshold = console.WaterfallLowThreshold12m;
                    break;
                case Band.B10M:
                    console.WaterfallLowThreshold10m = (float)udDisplayWaterfallLowLevel.Value;
                    Display.WaterfallLowThreshold = console.WaterfallLowThreshold10m;
                    break;
                case Band.B6M:
                    console.WaterfallLowThreshold6m = (float)udDisplayWaterfallLowLevel.Value;
                    Display.WaterfallLowThreshold = console.WaterfallLowThreshold6m;
                    break;
                case Band.WWV:
                    console.WaterfallLowThresholdWWV = (float)udDisplayWaterfallLowLevel.Value;
                    Display.WaterfallLowThreshold = console.WaterfallLowThresholdWWV;
                    break;
                case Band.GEN:
                    console.WaterfallLowThresholdGEN = (float)udDisplayWaterfallLowLevel.Value;
                    Display.WaterfallLowThreshold = console.WaterfallLowThresholdGEN;
                    break;

                // ke9ns add: .158 SWL
                case Band.BLMF:
                    console.WaterfallLowThresholdLMF = (float)udDisplayWaterfallLowLevel.Value;
                    Display.WaterfallLowThreshold = console.WaterfallLowThresholdLMF;
                    break;
                case Band.B120M:
                    console.WaterfallLowThreshold120m = (float)udDisplayWaterfallLowLevel.Value;
                    Display.WaterfallLowThreshold = console.WaterfallLowThreshold120m;
                    break;
                case Band.B90M:
                    console.WaterfallLowThreshold90m = (float)udDisplayWaterfallLowLevel.Value;
                    Display.WaterfallLowThreshold = console.WaterfallLowThreshold90m;
                    break;
                case Band.B61M:
                    console.WaterfallLowThreshold61m = (float)udDisplayWaterfallLowLevel.Value;
                    Display.WaterfallLowThreshold = console.WaterfallLowThreshold61m;
                    break;
                case Band.B49M:
                    console.WaterfallLowThreshold49m = (float)udDisplayWaterfallLowLevel.Value;
                    Display.WaterfallLowThreshold = console.WaterfallLowThreshold49m;
                    break;
                case Band.B41M:
                    console.WaterfallLowThreshold41m = (float)udDisplayWaterfallLowLevel.Value;
                    Display.WaterfallLowThreshold = console.WaterfallLowThreshold41m;
                    break;
                case Band.B31M:
                    console.WaterfallLowThreshold31m = (float)udDisplayWaterfallLowLevel.Value;
                    Display.WaterfallLowThreshold = console.WaterfallLowThreshold31m;
                    break;
                case Band.B25M:
                    console.WaterfallLowThreshold25m = (float)udDisplayWaterfallLowLevel.Value;
                    Display.WaterfallLowThreshold = console.WaterfallLowThreshold25m;
                    break;
                case Band.B22M:
                    console.WaterfallLowThreshold22m = (float)udDisplayWaterfallLowLevel.Value;
                    Display.WaterfallLowThreshold = console.WaterfallLowThreshold22m;
                    break;
                case Band.B19M:
                    console.WaterfallLowThreshold19m = (float)udDisplayWaterfallLowLevel.Value;
                    Display.WaterfallLowThreshold = console.WaterfallLowThreshold19m;
                    break;
                case Band.B16M:
                    console.WaterfallLowThreshold16m = (float)udDisplayWaterfallLowLevel.Value;
                    Display.WaterfallLowThreshold = console.WaterfallLowThreshold16m;
                    break;
                case Band.B14M:
                    console.WaterfallLowThreshold14m = (float)udDisplayWaterfallLowLevel.Value;
                    Display.WaterfallLowThreshold = console.WaterfallLowThreshold14m;
                    break;
                case Band.B13M:
                    console.WaterfallLowThreshold13m = (float)udDisplayWaterfallLowLevel.Value;
                    Display.WaterfallLowThreshold = console.WaterfallLowThreshold13m;
                    break;
                case Band.B11M:
                    console.WaterfallLowThreshold11m = (float)udDisplayWaterfallLowLevel.Value;
                    Display.WaterfallLowThreshold = console.WaterfallLowThreshold11m;
                    break;





                default:
                    console.WaterfallLowThresholdXVTR = (float)udDisplayWaterfallLowLevel.Value;
                    Display.WaterfallLowThreshold = console.WaterfallLowThresholdXVTR;
                    break;
            }
            // Display.WaterfallLowThreshold = (float)udDisplayWaterfallLowLevel.Value;

        } //  udDisplayWaterfallLowLevel_ValueChanged


        // ke9ns:   update the database with the new HIGH waterfall value to save at powerdown
        private void udDisplayWaterfallHighLevel_ValueChanged(object sender, System.EventArgs e)
        {
            UpdateWaterfallBandInfo();
            switch (console.RX1Band)
            {
                case Band.B160M:
                    console.WaterfallHighThreshold160m = (float)udDisplayWaterfallHighLevel.Value;
                    Display.WaterfallHighThreshold = console.WaterfallHighThreshold160m;
                    break;
                case Band.B80M:
                    console.WaterfallHighThreshold80m = (float)udDisplayWaterfallHighLevel.Value;
                    Display.WaterfallHighThreshold = console.WaterfallHighThreshold80m;
                    break;
                case Band.B60M:
                    console.WaterfallHighThreshold60m = (float)udDisplayWaterfallHighLevel.Value;
                    Display.WaterfallHighThreshold = console.WaterfallHighThreshold60m;
                    break;
                case Band.B40M:
                    console.WaterfallHighThreshold40m = (float)udDisplayWaterfallHighLevel.Value;
                    Display.WaterfallHighThreshold = console.WaterfallHighThreshold40m;
                    break;
                case Band.B30M:
                    console.WaterfallHighThreshold30m = (float)udDisplayWaterfallHighLevel.Value;
                    Display.WaterfallHighThreshold = console.WaterfallHighThreshold30m;
                    break;
                case Band.B20M:
                    console.WaterfallHighThreshold20m = (float)udDisplayWaterfallHighLevel.Value;
                    Display.WaterfallHighThreshold = console.WaterfallHighThreshold20m;
                    break;
                case Band.B17M:
                    console.WaterfallHighThreshold17m = (float)udDisplayWaterfallHighLevel.Value;
                    Display.WaterfallHighThreshold = console.WaterfallHighThreshold17m;
                    break;
                case Band.B15M:
                    console.WaterfallHighThreshold15m = (float)udDisplayWaterfallHighLevel.Value;
                    Display.WaterfallHighThreshold = console.WaterfallHighThreshold15m;
                    break;
                case Band.B12M:
                    console.WaterfallHighThreshold12m = (float)udDisplayWaterfallHighLevel.Value;
                    Display.WaterfallHighThreshold = console.WaterfallHighThreshold12m;
                    break;
                case Band.B10M:
                    console.WaterfallHighThreshold10m = (float)udDisplayWaterfallHighLevel.Value;
                    Display.WaterfallHighThreshold = console.WaterfallHighThreshold10m;
                    break;
                case Band.B6M:
                    console.WaterfallHighThreshold6m = (float)udDisplayWaterfallHighLevel.Value;
                    Display.WaterfallHighThreshold = console.WaterfallHighThreshold6m;
                    break;
                case Band.WWV:
                    console.WaterfallHighThresholdWWV = (float)udDisplayWaterfallHighLevel.Value;
                    Display.WaterfallHighThreshold = console.WaterfallHighThresholdWWV;
                    break;
                case Band.GEN:
                    console.WaterfallHighThresholdGEN = (float)udDisplayWaterfallHighLevel.Value;
                    Display.WaterfallHighThreshold = console.WaterfallHighThresholdGEN;
                    break;

                // ke9ns add: .158 SWL
                case Band.BLMF:
                    console.WaterfallHighThresholdLMF = (float)udDisplayWaterfallHighLevel.Value;
                    Display.WaterfallHighThreshold = console.WaterfallHighThresholdLMF;
                    break;
                case Band.B120M:
                    console.WaterfallHighThreshold120m = (float)udDisplayWaterfallHighLevel.Value;
                    Display.WaterfallHighThreshold = console.WaterfallHighThreshold120m;
                    break;
                case Band.B90M:
                    console.WaterfallHighThreshold90m = (float)udDisplayWaterfallHighLevel.Value;
                    Display.WaterfallHighThreshold = console.WaterfallHighThreshold90m;
                    break;
                case Band.B61M:
                    console.WaterfallHighThreshold61m = (float)udDisplayWaterfallHighLevel.Value;
                    Display.WaterfallHighThreshold = console.WaterfallHighThreshold61m;
                    break;
                case Band.B49M:
                    console.WaterfallHighThreshold49m = (float)udDisplayWaterfallHighLevel.Value;
                    Display.WaterfallHighThreshold = console.WaterfallHighThreshold49m;
                    break;
                case Band.B41M:
                    console.WaterfallHighThreshold41m = (float)udDisplayWaterfallHighLevel.Value;
                    Display.WaterfallHighThreshold = console.WaterfallHighThreshold41m;
                    break;
                case Band.B31M:
                    console.WaterfallHighThreshold31m = (float)udDisplayWaterfallHighLevel.Value;
                    Display.WaterfallHighThreshold = console.WaterfallHighThreshold31m;
                    break;
                case Band.B25M:
                    console.WaterfallHighThreshold25m = (float)udDisplayWaterfallHighLevel.Value;
                    Display.WaterfallHighThreshold = console.WaterfallHighThreshold25m;
                    break;
                case Band.B22M:
                    console.WaterfallHighThreshold22m = (float)udDisplayWaterfallHighLevel.Value;
                    Display.WaterfallHighThreshold = console.WaterfallHighThreshold22m;
                    break;
                case Band.B19M:
                    console.WaterfallHighThreshold19m = (float)udDisplayWaterfallHighLevel.Value;
                    Display.WaterfallHighThreshold = console.WaterfallHighThreshold19m;
                    break;
                case Band.B16M:
                    console.WaterfallHighThreshold16m = (float)udDisplayWaterfallHighLevel.Value;
                    Display.WaterfallHighThreshold = console.WaterfallHighThreshold16m;
                    break;
                case Band.B14M:
                    console.WaterfallHighThreshold14m = (float)udDisplayWaterfallHighLevel.Value;
                    Display.WaterfallHighThreshold = console.WaterfallHighThreshold14m;
                    break;
                case Band.B13M:
                    console.WaterfallHighThreshold13m = (float)udDisplayWaterfallHighLevel.Value;
                    Display.WaterfallHighThreshold = console.WaterfallHighThreshold13m;
                    break;
                case Band.B11M:
                    console.WaterfallHighThreshold11m = (float)udDisplayWaterfallHighLevel.Value;
                    Display.WaterfallHighThreshold = console.WaterfallHighThreshold11m;
                    break;



                default:
                    console.WaterfallHighThresholdXVTR = (float)udDisplayWaterfallHighLevel.Value;
                    Display.WaterfallHighThreshold = console.WaterfallHighThresholdXVTR;
                    break;
            }
            // Display.WaterfallHighThreshold = (float)udDisplayWaterfallHighLevel.Value;
        }

        private void udDisplayMultiPeakHoldTime_ValueChanged(object sender, System.EventArgs e)
        {
            console.MultimeterPeakHoldTime = (int)udDisplayMultiPeakHoldTime.Value;
        }

        private void udDisplayMultiTextHoldTime_ValueChanged(object sender, System.EventArgs e)
        {
            console.MultimeterTextPeakTime = (int)udDisplayMultiTextHoldTime.Value;
        }

        private void chkSpectrumPolyphase_CheckedChanged(object sender, System.EventArgs e)
        {
            console.dsp.GetDSPRX(0, 0).SpectrumPolyphase = chkSpectrumPolyphase.Checked;
            console.dsp.GetDSPRX(0, 1).SpectrumPolyphase = chkSpectrumPolyphase.Checked;
            console.dsp.GetDSPRX(1, 0).SpectrumPolyphase = chkSpectrumPolyphase.Checked;
            console.dsp.GetDSPRX(1, 1).SpectrumPolyphase = chkSpectrumPolyphase.Checked;
        }

        private void udDisplayScopeTime_ValueChanged(object sender, System.EventArgs e)
        {
            //console.ScopeTime = (int)udDisplayScopeTime.Value;
            int samples = (int)((double)udDisplayScopeTime.Value * console.SampleRate1 * 1e-6);
            //Debug.WriteLine("sample: "+samples);
            Audio.ScopeSamplesPerPixel = samples;
        }

        private void udDisplayMeterAvg_ValueChanged(object sender, System.EventArgs e)
        {
            double block_time = (double)udDisplayMeterDelay.Value * 0.001;
            int blocksToAvg = (int)((float)udDisplayMeterAvg.Value * 0.001 / block_time);
            blocksToAvg = Math.Max(2, blocksToAvg);
            console.MultiMeterAvgBlocks = blocksToAvg;
        }

        private void comboDisplayDriver_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            switch (comboDisplayDriver.Text)
            {
                case "GDI+":
                    console.CurrentDisplayEngine = DisplayEngine.GDI_PLUS;
                    break;
                    /*case "DirectX":
                        console.CurrentDisplayEngine = DisplayEngine.DIRECT_X;
                        break;*/
            }
        }

        #endregion

        #region DSP Tab Event Handlers
        // ======================================================
        // DSP Tab Event Handlers
        // ======================================================

        private void udDSPNB_ValueChanged(object sender, System.EventArgs e)
        {
            console.dsp.GetDSPRX(0, 0).NBThreshold = 0.165 * (double)(udDSPNB.Value);
            console.dsp.GetDSPRX(0, 1).NBThreshold = 0.165 * (double)(udDSPNB.Value);



            if (chkDSPRX2.Checked == true)  // ke9ns add RX2 console.chkRX2.Visible == true && 
            {
                console.dsp.GetDSPRX(1, 0).NBThreshold = 0.165 * (double)(udDSPNB.Value);
                console.dsp.GetDSPRX(1, 1).NBThreshold = 0.165 * (double)(udDSPNB.Value);
            }

        } //  udDSPNB_ValueChanged

        // ke9ns add:
        private void udDSPHT_SelectedIndexChanged_1(object sender, EventArgs e)
        {
           int temp = Convert.ToInt32(udDSPHT.SelectedItem);

           if (temp != 3 && temp != 7 && temp != 15 && temp != 31 && temp != 63) temp = 7;

            console.dsp.GetDSPRX(0, 0).NBHT = temp; // call dsp.cs then dttsp.cs
            console.dsp.GetDSPRX(0, 1).NBHT = temp;

            if (chkDSPRX2.Checked == true)  // ke9ns add RX2 console.chkRX2.Visible == true && 
            {
                console.dsp.GetDSPRX(1, 0).NBHT = temp;
                console.dsp.GetDSPRX(1, 1).NBHT = temp;
            }

        }

        private void udDSPHT_Click(object sender, EventArgs e)
        {
          
        }

        private void udDSPHT_KeyDown(object sender, KeyEventArgs e)
        {
          
        }


        private void udDSPHT_SelectedIndexChanged(object sender, EventArgs e)
        {
          

        } // udDSPHT_SelectedIndexChanged

        // ke9ns add:
        private void udDSPDLY_ValueChanged(object sender, EventArgs e)
        {
            console.dsp.GetDSPRX(0, 0).NBDLY = (int)(udDSPDLY.Value);
            console.dsp.GetDSPRX(0, 1).NBDLY = (int)(udDSPDLY.Value);

            if (chkDSPRX2.Checked == true)  // ke9ns add RX2 console.chkRX2.Visible == true && 
            {
                console.dsp.GetDSPRX(1, 0).NBDLY = (int)(udDSPDLY.Value);
                console.dsp.GetDSPRX(1, 1).NBDLY = (int)(udDSPDLY.Value);
            }

       
    } // udDSPDLY_ValueChanged

        private void udDSPNB2_ValueChanged(object sender, System.EventArgs e)
        {
            console.dsp.GetDSPRX(0, 0).SDROMThreshold = 0.165 * (double)(udDSPNB2.Value);
            console.dsp.GetDSPRX(0, 1).SDROMThreshold = 0.165 * (double)(udDSPNB2.Value);

            if (chkDSPRX2.Checked == true) // ke9ns add RX2  console.chkRX2.Visible == true &&
            {
                console.dsp.GetDSPRX(1, 0).SDROMThreshold = 0.165 * (double)(udDSPNB2.Value);
                console.dsp.GetDSPRX(1, 1).SDROMThreshold = 0.165 * (double)(udDSPNB2.Value);

            }
        }

        private void comboDSPWindow_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            console.dsp.GetDSPRX(0, 0).CurrentWindow = (Window)comboDSPWindow.SelectedIndex;
            console.dsp.GetDSPRX(0, 1).CurrentWindow = (Window)comboDSPWindow.SelectedIndex;

            if (chkDSPRX2.Checked == true) // ke9ns add RX2  console.chkRX2.Visible == true &&
            {
                console.dsp.GetDSPRX(1, 0).CurrentWindow = (Window)comboDSPWindow.SelectedIndex;
                console.dsp.GetDSPRX(1, 1).CurrentWindow = (Window)comboDSPWindow.SelectedIndex;


            }

        }


        //===============================================================================================
        // ke9ns changes RX buffer size for sharper receive filtering
        private void comboDSPPhoneRXBuf_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            console.DSPBufPhoneRX = int.Parse(comboDSPPhoneRXBuf.Text);
        }

        private void comboDSPPhoneTXBuf_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            console.DSPBufPhoneTX = int.Parse(comboDSPPhoneTXBuf.Text);
        }

        private void comboDSPCWRXBuf_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            console.DSPBufCWRX = int.Parse(comboDSPCWRXBuf.Text);
        }

        private void comboDSPCWTXBuf_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            console.DSPBufCWTX = int.Parse(comboDSPCWTXBuf.Text);
        }

        private void comboDSPDigRXBuf_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            console.DSPBufDigRX = int.Parse(comboDSPDigRXBuf.Text);
        }

        private void comboDSPDigTXBuf_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            console.DSPBufDigTX = int.Parse(comboDSPDigTXBuf.Text);
        }


        #region Image Reject

        private void udDSPImagePhaseTX_ValueChanged(object sender, System.EventArgs e)
        {
            double val = (double)udDSPImagePhaseTX.Value;
            try
            {
                console.dsp.GetDSPTX(0).TXCorrectIQPhase = val;
                CWSynth.ImagePhase = val;
            }
            catch (Exception)
            {
                MessageBox.Show(new Form { TopMost = true }, "Error setting TX Image Phase (" + val.ToString("f2") + ")");
                udDSPImagePhaseTX.Value = 0;
            }
            if (tbDSPImagePhaseTX.Value != (int)udDSPImagePhaseTX.Value)
                tbDSPImagePhaseTX.Value = (int)udDSPImagePhaseTX.Value;
        }

        private void tbDSPImagePhaseTX_Scroll(object sender, System.EventArgs e)
        {
            udDSPImagePhaseTX.Value = tbDSPImagePhaseTX.Value;
        }

        private void udDSPImageGainTX_ValueChanged(object sender, System.EventArgs e)
        {
            double val = (double)udDSPImageGainTX.Value;
            try
            {
                console.dsp.GetDSPTX(0).TXCorrectIQGain = val;
                CWSynth.ImageGain = val;
            }
            catch (Exception)
            {
                MessageBox.Show(new Form { TopMost = true }, "Error setting TX Image Gain (" + val.ToString("f2") + ")");
                udDSPImageGainTX.Value = 0;
            }
            if (tbDSPImageGainTX.Value != (int)udDSPImageGainTX.Value)
                tbDSPImageGainTX.Value = (int)udDSPImageGainTX.Value;
        }

        private void tbDSPImageGainTX_Scroll(object sender, System.EventArgs e)
        {
            udDSPImageGainTX.Value = tbDSPImageGainTX.Value;
        }

        private bool old_cpdr = false;
        private void chkTXImagCal_CheckedChanged(object sender, System.EventArgs e)
        {
            if (checkboxTXImagCal.Checked)
            {
                old_cpdr = console.CPDR;
                console.CPDR = false;

                Audio.SineFreq1 = console.CWPitch;
                Audio.TXInputSignal = Audio.SignalSource.SINE;
                Audio.SourceScale = 1.0;
            }
            else
            {
                Audio.TXInputSignal = Audio.SignalSource.RADIO;
                old_cpdr = console.CPDR;
            }
        }

        #endregion

        #region Keyer

        private void udDSPCWPitch_ValueChanged(object sender, System.EventArgs e)
        {
            console.CWPitch = (int)udDSPCWPitch.Value;
        }

        private void chkCWKeyerIambic_CheckedChanged(object sender, System.EventArgs e)
        {
            CWKeyer.Iambic = chkCWKeyerIambic.Checked;
            console.CWIambic = chkCWKeyerIambic.Checked;
            switch (console.CurrentModel)
            {
                case Model.FLEX3000:
                case Model.FLEX5000:
                    if (console.fwc_init)
                        FWC.SetIambic(chkCWKeyerIambic.Checked);
                    break;
            }
        }

        private void udCWKeyerWeight_ValueChanged(object sender, System.EventArgs e)
        {
            CWKeyer.Weight = (int)udCWKeyerWeight.Value;
        }

        private void udCWKeyerRamp_ValueChanged(object sender, System.EventArgs e)
        {
            CWSynth.RampTime = (int)udCWKeyerRamp.Value;
        }

        private void udCWKeyerSemiBreakInDelay_ValueChanged(object sender, System.EventArgs e)
        {
            CWKeyer.BreakInDelay = (double)udCWBreakInDelay.Value;
            console.BreakInDelay = (double)udCWBreakInDelay.Value;
        }

        private void chkDSPKeyerSemiBreakInEnabled_CheckedChanged(object sender, System.EventArgs e)
        {
            CWKeyer.BreakIn = chkCWBreakInEnabled.Checked;
            console.CWSemiBreakInEnabled = chkCWBreakInEnabled.Checked;
            console.BreakInEnabled = chkCWBreakInEnabled.Checked;
            udCWBreakInDelay.Enabled = chkCWBreakInEnabled.Checked;
        }

        private void chkDSPKeyerSidetone_CheckedChanged(object sender, System.EventArgs e)
        {
            console.CWSidetone = chkDSPKeyerSidetone.Checked;
        }

        private void chkCWKeyerRevPdl_CheckedChanged(object sender, System.EventArgs e)
        {
            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                case Model.FLEX3000:
                    FWC.ReversePaddles = chkCWKeyerRevPdl.Checked;
                    FWCMidi.ReversePaddles = chkCWKeyerRevPdl.Checked;
                    break;
                case Model.FLEX1500:
                    Flex1500.ReversePaddles = chkCWKeyerRevPdl.Checked;
                    break;
                case Model.SDR1000:
                    SDRSerialPort.ReversePaddles = chkCWKeyerRevPdl.Checked;
                    break;
            }
        }

        private void comboKeyerConnPrimary_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (!CWInput.SetPrimaryInput(comboKeyerConnPrimary.Text))
            {
                MessageBox.Show(new Form { TopMost = true }, "Error using " + comboKeyerConnPrimary.Text + " for Keyer Primary Input.\n" +
                    "The port may already be in use by another application.",
                    "Error using " + comboKeyerConnPrimary.Text,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                comboKeyerConnPrimary.Text = CWInput.PrimaryInput;
            }
        }

        private void comboKeyerConnSecondary_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (initializing) return;

            if (comboKeyerConnSecondary.Text == "CAT")
            {
                if (!chkCATEnable.Checked)
                {
                    MessageBox.Show(new Form { TopMost = true }, "CAT is not Enabled.  Please enable the CAT interface before selecting this option.",
                        "CAT not enabled",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Hand);
                    comboKeyerConnSecondary.Text = CWInput.SecondaryInput;
                    return;
                }
                else
                {
                    CWInput.SetSecondaryInput("None");
                    console.Siolisten.UseForKeyPTT = true;

                  //  console.Siolisten2.UseForKeyPTT = true; // ke9ns add .180 port2
                  //  console.Siolisten3.UseForKeyPTT = true; // ke9ns add .180 port3
                  //  console.Siolisten4.UseForKeyPTT = true; // ke9ns add .180 port4
                  //  console.Siolisten5.UseForKeyPTT = true; // ke9ns add .180 port5
                }
            }
            else
            {
                if (console.Siolisten != null)
                {
                    console.Siolisten.UseForKeyPTT = false;

                }

                if (console.Siolisten2 != null) // ke9ns add .180 port2
                {
                    console.Siolisten2.UseForKeyPTT = false;

                }
                if (console.Siolisten3 != null) // ke9ns add .180 port3
                {
                    console.Siolisten3.UseForKeyPTT = false;

                }
                if (console.Siolisten4 != null) // ke9ns add .180 port4
                {
                    console.Siolisten4.UseForKeyPTT = false;

                }
                if (console.Siolisten5 != null) // ke9ns add .180 port5
                {
                    console.Siolisten5.UseForKeyPTT = false;

                }
            }

            if (!CWInput.SetSecondaryInput(comboKeyerConnSecondary.Text))
            {
                MessageBox.Show(new Form { TopMost = true }, "Error using " + comboKeyerConnSecondary.Text + " for Keyer Secondary Input.\n" +
                    "The port may already be in use by another application.",
                    "Error using " + comboKeyerConnSecondary.Text,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                comboKeyerConnSecondary.Text = CWInput.SecondaryInput;
                return;
            }

            switch (comboKeyerConnSecondary.Text)
            {
                case "None":
                    lblKeyerConnPTTLine.Visible = false;
                    comboKeyerConnPTTLine.Visible = false;
                    lblKeyerConnKeyLine.Visible = false;
                    comboKeyerConnKeyLine.Visible = false;
                    break;
                case "CAT":
                    lblKeyerConnPTTLine.Visible = true;
                    comboKeyerConnPTTLine.Visible = true;
                    lblKeyerConnKeyLine.Visible = true;
                    comboKeyerConnKeyLine.Visible = true;

                    comboKeyerConnPTTLine_SelectedIndexChanged(this, EventArgs.Empty);
                    comboKeyerConnKeyLine_SelectedIndexChanged(this, EventArgs.Empty);
                    break;
                default: // COMx
                    lblKeyerConnPTTLine.Visible = true;
                    comboKeyerConnPTTLine.Visible = true;
                    lblKeyerConnKeyLine.Visible = true;
                    comboKeyerConnKeyLine.Visible = true;

                    comboKeyerConnPTTLine_SelectedIndexChanged(this, EventArgs.Empty);
                    comboKeyerConnKeyLine_SelectedIndexChanged(this, EventArgs.Empty);
                    break;
            }
        }

        private void comboKeyerConnKeyLine_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (comboKeyerConnKeyLine.SelectedIndex < 0) return;

            if (comboKeyerConnSecondary.Text == "CAT")
            {
                switch ((KeyerLine)comboKeyerConnKeyLine.SelectedIndex)
                {
                    case KeyerLine.None:
                        console.Siolisten.KeyOnDTR = false;
                        console.Siolisten.KeyOnRTS = false;
                        break;
                    case KeyerLine.DTR:
                        console.Siolisten.KeyOnDTR = true;
                        console.Siolisten.KeyOnRTS = false;
                        break;
                    case KeyerLine.RTS:
                        console.Siolisten.KeyOnDTR = false;
                        console.Siolisten.KeyOnRTS = true;
                        break;
                }
            }
            else CWInput.SecondaryKeyLine = (KeyerLine)comboKeyerConnKeyLine.SelectedIndex;
        }

        private void comboKeyerConnPTTLine_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (comboKeyerConnPTTLine.SelectedIndex < 0) return;

            if (comboKeyerConnSecondary.Text == "CAT")
            {
                switch ((KeyerLine)comboKeyerConnPTTLine.SelectedIndex)
                {
                    case KeyerLine.None:
                        console.Siolisten.PTTOnDTR = false;
                        console.Siolisten.PTTOnRTS = false;
                        break;
                    case KeyerLine.DTR:
                        console.Siolisten.PTTOnDTR = true;
                        console.Siolisten.PTTOnRTS = false;
                        break;
                    case KeyerLine.RTS:
                        console.Siolisten.PTTOnDTR = false;
                        console.Siolisten.PTTOnRTS = true;
                        break;
                }
            }
            else CWInput.SecondaryPTTLine = (KeyerLine)comboKeyerConnPTTLine.SelectedIndex;
        }

        #endregion

        #region AGC

        private void udDSPAGCFixedGaindB_ValueChanged(object sender, System.EventArgs e)
        {
            console.dsp.GetDSPRX(0, 0).RXFixedAGC = (double)udDSPAGCFixedGaindB.Value;
            console.dsp.GetDSPRX(0, 1).RXFixedAGC = (double)udDSPAGCFixedGaindB.Value;

            if (console.RX1AGCMode == AGCMode.FIXD)
                console.RF = (int)udDSPAGCFixedGaindB.Value;
        }

        private void udDSPAGCMaxGaindB_ValueChanged(object sender, System.EventArgs e)
        {
            console.dsp.GetDSPRX(0, 0).RXAGCMaxGain = (double)udDSPAGCMaxGaindB.Value;
            console.dsp.GetDSPRX(0, 1).RXAGCMaxGain = (double)udDSPAGCMaxGaindB.Value;

            if (console.RX1AGCMode != AGCMode.FIXD)
                console.RF = (int)udDSPAGCMaxGaindB.Value;
        }

        private void udDSPAGCAttack_ValueChanged(object sender, System.EventArgs e)
        {
            if (udDSPAGCAttack.Enabled)
            {
                console.dsp.GetDSPRX(0, 0).RXAGCAttack = (int)udDSPAGCAttack.Value;
                console.dsp.GetDSPRX(0, 1).RXAGCAttack = (int)udDSPAGCAttack.Value;



            }
        }

        private void udDSPAGCDecay_ValueChanged(object sender, System.EventArgs e)
        {
            if (udDSPAGCDecay.Enabled)
            {
                console.dsp.GetDSPRX(0, 0).RXAGCDecay = (int)udDSPAGCDecay.Value;
                console.dsp.GetDSPRX(0, 1).RXAGCDecay = (int)udDSPAGCDecay.Value;
            }
        }

        private void udDSPAGCSlope_ValueChanged(object sender, System.EventArgs e)
        {
            console.dsp.GetDSPRX(0, 0).RXAGCSlope = 10 * (int)(udDSPAGCSlope.Value);
            console.dsp.GetDSPRX(0, 1).RXAGCSlope = 10 * (int)(udDSPAGCSlope.Value);
        }

        private void udDSPAGCHangTime_ValueChanged(object sender, System.EventArgs e)
        {
            if (udDSPAGCHangTime.Enabled)
            {
                console.dsp.GetDSPRX(0, 0).RXAGCHang = (int)udDSPAGCHangTime.Value;
                console.dsp.GetDSPRX(0, 1).RXAGCHang = (int)udDSPAGCHangTime.Value;
            }
        }

        private void tbDSPAGCHangThreshold_Scroll(object sender, System.EventArgs e)
        {
            console.dsp.GetDSPRX(0, 0).RXAGCHangThreshold = (int)tbDSPAGCHangThreshold.Value;
            console.dsp.GetDSPRX(0, 1).RXAGCHangThreshold = (int)tbDSPAGCHangThreshold.Value;
        }

        #endregion

        #region Leveler

        private void udDSPLevelerHangTime_ValueChanged(object sender, System.EventArgs e)
        {
            console.dsp.GetDSPTX(0).TXLevelerHang = (int)udDSPLevelerHangTime.Value;
        }

        private void udDSPLevelerThreshold_ValueChanged(object sender, System.EventArgs e)
        {
            console.dsp.GetDSPTX(0).TXLevelerMaxGain = (double)udDSPLevelerThreshold.Value;
        }

        private void udDSPLevelerAttack_ValueChanged(object sender, System.EventArgs e)
        {
            console.dsp.GetDSPTX(0).TXLevelerAttack = (int)udDSPLevelerAttack.Value;
        }

        private void udDSPLevelerDecay_ValueChanged(object sender, System.EventArgs e)
        {
            console.dsp.GetDSPTX(0).TXLevelerDecay = (int)udDSPLevelerDecay.Value;
        }

        private void chkDSPLevelerEnabled_CheckedChanged(object sender, System.EventArgs e)
        {
            console.dsp.GetDSPTX(0).TXLevelerOn = chkDSPLevelerEnabled.Checked;
        }

        #endregion

        #region ALC

        private void udDSPALCHangTime_ValueChanged(object sender, System.EventArgs e)
        {
            console.dsp.GetDSPTX(0).TXALCHang = (int)udDSPALCHangTime.Value;
        }

        private void udDSPALCThreshold_ValueChanged(object sender, System.EventArgs e)
        {
            //DttSP.SetTXALCBot((double)udDSPALCThreshold.Value);
        }

        private void udDSPALCAttack_ValueChanged(object sender, System.EventArgs e)
        {
            console.dsp.GetDSPTX(0).TXALCAttack = (int)udDSPALCAttack.Value;
        }

        private void udDSPALCDecay_ValueChanged(object sender, System.EventArgs e)
        {
            console.dsp.GetDSPTX(0).TXALCDecay = (int)udDSPALCDecay.Value;
        }

        #endregion

        #endregion

        #region Transmit Tab Event Handlers


        // ke9ns add for 2nd TX meter
        private void chkTXMeter2_CheckedChanged(object sender, EventArgs e)
        {

            console.TXMeter2 = chkTXMeter2.Checked;
            console.Console_Resize(this, e);
        }

        private void udTXFilterHigh_ValueChanged(object sender, System.EventArgs e)
        {
            if (udTXFilterHigh.Value < udTXFilterLow.Value + 100)
            {
                udTXFilterHigh.Value = udTXFilterLow.Value + 100;
                return;
            }

            /*
			if(udTXFilterHigh.Focused && (udTXFilterHigh.Value - udTXFilterLow.Value) > 3000 &&
				(console.TXFilterHigh - console.TXFilterLow) <= 3000)
			{
				(new Thread(new ThreadStart(TXBW))).Start();
			}
            */
            console.TXFilterHigh = (int)udTXFilterHigh.Value;
            console.udTXFilterHigh.Value = udTXFilterHigh.Value;

        }

        private void TXBW()
        {
            MessageBox.Show(new Form { TopMost = true }, "The transmit bandwidth is being increased beyond 3kHz.\n\n" +
                "As the control operator, you are responsible for compliance with current " +
                "rules and good operating practice.",
                "Warning: Transmit Bandwidth",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }

        private void udTXFilterLow_ValueChanged(object sender, System.EventArgs e)
        {
            if (udTXFilterLow.Value > udTXFilterHigh.Value - 100)
            {
                udTXFilterLow.Value = udTXFilterHigh.Value - 100;
                return;
            }

            if (udTXFilterLow.Focused && (udTXFilterHigh.Value - udTXFilterLow.Value) > 3000 &&
                (console.TXFilterHigh - console.TXFilterLow) <= 3000)
            {
                (new Thread(new ThreadStart(TXBW))).Start(); // just a warning message
            }

            console.TXFilterLow = (int)udTXFilterLow.Value;
            console.udTXFilterLow.Value = udTXFilterLow.Value;
        }

        private void udTransmitTunePower_ValueChanged(object sender, System.EventArgs e)
        {
            if (console.chkBoxDrive.Checked == false)
            {
                if (udTXDriveMax.Value < udTXTunePower.Value) udTXTunePower.Value = udTXDriveMax.Value;

                console.TunePower = (int)udTXTunePower.Value;
            }
        }


        //==================================================================================================
        private string current_profile = "";
        private void comboTXProfileName_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            //  Debug.WriteLine("EQ combotxprofilename select==============");

            if (comboTXProfileName.SelectedIndex < 0 || initializing) return;

            if (chkAutoSaveTXProfile.Checked == true)
            {
                SaveTXProfileData();
            }
            else
            {
                Debug.WriteLine("0TXPROFILE");

                if (CheckTXProfileChanged() && comboTXProfileName.Focused)
                {
                    DialogResult result = MessageBox.Show(new Form { TopMost = true }, "The current profile has changed.  " +
                        "Would you like to save the current profile?",
                        "Save Current Profile?",
                        MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        btnTXProfileSave_Click(this, EventArgs.Empty);
                        //return;
                    }
                    else if (result == DialogResult.Cancel) return;
                }

                Debug.WriteLine("1TXPROFILE");
            }

            console.TXProfile = comboTXProfileName.Text;



            DB.ModTXProfileTable(); // ke9ns add: to modify old databases
            DB.ModTXProfileDefTable();

            DataRow[] rows = DB.ds.Tables["TxProfile"].Select("'" + comboTXProfileName.Text + "' = Name");

            if (rows.Length != 1)
            {
                MessageBox.Show(new Form { TopMost = true }, "Database error reading TxProfile Table.",
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            DataRow dr = rows[0];

            //int[] eq = null;

            Debug.WriteLine("LOAD EQ==============");

            try
            {
                console.eqForm.TXEQEnabled = (bool)dr["TXEQEnabled"];
            }
            catch (Exception)
            {
                console.eqForm.TXEQEnabled = false;
            }

            try
            {
                console.eqForm.NumBands = (int)dr["TXEQNumBands"];
            }
            catch (Exception)
            {
                console.eqForm.NumBands = 10;
            }

            int[] eq;

            try
            {
                console.eqForm.rad3Band.Checked = (bool)dr["TXEQBand"];
                // Debug.WriteLine("TXEQBAND " + console.eqForm.rad3Band.Checked);
                if (console.eqForm.rad3Band.Checked == true) eq = new int[4]; // ke9ns: restore 10band eq data
                else eq = new int[11]; // ke9ns: restore 10band eq data
            }
            catch (Exception)
            {
                eq = new int[11]; // ke9ns: restore 10band eq data
                console.eqForm.rad3Band.Checked = false;
            }

            //--------------------------------------------
            //  eq = new int[11]; // ke9ns: restore 10band eq data

            eq[0] = (int)dr["TXEQPreamp"];

            for (int i = 1; i < eq.Length; i++)
            {
                //  Debug.WriteLine("TXEQ " + i);

                try
                {
                    eq[i] = (int)dr["TXEQ" + i.ToString()];
                }
                catch (Exception)
                {
                    eq[i] = 0;
                }
            }

            console.eqForm.TXEQ = eq; // ke9ns: give EQform updated data

            //-------------------------------------

            int[] eq1 = new int[29]; // ke9ns: restore 28band eq data

            try
            {
                eq1[0] = (int)dr["TXEQ28Preamp"];
                Debug.WriteLine("TXEQ28Preamp " + eq1[0]);
            }
            catch (Exception)
            {
                eq1[0] = 0;
            }

            for (int i = 1; i < eq1.Length; i++)
            {
                Debug.WriteLine("TX28EQ " + i);

                try
                {
                    eq1[i] = (int)dr["TX28EQ" + i.ToString()];
                }
                catch (Exception)
                {
                    eq1[i] = 0;
                }
            }
          
            console.eqForm.TXEQ28 = eq1; // ke9ns: give EQform updated data
          

            //-------------------------------------

            int[] eq2 = new int[28];// ke9ns: restore 9band peq data

            try
            {
                eq2[0] = (int)dr["TXEQ9Preamp"];
            }
            catch (Exception)
            {
                eq2[0] = 0;
            }

            for (int i = 1; i < 10; i++)
            {
                Debug.WriteLine("PEQ " + i);

                try
                {
                    eq2[i] = (int)dr["PEQ" + i.ToString()];
                }
                catch (Exception)
                {
                    eq2[i] = 0;
                }
            }

            for (int i = 10; i < 19; i++)
            {
                Debug.WriteLine("PEQoctave" + i);

                try
                {
                    eq2[i] = (int)dr["PEQoctave" + (i - 9).ToString()];
                }
                catch (Exception)
                {
                    eq2[i] = 10;
                }
            }

            for (int i = 19; i < eq2.Length; i++)
            {
                Debug.WriteLine("PEQfreq" + i);

                try
                {
                    eq2[i] = (int)dr["PEQfreq" + (i - 18).ToString()];
                }
                catch (Exception)
                {
                    eq2[i] = 0;
                }
            }

            console.eqForm.PEQ = eq2;  // ke9ns: give EQform updated data   (peq, peqoctave, peqfreq) in that order

            //-------------------------------------------------------------
            // ke9ns: get checkbox EQform data restore
            try
            {
                console.eqForm.rad28Band.Checked = (bool)dr["TXEQ28Band"];

            }
            catch (Exception)
            {
                console.eqForm.rad28Band.Checked = false;
            }

            try
            {
                console.eqForm.rad10Band.Checked = (bool)dr["TXEQ10Band"];
            }
            catch (Exception)
            {
                console.eqForm.rad10Band.Checked = false;
            }

            try
            {
                console.eqForm.rad3Band.Checked = (bool)dr["TXEQBand"];
            }
            catch (Exception)
            {

                console.eqForm.rad3Band.Checked = false;
            }

            try
            {
                console.eqForm.radPEQ.Checked = (bool)dr["TXEQ9Band"];
            }
            catch (Exception)
            {
                console.eqForm.radPEQ.Checked = false;
            }
            try
            {

                console.eqForm.chkBothEQ.Checked = (bool)dr["TXEQ37Band"];
            }
            catch (Exception)
            {
                console.eqForm.chkBothEQ.Checked = false;
            }

            //----------------------------------
            // ke9ns: done with retrieving database data and puting into EQ registers, not update buffers

            console.eqForm.EQLoad(); // ke9ns: update the EQ now (by taking newly restored data and updating DSP.cs buffers immediatly

          

            //--------------------------------------------------------------------------------------------------

            udTXFilterLow.Value = Math.Min(Math.Max((int)dr["FilterLow"], udTXFilterLow.Minimum), udTXFilterLow.Maximum);
            udTXFilterHigh.Value = Math.Min(Math.Max((int)dr["FilterHigh"], udTXFilterHigh.Minimum), udTXFilterHigh.Maximum);

            console.DX = (bool)dr["DXOn"];
            console.DXLevel = (int)dr["DXLevel"];

            console.CPDR = (bool)dr["CompanderOn"];
            console.CPDRLevel = (int)dr["CompanderLevel"];

            console.Mic = (int)dr["MicGain"];
            console.FMMic = (int)dr["FMMicGain"];

            chkDSPLevelerEnabled.Checked = (bool)dr["Lev_On"];
            udDSPLevelerSlope.Value = (int)dr["Lev_Slope"];
            udDSPLevelerThreshold.Value = (int)dr["Lev_MaxGain"];
            udDSPLevelerAttack.Value = (int)dr["Lev_Attack"];
            udDSPLevelerDecay.Value = (int)dr["Lev_Decay"];
            udDSPLevelerHangTime.Value = (int)dr["Lev_Hang"];
            tbDSPLevelerHangThreshold.Value = (int)dr["Lev_HangThreshold"];

            udDSPALCSlope.Value = (int)dr["ALC_Slope"];
            udDSPALCThreshold.Value = (int)dr["ALC_MaxGain"];
            udDSPALCAttack.Value = (int)dr["ALC_Attack"];
            udDSPALCDecay.Value = (int)dr["ALC_Decay"];
            udDSPALCHangTime.Value = (int)dr["ALC_Hang"];
            tbDSPALCHangThreshold.Value = (int)dr["ALC_HangThreshold"];

            chkTXNoiseGateEnabled.Checked = (bool)dr["Dexp_On"];
            udTXNoiseGate.Value = (int)dr["Dexp_Threshold"];
            udTXNoiseGateAttenuate.Value = (int)dr["Dexp_Attenuate"];

            chkTXVOXEnabled.Checked = (bool)dr["VOX_On"];
            udTXVOXThreshold.Value = (int)dr["VOX_Threshold"];
            udTXVOXHangTime.Value = (int)dr["VOX_HangTime"];

            udTXTunePower.Value = (int)dr["Tune_Power"];
            comboTXTUNMeter.Text = (string)dr["Tune_Meter_Type"];

            chkTXLimitSlew.Checked = (bool)dr["TX_Limit_Slew"];
            udTX1500PhoneBlanking.Value = (int)dr["TXBlankingTime"];
            chkAudioMicBoost.Checked = (bool)dr["MicBoost"];

            console.TXAF = (int)dr["TX_AF_Level"];

            udTXAMCarrierLevel.Value = (int)dr["AM_Carrier_Level"];

            console.ShowTXFilter = (bool)dr["Show_TX_Filter"];

            chkAudioVACAutoEnable.Checked = (bool)dr["VAC1_Auto_On"];
            udAudioVACGainRX.Value = (int)dr["VAC1_RX_GAIN"];
            udAudioVACGainTX.Value = (int)dr["VAC1_TX_GAIN"];
            chkAudio2Stereo.Checked = (bool)dr["VAC1_Stereo_On"];
            comboAudioSampleRate2.Text = (string)dr["VAC1_Sample_Rate"];
            comboAudioBuffer2.Text = (string)dr["VAC1_Buffer_Size"];
            chkAudioIQtoVAC.Checked = (bool)dr["VAC1_IQ_Output"];
            chkAudioCorrectIQ.Checked = (bool)dr["VAC1_IQ_Correct"];
            chkVACAllowBypass.Checked = (bool)dr["VAC1_PTT_OverRide"];
            chkVACCombine.Checked = (bool)dr["VAC1_Combine_Input_Channels"];
            chkAudioLatencyManual2.Checked = true;
            udAudioLatency2.Value = (int)dr["VAC1_Latency_Duration"];
            chkAudioEnableVAC.Checked = (bool)dr["VAC1_On"];

            chkVAC2AutoEnable.Checked = (bool)dr["VAC2_Auto_On"];
            udVAC2GainRX.Value = (int)dr["VAC2_RX_GAIN"];
            udVAC2GainTX.Value = (int)dr["VAC2_TX_GAIN"];
            chkAudioStereo3.Checked = (bool)dr["VAC2_Stereo_On"];
            comboAudioSampleRate3.Text = (string)dr["VAC2_Sample_Rate"];
            comboAudioBuffer3.Text = (string)dr["VAC2_Buffer_Size"];
            chkVAC2DirectIQ.Checked = (bool)dr["VAC2_IQ_Output"];
            chkVAC2DirectIQCal.Checked = (bool)dr["VAC2_IQ_Correct"];
            chkVAC2Combine.Checked = (bool)dr["VAC2_Combine_Input_Channels"];
            chkVAC2LatencyManual.Checked = true;
            udVAC2Latency.Value = (int)dr["VAC2_Latency_Duration"];
            chkVAC2Enable.Checked = (bool)dr["VAC2_On"];

            comboDSPPhoneRXBuf.Text = (string)dr["Phone_RX_DSP_Buffer"];
            comboDSPPhoneTXBuf.Text = (string)dr["Phone_TX_DSP_Buffer"];
            comboDSPDigRXBuf.Text = (string)dr["Digi_RX_DSP_Buffer"];
            comboDSPDigTXBuf.Text = (string)dr["Digi_TX_DSP_Buffer"];
            comboDSPCWRXBuf.Text = (string)dr["CW_RX_DSP_Buffer"];

            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                    if (console.fwcMixForm != null)
                    {
                        console.fwcMixForm.MicInputSelected = (string)dr["Mic_Input_On"];
                        console.fwcMixForm.MicInput = (int)dr["Mic_Input_Level"];
                        console.fwcMixForm.LineInRCASelected = (string)dr["Line_Input_On"];
                        console.fwcMixForm.LineInRCA = (int)dr["Line_Input_Level"];
                        console.fwcMixForm.LineInPhonoSelected = (string)dr["Balanced_Line_Input_On"];
                        console.fwcMixForm.LineInPhono = (int)dr["Balanced_Line_Input_Level"];
                        console.fwcMixForm.LineInDB9Selected = (string)dr["FlexWire_Input_On"];
                        console.fwcMixForm.LineInDB9 = (int)dr["FlexWire_Input_Level"];
                    }
                    break;

                case Model.FLEX3000:
                    if (console.flex3000MixerForm != null)
                    {
                        console.flex3000MixerForm.MicInputSelected = (string)dr["Mic_Input_On"];
                        console.flex3000MixerForm.MicInput = (int)dr["Mic_Input_Level"];
                        console.flex3000MixerForm.LineInDB9Selected = (string)dr["FlexWire_Input_On"];
                        console.flex3000MixerForm.LineInDB9 = (int)dr["FlexWire_Input_Level"];
                    }
                    break;

                case Model.FLEX1500:
                    if (console.flex1500MixerForm != null)
                    {
                        console.flex1500MixerForm.MicInputSelectedStr = (string)dr["Mic_Input_On"];
                        console.flex1500MixerForm.MicInput = (int)dr["Mic_Input_Level"];
                        console.flex1500MixerForm.LineInDB9Selected = (string)dr["FlexWire_Input_On"];
                        console.flex1500MixerForm.FlexWireIn = (int)dr["FlexWire_Input_Level"];
                    }
                    break;

                default:
                    // do nothing for other radios models
                    break;
            }
            console.PWR = (int)dr["Power"];

            current_profile = comboTXProfileName.Text;

            // ke9ns .223 below to force TXEQ to activate upon a profile change
            console.eqForm.tbTXEQ28Preamp.Value = Math.Max(console.eqForm.tbTXEQ28Preamp.Minimum, Math.Min(console.eqForm.tbTXEQ28Preamp.Maximum, console.eqForm.tbTXEQ28Preamp.Value - 1)); //.223 add
            console.eqForm.tbTXEQPreamp.Value = Math.Max(console.eqForm.tbTXEQPreamp.Minimum, Math.Min(console.eqForm.tbTXEQPreamp.Maximum, console.eqForm.tbTXEQPreamp.Value - 1)); //.223 add
            console.eqForm.tbTXEQ9Preamp.Value = Math.Max(console.eqForm.tbTXEQ9Preamp.Minimum, Math.Min(console.eqForm.tbTXEQ9Preamp.Maximum, console.eqForm.tbTXEQ9Preamp.Value - 1)); //.223 add
       
          //  console.eqForm.tbPEQ1_Scroll(this, EventArgs.Empty); // ke9ns add .171 to properly load up the eq on profile change
          //  console.eqForm.tbTX28EQ15_Scroll(this, EventArgs.Empty); // ke9ns add .171
           // console.eqForm.tbTXEQ_Scroll(this, EventArgs.Empty); // ke9ns add .171

            console.eqForm.tbTXEQ28Preamp.Value = Math.Max(console.eqForm.tbTXEQ28Preamp.Minimum, Math.Min(console.eqForm.tbTXEQ28Preamp.Maximum, console.eqForm.tbTXEQ28Preamp.Value + 1));
            console.eqForm.tbTXEQPreamp.Value = Math.Max(console.eqForm.tbTXEQPreamp.Minimum, Math.Min(console.eqForm.tbTXEQPreamp.Maximum, console.eqForm.tbTXEQPreamp.Value + 1));
            console.eqForm.tbTXEQ9Preamp.Value = Math.Max(console.eqForm.tbTXEQ9Preamp.Minimum, Math.Min(console.eqForm.tbTXEQ9Preamp.Maximum, console.eqForm.tbTXEQ9Preamp.Value + 1));

            //--------------------------------------------------------------------------------------------------
            // ke9ns: .196 save DSP MODE for both RX1 and RX2


            if (chkRememberTXProfileOnModeChange.Checked == false) // ke9ns: dont change modes if mode is supposed to change TXProfile
            {
                try
                {

                    console.RX1DSPMODE = (DSPMode)dr["RX1DSPMODE"];


                    if (console.RX1DSPMODE == DSPMode.USB)
                    {
                        if (console.RX1Band == Band.B160M || console.RX1Band == Band.B80M || console.RX1Band == Band.B40M)
                        {
                            console.RX1DSPMODE = DSPMode.LSB;
                        }

                    }
                    else if (console.RX1DSPMODE == DSPMode.LSB)
                    {
                        if (console.RX1Band != Band.B160M && console.RX1Band != Band.B80M && console.RX1Band != Band.B40M)
                        {
                            console.RX1DSPMODE = DSPMode.USB;
                        }

                    }

                    console.RX1DSPMode = console.RX1DSPMODE; // ke9ns: set mode from TXprofile .196
                                                             //  console.TXMode = console.RX1DSPMODE;
                                                             //   console.SetRX1Mode(console.RX1DSPMODE);


                }
                catch (Exception)
                {
                    // console.RX1DSPMODE = DSPMode.FIRST;

                }


                try
                {
                    console.RX2DSPMODE = (DSPMode)dr["RX2DSPMODE"];

                    if (console.RX2DSPMODE == DSPMode.USB)
                    {
                        if (console.RX2Band == Band.B160M || console.RX2Band == Band.B80M || console.RX2Band == Band.B40M)
                        {
                            console.RX2DSPMODE = DSPMode.LSB;
                        }

                    }
                    else if (console.RX2DSPMODE == DSPMode.LSB)
                    {
                        if (console.RX2Band != Band.B160M && console.RX2Band != Band.B80M && console.RX2Band != Band.B40M)
                        {
                            console.RX2DSPMODE = DSPMode.USB;
                        }

                    }

                    console.RX2DSPMode = console.RX2DSPMODE; // ke9ns: set mode from TXprofile .196

                }
                catch (Exception)
                {
                    //  console.RX2DSPMODE = DSPMode.FIRST;
                }

            } // chkRememberTXProfileOnModeChange

           
            



        } // comboTXProfileName_SelectedIndexChanged


        //==============================================================
        // TXPROFILE save button pushed
        private void btnTXProfileSave_Click(object sender, System.EventArgs e)
        {

            if (chkAlwaysOnTop1.Checked == true) this.TopMost = false; // ke9ns add

            string name = InputBox.Show("Save Profile", "Please enter a profile name:", current_profile);

            if (name == "" || name == null)
            {
                MessageBox.Show(new Form { TopMost = true }, "TX Profile Save cancelled",
                    "TX Profile",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            DataRow dr = null; // ke9ns: refers to database.cs for actual tables that make up dr[]


            if (comboTXProfileName.Items.Contains(name))
            {
                DialogResult result = MessageBox.Show(
                    "Are you sure you want to overwrite the " + name + " TX Profile?",
                    "Overwrite Profile?",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.No) return;

                //  Debug.WriteLine("BTN....SAVETXPROFILE");

                foreach (DataRow d in DB.ds.Tables["TxProfile"].Rows) // ke9ns: go through EVERY ROW of the DataSet in the ds.Tables until you find the matching NAME,
                {
                    if ((string)d["Name"] == name)
                    {
                        // ke9ns mod back
                        try
                        {

                            if ((int)d["TX28EQ1"] == 0) // ke9ns add: if this key is not found a exception will occur here
                            {

                            }
                            if ((int)d["PEQ1"] == 0)
                            {


                            }
                            if ((bool)d["TXEQ10Band"] == false)
                            {

                            }
                            if ((DSPMode)d["RX1DSPMODE"] == 0) // .196
                            {


                            }

                            //   Debug.WriteLine("YES TXEQ12");
                            dr = d; // ke9ns when you match the name, copy over all the rows into the DataRow dr
                        }
                        catch (Exception) // do this to remove an old TXPROFILE with only TXEQ10 and then add back the same TXPROFILE with TXEQ28
                        {
                            Debug.WriteLine("8Remove old TXprofile and create new version: " + comboTXProfileName.Text);

                            DB.ModTXProfileTable(); // ke9ns add: to modify old databases
                            DB.ModTXProfileDefTable();

                            DataRow[] rows1 = DB.ds.Tables["TxProfile"].Select("'" + comboTXProfileName.Text + "' = Name");

                            Debug.WriteLine("9Remove old TXprofile and create new version");

                            DataRow dd = null;

                            dd = rows1[0];

                            rows1[0].Delete();


                            dr = DB.ds.Tables["TxProfile"].NewRow(); // ke9ns create a new TXPROFILE with all the new Rows found in ds.Tables

                            dr = dd; // copy original data back into 
                            dr["Name"] = name;

                            DB.ds.Tables["TxProfile"].Rows.Add(dr); // add a new TX profile

                            Debug.WriteLine("8new version created");


                        } // catch


                        break;
                    }
                } // for


            }
            else // ke9ns OR create a new name if your saving to a completely new file name
            {
                DB.ModTXProfileTable(); // ke9ns add: to modify old databases
                DB.ModTXProfileDefTable();


                dr = DB.ds.Tables["TxProfile"].NewRow(); // ke9ns create a new TXPROFILE with all the new Rows found in ds.Tables
                dr["Name"] = name;
            }


            dr["FilterLow"] = (int)udTXFilterLow.Value;
            dr["FilterHigh"] = (int)udTXFilterHigh.Value;

            dr["TXEQEnabled"] = console.eqForm.TXEQEnabled;
            dr["TXEQNumBands"] = console.eqForm.NumBands;

            //-------------------------------------------------------------
            // ke9ns add:
            dr["TXEQ28Band"] = console.eqForm.rad28Band.Checked;
            dr["TXEQ10Band"] = console.eqForm.rad10Band.Checked;
            dr["TXEQBand"] = console.eqForm.rad3Band.Checked;
            dr["TXEQ9Band"] = console.eqForm.radPEQ.Checked;
            dr["TXEQ37Band"] = console.eqForm.chkBothEQ.Checked;

            //-------------------------------------------------------------------
            int[] eq = console.eqForm.TXEQ; // ke9ns   could be 3, 10 band

            dr["TXEQPreamp"] = eq[0]; // always 0

            for (int i = 1; i < eq.Length; i++) // ke9ns save currnent TXEQ values
            {
                //  Debug.WriteLine("1EQ " + i);

                try
                {
                    dr["TXEQ" + i.ToString()] = eq[i];
                }
                catch (Exception)
                {
                    Debug.WriteLine("TXEQ bad at " + i);
                }

            }


            //------------------------------------------------------------------
            // ke9ns add: 28 band SAVE
            int[] eq1 = console.eqForm.TXEQ28;

            dr["TXEQ28Preamp"] = eq1[0];

            for (int i = 1; i < eq1.Length; i++)
            {

                try
                {
                    dr["TX28EQ" + i.ToString()] = eq1[i];
                }
                catch (Exception)
                {
                    Debug.WriteLine("TX28EQ bad at " + i);
                }
            }


            //------------------------------------------------------------------
            // ke9ns add: 9 band peq save

            int[] eq2 = console.eqForm.PEQ;

            dr["TXEQ9Preamp"] = eq2[0];

            for (int i = 1; i < 10; i++)
            {

                try
                {
                    dr["PEQ" + i.ToString()] = eq2[i];
                }
                catch (Exception)
                {
                    Debug.WriteLine("PEQ bad at " + i);
                }

            }

            for (int i = 10; i < 19; i++)
            {

                try
                {
                    dr["PEQoctave" + (i - 9).ToString()] = eq2[i];
                }
                catch (Exception E)
                {
                    Debug.WriteLine("PEQoctave bad at " + i + " , " + E);
                }

            }

            for (int i = 19; i < eq2.Length; i++)
            {

                try
                {
                    dr["PEQfreq" + (i - 18).ToString()] = eq2[i];
                }
                catch (Exception E)
                {
                    Debug.WriteLine("PEQfreq bad at " + i + " , " + E);
                }

            }

            //===============================================================

            dr["RX1DSPMODE"] = console.RX1DSPMODE; // .196
            dr["RX2DSPMODE"] = console.RX2DSPMODE; // .196


            //===============================================================
            dr["DXOn"] = console.DX;
            dr["DXLevel"] = console.DXLevel;
            dr["CompanderOn"] = console.CPDR;
            dr["CompanderLevel"] = console.CPDRLevel;

            dr["MicGain"] = console.Mic;
            dr["FMMicGain"] = console.FMMic;

            dr["Lev_On"] = chkDSPLevelerEnabled.Checked;
            dr["Lev_Slope"] = (int)udDSPLevelerSlope.Value;
            dr["Lev_MaxGain"] = (int)udDSPLevelerThreshold.Value;
            dr["Lev_Attack"] = (int)udDSPLevelerAttack.Value;
            dr["Lev_Decay"] = (int)udDSPLevelerDecay.Value;
            dr["Lev_Hang"] = (int)udDSPLevelerHangTime.Value;
            dr["Lev_HangThreshold"] = tbDSPLevelerHangThreshold.Value;

            dr["ALC_Slope"] = (int)udDSPALCSlope.Value;
            dr["ALC_MaxGain"] = (int)udDSPALCThreshold.Value;
            dr["ALC_Attack"] = (int)udDSPALCAttack.Value;
            dr["ALC_Decay"] = (int)udDSPALCDecay.Value;
            dr["ALC_Hang"] = (int)udDSPALCHangTime.Value;
            dr["ALC_HangThreshold"] = tbDSPALCHangThreshold.Value;

            dr["Power"] = console.PWR;

            dr["Dexp_On"] = chkTXNoiseGateEnabled.Checked;
            dr["Dexp_Threshold"] = (int)udTXNoiseGate.Value;
            dr["Dexp_Attenuate"] = (int)udTXNoiseGateAttenuate.Value;

            dr["VOX_On"] = chkTXVOXEnabled.Checked;
            dr["VOX_Threshold"] = (int)udTXVOXThreshold.Value;
            dr["VOX_HangTime"] = (int)udTXVOXHangTime.Value;

            dr["Tune_Power"] = (int)udTXTunePower.Value;
            dr["Tune_Meter_Type"] = (string)comboTXTUNMeter.Text;

            dr["TX_Limit_Slew"] = (bool)chkTXLimitSlew.Checked;
            dr["TXBlankingTime"] = (int)udTX1500PhoneBlanking.Value;
            dr["MicBoost"] = (bool)chkAudioMicBoost.Checked;

            dr["TX_AF_Level"] = console.TXAF;

            dr["AM_Carrier_Level"] = (int)udTXAMCarrierLevel.Value;

            dr["Show_TX_Filter"] = (bool)console.ShowTXFilter;

            dr["VAC1_On"] = (bool)chkAudioEnableVAC.Checked;
            dr["VAC1_Auto_On"] = (bool)chkAudioVACAutoEnable.Checked;
            dr["VAC1_RX_GAIN"] = (int)udAudioVACGainRX.Value;
            dr["VAC1_TX_GAIN"] = (int)udAudioVACGainTX.Value;
            dr["VAC1_Stereo_On"] = (bool)chkAudio2Stereo.Checked;
            dr["VAC1_Sample_Rate"] = (string)comboAudioSampleRate2.Text;
            dr["VAC1_Buffer_Size"] = (string)comboAudioBuffer2.Text;
            dr["VAC1_IQ_Output"] = (bool)chkAudioIQtoVAC.Checked;
            dr["VAC1_IQ_Correct"] = (bool)chkAudioCorrectIQ.Checked;
            dr["VAC1_PTT_OverRide"] = (bool)chkVACAllowBypass.Checked;
            dr["VAC1_Combine_Input_Channels"] = (bool)chkVACCombine.Checked;
            dr["VAC1_Latency_On"] = true;
            dr["VAC1_Latency_Duration"] = (int)udAudioLatency2.Value;

            dr["VAC2_On"] = (bool)chkVAC2Enable.Checked;
            dr["VAC2_Auto_On"] = (bool)chkVAC2AutoEnable.Checked;
            dr["VAC2_RX_GAIN"] = (int)udVAC2GainRX.Value;
            dr["VAC2_TX_GAIN"] = (int)udVAC2GainTX.Value;
            dr["VAC2_Stereo_On"] = (bool)chkAudioStereo3.Checked;
            dr["VAC2_Sample_Rate"] = (string)comboAudioSampleRate3.Text;
            dr["VAC2_Buffer_Size"] = (string)comboAudioBuffer3.Text;
            dr["VAC2_IQ_Output"] = (bool)chkVAC2DirectIQ.Checked;
            dr["VAC2_IQ_Correct"] = (bool)chkVAC2DirectIQCal.Checked;
            dr["VAC2_Combine_Input_Channels"] = (bool)chkVAC2Combine.Checked;
            dr["VAC2_Latency_On"] = true;
            dr["VAC2_Latency_Duration"] = (int)udVAC2Latency.Value;

            dr["Phone_RX_DSP_Buffer"] = (string)comboDSPPhoneRXBuf.Text;
            dr["Phone_TX_DSP_Buffer"] = (string)comboDSPPhoneTXBuf.Text;
            dr["Digi_RX_DSP_Buffer"] = (string)comboDSPDigRXBuf.Text;
            dr["Digi_TX_DSP_Buffer"] = (string)comboDSPDigTXBuf.Text;
            dr["CW_RX_DSP_Buffer"] = (string)comboDSPCWRXBuf.Text;

            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                    dr["Mic_Input_On"] = (string)console.fwcMixForm.MicInputSelected;
                    dr["Mic_Input_Level"] = (int)console.fwcMixForm.MicInput;
                    dr["Line_Input_On"] = (string)console.fwcMixForm.LineInRCASelected;
                    dr["Line_Input_Level"] = (int)console.fwcMixForm.LineInRCA;
                    dr["Balanced_Line_Input_On"] = (string)console.fwcMixForm.LineInPhonoSelected;
                    dr["Balanced_Line_Input_Level"] = (int)console.fwcMixForm.LineInPhono;
                    dr["FlexWire_Input_On"] = (string)console.fwcMixForm.LineInDB9Selected;
                    dr["FlexWire_Input_Level"] = (int)console.fwcMixForm.LineInDB9;
                    break;

                case Model.FLEX3000:
                    dr["Mic_Input_On"] = (string)console.flex3000MixerForm.MicInputSelected;
                    dr["Mic_Input_Level"] = (int)console.flex3000MixerForm.MicInput;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = (string)console.flex3000MixerForm.LineInDB9Selected;
                    dr["FlexWire_Input_Level"] = (int)console.flex3000MixerForm.LineInDB9;
                    break;

                case Model.FLEX1500:
                    dr["Mic_Input_On"] = (string)console.flex1500MixerForm.MicInputSelectedStr;
                    dr["Mic_Input_Level"] = (int)console.flex1500MixerForm.MicInput;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = (string)console.flex1500MixerForm.LineInDB9Selected;
                    dr["FlexWire_Input_Level"] = (int)console.flex1500MixerForm.FlexWireIn;
                    break;

                default:
                    dr["Mic_Input_On"] = "0";
                    dr["Mic_Input_Level"] = 0;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 0;
                    break;
            }

            if ((!comboTXProfileName.Items.Contains(name)))
            {
                Debug.WriteLine("ADDTXPROFILE");

                DB.ds.Tables["TxProfile"].Rows.Add(dr); // add a new TX profile

                comboTXProfileName.Items.Add(name);
                comboTXProfileName.Text = name;
            }

            console.UpdateTXProfile(name);

            if (chkAlwaysOnTop1.Checked == true) this.TopMost = true;// ke9ns add

        } // btnTXprofilesave()


        //=================================================================================
        // ke9ns remove TXprofile

        private bool profile_deleted = false;
        private void btnTXProfileDelete_Click(object sender, System.EventArgs e)
        {
            if (comboTXProfileName.Text == "Default")
            {
                MessageBox.Show(
                    "Deleting the Default transmit profile is not permitted.",
                    "Delete Profile Prohibited",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            DialogResult dr = MessageBox.Show(
                "Are you sure you want to delete the " + comboTXProfileName.Text + " TX Profile?",
                "Delete Profile?",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);

            if (dr == DialogResult.No)
                return;

            profile_deleted = true;

            DataRow[] rows = DB.ds.Tables["TxProfile"].Select("'" + comboTXProfileName.Text + "' = Name");

            if (rows.Length == 1) rows[0].Delete(); // ke9ns only delete this TXprofile if there is at least 1 remaining.

            int index = comboTXProfileName.SelectedIndex;
            comboTXProfileName.Items.Remove(comboTXProfileName.Text);
            if (comboTXProfileName.Items.Count > 0)
            {
                if (index > comboTXProfileName.Items.Count - 1)
                    index = comboTXProfileName.Items.Count - 1;

                comboTXProfileName.SelectedIndex = index;
            }

            console.UpdateTXProfile(comboTXProfileName.Text);

        } // btn DELETE TXPROFILE

        private void chkDCBlock_CheckedChanged(object sender, System.EventArgs e)
        {
            console.dsp.GetDSPTX(0).DCBlock = chkDCBlock.Checked;
        }

        private void chkTXVOXEnabled_CheckedChanged(object sender, System.EventArgs e)
        {
            //  if ((VACEnable || VAC2Enable) == true) chkTXVOXEnabled.Checked = false;      // ke9ns original code: keep VOX OFF if VAC1 or VAC2 enabled

            Audio.VOXEnabled = chkTXVOXEnabled.Checked;
            console.VOXEnable = chkTXVOXEnabled.Checked;

        }

        private void udTXVOXThreshold_ValueChanged(object sender, System.EventArgs e)
        {
            Audio.VOXThreshold = (float)udTXVOXThreshold.Value / 10000.0f;
            console.VOXSens = (int)udTXVOXThreshold.Value;

        }

        private void udTXVOXHangTime_ValueChanged(object sender, System.EventArgs e)
        {
            console.VOXHangTime = (int)udTXVOXHangTime.Value;
        }

        private void udTXNoiseGate_ValueChanged(object sender, System.EventArgs e)
        {
            console.dsp.GetDSPTX(0).TXSquelchThreshold = (float)udTXNoiseGate.Value;
            console.NoiseGate = (int)udTXNoiseGate.Value;
        }

        private void chkTXNoiseGateEnabled_CheckedChanged(object sender, System.EventArgs e)
        {
            console.dsp.GetDSPTX(0).TXSquelchOn = chkTXNoiseGateEnabled.Checked;
            console.NoiseGateEnabled = chkTXNoiseGateEnabled.Checked;
        }

        private void udTXAF_ValueChanged(object sender, System.EventArgs e)
        {
            console.TXAF = (int)udTXAF.Value;
        }

        private void udTXAMCarrierLevel_ValueChanged(object sender, System.EventArgs e)
        {
            console.dsp.GetDSPTX(0).TXAMCarrierLevel = Math.Sqrt((0.01 * (double)udTXAMCarrierLevel.Value) / 2); // max of 0.5 for half carrier, half mod
        }

        private void chkSaveTXProfileOnExit_CheckedChanged(object sender, EventArgs e)
        {
            console.SaveTXProfileOnExit = chkSaveTXProfileOnExit.Checked;
        }

        #endregion

        #region PA Settings Tab Event Handlers

        private void btnPAGainCalibration_Click(object sender, System.EventArgs e)
        {
            string s = "Is a 50 Ohm dummy load connected to the amplifier?\n" +
                "Failure to use a dummy load with this routine could cause damage to the amplifier.";
            if (radGenModelFLEX5000.Checked)
            {
                s = "Is a 50 Ohm dummy load connected to the correct antenna port (";
                switch (FWCAnt.ANT1)
                {
                    case FWCAnt.ANT1: s += "ANT 1"; break;
                        /*case FWCAnt.ANT2: s += "ANT 2"; break;
                        case FWCAnt.ANT3: s += "ANT 3"; break;*/
                }
                s += ")?\nFailure to connect a dummy load properly could cause damage to the radio.";
            }
            DialogResult dr = MessageBox.Show(s,
                "Warning: Is dummy load properly connected?",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (dr == DialogResult.No)
                return;

            btnPAGainCalibration.Enabled = false;
            progress = new Progress("Calibrate PA Gain");

            Thread t = new Thread(new ThreadStart(CalibratePAGain));
            t.Name = "PA Gain Calibration Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.AboveNormal;
            t.Start();

            if (console.PowerOn)
                progress.Show();
        }

        private void CalibratePAGain()
        {
            bool[] run = new bool[11];

            if (radPACalAllBands.Checked)
            {
                for (int i = 0; i < 11; i++) run[i] = true;
            }
            else
            {
                run[0] = chkPA160.Checked;
                run[1] = chkPA80.Checked;
                run[2] = chkPA60.Checked;
                run[3] = chkPA40.Checked;
                run[4] = chkPA30.Checked;
                run[5] = chkPA20.Checked;
                run[6] = chkPA17.Checked;
                run[7] = chkPA15.Checked;
                run[8] = chkPA12.Checked;
                run[9] = chkPA10.Checked;
                run[10] = chkPA6.Checked;
            }
            bool done = false;
            if (chkPANewCal.Checked) done = console.CalibratePAGain2(progress, run, false);
            else done = console.CalibratePAGain(progress, run, (int)udPACalPower.Value);
            if (done) MessageBox.Show(new Form { TopMost = true }, "PA Gain Calibration complete.");
            btnPAGainCalibration.Enabled = true;
        }

        private void udPAGain_ValueChanged(object sender, System.EventArgs e)
        {
            console.PWR = console.PWR;
        }

        private void btnPAGainReset_Click(object sender, System.EventArgs e)
        {
            udPAGain160.Value = 48.0M;
            udPAGain80.Value = 48.0M;
            udPAGain60.Value = 48.0M;
            udPAGain40.Value = 48.0M;
            udPAGain30.Value = 48.0M;
            udPAGain20.Value = 48.0M;
            udPAGain17.Value = 48.0M;
            udPAGain15.Value = 48.0M;
            udPAGain12.Value = 48.0M;
            udPAGain10.Value = 48.0M;
        }

        #endregion

        #region Appearance Tab Event Handlers

        private void clrbtnBackground_Changed(object sender, System.EventArgs e)
        {
            Display.DisplayBackgroundColor = clrbtnBackground.Color;
        }

        private void clrbtnGrid_Changed(object sender, System.EventArgs e)
        {
            Display.GridColor = clrbtnGrid.Color;

            // ke9ns add
            Display.BandTextColor = Color.FromArgb(tbPanGrid.Value, clrbtnGrid.Color); // ke9ns  combine color and alpha here

        }

        // ke9ns add
        private void tbPanGrid_Scroll(object sender, EventArgs e)
        {
            clrbtnGrid_Changed(this, EventArgs.Empty);
        }


        private void clrbtnZeroLine_Changed(object sender, System.EventArgs e)
        {
            Display.GridZeroColor = clrbtnZeroLine.Color;
        }

        private void clrbtnText_Changed(object sender, System.EventArgs e)
        {
            Display.GridTextColor = clrbtnText.Color;
        }

        private void clrbtnDataLine_Changed(object sender, System.EventArgs e)
        {
            Display.DataLineColor = clrbtnDataLine.Color;
        }

        private void clrbtnFilter_Changed(object sender, System.EventArgs e)
        {
            Display.DisplayFilterColor = Color.FromArgb(tbRX1FilterAlpha.Value, clrbtnFilter.Color);
        }

        private void udDisplayLineWidth_ValueChanged(object sender, System.EventArgs e)
        {
            Display.DisplayLineWidth = (float)udDisplayLineWidth.Value;
        }

        private void udBandSegmentBoxLineWidth_ValueChanged(object sender, System.EventArgs e)
        {
            Display.BandBoxWidth = (float)udBandSegmentBoxLineWidth.Value;
        }


        //================================================================================
        // ke9ns delete ORIGINAL meter colors (not used with the TR7 meter at the moment)

        private void clrbtnMeterLeft_Changed(object sender, System.EventArgs e)
        {
            console.MeterLeftColor = clrbtnMeterLeft.Color;
        }

        private void clrbtnMeterRight_Changed(object sender, System.EventArgs e)
        {
            console.MeterRightColor = clrbtnMeterRight.Color;
        }

        //==========================================================================================
        // ke9ns add ANALOG METER COLORS
        private void clrbtnMeterLow_Changed(object sender, EventArgs e)
        {
            console.AnalogLowColor = clrbtnMeterLow.Color;
        }

        private void clrbtnMeterIndicator_Changed(object sender, EventArgs e)
        {
            console.AnalogAVGColor = clrbtnMeterIndicator.Color;
        }

        private void clrbtnMeterHigh_Changed(object sender, EventArgs e)
        {
            console.AnalogHighColor = clrbtnMeterHigh.Color;
        }

        private void clrbtnMeterBackground_Changed(object sender, System.EventArgs e)
        {
            console.AnalogMeterBackgroundColor = clrbtnMeterBackground.Color;
            console.RingVFOColor = clrbtnVFORing.Color; // ke9ns add
        }

        //==========================================================================================

        private void clrbtnBtnSel_Changed(object sender, System.EventArgs e)
        {
            console.ButtonSelectedColor = clrbtnBtnSel.Color;
        }

        private void clrbtnVFODark_Changed(object sender, System.EventArgs e)
        {
            console.VFOTextDarkColor = clrbtnVFODark.Color;
        }

        private void clrbtnVFOLight_Changed(object sender, System.EventArgs e)
        {
            console.VFOTextLightColor = clrbtnVFOLight.Color;
        }

        private void clrbtnBandDark_Changed(object sender, System.EventArgs e)
        {
            console.BandTextDarkColor = clrbtnBandDark.Color;
        }

        private void clrbtnBandLight_Changed(object sender, System.EventArgs e)
        {
            console.BandTextLightColor = clrbtnBandLight.Color;
        }

        private void clrbtnPeakText_Changed(object sender, System.EventArgs e)
        {
            console.PeakTextColor = clrbtnPeakText.Color;
        }

        private void clrbtnOutOfBand_Changed(object sender, System.EventArgs e)
        {
            console.OutOfBandColor = clrbtnOutOfBand.Color;
        }

        private void chkVFOSmallLSD_CheckedChanged(object sender, System.EventArgs e)
        {
            console.SmallLSD = chkVFOSmallLSD.Checked;
        }

        private void clrbtnVFOSmallColor_Changed(object sender, System.EventArgs e)
        {
            console.SmallVFOColor = clrbtnVFOSmallColor.Color;
        }

        private void clrbtnPeakBackground_Changed(object sender, System.EventArgs e)
        {
            console.PeakBackgroundColor = clrbtnPeakBackground.Color;
        }



        private void clrbtnBandBackground_Changed(object sender, System.EventArgs e)
        {
            console.BandBackgroundColor = clrbtnBandBackground.Color;
            console.RingVFOColor = clrbtnVFORing.Color; // ke9ns add

        }

        private void clrbtnVFOBackground_Changed(object sender, System.EventArgs e)
        {
            console.VFOBackgroundColor = clrbtnVFOBackground.Color;
            console.RingVFOColor = clrbtnVFORing.Color; // ke9ns add
        }

        #endregion

        #region Keyboard Tab Event Handlers

        private void comboKBTuneUp1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            console.KeyTuneUp1 = (Keys)KeyList[comboKBTuneUp1.SelectedIndex];
        }

        private void comboKBTuneDown1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            console.KeyTuneDown1 = (Keys)KeyList[comboKBTuneDown1.SelectedIndex];
        }

        private void comboKBTuneUp2_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            console.KeyTuneUp2 = (Keys)KeyList[comboKBTuneUp2.SelectedIndex];
        }

        private void comboKBTuneDown2_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            console.KeyTuneDown2 = (Keys)KeyList[comboKBTuneDown2.SelectedIndex];
        }

        private void comboKBTuneUp3_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            console.KeyTuneUp3 = (Keys)KeyList[comboKBTuneUp3.SelectedIndex];
        }

        private void comboKBTuneDown3_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            console.KeyTuneDown3 = (Keys)KeyList[comboKBTuneDown3.SelectedIndex];
        }

        private void comboKBTuneUp4_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            console.KeyTuneUp4 = (Keys)KeyList[comboKBTuneUp4.SelectedIndex];
        }

        private void comboKBTuneDown4_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            console.KeyTuneDown4 = (Keys)KeyList[comboKBTuneDown4.SelectedIndex];
        }

        private void comboKBTuneUp5_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            console.KeyTuneUp5 = (Keys)KeyList[comboKBTuneUp5.SelectedIndex];
        }

        private void comboKBTuneDown5_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            console.KeyTuneDown5 = (Keys)KeyList[comboKBTuneDown5.SelectedIndex];
        }

        private void comboKBTuneUp6_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            console.KeyTuneUp6 = (Keys)KeyList[comboKBTuneUp6.SelectedIndex];
        }

        private void comboKBTuneDown6_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            console.KeyTuneDown6 = (Keys)KeyList[comboKBTuneDown6.SelectedIndex];
        }

        private void comboKBTuneUp7_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            console.KeyTuneUp7 = (Keys)KeyList[comboKBTuneUp7.SelectedIndex];
        }

        private void comboKBTuneDown7_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            console.KeyTuneDown7 = (Keys)KeyList[comboKBTuneDown7.SelectedIndex];
        }

        private void comboKBBandUp_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            console.KeyBandUp = (Keys)KeyList[comboKBBandUp.SelectedIndex];
        }

        private void comboKBBandDown_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            console.KeyBandDown = (Keys)KeyList[comboKBBandDown.SelectedIndex];
        }

        private void comboKBFilterUp_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            console.KeyFilterUp = (Keys)KeyList[comboKBFilterUp.SelectedIndex];
        }

        private void comboKBFilterDown_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            console.KeyFilterDown = (Keys)KeyList[comboKBFilterDown.SelectedIndex];
        }

        private void comboKBModeUp_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            console.KeyModeUp = (Keys)KeyList[comboKBModeUp.SelectedIndex];
        }

        private void comboKBModeDown_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            console.KeyModeDown = (Keys)KeyList[comboKBModeDown.SelectedIndex];
        }

        private void comboKBCWDot_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            console.KeyCWDot = (Keys)KeyList[comboKBCWDot.SelectedIndex];
        }

        private void comboKBCWDash_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            console.KeyCWDash = (Keys)KeyList[comboKBCWDash.SelectedIndex];
        }

        private void comboKBRITUp_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            console.KeyRITUp = (Keys)KeyList[comboKBRITUp.SelectedIndex];
        }

        private void comboKBRITDown_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            console.KeyRITDown = (Keys)KeyList[comboKBRITDown.SelectedIndex];
        }

        private void comboKBXITUp_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            console.KeyXITUp = (Keys)KeyList[comboKBXITUp.SelectedIndex];
        }

        private void comboKBXITDown_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            console.KeyXITDown = (Keys)KeyList[comboKBXITDown.SelectedIndex];
        }

        #endregion

        #region Ext Ctrl Tab Event Handlers

        private void chkExtRX160_CheckedChanged(object sender, System.EventArgs e)
        {
            int val = 0;
            if (chkExtRX1601.Checked) val += 1 << 0;
            if (chkExtRX1602.Checked) val += 1 << 1;
            if (chkExtRX1603.Checked) val += 1 << 2;
            if (chkExtRX1604.Checked) val += 1 << 3;
            if (chkExtRX1605.Checked) val += 1 << 4;
            if (chkExtRX1606.Checked) val += 1 << 5;

            console.X2160RX = (byte)val;
        }

        private void chkExtTX160_CheckedChanged(object sender, System.EventArgs e)
        {
            int val = 0;
            if (chkExtTX1601.Checked) val += 1 << 0;
            if (chkExtTX1602.Checked) val += 1 << 1;
            if (chkExtTX1603.Checked) val += 1 << 2;
            if (chkExtTX1604.Checked) val += 1 << 3;
            if (chkExtTX1605.Checked) val += 1 << 4;
            if (chkExtTX1606.Checked) val += 1 << 5;

            console.X2160TX = (byte)val;
        }

        private void chkExtRX80_CheckedChanged(object sender, System.EventArgs e)
        {
            int val = 0;
            if (chkExtRX801.Checked) val += 1 << 0;
            if (chkExtRX802.Checked) val += 1 << 1;
            if (chkExtRX803.Checked) val += 1 << 2;
            if (chkExtRX804.Checked) val += 1 << 3;
            if (chkExtRX805.Checked) val += 1 << 4;
            if (chkExtRX806.Checked) val += 1 << 5;

            console.X280RX = (byte)val;
        }

        private void chkExtTX80_CheckedChanged(object sender, System.EventArgs e)
        {
            int val = 0;
            if (chkExtTX801.Checked) val += 1 << 0;
            if (chkExtTX802.Checked) val += 1 << 1;
            if (chkExtTX803.Checked) val += 1 << 2;
            if (chkExtTX804.Checked) val += 1 << 3;
            if (chkExtTX805.Checked) val += 1 << 4;
            if (chkExtTX806.Checked) val += 1 << 5;

            console.X280TX = (byte)val;
        }

        private void chkExtRX60_CheckedChanged(object sender, System.EventArgs e)
        {
            int val = 0;
            if (chkExtRX601.Checked) val += 1 << 0;
            if (chkExtRX602.Checked) val += 1 << 1;
            if (chkExtRX603.Checked) val += 1 << 2;
            if (chkExtRX604.Checked) val += 1 << 3;
            if (chkExtRX605.Checked) val += 1 << 4;
            if (chkExtRX606.Checked) val += 1 << 5;

            console.X260RX = (byte)val;
        }

        private void chkExtTX60_CheckedChanged(object sender, System.EventArgs e)
        {
            int val = 0;
            if (chkExtTX601.Checked) val += 1 << 0;
            if (chkExtTX602.Checked) val += 1 << 1;
            if (chkExtTX603.Checked) val += 1 << 2;
            if (chkExtTX604.Checked) val += 1 << 3;
            if (chkExtTX605.Checked) val += 1 << 4;
            if (chkExtTX606.Checked) val += 1 << 5;

            console.X260TX = (byte)val;
        }

        private void chkExtRX40_CheckedChanged(object sender, System.EventArgs e)
        {
            int val = 0;
            if (chkExtRX401.Checked) val += 1 << 0;
            if (chkExtRX402.Checked) val += 1 << 1;
            if (chkExtRX403.Checked) val += 1 << 2;
            if (chkExtRX404.Checked) val += 1 << 3;
            if (chkExtRX405.Checked) val += 1 << 4;
            if (chkExtRX406.Checked) val += 1 << 5;

            console.X240RX = (byte)val;
        }

        private void chkExtTX40_CheckedChanged(object sender, System.EventArgs e)
        {
            int val = 0;
            if (chkExtTX401.Checked) val += 1 << 0;
            if (chkExtTX402.Checked) val += 1 << 1;
            if (chkExtTX403.Checked) val += 1 << 2;
            if (chkExtTX404.Checked) val += 1 << 3;
            if (chkExtTX405.Checked) val += 1 << 4;
            if (chkExtTX406.Checked) val += 1 << 5;

            console.X240TX = (byte)val;
        }

        private void chkExtRX30_CheckedChanged(object sender, System.EventArgs e)
        {
            int val = 0;
            if (chkExtRX301.Checked) val += 1 << 0;
            if (chkExtRX302.Checked) val += 1 << 1;
            if (chkExtRX303.Checked) val += 1 << 2;
            if (chkExtRX304.Checked) val += 1 << 3;
            if (chkExtRX305.Checked) val += 1 << 4;
            if (chkExtRX306.Checked) val += 1 << 5;

            console.X230RX = (byte)val;
        }

        private void chkExtTX30_CheckedChanged(object sender, System.EventArgs e)
        {
            int val = 0;
            if (chkExtTX301.Checked) val += 1 << 0;
            if (chkExtTX302.Checked) val += 1 << 1;
            if (chkExtTX303.Checked) val += 1 << 2;
            if (chkExtTX304.Checked) val += 1 << 3;
            if (chkExtTX305.Checked) val += 1 << 4;
            if (chkExtTX306.Checked) val += 1 << 5;

            console.X230TX = (byte)val;
        }

        private void chkExtRX20_CheckedChanged(object sender, System.EventArgs e)
        {
            int val = 0;
            if (chkExtRX201.Checked) val += 1 << 0;
            if (chkExtRX202.Checked) val += 1 << 1;
            if (chkExtRX203.Checked) val += 1 << 2;
            if (chkExtRX204.Checked) val += 1 << 3;
            if (chkExtRX205.Checked) val += 1 << 4;
            if (chkExtRX306.Checked) val += 1 << 5;

            console.X220RX = (byte)val;
        }

        private void chkExtTX20_CheckedChanged(object sender, System.EventArgs e)
        {
            int val = 0;
            if (chkExtTX201.Checked) val += 1 << 0;
            if (chkExtTX202.Checked) val += 1 << 1;
            if (chkExtTX203.Checked) val += 1 << 2;
            if (chkExtTX204.Checked) val += 1 << 3;
            if (chkExtTX205.Checked) val += 1 << 4;
            if (chkExtTX306.Checked) val += 1 << 5;

            console.X220TX = (byte)val;
        }

        private void chkExtRX17_CheckedChanged(object sender, System.EventArgs e)
        {
            int val = 0;
            if (chkExtRX171.Checked) val += 1 << 0;
            if (chkExtRX172.Checked) val += 1 << 1;
            if (chkExtRX173.Checked) val += 1 << 2;
            if (chkExtRX174.Checked) val += 1 << 3;
            if (chkExtRX175.Checked) val += 1 << 4;
            if (chkExtRX176.Checked) val += 1 << 5;

            console.X217RX = (byte)val;
        }

        private void chkExtTX17_CheckedChanged(object sender, System.EventArgs e)
        {
            int val = 0;
            if (chkExtTX171.Checked) val += 1 << 0;
            if (chkExtTX172.Checked) val += 1 << 1;
            if (chkExtTX173.Checked) val += 1 << 2;
            if (chkExtTX174.Checked) val += 1 << 3;
            if (chkExtTX175.Checked) val += 1 << 4;
            if (chkExtTX176.Checked) val += 1 << 5;

            console.X217TX = (byte)val;
        }

        private void chkExtRX15_CheckedChanged(object sender, System.EventArgs e)
        {
            int val = 0;
            if (chkExtRX151.Checked) val += 1 << 0;
            if (chkExtRX152.Checked) val += 1 << 1;
            if (chkExtRX153.Checked) val += 1 << 2;
            if (chkExtRX154.Checked) val += 1 << 3;
            if (chkExtRX155.Checked) val += 1 << 4;
            if (chkExtRX156.Checked) val += 1 << 5;

            console.X215RX = (byte)val;
        }

        private void chkExtTX15_CheckedChanged(object sender, System.EventArgs e)
        {
            int val = 0;
            if (chkExtTX151.Checked) val += 1 << 0;
            if (chkExtTX152.Checked) val += 1 << 1;
            if (chkExtTX153.Checked) val += 1 << 2;
            if (chkExtTX154.Checked) val += 1 << 3;
            if (chkExtTX155.Checked) val += 1 << 4;
            if (chkExtTX156.Checked) val += 1 << 5;

            console.X215TX = (byte)val;
        }

        private void chkExtRX12_CheckedChanged(object sender, System.EventArgs e)
        {
            int val = 0;
            if (chkExtRX121.Checked) val += 1 << 0;
            if (chkExtRX122.Checked) val += 1 << 1;
            if (chkExtRX123.Checked) val += 1 << 2;
            if (chkExtRX124.Checked) val += 1 << 3;
            if (chkExtRX125.Checked) val += 1 << 4;
            if (chkExtRX126.Checked) val += 1 << 5;

            console.X212RX = (byte)val;
        }

        private void chkExtTX12_CheckedChanged(object sender, System.EventArgs e)
        {
            int val = 0;
            if (chkExtTX121.Checked) val += 1 << 0;
            if (chkExtTX122.Checked) val += 1 << 1;
            if (chkExtTX123.Checked) val += 1 << 2;
            if (chkExtTX124.Checked) val += 1 << 3;
            if (chkExtTX125.Checked) val += 1 << 4;
            if (chkExtTX126.Checked) val += 1 << 5;

            console.X212TX = (byte)val;
        }

        private void chkExtRX10_CheckedChanged(object sender, System.EventArgs e)
        {
            int val = 0;
            if (chkExtRX101.Checked) val += 1 << 0;
            if (chkExtRX102.Checked) val += 1 << 1;
            if (chkExtRX103.Checked) val += 1 << 2;
            if (chkExtRX104.Checked) val += 1 << 3;
            if (chkExtRX105.Checked) val += 1 << 4;
            if (chkExtRX106.Checked) val += 1 << 5;

            console.X210RX = (byte)val;
        }

        private void chkExtTX10_CheckedChanged(object sender, System.EventArgs e)
        {
            int val = 0;
            if (chkExtTX101.Checked) val += 1 << 0;
            if (chkExtTX102.Checked) val += 1 << 1;
            if (chkExtTX103.Checked) val += 1 << 2;
            if (chkExtTX104.Checked) val += 1 << 3;
            if (chkExtTX105.Checked) val += 1 << 4;
            if (chkExtTX106.Checked) val += 1 << 5;

            console.X210TX = (byte)val;
        }

        private void chkExtRX6_CheckedChanged(object sender, System.EventArgs e)
        {
            int val = 0;
            if (chkExtRX61.Checked) val += 1 << 0;
            if (chkExtRX62.Checked) val += 1 << 1;
            if (chkExtRX63.Checked) val += 1 << 2;
            if (chkExtRX64.Checked) val += 1 << 3;
            if (chkExtRX65.Checked) val += 1 << 4;
            if (chkExtRX66.Checked) val += 1 << 5;

            console.X26RX = (byte)val;
        }

        private void chkExtTX6_CheckedChanged(object sender, System.EventArgs e)
        {
            int val = 0;
            if (chkExtTX61.Checked) val += 1 << 0;
            if (chkExtTX62.Checked) val += 1 << 1;
            if (chkExtTX63.Checked) val += 1 << 2;
            if (chkExtTX64.Checked) val += 1 << 3;
            if (chkExtTX65.Checked) val += 1 << 4;
            if (chkExtTX66.Checked) val += 1 << 5;

            console.X26TX = (byte)val;
        }

        private void chkExtRX2_CheckedChanged(object sender, System.EventArgs e)
        {
            int val = 0;
            if (chkExtRX21.Checked) val += 1 << 0;
            if (chkExtRX22.Checked) val += 1 << 1;
            if (chkExtRX23.Checked) val += 1 << 2;
            if (chkExtRX24.Checked) val += 1 << 3;
            if (chkExtRX25.Checked) val += 1 << 4;
            if (chkExtRX26.Checked) val += 1 << 5;

            console.X22RX = (byte)val;
        }

        private void chkExtTX2_CheckedChanged(object sender, System.EventArgs e)
        {
            int val = 0;
            if (chkExtTX21.Checked) val += 1 << 0;
            if (chkExtTX22.Checked) val += 1 << 1;
            if (chkExtTX23.Checked) val += 1 << 2;
            if (chkExtTX24.Checked) val += 1 << 3;
            if (chkExtTX25.Checked) val += 1 << 4;
            if (chkExtTX26.Checked) val += 1 << 5;

            console.X22TX = (byte)val;
        }

        private void chkExtEnable_CheckedChanged(object sender, System.EventArgs e)
        {
            grpExtRX.Enabled = chkExtEnable.Checked;
            grpExtTX.Enabled = chkExtEnable.Checked;
            console.ExtCtrlEnabled = chkExtEnable.Checked;
        }

        #endregion

        #region CAT Setup event handlers 

        public void initCATandPTTprops()
        {
            if (comboCATPort.Text.StartsWith("COM"))
                console.CATPort = Int32.Parse(comboCATPort.Text.Substring(3));

            // ke9ns add: .180
            if (comboCATPort2.Text.StartsWith("COM"))
                console.CATPort2 = Int32.Parse(comboCATPort2.Text.Substring(3));

            if (comboCATPort3.Text.StartsWith("COM"))
                console.CATPort3 = Int32.Parse(comboCATPort3.Text.Substring(3));

            if (comboCATPort4.Text.StartsWith("COM"))
                console.CATPort4 = Int32.Parse(comboCATPort4.Text.Substring(3));

            if (comboCATPort5.Text.StartsWith("COM"))
                console.CATPort5 = Int32.Parse(comboCATPort5.Text.Substring(3));

            if (comboCATPort6.Text.StartsWith("COM")) // .200
                console.CATPort6 = Int32.Parse(comboCATPort6.Text.Substring(3));

            if (comboROTORPort.Text.StartsWith("COM"))
                console.ROTORPort = Int32.Parse(comboROTORPort.Text.Substring(3));  // ke9ns add

            console.CATPTTRTS = chkCATPTT_RTS.Checked;
            console.CATPTTDTR = chkCATPTT_DTR.Checked;
            //console.PTTBitBangEnabled = chkCATPTTEnabled.Checked; 

            if (comboCATPTTPort.Text.StartsWith("COM")) console.CATPTTBitBangPort = Int32.Parse(comboCATPTTPort.Text.Substring(3));

            console.CATBaudRate = Convert.ToInt32((string)comboCATbaud.SelectedItem, 10);
            console.CATParity = SDRSerialPort.StringToParity((string)comboCATparity.SelectedItem);
            console.CATDataBits = int.Parse((string)comboCATdatabits.SelectedItem);
            console.CATStopBits = SDRSerialPort.StringToStopBits((string)comboCATstopbits.SelectedItem);

            console.CATEnabled = chkCATEnable.Checked;

            console.CATEnabled2 = chkCATEnable2.Checked; // ke9ns add .180
            console.CATEnabled3 = chkCATEnable3.Checked; // ke9ns add .180
            console.CATEnabled4 = chkCATEnable4.Checked; // ke9ns add .180
            console.CATEnabled5 = chkCATEnable5.Checked; // ke9ns add .180
            console.CATEnabled6 = chkCATEnable6.Checked; // ke9ns add .200

            //   console.ROTOREnabled = chkROTOREnable.Checked; // ke9ns add

            // make sure the enabled state of bitbang ptt is correct 
            if (chkCATPTT_RTS.Checked || chkCATPTT_DTR.Checked)
            {
                chkCATPTTEnabled.Enabled = true;
            }
            else
            {
                chkCATPTTEnabled.Enabled = false;
                chkCATPTTEnabled.Checked = false;
            }
        }

        // called in error cases to set the dialiog vars from 
        // the console properties -- sort of ugly, we should only have 1 copy 
        // of this stuff 
        public void copyCATPropsToDialogVars()
        {
            chkCATEnable.Checked = console.CATEnabled;

            string port = "COM" + console.CATPort.ToString();

            if (comboCATPort.Items.Contains(port))
                comboCATPort.Text = port;

            if (comboCATPort2.Items.Contains(port)) // ke9ns add .180
                comboCATPort2.Text = port;

            if (comboCATPort3.Items.Contains(port)) // ke9ns add .180
                comboCATPort3.Text = port;

            if (comboCATPort4.Items.Contains(port)) // ke9ns add .180
                comboCATPort4.Text = port;

            if (comboCATPort5.Items.Contains(port)) // ke9ns add .180
                comboCATPort5.Text = port;

            if (comboCATPort6.Items.Contains(port)) // ke9ns add .200
                comboCATPort6.Text = port;

            chkCATPTT_RTS.Checked = console.CATPTTRTS;
            chkCATPTT_DTR.Checked = console.CATPTTDTR;
            chkCATPTTEnabled.Checked = console.PTTBitBangEnabled;

            chkROTOREnable.Checked = console.ROTOREnabled; // ke9ns add

            port = "COM" + console.ROTORPort.ToString(); // ke9ns add

            if (comboROTORPort.Items.Contains(port)) // ke9ns add
                comboROTORPort.Text = port;

            port = "COM" + console.CATPTTBitBangPort.ToString();
            if (comboCATPTTPort.Items.Contains(port))
                comboCATPTTPort.Text = port;

            // wjt fixme -- need to hand baudrate, parity, data, stop -- see initCATandPTTprops 
        }

        private void chkCATEnable_CheckedChanged(object sender, System.EventArgs e)
        {
            if (initializing) return;

            if (comboCATPort.Text == "" || !comboCATPort.Text.StartsWith("COM"))
            {
                if (chkCATEnable.Focused && chkCATEnable.Checked)
                {
                    MessageBox.Show(new Form { TopMost = true }, "The CAT port \"" + comboCATPort.Text + "\" is not a valid port.\n" +
                        "Please select another port.");
                    chkCATEnable.Checked = false;
                }
                return;
            }

            // make sure we're not using the same comm port as the bit banger 
            //	if ( chkCATEnable.Checked && console.PTTBitBangEnabled && 
            //		( comboCATPort.Text == comboCATPTTPort.Text ) )
            string temp2 = comboCATPort.Text;

            if (
                 (chkCATEnable.Checked &&
                    (temp2 == comboCATPTTPort.Text || temp2 == comboCATPort2.Text ||
                    temp2 == comboCATPort3.Text || temp2 == comboCATPort4.Text
                    || temp2 == comboCATPort5.Text || temp2 == comboCATPort6.Text || temp2 == comboROTORPort.Text)
                 )

               )
            {
                MessageBox.Show(new Form { TopMost = true }, "CAT port cannot be the same as other CAT, PTT or ROTOR", "Port Selection Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                chkCATEnable.Checked = false;
            }

            // if enabled, disable changing of serial port 
            bool enable_sub_fields = !chkCATEnable.Checked;

            comboCATPort.Enabled = enable_sub_fields;
         
            enableCAT_HardwareFields(enable_sub_fields);

            if (chkCATEnable.Checked)
            {
                try
                {
                    console.CATEnabled = true;
                }
                catch (Exception ex)
                {
                    console.CATEnabled = false;
                    chkCATEnable.Checked = false;
                    MessageBox.Show(new Form { TopMost = true }, "Could not initialize CAT control.  Exception was:\n\n " + ex.Message +
                        "\n\nCAT control has been disabled.", "Error Initializing CAT control",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                if (comboKeyerConnSecondary.Text == "CAT" && chkCATEnable.Focused)
                {
                    MessageBox.Show(new Form { TopMost = true }, "The Secondary Keyer option has been changed to None since CAT has been disabled.",
                        "CAT Disabled",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    comboKeyerConnSecondary.Text = "None";
                }
                console.CATEnabled = false;
            }
        } // chkCATEnable_CheckedChanged



        private void chkCATEnable2_CheckedChanged(object sender, EventArgs e)
        {
           
            if (initializing) return;

            if (comboCATPort2.Text == "" || !comboCATPort2.Text.StartsWith("COM"))
            {
                if (chkCATEnable2.Focused && chkCATEnable2.Checked)
                {
                    MessageBox.Show(new Form { TopMost = true }, "The CAT2 port \"" + comboCATPort2.Text + "\" is not a valid port.\n" +
                        "Please select another port.");
                    chkCATEnable2.Checked = false;
                }
                return;
            }

            string temp2 = comboCATPort2.Text;

            if (
                 (chkCATEnable2.Checked &&
                    (temp2 == comboCATPTTPort.Text || temp2 == comboCATPort.Text || 
                    temp2 == comboCATPort3.Text || temp2 == comboCATPort4.Text
                    || temp2 == comboCATPort5.Text || temp2 == comboCATPort6.Text || temp2 == comboROTORPort.Text) 
                 )

               )
            {
                MessageBox.Show(new Form { TopMost = true }, "CAT2 port cannot be the same as other CAT, PTT or ROTOR", "Port Selection Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                chkCATEnable2.Checked = false;
            }

            // if enabled, disable changing of serial port 
            bool enable_sub_fields = !chkCATEnable2.Checked;

            // ke9ns add:  2-5
            comboCATPort2.Enabled = enable_sub_fields;


            enableCAT_HardwareFields(enable_sub_fields);

            if (chkCATEnable2.Checked)
            {
                try
                {
                    console.CATEnabled2 = true;
                }
                catch (Exception ex)
                {
                    console.CATEnabled2 = false;
                    chkCATEnable2.Checked = false;
                    MessageBox.Show(new Form { TopMost = true }, "Could not initialize CAT2 control.  Exception was:\n\n " + ex.Message +
                        "\n\nCAT2 control has been disabled.", "Error Initializing CAT2 control",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                if (comboKeyerConnSecondary.Text == "CAT" && chkCATEnable2.Focused)
                {
                    MessageBox.Show(new Form { TopMost = true }, "The Secondary Keyer option has been changed to None since CAT2 has been disabled.",
                        "CAT2 Disabled",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    comboKeyerConnSecondary.Text = "None";
                }
                console.CATEnabled2 = false;
            }


        } //  chkcat2enable

        private void chkCATEnable3_CheckedChanged(object sender, EventArgs e)
        {

            if (initializing) return;

            if (comboCATPort3.Text == "" || !comboCATPort3.Text.StartsWith("COM"))
            {
                if (chkCATEnable3.Focused && chkCATEnable3.Checked)
                {
                    MessageBox.Show(new Form { TopMost = true }, "The CAT3 port \"" + comboCATPort3.Text + "\" is not a valid port.\n" +
                        "Please select another port.");
                    chkCATEnable3.Checked = false;
                }
                return;
            }

            string temp2 = comboCATPort3.Text;

            if (
                 (chkCATEnable3.Checked &&
                    (temp2 == comboCATPTTPort.Text || temp2 == comboCATPort.Text ||
                    temp2 == comboCATPort2.Text || temp2 == comboCATPort4.Text
                    || temp2 == comboCATPort5.Text || temp2 == comboCATPort6.Text || temp2 == comboROTORPort.Text)
                 )

               )
            {
                MessageBox.Show(new Form { TopMost = true }, "CAT3 port cannot be the same as other CAT, PTT or ROTOR", "Port Selection Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                chkCATEnable3.Checked = false;
            }

            // if enabled, disable changing of serial port 
            bool enable_sub_fields = !chkCATEnable3.Checked;

            // ke9ns add:  2-5
            comboCATPort3.Enabled = enable_sub_fields;


            enableCAT_HardwareFields(enable_sub_fields);

            if (chkCATEnable3.Checked)
            {
                try
                {
                    console.CATEnabled3 = true;
                }
                catch (Exception ex)
                {
                    console.CATEnabled3 = false;
                    chkCATEnable3.Checked = false;
                    MessageBox.Show(new Form { TopMost = true }, "Could not initialize CAT3 control.  Exception was:\n\n " + ex.Message +
                        "\n\nCAT3 control has been disabled.", "Error Initializing CAT3 control",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                if (comboKeyerConnSecondary.Text == "CAT" && chkCATEnable3.Focused)
                {
                    MessageBox.Show(new Form { TopMost = true }, "The Secondary Keyer option has been changed to None since CAT3 has been disabled.",
                        "CAT3 Disabled",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    comboKeyerConnSecondary.Text = "None";
                }
                console.CATEnabled3 = false;
            }


        } //chkCATEnable3

        private void chkCATEnable4_CheckedChanged(object sender, EventArgs e)
        {

            if (initializing) return;

            if (comboCATPort4.Text == "" || !comboCATPort4.Text.StartsWith("COM"))
            {
                if (chkCATEnable4.Focused && chkCATEnable4.Checked)
                {
                    MessageBox.Show(new Form { TopMost = true }, "The CAT4 port \"" + comboCATPort4.Text + "\" is not a valid port.\n" +
                        "Please select another port.");
                    chkCATEnable4.Checked = false;
                }
                return;
            }

            string temp2 = comboCATPort4.Text;

            if (
                 (chkCATEnable4.Checked &&
                    (temp2 == comboCATPTTPort.Text || temp2 == comboCATPort.Text ||
                    temp2 == comboCATPort2.Text || temp2 == comboCATPort3.Text
                    || temp2 == comboCATPort5.Text || temp2 == comboCATPort6.Text || temp2 == comboROTORPort.Text)
                 )

               )
            {
                MessageBox.Show(new Form { TopMost = true }, "CAT4 port cannot be the same as other CAT, PTT or ROTOR", "Port Selection Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                chkCATEnable4.Checked = false;
            }

            // if enabled, disable changing of serial port 
            bool enable_sub_fields = !chkCATEnable4.Checked;

            // ke9ns add:  2-5
            comboCATPort4.Enabled = enable_sub_fields;


            enableCAT_HardwareFields(enable_sub_fields);

            if (chkCATEnable4.Checked)
            {
                try
                {
                    console.CATEnabled4 = true;
                }
                catch (Exception ex)
                {
                    console.CATEnabled4 = false;
                    chkCATEnable4.Checked = false;
                    MessageBox.Show(new Form { TopMost = true }, "Could not initialize CAT4 control.  Exception was:\n\n " + ex.Message +
                        "\n\nCAT4 control has been disabled.", "Error Initializing CAT4 control",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                if (comboKeyerConnSecondary.Text == "CAT" && chkCATEnable4.Focused)
                {
                    MessageBox.Show(new Form { TopMost = true }, "The Secondary Keyer option has been changed to None since CAT4 has been disabled.",
                        "CAT4 Disabled",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    comboKeyerConnSecondary.Text = "None";
                }
                console.CATEnabled4 = false;
            }



        } //chkCATEnable4

        private void chkCATEnable5_CheckedChanged(object sender, EventArgs e)
        {
          
            if (initializing) return;

            if (comboCATPort5.Text == "" || !comboCATPort5.Text.StartsWith("COM"))
            {
                if (chkCATEnable5.Focused && chkCATEnable5.Checked)
                {
                    MessageBox.Show(new Form { TopMost = true }, "The CAT5 port \"" + comboCATPort5.Text + "\" is not a valid port.\n" +
                        "Please select another port.");
                    chkCATEnable5.Checked = false;
                }
                return;
            }
            string temp2 = comboCATPort5.Text;

            if (
                 (chkCATEnable5.Checked &&
                    (temp2 == comboCATPTTPort.Text || temp2 == comboCATPort.Text ||
                    temp2 == comboCATPort2.Text || temp2 == comboCATPort3.Text
                    || temp2 == comboCATPort4.Text || temp2 == comboCATPort6.Text || temp2 == comboROTORPort.Text)
                 )

               )
            {
                MessageBox.Show(new Form { TopMost = true }, "CAT5 port cannot be the same as other CAT, PTT or ROTOR", "Port Selection Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                chkCATEnable5.Checked = false;
            }

            // if enabled, disable changing of serial port 
            bool enable_sub_fields = !chkCATEnable5.Checked;

            // ke9ns add:  2-5
            comboCATPort5.Enabled = enable_sub_fields;


            enableCAT_HardwareFields(enable_sub_fields);

            if (chkCATEnable5.Checked)
            {
                try
                {
                    console.CATEnabled5 = true;
                }
                catch (Exception ex)
                {
                    console.CATEnabled5 = false;
                    chkCATEnable5.Checked = false;
                    MessageBox.Show(new Form { TopMost = true }, "Could not initialize CAT5 control.  Exception was:\n\n " + ex.Message +
                        "\n\nCAT5 control has been disabled.", "Error Initializing CAT5 control",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                if (comboKeyerConnSecondary.Text == "CAT" && chkCATEnable5.Focused)
                {
                    MessageBox.Show(new Form { TopMost = true }, "The Secondary Keyer option has been changed to None since CAT5 has been disabled.",
                        "CAT5 Disabled",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    comboKeyerConnSecondary.Text = "None";
                }
                console.CATEnabled5 = false;
            }



        } // chkCATEnable5_CheckedChanged

        // KE9NS ADD .200
        private void chkCATEnable6_CheckedChanged(object sender, EventArgs e)
        {

            if (initializing) return;



            if (comboCATPort6.Text == "" || !comboCATPort6.Text.StartsWith("COM"))
            {
                if (chkCATEnable6.Focused && chkCATEnable6.Checked)
                {
                    MessageBox.Show(new Form { TopMost = true }, "The CAT6 port \"" + comboCATPort6.Text + "\" is not a valid port.\n" +
                        "Please select another port.");
                    chkCATEnable6.Checked = false;
                }
                return;
            }
            string temp2 = comboCATPort6.Text;

            if (
                 (chkCATEnable6.Checked &&
                    (temp2 == comboCATPTTPort.Text || temp2 == comboCATPort.Text ||
                    temp2 == comboCATPort2.Text || temp2 == comboCATPort3.Text
                    || temp2 == comboCATPort4.Text || temp2 == comboCATPort5.Text || temp2 == comboROTORPort.Text)
                 )

               )
            {
                MessageBox.Show(new Form { TopMost = true }, "CAT6 port cannot be the same as other CAT, PTT or ROTOR", "Port Selection Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                chkCATEnable6.Checked = false;
            }

            // if enabled, disable changing of serial port 
            bool enable_sub_fields = !chkCATEnable6.Checked;

            // ke9ns add:  2-5
            comboCATPort6.Enabled = enable_sub_fields;


            enableCAT_HardwareFields(enable_sub_fields);

            if (chkCATEnable6.Checked)
            {
                try
                {
                    console.CATEnabled6 = true;
                }
                catch (Exception ex)
                {
                    console.CATEnabled6 = false;
                    chkCATEnable6.Checked = false;
                    MessageBox.Show(new Form { TopMost = true }, "Could not initialize CAT6 control.  Exception was:\n\n " + ex.Message +
                        "\n\nCAT6 control has been disabled.", "Error Initializing CAT6 control",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                if (comboKeyerConnSecondary.Text == "CAT" && chkCATEnable6.Focused)
                {
                    MessageBox.Show(new Form { TopMost = true }, "The Secondary Keyer option has been changed to None since CAT6 has been disabled.",
                        "CAT6 Disabled",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    comboKeyerConnSecondary.Text = "None";
                }
                console.CATEnabled6 = false;
            }


        } // chkCATEnable6_CheckedChanged






        private void enableCAT_HardwareFields(bool enable)
        {
            comboCATbaud.Enabled = enable;
            comboCATparity.Enabled = enable;
            comboCATdatabits.Enabled = enable;
            comboCATstopbits.Enabled = enable;
        }

        private void doEnablementOnBitBangEnable()
        {
            if (comboCATPTTPort.Text != "None" && (chkCATPTT_RTS.Checked || chkCATPTT_DTR.Checked))  // if RTS or DTR & port is not None, enable 
            {
                chkCATPTTEnabled.Enabled = true;
            }
            else
            {
                chkCATPTTEnabled.Enabled = false;
                chkCATPTTEnabled.Checked = false; // make sure it is not checked 
            }
        }

        private void chkCATPTT_RTS_CheckedChanged(object sender, System.EventArgs e)
        {
            console.CATPTTRTS = chkCATPTT_RTS.Checked;
            doEnablementOnBitBangEnable();
        }

        private void chkCATPTT_DTR_CheckedChanged(object sender, System.EventArgs e)
        {
            console.CATPTTDTR = chkCATPTT_DTR.Checked;
            doEnablementOnBitBangEnable();
        }

        private void chkCATPTTEnabled_CheckedChanged(object sender, System.EventArgs e)
        {
            if (initializing) return;

            bool enable_sub_fields;

            if (comboCATPTTPort.Text == "" || !comboCATPTTPort.Text.StartsWith("COM"))
            {
                if (chkCATPTTEnabled.Focused && chkCATPTTEnabled.Checked)
                {
                    MessageBox.Show(new Form { TopMost = true }, "The PTT port \"" + comboCATPTTPort.Text + "\" is not a valid port.  Please select another port.");
                }
                chkCATPTTEnabled.Checked = false;
                return;
            }

            if (chkCATPTTEnabled.Checked && console.CATEnabled &&
                comboCATPort.Text == comboCATPTTPort.Text)
            {
                if (chkCATPTTEnabled.Focused)
                {
                    MessageBox.Show(new Form { TopMost = true }, "CAT port cannot be the same as Bit Bang Port", "Port Selection Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    chkCATPTTEnabled.Checked = false;
                }
                return;
            }

            console.PTTBitBangEnabled = chkCATPTTEnabled.Checked;
            if (chkCATPTTEnabled.Checked) // if it's enabled don't allow changing settings on port 
            {
                enable_sub_fields = false;
            }
            else
            {
                enable_sub_fields = true;
            }
            chkCATPTT_RTS.Enabled = enable_sub_fields;
            chkCATPTT_DTR.Enabled = enable_sub_fields;
            comboCATPTTPort.Enabled = enable_sub_fields;
        }

        private void comboCATparity_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            string selection = comboCATparity.SelectedText;
            if (selection == null) return;

            console.CATParity = SDRSerialPort.StringToParity(selection);
        }


        //================================================================
        private void comboCATPort_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (comboCATPort.Text == "None")
            {
                if (chkCATEnable.Checked)
                {
                    if (comboCATPort.Focused)
                        chkCATEnable.Checked = false;
                }

                chkCATEnable.Enabled = false;
            }
            else chkCATEnable.Enabled = true;

            if (comboCATPort.Text.StartsWith("COM"))
                console.CATPort = Int32.Parse(comboCATPort.Text.Substring(3));

        } // comboCATPort_SelectedIndexChanged


        //====================================================
        // ke9ns add: .180 CATPort2-5
        private void comboCATPort2_SelectedIndexChanged(object sender, EventArgs e)
        {
          
            if (comboCATPort2.Text == "None")
            {
                if (chkCATEnable2.Checked)
                {
                    if (comboCATPort2.Focused)
                        chkCATEnable2.Checked = false;
                }

                chkCATEnable2.Enabled = false;
            }
            else chkCATEnable2.Enabled = true;

            if (comboCATPort2.Text.StartsWith("COM"))
            {
                  console.CATPort2 = Int32.Parse(comboCATPort2.Text.Substring(3));
            }

        } // comboCATPort2_SelectedIndexChanged


        private void comboCATPort3_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            if (comboCATPort3.Text == "None")
            {
                if (chkCATEnable3.Checked)
                {
                    if (comboCATPort3.Focused)
                        chkCATEnable3.Checked = false;
                }

                chkCATEnable3.Enabled = false;
            }
            else chkCATEnable3.Enabled = true;

            if (comboCATPort3.Text.StartsWith("COM"))
            {
                  console.CATPort3 = Int32.Parse(comboCATPort3.Text.Substring(3));
            }

        }

        private void comboCATPort4_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (comboCATPort4.Text == "None")
            {
                if (chkCATEnable4.Checked)
                {
                    if (comboCATPort4.Focused)
                        chkCATEnable4.Checked = false;
                }

                chkCATEnable4.Enabled = false;
            }
            else chkCATEnable4.Enabled = true;

            if (comboCATPort4.Text.StartsWith("COM"))
            {
                console.CATPort4 = Int32.Parse(comboCATPort4.Text.Substring(3));
            }

        }

        private void comboCATPort5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboCATPort5.Text == "None")
            {
                if (chkCATEnable5.Checked)
                {
                    if (comboCATPort5.Focused)
                        chkCATEnable5.Checked = false;
                }

                chkCATEnable5.Enabled = false;
            }
            else chkCATEnable5.Enabled = true;

            if (comboCATPort5.Text.StartsWith("COM"))
            {
                console.CATPort5 = Int32.Parse(comboCATPort5.Text.Substring(3));
            }

        } // comboCATPort5_SelectedIndexChanged


        private void comboCATPort6_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboCATPort6.Text == "None")
            {
                if (chkCATEnable6.Checked)
                {
                    if (comboCATPort6.Focused)
                        chkCATEnable6.Checked = false;
                }

                chkCATEnable6.Enabled = false;
            }
            else chkCATEnable6.Enabled = true;

            if (comboCATPort6.Text.StartsWith("COM"))
            {
                console.CATPort6 = Int32.Parse(comboCATPort6.Text.Substring(3));
            }
        } // comboCATPort6_SelectedIndexChanged

        //==============================================================

        private void comboCATPTTPort_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (comboCATPTTPort.Text == "None")
            {
                if (chkCATPTTEnabled.Checked)
                {
                    if (comboCATPTTPort.Focused)
                        chkCATPTTEnabled.Checked = false;
                }

                //chkCATEnable.Enabled = false;
                doEnablementOnBitBangEnable();
            }
            else
            {
                if (chkCATPTT_RTS.Checked || chkCATPTT_DTR.Checked)
                    //chkCATEnable.Enabled = true;
                    doEnablementOnBitBangEnable();
            }

            if (comboCATPTTPort.Text.StartsWith("COM"))
                console.CATPTTBitBangPort = Int32.Parse(comboCATPTTPort.Text.Substring(3));
            if (!comboCATPTTPort.Focused)
                chkCATPTTEnabled_CheckedChanged(sender, e);
        }

        private void comboCATbaud_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (comboCATbaud.SelectedIndex >= 0)
                console.CATBaudRate = Int32.Parse(comboCATbaud.Text);
        }

        private void comboCATdatabits_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (comboCATdatabits.SelectedIndex >= 0)
                console.CATDataBits = int.Parse(comboCATdatabits.Text);
        }

        private void comboCATstopbits_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (comboCATstopbits.SelectedIndex >= 0)
                console.CATStopBits = SDRSerialPort.StringToStopBits(comboCATstopbits.Text);
        }

        private void btnCATTest_Click(object sender, System.EventArgs e)
        {
            CATTester cat = new CATTester(console);
            //this.Close();
            cat.Show();
            cat.Focus();
            cat.WindowState = FormWindowState.Normal; // ke9ns add
        }

        //Modified 10/12/08 BT to change "SDR-1000" to "PowerSDR"
        private void comboCATRigType_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            switch (comboCATRigType.Text)
            {
                case "PowerSDR":
                    console.CATRigType = 900;
                    break;
                case "TS-2000":
                    console.CATRigType = 19;
                    break;
                case "TS-50S":
                    console.CATRigType = 13;
                    break;
                case "TS-440":
                    console.CATRigType = 20;
                    break;
                default:
                    console.CATRigType = 900;
                    break;
            }
        }

        #endregion
        
        #region Test Tab Event Handlers

        private void chkTestIMD_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chekTestIMD.Checked)
            {
                if (!console.PowerOn)
                {
                    MessageBox.Show(new Form { TopMost = true }, "Power must be on to run this test.",
                        "Power is off",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Hand);
                    chekTestIMD.Checked = false;
                    return;
                }
                console.PreviousPWR = console.PWR;
                console.PWR = (int)udTestIMDPower.Value;
                console.MOX = true;

                if (!console.MOX)
                {
                    chekTestIMD.Checked = false;
                    return;
                }

                Audio.MOX = true;
                chekTestIMD.BackColor = console.ButtonSelectedColor;
                Audio.SineFreq1 = (double)udTestIMDFreq1.Value;
                Audio.SineFreq2 = (double)udTestIMDFreq2.Value;
                Audio.two_tone = true;
                Audio.TXInputSignal = Audio.SignalSource.SINE_TWO_TONE;
                Audio.SourceScale = 1.0;
            }
            else
            {
                udTestIMDPower.Value = console.PWR;
                console.PWR = console.PreviousPWR;
                Audio.TXInputSignal = Audio.SignalSource.RADIO;
                Audio.MOX = false;
                console.MOX = false;
                Audio.SineFreq1 = (double)udDSPCWPitch.Value;
                Audio.two_tone = false;
                chekTestIMD.BackColor = SystemColors.Control;
            }
        }

        private void chkTestX2_CheckedChanged(object sender, System.EventArgs e)
        {
            byte val = 0;
            if (chkTestX2Pin1.Checked) val |= 1 << 0;
            if (chkTestX2Pin2.Checked) val |= 1 << 1;
            if (chkTestX2Pin3.Checked) val |= 1 << 2;
            if (chkTestX2Pin4.Checked) val |= 1 << 3;
            if (chkTestX2Pin5.Checked) val |= 1 << 4;
            if (chkTestX2Pin6.Checked) val |= 1 << 5;

            console.Hdw.X2 = val;
        }

        private void btnTestAudioBalStart_Click(object sender, System.EventArgs e)
        {
            if (!console.PowerOn)
            {
                MessageBox.Show(new Form { TopMost = true }, "Power must be on to run this test.",
                    "Power is off",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Hand);
                return;
            }

            DialogResult dr = DialogResult.No;
            Audio.two_tone = false;
            Audio.SineFreq1 = 600.0;

            do
            {
                Audio.RX1OutputSignal = Audio.SignalSource.SINE_LEFT_ONLY;
                dr = MessageBox.Show(new Form { TopMost = true }, "Do you hear a tone in the left channel?",
                    "Tone in left channel?",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);

                Audio.RX1OutputSignal = Audio.SignalSource.RADIO;

                if (dr == DialogResult.No)
                {
                    DialogResult dr2 = MessageBox.Show(new Form { TopMost = true }, "Please double check cable and speaker connections.\n" +
                        "Click OK to try again (cancel to abort).",
                        "Check connections",
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Asterisk);
                    if (dr2 == DialogResult.Cancel)
                    {
                        MessageBox.Show(new Form { TopMost = true }, "Test Failed",
                            "Failed",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Stop);
                        btnTestAudioBalStart.BackColor = Color.Red;
                        return;
                    }
                }
                else if (dr == DialogResult.Cancel)
                {
                    MessageBox.Show(new Form { TopMost = true }, "Test Failed",
                        "Failed",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Stop);
                    btnTestAudioBalStart.BackColor = Color.Red;
                    return;
                }
            } while (dr != DialogResult.Yes);

            do
            {
                Audio.RX1OutputSignal = Audio.SignalSource.SINE_RIGHT_ONLY;
                dr = MessageBox.Show(new Form { TopMost = true }, "Do you hear a tone in the right channel?",
                    "Tone in right channel?",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);

                Audio.RX1OutputSignal = Audio.SignalSource.RADIO;

                if (dr == DialogResult.No)
                {
                    DialogResult dr2 = MessageBox.Show(new Form { TopMost = true }, "Please double check cable and speaker connections.\n" +
                        "Click OK to try again (cancel to abort).",
                        "Check connections",
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Asterisk);
                    if (dr2 == DialogResult.Cancel)
                    {
                        MessageBox.Show(new Form { TopMost = true }, "Test Failed",
                            "Failed",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Stop);
                        btnTestAudioBalStart.BackColor = Color.Red;
                        return;
                    }
                }
                else if (dr == DialogResult.Cancel)
                {
                    MessageBox.Show(new Form { TopMost = true }, "Test Failed",
                        "Failed",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Stop);
                    btnTestAudioBalStart.BackColor = Color.Red;
                    return;
                }
            } while (dr != DialogResult.Yes);

            MessageBox.Show(new Form { TopMost = true }, "Test was successful.",
                "Success",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            btnTestAudioBalStart.BackColor = Color.Green;
        }

        private void timer_sweep_Tick(object sender, System.EventArgs e)
        {
            if (tkbarTestGenFreq.Value >= udTestGenHigh.Value)
            {
                timer_sweep.Enabled = false;
                btnTestGenSweep.BackColor = SystemColors.Control;
            }
            else
            {
                tkbarTestGenFreq.Value += (int)(udTestGenHzSec.Value / 10);
                tkbarTestGenFreq_Scroll(this, EventArgs.Empty);
            }
        }

        private void buttonTestGenSweep_Click(object sender, System.EventArgs e)
        {
            if (timer_sweep.Enabled)
            {
                timer_sweep.Enabled = false;
                btnTestGenSweep.BackColor = SystemColors.Control;
            }
            else
            {
                btnTestGenSweep.BackColor = console.ButtonSelectedColor;
                tkbarTestGenFreq.Value = (int)udTestGenLow.Value;
                timer_sweep.Enabled = true;
            }
        }

        private void tkbarTestGenFreq_Scroll(object sender, System.EventArgs e)
        {
            Audio.SineFreq1 = tkbarTestGenFreq.Value;
        }



        //======================================================================================= receiver device select: RADIO means the receiver will output to the speaker from the Radio as a source
        private void cmboSigGenRXMode_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (cmboSigGenRXMode.SelectedIndex < 0) return;

            Audio.SignalSource source = Audio.SignalSource.RADIO;

            switch (cmboSigGenRXMode.Text)
            {
                case "Radio":
                    source = Audio.SignalSource.RADIO;
                    break;
                case "Tone":
                    source = Audio.SignalSource.SINE;
                    break;
                case "Noise":
                    source = Audio.SignalSource.NOISE;
                    break;
                case "Triangle":
                    source = Audio.SignalSource.TRIANGLE;
                    break;
                case "Sawtooth":
                    source = Audio.SignalSource.SAWTOOTH;
                    break;
                case "Pulse":
                    source = Audio.SignalSource.PULSE;
                    break;
                case "Silence":
                    source = Audio.SignalSource.SILENCE;
                    break;
            }

            if (chkSigGenRX2.Checked)
            {
                if (rdSigGenRXInput.Checked) Audio.RX2InputSignal = source;
                else Audio.RX2OutputSignal = source;
            }
            else
            {
                if (rdSigGenRXInput.Checked) Audio.RX1InputSignal = source;
                else Audio.RX1OutputSignal = source;
            }

            UpdateSigGenScaleVisible();
            UpdateSigGenPulseVisible();
        }

        //======================================================================================= receiver input radio button
        private void rdSigGenRXInput_CheckedChanged(object sender, System.EventArgs e)
        {
            if (rdSigGenRXInput.Checked)
            {
                Audio.RX1OutputSignal = Audio.SignalSource.RADIO;
                Audio.RX2OutputSignal = Audio.SignalSource.RADIO;
                cmboSigGenRXMode_SelectedIndexChanged(this, EventArgs.Empty);
            }
        }

        //======================================================================================= receiver output radio button
        private void rdSigGenRXOutput_CheckedChanged(object sender, System.EventArgs e)
        {
            if (rdSigGenRXOutput.Checked)
            {
                Audio.RX1InputSignal = Audio.SignalSource.RADIO;
                Audio.RX2InputSignal = Audio.SignalSource.RADIO;
                cmboSigGenRXMode_SelectedIndexChanged(this, EventArgs.Empty);
            }
        }

        private void chkSigGenRX2_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkSigGenRX2.Checked) Audio.RX1InputSignal = Audio.RX1OutputSignal = Audio.SignalSource.RADIO;
            else Audio.RX2InputSignal = Audio.RX2OutputSignal = Audio.SignalSource.RADIO;
            cmboSigGenRXMode_SelectedIndexChanged(this, EventArgs.Empty);
        }

        private void UpdateSigGenScaleVisible()
        {
            bool b = false;
            switch (cmboSigGenRXMode.Text)
            {
                case "Tone":
                case "Pulse":
                    b = true;
                    break;
            }

            switch (cmboSigGenTXMode.Text)
            {
                case "Tone":
                case "Pulse":
                    b = true;
                    break;
            }

            lblTestGenScale.Visible = b;
            udTestGenScale.Visible = b;
        }

        private void UpdateSigGenPulseVisible()
        {
            bool b = false;
            switch (cmboSigGenRXMode.Text)
            {
                case "Pulse": b = true; break;
            }

            switch (cmboSigGenTXMode.Text)
            {
                case "Pulse": b = true; break;
            }

            lblPulseDuty.Visible = b;
            udPulseDuty.Visible = b;
            lblPulsePeriod.Visible = b;
            udPulsePeriod.Visible = b;
        }



        //======================================================================================= Transmitter device select (Radio means the mic on the radio is the Transmitte output signal)
        private void cmboSigGenTXMode_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (cmboSigGenTXMode.SelectedIndex < 0) return;

            Audio.SignalSource source = Audio.SignalSource.RADIO;

            switch (cmboSigGenTXMode.Text)
            {
                case "Radio":
                    source = Audio.SignalSource.RADIO;
                    break;
                case "Tone":
                    source = Audio.SignalSource.SINE;
                    break;
                case "Noise":
                    source = Audio.SignalSource.NOISE;
                    break;
                case "Triangle":
                    source = Audio.SignalSource.TRIANGLE;
                    break;
                case "Sawtooth":
                    source = Audio.SignalSource.SAWTOOTH;
                    break;
                case "Pulse":
                    source = Audio.SignalSource.PULSE;
                    break;
                case "Silence":
                    source = Audio.SignalSource.SILENCE;
                    break;
            }

            if (rdSigGenTXInput.Checked)
                Audio.TXInputSignal = source;
            else Audio.TXOutputSignal = source;

            UpdateSigGenScaleVisible();
            UpdateSigGenPulseVisible();
        }

        private void rdSigGenTXInput_CheckedChanged(object sender, System.EventArgs e)
        {
            if (rdSigGenTXInput.Checked)
            {
                Audio.TXOutputSignal = Audio.SignalSource.RADIO;
                cmboSigGenTXMode_SelectedIndexChanged(this, EventArgs.Empty);
            }
        }

        private void rdSigGenTXOutput_CheckedChanged(object sender, System.EventArgs e)
        {
            if (rdSigGenTXOutput.Checked)
            {
                Audio.TXInputSignal = Audio.SignalSource.RADIO;
                cmboSigGenTXMode_SelectedIndexChanged(this, EventArgs.Empty);
            }
        }

        private void updnTestGenScale_ValueChanged(object sender, System.EventArgs e)
        {
            Audio.SourceScale = (double)udTestGenScale.Value;
        }

        private void btnImpulse_Click(object sender, System.EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(ImpulseFunction));
            t.Name = "Impulse";
            t.Priority = ThreadPriority.Highest;
            t.IsBackground = true;
            t.Start();
        }

        private void ImpulseFunction()
        {
            console.Hdw.ImpulseEnable = true;
            Thread.Sleep(500);
            for (int i = 0; i < (int)udImpulseNum.Value; i++)
            {
                console.Hdw.Impulse();
                Thread.Sleep(45);
            }
            Thread.Sleep(500);
            console.Hdw.ImpulseEnable = false;
        }

        #endregion

        #region Other Event Handlers
        // ======================================================
        // Display Tab Event Handlers
        // ======================================================

        private void btnWizard_Click(object sender, System.EventArgs e)
        {
            SetupWizard w = new SetupWizard(console, comboAudioSoundCard.SelectedIndex);
            w.Show();
            w.Focus();
        }

        private void btnOK_Click(object sender, System.EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(SaveOptions));
            t.Name = "Save Options Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.Lowest;
            t.Start();
            this.Hide();
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(GetOptions));
            t.Name = "Save Options Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.Lowest;
            t.Start();
            this.Hide();
        }


        //ke9ns mod

        public void btnApply_Click(object sender, System.EventArgs e)
        {
            textBoxSAVE.Text = " ";

            Thread t = new Thread(new ThreadStart(ApplyOptions));
            t.Name = "Save Options Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.AboveNormal; // ke9ns was lowest
            t.Start();
        }

        public void ApplyOptions()
        {
            Thread.Sleep(100);

            if (saving) return;

            console.SWR_Logger_Write(); // save SWR file now

           textBoxSAVE.Text = "Saving Changes";

            SaveOptions(); // save controls

            textBoxSAVE.Text = "Updating Database";

            DB.Update();

            textBoxSAVE.Text = "Save Done.";

        }

        private void udGeneralLPTDelay_ValueChanged(object sender, System.EventArgs e)
        {
            console.LatchDelay = (int)udGeneralLPTDelay.Value;
        }

        private void Setup_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        private bool db_import_success = true;

        private void btnImportDB_Click(object sender, System.EventArgs e)
        {
            console.chkPower.Checked = false; // ke9ns STOP

            openFileDialog1.InitialDirectory = String.Empty;

            string path = console.AppDataPath;

            path = path.Substring(0, path.LastIndexOf("\\"));

            openFileDialog1.InitialDirectory = path;

            // openFileDialog1.ShowDialog();

            if (openFileDialog1.ShowDialog() == DialogResult.Cancel) db_import_success = false;

            if (db_import_success == true)
            {
                // restart pSDR to ensure current EEPROM data is transferred to newly imported DB
                MessageBox.Show(new Form { TopMost = true }, "PowerSDR will be closed so that the newly imported database settings can be " +
                    "properly initialized when you restart PowerSDR.",
                    "PowerSDR Shutting Down", MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (SpotForm != null) // ke9ns add .198
                {
                    Debug.WriteLine("SPOT TURNED OFF " + SpotForm.checkBoxMUF.Checked + " , " + SpotForm.VOARUN + " , " + SpotForm.VOARUN + " , " + SpotControl.SP5_Active + " , " + SpotForm.mapon);

                    SpotForm.checkBoxMUF.CheckedChanged -= SpotForm.checkBoxMUF_CheckedChanged;
                    SpotForm.checkBoxMUF.Checked = false;
                    SpotControl.Map_Last = SpotControl.Map_Last | 2;    // force update of world map

                    SpotControl.SP5_Active = 0;                     // turn off tracking

                    Debug.WriteLine("SPOT TURNED OFF- " + SpotForm.checkBoxMUF.Checked + " , " + SpotForm.VOARUN + " , " + SpotForm.VOARUN + " , " + SpotControl.SP5_Active + " , " + SpotForm.mapon);

                } //  if (SpotForm != null) 


                console.Close();
            }

        } // btnImportDB_Click

        private void openFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            CompleteImport();
        }

        private void CompleteImport()
        {
            if (!File.Exists(openFileDialog1.FileName))
            {
                db_import_success = false;
                return;
            }

            // parse the DB import file to see if it was created with the same radio model and version of pSDR that is running
            string db_ver_num = "unknown version";
            string db_radio_type = "unknown SDR";

            XmlTextReader dbcheck = new XmlTextReader(openFileDialog1.FileName);
            dbcheck.WhitespaceHandling = WhitespaceHandling.None;
            try
            {
                while (!dbcheck.EOF)
                {
                    dbcheck.Read();

                    if (dbcheck.Value == "VersionNumber")
                    {
                        dbcheck.Read();
                        dbcheck.Read();
                        dbcheck.Read();
                        db_ver_num = dbcheck.Value;
                    }

                    if (dbcheck.Value == "RadioType")
                    {
                        dbcheck.Read();
                        dbcheck.Read();
                        dbcheck.Read();
                        db_radio_type = dbcheck.Value;
                    }
                }
                dbcheck.Close();
            }
            catch (System.Xml.XmlException ex)
            {
                MessageBox.Show(new Form { TopMost = true }, "ERROR: Could not import database.  Exception was:\n\n " + ex.Message +
                    "\n\nDatabase will not be imported.", "Error Importing Database",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                db_import_success = false;
                return;
            }

            string current_radio_type = Convert.ToString(console.CurrentModel);

            if (current_radio_type != db_radio_type)
            {
                MessageBox.Show(new Form { TopMost = true }, "Database Import Not Allowed.\r\n\r\nThe radio model associated with " +
                    "the database selected for import (" + db_radio_type + ") either can not be verified " +
                    "or does not match the radio model currently running (" + current_radio_type + ").\r\n\r\n" +
                    "The PowerSDR database import feature is intended to import databases created (Exported) with " +
                    "the same version of PowerSDR and radio model that is currently running.\r\n\r\n " +
                    "It is *very* important that the database being imported was created with the same radio " +
                    "model currently in use.  Failure to do so can result is unpredictable behavior and " +
                    "possibly damage the radio if incorrect parameters are imported.\r\n\r\n" +
                    "It is required to use the PowerSDR-DataTransfer™ utility included with this version of PowerSDR " +
                    "for migrating users settings from different radio types rather than the Setup Export/Import feture.\r\n\r\n" +
                    "Please ensure that the database selected for import was created using the " +
                    "same radio model before importing a database.",
                    "Database Import Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                db_import_success = false;
                return;
            }

            string current_ver_num = TitleBar.GetVerNum();

            if ((current_ver_num != db_ver_num))
            {
                Debug.WriteLine("current , old " + current_ver_num + " , " + db_ver_num);

                /*    if (chkImportDBRestrict.Checked)
                    {
                        MessageBox.Show(new Form { TopMost = true }, "Database Import Restricted.\r\n\r\nThe database selected for import was " +
                            "created from a version of PowerSDR that is not the same as PowerSDR v" + current_ver_num + ".\r\n\r\n" +
                            "The PowerSDR database Import feature is intended to import databases created (Exported) with the " +
                            "same version of PowerSDR and radio model that is currently running.\r\n\r\n" +
                            "The import of databases that were not created PowerSDR v" + current_ver_num + " may result " +
                            "in errors due to database schema incompatibilities or differences in default values.\r\n\r\n" +
                            "It is required to use the PowerSDR-DataTransfer™ utility included with this version of PowerSDR " +
                            "for migrating users settings from different versions of PowerSDR rather than the Setup Export/Import feature.\r\n\r\n" +
                            "Please ensure that the database selected for import was created using PowerSDR v" + current_ver_num +
                            " before importing a database.", 
                            "Database Import Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        db_import_success = false;
                        return;
                    }
                    else
                    {
                    */
                DialogResult result = MessageBox.Show(new Form { TopMost = true }, "The database selected for import was " +
                    "created (Exported) from a version of PowerSDR that is not the same as PowerSDR v" + current_ver_num + " and " +
                    "the DB import safeguard has been disabled.\r\n\r\n" +
                    "The PowerSDR database import feature is intended to import databases created with the " +
                    "same version of PowerSDR and radio model that is currently running.\r\n\r\n" +
                    "The import of databases that were not created PowerSDR v" + current_ver_num + " may result " +
                    "in errors due to database schema incompatibilities or differences in default values.\r\n\r\n" +
                    "It is recommended to use the PowerSDR-DataTransfer™ utility included with this version of PowerSDR " +
                    "for migrating users settings from different versions of PowerSDR rather than the Setup Export/Import feature.\r\n\r\n" +
                    "Do you want to continue the database import at your own risk?",
                    "Import Database?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    db_import_success = false;
                    return;
                }
                //  }
            }

            DB.ImportDatabase(openFileDialog1.FileName);

            GetTxProfiles();
            console.UpdateTXProfile(TXProfile);

            GetOptions();                   // load all database values
            console.GetState();
            if (console.eqForm != null) Common.RestoreForm(console.eqForm, "EQForm", false);
            if (console.xvtrForm != null) Common.RestoreForm(console.xvtrForm, "XVTR", false);
            if (console.ProdTestForm != null) Common.RestoreForm(console.ProdTestForm, "ProdTest", false);

            SaveOptions();                  // save all database values
            console.SaveState();
            if (console.eqForm != null) Common.SaveForm(console.eqForm, "EQForm");
            if (console.xvtrForm != null) Common.SaveForm(console.xvtrForm, "XVTR");
            if (console.ProdTestForm != null) Common.SaveForm(console.ProdTestForm, "ProdTest");

            udTransmitTunePower_ValueChanged(this, EventArgs.Empty);

            MessageBox.Show(new Form { TopMost = true }, "Database Import Complete");

        } // completeimport

        #endregion

        private bool shift_key = false;
        private bool ctrl_key = false;
        private bool alt_key = false;
        private bool windows_key = false;
        private bool menu_key = false;

        private void txtKB_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            Debug.WriteLine("KeyCode: " + e.KeyCode + " KeyData: " + e.KeyData + " KeyValue: " + e.KeyValue);
            shift_key = e.Shift;
            ctrl_key = e.Control;
            alt_key = e.Alt;

            if (e.KeyCode == Keys.LWin ||
                e.KeyCode == Keys.RWin)
                windows_key = true;

            if (e.KeyCode == Keys.Apps)
                menu_key = true;

            TextBoxTS txtbox = (TextBoxTS)sender;

            string s = "";

            if (ctrl_key) s += "Ctrl+";
            if (alt_key) s += "Alt+";
            if (shift_key) s += "Shift+";
            if (windows_key)
                s += "Win+";
            if (menu_key)
                s += "Menu+";

            if (e.KeyCode != Keys.ShiftKey &&
                e.KeyCode != Keys.ControlKey &&
                e.KeyCode != Keys.Menu &&
                e.KeyCode != Keys.RMenu &&
                e.KeyCode != Keys.LWin &&
                e.KeyCode != Keys.RWin &&
                e.KeyCode != Keys.Apps)
                s += KeyToString(e.KeyCode);

            txtbox.Text = s;
            e.Handled = true;
        }

        private void txtKB_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void txtKB_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            //Debug.WriteLine("KeyUp: "+e.KeyCode.ToString());
            shift_key = e.Shift;
            ctrl_key = e.Control;
            alt_key = e.Alt;

            if (e.KeyCode == Keys.LWin ||
                e.KeyCode == Keys.RWin)
                windows_key = false;

            if (e.KeyCode == Keys.Apps)
                menu_key = false;


            TextBoxTS txtbox = (TextBoxTS)sender;

            if (txtbox.Text.EndsWith("+"))
            {
                if (shift_key || ctrl_key || alt_key ||
                    windows_key || menu_key)
                {
                    string s = "";

                    if (ctrl_key) s += "Ctrl+";
                    if (alt_key) s += "Alt+";
                    if (shift_key) s += "Shift+";
                    if (windows_key)
                        s += "Win+";
                    if (menu_key)
                        s += "Menu+";

                    txtbox.Text = s;
                }
                else
                    txtbox.Text = "Not Assigned";
            }
        }

        private void clrbtnTXFilter_Changed(object sender, System.EventArgs e)
        {
            Display.DisplayFilterTXColor = clrbtnTXFilter.Color;
        }

        #region Lost Focus Event Handlers

        private void udGeneralCalFreq1_LostFocus(object sender, EventArgs e)
        {
            udGeneralCalFreq1.Value = udGeneralCalFreq1.Value;
        }

        private void udSoftRockCenterFreq_LostFocus(object sender, EventArgs e)
        {
            udSoftRockCenterFreq.Value = udSoftRockCenterFreq.Value;
        }

        private void udDDSCorrection_LostFocus(object sender, EventArgs e)
        {
            udDDSCorrection.Value = udDDSCorrection.Value;
        }

        private void udDDSIFFreq_LostFocus(object sender, EventArgs e)
        {
            udDDSIFFreq.Value = udDDSIFFreq.Value;
        }

        private void udDDSPLLMult_LostFocus(object sender, EventArgs e)
        {
            udDDSPLLMult.Value = udDDSPLLMult.Value;
        }

        private void udGeneralLPTDelay_LostFocus(object sender, EventArgs e)
        {
            udGeneralLPTDelay.Value = udGeneralLPTDelay.Value;
        }

        private void udOptClickTuneOffsetDIGL_LostFocus(object sender, EventArgs e)
        {
            udOptClickTuneOffsetDIGL.Value = udOptClickTuneOffsetDIGL.Value;
        }

        private void udOptClickTuneOffsetDIGU_LostFocus(object sender, EventArgs e)
        {
            udOptClickTuneOffsetDIGU.Value = udOptClickTuneOffsetDIGU.Value;
        }

        private void udGeneralX2Delay_LostFocus(object sender, EventArgs e)
        {
            udGeneralX2Delay.Value = udGeneralX2Delay.Value;
        }

        private void udGeneralCalFreq3_LostFocus(object sender, EventArgs e)
        {
            udGeneralCalFreq3.Value = udGeneralCalFreq3.Value;
        }

        private void udGeneralCalLevel_LostFocus(object sender, EventArgs e)
        {
            udGeneralCalLevel.Value = udGeneralCalLevel.Value;
        }

        private void udGeneralCalFreq2_LostFocus(object sender, EventArgs e)
        {
            udGeneralCalFreq2.Value = udGeneralCalFreq2.Value;
        }

        private void udFilterDefaultLowCut_LostFocus(object sender, EventArgs e)
        {
            udFilterDefaultLowCut.Value = udFilterDefaultLowCut.Value;
        }

        private void udOptMaxFilterShift_LostFocus(object sender, EventArgs e)
        {
            udOptMaxFilterShift.Value = udOptMaxFilterShift.Value;
        }

        private void udOptMaxFilterWidth_LostFocus(object sender, EventArgs e)
        {
            udOptMaxFilterWidth.Value = udOptMaxFilterWidth.Value;
        }

        private void udAudioMicGain1_LostFocus(object sender, EventArgs e)
        {
            udAudioMicGain1.Value = udAudioMicGain1.Value;
        }

        private void udAudioLineIn1_LostFocus(object sender, EventArgs e)
        {
            udAudioLineIn1.Value = udAudioLineIn1.Value;
        }

        private void udAudioVoltage1_LostFocus(object sender, EventArgs e)
        {
            udAudioVoltage1.Value = udAudioVoltage1.Value;
        }

        private void udAudioLatency1_LostFocus(object sender, EventArgs e)
        {
            udAudioLatency1.Value = udAudioLatency1.Value;
        }

        private void udAudioVACGainTX_LostFocus(object sender, EventArgs e)
        {
            udAudioVACGainTX.Value = udAudioVACGainTX.Value;
        }

        private void udAudioVACGainRX_LostFocus(object sender, EventArgs e)
        {
            udAudioVACGainRX.Value = udAudioVACGainRX.Value;
        }

        private void udAudioLatency2_LostFocus(object sender, EventArgs e)
        {
            udAudioLatency2.Value = udAudioLatency2.Value;
        }

        private void udVAC2Latency_LostFocus(object sender, EventArgs e)
        {
            udVAC2Latency.Value = udVAC2Latency.Value;
        }

        private void udDisplayScopeTime_LostFocus(object sender, EventArgs e)
        {
            udDisplayScopeTime.Value = udDisplayScopeTime.Value;
        }

        private void udDisplayMeterAvg_LostFocus(object sender, EventArgs e)
        {
            udDisplayMeterAvg.Value = udDisplayMeterAvg.Value;
        }

        private void udDisplayMultiTextHoldTime_LostFocus(object sender, EventArgs e)
        {
            udDisplayMultiTextHoldTime.Value = udDisplayMultiTextHoldTime.Value;
        }

        private void udDisplayMultiPeakHoldTime_LostFocus(object sender, EventArgs e)
        {
            udDisplayMultiPeakHoldTime.Value = udDisplayMultiPeakHoldTime.Value;
        }

        private void udDisplayWaterfallLowLevel_LostFocus(object sender, EventArgs e)
        {
            udDisplayWaterfallLowLevel.Value = udDisplayWaterfallLowLevel.Value;
        }

        private void udDisplayWaterfallHighLevel_LostFocus(object sender, EventArgs e)
        {
            udDisplayWaterfallHighLevel.Value = udDisplayWaterfallHighLevel.Value;
        }

        private void udDisplayCPUMeter_LostFocus(object sender, EventArgs e)
        {
            udDisplayCPUMeter.Value = udDisplayCPUMeter.Value;
        }

        private void udDisplayPeakText_LostFocus(object sender, EventArgs e)
        {
            udDisplayPeakText.Value = udDisplayPeakText.Value;
        }

        private void udDisplayMeterDelay_LostFocus(object sender, EventArgs e)
        {
            udDisplayMeterDelay.Value = udDisplayMeterDelay.Value;
        }

        private void udDisplayFPS_LostFocus(object sender, EventArgs e)
        {
            udDisplayFPS.Value = udDisplayFPS.Value;
        }

        private void udDisplayAVGTime_LostFocus(object sender, EventArgs e)
        {
            udDisplayAVGTime.Value = udDisplayAVGTime.Value;
        }

        private void udDisplayPhasePts_LostFocus(object sender, EventArgs e)
        {
            udDisplayPhasePts.Value = udDisplayPhasePts.Value;
        }

        private void udDisplayGridStep_LostFocus(object sender, EventArgs e)
        {
            udDisplayGridStep.Value = udDisplayGridStep.Value;
        }

        private void udDisplayGridMin_LostFocus(object sender, EventArgs e)
        {
            udDisplayGridMin.Value = udDisplayGridMin.Value;
        }

        private void udDSPNB_LostFocus(object sender, EventArgs e)
        {
            udDSPNB.Value = udDSPNB.Value;
        }

        private void udLMSNRgain_LostFocus(object sender, EventArgs e)
        {
            udLMSNRgain.Value = udLMSNRgain.Value;
        }

        private void udLMSNRdelay_LostFocus(object sender, EventArgs e)
        {
            udLMSNRdelay.Value = udLMSNRdelay.Value;
        }

        private void udLMSNRtaps_LostFocus(object sender, EventArgs e)
        {
            udLMSNRtaps.Value = udLMSNRtaps.Value;
        }

        private void udLMSANFgain_LostFocus(object sender, EventArgs e)
        {
            udLMSANFgain.Value = udLMSANFgain.Value;
        }

        private void udLMSANFdelay_LostFocus(object sender, EventArgs e)
        {
            udLMSANFdelay.Value = udLMSANFdelay.Value;
        }

        private void udLMSANFtaps_LostFocus(object sender, EventArgs e)
        {
            udLMSANFtaps.Value = udLMSANFtaps.Value;
        }

        private void udDSPNB2_LostFocus(object sender, EventArgs e)
        {
            udDSPNB2.Value = udDSPNB2.Value;
        }

        private void udDSPImageGainTX_LostFocus(object sender, EventArgs e)
        {
            udDSPImageGainTX.Value = udDSPImageGainTX.Value;
        }

        private void udDSPImagePhaseTX_LostFocus(object sender, EventArgs e)
        {
            udDSPImagePhaseTX.Value = udDSPImagePhaseTX.Value;
        }

        private void udDSPCWPitch_LostFocus(object sender, EventArgs e)
        {
            udDSPCWPitch.Value = udDSPCWPitch.Value;
        }

        private void udCWKeyerWeight_LostFocus(object sender, EventArgs e)
        {
            udCWKeyerWeight.Value = udCWKeyerWeight.Value;
        }

        private void udCWKeyerRamp_LostFocus(object sender, EventArgs e)
        {
            udCWKeyerRamp.Value = udCWKeyerRamp.Value;
        }

        private void udCWBreakInDelay_LostFocus(object sender, EventArgs e)
        {
            udCWBreakInDelay.Value = udCWBreakInDelay.Value;
        }

        private void udDSPLevelerHangTime_LostFocus(object sender, EventArgs e)
        {
            udDSPLevelerHangTime.Value = udDSPLevelerHangTime.Value;
        }

        private void udDSPLevelerThreshold_LostFocus(object sender, EventArgs e)
        {
            udDSPLevelerThreshold.Value = udDSPLevelerThreshold.Value;
        }

        private void udDSPLevelerSlope_LostFocus(object sender, EventArgs e)
        {
            udDSPLevelerSlope.Value = udDSPLevelerSlope.Value;
        }

        private void udDSPLevelerDecay_LostFocus(object sender, EventArgs e)
        {
            udDSPLevelerDecay.Value = udDSPLevelerDecay.Value;
        }

        private void udDSPLevelerAttack_LostFocus(object sender, EventArgs e)
        {
            udDSPLevelerAttack.Value = udDSPLevelerAttack.Value;
        }

        private void udDSPALCHangTime_LostFocus(object sender, EventArgs e)
        {
            udDSPALCHangTime.Value = udDSPALCHangTime.Value;
        }

        private void udDSPALCThreshold_LostFocus(object sender, EventArgs e)
        {
            udDSPALCThreshold.Value = udDSPALCThreshold.Value;
        }

        private void udDSPALCSlope_LostFocus(object sender, EventArgs e)
        {
            udDSPALCSlope.Value = udDSPALCSlope.Value;
        }

        private void udDSPALCDecay_LostFocus(object sender, EventArgs e)
        {
            udDSPALCDecay.Value = udDSPALCDecay.Value;
        }

        private void udDSPALCAttack_LostFocus(object sender, EventArgs e)
        {
            udDSPALCAttack.Value = udDSPALCAttack.Value;
        }

        private void udDSPAGCHangTime_LostFocus(object sender, EventArgs e)
        {
            udDSPAGCHangTime.Value = udDSPAGCHangTime.Value;
        }

        private void udDSPAGCMaxGaindB_LostFocus(object sender, EventArgs e)
        {
            udDSPAGCMaxGaindB.Value = udDSPAGCMaxGaindB.Value;
        }

        private void udDSPAGCSlope_LostFocus(object sender, EventArgs e)
        {
            udDSPAGCSlope.Value = udDSPAGCSlope.Value;
        }

        private void udDSPAGCDecay_LostFocus(object sender, EventArgs e)
        {
            udDSPAGCDecay.Value = udDSPAGCDecay.Value;
        }

        private void udDSPAGCAttack_LostFocus(object sender, EventArgs e)
        {
            udDSPAGCAttack.Value = udDSPAGCAttack.Value;
        }

        private void udDSPAGCFixedGaindB_LostFocus(object sender, EventArgs e)
        {
            udDSPAGCFixedGaindB.Value = udDSPAGCFixedGaindB.Value;
        }

        private void udTXAMCarrierLevel_LostFocus(object sender, EventArgs e)
        {
            udTXAMCarrierLevel.Value = udTXAMCarrierLevel.Value;
        }

        private void udTXAF_LostFocus(object sender, EventArgs e)
        {
            udTXAF.Value = udTXAF.Value;
        }

        private void udTXVOXHangTime_LostFocus(object sender, EventArgs e)
        {
            udTXVOXHangTime.Value = udTXVOXHangTime.Value;
        }

        private void udTXVOXThreshold_LostFocus(object sender, EventArgs e)
        {
            udTXVOXThreshold.Value = udTXVOXThreshold.Value;
        }

        private void udTXNoiseGate_LostFocus(object sender, EventArgs e)
        {
            udTXNoiseGate.Value = udTXNoiseGate.Value;
        }

        private void udTXTunePower_LostFocus(object sender, EventArgs e)
        {
            udTXTunePower.Value = udTXTunePower.Value;
            if (udTXDriveMax.Value < udTXTunePower.Value) udTXTunePower.Value = udTXDriveMax.Value;

        }

        private void udTXFilterLow_LostFocus(object sender, EventArgs e)
        {
            udTXFilterLow.Value = udTXFilterLow.Value;
        }

        private void udTXFilterHigh_LostFocus(object sender, EventArgs e)
        {
            udTXFilterHigh.Value = udTXFilterHigh.Value;
        }

        private void udPAADC17_LostFocus(object sender, EventArgs e)
        {
            udPAADC17.Value = udPAADC17.Value;
        }

        private void udPAADC15_LostFocus(object sender, EventArgs e)
        {
            udPAADC15.Value = udPAADC15.Value;
        }

        private void udPAADC20_LostFocus(object sender, EventArgs e)
        {
            udPAADC20.Value = udPAADC20.Value;
        }

        private void udPAADC12_LostFocus(object sender, EventArgs e)
        {
            udPAADC12.Value = udPAADC12.Value;
        }

        private void udPAADC10_LostFocus(object sender, EventArgs e)
        {
            udPAADC10.Value = udPAADC10.Value;
        }

        private void udPAADC160_LostFocus(object sender, EventArgs e)
        {
            udPAADC160.Value = udPAADC160.Value;
        }

        private void udPAADC80_LostFocus(object sender, EventArgs e)
        {
            udPAADC80.Value = udPAADC80.Value;
        }

        private void udPAADC60_LostFocus(object sender, EventArgs e)
        {
            udPAADC60.Value = udPAADC60.Value;
        }

        private void udPAADC40_LostFocus(object sender, EventArgs e)
        {
            udPAADC40.Value = udPAADC40.Value;
        }

        private void udPAADC30_LostFocus(object sender, EventArgs e)
        {
            udPAADC30.Value = udPAADC30.Value;
        }

        private void udPAGain10_LostFocus(object sender, EventArgs e)
        {
            udPAGain10.Value = udPAGain10.Value;
        }

        private void udPAGain12_LostFocus(object sender, EventArgs e)
        {
            udPAGain12.Value = udPAGain12.Value;
        }

        private void udPAGain15_LostFocus(object sender, EventArgs e)
        {
            udPAGain15.Value = udPAGain15.Value;
        }

        private void udPAGain17_LostFocus(object sender, EventArgs e)
        {
            udPAGain17.Value = udPAGain17.Value;
        }

        private void udPAGain20_LostFocus(object sender, EventArgs e)
        {
            udPAGain20.Value = udPAGain20.Value;
        }

        private void udPAGain30_LostFocus(object sender, EventArgs e)
        {
            udPAGain30.Value = udPAGain30.Value;
        }

        private void udPAGain40_LostFocus(object sender, EventArgs e)
        {
            udPAGain40.Value = udPAGain40.Value;
        }

        private void udPAGain60_LostFocus(object sender, EventArgs e)
        {
            udPAGain60.Value = udPAGain60.Value;
        }

        private void udPAGain80_LostFocus(object sender, EventArgs e)
        {
            udPAGain80.Value = udPAGain80.Value;
        }

        private void udPAGain160_LostFocus(object sender, EventArgs e)
        {
            udPAGain160.Value = udPAGain160.Value;
        }

        private void udPACalPower_LostFocus(object sender, EventArgs e)
        {
            udPACalPower.Value = udPACalPower.Value;
        }

        private void udDisplayLineWidth_LostFocus(object sender, EventArgs e)
        {
            udDisplayLineWidth.Value = udDisplayLineWidth.Value;
        }

        private void udBandSegmentBoxLineWidth_LostFocus(object sender, EventArgs e)
        {
            udBandSegmentBoxLineWidth.Value = udBandSegmentBoxLineWidth.Value;
        }

        private void udTestGenScale_LostFocus(object sender, EventArgs e)
        {
            udTestGenScale.Value = udTestGenScale.Value;
        }

        private void udTestGenHzSec_LostFocus(object sender, EventArgs e)
        {
            udTestGenHzSec.Value = udTestGenHzSec.Value;
        }

        private void udTestGenHigh_LostFocus(object sender, EventArgs e)
        {
            udTestGenHigh.Value = udTestGenHigh.Value;
        }

        private void udTestGenLow_LostFocus(object sender, EventArgs e)
        {
            udTestGenLow.Value = udTestGenLow.Value;
        }

        private void udTestIMDFreq2_LostFocus(object sender, EventArgs e)
        {
            udTestIMDFreq2.Value = udTestIMDFreq2.Value;
        }

        private void udTestIMDPower_LostFocus(object sender, EventArgs e)
        {
            udTestIMDPower.Value = udTestIMDPower.Value;
        }

        private void udTestIMDFreq1_LostFocus(object sender, EventArgs e)
        {
            udTestIMDFreq1.Value = udTestIMDFreq1.Value;
        }

        private void udImpulseNum_LostFocus(object sender, EventArgs e)
        {
            udImpulseNum.Value = udImpulseNum.Value;
        }

        #endregion

        private void chkShowFreqOffset_CheckedChanged(object sender, System.EventArgs e)
        {
            Display.ShowFreqOffset = chkShowFreqOffset.Checked;
        }

        private void clrbtnBandEdge_Changed(object sender, System.EventArgs e)
        {
            Display.BandEdgeColor = clrbtnBandEdge.Color;
        }

        private void clrbtnBandSegmentBox_Changed(object sender, System.EventArgs e)
        {
            Display.BandBoxColor = clrbtnBandSegmentBox.Color;
        }



        //==ke9ns mod=================================================================================
        public void comboMeterType_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            // see MTRset  (setupform.MTRset in console.cs)

            if (comboMeterType.Text == "") return;

            console.lblMultiSMeter.Visible = false;
            console.lblRX2Meter.Visible = false;


            switch (comboMeterType.Text)
            {

                case "AnalogTR7":
                    console.CurrentMeterDisplayMode = MultiMeterDisplayMode.Original;
                    comboMeterType.SelectedItem = MultiMeterDisplayMode.Original;
                    break;

                case "Edge":
                    console.CurrentMeterDisplayMode = MultiMeterDisplayMode.Edge;
                    comboMeterType.SelectedItem = MultiMeterDisplayMode.Edge;
                    break;

                case "Analog":
                    console.CurrentMeterDisplayMode = MultiMeterDisplayMode.Analog;
                    comboMeterType.SelectedItem = MultiMeterDisplayMode.Analog;
                    break;

                case "Bar":
                    console.CurrentMeterDisplayMode = MultiMeterDisplayMode.Bar;
                    comboMeterType.SelectedItem = MultiMeterDisplayMode.Bar;

                    console.lblMultiSMeter.Visible = true;

                    if (console.meterCombo != true) //MeterTXMode.Combo)
                    {
                        console.lblRX2Meter.Visible = true;
                    }
                    break;
            }


        }  // meter selected


        //================================================================================
        // ke9ns EDGE meter colors

        private void clrbtnMeterEdgeLow_Changed(object sender, System.EventArgs e)
        {
            console.EdgeLowColor = clrbtnMeterEdgeLow.Color;
        }

        private void clrbtnMeterEdgeHigh_Changed(object sender, System.EventArgs e)
        {
            console.EdgeHighColor = clrbtnMeterEdgeHigh.Color;
        }

        private void clrbtnMeterEdgeBackground_Changed(object sender, System.EventArgs e)
        {
            console.EdgeMeterBackgroundColor = clrbtnMeterEdgeBackground.Color;
            console.RingVFOColor = clrbtnVFORing.Color; // ke9ns add
        }

        private void clrbtnEdgeIndicator_Changed(object sender, System.EventArgs e)
        {
            console.EdgeAVGColor = clrbtnEdgeIndicator.Color;
        }

        //================================================================================

        private void clrbtnMeterDigText_Changed(object sender, System.EventArgs e)
        {
            console.MeterDigitalTextColor = clrbtnMeterDigText.Color;
        }

        private void clrbtnMeterDigBackground_Changed(object sender, System.EventArgs e)
        {
            console.MeterDigitalBackgroundColor = clrbtnMeterDigBackground.Color;
        }

        private void clrbtnSubRXFilter_Changed(object sender, System.EventArgs e)
        {
            Display.SubRXFilterColor = Color.FromArgb(tbMultiRXFilterAlpha.Value, clrbtnSubRXFilter.Color);
        }

        private void clrbtnSubRXZero_Changed(object sender, System.EventArgs e)
        {
            Display.SubRXZeroLine = clrbtnSubRXZero.Color;
        }

        private void chkCWKeyerMode_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkCWKeyerMode.Checked)
            {
                if (chkModeBStrict.Checked)
                    CWKeyer.CurrentIambicMode = CWKeyer.IambicMode.ModeBStrict;
                else
                    CWKeyer.CurrentIambicMode = CWKeyer.IambicMode.ModeB;
            }
            else CWKeyer.CurrentIambicMode = CWKeyer.IambicMode.ModeA;
        }

        private void chkDisableToolTips_CheckedChanged(object sender, System.EventArgs e)
        {
            toolTip1.Active = !chkDisableToolTips.Checked;
            console.DisableToolTips = chkDisableToolTips.Checked;
        }

        private void udDisplayWaterfallAvgTime_ValueChanged(object sender, System.EventArgs e)
        {
            double buffer_time = double.Parse(comboAudioBuffer1.Text) / (double)console.SampleRate1;
            int buffersToAvg = (int)((float)udDisplayWaterfallAvgTime.Value * 0.001 / buffer_time);
            buffersToAvg = Math.Max(2, buffersToAvg);
            Display.WaterfallAvgBlocks = buffersToAvg;
        }

        private void udDisplayWaterfallUpdatePeriod_ValueChanged(object sender, System.EventArgs e)
        {
            Display.WaterfallUpdatePeriod = (int)udDisplayWaterfallUpdatePeriod.Value;
            console.WaterfallUpdatePeriod = (int)udDisplayWaterfallUpdatePeriod.Value;
        }

        private void chkWeakSignalWaterfallSettings_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkWeakSignalWaterfallSettings.Checked)
            {
                udDisplayWaterfallAvgTime.Value = 750;
                udDisplayWaterfallUpdatePeriod.Value = 100;

                switch (Display.CurrentDisplayMode)
                {
                    case DisplayMode.WATERFALL:
                    case DisplayMode.PANAFALL:
                        console.DisplayAVG = true;
                        console.RX2DisplayAVG = true;
                        break;
                }
            }
            else
            {
                if (udDisplayWaterfallAvgTime.Value != 200)
                    udDisplayWaterfallAvgTime.Value = 200;

                if (udDisplayWaterfallUpdatePeriod.Value != 50)
                    udDisplayWaterfallUpdatePeriod.Value = 50;

                switch (Display.CurrentDisplayMode)
                {
                    case DisplayMode.WATERFALL:
                    case DisplayMode.PANAFALL:
                        if (console.DisplayAVG == true) console.DisplayAVG = false;

                        if (console.RX2DisplayAVG == true) console.RX2DisplayAVG = false;
                        break;
                }
            }
        }

        private void chkSnapClickTune_CheckedChanged(object sender, System.EventArgs e)
        {
            Debug.WriteLine("SNAP CLICK");

            console.SnapToClickTuning = chkSnapClickTune.Checked;
        }

        private void radPACalAllBands_CheckedChanged(object sender, System.EventArgs e)
        {
            foreach (Control c in grpPAGainByBand.Controls)
            {
                if (c.Name.StartsWith("chkPA"))
                {
                    c.Visible = !radPACalAllBands.Checked;
                }
            }
            if (radGenModelFLEX5000.Checked && !radPACalAllBands.Checked)
                chkPA6.Visible = true;
            else chkPA6.Visible = false;
        }

        private void chkZeroBeatRIT_CheckedChanged(object sender, System.EventArgs e)
        {
            console.ZeroBeatRIT = chkZeroBeatRIT.Checked;
        }

        private FWCAnt old_ant = FWCAnt.ANT1;

        private HIDAnt old_hid_ant = HIDAnt.PA;

        private void ckEnableSigGen_CheckedChanged(object sender, System.EventArgs e)
        {
            if (console.fwc_init && (console.CurrentModel == Model.FLEX5000 || console.CurrentModel == Model.FLEX3000))
            {
                if (ckEnableSigGen.Checked)
                {
                    old_ant = console.RX1Ant;
                    console.RX1Ant = FWCAnt.SIG_GEN;
                }
                else console.RX1Ant = old_ant;
                FWC.SetTest(ckEnableSigGen.Checked);
                FWC.SetGen(ckEnableSigGen.Checked);
                FWC.SetSig(ckEnableSigGen.Checked);

                if (!console.FullDuplex)
                    FWC.SetFullDuplex(ckEnableSigGen.Checked);
            }
            else if (console.hid_init && console.CurrentModel == Model.FLEX1500)
            {
                if (ckEnableSigGen.Checked)
                {
                    old_hid_ant = console.RXAnt1500;
                    console.RXAnt1500 = HIDAnt.BITE;
                }
                else console.RXAnt1500 = old_hid_ant;
                Thread.Sleep(10);

                USBHID.SetGen(ckEnableSigGen.Checked); Thread.Sleep(10);
                USBHID.SetQSE(ckEnableSigGen.Checked); Thread.Sleep(10);
                if (ckEnableSigGen.Checked)
                {
                    USBHID.SetPreamp(FLEX1500PreampMode.ZERO);
                    USBHID.SetTest(true); Thread.Sleep(20);
                }
                else
                {
                    USBHID.SetTest(false); Thread.Sleep(20);
                    console.RX1PreampMode = console.RX1PreampMode;
                }
            }
        }

        private void chkPANewCal_CheckedChanged(object sender, System.EventArgs e)
        {
            bool b = chkPANewCal.Checked;

            console.NewPowerCal = b;

            lblPAGainByBand160.Visible = !b;
            lblPAGainByBand80.Visible = !b;
            lblPAGainByBand60.Visible = !b;
            lblPAGainByBand40.Visible = !b;
            lblPAGainByBand30.Visible = !b;
            lblPAGainByBand20.Visible = !b;
            lblPAGainByBand17.Visible = !b;
            lblPAGainByBand15.Visible = !b;
            lblPAGainByBand12.Visible = !b;
            lblPAGainByBand10.Visible = !b;

            udPAGain160.Visible = !b;
            udPAGain80.Visible = !b;
            udPAGain60.Visible = !b;
            udPAGain40.Visible = !b;
            udPAGain30.Visible = !b;
            udPAGain20.Visible = !b;
            udPAGain17.Visible = !b;
            udPAGain15.Visible = !b;
            udPAGain12.Visible = !b;
            udPAGain10.Visible = !b;

            if (!radGenModelFLEX5000.Checked)
            {
                lblPACalTarget.Visible = !b;
                udPACalPower.Visible = !b;
                btnPAGainReset.Visible = !b;
            }
        }

        private void chkAudioExpert_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkAudioExpert.Checked && chkAudioExpert.Focused)
            {
                DialogResult dr = MessageBox.Show(new Form { TopMost = true }, "The Expert mode allows the user to control advanced controls that only " +
                    "experienced PowerSDR users should use.  These controls may allow the user " +
                    "to cause damage to the radio.\r\n\r\n" +
                    "Are you sure you want to enable Expert mode?",
                    "Warning: Enable Expert Mode?",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);
                if (dr == DialogResult.No)
                {
                    chkAudioExpert.Checked = false;
                    return;
                }
            }

            bool b = chkAudioExpert.Checked;
            grpAudioLatency1.Visible = b;
            grpAudioVolts1.Visible = ((b && !radGenModelFLEX5000.Checked) || (comboAudioSoundCard.Text == "Unsupported Card" && !radGenModelFLEX5000.Checked));
        }

        private void udMeterDigitalDelay_ValueChanged(object sender, System.EventArgs e)
        {
            console.MeterDigDelay = (int)udMeterDigitalDelay.Value;
        }

        private void chkMouseTuneStep_CheckedChanged(object sender, System.EventArgs e)
        {
            console.MouseTuneStep = chkMouseTuneStep.Checked;
        }

        private void chkGenDDSExpert_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkGenDDSExpert.Checked && chkGenDDSExpert.Focused)
            {
                DialogResult dr = MessageBox.Show(new Form { TopMost = true }, "The Expert mode allows the user to control advanced controls that only " +
                    "experienced PowerSDR users should use.  These controls may allow the user " +
                    "to change important calibration parameters.\r\n\r\n" +
                    "Are you sure you want to enable Expert mode?",
                    "Warning: Enable Expert Mode?",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);
                if (dr == DialogResult.No)
                {
                    chkGenDDSExpert.Checked = false;
                    return;
                }
            }

            bool b = chkGenDDSExpert.Checked;
            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                case Model.FLEX3000:
                case Model.FLEX1500:
                    lblClockCorrection.Visible = b;
                    udDDSCorrection.Visible = b;
                    lblIFFrequency.Visible = b;
                    udDDSIFFreq.Visible = b;
                    break;
                default:
                    lblClockCorrection.Visible = b;
                    udDDSCorrection.Visible = b;
                    lblPLLMult.Visible = b;
                    udDDSPLLMult.Visible = b;
                    lblIFFrequency.Visible = b;
                    udDDSIFFreq.Visible = b;
                    break;
            }
        }

        private void chkCalExpert_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radGenModelFLEX5000.Checked)
            {
                if (chkCalExpert.Checked && chkCalExpert.Focused)
                {
                    DialogResult dr = MessageBox.Show(new Form { TopMost = true }, "The Expert mode allows the user to control advanced controls that only " +
                        "experienced PowerSDR users should use.  These controls may allow the user " +
                        "to change important calibration parameters.\r\n\r\n" +
                        "Are you sure you want to enable?",
                        "Warning: Enable Expert Mode?",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);
                    if (dr == DialogResult.No)
                    {
                        chkCalExpert.Checked = false;
                        return;
                    }
                }

                bool b = chkCalExpert.Checked;
                switch (console.CurrentModel)
                {
                    case Model.FLEX5000:
                    case Model.FLEX3000:
                        grpGeneralCalibration.Visible = b;
                        grpGenCalLevel.Visible = false;
                        grpGenCalRXImage.Visible = false;
                        break;
                    default:
                        grpGeneralCalibration.Visible = b;
                        grpGenCalLevel.Visible = b;
                        grpGenCalRXImage.Visible = b;
                        break;
                }
            }
        }

        private void chkDSPImageExpert_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radGenModelFLEX5000.Checked)
            {
                if (chkDSPImageExpert.Checked && chkDSPImageExpert.Focused)
                {
                    DialogResult dr = MessageBox.Show(new Form { TopMost = true }, "The Expert mode allows the user to control advanced controls that only " +
                        "experienced PowerSDR users should use.  These controls may allow the user " +
                        "to cause damage to the radio or change important calibration parameters.\r\n\r\n" +
                        "Are you sure you want to enable Expert mode?",
                        "Warning: Enable Expert Mode?",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);
                    if (dr == DialogResult.No)
                    {
                        chkDSPImageExpert.Checked = false;
                        return;
                    }
                }

                bool b = chkDSPImageExpert.Checked;
                grpDSPImageRejectTX.Visible = b;
            }
        }

        public void UpdateCustomTitle()
        {
            txtGenCustomTitle_TextChanged(this, EventArgs.Empty);

        }



        private void txtGenCustomTitle_TextChanged(object sender, System.EventArgs e)
        {
            string title = console.Text;

            int index = title.IndexOf("   --   ");

            if (index >= 0) title = title.Substring(0, index);

            if (txtGenCustomTitle.Text != "" && txtGenCustomTitle.Text != null)
            {
                title += "   --   " + txtGenCustomTitle.Text;
            }

            console.Text = title;

        }

        private void chkGenFLEX5000ExtRef_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radGenModelFLEX5000.Checked)
                FWC.SetXREF(chkGenFLEX5000ExtRef.Checked);
        }

        private void chkGenFLEX1500Xref_CheckedChanged(object sender, EventArgs e)
        {
            if (radGenModelFLEX1500.Checked)
            {
                if (chkGenFLEX1500Xref.Checked && chkGenFLEX1500Xref.Focused)
                {
                    DialogResult dr = MessageBox.Show(new Form { TopMost = true }, "Warning: Checking this box without an external reference present will\n" +
                        "cause the radio not to function.  Are you sure you want to enabled the Ext. Reference Input?",
                        "Warning: Ext Ref Present?",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);
                    if (dr == DialogResult.No)
                    {
                        chkGenFLEX1500Xref.Checked = false;
                        return;
                    }
                }

                if (console.hid_init)
                    USBHID.SetXref(chkGenFLEX1500Xref.Checked);
                console.FLEX1500Xref = chkGenFLEX1500Xref.Checked;
            }
        }

        private void chkFPInstalled_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkFPInstalled.Checked)
            {
                flex_profiler_installed = true;
                console.ShowRemoteProfileMenu(true);
            }
            else
            {
                flex_profiler_installed = false;
                console.ShowRemoteProfileMenu(false);
            }

        }

        private void chkGenAllModeMicPTT_CheckedChanged(object sender, System.EventArgs e)
        {
            console.AllModeMicPTT = chkGenAllModeMicPTT.Checked;
        }



        private void udLMSNR_ValueChanged(object sender, System.EventArgs e)
        {
            console.dsp.GetDSPRX(0, 0).SetNRVals(
                (int)udLMSNRtaps.Value,
                (int)udLMSNRdelay.Value,
                1e-4 * (double)udLMSNRgain.Value,
                1e-7 * (double)udLMSNRLeak.Value);
            console.dsp.GetDSPRX(0, 1).SetNRVals(
                (int)udLMSNRtaps.Value,
                (int)udLMSNRdelay.Value,
                1e-4 * (double)udLMSNRgain.Value,
                1e-7 * (double)udLMSNRLeak.Value);

            if (chkDSPRX2.Checked == true) // ke9ns add  console.chkRX2.Visible == true &&
            {
                console.dsp.GetDSPRX(1, 0).SetNRVals(
                    (int)udLMSNRtaps.Value,
                    (int)udLMSNRdelay.Value,
                    1e-4 * (double)udLMSNRgain.Value,
                    1e-7 * (double)udLMSNRLeak.Value);
                console.dsp.GetDSPRX(1, 1).SetNRVals(
                    (int)udLMSNRtaps.Value,
                    (int)udLMSNRdelay.Value,
                    1e-4 * (double)udLMSNRgain.Value,
                    1e-7 * (double)udLMSNRLeak.Value);
            } // if(console.chkRX2.Visible == true)

        } // udLMSNR_ValueChanged

        private void udLMSANF_ValueChanged(object sender, System.EventArgs e)
        {
            console.dsp.GetDSPRX(0, 0).SetANFVals(
                (int)udLMSANFtaps.Value,
                (int)udLMSANFdelay.Value,
                1e-4 * (double)udLMSANFgain.Value,
                1e-7 * (double)udLMSANFLeak.Value);
            console.dsp.GetDSPRX(0, 1).SetANFVals(
                (int)udLMSANFtaps.Value,
                (int)udLMSANFdelay.Value,
                1e-4 * (double)udLMSANFgain.Value,
                1e-7 * (double)udLMSANFLeak.Value);


            if (chkDSPRX2.Checked == true) // ke9ns add  console.chkRX2.Visible == true && 
            {
                console.dsp.GetDSPRX(0, 0).SetANFVals(
                 (int)udLMSANFtaps.Value,
                 (int)udLMSANFdelay.Value,
                 1e-4 * (double)udLMSANFgain.Value,
                 1e-7 * (double)udLMSANFLeak.Value);
                console.dsp.GetDSPRX(0, 1).SetANFVals(
                    (int)udLMSANFtaps.Value,
                    (int)udLMSANFdelay.Value,
                    1e-4 * (double)udLMSANFgain.Value,
                    1e-7 * (double)udLMSANFLeak.Value);
            } // if(console.chkRX2.Visible == true)

        } //udLMSANF_ValueChanged

        private void chkKWAI_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkKWAI.Checked)
                AllowFreqBroadcast = true;
            else
                AllowFreqBroadcast = false;
        }
        private void chkKWAI2_CheckedChanged(object sender, System.EventArgs e) // ke9ns add .214
        {
            if (chkKWAI2.Checked)
                AllowFreqBroadcast2 = true;
            else
                AllowFreqBroadcast2 = false;
        }

        private void chkKWAI3_CheckedChanged(object sender, System.EventArgs e) // ke9ns add .214
        {
            if (chkKWAI3.Checked)
                AllowFreqBroadcast3 = true;
            else
                AllowFreqBroadcast3 = false;
        }

        private void chkKWAI4_CheckedChanged(object sender, System.EventArgs e) // ke9ns add .214
        {
            if (chkKWAI4.Checked)
                AllowFreqBroadcast4 = true;
            else
                AllowFreqBroadcast4 = false;
        }

        private void chkKWAI5_CheckedChanged(object sender, System.EventArgs e) // ke9ns add .214
        {
            if (chkKWAI5.Checked)
                AllowFreqBroadcast5 = true;
            else
                AllowFreqBroadcast5 = false;
        }

        private void chkKWAI6_CheckedChanged(object sender, System.EventArgs e) // ke9ns add .214
        {
            if (chkKWAI6.Checked)
                AllowFreqBroadcast6 = true;
            else
                AllowFreqBroadcast6 = false;
        }


        private void chkKWAI7_CheckedChanged(object sender, System.EventArgs e) // ke9ns add .214
        {
            if (chkKWAI7.Checked)
                AllowFreqBroadcast7 = true;
            else
                AllowFreqBroadcast7 = false;
        }


        private void chkSplitOff_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkSplitOff.Checked)
                console.DisableSplitOnBandchange = true;
            else
                console.DisableSplitOnBandchange = false;
        }

        private void chkSplitOffOnModeChange_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSplitOffOnModeChange.Checked)
                console.DisableSplitOnModeChange = true;
            else
                console.DisableSplitOnModeChange = false;
        }

        private void chkRememberTXProfileOnModeChange_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRememberTXProfileOnModeChange.Checked)
                console.TXProfileByMode = true;
            else
                console.TXProfileByMode = false;
        }

        public bool RFE_PA_TR
        {
            get { return chkEnableRFEPATR.Checked; }
            set { chkEnableRFEPATR.Checked = value; }
        }


        // ke9ns add
        //    public bool GRIDLINES
        //   {
        //       get { return gridBoxTS.Checked; }
        //       set { gridBoxTS.Checked = value; }
        //   }


        private void chkEnableRFEPATR_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkEnableRFEPATR.Checked)
                console.RFE_PA_TR_enable = true;
            else
                console.RFE_PA_TR_enable = false;
        }

        private void chkVACAllowBypass_CheckedChanged(object sender, System.EventArgs e)
        {
            console.AllowVACBypass = chkVACAllowBypass.Checked;
        }

        private void chkDSPTXMeterPeak_CheckedChanged(object sender, System.EventArgs e)
        {
            console.PeakTXMeter = chkDSPTXMeterPeak.Checked;
            console.PeakTX1Meter = chkDSPTXMeterPeak.Checked;
        }

        private void chkVACCombine_CheckedChanged(object sender, System.EventArgs e)
        {
            Audio.VACCombineInput = chkVACCombine.Checked;
        }

        private void chkCWAutoSwitchMode_CheckedChanged(object sender, System.EventArgs e)
        {
            console.CWAutoModeSwitch = chkCWAutoSwitchMode.Checked;
        }

        private void clrbtnGenBackground_Changed(object sender, System.EventArgs e)//k6jca 1/13/08
        {
            //console.GenBackgroundColor = clrbtnGenBackground.Color;
        }

        public MeterTXMode TuneMeterTXMode
        {
            set
            {
                switch (value)
                {
                    case MeterTXMode.FORWARD_POWER:
                        comboTXTUNMeter.Text = "Fwd Pwr";
                        break;
                    case MeterTXMode.REVERSE_POWER:
                        comboTXTUNMeter.Text = "Ref Pwr";
                        break;
                    case MeterTXMode.SWR:
                        comboTXTUNMeter.Text = "SWR";
                        break;
                    case MeterTXMode.OFF:
                        comboTXTUNMeter.Text = "Off";
                        break;
                }
            }
        }

        private void comboTXTUNMeter_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            switch (comboTXTUNMeter.Text)
            {
                case "Fwd Pwr":
                    console.TuneTXMeterMode = MeterTXMode.FORWARD_POWER;
                    break;
                case "Ref Pwr":
                    console.TuneTXMeterMode = MeterTXMode.REVERSE_POWER;
                    break;
                case "SWR":
                    console.TuneTXMeterMode = MeterTXMode.SWR;
                    break;
                case "Off":
                    console.TuneTXMeterMode = MeterTXMode.OFF;
                    break;
            }
        }


        //=============================================================================
        private void btnResetDB_Click(object sender, System.EventArgs e)
        {
            console.chkPower.Checked = false; // ke9ns STOP

            DialogResult dr = MessageBox.Show(new Form { TopMost = true }, "This will close the program, make a copy of the current\n" +
                                              "database to your desktop, and reset the active database\n" +
                                              "to factory defaults the next time PowerSDR is launched.\n\n" +
                                              "Are you sure you want to reset to factory defaults?",
                "Set To Factory Defaults?",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (dr == DialogResult.No) return;

            console.reset_db = true;

            // Change sampling rate to default for Firewire radios
            if (console.CurrentModel == Model.FLEX5000 || console.CurrentModel == Model.FLEX3000)
            {
                comboAudioSampleRate1.Text = "96000";
                comboAudioSampleRate1_SelectedIndexChanged(this, new EventArgs());
            }


            if (SpotForm != null) // ke9ns add .198
            {
                Debug.WriteLine("SPOT TURNED OFF " + SpotForm.checkBoxMUF.Checked + " , " + SpotForm.VOARUN + " , " + SpotForm.VOARUN + " , " + SpotControl.SP5_Active + " , " + SpotForm.mapon);

                SpotForm.checkBoxMUF.CheckedChanged -= SpotForm.checkBoxMUF_CheckedChanged;
                SpotForm.checkBoxMUF.Checked = false;
                SpotControl.Map_Last = SpotControl.Map_Last | 2;    // force update of world map


                SpotControl.SP5_Active = 0;                     // turn off tracking

               

                Debug.WriteLine("SPOT TURNED OFF- " + SpotForm.checkBoxMUF.Checked + " , " + SpotForm.VOARUN + " , " + SpotForm.VOARUN + " , " + SpotControl.SP5_Active + " , " + SpotForm.mapon);

            } //  if (SpotForm != null) 

            console.Close();
        } // btnResetDB_Click




        private void chkDisplayMeterShowDecimal_CheckedChanged(object sender, System.EventArgs e)
        {
            console.MeterDetail = chkDisplayMeterShowDecimal.Checked;
        }

        private void chkRTTYOffsetEnableA_CheckedChanged(object sender, System.EventArgs e)
        {
            rtty_offset_enabled_a = chkRTTYOffsetEnableA.Checked;
        }

        private void chkRTTYOffsetEnableB_CheckedChanged(object sender, System.EventArgs e)
        {
            rtty_offset_enabled_b = chkRTTYOffsetEnableB.Checked;
        }

        private void udRTTYL_ValueChanged(object sender, System.EventArgs e)
        {
            rtty_offset_low = (int)udRTTYL.Value;
        }

        private void udRTTYU_ValueChanged(object sender, System.EventArgs e)
        {
            rtty_offset_high = (int)udRTTYU.Value;
        }

        private void chkRX2AutoMuteRX2OnVFOATX_CheckedChanged(object sender, System.EventArgs e)
        {
            console.MuteRX2OnVFOATX = chkRX2AutoMuteRX2OnVFOATX.Checked;
        }

        private void chkAudioIQtoVAC_CheckedChanged(object sender, System.EventArgs e)
        {
            bool power = console.PowerOn;
            if (power && chkAudioEnableVAC.Checked)
            {
                console.PowerOn = false;
                Thread.Sleep(500);
            }

            if (console.SpurReduction)
                console.SpurReduction = false;

            Audio.VACOutputIQ = chkAudioIQtoVAC.Checked;

            if (power && chkAudioEnableVAC.Checked)
                console.PowerOn = true;

            chkAudioCorrectIQ.Enabled = chkAudioIQtoVAC.Checked;
            chkAudioRX2toVAC.Enabled = chkAudioIQtoVAC.Checked;
        }

        private void chkVAC2DirectIQ_CheckedChanged(object sender, EventArgs e)
        {
            bool power = console.PowerOn;
            if (power && chkVAC2Enable.Checked)
            {
                console.PowerOn = false;
                Thread.Sleep(500);
            }

            if (console.SpurReduction)
                console.SpurReduction = false;

            Audio.VAC2OutputIQ = chkVAC2DirectIQ.Checked;

            if (power && chkVAC2Enable.Checked)
                console.PowerOn = true;

            chkVAC2DirectIQCal.Enabled = chkVAC2DirectIQ.Checked;
        }

        private void chkAudioCorrectIQ_CheckChanged(object sender, System.EventArgs e)
        {
            Audio.VACCorrectIQ = chkAudioCorrectIQ.Checked;
        }

        private void chkVAC2DirectIQCal_CheckedChanged(object sender, EventArgs e)
        {
            Audio.VAC2CorrectIQ = chkVAC2DirectIQCal.Checked;
        }

        private void chkRX2AutoMuteRX1OnVFOBTX_CheckedChanged(object sender, System.EventArgs e)
        {
            console.MuteRX1OnVFOBTX = chkRX2AutoMuteRX1OnVFOBTX.Checked;
        }

        private void chkTXExpert_CheckedChanged(object sender, System.EventArgs e)
        {
            grpTXProfileDef.Visible = chkTXExpert.Checked;
        }


        //======================================================================
        private void btnTXProfileDefImport_Click(object sender, System.EventArgs e)
        {
            if (lstTXProfileDef.SelectedIndex < 0) return;

            DialogResult result = MessageBox.Show(new Form { TopMost = true }, "Import profile from defaults?",
                "Import?",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.No)
                return;

            string name = lstTXProfileDef.Text;

            DB.ModTXProfileTable(); // ke9ns add: to modify old databases
            DB.ModTXProfileDefTable();


            DataRow[] rows = DB.ds.Tables["TxProfileDef"].Select("'" + name + "' = Name");

            if (rows.Length != 1)
            {
                MessageBox.Show(new Form { TopMost = true }, "Database error reading TXProfileDef Table.",
                    "Database error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            DataRow dr = null;
            if (comboTXProfileName.Items.Contains(name))
            {
                result = MessageBox.Show(
                    "Are you sure you want to overwrite the " + name + " TX Profile?",
                    "Overwrite Profile?",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.No)
                    return;

                foreach (DataRow d in DB.ds.Tables["TxProfile"].Rows)
                {
                    if ((string)d["Name"] == name)
                    {


                        try
                        {
                            //  Debug.WriteLine("TXNAME btnTXProfileDefImport");


                            if ((int)d["TX28EQ2"] == 0)
                            {
                                // this is just to cause a catch if its an old TXProfile
                            }
                            if ((int)d["PEQ2"] == 0)
                            {
                                // this is just to cause a catch if its an old TXProfile
                            }
                            if ((bool)d["TXEQ10Band"] == false)
                            {

                            }

                            Debug.WriteLine("YES TXEQ12, tx28eq2, peq2 ");
                            dr = d; // ke9ns when you match the name, copy over all the rows into the DataRow dr
                        }
                        catch (Exception)// do this to remove an old TXPROFILE with only TXEQ10 and then add back the same TXPROFILE with TXEQ28
                        {
                            Debug.WriteLine("1Remove old TXprofile and create new version");

                            DB.ModTXProfileTable();
                            DB.ModTXProfileDefTable();

                            DataRow[] rows1 = DB.ds.Tables["TxProfile"].Select("'" + comboTXProfileName.Text + "' = Name");

                            DataRow dd = null;

                            dd = rows1[0];

                            rows1[0].Delete();

                            dr = DB.ds.Tables["TxProfile"].NewRow(); // ke9ns create a new TXPROFILE with all the new Rows found in ds.Tables

                            dr = dd; // copy original data back into 
                            dr["Name"] = name;

                            DB.ds.Tables["TxProfile"].Rows.Add(dr); // add a new TX profile

                        } // catch
                        break;


                    }
                }
            }
            else
            {
                dr = DB.ds.Tables["TxProfile"].NewRow();
                dr["Name"] = name;
            }

            for (int i = 0; i < dr.ItemArray.Length; i++)
                dr[i] = rows[0][i];

            if (!comboTXProfileName.Items.Contains(name))
            {
                DB.ds.Tables["TxProfile"].Rows.Add(dr);
                comboTXProfileName.Items.Add(name);
                comboTXProfileName.Text = name;
            }

            console.UpdateTXProfile(name);

        } // btnIMPORTtxprofile


        private void Setup_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.Control == true && e.Alt == true)
            {
                switch (e.KeyCode)
                {
                    case Keys.A:
                        chkPANewCal.Visible = true;
                        grpPAGainByBand.Visible = true;
                        break;
                    case Keys.O:
                        rdSigGenTXOutput.Visible = true;
                        break;
                }
            }

            if (e.KeyCode == Keys.F1) // ke9ns add for help messages (F1 help screen)
            {


                Debug.WriteLine("F1 key");

                if (MouseIsOverControl(chkBoxHTTP) == true)
                {
                    if (console.helpboxForm == null || console.helpboxForm.IsDisposed) console.helpboxForm = new helpbox(console);

                    console.helpboxForm.Show();
                    console.helpboxForm.Focus();
                    console.helpboxForm.WindowState = FormWindowState.Normal; // ke9ns add
                    console.helpboxForm.youtube_embed = @"https://ke9ns.com";
                    console.helpboxForm.helpbox_message.Text = console.helpboxForm.HTTPSERVER.Text;
                }
                else if (MouseIsOverControl(checkBoxHTTP1) == true)
                {
                    if (console.helpboxForm == null || console.helpboxForm.IsDisposed) console.helpboxForm = new helpbox(console);

                    console.helpboxForm.Show();
                    console.helpboxForm.Focus();
                    console.helpboxForm.WindowState = FormWindowState.Normal; // ke9ns add
                    console.helpboxForm.youtube_embed = @"https://ke9ns.com";
                    console.helpboxForm.helpbox_message.Text = console.helpboxForm.HTTPSERVER.Text;
                }
                else if (MouseIsOverControl(checkBoxN1MM) == true) // dont have N1MM help screen yet
                {
                    // if (console.helpboxForm == null || console.helpboxForm.IsDisposed) console.helpboxForm = new helpbox(console);

                    //  console.helpboxForm.Show();
                    //  console.helpboxForm.Focus();
                    //  console.helpboxForm.WindowState = FormWindowState.Normal; // ke9ns add
                    //   console.helpboxForm.youtube_embed = @"https://ke9ns.com";
                    //  console.helpboxForm.helpbox_message.Text = console.helpboxForm.HTTPSERVER.Text;
                }
                else if (MouseIsOverControl(groupBox2) == true)
                {
                    if (console.helpboxForm == null || console.helpboxForm.IsDisposed) console.helpboxForm = new helpbox(console);

                    console.helpboxForm.Show();
                    console.helpboxForm.Focus();
                    console.helpboxForm.WindowState = FormWindowState.Normal; // ke9ns add
                    console.helpboxForm.youtube_embed = @"https://ke9ns.com";
                    console.helpboxForm.helpbox_message.Text = console.helpboxForm.HTTPSERVER.Text;
                }

                // 

            } // if (e.KeyCode == Keys.F1)
        }

        // ke9ns add
        public bool MouseIsOverControl(Control c) // ke9ns keypreview must be TRUE and use MouseIsOverControl(Control c)
        {
            return c.ClientRectangle.Contains(c.PointToClient(Control.MousePosition));
        }

        private void chkDisplayPanFill_CheckedChanged(object sender, System.EventArgs e)
        {

            Display.PanFill = chkDisplayPanFill.Checked;
            Display.DisplayPanFillColor = Color.FromArgb(tbPanAlpha.Value, clrbtnPan.Color); // ke9ns add combine color and alpha here

        }


        private void udF3KFanTempThresh_ValueChanged(object sender, System.EventArgs e)
        {
            console.F3KTempThresh = (float)udF3KFanTempThresh.Value;
        }

        private void chkGenTX1Delay_CheckedChanged(object sender, System.EventArgs e)
        {
            bool b = chkGenTX1Delay.Checked;
            switch (console.CurrentModel)
            {
                case Model.FLEX3000:
                    if (!console.fwc_init) return;
                    FWC.SetAmpTX1DelayEnable(b);
                    udGenTX1Delay.Enabled = b;
                    udGenTX1Delay_ValueChanged(sender, e);
                    break;
                case Model.FLEX1500:
                    if (!console.hid_init) return;
                    USBHID.EnableTXOutDelay(b);
                    udGenTX1Delay.Enabled = b;
                    udGenTX1Delay_ValueChanged(sender, e);
                    break;
            }
        }

        private void udGenTX1Delay_ValueChanged(object sender, System.EventArgs e)
        {
            switch (console.CurrentModel)
            {
                case Model.FLEX3000:
                    if (!console.fwc_init) return;
                    FWC.SetAmpTX1Delay((uint)udGenTX1Delay.Value);
                    break;
                case Model.FLEX1500:
                    if (!console.hid_init) return;
                    USBHID.SetTXOutDelayValue((uint)udGenTX1Delay.Value);
                    break;
            }
        }

        public void comboAppSkin_SelectedIndexChanged(object sender, EventArgs e)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) +
                "\\FlexRadio Systems\\PowerSDR\\Skins\\";
            if (Directory.Exists(path + comboAppSkin.Text))
            {
                Skin.Restore(comboAppSkin.Text, path, console);
                console.CurrentSkin = comboAppSkin.Text;
                console.RadarColorUpdate = true;
            }

            //  SpotControl.Skin1 = comboAppSkin.Text;

        }

        private void btnSkinExport_Click(object sender, EventArgs e)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) +
                "\\FlexRadio Systems\\PowerSDR\\Skins\\";
            if (Directory.Exists(path + comboAppSkin.Text))
                Skin.Save(comboAppSkin.Text, path, console);
        }

        private void chkCWDisableUI_CheckedChanged(object sender, EventArgs e)
        {
            console.DisableUIMOXChanges = chkCWDisableUI.Checked;
        }

        private void chkAudioRX2toVAC_CheckedChanged(object sender, EventArgs e)
        {
            Audio.VACOutputRX2 = chkAudioRX2toVAC.Checked;
        }

        private void chkGenOptionsShowATUPopup_CheckedChanged(object sender, System.EventArgs e)
        {
            if (console.flex3000ATUForm != null && !console.flex3000ATUForm.IsDisposed)
                console.flex3000ATUForm.ShowFeedbackPopup = chkGenOptionsShowATUPopup.Checked;
        }

        private void chkSpaceNavControlVFOs_CheckChanged(object sender, System.EventArgs e)
        {
            console.SpaceNavControlVFOs = (bool)chkSpaceNavControlVFOs.Checked;
        }

        private void chkSpaceNAvFlyPanadapter_CheckChanged(object sender, System.EventArgs e)
        {
            console.SpaceNavFlyPanadapter = (bool)chkSpaceNavFlyPanadapter.Checked;
        }

        private void tbRX1FilterAlpha_Scroll(object sender, EventArgs e)
        {
            clrbtnFilter_Changed(this, EventArgs.Empty);

            tbWaterOffset_Scroll(this, EventArgs.Empty); // ke9ns add .167
            tbGridOffset_Scroll(this, EventArgs.Empty);
            tbAGCTadj_Scroll(this, EventArgs.Empty);


        }

        private void udTXNoiseGateAttenuate_ValueChanged(object sender, System.EventArgs e)
        {
            console.dsp.GetDSPTX(0).TXSquelchAttenuate = (float)udTXNoiseGateAttenuate.Value;
        }

        private void tbMultiRXFilterAlpha_Scroll(object sender, EventArgs e)
        {
            clrbtnSubRXFilter_Changed(this, EventArgs.Empty);
        }

        private void chkWheelTuneVFOB_CheckedChanged(object sender, EventArgs e)
        {
            console.WheelTunesVFOB = chkWheelTuneVFOB.Checked;
        }

        private void chkCWKeyerMonoCable_CheckedChanged(object sender, EventArgs e)
        {
            if (console.fwc_init && (console.CurrentModel == Model.FLEX5000 || console.CurrentModel == Model.FLEX3000))
            {
                FWC.ignore_dash = chkCWKeyerMonoCable.Checked;
            }
            else if (console.hid_init && (console.CurrentModel == Model.FLEX1500))
            {
                Flex1500.IgnoreDash = chkCWKeyerMonoCable.Checked;
            }

            if (chkCWKeyerMonoCable.Checked)
                CWKeyer.SensorEnqueue(new CWSensorItem(CWSensorItem.InputType.Dash, false));
        }

        private void btnExportDB_Click(object sender, EventArgs e)
        {

            if (!console.DB_Exists)
            {
                MessageBox.Show(new Form { TopMost = true }, "The current PowerSDR database is incomplete and cannot be exported.  " +
                    "A recently reset to Factory Default database was detected that does not yet contain a complete dataset.\n\n" +
                    "You must close PowerSDR and restart before exporting the database.",
                    "Database Export Prohibited",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string datetime = DateTime.Now.ToShortDateString().Replace("/", "-") + "_" +
                    DateTime.Now.ToShortTimeString().Replace(":", ".");
            string model_sn = "";

            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                    model_sn = "FLEX-5000_" + FWCEEPROM.SerialToString(FWCEEPROM.SerialNumber);
                    break;
                case Model.FLEX3000:
                    model_sn = "FLEX-3000_" + FWCEEPROM.SerialToString(FWCEEPROM.SerialNumber);
                    break;
                case Model.FLEX1500:
                    model_sn = "FLEX-1500_" + HIDEEPROM.SerialToString(HIDEEPROM.SerialNumber);
                    break;
                case Model.SDR1000:
                    model_sn = "SDR-1000";
                    break;
                case Model.DEMO:
                    model_sn = "Demo";
                    break;
            }

            saveFileDialog1.FileName = String.Empty;
            saveFileDialog1.FileName = desktop + "\\PowerSDR_database_export_" + model_sn + "_" + datetime + ".xml";
            saveFileDialog1.ShowDialog();
        }


        //====================================================================================
        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            try
            {
                DB.ds.WriteXml(saveFileDialog1.FileName, XmlWriteMode.WriteSchema);
            }
            catch (Exception ex)
            {
                MessageBox.Show(new Form { TopMost = true }, "A database write to file operation failed.  " +
                    "The exception error was:\n\n" + ex.Message,
                    "ERROR: Database Write Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        } // saveFileDialog1_FileOk



        private void udTX1500Blanking_ValueChanged(object sender, EventArgs e)
        {
            console.PhoneBlankTime = (int)udTX1500PhoneBlanking.Value;
        }

        private void udPulseDuty_ValueChanged(object sender, EventArgs e)
        {
            Audio.PulseDuty = (double)udPulseDuty.Value / 100.0;
        }

        private void udPulsePeriod_ValueChanged(object sender, EventArgs e)
        {
            Audio.PulsePeriod = (double)udPulsePeriod.Value;
        }

        private void chkStrictCharSpacing_CheckedChanged(object sender, EventArgs e)
        {
            CWKeyer.AutoCharSpace = chkStrictCharSpacing.Checked;
        }

        private void chkModeBStrict_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCWKeyerMode.Checked)
            {
                if (chkModeBStrict.Checked)
                    CWKeyer.CurrentIambicMode = CWKeyer.IambicMode.ModeBStrict;
                else
                    CWKeyer.CurrentIambicMode = CWKeyer.IambicMode.ModeB;
            }
        }

        private void tbOptUSBBuf_Scroll(object sender, EventArgs e)
        {
            if (console.CurrentModel != Model.FLEX1500) return;
            uint val = 1;
            switch (tbOptUSBBuf.Value)
            {
                case 0: val = 16; break;
                case 1: val = 8; break;
                case 2: val = 4; break;
                case 3: val = 2; break;
                case 4: val = 1; break;
            }

            Flex1500.SetFinalPacketSize(val);
        }

        private void chkTXLimitSlew_CheckedChanged(object sender, EventArgs e)
        {
            console.LimitSlew = chkTXLimitSlew.Checked;
        }

        private void chkVAC2UseRX2_CheckedChanged(object sender, EventArgs e)
        {
            console.VAC2RX2 = chkVAC2UseRX2.Checked;
        }

        private void chkRX2DisconnectOnTX_CheckedChanged(object sender, EventArgs e)
        {
            if (radGenModelFLEX5000.Checked && FWCEEPROM.RX2OK)
                FWC.SetRX2DisconnectOnTX(chkRX2DisconnectOnTX.Checked);
        }

        void FlexControlManager_DeviceCountChanged()
        {
            bool present = false;

            if (FlexControlManager.DeviceCount == 0)
                present = false;
            else present = true;

            if (FlexControlPresent == present) return;

            if (!this.InvokeRequired)
                FlexControlPresent = present;
            else Invoke((MethodInvoker)(() => { FlexControlPresent = present; }));
        }

        private void lblBandEdge_Click(object sender, EventArgs e)
        {

        }

        private void grpAppPanadapter_Enter(object sender, EventArgs e)
        {

        }

#if (NO_DJ)
        //mod DH1TW

        private DJConsoleMK2Config ConfigWindowMK2;
        private DJConsoleMP3e2Config ConfigWindowMP3e2;
        private DJConsoleMP3LEConfig ConfigWindowMP3LE;
#endif
        private void btnSelectUserInterface_Click(object sender, EventArgs e)
        {

        }

        private void DJConfiguratorClosed(object sender, FormClosedEventArgs e)
        {
#if (NO_DJ)
            console.DJConsoleConfigurator = null;
#endif
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void InitDJConsoles()
        {
#if (NO_DJ)
            if (console.DJConsoleObj == null) return;
            if (console.DJConsoleObj.connectedConsoles.Count > 0)
            {
                cbConsoleSelect.DataSource = new BindingSource(console.DJConsoleObj.connectedConsoles, null);
                cbConsoleSelect.DisplayMember = "Value";
                cbConsoleSelect.ValueMember = "Key";
                console.DJConsoleObj.SelectedConsole = (int)cbConsoleSelect.SelectedValue;
                this.cbConsoleSelect.SelectedIndexChanged += new System.EventHandler(this.cbConsoleSelect_SelectedIndexChanged);
            }
            else
            {
                //MessageBox.Show(new Form { TopMost = true }, "Sorry, no compatible device detected", "Error");
                //this.Dispose();
            }
#endif
        } // initDJ

        private void btnConfigure_Click(object sender, EventArgs e)
        {

#if (NO_DJ)
            if (console.DJConsoleObj.SelectedConsole == (int)DJConsoleModels.NotSupported)
            {
                // MessageBox.Show(new Form { TopMost = true }, "not supported");
                return;
            }


            if (console.DJConsoleObj.SelectedConsole == (int)DJConsoleModels.HerculesMP3e2)
            {
                if (ConfigWindowMP3e2 == null)
                {
                    ConfigWindowMP3e2 = new DJConsoleMP3e2Config(console);
                    ConfigWindowMP3e2.Show();
                    ConfigWindowMP3e2.Focus();
                    ConfigWindowMP3e2.FormClosed += new FormClosedEventHandler(ConfigWindowMP3e2Closed);
                }
                return;
            }


            if (console.DJConsoleObj.SelectedConsole == (int)DJConsoleModels.HerculesMK2)
            {
                if (ConfigWindowMK2 == null)
                {
                    ConfigWindowMK2 = new DJConsoleMK2Config(console);
                    ConfigWindowMK2.Show();
                    ConfigWindowMK2.Focus();
                    ConfigWindowMK2.FormClosed += new FormClosedEventHandler(ConfigWindowMK2Closed);
                }
                return;
            }

            if (console.DJConsoleObj.SelectedConsole == (int)DJConsoleModels.HerculesMP3LE)
            {
                if (ConfigWindowMP3LE == null)
                {
                    ConfigWindowMP3LE = new DJConsoleMP3LEConfig(console);
                    ConfigWindowMP3LE.Show();
                    ConfigWindowMP3LE.Focus();
                    ConfigWindowMP3LE.FormClosed += new FormClosedEventHandler(ConfigWindowMP3LEClosed);
                }
                return;

            }

            else
            {
                MessageBox.Show(new Form { TopMost = true }, "Please select a Console", "Error");
            }
#endif
        }// button click

#if (NO_DJ)
        private void ConfigWindowMK2Closed(object sender, FormClosedEventArgs e)
        {
            if (ConfigWindowMK2 != null)
            {
                ConfigWindowMK2 = null;
            }
        }

        private void ConfigWindowMP3e2Closed(object sender, FormClosedEventArgs e)
        {
            if (ConfigWindowMP3e2 != null)
            {
                ConfigWindowMP3e2 = null;
            }
        }

        private void ConfigWindowMP3LEClosed(object sender, FormClosedEventArgs e)
        {
            if (ConfigWindowMP3LE != null)
            {
                ConfigWindowMP3LE = null;
            }
        }


#endif

        private void btnSave_Click(object sender, EventArgs e)
        {
#if (NO_DJ)
            if (cbConsoleSelect.SelectedItem != null)
            {
                console.DJConsoleObj.SelectedConsole = (int)cbConsoleSelect.SelectedValue;
                console.DJConsoleObj.Reload();
            }
#endif
        }

        private void cbConsoleSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
#if (NO_DJ)
            if (cbConsoleSelect.SelectedItem != null)
            {
                console.DJConsoleObj.SelectedConsole = (int)cbConsoleSelect.SelectedValue;
                console.DJConsoleObj.Reload();
            }

#endif
        }

        // end mod DH1TW


        //============================================================================================================
        //============================================================================================================
        // ke9ns add  for RX2 low level waterfall (but it gets its value from the stored RX1 band low level data) no need to store seperate
        //============================================================================================================
        //============================================================================================================
        public void udDisplayWaterfallRX2Level_ValueChanged(object sender, EventArgs e) // ke9ns ADD
        {

            switch (console.RX2Band)
            {
                case Band.B160M:
                    console.WaterfallLowRX2Threshold160m = (float)udDisplayWaterfallRX2Level.Value;
                    Display.WaterfallLowRX2Threshold = console.WaterfallLowRX2Threshold160m;
                    break;
                case Band.B80M:
                    console.WaterfallLowRX2Threshold80m = (float)udDisplayWaterfallRX2Level.Value;
                    Display.WaterfallLowRX2Threshold = console.WaterfallLowRX2Threshold80m;
                    break;
                case Band.B60M:
                    console.WaterfallLowRX2Threshold60m = (float)udDisplayWaterfallRX2Level.Value;
                    Display.WaterfallLowRX2Threshold = console.WaterfallLowRX2Threshold60m;
                    break;
                case Band.B40M:
                    console.WaterfallLowRX2Threshold40m = (float)udDisplayWaterfallRX2Level.Value;
                    Display.WaterfallLowRX2Threshold = console.WaterfallLowRX2Threshold40m;
                    break;
                case Band.B30M:
                    console.WaterfallLowRX2Threshold30m = (float)udDisplayWaterfallRX2Level.Value;
                    Display.WaterfallLowRX2Threshold = console.WaterfallLowRX2Threshold30m;
                    break;
                case Band.B20M:
                    console.WaterfallLowRX2Threshold20m = (float)udDisplayWaterfallRX2Level.Value;
                    Display.WaterfallLowRX2Threshold = console.WaterfallLowRX2Threshold20m;
                    break;
                case Band.B17M:
                    console.WaterfallLowRX2Threshold17m = (float)udDisplayWaterfallRX2Level.Value;
                    Display.WaterfallLowRX2Threshold = console.WaterfallLowRX2Threshold17m;
                    break;
                case Band.B15M:
                    console.WaterfallLowRX2Threshold15m = (float)udDisplayWaterfallRX2Level.Value;
                    Display.WaterfallLowRX2Threshold = console.WaterfallLowRX2Threshold15m;
                    break;
                case Band.B12M:
                    console.WaterfallLowRX2Threshold12m = (float)udDisplayWaterfallRX2Level.Value;
                    Display.WaterfallLowRX2Threshold = console.WaterfallLowRX2Threshold12m;
                    break;
                case Band.B10M:
                    console.WaterfallLowRX2Threshold10m = (float)udDisplayWaterfallRX2Level.Value;
                    Display.WaterfallLowRX2Threshold = console.WaterfallLowRX2Threshold10m;
                    break;
                case Band.B6M:
                    console.WaterfallLowRX2Threshold6m = (float)udDisplayWaterfallRX2Level.Value;
                    Display.WaterfallLowRX2Threshold = console.WaterfallLowRX2Threshold6m;
                    break;
                case Band.WWV:
                    console.WaterfallLowRX2ThresholdWWV = (float)udDisplayWaterfallRX2Level.Value;
                    Display.WaterfallLowRX2Threshold = console.WaterfallLowRX2ThresholdWWV;
                    break;
                case Band.GEN:
                    console.WaterfallLowRX2ThresholdGEN = (float)udDisplayWaterfallRX2Level.Value;
                    Display.WaterfallLowRX2Threshold = console.WaterfallLowRX2ThresholdGEN;
                    break;


                // ke9ns add: .158

                case Band.BLMF:
                    console.WaterfallLowRX2ThresholdLMF = (float)udDisplayWaterfallRX2Level.Value;
                    Display.WaterfallLowRX2Threshold = console.WaterfallLowRX2ThresholdLMF;
                    break;
                case Band.B120M:
                    console.WaterfallLowRX2Threshold120m = (float)udDisplayWaterfallRX2Level.Value;
                    Display.WaterfallLowRX2Threshold = console.WaterfallLowRX2Threshold120m;
                    break;
                case Band.B90M:
                    console.WaterfallLowRX2Threshold90m = (float)udDisplayWaterfallRX2Level.Value;
                    Display.WaterfallLowRX2Threshold = console.WaterfallLowRX2Threshold90m;
                    break;
                case Band.B61M:
                    console.WaterfallLowRX2Threshold61m = (float)udDisplayWaterfallRX2Level.Value;
                    Display.WaterfallLowRX2Threshold = console.WaterfallLowRX2Threshold61m;
                    break;
                case Band.B49M:
                    console.WaterfallLowRX2Threshold49m = (float)udDisplayWaterfallRX2Level.Value;
                    Display.WaterfallLowRX2Threshold = console.WaterfallLowRX2Threshold49m;
                    break;
                case Band.B41M:
                    console.WaterfallLowRX2Threshold41m = (float)udDisplayWaterfallRX2Level.Value;
                    Display.WaterfallLowRX2Threshold = console.WaterfallLowRX2Threshold41m;
                    break;
                case Band.B31M:
                    console.WaterfallLowRX2Threshold31m = (float)udDisplayWaterfallRX2Level.Value;
                    Display.WaterfallLowRX2Threshold = console.WaterfallLowRX2Threshold31m;
                    break;
                case Band.B25M:
                    console.WaterfallLowRX2Threshold25m = (float)udDisplayWaterfallRX2Level.Value;
                    Display.WaterfallLowRX2Threshold = console.WaterfallLowRX2Threshold25m;
                    break;
                case Band.B22M:
                    console.WaterfallLowRX2Threshold22m = (float)udDisplayWaterfallRX2Level.Value;
                    Display.WaterfallLowRX2Threshold = console.WaterfallLowRX2Threshold22m;
                    break;
                case Band.B19M:
                    console.WaterfallLowRX2Threshold19m = (float)udDisplayWaterfallRX2Level.Value;
                    Display.WaterfallLowRX2Threshold = console.WaterfallLowRX2Threshold19m;
                    break;
                case Band.B16M:
                    console.WaterfallLowRX2Threshold16m = (float)udDisplayWaterfallRX2Level.Value;
                    Display.WaterfallLowRX2Threshold = console.WaterfallLowRX2Threshold16m;
                    break;
                case Band.B14M:
                    console.WaterfallLowRX2Threshold14m = (float)udDisplayWaterfallRX2Level.Value;
                    Display.WaterfallLowRX2Threshold = console.WaterfallLowRX2Threshold14m;
                    break;
                case Band.B13M:
                    console.WaterfallLowRX2Threshold13m = (float)udDisplayWaterfallRX2Level.Value;
                    Display.WaterfallLowRX2Threshold = console.WaterfallLowRX2Threshold13m;
                    break;
                case Band.B11M:
                    console.WaterfallLowRX2Threshold11m = (float)udDisplayWaterfallRX2Level.Value;
                    Display.WaterfallLowRX2Threshold = console.WaterfallLowRX2Threshold11m;
                    break;

                default:
                    console.WaterfallLowRX2ThresholdXVTR = (float)udDisplayWaterfallRX2Level.Value;
                    Display.WaterfallLowRX2Threshold = console.WaterfallLowRX2ThresholdXVTR;
                    break;
            }

        }// rx2 water level

        //============================================================================================================
        //============================================================================================================
        // ke9ns add  selects a TX low waterfall level
        //============================================================================================================
        //============================================================================================================
        public void udDisplayWaterfallMicLevel_ValueChanged(object sender, EventArgs e) // ke9ns Add
        {
            console.WaterfallLowThresholdMic = (float)udDisplayWaterfallMicLevel.Value;
            Display.WaterfallLowMicThreshold = console.WaterfallLowThresholdMic;  // value passed into display.cs file

        } // tx waterfall level


        //============================================================================================================
        //============================================================================================================
        // ke9ns add  This selects a wider waterfall (so less refreshing, but slower)
        //============================================================================================================
        //============================================================================================================
        public void checkWaterMoveSize_CheckedChanged(object sender, EventArgs e)// ke9ns Add
        {
            if (checkWaterMoveSize.Checked)
            {
                Display.WMS = 1;  // value passed into display.cs file (lare waterall width x5,1) .0222 seconds full waterfall line
            }
            else
            {
                Display.WMS = 0;  // value passed into display.cs file (small waterfall width x3,1) .0163 seconds full waterfall line
            }
        } //checkWaterMoveSize


        //=======================================================================
        // ke9ns add  open up folder to skins
        private void comboAppSkin_MouseHover(object sender, EventArgs e)
        {





        } // comboAppSkin_MouseHover

        private void grpAppSkins_MouseHover(object sender, EventArgs e)
        {

        }


        // ke9ns add RIGHT CLICK on SKIN goes to skin folder
        private void comboAppSkin_MouseDown(object sender, MouseEventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;

            if (me.Button == System.Windows.Forms.MouseButtons.Right)
            {
                Debug.WriteLine("right click ");

                //  string filePath = "%COMMONAPPDATAROOT%\\FlexRadio Systems\\PowerSDR\\Skins\\Default 2012";

                string filePath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\FlexRadio Systems\\PowerSDR\\Skins";

                if (!Directory.Exists(filePath))
                {
                    Debug.WriteLine("problem1 ");
                    return;
                }

                string argument = @"/select, " + filePath;

                System.Diagnostics.Process.Start("explorer.exe", argument);


            }
            if (me.Button == System.Windows.Forms.MouseButtons.Left)
            {


            }
        } // comboAppSkin_MouseDown



        //===================================================================================
        // ke9ns add  turn on/off grid lines in panadapter
        public void gridBoxTS_CheckedChanged(object sender, EventArgs e)
        {

            if (gridBoxTS.Checked == true) Display.GridOff = 1; // gridlines OFF
            else Display.GridOff = 0; // gridlines ON

        }

        // ke9ns add
        private void btnRSTNB_Click(object sender, EventArgs e)
        {
            udDSPNB.Value = 20;
            udDSPHT.SelectedItem = "7";
            udDSPDLY.Value = 2;

        }

        // ke9ns add
        private void btnRSTNB2_Click(object sender, EventArgs e)
        {
            udDSPNB2.Value = 15;
        }
        // ke9ns add
        private void btnRSTNR_Click(object sender, EventArgs e)
        {
            udLMSNRtaps.Value = 40;
            udLMSNRgain.Value = 16;
            udLMSNRdelay.Value = 30;
            udLMSNRLeak.Value = 10;

        }
        // ke9ns add
        private void btnRSTANF_Click(object sender, EventArgs e)
        {
            udLMSANFtaps.Value = 68;
            udLMSANFgain.Value = 25;
            udLMSANFdelay.Value = 60;
            udLMSANFLeak.Value = 1;
        }
        // ke9ns add
        private void udTNFWidth_ValueChanged(object sender, EventArgs e)
        {

        }



        public void tbGrayLineAlpha_Scroll(object sender, EventArgs e)
        {


        }


        //=================================================================================
        // ke9ns add below for pan fill color and alpha


        private void clrbtnPan_Changed(object sender, EventArgs e)
        {
            Display.DisplayPanFillColor = Color.FromArgb(tbPanAlpha.Value, clrbtnPan.Color); // ke9ns  combine color and alpha here

            Display.Gradient(Display.SpectrumGridMax, Display.SpectrumGridMin); // set new Gradient color scheme

        }

        private void tbPanAlpha_Scroll(object sender, EventArgs e)
        {
            Display.PanFillAlpha = tbPanAlpha.Value;

            clrbtnPan_Changed(this, EventArgs.Empty);

            Display.Gradient(Display.SpectrumGridMax, Display.SpectrumGridMin); // set new Gradient color scheme

        }


        // ke9ns add for auto waterfall and panadapter grid levels
        private void tbWaterOffset_Scroll(object sender, EventArgs e)
        {
            Display.WATEROFFSET = tbWaterOffset.Value;

            //   Debug.WriteLine("WATER " + tbWaterOffset.Value);

            if (Display.continuum == 0)
            {
                console.waterpanClick = true;
                console.waterpanClick2 = true;
                console.waterpanClick3 = true;

                Display.AutoBright = 1; // adjust RX or TX
            }

        } // tbWaterOffset_Scroll


        // ke9ns add for auto waterfall and panadapter grid levels
        public void tbGridOffset_Scroll(object sender, EventArgs e)
        {
            Display.GRIDOFFSET = tbGridOffset.Value;

            if (Display.continuum == 0)
            {
                console.waterpanClick = true;
                console.waterpanClick2 = true;
                console.waterpanClick3 = true;

                Display.AutoBright = 2; // adjust RX  panadapter level
            }

        }




        private void checkBoxTS1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void labelTS16_Click(object sender, EventArgs e)
        {

        }





        //==========================================================================
        // ke9ns add PowerMate
        public void chkBoxPM_CheckedChanged(object sender, EventArgs e)
        {
            //  if (FCBasicForm == null || FCBasicForm.IsDisposed) FCBasicForm = new FlexControlBasicForm(console);
            //   FCBasicForm.chkBoxPM.Checked = chkBoxPM.Checked;

            if (chkBoxPM.Checked == true)
            {
                if (console.KBON == 0) // only do if not already done
                {
                    if (!powerMate.Initialize())   // on load of form find Knob
                    {
                        console.KBON = 0;
                        Debug.WriteLine("COULD NOT FIND KNOB2==============");
                    }
                    else
                    {
                        console.KBON = 1;
                        Debug.WriteLine("FOUND KNOB2==============" + console.KBON);
                        powerMate.ButtonEvent += new HidDevice.PowerMate.ButtonHandler(console.OnButtonEvent);      // create button event
                        powerMate.RotateEvent += new HidDevice.PowerMate.RotationHandler(console.OnRotateEvent);    // create rotation event

                        //  this.powerMate.LedBrightness = 60;              // turns on LED light at startup


                    }
                }
            }
            else
            {

                if (console.KBON == 1) powerMate.Shutdown();
                console.KBON = 0;

            }



        } //chkBoxPM_CheckedChanged


        // ke9ns add
        private void udSpeedPM_ValueChanged(object sender, EventArgs e)
        {
            udSpeedPM.Value = udSpeedPM.Value + 0;// in console.cs  public void OnRotateEvent(int value1)

            //  if (FCBasicForm == null || FCBasicForm.IsDisposed) FCBasicForm = new FlexControlBasicForm(console);
            //  FCBasicForm.udSpeedPM.Value = udSpeedPM.Value;

            // in console.cs  public void OnButtonEvent(HidDevice.PowerMate.ButtonState bs, int value, int value1, int value2)
            // in console.cs but not used right public void OnSliderBrightness(object sender, EventArgs e)


        }

        //===========================================================================================
        // ke9ns add
        private void chkBoxHTTP_CheckedChanged(object sender, EventArgs e)
        {
            if (initializing) return;

            Debug.WriteLine("HTTP server " + chkBoxHTTP.Checked);

            if (chkBoxHTTP.Checked == true)
            {
                chkBoxHttp2.Checked = false;

                if (Console.m_terminated == true)
                {
                    Debug.WriteLine("CALL HTTPSERVER1");

                    try
                    {
                        console.HttpServer = true;
                    }
                    catch (Exception e1)
                    {
                        Debug.WriteLine("bad call " + e1);
                    }

                }

            }
            else
            {
                Http.terminate();

            }


        } //chkBoxHTTP_CheckedChanged

        //===============================================================================================
        // ke9ns add
        private void chkBoxHttp2_CheckedChanged(object sender, EventArgs e)
        {

            if (chkBoxHttp2.Checked == true)
            {
                chkBoxHTTP.Checked = false;
                checkBoxHTTP1.Checked = false;


                Http.terminate();

                console.startHttpServer((int)udHttpPort.Value);

            }
            else
            {
                console.stopHttpServer();
            }


        } // chkBoxHttp2_CheckedChanged


        // ke9ns: TuneStepIndex = TuneStepLookup(txtWheelTune.Text); // if you switch modes then lookup the correct tunestep index

        private void txtWheelTune2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) ChangeTuneStepUp2();
        }

        private void btnTuneStepChangeLarger2_Click(object sender, EventArgs e)
        {
            ChangeTuneStepUp2();
        }

        private void btnTuneStepChangeSmaller2_Click(object sender, EventArgs e)
        {
            ChangeTuneStepDown2();
        }

        private void btnTuneStepChangeSmaller3_Click(object sender, EventArgs e)
        {
            ChangeTuneStepDown3();
        }

        private void btnTuneStepChangeLarger3_Click(object sender, EventArgs e)
        {
            ChangeTuneStepUp3();
        }

        // ke9ns see console init routine calls this at powerSDR startup to get values set
        public void ChangeTuneStepUp2()
        {
            tune_step_index4 = tune_step_index2 = (tune_step_index2 + 1) % console.tune_step_list.Count;
            txtWheelTune2.Text = console.tune_step_list[tune_step_index2].Name;

            //    if (console.RX1DSPMode == DSPMode.CWL || console.RX1DSPMode == DSPMode.CWL) txtWheelTune5.Text = txtWheelTune2.Text; //CW mode only
            //    else txtWheelTune7.Text = txtWheelTune2.Text; // All other modes (ie SSB)

        }

        public void ChangeTuneStepDown2()
        {
            tune_step_index4 = tune_step_index2 = (tune_step_index2 - 1 + console.tune_step_list.Count) % console.tune_step_list.Count;
            txtWheelTune2.Text = console.tune_step_list[tune_step_index2].Name;

            //   if (console.RX1DSPMode == DSPMode.CWL || console.RX1DSPMode == DSPMode.CWL) txtWheelTune5.Text = txtWheelTune2.Text; //CW mode only
            //   else txtWheelTune7.Text = txtWheelTune2.Text; // All other modes (ie SSB)

        }

        public void ChangeTuneStepUp3()
        {
            tune_step_index4 = tune_step_index3 = (tune_step_index3 + 1) % console.tune_step_list.Count;
            txtWheelTune3.Text = console.tune_step_list[tune_step_index3].Name;

            //   if (console.RX1DSPMode == DSPMode.CWL || console.RX1DSPMode == DSPMode.CWL) txtWheelTune6.Text = txtWheelTune3.Text; //CW mode only
            //   else txtWheelTune8.Text = txtWheelTune3.Text; // All other modes (ie SSB)

        }

        public void ChangeTuneStepDown3()
        {
            tune_step_index4 = tune_step_index3 = (tune_step_index3 - 1 + console.tune_step_list.Count) % console.tune_step_list.Count;
            txtWheelTune3.Text = console.tune_step_list[tune_step_index3].Name;

            //   if (console.RX1DSPMode == DSPMode.CWL || console.RX1DSPMode == DSPMode.CWL) txtWheelTune6.Text = txtWheelTune3.Text; //CW mode only
            //  else txtWheelTune8.Text = txtWheelTune3.Text; // All other modes (ie SSB)

        }
        public int tune_step_index2;                        // An index into the above array
        public int tune_step_index3;                        // An index into the above array

        public int tune_step_index4;



        //===============================================================================================

        // ke9ns add
        private void txtHttpUser_MouseDown(object sender, MouseEventArgs e)
        {
            Http.terminate();
            chkBoxHTTP.Checked = false;

        }

        // ke9ns add
        private void txtHttpPass_MouseDown(object sender, MouseEventArgs e)
        {
            Http.terminate();
            chkBoxHTTP.Checked = false;
        }

        // ke9ns add
        private void udHttpPort_MouseDown(object sender, MouseEventArgs e)
        {
            Http.terminate();
            chkBoxHTTP.Checked = false;
        }

        // ke9ns add
        private void udHttpPort_ValueChanged(object sender, EventArgs e)
        {

        }

        // ke9ns add
        private void txtHttpUser_TextChanged(object sender, EventArgs e)
        {


        }

        // ke9ns add
        private void txtHttpPass_TextChanged(object sender, EventArgs e)
        {

        }

        private void chkROTOREnable_CheckedChanged(object sender, EventArgs e)
        {

            if (initializing) return;

            Debug.WriteLine("chkROTORENable=====");

            if (comboROTORPort.Text == "" || !comboROTORPort.Text.StartsWith("COM"))
            {
                if (chkROTOREnable.Focused && chkROTOREnable.Checked)
                {
                    MessageBox.Show(new Form { TopMost = true }, "The ROTOR port \"" + comboROTORPort.Text + "\" is not a valid port.\n" +
                        "Please select another port.");
                    chkROTOREnable.Checked = false;
                }
                return;
            }

            // make sure we're not using the same comm port as the bit banger 
            if (chkROTOREnable.Checked && ((console.PTTBitBangEnabled) && (comboROTORPort.Text == comboCATPTTPort.Text)
                || ((comboROTORPort.Text == comboCATPort.Text) && (chkCATEnable.Checked))))
            {
                MessageBox.Show(new Form { TopMost = true }, "ROTOR port cannot be the same as PTT or CAT port", "Port Selection Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                chkCATEnable.Checked = false;
            }

            // if enabled, disable changing of serial port 
            bool enable_sub_fields = !chkROTOREnable.Checked;

            comboROTORPort.Enabled = enable_sub_fields; // light up COM port field if ENABLE checkbox OFF

            enableCAT_HardwareFields(enable_sub_fields);

            Debug.WriteLine("chkCATEnable.Checked====" + chkCATEnable.Checked);

          //  if (chkCATEnable.Checked)  // ke9ns .181 removed
          //  {

                try
                {
                    console.ROTOREnabled = chkROTOREnable.Checked;
                }
                catch (Exception ex)
                {
                    console.ROTOREnabled = false;
                    chkROTOREnable.Checked = false;
                    MessageBox.Show(new Form { TopMost = true }, "Could not initialize ROTOR control.  Exception was:\n\n " + ex.Message +
                        "\n\nROTOR control has been disabled.", "Error Initializing ROTOR control",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
         //  }
          //  else
           // {

            //    console.ROTOREnabled = false;
          //  }


        } // chkROTORENable


        private void comboROTORPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboROTORPort.Text == "None")
            {
                if (chkROTOREnable.Checked)
                {
                    if (comboROTORPort.Focused)
                        chkROTOREnable.Checked = false;
                }

                chkROTOREnable.Enabled = false;
            }
            else chkROTOREnable.Enabled = true;

            if (comboROTORPort.Text.StartsWith("COM"))
                console.ROTORPort = Int32.Parse(comboROTORPort.Text.Substring(3));

        } // comboROTORPort_SelectedIndexChanged


        //================================================================================================
        // ke9ns add pulser
        private void tbPulseRate_MouseUp(object sender, MouseEventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbPulseRate1, "Pulse Rate: " + ((int)tbPulseRate1.Value).ToString() + " / second");

        }

        //================================================================================================
        // ke9ns add pulser
        private void tbPulseRate_MouseDown(object sender, MouseEventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbPulseRate1, "Pulse Rate: " + ((int)tbPulseRate1.Value).ToString() + " / second");

        }

        //================================================================================================
        // ke9ns add pulser
        private void tbDutyCycle_MouseUp(object sender, MouseEventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbDutyCycle, "Duty Cycle: " + ((int)tbDutyCycle.Value).ToString() + " % on time");

        }

        //================================================================================================
        // ke9ns add pulser
        private void tbDutyCycle_MouseDown(object sender, MouseEventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbDutyCycle, "Duty Cycle: " + ((int)tbDutyCycle.Value).ToString() + " % on time");

        }

        //================================================================================================
        // ke9ns add pulser
        private void tbDutyCycle_Scroll(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbDutyCycle, "Duty Cycle: " + ((int)tbDutyCycle.Value).ToString() + " % on time");

        }

        //================================================================================================
        // ke9ns add pulser
        private void tbPulseRate_Scroll(object sender, EventArgs e)
        {
            if (tbPulseRate1.Value > 20) tbPulseRate1.Value = 20;

            this.toolTip1.SetToolTip(this.tbPulseRate1, "Pulse Rate: " + ((int)tbPulseRate1.Value).ToString() + " / second");

        }

        //================================================================================================
        // ke9ns add pulser
        private void chkBoxPulser_CheckedChanged(object sender, EventArgs e)
        {
            if (tbPulseRate1.Value > 20) tbPulseRate1.Value = 20;


            if (chkBoxPulser.Checked == false)
            {
                console.chkTUN.Text = "TUN";
                console.chkFWCATU.Enabled = true;

            }
            else
            {
                console.chkTUN.Text = "TUNp";
                console.chkFWCATU.Enabled = false;
            }

        }

        //================================================================================================
        // ke9ns add pulser
        private void tbPulseRate_ValueChanged(object sender, EventArgs e)
        {
            if (tbPulseRate1.Value > 20) tbPulseRate1.Value = 20;

            Audio.PulsePeriod1 = (double)tbPulseRate1.Value;
            Audio.PulseDuty1 = (double)tbDutyCycle.Value;

        }

        //================================================================================================
        // ke9ns add pulser
        private void tbDutyCycle_ValueChanged(object sender, EventArgs e)
        {
            Audio.PulsePeriod1 = (double)tbPulseRate1.Value;
            Audio.PulseDuty1 = (double)tbDutyCycle.Value;

        }



        //==================================================================
        // ke9ns add color around the VFOA and B and Meters
        private void clrbtnVFORing_Changed(object sender, EventArgs e)
        {
            console.RingVFOColor = clrbtnVFORing.Color;

        }

        //==================================================================
        // ke9ns add chose the font for the VFO
        private void chkVFOBoldFont_CheckedChanged(object sender, EventArgs e)
        {

            if (chkVFOOpenFont.Checked == true)
            {
                console.VFOOpenFont = true;
            }
            else console.VFOOpenFont = false;

            if (chkVFOBoldFont.Checked == true)
            {
                console.VFOBoldFont = true;
            }
            else console.VFOBoldFont = false;


        } // chkVFOBoldFont_CheckedChanged


        // ke9ns add
        private void chkVFOOpenFont_CheckedChanged(object sender, EventArgs e)
        {

            if (chkVFOBoldFont.Checked == true)
            {
                console.VFOBoldFont = true;
            }
            else console.VFOBoldFont = false;

            if (chkVFOOpenFont.Checked == true)
            {
                console.VFOOpenFont = true;
            }
            else console.VFOOpenFont = false;

            if (chkVFOBoldFont.Checked == true)
            {
                console.VFOBoldFont = true;
            }
            else console.VFOBoldFont = false;


        } // chkVFOOpenFont_CheckedChanged




        //===============================================================================
        // ke9ns add 
        private void chkKeyPoll_CheckedChanged(object sender, EventArgs e)
        {
            console.CWP = chkKeyPoll.Checked;
        } // chkKeyPoll_CheckedChanged



        //=================================================================================
        // ke9ns add true = you have a light meter background and so want dark letters and dark needle with a dark shadow
        //           false = you have a dark meter background and want light letters 
        private void chkBoxLMB_CheckedChanged(object sender, EventArgs e)
        {

            console.AnalogMeterLMB = chkBoxLMB.Checked;
            if (chkBoxDMB.Checked == true)
            {
                chkBoxDMB.CheckedChanged -= chkBoxDMB_CheckedChanged;  // ke9ns turn off checkchanged temporarily    // ke9ns turn off valuechanged temporarily 
                console.AnalogMeterDMB = chkBoxDMB.Checked = false;
                chkBoxDMB.CheckedChanged += chkBoxDMB_CheckedChanged;


            }
            console.RingVFOColor = clrbtnVFORing.Color; // ke9ns add


        } // chkBoxLMB_CheckedChanged

        private void chkBoxDMB_CheckedChanged(object sender, EventArgs e)
        {

            console.AnalogMeterDMB = chkBoxDMB.Checked;

            if (chkBoxLMB.Checked == true)
            {
                chkBoxLMB.CheckedChanged -= chkBoxLMB_CheckedChanged;  // ke9ns turn off checkchanged temporarily    // ke9ns turn off valuechanged temporarily 
                console.AnalogMeterLMB = chkBoxLMB.Checked = false;
                chkBoxLMB.CheckedChanged += chkBoxLMB_CheckedChanged;

            }
            console.RingVFOColor = clrbtnVFORing.Color; // ke9ns add

        }// chkBoxDMB_CheckedChanged


        private void chkBoxPointer_CheckedChanged(object sender, EventArgs e)
        {
            console.AnalogPointer = chkBoxPointer.Checked;
            console.RingVFOColor = clrbtnVFORing.Color; // ke9ns add
        }



        //ke9ns add (wider TX waterfall id width
        private void chkTXWtrID_CheckedChanged(object sender, EventArgs e)
        {
            console.WideWaterID = chkTXWtrID.Checked;


        } // chkTXWtrID

        private void checkBoxHTTP1_CheckedChanged(object sender, EventArgs e)
        {

        }

        // ke9ns add turn on 2nd meter for TX metering
        private void chkTXMeter2_CheckedChanged(object sender, CancelEventArgs e)
        {
            console.Console_Resize(this, e);
            console.TXMeter2 = chkTXMeter2.Checked;

        }
       

        // ke9ns add to move 2nd meter up under 1st meter
        private void chk2ndMeter_CheckedChanged(object sender, EventArgs e)
        {

            console.Console_Resize(this, e);
            console.TXMeter2 = chkTXMeter2.Checked;
           
           
           

        } //  chk2ndMeter_CheckedChanged


        // ke9ns add for checking updates to PowerSDR
        private void buttonTS1_Click(object sender, EventArgs e)
        {
            // ke9ns this is the file that must appear on my web server
            /*
           <?xml version="1.0" encoding = "utf-8"?>
           <powersdr>
               <version>2.8.0.28</version>
              <url>https://ke9ns.com/flexpage.html/</url>
           </powersdr>
      */

            string downloadUrl = "";
            Version newVersion = null;
            string xmlUrl = "http://ke9ns.com/update.xml";

            XmlTextReader reader = null;

            try
            {
                Debug.WriteLine("HERE0");

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                // Use SecurityProtocolType.Ssl3 if needed for compatibility reasons
                // ServicePointManager.ServerCertificateValidationCallback += new System.Net.Security.RemoteCertificateValidationCallback((s, ce, ch, ssl) => true); // if you want to validate any ssl good or bad
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(xmlUrl);
                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();

                reader = new XmlTextReader(webResponse.GetResponseStream());
                //  reader = new XmlTextReader(xmlUrl);

                Debug.WriteLine("HERE1");

                reader.MoveToContent();

                string elementName = "";
                Debug.WriteLine("HERE2");

                if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "powersdr"))
                {
                    Debug.WriteLine("HERE3");

                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element)
                        {
                            elementName = reader.Name;
                        }
                        else
                        {
                            if ((reader.NodeType == XmlNodeType.Text) && (reader.HasValue))
                            {
                                switch (elementName)
                                {
                                    case "version":
                                        newVersion = new Version(reader.Value);
                                        break;
                                    case "url":
                                        downloadUrl = reader.Value;
                                        break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e1)
            {
                if (reader != null) reader.Close();
                MessageBox.Show(new Form { TopMost = true }, "Failed to get update information. " + e1,
                 "Update Error",
                 MessageBoxButtons.OK,
                 MessageBoxIcon.Error);

                return;
            }
            finally
            {
                if (reader != null) reader.Close();
            }


            Version appVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version; // ke9ns this is your current installed version

            if (appVersion.CompareTo(newVersion) < 0)
            {
                DialogResult dr = MessageBox.Show(
                    "Version " + newVersion.Major + "." + newVersion.Minor + "." + newVersion.Build + "." + newVersion.Revision + " of ke9ns PowerSDR is available for download, would you like to download it?",

                    "This is Your currently installed version: " + appVersion.Major + "." + appVersion.Minor + "." + appVersion.Build + "." + appVersion.Revision,
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (dr == DialogResult.No) return;
                else if (dr == DialogResult.Yes)
                {
                    // var startInfo = new ProcessStartInfo("explorer.exe", url);
                    //  System.Diagnostics.Process.Start("https://ke9ns.com/flexpage.html"); // open up my web site
                    Process myProcess = new Process();

                    try
                    {
                        // true is the default, but it is important not to set it to false
                        myProcess.StartInfo.UseShellExecute = true;
                        myProcess.StartInfo.FileName = "https://ke9ns.com/flexpage.html";
                        myProcess.Start();
                    }
                    catch (Exception eq)
                    {
                        Debug.WriteLine(eq);
                    }
                }

            }
            else
            {
                MessageBox.Show(new Form { TopMost = true }, "PowerSDR ke9ns Version: " + appVersion.Major + "." + appVersion.Minor + "." + appVersion.Build + "." + appVersion.Revision + " is up to date!",
                "No need to Update",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            }


        } // buttonTS1_Click

        //=====================================================================
        // ke9ns add
        private void chkBoxMRX_CheckedChanged(object sender, EventArgs e)
        {

        } // chkBoxMRX_CheckedChanged

        //==============================================================
        // ke9ns add
        private void chkBoxDial_CheckedChanged(object sender, EventArgs e)
        {

            console.VFODIAL = chkBoxDial.Checked;

            if (chkBoxDial.Checked == true)
            {

                console.VFODialA.Enabled = true;
                console.VFODialB.Enabled = true;
                console.VFODialA.Visible = true;
                console.VFODialB.Visible = true;

                console.VFODialA.Invalidate();
                console.VFODialB.Invalidate();

                console.VFODialAA.Enabled = true;
                console.VFODialBB.Enabled = true;
                console.VFODialAA.Visible = true;
                console.VFODialBB.Visible = true;

                console.VFODialAA.Invalidate();
                console.VFODialBB.Invalidate();
            }
            else
            {
                console.VFODialA.Enabled = false;
                console.VFODialB.Enabled = false;
                console.VFODialA.Visible = false;
                console.VFODialB.Visible = false;

                console.VFODialA.Invalidate();
                console.VFODialB.Invalidate();

                console.VFODialAA.Enabled = false;
                console.VFODialBB.Enabled = false;
                console.VFODialAA.Visible = false;
                console.VFODialBB.Visible = false;

                console.VFODialAA.Invalidate();
                console.VFODialBB.Invalidate();
            }

            console.Console_Resize(this, e);



        } // chkBoxDial_CheckedChanged


        // ke9ns add
        private void chkFMDataMic_CheckedChanged(object sender, EventArgs e)
        {

        } // chkFMDataMic_CheckedChanged


        // ke9ns add Time-Out Timer function
        public void chkBoxTOT_CheckedChanged(object sender, EventArgs e)
        {
            console.TOT_ONOFF = chkBoxTOT.Checked; // update console to let it know if you want the Time-Out Timer ON or OFF

            if (chkBoxTOT.Checked == false) textBoxTOT.Text = "OFF";


        } //  chkBoxTOT_CheckedChanged

        //ke9ns add
        private void checkBoxN1MM_CheckedChanged(object sender, EventArgs e)
        {
            console.N1MM = checkBoxN1MM.Checked; // tell console to start N1MM specturm

        } // checkBoxN1MM_CheckedChanged

        private void Setup_Deactivate(object sender, EventArgs e)
        {
            textBoxSAVE.Text = " ";
        }





        // ke9ns add  update bandtext data with button push
        private void buttonTS2_Click(object sender, EventArgs e)
        {

            console.Refresh_Tables();

        } //buttonTS2_Click

        private void chkBoxTNTX3_CheckedChanged(object sender, EventArgs e)
        {

        }

        // ke9ns add
        private void chkBoxHTTP_KeyDown(object sender, KeyEventArgs e)
        {


        }

        // ke9ns add for hang time for PTT (just like a vox hang time)
        private void chkBoxPTTHT_CheckedChanged(object sender, EventArgs e)
        {


            // if ((VACEnable || VAC2Enable) == true) chkTXVOXEnabled.Checked = false;
            //  Audio.VOXEnabled = chkTXVOXEnabled.Checked;
            console.PTTHTActive = chkBoxPTTHT.Checked;

            if (chkBoxPTTHT.Checked == false) console.PTTHT = 0;
            else console.PTTHT = (int)udPTTHT.Value;
        }

        // ke9ns add
        private void udPTTHT_ValueChanged(object sender, EventArgs e)
        {

            if (chkBoxPTTHT.Checked == false) console.PTTHT = 0;
            else console.PTTHT = (int)udPTTHT.Value;
        }

        // ke9ns add
        private void udTXDriveMax_ValueChanged(object sender, EventArgs e)
        {

            if (udTXDriveMax.Value < udTXTunePower.Value) // if TUNE exceeds max, then reduce TUNE to max
            {
                udTXTunePower.Value = udTXDriveMax.Value;
                console.ptbTune.Value = (int)udTXDriveMax.Value;
                console.lblTUNE.Text = "Tune: " + console.ptbTune.Value.ToString();
            }

            if (udTXDriveMax.Value < (decimal)console.ptbPWR.Value)// if drive exceeds max, then reduce drive to max
            {
                console.ptbPWR.Value = (int)udTXDriveMax.Value;
                console.lblPWR.Text = "Drive: " + console.ptbPWR.Value.ToString();
            }


        } // udTXDriveMax_ValueChanged

        private void chkBoxPanFillColor_CheckedChanged(object sender, EventArgs e)
        {
            Display.Gradient(Display.SpectrumGridMax, Display.SpectrumGridMin); // set new Gradient color scheme

            Display.PanFillGradient = chkBoxPanFillColor.Checked;

        } // chkBoxPanFillColor_CheckedChanged

        private void labelTS34_Click(object sender, EventArgs e)
        {

        }


        // ke9ns add
        private void tbMapBright_ValueChanged(object sender, EventArgs e)
        {

            Debug.WriteLine("mapbright ");


            console.MAPBRIGHT = (int)tbMapBright.Value;

            if (SpotForm != null) SpotForm.Darken();

            Thread.Sleep(100);

            SpotControl.Map_Last = SpotControl.Map_Last | 2;    // force update of world map


        } // tbMapBright_ValueChanged

        // ke9ns add (for automatically updating the water and pan levels
        private void chkBoxAutoWtrPan_CheckedChanged(object sender, EventArgs e)
        {
            console.AutoWaterPan(); // start up thread to auto update


        } //  chkBoxAutoWtrPan_CheckedChanged



        //===========================================================================
        //===========================================================================
        //===========================================================================
        // ke9ns add
        public void checkBoxDTMF1_MouseDown(object sender, MouseEventArgs e)
        {
            //  console.MOX = true;

            if (console.MOX == true)
            {
                Audio.MOX = true;

                Audio.SineFreq1 = 697.0;
                Audio.SineFreq2 = 1209.0;
                Audio.two_tone = true;
                Audio.TXInputSignal = Audio.SignalSource.SINE_TWO_TONE;
                Audio.SourceScale = 1.0;
            }

        } // checkboxDTMF1

        public void checkBoxDTMF1_MouseUp(object sender, MouseEventArgs e)
        {
            Audio.TXInputSignal = Audio.SignalSource.RADIO;
            Audio.SineFreq1 = (double)udDSPCWPitch.Value;
            Audio.two_tone = false;
        }


        public void checkBoxDTMF2_MouseDown(object sender, MouseEventArgs e)
        {
            //  console.MOX = true;

            if (console.MOX == true)
            {
                Audio.MOX = true;

                Audio.SineFreq1 = 697.0;
                Audio.SineFreq2 = 1336.0;
                Audio.two_tone = true;
                Audio.TXInputSignal = Audio.SignalSource.SINE_TWO_TONE;
                Audio.SourceScale = 1.0;
            }

        } // checkboxDTMF2



        public void checkBoxDTMF3_MouseDown(object sender, MouseEventArgs e)
        {
            //  console.MOX = true;

            if (console.MOX == true)
            {
                Audio.MOX = true;

                Audio.SineFreq1 = 697.0;
                Audio.SineFreq2 = 1477.0;
                Audio.two_tone = true;
                Audio.TXInputSignal = Audio.SignalSource.SINE_TWO_TONE;
                Audio.SourceScale = 1.0;
            }

        } // checkboxDTMF3

        public void checkBoxDTMF4_MouseDown(object sender, MouseEventArgs e)
        {
            //  console.MOX = true;

            if (console.MOX == true)
            {
                Audio.MOX = true;

                Audio.SineFreq1 = 770.0;
                Audio.SineFreq2 = 1209.0;
                Audio.two_tone = true;
                Audio.TXInputSignal = Audio.SignalSource.SINE_TWO_TONE;
                Audio.SourceScale = 1.0;
            }

        } // checkboxDTMF4


        public void checkBoxDTMF5_MouseDown(object sender, MouseEventArgs e)
        {
            //  console.MOX = true;

            if (console.MOX == true)
            {
                Audio.MOX = true;

                Audio.SineFreq1 = 770.0;
                Audio.SineFreq2 = 1336.0;
                Audio.two_tone = true;
                Audio.TXInputSignal = Audio.SignalSource.SINE_TWO_TONE;
                Audio.SourceScale = 1.0;
            }

        } // checkboxDTMF5


        public void checkBoxDTMF6_MouseDown(object sender, MouseEventArgs e)
        {
            //  console.MOX = true;

            if (console.MOX == true)
            {
                Audio.MOX = true;

                Audio.SineFreq1 = 770.0;
                Audio.SineFreq2 = 1477.0;
                Audio.two_tone = true;
                Audio.TXInputSignal = Audio.SignalSource.SINE_TWO_TONE;
                Audio.SourceScale = 1.0;
            }

        } // checkboxDTMF6


        public void checkBoxDTMF7_MouseDown(object sender, MouseEventArgs e)
        {
            //  console.MOX = true;

            if (console.MOX == true)
            {
                Audio.MOX = true;

                Audio.SineFreq1 = 852.0;
                Audio.SineFreq2 = 1209.0;
                Audio.two_tone = true;
                Audio.TXInputSignal = Audio.SignalSource.SINE_TWO_TONE;
                Audio.SourceScale = 1.0;
            }

        } // checkboxDTMF7


        public void checkBoxDTMF8_MouseDown(object sender, MouseEventArgs e)
        {
            //  console.MOX = true;

            if (console.MOX == true)
            {
                Audio.MOX = true;

                Audio.SineFreq1 = 852.0;
                Audio.SineFreq2 = 1336.0;
                Audio.two_tone = true;
                Audio.TXInputSignal = Audio.SignalSource.SINE_TWO_TONE;
                Audio.SourceScale = 1.0;
            }

        } // checkboxDTMF8


        public void checkBoxDTMF9_MouseDown(object sender, MouseEventArgs e)
        {
            //  console.MOX = true;

            if (console.MOX == true)
            {
                Audio.MOX = true;

                Audio.SineFreq1 = 852.0;
                Audio.SineFreq2 = 1477.0;
                Audio.two_tone = true;
                Audio.TXInputSignal = Audio.SignalSource.SINE_TWO_TONE;
                Audio.SourceScale = 1.0;
            }

        } // checkboxDTMF9


        public void checkBoxDTMF10_MouseDown(object sender, MouseEventArgs e) // * key
        {
            //  console.MOX = true;

            if (console.MOX == true)
            {
                Audio.MOX = true;

                Audio.SineFreq1 = 941.0;
                Audio.SineFreq2 = 1209.0;
                Audio.two_tone = true;
                Audio.TXInputSignal = Audio.SignalSource.SINE_TWO_TONE;
                Audio.SourceScale = 1.0;
            }

        } // checkboxDTMF10


        public void checkBoxDTMF0_MouseDown(object sender, MouseEventArgs e) // 0 key
        {
            //  console.MOX = true;

            if (console.MOX == true)
            {
                Audio.MOX = true;

                Audio.SineFreq1 = 941.0;
                Audio.SineFreq2 = 1336.0;
                Audio.two_tone = true;
                Audio.TXInputSignal = Audio.SignalSource.SINE_TWO_TONE;
                Audio.SourceScale = 1.0;
            }

        } // checkboxDTMF0


        public void checkBoxDTMF11_MouseDown(object sender, MouseEventArgs e) // # key
        {
            //  console.MOX = true;

            if (console.MOX == true)
            {
                Audio.MOX = true;

                Audio.SineFreq1 = 941.0;
                Audio.SineFreq2 = 1477.0;
                Audio.two_tone = true;
                Audio.TXInputSignal = Audio.SignalSource.SINE_TWO_TONE;
                Audio.SourceScale = 1.0;
            }

        } // checkboxDTMF11

        // ke9ns add for IIC SS AMP control
        private void chkBoxIICON_CheckedChanged(object sender, EventArgs e)
        {

            //  Band temp = console.RX1Band;
            //  console.RX1Band = temp;

            if ((chkBoxIIC.Checked == true))
            {
                console.checkBoxIICPTT.Visible = true;
                console.checkBoxIICON.Visible = true;
            }
            else
            {
                console.checkBoxIICPTT.Visible = false;
                console.checkBoxIICON.Visible = false;
            }

            console.checkBoxIICON.Checked = chkBoxIICON.Checked;

            console.IIC_AMPCONTROL(console.AMPBAND, console.AMPBAND1); // call routine to update the IIC bus


        } //chkBoxIICON_CheckedChanged

        public bool PMON = false; //.212
        private void chkBoxPwrMst_CheckedChanged(object sender, EventArgs e) // .212
        {
         

            if ((chkBoxPM1.Checked == true))
            {
                if (udPwrMstrCOM.Value != 0)
                {

                    try
                    {
                        Debug.WriteLine("POWERMASTER");
                        Debug.WriteLine("POWERMASTER0 " + udPwrMstrCOM.Value);

                        int temp = (int)udPwrMstrCOM.Value;


                        Debug.WriteLine("POWERMASTER1 " + temp);

                        console.pm = new PowerMaster(temp, chkPM2.Checked); // .212 attempt to talk to powerMaster
                        Debug.WriteLine("POWERMASTER2");

                        console.pwrMstWatts.Visible = true;
                        console.pwrMstSWR.Visible = true;


                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Error opening COM Port for Power Master",
                            "COM Port Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        console.pwrMstWatts.Visible = false;
                        console.pwrMstSWR.Visible = false;

                        return;
                    }

                } //if (udPwrMstrCOM.Value != 0)
                else
                {
                  //  chkBoxPM1.Checked = false;

                    console.pwrMstWatts.Visible = false;
                    console.pwrMstSWR.Visible = false;

                    if (PMON == true)
                    {
                        console.pm.PMClose(); // ke9ns add .212 to turn off the Powermaster

                        console.pm.Close(); // .212 turn off COM port talking to PowerMaster
                        PMON = false;
                    }

                    return;
                }

                Thread.Sleep(500);

                PMON = true;
                if (!console.pm.Present)
                {
                    MessageBox.Show("No data received from PowerMaster on COM" + udPwrMstrCOM.Value.ToString()  + ".\n" +
                        "Please check COM port and PowerMaster connections and settings.\n\n" +
                        "Make sure the PowerMaster shows \"F\" on the upper left of the display.\n" +
                        "Verify the selected COM port is correct.  Verify port in Device Manager.",
                        "No Data From PowerMaster",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    console.pwrMstWatts.Visible = false;
                    console.pwrMstSWR.Visible = false;

                    return;
                }
                
            }
            else
            {
                console.pwrMstWatts.Visible = false;
                console.pwrMstSWR.Visible = false;

                if (PMON == true)
                {
                    console.pm.PMClose(); // ke9ns add .212 to turn off the Powermaster

                    console.pm.Close(); // .212 turn off COM port talking to PowerMaster
                    PMON = false;
                }


            } // false


        } //chkBoxPwrMst_CheckedChanged


        private void udPwrMstrCOM_ValueChanged(object sender, EventArgs e)
        {
            if ((chkBoxPM1.Checked == true))
            {
                if (udPwrMstrCOM.Value != 0)
                {

                    try
                    {
                        Debug.WriteLine("POWERMASTER4");
                        Debug.WriteLine("POWERMASTER5 " + udPwrMstrCOM.Value);

                        int temp = (int)udPwrMstrCOM.Value;


                        Debug.WriteLine("POWERMASTER6 " + temp);

                        console.pm = new PowerMaster(temp, chkPM2.Checked); // .212 attempt to talk to powerMaster
                        Debug.WriteLine("POWERMASTER7");

                        console.pwrMstWatts.Visible = true;
                        console.pwrMstSWR.Visible = true;


                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Error opening COM Port for PowerMaster: " + (int)udPwrMstrCOM.Value,
                            "COM Port Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        console.pwrMstWatts.Visible = false;
                        console.pwrMstSWR.Visible = false;

                        return;
                    }

                } //if (udPwrMstrCOM.Value != 0)
                else
                {
                    //  chkBoxPM1.Checked = false;

                    console.pwrMstWatts.Visible = false;
                    console.pwrMstSWR.Visible = false;

                    if (PMON == true)
                    {
                        console.pm.PMClose(); // ke9ns add .212 to turn off the Powermaster

                        console.pm.Close(); // .212 turn off COM port talking to PowerMaster
                        PMON = false;
                    }

                    return;
                }

                Thread.Sleep(500);

                PMON = true;
                if (!console.pm.Present)
                {
                    MessageBox.Show("No data received from PowerMaster on COM" + udPwrMstrCOM.Value.ToString() + ".\n" +
                        "Please check COM port and PowerMaster connections and settings.\n\n" +
                        "Make sure the PowerMaster shows \"F\" on the upper left of the display.\n" +
                        "Verify the selected COM port is correct.  Verify port in Device Manager.",
                        "No Data From PowerMaster",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    console.pwrMstWatts.Visible = false;
                    console.pwrMstSWR.Visible = false;

                    return;
                }

            }
            else
            {
                console.pwrMstWatts.Visible = false;
                console.pwrMstSWR.Visible = false;

                if (PMON == true)
                {
                    console.pm.PMClose(); // ke9ns add .212 to turn off the Powermaster

                    console.pm.Close(); // .212 turn off COM port talking to PowerMaster
                    PMON = false;
                }


            } // false

        } // udPwrMstrCOM_ValueChanged





        private void chkBoxIIC_CheckedChanged(object sender, EventArgs e)
        {
            if ((chkBoxIIC.Checked == true))
            {
                console.checkBoxIICPTT.Visible = true;
                console.checkBoxIICON.Visible = true;
            }
            else
            {
                console.checkBoxIICPTT.Visible = false;
                console.checkBoxIICON.Visible = false;
            }

            console.IIC_AMPCONTROL(console.AMPBAND, console.AMPBAND1); // call routine to update the IIC bus
        }

        private void tbMapBright_Scroll(object sender, EventArgs e)
        {

        }

        // ke9ns add  Auto start up PowerSDR upon launch of PowerSDR
        private void chkBoxAutoStart_CheckedChanged(object sender, EventArgs e)
        {


        } // chkBoxAutoStart_CheckedChanged

        // ke9ns add
        private void chkCWDisplay_CheckedChanged(object sender, EventArgs e)
        {
            if (console.cwxForm != null)
            {
                console.cwxForm.checkBoxCWD.Checked = chkCWDisplay.Checked;

            }
        }

        private void ChkAlwaysOnTop1_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = chkAlwaysOnTop1.Checked;
        }

        private void Setup_MouseEnter(object sender, EventArgs e)
        {
            if (chkBoxAutoFocus.Checked == true && chkAlwaysOnTop1.Checked == true) this.Activate();
        }

        //==========================================================================================================
        // ke9ns add: to go from 4096 points to 16384 points on the pan and waterfall (much higher resolution)
        public void ChkSpectrumHiRes_CheckedChanged(object sender, EventArgs e)
        {

            if (console.PowerOn == true)
            {
                console.PowerOn = false;
                Thread.Sleep(500);
            }

            if (chkSpectrumHiRes.Checked == true)
            {

                Display.BUFFER_SIZE = 16384;
                Display.DATA_BUFFER_SIZE = 16384;
                DSP.SyncHiRes(16384);
                Display.Init();  // update buffers in display

            }
            else
            {

                DSP.SyncHiRes(4096);         // ke9ns: 
                Display.BUFFER_SIZE = 4096;
                Display.DATA_BUFFER_SIZE = 4096;
                Display.Init();

            }

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    DSPRX dsp_rx = console.dsp.GetDSPRX(i, j);
                    dsp_rx.Update = false;
                    dsp_rx.Force = true;
                    dsp_rx.Update = true;
                    dsp_rx.Force = false;
                }
            }

            for (int i = 0; i < 1; i++)
            {
                DSPTX dsp_tx = console.dsp.GetDSPTX(i);
                dsp_tx.Update = false;
                dsp_tx.Force = true;
                dsp_tx.Update = true;
                dsp_tx.Force = false;
            }




        } // ChkSpectrumHiRes_CheckedChanged

        // ke9ns add
        private void ChkBoxPTT_CheckedChanged(object sender, EventArgs e)
        {
            chkBoxPTTLatch.CheckedChanged -= ChkBoxPTTLatch_CheckedChanged; // turn off (allows only 1 of the 2 checkboxes to be selected)
            if (chkBoxPTTLatch.Checked == true) chkBoxPTTLatch.Checked = false;
            chkBoxPTTLatch.CheckedChanged += ChkBoxPTTLatch_CheckedChanged; // turn on



        }


        // ke9ns add
        private void ChkBoxPTTLatch_CheckedChanged(object sender, EventArgs e)
        {
            chkBoxPTT.CheckedChanged -= ChkBoxPTT_CheckedChanged; // turn off (allows only 1 of the 2 checkboxes to be selected)
            if (chkBoxPTT.Checked == true) chkBoxPTT.Checked = false;
            chkBoxPTT.CheckedChanged += ChkBoxPTT_CheckedChanged; // turn on


        }

        // ke9ns add: for AGC-T green line on pan  
        private void tbAGCTadj_Scroll(object sender, EventArgs e)
        {
            Display.AGCT_Adj_Ratio = (double)tbAGCTadj.Value / (double)1000.00;  // default is 1.6155000
            console.AGCTUPDATE = true; // force green line AGCT update



        }


        //==================================================================
        // ke9ns add
        private void txtWheelTune2_TextChanged(object sender, EventArgs e)
        {
            if (console.initializing == false) // dont update if initializing
            {

                tune_step_index4 = tune_step_index2 = console.TuneStepLookup(txtWheelTune2.Text);

                tune_step_index2--;   // ke9ns add Powermate and Flexcontrol knob alt tuning #1
                ChangeTuneStepUp2(); // ke9ns add


                tune_step_index4 = tune_step_index2;


            }


            grpBoxTS1.Invalidate();
        }

        private void txtWheelTune3_TextChanged(object sender, EventArgs e)
        {

            if (console.initializing == false) // dont update if initializing
            {

                tune_step_index4 = tune_step_index3 = console.TuneStepLookup(txtWheelTune3.Text);


                tune_step_index3--;   // ke9ns addadd Powermate  knob alt tuning #2
                ChangeTuneStepUp3(); // ke9ns add

                tune_step_index4 = tune_step_index2;

            }

            grpBoxTS1.Invalidate();
        }

        // ke9ns add: 3D pan Major color 
        private void clrbtn3DDataLine_Changed(object sender, EventArgs e)
        {
            Display.Data3DLineColor = clrbtn3DDataLine.Color;
        }

        private void tbPan3DAlpha_Scroll(object sender, EventArgs e)
        {
            Display.Data3DLineAlpha = tbPan3DAlpha.Value;
            Display.Data3DLineColor = clrbtn3DDataLine.Color;

        }

        // ke9ns add: move meters to top line
        public void chkBoxMeterTop_CheckedChanged(object sender, EventArgs e)
        {
            console.MeterTop = chkBoxMeterTop.Checked;

            if (chkBoxMeterTop.Checked)
            {
                console.panelBandHFRX2.Visible = true;
                console.panelBandVHFRX2.Visible = true;
                console.panelBandGNRX2.Visible = true;

            }
            else
            {
                console.panelBandHFRX2.Visible = false;
                console.panelBandVHFRX2.Visible = false;
                console.panelBandGNRX2.Visible = false;
                
            }


            console.Console_Resize(this, e);

            //  console.TXMeter2 = chkTXMeter2.Checked;
        }

        // ke9ns add
        private void checkBoxMixAudio_CheckedChanged(object sender, EventArgs e)
        {
            chkVACAllowBypass.Checked = false; // ke9ns add: disable PTT bypass for audio MIXER

        } // checkBoxMixAudio_CheckedChanged


        // ke9ns add: .153
        public void chkBoxMeterMenus_CheckedChanged(object sender, EventArgs e)
        {

            console.MeterMenu = chkBoxMeterMenus.Checked;

            console.Console_Resize(this, e);


        } //  chkBoxMeterMenus_CheckedChanged

        //ke9ns add
        private void buttonExit_Click(object sender, EventArgs e)
        {
            console.TurnOffVOA();


            textBoxSAVE.Text = " ";

            ApplyOptions(); // ke9ns add .193a (wait until save is updated before closing everything


            console.Close();

        } // buttonExit_Click

        // ke9ns add: to disable Title Bar (top line) of console
        private void chkBoxTitle_CheckedChanged_1(object sender, EventArgs e)
        {
            if (chkBoxTitle.Checked == true)
            {
                //  console.FormBorderStyle = FormBorderStyle.None;

                console.FormBorderStyle = FormBorderStyle.None;
                chkBoxConsoleRing.Enabled = true; // enable ring with top line off
                console.labelPowerSDR.Visible = true;
                console.labelPowerSDR.Visible = true;
                console.labelMove.Visible = true;
                console.labelSize.Visible = true;
                console.labelMax.Visible = true;
            }
            else
            {
                chkBoxConsoleRing.Checked = false; // turn off ring if you have a title bard
                chkBoxConsoleRing.Enabled = false;
                console.labelPowerSDR.Visible = false;
                console.labelMove.Visible = false;
                console.labelSize.Visible = false;
                console.labelMax.Visible = false;
                console.FormBorderStyle = FormBorderStyle.Sizable;
            }

        }

        // ke9ns add: 
        private void chkBoxConsoleRing_Click(object sender, EventArgs e)
        {

            console.Invalidate();

        }

        // ke9ns add: maximize console 
        private void chkBoxMax_CheckedChanged(object sender, EventArgs e)
        {
            if (!initializing)
            {
                if (chkBoxMax.Checked == true)
                {
                   // Debug.WriteLine("Setup_maximize ");
                    console.WindowState = FormWindowState.Maximized;
                }
                else
                {
                   // Debug.WriteLine("Setup_normal ");
                    console.WindowState = FormWindowState.Normal;
                }
            }
        }

        private void tbGrayLineAlpha_ValueChanged(object sender, EventArgs e)
        {
            Debug.WriteLine("mapbright1 ");


            console.MAPBRIGHT1 = (int)tbGrayLineBright.Value;

            if (SpotForm != null) SpotForm.Darken();

            Thread.Sleep(100);

            SpotControl.Map_Last = SpotControl.Map_Last | 2;    // force update of world map


        }

        private void checkBoxTS1_CheckedChanged_1(object sender, EventArgs e)
        {

        }

        bool CATSOCKET = false; // ke9ns

        // ke9ns add: for URL CAT support   private CATParser parser;
        private void chkCatURLON_CheckedChanged(object sender, EventArgs e)
        {

            try
            {
                txtCatURL.Text = GetLocalIPAddress();
            }
            catch (Exception e2)
            {
                txtCatURL.Text = "no IP detected";
            }

            try
            {
                txtCatURL2.Text = GetLocalIPAddress2();
            }
            catch (Exception e2)
            {
                txtCatURL2.Text = "no IP detected";
                chkCatURLALT.Checked = false;
            }

           

            if (chkCatURLON.Checked)
            {
                if (CONNECTSTREAM == false)
                {
                    CatURL(); // for receiving from CAT device
                  
                }
            }
            else
            {
               
               

            }



        } // chkCatURLON_CheckedChanged



        //============================================
        // ke9ns add:

        String URL1 = "192.168.0.102";
        String Port1 = "219";

        void CatURL()
        {

            Debug.WriteLine("CAT URL NOW ON");

            if (Restart == false)   console.helpboxForm.helpbox_message.Text = "CAT TCP Server activated. No connection";
            else
            {
                Restart = false;
                console.helpboxForm.helpbox_message.Text += "\r\n\r\nCAT TCP Server activated. No connection";
            }

            if (chkCatURLALT.Checked) URL1 = txtCatURL2.Text; // .212
            else URL1 = txtCatURL.Text;


            Port1 = txtCatPort.Text;

            //  HttpWebRequest webReq = (HttpWebRequest)HttpWebRequest.Create("http://"+ URL1 + ":" + Port1)

            if (DATACONNECT == false && CONNECTSTREAM == false)
            {
                Thread t4 = new Thread(new ThreadStart(CATURL)); // use: RX3 = parser.Get(RX2); for CAT command requests
                t4.IsBackground = true;
                t4.Priority = ThreadPriority.Normal;
                t4.Name = "CAT TCP IP SOCKET THREAD RX";
                t4.Start();

             //   Thread t5 = new Thread(new ThreadStart(CATURL_TX)); // use: RX3 = parser.Get(RX2); for CAT command requests
              //  t5.IsBackground = true;
              //  t5.Priority = ThreadPriority.Normal;
              //  t5.Name = "CAT TCP IP SOCKET THREAD TX";
              //  t5.Start();


            }



        } // CATURL()

        //============================================
        // ke9ns add: thread for CAT URL socket

        public bool CONNECTSTREAM = false;
        public bool DATACONNECT = false;
        public static TcpClient client;
        public static TcpListener client1;

        public static NetworkStream networkStream;
        public static NetworkStream networkStream1;
        public static String RX2; // ke9ns cat data sent to PowerSDR
        public static String RX3; // ke9ns: cat data sent from PowerSDR
      
        public static byte cnt = 0;
        public static byte[] msg = new byte[50];

        public bool Restart = false; // true = forced disconnect, so redo after the thread ends

        void CATURL() // thread
        {
            PASS:
            
            Debug.WriteLine("CAT thread start now " + CONNECTSTREAM);
            radioButtonCAT.Checked = false;
            DATACONNECT = false;

            if (CONNECTSTREAM == false)
            {
                Restart = false;

                Debug.WriteLine("CAT try to open data port");
                console.helpboxForm.helpbox_message.Text += "\r\n>>Try to Start CAT TCP Server..";

                

                try
                {
                  
                    console.helpboxForm.helpbox_message.Text += "\r\n>>WAIT for Initial connection.";
                   
                    client1 = new TcpListener(IPAddress.Any, Convert.ToInt16(Port1));
             
                    CONNECTSTREAM = true;
                    client1.Start();        // WAITS FOR A CONNECTION HERE


                    client = client1.AcceptTcpClient();

                  
                    networkStream = client.GetStream();

                   
                    console.helpboxForm.helpbox_message.Text += "\r\n>>CAT TCP Server STARTED on Port: " + Port1;

                    Debug.WriteLine("CAT Data port OPEN: " + Port1);

                    DATACONNECT = true;


                }
                catch (Exception e)
                {
                    CONNECTSTREAM = false;
                    Debug.WriteLine("CAT URL FAULT: " + e);

                    console.helpboxForm.helpbox_message.Text += "\r\n>>CAT Server startup failure: " + e;

                    chkCatURLON.Checked = false;
                    Restart = true;
                    
                    client1.Stop();
                    client.Close();
                    DATACONNECT = false;
                    Array.Clear(msg, 0, 50);
                }

            } // if (CONNECTSTREAM == false)

            for (; ; )
            {
                radioButtonCAT.Checked = false;
                if (client.Connected ) break;
                if (chkCatURLON.Checked == false)
                {
                  
                    radioButtonCAT.Checked = false;
                    client.Close();
                    client1.Stop();
                    Array.Clear(msg, 0, 50);
                    CONNECTSTREAM = false;
                    DATACONNECT = false;
                   
                    break;
                }
            }

            if (CONNECTSTREAM == true)
            {

                Debug.WriteLine("CAT URL LOOP");
                byte[] test = new byte[50];

                test = Encoding.ASCII.GetBytes("PowerSDR ke9ns v2.8.0 TCP Cat Connection started...\r\n");
                networkStream.Write(test, 0, test.Length);

                console.helpboxForm.helpbox_message.Text += "\r\n" + Encoding.ASCII.GetString(test);

                cnt = 0;


                for (; ; )
                {
                    Thread.Sleep(70);
                 
                    Debug.WriteLine("CAT URL WAIT FOR CHARACTER" );
                   
                    try
                    {

                        if (chkCatURLON.Checked == false)
                        {
                            console.helpboxForm.helpbox_message.Text += "\r\n>>You turned OFF CAT TCP Server.";
                            break; // end if you turn it off
                        }
                       
                        if (client.Connected == false)  // end if connection is lost
                        {
                            console.helpboxForm.helpbox_message.Text += "\r\n>>External CAT TCP Connection Dropped.";

                            radioButtonCAT.Checked = false;
                            client.Close();
                            client1.Stop();
                            Array.Clear(msg, 0, 50);

                            CONNECTSTREAM = false; // .201

                            goto PASS;

                        }

                        radioButtonCAT.Checked = true;

                       networkStream.Read(msg, cnt++, 1); // wait for data incoming to TCP URL socket

                  
                       

                            if (cnt > 48)
                            {
                                console.helpboxForm.helpbox_message.Text += "\r\n>>Large number of 0 bytes received by PowerSDR.";

                                if (msg[cnt - 1] == 0)  // ke9ns: end this is receiving nulls
                                {
                                    console.helpboxForm.helpbox_message.Text += "\r\n>>Shutting Down CAT TCP Server due to 0 bytes.";

                                    radioButtonCAT.Checked = false;
                                    client.Close();
                                    client1.Stop();

                                    Array.Clear(msg, 0, 50);
                                    CONNECTSTREAM = false; // .201

                                    //  Debug.WriteLine("CONNECTSTREAM " + CONNECTSTREAM);
                                    goto PASS;
                                }
                            }

                            //  Debug.WriteLine("CAT got char " + Convert.ToByte(msg[4]));

                            RX2 = Encoding.Default.GetString(msg, 0, cnt); // TCP/IP RECEIVED DATA into RX2 string to send to PowerSDR for CAT decoding

                            // Debug.WriteLine("CAT RX2: " + RX2);
                            if (RX2.Contains("\n") && cnt < 3)
                            {
                                cnt = 0;
                                Array.Clear(msg, 0, 50); // clear any extra <CR>
                            }
                            else if (RX2.Contains("\r\n") && cnt < 3)
                            {
                                cnt = 0;
                                Array.Clear(msg, 0, 50); // clear any extra <CR>
                            }
                            else if (RX2.Contains("\r") && (cnt < 3))
                            {
                                cnt = 0;
                                Array.Clear(msg, 0, 50); // clear any extra <CR>

                            }
                            else if (RX2.Contains("\r") && (cnt > 2))
                            {
                                console.helpboxForm.helpbox_message.Text += "\r\n>>Command ReceiveD: " + RX2;

                                Debug.WriteLine("got CR instead of end character " + cnt + " length: " + msg.Length);
                                if (cnt > 1)
                                {
                                    msg[cnt - 1] = 59; // this is a ';'
                                    msg[cnt] = 0;
                                    msg[cnt + 1] = 0;
                                }

                                RX2 = Encoding.Default.GetString(msg, 0, cnt); // TCP/IP RECEIVED DATA into RX2 string (like ZZAU;) to send to PowerSDR for CAT decoding
                                RX3 = parser.Get(RX2); // PowerSDR CAT parser to string RX3 to send reponse back to TCP/IP CAT device 

                                console.helpboxForm.helpbox_message.Text += "\r\n>>PowerSDR sent: " + RX3;

                                test = Encoding.Default.GetBytes(RX3); // convert CAT answer to byte array in order to send back via TCP/IP

                                networkStream.Write(test, 0, test.Length); // send PowerSDR CAT response back across the TCP URL socket

                                cnt = 0;
                                Array.Clear(msg, 0, 50);
                            }
                            else if (RX2.Contains(";") == true)
                            {
                                Debug.WriteLine("got end char");

                                console.helpboxForm.helpbox_message.Text += "\r\n>>Command Received: " + RX2;

                                RX3 = parser.Get(RX2); // send TCP Socket CAT COMMAND to parser to get an answer from PowerSDR as RX3

                                console.helpboxForm.helpbox_message.Text += "\r\n>>PowerSDR sent: " + RX3;

                                test = Encoding.Default.GetBytes(RX3);

                                networkStream.Write(test, 0, test.Length); // send PowerSDR CAT response back across the TCP URL socket
                                cnt = 0;
                                Array.Clear(msg, 0, 50);
                            }



                            if (cnt > 48)
                            {
                                console.helpboxForm.helpbox_message.Text += "\r\n>>Too long of command";

                                cnt = 0;
                                Array.Clear(msg, 0, 50);
                            }
                        
                       
                         //   if (console.KWAI7 && console.setupForm.AllowFreqBroadcast7)
                          //  {
                           //     test = Encoding.Default.GetBytes(console.CATURLFREQ);
                           //     networkStream.Write(test, 0, test.Length); // send PowerSDR CAT response back across the TCP URL socket
                           //     console.KWAI7 = false;
                          //  }
                            
                        

                    }
                    catch (Exception e)
                    {
                        console.helpboxForm.helpbox_message.Text += "\r\n>>FAILURE: " + e;
                        chkCatURLON.Checked = false;
                      
                        Debug.WriteLine("CAT CONNECTION FAULT: " + e);
                                            

                        client.Close();
                        client1.Stop();
                        Array.Clear(msg, 0, 50);

                        radioButtonCAT.Checked = false;
                        Restart = true; // someone killed the connection instead of closing it.
                      
                        break;
                    } // catch


                } // FOR loop

                console.helpboxForm.helpbox_message.Text += "\r\n>>CAT Server Thread stopped";

                Debug.WriteLine("CAT URL THREAD DONE");

                client.Close();
                client1.Stop();
                Array.Clear(msg, 0, 50);
                 CONNECTSTREAM = false;
                DATACONNECT = false;
                radioButtonCAT.Checked = false;


            } //  if (CONNECTSTREAM == true)


            if (Restart == true) chkCatURLON.Checked = true;
           

        } // CATURL thread


     



















        public static string GetLocalIPAddress()
        {


            StringBuilder sb = new StringBuilder();
            String strHostName = string.Empty;
            strHostName = Dns.GetHostName();

            Debug.WriteLine("HOST: " + strHostName);

            string myIP = Dns.GetHostByName(strHostName).AddressList[0].ToString();

            return myIP;


           
        }

        public static string GetLocalIPAddress2()
        {


            StringBuilder sb = new StringBuilder();
            String strHostName = string.Empty;
            strHostName = Dns.GetHostName();

            Debug.WriteLine("HOST: " + strHostName);

            string myIP = Dns.GetHostByName(strHostName).AddressList[1].ToString();

            return myIP;



        }
        private void tpCAT_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                txtCatURL.Text = GetLocalIPAddress();
            }
            catch (Exception e2)
            {
                txtCatURL.Text = "no IP detected";
            }
            try
            {
                txtCatURL2.Text = GetLocalIPAddress2();
            }
            catch (Exception e2)
            {
                txtCatURL2.Text = "no IP detected";
                chkCatURLALT.Checked = false;
            }



        }

        private void txtCatPort_TextChanged(object sender, EventArgs e)
        {

            try
            {
                txtCatURL.Text = GetLocalIPAddress();
            }
            catch (Exception e2)
            {
                txtCatURL.Text = "no IP detected";
            }

            try
            {
                txtCatURL2.Text = GetLocalIPAddress2();
            }
            catch (Exception e2)
            {
                txtCatURL2.Text = "no IP detected";
                chkCatURLALT.Checked = false;
            }

            if (chkCatURLON.Checked == true) Restart = true;

            chkCatURLON.Checked = false;
            CONNECTSTREAM = false;
            DATACONNECT = false;

           

        } //txtCatPort_TextChanged

        // ke9ns add: check for right click to open text window
        private void chkCatURLON_MouseDown(object sender, MouseEventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;

            if (me.Button == System.Windows.Forms.MouseButtons.Right)
            {

                if (console.helpboxForm == null || console.helpboxForm.IsDisposed) console.helpboxForm = new helpbox(console);

                console.helpboxForm.Text = "PowerSDR CAT TCP Server.";
                console.helpboxForm.Show();
                console.helpboxForm.Focus();
                console.helpboxForm.WindowState = FormWindowState.Normal; // ke9ns add


            }


            } // chkCatURLON_MouseDown

        private void labelTS56_Click(object sender, EventArgs e)
        {

        }

        // ke9ns add .190
        private void chkQuindarStart_CheckedChanged(object sender, EventArgs e)
        {
            console.Quindar_Start = chkQuindarStart.Checked;


        } // chkQuindarStart_CheckedChanged

        // ke9ns add .190
        private void chkQuindarEnd_CheckedChanged(object sender, EventArgs e)
        {
                console.Quindar_End = chkQuindarEnd.Checked;



        } // chkQuindarEnd_CheckedChanged

        // ke9ns add .190
        private void udQuindarTonesVol_ValueChanged(object sender, EventArgs e)
        {
           
         //   Audio.WavePreamp = Math.Pow(10.0, (int)udQuindarTonesVol.Value / 20.0); // convert to scalar
          //  Audio.WavePreamp = Math.Pow(10.0, (int)udPreamp.Value / 20.0); // convert to scalar

        } // udQuindarTonesVol_ValueChanged

        private void checkBoxRX2_CheckedChanged(object sender, EventArgs e) //.219
        {
           console.N1MM_RX2 = checkBoxRX2.Checked;

        } //checkBoxRX2_CheckedChanged

        private void chkVFOLargeWindow_CheckedChanged(object sender, EventArgs e) // .228
        {

            // ke9ns: .228 must resize console when size of grpVFO changes:   Console_Resize(this, EventArgs.Empty);
            console.Invalidate();
            console.Console_Resize(this, EventArgs.Empty);

        }

































        //setupForm.gridBoxTS.CheckedChanged -= setupForm.gridBoxTS_CheckedChanged;  // ke9ns turn off checkchanged temporarily   
        // 
        // setupForm.gridBoxTS.CheckedChanged += setupForm.gridBoxTS_CheckedChanged;





        // ke9ns add
        //    private void chkPHROTEnable_CheckedChanged(object sender, EventArgs e)
        //   {
        //  int run;
        //  if (chkPHROTEnable.Checked) run = 1;
        //  else run = 0;
        //  wdsp.SetTXAPHROTRun(wdsp.id(1, 0), run);
        //   }





        //===============================================================================



    } // class setup



    #region PADeviceInfo Helper Class

    public class PADeviceInfo
    {
        private string _Name;
        private int _Index;

        public string Name
        {
            get { return _Name; }
        }

        public int Index
        {
            get { return _Index; }
        }

        public PADeviceInfo(String argName, int argIndex)
        {
            _Name = argName;
            _Index = argIndex;
        }

        public override string ToString()
        {
            return _Name;
        }
    }

    #endregion
}
