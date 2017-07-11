//=================================================================
// alphawarnform.cs
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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Resources;
using System.Windows.Forms;

namespace PowerSDR
{
	public class AlphaWarnForm : System.Windows.Forms.Form
	{
		private Console console;
		private System.Windows.Forms.RichTextBox rtxtWarning;
		private System.Windows.Forms.CheckBox chkShowThisOnStartup;
		private System.Windows.Forms.Button btnContinue;		
		private System.ComponentModel.Container components = null;

		public AlphaWarnForm(Console c)
		{
			InitializeComponent();
			console = c;
			this.ActiveControl = btnContinue;
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
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AlphaWarnForm));
            this.rtxtWarning = new System.Windows.Forms.RichTextBox();
            this.chkShowThisOnStartup = new System.Windows.Forms.CheckBox();
            this.btnContinue = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rtxtWarning
            // 
            this.rtxtWarning.BackColor = System.Drawing.SystemColors.ControlText;
            this.rtxtWarning.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtxtWarning.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.rtxtWarning.Location = new System.Drawing.Point(16, 16);
            this.rtxtWarning.Name = "rtxtWarning";
            this.rtxtWarning.ReadOnly = true;
            this.rtxtWarning.Size = new System.Drawing.Size(384, 216);
            this.rtxtWarning.TabIndex = 1;
            this.rtxtWarning.Text = resources.GetString("rtxtWarning.Text");
            this.rtxtWarning.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.rtxtWarning_LinkClicked);
            // 
            // chkShowThisOnStartup
            // 
            this.chkShowThisOnStartup.BackColor = System.Drawing.Color.Black;
            this.chkShowThisOnStartup.Checked = true;
            this.chkShowThisOnStartup.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowThisOnStartup.ForeColor = System.Drawing.Color.White;
            this.chkShowThisOnStartup.Location = new System.Drawing.Point(16, 240);
            this.chkShowThisOnStartup.Name = "chkShowThisOnStartup";
            this.chkShowThisOnStartup.Size = new System.Drawing.Size(168, 24);
            this.chkShowThisOnStartup.TabIndex = 2;
            this.chkShowThisOnStartup.Text = "Show this warning on startup";
            this.chkShowThisOnStartup.UseVisualStyleBackColor = false;
            // 
            // btnContinue
            // 
            this.btnContinue.BackColor = System.Drawing.SystemColors.Control;
            this.btnContinue.Location = new System.Drawing.Point(256, 240);
            this.btnContinue.Name = "btnContinue";
            this.btnContinue.Size = new System.Drawing.Size(75, 23);
            this.btnContinue.TabIndex = 3;
            this.btnContinue.Text = "Continue";
            this.btnContinue.UseVisualStyleBackColor = false;
            this.btnContinue.Click += new System.EventHandler(this.btnContinue_Click);
            // 
            // AlphaWarnForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.SystemColors.ControlText;
            this.ClientSize = new System.Drawing.Size(416, 268);
            this.Controls.Add(this.btnContinue);
            this.Controls.Add(this.chkShowThisOnStartup);
            this.Controls.Add(this.rtxtWarning);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AlphaWarnForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Alpha Warning";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.AlphaWarnForm_Closing);
            this.ResumeLayout(false);

		}
		#endregion

		private Stream GetResource(string name)
		{
			return this.GetType().Assembly.GetManifestResourceStream(name);
		}

		private void btnContinue_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void rtxtWarning_LinkClicked(object sender, System.Windows.Forms.LinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start(e.LinkText); 
		}

		private void AlphaWarnForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			this.Hide();
			if(!chkShowThisOnStartup.Checked)
			{
				ArrayList a = new ArrayList();
				a.Add("show_alpha_warning/False");
				DB.SaveVars("State", ref a);
			}
		}
	}
}
