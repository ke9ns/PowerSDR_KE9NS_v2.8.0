//=================================================================
// InputBox.cs
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
using System.Windows.Forms;

namespace PowerSDR
{
    /// <summary>
    /// Summary description for InputBox.
    /// </summary>
    public partial class InputBox : System.Windows.Forms.Form
    {
      
        private string retval;
       

        #region Misc Routines

        public static string Show(string title, string label, string textbox)
        {
            InputBox box = new InputBox();
            box.Text = title;
            box.label.Text = label;
            box.textbox.Text = textbox;

            box.ShowDialog();
            string retval = box.retval;
            box.Dispose();
            return retval;
        }

        #endregion

        #region Event Handlers

        private void btnOK_Click(object sender, System.EventArgs e)
        {
            retval = textbox.Text;
            this.Close();
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            retval = "";
            this.Close();
        }

        #endregion
    }
}
