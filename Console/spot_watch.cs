//=================================================================
// spot_watch.cs
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
    public class SpotWatch : System.Windows.Forms.Form
    {
        #region Variable Declaration
        private System.Windows.Forms.ToolTip toolTip1;
        private System.ComponentModel.IContainer components;
        public TextBox watchBox;
        private Label label1;
        public static SpotControl SpotForm;                       // ke9ns add DX spotter function

        #endregion

        #region Constructor and Destructor

        public SpotWatch()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            Common.RestoreForm(this, "SpotWatch", false);

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SpotWatch));
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.watchBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // watchBox
            // 
            this.watchBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.watchBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.watchBox.Location = new System.Drawing.Point(99, 98);
            this.watchBox.MaxLength = 20;
            this.watchBox.Name = "watchBox";
            this.watchBox.Size = new System.Drawing.Size(156, 22);
            this.watchBox.TabIndex = 90;
            this.watchBox.Text = "call sign";
            this.toolTip1.SetToolTip(this.watchBox, "Type anything you want to Watch for from a DX Spot.\r\n\r\nType a DX Spot call sign, " +
        "a Spotter call sign\r\nType a partial call sign to watch for a Country\r\nType a par" +
        "tial message to watch for a message");
            this.watchBox.TextChanged += new System.EventHandler(this.watchBox_TextChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(96, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 13);
            this.label1.TabIndex = 91;
            this.label1.Text = "Watch for DX Spot:";
            // 
            // SpotWatch
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(367, 246);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.watchBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SpotWatch";
            this.Text = "Spotter Settings";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.SpotWatch_Closing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        #region Properties






        #endregion

        #region Event Handler

        private void SpotWatch_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
          
            this.Hide();
            e.Cancel = true;
            Common.SaveForm(this, "SpotWatch");
        }



        #endregion

       
      
      


        private void watchBox_TextChanged(object sender, EventArgs e) //.278 moved from SpotAge to new SpotWatch
        {
            if (watchBox.TextLength >= 2 || watchBox.Text != " ")
            {
                SpotForm.SpotWatchCall = watchBox.Text; //.269
            }
        }


    } // SpotWatch

} // PowerSDR
