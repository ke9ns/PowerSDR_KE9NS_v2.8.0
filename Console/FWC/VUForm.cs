//=================================================================
// VUForm.cs
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
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PowerSDR
{
    public partial class VUForm : Form
    {
        bool sendToHardware = true;
        private Console console;
        // private XVTRForm XVTRForm;

        public VUForm(Console c)
        {
            console = c;
            InitializeComponent();
        }

        private void chkFanHigh_CheckedChanged(object sender, EventArgs e)
        {
            if (sendToHardware)
                FWC.SetVU_FanHigh(chkVUFanHigh.Checked);
        }

        private void chkVUKeyV_CheckedChanged(object sender, EventArgs e)
        {
            if (sendToHardware)
                FWC.SetVU_KeyV(chkVUKeyV.Checked);
        }

        private void chkVUTXIFU_CheckedChanged(object sender, EventArgs e)
        {
            if (sendToHardware)
                FWC.SetVU_TXIFU(chkVUTXIFU.Checked);
        }

        private void chkVUKeyU_CheckedChanged(object sender, EventArgs e)
        {
            if (sendToHardware)
                FWC.SetVU_KeyU(chkVUKeyU.Checked);
        }

        private void chkVURXURX2_CheckedChanged(object sender, EventArgs e)
        {
            if (sendToHardware)
                FWC.SetVU_RXURX2(chkVURXURX2.Checked);
        }

        private void chkVURX2V_CheckedChanged(object sender, EventArgs e)
        {
            if (sendToHardware)
                FWC.SetVU_RX2V(chkVURX2V.Checked);  //rx2u to rx2v
        }

        private void chkVURXV_CheckedChanged(object sender, EventArgs e)
        {
            if (sendToHardware)
                FWC.SetVU_RXV(chkVURXV.Checked);  //rx2v to rxv
        }

        private void chkVUTXU_CheckedChanged(object sender, EventArgs e)
        {
            if (sendToHardware)
                FWC.SetVU_TXU(chkVUTXU.Checked);
        }

        private void chkVUTXV_CheckedChanged(object sender, EventArgs e)
        {
            if (sendToHardware)
                FWC.SetVU_TXV(chkVUTXV.Checked);
        }

        private void chkVUK14_CheckedChanged(object sender, EventArgs e)
        {
            if (sendToHardware)
                FWC.SetVU_K14(chkVUK14.Checked);  //15 to 14
        }

        private void chkVUK16_CheckedChanged(object sender, EventArgs e)
        {
            if (sendToHardware)
                FWC.SetVU_K16(chkVUK16.Checked); //18 to 16
        }

        private void chkVUK17_CheckedChanged(object sender, EventArgs e)
        {
            if (sendToHardware)
                FWC.SetVU_K17(chkVUK17.Checked); //16 to 17
        }

        private void chkVUKeyVU_CheckedChanged(object sender, EventArgs e)
        {
            if (sendToHardware)
                FWC.SetVU_KeyVU(chkVUKeyVU.Checked);
        }

        private void chkVUDrvU_CheckedChanged(object sender, EventArgs e)
        {
            if (sendToHardware)
                FWC.SetVU_DrvU(chkVUDrvU.Checked);
        }

        private void chkVUDrvV_CheckedChanged(object sender, EventArgs e)
        {
            if (sendToHardware)
                FWC.SetVU_DrvV(chkVUDrvV.Checked);
        }

        private void chkVUK18_CheckedChanged(object sender, EventArgs e)
        {
            if (sendToHardware)
                FWC.SetVU_K18(chkVUK18.Checked); //12 to 18
        }

        private void chkVUK15_CheckedChanged(object sender, EventArgs e)
        {
            if (sendToHardware)
                FWC.SetVU_K15(chkVUK15.Checked);  //13 to 15
        }

        private void chkVULPwrU_CheckedChanged(object sender, EventArgs e)
        {
            if (sendToHardware)
                FWC.SetVU_LPwrU(chkVULPwrU.Checked);
        }

        private void chkVULPwrV_CheckedChanged(object sender, EventArgs e)
        {
            if (sendToHardware)
                FWC.SetVU_LPwrV(chkVULPwrV.Checked);
        }

        private void VUForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        private void chkVUUIFHG1_CheckedChanged(object sender, EventArgs e)
        {
            if (sendToHardware)
                FWC.SetVU_UIFHG1(chkVUUIFHG1.Checked);
        }

        private void chkVUVIFHG1_CheckedChanged(object sender, EventArgs e)
        {
            if (sendToHardware)
                FWC.SetVU_VIFHG1(chkVUVIFHG1.Checked);
        }

        private void chkVUUIFHG2_CheckedChanged(object sender, EventArgs e)
        {
            if (sendToHardware)
                FWC.SetVU_UIFHG2(chkVUUIFHG2.Checked);
        }

        private void chkVUVIFHG2_CheckedChanged(object sender, EventArgs e)
        {
            if (sendToHardware)
                FWC.SetVU_VIFHG2(chkVUVIFHG2.Checked);
        }

        private void chkVURXIFV_CheckedChanged(object sender, EventArgs e)
        {
            if (sendToHardware)
                FWC.SetVU_VIFHG2(chkVURXIFV.Checked);
        }

        //User Modes
        private void chkVUmodeVHGRX1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkVUmodeVHGRX1.Checked)
            {
                FWC.SetVU_modeVHGRX1(chkVUmodeVHGRX1.Checked);
                chkVUKeyVU.Checked = false;
                chkVUKeyV.Checked = false;
                chkVUKeyU.Checked = false;
                chkVUTXV.Checked = false;
                chkVUTXU.Checked = false;

                chkVUK15.Checked = true;
                chkVURXIFV.Checked = true;
                chkVURXV.Checked = true;
                chkVUVIFHG2.Checked = false;
                chkVUVIFHG1.Checked = true;
            }
            else
            {
                FWC.SetVU_modeVHGRX1(chkVUmodeVHGRX1.Checked);
                chkVUK15.Checked = false;
                chkVURXIFV.Checked = false;
                chkVURXV.Checked = false;
                chkVUVIFHG2.Checked = false;
                chkVUVIFHG1.Checked = false;
            }
        }

        private void chkVUmodeVLG1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkVUmodeVLG1.Checked)
            {
                FWC.SetVU_modeVLG1(chkVUmodeVLG1.Checked);
                chkVUKeyVU.Checked = false;
                chkVUKeyV.Checked = false;
                chkVUKeyU.Checked = false;
                chkVUTXV.Checked = false;
                chkVUTXU.Checked = false;

                chkVUK15.Checked = true;
                chkVURXIFV.Checked = true;
                chkVURXV.Checked = true;
                chkVUVIFHG2.Checked = true;
                chkVUVIFHG1.Checked = false;
            }
            else
            {
                FWC.SetVU_modeVLG1(chkVUmodeVLG1.Checked);
                chkVUK15.Checked = false;
                chkVURXIFV.Checked = false;
                chkVURXV.Checked = false;
                chkVUVIFHG2.Checked = false;
                chkVUVIFHG1.Checked = false;
            }
        }

        private void chkVUmodeTXVLP_CheckedChanged(object sender, EventArgs e)
        {
            if (chkVUmodeTXVLP.Checked)
            {
                FWC.SetVU_modeTXVLP(chkVUmodeTXVLP.Checked);
                chkVUK15.Checked = false;
                chkVUK14.Checked = true;
                chkVULPwrV.Checked = true;
                chkVUDrvU.Checked = false;
                chkVUDrvV.Checked = true;
                chkVUTXIFU.Checked = false;
                chkVUKeyV.Checked = true;
                chkVUKeyVU.Checked = true;
            }
            else
            {
                chkVUK15.Checked = false;
                chkVUK14.Checked = false;
                chkVULPwrV.Checked = false;
                chkVUDrvU.Checked = false;
                chkVUDrvV.Checked = false;
                chkVUTXIFU.Checked = false;
                chkVUKeyV.Checked = false;
                chkVUKeyVU.Checked = false;
            }
        }

        private void chkVUmodeTXV60W_CheckedChanged(object sender, EventArgs e)
        {
            FWC.SetVU_modeTXV60W(chkVUmodeTXV60W.Checked);
            if (chkVUmodeTXV60W.Checked)
            {
                chkVUFanHigh.Checked = true;
                chkVUK15.Checked = false;
                chkVUTXV.Checked = true;
                chkVUTXV.Checked = false;
                chkVUK14.Checked = false;
                chkVULPwrV.Checked = false;
                chkVUDrvU.Checked = false;
                chkVUDrvV.Checked = true;
                chkVUTXIFU.Checked = false;
                chkVUKeyV.Checked = true;
                chkVUKeyVU.Checked = true;
            }
            else
            {
                chkVUFanHigh.Checked = false;
                chkVUK15.Checked = false;
                chkVUTXV.Checked = false;
                chkVUTXV.Checked = false;
                chkVUK14.Checked = false;
                chkVULPwrV.Checked = false;
                chkVUDrvU.Checked = false;
                chkVUDrvV.Checked = false;
                chkVUTXIFU.Checked = false;
                chkVUKeyV.Checked = false;
                chkVUKeyVU.Checked = false;

            }
        }

        private void chkVUmodeVLG2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chkVUmodeVHG2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chkVUmodeULG1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chkVUmodeUHG1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chkVUmodeULG2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chkVUmodeUHG2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chkVUmodeTXULP_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chkVUmodeTXU60W_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chkVU_VIFGain_CheckedChanged(object sender, EventArgs e)
        {
            setUserModeV();
        }

        private void chkVU_VPAEnable_CheckedChanged(object sender, EventArgs e)
        {
            setUserModeV();
        }

        private void chkVU_UIFGain_CheckedChanged(object sender, EventArgs e)
        {
            setUserModeU();
        }

        private void chkVU_UPAEnable_CheckedChanged(object sender, EventArgs e)
        {
            setUserModeU();
        }

        public void setUserModeV()
        {
            if (!chkVU_VIFGain.Checked && !chkVU_VPAEnable.Checked && (console.RX1XVTRIndex == 0 || console.RX2Enabled && console.RX2XVTRIndex == 0))
            {
                FWC.SetVU_VIFGain_PAEnable_00(true);
                Debug.WriteLine("FWC.SetVU_VIFGain_PAEnable_00(true)");

                sendToHardware = false;
                //RX
                chkVUK18.Checked = false;		//K18 -
                chkVUK15.Checked = true;		//K15 +
                chkVURXIFV.Checked = true;		//K9 +  
                chkVURXV.Checked = true;		//K8 +  
                chkVUVIFHG2.Checked = true;	    //K6 +  
                chkVUVIFHG1.Checked = false;	//K7 -
                //TX
                Debug.WriteLine("SetVU_FAN_enable = false;");
                Debug.WriteLine("SetVU_K15_enable = false;");
                Debug.WriteLine("SetVU_TXV_enable = false;");
                chkVUK14.Checked = true;		//K14+
                chkVULPwrV.Checked = true;	//K1+
                chkVUDrvU.Checked = false;		//K3-
                chkVUDrvV.Checked = true;		//K2+
                chkVUTXIFU.Checked = false;		//K4-
                Debug.WriteLine("SetVU_KeyV_enable = true;");
                Debug.WriteLine("SetVU_KeyVU_enable = true;");
                sendToHardware = true;
                //RX2
                if (chkVRX2.Checked)
                {
                    FWC.SetVU_VRX2Enable(true);
                }
                else
                {
                    FWC.SetVU_VRX2Enable(false);
                }
            }

            if (!chkVU_VIFGain.Checked && chkVU_VPAEnable.Checked && console.RX1XVTRIndex == 0)
            {
                FWC.SetVU_VIFGain_PAEnable_01(true);
                Debug.WriteLine("FWC.SetVU_VIFGain_PAEnable_01(true)");

                sendToHardware = false;
                //RX
                chkVUK18.Checked = false;		//K18 -
                chkVUK15.Checked = true;		//K15 +
                chkVURXIFV.Checked = true;		//K9 +  
                chkVURXV.Checked = true;		//K8 +  
                chkVUVIFHG2.Checked = true;	    //K6 +  
                chkVUVIFHG1.Checked = false;	//K7 -
                //TX
                Debug.WriteLine("SetVU_FAN_enable = true;");
                Debug.WriteLine("SetVU_K15_enable = false;");
                Debug.WriteLine("SetVU_TXV_enable = true;");
                chkVUK14.Checked = false;		//K14-
                chkVULPwrV.Checked = false;	    //K1-
                chkVUDrvU.Checked = false;		//K3-
                chkVUDrvV.Checked = true;		//K2+
                chkVUTXIFU.Checked = false;		//K4-
                Debug.WriteLine("SetVU_KeyV_enable = true;");
                Debug.WriteLine("SetVU_KeyVU_enable = true;");
                sendToHardware = true;
                //RX2
                if (chkVRX2.Checked)
                {
                    FWC.SetVU_VRX2Enable(true);
                }
                else
                {
                    FWC.SetVU_VRX2Enable(false);
                }

            }

            if (chkVU_VIFGain.Checked && !chkVU_VPAEnable.Checked && console.RX1XVTRIndex == 0)
            {
                FWC.SetVU_VIFGain_PAEnable_10(true);
                Debug.WriteLine("FWC.SetVU_VIFGain_PAEnable_10(true)");

                sendToHardware = false;
                //RX
                chkVUK18.Checked = false;		//K18 -
                chkVUK15.Checked = true;		//K15 +
                chkVURXIFV.Checked = true;		//K9 +  
                chkVURXV.Checked = true;		//K8 +  
                chkVUVIFHG2.Checked = false;	//K6 -  
                chkVUVIFHG1.Checked = true;	    //K7 +
                //TX
                Debug.WriteLine("SetVU_FAN_enable = false;");
                Debug.WriteLine("SetVU_K15_enable = false;");
                Debug.WriteLine("SetVU_TXV_enable = false;");
                chkVUK14.Checked = true;		//K14+
                chkVULPwrV.Checked = true;	    //K1+
                chkVUDrvU.Checked = false;		//K3-
                chkVUDrvV.Checked = true;		//K2+
                chkVUTXIFU.Checked = false;	    //K4-
                Debug.WriteLine("SetVU_KeyV_enable = true;");
                Debug.WriteLine("SetVU_KeyVU_enable = true;");
                sendToHardware = true;
                //RX2
                if (chkVRX2.Checked)
                {
                    FWC.SetVU_VRX2Enable(true);
                }
                else
                {
                    FWC.SetVU_VRX2Enable(false);
                }
            }

            if (chkVU_VIFGain.Checked && chkVU_VPAEnable.Checked && console.RX1XVTRIndex == 0)
            {
                FWC.SetVU_VIFGain_PAEnable_11(true);
                Debug.WriteLine("FWC.SetVU_VIFGain_PAEnable_11(true)");

                sendToHardware = false;
                //RX
                chkVUK18.Checked = false;		//K18 -
                chkVUK15.Checked = true;		//K15 +
                chkVURXIFV.Checked = true;		//K9 +  
                chkVURXV.Checked = true;		//K8 +  
                chkVUVIFHG2.Checked = false;    //K6 - 
                chkVUVIFHG1.Checked = true;	    //K7 +
                //TX
                Debug.WriteLine("SetVU_FAN_enable = true;");
                Debug.WriteLine("SetVU_K15_enable = false;");
                Debug.WriteLine("SetVU_TXV_enable = true;");
                chkVUK14.Checked = false;		//K14-
                chkVULPwrV.Checked = false;	    //K1-
                chkVUDrvU.Checked = false;		//K3-
                chkVUDrvV.Checked = true;		//K2+
                chkVUTXIFU.Checked = false;		//K4-
                Debug.WriteLine("SetVU_KeyV_enable = true;");
                Debug.WriteLine("SetVU_KeyVU_enable = true;");
                sendToHardware = true;
                //RX2
                if (chkVRX2.Checked)
                {
                    FWC.SetVU_VRX2Enable(true);
                }
                else
                {
                    FWC.SetVU_VRX2Enable(false);
                }
            }

        }
        public void setUserModeU()
        {
            if (!chkVU_UIFGain.Checked && !chkVU_UPAEnable.Checked && console.RX1XVTRIndex == 1)
            {
                FWC.SetVU_UIFGain_PAEnable_00(true);
                Debug.WriteLine(" FWC.SetVU_UIFGain_PAEnable_00(true)");
                sendToHardware = false;
                //RX
                chkVUK18.Checked = true;		//K18 +
                chkVUK15.Checked = false;		//K15 -
                chkVURXIFV.Checked = false;		//K9 -  
                chkVURXV.Checked = false;		//K8 -  
                chkVUUIFHG2.Checked = true;	   //K12 + 
                chkVUUIFHG1.Checked = false;	//K10 -
                //TX
                Debug.WriteLine("SetVU_FAN_enable = false;");
                Debug.WriteLine("SetVU_K18_enable = false;");
                Debug.WriteLine("SetVU_TXV_enable = false;");
                chkVUK17.Checked = true;        //K17+
                chkVULPwrU.Checked = true;      //K5+
                chkVUDrvU.Checked = true;		//K3+
                chkVUDrvV.Checked = false;		//K2-
                chkVUTXIFU.Checked = true;		//K4+
                Debug.WriteLine("SetVU_KeyU_enable = true;");
                Debug.WriteLine("SetVU_KeyVU_enable = true;");
                sendToHardware = true;
                //RX2
                if (chkURX2.Checked)
                {
                    FWC.SetVU_URX2Enable(true);
                }
                else
                {
                    FWC.SetVU_URX2Enable(false);
                }
            }

            if (!chkVU_UIFGain.Checked && chkVU_UPAEnable.Checked && console.RX1XVTRIndex == 1)
            {
                sendToHardware = false;
                FWC.SetVU_UIFGain_PAEnable_01(true);
                Debug.WriteLine(" FWC.SetVU_UIFGain_PAEnable_01(true)");
                //RX
                chkVUK18.Checked = true;		//K18 +
                chkVUK15.Checked = false;		//K15 -
                chkVURXIFV.Checked = false;		//K9 -  
                chkVURXV.Checked = false;		//K8 -  
                chkVUUIFHG2.Checked = true;	    //K12+
                chkVUUIFHG1.Checked = false;	//K10 -
                //TX
                Debug.WriteLine("SetVU_FAN_enable = true;");
                Debug.WriteLine("SetVU_K18_enable = false;");
                Debug.WriteLine("SetVU_TXV_enable = true;");
                chkVUK17.Checked = false;        //K17-
                chkVULPwrU.Checked = false;      //K5-
                chkVUDrvU.Checked = true;		//K3+
                chkVUDrvV.Checked = false;		//K2-
                chkVUTXIFU.Checked = true;		//K4+
                Debug.WriteLine("SetVU_KeyU_enable = true;");
                Debug.WriteLine("SetVU_KeyVU_enable = true;");
                sendToHardware = true;
                if (chkURX2.Checked)
                {
                    FWC.SetVU_URX2Enable(true);
                }
                else
                {
                    FWC.SetVU_URX2Enable(false);
                }
            }

            if (chkVU_UIFGain.Checked && !chkVU_UPAEnable.Checked && console.RX1XVTRIndex == 1)
            {
                FWC.SetVU_UIFGain_PAEnable_10(true);
                Debug.WriteLine(" FWC.SetVU_UIFGain_PAEnable_10(true)");
                sendToHardware = false;
                //RX
                chkVUK18.Checked = true;		//K18 +
                chkVUK15.Checked = false;		//K15 -
                chkVURXIFV.Checked = false;		//K9 -  
                chkVURXV.Checked = false;		//K8 -  
                chkVUUIFHG2.Checked = false;	    //K12 -
                chkVUUIFHG1.Checked = true;	    //K10 +
                //TX
                Debug.WriteLine("SetVU_FAN_enable = false;");
                Debug.WriteLine("SetVU_K18_enable = false;");
                Debug.WriteLine("SetVU_TXV_enable = false;");
                chkVUK17.Checked = true;        //K17+
                chkVULPwrU.Checked = true;      //K5+
                chkVUDrvU.Checked = true;		//K3+
                chkVUDrvV.Checked = false;		//K2-
                chkVUTXIFU.Checked = true;		//K4+
                Debug.WriteLine("SetVU_KeyU_enable = true;");
                Debug.WriteLine("SetVU_KeyVU_enable = true;");
                sendToHardware = true;
                if (chkURX2.Checked)
                {
                    FWC.SetVU_URX2Enable(true);
                }
                else
                {
                    FWC.SetVU_URX2Enable(false);
                }
            }

            if (chkVU_UIFGain.Checked && chkVU_UPAEnable.Checked && console.RX1XVTRIndex == 1)
            {
                FWC.SetVU_UIFGain_PAEnable_11(true);
                Debug.WriteLine(" FWC.SetVU_UIFGain_PAEnable_11(true)");
                sendToHardware = false;
                //RX
                chkVUK18.Checked = true;		//K18 +
                chkVUK15.Checked = false;		//K15 -
                chkVURXIFV.Checked = false;		//K9 -  
                chkVURXV.Checked = false;		//K8 -  
                chkVUUIFHG2.Checked = false;	    //K12 - 
                chkVUUIFHG1.Checked = true;	//K10 +
                //TX
                Debug.WriteLine("SetVU_FAN_enable = true;");
                Debug.WriteLine("SetVU_K18_enable = false;");
                Debug.WriteLine("SetVU_TXV_enable = true;");
                chkVUK17.Checked = false;        //K17-
                chkVULPwrU.Checked = false;      //K5-
                chkVUDrvU.Checked = true;		//K3+
                chkVUDrvV.Checked = false;		//K2-
                chkVUTXIFU.Checked = true;		//K4+
                Debug.WriteLine("SetVU_KeyU_enable = true;");
                Debug.WriteLine("SetVU_KeyVU_enable = true;");
                sendToHardware = true;
                if (chkURX2.Checked)
                {
                    FWC.SetVU_URX2Enable(true);
                }
                else
                {
                    FWC.SetVU_URX2Enable(false);
                }
            }

        }

        private void chkVRX2_CheckedChanged(object sender, EventArgs e)
        {
            setUserModeV();
        }

        private void chkURX2_CheckedChanged(object sender, EventArgs e)
        {
            setUserModeU();
        }
    }
}
