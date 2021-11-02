//=================================================================
// fwcmixform.cs
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
    /// Summary description for fwcmixform.
    /// </summary>
    public partial class FWCMixForm : System.Windows.Forms.Form
    {
        

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FWCMixForm));
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.chkLineOutRCASel = new System.Windows.Forms.CheckBoxTS();
            this.tbLineOutRCA = new System.Windows.Forms.TrackBarTS();
            this.chkHeadphoneSel = new System.Windows.Forms.CheckBoxTS();
            this.tbHeadphone = new System.Windows.Forms.TrackBarTS();
            this.chkExtSpkrSel = new System.Windows.Forms.CheckBoxTS();
            this.tbExtSpkr = new System.Windows.Forms.TrackBarTS();
            this.chkOutputMuteAll = new System.Windows.Forms.CheckBoxTS();
            this.chkIntSpkrSel = new System.Windows.Forms.CheckBoxTS();
            this.tbIntSpkr = new System.Windows.Forms.TrackBarTS();
            this.grpInput = new System.Windows.Forms.GroupBoxTS();
            this.chkInputMuteAll = new System.Windows.Forms.CheckBoxTS();
            this.lblLineInDB9 = new System.Windows.Forms.LabelTS();
            this.chkLineInDB9Sel = new System.Windows.Forms.CheckBoxTS();
            this.tbLineInDB9 = new System.Windows.Forms.TrackBarTS();
            this.lblLineInPhono = new System.Windows.Forms.LabelTS();
            this.chkLineInPhonoSel = new System.Windows.Forms.CheckBoxTS();
            this.tbLineInPhono = new System.Windows.Forms.TrackBarTS();
            this.lblLineInRCA = new System.Windows.Forms.LabelTS();
            this.chkLineInRCASel = new System.Windows.Forms.CheckBoxTS();
            this.tbLineInRCA = new System.Windows.Forms.TrackBarTS();
            this.lblMic = new System.Windows.Forms.LabelTS();
            this.chkMicSel = new System.Windows.Forms.CheckBoxTS();
            this.tbMic = new System.Windows.Forms.TrackBarTS();
            this.grpOutput = new System.Windows.Forms.GroupBoxTS();
            this.lblLineOutRCA = new System.Windows.Forms.LabelTS();
            this.lblHeadphone = new System.Windows.Forms.LabelTS();
            this.lblExtSpkr = new System.Windows.Forms.LabelTS();
            this.lblIntSpkr = new System.Windows.Forms.LabelTS();
            ((System.ComponentModel.ISupportInitialize)(this.tbLineOutRCA)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbHeadphone)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbExtSpkr)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbIntSpkr)).BeginInit();
            this.grpInput.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbLineInDB9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbLineInPhono)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbLineInRCA)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbMic)).BeginInit();
            this.grpOutput.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkLineOutRCASel
            // 
            this.chkLineOutRCASel.Checked = true;
            this.chkLineOutRCASel.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkLineOutRCASel.Image = null;
            this.chkLineOutRCASel.Location = new System.Drawing.Point(174, 144);
            this.chkLineOutRCASel.Name = "chkLineOutRCASel";
            this.chkLineOutRCASel.Size = new System.Drawing.Size(16, 24);
            this.chkLineOutRCASel.TabIndex = 21;
            this.toolTip1.SetToolTip(this.chkLineOutRCASel, "Selects/Unselects the Line Out RCA Output");
            this.chkLineOutRCASel.CheckedChanged += new System.EventHandler(this.chkLineOutRCASel_CheckedChanged);
            // 
            // tbLineOutRCA
            // 
            this.tbLineOutRCA.Location = new System.Drawing.Point(160, 48);
            this.tbLineOutRCA.Maximum = 255;
            this.tbLineOutRCA.Minimum = 128;
            this.tbLineOutRCA.Name = "tbLineOutRCA";
            this.tbLineOutRCA.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbLineOutRCA.Size = new System.Drawing.Size(45, 104);
            this.tbLineOutRCA.TabIndex = 20;
            this.tbLineOutRCA.TickFrequency = 16;
            this.tbLineOutRCA.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.toolTip1.SetToolTip(this.tbLineOutRCA, "Adjusts the Line Out RCA Volume");
            this.tbLineOutRCA.Value = 248;
            this.tbLineOutRCA.Scroll += new System.EventHandler(this.tbLineOutRCA_Scroll);
            // 
            // chkHeadphoneSel
            // 
            this.chkHeadphoneSel.Checked = true;
            this.chkHeadphoneSel.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkHeadphoneSel.Image = null;
            this.chkHeadphoneSel.Location = new System.Drawing.Point(126, 144);
            this.chkHeadphoneSel.Name = "chkHeadphoneSel";
            this.chkHeadphoneSel.Size = new System.Drawing.Size(16, 24);
            this.chkHeadphoneSel.TabIndex = 18;
            this.toolTip1.SetToolTip(this.chkHeadphoneSel, "Selects/Unselects the Headphone Output");
            this.chkHeadphoneSel.CheckedChanged += new System.EventHandler(this.chkHeadphoneSel_CheckedChanged);
            // 
            // tbHeadphone
            // 
            this.tbHeadphone.Location = new System.Drawing.Point(112, 48);
            this.tbHeadphone.Maximum = 255;
            this.tbHeadphone.Minimum = 128;
            this.tbHeadphone.Name = "tbHeadphone";
            this.tbHeadphone.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbHeadphone.Size = new System.Drawing.Size(45, 104);
            this.tbHeadphone.TabIndex = 17;
            this.tbHeadphone.TickFrequency = 16;
            this.tbHeadphone.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.toolTip1.SetToolTip(this.tbHeadphone, "Adjusts the Headphone Volume");
            this.tbHeadphone.Value = 248;
            this.tbHeadphone.Scroll += new System.EventHandler(this.tbHeadphone_Scroll);
            // 
            // chkExtSpkrSel
            // 
            this.chkExtSpkrSel.Checked = true;
            this.chkExtSpkrSel.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkExtSpkrSel.Image = null;
            this.chkExtSpkrSel.Location = new System.Drawing.Point(78, 144);
            this.chkExtSpkrSel.Name = "chkExtSpkrSel";
            this.chkExtSpkrSel.Size = new System.Drawing.Size(16, 24);
            this.chkExtSpkrSel.TabIndex = 15;
            this.toolTip1.SetToolTip(this.chkExtSpkrSel, "Selects/Unselects the External Speaker Output");
            this.chkExtSpkrSel.CheckedChanged += new System.EventHandler(this.chkExtSpkrSel_CheckedChanged);
            // 
            // tbExtSpkr
            // 
            this.tbExtSpkr.Location = new System.Drawing.Point(64, 48);
            this.tbExtSpkr.Maximum = 255;
            this.tbExtSpkr.Minimum = 128;
            this.tbExtSpkr.Name = "tbExtSpkr";
            this.tbExtSpkr.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbExtSpkr.Size = new System.Drawing.Size(45, 104);
            this.tbExtSpkr.TabIndex = 14;
            this.tbExtSpkr.TickFrequency = 16;
            this.tbExtSpkr.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.toolTip1.SetToolTip(this.tbExtSpkr, "Adjusts the External Speaker Volume");
            this.tbExtSpkr.Value = 248;
            this.tbExtSpkr.Scroll += new System.EventHandler(this.tbExtSpkr_Scroll);
            // 
            // chkOutputMuteAll
            // 
            this.chkOutputMuteAll.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkOutputMuteAll.Image = null;
            this.chkOutputMuteAll.Location = new System.Drawing.Point(16, 176);
            this.chkOutputMuteAll.Name = "chkOutputMuteAll";
            this.chkOutputMuteAll.Size = new System.Drawing.Size(184, 24);
            this.chkOutputMuteAll.TabIndex = 13;
            this.chkOutputMuteAll.Text = "Mute All Outputs";
            this.chkOutputMuteAll.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.chkOutputMuteAll, "Mute All Output Lines");
            this.chkOutputMuteAll.CheckedChanged += new System.EventHandler(this.chkOutputMuteAll_CheckedChanged);
            // 
            // chkIntSpkrSel
            // 
            this.chkIntSpkrSel.Checked = true;
            this.chkIntSpkrSel.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIntSpkrSel.Image = null;
            this.chkIntSpkrSel.Location = new System.Drawing.Point(31, 144);
            this.chkIntSpkrSel.Name = "chkIntSpkrSel";
            this.chkIntSpkrSel.Size = new System.Drawing.Size(16, 24);
            this.chkIntSpkrSel.TabIndex = 1;
            this.toolTip1.SetToolTip(this.chkIntSpkrSel, "Selects/Unselects the Internal Speaker Output");
            this.chkIntSpkrSel.CheckedChanged += new System.EventHandler(this.chkIntSpkrSel_CheckedChanged);
            // 
            // tbIntSpkr
            // 
            this.tbIntSpkr.Location = new System.Drawing.Point(16, 48);
            this.tbIntSpkr.Maximum = 255;
            this.tbIntSpkr.Minimum = 128;
            this.tbIntSpkr.Name = "tbIntSpkr";
            this.tbIntSpkr.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbIntSpkr.Size = new System.Drawing.Size(45, 104);
            this.tbIntSpkr.TabIndex = 0;
            this.tbIntSpkr.TickFrequency = 16;
            this.tbIntSpkr.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.toolTip1.SetToolTip(this.tbIntSpkr, "Adjusts the Internal Speaker  Volume");
            this.tbIntSpkr.Value = 230;
            this.tbIntSpkr.Scroll += new System.EventHandler(this.tbIntSpkr_Scroll);
            // 
            // grpInput
            // 
            this.grpInput.Controls.Add(this.chkInputMuteAll);
            this.grpInput.Controls.Add(this.lblLineInDB9);
            this.grpInput.Controls.Add(this.chkLineInDB9Sel);
            this.grpInput.Controls.Add(this.tbLineInDB9);
            this.grpInput.Controls.Add(this.lblLineInPhono);
            this.grpInput.Controls.Add(this.chkLineInPhonoSel);
            this.grpInput.Controls.Add(this.tbLineInPhono);
            this.grpInput.Controls.Add(this.lblLineInRCA);
            this.grpInput.Controls.Add(this.chkLineInRCASel);
            this.grpInput.Controls.Add(this.tbLineInRCA);
            this.grpInput.Controls.Add(this.lblMic);
            this.grpInput.Controls.Add(this.chkMicSel);
            this.grpInput.Controls.Add(this.tbMic);
            this.grpInput.Location = new System.Drawing.Point(8, 8);
            this.grpInput.Name = "grpInput";
            this.grpInput.Size = new System.Drawing.Size(216, 216);
            this.grpInput.TabIndex = 2;
            this.grpInput.TabStop = false;
            this.grpInput.Text = "Input (Stage 1)";
            this.toolTip1.SetToolTip(this.grpInput, resources.GetString("grpInput.ToolTip"));
            // 
            // chkInputMuteAll
            // 
            this.chkInputMuteAll.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkInputMuteAll.Image = null;
            this.chkInputMuteAll.Location = new System.Drawing.Point(16, 176);
            this.chkInputMuteAll.Name = "chkInputMuteAll";
            this.chkInputMuteAll.Size = new System.Drawing.Size(184, 24);
            this.chkInputMuteAll.TabIndex = 12;
            this.chkInputMuteAll.Text = "Mute All Inputs";
            this.chkInputMuteAll.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.chkInputMuteAll, "Mute All Input Lines");
            this.chkInputMuteAll.CheckedChanged += new System.EventHandler(this.chkInputMuteAll_CheckedChanged);
            // 
            // lblLineInDB9
            // 
            this.lblLineInDB9.Image = null;
            this.lblLineInDB9.Location = new System.Drawing.Point(157, 24);
            this.lblLineInDB9.Name = "lblLineInDB9";
            this.lblLineInDB9.Size = new System.Drawing.Size(49, 24);
            this.lblLineInDB9.TabIndex = 11;
            this.lblLineInDB9.Text = "FlexWire In";
            this.lblLineInDB9.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.toolTip1.SetToolTip(this.lblLineInDB9, resources.GetString("lblLineInDB9.ToolTip"));
            // 
            // chkLineInDB9Sel
            // 
            this.chkLineInDB9Sel.Image = null;
            this.chkLineInDB9Sel.Location = new System.Drawing.Point(174, 144);
            this.chkLineInDB9Sel.Name = "chkLineInDB9Sel";
            this.chkLineInDB9Sel.Size = new System.Drawing.Size(16, 24);
            this.chkLineInDB9Sel.TabIndex = 10;
            this.toolTip1.SetToolTip(this.chkLineInDB9Sel, "Selects/Unselects the Line In DB9 Input");
            this.chkLineInDB9Sel.CheckedChanged += new System.EventHandler(this.chkLineInDB9Sel_CheckedChanged);
            // 
            // tbLineInDB9
            // 
            this.tbLineInDB9.LargeChange = 1;
            this.tbLineInDB9.Location = new System.Drawing.Point(160, 48);
            this.tbLineInDB9.Maximum = 0;
            this.tbLineInDB9.Minimum = -128;
            this.tbLineInDB9.Name = "tbLineInDB9";
            this.tbLineInDB9.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbLineInDB9.Size = new System.Drawing.Size(45, 104);
            this.tbLineInDB9.TabIndex = 9;
            this.tbLineInDB9.TickFrequency = 16;
            this.tbLineInDB9.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.toolTip1.SetToolTip(this.tbLineInDB9, resources.GetString("tbLineInDB9.ToolTip"));
            this.tbLineInDB9.Value = -33;
            this.tbLineInDB9.Scroll += new System.EventHandler(this.tbLineInDB9_Scroll);
            this.tbLineInDB9.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tbLineInDB9_MouseDown);
            this.tbLineInDB9.MouseEnter += new System.EventHandler(this.tbLineInDB9_MouseEnter);
            this.tbLineInDB9.MouseHover += new System.EventHandler(this.tbLineInDB9_MouseHover);
            // 
            // lblLineInPhono
            // 
            this.lblLineInPhono.Image = null;
            this.lblLineInPhono.Location = new System.Drawing.Point(112, 24);
            this.lblLineInPhono.Name = "lblLineInPhono";
            this.lblLineInPhono.Size = new System.Drawing.Size(45, 24);
            this.lblLineInPhono.TabIndex = 8;
            this.lblLineInPhono.Text = "Bal Line In";
            this.lblLineInPhono.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.toolTip1.SetToolTip(this.lblLineInPhono, resources.GetString("lblLineInPhono.ToolTip"));
            // 
            // chkLineInPhonoSel
            // 
            this.chkLineInPhonoSel.Image = null;
            this.chkLineInPhonoSel.Location = new System.Drawing.Point(126, 144);
            this.chkLineInPhonoSel.Name = "chkLineInPhonoSel";
            this.chkLineInPhonoSel.Size = new System.Drawing.Size(16, 24);
            this.chkLineInPhonoSel.TabIndex = 7;
            this.toolTip1.SetToolTip(this.chkLineInPhonoSel, "Selects/Unselects the Line In Phono Input");
            this.chkLineInPhonoSel.CheckedChanged += new System.EventHandler(this.chkLineInPhonoSel_CheckedChanged);
            // 
            // tbLineInPhono
            // 
            this.tbLineInPhono.LargeChange = 1;
            this.tbLineInPhono.Location = new System.Drawing.Point(112, 48);
            this.tbLineInPhono.Maximum = 0;
            this.tbLineInPhono.Minimum = -128;
            this.tbLineInPhono.Name = "tbLineInPhono";
            this.tbLineInPhono.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbLineInPhono.Size = new System.Drawing.Size(45, 104);
            this.tbLineInPhono.TabIndex = 6;
            this.tbLineInPhono.TickFrequency = 16;
            this.tbLineInPhono.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.toolTip1.SetToolTip(this.tbLineInPhono, resources.GetString("tbLineInPhono.ToolTip"));
            this.tbLineInPhono.Value = -33;
            this.tbLineInPhono.Scroll += new System.EventHandler(this.tbLineInPhono_Scroll);
            this.tbLineInPhono.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tbLineInPhono_MouseHover);
            this.tbLineInPhono.MouseEnter += new System.EventHandler(this.tbLineInPhono_MouseHover);
            this.tbLineInPhono.MouseHover += new System.EventHandler(this.tbLineInPhono_MouseHover);
            // 
            // lblLineInRCA
            // 
            this.lblLineInRCA.Image = null;
            this.lblLineInRCA.Location = new System.Drawing.Point(64, 24);
            this.lblLineInRCA.Name = "lblLineInRCA";
            this.lblLineInRCA.Size = new System.Drawing.Size(45, 24);
            this.lblLineInRCA.TabIndex = 5;
            this.lblLineInRCA.Text = "Line In RCA";
            this.lblLineInRCA.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.toolTip1.SetToolTip(this.lblLineInRCA, resources.GetString("lblLineInRCA.ToolTip"));
            // 
            // chkLineInRCASel
            // 
            this.chkLineInRCASel.Image = null;
            this.chkLineInRCASel.Location = new System.Drawing.Point(78, 144);
            this.chkLineInRCASel.Name = "chkLineInRCASel";
            this.chkLineInRCASel.Size = new System.Drawing.Size(16, 24);
            this.chkLineInRCASel.TabIndex = 4;
            this.toolTip1.SetToolTip(this.chkLineInRCASel, "Selects/Unselects the Line In RCA Input");
            this.chkLineInRCASel.CheckedChanged += new System.EventHandler(this.chkLineInRCASel_CheckedChanged);
            // 
            // tbLineInRCA
            // 
            this.tbLineInRCA.LargeChange = 1;
            this.tbLineInRCA.Location = new System.Drawing.Point(64, 48);
            this.tbLineInRCA.Maximum = 0;
            this.tbLineInRCA.Minimum = -128;
            this.tbLineInRCA.Name = "tbLineInRCA";
            this.tbLineInRCA.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbLineInRCA.Size = new System.Drawing.Size(45, 104);
            this.tbLineInRCA.TabIndex = 3;
            this.tbLineInRCA.TickFrequency = 16;
            this.tbLineInRCA.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.toolTip1.SetToolTip(this.tbLineInRCA, resources.GetString("tbLineInRCA.ToolTip"));
            this.tbLineInRCA.Value = -33;
            this.tbLineInRCA.Scroll += new System.EventHandler(this.tbLineInRCA_Scroll);
            this.tbLineInRCA.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tbLineInRCA_MouseDown);
            this.tbLineInRCA.MouseEnter += new System.EventHandler(this.tbLineInRCA_MouseEnter);
            this.tbLineInRCA.MouseHover += new System.EventHandler(this.tbLineInRCA_MouseHover);
            // 
            // lblMic
            // 
            this.lblMic.Image = null;
            this.lblMic.Location = new System.Drawing.Point(16, 24);
            this.lblMic.Name = "lblMic";
            this.lblMic.Size = new System.Drawing.Size(45, 24);
            this.lblMic.TabIndex = 2;
            this.lblMic.Text = "Mic";
            this.lblMic.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.toolTip1.SetToolTip(this.lblMic, resources.GetString("lblMic.ToolTip"));
            // 
            // chkMicSel
            // 
            this.chkMicSel.Checked = true;
            this.chkMicSel.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMicSel.Image = null;
            this.chkMicSel.Location = new System.Drawing.Point(30, 144);
            this.chkMicSel.Name = "chkMicSel";
            this.chkMicSel.Size = new System.Drawing.Size(16, 24);
            this.chkMicSel.TabIndex = 1;
            this.toolTip1.SetToolTip(this.chkMicSel, "Selects/Unselects the Mic Input");
            this.chkMicSel.CheckedChanged += new System.EventHandler(this.chkMicSel_CheckedChanged);
            // 
            // tbMic
            // 
            this.tbMic.LargeChange = 1;
            this.tbMic.Location = new System.Drawing.Point(16, 48);
            this.tbMic.Maximum = 0;
            this.tbMic.Minimum = -128;
            this.tbMic.Name = "tbMic";
            this.tbMic.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbMic.Size = new System.Drawing.Size(45, 104);
            this.tbMic.TabIndex = 0;
            this.tbMic.TickFrequency = 16;
            this.tbMic.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.toolTip1.SetToolTip(this.tbMic, resources.GetString("tbMic.ToolTip"));
            this.tbMic.Value = -33;
            this.tbMic.Scroll += new System.EventHandler(this.tbMic_Scroll);
            this.tbMic.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tbMic_MouseDown);
            this.tbMic.MouseEnter += new System.EventHandler(this.tbMic_MouseEnter);
            this.tbMic.MouseHover += new System.EventHandler(this.tbMic_MouseHover);
            // 
            // grpOutput
            // 
            this.grpOutput.Controls.Add(this.lblLineOutRCA);
            this.grpOutput.Controls.Add(this.chkLineOutRCASel);
            this.grpOutput.Controls.Add(this.tbLineOutRCA);
            this.grpOutput.Controls.Add(this.lblHeadphone);
            this.grpOutput.Controls.Add(this.chkHeadphoneSel);
            this.grpOutput.Controls.Add(this.tbHeadphone);
            this.grpOutput.Controls.Add(this.lblExtSpkr);
            this.grpOutput.Controls.Add(this.chkExtSpkrSel);
            this.grpOutput.Controls.Add(this.tbExtSpkr);
            this.grpOutput.Controls.Add(this.chkOutputMuteAll);
            this.grpOutput.Controls.Add(this.lblIntSpkr);
            this.grpOutput.Controls.Add(this.chkIntSpkrSel);
            this.grpOutput.Controls.Add(this.tbIntSpkr);
            this.grpOutput.Location = new System.Drawing.Point(232, 8);
            this.grpOutput.Name = "grpOutput";
            this.grpOutput.Size = new System.Drawing.Size(216, 216);
            this.grpOutput.TabIndex = 3;
            this.grpOutput.TabStop = false;
            this.grpOutput.Text = "Output";
            // 
            // lblLineOutRCA
            // 
            this.lblLineOutRCA.Image = null;
            this.lblLineOutRCA.Location = new System.Drawing.Point(158, 24);
            this.lblLineOutRCA.Name = "lblLineOutRCA";
            this.lblLineOutRCA.Size = new System.Drawing.Size(48, 32);
            this.lblLineOutRCA.TabIndex = 22;
            this.lblLineOutRCA.Text = "Line Out RCA";
            this.lblLineOutRCA.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblHeadphone
            // 
            this.lblHeadphone.Image = null;
            this.lblHeadphone.Location = new System.Drawing.Point(110, 24);
            this.lblHeadphone.Name = "lblHeadphone";
            this.lblHeadphone.Size = new System.Drawing.Size(48, 32);
            this.lblHeadphone.TabIndex = 19;
            this.lblHeadphone.Text = "Head Phones";
            this.lblHeadphone.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblExtSpkr
            // 
            this.lblExtSpkr.Image = null;
            this.lblExtSpkr.Location = new System.Drawing.Point(58, 24);
            this.lblExtSpkr.Name = "lblExtSpkr";
            this.lblExtSpkr.Size = new System.Drawing.Size(56, 32);
            this.lblExtSpkr.TabIndex = 16;
            this.lblExtSpkr.Text = "Pow Spkr Line Out";
            this.lblExtSpkr.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblIntSpkr
            // 
            this.lblIntSpkr.Image = null;
            this.lblIntSpkr.Location = new System.Drawing.Point(14, 24);
            this.lblIntSpkr.Name = "lblIntSpkr";
            this.lblIntSpkr.Size = new System.Drawing.Size(48, 32);
            this.lblIntSpkr.TabIndex = 2;
            this.lblIntSpkr.Text = "Internal Speaker";
            this.lblIntSpkr.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // FWCMixForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(456, 230);
            this.Controls.Add(this.grpOutput);
            this.Controls.Add(this.grpInput);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(472, 269);
            this.MinimumSize = new System.Drawing.Size(472, 269);
            this.Name = "FWCMixForm";
            this.Text = "FLEX-5000 Audio Mixer";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.FWCMixForm_Closing);
            ((System.ComponentModel.ISupportInitialize)(this.tbLineOutRCA)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbHeadphone)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbExtSpkr)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbIntSpkr)).EndInit();
            this.grpInput.ResumeLayout(false);
            this.grpInput.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbLineInDB9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbLineInPhono)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbLineInRCA)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbMic)).EndInit();
            this.grpOutput.ResumeLayout(false);
            this.grpOutput.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

    
        private System.Windows.Forms.TrackBarTS tbMic;
        private System.Windows.Forms.GroupBoxTS grpInput;
        private System.Windows.Forms.LabelTS lblMic;
        private System.Windows.Forms.GroupBoxTS grpOutput;
        private System.Windows.Forms.LabelTS lblIntSpkr;
        private System.Windows.Forms.CheckBoxTS chkInputMuteAll;
        private System.Windows.Forms.CheckBoxTS chkMicSel;
        private System.Windows.Forms.LabelTS lblLineInDB9;
        private System.Windows.Forms.CheckBoxTS chkLineInDB9Sel;
        private System.Windows.Forms.TrackBarTS tbLineInDB9;
        private System.Windows.Forms.LabelTS lblLineInPhono;
        private System.Windows.Forms.CheckBoxTS chkLineInPhonoSel;
        private System.Windows.Forms.LabelTS lblLineInRCA;
        private System.Windows.Forms.CheckBoxTS chkLineInRCASel;
        private System.Windows.Forms.TrackBarTS tbLineInRCA;
        private System.Windows.Forms.CheckBoxTS chkIntSpkrSel;
        public System.Windows.Forms.TrackBarTS tbIntSpkr;
        private System.Windows.Forms.CheckBoxTS chkOutputMuteAll;
        private System.Windows.Forms.LabelTS lblExtSpkr;
        public System.Windows.Forms.CheckBoxTS chkExtSpkrSel;
        private System.Windows.Forms.TrackBarTS tbExtSpkr;
        private System.Windows.Forms.LabelTS lblLineOutRCA;
        private System.Windows.Forms.TrackBarTS tbLineOutRCA;
        private System.Windows.Forms.LabelTS lblHeadphone;
        public System.Windows.Forms.CheckBoxTS chkHeadphoneSel;
        private System.Windows.Forms.TrackBarTS tbHeadphone;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TrackBarTS tbLineInPhono;
        public System.Windows.Forms.CheckBoxTS chkLineOutRCASel;
        private System.ComponentModel.IContainer components;

        
    }
}
