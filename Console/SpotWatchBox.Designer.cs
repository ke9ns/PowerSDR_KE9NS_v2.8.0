//=================================================================
//  SpotWatchBox.cs
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
    public partial class  SpotWatchBox : System.Windows.Forms.Form
    {


        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SpotWatchBox));
            this.btnTrack = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.chkAlwaysOnTop = new System.Windows.Forms.CheckBoxTS();
            this.SuspendLayout();
            // 
            // btnTrack
            // 
            this.btnTrack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnTrack.BackColor = System.Drawing.Color.Yellow;
            this.btnTrack.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTrack.ForeColor = System.Drawing.Color.Black;
            this.btnTrack.Location = new System.Drawing.Point(44, 48);
            this.btnTrack.Name = "btnTrack";
            this.btnTrack.Size = new System.Drawing.Size(279, 79);
            this.btnTrack.TabIndex = 99;
            this.btnTrack.Text = "Not Yet";
            this.toolTip1.SetToolTip(this.btnTrack, "Left Click: Set VFOA to this Spot\r\n\r\nWheel Click: Set VFOB to this Spot. ");
            this.btnTrack.UseVisualStyleBackColor = false;
            this.btnTrack.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnTrack_MouseDown);
            // 
            // chkAlwaysOnTop
            // 
            this.chkAlwaysOnTop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkAlwaysOnTop.Checked = true;
            this.chkAlwaysOnTop.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAlwaysOnTop.Image = null;
            this.chkAlwaysOnTop.Location = new System.Drawing.Point(252, 145);
            this.chkAlwaysOnTop.Name = "chkAlwaysOnTop";
            this.chkAlwaysOnTop.Size = new System.Drawing.Size(103, 31);
            this.chkAlwaysOnTop.TabIndex = 100;
            this.chkAlwaysOnTop.Text = "Always On Top";
            this.chkAlwaysOnTop.Visible = false;
            this.chkAlwaysOnTop.CheckedChanged += new System.EventHandler(this.chkAlwaysOnTop_CheckedChanged);
            // 
            // SpotWatchBox
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.Crimson;
            this.ClientSize = new System.Drawing.Size(367, 188);
            this.Controls.Add(this.btnTrack);
            this.Controls.Add(this.chkAlwaysOnTop);
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(383, 227);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(383, 227);
            this.Name = "SpotWatchBox";
            this.Text = "DX Watch Match";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.SpotWatchBox_Closing);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SpotWatchBox_FormClosing);
            this.Load += new System.EventHandler(this.SpotWatchBox_Load);
            this.VisibleChanged += new System.EventHandler(this.SpotWatchBox_VisibleChanged);
            this.ResumeLayout(false);

        }
        #endregion


       
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.CheckBoxTS chkAlwaysOnTop;
        private System.Windows.Forms.ToolTip toolTip1;
        public System.Windows.Forms.Button btnTrack;

      

    } //  SpotWatchBox

} // PowerSDR
