//=================================================================
// fwcTestForm.cs
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
using System.Threading;
using System.Windows.Forms;

namespace PowerSDR
{
	public class FWCTestForm : System.Windows.Forms.Form
	{
		#region Variable Declaration

		private Console console;
		private System.Windows.Forms.TextBox txtVolts;
        private System.Windows.Forms.TextBox txtTemp;
        private System.Windows.Forms.TextBox txtInfoText;
		private System.ComponentModel.Container components = null;

		#endregion

		#region Constructor and Destructor

		public FWCTestForm(Console c)
		{
			InitializeComponent();
			console = c;

			Thread t = new Thread(new ThreadStart(PollADC));
			t.Name = "Poll ADC Thread";
			t.IsBackground = true;
			t.Priority = ThreadPriority.BelowNormal;
			t.Start();
		}

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

		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FWCTestForm));
            this.txtVolts = new System.Windows.Forms.TextBox();
            this.txtTemp = new System.Windows.Forms.TextBox();
            this.txtInfoText = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtVolts
            // 
            this.txtVolts.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVolts.Location = new System.Drawing.Point(8, 8);
            this.txtVolts.Name = "txtVolts";
            this.txtVolts.ReadOnly = true;
            this.txtVolts.Size = new System.Drawing.Size(149, 26);
            this.txtVolts.TabIndex = 1;
            this.txtVolts.Text = "Voltage: 13.8";
            this.txtVolts.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtTemp
            // 
            this.txtTemp.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTemp.Location = new System.Drawing.Point(8, 40);
            this.txtTemp.Name = "txtTemp";
            this.txtTemp.ReadOnly = true;
            this.txtTemp.Size = new System.Drawing.Size(149, 26);
            this.txtTemp.TabIndex = 2;
            this.txtTemp.Text = "Temp: 26° C";
            this.txtTemp.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtTemp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txtTemp_MouseDown);
            // 
            // txtInfoText
            // 
            this.txtInfoText.Location = new System.Drawing.Point(8, 72);
            this.txtInfoText.Multiline = true;
            this.txtInfoText.Name = "txtInfoText";
            this.txtInfoText.Size = new System.Drawing.Size(149, 47);
            this.txtInfoText.TabIndex = 3;
            this.txtInfoText.Text = "Parameter exceeds recommended operating threshold if background is red";
            this.txtInfoText.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // FWCTestForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(167, 128);
            this.Controls.Add(this.txtInfoText);
            this.Controls.Add(this.txtTemp);
            this.Controls.Add(this.txtVolts);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FWCTestForm";
            this.Text = "PA Info";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.FWCTestForm_Closing);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		#region Misc Routines

		private bool closing = false;
		private void PollADC()
		{
			int chan = 0;
			switch(console.CurrentModel)
			{
				case Model.FLEX5000:
					chan = 4;
					break;
				case Model.FLEX3000:
					chan = 3;
					break;
			}

            while (!closing)
            {
                int val;
                if (FWC.ReadPAADC(2, out val) == 0) break;
                float volts = (float)val / 4096 * 2.5f * 11;
                if(volts >= 15.0) txtVolts.BackColor = Color.Red;
                else txtVolts.BackColor = SystemColors.Control;
                if (!closing) txtVolts.Text = "Voltage: " + volts.ToString("f1");
                Thread.Sleep(1000);

                if (FWC.ReadPAADC(chan, out val) == 0) break;
                volts = (float)val / 4096 * 2.5f;
                double temp_c = 301 - volts * 1000 / 2.2;
                if(temp_c >= 100) txtTemp.BackColor = Color.Red;
                else txtTemp.BackColor = SystemColors.Control;
                if (!closing)
                {
                    switch (temp_format)
                    {
                        case TempFormat.Celsius:
                            txtTemp.Text = "Temp: " + temp_c.ToString("f0") + "° C";
                            break;
                        case TempFormat.Fahrenheit:
                            txtTemp.Text = "Temp: " + ((temp_c * 1.8) + 32).ToString("f0") + "° F";
                            break;
                    }
                }
                Thread.Sleep(1000);
            }
		}

		#endregion

		#region Event Handlers

		private void FWCTestForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			closing = true;
		}

		private enum TempFormat
		{
			Celsius = 0,
			Fahrenheit,
		}

		private TempFormat temp_format = TempFormat.Celsius;
		private void txtTemp_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			switch(temp_format)
			{
				case TempFormat.Celsius:
					temp_format = TempFormat.Fahrenheit;
					break;
				case TempFormat.Fahrenheit:
					temp_format = TempFormat.Celsius;
					break;
			}
		}
		
        #endregion
	}
}
