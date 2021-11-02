//=================================================================
// DSPTestForm.cs
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
    unsafe public partial class DSPTestForm : System.Windows.Forms.Form
    {
        private Console console;
       

        private void chkMu_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkMu.Checked)
            {
                chkMu.BackColor = console.ButtonSelectedColor;
                DttSP.SetCorrectIQMu(0, 0, (double)udMu.Value);
            }
            else
            {
                chkMu.BackColor = SystemColors.Control;
                DttSP.SetCorrectIQMu(0, 0, 0.000);
            }
        }

        private void udMu_ValueChanged(object sender, System.EventArgs e)
        {
            if (chkMu.Checked) DttSP.SetCorrectIQMu(0, 0, (double)udMu.Value);
        }

        private void udLMSNR_ValueChanged(object sender, System.EventArgs e)
        {
            console.dsp.GetDSPRX(0, 0).SetNRVals(
                (int)udLMSNRtaps.Value,
                (int)udLMSNRdelay.Value,
                0.00001 * (double)udLMSNRgain.Value,
                0.0000001 * (double)udLMSNRLeak.Value);
            console.dsp.GetDSPRX(0, 1).SetNRVals(
                (int)udLMSNRtaps.Value,
                (int)udLMSNRdelay.Value,
                0.00001 * (double)udLMSNRgain.Value,
                0.0000001 * (double)udLMSNRLeak.Value);
        }

        private void udLMSANF_ValueChanged(object sender, System.EventArgs e)
        {
            console.dsp.GetDSPRX(0, 0).SetANFVals(
                (int)udLMSANFtaps.Value,
                (int)udLMSANFdelay.Value,
                0.00001 * (double)udLMSANFgain.Value,
                0.00005);
        }

        private void checkBoxIQEnable_CheckedChanged(object sender, System.EventArgs e)
        {
            if (checkBoxIQEnable.Checked)
                DttSP.SetCorrectIQEnable(0);
            else DttSP.SetCorrectIQEnable(1);
        }

        private void btnSAMPLL_Click(object sender, System.EventArgs e)
        {
            float freq;
            DttSP.GetSAMFreq(0, 0, &freq);
            freq = (float)(freq * console.SampleRate1 / (2 * Math.PI));
            txtSAMPLL.Text = freq.ToString("f0");
        }

        private void btnIQW_Click(object sender, System.EventArgs e)
        {
            float real, imag;
            DttSP.GetCorrectRXIQw(0, 0, &real, &imag, 0);
            txtIQWReal.Text = real.ToString("f6");
            txtIQWImag.Text = imag.ToString("f6");
        }

        private void udDCBlock_ValueChanged(object sender, EventArgs e)
        {
            // do something to affect DC Block params here
            DttSP.SetRXDCBlockGain(0, 0, (float)udDCBlock.Value);
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void checkBoxRXDCBlockEnable_CheckedChanged(object sender, EventArgs e)
        {
            DttSP.SetRXDCBlock(0, 0, checkBoxRXDCBlockEnable.Checked);

        }

        private void chkAudioMox_CheckedChanged(object sender, EventArgs e)
        {
            Audio.MOX = chkAudioMox.Checked;
        }

        private void checkBoxMNEnable_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxMNEnable.Checked)
                DttSP.SetRXManualNotchEnable(0, 0, 0, true);
            else
                DttSP.SetRXManualNotchEnable(0, 0, 0, false);

        }

        private void udMNFreq_ValueChanged(object sender, EventArgs e)
        {
            DttSP.SetRXManualNotchFreq(0, 0, 0, (double)udMNFreq.Value);
        }
    }
}
