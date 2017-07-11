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

using System;
using System.Diagnostics;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;

namespace PowerSDR
{
	/// <summary>
	/// Summary description for WaveOptions.
	/// </summary>
	public class helpbox : System.Windows.Forms.Form
	{

        public static Console console;   // ke9ns mod  to allow console to pass back values to setup screen


        #region Variable Declaration
        private System.ComponentModel.IContainer components;
        private CheckBoxTS chkAlwaysOnTop;
        public RichTextBox txttimer_message;
        public RichTextBox helpbox_message;
        public RichTextBox solar_message;
        public RichTextBox recplay_message;
        public RichTextBox PropagationTextBox;
        public RichTextBox TRACKMap;
        private ToolTip toolTip1;

        // public static helpbox helpboxForm;                       // ke9ns add 

        #endregion

        #region Constructor and Destructor

        public helpbox(Console c)  // called the very first time
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
           // console = c;
            Common.RestoreForm(this, "helpbox", false);

            this.TopMost = true;

           
        }
        
          /// <summary>
          /// Clean up any resources being used.
          /// </summary>
        protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );

            console.TIMETOID = false;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(helpbox));
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.chkAlwaysOnTop = new System.Windows.Forms.CheckBoxTS();
            this.txttimer_message = new System.Windows.Forms.RichTextBox();
            this.helpbox_message = new System.Windows.Forms.RichTextBox();
            this.solar_message = new System.Windows.Forms.RichTextBox();
            this.recplay_message = new System.Windows.Forms.RichTextBox();
            this.PropagationTextBox = new System.Windows.Forms.RichTextBox();
            this.TRACKMap = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // chkAlwaysOnTop
            // 
            this.chkAlwaysOnTop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkAlwaysOnTop.Checked = true;
            this.chkAlwaysOnTop.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAlwaysOnTop.Image = null;
            this.chkAlwaysOnTop.Location = new System.Drawing.Point(545, 370);
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
            this.helpbox_message.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.helpbox_message.Location = new System.Drawing.Point(12, 12);
            this.helpbox_message.Name = "helpbox_message";
            this.helpbox_message.Size = new System.Drawing.Size(636, 389);
            this.helpbox_message.TabIndex = 102;
            this.helpbox_message.Text = "helpbox";
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
            // helpbox
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.ClientSize = new System.Drawing.Size(660, 413);
            this.Controls.Add(this.helpbox_message);
            this.Controls.Add(this.chkAlwaysOnTop);
            this.Controls.Add(this.txttimer_message);
            this.Controls.Add(this.solar_message);
            this.Controls.Add(this.recplay_message);
            this.Controls.Add(this.PropagationTextBox);
            this.Controls.Add(this.TRACKMap);
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(676, 452);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(676, 452);
            this.Name = "helpbox";
            this.Text = "PowerSDR F1 Help";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.helpbox_Closing);
            this.ResumeLayout(false);

		}
		#endregion

		#region Properties

		


       

		#endregion

		#region Event Handler

		private void helpbox_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
            
            this.Hide();
			e.Cancel = true;
			Common.SaveForm(this, "helpbox");
		}



        #endregion

      
      

        private void chkAlwaysOnTop_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = true;
        }

        private void helpbox_message_TextChanged(object sender, EventArgs e)
        {

        }
    } // helpbox

} // PowerSDR
