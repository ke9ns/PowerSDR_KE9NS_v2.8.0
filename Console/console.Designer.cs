
//=================================================================
// console.cs
//=================================================================
// PowerSDR is a C# implementation of a Software Defined Radio. 
// Copyright (C) 2003-2013  FlexRadio Systems.  
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful
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

    sealed unsafe public partial class Console : System.Windows.Forms.Form
    {
        

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Console));
            this.timer_cpu_meter = new System.Windows.Forms.Timer(this.components);
            this.timer_peak_text = new System.Windows.Forms.Timer(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.labelPowerSDR = new System.Windows.Forms.Label();
            this.labelMove = new System.Windows.Forms.Label();
            this.labelSize = new System.Windows.Forms.Label();
            this.labelMax = new System.Windows.Forms.Label();
            this.pwrMstWatts = new System.Windows.Forms.TextBox();
            this.pwrMstSWR = new System.Windows.Forms.TextBox();
            this.btnBandVHFRX2 = new System.Windows.Forms.ButtonTS();
            this.radBandGENRX2 = new System.Windows.Forms.RadioButtonTS();
            this.radBandWWVRX2 = new System.Windows.Forms.RadioButtonTS();
            this.radBand2RX2 = new System.Windows.Forms.RadioButtonTS();
            this.radBand6RX2 = new System.Windows.Forms.RadioButtonTS();
            this.radBand10RX2 = new System.Windows.Forms.RadioButtonTS();
            this.radBand12RX2 = new System.Windows.Forms.RadioButtonTS();
            this.radBand15RX2 = new System.Windows.Forms.RadioButtonTS();
            this.radBand17RX2 = new System.Windows.Forms.RadioButtonTS();
            this.radBand20RX2 = new System.Windows.Forms.RadioButtonTS();
            this.radBand30RX2 = new System.Windows.Forms.RadioButtonTS();
            this.radBand40RX2 = new System.Windows.Forms.RadioButtonTS();
            this.radBand60RX2 = new System.Windows.Forms.RadioButtonTS();
            this.radBand160RX2 = new System.Windows.Forms.RadioButtonTS();
            this.radBand80RX2 = new System.Windows.Forms.RadioButtonTS();
            this.picRX3Meter = new System.Windows.Forms.PictureBox();
            this.picRX2Meter = new System.Windows.Forms.PictureBox();
            this.comboMeterTX1Mode = new System.Windows.Forms.ComboBoxTS();
            this.comboRX2MeterMode = new System.Windows.Forms.ComboBoxTS();
            this.radBandGN13RX2 = new System.Windows.Forms.RadioButtonTS();
            this.radBandGN12RX2 = new System.Windows.Forms.RadioButtonTS();
            this.radBandGN11RX2 = new System.Windows.Forms.RadioButtonTS();
            this.radBandGN10RX2 = new System.Windows.Forms.RadioButtonTS();
            this.radBandGN9RX2 = new System.Windows.Forms.RadioButtonTS();
            this.radBandGN8RX2 = new System.Windows.Forms.RadioButtonTS();
            this.radBandGN7RX2 = new System.Windows.Forms.RadioButtonTS();
            this.radBandGN6RX2 = new System.Windows.Forms.RadioButtonTS();
            this.radBandGN5RX2 = new System.Windows.Forms.RadioButtonTS();
            this.radBandGN4RX2 = new System.Windows.Forms.RadioButtonTS();
            this.radBandGN3RX2 = new System.Windows.Forms.RadioButtonTS();
            this.radBandGN2RX2 = new System.Windows.Forms.RadioButtonTS();
            this.radBandGN1RX2 = new System.Windows.Forms.RadioButtonTS();
            this.radBandGN0RX2 = new System.Windows.Forms.RadioButtonTS();
            this.comboMeterTXMode = new System.Windows.Forms.ComboBoxTS();
            this.picMultiMeterDigital = new System.Windows.Forms.PictureBox();
            this.comboMeterRXMode = new System.Windows.Forms.ComboBoxTS();
            this.txtMultiText = new System.Windows.Forms.TextBoxTS();
            this.radBandGEN = new System.Windows.Forms.RadioButtonTS();
            this.radBandWWV = new System.Windows.Forms.RadioButtonTS();
            this.radBand2 = new System.Windows.Forms.RadioButtonTS();
            this.radBand6 = new System.Windows.Forms.RadioButtonTS();
            this.radBand10 = new System.Windows.Forms.RadioButtonTS();
            this.radBand12 = new System.Windows.Forms.RadioButtonTS();
            this.radBand15 = new System.Windows.Forms.RadioButtonTS();
            this.radBand17 = new System.Windows.Forms.RadioButtonTS();
            this.radBand20 = new System.Windows.Forms.RadioButtonTS();
            this.radBand30 = new System.Windows.Forms.RadioButtonTS();
            this.radBand40 = new System.Windows.Forms.RadioButtonTS();
            this.radBand60 = new System.Windows.Forms.RadioButtonTS();
            this.radBand160 = new System.Windows.Forms.RadioButtonTS();
            this.radBand80 = new System.Windows.Forms.RadioButtonTS();
            this.btnBandVHF = new System.Windows.Forms.ButtonTS();
            this.chkTXEQ1 = new System.Windows.Forms.CheckBoxTS();
            this.chkRXEQ1 = new System.Windows.Forms.CheckBoxTS();
            this.udFM1750Timer = new System.Windows.Forms.NumericUpDownTS();
            this.chkFM1750 = new System.Windows.Forms.CheckBoxTS();
            this.chkFMTXLow = new System.Windows.Forms.CheckBoxTS();
            this.btnFMMemory = new System.Windows.Forms.ButtonTS();
            this.btnFMMemoryUp = new System.Windows.Forms.ButtonTS();
            this.btnFMMemoryDown = new System.Windows.Forms.ButtonTS();
            this.radFMDeviation2kHz = new System.Windows.Forms.RadioButtonTS();
            this.udFMOffset = new System.Windows.Forms.NumericUpDownTS();
            this.chkFMTXRev = new System.Windows.Forms.CheckBoxTS();
            this.radFMDeviation5kHz = new System.Windows.Forms.RadioButtonTS();
            this.comboFMCTCSS = new System.Windows.Forms.ComboBoxTS();
            this.chkFMCTCSS = new System.Windows.Forms.CheckBoxTS();
            this.chkFMTXSimplex = new System.Windows.Forms.CheckBoxTS();
            this.chkFMTXHigh = new System.Windows.Forms.CheckBoxTS();
            this.comboFMTXProfile = new System.Windows.Forms.ComboBoxTS();
            this.checkBoxIICPTT = new System.Windows.Forms.CheckBoxTS();
            this.checkBoxIICON = new System.Windows.Forms.CheckBoxTS();
            this.chkVAC2 = new System.Windows.Forms.CheckBoxTS();
            this.btnZeroBeat = new System.Windows.Forms.ButtonTS();
            this.chkVFOSplit = new System.Windows.Forms.CheckBoxTS();
            this.btnRITReset = new System.Windows.Forms.ButtonTS();
            this.btnXITReset = new System.Windows.Forms.ButtonTS();
            this.udRIT = new System.Windows.Forms.NumericUpDownTS();
            this.btnIFtoVFO = new System.Windows.Forms.ButtonTS();
            this.chkRIT = new System.Windows.Forms.CheckBoxTS();
            this.btnVFOSwap = new System.Windows.Forms.ButtonTS();
            this.chkXIT = new System.Windows.Forms.CheckBoxTS();
            this.btnVFOBtoA = new System.Windows.Forms.ButtonTS();
            this.udXIT = new System.Windows.Forms.NumericUpDownTS();
            this.btnVFOAtoB = new System.Windows.Forms.ButtonTS();
            this.chkVAC1 = new System.Windows.Forms.CheckBoxTS();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.richTextBox3 = new System.Windows.Forms.RichTextBox();
            this.richTextBox5 = new System.Windows.Forms.RichTextBox();
            this.richTextBox6 = new System.Windows.Forms.RichTextBox();
            this.richTextBox7 = new System.Windows.Forms.RichTextBox();
            this.richTextBox8 = new System.Windows.Forms.RichTextBox();
            this.buttonbs = new System.Windows.Forms.ButtonTS();
            this.chkBoxBS = new System.Windows.Forms.CheckBoxTS();
            this.labelTS5 = new System.Windows.Forms.LabelTS();
            this.regBox1 = new System.Windows.Forms.TextBoxTS();
            this.regBox = new System.Windows.Forms.TextBoxTS();
            this.lblTuneStep = new System.Windows.Forms.LabelTS();
            this.chkVFOSync = new System.Windows.Forms.CheckBoxTS();
            this.chkFullDuplex = new System.Windows.Forms.CheckBoxTS();
            this.btnTuneStepChangeLarger = new System.Windows.Forms.ButtonTS();
            this.btnTuneStepChangeSmaller = new System.Windows.Forms.ButtonTS();
            this.chkVFOLock = new System.Windows.Forms.CheckBoxTS();
            this.txtWheelTune = new System.Windows.Forms.TextBoxTS();
            this.btnMemoryQuickRestore = new System.Windows.Forms.ButtonTS();
            this.btnMemoryQuickSave = new System.Windows.Forms.ButtonTS();
            this.txtMemoryQuick = new System.Windows.Forms.TextBoxTS();
            this.chkVFOBTX = new System.Windows.Forms.CheckBoxTS();
            this.chkVFOATX = new System.Windows.Forms.CheckBoxTS();
            this.txtVFOABand = new System.Windows.Forms.TextBoxTS();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnTNFAdd = new System.Windows.Forms.ButtonTS();
            this.chkTNF = new System.Windows.Forms.CheckBoxTS();
            this.chkDisplayPeak = new System.Windows.Forms.CheckBoxTS();
            this.comboDisplayMode = new System.Windows.Forms.ComboBoxTS();
            this.chkDisplayAVG = new System.Windows.Forms.CheckBoxTS();
            this.label6 = new System.Windows.Forms.Label();
            this.ptbRX2RF = new PowerSDR.PrettyTrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBoxID = new System.Windows.Forms.CheckBoxTS();
            this.chkFWCATU = new System.Windows.Forms.CheckBoxTS();
            this.chkFWCATUBypass = new System.Windows.Forms.CheckBoxTS();
            this.ckQuickPlay = new System.Windows.Forms.CheckBoxTS();
            this.chkMON = new System.Windows.Forms.CheckBoxTS();
            this.ckQuickRec = new System.Windows.Forms.CheckBoxTS();
            this.chkMUT = new System.Windows.Forms.CheckBoxTS();
            this.chkMOX = new System.Windows.Forms.CheckBoxTS();
            this.chkTUN = new System.Windows.Forms.CheckBoxTS();
            this.chkX2TR = new System.Windows.Forms.CheckBoxTS();
            this.comboTuneMode = new System.Windows.Forms.ComboBoxTS();
            this.udCWPitch = new System.Windows.Forms.NumericUpDownTS();
            this.udCWBreakInDelay = new System.Windows.Forms.NumericUpDownTS();
            this.chkCWBreakInEnabled = new System.Windows.Forms.CheckBoxTS();
            this.chkShowTXCWFreq = new System.Windows.Forms.CheckBoxTS();
            this.chkCWSidetone = new System.Windows.Forms.CheckBoxTS();
            this.chkCWIambic = new System.Windows.Forms.CheckBoxTS();
            this.chkShowTXFilter = new System.Windows.Forms.CheckBoxTS();
            this.chkShowDigTXFilter = new System.Windows.Forms.CheckBoxTS();
            this.chkDX = new System.Windows.Forms.CheckBoxTS();
            this.chkTXEQ = new System.Windows.Forms.CheckBoxTS();
            this.comboTXProfile = new System.Windows.Forms.ComboBoxTS();
            this.chkRXEQ = new System.Windows.Forms.CheckBoxTS();
            this.chkCPDR = new System.Windows.Forms.CheckBoxTS();
            this.chkVOX = new System.Windows.Forms.CheckBoxTS();
            this.chkNoiseGate = new System.Windows.Forms.CheckBoxTS();
            this.udRX2FilterHigh = new System.Windows.Forms.NumericUpDownTS();
            this.udRX2FilterLow = new System.Windows.Forms.NumericUpDownTS();
            this.radRX2ModeAM = new System.Windows.Forms.RadioButtonTS();
            this.radRX2ModeLSB = new System.Windows.Forms.RadioButtonTS();
            this.radRX2ModeSAM = new System.Windows.Forms.RadioButtonTS();
            this.radRX2ModeCWL = new System.Windows.Forms.RadioButtonTS();
            this.radRX2ModeDSB = new System.Windows.Forms.RadioButtonTS();
            this.radRX2ModeUSB = new System.Windows.Forms.RadioButtonTS();
            this.radRX2ModeCWU = new System.Windows.Forms.RadioButtonTS();
            this.radRX2ModeFMN = new System.Windows.Forms.RadioButtonTS();
            this.radRX2ModeDIGU = new System.Windows.Forms.RadioButtonTS();
            this.radRX2ModeDRM = new System.Windows.Forms.RadioButtonTS();
            this.radRX2ModeDIGL = new System.Windows.Forms.RadioButtonTS();
            this.radRX2ModeSPEC = new System.Windows.Forms.RadioButtonTS();
            this.chkRX2DisplayPeak = new System.Windows.Forms.CheckBoxTS();
            this.comboRX2DisplayMode = new System.Windows.Forms.ComboBoxTS();
            this.chkRX2Mute = new System.Windows.Forms.CheckBoxTS();
            this.chkPanSwap = new System.Windows.Forms.CheckBoxTS();
            this.chkEnableMultiRX = new System.Windows.Forms.CheckBoxTS();
            this.chkSR = new System.Windows.Forms.CheckBoxTS();
            this.chkNR = new System.Windows.Forms.CheckBoxTS();
            this.chkDSPNB2 = new System.Windows.Forms.CheckBoxTS();
            this.chkBIN = new System.Windows.Forms.CheckBoxTS();
            this.chkNB = new System.Windows.Forms.CheckBoxTS();
            this.chkANF = new System.Windows.Forms.CheckBoxTS();
            this.chkRX1Preamp = new System.Windows.Forms.CheckBoxTS();
            this.comboAGC = new System.Windows.Forms.ComboBoxTS();
            this.lblAGC = new System.Windows.Forms.LabelTS();
            this.comboPreamp = new System.Windows.Forms.ComboBoxTS();
            this.lblRF = new System.Windows.Forms.LabelTS();
            this.comboDigTXProfile = new System.Windows.Forms.ComboBoxTS();
            this.chkVACStereo = new System.Windows.Forms.CheckBoxTS();
            this.comboVACSampleRate = new System.Windows.Forms.ComboBoxTS();
            this.btnDisplayPanCenter = new System.Windows.Forms.ButtonTS();
            this.udFilterHigh = new System.Windows.Forms.NumericUpDownTS();
            this.udFilterLow = new System.Windows.Forms.NumericUpDownTS();
            this.btnFilterShiftReset = new System.Windows.Forms.ButtonTS();
            this.radModeAM = new System.Windows.Forms.RadioButtonTS();
            this.radModeLSB = new System.Windows.Forms.RadioButtonTS();
            this.radModeSAM = new System.Windows.Forms.RadioButtonTS();
            this.radModeCWL = new System.Windows.Forms.RadioButtonTS();
            this.radModeDSB = new System.Windows.Forms.RadioButtonTS();
            this.radModeUSB = new System.Windows.Forms.RadioButtonTS();
            this.radModeCWU = new System.Windows.Forms.RadioButtonTS();
            this.radModeFMN = new System.Windows.Forms.RadioButtonTS();
            this.radModeDIGU = new System.Windows.Forms.RadioButtonTS();
            this.radModeDRM = new System.Windows.Forms.RadioButtonTS();
            this.radModeDIGL = new System.Windows.Forms.RadioButtonTS();
            this.radModeSPEC = new System.Windows.Forms.RadioButtonTS();
            this.comboRX2Band = new System.Windows.Forms.ComboBoxTS();
            this.chkPower = new System.Windows.Forms.CheckBoxTS();
            this.chkSquelch = new System.Windows.Forms.CheckBoxTS();
            this.chkBCI = new System.Windows.Forms.CheckBoxTS();
            this.chkSplitDisplay = new System.Windows.Forms.CheckBoxTS();
            this.comboDisplayModeTop = new System.Windows.Forms.ComboBoxTS();
            this.comboDisplayModeBottom = new System.Windows.Forms.ComboBoxTS();
            this.chkRX2SR = new System.Windows.Forms.CheckBoxTS();
            this.chkRX2NB2 = new System.Windows.Forms.CheckBoxTS();
            this.chkRX2NB = new System.Windows.Forms.CheckBoxTS();
            this.chkRX2ANF = new System.Windows.Forms.CheckBoxTS();
            this.chkRX2NR = new System.Windows.Forms.CheckBoxTS();
            this.chkRX2BIN = new System.Windows.Forms.CheckBoxTS();
            this.comboRX2AGC = new System.Windows.Forms.ComboBoxTS();
            this.lblRX2AGC = new System.Windows.Forms.LabelTS();
            this.lblRX2RF = new System.Windows.Forms.LabelTS();
            this.chkRX2Squelch = new System.Windows.Forms.CheckBoxTS();
            this.chkRX2Preamp = new System.Windows.Forms.CheckBoxTS();
            this.chkRX2DisplayAVG = new System.Windows.Forms.CheckBoxTS();
            this.radDisplayZoom05 = new System.Windows.Forms.RadioButtonTS();
            this.radDisplayZoom4x = new System.Windows.Forms.RadioButtonTS();
            this.radDisplayZoom2x = new System.Windows.Forms.RadioButtonTS();
            this.radDisplayZoom1x = new System.Windows.Forms.RadioButtonTS();
            this.txtDisplayPeakOffset = new System.Windows.Forms.TextBoxTS();
            this.txtDisplayCursorOffset = new System.Windows.Forms.TextBoxTS();
            this.txtDisplayCursorPower = new System.Windows.Forms.TextBoxTS();
            this.txtDisplayCursorFreq = new System.Windows.Forms.TextBoxTS();
            this.txtDisplayPeakPower = new System.Windows.Forms.TextBoxTS();
            this.txtDisplayPeakFreq = new System.Windows.Forms.TextBoxTS();
            this.autoBrightBox = new System.Windows.Forms.TextBoxTS();
            this.radBandGN13 = new System.Windows.Forms.RadioButtonTS();
            this.radBandGN12 = new System.Windows.Forms.RadioButtonTS();
            this.radBandGN11 = new System.Windows.Forms.RadioButtonTS();
            this.radBandGN10 = new System.Windows.Forms.RadioButtonTS();
            this.radBandGN9 = new System.Windows.Forms.RadioButtonTS();
            this.radBandGN8 = new System.Windows.Forms.RadioButtonTS();
            this.radBandGN7 = new System.Windows.Forms.RadioButtonTS();
            this.radBandGN6 = new System.Windows.Forms.RadioButtonTS();
            this.radBandGN5 = new System.Windows.Forms.RadioButtonTS();
            this.radBandGN4 = new System.Windows.Forms.RadioButtonTS();
            this.radBandGN3 = new System.Windows.Forms.RadioButtonTS();
            this.radBandGN2 = new System.Windows.Forms.RadioButtonTS();
            this.radBandGN1 = new System.Windows.Forms.RadioButtonTS();
            this.radBandGN0 = new System.Windows.Forms.RadioButtonTS();
            this.lblAntRX2 = new System.Windows.Forms.LabelTS();
            this.lblAntRX1 = new System.Windows.Forms.LabelTS();
            this.lblAntTX = new System.Windows.Forms.LabelTS();
            this.labelTS4 = new System.Windows.Forms.RichTextBox();
            this.labelTS3 = new System.Windows.Forms.RichTextBox();
            this.chkRX1MUTE = new System.Windows.Forms.CheckBoxTS();
            this.ptbCWSpeed = new PowerSDR.PrettyTrackBar();
            this.ptbRX2Pan = new PowerSDR.PrettyTrackBar();
            this.ptbRX2Gain = new PowerSDR.PrettyTrackBar();
            this.ptbRX1Gain = new PowerSDR.PrettyTrackBar();
            this.ptbPanSubRX = new PowerSDR.PrettyTrackBar();
            this.ptbRX0Gain = new PowerSDR.PrettyTrackBar();
            this.ptbPanMainRX = new PowerSDR.PrettyTrackBar();
            this.ptbPWR = new PowerSDR.PrettyTrackBar();
            this.ptbRF = new PowerSDR.PrettyTrackBar();
            this.ptbAF = new PowerSDR.PrettyTrackBar();
            this.ptbVACTXGain = new PowerSDR.PrettyTrackBar();
            this.ptbVACRXGain = new PowerSDR.PrettyTrackBar();
            this.ptbDisplayZoom = new PowerSDR.PrettyTrackBar();
            this.ptbDisplayPan = new PowerSDR.PrettyTrackBar();
            this.ptbFilterShift = new PowerSDR.PrettyTrackBar();
            this.ptbFilterWidth = new PowerSDR.PrettyTrackBar();
            this.txtNOAA2 = new System.Windows.Forms.RichTextBox();
            this.txtNOAA = new System.Windows.Forms.RichTextBox();
            this.udTXFilterLow = new System.Windows.Forms.NumericUpDownTS();
            this.udTXFilterHigh = new System.Windows.Forms.NumericUpDownTS();
            this.chkBoxMuteSpk = new System.Windows.Forms.CheckBoxTS();
            this.chkBoxDrive = new System.Windows.Forms.CheckBoxTS();
            this.lblPWR = new System.Windows.Forms.LabelTS();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.buttonSort = new System.Windows.Forms.Button();
            this.buttonDel = new System.Windows.Forms.Button();
            this.txtTimer = new System.Windows.Forms.RichTextBox();
            this.ptbTune = new PowerSDR.PrettyTrackBar();
            this.lblTUNE = new System.Windows.Forms.LabelTS();
            this.ptbMON = new PowerSDR.PrettyTrackBar();
            this.lblMON = new System.Windows.Forms.LabelTS();
            this.lblAF = new System.Windows.Forms.LabelTS();
            this.ptbMic = new PowerSDR.PrettyTrackBar();
            this.ptbNoiseGate = new PowerSDR.PrettyTrackBar();
            this.ptbCPDR = new PowerSDR.PrettyTrackBar();
            this.ptbDX = new PowerSDR.PrettyTrackBar();
            this.comboCWTXProfile = new System.Windows.Forms.ComboBoxTS();
            this.ScreenCap = new System.Windows.Forms.PictureBox();
            this.ScreenCap1 = new System.Windows.Forms.PictureBox();
            this.checkVOX = new System.Windows.Forms.CheckBoxTS();
            this.ptbVOX = new PowerSDR.PrettyTrackBar();
            this.lblMIC = new System.Windows.Forms.LabelTS();
            this.udCQCQRepeat = new System.Windows.Forms.NumericUpDownTS();
            this.lblPreamp = new System.Windows.Forms.LabelTS();
            this.buttonCall1 = new System.Windows.Forms.PictureBox();
            this.buttonCQ1 = new System.Windows.Forms.PictureBox();
            this.lblDisplayPan1 = new System.Windows.Forms.PictureBox();
            this.lblDisplayZoom1 = new System.Windows.Forms.PictureBox();
            this.radRX2Filter1 = new System.Windows.Forms.RadioButtonTS();
            this.radRX2FilterVar2 = new System.Windows.Forms.RadioButtonTS();
            this.radRX2FilterVar1 = new System.Windows.Forms.RadioButtonTS();
            this.radFilter1 = new System.Windows.Forms.RadioButtonTS();
            this.radFilterVar2 = new System.Windows.Forms.RadioButtonTS();
            this.radFilterVar1 = new System.Windows.Forms.RadioButtonTS();
            this.buttonVK1 = new System.Windows.Forms.PictureBox();
            this.buttonVK2 = new System.Windows.Forms.PictureBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.lblAntTX2 = new System.Windows.Forms.LabelTS();
            this.lblAntRX1a = new System.Windows.Forms.LabelTS();
            this.lblAntTXa = new System.Windows.Forms.LabelTS();
            this.lblAntRX2a = new System.Windows.Forms.LabelTS();
            this.lblAntTX2a = new System.Windows.Forms.LabelTS();
            this.ptbDisplayZoom2 = new PowerSDR.PrettyTrackBar();
            this.ptbDisplayPan2 = new PowerSDR.PrettyTrackBar();
            this.timer_clock = new System.Windows.Forms.Timer(this.components);
            this.contextMenuStripFilterRX1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemRX1FilterConfigure = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemRX1FilterReset = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripFilterRX2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemRX2FilterConfigure = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemRX2FilterReset = new System.Windows.Forms.ToolStripMenuItem();
            this.timer_navigate = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.contextMenuStripNotch = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripNotchDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripNotchRemember = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripNotchNormal = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripNotchDeep = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripNotchVeryDeep = new System.Windows.Forms.ToolStripMenuItem();
            this.timerNotchZoom = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.setupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.memoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.waveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.equalizerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xVTRsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cWXToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uCBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mixerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.eSCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.antennaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.relaysToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aTUToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.flexControlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.GrayMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TXIDMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.callsignTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.ScanMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.spotterMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.MapMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SWLMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.herosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.remoteProfilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reportBugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.picRX2Squelch = new System.Windows.Forms.PictureBox();
            this.picSquelch = new System.Windows.Forms.PictureBox();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.VFODialA = new System.Windows.Forms.PictureBox();
            this.VFODialB = new System.Windows.Forms.PictureBox();
            this.VFODialAA = new System.Windows.Forms.PictureBox();
            this.VFODialBB = new System.Windows.Forms.PictureBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.panelBandVHFRX2 = new System.Windows.Forms.PanelTS();
            this.radBandVHF13RX2 = new System.Windows.Forms.RadioButtonTS();
            this.radBandVHF12RX2 = new System.Windows.Forms.RadioButtonTS();
            this.radBandVHF11RX2 = new System.Windows.Forms.RadioButtonTS();
            this.radBandVHF10RX2 = new System.Windows.Forms.RadioButtonTS();
            this.radBandVHF9RX2 = new System.Windows.Forms.RadioButtonTS();
            this.radBandVHF8RX2 = new System.Windows.Forms.RadioButtonTS();
            this.radBandVHF7RX2 = new System.Windows.Forms.RadioButtonTS();
            this.radBandVHF6RX2 = new System.Windows.Forms.RadioButtonTS();
            this.radBandVHF5RX2 = new System.Windows.Forms.RadioButtonTS();
            this.radBandVHF4RX2 = new System.Windows.Forms.RadioButtonTS();
            this.radBandVHF3RX2 = new System.Windows.Forms.RadioButtonTS();
            this.radBandVHF2RX2 = new System.Windows.Forms.RadioButtonTS();
            this.radBandVHF1RX2 = new System.Windows.Forms.RadioButtonTS();
            this.radBandVHF0RX2 = new System.Windows.Forms.RadioButtonTS();
            this.btnBandHFRX2 = new System.Windows.Forms.ButtonTS();
            this.panelBandHFRX2 = new System.Windows.Forms.PanelTS();
            this.panelBandVHF = new System.Windows.Forms.PanelTS();
            this.radBandVHF13 = new System.Windows.Forms.RadioButtonTS();
            this.radBandVHF12 = new System.Windows.Forms.RadioButtonTS();
            this.radBandVHF11 = new System.Windows.Forms.RadioButtonTS();
            this.radBandVHF10 = new System.Windows.Forms.RadioButtonTS();
            this.radBandVHF9 = new System.Windows.Forms.RadioButtonTS();
            this.radBandVHF8 = new System.Windows.Forms.RadioButtonTS();
            this.radBandVHF7 = new System.Windows.Forms.RadioButtonTS();
            this.radBandVHF6 = new System.Windows.Forms.RadioButtonTS();
            this.radBandVHF5 = new System.Windows.Forms.RadioButtonTS();
            this.radBandVHF4 = new System.Windows.Forms.RadioButtonTS();
            this.radBandVHF3 = new System.Windows.Forms.RadioButtonTS();
            this.radBandVHF2 = new System.Windows.Forms.RadioButtonTS();
            this.radBandVHF1 = new System.Windows.Forms.RadioButtonTS();
            this.radBandVHF0 = new System.Windows.Forms.RadioButtonTS();
            this.btnBandHF = new System.Windows.Forms.ButtonTS();
            this.grpRX2Meter = new System.Windows.Forms.PanelTS();
            this.lblRX2Meter = new System.Windows.Forms.LabelTS();
            this.txtRX2Meter = new System.Windows.Forms.TextBoxTS();
            this.panelBandGNRX2 = new System.Windows.Forms.PanelTS();
            this.btnBandHF1RX2 = new System.Windows.Forms.ButtonTS();
            this.grpMultimeter = new System.Windows.Forms.PanelTS();
            this.lblMultiSMeter = new System.Windows.Forms.LabelTS();
            this.panelBandHF = new System.Windows.Forms.PanelTS();
            this.panelModeSpecificFM = new System.Windows.Forms.PanelTS();
            this.lblFMMemory = new System.Windows.Forms.LabelTS();
            this.comboFMMemory = new System.Windows.Forms.ComboBoxTS();
            this.lblFMOffset = new System.Windows.Forms.LabelTS();
            this.lblFMDeviation = new System.Windows.Forms.LabelTS();
            this.ptbFMMic = new PowerSDR.PrettyTrackBar();
            this.lblMicValFM = new System.Windows.Forms.LabelTS();
            this.lblFMMic = new System.Windows.Forms.LabelTS();
            this.labelTS7 = new System.Windows.Forms.LabelTS();
            this.panelVFO = new System.Windows.Forms.PanelTS();
            this.panelTS1 = new System.Windows.Forms.PanelTS();
            this.grpVFOBetween = new System.Windows.Forms.PanelTS();
            this.grpVFOB = new System.Windows.Forms.PanelTS();
            this.panelVFOBHover = new System.Windows.Forms.Panel();
            this.txtVFOBLSD = new System.Windows.Forms.TextBoxTS();
            this.txtVFOBMSD = new System.Windows.Forms.TextBoxTS();
            this.txtVFOBBand = new System.Windows.Forms.TextBoxTS();
            this.txtVFOBFreq = new System.Windows.Forms.TextBoxTS();
            this.grpVFOA = new System.Windows.Forms.PanelTS();
            this.panelVFOASubHover = new System.Windows.Forms.Panel();
            this.panelVFOAHover = new System.Windows.Forms.Panel();
            this.txtVFOALSD = new System.Windows.Forms.TextBoxTS();
            this.txtVFOAMSD = new System.Windows.Forms.TextBoxTS();
            this.txtVFOAFreq = new System.Windows.Forms.TextBoxTS();
            this.btnHidden = new System.Windows.Forms.ButtonTS();
            this.panelDisplay2 = new System.Windows.Forms.PanelTS();
            this.ptbRX2Squelch = new PowerSDR.PrettyTrackBar();
            this.panelOptions = new System.Windows.Forms.PanelTS();
            this.panelBandGN = new System.Windows.Forms.PanelTS();
            this.btnBandHF1 = new System.Windows.Forms.ButtonTS();
            this.panelTSBandStack = new System.Windows.Forms.PanelTS();
            this.panelModeSpecificPhone = new System.Windows.Forms.PanelTS();
            this.labelTS2 = new System.Windows.Forms.LabelTS();
            this.labelTS1 = new System.Windows.Forms.LabelTS();
            this.picNoiseGate = new System.Windows.Forms.PictureBox();
            this.lblNoiseGateVal = new System.Windows.Forms.LabelTS();
            this.picVOX = new System.Windows.Forms.PictureBox();
            this.lblVOXVal = new System.Windows.Forms.LabelTS();
            this.lblCPDRVal = new System.Windows.Forms.LabelTS();
            this.lblDXVal = new System.Windows.Forms.LabelTS();
            this.lblMicVal = new System.Windows.Forms.LabelTS();
            this.lblTransmitProfile = new System.Windows.Forms.LabelTS();
            this.panelModeSpecificCW = new System.Windows.Forms.PanelTS();
            this.labelTS6 = new System.Windows.Forms.LabelTS();
            this.lblCWSpeed = new System.Windows.Forms.LabelTS();
            this.grpSemiBreakIn = new System.Windows.Forms.GroupBoxTS();
            this.lblCWBreakInDelay = new System.Windows.Forms.LabelTS();
            this.lblCWPitchFreq = new System.Windows.Forms.LabelTS();
            this.panelModeSpecificDigital = new System.Windows.Forms.PanelTS();
            this.labelVOXVal = new System.Windows.Forms.LabelTS();
            this.lblVACTXIndicator = new System.Windows.Forms.LabelTS();
            this.lblVACRXIndicator = new System.Windows.Forms.LabelTS();
            this.lblDigTXProfile = new System.Windows.Forms.LabelTS();
            this.lblRXGain = new System.Windows.Forms.LabelTS();
            this.grpVACStereo = new System.Windows.Forms.GroupBoxTS();
            this.lblTXGain = new System.Windows.Forms.LabelTS();
            this.grpDIGSampleRate = new System.Windows.Forms.GroupBoxTS();
            this.pictureBoxVOX = new System.Windows.Forms.PictureBox();
            this.prettyTrackBarVOX = new PowerSDR.PrettyTrackBar();
            this.panelAntenna = new System.Windows.Forms.PanelTS();
            this.panelRX2Filter = new System.Windows.Forms.PanelTS();
            this.lblRX2FilterHigh = new System.Windows.Forms.LabelTS();
            this.lblRX2FilterLow = new System.Windows.Forms.LabelTS();
            this.radRX2Filter2 = new System.Windows.Forms.RadioButtonTS();
            this.radRX2Filter3 = new System.Windows.Forms.RadioButtonTS();
            this.radRX2Filter4 = new System.Windows.Forms.RadioButtonTS();
            this.radRX2Filter7 = new System.Windows.Forms.RadioButtonTS();
            this.radRX2Filter5 = new System.Windows.Forms.RadioButtonTS();
            this.radRX2Filter6 = new System.Windows.Forms.RadioButtonTS();
            this.panelRX2Mode = new System.Windows.Forms.PanelTS();
            this.panelRX2Display = new System.Windows.Forms.PanelTS();
            this.label7 = new System.Windows.Forms.Label();
            this.panelRX2Mixer = new System.Windows.Forms.PanelTS();
            this.lblRX2Pan = new System.Windows.Forms.Label();
            this.lblRX2Vol = new System.Windows.Forms.Label();
            this.lblRX2Mute = new System.Windows.Forms.Label();
            this.panelMultiRX = new System.Windows.Forms.PanelTS();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panelDSP = new System.Windows.Forms.PanelTS();
            this.lblCPUMeter = new System.Windows.Forms.RichTextBox();
            this.panelDateTime = new System.Windows.Forms.PanelTS();
            this.txtTime = new System.Windows.Forms.RichTextBox();
            this.txtDate = new System.Windows.Forms.RichTextBox();
            this.panelSoundControls = new System.Windows.Forms.PanelTS();
            this.panelDisplay = new System.Windows.Forms.PanelTS();
            this.picDisplay = new System.Windows.Forms.PictureBox();
            this.panelFilter = new System.Windows.Forms.PanelTS();
            this.lblFilterHigh = new System.Windows.Forms.LabelTS();
            this.lblFilterLow = new System.Windows.Forms.LabelTS();
            this.lblFilterWidth = new System.Windows.Forms.LabelTS();
            this.radFilter10 = new System.Windows.Forms.RadioButtonTS();
            this.lblFilterShift = new System.Windows.Forms.LabelTS();
            this.radFilter9 = new System.Windows.Forms.RadioButtonTS();
            this.radFilter8 = new System.Windows.Forms.RadioButtonTS();
            this.radFilter2 = new System.Windows.Forms.RadioButtonTS();
            this.radFilter7 = new System.Windows.Forms.RadioButtonTS();
            this.radFilter3 = new System.Windows.Forms.RadioButtonTS();
            this.radFilter6 = new System.Windows.Forms.RadioButtonTS();
            this.radFilter4 = new System.Windows.Forms.RadioButtonTS();
            this.radFilter5 = new System.Windows.Forms.RadioButtonTS();
            this.panelMode = new System.Windows.Forms.PanelTS();
            this.lblDisplayModeTop = new System.Windows.Forms.LabelTS();
            this.lblDisplayModeBottom = new System.Windows.Forms.LabelTS();
            this.grpDisplaySplit = new System.Windows.Forms.GroupBoxTS();
            this.chkRX2 = new System.Windows.Forms.CheckBoxTS();
            this.lblRX2Band = new System.Windows.Forms.LabelTS();
            this.panelRX2DSP = new System.Windows.Forms.PanelTS();
            this.ptbSquelch = new PowerSDR.PrettyTrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.picRX3Meter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picRX2Meter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMultiMeterDigital)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udFM1750Timer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udFMOffset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udRIT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udXIT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptbRX2RF)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udCWPitch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udCWBreakInDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udRX2FilterHigh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udRX2FilterLow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udFilterHigh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udFilterLow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptbCWSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptbRX2Pan)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptbRX2Gain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptbRX1Gain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptbPanSubRX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptbRX0Gain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptbPanMainRX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptbPWR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptbRF)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptbAF)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptbVACTXGain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptbVACRXGain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptbDisplayZoom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptbDisplayPan)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptbFilterShift)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptbFilterWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udTXFilterLow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udTXFilterHigh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptbTune)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptbMON)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptbMic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptbNoiseGate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptbCPDR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptbDX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScreenCap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScreenCap1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptbVOX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udCQCQRepeat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.buttonCall1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.buttonCQ1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDisplayPan1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDisplayZoom1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.buttonVK1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.buttonVK2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptbDisplayZoom2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptbDisplayPan2)).BeginInit();
            this.contextMenuStripFilterRX1.SuspendLayout();
            this.contextMenuStripFilterRX2.SuspendLayout();
            this.contextMenuStripNotch.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picRX2Squelch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSquelch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VFODialA)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VFODialB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VFODialAA)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VFODialBB)).BeginInit();
            this.panelBandVHFRX2.SuspendLayout();
            this.panelBandHFRX2.SuspendLayout();
            this.panelBandVHF.SuspendLayout();
            this.grpRX2Meter.SuspendLayout();
            this.panelBandGNRX2.SuspendLayout();
            this.grpMultimeter.SuspendLayout();
            this.panelBandHF.SuspendLayout();
            this.panelModeSpecificFM.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ptbFMMic)).BeginInit();
            this.panelVFO.SuspendLayout();
            this.panelTS1.SuspendLayout();
            this.grpVFOBetween.SuspendLayout();
            this.grpVFOB.SuspendLayout();
            this.grpVFOA.SuspendLayout();
            this.panelDisplay2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ptbRX2Squelch)).BeginInit();
            this.panelOptions.SuspendLayout();
            this.panelBandGN.SuspendLayout();
            this.panelTSBandStack.SuspendLayout();
            this.panelModeSpecificPhone.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picNoiseGate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picVOX)).BeginInit();
            this.panelModeSpecificCW.SuspendLayout();
            this.grpSemiBreakIn.SuspendLayout();
            this.panelModeSpecificDigital.SuspendLayout();
            this.grpVACStereo.SuspendLayout();
            this.grpDIGSampleRate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxVOX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.prettyTrackBarVOX)).BeginInit();
            this.panelAntenna.SuspendLayout();
            this.panelRX2Filter.SuspendLayout();
            this.panelRX2Mode.SuspendLayout();
            this.panelRX2Display.SuspendLayout();
            this.panelRX2Mixer.SuspendLayout();
            this.panelMultiRX.SuspendLayout();
            this.panelDSP.SuspendLayout();
            this.panelDateTime.SuspendLayout();
            this.panelSoundControls.SuspendLayout();
            this.panelDisplay.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picDisplay)).BeginInit();
            this.panelFilter.SuspendLayout();
            this.panelMode.SuspendLayout();
            this.grpDisplaySplit.SuspendLayout();
            this.panelRX2DSP.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ptbSquelch)).BeginInit();
            this.SuspendLayout();
            // 
            // timer_cpu_meter
            // 
            this.timer_cpu_meter.Enabled = true;
            this.timer_cpu_meter.Interval = 1000;
            this.timer_cpu_meter.Tick += new System.EventHandler(this.timer_cpu_meter_Tick);
            // 
            // timer_peak_text
            // 
            this.timer_peak_text.Interval = 500;
            this.timer_peak_text.Tick += new System.EventHandler(this.timer_peak_text_Tick);
            // 
            // toolTip1
            // 
            this.toolTip1.AutomaticDelay = 200;
            this.toolTip1.AutoPopDelay = 13000;
            this.toolTip1.InitialDelay = 500;
            this.toolTip1.ReshowDelay = 40;
            // 
            // labelPowerSDR
            // 
            resources.ApplyResources(this.labelPowerSDR, "labelPowerSDR");
            this.labelPowerSDR.BackColor = System.Drawing.Color.Transparent;
            this.labelPowerSDR.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.labelPowerSDR.Name = "labelPowerSDR";
            this.toolTip1.SetToolTip(this.labelPowerSDR, resources.GetString("labelPowerSDR.ToolTip"));
            this.labelPowerSDR.MouseDown += new System.Windows.Forms.MouseEventHandler(this.labelPowerSDR_MouseDown);
            this.labelPowerSDR.MouseMove += new System.Windows.Forms.MouseEventHandler(this.labelPowerSDR_MouseMove);
            this.labelPowerSDR.MouseUp += new System.Windows.Forms.MouseEventHandler(this.labelPowerSDR_MouseUp);
            // 
            // labelMove
            // 
            resources.ApplyResources(this.labelMove, "labelMove");
            this.labelMove.BackColor = System.Drawing.Color.Transparent;
            this.labelMove.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.labelMove.Name = "labelMove";
            this.toolTip1.SetToolTip(this.labelMove, resources.GetString("labelMove.ToolTip"));
            this.labelMove.MouseDown += new System.Windows.Forms.MouseEventHandler(this.labelMove_MouseDown);
            this.labelMove.MouseMove += new System.Windows.Forms.MouseEventHandler(this.labelPowerSDR_MouseMove);
            this.labelMove.MouseUp += new System.Windows.Forms.MouseEventHandler(this.labelPowerSDR_MouseUp);
            // 
            // labelSize
            // 
            resources.ApplyResources(this.labelSize, "labelSize");
            this.labelSize.BackColor = System.Drawing.Color.Transparent;
            this.labelSize.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.labelSize.Name = "labelSize";
            this.toolTip1.SetToolTip(this.labelSize, resources.GetString("labelSize.ToolTip"));
            this.labelSize.MouseDown += new System.Windows.Forms.MouseEventHandler(this.labelSize_MouseDown);
            this.labelSize.MouseMove += new System.Windows.Forms.MouseEventHandler(this.label9_MouseMove);
            this.labelSize.MouseUp += new System.Windows.Forms.MouseEventHandler(this.labelSize_MouseUp);
            // 
            // labelMax
            // 
            resources.ApplyResources(this.labelMax, "labelMax");
            this.labelMax.BackColor = System.Drawing.Color.Transparent;
            this.labelMax.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.labelMax.Name = "labelMax";
            this.toolTip1.SetToolTip(this.labelMax, resources.GetString("labelMax.ToolTip"));
            this.labelMax.Click += new System.EventHandler(this.label8_Click);
            this.labelMax.MouseDown += new System.Windows.Forms.MouseEventHandler(this.labelMax_MouseDown);
            // 
            // pwrMstWatts
            // 
            resources.ApplyResources(this.pwrMstWatts, "pwrMstWatts");
            this.pwrMstWatts.Name = "pwrMstWatts";
            this.pwrMstWatts.TabStop = false;
            this.toolTip1.SetToolTip(this.pwrMstWatts, resources.GetString("pwrMstWatts.ToolTip"));
            // 
            // pwrMstSWR
            // 
            resources.ApplyResources(this.pwrMstSWR, "pwrMstSWR");
            this.pwrMstSWR.Name = "pwrMstSWR";
            this.pwrMstSWR.TabStop = false;
            this.toolTip1.SetToolTip(this.pwrMstSWR, resources.GetString("pwrMstSWR.ToolTip"));
            // 
            // btnBandVHFRX2
            // 
            this.btnBandVHFRX2.FlatAppearance.BorderSize = 0;
            resources.ApplyResources(this.btnBandVHFRX2, "btnBandVHFRX2");
            this.btnBandVHFRX2.ForeColor = System.Drawing.Color.Gold;
            this.btnBandVHFRX2.Name = "btnBandVHFRX2";
            this.toolTip1.SetToolTip(this.btnBandVHFRX2, resources.GetString("btnBandVHFRX2.ToolTip"));
            this.btnBandVHFRX2.Click += new System.EventHandler(this.btnBandVHFRX2_Click);
            // 
            // radBandGENRX2
            // 
            resources.ApplyResources(this.radBandGENRX2, "radBandGENRX2");
            this.radBandGENRX2.FlatAppearance.BorderSize = 0;
            this.radBandGENRX2.ForeColor = System.Drawing.Color.Coral;
            this.radBandGENRX2.Name = "radBandGENRX2";
            this.radBandGENRX2.TabStop = true;
            this.toolTip1.SetToolTip(this.radBandGENRX2, resources.GetString("radBandGENRX2.ToolTip"));
            this.radBandGENRX2.UseVisualStyleBackColor = true;
            this.radBandGENRX2.Click += new System.EventHandler(this.btnBandGENRX2_Click);
            // 
            // radBandWWVRX2
            // 
            resources.ApplyResources(this.radBandWWVRX2, "radBandWWVRX2");
            this.radBandWWVRX2.FlatAppearance.BorderSize = 0;
            this.radBandWWVRX2.ForeColor = System.Drawing.Color.LightGreen;
            this.radBandWWVRX2.Name = "radBandWWVRX2";
            this.radBandWWVRX2.TabStop = true;
            this.toolTip1.SetToolTip(this.radBandWWVRX2, resources.GetString("radBandWWVRX2.ToolTip"));
            this.radBandWWVRX2.UseVisualStyleBackColor = true;
            this.radBandWWVRX2.Click += new System.EventHandler(this.radBandWWVRX2_Click);
            // 
            // radBand2RX2
            // 
            resources.ApplyResources(this.radBand2RX2, "radBand2RX2");
            this.radBand2RX2.FlatAppearance.BorderSize = 0;
            this.radBand2RX2.ForeColor = System.Drawing.Color.White;
            this.radBand2RX2.Name = "radBand2RX2";
            this.radBand2RX2.TabStop = true;
            this.toolTip1.SetToolTip(this.radBand2RX2, resources.GetString("radBand2RX2.ToolTip"));
            this.radBand2RX2.UseVisualStyleBackColor = true;
            this.radBand2RX2.Click += new System.EventHandler(this.radBand2RX2_Click);
            // 
            // radBand6RX2
            // 
            resources.ApplyResources(this.radBand6RX2, "radBand6RX2");
            this.radBand6RX2.FlatAppearance.BorderSize = 0;
            this.radBand6RX2.ForeColor = System.Drawing.Color.White;
            this.radBand6RX2.Name = "radBand6RX2";
            this.radBand6RX2.TabStop = true;
            this.toolTip1.SetToolTip(this.radBand6RX2, resources.GetString("radBand6RX2.ToolTip"));
            this.radBand6RX2.UseVisualStyleBackColor = true;
            this.radBand6RX2.Click += new System.EventHandler(this.radBand6RX2_Click);
            // 
            // radBand10RX2
            // 
            resources.ApplyResources(this.radBand10RX2, "radBand10RX2");
            this.radBand10RX2.FlatAppearance.BorderSize = 0;
            this.radBand10RX2.ForeColor = System.Drawing.Color.White;
            this.radBand10RX2.Name = "radBand10RX2";
            this.radBand10RX2.TabStop = true;
            this.toolTip1.SetToolTip(this.radBand10RX2, resources.GetString("radBand10RX2.ToolTip"));
            this.radBand10RX2.UseVisualStyleBackColor = true;
            this.radBand10RX2.Click += new System.EventHandler(this.radBand10RX2_Click);
            // 
            // radBand12RX2
            // 
            resources.ApplyResources(this.radBand12RX2, "radBand12RX2");
            this.radBand12RX2.FlatAppearance.BorderSize = 0;
            this.radBand12RX2.ForeColor = System.Drawing.Color.White;
            this.radBand12RX2.Name = "radBand12RX2";
            this.radBand12RX2.TabStop = true;
            this.toolTip1.SetToolTip(this.radBand12RX2, resources.GetString("radBand12RX2.ToolTip"));
            this.radBand12RX2.UseVisualStyleBackColor = true;
            this.radBand12RX2.Click += new System.EventHandler(this.radBand12RX2_Click);
            // 
            // radBand15RX2
            // 
            resources.ApplyResources(this.radBand15RX2, "radBand15RX2");
            this.radBand15RX2.FlatAppearance.BorderSize = 0;
            this.radBand15RX2.ForeColor = System.Drawing.Color.White;
            this.radBand15RX2.Name = "radBand15RX2";
            this.radBand15RX2.TabStop = true;
            this.toolTip1.SetToolTip(this.radBand15RX2, resources.GetString("radBand15RX2.ToolTip"));
            this.radBand15RX2.UseVisualStyleBackColor = true;
            this.radBand15RX2.Click += new System.EventHandler(this.radBand15RX2_Click);
            // 
            // radBand17RX2
            // 
            resources.ApplyResources(this.radBand17RX2, "radBand17RX2");
            this.radBand17RX2.FlatAppearance.BorderSize = 0;
            this.radBand17RX2.ForeColor = System.Drawing.Color.White;
            this.radBand17RX2.Name = "radBand17RX2";
            this.radBand17RX2.TabStop = true;
            this.toolTip1.SetToolTip(this.radBand17RX2, resources.GetString("radBand17RX2.ToolTip"));
            this.radBand17RX2.UseVisualStyleBackColor = true;
            this.radBand17RX2.Click += new System.EventHandler(this.radBand17RX2_Click);
            // 
            // radBand20RX2
            // 
            resources.ApplyResources(this.radBand20RX2, "radBand20RX2");
            this.radBand20RX2.FlatAppearance.BorderSize = 0;
            this.radBand20RX2.ForeColor = System.Drawing.Color.White;
            this.radBand20RX2.Name = "radBand20RX2";
            this.radBand20RX2.TabStop = true;
            this.toolTip1.SetToolTip(this.radBand20RX2, resources.GetString("radBand20RX2.ToolTip"));
            this.radBand20RX2.UseVisualStyleBackColor = true;
            this.radBand20RX2.Click += new System.EventHandler(this.radBand20RX2_Click);
            // 
            // radBand30RX2
            // 
            resources.ApplyResources(this.radBand30RX2, "radBand30RX2");
            this.radBand30RX2.FlatAppearance.BorderSize = 0;
            this.radBand30RX2.ForeColor = System.Drawing.Color.White;
            this.radBand30RX2.Name = "radBand30RX2";
            this.radBand30RX2.TabStop = true;
            this.toolTip1.SetToolTip(this.radBand30RX2, resources.GetString("radBand30RX2.ToolTip"));
            this.radBand30RX2.UseVisualStyleBackColor = true;
            this.radBand30RX2.Click += new System.EventHandler(this.radBand30RX2_Click);
            // 
            // radBand40RX2
            // 
            resources.ApplyResources(this.radBand40RX2, "radBand40RX2");
            this.radBand40RX2.FlatAppearance.BorderSize = 0;
            this.radBand40RX2.ForeColor = System.Drawing.Color.White;
            this.radBand40RX2.Name = "radBand40RX2";
            this.radBand40RX2.TabStop = true;
            this.toolTip1.SetToolTip(this.radBand40RX2, resources.GetString("radBand40RX2.ToolTip"));
            this.radBand40RX2.UseVisualStyleBackColor = true;
            this.radBand40RX2.Click += new System.EventHandler(this.radBand40RX2_Click);
            // 
            // radBand60RX2
            // 
            resources.ApplyResources(this.radBand60RX2, "radBand60RX2");
            this.radBand60RX2.FlatAppearance.BorderSize = 0;
            this.radBand60RX2.ForeColor = System.Drawing.Color.White;
            this.radBand60RX2.Name = "radBand60RX2";
            this.radBand60RX2.TabStop = true;
            this.toolTip1.SetToolTip(this.radBand60RX2, resources.GetString("radBand60RX2.ToolTip"));
            this.radBand60RX2.UseVisualStyleBackColor = true;
            this.radBand60RX2.Click += new System.EventHandler(this.radBand60RX2_Click);
            // 
            // radBand160RX2
            // 
            resources.ApplyResources(this.radBand160RX2, "radBand160RX2");
            this.radBand160RX2.FlatAppearance.BorderSize = 0;
            this.radBand160RX2.ForeColor = System.Drawing.Color.White;
            this.radBand160RX2.Name = "radBand160RX2";
            this.radBand160RX2.TabStop = true;
            this.toolTip1.SetToolTip(this.radBand160RX2, resources.GetString("radBand160RX2.ToolTip"));
            this.radBand160RX2.UseVisualStyleBackColor = true;
            this.radBand160RX2.Click += new System.EventHandler(this.radBand160RX2_Click);
            // 
            // radBand80RX2
            // 
            resources.ApplyResources(this.radBand80RX2, "radBand80RX2");
            this.radBand80RX2.FlatAppearance.BorderSize = 0;
            this.radBand80RX2.ForeColor = System.Drawing.Color.White;
            this.radBand80RX2.Name = "radBand80RX2";
            this.radBand80RX2.TabStop = true;
            this.toolTip1.SetToolTip(this.radBand80RX2, resources.GetString("radBand80RX2.ToolTip"));
            this.radBand80RX2.UseVisualStyleBackColor = true;
            this.radBand80RX2.Click += new System.EventHandler(this.radBand80RX2_Click);
            // 
            // picRX3Meter
            // 
            this.picRX3Meter.BackColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.picRX3Meter, "picRX3Meter");
            this.picRX3Meter.Name = "picRX3Meter";
            this.picRX3Meter.TabStop = false;
            this.toolTip1.SetToolTip(this.picRX3Meter, resources.GetString("picRX3Meter.ToolTip"));
            this.picRX3Meter.Click += new System.EventHandler(this.picRX3Meter_Click);
            this.picRX3Meter.Paint += new System.Windows.Forms.PaintEventHandler(this.picRX3Meter_Paint);
            // 
            // picRX2Meter
            // 
            this.picRX2Meter.BackColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.picRX2Meter, "picRX2Meter");
            this.picRX2Meter.Name = "picRX2Meter";
            this.picRX2Meter.TabStop = false;
            this.toolTip1.SetToolTip(this.picRX2Meter, resources.GetString("picRX2Meter.ToolTip"));
            this.picRX2Meter.Paint += new System.Windows.Forms.PaintEventHandler(this.picRX2Meter_Paint);
            this.picRX2Meter.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picRX2Meter_MouseUp);
            // 
            // comboMeterTX1Mode
            // 
            this.comboMeterTX1Mode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.comboMeterTX1Mode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboMeterTX1Mode.DropDownWidth = 72;
            resources.ApplyResources(this.comboMeterTX1Mode, "comboMeterTX1Mode");
            this.comboMeterTX1Mode.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.comboMeterTX1Mode.Name = "comboMeterTX1Mode";
            this.toolTip1.SetToolTip(this.comboMeterTX1Mode, resources.GetString("comboMeterTX1Mode.ToolTip"));
            this.comboMeterTX1Mode.SelectedIndexChanged += new System.EventHandler(this.comboMeterTX1Mode_SelectedIndexChanged);
            // 
            // comboRX2MeterMode
            // 
            this.comboRX2MeterMode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.comboRX2MeterMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboRX2MeterMode.DropDownWidth = 72;
            resources.ApplyResources(this.comboRX2MeterMode, "comboRX2MeterMode");
            this.comboRX2MeterMode.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.comboRX2MeterMode.Name = "comboRX2MeterMode";
            this.toolTip1.SetToolTip(this.comboRX2MeterMode, resources.GetString("comboRX2MeterMode.ToolTip"));
            this.comboRX2MeterMode.SelectedIndexChanged += new System.EventHandler(this.comboRX2MeterMode_SelectedIndexChanged);
            // 
            // radBandGN13RX2
            // 
            resources.ApplyResources(this.radBandGN13RX2, "radBandGN13RX2");
            this.radBandGN13RX2.FlatAppearance.BorderSize = 0;
            this.radBandGN13RX2.ForeColor = System.Drawing.Color.Yellow;
            this.radBandGN13RX2.Name = "radBandGN13RX2";
            this.radBandGN13RX2.TabStop = true;
            this.toolTip1.SetToolTip(this.radBandGN13RX2, resources.GetString("radBandGN13RX2.ToolTip"));
            this.radBandGN13RX2.UseVisualStyleBackColor = true;
            this.radBandGN13RX2.Click += new System.EventHandler(this.radBandGEN13RX2_Click);
            // 
            // radBandGN12RX2
            // 
            resources.ApplyResources(this.radBandGN12RX2, "radBandGN12RX2");
            this.radBandGN12RX2.FlatAppearance.BorderSize = 0;
            this.radBandGN12RX2.ForeColor = System.Drawing.Color.Yellow;
            this.radBandGN12RX2.Name = "radBandGN12RX2";
            this.radBandGN12RX2.TabStop = true;
            this.toolTip1.SetToolTip(this.radBandGN12RX2, resources.GetString("radBandGN12RX2.ToolTip"));
            this.radBandGN12RX2.UseVisualStyleBackColor = false;
            this.radBandGN12RX2.Click += new System.EventHandler(this.radBandGEN12RX2_Click);
            // 
            // radBandGN11RX2
            // 
            resources.ApplyResources(this.radBandGN11RX2, "radBandGN11RX2");
            this.radBandGN11RX2.FlatAppearance.BorderSize = 0;
            this.radBandGN11RX2.ForeColor = System.Drawing.Color.Yellow;
            this.radBandGN11RX2.Name = "radBandGN11RX2";
            this.radBandGN11RX2.TabStop = true;
            this.toolTip1.SetToolTip(this.radBandGN11RX2, resources.GetString("radBandGN11RX2.ToolTip"));
            this.radBandGN11RX2.UseVisualStyleBackColor = true;
            this.radBandGN11RX2.Click += new System.EventHandler(this.radBandGEN11RX2_Click);
            // 
            // radBandGN10RX2
            // 
            resources.ApplyResources(this.radBandGN10RX2, "radBandGN10RX2");
            this.radBandGN10RX2.FlatAppearance.BorderSize = 0;
            this.radBandGN10RX2.ForeColor = System.Drawing.Color.Yellow;
            this.radBandGN10RX2.Name = "radBandGN10RX2";
            this.radBandGN10RX2.TabStop = true;
            this.toolTip1.SetToolTip(this.radBandGN10RX2, resources.GetString("radBandGN10RX2.ToolTip"));
            this.radBandGN10RX2.UseVisualStyleBackColor = true;
            this.radBandGN10RX2.Click += new System.EventHandler(this.radBandGEN10RX2_Click);
            // 
            // radBandGN9RX2
            // 
            resources.ApplyResources(this.radBandGN9RX2, "radBandGN9RX2");
            this.radBandGN9RX2.FlatAppearance.BorderSize = 0;
            this.radBandGN9RX2.ForeColor = System.Drawing.Color.Yellow;
            this.radBandGN9RX2.Name = "radBandGN9RX2";
            this.radBandGN9RX2.TabStop = true;
            this.toolTip1.SetToolTip(this.radBandGN9RX2, resources.GetString("radBandGN9RX2.ToolTip"));
            this.radBandGN9RX2.UseVisualStyleBackColor = true;
            this.radBandGN9RX2.Click += new System.EventHandler(this.radBandGEN9RX2_Click);
            // 
            // radBandGN8RX2
            // 
            resources.ApplyResources(this.radBandGN8RX2, "radBandGN8RX2");
            this.radBandGN8RX2.FlatAppearance.BorderSize = 0;
            this.radBandGN8RX2.ForeColor = System.Drawing.Color.Yellow;
            this.radBandGN8RX2.Name = "radBandGN8RX2";
            this.radBandGN8RX2.TabStop = true;
            this.toolTip1.SetToolTip(this.radBandGN8RX2, resources.GetString("radBandGN8RX2.ToolTip"));
            this.radBandGN8RX2.UseVisualStyleBackColor = true;
            this.radBandGN8RX2.Click += new System.EventHandler(this.radBandGEN8RX2_Click);
            // 
            // radBandGN7RX2
            // 
            resources.ApplyResources(this.radBandGN7RX2, "radBandGN7RX2");
            this.radBandGN7RX2.FlatAppearance.BorderSize = 0;
            this.radBandGN7RX2.ForeColor = System.Drawing.Color.Yellow;
            this.radBandGN7RX2.Name = "radBandGN7RX2";
            this.radBandGN7RX2.TabStop = true;
            this.toolTip1.SetToolTip(this.radBandGN7RX2, resources.GetString("radBandGN7RX2.ToolTip"));
            this.radBandGN7RX2.UseVisualStyleBackColor = true;
            this.radBandGN7RX2.Click += new System.EventHandler(this.radBandGEN7RX2_Click);
            // 
            // radBandGN6RX2
            // 
            resources.ApplyResources(this.radBandGN6RX2, "radBandGN6RX2");
            this.radBandGN6RX2.FlatAppearance.BorderSize = 0;
            this.radBandGN6RX2.ForeColor = System.Drawing.Color.Yellow;
            this.radBandGN6RX2.Name = "radBandGN6RX2";
            this.radBandGN6RX2.TabStop = true;
            this.toolTip1.SetToolTip(this.radBandGN6RX2, resources.GetString("radBandGN6RX2.ToolTip"));
            this.radBandGN6RX2.UseVisualStyleBackColor = true;
            this.radBandGN6RX2.Click += new System.EventHandler(this.radBandGEN6RX2_Click);
            // 
            // radBandGN5RX2
            // 
            resources.ApplyResources(this.radBandGN5RX2, "radBandGN5RX2");
            this.radBandGN5RX2.FlatAppearance.BorderSize = 0;
            this.radBandGN5RX2.ForeColor = System.Drawing.Color.Yellow;
            this.radBandGN5RX2.Name = "radBandGN5RX2";
            this.radBandGN5RX2.TabStop = true;
            this.toolTip1.SetToolTip(this.radBandGN5RX2, resources.GetString("radBandGN5RX2.ToolTip"));
            this.radBandGN5RX2.UseVisualStyleBackColor = true;
            this.radBandGN5RX2.Click += new System.EventHandler(this.radBandGEN5RX2_Click);
            // 
            // radBandGN4RX2
            // 
            resources.ApplyResources(this.radBandGN4RX2, "radBandGN4RX2");
            this.radBandGN4RX2.FlatAppearance.BorderSize = 0;
            this.radBandGN4RX2.ForeColor = System.Drawing.Color.Yellow;
            this.radBandGN4RX2.Name = "radBandGN4RX2";
            this.radBandGN4RX2.TabStop = true;
            this.toolTip1.SetToolTip(this.radBandGN4RX2, resources.GetString("radBandGN4RX2.ToolTip"));
            this.radBandGN4RX2.UseVisualStyleBackColor = true;
            this.radBandGN4RX2.Click += new System.EventHandler(this.radBandGEN4RX2_Click);
            // 
            // radBandGN3RX2
            // 
            resources.ApplyResources(this.radBandGN3RX2, "radBandGN3RX2");
            this.radBandGN3RX2.FlatAppearance.BorderSize = 0;
            this.radBandGN3RX2.ForeColor = System.Drawing.Color.Yellow;
            this.radBandGN3RX2.Name = "radBandGN3RX2";
            this.radBandGN3RX2.TabStop = true;
            this.toolTip1.SetToolTip(this.radBandGN3RX2, resources.GetString("radBandGN3RX2.ToolTip"));
            this.radBandGN3RX2.UseVisualStyleBackColor = true;
            this.radBandGN3RX2.Click += new System.EventHandler(this.radBandGEN3RX2_Click);
            // 
            // radBandGN2RX2
            // 
            resources.ApplyResources(this.radBandGN2RX2, "radBandGN2RX2");
            this.radBandGN2RX2.FlatAppearance.BorderSize = 0;
            this.radBandGN2RX2.ForeColor = System.Drawing.Color.Yellow;
            this.radBandGN2RX2.Name = "radBandGN2RX2";
            this.radBandGN2RX2.TabStop = true;
            this.toolTip1.SetToolTip(this.radBandGN2RX2, resources.GetString("radBandGN2RX2.ToolTip"));
            this.radBandGN2RX2.UseVisualStyleBackColor = true;
            this.radBandGN2RX2.Click += new System.EventHandler(this.radBandGEN2RX2_Click);
            // 
            // radBandGN1RX2
            // 
            resources.ApplyResources(this.radBandGN1RX2, "radBandGN1RX2");
            this.radBandGN1RX2.FlatAppearance.BorderSize = 0;
            this.radBandGN1RX2.ForeColor = System.Drawing.Color.Yellow;
            this.radBandGN1RX2.Name = "radBandGN1RX2";
            this.radBandGN1RX2.TabStop = true;
            this.toolTip1.SetToolTip(this.radBandGN1RX2, resources.GetString("radBandGN1RX2.ToolTip"));
            this.radBandGN1RX2.UseVisualStyleBackColor = true;
            this.radBandGN1RX2.Click += new System.EventHandler(this.radBandGEN1RX2_Click);
            // 
            // radBandGN0RX2
            // 
            resources.ApplyResources(this.radBandGN0RX2, "radBandGN0RX2");
            this.radBandGN0RX2.FlatAppearance.BorderSize = 0;
            this.radBandGN0RX2.ForeColor = System.Drawing.Color.Yellow;
            this.radBandGN0RX2.Name = "radBandGN0RX2";
            this.radBandGN0RX2.TabStop = true;
            this.toolTip1.SetToolTip(this.radBandGN0RX2, resources.GetString("radBandGN0RX2.ToolTip"));
            this.radBandGN0RX2.UseVisualStyleBackColor = true;
            this.radBandGN0RX2.Click += new System.EventHandler(this.radBandGEN0RX2_Click);
            // 
            // comboMeterTXMode
            // 
            this.comboMeterTXMode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.comboMeterTXMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboMeterTXMode.DropDownWidth = 72;
            resources.ApplyResources(this.comboMeterTXMode, "comboMeterTXMode");
            this.comboMeterTXMode.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.comboMeterTXMode.Name = "comboMeterTXMode";
            this.toolTip1.SetToolTip(this.comboMeterTXMode, resources.GetString("comboMeterTXMode.ToolTip"));
            this.comboMeterTXMode.SelectedIndexChanged += new System.EventHandler(this.comboMeterTXMode_SelectedIndexChanged);
            // 
            // picMultiMeterDigital
            // 
            this.picMultiMeterDigital.BackColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.picMultiMeterDigital, "picMultiMeterDigital");
            this.picMultiMeterDigital.Name = "picMultiMeterDigital";
            this.picMultiMeterDigital.TabStop = false;
            this.toolTip1.SetToolTip(this.picMultiMeterDigital, resources.GetString("picMultiMeterDigital.ToolTip"));
            this.picMultiMeterDigital.Paint += new System.Windows.Forms.PaintEventHandler(this.picMultiMeterDigital_Paint);
            this.picMultiMeterDigital.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picMultiMeterDigital_MouseUp);
            // 
            // comboMeterRXMode
            // 
            this.comboMeterRXMode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.comboMeterRXMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboMeterRXMode.DropDownWidth = 72;
            resources.ApplyResources(this.comboMeterRXMode, "comboMeterRXMode");
            this.comboMeterRXMode.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.comboMeterRXMode.Name = "comboMeterRXMode";
            this.toolTip1.SetToolTip(this.comboMeterRXMode, resources.GetString("comboMeterRXMode.ToolTip"));
            this.comboMeterRXMode.SelectedIndexChanged += new System.EventHandler(this.comboMeterRXMode_SelectedIndexChanged);
            // 
            // txtMultiText
            // 
            this.txtMultiText.BackColor = System.Drawing.Color.Black;
            this.txtMultiText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.txtMultiText, "txtMultiText");
            this.txtMultiText.ForeColor = System.Drawing.Color.Yellow;
            this.txtMultiText.Name = "txtMultiText";
            this.txtMultiText.ReadOnly = true;
            this.txtMultiText.ShortcutsEnabled = false;
            this.toolTip1.SetToolTip(this.txtMultiText, resources.GetString("txtMultiText.ToolTip"));
            this.txtMultiText.GotFocus += new System.EventHandler(this.HideFocus);
            this.txtMultiText.MouseUp += new System.Windows.Forms.MouseEventHandler(this.txtMultiText_MouseUp);
            // 
            // radBandGEN
            // 
            resources.ApplyResources(this.radBandGEN, "radBandGEN");
            this.radBandGEN.FlatAppearance.BorderSize = 0;
            this.radBandGEN.ForeColor = System.Drawing.Color.Coral;
            this.radBandGEN.Name = "radBandGEN";
            this.radBandGEN.TabStop = true;
            this.toolTip1.SetToolTip(this.radBandGEN, resources.GetString("radBandGEN.ToolTip"));
            this.radBandGEN.UseVisualStyleBackColor = true;
            this.radBandGEN.Click += new System.EventHandler(this.btnBandGEN_Click);
            this.radBandGEN.PaddingChanged += new System.EventHandler(this.radBandGEN_Click);
            // 
            // radBandWWV
            // 
            resources.ApplyResources(this.radBandWWV, "radBandWWV");
            this.radBandWWV.FlatAppearance.BorderSize = 0;
            this.radBandWWV.ForeColor = System.Drawing.Color.LightGreen;
            this.radBandWWV.Name = "radBandWWV";
            this.radBandWWV.TabStop = true;
            this.toolTip1.SetToolTip(this.radBandWWV, resources.GetString("radBandWWV.ToolTip"));
            this.radBandWWV.UseVisualStyleBackColor = true;
            this.radBandWWV.Click += new System.EventHandler(this.radBandWWV_Click);
            this.radBandWWV.MouseDown += new System.Windows.Forms.MouseEventHandler(this.radBandWWV_MouseDown);
            // 
            // radBand2
            // 
            resources.ApplyResources(this.radBand2, "radBand2");
            this.radBand2.FlatAppearance.BorderSize = 0;
            this.radBand2.ForeColor = System.Drawing.Color.White;
            this.radBand2.Name = "radBand2";
            this.radBand2.TabStop = true;
            this.toolTip1.SetToolTip(this.radBand2, resources.GetString("radBand2.ToolTip"));
            this.radBand2.UseVisualStyleBackColor = true;
            this.radBand2.Click += new System.EventHandler(this.radBand2_Click);
            // 
            // radBand6
            // 
            resources.ApplyResources(this.radBand6, "radBand6");
            this.radBand6.FlatAppearance.BorderSize = 0;
            this.radBand6.ForeColor = System.Drawing.Color.White;
            this.radBand6.Name = "radBand6";
            this.radBand6.TabStop = true;
            this.toolTip1.SetToolTip(this.radBand6, resources.GetString("radBand6.ToolTip"));
            this.radBand6.UseVisualStyleBackColor = true;
            this.radBand6.Click += new System.EventHandler(this.radBand6_Click);
            this.radBand6.MouseDown += new System.Windows.Forms.MouseEventHandler(this.radBand6_MouseDown);
            // 
            // radBand10
            // 
            resources.ApplyResources(this.radBand10, "radBand10");
            this.radBand10.FlatAppearance.BorderSize = 0;
            this.radBand10.ForeColor = System.Drawing.Color.White;
            this.radBand10.Name = "radBand10";
            this.radBand10.TabStop = true;
            this.toolTip1.SetToolTip(this.radBand10, resources.GetString("radBand10.ToolTip"));
            this.radBand10.UseVisualStyleBackColor = true;
            this.radBand10.Click += new System.EventHandler(this.radBand10_Click);
            this.radBand10.MouseDown += new System.Windows.Forms.MouseEventHandler(this.radBand10_MouseDown);
            // 
            // radBand12
            // 
            resources.ApplyResources(this.radBand12, "radBand12");
            this.radBand12.FlatAppearance.BorderSize = 0;
            this.radBand12.ForeColor = System.Drawing.Color.White;
            this.radBand12.Name = "radBand12";
            this.radBand12.TabStop = true;
            this.toolTip1.SetToolTip(this.radBand12, resources.GetString("radBand12.ToolTip"));
            this.radBand12.UseVisualStyleBackColor = true;
            this.radBand12.Click += new System.EventHandler(this.radBand12_Click);
            this.radBand12.MouseDown += new System.Windows.Forms.MouseEventHandler(this.radBand12_MouseDown);
            // 
            // radBand15
            // 
            resources.ApplyResources(this.radBand15, "radBand15");
            this.radBand15.FlatAppearance.BorderSize = 0;
            this.radBand15.ForeColor = System.Drawing.Color.White;
            this.radBand15.Name = "radBand15";
            this.radBand15.TabStop = true;
            this.toolTip1.SetToolTip(this.radBand15, resources.GetString("radBand15.ToolTip"));
            this.radBand15.UseVisualStyleBackColor = true;
            this.radBand15.Click += new System.EventHandler(this.radBand15_Click);
            this.radBand15.MouseDown += new System.Windows.Forms.MouseEventHandler(this.radBand15_MouseDown);
            // 
            // radBand17
            // 
            resources.ApplyResources(this.radBand17, "radBand17");
            this.radBand17.FlatAppearance.BorderSize = 0;
            this.radBand17.ForeColor = System.Drawing.Color.White;
            this.radBand17.Name = "radBand17";
            this.radBand17.TabStop = true;
            this.toolTip1.SetToolTip(this.radBand17, resources.GetString("radBand17.ToolTip"));
            this.radBand17.UseVisualStyleBackColor = true;
            this.radBand17.Click += new System.EventHandler(this.radBand17_Click);
            this.radBand17.MouseDown += new System.Windows.Forms.MouseEventHandler(this.radBand17_MouseDown);
            // 
            // radBand20
            // 
            resources.ApplyResources(this.radBand20, "radBand20");
            this.radBand20.FlatAppearance.BorderSize = 0;
            this.radBand20.ForeColor = System.Drawing.Color.White;
            this.radBand20.Name = "radBand20";
            this.radBand20.TabStop = true;
            this.toolTip1.SetToolTip(this.radBand20, resources.GetString("radBand20.ToolTip"));
            this.radBand20.UseVisualStyleBackColor = true;
            this.radBand20.Click += new System.EventHandler(this.radBand20_Click);
            this.radBand20.MouseDown += new System.Windows.Forms.MouseEventHandler(this.radBand20_MouseDown);
            // 
            // radBand30
            // 
            resources.ApplyResources(this.radBand30, "radBand30");
            this.radBand30.FlatAppearance.BorderSize = 0;
            this.radBand30.ForeColor = System.Drawing.Color.White;
            this.radBand30.Name = "radBand30";
            this.radBand30.TabStop = true;
            this.toolTip1.SetToolTip(this.radBand30, resources.GetString("radBand30.ToolTip"));
            this.radBand30.UseVisualStyleBackColor = true;
            this.radBand30.Click += new System.EventHandler(this.radBand30_Click);
            this.radBand30.MouseDown += new System.Windows.Forms.MouseEventHandler(this.radBand30_MouseDown);
            // 
            // radBand40
            // 
            resources.ApplyResources(this.radBand40, "radBand40");
            this.radBand40.FlatAppearance.BorderSize = 0;
            this.radBand40.ForeColor = System.Drawing.Color.White;
            this.radBand40.Name = "radBand40";
            this.radBand40.TabStop = true;
            this.toolTip1.SetToolTip(this.radBand40, resources.GetString("radBand40.ToolTip"));
            this.radBand40.UseVisualStyleBackColor = true;
            this.radBand40.Click += new System.EventHandler(this.radBand40_Click);
            this.radBand40.MouseDown += new System.Windows.Forms.MouseEventHandler(this.radBand40_MouseDown);
            // 
            // radBand60
            // 
            resources.ApplyResources(this.radBand60, "radBand60");
            this.radBand60.FlatAppearance.BorderSize = 0;
            this.radBand60.ForeColor = System.Drawing.Color.White;
            this.radBand60.Name = "radBand60";
            this.radBand60.TabStop = true;
            this.toolTip1.SetToolTip(this.radBand60, resources.GetString("radBand60.ToolTip"));
            this.radBand60.UseVisualStyleBackColor = true;
            this.radBand60.Click += new System.EventHandler(this.radBand60_Click);
            this.radBand60.MouseDown += new System.Windows.Forms.MouseEventHandler(this.radBand60_MouseDown);
            // 
            // radBand160
            // 
            resources.ApplyResources(this.radBand160, "radBand160");
            this.radBand160.FlatAppearance.BorderSize = 0;
            this.radBand160.ForeColor = System.Drawing.Color.White;
            this.radBand160.Name = "radBand160";
            this.radBand160.TabStop = true;
            this.toolTip1.SetToolTip(this.radBand160, resources.GetString("radBand160.ToolTip"));
            this.radBand160.UseVisualStyleBackColor = true;
            this.radBand160.Click += new System.EventHandler(this.radBand160_Click);
            this.radBand160.MouseDown += new System.Windows.Forms.MouseEventHandler(this.radBand160_MouseDown);
            // 
            // radBand80
            // 
            resources.ApplyResources(this.radBand80, "radBand80");
            this.radBand80.FlatAppearance.BorderSize = 0;
            this.radBand80.ForeColor = System.Drawing.Color.White;
            this.radBand80.Name = "radBand80";
            this.radBand80.TabStop = true;
            this.toolTip1.SetToolTip(this.radBand80, resources.GetString("radBand80.ToolTip"));
            this.radBand80.UseVisualStyleBackColor = true;
            this.radBand80.Click += new System.EventHandler(this.radBand80_Click);
            this.radBand80.MouseDown += new System.Windows.Forms.MouseEventHandler(this.radBand80_MouseDown);
            // 
            // btnBandVHF
            // 
            this.btnBandVHF.FlatAppearance.BorderSize = 0;
            resources.ApplyResources(this.btnBandVHF, "btnBandVHF");
            this.btnBandVHF.ForeColor = System.Drawing.Color.Gold;
            this.btnBandVHF.Name = "btnBandVHF";
            this.toolTip1.SetToolTip(this.btnBandVHF, resources.GetString("btnBandVHF.ToolTip"));
            this.btnBandVHF.Click += new System.EventHandler(this.btnBandVHF_Click);
            // 
            // chkTXEQ1
            // 
            resources.ApplyResources(this.chkTXEQ1, "chkTXEQ1");
            this.chkTXEQ1.FlatAppearance.BorderSize = 0;
            this.chkTXEQ1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkTXEQ1.Name = "chkTXEQ1";
            this.toolTip1.SetToolTip(this.chkTXEQ1, resources.GetString("chkTXEQ1.ToolTip"));
            this.chkTXEQ1.CheckedChanged += new System.EventHandler(this.chkTXEQ_CheckedChanged);
            this.chkTXEQ1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.chkTXEQ_MouseDown);
            // 
            // chkRXEQ1
            // 
            resources.ApplyResources(this.chkRXEQ1, "chkRXEQ1");
            this.chkRXEQ1.FlatAppearance.BorderSize = 0;
            this.chkRXEQ1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkRXEQ1.Name = "chkRXEQ1";
            this.toolTip1.SetToolTip(this.chkRXEQ1, resources.GetString("chkRXEQ1.ToolTip"));
            this.chkRXEQ1.CheckedChanged += new System.EventHandler(this.chkRXEQ_CheckedChanged);
            this.chkRXEQ1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.chkRXEQ_MouseDown);
            // 
            // udFM1750Timer
            // 
            this.udFM1750Timer.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            resources.ApplyResources(this.udFM1750Timer, "udFM1750Timer");
            this.udFM1750Timer.Maximum = new decimal(new int[] {
            900,
            0,
            0,
            0});
            this.udFM1750Timer.Minimum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.udFM1750Timer.Name = "udFM1750Timer";
            this.toolTip1.SetToolTip(this.udFM1750Timer, resources.GetString("udFM1750Timer.ToolTip"));
            this.udFM1750Timer.Value = new decimal(new int[] {
            400,
            0,
            0,
            0});
            // 
            // chkFM1750
            // 
            resources.ApplyResources(this.chkFM1750, "chkFM1750");
            this.chkFM1750.BackColor = System.Drawing.Color.Black;
            this.chkFM1750.FlatAppearance.BorderSize = 0;
            this.chkFM1750.ForeColor = System.Drawing.Color.White;
            this.chkFM1750.Name = "chkFM1750";
            this.toolTip1.SetToolTip(this.chkFM1750, resources.GetString("chkFM1750.ToolTip"));
            this.chkFM1750.UseVisualStyleBackColor = false;
            this.chkFM1750.Click += new System.EventHandler(this.chkFM1750_Click);
            // 
            // chkFMTXLow
            // 
            resources.ApplyResources(this.chkFMTXLow, "chkFMTXLow");
            this.chkFMTXLow.FlatAppearance.BorderSize = 0;
            this.chkFMTXLow.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkFMTXLow.Name = "chkFMTXLow";
            this.toolTip1.SetToolTip(this.chkFMTXLow, resources.GetString("chkFMTXLow.ToolTip"));
            this.chkFMTXLow.CheckedChanged += new System.EventHandler(this.chkFMTXLow_CheckedChanged);
            this.chkFMTXLow.Click += new System.EventHandler(this.chkFMMode_Click);
            // 
            // btnFMMemory
            // 
            this.btnFMMemory.FlatAppearance.BorderSize = 0;
            resources.ApplyResources(this.btnFMMemory, "btnFMMemory");
            this.btnFMMemory.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnFMMemory.Name = "btnFMMemory";
            this.toolTip1.SetToolTip(this.btnFMMemory, resources.GetString("btnFMMemory.ToolTip"));
            this.btnFMMemory.Click += new System.EventHandler(this.btnFMMemory_Click);
            // 
            // btnFMMemoryUp
            // 
            this.btnFMMemoryUp.FlatAppearance.BorderSize = 0;
            resources.ApplyResources(this.btnFMMemoryUp, "btnFMMemoryUp");
            this.btnFMMemoryUp.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnFMMemoryUp.Name = "btnFMMemoryUp";
            this.toolTip1.SetToolTip(this.btnFMMemoryUp, resources.GetString("btnFMMemoryUp.ToolTip"));
            this.btnFMMemoryUp.Click += new System.EventHandler(this.btnFMMemoryUp_Click);
            // 
            // btnFMMemoryDown
            // 
            this.btnFMMemoryDown.FlatAppearance.BorderSize = 0;
            resources.ApplyResources(this.btnFMMemoryDown, "btnFMMemoryDown");
            this.btnFMMemoryDown.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnFMMemoryDown.Name = "btnFMMemoryDown";
            this.toolTip1.SetToolTip(this.btnFMMemoryDown, resources.GetString("btnFMMemoryDown.ToolTip"));
            this.btnFMMemoryDown.Click += new System.EventHandler(this.btnFMMemoryDown_Click);
            // 
            // radFMDeviation2kHz
            // 
            resources.ApplyResources(this.radFMDeviation2kHz, "radFMDeviation2kHz");
            this.radFMDeviation2kHz.FlatAppearance.BorderSize = 0;
            this.radFMDeviation2kHz.ForeColor = System.Drawing.Color.White;
            this.radFMDeviation2kHz.Name = "radFMDeviation2kHz";
            this.toolTip1.SetToolTip(this.radFMDeviation2kHz, resources.GetString("radFMDeviation2kHz.ToolTip"));
            this.radFMDeviation2kHz.UseVisualStyleBackColor = true;
            this.radFMDeviation2kHz.CheckedChanged += new System.EventHandler(this.radFMDeviation2kHz_CheckedChanged);
            // 
            // udFMOffset
            // 
            this.udFMOffset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.udFMOffset.DecimalPlaces = 3;
            this.udFMOffset.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.udFMOffset.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            resources.ApplyResources(this.udFMOffset, "udFMOffset");
            this.udFMOffset.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.udFMOffset.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.udFMOffset.Name = "udFMOffset";
            this.toolTip1.SetToolTip(this.udFMOffset, resources.GetString("udFMOffset.ToolTip"));
            this.udFMOffset.Value = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.udFMOffset.ValueChanged += new System.EventHandler(this.udFMOffset_ValueChanged);
            // 
            // chkFMTXRev
            // 
            resources.ApplyResources(this.chkFMTXRev, "chkFMTXRev");
            this.chkFMTXRev.FlatAppearance.BorderSize = 0;
            this.chkFMTXRev.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkFMTXRev.Name = "chkFMTXRev";
            this.toolTip1.SetToolTip(this.chkFMTXRev, resources.GetString("chkFMTXRev.ToolTip"));
            this.chkFMTXRev.CheckedChanged += new System.EventHandler(this.chkFMTXRev_CheckedChanged);
            // 
            // radFMDeviation5kHz
            // 
            resources.ApplyResources(this.radFMDeviation5kHz, "radFMDeviation5kHz");
            this.radFMDeviation5kHz.Checked = true;
            this.radFMDeviation5kHz.FlatAppearance.BorderSize = 0;
            this.radFMDeviation5kHz.ForeColor = System.Drawing.Color.White;
            this.radFMDeviation5kHz.Name = "radFMDeviation5kHz";
            this.radFMDeviation5kHz.TabStop = true;
            this.toolTip1.SetToolTip(this.radFMDeviation5kHz, resources.GetString("radFMDeviation5kHz.ToolTip"));
            this.radFMDeviation5kHz.UseVisualStyleBackColor = true;
            this.radFMDeviation5kHz.CheckedChanged += new System.EventHandler(this.radFMDeviation5kHz_CheckedChanged);
            // 
            // comboFMCTCSS
            // 
            this.comboFMCTCSS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboFMCTCSS.DropDownWidth = 60;
            resources.ApplyResources(this.comboFMCTCSS, "comboFMCTCSS");
            this.comboFMCTCSS.Name = "comboFMCTCSS";
            this.toolTip1.SetToolTip(this.comboFMCTCSS, resources.GetString("comboFMCTCSS.ToolTip"));
            this.comboFMCTCSS.SelectedIndexChanged += new System.EventHandler(this.comboFMCTCSS_SelectedIndexChanged);
            // 
            // chkFMCTCSS
            // 
            resources.ApplyResources(this.chkFMCTCSS, "chkFMCTCSS");
            this.chkFMCTCSS.FlatAppearance.BorderSize = 0;
            this.chkFMCTCSS.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkFMCTCSS.Name = "chkFMCTCSS";
            this.toolTip1.SetToolTip(this.chkFMCTCSS, resources.GetString("chkFMCTCSS.ToolTip"));
            this.chkFMCTCSS.CheckedChanged += new System.EventHandler(this.chkFMCTCSS_CheckedChanged);
            // 
            // chkFMTXSimplex
            // 
            resources.ApplyResources(this.chkFMTXSimplex, "chkFMTXSimplex");
            this.chkFMTXSimplex.Checked = true;
            this.chkFMTXSimplex.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkFMTXSimplex.FlatAppearance.BorderSize = 0;
            this.chkFMTXSimplex.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkFMTXSimplex.Name = "chkFMTXSimplex";
            this.toolTip1.SetToolTip(this.chkFMTXSimplex, resources.GetString("chkFMTXSimplex.ToolTip"));
            this.chkFMTXSimplex.CheckedChanged += new System.EventHandler(this.chkFMTXSimplex_CheckedChanged);
            this.chkFMTXSimplex.Click += new System.EventHandler(this.chkFMMode_Click);
            // 
            // chkFMTXHigh
            // 
            resources.ApplyResources(this.chkFMTXHigh, "chkFMTXHigh");
            this.chkFMTXHigh.FlatAppearance.BorderSize = 0;
            this.chkFMTXHigh.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkFMTXHigh.Name = "chkFMTXHigh";
            this.toolTip1.SetToolTip(this.chkFMTXHigh, resources.GetString("chkFMTXHigh.ToolTip"));
            this.chkFMTXHigh.CheckedChanged += new System.EventHandler(this.chkFMTXHigh_CheckedChanged);
            this.chkFMTXHigh.Click += new System.EventHandler(this.chkFMMode_Click);
            // 
            // comboFMTXProfile
            // 
            this.comboFMTXProfile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.comboFMTXProfile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboFMTXProfile.DropDownWidth = 96;
            this.comboFMTXProfile.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            resources.ApplyResources(this.comboFMTXProfile, "comboFMTXProfile");
            this.comboFMTXProfile.Name = "comboFMTXProfile";
            this.toolTip1.SetToolTip(this.comboFMTXProfile, resources.GetString("comboFMTXProfile.ToolTip"));
            this.comboFMTXProfile.SelectedIndexChanged += new System.EventHandler(this.comboFMTXProfile_SelectedIndexChanged);
            this.comboFMTXProfile.MouseDown += new System.Windows.Forms.MouseEventHandler(this.comboFMTXProfile_MouseDown);
            // 
            // checkBoxIICPTT
            // 
            resources.ApplyResources(this.checkBoxIICPTT, "checkBoxIICPTT");
            this.checkBoxIICPTT.BackColor = System.Drawing.Color.Yellow;
            this.checkBoxIICPTT.FlatAppearance.CheckedBackColor = System.Drawing.Color.LimeGreen;
            this.checkBoxIICPTT.Name = "checkBoxIICPTT";
            this.toolTip1.SetToolTip(this.checkBoxIICPTT, resources.GetString("checkBoxIICPTT.ToolTip"));
            this.checkBoxIICPTT.UseVisualStyleBackColor = false;
            this.checkBoxIICPTT.CheckedChanged += new System.EventHandler(this.checkBoxIICPTT_CheckedChanged);
            // 
            // checkBoxIICON
            // 
            resources.ApplyResources(this.checkBoxIICON, "checkBoxIICON");
            this.checkBoxIICON.BackColor = System.Drawing.Color.Orange;
            this.checkBoxIICON.FlatAppearance.CheckedBackColor = System.Drawing.Color.Red;
            this.checkBoxIICON.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkBoxIICON.Name = "checkBoxIICON";
            this.toolTip1.SetToolTip(this.checkBoxIICON, resources.GetString("checkBoxIICON.ToolTip"));
            this.checkBoxIICON.UseVisualStyleBackColor = false;
            this.checkBoxIICON.CheckedChanged += new System.EventHandler(this.checkBoxIICON_CheckedChanged);
            // 
            // chkVAC2
            // 
            resources.ApplyResources(this.chkVAC2, "chkVAC2");
            this.chkVAC2.FlatAppearance.BorderSize = 0;
            this.chkVAC2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkVAC2.Name = "chkVAC2";
            this.toolTip1.SetToolTip(this.chkVAC2, resources.GetString("chkVAC2.ToolTip"));
            this.chkVAC2.CheckedChanged += new System.EventHandler(this.chkVAC2_CheckedChanged);
            this.chkVAC2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.chkVAC2_MouseDown);
            // 
            // btnZeroBeat
            // 
            resources.ApplyResources(this.btnZeroBeat, "btnZeroBeat");
            this.btnZeroBeat.FlatAppearance.BorderSize = 0;
            this.btnZeroBeat.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnZeroBeat.Name = "btnZeroBeat";
            this.toolTip1.SetToolTip(this.btnZeroBeat, resources.GetString("btnZeroBeat.ToolTip"));
            this.btnZeroBeat.Click += new System.EventHandler(this.btnZeroBeat_Click);
            // 
            // chkVFOSplit
            // 
            resources.ApplyResources(this.chkVFOSplit, "chkVFOSplit");
            this.chkVFOSplit.FlatAppearance.BorderSize = 0;
            this.chkVFOSplit.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkVFOSplit.Name = "chkVFOSplit";
            this.toolTip1.SetToolTip(this.chkVFOSplit, resources.GetString("chkVFOSplit.ToolTip"));
            this.chkVFOSplit.CheckedChanged += new System.EventHandler(this.chkVFOSplit_CheckedChanged);
            this.chkVFOSplit.MouseDown += new System.Windows.Forms.MouseEventHandler(this.chkVFOSplit_MouseDown);
            // 
            // btnRITReset
            // 
            this.btnRITReset.FlatAppearance.BorderSize = 0;
            resources.ApplyResources(this.btnRITReset, "btnRITReset");
            this.btnRITReset.Name = "btnRITReset";
            this.toolTip1.SetToolTip(this.btnRITReset, resources.GetString("btnRITReset.ToolTip"));
            this.btnRITReset.Click += new System.EventHandler(this.btnRITReset_Click);
            // 
            // btnXITReset
            // 
            this.btnXITReset.FlatAppearance.BorderSize = 0;
            resources.ApplyResources(this.btnXITReset, "btnXITReset");
            this.btnXITReset.Name = "btnXITReset";
            this.toolTip1.SetToolTip(this.btnXITReset, resources.GetString("btnXITReset.ToolTip"));
            this.btnXITReset.Click += new System.EventHandler(this.btnXITReset_Click);
            // 
            // udRIT
            // 
            this.udRIT.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.udRIT.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.udRIT.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            resources.ApplyResources(this.udRIT, "udRIT");
            this.udRIT.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.udRIT.Minimum = new decimal(new int[] {
            99999,
            0,
            0,
            -2147483648});
            this.udRIT.Name = "udRIT";
            this.toolTip1.SetToolTip(this.udRIT, resources.GetString("udRIT.ToolTip"));
            this.udRIT.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.udRIT.ValueChanged += new System.EventHandler(this.udRIT_ValueChanged);
            this.udRIT.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Console_KeyPress);
            this.udRIT.LostFocus += new System.EventHandler(this.udRIT_LostFocus);
            // 
            // btnIFtoVFO
            // 
            this.btnIFtoVFO.FlatAppearance.BorderSize = 0;
            resources.ApplyResources(this.btnIFtoVFO, "btnIFtoVFO");
            this.btnIFtoVFO.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnIFtoVFO.Name = "btnIFtoVFO";
            this.toolTip1.SetToolTip(this.btnIFtoVFO, resources.GetString("btnIFtoVFO.ToolTip"));
            this.btnIFtoVFO.Click += new System.EventHandler(this.btnIFtoVFO_Click);
            // 
            // chkRIT
            // 
            resources.ApplyResources(this.chkRIT, "chkRIT");
            this.chkRIT.FlatAppearance.BorderSize = 0;
            this.chkRIT.Name = "chkRIT";
            this.toolTip1.SetToolTip(this.chkRIT, resources.GetString("chkRIT.ToolTip"));
            this.chkRIT.CheckedChanged += new System.EventHandler(this.chkRIT_CheckedChanged);
            // 
            // btnVFOSwap
            // 
            this.btnVFOSwap.FlatAppearance.BorderSize = 0;
            resources.ApplyResources(this.btnVFOSwap, "btnVFOSwap");
            this.btnVFOSwap.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnVFOSwap.Name = "btnVFOSwap";
            this.toolTip1.SetToolTip(this.btnVFOSwap, resources.GetString("btnVFOSwap.ToolTip"));
            this.btnVFOSwap.Click += new System.EventHandler(this.btnVFOSwap_Click);
            // 
            // chkXIT
            // 
            resources.ApplyResources(this.chkXIT, "chkXIT");
            this.chkXIT.FlatAppearance.BorderSize = 0;
            this.chkXIT.Name = "chkXIT";
            this.toolTip1.SetToolTip(this.chkXIT, resources.GetString("chkXIT.ToolTip"));
            this.chkXIT.CheckedChanged += new System.EventHandler(this.chkXIT_CheckedChanged);
            this.chkXIT.MouseDown += new System.Windows.Forms.MouseEventHandler(this.chkXIT_MouseDown);
            // 
            // btnVFOBtoA
            // 
            this.btnVFOBtoA.FlatAppearance.BorderSize = 0;
            resources.ApplyResources(this.btnVFOBtoA, "btnVFOBtoA");
            this.btnVFOBtoA.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnVFOBtoA.Name = "btnVFOBtoA";
            this.toolTip1.SetToolTip(this.btnVFOBtoA, resources.GetString("btnVFOBtoA.ToolTip"));
            this.btnVFOBtoA.Click += new System.EventHandler(this.btnVFOBtoA_Click);
            // 
            // udXIT
            // 
            this.udXIT.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.udXIT.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.udXIT.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            resources.ApplyResources(this.udXIT, "udXIT");
            this.udXIT.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.udXIT.Minimum = new decimal(new int[] {
            99999,
            0,
            0,
            -2147483648});
            this.udXIT.Name = "udXIT";
            this.toolTip1.SetToolTip(this.udXIT, resources.GetString("udXIT.ToolTip"));
            this.udXIT.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.udXIT.ValueChanged += new System.EventHandler(this.udXIT_ValueChanged);
            this.udXIT.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Console_KeyPress);
            this.udXIT.LostFocus += new System.EventHandler(this.udXIT_LostFocus);
            // 
            // btnVFOAtoB
            // 
            this.btnVFOAtoB.FlatAppearance.BorderSize = 0;
            resources.ApplyResources(this.btnVFOAtoB, "btnVFOAtoB");
            this.btnVFOAtoB.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnVFOAtoB.Name = "btnVFOAtoB";
            this.toolTip1.SetToolTip(this.btnVFOAtoB, resources.GetString("btnVFOAtoB.ToolTip"));
            this.btnVFOAtoB.Click += new System.EventHandler(this.btnVFOAtoB_Click);
            // 
            // chkVAC1
            // 
            resources.ApplyResources(this.chkVAC1, "chkVAC1");
            this.chkVAC1.FlatAppearance.BorderSize = 0;
            this.chkVAC1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkVAC1.Name = "chkVAC1";
            this.toolTip1.SetToolTip(this.chkVAC1, resources.GetString("chkVAC1.ToolTip"));
            this.chkVAC1.CheckedChanged += new System.EventHandler(this.chkVAC1_CheckedChanged);
            this.chkVAC1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.chkVAC1_MouseDown);
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.richTextBox1, "richTextBox1");
            this.richTextBox1.ForeColor = System.Drawing.Color.White;
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.toolTip1.SetToolTip(this.richTextBox1, resources.GetString("richTextBox1.ToolTip"));
            this.richTextBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panelTS1_MouseClick);
            // 
            // richTextBox2
            // 
            this.richTextBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.richTextBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.richTextBox2, "richTextBox2");
            this.richTextBox2.ForeColor = System.Drawing.Color.White;
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.ReadOnly = true;
            this.toolTip1.SetToolTip(this.richTextBox2, resources.GetString("richTextBox2.ToolTip"));
            this.richTextBox2.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panelTS1_MouseClick);
            // 
            // richTextBox3
            // 
            this.richTextBox3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.richTextBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.richTextBox3, "richTextBox3");
            this.richTextBox3.ForeColor = System.Drawing.Color.White;
            this.richTextBox3.Name = "richTextBox3";
            this.richTextBox3.ReadOnly = true;
            this.toolTip1.SetToolTip(this.richTextBox3, resources.GetString("richTextBox3.ToolTip"));
            this.richTextBox3.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panelTS1_MouseClick);
            // 
            // richTextBox5
            // 
            this.richTextBox5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.richTextBox5.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.richTextBox5, "richTextBox5");
            this.richTextBox5.ForeColor = System.Drawing.Color.White;
            this.richTextBox5.Name = "richTextBox5";
            this.toolTip1.SetToolTip(this.richTextBox5, resources.GetString("richTextBox5.ToolTip"));
            this.richTextBox5.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panelTS1_MouseClick);
            // 
            // richTextBox6
            // 
            this.richTextBox6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.richTextBox6.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.richTextBox6, "richTextBox6");
            this.richTextBox6.ForeColor = System.Drawing.Color.White;
            this.richTextBox6.Name = "richTextBox6";
            this.richTextBox6.ReadOnly = true;
            this.toolTip1.SetToolTip(this.richTextBox6, resources.GetString("richTextBox6.ToolTip"));
            this.richTextBox6.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panelTS1_MouseClick);
            // 
            // richTextBox7
            // 
            this.richTextBox7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.richTextBox7.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.richTextBox7, "richTextBox7");
            this.richTextBox7.ForeColor = System.Drawing.Color.White;
            this.richTextBox7.Name = "richTextBox7";
            this.richTextBox7.ReadOnly = true;
            this.toolTip1.SetToolTip(this.richTextBox7, resources.GetString("richTextBox7.ToolTip"));
            this.richTextBox7.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panelTS1_MouseClick);
            // 
            // richTextBox8
            // 
            this.richTextBox8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.richTextBox8.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.richTextBox8, "richTextBox8");
            this.richTextBox8.ForeColor = System.Drawing.Color.White;
            this.richTextBox8.Name = "richTextBox8";
            this.toolTip1.SetToolTip(this.richTextBox8, resources.GetString("richTextBox8.ToolTip"));
            this.richTextBox8.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panelTS1_MouseClick);
            // 
            // buttonbs
            // 
            this.buttonbs.BackColor = System.Drawing.SystemColors.Desktop;
            this.buttonbs.FlatAppearance.BorderSize = 0;
            resources.ApplyResources(this.buttonbs, "buttonbs");
            this.buttonbs.ForeColor = System.Drawing.Color.White;
            this.buttonbs.Name = "buttonbs";
            this.toolTip1.SetToolTip(this.buttonbs, resources.GetString("buttonbs.ToolTip"));
            this.buttonbs.UseVisualStyleBackColor = false;
            this.buttonbs.MouseUp += new System.Windows.Forms.MouseEventHandler(this.regBox_MouseUp);
            // 
            // chkBoxBS
            // 
            resources.ApplyResources(this.chkBoxBS, "chkBoxBS");
            this.chkBoxBS.Name = "chkBoxBS";
            this.toolTip1.SetToolTip(this.chkBoxBS, resources.GetString("chkBoxBS.ToolTip"));
            // 
            // labelTS5
            // 
            this.labelTS5.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.labelTS5, "labelTS5");
            this.labelTS5.Name = "labelTS5";
            this.toolTip1.SetToolTip(this.labelTS5, resources.GetString("labelTS5.ToolTip"));
            this.labelTS5.MouseUp += new System.Windows.Forms.MouseEventHandler(this.regBox_MouseUp);
            // 
            // regBox1
            // 
            this.regBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.regBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.regBox1, "regBox1");
            this.regBox1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.regBox1.Name = "regBox1";
            this.regBox1.ReadOnly = true;
            this.toolTip1.SetToolTip(this.regBox1, resources.GetString("regBox1.ToolTip"));
            this.regBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.regBox1_MouseDown);
            this.regBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.regBox_MouseUp);
            // 
            // regBox
            // 
            this.regBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.regBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.regBox, "regBox");
            this.regBox.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.regBox.Name = "regBox";
            this.regBox.ReadOnly = true;
            this.toolTip1.SetToolTip(this.regBox, resources.GetString("regBox.ToolTip"));
            this.regBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.regBox1_MouseDown);
            this.regBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.regBox_MouseUp);
            // 
            // lblTuneStep
            // 
            this.lblTuneStep.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            resources.ApplyResources(this.lblTuneStep, "lblTuneStep");
            this.lblTuneStep.Name = "lblTuneStep";
            this.toolTip1.SetToolTip(this.lblTuneStep, resources.GetString("lblTuneStep.ToolTip"));
            this.lblTuneStep.Click += new System.EventHandler(this.lblTuneStep_Click);
            // 
            // chkVFOSync
            // 
            resources.ApplyResources(this.chkVFOSync, "chkVFOSync");
            this.chkVFOSync.FlatAppearance.BorderSize = 0;
            this.chkVFOSync.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkVFOSync.Name = "chkVFOSync";
            this.toolTip1.SetToolTip(this.chkVFOSync, resources.GetString("chkVFOSync.ToolTip"));
            this.chkVFOSync.CheckedChanged += new System.EventHandler(this.chkVFOSync_CheckedChanged);
            // 
            // chkFullDuplex
            // 
            resources.ApplyResources(this.chkFullDuplex, "chkFullDuplex");
            this.chkFullDuplex.FlatAppearance.BorderSize = 0;
            this.chkFullDuplex.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkFullDuplex.Name = "chkFullDuplex";
            this.toolTip1.SetToolTip(this.chkFullDuplex, resources.GetString("chkFullDuplex.ToolTip"));
            this.chkFullDuplex.CheckedChanged += new System.EventHandler(this.chkFullDuplex_CheckedChanged);
            // 
            // btnTuneStepChangeLarger
            // 
            this.btnTuneStepChangeLarger.FlatAppearance.BorderSize = 0;
            resources.ApplyResources(this.btnTuneStepChangeLarger, "btnTuneStepChangeLarger");
            this.btnTuneStepChangeLarger.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnTuneStepChangeLarger.Name = "btnTuneStepChangeLarger";
            this.toolTip1.SetToolTip(this.btnTuneStepChangeLarger, resources.GetString("btnTuneStepChangeLarger.ToolTip"));
            this.btnTuneStepChangeLarger.Click += new System.EventHandler(this.btnChangeTuneStepLarger_Click);
            // 
            // btnTuneStepChangeSmaller
            // 
            this.btnTuneStepChangeSmaller.FlatAppearance.BorderSize = 0;
            resources.ApplyResources(this.btnTuneStepChangeSmaller, "btnTuneStepChangeSmaller");
            this.btnTuneStepChangeSmaller.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnTuneStepChangeSmaller.Name = "btnTuneStepChangeSmaller";
            this.toolTip1.SetToolTip(this.btnTuneStepChangeSmaller, resources.GetString("btnTuneStepChangeSmaller.ToolTip"));
            this.btnTuneStepChangeSmaller.Click += new System.EventHandler(this.btnChangeTuneStepSmaller_Click);
            // 
            // chkVFOLock
            // 
            resources.ApplyResources(this.chkVFOLock, "chkVFOLock");
            this.chkVFOLock.FlatAppearance.BorderSize = 0;
            this.chkVFOLock.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkVFOLock.Name = "chkVFOLock";
            this.toolTip1.SetToolTip(this.chkVFOLock, resources.GetString("chkVFOLock.ToolTip"));
            this.chkVFOLock.CheckedChanged += new System.EventHandler(this.chkVFOLock_CheckedChanged);
            // 
            // txtWheelTune
            // 
            this.txtWheelTune.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.txtWheelTune.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.txtWheelTune, "txtWheelTune");
            this.txtWheelTune.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.txtWheelTune.Name = "txtWheelTune";
            this.txtWheelTune.ReadOnly = true;
            this.toolTip1.SetToolTip(this.txtWheelTune, resources.GetString("txtWheelTune.ToolTip"));
            this.txtWheelTune.TextChanged += new System.EventHandler(this.txtWheelTune_TextChanged);
            this.txtWheelTune.GotFocus += new System.EventHandler(this.HideFocus);
            this.txtWheelTune.MouseDown += new System.Windows.Forms.MouseEventHandler(this.WheelTune_MouseDown);
            // 
            // btnMemoryQuickRestore
            // 
            this.btnMemoryQuickRestore.FlatAppearance.BorderSize = 0;
            resources.ApplyResources(this.btnMemoryQuickRestore, "btnMemoryQuickRestore");
            this.btnMemoryQuickRestore.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnMemoryQuickRestore.Name = "btnMemoryQuickRestore";
            this.toolTip1.SetToolTip(this.btnMemoryQuickRestore, resources.GetString("btnMemoryQuickRestore.ToolTip"));
            this.btnMemoryQuickRestore.Click += new System.EventHandler(this.btnMemoryQuickRestore_Click);
            // 
            // btnMemoryQuickSave
            // 
            this.btnMemoryQuickSave.FlatAppearance.BorderSize = 0;
            resources.ApplyResources(this.btnMemoryQuickSave, "btnMemoryQuickSave");
            this.btnMemoryQuickSave.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnMemoryQuickSave.Name = "btnMemoryQuickSave";
            this.toolTip1.SetToolTip(this.btnMemoryQuickSave, resources.GetString("btnMemoryQuickSave.ToolTip"));
            this.btnMemoryQuickSave.Click += new System.EventHandler(this.btnMemoryQuickSave_Click);
            // 
            // txtMemoryQuick
            // 
            this.txtMemoryQuick.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.txtMemoryQuick.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.txtMemoryQuick, "txtMemoryQuick");
            this.txtMemoryQuick.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.txtMemoryQuick.Name = "txtMemoryQuick";
            this.txtMemoryQuick.ReadOnly = true;
            this.toolTip1.SetToolTip(this.txtMemoryQuick, resources.GetString("txtMemoryQuick.ToolTip"));
            // 
            // chkVFOBTX
            // 
            resources.ApplyResources(this.chkVFOBTX, "chkVFOBTX");
            this.chkVFOBTX.FlatAppearance.BorderSize = 0;
            this.chkVFOBTX.Name = "chkVFOBTX";
            this.toolTip1.SetToolTip(this.chkVFOBTX, resources.GetString("chkVFOBTX.ToolTip"));
            this.chkVFOBTX.CheckedChanged += new System.EventHandler(this.chkVFOBTX_CheckedChanged);
            // 
            // chkVFOATX
            // 
            resources.ApplyResources(this.chkVFOATX, "chkVFOATX");
            this.chkVFOATX.BackColor = System.Drawing.Color.Transparent;
            this.chkVFOATX.Checked = true;
            this.chkVFOATX.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkVFOATX.FlatAppearance.BorderSize = 0;
            this.chkVFOATX.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkVFOATX.Name = "chkVFOATX";
            this.toolTip1.SetToolTip(this.chkVFOATX, resources.GetString("chkVFOATX.ToolTip"));
            this.chkVFOATX.UseVisualStyleBackColor = false;
            this.chkVFOATX.CheckedChanged += new System.EventHandler(this.chkVFOATX_CheckedChanged);
            // 
            // txtVFOABand
            // 
            this.txtVFOABand.BackColor = System.Drawing.Color.Black;
            this.txtVFOABand.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.txtVFOABand, "txtVFOABand");
            this.txtVFOABand.ForeColor = System.Drawing.Color.Green;
            this.txtVFOABand.Name = "txtVFOABand";
            this.txtVFOABand.ReadOnly = true;
            this.txtVFOABand.ShortcutsEnabled = false;
            this.toolTip1.SetToolTip(this.txtVFOABand, resources.GetString("txtVFOABand.ToolTip"));
            this.txtVFOABand.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtVFOABand_KeyPress);
            this.txtVFOABand.LostFocus += new System.EventHandler(this.txtVFOABand_LostFocus);
            this.txtVFOABand.MouseLeave += new System.EventHandler(this.txtVFOABand_MouseLeave);
            this.txtVFOABand.MouseMove += new System.Windows.Forms.MouseEventHandler(this.txtVFOABand_MouseMove);
            this.txtVFOABand.MouseUp += new System.Windows.Forms.MouseEventHandler(this.txtVFOABand_MouseUp);
            // 
            // pictureBox1
            // 
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox1, resources.GetString("pictureBox1.ToolTip"));
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.button1_MouseDown);
            // 
            // btnTNFAdd
            // 
            this.btnTNFAdd.FlatAppearance.BorderSize = 0;
            resources.ApplyResources(this.btnTNFAdd, "btnTNFAdd");
            this.btnTNFAdd.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnTNFAdd.Name = "btnTNFAdd";
            this.toolTip1.SetToolTip(this.btnTNFAdd, resources.GetString("btnTNFAdd.ToolTip"));
            this.btnTNFAdd.Click += new System.EventHandler(this.btnTNFAdd_Click);
            this.btnTNFAdd.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnTNFAdd_MouseDown);
            // 
            // chkTNF
            // 
            resources.ApplyResources(this.chkTNF, "chkTNF");
            this.chkTNF.Checked = true;
            this.chkTNF.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTNF.FlatAppearance.BorderSize = 0;
            this.chkTNF.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkTNF.Name = "chkTNF";
            this.toolTip1.SetToolTip(this.chkTNF, resources.GetString("chkTNF.ToolTip"));
            this.chkTNF.CheckedChanged += new System.EventHandler(this.chkTNF_CheckedChanged);
            this.chkTNF.MouseDown += new System.Windows.Forms.MouseEventHandler(this.chkTNF_MouseDown);
            // 
            // chkDisplayPeak
            // 
            resources.ApplyResources(this.chkDisplayPeak, "chkDisplayPeak");
            this.chkDisplayPeak.FlatAppearance.BorderSize = 0;
            this.chkDisplayPeak.Name = "chkDisplayPeak";
            this.toolTip1.SetToolTip(this.chkDisplayPeak, resources.GetString("chkDisplayPeak.ToolTip"));
            this.chkDisplayPeak.CheckedChanged += new System.EventHandler(this.chkDisplayPeak_CheckedChanged);
            // 
            // comboDisplayMode
            // 
            this.comboDisplayMode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.comboDisplayMode.DisplayMember = "0";
            this.comboDisplayMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboDisplayMode.DropDownWidth = 88;
            resources.ApplyResources(this.comboDisplayMode, "comboDisplayMode");
            this.comboDisplayMode.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.comboDisplayMode.Name = "comboDisplayMode";
            this.toolTip1.SetToolTip(this.comboDisplayMode, resources.GetString("comboDisplayMode.ToolTip"));
            this.comboDisplayMode.SelectedIndexChanged += new System.EventHandler(this.comboDisplayMode_SelectedIndexChanged);
            this.comboDisplayMode.MouseUp += new System.Windows.Forms.MouseEventHandler(this.comboDisplayMode_MouseUp);
            // 
            // chkDisplayAVG
            // 
            resources.ApplyResources(this.chkDisplayAVG, "chkDisplayAVG");
            this.chkDisplayAVG.FlatAppearance.BorderSize = 0;
            this.chkDisplayAVG.Name = "chkDisplayAVG";
            this.toolTip1.SetToolTip(this.chkDisplayAVG, resources.GetString("chkDisplayAVG.ToolTip"));
            this.chkDisplayAVG.CheckedChanged += new System.EventHandler(this.chkDisplayAVG_CheckedChanged);
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label6.Name = "label6";
            this.toolTip1.SetToolTip(this.label6, resources.GetString("label6.ToolTip"));
            this.label6.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label6_MouseDown);
            // 
            // ptbRX2RF
            // 
            this.ptbRX2RF.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.ptbRX2RF, "ptbRX2RF");
            this.ptbRX2RF.HeadImage = null;
            this.ptbRX2RF.LargeChange = 1;
            this.ptbRX2RF.Maximum = 120;
            this.ptbRX2RF.Minimum = -20;
            this.ptbRX2RF.Name = "ptbRX2RF";
            this.ptbRX2RF.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.ptbRX2RF.SmallChange = 1;
            this.ptbRX2RF.TabStop = false;
            this.toolTip1.SetToolTip(this.ptbRX2RF, resources.GetString("ptbRX2RF.ToolTip"));
            this.ptbRX2RF.Value = 90;
            this.ptbRX2RF.Scroll += new PowerSDR.PrettyTrackBar.ScrollHandler(this.ptbRX2RF_Scroll);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label1.Name = "label1";
            this.toolTip1.SetToolTip(this.label1, resources.GetString("label1.ToolTip"));
            // 
            // checkBoxID
            // 
            resources.ApplyResources(this.checkBoxID, "checkBoxID");
            this.checkBoxID.Name = "checkBoxID";
            this.toolTip1.SetToolTip(this.checkBoxID, resources.GetString("checkBoxID.ToolTip"));
            this.checkBoxID.CheckedChanged += new System.EventHandler(this.checkBoxID_CheckedChanged);
            // 
            // chkFWCATU
            // 
            resources.ApplyResources(this.chkFWCATU, "chkFWCATU");
            this.chkFWCATU.FlatAppearance.BorderSize = 0;
            this.chkFWCATU.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkFWCATU.Name = "chkFWCATU";
            this.toolTip1.SetToolTip(this.chkFWCATU, resources.GetString("chkFWCATU.ToolTip"));
            this.chkFWCATU.CheckedChanged += new System.EventHandler(this.chkFWCATU_CheckedChanged);
            this.chkFWCATU.Click += new System.EventHandler(this.chkFWCATU_Click);
            // 
            // chkFWCATUBypass
            // 
            resources.ApplyResources(this.chkFWCATUBypass, "chkFWCATUBypass");
            this.chkFWCATUBypass.FlatAppearance.BorderSize = 0;
            this.chkFWCATUBypass.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkFWCATUBypass.Name = "chkFWCATUBypass";
            this.toolTip1.SetToolTip(this.chkFWCATUBypass, resources.GetString("chkFWCATUBypass.ToolTip"));
            this.chkFWCATUBypass.Click += new System.EventHandler(this.chkFWCATUBypass_Click);
            // 
            // ckQuickPlay
            // 
            resources.ApplyResources(this.ckQuickPlay, "ckQuickPlay");
            this.ckQuickPlay.FlatAppearance.BorderSize = 0;
            this.ckQuickPlay.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.ckQuickPlay.Name = "ckQuickPlay";
            this.toolTip1.SetToolTip(this.ckQuickPlay, resources.GetString("ckQuickPlay.ToolTip"));
            this.ckQuickPlay.CheckedChanged += new System.EventHandler(this.ckQuickPlay_CheckedChanged);
            this.ckQuickPlay.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ckQuickPlay_MouseDown);
            // 
            // chkMON
            // 
            resources.ApplyResources(this.chkMON, "chkMON");
            this.chkMON.FlatAppearance.BorderSize = 0;
            this.chkMON.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkMON.Name = "chkMON";
            this.toolTip1.SetToolTip(this.chkMON, resources.GetString("chkMON.ToolTip"));
            this.chkMON.CheckedChanged += new System.EventHandler(this.chkMON_CheckedChanged);
            // 
            // ckQuickRec
            // 
            resources.ApplyResources(this.ckQuickRec, "ckQuickRec");
            this.ckQuickRec.FlatAppearance.BorderSize = 0;
            this.ckQuickRec.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.ckQuickRec.Name = "ckQuickRec";
            this.toolTip1.SetToolTip(this.ckQuickRec, resources.GetString("ckQuickRec.ToolTip"));
            this.ckQuickRec.CheckedChanged += new System.EventHandler(this.ckQuickRec_CheckedChanged);
            this.ckQuickRec.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ckQuickRec_MouseDown);
            // 
            // chkMUT
            // 
            resources.ApplyResources(this.chkMUT, "chkMUT");
            this.chkMUT.FlatAppearance.BorderSize = 0;
            this.chkMUT.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkMUT.Name = "chkMUT";
            this.toolTip1.SetToolTip(this.chkMUT, resources.GetString("chkMUT.ToolTip"));
            this.chkMUT.CheckedChanged += new System.EventHandler(this.chkMUT_CheckedChanged);
            this.chkMUT.MouseDown += new System.Windows.Forms.MouseEventHandler(this.chkMUT_MouseDown);
            // 
            // chkMOX
            // 
            resources.ApplyResources(this.chkMOX, "chkMOX");
            this.chkMOX.FlatAppearance.BorderSize = 0;
            this.chkMOX.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkMOX.Name = "chkMOX";
            this.toolTip1.SetToolTip(this.chkMOX, resources.GetString("chkMOX.ToolTip"));
            this.chkMOX.CheckedChanged += new System.EventHandler(this.chkMOX_CheckedChanged2);
            this.chkMOX.Click += new System.EventHandler(this.chkMOX_Click);
            // 
            // chkTUN
            // 
            resources.ApplyResources(this.chkTUN, "chkTUN");
            this.chkTUN.FlatAppearance.BorderSize = 0;
            this.chkTUN.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkTUN.Name = "chkTUN";
            this.toolTip1.SetToolTip(this.chkTUN, resources.GetString("chkTUN.ToolTip"));
            this.chkTUN.CheckedChanged += new System.EventHandler(this.chkTUN_CheckedChanged);
            this.chkTUN.MouseDown += new System.Windows.Forms.MouseEventHandler(this.chkTUN_MouseDown);
            // 
            // chkX2TR
            // 
            resources.ApplyResources(this.chkX2TR, "chkX2TR");
            this.chkX2TR.FlatAppearance.BorderSize = 0;
            this.chkX2TR.Name = "chkX2TR";
            this.toolTip1.SetToolTip(this.chkX2TR, resources.GetString("chkX2TR.ToolTip"));
            this.chkX2TR.CheckedChanged += new System.EventHandler(this.chkX2TR_CheckedChanged);
            // 
            // comboTuneMode
            // 
            this.comboTuneMode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.comboTuneMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboTuneMode.DropDownWidth = 42;
            resources.ApplyResources(this.comboTuneMode, "comboTuneMode");
            this.comboTuneMode.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.comboTuneMode.Items.AddRange(new object[] {
            resources.GetString("comboTuneMode.Items"),
            resources.GetString("comboTuneMode.Items1"),
            resources.GetString("comboTuneMode.Items2")});
            this.comboTuneMode.Name = "comboTuneMode";
            this.toolTip1.SetToolTip(this.comboTuneMode, resources.GetString("comboTuneMode.ToolTip"));
            this.comboTuneMode.SelectedIndexChanged += new System.EventHandler(this.comboTuneMode_SelectedIndexChanged);
            // 
            // udCWPitch
            // 
            this.udCWPitch.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            resources.ApplyResources(this.udCWPitch, "udCWPitch");
            this.udCWPitch.Maximum = new decimal(new int[] {
            2250,
            0,
            0,
            0});
            this.udCWPitch.Minimum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.udCWPitch.Name = "udCWPitch";
            this.toolTip1.SetToolTip(this.udCWPitch, resources.GetString("udCWPitch.ToolTip"));
            this.udCWPitch.Value = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.udCWPitch.ValueChanged += new System.EventHandler(this.udCWPitch_ValueChanged);
            // 
            // udCWBreakInDelay
            // 
            this.udCWBreakInDelay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.udCWBreakInDelay.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.udCWBreakInDelay.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            resources.ApplyResources(this.udCWBreakInDelay, "udCWBreakInDelay");
            this.udCWBreakInDelay.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.udCWBreakInDelay.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.udCWBreakInDelay.Name = "udCWBreakInDelay";
            this.toolTip1.SetToolTip(this.udCWBreakInDelay, resources.GetString("udCWBreakInDelay.ToolTip"));
            this.udCWBreakInDelay.Value = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.udCWBreakInDelay.ValueChanged += new System.EventHandler(this.udCWBreakInDelay_ValueChanged);
            this.udCWBreakInDelay.LostFocus += new System.EventHandler(this.udCWBreakInDelay_LostFocus);
            // 
            // chkCWBreakInEnabled
            // 
            this.chkCWBreakInEnabled.Checked = true;
            this.chkCWBreakInEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            resources.ApplyResources(this.chkCWBreakInEnabled, "chkCWBreakInEnabled");
            this.chkCWBreakInEnabled.ForeColor = System.Drawing.Color.White;
            this.chkCWBreakInEnabled.Name = "chkCWBreakInEnabled";
            this.toolTip1.SetToolTip(this.chkCWBreakInEnabled, resources.GetString("chkCWBreakInEnabled.ToolTip"));
            this.chkCWBreakInEnabled.CheckedChanged += new System.EventHandler(this.chkCWBreakInEnabled_CheckedChanged);
            // 
            // chkShowTXCWFreq
            // 
            this.chkShowTXCWFreq.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.chkShowTXCWFreq, "chkShowTXCWFreq");
            this.chkShowTXCWFreq.Name = "chkShowTXCWFreq";
            this.toolTip1.SetToolTip(this.chkShowTXCWFreq, resources.GetString("chkShowTXCWFreq.ToolTip"));
            this.chkShowTXCWFreq.CheckedChanged += new System.EventHandler(this.chkShowTXCWFreq_CheckedChanged);
            // 
            // chkCWSidetone
            // 
            this.chkCWSidetone.Checked = true;
            this.chkCWSidetone.CheckState = System.Windows.Forms.CheckState.Checked;
            resources.ApplyResources(this.chkCWSidetone, "chkCWSidetone");
            this.chkCWSidetone.ForeColor = System.Drawing.Color.White;
            this.chkCWSidetone.Name = "chkCWSidetone";
            this.toolTip1.SetToolTip(this.chkCWSidetone, resources.GetString("chkCWSidetone.ToolTip"));
            this.chkCWSidetone.CheckedChanged += new System.EventHandler(this.chkCWSidetone_CheckedChanged);
            // 
            // chkCWIambic
            // 
            this.chkCWIambic.Checked = true;
            this.chkCWIambic.CheckState = System.Windows.Forms.CheckState.Checked;
            resources.ApplyResources(this.chkCWIambic, "chkCWIambic");
            this.chkCWIambic.ForeColor = System.Drawing.Color.White;
            this.chkCWIambic.Name = "chkCWIambic";
            this.toolTip1.SetToolTip(this.chkCWIambic, resources.GetString("chkCWIambic.ToolTip"));
            this.chkCWIambic.CheckedChanged += new System.EventHandler(this.chkCWIambic_CheckedChanged);
            // 
            // chkShowTXFilter
            // 
            this.chkShowTXFilter.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.chkShowTXFilter, "chkShowTXFilter");
            this.chkShowTXFilter.Name = "chkShowTXFilter";
            this.toolTip1.SetToolTip(this.chkShowTXFilter, resources.GetString("chkShowTXFilter.ToolTip"));
            this.chkShowTXFilter.CheckedChanged += new System.EventHandler(this.chkShowTXFilter_CheckedChanged);
            // 
            // chkShowDigTXFilter
            // 
            this.chkShowDigTXFilter.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.chkShowDigTXFilter, "chkShowDigTXFilter");
            this.chkShowDigTXFilter.Name = "chkShowDigTXFilter";
            this.toolTip1.SetToolTip(this.chkShowDigTXFilter, resources.GetString("chkShowDigTXFilter.ToolTip"));
            this.chkShowDigTXFilter.CheckedChanged += new System.EventHandler(this.chkShowDigTXFilter_CheckedChanged);
            // 
            // chkDX
            // 
            resources.ApplyResources(this.chkDX, "chkDX");
            this.chkDX.FlatAppearance.BorderSize = 0;
            this.chkDX.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkDX.Name = "chkDX";
            this.toolTip1.SetToolTip(this.chkDX, resources.GetString("chkDX.ToolTip"));
            this.chkDX.CheckedChanged += new System.EventHandler(this.chkDX_CheckedChanged);
            // 
            // chkTXEQ
            // 
            resources.ApplyResources(this.chkTXEQ, "chkTXEQ");
            this.chkTXEQ.FlatAppearance.BorderSize = 0;
            this.chkTXEQ.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkTXEQ.Name = "chkTXEQ";
            this.toolTip1.SetToolTip(this.chkTXEQ, resources.GetString("chkTXEQ.ToolTip"));
            this.chkTXEQ.CheckedChanged += new System.EventHandler(this.chkTXEQ_CheckedChanged);
            this.chkTXEQ.MouseDown += new System.Windows.Forms.MouseEventHandler(this.chkTXEQ_MouseDown);
            // 
            // comboTXProfile
            // 
            this.comboTXProfile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.comboTXProfile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboTXProfile.DropDownWidth = 96;
            resources.ApplyResources(this.comboTXProfile, "comboTXProfile");
            this.comboTXProfile.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.comboTXProfile.Name = "comboTXProfile";
            this.toolTip1.SetToolTip(this.comboTXProfile, resources.GetString("comboTXProfile.ToolTip"));
            this.comboTXProfile.SelectedIndexChanged += new System.EventHandler(this.comboTXProfile_SelectedIndexChanged);
            this.comboTXProfile.MouseDown += new System.Windows.Forms.MouseEventHandler(this.comboTXProfile_MouseDown);
            // 
            // chkRXEQ
            // 
            resources.ApplyResources(this.chkRXEQ, "chkRXEQ");
            this.chkRXEQ.FlatAppearance.BorderSize = 0;
            this.chkRXEQ.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkRXEQ.Name = "chkRXEQ";
            this.toolTip1.SetToolTip(this.chkRXEQ, resources.GetString("chkRXEQ.ToolTip"));
            this.chkRXEQ.CheckedChanged += new System.EventHandler(this.chkRXEQ_CheckedChanged);
            this.chkRXEQ.MouseDown += new System.Windows.Forms.MouseEventHandler(this.chkRXEQ_MouseDown);
            // 
            // chkCPDR
            // 
            resources.ApplyResources(this.chkCPDR, "chkCPDR");
            this.chkCPDR.FlatAppearance.BorderSize = 0;
            this.chkCPDR.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkCPDR.Name = "chkCPDR";
            this.toolTip1.SetToolTip(this.chkCPDR, resources.GetString("chkCPDR.ToolTip"));
            this.chkCPDR.CheckedChanged += new System.EventHandler(this.chkCPDR_CheckedChanged);
            // 
            // chkVOX
            // 
            resources.ApplyResources(this.chkVOX, "chkVOX");
            this.chkVOX.FlatAppearance.BorderSize = 0;
            this.chkVOX.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkVOX.Name = "chkVOX";
            this.toolTip1.SetToolTip(this.chkVOX, resources.GetString("chkVOX.ToolTip"));
            this.chkVOX.CheckedChanged += new System.EventHandler(this.chkVOX_CheckedChanged);
            this.chkVOX.MouseDown += new System.Windows.Forms.MouseEventHandler(this.chkVOX_MouseDown);
            // 
            // chkNoiseGate
            // 
            resources.ApplyResources(this.chkNoiseGate, "chkNoiseGate");
            this.chkNoiseGate.FlatAppearance.BorderSize = 0;
            this.chkNoiseGate.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkNoiseGate.Name = "chkNoiseGate";
            this.toolTip1.SetToolTip(this.chkNoiseGate, resources.GetString("chkNoiseGate.ToolTip"));
            this.chkNoiseGate.CheckedChanged += new System.EventHandler(this.chkNoiseGate_CheckedChanged);
            // 
            // udRX2FilterHigh
            // 
            this.udRX2FilterHigh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            resources.ApplyResources(this.udRX2FilterHigh, "udRX2FilterHigh");
            this.udRX2FilterHigh.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.udRX2FilterHigh.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.udRX2FilterHigh.Maximum = new decimal(new int[] {
            52000,
            0,
            0,
            0});
            this.udRX2FilterHigh.Minimum = new decimal(new int[] {
            52000,
            0,
            0,
            -2147483648});
            this.udRX2FilterHigh.Name = "udRX2FilterHigh";
            this.toolTip1.SetToolTip(this.udRX2FilterHigh, resources.GetString("udRX2FilterHigh.ToolTip"));
            this.udRX2FilterHigh.Value = new decimal(new int[] {
            6000,
            0,
            0,
            0});
            this.udRX2FilterHigh.ValueChanged += new System.EventHandler(this.udRX2FilterHigh_ValueChanged);
            this.udRX2FilterHigh.MouseDown += new System.Windows.Forms.MouseEventHandler(this.udRX2FilterHigh_MouseDown);
            // 
            // udRX2FilterLow
            // 
            this.udRX2FilterLow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            resources.ApplyResources(this.udRX2FilterLow, "udRX2FilterLow");
            this.udRX2FilterLow.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.udRX2FilterLow.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.udRX2FilterLow.Maximum = new decimal(new int[] {
            52000,
            0,
            0,
            0});
            this.udRX2FilterLow.Minimum = new decimal(new int[] {
            52000,
            0,
            0,
            -2147483648});
            this.udRX2FilterLow.Name = "udRX2FilterLow";
            this.toolTip1.SetToolTip(this.udRX2FilterLow, resources.GetString("udRX2FilterLow.ToolTip"));
            this.udRX2FilterLow.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.udRX2FilterLow.ValueChanged += new System.EventHandler(this.udRX2FilterLow_ValueChanged);
            this.udRX2FilterLow.MouseDown += new System.Windows.Forms.MouseEventHandler(this.udRX2FilterLow_MouseDown);
            // 
            // radRX2ModeAM
            // 
            resources.ApplyResources(this.radRX2ModeAM, "radRX2ModeAM");
            this.radRX2ModeAM.FlatAppearance.BorderSize = 0;
            this.radRX2ModeAM.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radRX2ModeAM.Name = "radRX2ModeAM";
            this.toolTip1.SetToolTip(this.radRX2ModeAM, resources.GetString("radRX2ModeAM.ToolTip"));
            this.radRX2ModeAM.CheckedChanged += new System.EventHandler(this.radRX2ModeAM_CheckedChanged);
            // 
            // radRX2ModeLSB
            // 
            resources.ApplyResources(this.radRX2ModeLSB, "radRX2ModeLSB");
            this.radRX2ModeLSB.FlatAppearance.BorderSize = 0;
            this.radRX2ModeLSB.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radRX2ModeLSB.Name = "radRX2ModeLSB";
            this.toolTip1.SetToolTip(this.radRX2ModeLSB, resources.GetString("radRX2ModeLSB.ToolTip"));
            this.radRX2ModeLSB.CheckedChanged += new System.EventHandler(this.radRX2ModeLSB_CheckedChanged);
            // 
            // radRX2ModeSAM
            // 
            resources.ApplyResources(this.radRX2ModeSAM, "radRX2ModeSAM");
            this.radRX2ModeSAM.FlatAppearance.BorderSize = 0;
            this.radRX2ModeSAM.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radRX2ModeSAM.Name = "radRX2ModeSAM";
            this.toolTip1.SetToolTip(this.radRX2ModeSAM, resources.GetString("radRX2ModeSAM.ToolTip"));
            this.radRX2ModeSAM.CheckedChanged += new System.EventHandler(this.radRX2ModeSAM_CheckedChanged);
            // 
            // radRX2ModeCWL
            // 
            resources.ApplyResources(this.radRX2ModeCWL, "radRX2ModeCWL");
            this.radRX2ModeCWL.FlatAppearance.BorderSize = 0;
            this.radRX2ModeCWL.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radRX2ModeCWL.Name = "radRX2ModeCWL";
            this.toolTip1.SetToolTip(this.radRX2ModeCWL, resources.GetString("radRX2ModeCWL.ToolTip"));
            this.radRX2ModeCWL.CheckedChanged += new System.EventHandler(this.radRX2ModeCWL_CheckedChanged);
            this.radRX2ModeCWL.MouseDown += new System.Windows.Forms.MouseEventHandler(this.radRX2ModeCWL_MouseDown);
            // 
            // radRX2ModeDSB
            // 
            resources.ApplyResources(this.radRX2ModeDSB, "radRX2ModeDSB");
            this.radRX2ModeDSB.FlatAppearance.BorderSize = 0;
            this.radRX2ModeDSB.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radRX2ModeDSB.Name = "radRX2ModeDSB";
            this.toolTip1.SetToolTip(this.radRX2ModeDSB, resources.GetString("radRX2ModeDSB.ToolTip"));
            this.radRX2ModeDSB.CheckedChanged += new System.EventHandler(this.radRX2ModeDSB_CheckedChanged);
            // 
            // radRX2ModeUSB
            // 
            resources.ApplyResources(this.radRX2ModeUSB, "radRX2ModeUSB");
            this.radRX2ModeUSB.BackColor = System.Drawing.SystemColors.Control;
            this.radRX2ModeUSB.FlatAppearance.BorderSize = 0;
            this.radRX2ModeUSB.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radRX2ModeUSB.Name = "radRX2ModeUSB";
            this.toolTip1.SetToolTip(this.radRX2ModeUSB, resources.GetString("radRX2ModeUSB.ToolTip"));
            this.radRX2ModeUSB.UseVisualStyleBackColor = false;
            this.radRX2ModeUSB.CheckedChanged += new System.EventHandler(this.radRX2ModeUSB_CheckedChanged);
            // 
            // radRX2ModeCWU
            // 
            resources.ApplyResources(this.radRX2ModeCWU, "radRX2ModeCWU");
            this.radRX2ModeCWU.FlatAppearance.BorderSize = 0;
            this.radRX2ModeCWU.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radRX2ModeCWU.Name = "radRX2ModeCWU";
            this.toolTip1.SetToolTip(this.radRX2ModeCWU, resources.GetString("radRX2ModeCWU.ToolTip"));
            this.radRX2ModeCWU.CheckedChanged += new System.EventHandler(this.radRX2ModeCWU_CheckedChanged);
            this.radRX2ModeCWU.MouseDown += new System.Windows.Forms.MouseEventHandler(this.radRX2ModeCWU_MouseDown);
            // 
            // radRX2ModeFMN
            // 
            resources.ApplyResources(this.radRX2ModeFMN, "radRX2ModeFMN");
            this.radRX2ModeFMN.FlatAppearance.BorderSize = 0;
            this.radRX2ModeFMN.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radRX2ModeFMN.Name = "radRX2ModeFMN";
            this.toolTip1.SetToolTip(this.radRX2ModeFMN, resources.GetString("radRX2ModeFMN.ToolTip"));
            this.radRX2ModeFMN.CheckedChanged += new System.EventHandler(this.radRX2ModeFMN_CheckedChanged);
            this.radRX2ModeFMN.MouseDown += new System.Windows.Forms.MouseEventHandler(this.radRX2ModeFMN_MouseUp);
            // 
            // radRX2ModeDIGU
            // 
            resources.ApplyResources(this.radRX2ModeDIGU, "radRX2ModeDIGU");
            this.radRX2ModeDIGU.FlatAppearance.BorderSize = 0;
            this.radRX2ModeDIGU.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radRX2ModeDIGU.Name = "radRX2ModeDIGU";
            this.toolTip1.SetToolTip(this.radRX2ModeDIGU, resources.GetString("radRX2ModeDIGU.ToolTip"));
            this.radRX2ModeDIGU.CheckedChanged += new System.EventHandler(this.radRX2ModeDIGU_CheckedChanged);
            // 
            // radRX2ModeDRM
            // 
            resources.ApplyResources(this.radRX2ModeDRM, "radRX2ModeDRM");
            this.radRX2ModeDRM.FlatAppearance.BorderSize = 0;
            this.radRX2ModeDRM.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radRX2ModeDRM.Name = "radRX2ModeDRM";
            this.toolTip1.SetToolTip(this.radRX2ModeDRM, resources.GetString("radRX2ModeDRM.ToolTip"));
            this.radRX2ModeDRM.CheckedChanged += new System.EventHandler(this.radRX2ModeDRM_CheckedChanged);
            // 
            // radRX2ModeDIGL
            // 
            resources.ApplyResources(this.radRX2ModeDIGL, "radRX2ModeDIGL");
            this.radRX2ModeDIGL.FlatAppearance.BorderSize = 0;
            this.radRX2ModeDIGL.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radRX2ModeDIGL.Name = "radRX2ModeDIGL";
            this.toolTip1.SetToolTip(this.radRX2ModeDIGL, resources.GetString("radRX2ModeDIGL.ToolTip"));
            this.radRX2ModeDIGL.CheckedChanged += new System.EventHandler(this.radRX2ModeDIGL_CheckedChanged);
            // 
            // radRX2ModeSPEC
            // 
            resources.ApplyResources(this.radRX2ModeSPEC, "radRX2ModeSPEC");
            this.radRX2ModeSPEC.FlatAppearance.BorderSize = 0;
            this.radRX2ModeSPEC.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radRX2ModeSPEC.Name = "radRX2ModeSPEC";
            this.toolTip1.SetToolTip(this.radRX2ModeSPEC, resources.GetString("radRX2ModeSPEC.ToolTip"));
            // 
            // chkRX2DisplayPeak
            // 
            resources.ApplyResources(this.chkRX2DisplayPeak, "chkRX2DisplayPeak");
            this.chkRX2DisplayPeak.FlatAppearance.BorderSize = 0;
            this.chkRX2DisplayPeak.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkRX2DisplayPeak.Name = "chkRX2DisplayPeak";
            this.toolTip1.SetToolTip(this.chkRX2DisplayPeak, resources.GetString("chkRX2DisplayPeak.ToolTip"));
            this.chkRX2DisplayPeak.CheckedChanged += new System.EventHandler(this.chkRX2DisplayPeak_CheckedChanged);
            // 
            // comboRX2DisplayMode
            // 
            this.comboRX2DisplayMode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.comboRX2DisplayMode.DisplayMember = "0";
            this.comboRX2DisplayMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboRX2DisplayMode.DropDownWidth = 88;
            resources.ApplyResources(this.comboRX2DisplayMode, "comboRX2DisplayMode");
            this.comboRX2DisplayMode.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.comboRX2DisplayMode.Items.AddRange(new object[] {
            resources.GetString("comboRX2DisplayMode.Items"),
            resources.GetString("comboRX2DisplayMode.Items1"),
            resources.GetString("comboRX2DisplayMode.Items2"),
            resources.GetString("comboRX2DisplayMode.Items3")});
            this.comboRX2DisplayMode.Name = "comboRX2DisplayMode";
            this.toolTip1.SetToolTip(this.comboRX2DisplayMode, resources.GetString("comboRX2DisplayMode.ToolTip"));
            this.comboRX2DisplayMode.SelectedIndexChanged += new System.EventHandler(this.comboRX2DisplayMode_SelectedIndexChanged);
            // 
            // chkRX2Mute
            // 
            resources.ApplyResources(this.chkRX2Mute, "chkRX2Mute");
            this.chkRX2Mute.Name = "chkRX2Mute";
            this.toolTip1.SetToolTip(this.chkRX2Mute, resources.GetString("chkRX2Mute.ToolTip"));
            this.chkRX2Mute.CheckedChanged += new System.EventHandler(this.chkRX2Mute_CheckedChanged);
            // 
            // chkPanSwap
            // 
            resources.ApplyResources(this.chkPanSwap, "chkPanSwap");
            this.chkPanSwap.FlatAppearance.BorderSize = 0;
            this.chkPanSwap.ForeColor = System.Drawing.Color.White;
            this.chkPanSwap.Name = "chkPanSwap";
            this.toolTip1.SetToolTip(this.chkPanSwap, resources.GetString("chkPanSwap.ToolTip"));
            this.chkPanSwap.CheckedChanged += new System.EventHandler(this.chkPanSwap_CheckedChanged);
            // 
            // chkEnableMultiRX
            // 
            resources.ApplyResources(this.chkEnableMultiRX, "chkEnableMultiRX");
            this.chkEnableMultiRX.FlatAppearance.BorderSize = 0;
            this.chkEnableMultiRX.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkEnableMultiRX.Name = "chkEnableMultiRX";
            this.toolTip1.SetToolTip(this.chkEnableMultiRX, resources.GetString("chkEnableMultiRX.ToolTip"));
            this.chkEnableMultiRX.CheckedChanged += new System.EventHandler(this.chEnableMultiRX_CheckedChanged);
            this.chkEnableMultiRX.MouseDown += new System.Windows.Forms.MouseEventHandler(this.chkEnableMultiRX_MouseDown);
            // 
            // chkSR
            // 
            resources.ApplyResources(this.chkSR, "chkSR");
            this.chkSR.BackColor = System.Drawing.Color.Yellow;
            this.chkSR.Checked = true;
            this.chkSR.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSR.FlatAppearance.BorderSize = 0;
            this.chkSR.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkSR.Name = "chkSR";
            this.toolTip1.SetToolTip(this.chkSR, resources.GetString("chkSR.ToolTip"));
            this.chkSR.UseVisualStyleBackColor = false;
            this.chkSR.CheckedChanged += new System.EventHandler(this.chkSR_CheckedChanged);
            // 
            // chkNR
            // 
            resources.ApplyResources(this.chkNR, "chkNR");
            this.chkNR.FlatAppearance.BorderSize = 0;
            this.chkNR.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkNR.Name = "chkNR";
            this.toolTip1.SetToolTip(this.chkNR, resources.GetString("chkNR.ToolTip"));
            this.chkNR.CheckedChanged += new System.EventHandler(this.chkNR_CheckedChanged);
            this.chkNR.MouseDown += new System.Windows.Forms.MouseEventHandler(this.chkNR_MouseDown);
            // 
            // chkDSPNB2
            // 
            resources.ApplyResources(this.chkDSPNB2, "chkDSPNB2");
            this.chkDSPNB2.FlatAppearance.BorderSize = 0;
            this.chkDSPNB2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkDSPNB2.Name = "chkDSPNB2";
            this.toolTip1.SetToolTip(this.chkDSPNB2, resources.GetString("chkDSPNB2.ToolTip"));
            this.chkDSPNB2.CheckedChanged += new System.EventHandler(this.chkDSPNB2_CheckedChanged);
            this.chkDSPNB2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.chkDSPNB2_MouseDown);
            // 
            // chkBIN
            // 
            resources.ApplyResources(this.chkBIN, "chkBIN");
            this.chkBIN.FlatAppearance.BorderSize = 0;
            this.chkBIN.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkBIN.Name = "chkBIN";
            this.toolTip1.SetToolTip(this.chkBIN, resources.GetString("chkBIN.ToolTip"));
            this.chkBIN.CheckedChanged += new System.EventHandler(this.chkBIN_CheckedChanged);
            // 
            // chkNB
            // 
            resources.ApplyResources(this.chkNB, "chkNB");
            this.chkNB.FlatAppearance.BorderSize = 0;
            this.chkNB.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkNB.Name = "chkNB";
            this.toolTip1.SetToolTip(this.chkNB, resources.GetString("chkNB.ToolTip"));
            this.chkNB.CheckedChanged += new System.EventHandler(this.chkNB_CheckedChanged);
            this.chkNB.MouseDown += new System.Windows.Forms.MouseEventHandler(this.chkNB_MouseDown);
            // 
            // chkANF
            // 
            resources.ApplyResources(this.chkANF, "chkANF");
            this.chkANF.FlatAppearance.BorderSize = 0;
            this.chkANF.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkANF.Name = "chkANF";
            this.toolTip1.SetToolTip(this.chkANF, resources.GetString("chkANF.ToolTip"));
            this.chkANF.CheckedChanged += new System.EventHandler(this.chkANF_CheckedChanged);
            this.chkANF.MouseDown += new System.Windows.Forms.MouseEventHandler(this.chkANF_MouseDown);
            // 
            // chkRX1Preamp
            // 
            resources.ApplyResources(this.chkRX1Preamp, "chkRX1Preamp");
            this.chkRX1Preamp.FlatAppearance.BorderSize = 0;
            this.chkRX1Preamp.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkRX1Preamp.Name = "chkRX1Preamp";
            this.toolTip1.SetToolTip(this.chkRX1Preamp, resources.GetString("chkRX1Preamp.ToolTip"));
            this.chkRX1Preamp.CheckedChanged += new System.EventHandler(this.chkRX1Preamp_CheckedChanged);
            // 
            // comboAGC
            // 
            this.comboAGC.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.comboAGC.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboAGC.DropDownWidth = 48;
            resources.ApplyResources(this.comboAGC, "comboAGC");
            this.comboAGC.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.comboAGC.Name = "comboAGC";
            this.toolTip1.SetToolTip(this.comboAGC, resources.GetString("comboAGC.ToolTip"));
            this.comboAGC.SelectedIndexChanged += new System.EventHandler(this.comboAGC_SelectedIndexChanged);
            // 
            // lblAGC
            // 
            this.lblAGC.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.lblAGC, "lblAGC");
            this.lblAGC.Name = "lblAGC";
            this.toolTip1.SetToolTip(this.lblAGC, resources.GetString("lblAGC.ToolTip"));
            // 
            // comboPreamp
            // 
            this.comboPreamp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.comboPreamp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboPreamp.DropDownWidth = 48;
            resources.ApplyResources(this.comboPreamp, "comboPreamp");
            this.comboPreamp.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.comboPreamp.Items.AddRange(new object[] {
            resources.GetString("comboPreamp.Items"),
            resources.GetString("comboPreamp.Items1"),
            resources.GetString("comboPreamp.Items2"),
            resources.GetString("comboPreamp.Items3")});
            this.comboPreamp.Name = "comboPreamp";
            this.toolTip1.SetToolTip(this.comboPreamp, resources.GetString("comboPreamp.ToolTip"));
            this.comboPreamp.SelectedIndexChanged += new System.EventHandler(this.comboPreamp_SelectedIndexChanged);
            // 
            // lblRF
            // 
            this.lblRF.ForeColor = System.Drawing.Color.Yellow;
            resources.ApplyResources(this.lblRF, "lblRF");
            this.lblRF.Name = "lblRF";
            this.toolTip1.SetToolTip(this.lblRF, resources.GetString("lblRF.ToolTip"));
            this.lblRF.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblRF_MouseDown);
            // 
            // comboDigTXProfile
            // 
            this.comboDigTXProfile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.comboDigTXProfile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboDigTXProfile.DropDownWidth = 96;
            resources.ApplyResources(this.comboDigTXProfile, "comboDigTXProfile");
            this.comboDigTXProfile.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.comboDigTXProfile.Name = "comboDigTXProfile";
            this.toolTip1.SetToolTip(this.comboDigTXProfile, resources.GetString("comboDigTXProfile.ToolTip"));
            this.comboDigTXProfile.SelectedIndexChanged += new System.EventHandler(this.comboDigTXProfile_SelectedIndexChanged);
            this.comboDigTXProfile.MouseDown += new System.Windows.Forms.MouseEventHandler(this.comboDigTXProfile_MouseDown);
            // 
            // chkVACStereo
            // 
            this.chkVACStereo.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.chkVACStereo, "chkVACStereo");
            this.chkVACStereo.Name = "chkVACStereo";
            this.toolTip1.SetToolTip(this.chkVACStereo, resources.GetString("chkVACStereo.ToolTip"));
            this.chkVACStereo.CheckedChanged += new System.EventHandler(this.chkVACStereo_CheckedChanged);
            // 
            // comboVACSampleRate
            // 
            this.comboVACSampleRate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.comboVACSampleRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboVACSampleRate.DropDownWidth = 64;
            resources.ApplyResources(this.comboVACSampleRate, "comboVACSampleRate");
            this.comboVACSampleRate.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.comboVACSampleRate.Items.AddRange(new object[] {
            resources.GetString("comboVACSampleRate.Items"),
            resources.GetString("comboVACSampleRate.Items1"),
            resources.GetString("comboVACSampleRate.Items2"),
            resources.GetString("comboVACSampleRate.Items3"),
            resources.GetString("comboVACSampleRate.Items4"),
            resources.GetString("comboVACSampleRate.Items5"),
            resources.GetString("comboVACSampleRate.Items6"),
            resources.GetString("comboVACSampleRate.Items7"),
            resources.GetString("comboVACSampleRate.Items8")});
            this.comboVACSampleRate.Name = "comboVACSampleRate";
            this.toolTip1.SetToolTip(this.comboVACSampleRate, resources.GetString("comboVACSampleRate.ToolTip"));
            this.comboVACSampleRate.SelectedIndexChanged += new System.EventHandler(this.comboVACSampleRate_SelectedIndexChanged);
            this.comboVACSampleRate.MouseDown += new System.Windows.Forms.MouseEventHandler(this.comboVACSampleRate_MouseDown);
            // 
            // btnDisplayPanCenter
            // 
            this.btnDisplayPanCenter.FlatAppearance.BorderSize = 0;
            resources.ApplyResources(this.btnDisplayPanCenter, "btnDisplayPanCenter");
            this.btnDisplayPanCenter.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnDisplayPanCenter.Name = "btnDisplayPanCenter";
            this.btnDisplayPanCenter.Tag = "";
            this.toolTip1.SetToolTip(this.btnDisplayPanCenter, resources.GetString("btnDisplayPanCenter.ToolTip"));
            this.btnDisplayPanCenter.UseVisualStyleBackColor = false;
            this.btnDisplayPanCenter.Click += new System.EventHandler(this.btnDisplayPanCenter_Click);
            this.btnDisplayPanCenter.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnDisplayPanCenter_MouseDown);
            // 
            // udFilterHigh
            // 
            this.udFilterHigh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            resources.ApplyResources(this.udFilterHigh, "udFilterHigh");
            this.udFilterHigh.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.udFilterHigh.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.udFilterHigh.Maximum = new decimal(new int[] {
            52000,
            0,
            0,
            0});
            this.udFilterHigh.Minimum = new decimal(new int[] {
            52000,
            0,
            0,
            -2147483648});
            this.udFilterHigh.Name = "udFilterHigh";
            this.toolTip1.SetToolTip(this.udFilterHigh, resources.GetString("udFilterHigh.ToolTip"));
            this.udFilterHigh.Value = new decimal(new int[] {
            6000,
            0,
            0,
            0});
            this.udFilterHigh.ValueChanged += new System.EventHandler(this.udFilterHigh_ValueChanged);
            this.udFilterHigh.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Console_KeyPress);
            this.udFilterHigh.LostFocus += new System.EventHandler(this.udFilterHigh_LostFocus);
            this.udFilterHigh.MouseDown += new System.Windows.Forms.MouseEventHandler(this.udFilterHigh_MouseDown);
            // 
            // udFilterLow
            // 
            this.udFilterLow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            resources.ApplyResources(this.udFilterLow, "udFilterLow");
            this.udFilterLow.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.udFilterLow.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.udFilterLow.Maximum = new decimal(new int[] {
            52000,
            0,
            0,
            0});
            this.udFilterLow.Minimum = new decimal(new int[] {
            52000,
            0,
            0,
            -2147483648});
            this.udFilterLow.Name = "udFilterLow";
            this.toolTip1.SetToolTip(this.udFilterLow, resources.GetString("udFilterLow.ToolTip"));
            this.udFilterLow.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.udFilterLow.ValueChanged += new System.EventHandler(this.udFilterLow_ValueChanged);
            this.udFilterLow.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Console_KeyPress);
            this.udFilterLow.LostFocus += new System.EventHandler(this.udFilterLow_LostFocus);
            this.udFilterLow.MouseDown += new System.Windows.Forms.MouseEventHandler(this.udFilterLow_MouseDown);
            // 
            // btnFilterShiftReset
            // 
            this.btnFilterShiftReset.FlatAppearance.BorderSize = 0;
            resources.ApplyResources(this.btnFilterShiftReset, "btnFilterShiftReset");
            this.btnFilterShiftReset.Name = "btnFilterShiftReset";
            this.btnFilterShiftReset.Tag = "Reset Filter Shift";
            this.toolTip1.SetToolTip(this.btnFilterShiftReset, resources.GetString("btnFilterShiftReset.ToolTip"));
            this.btnFilterShiftReset.Click += new System.EventHandler(this.btnFilterShiftReset_Click);
            // 
            // radModeAM
            // 
            resources.ApplyResources(this.radModeAM, "radModeAM");
            this.radModeAM.FlatAppearance.BorderSize = 0;
            this.radModeAM.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radModeAM.Name = "radModeAM";
            this.toolTip1.SetToolTip(this.radModeAM, resources.GetString("radModeAM.ToolTip"));
            this.radModeAM.CheckedChanged += new System.EventHandler(this.radModeAM_CheckedChanged);
            this.radModeAM.MouseUp += new System.Windows.Forms.MouseEventHandler(this.radModeAM_MouseUp);
            // 
            // radModeLSB
            // 
            resources.ApplyResources(this.radModeLSB, "radModeLSB");
            this.radModeLSB.FlatAppearance.BorderSize = 0;
            this.radModeLSB.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radModeLSB.Name = "radModeLSB";
            this.toolTip1.SetToolTip(this.radModeLSB, resources.GetString("radModeLSB.ToolTip"));
            this.radModeLSB.CheckedChanged += new System.EventHandler(this.radModeLSB_CheckedChanged);
            // 
            // radModeSAM
            // 
            resources.ApplyResources(this.radModeSAM, "radModeSAM");
            this.radModeSAM.FlatAppearance.BorderSize = 0;
            this.radModeSAM.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radModeSAM.Name = "radModeSAM";
            this.toolTip1.SetToolTip(this.radModeSAM, resources.GetString("radModeSAM.ToolTip"));
            this.radModeSAM.CheckedChanged += new System.EventHandler(this.radModeSAM_CheckedChanged);
            this.radModeSAM.MouseUp += new System.Windows.Forms.MouseEventHandler(this.radModeSAM_MouseUp);
            // 
            // radModeCWL
            // 
            resources.ApplyResources(this.radModeCWL, "radModeCWL");
            this.radModeCWL.FlatAppearance.BorderSize = 0;
            this.radModeCWL.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radModeCWL.Name = "radModeCWL";
            this.toolTip1.SetToolTip(this.radModeCWL, resources.GetString("radModeCWL.ToolTip"));
            this.radModeCWL.CheckedChanged += new System.EventHandler(this.radModeCWL_CheckedChanged);
            this.radModeCWL.MouseDown += new System.Windows.Forms.MouseEventHandler(this.radModeCWL_MouseDown);
            // 
            // radModeDSB
            // 
            resources.ApplyResources(this.radModeDSB, "radModeDSB");
            this.radModeDSB.FlatAppearance.BorderSize = 0;
            this.radModeDSB.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radModeDSB.Name = "radModeDSB";
            this.toolTip1.SetToolTip(this.radModeDSB, resources.GetString("radModeDSB.ToolTip"));
            this.radModeDSB.CheckedChanged += new System.EventHandler(this.radModeDSB_CheckedChanged);
            // 
            // radModeUSB
            // 
            resources.ApplyResources(this.radModeUSB, "radModeUSB");
            this.radModeUSB.BackColor = System.Drawing.SystemColors.Control;
            this.radModeUSB.FlatAppearance.BorderSize = 0;
            this.radModeUSB.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radModeUSB.Name = "radModeUSB";
            this.toolTip1.SetToolTip(this.radModeUSB, resources.GetString("radModeUSB.ToolTip"));
            this.radModeUSB.UseVisualStyleBackColor = false;
            this.radModeUSB.CheckedChanged += new System.EventHandler(this.radModeUSB_CheckedChanged);
            // 
            // radModeCWU
            // 
            resources.ApplyResources(this.radModeCWU, "radModeCWU");
            this.radModeCWU.FlatAppearance.BorderSize = 0;
            this.radModeCWU.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radModeCWU.Name = "radModeCWU";
            this.toolTip1.SetToolTip(this.radModeCWU, resources.GetString("radModeCWU.ToolTip"));
            this.radModeCWU.CheckedChanged += new System.EventHandler(this.radModeCWU_CheckedChanged);
            this.radModeCWU.MouseDown += new System.Windows.Forms.MouseEventHandler(this.radModeCWU_MouseDown);
            // 
            // radModeFMN
            // 
            resources.ApplyResources(this.radModeFMN, "radModeFMN");
            this.radModeFMN.FlatAppearance.BorderSize = 0;
            this.radModeFMN.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radModeFMN.Name = "radModeFMN";
            this.toolTip1.SetToolTip(this.radModeFMN, resources.GetString("radModeFMN.ToolTip"));
            this.radModeFMN.CheckedChanged += new System.EventHandler(this.radModeFMN_CheckedChanged);
            this.radModeFMN.MouseDown += new System.Windows.Forms.MouseEventHandler(this.radModeFMN_MouseDown);
            this.radModeFMN.MouseUp += new System.Windows.Forms.MouseEventHandler(this.radModeFMN_MouseUp);
            // 
            // radModeDIGU
            // 
            resources.ApplyResources(this.radModeDIGU, "radModeDIGU");
            this.radModeDIGU.FlatAppearance.BorderSize = 0;
            this.radModeDIGU.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radModeDIGU.Name = "radModeDIGU";
            this.toolTip1.SetToolTip(this.radModeDIGU, resources.GetString("radModeDIGU.ToolTip"));
            this.radModeDIGU.CheckedChanged += new System.EventHandler(this.radModeDIGU_CheckedChanged);
            // 
            // radModeDRM
            // 
            resources.ApplyResources(this.radModeDRM, "radModeDRM");
            this.radModeDRM.FlatAppearance.BorderSize = 0;
            this.radModeDRM.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radModeDRM.Name = "radModeDRM";
            this.toolTip1.SetToolTip(this.radModeDRM, resources.GetString("radModeDRM.ToolTip"));
            this.radModeDRM.CheckedChanged += new System.EventHandler(this.radModeDRM_CheckedChanged);
            // 
            // radModeDIGL
            // 
            resources.ApplyResources(this.radModeDIGL, "radModeDIGL");
            this.radModeDIGL.FlatAppearance.BorderSize = 0;
            this.radModeDIGL.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radModeDIGL.Name = "radModeDIGL";
            this.toolTip1.SetToolTip(this.radModeDIGL, resources.GetString("radModeDIGL.ToolTip"));
            this.radModeDIGL.CheckedChanged += new System.EventHandler(this.radModeDIGL_CheckedChanged);
            // 
            // radModeSPEC
            // 
            resources.ApplyResources(this.radModeSPEC, "radModeSPEC");
            this.radModeSPEC.FlatAppearance.BorderSize = 0;
            this.radModeSPEC.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radModeSPEC.Name = "radModeSPEC";
            this.toolTip1.SetToolTip(this.radModeSPEC, resources.GetString("radModeSPEC.ToolTip"));
            this.radModeSPEC.CheckedChanged += new System.EventHandler(this.radModeSPEC_CheckedChanged);
            // 
            // comboRX2Band
            // 
            this.comboRX2Band.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.comboRX2Band.DisplayMember = "0";
            this.comboRX2Band.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboRX2Band.DropDownWidth = 56;
            resources.ApplyResources(this.comboRX2Band, "comboRX2Band");
            this.comboRX2Band.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.comboRX2Band.Items.AddRange(new object[] {
            resources.GetString("comboRX2Band.Items"),
            resources.GetString("comboRX2Band.Items1"),
            resources.GetString("comboRX2Band.Items2"),
            resources.GetString("comboRX2Band.Items3"),
            resources.GetString("comboRX2Band.Items4"),
            resources.GetString("comboRX2Band.Items5"),
            resources.GetString("comboRX2Band.Items6"),
            resources.GetString("comboRX2Band.Items7"),
            resources.GetString("comboRX2Band.Items8"),
            resources.GetString("comboRX2Band.Items9"),
            resources.GetString("comboRX2Band.Items10"),
            resources.GetString("comboRX2Band.Items11"),
            resources.GetString("comboRX2Band.Items12"),
            resources.GetString("comboRX2Band.Items13"),
            resources.GetString("comboRX2Band.Items14"),
            resources.GetString("comboRX2Band.Items15"),
            resources.GetString("comboRX2Band.Items16"),
            resources.GetString("comboRX2Band.Items17"),
            resources.GetString("comboRX2Band.Items18"),
            resources.GetString("comboRX2Band.Items19"),
            resources.GetString("comboRX2Band.Items20"),
            resources.GetString("comboRX2Band.Items21"),
            resources.GetString("comboRX2Band.Items22"),
            resources.GetString("comboRX2Band.Items23"),
            resources.GetString("comboRX2Band.Items24"),
            resources.GetString("comboRX2Band.Items25"),
            resources.GetString("comboRX2Band.Items26"),
            resources.GetString("comboRX2Band.Items27"),
            resources.GetString("comboRX2Band.Items28"),
            resources.GetString("comboRX2Band.Items29"),
            resources.GetString("comboRX2Band.Items30"),
            resources.GetString("comboRX2Band.Items31"),
            resources.GetString("comboRX2Band.Items32"),
            resources.GetString("comboRX2Band.Items33"),
            resources.GetString("comboRX2Band.Items34"),
            resources.GetString("comboRX2Band.Items35"),
            resources.GetString("comboRX2Band.Items36"),
            resources.GetString("comboRX2Band.Items37"),
            resources.GetString("comboRX2Band.Items38"),
            resources.GetString("comboRX2Band.Items39"),
            resources.GetString("comboRX2Band.Items40")});
            this.comboRX2Band.Name = "comboRX2Band";
            this.toolTip1.SetToolTip(this.comboRX2Band, resources.GetString("comboRX2Band.ToolTip"));
            this.comboRX2Band.SelectedIndexChanged += new System.EventHandler(this.comboRX2Band_SelectedIndexChanged);
            // 
            // chkPower
            // 
            resources.ApplyResources(this.chkPower, "chkPower");
            this.chkPower.BackColor = System.Drawing.SystemColors.Control;
            this.chkPower.FlatAppearance.BorderSize = 0;
            this.chkPower.Name = "chkPower";
            this.toolTip1.SetToolTip(this.chkPower, resources.GetString("chkPower.ToolTip"));
            this.chkPower.UseVisualStyleBackColor = false;
            this.chkPower.CheckedChanged += new System.EventHandler(this.chkPower_CheckedChanged);
            this.chkPower.MouseDown += new System.Windows.Forms.MouseEventHandler(this.chkPower_MouseDown);
            // 
            // chkSquelch
            // 
            resources.ApplyResources(this.chkSquelch, "chkSquelch");
            this.chkSquelch.FlatAppearance.BorderSize = 0;
            this.chkSquelch.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkSquelch.Name = "chkSquelch";
            this.toolTip1.SetToolTip(this.chkSquelch, resources.GetString("chkSquelch.ToolTip"));
            this.chkSquelch.UseVisualStyleBackColor = false;
            this.chkSquelch.CheckedChanged += new System.EventHandler(this.chkSquelch_CheckedChanged);
            // 
            // chkBCI
            // 
            resources.ApplyResources(this.chkBCI, "chkBCI");
            this.chkBCI.FlatAppearance.BorderSize = 0;
            this.chkBCI.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkBCI.Name = "chkBCI";
            this.toolTip1.SetToolTip(this.chkBCI, resources.GetString("chkBCI.ToolTip"));
            this.chkBCI.CheckedChanged += new System.EventHandler(this.chkBCI_CheckedChanged);
            // 
            // chkSplitDisplay
            // 
            resources.ApplyResources(this.chkSplitDisplay, "chkSplitDisplay");
            this.chkSplitDisplay.Name = "chkSplitDisplay";
            this.toolTip1.SetToolTip(this.chkSplitDisplay, resources.GetString("chkSplitDisplay.ToolTip"));
            this.chkSplitDisplay.CheckedChanged += new System.EventHandler(this.chkSplitDisplay_CheckedChanged);
            // 
            // comboDisplayModeTop
            // 
            this.comboDisplayModeTop.BackColor = System.Drawing.SystemColors.Window;
            this.comboDisplayModeTop.DisplayMember = "0";
            this.comboDisplayModeTop.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboDisplayModeTop.DropDownWidth = 88;
            this.comboDisplayModeTop.ForeColor = System.Drawing.SystemColors.WindowText;
            resources.ApplyResources(this.comboDisplayModeTop, "comboDisplayModeTop");
            this.comboDisplayModeTop.Items.AddRange(new object[] {
            resources.GetString("comboDisplayModeTop.Items"),
            resources.GetString("comboDisplayModeTop.Items1"),
            resources.GetString("comboDisplayModeTop.Items2")});
            this.comboDisplayModeTop.Name = "comboDisplayModeTop";
            this.toolTip1.SetToolTip(this.comboDisplayModeTop, resources.GetString("comboDisplayModeTop.ToolTip"));
            this.comboDisplayModeTop.SelectedIndexChanged += new System.EventHandler(this.comboDisplayModeTop_SelectedIndexChanged);
            // 
            // comboDisplayModeBottom
            // 
            this.comboDisplayModeBottom.BackColor = System.Drawing.SystemColors.Window;
            this.comboDisplayModeBottom.DisplayMember = "0";
            this.comboDisplayModeBottom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboDisplayModeBottom.DropDownWidth = 88;
            this.comboDisplayModeBottom.ForeColor = System.Drawing.SystemColors.WindowText;
            resources.ApplyResources(this.comboDisplayModeBottom, "comboDisplayModeBottom");
            this.comboDisplayModeBottom.Items.AddRange(new object[] {
            resources.GetString("comboDisplayModeBottom.Items"),
            resources.GetString("comboDisplayModeBottom.Items1"),
            resources.GetString("comboDisplayModeBottom.Items2")});
            this.comboDisplayModeBottom.Name = "comboDisplayModeBottom";
            this.toolTip1.SetToolTip(this.comboDisplayModeBottom, resources.GetString("comboDisplayModeBottom.ToolTip"));
            this.comboDisplayModeBottom.SelectedIndexChanged += new System.EventHandler(this.comboDisplayModeBottom_SelectedIndexChanged);
            // 
            // chkRX2SR
            // 
            resources.ApplyResources(this.chkRX2SR, "chkRX2SR");
            this.chkRX2SR.BackColor = System.Drawing.Color.Yellow;
            this.chkRX2SR.Checked = true;
            this.chkRX2SR.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRX2SR.FlatAppearance.BorderSize = 0;
            this.chkRX2SR.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkRX2SR.Name = "chkRX2SR";
            this.toolTip1.SetToolTip(this.chkRX2SR, resources.GetString("chkRX2SR.ToolTip"));
            this.chkRX2SR.UseVisualStyleBackColor = false;
            this.chkRX2SR.CheckedChanged += new System.EventHandler(this.chkRX2SR_CheckedChanged);
            // 
            // chkRX2NB2
            // 
            resources.ApplyResources(this.chkRX2NB2, "chkRX2NB2");
            this.chkRX2NB2.FlatAppearance.BorderSize = 0;
            this.chkRX2NB2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkRX2NB2.Name = "chkRX2NB2";
            this.toolTip1.SetToolTip(this.chkRX2NB2, resources.GetString("chkRX2NB2.ToolTip"));
            this.chkRX2NB2.CheckedChanged += new System.EventHandler(this.chkRX2NB2_CheckedChanged);
            this.chkRX2NB2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.chkRX2NB2_MouseDown);
            // 
            // chkRX2NB
            // 
            resources.ApplyResources(this.chkRX2NB, "chkRX2NB");
            this.chkRX2NB.FlatAppearance.BorderSize = 0;
            this.chkRX2NB.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkRX2NB.Name = "chkRX2NB";
            this.toolTip1.SetToolTip(this.chkRX2NB, resources.GetString("chkRX2NB.ToolTip"));
            this.chkRX2NB.CheckedChanged += new System.EventHandler(this.chkRX2NB_CheckedChanged);
            this.chkRX2NB.MouseDown += new System.Windows.Forms.MouseEventHandler(this.chkRX2NB_MouseDown);
            // 
            // chkRX2ANF
            // 
            resources.ApplyResources(this.chkRX2ANF, "chkRX2ANF");
            this.chkRX2ANF.FlatAppearance.BorderSize = 0;
            this.chkRX2ANF.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkRX2ANF.Name = "chkRX2ANF";
            this.toolTip1.SetToolTip(this.chkRX2ANF, resources.GetString("chkRX2ANF.ToolTip"));
            this.chkRX2ANF.CheckedChanged += new System.EventHandler(this.chkRX2ANF_CheckedChanged);
            this.chkRX2ANF.MouseDown += new System.Windows.Forms.MouseEventHandler(this.chkRX2ANF_MouseDown);
            // 
            // chkRX2NR
            // 
            resources.ApplyResources(this.chkRX2NR, "chkRX2NR");
            this.chkRX2NR.FlatAppearance.BorderSize = 0;
            this.chkRX2NR.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkRX2NR.Name = "chkRX2NR";
            this.toolTip1.SetToolTip(this.chkRX2NR, resources.GetString("chkRX2NR.ToolTip"));
            this.chkRX2NR.CheckedChanged += new System.EventHandler(this.chkRX2NR_CheckedChanged);
            this.chkRX2NR.MouseDown += new System.Windows.Forms.MouseEventHandler(this.chkRX2NR_MouseDown);
            // 
            // chkRX2BIN
            // 
            resources.ApplyResources(this.chkRX2BIN, "chkRX2BIN");
            this.chkRX2BIN.FlatAppearance.BorderSize = 0;
            this.chkRX2BIN.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkRX2BIN.Name = "chkRX2BIN";
            this.toolTip1.SetToolTip(this.chkRX2BIN, resources.GetString("chkRX2BIN.ToolTip"));
            this.chkRX2BIN.CheckedChanged += new System.EventHandler(this.chkRX2BIN_CheckedChanged);
            // 
            // comboRX2AGC
            // 
            this.comboRX2AGC.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.comboRX2AGC.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboRX2AGC.DropDownWidth = 48;
            this.comboRX2AGC.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            resources.ApplyResources(this.comboRX2AGC, "comboRX2AGC");
            this.comboRX2AGC.Name = "comboRX2AGC";
            this.toolTip1.SetToolTip(this.comboRX2AGC, resources.GetString("comboRX2AGC.ToolTip"));
            this.comboRX2AGC.SelectedIndexChanged += new System.EventHandler(this.comboRX2AGC_SelectedIndexChanged);
            // 
            // lblRX2AGC
            // 
            this.lblRX2AGC.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            resources.ApplyResources(this.lblRX2AGC, "lblRX2AGC");
            this.lblRX2AGC.Name = "lblRX2AGC";
            this.toolTip1.SetToolTip(this.lblRX2AGC, resources.GetString("lblRX2AGC.ToolTip"));
            // 
            // lblRX2RF
            // 
            this.lblRX2RF.BackColor = System.Drawing.Color.Transparent;
            this.lblRX2RF.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            resources.ApplyResources(this.lblRX2RF, "lblRX2RF");
            this.lblRX2RF.Name = "lblRX2RF";
            this.toolTip1.SetToolTip(this.lblRX2RF, resources.GetString("lblRX2RF.ToolTip"));
            // 
            // chkRX2Squelch
            // 
            resources.ApplyResources(this.chkRX2Squelch, "chkRX2Squelch");
            this.chkRX2Squelch.FlatAppearance.BorderSize = 0;
            this.chkRX2Squelch.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkRX2Squelch.Name = "chkRX2Squelch";
            this.toolTip1.SetToolTip(this.chkRX2Squelch, resources.GetString("chkRX2Squelch.ToolTip"));
            this.chkRX2Squelch.CheckedChanged += new System.EventHandler(this.chkRX2Squelch_CheckedChanged);
            // 
            // chkRX2Preamp
            // 
            resources.ApplyResources(this.chkRX2Preamp, "chkRX2Preamp");
            this.chkRX2Preamp.FlatAppearance.BorderSize = 0;
            this.chkRX2Preamp.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkRX2Preamp.Name = "chkRX2Preamp";
            this.toolTip1.SetToolTip(this.chkRX2Preamp, resources.GetString("chkRX2Preamp.ToolTip"));
            this.chkRX2Preamp.CheckedChanged += new System.EventHandler(this.chkRX2Preamp_CheckedChanged);
            // 
            // chkRX2DisplayAVG
            // 
            resources.ApplyResources(this.chkRX2DisplayAVG, "chkRX2DisplayAVG");
            this.chkRX2DisplayAVG.FlatAppearance.BorderSize = 0;
            this.chkRX2DisplayAVG.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkRX2DisplayAVG.Name = "chkRX2DisplayAVG";
            this.toolTip1.SetToolTip(this.chkRX2DisplayAVG, resources.GetString("chkRX2DisplayAVG.ToolTip"));
            this.chkRX2DisplayAVG.CheckedChanged += new System.EventHandler(this.chkRX2DisplayAVG_CheckedChanged);
            // 
            // radDisplayZoom05
            // 
            resources.ApplyResources(this.radDisplayZoom05, "radDisplayZoom05");
            this.radDisplayZoom05.FlatAppearance.BorderSize = 0;
            this.radDisplayZoom05.ForeColor = System.Drawing.Color.White;
            this.radDisplayZoom05.Name = "radDisplayZoom05";
            this.radDisplayZoom05.TabStop = true;
            this.toolTip1.SetToolTip(this.radDisplayZoom05, resources.GetString("radDisplayZoom05.ToolTip"));
            this.radDisplayZoom05.UseVisualStyleBackColor = true;
            this.radDisplayZoom05.CheckedChanged += new System.EventHandler(this.radDisplayZoom05_CheckedChanged);
            this.radDisplayZoom05.MouseDown += new System.Windows.Forms.MouseEventHandler(this.radDisplayZoom05_MouseDown);
            // 
            // radDisplayZoom4x
            // 
            resources.ApplyResources(this.radDisplayZoom4x, "radDisplayZoom4x");
            this.radDisplayZoom4x.FlatAppearance.BorderSize = 0;
            this.radDisplayZoom4x.ForeColor = System.Drawing.Color.White;
            this.radDisplayZoom4x.Name = "radDisplayZoom4x";
            this.radDisplayZoom4x.TabStop = true;
            this.toolTip1.SetToolTip(this.radDisplayZoom4x, resources.GetString("radDisplayZoom4x.ToolTip"));
            this.radDisplayZoom4x.UseVisualStyleBackColor = true;
            this.radDisplayZoom4x.CheckedChanged += new System.EventHandler(this.radDisplayZoom4x_CheckedChanged);
            this.radDisplayZoom4x.MouseDown += new System.Windows.Forms.MouseEventHandler(this.radDisplayZoom4x_MouseDown);
            // 
            // radDisplayZoom2x
            // 
            resources.ApplyResources(this.radDisplayZoom2x, "radDisplayZoom2x");
            this.radDisplayZoom2x.FlatAppearance.BorderSize = 0;
            this.radDisplayZoom2x.ForeColor = System.Drawing.Color.White;
            this.radDisplayZoom2x.Name = "radDisplayZoom2x";
            this.radDisplayZoom2x.TabStop = true;
            this.toolTip1.SetToolTip(this.radDisplayZoom2x, resources.GetString("radDisplayZoom2x.ToolTip"));
            this.radDisplayZoom2x.UseVisualStyleBackColor = true;
            this.radDisplayZoom2x.CheckedChanged += new System.EventHandler(this.radDisplayZoom2x_CheckedChanged);
            this.radDisplayZoom2x.MouseDown += new System.Windows.Forms.MouseEventHandler(this.radDisplayZoom2x_MouseDown);
            // 
            // radDisplayZoom1x
            // 
            resources.ApplyResources(this.radDisplayZoom1x, "radDisplayZoom1x");
            this.radDisplayZoom1x.FlatAppearance.BorderSize = 0;
            this.radDisplayZoom1x.ForeColor = System.Drawing.Color.White;
            this.radDisplayZoom1x.Name = "radDisplayZoom1x";
            this.radDisplayZoom1x.TabStop = true;
            this.toolTip1.SetToolTip(this.radDisplayZoom1x, resources.GetString("radDisplayZoom1x.ToolTip"));
            this.radDisplayZoom1x.UseVisualStyleBackColor = true;
            this.radDisplayZoom1x.CheckedChanged += new System.EventHandler(this.radDisplayZoom1x_CheckedChanged);
            this.radDisplayZoom1x.MouseDown += new System.Windows.Forms.MouseEventHandler(this.radDisplayZoom1x_MouseDown);
            // 
            // txtDisplayPeakOffset
            // 
            resources.ApplyResources(this.txtDisplayPeakOffset, "txtDisplayPeakOffset");
            this.txtDisplayPeakOffset.BackColor = System.Drawing.Color.Black;
            this.txtDisplayPeakOffset.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDisplayPeakOffset.ForeColor = System.Drawing.Color.DodgerBlue;
            this.txtDisplayPeakOffset.Name = "txtDisplayPeakOffset";
            this.txtDisplayPeakOffset.ReadOnly = true;
            this.txtDisplayPeakOffset.ShortcutsEnabled = false;
            this.toolTip1.SetToolTip(this.txtDisplayPeakOffset, resources.GetString("txtDisplayPeakOffset.ToolTip"));
            this.txtDisplayPeakOffset.Click += new System.EventHandler(this.txtDisplayPeakOffset_TextChanged);
            this.txtDisplayPeakOffset.GotFocus += new System.EventHandler(this.HideFocus);
            // 
            // txtDisplayCursorOffset
            // 
            resources.ApplyResources(this.txtDisplayCursorOffset, "txtDisplayCursorOffset");
            this.txtDisplayCursorOffset.BackColor = System.Drawing.Color.Black;
            this.txtDisplayCursorOffset.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDisplayCursorOffset.ForeColor = System.Drawing.Color.DodgerBlue;
            this.txtDisplayCursorOffset.Name = "txtDisplayCursorOffset";
            this.txtDisplayCursorOffset.ReadOnly = true;
            this.txtDisplayCursorOffset.ShortcutsEnabled = false;
            this.toolTip1.SetToolTip(this.txtDisplayCursorOffset, resources.GetString("txtDisplayCursorOffset.ToolTip"));
            this.txtDisplayCursorOffset.GotFocus += new System.EventHandler(this.HideFocus);
            // 
            // txtDisplayCursorPower
            // 
            resources.ApplyResources(this.txtDisplayCursorPower, "txtDisplayCursorPower");
            this.txtDisplayCursorPower.BackColor = System.Drawing.Color.Black;
            this.txtDisplayCursorPower.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDisplayCursorPower.ForeColor = System.Drawing.Color.DodgerBlue;
            this.txtDisplayCursorPower.Name = "txtDisplayCursorPower";
            this.txtDisplayCursorPower.ReadOnly = true;
            this.txtDisplayCursorPower.ShortcutsEnabled = false;
            this.toolTip1.SetToolTip(this.txtDisplayCursorPower, resources.GetString("txtDisplayCursorPower.ToolTip"));
            // 
            // txtDisplayCursorFreq
            // 
            resources.ApplyResources(this.txtDisplayCursorFreq, "txtDisplayCursorFreq");
            this.txtDisplayCursorFreq.BackColor = System.Drawing.Color.Black;
            this.txtDisplayCursorFreq.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDisplayCursorFreq.ForeColor = System.Drawing.Color.DodgerBlue;
            this.txtDisplayCursorFreq.Name = "txtDisplayCursorFreq";
            this.txtDisplayCursorFreq.ReadOnly = true;
            this.txtDisplayCursorFreq.ShortcutsEnabled = false;
            this.toolTip1.SetToolTip(this.txtDisplayCursorFreq, resources.GetString("txtDisplayCursorFreq.ToolTip"));
            // 
            // txtDisplayPeakPower
            // 
            resources.ApplyResources(this.txtDisplayPeakPower, "txtDisplayPeakPower");
            this.txtDisplayPeakPower.BackColor = System.Drawing.Color.Black;
            this.txtDisplayPeakPower.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDisplayPeakPower.ForeColor = System.Drawing.Color.DodgerBlue;
            this.txtDisplayPeakPower.Name = "txtDisplayPeakPower";
            this.txtDisplayPeakPower.ReadOnly = true;
            this.txtDisplayPeakPower.ShortcutsEnabled = false;
            this.toolTip1.SetToolTip(this.txtDisplayPeakPower, resources.GetString("txtDisplayPeakPower.ToolTip"));
            this.txtDisplayPeakPower.Click += new System.EventHandler(this.txtDisplayPeakPower_TextChanged);
            // 
            // txtDisplayPeakFreq
            // 
            resources.ApplyResources(this.txtDisplayPeakFreq, "txtDisplayPeakFreq");
            this.txtDisplayPeakFreq.BackColor = System.Drawing.Color.Black;
            this.txtDisplayPeakFreq.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDisplayPeakFreq.ForeColor = System.Drawing.Color.DodgerBlue;
            this.txtDisplayPeakFreq.Name = "txtDisplayPeakFreq";
            this.txtDisplayPeakFreq.ReadOnly = true;
            this.txtDisplayPeakFreq.ShortcutsEnabled = false;
            this.toolTip1.SetToolTip(this.txtDisplayPeakFreq, resources.GetString("txtDisplayPeakFreq.ToolTip"));
            this.txtDisplayPeakFreq.Click += new System.EventHandler(this.txtDisplayPeakFreq_TextChanged);
            // 
            // autoBrightBox
            // 
            resources.ApplyResources(this.autoBrightBox, "autoBrightBox");
            this.autoBrightBox.BackColor = System.Drawing.Color.Black;
            this.autoBrightBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.autoBrightBox.ForeColor = System.Drawing.SystemColors.Info;
            this.autoBrightBox.Name = "autoBrightBox";
            this.autoBrightBox.ReadOnly = true;
            this.autoBrightBox.ShortcutsEnabled = false;
            this.autoBrightBox.TabStop = false;
            this.toolTip1.SetToolTip(this.autoBrightBox, resources.GetString("autoBrightBox.ToolTip"));
            this.autoBrightBox.Click += new System.EventHandler(this.autoBrightBox_Click);
            this.autoBrightBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.autoBrightBox_MouseDown);
            this.autoBrightBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.autoBrightBox_MouseUp);
            // 
            // radBandGN13
            // 
            resources.ApplyResources(this.radBandGN13, "radBandGN13");
            this.radBandGN13.FlatAppearance.BorderSize = 0;
            this.radBandGN13.ForeColor = System.Drawing.Color.Yellow;
            this.radBandGN13.Name = "radBandGN13";
            this.radBandGN13.TabStop = true;
            this.toolTip1.SetToolTip(this.radBandGN13, resources.GetString("radBandGN13.ToolTip"));
            this.radBandGN13.UseVisualStyleBackColor = true;
            this.radBandGN13.Click += new System.EventHandler(this.radBandGEN13_Click);
            this.radBandGN13.MouseDown += new System.Windows.Forms.MouseEventHandler(this.radBandGN13_MouseDown);
            // 
            // radBandGN12
            // 
            resources.ApplyResources(this.radBandGN12, "radBandGN12");
            this.radBandGN12.FlatAppearance.BorderSize = 0;
            this.radBandGN12.ForeColor = System.Drawing.Color.Yellow;
            this.radBandGN12.Name = "radBandGN12";
            this.radBandGN12.TabStop = true;
            this.toolTip1.SetToolTip(this.radBandGN12, resources.GetString("radBandGN12.ToolTip"));
            this.radBandGN12.UseVisualStyleBackColor = false;
            this.radBandGN12.Click += new System.EventHandler(this.radBandGEN12_Click);
            this.radBandGN12.MouseDown += new System.Windows.Forms.MouseEventHandler(this.radBandGN12_MouseDown);
            // 
            // radBandGN11
            // 
            resources.ApplyResources(this.radBandGN11, "radBandGN11");
            this.radBandGN11.FlatAppearance.BorderSize = 0;
            this.radBandGN11.ForeColor = System.Drawing.Color.Yellow;
            this.radBandGN11.Name = "radBandGN11";
            this.radBandGN11.TabStop = true;
            this.toolTip1.SetToolTip(this.radBandGN11, resources.GetString("radBandGN11.ToolTip"));
            this.radBandGN11.UseVisualStyleBackColor = true;
            this.radBandGN11.Click += new System.EventHandler(this.radBandGEN11_Click);
            this.radBandGN11.MouseDown += new System.Windows.Forms.MouseEventHandler(this.radBandGN11_MouseDown);
            // 
            // radBandGN10
            // 
            resources.ApplyResources(this.radBandGN10, "radBandGN10");
            this.radBandGN10.FlatAppearance.BorderSize = 0;
            this.radBandGN10.ForeColor = System.Drawing.Color.Yellow;
            this.radBandGN10.Name = "radBandGN10";
            this.radBandGN10.TabStop = true;
            this.toolTip1.SetToolTip(this.radBandGN10, resources.GetString("radBandGN10.ToolTip"));
            this.radBandGN10.UseVisualStyleBackColor = true;
            this.radBandGN10.Click += new System.EventHandler(this.radBandGEN10_Click);
            this.radBandGN10.MouseDown += new System.Windows.Forms.MouseEventHandler(this.radBandGN10_MouseDown);
            // 
            // radBandGN9
            // 
            resources.ApplyResources(this.radBandGN9, "radBandGN9");
            this.radBandGN9.FlatAppearance.BorderSize = 0;
            this.radBandGN9.ForeColor = System.Drawing.Color.Yellow;
            this.radBandGN9.Name = "radBandGN9";
            this.radBandGN9.TabStop = true;
            this.toolTip1.SetToolTip(this.radBandGN9, resources.GetString("radBandGN9.ToolTip"));
            this.radBandGN9.UseVisualStyleBackColor = true;
            this.radBandGN9.Click += new System.EventHandler(this.radBandGEN9_Click);
            this.radBandGN9.MouseDown += new System.Windows.Forms.MouseEventHandler(this.radBandGN9_MouseDown);
            // 
            // radBandGN8
            // 
            resources.ApplyResources(this.radBandGN8, "radBandGN8");
            this.radBandGN8.FlatAppearance.BorderSize = 0;
            this.radBandGN8.ForeColor = System.Drawing.Color.Yellow;
            this.radBandGN8.Name = "radBandGN8";
            this.radBandGN8.TabStop = true;
            this.toolTip1.SetToolTip(this.radBandGN8, resources.GetString("radBandGN8.ToolTip"));
            this.radBandGN8.UseVisualStyleBackColor = true;
            this.radBandGN8.Click += new System.EventHandler(this.radBandGEN8_Click);
            this.radBandGN8.MouseDown += new System.Windows.Forms.MouseEventHandler(this.radBandGN8_MouseDown);
            // 
            // radBandGN7
            // 
            resources.ApplyResources(this.radBandGN7, "radBandGN7");
            this.radBandGN7.FlatAppearance.BorderSize = 0;
            this.radBandGN7.ForeColor = System.Drawing.Color.Yellow;
            this.radBandGN7.Name = "radBandGN7";
            this.radBandGN7.TabStop = true;
            this.toolTip1.SetToolTip(this.radBandGN7, resources.GetString("radBandGN7.ToolTip"));
            this.radBandGN7.UseVisualStyleBackColor = true;
            this.radBandGN7.Click += new System.EventHandler(this.radBandGEN7_CheckedChanged);
            this.radBandGN7.MouseDown += new System.Windows.Forms.MouseEventHandler(this.radBandGN7_MouseDown);
            // 
            // radBandGN6
            // 
            resources.ApplyResources(this.radBandGN6, "radBandGN6");
            this.radBandGN6.FlatAppearance.BorderSize = 0;
            this.radBandGN6.ForeColor = System.Drawing.Color.Yellow;
            this.radBandGN6.Name = "radBandGN6";
            this.radBandGN6.TabStop = true;
            this.toolTip1.SetToolTip(this.radBandGN6, resources.GetString("radBandGN6.ToolTip"));
            this.radBandGN6.UseVisualStyleBackColor = true;
            this.radBandGN6.Click += new System.EventHandler(this.radBandGEN6_Click);
            this.radBandGN6.MouseDown += new System.Windows.Forms.MouseEventHandler(this.radBandGN6_MouseDown);
            // 
            // radBandGN5
            // 
            resources.ApplyResources(this.radBandGN5, "radBandGN5");
            this.radBandGN5.FlatAppearance.BorderSize = 0;
            this.radBandGN5.ForeColor = System.Drawing.Color.Yellow;
            this.radBandGN5.Name = "radBandGN5";
            this.radBandGN5.TabStop = true;
            this.toolTip1.SetToolTip(this.radBandGN5, resources.GetString("radBandGN5.ToolTip"));
            this.radBandGN5.UseVisualStyleBackColor = true;
            this.radBandGN5.Click += new System.EventHandler(this.radBandGEN5_Click);
            this.radBandGN5.MouseDown += new System.Windows.Forms.MouseEventHandler(this.radBandGN5_MouseDown);
            // 
            // radBandGN4
            // 
            resources.ApplyResources(this.radBandGN4, "radBandGN4");
            this.radBandGN4.FlatAppearance.BorderSize = 0;
            this.radBandGN4.ForeColor = System.Drawing.Color.Yellow;
            this.radBandGN4.Name = "radBandGN4";
            this.radBandGN4.TabStop = true;
            this.toolTip1.SetToolTip(this.radBandGN4, resources.GetString("radBandGN4.ToolTip"));
            this.radBandGN4.UseVisualStyleBackColor = true;
            this.radBandGN4.Click += new System.EventHandler(this.radBandGEN4_Click);
            this.radBandGN4.MouseDown += new System.Windows.Forms.MouseEventHandler(this.radBandGN4_MouseDown);
            // 
            // radBandGN3
            // 
            resources.ApplyResources(this.radBandGN3, "radBandGN3");
            this.radBandGN3.FlatAppearance.BorderSize = 0;
            this.radBandGN3.ForeColor = System.Drawing.Color.Yellow;
            this.radBandGN3.Name = "radBandGN3";
            this.radBandGN3.TabStop = true;
            this.toolTip1.SetToolTip(this.radBandGN3, resources.GetString("radBandGN3.ToolTip"));
            this.radBandGN3.UseVisualStyleBackColor = true;
            this.radBandGN3.Click += new System.EventHandler(this.radBandGEN3_Click);
            this.radBandGN3.MouseDown += new System.Windows.Forms.MouseEventHandler(this.radBandGN3_MouseDown);
            // 
            // radBandGN2
            // 
            resources.ApplyResources(this.radBandGN2, "radBandGN2");
            this.radBandGN2.FlatAppearance.BorderSize = 0;
            this.radBandGN2.ForeColor = System.Drawing.Color.Yellow;
            this.radBandGN2.Name = "radBandGN2";
            this.radBandGN2.TabStop = true;
            this.toolTip1.SetToolTip(this.radBandGN2, resources.GetString("radBandGN2.ToolTip"));
            this.radBandGN2.UseVisualStyleBackColor = true;
            this.radBandGN2.Click += new System.EventHandler(this.radBandGEN2_Click);
            this.radBandGN2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.radBandGN2_MouseDown);
            // 
            // radBandGN1
            // 
            resources.ApplyResources(this.radBandGN1, "radBandGN1");
            this.radBandGN1.FlatAppearance.BorderSize = 0;
            this.radBandGN1.ForeColor = System.Drawing.Color.Yellow;
            this.radBandGN1.Name = "radBandGN1";
            this.radBandGN1.TabStop = true;
            this.toolTip1.SetToolTip(this.radBandGN1, resources.GetString("radBandGN1.ToolTip"));
            this.radBandGN1.UseVisualStyleBackColor = true;
            this.radBandGN1.Click += new System.EventHandler(this.radBandGEN1_Click);
            this.radBandGN1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.radBandGN1_MouseDown);
            // 
            // radBandGN0
            // 
            resources.ApplyResources(this.radBandGN0, "radBandGN0");
            this.radBandGN0.FlatAppearance.BorderSize = 0;
            this.radBandGN0.ForeColor = System.Drawing.Color.Yellow;
            this.radBandGN0.Name = "radBandGN0";
            this.radBandGN0.TabStop = true;
            this.toolTip1.SetToolTip(this.radBandGN0, resources.GetString("radBandGN0.ToolTip"));
            this.radBandGN0.UseVisualStyleBackColor = true;
            this.radBandGN0.Click += new System.EventHandler(this.radBandGEN0_Click);
            this.radBandGN0.MouseDown += new System.Windows.Forms.MouseEventHandler(this.radBandGN0_MouseDown);
            // 
            // lblAntRX2
            // 
            resources.ApplyResources(this.lblAntRX2, "lblAntRX2");
            this.lblAntRX2.ForeColor = System.Drawing.Color.White;
            this.lblAntRX2.Name = "lblAntRX2";
            this.toolTip1.SetToolTip(this.lblAntRX2, resources.GetString("lblAntRX2.ToolTip"));
            this.lblAntRX2.Click += new System.EventHandler(this.lblAntRX2_Click);
            // 
            // lblAntRX1
            // 
            resources.ApplyResources(this.lblAntRX1, "lblAntRX1");
            this.lblAntRX1.ForeColor = System.Drawing.Color.White;
            this.lblAntRX1.Name = "lblAntRX1";
            this.toolTip1.SetToolTip(this.lblAntRX1, resources.GetString("lblAntRX1.ToolTip"));
            this.lblAntRX1.Click += new System.EventHandler(this.lblAntRX1_Click);
            this.lblAntRX1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblAntRX1_MouseDown);
            // 
            // lblAntTX
            // 
            resources.ApplyResources(this.lblAntTX, "lblAntTX");
            this.lblAntTX.ForeColor = System.Drawing.Color.White;
            this.lblAntTX.Name = "lblAntTX";
            this.toolTip1.SetToolTip(this.lblAntTX, resources.GetString("lblAntTX.ToolTip"));
            this.lblAntTX.Click += new System.EventHandler(this.lblAntTX_Click_1);
            // 
            // labelTS4
            // 
            this.labelTS4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.labelTS4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.labelTS4, "labelTS4");
            this.labelTS4.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.labelTS4.Name = "labelTS4";
            this.toolTip1.SetToolTip(this.labelTS4, resources.GetString("labelTS4.ToolTip"));
            this.labelTS4.Click += new System.EventHandler(this.labelTS4_Click);
            this.labelTS4.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.labelTS4_MouseWheel);
            // 
            // labelTS3
            // 
            this.labelTS3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.labelTS3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.labelTS3, "labelTS3");
            this.labelTS3.ForeColor = System.Drawing.Color.White;
            this.labelTS3.Name = "labelTS3";
            this.toolTip1.SetToolTip(this.labelTS3, resources.GetString("labelTS3.ToolTip"));
            this.labelTS3.Click += new System.EventHandler(this.labelTS3_Click);
            this.labelTS3.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.labelTS4_MouseWheel);
            // 
            // chkRX1MUTE
            // 
            resources.ApplyResources(this.chkRX1MUTE, "chkRX1MUTE");
            this.chkRX1MUTE.Name = "chkRX1MUTE";
            this.toolTip1.SetToolTip(this.chkRX1MUTE, resources.GetString("chkRX1MUTE.ToolTip"));
            // 
            // ptbCWSpeed
            // 
            resources.ApplyResources(this.ptbCWSpeed, "ptbCWSpeed");
            this.ptbCWSpeed.HeadImage = null;
            this.ptbCWSpeed.LargeChange = 1;
            this.ptbCWSpeed.Maximum = 60;
            this.ptbCWSpeed.Minimum = 1;
            this.ptbCWSpeed.Name = "ptbCWSpeed";
            this.ptbCWSpeed.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.ptbCWSpeed.SmallChange = 1;
            this.ptbCWSpeed.TabStop = false;
            this.toolTip1.SetToolTip(this.ptbCWSpeed, resources.GetString("ptbCWSpeed.ToolTip"));
            this.ptbCWSpeed.Value = 25;
            this.ptbCWSpeed.Scroll += new PowerSDR.PrettyTrackBar.ScrollHandler(this.ptbCWSpeed_Scroll);
            // 
            // ptbRX2Pan
            // 
            resources.ApplyResources(this.ptbRX2Pan, "ptbRX2Pan");
            this.ptbRX2Pan.HeadImage = null;
            this.ptbRX2Pan.LargeChange = 1;
            this.ptbRX2Pan.Maximum = 100;
            this.ptbRX2Pan.Minimum = 0;
            this.ptbRX2Pan.Name = "ptbRX2Pan";
            this.ptbRX2Pan.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.ptbRX2Pan.SmallChange = 1;
            this.ptbRX2Pan.TabStop = false;
            this.toolTip1.SetToolTip(this.ptbRX2Pan, resources.GetString("ptbRX2Pan.ToolTip"));
            this.ptbRX2Pan.Value = 50;
            this.ptbRX2Pan.Scroll += new PowerSDR.PrettyTrackBar.ScrollHandler(this.ptbRX2Pan_Scroll);
            // 
            // ptbRX2Gain
            // 
            resources.ApplyResources(this.ptbRX2Gain, "ptbRX2Gain");
            this.ptbRX2Gain.HeadImage = null;
            this.ptbRX2Gain.LargeChange = 1;
            this.ptbRX2Gain.Maximum = 100;
            this.ptbRX2Gain.Minimum = 0;
            this.ptbRX2Gain.Name = "ptbRX2Gain";
            this.ptbRX2Gain.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.ptbRX2Gain.SmallChange = 1;
            this.ptbRX2Gain.TabStop = false;
            this.toolTip1.SetToolTip(this.ptbRX2Gain, resources.GetString("ptbRX2Gain.ToolTip"));
            this.ptbRX2Gain.Value = 100;
            this.ptbRX2Gain.Scroll += new PowerSDR.PrettyTrackBar.ScrollHandler(this.ptbRX2Gain_Scroll);
            // 
            // ptbRX1Gain
            // 
            resources.ApplyResources(this.ptbRX1Gain, "ptbRX1Gain");
            this.ptbRX1Gain.HeadImage = null;
            this.ptbRX1Gain.LargeChange = 1;
            this.ptbRX1Gain.Maximum = 100;
            this.ptbRX1Gain.Minimum = 0;
            this.ptbRX1Gain.Name = "ptbRX1Gain";
            this.ptbRX1Gain.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.ptbRX1Gain.SmallChange = 1;
            this.ptbRX1Gain.TabStop = false;
            this.toolTip1.SetToolTip(this.ptbRX1Gain, resources.GetString("ptbRX1Gain.ToolTip"));
            this.ptbRX1Gain.Value = 100;
            this.ptbRX1Gain.Scroll += new PowerSDR.PrettyTrackBar.ScrollHandler(this.ptbRX1Gain_Scroll);
            // 
            // ptbPanSubRX
            // 
            resources.ApplyResources(this.ptbPanSubRX, "ptbPanSubRX");
            this.ptbPanSubRX.HeadImage = null;
            this.ptbPanSubRX.LargeChange = 1;
            this.ptbPanSubRX.Maximum = 100;
            this.ptbPanSubRX.Minimum = 0;
            this.ptbPanSubRX.Name = "ptbPanSubRX";
            this.ptbPanSubRX.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.ptbPanSubRX.SmallChange = 1;
            this.ptbPanSubRX.TabStop = false;
            this.toolTip1.SetToolTip(this.ptbPanSubRX, resources.GetString("ptbPanSubRX.ToolTip"));
            this.ptbPanSubRX.Value = 50;
            this.ptbPanSubRX.Scroll += new PowerSDR.PrettyTrackBar.ScrollHandler(this.ptbPanSubRX_Scroll);
            // 
            // ptbRX0Gain
            // 
            resources.ApplyResources(this.ptbRX0Gain, "ptbRX0Gain");
            this.ptbRX0Gain.HeadImage = null;
            this.ptbRX0Gain.LargeChange = 1;
            this.ptbRX0Gain.Maximum = 100;
            this.ptbRX0Gain.Minimum = 0;
            this.ptbRX0Gain.Name = "ptbRX0Gain";
            this.ptbRX0Gain.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.ptbRX0Gain.SmallChange = 1;
            this.ptbRX0Gain.TabStop = false;
            this.toolTip1.SetToolTip(this.ptbRX0Gain, resources.GetString("ptbRX0Gain.ToolTip"));
            this.ptbRX0Gain.Value = 100;
            this.ptbRX0Gain.Scroll += new PowerSDR.PrettyTrackBar.ScrollHandler(this.ptbRX0Gain_Scroll);
            // 
            // ptbPanMainRX
            // 
            resources.ApplyResources(this.ptbPanMainRX, "ptbPanMainRX");
            this.ptbPanMainRX.HeadImage = null;
            this.ptbPanMainRX.LargeChange = 1;
            this.ptbPanMainRX.Maximum = 100;
            this.ptbPanMainRX.Minimum = 0;
            this.ptbPanMainRX.Name = "ptbPanMainRX";
            this.ptbPanMainRX.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.ptbPanMainRX.SmallChange = 1;
            this.ptbPanMainRX.TabStop = false;
            this.toolTip1.SetToolTip(this.ptbPanMainRX, resources.GetString("ptbPanMainRX.ToolTip"));
            this.ptbPanMainRX.Value = 50;
            this.ptbPanMainRX.Scroll += new PowerSDR.PrettyTrackBar.ScrollHandler(this.ptbPanMainRX_Scroll);
            // 
            // ptbPWR
            // 
            resources.ApplyResources(this.ptbPWR, "ptbPWR");
            this.ptbPWR.HeadImage = null;
            this.ptbPWR.LargeChange = 1;
            this.ptbPWR.Maximum = 100;
            this.ptbPWR.Minimum = 0;
            this.ptbPWR.Name = "ptbPWR";
            this.ptbPWR.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.ptbPWR.SmallChange = 1;
            this.ptbPWR.TabStop = false;
            this.toolTip1.SetToolTip(this.ptbPWR, resources.GetString("ptbPWR.ToolTip"));
            this.ptbPWR.Value = 50;
            this.ptbPWR.Scroll += new PowerSDR.PrettyTrackBar.ScrollHandler(this.ptbPWR_Scroll);
            // 
            // ptbRF
            // 
            resources.ApplyResources(this.ptbRF, "ptbRF");
            this.ptbRF.HeadImage = null;
            this.ptbRF.LargeChange = 1;
            this.ptbRF.Maximum = 120;
            this.ptbRF.Minimum = -20;
            this.ptbRF.Name = "ptbRF";
            this.ptbRF.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.ptbRF.SmallChange = 1;
            this.ptbRF.TabStop = false;
            this.toolTip1.SetToolTip(this.ptbRF, resources.GetString("ptbRF.ToolTip"));
            this.ptbRF.Value = 90;
            this.ptbRF.Scroll += new PowerSDR.PrettyTrackBar.ScrollHandler(this.ptbRF_Scroll);
            this.ptbRF.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ptbRF_MouseUp);
            // 
            // ptbAF
            // 
            resources.ApplyResources(this.ptbAF, "ptbAF");
            this.ptbAF.HeadImage = null;
            this.ptbAF.LargeChange = 1;
            this.ptbAF.Maximum = 100;
            this.ptbAF.Minimum = 0;
            this.ptbAF.Name = "ptbAF";
            this.ptbAF.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.ptbAF.SmallChange = 1;
            this.ptbAF.TabStop = false;
            this.toolTip1.SetToolTip(this.ptbAF, resources.GetString("ptbAF.ToolTip"));
            this.ptbAF.Value = 50;
            this.ptbAF.Scroll += new PowerSDR.PrettyTrackBar.ScrollHandler(this.ptbAF_Scroll);
            // 
            // ptbVACTXGain
            // 
            resources.ApplyResources(this.ptbVACTXGain, "ptbVACTXGain");
            this.ptbVACTXGain.HeadImage = null;
            this.ptbVACTXGain.LargeChange = 1;
            this.ptbVACTXGain.Maximum = 40;
            this.ptbVACTXGain.Minimum = -40;
            this.ptbVACTXGain.Name = "ptbVACTXGain";
            this.ptbVACTXGain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.ptbVACTXGain.SmallChange = 1;
            this.ptbVACTXGain.TabStop = false;
            this.toolTip1.SetToolTip(this.ptbVACTXGain, resources.GetString("ptbVACTXGain.ToolTip"));
            this.ptbVACTXGain.Value = 0;
            this.ptbVACTXGain.Scroll += new PowerSDR.PrettyTrackBar.ScrollHandler(this.ptbVACTXGain_Scroll);
            // 
            // ptbVACRXGain
            // 
            resources.ApplyResources(this.ptbVACRXGain, "ptbVACRXGain");
            this.ptbVACRXGain.HeadImage = null;
            this.ptbVACRXGain.LargeChange = 1;
            this.ptbVACRXGain.Maximum = 40;
            this.ptbVACRXGain.Minimum = -40;
            this.ptbVACRXGain.Name = "ptbVACRXGain";
            this.ptbVACRXGain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.ptbVACRXGain.SmallChange = 1;
            this.ptbVACRXGain.TabStop = false;
            this.toolTip1.SetToolTip(this.ptbVACRXGain, resources.GetString("ptbVACRXGain.ToolTip"));
            this.ptbVACRXGain.Value = 0;
            this.ptbVACRXGain.Scroll += new PowerSDR.PrettyTrackBar.ScrollHandler(this.ptbVACRXGain_Scroll);
            // 
            // ptbDisplayZoom
            // 
            resources.ApplyResources(this.ptbDisplayZoom, "ptbDisplayZoom");
            this.ptbDisplayZoom.HeadImage = null;
            this.ptbDisplayZoom.LargeChange = 1;
            this.ptbDisplayZoom.Maximum = 260;
            this.ptbDisplayZoom.Minimum = 1;
            this.ptbDisplayZoom.Name = "ptbDisplayZoom";
            this.ptbDisplayZoom.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.ptbDisplayZoom.SmallChange = 1;
            this.ptbDisplayZoom.TabStop = false;
            this.toolTip1.SetToolTip(this.ptbDisplayZoom, resources.GetString("ptbDisplayZoom.ToolTip"));
            this.ptbDisplayZoom.Value = 150;
            this.ptbDisplayZoom.Scroll += new PowerSDR.PrettyTrackBar.ScrollHandler(this.ptbDisplayZoom_Scroll);
            this.ptbDisplayZoom.MouseHover += new System.EventHandler(this.ptbDisplayZoom_MouseHover);
            // 
            // ptbDisplayPan
            // 
            resources.ApplyResources(this.ptbDisplayPan, "ptbDisplayPan");
            this.ptbDisplayPan.HeadImage = null;
            this.ptbDisplayPan.LargeChange = 1;
            this.ptbDisplayPan.Maximum = 1000;
            this.ptbDisplayPan.Minimum = 0;
            this.ptbDisplayPan.Name = "ptbDisplayPan";
            this.ptbDisplayPan.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.ptbDisplayPan.SmallChange = 1;
            this.ptbDisplayPan.TabStop = false;
            this.toolTip1.SetToolTip(this.ptbDisplayPan, resources.GetString("ptbDisplayPan.ToolTip"));
            this.ptbDisplayPan.Value = 500;
            this.ptbDisplayPan.Scroll += new PowerSDR.PrettyTrackBar.ScrollHandler(this.ptbDisplayPan_Scroll);
            // 
            // ptbFilterShift
            // 
            resources.ApplyResources(this.ptbFilterShift, "ptbFilterShift");
            this.ptbFilterShift.HeadImage = null;
            this.ptbFilterShift.LargeChange = 1;
            this.ptbFilterShift.Maximum = 1000;
            this.ptbFilterShift.Minimum = -1000;
            this.ptbFilterShift.Name = "ptbFilterShift";
            this.ptbFilterShift.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.ptbFilterShift.SmallChange = 1;
            this.ptbFilterShift.TabStop = false;
            this.toolTip1.SetToolTip(this.ptbFilterShift, resources.GetString("ptbFilterShift.ToolTip"));
            this.ptbFilterShift.Value = 0;
            this.ptbFilterShift.Scroll += new PowerSDR.PrettyTrackBar.ScrollHandler(this.ptbFilterShift_Scroll);
            // 
            // ptbFilterWidth
            // 
            resources.ApplyResources(this.ptbFilterWidth, "ptbFilterWidth");
            this.ptbFilterWidth.HeadImage = null;
            this.ptbFilterWidth.LargeChange = 1;
            this.ptbFilterWidth.Maximum = 52000;
            this.ptbFilterWidth.Minimum = 0;
            this.ptbFilterWidth.Name = "ptbFilterWidth";
            this.ptbFilterWidth.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.ptbFilterWidth.SmallChange = 1;
            this.ptbFilterWidth.TabStop = false;
            this.toolTip1.SetToolTip(this.ptbFilterWidth, resources.GetString("ptbFilterWidth.ToolTip"));
            this.ptbFilterWidth.Value = 10;
            this.ptbFilterWidth.Scroll += new PowerSDR.PrettyTrackBar.ScrollHandler(this.ptbFilterWidth_Scroll);
            // 
            // txtNOAA2
            // 
            this.txtNOAA2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.txtNOAA2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.txtNOAA2, "txtNOAA2");
            this.txtNOAA2.ForeColor = System.Drawing.Color.White;
            this.txtNOAA2.Name = "txtNOAA2";
            this.txtNOAA2.ReadOnly = true;
            this.toolTip1.SetToolTip(this.txtNOAA2, resources.GetString("txtNOAA2.ToolTip"));
            this.txtNOAA2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txtNOAA_MouseDown);
            this.txtNOAA2.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.labelTS4_MouseWheel);
            // 
            // txtNOAA
            // 
            this.txtNOAA.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.txtNOAA.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.txtNOAA, "txtNOAA");
            this.txtNOAA.ForeColor = System.Drawing.Color.White;
            this.txtNOAA.Name = "txtNOAA";
            this.txtNOAA.ReadOnly = true;
            this.toolTip1.SetToolTip(this.txtNOAA, resources.GetString("txtNOAA.ToolTip"));
            this.txtNOAA.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txtNOAA_MouseDown);
            this.txtNOAA.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.labelTS4_MouseWheel);
            // 
            // udTXFilterLow
            // 
            this.udTXFilterLow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            resources.ApplyResources(this.udTXFilterLow, "udTXFilterLow");
            this.udTXFilterLow.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.udTXFilterLow.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.udTXFilterLow.Maximum = new decimal(new int[] {
            20000,
            0,
            0,
            0});
            this.udTXFilterLow.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.udTXFilterLow.Name = "udTXFilterLow";
            this.toolTip1.SetToolTip(this.udTXFilterLow, resources.GetString("udTXFilterLow.ToolTip"));
            this.udTXFilterLow.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.udTXFilterLow.ValueChanged += new System.EventHandler(this.udTXFilterLow_ValueChanged);
            this.udTXFilterLow.MouseDown += new System.Windows.Forms.MouseEventHandler(this.udTXFilterLow_MouseDown);
            // 
            // udTXFilterHigh
            // 
            this.udTXFilterHigh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            resources.ApplyResources(this.udTXFilterHigh, "udTXFilterHigh");
            this.udTXFilterHigh.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.udTXFilterHigh.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.udTXFilterHigh.Maximum = new decimal(new int[] {
            20000,
            0,
            0,
            0});
            this.udTXFilterHigh.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.udTXFilterHigh.Name = "udTXFilterHigh";
            this.toolTip1.SetToolTip(this.udTXFilterHigh, resources.GetString("udTXFilterHigh.ToolTip"));
            this.udTXFilterHigh.Value = new decimal(new int[] {
            6000,
            0,
            0,
            0});
            this.udTXFilterHigh.ValueChanged += new System.EventHandler(this.udTXFilterHigh_ValueChanged);
            this.udTXFilterHigh.MouseDown += new System.Windows.Forms.MouseEventHandler(this.udTXFilterHigh_MouseDown);
            // 
            // chkBoxMuteSpk
            // 
            resources.ApplyResources(this.chkBoxMuteSpk, "chkBoxMuteSpk");
            this.chkBoxMuteSpk.Name = "chkBoxMuteSpk";
            this.toolTip1.SetToolTip(this.chkBoxMuteSpk, resources.GetString("chkBoxMuteSpk.ToolTip"));
            this.chkBoxMuteSpk.CheckedChanged += new System.EventHandler(this.chkBoxMuteSpk_CheckedChanged);
            // 
            // chkBoxDrive
            // 
            resources.ApplyResources(this.chkBoxDrive, "chkBoxDrive");
            this.chkBoxDrive.Name = "chkBoxDrive";
            this.toolTip1.SetToolTip(this.chkBoxDrive, resources.GetString("chkBoxDrive.ToolTip"));
            this.chkBoxDrive.CheckedChanged += new System.EventHandler(this.chkBoxDrive_CheckedChanged);
            // 
            // lblPWR
            // 
            resources.ApplyResources(this.lblPWR, "lblPWR");
            this.lblPWR.ForeColor = System.Drawing.Color.White;
            this.lblPWR.Name = "lblPWR";
            this.toolTip1.SetToolTip(this.lblPWR, resources.GetString("lblPWR.ToolTip"));
            this.lblPWR.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblPWR_MouseDown);
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            resources.ApplyResources(this.textBox1, "textBox1");
            this.textBox1.ForeColor = System.Drawing.Color.LightYellow;
            this.textBox1.HideSelection = false;
            this.textBox1.Name = "textBox1";
            this.textBox1.TabStop = false;
            this.toolTip1.SetToolTip(this.textBox1, resources.GetString("textBox1.ToolTip"));
            this.textBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.textBox1_MouseDown);
            this.textBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.textBox1_MouseUp);
            this.textBox1.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.labelTS4_MouseWheel);
            // 
            // buttonAdd
            // 
            resources.ApplyResources(this.buttonAdd, "buttonAdd");
            this.buttonAdd.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonAdd.ForeColor = System.Drawing.Color.Black;
            this.buttonAdd.Name = "buttonAdd";
            this.toolTip1.SetToolTip(this.buttonAdd, resources.GetString("buttonAdd.ToolTip"));
            this.buttonAdd.UseVisualStyleBackColor = false;
            this.buttonAdd.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonAdd_MouseUp);
            // 
            // buttonSort
            // 
            resources.ApplyResources(this.buttonSort, "buttonSort");
            this.buttonSort.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonSort.ForeColor = System.Drawing.Color.Black;
            this.buttonSort.Name = "buttonSort";
            this.toolTip1.SetToolTip(this.buttonSort, resources.GetString("buttonSort.ToolTip"));
            this.buttonSort.UseVisualStyleBackColor = false;
            this.buttonSort.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonSort_MouseUp);
            // 
            // buttonDel
            // 
            resources.ApplyResources(this.buttonDel, "buttonDel");
            this.buttonDel.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonDel.ForeColor = System.Drawing.Color.Black;
            this.buttonDel.Name = "buttonDel";
            this.toolTip1.SetToolTip(this.buttonDel, resources.GetString("buttonDel.ToolTip"));
            this.buttonDel.UseVisualStyleBackColor = false;
            this.buttonDel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonDel_MouseUp);
            // 
            // txtTimer
            // 
            this.txtTimer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.txtTimer.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.txtTimer, "txtTimer");
            this.txtTimer.ForeColor = System.Drawing.Color.White;
            this.txtTimer.Name = "txtTimer";
            this.txtTimer.ReadOnly = true;
            this.toolTip1.SetToolTip(this.txtTimer, resources.GetString("txtTimer.ToolTip"));
            this.txtTimer.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTimer_KeyDown);
            this.txtTimer.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txtTimer_MouseDown);
            this.txtTimer.MouseUp += new System.Windows.Forms.MouseEventHandler(this.txtTimer_MouseUp);
            this.txtTimer.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.labelTS4_MouseWheel);
            // 
            // ptbTune
            // 
            this.ptbTune.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.ptbTune, "ptbTune");
            this.ptbTune.HeadImage = null;
            this.ptbTune.LargeChange = 1;
            this.ptbTune.Maximum = 100;
            this.ptbTune.Minimum = 0;
            this.ptbTune.Name = "ptbTune";
            this.ptbTune.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.ptbTune.SmallChange = 1;
            this.ptbTune.TabStop = false;
            this.toolTip1.SetToolTip(this.ptbTune, resources.GetString("ptbTune.ToolTip"));
            this.ptbTune.Value = 50;
            this.ptbTune.Scroll += new PowerSDR.PrettyTrackBar.ScrollHandler(this.ptbTune_Scroll);
            // 
            // lblTUNE
            // 
            resources.ApplyResources(this.lblTUNE, "lblTUNE");
            this.lblTUNE.ForeColor = System.Drawing.Color.White;
            this.lblTUNE.Name = "lblTUNE";
            this.toolTip1.SetToolTip(this.lblTUNE, resources.GetString("lblTUNE.ToolTip"));
            this.lblTUNE.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lblTUNE_MouseUp);
            // 
            // ptbMON
            // 
            resources.ApplyResources(this.ptbMON, "ptbMON");
            this.ptbMON.HeadImage = null;
            this.ptbMON.LargeChange = 1;
            this.ptbMON.Maximum = 100;
            this.ptbMON.Minimum = 0;
            this.ptbMON.Name = "ptbMON";
            this.ptbMON.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.ptbMON.SmallChange = 1;
            this.ptbMON.TabStop = false;
            this.toolTip1.SetToolTip(this.ptbMON, resources.GetString("ptbMON.ToolTip"));
            this.ptbMON.Value = 50;
            this.ptbMON.Scroll += new PowerSDR.PrettyTrackBar.ScrollHandler(this.ptbMON_Scroll);
            // 
            // lblMON
            // 
            this.lblMON.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.lblMON, "lblMON");
            this.lblMON.Name = "lblMON";
            this.toolTip1.SetToolTip(this.lblMON, resources.GetString("lblMON.ToolTip"));
            this.lblMON.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lblMON_MouseUp);
            // 
            // lblAF
            // 
            this.lblAF.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.lblAF, "lblAF");
            this.lblAF.Name = "lblAF";
            this.toolTip1.SetToolTip(this.lblAF, resources.GetString("lblAF.ToolTip"));
            this.lblAF.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lblAF_MouseUp);
            // 
            // ptbMic
            // 
            resources.ApplyResources(this.ptbMic, "ptbMic");
            this.ptbMic.HeadImage = null;
            this.ptbMic.LargeChange = 1;
            this.ptbMic.Maximum = 70;
            this.ptbMic.Minimum = 0;
            this.ptbMic.Name = "ptbMic";
            this.ptbMic.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.ptbMic.SmallChange = 1;
            this.ptbMic.TabStop = false;
            this.toolTip1.SetToolTip(this.ptbMic, resources.GetString("ptbMic.ToolTip"));
            this.ptbMic.Value = 10;
            this.ptbMic.Scroll += new PowerSDR.PrettyTrackBar.ScrollHandler(this.ptbMic_Scroll);
            this.ptbMic.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ptbMic_MouseDown);
            // 
            // ptbNoiseGate
            // 
            resources.ApplyResources(this.ptbNoiseGate, "ptbNoiseGate");
            this.ptbNoiseGate.HeadImage = null;
            this.ptbNoiseGate.LargeChange = 1;
            this.ptbNoiseGate.Maximum = 0;
            this.ptbNoiseGate.Minimum = -160;
            this.ptbNoiseGate.Name = "ptbNoiseGate";
            this.ptbNoiseGate.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.ptbNoiseGate.SmallChange = 1;
            this.ptbNoiseGate.TabStop = false;
            this.toolTip1.SetToolTip(this.ptbNoiseGate, resources.GetString("ptbNoiseGate.ToolTip"));
            this.ptbNoiseGate.Value = -40;
            this.ptbNoiseGate.Scroll += new PowerSDR.PrettyTrackBar.ScrollHandler(this.ptbNoiseGate_Scroll);
            // 
            // ptbCPDR
            // 
            resources.ApplyResources(this.ptbCPDR, "ptbCPDR");
            this.ptbCPDR.HeadImage = null;
            this.ptbCPDR.LargeChange = 1;
            this.ptbCPDR.Maximum = 10;
            this.ptbCPDR.Minimum = 0;
            this.ptbCPDR.Name = "ptbCPDR";
            this.ptbCPDR.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.ptbCPDR.SmallChange = 1;
            this.ptbCPDR.TabStop = false;
            this.toolTip1.SetToolTip(this.ptbCPDR, resources.GetString("ptbCPDR.ToolTip"));
            this.ptbCPDR.Value = 1;
            this.ptbCPDR.Scroll += new PowerSDR.PrettyTrackBar.ScrollHandler(this.ptbCPDR_Scroll);
            this.ptbCPDR.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ptbCPDR_MouseDown);
            // 
            // ptbDX
            // 
            resources.ApplyResources(this.ptbDX, "ptbDX");
            this.ptbDX.HeadImage = null;
            this.ptbDX.LargeChange = 1;
            this.ptbDX.Maximum = 10;
            this.ptbDX.Minimum = 0;
            this.ptbDX.Name = "ptbDX";
            this.ptbDX.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.ptbDX.SmallChange = 1;
            this.ptbDX.TabStop = false;
            this.toolTip1.SetToolTip(this.ptbDX, resources.GetString("ptbDX.ToolTip"));
            this.ptbDX.Value = 10;
            this.ptbDX.Scroll += new PowerSDR.PrettyTrackBar.ScrollHandler(this.ptbDX_Scroll);
            this.ptbDX.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ptbDX_MouseDown);
            // 
            // comboCWTXProfile
            // 
            this.comboCWTXProfile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.comboCWTXProfile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboCWTXProfile.DropDownWidth = 96;
            resources.ApplyResources(this.comboCWTXProfile, "comboCWTXProfile");
            this.comboCWTXProfile.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.comboCWTXProfile.Name = "comboCWTXProfile";
            this.toolTip1.SetToolTip(this.comboCWTXProfile, resources.GetString("comboCWTXProfile.ToolTip"));
            this.comboCWTXProfile.SelectedIndexChanged += new System.EventHandler(this.comboCWTXProfile_SelectedIndexChanged);
            this.comboCWTXProfile.MouseDown += new System.Windows.Forms.MouseEventHandler(this.comboCWTXProfile_MouseDown);
            // 
            // ScreenCap
            // 
            resources.ApplyResources(this.ScreenCap, "ScreenCap");
            this.ScreenCap.Name = "ScreenCap";
            this.ScreenCap.TabStop = false;
            this.toolTip1.SetToolTip(this.ScreenCap, resources.GetString("ScreenCap.ToolTip"));
            this.ScreenCap.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ScreenCap_MouseDown);
            this.ScreenCap.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ScreenCap_MouseUp);
            // 
            // ScreenCap1
            // 
            resources.ApplyResources(this.ScreenCap1, "ScreenCap1");
            this.ScreenCap1.Name = "ScreenCap1";
            this.ScreenCap1.TabStop = false;
            this.toolTip1.SetToolTip(this.ScreenCap1, resources.GetString("ScreenCap1.ToolTip"));
            // 
            // checkVOX
            // 
            resources.ApplyResources(this.checkVOX, "checkVOX");
            this.checkVOX.FlatAppearance.BorderSize = 0;
            this.checkVOX.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.checkVOX.Name = "checkVOX";
            this.toolTip1.SetToolTip(this.checkVOX, resources.GetString("checkVOX.ToolTip"));
            this.checkVOX.CheckedChanged += new System.EventHandler(this.checkVOX_CheckedChanged);
            this.checkVOX.MouseDown += new System.Windows.Forms.MouseEventHandler(this.chkVOX_MouseDown);
            // 
            // ptbVOX
            // 
            resources.ApplyResources(this.ptbVOX, "ptbVOX");
            this.ptbVOX.HeadImage = null;
            this.ptbVOX.LargeChange = 1;
            this.ptbVOX.Maximum = 500;
            this.ptbVOX.Minimum = 0;
            this.ptbVOX.Name = "ptbVOX";
            this.ptbVOX.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.ptbVOX.SmallChange = 1;
            this.ptbVOX.TabStop = false;
            this.toolTip1.SetToolTip(this.ptbVOX, resources.GetString("ptbVOX.ToolTip"));
            this.ptbVOX.Value = 100;
            this.ptbVOX.Scroll += new PowerSDR.PrettyTrackBar.ScrollHandler(this.ptbVOX_Scroll);
            this.ptbVOX.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ptbVOX_MouseDown);
            // 
            // lblMIC
            // 
            resources.ApplyResources(this.lblMIC, "lblMIC");
            this.lblMIC.ForeColor = System.Drawing.Color.White;
            this.lblMIC.Name = "lblMIC";
            this.toolTip1.SetToolTip(this.lblMIC, resources.GetString("lblMIC.ToolTip"));
            // 
            // udCQCQRepeat
            // 
            this.udCQCQRepeat.DecimalPlaces = 1;
            this.udCQCQRepeat.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            resources.ApplyResources(this.udCQCQRepeat, "udCQCQRepeat");
            this.udCQCQRepeat.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.udCQCQRepeat.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.udCQCQRepeat.Name = "udCQCQRepeat";
            this.toolTip1.SetToolTip(this.udCQCQRepeat, resources.GetString("udCQCQRepeat.ToolTip"));
            this.udCQCQRepeat.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.udCQCQRepeat.ValueChanged += new System.EventHandler(this.UdCQCQRepeat_ValueChanged);
            // 
            // lblPreamp
            // 
            this.lblPreamp.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.lblPreamp, "lblPreamp");
            this.lblPreamp.Name = "lblPreamp";
            this.toolTip1.SetToolTip(this.lblPreamp, resources.GetString("lblPreamp.ToolTip"));
            // 
            // buttonCall1
            // 
            resources.ApplyResources(this.buttonCall1, "buttonCall1");
            this.buttonCall1.Name = "buttonCall1";
            this.buttonCall1.TabStop = false;
            this.toolTip1.SetToolTip(this.buttonCall1, resources.GetString("buttonCall1.ToolTip"));
            this.buttonCall1.Click += new System.EventHandler(this.buttonCall_Click);
            this.buttonCall1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonCall_MouseDown);
            // 
            // buttonCQ1
            // 
            resources.ApplyResources(this.buttonCQ1, "buttonCQ1");
            this.buttonCQ1.Name = "buttonCQ1";
            this.buttonCQ1.TabStop = false;
            this.toolTip1.SetToolTip(this.buttonCQ1, resources.GetString("buttonCQ1.ToolTip"));
            this.buttonCQ1.Click += new System.EventHandler(this.btnTrack_Click);
            this.buttonCQ1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonCQ_MouseDown);
            // 
            // lblDisplayPan1
            // 
            resources.ApplyResources(this.lblDisplayPan1, "lblDisplayPan1");
            this.lblDisplayPan1.Name = "lblDisplayPan1";
            this.lblDisplayPan1.TabStop = false;
            this.toolTip1.SetToolTip(this.lblDisplayPan1, resources.GetString("lblDisplayPan1.ToolTip"));
            this.lblDisplayPan1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblDisplayPan_MouseDown);
            // 
            // lblDisplayZoom1
            // 
            resources.ApplyResources(this.lblDisplayZoom1, "lblDisplayZoom1");
            this.lblDisplayZoom1.Name = "lblDisplayZoom1";
            this.lblDisplayZoom1.TabStop = false;
            this.toolTip1.SetToolTip(this.lblDisplayZoom1, resources.GetString("lblDisplayZoom1.ToolTip"));
            this.lblDisplayZoom1.Click += new System.EventHandler(this.lblDisplayZoom_Click);
            this.lblDisplayZoom1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblDisplayZoom_MouseDown);
            this.lblDisplayZoom1.MouseHover += new System.EventHandler(this.lblDisplayZoom_MouseHover);
            // 
            // radRX2Filter1
            // 
            resources.ApplyResources(this.radRX2Filter1, "radRX2Filter1");
            this.radRX2Filter1.FlatAppearance.BorderSize = 0;
            this.radRX2Filter1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radRX2Filter1.Name = "radRX2Filter1";
            this.toolTip1.SetToolTip(this.radRX2Filter1, resources.GetString("radRX2Filter1.ToolTip"));
            this.radRX2Filter1.CheckedChanged += new System.EventHandler(this.radRX2Filter1_CheckedChanged);
            // 
            // radRX2FilterVar2
            // 
            resources.ApplyResources(this.radRX2FilterVar2, "radRX2FilterVar2");
            this.radRX2FilterVar2.FlatAppearance.BorderSize = 0;
            this.radRX2FilterVar2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radRX2FilterVar2.Name = "radRX2FilterVar2";
            this.toolTip1.SetToolTip(this.radRX2FilterVar2, resources.GetString("radRX2FilterVar2.ToolTip"));
            this.radRX2FilterVar2.CheckedChanged += new System.EventHandler(this.radRX2FilterVar2_CheckedChanged);
            // 
            // radRX2FilterVar1
            // 
            resources.ApplyResources(this.radRX2FilterVar1, "radRX2FilterVar1");
            this.radRX2FilterVar1.FlatAppearance.BorderSize = 0;
            this.radRX2FilterVar1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radRX2FilterVar1.Name = "radRX2FilterVar1";
            this.toolTip1.SetToolTip(this.radRX2FilterVar1, resources.GetString("radRX2FilterVar1.ToolTip"));
            this.radRX2FilterVar1.CheckedChanged += new System.EventHandler(this.radRX2FilterVar1_CheckedChanged);
            // 
            // radFilter1
            // 
            resources.ApplyResources(this.radFilter1, "radFilter1");
            this.radFilter1.FlatAppearance.BorderSize = 0;
            this.radFilter1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radFilter1.Name = "radFilter1";
            this.toolTip1.SetToolTip(this.radFilter1, resources.GetString("radFilter1.ToolTip"));
            this.radFilter1.CheckedChanged += new System.EventHandler(this.radFilter1_CheckedChanged);
            // 
            // radFilterVar2
            // 
            resources.ApplyResources(this.radFilterVar2, "radFilterVar2");
            this.radFilterVar2.FlatAppearance.BorderSize = 0;
            this.radFilterVar2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radFilterVar2.Name = "radFilterVar2";
            this.toolTip1.SetToolTip(this.radFilterVar2, resources.GetString("radFilterVar2.ToolTip"));
            this.radFilterVar2.CheckedChanged += new System.EventHandler(this.radFilterVar2_CheckedChanged);
            // 
            // radFilterVar1
            // 
            resources.ApplyResources(this.radFilterVar1, "radFilterVar1");
            this.radFilterVar1.FlatAppearance.BorderSize = 0;
            this.radFilterVar1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radFilterVar1.Name = "radFilterVar1";
            this.toolTip1.SetToolTip(this.radFilterVar1, resources.GetString("radFilterVar1.ToolTip"));
            this.radFilterVar1.CheckedChanged += new System.EventHandler(this.radFilterVar1_CheckedChanged);
            // 
            // buttonVK1
            // 
            resources.ApplyResources(this.buttonVK1, "buttonVK1");
            this.buttonVK1.Name = "buttonVK1";
            this.buttonVK1.TabStop = false;
            this.toolTip1.SetToolTip(this.buttonVK1, resources.GetString("buttonVK1.ToolTip"));
            this.buttonVK1.Click += new System.EventHandler(this.buttonVK1_Click);
            this.buttonVK1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonVK1_MouseDown);
            // 
            // buttonVK2
            // 
            resources.ApplyResources(this.buttonVK2, "buttonVK2");
            this.buttonVK2.Name = "buttonVK2";
            this.buttonVK2.TabStop = false;
            this.toolTip1.SetToolTip(this.buttonVK2, resources.GetString("buttonVK2.ToolTip"));
            this.buttonVK2.Click += new System.EventHandler(this.buttonVK2_Click);
            this.buttonVK2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonVK2_MouseDown);
            // 
            // textBox2
            // 
            resources.ApplyResources(this.textBox2, "textBox2");
            this.textBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.textBox2.ForeColor = System.Drawing.Color.LightYellow;
            this.textBox2.HideSelection = false;
            this.textBox2.Name = "textBox2";
            this.textBox2.TabStop = false;
            this.toolTip1.SetToolTip(this.textBox2, resources.GetString("textBox2.ToolTip"));
            this.textBox2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.textBox2_MouseDown);
            this.textBox2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.textBox2_MouseUp);
            // 
            // lblAntTX2
            // 
            resources.ApplyResources(this.lblAntTX2, "lblAntTX2");
            this.lblAntTX2.ForeColor = System.Drawing.Color.White;
            this.lblAntTX2.Name = "lblAntTX2";
            this.toolTip1.SetToolTip(this.lblAntTX2, resources.GetString("lblAntTX2.ToolTip"));
            this.lblAntTX2.Click += new System.EventHandler(this.lblAntTX2_Click);
            // 
            // lblAntRX1a
            // 
            resources.ApplyResources(this.lblAntRX1a, "lblAntRX1a");
            this.lblAntRX1a.ForeColor = System.Drawing.Color.White;
            this.lblAntRX1a.Name = "lblAntRX1a";
            this.toolTip1.SetToolTip(this.lblAntRX1a, resources.GetString("lblAntRX1a.ToolTip"));
            this.lblAntRX1a.Click += new System.EventHandler(this.lblAntRX1_Click);
            // 
            // lblAntTXa
            // 
            resources.ApplyResources(this.lblAntTXa, "lblAntTXa");
            this.lblAntTXa.ForeColor = System.Drawing.Color.White;
            this.lblAntTXa.Name = "lblAntTXa";
            this.toolTip1.SetToolTip(this.lblAntTXa, resources.GetString("lblAntTXa.ToolTip"));
            this.lblAntTXa.Click += new System.EventHandler(this.lblAntRX2_Click);
            // 
            // lblAntRX2a
            // 
            resources.ApplyResources(this.lblAntRX2a, "lblAntRX2a");
            this.lblAntRX2a.ForeColor = System.Drawing.Color.White;
            this.lblAntRX2a.Name = "lblAntRX2a";
            this.toolTip1.SetToolTip(this.lblAntRX2a, resources.GetString("lblAntRX2a.ToolTip"));
            this.lblAntRX2a.Click += new System.EventHandler(this.lblAntRX2_Click);
            // 
            // lblAntTX2a
            // 
            resources.ApplyResources(this.lblAntTX2a, "lblAntTX2a");
            this.lblAntTX2a.ForeColor = System.Drawing.Color.White;
            this.lblAntTX2a.Name = "lblAntTX2a";
            this.toolTip1.SetToolTip(this.lblAntTX2a, resources.GetString("lblAntTX2a.ToolTip"));
            this.lblAntTX2a.Click += new System.EventHandler(this.lblAntTX_Click);
            // 
            // ptbDisplayZoom2
            // 
            resources.ApplyResources(this.ptbDisplayZoom2, "ptbDisplayZoom2");
            this.ptbDisplayZoom2.HeadImage = null;
            this.ptbDisplayZoom2.LargeChange = 1;
            this.ptbDisplayZoom2.Maximum = 260;
            this.ptbDisplayZoom2.Minimum = 1;
            this.ptbDisplayZoom2.Name = "ptbDisplayZoom2";
            this.ptbDisplayZoom2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.ptbDisplayZoom2.SmallChange = 1;
            this.ptbDisplayZoom2.TabStop = false;
            this.toolTip1.SetToolTip(this.ptbDisplayZoom2, resources.GetString("ptbDisplayZoom2.ToolTip"));
            this.ptbDisplayZoom2.Value = 150;
            this.ptbDisplayZoom2.Scroll += new PowerSDR.PrettyTrackBar.ScrollHandler(this.ptbDisplayZoom_Scroll);
            this.ptbDisplayZoom2.MouseHover += new System.EventHandler(this.ptbDisplayZoom2_MouseHover);
            // 
            // ptbDisplayPan2
            // 
            resources.ApplyResources(this.ptbDisplayPan2, "ptbDisplayPan2");
            this.ptbDisplayPan2.HeadImage = null;
            this.ptbDisplayPan2.LargeChange = 1;
            this.ptbDisplayPan2.Maximum = 1000;
            this.ptbDisplayPan2.Minimum = 0;
            this.ptbDisplayPan2.Name = "ptbDisplayPan2";
            this.ptbDisplayPan2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.ptbDisplayPan2.SmallChange = 1;
            this.ptbDisplayPan2.TabStop = false;
            this.toolTip1.SetToolTip(this.ptbDisplayPan2, resources.GetString("ptbDisplayPan2.ToolTip"));
            this.ptbDisplayPan2.Value = 500;
            this.ptbDisplayPan2.Scroll += new PowerSDR.PrettyTrackBar.ScrollHandler(this.ptbDisplayPan2_Scroll);
            // 
            // timer_clock
            // 
            this.timer_clock.Enabled = true;
            this.timer_clock.Tick += new System.EventHandler(this.timer_clock_Tick);
            // 
            // contextMenuStripFilterRX1
            // 
            this.contextMenuStripFilterRX1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemRX1FilterConfigure,
            this.toolStripMenuItemRX1FilterReset});
            this.contextMenuStripFilterRX1.Name = "contextMenuStripFilterRX1";
            resources.ApplyResources(this.contextMenuStripFilterRX1, "contextMenuStripFilterRX1");
            // 
            // toolStripMenuItemRX1FilterConfigure
            // 
            this.toolStripMenuItemRX1FilterConfigure.Name = "toolStripMenuItemRX1FilterConfigure";
            resources.ApplyResources(this.toolStripMenuItemRX1FilterConfigure, "toolStripMenuItemRX1FilterConfigure");
            this.toolStripMenuItemRX1FilterConfigure.Click += new System.EventHandler(this.toolStripMenuItemRX1FilterConfigure_Click);
            // 
            // toolStripMenuItemRX1FilterReset
            // 
            this.toolStripMenuItemRX1FilterReset.Name = "toolStripMenuItemRX1FilterReset";
            resources.ApplyResources(this.toolStripMenuItemRX1FilterReset, "toolStripMenuItemRX1FilterReset");
            this.toolStripMenuItemRX1FilterReset.Click += new System.EventHandler(this.toolStripMenuItemRX1FilterReset_Click);
            // 
            // contextMenuStripFilterRX2
            // 
            this.contextMenuStripFilterRX2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemRX2FilterConfigure,
            this.toolStripMenuItemRX2FilterReset});
            this.contextMenuStripFilterRX2.Name = "contextMenuStripFilterRX2";
            resources.ApplyResources(this.contextMenuStripFilterRX2, "contextMenuStripFilterRX2");
            // 
            // toolStripMenuItemRX2FilterConfigure
            // 
            this.toolStripMenuItemRX2FilterConfigure.Name = "toolStripMenuItemRX2FilterConfigure";
            resources.ApplyResources(this.toolStripMenuItemRX2FilterConfigure, "toolStripMenuItemRX2FilterConfigure");
            this.toolStripMenuItemRX2FilterConfigure.Click += new System.EventHandler(this.toolStripMenuItemRX2FilterConfigure_Click);
            // 
            // toolStripMenuItemRX2FilterReset
            // 
            this.toolStripMenuItemRX2FilterReset.Name = "toolStripMenuItemRX2FilterReset";
            resources.ApplyResources(this.toolStripMenuItemRX2FilterReset, "toolStripMenuItemRX2FilterReset");
            this.toolStripMenuItemRX2FilterReset.Click += new System.EventHandler(this.toolStripMenuItemRX2FilterReset_Click);
            // 
            // timer_navigate
            // 
            this.timer_navigate.Tick += new System.EventHandler(this.timer_navigate_Tick);
            // 
            // timer2
            // 
            this.timer2.Interval = 200;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // contextMenuStripNotch
            // 
            this.contextMenuStripNotch.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripNotchDelete,
            this.toolStripNotchRemember,
            this.toolStripSeparator1,
            this.toolStripNotchNormal,
            this.toolStripNotchDeep,
            this.toolStripNotchVeryDeep});
            this.contextMenuStripNotch.Name = "contextMenuStripNotch";
            resources.ApplyResources(this.contextMenuStripNotch, "contextMenuStripNotch");
            // 
            // toolStripNotchDelete
            // 
            this.toolStripNotchDelete.Name = "toolStripNotchDelete";
            resources.ApplyResources(this.toolStripNotchDelete, "toolStripNotchDelete");
            this.toolStripNotchDelete.Click += new System.EventHandler(this.toolStripNotchDelete_Click);
            // 
            // toolStripNotchRemember
            // 
            this.toolStripNotchRemember.Name = "toolStripNotchRemember";
            resources.ApplyResources(this.toolStripNotchRemember, "toolStripNotchRemember");
            this.toolStripNotchRemember.Click += new System.EventHandler(this.toolStripNotchRemember_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // toolStripNotchNormal
            // 
            this.toolStripNotchNormal.Name = "toolStripNotchNormal";
            resources.ApplyResources(this.toolStripNotchNormal, "toolStripNotchNormal");
            this.toolStripNotchNormal.Click += new System.EventHandler(this.toolStripNotchNormal_Click);
            // 
            // toolStripNotchDeep
            // 
            this.toolStripNotchDeep.Checked = true;
            this.toolStripNotchDeep.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripNotchDeep.Name = "toolStripNotchDeep";
            resources.ApplyResources(this.toolStripNotchDeep, "toolStripNotchDeep");
            this.toolStripNotchDeep.Click += new System.EventHandler(this.toolStripNotchDeep_Click);
            // 
            // toolStripNotchVeryDeep
            // 
            this.toolStripNotchVeryDeep.Name = "toolStripNotchVeryDeep";
            resources.ApplyResources(this.toolStripNotchVeryDeep, "toolStripNotchVeryDeep");
            this.toolStripNotchVeryDeep.Click += new System.EventHandler(this.toolStripNotchVeryDeep_Click);
            // 
            // timerNotchZoom
            // 
            this.timerNotchZoom.Interval = 1000;
            this.timerNotchZoom.Tick += new System.EventHandler(this.timerNotchZoom_Tick);
            // 
            // menuStrip1
            // 
            resources.ApplyResources(this.menuStrip1, "menuStrip1");
            this.menuStrip1.BackColor = System.Drawing.Color.Transparent;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setupToolStripMenuItem,
            this.memoryToolStripMenuItem,
            this.waveToolStripMenuItem,
            this.equalizerToolStripMenuItem,
            this.xVTRsToolStripMenuItem,
            this.cWXToolStripMenuItem,
            this.uCBToolStripMenuItem,
            this.mixerToolStripMenuItem,
            this.eSCToolStripMenuItem,
            this.antennaToolStripMenuItem,
            this.relaysToolStripMenuItem,
            this.aTUToolStripMenuItem,
            this.flexControlToolStripMenuItem,
            this.GrayMenuItem,
            this.TXIDMenuItem,
            this.callsignTextBox,
            this.ScanMenuItem,
            this.spotterMenu,
            this.MapMenuItem,
            this.SWLMenuItem,
            this.herosToolStripMenuItem,
            this.aboutToolStripMenuItem,
            this.remoteProfilesToolStripMenuItem,
            this.reportBugToolStripMenuItem});
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.ShowItemToolTips = true;
            // 
            // setupToolStripMenuItem
            // 
            this.setupToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.setupToolStripMenuItem.Name = "setupToolStripMenuItem";
            resources.ApplyResources(this.setupToolStripMenuItem, "setupToolStripMenuItem");
            this.setupToolStripMenuItem.Click += new System.EventHandler(this.setupToolStripMenuItem_Click);
            // 
            // memoryToolStripMenuItem
            // 
            this.memoryToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.memoryToolStripMenuItem.Name = "memoryToolStripMenuItem";
            resources.ApplyResources(this.memoryToolStripMenuItem, "memoryToolStripMenuItem");
            this.memoryToolStripMenuItem.Click += new System.EventHandler(this.memoryToolStripMenuItem_Click);
            // 
            // waveToolStripMenuItem
            // 
            this.waveToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.waveToolStripMenuItem.Name = "waveToolStripMenuItem";
            resources.ApplyResources(this.waveToolStripMenuItem, "waveToolStripMenuItem");
            this.waveToolStripMenuItem.Click += new System.EventHandler(this.waveToolStripMenuItem_Click);
            // 
            // equalizerToolStripMenuItem
            // 
            this.equalizerToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.equalizerToolStripMenuItem.Name = "equalizerToolStripMenuItem";
            resources.ApplyResources(this.equalizerToolStripMenuItem, "equalizerToolStripMenuItem");
            this.equalizerToolStripMenuItem.Click += new System.EventHandler(this.equalizerToolStripMenuItem_Click);
            // 
            // xVTRsToolStripMenuItem
            // 
            this.xVTRsToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.xVTRsToolStripMenuItem.Name = "xVTRsToolStripMenuItem";
            resources.ApplyResources(this.xVTRsToolStripMenuItem, "xVTRsToolStripMenuItem");
            this.xVTRsToolStripMenuItem.Click += new System.EventHandler(this.xVTRsToolStripMenuItem_Click);
            // 
            // cWXToolStripMenuItem
            // 
            this.cWXToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.cWXToolStripMenuItem.Name = "cWXToolStripMenuItem";
            resources.ApplyResources(this.cWXToolStripMenuItem, "cWXToolStripMenuItem");
            this.cWXToolStripMenuItem.Click += new System.EventHandler(this.cWXToolStripMenuItem_Click);
            // 
            // uCBToolStripMenuItem
            // 
            this.uCBToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.uCBToolStripMenuItem.Name = "uCBToolStripMenuItem";
            resources.ApplyResources(this.uCBToolStripMenuItem, "uCBToolStripMenuItem");
            this.uCBToolStripMenuItem.Click += new System.EventHandler(this.uCBToolStripMenuItem_Click);
            // 
            // mixerToolStripMenuItem
            // 
            this.mixerToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.mixerToolStripMenuItem.Name = "mixerToolStripMenuItem";
            resources.ApplyResources(this.mixerToolStripMenuItem, "mixerToolStripMenuItem");
            this.mixerToolStripMenuItem.Click += new System.EventHandler(this.mixerToolStripMenuItem_Click);
            // 
            // eSCToolStripMenuItem
            // 
            resources.ApplyResources(this.eSCToolStripMenuItem, "eSCToolStripMenuItem");
            this.eSCToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.eSCToolStripMenuItem.Name = "eSCToolStripMenuItem";
            this.eSCToolStripMenuItem.Click += new System.EventHandler(this.eSCToolStripMenuItem_Click);
            this.eSCToolStripMenuItem.MouseLeave += new System.EventHandler(this.ESCToolStripMenuItem_MouseLeave);
            this.eSCToolStripMenuItem.MouseHover += new System.EventHandler(this.ESCToolStripMenuItem_MouseHover);
            // 
            // antennaToolStripMenuItem
            // 
            this.antennaToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.antennaToolStripMenuItem.Name = "antennaToolStripMenuItem";
            resources.ApplyResources(this.antennaToolStripMenuItem, "antennaToolStripMenuItem");
            this.antennaToolStripMenuItem.Click += new System.EventHandler(this.antennaToolStripMenuItem_Click);
            // 
            // relaysToolStripMenuItem
            // 
            this.relaysToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.relaysToolStripMenuItem.Name = "relaysToolStripMenuItem";
            resources.ApplyResources(this.relaysToolStripMenuItem, "relaysToolStripMenuItem");
            this.relaysToolStripMenuItem.Click += new System.EventHandler(this.relaysToolStripMenuItem_Click);
            // 
            // aTUToolStripMenuItem
            // 
            this.aTUToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.aTUToolStripMenuItem.Name = "aTUToolStripMenuItem";
            resources.ApplyResources(this.aTUToolStripMenuItem, "aTUToolStripMenuItem");
            this.aTUToolStripMenuItem.Click += new System.EventHandler(this.aTUToolStripMenuItem_Click);
            // 
            // flexControlToolStripMenuItem
            // 
            this.flexControlToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.flexControlToolStripMenuItem.Name = "flexControlToolStripMenuItem";
            resources.ApplyResources(this.flexControlToolStripMenuItem, "flexControlToolStripMenuItem");
            this.flexControlToolStripMenuItem.Click += new System.EventHandler(this.flexControlToolStripMenuItem_Click);
            // 
            // GrayMenuItem
            // 
            this.GrayMenuItem.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.GrayMenuItem.Name = "GrayMenuItem";
            resources.ApplyResources(this.GrayMenuItem, "GrayMenuItem");
            this.GrayMenuItem.Click += new System.EventHandler(this.GrayMenuItem_Click);
            this.GrayMenuItem.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GrayMenuItem_MouseDown);
            // 
            // TXIDMenuItem
            // 
            resources.ApplyResources(this.TXIDMenuItem, "TXIDMenuItem");
            this.TXIDMenuItem.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.TXIDMenuItem.Name = "TXIDMenuItem";
            this.TXIDMenuItem.CheckedChanged += new System.EventHandler(this.TXIDMenuItem_CheckedChanged);
            this.TXIDMenuItem.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TXIDMenuItem_MouseDown);
            this.TXIDMenuItem.MouseHover += new System.EventHandler(this.TXIDMenuItem_MouseHover);
            // 
            // callsignTextBox
            // 
            resources.ApplyResources(this.callsignTextBox, "callsignTextBox");
            this.callsignTextBox.BackColor = System.Drawing.Color.WhiteSmoke;
            this.callsignTextBox.Margin = new System.Windows.Forms.Padding(1, 1, 1, 0);
            this.callsignTextBox.Name = "callsignTextBox";
            this.callsignTextBox.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.callsignTextBox.Leave += new System.EventHandler(this.callsignTextBox_Leave);
            this.callsignTextBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.callsignTextBox_MouseDown);
            this.callsignTextBox.MouseEnter += new System.EventHandler(this.callsignTextBox_MouseEnter);
            this.callsignTextBox.MouseLeave += new System.EventHandler(this.callsignTextBox_MouseLeave);
            this.callsignTextBox.TextChanged += new System.EventHandler(this.callsignTextBox_TextChanged_1);
            // 
            // ScanMenuItem
            // 
            resources.ApplyResources(this.ScanMenuItem, "ScanMenuItem");
            this.ScanMenuItem.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.ScanMenuItem.Name = "ScanMenuItem";
            this.ScanMenuItem.Click += new System.EventHandler(this.ScanMenuItem_Click);
            // 
            // spotterMenu
            // 
            resources.ApplyResources(this.spotterMenu, "spotterMenu");
            this.spotterMenu.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.spotterMenu.Name = "spotterMenu";
            this.spotterMenu.Click += new System.EventHandler(this.spotterMenu_Click);
            this.spotterMenu.MouseDown += new System.Windows.Forms.MouseEventHandler(this.spotterMenu_MouseDown);
            // 
            // MapMenuItem
            // 
            resources.ApplyResources(this.MapMenuItem, "MapMenuItem");
            this.MapMenuItem.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.MapMenuItem.Name = "MapMenuItem";
            this.MapMenuItem.Click += new System.EventHandler(this.trackMenuItem1_Click);
            this.MapMenuItem.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MapMenuItem_MouseDown);
            this.MapMenuItem.MouseEnter += new System.EventHandler(this.trackMenuItem1_MouseEnter);
            this.MapMenuItem.MouseLeave += new System.EventHandler(this.trackMenuItem1_MouseLeave);
            // 
            // SWLMenuItem
            // 
            resources.ApplyResources(this.SWLMenuItem, "SWLMenuItem");
            this.SWLMenuItem.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.SWLMenuItem.Name = "SWLMenuItem";
            this.SWLMenuItem.Click += new System.EventHandler(this.SWLMenuItem_Click);
            // 
            // herosToolStripMenuItem
            // 
            this.herosToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.herosToolStripMenuItem.Name = "herosToolStripMenuItem";
            resources.ApplyResources(this.herosToolStripMenuItem, "herosToolStripMenuItem");
            this.herosToolStripMenuItem.Click += new System.EventHandler(this.herosToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            resources.ApplyResources(this.aboutToolStripMenuItem, "aboutToolStripMenuItem");
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // remoteProfilesToolStripMenuItem
            // 
            this.remoteProfilesToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.remoteProfilesToolStripMenuItem.Name = "remoteProfilesToolStripMenuItem";
            resources.ApplyResources(this.remoteProfilesToolStripMenuItem, "remoteProfilesToolStripMenuItem");
            this.remoteProfilesToolStripMenuItem.Click += new System.EventHandler(this.remoteProfilesToolStripMenuItem_Click);
            // 
            // reportBugToolStripMenuItem
            // 
            this.reportBugToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.reportBugToolStripMenuItem.Name = "reportBugToolStripMenuItem";
            resources.ApplyResources(this.reportBugToolStripMenuItem, "reportBugToolStripMenuItem");
            this.reportBugToolStripMenuItem.Click += new System.EventHandler(this.reportBugToolStripMenuItem_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "QuickAudio";
            // 
            // picRX2Squelch
            // 
            this.picRX2Squelch.BackColor = System.Drawing.SystemColors.ControlText;
            resources.ApplyResources(this.picRX2Squelch, "picRX2Squelch");
            this.picRX2Squelch.Name = "picRX2Squelch";
            this.picRX2Squelch.TabStop = false;
            this.picRX2Squelch.Paint += new System.Windows.Forms.PaintEventHandler(this.picRX2Squelch_Paint);
            // 
            // picSquelch
            // 
            this.picSquelch.BackColor = System.Drawing.SystemColors.ControlText;
            resources.ApplyResources(this.picSquelch, "picSquelch");
            this.picSquelch.Name = "picSquelch";
            this.picSquelch.TabStop = false;
            this.picSquelch.Paint += new System.Windows.Forms.PaintEventHandler(this.picSquelch_Paint);
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            resources.ApplyResources(this.imageList1, "imageList1");
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // VFODialA
            // 
            this.VFODialA.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.VFODialA, "VFODialA");
            this.VFODialA.Name = "VFODialA";
            this.VFODialA.TabStop = false;
            // 
            // VFODialB
            // 
            this.VFODialB.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.VFODialB, "VFODialB");
            this.VFODialB.Image = global::PowerSDR.Properties.Resources.disk3;
            this.VFODialB.Name = "VFODialB";
            this.VFODialB.TabStop = false;
            // 
            // VFODialAA
            // 
            this.VFODialAA.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.VFODialAA, "VFODialAA");
            this.VFODialAA.Name = "VFODialAA";
            this.VFODialAA.TabStop = false;
            this.VFODialAA.Paint += new System.Windows.Forms.PaintEventHandler(this.VFODialAA_Paint);
            this.VFODialAA.MouseEnter += new System.EventHandler(this.VFODialAA_MouseEnter);
            this.VFODialAA.MouseLeave += new System.EventHandler(this.VFODialAA_MouseLeave);
            // 
            // VFODialBB
            // 
            this.VFODialBB.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.VFODialBB, "VFODialBB");
            this.VFODialBB.Name = "VFODialBB";
            this.VFODialBB.TabStop = false;
            this.VFODialBB.Paint += new System.Windows.Forms.PaintEventHandler(this.VFODialBB_Paint);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            resources.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
            // 
            // panelBandVHFRX2
            // 
            resources.ApplyResources(this.panelBandVHFRX2, "panelBandVHFRX2");
            this.panelBandVHFRX2.BackColor = System.Drawing.Color.Transparent;
            this.panelBandVHFRX2.Controls.Add(this.radBandVHF13RX2);
            this.panelBandVHFRX2.Controls.Add(this.radBandVHF12RX2);
            this.panelBandVHFRX2.Controls.Add(this.radBandVHF11RX2);
            this.panelBandVHFRX2.Controls.Add(this.radBandVHF10RX2);
            this.panelBandVHFRX2.Controls.Add(this.radBandVHF9RX2);
            this.panelBandVHFRX2.Controls.Add(this.radBandVHF8RX2);
            this.panelBandVHFRX2.Controls.Add(this.radBandVHF7RX2);
            this.panelBandVHFRX2.Controls.Add(this.radBandVHF6RX2);
            this.panelBandVHFRX2.Controls.Add(this.radBandVHF5RX2);
            this.panelBandVHFRX2.Controls.Add(this.radBandVHF4RX2);
            this.panelBandVHFRX2.Controls.Add(this.radBandVHF3RX2);
            this.panelBandVHFRX2.Controls.Add(this.radBandVHF2RX2);
            this.panelBandVHFRX2.Controls.Add(this.radBandVHF1RX2);
            this.panelBandVHFRX2.Controls.Add(this.radBandVHF0RX2);
            this.panelBandVHFRX2.Controls.Add(this.btnBandHFRX2);
            this.panelBandVHFRX2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.panelBandVHFRX2.Name = "panelBandVHFRX2";
            this.panelBandVHFRX2.VisibleChanged += new System.EventHandler(this.panelBandVHFRX2_VisibleChanged);
            this.panelBandVHFRX2.Paint += new System.Windows.Forms.PaintEventHandler(this.panelRX2Ring_Paint);
            // 
            // radBandVHF13RX2
            // 
            resources.ApplyResources(this.radBandVHF13RX2, "radBandVHF13RX2");
            this.radBandVHF13RX2.FlatAppearance.BorderSize = 0;
            this.radBandVHF13RX2.ForeColor = System.Drawing.Color.White;
            this.radBandVHF13RX2.Name = "radBandVHF13RX2";
            this.radBandVHF13RX2.TabStop = true;
            this.radBandVHF13RX2.UseVisualStyleBackColor = true;
            this.radBandVHF13RX2.Click += new System.EventHandler(this.radBandVHF13RX2_Click);
            // 
            // radBandVHF12RX2
            // 
            resources.ApplyResources(this.radBandVHF12RX2, "radBandVHF12RX2");
            this.radBandVHF12RX2.FlatAppearance.BorderSize = 0;
            this.radBandVHF12RX2.ForeColor = System.Drawing.Color.White;
            this.radBandVHF12RX2.Name = "radBandVHF12RX2";
            this.radBandVHF12RX2.TabStop = true;
            this.radBandVHF12RX2.UseVisualStyleBackColor = true;
            this.radBandVHF12RX2.Click += new System.EventHandler(this.radBandVHF12RX2_Click);
            // 
            // radBandVHF11RX2
            // 
            resources.ApplyResources(this.radBandVHF11RX2, "radBandVHF11RX2");
            this.radBandVHF11RX2.FlatAppearance.BorderSize = 0;
            this.radBandVHF11RX2.ForeColor = System.Drawing.Color.White;
            this.radBandVHF11RX2.Name = "radBandVHF11RX2";
            this.radBandVHF11RX2.TabStop = true;
            this.radBandVHF11RX2.UseVisualStyleBackColor = true;
            this.radBandVHF11RX2.Click += new System.EventHandler(this.radBandVHF11RX2_Click);
            // 
            // radBandVHF10RX2
            // 
            resources.ApplyResources(this.radBandVHF10RX2, "radBandVHF10RX2");
            this.radBandVHF10RX2.FlatAppearance.BorderSize = 0;
            this.radBandVHF10RX2.ForeColor = System.Drawing.Color.White;
            this.radBandVHF10RX2.Name = "radBandVHF10RX2";
            this.radBandVHF10RX2.TabStop = true;
            this.radBandVHF10RX2.UseVisualStyleBackColor = true;
            this.radBandVHF10RX2.Click += new System.EventHandler(this.radBandVHF10RX2_Click);
            // 
            // radBandVHF9RX2
            // 
            resources.ApplyResources(this.radBandVHF9RX2, "radBandVHF9RX2");
            this.radBandVHF9RX2.FlatAppearance.BorderSize = 0;
            this.radBandVHF9RX2.ForeColor = System.Drawing.Color.White;
            this.radBandVHF9RX2.Name = "radBandVHF9RX2";
            this.radBandVHF9RX2.TabStop = true;
            this.radBandVHF9RX2.UseVisualStyleBackColor = true;
            this.radBandVHF9RX2.Click += new System.EventHandler(this.radBandVHF9RX2_Click);
            // 
            // radBandVHF8RX2
            // 
            resources.ApplyResources(this.radBandVHF8RX2, "radBandVHF8RX2");
            this.radBandVHF8RX2.FlatAppearance.BorderSize = 0;
            this.radBandVHF8RX2.ForeColor = System.Drawing.Color.White;
            this.radBandVHF8RX2.Name = "radBandVHF8RX2";
            this.radBandVHF8RX2.TabStop = true;
            this.radBandVHF8RX2.UseVisualStyleBackColor = true;
            this.radBandVHF8RX2.Click += new System.EventHandler(this.radBandVHF8RX2_Click);
            // 
            // radBandVHF7RX2
            // 
            resources.ApplyResources(this.radBandVHF7RX2, "radBandVHF7RX2");
            this.radBandVHF7RX2.FlatAppearance.BorderSize = 0;
            this.radBandVHF7RX2.ForeColor = System.Drawing.Color.White;
            this.radBandVHF7RX2.Name = "radBandVHF7RX2";
            this.radBandVHF7RX2.TabStop = true;
            this.radBandVHF7RX2.UseVisualStyleBackColor = true;
            this.radBandVHF7RX2.Click += new System.EventHandler(this.radBandVHF7RX2_Click);
            // 
            // radBandVHF6RX2
            // 
            resources.ApplyResources(this.radBandVHF6RX2, "radBandVHF6RX2");
            this.radBandVHF6RX2.FlatAppearance.BorderSize = 0;
            this.radBandVHF6RX2.ForeColor = System.Drawing.Color.White;
            this.radBandVHF6RX2.Name = "radBandVHF6RX2";
            this.radBandVHF6RX2.TabStop = true;
            this.radBandVHF6RX2.UseVisualStyleBackColor = true;
            this.radBandVHF6RX2.Click += new System.EventHandler(this.radBandVHF6RX2_Click);
            // 
            // radBandVHF5RX2
            // 
            resources.ApplyResources(this.radBandVHF5RX2, "radBandVHF5RX2");
            this.radBandVHF5RX2.FlatAppearance.BorderSize = 0;
            this.radBandVHF5RX2.ForeColor = System.Drawing.Color.White;
            this.radBandVHF5RX2.Name = "radBandVHF5RX2";
            this.radBandVHF5RX2.TabStop = true;
            this.radBandVHF5RX2.UseVisualStyleBackColor = true;
            this.radBandVHF5RX2.Click += new System.EventHandler(this.radBandVHF5RX2_Click);
            // 
            // radBandVHF4RX2
            // 
            resources.ApplyResources(this.radBandVHF4RX2, "radBandVHF4RX2");
            this.radBandVHF4RX2.FlatAppearance.BorderSize = 0;
            this.radBandVHF4RX2.ForeColor = System.Drawing.Color.White;
            this.radBandVHF4RX2.Name = "radBandVHF4RX2";
            this.radBandVHF4RX2.TabStop = true;
            this.radBandVHF4RX2.UseVisualStyleBackColor = true;
            this.radBandVHF4RX2.Click += new System.EventHandler(this.radBandVHF4RX2_Click);
            // 
            // radBandVHF3RX2
            // 
            resources.ApplyResources(this.radBandVHF3RX2, "radBandVHF3RX2");
            this.radBandVHF3RX2.FlatAppearance.BorderSize = 0;
            this.radBandVHF3RX2.ForeColor = System.Drawing.Color.White;
            this.radBandVHF3RX2.Name = "radBandVHF3RX2";
            this.radBandVHF3RX2.TabStop = true;
            this.radBandVHF3RX2.UseVisualStyleBackColor = true;
            this.radBandVHF3RX2.Click += new System.EventHandler(this.radBandVHF3RX2_Click);
            // 
            // radBandVHF2RX2
            // 
            resources.ApplyResources(this.radBandVHF2RX2, "radBandVHF2RX2");
            this.radBandVHF2RX2.FlatAppearance.BorderSize = 0;
            this.radBandVHF2RX2.ForeColor = System.Drawing.Color.White;
            this.radBandVHF2RX2.Name = "radBandVHF2RX2";
            this.radBandVHF2RX2.TabStop = true;
            this.radBandVHF2RX2.UseVisualStyleBackColor = true;
            this.radBandVHF2RX2.Click += new System.EventHandler(this.radBandVHF2RX2_Click);
            // 
            // radBandVHF1RX2
            // 
            resources.ApplyResources(this.radBandVHF1RX2, "radBandVHF1RX2");
            this.radBandVHF1RX2.FlatAppearance.BorderSize = 0;
            this.radBandVHF1RX2.ForeColor = System.Drawing.Color.White;
            this.radBandVHF1RX2.Name = "radBandVHF1RX2";
            this.radBandVHF1RX2.TabStop = true;
            this.radBandVHF1RX2.UseVisualStyleBackColor = true;
            this.radBandVHF1RX2.Click += new System.EventHandler(this.radBandVHF1RX2_Click);
            // 
            // radBandVHF0RX2
            // 
            resources.ApplyResources(this.radBandVHF0RX2, "radBandVHF0RX2");
            this.radBandVHF0RX2.FlatAppearance.BorderSize = 0;
            this.radBandVHF0RX2.ForeColor = System.Drawing.Color.White;
            this.radBandVHF0RX2.Name = "radBandVHF0RX2";
            this.radBandVHF0RX2.TabStop = true;
            this.radBandVHF0RX2.UseVisualStyleBackColor = true;
            this.radBandVHF0RX2.Click += new System.EventHandler(this.radBandVHFRX2_Click);
            // 
            // btnBandHFRX2
            // 
            this.btnBandHFRX2.FlatAppearance.BorderSize = 0;
            resources.ApplyResources(this.btnBandHFRX2, "btnBandHFRX2");
            this.btnBandHFRX2.ForeColor = System.Drawing.Color.Yellow;
            this.btnBandHFRX2.Name = "btnBandHFRX2";
            this.btnBandHFRX2.Click += new System.EventHandler(this.btnBandHFRX2_Click);
            // 
            // panelBandHFRX2
            // 
            resources.ApplyResources(this.panelBandHFRX2, "panelBandHFRX2");
            this.panelBandHFRX2.BackColor = System.Drawing.Color.Transparent;
            this.panelBandHFRX2.Controls.Add(this.btnBandVHFRX2);
            this.panelBandHFRX2.Controls.Add(this.radBandGENRX2);
            this.panelBandHFRX2.Controls.Add(this.radBandWWVRX2);
            this.panelBandHFRX2.Controls.Add(this.radBand2RX2);
            this.panelBandHFRX2.Controls.Add(this.radBand6RX2);
            this.panelBandHFRX2.Controls.Add(this.radBand10RX2);
            this.panelBandHFRX2.Controls.Add(this.radBand12RX2);
            this.panelBandHFRX2.Controls.Add(this.radBand15RX2);
            this.panelBandHFRX2.Controls.Add(this.radBand17RX2);
            this.panelBandHFRX2.Controls.Add(this.radBand20RX2);
            this.panelBandHFRX2.Controls.Add(this.radBand30RX2);
            this.panelBandHFRX2.Controls.Add(this.radBand40RX2);
            this.panelBandHFRX2.Controls.Add(this.radBand60RX2);
            this.panelBandHFRX2.Controls.Add(this.radBand160RX2);
            this.panelBandHFRX2.Controls.Add(this.radBand80RX2);
            this.panelBandHFRX2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.panelBandHFRX2.Name = "panelBandHFRX2";
            this.panelBandHFRX2.VisibleChanged += new System.EventHandler(this.panelBandHFRX2_VisibleChanged);
            this.panelBandHFRX2.Paint += new System.Windows.Forms.PaintEventHandler(this.panelRX2Ring_Paint);
            // 
            // panelBandVHF
            // 
            resources.ApplyResources(this.panelBandVHF, "panelBandVHF");
            this.panelBandVHF.BackColor = System.Drawing.Color.Transparent;
            this.panelBandVHF.Controls.Add(this.radBandVHF13);
            this.panelBandVHF.Controls.Add(this.radBandVHF12);
            this.panelBandVHF.Controls.Add(this.radBandVHF11);
            this.panelBandVHF.Controls.Add(this.radBandVHF10);
            this.panelBandVHF.Controls.Add(this.radBandVHF9);
            this.panelBandVHF.Controls.Add(this.radBandVHF8);
            this.panelBandVHF.Controls.Add(this.radBandVHF7);
            this.panelBandVHF.Controls.Add(this.radBandVHF6);
            this.panelBandVHF.Controls.Add(this.radBandVHF5);
            this.panelBandVHF.Controls.Add(this.radBandVHF4);
            this.panelBandVHF.Controls.Add(this.radBandVHF3);
            this.panelBandVHF.Controls.Add(this.radBandVHF2);
            this.panelBandVHF.Controls.Add(this.radBandVHF1);
            this.panelBandVHF.Controls.Add(this.radBandVHF0);
            this.panelBandVHF.Controls.Add(this.btnBandHF);
            this.panelBandVHF.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.panelBandVHF.Name = "panelBandVHF";
            this.panelBandVHF.VisibleChanged += new System.EventHandler(this.panelBandVHF_VisibleChanged);
            this.panelBandVHF.Paint += new System.Windows.Forms.PaintEventHandler(this.panelRX1Ring_Paint);
            // 
            // radBandVHF13
            // 
            resources.ApplyResources(this.radBandVHF13, "radBandVHF13");
            this.radBandVHF13.FlatAppearance.BorderSize = 0;
            this.radBandVHF13.ForeColor = System.Drawing.Color.White;
            this.radBandVHF13.Name = "radBandVHF13";
            this.radBandVHF13.TabStop = true;
            this.radBandVHF13.UseVisualStyleBackColor = true;
            this.radBandVHF13.Click += new System.EventHandler(this.radBandVHF_Click);
            this.radBandVHF13.MouseDown += new System.Windows.Forms.MouseEventHandler(this.radBandVHF13_MouseDown);
            // 
            // radBandVHF12
            // 
            resources.ApplyResources(this.radBandVHF12, "radBandVHF12");
            this.radBandVHF12.FlatAppearance.BorderSize = 0;
            this.radBandVHF12.ForeColor = System.Drawing.Color.White;
            this.radBandVHF12.Name = "radBandVHF12";
            this.radBandVHF12.TabStop = true;
            this.radBandVHF12.UseVisualStyleBackColor = true;
            this.radBandVHF12.Click += new System.EventHandler(this.radBandVHF_Click);
            this.radBandVHF12.MouseDown += new System.Windows.Forms.MouseEventHandler(this.radBandVHF12_MouseDown);
            // 
            // radBandVHF11
            // 
            resources.ApplyResources(this.radBandVHF11, "radBandVHF11");
            this.radBandVHF11.FlatAppearance.BorderSize = 0;
            this.radBandVHF11.ForeColor = System.Drawing.Color.White;
            this.radBandVHF11.Name = "radBandVHF11";
            this.radBandVHF11.TabStop = true;
            this.radBandVHF11.UseVisualStyleBackColor = true;
            this.radBandVHF11.Click += new System.EventHandler(this.radBandVHF_Click);
            this.radBandVHF11.MouseDown += new System.Windows.Forms.MouseEventHandler(this.radBandVHF11_MouseDown);
            // 
            // radBandVHF10
            // 
            resources.ApplyResources(this.radBandVHF10, "radBandVHF10");
            this.radBandVHF10.FlatAppearance.BorderSize = 0;
            this.radBandVHF10.ForeColor = System.Drawing.Color.White;
            this.radBandVHF10.Name = "radBandVHF10";
            this.radBandVHF10.TabStop = true;
            this.radBandVHF10.UseVisualStyleBackColor = true;
            this.radBandVHF10.Click += new System.EventHandler(this.radBandVHF_Click);
            this.radBandVHF10.MouseDown += new System.Windows.Forms.MouseEventHandler(this.radBandVHF10_MouseDown);
            // 
            // radBandVHF9
            // 
            resources.ApplyResources(this.radBandVHF9, "radBandVHF9");
            this.radBandVHF9.FlatAppearance.BorderSize = 0;
            this.radBandVHF9.ForeColor = System.Drawing.Color.White;
            this.radBandVHF9.Name = "radBandVHF9";
            this.radBandVHF9.TabStop = true;
            this.radBandVHF9.UseVisualStyleBackColor = true;
            this.radBandVHF9.Click += new System.EventHandler(this.radBandVHF_Click);
            this.radBandVHF9.MouseDown += new System.Windows.Forms.MouseEventHandler(this.radBandVHF9_MouseDown);
            // 
            // radBandVHF8
            // 
            resources.ApplyResources(this.radBandVHF8, "radBandVHF8");
            this.radBandVHF8.FlatAppearance.BorderSize = 0;
            this.radBandVHF8.ForeColor = System.Drawing.Color.White;
            this.radBandVHF8.Name = "radBandVHF8";
            this.radBandVHF8.TabStop = true;
            this.radBandVHF8.UseVisualStyleBackColor = true;
            this.radBandVHF8.Click += new System.EventHandler(this.radBandVHF_Click);
            this.radBandVHF8.MouseDown += new System.Windows.Forms.MouseEventHandler(this.radBandVHF8_MouseDown);
            // 
            // radBandVHF7
            // 
            resources.ApplyResources(this.radBandVHF7, "radBandVHF7");
            this.radBandVHF7.FlatAppearance.BorderSize = 0;
            this.radBandVHF7.ForeColor = System.Drawing.Color.White;
            this.radBandVHF7.Name = "radBandVHF7";
            this.radBandVHF7.TabStop = true;
            this.radBandVHF7.UseVisualStyleBackColor = true;
            this.radBandVHF7.Click += new System.EventHandler(this.radBandVHF_Click);
            this.radBandVHF7.MouseDown += new System.Windows.Forms.MouseEventHandler(this.radBandVHF7_MouseDown);
            // 
            // radBandVHF6
            // 
            resources.ApplyResources(this.radBandVHF6, "radBandVHF6");
            this.radBandVHF6.FlatAppearance.BorderSize = 0;
            this.radBandVHF6.ForeColor = System.Drawing.Color.White;
            this.radBandVHF6.Name = "radBandVHF6";
            this.radBandVHF6.TabStop = true;
            this.radBandVHF6.UseVisualStyleBackColor = true;
            this.radBandVHF6.Click += new System.EventHandler(this.radBandVHF_Click);
            this.radBandVHF6.MouseDown += new System.Windows.Forms.MouseEventHandler(this.radBandVHF6_MouseDown);
            // 
            // radBandVHF5
            // 
            resources.ApplyResources(this.radBandVHF5, "radBandVHF5");
            this.radBandVHF5.FlatAppearance.BorderSize = 0;
            this.radBandVHF5.ForeColor = System.Drawing.Color.White;
            this.radBandVHF5.Name = "radBandVHF5";
            this.radBandVHF5.TabStop = true;
            this.radBandVHF5.UseVisualStyleBackColor = true;
            this.radBandVHF5.Click += new System.EventHandler(this.radBandVHF_Click);
            this.radBandVHF5.MouseDown += new System.Windows.Forms.MouseEventHandler(this.radBandVHF5_MouseDown);
            // 
            // radBandVHF4
            // 
            resources.ApplyResources(this.radBandVHF4, "radBandVHF4");
            this.radBandVHF4.FlatAppearance.BorderSize = 0;
            this.radBandVHF4.ForeColor = System.Drawing.Color.White;
            this.radBandVHF4.Name = "radBandVHF4";
            this.radBandVHF4.TabStop = true;
            this.radBandVHF4.UseVisualStyleBackColor = true;
            this.radBandVHF4.Click += new System.EventHandler(this.radBandVHF_Click);
            this.radBandVHF4.MouseDown += new System.Windows.Forms.MouseEventHandler(this.radBandVHF4_MouseDown);
            // 
            // radBandVHF3
            // 
            resources.ApplyResources(this.radBandVHF3, "radBandVHF3");
            this.radBandVHF3.FlatAppearance.BorderSize = 0;
            this.radBandVHF3.ForeColor = System.Drawing.Color.White;
            this.radBandVHF3.Name = "radBandVHF3";
            this.radBandVHF3.TabStop = true;
            this.radBandVHF3.UseVisualStyleBackColor = true;
            this.radBandVHF3.CheckedChanged += new System.EventHandler(this.radBandVHF3_CheckedChanged);
            this.radBandVHF3.Click += new System.EventHandler(this.radBandVHF_Click);
            this.radBandVHF3.MouseDown += new System.Windows.Forms.MouseEventHandler(this.radBandVHF3_MouseDown);
            // 
            // radBandVHF2
            // 
            resources.ApplyResources(this.radBandVHF2, "radBandVHF2");
            this.radBandVHF2.FlatAppearance.BorderSize = 0;
            this.radBandVHF2.ForeColor = System.Drawing.Color.White;
            this.radBandVHF2.Name = "radBandVHF2";
            this.radBandVHF2.TabStop = true;
            this.radBandVHF2.UseVisualStyleBackColor = true;
            this.radBandVHF2.Click += new System.EventHandler(this.radBandVHF_Click);
            this.radBandVHF2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.radBandVHF2_MouseDown);
            // 
            // radBandVHF1
            // 
            resources.ApplyResources(this.radBandVHF1, "radBandVHF1");
            this.radBandVHF1.FlatAppearance.BorderSize = 0;
            this.radBandVHF1.ForeColor = System.Drawing.Color.White;
            this.radBandVHF1.Name = "radBandVHF1";
            this.radBandVHF1.TabStop = true;
            this.radBandVHF1.UseVisualStyleBackColor = true;
            this.radBandVHF1.Click += new System.EventHandler(this.radBandVHF_Click);
            this.radBandVHF1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.radBandVHF1_MouseDown);
            // 
            // radBandVHF0
            // 
            resources.ApplyResources(this.radBandVHF0, "radBandVHF0");
            this.radBandVHF0.FlatAppearance.BorderSize = 0;
            this.radBandVHF0.ForeColor = System.Drawing.Color.White;
            this.radBandVHF0.Name = "radBandVHF0";
            this.radBandVHF0.TabStop = true;
            this.radBandVHF0.UseVisualStyleBackColor = true;
            this.radBandVHF0.Click += new System.EventHandler(this.radBandVHF_Click);
            this.radBandVHF0.MouseDown += new System.Windows.Forms.MouseEventHandler(this.radBandVHF0_MouseDown);
            // 
            // btnBandHF
            // 
            this.btnBandHF.FlatAppearance.BorderSize = 0;
            resources.ApplyResources(this.btnBandHF, "btnBandHF");
            this.btnBandHF.ForeColor = System.Drawing.Color.Yellow;
            this.btnBandHF.Name = "btnBandHF";
            this.btnBandHF.Click += new System.EventHandler(this.btnBandHF_Click);
            // 
            // grpRX2Meter
            // 
            resources.ApplyResources(this.grpRX2Meter, "grpRX2Meter");
            this.grpRX2Meter.BackColor = System.Drawing.Color.Transparent;
            this.grpRX2Meter.Controls.Add(this.lblRX2Meter);
            this.grpRX2Meter.Controls.Add(this.picRX3Meter);
            this.grpRX2Meter.Controls.Add(this.picRX2Meter);
            this.grpRX2Meter.Controls.Add(this.comboMeterTX1Mode);
            this.grpRX2Meter.Controls.Add(this.comboRX2MeterMode);
            this.grpRX2Meter.Controls.Add(this.txtRX2Meter);
            this.grpRX2Meter.ForeColor = System.Drawing.Color.White;
            this.grpRX2Meter.Name = "grpRX2Meter";
            this.grpRX2Meter.Paint += new System.Windows.Forms.PaintEventHandler(this.grpRX2Meter_Paint);
            this.grpRX2Meter.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picRX2Meter_MouseUp);
            // 
            // lblRX2Meter
            // 
            resources.ApplyResources(this.lblRX2Meter, "lblRX2Meter");
            this.lblRX2Meter.Name = "lblRX2Meter";
            // 
            // txtRX2Meter
            // 
            this.txtRX2Meter.BackColor = System.Drawing.Color.Black;
            this.txtRX2Meter.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.txtRX2Meter, "txtRX2Meter");
            this.txtRX2Meter.ForeColor = System.Drawing.Color.Yellow;
            this.txtRX2Meter.Name = "txtRX2Meter";
            this.txtRX2Meter.ReadOnly = true;
            this.txtRX2Meter.ShortcutsEnabled = false;
            this.txtRX2Meter.MouseUp += new System.Windows.Forms.MouseEventHandler(this.txtRX2Meter_MouseUp);
            // 
            // panelBandGNRX2
            // 
            resources.ApplyResources(this.panelBandGNRX2, "panelBandGNRX2");
            this.panelBandGNRX2.BackColor = System.Drawing.Color.Transparent;
            this.panelBandGNRX2.Controls.Add(this.radBandGN13RX2);
            this.panelBandGNRX2.Controls.Add(this.radBandGN12RX2);
            this.panelBandGNRX2.Controls.Add(this.radBandGN11RX2);
            this.panelBandGNRX2.Controls.Add(this.radBandGN10RX2);
            this.panelBandGNRX2.Controls.Add(this.radBandGN9RX2);
            this.panelBandGNRX2.Controls.Add(this.radBandGN8RX2);
            this.panelBandGNRX2.Controls.Add(this.radBandGN7RX2);
            this.panelBandGNRX2.Controls.Add(this.radBandGN6RX2);
            this.panelBandGNRX2.Controls.Add(this.radBandGN5RX2);
            this.panelBandGNRX2.Controls.Add(this.radBandGN4RX2);
            this.panelBandGNRX2.Controls.Add(this.radBandGN3RX2);
            this.panelBandGNRX2.Controls.Add(this.radBandGN2RX2);
            this.panelBandGNRX2.Controls.Add(this.radBandGN1RX2);
            this.panelBandGNRX2.Controls.Add(this.radBandGN0RX2);
            this.panelBandGNRX2.Controls.Add(this.btnBandHF1RX2);
            this.panelBandGNRX2.Name = "panelBandGNRX2";
            this.panelBandGNRX2.VisibleChanged += new System.EventHandler(this.panelBandGNRX2_VisibleChanged);
            this.panelBandGNRX2.Paint += new System.Windows.Forms.PaintEventHandler(this.panelRX2Ring_Paint);
            // 
            // btnBandHF1RX2
            // 
            this.btnBandHF1RX2.FlatAppearance.BorderSize = 0;
            resources.ApplyResources(this.btnBandHF1RX2, "btnBandHF1RX2");
            this.btnBandHF1RX2.ForeColor = System.Drawing.Color.OrangeRed;
            this.btnBandHF1RX2.Name = "btnBandHF1RX2";
            this.btnBandHF1RX2.Click += new System.EventHandler(this.btnBandHFRX2_Click);
            // 
            // grpMultimeter
            // 
            resources.ApplyResources(this.grpMultimeter, "grpMultimeter");
            this.grpMultimeter.BackColor = System.Drawing.Color.Transparent;
            this.grpMultimeter.Controls.Add(this.lblMultiSMeter);
            this.grpMultimeter.Controls.Add(this.comboMeterTXMode);
            this.grpMultimeter.Controls.Add(this.picMultiMeterDigital);
            this.grpMultimeter.Controls.Add(this.comboMeterRXMode);
            this.grpMultimeter.Controls.Add(this.txtMultiText);
            this.grpMultimeter.ForeColor = System.Drawing.Color.White;
            this.grpMultimeter.Name = "grpMultimeter";
            this.grpMultimeter.Paint += new System.Windows.Forms.PaintEventHandler(this.grpMultimeter_Paint);
            // 
            // lblMultiSMeter
            // 
            resources.ApplyResources(this.lblMultiSMeter, "lblMultiSMeter");
            this.lblMultiSMeter.Name = "lblMultiSMeter";
            // 
            // panelBandHF
            // 
            resources.ApplyResources(this.panelBandHF, "panelBandHF");
            this.panelBandHF.BackColor = System.Drawing.Color.Transparent;
            this.panelBandHF.Controls.Add(this.radBandGEN);
            this.panelBandHF.Controls.Add(this.radBandWWV);
            this.panelBandHF.Controls.Add(this.radBand2);
            this.panelBandHF.Controls.Add(this.radBand6);
            this.panelBandHF.Controls.Add(this.radBand10);
            this.panelBandHF.Controls.Add(this.radBand12);
            this.panelBandHF.Controls.Add(this.radBand15);
            this.panelBandHF.Controls.Add(this.radBand17);
            this.panelBandHF.Controls.Add(this.radBand20);
            this.panelBandHF.Controls.Add(this.radBand30);
            this.panelBandHF.Controls.Add(this.radBand40);
            this.panelBandHF.Controls.Add(this.radBand60);
            this.panelBandHF.Controls.Add(this.radBand160);
            this.panelBandHF.Controls.Add(this.radBand80);
            this.panelBandHF.Controls.Add(this.btnBandVHF);
            this.panelBandHF.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.panelBandHF.Name = "panelBandHF";
            this.panelBandHF.VisibleChanged += new System.EventHandler(this.panelBandHF_VisibleChanged);
            this.panelBandHF.Paint += new System.Windows.Forms.PaintEventHandler(this.panelRX1Ring_Paint);
            // 
            // panelModeSpecificFM
            // 
            resources.ApplyResources(this.panelModeSpecificFM, "panelModeSpecificFM");
            this.panelModeSpecificFM.BackColor = System.Drawing.Color.Transparent;
            this.panelModeSpecificFM.Controls.Add(this.chkTXEQ1);
            this.panelModeSpecificFM.Controls.Add(this.chkRXEQ1);
            this.panelModeSpecificFM.Controls.Add(this.udFM1750Timer);
            this.panelModeSpecificFM.Controls.Add(this.chkFM1750);
            this.panelModeSpecificFM.Controls.Add(this.chkFMTXLow);
            this.panelModeSpecificFM.Controls.Add(this.btnFMMemory);
            this.panelModeSpecificFM.Controls.Add(this.lblFMMemory);
            this.panelModeSpecificFM.Controls.Add(this.btnFMMemoryUp);
            this.panelModeSpecificFM.Controls.Add(this.btnFMMemoryDown);
            this.panelModeSpecificFM.Controls.Add(this.comboFMMemory);
            this.panelModeSpecificFM.Controls.Add(this.radFMDeviation2kHz);
            this.panelModeSpecificFM.Controls.Add(this.lblFMOffset);
            this.panelModeSpecificFM.Controls.Add(this.udFMOffset);
            this.panelModeSpecificFM.Controls.Add(this.chkFMTXRev);
            this.panelModeSpecificFM.Controls.Add(this.lblFMDeviation);
            this.panelModeSpecificFM.Controls.Add(this.radFMDeviation5kHz);
            this.panelModeSpecificFM.Controls.Add(this.comboFMCTCSS);
            this.panelModeSpecificFM.Controls.Add(this.chkFMCTCSS);
            this.panelModeSpecificFM.Controls.Add(this.chkFMTXSimplex);
            this.panelModeSpecificFM.Controls.Add(this.chkFMTXHigh);
            this.panelModeSpecificFM.Controls.Add(this.ptbFMMic);
            this.panelModeSpecificFM.Controls.Add(this.lblMicValFM);
            this.panelModeSpecificFM.Controls.Add(this.lblFMMic);
            this.panelModeSpecificFM.Controls.Add(this.labelTS7);
            this.panelModeSpecificFM.Controls.Add(this.comboFMTXProfile);
            this.panelModeSpecificFM.Name = "panelModeSpecificFM";
            this.panelModeSpecificFM.Paint += new System.Windows.Forms.PaintEventHandler(this.panelRing_Paint);
            // 
            // lblFMMemory
            // 
            this.lblFMMemory.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.lblFMMemory, "lblFMMemory");
            this.lblFMMemory.Name = "lblFMMemory";
            // 
            // comboFMMemory
            // 
            this.comboFMMemory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.comboFMMemory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboFMMemory.DropDownWidth = 96;
            this.comboFMMemory.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            resources.ApplyResources(this.comboFMMemory, "comboFMMemory");
            this.comboFMMemory.Name = "comboFMMemory";
            this.comboFMMemory.SelectedIndexChanged += new System.EventHandler(this.comboFMMemory_SelectedIndexChanged);
            // 
            // lblFMOffset
            // 
            resources.ApplyResources(this.lblFMOffset, "lblFMOffset");
            this.lblFMOffset.ForeColor = System.Drawing.Color.White;
            this.lblFMOffset.Name = "lblFMOffset";
            // 
            // lblFMDeviation
            // 
            resources.ApplyResources(this.lblFMDeviation, "lblFMDeviation");
            this.lblFMDeviation.ForeColor = System.Drawing.Color.White;
            this.lblFMDeviation.Name = "lblFMDeviation";
            // 
            // ptbFMMic
            // 
            resources.ApplyResources(this.ptbFMMic, "ptbFMMic");
            this.ptbFMMic.HeadImage = null;
            this.ptbFMMic.LargeChange = 1;
            this.ptbFMMic.Maximum = 70;
            this.ptbFMMic.Minimum = 0;
            this.ptbFMMic.Name = "ptbFMMic";
            this.ptbFMMic.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.ptbFMMic.SmallChange = 1;
            this.ptbFMMic.TabStop = false;
            this.ptbFMMic.Value = 10;
            this.ptbFMMic.Scroll += new PowerSDR.PrettyTrackBar.ScrollHandler(this.ptbFMMic_Scroll);
            // 
            // lblMicValFM
            // 
            resources.ApplyResources(this.lblMicValFM, "lblMicValFM");
            this.lblMicValFM.ForeColor = System.Drawing.Color.White;
            this.lblMicValFM.Name = "lblMicValFM";
            // 
            // lblFMMic
            // 
            resources.ApplyResources(this.lblFMMic, "lblFMMic");
            this.lblFMMic.ForeColor = System.Drawing.Color.White;
            this.lblFMMic.Name = "lblFMMic";
            // 
            // labelTS7
            // 
            this.labelTS7.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.labelTS7, "labelTS7");
            this.labelTS7.Name = "labelTS7";
            // 
            // panelVFO
            // 
            resources.ApplyResources(this.panelVFO, "panelVFO");
            this.panelVFO.BackColor = System.Drawing.Color.Transparent;
            this.panelVFO.Controls.Add(this.chkVAC2);
            this.panelVFO.Controls.Add(this.btnZeroBeat);
            this.panelVFO.Controls.Add(this.chkVFOSplit);
            this.panelVFO.Controls.Add(this.btnRITReset);
            this.panelVFO.Controls.Add(this.btnXITReset);
            this.panelVFO.Controls.Add(this.udRIT);
            this.panelVFO.Controls.Add(this.btnIFtoVFO);
            this.panelVFO.Controls.Add(this.chkRIT);
            this.panelVFO.Controls.Add(this.btnVFOSwap);
            this.panelVFO.Controls.Add(this.chkXIT);
            this.panelVFO.Controls.Add(this.btnVFOBtoA);
            this.panelVFO.Controls.Add(this.udXIT);
            this.panelVFO.Controls.Add(this.btnVFOAtoB);
            this.panelVFO.Controls.Add(this.chkVAC1);
            this.panelVFO.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.panelVFO.Name = "panelVFO";
            // 
            // panelTS1
            // 
            resources.ApplyResources(this.panelTS1, "panelTS1");
            this.panelTS1.BackColor = System.Drawing.Color.Transparent;
            this.panelTS1.Controls.Add(this.richTextBox1);
            this.panelTS1.Controls.Add(this.richTextBox2);
            this.panelTS1.Controls.Add(this.richTextBox3);
            this.panelTS1.Controls.Add(this.richTextBox5);
            this.panelTS1.Controls.Add(this.richTextBox6);
            this.panelTS1.Controls.Add(this.richTextBox7);
            this.panelTS1.Controls.Add(this.richTextBox8);
            this.panelTS1.Name = "panelTS1";
            this.panelTS1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panelTS1_MouseClick);
            // 
            // grpVFOBetween
            // 
            resources.ApplyResources(this.grpVFOBetween, "grpVFOBetween");
            this.grpVFOBetween.BackColor = System.Drawing.Color.Transparent;
            this.grpVFOBetween.Controls.Add(this.buttonbs);
            this.grpVFOBetween.Controls.Add(this.chkBoxBS);
            this.grpVFOBetween.Controls.Add(this.labelTS5);
            this.grpVFOBetween.Controls.Add(this.regBox1);
            this.grpVFOBetween.Controls.Add(this.regBox);
            this.grpVFOBetween.Controls.Add(this.lblTuneStep);
            this.grpVFOBetween.Controls.Add(this.chkVFOSync);
            this.grpVFOBetween.Controls.Add(this.chkFullDuplex);
            this.grpVFOBetween.Controls.Add(this.btnTuneStepChangeLarger);
            this.grpVFOBetween.Controls.Add(this.btnTuneStepChangeSmaller);
            this.grpVFOBetween.Controls.Add(this.chkVFOLock);
            this.grpVFOBetween.Controls.Add(this.txtWheelTune);
            this.grpVFOBetween.Controls.Add(this.btnMemoryQuickRestore);
            this.grpVFOBetween.Controls.Add(this.btnMemoryQuickSave);
            this.grpVFOBetween.Controls.Add(this.txtMemoryQuick);
            this.grpVFOBetween.Name = "grpVFOBetween";
            this.grpVFOBetween.Paint += new System.Windows.Forms.PaintEventHandler(this.grpVFOBetween_Paint);
            // 
            // grpVFOB
            // 
            resources.ApplyResources(this.grpVFOB, "grpVFOB");
            this.grpVFOB.BackColor = System.Drawing.Color.Transparent;
            this.grpVFOB.Controls.Add(this.chkVFOBTX);
            this.grpVFOB.Controls.Add(this.panelVFOBHover);
            this.grpVFOB.Controls.Add(this.txtVFOBLSD);
            this.grpVFOB.Controls.Add(this.txtVFOBMSD);
            this.grpVFOB.Controls.Add(this.txtVFOBBand);
            this.grpVFOB.Controls.Add(this.txtVFOBFreq);
            this.grpVFOB.ForeColor = System.Drawing.Color.White;
            this.grpVFOB.Name = "grpVFOB";
            this.grpVFOB.Paint += new System.Windows.Forms.PaintEventHandler(this.grpVFOB_Paint);
            this.grpVFOB.MouseHover += new System.EventHandler(this.grpVFOB_MouseHover);
            // 
            // panelVFOBHover
            // 
            resources.ApplyResources(this.panelVFOBHover, "panelVFOBHover");
            this.panelVFOBHover.BackColor = System.Drawing.Color.Black;
            this.panelVFOBHover.Name = "panelVFOBHover";
            this.panelVFOBHover.Paint += new System.Windows.Forms.PaintEventHandler(this.panelVFOBHover_Paint);
            this.panelVFOBHover.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelVFOBHover_MouseMove);
            // 
            // txtVFOBLSD
            // 
            resources.ApplyResources(this.txtVFOBLSD, "txtVFOBLSD");
            this.txtVFOBLSD.BackColor = System.Drawing.Color.Black;
            this.txtVFOBLSD.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtVFOBLSD.ForeColor = System.Drawing.Color.Olive;
            this.txtVFOBLSD.Name = "txtVFOBLSD";
            this.txtVFOBLSD.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txtVFOBLSD_MouseDown);
            this.txtVFOBLSD.MouseMove += new System.Windows.Forms.MouseEventHandler(this.txtVFOBLSD_MouseMove);
            // 
            // txtVFOBMSD
            // 
            resources.ApplyResources(this.txtVFOBMSD, "txtVFOBMSD");
            this.txtVFOBMSD.BackColor = System.Drawing.Color.Black;
            this.txtVFOBMSD.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtVFOBMSD.ForeColor = System.Drawing.Color.Olive;
            this.txtVFOBMSD.Name = "txtVFOBMSD";
            this.txtVFOBMSD.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txtVFOBMSD_MouseDown);
            this.txtVFOBMSD.MouseLeave += new System.EventHandler(this.txtVFOBMSD_MouseLeave);
            this.txtVFOBMSD.MouseMove += new System.Windows.Forms.MouseEventHandler(this.txtVFOBMSD_MouseMove);
            // 
            // txtVFOBBand
            // 
            this.txtVFOBBand.BackColor = System.Drawing.Color.Black;
            this.txtVFOBBand.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.txtVFOBBand, "txtVFOBBand");
            this.txtVFOBBand.ForeColor = System.Drawing.Color.Green;
            this.txtVFOBBand.Name = "txtVFOBBand";
            this.txtVFOBBand.ReadOnly = true;
            this.txtVFOBBand.ShortcutsEnabled = false;
            this.txtVFOBBand.GotFocus += new System.EventHandler(this.HideFocus);
            this.txtVFOBBand.MouseUp += new System.Windows.Forms.MouseEventHandler(this.txtVFOBBand_MouseUp);
            // 
            // txtVFOBFreq
            // 
            resources.ApplyResources(this.txtVFOBFreq, "txtVFOBFreq");
            this.txtVFOBFreq.BackColor = System.Drawing.Color.Black;
            this.txtVFOBFreq.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtVFOBFreq.ForeColor = System.Drawing.Color.Olive;
            this.txtVFOBFreq.Name = "txtVFOBFreq";
            this.txtVFOBFreq.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtVFOBFreq_KeyPress);
            this.txtVFOBFreq.LostFocus += new System.EventHandler(this.txtVFOBFreq_LostFocus);
            this.txtVFOBFreq.MouseLeave += new System.EventHandler(this.txtVFOBFreq_MouseLeave);
            this.txtVFOBFreq.MouseMove += new System.Windows.Forms.MouseEventHandler(this.txtVFOBFreq_MouseMove);
            // 
            // grpVFOA
            // 
            resources.ApplyResources(this.grpVFOA, "grpVFOA");
            this.grpVFOA.BackColor = System.Drawing.Color.Transparent;
            this.grpVFOA.Controls.Add(this.chkVFOATX);
            this.grpVFOA.Controls.Add(this.panelVFOASubHover);
            this.grpVFOA.Controls.Add(this.panelVFOAHover);
            this.grpVFOA.Controls.Add(this.txtVFOALSD);
            this.grpVFOA.Controls.Add(this.txtVFOABand);
            this.grpVFOA.Controls.Add(this.txtVFOAMSD);
            this.grpVFOA.Controls.Add(this.txtVFOAFreq);
            this.grpVFOA.Controls.Add(this.btnHidden);
            this.grpVFOA.ForeColor = System.Drawing.Color.White;
            this.grpVFOA.Name = "grpVFOA";
            this.grpVFOA.Paint += new System.Windows.Forms.PaintEventHandler(this.grpVFOA_Paint);
            this.grpVFOA.MouseHover += new System.EventHandler(this.grpVFOA_MouseHover);
            // 
            // panelVFOASubHover
            // 
            resources.ApplyResources(this.panelVFOASubHover, "panelVFOASubHover");
            this.panelVFOASubHover.BackColor = System.Drawing.Color.Black;
            this.panelVFOASubHover.ForeColor = System.Drawing.Color.Black;
            this.panelVFOASubHover.Name = "panelVFOASubHover";
            this.panelVFOASubHover.Paint += new System.Windows.Forms.PaintEventHandler(this.panelVFOASubHover_Paint);
            this.panelVFOASubHover.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelVFOASubHover_MouseMove);
            // 
            // panelVFOAHover
            // 
            resources.ApplyResources(this.panelVFOAHover, "panelVFOAHover");
            this.panelVFOAHover.BackColor = System.Drawing.Color.Black;
            this.panelVFOAHover.Name = "panelVFOAHover";
            this.panelVFOAHover.Paint += new System.Windows.Forms.PaintEventHandler(this.panelVFOAHover_Paint);
            this.panelVFOAHover.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelVFOAHover_MouseMove);
            // 
            // txtVFOALSD
            // 
            resources.ApplyResources(this.txtVFOALSD, "txtVFOALSD");
            this.txtVFOALSD.BackColor = System.Drawing.Color.Black;
            this.txtVFOALSD.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtVFOALSD.ForeColor = System.Drawing.Color.Olive;
            this.txtVFOALSD.Name = "txtVFOALSD";
            this.txtVFOALSD.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txtVFOALSD_MouseDown);
            this.txtVFOALSD.MouseMove += new System.Windows.Forms.MouseEventHandler(this.txtVFOALSD_MouseMove);
            // 
            // txtVFOAMSD
            // 
            this.txtVFOAMSD.BackColor = System.Drawing.Color.Black;
            this.txtVFOAMSD.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.txtVFOAMSD, "txtVFOAMSD");
            this.txtVFOAMSD.ForeColor = System.Drawing.Color.Olive;
            this.txtVFOAMSD.Name = "txtVFOAMSD";
            this.txtVFOAMSD.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txtVFOAMSD_MouseDown);
            this.txtVFOAMSD.MouseLeave += new System.EventHandler(this.txtVFOAMSD_MouseLeave);
            this.txtVFOAMSD.MouseMove += new System.Windows.Forms.MouseEventHandler(this.txtVFOAMSD_MouseMove);
            // 
            // txtVFOAFreq
            // 
            this.txtVFOAFreq.BackColor = System.Drawing.Color.Black;
            this.txtVFOAFreq.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.txtVFOAFreq, "txtVFOAFreq");
            this.txtVFOAFreq.ForeColor = System.Drawing.Color.Olive;
            this.txtVFOAFreq.Name = "txtVFOAFreq";
            this.txtVFOAFreq.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtVFOAFreq_KeyPress);
            this.txtVFOAFreq.LostFocus += new System.EventHandler(this.txtVFOAFreq_LostFocus);
            this.txtVFOAFreq.MouseLeave += new System.EventHandler(this.txtVFOAFreq_MouseLeave);
            this.txtVFOAFreq.MouseMove += new System.Windows.Forms.MouseEventHandler(this.txtVFOAFreq_MouseMove);
            // 
            // btnHidden
            // 
            resources.ApplyResources(this.btnHidden, "btnHidden");
            this.btnHidden.Name = "btnHidden";
            // 
            // panelDisplay2
            // 
            resources.ApplyResources(this.panelDisplay2, "panelDisplay2");
            this.panelDisplay2.BackColor = System.Drawing.Color.Transparent;
            this.panelDisplay2.Controls.Add(this.pictureBox1);
            this.panelDisplay2.Controls.Add(this.btnTNFAdd);
            this.panelDisplay2.Controls.Add(this.chkTNF);
            this.panelDisplay2.Controls.Add(this.chkDisplayPeak);
            this.panelDisplay2.Controls.Add(this.comboDisplayMode);
            this.panelDisplay2.Controls.Add(this.chkDisplayAVG);
            this.panelDisplay2.Controls.Add(this.label6);
            this.panelDisplay2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.panelDisplay2.Name = "panelDisplay2";
            // 
            // ptbRX2Squelch
            // 
            this.ptbRX2Squelch.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.ptbRX2Squelch, "ptbRX2Squelch");
            this.ptbRX2Squelch.HeadImage = null;
            this.ptbRX2Squelch.LargeChange = 1;
            this.ptbRX2Squelch.Maximum = 0;
            this.ptbRX2Squelch.Minimum = -160;
            this.ptbRX2Squelch.Name = "ptbRX2Squelch";
            this.ptbRX2Squelch.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.ptbRX2Squelch.SmallChange = 1;
            this.ptbRX2Squelch.TabStop = false;
            this.ptbRX2Squelch.Value = -150;
            this.ptbRX2Squelch.Scroll += new PowerSDR.PrettyTrackBar.ScrollHandler(this.ptbRX2Squelch_Scroll);
            // 
            // panelOptions
            // 
            resources.ApplyResources(this.panelOptions, "panelOptions");
            this.panelOptions.BackColor = System.Drawing.Color.Transparent;
            this.panelOptions.Controls.Add(this.label1);
            this.panelOptions.Controls.Add(this.checkBoxID);
            this.panelOptions.Controls.Add(this.chkFWCATU);
            this.panelOptions.Controls.Add(this.chkFWCATUBypass);
            this.panelOptions.Controls.Add(this.ckQuickPlay);
            this.panelOptions.Controls.Add(this.chkMON);
            this.panelOptions.Controls.Add(this.ckQuickRec);
            this.panelOptions.Controls.Add(this.chkMUT);
            this.panelOptions.Controls.Add(this.chkMOX);
            this.panelOptions.Controls.Add(this.chkTUN);
            this.panelOptions.Controls.Add(this.chkX2TR);
            this.panelOptions.Controls.Add(this.comboTuneMode);
            this.panelOptions.Controls.Add(this.chkBoxMuteSpk);
            this.panelOptions.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.panelOptions.Name = "panelOptions";
            this.panelOptions.Paint += new System.Windows.Forms.PaintEventHandler(this.panelOptions_Paint);
            // 
            // panelBandGN
            // 
            resources.ApplyResources(this.panelBandGN, "panelBandGN");
            this.panelBandGN.BackColor = System.Drawing.Color.Transparent;
            this.panelBandGN.Controls.Add(this.radBandGN13);
            this.panelBandGN.Controls.Add(this.radBandGN12);
            this.panelBandGN.Controls.Add(this.radBandGN11);
            this.panelBandGN.Controls.Add(this.radBandGN10);
            this.panelBandGN.Controls.Add(this.radBandGN9);
            this.panelBandGN.Controls.Add(this.radBandGN8);
            this.panelBandGN.Controls.Add(this.radBandGN7);
            this.panelBandGN.Controls.Add(this.radBandGN6);
            this.panelBandGN.Controls.Add(this.radBandGN5);
            this.panelBandGN.Controls.Add(this.radBandGN4);
            this.panelBandGN.Controls.Add(this.radBandGN3);
            this.panelBandGN.Controls.Add(this.radBandGN2);
            this.panelBandGN.Controls.Add(this.radBandGN1);
            this.panelBandGN.Controls.Add(this.radBandGN0);
            this.panelBandGN.Controls.Add(this.btnBandHF1);
            this.panelBandGN.Name = "panelBandGN";
            this.panelBandGN.VisibleChanged += new System.EventHandler(this.panelBandGN_VisibleChanged);
            this.panelBandGN.Paint += new System.Windows.Forms.PaintEventHandler(this.panelRX1Ring_Paint);
            // 
            // btnBandHF1
            // 
            this.btnBandHF1.FlatAppearance.BorderSize = 0;
            resources.ApplyResources(this.btnBandHF1, "btnBandHF1");
            this.btnBandHF1.ForeColor = System.Drawing.Color.OrangeRed;
            this.btnBandHF1.Name = "btnBandHF1";
            this.btnBandHF1.Click += new System.EventHandler(this.btnBandHF_Click);
            // 
            // panelTSBandStack
            // 
            resources.ApplyResources(this.panelTSBandStack, "panelTSBandStack");
            this.panelTSBandStack.BackColor = System.Drawing.Color.Transparent;
            this.panelTSBandStack.Controls.Add(this.textBox1);
            this.panelTSBandStack.Controls.Add(this.textBox2);
            this.panelTSBandStack.Controls.Add(this.buttonAdd);
            this.panelTSBandStack.Controls.Add(this.buttonSort);
            this.panelTSBandStack.Controls.Add(this.buttonDel);
            this.panelTSBandStack.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.panelTSBandStack.Name = "panelTSBandStack";
            this.panelTSBandStack.Paint += new System.Windows.Forms.PaintEventHandler(this.panelTSBandStack_Paint);
            this.panelTSBandStack.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panelTSBandStack_MouseUp);
            // 
            // panelModeSpecificPhone
            // 
            resources.ApplyResources(this.panelModeSpecificPhone, "panelModeSpecificPhone");
            this.panelModeSpecificPhone.BackColor = System.Drawing.Color.Transparent;
            this.panelModeSpecificPhone.Controls.Add(this.labelTS2);
            this.panelModeSpecificPhone.Controls.Add(this.labelTS1);
            this.panelModeSpecificPhone.Controls.Add(this.udTXFilterLow);
            this.panelModeSpecificPhone.Controls.Add(this.udTXFilterHigh);
            this.panelModeSpecificPhone.Controls.Add(this.ptbMic);
            this.panelModeSpecificPhone.Controls.Add(this.picNoiseGate);
            this.panelModeSpecificPhone.Controls.Add(this.lblNoiseGateVal);
            this.panelModeSpecificPhone.Controls.Add(this.ptbNoiseGate);
            this.panelModeSpecificPhone.Controls.Add(this.picVOX);
            this.panelModeSpecificPhone.Controls.Add(this.ptbVOX);
            this.panelModeSpecificPhone.Controls.Add(this.lblVOXVal);
            this.panelModeSpecificPhone.Controls.Add(this.ptbCPDR);
            this.panelModeSpecificPhone.Controls.Add(this.lblCPDRVal);
            this.panelModeSpecificPhone.Controls.Add(this.ptbDX);
            this.panelModeSpecificPhone.Controls.Add(this.lblDXVal);
            this.panelModeSpecificPhone.Controls.Add(this.lblMicVal);
            this.panelModeSpecificPhone.Controls.Add(this.lblMIC);
            this.panelModeSpecificPhone.Controls.Add(this.chkShowTXFilter);
            this.panelModeSpecificPhone.Controls.Add(this.chkDX);
            this.panelModeSpecificPhone.Controls.Add(this.lblTransmitProfile);
            this.panelModeSpecificPhone.Controls.Add(this.chkTXEQ);
            this.panelModeSpecificPhone.Controls.Add(this.comboTXProfile);
            this.panelModeSpecificPhone.Controls.Add(this.chkRXEQ);
            this.panelModeSpecificPhone.Controls.Add(this.chkCPDR);
            this.panelModeSpecificPhone.Controls.Add(this.chkVOX);
            this.panelModeSpecificPhone.Controls.Add(this.chkNoiseGate);
            this.panelModeSpecificPhone.Name = "panelModeSpecificPhone";
            this.panelModeSpecificPhone.Paint += new System.Windows.Forms.PaintEventHandler(this.panelRing_Paint);
            // 
            // labelTS2
            // 
            this.labelTS2.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.labelTS2, "labelTS2");
            this.labelTS2.Name = "labelTS2";
            // 
            // labelTS1
            // 
            this.labelTS1.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.labelTS1, "labelTS1");
            this.labelTS1.Name = "labelTS1";
            // 
            // picNoiseGate
            // 
            this.picNoiseGate.BackColor = System.Drawing.SystemColors.ControlText;
            resources.ApplyResources(this.picNoiseGate, "picNoiseGate");
            this.picNoiseGate.Name = "picNoiseGate";
            this.picNoiseGate.TabStop = false;
            this.picNoiseGate.Paint += new System.Windows.Forms.PaintEventHandler(this.picNoiseGate_Paint);
            // 
            // lblNoiseGateVal
            // 
            resources.ApplyResources(this.lblNoiseGateVal, "lblNoiseGateVal");
            this.lblNoiseGateVal.ForeColor = System.Drawing.Color.White;
            this.lblNoiseGateVal.Name = "lblNoiseGateVal";
            // 
            // picVOX
            // 
            this.picVOX.BackColor = System.Drawing.SystemColors.ControlText;
            resources.ApplyResources(this.picVOX, "picVOX");
            this.picVOX.Name = "picVOX";
            this.picVOX.TabStop = false;
            this.picVOX.Paint += new System.Windows.Forms.PaintEventHandler(this.picVOX_Paint);
            // 
            // lblVOXVal
            // 
            resources.ApplyResources(this.lblVOXVal, "lblVOXVal");
            this.lblVOXVal.ForeColor = System.Drawing.Color.White;
            this.lblVOXVal.Name = "lblVOXVal";
            // 
            // lblCPDRVal
            // 
            resources.ApplyResources(this.lblCPDRVal, "lblCPDRVal");
            this.lblCPDRVal.ForeColor = System.Drawing.Color.White;
            this.lblCPDRVal.Name = "lblCPDRVal";
            // 
            // lblDXVal
            // 
            resources.ApplyResources(this.lblDXVal, "lblDXVal");
            this.lblDXVal.ForeColor = System.Drawing.Color.White;
            this.lblDXVal.Name = "lblDXVal";
            // 
            // lblMicVal
            // 
            resources.ApplyResources(this.lblMicVal, "lblMicVal");
            this.lblMicVal.ForeColor = System.Drawing.Color.White;
            this.lblMicVal.Name = "lblMicVal";
            // 
            // lblTransmitProfile
            // 
            this.lblTransmitProfile.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.lblTransmitProfile, "lblTransmitProfile");
            this.lblTransmitProfile.Name = "lblTransmitProfile";
            // 
            // panelModeSpecificCW
            // 
            resources.ApplyResources(this.panelModeSpecificCW, "panelModeSpecificCW");
            this.panelModeSpecificCW.BackColor = System.Drawing.Color.Transparent;
            this.panelModeSpecificCW.Controls.Add(this.labelTS6);
            this.panelModeSpecificCW.Controls.Add(this.comboCWTXProfile);
            this.panelModeSpecificCW.Controls.Add(this.ptbCWSpeed);
            this.panelModeSpecificCW.Controls.Add(this.udCWPitch);
            this.panelModeSpecificCW.Controls.Add(this.lblCWSpeed);
            this.panelModeSpecificCW.Controls.Add(this.grpSemiBreakIn);
            this.panelModeSpecificCW.Controls.Add(this.lblCWPitchFreq);
            this.panelModeSpecificCW.Controls.Add(this.chkShowTXCWFreq);
            this.panelModeSpecificCW.Controls.Add(this.chkCWSidetone);
            this.panelModeSpecificCW.Controls.Add(this.chkCWIambic);
            this.panelModeSpecificCW.Name = "panelModeSpecificCW";
            this.panelModeSpecificCW.Paint += new System.Windows.Forms.PaintEventHandler(this.panelRing_Paint);
            // 
            // labelTS6
            // 
            this.labelTS6.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.labelTS6, "labelTS6");
            this.labelTS6.Name = "labelTS6";
            // 
            // lblCWSpeed
            // 
            this.lblCWSpeed.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.lblCWSpeed, "lblCWSpeed");
            this.lblCWSpeed.Name = "lblCWSpeed";
            // 
            // grpSemiBreakIn
            // 
            this.grpSemiBreakIn.Controls.Add(this.udCWBreakInDelay);
            this.grpSemiBreakIn.Controls.Add(this.lblCWBreakInDelay);
            this.grpSemiBreakIn.Controls.Add(this.chkCWBreakInEnabled);
            this.grpSemiBreakIn.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.grpSemiBreakIn, "grpSemiBreakIn");
            this.grpSemiBreakIn.Name = "grpSemiBreakIn";
            this.grpSemiBreakIn.TabStop = false;
            // 
            // lblCWBreakInDelay
            // 
            this.lblCWBreakInDelay.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.lblCWBreakInDelay, "lblCWBreakInDelay");
            this.lblCWBreakInDelay.Name = "lblCWBreakInDelay";
            // 
            // lblCWPitchFreq
            // 
            this.lblCWPitchFreq.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.lblCWPitchFreq, "lblCWPitchFreq");
            this.lblCWPitchFreq.Name = "lblCWPitchFreq";
            // 
            // panelModeSpecificDigital
            // 
            resources.ApplyResources(this.panelModeSpecificDigital, "panelModeSpecificDigital");
            this.panelModeSpecificDigital.BackColor = System.Drawing.Color.Transparent;
            this.panelModeSpecificDigital.Controls.Add(this.labelVOXVal);
            this.panelModeSpecificDigital.Controls.Add(this.checkVOX);
            this.panelModeSpecificDigital.Controls.Add(this.chkShowDigTXFilter);
            this.panelModeSpecificDigital.Controls.Add(this.lblVACTXIndicator);
            this.panelModeSpecificDigital.Controls.Add(this.lblVACRXIndicator);
            this.panelModeSpecificDigital.Controls.Add(this.ptbVACTXGain);
            this.panelModeSpecificDigital.Controls.Add(this.comboDigTXProfile);
            this.panelModeSpecificDigital.Controls.Add(this.ptbVACRXGain);
            this.panelModeSpecificDigital.Controls.Add(this.lblDigTXProfile);
            this.panelModeSpecificDigital.Controls.Add(this.lblRXGain);
            this.panelModeSpecificDigital.Controls.Add(this.grpVACStereo);
            this.panelModeSpecificDigital.Controls.Add(this.lblTXGain);
            this.panelModeSpecificDigital.Controls.Add(this.grpDIGSampleRate);
            this.panelModeSpecificDigital.Controls.Add(this.pictureBoxVOX);
            this.panelModeSpecificDigital.Controls.Add(this.prettyTrackBarVOX);
            this.panelModeSpecificDigital.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.panelModeSpecificDigital.Name = "panelModeSpecificDigital";
            this.panelModeSpecificDigital.Paint += new System.Windows.Forms.PaintEventHandler(this.panelRing_Paint);
            // 
            // labelVOXVal
            // 
            resources.ApplyResources(this.labelVOXVal, "labelVOXVal");
            this.labelVOXVal.ForeColor = System.Drawing.Color.White;
            this.labelVOXVal.Name = "labelVOXVal";
            // 
            // lblVACTXIndicator
            // 
            this.lblVACTXIndicator.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            resources.ApplyResources(this.lblVACTXIndicator, "lblVACTXIndicator");
            this.lblVACTXIndicator.Name = "lblVACTXIndicator";
            // 
            // lblVACRXIndicator
            // 
            this.lblVACRXIndicator.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            resources.ApplyResources(this.lblVACRXIndicator, "lblVACRXIndicator");
            this.lblVACRXIndicator.Name = "lblVACRXIndicator";
            // 
            // lblDigTXProfile
            // 
            this.lblDigTXProfile.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.lblDigTXProfile, "lblDigTXProfile");
            this.lblDigTXProfile.Name = "lblDigTXProfile";
            // 
            // lblRXGain
            // 
            this.lblRXGain.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.lblRXGain, "lblRXGain");
            this.lblRXGain.Name = "lblRXGain";
            // 
            // grpVACStereo
            // 
            this.grpVACStereo.Controls.Add(this.chkVACStereo);
            this.grpVACStereo.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.grpVACStereo, "grpVACStereo");
            this.grpVACStereo.Name = "grpVACStereo";
            this.grpVACStereo.TabStop = false;
            // 
            // lblTXGain
            // 
            this.lblTXGain.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.lblTXGain, "lblTXGain");
            this.lblTXGain.Name = "lblTXGain";
            // 
            // grpDIGSampleRate
            // 
            this.grpDIGSampleRate.Controls.Add(this.comboVACSampleRate);
            this.grpDIGSampleRate.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.grpDIGSampleRate, "grpDIGSampleRate");
            this.grpDIGSampleRate.Name = "grpDIGSampleRate";
            this.grpDIGSampleRate.TabStop = false;
            // 
            // pictureBoxVOX
            // 
            this.pictureBoxVOX.BackColor = System.Drawing.SystemColors.ControlText;
            resources.ApplyResources(this.pictureBoxVOX, "pictureBoxVOX");
            this.pictureBoxVOX.Name = "pictureBoxVOX";
            this.pictureBoxVOX.TabStop = false;
            this.pictureBoxVOX.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBoxVOX_Paint);
            // 
            // prettyTrackBarVOX
            // 
            resources.ApplyResources(this.prettyTrackBarVOX, "prettyTrackBarVOX");
            this.prettyTrackBarVOX.HeadImage = null;
            this.prettyTrackBarVOX.LargeChange = 1;
            this.prettyTrackBarVOX.Maximum = 500;
            this.prettyTrackBarVOX.Minimum = 0;
            this.prettyTrackBarVOX.Name = "prettyTrackBarVOX";
            this.prettyTrackBarVOX.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.prettyTrackBarVOX.SmallChange = 1;
            this.prettyTrackBarVOX.TabStop = false;
            this.prettyTrackBarVOX.Value = 100;
            this.prettyTrackBarVOX.Scroll += new PowerSDR.PrettyTrackBar.ScrollHandler(this.prettyTrackBarVOX_Scroll);
            this.prettyTrackBarVOX.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ptbVOX_MouseDown);
            // 
            // panelAntenna
            // 
            resources.ApplyResources(this.panelAntenna, "panelAntenna");
            this.panelAntenna.BackColor = System.Drawing.Color.Transparent;
            this.panelAntenna.Controls.Add(this.lblAntTX2);
            this.panelAntenna.Controls.Add(this.lblAntRX2);
            this.panelAntenna.Controls.Add(this.lblAntRX1);
            this.panelAntenna.Controls.Add(this.lblAntTX);
            this.panelAntenna.Controls.Add(this.lblAntRX1a);
            this.panelAntenna.Controls.Add(this.lblAntTXa);
            this.panelAntenna.Controls.Add(this.lblAntRX2a);
            this.panelAntenna.Controls.Add(this.lblAntTX2a);
            this.panelAntenna.Name = "panelAntenna";
            this.panelAntenna.Paint += new System.Windows.Forms.PaintEventHandler(this.panelAntenna_Paint);
            // 
            // panelRX2Filter
            // 
            resources.ApplyResources(this.panelRX2Filter, "panelRX2Filter");
            this.panelRX2Filter.BackColor = System.Drawing.Color.Transparent;
            this.panelRX2Filter.ContextMenuStrip = this.contextMenuStripFilterRX2;
            this.panelRX2Filter.Controls.Add(this.udRX2FilterHigh);
            this.panelRX2Filter.Controls.Add(this.radRX2Filter1);
            this.panelRX2Filter.Controls.Add(this.udRX2FilterLow);
            this.panelRX2Filter.Controls.Add(this.lblRX2FilterHigh);
            this.panelRX2Filter.Controls.Add(this.lblRX2FilterLow);
            this.panelRX2Filter.Controls.Add(this.radRX2Filter2);
            this.panelRX2Filter.Controls.Add(this.radRX2FilterVar2);
            this.panelRX2Filter.Controls.Add(this.radRX2Filter3);
            this.panelRX2Filter.Controls.Add(this.radRX2FilterVar1);
            this.panelRX2Filter.Controls.Add(this.radRX2Filter4);
            this.panelRX2Filter.Controls.Add(this.radRX2Filter7);
            this.panelRX2Filter.Controls.Add(this.radRX2Filter5);
            this.panelRX2Filter.Controls.Add(this.radRX2Filter6);
            this.panelRX2Filter.Name = "panelRX2Filter";
            this.panelRX2Filter.Paint += new System.Windows.Forms.PaintEventHandler(this.panelRing_Paint);
            // 
            // lblRX2FilterHigh
            // 
            this.lblRX2FilterHigh.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            resources.ApplyResources(this.lblRX2FilterHigh, "lblRX2FilterHigh");
            this.lblRX2FilterHigh.Name = "lblRX2FilterHigh";
            // 
            // lblRX2FilterLow
            // 
            this.lblRX2FilterLow.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            resources.ApplyResources(this.lblRX2FilterLow, "lblRX2FilterLow");
            this.lblRX2FilterLow.Name = "lblRX2FilterLow";
            // 
            // radRX2Filter2
            // 
            resources.ApplyResources(this.radRX2Filter2, "radRX2Filter2");
            this.radRX2Filter2.FlatAppearance.BorderSize = 0;
            this.radRX2Filter2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radRX2Filter2.Name = "radRX2Filter2";
            this.radRX2Filter2.CheckedChanged += new System.EventHandler(this.radRX2Filter2_CheckedChanged);
            // 
            // radRX2Filter3
            // 
            resources.ApplyResources(this.radRX2Filter3, "radRX2Filter3");
            this.radRX2Filter3.FlatAppearance.BorderSize = 0;
            this.radRX2Filter3.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radRX2Filter3.Name = "radRX2Filter3";
            this.radRX2Filter3.CheckedChanged += new System.EventHandler(this.radRX2Filter3_CheckedChanged);
            // 
            // radRX2Filter4
            // 
            resources.ApplyResources(this.radRX2Filter4, "radRX2Filter4");
            this.radRX2Filter4.FlatAppearance.BorderSize = 0;
            this.radRX2Filter4.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radRX2Filter4.Name = "radRX2Filter4";
            this.radRX2Filter4.CheckedChanged += new System.EventHandler(this.radRX2Filter4_CheckedChanged);
            // 
            // radRX2Filter7
            // 
            resources.ApplyResources(this.radRX2Filter7, "radRX2Filter7");
            this.radRX2Filter7.FlatAppearance.BorderSize = 0;
            this.radRX2Filter7.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radRX2Filter7.Name = "radRX2Filter7";
            this.radRX2Filter7.CheckedChanged += new System.EventHandler(this.radRX2Filter7_CheckedChanged);
            // 
            // radRX2Filter5
            // 
            resources.ApplyResources(this.radRX2Filter5, "radRX2Filter5");
            this.radRX2Filter5.FlatAppearance.BorderSize = 0;
            this.radRX2Filter5.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radRX2Filter5.Name = "radRX2Filter5";
            this.radRX2Filter5.CheckedChanged += new System.EventHandler(this.radRX2Filter5_CheckedChanged);
            // 
            // radRX2Filter6
            // 
            resources.ApplyResources(this.radRX2Filter6, "radRX2Filter6");
            this.radRX2Filter6.FlatAppearance.BorderSize = 0;
            this.radRX2Filter6.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radRX2Filter6.Name = "radRX2Filter6";
            this.radRX2Filter6.CheckedChanged += new System.EventHandler(this.radRX2Filter6_CheckedChanged);
            // 
            // panelRX2Mode
            // 
            resources.ApplyResources(this.panelRX2Mode, "panelRX2Mode");
            this.panelRX2Mode.BackColor = System.Drawing.Color.Transparent;
            this.panelRX2Mode.Controls.Add(this.radRX2ModeAM);
            this.panelRX2Mode.Controls.Add(this.radRX2ModeLSB);
            this.panelRX2Mode.Controls.Add(this.radRX2ModeSAM);
            this.panelRX2Mode.Controls.Add(this.radRX2ModeCWL);
            this.panelRX2Mode.Controls.Add(this.radRX2ModeDSB);
            this.panelRX2Mode.Controls.Add(this.radRX2ModeUSB);
            this.panelRX2Mode.Controls.Add(this.radRX2ModeCWU);
            this.panelRX2Mode.Controls.Add(this.radRX2ModeFMN);
            this.panelRX2Mode.Controls.Add(this.radRX2ModeDIGU);
            this.panelRX2Mode.Controls.Add(this.radRX2ModeDRM);
            this.panelRX2Mode.Controls.Add(this.radRX2ModeDIGL);
            this.panelRX2Mode.Controls.Add(this.radRX2ModeSPEC);
            this.panelRX2Mode.Name = "panelRX2Mode";
            this.panelRX2Mode.Paint += new System.Windows.Forms.PaintEventHandler(this.panelRing_Paint);
            // 
            // panelRX2Display
            // 
            resources.ApplyResources(this.panelRX2Display, "panelRX2Display");
            this.panelRX2Display.BackColor = System.Drawing.Color.Transparent;
            this.panelRX2Display.Controls.Add(this.chkRX2DisplayPeak);
            this.panelRX2Display.Controls.Add(this.comboRX2DisplayMode);
            this.panelRX2Display.Controls.Add(this.chkRX2DisplayAVG);
            this.panelRX2Display.Controls.Add(this.label7);
            this.panelRX2Display.Name = "panelRX2Display";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label7.Name = "label7";
            // 
            // panelRX2Mixer
            // 
            resources.ApplyResources(this.panelRX2Mixer, "panelRX2Mixer");
            this.panelRX2Mixer.BackColor = System.Drawing.Color.Transparent;
            this.panelRX2Mixer.Controls.Add(this.ptbRX2Pan);
            this.panelRX2Mixer.Controls.Add(this.ptbRX2Gain);
            this.panelRX2Mixer.Controls.Add(this.lblRX2Pan);
            this.panelRX2Mixer.Controls.Add(this.lblRX2Vol);
            this.panelRX2Mixer.Controls.Add(this.lblRX2Mute);
            this.panelRX2Mixer.Controls.Add(this.chkRX2Mute);
            this.panelRX2Mixer.Controls.Add(this.chkRX1MUTE);
            this.panelRX2Mixer.Name = "panelRX2Mixer";
            // 
            // lblRX2Pan
            // 
            resources.ApplyResources(this.lblRX2Pan, "lblRX2Pan");
            this.lblRX2Pan.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblRX2Pan.Name = "lblRX2Pan";
            // 
            // lblRX2Vol
            // 
            resources.ApplyResources(this.lblRX2Vol, "lblRX2Vol");
            this.lblRX2Vol.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblRX2Vol.Name = "lblRX2Vol";
            // 
            // lblRX2Mute
            // 
            resources.ApplyResources(this.lblRX2Mute, "lblRX2Mute");
            this.lblRX2Mute.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblRX2Mute.Name = "lblRX2Mute";
            // 
            // panelMultiRX
            // 
            resources.ApplyResources(this.panelMultiRX, "panelMultiRX");
            this.panelMultiRX.BackColor = System.Drawing.Color.Transparent;
            this.panelMultiRX.Controls.Add(this.label5);
            this.panelMultiRX.Controls.Add(this.label3);
            this.panelMultiRX.Controls.Add(this.label4);
            this.panelMultiRX.Controls.Add(this.label2);
            this.panelMultiRX.Controls.Add(this.ptbRX1Gain);
            this.panelMultiRX.Controls.Add(this.ptbPanSubRX);
            this.panelMultiRX.Controls.Add(this.ptbRX0Gain);
            this.panelMultiRX.Controls.Add(this.ptbPanMainRX);
            this.panelMultiRX.Controls.Add(this.chkPanSwap);
            this.panelMultiRX.Controls.Add(this.chkEnableMultiRX);
            this.panelMultiRX.Name = "panelMultiRX";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label5.Name = "label5";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label4.Name = "label4";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label2.Name = "label2";
            // 
            // panelDSP
            // 
            resources.ApplyResources(this.panelDSP, "panelDSP");
            this.panelDSP.BackColor = System.Drawing.Color.Transparent;
            this.panelDSP.Controls.Add(this.chkSR);
            this.panelDSP.Controls.Add(this.chkNR);
            this.panelDSP.Controls.Add(this.chkDSPNB2);
            this.panelDSP.Controls.Add(this.chkBIN);
            this.panelDSP.Controls.Add(this.chkNB);
            this.panelDSP.Controls.Add(this.chkANF);
            this.panelDSP.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.panelDSP.Name = "panelDSP";
            // 
            // lblCPUMeter
            // 
            this.lblCPUMeter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.lblCPUMeter.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.lblCPUMeter, "lblCPUMeter");
            this.lblCPUMeter.ForeColor = System.Drawing.Color.White;
            this.lblCPUMeter.Name = "lblCPUMeter";
            this.lblCPUMeter.Click += new System.EventHandler(this.lblCPUMeter_Click);
            this.lblCPUMeter.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.labelTS4_MouseWheel);
            // 
            // panelDateTime
            // 
            resources.ApplyResources(this.panelDateTime, "panelDateTime");
            this.panelDateTime.BackColor = System.Drawing.Color.Transparent;
            this.panelDateTime.Controls.Add(this.txtTimer);
            this.panelDateTime.Controls.Add(this.txtNOAA2);
            this.panelDateTime.Controls.Add(this.txtNOAA);
            this.panelDateTime.Controls.Add(this.labelTS4);
            this.panelDateTime.Controls.Add(this.labelTS3);
            this.panelDateTime.Controls.Add(this.txtTime);
            this.panelDateTime.Controls.Add(this.txtDate);
            this.panelDateTime.Controls.Add(this.lblCPUMeter);
            this.panelDateTime.Name = "panelDateTime";
            this.panelDateTime.Paint += new System.Windows.Forms.PaintEventHandler(this.panelDateTime_Paint);
            // 
            // txtTime
            // 
            this.txtTime.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.txtTime.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.txtTime, "txtTime");
            this.txtTime.ForeColor = System.Drawing.Color.White;
            this.txtTime.Name = "txtTime";
            this.txtTime.ReadOnly = true;
            this.txtTime.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DateTime_MouseDown);
            this.txtTime.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.labelTS4_MouseWheel);
            // 
            // txtDate
            // 
            this.txtDate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.txtDate.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.txtDate, "txtDate");
            this.txtDate.ForeColor = System.Drawing.Color.White;
            this.txtDate.Name = "txtDate";
            this.txtDate.ReadOnly = true;
            this.txtDate.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DateTime_MouseDown);
            this.txtDate.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.labelTS4_MouseWheel);
            // 
            // panelSoundControls
            // 
            resources.ApplyResources(this.panelSoundControls, "panelSoundControls");
            this.panelSoundControls.BackColor = System.Drawing.Color.Transparent;
            this.panelSoundControls.Controls.Add(this.ptbMON);
            this.panelSoundControls.Controls.Add(this.lblMON);
            this.panelSoundControls.Controls.Add(this.lblTUNE);
            this.panelSoundControls.Controls.Add(this.ptbTune);
            this.panelSoundControls.Controls.Add(this.chkBoxDrive);
            this.panelSoundControls.Controls.Add(this.comboAGC);
            this.panelSoundControls.Controls.Add(this.ptbPWR);
            this.panelSoundControls.Controls.Add(this.ptbRF);
            this.panelSoundControls.Controls.Add(this.ptbAF);
            this.panelSoundControls.Controls.Add(this.lblAF);
            this.panelSoundControls.Controls.Add(this.lblAGC);
            this.panelSoundControls.Controls.Add(this.lblPreamp);
            this.panelSoundControls.Controls.Add(this.comboPreamp);
            this.panelSoundControls.Controls.Add(this.lblRF);
            this.panelSoundControls.Controls.Add(this.lblPWR);
            this.panelSoundControls.Controls.Add(this.chkRX1Preamp);
            this.panelSoundControls.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.panelSoundControls.Name = "panelSoundControls";
            // 
            // panelDisplay
            // 
            resources.ApplyResources(this.panelDisplay, "panelDisplay");
            this.panelDisplay.BackColor = System.Drawing.Color.Transparent;
            this.panelDisplay.Controls.Add(this.ptbDisplayPan2);
            this.panelDisplay.Controls.Add(this.ptbDisplayZoom);
            this.panelDisplay.Controls.Add(this.panelTSBandStack);
            this.panelDisplay.Controls.Add(this.buttonVK2);
            this.panelDisplay.Controls.Add(this.buttonVK1);
            this.panelDisplay.Controls.Add(this.lblDisplayZoom1);
            this.panelDisplay.Controls.Add(this.lblDisplayPan1);
            this.panelDisplay.Controls.Add(this.buttonCQ1);
            this.panelDisplay.Controls.Add(this.buttonCall1);
            this.panelDisplay.Controls.Add(this.btnDisplayPanCenter);
            this.panelDisplay.Controls.Add(this.ptbDisplayPan);
            this.panelDisplay.Controls.Add(this.udCQCQRepeat);
            this.panelDisplay.Controls.Add(this.ScreenCap);
            this.panelDisplay.Controls.Add(this.picDisplay);
            this.panelDisplay.Controls.Add(this.radDisplayZoom4x);
            this.panelDisplay.Controls.Add(this.radDisplayZoom2x);
            this.panelDisplay.Controls.Add(this.radDisplayZoom1x);
            this.panelDisplay.Controls.Add(this.radDisplayZoom05);
            this.panelDisplay.Controls.Add(this.txtDisplayPeakOffset);
            this.panelDisplay.Controls.Add(this.txtDisplayCursorOffset);
            this.panelDisplay.Controls.Add(this.autoBrightBox);
            this.panelDisplay.Controls.Add(this.txtDisplayCursorPower);
            this.panelDisplay.Controls.Add(this.txtDisplayCursorFreq);
            this.panelDisplay.Controls.Add(this.txtDisplayPeakPower);
            this.panelDisplay.Controls.Add(this.txtDisplayPeakFreq);
            this.panelDisplay.Controls.Add(this.ScreenCap1);
            this.panelDisplay.Controls.Add(this.ptbDisplayZoom2);
            this.panelDisplay.Name = "panelDisplay";
            this.panelDisplay.MouseEnter += new System.EventHandler(this.Console_MouseEnter);
            // 
            // picDisplay
            // 
            this.picDisplay.BackColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.picDisplay, "picDisplay");
            this.picDisplay.Name = "picDisplay";
            this.picDisplay.TabStop = false;
            this.picDisplay.Paint += new System.Windows.Forms.PaintEventHandler(this.picDisplay_Paint);
            this.picDisplay.DoubleClick += new System.EventHandler(this.picDisplay_DoubleClick);
            this.picDisplay.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picDisplay_MouseDown);
            this.picDisplay.MouseEnter += new System.EventHandler(this.Console_MouseEnter);
            this.picDisplay.MouseLeave += new System.EventHandler(this.picDisplay_MouseLeave);
            this.picDisplay.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picDisplay_MouseMove);
            this.picDisplay.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picDisplay_MouseUp);
            this.picDisplay.Resize += new System.EventHandler(this.picDisplay_Resize);
            // 
            // panelFilter
            // 
            resources.ApplyResources(this.panelFilter, "panelFilter");
            this.panelFilter.BackColor = System.Drawing.Color.Transparent;
            this.panelFilter.ContextMenuStrip = this.contextMenuStripFilterRX1;
            this.panelFilter.Controls.Add(this.ptbFilterShift);
            this.panelFilter.Controls.Add(this.ptbFilterWidth);
            this.panelFilter.Controls.Add(this.btnFilterShiftReset);
            this.panelFilter.Controls.Add(this.udFilterHigh);
            this.panelFilter.Controls.Add(this.radFilter1);
            this.panelFilter.Controls.Add(this.udFilterLow);
            this.panelFilter.Controls.Add(this.lblFilterHigh);
            this.panelFilter.Controls.Add(this.lblFilterLow);
            this.panelFilter.Controls.Add(this.lblFilterWidth);
            this.panelFilter.Controls.Add(this.radFilterVar2);
            this.panelFilter.Controls.Add(this.radFilterVar1);
            this.panelFilter.Controls.Add(this.radFilter10);
            this.panelFilter.Controls.Add(this.lblFilterShift);
            this.panelFilter.Controls.Add(this.radFilter9);
            this.panelFilter.Controls.Add(this.radFilter8);
            this.panelFilter.Controls.Add(this.radFilter2);
            this.panelFilter.Controls.Add(this.radFilter7);
            this.panelFilter.Controls.Add(this.radFilter3);
            this.panelFilter.Controls.Add(this.radFilter6);
            this.panelFilter.Controls.Add(this.radFilter4);
            this.panelFilter.Controls.Add(this.radFilter5);
            this.panelFilter.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.panelFilter.Name = "panelFilter";
            this.panelFilter.Paint += new System.Windows.Forms.PaintEventHandler(this.panelRing_Paint);
            // 
            // lblFilterHigh
            // 
            this.lblFilterHigh.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.lblFilterHigh, "lblFilterHigh");
            this.lblFilterHigh.Name = "lblFilterHigh";
            // 
            // lblFilterLow
            // 
            this.lblFilterLow.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.lblFilterLow, "lblFilterLow");
            this.lblFilterLow.Name = "lblFilterLow";
            // 
            // lblFilterWidth
            // 
            this.lblFilterWidth.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.lblFilterWidth, "lblFilterWidth");
            this.lblFilterWidth.Name = "lblFilterWidth";
            // 
            // radFilter10
            // 
            resources.ApplyResources(this.radFilter10, "radFilter10");
            this.radFilter10.FlatAppearance.BorderSize = 0;
            this.radFilter10.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radFilter10.Name = "radFilter10";
            this.radFilter10.CheckedChanged += new System.EventHandler(this.radFilter10_CheckedChanged);
            // 
            // lblFilterShift
            // 
            this.lblFilterShift.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.lblFilterShift, "lblFilterShift");
            this.lblFilterShift.Name = "lblFilterShift";
            // 
            // radFilter9
            // 
            resources.ApplyResources(this.radFilter9, "radFilter9");
            this.radFilter9.FlatAppearance.BorderSize = 0;
            this.radFilter9.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radFilter9.Name = "radFilter9";
            this.radFilter9.CheckedChanged += new System.EventHandler(this.radFilter9_CheckedChanged);
            // 
            // radFilter8
            // 
            resources.ApplyResources(this.radFilter8, "radFilter8");
            this.radFilter8.FlatAppearance.BorderSize = 0;
            this.radFilter8.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radFilter8.Name = "radFilter8";
            this.radFilter8.CheckedChanged += new System.EventHandler(this.radFilter8_CheckedChanged);
            // 
            // radFilter2
            // 
            resources.ApplyResources(this.radFilter2, "radFilter2");
            this.radFilter2.FlatAppearance.BorderSize = 0;
            this.radFilter2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radFilter2.Name = "radFilter2";
            this.radFilter2.CheckedChanged += new System.EventHandler(this.radFilter2_CheckedChanged);
            // 
            // radFilter7
            // 
            resources.ApplyResources(this.radFilter7, "radFilter7");
            this.radFilter7.FlatAppearance.BorderSize = 0;
            this.radFilter7.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radFilter7.Name = "radFilter7";
            this.radFilter7.CheckedChanged += new System.EventHandler(this.radFilter7_CheckedChanged);
            // 
            // radFilter3
            // 
            resources.ApplyResources(this.radFilter3, "radFilter3");
            this.radFilter3.FlatAppearance.BorderSize = 0;
            this.radFilter3.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radFilter3.Name = "radFilter3";
            this.radFilter3.CheckedChanged += new System.EventHandler(this.radFilter3_CheckedChanged);
            // 
            // radFilter6
            // 
            resources.ApplyResources(this.radFilter6, "radFilter6");
            this.radFilter6.FlatAppearance.BorderSize = 0;
            this.radFilter6.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radFilter6.Name = "radFilter6";
            this.radFilter6.CheckedChanged += new System.EventHandler(this.radFilter6_CheckedChanged);
            // 
            // radFilter4
            // 
            resources.ApplyResources(this.radFilter4, "radFilter4");
            this.radFilter4.FlatAppearance.BorderSize = 0;
            this.radFilter4.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radFilter4.Name = "radFilter4";
            this.radFilter4.CheckedChanged += new System.EventHandler(this.radFilter4_CheckedChanged);
            // 
            // radFilter5
            // 
            resources.ApplyResources(this.radFilter5, "radFilter5");
            this.radFilter5.FlatAppearance.BorderSize = 0;
            this.radFilter5.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radFilter5.Name = "radFilter5";
            this.radFilter5.CheckedChanged += new System.EventHandler(this.radFilter5_CheckedChanged);
            // 
            // panelMode
            // 
            resources.ApplyResources(this.panelMode, "panelMode");
            this.panelMode.BackColor = System.Drawing.Color.Transparent;
            this.panelMode.Controls.Add(this.radModeAM);
            this.panelMode.Controls.Add(this.radModeLSB);
            this.panelMode.Controls.Add(this.radModeSAM);
            this.panelMode.Controls.Add(this.radModeCWL);
            this.panelMode.Controls.Add(this.radModeDSB);
            this.panelMode.Controls.Add(this.radModeUSB);
            this.panelMode.Controls.Add(this.radModeCWU);
            this.panelMode.Controls.Add(this.radModeFMN);
            this.panelMode.Controls.Add(this.radModeDIGU);
            this.panelMode.Controls.Add(this.radModeDRM);
            this.panelMode.Controls.Add(this.radModeDIGL);
            this.panelMode.Controls.Add(this.radModeSPEC);
            this.panelMode.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.panelMode.Name = "panelMode";
            this.panelMode.Paint += new System.Windows.Forms.PaintEventHandler(this.panelRing_Paint);
            // 
            // lblDisplayModeTop
            // 
            resources.ApplyResources(this.lblDisplayModeTop, "lblDisplayModeTop");
            this.lblDisplayModeTop.Name = "lblDisplayModeTop";
            // 
            // lblDisplayModeBottom
            // 
            resources.ApplyResources(this.lblDisplayModeBottom, "lblDisplayModeBottom");
            this.lblDisplayModeBottom.Name = "lblDisplayModeBottom";
            // 
            // grpDisplaySplit
            // 
            this.grpDisplaySplit.Controls.Add(this.chkSplitDisplay);
            this.grpDisplaySplit.Controls.Add(this.comboDisplayModeTop);
            this.grpDisplaySplit.Controls.Add(this.comboDisplayModeBottom);
            this.grpDisplaySplit.Controls.Add(this.lblDisplayModeTop);
            this.grpDisplaySplit.Controls.Add(this.lblDisplayModeBottom);
            resources.ApplyResources(this.grpDisplaySplit, "grpDisplaySplit");
            this.grpDisplaySplit.Name = "grpDisplaySplit";
            this.grpDisplaySplit.TabStop = false;
            // 
            // chkRX2
            // 
            resources.ApplyResources(this.chkRX2, "chkRX2");
            this.chkRX2.FlatAppearance.BorderSize = 0;
            this.chkRX2.Name = "chkRX2";
            this.chkRX2.CheckedChanged += new System.EventHandler(this.chkRX2_CheckedChanged);
            // 
            // lblRX2Band
            // 
            this.lblRX2Band.BackColor = System.Drawing.Color.Transparent;
            this.lblRX2Band.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            resources.ApplyResources(this.lblRX2Band, "lblRX2Band");
            this.lblRX2Band.Name = "lblRX2Band";
            // 
            // panelRX2DSP
            // 
            resources.ApplyResources(this.panelRX2DSP, "panelRX2DSP");
            this.panelRX2DSP.BackColor = System.Drawing.Color.Transparent;
            this.panelRX2DSP.Controls.Add(this.chkRX2NB2);
            this.panelRX2DSP.Controls.Add(this.chkRX2NR);
            this.panelRX2DSP.Controls.Add(this.chkRX2NB);
            this.panelRX2DSP.Controls.Add(this.lblRX2AGC);
            this.panelRX2DSP.Controls.Add(this.chkRX2ANF);
            this.panelRX2DSP.Controls.Add(this.comboRX2AGC);
            this.panelRX2DSP.Controls.Add(this.chkRX2SR);
            this.panelRX2DSP.Controls.Add(this.chkRX2BIN);
            this.panelRX2DSP.Name = "panelRX2DSP";
            // 
            // ptbSquelch
            // 
            this.ptbSquelch.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.ptbSquelch, "ptbSquelch");
            this.ptbSquelch.HeadImage = null;
            this.ptbSquelch.LargeChange = 1;
            this.ptbSquelch.Maximum = 0;
            this.ptbSquelch.Minimum = -160;
            this.ptbSquelch.Name = "ptbSquelch";
            this.ptbSquelch.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.ptbSquelch.SmallChange = 1;
            this.ptbSquelch.TabStop = false;
            this.ptbSquelch.Value = -150;
            this.ptbSquelch.Scroll += new PowerSDR.PrettyTrackBar.ScrollHandler(this.ptbSquelch_Scroll);
            // 
            // Console
            // 
            resources.ApplyResources(this, "$this");
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.Controls.Add(this.panelBandVHFRX2);
            this.Controls.Add(this.panelBandHFRX2);
            this.Controls.Add(this.panelBandVHF);
            this.Controls.Add(this.pwrMstSWR);
            this.Controls.Add(this.pwrMstWatts);
            this.Controls.Add(this.grpRX2Meter);
            this.Controls.Add(this.panelBandGNRX2);
            this.Controls.Add(this.grpMultimeter);
            this.Controls.Add(this.panelBandHF);
            this.Controls.Add(this.labelSize);
            this.Controls.Add(this.labelMove);
            this.Controls.Add(this.panelModeSpecificFM);
            this.Controls.Add(this.checkBoxIICPTT);
            this.Controls.Add(this.checkBoxIICON);
            this.Controls.Add(this.labelPowerSDR);
            this.Controls.Add(this.panelVFO);
            this.Controls.Add(this.panelTS1);
            this.Controls.Add(this.VFODialBB);
            this.Controls.Add(this.VFODialAA);
            this.Controls.Add(this.grpVFOBetween);
            this.Controls.Add(this.grpVFOB);
            this.Controls.Add(this.grpVFOA);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.panelDisplay2);
            this.Controls.Add(this.picRX2Squelch);
            this.Controls.Add(this.ptbRX2Squelch);
            this.Controls.Add(this.ptbRX2RF);
            this.Controls.Add(this.panelOptions);
            this.Controls.Add(this.panelMode);
            this.Controls.Add(this.panelModeSpecificCW);
            this.Controls.Add(this.panelAntenna);
            this.Controls.Add(this.panelFilter);
            this.Controls.Add(this.panelRX2Filter);
            this.Controls.Add(this.panelRX2Mode);
            this.Controls.Add(this.panelRX2Display);
            this.Controls.Add(this.panelRX2DSP);
            this.Controls.Add(this.panelRX2Mixer);
            this.Controls.Add(this.panelMultiRX);
            this.Controls.Add(this.panelDSP);
            this.Controls.Add(this.panelDateTime);
            this.Controls.Add(this.panelSoundControls);
            this.Controls.Add(this.comboRX2Band);
            this.Controls.Add(this.lblRX2Band);
            this.Controls.Add(this.chkRX2Preamp);
            this.Controls.Add(this.chkRX2Squelch);
            this.Controls.Add(this.lblRX2RF);
            this.Controls.Add(this.grpDisplaySplit);
            this.Controls.Add(this.chkBCI);
            this.Controls.Add(this.picSquelch);
            this.Controls.Add(this.chkPower);
            this.Controls.Add(this.chkSquelch);
            this.Controls.Add(this.chkRX2);
            this.Controls.Add(this.panelDisplay);
            this.Controls.Add(this.ptbSquelch);
            this.Controls.Add(this.VFODialA);
            this.Controls.Add(this.panelModeSpecificDigital);
            this.Controls.Add(this.panelModeSpecificPhone);
            this.Controls.Add(this.VFODialB);
            this.Controls.Add(this.labelMax);
            this.Controls.Add(this.panelBandGN);
            this.DoubleBuffered = true;
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Console";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.Console_Closing1);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Console_Closing);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Console_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Console_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Console_KeyPress);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Console_KeyUp);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Console_MouseDown);
            this.MouseEnter += new System.EventHandler(this.Console_MouseEnter);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Console_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Console_MouseUp);
            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.Console_MouseWheel);
            this.Resize += new System.EventHandler(this.Console_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.picRX3Meter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picRX2Meter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMultiMeterDigital)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udFM1750Timer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udFMOffset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udRIT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udXIT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptbRX2RF)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udCWPitch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udCWBreakInDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udRX2FilterHigh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udRX2FilterLow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udFilterHigh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udFilterLow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptbCWSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptbRX2Pan)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptbRX2Gain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptbRX1Gain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptbPanSubRX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptbRX0Gain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptbPanMainRX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptbPWR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptbRF)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptbAF)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptbVACTXGain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptbVACRXGain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptbDisplayZoom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptbDisplayPan)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptbFilterShift)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptbFilterWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udTXFilterLow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udTXFilterHigh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptbTune)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptbMON)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptbMic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptbNoiseGate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptbCPDR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptbDX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScreenCap)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScreenCap1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptbVOX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udCQCQRepeat)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.buttonCall1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.buttonCQ1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDisplayPan1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDisplayZoom1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.buttonVK1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.buttonVK2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptbDisplayZoom2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptbDisplayPan2)).EndInit();
            this.contextMenuStripFilterRX1.ResumeLayout(false);
            this.contextMenuStripFilterRX2.ResumeLayout(false);
            this.contextMenuStripNotch.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picRX2Squelch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSquelch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VFODialA)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VFODialB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VFODialAA)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VFODialBB)).EndInit();
            this.panelBandVHFRX2.ResumeLayout(false);
            this.panelBandHFRX2.ResumeLayout(false);
            this.panelBandVHF.ResumeLayout(false);
            this.grpRX2Meter.ResumeLayout(false);
            this.grpRX2Meter.PerformLayout();
            this.panelBandGNRX2.ResumeLayout(false);
            this.grpMultimeter.ResumeLayout(false);
            this.grpMultimeter.PerformLayout();
            this.panelBandHF.ResumeLayout(false);
            this.panelModeSpecificFM.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ptbFMMic)).EndInit();
            this.panelVFO.ResumeLayout(false);
            this.panelTS1.ResumeLayout(false);
            this.grpVFOBetween.ResumeLayout(false);
            this.grpVFOBetween.PerformLayout();
            this.grpVFOB.ResumeLayout(false);
            this.grpVFOB.PerformLayout();
            this.grpVFOA.ResumeLayout(false);
            this.grpVFOA.PerformLayout();
            this.panelDisplay2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ptbRX2Squelch)).EndInit();
            this.panelOptions.ResumeLayout(false);
            this.panelBandGN.ResumeLayout(false);
            this.panelTSBandStack.ResumeLayout(false);
            this.panelTSBandStack.PerformLayout();
            this.panelModeSpecificPhone.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picNoiseGate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picVOX)).EndInit();
            this.panelModeSpecificCW.ResumeLayout(false);
            this.grpSemiBreakIn.ResumeLayout(false);
            this.panelModeSpecificDigital.ResumeLayout(false);
            this.grpVACStereo.ResumeLayout(false);
            this.grpDIGSampleRate.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxVOX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.prettyTrackBarVOX)).EndInit();
            this.panelAntenna.ResumeLayout(false);
            this.panelRX2Filter.ResumeLayout(false);
            this.panelRX2Mode.ResumeLayout(false);
            this.panelRX2Display.ResumeLayout(false);
            this.panelRX2Mixer.ResumeLayout(false);
            this.panelMultiRX.ResumeLayout(false);
            this.panelDSP.ResumeLayout(false);
            this.panelDateTime.ResumeLayout(false);
            this.panelSoundControls.ResumeLayout(false);
            this.panelDisplay.ResumeLayout(false);
            this.panelDisplay.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picDisplay)).EndInit();
            this.panelFilter.ResumeLayout(false);
            this.panelMode.ResumeLayout(false);
            this.grpDisplaySplit.ResumeLayout(false);
            this.panelRX2DSP.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ptbSquelch)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        } // InitializeComponent()

        #endregion




        #region Windows Form Generated Code

        private System.Windows.Forms.ButtonTS btnHidden;
        public System.Windows.Forms.TextBoxTS txtVFOAFreq;
        private System.Windows.Forms.TextBoxTS txtVFOABand;
        public System.Windows.Forms.TextBoxTS txtVFOBFreq;
        public System.Windows.Forms.PictureBox picDisplay;
        public System.Windows.Forms.PanelTS grpVFOA;
        public System.Windows.Forms.PanelTS grpVFOB;
        private System.Windows.Forms.TextBoxTS txtVFOBBand;
        public System.Windows.Forms.CheckBoxTS chkPower;
        private System.Windows.Forms.RichTextBox lblCPUMeter; // ke9ns mod   private System.Windows.Forms.LabelTS lblCPUMeter;
        private System.Windows.Forms.NumericUpDownTS udFilterLow;
        private System.Windows.Forms.NumericUpDownTS udFilterHigh;
        private System.Windows.Forms.RadioButtonTS radFilterVar1;
        private System.Windows.Forms.RadioButtonTS radFilterVar2;
        private System.Windows.Forms.RadioButtonTS radModeSPEC;
        public System.Windows.Forms.RadioButtonTS radModeLSB;
        private System.Windows.Forms.RadioButtonTS radModeDIGL;
        private System.Windows.Forms.RadioButtonTS radModeCWU;
        private System.Windows.Forms.RadioButtonTS radModeDSB;
        private System.Windows.Forms.RadioButtonTS radModeSAM;
        private System.Windows.Forms.RadioButtonTS radModeAM;
        private System.Windows.Forms.RadioButtonTS radModeCWL;
        public System.Windows.Forms.RadioButtonTS radModeUSB;
        private System.Windows.Forms.RadioButtonTS radModeFMN;
        private System.Windows.Forms.RadioButtonTS radModeDRM;
        private System.Windows.Forms.LabelTS lblAGC;
        private System.Windows.Forms.ComboBoxTS comboAGC;
        private System.Windows.Forms.CheckBoxTS chkNB;
        private System.Windows.Forms.CheckBoxTS chkANF;
        private System.Windows.Forms.CheckBoxTS chkNR;
        public System.Windows.Forms.CheckBoxTS chkMON;
        public System.Windows.Forms.CheckBoxTS chkTUN;
        public System.Windows.Forms.CheckBoxTS chkMOX;
        private System.Windows.Forms.NumericUpDownTS udXIT;
        private System.Windows.Forms.NumericUpDownTS udRIT;
        private System.Windows.Forms.CheckBoxTS chkMUT;
        private System.Windows.Forms.CheckBoxTS chkXIT;
        private System.Windows.Forms.CheckBoxTS chkRIT;
        public System.Windows.Forms.LabelTS lblPWR;
        private System.Windows.Forms.LabelTS lblAF;
        private System.Windows.Forms.LabelTS lblMIC;
        private System.Windows.Forms.TextBoxTS txtWheelTune;
        public System.Windows.Forms.CheckBoxTS chkBIN;
        private System.Windows.Forms.PanelTS grpMultimeter;
        private System.Windows.Forms.ButtonTS btnVFOSwap;
        private System.Windows.Forms.ButtonTS btnVFOBtoA;
        private System.Windows.Forms.ButtonTS btnVFOAtoB;
        public System.Windows.Forms.CheckBoxTS chkVFOSplit;
        private System.Windows.Forms.TextBoxTS txtMultiText;
        private System.Windows.Forms.Timer timer_cpu_meter;
        private System.Windows.Forms.LabelTS lblFilterHigh;
        private System.Windows.Forms.LabelTS lblFilterLow;
        public System.Windows.Forms.LabelTS lblMultiSMeter;
        private System.Windows.Forms.PictureBox picMultiMeterDigital;
        private System.Windows.Forms.PictureBox picRX2Meter;
        private System.Windows.Forms.CheckBoxTS chkSquelch;
        private System.Windows.Forms.Timer timer_peak_text;
        private System.Windows.Forms.TextBoxTS txtMemoryQuick;
        private System.Windows.Forms.ButtonTS btnMemoryQuickSave;
        private System.Windows.Forms.ButtonTS btnMemoryQuickRestore;
        public System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.LabelTS lblFilterShift;
        private System.Windows.Forms.ButtonTS btnFilterShiftReset;
        private System.Windows.Forms.Timer timer_clock;
        private System.Windows.Forms.RichTextBox txtDate;   // ke9ns mod (was texboxts)
        private System.Windows.Forms.Panel panelVFOAHover;
        private System.Windows.Forms.Panel panelVFOBHover;
        private System.Windows.Forms.ComboBoxTS comboMeterRXMode;
        private System.Windows.Forms.ComboBoxTS comboMeterTXMode;
        private System.Windows.Forms.ButtonTS btnXITReset;
        private System.Windows.Forms.ButtonTS btnRITReset;
        private System.Windows.Forms.ComboBoxTS comboTuneMode;
        private System.Windows.Forms.ComboBoxTS comboPreamp;
        private System.Windows.Forms.LabelTS lblPreamp;
        private System.Windows.Forms.CheckBoxTS chkDSPNB2;
        private System.Windows.Forms.CheckBoxTS chkVFOLock;
        private System.Windows.Forms.LabelTS lblFilterWidth;
        private System.Windows.Forms.ButtonTS btnBandHF;
        private System.Windows.Forms.ButtonTS btnBandVHF;
        public System.Windows.Forms.LabelTS lblCWSpeed;
        private System.Windows.Forms.ButtonTS btnIFtoVFO;
        private System.Windows.Forms.ButtonTS btnZeroBeat;
        private System.Windows.Forms.RichTextBox txtTime;  // ke9ns mod
        private System.Windows.Forms.RadioButtonTS radModeDIGU;
        private System.Windows.Forms.RadioButtonTS radFilter1;
        private System.Windows.Forms.RadioButtonTS radFilter2;
        private System.Windows.Forms.RadioButtonTS radFilter3;
        private System.Windows.Forms.RadioButtonTS radFilter4;
        private System.Windows.Forms.RadioButtonTS radFilter5;
        private System.Windows.Forms.RadioButtonTS radFilter6;
        private System.Windows.Forms.RadioButtonTS radFilter7;
        private System.Windows.Forms.RadioButtonTS radFilter8;
        private System.Windows.Forms.RadioButtonTS radFilter9;
        private System.Windows.Forms.RadioButtonTS radFilter10;
        private System.Windows.Forms.LabelTS lblRF;
        private System.Windows.Forms.LabelTS lblTuneStep;
        private System.Windows.Forms.PanelTS grpVFOBetween; // ke9ns mod
        private System.Windows.Forms.CheckBoxTS chkVOX;
        private System.Windows.Forms.LabelTS lblTXGain;
        private System.Windows.Forms.LabelTS lblRXGain;
        private System.Windows.Forms.PictureBox picVOX;
        private System.Windows.Forms.CheckBoxTS chkNoiseGate;
        private System.Windows.Forms.PictureBox picNoiseGate;
        public System.Windows.Forms.TextBoxTS autoBrightBox;
        private System.Windows.Forms.TextBoxTS txtDisplayCursorOffset;
        private System.Windows.Forms.TextBoxTS txtDisplayCursorPower;
        private System.Windows.Forms.TextBoxTS txtDisplayCursorFreq;
        private System.Windows.Forms.TextBoxTS txtDisplayPeakOffset;
        private System.Windows.Forms.TextBoxTS txtDisplayPeakFreq;
        public System.Windows.Forms.TextBoxTS txtDisplayPeakPower;
        public System.Windows.Forms.TextBoxTS txtVFOAMSD;
        public System.Windows.Forms.TextBoxTS txtVFOBMSD;
        private System.Windows.Forms.TextBoxTS txtVFOALSD;
        private System.Windows.Forms.TextBoxTS txtVFOBLSD;
        public System.Windows.Forms.CheckBoxTS chkSR;
        private System.Windows.Forms.ButtonTS btnTuneStepChangeSmaller;
        private System.Windows.Forms.ComboBoxTS comboTXProfile;
        private System.Windows.Forms.CheckBoxTS chkShowTXFilter;
        private System.Windows.Forms.CheckBoxTS chkShowDigTXFilter;
        private System.Windows.Forms.ComboBoxTS comboVACSampleRate;
        private System.Windows.Forms.GroupBoxTS grpDIGSampleRate;
        private System.Windows.Forms.GroupBoxTS grpVACStereo;
        private System.Windows.Forms.CheckBoxTS chkVACStereo;
        private System.Windows.Forms.CheckBoxTS chkCWSidetone;
        private System.Windows.Forms.CheckBoxTS chkCWIambic;
        private System.Windows.Forms.LabelTS lblCWPitchFreq;
        public System.Windows.Forms.NumericUpDownTS udCWPitch;
        private System.Windows.Forms.ButtonTS btnDisplayPanCenter;
        private System.Windows.Forms.LabelTS lblTransmitProfile;
        private System.Windows.Forms.CheckBoxTS chkX2TR;
        private System.Windows.Forms.CheckBoxTS chkShowTXCWFreq;
        private System.Windows.Forms.CheckBoxTS chkPanSwap;
        private System.Windows.Forms.GroupBoxTS grpSemiBreakIn;
        private System.Windows.Forms.LabelTS lblCWBreakInDelay;
        private System.Windows.Forms.CheckBoxTS chkCWBreakInEnabled;
        private System.Windows.Forms.NumericUpDownTS udCWBreakInDelay;
        private System.Windows.Forms.CheckBoxTS chkVAC1;
        private System.Windows.Forms.ComboBoxTS comboDigTXProfile;
        private System.Windows.Forms.LabelTS lblDigTXProfile;
        private System.Windows.Forms.CheckBoxTS chkRXEQ;
        private System.Windows.Forms.CheckBoxTS chkTXEQ;
        private System.Windows.Forms.CheckBoxTS chkBCI;
        private System.ComponentModel.IContainer components;
        public System.Windows.Forms.CheckBoxTS chkEnableMultiRX;
        private System.Windows.Forms.ButtonTS btnTuneStepChangeLarger;
        private System.Windows.Forms.LabelTS lblAntRX1;
        private System.Windows.Forms.LabelTS lblAntTX;
        private System.Windows.Forms.CheckBoxTS chkSplitDisplay;
        private System.Windows.Forms.ComboBoxTS comboDisplayModeTop;
        private System.Windows.Forms.ComboBoxTS comboDisplayModeBottom;
        private System.Windows.Forms.LabelTS lblDisplayModeTop;
        private System.Windows.Forms.LabelTS lblDisplayModeBottom;
        private System.Windows.Forms.CheckBoxTS chkCPDR;
        private System.Windows.Forms.CheckBoxTS chkDX;
        public System.Windows.Forms.CheckBoxTS ckQuickPlay;
        public System.Windows.Forms.CheckBoxTS ckQuickRec;
        private System.Windows.Forms.GroupBoxTS grpDisplaySplit;
        public System.Windows.Forms.CheckBoxTS chkRX2;
        private System.Windows.Forms.CheckBoxTS chkRX2SR;
        private System.Windows.Forms.Panel panelVFOASubHover;
        private System.Windows.Forms.LabelTS lblAntRX2;
        private System.Windows.Forms.RadioButtonTS radRX2ModeAM;
        private System.Windows.Forms.RadioButtonTS radRX2ModeSAM;
        private System.Windows.Forms.RadioButtonTS radRX2ModeDSB;
        private System.Windows.Forms.RadioButtonTS radRX2ModeCWU;
        private System.Windows.Forms.RadioButtonTS radRX2ModeDIGU;
        private System.Windows.Forms.RadioButtonTS radRX2ModeDIGL;
        public System.Windows.Forms.RadioButtonTS radRX2ModeLSB;
        private System.Windows.Forms.RadioButtonTS radRX2ModeSPEC;
        private System.Windows.Forms.RadioButtonTS radRX2ModeDRM;
        private System.Windows.Forms.RadioButtonTS radRX2ModeFMN;
        public System.Windows.Forms.RadioButtonTS radRX2ModeUSB;
        private System.Windows.Forms.RadioButtonTS radRX2ModeCWL;
        private System.Windows.Forms.CheckBoxTS chkRX2BIN;
        private System.Windows.Forms.RadioButtonTS radRX2Filter1;
        private System.Windows.Forms.RadioButtonTS radRX2Filter2;
        private System.Windows.Forms.RadioButtonTS radRX2Filter3;
        private System.Windows.Forms.RadioButtonTS radRX2Filter4;
        private System.Windows.Forms.RadioButtonTS radRX2Filter5;
        private System.Windows.Forms.RadioButtonTS radRX2Filter6;
        private System.Windows.Forms.RadioButtonTS radRX2Filter7;
        private System.Windows.Forms.RadioButtonTS radRX2FilterVar1;
        private System.Windows.Forms.RadioButtonTS radRX2FilterVar2;
        private System.Windows.Forms.PanelTS grpRX2Meter;
        private System.Windows.Forms.ComboBoxTS comboRX2MeterMode;
        private System.Windows.Forms.NumericUpDownTS udRX2FilterLow;
        private System.Windows.Forms.NumericUpDownTS udRX2FilterHigh;
        private System.Windows.Forms.LabelTS lblRX2FilterLow;
        private System.Windows.Forms.LabelTS lblRX2FilterHigh;
        private System.Windows.Forms.CheckBoxTS chkRX2NB2;
        private System.Windows.Forms.CheckBoxTS chkRX2NB;
        private System.Windows.Forms.CheckBoxTS chkRX2ANF;
        private System.Windows.Forms.CheckBoxTS chkRX2NR;

        private System.Windows.Forms.TextBoxTS txtRX2Meter;
        public System.Windows.Forms.LabelTS lblRX2Meter;
        private System.Windows.Forms.CheckBoxTS chkRX2Preamp;
        private System.Windows.Forms.LabelTS lblRX2RF;
        private System.Windows.Forms.PictureBox picSquelch;
        private System.Windows.Forms.CheckBoxTS chkRX2Squelch;
        private System.Windows.Forms.PictureBox picRX2Squelch;
        private System.Windows.Forms.CheckBoxTS chkRX1Preamp;
        private System.Windows.Forms.CheckBoxTS chkRX2Mute;
        private System.Windows.Forms.CheckBoxTS chkRX2DisplayPeak;
        private System.Windows.Forms.ComboBoxTS comboRX2DisplayMode;
        private System.Windows.Forms.CheckBoxTS chkRX2DisplayAVG;
        private System.Windows.Forms.Label lblRX2Mute;
        private System.Windows.Forms.Label lblRX2Pan;
        private System.Windows.Forms.Label lblRX2Vol;
        private System.Windows.Forms.ComboBoxTS comboRX2Band;
        private System.Windows.Forms.LabelTS lblRX2Band;
        private System.Windows.Forms.ComboBoxTS comboRX2AGC;
        private System.Windows.Forms.LabelTS lblRX2AGC;
        private System.Windows.Forms.CheckBoxTS chkVFOSync;
        public System.Windows.Forms.CheckBoxTS chkVFOATX;
        public System.Windows.Forms.CheckBoxTS chkVFOBTX;
        private System.Windows.Forms.PanelTS panelBandHF;
        private System.Windows.Forms.PanelTS panelBandVHF;
        private System.Windows.Forms.PanelTS panelBandGN;
        public System.Windows.Forms.PanelTS panelBandHFRX2;
        public System.Windows.Forms.PanelTS panelBandVHFRX2;
        public System.Windows.Forms.PanelTS panelBandGNRX2;
        private System.Windows.Forms.PanelTS panelMode;
        private System.Windows.Forms.PanelTS panelFilter;
        private System.Windows.Forms.PanelTS panelDisplay;
        private System.Windows.Forms.PanelTS panelOptions;
        private System.Windows.Forms.PanelTS panelSoundControls;
        public System.Windows.Forms.PanelTS panelAntenna;
        private System.Windows.Forms.PanelTS panelDateTime;
        private System.Windows.Forms.PanelTS panelVFO;
        private System.Windows.Forms.PanelTS panelDSP;
        private System.Windows.Forms.PanelTS panelMultiRX;
        private System.Windows.Forms.PanelTS panelModeSpecificCW;
        private System.Windows.Forms.PanelTS panelModeSpecificPhone;
        private System.Windows.Forms.PanelTS panelModeSpecificFM;
        private System.Windows.Forms.PanelTS panelModeSpecificDigital;
        private System.Windows.Forms.RadioButtonTS radBand160;
        private System.Windows.Forms.RadioButtonTS radBand80;
        private System.Windows.Forms.RadioButtonTS radBand60;
        private System.Windows.Forms.RadioButtonTS radBand40;
        private System.Windows.Forms.RadioButtonTS radBand30;
        private System.Windows.Forms.RadioButtonTS radBand20;
        private System.Windows.Forms.RadioButtonTS radBand17;
        private System.Windows.Forms.RadioButtonTS radBand15;
        private System.Windows.Forms.RadioButtonTS radBand12;
        private System.Windows.Forms.RadioButtonTS radBand10;
        private System.Windows.Forms.RadioButtonTS radBand6;
        private System.Windows.Forms.RadioButtonTS radBand2;
        private System.Windows.Forms.RadioButtonTS radBandWWV;
        private System.Windows.Forms.RadioButtonTS radBandGEN;

        private System.Windows.Forms.RadioButtonTS radBandGN13; // ke9ns add
        private System.Windows.Forms.RadioButtonTS radBandGN12;
        private System.Windows.Forms.RadioButtonTS radBandGN11;
        private System.Windows.Forms.RadioButtonTS radBandGN10;
        private System.Windows.Forms.RadioButtonTS radBandGN9;
        private System.Windows.Forms.RadioButtonTS radBandGN8;
        private System.Windows.Forms.RadioButtonTS radBandGN7;
        private System.Windows.Forms.RadioButtonTS radBandGN6;
        private System.Windows.Forms.RadioButtonTS radBandGN5;
        private System.Windows.Forms.RadioButtonTS radBandGN4;
        private System.Windows.Forms.RadioButtonTS radBandGN3;
        private System.Windows.Forms.RadioButtonTS radBandGN2;
        private System.Windows.Forms.RadioButtonTS radBandGN1;
        private System.Windows.Forms.RadioButtonTS radBandGN0;
        private System.Windows.Forms.ButtonTS btnBandHF1;

        private System.Windows.Forms.RadioButtonTS radBandVHF0;
        private System.Windows.Forms.RadioButtonTS radBandVHF11;
        private System.Windows.Forms.RadioButtonTS radBandVHF10;
        private System.Windows.Forms.RadioButtonTS radBandVHF9;
        private System.Windows.Forms.RadioButtonTS radBandVHF8;
        private System.Windows.Forms.RadioButtonTS radBandVHF7;
        private System.Windows.Forms.RadioButtonTS radBandVHF6;
        private System.Windows.Forms.RadioButtonTS radBandVHF5;
        private System.Windows.Forms.RadioButtonTS radBandVHF4;
        private System.Windows.Forms.RadioButtonTS radBandVHF3;
        private System.Windows.Forms.RadioButtonTS radBandVHF2;
        private System.Windows.Forms.RadioButtonTS radBandVHF1;
        private System.Windows.Forms.RadioButtonTS radBandVHF13;
        private System.Windows.Forms.RadioButtonTS radBandVHF12;


        private System.Windows.Forms.PanelTS panelRX2Mixer;
        private System.Windows.Forms.PanelTS panelRX2DSP;
        private System.Windows.Forms.PanelTS panelRX2Display;
        private System.Windows.Forms.PanelTS panelRX2Mode;
        private System.Windows.Forms.PanelTS panelRX2Filter;
        private PrettyTrackBar ptbDisplayPan;
        public PrettyTrackBar ptbDisplayZoom;
        public PrettyTrackBar ptbAF;
        public PrettyTrackBar ptbRF;
        public PrettyTrackBar ptbPWR;
        private PrettyTrackBar ptbSquelch;
        private PrettyTrackBar ptbMic;
        private System.Windows.Forms.LabelTS lblMicVal;
        private PrettyTrackBar ptbDX;
        private System.Windows.Forms.LabelTS lblDXVal;
        private PrettyTrackBar ptbCPDR;
        private System.Windows.Forms.LabelTS lblCPDRVal;
        private PrettyTrackBar ptbVOX;
        private System.Windows.Forms.LabelTS lblVOXVal;
        private PrettyTrackBar ptbNoiseGate;
        private System.Windows.Forms.LabelTS lblNoiseGateVal;
        private PrettyTrackBar ptbFilterWidth;
        private PrettyTrackBar ptbFilterShift;
        public PrettyTrackBar ptbCWSpeed;
        private PrettyTrackBar ptbPanMainRX;
        private PrettyTrackBar ptbPanSubRX;
        private PrettyTrackBar ptbRX2RF;
        private PrettyTrackBar ptbRX2Squelch;
        private PrettyTrackBar ptbRX2Gain;
        private PrettyTrackBar ptbRX2Pan;
        private PrettyTrackBar ptbRX1Gain;
        private PrettyTrackBar ptbRX0Gain;
        private PrettyTrackBar ptbVACRXGain;
        private PrettyTrackBar ptbVACTXGain;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripFilterRX1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemRX1FilterConfigure;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemRX1FilterReset;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripFilterRX2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemRX2FilterConfigure;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemRX2FilterReset;
        public System.Windows.Forms.Timer timer_navigate;
        private System.Windows.Forms.RadioButtonTS radDisplayZoom05;
        private System.Windows.Forms.RadioButtonTS radDisplayZoom4x;
        private System.Windows.Forms.RadioButtonTS radDisplayZoom2x;
        private System.Windows.Forms.RadioButtonTS radDisplayZoom1x;
        private System.Windows.Forms.CheckBoxTS chkFWCATUBypass;
        public System.Windows.Forms.CheckBoxTS chkFWCATU;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripNotch;
        private System.Windows.Forms.ToolStripMenuItem toolStripNotchDelete;
        private System.Windows.Forms.ToolStripMenuItem toolStripNotchRemember;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem toolStripNotchNormal;
        private System.Windows.Forms.ToolStripMenuItem toolStripNotchDeep;
        private System.Windows.Forms.ToolStripMenuItem toolStripNotchVeryDeep;
        private System.Windows.Forms.PanelTS panelDisplay2;
        private System.Windows.Forms.ButtonTS btnTNFAdd;
        private System.Windows.Forms.CheckBoxTS chkTNF;
        private System.Windows.Forms.CheckBoxTS chkDisplayPeak;
        public System.Windows.Forms.ComboBoxTS comboDisplayMode;
        private System.Windows.Forms.CheckBoxTS chkDisplayAVG;

        private System.Windows.Forms.LabelTS lblMicValFM;
        private PrettyTrackBar ptbFMMic;
        private System.Windows.Forms.LabelTS lblFMMic;
        private System.Windows.Forms.LabelTS labelTS7;
        public System.Windows.Forms.ComboBoxTS comboFMTXProfile;
        private System.Windows.Forms.CheckBoxTS chkFMTXLow;
        private System.Windows.Forms.CheckBoxTS chkFMTXSimplex;
        private System.Windows.Forms.CheckBoxTS chkFMTXHigh;
        private System.Windows.Forms.CheckBoxTS chkFMCTCSS;
        private System.Windows.Forms.ComboBoxTS comboFMCTCSS;
        private System.Windows.Forms.RadioButtonTS radFMDeviation2kHz;
        private System.Windows.Forms.RadioButtonTS radFMDeviation5kHz;
        private System.Windows.Forms.LabelTS lblFMDeviation;
        private System.Windows.Forms.CheckBoxTS chkFMTXRev;
        private System.Windows.Forms.LabelTS lblFMOffset;
        private System.Windows.Forms.NumericUpDownTS udFMOffset;
        public System.Windows.Forms.ComboBoxTS comboFMMemory;
        private System.Windows.Forms.ButtonTS btnFMMemoryUp;
        private System.Windows.Forms.ButtonTS btnFMMemoryDown;
        private System.Windows.Forms.LabelTS lblFMMemory;
        private System.Windows.Forms.ButtonTS btnFMMemory;
        public System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem setupToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem memoryToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem waveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem equalizerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem xVTRsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cWXToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uCBToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mixerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem antennaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem relaysToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aTUToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem flexControlToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem eSCToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reportBugToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem remoteProfilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.Timer timerNotchZoom;
        private System.Windows.Forms.ToolStripMenuItem herosToolStripMenuItem;
        private System.Windows.Forms.CheckBoxTS chkVAC2;
        private System.Windows.Forms.LabelTS lblVACRXIndicator;
        private System.Windows.Forms.LabelTS lblVACTXIndicator;
        private System.Windows.Forms.ToolStripMenuItem GrayMenuItem;
        private System.Windows.Forms.RichTextBox labelTS4;  // ke9ns add
        private System.Windows.Forms.RichTextBox labelTS3; // ke9ns add
        public System.Windows.Forms.ToolStripMenuItem TXIDMenuItem;
        public System.Windows.Forms.ToolStripTextBox callsignTextBox;
        private System.Windows.Forms.ToolStripMenuItem ScanMenuItem;
        public System.Windows.Forms.OpenFileDialog openFileDialog1;
        public System.Windows.Forms.TextBoxTS regBox;
        public System.Windows.Forms.TextBoxTS regBox1;
        public System.Windows.Forms.ToolStripMenuItem spotterMenu;
        public System.Windows.Forms.CheckBoxTS checkBoxID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBoxTS comboMeterTX1Mode;
        private System.Windows.Forms.CheckBoxTS chkFullDuplex;


        public System.Windows.Forms.CheckBoxTS chkRX1MUTE;  // ke9ns add allow RX1 mute of flex radio audio but not vac stream audio


        private System.Windows.Forms.RichTextBox txtNOAA2;    // ke9ns add for space weather on main console screen
        private System.Windows.Forms.RichTextBox txtNOAA;    // ke9ns add for space weather on main console screen


        private System.Windows.Forms.LabelTS labelTS2;
        private System.Windows.Forms.LabelTS labelTS1;
        public System.Windows.Forms.NumericUpDownTS udTXFilterLow;
        public System.Windows.Forms.NumericUpDownTS udTXFilterHigh;
        public System.Windows.Forms.CheckBoxTS chkBoxMuteSpk;
        public System.Windows.Forms.CheckBoxTS chkBoxDrive;
        private System.Windows.Forms.LabelTS labelTS5;
        public System.Windows.Forms.PanelTS panelTSBandStack;
        private System.Windows.Forms.Button buttonDel;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonSort;
        public System.Windows.Forms.TextBox textBox1;
        public System.Windows.Forms.RichTextBox txtTimer;
        private System.Windows.Forms.ToolStripMenuItem MapMenuItem;
        public PrettyTrackBar ptbTune;
        public System.Windows.Forms.LabelTS lblTUNE;
        private System.Windows.Forms.LabelTS lblMON;
        public PrettyTrackBar ptbMON;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ImageList imageList1;
        public System.Windows.Forms.PictureBox VFODialA;
        public System.Windows.Forms.PictureBox VFODialB;
        public System.Windows.Forms.PictureBox VFODialAA;
        public System.Windows.Forms.PictureBox VFODialBB;
        private System.Windows.Forms.LabelTS labelTS6;
        private System.Windows.Forms.ComboBoxTS comboCWTXProfile;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.PictureBox picRX3Meter;
        public System.Windows.Forms.PanelTS panelTS1;
        public System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.RichTextBox richTextBox2;
        private System.Windows.Forms.RichTextBox richTextBox3;
        private System.Windows.Forms.RichTextBox richTextBox5;
        private System.Windows.Forms.RichTextBox richTextBox6;
        private System.Windows.Forms.RichTextBox richTextBox7;
        private System.Windows.Forms.RichTextBox richTextBox8;
        private System.Windows.Forms.ToolStripMenuItem SWLMenuItem;
        private System.Windows.Forms.PictureBox ScreenCap;
        private System.Windows.Forms.PictureBox ScreenCap1;
        public System.Windows.Forms.CheckBoxTS checkBoxIICPTT;
        public System.Windows.Forms.CheckBoxTS checkBoxIICON;
        private PrettyTrackBar prettyTrackBarVOX;
        private System.Windows.Forms.LabelTS labelVOXVal;
        private System.Windows.Forms.CheckBoxTS checkVOX;
        private System.Windows.Forms.PictureBox pictureBoxVOX;
        private System.Windows.Forms.NumericUpDownTS udCQCQRepeat;
        private System.Windows.Forms.Label label7;
        public System.Windows.Forms.CheckBoxTS chkBoxBS;
        public System.Windows.Forms.Label labelPowerSDR;
        public System.Windows.Forms.Label labelSize;
        public System.Windows.Forms.Label labelMove;
        public System.Windows.Forms.Label labelMax;
        private System.Windows.Forms.PictureBox pictureBox1;
        public System.Windows.Forms.PictureBox buttonCQ1;
        public System.Windows.Forms.PictureBox buttonCall1;

        public System.Windows.Forms.PictureBox lblDisplayPan1;
        public System.Windows.Forms.PictureBox lblDisplayZoom1;
        public System.Windows.Forms.CheckBoxTS chkFM1750;
        public System.Windows.Forms.PictureBox buttonVK1;
        public System.Windows.Forms.PictureBox buttonVK2;
        private System.Windows.Forms.NumericUpDownTS udFM1750Timer;

        private System.Windows.Forms.RadioButtonTS radBandGENRX2;
        private System.Windows.Forms.RadioButtonTS radBandWWVRX2;
        private System.Windows.Forms.RadioButtonTS radBand2RX2;
        private System.Windows.Forms.RadioButtonTS radBand6RX2;
        private System.Windows.Forms.RadioButtonTS radBand10RX2;
        private System.Windows.Forms.RadioButtonTS radBand12RX2;
        private System.Windows.Forms.RadioButtonTS radBand15RX2;
        private System.Windows.Forms.RadioButtonTS radBand17RX2;
        private System.Windows.Forms.RadioButtonTS radBand20RX2;
        private System.Windows.Forms.RadioButtonTS radBand30RX2;
        private System.Windows.Forms.RadioButtonTS radBand40RX2;
        private System.Windows.Forms.RadioButtonTS radBand60RX2;
        private System.Windows.Forms.RadioButtonTS radBand160RX2;
        private System.Windows.Forms.RadioButtonTS radBand80RX2;
        private System.Windows.Forms.ButtonTS btnBandVHFRX2;

        private System.Windows.Forms.RadioButtonTS radBandGN13RX2;
        private System.Windows.Forms.RadioButtonTS radBandGN12RX2;
        private System.Windows.Forms.RadioButtonTS radBandGN11RX2;
        private System.Windows.Forms.RadioButtonTS radBandGN10RX2;
        private System.Windows.Forms.RadioButtonTS radBandGN9RX2;
        private System.Windows.Forms.RadioButtonTS radBandGN8RX2;
        private System.Windows.Forms.RadioButtonTS radBandGN7RX2;
        private System.Windows.Forms.RadioButtonTS radBandGN6RX2;
        private System.Windows.Forms.RadioButtonTS radBandGN5RX2;
        private System.Windows.Forms.RadioButtonTS radBandGN4RX2;
        private System.Windows.Forms.RadioButtonTS radBandGN3RX2;
        private System.Windows.Forms.RadioButtonTS radBandGN2RX2;
        private System.Windows.Forms.RadioButtonTS radBandGN1RX2;
        private System.Windows.Forms.RadioButtonTS radBandGN0RX2;
        private System.Windows.Forms.ButtonTS btnBandHF1RX2;

        private System.Windows.Forms.RadioButtonTS radBandVHF13RX2;
        private System.Windows.Forms.RadioButtonTS radBandVHF12RX2;
        private System.Windows.Forms.RadioButtonTS radBandVHF11RX2;
        private System.Windows.Forms.RadioButtonTS radBandVHF10RX2;
        private System.Windows.Forms.RadioButtonTS radBandVHF9RX2;
        private System.Windows.Forms.RadioButtonTS radBandVHF8RX2;
        private System.Windows.Forms.RadioButtonTS radBandVHF7RX2;
        private System.Windows.Forms.RadioButtonTS radBandVHF6RX2;
        private System.Windows.Forms.RadioButtonTS radBandVHF5RX2;
        private System.Windows.Forms.RadioButtonTS radBandVHF4RX2;
        private System.Windows.Forms.RadioButtonTS radBandVHF3RX2;
        private System.Windows.Forms.RadioButtonTS radBandVHF2RX2;
        private System.Windows.Forms.RadioButtonTS radBandVHF1RX2;
        private System.Windows.Forms.RadioButtonTS radBandVHF0RX2;
        private System.Windows.Forms.ButtonTS btnBandHFRX2;
        public System.Windows.Forms.TextBox textBox2;
        public System.Windows.Forms.ButtonTS buttonbs;
        public System.Windows.Forms.TextBox pwrMstWatts;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        public System.Windows.Forms.TextBox pwrMstSWR;
        private System.Windows.Forms.LabelTS lblAntTX2;
        private System.Windows.Forms.LabelTS lblAntTX2a;
        private System.Windows.Forms.LabelTS lblAntRX2a;
        public System.Windows.Forms.LabelTS lblAntTXa;
        public System.Windows.Forms.LabelTS lblAntRX1a;
        private System.Windows.Forms.CheckBoxTS chkTXEQ1;
        private System.Windows.Forms.CheckBoxTS chkRXEQ1;
        public PrettyTrackBar ptbDisplayZoom2;
        private PrettyTrackBar ptbDisplayPan2;


        private System.Windows.Forms.RadioButtonTS[] vhf_text;
        private System.Windows.Forms.RadioButtonTS[] vhf_text2; // .212



        #endregion

    } // class console



} // powerSDR