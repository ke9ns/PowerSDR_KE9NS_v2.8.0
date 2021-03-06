//=================================================================
// ucbform.cs
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
    /// Summary description for UCBForm.
    /// </summary>
    public partial class UCBForm : System.Windows.Forms.Form
    {
       
        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCBForm));
            this.btnEnable = new System.Windows.Forms.ButtonTS();
            this.btnDisable = new System.Windows.Forms.ButtonTS();
            this.btnDisableClear = new System.Windows.Forms.ButtonTS();
            this.chkL00R01 = new System.Windows.Forms.CheckBoxTS();
            this.chkL00R02 = new System.Windows.Forms.CheckBoxTS();
            this.chkL00R03 = new System.Windows.Forms.CheckBoxTS();
            this.chkL00R04 = new System.Windows.Forms.CheckBoxTS();
            this.chkL00R05 = new System.Windows.Forms.CheckBoxTS();
            this.chkL00R06 = new System.Windows.Forms.CheckBoxTS();
            this.chkL00R07 = new System.Windows.Forms.CheckBoxTS();
            this.chkL00R08 = new System.Windows.Forms.CheckBoxTS();
            this.chkL00R09 = new System.Windows.Forms.CheckBoxTS();
            this.chkL00R10 = new System.Windows.Forms.CheckBoxTS();
            this.chkL00R11 = new System.Windows.Forms.CheckBoxTS();
            this.chkL00R12 = new System.Windows.Forms.CheckBoxTS();
            this.chkL00R13 = new System.Windows.Forms.CheckBoxTS();
            this.chkL00R14 = new System.Windows.Forms.CheckBoxTS();
            this.chkL00R15 = new System.Windows.Forms.CheckBoxTS();
            this.chkL00R16 = new System.Windows.Forms.CheckBoxTS();
            this.lblR1 = new System.Windows.Forms.LabelTS();
            this.lblR2 = new System.Windows.Forms.LabelTS();
            this.lblR3 = new System.Windows.Forms.LabelTS();
            this.lblR4 = new System.Windows.Forms.LabelTS();
            this.lblR5 = new System.Windows.Forms.LabelTS();
            this.lblR6 = new System.Windows.Forms.LabelTS();
            this.lblR7 = new System.Windows.Forms.LabelTS();
            this.lblR8 = new System.Windows.Forms.LabelTS();
            this.lblR9 = new System.Windows.Forms.LabelTS();
            this.lblR10 = new System.Windows.Forms.LabelTS();
            this.lblR11 = new System.Windows.Forms.LabelTS();
            this.lblR12 = new System.Windows.Forms.LabelTS();
            this.lblR13 = new System.Windows.Forms.LabelTS();
            this.lblR14 = new System.Windows.Forms.LabelTS();
            this.lblR15 = new System.Windows.Forms.LabelTS();
            this.lblR16 = new System.Windows.Forms.LabelTS();
            this.chkL01R09 = new System.Windows.Forms.CheckBoxTS();
            this.chkL01R10 = new System.Windows.Forms.CheckBoxTS();
            this.chkL01R11 = new System.Windows.Forms.CheckBoxTS();
            this.chkL01R12 = new System.Windows.Forms.CheckBoxTS();
            this.chkL01R13 = new System.Windows.Forms.CheckBoxTS();
            this.chkL01R14 = new System.Windows.Forms.CheckBoxTS();
            this.chkL01R15 = new System.Windows.Forms.CheckBoxTS();
            this.chkL01R16 = new System.Windows.Forms.CheckBoxTS();
            this.chkL01R08 = new System.Windows.Forms.CheckBoxTS();
            this.chkL01R07 = new System.Windows.Forms.CheckBoxTS();
            this.chkL01R06 = new System.Windows.Forms.CheckBoxTS();
            this.chkL01R05 = new System.Windows.Forms.CheckBoxTS();
            this.chkL01R04 = new System.Windows.Forms.CheckBoxTS();
            this.chkL01R03 = new System.Windows.Forms.CheckBoxTS();
            this.chkL01R02 = new System.Windows.Forms.CheckBoxTS();
            this.chkL01R01 = new System.Windows.Forms.CheckBoxTS();
            this.chkL02R09 = new System.Windows.Forms.CheckBoxTS();
            this.chkL02R10 = new System.Windows.Forms.CheckBoxTS();
            this.chkL02R11 = new System.Windows.Forms.CheckBoxTS();
            this.chkL02R12 = new System.Windows.Forms.CheckBoxTS();
            this.chkL02R13 = new System.Windows.Forms.CheckBoxTS();
            this.chkL02R14 = new System.Windows.Forms.CheckBoxTS();
            this.chkL02R15 = new System.Windows.Forms.CheckBoxTS();
            this.chkL02R16 = new System.Windows.Forms.CheckBoxTS();
            this.chkL02R08 = new System.Windows.Forms.CheckBoxTS();
            this.chkL02R07 = new System.Windows.Forms.CheckBoxTS();
            this.chkL02R06 = new System.Windows.Forms.CheckBoxTS();
            this.chkL02R05 = new System.Windows.Forms.CheckBoxTS();
            this.chkL02R04 = new System.Windows.Forms.CheckBoxTS();
            this.chkL02R03 = new System.Windows.Forms.CheckBoxTS();
            this.chkL02R02 = new System.Windows.Forms.CheckBoxTS();
            this.chkL02R01 = new System.Windows.Forms.CheckBoxTS();
            this.chkL03R09 = new System.Windows.Forms.CheckBoxTS();
            this.chkL03R10 = new System.Windows.Forms.CheckBoxTS();
            this.chkL03R11 = new System.Windows.Forms.CheckBoxTS();
            this.chkL03R12 = new System.Windows.Forms.CheckBoxTS();
            this.chkL03R13 = new System.Windows.Forms.CheckBoxTS();
            this.chkL03R14 = new System.Windows.Forms.CheckBoxTS();
            this.chkL03R15 = new System.Windows.Forms.CheckBoxTS();
            this.chkL03R16 = new System.Windows.Forms.CheckBoxTS();
            this.chkL03R08 = new System.Windows.Forms.CheckBoxTS();
            this.chkL03R07 = new System.Windows.Forms.CheckBoxTS();
            this.chkL03R06 = new System.Windows.Forms.CheckBoxTS();
            this.chkL03R05 = new System.Windows.Forms.CheckBoxTS();
            this.chkL03R04 = new System.Windows.Forms.CheckBoxTS();
            this.chkL03R03 = new System.Windows.Forms.CheckBoxTS();
            this.chkL03R02 = new System.Windows.Forms.CheckBoxTS();
            this.chkL03R01 = new System.Windows.Forms.CheckBoxTS();
            this.chkL04R09 = new System.Windows.Forms.CheckBoxTS();
            this.chkL04R10 = new System.Windows.Forms.CheckBoxTS();
            this.chkL04R11 = new System.Windows.Forms.CheckBoxTS();
            this.chkL04R12 = new System.Windows.Forms.CheckBoxTS();
            this.chkL04R13 = new System.Windows.Forms.CheckBoxTS();
            this.chkL04R14 = new System.Windows.Forms.CheckBoxTS();
            this.chkL04R15 = new System.Windows.Forms.CheckBoxTS();
            this.chkL04R16 = new System.Windows.Forms.CheckBoxTS();
            this.chkL04R08 = new System.Windows.Forms.CheckBoxTS();
            this.chkL04R07 = new System.Windows.Forms.CheckBoxTS();
            this.chkL04R06 = new System.Windows.Forms.CheckBoxTS();
            this.chkL04R05 = new System.Windows.Forms.CheckBoxTS();
            this.chkL04R04 = new System.Windows.Forms.CheckBoxTS();
            this.chkL04R03 = new System.Windows.Forms.CheckBoxTS();
            this.chkL04R02 = new System.Windows.Forms.CheckBoxTS();
            this.chkL04R01 = new System.Windows.Forms.CheckBoxTS();
            this.chkL05R09 = new System.Windows.Forms.CheckBoxTS();
            this.chkL05R10 = new System.Windows.Forms.CheckBoxTS();
            this.chkL05R11 = new System.Windows.Forms.CheckBoxTS();
            this.chkL05R12 = new System.Windows.Forms.CheckBoxTS();
            this.chkL05R13 = new System.Windows.Forms.CheckBoxTS();
            this.chkL05R14 = new System.Windows.Forms.CheckBoxTS();
            this.chkL05R15 = new System.Windows.Forms.CheckBoxTS();
            this.chkL05R16 = new System.Windows.Forms.CheckBoxTS();
            this.chkL05R08 = new System.Windows.Forms.CheckBoxTS();
            this.chkL05R07 = new System.Windows.Forms.CheckBoxTS();
            this.chkL05R06 = new System.Windows.Forms.CheckBoxTS();
            this.chkL05R05 = new System.Windows.Forms.CheckBoxTS();
            this.chkL05R04 = new System.Windows.Forms.CheckBoxTS();
            this.chkL05R03 = new System.Windows.Forms.CheckBoxTS();
            this.chkL05R02 = new System.Windows.Forms.CheckBoxTS();
            this.chkL05R01 = new System.Windows.Forms.CheckBoxTS();
            this.chkL06R09 = new System.Windows.Forms.CheckBoxTS();
            this.chkL06R10 = new System.Windows.Forms.CheckBoxTS();
            this.chkL06R11 = new System.Windows.Forms.CheckBoxTS();
            this.chkL06R12 = new System.Windows.Forms.CheckBoxTS();
            this.chkL06R13 = new System.Windows.Forms.CheckBoxTS();
            this.chkL06R14 = new System.Windows.Forms.CheckBoxTS();
            this.chkL06R15 = new System.Windows.Forms.CheckBoxTS();
            this.chkL06R16 = new System.Windows.Forms.CheckBoxTS();
            this.chkL06R08 = new System.Windows.Forms.CheckBoxTS();
            this.chkL06R07 = new System.Windows.Forms.CheckBoxTS();
            this.chkL06R06 = new System.Windows.Forms.CheckBoxTS();
            this.chkL06R05 = new System.Windows.Forms.CheckBoxTS();
            this.chkL06R04 = new System.Windows.Forms.CheckBoxTS();
            this.chkL06R03 = new System.Windows.Forms.CheckBoxTS();
            this.chkL06R02 = new System.Windows.Forms.CheckBoxTS();
            this.chkL06R01 = new System.Windows.Forms.CheckBoxTS();
            this.chkL07R09 = new System.Windows.Forms.CheckBoxTS();
            this.chkL07R10 = new System.Windows.Forms.CheckBoxTS();
            this.chkL07R11 = new System.Windows.Forms.CheckBoxTS();
            this.chkL07R12 = new System.Windows.Forms.CheckBoxTS();
            this.chkL07R13 = new System.Windows.Forms.CheckBoxTS();
            this.chkL07R14 = new System.Windows.Forms.CheckBoxTS();
            this.chkL07R15 = new System.Windows.Forms.CheckBoxTS();
            this.chkL07R16 = new System.Windows.Forms.CheckBoxTS();
            this.chkL07R08 = new System.Windows.Forms.CheckBoxTS();
            this.chkL07R07 = new System.Windows.Forms.CheckBoxTS();
            this.chkL07R06 = new System.Windows.Forms.CheckBoxTS();
            this.chkL07R05 = new System.Windows.Forms.CheckBoxTS();
            this.chkL07R04 = new System.Windows.Forms.CheckBoxTS();
            this.chkL07R03 = new System.Windows.Forms.CheckBoxTS();
            this.chkL07R02 = new System.Windows.Forms.CheckBoxTS();
            this.chkL07R01 = new System.Windows.Forms.CheckBoxTS();
            this.chkL08R09 = new System.Windows.Forms.CheckBoxTS();
            this.chkL08R10 = new System.Windows.Forms.CheckBoxTS();
            this.chkL08R11 = new System.Windows.Forms.CheckBoxTS();
            this.chkL08R12 = new System.Windows.Forms.CheckBoxTS();
            this.chkL08R13 = new System.Windows.Forms.CheckBoxTS();
            this.chkL08R14 = new System.Windows.Forms.CheckBoxTS();
            this.chkL08R15 = new System.Windows.Forms.CheckBoxTS();
            this.chkL08R16 = new System.Windows.Forms.CheckBoxTS();
            this.chkL08R08 = new System.Windows.Forms.CheckBoxTS();
            this.chkL08R07 = new System.Windows.Forms.CheckBoxTS();
            this.chkL08R06 = new System.Windows.Forms.CheckBoxTS();
            this.chkL08R05 = new System.Windows.Forms.CheckBoxTS();
            this.chkL08R04 = new System.Windows.Forms.CheckBoxTS();
            this.chkL08R03 = new System.Windows.Forms.CheckBoxTS();
            this.chkL08R02 = new System.Windows.Forms.CheckBoxTS();
            this.chkL08R01 = new System.Windows.Forms.CheckBoxTS();
            this.chkL09R09 = new System.Windows.Forms.CheckBoxTS();
            this.chkL09R10 = new System.Windows.Forms.CheckBoxTS();
            this.chkL09R11 = new System.Windows.Forms.CheckBoxTS();
            this.chkL09R12 = new System.Windows.Forms.CheckBoxTS();
            this.chkL09R13 = new System.Windows.Forms.CheckBoxTS();
            this.chkL09R14 = new System.Windows.Forms.CheckBoxTS();
            this.chkL09R15 = new System.Windows.Forms.CheckBoxTS();
            this.chkL09R16 = new System.Windows.Forms.CheckBoxTS();
            this.chkL09R08 = new System.Windows.Forms.CheckBoxTS();
            this.chkL09R07 = new System.Windows.Forms.CheckBoxTS();
            this.chkL09R06 = new System.Windows.Forms.CheckBoxTS();
            this.chkL09R05 = new System.Windows.Forms.CheckBoxTS();
            this.chkL09R04 = new System.Windows.Forms.CheckBoxTS();
            this.chkL09R03 = new System.Windows.Forms.CheckBoxTS();
            this.chkL09R02 = new System.Windows.Forms.CheckBoxTS();
            this.chkL09R01 = new System.Windows.Forms.CheckBoxTS();
            this.chkL10R09 = new System.Windows.Forms.CheckBoxTS();
            this.chkL10R10 = new System.Windows.Forms.CheckBoxTS();
            this.chkL10R11 = new System.Windows.Forms.CheckBoxTS();
            this.chkL10R12 = new System.Windows.Forms.CheckBoxTS();
            this.chkL10R13 = new System.Windows.Forms.CheckBoxTS();
            this.chkL10R14 = new System.Windows.Forms.CheckBoxTS();
            this.chkL10R15 = new System.Windows.Forms.CheckBoxTS();
            this.chkL10R16 = new System.Windows.Forms.CheckBoxTS();
            this.chkL10R08 = new System.Windows.Forms.CheckBoxTS();
            this.chkL10R07 = new System.Windows.Forms.CheckBoxTS();
            this.chkL10R06 = new System.Windows.Forms.CheckBoxTS();
            this.chkL10R05 = new System.Windows.Forms.CheckBoxTS();
            this.chkL10R04 = new System.Windows.Forms.CheckBoxTS();
            this.chkL10R03 = new System.Windows.Forms.CheckBoxTS();
            this.chkL10R02 = new System.Windows.Forms.CheckBoxTS();
            this.chkL10R01 = new System.Windows.Forms.CheckBoxTS();
            this.chkL11R09 = new System.Windows.Forms.CheckBoxTS();
            this.chkL11R10 = new System.Windows.Forms.CheckBoxTS();
            this.chkL11R11 = new System.Windows.Forms.CheckBoxTS();
            this.chkL11R12 = new System.Windows.Forms.CheckBoxTS();
            this.chkL11R13 = new System.Windows.Forms.CheckBoxTS();
            this.chkL11R14 = new System.Windows.Forms.CheckBoxTS();
            this.chkL11R15 = new System.Windows.Forms.CheckBoxTS();
            this.chkL11R16 = new System.Windows.Forms.CheckBoxTS();
            this.chkL11R08 = new System.Windows.Forms.CheckBoxTS();
            this.chkL11R07 = new System.Windows.Forms.CheckBoxTS();
            this.chkL11R06 = new System.Windows.Forms.CheckBoxTS();
            this.chkL11R05 = new System.Windows.Forms.CheckBoxTS();
            this.chkL11R04 = new System.Windows.Forms.CheckBoxTS();
            this.chkL11R03 = new System.Windows.Forms.CheckBoxTS();
            this.chkL11R02 = new System.Windows.Forms.CheckBoxTS();
            this.chkL11R01 = new System.Windows.Forms.CheckBoxTS();
            this.chkL12R09 = new System.Windows.Forms.CheckBoxTS();
            this.chkL12R10 = new System.Windows.Forms.CheckBoxTS();
            this.chkL12R11 = new System.Windows.Forms.CheckBoxTS();
            this.chkL12R12 = new System.Windows.Forms.CheckBoxTS();
            this.chkL12R13 = new System.Windows.Forms.CheckBoxTS();
            this.chkL12R14 = new System.Windows.Forms.CheckBoxTS();
            this.chkL12R15 = new System.Windows.Forms.CheckBoxTS();
            this.chkL12R16 = new System.Windows.Forms.CheckBoxTS();
            this.chkL12R08 = new System.Windows.Forms.CheckBoxTS();
            this.chkL12R07 = new System.Windows.Forms.CheckBoxTS();
            this.chkL12R06 = new System.Windows.Forms.CheckBoxTS();
            this.chkL12R05 = new System.Windows.Forms.CheckBoxTS();
            this.chkL12R04 = new System.Windows.Forms.CheckBoxTS();
            this.chkL12R03 = new System.Windows.Forms.CheckBoxTS();
            this.chkL12R02 = new System.Windows.Forms.CheckBoxTS();
            this.chkL12R01 = new System.Windows.Forms.CheckBoxTS();
            this.chkL13R09 = new System.Windows.Forms.CheckBoxTS();
            this.chkL13R10 = new System.Windows.Forms.CheckBoxTS();
            this.chkL13R11 = new System.Windows.Forms.CheckBoxTS();
            this.chkL13R12 = new System.Windows.Forms.CheckBoxTS();
            this.chkL13R13 = new System.Windows.Forms.CheckBoxTS();
            this.chkL13R14 = new System.Windows.Forms.CheckBoxTS();
            this.chkL13R15 = new System.Windows.Forms.CheckBoxTS();
            this.chkL13R16 = new System.Windows.Forms.CheckBoxTS();
            this.chkL13R08 = new System.Windows.Forms.CheckBoxTS();
            this.chkL13R07 = new System.Windows.Forms.CheckBoxTS();
            this.chkL13R06 = new System.Windows.Forms.CheckBoxTS();
            this.chkL13R05 = new System.Windows.Forms.CheckBoxTS();
            this.chkL13R04 = new System.Windows.Forms.CheckBoxTS();
            this.chkL13R03 = new System.Windows.Forms.CheckBoxTS();
            this.chkL13R02 = new System.Windows.Forms.CheckBoxTS();
            this.chkL13R01 = new System.Windows.Forms.CheckBoxTS();
            this.chkL14R09 = new System.Windows.Forms.CheckBoxTS();
            this.chkL14R10 = new System.Windows.Forms.CheckBoxTS();
            this.chkL14R11 = new System.Windows.Forms.CheckBoxTS();
            this.chkL14R12 = new System.Windows.Forms.CheckBoxTS();
            this.chkL14R13 = new System.Windows.Forms.CheckBoxTS();
            this.chkL14R14 = new System.Windows.Forms.CheckBoxTS();
            this.chkL14R15 = new System.Windows.Forms.CheckBoxTS();
            this.chkL14R16 = new System.Windows.Forms.CheckBoxTS();
            this.chkL14R08 = new System.Windows.Forms.CheckBoxTS();
            this.chkL14R07 = new System.Windows.Forms.CheckBoxTS();
            this.chkL14R06 = new System.Windows.Forms.CheckBoxTS();
            this.chkL14R05 = new System.Windows.Forms.CheckBoxTS();
            this.chkL14R04 = new System.Windows.Forms.CheckBoxTS();
            this.chkL14R03 = new System.Windows.Forms.CheckBoxTS();
            this.chkL14R02 = new System.Windows.Forms.CheckBoxTS();
            this.chkL14R01 = new System.Windows.Forms.CheckBoxTS();
            this.chkL15R09 = new System.Windows.Forms.CheckBoxTS();
            this.chkL15R10 = new System.Windows.Forms.CheckBoxTS();
            this.chkL15R11 = new System.Windows.Forms.CheckBoxTS();
            this.chkL15R12 = new System.Windows.Forms.CheckBoxTS();
            this.chkL15R13 = new System.Windows.Forms.CheckBoxTS();
            this.chkL15R14 = new System.Windows.Forms.CheckBoxTS();
            this.chkL15R15 = new System.Windows.Forms.CheckBoxTS();
            this.chkL15R16 = new System.Windows.Forms.CheckBoxTS();
            this.chkL15R08 = new System.Windows.Forms.CheckBoxTS();
            this.chkL15R07 = new System.Windows.Forms.CheckBoxTS();
            this.chkL15R06 = new System.Windows.Forms.CheckBoxTS();
            this.chkL15R05 = new System.Windows.Forms.CheckBoxTS();
            this.chkL15R04 = new System.Windows.Forms.CheckBoxTS();
            this.chkL15R03 = new System.Windows.Forms.CheckBoxTS();
            this.chkL15R02 = new System.Windows.Forms.CheckBoxTS();
            this.chkL15R01 = new System.Windows.Forms.CheckBoxTS();
            this.lblAddr = new System.Windows.Forms.LabelTS();
            this.lblLine0 = new System.Windows.Forms.LabelTS();
            this.lblLine1 = new System.Windows.Forms.LabelTS();
            this.lblLine2 = new System.Windows.Forms.LabelTS();
            this.lblLine3 = new System.Windows.Forms.LabelTS();
            this.lblLine4 = new System.Windows.Forms.LabelTS();
            this.lblLine5 = new System.Windows.Forms.LabelTS();
            this.lblLine6 = new System.Windows.Forms.LabelTS();
            this.lblLine7 = new System.Windows.Forms.LabelTS();
            this.lblLine8 = new System.Windows.Forms.LabelTS();
            this.lblLine9 = new System.Windows.Forms.LabelTS();
            this.lblLine10 = new System.Windows.Forms.LabelTS();
            this.lblLine11 = new System.Windows.Forms.LabelTS();
            this.lblLine12 = new System.Windows.Forms.LabelTS();
            this.lblLine13 = new System.Windows.Forms.LabelTS();
            this.lblLine14 = new System.Windows.Forms.LabelTS();
            this.lblLine15 = new System.Windows.Forms.LabelTS();
            this.btnWriteLine0 = new System.Windows.Forms.ButtonTS();
            this.btnWriteLine1 = new System.Windows.Forms.ButtonTS();
            this.btnWriteLine2 = new System.Windows.Forms.ButtonTS();
            this.btnWriteLine3 = new System.Windows.Forms.ButtonTS();
            this.btnWriteLine4 = new System.Windows.Forms.ButtonTS();
            this.btnWriteLine5 = new System.Windows.Forms.ButtonTS();
            this.btnWriteLine6 = new System.Windows.Forms.ButtonTS();
            this.btnWriteLine7 = new System.Windows.Forms.ButtonTS();
            this.btnWriteLine8 = new System.Windows.Forms.ButtonTS();
            this.btnWriteLine9 = new System.Windows.Forms.ButtonTS();
            this.btnWriteLine10 = new System.Windows.Forms.ButtonTS();
            this.btnWriteLine11 = new System.Windows.Forms.ButtonTS();
            this.btnWriteLine12 = new System.Windows.Forms.ButtonTS();
            this.btnWriteLine13 = new System.Windows.Forms.ButtonTS();
            this.btnWriteLine14 = new System.Windows.Forms.ButtonTS();
            this.btnWriteLine15 = new System.Windows.Forms.ButtonTS();
            this.btnWriteAll = new System.Windows.Forms.ButtonTS();
            this.radLine0 = new System.Windows.Forms.RadioButtonTS();
            this.radLine1 = new System.Windows.Forms.RadioButtonTS();
            this.radLine2 = new System.Windows.Forms.RadioButtonTS();
            this.radLine3 = new System.Windows.Forms.RadioButtonTS();
            this.radLine4 = new System.Windows.Forms.RadioButtonTS();
            this.radLine5 = new System.Windows.Forms.RadioButtonTS();
            this.radLine6 = new System.Windows.Forms.RadioButtonTS();
            this.radLine7 = new System.Windows.Forms.RadioButtonTS();
            this.radLine8 = new System.Windows.Forms.RadioButtonTS();
            this.radLine9 = new System.Windows.Forms.RadioButtonTS();
            this.radLine10 = new System.Windows.Forms.RadioButtonTS();
            this.radLine11 = new System.Windows.Forms.RadioButtonTS();
            this.radLine12 = new System.Windows.Forms.RadioButtonTS();
            this.radLine13 = new System.Windows.Forms.RadioButtonTS();
            this.radLine14 = new System.Windows.Forms.RadioButtonTS();
            this.radLine15 = new System.Windows.Forms.RadioButtonTS();
            this.lblRelays = new System.Windows.Forms.LabelTS();
            this.lblDelay = new System.Windows.Forms.LabelTS();
            this.comboDelay = new System.Windows.Forms.ComboBoxTS();
            this.btnSetDelay = new System.Windows.Forms.Button();
            this.btnSetupXVTR = new System.Windows.Forms.Button();
            this.chkFlexWire = new System.Windows.Forms.CheckBoxTS();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // btnEnable
            // 
            this.btnEnable.Image = null;
            this.btnEnable.Location = new System.Drawing.Point(128, 16);
            this.btnEnable.Name = "btnEnable";
            this.btnEnable.Size = new System.Drawing.Size(88, 23);
            this.btnEnable.TabIndex = 0;
            this.btnEnable.Text = "Enable Relays";
            this.btnEnable.Click += new System.EventHandler(this.btnEnable_Click);
            // 
            // btnDisable
            // 
            this.btnDisable.Image = null;
            this.btnDisable.Location = new System.Drawing.Point(128, 48);
            this.btnDisable.Name = "btnDisable";
            this.btnDisable.Size = new System.Drawing.Size(88, 23);
            this.btnDisable.TabIndex = 1;
            this.btnDisable.Text = "Disable";
            this.btnDisable.Click += new System.EventHandler(this.btnDisable_Click);
            // 
            // btnDisableClear
            // 
            this.btnDisableClear.Image = null;
            this.btnDisableClear.Location = new System.Drawing.Point(232, 48);
            this.btnDisableClear.Name = "btnDisableClear";
            this.btnDisableClear.Size = new System.Drawing.Size(136, 23);
            this.btnDisableClear.TabIndex = 2;
            this.btnDisableClear.Text = "Disable and Clear Matrix";
            this.btnDisableClear.Click += new System.EventHandler(this.btnDisableClear_Click);
            // 
            // chkL00R01
            // 
            this.chkL00R01.Image = null;
            this.chkL00R01.Location = new System.Drawing.Point(104, 112);
            this.chkL00R01.Name = "chkL00R01";
            this.chkL00R01.Size = new System.Drawing.Size(16, 24);
            this.chkL00R01.TabIndex = 3;
            this.chkL00R01.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL00R02
            // 
            this.chkL00R02.Image = null;
            this.chkL00R02.Location = new System.Drawing.Point(120, 112);
            this.chkL00R02.Name = "chkL00R02";
            this.chkL00R02.Size = new System.Drawing.Size(16, 24);
            this.chkL00R02.TabIndex = 4;
            this.chkL00R02.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL00R03
            // 
            this.chkL00R03.Image = null;
            this.chkL00R03.Location = new System.Drawing.Point(136, 112);
            this.chkL00R03.Name = "chkL00R03";
            this.chkL00R03.Size = new System.Drawing.Size(16, 24);
            this.chkL00R03.TabIndex = 5;
            this.chkL00R03.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL00R04
            // 
            this.chkL00R04.Image = null;
            this.chkL00R04.Location = new System.Drawing.Point(152, 112);
            this.chkL00R04.Name = "chkL00R04";
            this.chkL00R04.Size = new System.Drawing.Size(16, 24);
            this.chkL00R04.TabIndex = 6;
            this.chkL00R04.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL00R05
            // 
            this.chkL00R05.Image = null;
            this.chkL00R05.Location = new System.Drawing.Point(176, 112);
            this.chkL00R05.Name = "chkL00R05";
            this.chkL00R05.Size = new System.Drawing.Size(16, 24);
            this.chkL00R05.TabIndex = 7;
            this.chkL00R05.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL00R06
            // 
            this.chkL00R06.Image = null;
            this.chkL00R06.Location = new System.Drawing.Point(192, 112);
            this.chkL00R06.Name = "chkL00R06";
            this.chkL00R06.Size = new System.Drawing.Size(16, 24);
            this.chkL00R06.TabIndex = 8;
            this.chkL00R06.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL00R07
            // 
            this.chkL00R07.Image = null;
            this.chkL00R07.Location = new System.Drawing.Point(208, 112);
            this.chkL00R07.Name = "chkL00R07";
            this.chkL00R07.Size = new System.Drawing.Size(16, 24);
            this.chkL00R07.TabIndex = 9;
            this.chkL00R07.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL00R08
            // 
            this.chkL00R08.Image = null;
            this.chkL00R08.Location = new System.Drawing.Point(224, 112);
            this.chkL00R08.Name = "chkL00R08";
            this.chkL00R08.Size = new System.Drawing.Size(16, 24);
            this.chkL00R08.TabIndex = 10;
            this.chkL00R08.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL00R09
            // 
            this.chkL00R09.Image = null;
            this.chkL00R09.Location = new System.Drawing.Point(248, 112);
            this.chkL00R09.Name = "chkL00R09";
            this.chkL00R09.Size = new System.Drawing.Size(16, 24);
            this.chkL00R09.TabIndex = 18;
            this.chkL00R09.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL00R10
            // 
            this.chkL00R10.Image = null;
            this.chkL00R10.Location = new System.Drawing.Point(264, 112);
            this.chkL00R10.Name = "chkL00R10";
            this.chkL00R10.Size = new System.Drawing.Size(16, 24);
            this.chkL00R10.TabIndex = 17;
            this.chkL00R10.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL00R11
            // 
            this.chkL00R11.Image = null;
            this.chkL00R11.Location = new System.Drawing.Point(280, 112);
            this.chkL00R11.Name = "chkL00R11";
            this.chkL00R11.Size = new System.Drawing.Size(16, 24);
            this.chkL00R11.TabIndex = 16;
            this.chkL00R11.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL00R12
            // 
            this.chkL00R12.Image = null;
            this.chkL00R12.Location = new System.Drawing.Point(296, 112);
            this.chkL00R12.Name = "chkL00R12";
            this.chkL00R12.Size = new System.Drawing.Size(16, 24);
            this.chkL00R12.TabIndex = 15;
            this.chkL00R12.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL00R13
            // 
            this.chkL00R13.Image = null;
            this.chkL00R13.Location = new System.Drawing.Point(320, 112);
            this.chkL00R13.Name = "chkL00R13";
            this.chkL00R13.Size = new System.Drawing.Size(16, 24);
            this.chkL00R13.TabIndex = 14;
            this.chkL00R13.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL00R14
            // 
            this.chkL00R14.Image = null;
            this.chkL00R14.Location = new System.Drawing.Point(336, 112);
            this.chkL00R14.Name = "chkL00R14";
            this.chkL00R14.Size = new System.Drawing.Size(16, 24);
            this.chkL00R14.TabIndex = 13;
            this.chkL00R14.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL00R15
            // 
            this.chkL00R15.Image = null;
            this.chkL00R15.Location = new System.Drawing.Point(352, 112);
            this.chkL00R15.Name = "chkL00R15";
            this.chkL00R15.Size = new System.Drawing.Size(16, 24);
            this.chkL00R15.TabIndex = 12;
            this.chkL00R15.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL00R16
            // 
            this.chkL00R16.Image = null;
            this.chkL00R16.Location = new System.Drawing.Point(368, 112);
            this.chkL00R16.Name = "chkL00R16";
            this.chkL00R16.Size = new System.Drawing.Size(16, 24);
            this.chkL00R16.TabIndex = 11;
            this.chkL00R16.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // lblR1
            // 
            this.lblR1.Image = null;
            this.lblR1.Location = new System.Drawing.Point(104, 96);
            this.lblR1.Name = "lblR1";
            this.lblR1.Size = new System.Drawing.Size(17, 16);
            this.lblR1.TabIndex = 19;
            this.lblR1.Text = "1";
            // 
            // lblR2
            // 
            this.lblR2.Image = null;
            this.lblR2.Location = new System.Drawing.Point(120, 96);
            this.lblR2.Name = "lblR2";
            this.lblR2.Size = new System.Drawing.Size(17, 16);
            this.lblR2.TabIndex = 20;
            this.lblR2.Text = "2";
            // 
            // lblR3
            // 
            this.lblR3.Image = null;
            this.lblR3.Location = new System.Drawing.Point(136, 96);
            this.lblR3.Name = "lblR3";
            this.lblR3.Size = new System.Drawing.Size(17, 16);
            this.lblR3.TabIndex = 21;
            this.lblR3.Text = "3";
            // 
            // lblR4
            // 
            this.lblR4.Image = null;
            this.lblR4.Location = new System.Drawing.Point(152, 96);
            this.lblR4.Name = "lblR4";
            this.lblR4.Size = new System.Drawing.Size(17, 16);
            this.lblR4.TabIndex = 22;
            this.lblR4.Text = "4";
            // 
            // lblR5
            // 
            this.lblR5.Image = null;
            this.lblR5.Location = new System.Drawing.Point(176, 96);
            this.lblR5.Name = "lblR5";
            this.lblR5.Size = new System.Drawing.Size(17, 16);
            this.lblR5.TabIndex = 23;
            this.lblR5.Text = "5";
            // 
            // lblR6
            // 
            this.lblR6.Image = null;
            this.lblR6.Location = new System.Drawing.Point(192, 96);
            this.lblR6.Name = "lblR6";
            this.lblR6.Size = new System.Drawing.Size(17, 16);
            this.lblR6.TabIndex = 24;
            this.lblR6.Text = "6";
            // 
            // lblR7
            // 
            this.lblR7.Image = null;
            this.lblR7.Location = new System.Drawing.Point(208, 96);
            this.lblR7.Name = "lblR7";
            this.lblR7.Size = new System.Drawing.Size(17, 16);
            this.lblR7.TabIndex = 25;
            this.lblR7.Text = "7";
            // 
            // lblR8
            // 
            this.lblR8.Image = null;
            this.lblR8.Location = new System.Drawing.Point(224, 96);
            this.lblR8.Name = "lblR8";
            this.lblR8.Size = new System.Drawing.Size(17, 16);
            this.lblR8.TabIndex = 26;
            this.lblR8.Text = "8";
            // 
            // lblR9
            // 
            this.lblR9.Image = null;
            this.lblR9.Location = new System.Drawing.Point(248, 96);
            this.lblR9.Name = "lblR9";
            this.lblR9.Size = new System.Drawing.Size(17, 16);
            this.lblR9.TabIndex = 27;
            this.lblR9.Text = "9";
            // 
            // lblR10
            // 
            this.lblR10.Image = null;
            this.lblR10.Location = new System.Drawing.Point(262, 96);
            this.lblR10.Name = "lblR10";
            this.lblR10.Size = new System.Drawing.Size(19, 16);
            this.lblR10.TabIndex = 28;
            this.lblR10.Text = "10";
            // 
            // lblR11
            // 
            this.lblR11.Image = null;
            this.lblR11.Location = new System.Drawing.Point(279, 96);
            this.lblR11.Name = "lblR11";
            this.lblR11.Size = new System.Drawing.Size(19, 16);
            this.lblR11.TabIndex = 29;
            this.lblR11.Text = "11";
            // 
            // lblR12
            // 
            this.lblR12.Image = null;
            this.lblR12.Location = new System.Drawing.Point(294, 96);
            this.lblR12.Name = "lblR12";
            this.lblR12.Size = new System.Drawing.Size(19, 16);
            this.lblR12.TabIndex = 30;
            this.lblR12.Text = "12";
            // 
            // lblR13
            // 
            this.lblR13.Image = null;
            this.lblR13.Location = new System.Drawing.Point(318, 96);
            this.lblR13.Name = "lblR13";
            this.lblR13.Size = new System.Drawing.Size(19, 16);
            this.lblR13.TabIndex = 31;
            this.lblR13.Text = "13";
            // 
            // lblR14
            // 
            this.lblR14.Image = null;
            this.lblR14.Location = new System.Drawing.Point(334, 96);
            this.lblR14.Name = "lblR14";
            this.lblR14.Size = new System.Drawing.Size(19, 16);
            this.lblR14.TabIndex = 32;
            this.lblR14.Text = "14";
            // 
            // lblR15
            // 
            this.lblR15.Image = null;
            this.lblR15.Location = new System.Drawing.Point(350, 96);
            this.lblR15.Name = "lblR15";
            this.lblR15.Size = new System.Drawing.Size(19, 16);
            this.lblR15.TabIndex = 33;
            this.lblR15.Text = "15";
            // 
            // lblR16
            // 
            this.lblR16.Image = null;
            this.lblR16.Location = new System.Drawing.Point(366, 96);
            this.lblR16.Name = "lblR16";
            this.lblR16.Size = new System.Drawing.Size(19, 16);
            this.lblR16.TabIndex = 34;
            this.lblR16.Text = "16";
            // 
            // chkL01R09
            // 
            this.chkL01R09.Image = null;
            this.chkL01R09.Location = new System.Drawing.Point(248, 136);
            this.chkL01R09.Name = "chkL01R09";
            this.chkL01R09.Size = new System.Drawing.Size(16, 24);
            this.chkL01R09.TabIndex = 50;
            this.chkL01R09.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL01R10
            // 
            this.chkL01R10.Image = null;
            this.chkL01R10.Location = new System.Drawing.Point(264, 136);
            this.chkL01R10.Name = "chkL01R10";
            this.chkL01R10.Size = new System.Drawing.Size(16, 24);
            this.chkL01R10.TabIndex = 49;
            this.chkL01R10.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL01R11
            // 
            this.chkL01R11.Image = null;
            this.chkL01R11.Location = new System.Drawing.Point(280, 136);
            this.chkL01R11.Name = "chkL01R11";
            this.chkL01R11.Size = new System.Drawing.Size(16, 24);
            this.chkL01R11.TabIndex = 48;
            this.chkL01R11.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL01R12
            // 
            this.chkL01R12.Image = null;
            this.chkL01R12.Location = new System.Drawing.Point(296, 136);
            this.chkL01R12.Name = "chkL01R12";
            this.chkL01R12.Size = new System.Drawing.Size(16, 24);
            this.chkL01R12.TabIndex = 47;
            this.chkL01R12.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL01R13
            // 
            this.chkL01R13.Image = null;
            this.chkL01R13.Location = new System.Drawing.Point(320, 136);
            this.chkL01R13.Name = "chkL01R13";
            this.chkL01R13.Size = new System.Drawing.Size(16, 24);
            this.chkL01R13.TabIndex = 46;
            this.chkL01R13.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL01R14
            // 
            this.chkL01R14.Image = null;
            this.chkL01R14.Location = new System.Drawing.Point(336, 136);
            this.chkL01R14.Name = "chkL01R14";
            this.chkL01R14.Size = new System.Drawing.Size(16, 24);
            this.chkL01R14.TabIndex = 45;
            this.chkL01R14.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL01R15
            // 
            this.chkL01R15.Image = null;
            this.chkL01R15.Location = new System.Drawing.Point(352, 136);
            this.chkL01R15.Name = "chkL01R15";
            this.chkL01R15.Size = new System.Drawing.Size(16, 24);
            this.chkL01R15.TabIndex = 44;
            this.chkL01R15.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL01R16
            // 
            this.chkL01R16.Image = null;
            this.chkL01R16.Location = new System.Drawing.Point(368, 136);
            this.chkL01R16.Name = "chkL01R16";
            this.chkL01R16.Size = new System.Drawing.Size(16, 24);
            this.chkL01R16.TabIndex = 43;
            this.chkL01R16.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL01R08
            // 
            this.chkL01R08.Image = null;
            this.chkL01R08.Location = new System.Drawing.Point(224, 136);
            this.chkL01R08.Name = "chkL01R08";
            this.chkL01R08.Size = new System.Drawing.Size(16, 24);
            this.chkL01R08.TabIndex = 42;
            this.chkL01R08.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL01R07
            // 
            this.chkL01R07.Image = null;
            this.chkL01R07.Location = new System.Drawing.Point(208, 136);
            this.chkL01R07.Name = "chkL01R07";
            this.chkL01R07.Size = new System.Drawing.Size(16, 24);
            this.chkL01R07.TabIndex = 41;
            this.chkL01R07.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL01R06
            // 
            this.chkL01R06.Image = null;
            this.chkL01R06.Location = new System.Drawing.Point(192, 136);
            this.chkL01R06.Name = "chkL01R06";
            this.chkL01R06.Size = new System.Drawing.Size(16, 24);
            this.chkL01R06.TabIndex = 40;
            this.chkL01R06.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL01R05
            // 
            this.chkL01R05.Image = null;
            this.chkL01R05.Location = new System.Drawing.Point(176, 136);
            this.chkL01R05.Name = "chkL01R05";
            this.chkL01R05.Size = new System.Drawing.Size(16, 24);
            this.chkL01R05.TabIndex = 39;
            this.chkL01R05.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL01R04
            // 
            this.chkL01R04.Image = null;
            this.chkL01R04.Location = new System.Drawing.Point(152, 136);
            this.chkL01R04.Name = "chkL01R04";
            this.chkL01R04.Size = new System.Drawing.Size(16, 24);
            this.chkL01R04.TabIndex = 38;
            this.chkL01R04.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL01R03
            // 
            this.chkL01R03.Image = null;
            this.chkL01R03.Location = new System.Drawing.Point(136, 136);
            this.chkL01R03.Name = "chkL01R03";
            this.chkL01R03.Size = new System.Drawing.Size(16, 24);
            this.chkL01R03.TabIndex = 37;
            this.chkL01R03.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL01R02
            // 
            this.chkL01R02.Image = null;
            this.chkL01R02.Location = new System.Drawing.Point(120, 136);
            this.chkL01R02.Name = "chkL01R02";
            this.chkL01R02.Size = new System.Drawing.Size(16, 24);
            this.chkL01R02.TabIndex = 36;
            this.chkL01R02.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL01R01
            // 
            this.chkL01R01.Image = null;
            this.chkL01R01.Location = new System.Drawing.Point(104, 136);
            this.chkL01R01.Name = "chkL01R01";
            this.chkL01R01.Size = new System.Drawing.Size(16, 24);
            this.chkL01R01.TabIndex = 35;
            this.chkL01R01.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL02R09
            // 
            this.chkL02R09.Image = null;
            this.chkL02R09.Location = new System.Drawing.Point(248, 160);
            this.chkL02R09.Name = "chkL02R09";
            this.chkL02R09.Size = new System.Drawing.Size(16, 24);
            this.chkL02R09.TabIndex = 66;
            this.chkL02R09.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL02R10
            // 
            this.chkL02R10.Image = null;
            this.chkL02R10.Location = new System.Drawing.Point(264, 160);
            this.chkL02R10.Name = "chkL02R10";
            this.chkL02R10.Size = new System.Drawing.Size(16, 24);
            this.chkL02R10.TabIndex = 65;
            this.chkL02R10.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL02R11
            // 
            this.chkL02R11.Image = null;
            this.chkL02R11.Location = new System.Drawing.Point(280, 160);
            this.chkL02R11.Name = "chkL02R11";
            this.chkL02R11.Size = new System.Drawing.Size(16, 24);
            this.chkL02R11.TabIndex = 64;
            this.chkL02R11.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL02R12
            // 
            this.chkL02R12.Image = null;
            this.chkL02R12.Location = new System.Drawing.Point(296, 160);
            this.chkL02R12.Name = "chkL02R12";
            this.chkL02R12.Size = new System.Drawing.Size(16, 24);
            this.chkL02R12.TabIndex = 63;
            this.chkL02R12.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL02R13
            // 
            this.chkL02R13.Image = null;
            this.chkL02R13.Location = new System.Drawing.Point(320, 160);
            this.chkL02R13.Name = "chkL02R13";
            this.chkL02R13.Size = new System.Drawing.Size(16, 24);
            this.chkL02R13.TabIndex = 62;
            this.chkL02R13.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL02R14
            // 
            this.chkL02R14.Image = null;
            this.chkL02R14.Location = new System.Drawing.Point(336, 160);
            this.chkL02R14.Name = "chkL02R14";
            this.chkL02R14.Size = new System.Drawing.Size(16, 24);
            this.chkL02R14.TabIndex = 61;
            this.chkL02R14.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL02R15
            // 
            this.chkL02R15.Image = null;
            this.chkL02R15.Location = new System.Drawing.Point(352, 160);
            this.chkL02R15.Name = "chkL02R15";
            this.chkL02R15.Size = new System.Drawing.Size(16, 24);
            this.chkL02R15.TabIndex = 60;
            this.chkL02R15.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL02R16
            // 
            this.chkL02R16.Image = null;
            this.chkL02R16.Location = new System.Drawing.Point(368, 160);
            this.chkL02R16.Name = "chkL02R16";
            this.chkL02R16.Size = new System.Drawing.Size(16, 24);
            this.chkL02R16.TabIndex = 59;
            this.chkL02R16.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL02R08
            // 
            this.chkL02R08.Image = null;
            this.chkL02R08.Location = new System.Drawing.Point(224, 160);
            this.chkL02R08.Name = "chkL02R08";
            this.chkL02R08.Size = new System.Drawing.Size(16, 24);
            this.chkL02R08.TabIndex = 58;
            this.chkL02R08.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL02R07
            // 
            this.chkL02R07.Image = null;
            this.chkL02R07.Location = new System.Drawing.Point(208, 160);
            this.chkL02R07.Name = "chkL02R07";
            this.chkL02R07.Size = new System.Drawing.Size(16, 24);
            this.chkL02R07.TabIndex = 57;
            this.chkL02R07.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL02R06
            // 
            this.chkL02R06.Image = null;
            this.chkL02R06.Location = new System.Drawing.Point(192, 160);
            this.chkL02R06.Name = "chkL02R06";
            this.chkL02R06.Size = new System.Drawing.Size(16, 24);
            this.chkL02R06.TabIndex = 56;
            this.chkL02R06.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL02R05
            // 
            this.chkL02R05.Image = null;
            this.chkL02R05.Location = new System.Drawing.Point(176, 160);
            this.chkL02R05.Name = "chkL02R05";
            this.chkL02R05.Size = new System.Drawing.Size(16, 24);
            this.chkL02R05.TabIndex = 55;
            this.chkL02R05.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL02R04
            // 
            this.chkL02R04.Image = null;
            this.chkL02R04.Location = new System.Drawing.Point(152, 160);
            this.chkL02R04.Name = "chkL02R04";
            this.chkL02R04.Size = new System.Drawing.Size(16, 24);
            this.chkL02R04.TabIndex = 54;
            this.chkL02R04.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL02R03
            // 
            this.chkL02R03.Image = null;
            this.chkL02R03.Location = new System.Drawing.Point(136, 160);
            this.chkL02R03.Name = "chkL02R03";
            this.chkL02R03.Size = new System.Drawing.Size(16, 24);
            this.chkL02R03.TabIndex = 53;
            this.chkL02R03.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL02R02
            // 
            this.chkL02R02.Image = null;
            this.chkL02R02.Location = new System.Drawing.Point(120, 160);
            this.chkL02R02.Name = "chkL02R02";
            this.chkL02R02.Size = new System.Drawing.Size(16, 24);
            this.chkL02R02.TabIndex = 52;
            this.chkL02R02.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL02R01
            // 
            this.chkL02R01.Image = null;
            this.chkL02R01.Location = new System.Drawing.Point(104, 160);
            this.chkL02R01.Name = "chkL02R01";
            this.chkL02R01.Size = new System.Drawing.Size(16, 24);
            this.chkL02R01.TabIndex = 51;
            this.chkL02R01.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL03R09
            // 
            this.chkL03R09.Image = null;
            this.chkL03R09.Location = new System.Drawing.Point(248, 184);
            this.chkL03R09.Name = "chkL03R09";
            this.chkL03R09.Size = new System.Drawing.Size(16, 24);
            this.chkL03R09.TabIndex = 82;
            this.chkL03R09.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL03R10
            // 
            this.chkL03R10.Image = null;
            this.chkL03R10.Location = new System.Drawing.Point(264, 184);
            this.chkL03R10.Name = "chkL03R10";
            this.chkL03R10.Size = new System.Drawing.Size(16, 24);
            this.chkL03R10.TabIndex = 81;
            this.chkL03R10.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL03R11
            // 
            this.chkL03R11.Image = null;
            this.chkL03R11.Location = new System.Drawing.Point(280, 184);
            this.chkL03R11.Name = "chkL03R11";
            this.chkL03R11.Size = new System.Drawing.Size(16, 24);
            this.chkL03R11.TabIndex = 80;
            this.chkL03R11.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL03R12
            // 
            this.chkL03R12.Image = null;
            this.chkL03R12.Location = new System.Drawing.Point(296, 184);
            this.chkL03R12.Name = "chkL03R12";
            this.chkL03R12.Size = new System.Drawing.Size(16, 24);
            this.chkL03R12.TabIndex = 79;
            this.chkL03R12.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL03R13
            // 
            this.chkL03R13.Image = null;
            this.chkL03R13.Location = new System.Drawing.Point(320, 184);
            this.chkL03R13.Name = "chkL03R13";
            this.chkL03R13.Size = new System.Drawing.Size(16, 24);
            this.chkL03R13.TabIndex = 78;
            this.chkL03R13.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL03R14
            // 
            this.chkL03R14.Image = null;
            this.chkL03R14.Location = new System.Drawing.Point(336, 184);
            this.chkL03R14.Name = "chkL03R14";
            this.chkL03R14.Size = new System.Drawing.Size(16, 24);
            this.chkL03R14.TabIndex = 77;
            this.chkL03R14.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL03R15
            // 
            this.chkL03R15.Image = null;
            this.chkL03R15.Location = new System.Drawing.Point(352, 184);
            this.chkL03R15.Name = "chkL03R15";
            this.chkL03R15.Size = new System.Drawing.Size(16, 24);
            this.chkL03R15.TabIndex = 76;
            this.chkL03R15.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL03R16
            // 
            this.chkL03R16.Image = null;
            this.chkL03R16.Location = new System.Drawing.Point(368, 184);
            this.chkL03R16.Name = "chkL03R16";
            this.chkL03R16.Size = new System.Drawing.Size(16, 24);
            this.chkL03R16.TabIndex = 75;
            this.chkL03R16.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL03R08
            // 
            this.chkL03R08.Image = null;
            this.chkL03R08.Location = new System.Drawing.Point(224, 184);
            this.chkL03R08.Name = "chkL03R08";
            this.chkL03R08.Size = new System.Drawing.Size(16, 24);
            this.chkL03R08.TabIndex = 74;
            this.chkL03R08.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL03R07
            // 
            this.chkL03R07.Image = null;
            this.chkL03R07.Location = new System.Drawing.Point(208, 184);
            this.chkL03R07.Name = "chkL03R07";
            this.chkL03R07.Size = new System.Drawing.Size(16, 24);
            this.chkL03R07.TabIndex = 73;
            this.chkL03R07.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL03R06
            // 
            this.chkL03R06.Image = null;
            this.chkL03R06.Location = new System.Drawing.Point(192, 184);
            this.chkL03R06.Name = "chkL03R06";
            this.chkL03R06.Size = new System.Drawing.Size(16, 24);
            this.chkL03R06.TabIndex = 72;
            this.chkL03R06.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL03R05
            // 
            this.chkL03R05.Image = null;
            this.chkL03R05.Location = new System.Drawing.Point(176, 184);
            this.chkL03R05.Name = "chkL03R05";
            this.chkL03R05.Size = new System.Drawing.Size(16, 24);
            this.chkL03R05.TabIndex = 71;
            this.chkL03R05.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL03R04
            // 
            this.chkL03R04.Image = null;
            this.chkL03R04.Location = new System.Drawing.Point(152, 184);
            this.chkL03R04.Name = "chkL03R04";
            this.chkL03R04.Size = new System.Drawing.Size(16, 24);
            this.chkL03R04.TabIndex = 70;
            this.chkL03R04.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL03R03
            // 
            this.chkL03R03.Image = null;
            this.chkL03R03.Location = new System.Drawing.Point(136, 184);
            this.chkL03R03.Name = "chkL03R03";
            this.chkL03R03.Size = new System.Drawing.Size(16, 24);
            this.chkL03R03.TabIndex = 69;
            this.chkL03R03.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL03R02
            // 
            this.chkL03R02.Image = null;
            this.chkL03R02.Location = new System.Drawing.Point(120, 184);
            this.chkL03R02.Name = "chkL03R02";
            this.chkL03R02.Size = new System.Drawing.Size(16, 24);
            this.chkL03R02.TabIndex = 68;
            this.chkL03R02.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL03R01
            // 
            this.chkL03R01.Image = null;
            this.chkL03R01.Location = new System.Drawing.Point(104, 184);
            this.chkL03R01.Name = "chkL03R01";
            this.chkL03R01.Size = new System.Drawing.Size(16, 24);
            this.chkL03R01.TabIndex = 67;
            this.chkL03R01.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL04R09
            // 
            this.chkL04R09.Image = null;
            this.chkL04R09.Location = new System.Drawing.Point(248, 216);
            this.chkL04R09.Name = "chkL04R09";
            this.chkL04R09.Size = new System.Drawing.Size(16, 24);
            this.chkL04R09.TabIndex = 98;
            this.chkL04R09.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL04R10
            // 
            this.chkL04R10.Image = null;
            this.chkL04R10.Location = new System.Drawing.Point(264, 216);
            this.chkL04R10.Name = "chkL04R10";
            this.chkL04R10.Size = new System.Drawing.Size(16, 24);
            this.chkL04R10.TabIndex = 97;
            this.chkL04R10.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL04R11
            // 
            this.chkL04R11.Image = null;
            this.chkL04R11.Location = new System.Drawing.Point(280, 216);
            this.chkL04R11.Name = "chkL04R11";
            this.chkL04R11.Size = new System.Drawing.Size(16, 24);
            this.chkL04R11.TabIndex = 96;
            this.chkL04R11.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL04R12
            // 
            this.chkL04R12.Image = null;
            this.chkL04R12.Location = new System.Drawing.Point(296, 216);
            this.chkL04R12.Name = "chkL04R12";
            this.chkL04R12.Size = new System.Drawing.Size(16, 24);
            this.chkL04R12.TabIndex = 95;
            this.chkL04R12.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL04R13
            // 
            this.chkL04R13.Image = null;
            this.chkL04R13.Location = new System.Drawing.Point(320, 216);
            this.chkL04R13.Name = "chkL04R13";
            this.chkL04R13.Size = new System.Drawing.Size(16, 24);
            this.chkL04R13.TabIndex = 94;
            this.chkL04R13.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL04R14
            // 
            this.chkL04R14.Image = null;
            this.chkL04R14.Location = new System.Drawing.Point(336, 216);
            this.chkL04R14.Name = "chkL04R14";
            this.chkL04R14.Size = new System.Drawing.Size(16, 24);
            this.chkL04R14.TabIndex = 93;
            this.chkL04R14.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL04R15
            // 
            this.chkL04R15.Image = null;
            this.chkL04R15.Location = new System.Drawing.Point(352, 216);
            this.chkL04R15.Name = "chkL04R15";
            this.chkL04R15.Size = new System.Drawing.Size(16, 24);
            this.chkL04R15.TabIndex = 92;
            this.chkL04R15.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL04R16
            // 
            this.chkL04R16.Image = null;
            this.chkL04R16.Location = new System.Drawing.Point(368, 216);
            this.chkL04R16.Name = "chkL04R16";
            this.chkL04R16.Size = new System.Drawing.Size(16, 24);
            this.chkL04R16.TabIndex = 91;
            this.chkL04R16.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL04R08
            // 
            this.chkL04R08.Image = null;
            this.chkL04R08.Location = new System.Drawing.Point(224, 216);
            this.chkL04R08.Name = "chkL04R08";
            this.chkL04R08.Size = new System.Drawing.Size(16, 24);
            this.chkL04R08.TabIndex = 90;
            this.chkL04R08.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL04R07
            // 
            this.chkL04R07.Image = null;
            this.chkL04R07.Location = new System.Drawing.Point(208, 216);
            this.chkL04R07.Name = "chkL04R07";
            this.chkL04R07.Size = new System.Drawing.Size(16, 24);
            this.chkL04R07.TabIndex = 89;
            this.chkL04R07.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL04R06
            // 
            this.chkL04R06.Image = null;
            this.chkL04R06.Location = new System.Drawing.Point(192, 216);
            this.chkL04R06.Name = "chkL04R06";
            this.chkL04R06.Size = new System.Drawing.Size(16, 24);
            this.chkL04R06.TabIndex = 88;
            this.chkL04R06.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL04R05
            // 
            this.chkL04R05.Image = null;
            this.chkL04R05.Location = new System.Drawing.Point(176, 216);
            this.chkL04R05.Name = "chkL04R05";
            this.chkL04R05.Size = new System.Drawing.Size(16, 24);
            this.chkL04R05.TabIndex = 87;
            this.chkL04R05.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL04R04
            // 
            this.chkL04R04.Image = null;
            this.chkL04R04.Location = new System.Drawing.Point(152, 216);
            this.chkL04R04.Name = "chkL04R04";
            this.chkL04R04.Size = new System.Drawing.Size(16, 24);
            this.chkL04R04.TabIndex = 86;
            this.chkL04R04.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL04R03
            // 
            this.chkL04R03.Image = null;
            this.chkL04R03.Location = new System.Drawing.Point(136, 216);
            this.chkL04R03.Name = "chkL04R03";
            this.chkL04R03.Size = new System.Drawing.Size(16, 24);
            this.chkL04R03.TabIndex = 85;
            this.chkL04R03.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL04R02
            // 
            this.chkL04R02.Image = null;
            this.chkL04R02.Location = new System.Drawing.Point(120, 216);
            this.chkL04R02.Name = "chkL04R02";
            this.chkL04R02.Size = new System.Drawing.Size(16, 24);
            this.chkL04R02.TabIndex = 84;
            this.chkL04R02.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL04R01
            // 
            this.chkL04R01.Image = null;
            this.chkL04R01.Location = new System.Drawing.Point(104, 216);
            this.chkL04R01.Name = "chkL04R01";
            this.chkL04R01.Size = new System.Drawing.Size(16, 24);
            this.chkL04R01.TabIndex = 83;
            this.chkL04R01.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL05R09
            // 
            this.chkL05R09.Image = null;
            this.chkL05R09.Location = new System.Drawing.Point(248, 240);
            this.chkL05R09.Name = "chkL05R09";
            this.chkL05R09.Size = new System.Drawing.Size(16, 24);
            this.chkL05R09.TabIndex = 114;
            this.chkL05R09.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL05R10
            // 
            this.chkL05R10.Image = null;
            this.chkL05R10.Location = new System.Drawing.Point(264, 240);
            this.chkL05R10.Name = "chkL05R10";
            this.chkL05R10.Size = new System.Drawing.Size(16, 24);
            this.chkL05R10.TabIndex = 113;
            this.chkL05R10.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL05R11
            // 
            this.chkL05R11.Image = null;
            this.chkL05R11.Location = new System.Drawing.Point(280, 240);
            this.chkL05R11.Name = "chkL05R11";
            this.chkL05R11.Size = new System.Drawing.Size(16, 24);
            this.chkL05R11.TabIndex = 112;
            this.chkL05R11.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL05R12
            // 
            this.chkL05R12.Image = null;
            this.chkL05R12.Location = new System.Drawing.Point(296, 240);
            this.chkL05R12.Name = "chkL05R12";
            this.chkL05R12.Size = new System.Drawing.Size(16, 24);
            this.chkL05R12.TabIndex = 111;
            this.chkL05R12.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL05R13
            // 
            this.chkL05R13.Image = null;
            this.chkL05R13.Location = new System.Drawing.Point(320, 240);
            this.chkL05R13.Name = "chkL05R13";
            this.chkL05R13.Size = new System.Drawing.Size(16, 24);
            this.chkL05R13.TabIndex = 110;
            this.chkL05R13.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL05R14
            // 
            this.chkL05R14.Image = null;
            this.chkL05R14.Location = new System.Drawing.Point(336, 240);
            this.chkL05R14.Name = "chkL05R14";
            this.chkL05R14.Size = new System.Drawing.Size(16, 24);
            this.chkL05R14.TabIndex = 109;
            this.chkL05R14.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL05R15
            // 
            this.chkL05R15.Image = null;
            this.chkL05R15.Location = new System.Drawing.Point(352, 240);
            this.chkL05R15.Name = "chkL05R15";
            this.chkL05R15.Size = new System.Drawing.Size(16, 24);
            this.chkL05R15.TabIndex = 108;
            this.chkL05R15.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL05R16
            // 
            this.chkL05R16.Image = null;
            this.chkL05R16.Location = new System.Drawing.Point(368, 240);
            this.chkL05R16.Name = "chkL05R16";
            this.chkL05R16.Size = new System.Drawing.Size(16, 24);
            this.chkL05R16.TabIndex = 107;
            this.chkL05R16.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL05R08
            // 
            this.chkL05R08.Image = null;
            this.chkL05R08.Location = new System.Drawing.Point(224, 240);
            this.chkL05R08.Name = "chkL05R08";
            this.chkL05R08.Size = new System.Drawing.Size(16, 24);
            this.chkL05R08.TabIndex = 106;
            this.chkL05R08.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL05R07
            // 
            this.chkL05R07.Image = null;
            this.chkL05R07.Location = new System.Drawing.Point(208, 240);
            this.chkL05R07.Name = "chkL05R07";
            this.chkL05R07.Size = new System.Drawing.Size(16, 24);
            this.chkL05R07.TabIndex = 105;
            this.chkL05R07.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL05R06
            // 
            this.chkL05R06.Image = null;
            this.chkL05R06.Location = new System.Drawing.Point(192, 240);
            this.chkL05R06.Name = "chkL05R06";
            this.chkL05R06.Size = new System.Drawing.Size(16, 24);
            this.chkL05R06.TabIndex = 104;
            this.chkL05R06.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL05R05
            // 
            this.chkL05R05.Image = null;
            this.chkL05R05.Location = new System.Drawing.Point(176, 240);
            this.chkL05R05.Name = "chkL05R05";
            this.chkL05R05.Size = new System.Drawing.Size(16, 24);
            this.chkL05R05.TabIndex = 103;
            this.chkL05R05.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL05R04
            // 
            this.chkL05R04.Image = null;
            this.chkL05R04.Location = new System.Drawing.Point(152, 240);
            this.chkL05R04.Name = "chkL05R04";
            this.chkL05R04.Size = new System.Drawing.Size(16, 24);
            this.chkL05R04.TabIndex = 102;
            this.chkL05R04.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL05R03
            // 
            this.chkL05R03.Image = null;
            this.chkL05R03.Location = new System.Drawing.Point(136, 240);
            this.chkL05R03.Name = "chkL05R03";
            this.chkL05R03.Size = new System.Drawing.Size(16, 24);
            this.chkL05R03.TabIndex = 101;
            this.chkL05R03.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL05R02
            // 
            this.chkL05R02.Image = null;
            this.chkL05R02.Location = new System.Drawing.Point(120, 240);
            this.chkL05R02.Name = "chkL05R02";
            this.chkL05R02.Size = new System.Drawing.Size(16, 24);
            this.chkL05R02.TabIndex = 100;
            this.chkL05R02.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL05R01
            // 
            this.chkL05R01.Image = null;
            this.chkL05R01.Location = new System.Drawing.Point(104, 240);
            this.chkL05R01.Name = "chkL05R01";
            this.chkL05R01.Size = new System.Drawing.Size(16, 24);
            this.chkL05R01.TabIndex = 99;
            this.chkL05R01.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL06R09
            // 
            this.chkL06R09.Image = null;
            this.chkL06R09.Location = new System.Drawing.Point(248, 264);
            this.chkL06R09.Name = "chkL06R09";
            this.chkL06R09.Size = new System.Drawing.Size(16, 24);
            this.chkL06R09.TabIndex = 130;
            this.chkL06R09.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL06R10
            // 
            this.chkL06R10.Image = null;
            this.chkL06R10.Location = new System.Drawing.Point(264, 264);
            this.chkL06R10.Name = "chkL06R10";
            this.chkL06R10.Size = new System.Drawing.Size(16, 24);
            this.chkL06R10.TabIndex = 129;
            this.chkL06R10.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL06R11
            // 
            this.chkL06R11.Image = null;
            this.chkL06R11.Location = new System.Drawing.Point(280, 264);
            this.chkL06R11.Name = "chkL06R11";
            this.chkL06R11.Size = new System.Drawing.Size(16, 24);
            this.chkL06R11.TabIndex = 128;
            this.chkL06R11.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL06R12
            // 
            this.chkL06R12.Image = null;
            this.chkL06R12.Location = new System.Drawing.Point(296, 264);
            this.chkL06R12.Name = "chkL06R12";
            this.chkL06R12.Size = new System.Drawing.Size(16, 24);
            this.chkL06R12.TabIndex = 127;
            this.chkL06R12.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL06R13
            // 
            this.chkL06R13.Image = null;
            this.chkL06R13.Location = new System.Drawing.Point(320, 264);
            this.chkL06R13.Name = "chkL06R13";
            this.chkL06R13.Size = new System.Drawing.Size(16, 24);
            this.chkL06R13.TabIndex = 126;
            this.chkL06R13.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL06R14
            // 
            this.chkL06R14.Image = null;
            this.chkL06R14.Location = new System.Drawing.Point(336, 264);
            this.chkL06R14.Name = "chkL06R14";
            this.chkL06R14.Size = new System.Drawing.Size(16, 24);
            this.chkL06R14.TabIndex = 125;
            this.chkL06R14.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL06R15
            // 
            this.chkL06R15.Image = null;
            this.chkL06R15.Location = new System.Drawing.Point(352, 264);
            this.chkL06R15.Name = "chkL06R15";
            this.chkL06R15.Size = new System.Drawing.Size(16, 24);
            this.chkL06R15.TabIndex = 124;
            this.chkL06R15.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL06R16
            // 
            this.chkL06R16.Image = null;
            this.chkL06R16.Location = new System.Drawing.Point(368, 264);
            this.chkL06R16.Name = "chkL06R16";
            this.chkL06R16.Size = new System.Drawing.Size(16, 24);
            this.chkL06R16.TabIndex = 123;
            this.chkL06R16.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL06R08
            // 
            this.chkL06R08.Image = null;
            this.chkL06R08.Location = new System.Drawing.Point(224, 264);
            this.chkL06R08.Name = "chkL06R08";
            this.chkL06R08.Size = new System.Drawing.Size(16, 24);
            this.chkL06R08.TabIndex = 122;
            this.chkL06R08.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL06R07
            // 
            this.chkL06R07.Image = null;
            this.chkL06R07.Location = new System.Drawing.Point(208, 264);
            this.chkL06R07.Name = "chkL06R07";
            this.chkL06R07.Size = new System.Drawing.Size(16, 24);
            this.chkL06R07.TabIndex = 121;
            this.chkL06R07.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL06R06
            // 
            this.chkL06R06.Image = null;
            this.chkL06R06.Location = new System.Drawing.Point(192, 264);
            this.chkL06R06.Name = "chkL06R06";
            this.chkL06R06.Size = new System.Drawing.Size(16, 24);
            this.chkL06R06.TabIndex = 120;
            this.chkL06R06.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL06R05
            // 
            this.chkL06R05.Image = null;
            this.chkL06R05.Location = new System.Drawing.Point(176, 264);
            this.chkL06R05.Name = "chkL06R05";
            this.chkL06R05.Size = new System.Drawing.Size(16, 24);
            this.chkL06R05.TabIndex = 119;
            this.chkL06R05.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL06R04
            // 
            this.chkL06R04.Image = null;
            this.chkL06R04.Location = new System.Drawing.Point(152, 264);
            this.chkL06R04.Name = "chkL06R04";
            this.chkL06R04.Size = new System.Drawing.Size(16, 24);
            this.chkL06R04.TabIndex = 118;
            this.chkL06R04.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL06R03
            // 
            this.chkL06R03.Image = null;
            this.chkL06R03.Location = new System.Drawing.Point(136, 264);
            this.chkL06R03.Name = "chkL06R03";
            this.chkL06R03.Size = new System.Drawing.Size(16, 24);
            this.chkL06R03.TabIndex = 117;
            this.chkL06R03.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL06R02
            // 
            this.chkL06R02.Image = null;
            this.chkL06R02.Location = new System.Drawing.Point(120, 264);
            this.chkL06R02.Name = "chkL06R02";
            this.chkL06R02.Size = new System.Drawing.Size(16, 24);
            this.chkL06R02.TabIndex = 116;
            this.chkL06R02.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL06R01
            // 
            this.chkL06R01.Image = null;
            this.chkL06R01.Location = new System.Drawing.Point(104, 264);
            this.chkL06R01.Name = "chkL06R01";
            this.chkL06R01.Size = new System.Drawing.Size(16, 24);
            this.chkL06R01.TabIndex = 115;
            this.chkL06R01.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL07R09
            // 
            this.chkL07R09.Image = null;
            this.chkL07R09.Location = new System.Drawing.Point(248, 288);
            this.chkL07R09.Name = "chkL07R09";
            this.chkL07R09.Size = new System.Drawing.Size(16, 24);
            this.chkL07R09.TabIndex = 146;
            this.chkL07R09.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL07R10
            // 
            this.chkL07R10.Image = null;
            this.chkL07R10.Location = new System.Drawing.Point(264, 288);
            this.chkL07R10.Name = "chkL07R10";
            this.chkL07R10.Size = new System.Drawing.Size(16, 24);
            this.chkL07R10.TabIndex = 145;
            this.chkL07R10.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL07R11
            // 
            this.chkL07R11.Image = null;
            this.chkL07R11.Location = new System.Drawing.Point(280, 288);
            this.chkL07R11.Name = "chkL07R11";
            this.chkL07R11.Size = new System.Drawing.Size(16, 24);
            this.chkL07R11.TabIndex = 144;
            this.chkL07R11.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL07R12
            // 
            this.chkL07R12.Image = null;
            this.chkL07R12.Location = new System.Drawing.Point(296, 288);
            this.chkL07R12.Name = "chkL07R12";
            this.chkL07R12.Size = new System.Drawing.Size(16, 24);
            this.chkL07R12.TabIndex = 143;
            this.chkL07R12.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL07R13
            // 
            this.chkL07R13.Image = null;
            this.chkL07R13.Location = new System.Drawing.Point(320, 288);
            this.chkL07R13.Name = "chkL07R13";
            this.chkL07R13.Size = new System.Drawing.Size(16, 24);
            this.chkL07R13.TabIndex = 142;
            this.chkL07R13.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL07R14
            // 
            this.chkL07R14.Image = null;
            this.chkL07R14.Location = new System.Drawing.Point(336, 288);
            this.chkL07R14.Name = "chkL07R14";
            this.chkL07R14.Size = new System.Drawing.Size(16, 24);
            this.chkL07R14.TabIndex = 141;
            this.chkL07R14.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL07R15
            // 
            this.chkL07R15.Image = null;
            this.chkL07R15.Location = new System.Drawing.Point(352, 288);
            this.chkL07R15.Name = "chkL07R15";
            this.chkL07R15.Size = new System.Drawing.Size(16, 24);
            this.chkL07R15.TabIndex = 140;
            this.chkL07R15.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL07R16
            // 
            this.chkL07R16.Image = null;
            this.chkL07R16.Location = new System.Drawing.Point(368, 288);
            this.chkL07R16.Name = "chkL07R16";
            this.chkL07R16.Size = new System.Drawing.Size(16, 24);
            this.chkL07R16.TabIndex = 139;
            this.chkL07R16.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL07R08
            // 
            this.chkL07R08.Image = null;
            this.chkL07R08.Location = new System.Drawing.Point(224, 288);
            this.chkL07R08.Name = "chkL07R08";
            this.chkL07R08.Size = new System.Drawing.Size(16, 24);
            this.chkL07R08.TabIndex = 138;
            this.chkL07R08.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL07R07
            // 
            this.chkL07R07.Image = null;
            this.chkL07R07.Location = new System.Drawing.Point(208, 288);
            this.chkL07R07.Name = "chkL07R07";
            this.chkL07R07.Size = new System.Drawing.Size(16, 24);
            this.chkL07R07.TabIndex = 137;
            this.chkL07R07.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL07R06
            // 
            this.chkL07R06.Image = null;
            this.chkL07R06.Location = new System.Drawing.Point(192, 288);
            this.chkL07R06.Name = "chkL07R06";
            this.chkL07R06.Size = new System.Drawing.Size(16, 24);
            this.chkL07R06.TabIndex = 136;
            this.chkL07R06.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL07R05
            // 
            this.chkL07R05.Image = null;
            this.chkL07R05.Location = new System.Drawing.Point(176, 288);
            this.chkL07R05.Name = "chkL07R05";
            this.chkL07R05.Size = new System.Drawing.Size(16, 24);
            this.chkL07R05.TabIndex = 135;
            this.chkL07R05.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL07R04
            // 
            this.chkL07R04.Image = null;
            this.chkL07R04.Location = new System.Drawing.Point(152, 288);
            this.chkL07R04.Name = "chkL07R04";
            this.chkL07R04.Size = new System.Drawing.Size(16, 24);
            this.chkL07R04.TabIndex = 134;
            this.chkL07R04.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL07R03
            // 
            this.chkL07R03.Image = null;
            this.chkL07R03.Location = new System.Drawing.Point(136, 288);
            this.chkL07R03.Name = "chkL07R03";
            this.chkL07R03.Size = new System.Drawing.Size(16, 24);
            this.chkL07R03.TabIndex = 133;
            this.chkL07R03.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL07R02
            // 
            this.chkL07R02.Image = null;
            this.chkL07R02.Location = new System.Drawing.Point(120, 288);
            this.chkL07R02.Name = "chkL07R02";
            this.chkL07R02.Size = new System.Drawing.Size(16, 24);
            this.chkL07R02.TabIndex = 132;
            this.chkL07R02.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL07R01
            // 
            this.chkL07R01.Image = null;
            this.chkL07R01.Location = new System.Drawing.Point(104, 288);
            this.chkL07R01.Name = "chkL07R01";
            this.chkL07R01.Size = new System.Drawing.Size(16, 24);
            this.chkL07R01.TabIndex = 131;
            this.chkL07R01.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL08R09
            // 
            this.chkL08R09.Image = null;
            this.chkL08R09.Location = new System.Drawing.Point(248, 320);
            this.chkL08R09.Name = "chkL08R09";
            this.chkL08R09.Size = new System.Drawing.Size(16, 24);
            this.chkL08R09.TabIndex = 162;
            this.chkL08R09.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL08R10
            // 
            this.chkL08R10.Image = null;
            this.chkL08R10.Location = new System.Drawing.Point(264, 320);
            this.chkL08R10.Name = "chkL08R10";
            this.chkL08R10.Size = new System.Drawing.Size(16, 24);
            this.chkL08R10.TabIndex = 161;
            this.chkL08R10.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL08R11
            // 
            this.chkL08R11.Image = null;
            this.chkL08R11.Location = new System.Drawing.Point(280, 320);
            this.chkL08R11.Name = "chkL08R11";
            this.chkL08R11.Size = new System.Drawing.Size(16, 24);
            this.chkL08R11.TabIndex = 160;
            this.chkL08R11.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL08R12
            // 
            this.chkL08R12.Image = null;
            this.chkL08R12.Location = new System.Drawing.Point(296, 320);
            this.chkL08R12.Name = "chkL08R12";
            this.chkL08R12.Size = new System.Drawing.Size(16, 24);
            this.chkL08R12.TabIndex = 159;
            this.chkL08R12.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL08R13
            // 
            this.chkL08R13.Image = null;
            this.chkL08R13.Location = new System.Drawing.Point(320, 320);
            this.chkL08R13.Name = "chkL08R13";
            this.chkL08R13.Size = new System.Drawing.Size(16, 24);
            this.chkL08R13.TabIndex = 158;
            this.chkL08R13.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL08R14
            // 
            this.chkL08R14.Image = null;
            this.chkL08R14.Location = new System.Drawing.Point(336, 320);
            this.chkL08R14.Name = "chkL08R14";
            this.chkL08R14.Size = new System.Drawing.Size(16, 24);
            this.chkL08R14.TabIndex = 157;
            this.chkL08R14.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL08R15
            // 
            this.chkL08R15.Image = null;
            this.chkL08R15.Location = new System.Drawing.Point(352, 320);
            this.chkL08R15.Name = "chkL08R15";
            this.chkL08R15.Size = new System.Drawing.Size(16, 24);
            this.chkL08R15.TabIndex = 156;
            this.chkL08R15.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL08R16
            // 
            this.chkL08R16.Image = null;
            this.chkL08R16.Location = new System.Drawing.Point(368, 320);
            this.chkL08R16.Name = "chkL08R16";
            this.chkL08R16.Size = new System.Drawing.Size(16, 24);
            this.chkL08R16.TabIndex = 155;
            this.chkL08R16.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL08R08
            // 
            this.chkL08R08.Image = null;
            this.chkL08R08.Location = new System.Drawing.Point(224, 320);
            this.chkL08R08.Name = "chkL08R08";
            this.chkL08R08.Size = new System.Drawing.Size(16, 24);
            this.chkL08R08.TabIndex = 154;
            this.chkL08R08.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL08R07
            // 
            this.chkL08R07.Image = null;
            this.chkL08R07.Location = new System.Drawing.Point(208, 320);
            this.chkL08R07.Name = "chkL08R07";
            this.chkL08R07.Size = new System.Drawing.Size(16, 24);
            this.chkL08R07.TabIndex = 153;
            this.chkL08R07.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL08R06
            // 
            this.chkL08R06.Image = null;
            this.chkL08R06.Location = new System.Drawing.Point(192, 320);
            this.chkL08R06.Name = "chkL08R06";
            this.chkL08R06.Size = new System.Drawing.Size(16, 24);
            this.chkL08R06.TabIndex = 152;
            this.chkL08R06.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL08R05
            // 
            this.chkL08R05.Image = null;
            this.chkL08R05.Location = new System.Drawing.Point(176, 320);
            this.chkL08R05.Name = "chkL08R05";
            this.chkL08R05.Size = new System.Drawing.Size(16, 24);
            this.chkL08R05.TabIndex = 151;
            this.chkL08R05.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL08R04
            // 
            this.chkL08R04.Image = null;
            this.chkL08R04.Location = new System.Drawing.Point(152, 320);
            this.chkL08R04.Name = "chkL08R04";
            this.chkL08R04.Size = new System.Drawing.Size(16, 24);
            this.chkL08R04.TabIndex = 150;
            this.chkL08R04.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL08R03
            // 
            this.chkL08R03.Image = null;
            this.chkL08R03.Location = new System.Drawing.Point(136, 320);
            this.chkL08R03.Name = "chkL08R03";
            this.chkL08R03.Size = new System.Drawing.Size(16, 24);
            this.chkL08R03.TabIndex = 149;
            this.chkL08R03.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL08R02
            // 
            this.chkL08R02.Image = null;
            this.chkL08R02.Location = new System.Drawing.Point(120, 320);
            this.chkL08R02.Name = "chkL08R02";
            this.chkL08R02.Size = new System.Drawing.Size(16, 24);
            this.chkL08R02.TabIndex = 148;
            this.chkL08R02.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL08R01
            // 
            this.chkL08R01.Image = null;
            this.chkL08R01.Location = new System.Drawing.Point(104, 320);
            this.chkL08R01.Name = "chkL08R01";
            this.chkL08R01.Size = new System.Drawing.Size(16, 24);
            this.chkL08R01.TabIndex = 147;
            this.chkL08R01.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL09R09
            // 
            this.chkL09R09.Image = null;
            this.chkL09R09.Location = new System.Drawing.Point(248, 344);
            this.chkL09R09.Name = "chkL09R09";
            this.chkL09R09.Size = new System.Drawing.Size(16, 24);
            this.chkL09R09.TabIndex = 178;
            this.chkL09R09.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL09R10
            // 
            this.chkL09R10.Image = null;
            this.chkL09R10.Location = new System.Drawing.Point(264, 344);
            this.chkL09R10.Name = "chkL09R10";
            this.chkL09R10.Size = new System.Drawing.Size(16, 24);
            this.chkL09R10.TabIndex = 177;
            this.chkL09R10.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL09R11
            // 
            this.chkL09R11.Image = null;
            this.chkL09R11.Location = new System.Drawing.Point(280, 344);
            this.chkL09R11.Name = "chkL09R11";
            this.chkL09R11.Size = new System.Drawing.Size(16, 24);
            this.chkL09R11.TabIndex = 176;
            this.chkL09R11.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL09R12
            // 
            this.chkL09R12.Image = null;
            this.chkL09R12.Location = new System.Drawing.Point(296, 344);
            this.chkL09R12.Name = "chkL09R12";
            this.chkL09R12.Size = new System.Drawing.Size(16, 24);
            this.chkL09R12.TabIndex = 175;
            this.chkL09R12.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL09R13
            // 
            this.chkL09R13.Image = null;
            this.chkL09R13.Location = new System.Drawing.Point(320, 344);
            this.chkL09R13.Name = "chkL09R13";
            this.chkL09R13.Size = new System.Drawing.Size(16, 24);
            this.chkL09R13.TabIndex = 174;
            this.chkL09R13.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL09R14
            // 
            this.chkL09R14.Image = null;
            this.chkL09R14.Location = new System.Drawing.Point(336, 344);
            this.chkL09R14.Name = "chkL09R14";
            this.chkL09R14.Size = new System.Drawing.Size(16, 24);
            this.chkL09R14.TabIndex = 173;
            this.chkL09R14.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL09R15
            // 
            this.chkL09R15.Image = null;
            this.chkL09R15.Location = new System.Drawing.Point(352, 344);
            this.chkL09R15.Name = "chkL09R15";
            this.chkL09R15.Size = new System.Drawing.Size(16, 24);
            this.chkL09R15.TabIndex = 172;
            this.chkL09R15.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL09R16
            // 
            this.chkL09R16.Image = null;
            this.chkL09R16.Location = new System.Drawing.Point(368, 344);
            this.chkL09R16.Name = "chkL09R16";
            this.chkL09R16.Size = new System.Drawing.Size(16, 24);
            this.chkL09R16.TabIndex = 171;
            this.chkL09R16.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL09R08
            // 
            this.chkL09R08.Image = null;
            this.chkL09R08.Location = new System.Drawing.Point(224, 344);
            this.chkL09R08.Name = "chkL09R08";
            this.chkL09R08.Size = new System.Drawing.Size(16, 24);
            this.chkL09R08.TabIndex = 170;
            this.chkL09R08.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL09R07
            // 
            this.chkL09R07.Image = null;
            this.chkL09R07.Location = new System.Drawing.Point(208, 344);
            this.chkL09R07.Name = "chkL09R07";
            this.chkL09R07.Size = new System.Drawing.Size(16, 24);
            this.chkL09R07.TabIndex = 169;
            this.chkL09R07.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL09R06
            // 
            this.chkL09R06.Image = null;
            this.chkL09R06.Location = new System.Drawing.Point(192, 344);
            this.chkL09R06.Name = "chkL09R06";
            this.chkL09R06.Size = new System.Drawing.Size(16, 24);
            this.chkL09R06.TabIndex = 168;
            this.chkL09R06.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL09R05
            // 
            this.chkL09R05.Image = null;
            this.chkL09R05.Location = new System.Drawing.Point(176, 344);
            this.chkL09R05.Name = "chkL09R05";
            this.chkL09R05.Size = new System.Drawing.Size(16, 24);
            this.chkL09R05.TabIndex = 167;
            this.chkL09R05.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL09R04
            // 
            this.chkL09R04.Image = null;
            this.chkL09R04.Location = new System.Drawing.Point(152, 344);
            this.chkL09R04.Name = "chkL09R04";
            this.chkL09R04.Size = new System.Drawing.Size(16, 24);
            this.chkL09R04.TabIndex = 166;
            this.chkL09R04.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL09R03
            // 
            this.chkL09R03.Image = null;
            this.chkL09R03.Location = new System.Drawing.Point(136, 344);
            this.chkL09R03.Name = "chkL09R03";
            this.chkL09R03.Size = new System.Drawing.Size(16, 24);
            this.chkL09R03.TabIndex = 165;
            this.chkL09R03.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL09R02
            // 
            this.chkL09R02.Image = null;
            this.chkL09R02.Location = new System.Drawing.Point(120, 344);
            this.chkL09R02.Name = "chkL09R02";
            this.chkL09R02.Size = new System.Drawing.Size(16, 24);
            this.chkL09R02.TabIndex = 164;
            this.chkL09R02.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL09R01
            // 
            this.chkL09R01.Image = null;
            this.chkL09R01.Location = new System.Drawing.Point(104, 344);
            this.chkL09R01.Name = "chkL09R01";
            this.chkL09R01.Size = new System.Drawing.Size(16, 24);
            this.chkL09R01.TabIndex = 163;
            this.chkL09R01.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL10R09
            // 
            this.chkL10R09.Image = null;
            this.chkL10R09.Location = new System.Drawing.Point(248, 368);
            this.chkL10R09.Name = "chkL10R09";
            this.chkL10R09.Size = new System.Drawing.Size(16, 24);
            this.chkL10R09.TabIndex = 194;
            this.chkL10R09.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL10R10
            // 
            this.chkL10R10.Image = null;
            this.chkL10R10.Location = new System.Drawing.Point(264, 368);
            this.chkL10R10.Name = "chkL10R10";
            this.chkL10R10.Size = new System.Drawing.Size(16, 24);
            this.chkL10R10.TabIndex = 193;
            this.chkL10R10.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL10R11
            // 
            this.chkL10R11.Image = null;
            this.chkL10R11.Location = new System.Drawing.Point(280, 368);
            this.chkL10R11.Name = "chkL10R11";
            this.chkL10R11.Size = new System.Drawing.Size(16, 24);
            this.chkL10R11.TabIndex = 192;
            this.chkL10R11.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL10R12
            // 
            this.chkL10R12.Image = null;
            this.chkL10R12.Location = new System.Drawing.Point(296, 368);
            this.chkL10R12.Name = "chkL10R12";
            this.chkL10R12.Size = new System.Drawing.Size(16, 24);
            this.chkL10R12.TabIndex = 191;
            this.chkL10R12.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL10R13
            // 
            this.chkL10R13.Image = null;
            this.chkL10R13.Location = new System.Drawing.Point(320, 368);
            this.chkL10R13.Name = "chkL10R13";
            this.chkL10R13.Size = new System.Drawing.Size(16, 24);
            this.chkL10R13.TabIndex = 190;
            this.chkL10R13.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL10R14
            // 
            this.chkL10R14.Image = null;
            this.chkL10R14.Location = new System.Drawing.Point(336, 368);
            this.chkL10R14.Name = "chkL10R14";
            this.chkL10R14.Size = new System.Drawing.Size(16, 24);
            this.chkL10R14.TabIndex = 189;
            this.chkL10R14.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL10R15
            // 
            this.chkL10R15.Image = null;
            this.chkL10R15.Location = new System.Drawing.Point(352, 368);
            this.chkL10R15.Name = "chkL10R15";
            this.chkL10R15.Size = new System.Drawing.Size(16, 24);
            this.chkL10R15.TabIndex = 188;
            this.chkL10R15.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL10R16
            // 
            this.chkL10R16.Image = null;
            this.chkL10R16.Location = new System.Drawing.Point(368, 368);
            this.chkL10R16.Name = "chkL10R16";
            this.chkL10R16.Size = new System.Drawing.Size(16, 24);
            this.chkL10R16.TabIndex = 187;
            this.chkL10R16.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL10R08
            // 
            this.chkL10R08.Image = null;
            this.chkL10R08.Location = new System.Drawing.Point(224, 368);
            this.chkL10R08.Name = "chkL10R08";
            this.chkL10R08.Size = new System.Drawing.Size(16, 24);
            this.chkL10R08.TabIndex = 186;
            this.chkL10R08.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL10R07
            // 
            this.chkL10R07.Image = null;
            this.chkL10R07.Location = new System.Drawing.Point(208, 368);
            this.chkL10R07.Name = "chkL10R07";
            this.chkL10R07.Size = new System.Drawing.Size(16, 24);
            this.chkL10R07.TabIndex = 185;
            this.chkL10R07.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL10R06
            // 
            this.chkL10R06.Image = null;
            this.chkL10R06.Location = new System.Drawing.Point(192, 368);
            this.chkL10R06.Name = "chkL10R06";
            this.chkL10R06.Size = new System.Drawing.Size(16, 24);
            this.chkL10R06.TabIndex = 184;
            this.chkL10R06.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL10R05
            // 
            this.chkL10R05.Image = null;
            this.chkL10R05.Location = new System.Drawing.Point(176, 368);
            this.chkL10R05.Name = "chkL10R05";
            this.chkL10R05.Size = new System.Drawing.Size(16, 24);
            this.chkL10R05.TabIndex = 183;
            this.chkL10R05.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL10R04
            // 
            this.chkL10R04.Image = null;
            this.chkL10R04.Location = new System.Drawing.Point(152, 368);
            this.chkL10R04.Name = "chkL10R04";
            this.chkL10R04.Size = new System.Drawing.Size(16, 24);
            this.chkL10R04.TabIndex = 182;
            this.chkL10R04.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL10R03
            // 
            this.chkL10R03.Image = null;
            this.chkL10R03.Location = new System.Drawing.Point(136, 368);
            this.chkL10R03.Name = "chkL10R03";
            this.chkL10R03.Size = new System.Drawing.Size(16, 24);
            this.chkL10R03.TabIndex = 181;
            this.chkL10R03.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL10R02
            // 
            this.chkL10R02.Image = null;
            this.chkL10R02.Location = new System.Drawing.Point(120, 368);
            this.chkL10R02.Name = "chkL10R02";
            this.chkL10R02.Size = new System.Drawing.Size(16, 24);
            this.chkL10R02.TabIndex = 180;
            this.chkL10R02.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL10R01
            // 
            this.chkL10R01.Image = null;
            this.chkL10R01.Location = new System.Drawing.Point(104, 368);
            this.chkL10R01.Name = "chkL10R01";
            this.chkL10R01.Size = new System.Drawing.Size(16, 24);
            this.chkL10R01.TabIndex = 179;
            this.chkL10R01.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL11R09
            // 
            this.chkL11R09.Image = null;
            this.chkL11R09.Location = new System.Drawing.Point(248, 392);
            this.chkL11R09.Name = "chkL11R09";
            this.chkL11R09.Size = new System.Drawing.Size(16, 24);
            this.chkL11R09.TabIndex = 210;
            this.chkL11R09.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL11R10
            // 
            this.chkL11R10.Image = null;
            this.chkL11R10.Location = new System.Drawing.Point(264, 392);
            this.chkL11R10.Name = "chkL11R10";
            this.chkL11R10.Size = new System.Drawing.Size(16, 24);
            this.chkL11R10.TabIndex = 209;
            this.chkL11R10.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL11R11
            // 
            this.chkL11R11.Image = null;
            this.chkL11R11.Location = new System.Drawing.Point(280, 392);
            this.chkL11R11.Name = "chkL11R11";
            this.chkL11R11.Size = new System.Drawing.Size(16, 24);
            this.chkL11R11.TabIndex = 208;
            this.chkL11R11.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL11R12
            // 
            this.chkL11R12.Image = null;
            this.chkL11R12.Location = new System.Drawing.Point(296, 392);
            this.chkL11R12.Name = "chkL11R12";
            this.chkL11R12.Size = new System.Drawing.Size(16, 24);
            this.chkL11R12.TabIndex = 207;
            this.chkL11R12.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL11R13
            // 
            this.chkL11R13.Image = null;
            this.chkL11R13.Location = new System.Drawing.Point(320, 392);
            this.chkL11R13.Name = "chkL11R13";
            this.chkL11R13.Size = new System.Drawing.Size(16, 24);
            this.chkL11R13.TabIndex = 206;
            this.chkL11R13.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL11R14
            // 
            this.chkL11R14.Image = null;
            this.chkL11R14.Location = new System.Drawing.Point(336, 392);
            this.chkL11R14.Name = "chkL11R14";
            this.chkL11R14.Size = new System.Drawing.Size(16, 24);
            this.chkL11R14.TabIndex = 205;
            this.chkL11R14.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL11R15
            // 
            this.chkL11R15.Image = null;
            this.chkL11R15.Location = new System.Drawing.Point(352, 392);
            this.chkL11R15.Name = "chkL11R15";
            this.chkL11R15.Size = new System.Drawing.Size(16, 24);
            this.chkL11R15.TabIndex = 204;
            this.chkL11R15.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL11R16
            // 
            this.chkL11R16.Image = null;
            this.chkL11R16.Location = new System.Drawing.Point(368, 392);
            this.chkL11R16.Name = "chkL11R16";
            this.chkL11R16.Size = new System.Drawing.Size(16, 24);
            this.chkL11R16.TabIndex = 203;
            this.chkL11R16.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL11R08
            // 
            this.chkL11R08.Image = null;
            this.chkL11R08.Location = new System.Drawing.Point(224, 392);
            this.chkL11R08.Name = "chkL11R08";
            this.chkL11R08.Size = new System.Drawing.Size(16, 24);
            this.chkL11R08.TabIndex = 202;
            this.chkL11R08.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL11R07
            // 
            this.chkL11R07.Image = null;
            this.chkL11R07.Location = new System.Drawing.Point(208, 392);
            this.chkL11R07.Name = "chkL11R07";
            this.chkL11R07.Size = new System.Drawing.Size(16, 24);
            this.chkL11R07.TabIndex = 201;
            this.chkL11R07.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL11R06
            // 
            this.chkL11R06.Image = null;
            this.chkL11R06.Location = new System.Drawing.Point(192, 392);
            this.chkL11R06.Name = "chkL11R06";
            this.chkL11R06.Size = new System.Drawing.Size(16, 24);
            this.chkL11R06.TabIndex = 200;
            this.chkL11R06.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL11R05
            // 
            this.chkL11R05.Image = null;
            this.chkL11R05.Location = new System.Drawing.Point(176, 392);
            this.chkL11R05.Name = "chkL11R05";
            this.chkL11R05.Size = new System.Drawing.Size(16, 24);
            this.chkL11R05.TabIndex = 199;
            this.chkL11R05.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL11R04
            // 
            this.chkL11R04.Image = null;
            this.chkL11R04.Location = new System.Drawing.Point(152, 392);
            this.chkL11R04.Name = "chkL11R04";
            this.chkL11R04.Size = new System.Drawing.Size(16, 24);
            this.chkL11R04.TabIndex = 198;
            this.chkL11R04.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL11R03
            // 
            this.chkL11R03.Image = null;
            this.chkL11R03.Location = new System.Drawing.Point(136, 392);
            this.chkL11R03.Name = "chkL11R03";
            this.chkL11R03.Size = new System.Drawing.Size(16, 24);
            this.chkL11R03.TabIndex = 197;
            this.chkL11R03.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL11R02
            // 
            this.chkL11R02.Image = null;
            this.chkL11R02.Location = new System.Drawing.Point(120, 392);
            this.chkL11R02.Name = "chkL11R02";
            this.chkL11R02.Size = new System.Drawing.Size(16, 24);
            this.chkL11R02.TabIndex = 196;
            this.chkL11R02.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL11R01
            // 
            this.chkL11R01.Image = null;
            this.chkL11R01.Location = new System.Drawing.Point(104, 392);
            this.chkL11R01.Name = "chkL11R01";
            this.chkL11R01.Size = new System.Drawing.Size(16, 24);
            this.chkL11R01.TabIndex = 195;
            this.chkL11R01.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL12R09
            // 
            this.chkL12R09.Image = null;
            this.chkL12R09.Location = new System.Drawing.Point(248, 424);
            this.chkL12R09.Name = "chkL12R09";
            this.chkL12R09.Size = new System.Drawing.Size(16, 24);
            this.chkL12R09.TabIndex = 226;
            this.chkL12R09.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL12R10
            // 
            this.chkL12R10.Image = null;
            this.chkL12R10.Location = new System.Drawing.Point(264, 424);
            this.chkL12R10.Name = "chkL12R10";
            this.chkL12R10.Size = new System.Drawing.Size(16, 24);
            this.chkL12R10.TabIndex = 225;
            this.chkL12R10.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL12R11
            // 
            this.chkL12R11.Image = null;
            this.chkL12R11.Location = new System.Drawing.Point(280, 424);
            this.chkL12R11.Name = "chkL12R11";
            this.chkL12R11.Size = new System.Drawing.Size(16, 24);
            this.chkL12R11.TabIndex = 224;
            this.chkL12R11.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL12R12
            // 
            this.chkL12R12.Image = null;
            this.chkL12R12.Location = new System.Drawing.Point(296, 424);
            this.chkL12R12.Name = "chkL12R12";
            this.chkL12R12.Size = new System.Drawing.Size(16, 24);
            this.chkL12R12.TabIndex = 223;
            this.chkL12R12.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL12R13
            // 
            this.chkL12R13.Image = null;
            this.chkL12R13.Location = new System.Drawing.Point(320, 424);
            this.chkL12R13.Name = "chkL12R13";
            this.chkL12R13.Size = new System.Drawing.Size(16, 24);
            this.chkL12R13.TabIndex = 222;
            this.chkL12R13.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL12R14
            // 
            this.chkL12R14.Image = null;
            this.chkL12R14.Location = new System.Drawing.Point(336, 424);
            this.chkL12R14.Name = "chkL12R14";
            this.chkL12R14.Size = new System.Drawing.Size(16, 24);
            this.chkL12R14.TabIndex = 221;
            this.chkL12R14.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL12R15
            // 
            this.chkL12R15.Image = null;
            this.chkL12R15.Location = new System.Drawing.Point(352, 424);
            this.chkL12R15.Name = "chkL12R15";
            this.chkL12R15.Size = new System.Drawing.Size(16, 24);
            this.chkL12R15.TabIndex = 220;
            this.chkL12R15.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL12R16
            // 
            this.chkL12R16.Image = null;
            this.chkL12R16.Location = new System.Drawing.Point(368, 424);
            this.chkL12R16.Name = "chkL12R16";
            this.chkL12R16.Size = new System.Drawing.Size(16, 24);
            this.chkL12R16.TabIndex = 219;
            this.chkL12R16.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL12R08
            // 
            this.chkL12R08.Image = null;
            this.chkL12R08.Location = new System.Drawing.Point(224, 424);
            this.chkL12R08.Name = "chkL12R08";
            this.chkL12R08.Size = new System.Drawing.Size(16, 24);
            this.chkL12R08.TabIndex = 218;
            this.chkL12R08.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL12R07
            // 
            this.chkL12R07.Image = null;
            this.chkL12R07.Location = new System.Drawing.Point(208, 424);
            this.chkL12R07.Name = "chkL12R07";
            this.chkL12R07.Size = new System.Drawing.Size(16, 24);
            this.chkL12R07.TabIndex = 217;
            this.chkL12R07.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL12R06
            // 
            this.chkL12R06.Image = null;
            this.chkL12R06.Location = new System.Drawing.Point(192, 424);
            this.chkL12R06.Name = "chkL12R06";
            this.chkL12R06.Size = new System.Drawing.Size(16, 24);
            this.chkL12R06.TabIndex = 216;
            this.chkL12R06.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL12R05
            // 
            this.chkL12R05.Image = null;
            this.chkL12R05.Location = new System.Drawing.Point(176, 424);
            this.chkL12R05.Name = "chkL12R05";
            this.chkL12R05.Size = new System.Drawing.Size(16, 24);
            this.chkL12R05.TabIndex = 215;
            this.chkL12R05.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL12R04
            // 
            this.chkL12R04.Image = null;
            this.chkL12R04.Location = new System.Drawing.Point(152, 424);
            this.chkL12R04.Name = "chkL12R04";
            this.chkL12R04.Size = new System.Drawing.Size(16, 24);
            this.chkL12R04.TabIndex = 214;
            this.chkL12R04.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL12R03
            // 
            this.chkL12R03.Image = null;
            this.chkL12R03.Location = new System.Drawing.Point(136, 424);
            this.chkL12R03.Name = "chkL12R03";
            this.chkL12R03.Size = new System.Drawing.Size(16, 24);
            this.chkL12R03.TabIndex = 213;
            this.chkL12R03.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL12R02
            // 
            this.chkL12R02.Image = null;
            this.chkL12R02.Location = new System.Drawing.Point(120, 424);
            this.chkL12R02.Name = "chkL12R02";
            this.chkL12R02.Size = new System.Drawing.Size(16, 24);
            this.chkL12R02.TabIndex = 212;
            this.chkL12R02.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL12R01
            // 
            this.chkL12R01.Image = null;
            this.chkL12R01.Location = new System.Drawing.Point(104, 424);
            this.chkL12R01.Name = "chkL12R01";
            this.chkL12R01.Size = new System.Drawing.Size(16, 24);
            this.chkL12R01.TabIndex = 211;
            this.chkL12R01.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL13R09
            // 
            this.chkL13R09.Image = null;
            this.chkL13R09.Location = new System.Drawing.Point(248, 448);
            this.chkL13R09.Name = "chkL13R09";
            this.chkL13R09.Size = new System.Drawing.Size(16, 24);
            this.chkL13R09.TabIndex = 242;
            this.chkL13R09.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL13R10
            // 
            this.chkL13R10.Image = null;
            this.chkL13R10.Location = new System.Drawing.Point(264, 448);
            this.chkL13R10.Name = "chkL13R10";
            this.chkL13R10.Size = new System.Drawing.Size(16, 24);
            this.chkL13R10.TabIndex = 241;
            this.chkL13R10.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL13R11
            // 
            this.chkL13R11.Image = null;
            this.chkL13R11.Location = new System.Drawing.Point(280, 448);
            this.chkL13R11.Name = "chkL13R11";
            this.chkL13R11.Size = new System.Drawing.Size(16, 24);
            this.chkL13R11.TabIndex = 240;
            this.chkL13R11.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL13R12
            // 
            this.chkL13R12.Image = null;
            this.chkL13R12.Location = new System.Drawing.Point(296, 448);
            this.chkL13R12.Name = "chkL13R12";
            this.chkL13R12.Size = new System.Drawing.Size(16, 24);
            this.chkL13R12.TabIndex = 239;
            this.chkL13R12.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL13R13
            // 
            this.chkL13R13.Image = null;
            this.chkL13R13.Location = new System.Drawing.Point(320, 448);
            this.chkL13R13.Name = "chkL13R13";
            this.chkL13R13.Size = new System.Drawing.Size(16, 24);
            this.chkL13R13.TabIndex = 238;
            this.chkL13R13.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL13R14
            // 
            this.chkL13R14.Image = null;
            this.chkL13R14.Location = new System.Drawing.Point(336, 448);
            this.chkL13R14.Name = "chkL13R14";
            this.chkL13R14.Size = new System.Drawing.Size(16, 24);
            this.chkL13R14.TabIndex = 237;
            this.chkL13R14.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL13R15
            // 
            this.chkL13R15.Image = null;
            this.chkL13R15.Location = new System.Drawing.Point(352, 448);
            this.chkL13R15.Name = "chkL13R15";
            this.chkL13R15.Size = new System.Drawing.Size(16, 24);
            this.chkL13R15.TabIndex = 236;
            this.chkL13R15.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL13R16
            // 
            this.chkL13R16.Image = null;
            this.chkL13R16.Location = new System.Drawing.Point(368, 448);
            this.chkL13R16.Name = "chkL13R16";
            this.chkL13R16.Size = new System.Drawing.Size(16, 24);
            this.chkL13R16.TabIndex = 235;
            this.chkL13R16.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL13R08
            // 
            this.chkL13R08.Image = null;
            this.chkL13R08.Location = new System.Drawing.Point(224, 448);
            this.chkL13R08.Name = "chkL13R08";
            this.chkL13R08.Size = new System.Drawing.Size(16, 24);
            this.chkL13R08.TabIndex = 234;
            this.chkL13R08.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL13R07
            // 
            this.chkL13R07.Image = null;
            this.chkL13R07.Location = new System.Drawing.Point(208, 448);
            this.chkL13R07.Name = "chkL13R07";
            this.chkL13R07.Size = new System.Drawing.Size(16, 24);
            this.chkL13R07.TabIndex = 233;
            this.chkL13R07.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL13R06
            // 
            this.chkL13R06.Image = null;
            this.chkL13R06.Location = new System.Drawing.Point(192, 448);
            this.chkL13R06.Name = "chkL13R06";
            this.chkL13R06.Size = new System.Drawing.Size(16, 24);
            this.chkL13R06.TabIndex = 232;
            this.chkL13R06.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL13R05
            // 
            this.chkL13R05.Image = null;
            this.chkL13R05.Location = new System.Drawing.Point(176, 448);
            this.chkL13R05.Name = "chkL13R05";
            this.chkL13R05.Size = new System.Drawing.Size(16, 24);
            this.chkL13R05.TabIndex = 231;
            this.chkL13R05.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL13R04
            // 
            this.chkL13R04.Image = null;
            this.chkL13R04.Location = new System.Drawing.Point(152, 448);
            this.chkL13R04.Name = "chkL13R04";
            this.chkL13R04.Size = new System.Drawing.Size(16, 24);
            this.chkL13R04.TabIndex = 230;
            this.chkL13R04.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL13R03
            // 
            this.chkL13R03.Image = null;
            this.chkL13R03.Location = new System.Drawing.Point(136, 448);
            this.chkL13R03.Name = "chkL13R03";
            this.chkL13R03.Size = new System.Drawing.Size(16, 24);
            this.chkL13R03.TabIndex = 229;
            this.chkL13R03.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL13R02
            // 
            this.chkL13R02.Image = null;
            this.chkL13R02.Location = new System.Drawing.Point(120, 448);
            this.chkL13R02.Name = "chkL13R02";
            this.chkL13R02.Size = new System.Drawing.Size(16, 24);
            this.chkL13R02.TabIndex = 228;
            this.chkL13R02.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL13R01
            // 
            this.chkL13R01.Image = null;
            this.chkL13R01.Location = new System.Drawing.Point(104, 448);
            this.chkL13R01.Name = "chkL13R01";
            this.chkL13R01.Size = new System.Drawing.Size(16, 24);
            this.chkL13R01.TabIndex = 227;
            this.chkL13R01.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL14R09
            // 
            this.chkL14R09.Image = null;
            this.chkL14R09.Location = new System.Drawing.Point(248, 472);
            this.chkL14R09.Name = "chkL14R09";
            this.chkL14R09.Size = new System.Drawing.Size(16, 24);
            this.chkL14R09.TabIndex = 258;
            this.chkL14R09.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL14R10
            // 
            this.chkL14R10.Image = null;
            this.chkL14R10.Location = new System.Drawing.Point(264, 472);
            this.chkL14R10.Name = "chkL14R10";
            this.chkL14R10.Size = new System.Drawing.Size(16, 24);
            this.chkL14R10.TabIndex = 257;
            this.chkL14R10.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL14R11
            // 
            this.chkL14R11.Image = null;
            this.chkL14R11.Location = new System.Drawing.Point(280, 472);
            this.chkL14R11.Name = "chkL14R11";
            this.chkL14R11.Size = new System.Drawing.Size(16, 24);
            this.chkL14R11.TabIndex = 256;
            this.chkL14R11.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL14R12
            // 
            this.chkL14R12.Image = null;
            this.chkL14R12.Location = new System.Drawing.Point(296, 472);
            this.chkL14R12.Name = "chkL14R12";
            this.chkL14R12.Size = new System.Drawing.Size(16, 24);
            this.chkL14R12.TabIndex = 255;
            this.chkL14R12.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL14R13
            // 
            this.chkL14R13.Image = null;
            this.chkL14R13.Location = new System.Drawing.Point(320, 472);
            this.chkL14R13.Name = "chkL14R13";
            this.chkL14R13.Size = new System.Drawing.Size(16, 24);
            this.chkL14R13.TabIndex = 254;
            this.chkL14R13.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL14R14
            // 
            this.chkL14R14.Image = null;
            this.chkL14R14.Location = new System.Drawing.Point(336, 472);
            this.chkL14R14.Name = "chkL14R14";
            this.chkL14R14.Size = new System.Drawing.Size(16, 24);
            this.chkL14R14.TabIndex = 253;
            this.chkL14R14.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL14R15
            // 
            this.chkL14R15.Image = null;
            this.chkL14R15.Location = new System.Drawing.Point(352, 472);
            this.chkL14R15.Name = "chkL14R15";
            this.chkL14R15.Size = new System.Drawing.Size(16, 24);
            this.chkL14R15.TabIndex = 252;
            this.chkL14R15.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL14R16
            // 
            this.chkL14R16.Image = null;
            this.chkL14R16.Location = new System.Drawing.Point(368, 472);
            this.chkL14R16.Name = "chkL14R16";
            this.chkL14R16.Size = new System.Drawing.Size(16, 24);
            this.chkL14R16.TabIndex = 251;
            this.chkL14R16.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL14R08
            // 
            this.chkL14R08.Image = null;
            this.chkL14R08.Location = new System.Drawing.Point(224, 472);
            this.chkL14R08.Name = "chkL14R08";
            this.chkL14R08.Size = new System.Drawing.Size(16, 24);
            this.chkL14R08.TabIndex = 250;
            this.chkL14R08.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL14R07
            // 
            this.chkL14R07.Image = null;
            this.chkL14R07.Location = new System.Drawing.Point(208, 472);
            this.chkL14R07.Name = "chkL14R07";
            this.chkL14R07.Size = new System.Drawing.Size(16, 24);
            this.chkL14R07.TabIndex = 249;
            this.chkL14R07.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL14R06
            // 
            this.chkL14R06.Image = null;
            this.chkL14R06.Location = new System.Drawing.Point(192, 472);
            this.chkL14R06.Name = "chkL14R06";
            this.chkL14R06.Size = new System.Drawing.Size(16, 24);
            this.chkL14R06.TabIndex = 248;
            this.chkL14R06.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL14R05
            // 
            this.chkL14R05.Image = null;
            this.chkL14R05.Location = new System.Drawing.Point(176, 472);
            this.chkL14R05.Name = "chkL14R05";
            this.chkL14R05.Size = new System.Drawing.Size(16, 24);
            this.chkL14R05.TabIndex = 247;
            this.chkL14R05.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL14R04
            // 
            this.chkL14R04.Image = null;
            this.chkL14R04.Location = new System.Drawing.Point(152, 472);
            this.chkL14R04.Name = "chkL14R04";
            this.chkL14R04.Size = new System.Drawing.Size(16, 24);
            this.chkL14R04.TabIndex = 246;
            this.chkL14R04.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL14R03
            // 
            this.chkL14R03.Image = null;
            this.chkL14R03.Location = new System.Drawing.Point(136, 472);
            this.chkL14R03.Name = "chkL14R03";
            this.chkL14R03.Size = new System.Drawing.Size(16, 24);
            this.chkL14R03.TabIndex = 245;
            this.chkL14R03.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL14R02
            // 
            this.chkL14R02.Image = null;
            this.chkL14R02.Location = new System.Drawing.Point(120, 472);
            this.chkL14R02.Name = "chkL14R02";
            this.chkL14R02.Size = new System.Drawing.Size(16, 24);
            this.chkL14R02.TabIndex = 244;
            this.chkL14R02.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL14R01
            // 
            this.chkL14R01.Image = null;
            this.chkL14R01.Location = new System.Drawing.Point(104, 472);
            this.chkL14R01.Name = "chkL14R01";
            this.chkL14R01.Size = new System.Drawing.Size(16, 24);
            this.chkL14R01.TabIndex = 243;
            this.chkL14R01.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL15R09
            // 
            this.chkL15R09.Image = null;
            this.chkL15R09.Location = new System.Drawing.Point(248, 496);
            this.chkL15R09.Name = "chkL15R09";
            this.chkL15R09.Size = new System.Drawing.Size(16, 24);
            this.chkL15R09.TabIndex = 274;
            this.chkL15R09.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL15R10
            // 
            this.chkL15R10.Image = null;
            this.chkL15R10.Location = new System.Drawing.Point(264, 496);
            this.chkL15R10.Name = "chkL15R10";
            this.chkL15R10.Size = new System.Drawing.Size(16, 24);
            this.chkL15R10.TabIndex = 273;
            this.chkL15R10.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL15R11
            // 
            this.chkL15R11.Image = null;
            this.chkL15R11.Location = new System.Drawing.Point(280, 496);
            this.chkL15R11.Name = "chkL15R11";
            this.chkL15R11.Size = new System.Drawing.Size(16, 24);
            this.chkL15R11.TabIndex = 272;
            this.chkL15R11.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL15R12
            // 
            this.chkL15R12.Image = null;
            this.chkL15R12.Location = new System.Drawing.Point(296, 496);
            this.chkL15R12.Name = "chkL15R12";
            this.chkL15R12.Size = new System.Drawing.Size(16, 24);
            this.chkL15R12.TabIndex = 271;
            this.chkL15R12.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL15R13
            // 
            this.chkL15R13.Image = null;
            this.chkL15R13.Location = new System.Drawing.Point(320, 496);
            this.chkL15R13.Name = "chkL15R13";
            this.chkL15R13.Size = new System.Drawing.Size(16, 24);
            this.chkL15R13.TabIndex = 270;
            this.chkL15R13.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL15R14
            // 
            this.chkL15R14.Image = null;
            this.chkL15R14.Location = new System.Drawing.Point(336, 496);
            this.chkL15R14.Name = "chkL15R14";
            this.chkL15R14.Size = new System.Drawing.Size(16, 24);
            this.chkL15R14.TabIndex = 269;
            this.chkL15R14.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL15R15
            // 
            this.chkL15R15.Image = null;
            this.chkL15R15.Location = new System.Drawing.Point(352, 496);
            this.chkL15R15.Name = "chkL15R15";
            this.chkL15R15.Size = new System.Drawing.Size(16, 24);
            this.chkL15R15.TabIndex = 268;
            this.chkL15R15.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL15R16
            // 
            this.chkL15R16.Image = null;
            this.chkL15R16.Location = new System.Drawing.Point(368, 496);
            this.chkL15R16.Name = "chkL15R16";
            this.chkL15R16.Size = new System.Drawing.Size(16, 24);
            this.chkL15R16.TabIndex = 267;
            this.chkL15R16.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL15R08
            // 
            this.chkL15R08.Image = null;
            this.chkL15R08.Location = new System.Drawing.Point(224, 496);
            this.chkL15R08.Name = "chkL15R08";
            this.chkL15R08.Size = new System.Drawing.Size(16, 24);
            this.chkL15R08.TabIndex = 266;
            this.chkL15R08.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL15R07
            // 
            this.chkL15R07.Image = null;
            this.chkL15R07.Location = new System.Drawing.Point(208, 496);
            this.chkL15R07.Name = "chkL15R07";
            this.chkL15R07.Size = new System.Drawing.Size(16, 24);
            this.chkL15R07.TabIndex = 265;
            this.chkL15R07.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL15R06
            // 
            this.chkL15R06.Image = null;
            this.chkL15R06.Location = new System.Drawing.Point(192, 496);
            this.chkL15R06.Name = "chkL15R06";
            this.chkL15R06.Size = new System.Drawing.Size(16, 24);
            this.chkL15R06.TabIndex = 264;
            this.chkL15R06.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL15R05
            // 
            this.chkL15R05.Image = null;
            this.chkL15R05.Location = new System.Drawing.Point(176, 496);
            this.chkL15R05.Name = "chkL15R05";
            this.chkL15R05.Size = new System.Drawing.Size(16, 24);
            this.chkL15R05.TabIndex = 263;
            this.chkL15R05.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL15R04
            // 
            this.chkL15R04.Image = null;
            this.chkL15R04.Location = new System.Drawing.Point(152, 496);
            this.chkL15R04.Name = "chkL15R04";
            this.chkL15R04.Size = new System.Drawing.Size(16, 24);
            this.chkL15R04.TabIndex = 262;
            this.chkL15R04.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL15R03
            // 
            this.chkL15R03.Image = null;
            this.chkL15R03.Location = new System.Drawing.Point(136, 496);
            this.chkL15R03.Name = "chkL15R03";
            this.chkL15R03.Size = new System.Drawing.Size(16, 24);
            this.chkL15R03.TabIndex = 261;
            this.chkL15R03.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL15R02
            // 
            this.chkL15R02.Image = null;
            this.chkL15R02.Location = new System.Drawing.Point(120, 496);
            this.chkL15R02.Name = "chkL15R02";
            this.chkL15R02.Size = new System.Drawing.Size(16, 24);
            this.chkL15R02.TabIndex = 260;
            this.chkL15R02.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // chkL15R01
            // 
            this.chkL15R01.Image = null;
            this.chkL15R01.Location = new System.Drawing.Point(104, 496);
            this.chkL15R01.Name = "chkL15R01";
            this.chkL15R01.Size = new System.Drawing.Size(16, 24);
            this.chkL15R01.TabIndex = 259;
            this.chkL15R01.CheckedChanged += new System.EventHandler(this.chkRelay_CheckedChanged);
            // 
            // lblAddr
            // 
            this.lblAddr.Image = null;
            this.lblAddr.Location = new System.Drawing.Point(40, 80);
            this.lblAddr.Name = "lblAddr";
            this.lblAddr.Size = new System.Drawing.Size(48, 24);
            this.lblAddr.TabIndex = 275;
            this.lblAddr.Text = "Addr";
            this.lblAddr.DoubleClick += new System.EventHandler(this.label_DoubleClick);
            // 
            // lblLine0
            // 
            this.lblLine0.Image = null;
            this.lblLine0.Location = new System.Drawing.Point(40, 112);
            this.lblLine0.Name = "lblLine0";
            this.lblLine0.Size = new System.Drawing.Size(64, 23);
            this.lblLine0.TabIndex = 276;
            this.lblLine0.Text = "Line 0";
            this.lblLine0.DoubleClick += new System.EventHandler(this.label_DoubleClick);
            // 
            // lblLine1
            // 
            this.lblLine1.Image = null;
            this.lblLine1.Location = new System.Drawing.Point(40, 136);
            this.lblLine1.Name = "lblLine1";
            this.lblLine1.Size = new System.Drawing.Size(64, 23);
            this.lblLine1.TabIndex = 277;
            this.lblLine1.Text = "Line 1";
            this.lblLine1.DoubleClick += new System.EventHandler(this.label_DoubleClick);
            // 
            // lblLine2
            // 
            this.lblLine2.Image = null;
            this.lblLine2.Location = new System.Drawing.Point(40, 160);
            this.lblLine2.Name = "lblLine2";
            this.lblLine2.Size = new System.Drawing.Size(64, 23);
            this.lblLine2.TabIndex = 278;
            this.lblLine2.Text = "Line 2";
            this.lblLine2.DoubleClick += new System.EventHandler(this.label_DoubleClick);
            // 
            // lblLine3
            // 
            this.lblLine3.Image = null;
            this.lblLine3.Location = new System.Drawing.Point(40, 184);
            this.lblLine3.Name = "lblLine3";
            this.lblLine3.Size = new System.Drawing.Size(64, 23);
            this.lblLine3.TabIndex = 279;
            this.lblLine3.Text = "Line 3";
            this.lblLine3.DoubleClick += new System.EventHandler(this.label_DoubleClick);
            // 
            // lblLine4
            // 
            this.lblLine4.Image = null;
            this.lblLine4.Location = new System.Drawing.Point(40, 216);
            this.lblLine4.Name = "lblLine4";
            this.lblLine4.Size = new System.Drawing.Size(64, 23);
            this.lblLine4.TabIndex = 280;
            this.lblLine4.Text = "Line 4";
            this.lblLine4.DoubleClick += new System.EventHandler(this.label_DoubleClick);
            // 
            // lblLine5
            // 
            this.lblLine5.Image = null;
            this.lblLine5.Location = new System.Drawing.Point(40, 240);
            this.lblLine5.Name = "lblLine5";
            this.lblLine5.Size = new System.Drawing.Size(64, 23);
            this.lblLine5.TabIndex = 281;
            this.lblLine5.Text = "Line 5";
            this.lblLine5.DoubleClick += new System.EventHandler(this.label_DoubleClick);
            // 
            // lblLine6
            // 
            this.lblLine6.Image = null;
            this.lblLine6.Location = new System.Drawing.Point(40, 264);
            this.lblLine6.Name = "lblLine6";
            this.lblLine6.Size = new System.Drawing.Size(64, 23);
            this.lblLine6.TabIndex = 282;
            this.lblLine6.Text = "Line 6";
            this.lblLine6.DoubleClick += new System.EventHandler(this.label_DoubleClick);
            // 
            // lblLine7
            // 
            this.lblLine7.Image = null;
            this.lblLine7.Location = new System.Drawing.Point(40, 288);
            this.lblLine7.Name = "lblLine7";
            this.lblLine7.Size = new System.Drawing.Size(64, 23);
            this.lblLine7.TabIndex = 283;
            this.lblLine7.Text = "Line 7";
            this.lblLine7.DoubleClick += new System.EventHandler(this.label_DoubleClick);
            // 
            // lblLine8
            // 
            this.lblLine8.Image = null;
            this.lblLine8.Location = new System.Drawing.Point(40, 320);
            this.lblLine8.Name = "lblLine8";
            this.lblLine8.Size = new System.Drawing.Size(64, 23);
            this.lblLine8.TabIndex = 284;
            this.lblLine8.Text = "Line 8";
            this.lblLine8.DoubleClick += new System.EventHandler(this.label_DoubleClick);
            // 
            // lblLine9
            // 
            this.lblLine9.Image = null;
            this.lblLine9.Location = new System.Drawing.Point(40, 344);
            this.lblLine9.Name = "lblLine9";
            this.lblLine9.Size = new System.Drawing.Size(64, 23);
            this.lblLine9.TabIndex = 285;
            this.lblLine9.Text = "Line 9";
            this.lblLine9.DoubleClick += new System.EventHandler(this.label_DoubleClick);
            // 
            // lblLine10
            // 
            this.lblLine10.Image = null;
            this.lblLine10.Location = new System.Drawing.Point(40, 368);
            this.lblLine10.Name = "lblLine10";
            this.lblLine10.Size = new System.Drawing.Size(64, 23);
            this.lblLine10.TabIndex = 286;
            this.lblLine10.Text = "Line 10";
            this.lblLine10.DoubleClick += new System.EventHandler(this.label_DoubleClick);
            // 
            // lblLine11
            // 
            this.lblLine11.Image = null;
            this.lblLine11.Location = new System.Drawing.Point(40, 392);
            this.lblLine11.Name = "lblLine11";
            this.lblLine11.Size = new System.Drawing.Size(64, 23);
            this.lblLine11.TabIndex = 287;
            this.lblLine11.Text = "Line 11";
            this.lblLine11.DoubleClick += new System.EventHandler(this.label_DoubleClick);
            // 
            // lblLine12
            // 
            this.lblLine12.Image = null;
            this.lblLine12.Location = new System.Drawing.Point(40, 424);
            this.lblLine12.Name = "lblLine12";
            this.lblLine12.Size = new System.Drawing.Size(64, 23);
            this.lblLine12.TabIndex = 288;
            this.lblLine12.Text = "Line 12";
            this.lblLine12.DoubleClick += new System.EventHandler(this.label_DoubleClick);
            // 
            // lblLine13
            // 
            this.lblLine13.Image = null;
            this.lblLine13.Location = new System.Drawing.Point(40, 448);
            this.lblLine13.Name = "lblLine13";
            this.lblLine13.Size = new System.Drawing.Size(64, 23);
            this.lblLine13.TabIndex = 289;
            this.lblLine13.Text = "Line 13";
            this.lblLine13.DoubleClick += new System.EventHandler(this.label_DoubleClick);
            // 
            // lblLine14
            // 
            this.lblLine14.Image = null;
            this.lblLine14.Location = new System.Drawing.Point(40, 472);
            this.lblLine14.Name = "lblLine14";
            this.lblLine14.Size = new System.Drawing.Size(64, 23);
            this.lblLine14.TabIndex = 290;
            this.lblLine14.Text = "Line 14";
            this.lblLine14.DoubleClick += new System.EventHandler(this.label_DoubleClick);
            // 
            // lblLine15
            // 
            this.lblLine15.Image = null;
            this.lblLine15.Location = new System.Drawing.Point(40, 496);
            this.lblLine15.Name = "lblLine15";
            this.lblLine15.Size = new System.Drawing.Size(64, 23);
            this.lblLine15.TabIndex = 291;
            this.lblLine15.Text = "Line 15";
            this.lblLine15.DoubleClick += new System.EventHandler(this.label_DoubleClick);
            // 
            // btnWriteLine0
            // 
            this.btnWriteLine0.Image = null;
            this.btnWriteLine0.Location = new System.Drawing.Point(400, 112);
            this.btnWriteLine0.Name = "btnWriteLine0";
            this.btnWriteLine0.Size = new System.Drawing.Size(48, 20);
            this.btnWriteLine0.TabIndex = 292;
            this.btnWriteLine0.Text = "Write";
            this.btnWriteLine0.Click += new System.EventHandler(this.btnWriteLine0_Click);
            // 
            // btnWriteLine1
            // 
            this.btnWriteLine1.Image = null;
            this.btnWriteLine1.Location = new System.Drawing.Point(400, 136);
            this.btnWriteLine1.Name = "btnWriteLine1";
            this.btnWriteLine1.Size = new System.Drawing.Size(48, 20);
            this.btnWriteLine1.TabIndex = 293;
            this.btnWriteLine1.Text = "Write";
            this.btnWriteLine1.Click += new System.EventHandler(this.btnWriteLine1_Click);
            // 
            // btnWriteLine2
            // 
            this.btnWriteLine2.Image = null;
            this.btnWriteLine2.Location = new System.Drawing.Point(400, 160);
            this.btnWriteLine2.Name = "btnWriteLine2";
            this.btnWriteLine2.Size = new System.Drawing.Size(48, 20);
            this.btnWriteLine2.TabIndex = 294;
            this.btnWriteLine2.Text = "Write";
            this.btnWriteLine2.Click += new System.EventHandler(this.btnWriteLine2_Click);
            // 
            // btnWriteLine3
            // 
            this.btnWriteLine3.Image = null;
            this.btnWriteLine3.Location = new System.Drawing.Point(400, 184);
            this.btnWriteLine3.Name = "btnWriteLine3";
            this.btnWriteLine3.Size = new System.Drawing.Size(48, 20);
            this.btnWriteLine3.TabIndex = 295;
            this.btnWriteLine3.Text = "Write";
            this.btnWriteLine3.Click += new System.EventHandler(this.btnWriteLine3_Click);
            // 
            // btnWriteLine4
            // 
            this.btnWriteLine4.Image = null;
            this.btnWriteLine4.Location = new System.Drawing.Point(400, 216);
            this.btnWriteLine4.Name = "btnWriteLine4";
            this.btnWriteLine4.Size = new System.Drawing.Size(48, 20);
            this.btnWriteLine4.TabIndex = 296;
            this.btnWriteLine4.Text = "Write";
            this.btnWriteLine4.Click += new System.EventHandler(this.btnWriteLine4_Click);
            // 
            // btnWriteLine5
            // 
            this.btnWriteLine5.Image = null;
            this.btnWriteLine5.Location = new System.Drawing.Point(400, 240);
            this.btnWriteLine5.Name = "btnWriteLine5";
            this.btnWriteLine5.Size = new System.Drawing.Size(48, 20);
            this.btnWriteLine5.TabIndex = 297;
            this.btnWriteLine5.Text = "Write";
            this.btnWriteLine5.Click += new System.EventHandler(this.btnWriteLine5_Click);
            // 
            // btnWriteLine6
            // 
            this.btnWriteLine6.Image = null;
            this.btnWriteLine6.Location = new System.Drawing.Point(400, 264);
            this.btnWriteLine6.Name = "btnWriteLine6";
            this.btnWriteLine6.Size = new System.Drawing.Size(48, 20);
            this.btnWriteLine6.TabIndex = 298;
            this.btnWriteLine6.Text = "Write";
            this.btnWriteLine6.Click += new System.EventHandler(this.btnWriteLine6_Click);
            // 
            // btnWriteLine7
            // 
            this.btnWriteLine7.Image = null;
            this.btnWriteLine7.Location = new System.Drawing.Point(400, 288);
            this.btnWriteLine7.Name = "btnWriteLine7";
            this.btnWriteLine7.Size = new System.Drawing.Size(48, 20);
            this.btnWriteLine7.TabIndex = 299;
            this.btnWriteLine7.Text = "Write";
            this.btnWriteLine7.Click += new System.EventHandler(this.btnWriteLine7_Click);
            // 
            // btnWriteLine8
            // 
            this.btnWriteLine8.Image = null;
            this.btnWriteLine8.Location = new System.Drawing.Point(400, 320);
            this.btnWriteLine8.Name = "btnWriteLine8";
            this.btnWriteLine8.Size = new System.Drawing.Size(48, 20);
            this.btnWriteLine8.TabIndex = 300;
            this.btnWriteLine8.Text = "Write";
            this.btnWriteLine8.Click += new System.EventHandler(this.btnWriteLine8_Click);
            // 
            // btnWriteLine9
            // 
            this.btnWriteLine9.Image = null;
            this.btnWriteLine9.Location = new System.Drawing.Point(400, 344);
            this.btnWriteLine9.Name = "btnWriteLine9";
            this.btnWriteLine9.Size = new System.Drawing.Size(48, 20);
            this.btnWriteLine9.TabIndex = 301;
            this.btnWriteLine9.Text = "Write";
            this.btnWriteLine9.Click += new System.EventHandler(this.btnWriteLine9_Click);
            // 
            // btnWriteLine10
            // 
            this.btnWriteLine10.Image = null;
            this.btnWriteLine10.Location = new System.Drawing.Point(400, 368);
            this.btnWriteLine10.Name = "btnWriteLine10";
            this.btnWriteLine10.Size = new System.Drawing.Size(48, 20);
            this.btnWriteLine10.TabIndex = 302;
            this.btnWriteLine10.Text = "Write";
            this.btnWriteLine10.Click += new System.EventHandler(this.btnWriteLine10_Click);
            // 
            // btnWriteLine11
            // 
            this.btnWriteLine11.Image = null;
            this.btnWriteLine11.Location = new System.Drawing.Point(400, 392);
            this.btnWriteLine11.Name = "btnWriteLine11";
            this.btnWriteLine11.Size = new System.Drawing.Size(48, 20);
            this.btnWriteLine11.TabIndex = 303;
            this.btnWriteLine11.Text = "Write";
            this.btnWriteLine11.Click += new System.EventHandler(this.btnWriteLine11_Click);
            // 
            // btnWriteLine12
            // 
            this.btnWriteLine12.Image = null;
            this.btnWriteLine12.Location = new System.Drawing.Point(400, 424);
            this.btnWriteLine12.Name = "btnWriteLine12";
            this.btnWriteLine12.Size = new System.Drawing.Size(48, 20);
            this.btnWriteLine12.TabIndex = 304;
            this.btnWriteLine12.Text = "Write";
            this.btnWriteLine12.Click += new System.EventHandler(this.btnWriteLine12_Click);
            // 
            // btnWriteLine13
            // 
            this.btnWriteLine13.Image = null;
            this.btnWriteLine13.Location = new System.Drawing.Point(400, 448);
            this.btnWriteLine13.Name = "btnWriteLine13";
            this.btnWriteLine13.Size = new System.Drawing.Size(48, 20);
            this.btnWriteLine13.TabIndex = 305;
            this.btnWriteLine13.Text = "Write";
            this.btnWriteLine13.Click += new System.EventHandler(this.btnWriteLine13_Click);
            // 
            // btnWriteLine14
            // 
            this.btnWriteLine14.Image = null;
            this.btnWriteLine14.Location = new System.Drawing.Point(400, 472);
            this.btnWriteLine14.Name = "btnWriteLine14";
            this.btnWriteLine14.Size = new System.Drawing.Size(48, 20);
            this.btnWriteLine14.TabIndex = 306;
            this.btnWriteLine14.Text = "Write";
            this.btnWriteLine14.Click += new System.EventHandler(this.btnWriteLine14_Click);
            // 
            // btnWriteLine15
            // 
            this.btnWriteLine15.Image = null;
            this.btnWriteLine15.Location = new System.Drawing.Point(400, 496);
            this.btnWriteLine15.Name = "btnWriteLine15";
            this.btnWriteLine15.Size = new System.Drawing.Size(48, 20);
            this.btnWriteLine15.TabIndex = 307;
            this.btnWriteLine15.Text = "Write";
            this.btnWriteLine15.Click += new System.EventHandler(this.btnWriteLine15_Click);
            // 
            // btnWriteAll
            // 
            this.btnWriteAll.Image = null;
            this.btnWriteAll.Location = new System.Drawing.Point(392, 72);
            this.btnWriteAll.Name = "btnWriteAll";
            this.btnWriteAll.Size = new System.Drawing.Size(64, 23);
            this.btnWriteAll.TabIndex = 308;
            this.btnWriteAll.Text = "Write All";
            this.btnWriteAll.Click += new System.EventHandler(this.btnWriteAll_Click);
            // 
            // radLine0
            // 
            this.radLine0.Checked = true;
            this.radLine0.Image = null;
            this.radLine0.Location = new System.Drawing.Point(16, 112);
            this.radLine0.Name = "radLine0";
            this.radLine0.Size = new System.Drawing.Size(16, 16);
            this.radLine0.TabIndex = 309;
            this.radLine0.TabStop = true;
            this.radLine0.Text = "radioButton1";
            this.toolTip1.SetToolTip(this.radLine0, "For SDR-1000 only");
            this.radLine0.CheckedChanged += new System.EventHandler(this.radLine_CheckedChanged);
            // 
            // radLine1
            // 
            this.radLine1.Image = null;
            this.radLine1.Location = new System.Drawing.Point(16, 136);
            this.radLine1.Name = "radLine1";
            this.radLine1.Size = new System.Drawing.Size(16, 16);
            this.radLine1.TabIndex = 310;
            this.radLine1.Text = "radioButton2";
            this.radLine1.CheckedChanged += new System.EventHandler(this.radLine_CheckedChanged);
            // 
            // radLine2
            // 
            this.radLine2.Image = null;
            this.radLine2.Location = new System.Drawing.Point(16, 160);
            this.radLine2.Name = "radLine2";
            this.radLine2.Size = new System.Drawing.Size(16, 16);
            this.radLine2.TabIndex = 311;
            this.radLine2.Text = "radioButton3";
            this.radLine2.CheckedChanged += new System.EventHandler(this.radLine_CheckedChanged);
            // 
            // radLine3
            // 
            this.radLine3.Image = null;
            this.radLine3.Location = new System.Drawing.Point(16, 184);
            this.radLine3.Name = "radLine3";
            this.radLine3.Size = new System.Drawing.Size(16, 16);
            this.radLine3.TabIndex = 312;
            this.radLine3.Text = "radioButton4";
            this.radLine3.CheckedChanged += new System.EventHandler(this.radLine_CheckedChanged);
            // 
            // radLine4
            // 
            this.radLine4.Image = null;
            this.radLine4.Location = new System.Drawing.Point(16, 216);
            this.radLine4.Name = "radLine4";
            this.radLine4.Size = new System.Drawing.Size(16, 16);
            this.radLine4.TabIndex = 313;
            this.radLine4.Text = "radioButton5";
            this.radLine4.CheckedChanged += new System.EventHandler(this.radLine_CheckedChanged);
            // 
            // radLine5
            // 
            this.radLine5.Image = null;
            this.radLine5.Location = new System.Drawing.Point(16, 240);
            this.radLine5.Name = "radLine5";
            this.radLine5.Size = new System.Drawing.Size(16, 16);
            this.radLine5.TabIndex = 314;
            this.radLine5.Text = "radioButton6";
            this.radLine5.CheckedChanged += new System.EventHandler(this.radLine_CheckedChanged);
            // 
            // radLine6
            // 
            this.radLine6.Image = null;
            this.radLine6.Location = new System.Drawing.Point(16, 264);
            this.radLine6.Name = "radLine6";
            this.radLine6.Size = new System.Drawing.Size(16, 16);
            this.radLine6.TabIndex = 315;
            this.radLine6.Text = "radioButton7";
            this.radLine6.CheckedChanged += new System.EventHandler(this.radLine_CheckedChanged);
            // 
            // radLine7
            // 
            this.radLine7.Image = null;
            this.radLine7.Location = new System.Drawing.Point(16, 288);
            this.radLine7.Name = "radLine7";
            this.radLine7.Size = new System.Drawing.Size(16, 16);
            this.radLine7.TabIndex = 316;
            this.radLine7.Text = "radioButton8";
            this.radLine7.CheckedChanged += new System.EventHandler(this.radLine_CheckedChanged);
            // 
            // radLine8
            // 
            this.radLine8.Image = null;
            this.radLine8.Location = new System.Drawing.Point(16, 320);
            this.radLine8.Name = "radLine8";
            this.radLine8.Size = new System.Drawing.Size(16, 16);
            this.radLine8.TabIndex = 317;
            this.radLine8.Text = "radioButton9";
            this.radLine8.CheckedChanged += new System.EventHandler(this.radLine_CheckedChanged);
            // 
            // radLine9
            // 
            this.radLine9.Image = null;
            this.radLine9.Location = new System.Drawing.Point(16, 344);
            this.radLine9.Name = "radLine9";
            this.radLine9.Size = new System.Drawing.Size(16, 16);
            this.radLine9.TabIndex = 318;
            this.radLine9.Text = "radioButton10";
            this.radLine9.CheckedChanged += new System.EventHandler(this.radLine_CheckedChanged);
            // 
            // radLine10
            // 
            this.radLine10.Image = null;
            this.radLine10.Location = new System.Drawing.Point(16, 368);
            this.radLine10.Name = "radLine10";
            this.radLine10.Size = new System.Drawing.Size(16, 16);
            this.radLine10.TabIndex = 319;
            this.radLine10.Text = "radioButton11";
            this.radLine10.CheckedChanged += new System.EventHandler(this.radLine_CheckedChanged);
            // 
            // radLine11
            // 
            this.radLine11.Image = null;
            this.radLine11.Location = new System.Drawing.Point(16, 392);
            this.radLine11.Name = "radLine11";
            this.radLine11.Size = new System.Drawing.Size(16, 16);
            this.radLine11.TabIndex = 320;
            this.radLine11.Text = "radioButton12";
            this.radLine11.CheckedChanged += new System.EventHandler(this.radLine_CheckedChanged);
            // 
            // radLine12
            // 
            this.radLine12.Image = null;
            this.radLine12.Location = new System.Drawing.Point(16, 424);
            this.radLine12.Name = "radLine12";
            this.radLine12.Size = new System.Drawing.Size(16, 16);
            this.radLine12.TabIndex = 321;
            this.radLine12.Text = "radioButton13";
            this.radLine12.CheckedChanged += new System.EventHandler(this.radLine_CheckedChanged);
            // 
            // radLine13
            // 
            this.radLine13.Image = null;
            this.radLine13.Location = new System.Drawing.Point(16, 448);
            this.radLine13.Name = "radLine13";
            this.radLine13.Size = new System.Drawing.Size(16, 16);
            this.radLine13.TabIndex = 322;
            this.radLine13.Text = "radioButton14";
            this.radLine13.CheckedChanged += new System.EventHandler(this.radLine_CheckedChanged);
            // 
            // radLine14
            // 
            this.radLine14.Image = null;
            this.radLine14.Location = new System.Drawing.Point(16, 472);
            this.radLine14.Name = "radLine14";
            this.radLine14.Size = new System.Drawing.Size(16, 16);
            this.radLine14.TabIndex = 323;
            this.radLine14.Text = "radioButton15";
            this.radLine14.CheckedChanged += new System.EventHandler(this.radLine_CheckedChanged);
            // 
            // radLine15
            // 
            this.radLine15.Image = null;
            this.radLine15.Location = new System.Drawing.Point(16, 496);
            this.radLine15.Name = "radLine15";
            this.radLine15.Size = new System.Drawing.Size(16, 16);
            this.radLine15.TabIndex = 324;
            this.radLine15.Text = "radioButton16";
            this.radLine15.CheckedChanged += new System.EventHandler(this.radLine_CheckedChanged);
            // 
            // lblRelays
            // 
            this.lblRelays.Image = null;
            this.lblRelays.Location = new System.Drawing.Point(104, 80);
            this.lblRelays.Name = "lblRelays";
            this.lblRelays.Size = new System.Drawing.Size(280, 16);
            this.lblRelays.TabIndex = 325;
            this.lblRelays.Text = "Relays";
            this.lblRelays.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblDelay
            // 
            this.lblDelay.Image = null;
            this.lblDelay.Location = new System.Drawing.Point(232, 16);
            this.lblDelay.Name = "lblDelay";
            this.lblDelay.Size = new System.Drawing.Size(40, 23);
            this.lblDelay.TabIndex = 326;
            this.lblDelay.Text = "Delay:";
            // 
            // comboDelay
            // 
            this.comboDelay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboDelay.DropDownWidth = 56;
            this.comboDelay.Items.AddRange(new object[] {
            "80ms",
            "40ms",
            "20ms",
            "10ms",
            "5ms",
            "2ms"});
            this.comboDelay.Location = new System.Drawing.Point(272, 16);
            this.comboDelay.Name = "comboDelay";
            this.comboDelay.Size = new System.Drawing.Size(56, 21);
            this.comboDelay.TabIndex = 327;
            // 
            // btnSetDelay
            // 
            this.btnSetDelay.Location = new System.Drawing.Point(336, 16);
            this.btnSetDelay.Name = "btnSetDelay";
            this.btnSetDelay.Size = new System.Drawing.Size(32, 20);
            this.btnSetDelay.TabIndex = 328;
            this.btnSetDelay.Text = "Set";
            this.btnSetDelay.Click += new System.EventHandler(this.btnSetDelay_Click);
            // 
            // btnSetupXVTR
            // 
            this.btnSetupXVTR.Location = new System.Drawing.Point(24, 16);
            this.btnSetupXVTR.Name = "btnSetupXVTR";
            this.btnSetupXVTR.Size = new System.Drawing.Size(88, 23);
            this.btnSetupXVTR.TabIndex = 329;
            this.btnSetupXVTR.Text = "Setup XVTRs";
            this.btnSetupXVTR.Click += new System.EventHandler(this.btnSetupXVTR_Click);
            // 
            // chkFlexWire
            // 
            this.chkFlexWire.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkFlexWire.Image = null;
            this.chkFlexWire.Location = new System.Drawing.Point(24, 48);
            this.chkFlexWire.Name = "chkFlexWire";
            this.chkFlexWire.Size = new System.Drawing.Size(88, 24);
            this.chkFlexWire.TabIndex = 330;
            this.chkFlexWire.Text = "FlexWire";
            this.chkFlexWire.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.chkFlexWire, resources.GetString("chkFlexWire.ToolTip"));
            this.chkFlexWire.CheckedChanged += new System.EventHandler(this.chkFlexWire_CheckedChanged);
            // 
            // UCBForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(496, 534);
            this.Controls.Add(this.chkFlexWire);
            this.Controls.Add(this.btnSetupXVTR);
            this.Controls.Add(this.btnSetDelay);
            this.Controls.Add(this.comboDelay);
            this.Controls.Add(this.lblDelay);
            this.Controls.Add(this.lblRelays);
            this.Controls.Add(this.radLine15);
            this.Controls.Add(this.radLine14);
            this.Controls.Add(this.radLine13);
            this.Controls.Add(this.radLine12);
            this.Controls.Add(this.radLine11);
            this.Controls.Add(this.radLine10);
            this.Controls.Add(this.radLine9);
            this.Controls.Add(this.radLine8);
            this.Controls.Add(this.radLine7);
            this.Controls.Add(this.radLine6);
            this.Controls.Add(this.radLine5);
            this.Controls.Add(this.radLine4);
            this.Controls.Add(this.radLine3);
            this.Controls.Add(this.radLine2);
            this.Controls.Add(this.radLine1);
            this.Controls.Add(this.radLine0);
            this.Controls.Add(this.btnWriteAll);
            this.Controls.Add(this.btnWriteLine15);
            this.Controls.Add(this.btnWriteLine14);
            this.Controls.Add(this.btnWriteLine13);
            this.Controls.Add(this.btnWriteLine12);
            this.Controls.Add(this.btnWriteLine11);
            this.Controls.Add(this.btnWriteLine10);
            this.Controls.Add(this.btnWriteLine9);
            this.Controls.Add(this.btnWriteLine8);
            this.Controls.Add(this.btnWriteLine7);
            this.Controls.Add(this.btnWriteLine6);
            this.Controls.Add(this.btnWriteLine5);
            this.Controls.Add(this.btnWriteLine4);
            this.Controls.Add(this.btnWriteLine3);
            this.Controls.Add(this.btnWriteLine2);
            this.Controls.Add(this.btnWriteLine1);
            this.Controls.Add(this.btnWriteLine0);
            this.Controls.Add(this.lblLine15);
            this.Controls.Add(this.lblLine14);
            this.Controls.Add(this.lblLine13);
            this.Controls.Add(this.lblLine12);
            this.Controls.Add(this.lblLine11);
            this.Controls.Add(this.lblLine10);
            this.Controls.Add(this.lblLine9);
            this.Controls.Add(this.lblLine8);
            this.Controls.Add(this.lblLine7);
            this.Controls.Add(this.lblLine6);
            this.Controls.Add(this.lblLine5);
            this.Controls.Add(this.lblLine4);
            this.Controls.Add(this.lblLine3);
            this.Controls.Add(this.lblLine2);
            this.Controls.Add(this.lblLine1);
            this.Controls.Add(this.lblLine0);
            this.Controls.Add(this.lblAddr);
            this.Controls.Add(this.chkL15R09);
            this.Controls.Add(this.chkL15R10);
            this.Controls.Add(this.chkL15R11);
            this.Controls.Add(this.chkL15R12);
            this.Controls.Add(this.chkL15R13);
            this.Controls.Add(this.chkL15R14);
            this.Controls.Add(this.chkL15R15);
            this.Controls.Add(this.chkL15R16);
            this.Controls.Add(this.chkL15R08);
            this.Controls.Add(this.chkL15R07);
            this.Controls.Add(this.chkL15R06);
            this.Controls.Add(this.chkL15R05);
            this.Controls.Add(this.chkL15R04);
            this.Controls.Add(this.chkL15R03);
            this.Controls.Add(this.chkL15R02);
            this.Controls.Add(this.chkL15R01);
            this.Controls.Add(this.chkL14R09);
            this.Controls.Add(this.chkL14R10);
            this.Controls.Add(this.chkL14R11);
            this.Controls.Add(this.chkL14R12);
            this.Controls.Add(this.chkL14R13);
            this.Controls.Add(this.chkL14R14);
            this.Controls.Add(this.chkL14R15);
            this.Controls.Add(this.chkL14R16);
            this.Controls.Add(this.chkL14R08);
            this.Controls.Add(this.chkL14R07);
            this.Controls.Add(this.chkL14R06);
            this.Controls.Add(this.chkL14R05);
            this.Controls.Add(this.chkL14R04);
            this.Controls.Add(this.chkL14R03);
            this.Controls.Add(this.chkL14R02);
            this.Controls.Add(this.chkL14R01);
            this.Controls.Add(this.chkL13R09);
            this.Controls.Add(this.chkL13R10);
            this.Controls.Add(this.chkL13R11);
            this.Controls.Add(this.chkL13R12);
            this.Controls.Add(this.chkL13R13);
            this.Controls.Add(this.chkL13R14);
            this.Controls.Add(this.chkL13R15);
            this.Controls.Add(this.chkL13R16);
            this.Controls.Add(this.chkL13R08);
            this.Controls.Add(this.chkL13R07);
            this.Controls.Add(this.chkL13R06);
            this.Controls.Add(this.chkL13R05);
            this.Controls.Add(this.chkL13R04);
            this.Controls.Add(this.chkL13R03);
            this.Controls.Add(this.chkL13R02);
            this.Controls.Add(this.chkL13R01);
            this.Controls.Add(this.chkL12R09);
            this.Controls.Add(this.chkL12R10);
            this.Controls.Add(this.chkL12R11);
            this.Controls.Add(this.chkL12R12);
            this.Controls.Add(this.chkL12R13);
            this.Controls.Add(this.chkL12R14);
            this.Controls.Add(this.chkL12R15);
            this.Controls.Add(this.chkL12R16);
            this.Controls.Add(this.chkL12R08);
            this.Controls.Add(this.chkL12R07);
            this.Controls.Add(this.chkL12R06);
            this.Controls.Add(this.chkL12R05);
            this.Controls.Add(this.chkL12R04);
            this.Controls.Add(this.chkL12R03);
            this.Controls.Add(this.chkL12R02);
            this.Controls.Add(this.chkL12R01);
            this.Controls.Add(this.chkL11R09);
            this.Controls.Add(this.chkL11R10);
            this.Controls.Add(this.chkL11R11);
            this.Controls.Add(this.chkL11R12);
            this.Controls.Add(this.chkL11R13);
            this.Controls.Add(this.chkL11R14);
            this.Controls.Add(this.chkL11R15);
            this.Controls.Add(this.chkL11R16);
            this.Controls.Add(this.chkL11R08);
            this.Controls.Add(this.chkL11R07);
            this.Controls.Add(this.chkL11R06);
            this.Controls.Add(this.chkL11R05);
            this.Controls.Add(this.chkL11R04);
            this.Controls.Add(this.chkL11R03);
            this.Controls.Add(this.chkL11R02);
            this.Controls.Add(this.chkL11R01);
            this.Controls.Add(this.chkL10R09);
            this.Controls.Add(this.chkL10R10);
            this.Controls.Add(this.chkL10R11);
            this.Controls.Add(this.chkL10R12);
            this.Controls.Add(this.chkL10R13);
            this.Controls.Add(this.chkL10R14);
            this.Controls.Add(this.chkL10R15);
            this.Controls.Add(this.chkL10R16);
            this.Controls.Add(this.chkL10R08);
            this.Controls.Add(this.chkL10R07);
            this.Controls.Add(this.chkL10R06);
            this.Controls.Add(this.chkL10R05);
            this.Controls.Add(this.chkL10R04);
            this.Controls.Add(this.chkL10R03);
            this.Controls.Add(this.chkL10R02);
            this.Controls.Add(this.chkL10R01);
            this.Controls.Add(this.chkL09R09);
            this.Controls.Add(this.chkL09R10);
            this.Controls.Add(this.chkL09R11);
            this.Controls.Add(this.chkL09R12);
            this.Controls.Add(this.chkL09R13);
            this.Controls.Add(this.chkL09R14);
            this.Controls.Add(this.chkL09R15);
            this.Controls.Add(this.chkL09R16);
            this.Controls.Add(this.chkL09R08);
            this.Controls.Add(this.chkL09R07);
            this.Controls.Add(this.chkL09R06);
            this.Controls.Add(this.chkL09R05);
            this.Controls.Add(this.chkL09R04);
            this.Controls.Add(this.chkL09R03);
            this.Controls.Add(this.chkL09R02);
            this.Controls.Add(this.chkL09R01);
            this.Controls.Add(this.chkL08R09);
            this.Controls.Add(this.chkL08R10);
            this.Controls.Add(this.chkL08R11);
            this.Controls.Add(this.chkL08R12);
            this.Controls.Add(this.chkL08R13);
            this.Controls.Add(this.chkL08R14);
            this.Controls.Add(this.chkL08R15);
            this.Controls.Add(this.chkL08R16);
            this.Controls.Add(this.chkL08R08);
            this.Controls.Add(this.chkL08R07);
            this.Controls.Add(this.chkL08R06);
            this.Controls.Add(this.chkL08R05);
            this.Controls.Add(this.chkL08R04);
            this.Controls.Add(this.chkL08R03);
            this.Controls.Add(this.chkL08R02);
            this.Controls.Add(this.chkL08R01);
            this.Controls.Add(this.chkL07R09);
            this.Controls.Add(this.chkL07R10);
            this.Controls.Add(this.chkL07R11);
            this.Controls.Add(this.chkL07R12);
            this.Controls.Add(this.chkL07R13);
            this.Controls.Add(this.chkL07R14);
            this.Controls.Add(this.chkL07R15);
            this.Controls.Add(this.chkL07R16);
            this.Controls.Add(this.chkL07R08);
            this.Controls.Add(this.chkL07R07);
            this.Controls.Add(this.chkL07R06);
            this.Controls.Add(this.chkL07R05);
            this.Controls.Add(this.chkL07R04);
            this.Controls.Add(this.chkL07R03);
            this.Controls.Add(this.chkL07R02);
            this.Controls.Add(this.chkL07R01);
            this.Controls.Add(this.chkL06R09);
            this.Controls.Add(this.chkL06R10);
            this.Controls.Add(this.chkL06R11);
            this.Controls.Add(this.chkL06R12);
            this.Controls.Add(this.chkL06R13);
            this.Controls.Add(this.chkL06R14);
            this.Controls.Add(this.chkL06R15);
            this.Controls.Add(this.chkL06R16);
            this.Controls.Add(this.chkL06R08);
            this.Controls.Add(this.chkL06R07);
            this.Controls.Add(this.chkL06R06);
            this.Controls.Add(this.chkL06R05);
            this.Controls.Add(this.chkL06R04);
            this.Controls.Add(this.chkL06R03);
            this.Controls.Add(this.chkL06R02);
            this.Controls.Add(this.chkL06R01);
            this.Controls.Add(this.chkL05R09);
            this.Controls.Add(this.chkL05R10);
            this.Controls.Add(this.chkL05R11);
            this.Controls.Add(this.chkL05R12);
            this.Controls.Add(this.chkL05R13);
            this.Controls.Add(this.chkL05R14);
            this.Controls.Add(this.chkL05R15);
            this.Controls.Add(this.chkL05R16);
            this.Controls.Add(this.chkL05R08);
            this.Controls.Add(this.chkL05R07);
            this.Controls.Add(this.chkL05R06);
            this.Controls.Add(this.chkL05R05);
            this.Controls.Add(this.chkL05R04);
            this.Controls.Add(this.chkL05R03);
            this.Controls.Add(this.chkL05R02);
            this.Controls.Add(this.chkL05R01);
            this.Controls.Add(this.chkL04R09);
            this.Controls.Add(this.chkL04R10);
            this.Controls.Add(this.chkL04R11);
            this.Controls.Add(this.chkL04R12);
            this.Controls.Add(this.chkL04R13);
            this.Controls.Add(this.chkL04R14);
            this.Controls.Add(this.chkL04R15);
            this.Controls.Add(this.chkL04R16);
            this.Controls.Add(this.chkL04R08);
            this.Controls.Add(this.chkL04R07);
            this.Controls.Add(this.chkL04R06);
            this.Controls.Add(this.chkL04R05);
            this.Controls.Add(this.chkL04R04);
            this.Controls.Add(this.chkL04R03);
            this.Controls.Add(this.chkL04R02);
            this.Controls.Add(this.chkL04R01);
            this.Controls.Add(this.chkL03R09);
            this.Controls.Add(this.chkL03R10);
            this.Controls.Add(this.chkL03R11);
            this.Controls.Add(this.chkL03R12);
            this.Controls.Add(this.chkL03R13);
            this.Controls.Add(this.chkL03R14);
            this.Controls.Add(this.chkL03R15);
            this.Controls.Add(this.chkL03R16);
            this.Controls.Add(this.chkL03R08);
            this.Controls.Add(this.chkL03R07);
            this.Controls.Add(this.chkL03R06);
            this.Controls.Add(this.chkL03R05);
            this.Controls.Add(this.chkL03R04);
            this.Controls.Add(this.chkL03R03);
            this.Controls.Add(this.chkL03R02);
            this.Controls.Add(this.chkL03R01);
            this.Controls.Add(this.chkL02R09);
            this.Controls.Add(this.chkL02R10);
            this.Controls.Add(this.chkL02R11);
            this.Controls.Add(this.chkL02R12);
            this.Controls.Add(this.chkL02R13);
            this.Controls.Add(this.chkL02R14);
            this.Controls.Add(this.chkL02R15);
            this.Controls.Add(this.chkL02R16);
            this.Controls.Add(this.chkL02R08);
            this.Controls.Add(this.chkL02R07);
            this.Controls.Add(this.chkL02R06);
            this.Controls.Add(this.chkL02R05);
            this.Controls.Add(this.chkL02R04);
            this.Controls.Add(this.chkL02R03);
            this.Controls.Add(this.chkL02R02);
            this.Controls.Add(this.chkL02R01);
            this.Controls.Add(this.chkL01R09);
            this.Controls.Add(this.chkL01R10);
            this.Controls.Add(this.chkL01R11);
            this.Controls.Add(this.chkL01R12);
            this.Controls.Add(this.chkL01R13);
            this.Controls.Add(this.chkL01R14);
            this.Controls.Add(this.chkL01R15);
            this.Controls.Add(this.chkL01R16);
            this.Controls.Add(this.chkL01R08);
            this.Controls.Add(this.chkL01R07);
            this.Controls.Add(this.chkL01R06);
            this.Controls.Add(this.chkL01R05);
            this.Controls.Add(this.chkL01R04);
            this.Controls.Add(this.chkL01R03);
            this.Controls.Add(this.chkL01R02);
            this.Controls.Add(this.chkL01R01);
            this.Controls.Add(this.lblR16);
            this.Controls.Add(this.lblR15);
            this.Controls.Add(this.lblR14);
            this.Controls.Add(this.lblR13);
            this.Controls.Add(this.lblR12);
            this.Controls.Add(this.lblR11);
            this.Controls.Add(this.lblR10);
            this.Controls.Add(this.lblR9);
            this.Controls.Add(this.lblR8);
            this.Controls.Add(this.lblR7);
            this.Controls.Add(this.lblR6);
            this.Controls.Add(this.lblR5);
            this.Controls.Add(this.lblR4);
            this.Controls.Add(this.lblR3);
            this.Controls.Add(this.lblR2);
            this.Controls.Add(this.lblR1);
            this.Controls.Add(this.chkL00R09);
            this.Controls.Add(this.chkL00R10);
            this.Controls.Add(this.chkL00R11);
            this.Controls.Add(this.chkL00R12);
            this.Controls.Add(this.chkL00R13);
            this.Controls.Add(this.chkL00R14);
            this.Controls.Add(this.chkL00R15);
            this.Controls.Add(this.chkL00R16);
            this.Controls.Add(this.chkL00R08);
            this.Controls.Add(this.chkL00R07);
            this.Controls.Add(this.chkL00R06);
            this.Controls.Add(this.chkL00R05);
            this.Controls.Add(this.chkL00R04);
            this.Controls.Add(this.chkL00R03);
            this.Controls.Add(this.chkL00R02);
            this.Controls.Add(this.chkL00R01);
            this.Controls.Add(this.btnDisableClear);
            this.Controls.Add(this.btnDisable);
            this.Controls.Add(this.btnEnable);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "UCBForm";
            this.Text = "UCB (Universal Control Board) Configuration and Setup";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.UCBForm_Closing);
            this.ResumeLayout(false);

        }
        #endregion

      
        private System.Windows.Forms.ButtonTS btnEnable;
        private System.Windows.Forms.ButtonTS btnDisable;
        private System.Windows.Forms.ButtonTS btnDisableClear;
        private System.Windows.Forms.CheckBoxTS chkL00R01;
        private System.Windows.Forms.CheckBoxTS chkL00R02;
        private System.Windows.Forms.CheckBoxTS chkL00R03;
        private System.Windows.Forms.CheckBoxTS chkL00R04;
        private System.Windows.Forms.CheckBoxTS chkL00R05;
        private System.Windows.Forms.CheckBoxTS chkL00R06;
        private System.Windows.Forms.CheckBoxTS chkL00R07;
        private System.Windows.Forms.CheckBoxTS chkL00R08;
        private System.Windows.Forms.CheckBoxTS chkL00R09;
        private System.Windows.Forms.CheckBoxTS chkL00R10;
        private System.Windows.Forms.CheckBoxTS chkL00R11;
        private System.Windows.Forms.CheckBoxTS chkL00R12;
        private System.Windows.Forms.CheckBoxTS chkL00R13;
        private System.Windows.Forms.CheckBoxTS chkL00R14;
        private System.Windows.Forms.CheckBoxTS chkL00R15;
        private System.Windows.Forms.CheckBoxTS chkL00R16;
        private System.Windows.Forms.LabelTS lblR1;
        private System.Windows.Forms.LabelTS lblR2;
        private System.Windows.Forms.LabelTS lblR3;
        private System.Windows.Forms.LabelTS lblR4;
        private System.Windows.Forms.LabelTS lblR5;
        private System.Windows.Forms.LabelTS lblR6;
        private System.Windows.Forms.LabelTS lblR7;
        private System.Windows.Forms.LabelTS lblR8;
        private System.Windows.Forms.LabelTS lblR9;
        private System.Windows.Forms.LabelTS lblR10;
        private System.Windows.Forms.LabelTS lblR11;
        private System.Windows.Forms.LabelTS lblR12;
        private System.Windows.Forms.LabelTS lblR13;
        private System.Windows.Forms.LabelTS lblR14;
        private System.Windows.Forms.LabelTS lblR15;
        private System.Windows.Forms.LabelTS lblR16;
        private System.Windows.Forms.LabelTS lblAddr;
        private System.Windows.Forms.LabelTS lblLine0;
        private System.Windows.Forms.ButtonTS btnWriteLine0;
        private System.Windows.Forms.ButtonTS btnWriteAll;
        private System.Windows.Forms.RadioButtonTS radLine0;
        private System.Windows.Forms.LabelTS lblRelays;
        private System.Windows.Forms.RadioButtonTS radLine1;
        private System.Windows.Forms.RadioButtonTS radLine2;
        private System.Windows.Forms.RadioButtonTS radLine3;
        private System.Windows.Forms.RadioButtonTS radLine4;
        private System.Windows.Forms.RadioButtonTS radLine5;
        private System.Windows.Forms.RadioButtonTS radLine6;
        private System.Windows.Forms.RadioButtonTS radLine7;
        private System.Windows.Forms.RadioButtonTS radLine8;
        private System.Windows.Forms.RadioButtonTS radLine9;
        private System.Windows.Forms.RadioButtonTS radLine10;
        private System.Windows.Forms.RadioButtonTS radLine11;
        private System.Windows.Forms.RadioButtonTS radLine12;
        private System.Windows.Forms.RadioButtonTS radLine13;
        private System.Windows.Forms.RadioButtonTS radLine14;
        private System.Windows.Forms.RadioButtonTS radLine15;
        private System.Windows.Forms.ButtonTS btnWriteLine1;
        private System.Windows.Forms.ButtonTS btnWriteLine2;
        private System.Windows.Forms.ButtonTS btnWriteLine3;
        private System.Windows.Forms.ButtonTS btnWriteLine4;
        private System.Windows.Forms.ButtonTS btnWriteLine5;
        private System.Windows.Forms.ButtonTS btnWriteLine6;
        private System.Windows.Forms.ButtonTS btnWriteLine7;
        private System.Windows.Forms.ButtonTS btnWriteLine8;
        private System.Windows.Forms.ButtonTS btnWriteLine9;
        private System.Windows.Forms.ButtonTS btnWriteLine10;
        private System.Windows.Forms.ButtonTS btnWriteLine11;
        private System.Windows.Forms.ButtonTS btnWriteLine12;
        private System.Windows.Forms.ButtonTS btnWriteLine13;
        private System.Windows.Forms.ButtonTS btnWriteLine14;
        private System.Windows.Forms.ButtonTS btnWriteLine15;
        private System.Windows.Forms.CheckBoxTS chkL01R09;
        private System.Windows.Forms.CheckBoxTS chkL01R10;
        private System.Windows.Forms.CheckBoxTS chkL01R11;
        private System.Windows.Forms.CheckBoxTS chkL01R12;
        private System.Windows.Forms.CheckBoxTS chkL01R13;
        private System.Windows.Forms.CheckBoxTS chkL01R14;
        private System.Windows.Forms.CheckBoxTS chkL01R15;
        private System.Windows.Forms.CheckBoxTS chkL01R16;
        private System.Windows.Forms.CheckBoxTS chkL01R08;
        private System.Windows.Forms.CheckBoxTS chkL01R07;
        private System.Windows.Forms.CheckBoxTS chkL01R06;
        private System.Windows.Forms.CheckBoxTS chkL01R05;
        private System.Windows.Forms.CheckBoxTS chkL01R04;
        private System.Windows.Forms.CheckBoxTS chkL01R03;
        private System.Windows.Forms.CheckBoxTS chkL01R02;
        private System.Windows.Forms.CheckBoxTS chkL01R01;
        private System.Windows.Forms.CheckBoxTS chkL02R09;
        private System.Windows.Forms.CheckBoxTS chkL02R10;
        private System.Windows.Forms.CheckBoxTS chkL02R11;
        private System.Windows.Forms.CheckBoxTS chkL02R12;
        private System.Windows.Forms.CheckBoxTS chkL02R13;
        private System.Windows.Forms.CheckBoxTS chkL02R14;
        private System.Windows.Forms.CheckBoxTS chkL02R15;
        private System.Windows.Forms.CheckBoxTS chkL02R16;
        private System.Windows.Forms.CheckBoxTS chkL02R08;
        private System.Windows.Forms.CheckBoxTS chkL02R07;
        private System.Windows.Forms.CheckBoxTS chkL02R06;
        private System.Windows.Forms.CheckBoxTS chkL02R05;
        private System.Windows.Forms.CheckBoxTS chkL02R04;
        private System.Windows.Forms.CheckBoxTS chkL02R03;
        private System.Windows.Forms.CheckBoxTS chkL02R02;
        private System.Windows.Forms.CheckBoxTS chkL02R01;
        private System.Windows.Forms.CheckBoxTS chkL03R09;
        private System.Windows.Forms.CheckBoxTS chkL03R10;
        private System.Windows.Forms.CheckBoxTS chkL03R11;
        private System.Windows.Forms.CheckBoxTS chkL03R12;
        private System.Windows.Forms.CheckBoxTS chkL03R13;
        private System.Windows.Forms.CheckBoxTS chkL03R14;
        private System.Windows.Forms.CheckBoxTS chkL03R15;
        private System.Windows.Forms.CheckBoxTS chkL03R16;
        private System.Windows.Forms.CheckBoxTS chkL03R08;
        private System.Windows.Forms.CheckBoxTS chkL03R07;
        private System.Windows.Forms.CheckBoxTS chkL03R06;
        private System.Windows.Forms.CheckBoxTS chkL03R05;
        private System.Windows.Forms.CheckBoxTS chkL03R04;
        private System.Windows.Forms.CheckBoxTS chkL03R03;
        private System.Windows.Forms.CheckBoxTS chkL03R02;
        private System.Windows.Forms.CheckBoxTS chkL03R01;
        private System.Windows.Forms.CheckBoxTS chkL04R09;
        private System.Windows.Forms.CheckBoxTS chkL04R10;
        private System.Windows.Forms.CheckBoxTS chkL04R11;
        private System.Windows.Forms.CheckBoxTS chkL04R12;
        private System.Windows.Forms.CheckBoxTS chkL04R13;
        private System.Windows.Forms.CheckBoxTS chkL04R14;
        private System.Windows.Forms.CheckBoxTS chkL04R15;
        private System.Windows.Forms.CheckBoxTS chkL04R16;
        private System.Windows.Forms.CheckBoxTS chkL04R08;
        private System.Windows.Forms.CheckBoxTS chkL04R07;
        private System.Windows.Forms.CheckBoxTS chkL04R06;
        private System.Windows.Forms.CheckBoxTS chkL04R05;
        private System.Windows.Forms.CheckBoxTS chkL04R04;
        private System.Windows.Forms.CheckBoxTS chkL04R03;
        private System.Windows.Forms.CheckBoxTS chkL04R02;
        private System.Windows.Forms.CheckBoxTS chkL04R01;
        private System.Windows.Forms.CheckBoxTS chkL05R09;
        private System.Windows.Forms.CheckBoxTS chkL05R10;
        private System.Windows.Forms.CheckBoxTS chkL05R11;
        private System.Windows.Forms.CheckBoxTS chkL05R12;
        private System.Windows.Forms.CheckBoxTS chkL05R13;
        private System.Windows.Forms.CheckBoxTS chkL05R14;
        private System.Windows.Forms.CheckBoxTS chkL05R15;
        private System.Windows.Forms.CheckBoxTS chkL05R16;
        private System.Windows.Forms.CheckBoxTS chkL05R08;
        private System.Windows.Forms.CheckBoxTS chkL05R07;
        private System.Windows.Forms.CheckBoxTS chkL05R06;
        private System.Windows.Forms.CheckBoxTS chkL05R05;
        private System.Windows.Forms.CheckBoxTS chkL05R04;
        private System.Windows.Forms.CheckBoxTS chkL05R03;
        private System.Windows.Forms.CheckBoxTS chkL05R02;
        private System.Windows.Forms.CheckBoxTS chkL05R01;
        private System.Windows.Forms.CheckBoxTS chkL06R09;
        private System.Windows.Forms.CheckBoxTS chkL06R10;
        private System.Windows.Forms.CheckBoxTS chkL06R11;
        private System.Windows.Forms.CheckBoxTS chkL06R12;
        private System.Windows.Forms.CheckBoxTS chkL06R13;
        private System.Windows.Forms.CheckBoxTS chkL06R14;
        private System.Windows.Forms.CheckBoxTS chkL06R15;
        private System.Windows.Forms.CheckBoxTS chkL06R16;
        private System.Windows.Forms.CheckBoxTS chkL06R08;
        private System.Windows.Forms.CheckBoxTS chkL06R07;
        private System.Windows.Forms.CheckBoxTS chkL06R06;
        private System.Windows.Forms.CheckBoxTS chkL06R05;
        private System.Windows.Forms.CheckBoxTS chkL06R04;
        private System.Windows.Forms.CheckBoxTS chkL06R03;
        private System.Windows.Forms.CheckBoxTS chkL06R02;
        private System.Windows.Forms.CheckBoxTS chkL06R01;
        private System.Windows.Forms.CheckBoxTS chkL07R09;
        private System.Windows.Forms.CheckBoxTS chkL07R10;
        private System.Windows.Forms.CheckBoxTS chkL07R11;
        private System.Windows.Forms.CheckBoxTS chkL07R12;
        private System.Windows.Forms.CheckBoxTS chkL07R13;
        private System.Windows.Forms.CheckBoxTS chkL07R14;
        private System.Windows.Forms.CheckBoxTS chkL07R15;
        private System.Windows.Forms.CheckBoxTS chkL07R16;
        private System.Windows.Forms.CheckBoxTS chkL07R08;
        private System.Windows.Forms.CheckBoxTS chkL07R07;
        private System.Windows.Forms.CheckBoxTS chkL07R06;
        private System.Windows.Forms.CheckBoxTS chkL07R05;
        private System.Windows.Forms.CheckBoxTS chkL07R04;
        private System.Windows.Forms.CheckBoxTS chkL07R03;
        private System.Windows.Forms.CheckBoxTS chkL07R02;
        private System.Windows.Forms.CheckBoxTS chkL07R01;
        private System.Windows.Forms.CheckBoxTS chkL08R09;
        private System.Windows.Forms.CheckBoxTS chkL08R10;
        private System.Windows.Forms.CheckBoxTS chkL08R11;
        private System.Windows.Forms.CheckBoxTS chkL08R12;
        private System.Windows.Forms.CheckBoxTS chkL08R13;
        private System.Windows.Forms.CheckBoxTS chkL08R14;
        private System.Windows.Forms.CheckBoxTS chkL08R15;
        private System.Windows.Forms.CheckBoxTS chkL08R16;
        private System.Windows.Forms.CheckBoxTS chkL08R08;
        private System.Windows.Forms.CheckBoxTS chkL08R07;
        private System.Windows.Forms.CheckBoxTS chkL08R06;
        private System.Windows.Forms.CheckBoxTS chkL08R05;
        private System.Windows.Forms.CheckBoxTS chkL08R04;
        private System.Windows.Forms.CheckBoxTS chkL08R03;
        private System.Windows.Forms.CheckBoxTS chkL08R02;
        private System.Windows.Forms.CheckBoxTS chkL08R01;
        private System.Windows.Forms.CheckBoxTS chkL09R09;
        private System.Windows.Forms.CheckBoxTS chkL09R10;
        private System.Windows.Forms.CheckBoxTS chkL09R11;
        private System.Windows.Forms.CheckBoxTS chkL09R12;
        private System.Windows.Forms.CheckBoxTS chkL09R13;
        private System.Windows.Forms.CheckBoxTS chkL09R14;
        private System.Windows.Forms.CheckBoxTS chkL09R15;
        private System.Windows.Forms.CheckBoxTS chkL09R16;
        private System.Windows.Forms.CheckBoxTS chkL09R08;
        private System.Windows.Forms.CheckBoxTS chkL09R07;
        private System.Windows.Forms.CheckBoxTS chkL09R06;
        private System.Windows.Forms.CheckBoxTS chkL09R05;
        private System.Windows.Forms.CheckBoxTS chkL09R04;
        private System.Windows.Forms.CheckBoxTS chkL09R03;
        private System.Windows.Forms.CheckBoxTS chkL09R02;
        private System.Windows.Forms.CheckBoxTS chkL09R01;
        private System.Windows.Forms.CheckBoxTS chkL10R09;
        private System.Windows.Forms.CheckBoxTS chkL10R10;
        private System.Windows.Forms.CheckBoxTS chkL10R11;
        private System.Windows.Forms.CheckBoxTS chkL10R12;
        private System.Windows.Forms.CheckBoxTS chkL10R13;
        private System.Windows.Forms.CheckBoxTS chkL10R14;
        private System.Windows.Forms.CheckBoxTS chkL10R15;
        private System.Windows.Forms.CheckBoxTS chkL10R16;
        private System.Windows.Forms.CheckBoxTS chkL10R08;
        private System.Windows.Forms.CheckBoxTS chkL10R07;
        private System.Windows.Forms.CheckBoxTS chkL10R06;
        private System.Windows.Forms.CheckBoxTS chkL10R05;
        private System.Windows.Forms.CheckBoxTS chkL10R04;
        private System.Windows.Forms.CheckBoxTS chkL10R03;
        private System.Windows.Forms.CheckBoxTS chkL10R02;
        private System.Windows.Forms.CheckBoxTS chkL10R01;
        private System.Windows.Forms.CheckBoxTS chkL11R09;
        private System.Windows.Forms.CheckBoxTS chkL11R10;
        private System.Windows.Forms.CheckBoxTS chkL11R11;
        private System.Windows.Forms.CheckBoxTS chkL11R12;
        private System.Windows.Forms.CheckBoxTS chkL11R13;
        private System.Windows.Forms.CheckBoxTS chkL11R14;
        private System.Windows.Forms.CheckBoxTS chkL11R15;
        private System.Windows.Forms.CheckBoxTS chkL11R16;
        private System.Windows.Forms.CheckBoxTS chkL11R08;
        private System.Windows.Forms.CheckBoxTS chkL11R07;
        private System.Windows.Forms.CheckBoxTS chkL11R06;
        private System.Windows.Forms.CheckBoxTS chkL11R05;
        private System.Windows.Forms.CheckBoxTS chkL11R04;
        private System.Windows.Forms.CheckBoxTS chkL11R03;
        private System.Windows.Forms.CheckBoxTS chkL11R02;
        private System.Windows.Forms.CheckBoxTS chkL11R01;
        private System.Windows.Forms.CheckBoxTS chkL12R09;
        private System.Windows.Forms.CheckBoxTS chkL12R10;
        private System.Windows.Forms.CheckBoxTS chkL12R11;
        private System.Windows.Forms.CheckBoxTS chkL12R12;
        private System.Windows.Forms.CheckBoxTS chkL12R13;
        private System.Windows.Forms.CheckBoxTS chkL12R14;
        private System.Windows.Forms.CheckBoxTS chkL12R15;
        private System.Windows.Forms.CheckBoxTS chkL12R16;
        private System.Windows.Forms.CheckBoxTS chkL12R08;
        private System.Windows.Forms.CheckBoxTS chkL12R07;
        private System.Windows.Forms.CheckBoxTS chkL12R06;
        private System.Windows.Forms.CheckBoxTS chkL12R05;
        private System.Windows.Forms.CheckBoxTS chkL12R04;
        private System.Windows.Forms.CheckBoxTS chkL12R03;
        private System.Windows.Forms.CheckBoxTS chkL12R02;
        private System.Windows.Forms.CheckBoxTS chkL12R01;
        private System.Windows.Forms.CheckBoxTS chkL13R09;
        private System.Windows.Forms.CheckBoxTS chkL13R10;
        private System.Windows.Forms.CheckBoxTS chkL13R11;
        private System.Windows.Forms.CheckBoxTS chkL13R12;
        private System.Windows.Forms.CheckBoxTS chkL13R13;
        private System.Windows.Forms.CheckBoxTS chkL13R14;
        private System.Windows.Forms.CheckBoxTS chkL13R15;
        private System.Windows.Forms.CheckBoxTS chkL13R16;
        private System.Windows.Forms.CheckBoxTS chkL13R08;
        private System.Windows.Forms.CheckBoxTS chkL13R07;
        private System.Windows.Forms.CheckBoxTS chkL13R06;
        private System.Windows.Forms.CheckBoxTS chkL13R05;
        private System.Windows.Forms.CheckBoxTS chkL13R04;
        private System.Windows.Forms.CheckBoxTS chkL13R03;
        private System.Windows.Forms.CheckBoxTS chkL13R02;
        private System.Windows.Forms.CheckBoxTS chkL13R01;
        private System.Windows.Forms.CheckBoxTS chkL14R09;
        private System.Windows.Forms.CheckBoxTS chkL14R10;
        private System.Windows.Forms.CheckBoxTS chkL14R11;
        private System.Windows.Forms.CheckBoxTS chkL14R12;
        private System.Windows.Forms.CheckBoxTS chkL14R13;
        private System.Windows.Forms.CheckBoxTS chkL14R14;
        private System.Windows.Forms.CheckBoxTS chkL14R15;
        private System.Windows.Forms.CheckBoxTS chkL14R16;
        private System.Windows.Forms.CheckBoxTS chkL14R08;
        private System.Windows.Forms.CheckBoxTS chkL14R07;
        private System.Windows.Forms.CheckBoxTS chkL14R06;
        private System.Windows.Forms.CheckBoxTS chkL14R05;
        private System.Windows.Forms.CheckBoxTS chkL14R04;
        private System.Windows.Forms.CheckBoxTS chkL14R03;
        private System.Windows.Forms.CheckBoxTS chkL14R02;
        private System.Windows.Forms.CheckBoxTS chkL14R01;
        private System.Windows.Forms.CheckBoxTS chkL15R09;
        private System.Windows.Forms.CheckBoxTS chkL15R10;
        private System.Windows.Forms.CheckBoxTS chkL15R11;
        private System.Windows.Forms.CheckBoxTS chkL15R12;
        private System.Windows.Forms.CheckBoxTS chkL15R13;
        private System.Windows.Forms.CheckBoxTS chkL15R14;
        private System.Windows.Forms.CheckBoxTS chkL15R15;
        private System.Windows.Forms.CheckBoxTS chkL15R16;
        private System.Windows.Forms.CheckBoxTS chkL15R08;
        private System.Windows.Forms.CheckBoxTS chkL15R07;
        private System.Windows.Forms.CheckBoxTS chkL15R06;
        private System.Windows.Forms.CheckBoxTS chkL15R05;
        private System.Windows.Forms.CheckBoxTS chkL15R04;
        private System.Windows.Forms.CheckBoxTS chkL15R03;
        private System.Windows.Forms.CheckBoxTS chkL15R02;
        private System.Windows.Forms.CheckBoxTS chkL15R01;
        private System.Windows.Forms.LabelTS lblLine1;
        private System.Windows.Forms.LabelTS lblLine2;
        private System.Windows.Forms.LabelTS lblLine3;
        private System.Windows.Forms.LabelTS lblLine4;
        private System.Windows.Forms.LabelTS lblLine5;
        private System.Windows.Forms.LabelTS lblLine6;
        private System.Windows.Forms.LabelTS lblLine7;
        private System.Windows.Forms.LabelTS lblLine8;
        private System.Windows.Forms.LabelTS lblLine9;
        private System.Windows.Forms.LabelTS lblLine10;
        private System.Windows.Forms.LabelTS lblLine11;
        private System.Windows.Forms.LabelTS lblLine12;
        private System.Windows.Forms.LabelTS lblLine13;
        private System.Windows.Forms.LabelTS lblLine14;
        private System.Windows.Forms.LabelTS lblLine15;
        private System.Windows.Forms.LabelTS lblDelay;
        private System.Windows.Forms.ComboBoxTS comboDelay;
        private System.Windows.Forms.Button btnSetDelay;
        private System.Windows.Forms.Button btnSetupXVTR;
        private System.Windows.Forms.CheckBoxTS chkFlexWire;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.ComponentModel.IContainer components;

      

    }
}
