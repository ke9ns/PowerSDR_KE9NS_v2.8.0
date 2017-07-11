//=================================================================
// LegacyForm.cs
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PowerSDR
{
    public partial class LegacyForm : Form
    {
        public LegacyForm()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.Close();
        }

        private void LegacyForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (chkSDR1000.Checked || chkSoftRock.Checked)
            {
                if (chkSDR1000.Checked)
                {
                    Radio r = new Radio(Model.SDR1000, null, txtSDR1000SN.Text, true);
                    Master.AddRadio(r);
                    RadiosAvailable.AddRadio(r);
                }

                if (chkSoftRock.Checked)
                {
                    Radio r = new Radio(Model.SOFTROCK40, null, txtSoftRockSN.Text, true);
                    Master.AddRadio(r);
                    RadiosAvailable.AddRadio(r);
                }

                Master.Commit();
            }
        }
    }
}
