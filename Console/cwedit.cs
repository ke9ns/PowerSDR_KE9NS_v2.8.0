//=================================================================
// cwedit.cs
//=================================================================
// PowerSDR is a C# implementation of a Software Defined Radio.
// Copyright (C) 2005, 2006  Richard Allen
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
using System.Diagnostics;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace PowerSDR
{
    /// <summary>
    /// Summary description for cwedit.
    /// </summary>
    public partial class cwedit : System.Windows.Forms.Form
    {
        Console console;

        #region Morse code definition editor

        private string sedit;

        //           11111111112222222222
        // 012345678901234567890123456789
        //	sw.WriteLine("37|%|.-...    | [AS]      ");
        //	sw.WriteLine("38|&|.........| 0123456789");
        //	sw.WriteLine("39|'|         |           ");
        //	sw.WriteLine("40|(|-.--.    | [KN]      ");
        //	sw.WriteLine("41|)|         |           ");

        private string id, els, cmnts;

        private void extract_fields()
        {
            id = sedit.Substring(0, 5);
            els = sedit.Substring(5, 9);
            //	Debug.WriteLine(sedit.Length);
            cmnts = sedit.Substring(16, 10);
        }

        private void make_current()
        {
            txtCurrent.Text = id + els + "| " + cmnts;
            //			Debug.WriteLine("'" + txtCurrent.Text + "' " + txtCurrent.Text.Length);
        }

        private void cwedit_Load(object sender, System.EventArgs e)
        {
            sedit = console.cwxForm.editline;

            txtOriginal.Text = sedit;

            //		Debug.WriteLine("enter '" + sedit + "' "+ sedit.Length);
            extract_fields();
            txtElements.Text = els;
            txtComments.Text = cmnts;
            make_current();
        }

        private void saveButton_Click(object sender, System.EventArgs e)
        {
            console.cwxForm.editline = txtCurrent.Text;
            this.Close();
        }

        private void cancelButton_Click(object sender, System.EventArgs e)
        {
            if (MessageBox.Show("Do you want to exit without saving?", " CW Editor",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                == DialogResult.Yes)
            {
                console.cwxForm.editline = "";
                this.Close();
            }
        }


        private string slen(string s, int len)
        {
            if (s.Length < len) return (s.PadRight(len, ' '));
            if (s.Length > len) return (s.Substring(0, len));
            return (s);
        }

        private void txtComments_Leave(object sender, System.EventArgs e)
        {
            string s;

            s = txtComments.Text;
            s = slen(s, 10);
            txtComments.Text = s;

            cmnts = s;
            //	Debug.WriteLine(s + " " + s.Length);
            make_current();
        }

        private void txtElements_Leave(object sender, System.EventArgs e)
        {
            string s;

            s = txtElements.Text;
            s = slen(s, 9);
            txtElements.Text = s;

            els = s;
            Debug.WriteLine(s + " " + s.Length);
            make_current();
        }
        #endregion

    } // end class
} // end namespace

