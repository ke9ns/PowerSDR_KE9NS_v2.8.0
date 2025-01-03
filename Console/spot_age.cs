//=================================================================
// spot_options.cs
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
using System.Windows.Forms;

namespace PowerSDR
{
    /// <summary>
    /// Summary description for WaveOptions.
    /// </summary>
    public class SpotAge : System.Windows.Forms.Form
    {
        #region Variable Declaration
        private System.Windows.Forms.ToolTip toolTip1;
        public NumericUpDownTS udSpotAge;
        private Label label5;
        private System.ComponentModel.IContainer components;
        public CheckBoxTS chkPanSpotBlank;
        public CheckBoxTS chkPanLoTWColor;
        public CheckBoxTS chkPanNoVert;
        public static SpotControl SpotForm;                       // ke9ns add DX spotter function

        #endregion

        #region Constructor and Destructor

        public SpotAge()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            Common.RestoreForm(this, "SpotAge", false);

            this.TopMost = true; //.262
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SpotAge));
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.udSpotAge = new System.Windows.Forms.NumericUpDownTS();
            this.chkPanSpotBlank = new System.Windows.Forms.CheckBoxTS();
            this.chkPanLoTWColor = new System.Windows.Forms.CheckBoxTS();
            this.chkPanNoVert = new System.Windows.Forms.CheckBoxTS();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.udSpotAge)).BeginInit();
            this.SuspendLayout();
            // 
            // udSpotAge
            // 
            this.udSpotAge.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udSpotAge.Location = new System.Drawing.Point(175, 12);
            this.udSpotAge.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
            this.udSpotAge.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.udSpotAge.Name = "udSpotAge";
            this.udSpotAge.Size = new System.Drawing.Size(46, 20);
            this.udSpotAge.TabIndex = 80;
            this.toolTip1.SetToolTip(this.udSpotAge, "Maxmimum Spot Age (in minutes) before being removed from the Spot List\r\n");
            this.udSpotAge.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.udSpotAge.ValueChanged += new System.EventHandler(this.udMethod_ValueChanged);
            // 
            // chkPanSpotBlank
            // 
            this.chkPanSpotBlank.AutoSize = true;
            this.chkPanSpotBlank.Checked = true;
            this.chkPanSpotBlank.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPanSpotBlank.Image = null;
            this.chkPanSpotBlank.Location = new System.Drawing.Point(15, 46);
            this.chkPanSpotBlank.Name = "chkPanSpotBlank";
            this.chkPanSpotBlank.Size = new System.Drawing.Size(256, 17);
            this.chkPanSpotBlank.TabIndex = 87;
            this.chkPanSpotBlank.Text = "Panadapter spot callsigns with Dark Background";
            this.toolTip1.SetToolTip(this.chkPanSpotBlank, "Check to put DX Callsign spot on the Panadater in front of a Dark background to m" +
        "ake them easier to see on a crowded band.");
            this.chkPanSpotBlank.UseVisualStyleBackColor = true;
            this.chkPanSpotBlank.CheckedChanged += new System.EventHandler(this.chkPanSpotBlank_CheckedChanged_1);
            // 
            // chkPanLoTWColor
            // 
            this.chkPanLoTWColor.AutoSize = true;
            this.chkPanLoTWColor.Checked = true;
            this.chkPanLoTWColor.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPanLoTWColor.Image = null;
            this.chkPanLoTWColor.Location = new System.Drawing.Point(15, 78);
            this.chkPanLoTWColor.Name = "chkPanLoTWColor";
            this.chkPanLoTWColor.Size = new System.Drawing.Size(229, 17);
            this.chkPanLoTWColor.TabIndex = 88;
            this.chkPanLoTWColor.Text = "Panadapter spot callsigns with LoTW Color\r\n";
            this.toolTip1.SetToolTip(this.chkPanLoTWColor, "Check to put DX Callsign spot on the Panadater in front of a Dark background to m" +
        "ake them easier to see on a crowded band.");
            this.chkPanLoTWColor.UseVisualStyleBackColor = true;
            this.chkPanLoTWColor.CheckedChanged += new System.EventHandler(this.chkPanLoTWColor_CheckedChanged);
            // 
            // chkPanNoVert
            // 
            this.chkPanNoVert.AutoSize = true;
            this.chkPanNoVert.Image = null;
            this.chkPanNoVert.Location = new System.Drawing.Point(15, 119);
            this.chkPanNoVert.Name = "chkPanNoVert";
            this.chkPanNoVert.Size = new System.Drawing.Size(268, 17);
            this.chkPanNoVert.TabIndex = 89;
            this.chkPanNoVert.Text = "Panadapter spot callsigns with NO vertical indicator\r\n";
            this.toolTip1.SetToolTip(this.chkPanNoVert, "Check to remove all vertical lines indicating the VFO frequency location.");
            this.chkPanNoVert.UseVisualStyleBackColor = true;
            this.chkPanNoVert.CheckedChanged += new System.EventHandler(this.chkPanNoVert_CheckedChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.label5.Location = new System.Drawing.Point(12, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(144, 13);
            this.label5.TabIndex = 85;
            this.label5.Text = "Maximum Spot Age (Minutes)";
            // 
            // SpotAge
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(367, 204);
            this.Controls.Add(this.chkPanNoVert);
            this.Controls.Add(this.chkPanLoTWColor);
            this.Controls.Add(this.chkPanSpotBlank);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.udSpotAge);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SpotAge";
            this.Text = "Spotter Settings";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.SpotAge_Closing);
            ((System.ComponentModel.ISupportInitialize)(this.udSpotAge)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        #region Properties






        #endregion

        #region Event Handler

        private void SpotAge_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SpotForm.VOACAP_FORCE = false;
            this.Hide();
            e.Cancel = true;
            Common.SaveForm(this, "SpotAge");
        }



        #endregion

        private void udMethod_ValueChanged(object sender, EventArgs e)
        {
            //  SpotForm.VOACAP_FORCE = true;
            //   SpotForm.VOACAP_CHECK();
        }

        private void udSSN_ValueChanged(object sender, EventArgs e)
        {
            // SpotForm.VOACAP_FORCE = true;
            //  SpotForm.VOACAP_CHECK();
        }

        private void udMTA_ValueChanged(object sender, EventArgs e)
        {
            //  SpotForm.VOACAP_FORCE = true;
            //   SpotForm.VOACAP_CHECK();
        }

        private void udRCR_ValueChanged(object sender, EventArgs e)
        {
            //  SpotForm.VOACAP_FORCE = true;
            //  SpotForm.VOACAP_CHECK();
        }

        private void udSNR_ValueChanged(object sender, EventArgs e)
        {
            //  SpotForm.VOACAP_FORCE = true;
            //  SpotForm.VOACAP_CHECK();
        }

        private void udWATTS_ValueChanged(object sender, EventArgs e)
        {
            //  SpotForm.VOACAP_FORCE = true;
            //  SpotForm.VOACAP_CHECK();

        }

        private void numericUpDownTS1_ValueChanged(object sender, EventArgs e)
        {
            //   SpotForm.VOACAP_FORCE = true;
            //  SpotForm.VOACAP_CHECK();
        }

        private void btnTrack_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("OPTIONS UPDATE HERE0");

            SpotForm.VOACAP_FORCE = true;

            Debug.WriteLine("OPTIONS UPDATE HERE");

            SpotForm.VOACAP_CHECK();
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }
       

        private void chkPanSpotBlank_CheckedChanged_1(object sender, EventArgs e) //.261
        {
            if (chkPanSpotBlank.Checked) SpotForm.SpotBackground = true;
            else SpotForm.SpotBackground = false;
        }

        private void chkPanLoTWColor_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPanLoTWColor.Checked) SpotForm.SpotLoTWColor = true;
            else SpotForm.SpotLoTWColor = false;
        }

        private void chkPanNoVert_CheckedChanged(object sender, EventArgs e) //.269
        {
            if (chkPanNoVert.Checked) SpotForm.SpotNoVert = true;
            else SpotForm.SpotNoVert = false;
        }

      

    } // SpotAge

} // PowerSDR
