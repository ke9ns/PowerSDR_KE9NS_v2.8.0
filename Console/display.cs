//=================================================================
// display.cs
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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Threading;
using System.Windows.Forms;
using System.Text;  // ke9ns add for stringbuilder
using System.Text.RegularExpressions;
using System.Globalization;
using System.IO;
using System.IO.Ports;

#if(!NO_TNF)
using Flex.TNF;
#endif

//using Microsoft.DirectX;
//using Microsoft.DirectX.Direct3D;


namespace PowerSDR
{		
	class Display
	{

        #region Variable Declaration

        public static Console console;
        public Setup setupForm;                        // ke9ns communications with setupform  (i.e. allow combometertype.text update from inside console.cs) 

        public static SpotControl SpotForm;                     // ke9ns add  communications with spot.cs and dx spotter
        public ScanControl ScanForm;                            // ke9ns add freq Scanner function

        //private static Mutex background_image_mutex;			// used to lock the base display image
        //private static Bitmap background_bmp;					// saved background picture for display
        //private static Bitmap display_bmp;					// Bitmap for use when drawing
        //private static int waterfall_counter;
        private static Bitmap waterfall_bmp;					// RX1 saved waterfall picture for display
		private static Bitmap waterfall_bmp2;                   // RX2
		private static int[] histogram_data;					// histogram display buffer
		private static int[] histogram_history;					// histogram counter
		//private static Graphics display_graphics;				// GDI graphics object
		public const float CLEAR_FLAG = -999.999F;				// for resetting buffers

		public const int BUFFER_SIZE = 4096;                    // ke9ns   this buffer size is always the same ?? 4096 data points across the display area always ???

        public const int DATA_BUFFER_SIZE = 4096;               // ke9ns add  every place in this file with 4096 was substitued with this const

        public const int abright = 5;                           // ke9ns add  used by auto water level, how much db to subtract from the actual avg determined by a scan of the waterfall
        public const int abrightpan = 15;                       // ke9ns add  used by auto water level, how much db to subtract from the actual avg determined by a scan of the panadapter


        public static float[] new_display_data;					// Buffer used to store the new data from the DSP for the display
		public static float[] current_display_data;             // Buffer used to store the current data for the display
        public static float[] current_display_data1;            // ke9ns add this is data that is never avg

        public static float[] new_display_data_bottom;          // RX2
		public static float[] current_display_data_bottom;      // RX2 
        public static float[] current_display_data_bottom1;     // ke9ns add this is data that is never avg


        public static float[] rx1_average_buffer;					// Averaged display data buffer
		public static float[] rx2_average_buffer;
		public static float[] rx1_peak_buffer;						// Peak hold display data buffer
		public static float[] rx2_peak_buffer;

        private static List<Channel> channels_60m;

    
        #endregion

        #region Properties

        public static List<Channel> Channels60m
        {
            get { return channels_60m; }
        }

   
        public static int[] band_edge_list_r77 =
                  {          26965000,26965100,  26975000,26975100,   26985000,26985100,  // channels 1-3
                             27005000,27005100,  27015000,27015100,   27025000,27025100,  // channels 4-6
                             27035000,27035100,  27055000,27055100,   27065000,27065100,  // channels 7-9
                             27075000,27075100,  27085000,27085100,   27105000,27105100,  // channels 10-12
                             27115000,27115100,  27125000,27125100,   27135000,27135100,  // channels 13-15
                             27155000,27155100,  27165000,27165100,   27175000,27175100,  // channels 16-18
                             27185000,27185100,  27205000,27205100,   27215000,27215100,  // channels 19-21
                             27225000,27225100,  27255000,27255100,   27235000,27235100,  // channels 22-24
                             27245000,27245100,  27265000,27265100,   27275000,27275100,  // channels 25-27
                             27285000,27285100,  27295000,27295100,   27305000,27305100,  // channels 28-30
                             27315000,27315100,  27325000,27325100,   27335000,27335100,  // channels 31-33
                             27345000,27345100,  27355000,27355100,   27365000,27365100,  // channels 34-36
                             27375000,27375100,  27385000,27385100,   27395000,27395100,  // channels 37-39
                             27405000,27405100,   // channel 40

                             27415000,27415100,  27425000,27425100,  27435000,27435100,  // channel 41-43
                             27445000,27445100,  27455000,27455100,  27465000,27465100,  // channel 44-46
                             27475000,27475100,  27485000,27485100,  27495000,27495100, // channel 47-49
                             27505000,27505100,  27515000,27515100,  27525000,27525100, // channel 50-52
                             27535000,27535100,  27545000,27545100,  27555000,27555100, // channel 53-55
                             27565000,27565100,  27575000,27575100,  27585000,27585100, // channel 56-58
                             27595000,27595100,  27605000,27605100,  27615000,27615100, // channel 59-61
                             27625000,27625100,  27635000,27635100,  27645000,27645100, // channel 62-64
                             27655000,27655100,  27665000,27665100,  27675000,27675100, // channel 65-67
                             27685000,27685100,  27695000,27695100,  27705000,27705100, // channel 68-70
                             27715000,27715100,  27725000,27725100,  27735000,27735100, // channel 71-73
                          
                             27745000,27745100,  27755000,27755100,  27765000,27765100, // channel 74-76
                             27775000,27775100,  27785000,27785100,  27795000,27795100, // channel 77-79
                             27805000,27805100



                    };
/*
        public static int[] band_edge_list_r77 =
                        {          26962200,26965000,26965000,26967800,   26972200, 26975000, 26975000, 26977800,   26982200, 26985000, 26985000, 26987800, // channels 1-3
                             27002200,27005000,27005000,27007800,  27012200,27015000,27015000,27017800,   27022200,27025000,27025000,27027800,  // channels 4-6
                             27032200,27035000,27035000,27037800,  27052200,27055000,27055000,27057800,   27062200,27065000,27065000,27067800,  // channels 7-9
                             27072200,27075000,27075000,27077800,  27082200,27085000,27085000,27087800,   27102200,27105000,27105000,27107800,  // channels 10-12
                             27112200,27115000,27115000,27117800,  27122200,27125000,27125000,27127800,   27132200,27135000,27135000,27137800,  // channels 13-15
                             27152200,27155000,27155000,27157800,  27162200,27165000,27165000,27167800,   27172200,27175000,27175000,27177800,  // channels 16-18
                             27182200,27185000,27185000,27187800,  27202200,27205000,27205000,27207800,   27212200,27215000,27215000,27217800,  // channels 19-21
                             27222200,27225000,27225000,27227800, 27252200,27255000,27255000,27257800,   27232200,27235000,27235000,27237800,  // channels 22-24
                             27242200,27245000,27245000,27247800,  27262200,27265000,27265000,27267800,   27272200,27275000,27275000,27277800,  // channels 25-27
                             27282200,27285000,27285000,27287800,  27292200,27295000,27295000,27297800,   27302200,27305000,27305000,27307800,  // channels 28-30
                             27312200,27315000,27315000,27317800,  27322200,27325000,27325000,27327800,   27332200,27335000,27335000,27337800,  // channels 31-33
                             27342200,27345000,27345000,27347800,  27352200,27355000,27355000,27357800,   27362200,27365000,27365000,27367800,  // channels 34-36
                             27372200,27375000,27375000,27377800,  27382200,27385000,27385000,27387800,   27392200,27395000,27395000,27397800,  // channels 37-39
                             27402200,27405000,27405000,27407800,   // channel 40

                             27412200,27415000,27415000,27417800,  27422200,27425000,27425000,27427800,  27432200,27435000,27435000,27437800,  // channel 41-43
                             27442200,27445000,27445000,27447800,  27452200,27455000,27455000,27457800,  27462200,27465000,27465000,27467800  // channel 44-46


                    };

            */
        private static bool tnf_zoom = false;
        public static bool TNFZoom
        {
            get { return tnf_zoom; }
            set
            {
                tnf_zoom = value;
                if (current_display_mode == DisplayMode.PANADAPTER)  DrawBackground();
            }
        }

        private static bool tnf_active = true;
        public static bool TNFActive
        {
            get { return tnf_active; }
            set 
            {
                tnf_active = value;
                if (current_display_mode == DisplayMode.PANADAPTER)
                    DrawBackground();
            }
        }

        //Color notch_on_color = Color.DarkGreen;
        //Color notch_highlight_color = Color.Chartreuse;
        //Color notch_perm_on_color = Color.DarkRed;
        //Color notch_perm_highlight_color = Color.DeepPink;

        private static Color notch_on_color = Color.Olive;
        private static Color notch_on_color_zoomed = Color.FromArgb(190, 128, 128, 0);
        private static Color notch_highlight_color = Color.YellowGreen;
        private static Color notch_highlight_color_zoomed = Color.FromArgb(190, 154, 205, 50);
        private static Color notch_perm_on_color = Color.DarkGreen;
        private static Color notch_perm_highlight_color = Color.Chartreuse;
        private static Color notch_off_color = Color.Gray;

        private static Color channel_background_on = Color.FromArgb(128, Color.DodgerBlue);
        private static Color channel_background_off = Color.FromArgb(32, Color.RoyalBlue);
        private static Color channel_foreground = Color.Cyan;

        private static double notch_zoom_start_freq;
        public static double NotchZoomStartFreq // in mhz or hz?
        {
            get { return notch_zoom_start_freq; }
            set { notch_zoom_start_freq = value; }
        }

        private static bool pan_fill = true;
        public static bool PanFill
        {
            get { return pan_fill; }
            set { pan_fill = value; }
        }

		private static bool tx_on_vfob = false;
		public static bool TXOnVFOB
		{
			get { return tx_on_vfob; }
			set
			{
				tx_on_vfob = value;
				if(current_display_mode == DisplayMode.PANADAPTER)
					DrawBackground();
			}
		}
		private static bool split_display = false;
		public static bool SplitDisplay
		{
			get { return split_display; }
			set
			{
				split_display = value;
				DrawBackground();
			}
		}

		/*private static DisplayMode current_display_mode_top = DisplayMode.PANADAPTER;
		public static DisplayMode CurrentDisplayModeTop
		{
			get { return current_display_mode_top; }
			set 
			{
				current_display_mode_top = value;
				if(split_display) DrawBackground();
			}
		}*/

		private static DisplayMode current_display_mode_bottom = DisplayMode.PANADAPTER;
		public static DisplayMode CurrentDisplayModeBottom
		{
			get { return current_display_mode_bottom; }
			set
			{
				current_display_mode_bottom = value;

                if (console.chkRX2.Checked) // dont check auto wtr level if rx2 isnt even on
                {
                    switch (current_display_mode_bottom)  //ke9ns add  (change visability of autobrightbox on console
                    {
                        case DisplayMode.PANADAPTER:
                        case DisplayMode.PANAFALL:
                        case DisplayMode.WATERFALL:
                            console.autoBrightBox.Text = "Auto Wtr/Pan Lvl";
                            autoBright5 = 1;

                            break;
                        default:
                            if (continuum == 0)
                            {
                                if (autoBright4 == 0)
                                {
                                    console.autoBrightBox.Text = "";
                                    //   Debug.WriteLine("off======");
                                }
                            }
                            autoBright5 = 0;
                            break;

                    }

                   //   Debug.WriteLine("ab4 " + autoBright4 + " ab5 " + autoBright5);

                }
                else autoBright5 = 0;


                if (split_display) DrawBackground();
			}
        } // DisplayMode CurrentDisplayModeBottom

        private static int rx1_filter_low;
		public static int RX1FilterLow
		{
			get { return rx1_filter_low; }
			set { rx1_filter_low = value; }
		}

		private static int rx1_filter_high;
		public static int RX1FilterHigh
		{
			get { return rx1_filter_high; }
			set	{ rx1_filter_high = value; }
		}

		private static int rx2_filter_low;
		public static int RX2FilterLow
		{
			get { return rx2_filter_low; }
			set { rx2_filter_low = value; }
		}

		private static int rx2_filter_high;
		public static int RX2FilterHigh
		{
			get { return rx2_filter_high; }
			set	{ rx2_filter_high = value; }
		}

		private static int tx_filter_low;
		public static int TXFilterLow
		{
			get { return tx_filter_low; }
			set { tx_filter_low = value; }
		}

		private static int tx_filter_high;
		public static int TXFilterHigh
		{
			get { return tx_filter_high; }
			set	{ tx_filter_high = value; }
		}

		private static Color sub_rx_zero_line_color = Color.LightSkyBlue;
		public static Color SubRXZeroLine
		{
			get { return sub_rx_zero_line_color; }
			set
			{
				sub_rx_zero_line_color = value;
				if(current_display_mode == DisplayMode.PANADAPTER && sub_rx1_enabled)
					DrawBackground();
			}
		}

		private static Color sub_rx_filter_color = Color.Blue;
		public static Color SubRXFilterColor
		{
			get { return sub_rx_filter_color; }
			set
			{
				sub_rx_filter_color = value;
				if(current_display_mode == DisplayMode.PANADAPTER && sub_rx1_enabled)
					DrawBackground();
			}
		}

		private static bool sub_rx1_enabled = false;
		public static bool SubRX1Enabled
		{
			get { return sub_rx1_enabled; }
			set
			{
				sub_rx1_enabled = value;
				if(current_display_mode == DisplayMode.PANADAPTER)
					DrawBackground();
			}
		}

		private static bool split_enabled = false;
		public static bool SplitEnabled
		{
			get { return split_enabled; }
			set
			{
				split_enabled = value;
				if(current_display_mode == DisplayMode.PANADAPTER && draw_tx_filter) DrawBackground();
			}
		}

		private static bool show_freq_offset = false;
		public static bool ShowFreqOffset
		{
			get { return show_freq_offset; }
			set
			{
				show_freq_offset = value;
				if(current_display_mode == DisplayMode.PANADAPTER)
					DrawBackground();
			}
		}

        private static Color band_box_color = Color.Lime;
        public static Color BandBoxColor
        {
            get { return band_box_color; }
            set
            {
                band_box_color = value;
                if (current_display_mode == DisplayMode.PANADAPTER)
                    DrawBackground();
            }
        }

        private static float band_box_width = 1.0F;
        public static float BandBoxWidth
        {
            get { return band_box_width; }
            set
            {
                band_box_width = value;
                if (current_display_mode == DisplayMode.PANADAPTER)
                    DrawBackground();
            }
        }
        
        private static Color band_edge_color = Color.Red;
		public static Color BandEdgeColor
		{
			get{ return band_edge_color;}
			set
			{
				band_edge_color = value;
				if(current_display_mode == DisplayMode.PANADAPTER)
					DrawBackground();
			}
		}

		private static long vfoa_hz;
		public static long VFOA
		{
			get
            {
                 return vfoa_hz; 
            }
			set
			{
				vfoa_hz = value;
           
                //if(current_display_mode == DisplayMode.PANADAPTER)
                //	DrawBackground();
            }
        }

       


        private static long vfoa_sub_hz;
		public static long VFOASub
		{
			get { return vfoa_sub_hz; }
			set 
			{
				vfoa_sub_hz = value; // value is in hz (full vfob value)
             //   Debug.WriteLine("vfoa_sub_hz" + vfoa_sub_hz);

				//if(current_display_mode == DisplayMode.PANADAPTER)
				//	DrawBackground();
			}
		}

		private static long vfob_hz;
		public static long VFOB
		{
			get { return vfob_hz; }
			set
			{
				vfob_hz = value;
				//if((current_display_mode == DisplayMode.PANADAPTER && split_enabled && draw_tx_filter) ||
				//	(current_display_mode == DisplayMode.PANADAPTER && sub_rx1_enabled))
				//	DrawBackground();
			}
		}

		private static long vfob_sub_hz;
		public static long VFOBSub
		{
			get { return vfob_sub_hz; }
			set 
			{
				vfob_sub_hz = value;
				//if(current_display_mode == DisplayMode.PANADAPTER)
				//	DrawBackground();
			}
		}

		private static int rit_hz;
		public static int RIT
		{ 
			get { return rit_hz; }
			set
			{
				rit_hz = value;
				//if(current_display_mode == DisplayMode.PANADAPTER)
				//	DrawBackground();
			}
		}

		private static int xit_hz;
		public static int XIT
		{ 
			get { return xit_hz; }
			set
			{
				xit_hz = value;
				//if(current_display_mode == DisplayMode.PANADAPTER && (draw_tx_filter || mox))
				//	DrawBackground();
			}
		}

		private static int cw_pitch = 600;
		public static int CWPitch
		{
			get { return cw_pitch; }
			set { cw_pitch = value; }
		}

       
		private static int H = 0;   // target height
        private static int W = 0;   // target width

        //=======================================================

        private static PixelFormat WtrColor = PixelFormat.Format24bppRgb;  //          


        public static int map = 0; // ke9ns add 1=map mode (panafall but only a small waterfall) and only when just in RX1 mode)

        public static int H1 = 0;  //  ke9ns add used to fool draw routines when displaying in 3rds 
        public static int H2 = 0;  //  ke9ns add used to fool draw routines when displaying in 4ths   

        public static int K9 = 0;  // ke9ns add rx1 display mode selector:  1=water,2=pan,3=panfall, 5=panfall with RX2 on any mode, 7=special map viewing panafall
        public static int K10 = 0; // ke9ns add rx2 display mode selector: 0=off 1=water,2=pan, 5=panfall

        private static int K11 = 0; // ke9ns add set to 5 in RX1 in panfall, otherwise 0
     
    
        private static int K10LAST = 0; // ke9ns add flag to check for only changes in display mode rx2
        private static int K9LAST = 0;  // ke9ns add flag to check for only changes in display mode rx1

        private static int K13 = 0; // ke9ns add original H size before being reduced and past onto drawwaterfall to create bmp file size correctly
        public static int K14 = 0; // ke9ns add used to draw the bmp waterfall 1 time when you changed display modes.
        private static int K15 = 1; // ke9ns add used to pass the divider factor back to the init() routine to keep the bmp waterfall size correct

        private static float temp_low_threshold = 0; // ke9ns add to switch between TX and RX low level waterfall levels
        private static float temp_high_threshold = 0; // ke9ns add for TX upper level

        public static int DIS_X = 0; // ke9ns add always the size of picdisplay
        public static int DIS_Y = 0; // ke9ns add

        //========================================================

        private static Control target = null;
		public static Control Target                 // ke9ns come here when picdisplay is resized (ie. console is resized)
		{
			get { return target; }
			set
			{
				target = value;
				DIS_Y = H = target.Height; // ke9ns mod
				DIS_X = W = target.Width; // ke9ns mod
				Audio.ScopeDisplayWidth = W;

                
			}
		}

		private static int rx_display_low = -4000; // in hz
		public static int RXDisplayLow
		{
			get { return rx_display_low; }  // ke9ns -96000 at 192k SR and zoom = .5
			set { rx_display_low = value; }
		}

		private static int rx_display_high = 4000;
		public static int RXDisplayHigh
		{
			get { return rx_display_high; } // ke9ns panadapter +96000 at 192k SR and zoom = .5 (different for spectrum display)
            set { rx_display_high = value; }
		}

		private static int tx_display_low = -4000;
		public static int TXDisplayLow
		{
			get { return tx_display_low; }
			set { tx_display_low = value; }
		}

		private static int tx_display_high = 4000;
		public static int TXDisplayHigh
		{
			get { return tx_display_high; }
			set { tx_display_high = value; }
		}

		private static float rx1_preamp_offset = 0.0f;
		public static float RX1PreampOffset
		{
			get { return rx1_preamp_offset; }
			set	{ rx1_preamp_offset = value; }
		}

		private static float rx2_preamp_offset = 0.0f;
		public static float RX2PreampOffset
		{
			get { return rx2_preamp_offset; }
			set	{ rx2_preamp_offset = value; }
		}

		private static float rx1_display_cal_offset;					// display calibration offset in dB
		public static float RX1DisplayCalOffset
		{
			get { return rx1_display_cal_offset; }
			set { rx1_display_cal_offset = value; }
		}

		private static float rx2_display_cal_offset;					// display calibration offset in dB
		public static float RX2DisplayCalOffset
		{
			get { return rx2_display_cal_offset; }
			set { rx2_display_cal_offset = value; }
		}

		private static Model current_model = Model.FLEX5000;
		public static Model CurrentModel
		{
			get { return current_model; }
			set	{ current_model = value; }
		}

		private static int display_cursor_x;						// x-coord of the cursor when over the display
		public static int DisplayCursorX
		{
			get { return display_cursor_x; }
			set { display_cursor_x = value; }
		}

		private static int display_cursor_y;						// y-coord of the cursor when over the display
		public static int DisplayCursorY
		{
			get { return display_cursor_y; }
			set { display_cursor_y = value; }
		}

		private static ClickTuneMode current_click_tune_mode = ClickTuneMode.Off;
		public static ClickTuneMode CurrentClickTuneMode
		{
			get { return current_click_tune_mode; }
			set { current_click_tune_mode = value; }
		}

		private static int scope_time = 50;
		public static int ScopeTime
		{
			get { return scope_time; }
			set { scope_time = value; }
		}

		private static int sample_rate = 48000;
		public static int SampleRate
		{
			get { return sample_rate; }
			set { sample_rate = value; }
		}

		private static bool high_swr = false;
		public static bool HighSWR
		{
			get { return high_swr; }
			set { high_swr = value; }
		}

		private static DisplayEngine current_display_engine = DisplayEngine.GDI_PLUS;
		public static DisplayEngine CurrentDisplayEngine
		{
			get { return current_display_engine; }
			set	{ current_display_engine = value; }
		}

		private static bool mox = false;
		public static bool MOX
		{
			get { return mox; }
			set { mox = value; }
		}

		private static DSPMode rx1_dsp_mode = DSPMode.USB;
		public static DSPMode RX1DSPMode
		{
			get { return rx1_dsp_mode; }
			set { rx1_dsp_mode = value; }
		}

		private static DSPMode rx2_dsp_mode = DSPMode.USB;
		public static DSPMode RX2DSPMode
		{
			get { return rx2_dsp_mode; }
			set { rx2_dsp_mode = value; }
		}


        public static byte continuum = 0; // ke9ns add

        public static byte autoBright4 = 0; // ke9ns add 1=rx1 in panafall or waterfall mode when RX2 ON
        public static byte autoBright5 = 0; // ke9ns add 1=rx2 in panafall or waterfall mode when RX1 ON

        public static DisplayMode current_display_mode = DisplayMode.PANADAPTER;
		public static DisplayMode CurrentDisplayMode
		{
			get { return current_display_mode; }
			set 
			{
				//PrepareDisplayVars(value);

				current_display_mode = value;

              
                switch (current_display_mode)  //ke9ns add  (change visability of autobrightbox on console
                {

                    case DisplayMode.PANADAPTER:
                    case DisplayMode.PANAFALL:
                    case DisplayMode.WATERFALL:
                         console.autoBrightBox.Text = "Auto Wtr/Pan Lvl";
                         autoBright4 = 1;
                        

                         break;
                    default:
                        if (continuum == 0)
                        {

                            if (autoBright5 == 0)
                            {
                                console.autoBrightBox.Text = "";
                                //  Debug.WriteLine("1off======");
                            }
                        }
                        autoBright4 = 0;
                        break;

                }


                if (current_display_mode == DisplayMode.CONTINUUM)
                {
                    continuum = 1; // ke9ns add  this is a waterfall mode where the data is MaxY txtDisplayPeakPower data from the console.
                    current_display_mode = DisplayMode.WATERFALL;

                }
                else
                {
                    continuum = 0; // ke9ns add continuum mode is off
                }



              //  Debug.WriteLine("1ab4 " + autoBright4 + " 1ab5 " + autoBright5);

                /*switch(current_display_mode)
				{
					case DisplayMode.PANADAPTER:
					case DisplayMode.WATERFALL:
						DttSP.SetPWSmode(0, 0, 1);
						DttSP.NotPan = false;
						break;
					default:
						DttSP.SetPWSmode(0, 0, 0);
						DttSP.NotPan = true;
						break;
				}*/

                switch (current_display_mode)
				{
					case DisplayMode.PHASE2:
						Audio.phase = true;
						break;
					default:
						Audio.phase = false;
						break;
				}

				if(average_on) ResetRX1DisplayAverage();
				if(peak_on) ResetRX1DisplayPeak();

				DrawBackground();
			}

        } // DisplayMode CurrentDisplayMode

        private static float max_x;								// x-coord of maxmimum over one display pass
		public static float MaxX
		{
			get { return max_x; }
			set { max_x = value; }
		}

		private static float max_y;								// y-coord of maxmimum over one display pass
		public static float MaxY
		{
			get { return max_y; }
			set { max_y = value; }
		}

		private static bool average_on;							// True if the Average button is pressed
		public static bool AverageOn
		{
			get { return average_on; }
			set 
			{
				average_on = value;
				if(!average_on) ResetRX1DisplayAverage();
			}
		}

		private static bool rx2_avg_on;
		public static bool RX2AverageOn
		{
			get { return rx2_avg_on; }
			set
			{
				rx2_avg_on = value;
				if(!rx2_avg_on) ResetRX2DisplayAverage();
			}
		}

		private static bool peak_on;							// True if the Peak button is pressed
		public static bool PeakOn
		{
			get { return peak_on; }
			set
			{
				peak_on = value;
				if(!peak_on) ResetRX1DisplayPeak();
			}
		}

		private static bool rx2_peak_on;
		public static bool RX2PeakOn
		{
			get { return rx2_peak_on; }
			set
			{
				rx2_peak_on = value;
				if(!rx2_peak_on) ResetRX2DisplayPeak();
			}
		}

		private static bool data_ready;					// True when there is new display data ready from the DSP
		public static bool DataReady
		{
			get { return data_ready; }
			set { data_ready = value; }
		}

		private static bool data_ready_bottom;
		public static bool DataReadyBottom
		{
			get { return data_ready_bottom; }
			set { data_ready_bottom = value; }
		}

		public static float display_avg_mult_old = 1 - (float)1/5;
		public static float display_avg_mult_new = (float)1/5;
		private static int display_avg_num_blocks = 5;

		public static int DisplayAvgBlocks   // ke9ns = (avgtime * .001)/(1/fps)  = 4 for my settings 
		{
			get { return display_avg_num_blocks; }
			set
			{
				display_avg_num_blocks = value;
				display_avg_mult_old = 1 - (float)1/display_avg_num_blocks;   // ke9ns   =  .75
				display_avg_mult_new = (float)1/display_avg_num_blocks;     // ke9ns = .25
			}
		}

		public static float waterfall_avg_mult_old = 1 - (float)1/18;  // ke9ns ?
		public static float waterfall_avg_mult_new = (float)1/18;
		private static int waterfall_avg_num_blocks = 18;
		public static int WaterfallAvgBlocks
		{
			get { return waterfall_avg_num_blocks; }
			set
			{
				waterfall_avg_num_blocks = value;
				waterfall_avg_mult_old = 1 - (float)1/waterfall_avg_num_blocks;
				waterfall_avg_mult_new = (float)1/waterfall_avg_num_blocks;
			}
		}



        private static int spectrum_grid_max1 = 0; // ke9ns add to adjust grid during transmit (this is just a holder of the original value to put back when done transmitting)
        private static int spectrum_grid_max = 0;
		public static int SpectrumGridMax
		{
			get{ return spectrum_grid_max;}
			set
			{
                spectrum_grid_max1 = spectrum_grid_max = value;
				DrawBackground();
			}
		}


        private static int spectrum_grid_min1 = -160; // ke9ns add
        private static int spectrum_grid_min = -160;
		public static int SpectrumGridMin
		{
			get{ return spectrum_grid_min;}
			set
			{
                spectrum_grid_min1 = spectrum_grid_min = value;
				DrawBackground();
			}
		}
        private static int spectrum_grid_step1 = 10;
        private static int spectrum_grid_step = 10;
		public static int SpectrumGridStep
		{
			get{ return spectrum_grid_step;}
			set
			{
                spectrum_grid_step1=  spectrum_grid_step = value;
				DrawBackground();
			}
		}

		private static Color grid_text_color = Color.Yellow;
		public static Color GridTextColor
		{
			get{ return grid_text_color;}
			set
			{
				grid_text_color = value;
				DrawBackground();
			}
		}

		private static Color grid_zero_color = Color.Red; // ke9ns this is the 0hz red line
		public static Color GridZeroColor
		{
			get{ return grid_zero_color;}
			set
			{
				grid_zero_color = value;
				DrawBackground();
			}
		}

		private static Color grid_color = Color.Purple;
		public static Color GridColor
		{
			get{ return grid_color;}
			set
			{
				grid_color = value;
				DrawBackground();
			}
		}

		private static Pen data_line_pen = new Pen(new SolidBrush(Color.White), display_line_width);
        private static Pen IDENT_pen = new Pen(new SolidBrush(Color.PaleGreen),1); // ke9ns add
        private static Pen IDENT_pen2 = new Pen(new SolidBrush(Color.PaleGoldenrod), 1); // ke9ns add
        private static Pen IDENT_pen3 = new Pen(new SolidBrush(Color.PaleVioletRed), 1); // ke9ns add

        private static Font IDENT_Font = new System.Drawing.Font("Arial", 12, FontStyle.Bold);// ke9ns add
        private static SolidBrush IDENT_Brush = new SolidBrush(Color.PaleVioletRed); // ke9ns add

        private static Color data_line_color = Color.White;
		public static Color DataLineColor
		{
			get{ return data_line_color;}
			set
			{
				data_line_color = value;
				data_line_pen = new Pen(new SolidBrush(data_line_color), display_line_width);
				DrawBackground();
			}
		}

		private static Color display_filter_color = Color.FromArgb(65, 255, 255, 255);
		public static Color DisplayFilterColor
		{
			get { return display_filter_color; }
			set
			{
				display_filter_color = value;
				DrawBackground();
			}
		}

        // ke9ns add for panadapter fill color and alpha
        private static Color display_pan_color = Color.FromArgb(70, Color.White);
        public static Color DisplayPanFillColor
        {
            get { return display_pan_color; }
            set
            {
                display_pan_color = value;
                DrawBackground();
            }
        }

        // ke9ns add  autobright                        helper to keep thread safe
        private static int wateroffset = 20;
        public static int WATEROFFSET
        {
            get { return wateroffset; }
            set
            {
                wateroffset = value;
                
            }
        }

        // ke9ns add autobright  
        private static int gridoffset = 20;
        public static int GRIDOFFSET
        {
            get { return gridoffset; }
            set
            {
                gridoffset = value;

            }
        }



        private static Color display_filter_tx_color = Color.Yellow;
		public static Color DisplayFilterTXColor
		{
			get { return display_filter_tx_color; }
			set
			{
				display_filter_tx_color = value;
				DrawBackground();
			}
		}

		private static bool draw_tx_filter = false;
		public static bool DrawTXFilter
		{
			get { return draw_tx_filter; }
			set
			{
				draw_tx_filter = value;
				DrawBackground();
			}
		}

		private static bool draw_tx_cw_freq = false;
		public static bool DrawTXCWFreq
		{
			get { return draw_tx_cw_freq; }
			set
			{
				draw_tx_cw_freq = value;
				DrawBackground();
			}
		}

		private static Color display_background_color = Color.Black;
		public static Color DisplayBackgroundColor
		{
			get { return display_background_color; }
			set
			{
				display_background_color = value;
				DrawBackground();
			}
		}
	
		private static Color waterfall_low_color = Color.Black;
		public static Color WaterfallLowColor
		{
			get { return waterfall_low_color; }
			set { waterfall_low_color = value; }
		}

		private static Color waterfall_mid_color = Color.Red;
		public static Color WaterfallMidColor
		{
			get { return waterfall_mid_color; }
			set { waterfall_mid_color = value; }
		}

		private static Color waterfall_high_color = Color.Yellow;
		public static Color WaterfallHighColor
		{
			get { return waterfall_high_color; }
			set { waterfall_high_color = value; }
		}

		private static float waterfall_high_threshold = -80.0F;
		public static float WaterfallHighThreshold
		{
			get { return waterfall_high_threshold; }
			set { waterfall_high_threshold = value; }
		}

		private static float waterfall_low_threshold = -130.0F;
		public static float WaterfallLowThreshold
		{
			get { return waterfall_low_threshold; }
			set { waterfall_low_threshold = value; }
       
          
        }

        private static float waterfall_lowRX2_threshold = -130.0F; // ke9ns ADD for RX2
        public static float WaterfallLowRX2Threshold
        {
            get { return waterfall_lowRX2_threshold; }
            set { waterfall_lowRX2_threshold = value; }
        }

        private static float waterfall_lowMic_threshold = -100.0F; //  ke9ns ADD
        public static float WaterfallLowMicThreshold
        {
            get { return waterfall_lowMic_threshold; }
            set { waterfall_lowMic_threshold = value; }
        }

        //================================================================
        // ke9ns add signal from console about Grayscale ON/OFF
        private static byte Gray_Scale = 0; //  ke9ns ADD from console 0=RGB  1=Gray
        public static byte GrayScale       // this is called or set in console
        {
            get { return Gray_Scale; }
            set {
                    Gray_Scale = value;
              } // set
        } // grayscale


        //================================================================
        // kes9ns add signal from setup grid lines on/off
        private static byte grid_off = 0; //  ke9ns ADD from setup 0=normal  1=gridlines off
        public static byte GridOff       // this is called or set in setup
        {
            get { return grid_off; }
            set
            {
                grid_off = value;
            } // set

        } // grid_off


        //================================================================
        //ke9ns 1=automatic bright adjustment (takes a snapshot of the overall avg and adjust RX1 and RX2 Low threshold for proper value, every time you push the button.)
        //      request comes from console.
        private static byte autobright = 0; //  ke9ns ADD from console rx1 waterfall bright adjust
        private static byte autobright2 = 0; //  ke9ns ADD from console rx2 waterfall bright adjust
        private static byte autobright3 = 0; //  ke9ns ADD from console TX waterfall bright adjust 1=adjust tx waterfall brightness
        private static byte autobright6 = 0; //  ke9ns ADD from console rx1 panadapter bright adjust 2=rx1 adjust
        private static byte autobright7 = 0; //  ke9ns ADD from console rx1 panadapter ZOOM scale adjust (for small signal or standard signal)


        public static byte AutoBright       // this is called or set in console 1=adjust waterfall, 2=adjust Pan level, 3=adjust Pan scale (small signal), 4=adjust Pan scale (standard)
        {
            get { return autobright; }
            set
            {
                if (mox)
                {
                    autobright3 = 1;   // ke9ns TX water adjust
                    autobright = autobright2 = 0;
                }
                else
                {
                    autobright3 = 0;

                   

                    if ( (console.chkPower.Checked) && ((current_display_mode == DisplayMode.PANADAPTER) || (current_display_mode == DisplayMode.PANAFALL)) )// PANADAPTER
                    {
                        if (value == 2)
                        {
                            autobright6 = value; // RX1 adjust 
                            return;
                        }
                        else if (value == 3) // ZOOM
                        {
                            autobright7 = value; // RX1 adjust small signal scale
                            return;
                        }
                        else if (value == 4) // UNZOOM
                        {
                            AB_Peak = -200;
                            AB_Count = 0;
                            AB_Total = 0;
                            autobright7 = value; // RX1 adjust standard signal scale
                            return;
                        }
                        else
                        {
                            autobright7 = 0;
                            autobright6 = 0;
                        }
                    }

                    if ((console.chkPower.Checked) && ((current_display_mode == DisplayMode.WATERFALL) || (current_display_mode == DisplayMode.PANAFALL)))
                    {
                        autobright = value; // RX1 adjust
                    }
                    else autobright = 0;

                    if ((console.chkRX2.Checked) && ((current_display_mode_bottom == DisplayMode.WATERFALL) || (current_display_mode_bottom == DisplayMode.PANAFALL)))
                    {
                        autobright2 = value;   // RX2 adjust
                    }
                    else autobright2 = 0;

                    
                }

             //   Debug.WriteLine("hereasdfadsf===========");

            } // set

        } // autobright


        //================================================================
        // signal from console about wider watermove ON/OFF
        private static byte wm = 0; //  ke9ns ADD from console 0=RGB  1=Gray
        public static byte WMS       // this is called or set in console
        {
            get { return wm; }
            set
            {
                if (value == 1)
                {
                    WaterMove = 5;
                    WaterMove1 = 2;
                    K9LAST = 0;
                }
                else
                {
                    WaterMove = 3;
                    WaterMove1 = 1;
                    K9LAST = 0;
                }


            } // set
        } // watermove

        //================================================================
        // signal from console chkPower switch ON/OFF
        private static byte DP = 0; //  ke9ns ADD  0=OFF, 1=ON
        public static byte Power           // this is called or set in console
        {
            get { return DP; }
            set
            {
                DP = value;

            } // set
        } // Power

        //================================================================
        // signal from console waterfall ID transmit
    //    private static byte TX_ID = 0; //  ke9ns ADD from console 0=off  1=TX
     //   public static byte TXID            // this is called or set in console
      //  {
          //  get { return TX_ID; }
          //  set
          //  {
             //   TX_ID = value;

          //  } // set
      //  } // tx id

        //================================================================
        // RX1 signal from console if "Panafall Mode" AVG mode  ( 0=no panafall mode and/or avg ON waterfall if its on panadater, 1=panafall mode and avg off in waterfall)
        private static byte pw_avg = 0; //  ke9ns ADD 
        public static  byte PW_AVG  // this is called or set in console
        {
            get { return pw_avg; }
            set
            {
                pw_avg = value;

            } // set
        } // pw_avg

        //================================================================
        // RX2 signal from console if "Panafall Mode" AVG mode 
        private static byte pw_avg2 = 0; //  ke9ns ADD 
        public static byte PW_AVG2  // this is called or set in console
        {
            get { return pw_avg2; }
            set
            {
                pw_avg2 = value;

            } // set
        } // pw_avg2


        private static float display_line_width = 1.0F;
		public static float DisplayLineWidth
		{
			get { return display_line_width; }
			set
			{
				display_line_width = value;
				data_line_pen = new Pen(new SolidBrush(data_line_color), display_line_width);
			}
		}

		private static DisplayLabelAlignment display_label_align = DisplayLabelAlignment.LEFT;
		public static DisplayLabelAlignment DisplayLabelAlign
		{
			get { return display_label_align; }
			set
			{
				display_label_align = value;
				DrawBackground();
			}
		}

		private static int phase_num_pts = 100;
		public static int PhaseNumPts
		{
			get{ return phase_num_pts;}
			set{ phase_num_pts = value;}
		}

		#endregion

		#region General Routines

		public static void Init()
		{
			histogram_data = new int[W];
			histogram_history = new int[W];
			for(int i=0; i < W; i++)
			{
				histogram_data[i] = Int32.MaxValue;
				histogram_history[i] = 0;
			}

            //display_bmp = new Bitmap(W, H);
            //display_graphics = Graphics.FromImage(display_bmp);


            //    waterfall_bmp = new Bitmap(W, H/K15 - 16, PixelFormat.Format24bppRgb);  // initialize waterfall display
            //    waterfall_bmp2 = new Bitmap(W, H/K15 - 16, PixelFormat.Format24bppRgb);  // ke9ns BMP

            if (WaterMove == 0)
            {
                waterfall_bmp = new Bitmap(W, H / K15 - 16, WtrColor);  // initialize waterfall display
                waterfall_bmp2 = new Bitmap(W, H / K15 - 16, WtrColor);  // ke9ns BMP
            }
            else if (continuum == 1)
            {
                waterfall_bmp = new Bitmap(W, H / K15 - 16, WtrColor);  // initialize waterfall display for continuum
                waterfall_bmp2 = new Bitmap(W * WaterMove, H / K15 - 16, WtrColor);  // was *3 ke9ns BMP

            }
            else
            { 
                waterfall_bmp = new Bitmap(W * WaterMove, H / K15 - 16, WtrColor);  // was *3 initialize waterfall display
                waterfall_bmp2 = new Bitmap(W * WaterMove, H / K15 - 16, WtrColor);  // was *3 ke9ns BMP

             //   waterfall_bmp.MakeTransparent(Color.FromArgb(0,0, 0, 0)); // ke9ns test


            }



            rx1_average_buffer = new float[BUFFER_SIZE];	// initialize averaging buffer array
			rx1_average_buffer[0] = CLEAR_FLAG;		// set the clear flag

			rx2_average_buffer = new float[BUFFER_SIZE];	// initialize averaging buffer array
			rx2_average_buffer[0] = CLEAR_FLAG;		// set the clear flag

			rx1_peak_buffer = new float[BUFFER_SIZE];
			rx1_peak_buffer[0] = CLEAR_FLAG;

			rx2_peak_buffer = new float[BUFFER_SIZE];
			rx2_peak_buffer[0] = CLEAR_FLAG;

			//background_image_mutex = new Mutex(false);

			new_display_data = new float[BUFFER_SIZE];
			current_display_data = new float[BUFFER_SIZE];
            current_display_data1 = new float[BUFFER_SIZE];  // ke9ns add
            new_display_data_bottom = new float[BUFFER_SIZE];
			current_display_data_bottom = new float[BUFFER_SIZE];
            current_display_data_bottom1 = new float[BUFFER_SIZE]; // ke9ns add

            for (int i=0; i < BUFFER_SIZE; i++) // fill the buffer with base data
			{
				new_display_data[i] = -200.0f;
                current_display_data1[i] = -200.0f;// ke9ns add
                current_display_data[i] = -200.0f;
				new_display_data_bottom[i] = -200.0f;
                current_display_data_bottom1[i] = -200.0f;// ke9ns add
                current_display_data_bottom[i] = -200.0f;
			}

			/*if(!DirectXInit()) console.SetupForm.DirectX = false;
			if(current_display_engine != DisplayEngine.DIRECT_X)
				DirectXRelease();*/

            channels_60m = new List<Channel>();

          

            switch (console.CurrentRegion)
            {
                case FRSRegion.UK_Plus:
                    /* channels_60m.Add(new Channel(5.2600, 2800));
                    channels_60m.Add(new Channel(5.2800, 2800));
                    channels_60m.Add(new Channel(5.2900, 2800));
                    channels_60m.Add(new Channel(5.3680, 2800));
                    channels_60m.Add(new Channel(5.3730, 2800));
                    channels_60m.Add(new Channel(5.4000, 2800));
                    channels_60m.Add(new Channel(5.4050, 2800));
                    */
                    break;
                //  5.351500, 5.335999, "60M 200hz Narrow Band Modes",    true,
                //          5.354000, 5.356999, "60M USB Voice",            true,
                //          5.357000, 5.359999, "60M USB Voice (US CH 3)",  true,
                //         5.360000, 5.362999, "60M USB Voice",            true,
                //          5.363000, 5.365999, "60M USB Voice",            true,
                //         5.366000, 5.366500, "60M 20hz Narrow Band Modes",    true,

                case FRSRegion.US: 

                    // list center of channel (stupid)
                    channels_60m.Add(new Channel(5.1690, 2800)); // ke9ns emergency only   5.1675

                    channels_60m.Add(new Channel(5.3320, 2800)); // channel 1   5.3305
                    channels_60m.Add(new Channel(5.3480, 2800)); // channel 2   5.3465
                    channels_60m.Add(new Channel(5.3585, 2800)); // channel 3   5.3570 
                    channels_60m.Add(new Channel(5.3730, 2800)); // channel 4   5.3715
                    channels_60m.Add(new Channel(5.4050, 2800)); // channel 5   5.4035

              



                    break;

                default:
                    /*
                    channels_60m.Add(new Channel(5.3320, 2800));
                    channels_60m.Add(new Channel(5.3480, 2800));
                    channels_60m.Add(new Channel(5.3585, 2800));
                    channels_60m.Add(new Channel(5.3730, 2800));
                    channels_60m.Add(new Channel(5.4050, 2800));
                     */
                    break;
            }

         

        } // init()

        // ke9ns add
     

        public static void DrawBackground()
		{
			// draws the background image for the display based
			// on the current selected display mode.

			if(current_display_engine == DisplayEngine.GDI_PLUS)
			{
				/*switch(current_display_mode)
				{
					case DisplayMode.SPECTRUM:
						DrawSpectrumGrid(ref background_bmp, W, H);
						break;
					case DisplayMode.PANADAPTER:
						DrawPanadapterGrid(ref background_bmp, W, H);
						break;
					case DisplayMode.SCOPE:
						DrawScopeGrid(ref background_bmp, W, H);
						break;
					case DisplayMode.PHASE:
						DrawPhaseGrid(ref background_bmp, W, H);
						break;	
					case DisplayMode.PHASE2:
						DrawPhaseGrid(ref background_bmp, W, H);
						break;
					case DisplayMode.WATERFALL:
						DrawWaterfallGrid(ref background_bmp, W, H);
						break;
					case DisplayMode.HISTOGRAM:
						DrawSpectrumGrid(ref background_bmp, W, H);
						break;
					case DisplayMode.OFF:
						DrawOffBackground(ref background_bmp, W, H);
						break;
					default:
						break;
				}
*/
				target.Invalidate();
			}
			/*else if(current_display_engine == DisplayEngine.DIRECT_X)
			{
				switch(current_display_mode)
				{
					case DisplayMode.SPECTRUM:
						current_background = SetupSpectrum();
						break;
					case DisplayMode.PANADAPTER:
						break;
					case DisplayMode.SCOPE:
						current_background = SetupScope();
						break;
					case DisplayMode.PHASE:
						break;	
					case DisplayMode.PHASE2:
						break;
					case DisplayMode.WATERFALL:
						break;
					case DisplayMode.HISTOGRAM:
						break;
					case DisplayMode.OFF:
						break;
					default:
						break;
				}				
				
				// redraw screen now if not starting up and if in standby
				//if(console.SetupForm != null && !console.PowerOn) RenderDirectX();
			}*/
		} // drawbackground

#if (!NO_TNF)
        // This draws a little callout on the notch to show it's frequency and bandwidth
        // xlimit is the right side of the picDisplay

        static Color c = Color.DarkOliveGreen;
        static Pen p = new Pen(Color.DarkOliveGreen, 1);
        static Brush b = new SolidBrush(Color.Chartreuse);

        private static void drawNotchStatus(Graphics g, Notch n, int x, int y, int x_limit, int height)
        {

            // if we're not supposed to be drawing this, return to caller
            if (!n.Details) return;
            // in case notch is showing on RX1 & RX2, just show it for the one that was clicked
            if ((y < height && n.RX == 2) || (y > height && n.RX == 1)) return;
            // first we need to test if it is OK to draw the box to the right of the notch ... I don't
            // know the panadapter limits in x, so I will use a constant.  This needs to be replaced
            int x_distance_from_notch = 40;
            int y_distance_from_bot = 20;
            int box_width = 120;
            int box_height = 55;
            int x_start, y_start, x_pin, y_pin;
            // determine if it will fit in the panadapter to the right of the notch
            if (x + box_width + x_distance_from_notch > x_limit)
            {
                // draw to the left
                x_pin = x - x_distance_from_notch;
                y_pin = y - y_distance_from_bot;
                x_start = x_pin - box_width;
                y_start = y_pin - (box_height / 2);
            }
            else
            {
                // draw to the right
                x_start = x + x_distance_from_notch;
                x_pin = x_start;
                y_pin = y - y_distance_from_bot;
                y_start = y_pin - (box_height / 2);
            }

            // such pretty colors of green, hardcoded for your viewing pleasure

          //  Color c = Color.DarkOliveGreen;
         //   Pen p = new Pen(Color.DarkOliveGreen, 1);
         //   Brush b = new SolidBrush(Color.Chartreuse);

            // Draw a nice rectangle to write into
            g.FillRectangle(new SolidBrush(c), x_start, y_start, box_width, box_height);
            // draw a left and right line on the side of the rectancle
            g.DrawLine(p, x, y, x_pin, y_pin);
            // get the Hz part of the frequency because we want to set it off from the actual number so it looks neato
            int right_three = (int)(n.Freq * 1e6) - (int)(n.Freq * 1e3) * 1000;
            double left_three = (((int)(n.Freq * 1e3)) / 1e3);
            //string perm = n.Permanent ? "*" : "";
            g.DrawString("RF Tracking Notch", // + perm,
                new Font("Trebuchet MS", 9, FontStyle.Underline),
                b, new Point(x_start + 5, y_start + 5));
            g.DrawString(left_three.ToString("f3") + " " + right_three.ToString("d3") + " MHz",
                new Font("Trebuchet MS", 9, FontStyle.Regular),
                b, new Point(x_start + 5, y_start + 20));
            g.DrawString(n.BW.ToString("d") + " Hz wide",
                new Font("Trebuchet MS", 9, FontStyle.Regular),
                b, new Point(x_start + 5, y_start + 35));
        }

        /// <summary>
        /// draws the vertical bar to highlight where a notch is on the panadapter
        /// </summary>
        /// <param name="g">Graphics object reference</param>
        /// <param name="n">Notch object reference</param>
        /// <param name="left">left side of notch in pixel location</param>
        /// <param name="right">right side of notch, pixel location</param>
        /// <param name="top">top of bar</param>
        /// <param name="H">height of bar</param>
        /// <param name="on">color for notch on</param>
        /// <param name="off">color for notch off</param>
        /// <param name="highlight">highlight color to draw highlights on bar</param>
        /// <param name="active">true if notches are turned on</param>
        static void drawNotchBar(Graphics g, Notch n, int left, int right, int top, int height, Color c, Color h)
        {
            int width = right - left;
            int hash_spacing_pixels = 1;
            switch (n.Depth)
            {
                case 1:
                    hash_spacing_pixels = 12;
                    break;
                case 2:
                    hash_spacing_pixels = 8;
                    break;
                case 3:
                    hash_spacing_pixels = 4;
                    break;
            }            

            // get a purty pen to draw with 
            Pen p = new Pen(h, 1);

            // shade in the notch
            g.FillRectangle(new SolidBrush(c), left, top, width, height);

            // draw a left and right line on the side of the rectancle if wide enough
            if (width > 2 && tnf_active)
            {
                g.DrawLine(p, left, top, left, top + height - 1);
                g.DrawLine(p, right, top, right, top + height - 1);

                // first draw down left side of notch indicator horizontal lines -- a series of 45-degree hashes
                for (int y = top + hash_spacing_pixels; y < top + height - 1 + width; y += hash_spacing_pixels)
                {
                    int start_y = y;
                    int start_x = left;
                    int end_x = right;
                    int end_y = start_y - width;

                    int min_y = top;
                    int _max_y = top + height - 1;

                    // if we are about to over-draw past the top of the rectangle, we must restrain ourselves!
                    if (end_y < min_y)
                    {
                        end_x -= (min_y - end_y);
                        end_y = top;
                    }

                    // if we are about to over-draw past the bottom of the rectangle, we must restrain ourselves!
                    if (start_y > _max_y)
                    {
                        start_x += (start_y - _max_y); 
                        start_y = _max_y;                        
                    }

                    g.DrawLine(p, start_x, start_y, end_x, end_y);
                }
            }
        }
#endif
        /// <summary>
        /// draws the vertical bar to highlight where a channel is on the panadapter
        /// </summary>
        /// <param name="g">Graphics object reference</param>
        /// <param name="n">Channel object reference</param>
        /// <param name="left">left side of notch in pixel location</param>
        /// <param name="right">right side of notch, pixel location</param>
        /// <param name="top">top of bar</param>
        /// <param name="H">height of bar</param>
        /// <param name="on">color for notch on</param>
        /// <param name="off">color for notch off</param>
        /// <param name="highlight">highlight color to draw highlights on bar</param>
        /// <param name="active">true if notches are turned on</param>
        static void drawChannelBar(Graphics g, Channel chan, int left, int right, int top, int height, Color c, Color h)
        {
            int width = right - left;

            // get a purty pen to draw with 
            Pen p = new Pen(h, 1);

            // shade in the notch
            g.FillRectangle(new SolidBrush(c), left, top, width, height);

            // draw a left and right line on the side of the rectancle if wide enough
            if (width > 2)
            {
                //g.DrawLine(p, left - 1, top, left - 1, top + height - 1);
                g.DrawLine(p, left, top, left, top + height - 1);                
                g.DrawLine(p, right, top, right, top + height - 1);
                //g.DrawLine(p, right+1, top, right+1, top + height - 1);
            }
        }

		#endregion

		#region GDI+

		unsafe public static void RenderGDIPlus(ref PaintEventArgs e)
		{

         
            /*BitmapData display_bmpData = display_bmp.LockBits(
				new Rectangle(0, 0, W, H),
				ImageLockMode.WriteOnly,
				display_bmp.PixelFormat);

			background_image_mutex.WaitOne();			// get background image

			BitmapData background_bmpData = background_bmp.LockBits(
				new Rectangle(0, 0, background_bmp.Width, background_bmp.Height),
				ImageLockMode.ReadOnly,
				background_bmp.PixelFormat);
				
			int total_size = background_bmpData.Stride * background_bmpData.Height;		// find buffer size

			byte *srcptr = (byte *)background_bmpData.Scan0.ToPointer();
			byte *destptr = (byte *)display_bmpData.Scan0.ToPointer();

			Win32.memcpy(destptr, srcptr, total_size);

			background_bmp.UnlockBits(background_bmpData);
			background_image_mutex.ReleaseMutex();

			display_bmp.UnlockBits(display_bmpData);*/

            //Graphics g = Graphics.FromImage(display_bitmap);
            //g.SmoothingMode = SmoothingMode.AntiAlias;
            bool update = true;
			
//======================================================
// ke9ns RX1 only TOP of panel to bottom of panel H
//======================================================

            K13 = H;  // used to create special BMP file size

            /*
            if (console.N1MM_MINIMIZE == true)
            {

                if (console.N1MM_ON == true)
                {
                    console.N1MM_Sample = W;
                    console.N1MM_Low = (int)((vfoa_hz + Low) / 1000);  //rx_display_low;
                    console.N1MM_High = (int)((vfoa_hz + High) / 1000);

                }

                //=================================================================
                // draw line that makes up spectrum (width of window)
                //=================================================================
                for (int i = 0; i < W; i++)
                {
                    float max = float.MinValue;                             // max = y point determined by RX data of spectrum as you go from 0 to W
                    float dval = i * slope + start_sample_index;            // dval = how many digital values per pixel (going left to right)
                    int lindex = (int)Math.Floor(dval);                     // L index = int of dval
                    int rindex = (int)Math.Floor(dval + slope);             // R index = int of dval + slope ?



                    if (rx == 1)
                    {

                        if (slope <= 1.0 || lindex == rindex)   // if your zoom in there is less than 1 digital value per pixel so fake it.
                        {
                            max = current_display_data[lindex % DATA_BUFFER_SIZE] * ((float)lindex - dval + 1) + current_display_data[(lindex + 1) % DATA_BUFFER_SIZE] * (dval - (float)lindex);
                        }
                        else // otherwise there is more than 1 digital value per pixel so take the largest value ?
                        {
                            for (int j = lindex; j < rindex; j++)
                                if (current_display_data[j % DATA_BUFFER_SIZE] > max) max = current_display_data[j % DATA_BUFFER_SIZE]; // % modulus (i.e. remainder only)
                        }



                    } // rx1
                    else if (rx == 2)
                    {
                        if (slope <= 1.0 || lindex == rindex)
                        {
                            max = current_display_data_bottom[lindex % DATA_BUFFER_SIZE] * ((float)lindex - dval + 1) + current_display_data_bottom[(lindex + 1) % DATA_BUFFER_SIZE] * (dval - (float)lindex);
                        }
                        else
                        {
                            for (int j = lindex; j < rindex; j++)
                                if (current_display_data_bottom[j % DATA_BUFFER_SIZE] > max) max = current_display_data_bottom[j % DATA_BUFFER_SIZE];
                        }
                    } // rx2

                    if (rx == 1) max += rx1_display_cal_offset;
                    else if (rx == 2) max += rx2_display_cal_offset;

                    if (!local_mox)
                    {
                        if (rx == 1) max += rx1_preamp_offset;
                        else if (rx == 2) max += rx2_preamp_offset;
                    }

                    if (max > local_max_y)
                    {
                        local_max_y = max;
                        max_x = i;
                    }


                    //=====================================================
                    // ke9ns add 
                    if ((console.N1MM_ON == true) && (!mox))
                    {
                        console.N1MM_Data[i] = (int)max;

                        if ((int)max < console.N1MM_Floor) console.N1MM_Floor = (int)max;

                    } // if (console.N1MM_ON == true)


                } // for (int i = 0; i < W; i++)

            } //  if (console.N1MM_MINIMIZE == true)
            */

            if (!split_display)
			{
				switch(current_display_mode) // ke9ns full screen display (only RX1)
				{
					case DisplayMode.SPECTRUM:
                        K9 = 4;
                        K11 = 0;
						update = DrawSpectrum(e.Graphics, W, H, false);
						break;
					case DisplayMode.PANADAPTER:
                        K9 = 2;
                        K11 = 0;
                     
                        update = DrawPanadapter(e.Graphics, W, H, 1, false);
						break;
					case DisplayMode.SCOPE:
                        K9 = 4;
                        K11 = 0;
						update = DrawScope(e.Graphics, W, H, false);
						break;
					case DisplayMode.PHASE:
                        K9 = 4;
                        K11 = 0;
						update = DrawPhase(e.Graphics, W, H, false);
						break;
					case DisplayMode.PHASE2:
                        K9 = 4;
                        K11 = 0;
						DrawPhase2(e.Graphics, W, H, false);
						break;
					case DisplayMode.WATERFALL:   // RX1: full H waterfall
                        K9 = 1;
                        K11 = 0;
                     	update = DrawWaterfall(e.Graphics, W, H, 1, false); // ke9ns was just H, false
						break;
					case DisplayMode.HISTOGRAM:
                        K9 = 4;
                        K11 = 0;
						update = DrawHistogram(e.Graphics, W, H);
						break;
					case DisplayMode.PANAFALL:

                      
                        if (map == 1) // ke9ns add  if in special map viewing panafall mode
                        {
                           
                            K9 = 7;             //special panafall mode for sun/grayline tracking mode
                            K11 = 0;

                          
                            update = DrawPanadapter(e.Graphics, W, 5 * H / 6, 1, false);    //     in pure panadapter mode: update = DrawPanadapter(e.Graphics, W, H, 1, false);
                            update = DrawWaterfall(e.Graphics, W, 5 * H / 6, 1, true);        // bottom half RX2 is not on
                            split_display = false;
                        }
                        else
                       {
                            K9 = 3;
                            K11 = 0;

                            split_display = true; // use wide vertgrid because your saying split
                            update = DrawPanadapter(e.Graphics, W, H / 2, 1, false); //top half 
                            update = DrawWaterfall(e.Graphics, W, H / 2, 1, true); // bottom half RX2 is not on
                            split_display = false;
                       }

						break;

					case DisplayMode.PANASCOPE:
                        K9 = 4;
                        K11 = 0;
						split_display = true;
						update = DrawPanadapter(e.Graphics, W, H/2, 1, false);
						update = DrawScope(e.Graphics, W, H/2, true);
						split_display = false;
						break;
					case DisplayMode.OFF:
                        K9 = 0;
                        K11 = 0;
						//Thread.Sleep(1000);
						break;
					default:
						break;
				}
			} // !split_display
			else
			{

        //======================================================
        // ke9ns RX1 TOP of panel and RX2 bottom of panel H
        // this is only RX1 here, RX2 is futher down below
        //======================================================
                
				switch(current_display_mode) // ke9ns split display (RX1 top  and RX2 on bottom)
				{
					case DisplayMode.SPECTRUM:
                          K9 = 4;
                          K11 = 0;
						update = DrawSpectrum(e.Graphics, W, H/2, false);
						break;
					case DisplayMode.PANADAPTER:
                        K9 = 2;
                        K11 = 0;
						update = DrawPanadapter(e.Graphics, W, H/2, 1, false); //ke9ns just as original
						break;
					case DisplayMode.SCOPE:
                          K9 = 4;
                          K11 = 0;
						update = DrawScope(e.Graphics, W, H/2, false);
						break;
					case DisplayMode.PHASE:
                          K9 = 4;
                          K11 = 0;
						update = DrawPhase(e.Graphics, W, H/2, false);
						break;
					case DisplayMode.PHASE2:
                          K9 = 4;
                          K11 = 0;
						DrawPhase2(e.Graphics, W, H/2, false);
						break;
					case DisplayMode.WATERFALL:
                        K9 = 6;
                        K11 = 0;
						update = DrawWaterfall(e.Graphics, W, H/2, 1, false);  // ke9ns was /2
						break;

					case DisplayMode.HISTOGRAM:
                          K9 = 4;
                          K11 = 0;
						update = DrawHistogram(e.Graphics, W, H/2);
						break;
                    
                    case DisplayMode.PANAFALL:   // ke9ns pan rX1 (KE9NS ADDED CODE)
                        K9 = 5;
                        K11 = 5;
                    
                        switch (current_display_mode_bottom)  // ke9ns check RX2 to see what to do with both RX1 and RX2
                        {
                            case DisplayMode.PANADAPTER:
                                K10 = 2;
                                 update = DrawPanadapter(e.Graphics, W, H/3, 1, false); // RX1 panadapter top 1/3
                                 update = DrawWaterfall(e.Graphics, W, H/3, 1, true);     // RX1 waterfall middle 1/3

                              	update = DrawPanadapter(e.Graphics, W, 2*H/3, 2, true); // RX2  bottom 1/3
					
                                break;

                            case DisplayMode.WATERFALL:
                                K10 = 1;

                                 update = DrawPanadapter(e.Graphics, W, H/3, 1, false); // RX1 panadapter top 1/3
                                 update = DrawWaterfall(e.Graphics, W, H/3, 1, true);     // RX1 waterfall middle 1/3

                                update = DrawWaterfall(e.Graphics, W, 2*H/3, 2, true);  // RX2 bottom 1/3

                                break;
                            case DisplayMode.PANAFALL:   // ke9ns pan (KE9NS ADDED CODE)  rx2 panafall with RX1 panafall as well
                                K10 = 5;
                                 update = DrawPanadapter(e.Graphics, W, H/4, 1, false); // RX1 panadapter top 1/4
                                 update = DrawWaterfall(e.Graphics, W, H/4, 1, true);     // RX1 waterfall middle 1/4

                                 update = DrawPanadapter(e.Graphics, W, 2*H/4, 2, true);
                                 update = DrawWaterfall(e.Graphics, W, 3*H/4, 2, true);
                      
                                break;

                            case DisplayMode.OFF:
                                K10 = 0;
                                DrawOffBackground(e.Graphics, W, H / 2, true);
                                K9 = 3;
                                K11 = 0;
                          
                                split_display = true; // use wide vertgrid because your saying split
						        update = DrawPanadapter(e.Graphics, W, H/2, 1, false); //top half 
						        update = DrawWaterfall(e.Graphics, W, H/2, 1, true); // bottom half RX2 is not on
						        split_display = false;

                            break; // rx2 off


                        } // switch (current_display_mode_bottom)
				      
                    break;  // rx1 panafall


					case DisplayMode.OFF:
                        K9 = 0;
                        K11 = 0;
						DrawOffBackground(e.Graphics, W, H/2, false);
						break;
    

					default:
                  
						break;

                } // RX1 switch(current_display_mode)   ke9ns split display (RX1 top  and RX2 on bottom)

                //=========================================
                // ke9ns display RX2 on bottom of screen
                //=========================================
                if (K11 == 0) //if rx1 is in panafall skip below
                {
                    switch (current_display_mode_bottom)  // ke9ns pan
                    {
                        case DisplayMode.SPECTRUM:
                            K10 = 0;
                            update = DrawSpectrum(e.Graphics, W, H / 2, true);   // RX1 on bottom half of screen
                            break;
                        case DisplayMode.PANADAPTER:
                            K10 = 2;
                            update = DrawPanadapter(e.Graphics, W, H / 2, 2, true); // RX2  (standard mode)
                            break;
                        case DisplayMode.SCOPE:
                            K10 = 0;
                            update = DrawScope(e.Graphics, W, H / 2, true);     // RX1 on bottom half of screen
                            break;
                        case DisplayMode.PHASE:
                            K10 = 0;
                            update = DrawPhase(e.Graphics, W, H / 2, true);  // RX1 on bottom half of screen
                            break;
                        case DisplayMode.PHASE2:
                            K10 = 0;
                            DrawPhase2(e.Graphics, W, H / 2, true);  // RX1 on bottom half of screen
                            break;

                        case DisplayMode.WATERFALL:
                            K10 = 1;
                            update = DrawWaterfall(e.Graphics, W, H / 2, 2, true);  // RX2
                            break;

                        case DisplayMode.HISTOGRAM:
                            K10 = 0;
                            update = DrawHistogram(e.Graphics, W, H / 2);  // RX1 on bottom half of screen
                            break;
                        case DisplayMode.OFF:
                            K10 = 0;
                            DrawOffBackground(e.Graphics, W, H / 2, true);
                         
                            switch (current_display_mode) // ke9ns split display (RX1 top  and RX2 on bottom)
                            {

                                case DisplayMode.PANAFALL:
                                    K9 = 3;
                                    K11 = 0;

                                    split_display = true; // use wide vertgrid because your saying split
                                    update = DrawPanadapter(e.Graphics, W, H / 2, 1, false); //top half 
                                    update = DrawWaterfall(e.Graphics, W, H / 2, 1, true); // bottom half RX2 is not on
                                    split_display = false;
                             
                                    break;
                            }


                            break; // RX2 OFF

                        case DisplayMode.PANAFALL:
                            K10 = 2;
                            update = DrawPanadapter(e.Graphics, W, H / 2, 2, true); // RX2  (standard mode)
                            break;
                        default:
                             K10 = 2;
                            update = DrawPanadapter(e.Graphics, W, H / 2, 2, true); // RX2  (standard mode)
                            break;

                           
                    } // switch(current_display_mode_bottom)
                } // K11 == 0
                else // rx1 in panafall mode
                {
                    switch (current_display_mode_bottom)  // ke9ns pan
                    {

                        case DisplayMode.OFF:
                            K10 = 0;
                         
                            DrawOffBackground(e.Graphics, W, H / 2, true);
  
                            break; // RX2 OFF

                    } // check rx2


                } // K11==5

			} // split_display


			if(update)
			{
				//e.Graphics.DrawImage(display_bmp, 0, 0);
			}
			else
			{
				Debug.WriteLine("display update = false");
			}


            if (Console.CTUN == true)
            {
                if (Console.UPDATEOFF > 0) Console.UPDATEOFF--;
                Debug.WriteLine("UPDATEOFF--------------" + Console.UPDATEOFF);

            }

		} // renderGDIPLUs


		private static void UpdateDisplayPeak(float[] buffer, float[] new_data) 
		{
			if(buffer[0] == CLEAR_FLAG)
			{
				//Debug.WriteLine("Clearing peak buf"); 
				for(int i=0; i < BUFFER_SIZE; i++)
					buffer[i] = new_data[i];
			}
			else
			{
				for(int i=0; i < BUFFER_SIZE; i++)
				{
					if(new_data[i] > buffer[i])
						buffer[i] = new_data[i];
					new_data[i] = buffer[i];
				}
			}
		}

		#region Drawing Routines
		// ======================================================
		// Drawing Routines
		// ======================================================

		
		private static void DrawPhaseGrid(ref Graphics g, int W, int H, bool bottom)
		{
			// draw background
			if(bottom) g.FillRectangle(new SolidBrush(display_background_color), 0, H, W, H);
			else g.FillRectangle(new SolidBrush(display_background_color), 0, 0, W, H);

			for(double i=0.50; i < 3; i+=.50)	// draw 3 concentric circles
			{
				if(bottom) g.DrawEllipse(new Pen(grid_color), (int)(W/2-H*i/2), H+(int)(H/2-H*i/2), (int)(H*i), (int)(H*i));
				else g.DrawEllipse(new Pen(grid_color), (int)(W/2-H*i/2), (int)(H/2-H*i/2), (int)(H*i), (int)(H*i));
			}

			if(high_swr && !bottom)
				g.DrawString("High SWR", new System.Drawing.Font("Arial", 14, FontStyle.Bold), new SolidBrush(Color.Red), 245, 20);
		}

		private static void DrawScopeGrid(ref Graphics g, int W, int H, bool bottom)
		{
			// draw background
			//if(bottom) g.FillRectangle(new SolidBrush(display_background_color), 0, H, W, H);
			//else g.FillRectangle(new SolidBrush(display_background_color), 0, 0, W, H);

			if(bottom)
			{
				g.DrawLine(new Pen(grid_color), 0, H+H/2, W, H+H/2);	// draw horizontal line
				g.DrawLine(new Pen(grid_color), W/2, H, W/2, H+H);	// draw vertical line
			}
			else
			{
				g.DrawLine(new Pen(grid_color), 0, H/2, W, H/2);	// draw horizontal line
				g.DrawLine(new Pen(grid_color), W/2, 0, W/2, H);	// draw vertical line
			}

			if(high_swr && !bottom)
				g.DrawString("High SWR", new System.Drawing.Font("Arial", 14, FontStyle.Bold), new SolidBrush(Color.Red), 245, 20);
		}

//================================================
// ke9ns spectrum
//================================================


		private static void DrawSpectrumGrid(ref Graphics g, int W, int H, bool bottom)
		{
			System.Drawing.Font font = new System.Drawing.Font("Swis721 BT", 9, FontStyle.Italic);
			SolidBrush grid_text_brush = new SolidBrush(grid_text_color);
			Pen grid_pen = new Pen(grid_color);

			// draw background
			if(bottom) g.FillRectangle(new SolidBrush(display_background_color), 0, H, W, H);
			else g.FillRectangle(new SolidBrush(display_background_color), 0, 0, W, H);

			int low = 0;								// init limit variables
			int high = 0;

			int center_line_x = (int)(-(double)low / (high-low) * W);

			if(!mox)
			{
				low = rx_display_low;				// get RX display limits  based on sample rate  (left to right freq range)
				high = rx_display_high;
			}
			else
			{
				if(rx1_dsp_mode == DSPMode.CWL || rx1_dsp_mode == DSPMode.CWU)
				{
					low = rx_display_low;
					high = rx_display_high;
				}
				else
				{
					low = tx_display_low;			// get RX display limits
					high = tx_display_high;
				}
			}

			int mid_w = W/2;
			int[] step_list = {10, 20, 25, 50};
			int step_power = 1;
			int step_index = 0;
			int freq_step_size = 50;
			int y_range = spectrum_grid_max - spectrum_grid_min;
			int grid_step = spectrum_grid_step;

			if(split_display) grid_step *= 2;

			if(high == 0)
			{
				int f = -low;
				// Calculate horizontal step size
				while(f/freq_step_size > 7)
				{
					freq_step_size = step_list[step_index]*(int)Math.Pow(10.0, step_power);
					step_index = (step_index+1)%4;
					if(step_index == 0) step_power++;
				}
				float pixel_step_size = (float)(W*freq_step_size/f);

				int num_steps = f/freq_step_size;

				// Draw vertical lines
				for(int i=1; i<=num_steps; i++)
				{
					int x = W-(int)Math.Floor(i*pixel_step_size);   // for negative numbers

                   
                        if (bottom) g.DrawLine(grid_pen, x, H, x, H + H);
                        else g.DrawLine(grid_pen, x, 0, x, H);              // draw right line
                   

					// Draw vertical line labels
					int num = i*freq_step_size;
					string label = num.ToString();
					int offset = (int)((label.Length+1)*4.1);
					if(x-offset >= 0)
					{
						if(bottom) g.DrawString("-"+label, font, grid_text_brush, x-offset, H+(float)Math.Floor(H*.01));
						else g.DrawString("-"+label, font, grid_text_brush, x-offset, (float)Math.Floor(H*.01));
					}
				}

				// Draw horizontal lines
				int V = (int)(spectrum_grid_max - spectrum_grid_min);
				num_steps = V/grid_step;
				pixel_step_size = H/num_steps;

				for(int i=1; i<num_steps; i++)
				{
					int xOffset = 0;
					int num = spectrum_grid_max - i*grid_step;
					int y = (int)Math.Floor((double)(spectrum_grid_max - num)*H/y_range);

					if(bottom) g.DrawLine(grid_pen, 0, H+y, W, H+y);
					else g.DrawLine(grid_pen, 0, y, W, y);

					// Draw horizontal line labels
                    if (i != 1) // avoid intersecting vertical and horizontal labels
                    {
                        string label = num.ToString();
                        if (label.Length == 3)   xOffset = (int)g.MeasureString("-", font).Width - 2;
                        int offset = (int)(label.Length * 4.1);
                        SizeF size = g.MeasureString(label, font);

                        int x = 0;
                        switch (display_label_align)
                        {
                            case DisplayLabelAlignment.LEFT:
                                x = xOffset + 3;
                                break;
                            case DisplayLabelAlignment.CENTER:
                                x = center_line_x + xOffset;
                                break;
                            case DisplayLabelAlignment.RIGHT:
                                x = (int)(W - size.Width - 3);
                                break;
                            case DisplayLabelAlignment.AUTO:
                                x = xOffset + 3;
                                break;
                            case DisplayLabelAlignment.Sunit: // ke9ns add
                                x = xOffset + 3;
                                break;
                            case DisplayLabelAlignment.OFF:
                                x = W;
                                break;
                        }

                        y -= 8;
                        if (y + 9 < H)
                        {
                            if (bottom) g.DrawString(label, font, grid_text_brush, x, H + y);
                            g.DrawString(label, font, grid_text_brush, x, y);
                        }
                    }
				}

				// Draw middle vertical line
				if(bottom)
				{
					g.DrawLine(new Pen(grid_zero_color), W-1, H, W-1, H+H);
					g.DrawLine(new Pen(grid_zero_color), W-2, H, W-2, H+H);
				}
				else
				{
					g.DrawLine(new Pen(grid_zero_color), W-1, 0, W-1, H);
					g.DrawLine(new Pen(grid_zero_color), W-2, 0, W-2, H);
				}
			}
			else if(low == 0)
			{
				int f = high;
				// Calculate horizontal step size
				while(f/freq_step_size > 7)
				{
					freq_step_size = step_list[step_index]*(int)Math.Pow(10.0, step_power);
					step_index = (step_index+1)%4;
					if(step_index == 0) step_power++;
				}
				float pixel_step_size = (float)(W*freq_step_size/f);
				int num_steps = f/freq_step_size;

				// Draw vertical lines
				for(int i=1; i<=num_steps; i++)
				{
					int x = (int)Math.Floor(i*pixel_step_size);// for positive numbers
					
					if(bottom) g.DrawLine(grid_pen, x, H, x, H+H);
					else g.DrawLine(grid_pen, x, 0, x, H);				// draw right line
				
					// Draw vertical line labels
					int num = i*freq_step_size;
					string label = num.ToString();
					int offset = (int)(label.Length*4.1);
					if(x-offset+label.Length*7 < W)
					{
						if(bottom) g.DrawString(label, font, grid_text_brush, x-offset, H+(float)Math.Floor(H*.01));
						else g.DrawString(label, font, grid_text_brush, x-offset, (float)Math.Floor(H*.01));
					}
				}

				// Draw horizontal lines
				int V = (int)(spectrum_grid_max - spectrum_grid_min);
				int numSteps = V/grid_step;
				pixel_step_size = H/numSteps;
				for(int i=1; i<numSteps; i++)
				{
					int xOffset = 0;
					int num = spectrum_grid_max - i*grid_step;
					int y = (int)Math.Floor((double)(spectrum_grid_max - num)*H/y_range);

					if(bottom) g.DrawLine(grid_pen, 0, H+y, W, H+y);
					else g.DrawLine(grid_pen, 0, y, W, y);

					// Draw horizontal line labels
                    if (i != 1) // avoid intersecting vertical and horizontal labels
                    {
                        string label = num.ToString();
                        if (label.Length == 3)
                            xOffset = (int)g.MeasureString("-", font).Width - 2;
                        int offset = (int)(label.Length * 4.1);
                        SizeF size = g.MeasureString(label, font);

                        int x = 0;
                        switch (display_label_align)
                        {
                            case DisplayLabelAlignment.LEFT:
                                x = xOffset + 3;
                                break;
                            case DisplayLabelAlignment.CENTER:
                                x = center_line_x + xOffset;
                                break;
                            case DisplayLabelAlignment.RIGHT:
                                x = (int)(W - size.Width - 3);
                                break;
                            case DisplayLabelAlignment.AUTO:
                                x = xOffset + 3;
                                break;
                            case DisplayLabelAlignment.Sunit: // ke9ns add
                                x = xOffset + 3;
                                break;
                            case DisplayLabelAlignment.OFF:
                                x = W;
                                break;
                        }

                        y -= 8;
                        if (y + 9 < H)
                        {
                            if (bottom) g.DrawString(label, font, grid_text_brush, x, H + y);
                            g.DrawString(label, font, grid_text_brush, x, y);
                        }
                    }
				}

				// Draw middle vertical line
				if(bottom)
				{
					g.DrawLine(new Pen(grid_zero_color), 0, H, 0, H+H);
					g.DrawLine(new Pen(grid_zero_color), 1, H, 1, H+H);
				}
				else
				{
					g.DrawLine(new Pen(grid_zero_color), 0, 0, 0, H);
					g.DrawLine(new Pen(grid_zero_color), 1, 0, 1, H);
				}
			}
			else if(low < 0 && high > 0)
			{
				int f = high;

				// Calculate horizontal step size
				while(f/freq_step_size > 4)
				{
					freq_step_size = step_list[step_index]*(int)Math.Pow(10.0, step_power);
					step_index = (step_index+1)%4;
					if(step_index == 0) step_power++;
				}
				int pixel_step_size = W/2*freq_step_size/f;
				int num_steps = f/freq_step_size;

				// Draw vertical lines
				for(int i=1; i<=num_steps; i++)
				{
					int xLeft = mid_w-(i*pixel_step_size);			// for negative numbers
					int xRight = mid_w+(i*pixel_step_size);		// for positive numbers
					if(bottom)
					{
						g.DrawLine(grid_pen, xLeft, H, xLeft, H+H);		// draw left line
						g.DrawLine(grid_pen, xRight, H, xRight, H+H);		// draw right line
					}
					else
					{
						g.DrawLine(grid_pen, xLeft, 0, xLeft, H);		// draw left line
						g.DrawLine(grid_pen, xRight, 0, xRight, H);		// draw right line
					}
				
					// Draw vertical line labels
					int num = i*freq_step_size;
					string label = num.ToString();
					int offsetL = (int)((label.Length+1)*4.1);
					int offsetR = (int)(label.Length*4.1);
					if(xLeft-offsetL >= 0)
					{
						if(bottom)
						{
							g.DrawString("-"+label, font, grid_text_brush, xLeft-offsetL, H+(float)Math.Floor(H*.01));
							g.DrawString(label, font, grid_text_brush, xRight-offsetR, H+(float)Math.Floor(H*.01));
						}
						else
						{
							g.DrawString("-"+label, font, grid_text_brush, xLeft-offsetL, (float)Math.Floor(H*.01));
							g.DrawString(label, font, grid_text_brush, xRight-offsetR, (float)Math.Floor(H*.01));
						}
					}
				}

				// Draw horizontal lines
				int V = (int)(spectrum_grid_max - spectrum_grid_min);
				int numSteps = V/grid_step;
				pixel_step_size = H/numSteps;
				for(int i=1; i<numSteps; i++)
				{
					int xOffset = 0;
					int num = spectrum_grid_max - i*grid_step;
					int y = (int)Math.Floor((double)(spectrum_grid_max - num)*H/y_range);
					if(bottom) g.DrawLine(grid_pen, 0, H+y, W, H+y);
					else g.DrawLine(grid_pen, 0, y, W, y);

					// Draw horizontal line labels
                    if (i != 1) // avoid intersecting vertical and horizontal labels
                    {
                        string label = num.ToString();
                        if (label.Length == 3)  xOffset = (int)g.MeasureString("-", font).Width - 2;
                        int offset = (int)(label.Length * 4.1);
                        SizeF size = g.MeasureString(label, font);

                        int x = 0;
                        switch (display_label_align)
                        {
                            case DisplayLabelAlignment.LEFT:
                                x = xOffset + 3;
                                break;
                            case DisplayLabelAlignment.CENTER:
                                x = center_line_x + xOffset;
                                break;
                            case DisplayLabelAlignment.RIGHT:
                                x = (int)(W - size.Width - 3);
                                break;
                            case DisplayLabelAlignment.AUTO:
                                x = xOffset + 3;
                                break;
                            case DisplayLabelAlignment.Sunit: // ke9ns add
                                x = xOffset + 3;
                                break;
                            case DisplayLabelAlignment.OFF:
                                x = W;
                                break;
                        }

                        y -= 8;
                        if (y + 9 < H)
                        {
                            if (bottom) g.DrawString(label, font, grid_text_brush, x, H + y);
                            g.DrawString(label, font, grid_text_brush, x, y);
                        }
                    }
				}

				// Draw middle vertical line
				if(bottom)
				{
					g.DrawLine(new Pen(grid_zero_color), mid_w, H, mid_w, H+H);
					g.DrawLine(new Pen(grid_zero_color), mid_w-1, H, mid_w-1, H+H);
				}
				else
				{
					g.DrawLine(new Pen(grid_zero_color), mid_w, 0, mid_w, H);
					g.DrawLine(new Pen(grid_zero_color), mid_w-1, 0, mid_w-1, H);
				}
			}

			if(high_swr && !bottom)
				g.DrawString("High SWR", new System.Drawing.Font("Arial", 14, FontStyle.Bold), new SolidBrush(Color.Red), 245, 20);
		}

#if(!NO_TNF)
        static float zoom_height = 1.5f;   // Should be > 1.  H = H/zoom_height
#endif


        //=========================================================
        // ke9ns draw panadapter grid
        //=========================================================

        public static int[] holder2 = new int[100];                           // ke9ns add MEMORY Spot used to allow the vertical lines to all be drawn first so the call sign text can draw over the top of it.
        public static int[] holder3 = new int[100];                          // ke9ns add

        public static int[] holder = new int[100];                           // ke9ns add DX Spot used to allow the vertical lines to all be drawn first so the call sign text can draw over the top of it.
        public static int[] holder1 = new int[100];                          // ke9ns add
        private static Font font1 = new Font("Ariel", 9, FontStyle.Regular);  // ke9ns add dx spot call sign font style

        private static Pen p1 = new Pen(Color.YellowGreen, 2.0f);             // ke9ns add vert line color and thickness  DXSPOTTER
        private static Pen p3 = new Pen(Color.Blue, 2.5f);                   // ke9ns add vert line color and thickness    MEMORY
        private static Pen p2 = new Pen(Color.Purple, 2.0f);                  // ke9ns add color for vert line of SWL list
       
        private static SizeF length;                                          // ke9ns add length of call sign so we can do usb/lsb and define a box to click into
        private static SizeF length1;                                          // ke9ns add length of call sign so we can do usb/lsb and define a box to click into

        private static bool low = false;                                     // ke9ns add true=LSB, false=USB
        private static int rx2 = 0;                                          // ke9ns add 0-49 spots for rx1 panadapter window for qrz callup  (50-100 for rx2)
        private static int rx3 = 0;                                          // ke9ns add 0-49 spots for rx1 panadapter window for qrz callup  (50-100 for rx2)

        public static int VFOLow = 0;                                       // ke9ns low freq (left side of screen) in HZ (used in DX_spot)
        public static int VFOHigh = 0;                                      // ke9ns high freq (right side of screen) in HZ
        public static int VFODiff = 0;                                      // ke9ns diff high-low

        static Color c1;
        static Color c2;

        static bool SUNIT = false;                                          // ke9ns add true= S-Unit scale was activated
       

        private static void DrawPanadapterGrid(ref Graphics g, int W, int H, int rx, bool bottom)
        {

            
            if ((K9 == 5) && (K10 != 5) && (bottom)) H1 = H - (H / 2); // to help RX2 pan display in 1/3 instead of 1/2

            if ((K9 == 5) && (K10 == 5) && (bottom)) H1 = H - (H / 2); // to help RX2 pan display in 1/4 instead of 1/2


            // draw background
            /*if(bottom) g.FillRectangle(new SolidBrush(display_background_color), 0, H, W, H);
			else g.FillRectangle(new SolidBrush(display_background_color), 0, 0, W, H);*/

            bool local_mox = false;

            if (mox && rx == 1 && !tx_on_vfob) local_mox = true;
            if (mox && rx == 2 && tx_on_vfob) local_mox = true;

            int Low = rx_display_low;           // ke9ns low= -96000 at 192k SR and zoom = .5
            int High = rx_display_high;         // ke9ns high= +96000 at 192k SR and zoom = .5
            int mid_w = W / 2;
            int[] step_list = { 10, 20, 25, 50 };
            int step_power = 1;
            int step_index = 0;
            int freq_step_size = 50;
            int inbetweenies = 5;

            int grid_step = 0;  // 

          
            //------------------------------------------------------------------------------
            // ke9ns add   this S-Unit scale is designed to display a Grid based on the the S unit dBm
            if ((display_label_align == DisplayLabelAlignment.Sunit) && (!local_mox)) // if in S unit mode then draw lines only on the S unit point
            {

                if (SUNIT == false)  SUNIT = true; // let code know we are now in special S-Unit mode

              


                if ((K9 == 5)) // if rx2 is enabled cut the number of grids down in half
                {
                    if (VFOA < 30000000) // HF     dBm num = spectrum_grid_max - (i * grid_step);
                    {
                        spectrum_grid_max = -49;
                        spectrum_grid_min = -139; // S0 is -133 to -122
                    }
                    else // VHF
                    {
                        spectrum_grid_max = -69;
                        spectrum_grid_min = -159; // S0 is -153 to -142
                    }

                    if (bottom)
                    {
                        spectrum_grid_step = 6; // 6
                        grid_step = 6;  // 6
                    }
                    else
                    {
                        spectrum_grid_step = 6;  // 6
                        grid_step = 12;  // 12
                    }

                }
                else // just RX1
                {
                    if (VFOA < 30000000) // HF     dBm num = spectrum_grid_max - (i * grid_step);
                    {
                        spectrum_grid_max = -43;   // -43
                        spectrum_grid_min = -139; // S0 is -133 to -122
                    }
                    else // VHF
                    {
                        spectrum_grid_max = -63;  // -63
                        spectrum_grid_min = -159; // S0 is -153 to -142
                    }

                    spectrum_grid_step = 6;
                    grid_step = 6;

                } // just rx1
               

            } // special dBm and S-Unit display
            else // use normal setup->display setpoints
            {
                if ((SUNIT == true) && (!local_mox)) // if you were in S-Unit mode, then put back normal values but only in RX mode
                {
                    spectrum_grid_max = spectrum_grid_max1; // use setupform orignal values over again
                    spectrum_grid_min = spectrum_grid_min1;
                    spectrum_grid_step = spectrum_grid_step1;
                    SUNIT = false;
                }
                
                grid_step = spectrum_grid_step; // you maybe in TX mode here

                if (split_display) grid_step = grid_step * 2; // increase grid_step since you have less space on screen

                if ((K9 == 5) && (K10 != 5) && (bottom)) grid_step = spectrum_grid_step; // 1.5 ke9ns ADDED THIS CODE increase grid_step again since you have even less space on screen
                if ((K9 == 5) && (K10 == 5) && (bottom)) grid_step = spectrum_grid_step; // 1.5 ke9ns ADDED THIS CODE increase grid_step again since you have even less space on screen


            } // standard dBm display

            //----------------------------------------------------------------------------------------
           
            //  if (bottom) Debug.WriteLine("bottom...top " + top + " H " + H);
            //  else Debug.WriteLine("top...top " + top + " H " + H);

            bool is_first = true;
            int _x = 0;
            int _y = 0;
            int _width = 0;
            int _height = 0;

            System.Drawing.Font font = new System.Drawing.Font("Swis721 BT", 9, FontStyle.Italic);
            SolidBrush grid_text_brush = new SolidBrush(grid_text_color);

            Pen grid_pen = new Pen(Color.FromArgb(42, Color.White));
            Pen grid_pen_dark = new Pen(Color.FromArgb(16, Color.White));

            Pen tx_filter_pen = new Pen(display_filter_tx_color);


            int y_range = spectrum_grid_max - spectrum_grid_min; // ke9ns H grid span min to max


            int filter_low, filter_high;     // filter bandwidth        

          
            int center_line_x = (int)(-(double)Low / (High - Low) * W); // center of display window

      
            if (local_mox) // get filter limits
            {
                filter_low = tx_filter_low;
                filter_high = tx_filter_high;
            }
            else if (rx == 1)
            {
                filter_low = rx1_filter_low;
                filter_high = rx1_filter_high;
            }
            else //if(rx == 2)
            {
                filter_low = rx2_filter_low;
                filter_high = rx2_filter_high;
            }

            if ((rx1_dsp_mode == DSPMode.DRM && rx == 1) || (rx2_dsp_mode == DSPMode.DRM && rx == 2))
            {
                filter_low = -5000;
                filter_high = 5000;
            }


            //===========================================================
            // Calculate horizontal step size
            //===========================================================

            int width = High - Low;
            while (width / freq_step_size > 10)
            {
                /*inbetweenies = step_list[step_index] / 10;
                if (inbetweenies == 1) inbetweenies = 10;*/
                freq_step_size = step_list[step_index] * (int)Math.Pow(10.0, step_power);
                step_index = (step_index + 1) % 4;
                if (step_index == 0) step_power++;
            }
            double w_pixel_step = (double)W * freq_step_size / width;
            int w_steps = width / freq_step_size;

         

            //===========================================================
            // calculate vertical step size
            //===========================================================


            int h_steps = (spectrum_grid_max - spectrum_grid_min) / grid_step; // if display area shrinks, grid step goes up, so h_steps goes down.


            if ((SUNIT) && (!local_mox)) // if in S unit mode then draw lines only on the S unit point
            {
                if ((K9 == 5)) // if RX2 is enabled
                {
                   if (bottom) h_steps = 23; // 23 if RX2 enabled
                    else h_steps = 11; // 11 if RX2 enabled
                }
                else  h_steps = 18; // 18 show all S units all the time in Sunit mode

            }

          
            double h_pixel_step = (double)H / h_steps; // ke9ns ?

          //  Debug.WriteLine("spectrum_grid_max, min, stp, hstp =" + spectrum_grid_max + " ," + spectrum_grid_min + " , " + spectrum_grid_step + " , " + grid_step + " , " + h_steps);


            if ((SUNIT) && (!local_mox))
            {

                if ((K9 == 5))
                {
                    if (bottom) h_steps = 11; // 11 if RX2 enabled
                    else h_steps = 11; // 11 if RX2 enabled
                }
                else h_steps = 18; // 18 show all S units all the time in Sunit mode

            }
            else // if in special dBm S-Unit mode
            {
                if ((K9 == 5) && (K10 != 5) && (bottom)) h_steps = (spectrum_grid_max - spectrum_grid_min) / (spectrum_grid_step * 2); // ke9ns ADDED CODE area are in thirds so hstep for RX1 and RX2 should be the same
                if ((K9 == 5) && (K10 == 5) && (bottom)) h_steps = (spectrum_grid_max - spectrum_grid_min) / (spectrum_grid_step * 2); // ke9ns ADDED CODE area are in 1/4rs so hstep for RX1 and RX2 should be the same

            }


            int top = (int)((double)grid_step * H / y_range); // find top of each window for the panadapter

          


            //===========================================================
            // draw sub filter
            //===========================================================
            if (!local_mox && sub_rx1_enabled && rx == 1)
            {

                // draw Sub RX filter
                // get filter screen coordinates

                int filter_left_x = (int)((float)(filter_low - Low + vfoa_sub_hz - vfoa_hz - rit_hz) / (High - Low) * W);
                int filter_right_x = (int)((float)(filter_high - Low + vfoa_sub_hz - vfoa_hz - rit_hz) / (High - Low) * W);

                // make the filter display at least one pixel wide.
                if (filter_left_x == filter_right_x) filter_right_x = filter_left_x + 1;

                // draw rx filter
                if (bottom)
                {
                    g.FillRectangle(new SolidBrush(sub_rx_filter_color),    // draw filter overlay
                        filter_left_x, H + top, filter_right_x - filter_left_x, H + H - top);
                }
                else
                {
                    g.FillRectangle(new SolidBrush(sub_rx_filter_color),    // draw filter overlay
                        filter_left_x, top, filter_right_x - filter_left_x, H - top);
                }

               
                //===============================================================
                // draw Sub RX 0Hz line
                //===============================================================
                int x = (int)((float)(vfoa_sub_hz - vfoa_hz - Low) / (High - Low) * W);  // ke9ns draw red line

                if (bottom)
                {
                    g.DrawLine(new Pen(sub_rx_zero_line_color), x, H + top, x, H + H); // ke9ns draw red line
                    g.DrawLine(new Pen(sub_rx_zero_line_color), x - 1, H + top, x - 1, H + H);
                }
                else
                {
                    g.DrawLine(new Pen(sub_rx_zero_line_color), x, top, x, H);         // ke9ns draw red line
                    g.DrawLine(new Pen(sub_rx_zero_line_color), x - 1, top, x - 1, H);
                }

            } // draw sub filter

        
            //============================================================================================
            //============================================================================================
            //============================================================================================

            if (rx == 1)
            {
                //============================================================================================
                // ke9ns RX1 draw main filter bandpass display
                //============================================================================================
               
                if (!(local_mox && (rx1_dsp_mode == DSPMode.CWL || rx1_dsp_mode == DSPMode.CWU)))  // draw bandpass for RX or bandpass for TX (but not in cW mode)) 
                {
                    // get filter screen coordinates

                    int filter_left_x;
                    int filter_right_x;


                    filter_left_x = (int)((float)(filter_low - Low) / (High - Low) * W); // original
                    filter_right_x = (int)((float)(filter_high - Low) / (High - Low) * W);


                    // make the filter display at least one pixel wide.
                    if (filter_left_x == filter_right_x) filter_right_x = filter_left_x + 1;


                    // draw rx filter
                    //   if (bottom)
                    //   {
                    //      g.FillRectangle(new SolidBrush(display_filter_color),   // draw filter overlay
                    //         filter_left_x, H + top, filter_right_x - filter_left_x, H + H - top );
                    // }
                    // else // top half
                    // {
                  
                    g.FillRectangle(new SolidBrush(display_filter_color),   // draw filter overlay
                            filter_left_x, top, filter_right_x - filter_left_x, H - top  );

                   // }
                } // main RX1 filter
                else if ((local_mox) && (rx1_dsp_mode == DSPMode.CWL || rx1_dsp_mode == DSPMode.CWU) && (!split_enabled)) // draw CW rx box when you tx.
                {

                    int filter_left_x;
                    int filter_right_x;


                    filter_left_x = (int)((float)(rx1_filter_low - Low) / (High - Low) * W); // original
                    filter_right_x = (int)((float)(rx1_filter_high - Low) / (High - Low) * W);


                    // make the filter display at least one pixel wide.
                    if (filter_left_x == filter_right_x) filter_right_x = filter_left_x + 1;


                  
                    g.FillRectangle(new SolidBrush(display_filter_color),   // draw filter overlay
                        filter_left_x, top, filter_right_x - filter_left_x, H - top);
                   
                }
               
                //============================================================================================
                // ke9ns  RX1 draw tx line for everything but cw
                //============================================================================================
                if (!local_mox && draw_tx_filter && (rx1_dsp_mode != DSPMode.CWL && rx1_dsp_mode != DSPMode.CWU) )
                {
                    // get tx filter limits
                    int filter_left_x;
                    int filter_right_x;

                   if (tx_on_vfob)
                    {
                        if (!split_enabled)
                        {
                           filter_left_x = (int)((float)(tx_filter_low - Low + xit_hz - rit_hz) / (High - Low) * W); //original
                           filter_right_x = (int)((float)(tx_filter_high - Low + xit_hz - rit_hz) / (High - Low) * W); // 
                        }
                        else
                        {
                            filter_left_x = (int)((float)(tx_filter_low - Low + xit_hz - rit_hz + (vfob_sub_hz - vfoa_hz)) / (High - Low) * W);
                            filter_right_x = (int)((float)(tx_filter_high - Low + xit_hz - rit_hz + (vfob_sub_hz - vfoa_hz)) / (High - Low) * W);
                        }
                    }
                    else // TX on VFOA
                    {
                        if (!split_enabled)
                        {
                            //Transmit profile high=4000, low = 70  results in:
                            // tx_filter_low = -4000 lsb or +70 usb hz
                            // tx_filter_high = -70 lsb or +4000 usb hz
                            // filter_left_x = 658 lsb or 732 usb pixel
                            // filter_right_x = 740 lsb or 804 usb  pixel
                            // W = 1463 based on size of console and resolution, HIGH = 40000, Low = - 40000  at Zoom=1 and 192k SR


                            filter_left_x = (int)((float)(tx_filter_low - Low + xit_hz - rit_hz) / (High - Low) * W);
                            filter_right_x = (int)((float)(tx_filter_high - Low + xit_hz - rit_hz) / (High - Low) * W);

                            //  Debug.Write(" txfilterlow=" + tx_filter_low); // 
                            //  Debug.Write(" txfilterhigh=" + tx_filter_high);
                            //  Debug.Write(" filter_left_x=" + filter_left_x);
                            //   Debug.Write(" filter_right_x=" + filter_right_x);
                            //   Debug.Write(" High" + High);
                            //   Debug.Write(" Low" + Low);
                            //  Debug.Write(" W" + W);
                        }
                        else // split on vfoa
                        {
                            filter_left_x = (int)((float)(tx_filter_low - Low + xit_hz - rit_hz + (vfoa_sub_hz - vfoa_hz)) / (High - Low) * W);
                            filter_right_x = (int)((float)(tx_filter_high - Low + xit_hz - rit_hz + (vfoa_sub_hz - vfoa_hz)) / (High - Low) * W);

                         


                        }
                    }

                    if (bottom) // && tx_on_vfob)  // if transmitting on RX2 then draw lines on bottom
                    {
                        // draw tx filter overlay
                        g.DrawLine(tx_filter_pen, filter_left_x, H + top, filter_left_x, H + H);
                        g.DrawLine(tx_filter_pen, filter_right_x, H + top, filter_right_x, H + H);  // draw tx filter overlay

                    }
                    else if ((!tx_on_vfob) && (!bottom)) // ke9ns if transmitting on normal RX1 draw lines // KE9NS ADD  fix mistake made by flex  makes the line thicker
                    {
                        // ke9ns pgrid ORANGE LEFT and RIGHT TX WIDTH LINES for TRANSMITTER ONLY
                        g.DrawLine(tx_filter_pen, filter_left_x, top, filter_left_x, H);        // LEFT draw tx filter overlay
                        g.DrawLine(tx_filter_pen, filter_right_x, top, filter_right_x, H);      // RIGHT draw tx filter ovelay
                    }

                } // draw ssb TX1 filter width lines


                //============================================================================================
                // ke9ns  RX1 draw tx line for cw
                //============================================================================================
                if ((!local_mox) && (draw_tx_cw_freq) && (rx1_dsp_mode == DSPMode.CWL || rx1_dsp_mode == DSPMode.CWU) )
                {
                    int pitch = cw_pitch;

                    if ( (rx1_dsp_mode == DSPMode.CWL)) pitch = -cw_pitch;
                 
                    int cw_line_x;

                    if (!split_enabled)

                        cw_line_x = (int)((float)(pitch - Low + xit_hz - rit_hz) / (High - Low) * W);

                    else
                        cw_line_x = (int)((float)(pitch - Low + xit_hz - rit_hz + (vfoa_sub_hz - vfoa_hz)) / (High - Low) * W);

                    if ((bottom) && tx_on_vfob) // KE9NS ADD  fix mistake made by flex
                    {
                        g.DrawLine(tx_filter_pen, cw_line_x, H + top, cw_line_x, H + H);
                        g.DrawLine(tx_filter_pen, cw_line_x + 1, H + top, cw_line_x + 1, H + H);
                    }
                    else if ((!tx_on_vfob) && (!bottom))
                    {
                        g.DrawLine(tx_filter_pen, cw_line_x, top, cw_line_x, H);
                        g.DrawLine(tx_filter_pen, cw_line_x + 1, top, cw_line_x + 1, H);
                    }

                } // cw filter lines

            } // rx == 1
            else // rx == 2
            {
                //============================================================================================
                // ke9ns RX2 draw main filter bandpass display
                //============================================================================================
                if (!(local_mox && (rx2_dsp_mode == DSPMode.CWL || rx2_dsp_mode == DSPMode.CWU)))  // draw bandpass for RX or bandpass for TX (but not in cW mode)) 
                {
                    // get filter screen coordinates

                    int filter_left_x;
                    int filter_right_x;

                    filter_left_x = (int)((float)(filter_low - Low) / (High - Low) * W); // original
                    filter_right_x = (int)((float)(filter_high - Low) / (High - Low) * W);

                    // make the filter display at least one pixel wide.
                    if (filter_left_x == filter_right_x) filter_right_x = filter_left_x + 1;

                 
                  // rx2 always on the bottom
                        g.FillRectangle(new SolidBrush(display_filter_color),   // draw filter overlay
                            filter_left_x, H + top, filter_right_x - filter_left_x, H + H - top);
                  
                } // filter
                else if ((local_mox) && (rx2_dsp_mode == DSPMode.CWL || rx2_dsp_mode == DSPMode.CWU)) // draw CW rx box
                {

                    int filter_left_x;
                    int filter_right_x;


                    filter_left_x = (int)((float)(rx2_filter_low - Low) / (High - Low) * W); // original
                    filter_right_x = (int)((float)(rx2_filter_high - Low) / (High - Low) * W);


                    // make the filter display at least one pixel wide.
                    if (filter_left_x == filter_right_x) filter_right_x = filter_left_x + 1;



                    g.FillRectangle(new SolidBrush(display_filter_color),   // draw filter overlay
                        filter_left_x, top, filter_right_x - filter_left_x, H - top);

                }

                //============================================================================================
                // ke9ns  RX2 draw tx line for everything but cw
                //============================================================================================
                if (!local_mox && draw_tx_filter && (rx2_dsp_mode != DSPMode.CWL && rx2_dsp_mode != DSPMode.CWU) && (tx_on_vfob))
                {
                    // get tx filter limits
                    int filter_left_x;
                    int filter_right_x;

                    
                        if (!split_enabled)
                        {
                            filter_left_x = (int)((float)(tx_filter_low - Low + xit_hz - rit_hz) / (High - Low) * W); //original
                            filter_right_x = (int)((float)(tx_filter_high - Low + xit_hz - rit_hz) / (High - Low) * W); // 
                        }
                        else
                        {
                            filter_left_x = (int)((float)(tx_filter_low - Low + xit_hz - rit_hz + (vfob_sub_hz - vfoa_hz)) / (High - Low) * W);
                            filter_right_x = (int)((float)(tx_filter_high - Low + xit_hz - rit_hz + (vfob_sub_hz - vfoa_hz)) / (High - Low) * W);
                        }
                    
                
                        // draw tx filter overlay
                        g.DrawLine(tx_filter_pen, filter_left_x, H + top, filter_left_x, H + H);
                        g.DrawLine(tx_filter_pen, filter_right_x, H + top, filter_right_x, H + H);  // draw tx filter overlay

                
                } // draw filter width lines


                //============================================================================================
                // ke9ns  RX2 draw tx line for cw
                //============================================================================================
                if (!local_mox && draw_tx_cw_freq && (rx2_dsp_mode == DSPMode.CWL || rx2_dsp_mode == DSPMode.CWU)  && (tx_on_vfob))
                {
                    int pitch = cw_pitch;

                    if (rx2_dsp_mode == DSPMode.CWL) pitch = -cw_pitch;

                    int cw_line_x;

                    if (!split_enabled)

                        cw_line_x = (int)((float)(pitch - Low + xit_hz - rit_hz) / (High - Low) * W);

                    else
                        cw_line_x = (int)((float)(pitch - Low + xit_hz - rit_hz + (vfoa_sub_hz - vfoa_hz)) / (High - Low) * W);

                 
                        g.DrawLine(tx_filter_pen, cw_line_x, H + top, cw_line_x, H + H);
                        g.DrawLine(tx_filter_pen, cw_line_x + 1, H + top, cw_line_x + 1, H + H);
                  

                } // cw filter line

            } // rx == 2



            //===============================================================
            // draw 60m channels if in view
            //===============================================================

            foreach (Channel c in channels_60m)
            {
                long rf_freq = vfoa_hz;
                int rit = rit_hz;
                if (local_mox) rit = 0;

                if (bottom)
                {
                    rf_freq = vfob_hz;
                }

                if (c.InBW((rf_freq + Low) * 1e-6, (rf_freq + High) * 1e-6)) // is channel visible?
                {
                    bool on_channel = console.RX1IsOn60mChannel(c); // only true if you are on channel and are in an acceptable mode

                    if (bottom) on_channel = console.RX2IsOn60mChannel(c);

                    DSPMode mode = rx1_dsp_mode;
                    if (bottom) mode = rx2_dsp_mode;

                    switch (mode)
                    {
                        case DSPMode.USB:
                        case DSPMode.DIGU:
                        case DSPMode.CWL:
                        case DSPMode.CWU:
                            break;
                        default:
                            on_channel = false; // make sure other modes do not look as if they could transmit
                            break;
                    }

                    // offset for CW Pitch to align display
                    if (bottom)
                    {
                        switch (rx2_dsp_mode)
                        {
                            case (DSPMode.CWL):
                                rf_freq += cw_pitch;
                                break;
                            case (DSPMode.CWU):
                                rf_freq -= cw_pitch;
                                break;
                        }
                    }
                    else
                    {
                        switch (rx1_dsp_mode)
                        {
                            case (DSPMode.CWL):
                                rf_freq += cw_pitch;
                                break;
                            case (DSPMode.CWU):
                                rf_freq -= cw_pitch;
                                break;
                        }
                    }



                    int chan_left_x = (int)((float)(c.Freq * 1e6 - rf_freq - c.BW / 2 - Low - rit) / (High - Low) * W);
                    int chan_right_x = (int)((float)(c.Freq * 1e6 - rf_freq + c.BW / 2 - Low - rit) / (High - Low) * W);

                    if (chan_right_x == chan_left_x)
                        chan_right_x = chan_left_x + 1;

                    // decide colors to draw notch
                    c1 = channel_background_off;
                    c2 = channel_foreground;

                    if (on_channel)
                    {
                        c1 = channel_background_on;
                    }

                    if (bottom)
                        drawChannelBar(g, c, chan_left_x, chan_right_x, H + top, H - top, c1, c2);
                    else
                        drawChannelBar(g, c, chan_left_x, chan_right_x, top, H - top, c1, c2);

                    //if (bottom)
                    //    drawNotchStatus(g, n, (notch_left_x + notch_right_x) / 2, H + top + 75, W, H);
                    //else
                    //    drawNotchStatus(g, n, (notch_left_x + notch_right_x) / 2, top + 75, W, H);
                }
            }  // 60m channels




#if (!NO_TNF)

            //===============================================================
            // Draw TNF NOTCH
            //===============================================================


            // draw notches if in RX
            if (!local_mox)
            {
                List<Notch> notches;
                if (!bottom)
                    notches = NotchList.NotchesInBW((double)vfoa_hz * 1e-6, Low, High);
                else
                    notches = NotchList.NotchesInBW((double)vfob_hz * 1e-6, Low, High);


                //draw notch bars in this for loop
                foreach (Notch n in notches)
                {
                    long rf_freq = vfoa_hz;
                    int rit = rit_hz;

                    if (bottom)
                    {
                        rf_freq = vfob_hz;
                    }

                    if (bottom)
                    {
                        switch (rx2_dsp_mode)
                        {
                            case (DSPMode.CWL):
                                rf_freq += cw_pitch;
                                break;
                            case (DSPMode.CWU):
                                rf_freq -= cw_pitch;
                                break;
                        }
                    }
                    else
                    {
                        switch (rx1_dsp_mode)
                        {
                            case (DSPMode.CWL):
                                rf_freq += cw_pitch;
                                break;
                            case (DSPMode.CWU):
                                rf_freq -= cw_pitch;
                                break;
                        }
                    }

                    int notch_left_x = (int)((float)(n.Freq * 1e6 - rf_freq - n.BW / 2 - Low - rit) / (High - Low) * W);
                    int notch_right_x = (int)((float)(n.Freq * 1e6 - rf_freq + n.BW / 2 - Low - rit) / (High - Low) * W);

                    if (notch_right_x == notch_left_x)
                        notch_right_x = notch_left_x + 1;

                    if (tnf_zoom && n.Details && ((bottom && n.RX == 2) || (!bottom && n.RX == 1)))
                    {
                        int zoomed_notch_center_freq = (int)(notch_zoom_start_freq * 1e6 - rf_freq - rit);

                        int original_bw = High - Low;
                        int zoom_bw = original_bw / 10;

                        int low = zoomed_notch_center_freq - zoom_bw / 2;
                        int high = zoomed_notch_center_freq + zoom_bw / 2;

                        if (low < Low) // check left limit
                        {
                            low = Low;
                            high = Low + zoom_bw;
                        }
                        else if (high > High) // check right limit
                        {
                            high = High;
                            low = High - zoom_bw;
                        }

                        int zoom_bw_left_x = (int)((float)(low - Low) / (High - Low) * W);
                        int zoom_bw_right_x = (int)((float)(high - Low) / (High - Low) * W);

                        Pen p = new Pen(Color.White, 2.0f);

                        if (!bottom)
                        {
                            // draw zoomed bandwidth outline TNF ZOOM
                            Point[] left_zoom_line_points = {
                                new Point(0, (int)(H/zoom_height)),
                                new Point(zoom_bw_left_x-1,(int)(0.5*H*(1+1/zoom_height))),
                                new Point(zoom_bw_left_x-1, H) };

                            g.DrawLines(p, left_zoom_line_points);

                            Point[] right_zoom_line_points = {
                                new Point(W, (int)(H/zoom_height)),
                                new Point(zoom_bw_right_x+1, (int)(0.5*H*(1+1/zoom_height))),
                                new Point(zoom_bw_right_x+1, H) };
                            g.DrawLines(p, right_zoom_line_points);

                            //grey out non-zoomed in area on actual panadapter
                            g.FillRectangle(new SolidBrush(Color.FromArgb(150, 0, 0, 0)), 0, H / zoom_height, zoom_bw_left_x, H - H / zoom_height);
                            g.FillRectangle(new SolidBrush(Color.FromArgb(150, 0, 0, 0)), zoom_bw_right_x, H / zoom_height, W - zoom_bw_right_x, H - H / zoom_height);
                        }
                        else
                        {
                            // draw zoomed bandwidth outline
                            Point[] left_zoom_line_points = {
                                new Point(0, H+(int)(H/zoom_height)),
                                new Point(zoom_bw_left_x-1, H+(int)(0.5*H*(1+1/zoom_height))),
                                new Point(zoom_bw_left_x-1, H+H) };
                            g.DrawLines(p, left_zoom_line_points);

                            Point[] right_zoom_line_points = {
                                new Point(W, H+(int)(H/zoom_height)),
                                new Point(zoom_bw_right_x+1, H+(int)(0.5*H*(1+1/zoom_height))),
                                new Point(zoom_bw_right_x+1, H+H) };
                            g.DrawLines(p, right_zoom_line_points);

                            g.FillRectangle(new SolidBrush(Color.FromArgb(160, 0, 0, 0)), 0, H + H / zoom_height, zoom_bw_left_x, H + H - H / zoom_height);
                            g.FillRectangle(new SolidBrush(Color.FromArgb(160, 0, 0, 0)), zoom_bw_right_x, H + H / zoom_height, W - zoom_bw_right_x, H + H - H / zoom_height);
                        }
                    } // tnf zoom

                    // decide colors to draw notch
                    c1 = notch_on_color;
                    c2 = notch_highlight_color;

                    if (!tnf_active)
                    {
                        c1 = notch_off_color;
                        c2 = Color.Black;
                    }
                    else if (n.Permanent)
                    {
                        c1 = notch_perm_on_color;
                        c2 = notch_perm_highlight_color;
                    }

                    if (bottom)
                        drawNotchBar(g, n, notch_left_x, notch_right_x, H + top, H - top, c1, c2);
                    else
                        drawNotchBar(g, n, notch_left_x, notch_right_x, top, H - top, c1, c2);
                }
              
                //draw notch statuses in this for loop
                if (!tnf_zoom)
                {
                    foreach (Notch n in notches)
                    {
                        long rf_freq = vfoa_hz;
                        int rit = rit_hz;

                        if (bottom)
                        {
                            rf_freq = vfob_hz;
                        }

                        if (bottom)
                        {
                            switch (rx2_dsp_mode)
                            {
                                case (DSPMode.CWL):
                                    rf_freq += cw_pitch;
                                    break;
                                case (DSPMode.CWU):
                                    rf_freq -= cw_pitch;
                                    break;
                            }
                        }
                        else
                        {
                            switch (rx1_dsp_mode)
                            {
                                case (DSPMode.CWL):
                                    rf_freq += cw_pitch;
                                    break;
                                case (DSPMode.CWU):
                                    rf_freq -= cw_pitch;
                                    break;
                            }
                        }

                        int notch_left_x = (int)((float)(n.Freq * 1e6 - rf_freq - n.BW / 2 - Low - rit) / (High - Low) * W);
                        int notch_right_x = (int)((float)(n.Freq * 1e6 - rf_freq + n.BW / 2 - Low - rit) / (High - Low) * W);

                        if (notch_right_x == notch_left_x)
                            notch_right_x = notch_left_x + 1;

                        if (bottom)
                            drawNotchStatus(g, n, (notch_left_x + notch_right_x) / 2, H + top + 75, W, H);
                        else
                            drawNotchStatus(g, n, (notch_left_x + notch_right_x) / 2, top + 75, W, H);
                    }
                }
            } // tnf
#endif // TNF



            //===============================================================
            // Draw VFO
            //===============================================================

            double vfo;
            if (rx == 1)
            {
                if (local_mox && !tx_on_vfob)
                {
                    if (split_enabled)   vfo = vfoa_sub_hz;
                    else   vfo = vfoa_hz;

                    vfo += xit_hz; 
                  }
                else
                {
                    vfo = vfoa_hz + rit_hz;
            
                }
            }
            else //if(rx==2)
            {
                if (local_mox && tx_on_vfob)  vfo = vfob_hz + xit_hz;
                else vfo = vfob_hz + rit_hz;
            }

            if (!bottom)
            {
                switch (rx1_dsp_mode)
                {
                    case DSPMode.CWL:
                        vfo += cw_pitch;
                        break;
                    case DSPMode.CWU:
                        vfo -= cw_pitch;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (rx2_dsp_mode)
                {
                    case DSPMode.CWL:
                        vfo += cw_pitch;
                        break;
                    case DSPMode.CWU:
                        vfo -= cw_pitch;
                        break;
                    default:
                        break;
                }
            }

            //===============================================================
            // Draw vertical lines - band edge markers and freq text
            //===============================================================

          
            long vfo_round = ((long)(vfo / freq_step_size)) * freq_step_size;
            long vfo_delta = (long)(vfo - vfo_round);
           
            int f_steps = (width/freq_step_size)+1;

         
            switch (console.CurrentRegion)
            {
                //============================================================================================================
                //============================================================================================================
                //============================================================================================================
                case FRSRegion.US: // 
                    for (int i = 0; i < f_steps + 1; i++)
                    {
                        string label;
                        int offsetL;
                        int offsetR;

                        int fgrid = i * freq_step_size + (Low / freq_step_size) * freq_step_size;
                        double actual_fgrid = ((double)(vfo_round + fgrid)) / 1000000;
                     
                        int vgrid = (int)((double)(fgrid - vfo_delta - Low ) / (High - Low) * W); 

                        if (!show_freq_offset)
                        {
                            if ( actual_fgrid == 0.1357 || actual_fgrid == 0.1358 ||   // 2200m band edges
                                actual_fgrid == 0.472 || actual_fgrid == 0.479 ||   // 630m band edges
                                actual_fgrid == 1.8 || actual_fgrid == 2.0 ||
                                actual_fgrid == 3.5 || actual_fgrid == 4.0 ||
                                actual_fgrid == 5.250 || actual_fgrid == 5.45 ||   
                                actual_fgrid == 7.0 || actual_fgrid == 7.3 ||
                                actual_fgrid == 10.1 || actual_fgrid == 10.15 ||
                                actual_fgrid == 14.0 || actual_fgrid == 14.35 ||
                                actual_fgrid == 18.068 || actual_fgrid == 18.168 ||
                                actual_fgrid == 21.0 || actual_fgrid == 21.45 ||
                                actual_fgrid == 24.89 || actual_fgrid == 24.99 ||
                                actual_fgrid == 26.960 || actual_fgrid == 27.410 || // ke9ns add CB
                                actual_fgrid == 28.0 || actual_fgrid == 29.7 ||
                                actual_fgrid == 50.0 || actual_fgrid == 54.0 ||
                                actual_fgrid == 144.0 || actual_fgrid == 148.0)
                            {
                                if (bottom) g.DrawLine(new Pen(band_edge_color), vgrid, H + top, vgrid, H + H); // draw vertical scale lines
                                else g.DrawLine(new Pen(band_edge_color), vgrid, top, vgrid, H);

                                label = actual_fgrid.ToString("f4");
                                label = label.Replace(",", ".");    // handle Windows localization issues
                                int offset = label.IndexOf('.') + 4;
                                label = label.Insert(offset, ".");

                                if (actual_fgrid < 10) offsetL = (int)((label.Length + 2) * 4.1) - 14;
                                else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                else offsetL = (int)((label.Length) * 4.1) - 8;

                                if (bottom) g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid - offsetL, H + (float)Math.Floor(H * .01));
                                else g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid - offsetL, (float)Math.Floor(H * .01));

                                int fgrid_2 = ((i + 1) * freq_step_size) + (int)((Low / freq_step_size) * freq_step_size);
                                int x_2 = (int)(((float)(fgrid_2 - vfo_delta - Low) / width * W));
                                float scale = (float)(x_2 - vgrid) / inbetweenies;

                                for (int j = 1; j < inbetweenies; j++)
                                {
                                    if (grid_off == 0) // ke9ns add (dont draw grid lines if =1 )
                                    {
                                        float x3 = (float)vgrid + (j * scale);
                                    
                                        if (bottom) g.DrawLine(grid_pen_dark, x3, H + top, x3, H + H); // ke9ns vertical lines
                                        else g.DrawLine(grid_pen_dark, x3, top, x3, H);
                                    }
                                }
                            }
                            else
                            {
                                if (grid_off == 0) // ke9ns add (dont draw grid lines if =1 
                                {
                                    if (bottom) g.DrawLine(grid_pen, vgrid, H + top, vgrid, H + H);
                                    else g.DrawLine(grid_pen, vgrid, top, vgrid, H);            //wa6ahl
                                }

                                int fgrid_2 = ((i + 1) * freq_step_size) + (int)((Low / freq_step_size) * freq_step_size);
                                int x_2 = (int)(((float)(fgrid_2 - vfo_delta - Low) / width * W));
                                float scale = (float)(x_2 - vgrid) / inbetweenies;

                                for (int j = 1; j < inbetweenies; j++)
                                {
                                    if (grid_off == 0) // ke9ns add (dont draw grid lines if =1 
                                    {
                                        float x3 = (float)vgrid + (j * scale);
                                        if (bottom) g.DrawLine(grid_pen_dark, x3, H + top, x3, H + H);
                                        else g.DrawLine(grid_pen_dark, x3, top, x3, H);
                                    }
                                }

                                // make freq grid labels
                                double actual_fgrid_label = Math.Round(actual_fgrid, 4);                                
                                label = actual_fgrid_label.ToString("f4");
                                label = label.Replace(",", ".");    // handle Windows localization issues
                                int offset = label.IndexOf('.') + 4;
                                label = label.Insert(offset, ".");

                                if (actual_fgrid < 10) offsetL = (int)((label.Length + 2) * 4.1) - 14;
                                else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                else offsetL = (int)((label.Length) * 4.1) - 8;

                                if (bottom) g.DrawString(label, font, grid_text_brush, vgrid - offsetL, H + (float)Math.Floor(H * .01));
                                else g.DrawString(label, font, grid_text_brush, vgrid - offsetL, (float)Math.Floor(H * .01));
                            }

                        }  //  if (!show_freq_offset)
                        else
                        {
                            vgrid = Convert.ToInt32((double)-(fgrid - Low) / (Low - High) * W); //wa6ahl

                            if (grid_off == 0) // ke9ns add (dont draw grid lines if =1 
                            {
                                if (bottom) g.DrawLine(grid_pen, vgrid, H + top, vgrid, H + H);
                                else g.DrawLine(grid_pen, vgrid, top, vgrid, H);            //wa6ahl
                            }

                            double new_fgrid = (vfoa_hz + fgrid) / 1000000;

                            label = fgrid.ToString();
                            offsetL = (int)((label.Length + 1) * 4.1);
                            offsetR = (int)(label.Length * 4.1);
                            if ((vgrid - offsetL >= 0) && (vgrid + offsetR < W) && (fgrid != 0))
                            {
                                if (bottom) g.DrawString(label, font, grid_text_brush, vgrid - offsetL, H + (float)Math.Floor(H * .01));
                                else g.DrawString(label, font, grid_text_brush, vgrid - offsetL, (float)Math.Floor(H * .01));
                            }
                        }
                    } // for loop


                    int[] band_edge_list_r2 = {  0135700, 0137800, 0472000, 0479000,  1800000, 2000000, 3500000, 4000000, 5250000,5450000,
                                                7000000, 7300000, 10100000, 10150000, 14000000, 14350000, 18068000, 18168000, 21000000, 21450000,
				                                24890000, 24990000, 26960000, 27410000, 28000000, 29700000, 50000000, 54000000, 144000000, 148000000 }; // ke9ns add CB

                    for (int i = 0; i < band_edge_list_r2.Length; i++)
                    {
                        double band_edge_offset = band_edge_list_r2[i] - vfo;
                        if (band_edge_offset >= Low && band_edge_offset <= High)
                        {
                            int temp_vline = (int)((double)(band_edge_offset - Low) / (High - Low) * W);//wa6ahl
                            if (bottom) g.DrawLine(new Pen(band_edge_color), temp_vline, H + top, temp_vline, H + H);//wa6ahl
                            else g.DrawLine(new Pen(band_edge_color), temp_vline, top, temp_vline, H);//wa6ahl
                        }

                    } // for loop

#if (!NO_KE9NS)
                    // ke9ns add CB 11m band channels 1 - 40

                    for (int i = 0; i < band_edge_list_r77.Length; i++)
                    {
                        double band_edge_offset = band_edge_list_r77[i] - vfo;

                        if (bottom)
                        {
                            if (is_first)
                            {
                                _x = (int)((double)(band_edge_offset - Low) / (High - Low) * W);
                                _y = H + top;
                                is_first = false;
                            }
                            else
                            {
                                _width = ((int)((double)(band_edge_offset - Low) / (High - Low) * W)) - _x;
                                _height = (H + H) - _y;
                                g.DrawRectangle(new Pen(band_box_color, band_box_width), new Rectangle(_x, _y, _width, _height));
                                is_first = true;
                            }
                        }
                        else
                        {
                            if (is_first)
                            {
                                _x = (int)((double)(band_edge_offset - Low) / (High - Low) * W);
                                _y = top;
                                is_first = false;
                            }
                            else
                            {
                                _width = ((int)((double)(band_edge_offset - Low) / (High - Low) * W)) - _x;
                                _height = H - _y;
                                g.DrawRectangle(new Pen(band_box_color, band_box_width), new Rectangle(_x, _y, _width, _height));
                                is_first = true;
                            }
                        }
                    } // for loop for 11m
                   
#endif
                    break; // case FRSRegion.US:

                //============================================================================================================
                //============================================================================================================
                //============================================================================================================
                //===============================================
                case FRSRegion.UK_Plus:
                    for (int i = 0; i < f_steps + 1; i++)
                    {
                        string label;
                        int offsetL;
                        int offsetR;

                        int fgrid = i * freq_step_size + (Low / freq_step_size) * freq_step_size;
                        double actual_fgrid = ((double)(vfo_round + fgrid)) / 1000000;
                        int vgrid = (int)((double)(fgrid - vfo_delta - Low) / (High - Low) * W);

                        if (!show_freq_offset)
                        {
                            if (actual_fgrid == 1.81 || actual_fgrid == 2.0 ||
                                actual_fgrid == 3.5 || actual_fgrid == 3.8 ||
                                actual_fgrid == 5.250 || actual_fgrid == 5.45 ||   // ke9ns add
                                actual_fgrid == 7.0 || actual_fgrid == 7.2 ||
                                actual_fgrid == 10.1 || actual_fgrid == 10.15 ||
                                actual_fgrid == 14.0 || actual_fgrid == 14.35 ||
                                actual_fgrid == 18.068 || actual_fgrid == 18.168 ||
                                actual_fgrid == 21.0 || actual_fgrid == 21.45 ||
                                actual_fgrid == 24.89 || actual_fgrid == 24.99 ||
                                actual_fgrid == 28.0 || actual_fgrid == 29.7 ||
                                actual_fgrid == 50.0 || actual_fgrid == 52.0 || // ke9ns fix from 50.08
                                actual_fgrid == 144.0 || actual_fgrid == 146.0)
                            {
                                if (bottom) g.DrawLine(new Pen(band_edge_color), vgrid, H + top, vgrid, H + H);
                                else g.DrawLine(new Pen(band_edge_color), vgrid, top, vgrid, H);

                                label = actual_fgrid.ToString("f4");
                                label = label.Replace(",", ".");    // handle Windows localization issues
                                int offset = label.IndexOf('.') + 4;
                                label = label.Insert(offset, ".");

                                if (actual_fgrid < 10) offsetL = (int)((label.Length + 2) * 4.1) - 14;
                                else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                else offsetL = (int)((label.Length) * 4.1) - 8;

                              

                                if (bottom) g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid - offsetL, H + (float)Math.Floor(H * .01));
                                else g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid - offsetL, (float)Math.Floor(H * .01));

                                int fgrid_2 = ((i + 1) * freq_step_size) + (int)((Low / freq_step_size) * freq_step_size);
                                int x_2 = (int)(((float)(fgrid_2 - vfo_delta - Low) / width * W));
                                float scale = (float)(x_2 - vgrid) / inbetweenies;

                                for (int j = 1; j < inbetweenies; j++)
                                {
                                    if (grid_off == 0) // ke9ns add (dont draw grid lines if =1 
                                    {
                                        float x3 = (float)vgrid + (j * scale);
                                        if (bottom) g.DrawLine(grid_pen_dark, x3, H + top, x3, H + H);
                                        else g.DrawLine(grid_pen_dark, x3, top, x3, H);
                                    }
                                }

                            }
                            else
                            {
                                if (bottom) g.DrawLine(grid_pen, vgrid, H + top, vgrid, H + H);
                                else g.DrawLine(grid_pen, vgrid, top, vgrid, H);			//wa6ahl

                                int fgrid_2 = ((i + 1) * freq_step_size) + (int)((Low / freq_step_size) * freq_step_size);
                                int x_2 = (int)(((float)(fgrid_2 - vfo_delta - Low) / width * W));
                                float scale = (float)(x_2 - vgrid) / inbetweenies;

                                for (int j = 1; j < inbetweenies; j++)
                                {
                                    if (grid_off == 0) // ke9ns add (dont draw grid lines if =1 
                                    {
                                        float x3 = (float)vgrid + (j * scale);
                                        if (bottom) g.DrawLine(grid_pen_dark, x3, H + top, x3, H + H);
                                        else g.DrawLine(grid_pen_dark, x3, top, x3, H);
                                    }
                                }

                                double actual_fgrid_label = Math.Round(actual_fgrid, 4);
                                label = actual_fgrid_label.ToString("f4");
                                label = label.Replace(",", ".");    // handle Windows localization issues
                                int offset = label.IndexOf('.') + 4;
                                label = label.Insert(offset, ".");

                                if (actual_fgrid < 10) offsetL = (int)((label.Length + 2) * 4.1) - 14;
                                else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                else offsetL = (int)((label.Length) * 4.1) - 8;

                                if (bottom) g.DrawString(label, font, grid_text_brush, vgrid - offsetL, H + (float)Math.Floor(H * .01));
                                else g.DrawString(label, font, grid_text_brush, vgrid - offsetL, (float)Math.Floor(H * .01));
                            }
                        }
                        else
                        {
                            vgrid = Convert.ToInt32((double)-(fgrid - Low) / (Low - High) * W); //wa6ahl
                            if (grid_off == 0) // ke9ns add (dont draw grid lines if =1 
                            {
                                if (bottom) g.DrawLine(grid_pen, vgrid, H + top, vgrid, H + H);
                                else g.DrawLine(grid_pen, vgrid, top, vgrid, H);            //wa6ahl
                            }

                            double new_fgrid = (vfoa_hz + fgrid) / 1000000;

                            label = fgrid.ToString();
                            offsetL = (int)((label.Length + 1) * 4.1);
                            offsetR = (int)(label.Length * 4.1);
                            if ((vgrid - offsetL >= 0) && (vgrid + offsetR < W) && (fgrid != 0))
                            {
                                if (bottom) g.DrawString(label, font, grid_text_brush, vgrid - offsetL, H + (float)Math.Floor(H * .01));
                                else g.DrawString(label, font, grid_text_brush, vgrid - offsetL, (float)Math.Floor(H * .01));
                            }
                        }
                    }
                    // draw band edge markers for bands not 60m
                    int[] band_edge_list_r3 = { 18068000, 18168000, 1810000, 2000000, 3500000, 3800000, 5250000,5450000,
				                                7000000, 7200000, 10100000, 10150000, 14000000, 14350000, 21000000, 21450000,
				                                24890000, 24990000, 28000000, 29700000, 50000000, 52000000, 144000000, 146000000 }; // ke9ns fix 50080000

                    for (int i = 0; i < band_edge_list_r3.Length; i++)
                    {
                        double band_edge_offset = band_edge_list_r3[i] - vfo;
                        if (band_edge_offset >= Low && band_edge_offset <= High)
                        {
                            int temp_vline = (int)((double)(band_edge_offset - Low) / (High - Low) * W);//wa6ahl
                            if (bottom) g.DrawLine(new Pen(band_edge_color), temp_vline, H + top, temp_vline, H + H);//wa6ahl
                            else g.DrawLine(new Pen(band_edge_color), temp_vline, top, temp_vline, H);//wa6ahl
                        }                        
                    }
                    // draw 60m band segment rectangles - UK+ only
                    int[] band_edge_list_r4 = { 5258500, 5264000, 5276000, 5284000, 5288500, 5292000, 
                                                5298000, 5307000, 5313000, 5323000, 5333000, 5338000, 
                                                5354000, 5358000, 5362000, 5374500, 5378000, 5382000, 
                                                5395000, 5401500, 5403500, 5406500 };

                    for (int i = 0; i < band_edge_list_r4.Length; i++)
                    {
                        double band_edge_offset = band_edge_list_r4[i] - vfo;
                        if (bottom)
                        {
                            if (is_first)
                            {
                                _x = (int)((double)(band_edge_offset - Low) / (High - Low) * W);
                                _y = H + top;
                                is_first = false;
                            }
                            else
                            {
                                _width = ((int)((double)(band_edge_offset - Low) / (High - Low) * W)) - _x;
                                _height = (H + H) - _y;
                                g.DrawRectangle(new Pen(band_box_color, band_box_width), new Rectangle(_x, _y, _width, _height));
                                is_first = true;
                            }
                        }
                        else
                        {
                            if (is_first)
                            {
                                _x = (int)((double)(band_edge_offset - Low) / (High - Low) * W);
                                _y = top;
                                is_first = false;
                            }
                            else
                            {
                                _width = ((int)((double)(band_edge_offset - Low) / (High - Low) * W)) - _x;
                                _height = H - _y;
                                g.DrawRectangle(new Pen(band_box_color, band_box_width), new Rectangle(_x, _y, _width, _height));
                                is_first = true;
                            }
                        }
                    } //  for (int i = 0; i < band_edge_list_r4.Length; i++)
#if (!NO_KE9NS)
                   // ke9ns add CB 11m band channels 1 - 40
                    for (int i = 0; i < band_edge_list_r77.Length; i++)
                    {
                        double band_edge_offset = band_edge_list_r77[i] - vfo;
                        if (bottom)
                        {
                            if (is_first)
                            {
                                _x = (int)((double)(band_edge_offset - Low) / (High - Low) * W);
                                _y = H + top;
                                is_first = false;
                            }
                            else
                            {
                                _width = ((int)((double)(band_edge_offset - Low) / (High - Low) * W)) - _x;
                                _height = (H + H) - _y;
                                g.DrawRectangle(new Pen(band_box_color, band_box_width), new Rectangle(_x, _y, _width, _height));
                                is_first = true;
                            }
                        }
                        else
                        {
                            if (is_first)
                            {
                                _x = (int)((double)(band_edge_offset - Low) / (High - Low) * W);
                                _y = top;
                                is_first = false;
                            }
                            else
                            {
                                _width = ((int)((double)(band_edge_offset - Low) / (High - Low) * W)) - _x;
                                _height = H - _y;
                                g.DrawRectangle(new Pen(band_box_color, band_box_width), new Rectangle(_x, _y, _width, _height));
                                is_first = true;
                            }
                        }
                    } // for loop for 11m
#endif                 
                    break; //   case FRSRegion.UK_Plus:

             

                //============================================================================================================
                //============================================================================================================
                //============================================================================================================
                //===============================================
                case FRSRegion.Europe:      // EU00 (IARU1 60m) & 51mhz 6m Germany
              

                    for (int i = 0; i < f_steps + 1; i++)
                    {
                        string label;
                        int offsetL;
                        int offsetR;

                        int fgrid = i * freq_step_size + (Low / freq_step_size) * freq_step_size;
                        double actual_fgrid = ((double)(vfo_round + fgrid)) / 1000000;
                        int vgrid = (int)((double)(fgrid - vfo_delta - Low) / (High - Low) * W);

                        if (!show_freq_offset)
                        {
                            if (actual_fgrid == 1.81 || actual_fgrid == 2.0 ||
                                actual_fgrid == 3.5 || actual_fgrid == 3.8 ||
                                actual_fgrid == 5.250 || actual_fgrid == 5.45 ||   // ke9ns add
                                actual_fgrid == 7.0 || actual_fgrid == 7.2 ||
                                actual_fgrid == 10.1 || actual_fgrid == 10.15 ||
                                actual_fgrid == 14.0 || actual_fgrid == 14.35 ||
                                actual_fgrid == 18.068 || actual_fgrid == 18.168 ||
                                actual_fgrid == 21.0 || actual_fgrid == 21.45 ||
                                actual_fgrid == 24.89 || actual_fgrid == 24.99 ||
                                actual_fgrid == 28.0 || actual_fgrid == 29.7 ||
                                actual_fgrid == 50.03 || actual_fgrid == 51.0 ||
                                actual_fgrid == 144.0 || actual_fgrid == 146.0)
                            {
                                if (bottom) g.DrawLine(new Pen(band_edge_color), vgrid, H + top, vgrid, H + H);
                                else g.DrawLine(new Pen(band_edge_color), vgrid, top, vgrid, H);

                                label = actual_fgrid.ToString("f4");
                                label = label.Replace(",", ".");    // handle Windows localization issues
                                int offset = label.IndexOf('.') + 4;
                                label = label.Insert(offset, ".");

                                if (actual_fgrid < 10) offsetL = (int)((label.Length + 2) * 4.1) - 14;
                                else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                else offsetL = (int)((label.Length) * 4.1) - 8;

                                if (bottom) g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid - offsetL, H + (float)Math.Floor(H * .01));
                                else g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid - offsetL, (float)Math.Floor(H * .01));

                                int fgrid_2 = ((i + 1) * freq_step_size) + (int)((Low / freq_step_size) * freq_step_size);
                                int x_2 = (int)(((float)(fgrid_2 - vfo_delta - Low) / width * W));
                                float scale = (float)(x_2 - vgrid) / inbetweenies;

                                for (int j = 1; j < inbetweenies; j++)
                                {
                                    if (grid_off == 0) // ke9ns add (dont draw grid lines if =1 
                                    {
                                        float x3 = (float)vgrid + (j * scale);
                                        if (bottom) g.DrawLine(grid_pen_dark, x3, H + top, x3, H + H);
                                        else g.DrawLine(grid_pen_dark, x3, top, x3, H);
                                    }
                                }
                            }
                            else
                            {
                                if (grid_off == 0) // ke9ns add (dont draw grid lines if =1 
                                {
                                    if (bottom) g.DrawLine(grid_pen, vgrid, H + top, vgrid, H + H);
                                    else g.DrawLine(grid_pen, vgrid, top, vgrid, H);            //wa6ahl
                                }

                                int fgrid_2 = ((i + 1) * freq_step_size) + (int)((Low / freq_step_size) * freq_step_size);
                                int x_2 = (int)(((float)(fgrid_2 - vfo_delta - Low) / width * W));
                                float scale = (float)(x_2 - vgrid) / inbetweenies;

                                for (int j = 1; j < inbetweenies; j++)
                                {
                                    if (grid_off == 0) // ke9ns add (dont draw grid lines if =1 
                                    {
                                        float x3 = (float)vgrid + (j * scale);
                                        if (bottom) g.DrawLine(grid_pen_dark, x3, H + top, x3, H + H);
                                        else g.DrawLine(grid_pen_dark, x3, top, x3, H);
                                    }
                                }
                                
                                double actual_fgrid_label = Math.Round(actual_fgrid, 4);
                                label = actual_fgrid_label.ToString("f4");
                                label = label.Replace(",", ".");    // handle Windows localization issues
                                int offset = label.IndexOf('.') + 4;
                                label = label.Insert(offset, ".");

                                if (actual_fgrid < 10) offsetL = (int)((label.Length + 2) * 4.1) - 14;
                                else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                else offsetL = (int)((label.Length) * 4.1) - 8;
                                
                                if (bottom) g.DrawString(label, font, grid_text_brush, vgrid - offsetL, H + (float)Math.Floor(H * .01));
                                else g.DrawString(label, font, grid_text_brush, vgrid - offsetL, (float)Math.Floor(H * .01));
                            }
                        }
                        else
                        {
                            vgrid = Convert.ToInt32((double)-(fgrid - Low) / (Low - High) * W); //wa6ahl
                            if (grid_off == 0) // ke9ns add (dont draw grid lines if =1 
                            {
                                if (bottom) g.DrawLine(grid_pen, vgrid, H + top, vgrid, H + H);
                                else g.DrawLine(grid_pen, vgrid, top, vgrid, H);            //wa6ahl
                            }
                            double new_fgrid = (vfoa_hz + fgrid) / 1000000;

                            label = fgrid.ToString();
                            offsetL = (int)((label.Length + 1) * 4.1);
                            offsetR = (int)(label.Length * 4.1);
                            if ((vgrid - offsetL >= 0) && (vgrid + offsetR < W) && (fgrid != 0))
                            {
                                if (bottom) g.DrawString(label, font, grid_text_brush, vgrid - offsetL, H + (float)Math.Floor(H * .01));
                                else g.DrawString(label, font, grid_text_brush, vgrid - offsetL, (float)Math.Floor(H * .01));
                            }
                        }
                    }

                    //---------------------------------------------------------------------------------------------------EU00
                    //---------------------------------------------------------------------------------------------------
                    int[] band_edge_list_r1 = { 1810000, 2000000,  3500000, 3800000, 5250000, 5450000,
                                                7000000, 7200000, 10100000, 10150000, 14000000, 14350000, 18068000, 18168000, 21000000, 21450000,
				                                24890000, 24990000, 28000000, 29700000, 50030000, 51000000, 144000000, 146000000 };

                    for (int i = 0; i < band_edge_list_r1.Length; i++)
                    {
                        double band_edge_offset = band_edge_list_r1[i] - vfo;
                        if (band_edge_offset >= Low && band_edge_offset <= High)
                        {
                            int temp_vline = (int)((double)(band_edge_offset - Low) / (High - Low) * W);//wa6ahl
                            if (bottom) g.DrawLine(new Pen(band_edge_color), temp_vline, H + top, temp_vline, H + H);//wa6ahl
                            else g.DrawLine(new Pen(band_edge_color), temp_vline, top, temp_vline, H);//wa6ahl
                        }
                    }

                    // draw 60m band segment rectangles - Sweden
                    //  int[] band_edge_list_r6 = { 5310000, 5313000, 5320000, 5323000, 5380000, 5383000, 5390000, 5393000 }; // ke9ns this was the old band plan before adopting IARU region 1 60m

                    //---------------------------------------------------------------------------------------------------
                    //---------------------------------------------------------------------------------------------------
                    // 60m edges
                  
                        int[] band_edge_list_r14 = {  5351500, 5353999, 5354000, 5365999, 5366000, 5366500  };

                        for (int i = 0; i < band_edge_list_r14.Length; i++)
                        {
                            double band_edge_offset = band_edge_list_r14[i] - vfo;
                            if (bottom)
                            {
                                if (is_first)
                                {
                                    _x = (int)((double)(band_edge_offset - Low) / (High - Low) * W);
                                    _y = H + top;
                                    is_first = false;
                                }
                                else
                                {
                                    _width = ((int)((double)(band_edge_offset - Low) / (High - Low) * W)) - _x;
                                    _height = (H + H) - _y;
                                    g.DrawRectangle(new Pen(band_box_color, band_box_width), new Rectangle(_x, _y, _width, _height));
                                    is_first = true;
                                }
                            }
                            else
                            {
                                if (is_first)
                                {
                                    _x = (int)((double)(band_edge_offset - Low) / (High - Low) * W);
                                    _y = top;
                                    is_first = false;
                                }
                                else
                                {
                                    _width = ((int)((double)(band_edge_offset - Low) / (High - Low) * W)) - _x;
                                    _height = H - _y;
                                    g.DrawRectangle(new Pen(band_box_color, band_box_width), new Rectangle(_x, _y, _width, _height));
                                    is_first = true;
                                }
                            }
                      

                    } // 60m europe
#if (!NO_KE9NS)
                    // ke9ns add CB 11m band channels 1 - 40
                    for (int i = 0; i < band_edge_list_r77.Length; i++)
                    {
                        double band_edge_offset = band_edge_list_r77[i] - vfo;
                        if (bottom)
                        {
                            if (is_first)
                            {
                                _x = (int)((double)(band_edge_offset - Low) / (High - Low) * W);
                                _y = H + top;
                                is_first = false;
                            }
                            else
                            {
                                _width = ((int)((double)(band_edge_offset - Low) / (High - Low) * W)) - _x;
                                _height = (H + H) - _y;
                                g.DrawRectangle(new Pen(band_box_color, band_box_width), new Rectangle(_x, _y, _width, _height));
                                is_first = true;
                            }
                        }
                        else
                        {
                            if (is_first)
                            {
                                _x = (int)((double)(band_edge_offset - Low) / (High - Low) * W);
                                _y = top;
                                is_first = false;
                            }
                            else
                            {
                                _width = ((int)((double)(band_edge_offset - Low) / (High - Low) * W)) - _x;
                                _height = H - _y;
                                g.DrawRectangle(new Pen(band_box_color, band_box_width), new Rectangle(_x, _y, _width, _height));
                                is_first = true;
                            }
                        }
                    } // for loop for 11m
#endif                 
                    break; // EU00






                //============================================================================================================
                //============================================================================================================
                //============================================================================================================
                //===============================================
                case FRSRegion.UK:          // EU01 (no 60m) & 52mhz 6m(UK+ has its own special segments)
                case FRSRegion.France:      // EU01 (no 60m)
                case FRSRegion.Slovakia:    // EU01 (no 60m)
                case FRSRegion.ES_CH_FIN:   // EU12 (IARU1 60m) & (52mhz for 6m)

                    for (int i = 0; i < f_steps + 1; i++)
                    {
                        string label;
                        int offsetL;
                        int offsetR;

                        int fgrid = i * freq_step_size + (Low / freq_step_size) * freq_step_size;
                        double actual_fgrid = ((double)(vfo_round + fgrid)) / 1000000;
                        int vgrid = (int)((double)(fgrid - vfo_delta - Low) / (High - Low) * W);

                        if (!show_freq_offset)
                        {
                            if (actual_fgrid == 1.81 || actual_fgrid == 2.0 ||
                                actual_fgrid == 3.5 || actual_fgrid == 3.8 ||
                                actual_fgrid == 5.250 || actual_fgrid == 5.45 ||   // ke9ns add
                                actual_fgrid == 7.0 || actual_fgrid == 7.2 ||
                                actual_fgrid == 10.1 || actual_fgrid == 10.15 ||
                                actual_fgrid == 14.0 || actual_fgrid == 14.35 ||
                                actual_fgrid == 18.068 || actual_fgrid == 18.168 ||
                                actual_fgrid == 21.0 || actual_fgrid == 21.45 ||
                                actual_fgrid == 24.89 || actual_fgrid == 24.99 ||
                                actual_fgrid == 28.0 || actual_fgrid == 29.7 ||
                                actual_fgrid == 50.0 || actual_fgrid == 52.0 ||
                                actual_fgrid == 144.0 || actual_fgrid == 146.0)
                            {
                                if (bottom) g.DrawLine(new Pen(band_edge_color), vgrid, H + top, vgrid, H + H);
                                else g.DrawLine(new Pen(band_edge_color), vgrid, top, vgrid, H);

                                label = actual_fgrid.ToString("f4");
                                label = label.Replace(",", ".");    // handle Windows localization issues
                                int offset = label.IndexOf('.') + 4;
                                label = label.Insert(offset, ".");

                                if (actual_fgrid < 10) offsetL = (int)((label.Length + 2) * 4.1) - 14;
                                else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                else offsetL = (int)((label.Length) * 4.1) - 8;

                                if (bottom) g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid - offsetL, H + (float)Math.Floor(H * .01));
                                else g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid - offsetL, (float)Math.Floor(H * .01));

                                int fgrid_2 = ((i + 1) * freq_step_size) + (int)((Low / freq_step_size) * freq_step_size);
                                int x_2 = (int)(((float)(fgrid_2 - vfo_delta - Low) / width * W));
                                float scale = (float)(x_2 - vgrid) / inbetweenies;

                                for (int j = 1; j < inbetweenies; j++)
                                {
                                    if (grid_off == 0) // ke9ns add (dont draw grid lines if =1 
                                    {
                                        float x3 = (float)vgrid + (j * scale);
                                        if (bottom) g.DrawLine(grid_pen_dark, x3, H + top, x3, H + H);
                                        else g.DrawLine(grid_pen_dark, x3, top, x3, H);
                                    }
                                }
                            }
                            else
                            {
                                if (grid_off == 0) // ke9ns add (dont draw grid lines if =1 
                                {
                                    if (bottom) g.DrawLine(grid_pen, vgrid, H + top, vgrid, H + H);
                                    else g.DrawLine(grid_pen, vgrid, top, vgrid, H);            //wa6ahl
                                }

                                int fgrid_2 = ((i + 1) * freq_step_size) + (int)((Low / freq_step_size) * freq_step_size);
                                int x_2 = (int)(((float)(fgrid_2 - vfo_delta - Low) / width * W));
                                float scale = (float)(x_2 - vgrid) / inbetweenies;

                                for (int j = 1; j < inbetweenies; j++)
                                {
                                    if (grid_off == 0) // ke9ns add (dont draw grid lines if =1 
                                    {
                                        float x3 = (float)vgrid + (j * scale);
                                        if (bottom) g.DrawLine(grid_pen_dark, x3, H + top, x3, H + H);
                                        else g.DrawLine(grid_pen_dark, x3, top, x3, H);
                                    }
                                }

                                double actual_fgrid_label = Math.Round(actual_fgrid, 4);
                                label = actual_fgrid_label.ToString("f4");
                                label = label.Replace(",", ".");    // handle Windows localization issues
                                int offset = label.IndexOf('.') + 4;
                                label = label.Insert(offset, ".");

                                if (actual_fgrid < 10) offsetL = (int)((label.Length + 2) * 4.1) - 14;
                                else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                else offsetL = (int)((label.Length) * 4.1) - 8;

                                if (bottom) g.DrawString(label, font, grid_text_brush, vgrid - offsetL, H + (float)Math.Floor(H * .01));
                                else g.DrawString(label, font, grid_text_brush, vgrid - offsetL, (float)Math.Floor(H * .01));
                            }
                        }
                        else
                        {
                            vgrid = Convert.ToInt32((double)-(fgrid - Low) / (Low - High) * W); //wa6ahl
                            if (grid_off == 0) // ke9ns add (dont draw grid lines if =1 
                            {
                                if (bottom) g.DrawLine(grid_pen, vgrid, H + top, vgrid, H + H);
                                else g.DrawLine(grid_pen, vgrid, top, vgrid, H);            //wa6ahl
                            }
                            double new_fgrid = (vfoa_hz + fgrid) / 1000000;

                            label = fgrid.ToString();
                            offsetL = (int)((label.Length + 1) * 4.1);
                            offsetR = (int)(label.Length * 4.1);
                            if ((vgrid - offsetL >= 0) && (vgrid + offsetR < W) && (fgrid != 0))
                            {
                                if (bottom) g.DrawString(label, font, grid_text_brush, vgrid - offsetL, H + (float)Math.Floor(H * .01));
                                else g.DrawString(label, font, grid_text_brush, vgrid - offsetL, (float)Math.Floor(H * .01));
                            }
                        }
                    }

                    //---------------------------------------------------------------------------------------------------EU01 and EU12
                    //---------------------------------------------------------------------------------------------------
                    int[] band_edge_list_r1a = { 1810000, 2000000,  3500000, 3800000, 5250000, 5450000,
                                                7000000, 7200000, 10100000, 10150000, 14000000, 14350000, 18068000, 18168000, 21000000, 21450000,
                                                24890000, 24990000, 28000000, 29700000, 50000000, 52000000, 144000000, 146000000 };

                    for (int i = 0; i < band_edge_list_r1a.Length; i++)
                    {
                        double band_edge_offset = band_edge_list_r1a[i] - vfo;
                        if (band_edge_offset >= Low && band_edge_offset <= High)
                        {
                            int temp_vline = (int)((double)(band_edge_offset - Low) / (High - Low) * W);//wa6ahl
                            if (bottom) g.DrawLine(new Pen(band_edge_color), temp_vline, H + top, temp_vline, H + H);//wa6ahl
                            else g.DrawLine(new Pen(band_edge_color), temp_vline, top, temp_vline, H);//wa6ahl
                        }
                    }

                   
                    //---------------------------------------------------------------------------------------------------
                    //---------------------------------------------------------------------------------------------------
                    // 60m edges  

                    int[] band_edge_list_r14a = { 5351500, 5353999, 5354000, 5365999, 5366000, 5366500 };

                    for (int i = 0; i < band_edge_list_r14a.Length; i++)
                    {
                        double band_edge_offset = band_edge_list_r14a[i] - vfo;
                        if (bottom)
                        {
                            if (is_first)
                            {
                                _x = (int)((double)(band_edge_offset - Low) / (High - Low) * W);
                                _y = H + top;
                                is_first = false;
                            }
                            else
                            {
                                _width = ((int)((double)(band_edge_offset - Low) / (High - Low) * W)) - _x;
                                _height = (H + H) - _y;
                                g.DrawRectangle(new Pen(band_box_color, band_box_width), new Rectangle(_x, _y, _width, _height));
                                is_first = true;
                            }
                        }
                        else
                        {
                            if (is_first)
                            {
                                _x = (int)((double)(band_edge_offset - Low) / (High - Low) * W);
                                _y = top;
                                is_first = false;
                            }
                            else
                            {
                                _width = ((int)((double)(band_edge_offset - Low) / (High - Low) * W)) - _x;
                                _height = H - _y;
                                g.DrawRectangle(new Pen(band_box_color, band_box_width), new Rectangle(_x, _y, _width, _height));
                                is_first = true;
                            }
                        }


                    } // 60m uk 
#if (!NO_KE9NS)
                    // ke9ns add CB 11m band channels 1 - 40
                    for (int i = 0; i < band_edge_list_r77.Length; i++)
                    {
                        double band_edge_offset = band_edge_list_r77[i] - vfo;
                        if (bottom)
                        {
                            if (is_first)
                            {
                                _x = (int)((double)(band_edge_offset - Low) / (High - Low) * W);
                                _y = H + top;
                                is_first = false;
                            }
                            else
                            {
                                _width = ((int)((double)(band_edge_offset - Low) / (High - Low) * W)) - _x;
                                _height = (H + H) - _y;
                                g.DrawRectangle(new Pen(band_box_color, band_box_width), new Rectangle(_x, _y, _width, _height));
                                is_first = true;
                            }
                        }
                        else
                        {
                            if (is_first)
                            {
                                _x = (int)((double)(band_edge_offset - Low) / (High - Low) * W);
                                _y = top;
                                is_first = false;
                            }
                            else
                            {
                                _width = ((int)((double)(band_edge_offset - Low) / (High - Low) * W)) - _x;
                                _height = H - _y;
                                g.DrawRectangle(new Pen(band_box_color, band_box_width), new Rectangle(_x, _y, _width, _height));
                                is_first = true;
                            }
                        }
                    } // for loop for 11m
#endif
                    break; // EU01, EU12




                //============================================================================================================
                //============================================================================================================
                //============================================================================================================
                //===============================================
                case FRSRegion.Norway:      // EU03  (5.25 to 5.45 60m) & 52mhz 6m) (also includes Czech rep)
                case FRSRegion.Denmark:     // EU03  (5.25 to 5.45 60m) & 52mhz 6m)
                case FRSRegion.EU_Travel:   // EU14 (5.25 to 5.45) & 52mhz for 6m)

                    for (int i = 0; i < f_steps + 1; i++)
                    {
                        string label;
                        int offsetL;
                        int offsetR;

                        int fgrid = i * freq_step_size + (Low / freq_step_size) * freq_step_size;
                        double actual_fgrid = ((double)(vfo_round + fgrid)) / 1000000;
                        int vgrid = (int)((double)(fgrid - vfo_delta - Low) / (High - Low) * W);

                        if (!show_freq_offset)
                        {
                            if (actual_fgrid == 1.81 || actual_fgrid == 2.0 ||
                                actual_fgrid == 3.5 || actual_fgrid == 3.8 ||
                                actual_fgrid == 5.250 || actual_fgrid == 5.45 ||   // ke9ns add
                                actual_fgrid == 7.0 || actual_fgrid == 7.2 ||
                                actual_fgrid == 10.1 || actual_fgrid == 10.15 ||
                                actual_fgrid == 14.0 || actual_fgrid == 14.35 ||
                                actual_fgrid == 18.068 || actual_fgrid == 18.168 ||
                                actual_fgrid == 21.0 || actual_fgrid == 21.45 ||
                                actual_fgrid == 24.89 || actual_fgrid == 24.99 ||
                                actual_fgrid == 28.0 || actual_fgrid == 29.7 ||
                                actual_fgrid == 50.0 || actual_fgrid == 52.0 ||
                                actual_fgrid == 144.0 || actual_fgrid == 146.0)
                            {
                                if (bottom) g.DrawLine(new Pen(band_edge_color), vgrid, H + top, vgrid, H + H);
                                else g.DrawLine(new Pen(band_edge_color), vgrid, top, vgrid, H);

                                label = actual_fgrid.ToString("f4");
                                label = label.Replace(",", ".");    // handle Windows localization issues
                                int offset = label.IndexOf('.') + 4;
                                label = label.Insert(offset, ".");

                                if (actual_fgrid < 10) offsetL = (int)((label.Length + 2) * 4.1) - 14;
                                else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                else offsetL = (int)((label.Length) * 4.1) - 8;

                                if (bottom) g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid - offsetL, H + (float)Math.Floor(H * .01));
                                else g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid - offsetL, (float)Math.Floor(H * .01));

                                int fgrid_2 = ((i + 1) * freq_step_size) + (int)((Low / freq_step_size) * freq_step_size);
                                int x_2 = (int)(((float)(fgrid_2 - vfo_delta - Low) / width * W));
                                float scale = (float)(x_2 - vgrid) / inbetweenies;

                                for (int j = 1; j < inbetweenies; j++)
                                {
                                    if (grid_off == 0) // ke9ns add (dont draw grid lines if =1 
                                    {
                                        float x3 = (float)vgrid + (j * scale);
                                        if (bottom) g.DrawLine(grid_pen_dark, x3, H + top, x3, H + H);
                                        else g.DrawLine(grid_pen_dark, x3, top, x3, H);
                                    }
                                }
                            }
                            else
                            {
                                if (grid_off == 0) // ke9ns add (dont draw grid lines if =1 
                                {
                                    if (bottom) g.DrawLine(grid_pen, vgrid, H + top, vgrid, H + H);
                                    else g.DrawLine(grid_pen, vgrid, top, vgrid, H);            //wa6ahl
                                }

                                int fgrid_2 = ((i + 1) * freq_step_size) + (int)((Low / freq_step_size) * freq_step_size);
                                int x_2 = (int)(((float)(fgrid_2 - vfo_delta - Low) / width * W));
                                float scale = (float)(x_2 - vgrid) / inbetweenies;

                                for (int j = 1; j < inbetweenies; j++)
                                {
                                    if (grid_off == 0) // ke9ns add (dont draw grid lines if =1 
                                    {
                                        float x3 = (float)vgrid + (j * scale);
                                        if (bottom) g.DrawLine(grid_pen_dark, x3, H + top, x3, H + H);
                                        else g.DrawLine(grid_pen_dark, x3, top, x3, H);
                                    }
                                }

                                double actual_fgrid_label = Math.Round(actual_fgrid, 4);
                                label = actual_fgrid_label.ToString("f4");
                                label = label.Replace(",", ".");    // handle Windows localization issues
                                int offset = label.IndexOf('.') + 4;
                                label = label.Insert(offset, ".");

                                if (actual_fgrid < 10) offsetL = (int)((label.Length + 2) * 4.1) - 14;
                                else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                else offsetL = (int)((label.Length) * 4.1) - 8;

                                if (bottom) g.DrawString(label, font, grid_text_brush, vgrid - offsetL, H + (float)Math.Floor(H * .01));
                                else g.DrawString(label, font, grid_text_brush, vgrid - offsetL, (float)Math.Floor(H * .01));
                            }
                        }
                        else
                        {
                            vgrid = Convert.ToInt32((double)-(fgrid - Low) / (Low - High) * W); //wa6ahl
                            if (grid_off == 0) // ke9ns add (dont draw grid lines if =1 
                            {
                                if (bottom) g.DrawLine(grid_pen, vgrid, H + top, vgrid, H + H);
                                else g.DrawLine(grid_pen, vgrid, top, vgrid, H);            //wa6ahl
                            }
                            double new_fgrid = (vfoa_hz + fgrid) / 1000000;

                            label = fgrid.ToString();
                            offsetL = (int)((label.Length + 1) * 4.1);
                            offsetR = (int)(label.Length * 4.1);
                            if ((vgrid - offsetL >= 0) && (vgrid + offsetR < W) && (fgrid != 0))
                            {
                                if (bottom) g.DrawString(label, font, grid_text_brush, vgrid - offsetL, H + (float)Math.Floor(H * .01));
                                else g.DrawString(label, font, grid_text_brush, vgrid - offsetL, (float)Math.Floor(H * .01));
                            }
                        }
                    }

                    //---------------------------------------------------------------------------------------------------EU03 and EU14
                    //---------------------------------------------------------------------------------------------------
                    int[] band_edge_list_r1b = { 1810000, 2000000,  3500000, 3800000, 5250000, 5450000,
                                                7000000, 7200000, 10100000, 10150000, 14000000, 14350000, 18068000, 18168000, 21000000, 21450000,
                                                24890000, 24990000, 28000000, 29700000, 50000000, 52000000, 144000000, 146000000 };

                    for (int i = 0; i < band_edge_list_r1b.Length; i++)
                    {
                        double band_edge_offset = band_edge_list_r1b[i] - vfo;
                        if (band_edge_offset >= Low && band_edge_offset <= High)
                        {
                            int temp_vline = (int)((double)(band_edge_offset - Low) / (High - Low) * W);//wa6ahl
                            if (bottom) g.DrawLine(new Pen(band_edge_color), temp_vline, H + top, temp_vline, H + H);//wa6ahl
                            else g.DrawLine(new Pen(band_edge_color), temp_vline, top, temp_vline, H);//wa6ahl
                        }
                    }

#if (!NO_KE9NS)
                    // ke9ns add CB 11m band channels 1 - 40
                    for (int i = 0; i < band_edge_list_r77.Length; i++)
                    {
                        double band_edge_offset = band_edge_list_r77[i] - vfo;
                        if (bottom)
                        {
                            if (is_first)
                            {
                                _x = (int)((double)(band_edge_offset - Low) / (High - Low) * W);
                                _y = H + top;
                                is_first = false;
                            }
                            else
                            {
                                _width = ((int)((double)(band_edge_offset - Low) / (High - Low) * W)) - _x;
                                _height = (H + H) - _y;
                                g.DrawRectangle(new Pen(band_box_color, band_box_width), new Rectangle(_x, _y, _width, _height));
                                is_first = true;
                            }
                        }
                        else
                        {
                            if (is_first)
                            {
                                _x = (int)((double)(band_edge_offset - Low) / (High - Low) * W);
                                _y = top;
                                is_first = false;
                            }
                            else
                            {
                                _width = ((int)((double)(band_edge_offset - Low) / (High - Low) * W)) - _x;
                                _height = H - _y;
                                g.DrawRectangle(new Pen(band_box_color, band_box_width), new Rectangle(_x, _y, _width, _height));
                                is_first = true;
                            }
                        }
                    } // for loop for 11m
#endif
                    break; // EU03, EU14



                //============================================================================================================
                //============================================================================================================
                //============================================================================================================
                //===============================================
                case FRSRegion.Belgium:     // EU04 (IARU1 60m)

                    for (int i = 0; i < f_steps + 1; i++)
                    {
                        string label;
                        int offsetL;
                        int offsetR;

                        int fgrid = i * freq_step_size + (Low / freq_step_size) * freq_step_size;
                        double actual_fgrid = ((double)(vfo_round + fgrid)) / 1000000;
                        int vgrid = (int)((double)(fgrid - vfo_delta - Low) / (High - Low) * W);

                        if (!show_freq_offset)
                        {
                            if (actual_fgrid == 1.81 || actual_fgrid == 1.88 ||
                                actual_fgrid == 3.5 || actual_fgrid == 3.8 ||
                                actual_fgrid == 5.250 || actual_fgrid == 5.45 ||   // ke9ns add
                                actual_fgrid == 7.0 || actual_fgrid == 7.2 ||
                                actual_fgrid == 10.1 || actual_fgrid == 10.15 ||
                                actual_fgrid == 14.0 || actual_fgrid == 14.35 ||
                                actual_fgrid == 18.068 || actual_fgrid == 18.168 ||
                                actual_fgrid == 21.0 || actual_fgrid == 21.45 ||
                                actual_fgrid == 24.89 || actual_fgrid == 24.99 ||
                                actual_fgrid == 28.0 || actual_fgrid == 29.7 ||
                                actual_fgrid == 50.0 || actual_fgrid == 52.0 ||
                                actual_fgrid == 144.0 || actual_fgrid == 146.0)
                            {
                                if (bottom) g.DrawLine(new Pen(band_edge_color), vgrid, H + top, vgrid, H + H);
                                else g.DrawLine(new Pen(band_edge_color), vgrid, top, vgrid, H);

                                label = actual_fgrid.ToString("f4");
                                label = label.Replace(",", ".");    // handle Windows localization issues
                                int offset = label.IndexOf('.') + 4;
                                label = label.Insert(offset, ".");

                                if (actual_fgrid < 10) offsetL = (int)((label.Length + 2) * 4.1) - 14;
                                else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                else offsetL = (int)((label.Length) * 4.1) - 8;

                                if (bottom) g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid - offsetL, H + (float)Math.Floor(H * .01));
                                else g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid - offsetL, (float)Math.Floor(H * .01));

                                int fgrid_2 = ((i + 1) * freq_step_size) + (int)((Low / freq_step_size) * freq_step_size);
                                int x_2 = (int)(((float)(fgrid_2 - vfo_delta - Low) / width * W));
                                float scale = (float)(x_2 - vgrid) / inbetweenies;

                                for (int j = 1; j < inbetweenies; j++)
                                {
                                    if (grid_off == 0) // ke9ns add (dont draw grid lines if =1 
                                    {
                                        float x3 = (float)vgrid + (j * scale);
                                        if (bottom) g.DrawLine(grid_pen_dark, x3, H + top, x3, H + H);
                                        else g.DrawLine(grid_pen_dark, x3, top, x3, H);
                                    }
                                }
                            }
                            else
                            {
                                if (grid_off == 0) // ke9ns add (dont draw grid lines if =1 
                                {
                                    if (bottom) g.DrawLine(grid_pen, vgrid, H + top, vgrid, H + H);
                                    else g.DrawLine(grid_pen, vgrid, top, vgrid, H);            //wa6ahl
                                }

                                int fgrid_2 = ((i + 1) * freq_step_size) + (int)((Low / freq_step_size) * freq_step_size);
                                int x_2 = (int)(((float)(fgrid_2 - vfo_delta - Low) / width * W));
                                float scale = (float)(x_2 - vgrid) / inbetweenies;

                                for (int j = 1; j < inbetweenies; j++)
                                {
                                    if (grid_off == 0) // ke9ns add (dont draw grid lines if =1 
                                    {
                                        float x3 = (float)vgrid + (j * scale);
                                        if (bottom) g.DrawLine(grid_pen_dark, x3, H + top, x3, H + H);
                                        else g.DrawLine(grid_pen_dark, x3, top, x3, H);
                                    }
                                }

                                double actual_fgrid_label = Math.Round(actual_fgrid, 4);
                                label = actual_fgrid_label.ToString("f4");
                                label = label.Replace(",", ".");    // handle Windows localization issues
                                int offset = label.IndexOf('.') + 4;
                                label = label.Insert(offset, ".");

                                if (actual_fgrid < 10) offsetL = (int)((label.Length + 2) * 4.1) - 14;
                                else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                else offsetL = (int)((label.Length) * 4.1) - 8;

                                if (bottom) g.DrawString(label, font, grid_text_brush, vgrid - offsetL, H + (float)Math.Floor(H * .01));
                                else g.DrawString(label, font, grid_text_brush, vgrid - offsetL, (float)Math.Floor(H * .01));
                            }
                        }
                        else
                        {
                            vgrid = Convert.ToInt32((double)-(fgrid - Low) / (Low - High) * W); //wa6ahl
                            if (grid_off == 0) // ke9ns add (dont draw grid lines if =1 
                            {
                                if (bottom) g.DrawLine(grid_pen, vgrid, H + top, vgrid, H + H);
                                else g.DrawLine(grid_pen, vgrid, top, vgrid, H);            //wa6ahl
                            }
                            double new_fgrid = (vfoa_hz + fgrid) / 1000000;

                            label = fgrid.ToString();
                            offsetL = (int)((label.Length + 1) * 4.1);
                            offsetR = (int)(label.Length * 4.1);
                            if ((vgrid - offsetL >= 0) && (vgrid + offsetR < W) && (fgrid != 0))
                            {
                                if (bottom) g.DrawString(label, font, grid_text_brush, vgrid - offsetL, H + (float)Math.Floor(H * .01));
                                else g.DrawString(label, font, grid_text_brush, vgrid - offsetL, (float)Math.Floor(H * .01));
                            }
                        }
                    }

                    //---------------------------------------------------------------------------------------------------EU04
                    //---------------------------------------------------------------------------------------------------
                    int[] band_edge_list_r1c = { 1810000, 1880000,  3500000, 3800000, 5250000, 5450000,
                                                7000000, 7200000, 10100000, 10150000, 14000000, 14350000, 18068000, 18168000, 21000000, 21450000,
                                                24890000, 24990000, 28000000, 29700000, 50000000, 52000000, 144000000, 146000000 };

                    for (int i = 0; i < band_edge_list_r1c.Length; i++)
                    {
                        double band_edge_offset = band_edge_list_r1c[i] - vfo;
                        if (band_edge_offset >= Low && band_edge_offset <= High)
                        {
                            int temp_vline = (int)((double)(band_edge_offset - Low) / (High - Low) * W);//wa6ahl
                            if (bottom) g.DrawLine(new Pen(band_edge_color), temp_vline, H + top, temp_vline, H + H);//wa6ahl
                            else g.DrawLine(new Pen(band_edge_color), temp_vline, top, temp_vline, H);//wa6ahl
                        }
                    }


                    //---------------------------------------------------------------------------------------------------
                    //---------------------------------------------------------------------------------------------------
                    // 60m edges  

                    int[] band_edge_list_r14c = { 5351500, 5353999, 5354000, 5365999, 5366000, 5366500 };

                    for (int i = 0; i < band_edge_list_r14c.Length; i++)
                    {
                        double band_edge_offset = band_edge_list_r14c[i] - vfo;
                        if (bottom)
                        {
                            if (is_first)
                            {
                                _x = (int)((double)(band_edge_offset - Low) / (High - Low) * W);
                                _y = H + top;
                                is_first = false;
                            }
                            else
                            {
                                _width = ((int)((double)(band_edge_offset - Low) / (High - Low) * W)) - _x;
                                _height = (H + H) - _y;
                                g.DrawRectangle(new Pen(band_box_color, band_box_width), new Rectangle(_x, _y, _width, _height));
                                is_first = true;
                            }
                        }
                        else
                        {
                            if (is_first)
                            {
                                _x = (int)((double)(band_edge_offset - Low) / (High - Low) * W);
                                _y = top;
                                is_first = false;
                            }
                            else
                            {
                                _width = ((int)((double)(band_edge_offset - Low) / (High - Low) * W)) - _x;
                                _height = H - _y;
                                g.DrawRectangle(new Pen(band_box_color, band_box_width), new Rectangle(_x, _y, _width, _height));
                                is_first = true;
                            }
                        }


                    } // 60m  
#if (!NO_KE9NS)
                    // ke9ns add CB 11m band channels 1 - 40
                    for (int i = 0; i < band_edge_list_r77.Length; i++)
                    {
                        double band_edge_offset = band_edge_list_r77[i] - vfo;
                        if (bottom)
                        {
                            if (is_first)
                            {
                                _x = (int)((double)(band_edge_offset - Low) / (High - Low) * W);
                                _y = H + top;
                                is_first = false;
                            }
                            else
                            {
                                _width = ((int)((double)(band_edge_offset - Low) / (High - Low) * W)) - _x;
                                _height = (H + H) - _y;
                                g.DrawRectangle(new Pen(band_box_color, band_box_width), new Rectangle(_x, _y, _width, _height));
                                is_first = true;
                            }
                        }
                        else
                        {
                            if (is_first)
                            {
                                _x = (int)((double)(band_edge_offset - Low) / (High - Low) * W);
                                _y = top;
                                is_first = false;
                            }
                            else
                            {
                                _width = ((int)((double)(band_edge_offset - Low) / (High - Low) * W)) - _x;
                                _height = H - _y;
                                g.DrawRectangle(new Pen(band_box_color, band_box_width), new Rectangle(_x, _y, _width, _height));
                                is_first = true;
                            }
                        }
                    } // for loop for 11m
#endif
                    break; // EU04



                //============================================================================================================
                //============================================================================================================
                //============================================================================================================
                //===============================================
                case FRSRegion.Hungary:     // EU05 (no 60m)

                    for (int i = 0; i < f_steps + 1; i++)
                    {
                        string label;
                        int offsetL;
                        int offsetR;

                        int fgrid = i * freq_step_size + (Low / freq_step_size) * freq_step_size;
                        double actual_fgrid = ((double)(vfo_round + fgrid)) / 1000000;
                        int vgrid = (int)((double)(fgrid - vfo_delta - Low) / (High - Low) * W);

                        if (!show_freq_offset)
                        {
                            if (actual_fgrid == 1.81 || actual_fgrid == 2.0 ||
                                actual_fgrid == 3.5 || actual_fgrid == 3.8 ||
                                actual_fgrid == 5.250 || actual_fgrid == 5.45 ||   // ke9ns add
                                actual_fgrid == 7.0 || actual_fgrid == 7.1 ||
                                actual_fgrid == 10.1 || actual_fgrid == 10.15 ||
                                actual_fgrid == 14.0 || actual_fgrid == 14.35 ||
                                actual_fgrid == 18.068 || actual_fgrid == 18.168 ||
                                actual_fgrid == 21.0 || actual_fgrid == 21.45 ||
                                actual_fgrid == 24.89 || actual_fgrid == 24.99 ||
                                actual_fgrid == 28.0 || actual_fgrid == 29.7 ||
                                actual_fgrid == 50.0 || actual_fgrid == 52.0 ||
                                actual_fgrid == 144.0 || actual_fgrid == 146.0)
                            {
                                if (bottom) g.DrawLine(new Pen(band_edge_color), vgrid, H + top, vgrid, H + H);
                                else g.DrawLine(new Pen(band_edge_color), vgrid, top, vgrid, H);

                                label = actual_fgrid.ToString("f4");
                                label = label.Replace(",", ".");    // handle Windows localization issues
                                int offset = label.IndexOf('.') + 4;
                                label = label.Insert(offset, ".");

                                if (actual_fgrid < 10) offsetL = (int)((label.Length + 2) * 4.1) - 14;
                                else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                else offsetL = (int)((label.Length) * 4.1) - 8;

                                if (bottom) g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid - offsetL, H + (float)Math.Floor(H * .01));
                                else g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid - offsetL, (float)Math.Floor(H * .01));

                                int fgrid_2 = ((i + 1) * freq_step_size) + (int)((Low / freq_step_size) * freq_step_size);
                                int x_2 = (int)(((float)(fgrid_2 - vfo_delta - Low) / width * W));
                                float scale = (float)(x_2 - vgrid) / inbetweenies;

                                for (int j = 1; j < inbetweenies; j++)
                                {
                                    if (grid_off == 0) // ke9ns add (dont draw grid lines if =1 
                                    {
                                        float x3 = (float)vgrid + (j * scale);
                                        if (bottom) g.DrawLine(grid_pen_dark, x3, H + top, x3, H + H);
                                        else g.DrawLine(grid_pen_dark, x3, top, x3, H);
                                    }
                                }
                            }
                            else
                            {
                                if (grid_off == 0) // ke9ns add (dont draw grid lines if =1 
                                {
                                    if (bottom) g.DrawLine(grid_pen, vgrid, H + top, vgrid, H + H);
                                    else g.DrawLine(grid_pen, vgrid, top, vgrid, H);            //wa6ahl
                                }

                                int fgrid_2 = ((i + 1) * freq_step_size) + (int)((Low / freq_step_size) * freq_step_size);
                                int x_2 = (int)(((float)(fgrid_2 - vfo_delta - Low) / width * W));
                                float scale = (float)(x_2 - vgrid) / inbetweenies;

                                for (int j = 1; j < inbetweenies; j++)
                                {
                                    if (grid_off == 0) // ke9ns add (dont draw grid lines if =1 
                                    {
                                        float x3 = (float)vgrid + (j * scale);
                                        if (bottom) g.DrawLine(grid_pen_dark, x3, H + top, x3, H + H);
                                        else g.DrawLine(grid_pen_dark, x3, top, x3, H);
                                    }
                                }

                                double actual_fgrid_label = Math.Round(actual_fgrid, 4);
                                label = actual_fgrid_label.ToString("f4");
                                label = label.Replace(",", ".");    // handle Windows localization issues
                                int offset = label.IndexOf('.') + 4;
                                label = label.Insert(offset, ".");

                                if (actual_fgrid < 10) offsetL = (int)((label.Length + 2) * 4.1) - 14;
                                else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                else offsetL = (int)((label.Length) * 4.1) - 8;

                                if (bottom) g.DrawString(label, font, grid_text_brush, vgrid - offsetL, H + (float)Math.Floor(H * .01));
                                else g.DrawString(label, font, grid_text_brush, vgrid - offsetL, (float)Math.Floor(H * .01));
                            }
                        }
                        else
                        {
                            vgrid = Convert.ToInt32((double)-(fgrid - Low) / (Low - High) * W); //wa6ahl
                            if (grid_off == 0) // ke9ns add (dont draw grid lines if =1 
                            {
                                if (bottom) g.DrawLine(grid_pen, vgrid, H + top, vgrid, H + H);
                                else g.DrawLine(grid_pen, vgrid, top, vgrid, H);            //wa6ahl
                            }
                            double new_fgrid = (vfoa_hz + fgrid) / 1000000;

                            label = fgrid.ToString();
                            offsetL = (int)((label.Length + 1) * 4.1);
                            offsetR = (int)(label.Length * 4.1);
                            if ((vgrid - offsetL >= 0) && (vgrid + offsetR < W) && (fgrid != 0))
                            {
                                if (bottom) g.DrawString(label, font, grid_text_brush, vgrid - offsetL, H + (float)Math.Floor(H * .01));
                                else g.DrawString(label, font, grid_text_brush, vgrid - offsetL, (float)Math.Floor(H * .01));
                            }
                        }
                    }

                    //---------------------------------------------------------------------------------------------------EU05
                    //---------------------------------------------------------------------------------------------------
                    int[] band_edge_list_r1d = { 1810000, 2000000,  3500000, 3800000, 5250000, 5450000,
                                                7000000, 7100000, 10100000, 10150000, 14000000, 14350000, 18068000, 18168000, 21000000, 21450000,
                                                24890000, 24990000, 28000000, 29700000, 50000000, 52000000, 144000000, 146000000 };

                    for (int i = 0; i < band_edge_list_r1d.Length; i++)
                    {
                        double band_edge_offset = band_edge_list_r1d[i] - vfo;
                        if (band_edge_offset >= Low && band_edge_offset <= High)
                        {
                            int temp_vline = (int)((double)(band_edge_offset - Low) / (High - Low) * W);//wa6ahl
                            if (bottom) g.DrawLine(new Pen(band_edge_color), temp_vline, H + top, temp_vline, H + H);//wa6ahl
                            else g.DrawLine(new Pen(band_edge_color), temp_vline, top, temp_vline, H);//wa6ahl
                        }
                    }


                    //---------------------------------------------------------------------------------------------------
                    //---------------------------------------------------------------------------------------------------
                    // 60m edges  

                    int[] band_edge_list_r14d = { 5351500, 5353999, 5354000, 5365999, 5366000, 5366500 };

                    for (int i = 0; i < band_edge_list_r14d.Length; i++)
                    {
                        double band_edge_offset = band_edge_list_r14d[i] - vfo;
                        if (bottom)
                        {
                            if (is_first)
                            {
                                _x = (int)((double)(band_edge_offset - Low) / (High - Low) * W);
                                _y = H + top;
                                is_first = false;
                            }
                            else
                            {
                                _width = ((int)((double)(band_edge_offset - Low) / (High - Low) * W)) - _x;
                                _height = (H + H) - _y;
                                g.DrawRectangle(new Pen(band_box_color, band_box_width), new Rectangle(_x, _y, _width, _height));
                                is_first = true;
                            }
                        }
                        else
                        {
                            if (is_first)
                            {
                                _x = (int)((double)(band_edge_offset - Low) / (High - Low) * W);
                                _y = top;
                                is_first = false;
                            }
                            else
                            {
                                _width = ((int)((double)(band_edge_offset - Low) / (High - Low) * W)) - _x;
                                _height = H - _y;
                                g.DrawRectangle(new Pen(band_box_color, band_box_width), new Rectangle(_x, _y, _width, _height));
                                is_first = true;
                            }
                        }


                    } // 60m  
#if (!NO_KE9NS)
                    // ke9ns add CB 11m band channels 1 - 40
                    for (int i = 0; i < band_edge_list_r77.Length; i++)
                    {
                        double band_edge_offset = band_edge_list_r77[i] - vfo;
                        if (bottom)
                        {
                            if (is_first)
                            {
                                _x = (int)((double)(band_edge_offset - Low) / (High - Low) * W);
                                _y = H + top;
                                is_first = false;
                            }
                            else
                            {
                                _width = ((int)((double)(band_edge_offset - Low) / (High - Low) * W)) - _x;
                                _height = (H + H) - _y;
                                g.DrawRectangle(new Pen(band_box_color, band_box_width), new Rectangle(_x, _y, _width, _height));
                                is_first = true;
                            }
                        }
                        else
                        {
                            if (is_first)
                            {
                                _x = (int)((double)(band_edge_offset - Low) / (High - Low) * W);
                                _y = top;
                                is_first = false;
                            }
                            else
                            {
                                _width = ((int)((double)(band_edge_offset - Low) / (High - Low) * W)) - _x;
                                _height = H - _y;
                                g.DrawRectangle(new Pen(band_box_color, band_box_width), new Rectangle(_x, _y, _width, _height));
                                is_first = true;
                            }
                        }
                    } // for loop for 11m
#endif
                    break; // EU05



                //============================================================================================================
                //============================================================================================================
                //============================================================================================================
                //===============================================
                 case FRSRegion.Sweden:      // EU06 (IARU1 60m) & (52mhz for 6m)
              
                    for (int i = 0; i < f_steps + 1; i++)
                    {
                        string label;
                        int offsetL;
                        int offsetR;

                        int fgrid = i * freq_step_size + (Low / freq_step_size) * freq_step_size;
                        double actual_fgrid = ((double)(vfo_round + fgrid)) / 1000000;
                        int vgrid = (int)((double)(fgrid - vfo_delta - Low) / (High - Low) * W);

                        if (!show_freq_offset)
                        {
                            if (actual_fgrid == 1.81 || actual_fgrid == 2.0 ||
                                actual_fgrid == 3.5 || actual_fgrid == 3.8 ||
                                actual_fgrid == 5.250 || actual_fgrid == 5.45 ||   // ke9ns add
                                actual_fgrid == 7.0 || actual_fgrid == 7.2 ||
                                actual_fgrid == 10.1 || actual_fgrid == 10.15 ||
                                actual_fgrid == 14.0 || actual_fgrid == 14.35 ||
                                actual_fgrid == 18.068 || actual_fgrid == 18.168 ||
                                actual_fgrid == 21.0 || actual_fgrid == 21.45 ||
                                actual_fgrid == 24.89 || actual_fgrid == 24.99 ||
                                actual_fgrid == 28.0 || actual_fgrid == 29.7 ||
                                actual_fgrid == 50.0 || actual_fgrid == 52.0 ||
                                actual_fgrid == 144.0 || actual_fgrid == 146.0)
                            {
                                if (bottom) g.DrawLine(new Pen(band_edge_color), vgrid, H + top, vgrid, H + H);
                                else g.DrawLine(new Pen(band_edge_color), vgrid, top, vgrid, H);

                                label = actual_fgrid.ToString("f4");
                                label = label.Replace(",", ".");    // handle Windows localization issues
                                int offset = label.IndexOf('.') + 4;
                                label = label.Insert(offset, ".");

                                if (actual_fgrid < 10) offsetL = (int)((label.Length + 2) * 4.1) - 14;
                                else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                else offsetL = (int)((label.Length) * 4.1) - 8;

                                if (bottom) g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid - offsetL, H + (float)Math.Floor(H * .01));
                                else g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid - offsetL, (float)Math.Floor(H * .01));

                                int fgrid_2 = ((i + 1) * freq_step_size) + (int)((Low / freq_step_size) * freq_step_size);
                                int x_2 = (int)(((float)(fgrid_2 - vfo_delta - Low) / width * W));
                                float scale = (float)(x_2 - vgrid) / inbetweenies;

                                for (int j = 1; j < inbetweenies; j++)
                                {
                                    if (grid_off == 0) // ke9ns add (dont draw grid lines if =1 
                                    {
                                        float x3 = (float)vgrid + (j * scale);
                                        if (bottom) g.DrawLine(grid_pen_dark, x3, H + top, x3, H + H);
                                        else g.DrawLine(grid_pen_dark, x3, top, x3, H);
                                    }
                                }
                            }
                            else
                            {
                                if (grid_off == 0) // ke9ns add (dont draw grid lines if =1 
                                {
                                    if (bottom) g.DrawLine(grid_pen, vgrid, H + top, vgrid, H + H);
                                    else g.DrawLine(grid_pen, vgrid, top, vgrid, H);            //wa6ahl
                                }

                                int fgrid_2 = ((i + 1) * freq_step_size) + (int)((Low / freq_step_size) * freq_step_size);
                                int x_2 = (int)(((float)(fgrid_2 - vfo_delta - Low) / width * W));
                                float scale = (float)(x_2 - vgrid) / inbetweenies;

                                for (int j = 1; j < inbetweenies; j++)
                                {
                                    if (grid_off == 0) // ke9ns add (dont draw grid lines if =1 
                                    {
                                        float x3 = (float)vgrid + (j * scale);
                                        if (bottom) g.DrawLine(grid_pen_dark, x3, H + top, x3, H + H);
                                        else g.DrawLine(grid_pen_dark, x3, top, x3, H);
                                    }
                                }

                                double actual_fgrid_label = Math.Round(actual_fgrid, 4);
                                label = actual_fgrid_label.ToString("f4");
                                label = label.Replace(",", ".");    // handle Windows localization issues
                                int offset = label.IndexOf('.') + 4;
                                label = label.Insert(offset, ".");

                                if (actual_fgrid < 10) offsetL = (int)((label.Length + 2) * 4.1) - 14;
                                else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                else offsetL = (int)((label.Length) * 4.1) - 8;

                                if (bottom) g.DrawString(label, font, grid_text_brush, vgrid - offsetL, H + (float)Math.Floor(H * .01));
                                else g.DrawString(label, font, grid_text_brush, vgrid - offsetL, (float)Math.Floor(H * .01));
                            }
                        }
                        else
                        {
                            vgrid = Convert.ToInt32((double)-(fgrid - Low) / (Low - High) * W); //wa6ahl
                            if (grid_off == 0) // ke9ns add (dont draw grid lines if =1 
                            {
                                if (bottom) g.DrawLine(grid_pen, vgrid, H + top, vgrid, H + H);
                                else g.DrawLine(grid_pen, vgrid, top, vgrid, H);            //wa6ahl
                            }
                            double new_fgrid = (vfoa_hz + fgrid) / 1000000;

                            label = fgrid.ToString();
                            offsetL = (int)((label.Length + 1) * 4.1);
                            offsetR = (int)(label.Length * 4.1);
                            if ((vgrid - offsetL >= 0) && (vgrid + offsetR < W) && (fgrid != 0))
                            {
                                if (bottom) g.DrawString(label, font, grid_text_brush, vgrid - offsetL, H + (float)Math.Floor(H * .01));
                                else g.DrawString(label, font, grid_text_brush, vgrid - offsetL, (float)Math.Floor(H * .01));
                            }
                        }
                    }

                    //--------------------------------------------------------------------------------------------------- EU06
                    //---------------------------------------------------------------------------------------------------
                    int[] band_edge_list_r1e = { 1810000, 2000000,  3500000, 3800000, 5250000, 5450000,
                                                7000000, 7200000, 10100000, 10150000, 14000000, 14350000, 18068000, 18168000, 21000000, 21450000,
                                                24890000, 24990000, 28000000, 29700000, 50000000, 52000000, 144000000, 146000000 };

                    for (int i = 0; i < band_edge_list_r1e.Length; i++)
                    {
                        double band_edge_offset = band_edge_list_r1e[i] - vfo;
                        if (band_edge_offset >= Low && band_edge_offset <= High)
                        {
                            int temp_vline = (int)((double)(band_edge_offset - Low) / (High - Low) * W);//wa6ahl
                            if (bottom) g.DrawLine(new Pen(band_edge_color), temp_vline, H + top, temp_vline, H + H);//wa6ahl
                            else g.DrawLine(new Pen(band_edge_color), temp_vline, top, temp_vline, H);//wa6ahl
                        }
                    }


                    //---------------------------------------------------------------------------------------------------
                    //---------------------------------------------------------------------------------------------------
                    // 60m edges  

                    int[] band_edge_list_r14e = { 5351500, 5353999, 5354000, 5365999, 5366000, 5366500 };

                    for (int i = 0; i < band_edge_list_r14e.Length; i++)
                    {
                        double band_edge_offset = band_edge_list_r14e[i] - vfo;
                        if (bottom)
                        {
                            if (is_first)
                            {
                                _x = (int)((double)(band_edge_offset - Low) / (High - Low) * W);
                                _y = H + top;
                                is_first = false;
                            }
                            else
                            {
                                _width = ((int)((double)(band_edge_offset - Low) / (High - Low) * W)) - _x;
                                _height = (H + H) - _y;
                                g.DrawRectangle(new Pen(band_box_color, band_box_width), new Rectangle(_x, _y, _width, _height));
                                is_first = true;
                            }
                        }
                        else
                        {
                            if (is_first)
                            {
                                _x = (int)((double)(band_edge_offset - Low) / (High - Low) * W);
                                _y = top;
                                is_first = false;
                            }
                            else
                            {
                                _width = ((int)((double)(band_edge_offset - Low) / (High - Low) * W)) - _x;
                                _height = H - _y;
                                g.DrawRectangle(new Pen(band_box_color, band_box_width), new Rectangle(_x, _y, _width, _height));
                                is_first = true;
                            }
                        }


                    } // 60m 

#if (!NO_KE9NS)
                    // ke9ns add CB 11m band channels 1 - 40
                    for (int i = 0; i < band_edge_list_r77.Length; i++)
                    {
                        double band_edge_offset = band_edge_list_r77[i] - vfo;
                        if (bottom)
                        {
                            if (is_first)
                            {
                                _x = (int)((double)(band_edge_offset - Low) / (High - Low) * W);
                                _y = H + top;
                                is_first = false;
                            }
                            else
                            {
                                _width = ((int)((double)(band_edge_offset - Low) / (High - Low) * W)) - _x;
                                _height = (H + H) - _y;
                                g.DrawRectangle(new Pen(band_box_color, band_box_width), new Rectangle(_x, _y, _width, _height));
                                is_first = true;
                            }
                        }
                        else
                        {
                            if (is_first)
                            {
                                _x = (int)((double)(band_edge_offset - Low) / (High - Low) * W);
                                _y = top;
                                is_first = false;
                            }
                            else
                            {
                                _width = ((int)((double)(band_edge_offset - Low) / (High - Low) * W)) - _x;
                                _height = H - _y;
                                g.DrawRectangle(new Pen(band_box_color, band_box_width), new Rectangle(_x, _y, _width, _height));
                                is_first = true;
                            }
                        }
                    } // for loop for 11m
#endif
                    break; // EU06



                //============================================================================================================
                //============================================================================================================
                //============================================================================================================
                //===============================================
                case FRSRegion.Bulgaria:    // EU07 (no 60m)

                    for (int i = 0; i < f_steps + 1; i++)
                    {
                        string label;
                        int offsetL;
                        int offsetR;

                        int fgrid = i * freq_step_size + (Low / freq_step_size) * freq_step_size;
                        double actual_fgrid = ((double)(vfo_round + fgrid)) / 1000000;
                        int vgrid = (int)((double)(fgrid - vfo_delta - Low) / (High - Low) * W);

                        if (!show_freq_offset)
                        {
                            if (actual_fgrid == 1.81 || actual_fgrid == 1.85 ||
                                actual_fgrid == 3.5 || actual_fgrid == 3.8 ||
                                actual_fgrid == 5.250 || actual_fgrid == 5.45 ||   // ke9ns add
                                actual_fgrid == 7.0 || actual_fgrid == 7.2 ||
                                actual_fgrid == 10.1 || actual_fgrid == 10.15 ||
                                actual_fgrid == 14.0 || actual_fgrid == 14.35 ||
                                actual_fgrid == 18.068 || actual_fgrid == 18.168 ||
                                actual_fgrid == 21.0 || actual_fgrid == 21.45 ||
                                actual_fgrid == 24.89 || actual_fgrid == 24.99 ||
                                actual_fgrid == 28.0 || actual_fgrid == 29.7 ||
                                actual_fgrid == 50.05 || actual_fgrid == 50.2 ||
                                actual_fgrid == 144.0 || actual_fgrid == 146.0)
                            {
                                if (bottom) g.DrawLine(new Pen(band_edge_color), vgrid, H + top, vgrid, H + H);
                                else g.DrawLine(new Pen(band_edge_color), vgrid, top, vgrid, H);

                                label = actual_fgrid.ToString("f4");
                                label = label.Replace(",", ".");    // handle Windows localization issues
                                int offset = label.IndexOf('.') + 4;
                                label = label.Insert(offset, ".");

                                if (actual_fgrid < 10) offsetL = (int)((label.Length + 2) * 4.1) - 14;
                                else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                else offsetL = (int)((label.Length) * 4.1) - 8;

                                if (bottom) g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid - offsetL, H + (float)Math.Floor(H * .01));
                                else g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid - offsetL, (float)Math.Floor(H * .01));

                                int fgrid_2 = ((i + 1) * freq_step_size) + (int)((Low / freq_step_size) * freq_step_size);
                                int x_2 = (int)(((float)(fgrid_2 - vfo_delta - Low) / width * W));
                                float scale = (float)(x_2 - vgrid) / inbetweenies;

                                for (int j = 1; j < inbetweenies; j++)
                                {
                                    if (grid_off == 0) // ke9ns add (dont draw grid lines if =1 
                                    {
                                        float x3 = (float)vgrid + (j * scale);
                                        if (bottom) g.DrawLine(grid_pen_dark, x3, H + top, x3, H + H);
                                        else g.DrawLine(grid_pen_dark, x3, top, x3, H);
                                    }
                                }
                            }
                            else
                            {
                                if (grid_off == 0) // ke9ns add (dont draw grid lines if =1 
                                {
                                    if (bottom) g.DrawLine(grid_pen, vgrid, H + top, vgrid, H + H);
                                    else g.DrawLine(grid_pen, vgrid, top, vgrid, H);            //wa6ahl
                                }

                                int fgrid_2 = ((i + 1) * freq_step_size) + (int)((Low / freq_step_size) * freq_step_size);
                                int x_2 = (int)(((float)(fgrid_2 - vfo_delta - Low) / width * W));
                                float scale = (float)(x_2 - vgrid) / inbetweenies;

                                for (int j = 1; j < inbetweenies; j++)
                                {
                                    if (grid_off == 0) // ke9ns add (dont draw grid lines if =1 
                                    {
                                        float x3 = (float)vgrid + (j * scale);
                                        if (bottom) g.DrawLine(grid_pen_dark, x3, H + top, x3, H + H);
                                        else g.DrawLine(grid_pen_dark, x3, top, x3, H);
                                    }
                                }

                                double actual_fgrid_label = Math.Round(actual_fgrid, 4);
                                label = actual_fgrid_label.ToString("f4");
                                label = label.Replace(",", ".");    // handle Windows localization issues
                                int offset = label.IndexOf('.') + 4;
                                label = label.Insert(offset, ".");

                                if (actual_fgrid < 10) offsetL = (int)((label.Length + 2) * 4.1) - 14;
                                else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                else offsetL = (int)((label.Length) * 4.1) - 8;

                                if (bottom) g.DrawString(label, font, grid_text_brush, vgrid - offsetL, H + (float)Math.Floor(H * .01));
                                else g.DrawString(label, font, grid_text_brush, vgrid - offsetL, (float)Math.Floor(H * .01));
                            }
                        }
                        else
                        {
                            vgrid = Convert.ToInt32((double)-(fgrid - Low) / (Low - High) * W); //wa6ahl
                            if (grid_off == 0) // ke9ns add (dont draw grid lines if =1 
                            {
                                if (bottom) g.DrawLine(grid_pen, vgrid, H + top, vgrid, H + H);
                                else g.DrawLine(grid_pen, vgrid, top, vgrid, H);            //wa6ahl
                            }
                            double new_fgrid = (vfoa_hz + fgrid) / 1000000;

                            label = fgrid.ToString();
                            offsetL = (int)((label.Length + 1) * 4.1);
                            offsetR = (int)(label.Length * 4.1);
                            if ((vgrid - offsetL >= 0) && (vgrid + offsetR < W) && (fgrid != 0))
                            {
                                if (bottom) g.DrawString(label, font, grid_text_brush, vgrid - offsetL, H + (float)Math.Floor(H * .01));
                                else g.DrawString(label, font, grid_text_brush, vgrid - offsetL, (float)Math.Floor(H * .01));
                            }
                        }
                    }

                    //---------------------------------------------------------------------------------------------------EU07
                    //---------------------------------------------------------------------------------------------------
                    int[] band_edge_list_r1f = { 1810000, 1850000,  3500000, 3800000, 5250000, 5450000,
                                                7000000, 7200000, 10100000, 10150000, 14000000, 14350000, 18068000, 18168000, 21000000, 21450000,
                                                24890000, 24990000, 28000000, 29700000, 50050000, 50200000, 144000000, 146000000 };

                    for (int i = 0; i < band_edge_list_r1f.Length; i++)
                    {
                        double band_edge_offset = band_edge_list_r1f[i] - vfo;
                        if (band_edge_offset >= Low && band_edge_offset <= High)
                        {
                            int temp_vline = (int)((double)(band_edge_offset - Low) / (High - Low) * W);//wa6ahl
                            if (bottom) g.DrawLine(new Pen(band_edge_color), temp_vline, H + top, temp_vline, H + H);//wa6ahl
                            else g.DrawLine(new Pen(band_edge_color), temp_vline, top, temp_vline, H);//wa6ahl
                        }
                    }


                    //---------------------------------------------------------------------------------------------------
                    //---------------------------------------------------------------------------------------------------
                    // 60m edges  

                    int[] band_edge_list_r14f = { 5351500, 5353999, 5354000, 5365999, 5366000, 5366500 };

                    for (int i = 0; i < band_edge_list_r14f.Length; i++)
                    {
                        double band_edge_offset = band_edge_list_r14f[i] - vfo;
                        if (bottom)
                        {
                            if (is_first)
                            {
                                _x = (int)((double)(band_edge_offset - Low) / (High - Low) * W);
                                _y = H + top;
                                is_first = false;
                            }
                            else
                            {
                                _width = ((int)((double)(band_edge_offset - Low) / (High - Low) * W)) - _x;
                                _height = (H + H) - _y;
                                g.DrawRectangle(new Pen(band_box_color, band_box_width), new Rectangle(_x, _y, _width, _height));
                                is_first = true;
                            }
                        }
                        else
                        {
                            if (is_first)
                            {
                                _x = (int)((double)(band_edge_offset - Low) / (High - Low) * W);
                                _y = top;
                                is_first = false;
                            }
                            else
                            {
                                _width = ((int)((double)(band_edge_offset - Low) / (High - Low) * W)) - _x;
                                _height = H - _y;
                                g.DrawRectangle(new Pen(band_box_color, band_box_width), new Rectangle(_x, _y, _width, _height));
                                is_first = true;
                            }
                        }


                    } // 60m 
#if (!NO_KE9NS)
                    // ke9ns add CB 11m band channels 1 - 40
                    for (int i = 0; i < band_edge_list_r77.Length; i++)
                    {
                        double band_edge_offset = band_edge_list_r77[i] - vfo;
                        if (bottom)
                        {
                            if (is_first)
                            {
                                _x = (int)((double)(band_edge_offset - Low) / (High - Low) * W);
                                _y = H + top;
                                is_first = false;
                            }
                            else
                            {
                                _width = ((int)((double)(band_edge_offset - Low) / (High - Low) * W)) - _x;
                                _height = (H + H) - _y;
                                g.DrawRectangle(new Pen(band_box_color, band_box_width), new Rectangle(_x, _y, _width, _height));
                                is_first = true;
                            }
                        }
                        else
                        {
                            if (is_first)
                            {
                                _x = (int)((double)(band_edge_offset - Low) / (High - Low) * W);
                                _y = top;
                                is_first = false;
                            }
                            else
                            {
                                _width = ((int)((double)(band_edge_offset - Low) / (High - Low) * W)) - _x;
                                _height = H - _y;
                                g.DrawRectangle(new Pen(band_box_color, band_box_width), new Rectangle(_x, _y, _width, _height));
                                is_first = true;
                            }
                        }
                    } // for loop for 11m
#endif
                    break; // EU07





                //============================================================================================================
                //============================================================================================================
                //============================================================================================================
                //===============================================
                case FRSRegion.Latvia:      // EU08 (no 60m)

                    for (int i = 0; i < f_steps + 1; i++)
                    {
                        string label;
                        int offsetL;
                        int offsetR;

                        int fgrid = i * freq_step_size + (Low / freq_step_size) * freq_step_size;
                        double actual_fgrid = ((double)(vfo_round + fgrid)) / 1000000;
                        int vgrid = (int)((double)(fgrid - vfo_delta - Low) / (High - Low) * W);

                        if (!show_freq_offset)
                        {
                            if (actual_fgrid == 1.81 || actual_fgrid == 2.00 ||
                                actual_fgrid == 3.5 || actual_fgrid == 3.8 ||
                                actual_fgrid == 5.250 || actual_fgrid == 5.45 ||   // ke9ns add
                                actual_fgrid == 7.0 || actual_fgrid == 7.2 ||
                                actual_fgrid == 10.1 || actual_fgrid == 10.15 ||
                                actual_fgrid == 14.0 || actual_fgrid == 14.35 ||
                                actual_fgrid == 18.068 || actual_fgrid == 18.168 ||
                                actual_fgrid == 21.0 || actual_fgrid == 21.45 ||
                                actual_fgrid == 24.89 || actual_fgrid == 24.99 ||
                                actual_fgrid == 28.0 || actual_fgrid == 29.7 ||
                                actual_fgrid == 50.0 || actual_fgrid == 51.0 ||
                                actual_fgrid == 144.0 || actual_fgrid == 146.0)
                            {
                                if (bottom) g.DrawLine(new Pen(band_edge_color), vgrid, H + top, vgrid, H + H);
                                else g.DrawLine(new Pen(band_edge_color), vgrid, top, vgrid, H);

                                label = actual_fgrid.ToString("f4");
                                label = label.Replace(",", ".");    // handle Windows localization issues
                                int offset = label.IndexOf('.') + 4;
                                label = label.Insert(offset, ".");

                                if (actual_fgrid < 10) offsetL = (int)((label.Length + 2) * 4.1) - 14;
                                else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                else offsetL = (int)((label.Length) * 4.1) - 8;

                                if (bottom) g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid - offsetL, H + (float)Math.Floor(H * .01));
                                else g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid - offsetL, (float)Math.Floor(H * .01));

                                int fgrid_2 = ((i + 1) * freq_step_size) + (int)((Low / freq_step_size) * freq_step_size);
                                int x_2 = (int)(((float)(fgrid_2 - vfo_delta - Low) / width * W));
                                float scale = (float)(x_2 - vgrid) / inbetweenies;

                                for (int j = 1; j < inbetweenies; j++)
                                {
                                    if (grid_off == 0) // ke9ns add (dont draw grid lines if =1 
                                    {
                                        float x3 = (float)vgrid + (j * scale);
                                        if (bottom) g.DrawLine(grid_pen_dark, x3, H + top, x3, H + H);
                                        else g.DrawLine(grid_pen_dark, x3, top, x3, H);
                                    }
                                }
                            }
                            else
                            {
                                if (grid_off == 0) // ke9ns add (dont draw grid lines if =1 
                                {
                                    if (bottom) g.DrawLine(grid_pen, vgrid, H + top, vgrid, H + H);
                                    else g.DrawLine(grid_pen, vgrid, top, vgrid, H);            //wa6ahl
                                }

                                int fgrid_2 = ((i + 1) * freq_step_size) + (int)((Low / freq_step_size) * freq_step_size);
                                int x_2 = (int)(((float)(fgrid_2 - vfo_delta - Low) / width * W));
                                float scale = (float)(x_2 - vgrid) / inbetweenies;

                                for (int j = 1; j < inbetweenies; j++)
                                {
                                    if (grid_off == 0) // ke9ns add (dont draw grid lines if =1 
                                    {
                                        float x3 = (float)vgrid + (j * scale);
                                        if (bottom) g.DrawLine(grid_pen_dark, x3, H + top, x3, H + H);
                                        else g.DrawLine(grid_pen_dark, x3, top, x3, H);
                                    }
                                }

                                double actual_fgrid_label = Math.Round(actual_fgrid, 4);
                                label = actual_fgrid_label.ToString("f4");
                                label = label.Replace(",", ".");    // handle Windows localization issues
                                int offset = label.IndexOf('.') + 4;
                                label = label.Insert(offset, ".");

                                if (actual_fgrid < 10) offsetL = (int)((label.Length + 2) * 4.1) - 14;
                                else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                else offsetL = (int)((label.Length) * 4.1) - 8;

                                if (bottom) g.DrawString(label, font, grid_text_brush, vgrid - offsetL, H + (float)Math.Floor(H * .01));
                                else g.DrawString(label, font, grid_text_brush, vgrid - offsetL, (float)Math.Floor(H * .01));
                            }
                        }
                        else
                        {
                            vgrid = Convert.ToInt32((double)-(fgrid - Low) / (Low - High) * W); //wa6ahl
                            if (grid_off == 0) // ke9ns add (dont draw grid lines if =1 
                            {
                                if (bottom) g.DrawLine(grid_pen, vgrid, H + top, vgrid, H + H);
                                else g.DrawLine(grid_pen, vgrid, top, vgrid, H);            //wa6ahl
                            }
                            double new_fgrid = (vfoa_hz + fgrid) / 1000000;

                            label = fgrid.ToString();
                            offsetL = (int)((label.Length + 1) * 4.1);
                            offsetR = (int)(label.Length * 4.1);
                            if ((vgrid - offsetL >= 0) && (vgrid + offsetR < W) && (fgrid != 0))
                            {
                                if (bottom) g.DrawString(label, font, grid_text_brush, vgrid - offsetL, H + (float)Math.Floor(H * .01));
                                else g.DrawString(label, font, grid_text_brush, vgrid - offsetL, (float)Math.Floor(H * .01));
                            }
                        }
                    }

                    //---------------------------------------------------------------------------------------------------EU08
                    //---------------------------------------------------------------------------------------------------
                    int[] band_edge_list_r1g = { 1810000, 2000000,  3500000, 3800000, 5250000, 5450000,
                                                7000000, 7200000, 10100000, 10150000, 14000000, 14350000, 18068000, 18168000, 21000000, 21450000,
                                                24890000, 24990000, 28000000, 29700000, 50000000, 51000000, 144000000, 146000000 };

                    for (int i = 0; i < band_edge_list_r1g.Length; i++)
                    {
                        double band_edge_offset = band_edge_list_r1g[i] - vfo;
                        if (band_edge_offset >= Low && band_edge_offset <= High)
                        {
                            int temp_vline = (int)((double)(band_edge_offset - Low) / (High - Low) * W);//wa6ahl
                            if (bottom) g.DrawLine(new Pen(band_edge_color), temp_vline, H + top, temp_vline, H + H);//wa6ahl
                            else g.DrawLine(new Pen(band_edge_color), temp_vline, top, temp_vline, H);//wa6ahl
                        }
                    }


                    //---------------------------------------------------------------------------------------------------
                    //---------------------------------------------------------------------------------------------------
                    // 60m edges  

                    int[] band_edge_list_r14g = { 5351500, 5353999, 5354000, 5365999, 5366000, 5366500 };

                    for (int i = 0; i < band_edge_list_r14g.Length; i++)
                    {
                        double band_edge_offset = band_edge_list_r14g[i] - vfo;
                        if (bottom)
                        {
                            if (is_first)
                            {
                                _x = (int)((double)(band_edge_offset - Low) / (High - Low) * W);
                                _y = H + top;
                                is_first = false;
                            }
                            else
                            {
                                _width = ((int)((double)(band_edge_offset - Low) / (High - Low) * W)) - _x;
                                _height = (H + H) - _y;
                                g.DrawRectangle(new Pen(band_box_color, band_box_width), new Rectangle(_x, _y, _width, _height));
                                is_first = true;
                            }
                        }
                        else
                        {
                            if (is_first)
                            {
                                _x = (int)((double)(band_edge_offset - Low) / (High - Low) * W);
                                _y = top;
                                is_first = false;
                            }
                            else
                            {
                                _width = ((int)((double)(band_edge_offset - Low) / (High - Low) * W)) - _x;
                                _height = H - _y;
                                g.DrawRectangle(new Pen(band_box_color, band_box_width), new Rectangle(_x, _y, _width, _height));
                                is_first = true;
                            }
                        }


                    } // 60m 
#if (!NO_KE9NS)
                    // ke9ns add CB 11m band channels 1 - 40
                    for (int i = 0; i < band_edge_list_r77.Length; i++)
                    {
                        double band_edge_offset = band_edge_list_r77[i] - vfo;
                        if (bottom)
                        {
                            if (is_first)
                            {
                                _x = (int)((double)(band_edge_offset - Low) / (High - Low) * W);
                                _y = H + top;
                                is_first = false;
                            }
                            else
                            {
                                _width = ((int)((double)(band_edge_offset - Low) / (High - Low) * W)) - _x;
                                _height = (H + H) - _y;
                                g.DrawRectangle(new Pen(band_box_color, band_box_width), new Rectangle(_x, _y, _width, _height));
                                is_first = true;
                            }
                        }
                        else
                        {
                            if (is_first)
                            {
                                _x = (int)((double)(band_edge_offset - Low) / (High - Low) * W);
                                _y = top;
                                is_first = false;
                            }
                            else
                            {
                                _width = ((int)((double)(band_edge_offset - Low) / (High - Low) * W)) - _x;
                                _height = H - _y;
                                g.DrawRectangle(new Pen(band_box_color, band_box_width), new Rectangle(_x, _y, _width, _height));
                                is_first = true;
                            }
                        }
                    } // for loop for 11m
#endif
                    break; // EU08



                //============================================================================================================
                //============================================================================================================
                //============================================================================================================
                //===============================================
                case FRSRegion.Greece:      // EU09 (no 60m)

                    for (int i = 0; i < f_steps + 1; i++)
                    {
                        string label;
                        int offsetL;
                        int offsetR;

                        int fgrid = i * freq_step_size + (Low / freq_step_size) * freq_step_size;
                        double actual_fgrid = ((double)(vfo_round + fgrid)) / 1000000;
                        int vgrid = (int)((double)(fgrid - vfo_delta - Low) / (High - Low) * W);

                        if (!show_freq_offset)
                        {
                            if (actual_fgrid == 1.81 || actual_fgrid == 1.85 ||
                                actual_fgrid == 3.5 || actual_fgrid == 3.8 ||
                                actual_fgrid == 5.250 || actual_fgrid == 5.45 ||   // ke9ns add
                                actual_fgrid == 7.0 || actual_fgrid == 7.2 ||
                                actual_fgrid == 10.1 || actual_fgrid == 10.15 ||
                                actual_fgrid == 14.0 || actual_fgrid == 14.35 ||
                                actual_fgrid == 18.068 || actual_fgrid == 18.168 ||
                                actual_fgrid == 21.0 || actual_fgrid == 21.45 ||
                                actual_fgrid == 24.89 || actual_fgrid == 24.99 ||
                                actual_fgrid == 28.0 || actual_fgrid == 29.7 ||
                                actual_fgrid == 50.0 || actual_fgrid == 52.0 ||
                                actual_fgrid == 144.0 || actual_fgrid == 146.0)
                            {
                                if (bottom) g.DrawLine(new Pen(band_edge_color), vgrid, H + top, vgrid, H + H);
                                else g.DrawLine(new Pen(band_edge_color), vgrid, top, vgrid, H);

                                label = actual_fgrid.ToString("f4");
                                label = label.Replace(",", ".");    // handle Windows localization issues
                                int offset = label.IndexOf('.') + 4;
                                label = label.Insert(offset, ".");

                                if (actual_fgrid < 10) offsetL = (int)((label.Length + 2) * 4.1) - 14;
                                else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                else offsetL = (int)((label.Length) * 4.1) - 8;

                                if (bottom) g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid - offsetL, H + (float)Math.Floor(H * .01));
                                else g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid - offsetL, (float)Math.Floor(H * .01));

                                int fgrid_2 = ((i + 1) * freq_step_size) + (int)((Low / freq_step_size) * freq_step_size);
                                int x_2 = (int)(((float)(fgrid_2 - vfo_delta - Low) / width * W));
                                float scale = (float)(x_2 - vgrid) / inbetweenies;

                                for (int j = 1; j < inbetweenies; j++)
                                {
                                    if (grid_off == 0) // ke9ns add (dont draw grid lines if =1 
                                    {
                                        float x3 = (float)vgrid + (j * scale);
                                        if (bottom) g.DrawLine(grid_pen_dark, x3, H + top, x3, H + H);
                                        else g.DrawLine(grid_pen_dark, x3, top, x3, H);
                                    }
                                }
                            }
                            else
                            {
                                if (grid_off == 0) // ke9ns add (dont draw grid lines if =1 
                                {
                                    if (bottom) g.DrawLine(grid_pen, vgrid, H + top, vgrid, H + H);
                                    else g.DrawLine(grid_pen, vgrid, top, vgrid, H);            //wa6ahl
                                }

                                int fgrid_2 = ((i + 1) * freq_step_size) + (int)((Low / freq_step_size) * freq_step_size);
                                int x_2 = (int)(((float)(fgrid_2 - vfo_delta - Low) / width * W));
                                float scale = (float)(x_2 - vgrid) / inbetweenies;

                                for (int j = 1; j < inbetweenies; j++)
                                {
                                    if (grid_off == 0) // ke9ns add (dont draw grid lines if =1 
                                    {
                                        float x3 = (float)vgrid + (j * scale);
                                        if (bottom) g.DrawLine(grid_pen_dark, x3, H + top, x3, H + H);
                                        else g.DrawLine(grid_pen_dark, x3, top, x3, H);
                                    }
                                }

                                double actual_fgrid_label = Math.Round(actual_fgrid, 4);
                                label = actual_fgrid_label.ToString("f4");
                                label = label.Replace(",", ".");    // handle Windows localization issues
                                int offset = label.IndexOf('.') + 4;
                                label = label.Insert(offset, ".");

                                if (actual_fgrid < 10) offsetL = (int)((label.Length + 2) * 4.1) - 14;
                                else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                else offsetL = (int)((label.Length) * 4.1) - 8;

                                if (bottom) g.DrawString(label, font, grid_text_brush, vgrid - offsetL, H + (float)Math.Floor(H * .01));
                                else g.DrawString(label, font, grid_text_brush, vgrid - offsetL, (float)Math.Floor(H * .01));
                            }
                        }
                        else
                        {
                            vgrid = Convert.ToInt32((double)-(fgrid - Low) / (Low - High) * W); //wa6ahl
                            if (grid_off == 0) // ke9ns add (dont draw grid lines if =1 
                            {
                                if (bottom) g.DrawLine(grid_pen, vgrid, H + top, vgrid, H + H);
                                else g.DrawLine(grid_pen, vgrid, top, vgrid, H);            //wa6ahl
                            }
                            double new_fgrid = (vfoa_hz + fgrid) / 1000000;

                            label = fgrid.ToString();
                            offsetL = (int)((label.Length + 1) * 4.1);
                            offsetR = (int)(label.Length * 4.1);
                            if ((vgrid - offsetL >= 0) && (vgrid + offsetR < W) && (fgrid != 0))
                            {
                                if (bottom) g.DrawString(label, font, grid_text_brush, vgrid - offsetL, H + (float)Math.Floor(H * .01));
                                else g.DrawString(label, font, grid_text_brush, vgrid - offsetL, (float)Math.Floor(H * .01));
                            }
                        }
                    }

                    //--------------------------------------------------------------------------------------------------- EU09
                    //---------------------------------------------------------------------------------------------------
                    int[] band_edge_list_r1h = { 1810000, 1850000,  3500000, 3800000, 5250000, 5450000,
                                                7000000, 7200000, 10100000, 10150000, 14000000, 14350000, 18068000, 18168000, 21000000, 21450000,
                                                24890000, 24990000, 28000000, 29700000, 50000000, 52000000, 144000000, 146000000 };

                    for (int i = 0; i < band_edge_list_r1h.Length; i++)
                    {
                        double band_edge_offset = band_edge_list_r1h[i] - vfo;
                        if (band_edge_offset >= Low && band_edge_offset <= High)
                        {
                            int temp_vline = (int)((double)(band_edge_offset - Low) / (High - Low) * W);//wa6ahl
                            if (bottom) g.DrawLine(new Pen(band_edge_color), temp_vline, H + top, temp_vline, H + H);//wa6ahl
                            else g.DrawLine(new Pen(band_edge_color), temp_vline, top, temp_vline, H);//wa6ahl
                        }
                    }


                    //---------------------------------------------------------------------------------------------------
                    //---------------------------------------------------------------------------------------------------
                    // 60m edges  

                    int[] band_edge_list_r14h = { 5351500, 5353999, 5354000, 5365999, 5366000, 5366500 };

                    for (int i = 0; i < band_edge_list_r14h.Length; i++)
                    {
                        double band_edge_offset = band_edge_list_r14h[i] - vfo;
                        if (bottom)
                        {
                            if (is_first)
                            {
                                _x = (int)((double)(band_edge_offset - Low) / (High - Low) * W);
                                _y = H + top;
                                is_first = false;
                            }
                            else
                            {
                                _width = ((int)((double)(band_edge_offset - Low) / (High - Low) * W)) - _x;
                                _height = (H + H) - _y;
                                g.DrawRectangle(new Pen(band_box_color, band_box_width), new Rectangle(_x, _y, _width, _height));
                                is_first = true;
                            }
                        }
                        else
                        {
                            if (is_first)
                            {
                                _x = (int)((double)(band_edge_offset - Low) / (High - Low) * W);
                                _y = top;
                                is_first = false;
                            }
                            else
                            {
                                _width = ((int)((double)(band_edge_offset - Low) / (High - Low) * W)) - _x;
                                _height = H - _y;
                                g.DrawRectangle(new Pen(band_box_color, band_box_width), new Rectangle(_x, _y, _width, _height));
                                is_first = true;
                            }
                        }


                    } // 60m 
#if (!NO_KE9NS)
                    // ke9ns add CB 11m band channels 1 - 40
                    for (int i = 0; i < band_edge_list_r77.Length; i++)
                    {
                        double band_edge_offset = band_edge_list_r77[i] - vfo;
                        if (bottom)
                        {
                            if (is_first)
                            {
                                _x = (int)((double)(band_edge_offset - Low) / (High - Low) * W);
                                _y = H + top;
                                is_first = false;
                            }
                            else
                            {
                                _width = ((int)((double)(band_edge_offset - Low) / (High - Low) * W)) - _x;
                                _height = (H + H) - _y;
                                g.DrawRectangle(new Pen(band_box_color, band_box_width), new Rectangle(_x, _y, _width, _height));
                                is_first = true;
                            }
                        }
                        else
                        {
                            if (is_first)
                            {
                                _x = (int)((double)(band_edge_offset - Low) / (High - Low) * W);
                                _y = top;
                                is_first = false;
                            }
                            else
                            {
                                _width = ((int)((double)(band_edge_offset - Low) / (High - Low) * W)) - _x;
                                _height = H - _y;
                                g.DrawRectangle(new Pen(band_box_color, band_box_width), new Rectangle(_x, _y, _width, _height));
                                is_first = true;
                            }
                        }
                    } // for loop for 11m
#endif
                    break; // EU09


                //============================================================================================================
                //============================================================================================================
                //============================================================================================================
                //===============================================
                case FRSRegion.Italy:       // EU10 (no 60m)

                    for (int i = 0; i < f_steps + 1; i++)
                    {
                        string label;
                        int offsetL;
                        int offsetR;

                        int fgrid = i * freq_step_size + (Low / freq_step_size) * freq_step_size;
                        double actual_fgrid = ((double)(vfo_round + fgrid)) / 1000000;
                        int vgrid = (int)((double)(fgrid - vfo_delta - Low) / (High - Low) * W);

                        if (!show_freq_offset)
                        {
                            if (actual_fgrid == 1.83 || actual_fgrid == 1.85 ||
                                actual_fgrid == 3.5 || actual_fgrid == 3.8 ||
                                actual_fgrid == 5.250 || actual_fgrid == 5.45 ||   // ke9ns add
                                actual_fgrid == 7.0 || actual_fgrid == 7.2 ||
                                actual_fgrid == 10.1 || actual_fgrid == 10.15 ||
                                actual_fgrid == 14.0 || actual_fgrid == 14.35 ||
                                actual_fgrid == 18.068 || actual_fgrid == 18.168 ||
                                actual_fgrid == 21.0 || actual_fgrid == 21.45 ||
                                actual_fgrid == 24.89 || actual_fgrid == 24.99 ||
                                actual_fgrid == 28.0 || actual_fgrid == 29.7 ||
                                actual_fgrid == 50.0 || actual_fgrid == 51.0 ||
                                actual_fgrid == 144.0 || actual_fgrid == 148.0)
                            {
                                if (bottom) g.DrawLine(new Pen(band_edge_color), vgrid, H + top, vgrid, H + H);
                                else g.DrawLine(new Pen(band_edge_color), vgrid, top, vgrid, H);

                                label = actual_fgrid.ToString("f4");
                                label = label.Replace(",", ".");    // handle Windows localization issues
                                int offset = label.IndexOf('.') + 4;
                                label = label.Insert(offset, ".");

                                if (actual_fgrid < 10) offsetL = (int)((label.Length + 2) * 4.1) - 14;
                                else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                else offsetL = (int)((label.Length) * 4.1) - 8;

                                if (bottom) g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid - offsetL, H + (float)Math.Floor(H * .01));
                                else g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid - offsetL, (float)Math.Floor(H * .01));

                                int fgrid_2 = ((i + 1) * freq_step_size) + (int)((Low / freq_step_size) * freq_step_size);
                                int x_2 = (int)(((float)(fgrid_2 - vfo_delta - Low) / width * W));
                                float scale = (float)(x_2 - vgrid) / inbetweenies;

                                for (int j = 1; j < inbetweenies; j++)
                                {
                                    if (grid_off == 0) // ke9ns add (dont draw grid lines if =1 
                                    {
                                        float x3 = (float)vgrid + (j * scale);
                                        if (bottom) g.DrawLine(grid_pen_dark, x3, H + top, x3, H + H);
                                        else g.DrawLine(grid_pen_dark, x3, top, x3, H);
                                    }
                                }
                            }
                            else
                            {
                                if (grid_off == 0) // ke9ns add (dont draw grid lines if =1 
                                {
                                    if (bottom) g.DrawLine(grid_pen, vgrid, H + top, vgrid, H + H);
                                    else g.DrawLine(grid_pen, vgrid, top, vgrid, H);            //wa6ahl
                                }
                                int fgrid_2 = ((i + 1) * freq_step_size) + (int)((Low / freq_step_size) * freq_step_size);
                                int x_2 = (int)(((float)(fgrid_2 - vfo_delta - Low) / width * W));
                                float scale = (float)(x_2 - vgrid) / inbetweenies;

                                for (int j = 1; j < inbetweenies; j++)
                                {
                                    if (grid_off == 0) // ke9ns add (dont draw grid lines if =1 
                                    {
                                        float x3 = (float)vgrid + (j * scale);
                                        if (bottom) g.DrawLine(grid_pen_dark, x3, H + top, x3, H + H);
                                        else g.DrawLine(grid_pen_dark, x3, top, x3, H);
                                    }
                                }

                                double actual_fgrid_label = Math.Round(actual_fgrid, 4);
                                label = actual_fgrid_label.ToString("f4");
                                label = label.Replace(",", ".");    // handle Windows localization issues
                                int offset = label.IndexOf('.') + 4;
                                label = label.Insert(offset, ".");

                                if (actual_fgrid < 10) offsetL = (int)((label.Length + 2) * 4.1) - 14;
                                else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                else offsetL = (int)((label.Length) * 4.1) - 8;

                                if (bottom) g.DrawString(label, font, grid_text_brush, vgrid - offsetL, H + (float)Math.Floor(H * .01));
                                else g.DrawString(label, font, grid_text_brush, vgrid - offsetL, (float)Math.Floor(H * .01));
                            }
                        }
                        else
                        {
                            vgrid = Convert.ToInt32((double)-(fgrid - Low) / (Low - High) * W); //wa6ahl
                            if (grid_off == 0) // ke9ns add (dont draw grid lines if =1 
                            {
                                if (bottom) g.DrawLine(grid_pen, vgrid, H + top, vgrid, H + H);
                                else g.DrawLine(grid_pen, vgrid, top, vgrid, H);            //wa6ahl
                            }
                            double new_fgrid = (vfoa_hz + fgrid) / 1000000;

                            label = fgrid.ToString();
                            offsetL = (int)((label.Length + 1) * 4.1);
                            offsetR = (int)(label.Length * 4.1);
                            if ((vgrid - offsetL >= 0) && (vgrid + offsetR < W) && (fgrid != 0))
                            {
                                if (bottom) g.DrawString(label, font, grid_text_brush, vgrid - offsetL, H + (float)Math.Floor(H * .01));
                                else g.DrawString(label, font, grid_text_brush, vgrid - offsetL, (float)Math.Floor(H * .01));
                            }
                        }
                    }

                    // EU10
                    int[] band_edge_list_r9 = {  1830000, 1850000, 3500000, 3800000, 5250000,5450000,
                                                7000000, 7200000, 10100000, 10150000, 14000000, 14350000, 18068000, 18168000, 21000000, 21450000,
                                                24890000, 24990000, 28000000, 29700000, 50000000, 51000000, 144000000, 148000000 };

                    for (int i = 0; i < band_edge_list_r9.Length; i++)
                    {
                        double band_edge_offset = band_edge_list_r9[i] - vfo;
                        if (band_edge_offset >= Low && band_edge_offset <= High)
                        {
                            int temp_vline = (int)((double)(band_edge_offset - Low) / (High - Low) * W);//wa6ahl
                            if (bottom) g.DrawLine(new Pen(band_edge_color), temp_vline, H + top, temp_vline, H + H);//wa6ahl
                            else g.DrawLine(new Pen(band_edge_color), temp_vline, top, temp_vline, H);//wa6ahl
                        }
                    }



                    // draw 60m band segment rectangles - European ( Germany, Belgium, Spain, Switzerland, Finland, Luxembourg)
                    int[] band_edge_list_r7 = { 5351500, 5353999, 5354000, 5365999, 5366000, 5366500 }; // but no TX on 60m

                    for (int i = 0; i < band_edge_list_r7.Length; i++)
                    {
                        double band_edge_offset = band_edge_list_r7[i] - vfo;
                        if (bottom)
                        {
                            if (is_first)
                            {
                                _x = (int)((double)(band_edge_offset - Low) / (High - Low) * W);
                                _y = H + top;
                                is_first = false;
                            }
                            else
                            {
                                _width = ((int)((double)(band_edge_offset - Low) / (High - Low) * W)) - _x;
                                _height = (H + H) - _y;
                                g.DrawRectangle(new Pen(band_box_color, band_box_width), new Rectangle(_x, _y, _width, _height));
                                is_first = true;
                            }
                        }
                        else
                        {
                            if (is_first)
                            {
                                _x = (int)((double)(band_edge_offset - Low) / (High - Low) * W);
                                _y = top;
                                is_first = false;
                            }
                            else
                            {
                                _width = ((int)((double)(band_edge_offset - Low) / (High - Low) * W)) - _x;
                                _height = H - _y;
                                g.DrawRectangle(new Pen(band_box_color, band_box_width), new Rectangle(_x, _y, _width, _height));
                                is_first = true;
                            }
                        }
                    } // for loop for 60m
#if (!NO_KE9NS)
                    // ke9ns add CB 11m band channels 1 - 40
                    for (int i = 0; i < band_edge_list_r77.Length; i++)
                    {
                        double band_edge_offset = band_edge_list_r77[i] - vfo;
                        if (bottom)
                        {
                            if (is_first)
                            {
                                _x = (int)((double)(band_edge_offset - Low) / (High - Low) * W);
                                _y = H + top;
                                is_first = false;
                            }
                            else
                            {
                                _width = ((int)((double)(band_edge_offset - Low) / (High - Low) * W)) - _x;
                                _height = (H + H) - _y;
                                g.DrawRectangle(new Pen(band_box_color, band_box_width), new Rectangle(_x, _y, _width, _height));
                                is_first = true;
                            }
                        }
                        else
                        {
                            if (is_first)
                            {
                                _x = (int)((double)(band_edge_offset - Low) / (High - Low) * W);
                                _y = top;
                                is_first = false;
                            }
                            else
                            {
                                _width = ((int)((double)(band_edge_offset - Low) / (High - Low) * W)) - _x;
                                _height = H - _y;
                                g.DrawRectangle(new Pen(band_box_color, band_box_width), new Rectangle(_x, _y, _width, _height));
                                is_first = true;
                            }
                        }
                    } // for loop for 11m
#endif
                    break; //EU10


                //============================================================================================================
                //============================================================================================================
                //============================================================================================================
                //===============================================
                case FRSRegion.Italy_Plus: // EU11 (no 60m)
                    for (int i = 0; i < f_steps + 1; i++)
                    {
                        string label;
                        int offsetL;
                        int offsetR;

                        int fgrid = i * freq_step_size + (Low / freq_step_size) * freq_step_size;
                        double actual_fgrid = ((double)(vfo_round + fgrid)) / 1000000;
                        int vgrid = (int)((double)(fgrid - vfo_delta - Low) / (High - Low) * W);

                        if (!show_freq_offset)
                        {
                            if (actual_fgrid == 1.83 || actual_fgrid == 1.85 ||
                                actual_fgrid == 3.5 || actual_fgrid == 3.8 ||
                                actual_fgrid == 5.250 || actual_fgrid == 5.45 ||   // ke9ns add
                                actual_fgrid == 6.975 || actual_fgrid == 7.2 ||
                                actual_fgrid == 10.1 || actual_fgrid == 10.15 ||
                                actual_fgrid == 14.0 || actual_fgrid == 14.35 ||
                                actual_fgrid == 18.068 || actual_fgrid == 18.168 ||
                                actual_fgrid == 21.0 || actual_fgrid == 21.45 ||
                                actual_fgrid == 24.89 || actual_fgrid == 24.99 ||
                                actual_fgrid == 28.0 || actual_fgrid == 29.7 ||
                                actual_fgrid == 50.0 || actual_fgrid == 51.0 ||
                                actual_fgrid == 144.0 || actual_fgrid == 148.0)
                            {
                                if (bottom) g.DrawLine(new Pen(band_edge_color), vgrid, H + top, vgrid, H + H);
                                else g.DrawLine(new Pen(band_edge_color), vgrid, top, vgrid, H);

                                label = actual_fgrid.ToString("f4");
                                label = label.Replace(",", ".");    // handle Windows localization issues
                                int offset = label.IndexOf('.') + 4;
                                label = label.Insert(offset, ".");

                                if (actual_fgrid < 10) offsetL = (int)((label.Length + 2) * 4.1) - 14;
                                else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                else offsetL = (int)((label.Length) * 4.1) - 8;

                                if (bottom) g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid - offsetL, H + (float)Math.Floor(H * .01));
                                else g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid - offsetL, (float)Math.Floor(H * .01));

                                int fgrid_2 = ((i + 1) * freq_step_size) + (int)((Low / freq_step_size) * freq_step_size);
                                int x_2 = (int)(((float)(fgrid_2 - vfo_delta - Low) / width * W));
                                float scale = (float)(x_2 - vgrid) / inbetweenies;

                                for (int j = 1; j < inbetweenies; j++)
                                {
                                    if (grid_off == 0) // ke9ns add (dont draw grid lines if =1 
                                    {
                                        float x3 = (float)vgrid + (j * scale);
                                        if (bottom) g.DrawLine(grid_pen_dark, x3, H + top, x3, H + H);
                                        else g.DrawLine(grid_pen_dark, x3, top, x3, H);
                                    }
                                }
                            }
                            else
                            {
                                if (grid_off == 0) // ke9ns add (dont draw grid lines if =1 
                                {
                                    if (bottom) g.DrawLine(grid_pen, vgrid, H + top, vgrid, H + H);
                                    else g.DrawLine(grid_pen, vgrid, top, vgrid, H);            //wa6ahl
                                }
                                int fgrid_2 = ((i + 1) * freq_step_size) + (int)((Low / freq_step_size) * freq_step_size);
                                int x_2 = (int)(((float)(fgrid_2 - vfo_delta - Low) / width * W));
                                float scale = (float)(x_2 - vgrid) / inbetweenies;

                                for (int j = 1; j < inbetweenies; j++)
                                {
                                    if (grid_off == 0) // ke9ns add (dont draw grid lines if =1 
                                    {
                                        float x3 = (float)vgrid + (j * scale);
                                        if (bottom) g.DrawLine(grid_pen_dark, x3, H + top, x3, H + H);
                                        else g.DrawLine(grid_pen_dark, x3, top, x3, H);
                                    }
                                }

                                double actual_fgrid_label = Math.Round(actual_fgrid, 4);
                                label = actual_fgrid_label.ToString("f4");
                                label = label.Replace(",", ".");    // handle Windows localization issues
                                int offset = label.IndexOf('.') + 4;
                                label = label.Insert(offset, ".");

                                if (actual_fgrid < 10) offsetL = (int)((label.Length + 2) * 4.1) - 14;
                                else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                else offsetL = (int)((label.Length) * 4.1) - 8;

                                if (bottom) g.DrawString(label, font, grid_text_brush, vgrid - offsetL, H + (float)Math.Floor(H * .01));
                                else g.DrawString(label, font, grid_text_brush, vgrid - offsetL, (float)Math.Floor(H * .01));
                            }
                        }
                        else
                        {
                            vgrid = Convert.ToInt32((double)-(fgrid - Low) / (Low - High) * W); //wa6ahl
                            if (grid_off == 0) // ke9ns add (dont draw grid lines if =1 
                            {
                                if (bottom) g.DrawLine(grid_pen, vgrid, H + top, vgrid, H + H);
                                else g.DrawLine(grid_pen, vgrid, top, vgrid, H);            //wa6ahl
                            }
                            double new_fgrid = (vfoa_hz + fgrid) / 1000000;

                            label = fgrid.ToString();
                            offsetL = (int)((label.Length + 1) * 4.1);
                            offsetR = (int)(label.Length * 4.1);
                            if ((vgrid - offsetL >= 0) && (vgrid + offsetR < W) && (fgrid != 0))
                            {
                                if (bottom) g.DrawString(label, font, grid_text_brush, vgrid - offsetL, H + (float)Math.Floor(H * .01));
                                else g.DrawString(label, font, grid_text_brush, vgrid - offsetL, (float)Math.Floor(H * .01));
                            }
                        }
                    }

                    // EU11
                    int[] band_edge_list_r9a = {  1830000, 1850000, 3500000, 3800000, 5250000,5450000,
                                                6975000, 7200000, 10100000, 10150000, 14000000, 14350000, 18068000, 18168000, 21000000, 21450000,
                                                24890000, 24990000, 28000000, 29700000, 50000000, 51000000, 144000000, 148000000 };

                    for (int i = 0; i < band_edge_list_r9a.Length; i++)
                    {
                        double band_edge_offset = band_edge_list_r9a[i] - vfo;
                        if (band_edge_offset >= Low && band_edge_offset <= High)
                        {
                            int temp_vline = (int)((double)(band_edge_offset - Low) / (High - Low) * W);//wa6ahl
                            if (bottom) g.DrawLine(new Pen(band_edge_color), temp_vline, H + top, temp_vline, H + H);//wa6ahl
                            else g.DrawLine(new Pen(band_edge_color), temp_vline, top, temp_vline, H);//wa6ahl
                        }
                    }



                    // draw 60m band segment rectangles - European ( Germany, Belgium, Spain, Switzerland, Finland, Luxembourg)
                    int[] band_edge_list_r7a = { 5351500, 5353999, 5354000, 5365999, 5366000, 5366500 }; // but no TX on 60m

                    for (int i = 0; i < band_edge_list_r7a.Length; i++)
                    {
                        double band_edge_offset = band_edge_list_r7a[i] - vfo;
                        if (bottom)
                        {
                            if (is_first)
                            {
                                _x = (int)((double)(band_edge_offset - Low) / (High - Low) * W);
                                _y = H + top;
                                is_first = false;
                            }
                            else
                            {
                                _width = ((int)((double)(band_edge_offset - Low) / (High - Low) * W)) - _x;
                                _height = (H + H) - _y;
                                g.DrawRectangle(new Pen(band_box_color, band_box_width), new Rectangle(_x, _y, _width, _height));
                                is_first = true;
                            }
                        }
                        else
                        {
                            if (is_first)
                            {
                                _x = (int)((double)(band_edge_offset - Low) / (High - Low) * W);
                                _y = top;
                                is_first = false;
                            }
                            else
                            {
                                _width = ((int)((double)(band_edge_offset - Low) / (High - Low) * W)) - _x;
                                _height = H - _y;
                                g.DrawRectangle(new Pen(band_box_color, band_box_width), new Rectangle(_x, _y, _width, _height));
                                is_first = true;
                            }
                        }
                    } // for loop for 60m
#if (!NO_KE9NS)
                    // ke9ns add CB 11m band channels 1 - 40
                    for (int i = 0; i < band_edge_list_r77.Length; i++)
                    {
                        double band_edge_offset = band_edge_list_r77[i] - vfo;
                        if (bottom)
                        {
                            if (is_first)
                            {
                                _x = (int)((double)(band_edge_offset - Low) / (High - Low) * W);
                                _y = H + top;
                                is_first = false;
                            }
                            else
                            {
                                _width = ((int)((double)(band_edge_offset - Low) / (High - Low) * W)) - _x;
                                _height = (H + H) - _y;
                                g.DrawRectangle(new Pen(band_box_color, band_box_width), new Rectangle(_x, _y, _width, _height));
                                is_first = true;
                            }
                        }
                        else
                        {
                            if (is_first)
                            {
                                _x = (int)((double)(band_edge_offset - Low) / (High - Low) * W);
                                _y = top;
                                is_first = false;
                            }
                            else
                            {
                                _width = ((int)((double)(band_edge_offset - Low) / (High - Low) * W)) - _x;
                                _height = H - _y;
                                g.DrawRectangle(new Pen(band_box_color, band_box_width), new Rectangle(_x, _y, _width, _height));
                                is_first = true;
                            }
                        }
                    } // for loop for 11m
#endif
                    break; //EU11



                //============================================================================================================
                //============================================================================================================
                //============================================================================================================
                //===============================================
                case FRSRegion.Netherlands: // EU13 (5.35 to 5.45 60m)

                    for (int i = 0; i < f_steps + 1; i++)
                    {
                        string label;
                        int offsetL;
                        int offsetR;

                        int fgrid = i * freq_step_size + (Low / freq_step_size) * freq_step_size;
                        double actual_fgrid = ((double)(vfo_round + fgrid)) / 1000000;
                        int vgrid = (int)((double)(fgrid - vfo_delta - Low) / (High - Low) * W);

                        if (!show_freq_offset)
                        {
                            if (actual_fgrid == 1.81 || actual_fgrid == 1.88 ||
                                actual_fgrid == 3.5 || actual_fgrid == 3.8 ||
                                actual_fgrid == 5.350 || actual_fgrid == 5.45 ||   // ke9ns add
                                actual_fgrid == 7.0 || actual_fgrid == 7.2 ||
                                actual_fgrid == 10.1 || actual_fgrid == 10.15 ||
                                actual_fgrid == 14.0 || actual_fgrid == 14.35 ||
                                actual_fgrid == 18.068 || actual_fgrid == 18.168 ||
                                actual_fgrid == 21.0 || actual_fgrid == 21.45 ||
                                actual_fgrid == 24.89 || actual_fgrid == 24.99 ||
                                actual_fgrid == 28.0 || actual_fgrid == 29.7 ||
                                actual_fgrid == 50.0 || actual_fgrid == 52.0 ||
                                actual_fgrid == 144.0 || actual_fgrid == 146.0)
                            {
                                if (bottom) g.DrawLine(new Pen(band_edge_color), vgrid, H + top, vgrid, H + H);
                                else g.DrawLine(new Pen(band_edge_color), vgrid, top, vgrid, H);

                                label = actual_fgrid.ToString("f4");
                                label = label.Replace(",", ".");    // handle Windows localization issues
                                int offset = label.IndexOf('.') + 4;
                                label = label.Insert(offset, ".");

                                if (actual_fgrid < 10) offsetL = (int)((label.Length + 2) * 4.1) - 14;
                                else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                else offsetL = (int)((label.Length) * 4.1) - 8;

                                if (bottom) g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid - offsetL, H + (float)Math.Floor(H * .01));
                                else g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid - offsetL, (float)Math.Floor(H * .01));

                                int fgrid_2 = ((i + 1) * freq_step_size) + (int)((Low / freq_step_size) * freq_step_size);
                                int x_2 = (int)(((float)(fgrid_2 - vfo_delta - Low) / width * W));
                                float scale = (float)(x_2 - vgrid) / inbetweenies;

                                for (int j = 1; j < inbetweenies; j++)
                                {
                                    if (grid_off == 0) // ke9ns add (dont draw grid lines if =1 
                                    {
                                        float x3 = (float)vgrid + (j * scale);
                                        if (bottom) g.DrawLine(grid_pen_dark, x3, H + top, x3, H + H);
                                        else g.DrawLine(grid_pen_dark, x3, top, x3, H);
                                    }
                                }
                            }
                            else
                            {
                                if (grid_off == 0) // ke9ns add (dont draw grid lines if =1 
                                {
                                    if (bottom) g.DrawLine(grid_pen, vgrid, H + top, vgrid, H + H);
                                    else g.DrawLine(grid_pen, vgrid, top, vgrid, H);            //wa6ahl
                                }

                                int fgrid_2 = ((i + 1) * freq_step_size) + (int)((Low / freq_step_size) * freq_step_size);
                                int x_2 = (int)(((float)(fgrid_2 - vfo_delta - Low) / width * W));
                                float scale = (float)(x_2 - vgrid) / inbetweenies;

                                for (int j = 1; j < inbetweenies; j++)
                                {
                                    if (grid_off == 0) // ke9ns add (dont draw grid lines if =1 
                                    {
                                        float x3 = (float)vgrid + (j * scale);
                                        if (bottom) g.DrawLine(grid_pen_dark, x3, H + top, x3, H + H);
                                        else g.DrawLine(grid_pen_dark, x3, top, x3, H);
                                    }
                                }

                                double actual_fgrid_label = Math.Round(actual_fgrid, 4);
                                label = actual_fgrid_label.ToString("f4");
                                label = label.Replace(",", ".");    // handle Windows localization issues
                                int offset = label.IndexOf('.') + 4;
                                label = label.Insert(offset, ".");

                                if (actual_fgrid < 10) offsetL = (int)((label.Length + 2) * 4.1) - 14;
                                else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                else offsetL = (int)((label.Length) * 4.1) - 8;

                                if (bottom) g.DrawString(label, font, grid_text_brush, vgrid - offsetL, H + (float)Math.Floor(H * .01));
                                else g.DrawString(label, font, grid_text_brush, vgrid - offsetL, (float)Math.Floor(H * .01));
                            }
                        }
                        else
                        {
                            vgrid = Convert.ToInt32((double)-(fgrid - Low) / (Low - High) * W); //wa6ahl
                            if (grid_off == 0) // ke9ns add (dont draw grid lines if =1 
                            {
                                if (bottom) g.DrawLine(grid_pen, vgrid, H + top, vgrid, H + H);
                                else g.DrawLine(grid_pen, vgrid, top, vgrid, H);            //wa6ahl
                            }
                            double new_fgrid = (vfoa_hz + fgrid) / 1000000;

                            label = fgrid.ToString();
                            offsetL = (int)((label.Length + 1) * 4.1);
                            offsetR = (int)(label.Length * 4.1);
                            if ((vgrid - offsetL >= 0) && (vgrid + offsetR < W) && (fgrid != 0))
                            {
                                if (bottom) g.DrawString(label, font, grid_text_brush, vgrid - offsetL, H + (float)Math.Floor(H * .01));
                                else g.DrawString(label, font, grid_text_brush, vgrid - offsetL, (float)Math.Floor(H * .01));
                            }
                        }
                    }

                    //--------------------------------------------------------------------------------------------------- EU13
                    //---------------------------------------------------------------------------------------------------
                    int[] band_edge_list_r1j = { 1810000, 1880000,  3500000, 3800000, 5350000, 5450000,
                                                7000000, 7200000, 10100000, 10150000, 14000000, 14350000, 18068000, 18168000, 21000000, 21450000,
                                                24890000, 24990000, 28000000, 29700000, 50000000, 52000000, 144000000, 146000000 };

                    for (int i = 0; i < band_edge_list_r1j.Length; i++)
                    {
                        double band_edge_offset = band_edge_list_r1j[i] - vfo;
                        if (band_edge_offset >= Low && band_edge_offset <= High)
                        {
                            int temp_vline = (int)((double)(band_edge_offset - Low) / (High - Low) * W);//wa6ahl
                            if (bottom) g.DrawLine(new Pen(band_edge_color), temp_vline, H + top, temp_vline, H + H);//wa6ahl
                            else g.DrawLine(new Pen(band_edge_color), temp_vline, top, temp_vline, H);//wa6ahl
                        }
                    }


 #if (!NO_KE9NS)
                    // ke9ns add CB 11m band channels 1 - 40
                    for (int i = 0; i < band_edge_list_r77.Length; i++)
                    {
                        double band_edge_offset = band_edge_list_r77[i] - vfo;
                        if (bottom)
                        {
                            if (is_first)
                            {
                                _x = (int)((double)(band_edge_offset - Low) / (High - Low) * W);
                                _y = H + top;
                                is_first = false;
                            }
                            else
                            {
                                _width = ((int)((double)(band_edge_offset - Low) / (High - Low) * W)) - _x;
                                _height = (H + H) - _y;
                                g.DrawRectangle(new Pen(band_box_color, band_box_width), new Rectangle(_x, _y, _width, _height));
                                is_first = true;
                            }
                        }
                        else
                        {
                            if (is_first)
                            {
                                _x = (int)((double)(band_edge_offset - Low) / (High - Low) * W);
                                _y = top;
                                is_first = false;
                            }
                            else
                            {
                                _width = ((int)((double)(band_edge_offset - Low) / (High - Low) * W)) - _x;
                                _height = H - _y;
                                g.DrawRectangle(new Pen(band_box_color, band_box_width), new Rectangle(_x, _y, _width, _height));
                                is_first = true;
                            }
                        }
                    } // for loop for 11m
#endif
                    break; // EU13



                //============================================================================================================
                //============================================================================================================
                //============================================================================================================
                //===============================================
                case FRSRegion.Luxembourg:  // EU15 (IARU1 60m) & 52mhz for 6m)
              
                    for (int i = 0; i < f_steps + 1; i++)
                    {
                        string label;
                        int offsetL;
                        int offsetR;

                        int fgrid = i * freq_step_size + (Low / freq_step_size) * freq_step_size;
                        double actual_fgrid = ((double)(vfo_round + fgrid)) / 1000000;
                        int vgrid = (int)((double)(fgrid - vfo_delta - Low) / (High - Low) * W);

                        if (!show_freq_offset)
                        {
                            if (actual_fgrid == 1.81 || actual_fgrid == 2.0 ||
                                actual_fgrid == 3.5 || actual_fgrid == 3.8 ||
                                actual_fgrid == 5.250 || actual_fgrid == 5.45 ||   // ke9ns add
                                actual_fgrid == 7.0 || actual_fgrid == 7.2 ||
                                actual_fgrid == 10.1 || actual_fgrid == 10.15 ||
                                actual_fgrid == 14.0 || actual_fgrid == 14.35 ||
                                actual_fgrid == 18.068 || actual_fgrid == 18.168 ||
                                actual_fgrid == 21.0 || actual_fgrid == 21.45 ||
                                actual_fgrid == 24.89 || actual_fgrid == 24.99 ||
                                actual_fgrid == 28.0 || actual_fgrid == 29.7 ||
                                actual_fgrid == 50.0 || actual_fgrid == 52.0 ||
                                actual_fgrid == 144.0 || actual_fgrid == 146.0)
                            {
                                if (bottom) g.DrawLine(new Pen(band_edge_color), vgrid, H + top, vgrid, H + H);
                                else g.DrawLine(new Pen(band_edge_color), vgrid, top, vgrid, H);

                                label = actual_fgrid.ToString("f4");
                                label = label.Replace(",", ".");    // handle Windows localization issues
                                int offset = label.IndexOf('.') + 4;
                                label = label.Insert(offset, ".");

                                if (actual_fgrid < 10) offsetL = (int)((label.Length + 2) * 4.1) - 14;
                                else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                else offsetL = (int)((label.Length) * 4.1) - 8;

                                if (bottom) g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid - offsetL, H + (float)Math.Floor(H * .01));
                                else g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid - offsetL, (float)Math.Floor(H * .01));

                                int fgrid_2 = ((i + 1) * freq_step_size) + (int)((Low / freq_step_size) * freq_step_size);
                                int x_2 = (int)(((float)(fgrid_2 - vfo_delta - Low) / width * W));
                                float scale = (float)(x_2 - vgrid) / inbetweenies;

                                for (int j = 1; j < inbetweenies; j++)
                                {
                                    if (grid_off == 0) // ke9ns add (dont draw grid lines if =1 
                                    {
                                        float x3 = (float)vgrid + (j * scale);
                                        if (bottom) g.DrawLine(grid_pen_dark, x3, H + top, x3, H + H);
                                        else g.DrawLine(grid_pen_dark, x3, top, x3, H);
                                    }
                                }
                            }
                            else
                            {
                                if (grid_off == 0) // ke9ns add (dont draw grid lines if =1 
                                {
                                    if (bottom) g.DrawLine(grid_pen, vgrid, H + top, vgrid, H + H);
                                    else g.DrawLine(grid_pen, vgrid, top, vgrid, H);            //wa6ahl
                                }

                                int fgrid_2 = ((i + 1) * freq_step_size) + (int)((Low / freq_step_size) * freq_step_size);
                                int x_2 = (int)(((float)(fgrid_2 - vfo_delta - Low) / width * W));
                                float scale = (float)(x_2 - vgrid) / inbetweenies;

                                for (int j = 1; j < inbetweenies; j++)
                                {
                                    if (grid_off == 0) // ke9ns add (dont draw grid lines if =1 
                                    {
                                        float x3 = (float)vgrid + (j * scale);
                                        if (bottom) g.DrawLine(grid_pen_dark, x3, H + top, x3, H + H);
                                        else g.DrawLine(grid_pen_dark, x3, top, x3, H);
                                    }
                                }

                                double actual_fgrid_label = Math.Round(actual_fgrid, 4);
                                label = actual_fgrid_label.ToString("f4");
                                label = label.Replace(",", ".");    // handle Windows localization issues
                                int offset = label.IndexOf('.') + 4;
                                label = label.Insert(offset, ".");

                                if (actual_fgrid < 10) offsetL = (int)((label.Length + 2) * 4.1) - 14;
                                else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                else offsetL = (int)((label.Length) * 4.1) - 8;

                                if (bottom) g.DrawString(label, font, grid_text_brush, vgrid - offsetL, H + (float)Math.Floor(H * .01));
                                else g.DrawString(label, font, grid_text_brush, vgrid - offsetL, (float)Math.Floor(H * .01));
                            }
                        }
                        else
                        {
                            vgrid = Convert.ToInt32((double)-(fgrid - Low) / (Low - High) * W); //wa6ahl
                            if (grid_off == 0) // ke9ns add (dont draw grid lines if =1 
                            {
                                if (bottom) g.DrawLine(grid_pen, vgrid, H + top, vgrid, H + H);
                                else g.DrawLine(grid_pen, vgrid, top, vgrid, H);            //wa6ahl
                            }
                            double new_fgrid = (vfoa_hz + fgrid) / 1000000;

                            label = fgrid.ToString();
                            offsetL = (int)((label.Length + 1) * 4.1);
                            offsetR = (int)(label.Length * 4.1);
                            if ((vgrid - offsetL >= 0) && (vgrid + offsetR < W) && (fgrid != 0))
                            {
                                if (bottom) g.DrawString(label, font, grid_text_brush, vgrid - offsetL, H + (float)Math.Floor(H * .01));
                                else g.DrawString(label, font, grid_text_brush, vgrid - offsetL, (float)Math.Floor(H * .01));
                            }
                        }
                    }

                    //---------------------------------------------------------------------------------------------------
                    //---------------------------------------------------------------------------------------------------
                    int[] band_edge_list_r1k = { 1810000, 2000000, 3500000, 3800000, 5250000, 5450000,
                                                7000000, 7200000, 10100000, 10150000, 14000000, 14350000, 18068000, 18168000, 21000000, 21450000,
                                                24890000, 24990000, 28000000, 29700000,  50000000, 52000000, 144000000, 146000000 };

                    for (int i = 0; i < band_edge_list_r1k.Length; i++)
                    {
                        double band_edge_offset = band_edge_list_r1k[i] - vfo;
                        if (band_edge_offset >= Low && band_edge_offset <= High)
                        {
                            int temp_vline = (int)((double)(band_edge_offset - Low) / (High - Low) * W);//wa6ahl
                            if (bottom) g.DrawLine(new Pen(band_edge_color), temp_vline, H + top, temp_vline, H + H);//wa6ahl
                            else g.DrawLine(new Pen(band_edge_color), temp_vline, top, temp_vline, H);//wa6ahl
                        }
                    }

                  
                   
                    //---------------------------------------------------------------------------------------------------EU15
                    //---------------------------------------------------------------------------------------------------
                    
                        int[] band_edge_list_r14l = { 5351500, 5353999, 5354000, 5365999, 5366000, 5366500 };

                        for (int i = 0; i < band_edge_list_r14l.Length; i++)
                        {
                            double band_edge_offset = band_edge_list_r14l[i] - vfo;
                            if (bottom)
                            {
                                if (is_first)
                                {
                                    _x = (int)((double)(band_edge_offset - Low) / (High - Low) * W);
                                    _y = H + top;
                                    is_first = false;
                                }
                                else
                                {
                                    _width = ((int)((double)(band_edge_offset - Low) / (High - Low) * W)) - _x;
                                    _height = (H + H) - _y;
                                    g.DrawRectangle(new Pen(band_box_color, band_box_width), new Rectangle(_x, _y, _width, _height));
                                    is_first = true;
                                }
                            }
                            else
                            {
                                if (is_first)
                                {
                                    _x = (int)((double)(band_edge_offset - Low) / (High - Low) * W);
                                    _y = top;
                                    is_first = false;
                                }
                                else
                                {
                                    _width = ((int)((double)(band_edge_offset - Low) / (High - Low) * W)) - _x;
                                    _height = H - _y;
                                    g.DrawRectangle(new Pen(band_box_color, band_box_width), new Rectangle(_x, _y, _width, _height));
                                    is_first = true;
                                }
                            }
                        } // for loop for 60m

#if (!NO_KE9NS)
                    // ke9ns add CB 11m band channels 1 - 40
                    for (int i = 0; i < band_edge_list_r77.Length; i++)
                    {
                        double band_edge_offset = band_edge_list_r77[i] - vfo;
                        if (bottom)
                        {
                            if (is_first)
                            {
                                _x = (int)((double)(band_edge_offset - Low) / (High - Low) * W);
                                _y = H + top;
                                is_first = false;
                            }
                            else
                            {
                                _width = ((int)((double)(band_edge_offset - Low) / (High - Low) * W)) - _x;
                                _height = (H + H) - _y;
                                g.DrawRectangle(new Pen(band_box_color, band_box_width), new Rectangle(_x, _y, _width, _height));
                                is_first = true;
                            }
                        }
                        else
                        {
                            if (is_first)
                            {
                                _x = (int)((double)(band_edge_offset - Low) / (High - Low) * W);
                                _y = top;
                                is_first = false;
                            }
                            else
                            {
                                _width = ((int)((double)(band_edge_offset - Low) / (High - Low) * W)) - _x;
                                _height = H - _y;
                                g.DrawRectangle(new Pen(band_box_color, band_box_width), new Rectangle(_x, _y, _width, _height));
                                is_first = true;
                            }
                        }
                    } // for loop for 11m
#endif
                    break; //EU15



                //============================================================================================================
                //============================================================================================================
                //============================================================================================================
                //===============================================
                case FRSRegion.Region_2:  // IARU2

                    for (int i = 0; i < f_steps + 1; i++)
                    {
                        string label;
                        int offsetL;
                        int offsetR;

                        int fgrid = i * freq_step_size + (Low / freq_step_size) * freq_step_size;
                        double actual_fgrid = ((double)(vfo_round + fgrid)) / 1000000;
                        int vgrid = (int)((double)(fgrid - vfo_delta - Low) / (High - Low) * W);

                        if (!show_freq_offset)
                        {
                            if (actual_fgrid == 1.81 || actual_fgrid == 2.0 ||
                                actual_fgrid == 3.5 || actual_fgrid == 3.4 ||
                                actual_fgrid == 5.250 || actual_fgrid == 5.45 ||   // ke9ns add
                                actual_fgrid == 7.0 || actual_fgrid == 7.3 ||
                                actual_fgrid == 10.1 || actual_fgrid == 10.15 ||
                                actual_fgrid == 14.0 || actual_fgrid == 14.35 ||
                                actual_fgrid == 18.068 || actual_fgrid == 18.168 ||
                                actual_fgrid == 21.0 || actual_fgrid == 21.45 ||
                                actual_fgrid == 24.89 || actual_fgrid == 24.99 ||
                                actual_fgrid == 28.0 || actual_fgrid == 29.7 ||
                                actual_fgrid == 50.0 || actual_fgrid == 54.0 ||
                                actual_fgrid == 144.0 || actual_fgrid == 146.0)
                            {
                                if (bottom) g.DrawLine(new Pen(band_edge_color), vgrid, H + top, vgrid, H + H);
                                else g.DrawLine(new Pen(band_edge_color), vgrid, top, vgrid, H);

                                label = actual_fgrid.ToString("f4");
                                label = label.Replace(",", ".");    // handle Windows localization issues
                                int offset = label.IndexOf('.') + 4;
                                label = label.Insert(offset, ".");

                                if (actual_fgrid < 10) offsetL = (int)((label.Length + 2) * 4.1) - 14;
                                else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                else offsetL = (int)((label.Length) * 4.1) - 8;

                                if (bottom) g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid - offsetL, H + (float)Math.Floor(H * .01));
                                else g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid - offsetL, (float)Math.Floor(H * .01));

                                int fgrid_2 = ((i + 1) * freq_step_size) + (int)((Low / freq_step_size) * freq_step_size);
                                int x_2 = (int)(((float)(fgrid_2 - vfo_delta - Low) / width * W));
                                float scale = (float)(x_2 - vgrid) / inbetweenies;

                                for (int j = 1; j < inbetweenies; j++)
                                {
                                    if (grid_off == 0) // ke9ns add (dont draw grid lines if =1 
                                    {
                                        float x3 = (float)vgrid + (j * scale);
                                        if (bottom) g.DrawLine(grid_pen_dark, x3, H + top, x3, H + H);
                                        else g.DrawLine(grid_pen_dark, x3, top, x3, H);
                                    }
                                }
                            }
                            else
                            {
                                if (grid_off == 0) // ke9ns add (dont draw grid lines if =1 
                                {
                                    if (bottom) g.DrawLine(grid_pen, vgrid, H + top, vgrid, H + H);
                                    else g.DrawLine(grid_pen, vgrid, top, vgrid, H);            //wa6ahl
                                }

                                int fgrid_2 = ((i + 1) * freq_step_size) + (int)((Low / freq_step_size) * freq_step_size);
                                int x_2 = (int)(((float)(fgrid_2 - vfo_delta - Low) / width * W));
                                float scale = (float)(x_2 - vgrid) / inbetweenies;

                                for (int j = 1; j < inbetweenies; j++)
                                {
                                    if (grid_off == 0) // ke9ns add (dont draw grid lines if =1 
                                    {
                                        float x3 = (float)vgrid + (j * scale);
                                        if (bottom) g.DrawLine(grid_pen_dark, x3, H + top, x3, H + H);
                                        else g.DrawLine(grid_pen_dark, x3, top, x3, H);
                                    }
                                }

                                double actual_fgrid_label = Math.Round(actual_fgrid, 4);
                                label = actual_fgrid_label.ToString("f4");
                                label = label.Replace(",", ".");    // handle Windows localization issues
                                int offset = label.IndexOf('.') + 4;
                                label = label.Insert(offset, ".");

                                if (actual_fgrid < 10) offsetL = (int)((label.Length + 2) * 4.1) - 14;
                                else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                else offsetL = (int)((label.Length) * 4.1) - 8;

                                if (bottom) g.DrawString(label, font, grid_text_brush, vgrid - offsetL, H + (float)Math.Floor(H * .01));
                                else g.DrawString(label, font, grid_text_brush, vgrid - offsetL, (float)Math.Floor(H * .01));
                            }
                        }
                        else
                        {
                            vgrid = Convert.ToInt32((double)-(fgrid - Low) / (Low - High) * W); //wa6ahl
                            if (grid_off == 0) // ke9ns add (dont draw grid lines if =1 
                            {
                                if (bottom) g.DrawLine(grid_pen, vgrid, H + top, vgrid, H + H);
                                else g.DrawLine(grid_pen, vgrid, top, vgrid, H);            //wa6ahl
                            }
                            double new_fgrid = (vfoa_hz + fgrid) / 1000000;

                            label = fgrid.ToString();
                            offsetL = (int)((label.Length + 1) * 4.1);
                            offsetR = (int)(label.Length * 4.1);
                            if ((vgrid - offsetL >= 0) && (vgrid + offsetR < W) && (fgrid != 0))
                            {
                                if (bottom) g.DrawString(label, font, grid_text_brush, vgrid - offsetL, H + (float)Math.Floor(H * .01));
                                else g.DrawString(label, font, grid_text_brush, vgrid - offsetL, (float)Math.Floor(H * .01));
                            }
                        }
                    }

                    //---------------------------------------------------------------------------------------------------IARU2
                    //---------------------------------------------------------------------------------------------------
                    int[] band_edge_list_r1r = { 1810000, 2000000, 3500000, 3400000, 5250000, 5450000,
                                                7000000, 7300000, 10100000, 10150000, 14000000, 14350000, 18068000, 18168000, 21000000, 21450000,
                                                24890000, 24990000, 28000000, 29700000,  50000000, 54000000, 144000000, 146000000 };

                    for (int i = 0; i < band_edge_list_r1r.Length; i++)
                    {
                        double band_edge_offset = band_edge_list_r1r[i] - vfo;
                        if (band_edge_offset >= Low && band_edge_offset <= High)
                        {
                            int temp_vline = (int)((double)(band_edge_offset - Low) / (High - Low) * W);//wa6ahl
                            if (bottom) g.DrawLine(new Pen(band_edge_color), temp_vline, H + top, temp_vline, H + H);//wa6ahl
                            else g.DrawLine(new Pen(band_edge_color), temp_vline, top, temp_vline, H);//wa6ahl
                        }
                    }



                    //---------------------------------------------------------------------------------------------------EU15
                    //---------------------------------------------------------------------------------------------------

                    int[] band_edge_list_r14r = { 5351500, 5353999, 5354000, 5365999, 5366000, 5366500 };

                    for (int i = 0; i < band_edge_list_r14r.Length; i++)
                    {
                        double band_edge_offset = band_edge_list_r14r[i] - vfo;
                        if (bottom)
                        {
                            if (is_first)
                            {
                                _x = (int)((double)(band_edge_offset - Low) / (High - Low) * W);
                                _y = H + top;
                                is_first = false;
                            }
                            else
                            {
                                _width = ((int)((double)(band_edge_offset - Low) / (High - Low) * W)) - _x;
                                _height = (H + H) - _y;
                                g.DrawRectangle(new Pen(band_box_color, band_box_width), new Rectangle(_x, _y, _width, _height));
                                is_first = true;
                            }
                        }
                        else
                        {
                            if (is_first)
                            {
                                _x = (int)((double)(band_edge_offset - Low) / (High - Low) * W);
                                _y = top;
                                is_first = false;
                            }
                            else
                            {
                                _width = ((int)((double)(band_edge_offset - Low) / (High - Low) * W)) - _x;
                                _height = H - _y;
                                g.DrawRectangle(new Pen(band_box_color, band_box_width), new Rectangle(_x, _y, _width, _height));
                                is_first = true;
                            }
                        }
                    } // for loop for 60m

#if (!NO_KE9NS)
                    // ke9ns add CB 11m band channels 1 - 40
                    for (int i = 0; i < band_edge_list_r77.Length; i++)
                    {
                        double band_edge_offset = band_edge_list_r77[i] - vfo;
                        if (bottom)
                        {
                            if (is_first)
                            {
                                _x = (int)((double)(band_edge_offset - Low) / (High - Low) * W);
                                _y = H + top;
                                is_first = false;
                            }
                            else
                            {
                                _width = ((int)((double)(band_edge_offset - Low) / (High - Low) * W)) - _x;
                                _height = (H + H) - _y;
                                g.DrawRectangle(new Pen(band_box_color, band_box_width), new Rectangle(_x, _y, _width, _height));
                                is_first = true;
                            }
                        }
                        else
                        {
                            if (is_first)
                            {
                                _x = (int)((double)(band_edge_offset - Low) / (High - Low) * W);
                                _y = top;
                                is_first = false;
                            }
                            else
                            {
                                _width = ((int)((double)(band_edge_offset - Low) / (High - Low) * W)) - _x;
                                _height = H - _y;
                                g.DrawRectangle(new Pen(band_box_color, band_box_width), new Rectangle(_x, _y, _width, _height));
                                is_first = true;
                            }
                        }
                    } // for loop for 11m
#endif
                    break; // IARU 2


             

                //============================================================================================================
                //============================================================================================================
                //============================================================================================================
                //===============================================
                case FRSRegion.Russia:      // RUSS (no 60m)

                    for (int i = 0; i < f_steps + 1; i++)
                    {
                        string label;
                        int offsetL;
                        int offsetR;

                        int fgrid = i * freq_step_size + (Low / freq_step_size) * freq_step_size;
                        double actual_fgrid = ((double)(vfo_round + fgrid)) / 1000000;
                        int vgrid = (int)((double)(fgrid - vfo_delta - Low) / (High - Low) * W);

                        if (!show_freq_offset)
                        {
                            if (actual_fgrid == 1.81 || actual_fgrid == 2.0 ||
                                actual_fgrid == 3.5 || actual_fgrid == 3.8 ||
                                actual_fgrid == 5.250 || actual_fgrid == 5.45 ||   // ke9ns add
                                actual_fgrid == 7.0 || actual_fgrid == 7.2 ||
                                actual_fgrid == 10.1 || actual_fgrid == 10.15 ||
                                actual_fgrid == 14.0 || actual_fgrid == 14.35 ||
                                actual_fgrid == 18.068 || actual_fgrid == 18.168 ||
                                actual_fgrid == 21.0 || actual_fgrid == 21.45 ||
                                actual_fgrid == 24.89 || actual_fgrid == 25.14 ||
                                actual_fgrid == 26.97 || actual_fgrid == 27.86 ||
                                actual_fgrid == 28.0 || actual_fgrid == 29.7 ||
                                actual_fgrid == 50.0 || actual_fgrid == 52.0 ||
                                actual_fgrid == 144.0 || actual_fgrid == 146.0)
                            {
                                if (bottom) g.DrawLine(new Pen(band_edge_color), vgrid, H + top, vgrid, H + H);
                                else g.DrawLine(new Pen(band_edge_color), vgrid, top, vgrid, H);

                                label = actual_fgrid.ToString("f4");
                                label = label.Replace(",", ".");    // handle Windows localization issues
                                int offset = label.IndexOf('.') + 4;
                                label = label.Insert(offset, ".");

                                if (actual_fgrid < 10) offsetL = (int)((label.Length + 2) * 4.1) - 14;
                                else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                else offsetL = (int)((label.Length) * 4.1) - 8;

                                if (bottom) g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid - offsetL, H + (float)Math.Floor(H * .01));
                                else g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid - offsetL, (float)Math.Floor(H * .01));

                                int fgrid_2 = ((i + 1) * freq_step_size) + (int)((Low / freq_step_size) * freq_step_size);
                                int x_2 = (int)(((float)(fgrid_2 - vfo_delta - Low) / width * W));
                                float scale = (float)(x_2 - vgrid) / inbetweenies;

                                for (int j = 1; j < inbetweenies; j++)
                                {
                                    if (grid_off == 0) // ke9ns add (dont draw grid lines if =1 
                                    {
                                        float x3 = (float)vgrid + (j * scale);
                                        if (bottom) g.DrawLine(grid_pen_dark, x3, H + top, x3, H + H);
                                        else g.DrawLine(grid_pen_dark, x3, top, x3, H);
                                    }
                                }
                            }
                            else
                            {
                                if (grid_off == 0) // ke9ns add (dont draw grid lines if =1 
                                {
                                    if (bottom) g.DrawLine(grid_pen, vgrid, H + top, vgrid, H + H);
                                    else g.DrawLine(grid_pen, vgrid, top, vgrid, H);            //wa6ahl
                                }

                                int fgrid_2 = ((i + 1) * freq_step_size) + (int)((Low / freq_step_size) * freq_step_size);
                                int x_2 = (int)(((float)(fgrid_2 - vfo_delta - Low) / width * W));
                                float scale = (float)(x_2 - vgrid) / inbetweenies;

                                for (int j = 1; j < inbetweenies; j++)
                                {
                                    if (grid_off == 0) // ke9ns add (dont draw grid lines if =1 
                                    {
                                        float x3 = (float)vgrid + (j * scale);
                                        if (bottom) g.DrawLine(grid_pen_dark, x3, H + top, x3, H + H);
                                        else g.DrawLine(grid_pen_dark, x3, top, x3, H);
                                    }
                                }

                                double actual_fgrid_label = Math.Round(actual_fgrid, 4);
                                label = actual_fgrid_label.ToString("f4");
                                label = label.Replace(",", ".");    // handle Windows localization issues
                                int offset = label.IndexOf('.') + 4;
                                label = label.Insert(offset, ".");

                                if (actual_fgrid < 10) offsetL = (int)((label.Length + 2) * 4.1) - 14;
                                else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                else offsetL = (int)((label.Length) * 4.1) - 8;

                                if (bottom) g.DrawString(label, font, grid_text_brush, vgrid - offsetL, H + (float)Math.Floor(H * .01));
                                else g.DrawString(label, font, grid_text_brush, vgrid - offsetL, (float)Math.Floor(H * .01));
                            }
                        }
                        else
                        {
                            vgrid = Convert.ToInt32((double)-(fgrid - Low) / (Low - High) * W); //wa6ahl
                            if (grid_off == 0) // ke9ns add (dont draw grid lines if =1 
                            {
                                if (bottom) g.DrawLine(grid_pen, vgrid, H + top, vgrid, H + H);
                                else g.DrawLine(grid_pen, vgrid, top, vgrid, H);            //wa6ahl
                            }
                            double new_fgrid = (vfoa_hz + fgrid) / 1000000;

                            label = fgrid.ToString();
                            offsetL = (int)((label.Length + 1) * 4.1);
                            offsetR = (int)(label.Length * 4.1);
                            if ((vgrid - offsetL >= 0) && (vgrid + offsetR < W) && (fgrid != 0))
                            {
                                if (bottom) g.DrawString(label, font, grid_text_brush, vgrid - offsetL, H + (float)Math.Floor(H * .01));
                                else g.DrawString(label, font, grid_text_brush, vgrid - offsetL, (float)Math.Floor(H * .01));
                            }
                        }
                    }

                    //---------------------------------------------------------------------------------------------------
                    //---------------------------------------------------------------------------------------------------
                    int[] band_edge_list_r1q = { 1810000, 2000000,  3500000, 3800000, 5250000, 5450000,
                                                7000000, 7200000, 10100000, 10150000, 14000000, 14350000, 18068000, 18168000, 21000000, 21450000,
                                                24890000, 25140000, 26970000, 27860000, 28000000, 29700000, 50000000, 54000000, 144000000, 146000000 };

                    for (int i = 0; i < band_edge_list_r1q.Length; i++)
                    {
                        double band_edge_offset = band_edge_list_r1q[i] - vfo;
                        if (band_edge_offset >= Low && band_edge_offset <= High)
                        {
                            int temp_vline = (int)((double)(band_edge_offset - Low) / (High - Low) * W);//wa6ahl
                            if (bottom) g.DrawLine(new Pen(band_edge_color), temp_vline, H + top, temp_vline, H + H);//wa6ahl
                            else g.DrawLine(new Pen(band_edge_color), temp_vline, top, temp_vline, H);//wa6ahl
                        }
                    }


                    //---------------------------------------------------------------------------------------------------
                    //---------------------------------------------------------------------------------------------------
                    // 60m edges  

                    int[] band_edge_list_r14q = { 5351500, 5353999, 5354000, 5365999, 5366000, 5366500 };

                    for (int i = 0; i < band_edge_list_r14q.Length; i++)
                    {
                        double band_edge_offset = band_edge_list_r14q[i] - vfo;
                        if (bottom)
                        {
                            if (is_first)
                            {
                                _x = (int)((double)(band_edge_offset - Low) / (High - Low) * W);
                                _y = H + top;
                                is_first = false;
                            }
                            else
                            {
                                _width = ((int)((double)(band_edge_offset - Low) / (High - Low) * W)) - _x;
                                _height = (H + H) - _y;
                                g.DrawRectangle(new Pen(band_box_color, band_box_width), new Rectangle(_x, _y, _width, _height));
                                is_first = true;
                            }
                        }
                        else
                        {
                            if (is_first)
                            {
                                _x = (int)((double)(band_edge_offset - Low) / (High - Low) * W);
                                _y = top;
                                is_first = false;
                            }
                            else
                            {
                                _width = ((int)((double)(band_edge_offset - Low) / (High - Low) * W)) - _x;
                                _height = H - _y;
                                g.DrawRectangle(new Pen(band_box_color, band_box_width), new Rectangle(_x, _y, _width, _height));
                                is_first = true;
                            }
                        }


                    } // 60m uk 
#if (!NO_KE9NS)
                    // ke9ns add CB 11m band channels 1 - 40
                    for (int i = 0; i < band_edge_list_r77.Length; i++)
                    {
                        double band_edge_offset = band_edge_list_r77[i] - vfo;
                        if (bottom)
                        {
                            if (is_first)
                            {
                                _x = (int)((double)(band_edge_offset - Low) / (High - Low) * W);
                                _y = H + top;
                                is_first = false;
                            }
                            else
                            {
                                _width = ((int)((double)(band_edge_offset - Low) / (High - Low) * W)) - _x;
                                _height = (H + H) - _y;
                                g.DrawRectangle(new Pen(band_box_color, band_box_width), new Rectangle(_x, _y, _width, _height));
                                is_first = true;
                            }
                        }
                        else
                        {
                            if (is_first)
                            {
                                _x = (int)((double)(band_edge_offset - Low) / (High - Low) * W);
                                _y = top;
                                is_first = false;
                            }
                            else
                            {
                                _width = ((int)((double)(band_edge_offset - Low) / (High - Low) * W)) - _x;
                                _height = H - _y;
                                g.DrawRectangle(new Pen(band_box_color, band_box_width), new Rectangle(_x, _y, _width, _height));
                                is_first = true;
                            }
                        }
                    } // for loop for 11m
#endif
                    break; //RUSS




                //============================================================================================================
                //============================================================================================================
                //============================================================================================================
                //===============================================
                case FRSRegion.Region_3: // (no 60m)
                case FRSRegion.Japan:

                    for (int i = 0; i < f_steps + 1; i++)
                    {
                        string label;
                        int offsetL;
                        int offsetR;

                        int fgrid = i * freq_step_size + (Low / freq_step_size) * freq_step_size;
                        double actual_fgrid = ((double)(vfo_round + fgrid)) / 1000000;
                        int vgrid = (int)((double)(fgrid - vfo_delta - Low) / (High - Low) * W);

                        if (!show_freq_offset)
                        {
                            if (actual_fgrid == 1.8 || actual_fgrid == 2.0 ||
                                actual_fgrid == 3.5 || actual_fgrid == 3.9 ||
                                actual_fgrid == 5.250 || actual_fgrid == 5.45 ||   // ke9ns add
                                actual_fgrid == 7.0 || actual_fgrid == 7.3 ||
                                actual_fgrid == 10.1 || actual_fgrid == 10.15 ||
                                actual_fgrid == 14.0 || actual_fgrid == 14.35 ||
                                actual_fgrid == 18.068 || actual_fgrid == 18.168 ||
                                actual_fgrid == 21.0 || actual_fgrid == 21.45 ||
                                actual_fgrid == 24.89 || actual_fgrid == 24.99 ||
                                actual_fgrid == 28.0 || actual_fgrid == 29.7 ||
                                actual_fgrid == 50.0 || actual_fgrid == 54.0 ||
                                actual_fgrid == 144.0 || actual_fgrid == 148.0)
                            {
                                if (bottom) g.DrawLine(new Pen(band_edge_color), vgrid, H + top, vgrid, H + H);
                                else g.DrawLine(new Pen(band_edge_color), vgrid, top, vgrid, H);

                                label = actual_fgrid.ToString("f4");
                                label = label.Replace(",", ".");    // handle Windows localization issues
                                int offset = label.IndexOf('.') + 4;
                                label = label.Insert(offset, ".");

                                if (actual_fgrid < 10) offsetL = (int)((label.Length + 2) * 4.1) - 14;
                                else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                else offsetL = (int)((label.Length) * 4.1) - 8;

                                if (bottom) g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid - offsetL, H + (float)Math.Floor(H * .01));
                                else g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid - offsetL, (float)Math.Floor(H * .01));

                                int fgrid_2 = ((i + 1) * freq_step_size) + (int)((Low / freq_step_size) * freq_step_size);
                                int x_2 = (int)(((float)(fgrid_2 - vfo_delta - Low) / width * W));
                                float scale = (float)(x_2 - vgrid) / inbetweenies;

                                for (int j = 1; j < inbetweenies; j++)
                                {
                                    if (grid_off == 0) // ke9ns add (dont draw grid lines if =1 
                                    {
                                        float x3 = (float)vgrid + (j * scale);
                                        if (bottom) g.DrawLine(grid_pen_dark, x3, H + top, x3, H + H);
                                        else g.DrawLine(grid_pen_dark, x3, top, x3, H);
                                    }
                                }
                            }
                            else
                            {
                                if (grid_off == 0) // ke9ns add (dont draw grid lines if =1 
                                {
                                    if (bottom) g.DrawLine(grid_pen, vgrid, H + top, vgrid, H + H);
                                    else g.DrawLine(grid_pen, vgrid, top, vgrid, H);            //wa6ahl
                                }
                                int fgrid_2 = ((i + 1) * freq_step_size) + (int)((Low / freq_step_size) * freq_step_size);
                                int x_2 = (int)(((float)(fgrid_2 - vfo_delta - Low) / width * W));
                                float scale = (float)(x_2 - vgrid) / inbetweenies;

                                for (int j = 1; j < inbetweenies; j++)
                                {
                                    if (grid_off == 0) // ke9ns add (dont draw grid lines if =1 
                                    {
                                        float x3 = (float)vgrid + (j * scale);
                                        if (bottom) g.DrawLine(grid_pen_dark, x3, H + top, x3, H + H);
                                        else g.DrawLine(grid_pen_dark, x3, top, x3, H);
                                    }
                                }

                                double actual_fgrid_label = Math.Round(actual_fgrid, 4);
                                label = actual_fgrid_label.ToString("f4");
                                label = label.Replace(",", ".");    // handle Windows localization issues
                                int offset = label.IndexOf('.') + 4;
                                label = label.Insert(offset, ".");

                                if (actual_fgrid < 10) offsetL = (int)((label.Length + 2) * 4.1) - 14;
                                else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                else offsetL = (int)((label.Length) * 4.1) - 8;

                                if (bottom) g.DrawString(label, font, grid_text_brush, vgrid - offsetL, H + (float)Math.Floor(H * .01));
                                else g.DrawString(label, font, grid_text_brush, vgrid - offsetL, (float)Math.Floor(H * .01));
                            }
                        }
                        else
                        {
                            vgrid = Convert.ToInt32((double)-(fgrid - Low) / (Low - High) * W); //wa6ahl
                            if (grid_off == 0) // ke9ns add (dont draw grid lines if =1 
                            {
                                if (bottom) g.DrawLine(grid_pen, vgrid, H + top, vgrid, H + H);
                                else g.DrawLine(grid_pen, vgrid, top, vgrid, H);            //wa6ahl
                            }
                            double new_fgrid = (vfoa_hz + fgrid) / 1000000;

                            label = fgrid.ToString();
                            offsetL = (int)((label.Length + 1) * 4.1);
                            offsetR = (int)(label.Length * 4.1);
                            if ((vgrid - offsetL >= 0) && (vgrid + offsetR < W) && (fgrid != 0))
                            {
                                if (bottom) g.DrawString(label, font, grid_text_brush, vgrid - offsetL, H + (float)Math.Floor(H * .01));
                                else g.DrawString(label, font, grid_text_brush, vgrid - offsetL, (float)Math.Floor(H * .01));
                            }
                        }
                    }

                    int[] band_edge_list_r8 = {  1800000, 2000000, 3500000, 3900000, 5250000,5450000,
                                                7000000, 7300000, 10100000, 10150000, 14000000, 14350000, 18068000, 18168000, 21000000, 21450000,
				                                24890000, 24990000, 28000000, 29700000, 50000000, 54000000, 144000000, 148000000 };

                    for (int i = 0; i < band_edge_list_r8.Length; i++)
                    {
                        double band_edge_offset = band_edge_list_r8[i] - vfo;
                        if (band_edge_offset >= Low && band_edge_offset <= High)
                        {
                            int temp_vline = (int)((double)(band_edge_offset - Low) / (High - Low) * W);//wa6ahl
                            if (bottom) g.DrawLine(new Pen(band_edge_color), temp_vline, H + top, temp_vline, H + H);//wa6ahl
                            else g.DrawLine(new Pen(band_edge_color), temp_vline, top, temp_vline, H);//wa6ahl
                        }
                    }

                    if (console.CurrentRegion == FRSRegion.Japan) // 
                    {
                        // draw special 80 segments, emergency channel, and 60m band segment rectangles - European ( Germany, Belgium, Spain, Switzerland, Finland, Luxembourg) NO TX in 60m
                        int[] band_edge_list_r15 = { 1810000, 1825000, 1907500, 1912500, 3500000, 3575000, 3599000, 3612000, 3680000, 3687000, 3702000, 3716000, 3745000, 3770000, 3791000, 3805000, 4629995, 4630005, 5351500, 5353999, 5354000, 5365999, 5366000, 5366500 }; // no TX

                        for (int i = 0; i < band_edge_list_r15.Length; i++)
                        {
                            double band_edge_offset = band_edge_list_r15[i] - vfo;
                            if (bottom)
                            {
                                if (is_first)
                                {
                                    _x = (int)((double)(band_edge_offset - Low) / (High - Low) * W);
                                    _y = H + top;
                                    is_first = false;
                                }
                                else
                                {
                                    _width = ((int)((double)(band_edge_offset - Low) / (High - Low) * W)) - _x;
                                    _height = (H + H) - _y;
                                    g.DrawRectangle(new Pen(band_box_color, band_box_width), new Rectangle(_x, _y, _width, _height));
                                    is_first = true;
                                }
                            }
                            else
                            {
                                if (is_first)
                                {
                                    _x = (int)((double)(band_edge_offset - Low) / (High - Low) * W);
                                    _y = top;
                                    is_first = false;
                                }
                                else
                                {
                                    _width = ((int)((double)(band_edge_offset - Low) / (High - Low) * W)) - _x;
                                    _height = H - _y;
                                    g.DrawRectangle(new Pen(band_box_color, band_box_width), new Rectangle(_x, _y, _width, _height));
                                    is_first = true;
                                }
                            }
                        } // for loop for 60m
                    } // Japan
                    else // Region 3 (asia)
                    {

                        // draw 60m band segment rectangles - European ( Germany, Belgium, Spain, Switzerland, Finland, Luxembourg)
                        int[] band_edge_list_r15 = {  5351500, 5353999, 5354000, 5365999, 5366000, 5366500 }; // no TX

                        for (int i = 0; i < band_edge_list_r15.Length; i++)
                        {
                            double band_edge_offset = band_edge_list_r15[i] - vfo;
                            if (bottom)
                            {
                                if (is_first)
                                {
                                    _x = (int)((double)(band_edge_offset - Low) / (High - Low) * W);
                                    _y = H + top;
                                    is_first = false;
                                }
                                else
                                {
                                    _width = ((int)((double)(band_edge_offset - Low) / (High - Low) * W)) - _x;
                                    _height = (H + H) - _y;
                                    g.DrawRectangle(new Pen(band_box_color, band_box_width), new Rectangle(_x, _y, _width, _height));
                                    is_first = true;
                                }
                            }
                            else
                            {
                                if (is_first)
                                {
                                    _x = (int)((double)(band_edge_offset - Low) / (High - Low) * W);
                                    _y = top;
                                    is_first = false;
                                }
                                else
                                {
                                    _width = ((int)((double)(band_edge_offset - Low) / (High - Low) * W)) - _x;
                                    _height = H - _y;
                                    g.DrawRectangle(new Pen(band_box_color, band_box_width), new Rectangle(_x, _y, _width, _height));
                                    is_first = true;
                                }
                            }
                        } // for loop for 60m
                    }

                    break;  // FRSRegion.Region_3: Japan 
                  

            } // Band edges (all regions)



            //===============================================================
            // Draw horizontal lines  ke9ns hstep
            //===============================================================

            for (int i=1; i < h_steps; i++)  // h_steps number of times
     		{
				int xOffset = 0;

                int num;


                if ( (K9 == 5) & (K10 != 5) && (bottom))  num = spectrum_grid_max - (i * (spectrum_grid_step * 2));  //ke9ns mod we want RX2 step to be the same as RX1 while in panafall
                else if ( (K9 == 5) & (K10 == 5) && (bottom)) num = spectrum_grid_max - (i * (spectrum_grid_step * 2));  // ke9ns mod we want RX2 step to be the same as RX1 while in panafall 
                else  num = spectrum_grid_max - (i * grid_step);                // generate the proper db number

                int y;
                if ( (K9 == 5) & (K10 != 5) && (bottom)) y = (int)((double)(spectrum_grid_max - num) * H1 / y_range);   // ke9ns mod
                else if ((K9 == 5) & (K10 == 5) && (bottom)) y = (int)((double)(spectrum_grid_max - num) * H1 / y_range); // ke9ns mod
			    else y = (int)((double)(spectrum_grid_max - num) * H / y_range);

                if (grid_off == 0) // ke9ns add (dont draw grid lines if =1 
                {
                    if (bottom) g.DrawLine(grid_pen, 0, H + y, W, H + y);  // draw lines 
                    else g.DrawLine(grid_pen, 0, y, W, y);
                }


                if ((console.BeaconSigAvg == true)) // ke9ns add draw blue line to show 0 and 1 threshold for BCD time signal from WWV
                {
                    if ((bottom == false) && (SpotForm.WTime == true) && (SpotForm.WWVPitch == false))
                    {
                        int thres = (int)((double)(spectrum_grid_max - SpotForm.WWVThreshold) * H / y_range);

                        g.DrawLine(p3, 100  , thres, W - 100 , thres);
                        g.DrawString(SpotForm.indexS.ToString(), font, grid_text_brush, 600, thres - 15);


                       // g.DrawLine(p3, 100, thres, W - 100, thres);
                    }
                }

               //   if (bottom) Debug.WriteLine("bottom..H " + H + " hpstep " + h_pixel_step + " hstep "+ h_steps + " top " + top + " num "+num + " gstep "+ grid_step + " Y "+ y + " yrange " + y_range);
               //   else Debug.WriteLine("top..H " + H + " hpstep " + h_pixel_step + " hstep " + h_steps + " top " + top + " num " + num + " gstep " + grid_step + " Y " + y + " yrange " + y_range);


            //===============================================================
            // Draw horizontal line labels
            //===============================================================
				if(i != 1) // avoid intersecting vertical and horizontal labels
				{

                    if ( (K9 == 5) & (K10 != 5) && (bottom)) num = spectrum_grid_max - (i * (spectrum_grid_step * 2)); // ke9ns mod (lines based on area of panadapter and waterfall etc)
                    else if ( (K9 == 5) & (K10 == 5) && (bottom)) num = spectrum_grid_max - (i * (spectrum_grid_step * 2)); // ke9ns mod
                    else  num = spectrum_grid_max - (i * grid_step);

					string label = num.ToString();
					if(label.Length == 3)	xOffset = (int)g.MeasureString("-", font).Width - 2;

					int offset = (int)(label.Length*4.1);
					SizeF size = g.MeasureString(label, font);

					int x = 0;
					switch(display_label_align)
					{
						case DisplayLabelAlignment.LEFT:
							x = xOffset + 3;
							break;
						case DisplayLabelAlignment.CENTER:
							x = center_line_x+xOffset;
							break;
						case DisplayLabelAlignment.RIGHT:
							x = (int)(W-size.Width - 3);
							break;
						case DisplayLabelAlignment.AUTO:
							x = xOffset + 3;
							break;
                        case DisplayLabelAlignment.Sunit: // ke9ns add
                            x = xOffset + 3;
                            break;
                        case DisplayLabelAlignment.OFF:
							x = W;
							break;
					}

					y -= 8;

					if( (y+9) < H)
					{

                        if (bottom) g.DrawString(label, font, grid_text_brush, x, H+y);  // draw dBm readings
						else g.DrawString(label, font, grid_text_brush, x, y);

                      
                        //-----------------------------------------------------------------------------
                        // ke9ns add to show S units for both HF and VHF
                       //  was if (grid_off == 1) // ke9ns add  (when grid off then draw dBm number on right side )
                        if ((!local_mox) && ((display_label_align == DisplayLabelAlignment.LEFT) || (display_label_align == DisplayLabelAlignment.AUTO) || (display_label_align == DisplayLabelAlignment.Sunit))) // ke9ns add 
                        {
                         
                            string SS;

                            if (VFOA < 30000000)
                            {
                                if (num >= -68) // wait until your 5dbm over s9
                                {
                                    SS = "+" + (num + 73).ToString();
                                }
                            
                                else if (num >= -73) SS = "S9"; // S9 is -73 to -64
                                else if (num >= -79) SS = "S8"; // S8 is -79 to -74
                                else if (num >= -85) SS = "S7"; // S7 is -85 to -80
                                else if (num >= -91) SS = "S6"; // S6 is -91 to -86
                                else if (num >= -97) SS = "S5"; // S5 is -97 to -92
                                else if (num >= -103) SS = "S4"; // S4 is -103 to -98
                                else if (num >= -109) SS = "S3"; // S3 is -109 to -104 
                                else if (num >= -115) SS = "S2"; // S2 is -115 to -110
                                else if (num >= -121) SS = "S1"; // S1 is -121 to -114 
                                else SS = "S0";                  // S0 is -133 to -122
                            }
                            else // VHF S readings
                            {

                                if (num >= -88) // wait until your 5dbm over s9
                                {
                                    SS = "+" + (num + 93).ToString();
                                }

                                else if (num >= -93) SS = "S9";  // S9 is -93 to -82
                                else if (num >= -99) SS = "S8";  // S8 is -99 to -94
                                else if (num >= -105) SS = "S7"; // S7 is -105 to -100
                                else if (num >= -111) SS = "S6"; // S6 is -111 to -106
                                else if (num >= -117) SS = "S5"; // S5 is -117 to -112
                                else if (num >= -123) SS = "S4"; // S4 is -123 to -118
                                else if (num >= -129) SS = "S3"; // S3 is -129 to -124 
                                else if (num >= -135) SS = "S2"; // S2 is -135 to -130
                                else if (num >= -141) SS = "S1"; // S1 is -141 to -134 
                                else SS = "S0";                  // S0 is -153 to -142
                            }

                          
                            size = g.MeasureString(SS, font);

                            x = (int)(W - size.Width - 3); //draw on right side

                            if (bottom) g.DrawString(SS, font, grid_text_brush, x, H + y); // draw S meter readings on Right side of screen only
                            else g.DrawString(SS, font, grid_text_brush, x, y);

                        } //  display dbm on left


                    } // if( (y+9) < H)



                } // i != 0

			} // for loop to draw hor lines and db numbers



            //===============================================================
            // Draw 0Hz vertical line if visible
            //===============================================================
  		
			if(center_line_x >= 0 && center_line_x <= W)
			{
				if(bottom)
				{
					g.DrawLine(new Pen(grid_zero_color), center_line_x , H+top, center_line_x , H+H);
					g.DrawLine(new Pen(grid_zero_color), center_line_x+1 , H+top, center_line_x+1 , H+H);
				}
				else
				{
					g.DrawLine(new Pen(grid_zero_color), center_line_x , top, center_line_x , H);
					g.DrawLine(new Pen(grid_zero_color), center_line_x+1 , top, center_line_x+1 , H);
				}
			}

			if(show_freq_offset)
			{
				if(bottom) g.DrawString("0", font, new SolidBrush(grid_zero_color), center_line_x-5, H+(float)Math.Floor(H*.01));
				else g.DrawString("0", font, new SolidBrush(grid_zero_color), center_line_x-5, (float)Math.Floor(H*.01));
			}

			if(high_swr && rx==1)
				g.DrawString("High SWR", new System.Drawing.Font("Arial", 14, FontStyle.Bold), new SolidBrush(Color.Red), 245, 20);



            //===============================================================================================================================================================
            //===============================================================================================================================================================
            //===============================================================================================================================================================
            //===============================================================================================================================================================
            //=====================================================================
            //=====================================================================
            // ke9ns add DRAW MEMORY SPOTS ON PANADAPTER
            //=====================================================================
            //=====================================================================

            if ((SpotControl.SP6_Active != 0) && (!mox) && (console.chkPower.Checked == true))// do memory spot if active and not transmitting
            {

                int iii = 0;                          // ke9ns add stairstep holder

                int kk = 0;                           // ke9ns add index for holder[] after you draw the vert line, then draw calls (so calls can overlap the vert lines)

                int vfo_hz = (int)vfoa_hz;    // vfo freq in hz

                int H1a = H / 2;            // length of vertical line (based on rx1 and rx2 display window configuration)
                int H1b = 20;               // starting point of vertical line

                // RX3/RX4 PanF/Pan = 5,2 (K9,K10)(short)  PanF/PanF = 5,5, (short) Pan/Pan 2,2 (long)
                if (bottom)                 // if your drawing to the bottom 
                {
                    if ((K9 == 2) && (K10 == 2)) H1a = H + (H / 2); // long
                    else H1a = H + (H / 4); // short

                    H1b = H + 20;

                    vfo_hz = (int)vfob_hz;
                    Console.MMK4 = 0;        // RX4 index to allow call signs to draw after all the vert lines on the screen

                }
                else
                {
                    Console.MMK3 = 0;        // RX3 index to allow call signs to draw after all the vert lines on the screen

                   
                }

                VFOLow = vfo_hz + RXDisplayLow;    // low freq (left side) in hz
                VFOHigh = vfo_hz + RXDisplayHigh; // high freq (right side) in hz
                VFODiff = VFOHigh - VFOLow;       // diff in hz

                int gg = SpotForm.dataGridView2.Rows.Count;  // get current # of memories we have available
              //  int gg = console.comboFMMemory.Items.Count;

                //-------------------------------------------------------------------------------------------------
                //-------------------------------------------------------------------------------------------------
                // draw MEMORY SPOTS
                //-------------------------------------------------------------------------------------------------
                //-------------------------------------------------------------------------------------------------


                for (int ii = 0; ii < gg ; ii++)     // Index through entire DXspot to find what is on this panadapter (draw vert lines first)
                {
                    
                    int hh = (int)(Convert.ToDouble(SpotForm.dataGridView2[1, ii].Value) * 1000000);  // MEMORY "RXFREQ"  convert to hz

                    if ( ( hh >= VFOLow) && (hh <= VFOHigh)) // find MEMORIES that appear on PAN
                    {
 
                        int VFO_DXPos = (int)((((float)W / (float)VFODiff) * (float)(hh - VFOLow))); // determine MEMORY spot line pos on current panadapter screen

                        holder2[kk] = ii;                    // ii is the actual MEMORY INDEX pos the the KK holds
                        holder3[kk] = VFO_DXPos;

                        kk++;

                        g.DrawLine(p3, VFO_DXPos, H1b, VFO_DXPos, H1a);   // draw vertical line

                    }

                } // for loop through MEMORIES


                if (bottom) Console.MMK4 = kk; // keep a count for the bottom MEMORY 
                else Console.MMK3 = kk; // count of spots in current panadapter


                //--------------------------------------------------------------------------------------------
                for (int ii = 0; ii < kk; ii++) // draw call signs to screen in order to draw over the vert lines
                {

                    string ll = (string)SpotForm.dataGridView2[2, holder2[ii]].Value;  // Name of MEMORY
                    string mm = (string)SpotForm.dataGridView2[0, holder2[ii]].Value;  // GROUP of MEMORY
                    string cc = (string)SpotForm.dataGridView2[9, holder2[ii]].Value;  // comments for hyperlinking of MEMORY

                    DSPMode nn = (DSPMode)SpotForm.dataGridView2[3, holder2[ii]].Value;  // DSPMODE of MEMORY

                    if ((nn == DSPMode.LSB) || (nn == DSPMode.DIGL) || (nn == DSPMode.CWL)) low = true;
                    else low = false;

                    // font
                    if (low) // 1=LSB so draw on left side of line
                    {
      
                        length = g.MeasureString(ll, font1);             //  temp used to determine the size of the string when in LSB and you need to reserve a certain space//  (cl.Width);
                        length1 = g.MeasureString(mm, font1);             //  length of "GROUP" string from Memory (to create a virtual box around the Memory Name & Group) to click on


                        g.DrawString(ll, font1, grid_text_brush, holder3[ii] - length.Width, H1b + iii);    // Memory Name
                        g.DrawString(mm, font1, grid_text_brush, holder3[ii] - length1.Width, H1b + iii+11); // Memory Group

                        if (bottom) rx3 = 50; // allow only 50 spots per Receiver
                        else rx3 = 0;

                        if (!mox) // only do when not transmitting
                        {

                            if (length1.Width > length.Width) length.Width = length1.Width; // make virtual box as wide as the widest text

                            Console.MMW[ii + rx3] = (int)length.Width;           // Width,Height,X,Y of upper left corner to locate where text actually is 
                            Console.MMH[ii + rx3] = (int)length.Height * 2;       // * 2 because of 2 lines of text
                            Console.MMX[ii + rx3] = holder3[ii] - (int)length.Width;
                            Console.MMY[ii + rx3] = H1b + iii;
                            Console.MMS[ii + rx3] = ll;
                            Console.MMC[ii + rx3] = cc; // comments
                            Console.MMM[ii + rx3] = holder2[ii];

                        }


                    } // LSB side
                    else   // 0=usb so draw on righ side of line (normal)
                    {
                       
                        length = g.MeasureString(ll, font1); //  not needed here but used for MEMORY NAME
                        length1 = g.MeasureString(mm, font1); //  length of "GROUP" string from Memory (to create a virtual box around the Memory Name & Group) to click on


                        g.DrawString(ll, font1, grid_text_brush, holder3[ii], H1b + iii); // Memory Name
                        g.DrawString(mm, font1, grid_text_brush, holder3[ii], H1b + iii + 11); // Memory Group
      
                        if (bottom) rx3 = 50;
                        else rx3 = 0;

                        if (!mox) // only do when not transmitting
                        {
                            if (length1.Width > length.Width) length.Width = length1.Width;

                            Console.MMW[ii + rx3] = (int)length.Width;    // Width,Height,X,Y of upper left corner to locate where text actually is 
                            Console.MMH[ii + rx3] = (int)length.Height * 2;  // H
                            Console.MMX[ii + rx3] = holder3[ii];         // X
                            Console.MMY[ii + rx3] = H1b + iii;           // Y
                            Console.MMS[ii + rx3] = ll;                  // Name of Memory showing up in Pan
                            Console.MMC[ii + rx3] = cc;                  // comments
                            Console.MMM[ii + rx3] = holder2[ii];         // index in memory.xml file
                        }

                    } // USB side

                    iii = iii + 22; // 11
                    if (iii > 90) iii = 0;


                }// for loop through MEMORIES


            } // MEMORY SPOTTING




            //===============================================================================================================================================================
            //===============================================================================================================================================================
            //===============================================================================================================================================================
            //===============================================================================================================================================================
            //=====================================================================
            //=====================================================================
            // ke9ns add DRAW SWL SPOTS ON PANADAPTER
            //=====================================================================
            //=====================================================================

            if ((!mox) && (console.chkPower.Checked == true) && (SpotControl.SP1_Active != 0))
            {
                DateTime UTCD = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc); // used by both RX1 and RX2 SWL display
                byte UTCDD = (byte)(1 << ((byte)UTCD.DayOfWeek));   // this is the day. Sun = 0, Mon = 1
                SpotControl.UTCNEW1 = Convert.ToInt16(UTCD.ToString("HHmm")); // convert 24hr UTC to int

                if ((!bottom) && (vfoa_hz < 30000000))// do SWL spot if active and not transmitting
                {

                    VFOLow = (int)vfoa_hz + RXDisplayLow; // low freq (left side) in hz
                    VFOHigh = (int)vfoa_hz + RXDisplayHigh; // high freq (right side) in hz
                    VFODiff = VFOHigh - VFOLow; // diff in hz

                    byte VFOLowB = (byte)(VFOLow / 1000000); // freq in mhz
                    byte VFOHighB = (byte)(VFOHigh / 1000000); // freq in mhz

                    int iii = 0; // stairstep the swl stations on the screen

                    int L_index = 0;                                              // 0Mhz in index
                    if (VFOLowB != 0) L_index = SpotControl.SWL_BandL[VFOLowB - 1]; // Left side  index position corresponding to the Left side Mhz
                    int H_index = SpotControl.SWL_BandL[VFOHighB];                // Right side index position corresponding to the right side Mhz

                    float XPOS = (float)W / (float)VFODiff;

                    int H1a = H * 2 / 3; // vert line length from top down
                   
                    // DayOfWeek UTCDD = UTCD.DayOfWeek; // day is spelled out: Monday, Tuesday


                    if ( (VFOHigh != SpotControl.VFOHLast)) // check if moved frequency
                    {

                        SpotControl.VFOHLast = VFOHigh;
                        SpotControl.Lindex = 0; // bottom of index to display spots
                        SpotControl.Hindex = 0; // top of index to display spots

                        Console.SXK = 0;


                        for (int ii = L_index; ii < H_index; ii++) // start by checking spots that fall within the mhz range of the panadapter
                        {
                            SpotControl.Hindex = ii; // get top index spot

                            if ((SpotControl.SWL_Freq[ii] > VFOHigh))
                            {
                                SpotControl.Hindex--; // get top index spot

                                break; // once a SWL spot if found off the right side of screen then DONE
                            }

                            if ((SpotControl.SWL_Freq[ii] >= VFOLow)) // find all swl spot within the screen area
                            {
                                if (SpotControl.Lindex == 0) SpotControl.Lindex = ii; // capture index of first valid spot on screen

                                //   Debug.Write(" FREQ-SWL " + ii);


                                // ke9ns check that the UTC day falls within the stations days listed at ON the air, then check the UTC time

                                if (
                            ((SpotControl.SWL_Day1[ii] & SpotControl.UTCDD) > 0) && (((SpotControl.SWL_TimeN[ii] <= SpotControl.UTCNEW1) && (SpotControl.SWL_TimeF[ii] >= SpotControl.UTCNEW1)) ||
                            ((SpotControl.SWL_TimeN[ii] >= SpotControl.UTCNEW1) && (SpotControl.SWL_TimeF[ii] >= SpotControl.UTCNEW1) && (SpotControl.SWL_TimeF[ii] < SpotControl.SWL_TimeN[ii])))
                             )
                                //   if ( ((SpotControl.SWL_Day1[ii] & SpotControl.UTCDD) > 0) && (SpotControl.SWL_TimeN[ii] <= SpotControl.UTCNEW1) && (SpotControl.SWL_TimeF[ii] >= SpotControl.UTCNEW1)) // ke9ns check if stations on the panadapter are on the air (based on time)
                                {


                                    int VFO_SWLPos = (int)(((XPOS) * (float)(SpotControl.SWL_Freq[ii] - VFOLow)));

                                    g.DrawLine(p2, VFO_SWLPos, 20, VFO_SWLPos, H1a);   // draw vertical line

                                    if ((Console.MMK3 > 0) && (SpotControl.SP6_Active != 0))
                                    {
                                        int x2 = VFO_SWLPos;
                                        int y2 = 20 + iii;

                                        SizeF length = g.MeasureString(SpotControl.SWL_Station[ii], font1); //  used for google lookups of SWL stations

                                        for (int jj = 0; jj < Console.MMK3; jj++)
                                        {

                                            if (((x2 + length.Width) >= Console.MMX[jj]) && (x2 < (Console.MMX[jj] + Console.MMW[jj])))
                                            {
                                                if (((y2 + length.Height) >= Console.MMY[jj]) && (y2 < (Console.MMY[jj] + Console.MMH[jj])))
                                                {
                                                    iii = iii + 33;
                                                    break;
                                                }
                                            }

                                        } // for loop to check if DX text will draw over top of Memory text
                                    }

                                    g.DrawString(SpotControl.SWL_Station[ii], font1, grid_text_brush, VFO_SWLPos, 20 + iii);

                                    //   Debug.WriteLine(" FINDSWL "+ ii );

                                    if (Console.SXK < 99)
                                    {

                                        SizeF length = g.MeasureString(SpotControl.SWL_Station[ii], font1); //  used for google lookups of SWL stations

                                        Console.SXW[Console.SXK] = (int)length.Width;
                                        Console.SXH[Console.SXK] = (int)length.Height;
                                        Console.SXX[Console.SXK] = VFO_SWLPos;
                                        Console.SXY[Console.SXK] = 20 + iii;
                                        Console.SXS[Console.SXK] = SpotControl.SWL_Station[ii];
                                        Console.SXF[Console.SXK] = SpotControl.SWL_Freq[ii];
                                        Console.SXM[Console.SXK] = SpotControl.SWL_Mode[ii];


                                        Console.SXK++;
                                    }
                                    else Debug.Write(" SXK OVERLIMIT ");


                                    iii = iii + 11; // stairstep spots
                                    if (iii > 90) iii = 0;

                                } // check time

                            } // make sure spot is > then left side of screen

                        } // for loop through SWL_Index

                        //   Debug.WriteLine(" L_index " + SpotControl.Lindex);
                        //   Debug.WriteLine(" H_index " + SpotControl.Hindex);

                        //   Debug.WriteLine(" VFOLow " + VFOLow);
                        //   Debug.WriteLine(" VFOHigh " + VFOHigh);


                    } // if you change vfo freq do above
                    else // if you dont change freq, then do below
                    {

                       

                            for (int ii = SpotControl.Lindex; ii <= SpotControl.Hindex; ii++) // now check only spots that fit exactly on panadapter
                            {
                                //  Debug.Write(" drawSWL " + ii);

                                if (
                             ((SpotControl.SWL_Day1[ii] & SpotControl.UTCDD) > 0) && (((SpotControl.SWL_TimeN[ii] <= SpotControl.UTCNEW1) && (SpotControl.SWL_TimeF[ii] >= SpotControl.UTCNEW1)) ||
                             ((SpotControl.SWL_TimeN[ii] >= SpotControl.UTCNEW1) && (SpotControl.SWL_TimeF[ii] >= SpotControl.UTCNEW1) && (SpotControl.SWL_TimeF[ii] < SpotControl.SWL_TimeN[ii])))
                              )
                                //     if (((SpotControl.SWL_Day1[ii] & SpotControl.UTCDD) > 0) && (SpotControl.SWL_TimeN[ii] <= SpotControl.UTCNEW1) && (SpotControl.SWL_TimeF[ii] >= SpotControl.UTCNEW1))
                                {
                                    int VFO_SWLPos = (int)(((XPOS) * (float)(SpotControl.SWL_Freq[ii] - VFOLow)));

                                    g.DrawLine(p2, VFO_SWLPos, 20, VFO_SWLPos, H1a);   // draw vertical line

                                    iii = iii + 11; // stairstep spots
                                    if (iii > 90) iii = 0;

                                } // check time

                            } // for loop to display all current swl spots
                            iii = 0;
                            for (int ii = SpotControl.Lindex; ii <= SpotControl.Hindex; ii++) // now check only spots that fit exactly on panadapter
                            {
                                //  Debug.Write(" drawSWL " + ii);

                                if (
                           ((SpotControl.SWL_Day1[ii] & SpotControl.UTCDD) > 0) && (((SpotControl.SWL_TimeN[ii] <= SpotControl.UTCNEW1) && (SpotControl.SWL_TimeF[ii] >= SpotControl.UTCNEW1)) ||
                           ((SpotControl.SWL_TimeN[ii] >= SpotControl.UTCNEW1) && (SpotControl.SWL_TimeF[ii] >= SpotControl.UTCNEW1) && (SpotControl.SWL_TimeF[ii] < SpotControl.SWL_TimeN[ii])))
                            )
                                //    if (((SpotControl.SWL_Day1[ii] & SpotControl.UTCDD) > 0) && (SpotControl.SWL_TimeN[ii] <= SpotControl.UTCNEW1) && (SpotControl.SWL_TimeF[ii] >= SpotControl.UTCNEW1))
                                {
                                    int VFO_SWLPos = (int)(((XPOS) * (float)(SpotControl.SWL_Freq[ii] - VFOLow)));

                                    if ((Console.MMK3 > 0) && (SpotControl.SP6_Active != 0))
                                    {
                                        int x2 = VFO_SWLPos;
                                        int y2 = 20 + iii;

                                        SizeF length = g.MeasureString(SpotControl.SWL_Station[ii], font1); //  used for google lookups of SWL stations

                                        for (int jj = 0; jj < Console.MMK3; jj++)
                                        {

                                            if (((x2 + length.Width) >= Console.MMX[jj]) && (x2 < (Console.MMX[jj] + Console.MMW[jj])))
                                            {
                                                if (((y2 + length.Height) >= Console.MMY[jj]) && (y2 < (Console.MMY[jj] + Console.MMH[jj])))
                                                {
                                                    iii = iii + 33;
                                                    break;
                                                }
                                            }

                                        } // for loop to check if DX text will draw over top of Memory text
                                    }

                                    g.DrawString(SpotControl.SWL_Station[ii], font1, grid_text_brush, VFO_SWLPos, 20 + iii); // draw station Name

                                    iii = iii + 11; // stairstep spots
                                    if (iii > 90) iii = 0;

                                } // check time

                            } // for loop to display all current swl spots
                       

                    } //do this above until you move freq again


                } // DISPLAY RX1 SWL SPots only


                //======================================================
                //======================================================
                //ke9ns add SWL spots to RX2

                if ((bottom) && (vfob_hz < 30000000))// do SWL spot if active and not transmitting
                {

                    VFOLow = (int)vfob_hz + RXDisplayLow; // low freq (left side) in hz
                    VFOHigh = (int)vfob_hz + RXDisplayHigh; // high freq (right side) in hz
                    VFODiff = VFOHigh - VFOLow; // diff in hz

                    byte VFOLowB = (byte)(VFOLow / 1000000); // freq in mhz
                    byte VFOHighB = (byte)(VFOHigh / 1000000); // freq in mhz

                    int iii = 0; // stairstep the swl stations on the screen

                    int L_index = 0;                                              // 0Mhz in index
                    if (VFOLowB != 0) L_index = SpotControl.SWL_BandL[VFOLowB - 1]; // Left side  index position corresponding to the Left side Mhz
                    int H_index = SpotControl.SWL_BandL[VFOHighB];                // Right side index position corresponding to the right side Mhz

                    float XPOS = (float)W / (float)VFODiff;


                    // below is for rx2 only
                    int H1a = H / 2;            // length of vertical line (based on rx1 and rx2 display window configuration)
                    int H1b = 20;               // starting point of vertical line

                    // RX3/RX4 PanF/Pan = 5,2 (K9,K10)(short)  PanF/PanF = 5,5, (short) Pan/Pan 2,2 (long)

                    if ((K9 == 2) && (K10 == 2)) H1a = H + (H / 2); // long
                    else H1a = H + (H / 4); // short

                    H1b = H + 20;


                    if ( (VFOHigh != SpotControl.VFOHLast)) // check if moved frequency
                    {

                        SpotControl.VFOHLast = VFOHigh;
                        SpotControl.Lindex = 0; // bottom of index to display spots
                        SpotControl.Hindex = 0; // top of index to display spots

                        Console.SXK2 = 0;


                        for (int ii = L_index; ii < H_index; ii++) // start by checking spots that fall within the mhz range of the panadapter
                        {
                            SpotControl.Hindex = ii; // get top index spot

                            if ((SpotControl.SWL_Freq[ii] > VFOHigh))
                            {
                                SpotControl.Hindex--; // get top index spot

                                break; // once a SWL spot if found off the right side of screen then DONE
                            }

                            if ((SpotControl.SWL_Freq[ii] >= VFOLow)) // find all swl spot within the screen area
                            {
                                if (SpotControl.Lindex == 0) SpotControl.Lindex = ii; // capture index of first valid spot on screen

                                //   Debug.Write(" FREQ-SWL " + ii);


                                // ke9ns check that the UTC day falls within the stations days listed at ON the air, then check the UTC time

                                if (
                                        ((SpotControl.SWL_Day1[ii] & SpotControl.UTCDD) > 0) && (((SpotControl.SWL_TimeN[ii] <= SpotControl.UTCNEW1) && (SpotControl.SWL_TimeF[ii] >= SpotControl.UTCNEW1)) ||
                                        ((SpotControl.SWL_TimeN[ii] >= SpotControl.UTCNEW1) && (SpotControl.SWL_TimeF[ii] >= SpotControl.UTCNEW1) && (SpotControl.SWL_TimeF[ii] < SpotControl.SWL_TimeN[ii])))
                                   )
                                {


                                    int VFO_SWLPos = (int)(((XPOS) * (float)(SpotControl.SWL_Freq[ii] - VFOLow)));

                                    g.DrawLine(p2, VFO_SWLPos, H1b, VFO_SWLPos, H1a);   // draw RX2 vertical line


                                    if ((Console.MMK4 > 0) && (SpotControl.SP6_Active != 0))
                                    {
                                        int x2 = VFO_SWLPos;
                                        int y2 = 20 + iii;

                                        SizeF length = g.MeasureString(SpotControl.SWL_Station[ii], font1); //  used for google lookups of SWL stations

                                        for (int jj = 0; jj < Console.MMK4; jj++)
                                        {

                                            if (((x2 + length.Width) >= Console.MMX[jj + rx3]) && (x2 < (Console.MMX[jj + rx3] + Console.MMW[jj + rx3])))
                                            {
                                                if (((y2 + length.Height) >= Console.MMY[jj + rx3]) && (y2 < (Console.MMY[jj + rx3] + Console.MMH[jj + rx3])))
                                                {
                                                    iii = iii + 33;
                                                    break;
                                                }
                                            }

                                        } // for loop to check if DX text will draw over top of Memory text
                                    }

                                    g.DrawString(SpotControl.SWL_Station[ii], font1, grid_text_brush, VFO_SWLPos, H1b + iii); // 20+iii

                                    //   Debug.WriteLine(" FINDSWL "+ ii );

                                    if (Console.SXK2 < 99)
                                    {

                                        SizeF length = g.MeasureString(SpotControl.SWL_Station[ii], font1); //  used for google lookups of SWL stations

                                        Console.SXW[Console.SXK2 + 100] = (int)length.Width;
                                        Console.SXH[Console.SXK2 + 100] = (int)length.Height;
                                        Console.SXX[Console.SXK2 + 100] = VFO_SWLPos;
                                        Console.SXY[Console.SXK2 + 100] = 20 + iii;
                                        Console.SXS[Console.SXK2 + 100] = SpotControl.SWL_Station[ii];
                                        Console.SXF[Console.SXK2 + 100] = SpotControl.SWL_Freq[ii];
                                        Console.SXM[Console.SXK2 + 100] = SpotControl.SWL_Mode[ii];

                                        Console.SXK2++;
                                    }
                                    else Debug.Write(" SXK2 OVERLIMIT ");


                                    iii = iii + 11; // stairstep spots
                                    if (iii > 90) iii = 0;

                                } // check time

                            } // make sure spot is > then left side of screen

                        } // for loop through SWL_Index

                        //   Debug.WriteLine(" L_index " + SpotControl.Lindex);
                        //   Debug.WriteLine(" H_index " + SpotControl.Hindex);

                        //   Debug.WriteLine(" VFOLow " + VFOLow);
                        //   Debug.WriteLine(" VFOHigh " + VFOHigh);


                    } // if you change vfo freq do above
                    else // if you dont change freq, then do below
                    {

                        
                            for (int ii = SpotControl.Lindex; ii <= SpotControl.Hindex; ii++) // now check only spots that fit exactly on panadapter
                            {
                                //  Debug.Write(" drawSWL " + ii);

                                if (
                             ((SpotControl.SWL_Day1[ii] & SpotControl.UTCDD) > 0) && (((SpotControl.SWL_TimeN[ii] <= SpotControl.UTCNEW1) && (SpotControl.SWL_TimeF[ii] >= SpotControl.UTCNEW1)) ||
                             ((SpotControl.SWL_TimeN[ii] >= SpotControl.UTCNEW1) && (SpotControl.SWL_TimeF[ii] >= SpotControl.UTCNEW1) && (SpotControl.SWL_TimeF[ii] < SpotControl.SWL_TimeN[ii])))
                              )
                                {
                                    int VFO_SWLPos = (int)(((XPOS) * (float)(SpotControl.SWL_Freq[ii] - VFOLow)));

                                    g.DrawLine(p2, VFO_SWLPos, H1b, VFO_SWLPos, H1a);   // draw RX2 vertical line

                                    iii = iii + 11; // stairstep spots
                                    if (iii > 90) iii = 0;

                                } // check time

                            } // for loop to display all current swl spots
                            iii = 0;
                            for (int ii = SpotControl.Lindex; ii <= SpotControl.Hindex; ii++) // now check only spots that fit exactly on panadapter
                            {
                                //  Debug.Write(" drawSWL " + ii);

                                if (
                           ((SpotControl.SWL_Day1[ii] & SpotControl.UTCDD) > 0) && (((SpotControl.SWL_TimeN[ii] <= SpotControl.UTCNEW1) && (SpotControl.SWL_TimeF[ii] >= SpotControl.UTCNEW1)) ||
                           ((SpotControl.SWL_TimeN[ii] >= SpotControl.UTCNEW1) && (SpotControl.SWL_TimeF[ii] >= SpotControl.UTCNEW1) && (SpotControl.SWL_TimeF[ii] < SpotControl.SWL_TimeN[ii])))
                            )
                                {
                                    int VFO_SWLPos = (int)(((XPOS) * (float)(SpotControl.SWL_Freq[ii] - VFOLow)));

                                    if ((Console.MMK4 > 0) && (SpotControl.SP6_Active != 0))
                                    {
                                        int x2 = VFO_SWLPos;
                                        int y2 = 20 + iii;

                                        SizeF length = g.MeasureString(SpotControl.SWL_Station[ii], font1); //  used for google lookups of SWL stations

                                        for (int jj = 0; jj < Console.MMK4; jj++)
                                        {

                                            if (((x2 + length.Width) >= Console.MMX[jj + rx3]) && (x2 < (Console.MMX[jj + rx3] + Console.MMW[jj + rx3])))
                                            {
                                                if (((y2 + length.Height) >= Console.MMY[jj + rx3]) && (y2 < (Console.MMY[jj + rx3] + Console.MMH[jj + rx3])))
                                                {
                                                    iii = iii + 33;
                                                    break;
                                                }
                                            }

                                        } // for loop to check if DX text will draw over top of Memory text
                                    }

                                    g.DrawString(SpotControl.SWL_Station[ii], font1, grid_text_brush, VFO_SWLPos, H1b + iii); // draw station Name  20+iii

                                    iii = iii + 11; // stairstep spots
                                    if (iii > 90) iii = 0;

                                } // check time

                            } // for loop to display all current swl spots
                        

                    } //do this above until you move freq again


                } // DISPLAY RX2 spots 


            } // CHECK FOR SWL listing   SP1_Active SWL CLUSTER




            //===============================================================================================================================================================
            //===============================================================================================================================================================
            //===============================================================================================================================================================
            //===============================================================================================================================================================
            //=====================================================================
            //=====================================================================
            // ke9ns add draw DX SPOTS on pandapter
            //=====================================================================
            //=====================================================================


            if ((SpotControl.SP_Active != 0) && (SpotForm.beacon1 == false)) // make sure DX cluster is running but Beacon chk is OFF so as to not crowd the screen.
            {

                int iii = 0;                          // ke9ns add stairstep holder

                int kk = 0;                           // ke9ns add index for holder[] after you draw the vert line, then draw calls (so calls can overlap the vert lines)

                int vfo_hz = (int)vfoa_hz;    // vfo freq in hz

                int H1a = H / 2;            // length of vertical line (based on rx1 and rx2 display window configuration)
                int H1b = 20;               // starting point of vertical line

                // RX1/RX2 PanF/Pan = 5,2 (K9,K10)(short)  PanF/PanF = 5,5, (short) Pan/Pan 2,2 (long)
                if (bottom)                 // if your drawing to the bottom 
                {
                    if ((K9 == 2) && (K10 == 2)) H1a = H + (H / 2); // long
                    else H1a = H + (H / 4); // short

                    H1b = H + 20;

                    vfo_hz = (int)vfob_hz;
                    Console.DXK2 = 0;        // RX2 index to allow call signs to draw after all the vert lines on the screen

                }
                else
                {
                    Console.DXK = 0;        // RX1 index to allow call signs to draw after all the vert lines on the screen
                }

                VFOLow = vfo_hz + RXDisplayLow;    // low freq (left side) in hz
                VFOHigh = vfo_hz + RXDisplayHigh; // high freq (right side) in hz
                VFODiff = VFOHigh - VFOLow;       // diff in hz

                if ((vfo_hz < 5000000) || ((vfo_hz > 6000000) && (vfo_hz < 8000000))) low = true; // LSB
                else low = false;     // usb

                //-------------------------------------------------------------------------------------------------
                //-------------------------------------------------------------------------------------------------
                // draw DX spots
                //-------------------------------------------------------------------------------------------------
                //-------------------------------------------------------------------------------------------------

                for (int ii = 0; ii < SpotControl.DX_Index; ii++)     // Index through entire DXspot to find what is on this panadapter (draw vert lines first)
                {
                  
                    if ((SpotControl.DX_Freq[ii] >= VFOLow) && (SpotControl.DX_Freq[ii] <= VFOHigh))
                    {
                        int VFO_DXPos = (int)((((float)W / (float)VFODiff) * (float)(SpotControl.DX_Freq[ii] - VFOLow))); // determine DX spot line pos on current panadapter screen
                      
                        holder[kk] = ii;                    // ii is the actual DX_INdex pos the the KK holds
                        holder1[kk] = VFO_DXPos;

                        kk++; 
                      
                        g.DrawLine(p1, VFO_DXPos, H1b, VFO_DXPos, H1a);   // draw vertical line
                       
                    }

                } // for loop through DX_Index


                int bb = 0;
                if (bottom)
                {
                    Console.DXK2 = kk; // keep a count for the bottom QRZ hyperlink
                    bb = Console.MMK4;
                }
                else
                {
                    Console.DXK = kk; // count of spots in current panadapter
                    bb = Console.MMK3; 
                }

              
                //--------------------------------------------------------------------------------------------
                for (int ii = 0; ii < kk; ii++) // draw call signs to screen in order to draw over the vert lines
                {

                    // font
                    if (low) // 1=LSB so draw on left side of line
                    {

                        if (Console.DXR == 0) // display Spotted on Pan
                        {
                            length = g.MeasureString(SpotControl.DX_Station[holder[ii]], font1); //  temp used to determine the size of the string when in LSB and you need to reserve a certain space//  (cl.Width);

                            if ((bb > 0) && (SpotControl.SP6_Active != 0))
                            {
                                int x2 = holder1[ii] - (int)length.Width;
                                int y2 = H1b + iii;

                                for (int jj = 0; jj < bb; jj++)
                                {

                                    if (((x2 + length.Width) >= Console.MMX[jj]) && (x2 < (Console.MMX[jj] + Console.MMW[jj])))
                                    {
                                        if (((y2 + length.Height) >= Console.MMY[jj]) && (y2 < (Console.MMY[jj] + Console.MMH[jj])))
                                        {
                                            iii = iii + 33;
                                            break;
                                        }
                                    }

                                } // for loop to check if DX text will draw over top of Memory text
                            }

                            g.DrawString(SpotControl.DX_Station[holder[ii]], font1, grid_text_brush, holder1[ii] - (int)length.Width, H1b + iii); // DX call sign to panadapter

                        }
                        else // display SPOTTER on Pan (not the Spotted)
                        {
                            length = g.MeasureString(SpotControl.DX_Spotter[holder[ii]], font1); //  temp used to determine the size of the string when in LSB and you need to reserve a certain space//  (cl.Width);

                            if ((bb > 0) && (SpotControl.SP6_Active != 0))
                            {
                                int x2 = holder1[ii] - (int)length.Width;
                                int y2 = H1b + iii;

                                for (int jj = 0; jj < bb; jj++)
                                {

                                    if (((x2 + length.Width) >= Console.MMX[jj]) && (x2 < (Console.MMX[jj] + Console.MMW[jj])))
                                    {
                                        if (((y2 + length.Height) >= Console.MMY[jj]) && (y2 < (Console.MMY[jj] + Console.MMH[jj])))
                                        {
                                            iii = iii + 33;
                                            break;
                                        }
                                    }

                                } // for loop to check if DX text will draw over top of Memory text
                            }

                            g.DrawString(SpotControl.DX_Spotter[holder[ii]], font1, grid_text_brush, holder1[ii] - (int)length.Width, H1b + iii);

                        }

                        if (bottom) rx2 = 50; // allow only 50 qrz spots per Receiver
                        else rx2 = 0;

                        if (!mox) // only do when not transmitting
                        {
                            Console.DXW[ii + rx2] = (int)length.Width;    // this is all for QRZ hyperlinking 
                            Console.DXH[ii + rx2] = (int)length.Height;
                            Console.DXX[ii + rx2] = holder1[ii] - (int)length.Width;
                            Console.DXY[ii + rx2] = H1b + iii;
                            Console.DXS[ii + rx2] = SpotControl.DX_Station[holder[ii]];
                            
                        }
                        

                    } // LSB side


                    else   // 0=usb so draw on righ side of line (normal)
                    {
                        if (Console.DXR == 0) // spot
                        {
                            length = g.MeasureString(SpotControl.DX_Station[holder[ii]], font1); //  not needed here but used for qrz hyperlinking

                            if ((bb > 0) && (SpotControl.SP6_Active != 0))
                            {
                                int x2 = holder1[ii];
                                int y2 = H1b + iii;

                                for (int jj = 0; jj < bb; jj++)
                                {

                                    if (((x2 + length.Width) >= Console.MMX[jj]) && (x2 < (Console.MMX[jj] + Console.MMW[jj])))
                                    {
                                        if (((y2 + length.Height) >= Console.MMY[jj]) && (y2 < (Console.MMY[jj] + Console.MMH[jj])))
                                        {
                                            iii = iii + 33;
                                            break;
                                        }
                                    }

                                } // for loop to check if DX text will draw over top of Memory text
                            }

                            g.DrawString(SpotControl.DX_Station[holder[ii]], font1, grid_text_brush, holder1[ii], H1b + iii); // DX station name
                        }
                        else // spotter
                        {
                            length = g.MeasureString(SpotControl.DX_Spotter[holder[ii]], font1); //  not needed here but used for qrz hyperlinking

                            if ((bb > 0) && (SpotControl.SP6_Active != 0))
                            {
                                int x2 = holder1[ii];
                                int y2 = H1b + iii;

                                for (int jj = 0; jj < bb; jj++)
                                {

                                    if (((x2 + length.Width) >= Console.MMX[jj]) && (x2 < (Console.MMX[jj] + Console.MMW[jj])))
                                    {
                                        if (((y2 + length.Height) >= Console.MMY[jj]) && (y2 < (Console.MMY[jj] + Console.MMH[jj])))
                                        {
                                            iii = iii + 33;
                                            break;
                                        }
                                    }

                                } // for loop to check if DX text will draw over top of Memory text
                            }

                            g.DrawString(SpotControl.DX_Spotter[holder[ii]], font1, grid_text_brush, holder1[ii], H1b + iii); // DX station name

                        }

                        if (bottom) rx2 = 50;
                        else rx2 = 0;

                        if (!mox) // only do when not transmitting
                        {
                            Console.DXW[ii + rx2] = (int)length.Width;   // this is all for QRZ hyperlinking 
                            Console.DXH[ii + rx2] = (int)length.Height;
                            Console.DXX[ii + rx2] = holder1[ii];
                            Console.DXY[ii + rx2] = H1b + iii;
                            Console.DXS[ii + rx2] = SpotControl.DX_Station[holder[ii]];
                        }

                        if (vfo_hz >= 50000000) // 50000000 or 50mhz
                        {
                            iii = iii + 11;
                            g.DrawString(SpotControl.DX_Grid[holder[ii]], font1, grid_text_brush, holder1[ii], H1b + iii); // DX grid name
                        }

                    } // USB side

                    iii = iii + 11;
                    if (iii > 90) iii = 0;
                   

                }// for loop through DX_Index
         

            } // SP_Active DX SSB CLUSTER

            //===============================================================================================================================================================
            //===============================================================================================================================================================
   

        } // draw panadapter grid




        //==========================================================================================================================================
        //================================================================
        // ke9ns drawwaterfall text
        //       continuum mode draws db numbers instead of freq
        //================================================================

        private static void DrawWaterfallGrid(ref Graphics g, int W, int H, int rx, bool bottom)
		{
			// draw background
            // full screen W = 1607, H = 541  (shurnk W=1168, H=303)

         //  Debug.WriteLine("KE9NS DRAWWATERFALLGRID....H................. "+ H);
         //   Debug.WriteLine("KE9NS DRAWWATERFALLGRID....W................. " + W);

         
        // ke9ns this assures a black line for the waterfall frequencies to go into
            if (bottom) g.FillRectangle(new SolidBrush(display_background_color), 0, H, W, H);  // fill black on bottom half of display
            else g.FillRectangle(new SolidBrush(display_background_color), 0, 0, W, H);  // fill black into entire display
            

            
            // Low frequency to High frequency ?
            int low = rx_display_low;	// ke9ns  BASED ON SAMPLE RATE (192000, or 96000, etc) -4000 initial  but reads -97370 
			int high = rx_display_high; //  = 4000 initial but reads 79371

			int mid_w = W/2; // mid point of display window (horizontal)

			int[] step_list = {10, 20, 25, 50};

			int step_power = 1;
			int step_index = 0;
			int freq_step_size = 50;


             
			System.Drawing.Font font = new System.Drawing.Font("Swis721 BT", 9, FontStyle.Italic); // Arial size and style of freq text for waterfall

            SolidBrush grid_text_brush = new SolidBrush(grid_text_color);
			Pen grid_pen = new Pen(grid_color);
			Pen tx_filter_pen = new Pen(display_filter_tx_color);

			
            int y_range = spectrum_grid_max - spectrum_grid_min; // ke9ns 0 - -160 :     y_range = 160 default, but reads 120


			int filter_low, filter_high;  // ke9ns based on your audio filter settings

			int center_line_x = (int)(-(double)low/(high-low)*W); // ke9ns 885 full screen (shrunk 643) =(97370/(176741))*1607

          //  Debug.WriteLine("KE9NS Y-Range................. " + y_range);

          //  Debug.WriteLine("KE9NS Centerlinex................. " + center_line_x);
            
			if(mox) // get filter limits
			{
				filter_low = tx_filter_low;
				filter_high = tx_filter_high;
			}
			else if(rx==1)
			{
				filter_low = rx1_filter_low;
				filter_high = rx1_filter_high;
			}
			else //if(rx==2)
			{
				filter_low = rx2_filter_low;
				filter_high = rx2_filter_high;
			}

			if((rx1_dsp_mode == DSPMode.DRM && rx==1) ||
				(rx2_dsp_mode == DSPMode.DRM && rx==2))
			{
				filter_low = -5000;
				filter_high = 5000;
			}

			// Calculate hor step size (left to right)
			int width = high-low; // high freq - low freq

			while(width/freq_step_size > 10)
			{
				freq_step_size = step_list[step_index]*(int)Math.Pow(10.0, step_power);
				step_index = (step_index+1) % 4;
				if(step_index == 0) step_power++;
			}
			double w_pixel_step = (double)W * freq_step_size / width;
			int w_steps = width / freq_step_size;


            //================================================================
            // ke9ns drawwaterfall vertical tick marks
            //================================================================

			// calculate vertical step size
			int h_steps = (spectrum_grid_max - spectrum_grid_min)/spectrum_grid_step;

            if ((rx == 1) && (continuum == 1)) // ke9ns add (20 db steps listed on the top line)
            {
                h_steps = 20;
            }

            double h_pixel_step = (double)H / h_steps;

			int top = (int)((double)spectrum_grid_step * H / y_range); // ke9ns top=12
	
            if(bottom) top = top * 2;

            //   Debug.WriteLine("KE9NS top................. " + top);

            if ((continuum == 0)) // || (rx == 2))
            {
                if (rx == 1) // was !bottom
                {

                    //===========================================================================================
                    // ke9ns RX1 draw sub rec bandpass area
                    //===========================================================================================
                    if (!mox && sub_rx1_enabled && (!bottom))
                    {
                        // draw Sub RX 0Hz line
                        int x = (int)((float)(vfoa_sub_hz - vfoa_hz - low) / (high - low) * W);

                        g.DrawLine(new Pen(sub_rx_zero_line_color), x, 0, x, top);
                        g.DrawLine(new Pen(sub_rx_zero_line_color), x - 1, 0, x - 1, top);

                        // draw Sub RX filter
                        // get filter screen coordinates
                        int filter_left_x = (int)((float)(filter_low - low + vfoa_sub_hz - vfoa_hz) / (high - low) * W);
                        int filter_right_x = (int)((float)(filter_high - low + vfoa_sub_hz - vfoa_hz) / (high - low) * W);

                        // make the filter display at least one pixel wide.
                        if (filter_left_x == filter_right_x) filter_right_x = filter_left_x + 1;

                        // draw rx filter
                        g.FillRectangle(new SolidBrush(sub_rx_filter_color),    // draw filter overlay
                            filter_left_x, 0, filter_right_x - filter_left_x, top);

                    } // sub rx1 on


                    //===========================================================================================
                    // ke9ns RX1 draw main bandpass area
                    //===========================================================================================
                    if (!(mox && (rx1_dsp_mode == DSPMode.CWL || rx1_dsp_mode == DSPMode.CWU)))  // draw bandpass for RX or bandpass for TX (but not in cW mode)
                    {
                     
                        // get filter screen coordinates
                        int filter_left_x;
                        int filter_right_x;
                   
                        filter_left_x = (int)((float)(filter_low - low) / (high - low) * W);
                        filter_right_x = (int)((float)(filter_high - low) / (high - low) * W);
                     
                        // make the filter display at least one pixel wide.
                        if (filter_left_x == filter_right_x) filter_right_x = filter_left_x + 1;

                        if (bottom) // bottom half K9==3 or K9==5 if rx2 enabled
                        {

                         //   if (K9 == 3 )
                          //  {
                          //      g.FillRectangle(new SolidBrush(display_filter_color),   // draw filter overlay
                          //          filter_left_x, H, filter_right_x - filter_left_x, H + top); 
                          //  }
                        }
                        else // top half this would be water only K9 == 1
                        {
                                g.FillRectangle(new SolidBrush(display_filter_color),   // draw filter overlay
                                filter_left_x, 0, filter_right_x - filter_left_x,  top);
                        }
                    } // main RX1 waterfall bandpass

                    //===========================================================================================
                    // ke9ns RX1 draw tx anything but cw lines
                    //===========================================================================================
                    if ((!mox) && (draw_tx_filter) && (rx1_dsp_mode != DSPMode.CWL && rx1_dsp_mode != DSPMode.CWU))
                    {
                        // get tx filter limits
                        int filter_left_x;
                        int filter_right_x;

                        if (!split_enabled)
                        {
                            filter_left_x = (int)((float)(tx_filter_low - low + xit_hz) / (high - low) * W);
                            filter_right_x = (int)((float)(tx_filter_high - low + xit_hz) / (high - low) * W);
                        }
                        else
                        {
                            filter_left_x = (int)((float)(tx_filter_low - low + xit_hz + (vfoa_sub_hz - vfoa_hz)) / (high - low) * W);
                            filter_right_x = (int)((float)(tx_filter_high - low + xit_hz + (vfoa_sub_hz - vfoa_hz)) / (high - low) * W);
                        }

                        if (!bottom) // top half 
                        {
                            g.DrawLine(tx_filter_pen, filter_left_x, 0, filter_left_x,  top);        // draw tx filter overlay  0. H + top
                            g.DrawLine(tx_filter_pen, filter_right_x, 0, filter_right_x,top);      // draw tx filter overlay
                        }
                        else // bottom half  K9 ==3 or K9==5 if rX2 on
                        {

                            if (K9 == 3)
                            {
                                g.DrawLine(tx_filter_pen, filter_left_x, H, filter_left_x, H + top);        // draw tx filter overlay  0. H + top
                                g.DrawLine(tx_filter_pen, filter_right_x, H, filter_right_x, H + top);      // draw tx filter overlay
                            }
                
                        }

                    } // 

                    //===========================================================================================
                    // ke9ns RX1 draw tx cw line
                    //===========================================================================================
                    if (!mox && (draw_tx_cw_freq) && (rx1_dsp_mode == DSPMode.CWL || rx1_dsp_mode == DSPMode.CWU))
                    {
                        int pitch = cw_pitch;
                        if (rx1_dsp_mode == DSPMode.CWL)   pitch = -cw_pitch;


                        int cw_line_x;
                        if (!split_enabled)
                         
                             cw_line_x = (int)((float)(pitch - low + xit_hz) / (high - low) * W);
                   
                        else
                             cw_line_x = (int)((float)(pitch - low + xit_hz + (vfoa_sub_hz - vfoa_hz)) / (high - low) * W);

                        if (!bottom) // top
                        {
                            g.DrawLine(tx_filter_pen, cw_line_x, 0, cw_line_x, top);
                            g.DrawLine(tx_filter_pen, cw_line_x + 1, 0, cw_line_x + 1, top);
                        }
                        else // bottom half  K9 ==3 or K9==5 if rX2 on
                        {
                            if (K9 == 3)
                            {
                                g.DrawLine(tx_filter_pen, cw_line_x, H, cw_line_x, H + top);
                                g.DrawLine(tx_filter_pen, cw_line_x + 1, H, cw_line_x + 1, H + top);
                            }

                        }
                    }

                } // rx == 1
                else // rx == 2
                {

                    if (K10 == 1) // if RX2 only on water mode
                    {
                        //===========================================================================================
                        // ke9ns RX2 draw main  bandpass area
                        //===========================================================================================
                        if (!(mox && (rx2_dsp_mode == DSPMode.CWL || rx2_dsp_mode == DSPMode.CWU) ))  // draw bandpass for RX or bandpass for TX (but not in cW mode)) 
                        {
                            // get filter screen coordinates
                            int filter_left_x;
                            int filter_right_x;

                            filter_left_x = (int)((float)(filter_low - low) / (high - low) * W);
                            filter_right_x = (int)((float)(filter_high - low) / (high - low) * W);

                            // make the filter display at least one pixel wide.
                            if (filter_left_x == filter_right_x) filter_right_x = filter_left_x + 1;

                            // draw rx filter
                            g.FillRectangle(new SolidBrush(display_filter_color),   // draw filter overlay
                                filter_left_x, H, filter_right_x - filter_left_x, H + top);

                        } // RX2 main rx2 bandpass in the waterfall


                        //===========================================================================================
                        // ke9ns RX2 draw tx anything but cw lines
                        //===========================================================================================
                        if (!mox && draw_tx_filter && (rx2_dsp_mode != DSPMode.CWL && rx2_dsp_mode != DSPMode.CWU) && (tx_on_vfob))
                        {
                            // get tx filter limits
                            int filter_left_x;
                            int filter_right_x;

                            if (!split_enabled)
                            {
                                filter_left_x = (int)((float)(tx_filter_low - low + xit_hz) / (high - low) * W);
                                filter_right_x = (int)((float)(tx_filter_high - low + xit_hz) / (high - low) * W);
                            }
                            else
                            {
                                filter_left_x = (int)((float)(tx_filter_low - low + xit_hz + (vfoa_sub_hz - vfoa_hz)) / (high - low) * W);
                                filter_right_x = (int)((float)(tx_filter_high - low + xit_hz + (vfoa_sub_hz - vfoa_hz)) / (high - low) * W);
                            }

                            g.DrawLine(tx_filter_pen, filter_left_x, H, filter_left_x, H + top);        // draw tx filter overlay
                            g.DrawLine(tx_filter_pen, filter_right_x, H, filter_right_x, H + top);      // draw tx filter overlay

                        } // TX2 transmit lines

                        //===========================================================================================
                        // ke9ns RX2 draw tx cw line
                        //===========================================================================================
                        if ((!mox) && (draw_tx_cw_freq) && (rx2_dsp_mode == DSPMode.CWL || rx2_dsp_mode == DSPMode.CWU) && (tx_on_vfob))
                        {
                            int pitch = cw_pitch;
                            if (rx2_dsp_mode == DSPMode.CWL) pitch = -cw_pitch;

                            int cw_line_x;
                            if (!split_enabled)

                                cw_line_x = (int)((float)(pitch - low + xit_hz) / (high - low) * W);

                            else
                                cw_line_x = (int)((float)(pitch - low + xit_hz + (vfoa_sub_hz - vfoa_hz)) / (high - low) * W);

                            g.DrawLine(tx_filter_pen, cw_line_x, H, cw_line_x, H + top);
                            g.DrawLine(tx_filter_pen, cw_line_x + 1, H, cw_line_x + 1, H + top);

                        } // cw transmit line

                    }
            } // rx==2



        } // no continuum mode

			double vfo;
			

            //===========================================================================================

			if(mox)
			{
                if (split_enabled) vfo = vfoa_sub_hz;
                else   vfo = vfoa_hz;
               
                vfo += xit_hz; 
           
            }
			else if(rx==1)
			{
				vfo = vfoa_hz + rit_hz;
                switch (rx1_dsp_mode)
                {
                    case DSPMode.CWL:
                        vfo += cw_pitch;
                        break;
                    case DSPMode.CWU:
                        vfo -= cw_pitch;
                        break;
                    default:
                        break;
                }
            }
            else //if(rx==2)
			{
				vfo = vfob_hz + rit_hz;
                switch (rx2_dsp_mode)
                {
                    case DSPMode.CWL:
                        vfo += cw_pitch;
                        break;
                    case DSPMode.CWU:
                        vfo -= cw_pitch;
                        break;
                    default:
                        break;
                }
            }

			

			long vfo_round = ((long)(vfo/freq_step_size))*freq_step_size;  // round freq you are currently on
			long vfo_delta = (long)(vfo - vfo_round); // difference between real and rounded

            //   Debug.WriteLine("round " + vfo_round);
            //   Debug.WriteLine("delta " + vfo_delta);
            //  Debug.WriteLine("vfo " + vfo);



            if ((rx == 1) && (continuum == 1)) // ke9ns add
            {

                for (int i = 0; i <= h_steps + 1; i++)  // ke9ns draw freq numbers in line just above waterfall
                {
               
                    int temp = (W) / h_steps;

                    int temp1 = i * temp; //ke9ns x pixel position to print text string (left to right: 0 to W*3)  (W = 1388, so W*3 = 4164)

                    int temp2 = (W) - temp1;                                     // ke9ns add reverse function (left to right: W*3 to 0)
                                                                                     //  int temp4 = (int)(50)* 1;          // ke9ns add (50)

                    int temp3 = (int)((float)temp2 * ((float)150 / (float)(W)));     // ke9ns add (4164*(107 / 4164))                               convert postition to -dbm 

                    string label1 = temp3.ToString("d3");

                    g.DrawString(label1, font, grid_text_brush, temp1, (float)Math.Floor(H * .005)); // ke9ns shift labels over 100 to allow room for time stamp on left side

              
                    //  Debug.WriteLine("W " + W + " temp " + temp + " temp1 " + temp1 + " temp2 " + temp2 + " temp3 " + temp3);
                } // horz steps

            } // continuum mode

            else // not in continuum mode
            {

                // Draw vertical lines
                switch (console.CurrentRegion)
                {
                    //============================================================================================================
                    //============================================================================================================
                    //============================================================================================================
                    //===============================================
                    case FRSRegion.US:
                        //  Debug.Write("low " + waterfall_low_threshold + " high " + waterfall_high_threshold);

                        for (int i = 0; i <= h_steps + 1; i++)  // ke9ns draw freq numbers in line just above waterfall
                        {
                            string label;
                            int offsetL;
                            int offsetR;

                            int fgrid = i * freq_step_size + (low / freq_step_size) * freq_step_size;
                            double actual_fgrid = ((double)(vfo_round + fgrid)) / 1000000;  // freq to print out

                            int vgrid = (int)((double)(fgrid - vfo_delta - low ) / (high - low) * W);

                            //   Debug.WriteLine("fgrid " + fgrid);
                            //   Debug.WriteLine("Afgrid " + actual_fgrid);
                            //   Debug.WriteLine("vgrid " + vgrid);


                            if (!show_freq_offset)
                            {
                                if (actual_fgrid == 0.1357 || actual_fgrid == 0.1358 ||   // 2200m band edges
                                    actual_fgrid == 0.472 || actual_fgrid == 0.479 ||   // 630m band edges
                                    actual_fgrid == 1.8 || actual_fgrid == 2.0 ||   // 160m band edges
                                    actual_fgrid == 3.5 || actual_fgrid == 4.0 ||
                                    actual_fgrid == 5.250 || actual_fgrid == 5.45 ||   // ke9ns add
                                    actual_fgrid == 7.0 || actual_fgrid == 7.3 ||
                                    actual_fgrid == 10.1 || actual_fgrid == 10.15 ||
                                    actual_fgrid == 14.0 || actual_fgrid == 14.35 ||
                                    actual_fgrid == 18.068 || actual_fgrid == 18.168 ||
                                    actual_fgrid == 21.0 || actual_fgrid == 21.45 ||
                                    actual_fgrid == 24.89 || actual_fgrid == 24.99 ||

                                    actual_fgrid == 28.0 || actual_fgrid == 29.7 ||
                                    actual_fgrid == 50.0 || actual_fgrid == 54.0 ||
                                    actual_fgrid == 144.0 || actual_fgrid == 148.0)
                                {

                                    if (bottom) g.DrawLine(new Pen(band_edge_color), vgrid, H + top, vgrid, H + H); // ke9ns draw little tick lines under freq at band edges only
                                    else g.DrawLine(new Pen(band_edge_color), vgrid, top, vgrid, H);

                                    label = actual_fgrid.ToString("f4");
                                    label = label.Replace(",", ".");    // handle Windows localization issues
                                    int offset = label.IndexOf('.') + 4;
                                    label = label.Insert(offset, ".");

                                    if (actual_fgrid < 10) offsetL = (int)((label.Length + 2) * 4.1) - 14;
                                    else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                    else offsetL = (int)((label.Length) * 4.1) - 8;

                                    /* if (actual_fgrid < 10) offsetL = (int)((label.Length + 1) * 4.1) - 14;
                                    else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                    else offsetL = (int)((label.Length + 1) * 4.1) - 8; */

                                    if (bottom) g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid - offsetL, H + (float)Math.Floor(H * .005)); // was .01 ke9ns draw frequency at band edges in RED
                                    else g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid - offsetL, (float)Math.Floor(H * .005)); // .01

                                } // actual_fgrid
                                else
                                {

                                    if (freq_step_size >= 2000)
                                    {
                                        double t100;
                                        double t1000;
                                        t100 = (actual_fgrid * 100);
                                        t1000 = (actual_fgrid * 1000);

                                        int it100 = (int)t100;
                                        int it1000 = (int)t1000;

                                        int it100x10 = it100 * 10;

                                        if (it100x10 == it1000)
                                        {
                                        }
                                        else
                                        {
                                            grid_pen.DashStyle = DashStyle.Dot;
                                        }
                                    }
                                    else
                                    {
                                        if (freq_step_size == 1000)
                                        {
                                            double t200;
                                            double t2000;
                                            t200 = (actual_fgrid * 200);
                                            t2000 = (actual_fgrid * 2000);

                                            int it200 = (int)t200;
                                            int it2000 = (int)t2000;

                                            int it200x10 = it200 * 10;

                                            if (it200x10 == it2000)
                                            {
                                            }
                                            else
                                            {
                                                grid_pen.DashStyle = DashStyle.Dot;
                                            }
                                        }
                                        else
                                        {
                                            double t1000;
                                            double t10000;
                                            t1000 = (actual_fgrid * 1000);
                                            t10000 = (actual_fgrid * 10000);

                                            int it1000 = (int)t1000;
                                            int it10000 = (int)t10000;

                                            int it1000x10 = it1000 * 10;

                                            if (it1000x10 == it10000)
                                            {
                                            }
                                            else
                                            {
                                                grid_pen.DashStyle = DashStyle.Dot;
                                            }
                                        }
                                    }


                                    if (bottom) g.DrawLine(grid_pen, vgrid, H + top, vgrid, H + H);
                                    else g.DrawLine(grid_pen, vgrid, top, vgrid, H);            //wa6ahl

                                    grid_pen.DashStyle = DashStyle.Solid;

                                    double actual_fgrid_label = Math.Round(actual_fgrid, 4);
                                    label = actual_fgrid_label.ToString("f4");
                                    label = label.Replace(",", ".");    // handle Windows localization issues
                                    int offset = label.IndexOf('.') + 4;
                                    label = label.Insert(offset, ".");

                                    if (actual_fgrid < 10) offsetL = (int)((label.Length + 2) * 4.1) - 14;
                                    else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                    else offsetL = (int)((label.Length) * 4.1) - 8;

                                    /* if (((double)((int)(actual_fgrid * 1000))) == actual_fgrid * 1000)
                                    {
                                        label = actual_fgrid.ToString("f3"); //wa6ahl

                                        //if(actual_fgrid > 1300.0)
                                        //	label = label.Substring(label.Length-4);

                                        if (actual_fgrid < 10) offsetL = (int)((label.Length + 1) * 4.1) - 14;
                                        else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                        else offsetL = (int)((label.Length + 1) * 4.1) - 8;
                                    }
                                    else
                                    {
                                        string temp_string;
                                        int jper;
                                        label = actual_fgrid.ToString("f4");
                                        temp_string = label;
                                        jper = label.IndexOf('.') + 4;
                                        label = label.Insert(jper, " ");

                                        //if(actual_fgrid > 1300.0)
                                        //	label = label.Substring(label.Length-4);

                                        if (actual_fgrid < 10) offsetL = (int)((label.Length) * 4.1) - 14;
                                        else if (actual_fgrid < 100.0) offsetL = (int)((label.Length) * 4.1) - 11;
                                        else offsetL = (int)((label.Length) * 4.1) - 8;
                                    } */

                                    //================================================================
                                    // ke9ns drawwaterfall actually draw text
                                    //================================================================

                                    if (bottom)
                                    {
                                        g.DrawString(label, font, grid_text_brush, vgrid - offsetL, H + (float)Math.Floor(H * .005)); // .01
                                    }
                                    else
                                    {
                                        g.DrawString(label, font, grid_text_brush, vgrid - offsetL, (float)Math.Floor(H * .005));
                                    }

                                    // Debug.WriteLine("KE9NS H................. " + H);

                                } // no actual fgrid (these are the remaining frequences to show in the strip above the waterfall)

                            } // no showfreqoffset
                            else
                            {
                                vgrid = Convert.ToInt32((double)-(fgrid - low) / (low - high) * W); //wa6ahl
                                if (bottom) g.DrawLine(grid_pen, vgrid, H + top, vgrid, H + H);
                                else g.DrawLine(grid_pen, vgrid, top, vgrid, H);            //wa6ahl

                                double new_fgrid = (vfoa_hz + fgrid) / 1000000;

                                label = fgrid.ToString();
                                offsetL = (int)((label.Length + 1) * 4.1);
                                offsetR = (int)(label.Length * 4.1);
                                if ((vgrid - offsetL >= 0) && (vgrid + offsetR < W) && (fgrid != 0))
                                {
                                    if (bottom) g.DrawString(label, font, grid_text_brush, vgrid - offsetL, H + (float)Math.Floor(H * .005));
                                    else g.DrawString(label, font, grid_text_brush, vgrid - offsetL, (float)Math.Floor(H * .005));
                                }
                            } // show freq offset


                        } // for i loop

                      //  actual_fgrid == 0.1357 || actual_fgrid == 0.1358 ||   // 2200m band edges
                          //          actual_fgrid == 0.472 || actual_fgrid == 0.479 ||   // 630m band edges


                        int[] band_edge_list_r2 = { 0135700, 0137800, 0472000, 0479000, 1800000, 2000000, 3500000, 4000000, 5250000,5450000,
                                       7000000, 7300000, 10100000, 10150000, 14000000, 14350000, 18068000, 18168000, 21000000, 21450000,
                                       24890000, 24990000,26960000, 27410000, 28000000, 29700000, 50000000, 54000000, 144000000, 148000000 }; //ke9ns add CB

                        for (int i = 0; i < band_edge_list_r2.Length; i++)
                        {
                            double band_edge_offset = band_edge_list_r2[i] - vfo;
                            if (band_edge_offset >= low && band_edge_offset <= high)
                            {
                                int temp_vline = (int)((double)(band_edge_offset - low) / (high - low) * W);//wa6ahl
                                if (bottom) g.DrawLine(new Pen(band_edge_color), temp_vline, H, temp_vline, H + top);//wa6ahl
                                else g.DrawLine(new Pen(band_edge_color), temp_vline, 0, temp_vline, top);
                            }
                            if (i == 1 && !show_freq_offset) break;
                        }


                        break;  // ke9ns US region end of drawing freq values for waterfall FRSREGION.US


                    //============================================================================================================
                    //============================================================================================================
                    //============================================================================================================
                    //===============================================
                    case FRSRegion.Europe:      // EU00 (IARU1 60m) Germany
                   

                        for (int i = 0; i <= h_steps + 1; i++)
                        {
                            string label;
                            int offsetL;
                            int offsetR;

                            int fgrid = i * freq_step_size + (low / freq_step_size) * freq_step_size;
                            double actual_fgrid = ((double)(vfo_round + fgrid)) / 1000000;
                            int vgrid = (int)((double)(fgrid - vfo_delta - low) / (high - low) * W);

                            if (!show_freq_offset)
                            {
                                if (actual_fgrid == 1.81 || actual_fgrid == 2.0 ||
                                    actual_fgrid == 3.5 || actual_fgrid == 3.8 ||
                                    actual_fgrid == 5.250 || actual_fgrid == 5.45 ||   // ke9ns add
                                    actual_fgrid == 7.0 || actual_fgrid == 7.2 ||
                                    actual_fgrid == 10.1 || actual_fgrid == 10.15 ||
                                    actual_fgrid == 14.0 || actual_fgrid == 14.35 ||
                                    actual_fgrid == 18.068 || actual_fgrid == 18.168 ||
                                    actual_fgrid == 21.0 || actual_fgrid == 21.45 ||
                                    actual_fgrid == 24.89 || actual_fgrid == 24.99 ||
                                    actual_fgrid == 28.0 || actual_fgrid == 29.7 ||
                                    actual_fgrid == 50.03 || actual_fgrid == 51.0 ||
                                    actual_fgrid == 144.0 || actual_fgrid == 146.0)
                                {
                                    if (bottom) g.DrawLine(new Pen(band_edge_color), vgrid, H + top, vgrid, H + H);
                                    else g.DrawLine(new Pen(band_edge_color), vgrid, top, vgrid, H);

                                    label = actual_fgrid.ToString("f4");
                                    label = label.Replace(",", ".");    // handle Windows localization issues
                                    int offset = label.IndexOf('.') + 4;
                                    label = label.Insert(offset, ".");

                                    if (actual_fgrid < 10) offsetL = (int)((label.Length + 2) * 4.1) - 14;
                                    else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                    else offsetL = (int)((label.Length) * 4.1) - 8;

                                    /* if (actual_fgrid < 10) offsetL = (int)((label.Length + 1) * 4.1) - 14;
                                    else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                    else offsetL = (int)((label.Length + 1) * 4.1) - 8; */

                                    if (bottom) g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid - offsetL, H + (float)Math.Floor(H * .005));
                                    else g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid - offsetL, (float)Math.Floor(H * .005));
                                }
                                else
                                {

                                    if (freq_step_size >= 2000)
                                    {
                                        double t100;
                                        double t1000;
                                        t100 = (actual_fgrid * 100);
                                        t1000 = (actual_fgrid * 1000);

                                        int it100 = (int)t100;
                                        int it1000 = (int)t1000;

                                        int it100x10 = it100 * 10;

                                        if (it100x10 == it1000)
                                        {
                                        }
                                        else
                                        {
                                            grid_pen.DashStyle = DashStyle.Dot;
                                        }
                                    }
                                    else
                                    {
                                        if (freq_step_size == 1000)
                                        {
                                            double t200;
                                            double t2000;
                                            t200 = (actual_fgrid * 200);
                                            t2000 = (actual_fgrid * 2000);

                                            int it200 = (int)t200;
                                            int it2000 = (int)t2000;

                                            int it200x10 = it200 * 10;

                                            if (it200x10 == it2000)
                                            {
                                            }
                                            else
                                            {
                                                grid_pen.DashStyle = DashStyle.Dot;
                                            }
                                        }
                                        else
                                        {
                                            double t1000;
                                            double t10000;
                                            t1000 = (actual_fgrid * 1000);
                                            t10000 = (actual_fgrid * 10000);

                                            int it1000 = (int)t1000;
                                            int it10000 = (int)t10000;

                                            int it1000x10 = it1000 * 10;

                                            if (it1000x10 == it10000)
                                            {
                                            }
                                            else
                                            {
                                                grid_pen.DashStyle = DashStyle.Dot;
                                            }
                                        }
                                    }
                                    if (bottom) g.DrawLine(grid_pen, vgrid, H + top, vgrid, H + H);
                                    else g.DrawLine(grid_pen, vgrid, top, vgrid, H);            //wa6ahl
                                    grid_pen.DashStyle = DashStyle.Solid;

                                    // make freq grid labels
                                    double actual_fgrid_label = Math.Round(actual_fgrid, 4);
                                    label = actual_fgrid_label.ToString("f4");
                                    label = label.Replace(",", ".");    // handle Windows localization issues
                                    int offset = label.IndexOf('.') + 4;
                                    label = label.Insert(offset, ".");

                                    if (actual_fgrid < 10) offsetL = (int)((label.Length + 2) * 4.1) - 14;
                                    else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                    else offsetL = (int)((label.Length) * 4.1) - 8;

                                    if (bottom) g.DrawString(label, font, grid_text_brush, vgrid - offsetL, H + (float)Math.Floor(H * .005));
                                    else g.DrawString(label, font, grid_text_brush, vgrid - offsetL, (float)Math.Floor(H * .005));
                                }
                            }
                            else
                            {
                                vgrid = Convert.ToInt32((double)-(fgrid - low) / (low - high) * W); //wa6ahl
                                if (bottom) g.DrawLine(grid_pen, vgrid, H + top, vgrid, H + H);
                                else g.DrawLine(grid_pen, vgrid, top, vgrid, H);            //wa6ahl

                                double new_fgrid = (vfoa_hz + fgrid) / 1000000;

                                label = fgrid.ToString();
                                offsetL = (int)((label.Length + 1) * 4.1);
                                offsetR = (int)(label.Length * 4.1);
                                if ((vgrid - offsetL >= 0) && (vgrid + offsetR < W) && (fgrid != 0))
                                {
                                    if (bottom) g.DrawString(label, font, grid_text_brush, vgrid - offsetL, H + (float)Math.Floor(H * .005));
                                    else g.DrawString(label, font, grid_text_brush, vgrid - offsetL, (float)Math.Floor(H * .005));
                                }
                            }
                        }


                        int[] band_edge_list_r1 = {  1810000, 2000000, 3500000, 3800000, 5250000,5450000,
                                    7000000, 7200000, 10100000, 10150000, 14000000, 14350000,18068000, 18168000, 21000000, 21450000,
                                    24890000, 24990000, 28000000, 29700000, 50030000, 51000000, 144000000, 146000000 };

                        for (int i = 0; i < band_edge_list_r1.Length; i++)
                        {
                            double band_edge_offset = band_edge_list_r1[i] - vfo;
                            if (band_edge_offset >= low && band_edge_offset <= high)
                            {
                                int temp_vline = (int)((double)(band_edge_offset - low) / (high - low) * W);//wa6ahl
                                if (bottom) g.DrawLine(new Pen(band_edge_color), temp_vline, H, temp_vline, H + top);//wa6ahl
                                else g.DrawLine(new Pen(band_edge_color), temp_vline, 0, temp_vline, top);
                            }
                            if (i == 1 && !show_freq_offset) break;
                        }
 

                        break; // EU00





                    //============================================================================================================
                    //============================================================================================================
                    //============================================================================================================
                    //===============================================
                    case FRSRegion.UK:          // EU01 (no 60m) (UK+ has its own special segments)
                    case FRSRegion.France:      // EU01 (no 60m)
                    case FRSRegion.Slovakia:    // EU01 (no 60m)
                    case FRSRegion.UK_Plus:     // EU02
                    case FRSRegion.Norway:      // EU03  (5.25 to 5.45 60m) (also includes Czech rep)
                    case FRSRegion.Denmark:     // EU03 
                    case FRSRegion.Sweden:      // EU06 (IARU1 60m)& (52mhz for 6m)
                    case FRSRegion.ES_CH_FIN:   // EU12(IARU1 60m) & (52mhz for 6m)
                    case FRSRegion.EU_Travel:   // EU14 (5.25 to 5.45 60m) & (52mhz 6m)
                   

                        for (int i = 0; i <= h_steps + 1; i++)
                        {
                            string label;
                            int offsetL;
                            int offsetR;

                            int fgrid = i * freq_step_size + (low / freq_step_size) * freq_step_size;
                            double actual_fgrid = ((double)(vfo_round + fgrid)) / 1000000;
                            int vgrid = (int)((double)(fgrid - vfo_delta - low) / (high - low) * W);

                            if (!show_freq_offset)
                            {
                                if (actual_fgrid == 1.81 || actual_fgrid == 2.0 ||
                                    actual_fgrid == 3.5 || actual_fgrid == 3.8 ||
                                    actual_fgrid == 5.250 || actual_fgrid == 5.45 ||   // ke9ns add
                                    actual_fgrid == 7.0 || actual_fgrid == 7.2 ||
                                    actual_fgrid == 10.1 || actual_fgrid == 10.15 ||
                                    actual_fgrid == 14.0 || actual_fgrid == 14.35 ||
                                    actual_fgrid == 18.068 || actual_fgrid == 18.168 ||
                                    actual_fgrid == 21.0 || actual_fgrid == 21.45 ||
                                    actual_fgrid == 24.89 || actual_fgrid == 24.99 ||
                                    actual_fgrid == 28.0 || actual_fgrid == 29.7 ||
                                    actual_fgrid == 50.00 || actual_fgrid == 52.0 ||
                                    actual_fgrid == 144.0 || actual_fgrid == 146.0)
                                {
                                    if (bottom) g.DrawLine(new Pen(band_edge_color), vgrid, H + top, vgrid, H + H);
                                    else g.DrawLine(new Pen(band_edge_color), vgrid, top, vgrid, H);

                                    label = actual_fgrid.ToString("f4");
                                    label = label.Replace(",", ".");    // handle Windows localization issues
                                    int offset = label.IndexOf('.') + 4;
                                    label = label.Insert(offset, ".");

                                    if (actual_fgrid < 10) offsetL = (int)((label.Length + 2) * 4.1) - 14;
                                    else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                    else offsetL = (int)((label.Length) * 4.1) - 8;

                                    /* if (actual_fgrid < 10) offsetL = (int)((label.Length + 1) * 4.1) - 14;
                                    else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                    else offsetL = (int)((label.Length + 1) * 4.1) - 8; */

                                    if (bottom) g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid - offsetL, H + (float)Math.Floor(H * .005));
                                    else g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid - offsetL, (float)Math.Floor(H * .005));
                                }
                                else
                                {

                                    if (freq_step_size >= 2000)
                                    {
                                        double t100;
                                        double t1000;
                                        t100 = (actual_fgrid * 100);
                                        t1000 = (actual_fgrid * 1000);

                                        int it100 = (int)t100;
                                        int it1000 = (int)t1000;

                                        int it100x10 = it100 * 10;

                                        if (it100x10 == it1000)
                                        {
                                        }
                                        else
                                        {
                                            grid_pen.DashStyle = DashStyle.Dot;
                                        }
                                    }
                                    else
                                    {
                                        if (freq_step_size == 1000)
                                        {
                                            double t200;
                                            double t2000;
                                            t200 = (actual_fgrid * 200);
                                            t2000 = (actual_fgrid * 2000);

                                            int it200 = (int)t200;
                                            int it2000 = (int)t2000;

                                            int it200x10 = it200 * 10;

                                            if (it200x10 == it2000)
                                            {
                                            }
                                            else
                                            {
                                                grid_pen.DashStyle = DashStyle.Dot;
                                            }
                                        }
                                        else
                                        {
                                            double t1000;
                                            double t10000;
                                            t1000 = (actual_fgrid * 1000);
                                            t10000 = (actual_fgrid * 10000);

                                            int it1000 = (int)t1000;
                                            int it10000 = (int)t10000;

                                            int it1000x10 = it1000 * 10;

                                            if (it1000x10 == it10000)
                                            {
                                            }
                                            else
                                            {
                                                grid_pen.DashStyle = DashStyle.Dot;
                                            }
                                        }
                                    }
                                    if (bottom) g.DrawLine(grid_pen, vgrid, H + top, vgrid, H + H);
                                    else g.DrawLine(grid_pen, vgrid, top, vgrid, H);            //wa6ahl
                                    grid_pen.DashStyle = DashStyle.Solid;

                                    // make freq grid labels
                                    double actual_fgrid_label = Math.Round(actual_fgrid, 4);
                                    label = actual_fgrid_label.ToString("f4");
                                    label = label.Replace(",", ".");    // handle Windows localization issues
                                    int offset = label.IndexOf('.') + 4;
                                    label = label.Insert(offset, ".");

                                    if (actual_fgrid < 10) offsetL = (int)((label.Length + 2) * 4.1) - 14;
                                    else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                    else offsetL = (int)((label.Length) * 4.1) - 8;

                                    if (bottom) g.DrawString(label, font, grid_text_brush, vgrid - offsetL, H + (float)Math.Floor(H * .005));
                                    else g.DrawString(label, font, grid_text_brush, vgrid - offsetL, (float)Math.Floor(H * .005));
                                }
                            }
                            else
                            {
                                vgrid = Convert.ToInt32((double)-(fgrid - low) / (low - high) * W); //wa6ahl
                                if (bottom) g.DrawLine(grid_pen, vgrid, H + top, vgrid, H + H);
                                else g.DrawLine(grid_pen, vgrid, top, vgrid, H);            //wa6ahl

                                double new_fgrid = (vfoa_hz + fgrid) / 1000000;

                                label = fgrid.ToString();
                                offsetL = (int)((label.Length + 1) * 4.1);
                                offsetR = (int)(label.Length * 4.1);
                                if ((vgrid - offsetL >= 0) && (vgrid + offsetR < W) && (fgrid != 0))
                                {
                                    if (bottom) g.DrawString(label, font, grid_text_brush, vgrid - offsetL, H + (float)Math.Floor(H * .005));
                                    else g.DrawString(label, font, grid_text_brush, vgrid - offsetL, (float)Math.Floor(H * .005));
                                }
                            }
                        }


                        int[] band_edge_list_r1a = {  1810000, 2000000, 3500000, 3800000, 5250000,5450000,
                                    7000000, 7200000, 10100000, 10150000, 14000000, 14350000,18068000, 18168000, 21000000, 21450000,
                                    24890000, 24990000, 28000000, 29700000, 50000000, 52000000, 144000000, 146000000 };

                        for (int i = 0; i < band_edge_list_r1a.Length; i++)
                        {
                            double band_edge_offset = band_edge_list_r1a[i] - vfo;
                            if (band_edge_offset >= low && band_edge_offset <= high)
                            {
                                int temp_vline = (int)((double)(band_edge_offset - low) / (high - low) * W);//wa6ahl
                                if (bottom) g.DrawLine(new Pen(band_edge_color), temp_vline, H, temp_vline, H + top);//wa6ahl
                                else g.DrawLine(new Pen(band_edge_color), temp_vline, 0, temp_vline, top);
                            }
                            if (i == 1 && !show_freq_offset) break;
                        }


                        break; // EU01,EU02,EU03,EU06, EU12,EU14




                    //============================================================================================================
                    //============================================================================================================
                    //============================================================================================================
                    //===============================================
                    case FRSRegion.Belgium:     // EU04 (IARU1 60m)

                        for (int i = 0; i <= h_steps + 1; i++)
                        {
                            string label;
                            int offsetL;
                            int offsetR;

                            int fgrid = i * freq_step_size + (low / freq_step_size) * freq_step_size;
                            double actual_fgrid = ((double)(vfo_round + fgrid)) / 1000000;
                            int vgrid = (int)((double)(fgrid - vfo_delta - low) / (high - low) * W);

                            if (!show_freq_offset)
                            {
                                if (actual_fgrid == 1.81 || actual_fgrid == 1.88 ||
                                    actual_fgrid == 3.5 || actual_fgrid == 3.8 ||
                                    actual_fgrid == 5.250 || actual_fgrid == 5.45 ||   // ke9ns add
                                    actual_fgrid == 7.0 || actual_fgrid == 7.2 ||
                                    actual_fgrid == 10.1 || actual_fgrid == 10.15 ||
                                    actual_fgrid == 14.0 || actual_fgrid == 14.35 ||
                                    actual_fgrid == 18.068 || actual_fgrid == 18.168 ||
                                    actual_fgrid == 21.0 || actual_fgrid == 21.45 ||
                                    actual_fgrid == 24.89 || actual_fgrid == 24.99 ||
                                    actual_fgrid == 28.0 || actual_fgrid == 29.7 ||
                                    actual_fgrid == 50.00 || actual_fgrid == 52.0 ||
                                    actual_fgrid == 144.0 || actual_fgrid == 146.0)
                                {
                                    if (bottom) g.DrawLine(new Pen(band_edge_color), vgrid, H + top, vgrid, H + H);
                                    else g.DrawLine(new Pen(band_edge_color), vgrid, top, vgrid, H);

                                    label = actual_fgrid.ToString("f4");
                                    label = label.Replace(",", ".");    // handle Windows localization issues
                                    int offset = label.IndexOf('.') + 4;
                                    label = label.Insert(offset, ".");

                                    if (actual_fgrid < 10) offsetL = (int)((label.Length + 2) * 4.1) - 14;
                                    else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                    else offsetL = (int)((label.Length) * 4.1) - 8;

                                    /* if (actual_fgrid < 10) offsetL = (int)((label.Length + 1) * 4.1) - 14;
                                    else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                    else offsetL = (int)((label.Length + 1) * 4.1) - 8; */

                                    if (bottom) g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid - offsetL, H + (float)Math.Floor(H * .005));
                                    else g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid - offsetL, (float)Math.Floor(H * .005));
                                }
                                else
                                {

                                    if (freq_step_size >= 2000)
                                    {
                                        double t100;
                                        double t1000;
                                        t100 = (actual_fgrid * 100);
                                        t1000 = (actual_fgrid * 1000);

                                        int it100 = (int)t100;
                                        int it1000 = (int)t1000;

                                        int it100x10 = it100 * 10;

                                        if (it100x10 == it1000)
                                        {
                                        }
                                        else
                                        {
                                            grid_pen.DashStyle = DashStyle.Dot;
                                        }
                                    }
                                    else
                                    {
                                        if (freq_step_size == 1000)
                                        {
                                            double t200;
                                            double t2000;
                                            t200 = (actual_fgrid * 200);
                                            t2000 = (actual_fgrid * 2000);

                                            int it200 = (int)t200;
                                            int it2000 = (int)t2000;

                                            int it200x10 = it200 * 10;

                                            if (it200x10 == it2000)
                                            {
                                            }
                                            else
                                            {
                                                grid_pen.DashStyle = DashStyle.Dot;
                                            }
                                        }
                                        else
                                        {
                                            double t1000;
                                            double t10000;
                                            t1000 = (actual_fgrid * 1000);
                                            t10000 = (actual_fgrid * 10000);

                                            int it1000 = (int)t1000;
                                            int it10000 = (int)t10000;

                                            int it1000x10 = it1000 * 10;

                                            if (it1000x10 == it10000)
                                            {
                                            }
                                            else
                                            {
                                                grid_pen.DashStyle = DashStyle.Dot;
                                            }
                                        }
                                    }
                                    if (bottom) g.DrawLine(grid_pen, vgrid, H + top, vgrid, H + H);
                                    else g.DrawLine(grid_pen, vgrid, top, vgrid, H);            //wa6ahl
                                    grid_pen.DashStyle = DashStyle.Solid;

                                    // make freq grid labels
                                    double actual_fgrid_label = Math.Round(actual_fgrid, 4);
                                    label = actual_fgrid_label.ToString("f4");
                                    label = label.Replace(",", ".");    // handle Windows localization issues
                                    int offset = label.IndexOf('.') + 4;
                                    label = label.Insert(offset, ".");

                                    if (actual_fgrid < 10) offsetL = (int)((label.Length + 2) * 4.1) - 14;
                                    else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                    else offsetL = (int)((label.Length) * 4.1) - 8;

                                    if (bottom) g.DrawString(label, font, grid_text_brush, vgrid - offsetL, H + (float)Math.Floor(H * .005));
                                    else g.DrawString(label, font, grid_text_brush, vgrid - offsetL, (float)Math.Floor(H * .005));
                                }
                            }
                            else
                            {
                                vgrid = Convert.ToInt32((double)-(fgrid - low) / (low - high) * W); //wa6ahl
                                if (bottom) g.DrawLine(grid_pen, vgrid, H + top, vgrid, H + H);
                                else g.DrawLine(grid_pen, vgrid, top, vgrid, H);            //wa6ahl

                                double new_fgrid = (vfoa_hz + fgrid) / 1000000;

                                label = fgrid.ToString();
                                offsetL = (int)((label.Length + 1) * 4.1);
                                offsetR = (int)(label.Length * 4.1);
                                if ((vgrid - offsetL >= 0) && (vgrid + offsetR < W) && (fgrid != 0))
                                {
                                    if (bottom) g.DrawString(label, font, grid_text_brush, vgrid - offsetL, H + (float)Math.Floor(H * .005));
                                    else g.DrawString(label, font, grid_text_brush, vgrid - offsetL, (float)Math.Floor(H * .005));
                                }
                            }
                        }


                        int[] band_edge_list_r1b = {  1810000, 1880000, 3500000, 3800000, 5250000,5450000,
                                    7000000, 7200000, 10100000, 10150000, 14000000, 14350000,18068000, 18168000, 21000000, 21450000,
                                    24890000, 24990000, 28000000, 29700000, 50000000, 52000000, 144000000, 146000000 };

                        for (int i = 0; i < band_edge_list_r1b.Length; i++)
                        {
                            double band_edge_offset = band_edge_list_r1b[i] - vfo;
                            if (band_edge_offset >= low && band_edge_offset <= high)
                            {
                                int temp_vline = (int)((double)(band_edge_offset - low) / (high - low) * W);//wa6ahl
                                if (bottom) g.DrawLine(new Pen(band_edge_color), temp_vline, H, temp_vline, H + top);//wa6ahl
                                else g.DrawLine(new Pen(band_edge_color), temp_vline, 0, temp_vline, top);
                            }
                            if (i == 1 && !show_freq_offset) break;
                        }


                        break; // EU04



                    //============================================================================================================
                    //============================================================================================================
                    //============================================================================================================
                    //===============================================
                    case FRSRegion.Hungary:     // EU05 (no 60m)

                        for (int i = 0; i <= h_steps + 1; i++)
                        {
                            string label;
                            int offsetL;
                            int offsetR;

                            int fgrid = i * freq_step_size + (low / freq_step_size) * freq_step_size;
                            double actual_fgrid = ((double)(vfo_round + fgrid)) / 1000000;
                            int vgrid = (int)((double)(fgrid - vfo_delta - low) / (high - low) * W);

                            if (!show_freq_offset)
                            {
                                if (actual_fgrid == 1.81 || actual_fgrid == 2.00 ||
                                    actual_fgrid == 3.5 || actual_fgrid == 3.8 ||
                                    actual_fgrid == 5.250 || actual_fgrid == 5.45 ||   // ke9ns add
                                    actual_fgrid == 7.0 || actual_fgrid == 7.1 ||
                                    actual_fgrid == 10.1 || actual_fgrid == 10.15 ||
                                    actual_fgrid == 14.0 || actual_fgrid == 14.35 ||
                                    actual_fgrid == 18.068 || actual_fgrid == 18.168 ||
                                    actual_fgrid == 21.0 || actual_fgrid == 21.45 ||
                                    actual_fgrid == 24.89 || actual_fgrid == 24.99 ||
                                    actual_fgrid == 28.0 || actual_fgrid == 29.7 ||
                                    actual_fgrid == 50.00 || actual_fgrid == 52.0 ||
                                    actual_fgrid == 144.0 || actual_fgrid == 146.0)
                                {
                                    if (bottom) g.DrawLine(new Pen(band_edge_color), vgrid, H + top, vgrid, H + H);
                                    else g.DrawLine(new Pen(band_edge_color), vgrid, top, vgrid, H);

                                    label = actual_fgrid.ToString("f4");
                                    label = label.Replace(",", ".");    // handle Windows localization issues
                                    int offset = label.IndexOf('.') + 4;
                                    label = label.Insert(offset, ".");

                                    if (actual_fgrid < 10) offsetL = (int)((label.Length + 2) * 4.1) - 14;
                                    else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                    else offsetL = (int)((label.Length) * 4.1) - 8;

                                    /* if (actual_fgrid < 10) offsetL = (int)((label.Length + 1) * 4.1) - 14;
                                    else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                    else offsetL = (int)((label.Length + 1) * 4.1) - 8; */

                                    if (bottom) g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid - offsetL, H + (float)Math.Floor(H * .005));
                                    else g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid - offsetL, (float)Math.Floor(H * .005));
                                }
                                else
                                {

                                    if (freq_step_size >= 2000)
                                    {
                                        double t100;
                                        double t1000;
                                        t100 = (actual_fgrid * 100);
                                        t1000 = (actual_fgrid * 1000);

                                        int it100 = (int)t100;
                                        int it1000 = (int)t1000;

                                        int it100x10 = it100 * 10;

                                        if (it100x10 == it1000)
                                        {
                                        }
                                        else
                                        {
                                            grid_pen.DashStyle = DashStyle.Dot;
                                        }
                                    }
                                    else
                                    {
                                        if (freq_step_size == 1000)
                                        {
                                            double t200;
                                            double t2000;
                                            t200 = (actual_fgrid * 200);
                                            t2000 = (actual_fgrid * 2000);

                                            int it200 = (int)t200;
                                            int it2000 = (int)t2000;

                                            int it200x10 = it200 * 10;

                                            if (it200x10 == it2000)
                                            {
                                            }
                                            else
                                            {
                                                grid_pen.DashStyle = DashStyle.Dot;
                                            }
                                        }
                                        else
                                        {
                                            double t1000;
                                            double t10000;
                                            t1000 = (actual_fgrid * 1000);
                                            t10000 = (actual_fgrid * 10000);

                                            int it1000 = (int)t1000;
                                            int it10000 = (int)t10000;

                                            int it1000x10 = it1000 * 10;

                                            if (it1000x10 == it10000)
                                            {
                                            }
                                            else
                                            {
                                                grid_pen.DashStyle = DashStyle.Dot;
                                            }
                                        }
                                    }
                                    if (bottom) g.DrawLine(grid_pen, vgrid, H + top, vgrid, H + H);
                                    else g.DrawLine(grid_pen, vgrid, top, vgrid, H);            //wa6ahl
                                    grid_pen.DashStyle = DashStyle.Solid;

                                    // make freq grid labels
                                    double actual_fgrid_label = Math.Round(actual_fgrid, 4);
                                    label = actual_fgrid_label.ToString("f4");
                                    label = label.Replace(",", ".");    // handle Windows localization issues
                                    int offset = label.IndexOf('.') + 4;
                                    label = label.Insert(offset, ".");

                                    if (actual_fgrid < 10) offsetL = (int)((label.Length + 2) * 4.1) - 14;
                                    else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                    else offsetL = (int)((label.Length) * 4.1) - 8;

                                    if (bottom) g.DrawString(label, font, grid_text_brush, vgrid - offsetL, H + (float)Math.Floor(H * .005));
                                    else g.DrawString(label, font, grid_text_brush, vgrid - offsetL, (float)Math.Floor(H * .005));
                                }
                            }
                            else
                            {
                                vgrid = Convert.ToInt32((double)-(fgrid - low) / (low - high) * W); //wa6ahl
                                if (bottom) g.DrawLine(grid_pen, vgrid, H + top, vgrid, H + H);
                                else g.DrawLine(grid_pen, vgrid, top, vgrid, H);            //wa6ahl

                                double new_fgrid = (vfoa_hz + fgrid) / 1000000;

                                label = fgrid.ToString();
                                offsetL = (int)((label.Length + 1) * 4.1);
                                offsetR = (int)(label.Length * 4.1);
                                if ((vgrid - offsetL >= 0) && (vgrid + offsetR < W) && (fgrid != 0))
                                {
                                    if (bottom) g.DrawString(label, font, grid_text_brush, vgrid - offsetL, H + (float)Math.Floor(H * .005));
                                    else g.DrawString(label, font, grid_text_brush, vgrid - offsetL, (float)Math.Floor(H * .005));
                                }
                            }
                        }


                        int[] band_edge_list_r1c = {  1810000, 2000000, 3500000, 3800000, 5250000,5450000,
                                    7000000, 7100000, 10100000, 10150000, 14000000, 14350000,18068000, 18168000, 21000000, 21450000,
                                    24890000, 24990000, 28000000, 29700000, 50000000, 52000000, 144000000, 146000000 };

                        for (int i = 0; i < band_edge_list_r1c.Length; i++)
                        {
                            double band_edge_offset = band_edge_list_r1c[i] - vfo;
                            if (band_edge_offset >= low && band_edge_offset <= high)
                            {
                                int temp_vline = (int)((double)(band_edge_offset - low) / (high - low) * W);//wa6ahl
                                if (bottom) g.DrawLine(new Pen(band_edge_color), temp_vline, H, temp_vline, H + top);//wa6ahl
                                else g.DrawLine(new Pen(band_edge_color), temp_vline, 0, temp_vline, top);
                            }
                            if (i == 1 && !show_freq_offset) break;
                        }


                        break; // EU05



                    //============================================================================================================
                    //============================================================================================================
                    //============================================================================================================
                    //===============================================
                    case FRSRegion.Bulgaria:    // EU07 (no 60m)

                        for (int i = 0; i <= h_steps + 1; i++)
                        {
                            string label;
                            int offsetL;
                            int offsetR;

                            int fgrid = i * freq_step_size + (low / freq_step_size) * freq_step_size;
                            double actual_fgrid = ((double)(vfo_round + fgrid)) / 1000000;
                            int vgrid = (int)((double)(fgrid - vfo_delta - low) / (high - low) * W);

                            if (!show_freq_offset)
                            {
                                if (actual_fgrid == 1.81 || actual_fgrid == 1.85 ||
                                    actual_fgrid == 3.5 || actual_fgrid == 3.8 ||
                                    actual_fgrid == 5.250 || actual_fgrid == 5.45 ||   // ke9ns add
                                    actual_fgrid == 7.0 || actual_fgrid == 7.2 ||
                                    actual_fgrid == 10.1 || actual_fgrid == 10.15 ||
                                    actual_fgrid == 14.0 || actual_fgrid == 14.35 ||
                                    actual_fgrid == 18.068 || actual_fgrid == 18.168 ||
                                    actual_fgrid == 21.0 || actual_fgrid == 21.45 ||
                                    actual_fgrid == 24.89 || actual_fgrid == 24.99 ||
                                    actual_fgrid == 28.0 || actual_fgrid == 29.7 ||
                                    actual_fgrid == 50.05 || actual_fgrid == 50.2 ||
                                    actual_fgrid == 144.0 || actual_fgrid == 146.0)
                                {
                                    if (bottom) g.DrawLine(new Pen(band_edge_color), vgrid, H + top, vgrid, H + H);
                                    else g.DrawLine(new Pen(band_edge_color), vgrid, top, vgrid, H);

                                    label = actual_fgrid.ToString("f4");
                                    label = label.Replace(",", ".");    // handle Windows localization issues
                                    int offset = label.IndexOf('.') + 4;
                                    label = label.Insert(offset, ".");

                                    if (actual_fgrid < 10) offsetL = (int)((label.Length + 2) * 4.1) - 14;
                                    else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                    else offsetL = (int)((label.Length) * 4.1) - 8;

                                    /* if (actual_fgrid < 10) offsetL = (int)((label.Length + 1) * 4.1) - 14;
                                    else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                    else offsetL = (int)((label.Length + 1) * 4.1) - 8; */

                                    if (bottom) g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid - offsetL, H + (float)Math.Floor(H * .005));
                                    else g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid - offsetL, (float)Math.Floor(H * .005));
                                }
                                else
                                {

                                    if (freq_step_size >= 2000)
                                    {
                                        double t100;
                                        double t1000;
                                        t100 = (actual_fgrid * 100);
                                        t1000 = (actual_fgrid * 1000);

                                        int it100 = (int)t100;
                                        int it1000 = (int)t1000;

                                        int it100x10 = it100 * 10;

                                        if (it100x10 == it1000)
                                        {
                                        }
                                        else
                                        {
                                            grid_pen.DashStyle = DashStyle.Dot;
                                        }
                                    }
                                    else
                                    {
                                        if (freq_step_size == 1000)
                                        {
                                            double t200;
                                            double t2000;
                                            t200 = (actual_fgrid * 200);
                                            t2000 = (actual_fgrid * 2000);

                                            int it200 = (int)t200;
                                            int it2000 = (int)t2000;

                                            int it200x10 = it200 * 10;

                                            if (it200x10 == it2000)
                                            {
                                            }
                                            else
                                            {
                                                grid_pen.DashStyle = DashStyle.Dot;
                                            }
                                        }
                                        else
                                        {
                                            double t1000;
                                            double t10000;
                                            t1000 = (actual_fgrid * 1000);
                                            t10000 = (actual_fgrid * 10000);

                                            int it1000 = (int)t1000;
                                            int it10000 = (int)t10000;

                                            int it1000x10 = it1000 * 10;

                                            if (it1000x10 == it10000)
                                            {
                                            }
                                            else
                                            {
                                                grid_pen.DashStyle = DashStyle.Dot;
                                            }
                                        }
                                    }
                                    if (bottom) g.DrawLine(grid_pen, vgrid, H + top, vgrid, H + H);
                                    else g.DrawLine(grid_pen, vgrid, top, vgrid, H);            //wa6ahl
                                    grid_pen.DashStyle = DashStyle.Solid;

                                    // make freq grid labels
                                    double actual_fgrid_label = Math.Round(actual_fgrid, 4);
                                    label = actual_fgrid_label.ToString("f4");
                                    label = label.Replace(",", ".");    // handle Windows localization issues
                                    int offset = label.IndexOf('.') + 4;
                                    label = label.Insert(offset, ".");

                                    if (actual_fgrid < 10) offsetL = (int)((label.Length + 2) * 4.1) - 14;
                                    else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                    else offsetL = (int)((label.Length) * 4.1) - 8;

                                    if (bottom) g.DrawString(label, font, grid_text_brush, vgrid - offsetL, H + (float)Math.Floor(H * .005));
                                    else g.DrawString(label, font, grid_text_brush, vgrid - offsetL, (float)Math.Floor(H * .005));
                                }
                            }
                            else
                            {
                                vgrid = Convert.ToInt32((double)-(fgrid - low) / (low - high) * W); //wa6ahl
                                if (bottom) g.DrawLine(grid_pen, vgrid, H + top, vgrid, H + H);
                                else g.DrawLine(grid_pen, vgrid, top, vgrid, H);            //wa6ahl

                                double new_fgrid = (vfoa_hz + fgrid) / 1000000;

                                label = fgrid.ToString();
                                offsetL = (int)((label.Length + 1) * 4.1);
                                offsetR = (int)(label.Length * 4.1);
                                if ((vgrid - offsetL >= 0) && (vgrid + offsetR < W) && (fgrid != 0))
                                {
                                    if (bottom) g.DrawString(label, font, grid_text_brush, vgrid - offsetL, H + (float)Math.Floor(H * .005));
                                    else g.DrawString(label, font, grid_text_brush, vgrid - offsetL, (float)Math.Floor(H * .005));
                                }
                            }
                        }


                        int[] band_edge_list_r1d = {  1810000, 1850000, 3500000, 3800000, 5250000,5450000,
                                    7000000, 7200000, 10100000, 10150000, 14000000, 14350000,18068000, 18168000, 21000000, 21450000,
                                    24890000, 24990000, 28000000, 29700000, 50050000, 50200000, 144000000, 146000000 };

                        for (int i = 0; i < band_edge_list_r1d.Length; i++)
                        {
                            double band_edge_offset = band_edge_list_r1d[i] - vfo;
                            if (band_edge_offset >= low && band_edge_offset <= high)
                            {
                                int temp_vline = (int)((double)(band_edge_offset - low) / (high - low) * W);//wa6ahl
                                if (bottom) g.DrawLine(new Pen(band_edge_color), temp_vline, H, temp_vline, H + top);//wa6ahl
                                else g.DrawLine(new Pen(band_edge_color), temp_vline, 0, temp_vline, top);
                            }
                            if (i == 1 && !show_freq_offset) break;
                        }


                        break; // EU07


                    //============================================================================================================
                    //============================================================================================================
                    //============================================================================================================
                    //===============================================
                    case FRSRegion.Latvia:      // EU08 (no 60m)

                        for (int i = 0; i <= h_steps + 1; i++)
                        {
                            string label;
                            int offsetL;
                            int offsetR;

                            int fgrid = i * freq_step_size + (low / freq_step_size) * freq_step_size;
                            double actual_fgrid = ((double)(vfo_round + fgrid)) / 1000000;
                            int vgrid = (int)((double)(fgrid - vfo_delta - low) / (high - low) * W);

                            if (!show_freq_offset)
                            {
                                if (actual_fgrid == 1.81 || actual_fgrid == 2.0 ||
                                    actual_fgrid == 3.5 || actual_fgrid == 3.8 ||
                                    actual_fgrid == 5.250 || actual_fgrid == 5.45 ||   // ke9ns add
                                    actual_fgrid == 7.0 || actual_fgrid == 7.2 ||
                                    actual_fgrid == 10.1 || actual_fgrid == 10.15 ||
                                    actual_fgrid == 14.0 || actual_fgrid == 14.35 ||
                                    actual_fgrid == 18.068 || actual_fgrid == 18.168 ||
                                    actual_fgrid == 21.0 || actual_fgrid == 21.45 ||
                                    actual_fgrid == 24.89 || actual_fgrid == 24.99 ||
                                    actual_fgrid == 28.0 || actual_fgrid == 29.7 ||
                                    actual_fgrid == 50.0 || actual_fgrid == 51.0 ||
                                    actual_fgrid == 144.0 || actual_fgrid == 146.0)
                                {
                                    if (bottom) g.DrawLine(new Pen(band_edge_color), vgrid, H + top, vgrid, H + H);
                                    else g.DrawLine(new Pen(band_edge_color), vgrid, top, vgrid, H);

                                    label = actual_fgrid.ToString("f4");
                                    label = label.Replace(",", ".");    // handle Windows localization issues
                                    int offset = label.IndexOf('.') + 4;
                                    label = label.Insert(offset, ".");

                                    if (actual_fgrid < 10) offsetL = (int)((label.Length + 2) * 4.1) - 14;
                                    else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                    else offsetL = (int)((label.Length) * 4.1) - 8;

                                    /* if (actual_fgrid < 10) offsetL = (int)((label.Length + 1) * 4.1) - 14;
                                    else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                    else offsetL = (int)((label.Length + 1) * 4.1) - 8; */

                                    if (bottom) g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid - offsetL, H + (float)Math.Floor(H * .005));
                                    else g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid - offsetL, (float)Math.Floor(H * .005));
                                }
                                else
                                {

                                    if (freq_step_size >= 2000)
                                    {
                                        double t100;
                                        double t1000;
                                        t100 = (actual_fgrid * 100);
                                        t1000 = (actual_fgrid * 1000);

                                        int it100 = (int)t100;
                                        int it1000 = (int)t1000;

                                        int it100x10 = it100 * 10;

                                        if (it100x10 == it1000)
                                        {
                                        }
                                        else
                                        {
                                            grid_pen.DashStyle = DashStyle.Dot;
                                        }
                                    }
                                    else
                                    {
                                        if (freq_step_size == 1000)
                                        {
                                            double t200;
                                            double t2000;
                                            t200 = (actual_fgrid * 200);
                                            t2000 = (actual_fgrid * 2000);

                                            int it200 = (int)t200;
                                            int it2000 = (int)t2000;

                                            int it200x10 = it200 * 10;

                                            if (it200x10 == it2000)
                                            {
                                            }
                                            else
                                            {
                                                grid_pen.DashStyle = DashStyle.Dot;
                                            }
                                        }
                                        else
                                        {
                                            double t1000;
                                            double t10000;
                                            t1000 = (actual_fgrid * 1000);
                                            t10000 = (actual_fgrid * 10000);

                                            int it1000 = (int)t1000;
                                            int it10000 = (int)t10000;

                                            int it1000x10 = it1000 * 10;

                                            if (it1000x10 == it10000)
                                            {
                                            }
                                            else
                                            {
                                                grid_pen.DashStyle = DashStyle.Dot;
                                            }
                                        }
                                    }
                                    if (bottom) g.DrawLine(grid_pen, vgrid, H + top, vgrid, H + H);
                                    else g.DrawLine(grid_pen, vgrid, top, vgrid, H);            //wa6ahl
                                    grid_pen.DashStyle = DashStyle.Solid;

                                    // make freq grid labels
                                    double actual_fgrid_label = Math.Round(actual_fgrid, 4);
                                    label = actual_fgrid_label.ToString("f4");
                                    label = label.Replace(",", ".");    // handle Windows localization issues
                                    int offset = label.IndexOf('.') + 4;
                                    label = label.Insert(offset, ".");

                                    if (actual_fgrid < 10) offsetL = (int)((label.Length + 2) * 4.1) - 14;
                                    else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                    else offsetL = (int)((label.Length) * 4.1) - 8;

                                    if (bottom) g.DrawString(label, font, grid_text_brush, vgrid - offsetL, H + (float)Math.Floor(H * .005));
                                    else g.DrawString(label, font, grid_text_brush, vgrid - offsetL, (float)Math.Floor(H * .005));
                                }
                            }
                            else
                            {
                                vgrid = Convert.ToInt32((double)-(fgrid - low) / (low - high) * W); //wa6ahl
                                if (bottom) g.DrawLine(grid_pen, vgrid, H + top, vgrid, H + H);
                                else g.DrawLine(grid_pen, vgrid, top, vgrid, H);            //wa6ahl

                                double new_fgrid = (vfoa_hz + fgrid) / 1000000;

                                label = fgrid.ToString();
                                offsetL = (int)((label.Length + 1) * 4.1);
                                offsetR = (int)(label.Length * 4.1);
                                if ((vgrid - offsetL >= 0) && (vgrid + offsetR < W) && (fgrid != 0))
                                {
                                    if (bottom) g.DrawString(label, font, grid_text_brush, vgrid - offsetL, H + (float)Math.Floor(H * .005));
                                    else g.DrawString(label, font, grid_text_brush, vgrid - offsetL, (float)Math.Floor(H * .005));
                                }
                            }
                        }


                        int[] band_edge_list_r1e = {  1810000, 2000000, 3500000, 3800000, 5250000,5450000,
                                    7000000, 7200000, 10100000, 10150000, 14000000, 14350000,18068000, 18168000, 21000000, 21450000,
                                    24890000, 24990000, 28000000, 29700000, 50000000, 51000000, 144000000, 146000000 };

                        for (int i = 0; i < band_edge_list_r1e.Length; i++)
                        {
                            double band_edge_offset = band_edge_list_r1e[i] - vfo;
                            if (band_edge_offset >= low && band_edge_offset <= high)
                            {
                                int temp_vline = (int)((double)(band_edge_offset - low) / (high - low) * W);//wa6ahl
                                if (bottom) g.DrawLine(new Pen(band_edge_color), temp_vline, H, temp_vline, H + top);//wa6ahl
                                else g.DrawLine(new Pen(band_edge_color), temp_vline, 0, temp_vline, top);
                            }
                            if (i == 1 && !show_freq_offset) break;
                        }


                        break; // EU08


                    //============================================================================================================
                    //============================================================================================================
                    //============================================================================================================
                    //===============================================
                    case FRSRegion.Greece:      // EU09 (no 60m)

                        for (int i = 0; i <= h_steps + 1; i++)
                        {
                            string label;
                            int offsetL;
                            int offsetR;

                            int fgrid = i * freq_step_size + (low / freq_step_size) * freq_step_size;
                            double actual_fgrid = ((double)(vfo_round + fgrid)) / 1000000;
                            int vgrid = (int)((double)(fgrid - vfo_delta - low) / (high - low) * W);

                            if (!show_freq_offset)
                            {
                                if (actual_fgrid == 1.81 || actual_fgrid == 1.85 ||
                                    actual_fgrid == 3.5 || actual_fgrid == 3.8 ||
                                    actual_fgrid == 5.250 || actual_fgrid == 5.45 ||   // ke9ns add
                                    actual_fgrid == 7.0 || actual_fgrid == 7.2 ||
                                    actual_fgrid == 10.1 || actual_fgrid == 10.15 ||
                                    actual_fgrid == 14.0 || actual_fgrid == 14.35 ||
                                    actual_fgrid == 18.068 || actual_fgrid == 18.168 ||
                                    actual_fgrid == 21.0 || actual_fgrid == 21.45 ||
                                    actual_fgrid == 24.89 || actual_fgrid == 24.99 ||
                                    actual_fgrid == 28.0 || actual_fgrid == 29.7 ||
                                    actual_fgrid == 50.0 || actual_fgrid == 52.0 ||
                                    actual_fgrid == 144.0 || actual_fgrid == 146.0)
                                {
                                    if (bottom) g.DrawLine(new Pen(band_edge_color), vgrid, H + top, vgrid, H + H);
                                    else g.DrawLine(new Pen(band_edge_color), vgrid, top, vgrid, H);

                                    label = actual_fgrid.ToString("f4");
                                    label = label.Replace(",", ".");    // handle Windows localization issues
                                    int offset = label.IndexOf('.') + 4;
                                    label = label.Insert(offset, ".");

                                    if (actual_fgrid < 10) offsetL = (int)((label.Length + 2) * 4.1) - 14;
                                    else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                    else offsetL = (int)((label.Length) * 4.1) - 8;

                                    /* if (actual_fgrid < 10) offsetL = (int)((label.Length + 1) * 4.1) - 14;
                                    else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                    else offsetL = (int)((label.Length + 1) * 4.1) - 8; */

                                    if (bottom) g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid - offsetL, H + (float)Math.Floor(H * .005));
                                    else g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid - offsetL, (float)Math.Floor(H * .005));
                                }
                                else
                                {

                                    if (freq_step_size >= 2000)
                                    {
                                        double t100;
                                        double t1000;
                                        t100 = (actual_fgrid * 100);
                                        t1000 = (actual_fgrid * 1000);

                                        int it100 = (int)t100;
                                        int it1000 = (int)t1000;

                                        int it100x10 = it100 * 10;

                                        if (it100x10 == it1000)
                                        {
                                        }
                                        else
                                        {
                                            grid_pen.DashStyle = DashStyle.Dot;
                                        }
                                    }
                                    else
                                    {
                                        if (freq_step_size == 1000)
                                        {
                                            double t200;
                                            double t2000;
                                            t200 = (actual_fgrid * 200);
                                            t2000 = (actual_fgrid * 2000);

                                            int it200 = (int)t200;
                                            int it2000 = (int)t2000;

                                            int it200x10 = it200 * 10;

                                            if (it200x10 == it2000)
                                            {
                                            }
                                            else
                                            {
                                                grid_pen.DashStyle = DashStyle.Dot;
                                            }
                                        }
                                        else
                                        {
                                            double t1000;
                                            double t10000;
                                            t1000 = (actual_fgrid * 1000);
                                            t10000 = (actual_fgrid * 10000);

                                            int it1000 = (int)t1000;
                                            int it10000 = (int)t10000;

                                            int it1000x10 = it1000 * 10;

                                            if (it1000x10 == it10000)
                                            {
                                            }
                                            else
                                            {
                                                grid_pen.DashStyle = DashStyle.Dot;
                                            }
                                        }
                                    }
                                    if (bottom) g.DrawLine(grid_pen, vgrid, H + top, vgrid, H + H);
                                    else g.DrawLine(grid_pen, vgrid, top, vgrid, H);            //wa6ahl
                                    grid_pen.DashStyle = DashStyle.Solid;

                                    // make freq grid labels
                                    double actual_fgrid_label = Math.Round(actual_fgrid, 4);
                                    label = actual_fgrid_label.ToString("f4");
                                    label = label.Replace(",", ".");    // handle Windows localization issues
                                    int offset = label.IndexOf('.') + 4;
                                    label = label.Insert(offset, ".");

                                    if (actual_fgrid < 10) offsetL = (int)((label.Length + 2) * 4.1) - 14;
                                    else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                    else offsetL = (int)((label.Length) * 4.1) - 8;

                                    if (bottom) g.DrawString(label, font, grid_text_brush, vgrid - offsetL, H + (float)Math.Floor(H * .005));
                                    else g.DrawString(label, font, grid_text_brush, vgrid - offsetL, (float)Math.Floor(H * .005));
                                }
                            }
                            else
                            {
                                vgrid = Convert.ToInt32((double)-(fgrid - low) / (low - high) * W); //wa6ahl
                                if (bottom) g.DrawLine(grid_pen, vgrid, H + top, vgrid, H + H);
                                else g.DrawLine(grid_pen, vgrid, top, vgrid, H);            //wa6ahl

                                double new_fgrid = (vfoa_hz + fgrid) / 1000000;

                                label = fgrid.ToString();
                                offsetL = (int)((label.Length + 1) * 4.1);
                                offsetR = (int)(label.Length * 4.1);
                                if ((vgrid - offsetL >= 0) && (vgrid + offsetR < W) && (fgrid != 0))
                                {
                                    if (bottom) g.DrawString(label, font, grid_text_brush, vgrid - offsetL, H + (float)Math.Floor(H * .005));
                                    else g.DrawString(label, font, grid_text_brush, vgrid - offsetL, (float)Math.Floor(H * .005));
                                }
                            }
                        }


                        int[] band_edge_list_r1f = {  1810000, 1850000, 3500000, 3800000, 5250000,5450000,
                                    7000000, 7200000, 10100000, 10150000, 14000000, 14350000,18068000, 18168000, 21000000, 21450000,
                                    24890000, 24990000, 28000000, 29700000, 50000000, 52000000, 144000000, 146000000 };

                        for (int i = 0; i < band_edge_list_r1f.Length; i++)
                        {
                            double band_edge_offset = band_edge_list_r1f[i] - vfo;
                            if (band_edge_offset >= low && band_edge_offset <= high)
                            {
                                int temp_vline = (int)((double)(band_edge_offset - low) / (high - low) * W);//wa6ahl
                                if (bottom) g.DrawLine(new Pen(band_edge_color), temp_vline, H, temp_vline, H + top);//wa6ahl
                                else g.DrawLine(new Pen(band_edge_color), temp_vline, 0, temp_vline, top);
                            }
                            if (i == 1 && !show_freq_offset) break;
                        }


                        break; // EU09



                    //============================================================================================================
                    //============================================================================================================
                    //============================================================================================================
                    //===============================================
                    case FRSRegion.Italy:       // EU10 (no 60m)
                   
                        for (int i = 0; i <= h_steps + 1; i++)
                        {
                            string label;
                            int offsetL;
                            int offsetR;

                            int fgrid = i * freq_step_size + (low / freq_step_size) * freq_step_size;
                            double actual_fgrid = ((double)(vfo_round + fgrid)) / 1000000;
                            int vgrid = (int)((double)(fgrid - vfo_delta - low) / (high - low) * W);

                            if (!show_freq_offset)
                            {
                                if (actual_fgrid == 1.83 || actual_fgrid == 1.85 ||
                                    actual_fgrid == 3.5 || actual_fgrid == 3.8 ||
                                    actual_fgrid == 5.250 || actual_fgrid == 5.45 ||   // ke9ns add
                                    actual_fgrid == 7.0 || actual_fgrid == 7.2 ||
                                    actual_fgrid == 10.1 || actual_fgrid == 10.15 ||
                                    actual_fgrid == 14.0 || actual_fgrid == 14.35 ||
                                    actual_fgrid == 18.068 || actual_fgrid == 18.168 ||
                                    actual_fgrid == 21.0 || actual_fgrid == 21.45 ||
                                    actual_fgrid == 24.89 || actual_fgrid == 24.99 ||
                                    actual_fgrid == 28.0 || actual_fgrid == 29.7 ||
                                    actual_fgrid == 50.0 || actual_fgrid == 51.0 ||
                                    actual_fgrid == 144.0 || actual_fgrid == 146.0)
                                {
                                    if (bottom) g.DrawLine(new Pen(band_edge_color), vgrid, H + top, vgrid, H + H);
                                    else g.DrawLine(new Pen(band_edge_color), vgrid, top, vgrid, H);

                                    label = actual_fgrid.ToString("f4");
                                    label = label.Replace(",", ".");    // handle Windows localization issues
                                    int offset = label.IndexOf('.') + 4;
                                    label = label.Insert(offset, ".");

                                    if (actual_fgrid < 10) offsetL = (int)((label.Length + 2) * 4.1) - 14;
                                    else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                    else offsetL = (int)((label.Length) * 4.1) - 8;

                                    /* if (actual_fgrid < 10) offsetL = (int)((label.Length + 1) * 4.1) - 14;
                                    else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                    else offsetL = (int)((label.Length + 1) * 4.1) - 8; */

                                    if (bottom) g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid - offsetL, H + (float)Math.Floor(H * .005));
                                    else g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid - offsetL, (float)Math.Floor(H * .005));
                                }
                                else
                                {

                                    if (freq_step_size >= 2000)
                                    {
                                        double t100;
                                        double t1000;
                                        t100 = (actual_fgrid * 100);
                                        t1000 = (actual_fgrid * 1000);

                                        int it100 = (int)t100;
                                        int it1000 = (int)t1000;

                                        int it100x10 = it100 * 10;

                                        if (it100x10 == it1000)
                                        {
                                        }
                                        else
                                        {
                                            grid_pen.DashStyle = DashStyle.Dot;
                                        }
                                    }
                                    else
                                    {
                                        if (freq_step_size == 1000)
                                        {
                                            double t200;
                                            double t2000;
                                            t200 = (actual_fgrid * 200);
                                            t2000 = (actual_fgrid * 2000);

                                            int it200 = (int)t200;
                                            int it2000 = (int)t2000;

                                            int it200x10 = it200 * 10;

                                            if (it200x10 == it2000)
                                            {
                                            }
                                            else
                                            {
                                                grid_pen.DashStyle = DashStyle.Dot;
                                            }
                                        }
                                        else
                                        {
                                            double t1000;
                                            double t10000;
                                            t1000 = (actual_fgrid * 1000);
                                            t10000 = (actual_fgrid * 10000);

                                            int it1000 = (int)t1000;
                                            int it10000 = (int)t10000;

                                            int it1000x10 = it1000 * 10;

                                            if (it1000x10 == it10000)
                                            {
                                            }
                                            else
                                            {
                                                grid_pen.DashStyle = DashStyle.Dot;
                                            }
                                        }
                                    }
                                    if (bottom) g.DrawLine(grid_pen, vgrid, H + top, vgrid, H + H);
                                    else g.DrawLine(grid_pen, vgrid, top, vgrid, H);            //wa6ahl
                                    grid_pen.DashStyle = DashStyle.Solid;

                                    // make freq grid labels
                                    double actual_fgrid_label = Math.Round(actual_fgrid, 4);
                                    label = actual_fgrid_label.ToString("f4");
                                    label = label.Replace(",", ".");    // handle Windows localization issues
                                    int offset = label.IndexOf('.') + 4;
                                    label = label.Insert(offset, ".");

                                    if (actual_fgrid < 10) offsetL = (int)((label.Length + 2) * 4.1) - 14;
                                    else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                    else offsetL = (int)((label.Length) * 4.1) - 8;

                                    if (bottom) g.DrawString(label, font, grid_text_brush, vgrid - offsetL, H + (float)Math.Floor(H * .005));
                                    else g.DrawString(label, font, grid_text_brush, vgrid - offsetL, (float)Math.Floor(H * .005));
                                }
                            }
                            else
                            {
                                vgrid = Convert.ToInt32((double)-(fgrid - low) / (low - high) * W); //wa6ahl
                                if (bottom) g.DrawLine(grid_pen, vgrid, H + top, vgrid, H + H);
                                else g.DrawLine(grid_pen, vgrid, top, vgrid, H);            //wa6ahl

                                double new_fgrid = (vfoa_hz + fgrid) / 1000000;

                                label = fgrid.ToString();
                                offsetL = (int)((label.Length + 1) * 4.1);
                                offsetR = (int)(label.Length * 4.1);
                                if ((vgrid - offsetL >= 0) && (vgrid + offsetR < W) && (fgrid != 0))
                                {
                                    if (bottom) g.DrawString(label, font, grid_text_brush, vgrid - offsetL, H + (float)Math.Floor(H * .005));
                                    else g.DrawString(label, font, grid_text_brush, vgrid - offsetL, (float)Math.Floor(H * .005));
                                }
                            }
                        }


                        int[] band_edge_list_r1g = {  1830000, 1850000, 3500000, 3800000, 5250000,5450000,
                                    7000000, 7200000, 10100000, 10150000, 14000000, 14350000,18068000, 18168000, 21000000, 21450000,
                                    24890000, 24990000, 28000000, 29700000, 50000000, 51000000, 144000000, 146000000 };

                        for (int i = 0; i < band_edge_list_r1g.Length; i++)
                        {
                            double band_edge_offset = band_edge_list_r1g[i] - vfo;
                            if (band_edge_offset >= low && band_edge_offset <= high)
                            {
                                int temp_vline = (int)((double)(band_edge_offset - low) / (high - low) * W);//wa6ahl
                                if (bottom) g.DrawLine(new Pen(band_edge_color), temp_vline, H, temp_vline, H + top);//wa6ahl
                                else g.DrawLine(new Pen(band_edge_color), temp_vline, 0, temp_vline, top);
                            }
                            if (i == 1 && !show_freq_offset) break;
                        }


                        break; // EU10



                    //============================================================================================================
                    //============================================================================================================
                    //============================================================================================================
                    //===============================================
                    case FRSRegion.Italy_Plus:  // EU11 (no 60m)

                        for (int i = 0; i <= h_steps + 1; i++)
                        {
                            string label;
                            int offsetL;
                            int offsetR;

                            int fgrid = i * freq_step_size + (low / freq_step_size) * freq_step_size;
                            double actual_fgrid = ((double)(vfo_round + fgrid)) / 1000000;
                            int vgrid = (int)((double)(fgrid - vfo_delta - low) / (high - low) * W);

                            if (!show_freq_offset)
                            {
                                if (actual_fgrid == 1.83 || actual_fgrid == 1.85 ||
                                    actual_fgrid == 3.5 || actual_fgrid == 3.8 ||
                                    actual_fgrid == 5.250 || actual_fgrid == 5.45 ||   // ke9ns add
                                    actual_fgrid == 6.975 || actual_fgrid == 7.2 ||
                                    actual_fgrid == 10.1 || actual_fgrid == 10.15 ||
                                    actual_fgrid == 14.0 || actual_fgrid == 14.35 ||
                                    actual_fgrid == 18.068 || actual_fgrid == 18.168 ||
                                    actual_fgrid == 21.0 || actual_fgrid == 21.45 ||
                                    actual_fgrid == 24.89 || actual_fgrid == 24.99 ||
                                    actual_fgrid == 28.0 || actual_fgrid == 29.7 ||
                                    actual_fgrid == 50.0 || actual_fgrid == 51.0 ||
                                    actual_fgrid == 144.0 || actual_fgrid == 146.0)
                                {
                                    if (bottom) g.DrawLine(new Pen(band_edge_color), vgrid, H + top, vgrid, H + H);
                                    else g.DrawLine(new Pen(band_edge_color), vgrid, top, vgrid, H);

                                    label = actual_fgrid.ToString("f4");
                                    label = label.Replace(",", ".");    // handle Windows localization issues
                                    int offset = label.IndexOf('.') + 4;
                                    label = label.Insert(offset, ".");

                                    if (actual_fgrid < 10) offsetL = (int)((label.Length + 2) * 4.1) - 14;
                                    else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                    else offsetL = (int)((label.Length) * 4.1) - 8;

                                    /* if (actual_fgrid < 10) offsetL = (int)((label.Length + 1) * 4.1) - 14;
                                    else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                    else offsetL = (int)((label.Length + 1) * 4.1) - 8; */

                                    if (bottom) g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid - offsetL, H + (float)Math.Floor(H * .005));
                                    else g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid - offsetL, (float)Math.Floor(H * .005));
                                }
                                else
                                {

                                    if (freq_step_size >= 2000)
                                    {
                                        double t100;
                                        double t1000;
                                        t100 = (actual_fgrid * 100);
                                        t1000 = (actual_fgrid * 1000);

                                        int it100 = (int)t100;
                                        int it1000 = (int)t1000;

                                        int it100x10 = it100 * 10;

                                        if (it100x10 == it1000)
                                        {
                                        }
                                        else
                                        {
                                            grid_pen.DashStyle = DashStyle.Dot;
                                        }
                                    }
                                    else
                                    {
                                        if (freq_step_size == 1000)
                                        {
                                            double t200;
                                            double t2000;
                                            t200 = (actual_fgrid * 200);
                                            t2000 = (actual_fgrid * 2000);

                                            int it200 = (int)t200;
                                            int it2000 = (int)t2000;

                                            int it200x10 = it200 * 10;

                                            if (it200x10 == it2000)
                                            {
                                            }
                                            else
                                            {
                                                grid_pen.DashStyle = DashStyle.Dot;
                                            }
                                        }
                                        else
                                        {
                                            double t1000;
                                            double t10000;
                                            t1000 = (actual_fgrid * 1000);
                                            t10000 = (actual_fgrid * 10000);

                                            int it1000 = (int)t1000;
                                            int it10000 = (int)t10000;

                                            int it1000x10 = it1000 * 10;

                                            if (it1000x10 == it10000)
                                            {
                                            }
                                            else
                                            {
                                                grid_pen.DashStyle = DashStyle.Dot;
                                            }
                                        }
                                    }
                                    if (bottom) g.DrawLine(grid_pen, vgrid, H + top, vgrid, H + H);
                                    else g.DrawLine(grid_pen, vgrid, top, vgrid, H);            //wa6ahl
                                    grid_pen.DashStyle = DashStyle.Solid;

                                    // make freq grid labels
                                    double actual_fgrid_label = Math.Round(actual_fgrid, 4);
                                    label = actual_fgrid_label.ToString("f4");
                                    label = label.Replace(",", ".");    // handle Windows localization issues
                                    int offset = label.IndexOf('.') + 4;
                                    label = label.Insert(offset, ".");

                                    if (actual_fgrid < 10) offsetL = (int)((label.Length + 2) * 4.1) - 14;
                                    else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                    else offsetL = (int)((label.Length) * 4.1) - 8;

                                    if (bottom) g.DrawString(label, font, grid_text_brush, vgrid - offsetL, H + (float)Math.Floor(H * .005));
                                    else g.DrawString(label, font, grid_text_brush, vgrid - offsetL, (float)Math.Floor(H * .005));
                                }
                            }
                            else
                            {
                                vgrid = Convert.ToInt32((double)-(fgrid - low) / (low - high) * W); //wa6ahl
                                if (bottom) g.DrawLine(grid_pen, vgrid, H + top, vgrid, H + H);
                                else g.DrawLine(grid_pen, vgrid, top, vgrid, H);            //wa6ahl

                                double new_fgrid = (vfoa_hz + fgrid) / 1000000;

                                label = fgrid.ToString();
                                offsetL = (int)((label.Length + 1) * 4.1);
                                offsetR = (int)(label.Length * 4.1);
                                if ((vgrid - offsetL >= 0) && (vgrid + offsetR < W) && (fgrid != 0))
                                {
                                    if (bottom) g.DrawString(label, font, grid_text_brush, vgrid - offsetL, H + (float)Math.Floor(H * .005));
                                    else g.DrawString(label, font, grid_text_brush, vgrid - offsetL, (float)Math.Floor(H * .005));
                                }
                            }
                        }


                        int[] band_edge_list_r1j = {  1830000, 1850000, 3500000, 3800000, 5250000,5450000,
                                    6975000, 7200000, 10100000, 10150000, 14000000, 14350000,18068000, 18168000, 21000000, 21450000,
                                    24890000, 24990000, 28000000, 29700000, 50000000, 52000000, 144000000, 146000000 };

                        for (int i = 0; i < band_edge_list_r1j.Length; i++)
                        {
                            double band_edge_offset = band_edge_list_r1j[i] - vfo;
                            if (band_edge_offset >= low && band_edge_offset <= high)
                            {
                                int temp_vline = (int)((double)(band_edge_offset - low) / (high - low) * W);//wa6ahl
                                if (bottom) g.DrawLine(new Pen(band_edge_color), temp_vline, H, temp_vline, H + top);//wa6ahl
                                else g.DrawLine(new Pen(band_edge_color), temp_vline, 0, temp_vline, top);
                            }
                            if (i == 1 && !show_freq_offset) break;
                        }


                        break; // EU11


                    //============================================================================================================
                    //============================================================================================================
                    //============================================================================================================
                    //===============================================
                    case FRSRegion.Netherlands: // EU13 (5.35 to 5.45 60m)
                    
                        for (int i = 0; i <= h_steps + 1; i++)
                        {
                            string label;
                            int offsetL;
                            int offsetR;

                            int fgrid = i * freq_step_size + (low / freq_step_size) * freq_step_size;
                            double actual_fgrid = ((double)(vfo_round + fgrid)) / 1000000;
                            int vgrid = (int)((double)(fgrid - vfo_delta - low) / (high - low) * W);

                            if (!show_freq_offset)
                            {
                                if (actual_fgrid == 1.81 || actual_fgrid == 1.88 ||
                                    actual_fgrid == 3.5 || actual_fgrid == 3.8 ||
                                    actual_fgrid == 5.350 || actual_fgrid == 5.45 ||   // ke9ns add
                                    actual_fgrid == 7.0 || actual_fgrid == 7.2 ||
                                    actual_fgrid == 10.1 || actual_fgrid == 10.15 ||
                                    actual_fgrid == 14.0 || actual_fgrid == 14.35 ||
                                    actual_fgrid == 18.068 || actual_fgrid == 18.168 ||
                                    actual_fgrid == 21.0 || actual_fgrid == 21.45 ||
                                    actual_fgrid == 24.89 || actual_fgrid == 24.99 ||
                                    actual_fgrid == 28.0 || actual_fgrid == 29.7 ||
                                    actual_fgrid == 50.0 || actual_fgrid == 52.0 ||
                                    actual_fgrid == 144.0 || actual_fgrid == 146.0)
                                {
                                    if (bottom) g.DrawLine(new Pen(band_edge_color), vgrid, H + top, vgrid, H + H);
                                    else g.DrawLine(new Pen(band_edge_color), vgrid, top, vgrid, H);

                                    label = actual_fgrid.ToString("f4");
                                    label = label.Replace(",", ".");    // handle Windows localization issues
                                    int offset = label.IndexOf('.') + 4;
                                    label = label.Insert(offset, ".");

                                    if (actual_fgrid < 10) offsetL = (int)((label.Length + 2) * 4.1) - 14;
                                    else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                    else offsetL = (int)((label.Length) * 4.1) - 8;

                                    /* if (actual_fgrid < 10) offsetL = (int)((label.Length + 1) * 4.1) - 14;
                                    else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                    else offsetL = (int)((label.Length + 1) * 4.1) - 8; */

                                    if (bottom) g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid - offsetL, H + (float)Math.Floor(H * .005));
                                    else g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid - offsetL, (float)Math.Floor(H * .005));
                                }
                                else
                                {

                                    if (freq_step_size >= 2000)
                                    {
                                        double t100;
                                        double t1000;
                                        t100 = (actual_fgrid * 100);
                                        t1000 = (actual_fgrid * 1000);

                                        int it100 = (int)t100;
                                        int it1000 = (int)t1000;

                                        int it100x10 = it100 * 10;

                                        if (it100x10 == it1000)
                                        {
                                        }
                                        else
                                        {
                                            grid_pen.DashStyle = DashStyle.Dot;
                                        }
                                    }
                                    else
                                    {
                                        if (freq_step_size == 1000)
                                        {
                                            double t200;
                                            double t2000;
                                            t200 = (actual_fgrid * 200);
                                            t2000 = (actual_fgrid * 2000);

                                            int it200 = (int)t200;
                                            int it2000 = (int)t2000;

                                            int it200x10 = it200 * 10;

                                            if (it200x10 == it2000)
                                            {
                                            }
                                            else
                                            {
                                                grid_pen.DashStyle = DashStyle.Dot;
                                            }
                                        }
                                        else
                                        {
                                            double t1000;
                                            double t10000;
                                            t1000 = (actual_fgrid * 1000);
                                            t10000 = (actual_fgrid * 10000);

                                            int it1000 = (int)t1000;
                                            int it10000 = (int)t10000;

                                            int it1000x10 = it1000 * 10;

                                            if (it1000x10 == it10000)
                                            {
                                            }
                                            else
                                            {
                                                grid_pen.DashStyle = DashStyle.Dot;
                                            }
                                        }
                                    }
                                    if (bottom) g.DrawLine(grid_pen, vgrid, H + top, vgrid, H + H);
                                    else g.DrawLine(grid_pen, vgrid, top, vgrid, H);            //wa6ahl
                                    grid_pen.DashStyle = DashStyle.Solid;

                                    // make freq grid labels
                                    double actual_fgrid_label = Math.Round(actual_fgrid, 4);
                                    label = actual_fgrid_label.ToString("f4");
                                    label = label.Replace(",", ".");    // handle Windows localization issues
                                    int offset = label.IndexOf('.') + 4;
                                    label = label.Insert(offset, ".");

                                    if (actual_fgrid < 10) offsetL = (int)((label.Length + 2) * 4.1) - 14;
                                    else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                    else offsetL = (int)((label.Length) * 4.1) - 8;

                                    if (bottom) g.DrawString(label, font, grid_text_brush, vgrid - offsetL, H + (float)Math.Floor(H * .005));
                                    else g.DrawString(label, font, grid_text_brush, vgrid - offsetL, (float)Math.Floor(H * .005));
                                }
                            }
                            else
                            {
                                vgrid = Convert.ToInt32((double)-(fgrid - low) / (low - high) * W); //wa6ahl
                                if (bottom) g.DrawLine(grid_pen, vgrid, H + top, vgrid, H + H);
                                else g.DrawLine(grid_pen, vgrid, top, vgrid, H);            //wa6ahl

                                double new_fgrid = (vfoa_hz + fgrid) / 1000000;

                                label = fgrid.ToString();
                                offsetL = (int)((label.Length + 1) * 4.1);
                                offsetR = (int)(label.Length * 4.1);
                                if ((vgrid - offsetL >= 0) && (vgrid + offsetR < W) && (fgrid != 0))
                                {
                                    if (bottom) g.DrawString(label, font, grid_text_brush, vgrid - offsetL, H + (float)Math.Floor(H * .005));
                                    else g.DrawString(label, font, grid_text_brush, vgrid - offsetL, (float)Math.Floor(H * .005));
                                }
                            }
                        }


                        int[] band_edge_list_r1k = {  1810000, 1880000, 3500000, 3800000, 5350000, 5450000,
                                    7000000, 7200000, 10100000, 10150000, 14000000, 14350000,18068000, 18168000, 21000000, 21450000,
                                    24890000, 24990000, 28000000, 29700000, 50000000, 52000000, 144000000, 146000000 };

                        for (int i = 0; i < band_edge_list_r1k.Length; i++)
                        {
                            double band_edge_offset = band_edge_list_r1k[i] - vfo;
                            if (band_edge_offset >= low && band_edge_offset <= high)
                            {
                                int temp_vline = (int)((double)(band_edge_offset - low) / (high - low) * W);//wa6ahl
                                if (bottom) g.DrawLine(new Pen(band_edge_color), temp_vline, H, temp_vline, H + top);//wa6ahl
                                else g.DrawLine(new Pen(band_edge_color), temp_vline, 0, temp_vline, top);
                            }
                            if (i == 1 && !show_freq_offset) break;
                        }


                        break; // EU13



                    //============================================================================================================
                    //============================================================================================================
                    //============================================================================================================
                    //===============================================
                    case FRSRegion.Luxembourg:  // EU15 (IARU1 60m) & (52mhz 6m)

                        for (int i = 0; i <= h_steps + 1; i++)
                        {
                            string label;
                            int offsetL;
                            int offsetR;

                            int fgrid = i * freq_step_size + (low / freq_step_size) * freq_step_size;
                            double actual_fgrid = ((double)(vfo_round + fgrid)) / 1000000;
                            int vgrid = (int)((double)(fgrid - vfo_delta - low) / (high - low) * W);

                            if (!show_freq_offset)
                            {
                                if (actual_fgrid == 1.81 || actual_fgrid == 2.00 ||
                                    actual_fgrid == 3.5 || actual_fgrid == 3.8 ||
                                    actual_fgrid == 5.250 || actual_fgrid == 5.45 ||   // ke9ns add
                                    actual_fgrid == 7.0 || actual_fgrid == 7.2 ||
                                    actual_fgrid == 10.1 || actual_fgrid == 10.15 ||
                                    actual_fgrid == 14.0 || actual_fgrid == 14.35 ||
                                    actual_fgrid == 18.068 || actual_fgrid == 18.168 ||
                                    actual_fgrid == 21.0 || actual_fgrid == 21.45 ||
                                    actual_fgrid == 24.89 || actual_fgrid == 24.99 ||
                                    actual_fgrid == 28.0 || actual_fgrid == 29.7 ||
                                    actual_fgrid == 50.0 || actual_fgrid == 52.0 ||
                                    actual_fgrid == 144.0 || actual_fgrid == 146.0)
                                {
                                    if (bottom) g.DrawLine(new Pen(band_edge_color), vgrid, H + top, vgrid, H + H);
                                    else g.DrawLine(new Pen(band_edge_color), vgrid, top, vgrid, H);

                                    label = actual_fgrid.ToString("f4");
                                    label = label.Replace(",", ".");    // handle Windows localization issues
                                    int offset = label.IndexOf('.') + 4;
                                    label = label.Insert(offset, ".");

                                    if (actual_fgrid < 10) offsetL = (int)((label.Length + 2) * 4.1) - 14;
                                    else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                    else offsetL = (int)((label.Length) * 4.1) - 8;

                                    /* if (actual_fgrid < 10) offsetL = (int)((label.Length + 1) * 4.1) - 14;
                                    else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                    else offsetL = (int)((label.Length + 1) * 4.1) - 8; */

                                    if (bottom) g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid - offsetL, H + (float)Math.Floor(H * .005));
                                    else g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid - offsetL, (float)Math.Floor(H * .005));
                                }
                                else
                                {

                                    if (freq_step_size >= 2000)
                                    {
                                        double t100;
                                        double t1000;
                                        t100 = (actual_fgrid * 100);
                                        t1000 = (actual_fgrid * 1000);

                                        int it100 = (int)t100;
                                        int it1000 = (int)t1000;

                                        int it100x10 = it100 * 10;

                                        if (it100x10 == it1000)
                                        {
                                        }
                                        else
                                        {
                                            grid_pen.DashStyle = DashStyle.Dot;
                                        }
                                    }
                                    else
                                    {
                                        if (freq_step_size == 1000)
                                        {
                                            double t200;
                                            double t2000;
                                            t200 = (actual_fgrid * 200);
                                            t2000 = (actual_fgrid * 2000);

                                            int it200 = (int)t200;
                                            int it2000 = (int)t2000;

                                            int it200x10 = it200 * 10;

                                            if (it200x10 == it2000)
                                            {
                                            }
                                            else
                                            {
                                                grid_pen.DashStyle = DashStyle.Dot;
                                            }
                                        }
                                        else
                                        {
                                            double t1000;
                                            double t10000;
                                            t1000 = (actual_fgrid * 1000);
                                            t10000 = (actual_fgrid * 10000);

                                            int it1000 = (int)t1000;
                                            int it10000 = (int)t10000;

                                            int it1000x10 = it1000 * 10;

                                            if (it1000x10 == it10000)
                                            {
                                            }
                                            else
                                            {
                                                grid_pen.DashStyle = DashStyle.Dot;
                                            }
                                        }
                                    }
                                    if (bottom) g.DrawLine(grid_pen, vgrid, H + top, vgrid, H + H);
                                    else g.DrawLine(grid_pen, vgrid, top, vgrid, H);            //wa6ahl
                                    grid_pen.DashStyle = DashStyle.Solid;

                                    // make freq grid labels
                                    double actual_fgrid_label = Math.Round(actual_fgrid, 4);
                                    label = actual_fgrid_label.ToString("f4");
                                    label = label.Replace(",", ".");    // handle Windows localization issues
                                    int offset = label.IndexOf('.') + 4;
                                    label = label.Insert(offset, ".");

                                    if (actual_fgrid < 10) offsetL = (int)((label.Length + 2) * 4.1) - 14;
                                    else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                    else offsetL = (int)((label.Length) * 4.1) - 8;

                                    if (bottom) g.DrawString(label, font, grid_text_brush, vgrid - offsetL, H + (float)Math.Floor(H * .005));
                                    else g.DrawString(label, font, grid_text_brush, vgrid - offsetL, (float)Math.Floor(H * .005));
                                }
                            }
                            else
                            {
                                vgrid = Convert.ToInt32((double)-(fgrid - low) / (low - high) * W); //wa6ahl
                                if (bottom) g.DrawLine(grid_pen, vgrid, H + top, vgrid, H + H);
                                else g.DrawLine(grid_pen, vgrid, top, vgrid, H);            //wa6ahl

                                double new_fgrid = (vfoa_hz + fgrid) / 1000000;

                                label = fgrid.ToString();
                                offsetL = (int)((label.Length + 1) * 4.1);
                                offsetR = (int)(label.Length * 4.1);
                                if ((vgrid - offsetL >= 0) && (vgrid + offsetR < W) && (fgrid != 0))
                                {
                                    if (bottom) g.DrawString(label, font, grid_text_brush, vgrid - offsetL, H + (float)Math.Floor(H * .005));
                                    else g.DrawString(label, font, grid_text_brush, vgrid - offsetL, (float)Math.Floor(H * .005));
                                }
                            }
                        }


                        int[] band_edge_list_r1u = {  1810000, 2000000, 3500000, 3800000, 5250000,5450000,
                                    7000000, 7200000, 10100000, 10150000, 14000000, 14350000,18068000, 18168000, 21000000, 21450000,
                                    24890000, 24990000, 28000000, 29700000, 50000000, 52000000, 144000000, 146000000 };

                        for (int i = 0; i < band_edge_list_r1u.Length; i++)
                        {
                            double band_edge_offset = band_edge_list_r1u[i] - vfo;
                            if (band_edge_offset >= low && band_edge_offset <= high)
                            {
                                int temp_vline = (int)((double)(band_edge_offset - low) / (high - low) * W);//wa6ahl
                                if (bottom) g.DrawLine(new Pen(band_edge_color), temp_vline, H, temp_vline, H + top);//wa6ahl
                                else g.DrawLine(new Pen(band_edge_color), temp_vline, 0, temp_vline, top);
                            }
                            if (i == 1 && !show_freq_offset) break;
                        }


                        break; // EU15






                    //============================================================================================================
                    //============================================================================================================
                    //============================================================================================================
                    //===============================================
                    case FRSRegion.Region_2:  // IARU2

                        for (int i = 0; i <= h_steps + 1; i++)
                        {
                            string label;
                            int offsetL;
                            int offsetR;

                            int fgrid = i * freq_step_size + (low / freq_step_size) * freq_step_size;
                            double actual_fgrid = ((double)(vfo_round + fgrid)) / 1000000;
                            int vgrid = (int)((double)(fgrid - vfo_delta - low) / (high - low) * W);

                            if (!show_freq_offset)
                            {
                                if (actual_fgrid == 1.80 || actual_fgrid == 2.00 ||
                                    actual_fgrid == 3.5 || actual_fgrid == 3.4 ||
                                    actual_fgrid == 5.250 || actual_fgrid == 5.45 ||   // ke9ns add
                                    actual_fgrid == 7.0 || actual_fgrid == 7.3 ||
                                    actual_fgrid == 10.1 || actual_fgrid == 10.15 ||
                                    actual_fgrid == 14.0 || actual_fgrid == 14.35 ||
                                    actual_fgrid == 18.068 || actual_fgrid == 18.168 ||
                                    actual_fgrid == 21.0 || actual_fgrid == 21.45 ||
                                    actual_fgrid == 24.89 || actual_fgrid == 24.99 ||
                                    actual_fgrid == 28.0 || actual_fgrid == 29.7 ||
                                    actual_fgrid == 50.0 || actual_fgrid == 54.0 ||
                                    actual_fgrid == 144.0 || actual_fgrid == 146.0)
                                {
                                    if (bottom) g.DrawLine(new Pen(band_edge_color), vgrid, H + top, vgrid, H + H);
                                    else g.DrawLine(new Pen(band_edge_color), vgrid, top, vgrid, H);

                                    label = actual_fgrid.ToString("f4");
                                    label = label.Replace(",", ".");    // handle Windows localization issues
                                    int offset = label.IndexOf('.') + 4;
                                    label = label.Insert(offset, ".");

                                    if (actual_fgrid < 10) offsetL = (int)((label.Length + 2) * 4.1) - 14;
                                    else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                    else offsetL = (int)((label.Length) * 4.1) - 8;

                                    /* if (actual_fgrid < 10) offsetL = (int)((label.Length + 1) * 4.1) - 14;
                                    else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                    else offsetL = (int)((label.Length + 1) * 4.1) - 8; */

                                    if (bottom) g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid - offsetL, H + (float)Math.Floor(H * .005));
                                    else g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid - offsetL, (float)Math.Floor(H * .005));
                                }
                                else
                                {

                                    if (freq_step_size >= 2000)
                                    {
                                        double t100;
                                        double t1000;
                                        t100 = (actual_fgrid * 100);
                                        t1000 = (actual_fgrid * 1000);

                                        int it100 = (int)t100;
                                        int it1000 = (int)t1000;

                                        int it100x10 = it100 * 10;

                                        if (it100x10 == it1000)
                                        {
                                        }
                                        else
                                        {
                                            grid_pen.DashStyle = DashStyle.Dot;
                                        }
                                    }
                                    else
                                    {
                                        if (freq_step_size == 1000)
                                        {
                                            double t200;
                                            double t2000;
                                            t200 = (actual_fgrid * 200);
                                            t2000 = (actual_fgrid * 2000);

                                            int it200 = (int)t200;
                                            int it2000 = (int)t2000;

                                            int it200x10 = it200 * 10;

                                            if (it200x10 == it2000)
                                            {
                                            }
                                            else
                                            {
                                                grid_pen.DashStyle = DashStyle.Dot;
                                            }
                                        }
                                        else
                                        {
                                            double t1000;
                                            double t10000;
                                            t1000 = (actual_fgrid * 1000);
                                            t10000 = (actual_fgrid * 10000);

                                            int it1000 = (int)t1000;
                                            int it10000 = (int)t10000;

                                            int it1000x10 = it1000 * 10;

                                            if (it1000x10 == it10000)
                                            {
                                            }
                                            else
                                            {
                                                grid_pen.DashStyle = DashStyle.Dot;
                                            }
                                        }
                                    }
                                    if (bottom) g.DrawLine(grid_pen, vgrid, H + top, vgrid, H + H);
                                    else g.DrawLine(grid_pen, vgrid, top, vgrid, H);            //wa6ahl
                                    grid_pen.DashStyle = DashStyle.Solid;

                                    // make freq grid labels
                                    double actual_fgrid_label = Math.Round(actual_fgrid, 4);
                                    label = actual_fgrid_label.ToString("f4");
                                    label = label.Replace(",", ".");    // handle Windows localization issues
                                    int offset = label.IndexOf('.') + 4;
                                    label = label.Insert(offset, ".");

                                    if (actual_fgrid < 10) offsetL = (int)((label.Length + 2) * 4.1) - 14;
                                    else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                    else offsetL = (int)((label.Length) * 4.1) - 8;

                                    if (bottom) g.DrawString(label, font, grid_text_brush, vgrid - offsetL, H + (float)Math.Floor(H * .005));
                                    else g.DrawString(label, font, grid_text_brush, vgrid - offsetL, (float)Math.Floor(H * .005));
                                }
                            }
                            else
                            {
                                vgrid = Convert.ToInt32((double)-(fgrid - low) / (low - high) * W); //wa6ahl
                                if (bottom) g.DrawLine(grid_pen, vgrid, H + top, vgrid, H + H);
                                else g.DrawLine(grid_pen, vgrid, top, vgrid, H);            //wa6ahl

                                double new_fgrid = (vfoa_hz + fgrid) / 1000000;

                                label = fgrid.ToString();
                                offsetL = (int)((label.Length + 1) * 4.1);
                                offsetR = (int)(label.Length * 4.1);
                                if ((vgrid - offsetL >= 0) && (vgrid + offsetR < W) && (fgrid != 0))
                                {
                                    if (bottom) g.DrawString(label, font, grid_text_brush, vgrid - offsetL, H + (float)Math.Floor(H * .005));
                                    else g.DrawString(label, font, grid_text_brush, vgrid - offsetL, (float)Math.Floor(H * .005));
                                }
                            }
                        }


                        int[] band_edge_list_r1q = {  1800000, 2000000, 3500000, 3800000, 5250000,5450000,
                                    7000000, 7200000, 10100000, 10150000, 14000000, 14350000,18068000, 18168000, 21000000, 21450000,
                                    24890000, 24990000, 28000000, 29700000, 50000000, 54000000, 144000000, 146000000 };

                        for (int i = 0; i < band_edge_list_r1q.Length; i++)
                        {
                            double band_edge_offset = band_edge_list_r1q[i] - vfo;
                            if (band_edge_offset >= low && band_edge_offset <= high)
                            {
                                int temp_vline = (int)((double)(band_edge_offset - low) / (high - low) * W);//wa6ahl
                                if (bottom) g.DrawLine(new Pen(band_edge_color), temp_vline, H, temp_vline, H + top);//wa6ahl
                                else g.DrawLine(new Pen(band_edge_color), temp_vline, 0, temp_vline, top);
                            }
                            if (i == 1 && !show_freq_offset) break;
                        }


                        break; // IARU2




                    //============================================================================================================
                    //============================================================================================================
                    //============================================================================================================
                    //===============================================
                    case FRSRegion.Region_3:  // IARU3

                        for (int i = 0; i <= h_steps + 1; i++)
                        {
                            string label;
                            int offsetL;
                            int offsetR;

                            int fgrid = i * freq_step_size + (low / freq_step_size) * freq_step_size;
                            double actual_fgrid = ((double)(vfo_round + fgrid)) / 1000000;
                            int vgrid = (int)((double)(fgrid - vfo_delta - low) / (high - low) * W);

                            if (!show_freq_offset)
                            {
                                if (actual_fgrid == 1.80 || actual_fgrid == 2.00 ||
                                    actual_fgrid == 3.5 || actual_fgrid == 3.9 ||
                                    actual_fgrid == 5.250 || actual_fgrid == 5.45 ||   // ke9ns add
                                    actual_fgrid == 7.0 || actual_fgrid == 7.3 ||
                                    actual_fgrid == 10.1 || actual_fgrid == 10.15 ||
                                    actual_fgrid == 14.0 || actual_fgrid == 14.35 ||
                                    actual_fgrid == 18.068 || actual_fgrid == 18.168 ||
                                    actual_fgrid == 21.0 || actual_fgrid == 21.45 ||
                                    actual_fgrid == 24.89 || actual_fgrid == 24.99 ||
                                    actual_fgrid == 28.0 || actual_fgrid == 29.7 ||
                                    actual_fgrid == 50.0 || actual_fgrid == 54.0 ||
                                    actual_fgrid == 144.0 || actual_fgrid == 146.0)
                                {
                                    if (bottom) g.DrawLine(new Pen(band_edge_color), vgrid, H + top, vgrid, H + H);
                                    else g.DrawLine(new Pen(band_edge_color), vgrid, top, vgrid, H);

                                    label = actual_fgrid.ToString("f4");
                                    label = label.Replace(",", ".");    // handle Windows localization issues
                                    int offset = label.IndexOf('.') + 4;
                                    label = label.Insert(offset, ".");

                                    if (actual_fgrid < 10) offsetL = (int)((label.Length + 2) * 4.1) - 14;
                                    else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                    else offsetL = (int)((label.Length) * 4.1) - 8;

                                    /* if (actual_fgrid < 10) offsetL = (int)((label.Length + 1) * 4.1) - 14;
                                    else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                    else offsetL = (int)((label.Length + 1) * 4.1) - 8; */

                                    if (bottom) g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid - offsetL, H + (float)Math.Floor(H * .005));
                                    else g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid - offsetL, (float)Math.Floor(H * .005));
                                }
                                else
                                {

                                    if (freq_step_size >= 2000)
                                    {
                                        double t100;
                                        double t1000;
                                        t100 = (actual_fgrid * 100);
                                        t1000 = (actual_fgrid * 1000);

                                        int it100 = (int)t100;
                                        int it1000 = (int)t1000;

                                        int it100x10 = it100 * 10;

                                        if (it100x10 == it1000)
                                        {
                                        }
                                        else
                                        {
                                            grid_pen.DashStyle = DashStyle.Dot;
                                        }
                                    }
                                    else
                                    {
                                        if (freq_step_size == 1000)
                                        {
                                            double t200;
                                            double t2000;
                                            t200 = (actual_fgrid * 200);
                                            t2000 = (actual_fgrid * 2000);

                                            int it200 = (int)t200;
                                            int it2000 = (int)t2000;

                                            int it200x10 = it200 * 10;

                                            if (it200x10 == it2000)
                                            {
                                            }
                                            else
                                            {
                                                grid_pen.DashStyle = DashStyle.Dot;
                                            }
                                        }
                                        else
                                        {
                                            double t1000;
                                            double t10000;
                                            t1000 = (actual_fgrid * 1000);
                                            t10000 = (actual_fgrid * 10000);

                                            int it1000 = (int)t1000;
                                            int it10000 = (int)t10000;

                                            int it1000x10 = it1000 * 10;

                                            if (it1000x10 == it10000)
                                            {
                                            }
                                            else
                                            {
                                                grid_pen.DashStyle = DashStyle.Dot;
                                            }
                                        }
                                    }
                                    if (bottom) g.DrawLine(grid_pen, vgrid, H + top, vgrid, H + H);
                                    else g.DrawLine(grid_pen, vgrid, top, vgrid, H);            //wa6ahl
                                    grid_pen.DashStyle = DashStyle.Solid;

                                    // make freq grid labels
                                    double actual_fgrid_label = Math.Round(actual_fgrid, 4);
                                    label = actual_fgrid_label.ToString("f4");
                                    label = label.Replace(",", ".");    // handle Windows localization issues
                                    int offset = label.IndexOf('.') + 4;
                                    label = label.Insert(offset, ".");

                                    if (actual_fgrid < 10) offsetL = (int)((label.Length + 2) * 4.1) - 14;
                                    else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                    else offsetL = (int)((label.Length) * 4.1) - 8;

                                    if (bottom) g.DrawString(label, font, grid_text_brush, vgrid - offsetL, H + (float)Math.Floor(H * .005));
                                    else g.DrawString(label, font, grid_text_brush, vgrid - offsetL, (float)Math.Floor(H * .005));
                                }
                            }
                            else
                            {
                                vgrid = Convert.ToInt32((double)-(fgrid - low) / (low - high) * W); //wa6ahl
                                if (bottom) g.DrawLine(grid_pen, vgrid, H + top, vgrid, H + H);
                                else g.DrawLine(grid_pen, vgrid, top, vgrid, H);            //wa6ahl

                                double new_fgrid = (vfoa_hz + fgrid) / 1000000;

                                label = fgrid.ToString();
                                offsetL = (int)((label.Length + 1) * 4.1);
                                offsetR = (int)(label.Length * 4.1);
                                if ((vgrid - offsetL >= 0) && (vgrid + offsetR < W) && (fgrid != 0))
                                {
                                    if (bottom) g.DrawString(label, font, grid_text_brush, vgrid - offsetL, H + (float)Math.Floor(H * .005));
                                    else g.DrawString(label, font, grid_text_brush, vgrid - offsetL, (float)Math.Floor(H * .005));
                                }
                            }
                        }


                        int[] band_edge_list_r1w = {  1800000, 2000000, 3500000, 3900000, 5250000,5450000,
                                    7000000, 7300000, 10100000, 10150000, 14000000, 14350000,18068000, 18168000, 21000000, 21450000,
                                    24890000, 24990000, 28000000, 29700000, 50000000, 54000000, 144000000, 146000000 };

                        for (int i = 0; i < band_edge_list_r1w.Length; i++)
                        {
                            double band_edge_offset = band_edge_list_r1w[i] - vfo;
                            if (band_edge_offset >= low && band_edge_offset <= high)
                            {
                                int temp_vline = (int)((double)(band_edge_offset - low) / (high - low) * W);//wa6ahl
                                if (bottom) g.DrawLine(new Pen(band_edge_color), temp_vline, H, temp_vline, H + top);//wa6ahl
                                else g.DrawLine(new Pen(band_edge_color), temp_vline, 0, temp_vline, top);
                            }
                            if (i == 1 && !show_freq_offset) break;
                        }


                        break; // IARU3



                    //============================================================================================================
                    //============================================================================================================
                    //============================================================================================================
                    //===============================================
                    case FRSRegion.Japan:  // IARU2

                        for (int i = 0; i <= h_steps + 1; i++)
                        {
                            string label;
                            int offsetL;
                            int offsetR;

                            int fgrid = i * freq_step_size + (low / freq_step_size) * freq_step_size;
                            double actual_fgrid = ((double)(vfo_round + fgrid)) / 1000000;
                            int vgrid = (int)((double)(fgrid - vfo_delta - low) / (high - low) * W);

                            if (!show_freq_offset)
                            {
                                if (actual_fgrid == 1.80 || actual_fgrid == 2.00 ||
                                    actual_fgrid == 3.5 || actual_fgrid == 3.9 ||
                                    actual_fgrid == 5.250 || actual_fgrid == 5.45 ||   // ke9ns add
                                    actual_fgrid == 7.0 || actual_fgrid == 7.2 ||
                                    actual_fgrid == 10.1 || actual_fgrid == 10.15 ||
                                    actual_fgrid == 14.0 || actual_fgrid == 14.35 ||
                                    actual_fgrid == 18.068 || actual_fgrid == 18.168 ||
                                    actual_fgrid == 21.0 || actual_fgrid == 21.45 ||
                                    actual_fgrid == 24.89 || actual_fgrid == 24.99 ||
                                    actual_fgrid == 28.0 || actual_fgrid == 29.7 ||
                                    actual_fgrid == 50.0 || actual_fgrid == 54.0 ||
                                    actual_fgrid == 144.0 || actual_fgrid == 146.0)
                                {
                                    if (bottom) g.DrawLine(new Pen(band_edge_color), vgrid, H + top, vgrid, H + H);
                                    else g.DrawLine(new Pen(band_edge_color), vgrid, top, vgrid, H);

                                    label = actual_fgrid.ToString("f4");
                                    label = label.Replace(",", ".");    // handle Windows localization issues
                                    int offset = label.IndexOf('.') + 4;
                                    label = label.Insert(offset, ".");

                                    if (actual_fgrid < 10) offsetL = (int)((label.Length + 2) * 4.1) - 14;
                                    else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                    else offsetL = (int)((label.Length) * 4.1) - 8;

                                    /* if (actual_fgrid < 10) offsetL = (int)((label.Length + 1) * 4.1) - 14;
                                    else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                    else offsetL = (int)((label.Length + 1) * 4.1) - 8; */

                                    if (bottom) g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid - offsetL, H + (float)Math.Floor(H * .005));
                                    else g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid - offsetL, (float)Math.Floor(H * .005));
                                }
                                else
                                {

                                    if (freq_step_size >= 2000)
                                    {
                                        double t100;
                                        double t1000;
                                        t100 = (actual_fgrid * 100);
                                        t1000 = (actual_fgrid * 1000);

                                        int it100 = (int)t100;
                                        int it1000 = (int)t1000;

                                        int it100x10 = it100 * 10;

                                        if (it100x10 == it1000)
                                        {
                                        }
                                        else
                                        {
                                            grid_pen.DashStyle = DashStyle.Dot;
                                        }
                                    }
                                    else
                                    {
                                        if (freq_step_size == 1000)
                                        {
                                            double t200;
                                            double t2000;
                                            t200 = (actual_fgrid * 200);
                                            t2000 = (actual_fgrid * 2000);

                                            int it200 = (int)t200;
                                            int it2000 = (int)t2000;

                                            int it200x10 = it200 * 10;

                                            if (it200x10 == it2000)
                                            {
                                            }
                                            else
                                            {
                                                grid_pen.DashStyle = DashStyle.Dot;
                                            }
                                        }
                                        else
                                        {
                                            double t1000;
                                            double t10000;
                                            t1000 = (actual_fgrid * 1000);
                                            t10000 = (actual_fgrid * 10000);

                                            int it1000 = (int)t1000;
                                            int it10000 = (int)t10000;

                                            int it1000x10 = it1000 * 10;

                                            if (it1000x10 == it10000)
                                            {
                                            }
                                            else
                                            {
                                                grid_pen.DashStyle = DashStyle.Dot;
                                            }
                                        }
                                    }
                                    if (bottom) g.DrawLine(grid_pen, vgrid, H + top, vgrid, H + H);
                                    else g.DrawLine(grid_pen, vgrid, top, vgrid, H);            //wa6ahl
                                    grid_pen.DashStyle = DashStyle.Solid;

                                    // make freq grid labels
                                    double actual_fgrid_label = Math.Round(actual_fgrid, 4);
                                    label = actual_fgrid_label.ToString("f4");
                                    label = label.Replace(",", ".");    // handle Windows localization issues
                                    int offset = label.IndexOf('.') + 4;
                                    label = label.Insert(offset, ".");

                                    if (actual_fgrid < 10) offsetL = (int)((label.Length + 2) * 4.1) - 14;
                                    else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                    else offsetL = (int)((label.Length) * 4.1) - 8;

                                    if (bottom) g.DrawString(label, font, grid_text_brush, vgrid - offsetL, H + (float)Math.Floor(H * .005));
                                    else g.DrawString(label, font, grid_text_brush, vgrid - offsetL, (float)Math.Floor(H * .005));
                                }
                            }
                            else
                            {
                                vgrid = Convert.ToInt32((double)-(fgrid - low) / (low - high) * W); //wa6ahl
                                if (bottom) g.DrawLine(grid_pen, vgrid, H + top, vgrid, H + H);
                                else g.DrawLine(grid_pen, vgrid, top, vgrid, H);            //wa6ahl

                                double new_fgrid = (vfoa_hz + fgrid) / 1000000;

                                label = fgrid.ToString();
                                offsetL = (int)((label.Length + 1) * 4.1);
                                offsetR = (int)(label.Length * 4.1);
                                if ((vgrid - offsetL >= 0) && (vgrid + offsetR < W) && (fgrid != 0))
                                {
                                    if (bottom) g.DrawString(label, font, grid_text_brush, vgrid - offsetL, H + (float)Math.Floor(H * .005));
                                    else g.DrawString(label, font, grid_text_brush, vgrid - offsetL, (float)Math.Floor(H * .005));
                                }
                            }
                        }


                        int[] band_edge_list_r1s = {  1810000, 2000000, 3500000, 3900000, 5250000,5450000,
                                    7000000, 7200000, 10100000, 10150000, 14000000, 14350000,18068000, 18168000, 21000000, 21450000,
                                    24890000, 24990000, 28000000, 29700000, 50000000, 54000000, 144000000, 146000000 };

                        for (int i = 0; i < band_edge_list_r1s.Length; i++)
                        {
                            double band_edge_offset = band_edge_list_r1s[i] - vfo;
                            if (band_edge_offset >= low && band_edge_offset <= high)
                            {
                                int temp_vline = (int)((double)(band_edge_offset - low) / (high - low) * W);//wa6ahl
                                if (bottom) g.DrawLine(new Pen(band_edge_color), temp_vline, H, temp_vline, H + top);//wa6ahl
                                else g.DrawLine(new Pen(band_edge_color), temp_vline, 0, temp_vline, top);
                            }
                            if (i == 1 && !show_freq_offset) break;
                        }


                        break; // Japan



                    //============================================================================================================
                    //============================================================================================================
                    //============================================================================================================
                    //===============================================
                    case FRSRegion.Russia:      // RUSS (no 60m)


                        for (int i = 0; i <= h_steps + 1; i++)
                        {
                            string label;
                            int offsetL;
                            int offsetR;

                            int fgrid = i * freq_step_size + (low / freq_step_size) * freq_step_size;
                            double actual_fgrid = ((double)(vfo_round + fgrid)) / 1000000;
                            int vgrid = (int)((double)(fgrid - vfo_delta - low) / (high - low) * W);

                            if (!show_freq_offset)
                            {
                                if (actual_fgrid == 1.81 || actual_fgrid == 2.0 ||
                                    actual_fgrid == 3.5 || actual_fgrid == 3.8 ||
                                    actual_fgrid == 5.250 || actual_fgrid == 5.45 ||   // ke9ns add
                                    actual_fgrid == 7.0 || actual_fgrid == 7.2 ||
                                    actual_fgrid == 10.1 || actual_fgrid == 10.15 ||
                                    actual_fgrid == 14.0 || actual_fgrid == 14.35 ||
                                    actual_fgrid == 18.068 || actual_fgrid == 18.168 ||
                                    actual_fgrid == 21.0 || actual_fgrid == 21.45 ||
                                    actual_fgrid == 24.89 || actual_fgrid == 25.14 ||
                                    actual_fgrid == 26.97 || actual_fgrid == 27.86 ||
                                    actual_fgrid == 28.0 || actual_fgrid == 29.7 ||
                                    actual_fgrid == 50.0 || actual_fgrid == 54.0 ||
                                    actual_fgrid == 144.0 || actual_fgrid == 146.0)
                                {
                                    if (bottom) g.DrawLine(new Pen(band_edge_color), vgrid, H + top, vgrid, H + H);
                                    else g.DrawLine(new Pen(band_edge_color), vgrid, top, vgrid, H);

                                    label = actual_fgrid.ToString("f4");
                                    label = label.Replace(",", ".");    // handle Windows localization issues
                                    int offset = label.IndexOf('.') + 4;
                                    label = label.Insert(offset, ".");

                                    if (actual_fgrid < 10) offsetL = (int)((label.Length + 2) * 4.1) - 14;
                                    else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                    else offsetL = (int)((label.Length) * 4.1) - 8;

                                    /* if (actual_fgrid < 10) offsetL = (int)((label.Length + 1) * 4.1) - 14;
                                    else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                    else offsetL = (int)((label.Length + 1) * 4.1) - 8; */

                                    if (bottom) g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid - offsetL, H + (float)Math.Floor(H * .005));
                                    else g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid - offsetL, (float)Math.Floor(H * .005));
                                }
                                else
                                {

                                    if (freq_step_size >= 2000)
                                    {
                                        double t100;
                                        double t1000;
                                        t100 = (actual_fgrid * 100);
                                        t1000 = (actual_fgrid * 1000);

                                        int it100 = (int)t100;
                                        int it1000 = (int)t1000;

                                        int it100x10 = it100 * 10;

                                        if (it100x10 == it1000)
                                        {
                                        }
                                        else
                                        {
                                            grid_pen.DashStyle = DashStyle.Dot;
                                        }
                                    }
                                    else
                                    {
                                        if (freq_step_size == 1000)
                                        {
                                            double t200;
                                            double t2000;
                                            t200 = (actual_fgrid * 200);
                                            t2000 = (actual_fgrid * 2000);

                                            int it200 = (int)t200;
                                            int it2000 = (int)t2000;

                                            int it200x10 = it200 * 10;

                                            if (it200x10 == it2000)
                                            {
                                            }
                                            else
                                            {
                                                grid_pen.DashStyle = DashStyle.Dot;
                                            }
                                        }
                                        else
                                        {
                                            double t1000;
                                            double t10000;
                                            t1000 = (actual_fgrid * 1000);
                                            t10000 = (actual_fgrid * 10000);

                                            int it1000 = (int)t1000;
                                            int it10000 = (int)t10000;

                                            int it1000x10 = it1000 * 10;

                                            if (it1000x10 == it10000)
                                            {
                                            }
                                            else
                                            {
                                                grid_pen.DashStyle = DashStyle.Dot;
                                            }
                                        }
                                    }
                                    if (bottom) g.DrawLine(grid_pen, vgrid, H + top, vgrid, H + H);
                                    else g.DrawLine(grid_pen, vgrid, top, vgrid, H);            //wa6ahl
                                    grid_pen.DashStyle = DashStyle.Solid;

                                    // make freq grid labels
                                    double actual_fgrid_label = Math.Round(actual_fgrid, 4);
                                    label = actual_fgrid_label.ToString("f4");
                                    label = label.Replace(",", ".");    // handle Windows localization issues
                                    int offset = label.IndexOf('.') + 4;
                                    label = label.Insert(offset, ".");

                                    if (actual_fgrid < 10) offsetL = (int)((label.Length + 2) * 4.1) - 14;
                                    else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                                    else offsetL = (int)((label.Length) * 4.1) - 8;

                                    if (bottom) g.DrawString(label, font, grid_text_brush, vgrid - offsetL, H + (float)Math.Floor(H * .005));
                                    else g.DrawString(label, font, grid_text_brush, vgrid - offsetL, (float)Math.Floor(H * .005));
                                }
                            }
                            else
                            {
                                vgrid = Convert.ToInt32((double)-(fgrid - low) / (low - high) * W); //wa6ahl
                                if (bottom) g.DrawLine(grid_pen, vgrid, H + top, vgrid, H + H);
                                else g.DrawLine(grid_pen, vgrid, top, vgrid, H);            //wa6ahl

                                double new_fgrid = (vfoa_hz + fgrid) / 1000000;

                                label = fgrid.ToString();
                                offsetL = (int)((label.Length + 1) * 4.1);
                                offsetR = (int)(label.Length * 4.1);
                                if ((vgrid - offsetL >= 0) && (vgrid + offsetR < W) && (fgrid != 0))
                                {
                                    if (bottom) g.DrawString(label, font, grid_text_brush, vgrid - offsetL, H + (float)Math.Floor(H * .005));
                                    else g.DrawString(label, font, grid_text_brush, vgrid - offsetL, (float)Math.Floor(H * .005));
                                }
                            }
                        }


                        int[] band_edge_list_r1r = {  1810000, 2000000, 3500000, 3800000, 5250000,5450000,
                                    7000000, 7200000, 10100000, 10150000, 14000000, 14350000,18068000, 18168000, 21000000, 21450000,
                                    24890000, 25140000,26970000,27860000, 28000000, 29700000, 50000000, 54000000, 144000000, 146000000 };

                        for (int i = 0; i < band_edge_list_r1r.Length; i++)
                        {
                            double band_edge_offset = band_edge_list_r1r[i] - vfo;
                            if (band_edge_offset >= low && band_edge_offset <= high)
                            {
                                int temp_vline = (int)((double)(band_edge_offset - low) / (high - low) * W);//wa6ahl
                                if (bottom) g.DrawLine(new Pen(band_edge_color), temp_vline, H, temp_vline, H + top);//wa6ahl
                                else g.DrawLine(new Pen(band_edge_color), temp_vline, 0, temp_vline, top);
                            }
                            if (i == 1 && !show_freq_offset) break;
                        }


                        break; // RUSS



















                } // check region for drawing waterfall freq numbers

            } // not in continuum mode



            /*// Draw horizontal lines
			for(int i=1; i<h_steps; i++)
			{
				int xOffset = 0;
				int num = spectrum_grid_max - i*spectrum_grid_step;
				int y = (int)((double)(spectrum_grid_max - num)*H/y_range);
				g.DrawLine(grid_pen, 0, y, W, y);

				// Draw horizontal line labels
				if(i != 1) // avoid intersecting vertical and horizontal labels
				{
					num = spectrum_grid_max - i*spectrum_grid_step;
					string label = num.ToString();
					if(label.Length == 3) xOffset = 7;
					//int offset = (int)(label.Length*4.1);
					if(display_label_align != DisplayLabelAlignment.LEFT &&
						display_label_align != DisplayLabelAlignment.AUTO &&
						(current_dsp_mode == DSPMode.USB ||
						current_dsp_mode == DSPMode.CWU))
						xOffset -= 32;
					SizeF size = g.MeasureString(label, font);

					int x = 0;
					switch(display_label_align)
					{
						case DisplayLabelAlignment.LEFT:
							x = xOffset + 3;
							break;
						case DisplayLabelAlignment.CENTER:
							x = center_line_x+xOffset;
							break;
						case DisplayLabelAlignment.RIGHT:
							x = (int)(W-size.Width);
							break;
						case DisplayLabelAlignment.AUTO:
							x = xOffset + 3;
							break;
						case DisplayLabelAlignment.OFF:
							x = W;
							break;
					}

					y -= 8;
					if(y+9 < H)
						g.DrawString(label, font, grid_text_brush, x, y);
				}
			}*/

            //=======================================================================
            // ke9ns ADD tick
            //=======================================================================


            //   int fgrid = (low / freq_step_size) * freq_step_size;
            //   double actual_fgrid = ((double)(vfo_round + fgrid)) / 1000000;  // freq to print out
            //   int vgrid = (int)((double)(fgrid - vfo_delta - low) / (high - low) * W); // 

            //  if (bottom) g.DrawLine(new Pen(Color.DarkSeaGreen), vgrid, H + top, vgrid, H + H); // ke9ns draw little tick lines under freq at band edges only
            //  else g.DrawLine(new Pen(Color.DarkSeaGreen), vgrid, top, vgrid, H);


            if ((continuum == 0)|| (rx == 2)) // ke9ns add
            {
                //=======================================================================
                // Draw 0Hz vertical line if visible
                if (center_line_x >= 0 && center_line_x <= W)
                {
                    if (!bottom)
                    {
                        g.DrawLine(new Pen(grid_zero_color), center_line_x, 0, center_line_x , top);
                        g.DrawLine(new Pen(grid_zero_color), center_line_x + 1 , 0, center_line_x + 1 , top);
                    }
                    else
                    {
                        g.DrawLine(new Pen(grid_zero_color), center_line_x , H, center_line_x , H + top);
                        g.DrawLine(new Pen(grid_zero_color), center_line_x + 1 , H, center_line_x + 1 , H + top);
                    }
                }

                if (show_freq_offset)
                {
                    if (bottom) g.DrawString("0", font, new SolidBrush(grid_zero_color), center_line_x - 5, H + (float)Math.Floor(H * .005));
                    else g.DrawString("0", font, new SolidBrush(grid_zero_color), center_line_x - 5, (float)Math.Floor(H * .005));
                }

            } // no continuum mode

			if(high_swr && !bottom)
				g.DrawString("High SWR", new System.Drawing.Font("Arial", 14, FontStyle.Bold), new SolidBrush(Color.Red), 245, 20);

        }// draw waterfall grid



        //=======================================================================

        //========================================================================================

        private static void DrawOffBackground(Graphics g, int W, int H, bool bottom)
		{
			// draw background
			if(bottom) g.FillRectangle(new SolidBrush(display_background_color), 0, H, W, H);
			else g.FillRectangle(new SolidBrush(display_background_color), 0, 0, W, H);

			if(high_swr && !bottom)
				g.DrawString("High SWR", new System.Drawing.Font("Arial", 14, FontStyle.Bold), new SolidBrush(Color.Red), 245, 20);
		}

		private static float[] scope_min = new float[W];
		public static float[] ScopeMin
		{
			get { return scope_min; }
			set { scope_min = value; }
		}
		private static float[] scope_max = new float[W];
		public static float[] ScopeMax
		{
			get { return scope_max; }
			set { scope_max = value; }
		}

        //================================================================================================
        // ke9ns time vs amplitude using audio.doscope() routine
		unsafe private static bool DrawScope(Graphics g, int W, int H, bool bottom)
		{
			if(scope_min.Length < W) 
			{
				scope_min = new float[W];
				Audio.ScopeMin = scope_min;  // ke9ns from DoScope() routine
			}

			if(scope_max.Length < W)
			{
				scope_max = new float[W];
				Audio.ScopeMax = scope_max;
			}

			DrawScopeGrid(ref g, W, H, bottom);

			Point[] points = new Point[W*2];            // create Point array

          //  Debug.WriteLine("scope");

            for (int i=0; i < W; i++)						// fill point array
			{	
				int pixel = 0;

				if(bottom) pixel = (int)(H/2 * scope_max[i]);
				else pixel = (int)(H/2 * scope_max[i]);          // ke9ns scope data scaled to fit display area available

				int y = H/2 - pixel;  // ke9ns this is the actual data moved to the part of the display being used

				points[i].X = i;
				points[i].Y = y;
				if(bottom) points[i].Y += H;

				if(bottom) pixel = (int)(H/2 * scope_min[i]);
				else pixel = (int)(H/2 * scope_min[i]);

				y = H/2 - pixel;
				points[W*2-1-i].X = i;
				points[W*2-1-i].Y = y;

				if(bottom)points[W*2-1-i].Y += H;
				//if(points[W*2-1-i].Y == points[i].Y)
				//	points[W*2-1-i].Y += 1;
			}

			// draw the connected points
			g.DrawLines(data_line_pen, points);
			g.FillPolygon(new SolidBrush(data_line_pen.Color), points);

			// draw long cursor
			if(current_click_tune_mode != ClickTuneMode.Off)
			{
				Pen p;
				if(current_click_tune_mode == ClickTuneMode.VFOA)
					p = new Pen(grid_text_color);
				else p = new Pen(Color.Red);
				if(bottom) g.DrawLine(p, display_cursor_x, 0, display_cursor_x, H+H);
				else g.DrawLine(p, display_cursor_x, 0, display_cursor_x, H);
				g.DrawLine(p, 0, display_cursor_y, W, display_cursor_y);
			}

			return true;
		} // draw scope





		unsafe private static bool DrawPhase(Graphics g, int W, int H, bool bottom)
		{
			DrawPhaseGrid(ref g, W, H, bottom);
			int num_points = phase_num_pts;

			if(!bottom && data_ready)
			{
				// get new data
				fixed(void *rptr = &new_display_data[0])
					fixed(void *wptr = &current_display_data[0])
						Win32.memcpy(wptr, rptr, BUFFER_SIZE*sizeof(float));
				data_ready = false;
			}
			else if(bottom && data_ready_bottom)
			{
				// get new data
				fixed(void *rptr = &new_display_data_bottom[0])
					fixed(void *wptr = &current_display_data_bottom[0])
						Win32.memcpy(wptr, rptr, BUFFER_SIZE*sizeof(float));
				data_ready_bottom = false;
			}

			Point[] points = new Point[num_points];		// declare Point array
			for(int i=0,j=0; i<num_points; i++,j+=8)	// fill point array
			{
				int x = 0;
				int y = 0;
				if(bottom)
				{
					x = (int)(current_display_data_bottom[i*2]*H/2);
					y = (int)(current_display_data_bottom[i*2+1]*H/2);
				}
				else
				{
					x = (int)(current_display_data[i*2]*H/2);
					y = (int)(current_display_data[i*2+1]*H/2);
				}
				points[i].X = W/2+x;
				points[i].Y = H/2+y;
				if(bottom) points[i].Y += H;
			}
			
			// draw each point
			for(int i=0; i<num_points; i++)
				g.DrawRectangle(data_line_pen, points[i].X, points[i].Y, 1, 1);

			// draw long cursor
			if(current_click_tune_mode != ClickTuneMode.Off)
			{
				Pen p;
				if(current_click_tune_mode == ClickTuneMode.VFOA)
					p = new Pen(grid_text_color);
				else p = new Pen(Color.Red);
				g.DrawLine(p, display_cursor_x, 0, display_cursor_x, H);
				g.DrawLine(p, 0, display_cursor_y, W, display_cursor_y);
			}

			return true;
		}

		unsafe private static void DrawPhase2(Graphics g, int W, int H, bool bottom)
		{
			DrawPhaseGrid(ref g, W, H, bottom);
			int num_points = phase_num_pts;

			if(!bottom && data_ready)
			{
				// get new data
				fixed(void *rptr = &new_display_data[0])
					fixed(void *wptr = &current_display_data[0])
						Win32.memcpy(wptr, rptr, BUFFER_SIZE*sizeof(float));
				data_ready = false;
			}
			else if(bottom && data_ready_bottom)
			{
				// get new data
				fixed(void *rptr = &new_display_data_bottom[0])
					fixed(void *wptr = &current_display_data_bottom[0])
						Win32.memcpy(wptr, rptr, BUFFER_SIZE*sizeof(float));
				data_ready_bottom = false;
			}

			Point[] points = new Point[num_points];		// declare Point array
			for(int i=0; i<num_points; i++)	// fill point array
			{
				int x = 0;
				int y = 0;
				if(bottom)
				{
					x = (int)(current_display_data_bottom[i*2]*H*0.5*500);
					y = (int)(current_display_data_bottom[i*2+1]*H*0.5*500);
				}
				else
				{
					x = (int)(current_display_data[i*2]*H*0.5*500);
					y = (int)(current_display_data[i*2+1]*H*0.5*500);
				}
				points[i].X = (int)(W*0.5+x);
				points[i].Y = (int)(H*0.5+y);
				if(bottom) points[i].Y += H;
			}
			
			// draw each point
			for(int i=0; i<num_points; i++)
				g.DrawRectangle(data_line_pen, points[i].X, points[i].Y, 1, 1);

			// draw long cursor
			if(current_click_tune_mode != ClickTuneMode.Off)
			{
				Pen p;
				if(current_click_tune_mode == ClickTuneMode.VFOA)
					p = new Pen(grid_text_color);
				else p = new Pen(Color.Red);
				if(bottom) g.DrawLine(p, display_cursor_x, 0, display_cursor_x, H);
				else g.DrawLine(p, display_cursor_x, H, display_cursor_x, H+H);
				g.DrawLine(p, 0, display_cursor_y, W, display_cursor_y);
			}
		}

		private static Point[] points;
        
        unsafe static private bool DrawSpectrum(Graphics g, int W, int H, bool bottom)
		{
			DrawSpectrumGrid(ref g, W, H, bottom);

            if (points == null || points.Length < W) points = new Point[W];			// array of points to display

			float slope = 0.0f;						// samples to process per pixel
			int num_samples = 0;					// number of samples to process
			int start_sample_index = 0;				// index to begin looking at samples
			int low = 0;
			int high = 0;

            float local_max_y = float.MinValue;

			if(!mox)
			{
				low = rx_display_low;
				high = rx_display_high;
			}
			else
			{
				low = tx_display_low;
				high = tx_display_high;
			}

			if(rx1_dsp_mode == DSPMode.DRM)
			{
				low = 2500;
				high = 21500;
			}

			int yRange = spectrum_grid_max - spectrum_grid_min;

			if(!bottom && data_ready)
			{
				if(mox && (rx1_dsp_mode == DSPMode.CWL || rx1_dsp_mode == DSPMode.CWU))
				{
					for(int i=0; i<current_display_data.Length; i++)
                        current_display_data[i] = spectrum_grid_min - rx1_display_cal_offset;
				}
				else
				{
					fixed(void *rptr = &new_display_data[0]) // spectrum
						fixed(void *wptr = &current_display_data[0])
							Win32.memcpy(wptr, rptr, BUFFER_SIZE*sizeof(float));

					//if ( current_model == Model.SOFTROCK40 ) 
					//	console.AdjustDisplayDataForBandEdge(ref current_display_data);
				}
				data_ready = false;
			}
			else if(bottom && data_ready_bottom)
			{
				/*if(mox && (rx1_dsp_mode == DSPMode.CWL || rx1_dsp_mode == DSPMode.CWU))
				{
					for(int i=0; i<current_display_data_bottom.Length; i++)
						current_display_data_bottom[i] = -200.0f;
				}
				else*/
				{
					fixed(void *rptr = &new_display_data_bottom[0]) // spectrum
						fixed(void *wptr = &current_display_data_bottom[0])
							Win32.memcpy(wptr, rptr, BUFFER_SIZE*sizeof(float));

					if ( current_model == Model.SOFTROCK40 ) 
						console.AdjustDisplayDataForBandEdge(ref current_display_data_bottom);
				}
				data_ready_bottom = false;
			}

			if(!bottom && average_on)
				console.UpdateRX1DisplayAverage(rx1_average_buffer, current_display_data);
			else if(bottom && rx2_avg_on)
				console.UpdateRX2DisplayAverage(rx2_average_buffer, current_display_data_bottom);

			if(!bottom && peak_on)
				UpdateDisplayPeak(rx1_peak_buffer, current_display_data);
			else if(bottom && rx2_peak_on)
				UpdateDisplayPeak(rx2_peak_buffer, current_display_data_bottom);

			start_sample_index = (BUFFER_SIZE>>1) + (int)((low * BUFFER_SIZE) / sample_rate);
			num_samples = (int)((high - low) * BUFFER_SIZE / sample_rate);

          //  Debug.WriteLine("start sample index " + start_sample_index + " , " + num_samples + " , " + sample_rate + " , " + BUFFER_SIZE);

            if (start_sample_index < 0) start_sample_index = 0;

			if ((num_samples - start_sample_index) > (BUFFER_SIZE+1))	num_samples = BUFFER_SIZE - start_sample_index;
           
            slope = (float)num_samples/(float)W;

        //   Debug.WriteLine("start sample index2 " + start_sample_index + " , " + num_samples + " , " + sample_rate + " , " + BUFFER_SIZE + " , " + slope);

            for (int i=0; i < W; i++)
			{
				float max = float.MinValue;
				float dval = i*slope + start_sample_index;
				int lindex = (int)Math.Floor(dval);

            //  if (i== 0)  Debug.WriteLine("spec1: " + lindex + " , " + current_display_data[lindex] + " , "+ ((float)lindex - dval + 1));
            //    if (i == W-1) Debug.WriteLine("spec2: " + lindex);

                if (!bottom)
				{
					if (slope <= 1) 
						max =  current_display_data[lindex]*((float)lindex-dval+1) + current_display_data[lindex+1]*(dval-(float)lindex);
					else 
					{
						int rindex = (int)Math.Floor(dval + slope);
						if (rindex > BUFFER_SIZE) rindex = BUFFER_SIZE;
						for(int j=lindex;j < rindex;j++)
							if (current_display_data[j] > max) max=current_display_data[j];
					}
				}
				else
				{
					if (slope <= 1) 
						max =  current_display_data_bottom[lindex]*((float)lindex-dval+1) + current_display_data_bottom[lindex+1]*(dval-(float)lindex);
					else 
					{
						int rindex = (int)Math.Floor(dval + slope);
						if (rindex > BUFFER_SIZE) rindex = BUFFER_SIZE;
						for(int j=lindex;j<rindex;j++)
							if (current_display_data_bottom[j] > max) max=current_display_data_bottom[j];
					}
				}

               
             //   if (lindex == 2046) Debug.WriteLine("MAX before " + max + " , " + current_display_data_bottom[lindex]);

                if (!bottom) max += rx1_display_cal_offset;
				else max += rx2_display_cal_offset;

				if(!mox)
				{
					if(!bottom)	max += rx1_preamp_offset;
					else max += rx2_preamp_offset;
				}

            //   if (lindex == 2047) Debug.WriteLine("MAX after " + max + " , " + rx1_display_cal_offset + " , "+ rx1_preamp_offset );

                if (max > local_max_y)
				{
					local_max_y = max;
					max_x = i;
				}

				points[i].X = i;
                points[i].Y = (int)Math.Min((Math.Floor((spectrum_grid_max - max) * H / yRange)), H);
				if(bottom) points[i].Y += H;
			}

			if(!bottom) max_y = local_max_y; // if top

			g.DrawLines(data_line_pen, points); // ke9ns draw spectrum

			// draw long cursor
			if(current_click_tune_mode != ClickTuneMode.Off)
			{
				Pen p;
				if(current_click_tune_mode == ClickTuneMode.VFOA)
					p = new Pen(grid_text_color);
				else p = new Pen(Color.Red);
				if(bottom)
				{
					g.DrawLine(p, display_cursor_x, H, display_cursor_x, H+H);
					g.DrawLine(p, 0, display_cursor_y, W, display_cursor_y);
				}
				else
				{
					g.DrawLine(p, display_cursor_x, 0, display_cursor_x, H);
					g.DrawLine(p, 0, display_cursor_y, W, display_cursor_y);
				}
			}

			return true;
		
        }  // drawspectrum

        //==============================================================
        //==============================================================
        //==============================================================
        // ke9ns mod panadapter
        //==============================================================
        //==============================================================
        //==============================================================

        private static int LowLast = 0; // ke9ns add
        private static int HighLast = 0; // ke9ns add

        public static float floor = 0; // ke9ns add noise floor of panadapter (not the meter)
        public static int[] IDENT_CountP = new int[5000]; // ke9ns add records time remaining for any detected peaks
        public static Point[] points1 = new Point[5000]; // ke9ns add X and Y of the peak

        public static int IDENT_Space = 0; // ke9ns add counter for spacing out the peaks (look for dead space after signal to know you are finished detecting this siganl)
        public static int IDENT_LastY = 0; // ke9ns add temp holder for finding peak
        public static int IDENT_Lasti = 0; // ke9ns add temp holder for i position of the peak you detected
        public static int IDENT_Peaki = 0; // ke9ns add
        public static int[] IDENT_Begin = new int[5000]; // ke9ns add begin of signal
        public static int[] IDENT_End = new int[5000]; // ke9ns add end of signal
        public static int IDENT_Begini = 0; // ke9ns add 
        public static float Zoom_last = 0; // ke9ns add  if zoom changes, then reset signal detection
        public static int pan_last = 0; // ke9ns add  if pan or water moves then reset signals or move them
        public static string Band_last; // ke9ns add
        public static bool IDENT_Sig = false; // ke9ns add
        public static bool IDENT_Reset = true; // ke9ns add
        public static int floorB = 1000; // ke9ns add
        public static int countB = 0; // ke9ns add

        unsafe static private bool DrawPanadapter(Graphics g, int W, int H, int rx, bool bottom)
		{
            bool local_mox = false;                 // whether you are transmitting or not
            A3B = A2B = AB = 0;    // ke9ns auto brightness

            if (rx == 1 && !tx_on_vfob && mox) local_mox = true;
            if (rx == 2 && tx_on_vfob && mox) local_mox = true;
         

            if (local_mox) // ke9ns ADD reset panadapter to TX mic levels , then set it back when back to RX
            {
              
                if ((tx_on_vfob) && (rx == 2))
                {

                        spectrum_grid_max = 0;// TX high level db
                        spectrum_grid_min = spectrum_grid_min1; // TX low level db

                }
                else if ((!tx_on_vfob) && (rx == 1))
                {
                       spectrum_grid_max = 0;// TX high level db
                       spectrum_grid_min = spectrum_grid_min1; // TX low level db
                }
                else
                {
                   //  Debug.WriteLine("never ");
                }
            }
            else
            {
                 spectrum_grid_max = spectrum_grid_max1;  //ke9ns put back normal RX grids 
                 spectrum_grid_min = spectrum_grid_min1;

            }
          
            DrawPanadapterGrid(ref g, W, H, rx, bottom);
          
            if ((K9 == 5) && (K10 != 5) && (bottom)) H1 = H - (H / 2); // for display in 1/3 area instead of 1/2

            if ((K9 == 5) && (K10 == 5) && (bottom)) H2 = H - (H / 2); // for display in 1/4 area instead of 1/2

           
            if (pan_fill)
            {
                if (points == null || points.Length < W + 2)  points = new Point[W + 2];
            }
            else
            {
                if (points == null || points.Length < W)
                    points = new Point[W];			// array of points to display
            }

			float slope = 0.0F;						// samples to process per pixel
			int num_samples = 0;					// number of samples to process
			int start_sample_index = 0;             // index to begin looking at samples

            int Low;
            int High;
         

            if (Console.UPDATEOFF > 0) // ke9ns add when CTUN active dont allow display to move just the vfoa is allowed to slide
            {
                Low = LowLast;
                High = HighLast;
          
            }
            else
            {
                Low = rx_display_low;               // low freqency on display window (left side)
                High = rx_display_high;             // high freqency on display window (right side)

                LowLast = Low;
                HighLast = High;
            }
            
            int yRange = spectrum_grid_max - spectrum_grid_min; // find vertical range of panadapter (around 120dbm)

			float local_max_y = float.MinValue; 
           
			if(rx1_dsp_mode == DSPMode.DRM)
			{
				Low = Low + 12000;                       // drm shifts freq range up for some reason ???
				High = High + 12000;
			}


            //=================================================================
            //get line of spectrum data (if new data available)
            //=================================================================

      
            if (rx == 1 && data_ready) //&& Console.UPDATEOFF < 2 )
			{
				if(local_mox && (rx1_dsp_mode == DSPMode.CWL || rx1_dsp_mode == DSPMode.CWU))
				{
                    for (int i = 0; i < current_display_data.Length; i++)
                    {
                        current_display_data[i] = spectrum_grid_min - rx1_display_cal_offset;
                    }
				}
				else
				{
					fixed(void *rptr = &new_display_data[0])                       // panadapter allows for a C pointer usage
                    {
                        fixed (void* wptr = &current_display_data[0])              // new line of RX data coming and and being copied into current display
                        {
                            Win32.memcpy(wptr, rptr, BUFFER_SIZE * sizeof(float));  // dest,source  # of bytes to copy 4096 float sized bytes
                        }

                        fixed (void* wptr = &current_display_data1[0])              // ke9ns add for RX1 no AVG waterfall option
                        {
                            Win32.memcpy(wptr, rptr, BUFFER_SIZE * sizeof(float));  // dest,source  # of bytes to copy 4096 float sized bytes
                        }
                    }
                    // ke9ns data
                   

                 //   if ( current_model == Model.SOFTROCK40 ) 	console.AdjustDisplayDataForBandEdge(ref current_display_data);
				}
				data_ready = false;

             

            } // RX1 get new data
            else if(rx == 2 && data_ready_bottom)
			{

                if ((current_display_mode_bottom != DisplayMode.WATERFALL)) //ke9ns add dont do below if RX2 only waterfall mode (no need to waste time)
                {

                    if (local_mox && (rx2_dsp_mode == DSPMode.CWL || rx2_dsp_mode == DSPMode.CWU))
                    {
                        for (int i = 0; i < current_display_data_bottom.Length; i++)
                        {
                            current_display_data_bottom[i] = spectrum_grid_min - rx2_display_cal_offset;
                        }
                    }
                    else
                    {


                        fixed (void* rptr = &new_display_data_bottom[0]) //  panadapter
                        {
                            fixed (void* wptr = &current_display_data_bottom[0])
                            {
                                Win32.memcpy(wptr, rptr, BUFFER_SIZE * sizeof(float));  // update rx2 current display data
                            }


                            fixed (void* wptr = &current_display_data_bottom1[0])
                            {
                                Win32.memcpy(wptr, rptr, BUFFER_SIZE * sizeof(float));  // ke9ns ADD rX2 no waterfall avg data
                            }

                        }


                        if (current_model == Model.SOFTROCK40)
                            console.AdjustDisplayDataForBandEdge(ref current_display_data_bottom);
                    }
                }

				data_ready_bottom = false;
			} // RX2 get new data

            // ke9ns add   Data at this point ranges from -53.0000 to -6

            //=================================================================
            // convert data ke9ns data
            //=================================================================

            if (rx == 1 && average_on)		console.UpdateRX1DisplayAverage(rx1_average_buffer, current_display_data);
			else if(rx == 2 && rx2_avg_on)	console.UpdateRX2DisplayAverage(rx2_average_buffer, current_display_data_bottom);

			if(rx==1 && peak_on)	UpdateDisplayPeak(rx1_peak_buffer, current_display_data);
			else if(rx==2 && rx2_peak_on)	UpdateDisplayPeak(rx2_peak_buffer, current_display_data_bottom);

            //=================================================================
            // determine how to divide out the actual RX data to pixels on the screen
            //=================================================================

            // since can zoom in/out figure out where the left side of the screen starts relative to freq.
            start_sample_index = (BUFFER_SIZE >> 1) +(int)((Low * BUFFER_SIZE) / sample_rate);  // (size/2)  + ((left side * size) / 192000)

            // determine the number of samples you need to cover the zoomed screen based on sample rate and buffer size
			num_samples = (int)((High - Low) * BUFFER_SIZE / sample_rate);                      // ((right - left) * size)/192000
			
            if (start_sample_index < 0) start_sample_index = start_sample_index + DATA_BUFFER_SIZE;  // ke9ns was 4096
              
			if ((num_samples - start_sample_index) > (BUFFER_SIZE + 1))	num_samples = BUFFER_SIZE-start_sample_index;

			
			slope = (float)num_samples/(float)W;                            // slope = number of samples need to fill left to right / width of screen in pixels
                                                                            // this is determined by the ZOOM level of the screen
                                                                            // x1 zoom =  slope = ~1 (that means 1 sample per pixel)
                                                                            // slope > 1 is when you zoom all the way out too see as much as possible (large freq range)
            double max1 = 0; // ke9ns for avg the base line your receiving



            //=====================================
            // ke9ns for signal detetion routine below and in Scanner.cs 
            int IDENT_Width = 20; // ke9ns add
            int IDENT_Time = 90; // ke9ns add
            int IDENT_Thres = 6; // ke9ns add

            if (console.ScanForm != null)
            {
                IDENT_Width = (int)console.ScanForm.udIDGap.Value; // ke9ns add max signal amount
                IDENT_Time = (int)console.ScanForm.udIDTimer.Value; // ke9ns add persistance of Peak signal detected
                IDENT_Thres = (int)console.ScanForm.udIDThres.Value; // ke9ns add dBm threshold
            }

            if (console.ptbDisplayZoom.Value != Zoom_last) // if zoom level changes then reset signal detection
            {
                Zoom_last = console.ptbDisplayZoom.Value;
                IDENT_Reset = true;

            }

            else if (console.last_band != Band_last) // if zoom level changes then reset signal detection
            {
                Band_last = console.last_band;
                IDENT_Reset = true;

            }
            else if (WM1 != pan_last) // if panadapter shift left/right then move signal peaks left or right
            {
                pan_last = WM1;
                IDENT_Reset = true;

            }
            
            // ke9ns add
            if (console.N1MM_ON == true)
            {
                console.N1MM_Sample = W;
                console.N1MM_Low = (int)((vfoa_hz + Low) / 1000);  //rx_display_low;
                console.N1MM_High = (int)((vfoa_hz + High) / 1000);
              
            }

            //=================================================================
            // draw line that makes up spectrum (width of window)
            //=================================================================
            for (int i=0; i < W; i++)
			{
				float max = float.MinValue;                             // max = y point determined by RX data of spectrum as you go from 0 to W
				float dval = i * slope + start_sample_index;            // dval = how many digital values per pixel (going left to right)
				int lindex = (int)Math.Floor(dval);                     // L index = int of dval
				int rindex = (int)Math.Floor(dval + slope);             // R index = int of dval + slope ?


              
				if(rx == 1)
				{
        
                    if (slope <= 1.0 || lindex == rindex)   // if your zoom in there is less than 1 digital value per pixel so fake it.
					{
						max =  current_display_data[lindex % DATA_BUFFER_SIZE] * ((float)lindex - dval + 1) + current_display_data[(lindex+1) % DATA_BUFFER_SIZE] * (dval -(float)lindex);
					}
					else // otherwise there is more than 1 digital value per pixel so take the largest value ?
					{					
						for(int j=lindex; j < rindex; j++)
							if (current_display_data[j % DATA_BUFFER_SIZE] > max) max = current_display_data[j % DATA_BUFFER_SIZE]; // % modulus (i.e. remainder only)
					}

                   

                } // rx1
                else if(rx == 2)
				{
					if (slope <= 1.0 || lindex == rindex) 
					{
						max =  current_display_data_bottom[lindex % DATA_BUFFER_SIZE] *((float)lindex-dval + 1) + current_display_data_bottom[(lindex+1) % DATA_BUFFER_SIZE] *(dval-(float)lindex);
					}
					else 
					{					
						for(int j=lindex; j < rindex; j++)
							if (current_display_data_bottom[j % DATA_BUFFER_SIZE] > max) max=current_display_data_bottom[j % DATA_BUFFER_SIZE];
					}
				} // rx2

				if(rx==1) max += rx1_display_cal_offset;
				else if(rx==2) max += rx2_display_cal_offset;

				if(!local_mox) 
				{
					if(rx==1) max += rx1_preamp_offset;
					else if(rx==2) max += rx2_preamp_offset;
				}

				if(max > local_max_y)
				{
					local_max_y = max;
					max_x = i;
				}


                //=====================================================
                // ke9ns add 
                if ((console.N1MM_ON == true) && (!mox))
                {
                     console.N1MM_Data[i] = (int)max;

                    if ((int)max < console.N1MM_Floor) console.N1MM_Floor = (int)max;

                } // if (console.N1MM_ON == true)


                points[i].X = i; //  position as you progress left to right

                if ((K9 == 5) && (K10 != 5) && (bottom)) points[i].Y = (int)(Math.Floor((spectrum_grid_max - max) * H1 / yRange));  // display in 3rds
                else if ((K9 == 5) && (K10 == 5) && (bottom)) points[i].Y = (int)(Math.Floor((spectrum_grid_max - max) * H2 / yRange));  // display in 4ths
		        else points[i].Y = (int)(Math.Floor((spectrum_grid_max - max) * H / yRange));  // display normal

                points[i].Y = Math.Min(points[i].Y, H); // returns smaller of the 2 numbers

               
                if ((K9 == 5) && (K10 != 5) && (bottom)) points[i].Y = points[i].Y + H;
                else if ((K9 == 5) && (K10 == 5) && (bottom)) points[i].Y = points[i].Y + H; // was H

                else if ((bottom)) points[i].Y = points[i].Y + H; // this is the origina only line


                //=========================================================================
                // ke9ns add
                if ( (console.BeaconSigAvg == true)  && ( rx == 1)) // ke9ns add for beacon scanning
                {
                    max1 = max1 + max; // sum up entire panadapter line (left to right)

                }

              
                //=========================================================
                // ke9ns add   use the floor value to check for peaks in the data stream
                if ( (!mox) && (console.ScanForm != null) && (console.ScanForm.chkBoxIdent.Checked == true) && (rx == 1))
                {
                   
                    max1 = max1 + max; // sum up entire panadapter line (left to right) to come up with floor value (-125 dBm) calculated at the just below this FOR loop

                    if (IDENT_Reset == true)
                    {
                        points1[i].Y = 1000; // reset Y dBm values for all bins
                        IDENT_CountP[i] = 0; // turn off all bins
                        IDENT_Begin[i] = 0;
                        IDENT_End[i] = 0;
                    }
                    else
                    {

                        if (max >= (floor + IDENT_Thres)) // check if this bin is above the noise floor
                        {

                            if (IDENT_Space > 0) // > 0 indicates you found a new peak to check for higher nearby values
                            {

                                if ((points[i].Y < IDENT_LastY)) // if you picked a peak, but now found a nearby higher peak, then change it to this new higher peak
                                {

                                    IDENT_LastY = points[i].Y; // temp holder
                                    IDENT_Peaki = i; // temp holder for the i bin that holds the peak of this current signal

                                }
                                IDENT_Lasti = i;           // temp holder
                                IDENT_Space = IDENT_Width; // keep going since your still seeing signal


                            }
                            else // below finds the start of a new signal (left to right)
                            {
                                IDENT_Sig = true;
                                IDENT_LastY = points[i].Y; // save the value of this point for finding the real peak of this signal
                                IDENT_Lasti = i; // save this to find the Peak when the Space runs down to 0
                                IDENT_Peaki = i;
                                IDENT_Begini = i;
                                
                                IDENT_Space = IDENT_Width; // reset

                            
                            }

                        } //  if (max > floor) strong signal 


                        else // do below if in a low spot of the panadapter (either between signals or a low part of the signal)
                        {

                            if ((IDENT_Space > 0) && (i < (W-2)) ) // stil believe your in the current identified signal
                            {
                                IDENT_Space--; // this might be open space if never above the floor for IDENT_Width times
                            }
                            else // IDENT_Space = 0, You have reached beyond the end of the signal, so you can see what was your last peak i and store it int the IDENT_CountP[]
                            {
                                if (IDENT_Sig == true)
                                {
                                    if (points[IDENT_Peaki].Y < points1[IDENT_Peaki].Y)  // < because we are talking about Y dims and not dBm
                                    {
                                        points1[IDENT_Peaki] = points[IDENT_Peaki]; // record the new higher dBm value (as a Y dim)
                                 
                                    }
                                    IDENT_Begin[IDENT_Peaki] = IDENT_Begini; // this records the width of the signal
                                    IDENT_End[IDENT_Peaki] = IDENT_Lasti;
                                    IDENT_CountP[IDENT_Peaki] = IDENT_Time; // reset the onscreen counter for this peak

                                    IDENT_Sig = false; // reset for next signal
                                }

                            }

                        } // if (max < floor) no signal

                        //-----------------------------------------------------------------------------------------

                        if (IDENT_CountP[i] > 0)
                        {
                           // countB++;
                          //  if (countB > 15) countB = 0;

                            if ((i > 0) && (points1[i-1].Y > points1[i].Y) && (points1[i].Y < points1[i + 1].Y)) // try to filter out nearby peaks and just display the best
                            {
                                g.DrawLine(IDENT_pen3, IDENT_Begin[i], floorB + 15 , IDENT_End[i], floorB + 15 ); // draw  line showing begin and end of signal
                                g.DrawRectangle(IDENT_pen, points1[i].X - 2, points1[i].Y - 10, 4, 4);


                               // g.DrawString("AM",)

                            }
                            IDENT_CountP[i]--; // slowly turn off old peak signals unless they re appear

                           

                        } //  if (IDENT_CountP[i] > 0)
                        else
                        {
                            points1[i].Y = 1000;
                            IDENT_Begin[i] = 0;
                            IDENT_End[i] = 0;

                        }

                    } // IDENT_Reset = false



                } //  chkboxident (signal ident routine from scanner.cs)
               
              


                //=========================================================================
                // ke9ns add auto
                if (autobright6 == 2) // RX1 waterfall adjust
                {
                    if ((!mox) && (rx == 1))
                    {
                      
                        AB = AB + (long)max; // ke9ns add autobright feature (detect floor)
                    }

                } // autobright6 == 2

                else if (autobright7 == 3) // rx1 pan scale adjust
                {
                    if ((!mox) && (rx == 1)) // rx only on rx1
                    {
                
                        if ((int)max > AB_Peak) // ke9ns values show up as -115.234 db
                        {
                            AB_Peak = (int)max;    // ke9ns add detect peak
                          
                        }
                    
                    } // !mox

                } // autobright == 3 or == 4
               

            }  // for loop from 0 to W wide

            if (console.BeaconSigAvg == true) // ke9ns for beacon scanner floor
            {
                SpotControl.BX_dBm2 = (int)(max1 / W); // avg db value of the freq your on now
             

            } //  if (console.BeaconSigAvg == true)

        

            //=========================================================================
            // ke9ns waterfall adjust
            //=========================================================================
            // ke9ns add auto
            if (autobright6 == 2) // rx1 adjust
            {
                Debug.WriteLine(" ");

                Debug.WriteLine("==========AUTOBRIGHT6=================");

                AB1[0] = AB / W; // get avg of the entire read

                AB3 = (float)(AB1[0]);

                autobright6 = autobright3 = autobright = 0; // turn off feature

                if ((AB3 > -170) && (AB3 < -50))
                {
                    console.setupForm.udDisplayGridMin.Value = (decimal)(AB3 - abrightpan - (gridoffset - 20));
                    SpectrumGridMin = (int)(AB3 - abrightpan - (gridoffset - 20));
                    Debug.WriteLine("min = "+ SpectrumGridMin);
                }
               

            } // autobright6 = 2


            //=========================================================================
            // ke9ns Panadapter SMALL SIGNAL SCALER (increase size of signals on panadapter)  (ZOOM)
            //=========================================================================
            // ke9ns add auto scale pan
            else if (autobright7 == 3) // rx1 adjust
            {

              //  Debug.WriteLine("==========AUTOBRIGHT7=================");

                if (AB_Count < 3)
                {
                    AB_Count++;
                    AB_Total = AB_Total + AB_Peak; // ke9ns accumulate
                    AB_Peak = -200; // ke9ns reset for next scan

                }
                else
                {
                   
                    AB_Total = AB_Total / 3; // new avg to set max to 

                    autobright7 = autobright3 = autobright = 0; // turn off feature

                  //  Debug.WriteLine("==========AUTOBRIGHT7 ready to adjust=================");

                    if ((AB_Total > -170) && (AB_Total < -20))
                    {
                      //  SpectrumGridMax = AB_Total - 10; 

                        // ke9ns 85% 
                        SpectrumGridMax = SpectrumGridMin - (int)(((float)SpectrumGridMin -(float)AB_Total ) * .95F);// ke9ns max scale = min - ((the peak you detected - min) * .85)

                        Debug.WriteLine("SMALL scale=== " + SpectrumGridMax);
                        SpectrumGridStep = 2;

                    }
                    else // ke9ns if out of bounds just use standard settings
                    {
                        SpectrumGridMin = console.AutoPanScaleMin;
                        SpectrumGridMax = console.AutoPanScaleMax;
                        SpectrumGridStep = console.AutoPanScaleStep;
                     //   Debug.WriteLine("==========AUTOBRIGHT7 out of bounds=================");

                    }

                    AB_Total = 0;
                    AB_Count = 0;
                    AB_Peak = -200;

                }

            } // autobright = 3

            //=========================================================================
            // ke9ns put back original Panadapter scales Min Max (STANDARD)  (UNZOOM)
            //=========================================================================
            else if (autobright7 == 4) // 
            {
             //   Debug.WriteLine("==========AUTOBRIGHT7 put back=================");

                autobright7 = autobright3 = autobright = 0; // turn off feature

                SpectrumGridMin = console.AutoPanScaleMin;
                SpectrumGridMax = console.AutoPanScaleMax;
                SpectrumGridStep = console.AutoPanScaleStep;

            } // autobright = 4


            //================================================================
           

            if (!bottom) max_y = local_max_y;                // used in TNF function

            if (pan_fill)                               // trace spectrum line and fill under it
            {
                points[W].X = W; points[W].Y = H;
                points[W + 1].X = 0; points[W + 1].Y = H;
                if (bottom)
                {
                    points[W].Y += H;
                    points[W + 1].Y += H;
                }

                data_line_pen.Color = DisplayPanFillColor; // was  Color.FromArgb(100, 255, 255, 255); // ke9ns draw white at 100

                g.FillPolygon(data_line_pen.Brush, points);
                points[W] = points[W - 1];

                points[W + 1] = points[W - 1];
                data_line_pen.Color = data_line_color;
             
                g.DrawLines(data_line_pen, points);
            }
            else
            {
                  g.DrawLines(data_line_pen, points);                                 // trace spectrum line to screen
            }

            
            // draw notch zoom if enabled
#if (!NO_TNF)
            if (tnf_zoom)
            {
                
                List<Notch> notches;
                if (!bottom)
                    notches = NotchList.NotchesInBW((double)vfoa_hz * 1e-6, Low, High);
                else
                    notches = NotchList.NotchesInBW((double)vfob_hz * 1e-6, Low, High);

                Notch notch = null;
                foreach (Notch n in notches)
                {
                    if (n.Details)
                    {
                        notch = n;
                        break;
                    }
                }

                if ( (notch != null &&  ((bottom && notch.RX == 2) || (!bottom && notch.RX == 1)))   )
                {

                  
                    // draw zoom background
                    if (bottom) g.FillRectangle(new SolidBrush(Color.FromArgb(230, 0, 0, 0)), 0, H, W, H / zoom_height);
                    else g.FillRectangle(new SolidBrush(Color.FromArgb(230, 0, 0, 0)), 0, 0, W, H / zoom_height);
                    

                    // calculate data needed for zoomed notch
                    long rf_freq = vfoa_hz;
                    int rit = rit_hz;

                    if (bottom)
                    {
                        rf_freq = vfob_hz;
                    }

                    if (bottom)
                    {
                        switch (rx2_dsp_mode)
                        {
                            case (DSPMode.CWL):
                                rf_freq += cw_pitch;
                                break;
                            case (DSPMode.CWU):
                                rf_freq -= cw_pitch;
                                break;
                        }
                    }
                    else
                    {
                        switch (rx1_dsp_mode)
                        {
                            case (DSPMode.CWL):
                                rf_freq += cw_pitch;
                                break;
                            case (DSPMode.CWU):
                                rf_freq -= cw_pitch;
                                break;
                        }
                    }

                    int zoomed_notch_center_freq = (int)(notch_zoom_start_freq * 1e6 - rf_freq - rit);

                    int original_bw = High - Low;
                    int zoom_bw = original_bw / 10;

                    int low = zoomed_notch_center_freq - zoom_bw / 2;
                    int high = zoomed_notch_center_freq + zoom_bw / 2;

                    if (low < Low) // check left limit
                    {
                        low = Low;
                        high = Low + zoom_bw;
                    }
                    else if (high > High) // check right limit
                    {
                        high = High;
                        low = High - zoom_bw;
                    }

                    // decide colors to draw notch
                    c1 = notch_on_color_zoomed;
                    c2 = notch_highlight_color_zoomed;

                    if (!tnf_active)
                    {
                        c1 = notch_off_color;
                        c2 = Color.Black;
                    }
                    else if (notch.Permanent)
                    {
                        c1 = notch_perm_on_color;
                        c2 = notch_perm_highlight_color;
                    }

                    int notch_zoom_left_x = (int)((float)(notch.Freq * 1e6 - rf_freq - notch.BW / 2 - low - rit) / (high - low) * W);
                    int notch_zoom_right_x = (int)((float)(notch.Freq * 1e6 - rf_freq + notch.BW / 2 - low - rit) / (high - low) * W);

                    if (notch_zoom_left_x == notch_zoom_right_x)  notch_zoom_right_x = notch_zoom_left_x + 1;

                    //draw zoomed notch bars
                    if (!bottom)         drawNotchBar(g, notch, notch_zoom_left_x, notch_zoom_right_x, 0, (int)(H / zoom_height), c1, c2);
                    else                       drawNotchBar(g, notch, notch_zoom_left_x, notch_zoom_right_x, H, (int)(H / zoom_height), c1, c2);

                    // draw data
                    start_sample_index = (BUFFER_SIZE >> 1) + (int)((low * BUFFER_SIZE) / sample_rate);
                    num_samples = (int)((high - low) * BUFFER_SIZE / sample_rate);
                    if (start_sample_index < 0) start_sample_index += DATA_BUFFER_SIZE;
                    if ((num_samples - start_sample_index) > (BUFFER_SIZE + 1))   num_samples = BUFFER_SIZE - start_sample_index;

                    //Debug.WriteLine("start_sample_index: "+start_sample_index);
                    slope = (float)num_samples / (float)W;
                    //int grid_max = spectrum_grid_min + (spectrum_grid_max - spectrum_grid_min) / 2;

                  
                    for (int i = 0; i < W; i++)
                    {
                        float max = float.MinValue;
                        float dval = i * slope + start_sample_index;
                        int lindex = (int)Math.Floor(dval);
                        int rindex = (int)Math.Floor(dval + slope);

                        if (rx == 1)
                        {
                            if (slope <= 1.0 || lindex == rindex)
                            {
                                max = current_display_data[lindex % DATA_BUFFER_SIZE] * ((float)lindex - dval + 1) + current_display_data[(lindex + 1) % DATA_BUFFER_SIZE] * (dval - (float)lindex);
                            }
                            else
                            {
                                for (int j = lindex; j < rindex; j++)
                                    if (current_display_data[j % DATA_BUFFER_SIZE] > max) max = current_display_data[j % DATA_BUFFER_SIZE];
                            }
                        }
                        else if (rx == 2)
                        {
                            if (slope <= 1.0 || lindex == rindex)
                            {
                                max = current_display_data_bottom[lindex % DATA_BUFFER_SIZE] * ((float)lindex - dval + 1) + current_display_data_bottom[(lindex + 1) % DATA_BUFFER_SIZE] * (dval - (float)lindex);
                            }
                            else
                            {
                                for (int j = lindex; j < rindex; j++)
                                    if (current_display_data_bottom[j % DATA_BUFFER_SIZE] > max) max = current_display_data_bottom[j % DATA_BUFFER_SIZE];
                            }
                        }

                        if (rx == 1) max += rx1_display_cal_offset;
                        else if (rx == 2) max += rx2_display_cal_offset;

                        if (!local_mox)
                        {
                            if (rx == 1) max += rx1_preamp_offset;
                            else if (rx == 2) max += rx2_preamp_offset;
                        }

                        if (max > local_max_y)
                        {
                            local_max_y = max;
                            max_x = i;
                        }

                        points[i].X = i;
                        points[i].Y = (int)(Math.Floor((spectrum_grid_max - max) * H / zoom_height / yRange));    //used to be 6
                        points[i].Y = Math.Min(points[i].Y, H);
                     
                        if (bottom) points[i].Y += H;
                    } // for loop out to W

                    if (pan_fill)
                    {
                        points[W].X = W; points[W].Y = (int)(H / zoom_height);
                        points[W + 1].X = 0; points[W + 1].Y = (int)(H / zoom_height);
                        if (bottom)
                        {
                            points[W].Y += H;
                            points[W + 1].Y += H;
                        }

                        data_line_pen.Color = DisplayPanFillColor; // was  Color.FromArgb(100, 255, 255, 255); // ke9ns draw white at 100

                        g.FillPolygon(data_line_pen.Brush, points);
                        points[W] = points[W - 1];
                        points[W + 1] = points[W - 1];

                        data_line_pen.Color = data_line_color;
                        g.DrawLines(data_line_pen, points);
                    }
                    else
                    {
                      
                        g.DrawLines(data_line_pen, points);  // g.DrawLines(data_line_pen, points);
                    }
                } // notches

                //-----------------------------------------------------------------------------------

                else if ((console.ZZOOM == true) )
                {

                    Debug.WriteLine("TNFZOOM9");

                    // draw zoom background box
                    if (bottom) g.FillRectangle(new SolidBrush(Color.FromArgb(230, 0, 0, 0)), 0, H, W, H / zoom_height);
                    else g.FillRectangle(new SolidBrush(Color.FromArgb(230, 0, 0, 0)), 0, 0, W, H / zoom_height);


                    // calculate data needed for zoomed notch
                    long rf_freq = vfoa_hz;
                    int rit = rit_hz;

                    if (bottom)
                    {
                        rf_freq = vfob_hz;
                    }

                    if (bottom)
                    {
                        switch (rx2_dsp_mode)
                        {
                            case (DSPMode.CWL):
                                rf_freq += cw_pitch;
                                break;
                            case (DSPMode.CWU):
                                rf_freq -= cw_pitch;
                                break;
                        }
                    }
                    else
                    {
                        switch (rx1_dsp_mode)
                        {
                            case (DSPMode.CWL):
                                rf_freq += cw_pitch;
                                break;
                            case (DSPMode.CWU):
                                rf_freq -= cw_pitch;
                                break;
                        }
                    }

                    int zoomed_notch_center_freq = (int)(notch_zoom_start_freq * 1e6 - rf_freq - rit);

                    int original_bw = High - Low;
                    int zoom_bw = original_bw / 10;

                    int low = zoomed_notch_center_freq - zoom_bw / 2;
                    int high = zoomed_notch_center_freq + zoom_bw / 2;

                    if (low < Low) // check left limit
                    {
                        low = Low;
                        high = Low + zoom_bw;
                    }
                    else if (high > High) // check right limit
                    {
                        high = High;
                        low = High - zoom_bw;
                    }

                    // decide colors to draw notch
                    c1 = notch_on_color_zoomed;
                    c2 = notch_highlight_color_zoomed;

                    if (!tnf_active)
                    {
                        c1 = notch_off_color;
                        c2 = Color.Black;
                    }
                    else if (notch.Permanent)
                    {
                        c1 = notch_perm_on_color;
                        c2 = notch_perm_highlight_color;
                    }
                    Debug.WriteLine("TNFZOOM99");

                 //   int notch_zoom_left_x = (int)((float)(notch.Freq * 1e6 - rf_freq - notch.BW / 2 - low - rit) / (high - low) * W);
                 //   int notch_zoom_right_x = (int)((float)(notch.Freq * 1e6 - rf_freq + notch.BW / 2 - low - rit) / (high - low) * W);

                  //  if (notch_zoom_left_x == notch_zoom_right_x)  notch_zoom_right_x = notch_zoom_left_x + 1;

                    //draw zoomed notch bars
                   // if (!bottom) drawNotchBar(g, notch, notch_zoom_left_x, notch_zoom_right_x, 0, (int)(H / zoom_height), c1, c2);
                  //  else drawNotchBar(g, notch, notch_zoom_left_x, notch_zoom_right_x, H, (int)(H / zoom_height), c1, c2);

                    // draw data
                    start_sample_index = (BUFFER_SIZE >> 1) + (int)((low * BUFFER_SIZE) / sample_rate);

                    num_samples = (int)((high - low) * BUFFER_SIZE / sample_rate);

                    if (start_sample_index < 0) start_sample_index += DATA_BUFFER_SIZE;

                    if ((num_samples - start_sample_index) > (BUFFER_SIZE + 1)) num_samples = BUFFER_SIZE - start_sample_index;

                    //Debug.WriteLine("start_sample_index: "+start_sample_index);
                    slope = (float)num_samples / (float)W;
                    //int grid_max = spectrum_grid_min + (spectrum_grid_max - spectrum_grid_min) / 2;

                    Debug.WriteLine("TNFZOOM999");

                    for (int i = 0; i < W; i++)
                    {
                        float max = float.MinValue;
                        float dval = i * slope + start_sample_index;

                        int lindex = (int)Math.Floor(dval);

                        int rindex = (int)Math.Floor(dval + slope);

                       
                        if (rx == 1)
                        {
                            if (slope <= 1.0 || lindex == rindex)
                            {
                                max = current_display_data[lindex % DATA_BUFFER_SIZE] * ((float)lindex - dval + 1) + current_display_data[(lindex + 1) % DATA_BUFFER_SIZE] * (dval - (float)lindex);
                            }
                            else
                            {
                                for (int j = lindex; j < rindex; j++)
                                    if (current_display_data[j % DATA_BUFFER_SIZE] > max) max = current_display_data[j % DATA_BUFFER_SIZE];
                            }
                        }
                        else if (rx == 2)
                        {
                            if (slope <= 1.0 || lindex == rindex)
                            {
                                max = current_display_data_bottom[lindex % DATA_BUFFER_SIZE] * ((float)lindex - dval + 1) + current_display_data_bottom[(lindex + 1) % DATA_BUFFER_SIZE] * (dval - (float)lindex);
                            }
                            else
                            {
                                for (int j = lindex; j < rindex; j++)
                                    if (current_display_data_bottom[j % DATA_BUFFER_SIZE] > max) max = current_display_data_bottom[j % DATA_BUFFER_SIZE];
                            }
                        }

                        if (rx == 1) max += rx1_display_cal_offset;
                        else if (rx == 2) max += rx2_display_cal_offset;

                        if (!local_mox)
                        {
                            if (rx == 1) max += rx1_preamp_offset;
                            else if (rx == 2) max += rx2_preamp_offset;
                        }

                        if (max > local_max_y)
                        {
                            local_max_y = max;
                            max_x = i;
                        }

                     

                        points[i].X = i;
                        points[i].Y = (int)(Math.Floor((spectrum_grid_max - max) * H / (zoom_height )/ yRange));    //used to be 6  / zoom_height 
                        points[i].Y = Math.Min(points[i].Y, H);

                        if (bottom) points[i].Y += H;
                    } // for loop out to W

                    if (pan_fill)
                    {
                        points[W].X = W; points[W].Y = (int)(H / zoom_height); // 
                        points[W + 1].X = 0; points[W + 1].Y = (int)(H / zoom_height);  // / zoom_height
                        if (bottom)
                        {
                            points[W].Y += H;
                            points[W + 1].Y += H;
                        }

                        data_line_pen.Color = DisplayPanFillColor; // was  Color.FromArgb(100, 255, 255, 255); // ke9ns draw white at 100

                        g.FillPolygon(data_line_pen.Brush, points);
                        points[W] = points[W - 1];
                        points[W + 1] = points[W - 1];

                        data_line_pen.Color = data_line_color;
                        g.DrawLines(data_line_pen, points);
                    }
                    else
                    {

                        g.DrawLines(data_line_pen, points);  // g.DrawLines(data_line_pen, points);
                    }
                } // ZZOOM == true



            }  // TNF ZOOM
#endif //TNF option

			points = null;



            //=========================================================
            // ke9ns add   
       
            if ((console.ScanForm != null) && (console.ScanForm.chkBoxIdent.Checked == true) && (rx == 1))
            {
  
                floor = (float)(max1 / W); // avg db value of the freq your on now
                IDENT_Sig = false;
                IDENT_Space = 0;   // end of this frame of display data so start over again
              
                IDENT_Reset = false; // any reset going on is now over

                int temp2 = (int)(Math.Floor((double)(spectrum_grid_max - (max1 / W)) * H / yRange));  // display normal
                floorB = temp2 = Math.Min(temp2, H); // returns smaller of the 2 numbers

                int temp3 = (int)(Math.Floor((double)(spectrum_grid_max - ((max1 / W) + IDENT_Thres)) * H / yRange));  // display normal
                temp3 = Math.Min(temp3, H); // returns smaller of the 2 numbers

                  g.DrawLine(IDENT_pen2, 0, temp2, 40, temp2); // Goldenrod these lines represent what is determined to be the noise floor
                  g.DrawLine(IDENT_pen2, W, temp2, W-40, temp2);

                  g.DrawLine(IDENT_pen, 0, temp3, 40, temp3); // PaleGreen these lines represent what is determined to be the noise floor
                  g.DrawLine(IDENT_pen, W, temp3, W - 40, temp3);

           
                if ((mox))
                {
                    IDENT_Reset = true;
                }


            } //  chkboxident



            //=====================================================================
            // draw long cursor
            //=====================================================================


            if (current_click_tune_mode != ClickTuneMode.Off)
			{
				Pen p;
				if(current_click_tune_mode == ClickTuneMode.VFOA)
					p = new Pen(grid_text_color);
				else p = new Pen(Color.Red);
				
                if(bottom)
				{
					if(display_cursor_y > H)
					{
						g.DrawLine(p, display_cursor_x, H, display_cursor_x, H+H);
						g.DrawLine(p, 0, display_cursor_y, W, display_cursor_y);
					}
				}
				else
				{
					if(display_cursor_y <= H)
					{
						g.DrawLine(p, display_cursor_x, 0, display_cursor_x, H);
						g.DrawLine(p, 0, display_cursor_y, W, display_cursor_y);
					}
				}
			}

			return true;

		}  // drawpanadapter


        
//=====================================================================
//=====================================================================

		private static int waterfall_update_period = 50; // in ms
		public static int WaterfallUpdatePeriod
		{
			get { return waterfall_update_period; }
			set { waterfall_update_period = value; }
		}


//=============================================================================
// ke9ns drawwaterfall
//  W,H = position of top of waterfall ?
// rx = receiver 1 or 2
// bottom = yes or no
//=============================================================================
        
        
		private static HiPerfTimer timer_waterfall = new HiPerfTimer();
		private static HiPerfTimer timer_waterfall2 = new HiPerfTimer();
		private static float[] waterfall_data;

    
        //WaterMove=5,2 you get scan0 301662208 total size 6351696 W 1123 bitmapData.Stride  16848  (.0000200 sec for black routine) (.0222519 for drawwaterfall )
        //waterMove=3,1 you get scan0 301662208 total size 6351696 W 1123 bitmapData.Stride  16848   (  black routine) (.0163125 for drawwaterfall) 

        private static int WaterMove = 3;  // 3,or 5 ke9ns ADD How many panels in watermove function, 0=OFF
        private static int WaterMove1 = 1;  //1, or 2 ke9ns ADD how many panels over to center starting point
        private static int WaterMove2 = 0;   // always W * 3 * WaterMove1

        private static int Wtemp = 0;     // ke9ns
        private static int WMtemp = 0;  // ke9ns

        private static int W2temp = 0;  // ke9ns
        private static int M2temp = 0;  // ke9ns
        private static int WM = W;      // ke9ns ADD WM is the watermove width of the bitmap. = W * WaterMove
        private static int WM1 = 0;     // ke9ns ADD tracks VFOA the +/- freq movement to shift the waterfall display bitmap on the screen
        private static int WM2 = 0;     // ke9ns ADD tracks VFOB the +/- freq movement to shift the waterfall display bitmap on the screen
        private static int WM1F = 0;    // ke9ns ADD rx1 draw shift
        private static int WM2F = 0;   // ke9ns ADD rx2 draw shift

        private static byte F2A = 0;   // ke9ns
        private static byte F2B = 0;  // ke9ns

        private static byte F3A = 0; // delay updating waterfall for scroll timing issues
        private static byte F3B = 0;  // ke9ns

        private static byte F5A = 0; // to take care of windows resizeing

        private static long WM2A = 0;     // ke9ns rx1  how many pixels to move the bmp frame -=going down in freq +=going up in freq
        private static long WM2B = 0;     // ke9ns rx2 how many pixels to move the bmp frame -=going down in freq +=going up in freq

        private static long WM2A_DIFF = 0;// ke9ns rx1
        private static long WM2B_DIFF = 0;// ke9ns rx2 

        private static long WM2A_LAST = 0;// ke9ns rx1
        private static long WM2B_LAST = 0;// ke9ns rx2

        private static float WM4 = 0;// ke9ns rx1 number of hz on screen
        private static float WM5 = 0;// ke9ns rx2 number of hz on screen

        // RX1
        private static long AB = 0; // ke9ns for autobright accumulator across a single W length line of data
        private static long[] AB1 = new long[10];// ke9ns
        private static float AB3 = 0; // ke9ns

        private static int AB_Peak = -200; // ke9ns peak signal for scaling small signals
        private static int AB_Count = 0; // ke9ns counter for determining peak signal
        private static int AB_Total = 0; // ke9ns sum total avg of peak

        // RX2
        private static long A2B = 0; // ke9ns for autobright
        private static long[] A2B1 = new long[10];// ke9ns
        private static float A2B3 = 0; // ke9ns

        // TX
        private static long A3B = 0; // ke9ns for autobright
        private static long[] A3B1 = new long[10];// ke9ns
        private static float A3B3 = 0;       // ke9ns
        private static int A4B = 0;          // ke9ns for autobright



        private static int itemp = 0; // ke9ns add continuum mode
        private static int itemp_last = 0; // ke9ns add continuum mode
        private static int timerflag = 0; // ke9ns add
        private static DateTime DT, DT1; // ke9ns add

        static Graphics g1; // 
        
        //================================================================================================================================

        unsafe static private bool DrawWaterfall(Graphics g, int W, int H, int rx, bool bottom)
		{

          //  Stopwatch stopWatch = new Stopwatch();
          //  stopWatch.Start();

            
            if (WaterMove == 0)   WM = W;  // standard old way 
            else   WM = WaterMove * W;  // ke9ns ADD  bitmap width (usually 3 times the W)  WM = 5 * 1123 = over 6000 pixels        
           
                    
            DrawWaterfallGrid(ref g, W, H, rx, bottom);  // ke9ns draw frequency text on line above waterfall

            // ke9ns add  this is used to keep track of rx thresholds when switching to tx threshold

            temp_low_threshold = waterfall_low_threshold;  // store original low rx1 threshold
            temp_high_threshold = waterfall_high_threshold; // store original high rx1/rx2 threshold


            //================================================
            // STEP 1) ke9ns change waterfall bitmap size to mode your in
            //==================================================

            if ((K9 != K9LAST)|| (K10 != K10LAST))  // check for only changes in display mode
            {
                K10LAST = K10;
                K9LAST = K9;
                K14 = 0;

                F2A = 1;  // reset values
                F2B = 1;

                F3A = 1;
                F3B = 1;

                WM2A_LAST = 0;
                WM2B_LAST = 0;
                WM2A = vfoa_hz; // ke9ns ADD freq vfoA when bitmap created
                WM2B = vfob_hz; // ke9ns ADD freq vfoB when bitmap created
       
            }


        // ke9ns add check for window resizing messing up bitmap size.

            if (DP == 1) // power on
            {
                F5A = 0; // power on, so reset counter to allow time
                DP = 0;
            }
            else // power off
            {
              // Debug.WriteLine("OFF   ");
             
            }
          
            if (F5A == 2)// if power turned ON, then wait 1 or 2 cycles before waterfall starts to allow vfo to update
            {
             //  Debug.WriteLine("RESET waterfall  ");
                F5A = 3;

                F2A = 1;
                F2B = 1;

                F3A = 1;
                F3B = 1;

                WM2A_LAST = 0;
                WM2B_LAST = 0;
                WM2A = vfoa_hz; // ke9ns ADD freq vfoA when bitmap created
                WM2B = vfob_hz; // ke9ns ADD freq vfoB when bitmap created

            }
            else if (F5A < 2)  F5A++; // allow time
          

            // K13 is original H size before H is divided and passed into this drawwaterfall routine
            
            if ((continuum == 1) && (split_display) && (K14 == 0))  // RX1:  1/2 water, RX2:  1/2 water
            {
                waterfall_bmp = new Bitmap(W, K13 / 2 - 16, WtrColor);	// initialize waterfall display
                waterfall_bmp2 = new Bitmap(WM, K13 / 2 - 16, WtrColor);  // ke9ns BMP
                K15 = 2;
                K14 = 1;
              //  Debug.Write("1   ");

            }
            else if ((continuum == 1) && (K14 == 0))
            {
                waterfall_bmp = new Bitmap(W, K13 - 16, WtrColor);  // initialize waterfall display for continuum which only is W wide since it does not move left to right
                waterfall_bmp2 = new Bitmap(WM, K13 - 16, WtrColor);  // ke9ns BMP
                K15 = 1;
                K14 = 1;
              //  Debug.Write("2   ");
            }
            else if ((K9 == 5) && (K10 == 1)&&(K14 == 0))  // RX1: 1/3 pan + 1/3 water, RX2: water 1/3
            {
                waterfall_bmp = new Bitmap(WM, K13/3 - 16, WtrColor);	// initialize waterfall display
                waterfall_bmp2 = new Bitmap(WM, K13/3 - 16, WtrColor);  // ke9ns BMP
                K15 = 3; // divide factor for the init() routine
                K14 = 1; // do only 1 time
            }
            else if ((K9 == 5) && (K10 == 5) && (K14 == 0))  // RX1: 1/4 pan + 1/4 water, RX2: pan 1/4 + 1/4 water
            {
                waterfall_bmp = new Bitmap(WM, K13/4 - 16, WtrColor);	// initialize waterfall display
                waterfall_bmp2 = new Bitmap(WM, K13/4 - 16, WtrColor);  // ke9ns BMP
                K15 = 4;
                K14 = 1;
            }
            else if ((K9 == 5) && (K10 == 2) && (K14 == 0))  // RX1: 1/3 pan + 1/3 water, RX2: pan 1/3
            {
                 waterfall_bmp = new Bitmap(WM, K13/3 - 16, WtrColor);  // initialize waterfall display
                 // waterfall_bmp2 = new Bitmap(W, K13/3 - 16, PixelFormat.Format24bppRgb);  // ke9ns BMP
                K15 =  3;
                K14 = 1;
            }
            else if ((K9 == 5) && (K10 == 3) && (K14 == 0))  // RX1: 1/4 pan + 1/4 water, RX2: pan 1/4 + 1/4 water
            {
                waterfall_bmp = new Bitmap(WM, K13 / 4 - 16, WtrColor);	// initialize waterfall display
                waterfall_bmp2 = new Bitmap(WM, K13 / 4 - 16, WtrColor);  // ke9ns BMP
                K15 =  4;
                K14 = 1;
            }
            else if ((split_display) && (K9 == 6) && (K10 == 1) && (K14 == 0))  // RX1:  1/2 water, RX2:  1/2 water
            {
                waterfall_bmp = new Bitmap(WM, K13/2  - 16, WtrColor);	// initialize waterfall display
                waterfall_bmp2 = new Bitmap(WM, K13/2 - 16, WtrColor);  // ke9ns BMP
                K15 = 2;
                K14 = 1;
            }
            else if ((split_display) && (K9 == 2) && (K10 == 1) && (K14 == 0))  // RX1:  1/2 pan, RX2:  1/2 water
            {
               // waterfall_bmp = new Bitmap(WM, K13 - 16, PixelFormat.Format24bppRgb);	// initialize waterfall display
                waterfall_bmp2 = new Bitmap(WM, K13/2 - 16, WtrColor);  // ke9ns BMP
                K15 =  2;
                K14 = 1;
              
            }
            else if ((split_display) && (K9 == 6) && (K10 == 2) && (K14 == 0))  // RX1:  1/2 water, RX2:  1/2 pan
            {
                waterfall_bmp = new Bitmap(WM, K13/2 - 16, WtrColor);	// initialize waterfall display
                waterfall_bmp2 = new Bitmap(WM, K13/2 - 16, WtrColor);  // ke9ns BMP
                K15 =  2;
                K14 = 1;
            }
            else if ((K9 == 1) && (K14 == 0))// waterfall on rx1 only
            {
                waterfall_bmp = new Bitmap(WM, K13 - 16, WtrColor);	// initialize waterfall display
                waterfall_bmp2 = new Bitmap(WM, K13 - 16, WtrColor);  // ke9ns BMP
             //   waterfall_bmp.MakeTransparent(Color.FromArgb(0,0, 0, 0)); // ke9ns test

                K15 = 1;
                K14 = 1;
            }
            else if ((K9 == 3) && (K14 == 0))// waterfall on rx1 only
            {
               
                waterfall_bmp = new Bitmap(WM, K13 - 16, WtrColor);	// initialize waterfall display
                waterfall_bmp2 = new Bitmap(WM, K13 - 16, WtrColor);  // ke9ns BMP
            
                K15 = 1;
                K14 = 1;
                 
            }
            else if ((K9 == 7) && (K14 == 0))// waterfall on rx1 only
            {

                waterfall_bmp = new Bitmap(WM, K13/4 - 16, WtrColor);	// initialize waterfall display
            
                //  waterfall_bmp2 = new Bitmap(WM, K13 - 16, WtrColor);  // ke9ns BMP
                                                                      // waterfall_bmp.MakeTransparent(Color.FromArgb(0,0,0,0)); // ke9ns test

                K15 = 1;
                K14 = 1;

            }

            else if ((K14 == 0))// all else
            {
                waterfall_bmp = new Bitmap(WM, K13 - 16, WtrColor);	// initialize waterfall display
                waterfall_bmp2 = new Bitmap(WM, K13 - 16, WtrColor);  // ke9ns BMP
                K15 = 1;
                K14 = 1;
            }



            //================================================
            // STEP 2) set up waterfall
            //==================================================

            if (waterfall_data == null || waterfall_data.Length < W)
            {
                waterfall_data = new float[W];      // array of points to display
            }

			float slope = 0.0F;						// samples to process per pixel
			int num_samples = 0;					// number of samples to process
			int start_sample_index = 0;             // index to begin looking at samples based on left side low freq and right side high freq

            int Low;
            int High;

            if (Console.UPDATEOFF > 0)
            {
                Low = LowLast;
                High = HighLast;
             
            }
            else
            {
                Low = rx_display_low;               // low freqency on display window (left side)
                High = rx_display_high;             // high freqency on display window (right side)

                LowLast = Low;
                HighLast = High;
            }
         
            //  Low = rx_display_low;               // left side low freq
          //  High = rx_display_high;             // right side high freq


            int yRange = spectrum_grid_max - spectrum_grid_min;   // grid range  (around 120db)

           
			float local_max_y = float.MinValue;     // top of waterfall
            bool local_mox = false;                 // transmitting or not

            if (rx == 1 && !tx_on_vfob && mox) local_mox = true;  // check rx1 if transmitting on rx1

            if (rx == 2 && tx_on_vfob && mox) local_mox = true;   // check rx2 if transmitting on rx2
			
			if((rx1_dsp_mode == DSPMode.DRM && rx==1) || (rx2_dsp_mode == DSPMode.DRM && rx==2))
			{
				Low += 12000;   // shift left and right freq in DRM mode up 12k ???
				High += 12000;
			}

            //================================================
            // STEP 3) GET RX1 & RX2 FRAME of DATA (new_display_data)
            //         copy it into current_display_data so it does not get clobbered
            //==================================================

         
            if (rx==1 && data_ready && Console.UPDATEOFF == 0 ) // ke9ns mod
			{
				if(local_mox && (rx1_dsp_mode == DSPMode.CWL || rx1_dsp_mode == DSPMode.CWU))
				{
					for(int i=0; i < current_display_data.Length; i++)
                        current_display_data[i] = spectrum_grid_min - rx1_display_cal_offset;
				}
				else
				{

                    if (((pw_avg & 1) == 0) || (current_display_mode == DisplayMode.WATERFALL)) // water avg on
                    {
                        fixed (void* rptr = &new_display_data[0])            // new display data waterfall
                        {
                            fixed (void* wptr = &current_display_data[0])
                            {
                                Win32.memcpy(wptr, rptr, BUFFER_SIZE * sizeof(float)); // copy new data into the current buffer
                            }
                        }
                    }
                    else  // water avg off
                    {
                        fixed (void* rptr = &new_display_data[0])            // this avoids averaging
                        {
                            fixed (void* wptr = &current_display_data1[0])
                            {
                                Win32.memcpy(wptr, rptr, BUFFER_SIZE * sizeof(float)); // ke9ns add copy new data into the current buffer
                            }
                        }

                    }

                    if ( current_model == Model.SOFTROCK40 ) 
						console.AdjustDisplayDataForBandEdge(ref current_display_data);
				}
				data_ready = false;
			} // RX1
			else if(rx==2 && data_ready_bottom )
			{

               
                if (local_mox && (rx2_dsp_mode == DSPMode.CWL || rx2_dsp_mode == DSPMode.CWU))
                {
                    for (int i = 0; i < current_display_data_bottom.Length; i++)
                        current_display_data_bottom[i] = spectrum_grid_min - rx2_display_cal_offset;
                }
                else
                {

                    if (((pw_avg2 & 1) == 0) || (current_display_mode_bottom == DisplayMode.WATERFALL)) // water avg on
                    {
                        fixed (void* rptr = &new_display_data_bottom[0])
                        {
                            fixed (void* wptr = &current_display_data_bottom[0])
                            {
                                Win32.memcpy(wptr, rptr, BUFFER_SIZE * sizeof(float)); // copy new data into the current buffer (to, from , #bytes)
                            }
                        }

                    }
                    else  // water avg off
                    {
                        fixed (void* rptr = &new_display_data_bottom[0])
                        {
                            fixed (void* wptr = &current_display_data_bottom1[0])
                            {
                                Win32.memcpy(wptr, rptr, BUFFER_SIZE * sizeof(float)); // ke9ns add this avoids averaging
                            }
                        }

                    }
                    if (current_model == Model.SOFTROCK40)
                        console.AdjustDisplayDataForBandEdge(ref current_display_data_bottom);
                }

				data_ready_bottom = false;
			} // RX2

       //================================================
       // STEP 4) More Waterfall setup including timers for how fast to move the waterfall and for avgB or avgP (which is also found in drawpanadapter)
       //==================================================
      
            if (current_display_mode != DisplayMode.PANAFALL) // don't do twice - in panafall avg already done in DrawPanadapter (ke9ns : if just RX1 water or RX2 water come here)
            {
                // RX1 must be in waterfall only mode

                if (rx == 1 && average_on)
                {
                    console.UpdateRX1DisplayAverage(rx1_average_buffer, current_display_data);
                }
                else if ((rx == 2) && (rx2_avg_on) )
                {
                    console.UpdateRX2DisplayAverage(rx2_average_buffer, current_display_data_bottom);
                }
                if (rx == 1 && peak_on)
                {
                    UpdateDisplayPeak(rx1_peak_buffer, current_display_data);
                }
                else if (rx == 2 && rx2_peak_on)
                {
                    UpdateDisplayPeak(rx2_peak_buffer, current_display_data_bottom);
                }

            } // RX1 in WATERFALL MODE
            else if (current_display_mode_bottom == DisplayMode.WATERFALL) // ke9ns add  this assume you came here because RX1 is in PANAFALL mode
            {
                 if (rx == 2 && rx2_avg_on)
                {
                    console.UpdateRX2DisplayAverage(rx2_average_buffer, current_display_data_bottom);
                }
                else if (rx == 2 && rx2_peak_on)
                {
                    UpdateDisplayPeak(rx2_peak_buffer, current_display_data_bottom);
                }
            } // RX2 in WATERFALL MODE

			int duration = 0;

			if(rx == 1)
			{
				timer_waterfall.Stop();
				duration = (int)timer_waterfall.DurationMsec;
			}
			else if(rx == 2)
			{
				timer_waterfall2.Stop();
				duration = (int)timer_waterfall2.DurationMsec;
			}


            //================================================
            // STEP 5) wait until its time to update waterfall with new line at top of bmp
            //==================================================


            if ((duration > waterfall_update_period) &&  console.chkPower.Checked)
			{
				if(rx == 1) timer_waterfall.Start();
				else if(rx == 2) timer_waterfall2.Start();

				num_samples = (High - Low);     // right side high freq - left side low freq

				start_sample_index = (BUFFER_SIZE >> 1) +(int)((Low * BUFFER_SIZE) / sample_rate);

				num_samples = (int)((High - Low) * BUFFER_SIZE / sample_rate);  // same as in panadapter draw

				if (start_sample_index < 0) start_sample_index += DATA_BUFFER_SIZE;

				if ((num_samples - start_sample_index) > (BUFFER_SIZE + 1)) num_samples = BUFFER_SIZE - start_sample_index;

				slope = (float)num_samples/(float)W;               // find number of samples / pixel (zoom x4 = .26 @ 192k) (zoom .5 = 2.3 @ 192k)



                //=================================================================
                // STEP 5a) ke9ns ADD compute offset in BMP
                //=================================================================

                WaterMove2 = W * 3 * WaterMove1; // find the absolute center of the bitmap row[WaterMove2] array specifically 
                                                    // 3 bytes per pixel
                

                if (rx == 1)
                {
                    if (F2A == 2)  // wait for freq to be set and 1 extra cycle because of the timing between freq set and getting here first.
                    {
                        if (((WM2A_LAST != vfoa_hz) && (Console.CTUN1_HZ == 0)) || (WM2A_LAST < (vfoa_hz-200000) || WM2A_LAST > (vfoa_hz + 200000))  ) // ke9ns dont move waterfall if in CTUN mode
                        {

                            if (F3A == 2) // routine to delay 2 cycle updating the watermove because of timing error. Also RESIZE resets this
                            {
                                F3A = 0;

                                WM2A_LAST = vfoa_hz;               // new last value

                                WM2A_DIFF = WM2A_LAST - WM2A; //  difference from original spot in bmp

                                WM4 = (float)((float)(High - Low) / (float)num_samples); // number of hz on screen

                                WM1 = (int)((float)WM2A_DIFF / WM4 / slope); // how many pixels to move the bmp frame -=going down in freq +=going up in freq

                             //    Debug.WriteLine("wm1 " + WM1);// pixels at this point not row[bytes] 
                             //     Debug.WriteLine("watermove2 " + WaterMove2);
                              //     Debug.WriteLine("W " + W);

                                if ((WM1 >= ((W*(WaterMove1))-2 )) || (WM1 <= (-(W*WaterMove1)+2 ))) // either you move W beyond or -W below
                                {
                                    K9LAST = 0; // redraw bitmap
                                    WM1 = 0;
                                  //  Debug.WriteLine("===================OVER EDGE=========================");

                                } // reset the bitmap

                                WM1F = WM1;  // for drawimage
                                WM1 = (WM1F * 3) + WaterMove2; // for row[wm1]  for RGB (WM1F * 3) + (3 * W);
                                                                    // this adds in the 3bytes per pixel  (24bit mode RGB)

                            } // F3A
                            else   F3A++;

                             Wtemp = (W * 3) / 4;
                             WMtemp = (W * 3 * WaterMove) / 4;// used by black routine

                        } // (WM2A_LAST != vfoa_hz)


                    } // F2A == 2
                    else // set new freq but wait before moving
                    {
                        F2A++;
                       WM1 = (WM1F *3) + WaterMove2; // for RGB(WM1F * 3) + (3 * W)
                   }
                 

                } //rx == 1
                else  // rx2
                {

                  
                      if (F2B == 2)                // wait for freq to be set and 1 extra cycle because of the timing between freq set and getting here first.
                      {
                   
                            if ((WM2B_LAST != vfob_hz))
                            {

                            if (F3B == 2) // routine to delay 1 cycle updating the watermove because of timing error.
                            {

                                F3B = 0;

                                WM2B_LAST = vfob_hz; // new last value

                                WM2B_DIFF = WM2B_LAST - WM2B; //  difference from original spot in bmp

                                WM5 = (float)((float)(High - Low) / (float)num_samples); // number of hz on screen

                                WM2 = (int)((float)WM2B_DIFF / WM5 / slope); // how many pixels to move the bmp frame -=going down in freq +=going up in freq

                                //  Debug.WriteLine("wm2 " + WM2);
                                //    Debug.WriteLine("wm5 " + WM5);
                                //    Debug.WriteLine("WM2B " + WM2B);

                                if ((WM2 >= ((W * WaterMove1)-2)) || (WM2 <= (-(W * WaterMove1)+2)))  // either you move W beyond or -W below
                                {
                                    K10LAST = 0; // redraw bitmap
                                    WM2 = 0;

                                } // reset the bitmap

                                // this is the position to place the upper left corner of the 3W bitmap so WM2 appears in the upper left corner of the display window 
                                // WM2F = the pixel offset from the original freq when the bitmap was created.

                                WM2F = WM2;

                                // final WM2 = Position within the 3W bitmap that you start writing too.
                                // WM2F*3 = # of RGB pixels to shift +/-
                                // 3*w = start at the center of the 3Wide bitmap
                                WM2 = (WM2F *3) + WaterMove2; //(WM2F * 3) + (3 * W);

                            } // F3B
                            else F3B++;

                             W2temp = (W * 3) / 4;
                             M2temp = (W * 3 * WaterMove) / 4; // used by black routine

                        } // vfoB moved in freq
                    }
                    else
                    {
                        F2B++;
                        WM2 = (WM2F * 3) + WaterMove2; // for RGB (WM2F * 3) + (3 * W);
                    }

                } // rx2


             //   Debug.WriteLine("pw2 " + pw_avg2 + " pw1 "+pw_avg);  // water avg on)


            //================================================
            // STEP 6) convert 1 line of RX current data into a buffer waterfall_data[] W = width of the viewing area in pixels
            //==================================================

                A3B = A2B = AB = 0;    // ke9ns auto brightness
                A4B = 1;               // ke9ns add tx counter
               

                if ((rx ==1) && (continuum == 1)) // ke9ns add (get the current Peak dbm value and convert to pixel location left=waterfall_low_threshold , right=waterall_high_threshold
                {
                    if ((MaxY < 20)&& (MaxY > -150) )
                    {  
                        itemp = (int)(150 - Math.Abs(MaxY)) ; // ke9ns add reverse db for plotting
                        itemp = itemp * (int)((float)W / (float)150);
                    }
                    else
                    {
                        // use last itemp value.
                    }

                    WM1 = 0; // ke9ns no water move while in continuum mode
                    Pen p = new Pen(Color.AntiqueWhite, 1);                // pen color white

                    g1 = Graphics.FromImage(waterfall_bmp); // ke9ns add get access to waterfall bitmap to write timestamp into it.
                    System.Drawing.Font font = new System.Drawing.Font("Swis721 BT", 9, FontStyle.Italic); // Arial size and style of freq text for waterfall
                    SolidBrush grid_text_brush = new SolidBrush(grid_text_color);
                  

                    if (timerflag == 0) // set timer for every 5 seconds
                    {
                        DT1 = DT = DateTime.Now;
                        timerflag = 1;
                    }
                    else
                    {
                        DT1 = DateTime.Now;
                    }

                 
                 //   Debug.Write("diff "+ DT1.Subtract(DT).TotalSeconds);
                  // Debug.Write("ts " + ts);

                    if ((DT1.Subtract(DT).TotalSeconds) >= 5)
                    {
                         g1.DrawString(string.Format("{0:yyyy-MM-dd_hh-mm-ss}", DateTime.Now), font, grid_text_brush, 1, 10); // ke9ns add continuum
                         timerflag = 0;
                    }

                 //   g1.DrawLine(p,itemp,1,itemp_last,0); // ke9ns draw line
                 //   itemp_last = itemp;


                } // in continuum mode

             
                //========================================================================

                for (int i=0; i < W; i++)
				{
					float max = float.MinValue;                      // storage for actual RX value 
					float dval = i * slope + start_sample_index;    // same as in draw panadapter
					int lindex = (int)Math.Floor(dval);
					int rindex = (int)Math.Floor(dval + slope);

					if(rx==1)
					{
						if (slope <= 1.0 || lindex == rindex)  // means high zoom level which means zooming into not enough resolution
						{
                            // find location in current display buffer to represent location on screen (left to right)
                            // less than 1 sample per pixel

                            if ( ((pw_avg & 1) == 0) ||  (current_display_mode == DisplayMode.WATERFALL) )  // water avg on
                            {
                                max = current_display_data[lindex % DATA_BUFFER_SIZE] * ((float)lindex - dval + 1) + current_display_data[(lindex + 1) % DATA_BUFFER_SIZE] * (dval - (float)lindex);
                            }
                            else
                            {
                                max = current_display_data1[lindex % DATA_BUFFER_SIZE] * ((float)lindex - dval + 1) + current_display_data1[(lindex + 1) % DATA_BUFFER_SIZE] * (dval - (float)lindex);
                            }
                        }
						else  // at least over 1 sample per pixel
						{
                            if (((pw_avg & 1) == 0) || (current_display_mode == DisplayMode.WATERFALL)) // water avg on
                            {
                                for (int j = lindex; j < rindex; j++)
                                {
                                    if (current_display_data[j % DATA_BUFFER_SIZE] > max) max = current_display_data[j % DATA_BUFFER_SIZE];
                                }
                            }
                            else
                            {
                                for (int j = lindex; j < rindex; j++)
                                {
                                    if (current_display_data1[j % DATA_BUFFER_SIZE] > max) max = current_display_data1[j % DATA_BUFFER_SIZE];
                                }
                            }
						}
					} // rx1
					else if(rx==2)
					{
						if (slope <= 1.0 || lindex == rindex) 
						{
                            if (((pw_avg2 & 1) == 0) || (current_display_mode_bottom == DisplayMode.WATERFALL))  // water avg on
                            {
                                max = current_display_data_bottom[lindex % DATA_BUFFER_SIZE] * ((float)lindex - dval + 1) + current_display_data_bottom[(lindex + 1) % DATA_BUFFER_SIZE] * (dval - (float)lindex);
                            }
                            else // water avg off
                            {
                                max = current_display_data_bottom1[lindex % DATA_BUFFER_SIZE] * ((float)lindex - dval + 1) + current_display_data_bottom1[(lindex + 1) % DATA_BUFFER_SIZE] * (dval - (float)lindex);
                            }

                        }
                        else 
						{
                            if ( ((pw_avg2 & 1) == 0) || (current_display_mode_bottom == DisplayMode.WATERFALL)) // water avg on
                            {
                                for (int j = lindex; j < rindex; j++)
                                    if (current_display_data_bottom[j % DATA_BUFFER_SIZE] > max) max = current_display_data_bottom[j % DATA_BUFFER_SIZE];
                            }
                            else
                            {
                                for (int j = lindex; j < rindex; j++)
                                    if (current_display_data_bottom1[j % DATA_BUFFER_SIZE] > max) max = current_display_data_bottom1[j % DATA_BUFFER_SIZE];
                            }
                        }
					} // rx2

					if(rx==1) max += rx1_display_cal_offset;
					else if(rx==2) max += rx2_display_cal_offset;

					if(!mox) 
					{
						if(rx==1) max += rx1_preamp_offset;         // adjust for preamp on/off
						else if(rx==2) max += rx2_preamp_offset;
					}

					if(max > local_max_y)
					{
						local_max_y = max;
						max_x = i;
					}

                    

                    if ((continuum == 0) || (rx==2)) // ke9ns add special Peak Power db shown vs Time
                    {
                                                                            // ke9ns this is the actual brightness data per pixel to draw to the screen (either color or gray)
                        waterfall_data[i] = max;                            // RX signal strength per pixel of data 0 to i = 0 to W wide (1 line of waterfall data)
                    }
                    else // continuum mode here
                    {
                        if (rx == 1)
                        {

                            //   Debug.Write("last " + itemp_last + " itemp " + itemp);


                            waterfall_data[i] = waterfall_low_threshold; // ke9ns add no line shown

                            
                            if (itemp > itemp_last)
                            {
                                if ((i <= itemp) && (i >= itemp_last))
                                {
                                    waterfall_data[i] = waterfall_high_threshold; // ke9ns add draw line showing peak power per time
                                }
                                else
                                {
                                  waterfall_data[i] = waterfall_low_threshold; // ke9ns add no line shown
                                }

                            }
                            else // itemp < itemp_last
                            {
                                if ((i >= itemp) && (i <= itemp_last))
                                {
                                    waterfall_data[i] = waterfall_high_threshold; // ke9ns add draw line showing peak power per time
                                }
                                else
                                {
                                   waterfall_data[i] = waterfall_low_threshold; // ke9ns add no line shown
                                }

                            }
                           

                        } // rx == 1
                       
                    } // continuum mode here

                    //-----------------------------------------------------
                    // ke9ns add collect data to average for auto bright 

                    if (autobright == 1) // RX1
                    {
                        if ((!mox)  && (rx == 1)) AB = AB + (long)max; // ke9ns add autobright feature 

                    }
                    else if (autobright2 == 1) // RX2
                    {
                        if ((!mox) && (rx == 2)) A2B = A2B + (long)max; // ke9ns add autobright feature 
                    }
                    else if (autobright3 == 1)  // TX
                    {
                        if (mox)
                        {

                          //  Debug.WriteLine("VALS ==  " + max);

                            if (max > -100)  // use only mic data
                            {
                                A3B = A3B + (long)max; // ke9ns add autobright feature 
                                A4B++;
                            }


                        }

                    } // autobright3 = 1


                } // for i loop (W wide)

               if ((continuum == 1) && (rx == 1))  itemp_last = itemp; // ke9ns add used to draw to screen
               
               
               
                //=========================================================================
                // ke9ns add autobright (auto water level) feature
                //=========================================================================

                if (mox) // transmit here
                {
                    if (autobright3 == 1) // tx adjust
                    {
                        A3B1[0] = A3B / A4B; // get avg of the entire read

                        A3B3 = (float)(A3B1[0]);

                        autobright = autobright2 = autobright3 = 0; // turn off feature


                        if ((A3B3 > -100) || (A3B3 < -10))
                        {
                            WaterfallLowMicThreshold = A3B3 - abright - (wateroffset-20); //  console.setupForm.WaterfallLowMicThreshold
                            console.setupForm.udDisplayWaterfallMicLevel.Value = (decimal)WaterfallLowMicThreshold;
                        }
                        //  Debug.WriteLine("TX value " + A3B3);
                        //   console.setupForm.udDisplayWaterfallRX2Level.Invalidate();

                    } // autobright3 = 1

                }
                else if (rx == 1) // RX1 receive here
                {
                    if (autobright == 1) // rx1 adjust
                    {
                        AB1[0] = AB / W; // get avg of the entire read

                        AB3 = (float)(AB1[0]);

                        autobright3 = autobright = 0; // turn off feature

                        if ((AB3 > -170) && (AB3 < -50))
                        {
                            console.setupForm.WaterfallLowThreshold = temp_low_threshold = WaterfallLowThreshold = AB3 - abright - (wateroffset - 20);
                            console.setupForm.udDisplayWaterfallLowLevel.Value = (decimal)WaterfallLowThreshold;

                            //  console.setupForm.udDisplayWaterfallLowLevel.Invalidate();
                        }
                        //  Debug.WriteLine("rx1 value " + AB3);

                    } // autobright = 1

                }
                else if (rx == 2)
                {

                    if (autobright2 == 1)  // rx2 adjust
                    {
                        A2B1[0] = A2B / W; // get avg of the entire read


                        A2B3 = (float)(A2B1[0]);

                        autobright3 = autobright2 = 0; // turn off feature

                        if ((A2B3 > -140) && (A2B3 < -50))
                        {
                            console.setupForm.WaterfallLowRX2Threshold = WaterfallLowRX2Threshold = A2B3 - abright - (wateroffset - 20);
                            console.setupForm.udDisplayWaterfallRX2Level.Value = (decimal)WaterfallLowRX2Threshold;
                        }
                         //  Debug.WriteLine("rx2 value " + A2B3 );
                        //   console.setupForm.udDisplayWaterfallRX2Level.Invalidate();

                    } // autobright2 = 1
                } // rx 2

  

                //================================================
                // Process waterfall_data[] into line of bmp data
                //==================================================

                if (!bottom) max_y = local_max_y;  // ke9ns   if top half then max y


                BitmapData bitmapData;  //  specifies attributes of image

        //=================================================================
        // STEP 7) This LOCKS the screen as-is so you can copy, modify, copy back and display later
        //=================================================================

                if (rx == 1) 
				{
                    
                     bitmapData = waterfall_bmp.LockBits(new Rectangle(0, 0,  waterfall_bmp.Width,  (waterfall_bmp.Height)), 
                        ImageLockMode.ReadWrite,  waterfall_bmp.PixelFormat);
				}
				else
				{
					bitmapData = waterfall_bmp2.LockBits(new Rectangle(0, 0,  waterfall_bmp2.Width,  waterfall_bmp2.Height), 
                        ImageLockMode.ReadWrite,  waterfall_bmp2.PixelFormat);
				}

				int pixel_size = 3;       // step size through bitmap 3 bytes per pixel for color RGB or grayscal 
                                          // but now used as a holder for the pixel location in the row
				
                byte* row = null;       // row as a pointer
                int* row1 = null;

                //=================================================================
                // STEP 8) Copy the entire bitmap down 1 line 
                // which makes the water appear to FALL, but current top line still has last old data in it
                //=================================================================

                int total_size = bitmapData.Stride * bitmapData.Height;                 // find buffer size (stride = scan width) ke9ns height = 281 or 525 full screen 

               Win32.memcpy(
                   new IntPtr((int)bitmapData.Scan0 + (bitmapData.Stride)).ToPointer(),  // + stride is 1 row down
                 bitmapData.Scan0 .ToPointer(),
                   total_size - bitmapData.Stride
                   );  // copy (dest, source, count)

              
                   row = (byte*)bitmapData.Scan0;     // ke9ns  first row but W over to the right since the Width of the bmp is 3times the real viewing area
                   row1 = (int*)bitmapData.Scan0;     // ke9ns used for faster clearing out of bad data

              //  Debug.WriteLine("scan0 " + bitmapData.Scan0 + " total size " + total_size + " W " + W +" bitmapData.Stride  "+bitmapData.Stride);
              
                
                //=================================================================
                // ke9ns ADD BLACK or CLEAR out the part of the bitmap that is not on the screen
                //=================================================================

                if (WaterMove != 0) // do if waterfall move option is turned ON 
                {

                   
                        if ((rx == 1) && (continuum == 0))  // RX1
                        {

                            int WM1temp = WM1 >> 2;

                            for (int i = 0; i < WM1temp; i++)               //  wm1 represents number of bytes away from center of bitmap
                            {
                                row1[i] = 0; // 4 bytes long
                            }
                            for (int i = WM1temp + Wtemp; i < WMtemp; i++) //sca
                            {
                                row1[i] = 0; // 4 bytes long
                            }

                        }
                        else if (rx==2) // for RX2
                        {
                            int WM2temp = WM2 >> 2;

                            for (int i = 0; i < WM2temp; i++)              //  wm1 represents number of bytes away from center of bitmap
                            {
                                row1[i] = 0; // 4 bytes long
                            }
                            for (int i = WM2temp + W2temp; i < M2temp; i++) //
                            {
                                row1[i] = 0; // 4 bytes long
                            }


                        }// rx2
                   

                } // watermove

                //=================================================================
                // ke9ns choose a threshold for RX or TX
                //=================================================================
                if (local_mox)
                {
                    if ((tx_on_vfob) && (rx == 2))
                    {
                        waterfall_low_threshold = waterfall_lowMic_threshold;  // TX low level db
                        waterfall_high_threshold = 0;

                    }
                    else if ((!tx_on_vfob) && (rx == 1))
                    {
                        waterfall_low_threshold = waterfall_lowMic_threshold; // TX low level
                        waterfall_high_threshold = 0;
                    }
                    else
                    {
                        waterfall_low_threshold = -200; // if you dont have a low level use -200
                     //   Debug.WriteLine("never ");
                    }

                }
                else  // if in RX mode
                {
                    if (rx == 1)
                    {
                        waterfall_low_threshold = temp_low_threshold;  // rx1 db
                        waterfall_high_threshold = temp_high_threshold;  // rx1 db
                    }
                    else
                    {
                        waterfall_low_threshold = waterfall_lowRX2_threshold; // rx2
                        waterfall_high_threshold = temp_high_threshold;  // rx1 db
                    }
            
                }



        //============================================
        // convert to RGB or Grayscale data
        // draw new line of data (left to right)
        //============================================

                float range = waterfall_high_threshold - waterfall_low_threshold; // diff in high to low db values

                int Maxcolor = 255; // 255


                if (Gray_Scale == 0) // RGB
                {
                                      
                    for (int i = 0; i < W; i++) // for each pixel in the new line
                    {
                        int R, G, B;        // variables to save Red, Green and Blue component values
                      

                        if (waterfall_data[i] <= waterfall_low_threshold) // if RX strength of sample is below low threshold, only go down to low end of color
                        {
                            R = waterfall_low_color.R;  // default of black by default which would be zero for all 3 colors
                            G = waterfall_low_color.G;
                            B = waterfall_low_color.B;
                        }
                        else if (waterfall_data[i] >= waterfall_high_threshold) // if strength exceeds high value, then dont go beyond max color values
                        {
                            R = 192;            // 192
                            G = 124;       // 124     
                            B = Maxcolor; // 255
                        }
                        else // value is between low and high
                        {
                            //  float range = waterfall_high_threshold - waterfall_low_threshold;  // found up above now

                            float offset = waterfall_data[i] - waterfall_low_threshold;

                            float overall_percent = offset / range; // value from 0.0 to 1.0 where 1.0 is high and 0.0 is low.

                            if (overall_percent < (float)2 / 9) // background to blue
                            {
                                float local_percent = overall_percent / ((float)2 / 9);
                                R = (int)((1.0 - local_percent) * waterfall_low_color.R);
                                G = (int)((1.0 - local_percent) * waterfall_low_color.G);
                                B = (int)(waterfall_low_color.B + local_percent * (Maxcolor - waterfall_low_color.B));
                            }
                            else if (overall_percent < (float)3 / 9) // blue to blue-green
                            {
                                float local_percent = (overall_percent - (float)2 / 9) / ((float)1 / 9);
                                R = 0;
                                G = (int)(local_percent * Maxcolor);
                                B = Maxcolor;
                            }
                            else if (overall_percent < (float)4 / 9) // blue-green to green
                            {
                                float local_percent = (overall_percent - (float)3 / 9) / ((float)1 / 9);
                                R = 0;
                                G = Maxcolor;
                                B = (int)((1.0 - local_percent) * Maxcolor);
                            }
                            else if (overall_percent < (float)5 / 9) // green to red-green
                            {
                                float local_percent = (overall_percent - (float)4 / 9) / ((float)1 / 9);
                                R = (int)(local_percent * Maxcolor);
                                G = Maxcolor;
                                B = 0;
                            }
                            else if (overall_percent < (float)7 / 9) // red-green to red
                            {
                                float local_percent = (overall_percent - (float)5 / 9) / ((float)2 / 9);
                                R = Maxcolor;
                                G = (int)((1.0 - local_percent) * Maxcolor);
                                B = 0;
                            }
                            else if (overall_percent < (float)8 / 9) // red to red-blue
                            {
                                float local_percent = (overall_percent - (float)7 / 9) / ((float)1 / 9);
                                R = 255;
                                G = 0;
                                B = (int)(local_percent * Maxcolor);
                            }
                            else // red-blue to purple end
                            {
                                float local_percent = (overall_percent - (float)8 / 9) / ((float)1 / 9);
                                R = (int)((0.75 + 0.25 * (1.0 - local_percent)) * Maxcolor);
                                G = (int)(local_percent * Maxcolor * 0.5);
                                B = Maxcolor;
                            }
                        } // // value is between low and high



                        // set pixel color
                        //  if (WaterMove == 0)wm1
                        //  {
                        //    pixel_size = (i * 3);
                       //     row[ pixel_size + 0] = (byte)B;  // set color in memory
                       //     row[ pixel_size + 1] = (byte)G;
                       //     row[ pixel_size + 2] = (byte)R;
                       //  }
                       //  else
                       //  {

                            if (rx == 1)
                            {

                                pixel_size = (i * 3) + WM1; // 

                                row[ pixel_size + 0 ] = (byte)B;  //  ke9ns ADD draw new image only in the center portion of the 3 Wide bitmap
                                row[ pixel_size + 1 ] = (byte)G; //  WM1 = number of pixels to offset for the part of the 3Wide bitmap that is actually in the display area
                                row[ pixel_size + 2 ] = (byte)R; //  WM1 = (WM1F * 3) + (3 * W); where 3W=start at center of bitmap, 3WM1F=+/- pixel shift in the bitmap

                            }
                            else
                            {
                                pixel_size = (i * 3) + WM2;

                                row[ pixel_size + 0 ] = (byte)B;  //  ke9ns ADD draw new image only in the center portion of the 3 Wide bitmap
                                row[ pixel_size + 1 ] = (byte)G;
                                row[ pixel_size + 2 ] = (byte)R;

                            }

                     //   }
                        

                    } // for loop 0 to W wide

                }
                else  // Gray=0 RGB scale
                {
                  
                   
                  
                     Byte Gray = 0;        // Gray scale 0 to 255
                     float offset = 0;

                    for (int i = 0; i < W; i++) // for each pixel in the new line
                    {
                     

                        if (waterfall_data[i] <= waterfall_low_threshold) // if RX strength of sample is below low threshold, only go down to low end of color
                        {
                             Gray = 0;
                        }
                        else if (waterfall_data[i] >= waterfall_high_threshold) // if strength exceeds high value, then dont go beyond max color values
                        {

                            Gray = 255;  //  255;// max value
                        }                        else // value is between low and high
                        {
                           
                             offset = waterfall_data[i] - waterfall_low_threshold;  // typical data -110 - 120 = almost 0

                            float overall_percent = offset / range; // value from 0.0 to 1.0 where 1.0 is high and 0.0 is low.

                            Gray = (Byte)(overall_percent * 255);   // 255);
                           
                            
                        }  // value is between low and high

                        //   Debug.WriteLine("water "+ waterfall_data[i] + " offset " + offset + " range " + range + " Gray " + Gray+" Gray1 " + Gray1 + " Gray2 "+ Gray2);
      
                        if (rx == 1)
                            {
                                pixel_size = (i * 3) + WM1;

                                row[ pixel_size + 0 ] = Gray; // R // ke9ns ADD draw new image only in the center portion of the 3 Wide bitmap
                                row[ pixel_size + 1 ] = Gray;  // G
                                row[ pixel_size + 2 ] = Gray;  // B
                            }
                            else
                            {
                                pixel_size = (i * 3) + WM2;

                                row[ pixel_size + 0 ] = Gray; // R // ke9ns ADD draw new image only in the center portion of the 3 Wide bitmap
                                row[ pixel_size + 1 ] = Gray;  // G
                                row[ pixel_size + 2 ] = Gray;  // B

                            }
                      //  }

                        //   byte Gray = (byte)((float)(R * .299) + (float)(G * .587) + (float)(B * .114));

                    } // for loop 0 to W wide

                } // Gray = 1 Gray scale


              
                //=======================================
                // UNLOCK SCREEN which will now display the new data
                //=======================================

                if (rx == 1)
					waterfall_bmp.UnlockBits(bitmapData);  // RX1
				else
					waterfall_bmp2.UnlockBits(bitmapData);  // RX2


                waterfall_low_threshold = temp_low_threshold;  // KE9NS reset low thres back to rx db
                waterfall_high_threshold = temp_high_threshold; // ke9ns reset high back to rx level


            } // if(duration > waterfall_update_period) STEP 5) waiting period



        //=======================================
        // Draw BITMAP image to screen (whatever data is now in the bitmap)
        //=======================================

            if (bottom)  // ke9ns if bottom half of screen start drawing image just below waterfall text + 16
			{
                if (rx == 1)
                {

                    //  if (WaterMove == 0)
                    //   {
                    //   g.DrawImageUnscaled(waterfall_bmp, 0, H + 16); // original  16 is the space for the freq text above the waterfall
                    //  }
                    //  else
                    // {
                    if (continuum == 0)
                    {
                        g.DrawImageUnscaled(waterfall_bmp, (-(W * WaterMove1)) - WM1F, H + 16); // ke9ns  this draws only the center WIDTH of the 3 Wide bitmap
                    }
                    else
                    {
                        g.DrawImageUnscaled(waterfall_bmp, 0, 16);  // ke9ns draw image shift down 16 and over to the right 100 to allow for time stamp and db values	

                    }
                    //  }


                }
                else if (rx == 2)
                {
                  //  if (WaterMove == 0)
                  //  {
                  //      g.DrawImageUnscaled(waterfall_bmp2, 0, H + 16); // original
                  //  }
                  //  else
                   // {
                        g.DrawImageUnscaled(waterfall_bmp2, (-(W* WaterMove1)) - WM2F, H + 16);
                   // }


                }

			} // bottom
			else
			{
                if (rx == 1)
                {
                    // if (WaterMove == 0)
                    //  {
                    //      g.DrawImageUnscaled(waterfall_bmp, 0, 16);  // draw the image on the background	
                    //  }
                    //  else
                    //  {

                    if (continuum == 0)
                    {
                         g.DrawImageUnscaled(waterfall_bmp, (-(W * WaterMove1)) - WM1F, 16);  // draw the image on the background	
                    }
                    else
                    {
                        g.DrawImageUnscaled(waterfall_bmp, 0, 16);  // ke9ns draw image shift down 16 and over to the right 100 to allow for time stamp and db values	
                    }

                  //  }
                }
                else if (rx == 2)
                {
                  //  if (WaterMove == 0)
                  //  {
                  //      g.DrawImageUnscaled(waterfall_bmp2, 0, 16); // draw the image on the backgroun
                  //  }
                  //  else
                  //  {
                        g.DrawImageUnscaled(waterfall_bmp2, (-(W* WaterMove)) - WM2F, 16); // draw the image on the backgroun	
                   // }
                }


             //   g.DrawImageUnscaled(waterfall_bmp, new Rectangle(0, 50, W, 100));

			} // top

            //	waterfall_counter++; // ke9ns not used ?

          //  stopWatch.Stop();
          //  TimeSpan ts = stopWatch.Elapsed;

         //   if (rx==1)  Debug.WriteLine("RunTime1 " + ts);
         //   else Debug.WriteLine("RunTime2 " + ts);


            //=======================================
            // draw long cursor
            //=======================================

            if (current_click_tune_mode != ClickTuneMode.Off)
			{
				Pen p;
				if(current_click_tune_mode == ClickTuneMode.VFOA)
					p = new Pen(grid_text_color);
				else p = new Pen(Color.Red);
                if (bottom)
                {                    if (display_cursor_y > H)
                    {
                        g.DrawLine(p, display_cursor_x, 0, display_cursor_x, H + H);
                        g.DrawLine(p, 0, display_cursor_y, W, display_cursor_y);
                    }
                    else g.DrawLine(p, display_cursor_x, 0, display_cursor_x, H + H);
                }
                else
                {
                    if (display_cursor_y <= H)
                    {
                        g.DrawLine(p, display_cursor_x, 0, display_cursor_x, H);
                        g.DrawLine(p, 0, display_cursor_y, W, display_cursor_y);
                    }
                }
			}

			return true;
		}

		unsafe static private bool DrawHistogram(Graphics g, int W, int H)
		{
			DrawSpectrumGrid(ref g, W, H, false);
			if(points == null || points.Length < W) 
				points = new Point[W];			// array of points to display
			float slope = 0.0F;						// samples to process per pixel
			int num_samples = 0;					// number of samples to process
			int start_sample_index = 0;				// index to begin looking at samples
			int low = 0;
			int high = 0;
			float local_max_y = Int32.MinValue;

			if(!mox)								// Receive Mode
			{
				low = rx_display_low;
				high = rx_display_high;
			}
			else									// Transmit Mode
			{
				low = tx_display_low;
				high = tx_display_high;
			}

			if(rx1_dsp_mode == DSPMode.DRM)
			{
				low = 2500;
				high = 21500;
			}

			int yRange = spectrum_grid_max - spectrum_grid_min;  

			if(data_ready)
			{
				// get new data
				fixed(void *rptr = &new_display_data[0]) // histogram
					fixed(void *wptr = &current_display_data[0])
						Win32.memcpy(wptr, rptr, BUFFER_SIZE*sizeof(float));
						
				// kb9yig sr mod starts 
				if ( current_model == Model.SOFTROCK40 ) 
					console.AdjustDisplayDataForBandEdge(ref current_display_data);
				// end kb9yig sr mods 

				data_ready = false;
			}

			if(average_on)
			{
				//if(!bottom)
					console.UpdateRX1DisplayAverage(rx1_average_buffer, current_display_data);
				/*else
					console.UpdateRX2DisplayAverage(rx2_average_buffer, current_display_data_bottom);*/
			}
			if(peak_on)
			{
				//if(!bottom)
					UpdateDisplayPeak(rx1_peak_buffer, current_display_data);
				/*else
					UpdateDisplayPeak(rx2_peak_buffer, current_display_data_bottom);*/
			}

			num_samples = (high - low);

			start_sample_index = (BUFFER_SIZE>>1) +(int)((low * BUFFER_SIZE) / sample_rate);
			num_samples = (int)((high - low) * BUFFER_SIZE / sample_rate);
			if (start_sample_index < 0) start_sample_index = 0;
			if ((num_samples - start_sample_index) > (BUFFER_SIZE+1))
				num_samples = BUFFER_SIZE-start_sample_index;

			slope = (float)num_samples/(float)W;
			for(int i=0; i<W; i++)
			{
				float max = float.MinValue;
				float dval = i*slope + start_sample_index;
				int lindex = (int)Math.Floor(dval);
				if (slope <= 1) 
					max =  current_display_data[lindex]*((float)lindex-dval+1) + current_display_data[lindex+1]*(dval-(float)lindex);
				else 
				{
					int rindex = (int)Math.Floor(dval + slope);
					if (rindex > BUFFER_SIZE) rindex = BUFFER_SIZE;
					for(int j=lindex;j<rindex;j++)
						if (current_display_data[j] > max) max=current_display_data[j];

				}

				max += rx1_display_cal_offset;
				if(!mox) max += rx1_preamp_offset;

				switch(rx1_dsp_mode)
				{
					case DSPMode.SPEC:
						max += 6.0F;
						break;
				}
				if(max > local_max_y)
				{
					local_max_y = max;
					max_x = i;
				}

				points[i].X = i;
                points[i].Y = (int)Math.Min((Math.Floor((spectrum_grid_max - max) * H / yRange)), H);
			} 

			max_y = local_max_y;

			// get the average
			float avg = 0.0F;
			int sum = 0;
			foreach(Point p in points)
				sum += p.Y;

			avg = (float)((float)sum/points.Length / 1.12);

			for(int i=0; i<W; i++)
			{
				if(points[i].Y < histogram_data[i])
				{
					histogram_history[i] = 0;
					histogram_data[i] = points[i].Y;
				}
				else
				{
					histogram_history[i]++;
					if(histogram_history[i] > 51)
					{
						histogram_history[i] = 0;
						histogram_data[i] = points[i].Y;
					}

					int alpha = (int)Math.Max(255-histogram_history[i]*5, 0);
					Color c = Color.FromArgb(alpha, 0, 255, 0);
					int height = points[i].Y-histogram_data[i];
					g.DrawRectangle(new Pen(c), i, histogram_data[i], 1, height);
				}

				if(points[i].Y >= avg)		// value is below the average
				{
					Color c = Color.FromArgb(150, 0, 0, 255);
					g.DrawRectangle(new Pen(c), points[i].X, points[i].Y, 1, H-points[i].Y);
				}
				else 
				{
					g.DrawRectangle(new Pen(Color.FromArgb(150, 0, 0, 255)), points[i].X, (int)Math.Floor(avg), 1, H-(int)Math.Floor(avg));
					g.DrawRectangle(new Pen(Color.FromArgb(150, 255, 0, 0)), points[i].X, points[i].Y, 1, (int)Math.Floor(avg)-points[i].Y);
				}
			}

			// draw long cursor
			if(current_click_tune_mode != ClickTuneMode.Off)
			{
				Pen p;
				if(current_click_tune_mode == ClickTuneMode.VFOA)
					p = new Pen(grid_text_color);
				else p = new Pen(Color.Red);
				g.DrawLine(p, display_cursor_x, 0, display_cursor_x, H);
				g.DrawLine(p, 0, display_cursor_y, W, display_cursor_y);
			}

			return true;
		}

		public static void ResetRX1DisplayAverage()
		{
			rx1_average_buffer[0] = CLEAR_FLAG;	// set reset flag
		}

		public static void ResetRX2DisplayAverage()
		{
			rx2_average_buffer[0] = CLEAR_FLAG;	// set reset flag
		}

		public static void ResetRX1DisplayPeak()
		{
			rx1_peak_buffer[0] = CLEAR_FLAG; // set reset flag
		}

		public static void ResetRX2DisplayPeak()
		{
			rx2_peak_buffer[0] = CLEAR_FLAG; // set reset flag
		}

        private class int16
        {
        }

#endregion

#endregion

#region DirectX
        /*
#region Variable Declaration

                private static Device dx_device = null;
                private static VertexBuffer dx_data_vb = null;
                private static VertexBuffer dx_mouse_vb = null;

                private static Background current_background;
                private static Microsoft.DirectX.Direct3D.Font bg_string_font;

                private static float[] data = null;
                private static CustomVertex.TransformedColored[] verts = null;

#endregion

#region Properties

                private static RenderType directx_render_type = RenderType.NONE;
                public static RenderType DirectXRenderType
                {
                    get { return directx_render_type; }
                    set { directx_render_type = value; }
                }

#endregion

#region Routines

                public static bool DirectXInit()
                {
                    // Now let's setup our D3D stuff
                    PresentParameters presentParams = new PresentParameters();
                    presentParams.Windowed = true;
                    presentParams.SwapEffect = SwapEffect.Discard;
                    try
                    {
                        dx_device = new Device(0,
                            DeviceType.Hardware, 
                            target, 
                            CreateFlags.HardwareVertexProcessing | CreateFlags.FpuPreserve, 
                            presentParams);
                        directx_render_type = RenderType.HARDWARE;
                    }
                    catch(DirectXException)
                    {
                        try
                        {
                            dx_device = new Device(0, 
                                DeviceType.Hardware, 
                                target, 
                                CreateFlags.SoftwareVertexProcessing | CreateFlags.FpuPreserve, 
                                presentParams);
                            directx_render_type = RenderType.SOFTWARE;
                        }
                        catch(DirectXException)
                        {
                            directx_render_type = RenderType.NONE;
                            return false;
                        }
                    }
                    OnCreateDirectXDevice(dx_device, null);
                    dx_device.RenderState.Lighting = false;
                    bg_string_font = new Microsoft.DirectX.Direct3D.Font(
                        dx_device, new System.Drawing.Font("Arial", 9.0f));

                    return true;
                }

                public static void DirectXRelease()
                {
                    dx_device = null;
                    dx_data_vb = null;
                    dx_mouse_vb = null;

                    current_background = null;
                    bg_string_font = null;

                    data = null;
                    verts = null;
                }

                public static void PrepareDisplayVars(DisplayMode mode)
                {
                    switch(console.CurrentDisplayEngine)
                    {
                        case DisplayEngine.DIRECT_X:
                            int data_length = 0;
                            int num_verts = 0;

                            switch(mode)
                            {
                                case DisplayMode.PANADAPTER:
                                case DisplayMode.SPECTRUM:
                                case DisplayMode.SCOPE:
                                    data_length = W;
                                    num_verts = W;
                                    break;
                            }

                            verts = new CustomVertex.TransformedColored[num_verts];
                            data = new float[data_length];
                            break;
                    }
                }

                public static void OnCreateDirectXDevice(object sender, EventArgs e)
                {
                    Device dev = (Device)sender;

                    dx_data_vb = new VertexBuffer(
                        typeof(CustomVertex.TransformedColored),
                        W,
                        dev,
                        Usage.WriteOnly,
                        CustomVertex.TransformedColored.Format,
                        Pool.Managed);

                    dx_mouse_vb = new VertexBuffer(
                        typeof(CustomVertex.TransformedColored),
                        4,
                        dev,
                        Usage.WriteOnly,
                        CustomVertex.TransformedColored.Format,
                        Pool.Managed);
                }

                public static void UpdateDataVertexBuffer(object sender, float[] data)
                {
                    VertexBuffer vb = (VertexBuffer)sender;
                    //CustomVertex.TransformedColored[] verts = new CustomVertex.TransformedColored[W];

                    int color = data_line_color.ToArgb();
                    for(int i=0; i<W; i++)
                    {
                        verts[i].X = i;
                        verts[i].Y = data[i];
                        verts[i].Z = 0.0f;
                        verts[i].Rhw = 1; 
                        verts[i].Color = color;
                    }

                    //GraphicsStream stm = vb.Lock(0, 0, 0);
                    //stm.Write(verts);
                    //vb.Unlock();
                    vb.SetData(verts, 0, LockFlags.None);
                }

                public static void UpdateDataVertexBuffer(object sender, Point[] data)
                {
                    VertexBuffer vb = (VertexBuffer)sender;
                    CustomVertex.TransformedColored[] verts = new CustomVertex.TransformedColored[data.Length];

                    for(int i=0; i<data.Length; i++)
                    {
                        verts[i].X = data[i].X;
                        verts[i].Y = data[i].Y;
                        verts[i].Z = 0.0f;
                        verts[i].Rhw = 1; 
                        verts[i].Color = data_line_color.ToArgb();
                    }

                    //GraphicsStream stm = vb.Lock(0, 0, 0);
                    //stm.Write(verts);
                    //vb.Unlock();
                    vb.SetData(verts, 0, LockFlags.None);
                }

                public static void UpdateMouseVertexBuffer(object sender, int x, int y)
                {
                    VertexBuffer vb = (VertexBuffer)sender;
                    CustomVertex.TransformedColored[] verts = new CustomVertex.TransformedColored[4];

                    verts[0].X = 0.0f;
                    verts[0].Y = (float)y;
                    verts[0].Z = 0.0f;
                    verts[0].Rhw = 1;
                    verts[0].Color = grid_text_color.ToArgb();

                    verts[1].X = (float)W;
                    verts[1].Y = (float)y;
                    verts[1].Z = 0.0f;
                    verts[1].Rhw = 1;
                    verts[1].Color = grid_text_color.ToArgb();

                    verts[2].X = (float)x;
                    verts[2].Y = 0.0f;
                    verts[2].Z = 0.0f;
                    verts[2].Rhw = 1;
                    verts[2].Color = grid_text_color.ToArgb();

                    verts[3].X = (float)x;
                    verts[3].Y = (float)H;
                    verts[3].Z = 0.0f;
                    verts[3].Rhw = 1;
                    verts[3].Color = grid_text_color.ToArgb();

                    //GraphicsStream stm = vb.Lock(0, 0, 0);
                    //stm.Write(verts);
                    //vb.Unlock();
                    vb.SetData(verts, 0, LockFlags.None);
                }

                public static void RenderDXBackground()
                {
                    if(current_background != null)
                    {		
                        // draw grid lines
                        foreach(DXLine l in current_background.lines)
                        {
                            l.Draw();
                        }

                        if(bg_string_font != null) // ensure the font has been instantiated
                        {
                            //verify background object string content
                            Debug.Assert(current_background.str_loc.Count == current_background.strings.Count);

                            // draw each string at the indicated points
                            for(int i=0; i<current_background.strings.Count; i++)
                            {
                                bg_string_font.DrawText(
                                    null, 
                                    (string)current_background.strings[i], 
                                    ((Point)current_background.str_loc[i]).X, 
                                    ((Point)current_background.str_loc[i]).Y, 
                                    grid_text_color);
                            }
                        }				
                    }
                }

                public static void RenderDirectX()
                {
                    if(dx_device == null) return;

                    // setup data
                    switch(current_display_mode)
                    {
                        case DisplayMode.PANADAPTER:
                            ConvertDataForPanadapter();
                            break;
                        case DisplayMode.SPECTRUM:
                            ConvertDataForSpectrum();
                            break;
                        case DisplayMode.PHASE:
                            ConvertDataForPhase();
                            break;
                        case DisplayMode.SCOPE:
                            ConvertDataForScope();
                            break;
                    }
                    if(console.LongCrosshair)
                        UpdateMouseVertexBuffer(dx_mouse_vb, console.DisplayCursorX, console.DisplayCursorY);

                    dx_device.Clear(ClearFlags.Target, display_background_color, 0.0f, 0);

                    //Begin the scene
                    dx_device.BeginScene();			

                    //dx_device.SetTexture(0, dx_texture);
                    //dx_device.SetStreamSource(0, dx_background_vb, 0);
                    //dx_device.VertexFormat = CustomVertex.TransformedTextured.Format;
                    //dx_device.DrawPrimitives(PrimitiveType.TriangleFan, 0, 2);
                    //dx_device.SetTexture(0, null);

                    RenderDXBackground();

                    dx_device.SetStreamSource(0, dx_data_vb, 0);
                    dx_device.VertexFormat = CustomVertex.TransformedColored.Format;

                    switch(current_display_mode)
                    {
                        case DisplayMode.PANADAPTER:
                        case DisplayMode.SPECTRUM:
                        case DisplayMode.SCOPE:
                            dx_device.DrawPrimitives(PrimitiveType.LineStrip, 0, W-1);
                            break;
                        case DisplayMode.PHASE:
                            dx_device.DrawPrimitives(PrimitiveType.PointList, 0, phase_num_pts);
                            break;
                    }

                    if(console.LongCrosshair)
                    {
                        dx_device.SetStreamSource(0, dx_mouse_vb, 0);
                        dx_device.DrawPrimitives(PrimitiveType.LineStrip, 0, 1);
                        dx_device.DrawPrimitives(PrimitiveType.LineStrip, 2, 1);
                    }

                    /*Microsoft.DirectX.Direct3D.Font font = new Microsoft.DirectX.Direct3D.Font(
                        dx_device, 
                        new System.Drawing.Font("Arial", 14.0f, FontStyle.Bold));
                    font.DrawText(null, string.Format("Testing"), new Rectangle(256, 20, 0, 0), DrawTextFormat.NoClip, Color.Red);

                    //if(console.HighSWR)
                    //{
                    //	if(directx_render_type == RenderType.HARDWARE)
                    //	{
                    //		Microsoft.DirectX.Direct3D.Font high_swr_font = new Microsoft.DirectX.Direct3D.Font(dx_device, new System.Drawing.Font("Arial", 14.0f, FontStyle.Bold));
                    //		high_swr_font.DrawText(null, string.Format("High SWR"), new Rectangle(245, 20, 0, 0), DrawTextFormat.NoClip, Color.Red);
                    //	}
                    //}

                    //End the scene
                    dx_device.EndScene();
                    dx_device.Present();
                }

                unsafe private static void ConvertDataForPanadapter()
                {
                    //float[] data = new float[W];			// array of points to display
                    float slope = 0.0f;						// samples to process per pixel
                    int num_samples = 0;					// number of samples to process
                    int start_sample_index = 0;				// index to begin looking at samples
                    int Low = -10000;
                    int High = 10000;
                    int yRange = spectrum_grid_max - spectrum_grid_min;

                    if(console.CurrentDSPMode == DSPMode.DRM)
                    {
                        Low = 2000;
                        High = 22000;
                    }

                    max_y = Int32.MinValue;

                    if(data_ready)
                    {
                        // get new data
                        fixed(void *rptr = &new_display_data[0])
                            fixed(void *wptr = &current_display_data[0])
                                Win32.memcpy(wptr, rptr, BUFFER_SIZE*sizeof(float));

                        // kb9yig sr mod starts 
                        if ( console.CurrentModel == Model.SOFTROCK40 ) 
                            console.AdjustDisplayDataForBandEdge(ref current_display_data);
                        // end kb9yig sr mods 

                        data_ready = false;
                    }

                    if(average_on)
                        console.UpdateDisplayAverage();
                    if(peak_on)
                        UpdateDisplayPeak();

                    num_samples = (High - Low);

                    start_sample_index = (BUFFER_SIZE>>1) +(int)((Low * BUFFER_SIZE) / sample_rate);
                    num_samples = (int)((High - Low) * BUFFER_SIZE / sample_rate);
                    if (start_sample_index < 0) start_sample_index = 0;
                    if ((num_samples - start_sample_index) > (BUFFER_SIZE+1))
                        num_samples = BUFFER_SIZE-start_sample_index;

                    slope = (float)num_samples/(float)W;
                    for(int i=0; i<W; i++)
                    {
                        float max = float.MinValue;
                        float dval = i*slope + start_sample_index;
                        int lindex = (int)Math.Floor(dval);
                        int rindex = (int)Math.Floor(dval + slope);
                        if (rindex > BUFFER_SIZE) rindex = BUFFER_SIZE;

                        for(int j=lindex;j<rindex;j++)
                            if (current_display_data[j] > max) max=current_display_data[j];

                        max = max +
                            console.DisplayCalOffset + 
                            console.PreampOffset;

                        if(max > max_y)
                        {
                            max_y = max;
                            max_x = i;
                        }

                        data[i] = (int)(Math.Floor((spectrum_grid_max - max)*H/yRange));
                    } 

                    UpdateDataVertexBuffer(dx_data_vb, data);
                }

                unsafe static private void ConvertDataForSpectrum()
                {
                    //float[] data = new float[W];			// array of points to display
                    float slope = 0.0f;						// samples to process per pixel
                    int num_samples = 0;					// number of samples to process
                    int start_sample_index = 0;				// index to begin looking at samples
                    int low = 0;
                    int high = 0;

                    max_y = Int32.MinValue;

                    if(!console.MOX)
                    {
                        low = rx_display_low;
                        high = rx_display_high;
                    }
                    else
                    {
                        low = tx_display_low;
                        high = tx_display_high;
                    }

                    if(console.CurrentDSPMode == DSPMode.DRM)
                    {
                        low = 2000;
                        high = 22000;
                    }

                    int yRange = spectrum_grid_max - spectrum_grid_min;

                    if(data_ready)
                    {
                        // get new data
                        fixed(void *rptr = &new_display_data[0])
                            fixed(void *wptr = &current_display_data[0])
                                Win32.memcpy(wptr, rptr, BUFFER_SIZE*sizeof(float));

                        // kb9yig sr mod starts 
                        if ( console.CurrentModel == Model.SOFTROCK40 ) 
                            console.AdjustDisplayDataForBandEdge(ref current_display_data);
                        // end kb9yig sr mods 

                        data_ready = false;
                    }

                    if(average_on)
                        console.UpdateDisplayAverage();
                    if(peak_on)
                        UpdateDisplayPeak();

                    start_sample_index = (BUFFER_SIZE>>1) + (int)((low * BUFFER_SIZE) / sample_rate);
                    num_samples = (int)((high - low) * BUFFER_SIZE / sample_rate);

                    if (start_sample_index < 0) start_sample_index = 0;
                    if ((num_samples - start_sample_index) > (BUFFER_SIZE+1))
                        num_samples = BUFFER_SIZE - start_sample_index;

                    slope = (float)num_samples/(float)W;
                    for(int i=0; i<W; i++)
                    {
                        float max = float.MinValue;
                        float dval = i*slope + start_sample_index;
                        int lindex = (int)Math.Floor(dval);
                        int rindex;

                        if (slope <= 1) 
                            max =  current_display_data[lindex]*((float)lindex-dval+1) + current_display_data[lindex+1]*(dval-(float)lindex);
                        else 
                        {
                            rindex = (int)Math.Floor(dval + slope);
                            if (rindex > BUFFER_SIZE) rindex = BUFFER_SIZE;
                            for(int j=lindex; j<rindex; j++)
                                if (current_display_data[j] > max) max=current_display_data[j];

                        }

                        max = max + 
                            console.DisplayCalOffset + 
                            console.PreampOffset;

                        if(max > max_y)
                        {
                            max_y = max;
                            max_x = i;
                        }

                        data[i] = (int)(Math.Floor((spectrum_grid_max - max)*H/yRange));
                    }

                    UpdateDataVertexBuffer(dx_data_vb, data);
                }

                unsafe private static void ConvertDataForPhase()
                {
                    int num_points = phase_num_pts;

                    if(data_ready)
                    {
                        // get new data
                        fixed(void *rptr = &new_display_data[0])
                            fixed(void *wptr = &current_display_data[0])
                                Win32.memcpy(wptr, rptr, BUFFER_SIZE*sizeof(float));

                        data_ready = false;
                    }

                    Point[] points = new Point[num_points];		// declare Point array
                    for(int i=0,j=0; i<num_points; i++,j+=8)	// fill point array
                    {
                        int x = (int)(current_display_data[i*2]*H/2);
                        int y = (int)(current_display_data[i*2+1]*H/2);
                        points[i].X = W/2+x;
                        points[i].Y = H/2+y;
                    }

                    // draw each point
                    UpdateDataVertexBuffer(dx_data_vb, points);

                    points = null;
                }

                unsafe private static void ConvertDataForScope()
                {
                    if(data_ready)
                    {
                        // get new data
                        fixed(void *rptr = &new_display_data[0])
                            fixed(void *wptr = &current_display_data[0])
                                Win32.memcpy(wptr, rptr, BUFFER_SIZE*sizeof(float));

                        data_ready = false;
                    }

                    double num_samples = console.ScopeTime/1000.0*console.SampleRate1;
                    double slope = num_samples/(double)W;

                    //float[] data = new float[W];				// create Point array
                    for(int i=0; i<W; i++)						// fill point array
                    {	
                        int pixels = (int)(H/2 * current_display_data[(int)Math.Floor(i*slope)]);
                        int y = H/2 - pixels;
                        if(y < max_y)
                        {
                            max_y = y;
                            max_x = i;
                        }

                        data[i] = y;
                    }

                    // draw the connected points
                    UpdateDataVertexBuffer(dx_data_vb, data);
                }

                private static Background SetupScope()
                {			
                    Background bg = new Background();

                    // Add horizontal line
                    bg.lines.Add(new DXLine(new Point(0, H/2), new Point(W, H/2), 1, grid_color, dx_device));

                    // Add vertical line
                    bg.lines.Add(new DXLine(new Point(W/2, 0), new Point(W/2, H), 1, grid_color, dx_device));

                    return bg;
                }

                private static Background SetupSpectrum()
                {
                    Background bg = new Background();
                    Graphics g = console.CreateGraphics();
                    System.Drawing.Font font = new System.Drawing.Font("Arial", 9);

                    int low = 0;								// init limit variables
                    int high = 0;

                    if(!console.MOX)
                    {
                        low = rx_display_low;				// get RX display limits
                        high = rx_display_high;
                    }
                    else
                    {
                        low = tx_display_low;				// get TX display limits
                        high = tx_display_high;
                    }

                    int mid_w = W/2;
                    int[] step_list = {10, 20, 25, 50};
                    int step_power = 1;
                    int step_index = 0;
                    int freq_step_size = 50;

                    int y_range = spectrum_grid_max - spectrum_grid_min;

                    if(high == 0)
                    {
                        int f = -low;
                        // Calculate horizontal step size
                        while(f/freq_step_size > 7)
                        {
                            freq_step_size = step_list[step_index]*(int)Math.Pow(10.0, step_power);
                            step_index = (step_index+1)%4;
                            if(step_index == 0) step_power++;
                        }
                        float pixel_step_size = (float)(W*freq_step_size/f);

                        int num_steps = f/freq_step_size;

                        // Draw vertical lines
                        for(int i=1; i<=num_steps; i++)
                        {
                            int x = W-(int)Math.Floor(i*pixel_step_size);	// for negative numbers

                            bg.lines.Add(new DXLine(new Point(x, 0), new Point(x, H), 1, grid_color, dx_device));
                            //g.DrawLine(grid_pen, x, 0, x, H);				// draw right line

                            // Draw vertical line labels
                            int num = i*freq_step_size;
                            string label = num.ToString();
                            int offset = (int)((label.Length+1)*4.1);
                            if(x-offset >= 0)
                            {
                                //g.DrawString("-"+label, font, grid_text_brush, x-offset, (float)Math.Floor(H*.01));
                                bg.strings.Add("-"+label);
                                bg.str_loc.Add(new Point(x-offset, (int)Math.Floor(H*.01)));
                            }
                        }

                        // Draw horizontal lines
                        int V = (int)(spectrum_grid_max - spectrum_grid_min);
                        num_steps = V/spectrum_grid_step;
                        pixel_step_size = H/num_steps;

                        for(int i=1; i<num_steps; i++)
                        {
                            int xOffset = 0;
                            int num = spectrum_grid_max - i*spectrum_grid_step;
                            int y = (int)Math.Floor((double)(spectrum_grid_max - num)*H/y_range);

                            //g.DrawLine(grid_pen, 0, y, W, y);
                            bg.lines.Add(new DXLine(new Point(0, y), new Point(W, y), 1, grid_color, dx_device));

                            // Draw horizontal line labels
                            string label = num.ToString();
                            int offset = (int)(label.Length*4.1);
                            if(label.Length == 3)
                                xOffset = (int)g.MeasureString("-", font).Width - 2;
                            SizeF size = g.MeasureString(label, font);

                            y -= 8;
                            int x = 0;
                            switch(display_label_align)
                            {
                                case DisplayLabelAlignment.LEFT:
                                    x = xOffset + 3;
                                    break;
                                case DisplayLabelAlignment.CENTER:
                                    x = W/2+xOffset;
                                    break;
                                case DisplayLabelAlignment.RIGHT:
                                case DisplayLabelAlignment.AUTO:
                                    x = (int)(W-size.Width);
                                    break;						
                                case DisplayLabelAlignment.OFF:
                                    x = W;
                                    break;
                            }

                            if(y+9 < H)
                            {
                                //g.DrawString(label, font, grid_text_brush, x, y);
                                bg.strings.Add(label);
                                bg.str_loc.Add(new Point(x, y));
                            }
                        }

                        // Draw middle vertical line
                        //g.DrawLine(new Pen(grid_zero_color), W-1, 0, W-1, H);
                        //g.DrawLine(new Pen(grid_zero_color), W-2, 0, W-2, H);
                        bg.lines.Add(new DXLine(new Point(W-1, 0), new Point(W-1, H), 1, grid_zero_color, dx_device));
                        bg.lines.Add(new DXLine(new Point(W-2, 0), new Point(W-2, H), 1, grid_zero_color, dx_device));
                    }
                    else if(low == 0)
                    {
                        int f = high;
                        // Calculate horizontal step size
                        while(f/freq_step_size > 7)
                        {
                            freq_step_size = step_list[step_index]*(int)Math.Pow(10.0, step_power);
                            step_index = (step_index+1)%4;
                            if(step_index == 0) step_power++;
                        }
                        float pixel_step_size = (float)(W*freq_step_size/f);
                        int num_steps = f/freq_step_size;

                        // Draw vertical lines
                        for(int i=1; i<=num_steps; i++)
                        {
                            int x = (int)Math.Floor(i*pixel_step_size);// for positive numbers

                            //g.DrawLine(grid_pen, x, 0, x, H);			// draw right line
                            bg.lines.Add(new DXLine(new Point(x, 0), new Point(x, H), 1, grid_color, dx_device));

                            // Draw vertical line labels
                            int num = i*freq_step_size;
                            string label = num.ToString();
                            int offset = (int)(label.Length*4.1);
                            if(x-offset+label.Length*7 < W)
                            {
                                //g.DrawString(label, font, grid_text_brush, x-offset, (float)Math.Floor(H*.01));
                                bg.strings.Add(label);
                                bg.str_loc.Add(new Point(x-offset, (int)Math.Floor(H*.01)));
                            }
                        }

                        // Draw horizontal lines
                        int V = (int)(spectrum_grid_max - spectrum_grid_min);
                        int numSteps = V/spectrum_grid_step;
                        pixel_step_size = H/numSteps;
                        for(int i=1; i<numSteps; i++)
                        {
                            int xOffset = 0;
                            int num = spectrum_grid_max - i*spectrum_grid_step;
                            int y = (int)Math.Floor((double)(spectrum_grid_max - num)*H/y_range);

                            //g.DrawLine(grid_pen, 0, y, W, y);
                            bg.lines.Add(new DXLine(new Point(0, y), new Point(W, y), 1, grid_color, dx_device));

                            // Draw horizontal line labels
                            string label = num.ToString();
                            if(label.Length == 3)
                                xOffset = (int)g.MeasureString("-", font).Width - 2;
                            int offset = (int)(label.Length*4.1);
                            SizeF size = g.MeasureString(label, font);

                            int x = 0;
                            switch(display_label_align)
                            {
                                case DisplayLabelAlignment.LEFT:
                                case DisplayLabelAlignment.AUTO:
                                    x = xOffset + 3;
                                    break;
                                case DisplayLabelAlignment.CENTER:
                                    x = W/2+xOffset;
                                    break;
                                case DisplayLabelAlignment.RIGHT:
                                    x = (int)(W-size.Width);
                                    break;
                                case DisplayLabelAlignment.OFF:
                                    x = W;
                                    break;
                            }

                            y -= 8;
                            if(y+9 < H)
                            {
                                //g.DrawString(label, font, grid_text_brush, x, y);
                                bg.strings.Add(label);
                                bg.str_loc.Add(new Point(x, y));
                            }
                        }

                        // Draw middle vertical line
                        //g.DrawLine(new Pen(grid_zero_color), 0, 0, 0, H);
                        //g.DrawLine(new Pen(grid_zero_color), 1, 0, 1, H);
                        bg.lines.Add(new DXLine(new Point(0, 0), new Point(0, H), 1, grid_zero_color, dx_device));
                        bg.lines.Add(new DXLine(new Point(1, 0), new Point(1, H), 1, grid_zero_color, dx_device));
                    }
                    if(low < 0 && high > 0)
                    {
                        int f = high;

                        // Calculate horizontal step size
                        while(f/freq_step_size > 4)
                        {
                            freq_step_size = step_list[step_index]*(int)Math.Pow(10.0, step_power);
                            step_index = (step_index+1)%4;
                            if(step_index == 0) step_power++;
                        }
                        int pixel_step_size = W/2*freq_step_size/f;
                        int num_steps = f/freq_step_size;

                        // Draw vertical lines
                        for(int i=1; i<=num_steps; i++)
                        {
                            int xLeft = mid_w-(i*pixel_step_size);			// for negative numbers
                            int xRight = mid_w+(i*pixel_step_size);		// for positive numbers
                            //g.DrawLine(grid_pen, xLeft, 0, xLeft, H);		// draw left line
                            //g.DrawLine(grid_pen, xRight, 0, xRight, H);		// draw right line
                            bg.lines.Add(new DXLine(new Point(xLeft, 0), new Point(xLeft, H), 1, grid_color, dx_device));
                            bg.lines.Add(new DXLine(new Point(xRight, 0), new Point(xRight, H), 1, grid_color, dx_device));

                            // Draw vertical line labels
                            int num = i*freq_step_size;
                            string label = num.ToString();
                            int offsetL = (int)((label.Length+1)*4.1);
                            int offsetR = (int)(label.Length*4.1);
                            if(xLeft-offsetL >= 0)
                            {
                                //g.DrawString("-"+label, font, grid_text_brush, xLeft-offsetL, (float)Math.Floor(H*.01));
                                bg.strings.Add("-"+label);
                                bg.str_loc.Add(new Point(xLeft-offsetL, (int)Math.Floor(H*.01)));
                                //g.DrawString(label, font, grid_text_brush, xRight-offsetR, (float)Math.Floor(H*.01));
                                bg.strings.Add(label);
                                bg.str_loc.Add(new Point(xRight-offsetR, (int)Math.Floor(H*.01)));
                            }
                        }

                        // Draw horizontal lines
                        int V = (int)(spectrum_grid_max - spectrum_grid_min);
                        int numSteps = V/spectrum_grid_step;
                        pixel_step_size = H/numSteps;
                        for(int i=1; i<numSteps; i++)
                        {
                            int xOffset = 0;
                            int num = spectrum_grid_max - i*spectrum_grid_step;
                            int y = (int)Math.Floor((double)(spectrum_grid_max - num)*H/y_range);
                            //g.DrawLine(grid_pen, 0, y, W, y);
                            bg.lines.Add(new DXLine(new Point(0, y), new Point(W, y), 1, grid_color, dx_device));

                            // Draw horizontal line labels
                            string label = num.ToString();
                            if(label.Length == 3) xOffset = 7;
                            int offset = (int)(label.Length*4.1);
                            SizeF size = g.MeasureString(label, font);

                            int x = 0;
                            switch(display_label_align)
                            {
                                case DisplayLabelAlignment.LEFT:
                                    x = xOffset + 3;
                                    break;
                                case DisplayLabelAlignment.CENTER:
                                case DisplayLabelAlignment.AUTO:
                                    x = W/2+xOffset;
                                    break;
                                case DisplayLabelAlignment.RIGHT:
                                    x = (int)(W-size.Width);
                                    break;
                                case DisplayLabelAlignment.OFF:
                                    x = W;
                                    break;
                            }

                            y -= 8;
                            if(y+9 < H)
                            {
                                //g.DrawString(label, font, grid_text_brush, x, y);
                                bg.strings.Add(label);
                                bg.str_loc.Add(new Point(x, y));
                            }
                        }

                        // Draw middle vertical line
                        //g.DrawLine(new Pen(grid_zero_color), mid_w, 0, mid_w, H);
                        //g.DrawLine(new Pen(grid_zero_color), mid_w-1, 0, mid_w-1, H);
                        bg.lines.Add(new DXLine(new Point(mid_w, 0), new Point(mid_w, H), 1, grid_zero_color, dx_device));
                        bg.lines.Add(new DXLine(new Point(mid_w-1, 0), new Point(mid_w-1, H), 1, grid_zero_color, dx_device));
                    }

                    if(console.HighSWR)
                    {
                        //g.DrawString("High SWR", high_swr_font, red_brush, 245, 20);
                        bg.strings.Add("High SWR");
                        bg.str_loc.Add(new Point(245, 20));
                    }

                    g = null;
                    font = null;

                    return bg;
                }

#endregion

#endregion


#region Background Class

                public class Background
                {
                    public ArrayList strings;		// array of strings to be drawn on the background
                    public ArrayList str_loc;		// array of top/left location of strings to be drawn
                    public ArrayList lines;			// array of DXLines to be drawn
                    public ArrayList overlay;		// array of points for overlay to be drawn

                    public Background()
                    {
                        strings = new ArrayList();
                        str_loc = new ArrayList();
                        lines = new ArrayList();
                        overlay = new ArrayList();
                    }
                }

#endregion

#region DXLine Class

                public class DXLine
                {
                    // Line object 
                    private Microsoft.DirectX.Direct3D.Line mLine;
                    private Vector2[] line_vectors;

                    // Starting point for the line 
                    private Point mStartPoint;
                    public Point StartPoint
                    {
                        get { return mStartPoint; }
                        set
                        {
                            mStartPoint = value;
                            UpdateLineVectors();
                        }
                    }

                    // Ending point for the line 
                    private Point mEndPoint;
                    public Point EndPoint
                    {
                        get { return mEndPoint; }
                        set
                        {
                            mEndPoint = value;
                            UpdateLineVectors();
                        }
                    }

                    // Width of the line 
                    private int mWidth;
                    public int Width 
                    {
                        get { return mWidth; }
                        set { mWidth = value; }
                    }

                    // Color for the line 
                    private Color mColor;
                    public Color Color
                    {
                        get { return mColor; }
                        set { mColor = value; }
                    }

                    // Line class constructor 
                    public DXLine(Point startPoint, Point endPoint, int width, Color color, Device device)
                    {
                        // Store the data passed into the class constructor 
                        mStartPoint = startPoint;
                        mEndPoint = endPoint;
                        mWidth = width;
                        mColor = color;

                        // create line vectors
                        line_vectors = new Vector2[2];
                        UpdateLineVectors();

                        // Create the line object 
                        mLine = new Line(device);
                    }

                    // Draw the line using the current class values for starpoint, endpoint, width and color 
                    public void Draw()
                    {
                        // Render the line 
                        mLine.Begin();
                        mLine.Draw(line_vectors, Color);
                        mLine.End();
                    } 

                    // Construct the Line Vectors based on the Start and End Points 
                    private void UpdateLineVectors()
                    {
                        // Set the starting point of the line 
                        line_vectors[0].X = StartPoint.X;
                        line_vectors[0].Y = StartPoint.Y; 

                        // Set the end point of the line 
                        line_vectors[1].X = EndPoint.X;
                        line_vectors[1].Y = EndPoint.Y;
                    }
                }
        */
#endregion
    }
}