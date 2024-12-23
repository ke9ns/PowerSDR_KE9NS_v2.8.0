//=================================================================
// cwx.cs
//=================================================================
// CWX - new version of the old keyer memory and keyboard stuff
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
//
//         CWX written by Richard Allen, W5SXD
//    with various hooks from Eric Wachsman, KE5DTO, 
//          and Herr Doktor Bob McGwier, N4HY
//            November 2005 - February 2006
//
//=================================================================


namespace PowerSDR
{
 
    public partial class CWX : System.Windows.Forms.Form
    {
        

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CWX));
            this.pttLed = new System.Windows.Forms.Panel();
            this.keyLed = new System.Windows.Forms.Panel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.keyboardLed = new System.Windows.Forms.Panel();
            this.labelTS1 = new System.Windows.Forms.LabelTS();
            this.txt10 = new System.Windows.Forms.TextBoxTS();
            this.checkBoxTS1 = new System.Windows.Forms.CheckBoxTS();
            this.checkBoxCWD = new System.Windows.Forms.CheckBoxTS();
            this.chkKeyPoll = new System.Windows.Forms.CheckBoxTS();
            this.udWPM = new System.Windows.Forms.NumericUpDownTS();
            this.pttdelaylabel = new System.Windows.Forms.LabelTS();
            this.udPtt = new System.Windows.Forms.NumericUpDownTS();
            this.expandButton = new System.Windows.Forms.ButtonTS();
            this.keyboardButton = new System.Windows.Forms.ButtonTS();
            this.clearButton = new System.Windows.Forms.ButtonTS();
            this.chkPause = new System.Windows.Forms.CheckBoxTS();
            this.txt9 = new System.Windows.Forms.TextBoxTS();
            this.txt8 = new System.Windows.Forms.TextBoxTS();
            this.txt7 = new System.Windows.Forms.TextBoxTS();
            this.txt6 = new System.Windows.Forms.TextBoxTS();
            this.txt5 = new System.Windows.Forms.TextBoxTS();
            this.txt4 = new System.Windows.Forms.TextBoxTS();
            this.txt3 = new System.Windows.Forms.TextBoxTS();
            this.txt2 = new System.Windows.Forms.TextBoxTS();
            this.txt1 = new System.Windows.Forms.TextBoxTS();
            this.keyButton = new System.Windows.Forms.ButtonTS();
            this.dropdelaylabel = new System.Windows.Forms.LabelTS();
            this.udDrop = new System.Windows.Forms.NumericUpDownTS();
            this.s9 = new System.Windows.Forms.ButtonTS();
            this.s8 = new System.Windows.Forms.ButtonTS();
            this.s7 = new System.Windows.Forms.ButtonTS();
            this.stopButton = new System.Windows.Forms.ButtonTS();
            this.repeatdelayLabel = new System.Windows.Forms.LabelTS();
            this.udDelay = new System.Windows.Forms.NumericUpDownTS();
            this.cbMorse = new System.Windows.Forms.ComboBoxTS();
            this.notesButton = new System.Windows.Forms.ButtonTS();
            this.speedLabel = new System.Windows.Forms.LabelTS();
            this.s6 = new System.Windows.Forms.ButtonTS();
            this.s5 = new System.Windows.Forms.ButtonTS();
            this.s4 = new System.Windows.Forms.ButtonTS();
            this.s3 = new System.Windows.Forms.ButtonTS();
            this.s2 = new System.Windows.Forms.ButtonTS();
            this.s1 = new System.Windows.Forms.ButtonTS();
            this.labelTS2 = new System.Windows.Forms.LabelTS();
            this.chkAlwaysOnTop = new System.Windows.Forms.CheckBoxTS();
            this.txtdummy1 = new System.Windows.Forms.TextBoxTS();
            this.label7 = new System.Windows.Forms.LabelTS();
            this.label6 = new System.Windows.Forms.LabelTS();
            this.label5 = new System.Windows.Forms.LabelTS();
            this.label4 = new System.Windows.Forms.LabelTS();
            ((System.ComponentModel.ISupportInitialize)(this.udWPM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPtt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udDrop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udDelay)).BeginInit();
            this.SuspendLayout();
            // 
            // pttLed
            // 
            this.pttLed.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pttLed.Location = new System.Drawing.Point(10, 8);
            this.pttLed.Name = "pttLed";
            this.pttLed.Size = new System.Drawing.Size(24, 13);
            this.pttLed.TabIndex = 49;
            this.toolTip1.SetToolTip(this.pttLed, " PTT status");
            // 
            // keyLed
            // 
            this.keyLed.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.keyLed.Location = new System.Drawing.Point(10, 24);
            this.keyLed.Name = "keyLed";
            this.keyLed.Size = new System.Drawing.Size(24, 13);
            this.keyLed.TabIndex = 50;
            this.toolTip1.SetToolTip(this.keyLed, "Key status");
            // 
            // keyboardLed
            // 
            this.keyboardLed.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.keyboardLed.Location = new System.Drawing.Point(353, 187);
            this.keyboardLed.Name = "keyboardLed";
            this.keyboardLed.Size = new System.Drawing.Size(24, 13);
            this.keyboardLed.TabIndex = 52;
            this.toolTip1.SetToolTip(this.keyboardLed, " Keyboard active indicator.");
            // 
            // labelTS1
            // 
            this.labelTS1.Image = null;
            this.labelTS1.Location = new System.Drawing.Point(193, 59);
            this.labelTS1.Name = "labelTS1";
            this.labelTS1.Size = new System.Drawing.Size(86, 16);
            this.labelTS1.TabIndex = 61;
            this.labelTS1.Text = "Wild Card Text:";
            this.toolTip1.SetToolTip(this.labelTS1, "WildCard Text goes here.\r\nThen in F1 through F9 use a ~ character \r\nThe ~ will be" +
        " replace with the WildCard text\r\n");
            // 
            // txt10
            // 
            this.txt10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt10.Location = new System.Drawing.Point(281, 56);
            this.txt10.MaxLength = 30;
            this.txt10.Name = "txt10";
            this.txt10.Size = new System.Drawing.Size(194, 21);
            this.txt10.TabIndex = 60;
            this.toolTip1.SetToolTip(this.txt10, "WildCard Text goes here.\r\nThen in F1 through F9 use a ~ character \r\nThe ~ will be" +
        " replace with the WildCard text");
            // 
            // checkBoxTS1
            // 
            this.checkBoxTS1.Image = null;
            this.checkBoxTS1.Location = new System.Drawing.Point(227, 303);
            this.checkBoxTS1.Name = "checkBoxTS1";
            this.checkBoxTS1.Size = new System.Drawing.Size(294, 24);
            this.checkBoxTS1.TabIndex = 59;
            this.checkBoxTS1.Text = "Override Voice Keyer buttons with CWX macro F3-F6\r\n";
            this.toolTip1.SetToolTip(this.checkBoxTS1, resources.GetString("checkBoxTS1.ToolTip"));
            this.checkBoxTS1.CheckedChanged += new System.EventHandler(this.checkBoxTS1_CheckedChanged);
            // 
            // checkBoxCWD
            // 
            this.checkBoxCWD.Image = null;
            this.checkBoxCWD.Location = new System.Drawing.Point(565, 26);
            this.checkBoxCWD.Name = "checkBoxCWD";
            this.checkBoxCWD.Size = new System.Drawing.Size(99, 24);
            this.checkBoxCWD.TabIndex = 58;
            this.checkBoxCWD.Text = "Visual CW\r\n";
            this.toolTip1.SetToolTip(this.checkBoxCWD, "Check Box to Visually display CW in Panadapter and Waterfall\r\n");
            this.checkBoxCWD.CheckedChanged += new System.EventHandler(this.checkBoxCWD_CheckedChanged);
            // 
            // chkKeyPoll
            // 
            this.chkKeyPoll.Image = null;
            this.chkKeyPoll.Location = new System.Drawing.Point(565, 2);
            this.chkKeyPoll.Name = "chkKeyPoll";
            this.chkKeyPoll.Size = new System.Drawing.Size(99, 24);
            this.chkKeyPoll.TabIndex = 2;
            this.chkKeyPoll.Text = "Poll CW Key";
            this.toolTip1.SetToolTip(this.chkKeyPoll, resources.GetString("chkKeyPoll.ToolTip"));
            this.chkKeyPoll.CheckedChanged += new System.EventHandler(this.ckKeyPoll_CheckedChanged);
            // 
            // udWPM
            // 
            this.udWPM.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.udWPM.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udWPM.Location = new System.Drawing.Point(240, 8);
            this.udWPM.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.udWPM.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udWPM.Name = "udWPM";
            this.udWPM.Size = new System.Drawing.Size(56, 20);
            this.udWPM.TabIndex = 56;
            this.toolTip1.SetToolTip(this.udWPM, "Set memory keyer (not paddle) speed in words per minute. (PARIS method)");
            this.udWPM.Value = new decimal(new int[] {
            22,
            0,
            0,
            0});
            this.udWPM.ValueChanged += new System.EventHandler(this.udWPM_ValueChanged);
            this.udWPM.LostFocus += new System.EventHandler(this.udWPM_LostFocus);
            // 
            // pttdelaylabel
            // 
            this.pttdelaylabel.Image = null;
            this.pttdelaylabel.Location = new System.Drawing.Point(456, 32);
            this.pttdelaylabel.Name = "pttdelaylabel";
            this.pttdelaylabel.Size = new System.Drawing.Size(64, 16);
            this.pttdelaylabel.TabIndex = 55;
            this.pttdelaylabel.Text = "PTT Delay";
            this.toolTip1.SetToolTip(this.pttdelaylabel, "Set delay from PTT to key down in milliseconds.");
            // 
            // udPtt
            // 
            this.udPtt.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udPtt.Location = new System.Drawing.Point(456, 8);
            this.udPtt.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.udPtt.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.udPtt.Name = "udPtt";
            this.udPtt.Size = new System.Drawing.Size(56, 20);
            this.udPtt.TabIndex = 54;
            this.toolTip1.SetToolTip(this.udPtt, "Set delay from PTT to key down in milliseconds.");
            this.udPtt.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.udPtt.ValueChanged += new System.EventHandler(this.udPtt_ValueChanged);
            this.udPtt.LostFocus += new System.EventHandler(this.udPtt_LostFocus);
            // 
            // expandButton
            // 
            this.expandButton.BackColor = System.Drawing.Color.RoyalBlue;
            this.expandButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.expandButton.Image = null;
            this.expandButton.Location = new System.Drawing.Point(705, 284);
            this.expandButton.Name = "expandButton";
            this.expandButton.Size = new System.Drawing.Size(8, 8);
            this.expandButton.TabIndex = 53;
            this.toolTip1.SetToolTip(this.expandButton, "Contract Form");
            this.expandButton.UseVisualStyleBackColor = false;
            this.expandButton.Click += new System.EventHandler(this.expandButton_Click);
            // 
            // keyboardButton
            // 
            this.keyboardButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.keyboardButton.Image = null;
            this.keyboardButton.Location = new System.Drawing.Point(225, 182);
            this.keyboardButton.Name = "keyboardButton";
            this.keyboardButton.Size = new System.Drawing.Size(112, 23);
            this.keyboardButton.TabIndex = 45;
            this.keyboardButton.Text = "Keyboard";
            this.toolTip1.SetToolTip(this.keyboardButton, " Enable keyboard.  This must be selected for keyboard to work.");
            this.keyboardButton.Enter += new System.EventHandler(this.keyboardButton_Enter);
            this.keyboardButton.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.keyboardButton_KeyPress);
            this.keyboardButton.Leave += new System.EventHandler(this.keyboardButton_Leave);
            // 
            // clearButton
            // 
            this.clearButton.Image = null;
            this.clearButton.Location = new System.Drawing.Point(129, 182);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(75, 23);
            this.clearButton.TabIndex = 46;
            this.clearButton.Text = "Clear (F12)";
            this.toolTip1.SetToolTip(this.clearButton, " Clear the keyboard buffer.");
            this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
            // 
            // chkPause
            // 
            this.chkPause.Image = null;
            this.chkPause.Location = new System.Drawing.Point(25, 186);
            this.chkPause.Name = "chkPause";
            this.chkPause.Size = new System.Drawing.Size(98, 16);
            this.chkPause.TabIndex = 43;
            this.chkPause.Text = "Pause (F11)";
            this.toolTip1.SetToolTip(this.chkPause, " Pause keyboard transmission.");
            this.chkPause.CheckedChanged += new System.EventHandler(this.chkPause_CheckedChanged);
            // 
            // txt9
            // 
            this.txt9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt9.Location = new System.Drawing.Point(513, 150);
            this.txt9.Name = "txt9";
            this.txt9.Size = new System.Drawing.Size(184, 21);
            this.txt9.TabIndex = 34;
            this.toolTip1.SetToolTip(this.txt9, "Message edit box.");
            // 
            // txt8
            // 
            this.txt8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt8.Location = new System.Drawing.Point(513, 118);
            this.txt8.Name = "txt8";
            this.txt8.Size = new System.Drawing.Size(184, 21);
            this.txt8.TabIndex = 32;
            this.toolTip1.SetToolTip(this.txt8, "Message edit box.");
            // 
            // txt7
            // 
            this.txt7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt7.Location = new System.Drawing.Point(513, 86);
            this.txt7.Name = "txt7";
            this.txt7.Size = new System.Drawing.Size(184, 21);
            this.txt7.TabIndex = 29;
            this.toolTip1.SetToolTip(this.txt7, "Message edit box.");
            // 
            // txt6
            // 
            this.txt6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt6.Location = new System.Drawing.Point(281, 150);
            this.txt6.Name = "txt6";
            this.txt6.Size = new System.Drawing.Size(194, 21);
            this.txt6.TabIndex = 13;
            this.toolTip1.SetToolTip(this.txt6, "Message edit box.");
            // 
            // txt5
            // 
            this.txt5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt5.Location = new System.Drawing.Point(281, 118);
            this.txt5.Name = "txt5";
            this.txt5.Size = new System.Drawing.Size(194, 21);
            this.txt5.TabIndex = 11;
            this.toolTip1.SetToolTip(this.txt5, "Message edit box.");
            // 
            // txt4
            // 
            this.txt4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt4.Location = new System.Drawing.Point(281, 86);
            this.txt4.Name = "txt4";
            this.txt4.Size = new System.Drawing.Size(194, 21);
            this.txt4.TabIndex = 9;
            this.toolTip1.SetToolTip(this.txt4, "Message edit box.");
            // 
            // txt3
            // 
            this.txt3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt3.Location = new System.Drawing.Point(49, 150);
            this.txt3.Name = "txt3";
            this.txt3.Size = new System.Drawing.Size(194, 21);
            this.txt3.TabIndex = 7;
            this.toolTip1.SetToolTip(this.txt3, "Message edit box.");
            // 
            // txt2
            // 
            this.txt2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt2.Location = new System.Drawing.Point(49, 118);
            this.txt2.Name = "txt2";
            this.txt2.Size = new System.Drawing.Size(194, 21);
            this.txt2.TabIndex = 5;
            this.toolTip1.SetToolTip(this.txt2, "Message edit box.");
            // 
            // txt1
            // 
            this.txt1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt1.Location = new System.Drawing.Point(49, 86);
            this.txt1.Name = "txt1";
            this.txt1.Size = new System.Drawing.Size(194, 21);
            this.txt1.TabIndex = 3;
            this.txt1.Text = "cq cq test w5sxd test";
            this.toolTip1.SetToolTip(this.txt1, "Message edit box.");
            // 
            // keyButton
            // 
            this.keyButton.Image = null;
            this.keyButton.Location = new System.Drawing.Point(128, 8);
            this.keyButton.Name = "keyButton";
            this.keyButton.Size = new System.Drawing.Size(40, 24);
            this.keyButton.TabIndex = 37;
            this.keyButton.Text = "Key";
            this.toolTip1.SetToolTip(this.keyButton, "Turn on transmitter and key it. (60 second timeout)");
            this.keyButton.Click += new System.EventHandler(this.keyButton_Click);
            // 
            // dropdelaylabel
            // 
            this.dropdelaylabel.Image = null;
            this.dropdelaylabel.Location = new System.Drawing.Point(384, 32);
            this.dropdelaylabel.Name = "dropdelaylabel";
            this.dropdelaylabel.Size = new System.Drawing.Size(64, 16);
            this.dropdelaylabel.TabIndex = 36;
            this.dropdelaylabel.Text = "Drop Delay";
            this.toolTip1.SetToolTip(this.dropdelaylabel, "Set \"Break In\" PTT drop out  time (mSec). Minimum allowed is PTT Delay * 1.5 \r\nTh" +
        "is only works when the CW main console panel \"Break In\" or \r\nSetup->DSP->Keyer->" +
        "Break In is NOT Enabled");
            // 
            // udDrop
            // 
            this.udDrop.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udDrop.Location = new System.Drawing.Point(384, 8);
            this.udDrop.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.udDrop.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.udDrop.Name = "udDrop";
            this.udDrop.Size = new System.Drawing.Size(56, 20);
            this.udDrop.TabIndex = 35;
            this.toolTip1.SetToolTip(this.udDrop, "Set \"Break In\" PTT drop out  time (mSec). Minimum allowed is PTT Delay * 1.5 \r\nTh" +
        "is only works when the CW main console panel \"Break In\" or \r\nSetup->DSP->Keyer->" +
        "Break In is NOT Enabled\r\n");
            this.udDrop.Value = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.udDrop.ValueChanged += new System.EventHandler(this.udDrop_ValueChanged);
            this.udDrop.LostFocus += new System.EventHandler(this.udDrop_LostFocus);
            // 
            // s9
            // 
            this.s9.Image = null;
            this.s9.Location = new System.Drawing.Point(481, 150);
            this.s9.Name = "s9";
            this.s9.Size = new System.Drawing.Size(27, 20);
            this.s9.TabIndex = 33;
            this.s9.Text = "F9";
            this.toolTip1.SetToolTip(this.s9, "F9 Start message 9.\r\n\r\n");
            this.s9.Click += new System.EventHandler(this.s9_Click);
            this.s9.MouseDown += new System.Windows.Forms.MouseEventHandler(this.s9_MouseDown);
            // 
            // s8
            // 
            this.s8.Image = null;
            this.s8.Location = new System.Drawing.Point(481, 118);
            this.s8.Name = "s8";
            this.s8.Size = new System.Drawing.Size(27, 20);
            this.s8.TabIndex = 31;
            this.s8.Text = "F8";
            this.toolTip1.SetToolTip(this.s8, "F8 Start message 8.\r\n\r\n");
            this.s8.Click += new System.EventHandler(this.s8_Click);
            this.s8.MouseDown += new System.Windows.Forms.MouseEventHandler(this.s8_MouseDown);
            // 
            // s7
            // 
            this.s7.Image = null;
            this.s7.Location = new System.Drawing.Point(481, 86);
            this.s7.Name = "s7";
            this.s7.Size = new System.Drawing.Size(27, 20);
            this.s7.TabIndex = 30;
            this.s7.Text = "F7";
            this.toolTip1.SetToolTip(this.s7, "F7 Start message 7.\r\n\r\n");
            this.s7.Click += new System.EventHandler(this.s7_Click);
            this.s7.MouseDown += new System.Windows.Forms.MouseEventHandler(this.s7_MouseDown);
            // 
            // stopButton
            // 
            this.stopButton.Image = null;
            this.stopButton.Location = new System.Drawing.Point(48, 8);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(72, 24);
            this.stopButton.TabIndex = 26;
            this.stopButton.Text = "Stop (Esc)";
            this.toolTip1.SetToolTip(this.stopButton, "Stop all keying. \r\nAnd also Cancel any Repeat messages (text strings with a \" on " +
        "the end)");
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // repeatdelayLabel
            // 
            this.repeatdelayLabel.Image = null;
            this.repeatdelayLabel.Location = new System.Drawing.Point(304, 32);
            this.repeatdelayLabel.Name = "repeatdelayLabel";
            this.repeatdelayLabel.Size = new System.Drawing.Size(80, 16);
            this.repeatdelayLabel.TabIndex = 48;
            this.repeatdelayLabel.Text = "Repeat Delay";
            this.toolTip1.SetToolTip(this.repeatdelayLabel, "Set repeat message delay in seconds.\r\nBy adding a \" (quotation mark) to the end o" +
        "f a text string\r\nHit STOP to End");
            // 
            // udDelay
            // 
            this.udDelay.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udDelay.Location = new System.Drawing.Point(312, 8);
            this.udDelay.Maximum = new decimal(new int[] {
            3600,
            0,
            0,
            0});
            this.udDelay.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.udDelay.Name = "udDelay";
            this.udDelay.Size = new System.Drawing.Size(56, 20);
            this.udDelay.TabIndex = 20;
            this.toolTip1.SetToolTip(this.udDelay, "Set repeat message delay in seconds.\r\nBy adding a \" (quotation mark) to the end o" +
        "f a text string\r\nHit STOP to End\r\n");
            this.udDelay.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.udDelay.ValueChanged += new System.EventHandler(this.udDelay_ValueChanged);
            this.udDelay.LostFocus += new System.EventHandler(this.udDelay_LostFocus);
            // 
            // cbMorse
            // 
            this.cbMorse.Cursor = System.Windows.Forms.Cursors.Default;
            this.cbMorse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMorse.DropDownWidth = 208;
            this.cbMorse.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbMorse.Location = new System.Drawing.Point(489, 183);
            this.cbMorse.Name = "cbMorse";
            this.cbMorse.Size = new System.Drawing.Size(208, 23);
            this.cbMorse.TabIndex = 19;
            this.toolTip1.SetToolTip(this.cbMorse, " View and right click to edit Morse definition table.");
            this.cbMorse.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cbMorse_MouseDown);
            // 
            // notesButton
            // 
            this.notesButton.Image = null;
            this.notesButton.Location = new System.Drawing.Point(176, 8);
            this.notesButton.Name = "notesButton";
            this.notesButton.Size = new System.Drawing.Size(48, 24);
            this.notesButton.TabIndex = 17;
            this.notesButton.Text = "Help";
            this.toolTip1.SetToolTip(this.notesButton, "Show program notes.");
            this.notesButton.Click += new System.EventHandler(this.notesButton_Click);
            // 
            // speedLabel
            // 
            this.speedLabel.Image = null;
            this.speedLabel.Location = new System.Drawing.Point(232, 32);
            this.speedLabel.Name = "speedLabel";
            this.speedLabel.Size = new System.Drawing.Size(72, 16);
            this.speedLabel.TabIndex = 15;
            this.speedLabel.Text = "Speed WPM";
            this.toolTip1.SetToolTip(this.speedLabel, " Set memory keyer (not paddle) speed in words per minute. (PARIS method)");
            // 
            // s6
            // 
            this.s6.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.s6.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.s6.Image = null;
            this.s6.Location = new System.Drawing.Point(249, 150);
            this.s6.Name = "s6";
            this.s6.Size = new System.Drawing.Size(27, 20);
            this.s6.TabIndex = 14;
            this.s6.Text = "F6";
            this.toolTip1.SetToolTip(this.s6, "F6 Start message 6.\r\n\r\n");
            this.s6.Click += new System.EventHandler(this.s6_Click);
            this.s6.MouseDown += new System.Windows.Forms.MouseEventHandler(this.s6_MouseDown);
            // 
            // s5
            // 
            this.s5.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.s5.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.s5.Image = null;
            this.s5.Location = new System.Drawing.Point(249, 118);
            this.s5.Name = "s5";
            this.s5.Size = new System.Drawing.Size(27, 20);
            this.s5.TabIndex = 12;
            this.s5.Text = "F5";
            this.toolTip1.SetToolTip(this.s5, "F5 Start message 5.\r\n\r\n");
            this.s5.Click += new System.EventHandler(this.s5_Click);
            this.s5.MouseDown += new System.Windows.Forms.MouseEventHandler(this.s5_MouseDown);
            // 
            // s4
            // 
            this.s4.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.s4.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.s4.Image = null;
            this.s4.Location = new System.Drawing.Point(249, 86);
            this.s4.Name = "s4";
            this.s4.Size = new System.Drawing.Size(27, 20);
            this.s4.TabIndex = 10;
            this.s4.Text = "F4";
            this.toolTip1.SetToolTip(this.s4, "F4 Start message 4.\r\n\r\n");
            this.s4.Click += new System.EventHandler(this.s4_Click);
            this.s4.MouseDown += new System.Windows.Forms.MouseEventHandler(this.s4_MouseDown);
            // 
            // s3
            // 
            this.s3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.s3.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.s3.Image = null;
            this.s3.Location = new System.Drawing.Point(14, 150);
            this.s3.Name = "s3";
            this.s3.Size = new System.Drawing.Size(27, 20);
            this.s3.TabIndex = 8;
            this.s3.Text = "F3";
            this.toolTip1.SetToolTip(this.s3, "F3 Start message 3.\r\n\r\n");
            this.s3.Click += new System.EventHandler(this.s3_Click);
            this.s3.MouseDown += new System.Windows.Forms.MouseEventHandler(this.s3_MouseDown);
            // 
            // s2
            // 
            this.s2.Image = null;
            this.s2.Location = new System.Drawing.Point(14, 118);
            this.s2.Name = "s2";
            this.s2.Size = new System.Drawing.Size(27, 20);
            this.s2.TabIndex = 6;
            this.s2.Text = "F2";
            this.toolTip1.SetToolTip(this.s2, "F2 Start message 2.\r\n\r\n");
            this.s2.Click += new System.EventHandler(this.s2_Click);
            this.s2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.s2_MouseDown);
            // 
            // s1
            // 
            this.s1.Image = null;
            this.s1.Location = new System.Drawing.Point(14, 86);
            this.s1.Name = "s1";
            this.s1.Size = new System.Drawing.Size(27, 20);
            this.s1.TabIndex = 4;
            this.s1.Text = "F1";
            this.toolTip1.SetToolTip(this.s1, "F1 Start message 1.\r\n\r\n\r\n");
            this.s1.Click += new System.EventHandler(this.s1_Click);
            this.s1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.s1_MouseDown);
            // 
            // labelTS2
            // 
            this.labelTS2.Image = null;
            this.labelTS2.Location = new System.Drawing.Point(417, 186);
            this.labelTS2.Name = "labelTS2";
            this.labelTS2.Size = new System.Drawing.Size(66, 16);
            this.labelTS2.TabIndex = 62;
            this.labelTS2.Text = "Edit Morse:";
            this.toolTip1.SetToolTip(this.labelTS2, "WildCard Text goes here.\r\nThen in F1 through F9 use a ~ character \r\nThe ~ will be" +
        " replace with the WildCard text\r\n");
            // 
            // chkAlwaysOnTop
            // 
            this.chkAlwaysOnTop.Image = null;
            this.chkAlwaysOnTop.Location = new System.Drawing.Point(574, 308);
            this.chkAlwaysOnTop.Name = "chkAlwaysOnTop";
            this.chkAlwaysOnTop.Size = new System.Drawing.Size(123, 19);
            this.chkAlwaysOnTop.TabIndex = 57;
            this.chkAlwaysOnTop.Text = "Always On Top";
            this.chkAlwaysOnTop.CheckedChanged += new System.EventHandler(this.chkAlwaysOnTop_CheckedChanged);
            // 
            // txtdummy1
            // 
            this.txtdummy1.Location = new System.Drawing.Point(25, 215);
            this.txtdummy1.Multiline = true;
            this.txtdummy1.Name = "txtdummy1";
            this.txtdummy1.Size = new System.Drawing.Size(672, 77);
            this.txtdummy1.TabIndex = 42;
            this.txtdummy1.Text = "the actual text box will be a graphic here and this one disabled";
            // 
            // label7
            // 
            this.label7.Image = null;
            this.label7.Location = new System.Drawing.Point(385, 382);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(256, 32);
            this.label7.TabIndex = 47;
            this.label7.Text = "label7";
            // 
            // label6
            // 
            this.label6.Image = null;
            this.label6.Location = new System.Drawing.Point(65, 382);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(256, 32);
            this.label6.TabIndex = 28;
            this.label6.Text = "label6";
            // 
            // label5
            // 
            this.label5.Image = null;
            this.label5.Location = new System.Drawing.Point(385, 334);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(256, 32);
            this.label5.TabIndex = 27;
            this.label5.Text = "label5";
            // 
            // label4
            // 
            this.label4.Image = null;
            this.label4.Location = new System.Drawing.Point(65, 334);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(256, 32);
            this.label4.TabIndex = 25;
            this.label4.Text = "label4";
            // 
            // CWX
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(725, 328);
            this.Controls.Add(this.labelTS2);
            this.Controls.Add(this.labelTS1);
            this.Controls.Add(this.txt10);
            this.Controls.Add(this.checkBoxTS1);
            this.Controls.Add(this.checkBoxCWD);
            this.Controls.Add(this.chkAlwaysOnTop);
            this.Controls.Add(this.chkKeyPoll);
            this.Controls.Add(this.udWPM);
            this.Controls.Add(this.pttdelaylabel);
            this.Controls.Add(this.udPtt);
            this.Controls.Add(this.expandButton);
            this.Controls.Add(this.keyboardLed);
            this.Controls.Add(this.keyLed);
            this.Controls.Add(this.pttLed);
            this.Controls.Add(this.keyboardButton);
            this.Controls.Add(this.clearButton);
            this.Controls.Add(this.chkPause);
            this.Controls.Add(this.txtdummy1);
            this.Controls.Add(this.txt9);
            this.Controls.Add(this.txt8);
            this.Controls.Add(this.txt7);
            this.Controls.Add(this.txt6);
            this.Controls.Add(this.txt5);
            this.Controls.Add(this.txt4);
            this.Controls.Add(this.txt3);
            this.Controls.Add(this.txt2);
            this.Controls.Add(this.txt1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.keyButton);
            this.Controls.Add(this.dropdelaylabel);
            this.Controls.Add(this.udDrop);
            this.Controls.Add(this.s9);
            this.Controls.Add(this.s8);
            this.Controls.Add(this.s7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.repeatdelayLabel);
            this.Controls.Add(this.udDelay);
            this.Controls.Add(this.cbMorse);
            this.Controls.Add(this.notesButton);
            this.Controls.Add(this.speedLabel);
            this.Controls.Add(this.s6);
            this.Controls.Add(this.s5);
            this.Controls.Add(this.s4);
            this.Controls.Add(this.s3);
            this.Controls.Add(this.s2);
            this.Controls.Add(this.s1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximumSize = new System.Drawing.Size(745, 371);
            this.MinimumSize = new System.Drawing.Size(745, 371);
            this.Name = "CWX";
            this.Text = "   CW Memories and Keyboard ...";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.CWX_Closing);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CWX_FormClosing);
            this.Load += new System.EventHandler(this.CWX_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.CWX_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CWX_KeyDown_1);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.CWX_KeyUp_1);
            this.MouseEnter += new System.EventHandler(this.CWX_MouseEnter);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.CWX_MouseMove);
            ((System.ComponentModel.ISupportInitialize)(this.udWPM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPtt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udDrop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udDelay)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        } //Initialize components


        #endregion
      
        
        private System.ComponentModel.IContainer components;

        private System.Windows.Forms.LabelTS label4;
        private System.Windows.Forms.LabelTS label5;
        private System.Windows.Forms.LabelTS label6;                                // pulling queue pointer
        private System.Windows.Forms.ButtonTS s1;
        private System.Windows.Forms.TextBoxTS txt1;
        private System.Windows.Forms.ButtonTS s2;
        private System.Windows.Forms.TextBoxTS txt2;
        private System.Windows.Forms.ButtonTS s3;
        private System.Windows.Forms.TextBoxTS txt3;
        private System.Windows.Forms.ButtonTS s4;
        private System.Windows.Forms.TextBoxTS txt4;
        private System.Windows.Forms.ButtonTS s5;
        private System.Windows.Forms.TextBoxTS txt5;
        private System.Windows.Forms.ButtonTS s6;
        private System.Windows.Forms.TextBoxTS txt6;
        private System.Windows.Forms.LabelTS speedLabel;
        private System.Windows.Forms.ButtonTS notesButton;
        private System.Windows.Forms.ComboBoxTS cbMorse;
        private System.Windows.Forms.NumericUpDownTS udDelay;
        private System.Windows.Forms.LabelTS repeatdelayLabel;
        private System.Windows.Forms.TextBoxTS txt7;
        private System.Windows.Forms.ButtonTS s7;
        private System.Windows.Forms.ButtonTS s8;
        private System.Windows.Forms.TextBoxTS txt8;
        private System.Windows.Forms.ButtonTS s9;
        private System.Windows.Forms.TextBoxTS txt9;
        private System.Windows.Forms.LabelTS dropdelaylabel;
        private System.Windows.Forms.NumericUpDownTS udDrop;
        private System.Windows.Forms.ButtonTS keyButton;
        private System.Windows.Forms.LabelTS label7;
        private System.Windows.Forms.TextBoxTS txtdummy1;
        private System.Windows.Forms.CheckBoxTS chkPause;
        private System.Windows.Forms.ButtonTS clearButton;
        private System.Windows.Forms.ButtonTS keyboardButton;
        private System.Windows.Forms.Panel pttLed;
        private System.Windows.Forms.Panel keyLed;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Panel keyboardLed;
        private System.Windows.Forms.ButtonTS expandButton;
        private System.Windows.Forms.NumericUpDownTS udPtt;
        private System.Windows.Forms.LabelTS pttdelaylabel;
        public System.Windows.Forms.CheckBoxTS chkAlwaysOnTop;
        public System.Windows.Forms.NumericUpDownTS udWPM;
        private System.Windows.Forms.CheckBoxTS chkKeyPoll;
        public System.Windows.Forms.CheckBoxTS checkBoxCWD;
        public System.Windows.Forms.CheckBoxTS checkBoxTS1;
        public System.Windows.Forms.ButtonTS stopButton;
        private System.Windows.Forms.TextBoxTS txt10;
        private System.Windows.Forms.LabelTS labelTS1;
        private System.Windows.Forms.LabelTS labelTS2;
    } // end class
} // end namespace