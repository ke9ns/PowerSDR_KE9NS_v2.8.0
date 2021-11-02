//=================================================================
// helpbox.cs
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
    /// Summary description for WaveOptions.
    /// </summary>
    public partial class helpbox : System.Windows.Forms.Form
    {


        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(helpbox));
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.chkAlwaysOnTop = new System.Windows.Forms.CheckBoxTS();
            this.txttimer_message = new System.Windows.Forms.RichTextBox();
            this.helpbox_message = new System.Windows.Forms.RichTextBox();
            this.solar_message = new System.Windows.Forms.RichTextBox();
            this.recplay_message = new System.Windows.Forms.RichTextBox();
            this.PropagationTextBox = new System.Windows.Forms.RichTextBox();
            this.TRACKMap = new System.Windows.Forms.RichTextBox();
            this.SWRScanner = new System.Windows.Forms.RichTextBox();
            this.AntennaDelay = new System.Windows.Forms.RichTextBox();
            this.HTTPSERVER = new System.Windows.Forms.RichTextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.LoTW_help = new System.Windows.Forms.RichTextBox();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // chkAlwaysOnTop
            // 
            this.chkAlwaysOnTop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkAlwaysOnTop.Checked = true;
            this.chkAlwaysOnTop.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAlwaysOnTop.Image = null;
            this.chkAlwaysOnTop.Location = new System.Drawing.Point(545, -58);
            this.chkAlwaysOnTop.MaximumSize = new System.Drawing.Size(103, 31);
            this.chkAlwaysOnTop.MinimumSize = new System.Drawing.Size(103, 31);
            this.chkAlwaysOnTop.Name = "chkAlwaysOnTop";
            this.chkAlwaysOnTop.Size = new System.Drawing.Size(103, 31);
            this.chkAlwaysOnTop.TabIndex = 100;
            this.chkAlwaysOnTop.Text = "Always On Top";
            this.chkAlwaysOnTop.Visible = false;
            this.chkAlwaysOnTop.CheckedChanged += new System.EventHandler(this.chkAlwaysOnTop_CheckedChanged);
            // 
            // txttimer_message
            // 
            this.txttimer_message.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txttimer_message.Location = new System.Drawing.Point(12, 12);
            this.txttimer_message.Name = "txttimer_message";
            this.txttimer_message.Size = new System.Drawing.Size(636, 389);
            this.txttimer_message.TabIndex = 101;
            this.txttimer_message.Text = resources.GetString("txttimer_message.Text");
            // 
            // helpbox_message
            // 
            this.helpbox_message.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.helpbox_message.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.helpbox_message.Location = new System.Drawing.Point(12, 12);
            this.helpbox_message.Name = "helpbox_message";
            this.helpbox_message.Size = new System.Drawing.Size(634, 389);
            this.helpbox_message.TabIndex = 102;
            this.helpbox_message.Text = "";
            this.helpbox_message.TextChanged += new System.EventHandler(this.helpbox_message_TextChanged);
            // 
            // solar_message
            // 
            this.solar_message.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.solar_message.Location = new System.Drawing.Point(12, 12);
            this.solar_message.Name = "solar_message";
            this.solar_message.Size = new System.Drawing.Size(636, 389);
            this.solar_message.TabIndex = 103;
            this.solar_message.Text = resources.GetString("solar_message.Text");
            // 
            // recplay_message
            // 
            this.recplay_message.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.recplay_message.Location = new System.Drawing.Point(12, 12);
            this.recplay_message.Name = "recplay_message";
            this.recplay_message.Size = new System.Drawing.Size(636, 389);
            this.recplay_message.TabIndex = 104;
            this.recplay_message.Text = "recplay";
            // 
            // PropagationTextBox
            // 
            this.PropagationTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PropagationTextBox.Location = new System.Drawing.Point(12, 12);
            this.PropagationTextBox.Name = "PropagationTextBox";
            this.PropagationTextBox.Size = new System.Drawing.Size(636, 389);
            this.PropagationTextBox.TabIndex = 105;
            this.PropagationTextBox.Text = resources.GetString("PropagationTextBox.Text");
            // 
            // TRACKMap
            // 
            this.TRACKMap.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TRACKMap.Location = new System.Drawing.Point(12, 12);
            this.TRACKMap.Name = "TRACKMap";
            this.TRACKMap.Size = new System.Drawing.Size(636, 389);
            this.TRACKMap.TabIndex = 106;
            this.TRACKMap.Text = resources.GetString("TRACKMap.Text");
            // 
            // SWRScanner
            // 
            this.SWRScanner.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SWRScanner.Location = new System.Drawing.Point(12, 12);
            this.SWRScanner.Name = "SWRScanner";
            this.SWRScanner.Size = new System.Drawing.Size(636, 389);
            this.SWRScanner.TabIndex = 107;
            this.SWRScanner.Text = resources.GetString("SWRScanner.Text");
            // 
            // AntennaDelay
            // 
            this.AntennaDelay.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AntennaDelay.Location = new System.Drawing.Point(12, 12);
            this.AntennaDelay.Name = "AntennaDelay";
            this.AntennaDelay.Size = new System.Drawing.Size(636, 389);
            this.AntennaDelay.TabIndex = 108;
            this.AntennaDelay.Text = resources.GetString("AntennaDelay.Text");
            // 
            // HTTPSERVER
            // 
            this.HTTPSERVER.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HTTPSERVER.Location = new System.Drawing.Point(12, 12);
            this.HTTPSERVER.Name = "HTTPSERVER";
            this.HTTPSERVER.Size = new System.Drawing.Size(636, 389);
            this.HTTPSERVER.TabIndex = 109;
            this.HTTPSERVER.Text = resources.GetString("HTTPSERVER.Text");
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(12, 417);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(636, 412);
            this.groupBox1.TabIndex = 110;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Help Video";
            this.groupBox1.Visible = false;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.button2.Location = new System.Drawing.Point(666, 28);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(126, 37);
            this.button2.TabIndex = 111;
            this.button2.Text = "FireWire Issues";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Visible = false;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.button1.Location = new System.Drawing.Point(666, 71);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(126, 37);
            this.button1.TabIndex = 112;
            this.button1.Text = "SWR Plotter";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Visible = false;
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.button3.Location = new System.Drawing.Point(666, 114);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(126, 37);
            this.button3.TabIndex = 113;
            this.button3.Text = "Combo Meter, Local weather, WSJT-X";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Visible = false;
            // 
            // LoTW_help
            // 
            this.LoTW_help.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LoTW_help.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LoTW_help.Location = new System.Drawing.Point(13, 12);
            this.LoTW_help.Name = "LoTW_help";
            this.LoTW_help.Size = new System.Drawing.Size(634, 389);
            this.LoTW_help.TabIndex = 114;
            this.LoTW_help.Text = resources.GetString("LoTW_help.Text");
            // 
            // webBrowser1
            // 
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1.Location = new System.Drawing.Point(0, 0);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(635, 394);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.ScrollBarsEnabled = false;
            this.webBrowser1.Size = new System.Drawing.Size(660, 413);
            this.webBrowser1.TabIndex = 115;
            this.webBrowser1.Visible = false;
            // 
            // helpbox
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.ClientSize = new System.Drawing.Size(660, 413);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.helpbox_message);
            this.Controls.Add(this.chkAlwaysOnTop);
            this.Controls.Add(this.txttimer_message);
            this.Controls.Add(this.solar_message);
            this.Controls.Add(this.recplay_message);
            this.Controls.Add(this.PropagationTextBox);
            this.Controls.Add(this.TRACKMap);
            this.Controls.Add(this.SWRScanner);
            this.Controls.Add(this.AntennaDelay);
            this.Controls.Add(this.HTTPSERVER);
            this.Controls.Add(this.LoTW_help);
            this.Controls.Add(this.webBrowser1);
            this.ForeColor = System.Drawing.Color.Black;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1200, 1200);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(676, 452);
            this.Name = "helpbox";
            this.Text = "PowerSDR F1 Text Help (F2 Video Help)";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.helpbox_Closing);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.helpbox_FormClosing);
            this.ResumeLayout(false);

        }
        #endregion

        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.CheckBoxTS chkAlwaysOnTop;
        public System.Windows.Forms.RichTextBox txttimer_message;
        public System.Windows.Forms.RichTextBox helpbox_message;
        public System.Windows.Forms.RichTextBox solar_message;
        public System.Windows.Forms.RichTextBox recplay_message;
        public System.Windows.Forms.RichTextBox PropagationTextBox;
        public System.Windows.Forms.RichTextBox TRACKMap;
        public System.Windows.Forms.RichTextBox SWRScanner;
        public System.Windows.Forms.RichTextBox AntennaDelay;
        public System.Windows.Forms.RichTextBox HTTPSERVER;
        public System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.Button button2;
        public System.Windows.Forms.Button button1;
        public System.Windows.Forms.Button button3;
        public System.Windows.Forms.RichTextBox LoTW_help;
        public System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.ToolTip toolTip1;

       

    } // helpbox

} // PowerSDR
