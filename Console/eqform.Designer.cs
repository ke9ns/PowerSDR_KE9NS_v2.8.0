//=================================================================
// eqform.cs
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
    /// Summary description for EQForm.
    /// </summary>
    public partial class EQForm : System.Windows.Forms.Form
    {
      
        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EQForm));
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.labelTS18 = new System.Windows.Forms.LabelTS();
            this.chkAlwaysOnTop1 = new System.Windows.Forms.CheckBoxTS();
            this.rad28Band = new System.Windows.Forms.RadioButtonTS();
            this.rad10Band = new System.Windows.Forms.RadioButtonTS();
            this.rad3Band = new System.Windows.Forms.RadioButtonTS();
            this.grpRXEQ = new System.Windows.Forms.GroupBoxTS();
            this.labelTS8 = new System.Windows.Forms.LabelTS();
            this.lblRXEQ15db2 = new System.Windows.Forms.LabelTS();
            this.lblRXEQ0dB2 = new System.Windows.Forms.LabelTS();
            this.lblRXEQminus12db2 = new System.Windows.Forms.LabelTS();
            this.lblRXEQ10 = new System.Windows.Forms.LabelTS();
            this.tbRXEQ10 = new System.Windows.Forms.TrackBarTS();
            this.lblRXEQ7 = new System.Windows.Forms.LabelTS();
            this.lblRXEQ8 = new System.Windows.Forms.LabelTS();
            this.lblRXEQ9 = new System.Windows.Forms.LabelTS();
            this.tbRXEQ7 = new System.Windows.Forms.TrackBarTS();
            this.tbRXEQ8 = new System.Windows.Forms.TrackBarTS();
            this.tbRXEQ9 = new System.Windows.Forms.TrackBarTS();
            this.tbRXEQ4 = new System.Windows.Forms.TrackBarTS();
            this.tbRXEQ5 = new System.Windows.Forms.TrackBarTS();
            this.tbRXEQ6 = new System.Windows.Forms.TrackBarTS();
            this.lblRXEQ4 = new System.Windows.Forms.LabelTS();
            this.lblRXEQ5 = new System.Windows.Forms.LabelTS();
            this.lblRXEQ6 = new System.Windows.Forms.LabelTS();
            this.picRXEQ = new System.Windows.Forms.PictureBox();
            this.btnRXEQReset = new System.Windows.Forms.ButtonTS();
            this.chkRXEQEnabled = new System.Windows.Forms.CheckBoxTS();
            this.tbRXEQ1 = new System.Windows.Forms.TrackBarTS();
            this.tbRXEQ2 = new System.Windows.Forms.TrackBarTS();
            this.tbRXEQ3 = new System.Windows.Forms.TrackBarTS();
            this.lblRXEQ1 = new System.Windows.Forms.LabelTS();
            this.lblRXEQ2 = new System.Windows.Forms.LabelTS();
            this.lblRXEQ3 = new System.Windows.Forms.LabelTS();
            this.lblRXEQPreamp = new System.Windows.Forms.LabelTS();
            this.tbRXEQPreamp = new System.Windows.Forms.TrackBarTS();
            this.lblRXEQ15db = new System.Windows.Forms.LabelTS();
            this.lblRXEQ0dB = new System.Windows.Forms.LabelTS();
            this.lblRXEQminus12db = new System.Windows.Forms.LabelTS();
            this.grpTXEQ = new System.Windows.Forms.GroupBoxTS();
            this.grpPEQ = new System.Windows.Forms.GroupBoxTS();
            this.labelTS10 = new System.Windows.Forms.LabelTS();
            this.tbTXEQ9Preamp = new System.Windows.Forms.TrackBarTS();
            this.udPEQfreq9 = new System.Windows.Forms.NumericUpDownTS();
            this.udPEQfreq8 = new System.Windows.Forms.NumericUpDownTS();
            this.udPEQfreq7 = new System.Windows.Forms.NumericUpDownTS();
            this.udPEQfreq6 = new System.Windows.Forms.NumericUpDownTS();
            this.udPEQfreq5 = new System.Windows.Forms.NumericUpDownTS();
            this.udPEQfreq4 = new System.Windows.Forms.NumericUpDownTS();
            this.udPEQfreq3 = new System.Windows.Forms.NumericUpDownTS();
            this.udPEQfreq2 = new System.Windows.Forms.NumericUpDownTS();
            this.udPEQfreq1 = new System.Windows.Forms.NumericUpDownTS();
            this.udPEQoctave9 = new System.Windows.Forms.NumericUpDownTS();
            this.tbPEQ9 = new System.Windows.Forms.TrackBarTS();
            this.labelTS16 = new System.Windows.Forms.LabelTS();
            this.labelTS15 = new System.Windows.Forms.LabelTS();
            this.udPEQoctave8 = new System.Windows.Forms.NumericUpDownTS();
            this.tbPEQ8 = new System.Windows.Forms.TrackBarTS();
            this.udPEQoctave7 = new System.Windows.Forms.NumericUpDownTS();
            this.tbPEQ7 = new System.Windows.Forms.TrackBarTS();
            this.udPEQoctave6 = new System.Windows.Forms.NumericUpDownTS();
            this.tbPEQ6 = new System.Windows.Forms.TrackBarTS();
            this.udPEQoctave5 = new System.Windows.Forms.NumericUpDownTS();
            this.tbPEQ5 = new System.Windows.Forms.TrackBarTS();
            this.udPEQoctave4 = new System.Windows.Forms.NumericUpDownTS();
            this.tbPEQ4 = new System.Windows.Forms.TrackBarTS();
            this.udPEQoctave3 = new System.Windows.Forms.NumericUpDownTS();
            this.tbPEQ3 = new System.Windows.Forms.TrackBarTS();
            this.udPEQoctave2 = new System.Windows.Forms.NumericUpDownTS();
            this.tbPEQ2 = new System.Windows.Forms.TrackBarTS();
            this.udPEQoctave1 = new System.Windows.Forms.NumericUpDownTS();
            this.tbPEQ1 = new System.Windows.Forms.TrackBarTS();
            this.labelTS11 = new System.Windows.Forms.LabelTS();
            this.labelTS12 = new System.Windows.Forms.LabelTS();
            this.labelTS13 = new System.Windows.Forms.LabelTS();
            this.labelTS14 = new System.Windows.Forms.LabelTS();
            this.btnTXPEQReset = new System.Windows.Forms.ButtonTS();
            this.labelTS43 = new System.Windows.Forms.LabelTS();
            this.labelTS44 = new System.Windows.Forms.LabelTS();
            this.grpTXEQ28 = new System.Windows.Forms.GroupBoxTS();
            this.labelTS17 = new System.Windows.Forms.LabelTS();
            this.tbTXEQ28Preamp = new System.Windows.Forms.TrackBarTS();
            this.labelTS7 = new System.Windows.Forms.LabelTS();
            this.labelTS5 = new System.Windows.Forms.LabelTS();
            this.labelTS4 = new System.Windows.Forms.LabelTS();
            this.labelTS2 = new System.Windows.Forms.LabelTS();
            this.labelTS1 = new System.Windows.Forms.LabelTS();
            this.lbl2815 = new System.Windows.Forms.LabelTS();
            this.lbl2814 = new System.Windows.Forms.LabelTS();
            this.btnTXEQReset28 = new System.Windows.Forms.ButtonTS();
            this.lbl2825 = new System.Windows.Forms.LabelTS();
            this.lbl2821 = new System.Windows.Forms.LabelTS();
            this.lbl2828 = new System.Windows.Forms.LabelTS();
            this.lbl2827 = new System.Windows.Forms.LabelTS();
            this.tbTX28EQ28 = new System.Windows.Forms.TrackBarTS();
            this.tbTX28EQ27 = new System.Windows.Forms.TrackBarTS();
            this.lbl2826 = new System.Windows.Forms.LabelTS();
            this.tbTX28EQ26 = new System.Windows.Forms.TrackBarTS();
            this.tbTX28EQ25 = new System.Windows.Forms.TrackBarTS();
            this.lbl2824 = new System.Windows.Forms.LabelTS();
            this.lbl2823 = new System.Windows.Forms.LabelTS();
            this.tbTX28EQ24 = new System.Windows.Forms.TrackBarTS();
            this.tbTX28EQ23 = new System.Windows.Forms.TrackBarTS();
            this.lbl2822 = new System.Windows.Forms.LabelTS();
            this.tbTX28EQ22 = new System.Windows.Forms.TrackBarTS();
            this.lbl2820 = new System.Windows.Forms.LabelTS();
            this.tbTX28EQ21 = new System.Windows.Forms.TrackBarTS();
            this.tbTX28EQ20 = new System.Windows.Forms.TrackBarTS();
            this.lbl2819 = new System.Windows.Forms.LabelTS();
            this.tbTX28EQ19 = new System.Windows.Forms.TrackBarTS();
            this.tbTX28EQ18 = new System.Windows.Forms.TrackBarTS();
            this.lbl2818 = new System.Windows.Forms.LabelTS();
            this.lbl2817 = new System.Windows.Forms.LabelTS();
            this.lbl2816 = new System.Windows.Forms.LabelTS();
            this.tbTX28EQ17 = new System.Windows.Forms.TrackBarTS();
            this.tbTX28EQ16 = new System.Windows.Forms.TrackBarTS();
            this.tbTX28EQ15 = new System.Windows.Forms.TrackBarTS();
            this.lbl287 = new System.Windows.Forms.LabelTS();
            this.lbl2813 = new System.Windows.Forms.LabelTS();
            this.tbTX28EQ14 = new System.Windows.Forms.TrackBarTS();
            this.tbTX28EQ13 = new System.Windows.Forms.TrackBarTS();
            this.lbl2812 = new System.Windows.Forms.LabelTS();
            this.tbTX28EQ12 = new System.Windows.Forms.TrackBarTS();
            this.tbTX28EQ11 = new System.Windows.Forms.TrackBarTS();
            this.lbl2811 = new System.Windows.Forms.LabelTS();
            this.lbl2810 = new System.Windows.Forms.LabelTS();
            this.lbl289 = new System.Windows.Forms.LabelTS();
            this.tbTX28EQ10 = new System.Windows.Forms.TrackBarTS();
            this.tbTX28EQ9 = new System.Windows.Forms.TrackBarTS();
            this.lbl288 = new System.Windows.Forms.LabelTS();
            this.tbTX28EQ8 = new System.Windows.Forms.TrackBarTS();
            this.lbl286 = new System.Windows.Forms.LabelTS();
            this.tbTX28EQ7 = new System.Windows.Forms.TrackBarTS();
            this.tbTX28EQ6 = new System.Windows.Forms.TrackBarTS();
            this.lbl285 = new System.Windows.Forms.LabelTS();
            this.tbTX28EQ5 = new System.Windows.Forms.TrackBarTS();
            this.tbTX28EQ4 = new System.Windows.Forms.TrackBarTS();
            this.lbl284 = new System.Windows.Forms.LabelTS();
            this.lbl283 = new System.Windows.Forms.LabelTS();
            this.lbl282 = new System.Windows.Forms.LabelTS();
            this.tbTX28EQ3 = new System.Windows.Forms.TrackBarTS();
            this.tbTX28EQ2 = new System.Windows.Forms.TrackBarTS();
            this.lbl281 = new System.Windows.Forms.LabelTS();
            this.tbTX28EQ1 = new System.Windows.Forms.TrackBarTS();
            this.labelTS6 = new System.Windows.Forms.LabelTS();
            this.labelTS3 = new System.Windows.Forms.LabelTS();
            this.btnTXEQReset = new System.Windows.Forms.ButtonTS();
            this.chkTXEQ160Notch = new System.Windows.Forms.CheckBoxTS();
            this.picTXEQ = new System.Windows.Forms.PictureBox();
            this.lblTXEQ15db2 = new System.Windows.Forms.LabelTS();
            this.lblTXEQ0dB2 = new System.Windows.Forms.LabelTS();
            this.lblTXEQminus12db2 = new System.Windows.Forms.LabelTS();
            this.tbTXEQ10 = new System.Windows.Forms.TrackBarTS();
            this.lblTXEQ10 = new System.Windows.Forms.LabelTS();
            this.tbTXEQ7 = new System.Windows.Forms.TrackBarTS();
            this.tbTXEQ8 = new System.Windows.Forms.TrackBarTS();
            this.tbTXEQ9 = new System.Windows.Forms.TrackBarTS();
            this.lblTXEQ7 = new System.Windows.Forms.LabelTS();
            this.lblTXEQ8 = new System.Windows.Forms.LabelTS();
            this.lblTXEQ9 = new System.Windows.Forms.LabelTS();
            this.tbTXEQ4 = new System.Windows.Forms.TrackBarTS();
            this.tbTXEQ5 = new System.Windows.Forms.TrackBarTS();
            this.tbTXEQ6 = new System.Windows.Forms.TrackBarTS();
            this.lblTXEQ4 = new System.Windows.Forms.LabelTS();
            this.lblTXEQ5 = new System.Windows.Forms.LabelTS();
            this.lblTXEQ6 = new System.Windows.Forms.LabelTS();
            this.chkTXEQEnabled = new System.Windows.Forms.CheckBoxTS();
            this.tbTXEQ1 = new System.Windows.Forms.TrackBarTS();
            this.tbTXEQ2 = new System.Windows.Forms.TrackBarTS();
            this.tbTXEQ3 = new System.Windows.Forms.TrackBarTS();
            this.lblTXEQ1 = new System.Windows.Forms.LabelTS();
            this.lblTXEQ2 = new System.Windows.Forms.LabelTS();
            this.lblTXEQ3 = new System.Windows.Forms.LabelTS();
            this.lblTXEQPreamp = new System.Windows.Forms.LabelTS();
            this.tbTXEQPreamp = new System.Windows.Forms.TrackBarTS();
            this.lblTXEQ15db = new System.Windows.Forms.LabelTS();
            this.lblTXEQ0dB = new System.Windows.Forms.LabelTS();
            this.lblTXEQminus12db = new System.Windows.Forms.LabelTS();
            this.labelTS9 = new System.Windows.Forms.LabelTS();
            this.chkBothEQ = new System.Windows.Forms.CheckBoxTS();
            this.radPEQ = new System.Windows.Forms.RadioButtonTS();
            this.grpRXEQ.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbRXEQ10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbRXEQ7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbRXEQ8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbRXEQ9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbRXEQ4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbRXEQ5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbRXEQ6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picRXEQ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbRXEQ1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbRXEQ2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbRXEQ3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbRXEQPreamp)).BeginInit();
            this.grpTXEQ.SuspendLayout();
            this.grpPEQ.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbTXEQ9Preamp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPEQfreq9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPEQfreq8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPEQfreq7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPEQfreq6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPEQfreq5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPEQfreq4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPEQfreq3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPEQfreq2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPEQfreq1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPEQoctave9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbPEQ9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPEQoctave8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbPEQ8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPEQoctave7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbPEQ7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPEQoctave6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbPEQ6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPEQoctave5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbPEQ5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPEQoctave4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbPEQ4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPEQoctave3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbPEQ3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPEQoctave2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbPEQ2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPEQoctave1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbPEQ1)).BeginInit();
            this.grpTXEQ28.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbTXEQ28Preamp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ28)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ27)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ26)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ25)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ24)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ23)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ22)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ21)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ20)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ19)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ18)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ17)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ16)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ15)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ14)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ13)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ12)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTXEQ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTXEQ10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTXEQ7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTXEQ8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTXEQ9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTXEQ4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTXEQ5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTXEQ6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTXEQ1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTXEQ2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTXEQ3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTXEQPreamp)).BeginInit();
            this.SuspendLayout();
            // 
            // toolTip1
            // 
            this.toolTip1.AutomaticDelay = 200;
            this.toolTip1.AutoPopDelay = 5000;
            this.toolTip1.InitialDelay = 200;
            this.toolTip1.IsBalloon = true;
            this.toolTip1.ReshowDelay = 20;
            // 
            // labelTS18
            // 
            this.labelTS18.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTS18.Image = null;
            this.labelTS18.Location = new System.Drawing.Point(555, 51);
            this.labelTS18.Name = "labelTS18";
            this.labelTS18.Size = new System.Drawing.Size(285, 218);
            this.labelTS18.TabIndex = 292;
            this.labelTS18.Text = resources.GetString("labelTS18.Text");
            // 
            // chkAlwaysOnTop1
            // 
            this.chkAlwaysOnTop1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.chkAlwaysOnTop1.Image = null;
            this.chkAlwaysOnTop1.Location = new System.Drawing.Point(770, 10);
            this.chkAlwaysOnTop1.Name = "chkAlwaysOnTop1";
            this.chkAlwaysOnTop1.Size = new System.Drawing.Size(103, 24);
            this.chkAlwaysOnTop1.TabIndex = 187;
            this.chkAlwaysOnTop1.Text = "Always On Top";
            this.chkAlwaysOnTop1.CheckedChanged += new System.EventHandler(this.ChkAlwaysOnTop1_CheckedChanged);
            // 
            // rad28Band
            // 
            this.rad28Band.Image = null;
            this.rad28Band.Location = new System.Drawing.Point(211, 10);
            this.rad28Band.Name = "rad28Band";
            this.rad28Band.Size = new System.Drawing.Size(149, 24);
            this.rad28Band.TabIndex = 186;
            this.rad28Band.Text = "28-Band TX Graphic EQ";
            this.toolTip1.SetToolTip(this.rad28Band, "10 RX EQ Bands with 1 octave Band Width each, and 28 TX EQ Bands with 1/3 octave " +
        "Band Width each");
            this.rad28Band.Click += new System.EventHandler(this.rad28Band_CheckedChanged);
            // 
            // rad10Band
            // 
            this.rad10Band.Image = null;
            this.rad10Band.Location = new System.Drawing.Point(80, 10);
            this.rad10Band.Name = "rad10Band";
            this.rad10Band.Size = new System.Drawing.Size(129, 24);
            this.rad10Band.TabIndex = 3;
            this.rad10Band.Text = "10-Band Graphic EQ";
            this.toolTip1.SetToolTip(this.rad10Band, "10 RX and TX EQ Bands with 1 octave Band Width each");
            this.rad10Band.Click += new System.EventHandler(this.rad10Band_CheckedChanged);
            // 
            // rad3Band
            // 
            this.rad3Band.Checked = true;
            this.rad3Band.Image = null;
            this.rad3Band.Location = new System.Drawing.Point(15, 10);
            this.rad3Band.Name = "rad3Band";
            this.rad3Band.Size = new System.Drawing.Size(65, 24);
            this.rad3Band.TabIndex = 2;
            this.rad3Band.TabStop = true;
            this.rad3Band.Text = "3-Band";
            this.rad3Band.Click += new System.EventHandler(this.rad3Band_CheckedChanged);
            // 
            // grpRXEQ
            // 
            this.grpRXEQ.Controls.Add(this.labelTS8);
            this.grpRXEQ.Controls.Add(this.lblRXEQ15db2);
            this.grpRXEQ.Controls.Add(this.lblRXEQ0dB2);
            this.grpRXEQ.Controls.Add(this.lblRXEQminus12db2);
            this.grpRXEQ.Controls.Add(this.lblRXEQ10);
            this.grpRXEQ.Controls.Add(this.tbRXEQ10);
            this.grpRXEQ.Controls.Add(this.lblRXEQ7);
            this.grpRXEQ.Controls.Add(this.lblRXEQ8);
            this.grpRXEQ.Controls.Add(this.lblRXEQ9);
            this.grpRXEQ.Controls.Add(this.tbRXEQ7);
            this.grpRXEQ.Controls.Add(this.tbRXEQ8);
            this.grpRXEQ.Controls.Add(this.tbRXEQ9);
            this.grpRXEQ.Controls.Add(this.tbRXEQ4);
            this.grpRXEQ.Controls.Add(this.tbRXEQ5);
            this.grpRXEQ.Controls.Add(this.tbRXEQ6);
            this.grpRXEQ.Controls.Add(this.lblRXEQ4);
            this.grpRXEQ.Controls.Add(this.lblRXEQ5);
            this.grpRXEQ.Controls.Add(this.lblRXEQ6);
            this.grpRXEQ.Controls.Add(this.picRXEQ);
            this.grpRXEQ.Controls.Add(this.btnRXEQReset);
            this.grpRXEQ.Controls.Add(this.chkRXEQEnabled);
            this.grpRXEQ.Controls.Add(this.tbRXEQ1);
            this.grpRXEQ.Controls.Add(this.tbRXEQ2);
            this.grpRXEQ.Controls.Add(this.tbRXEQ3);
            this.grpRXEQ.Controls.Add(this.lblRXEQ1);
            this.grpRXEQ.Controls.Add(this.lblRXEQ2);
            this.grpRXEQ.Controls.Add(this.lblRXEQ3);
            this.grpRXEQ.Controls.Add(this.lblRXEQPreamp);
            this.grpRXEQ.Controls.Add(this.tbRXEQPreamp);
            this.grpRXEQ.Controls.Add(this.lblRXEQ15db);
            this.grpRXEQ.Controls.Add(this.lblRXEQ0dB);
            this.grpRXEQ.Controls.Add(this.lblRXEQminus12db);
            this.grpRXEQ.Location = new System.Drawing.Point(8, 40);
            this.grpRXEQ.Name = "grpRXEQ";
            this.grpRXEQ.Size = new System.Drawing.Size(528, 282);
            this.grpRXEQ.TabIndex = 1;
            this.grpRXEQ.TabStop = false;
            this.grpRXEQ.Text = "Receive Equalizer";
            // 
            // labelTS8
            // 
            this.labelTS8.Image = null;
            this.labelTS8.Location = new System.Drawing.Point(72, 260);
            this.labelTS8.Name = "labelTS8";
            this.labelTS8.Size = new System.Drawing.Size(419, 16);
            this.labelTS8.TabIndex = 247;
            this.labelTS8.Text = "Each Band Slider has 1 Octave BW (Example: 88 low - 125center  - 176 high)";
            this.labelTS8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblRXEQ15db2
            // 
            this.lblRXEQ15db2.Image = null;
            this.lblRXEQ15db2.Location = new System.Drawing.Point(478, 144);
            this.lblRXEQ15db2.Name = "lblRXEQ15db2";
            this.lblRXEQ15db2.Size = new System.Drawing.Size(32, 16);
            this.lblRXEQ15db2.TabIndex = 126;
            this.lblRXEQ15db2.Text = "15dB";
            this.lblRXEQ15db2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblRXEQ0dB2
            // 
            this.lblRXEQ0dB2.Image = null;
            this.lblRXEQ0dB2.Location = new System.Drawing.Point(478, 200);
            this.lblRXEQ0dB2.Name = "lblRXEQ0dB2";
            this.lblRXEQ0dB2.Size = new System.Drawing.Size(32, 16);
            this.lblRXEQ0dB2.TabIndex = 127;
            this.lblRXEQ0dB2.Text = "  0dB";
            this.lblRXEQ0dB2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblRXEQminus12db2
            // 
            this.lblRXEQminus12db2.Image = null;
            this.lblRXEQminus12db2.Location = new System.Drawing.Point(475, 244);
            this.lblRXEQminus12db2.Name = "lblRXEQminus12db2";
            this.lblRXEQminus12db2.Size = new System.Drawing.Size(34, 16);
            this.lblRXEQminus12db2.TabIndex = 128;
            this.lblRXEQminus12db2.Text = "-15dB";
            this.lblRXEQminus12db2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblRXEQ10
            // 
            this.lblRXEQ10.Image = null;
            this.lblRXEQ10.Location = new System.Drawing.Point(435, 122);
            this.lblRXEQ10.Name = "lblRXEQ10";
            this.lblRXEQ10.Size = new System.Drawing.Size(40, 16);
            this.lblRXEQ10.TabIndex = 125;
            this.lblRXEQ10.Text = "16K";
            this.lblRXEQ10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblRXEQ10.Visible = false;
            // 
            // tbRXEQ10
            // 
            this.tbRXEQ10.AutoSize = false;
            this.tbRXEQ10.LargeChange = 3;
            this.tbRXEQ10.Location = new System.Drawing.Point(443, 138);
            this.tbRXEQ10.Maximum = 15;
            this.tbRXEQ10.Minimum = -15;
            this.tbRXEQ10.Name = "tbRXEQ10";
            this.tbRXEQ10.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbRXEQ10.Size = new System.Drawing.Size(32, 128);
            this.tbRXEQ10.TabIndex = 124;
            this.tbRXEQ10.TickFrequency = 3;
            this.tbRXEQ10.Visible = false;
            this.tbRXEQ10.Scroll += new System.EventHandler(this.tbRXEQ_Scroll);
            // 
            // lblRXEQ7
            // 
            this.lblRXEQ7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRXEQ7.Image = null;
            this.lblRXEQ7.Location = new System.Drawing.Point(315, 122);
            this.lblRXEQ7.Name = "lblRXEQ7";
            this.lblRXEQ7.Size = new System.Drawing.Size(40, 16);
            this.lblRXEQ7.TabIndex = 121;
            this.lblRXEQ7.Text = "2K";
            this.lblRXEQ7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblRXEQ7.Visible = false;
            // 
            // lblRXEQ8
            // 
            this.lblRXEQ8.Image = null;
            this.lblRXEQ8.Location = new System.Drawing.Point(355, 122);
            this.lblRXEQ8.Name = "lblRXEQ8";
            this.lblRXEQ8.Size = new System.Drawing.Size(40, 16);
            this.lblRXEQ8.TabIndex = 122;
            this.lblRXEQ8.Text = "4K";
            this.lblRXEQ8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblRXEQ8.Visible = false;
            // 
            // lblRXEQ9
            // 
            this.lblRXEQ9.Image = null;
            this.lblRXEQ9.Location = new System.Drawing.Point(395, 122);
            this.lblRXEQ9.Name = "lblRXEQ9";
            this.lblRXEQ9.Size = new System.Drawing.Size(40, 16);
            this.lblRXEQ9.TabIndex = 123;
            this.lblRXEQ9.Text = "High";
            this.lblRXEQ9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.lblRXEQ9, "1500-6000Hz");
            // 
            // tbRXEQ7
            // 
            this.tbRXEQ7.AutoSize = false;
            this.tbRXEQ7.LargeChange = 3;
            this.tbRXEQ7.Location = new System.Drawing.Point(323, 138);
            this.tbRXEQ7.Maximum = 15;
            this.tbRXEQ7.Minimum = -15;
            this.tbRXEQ7.Name = "tbRXEQ7";
            this.tbRXEQ7.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbRXEQ7.Size = new System.Drawing.Size(32, 128);
            this.tbRXEQ7.TabIndex = 118;
            this.tbRXEQ7.TickFrequency = 3;
            this.tbRXEQ7.Visible = false;
            this.tbRXEQ7.Scroll += new System.EventHandler(this.tbRXEQ_Scroll);
            // 
            // tbRXEQ8
            // 
            this.tbRXEQ8.AutoSize = false;
            this.tbRXEQ8.LargeChange = 3;
            this.tbRXEQ8.Location = new System.Drawing.Point(363, 138);
            this.tbRXEQ8.Maximum = 15;
            this.tbRXEQ8.Minimum = -15;
            this.tbRXEQ8.Name = "tbRXEQ8";
            this.tbRXEQ8.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbRXEQ8.Size = new System.Drawing.Size(32, 128);
            this.tbRXEQ8.TabIndex = 119;
            this.tbRXEQ8.TickFrequency = 3;
            this.tbRXEQ8.Visible = false;
            this.tbRXEQ8.Scroll += new System.EventHandler(this.tbRXEQ_Scroll);
            // 
            // tbRXEQ9
            // 
            this.tbRXEQ9.AutoSize = false;
            this.tbRXEQ9.LargeChange = 3;
            this.tbRXEQ9.Location = new System.Drawing.Point(403, 138);
            this.tbRXEQ9.Maximum = 15;
            this.tbRXEQ9.Minimum = -15;
            this.tbRXEQ9.Name = "tbRXEQ9";
            this.tbRXEQ9.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbRXEQ9.Size = new System.Drawing.Size(32, 128);
            this.tbRXEQ9.TabIndex = 120;
            this.tbRXEQ9.TickFrequency = 3;
            this.tbRXEQ9.Scroll += new System.EventHandler(this.tbRXEQ_Scroll);
            // 
            // tbRXEQ4
            // 
            this.tbRXEQ4.AutoSize = false;
            this.tbRXEQ4.LargeChange = 3;
            this.tbRXEQ4.Location = new System.Drawing.Point(203, 138);
            this.tbRXEQ4.Maximum = 15;
            this.tbRXEQ4.Minimum = -15;
            this.tbRXEQ4.Name = "tbRXEQ4";
            this.tbRXEQ4.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbRXEQ4.Size = new System.Drawing.Size(32, 128);
            this.tbRXEQ4.TabIndex = 112;
            this.tbRXEQ4.TickFrequency = 3;
            this.tbRXEQ4.Visible = false;
            this.tbRXEQ4.Scroll += new System.EventHandler(this.tbRXEQ_Scroll);
            // 
            // tbRXEQ5
            // 
            this.tbRXEQ5.AutoSize = false;
            this.tbRXEQ5.LargeChange = 3;
            this.tbRXEQ5.Location = new System.Drawing.Point(243, 138);
            this.tbRXEQ5.Maximum = 15;
            this.tbRXEQ5.Minimum = -15;
            this.tbRXEQ5.Name = "tbRXEQ5";
            this.tbRXEQ5.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbRXEQ5.Size = new System.Drawing.Size(32, 128);
            this.tbRXEQ5.TabIndex = 113;
            this.tbRXEQ5.TickFrequency = 3;
            this.tbRXEQ5.Scroll += new System.EventHandler(this.tbRXEQ_Scroll);
            // 
            // tbRXEQ6
            // 
            this.tbRXEQ6.AutoSize = false;
            this.tbRXEQ6.LargeChange = 3;
            this.tbRXEQ6.Location = new System.Drawing.Point(283, 138);
            this.tbRXEQ6.Maximum = 15;
            this.tbRXEQ6.Minimum = -15;
            this.tbRXEQ6.Name = "tbRXEQ6";
            this.tbRXEQ6.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbRXEQ6.Size = new System.Drawing.Size(32, 128);
            this.tbRXEQ6.TabIndex = 114;
            this.tbRXEQ6.TickFrequency = 3;
            this.tbRXEQ6.Visible = false;
            this.tbRXEQ6.Scroll += new System.EventHandler(this.tbRXEQ_Scroll);
            // 
            // lblRXEQ4
            // 
            this.lblRXEQ4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRXEQ4.Image = null;
            this.lblRXEQ4.Location = new System.Drawing.Point(195, 122);
            this.lblRXEQ4.Name = "lblRXEQ4";
            this.lblRXEQ4.Size = new System.Drawing.Size(40, 16);
            this.lblRXEQ4.TabIndex = 115;
            this.lblRXEQ4.Text = "250";
            this.lblRXEQ4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblRXEQ4.Visible = false;
            // 
            // lblRXEQ5
            // 
            this.lblRXEQ5.Image = null;
            this.lblRXEQ5.Location = new System.Drawing.Point(235, 122);
            this.lblRXEQ5.Name = "lblRXEQ5";
            this.lblRXEQ5.Size = new System.Drawing.Size(40, 16);
            this.lblRXEQ5.TabIndex = 116;
            this.lblRXEQ5.Text = "Mid";
            this.lblRXEQ5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.lblRXEQ5, "400-1500Hz");
            // 
            // lblRXEQ6
            // 
            this.lblRXEQ6.Image = null;
            this.lblRXEQ6.Location = new System.Drawing.Point(275, 122);
            this.lblRXEQ6.Name = "lblRXEQ6";
            this.lblRXEQ6.Size = new System.Drawing.Size(40, 16);
            this.lblRXEQ6.TabIndex = 117;
            this.lblRXEQ6.Text = "1K";
            this.lblRXEQ6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblRXEQ6.Visible = false;
            // 
            // picRXEQ
            // 
            this.picRXEQ.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.picRXEQ.Location = new System.Drawing.Point(88, 24);
            this.picRXEQ.Name = "picRXEQ";
            this.picRXEQ.Size = new System.Drawing.Size(384, 95);
            this.picRXEQ.TabIndex = 111;
            this.picRXEQ.TabStop = false;
            this.picRXEQ.Paint += new System.Windows.Forms.PaintEventHandler(this.picRXEQ_Paint);
            // 
            // btnRXEQReset
            // 
            this.btnRXEQReset.Image = null;
            this.btnRXEQReset.Location = new System.Drawing.Point(16, 80);
            this.btnRXEQReset.Name = "btnRXEQReset";
            this.btnRXEQReset.Size = new System.Drawing.Size(56, 20);
            this.btnRXEQReset.TabIndex = 110;
            this.btnRXEQReset.Text = "Reset";
            this.btnRXEQReset.Click += new System.EventHandler(this.btnRXEQReset_Click);
            // 
            // chkRXEQEnabled
            // 
            this.chkRXEQEnabled.Image = null;
            this.chkRXEQEnabled.Location = new System.Drawing.Point(16, 24);
            this.chkRXEQEnabled.Name = "chkRXEQEnabled";
            this.chkRXEQEnabled.Size = new System.Drawing.Size(72, 16);
            this.chkRXEQEnabled.TabIndex = 109;
            this.chkRXEQEnabled.Text = "Enabled";
            this.chkRXEQEnabled.CheckedChanged += new System.EventHandler(this.chkRXEQEnabled_CheckedChanged);
            // 
            // tbRXEQ1
            // 
            this.tbRXEQ1.AutoSize = false;
            this.tbRXEQ1.LargeChange = 3;
            this.tbRXEQ1.Location = new System.Drawing.Point(83, 138);
            this.tbRXEQ1.Maximum = 15;
            this.tbRXEQ1.Minimum = -15;
            this.tbRXEQ1.Name = "tbRXEQ1";
            this.tbRXEQ1.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbRXEQ1.Size = new System.Drawing.Size(32, 128);
            this.tbRXEQ1.TabIndex = 4;
            this.tbRXEQ1.TickFrequency = 3;
            this.tbRXEQ1.Scroll += new System.EventHandler(this.tbRXEQ_Scroll);
            // 
            // tbRXEQ2
            // 
            this.tbRXEQ2.AutoSize = false;
            this.tbRXEQ2.LargeChange = 3;
            this.tbRXEQ2.Location = new System.Drawing.Point(123, 138);
            this.tbRXEQ2.Maximum = 15;
            this.tbRXEQ2.Minimum = -15;
            this.tbRXEQ2.Name = "tbRXEQ2";
            this.tbRXEQ2.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbRXEQ2.Size = new System.Drawing.Size(32, 128);
            this.tbRXEQ2.TabIndex = 5;
            this.tbRXEQ2.TickFrequency = 3;
            this.tbRXEQ2.Visible = false;
            this.tbRXEQ2.Scroll += new System.EventHandler(this.tbRXEQ_Scroll);
            // 
            // tbRXEQ3
            // 
            this.tbRXEQ3.AutoSize = false;
            this.tbRXEQ3.LargeChange = 3;
            this.tbRXEQ3.Location = new System.Drawing.Point(163, 138);
            this.tbRXEQ3.Maximum = 15;
            this.tbRXEQ3.Minimum = -15;
            this.tbRXEQ3.Name = "tbRXEQ3";
            this.tbRXEQ3.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbRXEQ3.Size = new System.Drawing.Size(32, 128);
            this.tbRXEQ3.TabIndex = 6;
            this.tbRXEQ3.TickFrequency = 3;
            this.tbRXEQ3.Visible = false;
            this.tbRXEQ3.Scroll += new System.EventHandler(this.tbRXEQ_Scroll);
            // 
            // lblRXEQ1
            // 
            this.lblRXEQ1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRXEQ1.Image = null;
            this.lblRXEQ1.Location = new System.Drawing.Point(75, 122);
            this.lblRXEQ1.Name = "lblRXEQ1";
            this.lblRXEQ1.Size = new System.Drawing.Size(40, 16);
            this.lblRXEQ1.TabIndex = 43;
            this.lblRXEQ1.Text = "Low";
            this.lblRXEQ1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.lblRXEQ1, "0-400Hz");
            // 
            // lblRXEQ2
            // 
            this.lblRXEQ2.Image = null;
            this.lblRXEQ2.Location = new System.Drawing.Point(115, 122);
            this.lblRXEQ2.Name = "lblRXEQ2";
            this.lblRXEQ2.Size = new System.Drawing.Size(40, 16);
            this.lblRXEQ2.TabIndex = 44;
            this.lblRXEQ2.Text = "63";
            this.lblRXEQ2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblRXEQ2.Visible = false;
            // 
            // lblRXEQ3
            // 
            this.lblRXEQ3.Image = null;
            this.lblRXEQ3.Location = new System.Drawing.Point(155, 122);
            this.lblRXEQ3.Name = "lblRXEQ3";
            this.lblRXEQ3.Size = new System.Drawing.Size(40, 16);
            this.lblRXEQ3.TabIndex = 45;
            this.lblRXEQ3.Text = "125";
            this.lblRXEQ3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblRXEQ3.Visible = false;
            // 
            // lblRXEQPreamp
            // 
            this.lblRXEQPreamp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRXEQPreamp.Image = null;
            this.lblRXEQPreamp.Location = new System.Drawing.Point(3, 122);
            this.lblRXEQPreamp.Name = "lblRXEQPreamp";
            this.lblRXEQPreamp.Size = new System.Drawing.Size(48, 16);
            this.lblRXEQPreamp.TabIndex = 74;
            this.lblRXEQPreamp.Text = "Preamp";
            this.lblRXEQPreamp.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // tbRXEQPreamp
            // 
            this.tbRXEQPreamp.AutoSize = false;
            this.tbRXEQPreamp.LargeChange = 3;
            this.tbRXEQPreamp.Location = new System.Drawing.Point(11, 138);
            this.tbRXEQPreamp.Maximum = 15;
            this.tbRXEQPreamp.Minimum = -12;
            this.tbRXEQPreamp.Name = "tbRXEQPreamp";
            this.tbRXEQPreamp.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbRXEQPreamp.Size = new System.Drawing.Size(32, 128);
            this.tbRXEQPreamp.TabIndex = 35;
            this.tbRXEQPreamp.TickFrequency = 3;
            this.tbRXEQPreamp.Scroll += new System.EventHandler(this.tbRXEQ_Scroll);
            this.tbRXEQPreamp.MouseHover += new System.EventHandler(this.tbRXEQPreamp_MouseHover);
            // 
            // lblRXEQ15db
            // 
            this.lblRXEQ15db.Image = null;
            this.lblRXEQ15db.Location = new System.Drawing.Point(51, 144);
            this.lblRXEQ15db.Name = "lblRXEQ15db";
            this.lblRXEQ15db.Size = new System.Drawing.Size(32, 16);
            this.lblRXEQ15db.TabIndex = 40;
            this.lblRXEQ15db.Text = "15dB";
            this.lblRXEQ15db.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblRXEQ0dB
            // 
            this.lblRXEQ0dB.Image = null;
            this.lblRXEQ0dB.Location = new System.Drawing.Point(51, 200);
            this.lblRXEQ0dB.Name = "lblRXEQ0dB";
            this.lblRXEQ0dB.Size = new System.Drawing.Size(32, 16);
            this.lblRXEQ0dB.TabIndex = 41;
            this.lblRXEQ0dB.Text = "  0dB";
            this.lblRXEQ0dB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblRXEQminus12db
            // 
            this.lblRXEQminus12db.Image = null;
            this.lblRXEQminus12db.Location = new System.Drawing.Point(47, 244);
            this.lblRXEQminus12db.Name = "lblRXEQminus12db";
            this.lblRXEQminus12db.Size = new System.Drawing.Size(34, 16);
            this.lblRXEQminus12db.TabIndex = 42;
            this.lblRXEQminus12db.Text = "-15dB";
            this.lblRXEQminus12db.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // grpTXEQ
            // 
            this.grpTXEQ.Controls.Add(this.grpPEQ);
            this.grpTXEQ.Controls.Add(this.grpTXEQ28);
            this.grpTXEQ.Controls.Add(this.btnTXEQReset);
            this.grpTXEQ.Controls.Add(this.chkTXEQ160Notch);
            this.grpTXEQ.Controls.Add(this.picTXEQ);
            this.grpTXEQ.Controls.Add(this.lblTXEQ15db2);
            this.grpTXEQ.Controls.Add(this.lblTXEQ0dB2);
            this.grpTXEQ.Controls.Add(this.lblTXEQminus12db2);
            this.grpTXEQ.Controls.Add(this.tbTXEQ10);
            this.grpTXEQ.Controls.Add(this.lblTXEQ10);
            this.grpTXEQ.Controls.Add(this.tbTXEQ7);
            this.grpTXEQ.Controls.Add(this.tbTXEQ8);
            this.grpTXEQ.Controls.Add(this.tbTXEQ9);
            this.grpTXEQ.Controls.Add(this.lblTXEQ7);
            this.grpTXEQ.Controls.Add(this.lblTXEQ8);
            this.grpTXEQ.Controls.Add(this.lblTXEQ9);
            this.grpTXEQ.Controls.Add(this.tbTXEQ4);
            this.grpTXEQ.Controls.Add(this.tbTXEQ5);
            this.grpTXEQ.Controls.Add(this.tbTXEQ6);
            this.grpTXEQ.Controls.Add(this.lblTXEQ4);
            this.grpTXEQ.Controls.Add(this.lblTXEQ5);
            this.grpTXEQ.Controls.Add(this.lblTXEQ6);
            this.grpTXEQ.Controls.Add(this.chkTXEQEnabled);
            this.grpTXEQ.Controls.Add(this.tbTXEQ1);
            this.grpTXEQ.Controls.Add(this.tbTXEQ2);
            this.grpTXEQ.Controls.Add(this.tbTXEQ3);
            this.grpTXEQ.Controls.Add(this.lblTXEQ1);
            this.grpTXEQ.Controls.Add(this.lblTXEQ2);
            this.grpTXEQ.Controls.Add(this.lblTXEQ3);
            this.grpTXEQ.Controls.Add(this.lblTXEQPreamp);
            this.grpTXEQ.Controls.Add(this.tbTXEQPreamp);
            this.grpTXEQ.Controls.Add(this.lblTXEQ15db);
            this.grpTXEQ.Controls.Add(this.lblTXEQ0dB);
            this.grpTXEQ.Controls.Add(this.lblTXEQminus12db);
            this.grpTXEQ.Controls.Add(this.labelTS9);
            this.grpTXEQ.Location = new System.Drawing.Point(8, 328);
            this.grpTXEQ.Name = "grpTXEQ";
            this.grpTXEQ.Size = new System.Drawing.Size(852, 349);
            this.grpTXEQ.TabIndex = 1;
            this.grpTXEQ.TabStop = false;
            this.grpTXEQ.Text = "Stage 3: Transmit Equalizer";
            // 
            // grpPEQ
            // 
            this.grpPEQ.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.grpPEQ.Controls.Add(this.labelTS10);
            this.grpPEQ.Controls.Add(this.tbTXEQ9Preamp);
            this.grpPEQ.Controls.Add(this.udPEQfreq9);
            this.grpPEQ.Controls.Add(this.udPEQfreq8);
            this.grpPEQ.Controls.Add(this.udPEQfreq7);
            this.grpPEQ.Controls.Add(this.udPEQfreq6);
            this.grpPEQ.Controls.Add(this.udPEQfreq5);
            this.grpPEQ.Controls.Add(this.udPEQfreq4);
            this.grpPEQ.Controls.Add(this.udPEQfreq3);
            this.grpPEQ.Controls.Add(this.udPEQfreq2);
            this.grpPEQ.Controls.Add(this.udPEQfreq1);
            this.grpPEQ.Controls.Add(this.udPEQoctave9);
            this.grpPEQ.Controls.Add(this.tbPEQ9);
            this.grpPEQ.Controls.Add(this.labelTS16);
            this.grpPEQ.Controls.Add(this.labelTS15);
            this.grpPEQ.Controls.Add(this.udPEQoctave8);
            this.grpPEQ.Controls.Add(this.tbPEQ8);
            this.grpPEQ.Controls.Add(this.udPEQoctave7);
            this.grpPEQ.Controls.Add(this.tbPEQ7);
            this.grpPEQ.Controls.Add(this.udPEQoctave6);
            this.grpPEQ.Controls.Add(this.tbPEQ6);
            this.grpPEQ.Controls.Add(this.udPEQoctave5);
            this.grpPEQ.Controls.Add(this.tbPEQ5);
            this.grpPEQ.Controls.Add(this.udPEQoctave4);
            this.grpPEQ.Controls.Add(this.tbPEQ4);
            this.grpPEQ.Controls.Add(this.udPEQoctave3);
            this.grpPEQ.Controls.Add(this.tbPEQ3);
            this.grpPEQ.Controls.Add(this.udPEQoctave2);
            this.grpPEQ.Controls.Add(this.tbPEQ2);
            this.grpPEQ.Controls.Add(this.udPEQoctave1);
            this.grpPEQ.Controls.Add(this.tbPEQ1);
            this.grpPEQ.Controls.Add(this.labelTS11);
            this.grpPEQ.Controls.Add(this.labelTS12);
            this.grpPEQ.Controls.Add(this.labelTS13);
            this.grpPEQ.Controls.Add(this.labelTS14);
            this.grpPEQ.Controls.Add(this.btnTXPEQReset);
            this.grpPEQ.Controls.Add(this.labelTS43);
            this.grpPEQ.Controls.Add(this.labelTS44);
            this.grpPEQ.Location = new System.Drawing.Point(0, 135);
            this.grpPEQ.Name = "grpPEQ";
            this.grpPEQ.Size = new System.Drawing.Size(852, 213);
            this.grpPEQ.TabIndex = 247;
            this.grpPEQ.TabStop = false;
            this.grpPEQ.Text = "Parametric Eq.";
            this.grpPEQ.Visible = false;
            // 
            // labelTS10
            // 
            this.labelTS10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTS10.Image = null;
            this.labelTS10.Location = new System.Drawing.Point(4, 23);
            this.labelTS10.Name = "labelTS10";
            this.labelTS10.Size = new System.Drawing.Size(48, 16);
            this.labelTS10.TabIndex = 290;
            this.labelTS10.Text = "Preamp";
            this.labelTS10.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // tbTXEQ9Preamp
            // 
            this.tbTXEQ9Preamp.AutoSize = false;
            this.tbTXEQ9Preamp.LargeChange = 1;
            this.tbTXEQ9Preamp.Location = new System.Drawing.Point(14, 46);
            this.tbTXEQ9Preamp.Maximum = 15;
            this.tbTXEQ9Preamp.Minimum = -15;
            this.tbTXEQ9Preamp.Name = "tbTXEQ9Preamp";
            this.tbTXEQ9Preamp.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTXEQ9Preamp.Size = new System.Drawing.Size(32, 128);
            this.tbTXEQ9Preamp.TabIndex = 289;
            this.tbTXEQ9Preamp.TickFrequency = 3;
            this.tbTXEQ9Preamp.Scroll += new System.EventHandler(this.tbPEQ1_Scroll);
            // 
            // udPEQfreq9
            // 
            this.udPEQfreq9.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.udPEQfreq9.Cursor = System.Windows.Forms.Cursors.Hand;
            this.udPEQfreq9.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.udPEQfreq9.Location = new System.Drawing.Point(732, 30);
            this.udPEQfreq9.Maximum = new decimal(new int[] {
            16000,
            0,
            0,
            0});
            this.udPEQfreq9.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.udPEQfreq9.Name = "udPEQfreq9";
            this.udPEQfreq9.Size = new System.Drawing.Size(52, 16);
            this.udPEQfreq9.TabIndex = 288;
            this.udPEQfreq9.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolTip1.SetToolTip(this.udPEQfreq9, "Center Frequency (hz) of Parametric Band.\r\nWidth is effected by the octave (bw) v" +
        "alue (down below)");
            this.udPEQfreq9.Value = new decimal(new int[] {
            8000,
            0,
            0,
            0});
            this.udPEQfreq9.ValueChanged += new System.EventHandler(this.tbPEQ1_Scroll);
            // 
            // udPEQfreq8
            // 
            this.udPEQfreq8.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.udPEQfreq8.Cursor = System.Windows.Forms.Cursors.Hand;
            this.udPEQfreq8.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.udPEQfreq8.Location = new System.Drawing.Point(651, 30);
            this.udPEQfreq8.Maximum = new decimal(new int[] {
            16000,
            0,
            0,
            0});
            this.udPEQfreq8.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.udPEQfreq8.Name = "udPEQfreq8";
            this.udPEQfreq8.Size = new System.Drawing.Size(52, 16);
            this.udPEQfreq8.TabIndex = 287;
            this.udPEQfreq8.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolTip1.SetToolTip(this.udPEQfreq8, "Center Frequency (hz) of Parametric Band.\r\nWidth is effected by the octave (bw) v" +
        "alue (down below)");
            this.udPEQfreq8.Value = new decimal(new int[] {
            4000,
            0,
            0,
            0});
            this.udPEQfreq8.ValueChanged += new System.EventHandler(this.tbPEQ1_Scroll);
            // 
            // udPEQfreq7
            // 
            this.udPEQfreq7.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.udPEQfreq7.Cursor = System.Windows.Forms.Cursors.Hand;
            this.udPEQfreq7.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.udPEQfreq7.Location = new System.Drawing.Point(570, 30);
            this.udPEQfreq7.Maximum = new decimal(new int[] {
            16000,
            0,
            0,
            0});
            this.udPEQfreq7.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.udPEQfreq7.Name = "udPEQfreq7";
            this.udPEQfreq7.Size = new System.Drawing.Size(52, 16);
            this.udPEQfreq7.TabIndex = 286;
            this.udPEQfreq7.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolTip1.SetToolTip(this.udPEQfreq7, "Center Frequency (hz) of Parametric Band.\r\nWidth is effected by the octave (bw) v" +
        "alue (down below)");
            this.udPEQfreq7.Value = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.udPEQfreq7.ValueChanged += new System.EventHandler(this.tbPEQ1_Scroll);
            // 
            // udPEQfreq6
            // 
            this.udPEQfreq6.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.udPEQfreq6.Cursor = System.Windows.Forms.Cursors.Hand;
            this.udPEQfreq6.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.udPEQfreq6.Location = new System.Drawing.Point(490, 30);
            this.udPEQfreq6.Maximum = new decimal(new int[] {
            16000,
            0,
            0,
            0});
            this.udPEQfreq6.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.udPEQfreq6.Name = "udPEQfreq6";
            this.udPEQfreq6.Size = new System.Drawing.Size(52, 16);
            this.udPEQfreq6.TabIndex = 285;
            this.udPEQfreq6.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolTip1.SetToolTip(this.udPEQfreq6, "Center Frequency (hz) of Parametric Band.\r\nWidth is effected by the octave (bw) v" +
        "alue (down below)");
            this.udPEQfreq6.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.udPEQfreq6.ValueChanged += new System.EventHandler(this.tbPEQ1_Scroll);
            // 
            // udPEQfreq5
            // 
            this.udPEQfreq5.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.udPEQfreq5.Cursor = System.Windows.Forms.Cursors.Hand;
            this.udPEQfreq5.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.udPEQfreq5.Location = new System.Drawing.Point(411, 30);
            this.udPEQfreq5.Maximum = new decimal(new int[] {
            16000,
            0,
            0,
            0});
            this.udPEQfreq5.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.udPEQfreq5.Name = "udPEQfreq5";
            this.udPEQfreq5.Size = new System.Drawing.Size(52, 16);
            this.udPEQfreq5.TabIndex = 284;
            this.udPEQfreq5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolTip1.SetToolTip(this.udPEQfreq5, "Center Frequency (hz) of Parametric Band.\r\nWidth is effected by the octave (bw) v" +
        "alue (down below)");
            this.udPEQfreq5.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.udPEQfreq5.ValueChanged += new System.EventHandler(this.tbPEQ1_Scroll);
            // 
            // udPEQfreq4
            // 
            this.udPEQfreq4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.udPEQfreq4.Cursor = System.Windows.Forms.Cursors.Hand;
            this.udPEQfreq4.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.udPEQfreq4.Location = new System.Drawing.Point(336, 30);
            this.udPEQfreq4.Maximum = new decimal(new int[] {
            16000,
            0,
            0,
            0});
            this.udPEQfreq4.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.udPEQfreq4.Name = "udPEQfreq4";
            this.udPEQfreq4.Size = new System.Drawing.Size(52, 16);
            this.udPEQfreq4.TabIndex = 283;
            this.udPEQfreq4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolTip1.SetToolTip(this.udPEQfreq4, "Center Frequency (hz) of Parametric Band.\r\nWidth is effected by the octave (bw) v" +
        "alue (down below)");
            this.udPEQfreq4.Value = new decimal(new int[] {
            250,
            0,
            0,
            0});
            this.udPEQfreq4.ValueChanged += new System.EventHandler(this.tbPEQ1_Scroll);
            // 
            // udPEQfreq3
            // 
            this.udPEQfreq3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.udPEQfreq3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.udPEQfreq3.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.udPEQfreq3.Location = new System.Drawing.Point(260, 30);
            this.udPEQfreq3.Maximum = new decimal(new int[] {
            16000,
            0,
            0,
            0});
            this.udPEQfreq3.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.udPEQfreq3.Name = "udPEQfreq3";
            this.udPEQfreq3.Size = new System.Drawing.Size(52, 16);
            this.udPEQfreq3.TabIndex = 282;
            this.udPEQfreq3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolTip1.SetToolTip(this.udPEQfreq3, "Center Frequency (hz) of Parametric Band.\r\nWidth is effected by the octave (bw) v" +
        "alue (down below)");
            this.udPEQfreq3.Value = new decimal(new int[] {
            125,
            0,
            0,
            0});
            this.udPEQfreq3.ValueChanged += new System.EventHandler(this.tbPEQ1_Scroll);
            // 
            // udPEQfreq2
            // 
            this.udPEQfreq2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.udPEQfreq2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.udPEQfreq2.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.udPEQfreq2.Location = new System.Drawing.Point(186, 31);
            this.udPEQfreq2.Maximum = new decimal(new int[] {
            16000,
            0,
            0,
            0});
            this.udPEQfreq2.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.udPEQfreq2.Name = "udPEQfreq2";
            this.udPEQfreq2.Size = new System.Drawing.Size(52, 16);
            this.udPEQfreq2.TabIndex = 281;
            this.udPEQfreq2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolTip1.SetToolTip(this.udPEQfreq2, "Center Frequency (hz) of Parametric Band.\r\nWidth is effected by the octave (bw) v" +
        "alue (down below)");
            this.udPEQfreq2.Value = new decimal(new int[] {
            63,
            0,
            0,
            0});
            this.udPEQfreq2.ValueChanged += new System.EventHandler(this.tbPEQ1_Scroll);
            // 
            // udPEQfreq1
            // 
            this.udPEQfreq1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.udPEQfreq1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.udPEQfreq1.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.udPEQfreq1.Location = new System.Drawing.Point(114, 31);
            this.udPEQfreq1.Maximum = new decimal(new int[] {
            16000,
            0,
            0,
            0});
            this.udPEQfreq1.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.udPEQfreq1.Name = "udPEQfreq1";
            this.udPEQfreq1.Size = new System.Drawing.Size(52, 16);
            this.udPEQfreq1.TabIndex = 280;
            this.udPEQfreq1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolTip1.SetToolTip(this.udPEQfreq1, "Center Frequency (hz) of Parametric Band.\r\nWidth is effected by the octave (bw) v" +
        "alue (down below)");
            this.udPEQfreq1.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.udPEQfreq1.ValueChanged += new System.EventHandler(this.tbPEQ1_Scroll);
            // 
            // udPEQoctave9
            // 
            this.udPEQoctave9.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.udPEQoctave9.DecimalPlaces = 1;
            this.udPEQoctave9.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.udPEQoctave9.Location = new System.Drawing.Point(745, 169);
            this.udPEQoctave9.Maximum = new decimal(new int[] {
            40,
            0,
            0,
            65536});
            this.udPEQoctave9.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.udPEQoctave9.Name = "udPEQoctave9";
            this.udPEQoctave9.Size = new System.Drawing.Size(42, 16);
            this.udPEQoctave9.TabIndex = 276;
            this.toolTip1.SetToolTip(this.udPEQoctave9, "octave (bw) Width of Parametric Band. \r\n1/5 octave = 0.2\r\n1/3 octave = 0.3\r\n1/2 O" +
        "ctive = 0.5\r\n3/4 octave = 0.7\r\n1.0 octave = 1.0\r\n2.0 octave = 2.0\r\n");
            this.udPEQoctave9.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udPEQoctave9.ValueChanged += new System.EventHandler(this.tbPEQ1_Scroll);
            // 
            // tbPEQ9
            // 
            this.tbPEQ9.AutoSize = false;
            this.tbPEQ9.LargeChange = 1;
            this.tbPEQ9.Location = new System.Drawing.Point(749, 45);
            this.tbPEQ9.Maximum = 15;
            this.tbPEQ9.Minimum = -15;
            this.tbPEQ9.Name = "tbPEQ9";
            this.tbPEQ9.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbPEQ9.Size = new System.Drawing.Size(32, 128);
            this.tbPEQ9.TabIndex = 274;
            this.tbPEQ9.TickFrequency = 3;
            this.tbPEQ9.Scroll += new System.EventHandler(this.tbPEQ1_Scroll);
            this.tbPEQ9.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tbPEQ9_MouseDown);
            this.tbPEQ9.MouseHover += new System.EventHandler(this.tbPEQ9_MouseHover);
            // 
            // labelTS16
            // 
            this.labelTS16.Image = null;
            this.labelTS16.Location = new System.Drawing.Point(285, 11);
            this.labelTS16.Name = "labelTS16";
            this.labelTS16.Size = new System.Drawing.Size(474, 16);
            this.labelTS16.TabIndex = 252;
            this.labelTS16.Text = "Center Frequency (hz) of each Parametric Band Slider.";
            this.labelTS16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelTS15
            // 
            this.labelTS15.Image = null;
            this.labelTS15.Location = new System.Drawing.Point(195, 192);
            this.labelTS15.Name = "labelTS15";
            this.labelTS15.Size = new System.Drawing.Size(462, 16);
            this.labelTS15.TabIndex = 251;
            this.labelTS15.Text = "Octave (bandwidth) of each Parametric Band Slider. (Larger value = wider band wid" +
    "th)\r\n";
            this.labelTS15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip1.SetToolTip(this.labelTS15, "octave (bw) Width of Parametric Band. \r\nCentered on the Frequency (hz) above each" +
        " slider\r\n1/5 octave = 0.2\r\n1/3 octave = 0.3\r\n1/2 octave = 0.5\r\n3/4 octave = 0.7\r" +
        "\n1.0 octave = 1.0\r\n2.0 octave = 2.0\r\n");
            // 
            // udPEQoctave8
            // 
            this.udPEQoctave8.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.udPEQoctave8.DecimalPlaces = 1;
            this.udPEQoctave8.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.udPEQoctave8.Location = new System.Drawing.Point(659, 169);
            this.udPEQoctave8.Maximum = new decimal(new int[] {
            40,
            0,
            0,
            65536});
            this.udPEQoctave8.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.udPEQoctave8.Name = "udPEQoctave8";
            this.udPEQoctave8.Size = new System.Drawing.Size(42, 16);
            this.udPEQoctave8.TabIndex = 273;
            this.toolTip1.SetToolTip(this.udPEQoctave8, "octave (bw) Width of Parametric Band. \r\n1/5 octave = 0.2\r\n1/3 octave = 0.3\r\n1/2 O" +
        "ctive = 0.5\r\n3/4 octave = 0.7\r\n1.0 octave = 1.0\r\n2.0 octave = 2.0\r\n");
            this.udPEQoctave8.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udPEQoctave8.ValueChanged += new System.EventHandler(this.tbPEQ1_Scroll);
            // 
            // tbPEQ8
            // 
            this.tbPEQ8.AutoSize = false;
            this.tbPEQ8.LargeChange = 1;
            this.tbPEQ8.Location = new System.Drawing.Point(669, 45);
            this.tbPEQ8.Maximum = 15;
            this.tbPEQ8.Minimum = -15;
            this.tbPEQ8.Name = "tbPEQ8";
            this.tbPEQ8.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbPEQ8.Size = new System.Drawing.Size(32, 128);
            this.tbPEQ8.TabIndex = 271;
            this.tbPEQ8.TickFrequency = 3;
            this.tbPEQ8.Scroll += new System.EventHandler(this.tbPEQ1_Scroll);
            this.tbPEQ8.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tbPEQ8_MouseDown);
            this.tbPEQ8.MouseHover += new System.EventHandler(this.tbPEQ8_MouseHover);
            // 
            // udPEQoctave7
            // 
            this.udPEQoctave7.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.udPEQoctave7.DecimalPlaces = 1;
            this.udPEQoctave7.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.udPEQoctave7.Location = new System.Drawing.Point(580, 169);
            this.udPEQoctave7.Maximum = new decimal(new int[] {
            40,
            0,
            0,
            65536});
            this.udPEQoctave7.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.udPEQoctave7.Name = "udPEQoctave7";
            this.udPEQoctave7.Size = new System.Drawing.Size(42, 16);
            this.udPEQoctave7.TabIndex = 270;
            this.toolTip1.SetToolTip(this.udPEQoctave7, "octave (bw) Width of Parametric Band. \r\n1/5 octave = 0.2\r\n1/3 octave = 0.3\r\n1/2 O" +
        "ctive = 0.5\r\n3/4 octave = 0.7\r\n1.0 octave = 1.0\r\n2.0 octave = 2.0\r\n");
            this.udPEQoctave7.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udPEQoctave7.ValueChanged += new System.EventHandler(this.tbPEQ1_Scroll);
            // 
            // tbPEQ7
            // 
            this.tbPEQ7.AutoSize = false;
            this.tbPEQ7.LargeChange = 1;
            this.tbPEQ7.Location = new System.Drawing.Point(590, 45);
            this.tbPEQ7.Maximum = 15;
            this.tbPEQ7.Minimum = -15;
            this.tbPEQ7.Name = "tbPEQ7";
            this.tbPEQ7.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbPEQ7.Size = new System.Drawing.Size(32, 128);
            this.tbPEQ7.TabIndex = 268;
            this.tbPEQ7.TickFrequency = 3;
            this.tbPEQ7.Scroll += new System.EventHandler(this.tbPEQ1_Scroll);
            this.tbPEQ7.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tbPEQ7_MouseDown);
            this.tbPEQ7.MouseHover += new System.EventHandler(this.tbPEQ7_MouseHover);
            // 
            // udPEQoctave6
            // 
            this.udPEQoctave6.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.udPEQoctave6.DecimalPlaces = 1;
            this.udPEQoctave6.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.udPEQoctave6.Location = new System.Drawing.Point(500, 169);
            this.udPEQoctave6.Maximum = new decimal(new int[] {
            40,
            0,
            0,
            65536});
            this.udPEQoctave6.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.udPEQoctave6.Name = "udPEQoctave6";
            this.udPEQoctave6.Size = new System.Drawing.Size(42, 16);
            this.udPEQoctave6.TabIndex = 267;
            this.toolTip1.SetToolTip(this.udPEQoctave6, "octave (bw) Width of Parametric Band. \r\n1/5 octave = 0.2\r\n1/3 octave = 0.3\r\n1/2 O" +
        "ctive = 0.5\r\n3/4 octave = 0.7\r\n1.0 octave = 1.0\r\n2.0 octave = 2.0\r\n");
            this.udPEQoctave6.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udPEQoctave6.ValueChanged += new System.EventHandler(this.tbPEQ1_Scroll);
            // 
            // tbPEQ6
            // 
            this.tbPEQ6.AutoSize = false;
            this.tbPEQ6.LargeChange = 1;
            this.tbPEQ6.Location = new System.Drawing.Point(505, 45);
            this.tbPEQ6.Maximum = 15;
            this.tbPEQ6.Minimum = -15;
            this.tbPEQ6.Name = "tbPEQ6";
            this.tbPEQ6.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbPEQ6.Size = new System.Drawing.Size(32, 128);
            this.tbPEQ6.TabIndex = 265;
            this.tbPEQ6.TickFrequency = 3;
            this.tbPEQ6.Scroll += new System.EventHandler(this.tbPEQ1_Scroll);
            this.tbPEQ6.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tbPEQ6_MouseDown);
            this.tbPEQ6.MouseHover += new System.EventHandler(this.tbPEQ6_MouseHover);
            // 
            // udPEQoctave5
            // 
            this.udPEQoctave5.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.udPEQoctave5.DecimalPlaces = 1;
            this.udPEQoctave5.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.udPEQoctave5.Location = new System.Drawing.Point(415, 169);
            this.udPEQoctave5.Maximum = new decimal(new int[] {
            40,
            0,
            0,
            65536});
            this.udPEQoctave5.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.udPEQoctave5.Name = "udPEQoctave5";
            this.udPEQoctave5.Size = new System.Drawing.Size(42, 16);
            this.udPEQoctave5.TabIndex = 264;
            this.toolTip1.SetToolTip(this.udPEQoctave5, "octave (bw) Width of Parametric Band. \r\n1/5 octave = 0.2\r\n1/3 octave = 0.3\r\n1/2 O" +
        "ctive = 0.5\r\n3/4 octave = 0.7\r\n1.0 octave = 1.0\r\n2.0 octave = 2.0\r\n");
            this.udPEQoctave5.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udPEQoctave5.ValueChanged += new System.EventHandler(this.tbPEQ1_Scroll);
            // 
            // tbPEQ5
            // 
            this.tbPEQ5.AutoSize = false;
            this.tbPEQ5.LargeChange = 1;
            this.tbPEQ5.Location = new System.Drawing.Point(425, 45);
            this.tbPEQ5.Maximum = 15;
            this.tbPEQ5.Minimum = -15;
            this.tbPEQ5.Name = "tbPEQ5";
            this.tbPEQ5.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbPEQ5.Size = new System.Drawing.Size(32, 128);
            this.tbPEQ5.TabIndex = 262;
            this.tbPEQ5.TickFrequency = 3;
            this.tbPEQ5.Scroll += new System.EventHandler(this.tbPEQ1_Scroll);
            this.tbPEQ5.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tbPEQ5_MouseDown);
            this.tbPEQ5.MouseHover += new System.EventHandler(this.tbPEQ5_MouseHover);
            // 
            // udPEQoctave4
            // 
            this.udPEQoctave4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.udPEQoctave4.DecimalPlaces = 1;
            this.udPEQoctave4.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.udPEQoctave4.Location = new System.Drawing.Point(343, 169);
            this.udPEQoctave4.Maximum = new decimal(new int[] {
            40,
            0,
            0,
            65536});
            this.udPEQoctave4.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.udPEQoctave4.Name = "udPEQoctave4";
            this.udPEQoctave4.Size = new System.Drawing.Size(42, 16);
            this.udPEQoctave4.TabIndex = 261;
            this.toolTip1.SetToolTip(this.udPEQoctave4, "octave (bw) Width of Parametric Band. \r\n1/5 octave = 0.2\r\n1/3 octave = 0.3\r\n1/2 O" +
        "ctive = 0.5\r\n3/4 octave = 0.7\r\n1.0 octave = 1.0\r\n2.0 octave = 2.0\r\n");
            this.udPEQoctave4.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udPEQoctave4.ValueChanged += new System.EventHandler(this.tbPEQ1_Scroll);
            // 
            // tbPEQ4
            // 
            this.tbPEQ4.AutoSize = false;
            this.tbPEQ4.LargeChange = 1;
            this.tbPEQ4.Location = new System.Drawing.Point(347, 45);
            this.tbPEQ4.Maximum = 15;
            this.tbPEQ4.Minimum = -15;
            this.tbPEQ4.Name = "tbPEQ4";
            this.tbPEQ4.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbPEQ4.Size = new System.Drawing.Size(32, 128);
            this.tbPEQ4.TabIndex = 259;
            this.tbPEQ4.TickFrequency = 3;
            this.tbPEQ4.Scroll += new System.EventHandler(this.tbPEQ1_Scroll);
            this.tbPEQ4.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tbPEQ4_MouseDown);
            this.tbPEQ4.MouseHover += new System.EventHandler(this.tbPEQ4_MouseHover);
            // 
            // udPEQoctave3
            // 
            this.udPEQoctave3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.udPEQoctave3.DecimalPlaces = 1;
            this.udPEQoctave3.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.udPEQoctave3.Location = new System.Drawing.Point(266, 169);
            this.udPEQoctave3.Maximum = new decimal(new int[] {
            40,
            0,
            0,
            65536});
            this.udPEQoctave3.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.udPEQoctave3.Name = "udPEQoctave3";
            this.udPEQoctave3.Size = new System.Drawing.Size(42, 16);
            this.udPEQoctave3.TabIndex = 258;
            this.toolTip1.SetToolTip(this.udPEQoctave3, "octave (bw) Width of Parametric Band. \r\n1/5 octave = 0.2\r\n1/3 octave = 0.3\r\n1/2 O" +
        "ctive = 0.5\r\n3/4 octave = 0.7\r\n1.0 octave = 1.0\r\n2.0 octave = 2.0\r\n");
            this.udPEQoctave3.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udPEQoctave3.ValueChanged += new System.EventHandler(this.tbPEQ1_Scroll);
            // 
            // tbPEQ3
            // 
            this.tbPEQ3.AutoSize = false;
            this.tbPEQ3.LargeChange = 1;
            this.tbPEQ3.Location = new System.Drawing.Point(272, 45);
            this.tbPEQ3.Maximum = 15;
            this.tbPEQ3.Minimum = -15;
            this.tbPEQ3.Name = "tbPEQ3";
            this.tbPEQ3.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbPEQ3.Size = new System.Drawing.Size(32, 128);
            this.tbPEQ3.TabIndex = 256;
            this.tbPEQ3.TickFrequency = 3;
            this.tbPEQ3.Scroll += new System.EventHandler(this.tbPEQ1_Scroll);
            this.tbPEQ3.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tbPEQ3_MouseDown);
            this.tbPEQ3.MouseHover += new System.EventHandler(this.tbPEQ3_MouseHover);
            // 
            // udPEQoctave2
            // 
            this.udPEQoctave2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.udPEQoctave2.DecimalPlaces = 1;
            this.udPEQoctave2.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.udPEQoctave2.Location = new System.Drawing.Point(193, 170);
            this.udPEQoctave2.Maximum = new decimal(new int[] {
            40,
            0,
            0,
            65536});
            this.udPEQoctave2.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.udPEQoctave2.Name = "udPEQoctave2";
            this.udPEQoctave2.Size = new System.Drawing.Size(42, 16);
            this.udPEQoctave2.TabIndex = 255;
            this.toolTip1.SetToolTip(this.udPEQoctave2, "octave (bw) Width of Parametric Band. \r\n1/5 octave = 0.2\r\n1/3 octave = 0.3\r\n1/2 O" +
        "ctive = 0.5\r\n3/4 octave = 0.7\r\n1.0 octave = 1.0\r\n2.0 octave = 2.0\r\n");
            this.udPEQoctave2.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udPEQoctave2.ValueChanged += new System.EventHandler(this.tbPEQ1_Scroll);
            // 
            // tbPEQ2
            // 
            this.tbPEQ2.AutoSize = false;
            this.tbPEQ2.LargeChange = 1;
            this.tbPEQ2.Location = new System.Drawing.Point(199, 46);
            this.tbPEQ2.Maximum = 15;
            this.tbPEQ2.Minimum = -15;
            this.tbPEQ2.Name = "tbPEQ2";
            this.tbPEQ2.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbPEQ2.Size = new System.Drawing.Size(32, 128);
            this.tbPEQ2.TabIndex = 253;
            this.tbPEQ2.TickFrequency = 3;
            this.tbPEQ2.Scroll += new System.EventHandler(this.tbPEQ1_Scroll);
            this.tbPEQ2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tbPEQ2_MouseDown);
            this.tbPEQ2.MouseHover += new System.EventHandler(this.tbPEQ2_MouseHover);
            // 
            // udPEQoctave1
            // 
            this.udPEQoctave1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.udPEQoctave1.DecimalPlaces = 1;
            this.udPEQoctave1.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.udPEQoctave1.Location = new System.Drawing.Point(116, 170);
            this.udPEQoctave1.Maximum = new decimal(new int[] {
            40,
            0,
            0,
            65536});
            this.udPEQoctave1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.udPEQoctave1.Name = "udPEQoctave1";
            this.udPEQoctave1.Size = new System.Drawing.Size(42, 16);
            this.udPEQoctave1.TabIndex = 250;
            this.toolTip1.SetToolTip(this.udPEQoctave1, "octave (bw) Width of Parametric Band. \r\n1/5 octave = 0.2\r\n1/3 octave = 0.3\r\n1/2 O" +
        "ctive = 0.5\r\n3/4 octave = 0.7\r\n1.0 octave = 1.0\r\n2.0 octave = 2.0\r\n");
            this.udPEQoctave1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udPEQoctave1.ValueChanged += new System.EventHandler(this.tbPEQ1_Scroll);
            // 
            // tbPEQ1
            // 
            this.tbPEQ1.AutoSize = false;
            this.tbPEQ1.LargeChange = 1;
            this.tbPEQ1.Location = new System.Drawing.Point(126, 46);
            this.tbPEQ1.Maximum = 15;
            this.tbPEQ1.Minimum = -15;
            this.tbPEQ1.Name = "tbPEQ1";
            this.tbPEQ1.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbPEQ1.Size = new System.Drawing.Size(32, 128);
            this.tbPEQ1.TabIndex = 248;
            this.tbPEQ1.TickFrequency = 3;
            this.tbPEQ1.Scroll += new System.EventHandler(this.tbPEQ1_Scroll);
            this.tbPEQ1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tbPEQ1_MouseDown);
            this.tbPEQ1.MouseHover += new System.EventHandler(this.tbPEQ1_MouseHover);
            // 
            // labelTS11
            // 
            this.labelTS11.Image = null;
            this.labelTS11.Location = new System.Drawing.Point(60, 46);
            this.labelTS11.Margin = new System.Windows.Forms.Padding(0);
            this.labelTS11.Name = "labelTS11";
            this.labelTS11.Size = new System.Drawing.Size(32, 16);
            this.labelTS11.TabIndex = 244;
            this.labelTS11.Text = "15dB";
            this.labelTS11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelTS12
            // 
            this.labelTS12.Image = null;
            this.labelTS12.Location = new System.Drawing.Point(59, 102);
            this.labelTS12.Margin = new System.Windows.Forms.Padding(0);
            this.labelTS12.Name = "labelTS12";
            this.labelTS12.Size = new System.Drawing.Size(32, 16);
            this.labelTS12.TabIndex = 243;
            this.labelTS12.Text = "  0dB";
            this.labelTS12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelTS13
            // 
            this.labelTS13.Image = null;
            this.labelTS13.Location = new System.Drawing.Point(810, 102);
            this.labelTS13.Name = "labelTS13";
            this.labelTS13.Size = new System.Drawing.Size(32, 16);
            this.labelTS13.TabIndex = 131;
            this.labelTS13.Text = "  0dB";
            this.labelTS13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelTS14
            // 
            this.labelTS14.Image = null;
            this.labelTS14.Location = new System.Drawing.Point(810, 46);
            this.labelTS14.Name = "labelTS14";
            this.labelTS14.Size = new System.Drawing.Size(32, 16);
            this.labelTS14.TabIndex = 131;
            this.labelTS14.Text = "15dB";
            this.labelTS14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnTXPEQReset
            // 
            this.btnTXPEQReset.Image = null;
            this.btnTXPEQReset.Location = new System.Drawing.Point(790, 11);
            this.btnTXPEQReset.Name = "btnTXPEQReset";
            this.btnTXPEQReset.Size = new System.Drawing.Size(56, 20);
            this.btnTXPEQReset.TabIndex = 131;
            this.btnTXPEQReset.Text = "Reset";
            this.btnTXPEQReset.Click += new System.EventHandler(this.btnTXPEQReset_Click);
            // 
            // labelTS43
            // 
            this.labelTS43.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTS43.Image = null;
            this.labelTS43.Location = new System.Drawing.Point(60, 158);
            this.labelTS43.Name = "labelTS43";
            this.labelTS43.Size = new System.Drawing.Size(42, 16);
            this.labelTS43.TabIndex = 245;
            this.labelTS43.Text = "-15dB";
            this.labelTS43.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelTS44
            // 
            this.labelTS44.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTS44.Image = null;
            this.labelTS44.Location = new System.Drawing.Point(810, 158);
            this.labelTS44.Name = "labelTS44";
            this.labelTS44.Size = new System.Drawing.Size(34, 16);
            this.labelTS44.TabIndex = 131;
            this.labelTS44.Text = "-15dB";
            this.labelTS44.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // grpTXEQ28
            // 
            this.grpTXEQ28.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.grpTXEQ28.Controls.Add(this.labelTS17);
            this.grpTXEQ28.Controls.Add(this.tbTXEQ28Preamp);
            this.grpTXEQ28.Controls.Add(this.labelTS7);
            this.grpTXEQ28.Controls.Add(this.labelTS5);
            this.grpTXEQ28.Controls.Add(this.labelTS4);
            this.grpTXEQ28.Controls.Add(this.labelTS2);
            this.grpTXEQ28.Controls.Add(this.labelTS1);
            this.grpTXEQ28.Controls.Add(this.lbl2815);
            this.grpTXEQ28.Controls.Add(this.lbl2814);
            this.grpTXEQ28.Controls.Add(this.btnTXEQReset28);
            this.grpTXEQ28.Controls.Add(this.lbl2825);
            this.grpTXEQ28.Controls.Add(this.lbl2821);
            this.grpTXEQ28.Controls.Add(this.lbl2828);
            this.grpTXEQ28.Controls.Add(this.lbl2827);
            this.grpTXEQ28.Controls.Add(this.tbTX28EQ28);
            this.grpTXEQ28.Controls.Add(this.tbTX28EQ27);
            this.grpTXEQ28.Controls.Add(this.lbl2826);
            this.grpTXEQ28.Controls.Add(this.tbTX28EQ26);
            this.grpTXEQ28.Controls.Add(this.tbTX28EQ25);
            this.grpTXEQ28.Controls.Add(this.lbl2824);
            this.grpTXEQ28.Controls.Add(this.lbl2823);
            this.grpTXEQ28.Controls.Add(this.tbTX28EQ24);
            this.grpTXEQ28.Controls.Add(this.tbTX28EQ23);
            this.grpTXEQ28.Controls.Add(this.lbl2822);
            this.grpTXEQ28.Controls.Add(this.tbTX28EQ22);
            this.grpTXEQ28.Controls.Add(this.lbl2820);
            this.grpTXEQ28.Controls.Add(this.tbTX28EQ21);
            this.grpTXEQ28.Controls.Add(this.tbTX28EQ20);
            this.grpTXEQ28.Controls.Add(this.lbl2819);
            this.grpTXEQ28.Controls.Add(this.tbTX28EQ19);
            this.grpTXEQ28.Controls.Add(this.tbTX28EQ18);
            this.grpTXEQ28.Controls.Add(this.lbl2818);
            this.grpTXEQ28.Controls.Add(this.lbl2817);
            this.grpTXEQ28.Controls.Add(this.lbl2816);
            this.grpTXEQ28.Controls.Add(this.tbTX28EQ17);
            this.grpTXEQ28.Controls.Add(this.tbTX28EQ16);
            this.grpTXEQ28.Controls.Add(this.tbTX28EQ15);
            this.grpTXEQ28.Controls.Add(this.lbl287);
            this.grpTXEQ28.Controls.Add(this.lbl2813);
            this.grpTXEQ28.Controls.Add(this.tbTX28EQ14);
            this.grpTXEQ28.Controls.Add(this.tbTX28EQ13);
            this.grpTXEQ28.Controls.Add(this.lbl2812);
            this.grpTXEQ28.Controls.Add(this.tbTX28EQ12);
            this.grpTXEQ28.Controls.Add(this.tbTX28EQ11);
            this.grpTXEQ28.Controls.Add(this.lbl2811);
            this.grpTXEQ28.Controls.Add(this.lbl2810);
            this.grpTXEQ28.Controls.Add(this.lbl289);
            this.grpTXEQ28.Controls.Add(this.tbTX28EQ10);
            this.grpTXEQ28.Controls.Add(this.tbTX28EQ9);
            this.grpTXEQ28.Controls.Add(this.lbl288);
            this.grpTXEQ28.Controls.Add(this.tbTX28EQ8);
            this.grpTXEQ28.Controls.Add(this.lbl286);
            this.grpTXEQ28.Controls.Add(this.tbTX28EQ7);
            this.grpTXEQ28.Controls.Add(this.tbTX28EQ6);
            this.grpTXEQ28.Controls.Add(this.lbl285);
            this.grpTXEQ28.Controls.Add(this.tbTX28EQ5);
            this.grpTXEQ28.Controls.Add(this.tbTX28EQ4);
            this.grpTXEQ28.Controls.Add(this.lbl284);
            this.grpTXEQ28.Controls.Add(this.lbl283);
            this.grpTXEQ28.Controls.Add(this.lbl282);
            this.grpTXEQ28.Controls.Add(this.tbTX28EQ3);
            this.grpTXEQ28.Controls.Add(this.tbTX28EQ2);
            this.grpTXEQ28.Controls.Add(this.lbl281);
            this.grpTXEQ28.Controls.Add(this.tbTX28EQ1);
            this.grpTXEQ28.Controls.Add(this.labelTS6);
            this.grpTXEQ28.Controls.Add(this.labelTS3);
            this.grpTXEQ28.Location = new System.Drawing.Point(-1, 137);
            this.grpTXEQ28.Name = "grpTXEQ28";
            this.grpTXEQ28.Size = new System.Drawing.Size(853, 209);
            this.grpTXEQ28.TabIndex = 131;
            this.grpTXEQ28.TabStop = false;
            this.grpTXEQ28.Text = "Stage 3:  28 Band Transmit Equalizer";
            this.grpTXEQ28.Visible = false;
            // 
            // labelTS17
            // 
            this.labelTS17.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTS17.Image = null;
            this.labelTS17.Location = new System.Drawing.Point(4, 43);
            this.labelTS17.Name = "labelTS17";
            this.labelTS17.Size = new System.Drawing.Size(48, 16);
            this.labelTS17.TabIndex = 248;
            this.labelTS17.Text = "Preamp";
            this.labelTS17.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // tbTXEQ28Preamp
            // 
            this.tbTXEQ28Preamp.AutoSize = false;
            this.tbTXEQ28Preamp.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.tbTXEQ28Preamp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tbTXEQ28Preamp.LargeChange = 1;
            this.tbTXEQ28Preamp.Location = new System.Drawing.Point(12, 62);
            this.tbTXEQ28Preamp.Maximum = 15;
            this.tbTXEQ28Preamp.Minimum = -15;
            this.tbTXEQ28Preamp.Name = "tbTXEQ28Preamp";
            this.tbTXEQ28Preamp.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTXEQ28Preamp.Size = new System.Drawing.Size(23, 128);
            this.tbTXEQ28Preamp.TabIndex = 247;
            this.tbTXEQ28Preamp.TickFrequency = 3;
            this.tbTXEQ28Preamp.Scroll += new System.EventHandler(this.tbTX28EQ15_Scroll);
            // 
            // labelTS7
            // 
            this.labelTS7.Image = null;
            this.labelTS7.Location = new System.Drawing.Point(184, 18);
            this.labelTS7.Name = "labelTS7";
            this.labelTS7.Size = new System.Drawing.Size(435, 16);
            this.labelTS7.TabIndex = 246;
            this.labelTS7.Text = "Each Band Slider has 1/3-Octave BW (Example: 112 low - 125 center  - 141 high)";
            this.labelTS7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelTS5
            // 
            this.labelTS5.Image = null;
            this.labelTS5.Location = new System.Drawing.Point(56, 62);
            this.labelTS5.Margin = new System.Windows.Forms.Padding(0);
            this.labelTS5.Name = "labelTS5";
            this.labelTS5.Size = new System.Drawing.Size(32, 16);
            this.labelTS5.TabIndex = 244;
            this.labelTS5.Text = "15dB";
            this.labelTS5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelTS4
            // 
            this.labelTS4.Image = null;
            this.labelTS4.Location = new System.Drawing.Point(55, 118);
            this.labelTS4.Margin = new System.Windows.Forms.Padding(0);
            this.labelTS4.Name = "labelTS4";
            this.labelTS4.Size = new System.Drawing.Size(32, 16);
            this.labelTS4.TabIndex = 243;
            this.labelTS4.Text = "  0dB";
            this.labelTS4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelTS2
            // 
            this.labelTS2.Image = null;
            this.labelTS2.Location = new System.Drawing.Point(806, 118);
            this.labelTS2.Name = "labelTS2";
            this.labelTS2.Size = new System.Drawing.Size(32, 16);
            this.labelTS2.TabIndex = 131;
            this.labelTS2.Text = "  0dB";
            this.labelTS2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelTS1
            // 
            this.labelTS1.Image = null;
            this.labelTS1.Location = new System.Drawing.Point(806, 62);
            this.labelTS1.Name = "labelTS1";
            this.labelTS1.Size = new System.Drawing.Size(32, 16);
            this.labelTS1.TabIndex = 131;
            this.labelTS1.Text = "15dB";
            this.labelTS1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbl2815
            // 
            this.lbl2815.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl2815.Image = null;
            this.lbl2815.Location = new System.Drawing.Point(441, 43);
            this.lbl2815.Name = "lbl2815";
            this.lbl2815.Size = new System.Drawing.Size(32, 16);
            this.lbl2815.TabIndex = 217;
            this.lbl2815.Text = "800";
            this.lbl2815.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl2814
            // 
            this.lbl2814.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl2814.Image = null;
            this.lbl2814.Location = new System.Drawing.Point(418, 43);
            this.lbl2814.Name = "lbl2814";
            this.lbl2814.Size = new System.Drawing.Size(26, 16);
            this.lbl2814.TabIndex = 214;
            this.lbl2814.Text = "630";
            this.lbl2814.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnTXEQReset28
            // 
            this.btnTXEQReset28.Image = null;
            this.btnTXEQReset28.Location = new System.Drawing.Point(777, 18);
            this.btnTXEQReset28.Name = "btnTXEQReset28";
            this.btnTXEQReset28.Size = new System.Drawing.Size(56, 20);
            this.btnTXEQReset28.TabIndex = 131;
            this.btnTXEQReset28.Text = "Reset";
            this.btnTXEQReset28.Click += new System.EventHandler(this.btnTXEQReset28_Click);
            // 
            // lbl2825
            // 
            this.lbl2825.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl2825.Image = null;
            this.lbl2825.Location = new System.Drawing.Point(701, 43);
            this.lbl2825.Name = "lbl2825";
            this.lbl2825.Size = new System.Drawing.Size(26, 16);
            this.lbl2825.TabIndex = 236;
            this.lbl2825.Text = "8k";
            this.lbl2825.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl2821
            // 
            this.lbl2821.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl2821.Image = null;
            this.lbl2821.Location = new System.Drawing.Point(594, 43);
            this.lbl2821.Name = "lbl2821";
            this.lbl2821.Size = new System.Drawing.Size(31, 16);
            this.lbl2821.TabIndex = 228;
            this.lbl2821.Text = "3.15k";
            this.lbl2821.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl2828
            // 
            this.lbl2828.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl2828.Image = null;
            this.lbl2828.Location = new System.Drawing.Point(780, 43);
            this.lbl2828.Name = "lbl2828";
            this.lbl2828.Size = new System.Drawing.Size(26, 16);
            this.lbl2828.TabIndex = 242;
            this.lbl2828.Text = "16k";
            this.lbl2828.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl2827
            // 
            this.lbl2827.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl2827.Image = null;
            this.lbl2827.Location = new System.Drawing.Point(750, 43);
            this.lbl2827.Name = "lbl2827";
            this.lbl2827.Size = new System.Drawing.Size(34, 16);
            this.lbl2827.TabIndex = 241;
            this.lbl2827.Text = "12.5k";
            this.lbl2827.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbTX28EQ28
            // 
            this.tbTX28EQ28.AutoSize = false;
            this.tbTX28EQ28.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.tbTX28EQ28.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tbTX28EQ28.LargeChange = 1;
            this.tbTX28EQ28.Location = new System.Drawing.Point(777, 62);
            this.tbTX28EQ28.Maximum = 15;
            this.tbTX28EQ28.Minimum = -15;
            this.tbTX28EQ28.Name = "tbTX28EQ28";
            this.tbTX28EQ28.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTX28EQ28.Size = new System.Drawing.Size(23, 128);
            this.tbTX28EQ28.TabIndex = 240;
            this.tbTX28EQ28.TickFrequency = 3;
            this.tbTX28EQ28.Scroll += new System.EventHandler(this.tbTX28EQ15_Scroll);
            this.tbTX28EQ28.MouseHover += new System.EventHandler(this.tbTX28EQ28_MouseHover);
            // 
            // tbTX28EQ27
            // 
            this.tbTX28EQ27.AutoSize = false;
            this.tbTX28EQ27.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.tbTX28EQ27.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tbTX28EQ27.LargeChange = 1;
            this.tbTX28EQ27.Location = new System.Drawing.Point(753, 62);
            this.tbTX28EQ27.Maximum = 15;
            this.tbTX28EQ27.Minimum = -15;
            this.tbTX28EQ27.Name = "tbTX28EQ27";
            this.tbTX28EQ27.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTX28EQ27.Size = new System.Drawing.Size(23, 128);
            this.tbTX28EQ27.TabIndex = 239;
            this.tbTX28EQ27.TickFrequency = 3;
            this.tbTX28EQ27.Scroll += new System.EventHandler(this.tbTX28EQ15_Scroll);
            this.tbTX28EQ27.MouseHover += new System.EventHandler(this.tbTX28EQ27_MouseHover);
            // 
            // lbl2826
            // 
            this.lbl2826.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl2826.Image = null;
            this.lbl2826.Location = new System.Drawing.Point(718, 43);
            this.lbl2826.Name = "lbl2826";
            this.lbl2826.Size = new System.Drawing.Size(40, 16);
            this.lbl2826.TabIndex = 229;
            this.lbl2826.Text = "10.0k";
            this.lbl2826.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbTX28EQ26
            // 
            this.tbTX28EQ26.AutoSize = false;
            this.tbTX28EQ26.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.tbTX28EQ26.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tbTX28EQ26.LargeChange = 1;
            this.tbTX28EQ26.Location = new System.Drawing.Point(727, 62);
            this.tbTX28EQ26.Maximum = 15;
            this.tbTX28EQ26.Minimum = -15;
            this.tbTX28EQ26.Name = "tbTX28EQ26";
            this.tbTX28EQ26.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTX28EQ26.Size = new System.Drawing.Size(23, 128);
            this.tbTX28EQ26.TabIndex = 238;
            this.tbTX28EQ26.TickFrequency = 3;
            this.tbTX28EQ26.Scroll += new System.EventHandler(this.tbTX28EQ15_Scroll);
            this.tbTX28EQ26.MouseHover += new System.EventHandler(this.tbTX28EQ26_MouseHover);
            // 
            // tbTX28EQ25
            // 
            this.tbTX28EQ25.AutoSize = false;
            this.tbTX28EQ25.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.tbTX28EQ25.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tbTX28EQ25.LargeChange = 1;
            this.tbTX28EQ25.Location = new System.Drawing.Point(702, 62);
            this.tbTX28EQ25.Maximum = 15;
            this.tbTX28EQ25.Minimum = -15;
            this.tbTX28EQ25.Name = "tbTX28EQ25";
            this.tbTX28EQ25.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTX28EQ25.Size = new System.Drawing.Size(23, 128);
            this.tbTX28EQ25.TabIndex = 237;
            this.tbTX28EQ25.TickFrequency = 3;
            this.tbTX28EQ25.Scroll += new System.EventHandler(this.tbTX28EQ15_Scroll);
            this.tbTX28EQ25.MouseHover += new System.EventHandler(this.tbTX28EQ25_MouseHover);
            // 
            // lbl2824
            // 
            this.lbl2824.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl2824.Image = null;
            this.lbl2824.Location = new System.Drawing.Point(670, 43);
            this.lbl2824.Name = "lbl2824";
            this.lbl2824.Size = new System.Drawing.Size(38, 16);
            this.lbl2824.TabIndex = 235;
            this.lbl2824.Text = "6.3k";
            this.lbl2824.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl2823
            // 
            this.lbl2823.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl2823.Image = null;
            this.lbl2823.Location = new System.Drawing.Point(642, 43);
            this.lbl2823.Name = "lbl2823";
            this.lbl2823.Size = new System.Drawing.Size(34, 16);
            this.lbl2823.TabIndex = 234;
            this.lbl2823.Text = "5.0k";
            this.lbl2823.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbTX28EQ24
            // 
            this.tbTX28EQ24.AutoSize = false;
            this.tbTX28EQ24.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.tbTX28EQ24.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tbTX28EQ24.LargeChange = 1;
            this.tbTX28EQ24.Location = new System.Drawing.Point(675, 62);
            this.tbTX28EQ24.Maximum = 15;
            this.tbTX28EQ24.Minimum = -15;
            this.tbTX28EQ24.Name = "tbTX28EQ24";
            this.tbTX28EQ24.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTX28EQ24.Size = new System.Drawing.Size(23, 128);
            this.tbTX28EQ24.TabIndex = 233;
            this.tbTX28EQ24.TickFrequency = 3;
            this.tbTX28EQ24.Scroll += new System.EventHandler(this.tbTX28EQ15_Scroll);
            this.tbTX28EQ24.MouseHover += new System.EventHandler(this.tbTX28EQ24_MouseHover);
            // 
            // tbTX28EQ23
            // 
            this.tbTX28EQ23.AutoSize = false;
            this.tbTX28EQ23.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.tbTX28EQ23.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tbTX28EQ23.LargeChange = 1;
            this.tbTX28EQ23.Location = new System.Drawing.Point(649, 62);
            this.tbTX28EQ23.Maximum = 15;
            this.tbTX28EQ23.Minimum = -15;
            this.tbTX28EQ23.Name = "tbTX28EQ23";
            this.tbTX28EQ23.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTX28EQ23.Size = new System.Drawing.Size(23, 128);
            this.tbTX28EQ23.TabIndex = 232;
            this.tbTX28EQ23.TickFrequency = 3;
            this.tbTX28EQ23.Scroll += new System.EventHandler(this.tbTX28EQ15_Scroll);
            this.tbTX28EQ23.MouseHover += new System.EventHandler(this.tbTX28EQ23_MouseHover);
            // 
            // lbl2822
            // 
            this.lbl2822.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl2822.Image = null;
            this.lbl2822.Location = new System.Drawing.Point(619, 43);
            this.lbl2822.Name = "lbl2822";
            this.lbl2822.Size = new System.Drawing.Size(32, 16);
            this.lbl2822.TabIndex = 230;
            this.lbl2822.Text = "4k";
            this.lbl2822.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbTX28EQ22
            // 
            this.tbTX28EQ22.AutoSize = false;
            this.tbTX28EQ22.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.tbTX28EQ22.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tbTX28EQ22.LargeChange = 1;
            this.tbTX28EQ22.Location = new System.Drawing.Point(623, 62);
            this.tbTX28EQ22.Maximum = 15;
            this.tbTX28EQ22.Minimum = -15;
            this.tbTX28EQ22.Name = "tbTX28EQ22";
            this.tbTX28EQ22.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTX28EQ22.Size = new System.Drawing.Size(23, 128);
            this.tbTX28EQ22.TabIndex = 231;
            this.tbTX28EQ22.TickFrequency = 3;
            this.tbTX28EQ22.Scroll += new System.EventHandler(this.tbTX28EQ15_Scroll);
            this.tbTX28EQ22.MouseHover += new System.EventHandler(this.tbTX28EQ22_MouseHover);
            // 
            // lbl2820
            // 
            this.lbl2820.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl2820.Image = null;
            this.lbl2820.Location = new System.Drawing.Point(566, 43);
            this.lbl2820.Name = "lbl2820";
            this.lbl2820.Size = new System.Drawing.Size(34, 16);
            this.lbl2820.TabIndex = 227;
            this.lbl2820.Text = "2.5k";
            this.lbl2820.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbTX28EQ21
            // 
            this.tbTX28EQ21.AutoSize = false;
            this.tbTX28EQ21.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.tbTX28EQ21.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tbTX28EQ21.LargeChange = 1;
            this.tbTX28EQ21.Location = new System.Drawing.Point(596, 62);
            this.tbTX28EQ21.Maximum = 15;
            this.tbTX28EQ21.Minimum = -15;
            this.tbTX28EQ21.Name = "tbTX28EQ21";
            this.tbTX28EQ21.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTX28EQ21.Size = new System.Drawing.Size(23, 128);
            this.tbTX28EQ21.TabIndex = 226;
            this.tbTX28EQ21.TickFrequency = 3;
            this.tbTX28EQ21.Scroll += new System.EventHandler(this.tbTX28EQ15_Scroll);
            this.tbTX28EQ21.MouseHover += new System.EventHandler(this.tbTX28EQ21_MouseHover);
            // 
            // tbTX28EQ20
            // 
            this.tbTX28EQ20.AutoSize = false;
            this.tbTX28EQ20.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.tbTX28EQ20.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tbTX28EQ20.LargeChange = 1;
            this.tbTX28EQ20.Location = new System.Drawing.Point(571, 62);
            this.tbTX28EQ20.Maximum = 15;
            this.tbTX28EQ20.Minimum = -15;
            this.tbTX28EQ20.Name = "tbTX28EQ20";
            this.tbTX28EQ20.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTX28EQ20.Size = new System.Drawing.Size(23, 128);
            this.tbTX28EQ20.TabIndex = 225;
            this.tbTX28EQ20.TickFrequency = 3;
            this.tbTX28EQ20.Scroll += new System.EventHandler(this.tbTX28EQ15_Scroll);
            this.tbTX28EQ20.MouseHover += new System.EventHandler(this.tbTX28EQ20_MouseHover);
            // 
            // lbl2819
            // 
            this.lbl2819.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl2819.Image = null;
            this.lbl2819.Location = new System.Drawing.Point(543, 43);
            this.lbl2819.Name = "lbl2819";
            this.lbl2819.Size = new System.Drawing.Size(32, 16);
            this.lbl2819.TabIndex = 215;
            this.lbl2819.Text = "2k";
            this.lbl2819.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbTX28EQ19
            // 
            this.tbTX28EQ19.AutoSize = false;
            this.tbTX28EQ19.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.tbTX28EQ19.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tbTX28EQ19.LargeChange = 1;
            this.tbTX28EQ19.Location = new System.Drawing.Point(546, 62);
            this.tbTX28EQ19.Maximum = 15;
            this.tbTX28EQ19.Minimum = -15;
            this.tbTX28EQ19.Name = "tbTX28EQ19";
            this.tbTX28EQ19.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTX28EQ19.Size = new System.Drawing.Size(23, 128);
            this.tbTX28EQ19.TabIndex = 224;
            this.tbTX28EQ19.TickFrequency = 3;
            this.tbTX28EQ19.Scroll += new System.EventHandler(this.tbTX28EQ15_Scroll);
            this.tbTX28EQ19.MouseHover += new System.EventHandler(this.tbTX28EQ19_MouseHover);
            // 
            // tbTX28EQ18
            // 
            this.tbTX28EQ18.AutoSize = false;
            this.tbTX28EQ18.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.tbTX28EQ18.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tbTX28EQ18.LargeChange = 1;
            this.tbTX28EQ18.Location = new System.Drawing.Point(521, 62);
            this.tbTX28EQ18.Maximum = 15;
            this.tbTX28EQ18.Minimum = -15;
            this.tbTX28EQ18.Name = "tbTX28EQ18";
            this.tbTX28EQ18.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTX28EQ18.Size = new System.Drawing.Size(23, 128);
            this.tbTX28EQ18.TabIndex = 223;
            this.tbTX28EQ18.TickFrequency = 3;
            this.tbTX28EQ18.Scroll += new System.EventHandler(this.tbTX28EQ15_Scroll);
            this.tbTX28EQ18.MouseHover += new System.EventHandler(this.tbTX28EQ18_MouseHover);
            // 
            // lbl2818
            // 
            this.lbl2818.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl2818.Image = null;
            this.lbl2818.Location = new System.Drawing.Point(515, 43);
            this.lbl2818.Name = "lbl2818";
            this.lbl2818.Size = new System.Drawing.Size(33, 16);
            this.lbl2818.TabIndex = 222;
            this.lbl2818.Text = "1.6k";
            this.lbl2818.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl2817
            // 
            this.lbl2817.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl2817.Image = null;
            this.lbl2817.Location = new System.Drawing.Point(487, 43);
            this.lbl2817.Name = "lbl2817";
            this.lbl2817.Size = new System.Drawing.Size(31, 16);
            this.lbl2817.TabIndex = 221;
            this.lbl2817.Text = "1.25k";
            this.lbl2817.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl2816
            // 
            this.lbl2816.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl2816.Image = null;
            this.lbl2816.Location = new System.Drawing.Point(461, 43);
            this.lbl2816.Name = "lbl2816";
            this.lbl2816.Size = new System.Drawing.Size(34, 16);
            this.lbl2816.TabIndex = 220;
            this.lbl2816.Text = "1k";
            this.lbl2816.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbTX28EQ17
            // 
            this.tbTX28EQ17.AutoSize = false;
            this.tbTX28EQ17.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.tbTX28EQ17.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tbTX28EQ17.LargeChange = 1;
            this.tbTX28EQ17.Location = new System.Drawing.Point(494, 62);
            this.tbTX28EQ17.Maximum = 15;
            this.tbTX28EQ17.Minimum = -15;
            this.tbTX28EQ17.Name = "tbTX28EQ17";
            this.tbTX28EQ17.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTX28EQ17.Size = new System.Drawing.Size(23, 128);
            this.tbTX28EQ17.TabIndex = 219;
            this.tbTX28EQ17.TickFrequency = 3;
            this.tbTX28EQ17.Scroll += new System.EventHandler(this.tbTX28EQ15_Scroll);
            this.tbTX28EQ17.MouseHover += new System.EventHandler(this.tbTX28EQ17_MouseHover);
            // 
            // tbTX28EQ16
            // 
            this.tbTX28EQ16.AutoSize = false;
            this.tbTX28EQ16.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.tbTX28EQ16.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tbTX28EQ16.LargeChange = 1;
            this.tbTX28EQ16.Location = new System.Drawing.Point(468, 62);
            this.tbTX28EQ16.Maximum = 15;
            this.tbTX28EQ16.Minimum = -15;
            this.tbTX28EQ16.Name = "tbTX28EQ16";
            this.tbTX28EQ16.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTX28EQ16.Size = new System.Drawing.Size(23, 128);
            this.tbTX28EQ16.TabIndex = 218;
            this.tbTX28EQ16.TickFrequency = 3;
            this.tbTX28EQ16.Scroll += new System.EventHandler(this.tbTX28EQ15_Scroll);
            this.tbTX28EQ16.MouseHover += new System.EventHandler(this.tbTX28EQ16_MouseHover);
            // 
            // tbTX28EQ15
            // 
            this.tbTX28EQ15.AutoSize = false;
            this.tbTX28EQ15.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.tbTX28EQ15.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tbTX28EQ15.LargeChange = 1;
            this.tbTX28EQ15.Location = new System.Drawing.Point(442, 62);
            this.tbTX28EQ15.Maximum = 15;
            this.tbTX28EQ15.Minimum = -15;
            this.tbTX28EQ15.Name = "tbTX28EQ15";
            this.tbTX28EQ15.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTX28EQ15.Size = new System.Drawing.Size(23, 128);
            this.tbTX28EQ15.TabIndex = 216;
            this.tbTX28EQ15.TickFrequency = 3;
            this.tbTX28EQ15.Scroll += new System.EventHandler(this.tbTX28EQ15_Scroll);
            this.tbTX28EQ15.MouseHover += new System.EventHandler(this.tbTX28EQ15_MouseHover);
            // 
            // lbl287
            // 
            this.lbl287.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl287.Image = null;
            this.lbl287.Location = new System.Drawing.Point(237, 43);
            this.lbl287.Name = "lbl287";
            this.lbl287.Size = new System.Drawing.Size(26, 16);
            this.lbl287.TabIndex = 200;
            this.lbl287.Text = "125";
            this.lbl287.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl2813
            // 
            this.lbl2813.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl2813.Image = null;
            this.lbl2813.Location = new System.Drawing.Point(388, 43);
            this.lbl2813.Name = "lbl2813";
            this.lbl2813.Size = new System.Drawing.Size(34, 16);
            this.lbl2813.TabIndex = 213;
            this.lbl2813.Text = "500";
            this.lbl2813.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbTX28EQ14
            // 
            this.tbTX28EQ14.AutoSize = false;
            this.tbTX28EQ14.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.tbTX28EQ14.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tbTX28EQ14.LargeChange = 1;
            this.tbTX28EQ14.Location = new System.Drawing.Point(415, 62);
            this.tbTX28EQ14.Maximum = 15;
            this.tbTX28EQ14.Minimum = -15;
            this.tbTX28EQ14.Name = "tbTX28EQ14";
            this.tbTX28EQ14.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTX28EQ14.Size = new System.Drawing.Size(23, 128);
            this.tbTX28EQ14.TabIndex = 212;
            this.tbTX28EQ14.TickFrequency = 3;
            this.tbTX28EQ14.Scroll += new System.EventHandler(this.tbTX28EQ15_Scroll);
            this.tbTX28EQ14.MouseHover += new System.EventHandler(this.tbTX28EQ14_MouseHover);
            // 
            // tbTX28EQ13
            // 
            this.tbTX28EQ13.AutoSize = false;
            this.tbTX28EQ13.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.tbTX28EQ13.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tbTX28EQ13.LargeChange = 1;
            this.tbTX28EQ13.Location = new System.Drawing.Point(390, 62);
            this.tbTX28EQ13.Maximum = 15;
            this.tbTX28EQ13.Minimum = -15;
            this.tbTX28EQ13.Name = "tbTX28EQ13";
            this.tbTX28EQ13.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTX28EQ13.Size = new System.Drawing.Size(23, 128);
            this.tbTX28EQ13.TabIndex = 211;
            this.tbTX28EQ13.TickFrequency = 3;
            this.tbTX28EQ13.Scroll += new System.EventHandler(this.tbTX28EQ15_Scroll);
            this.tbTX28EQ13.MouseHover += new System.EventHandler(this.tbTX28EQ13_MouseHover);
            // 
            // lbl2812
            // 
            this.lbl2812.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl2812.Image = null;
            this.lbl2812.Location = new System.Drawing.Point(362, 43);
            this.lbl2812.Name = "lbl2812";
            this.lbl2812.Size = new System.Drawing.Size(32, 16);
            this.lbl2812.TabIndex = 201;
            this.lbl2812.Text = "400";
            this.lbl2812.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbTX28EQ12
            // 
            this.tbTX28EQ12.AutoSize = false;
            this.tbTX28EQ12.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.tbTX28EQ12.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tbTX28EQ12.LargeChange = 1;
            this.tbTX28EQ12.Location = new System.Drawing.Point(365, 62);
            this.tbTX28EQ12.Maximum = 15;
            this.tbTX28EQ12.Minimum = -15;
            this.tbTX28EQ12.Name = "tbTX28EQ12";
            this.tbTX28EQ12.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTX28EQ12.Size = new System.Drawing.Size(23, 128);
            this.tbTX28EQ12.TabIndex = 210;
            this.tbTX28EQ12.TickFrequency = 3;
            this.tbTX28EQ12.Scroll += new System.EventHandler(this.tbTX28EQ15_Scroll);
            this.tbTX28EQ12.MouseHover += new System.EventHandler(this.tbTX28EQ12_MouseHover);
            // 
            // tbTX28EQ11
            // 
            this.tbTX28EQ11.AutoSize = false;
            this.tbTX28EQ11.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.tbTX28EQ11.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tbTX28EQ11.LargeChange = 1;
            this.tbTX28EQ11.Location = new System.Drawing.Point(340, 62);
            this.tbTX28EQ11.Maximum = 15;
            this.tbTX28EQ11.Minimum = -15;
            this.tbTX28EQ11.Name = "tbTX28EQ11";
            this.tbTX28EQ11.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTX28EQ11.Size = new System.Drawing.Size(23, 128);
            this.tbTX28EQ11.TabIndex = 209;
            this.tbTX28EQ11.TickFrequency = 3;
            this.tbTX28EQ11.Scroll += new System.EventHandler(this.tbTX28EQ15_Scroll);
            this.tbTX28EQ11.MouseHover += new System.EventHandler(this.tbTX28EQ11_MouseHover);
            // 
            // lbl2811
            // 
            this.lbl2811.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl2811.Image = null;
            this.lbl2811.Location = new System.Drawing.Point(339, 43);
            this.lbl2811.Name = "lbl2811";
            this.lbl2811.Size = new System.Drawing.Size(26, 16);
            this.lbl2811.TabIndex = 208;
            this.lbl2811.Text = "315";
            this.lbl2811.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl2810
            // 
            this.lbl2810.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl2810.Image = null;
            this.lbl2810.Location = new System.Drawing.Point(310, 43);
            this.lbl2810.Name = "lbl2810";
            this.lbl2810.Size = new System.Drawing.Size(26, 16);
            this.lbl2810.TabIndex = 207;
            this.lbl2810.Text = "250";
            this.lbl2810.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl289
            // 
            this.lbl289.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl289.Image = null;
            this.lbl289.Location = new System.Drawing.Point(280, 43);
            this.lbl289.Name = "lbl289";
            this.lbl289.Size = new System.Drawing.Size(34, 16);
            this.lbl289.TabIndex = 206;
            this.lbl289.Text = "200";
            this.lbl289.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbTX28EQ10
            // 
            this.tbTX28EQ10.AutoSize = false;
            this.tbTX28EQ10.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.tbTX28EQ10.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tbTX28EQ10.LargeChange = 1;
            this.tbTX28EQ10.Location = new System.Drawing.Point(313, 62);
            this.tbTX28EQ10.Maximum = 15;
            this.tbTX28EQ10.Minimum = -15;
            this.tbTX28EQ10.Name = "tbTX28EQ10";
            this.tbTX28EQ10.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTX28EQ10.Size = new System.Drawing.Size(23, 128);
            this.tbTX28EQ10.TabIndex = 205;
            this.tbTX28EQ10.TickFrequency = 3;
            this.tbTX28EQ10.Scroll += new System.EventHandler(this.tbTX28EQ15_Scroll);
            this.tbTX28EQ10.MouseHover += new System.EventHandler(this.tbTX28EQ10_MouseHover);
            // 
            // tbTX28EQ9
            // 
            this.tbTX28EQ9.AutoSize = false;
            this.tbTX28EQ9.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.tbTX28EQ9.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tbTX28EQ9.LargeChange = 1;
            this.tbTX28EQ9.Location = new System.Drawing.Point(287, 62);
            this.tbTX28EQ9.Maximum = 15;
            this.tbTX28EQ9.Minimum = -15;
            this.tbTX28EQ9.Name = "tbTX28EQ9";
            this.tbTX28EQ9.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTX28EQ9.Size = new System.Drawing.Size(23, 128);
            this.tbTX28EQ9.TabIndex = 204;
            this.tbTX28EQ9.TickFrequency = 3;
            this.tbTX28EQ9.Scroll += new System.EventHandler(this.tbTX28EQ15_Scroll);
            this.tbTX28EQ9.MouseHover += new System.EventHandler(this.tbTX28EQ9_MouseHover);
            // 
            // lbl288
            // 
            this.lbl288.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl288.Image = null;
            this.lbl288.Location = new System.Drawing.Point(257, 43);
            this.lbl288.Name = "lbl288";
            this.lbl288.Size = new System.Drawing.Size(32, 16);
            this.lbl288.TabIndex = 202;
            this.lbl288.Text = "160";
            this.lbl288.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbTX28EQ8
            // 
            this.tbTX28EQ8.AutoSize = false;
            this.tbTX28EQ8.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.tbTX28EQ8.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tbTX28EQ8.LargeChange = 1;
            this.tbTX28EQ8.Location = new System.Drawing.Point(261, 62);
            this.tbTX28EQ8.Maximum = 15;
            this.tbTX28EQ8.Minimum = -15;
            this.tbTX28EQ8.Name = "tbTX28EQ8";
            this.tbTX28EQ8.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTX28EQ8.Size = new System.Drawing.Size(23, 128);
            this.tbTX28EQ8.TabIndex = 203;
            this.tbTX28EQ8.TickFrequency = 3;
            this.tbTX28EQ8.Scroll += new System.EventHandler(this.tbTX28EQ15_Scroll);
            this.tbTX28EQ8.MouseHover += new System.EventHandler(this.tbTX28EQ8_MouseHover);
            // 
            // lbl286
            // 
            this.lbl286.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl286.Image = null;
            this.lbl286.Location = new System.Drawing.Point(204, 43);
            this.lbl286.Name = "lbl286";
            this.lbl286.Size = new System.Drawing.Size(34, 16);
            this.lbl286.TabIndex = 199;
            this.lbl286.Text = "100";
            this.lbl286.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbTX28EQ7
            // 
            this.tbTX28EQ7.AutoSize = false;
            this.tbTX28EQ7.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.tbTX28EQ7.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tbTX28EQ7.LargeChange = 1;
            this.tbTX28EQ7.Location = new System.Drawing.Point(235, 62);
            this.tbTX28EQ7.Maximum = 15;
            this.tbTX28EQ7.Minimum = -15;
            this.tbTX28EQ7.Name = "tbTX28EQ7";
            this.tbTX28EQ7.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTX28EQ7.Size = new System.Drawing.Size(23, 128);
            this.tbTX28EQ7.TabIndex = 198;
            this.tbTX28EQ7.TickFrequency = 3;
            this.tbTX28EQ7.Scroll += new System.EventHandler(this.tbTX28EQ15_Scroll);
            this.tbTX28EQ7.MouseHover += new System.EventHandler(this.tbTX28EQ7_MouseHover);
            // 
            // tbTX28EQ6
            // 
            this.tbTX28EQ6.AutoSize = false;
            this.tbTX28EQ6.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.tbTX28EQ6.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tbTX28EQ6.LargeChange = 1;
            this.tbTX28EQ6.Location = new System.Drawing.Point(209, 62);
            this.tbTX28EQ6.Maximum = 15;
            this.tbTX28EQ6.Minimum = -15;
            this.tbTX28EQ6.Name = "tbTX28EQ6";
            this.tbTX28EQ6.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTX28EQ6.Size = new System.Drawing.Size(23, 128);
            this.tbTX28EQ6.TabIndex = 197;
            this.tbTX28EQ6.TickFrequency = 3;
            this.tbTX28EQ6.Scroll += new System.EventHandler(this.tbTX28EQ15_Scroll);
            this.tbTX28EQ6.MouseHover += new System.EventHandler(this.tbTX28EQ6_MouseHover);
            // 
            // lbl285
            // 
            this.lbl285.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl285.Image = null;
            this.lbl285.Location = new System.Drawing.Point(181, 43);
            this.lbl285.Name = "lbl285";
            this.lbl285.Size = new System.Drawing.Size(32, 16);
            this.lbl285.TabIndex = 187;
            this.lbl285.Text = "80";
            this.lbl285.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbTX28EQ5
            // 
            this.tbTX28EQ5.AutoSize = false;
            this.tbTX28EQ5.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.tbTX28EQ5.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tbTX28EQ5.LargeChange = 1;
            this.tbTX28EQ5.Location = new System.Drawing.Point(184, 62);
            this.tbTX28EQ5.Maximum = 15;
            this.tbTX28EQ5.Minimum = -15;
            this.tbTX28EQ5.Name = "tbTX28EQ5";
            this.tbTX28EQ5.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTX28EQ5.Size = new System.Drawing.Size(23, 128);
            this.tbTX28EQ5.TabIndex = 196;
            this.tbTX28EQ5.TickFrequency = 3;
            this.tbTX28EQ5.Scroll += new System.EventHandler(this.tbTX28EQ15_Scroll);
            this.tbTX28EQ5.MouseHover += new System.EventHandler(this.tbTX28EQ5_MouseHover);
            // 
            // tbTX28EQ4
            // 
            this.tbTX28EQ4.AutoSize = false;
            this.tbTX28EQ4.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.tbTX28EQ4.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tbTX28EQ4.LargeChange = 1;
            this.tbTX28EQ4.Location = new System.Drawing.Point(159, 62);
            this.tbTX28EQ4.Maximum = 15;
            this.tbTX28EQ4.Minimum = -15;
            this.tbTX28EQ4.Name = "tbTX28EQ4";
            this.tbTX28EQ4.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTX28EQ4.Size = new System.Drawing.Size(23, 128);
            this.tbTX28EQ4.TabIndex = 195;
            this.tbTX28EQ4.TickFrequency = 3;
            this.tbTX28EQ4.Scroll += new System.EventHandler(this.tbTX28EQ15_Scroll);
            this.tbTX28EQ4.MouseHover += new System.EventHandler(this.tbTX28EQ4_MouseHover);
            // 
            // lbl284
            // 
            this.lbl284.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl284.Image = null;
            this.lbl284.Location = new System.Drawing.Point(158, 43);
            this.lbl284.Name = "lbl284";
            this.lbl284.Size = new System.Drawing.Size(26, 16);
            this.lbl284.TabIndex = 194;
            this.lbl284.Text = "63";
            this.lbl284.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl283
            // 
            this.lbl283.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl283.Image = null;
            this.lbl283.Location = new System.Drawing.Point(129, 43);
            this.lbl283.Name = "lbl283";
            this.lbl283.Size = new System.Drawing.Size(26, 16);
            this.lbl283.TabIndex = 193;
            this.lbl283.Text = "50";
            this.lbl283.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl282
            // 
            this.lbl282.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl282.Image = null;
            this.lbl282.Location = new System.Drawing.Point(99, 43);
            this.lbl282.Name = "lbl282";
            this.lbl282.Size = new System.Drawing.Size(34, 16);
            this.lbl282.TabIndex = 192;
            this.lbl282.Text = "40";
            this.lbl282.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl282.Click += new System.EventHandler(this.lbl282_Click);
            // 
            // tbTX28EQ3
            // 
            this.tbTX28EQ3.AutoSize = false;
            this.tbTX28EQ3.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.tbTX28EQ3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tbTX28EQ3.LargeChange = 1;
            this.tbTX28EQ3.Location = new System.Drawing.Point(132, 62);
            this.tbTX28EQ3.Maximum = 15;
            this.tbTX28EQ3.Minimum = -15;
            this.tbTX28EQ3.Name = "tbTX28EQ3";
            this.tbTX28EQ3.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTX28EQ3.Size = new System.Drawing.Size(23, 128);
            this.tbTX28EQ3.TabIndex = 191;
            this.tbTX28EQ3.TickFrequency = 3;
            this.tbTX28EQ3.Scroll += new System.EventHandler(this.tbTX28EQ15_Scroll);
            this.tbTX28EQ3.MouseHover += new System.EventHandler(this.tbTX28EQ3_MouseHover);
            // 
            // tbTX28EQ2
            // 
            this.tbTX28EQ2.AutoSize = false;
            this.tbTX28EQ2.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.tbTX28EQ2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tbTX28EQ2.LargeChange = 1;
            this.tbTX28EQ2.Location = new System.Drawing.Point(106, 62);
            this.tbTX28EQ2.Maximum = 15;
            this.tbTX28EQ2.Minimum = -15;
            this.tbTX28EQ2.Name = "tbTX28EQ2";
            this.tbTX28EQ2.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTX28EQ2.Size = new System.Drawing.Size(23, 128);
            this.tbTX28EQ2.TabIndex = 190;
            this.tbTX28EQ2.TickFrequency = 3;
            this.tbTX28EQ2.Scroll += new System.EventHandler(this.tbTX28EQ15_Scroll);
            this.tbTX28EQ2.MouseHover += new System.EventHandler(this.tbTX28EQ2_MouseHover);
            // 
            // lbl281
            // 
            this.lbl281.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl281.Image = null;
            this.lbl281.Location = new System.Drawing.Point(73, 43);
            this.lbl281.Name = "lbl281";
            this.lbl281.Size = new System.Drawing.Size(32, 16);
            this.lbl281.TabIndex = 188;
            this.lbl281.Text = "32";
            this.lbl281.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbTX28EQ1
            // 
            this.tbTX28EQ1.AutoSize = false;
            this.tbTX28EQ1.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.tbTX28EQ1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tbTX28EQ1.LargeChange = 1;
            this.tbTX28EQ1.Location = new System.Drawing.Point(82, 62);
            this.tbTX28EQ1.Maximum = 15;
            this.tbTX28EQ1.Minimum = -15;
            this.tbTX28EQ1.Name = "tbTX28EQ1";
            this.tbTX28EQ1.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTX28EQ1.Size = new System.Drawing.Size(23, 128);
            this.tbTX28EQ1.TabIndex = 189;
            this.tbTX28EQ1.TickFrequency = 3;
            this.tbTX28EQ1.Scroll += new System.EventHandler(this.tbTX28EQ15_Scroll);
            this.tbTX28EQ1.MouseHover += new System.EventHandler(this.tbTX28EQ1_MouseHover);
            // 
            // labelTS6
            // 
            this.labelTS6.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTS6.Image = null;
            this.labelTS6.Location = new System.Drawing.Point(56, 174);
            this.labelTS6.Name = "labelTS6";
            this.labelTS6.Size = new System.Drawing.Size(42, 16);
            this.labelTS6.TabIndex = 245;
            this.labelTS6.Text = "-15dB";
            this.labelTS6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelTS3
            // 
            this.labelTS3.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTS3.Image = null;
            this.labelTS3.Location = new System.Drawing.Point(806, 174);
            this.labelTS3.Name = "labelTS3";
            this.labelTS3.Size = new System.Drawing.Size(34, 16);
            this.labelTS3.TabIndex = 131;
            this.labelTS3.Text = "-15dB";
            this.labelTS3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnTXEQReset
            // 
            this.btnTXEQReset.Image = null;
            this.btnTXEQReset.Location = new System.Drawing.Point(406, 320);
            this.btnTXEQReset.Name = "btnTXEQReset";
            this.btnTXEQReset.Size = new System.Drawing.Size(56, 20);
            this.btnTXEQReset.TabIndex = 107;
            this.btnTXEQReset.Text = "Reset";
            this.btnTXEQReset.Click += new System.EventHandler(this.btnTXEQReset_Click);
            // 
            // chkTXEQ160Notch
            // 
            this.chkTXEQ160Notch.Image = null;
            this.chkTXEQ160Notch.Location = new System.Drawing.Point(276, 323);
            this.chkTXEQ160Notch.Name = "chkTXEQ160Notch";
            this.chkTXEQ160Notch.Size = new System.Drawing.Size(96, 16);
            this.chkTXEQ160Notch.TabIndex = 113;
            this.chkTXEQ160Notch.Text = "160Hz Notch";
            this.chkTXEQ160Notch.Visible = false;
            // 
            // picTXEQ
            // 
            this.picTXEQ.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.picTXEQ.Location = new System.Drawing.Point(88, 24);
            this.picTXEQ.Name = "picTXEQ";
            this.picTXEQ.Size = new System.Drawing.Size(384, 111);
            this.picTXEQ.TabIndex = 112;
            this.picTXEQ.TabStop = false;
            this.toolTip1.SetToolTip(this.picTXEQ, "When Enabled will show you the +/- dB gain for each Frequency Band.\r\n");
            this.picTXEQ.Paint += new System.Windows.Forms.PaintEventHandler(this.picTXEQ_Paint);
            // 
            // lblTXEQ15db2
            // 
            this.lblTXEQ15db2.Image = null;
            this.lblTXEQ15db2.Location = new System.Drawing.Point(483, 205);
            this.lblTXEQ15db2.Name = "lblTXEQ15db2";
            this.lblTXEQ15db2.Size = new System.Drawing.Size(32, 16);
            this.lblTXEQ15db2.TabIndex = 129;
            this.lblTXEQ15db2.Text = "15dB";
            this.lblTXEQ15db2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTXEQ0dB2
            // 
            this.lblTXEQ0dB2.Image = null;
            this.lblTXEQ0dB2.Location = new System.Drawing.Point(483, 261);
            this.lblTXEQ0dB2.Name = "lblTXEQ0dB2";
            this.lblTXEQ0dB2.Size = new System.Drawing.Size(32, 16);
            this.lblTXEQ0dB2.TabIndex = 128;
            this.lblTXEQ0dB2.Text = "  0dB";
            this.lblTXEQ0dB2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTXEQminus12db2
            // 
            this.lblTXEQminus12db2.Image = null;
            this.lblTXEQminus12db2.Location = new System.Drawing.Point(480, 305);
            this.lblTXEQminus12db2.Name = "lblTXEQminus12db2";
            this.lblTXEQminus12db2.Size = new System.Drawing.Size(42, 16);
            this.lblTXEQminus12db2.TabIndex = 130;
            this.lblTXEQminus12db2.Text = "-12dB";
            this.lblTXEQminus12db2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbTXEQ10
            // 
            this.tbTXEQ10.AutoSize = false;
            this.tbTXEQ10.LargeChange = 3;
            this.tbTXEQ10.Location = new System.Drawing.Point(448, 199);
            this.tbTXEQ10.Maximum = 15;
            this.tbTXEQ10.Minimum = -12;
            this.tbTXEQ10.Name = "tbTXEQ10";
            this.tbTXEQ10.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTXEQ10.Size = new System.Drawing.Size(32, 128);
            this.tbTXEQ10.TabIndex = 126;
            this.tbTXEQ10.TickFrequency = 3;
            this.tbTXEQ10.Visible = false;
            this.tbTXEQ10.Scroll += new System.EventHandler(this.tbTXEQ_Scroll);
            this.tbTXEQ10.MouseHover += new System.EventHandler(this.tbTXEQ10_MouseHover);
            // 
            // lblTXEQ10
            // 
            this.lblTXEQ10.Image = null;
            this.lblTXEQ10.Location = new System.Drawing.Point(440, 183);
            this.lblTXEQ10.Name = "lblTXEQ10";
            this.lblTXEQ10.Size = new System.Drawing.Size(40, 16);
            this.lblTXEQ10.TabIndex = 127;
            this.lblTXEQ10.Text = "16K";
            this.lblTXEQ10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.lblTXEQ10, "1500-6000Hz");
            this.lblTXEQ10.Visible = false;
            // 
            // tbTXEQ7
            // 
            this.tbTXEQ7.AutoSize = false;
            this.tbTXEQ7.LargeChange = 3;
            this.tbTXEQ7.Location = new System.Drawing.Point(328, 199);
            this.tbTXEQ7.Maximum = 15;
            this.tbTXEQ7.Minimum = -12;
            this.tbTXEQ7.Name = "tbTXEQ7";
            this.tbTXEQ7.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTXEQ7.Size = new System.Drawing.Size(32, 128);
            this.tbTXEQ7.TabIndex = 120;
            this.tbTXEQ7.TickFrequency = 3;
            this.tbTXEQ7.Visible = false;
            this.tbTXEQ7.Scroll += new System.EventHandler(this.tbTXEQ_Scroll);
            this.tbTXEQ7.MouseHover += new System.EventHandler(this.tbTXEQ7_MouseHover);
            // 
            // tbTXEQ8
            // 
            this.tbTXEQ8.AutoSize = false;
            this.tbTXEQ8.LargeChange = 3;
            this.tbTXEQ8.Location = new System.Drawing.Point(368, 199);
            this.tbTXEQ8.Maximum = 15;
            this.tbTXEQ8.Minimum = -12;
            this.tbTXEQ8.Name = "tbTXEQ8";
            this.tbTXEQ8.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTXEQ8.Size = new System.Drawing.Size(32, 128);
            this.tbTXEQ8.TabIndex = 121;
            this.tbTXEQ8.TickFrequency = 3;
            this.tbTXEQ8.Visible = false;
            this.tbTXEQ8.Scroll += new System.EventHandler(this.tbTXEQ_Scroll);
            this.tbTXEQ8.MouseHover += new System.EventHandler(this.tbTXEQ8_MouseHover);
            // 
            // tbTXEQ9
            // 
            this.tbTXEQ9.AutoSize = false;
            this.tbTXEQ9.LargeChange = 3;
            this.tbTXEQ9.Location = new System.Drawing.Point(408, 199);
            this.tbTXEQ9.Maximum = 15;
            this.tbTXEQ9.Minimum = -12;
            this.tbTXEQ9.Name = "tbTXEQ9";
            this.tbTXEQ9.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTXEQ9.Size = new System.Drawing.Size(32, 128);
            this.tbTXEQ9.TabIndex = 122;
            this.tbTXEQ9.TickFrequency = 3;
            this.tbTXEQ9.Scroll += new System.EventHandler(this.tbTXEQ_Scroll);
            this.tbTXEQ9.MouseHover += new System.EventHandler(this.tbTXEQ9_MouseHover);
            // 
            // lblTXEQ7
            // 
            this.lblTXEQ7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTXEQ7.Image = null;
            this.lblTXEQ7.Location = new System.Drawing.Point(320, 183);
            this.lblTXEQ7.Name = "lblTXEQ7";
            this.lblTXEQ7.Size = new System.Drawing.Size(40, 16);
            this.lblTXEQ7.TabIndex = 123;
            this.lblTXEQ7.Text = "2K";
            this.lblTXEQ7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.lblTXEQ7, "0-400Hz");
            this.lblTXEQ7.Visible = false;
            // 
            // lblTXEQ8
            // 
            this.lblTXEQ8.Image = null;
            this.lblTXEQ8.Location = new System.Drawing.Point(360, 183);
            this.lblTXEQ8.Name = "lblTXEQ8";
            this.lblTXEQ8.Size = new System.Drawing.Size(40, 16);
            this.lblTXEQ8.TabIndex = 124;
            this.lblTXEQ8.Text = "4K";
            this.lblTXEQ8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.lblTXEQ8, "400-1500Hz");
            this.lblTXEQ8.Visible = false;
            // 
            // lblTXEQ9
            // 
            this.lblTXEQ9.Image = null;
            this.lblTXEQ9.Location = new System.Drawing.Point(400, 183);
            this.lblTXEQ9.Name = "lblTXEQ9";
            this.lblTXEQ9.Size = new System.Drawing.Size(40, 16);
            this.lblTXEQ9.TabIndex = 125;
            this.lblTXEQ9.Text = "High";
            this.lblTXEQ9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.lblTXEQ9, "1500-6000Hz");
            // 
            // tbTXEQ4
            // 
            this.tbTXEQ4.AutoSize = false;
            this.tbTXEQ4.LargeChange = 3;
            this.tbTXEQ4.Location = new System.Drawing.Point(208, 199);
            this.tbTXEQ4.Maximum = 15;
            this.tbTXEQ4.Minimum = -12;
            this.tbTXEQ4.Name = "tbTXEQ4";
            this.tbTXEQ4.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTXEQ4.Size = new System.Drawing.Size(32, 128);
            this.tbTXEQ4.TabIndex = 114;
            this.tbTXEQ4.TickFrequency = 3;
            this.tbTXEQ4.Visible = false;
            this.tbTXEQ4.Scroll += new System.EventHandler(this.tbTXEQ_Scroll);
            this.tbTXEQ4.MouseHover += new System.EventHandler(this.tbTXEQ4_MouseHover);
            // 
            // tbTXEQ5
            // 
            this.tbTXEQ5.AutoSize = false;
            this.tbTXEQ5.LargeChange = 3;
            this.tbTXEQ5.Location = new System.Drawing.Point(248, 199);
            this.tbTXEQ5.Maximum = 15;
            this.tbTXEQ5.Minimum = -12;
            this.tbTXEQ5.Name = "tbTXEQ5";
            this.tbTXEQ5.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTXEQ5.Size = new System.Drawing.Size(32, 128);
            this.tbTXEQ5.TabIndex = 115;
            this.tbTXEQ5.TickFrequency = 3;
            this.tbTXEQ5.Scroll += new System.EventHandler(this.tbTXEQ_Scroll);
            this.tbTXEQ5.MouseHover += new System.EventHandler(this.tbTXEQ5_MouseHover);
            // 
            // tbTXEQ6
            // 
            this.tbTXEQ6.AutoSize = false;
            this.tbTXEQ6.LargeChange = 3;
            this.tbTXEQ6.Location = new System.Drawing.Point(288, 199);
            this.tbTXEQ6.Maximum = 15;
            this.tbTXEQ6.Minimum = -12;
            this.tbTXEQ6.Name = "tbTXEQ6";
            this.tbTXEQ6.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTXEQ6.Size = new System.Drawing.Size(32, 128);
            this.tbTXEQ6.TabIndex = 116;
            this.tbTXEQ6.TickFrequency = 3;
            this.tbTXEQ6.Visible = false;
            this.tbTXEQ6.Scroll += new System.EventHandler(this.tbTXEQ_Scroll);
            this.tbTXEQ6.MouseHover += new System.EventHandler(this.tbTXEQ6_MouseHover);
            // 
            // lblTXEQ4
            // 
            this.lblTXEQ4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTXEQ4.Image = null;
            this.lblTXEQ4.Location = new System.Drawing.Point(200, 183);
            this.lblTXEQ4.Name = "lblTXEQ4";
            this.lblTXEQ4.Size = new System.Drawing.Size(40, 16);
            this.lblTXEQ4.TabIndex = 117;
            this.lblTXEQ4.Text = "250";
            this.lblTXEQ4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.lblTXEQ4, "0-400Hz");
            this.lblTXEQ4.Visible = false;
            // 
            // lblTXEQ5
            // 
            this.lblTXEQ5.Image = null;
            this.lblTXEQ5.Location = new System.Drawing.Point(240, 183);
            this.lblTXEQ5.Name = "lblTXEQ5";
            this.lblTXEQ5.Size = new System.Drawing.Size(40, 16);
            this.lblTXEQ5.TabIndex = 118;
            this.lblTXEQ5.Text = "Mid";
            this.lblTXEQ5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.lblTXEQ5, "400-1500Hz");
            // 
            // lblTXEQ6
            // 
            this.lblTXEQ6.Image = null;
            this.lblTXEQ6.Location = new System.Drawing.Point(280, 183);
            this.lblTXEQ6.Name = "lblTXEQ6";
            this.lblTXEQ6.Size = new System.Drawing.Size(40, 16);
            this.lblTXEQ6.TabIndex = 119;
            this.lblTXEQ6.Text = "1K";
            this.lblTXEQ6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.lblTXEQ6, "1500-6000Hz");
            this.lblTXEQ6.Visible = false;
            // 
            // chkTXEQEnabled
            // 
            this.chkTXEQEnabled.Image = null;
            this.chkTXEQEnabled.Location = new System.Drawing.Point(16, 24);
            this.chkTXEQEnabled.Name = "chkTXEQEnabled";
            this.chkTXEQEnabled.Size = new System.Drawing.Size(72, 16);
            this.chkTXEQEnabled.TabIndex = 106;
            this.chkTXEQEnabled.Text = "Enabled";
            this.chkTXEQEnabled.CheckedChanged += new System.EventHandler(this.chkTXEQEnabled_CheckedChanged);
            // 
            // tbTXEQ1
            // 
            this.tbTXEQ1.AutoSize = false;
            this.tbTXEQ1.LargeChange = 3;
            this.tbTXEQ1.Location = new System.Drawing.Point(88, 199);
            this.tbTXEQ1.Maximum = 15;
            this.tbTXEQ1.Minimum = -12;
            this.tbTXEQ1.Name = "tbTXEQ1";
            this.tbTXEQ1.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTXEQ1.Size = new System.Drawing.Size(32, 128);
            this.tbTXEQ1.TabIndex = 4;
            this.tbTXEQ1.TickFrequency = 3;
            this.tbTXEQ1.Scroll += new System.EventHandler(this.tbTXEQ_Scroll);
            this.tbTXEQ1.MouseHover += new System.EventHandler(this.tbTXEQ1_MouseHover);
            // 
            // tbTXEQ2
            // 
            this.tbTXEQ2.AutoSize = false;
            this.tbTXEQ2.LargeChange = 3;
            this.tbTXEQ2.Location = new System.Drawing.Point(128, 199);
            this.tbTXEQ2.Maximum = 15;
            this.tbTXEQ2.Minimum = -12;
            this.tbTXEQ2.Name = "tbTXEQ2";
            this.tbTXEQ2.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTXEQ2.Size = new System.Drawing.Size(32, 128);
            this.tbTXEQ2.TabIndex = 5;
            this.tbTXEQ2.TickFrequency = 3;
            this.tbTXEQ2.Visible = false;
            this.tbTXEQ2.Scroll += new System.EventHandler(this.tbTXEQ_Scroll);
            this.tbTXEQ2.MouseHover += new System.EventHandler(this.tbTXEQ2_MouseHover);
            // 
            // tbTXEQ3
            // 
            this.tbTXEQ3.AutoSize = false;
            this.tbTXEQ3.LargeChange = 3;
            this.tbTXEQ3.Location = new System.Drawing.Point(168, 199);
            this.tbTXEQ3.Maximum = 15;
            this.tbTXEQ3.Minimum = -12;
            this.tbTXEQ3.Name = "tbTXEQ3";
            this.tbTXEQ3.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTXEQ3.Size = new System.Drawing.Size(32, 128);
            this.tbTXEQ3.TabIndex = 6;
            this.tbTXEQ3.TickFrequency = 3;
            this.tbTXEQ3.Visible = false;
            this.tbTXEQ3.Scroll += new System.EventHandler(this.tbTXEQ_Scroll);
            this.tbTXEQ3.MouseHover += new System.EventHandler(this.tbTXEQ3_MouseHover);
            // 
            // lblTXEQ1
            // 
            this.lblTXEQ1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTXEQ1.Image = null;
            this.lblTXEQ1.Location = new System.Drawing.Point(80, 183);
            this.lblTXEQ1.Name = "lblTXEQ1";
            this.lblTXEQ1.Size = new System.Drawing.Size(40, 16);
            this.lblTXEQ1.TabIndex = 74;
            this.lblTXEQ1.Text = "Low";
            this.lblTXEQ1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.lblTXEQ1, "0-400Hz");
            // 
            // lblTXEQ2
            // 
            this.lblTXEQ2.Image = null;
            this.lblTXEQ2.Location = new System.Drawing.Point(120, 183);
            this.lblTXEQ2.Name = "lblTXEQ2";
            this.lblTXEQ2.Size = new System.Drawing.Size(40, 16);
            this.lblTXEQ2.TabIndex = 75;
            this.lblTXEQ2.Text = "63";
            this.lblTXEQ2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.lblTXEQ2, "400-1500Hz");
            this.lblTXEQ2.Visible = false;
            // 
            // lblTXEQ3
            // 
            this.lblTXEQ3.Image = null;
            this.lblTXEQ3.Location = new System.Drawing.Point(160, 183);
            this.lblTXEQ3.Name = "lblTXEQ3";
            this.lblTXEQ3.Size = new System.Drawing.Size(40, 16);
            this.lblTXEQ3.TabIndex = 76;
            this.lblTXEQ3.Text = "125";
            this.lblTXEQ3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.lblTXEQ3, "1500-6000Hz");
            this.lblTXEQ3.Visible = false;
            // 
            // lblTXEQPreamp
            // 
            this.lblTXEQPreamp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTXEQPreamp.Image = null;
            this.lblTXEQPreamp.Location = new System.Drawing.Point(8, 183);
            this.lblTXEQPreamp.Name = "lblTXEQPreamp";
            this.lblTXEQPreamp.Size = new System.Drawing.Size(48, 16);
            this.lblTXEQPreamp.TabIndex = 105;
            this.lblTXEQPreamp.Text = "Preamp";
            this.lblTXEQPreamp.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // tbTXEQPreamp
            // 
            this.tbTXEQPreamp.AutoSize = false;
            this.tbTXEQPreamp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tbTXEQPreamp.LargeChange = 1;
            this.tbTXEQPreamp.Location = new System.Drawing.Point(16, 199);
            this.tbTXEQPreamp.Maximum = 15;
            this.tbTXEQPreamp.Minimum = -12;
            this.tbTXEQPreamp.Name = "tbTXEQPreamp";
            this.tbTXEQPreamp.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTXEQPreamp.Size = new System.Drawing.Size(32, 128);
            this.tbTXEQPreamp.TabIndex = 36;
            this.tbTXEQPreamp.TickFrequency = 3;
            this.toolTip1.SetToolTip(this.tbTXEQPreamp, "0dB");
            this.tbTXEQPreamp.Scroll += new System.EventHandler(this.tbTXEQ_Scroll);
            this.tbTXEQPreamp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tbTXEQPreamp_MouseDown);
            this.tbTXEQPreamp.MouseHover += new System.EventHandler(this.tbTXEQPreamp_MouseHover);
            // 
            // lblTXEQ15db
            // 
            this.lblTXEQ15db.Image = null;
            this.lblTXEQ15db.Location = new System.Drawing.Point(56, 207);
            this.lblTXEQ15db.Name = "lblTXEQ15db";
            this.lblTXEQ15db.Size = new System.Drawing.Size(32, 16);
            this.lblTXEQ15db.TabIndex = 43;
            this.lblTXEQ15db.Text = "15dB";
            this.lblTXEQ15db.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTXEQ0dB
            // 
            this.lblTXEQ0dB.Image = null;
            this.lblTXEQ0dB.Location = new System.Drawing.Point(56, 261);
            this.lblTXEQ0dB.Name = "lblTXEQ0dB";
            this.lblTXEQ0dB.Size = new System.Drawing.Size(32, 16);
            this.lblTXEQ0dB.TabIndex = 0;
            this.lblTXEQ0dB.Text = "  0dB";
            this.lblTXEQ0dB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTXEQminus12db
            // 
            this.lblTXEQminus12db.Image = null;
            this.lblTXEQminus12db.Location = new System.Drawing.Point(52, 303);
            this.lblTXEQminus12db.Name = "lblTXEQminus12db";
            this.lblTXEQminus12db.Size = new System.Drawing.Size(34, 16);
            this.lblTXEQminus12db.TabIndex = 45;
            this.lblTXEQminus12db.Text = "-12dB";
            this.lblTXEQminus12db.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelTS9
            // 
            this.labelTS9.Image = null;
            this.labelTS9.Location = new System.Drawing.Point(89, 167);
            this.labelTS9.Name = "labelTS9";
            this.labelTS9.Size = new System.Drawing.Size(391, 16);
            this.labelTS9.TabIndex = 247;
            this.labelTS9.Text = "Each Band Slider has 1 octave BW (Example: 88 low - 125center  - 176 high)";
            this.labelTS9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkBothEQ
            // 
            this.chkBothEQ.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.chkBothEQ.Image = null;
            this.chkBothEQ.Location = new System.Drawing.Point(520, 10);
            this.chkBothEQ.Name = "chkBothEQ";
            this.chkBothEQ.Size = new System.Drawing.Size(260, 24);
            this.chkBothEQ.TabIndex = 293;
            this.chkBothEQ.Text = "Both TX 28GEQ and TX 9PEQ ON Together";
            this.toolTip1.SetToolTip(this.chkBothEQ, "Both the 28 band Graphic EQ and 9 band Parametric EQ are combined together.\r\n");
            this.chkBothEQ.CheckedChanged += new System.EventHandler(this.chkBothEQ_CheckedChanged);
            // 
            // radPEQ
            // 
            this.radPEQ.Image = null;
            this.radPEQ.Location = new System.Drawing.Point(360, 10);
            this.radPEQ.Name = "radPEQ";
            this.radPEQ.Size = new System.Drawing.Size(164, 24);
            this.radPEQ.TabIndex = 188;
            this.radPEQ.Text = "9-Band TX Parametric EQ";
            this.toolTip1.SetToolTip(this.radPEQ, "10 RX Graphic EQ Bands with 1 octave Band Width each,\r\nAND\r\n10 TX Parametric EQ B" +
        "ands (Select: Freq, Gain, BW)");
            this.radPEQ.Click += new System.EventHandler(this.radPEQ_CheckedChanged);
            // 
            // EQForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ClientSize = new System.Drawing.Size(872, 700);
            this.Controls.Add(this.labelTS18);
            this.Controls.Add(this.chkAlwaysOnTop1);
            this.Controls.Add(this.rad28Band);
            this.Controls.Add(this.rad10Band);
            this.Controls.Add(this.rad3Band);
            this.Controls.Add(this.grpRXEQ);
            this.Controls.Add(this.grpTXEQ);
            this.Controls.Add(this.chkBothEQ);
            this.Controls.Add(this.radPEQ);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(888, 739);
            this.MinimumSize = new System.Drawing.Size(888, 739);
            this.Name = "EQForm";
            this.Text = " Equalizer Settings";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.EQForm_Closing);
            this.MouseEnter += new System.EventHandler(this.EQForm_MouseEnter);
            this.grpRXEQ.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tbRXEQ10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbRXEQ7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbRXEQ8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbRXEQ9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbRXEQ4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbRXEQ5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbRXEQ6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picRXEQ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbRXEQ1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbRXEQ2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbRXEQ3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbRXEQPreamp)).EndInit();
            this.grpTXEQ.ResumeLayout(false);
            this.grpPEQ.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tbTXEQ9Preamp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPEQfreq9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPEQfreq8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPEQfreq7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPEQfreq6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPEQfreq5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPEQfreq4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPEQfreq3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPEQfreq2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPEQfreq1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPEQoctave9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbPEQ9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPEQoctave8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbPEQ8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPEQoctave7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbPEQ7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPEQoctave6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbPEQ6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPEQoctave5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbPEQ5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPEQoctave4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbPEQ4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPEQoctave3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbPEQ3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPEQoctave2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbPEQ2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPEQoctave1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbPEQ1)).EndInit();
            this.grpTXEQ28.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tbTXEQ28Preamp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ28)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ27)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ26)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ25)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ24)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ23)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ22)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ21)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ20)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ19)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ18)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ17)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ16)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ15)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ14)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ13)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ12)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTXEQ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTXEQ10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTXEQ7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTXEQ8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTXEQ9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTXEQ4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTXEQ5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTXEQ6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTXEQ1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTXEQ2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTXEQ3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTXEQPreamp)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

     

        private System.Windows.Forms.GroupBoxTS grpRXEQ;
        private System.Windows.Forms.GroupBoxTS grpTXEQ;
        private System.Windows.Forms.TrackBarTS tbRXEQ1;
        private System.Windows.Forms.TrackBarTS tbRXEQ2;
        private System.Windows.Forms.TrackBarTS tbRXEQ3;
        private System.Windows.Forms.TrackBarTS tbTXEQ3;
        private System.Windows.Forms.TrackBarTS tbTXEQ1;
        private System.Windows.Forms.TrackBarTS tbTXEQ2;
        private System.Windows.Forms.LabelTS lblRXEQ0dB;
        private System.Windows.Forms.LabelTS lblTXEQ0dB;
        private System.Windows.Forms.LabelTS lblRXEQ1;
        private System.Windows.Forms.LabelTS lblRXEQ2;
        private System.Windows.Forms.LabelTS lblRXEQ3;
        private System.Windows.Forms.LabelTS lblTXEQ3;
        private System.Windows.Forms.LabelTS lblTXEQ2;
        private System.Windows.Forms.LabelTS lblTXEQ1;
        private System.Windows.Forms.LabelTS lblRXEQPreamp;
        private System.Windows.Forms.LabelTS lblTXEQPreamp;
        private System.Windows.Forms.CheckBoxTS chkTXEQEnabled;
        private System.Windows.Forms.TrackBarTS tbRXEQPreamp;
        public System.Windows.Forms.TrackBarTS tbTXEQPreamp;
        private System.Windows.Forms.CheckBoxTS chkRXEQEnabled;
        private System.Windows.Forms.PictureBox picRXEQ;
        public System.Windows.Forms.PictureBox picTXEQ;
        private System.Windows.Forms.ButtonTS btnTXEQReset;
        private System.Windows.Forms.ButtonTS btnRXEQReset;
        private System.Windows.Forms.LabelTS lblRXEQ15db;
        private System.Windows.Forms.LabelTS lblTXEQ15db;
        private System.Windows.Forms.LabelTS lblRXEQminus12db;
        private System.Windows.Forms.LabelTS lblTXEQminus12db;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBoxTS chkTXEQ160Notch;
        private System.Windows.Forms.TrackBarTS tbRXEQ4;
        private System.Windows.Forms.TrackBarTS tbRXEQ5;
        private System.Windows.Forms.TrackBarTS tbRXEQ6;
        private System.Windows.Forms.TrackBarTS tbRXEQ7;
        private System.Windows.Forms.TrackBarTS tbRXEQ8;
        private System.Windows.Forms.TrackBarTS tbRXEQ9;
        private System.Windows.Forms.TrackBarTS tbRXEQ10;
        private System.Windows.Forms.LabelTS lblRXEQ15db2;
        private System.Windows.Forms.LabelTS lblRXEQ0dB2;
        private System.Windows.Forms.LabelTS lblRXEQminus12db2;
        private System.Windows.Forms.LabelTS lblTXEQ15db2;
        private System.Windows.Forms.LabelTS lblTXEQ0dB2;
        private System.Windows.Forms.LabelTS lblTXEQminus12db2;
        public System.Windows.Forms.RadioButtonTS rad3Band;
        public System.Windows.Forms.RadioButtonTS rad10Band;
        private System.Windows.Forms.TrackBarTS tbTXEQ4;
        private System.Windows.Forms.TrackBarTS tbTXEQ5;
        private System.Windows.Forms.TrackBarTS tbTXEQ6;
        private System.Windows.Forms.TrackBarTS tbTXEQ7;
        private System.Windows.Forms.TrackBarTS tbTXEQ8;
        private System.Windows.Forms.TrackBarTS tbTXEQ9;
        private System.Windows.Forms.TrackBarTS tbTXEQ10;
        private System.Windows.Forms.LabelTS lblRXEQ4;
        private System.Windows.Forms.LabelTS lblRXEQ5;
        private System.Windows.Forms.LabelTS lblRXEQ6;
        private System.Windows.Forms.LabelTS lblRXEQ7;
        private System.Windows.Forms.LabelTS lblRXEQ8;
        private System.Windows.Forms.LabelTS lblRXEQ9;
        private System.Windows.Forms.LabelTS lblRXEQ10;
        private System.Windows.Forms.LabelTS lblTXEQ4;
        private System.Windows.Forms.LabelTS lblTXEQ5;
        private System.Windows.Forms.LabelTS lblTXEQ6;
        private System.Windows.Forms.LabelTS lblTXEQ7;
        private System.Windows.Forms.LabelTS lblTXEQ8;
        private System.Windows.Forms.LabelTS lblTXEQ9;
        private System.Windows.Forms.LabelTS lblTXEQ10;
        private System.Windows.Forms.GroupBoxTS grpTXEQ28;
        private System.Windows.Forms.LabelTS lbl2821;
        private System.Windows.Forms.LabelTS lbl2828;
        private System.Windows.Forms.LabelTS lbl2827;
        private System.Windows.Forms.TrackBarTS tbTX28EQ28;
        private System.Windows.Forms.TrackBarTS tbTX28EQ27;
        private System.Windows.Forms.LabelTS lbl2826;
        private System.Windows.Forms.TrackBarTS tbTX28EQ26;
        private System.Windows.Forms.TrackBarTS tbTX28EQ25;
        private System.Windows.Forms.LabelTS lbl2825;
        private System.Windows.Forms.LabelTS lbl2824;
        private System.Windows.Forms.LabelTS lbl2823;
        private System.Windows.Forms.TrackBarTS tbTX28EQ24;
        private System.Windows.Forms.TrackBarTS tbTX28EQ23;
        private System.Windows.Forms.LabelTS lbl2822;
        private System.Windows.Forms.TrackBarTS tbTX28EQ22;
        private System.Windows.Forms.LabelTS lbl2820;
        private System.Windows.Forms.TrackBarTS tbTX28EQ21;
        private System.Windows.Forms.TrackBarTS tbTX28EQ20;
        private System.Windows.Forms.LabelTS lbl2819;
        private System.Windows.Forms.TrackBarTS tbTX28EQ19;
        private System.Windows.Forms.TrackBarTS tbTX28EQ18;
        private System.Windows.Forms.LabelTS lbl2818;
        private System.Windows.Forms.LabelTS lbl2817;
        private System.Windows.Forms.LabelTS lbl2816;
        private System.Windows.Forms.TrackBarTS tbTX28EQ17;
        private System.Windows.Forms.TrackBarTS tbTX28EQ16;
        private System.Windows.Forms.LabelTS lbl2815;
        private System.Windows.Forms.TrackBarTS tbTX28EQ15;
        private System.Windows.Forms.LabelTS lbl287;
        private System.Windows.Forms.LabelTS lbl2814;
        private System.Windows.Forms.LabelTS lbl2813;
        private System.Windows.Forms.TrackBarTS tbTX28EQ14;
        private System.Windows.Forms.TrackBarTS tbTX28EQ13;
        private System.Windows.Forms.LabelTS lbl2812;
        private System.Windows.Forms.TrackBarTS tbTX28EQ12;
        private System.Windows.Forms.TrackBarTS tbTX28EQ11;
        private System.Windows.Forms.LabelTS lbl2811;
        private System.Windows.Forms.LabelTS lbl2810;
        private System.Windows.Forms.LabelTS lbl289;
        private System.Windows.Forms.TrackBarTS tbTX28EQ10;
        private System.Windows.Forms.TrackBarTS tbTX28EQ9;
        private System.Windows.Forms.LabelTS lbl288;
        private System.Windows.Forms.TrackBarTS tbTX28EQ8;
        private System.Windows.Forms.LabelTS lbl286;
        private System.Windows.Forms.TrackBarTS tbTX28EQ7;
        private System.Windows.Forms.TrackBarTS tbTX28EQ6;
        private System.Windows.Forms.LabelTS lbl285;
        private System.Windows.Forms.TrackBarTS tbTX28EQ5;
        private System.Windows.Forms.TrackBarTS tbTX28EQ4;
        private System.Windows.Forms.LabelTS lbl284;
        private System.Windows.Forms.LabelTS lbl283;
        private System.Windows.Forms.LabelTS lbl282;
        private System.Windows.Forms.TrackBarTS tbTX28EQ3;
        private System.Windows.Forms.TrackBarTS tbTX28EQ2;
        private System.Windows.Forms.LabelTS lbl281;
        private System.Windows.Forms.TrackBarTS tbTX28EQ1;
        public System.Windows.Forms.RadioButtonTS rad28Band;
        private System.Windows.Forms.ButtonTS btnTXEQReset28;
        private System.Windows.Forms.LabelTS labelTS3;
        private System.Windows.Forms.LabelTS labelTS2;
        private System.Windows.Forms.LabelTS labelTS1;
        private System.Windows.Forms.LabelTS labelTS6;
        private System.Windows.Forms.LabelTS labelTS5;
        private System.Windows.Forms.LabelTS labelTS4;
        public System.Windows.Forms.CheckBoxTS chkAlwaysOnTop1;
        private System.Windows.Forms.LabelTS labelTS7;
        private System.Windows.Forms.LabelTS labelTS9;
        private System.Windows.Forms.LabelTS labelTS8;
        private System.Windows.Forms.GroupBoxTS grpPEQ;
        private System.Windows.Forms.TrackBarTS tbPEQ1;
        private System.Windows.Forms.LabelTS labelTS11;
        private System.Windows.Forms.LabelTS labelTS12;
        private System.Windows.Forms.LabelTS labelTS13;
        private System.Windows.Forms.LabelTS labelTS14;
        private System.Windows.Forms.ButtonTS btnTXPEQReset;
        private System.Windows.Forms.LabelTS labelTS43;
        private System.Windows.Forms.LabelTS labelTS44;
        public System.Windows.Forms.RadioButtonTS radPEQ;
        private System.Windows.Forms.LabelTS labelTS15;
        public System.Windows.Forms.NumericUpDownTS udPEQoctave1;
        private System.Windows.Forms.LabelTS labelTS16;
        public System.Windows.Forms.NumericUpDownTS udPEQoctave3;
        private System.Windows.Forms.TrackBarTS tbPEQ3;
        public System.Windows.Forms.NumericUpDownTS udPEQoctave2;
        private System.Windows.Forms.TrackBarTS tbPEQ2;
        public System.Windows.Forms.NumericUpDownTS udPEQoctave9;
        private System.Windows.Forms.TrackBarTS tbPEQ9;
        public System.Windows.Forms.NumericUpDownTS udPEQoctave8;
        private System.Windows.Forms.TrackBarTS tbPEQ8;
        public System.Windows.Forms.NumericUpDownTS udPEQoctave7;
        private System.Windows.Forms.TrackBarTS tbPEQ7;
        public System.Windows.Forms.NumericUpDownTS udPEQoctave6;
        private System.Windows.Forms.TrackBarTS tbPEQ6;
        public System.Windows.Forms.NumericUpDownTS udPEQoctave5;
        private System.Windows.Forms.TrackBarTS tbPEQ5;
        public System.Windows.Forms.NumericUpDownTS udPEQoctave4;
        private System.Windows.Forms.TrackBarTS tbPEQ4;
        public System.Windows.Forms.NumericUpDownTS udPEQfreq9;
        public System.Windows.Forms.NumericUpDownTS udPEQfreq8;
        public System.Windows.Forms.NumericUpDownTS udPEQfreq7;
        public System.Windows.Forms.NumericUpDownTS udPEQfreq6;
        public System.Windows.Forms.NumericUpDownTS udPEQfreq5;
        public System.Windows.Forms.NumericUpDownTS udPEQfreq4;
        public System.Windows.Forms.NumericUpDownTS udPEQfreq3;
        public System.Windows.Forms.NumericUpDownTS udPEQfreq2;
        public System.Windows.Forms.NumericUpDownTS udPEQfreq1;
        public System.Windows.Forms.TrackBarTS tbTXEQ9Preamp;
        private System.Windows.Forms.LabelTS labelTS10;
        private System.Windows.Forms.LabelTS labelTS17;
        public System.Windows.Forms.TrackBarTS tbTXEQ28Preamp;
        private System.Windows.Forms.LabelTS labelTS18;
        public System.Windows.Forms.CheckBoxTS chkBothEQ;
        private System.ComponentModel.IContainer components;


    } // EQForm
} // PoweSDR