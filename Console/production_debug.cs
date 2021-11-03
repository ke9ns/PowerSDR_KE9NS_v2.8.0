//=================================================================
// production_debug.cs
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
    /// <summary>
    /// Summary description for production_debug.
    /// </summary>
    public partial class ProductionDebug : System.Windows.Forms.Form
    {
        #region Variable Declaration

        private Console console;
        
        #endregion


        #region Thread Routines

        private void PIORotate()
        {
            while (radPIORotate.Checked)
            {
                console.Hdw.TestPIO1();
                Thread.Sleep(100);
            }
        }

        private void PIOSetClearAll()
        {
            while (radPIOSetClearAll.Checked)
            {
                console.Hdw.TestPIO3();
                Thread.Sleep(100);
            }
        }

        #endregion

        #region Event Handlers

        private void ProductionDebug_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Hide();
            radPIOOff.Checked = true;
            e.Cancel = true;
        }

        private void radPIOOff_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radPIOOff.Checked)
            {
                console.Hdw.PowerOn();
                Thread.Sleep(100);
                console.PowerOn = true;
            }
        }

        private void radPIORotate_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radPIORotate.Checked)
            {
                Thread t = new Thread(new ThreadStart(PIORotate));
                t.Name = "PIORotate Thread";
                t.IsBackground = true;
                t.Priority = ThreadPriority.Normal;
                t.Start();
            }
        }

        private void radPIOSetClearAll_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radPIOSetClearAll.Checked)
            {
                Thread t = new Thread(new ThreadStart(PIOSetClearAll));
                t.Name = "PIOSetClearAll Thread";
                t.IsBackground = true;
                t.Priority = ThreadPriority.Normal;
                t.Start();
            }
        }

        private void radPIOSetEven_CheckedChanged(object sender, System.EventArgs e)
        {
            console.Hdw.TestPIO2(true);
        }

        private void radPIOSetOdd_CheckedChanged(object sender, System.EventArgs e)
        {
            console.Hdw.TestPIO2(false);
        }

        #endregion
    }
}
