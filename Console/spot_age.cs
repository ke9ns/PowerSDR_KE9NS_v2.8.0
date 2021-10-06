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
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.udSpotAge)).BeginInit();
            this.SuspendLayout();
            // 
            // udSpotAge
            // 
            this.udSpotAge.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.udSpotAge.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udSpotAge.Location = new System.Drawing.Point(173, 6);
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
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 13);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(144, 13);
            this.label5.TabIndex = 85;
            this.label5.Text = "Maximum Spot Age (Minutes)";
            // 
            // SpotAge
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(367, 246);
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
    } // SpotAge

} // PowerSDR
