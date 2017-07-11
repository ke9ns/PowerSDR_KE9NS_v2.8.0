//=================================================================
// audio.cs
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

//#define VAC_DEBUG
//#define MINMAX
//#define TIMER
//#define INTERLEAVED
//#define SPLIT_INTERLEAVED


using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

using FlexCW;

namespace PowerSDR
{
	public class Audio
	{
		#region PowerSDR Specific Variables
		// ======================================================
		// PowerSDR Specific Variables
		// ======================================================

		public enum SignalSource
		{
			RADIO,
			SINE,
			SINE_TWO_TONE,
			SINE_LEFT_ONLY,
			SINE_RIGHT_ONLY,
			NOISE,
			TRIANGLE,
			SAWTOOTH,
            PULSE,
			SILENCE,
		}

#if(INTERLEAVED)
#if(SPLIT_INTERLEAVED)
		unsafe private static PA19.PaStreamCallback callback1 = new PA19.PaStreamCallback(Callback1ILDI);	// Init callbacks to prevent GC
		unsafe private static PA19.PaStreamCallback callbackVAC = new PA19.PaStreamCallback(CallbackVACILDI);
		unsafe private static PA19.PaStreamCallback callback4port = new PA19.PaStreamCallback(Callback4PortILDI);
#else
		unsafe private static PA19.PaStreamCallback callback1 = new PA19.PaStreamCallback(Callback1IL);	// Init callbacks to prevent GC
		unsafe private static PA19.PaStreamCallback callbackVAC = new PA19.PaStreamCallback(CallbackVACIL);
		unsafe private static PA19.PaStreamCallback callback4port = new PA19.PaStreamCallback(Callback4PortIL);
#endif
#else
		unsafe private static PA19.PaStreamCallback callback1 = new PA19.PaStreamCallback(Callback1);	// Init callbacks to prevent GC
		unsafe private static PA19.PaStreamCallback callbackVAC = new PA19.PaStreamCallback(CallbackVAC);
        unsafe private static PA19.PaStreamCallback callbackVAC2 = new PA19.PaStreamCallback(CallbackVAC2);
		unsafe private static PA19.PaStreamCallback callback4port = new PA19.PaStreamCallback(Callback4Port);

		unsafe private static PA19.PaStreamCallback callback8 = new PA19.PaStreamCallback(Callback2);
#endif

		public static int callback_return = 0;

		/*private static bool spike = false;
		public static bool Spike
		{
			get { return spike; }
			set { spike = value; }
		}*/

		private static bool rx2_auto_mute_tx = true;
		public static bool RX2AutoMuteTX
		{
			get { return rx2_auto_mute_tx; }
			set { rx2_auto_mute_tx = value; }
		}

		private static double source_scale = 1.0;
		public static double SourceScale
		{
			get { return source_scale; }
			set { source_scale = value; }
		}

		private static SignalSource rx1_input_signal = SignalSource.RADIO;
		public static SignalSource RX1InputSignal
		{
			get { return rx1_input_signal; }
			set { rx1_input_signal = value; }
		}

		private static SignalSource rx1_output_signal = SignalSource.RADIO; 
		public static SignalSource RX1OutputSignal  // ke9ns this is from the setup test screen for Receiver select Radio as default
		{
			get { return rx1_output_signal; }
			set { rx1_output_signal = value; }
		}

		private static SignalSource rx2_input_signal = SignalSource.RADIO;
		public static SignalSource RX2InputSignal
		{
			get { return rx2_input_signal; }
			set { rx2_input_signal = value; }
		}

		private static SignalSource rx2_output_signal = SignalSource.RADIO;
		public static SignalSource RX2OutputSignal
		{
			get { return rx2_output_signal; }
			set { rx2_output_signal = value; }
		}

		private static SignalSource tx_input_signal = SignalSource.RADIO;
		public static SignalSource TXInputSignal
		{
			get { return tx_input_signal; }
			set { tx_input_signal = value; }
		}

		private static SignalSource tx_output_signal = SignalSource.RADIO;
		public static SignalSource TXOutputSignal
		{
			get { return tx_output_signal; }
			set { tx_output_signal = value; }
		}

		private static bool record_rx_preprocessed = true;
		public static bool RecordRXPreProcessed
		{
			get { return record_rx_preprocessed; }
			set { record_rx_preprocessed = value; }
		}

		private static bool record_tx_preprocessed = false;
		public static bool RecordTXPreProcessed
		{
			get { return record_tx_preprocessed; }
			set { record_tx_preprocessed = value; }
		}

		private static float peak = float.MinValue;
		public static float Peak
		{
			get { return peak; }
			set { peak = value; }
		}


        private static float peak1 = 0.001F; // ke9ns add (for MIC dBm value while in RX mode
        public static float Peak1
        {
            get {
                    if (peak1 < 0.0001F) return 0.001F;
                    else return peak1;
                }
            set {
                    peak1 = value;
                }
        } // Peak1

        private static bool vox_enabled = false;
		public static bool VOXEnabled
		{
			get { return vox_enabled; }
			set { vox_enabled = value; }
		}

		private static float vox_threshold = 0.001f;
		public static float VOXThreshold
		{
			get { return vox_threshold; }
			set { vox_threshold = value; }
		}

		public static double TXScale
		{
			get { return high_swr_scale * radio_volume; }
		}

		public static double FWCTXScale
		{
			get { return high_swr_scale * temp_scale * radio_volume; }
		}

		private static double temp_scale = 1.0;
		public static double TempScale
		{
			get { return temp_scale; }
			set { temp_scale = value; }
		}

		private static double high_swr_scale = 1.0;
		public static double HighSWRScale
		{
			get { return high_swr_scale; }
			set	{ high_swr_scale = value; } 
		}

		private static double mic_preamp = 1.0;
		public static double MicPreamp
		{
			get { return mic_preamp; }
			set { mic_preamp = value; }
		}

		private static double wave_preamp = 1.0;
		public static double WavePreamp
		{
			get { return wave_preamp; }
			set { wave_preamp = value; }
		}

		private static double monitor_volume = 0.0;
		public static double MonitorVolume
		{
			get {
              //  Debug.WriteLine("monitor_volume: "+monitor_volume);

                return monitor_volume; }
			set
			{
				Debug.WriteLine("monitor_volume: "+value.ToString("f3"));
				
              
                    monitor_volume = value;
               
               //   Trace.WriteLine("monvolume " + value);

            }
        }

		private static double radio_volume = 0.0;
		public static double RadioVolume
		{
			get { return radio_volume; }
			set
			{
				//Debug.WriteLine("radio_volume: "+value.ToString("f3"));
				radio_volume = value; 				
			}
		}

        /*private static double ramp_time = 5.0; // in ms
		private static int ramp_samples = (int)(sample_rate1*ramp_time*1e-3);
		private static int ramp_count = 0;
        private static double ramp_step = Math.PI / 2.0 / ramp_samples;

		private static bool ramp_down = false;
		public static bool RampDown
		{
			get { return ramp_down; }
			set
			{
				ramp_down = value;
                ramp_samples = (int)(sample_rate1 * ramp_time * 1e-3);
                ramp_step = Math.PI / 2.0 / ramp_samples;
				if(value)
                    ramp_count = 0;
			}
		}*/

		private static double sine_freq1 = 1250.0;
		private static double phase_step1 = sine_freq1/sample_rate1*2*Math.PI;
		private static double phase_accumulator1 = 0.0;

		private static double sine_freq2 = 1900.0;
		private static double phase_step2 = sine_freq2/sample_rate1*2*Math.PI;
		private static double phase_accumulator2 = 0.0;

		public static double SineFreq1
		{
			get { return sine_freq1; }
			set
			{
				sine_freq1 = value;
				phase_step1 = sine_freq1/sample_rate1*2*Math.PI;
			}
		}

		public static double SineFreq2
		{
			get { return sine_freq2; }
			set
			{
				sine_freq2 = value;
				phase_step2 = sine_freq2/sample_rate1*2*Math.PI;
			}
		}

		private static int in_rx1_l = 0;
		public static int IN_RX1_L
		{
			get { return in_rx1_l; }
			set { in_rx1_l = value; }
		}

		private static int in_rx1_r = 1;
		public static int IN_RX1_R
		{
			get { return in_rx1_r; }
			set { in_rx1_r = value; }
		}

		private static int in_rx2_l = 4;
		public static int IN_RX2_L
		{
			get { return in_rx2_l; }
			set { in_rx2_l = value; }
		}

		private static int in_rx2_r = 5;
		public static int IN_RX2_R
		{
			get { return in_rx2_r; }
			set { in_rx2_r = value; }
		}

		private static int in_tx_l = 2;
		public static int IN_TX_L
		{
			get { return in_tx_l; }
			set 
			{
				in_tx_l = value;
				switch(in_tx_l)
				{
					case 4:
					case 5:
					case 6:
						in_tx_r = in_tx_l+1;
						break;
					case 7:
						in_tx_r = 4;
						break;
				}
			}
		}

		private static int in_tx_r = 3;
		public static int IN_TX_R
		{
			get { return in_tx_r; }
			set { in_tx_r = value; }
		}

		private static bool rx2_enabled = false;
		public static bool RX2Enabled
		{
			get { return rx2_enabled; }
			set { rx2_enabled = value; }
		}

        private static bool tx_1500_image_cal = false;
        public static bool TX1500ImageCal
        {
            get { return tx_1500_image_cal; }
            set { tx_1500_image_cal = value; }
        }

		public static Console console = null;
        public static Setup setupForm = null; // ke9ns add


		unsafe private static void *stream1;
		unsafe private static void *stream2;
        unsafe private static void *stream3;
		//private static int block_size2 = 2048;
		public static float[] phase_buf_l;
		public static float[] phase_buf_r;
		public static bool phase = false;
		public static bool wave_record = false;
		public static bool wave_playback = false;
		public static WaveFileWriter wave_file_writer;
        public static WaveFileWriter wave_file_writer2;
		public static WaveFileReader1 wave_file_reader;    // ke9ns   RX1 WaveFileReader  (was waveFileReader)
        public static WaveFileReader1 wave_file_reader2;   // ke9ns   RX2 
		public static bool two_tone = false;
		//public static Mutex phase_mutex = new Mutex();
		public static bool testing = false;
		private static bool localmox;

        private static int empty_buffers = 0;
        public static int EmptyBuffers
        {
            get { return empty_buffers; }
        }

		#region VAC Variables

		private static RingBufferFloat rb_vacIN_l;
		private static RingBufferFloat rb_vacIN_r;
		private static RingBufferFloat rb_vacOUT_l;
		private static RingBufferFloat rb_vacOUT_r;

		private static float[] res_inl;
		private static float[] res_inr;
		private static float[] res_outl;
		private static float[] res_outr;

		unsafe private static void *resampPtrIn_l;
		unsafe private static void *resampPtrIn_r;
		unsafe private static void *resampPtrOut_l;
        unsafe private static void *resampPtrOut_r;

        unsafe private static void* resampPtrOut_T;  // ke9ns add

        private static bool vac_resample = false;
		private static bool vac_combine_input = false;
		public static bool VACCombineInput
		{
			get { return vac_combine_input; }
			set { vac_combine_input = value; }
		}

		#endregion

        #region VAC2 Variables

        private static RingBufferFloat rb_vac2IN_l;
        private static RingBufferFloat rb_vac2IN_r;
        private static RingBufferFloat rb_vac2OUT_l;
        private static RingBufferFloat rb_vac2OUT_r;

        private static float[] res_vac2_inl;
        private static float[] res_vac2_inr;
        private static float[] res_vac2_outl;
        private static float[] res_vac2_outr;

        unsafe private static void* resampVAC2PtrIn_l;
        unsafe private static void* resampVAC2PtrIn_r;
        unsafe private static void* resampVAC2PtrOut_l;
        unsafe private static void* resampVAC2PtrOut_r;

        private static bool vac2_resample = false;
        private static bool vac2_combine_input = false;
        public static bool VAC2CombineInput
        {
            get { return vac2_combine_input; }
            set { vac2_combine_input = value; }
        }

        #endregion

        #endregion

        #region Local Copies of External Properties

        private static bool mox = false;
		public static bool MOX
		{
			set { mox = value; }
		}

		unsafe private static void *cs_vac;
        unsafe private static void *cs_vac2;

		private static bool mon = false;
		public static bool MON
		{
			set { mon = value; }
		}


        //==========================================================
        // ke9ns add true = pre-processes, false=standard post-process (except for AM/FM since they dont work in pre)
        private static byte monpre = 0;
        public static byte MON_PRE
        {
            set
            {
                monpre = value;
            }
            get { return monpre; }
        }


        private static bool full_duplex = false;
		public static bool FullDuplex
		{
			set { full_duplex = value; }
		}

        private static bool vfob_tx = false;
        public static bool VFOBTX
        {
            set { vfob_tx = value; }
        }

		private static bool vac_enabled = false;
		public static bool VACEnabled
		{
			set
			{
				vac_enabled = value;
				if(vac_enabled) InitVAC();
				else CleanUpVAC();
			}
			get { return vac_enabled; }
		}

        private static bool vac2_enabled = false;
        public static bool VAC2Enabled // called from console.cs
        {
            set
            {
                vac2_enabled = value;
                if (vac2_enabled) InitVAC2();
                else CleanUpVAC2();
            }
            get { return vac2_enabled; }
        }

        private static bool vac2_rx2 = true;
        public static bool VAC2RX2
        {
            get { return vac2_rx2; }
            set { vac2_rx2 = value; }
        }

		private static bool vac_bypass = false;
		public static bool VACBypass
		{
			get { return vac_bypass; }
			set { vac_bypass = value; }
		}

		private static bool vac_rb_reset = false;
		public static bool VACRBReset
		{
			set
			{
				vac_rb_reset = value;
			}
			get { return vac_rb_reset; }
		}

        private static bool vac2_rb_reset = false;
        public static bool VAC2RBReset
        {
            get { return vac2_rb_reset; }
            set { vac2_rb_reset = value; }
        }

		private static double vac_preamp = 1.0;
		public static double VACPreamp
		{
			get { return vac_preamp; }
			set
			{
				//Debug.WriteLine("vac_preamp: "+value.ToString("f3"));
				vac_preamp = value;
			}
		}

        private static double vac2_tx_scale = 1.0;
        public static double VAC2TXScale
        {
            get { return vac2_tx_scale; }
            set
            {
                //Debug.Writeline("vac2_tx_scale: "+value.ToString("f3"));
                vac2_tx_scale = value;
            }
        }

		private static double vac_rx_scale = 1.0;
		public static double VACRXScale
		{
			get { return vac_rx_scale; }
			set
			{
				//Debug.WriteLine("vac_rx_scale: "+value.ToString("f3"));
				vac_rx_scale = value;
			}
		}

        private static double vac2_rx_scale = 1.0;
        public static double VAC2RXScale
        {
            get { return vac2_rx_scale; }
            set
            {
                //Debug.WriteLine("vac2_rx_scale: "+value.ToString("f3"));
                vac2_rx_scale = value;
            }
        }

		private static DSPMode tx_dsp_mode = DSPMode.LSB;
		public static DSPMode TXDSPMode
		{
			get { return tx_dsp_mode; }
			set { tx_dsp_mode = value; }
		}

        //===================================================
        // ke9ns check sample rate for primary audio
		private static int sample_rate1 = 48000;
		public static int SampleRate1
		{
			get { return sample_rate1; }
			set	
			{
				sample_rate1 = value;
				SineFreq1 = sine_freq1;
				SineFreq2 = sine_freq2;
			}
		}
        
        
        //===================================================
        // ke9ns check sample rate for vac1 audio
        private static int sample_rate2 = 48000;
		public static int SampleRate2
		{
			get { return sample_rate2; }
			set 
			{
				sample_rate2 = value; 
				if(vac_enabled) InitVAC();
			}			
		}

        //===================================================
        // ke9ns check sample rate for vac2 audio
        private static int sample_rate3 = 48000;
        public static int SampleRate3
        {
            get { return sample_rate3; }
            set
            {
                sample_rate3 = value;
                if (vac2_enabled) InitVAC2();
            }
        }

		private static int block_size1 = 1024;
		public static int BlockSize
		{
			get { return block_size1; }
			set { block_size1 = value; }			
		}

		private static int block_size_vac = 2048;
		public static int BlockSizeVAC
		{
			get { return block_size_vac; }
			set { block_size_vac = value; }			
		}

        private static int block_size_vac2 = 2048;
        public static int BlockSizeVAC2
        {
            get { return block_size_vac2; }
            set { block_size_vac2 = value; }
        }

		private static double audio_volts1 = 2.23;
		public static double AudioVolts1
		{
			get { return audio_volts1; }
			set { audio_volts1 = value; }
		}

		private static bool vac_stereo = false;
		public static bool VACStereo
		{
			set { vac_stereo = value; }
		}

        private static bool vac2_stereo = false;
        public static bool VAC2Stereo
        {
            set { vac2_stereo = value; }
        }

		private static bool vac_output_iq = false;
		public static bool VACOutputIQ
		{
			set { vac_output_iq = value; }
		}

        private static bool vac2_output_iq = false;
        public static bool VAC2OutputIQ
        {
            set { vac2_output_iq = value; }
        }

        private static bool vac_output_rx2 = false;
        public static bool VACOutputRX2
        {
            set { vac_output_rx2 = value; }
        }

		private static float iq_phase = 0.0f;
		public static float IQPhase
		{
			set { iq_phase = value; }
		}

		private static float iq_gain = 1.0f;
		public static float IQGain
		{
			set { iq_gain = value; }
		}

        private static float iq_phase2 = 0.0f;
        public static float IQPhase2
        {
            set { iq_phase2 = value; }
        }

        private static float iq_gain2 = 1.0f;
        public static float IQGain2
        {
            set { iq_gain2 = value; }
        }

		private static bool vac_correct_iq = true;
		public static bool VACCorrectIQ
		{
			set { vac_correct_iq = value; }
		}

        private static bool vac2_correct_iq = true;
        public static bool VAC2CorrectIQ
        {
            set { vac2_correct_iq = value; }
        }

		private static SoundCard soundcard = SoundCard.UNSUPPORTED_CARD;
		public static SoundCard CurSoundCard
		{
			set { soundcard = value; }
		}

		private static bool vox_active = false;
		public static bool VOXActive
		{
			get { return vox_active; }
			set { vox_active = value; }
		}

		private static int num_channels = 2;
		public static int NumChannels
		{
			get { return num_channels; }
			set { num_channels = value; }
		}

		private static int host1 = 0;
		public static int Host1
		{
			get { return host1; }
			set { host1 = value; }
		}

		private static int host2 = 0;  // VAC1 driver from setup
        public static int Host2
		{
			get { return host2; }
			set { host2 = value; }
		}

        private static int host3 = 0; // VAC2 driver from setup
        public static int Host3
        {
            get { return host3; }
            set { host3 = value; }
        }

		private static int input_dev1 = 0;
		public static int Input1
		{
			get { return input_dev1; }
			set { input_dev1 = value; } // ke9ns primary audio input to transmit with  setup int new_input = ((PADeviceInfo)comboAudioInput1.SelectedItem).Index;

        }


        //===========================================
        // ke9ns input device from VAC1
        //==========================================
        private static int input_dev2 = 0;
		public static int Input2
		{
			get { return input_dev2; }
			set { input_dev2 = value; }        // ke9ns 	int new_input = ((PADeviceInfo)comboAudioInput2.SelectedItem).Index;  in setup form

        }

        private static int input_dev3 = 0;
        public static int Input3
        {
            get { return input_dev3; }
            set { input_dev3 = value; }
        }

		private static int output_dev1 = 0;
		public static int Output1
		{
			get { return output_dev1; }
			set { output_dev1 = value; } // ke9ns primaary output device to receive to  setup 	int new_output = ((PADeviceInfo)comboAudioOutput1.SelectedItem).Index;

        }

        private static int output_dev2 = 0;
		public static int Output2
		{
			get { return output_dev2; }
			set { output_dev2 = value; }
		}

        private static int output_dev3 = 0;
        public static int Output3
        {
            get { return output_dev3; }
            set { output_dev3 = value; }
        }

		private static int latency1 = 0;
		public static int Latency1
		{
			get { return latency1; }
			set { latency1 = value; }
		}

		private static int latency2 = 120;
		public static int Latency2
		{
			set { latency2 = value; }
		}

        private static int latency3 = 120;
        public static int Latency3
        {
            set { latency3 = value; }
        }

        private static float min_in_l = float.MaxValue;
        public static float MinInL
        {
            get { return min_in_l; }
        }

        private static float min_in_r = float.MaxValue;
        public static float MinInR
        {
            get { return min_in_r; }
        }

        private static float min_out_l = float.MaxValue;
        public static float MinOutL
        {
            get { return min_out_l; }
        }

        private static float min_out_r = float.MaxValue;
        public static float MinOutR
        {
            get { return min_out_r; }
        }

        private static float max_in_l = float.MaxValue;
        public static float MaxInL
        {
            get { return max_in_l; }
        }

        private static float max_in_r = float.MaxValue;
        public static float MaxInR
        {
            get { return max_in_r; }
        }

        private static float max_out_l = float.MaxValue;
        public static float MaxOutL
        {
            get { return max_out_l; }
        }

        private static float max_out_r = float.MaxValue;
        public static float MaxOutR
        {
            get { return max_out_r; }
        }

        private static bool mute_output = false;
        public static bool MuteOutput
        {
            get { return mute_output; }
            set { mute_output = value; }
        }

        private static int ramp_time = 5; // in ms
        private static int ramp_samples; // total samples to complete the ramp
        private static int ramp_count = 0;
        private static double ramp_step;
        private static bool ramp = false;
        public static bool Ramp
        {
            get { return ramp; }
            set
            {                
                if(value)
                {
                    ramp_samples = (int)(ramp_time * 1e-3 * sample_rate1);
                    ramp_step = Math.PI / 2.0 / ramp_samples;
                    ramp_count = 0;
                }
                ramp = value;
            }
        }

        #endregion

        #region Callback Routines




        //==============================================================================================================         
        // ke9ns   not used for Flex radios ? (use callback2 or callback1500)
        //==============================================================================================================         
        unsafe public static int Callback1(void* input, void* output, int frameCount,	PA19.PaStreamCallbackTimeInfo* timeInfo, int statusFlags, void *userData)
		{
#if(TIMER)
			t1.Start();
#endif
         //  Debug.WriteLine("Callback1=================================");

			int* array_ptr = (int *)input;
			float* in_l_ptr1 = (float *)array_ptr[0];
			float* in_r_ptr1 = (float *)array_ptr[1];
			array_ptr = (int *)output;
			float* out_l_ptr1 = (float *)array_ptr[0];
			float* out_r_ptr1 = (float *)array_ptr[1];

            float* in_l = null, in_r = null, out_l = null, out_r = null;

            in_l = in_l_ptr1;
            in_r = in_r_ptr1;

            out_l = out_l_ptr1;
            out_r = out_r_ptr1;

            localmox = mox;

       
			if(wave_playback)	wave_file_reader.GetPlayBuffer(in_l_ptr1, in_r_ptr1);

            if ((wave_record && !localmox && record_rx_preprocessed) || (wave_record && localmox && record_tx_preprocessed))
				wave_file_writer.AddWriteBuffer(in_l_ptr1, in_r_ptr1);			

			if(phase)
			{
				//phase_mutex.WaitOne();
				Marshal.Copy(new IntPtr(in_l_ptr1), phase_buf_l, 0, frameCount);
				Marshal.Copy(new IntPtr(in_r_ptr1), phase_buf_r, 0, frameCount);
				//phase_mutex.ReleaseMutex();
			}

			// handle VAC1 Input
            if (vac_enabled && rb_vacOUT_l != null && rb_vacOUT_r != null)
            {
                if (vac_bypass || !localmox) // drain VAC Input ring buffer
                {
                    if ((rb_vacIN_l.ReadSpace() >= frameCount) && (rb_vacIN_r.ReadSpace() >= frameCount))
                    {
                        Win32.EnterCriticalSection(cs_vac);
                        rb_vacIN_l.ReadPtr(out_l_ptr1, frameCount);
                        rb_vacIN_r.ReadPtr(out_r_ptr1, frameCount);
                        Win32.LeaveCriticalSection(cs_vac);
                    }
                }
                else // VAC is on -- copy data for transmit mode
                {
                    if (rb_vacIN_l.ReadSpace() >= frameCount)
                    {
                        Win32.EnterCriticalSection(cs_vac);
                        rb_vacIN_l.ReadPtr(in_l, frameCount);
                        rb_vacIN_r.ReadPtr(in_r, frameCount);
                        Win32.LeaveCriticalSection(cs_vac);
                        if (vac_combine_input)
                            AddBuffer(in_l, in_r, frameCount);

                        ScaleBuffer(in_l, in_l, frameCount, (float)vac_preamp);
                        ScaleBuffer(in_r, in_r, frameCount, (float)vac_preamp);
                    }
                    else
                    {
                        ClearBuffer(in_l, frameCount);
                        ClearBuffer(in_r, frameCount);
                        VACDebug("rb_vacIN underflow 4inTX");
                    }
                }

            } // vac1 on 



            // handle VAC2 Input
            if (vac2_enabled && rb_vac2OUT_l != null && rb_vac2OUT_r != null)
            {
                if (vac_bypass || !localmox || !vfob_tx) // drain VAC2 Input ring buffer
                {
                    if ((rb_vac2IN_l.ReadSpace() >= frameCount) && (rb_vac2IN_r.ReadSpace() >= frameCount))
                    {
                        Win32.EnterCriticalSection(cs_vac2);
                        rb_vac2IN_l.ReadPtr(out_l_ptr1, frameCount);
                        rb_vac2IN_r.ReadPtr(out_r_ptr1, frameCount);
                        Win32.LeaveCriticalSection(cs_vac2);
                    }
                }
                else // VAC2 is on -- copy data for transmit mode if VFO B TX is selected
                {
                    if (vfob_tx && rb_vac2IN_l.ReadSpace() >= frameCount)
                    {
                        Win32.EnterCriticalSection(cs_vac2);
                        rb_vac2IN_l.ReadPtr(in_l, frameCount);
                        rb_vac2IN_r.ReadPtr(in_r, frameCount);
                        Win32.LeaveCriticalSection(cs_vac2);
                        if (vac2_combine_input)
                            AddBuffer(in_l, in_r, frameCount);

                        ScaleBuffer(in_l, in_l, frameCount, (float)vac2_tx_scale);
                        ScaleBuffer(in_r, in_r, frameCount, (float)vac2_tx_scale);
                    }
                    else
                    {
                        ClearBuffer(in_l, frameCount);
                        ClearBuffer(in_r, frameCount);
                        VACDebug("rb_vac2IN underflow 4inTX");
                    }
                }
            } // vac2 on



            min_in_l = Math.Min(min_in_l, MinSample(in_l, frameCount));
            min_in_r = Math.Min(min_in_r, MinSample(in_r, frameCount));
            max_in_l = Math.Max(max_in_l, MaxSample(in_l, frameCount));
            max_in_r = Math.Max(max_in_r, MaxSample(in_r, frameCount));

			// scale input with mic preamp
            if (localmox && ((!vac_enabled &&
                (tx_dsp_mode == DSPMode.LSB ||
                tx_dsp_mode == DSPMode.USB ||
                tx_dsp_mode == DSPMode.DSB ||
                tx_dsp_mode == DSPMode.AM ||
                tx_dsp_mode == DSPMode.SAM ||
                tx_dsp_mode == DSPMode.FM ||
                tx_dsp_mode == DSPMode.DIGL ||
                tx_dsp_mode == DSPMode.DIGU)) ||
                (vac_enabled && vac_bypass &&
                (tx_dsp_mode == DSPMode.DIGL ||
                tx_dsp_mode == DSPMode.DIGU ||
                tx_dsp_mode == DSPMode.LSB ||
                tx_dsp_mode == DSPMode.USB ||
                tx_dsp_mode == DSPMode.DSB ||
                tx_dsp_mode == DSPMode.AM ||
                tx_dsp_mode == DSPMode.SAM ||
                tx_dsp_mode == DSPMode.FM))))
            {
                if (wave_playback)
                {
                    ScaleBuffer(in_l, in_l, frameCount, (float)wave_preamp);
                    ScaleBuffer(in_r, in_r, frameCount, (float)wave_preamp);
                }
                else
                {
                    if (!vac_enabled && (tx_dsp_mode == DSPMode.DIGL || tx_dsp_mode == DSPMode.DIGU  || (tx_dsp_mode == DSPMode.FM && console.FMData == true && console.setupForm.chkFMDataMic.Checked == false))   )
                    {
                        ScaleBuffer(in_l, in_l, frameCount, (float)vac_preamp);
                        ScaleBuffer(in_r, in_r, frameCount, (float)vac_preamp);
                       
                    }
                    else
                    {
                        ScaleBuffer(in_l, in_l, frameCount, (float)mic_preamp);
                        ScaleBuffer(in_r, in_r, frameCount, (float)mic_preamp);

                     //   if ( (console.TXMeter2 == true) && (console.CurrentMeterTX1Mode == MeterTXMode.MIC)) peak1 = MaxSample(in_l, in_r, frameCount); // ke9ns add to allow for MIC level check in RX mode

                    }
                }
            }

			#region Input Signal Source

			if(!localmox)
			{
				switch(rx1_input_signal)  // ke9ns selected from setup->test->receiver screen
				{
					case SignalSource.RADIO:
						break;
					case SignalSource.SINE:


                        SineWave(in_l, frameCount, phase_accumulator1, sine_freq1);
						phase_accumulator1 = CosineWave(in_r, frameCount, phase_accumulator1, sine_freq1);


                        ScaleBuffer(in_l, in_l, frameCount, (float)source_scale);
						ScaleBuffer(in_r, in_r, frameCount, (float)source_scale);
						break;
					case SignalSource.SINE_TWO_TONE:
						double dump;
						SineWave2Tone(in_l, frameCount, phase_accumulator1, phase_accumulator2,
							sine_freq1, sine_freq2, out dump, out dump);
						CosineWave2Tone(in_r, frameCount, phase_accumulator1, phase_accumulator2,
							sine_freq1, sine_freq2, out phase_accumulator1, out phase_accumulator2);
						ScaleBuffer(in_l, in_l, frameCount, (float)source_scale);
						ScaleBuffer(in_r, in_r, frameCount, (float)source_scale);
						break;
					case SignalSource.NOISE:
						Noise(in_l, frameCount);
						Noise(in_r, frameCount);
						break;
					case SignalSource.TRIANGLE:
						Triangle(in_l, frameCount, sine_freq1);
						CopyBuffer(in_l, in_r, frameCount);
						break;
					case SignalSource.SAWTOOTH:
						Sawtooth(in_l, frameCount, sine_freq1);
						CopyBuffer(in_l, in_r, frameCount);
						break;
                    case SignalSource.PULSE:
                        Pulse(in_l, frameCount);
                        CopyBuffer(in_l, in_r, frameCount);
                        ScaleBuffer(in_l, in_l, frameCount, (float)source_scale);
                        ScaleBuffer(in_r, in_r, frameCount, (float)source_scale);
                        break;
					case SignalSource.SILENCE:
						ClearBuffer(in_l, frameCount);
						ClearBuffer(in_r, frameCount);
						break;
				}
			}
			else
			{
				switch(tx_input_signal) // ke9ns selected from setup->test->transmitter screen
                {
					case SignalSource.RADIO:
						break;
					case SignalSource.SINE:


                        SineWave(in_l, frameCount, phase_accumulator1, sine_freq1);
						phase_accumulator1 = CosineWave(in_r, frameCount, phase_accumulator1, sine_freq1);


                        ScaleBuffer(in_l, in_l, frameCount, (float)source_scale);
						ScaleBuffer(in_r, in_r, frameCount, (float)source_scale);
						break;
					case SignalSource.SINE_TWO_TONE:
						double dump;
						SineWave2Tone(in_l, frameCount, phase_accumulator1, phase_accumulator2,
							sine_freq1, sine_freq2, out dump, out dump);
						CosineWave2Tone(in_r, frameCount, phase_accumulator1, phase_accumulator2,
							sine_freq1, sine_freq2, out phase_accumulator1, out phase_accumulator2);
						ScaleBuffer(in_l, in_l, frameCount, (float)source_scale);
						ScaleBuffer(in_r, in_r, frameCount, (float)source_scale);
						break;
					case SignalSource.NOISE:
						Noise(in_l, frameCount);
						Noise(in_r, frameCount);
						break;
					case SignalSource.TRIANGLE:
						Triangle(in_l, frameCount, sine_freq1);
						CopyBuffer(in_l, in_r, frameCount);
						break;
					case SignalSource.SAWTOOTH:
						Sawtooth(in_l, frameCount, sine_freq1);
						CopyBuffer(in_l, in_r, frameCount);
						break;
                    case SignalSource.PULSE:
                        Pulse(in_l, frameCount);
                        CopyBuffer(in_l, in_r, frameCount);
                        ScaleBuffer(in_l, in_l, frameCount, (float)source_scale);
                        ScaleBuffer(in_r, in_r, frameCount, (float)source_scale);
                        break;
					case SignalSource.SILENCE:
						ClearBuffer(in_l, frameCount);
						ClearBuffer(in_r, frameCount);
						break;
				}
			}

			#endregion

#if(MINMAX)
			Debug.Write(MaxSample(in_l, in_r, frameCount).ToString("f6")+",");
#endif
            // handle VAC1 output if doing Direct IQ
            if (vac_enabled && vac_output_iq &&
                rb_vacOUT_l != null && rb_vacOUT_r != null &&
                in_l != null && in_r != null)
            {
                if (!localmox)
                {
                    if ((rb_vacOUT_l.WriteSpace() >= frameCount) && (rb_vacOUT_r.WriteSpace() >= frameCount))
                    {
                        if (vac_correct_iq)
                            fixed (float* res_outl_ptr = &(res_outl[0]))
                            fixed (float* res_outr_ptr = &(res_outr[0]))
                            {
                                CorrectIQBuffer(in_l, in_r, res_outl_ptr, res_outr_ptr, frameCount);

                                Win32.EnterCriticalSection(cs_vac);
                                rb_vacOUT_l.WritePtr(res_outl_ptr, frameCount);
                                rb_vacOUT_r.WritePtr(res_outr_ptr, frameCount);
                                Win32.LeaveCriticalSection(cs_vac);
                            }
                        else
                        {
                            Win32.EnterCriticalSection(cs_vac);
                            rb_vacOUT_l.WritePtr(in_r, frameCount); // why are these reversed??
                            rb_vacOUT_r.WritePtr(in_l, frameCount);
                            Win32.LeaveCriticalSection(cs_vac);
                        }
                    }
                    else
                    {
                        VACDebug("rb_vacOUT_l I/Q overflow ");
                        vac_rb_reset = true;
                    }
                }
            } // vac1 IQ on



            // handle VAC2 output if doing Direct IQ
            if (vac2_enabled && vac2_output_iq &&
                rb_vac2OUT_l != null && rb_vac2OUT_r != null &&
                in_l != null && in_r != null)
            {
                if (!localmox)
                {
                    if ((rb_vac2OUT_l.WriteSpace() >= frameCount) && (rb_vac2OUT_r.WriteSpace() >= frameCount))
                    {
                        if (vac2_correct_iq)
                            fixed (float* res_outl_ptr = &(res_vac2_outl[0]))
                            fixed (float* res_outr_ptr = &(res_vac2_outr[0]))
                            {
                                CorrectIQBuffer(in_l, in_r, res_outl_ptr, res_outr_ptr, frameCount);

                                Win32.EnterCriticalSection(cs_vac2);
                                rb_vac2OUT_l.WritePtr(res_outl_ptr, frameCount);
                                rb_vac2OUT_r.WritePtr(res_outr_ptr, frameCount);
                                Win32.LeaveCriticalSection(cs_vac2);
                            }
                        else
                        {
                            Win32.EnterCriticalSection(cs_vac2);
                            rb_vac2OUT_l.WritePtr(in_r, frameCount); // why are these reversed??
                            rb_vac2OUT_r.WritePtr(in_l, frameCount);
                            Win32.LeaveCriticalSection(cs_vac2);
                        }
                    }
                    else
                    {
                        VACDebug("rb_vac2OUT_l I/Q overflow ");
                        vac2_rb_reset = true;
                    }
                }
            } // vac2 IQ on


            
            //DttSP.ExchangeSamples(in_l, in_r, out_l, out_r, frameCount);

            if (localmox && (tx_dsp_mode == DSPMode.CWU || tx_dsp_mode == DSPMode.CWL))
            {
                double time = CWSensorItem.GetCurrentTime();
                CWSynth.Advance(out_l, out_r, frameCount, time);
            }
            else if (tx_dsp_mode == DSPMode.CWU || tx_dsp_mode == DSPMode.CWL)
            {                        
                double time = CWSensorItem.GetCurrentTime();
                CWSynth.Advance(out_l, out_r, frameCount, time);

                DttSP.ExchangeSamples(in_l, in_r, out_l, out_r, frameCount);
            }
            else
            {
                DttSP.ExchangeSamples(in_l, in_r, out_l, out_r, frameCount);
            }
#if(MINMAX)
			Debug.WriteLine(MaxSample(out_l, out_r, frameCount));
#endif

			#region Output Signal Source

			if(!localmox)
			{
				switch(rx1_output_signal)  // ke9ns selected from setup->test->receiver screen

                {
					case SignalSource.RADIO:
						break;
					case SignalSource.SINE:
						SineWave(out_l_ptr1, frameCount, phase_accumulator1, sine_freq1);
						phase_accumulator1 = CosineWave(out_r_ptr1, frameCount, phase_accumulator1, sine_freq1);
						ScaleBuffer(out_l_ptr1, out_l_ptr1, frameCount, (float)source_scale);
						ScaleBuffer(out_r_ptr1, out_r_ptr1, frameCount, (float)source_scale);
						break;
					case SignalSource.SINE_TWO_TONE:
						double dump;
						SineWave2Tone(out_l_ptr1, frameCount, phase_accumulator1, phase_accumulator2,
							sine_freq1, sine_freq2, out dump, out dump);
						CosineWave2Tone(out_r_ptr1, frameCount, phase_accumulator1, phase_accumulator2,
							sine_freq1, sine_freq2, out phase_accumulator1, out phase_accumulator2);
						ScaleBuffer(out_l_ptr1, out_l_ptr1, frameCount, (float)source_scale);
						ScaleBuffer(out_r_ptr1, out_r_ptr1, frameCount, (float)source_scale);
						break;
					case SignalSource.NOISE:
						Noise(out_l_ptr1, frameCount);
						Noise(out_r_ptr1, frameCount);
						break;
					case SignalSource.TRIANGLE:
						Triangle(out_l_ptr1, frameCount, sine_freq1);
						CopyBuffer(out_l_ptr1, out_r_ptr1, frameCount);
						break;
					case SignalSource.SAWTOOTH:
						Sawtooth(out_l_ptr1, frameCount, sine_freq1);
						CopyBuffer(out_l_ptr1, out_r_ptr1, frameCount);
						break;
                    case SignalSource.PULSE:
                        Pulse(out_l_ptr1, frameCount);
                        CopyBuffer(out_l_ptr1, out_r_ptr1, frameCount);
                        ScaleBuffer(out_l_ptr1, out_l_ptr1, frameCount, (float)source_scale);
                        ScaleBuffer(out_r_ptr1, out_r_ptr1, frameCount, (float)source_scale);
                        break;
					case SignalSource.SILENCE:
						ClearBuffer(out_l_ptr1, frameCount);
						ClearBuffer(out_r_ptr1, frameCount);
						break;
				}
			}
			else
			{
				switch(tx_output_signal) // ke9ns selected from setup->test->transmitter screen
                {
					case SignalSource.RADIO:
						break;
					case SignalSource.SINE:


                        SineWave(out_l_ptr1, frameCount, phase_accumulator1, sine_freq1);
						phase_accumulator1 = CosineWave(out_l_ptr1, frameCount, phase_accumulator1, sine_freq1);


                        ScaleBuffer(out_l_ptr1, out_l_ptr1, frameCount, (float)source_scale);
						ScaleBuffer(out_r_ptr1, out_r_ptr1, frameCount, (float)source_scale);
						break;
					case SignalSource.SINE_TWO_TONE:
						double dump;
						SineWave2Tone(out_l_ptr1, frameCount, phase_accumulator1, phase_accumulator2,
							sine_freq1, sine_freq2, out dump, out dump);
						CosineWave2Tone(out_r_ptr1, frameCount, phase_accumulator1, phase_accumulator2,
							sine_freq1, sine_freq2, out phase_accumulator1, out phase_accumulator2);
						ScaleBuffer(out_l_ptr1, out_l_ptr1, frameCount, (float)source_scale);
						ScaleBuffer(out_r_ptr1, out_r_ptr1, frameCount, (float)source_scale);
						break;
					case SignalSource.NOISE:
						Noise(out_l_ptr1, frameCount);
						Noise(out_r_ptr1, frameCount);
						break;
					case SignalSource.TRIANGLE:
						Triangle(out_l_ptr1, frameCount, sine_freq1);
						CopyBuffer(out_l_ptr1, out_r_ptr1, frameCount);
						break;
					case SignalSource.SAWTOOTH:
						Sawtooth(out_l_ptr1, frameCount, sine_freq1);
						CopyBuffer(out_l_ptr1, out_r_ptr1, frameCount);
						break;
                    case SignalSource.PULSE:
                        Pulse(out_l_ptr1, frameCount);
                        CopyBuffer(out_l_ptr1, out_r_ptr1, frameCount);
                        ScaleBuffer(out_l_ptr1, out_l_ptr1, frameCount, (float)source_scale);
                        ScaleBuffer(out_r_ptr1, out_r_ptr1, frameCount, (float)source_scale);
                        break;
					case SignalSource.SILENCE:
						ClearBuffer(out_l_ptr1, frameCount);
						ClearBuffer(out_r_ptr1, frameCount);
						break;
				}
			}

			#endregion

			DoScope(out_l_ptr1, frameCount);   // ke9ns   show in scope mode ?

            if (wave_record)  // ke9ns using audio recorder
            {
                if (!localmox)
                {
                    if (!record_rx_preprocessed)
                    {
                        wave_file_writer.AddWriteBuffer(out_l, out_r);


                    }
                }
                else
                {
                    if (!record_tx_preprocessed)
                    {
                        wave_file_writer.AddWriteBuffer(out_l, out_r);
                    }
                }
            }


            // scale output for VAC1 -- use input as spare buffer
			if(vac_enabled && !vac_output_iq &&
				rb_vacIN_l != null && rb_vacIN_r != null && 
				rb_vacOUT_l != null && rb_vacOUT_r != null)
			{
                if (!localmox)
                {

                        ScaleBuffer(out_l, in_l, frameCount, (float)vac_rx_scale);
                        ScaleBuffer(out_r, in_r, frameCount, (float)vac_rx_scale);
                    
                }
                else if (mon)
                {
                    if (tx_dsp_mode == DSPMode.AM || tx_dsp_mode == DSPMode.SAM || tx_dsp_mode == DSPMode.FM)  // ke9ns add  use pre-processed audio for MON function in these modes only
                    {
                        ScaleBuffer(in_l_ptr1, in_l, frameCount, (float)vac_rx_scale); // ke9ns add dont have a tx_in_l
                        ScaleBuffer(in_r_ptr1, in_r, frameCount, (float)vac_rx_scale);

                     //   Trace.Write("help====");

                    }
                    else
                    {
                        ScaleBuffer(out_l, in_l, frameCount, (float)vac_rx_scale);
                        ScaleBuffer(out_r, in_r, frameCount, (float)vac_rx_scale);
                    }
                }
                else // zero samples going back to VAC since TX monitor is off
                {
                    ScaleBuffer(out_l, in_l, frameCount, 0.0f);
                    ScaleBuffer(out_r, in_r, frameCount, 0.0f);
                }

				if (sample_rate2 == sample_rate1) // no resample necessary  ke9ns (if primary audio and vac1 match then do below)
				{
					if ((rb_vacOUT_l.WriteSpace() >= frameCount) && (rb_vacOUT_r.WriteSpace() >= frameCount))
					{
                       
                            Win32.EnterCriticalSection(cs_vac);        // ke9ns Waits for ownership of the specified critical section object
                            rb_vacOUT_l.WritePtr(in_l, frameCount);  // 
                            rb_vacOUT_r.WritePtr(in_r, frameCount);
                            Win32.LeaveCriticalSection(cs_vac);
                        

					}
					else 
					{
						VACDebug("rb_vacOUT_l overflow");
						VACDebug("rb_vacOUT_r overflow");
					}
				} // match SR 
				else 
				{
					if (vac_stereo) 
					{						
						fixed(float *res_outl_ptr = &(res_outl[0]))
							fixed(float *res_outr_ptr = &(res_outr[0])) 
							{
								int outsamps;
								DttSP.DoResamplerF(in_l, res_outl_ptr, frameCount, &outsamps, resampPtrOut_l);
								DttSP.DoResamplerF(in_r, res_outr_ptr, frameCount, &outsamps, resampPtrOut_r);
								if ((rb_vacOUT_l.WriteSpace() >= outsamps) && (rb_vacOUT_r.WriteSpace() >= outsamps))
								{
									Win32.EnterCriticalSection(cs_vac);
									rb_vacOUT_l.WritePtr(res_outl_ptr, outsamps);
									rb_vacOUT_r.WritePtr(res_outr_ptr, outsamps);
									Win32.LeaveCriticalSection(cs_vac);
								}
								else 
								{
									VACDebug("rb_vacOUT_l overflow");
									VACDebug("rb_vacOUT_r overflow");
								}
							}
					}
					else 
					{
						fixed(float *res_outl_ptr = &(res_outl[0]))
						{
							int outsamps;
							DttSP.DoResamplerF(in_l, res_outl_ptr, frameCount, &outsamps, resampPtrOut_l);
							if ((rb_vacOUT_l.WriteSpace() >= outsamps) && (rb_vacOUT_r.WriteSpace() >= outsamps))
							{
								Win32.EnterCriticalSection(cs_vac);
								rb_vacOUT_l.WritePtr(res_outl_ptr, outsamps);
								rb_vacOUT_r.WritePtr(res_outl_ptr, outsamps);
								Win32.LeaveCriticalSection(cs_vac);
							}
							else
							{
								VACDebug("rb_vacOUT_l overflow");
								VACDebug("rb_vacOUT_r overflow");
							}
						}
					}
				}

			} // vac1 on




            // scale output for VAC2 -- use input as spare buffer
            if (vac2_enabled && !vac2_output_iq &&
                rb_vac2IN_l != null && rb_vac2IN_r != null &&
                rb_vac2OUT_l != null && rb_vac2OUT_r != null)
            {
                ScaleBuffer(out_l, in_l, frameCount, (float)vac2_rx_scale);
                ScaleBuffer(out_r, in_r, frameCount, (float)vac2_rx_scale);

                if (sample_rate3 == sample_rate1) // no resample necessary
                {
                    if ((rb_vac2OUT_l.WriteSpace() >= frameCount) && (rb_vac2OUT_r.WriteSpace() >= frameCount))
                    {
                        Win32.EnterCriticalSection(cs_vac2);
                        rb_vac2OUT_l.WritePtr(in_l, frameCount);
                        rb_vac2OUT_r.WritePtr(in_r, frameCount);
                        Win32.LeaveCriticalSection(cs_vac2);
                    }
                    else
                    {
                        VACDebug("rb_vac2OUT_l overflow");
                        VACDebug("rb_vac2OUT_r overflow");
                    }
                }
                else
                {
                    if (vac2_stereo)
                    {
                        fixed (float* res_outl_ptr = &(res_vac2_outl[0]))
                        fixed (float* res_outr_ptr = &(res_vac2_outr[0]))
                        {
                            int outsamps;
                            DttSP.DoResamplerF(in_l, res_outl_ptr, frameCount, &outsamps, resampVAC2PtrOut_l);
                            DttSP.DoResamplerF(in_r, res_outr_ptr, frameCount, &outsamps, resampVAC2PtrOut_r);
                            if ((rb_vac2OUT_l.WriteSpace() >= outsamps) && (rb_vac2OUT_r.WriteSpace() >= outsamps))
                            {
                                Win32.EnterCriticalSection(cs_vac2);
                                rb_vac2OUT_l.WritePtr(res_outl_ptr, outsamps);
                                rb_vac2OUT_r.WritePtr(res_outr_ptr, outsamps);
                                Win32.LeaveCriticalSection(cs_vac2);
                            }
                            else
                            {
                                VACDebug("rb_vac2OUT_l overflow");
                                VACDebug("rb_vac2OUT_r overflow");
                            }
                        }
                    }
                    else
                    {
                        fixed (float* res_outl_ptr = &(res_vac2_outl[0]))
                        {
                            int outsamps;
                            DttSP.DoResamplerF(in_l, res_outl_ptr, frameCount, &outsamps, resampVAC2PtrOut_l);
                            if ((rb_vac2OUT_l.WriteSpace() >= outsamps) && (rb_vac2OUT_r.WriteSpace() >= outsamps))
                            {
                                Win32.EnterCriticalSection(cs_vac2);
                                rb_vac2OUT_l.WritePtr(res_outl_ptr, outsamps);
                                rb_vac2OUT_r.WritePtr(res_outl_ptr, outsamps);
                                Win32.LeaveCriticalSection(cs_vac2);
                            }
                            else
                            {
                                VACDebug("rb_vac2OUT_l overflow");
                                VACDebug("rb_vac2OUT_r overflow");
                            }
                        }
                    }
                }
            }

          

            double vol = monitor_volume;

           

            if (localmox)
            {
                if (tx_output_signal != SignalSource.RADIO)   vol = 1.0f;
                else   vol = TXScale;
            }
            else // receive
            {
                if (rx1_output_signal != SignalSource.RADIO)  vol = 1.0f;  // ke9ns do this is you dont select the radio as the output of audio that goes to the speaker
            }

            if (vol != 1.0)
            {
                ScaleBuffer(out_l, out_l, frameCount, (float)vol);  // ke9ns do this if Radio is where the speaker audio comes from (setup->test->receiver->radio)
                ScaleBuffer(out_r, out_r, frameCount, (float)vol);
            }

#if(MINMAX)
			Debug.Write(MaxSample(out_l, out_r, frameCount).ToString("f6")+",");

			float current_max = MaxSample(out_l, out_r, frameCount);
			if(current_max > max) max = current_max;
			Debug.WriteLine(" max: "+max.ToString("f6"));
#endif

			/*if((wave_record && !localmox && !record_rx_preprocessed) ||
				(wave_record && localmox && !record_tx_preprocessed))
				wave_file_writer.AddWriteBuffer(out_l_ptr1, out_r_ptr1);*/
#if(TIMER)
			t1.Stop();
			Debug.WriteLine(t1.Duration);
#endif

            min_out_l = Math.Min(min_out_l, MinSample(out_l, frameCount));
            min_out_r = Math.Min(min_out_r, MinSample(out_r, frameCount));
            max_out_l = Math.Max(max_out_l, MaxSample(out_l, frameCount));
            max_out_r = Math.Max(max_out_r, MaxSample(out_r, frameCount));

			return callback_return;
		}  // Callback1


      
        
        //==============================================================================================================         
        // ke9ns routine for flex1500 (instead of callback2 below)
        //==============================================================================================================         
        unsafe public static int Callback1500(float[] AudioInBuf1, float[] AudioInBuf2,float[] AudioOutBuf1, float[] AudioOutBuf2, uint paFlags)
        {
#if(TIMER)
			t1.Start();
#endif
          

            /*int* array_ptr = (int*)input;
            float* in_l_ptr1 = (float*)array_ptr[0];
            float* in_r_ptr1 = (float*)array_ptr[1];
            array_ptr = (int*)output;
            float* out_l_ptr1 = (float*)array_ptr[0];
            float* out_r_ptr1 = (float*)array_ptr[1];*/

            Debug.Assert(AudioInBuf1.Length == AudioInBuf2.Length);
            Debug.Assert(AudioOutBuf1.Length == AudioOutBuf2.Length);
            Debug.Assert(AudioInBuf1.Length == AudioOutBuf1.Length);

            int frameCount = AudioInBuf1.Length;

            localmox = mox;


             float[] audiotemp = new float[frameCount+1]; // ke9ns add
             float[] audiotemp1 = new float[frameCount + 1]; // ke9ns add

            fixed (float* in_tl_ptr1 = &audiotemp[0]) // ke9ns add temp ptr to preprocessed data
            fixed (float* in_tr_ptr1 = &audiotemp1[0]) // ke9ns add temp ptr to preprocessed data

            fixed (float* in_l_ptr1 = &AudioInBuf1[0])
            fixed(float* in_r_ptr1 = &AudioInBuf2[0])
            fixed(float* out_l_ptr1 = &AudioOutBuf1[0])
            fixed(float* out_r_ptr1 = &AudioOutBuf2[0])
            {
                float* in_l = null, in_r = null, out_l = null, out_r = null;

                float* in_t = null; //ke9ns add temp pointer

                //ClearBuffer(in_l_ptr1, frameCount);
                //ClearBuffer(in_r_ptr1, frameCount);

                in_l = in_l_ptr1;
                in_r = in_r_ptr1;

                out_l = out_l_ptr1;
                out_r = out_r_ptr1;

              

                if (wave_playback)
                {
                    wave_file_reader.GetPlayBuffer(in_l_ptr1, in_r_ptr1);
                }

                if ((wave_record && !mox && record_rx_preprocessed) || (wave_record && mox && record_tx_preprocessed))
                {
                    wave_file_writer.AddWriteBuffer(in_l_ptr1, in_r_ptr1); // ke9ns this is preprocessed audio
                }



                if (phase)
                {
                    //phase_mutex.WaitOne();
                    Marshal.Copy(new IntPtr(in_l_ptr1), phase_buf_l, 0, frameCount);
                    Marshal.Copy(new IntPtr(in_r_ptr1), phase_buf_r, 0, frameCount);
                    //phase_mutex.ReleaseMutex();
                }



                //============================================================================
                // handle VAC Input
                if (vac_enabled && rb_vacOUT_l != null && rb_vacOUT_r != null)
                {
                   // Debug.WriteLine("1500 VAC1");

                    if (vac_bypass || !localmox) // drain VAC Input ring buffer
                    {
                        if ((rb_vacIN_l.ReadSpace() >= frameCount) && (rb_vacIN_r.ReadSpace() >= frameCount))
                        {
                            Win32.EnterCriticalSection(cs_vac);
                            rb_vacIN_l.ReadPtr(out_l_ptr1, frameCount);
                            rb_vacIN_r.ReadPtr(out_r_ptr1, frameCount);
                            Win32.LeaveCriticalSection(cs_vac);
                        }
                    }
                    else // VAC is on -- copy data for transmit mode
                    {
                        if (rb_vacIN_l.ReadSpace() >= frameCount)
                        {
                            Win32.EnterCriticalSection(cs_vac);
                            rb_vacIN_l.ReadPtr(in_l_ptr1, frameCount);
                            rb_vacIN_r.ReadPtr(in_r_ptr1, frameCount);
                            Win32.LeaveCriticalSection(cs_vac);
                            if (vac_combine_input)
                                AddBuffer(in_l_ptr1, in_r_ptr1, frameCount);

                            ScaleBuffer(in_l_ptr1, in_l_ptr1, frameCount, (float)vac_preamp);
                            ScaleBuffer(in_r_ptr1, in_r_ptr1, frameCount, (float)vac_preamp);
                        }
                        else
                        {
                            ClearBuffer(in_l_ptr1, frameCount);
                            ClearBuffer(in_r_ptr1, frameCount);
                            VACDebug("rb_vacIN underflow 4inTX");
                        }
                    }
                } // VAC1 on 

             //   if (mon && mox) // ke9ns this needs to be here(below VAC1 input) to grab the redirect from the VAC1
              //  {

                //    if (tx_dsp_mode == DSPMode.AM || tx_dsp_mode == DSPMode.SAM || tx_dsp_mode == DSPMode.FM)  // ke9ns add  use pre-processed audio for MON function in these modes only
                 //   {
                        //  Trace.Write("first=======");
                   //     ScaleBuffer(in_l_ptr1, in_tl_ptr1, frameCount, 1.0f); // ke9ns add save a copy of the preprocess audio for MON output at the bottom of this routine
                    //    ScaleBuffer(in_r_ptr1, in_tr_ptr1, frameCount, 1.0f); // ke9ns add save a copy of the preprocess audio for MON output at the bottom of this routine

                  //  }


              //  }

                //===============================================================================================
                // handle VAC2 Input
                if (vac2_enabled &&  rb_vac2OUT_l != null && rb_vac2OUT_r != null)
                {
                 //   Debug.WriteLine("1500 VAC2");

                    if (vac_bypass || !localmox || !vfob_tx) // drain VAC2 Input ring buffer
                    {
                        if ((rb_vac2IN_l.ReadSpace() >= frameCount) && (rb_vac2IN_r.ReadSpace() >= frameCount))
                        {
                            Win32.EnterCriticalSection(cs_vac2);
                            rb_vac2IN_l.ReadPtr(out_l_ptr1, frameCount);
                            rb_vac2IN_r.ReadPtr(out_r_ptr1, frameCount);
                            Win32.LeaveCriticalSection(cs_vac2);
                        }
                    }
                    else // VAC is on -- copy data for transmit mode if VFOB TX is on
                    {
                        if (vfob_tx && rb_vac2IN_l.ReadSpace() >= frameCount)
                        {
                            Win32.EnterCriticalSection(cs_vac2);
                            rb_vac2IN_l.ReadPtr(in_l_ptr1, frameCount);
                            rb_vac2IN_r.ReadPtr(in_r_ptr1, frameCount);
                            Win32.LeaveCriticalSection(cs_vac2);
                            if (vac2_combine_input)
                                AddBuffer(in_l_ptr1, in_r_ptr1, frameCount);

                            ScaleBuffer(in_l_ptr1, in_l_ptr1, frameCount, (float)vac2_tx_scale);
                            ScaleBuffer(in_r_ptr1, in_r_ptr1, frameCount, (float)vac2_tx_scale);
                        }
                        else
                        {
                            ClearBuffer(in_l_ptr1, frameCount);
                            ClearBuffer(in_r_ptr1, frameCount);
                            VACDebug("rb_vacIN underflow 4inTX");
                        }
                    }
                } // VAC2 on




                min_in_l = Math.Min(min_in_l, MinSample(in_l, frameCount));
                min_in_r = Math.Min(min_in_r, MinSample(in_r, frameCount));
                max_in_l = Math.Max(max_in_l, MaxSample(in_l, frameCount));
                max_in_r = Math.Max(max_in_r, MaxSample(in_r, frameCount));

                // scale input with mic preamp
                if (localmox && ((!vac_enabled &&
                (tx_dsp_mode == DSPMode.LSB ||
                tx_dsp_mode == DSPMode.USB ||
                tx_dsp_mode == DSPMode.DSB ||
                tx_dsp_mode == DSPMode.AM ||
                tx_dsp_mode == DSPMode.SAM ||
                tx_dsp_mode == DSPMode.FM ||
                tx_dsp_mode == DSPMode.DIGL ||
                tx_dsp_mode == DSPMode.DIGU)) ||
                (vac_enabled && vac_bypass &&
                (tx_dsp_mode == DSPMode.DIGL ||
                tx_dsp_mode == DSPMode.DIGU ||
                tx_dsp_mode == DSPMode.LSB ||
                tx_dsp_mode == DSPMode.USB ||
                tx_dsp_mode == DSPMode.DSB ||
                tx_dsp_mode == DSPMode.AM ||
                tx_dsp_mode == DSPMode.SAM ||
                tx_dsp_mode == DSPMode.FM))))
                {
                    if (wave_playback)
                    {
                        ScaleBuffer(in_l, in_l, frameCount, (float)wave_preamp);
                        ScaleBuffer(in_r, in_r, frameCount, (float)wave_preamp);
                    }
                    else
                    {
                        if (!vac_enabled && (tx_dsp_mode == DSPMode.DIGL || tx_dsp_mode == DSPMode.DIGU || (tx_dsp_mode == DSPMode.FM && console.FMData == true && console.setupForm.chkFMDataMic.Checked == false))   )  // ke9ns mod for FMData
                        {
                            ScaleBuffer(in_l, in_l, frameCount, (float)vac_preamp);
                            ScaleBuffer(in_r, in_r, frameCount, (float)vac_preamp);
                          
                        }
                        else
                        {
                          
                            if (console.setupForm.chkPhaseRotate.Checked == true) // ke9ns add phase rotation
                            {

                                PhaseRotate(in_l, in_l, frameCount, (float)mic_preamp);
                                PhaseRotate(in_r, in_r, frameCount, (float)mic_preamp);

                            }
                            else
                            {
                                ScaleBuffer(in_l, in_l, frameCount, (float)mic_preamp);
                                ScaleBuffer(in_r, in_r, frameCount, (float)mic_preamp);


                            }



                            //  if ((console.TXMeter2 == true) && (console.CurrentMeterTX1Mode == MeterTXMode.MIC)) peak1 = MaxSample(in_l, in_r, frameCount); // ke9ns add to allow for MIC level check in RX mode
                        }
                    }
                }

                #region Input Signal Source

                if (!mox)
                {
                    switch (rx1_input_signal)
                    {
                        case SignalSource.RADIO:
                            break;
                        case SignalSource.SINE:
                            SineWave(in_l, frameCount, phase_accumulator1, sine_freq1);
                            phase_accumulator1 = CosineWave(in_r, frameCount, phase_accumulator1, sine_freq1);
                            ScaleBuffer(in_l, in_l, frameCount, (float)source_scale);
                            ScaleBuffer(in_r, in_r, frameCount, (float)source_scale);
                            break;
                        case SignalSource.SINE_TWO_TONE:
                            double dump;
                            SineWave2Tone(in_l, frameCount, phase_accumulator1, phase_accumulator2,
                                sine_freq1, sine_freq2, out dump, out dump);
                            CosineWave2Tone(in_r, frameCount, phase_accumulator1, phase_accumulator2,
                                sine_freq1, sine_freq2, out phase_accumulator1, out phase_accumulator2);
                            ScaleBuffer(in_l, in_l, frameCount, (float)source_scale);
                            ScaleBuffer(in_r, in_r, frameCount, (float)source_scale);
                            break;
                        case SignalSource.SINE_LEFT_ONLY:
                            phase_accumulator1 = SineWave(in_l, frameCount, phase_accumulator1, sine_freq1);
                            ScaleBuffer(in_l, in_l, frameCount, (float)source_scale);
                            ClearBuffer(in_r, frameCount);
                            break;
                        case SignalSource.SINE_RIGHT_ONLY:
                            phase_accumulator1 = SineWave(in_r, frameCount, phase_accumulator1, sine_freq1);
                            ScaleBuffer(in_r, in_r, frameCount, (float)source_scale);
                            ClearBuffer(in_l, frameCount);
                            break;
                        case SignalSource.NOISE:
                            Noise(in_l, frameCount);
                            Noise(in_r, frameCount);
                            break;
                        case SignalSource.TRIANGLE:
                            Triangle(in_l, frameCount, sine_freq1);
                            CopyBuffer(in_l, in_r, frameCount);
                            break;
                        case SignalSource.SAWTOOTH:
                            Sawtooth(in_l, frameCount, sine_freq1);
                            CopyBuffer(in_l, in_r, frameCount);
                            break;
                        case SignalSource.PULSE:
                            Pulse(in_l, frameCount);
                            CopyBuffer(in_l, in_r, frameCount);
                            ScaleBuffer(in_l, in_l, frameCount, (float)source_scale);
                            ScaleBuffer(in_r, in_r, frameCount, (float)source_scale);
                            break;
                        case SignalSource.SILENCE:
                            ClearBuffer(in_l, frameCount);
                            ClearBuffer(in_r, frameCount);
                            break;
                    }
                }
                else
                {
                    switch (tx_input_signal)
                    {
                        case SignalSource.RADIO:

                           
                                if (console.keydot == true)// ke9ns add allow cw while talking
                                {
                                    SineWave(in_l, frameCount, phase_accumulator1, (double) console.udCWPitch.Value);
                                    phase_accumulator1 = CosineWave(in_r, frameCount, phase_accumulator1, (double)console.udCWPitch.Value);
                                }
                           


                            break;
                        case SignalSource.SINE:

                            if ((console.PulseON == true))  // ke9ns add
                            {
                                Pulser(in_l, frameCount, sine_freq1);  // ke9ns add pulser function    
                                CopyBuffer(in_l, in_r, frameCount);
                            }
                            else
                            {
                                SineWave(in_l, frameCount, phase_accumulator1, sine_freq1);
                                phase_accumulator1 = CosineWave(in_r, frameCount, phase_accumulator1, sine_freq1);
                            }


                            ScaleBuffer(in_l, in_l, frameCount, (float)source_scale);
                            ScaleBuffer(in_r, in_r, frameCount, (float)source_scale);
                            break;
                        case SignalSource.SINE_TWO_TONE:
                            double dump;
                            SineWave2Tone(in_l, frameCount, phase_accumulator1, phase_accumulator2,
                                sine_freq1, sine_freq2, out dump, out dump);
                            CosineWave2Tone(in_r, frameCount, phase_accumulator1, phase_accumulator2,
                                sine_freq1, sine_freq2, out phase_accumulator1, out phase_accumulator2);
                            ScaleBuffer(in_l, in_l, frameCount, (float)source_scale);
                            ScaleBuffer(in_r, in_r, frameCount, (float)source_scale);
                            break;
                        case SignalSource.NOISE:
                            Noise(in_l, frameCount);
                            Noise(in_r, frameCount);
                            break;
                        case SignalSource.TRIANGLE:
                            Triangle(in_l, frameCount, sine_freq1);
                            CopyBuffer(in_l, in_r, frameCount);
                            break;
                        case SignalSource.SAWTOOTH:
                            Sawtooth(in_l, frameCount, sine_freq1);
                            CopyBuffer(in_l, in_r, frameCount);
                            break;
                        case SignalSource.PULSE:
                            Pulse(in_l, frameCount);
                            CopyBuffer(in_l, in_r, frameCount);
                            ScaleBuffer(in_l, in_l, frameCount, (float)source_scale);
                            ScaleBuffer(in_r, in_r, frameCount, (float)source_scale);
                            break;
                        case SignalSource.SILENCE:
                            ClearBuffer(in_l, frameCount);
                            ClearBuffer(in_r, frameCount);
                            break;
                    }
                }

                #endregion

#if(MINMAX)
			Debug.Write(MaxSample(in_l, in_r, frameCount).ToString("f6")+",");
#endif
               
                //=========================================================================
                //handle VAC if Direct IQ is on
                if (vac_enabled && vac_output_iq &&
                rb_vacOUT_l != null && rb_vacOUT_r != null &&
                in_l != null && in_r != null)
                {
                    if (!localmox)
                    {
                        if ((rb_vacOUT_l.WriteSpace() >= frameCount) && (rb_vacOUT_r.WriteSpace() >= frameCount))
                        {
                            if (vac_correct_iq)
                                fixed (float* res_outl_ptr = &(res_outl[0]))
                                fixed (float* res_outr_ptr = &(res_outr[0]))
                                {
                                    CorrectIQBuffer(in_l, in_r, res_outl_ptr, res_outr_ptr, frameCount);

                                    Win32.EnterCriticalSection(cs_vac);
                                    rb_vacOUT_l.WritePtr(res_outl_ptr, frameCount);
                                    rb_vacOUT_r.WritePtr(res_outr_ptr, frameCount);
                                    Win32.LeaveCriticalSection(cs_vac);
                                }
                            else
                            {
                                Win32.EnterCriticalSection(cs_vac);
                                rb_vacOUT_l.WritePtr(in_r, frameCount); // why are these reversed??
                                rb_vacOUT_r.WritePtr(in_l, frameCount);
                                Win32.LeaveCriticalSection(cs_vac);
                            }
                        }
                        else
                        {
                            VACDebug("rb_vacOUT_l I/Q overflow ");
                            vac_rb_reset = true;
                        }
                    }
                }



                //================================================================================================
                //handle VAC2 if Direct IQ is on
                if (vac2_enabled && vac2_output_iq &&
                    rb_vac2OUT_l != null && rb_vac2OUT_r != null &&
                    in_l != null && in_r != null)
                {
                    if (!localmox)
                    {
                        if ((rb_vac2OUT_l.WriteSpace() >= frameCount) && (rb_vac2OUT_r.WriteSpace() >= frameCount))
                        {
                            if (vac2_correct_iq)
                            {
                                fixed (float* res_outl_ptr = &(res_vac2_outl[0]))
                                fixed (float* res_outr_ptr = &(res_vac2_outr[0]))
                                {
                                    CorrectIQBuffer(in_l, in_r, res_outl_ptr, res_outr_ptr, frameCount);

                                    Win32.EnterCriticalSection(cs_vac2);
                                    rb_vac2OUT_l.WritePtr(res_outl_ptr, frameCount);
                                    rb_vac2OUT_r.WritePtr(res_outr_ptr, frameCount);
                                    Win32.LeaveCriticalSection(cs_vac2);
                                }
                            }
                            else
                            {
                                Win32.EnterCriticalSection(cs_vac2);
                                rb_vac2OUT_l.WritePtr(in_r, frameCount); // why are these reversed??
                                rb_vac2OUT_r.WritePtr(in_l, frameCount);
                                Win32.LeaveCriticalSection(cs_vac2);
                            }
                        }
                        else
                        {
                            VACDebug("rb_vac2OUT_l I/Q overflow ");
                            vac2_rb_reset = true;
                        }
                    }
                } // vac2 on with IQ


            //========================================================================
              
                if (localmox && (tx_dsp_mode == DSPMode.CWL || tx_dsp_mode == DSPMode.CWU))
                {
                    if (!tx_1500_image_cal)
                    {
                        double time = CWSensorItem.GetCurrentTime();
                        CWSynth.Advance(out_l, out_r, frameCount, time);
                    }
                }
                else if (tx_dsp_mode == DSPMode.CWL || tx_dsp_mode == DSPMode.CWU)
                {
                    
                    if (!tx_1500_image_cal)
                    {
                        double time = CWSensorItem.GetCurrentTime();
                        CWSynth.Advance(out_l, out_r, frameCount, time);
                    }

                    DttSP.ExchangeSamples(in_l, in_r, out_l, out_r, frameCount);
                }
                else
                {
                    DttSP.ExchangeSamples(in_l, in_r, out_l, out_r, frameCount);
                }

                if (tx_1500_image_cal)
                {
                    double time = CWSensorItem.GetCurrentTime();
                    CWSynth.Advance(out_l, out_r, frameCount, time);

                    ScaleBuffer(out_l, out_l, frameCount, (float)source_scale);
                    ScaleBuffer(out_r, out_r, frameCount, (float)source_scale);
                }

#if(MINMAX)
			Debug.WriteLine(MaxSample(out_l, out_r, frameCount));
#endif

                #region Output Signal Source

                if (!mox)
                {
                    switch (rx1_output_signal)
                    {
                        case SignalSource.RADIO:
                            break;
                        case SignalSource.SINE:
                            SineWave(out_l_ptr1, frameCount, phase_accumulator1, sine_freq1);
                            phase_accumulator1 = CosineWave(out_r_ptr1, frameCount, phase_accumulator1, sine_freq1);
                            ScaleBuffer(out_l_ptr1, out_l_ptr1, frameCount, (float)source_scale);
                            ScaleBuffer(out_r_ptr1, out_r_ptr1, frameCount, (float)source_scale);
                            break;
                        case SignalSource.SINE_TWO_TONE:
                            double dump;
                            SineWave2Tone(out_l_ptr1, frameCount, phase_accumulator1, phase_accumulator2,
                                sine_freq1, sine_freq2, out dump, out dump);
                            CosineWave2Tone(out_r_ptr1, frameCount, phase_accumulator1, phase_accumulator2,
                                sine_freq1, sine_freq2, out phase_accumulator1, out phase_accumulator2);
                            ScaleBuffer(out_l_ptr1, out_l_ptr1, frameCount, (float)source_scale);
                            ScaleBuffer(out_r_ptr1, out_r_ptr1, frameCount, (float)source_scale);
                            break;
                        case SignalSource.SINE_LEFT_ONLY:
                            phase_accumulator1 = SineWave(out_l_ptr1, frameCount, phase_accumulator1, sine_freq1);
                            ScaleBuffer(out_l_ptr1, out_l_ptr1, frameCount, (float)source_scale);
                            ClearBuffer(out_r_ptr1, frameCount);
                            break;
                        case SignalSource.SINE_RIGHT_ONLY:
                            phase_accumulator1 = SineWave(out_r_ptr1, frameCount, phase_accumulator1, sine_freq1);
                            ScaleBuffer(out_r_ptr1, out_r_ptr1, frameCount, (float)source_scale);
                            ClearBuffer(out_l_ptr1, frameCount);
                            break;
                        case SignalSource.NOISE:
                            Noise(out_l_ptr1, frameCount);
                            Noise(out_r_ptr1, frameCount);
                            break;
                        case SignalSource.TRIANGLE:
                            Triangle(out_l_ptr1, frameCount, sine_freq1);
                            CopyBuffer(out_l_ptr1, out_r_ptr1, frameCount);
                            break;
                        case SignalSource.SAWTOOTH:
                            Sawtooth(out_l_ptr1, frameCount, sine_freq1);
                            CopyBuffer(out_l_ptr1, out_r_ptr1, frameCount);
                            break;
                        case SignalSource.PULSE:
                            Pulse(out_l_ptr1, frameCount);
                            CopyBuffer(out_l_ptr1, out_r_ptr1, frameCount);
                            ScaleBuffer(out_l_ptr1, out_l_ptr1, frameCount, (float)source_scale);
                            ScaleBuffer(out_r_ptr1, out_r_ptr1, frameCount, (float)source_scale);
                            break;
                        case SignalSource.SILENCE:
                            ClearBuffer(out_l_ptr1, frameCount);
                            ClearBuffer(out_r_ptr1, frameCount);
                            break;
                    }
                }
                else
                {
                    switch (tx_output_signal)
                    {
                        case SignalSource.RADIO:

                         //   if (mon && mox)
                           // {
                             //   if (tx_dsp_mode == DSPMode.AM || tx_dsp_mode == DSPMode.SAM || tx_dsp_mode == DSPMode.FM)  // ke9ns add  use pre-processed audio for MON function in these modes only
                              //  {
                                //    DoScope(out_l_ptr1, frameCount); // do above the reroute

                                //        Trace.Write("PRI==");
                                 //   ScaleBuffer(in_tl_ptr1, out_l_ptr1, frameCount, 1.0f); // ke9ns add pre processed MON
                                //    ScaleBuffer(in_tl_ptr1, out_r_ptr1, frameCount, 1.0f); // ke9ns add pre processed MON

                                 //   ScaleBuffer(in_tr_ptr1, in_r, frameCount, (float)vac_rx_scale);
                                 //   ScaleBuffer(in_tr_ptr1, in_l, frameCount, (float)vac_rx_scale);

                              //  }
                         //   }
                          

                            break;
                        case SignalSource.SINE:

                           
                            SineWave(out_l_ptr1, frameCount, phase_accumulator1, sine_freq1);
                            phase_accumulator1 = CosineWave(out_l_ptr1, frameCount, phase_accumulator1, sine_freq1);

                            ScaleBuffer(out_l_ptr1, out_l_ptr1, frameCount, (float)source_scale);
                            ScaleBuffer(out_r_ptr1, out_r_ptr1, frameCount, (float)source_scale);
                            break;
                        case SignalSource.SINE_TWO_TONE:
                            double dump;
                            SineWave2Tone(out_l_ptr1, frameCount, phase_accumulator1, phase_accumulator2,
                                sine_freq1, sine_freq2, out dump, out dump);
                            CosineWave2Tone(out_r_ptr1, frameCount, phase_accumulator1, phase_accumulator2,
                                sine_freq1, sine_freq2, out phase_accumulator1, out phase_accumulator2);
                            ScaleBuffer(out_l_ptr1, out_l_ptr1, frameCount, (float)source_scale);
                            ScaleBuffer(out_r_ptr1, out_r_ptr1, frameCount, (float)source_scale);
                            break;
                        case SignalSource.NOISE:
                            Noise(out_l_ptr1, frameCount);
                            Noise(out_r_ptr1, frameCount);
                            break;
                        case SignalSource.TRIANGLE:
                            Triangle(out_l_ptr1, frameCount, sine_freq1);
                            CopyBuffer(out_l_ptr1, out_r_ptr1, frameCount);
                            break;
                        case SignalSource.SAWTOOTH:
                            Sawtooth(out_l_ptr1, frameCount, sine_freq1);
                            CopyBuffer(out_l_ptr1, out_r_ptr1, frameCount);
                            break;
                        case SignalSource.PULSE:
                            Pulse(out_l_ptr1, frameCount);
                            CopyBuffer(out_l_ptr1, out_r_ptr1, frameCount);
                            ScaleBuffer(out_l_ptr1, out_l_ptr1, frameCount, (float)source_scale);
                            ScaleBuffer(out_r_ptr1, out_r_ptr1, frameCount, (float)source_scale);
                            break;
                        case SignalSource.SILENCE:
                            ClearBuffer(out_l_ptr1, frameCount);
                            ClearBuffer(out_r_ptr1, frameCount);
                            break;
                    }
                }

                #endregion

                if (!localmox && mute_output)
                {
                    ClearBuffer(out_l_ptr1, frameCount);
                    ClearBuffer(out_r_ptr1, frameCount);
                }

               // if (tx_dsp_mode != DSPMode.AM && tx_dsp_mode != DSPMode.SAM && tx_dsp_mode != DSPMode.FM)  // ke9ns add  use pre-processed audio for MON function in these modes only
               // {

                    DoScope(out_l_ptr1, frameCount); // do up above if in AM/FM instead of here
               // }

                if (wave_record)
                {
                    if (!localmox)
                    {
                        if (!record_rx_preprocessed)
                        {
                            wave_file_writer.AddWriteBuffer(out_l, out_r);
                        }
                    }
                    else
                    {
                        if (!record_tx_preprocessed)
                        {
                            wave_file_writer.AddWriteBuffer(out_l, out_r);
                        }
                    }
                }

                //--------------------------------------------------------------
                // ke9ns add  comes here every 42msec @ 48kSR with 4096 Buffer size
                if (console.BeaconSigAvg == true) // true = beacon scan on or wwv time reading
                {

                    fixed (float* WWVP = &console.WWV_data[0]) // 4096 readings per frame
                    {
                         Win32.memcpy(WWVP, out_l_ptr1, frameCount * sizeof(float));  // dest,source  # of bytes to copy 2048 float sized bytes
                      
                    }
                  
                    console.WWVTone = console.Goertzel(console.WWV_data, 0, frameCount); // determine the magnitude of the 100hz TONE in the sample
                    console.WWVReady = true;
                    console.WWV_Count = 0;
         

                } //   if (console.BeaconSigAvg == true)


                //=========================================================================================
                // scale output for VAC1 -- use input as spare buffer ke9ns this buffer is the VAC back to your PC speaker
                if (vac_enabled && !vac_output_iq &&
                    rb_vacIN_l != null && rb_vacIN_r != null &&
                    rb_vacOUT_l != null && rb_vacOUT_r != null)
                {
                    if (!localmox)
                    {
                        ScaleBuffer(out_l, in_l, frameCount, (float)vac_rx_scale);
                        ScaleBuffer(out_r, in_r, frameCount, (float)vac_rx_scale);
                    }
                    else if (mon)
                    {
                        if ((monpre ==1) ||(tx_dsp_mode == DSPMode.AM || tx_dsp_mode == DSPMode.SAM || tx_dsp_mode == DSPMode.FM))  // ke9ns add  use pre-processed audio for MON function in these modes only
                        {

                            ScaleBuffer(in_l_ptr1, in_l, frameCount, (float)vac_rx_scale); // ke9ns add pre processed MON over VAC
                            ScaleBuffer(in_r_ptr1, in_r, frameCount, (float)vac_rx_scale);
                          // Trace.Write("vac==");

                        }
                        else
                        {
                            ScaleBuffer(out_l, in_l, frameCount, (float)vac_rx_scale); // ke9ns post processed MON
                            ScaleBuffer(out_r, in_r, frameCount, (float)vac_rx_scale);
                        }


                    }
                    else // zero samples going back to VAC since TX monitor is off
                    {
                        ScaleBuffer(out_l, in_l, frameCount, 0.0f);
                        ScaleBuffer(out_r, in_r, frameCount, 0.0f);
                    }




                    if (sample_rate2 == sample_rate1) // no resample necessary
                    {
                        if ((rb_vacOUT_l.WriteSpace() >= frameCount) && (rb_vacOUT_r.WriteSpace() >= frameCount))
                        {
                            Win32.EnterCriticalSection(cs_vac);
                            rb_vacOUT_l.WritePtr(in_l, frameCount);
                            rb_vacOUT_r.WritePtr(in_r, frameCount);
                            Win32.LeaveCriticalSection(cs_vac);
                        }
                        else
                        {
                            VACDebug("rb_vacOUT_l overflow");
                            VACDebug("rb_vacOUT_r overflow");
                        }
                    }
                    else
                    {
                        if (vac_stereo)
                        {
                            fixed (float* res_outl_ptr = &(res_outl[0]))
                            fixed (float* res_outr_ptr = &(res_outr[0]))
                            {
                                int outsamps;
                                DttSP.DoResamplerF(in_l, res_outl_ptr, frameCount, &outsamps, resampPtrOut_l);
                                DttSP.DoResamplerF(in_r, res_outr_ptr, frameCount, &outsamps, resampPtrOut_r);
                                if ((rb_vacOUT_l.WriteSpace() >= outsamps) && (rb_vacOUT_r.WriteSpace() >= outsamps))
                                {
                                    Win32.EnterCriticalSection(cs_vac);
                                    rb_vacOUT_l.WritePtr(res_outl_ptr, outsamps);
                                    rb_vacOUT_r.WritePtr(res_outr_ptr, outsamps);
                                    Win32.LeaveCriticalSection(cs_vac);
                                }
                                else
                                {
                                    VACDebug("rb_vacOUT_l overflow");
                                    VACDebug("rb_vacOUT_r overflow");
                                }
                            }
                        }
                        else
                        {
                            fixed (float* res_outl_ptr = &(res_outl[0]))
                            {
                                int outsamps;
                                DttSP.DoResamplerF(in_l, res_outl_ptr, frameCount, &outsamps, resampPtrOut_l);
                                if ((rb_vacOUT_l.WriteSpace() >= outsamps) && (rb_vacOUT_r.WriteSpace() >= outsamps))
                                {
                                    Win32.EnterCriticalSection(cs_vac);
                                    rb_vacOUT_l.WritePtr(res_outl_ptr, outsamps);
                                    rb_vacOUT_r.WritePtr(res_outl_ptr, outsamps);
                                    Win32.LeaveCriticalSection(cs_vac);
                                }
                                else
                                {
                                    VACDebug("rb_vacOUT_l overflow");
                                    VACDebug("rb_vacOUT_r overflow");
                                }
                            }
                        }
                    }
                } // VAC1 on

                // scale output for VAC2 -- use input as spare buffer
                if (vac2_enabled && !vac2_output_iq &&
                    rb_vac2IN_l != null && rb_vac2IN_r != null &&
                    rb_vac2OUT_l != null && rb_vac2OUT_r != null)
                {
                    ScaleBuffer(out_l, in_l, frameCount, (float)vac2_rx_scale);
                    ScaleBuffer(out_r, in_r, frameCount, (float)vac2_rx_scale);

                    if (sample_rate3 == sample_rate1) // no resample necessary
                    {
                        if ((rb_vac2OUT_l.WriteSpace() >= frameCount) && (rb_vac2OUT_r.WriteSpace() >= frameCount))
                        {
                            Win32.EnterCriticalSection(cs_vac2);
                            rb_vac2OUT_l.WritePtr(in_l, frameCount);
                            rb_vac2OUT_r.WritePtr(in_r, frameCount);
                            Win32.LeaveCriticalSection(cs_vac2);
                        }
                        else
                        {
                            VACDebug("rb_vac2OUT_l overflow");
                            VACDebug("rb_vac2OUT_r overflow");
                        }
                    }
                    else
                    {
                        if (vac2_stereo)
                        {
                            fixed (float* res_outl_ptr = &(res_vac2_outl[0]))
                            fixed (float* res_outr_ptr = &(res_vac2_outr[0]))
                            {
                                int outsamps;
                                DttSP.DoResamplerF(in_l, res_outl_ptr, frameCount, &outsamps, resampVAC2PtrOut_l);
                                DttSP.DoResamplerF(in_r, res_outr_ptr, frameCount, &outsamps, resampVAC2PtrOut_r);
                                if ((rb_vac2OUT_l.WriteSpace() >= outsamps) && (rb_vac2OUT_r.WriteSpace() >= outsamps))
                                {
                                    Win32.EnterCriticalSection(cs_vac2);
                                    rb_vac2OUT_l.WritePtr(res_outl_ptr, outsamps);
                                    rb_vac2OUT_r.WritePtr(res_outr_ptr, outsamps);
                                    Win32.LeaveCriticalSection(cs_vac2);
                                }
                                else
                                {
                                    VACDebug("rb_vac2OUT_l overflow");
                                    VACDebug("rb_vac2OUT_r overflow");
                                }
                            }
                        }
                        else
                        {
                            fixed (float* res_outl_ptr = &(res_vac2_outl[0]))
                            {
                                int outsamps;
                                DttSP.DoResamplerF(in_l, res_outl_ptr, frameCount, &outsamps, resampVAC2PtrOut_l);
                                if ((rb_vac2OUT_l.WriteSpace() >= outsamps) && (rb_vac2OUT_r.WriteSpace() >= outsamps))
                                {
                                    Win32.EnterCriticalSection(cs_vac2);
                                    rb_vac2OUT_l.WritePtr(res_outl_ptr, outsamps);
                                    rb_vac2OUT_r.WritePtr(res_outl_ptr, outsamps);
                                    Win32.LeaveCriticalSection(cs_vac2);
                                }
                                else
                                {
                                    VACDebug("rb_vac2OUT_l overflow");
                                    VACDebug("rb_vac2OUT_r overflow");
                                }
                            }
                        }
                    }
                }

                double vol = monitor_volume;

               

                if (mox)
                {
                    if (tx_output_signal != SignalSource.RADIO || tx_1500_image_cal)
                        vol = 1.0;
                    else
                        vol = TXScale;
                }
                else // receive
                {
                    if (rx1_output_signal != SignalSource.RADIO)
                        vol = 1.0f;
                }               

                if (vol != 1.0)
                {
                    ScaleBuffer(out_l, out_l, frameCount, (float)vol); // ke9ns out to the transmitter
                    ScaleBuffer(out_r, out_r, frameCount, (float)vol);
                }

#if(MINMAX)
			Debug.Write(MaxSample(out_l, out_r, frameCount).ToString("f6")+",");

			float current_max = MaxSample(out_l, out_r, frameCount);
			if(current_max > max) max = current_max;
			Debug.WriteLine(" max: "+max.ToString("f6"));
#endif

                /*if ((wave_record && !mox && !record_rx_preprocessed) ||
                    (wave_record && mox && !record_tx_preprocessed))
                    wave_file_writer.AddWriteBuffer(out_l_ptr1, out_r_ptr1);*/
#if(TIMER)
			t1.Stop();
			Debug.WriteLine(t1.Duration);
#endif
                min_out_l = Math.Min(min_out_l, MinSample(out_l, frameCount));
                min_out_r = Math.Min(min_out_r, MinSample(out_r, frameCount));
                max_out_l = Math.Max(max_out_l, MaxSample(out_l, frameCount));
                max_out_r = Math.Max(max_out_r, MaxSample(out_r, frameCount));
            }           

            return callback_return;


        } // callback1500





#if(TIMER)
		private static HiPerfTimer t1 = new HiPerfTimer();
#endif
		//private static int count = 0;
		unsafe public static int Callback4Port(void* input, void* output, int frameCount,
			PA19.PaStreamCallbackTimeInfo* timeInfo, int statusFlags, void *userData)
		{
#if(TIMER)
			t1.Start();
#endif
			float* in_l = null, in_r = null, out_l = null, out_r = null;
			float* out_l1 = null, out_r1 = null, out_l2 = null, out_r2 = null;
			localmox = mox;

			void* ex_input  = (void *)input;
			void* ex_output = (void *)output;
			int* array_ptr_input = (int *)input;
			float* in_l_ptr1 = (float *)array_ptr_input[0];
			float* in_r_ptr1 = (float *)array_ptr_input[1];
			float* in_l_ptr2 = (float *)array_ptr_input[2];
			float* in_r_ptr2 = (float *)array_ptr_input[3];
			int* array_ptr_output = (int *)output;
			float* out_l_ptr1 = (float *)array_ptr_output[0];
			float* out_r_ptr1 = (float *)array_ptr_output[1];
			float* out_l_ptr2 = (float *)array_ptr_output[2];
			float* out_r_ptr2 = (float *)array_ptr_output[3];

			// arrange input buffers in the following order:
			// RX1 Left, RX1 Right,   TX Left, TX Right,   RX2 Left, RX2 Right
			int* array_ptr = (int *)input;
			switch(in_rx1_l)
			{
				case 0: array_ptr[0] = (int)in_l_ptr1; break;
				case 1: array_ptr[0] = (int)in_r_ptr1; break;
				case 2: array_ptr[0] = (int)in_l_ptr2; break;
				case 3: array_ptr[0] = (int)in_r_ptr2; break;
			}
			
			switch(in_rx1_r)
			{
				case 0: array_ptr[1] = (int)in_l_ptr1; break;
				case 1: array_ptr[1] = (int)in_r_ptr1; break;
				case 2: array_ptr[1] = (int)in_l_ptr2; break;
				case 3: array_ptr[1] = (int)in_r_ptr2; break;
			}
			
			switch(in_tx_l)
			{
				case 0: array_ptr[2] = (int)in_l_ptr1; break;
				case 1: array_ptr[2] = (int)in_r_ptr1; break;
				case 2: array_ptr[2] = (int)in_l_ptr2; break;
				case 3: array_ptr[2] = (int)in_r_ptr2; break;
			}
				
			switch(in_tx_r)
			{
				case 0: array_ptr[3] = (int)in_l_ptr1; break;
				case 1: array_ptr[3] = (int)in_r_ptr1; break;
				case 2: array_ptr[3] = (int)in_l_ptr2; break;
				case 3: array_ptr[3] = (int)in_r_ptr2; break;
			}

			/*switch(in_rx2_l)
			{
				case 0: break;
				case 1: array_ptr[4] = (int)in_r_ptr1; break;
				case 2: array_ptr[4] = (int)in_l_ptr2; break;
				case 3: array_ptr[4] = (int)in_r_ptr2; break;
			}
			switch(in_rx2_r)
			{
				case 0: break;
				case 1: array_ptr[5] = (int)in_r_ptr1; break;
				case 2: array_ptr[5] = (int)in_l_ptr2; break;
				case 3: array_ptr[5] = (int)in_r_ptr2; break;
			}*/
	
			if(!localmox)
			{
				in_l = (float *)array_ptr_input[0];
				in_r = (float *)array_ptr_input[1];
			}
			else
			{
				in_l = (float *)array_ptr_input[2];
				in_r = (float *)array_ptr_input[3];
			}

            if (wave_playback)
            {
                wave_file_reader.GetPlayBuffer(in_l, in_r);
            //    Debug.WriteLine("===========testing"); // ke9ns testdsp

            }

            if (wave_record)
			{
				if(!localmox)
				{
					if(record_rx_preprocessed)
					{
						wave_file_writer.AddWriteBuffer(in_l, in_r);
                       
                    }
                }
				else
				{
					if(record_tx_preprocessed)
					{
						wave_file_writer.AddWriteBuffer(in_l, in_r);
					}
				}
			}				

			if(phase)
			{
				//phase_mutex.WaitOne();
				Marshal.Copy(new IntPtr(in_l), phase_buf_l, 0, frameCount);
				Marshal.Copy(new IntPtr(in_r), phase_buf_r, 0, frameCount);
				//phase_mutex.ReleaseMutex();
			}

			// handle VAC Input
			if(vac_enabled && rb_vacIN_l != null && rb_vacIN_r != null && rb_vacOUT_l != null && rb_vacOUT_r != null)
			{
				if(vac_bypass || !localmox) // drain VAC Input ring buffer
				{
					if ((rb_vacIN_l.ReadSpace() >= frameCount) && (rb_vacIN_r.ReadSpace() >= frameCount))
					{
						Win32.EnterCriticalSection(cs_vac);
						rb_vacIN_l.ReadPtr(out_l_ptr2, frameCount);
						rb_vacIN_r.ReadPtr(out_r_ptr2, frameCount);
						Win32.LeaveCriticalSection(cs_vac);
					}
				}
				else
				{
					if(rb_vacIN_l.ReadSpace() >= frameCount) 
					{
						Win32.EnterCriticalSection(cs_vac);
						rb_vacIN_l.ReadPtr(in_l, frameCount);
						rb_vacIN_r.ReadPtr(in_r, frameCount);
						Win32.LeaveCriticalSection(cs_vac);
						if(vac_combine_input)
							AddBuffer(in_l, in_r, frameCount);
					}
					else
					{
						ClearBuffer(in_l, frameCount);
						ClearBuffer(in_r, frameCount);
						VACDebug("rb_vacIN underflow 4inTX");
					}
					ScaleBuffer(in_l, in_l, frameCount, (float)vac_preamp);
					ScaleBuffer(in_r, in_r, frameCount, (float)vac_preamp);
				}
			}

            // handle VAC2 Input
            /*if (vac2_enabled &&
                rb_vac2IN_l != null && rb_vac2IN_r != null &&
                rb_vac2OUT_l != null && rb_vac2OUT_r != null)
            {
                if (vac_bypass || !localmox || !vfob_tx) // drain VAC Input ring buffer
                {
                    if ((rb_vac2IN_l.ReadSpace() >= frameCount) && (rb_vac2IN_r.ReadSpace() >= frameCount))
                    {
                        Win32.EnterCriticalSection(cs_vac2);
                        rb_vac2IN_l.ReadPtr(out_l_ptr2, frameCount);
                        rb_vac2IN_r.ReadPtr(out_r_ptr2, frameCount);
                        Win32.LeaveCriticalSection(cs_vac2);
                    }
                }
                else
                {
                    if (vfob_tx && rb_vac2IN_l.ReadSpace() >= frameCount)
                    {
                        Win32.EnterCriticalSection(cs_vac2);
                        rb_vac2IN_l.ReadPtr(in_l, frameCount);
                        rb_vac2IN_r.ReadPtr(in_r, frameCount);
                        Win32.LeaveCriticalSection(cs_vac2);
                        if (vac2_combine_input)
                            AddBuffer(in_l, in_r, frameCount);

                        ScaleBuffer(in_l, in_l, frameCount, (float)vac_preamp);
                        ScaleBuffer(in_r, in_r, frameCount, (float)vac_preamp);
                    }
                    else
                    {
                        ClearBuffer(in_l, frameCount);
                        ClearBuffer(in_r, frameCount);
                        VACDebug("rb_vac2IN underflow 4inTX");
                    }                    
                }
            }*/





            #region VOX
            if (vox_enabled)
            {
                float* vox_l = null, vox_r = null;
                //switch(soundcard)
                //		{
                //	case SoundCard.FIREBOX: // ke9ns remove
                //case SoundCard.EDIROL_FA_66:
                //		vox_l = in_l_ptr1;
                //	vox_r = in_r_ptr1;
                //	break;
                //	case SoundCard.DELTA_44:
                //		default:
                vox_l = in_l_ptr2;
                vox_r = in_r_ptr2;
                //		break;
                //	}

                // ke9ns add this 
                //This is a standard 16 bit WAV WAV file. The largest sample value in the files
                //is 21188 and the maximum possible value is 32767.
                //    20 * log10(21188 / 32767) => -3.79dB

                if (tx_dsp_mode == DSPMode.LSB ||
                    tx_dsp_mode == DSPMode.USB ||
                    tx_dsp_mode == DSPMode.DSB ||
                    tx_dsp_mode == DSPMode.AM ||
                    tx_dsp_mode == DSPMode.SAM ||
                    tx_dsp_mode == DSPMode.FM)
                {
                    peak = MaxSample(vox_l, vox_r, frameCount);


                    // compare power to threshold
                    if (peak > vox_threshold)
                        vox_active = true;
                    else
                        vox_active = false;
                }
            }
           
           

            #endregion

            // scale input with mic preamp
            if ((!vac_enabled &&
				(tx_dsp_mode == DSPMode.LSB ||
				tx_dsp_mode == DSPMode.USB ||
				tx_dsp_mode == DSPMode.DSB ||
				tx_dsp_mode == DSPMode.AM  ||
				tx_dsp_mode == DSPMode.SAM ||
				tx_dsp_mode == DSPMode.FM ||
				tx_dsp_mode == DSPMode.DIGL ||
				tx_dsp_mode == DSPMode.DIGU)) ||
				(vac_enabled && vac_bypass &&
				(tx_dsp_mode == DSPMode.DIGL ||
				tx_dsp_mode == DSPMode.DIGU ||
				tx_dsp_mode == DSPMode.LSB ||
				tx_dsp_mode == DSPMode.USB ||
				tx_dsp_mode == DSPMode.DSB ||
				tx_dsp_mode == DSPMode.AM ||
				tx_dsp_mode == DSPMode.SAM ||
				tx_dsp_mode == DSPMode.FM)))
			{
				if(wave_playback)
				{
					ScaleBuffer(in_l, in_l, frameCount, (float)wave_preamp);
					ScaleBuffer(in_r, in_r, frameCount, (float)wave_preamp);
				}
				else
				{
					if(localmox)
					{
						if(!vac_enabled && (tx_dsp_mode == DSPMode.DIGL || tx_dsp_mode == DSPMode.DIGU || (tx_dsp_mode == DSPMode.FM && console.FMData == true && console.setupForm.chkFMDataMic.Checked == false))    )
                        {
							ScaleBuffer(in_l, in_l, frameCount, (float)vac_preamp);
							ScaleBuffer(in_r, in_r, frameCount, (float)vac_preamp);

                          

						}
						else
						{

                            if (console.setupForm.chkPhaseRotate.Checked == true) // ke9ns add phase rotation
                            {
                              
                                PhaseRotate(in_l, in_l, frameCount, (float)mic_preamp);
                                PhaseRotate(in_r, in_r, frameCount, (float)mic_preamp);

                            }
                            else
                            {
                                ScaleBuffer(in_l, in_l, frameCount, (float)mic_preamp);
                                ScaleBuffer(in_r, in_r, frameCount, (float)mic_preamp);

                            }




                            if ((console.TXMeter2 == true) && (console.CurrentMeterTX1Mode == MeterTXMode.MIC)) peak1 = MaxSample(in_l, in_r, frameCount); // ke9ns add, to allow for MIC level check in RX mode
                        }
					}
				}
			}

			#region Input Signal Source

			if(!mox)
			{
				switch(rx1_input_signal)
				{
					case SignalSource.RADIO:
						break;
					case SignalSource.SINE:
						SineWave(in_l, frameCount, phase_accumulator1, sine_freq1);
						phase_accumulator1 = CosineWave(in_r, frameCount, phase_accumulator1, sine_freq1);
						ScaleBuffer(in_l, in_l, frameCount, (float)source_scale);
						ScaleBuffer(in_r, in_r, frameCount, (float)source_scale);
						break;
					case SignalSource.SINE_TWO_TONE:
						double dump;
						SineWave2Tone(in_l, frameCount, phase_accumulator1, phase_accumulator2,
							sine_freq1, sine_freq2, out dump, out dump);
						CosineWave2Tone(in_r, frameCount, phase_accumulator1, phase_accumulator2,
							sine_freq1, sine_freq2, out phase_accumulator1, out phase_accumulator2);
						ScaleBuffer(in_l, in_l, frameCount, (float)source_scale);
						ScaleBuffer(in_r, in_r, frameCount, (float)source_scale);
						break;
					case SignalSource.SINE_LEFT_ONLY:
						phase_accumulator1 = SineWave(in_l, frameCount, phase_accumulator1, sine_freq1);
						ScaleBuffer(in_l, in_l, frameCount, (float)source_scale);
						ClearBuffer(in_r, frameCount);
						break;
					case SignalSource.SINE_RIGHT_ONLY:
						phase_accumulator1 = SineWave(in_r, frameCount, phase_accumulator1, sine_freq1);
						ScaleBuffer(in_r, in_r, frameCount, (float)source_scale);
						ClearBuffer(in_l, frameCount);
						break;
					case SignalSource.NOISE:
						Noise(in_l, frameCount);
						Noise(in_r, frameCount);
						break;
					case SignalSource.TRIANGLE:
						Triangle(in_l, frameCount, sine_freq1);
						CopyBuffer(in_l, in_r, frameCount);
						break;
					case SignalSource.SAWTOOTH:
						Sawtooth(in_l, frameCount, sine_freq1);
						CopyBuffer(in_l, in_r, frameCount);
						break;
                    case SignalSource.PULSE:
                        Pulse(in_l, frameCount);
                        CopyBuffer(in_l, in_r, frameCount);
                        ScaleBuffer(in_l, in_l, frameCount, (float)source_scale);
                        ScaleBuffer(in_r, in_r, frameCount, (float)source_scale);
                        break;
					case SignalSource.SILENCE:
						ClearBuffer(in_l, frameCount);
						ClearBuffer(in_r, frameCount);
						break;
				}
			}
			else
			{
				switch(tx_input_signal)
				{
					case SignalSource.RADIO:
						break;
					case SignalSource.SINE:
                       
						SineWave(in_l, frameCount, phase_accumulator1, sine_freq1);
						phase_accumulator1 = CosineWave(in_r, frameCount, phase_accumulator1, sine_freq1);

                        ScaleBuffer(in_l, in_l, frameCount, (float)source_scale);
						ScaleBuffer(in_r, in_r, frameCount, (float)source_scale);
						break;
					case SignalSource.SINE_TWO_TONE:
						double dump;
						SineWave2Tone(in_l, frameCount, phase_accumulator1, phase_accumulator2,
							sine_freq1, sine_freq2, out dump, out dump);
						CosineWave2Tone(in_r, frameCount, phase_accumulator1, phase_accumulator2,
							sine_freq1, sine_freq2, out phase_accumulator1, out phase_accumulator2);
						ScaleBuffer(in_l, in_l, frameCount, (float)source_scale);
						ScaleBuffer(in_r, in_r, frameCount, (float)source_scale);
						break;
					case SignalSource.NOISE:
						Noise(in_l, frameCount);
						Noise(in_r, frameCount);
						break;
					case SignalSource.TRIANGLE:
						Triangle(in_l, frameCount, sine_freq1);
						CopyBuffer(in_l, in_r, frameCount);
						break;
					case SignalSource.SAWTOOTH:
						Sawtooth(in_l, frameCount, sine_freq1);
						CopyBuffer(in_l, in_r, frameCount);
						break;
                    case SignalSource.PULSE:
                        Pulse(in_l, frameCount);
                        CopyBuffer(in_l, in_r, frameCount);
                        ScaleBuffer(in_l, in_l, frameCount, (float)source_scale);
                        ScaleBuffer(in_r, in_r, frameCount, (float)source_scale);
                        break;
					case SignalSource.SILENCE:
						ClearBuffer(in_l, frameCount);
						ClearBuffer(in_r, frameCount);
						break;
				}
			}

			#endregion

#if(MINMAX)
			Debug.Write(MaxSample(in_l, in_r, frameCount).ToString("f6")+",");
#endif

            if (localmox && (tx_dsp_mode == DSPMode.CWL || tx_dsp_mode == DSPMode.CWU))
            {
                double time = CWSensorItem.GetCurrentTime();
                CWSynth.Advance(out_l_ptr2, out_r_ptr2, frameCount, time);
            }
            else if (tx_dsp_mode == DSPMode.CWL || tx_dsp_mode == DSPMode.CWU)
            {
                double time = CWSensorItem.GetCurrentTime();
                CWSynth.Advance(out_l_ptr2, out_r_ptr2, frameCount, time);

                DttSP.ExchangeSamples2(ex_input, ex_output, frameCount);
            }
            else
            {
                DttSP.ExchangeSamples2(ex_input, ex_output, frameCount);
            }

#if(MINMAX)
			Debug.Write(MaxSample(out_l_ptr1, out_r_ptr1, frameCount).ToString("f6")+",");
#endif

			#region Output Signal Source

			if(!mox)
			{
				switch(rx1_output_signal)
				{
					case SignalSource.RADIO:
						break;
					case SignalSource.SINE:
						SineWave(out_l_ptr1, frameCount, phase_accumulator1, sine_freq1);
						phase_accumulator1 = CosineWave(out_r_ptr1, frameCount, phase_accumulator1, sine_freq1);
						ScaleBuffer(out_l_ptr1, out_l_ptr1, frameCount, (float)source_scale);
						ScaleBuffer(out_r_ptr1, out_r_ptr1, frameCount, (float)source_scale);
						break;
					case SignalSource.SINE_TWO_TONE:
						double dump;
						SineWave2Tone(out_l_ptr1, frameCount, phase_accumulator1, phase_accumulator2,
							sine_freq1, sine_freq2, out dump, out dump);
						CosineWave2Tone(out_r_ptr1, frameCount, phase_accumulator1, phase_accumulator2,
							sine_freq1, sine_freq2, out phase_accumulator1, out phase_accumulator2);
						ScaleBuffer(out_l_ptr1, out_l_ptr1, frameCount, (float)source_scale);
						ScaleBuffer(out_r_ptr1, out_r_ptr1, frameCount, (float)source_scale);
						break;
					case SignalSource.SINE_LEFT_ONLY:
						phase_accumulator1 = SineWave(out_l_ptr1, frameCount, phase_accumulator1, sine_freq1);
						ScaleBuffer(out_l_ptr1, out_l_ptr1, frameCount, (float)source_scale);
						ClearBuffer(out_r_ptr1, frameCount);
						break;
					case SignalSource.SINE_RIGHT_ONLY:
						phase_accumulator1 = SineWave(out_r_ptr1, frameCount, phase_accumulator1, sine_freq1);
						ScaleBuffer(out_r_ptr1, out_r_ptr1, frameCount, (float)source_scale);
						ClearBuffer(out_l_ptr1, frameCount);
						break;
					case SignalSource.NOISE:
						Noise(out_l_ptr1, frameCount);
						Noise(out_r_ptr1, frameCount);
						break;
					case SignalSource.TRIANGLE:
						Triangle(out_l_ptr1, frameCount, sine_freq1);
						CopyBuffer(out_l_ptr1, out_r_ptr1, frameCount);
						break;
					case SignalSource.SAWTOOTH:
						Sawtooth(out_l_ptr1, frameCount, sine_freq1);
						CopyBuffer(out_l_ptr1, out_r_ptr1, frameCount);
						break;
                    case SignalSource.PULSE:
                        Pulse(out_l_ptr1, frameCount);
                        CopyBuffer(out_l_ptr1, out_r_ptr1, frameCount);
                        ScaleBuffer(out_l_ptr1, out_l_ptr1, frameCount, (float)source_scale);
                        ScaleBuffer(out_r_ptr1, out_r_ptr1, frameCount, (float)source_scale);
                        break;
					case SignalSource.SILENCE:
						ClearBuffer(out_l_ptr1, frameCount);
						ClearBuffer(out_r_ptr1, frameCount);
						break;
				}
			}
			else
			{
				switch(tx_output_signal)
				{
					case SignalSource.RADIO:
						break;
					case SignalSource.SINE:

                        SineWave(out_l_ptr2, frameCount, phase_accumulator1, sine_freq1);
						phase_accumulator1 = CosineWave(out_r_ptr2, frameCount, phase_accumulator1, sine_freq1);

                        ScaleBuffer(out_l_ptr2, out_l_ptr2, frameCount, (float)source_scale);
						ScaleBuffer(out_r_ptr2, out_r_ptr2, frameCount, (float)source_scale);
						break;
					case SignalSource.SINE_TWO_TONE:
						double dump;
						SineWave2Tone(out_l_ptr2, frameCount, phase_accumulator1, phase_accumulator2,
							sine_freq1, sine_freq2, out dump, out dump);
						CosineWave2Tone(out_r_ptr2, frameCount, phase_accumulator1, phase_accumulator2,
							sine_freq1, sine_freq2, out phase_accumulator1, out phase_accumulator2);
						ScaleBuffer(out_l_ptr2, out_l_ptr2, frameCount, (float)source_scale);
						ScaleBuffer(out_r_ptr2, out_r_ptr2, frameCount, (float)source_scale);
						break;
					case SignalSource.NOISE:
						Noise(out_l_ptr2, frameCount);
						Noise(out_r_ptr2, frameCount);
						break;
					case SignalSource.TRIANGLE:
						Triangle(out_l_ptr2, frameCount, sine_freq1);
						CopyBuffer(out_l_ptr2, out_r_ptr2, frameCount);
						break;
					case SignalSource.SAWTOOTH:
						Sawtooth(out_l_ptr2, frameCount, sine_freq1);
						CopyBuffer(out_l_ptr2, out_r_ptr2, frameCount);
						break;
                    case SignalSource.PULSE:
                        Pulse(out_l_ptr2, frameCount);
                        CopyBuffer(out_l_ptr2, out_r_ptr2, frameCount);
                        ScaleBuffer(out_l_ptr2, out_l_ptr2, frameCount, (float)source_scale);
                        ScaleBuffer(out_r_ptr2, out_r_ptr2, frameCount, (float)source_scale);
                        break;
					case SignalSource.SILENCE:
						ClearBuffer(out_l_ptr2, frameCount);
						ClearBuffer(out_r_ptr2, frameCount);
						break;
				}
			}

			#endregion
					
			if(!localmox) DoScope(out_l_ptr1, frameCount);
			else DoScope(out_l_ptr2, frameCount);

			out_l1 = out_l_ptr1;
			out_r1 = out_r_ptr1;
			out_l2 = out_l_ptr2;
			out_r2 = out_r_ptr2;

			if(wave_record)
			{
				if(!localmox)
				{
					if(!record_rx_preprocessed)
					{
						wave_file_writer.AddWriteBuffer(out_l_ptr1, out_r_ptr1);
					}
				}
				else
				{
					if(!record_tx_preprocessed)
					{
						wave_file_writer.AddWriteBuffer(out_l_ptr2, out_r_ptr2);
					}
				}
			}

			// scale output for VAC
			if(vac_enabled &&
				rb_vacIN_l != null && rb_vacIN_r != null && 
				rb_vacOUT_l != null && rb_vacOUT_r != null)
			{
				if(!localmox)
				{
					ScaleBuffer(out_l1, out_l2, frameCount, (float)vac_rx_scale);
					ScaleBuffer(out_r1, out_r2, frameCount, (float)vac_rx_scale);
				}
				else if(mon)
				{
                    if( (monpre == 1) || (tx_dsp_mode == DSPMode.AM || tx_dsp_mode == DSPMode.SAM || tx_dsp_mode == DSPMode.FM) ) // ke9ns add  use pre-processed audio for MON function in these modes only
                    {
                        ScaleBuffer(in_l_ptr1, out_l1, frameCount, (float)vac_rx_scale);  // ke9ns add preprocess
                        ScaleBuffer(in_r_ptr1, out_r1, frameCount, (float)vac_rx_scale);
                    }
                    else
                    {
                        ScaleBuffer(out_l2, out_l1, frameCount, (float)vac_rx_scale); // ke9ns postprocess
                        ScaleBuffer(out_r2, out_r1, frameCount, (float)vac_rx_scale);
                    }
				}
				else // zero samples going back to VAC since TX monitor is off
				{
					ScaleBuffer(out_l2, out_l1, frameCount, 0.0f);
					ScaleBuffer(out_r2, out_r1, frameCount, 0.0f);
				}

				float* vac_l, vac_r;
				if(!localmox)
				{
					vac_l = out_l2;
					vac_r = out_r2;
				}
				else
				{
					vac_l = out_l1;
					vac_r = out_r1;
				}

				if (sample_rate2 == sample_rate1) 
				{
					if ((rb_vacOUT_l.WriteSpace()>=frameCount)&&(rb_vacOUT_r.WriteSpace()>=frameCount))
					{
						Win32.EnterCriticalSection(cs_vac);
						rb_vacOUT_l.WritePtr(vac_l, frameCount);
						rb_vacOUT_r.WritePtr(vac_r, frameCount);
						Win32.LeaveCriticalSection(cs_vac);
					}
					else 
					{
						VACDebug("rb_vacOUT_l overflow ");
						vac_rb_reset = true;
					}
				} 
				else 
				{
					if (vac_stereo)
					{
						fixed(float *res_outl_ptr = &(res_outl[0]))
							fixed(float *res_outr_ptr = &(res_outr[0])) 
							{
								int outsamps;
								DttSP.DoResamplerF(vac_l, res_outl_ptr, frameCount, &outsamps, resampPtrOut_l);
								DttSP.DoResamplerF(vac_r, res_outr_ptr, frameCount, &outsamps, resampPtrOut_r);
								//Debug.WriteLine("Outsamps: "+outsamps.ToString());
								if ((rb_vacOUT_l.WriteSpace()>=outsamps)&&(rb_vacOUT_r.WriteSpace()>=outsamps))
								{
									Win32.EnterCriticalSection(cs_vac);
									rb_vacOUT_l.WritePtr(res_outl_ptr, outsamps);
									rb_vacOUT_r.WritePtr(res_outr_ptr, outsamps);
									Win32.LeaveCriticalSection(cs_vac);
								}
								else
								{
									vac_rb_reset = true;
									VACDebug("rb_vacOUT_l overflow");
								}
							}
					}
					else 
					{
						fixed(float *res_outl_ptr = &(res_outl[0]))
						{
							int outsamps;
							DttSP.DoResamplerF(vac_l, res_outl_ptr, frameCount, &outsamps, resampPtrOut_l);
							//Debug.WriteLine("Framecount: "+frameCount.ToString() + " Outsamps: "+outsamps.ToString());
							if ((rb_vacOUT_l.WriteSpace() >= outsamps)&&(rb_vacOUT_r.WriteSpace() >= outsamps))
							{
								Win32.EnterCriticalSection(cs_vac);
								rb_vacOUT_l.WritePtr(res_outl_ptr, outsamps);
								rb_vacOUT_r.WritePtr(res_outl_ptr, outsamps);
								Win32.LeaveCriticalSection(cs_vac);
							}
							else
							{
								vac_rb_reset = true;
								VACDebug("rb_vacOUT_l overflow");
							}
						}
					}
				}
			}

            // scale output for VAC2
            if (vac2_enabled &&
                rb_vac2IN_l != null && rb_vac2IN_r != null &&
                rb_vac2OUT_l != null && rb_vac2OUT_r != null)
            {
                if (!localmox)
                {
                    ScaleBuffer(out_l1, out_l2, frameCount, (float)vac2_rx_scale);
                    ScaleBuffer(out_r1, out_r2, frameCount, (float)vac2_rx_scale);
                }
                else if (mon)
                {
                    if ((monpre == 1) || (tx_dsp_mode == DSPMode.AM || tx_dsp_mode == DSPMode.SAM || tx_dsp_mode == DSPMode.FM) ) // ke9ns add  use pre-processed audio for MON function in these modes only
                    {

                        ScaleBuffer(in_l_ptr1, out_l1, frameCount, (float)vac2_rx_scale); // ke9ns add preprocess
                        ScaleBuffer(in_r_ptr1, out_r1, frameCount, (float)vac2_rx_scale);
                    }
                    else
                    {
                        ScaleBuffer(out_l2, out_l1, frameCount, (float)vac2_rx_scale); // ke9ns postprocess
                        ScaleBuffer(out_r2, out_r1, frameCount, (float)vac2_rx_scale);
                    }
                }
                else // zero samples going back to VAC since TX monitor is off
                {
                    ScaleBuffer(out_l2, out_l1, frameCount, 0.0f);
                    ScaleBuffer(out_r2, out_r1, frameCount, 0.0f);
                }

                float* vac_l, vac_r;
                if (!localmox)
                {
                    vac_l = out_l2;
                    vac_r = out_r2;
                }
                else
                {
                    vac_l = out_l1;
                    vac_r = out_r1;
                }

                if (sample_rate3 == sample_rate1)
                {
                    if ((rb_vac2OUT_l.WriteSpace() >= frameCount) && (rb_vac2OUT_r.WriteSpace() >= frameCount))
                    {
                        Win32.EnterCriticalSection(cs_vac2);
                        rb_vac2OUT_l.WritePtr(vac_l, frameCount);
                        rb_vac2OUT_r.WritePtr(vac_r, frameCount);
                        Win32.LeaveCriticalSection(cs_vac2);
                    }
                    else
                    {
                        VACDebug("rb_vac2OUT_l overflow ");
                        vac2_rb_reset = true;
                    }
                }
                else
                {
                    if (vac2_stereo)
                    {
                        fixed (float* res_outl_ptr = &(res_vac2_outl[0]))
                        fixed (float* res_outr_ptr = &(res_vac2_outr[0]))
                        {
                            int outsamps;
                            DttSP.DoResamplerF(vac_l, res_outl_ptr, frameCount, &outsamps, resampVAC2PtrOut_l);
                            DttSP.DoResamplerF(vac_r, res_outr_ptr, frameCount, &outsamps, resampVAC2PtrOut_r);
                            //Debug.WriteLine("Outsamps: "+outsamps.ToString());
                            if ((rb_vac2OUT_l.WriteSpace() >= outsamps) && (rb_vac2OUT_r.WriteSpace() >= outsamps))
                            {
                                Win32.EnterCriticalSection(cs_vac2);
                                rb_vac2OUT_l.WritePtr(res_outl_ptr, outsamps);
                                rb_vac2OUT_r.WritePtr(res_outr_ptr, outsamps);
                                Win32.LeaveCriticalSection(cs_vac2);
                            }
                            else
                            {
                                vac2_rb_reset = true;
                                VACDebug("rb_vac2OUT_l overflow");
                            }
                        }
                    }
                    else
                    {
                        fixed (float* res_outl_ptr = &(res_vac2_outl[0]))
                        {
                            int outsamps;
                            DttSP.DoResamplerF(vac_l, res_outl_ptr, frameCount, &outsamps, resampVAC2PtrOut_l);
                            //Debug.WriteLine("Framecount: "+frameCount.ToString() + " Outsamps: "+outsamps.ToString());
                            if ((rb_vac2OUT_l.WriteSpace() >= outsamps) && (rb_vac2OUT_r.WriteSpace() >= outsamps))
                            {
                                Win32.EnterCriticalSection(cs_vac2);
                                rb_vac2OUT_l.WritePtr(res_outl_ptr, outsamps);
                                rb_vac2OUT_r.WritePtr(res_outl_ptr, outsamps);
                                Win32.LeaveCriticalSection(cs_vac2);
                            }
                            else
                            {
                                vac2_rb_reset = true;
                                VACDebug("rb_vac2OUT_l overflow");
                            }
                        }
                    }
                }
            }

			// Scale output to SDR-1000
			if(!localmox)
			{
				ScaleBuffer(out_l1, out_l1, frameCount, (float)monitor_volume);
				ScaleBuffer(out_r1, out_r1, frameCount, (float)monitor_volume);
				ClearBuffer(out_l2, frameCount);
				ClearBuffer(out_r2, frameCount);
			}
			else
			{
				double tx_vol = TXScale;

				if(tx_output_signal != SignalSource.RADIO)		tx_vol = 1.0;
				
				ScaleBuffer(out_l2, out_l1, frameCount, (float)monitor_volume);
				ScaleBuffer(out_l2, out_l2, frameCount, (float)tx_vol);


				ScaleBuffer(out_r2, out_r1, frameCount, (float)monitor_volume);
				ScaleBuffer(out_r2, out_r2, frameCount, (float)tx_vol);
			}

			/*Debug.WriteLine("Max 1L: "+MaxSample(out_l1, frameCount).ToString("f5")+" 1R: "+MaxSample(out_r1, frameCount).ToString("f5")+
				" 2L: "+MaxSample(out_l2, frameCount).ToString("f5")+" 2R: "+MaxSample(out_r2, frameCount).ToString("f5"));*/

			if(!testing && soundcard != SoundCard.DELTA_44)
			{
				// clip radio output to prevent overdrive
				float clip_thresh = (float)(1.5f / audio_volts1);
				for(int i=0; i<frameCount; i++)
				{
					if(out_l2[i] > clip_thresh)
					{
						//Debug.WriteLine("Clip Left High: "+out_l2[i].ToString("f5"));
						out_l2[i] = clip_thresh;
					}
					else if(out_l2[i] < -clip_thresh)
					{
						//Debug.WriteLine("Clip Left Low: "+out_l2[i].ToString("f5"));
						out_l2[i] = -clip_thresh;
					}

					if(out_r2[i] > clip_thresh) 
					{
						//Debug.WriteLine("Clip Right High: "+out_l2[i].ToString("f5"));
						out_r2[i] = clip_thresh;							
					}
					else if(out_r2[i] < -clip_thresh)
					{
						//Debug.WriteLine("Clip Right Low: "+out_l2[i].ToString("f5"));
						out_r2[i] = -clip_thresh;
					} 

					/*// Branchless clipping -- testing found this was more costly overall and especially when 
					 * when dealing with samples that mostly do not need clipping

					float x1 = Math.Abs(out_l2[i]+clip_thresh);
					float x2 = Math.Abs(out_l2[i]-clip_thresh);
					x1 -= x2;
					out_l2[i] = x1 * 0.5f;
					x1 = Math.Abs(out_r2[i]+clip_thresh);
					x2 = Math.Abs(out_r2[i]-clip_thresh);
					x1 -= x2;
					out_r2[i] = x1 * 0.5f;*/
					

				}

				if(audio_volts1 > 1.5f)
				{
					// scale FireBox monitor output to prevent overdrive
					ScaleBuffer(out_l1, out_l1, frameCount, (float)(1.5f / audio_volts1));
					ScaleBuffer(out_r1, out_r1, frameCount, (float)(1.5f / audio_volts1));
				}
			}

#if(MINMAX)
			Debug.Write(MaxSample(out_l2, out_r2, frameCount).ToString("f6")+",");

			float current_max = MaxSample(out_l2, out_r2, frameCount);
			if(current_max > max) max = current_max;
			Debug.WriteLine(" max: "+max.ToString("f6"));
#endif

#if(TIMER)
			t1.Stop();
			Debug.WriteLine(t1.Duration);
#endif
			return callback_return;
		} // callback4port

#if (MINMAX)
		private static float max = float.MinValue;
#endif
        //private static HiPerfTimer t2 = new HiPerfTimer();
        //private static double period = 0.0;
        //private static HiPerfTimer t3 = new HiPerfTimer();
        //private static ArrayList list = new ArrayList();



     

        //==============================================================================================================         
        // ke9ns  (callback8) called to setup RX and TX streams (to and from the flex radio)
        //        input here input_dev1 from setup.cs
        //        output here output_dev1 from setup.cs
        //==============================================================================================================         
        unsafe public static int Callback2(void* input, void* output, int frameCount, PA19.PaStreamCallbackTimeInfo* timeInfo, int statusFlags, void* userData)
        {
#if (TIMER)
			t1.Start();
#endif
            //t2.Start();
            float* in_l = null, in_r = null;
            float* out_l1 = null, out_r1 = null, out_l2 = null, out_r2 = null;
            float* out_l3 = null, out_r3 = null, out_l4 = null, out_r4 = null;
            float* rx1_in_l = null, rx1_in_r = null, tx_in_l = null, tx_in_r = null;
            float* rx2_in_l = null, rx2_in_r = null;
            float* rx1_out_l = null, rx1_out_r = null, tx_out_l = null, tx_out_r = null;
            float* rx2_out_l = null, rx2_out_r = null;

            localmox = mox;

           
            //=====================================================================================
            // OUTPUT pointer streams

            void* ex_output = (int*)output;

            int* array_ptr_output = (int*)output;

            float* out_l_ptr1 = (float*)array_ptr_output[0];
            float* out_r_ptr1 = (float*)array_ptr_output[1];

            float* out_l_ptr2 = (float*)array_ptr_output[2];
            float* out_r_ptr2 = (float*)array_ptr_output[3];

            float* out_l_ptr3 = (float*)array_ptr_output[4];
            float* out_r_ptr3 = (float*)array_ptr_output[5];

            float* out_l_ptr4 = (float*)array_ptr_output[6];
            float* out_r_ptr4 = (float*)array_ptr_output[7];


            //=====================================================================================
            // INPUT pointer streams

            void* ex_input = (int*)input;

            int* array_ptr_input = (int*)input; // create array of pointers for inputs

            float* in_l_ptr1 = (float*)array_ptr_input[0];// rx1
            float* in_r_ptr1 = (float*)array_ptr_input[1];

            float* in_l_ptr2 = (float*)array_ptr_input[2]; // rx2
            float* in_r_ptr2 = (float*)array_ptr_input[3];

            float* in_l_ptr3 = (float*)array_ptr_input[4]; // spare ??
            float* in_r_ptr3 = (float*)array_ptr_input[5];

            float* in_l_ptr4 = (float*)array_ptr_input[6]; // tx
            float* in_r_ptr4 = (float*)array_ptr_input[7];

          
            // arrange input buffers in the following order:
            // RX1 Left, RX1 Right, TX Left, TX Right, RX2 Left, RX2 Right
            //int* array_ptr = (int *)input;
        

            switch (in_rx1_l)
            {
                case 0: array_ptr_input[0] = (int)in_l_ptr1; break; // ke9ns default
                case 1: array_ptr_input[0] = (int)in_r_ptr1; break;
                case 2: array_ptr_input[0] = (int)in_l_ptr2; break;
                case 3: array_ptr_input[0] = (int)in_r_ptr2; break;
                case 4: array_ptr_input[0] = (int)in_l_ptr3; break;
                case 5: array_ptr_input[0] = (int)in_r_ptr3; break;
                case 6: array_ptr_input[0] = (int)in_l_ptr4; break;
                case 7: array_ptr_input[0] = (int)in_r_ptr4; break;
            }

            switch (in_rx1_r)
            {
                case 0: array_ptr_input[1] = (int)in_l_ptr1; break;
                case 1: array_ptr_input[1] = (int)in_r_ptr1; break; // ke9ns default
                case 2: array_ptr_input[1] = (int)in_l_ptr2; break;
                case 3: array_ptr_input[1] = (int)in_r_ptr2; break;
                case 4: array_ptr_input[1] = (int)in_l_ptr3; break;
                case 5: array_ptr_input[1] = (int)in_r_ptr3; break;
                case 6: array_ptr_input[1] = (int)in_l_ptr4; break;
                case 7: array_ptr_input[1] = (int)in_r_ptr4; break;
            }

            switch (in_tx_l)
            {
                case 0: array_ptr_input[2] = (int)in_l_ptr1; break;
                case 1: array_ptr_input[2] = (int)in_r_ptr1; break;
                case 2: array_ptr_input[2] = (int)in_l_ptr2; break;
                case 3: array_ptr_input[2] = (int)in_r_ptr2; break;
                case 4: array_ptr_input[2] = (int)in_l_ptr3; break;
                case 5: array_ptr_input[2] = (int)in_r_ptr3; break;
                case 6: array_ptr_input[2] = (int)in_l_ptr4; break; // ke9ns default
                case 7: array_ptr_input[2] = (int)in_r_ptr4; break;
            }

            switch (in_tx_r)
            {
                case 0: array_ptr_input[3] = (int)in_l_ptr1; break;
                case 1: array_ptr_input[3] = (int)in_r_ptr1; break;
                case 2: array_ptr_input[3] = (int)in_l_ptr2; break;
                case 3: array_ptr_input[3] = (int)in_r_ptr2; break;
                case 4: array_ptr_input[3] = (int)in_l_ptr3; break;
                case 5: array_ptr_input[3] = (int)in_r_ptr3; break;
                case 6: array_ptr_input[3] = (int)in_l_ptr4; break;
                case 7: array_ptr_input[3] = (int)in_r_ptr4; break; // ke9ns default
            }

            switch (in_rx2_l)
            {
                case 0: array_ptr_input[4] = (int)in_l_ptr1; break;
                case 1: array_ptr_input[4] = (int)in_r_ptr1; break;
                case 2: array_ptr_input[4] = (int)in_l_ptr2; break; // ke9ns default
                case 3: array_ptr_input[4] = (int)in_r_ptr2; break;
                case 4: array_ptr_input[4] = (int)in_l_ptr3; break;
                case 5: array_ptr_input[4] = (int)in_r_ptr3; break;
                case 6: array_ptr_input[4] = (int)in_l_ptr4; break;
                case 7: array_ptr_input[4] = (int)in_r_ptr4; break;
            }
            switch (in_rx2_r)
            {
                case 0: array_ptr_input[5] = (int)in_l_ptr1; break;
                case 1: array_ptr_input[5] = (int)in_r_ptr1; break;
                case 2: array_ptr_input[5] = (int)in_l_ptr2; break;
                case 3: array_ptr_input[5] = (int)in_r_ptr2; break; // ke9ns default
                case 4: array_ptr_input[5] = (int)in_l_ptr3; break;
                case 5: array_ptr_input[5] = (int)in_r_ptr3; break;
                case 6: array_ptr_input[5] = (int)in_l_ptr4; break;
                case 7: array_ptr_input[5] = (int)in_r_ptr4; break;
            }


            //  Debug.Write(" in_rx1_l " + in_rx1_l);
            //  Debug.Write(" in_rx1_r " + in_rx1_r);
            //  Debug.Write(" in_tx_l " + in_tx_l);
            //  Debug.Write(" in_tx_r " + in_tx_r);
            //  Debug.Write(" in_rx2_l " + in_rx2_l);
            //   Debug.Write(" in_rx2_r " + in_rx2_r);

            // output from DSP is organized as follows
            //=========================================================
            //Channel |   0      1    |    2      3   |   4      5    |
            //Signal  | RX1 L  RX1 R  |  TX L   TX R  | RX2 L  RX2 R  |
            //Pointer | out_l1 out_r1 | out_l2 out_r2 | out_l3 out_r3 |
            //=========================================================

            // output DAC lineup for FLEX-5000
            //===================================================================================================================
            //Channel |   0       1    |      2           3       |     4           5      |            6            |    7     |
            //Signal  | QSE I   QSE Q  | Headphone R  Headphone L | Ext Spkr R  Ext Spkr L | RCA Line Out + FlexWire | Int Spkr |
            //Pointer | out_l1  out_r1 |    out_l2      out_r2    |   out_l3      out_r3   |          out_l4         |  out_r4  |
            //===================================================================================================================

            //------------------------------------------------------------------------------
            //------------------------------------------------------------------------------
            //------------------------------------------------------------------------------
            // PRE PROCESSED STREAMS (original input streams)

            // RECEIVER1 INPUT Stream (I believe this would be as wide as your sample rate)
            // if receiving on RX1 (this is raw audio stream from the RX1 signal)
            rx1_in_l = (float*)array_ptr_input[0]; // = in_l_ptr1;  points to -> in_l
            rx1_in_r = (float*)array_ptr_input[1]; // = in_r_ptr1;  points to -> in_r

            //------------------------------------------------------------------------------
            // Transmint INPUT Stream (as wide as your source stream)
            // if transmitting (this is the raw audio stream to transmit (from say your MIC) but its whatever input you direct to this stream)
            tx_in_l = (float*)array_ptr_input[2]; // = in_l_ptr4;  points to = in_l
            tx_in_r = (float*)array_ptr_input[3]; // = in_r_ptr4;  points to = in_r

            //------------------------------------------------------------------------------
            // RECEIVER2 INPUT Stream (wide as your sample rate)
            // if receiving on RX2 (this is the raw audio stream from the RX2 signal)
            rx2_in_l = (float*)array_ptr_input[4]; // = in_l_ptr2;  
            rx2_in_r = (float*)array_ptr_input[5]; // = in_r_ptr2; 

            //------------------------------------------------------------------------------
            // SPARE INPUT Stream ?
            //   rx_in_l = (float*)array_ptr_input[6]; // = in_l_ptr3;  
           //  rx_in_r = (float*)array_ptr_input[7]; // = in_r_ptr3; 

            //------------------------------------------------------------------------------
            //------------------------------------------------------------------------------
            //------------------------------------------------------------------------------
            // POST PROCESSED STREAMS (streams after they get a pass thru DttSP routines->winmain, update, etc)

            // Now Filtered From RX1 audio stream reduced down to the passband high/low rx filters and eq'ed 
            rx1_out_l = (float*)array_ptr_output[0]; // out_l_ptr1;  out_l1 = QSE I
            rx1_out_r = (float*)array_ptr_output[1]; // out_r_ptr1;  out_r1 = QSE Q

            //------------------------------------------------------------------------------
            // SEND TO TRANSMITTER and HEADPHONE JACK (this is the audio stream sent to your headphone and transmitter)  (ramp up function on this stream only)
            // Now Filtered TX audio stream reduced down to the TX passband high/low TX fitlers and eq'ed 
            tx_out_l = (float*)array_ptr_output[2]; //out_l_ptr2;   out_l2 =  L (sent to transmitter ) (sent to headphones in MONps mode)
            tx_out_r = (float*)array_ptr_output[3]; //out_r_ptr2;   out_r2 =  R (sent to transmitter ) (sent to headphones in MONps mode)

            //------------------------------------------------------------------------------
            //   Now Filtered From RX2 audio stream reduced down to the passband high/low rx filters and eq'ed 
            // SEND TO EXTERAL SPEAKER JACK
            rx2_out_l = (float*)array_ptr_output[4]; //out_l_ptr3;  out_l3 = ext spkr L
            rx2_out_r = (float*)array_ptr_output[5]; //out_r_ptr3;  out_r3 = ext spkr R

            //------------------------------------------------------------------------------
            // SEND TO RCA and FLEXWIRE JACK
            //   float* out_l_ptr4 = (float*)array_ptr_output[6];  // out_l4 = RCA out and FLexwire (mono)
            //   float* out_r_ptr4 = (float*)array_ptr_output[7];  // out_r4  = int spker (which is not used) (mono)



            //=============================================================================================
            //=============================================================================================
            // ke9ns below is where the INPUT audio is played
            //=============================================================================================
            //=============================================================================================

            if (!localmox)
            {
                in_l = rx1_in_l;  // ke9ns if Receiving
                in_r = rx1_in_r;
            }
            else
            {
                in_l = tx_in_l;   // ke9ns if transmitting
                in_r = tx_in_r;
            }

            float sum = SumBuffer(rx1_in_l, frameCount); // ke9ns sum up the entire sample to see if anything in it

            if (sum == 0.0f)
            {
                empty_buffers++;
            }
            else
            {
                empty_buffers = 0;
            }

#if true // EHR RX2 QSK
            if (localmox && rx2_enabled && rx2_auto_mute_tx)
            {
                ClearBuffer(rx2_in_l, frameCount);
                ClearBuffer(rx2_in_r, frameCount);
            }
#endif


            //---------------------------------------------------------------------------------------------------------
            // ke9ns  audio playback of wav file (both RX and TX)
//#if NO_WIDETX
            if (wave_playback)                 
            {
           
               wave_file_reader.GetPlayBuffer(in_l, in_r); // WAV file audio streamed into in_l and in_r 
             
                if (rx2_enabled)
                {
                    if (wave_file_reader2 != null)
                    {
                        wave_file_reader2.GetPlayBuffer(rx2_in_l, rx2_in_r);
                    }
                    else if (!localmox)
                    {
                        CopyBuffer(in_l, rx2_in_l, frameCount);  // in, out
                        CopyBuffer(in_r, rx2_in_r, frameCount);
                    }
                }

            } // audio file playback
//#endif
            //-------------------------------------------------------------------------------------
            // ke9ns PRE AUDIO PROCESSING WAVE RECORDING
            if (wave_record)                          
            {
                if (!localmox)   // ke9ns record receiving audio 
                {
                    if (record_rx_preprocessed)
                    {
                        wave_file_writer.AddWriteBuffer(rx1_in_l, rx1_in_r);

                        if (wave_file_writer2 != null)  wave_file_writer2.AddWriteBuffer(rx2_in_l, rx2_in_r);
                    }
                }
                else // ke9ns record transmitting audio (mic)
                {
                    if (record_tx_preprocessed) // ke9ns capture preprocessed transmitting audio
                    {
                        wave_file_writer.AddWriteBuffer(tx_in_l, tx_in_r); // ke9ns this audio is still real pre processed audio , where the section further down is post processed and AM mode is in AM mode
                         //   Trace.Write("pretest====");
                    }
                }
            } // audio file record

            if (phase)
            {
                Debug.WriteLine("phase ===================="); // ke9ns testdsp

                //phase_mutex.WaitOne();
                Marshal.Copy(new IntPtr(in_l), phase_buf_l, 0, frameCount);
                Marshal.Copy(new IntPtr(in_r), phase_buf_r, 0, frameCount);
                //phase_mutex.ReleaseMutex();
            }


            //---------------------------------------------------------------------------------
            // handle VAC1 Input (receving send to > out_l_ptr2) (transmitting send pc mic to > tx_in_l) and _r side as well
            
#region vac1

            if (vac_enabled && rb_vacOUT_l != null && rb_vacOUT_r != null)
            {
                if (vac_bypass || !localmox) // drain VAC Input ring buffer
                {
                    if ((rb_vacIN_l.ReadSpace() >= frameCount) && (rb_vacIN_r.ReadSpace() >= frameCount))
                    {
                        Win32.EnterCriticalSection(cs_vac);
                        rb_vacIN_l.ReadPtr(out_l_ptr2, frameCount);
                        rb_vacIN_r.ReadPtr(out_r_ptr2, frameCount);
                        Win32.LeaveCriticalSection(cs_vac);
                    }
                }
                else
                {
                    if (rb_vacIN_l.ReadSpace() >= frameCount)
                    {
                        Win32.EnterCriticalSection(cs_vac);  // ke9ns used to make sure you have control
                        rb_vacIN_l.ReadPtr(tx_in_l, frameCount);
                        rb_vacIN_r.ReadPtr(tx_in_r, frameCount);
                        Win32.LeaveCriticalSection(cs_vac);

                        if (vac_combine_input) AddBuffer(tx_in_l, tx_in_r, frameCount);
                    }
                    else
                    {
                        ClearBuffer(tx_in_l, frameCount);
                        ClearBuffer(tx_in_r, frameCount);
                        VACDebug("rb_vacIN underflow 4inTX");
                    }

                    ScaleBuffer(tx_in_l, tx_in_l, frameCount, (float)vac_preamp);
                    ScaleBuffer(tx_in_r, tx_in_r, frameCount, (float)vac_preamp);
                }

            } // vac1 on 

#endregion

            //---------------------------------------------------------------------------------
            // handle VAC2 Input (receving send to > out_l_ptr2) (transmitting send pc mic to > tx_in_l) and _r side as well
#region vac2

            if (vac2_enabled && rb_vac2OUT_l != null && rb_vac2OUT_r != null)
            {
              
                if (vac_bypass || !localmox || !vfob_tx) // drain VAC2 Input ring buffer
                {

                    if ((rb_vac2IN_l.ReadSpace() >= frameCount) && (rb_vac2IN_r.ReadSpace() >= frameCount))
                    {
                        Win32.EnterCriticalSection(cs_vac2);
                        rb_vac2IN_l.ReadPtr(out_l_ptr2, frameCount);
                        rb_vac2IN_r.ReadPtr(out_r_ptr2, frameCount);
                        Win32.LeaveCriticalSection(cs_vac2);
                    }
                }
                else
                {
                  
                    if (rb_vac2IN_l.ReadSpace() >= frameCount)
                    {
                        Win32.EnterCriticalSection(cs_vac2);
                        rb_vac2IN_l.ReadPtr(tx_in_l, frameCount);
                        rb_vac2IN_r.ReadPtr(tx_in_r, frameCount);
                        Win32.LeaveCriticalSection(cs_vac2);

                        if (vac2_combine_input) AddBuffer(tx_in_l, tx_in_r, frameCount);

                        ScaleBuffer(tx_in_l, tx_in_l, frameCount, (float)vac2_tx_scale);
                        ScaleBuffer(tx_in_r, tx_in_r, frameCount, (float)vac2_tx_scale);
                    }
                    else
                    {
                        ClearBuffer(tx_in_l, frameCount);
                        ClearBuffer(tx_in_r, frameCount);
                        VACDebug("rb_vac2IN underflow 4inTX");
                    }
                }
            } // vac2 on 
            #endregion

            //---------------------------------------------------------------------------------
            // VOX enabled (check threshold against tx_in_l and _r)
            #region VOX
            if (vox_enabled)
            {
                if (tx_dsp_mode == DSPMode.LSB ||
                    tx_dsp_mode == DSPMode.USB ||
                    tx_dsp_mode == DSPMode.DSB ||
                    tx_dsp_mode == DSPMode.AM ||
                    tx_dsp_mode == DSPMode.SAM ||
                    tx_dsp_mode == DSPMode.FM ||
                    tx_dsp_mode == DSPMode.DIGL ||
                    tx_dsp_mode == DSPMode.DIGU)
                {
                    peak = MaxSample(tx_in_l, tx_in_r, frameCount);

                    // compare power to threshold
                    if (peak > vox_threshold)    vox_active = true;
                    else
                        vox_active = false;
                }
            }
            
            
            #endregion


            //-----------------------------------------------------------------------------------
            // scale input with mic preamp
            if ((!vac_enabled &&  (tx_dsp_mode == DSPMode.LSB ||
                tx_dsp_mode == DSPMode.USB ||
                tx_dsp_mode == DSPMode.DSB ||
                tx_dsp_mode == DSPMode.AM ||
                tx_dsp_mode == DSPMode.SAM ||
                tx_dsp_mode == DSPMode.FM ||
                tx_dsp_mode == DSPMode.DIGL ||
                tx_dsp_mode == DSPMode.DIGU))
                
                ||
                (vac_enabled && vac_bypass &&  (tx_dsp_mode == DSPMode.DIGL ||
                tx_dsp_mode == DSPMode.DIGU ||
                tx_dsp_mode == DSPMode.LSB ||
                tx_dsp_mode == DSPMode.USB ||
                tx_dsp_mode == DSPMode.DSB ||
                tx_dsp_mode == DSPMode.AM ||
                tx_dsp_mode == DSPMode.SAM ||
                tx_dsp_mode == DSPMode.FM)))
            {


                if (wave_playback) // playing back a wav file as though it was coming from the MIC
                {
//#if NO_WIDETX
                      ScaleBuffer(tx_in_l, tx_in_l, frameCount, (float)wave_preamp);
                      ScaleBuffer(tx_in_r, tx_in_r, frameCount, (float)wave_preamp);
//#endif
                    
                }
                else
                {
                    if (!vac_enabled && (tx_dsp_mode == DSPMode.DIGL || tx_dsp_mode == DSPMode.DIGU || (tx_dsp_mode == DSPMode.FM && console.FMData == true && console.setupForm.chkFMDataMic.Checked == false)))
                    {
                        ScaleBuffer(tx_in_l, tx_in_l, frameCount, (float)vac_preamp);
                        ScaleBuffer(tx_in_r, tx_in_r, frameCount, (float)vac_preamp);
                       

                    }
                    else
                    {
                        if (console.setupForm.chkPhaseRotate.Checked == true) // ke9ns add phase rotation
                        {
                            PhaseRotate(tx_in_l, tx_in_l, frameCount, (float)mic_preamp);
                            PhaseRotate(tx_in_r, tx_in_r, frameCount, (float)mic_preamp);
                        }
                        else
                        {
                            ScaleBuffer(tx_in_l, tx_in_l, frameCount, (float)mic_preamp);
                            ScaleBuffer(tx_in_r, tx_in_r, frameCount, (float)mic_preamp);
                        }

                        if ((console.TXMeter2 == true) && (console.CurrentMeterTX1Mode == MeterTXMode.MIC)) peak1 = MaxSample(tx_in_l, tx_in_r, frameCount); // ke9ns add to allow for MIC level check in RX mode
                    }
                }


            } // scale inputs


            //---------------------------------------------------------------------------------
            // input signal source (inject sine,noise,etc into rx1_in_l & _r)

#region Input Signal Source

            switch (rx1_input_signal)
            {
                case SignalSource.RADIO:
                    break;
                case SignalSource.SINE:
                    SineWave(rx1_in_l, frameCount, phase_accumulator1, sine_freq1);
                    phase_accumulator1 = CosineWave(rx1_in_r, frameCount, phase_accumulator1, sine_freq1);
                    ScaleBuffer(rx1_in_l, rx1_in_l, frameCount, (float)source_scale);
                    ScaleBuffer(rx1_in_r, rx1_in_r, frameCount, (float)source_scale);
                    break;
                case SignalSource.SINE_TWO_TONE:
                    double dump;
                    SineWave2Tone(rx1_in_l, frameCount, phase_accumulator1, phase_accumulator2,
                        sine_freq1, sine_freq2, out dump, out dump);
                    CosineWave2Tone(rx1_in_r, frameCount, phase_accumulator1, phase_accumulator2,
                        sine_freq1, sine_freq2, out phase_accumulator1, out phase_accumulator2);
                    ScaleBuffer(rx1_in_l, rx1_in_l, frameCount, (float)source_scale);
                    ScaleBuffer(rx1_in_r, rx1_in_r, frameCount, (float)source_scale);
                    break;
                case SignalSource.SINE_LEFT_ONLY:
                    phase_accumulator1 = SineWave(rx1_in_l, frameCount, phase_accumulator1, sine_freq1);
                    ScaleBuffer(rx1_in_l, rx1_in_l, frameCount, (float)source_scale);
                    ClearBuffer(rx1_in_r, frameCount);
                    break;
                case SignalSource.SINE_RIGHT_ONLY:
                    phase_accumulator1 = SineWave(rx1_in_r, frameCount, phase_accumulator1, sine_freq1);
                    ScaleBuffer(rx1_in_r, rx1_in_r, frameCount, (float)source_scale);
                    ClearBuffer(rx1_in_l, frameCount);
                    break;
                case SignalSource.NOISE:
                    Noise(rx1_in_l, frameCount);
                    Noise(rx1_in_r, frameCount);
                    break;
                case SignalSource.TRIANGLE:
                    Triangle(rx1_in_l, frameCount, sine_freq1);
                    CopyBuffer(rx1_in_l, rx1_in_r, frameCount);
                    break;
                case SignalSource.SAWTOOTH:
                    Sawtooth(rx1_in_l, frameCount, sine_freq1);
                    CopyBuffer(rx1_in_l, rx1_in_r, frameCount);
                    break;
                case SignalSource.PULSE:
                    Pulse(rx1_in_l, frameCount);
                    CopyBuffer(rx1_in_l, rx1_in_r, frameCount);
                    ScaleBuffer(rx1_in_l, rx1_in_l, frameCount, (float)source_scale);
                    ScaleBuffer(rx1_in_r, rx1_in_r, frameCount, (float)source_scale);
                    break;
                case SignalSource.SILENCE:
                    ClearBuffer(rx1_in_l, frameCount);
                    ClearBuffer(rx1_in_r, frameCount);
                    break;
            }

            if (rx2_enabled)
            {
                switch (rx2_input_signal)
                {
                    case SignalSource.RADIO:
                        break;
                    case SignalSource.SINE:
                        SineWave(rx2_in_l, frameCount, phase_accumulator1, sine_freq1);
                        phase_accumulator1 = CosineWave(rx2_in_r, frameCount, phase_accumulator1, sine_freq1);
                        ScaleBuffer(rx2_in_l, rx2_in_l, frameCount, (float)source_scale);
                        ScaleBuffer(rx2_in_r, rx2_in_r, frameCount, (float)source_scale);
                        break;
                    case SignalSource.SINE_TWO_TONE:
                        double dump;
                        SineWave2Tone(rx2_in_l, frameCount, phase_accumulator1, phase_accumulator2,
                            sine_freq1, sine_freq2, out dump, out dump);
                        CosineWave2Tone(rx2_in_r, frameCount, phase_accumulator1, phase_accumulator2,
                            sine_freq1, sine_freq2, out phase_accumulator1, out phase_accumulator2);
                        ScaleBuffer(rx2_in_l, rx2_in_l, frameCount, (float)source_scale);
                        ScaleBuffer(rx2_in_r, rx2_in_r, frameCount, (float)source_scale);
                        break;
                    case SignalSource.SINE_LEFT_ONLY:
                        phase_accumulator1 = SineWave(rx2_in_l, frameCount, phase_accumulator1, sine_freq1);
                        ScaleBuffer(rx2_in_l, rx2_in_l, frameCount, (float)source_scale);
                        ClearBuffer(rx2_in_r, frameCount);
                        break;
                    case SignalSource.SINE_RIGHT_ONLY:
                        phase_accumulator1 = SineWave(rx2_in_r, frameCount, phase_accumulator1, sine_freq1);
                        ScaleBuffer(rx2_in_r, rx2_in_r, frameCount, (float)source_scale);
                        ClearBuffer(rx2_in_l, frameCount);
                        break;
                    case SignalSource.NOISE:
                        Noise(rx2_in_l, frameCount);
                        Noise(rx2_in_r, frameCount);
                        break;
                    case SignalSource.TRIANGLE:
                        Triangle(rx2_in_l, frameCount, sine_freq1);
                        CopyBuffer(rx2_in_l, rx2_in_r, frameCount);
                        break;
                    case SignalSource.SAWTOOTH:
                        Sawtooth(rx2_in_l, frameCount, sine_freq1);
                        CopyBuffer(rx2_in_l, rx2_in_r, frameCount);
                        break;
                    case SignalSource.PULSE:
                        Pulse(rx2_in_l, frameCount);
                        CopyBuffer(rx2_in_l, rx2_in_r, frameCount);
                        ScaleBuffer(rx2_in_l, rx2_in_l, frameCount, (float)source_scale);
                        ScaleBuffer(rx2_in_r, rx2_in_r, frameCount, (float)source_scale);
                        break;
                    case SignalSource.SILENCE:
                        ClearBuffer(rx2_in_l, frameCount);
                        ClearBuffer(rx2_in_r, frameCount);
                        break;
                }
            }

            switch (tx_input_signal)
            {
                case SignalSource.RADIO:

                 
                        if (console.keydot == true) // ke9ns add allow cw while talking
                        {
                            SineWave(tx_in_l, frameCount, phase_accumulator1, (double)console.udCWPitch.Value);
                            phase_accumulator1 = CosineWave(tx_in_r, frameCount, phase_accumulator1, (double)console.udCWPitch.Value);
                        }
                    break;
                case SignalSource.SINE:

                   
                    if ((console.PulseON == true))  // ke9ns add
                    {
                        Pulser(tx_in_l, frameCount, sine_freq1);  // ke9ns add pulser function    
                        CopyBuffer(tx_in_l, tx_in_r, frameCount);
                    }
                    else
                    {
                        SineWave(tx_in_l, frameCount, phase_accumulator1, sine_freq1);
                        phase_accumulator1 = CosineWave(tx_in_r, frameCount, phase_accumulator1, sine_freq1);
                    }

                
                    /*if (!ramp_down)
                    {*/
                    ScaleBuffer(tx_in_l, tx_in_l, frameCount, (float)source_scale);
                    ScaleBuffer(tx_in_r, tx_in_r, frameCount, (float)source_scale);
                    /*}
                    else
                    {
                        RampDownBuffer(tx_in_l, frameCount, (float)source_scale);
                        RampDownBuffer(tx_in_r, frameCount, (float)source_scale);
                        if (ramp_count >= ramp_samples)
                        {
                            tx_input_signal = SignalSource.RADIO;
                            ramp_down = false;
                        }
                    }*/
                    break;
                case SignalSource.SINE_TWO_TONE:
                    double dump;
                    SineWave2Tone(tx_in_l, frameCount, phase_accumulator1, phase_accumulator2,
                        sine_freq1, sine_freq2, out dump, out dump);
                    CosineWave2Tone(tx_in_r, frameCount, phase_accumulator1, phase_accumulator2,
                        sine_freq1, sine_freq2, out phase_accumulator1, out phase_accumulator2);
                    ScaleBuffer(tx_in_l, tx_in_l, frameCount, (float)source_scale);
                    ScaleBuffer(tx_in_r, tx_in_r, frameCount, (float)source_scale);
                    break;
                case SignalSource.NOISE:
                    Noise(tx_in_l, frameCount);
                    Noise(tx_in_r, frameCount);
                    break;
                case SignalSource.TRIANGLE:
                    Triangle(tx_in_l, frameCount, sine_freq1);
                    CopyBuffer(tx_in_l, tx_in_r, frameCount);
                    break;
                case SignalSource.SAWTOOTH:
                    Sawtooth(tx_in_l, frameCount, sine_freq1);
                    CopyBuffer(tx_in_l, tx_in_r, frameCount);
                    break;
                case SignalSource.PULSE:
                    Pulse(tx_in_l, frameCount);
                    CopyBuffer(tx_in_l, tx_in_r, frameCount);
                    ScaleBuffer(tx_in_l, tx_in_l, frameCount, (float)source_scale);
                    ScaleBuffer(tx_in_r, tx_in_r, frameCount, (float)source_scale);

                    break;
                case SignalSource.SILENCE:
                    ClearBuffer(tx_in_l, frameCount);
                    ClearBuffer(tx_in_r, frameCount);
                    break;
            }

#endregion

#if (MINMAX)
			/*float local_max = MaxSample(in_l, in_r, frameCount);
			if(local_max > max)
			{
				max = local_max;
				Debug.WriteLine("max in: "+max.ToString("f6"));
			}*/

			Debug.Write(MaxSample(in_l, in_r, frameCount).ToString("f6")+",");
#endif


            //---------------------------------------------------------------------------------
            // handle Direct IQ for VAC1
#region vac1IQ

            if (vac_enabled && vac_output_iq && (rb_vacOUT_l != null) && (rb_vacOUT_r != null) && (rx1_in_l != null) && (rx1_in_r != null) )
            {
                if ((rb_vacOUT_l.WriteSpace() >= frameCount) && (rb_vacOUT_r.WriteSpace() >= frameCount))
                {
                    if (vac_correct_iq)
                        fixed (float* res_outl_ptr = &(res_outl[0]))
                            fixed (float* res_outr_ptr = &(res_outr[0]))
                        {
                            if (vac_output_rx2)
                                CorrectIQBuffer(rx2_in_l, rx2_in_r, res_outl_ptr, res_outr_ptr, frameCount);
                            else
                                CorrectIQBuffer(rx1_in_l, rx1_in_r, res_outl_ptr, res_outr_ptr, frameCount);

                            Win32.EnterCriticalSection(cs_vac);

                            rb_vacOUT_l.WritePtr(res_outr_ptr, frameCount); // why are these reversed??
                            rb_vacOUT_r.WritePtr(res_outl_ptr, frameCount);

                            Win32.LeaveCriticalSection(cs_vac);

                        }
                    else
                    {
                        Win32.EnterCriticalSection(cs_vac);
                        if (vac_output_rx2)
                        {
                            rb_vacOUT_l.WritePtr(rx2_in_r, frameCount);
                            rb_vacOUT_r.WritePtr(rx2_in_l, frameCount);
                        }
                        else
                        {
                            rb_vacOUT_l.WritePtr(rx1_in_r, frameCount);
                            rb_vacOUT_r.WritePtr(rx1_in_l, frameCount);
                        }
                        Win32.LeaveCriticalSection(cs_vac);
                    }
                }
                else
                {
                    VACDebug("rb_vacOUT_l I/Q overflow ");
                    vac_rb_reset = true;
                }
            } // vac1 with IQ ON

#endregion // vac1IQ

            //---------------------------------------------------------------------------------
            // handle Direct IQ for VAC2
#region VAC2IQ

            if (vac2_enabled && vac2_output_iq && rb_vac2OUT_l != null && rb_vac2OUT_r != null)
            {
                Debug.WriteLine("4VAC2--");

                if ((rb_vac2OUT_l.WriteSpace() >= frameCount) && (rb_vac2OUT_r.WriteSpace() >= frameCount))
                {
                    if (vac_correct_iq)
                        fixed (float* res_outl_ptr = &(res_vac2_outl[0]))
                        fixed (float* res_outr_ptr = &(res_vac2_outr[0]))
                        {
                            if (vac2_rx2)
                                CorrectIQBuffer(rx2_in_l, rx2_in_r, res_outl_ptr, res_outr_ptr, frameCount);
                            else
                                CorrectIQBuffer(rx1_in_l, rx1_in_r, res_outl_ptr, res_outr_ptr, frameCount);

                            Win32.EnterCriticalSection(cs_vac2);
                            rb_vac2OUT_l.WritePtr(res_outr_ptr, frameCount); // why are these reversed??
                            rb_vac2OUT_r.WritePtr(res_outl_ptr, frameCount);
                            Win32.LeaveCriticalSection(cs_vac2);

                        }
                    else
                    {
                        Win32.EnterCriticalSection(cs_vac2);
                        if (vac2_rx2)
                        {
                            rb_vac2OUT_l.WritePtr(rx2_in_r, frameCount);
                            rb_vac2OUT_r.WritePtr(rx2_in_l, frameCount);
                        }
                        else
                        {
                            rb_vac2OUT_l.WritePtr(rx1_in_r, frameCount);
                            rb_vac2OUT_r.WritePtr(rx1_in_l, frameCount);
                        }
                        Win32.LeaveCriticalSection(cs_vac2);
                    }
                }
                else
                {
                    VACDebug("rb_vac2OUT_l I/Q overflow ");
                    vac2_rb_reset = true;
                }
            } // Vac2 with IQ on

#endregion // VAC2 IQ



            double tx_vol = FWCTXScale;

            //---------------------------------------------------------------------------------
            //---------------------------------------------------------------------------------
            // ke9ns DttSP 

            if (localmox && (tx_dsp_mode == DSPMode.CWL || tx_dsp_mode == DSPMode.CWU))
            {
                DttSP.ExchangeSamples2(ex_input, ex_output, frameCount);   

                double time = CWSensorItem.GetCurrentTime();
                CWSynth.Advance(tx_out_l, tx_out_r, frameCount, time);
            }
            else if (tx_dsp_mode == DSPMode.CWL || tx_dsp_mode == DSPMode.CWU)
            {
                double time = CWSensorItem.GetCurrentTime();
                CWSynth.Advance(tx_out_l, tx_out_r, frameCount, time);

                DttSP.ExchangeSamples2(ex_input, ex_output, frameCount);  
            }
            else
            {
              
                //====================================================================================
                // ke9ns this is where pre audio is converted to post audio (i.e. filtered, eq'ed, modulated, etc, etc,)
                // ke9ns ex_input is a pointer to an array of input pointers array_ptr_input[0-7];
                // ke9ns ex_output is a pointer to an array of output pointers array_ptr_output[0-7];

                DttSP.ExchangeSamples2(ex_input, ex_output, frameCount);            // ke9ns for standard audio do this routine found in  winmain.c as Audio_Callback2
                
            }


#if (MINMAX)
			Debug.Write(MaxSample(out_l_ptr2, frameCount).ToString("f6")+",");
			Debug.Write(MaxSample(out_r_ptr2, frameCount).ToString("f6")+"\n");
#endif

            //=============================================================================================
            //=============================================================================================
            // ke9ns below is where the OUTPUT audio is played (after the DSP just above)
            //=============================================================================================
            //=============================================================================================

            //-------------------------------------------------------------------------------------
            // ke9ns output signal source (sine, noise, triangle, etc to rx1_out_l & _r)
#region Output Signal Source

            switch (rx1_output_signal)
            {
                case SignalSource.RADIO:
                    break;
                case SignalSource.SINE:
                    SineWave(rx1_out_l, frameCount, phase_accumulator1, sine_freq1);
                    phase_accumulator1 = CosineWave(rx1_out_r, frameCount, phase_accumulator1, sine_freq1);
                    ScaleBuffer(rx1_out_l, rx1_out_l, frameCount, (float)source_scale);
                    ScaleBuffer(rx1_out_r, rx1_out_r, frameCount, (float)source_scale);
                    break;
                case SignalSource.SINE_TWO_TONE:
                    double dump;
                    SineWave2Tone(rx1_out_l, frameCount, phase_accumulator1, phase_accumulator2,
                        sine_freq1, sine_freq2, out dump, out dump);
                    CosineWave2Tone(rx1_out_r, frameCount, phase_accumulator1, phase_accumulator2,
                        sine_freq1, sine_freq2, out phase_accumulator1, out phase_accumulator2);
                    ScaleBuffer(rx1_out_l, rx1_out_l, frameCount, (float)source_scale);
                    ScaleBuffer(rx1_out_r, rx1_out_r, frameCount, (float)source_scale);
                    break;
                case SignalSource.SINE_LEFT_ONLY:
                    phase_accumulator1 = SineWave(rx1_out_l, frameCount, phase_accumulator1, sine_freq1);
                    ScaleBuffer(rx1_out_l, rx1_out_l, frameCount, (float)source_scale);
                    ClearBuffer(rx1_out_r, frameCount);
                    break;
                case SignalSource.SINE_RIGHT_ONLY:
                    phase_accumulator1 = SineWave(rx1_out_r, frameCount, phase_accumulator1, sine_freq1);
                    ScaleBuffer(rx1_out_r, rx1_out_r, frameCount, (float)source_scale);
                    ClearBuffer(rx1_out_l, frameCount);
                    break;
                case SignalSource.NOISE:
                    Noise(rx1_out_l, frameCount);
                    Noise(rx1_out_r, frameCount);
                    break;
                case SignalSource.TRIANGLE:
                    Triangle(rx1_out_l, frameCount, sine_freq1);
                    CopyBuffer(rx1_out_l, rx1_out_r, frameCount);
                    break;
                case SignalSource.SAWTOOTH:
                    Sawtooth(rx1_out_l, frameCount, sine_freq1);
                    CopyBuffer(rx1_out_l, rx1_out_r, frameCount);
                    break;
                case SignalSource.PULSE:
                    Pulse(rx1_out_l, frameCount);
                    CopyBuffer(rx1_out_l, rx1_out_r, frameCount);
                    ScaleBuffer(rx1_out_l, rx1_out_l, frameCount, (float)source_scale);
                    ScaleBuffer(rx1_out_r, rx1_out_r, frameCount, (float)source_scale);
                    break;
                case SignalSource.SILENCE:
                    ClearBuffer(rx1_out_l, frameCount);
                    ClearBuffer(rx1_out_r, frameCount);
                    break;
            }

            if (rx2_enabled)
            {
                switch (rx2_output_signal)
                {
                    case SignalSource.RADIO:
                        break;
                    case SignalSource.SINE:
                        SineWave(rx2_out_l, frameCount, phase_accumulator1, sine_freq1);
                        phase_accumulator1 = CosineWave(rx2_out_r, frameCount, phase_accumulator1, sine_freq1);
                        ScaleBuffer(rx2_out_l, rx2_out_l, frameCount, (float)source_scale);
                        ScaleBuffer(rx2_out_r, rx2_out_r, frameCount, (float)source_scale);
                        break;
                    case SignalSource.SINE_TWO_TONE:
                        double dump;
                        SineWave2Tone(rx2_out_l, frameCount, phase_accumulator1, phase_accumulator2,
                            sine_freq1, sine_freq2, out dump, out dump);
                        CosineWave2Tone(rx2_out_r, frameCount, phase_accumulator1, phase_accumulator2,
                            sine_freq1, sine_freq2, out phase_accumulator1, out phase_accumulator2);
                        ScaleBuffer(rx2_out_l, rx2_out_l, frameCount, (float)source_scale);
                        ScaleBuffer(rx2_out_r, rx2_out_r, frameCount, (float)source_scale);
                        break;
                    case SignalSource.SINE_LEFT_ONLY:
                        phase_accumulator1 = SineWave(rx2_out_l, frameCount, phase_accumulator1, sine_freq1);
                        ScaleBuffer(rx2_out_l, rx2_out_l, frameCount, (float)source_scale);
                        ClearBuffer(rx2_out_r, frameCount);
                        break;
                    case SignalSource.SINE_RIGHT_ONLY:
                        phase_accumulator1 = SineWave(rx2_out_r, frameCount, phase_accumulator1, sine_freq1);
                        ScaleBuffer(rx2_out_r, rx2_out_r, frameCount, (float)source_scale);
                        ClearBuffer(rx2_out_l, frameCount);
                        break;
                    case SignalSource.NOISE:
                        Noise(rx2_out_l, frameCount);
                        Noise(rx2_out_r, frameCount);
                        break;
                    case SignalSource.TRIANGLE:
                        Triangle(rx2_out_l, frameCount, sine_freq1);
                        CopyBuffer(rx2_out_l, rx2_out_r, frameCount);
                        break;
                    case SignalSource.SAWTOOTH:
                        Sawtooth(rx2_out_l, frameCount, sine_freq1);
                        CopyBuffer(rx2_out_l, rx2_out_r, frameCount);
                        break;
                    case SignalSource.PULSE:
                        Pulse(rx2_out_l, frameCount);
                        CopyBuffer(rx2_out_l, rx2_out_r, frameCount);
                        ScaleBuffer(rx2_out_l, rx2_out_l, frameCount, (float)source_scale);
                        ScaleBuffer(rx2_out_r, rx2_out_r, frameCount, (float)source_scale);
                        break;
                    case SignalSource.SILENCE:
                        ClearBuffer(rx2_out_l, frameCount);
                        ClearBuffer(rx2_out_r, frameCount);
                        break;
                }
            }

            switch (tx_output_signal)
            {
                case SignalSource.RADIO:
                    break;
                case SignalSource.SINE:


                    SineWave(tx_out_l, frameCount, phase_accumulator1, sine_freq1);
                    phase_accumulator1 = CosineWave(tx_out_r, frameCount, phase_accumulator1, sine_freq1);


                    ScaleBuffer(tx_out_l, tx_out_l, frameCount, (float)source_scale);
                    ScaleBuffer(tx_out_r, tx_out_r, frameCount, (float)source_scale);
                    break;
                case SignalSource.SINE_TWO_TONE:
                    double dump;
                    SineWave2Tone(tx_out_l, frameCount, phase_accumulator1, phase_accumulator2,
                        sine_freq1, sine_freq2, out dump, out dump);
                    CosineWave2Tone(tx_out_r, frameCount, phase_accumulator1, phase_accumulator2,
                        sine_freq1, sine_freq2, out phase_accumulator1, out phase_accumulator2);
                    ScaleBuffer(tx_out_l, tx_out_l, frameCount, (float)source_scale);
                    ScaleBuffer(tx_out_r, tx_out_r, frameCount, (float)source_scale);
                    break;
                case SignalSource.NOISE:
                    Noise(tx_out_l, frameCount);
                    Noise(tx_out_r, frameCount);
                    break;
                case SignalSource.TRIANGLE:
                    Triangle(tx_out_l, frameCount, sine_freq1);
                    CopyBuffer(tx_out_l, tx_out_r, frameCount);
                    break;
                case SignalSource.SAWTOOTH:
                    Sawtooth(tx_out_l, frameCount, sine_freq1);
                    CopyBuffer(tx_out_l, tx_out_r, frameCount);
                    break;
                case SignalSource.PULSE:
                    Pulse(tx_out_l, frameCount);
                    CopyBuffer(tx_out_l, tx_out_r, frameCount);
                    ScaleBuffer(tx_out_l, tx_out_l, frameCount, (float)source_scale);
                    ScaleBuffer(tx_out_r, tx_out_r, frameCount, (float)source_scale);
                    break;
                case SignalSource.SILENCE:
                    ClearBuffer(tx_out_l, frameCount);
                    ClearBuffer(tx_out_r, frameCount);
                    break;
            }

#endregion
          
            //-------------------------------------------------------------------------------------

            if (localmox && ramp)  // ke9ns ramp up function to give amps time to come on line
            {
                for (int i = 0; i < frameCount; i++)
                {
                    float scale = (float)Math.Cos(ramp_count * ramp_step);
                    //Debug.WriteLine("ramp scale: " + scale.ToString("f6"));

                    out_l_ptr2[i] *= scale;
                    out_r_ptr2[i] *= scale;

                    if (++ramp_count > ramp_samples)
                    {
                        // clear the rest of the this TX buffer
                        for (int j = i + 1; j < frameCount; j++)
                        {
                            out_l_ptr2[i] = 0.0f;
                            out_r_ptr2[i] = 0.0f;
                        }

                        mox = false;
                        ramp = false;
                        break;
                    }
                }
            } // if (localmox && ramp)

            //-------------------------------------------------------------------------------------

            if (Display.CurrentDisplayMode == DisplayMode.SCOPE || Display.CurrentDisplayMode == DisplayMode.PANASCOPE  )  // ke9ns add  no need to do scope if your not using it.
            { 
                if (!localmox)
                {
                    // ke9ns this produces scope screen data in Receive out_l_ptr1 = rx1_out_l  = OUT_L1
                    DoScope(out_l_ptr1, frameCount); // why on separate channels for RX/TX?
                }
                else
                {
                    // ke9ns this produces scope screan data in Transmit, and yet MON audio in AM mode the audio has an envelope
                    DoScope(out_l_ptr2, frameCount);  // ke9ns out_L_ptr2 = tx_out_L = OUT_L2 (this shows AM signal just fine, but audio out is bad)
                }

           } // do only if needing scope


            //--------------------------------------------------------------
            // ke9ns add  comes here every 10msec @ 192kSR, 21msec @ 96kSR, 42msec @ 48kSR with 2048 Buffer size
            if (console.BeaconSigAvg == true)
            {

         

                fixed (float* WWVP = &console.WWV_data[console.WWVframeCount]) // 2048 readings per frame
                {
                      Win32.memcpy(WWVP, out_l_ptr1, frameCount * sizeof(float));  // dest,source  # of bytes to copy 2048 float sized bytes
        
                }


                if (console.SampleRate1 == 192000) 
                {
                   
                    if (console.WWV_Count == 5) 
                    {
                      
                        console.WWVTone = console.Goertzel(console.WWV_data, 0, console.WWVframeCount); // determine the magnitude of the 100hz TONE in the sample

                        console.WWVframeCount = 0;
                        console.WWVReady = true;
                        console.WWV_Count = 0;
                    
                    }
                   else
                    {
                        // console.WWV_Count = frameCount;  // double up the 192kSR so you get 20msec of data
                        console.WWV_Count++;
                        console.WWVframeCount = (frameCount * console.WWV_Count);  // double up the 192kSR so you get 20msec of data


                    }
                }
                else // if SR !=192k
                {
               
                
                    console.WWVTone = console.Goertzel(console.WWV_data, 0, frameCount); // determine the magnitude of the 100hz TONE in the sample
                    console.WWVReady = true;
                    console.WWV_Count = 0;
                    console.WWVframeCount = 0;

               }


            } //   if (console.BeaconSigAvg == true)
            //-------------------------------------------------------------------------------------
            // ke9ns POST AUDIO PROCESSING WAVE RECORDING

            if (wave_record)
			{
				if(!localmox) // receiving while recording, so record the samplerate (entire panadapter)
				{
					if(!record_rx_preprocessed)
					{
						wave_file_writer.AddWriteBuffer(out_l_ptr1, out_r_ptr1);    // record RX1:  rx1_out_l & rx1_out_r,   out_l1 & out_r1



                       //     Debug.WriteLine("2===========testing"); // ke9ns testdsp

                        if (wave_file_writer2 != null)  wave_file_writer2.AddWriteBuffer(rx2_out_l, rx2_out_r);  // record RX2
					}
				}
				else // transmitting while recording (so record my MIC or whatever is going to the transmitter)
				{
					if(!record_tx_preprocessed)               // ke9ns either do here or up above
					{
						wave_file_writer.AddWriteBuffer(out_l_ptr2, out_r_ptr2); // ke9ns post process audio and so AM mode is modulated here (above wave_record section is pre process so its not modulated)
                    
                      //  Trace.Write("test9===============");

                    }
				}

			} // wave_record

         

            //-------------------------------------------------------------------------------------
            // output from DSP unit (post processed steam)

            out_l1 = rx1_out_l;   // ke9ns RX1 receive signal (from radio unless setup->test->receiver is changed) out_l_ptr1
			out_r1 = rx1_out_r;   // out_r_ptr1

			out_l2 = out_l_ptr2;  // ke9ns transmit signal (from mic) tx_out_l (also sent out to headphones in MON mode)
            out_r2 = out_r_ptr2;  // tx_out_R (also sent out to headphones in MON mode)

            out_l3 = out_l_ptr3;  // ke9ns RX2 receive signal (also sent out to ext speaker in MON mode)as in out_l2 copied to out_l3
            out_r3 = out_r_ptr3;  // ke9ns (also sent out to ext speaker in MON mode)

            out_l4 = out_l_ptr4;  // ke9ns extra unused buffer  (used for LINE OUT channel as in out_l2 copied to out_l4 in MON mode)
			out_r4 = out_r_ptr4;  // ke9ns internal speaker (which was never used) out_r2 copied to out_r4


            //---------------------------------------------------------------------------------------------------------
            // ke9ns testdsp audio playback ov wav file (both RX and TX)
            /*
            #if !NO_WIDETX
                        if (wave_playback)
                        {
                            //  Debug.WriteLine("wave playback ===================="); // ke9ns testdsp

                                wave_file_reader.GetPlayBuffer(out_l2, out_r2); // WAV file audio streamed into in_l and in_r 

                               ScaleBuffer(out_l2, out_l2, frameCount, (float)wave_preamp);
                               ScaleBuffer(out_r2, out_r2, frameCount, (float)wave_preamp);



                        } // audio file playback

            #endif 
             */
            //---------------------------------------------------------------------------------
            // scale output for VAC1 -- use chan 4 as spare buffer

            #region vac1scale

        //    if ((vac_enabled) && (!vac_output_iq) && (rb_vacIN_l != null) && (rb_vacIN_r != null) && (rb_vacOUT_l != null) && (rb_vacOUT_r != null))

            if ( (vac_enabled) && (!vac_output_iq)  && (rb_vacIN_l != null) && (rb_vacIN_r != null) && (rb_vacOUT_l != null) && (rb_vacOUT_r != null) )
			{
              //  Debug.WriteLine("VAC TESTING ENTER");

                if ((!localmox))
              	{

                  //  if (monitor_volume == 0) // ke9ns test to mute vac1 audio from the mute button (it works, but that is not the intent of the MUT button as flex designed it)
                   // {
                  //      ScaleBuffer(out_l2, out_l4, frameCount, 0.0f);
                   //     ScaleBuffer(out_r2, out_r4, frameCount, 0.0f);
                       
                   // }
                   // else
                   // {
                        ScaleBuffer(out_l1, out_l4, frameCount, (float)vac_rx_scale); // ke9ns input is out1  output is out4
                        ScaleBuffer(out_r1, out_r4, frameCount, (float)vac_rx_scale);

                 //   Debug.WriteLine("VAC TESTING1");
                    // }
                }
                else if ( (console.MuteRX1OnVFOBTX == false))
                {
                    ScaleBuffer(out_l1, out_l4, frameCount, (float)vac_rx_scale);
                    ScaleBuffer(out_r1, out_r4, frameCount, (float)vac_rx_scale);

                  //  Debug.WriteLine("VAC TESTING2");
                }
                else if(mon)
				{

                    if ((monpre == 1) || (tx_dsp_mode == DSPMode.AM || tx_dsp_mode == DSPMode.SAM || tx_dsp_mode == DSPMode.FM)) // ke9ns add  use pre-processed audio for MON function in these modes only
                    {
                        ScaleBuffer(tx_in_l, out_l4, frameCount, (float)vac_rx_scale); // ke9ns add pre process 
                        ScaleBuffer(tx_in_r, out_r4, frameCount, (float)vac_rx_scale);
                    }
                    else
                    {
                        ScaleBuffer(out_l2, out_l4, frameCount, (float)vac_rx_scale); // ke9ns  post process so doesnt work (modulated AM here)
                        ScaleBuffer(out_r2, out_r4, frameCount, (float)vac_rx_scale);
                    }

                }
				else // zero samples going back to VAC since TX monitor is off
				{
					ScaleBuffer(out_l2, out_l4, frameCount, 0.0f);
					ScaleBuffer(out_r2, out_r4, frameCount, 0.0f);
				}

				if (sample_rate2 == sample_rate1) 
				{
					if ((rb_vacOUT_l.WriteSpace() >= frameCount) && (rb_vacOUT_r.WriteSpace() >= frameCount))
					{
						Win32.EnterCriticalSection(cs_vac);              // ke9ns wait for control
						rb_vacOUT_l.WritePtr(out_l4, frameCount);        // ke9ns send out_L4 to ringbuffer vac1out L
						rb_vacOUT_r.WritePtr(out_r4, frameCount);
						Win32.LeaveCriticalSection(cs_vac);
					}
					else 
					{
						VACDebug("rb_vacOUT_l overflow ");
						vac_rb_reset = true;
					}
				} 
				else   // ke9ns do below if the sample rates of internal and VAC1 dont match
				{
					if (vac_stereo)
					{
						fixed(float *res_outl_ptr = &(res_outl[0]))
							fixed(float *res_outr_ptr = &(res_outr[0])) 
							{
								int outsamps;
								DttSP.DoResamplerF(out_l4, res_outl_ptr, frameCount, &outsamps, resampPtrOut_l);
								DttSP.DoResamplerF(out_r4, res_outr_ptr, frameCount, &outsamps, resampPtrOut_r);

								if((rb_vacOUT_l.WriteSpace() >= outsamps) && (rb_vacOUT_r.WriteSpace() >= outsamps))
								{
									Win32.EnterCriticalSection(cs_vac);
									rb_vacOUT_l.WritePtr(res_outl_ptr, outsamps);
									rb_vacOUT_r.WritePtr(res_outr_ptr, outsamps);
									Win32.LeaveCriticalSection(cs_vac);
								}
								else
								{
									vac_rb_reset = true;
									VACDebug("rb_vacOUT_l overflow");
								}
							}
					}
					else  // ke9ns NOT VAC1 STEREO and need a sample rate conversion
					{
						fixed(float *res_outl_ptr = &(res_outl[0]))                                         // ke9ns dont allow garbage collection to move any of this around
						{
							int outsamps;
							DttSP.DoResamplerF(out_l4, res_outl_ptr, frameCount, &outsamps, resampPtrOut_l);
							if ((rb_vacOUT_l.WriteSpace() >= outsamps)&&(rb_vacOUT_r.WriteSpace() >= outsamps))
							{
								Win32.EnterCriticalSection(cs_vac);
								rb_vacOUT_l.WritePtr(res_outl_ptr, outsamps);
								rb_vacOUT_r.WritePtr(res_outl_ptr, outsamps);
								Win32.LeaveCriticalSection(cs_vac);
							}
							else
							{
								vac_rb_reset = true;
								VACDebug("rb_vacOUT_l overflow");
							}
						}
					}
				}

			} // vac1 ON output

#endregion

            //---------------------------------------------------------------------------------
            // scale output for VAC2 -- use chan 4 as spare buffer

#region vac2scale

            if (vac2_enabled && !vac2_output_iq && rb_vac2IN_l != null && rb_vac2IN_r != null && rb_vac2OUT_l != null && rb_vac2OUT_r != null)
            {
                if (!localmox || (localmox && !vfob_tx))
                {
                    if (!vac2_rx2)
                    {
                        ScaleBuffer(out_l1, out_l4, frameCount, (float)vac2_rx_scale);
                        ScaleBuffer(out_r1, out_r4, frameCount, (float)vac2_rx_scale);
                    }
                    else
                    {
                        ScaleBuffer(out_l3, out_l4, frameCount, (float)vac2_rx_scale);
                        ScaleBuffer(out_r3, out_r4, frameCount, (float)vac2_rx_scale);
                    }
                }
                else if (mon)
                {
                    if( (monpre == 1) || (tx_dsp_mode == DSPMode.AM || tx_dsp_mode == DSPMode.SAM || tx_dsp_mode == DSPMode.FM))  // ke9ns add  use pre-processed audio for MON function in these modes only
                    {
                        ScaleBuffer(tx_in_l, out_l4, frameCount, (float)vac2_rx_scale); // ke9ns add pre process so AM is still PCM
                        ScaleBuffer(tx_in_r, out_r4, frameCount, (float)vac2_rx_scale);
                    }
                    else
                    {

                        ScaleBuffer(out_l2, out_l4, frameCount, (float)vac2_rx_scale); // ke9ns post process so AM is modulated here
                        ScaleBuffer(out_r2, out_r4, frameCount, (float)vac2_rx_scale);
                    }

                }
                else // zero samples going back to VAC since TX monitor is off
                {
                    ScaleBuffer(out_l2, out_l4, frameCount, 0.0f);
                    ScaleBuffer(out_r2, out_r4, frameCount, 0.0f);
                }

                if (sample_rate3 == sample_rate1)
                {
                    if ((rb_vac2OUT_l.WriteSpace() >= frameCount) && (rb_vac2OUT_r.WriteSpace() >= frameCount))
                    {
                        Win32.EnterCriticalSection(cs_vac2);
                        rb_vac2OUT_l.WritePtr(out_l4, frameCount);       // ke9ns send audio to ringbuffer for VAC2
                        rb_vac2OUT_r.WritePtr(out_r4, frameCount);
                        Win32.LeaveCriticalSection(cs_vac2);
                    }
                    else
                    {
                        VACDebug("rb_vac2OUT_l overflow ");
                        vac2_rb_reset = true;
                    }
                }
                else
                {
                    if (vac2_stereo)
                    {
                        fixed (float* res_outl_ptr = &(res_vac2_outl[0]))
                        fixed (float* res_outr_ptr = &(res_vac2_outr[0]))
                        {
                            int outsamps;
                            DttSP.DoResamplerF(out_l4, res_outl_ptr, frameCount, &outsamps, resampVAC2PtrOut_l);
                            DttSP.DoResamplerF(out_r4, res_outr_ptr, frameCount, &outsamps, resampVAC2PtrOut_r);
                            if ((rb_vac2OUT_l.WriteSpace() >= outsamps) && (rb_vac2OUT_r.WriteSpace() >= outsamps))
                            {
                                Win32.EnterCriticalSection(cs_vac2);
                                rb_vac2OUT_l.WritePtr(res_outl_ptr, outsamps);
                                rb_vac2OUT_r.WritePtr(res_outr_ptr, outsamps);
                                Win32.LeaveCriticalSection(cs_vac2);
                            }
                            else
                            {
                                vac2_rb_reset = true;
                                VACDebug("rb_vac2OUT_l overflow");
                            }
                        }
                    }
                    else
                    {
                        fixed (float* res_outl_ptr = &(res_vac2_outl[0]))
                        {
                            int outsamps;
                            DttSP.DoResamplerF(out_l4, res_outl_ptr, frameCount, &outsamps, resampVAC2PtrOut_l);
                            if ((rb_vac2OUT_l.WriteSpace() >= outsamps) && (rb_vac2OUT_r.WriteSpace() >= outsamps))
                            {
                                Win32.EnterCriticalSection(cs_vac2);
                                rb_vac2OUT_l.WritePtr(res_outl_ptr, outsamps);
                                rb_vac2OUT_r.WritePtr(res_outl_ptr, outsamps);
                                Win32.LeaveCriticalSection(cs_vac2);
                            }
                            else
                            {
                                vac2_rb_reset = true;
                                VACDebug("rb_vac2OUT_l overflow");
                            }
                        }
                    }
                }
            } // vac2 ON and output
#endregion


        //    double tx_vol = FWCTXScale;

            if (tx_output_signal != SignalSource.RADIO)	tx_vol = 1.0;


            //=============================================================================================
            //=============================================================================================
            // ke9ns below is 
            //=============================================================================================
            //=============================================================================================


            // output from DSP is organized as follows
            //=========================================================
            //Channel |   0      1    |    2      3   |   4      5    |
            //Signal  | RX1 L  RX1 R  |  TX L   TX R  | RX2 L  RX2 R  |
            //Pointer | out_l1 out_r1 | out_l2 out_r2 | out_l3 out_r3 |
            //=========================================================

            // output DAC lineup for FLEX-5000
            //===================================================================================================================
            //Channel |   0       1    |      2           3       |     4           5      |            6            |    7     |
            //Signal  | QSE I   QSE Q  | Headphone R  Headphone L | Ext Spkr R  Ext Spkr L | RCA Line Out + FlexWire | Int Spkr |
            //Pointer | out_l1  out_r1 |    out_l2      out_r2    |   out_l3      out_r3   |          out_l4         |  out_r4  |
            //===================================================================================================================

            // save off RX1 output since this is where the QSE output info goes
            CopyBuffer(out_l1, out_l4, frameCount);
            CopyBuffer(out_r1, out_r4, frameCount);

            // Handle output to QSE (Quadrature Sampling Exciter I and Q ) or Transmit mixer

            // if not in some kind of TX mode, clear the QSE output
            if (!localmox && !full_duplex)
            {
                ClearBuffer(out_l1, frameCount); // receive and normal opeation
                ClearBuffer(out_r1, frameCount);
            }
            else // otherwise, scale using power/swr factors
            {

             //   Debug.Write("testing------------");

                ScaleBuffer(out_l2, out_l1, frameCount, (float)tx_vol);       // ke9ns transmit  (in, out)

                ScaleBuffer(out_r2, out_r1, frameCount, (float)tx_vol);
            }

            // Handle output to speakers/headphones (all but Line Out)

            // handle simple cases first -- non- full-duplex
            if (!full_duplex)
            {
              
                if (!localmox) // RX Mode  (ke9ns only comes here if chkVFOATX is true)
                {
                    if ((console.chkRX1MUTE.Checked == true)) // ke9ns add to allow MUTE of just RX1 only
                    {
                                ClearBuffer(out_l4, frameCount); // 
                                ClearBuffer(out_r4, frameCount);
                        //   Debug.Write("testing------------");

                    }


                    // if RX2 is present, combine the outputs ---- A (GRE)
                    if (rx2_enabled) 
                    {
                        AddBuffer(out_l4, out_l3, frameCount);
                        AddBuffer(out_r4, out_r3, frameCount);
                    }


                  
                    // non-scaled output is already in the Line Out channel

                    // Scale the output for the headphones
                    ScaleBuffer(out_l4, out_r2, frameCount, (float)monitor_volume); // original 
                    ScaleBuffer(out_r4, out_l2, frameCount, (float)monitor_volume); // original

                 //   ScaleBuffer(out_l4, out_r2, frameCount, (float)0); // ke9ns mod
                 //   ScaleBuffer(out_r4, out_l2, frameCount, (float)0); // ke9ns mod

                    // Copy the output for the Ext Spkr
                    CopyBuffer(out_l2, out_l3, frameCount);
                    CopyBuffer(out_r2, out_r3, frameCount);

                    // Copy the output for the Int Spkr
                    CopyBuffer(out_r2, out_r4, frameCount); 
                }
                else if (mon) // TX + Monitor -- B (RED)
                {
                    // scale monitor output to match receiver level (half scale)
                    //   Trace.Write("mon====");


                    if (tx_dsp_mode == DSPMode.AM || tx_dsp_mode == DSPMode.SAM)  // ke9ns add  use pre-processed audio for MON function in these modes only
                    {
                        ScaleBuffer(tx_in_l, out_l2, frameCount, 1.0f); // ke9ns add   preprocess  scaleBuffer(in, out, , )
                        ScaleBuffer(tx_in_l, out_r2, frameCount, 1.0f);

                    }
                    else if ((monpre == 1) || (tx_dsp_mode == DSPMode.FM)) // ke9ns add  use pre-processed audio for MON function in these modes only
                    {
                        ScaleBuffer(tx_in_l, out_l2, frameCount, 1.0f); // ke9ns add preprocess
                        ScaleBuffer(tx_in_l, out_r2, frameCount, 1.0f);
                    }
                    else
                    {
                        ScaleBuffer(out_l2, out_l2, frameCount, 1.0f); // ke9ns post process
                        ScaleBuffer(out_r2, out_r2, frameCount, 1.0f);
                    }

                    // if RX2 is present, combine the outputs
                    if (rx2_enabled && !rx2_auto_mute_tx)
                    {
                        AddBuffer(out_l2, out_r3, frameCount);
                        AddBuffer(out_r2, out_l3, frameCount);
                    }

                    // copy the non-scaled output to the Line Out channel
                    CopyBuffer(out_l2, out_l4, frameCount);

                    // Scale the output for the headphones
                    ScaleBuffer(out_l2, out_l2, frameCount, (float)monitor_volume); // original
                    ScaleBuffer(out_r2, out_r2, frameCount, (float)monitor_volume); // original

                  //  ScaleBuffer(out_l2, out_l2, frameCount, (float)50); // ke9ns mod
                  //  ScaleBuffer(out_r2, out_r2, frameCount, (float)50); // ke9ns mod

                    // Copy the output for the Ext Spkr
                    CopyBuffer(out_l2, out_l3, frameCount);
                    CopyBuffer(out_r2, out_r3, frameCount);

                    // Copy the output for the Int Spkr
                    CopyBuffer(out_r2, out_r4, frameCount);
                }
                else // TX (w/o Monitor) --- C (ORA)
                {
                 //   Debug.Write("transmit===============");

                    // if RX2 is present, use that output
                    if (rx2_enabled && !rx2_auto_mute_tx)
                    {
                        // copy non-scaled output to the Line Out channel
                        CopyBuffer(out_l3, out_l4, frameCount);

                        // Scale the output for the headphones
                        ScaleBuffer(out_l3, out_r2, frameCount, (float)monitor_volume); // original
                        ScaleBuffer(out_r3, out_l2, frameCount, (float)monitor_volume); // original

                    //    ScaleBuffer(out_l3, out_r2, frameCount, (float)50); //ke9ns mod
                   //     ScaleBuffer(out_r3, out_l2, frameCount, (float)50);

                        // Copy the output for the Ext Spkr
                        CopyBuffer(out_l2, out_l3, frameCount);
                        CopyBuffer(out_r2, out_r3, frameCount);

                        // Copy the output for the Int Spkr
                        CopyBuffer(out_r2, out_r4, frameCount);
                    }
                    else // no RX2, so silence all outputs  ---  D (BLU)                  // ke9ns transmit on VFOA
                    {
                        // output silence to Line Out
                        ClearBuffer(out_l4, frameCount);

                        // output silence to Headphones
                        ClearBuffer(out_l2, frameCount);

                        ClearBuffer(out_r2, frameCount);

                        // output silence to Ext Spkr
                        ClearBuffer(out_l3, frameCount);
                        ClearBuffer(out_r3, frameCount);

                        // output silence to Int Spkr
                        ClearBuffer(out_r4, frameCount);
                    }
                }
            }
            else // Full Duplex
            {
                if (!rx2_enabled)
                {
                    // monitor is on, should hear TX audio
                    if (mon) // --- GRE
                    {
                        // scale monitor output to match receiver level (half scale)
                        
                        if (tx_dsp_mode == DSPMode.AM || tx_dsp_mode == DSPMode.SAM )  // ke9ns add  use pre-processed audio for MON function in these modes only
                        {
                            ScaleBuffer(tx_in_l, out_l2, frameCount, 1.0f); // ke9ns add preprocess
                            ScaleBuffer(tx_in_l, out_r2, frameCount, 1.0f);
                        }
                        else if ((monpre == 1) || (tx_dsp_mode == DSPMode.FM))  // ke9ns add  use pre-processed audio for MON function in these modes only
                        {
                            ScaleBuffer(tx_in_l, out_l2, frameCount, 1.0f); // ke9ns add preprocess
                            ScaleBuffer(tx_in_l, out_r2, frameCount, 1.0f);
                        }
                        else
                        {

                            ScaleBuffer(out_l2, out_l2, frameCount, 0.5f); // ke9ns post process
                            ScaleBuffer(out_r2, out_r2, frameCount, 0.5f);
                        }


                        // copy the non-scaled output to the Line Out channel
                        CopyBuffer(out_l2, out_l4, frameCount);

                        // Scale the output for the headphones
                        ScaleBuffer(out_l2, out_l2, frameCount, (float)monitor_volume);
                        ScaleBuffer(out_r2, out_r2, frameCount, (float)monitor_volume);

                        // Copy the output for the Ext Spkr
                        CopyBuffer(out_l2, out_l3, frameCount);
                        CopyBuffer(out_r2, out_r3, frameCount);

                        // Copy the output for the Int Spkr
                        CopyBuffer(out_r2, out_r4, frameCount);
                    }
                    else // monitor is off, should hear RX audio -- RED
                    {
                        // non-scaled output is already in the Line Out channel

                        // Scale the output for the headphones
                        ScaleBuffer(out_l4, out_r2, frameCount, (float)monitor_volume);
                        ScaleBuffer(out_r4, out_l2, frameCount, (float)monitor_volume);

                        // Copy the output for the Ext Spkr
                        CopyBuffer(out_l2, out_l3, frameCount);
                        CopyBuffer(out_r2, out_r3, frameCount);

                        // Copy the output for the Int Spkr
                        CopyBuffer(out_r2, out_r4, frameCount);
                    }
                }
                else // ke9ns    RX2 is enabled and full duplex on (ie chkVFOBTX = true)
                {

                    if ((console.chkRX1MUTE.Checked == true) && ((console.setupForm.chkRX2AutoMuteRX2OnVFOATX.Checked == false || console.setupForm.chkRX2AutoMuteRX1OnVFOBTX.Checked == false)) ) // ke9ns add to allow MUTE of just RX1 only
                    {
                        ClearBuffer(out_l4, frameCount); // 
                        ClearBuffer(out_r4, frameCount);


                    }

                    if (!localmox) // --- ORA
                    {

                        if ((console.chkRX1MUTE.Checked == true)) // ke9ns add to allow MUTE of just RX1 only
                        {
                            ClearBuffer(out_l4, frameCount); // 
                            ClearBuffer(out_r4, frameCount);
                              

                        }


                        // combine RX2 audio with RX1
                        AddBuffer(out_l4, out_l3, frameCount);
                        AddBuffer(out_r4, out_r3, frameCount);                    

                        // non-scaled output is already in the Line Out channel

                        // Scale the output for the headphones
                        ScaleBuffer(out_l4, out_r2, frameCount, (float)monitor_volume);
                        ScaleBuffer(out_r4, out_l2, frameCount, (float)monitor_volume);

                        // Copy the output for the Ext Spkr
                        CopyBuffer(out_l2, out_l3, frameCount);
                        CopyBuffer(out_r2, out_r3, frameCount);

                        // Copy the output for the Int Spkr
                        CopyBuffer(out_r2, out_r4, frameCount);
                    }
                    else if (mon) // monitor is on, should hear RX1 + TX audio -- BLU
                    {
                        // scale monitor output to match receiver level (half scale)

                        if (tx_dsp_mode == DSPMode.AM || tx_dsp_mode == DSPMode.SAM )  // ke9ns add  use pre-processed audio for MON function in these modes only
                        {
                            ScaleBuffer(tx_in_l, out_l2, frameCount, 0.5f); // ke9ns add preprocess
                            ScaleBuffer(tx_in_l, out_r2, frameCount, 0.5f);
                        }
                        else if( (monpre == 1) || (tx_dsp_mode == DSPMode.FM) ) // ke9ns add  use pre-processed audio for MON function in these modes only
                        {
                            ScaleBuffer(tx_in_l, out_l2, frameCount, 1.0f); // ke9ns add preprocess
                            ScaleBuffer(tx_in_l, out_r2, frameCount, 1.0f);
                        }
                        else
                        {
                            ScaleBuffer(out_l2, out_l2, frameCount, 0.5f); // ke9ns post process
                            ScaleBuffer(out_r2, out_r2, frameCount, 0.5f);
                        }

                        // combine the RX1 and TX audio

                        AddBuffer(out_l4, out_l2, frameCount); // ke9ns TX audio 

                        AddBuffer(out_r4, out_r2, frameCount);

                        // Scale the output for the headphones
                        ScaleBuffer(out_l4, out_r2, frameCount, (float)monitor_volume);
                        ScaleBuffer(out_r4, out_l2, frameCount, (float)monitor_volume);

                        // Copy the output for the Ext Spkr
                        CopyBuffer(out_l2, out_l3, frameCount);
                        CopyBuffer(out_r2, out_r3, frameCount);

                        // Copy the output for the Int Spkr
                        CopyBuffer(out_r2, out_r4, frameCount);
                    }
                    else // monitor is off, should hear RX1 audio  -BLACK
                    {
                        // non-scaled output is already in the Line Out channel

                        // Scale the output for the headphones
                        ScaleBuffer(out_l4, out_r2, frameCount, (float)monitor_volume);
                        ScaleBuffer(out_r4, out_l2, frameCount, (float)monitor_volume);

                        // Copy the output for the Ext Spkr
                        CopyBuffer(out_l2, out_l3, frameCount);
                        CopyBuffer(out_r2, out_r3, frameCount);

                        // Copy the output for the Int Spkr
                        CopyBuffer(out_r2, out_r4, frameCount);
                    }
                } // rx2

            } // full duplex

			// Scale output to FLEX-5000
			/*if(full_duplex)
			{
				if(mon)
				{
					ScaleBuffer(out_l2, out_l1, frameCount, (float)tx_vol);
                    if (rx2_enabled)
                        AddBuffer(out_l2, out_l3, frameCount);
					ScaleBuffer(out_l2, out_l2, frameCount, (float)monitor_volume);
					CopyBuffer(out_l2, out_l3, frameCount);
					CopyBuffer(out_l2, out_l4, frameCount);
					ScaleBuffer(out_r2, out_r1, frameCount, (float)tx_vol);
                    if (rx2_enabled)
                        AddBuffer(out_r2, out_r3, frameCount);
					ScaleBuffer(out_r2, out_r2, frameCount, (float)monitor_volume);
					CopyBuffer(out_r2, out_r3, frameCount);
					CopyBuffer(out_r2, out_r4, frameCount);	
				}
				else
				{
                    if (rx2_enabled)
                    {
                        AddBuffer(out_l1, out_l3, frameCount);
                        AddBuffer(out_r1, out_r3, frameCount);
                    }
					ScaleBuffer(out_l1, out_r3, frameCount, (float)monitor_volume);
					ScaleBuffer(out_r1, out_l3, frameCount, (float)monitor_volume);
					ScaleBuffer(out_l2, out_l1, frameCount, (float)tx_vol);
					ScaleBuffer(out_r2, out_r1, frameCount, (float)tx_vol);
					CopyBuffer(out_l3, out_l2, frameCount);
					CopyBuffer(out_r3, out_r2, frameCount);
					CopyBuffer(out_l3, out_l4, frameCount);
					CopyBuffer(out_r3, out_r4, frameCount);
				}
			}
			else if(!localmox)
			{				
				if(rx2_enabled)
				{
					AddBuffer(out_l1, out_l3, frameCount);
					AddBuffer(out_r1, out_r3, frameCount);
				}
				ScaleBuffer(out_l1, out_r3, frameCount, (float)monitor_volume);
				ScaleBuffer(out_r1, out_l3, frameCount, (float)monitor_volume);
				ScaleBuffer(out_l2, out_l1, frameCount, (float)tx_vol);
				ScaleBuffer(out_r2, out_r1, frameCount, (float)tx_vol);
				CopyBuffer(out_l3, out_l2, frameCount);
				CopyBuffer(out_r3, out_r2, frameCount);
				CopyBuffer(out_l3, out_l4, frameCount);				
				CopyBuffer(out_r3, out_r4, frameCount);				
				//ClearBuffer(out_l1, frameCount);
				//ClearBuffer(out_r1, frameCount);
			}
			else
			{
		        // handle all the Left channels first
				ScaleBuffer(out_l2, out_l1, frameCount, (float)tx_vol); // copy/scale output for transmitter
				if(mon)
				{
                    ScaleBuffer(out_l2, out_l2, frameCount, (float)monitor_volume); // copy/scale output for Headphones

                    if (rx2_enabled && !rx2_auto_mute_tx && (tx_dsp_mode == DSPMode.CWL || tx_dsp_mode == DSPMode.CWU))
                    {
                        ScaleBuffer(out_l3, out_l3, frameCount, (float)monitor_volume); // scale RX2 output
                        AddBuffer(out_l2, out_l3, frameCount); // mix RX2 into output
                    }
                        
                    CopyBuffer(out_l2, out_l3, frameCount); // copy output for Ext Spkr
                    CopyBuffer(out_l2, out_l4, frameCount); // copy output for Line Out & FlexWire
				}
				else
				{
					if(rx2_enabled && !rx2_auto_mute_tx)
					{
                        ScaleBuffer(out_l3, out_l2, frameCount, (float)monitor_volume); // copy/scale output for Headphones
                        CopyBuffer(out_l2, out_l3, frameCount); // copy output for Ext Spkr
                        CopyBuffer(out_l2, out_l4, frameCount); // copy output for Line Out & FlexWire
					}
					else
					{
                        ClearBuffer(out_l2, frameCount); // copy output for Headphones
                        ClearBuffer(out_l3, frameCount); // copy output for Ext Spkr
                        ClearBuffer(out_l4, frameCount); // copy output for Line Out & FlexWire
					}
				}

                // Handle all the Right channels next
                ScaleBuffer(out_r2, out_r1, frameCount, (float)tx_vol);	// copy/scale output for transmitter
				if(mon)
				{
                    ScaleBuffer(out_r2, out_r2, frameCount, (float)monitor_volume); // copy/scale output for Headphones

                    if (rx2_enabled && !rx2_auto_mute_tx && (tx_dsp_mode == DSPMode.CWL || tx_dsp_mode == DSPMode.CWU))
                    {
                        ScaleBuffer(out_r3, out_r3, frameCount, (float)monitor_volume); // scale RX2 output
                        AddBuffer(out_r2, out_r3, frameCount); // mix RX2 into output
                    }

                    CopyBuffer(out_r2, out_r3, frameCount); // copy output for Ext Spkr
                    CopyBuffer(out_r2, out_r4, frameCount);	// copy output for Int Spkr
				}
				else
				{
					if(rx2_enabled && !rx2_auto_mute_tx)
					{
                        ScaleBuffer(out_r3, out_r2, frameCount, (float)monitor_volume); // copy/scale output for Headphones
                        CopyBuffer(out_r2, out_r3, frameCount); // copy output for Ext Spkr
                        CopyBuffer(out_r2, out_r4, frameCount); // copy output for Int Spkr
					}
					else
					{
                        ClearBuffer(out_r2, frameCount); // copy output for Headphones
                        ClearBuffer(out_r3, frameCount); // copy output for Ext Spkr
                        ClearBuffer(out_r4, frameCount); // copy output for Int Spkr
					}
				}
			}*/

#if (MINMAX)
			/*Debug.Write(MaxSample(out_l2, out_r2, frameCount).ToString("f6")+",");

			float current_max = MaxSample(out_l2, out_r2, frameCount);
			if(current_max > max) max = current_max;
			Debug.WriteLine(" max: "+max.ToString("f6"));*/
#endif

			//Debug.WriteLine(MaxSample(out_l2, out_r2, frameCount).ToString("f6"));
            //if(period > 8) Debug.
            //    WriteLine("period: " + period.ToString("f2"));
#if (TIMER)
			t1.Stop();
			Debug.WriteLine(t1.Duration);
#endif
            /*t2.Stop();
            period = t2.DurationMsec;
            if(period > 1.0 || statusFlags != 0)
                Debug.WriteLine("flags: " + statusFlags.ToString("X") + "  period: " + period.ToString("f2"));*/

            /*t3.Stop();
            list.Add(t3.DurationMsec);
            if (list.Count == 100)
            {
                for (int i = 1; i < list.Count; i++)
                    list[i-1] = (double)list[i] - (double)list[i - 1];

                double avg = 0.0f;
                for (int i = 0; i < list.Count-1; i++)
                    avg += (double)list[i];
                avg /= (list.Count-1);

                double stdev = 0.0f;
                for (int i = 0; i < list.Count-1; i++)
                    stdev += Math.Pow((double)list[i] - avg, 2.0);

                stdev /= (list.Count - 1);
                stdev = Math.Sqrt(stdev);
                list.Clear();
                Debug.WriteLine("avg: " + avg.ToString("f1") + "  stdev: " + stdev.ToString("f1"));
            }*/

			return callback_return;

		} // callback2




        //=====================================================================================================
        // ke9ns used to input output VAC1 streams
        //=====================================================================================================
        // The VAC callback from 1.8.0 untouched in any way.
        unsafe public static int CallbackVAC(void* input, void* output, int frameCount,
			PA19.PaStreamCallbackTimeInfo* timeInfo, int statusFlags, void *userData)
		{
            if (!vac_enabled) return 0;

          //  Debug.WriteLine("NOT GOOD1============");

            int* array_ptr = (int *)input;

            float* in_l_ptr1 = (float *)array_ptr[0];  // ke9ns this is the inpu from VAC1	int new_input = ((PADeviceInfo)comboAudioInput2.SelectedItem).Index;

            float* in_r_ptr1 = null;

            if (vac_stereo || vac_output_iq) in_r_ptr1 = (float *)array_ptr[1];

            array_ptr = (int *)output;

            float* out_l_ptr1 = (float *)array_ptr[0]; // ke9ns this is the output to VAC1 int new_output = ((PADeviceInfo)comboAudioOutput2.SelectedItem).Index;
            float* out_r_ptr1 = null;

            if (vac_stereo || vac_output_iq) out_r_ptr1 = (float *)array_ptr[1];

			if (vac_rb_reset)
			{
				vac_rb_reset = false;
				ClearBuffer(out_l_ptr1,frameCount);

                if (vac_stereo || vac_output_iq) ClearBuffer(out_r_ptr1, frameCount);

                Win32.EnterCriticalSection(cs_vac);
				rb_vacIN_l.Reset();
				rb_vacIN_r.Reset();

                rb_vacOUT_l.Reset();
				rb_vacOUT_r.Reset();
				Win32.LeaveCriticalSection(cs_vac);

                return 0;
			}

			if (vac_stereo || vac_output_iq)
			{
				if (vac_resample) 
				{
					int outsamps;
					fixed(float *res_inl_ptr = &(res_inl[0]))
						fixed(float *res_inr_ptr = &(res_inr[0])) 
						{
							DttSP.DoResamplerF(in_l_ptr1, res_inl_ptr, frameCount, &outsamps, resampPtrIn_l);
							DttSP.DoResamplerF(in_r_ptr1, res_inr_ptr, frameCount, &outsamps, resampPtrIn_r);
							if ((rb_vacIN_l.WriteSpace() >= outsamps)&&(rb_vacIN_r.WriteSpace() >= outsamps))
							{
								Win32.EnterCriticalSection(cs_vac);
								rb_vacIN_l.WritePtr(res_inl_ptr, outsamps);
								rb_vacIN_r.WritePtr(res_inr_ptr, outsamps);
								Win32.LeaveCriticalSection(cs_vac);
							}
							else 
							{
								vac_rb_reset = true;
								VACDebug("rb_vacIN overflow stereo CBvac");
							}
						}
				} 
				else 
				{
					if ((rb_vacIN_l.WriteSpace() >= frameCount) && (rb_vacIN_r.WriteSpace() >= frameCount))
					{
						Win32.EnterCriticalSection(cs_vac);
						rb_vacIN_l.WritePtr(in_l_ptr1,frameCount);  // ke9ns this is where you normally would come for VAC1. This sets the VAC1 input stream up 
						rb_vacIN_r.WritePtr(in_r_ptr1,frameCount);
						Win32.LeaveCriticalSection(cs_vac);
					}
					else
					{
						//vac_rb_reset = true;
						VACDebug("rb_vacIN overflow mono CBvac");
					}
				}
				
				if ((rb_vacOUT_l.ReadSpace() >= frameCount) && (rb_vacOUT_r.ReadSpace() >= frameCount))
				{
					Win32.EnterCriticalSection(cs_vac);
					rb_vacOUT_l.ReadPtr(out_l_ptr1, frameCount);  // ke9ns this is where you normally would come for VAC1. This sets the VAC1 output stream up
                    rb_vacOUT_r.ReadPtr(out_r_ptr1, frameCount);
					Win32.LeaveCriticalSection(cs_vac);
				}
				else
				{
					ClearBuffer(out_l_ptr1, frameCount);
					ClearBuffer(out_r_ptr1, frameCount);
					VACDebug("rb_vacOUT underflow");
				}
			} 
			else 
			{
				if (vac_resample) 
				{
					int outsamps;
					fixed(float *res_inl_ptr = &(res_inl[0]))
					{
						DttSP.DoResamplerF(in_l_ptr1, res_inl_ptr, frameCount, &outsamps, resampPtrIn_l);
						if ((rb_vacIN_l.WriteSpace() >= outsamps) && (rb_vacIN_r.WriteSpace() >= outsamps))
						{
							Win32.EnterCriticalSection(cs_vac);
							rb_vacIN_l.WritePtr(res_inl_ptr, outsamps);
							rb_vacIN_r.WritePtr(res_inl_ptr, outsamps);
							Win32.LeaveCriticalSection(cs_vac);
						}
						else 
						{
							//vac_rb_reset = true;
							VACDebug("rb_vacIN_l overflow");
						}
					}
				} 
				else 
				{
					if ((rb_vacIN_l.WriteSpace() >= frameCount) && (rb_vacIN_r.WriteSpace() >= frameCount))
					{
						Win32.EnterCriticalSection(cs_vac);
						rb_vacIN_l.WritePtr(in_l_ptr1, frameCount);
						rb_vacIN_r.WritePtr(in_l_ptr1, frameCount);
						Win32.LeaveCriticalSection(cs_vac);
					}
					else
					{
						//vac_rb_reset = true;
						VACDebug("rb_vacIN_l overflow");
					}
				}
				if ((rb_vacOUT_l.ReadSpace() >= frameCount) && (rb_vacOUT_r.ReadSpace() >= frameCount))
				{
					Win32.EnterCriticalSection(cs_vac);
					rb_vacOUT_l.ReadPtr(out_l_ptr1, frameCount);
					rb_vacOUT_r.ReadPtr(out_l_ptr1, frameCount);
					Win32.LeaveCriticalSection(cs_vac);
				}
				else 
				{
					ClearBuffer(out_l_ptr1, frameCount);
					VACDebug("rb_vacOUT_l underflow");
				}
			}

			return 0;

        } //  CallbackVAC



        //=====================================================================================================
        // ke9ns used to input output VAC2 streams
        //=====================================================================================================
        unsafe public static int CallbackVAC2(void* input, void* output, int frameCount,
            PA19.PaStreamCallbackTimeInfo* timeInfo, int statusFlags, void* userData)
        {
            if (!vac2_enabled) return 0;

            int* array_ptr = (int*)input;
            float* in_l_ptr1 = (float*)array_ptr[0];
            float* in_r_ptr1 = null;
            if (vac2_stereo || vac2_output_iq) in_r_ptr1 = (float*)array_ptr[1];
            array_ptr = (int*)output;
            float* out_l_ptr1 = (float*)array_ptr[0];
            float* out_r_ptr1 = null;
            if (vac2_stereo || vac2_output_iq) out_r_ptr1 = (float*)array_ptr[1];

            if (vac2_rb_reset)
            {
                vac2_rb_reset = false;
                ClearBuffer(out_l_ptr1, frameCount);
                if (vac2_stereo || vac2_output_iq) ClearBuffer(out_r_ptr1, frameCount);
                Win32.EnterCriticalSection(cs_vac2);
                rb_vac2IN_l.Reset();
                rb_vac2IN_r.Reset();
                rb_vac2OUT_l.Reset();
                rb_vac2OUT_r.Reset();
                Win32.LeaveCriticalSection(cs_vac2);
                return 0;
            }
            if (vac2_stereo || vac2_output_iq)
            {
                if (vac2_resample)
                {
                    int outsamps;
                    fixed (float* res_inl_ptr = &(res_vac2_inl[0]))
                    fixed (float* res_inr_ptr = &(res_vac2_inr[0]))
                    {
                        DttSP.DoResamplerF(in_l_ptr1, res_inl_ptr, frameCount, &outsamps, resampVAC2PtrIn_l);
                        DttSP.DoResamplerF(in_r_ptr1, res_inr_ptr, frameCount, &outsamps, resampVAC2PtrIn_r);
                        if ((rb_vac2IN_l.WriteSpace() >= outsamps) && (rb_vac2IN_r.WriteSpace() >= outsamps))
                        {
                            Win32.EnterCriticalSection(cs_vac2);
                            rb_vac2IN_l.WritePtr(res_inl_ptr, outsamps);
                            rb_vac2IN_r.WritePtr(res_inr_ptr, outsamps);
                            Win32.LeaveCriticalSection(cs_vac2);
                        }
                        else
                        {
                            vac2_rb_reset = true;
                            VACDebug("rb_vac2IN overflow stereo CBvac");
                        }
                    }
                }
                else
                {
                    if ((rb_vac2IN_l.WriteSpace() >= frameCount) && (rb_vac2IN_r.WriteSpace() >= frameCount))
                    {
                        Win32.EnterCriticalSection(cs_vac2);
                        rb_vac2IN_l.WritePtr(in_l_ptr1, frameCount);
                        rb_vac2IN_r.WritePtr(in_r_ptr1, frameCount);
                        Win32.LeaveCriticalSection(cs_vac2);
                    }
                    else
                    {
                        //vac2_rb_reset = true;
                        VACDebug("rb_vac2IN overflow mono CBvac");
                    }
                }

                if ((rb_vac2OUT_l.ReadSpace() >= frameCount) && (rb_vac2OUT_r.ReadSpace() >= frameCount))
                {
                    Win32.EnterCriticalSection(cs_vac2);
                    rb_vac2OUT_l.ReadPtr(out_l_ptr1, frameCount);
                    rb_vac2OUT_r.ReadPtr(out_r_ptr1, frameCount);
                    Win32.LeaveCriticalSection(cs_vac2);
                }
                else
                {
                    ClearBuffer(out_l_ptr1, frameCount);
                    ClearBuffer(out_r_ptr1, frameCount);
                    VACDebug("rb_vac2OUT underflow");
                }
            }
            else
            {
                if (vac2_resample)
                {
                    int outsamps;
                    fixed (float* res_inl_ptr = &(res_vac2_inl[0]))
                    {
                        DttSP.DoResamplerF(in_l_ptr1, res_inl_ptr, frameCount, &outsamps, resampVAC2PtrIn_l);
                        if ((rb_vac2IN_l.WriteSpace() >= outsamps) && (rb_vac2IN_r.WriteSpace() >= outsamps))
                        {
                            Win32.EnterCriticalSection(cs_vac2);
                            rb_vac2IN_l.WritePtr(res_inl_ptr, outsamps);
                            rb_vac2IN_r.WritePtr(res_inl_ptr, outsamps);
                            Win32.LeaveCriticalSection(cs_vac2);
                        }
                        else
                        {
                            //vac_rb_reset = true;
                            VACDebug("rb_vac2IN_l overflow");
                        }
                    }
                }
                else
                {
                    if ((rb_vac2IN_l.WriteSpace() >= frameCount) && (rb_vac2IN_r.WriteSpace() >= frameCount))
                    {
                        Win32.EnterCriticalSection(cs_vac2);
                        rb_vac2IN_l.WritePtr(in_l_ptr1, frameCount);
                        rb_vac2IN_r.WritePtr(in_l_ptr1, frameCount);
                        Win32.LeaveCriticalSection(cs_vac2);
                    }
                    else
                    {
                        //vac_rb_reset = true;
                        VACDebug("rb_vac2IN_l overflow");
                    }
                }
                if ((rb_vac2OUT_l.ReadSpace() >= frameCount) && (rb_vac2OUT_r.ReadSpace() >= frameCount))
                {
                    Win32.EnterCriticalSection(cs_vac2);
                    rb_vac2OUT_l.ReadPtr(out_l_ptr1, frameCount);
                    rb_vac2OUT_r.ReadPtr(out_l_ptr1, frameCount);
                    Win32.LeaveCriticalSection(cs_vac2);
                }
                else
                {
                    ClearBuffer(out_l_ptr1, frameCount);
                    VACDebug("rb_vac2OUT_l underflow");
                }
            }

            return 0;
        }		

		unsafe public static int Pipe(void* input, void* output, int frameCount,
			PA19.PaStreamCallbackTimeInfo* timeInfo, int statusFlags, void *userData)
		{
			float *inptr = (float *)input;
			float *outptr = (float *)output;

			for(int i=0; i<frameCount; i++)
			{
				*outptr++ = *inptr++;
				*outptr++ = *inptr++;
			}
			return 0;
		}

        #endregion




        #region Buffer Operations

        unsafe private static float SumBuffer(float* buf, int samples)
        {
            float temp = 0.0f;
            for (int i = 0; i < samples; i++)
                temp += buf[i];
            return temp;
        }

        unsafe private static void ClearBuffer(float* buf, int samples)
		{
			Win32.memset(buf, 0, samples*sizeof(float));
		}

		unsafe private static void CopyBuffer(float *inbuf, float *outbuf, int samples)
		{
			Win32.memcpy(outbuf, inbuf, samples*sizeof(float));
		}

		unsafe private static void ScaleBuffer(float *inbuf, float *outbuf, int samples, float scale)
		{
            if (scale == 1.0f)
            {
                CopyBuffer(inbuf, outbuf, samples);
                return;
            }

			for(int i=0; i<samples; i++)
				outbuf[i] = inbuf[i] * scale;
		}

        //===================================================================================================
        // ke9ns add
        unsafe private static void PhaseRotate(float* inbuf, float* outbuf, int samples, float scale)
        {
   
                for (int i = 0; i < samples; i++)
                {
                    outbuf[i] = -inbuf[i] * scale;
                }
                    
        } // PhaseRotate

        /*unsafe private static void RampDownBuffer(float* buf, int samples, float scale)
        {
            for (int i = 0; i < samples; i++)
            {
                if (ramp_count < ramp_samples) ramp_count++;
                buf[i] *= (scale * (float)Math.Cos(ramp_step * ramp_count));
            }
        }*/

        unsafe private static void AddBuffer(float* dest, float* buftoadd, int samples)
		{
			for(int i=0; i<samples; i++)
				dest[i] += buftoadd[i];
		}

		unsafe public static float MaxSample(float* buf, int samples)
		{
			float max = float.MinValue;
			for(int i=0; i<samples; i++)
				max = Math.Max(max, buf[i]);
				
			return max;
		}

		unsafe public static float MaxSample(float* buf1, float* buf2, int samples)
		{
			float max = float.MinValue;
			for(int i=0; i < samples; i++)
			{
				if(buf1[i] > max) max = buf1[i];
				if(buf2[i] > max) max = buf2[i];
			}
			return max;
		}

        unsafe public static float MinSample(float* buf, int samples)
        {
            float min = float.MaxValue;
            for (int i = 0; i < samples; i++)
                min = Math.Min(min, buf[i]);

            return min;
        }

		unsafe public static float MinSample(float* buf1, float* buf2, int samples)
		{
			float min = float.MaxValue;
			for(int i=0; i<samples; i++)
			{
                min = Math.Min(min, buf1[i]);
                min = Math.Min(min, buf2[i]);
			}
				
			return min;
		}

		unsafe private static void CorrectIQBuffer(float *inbufI, float *inbufQ, float *outbufI, float *outbufQ, int samples)
		{
			//float phase = (float) (0.001 * console.dsp.GetDSPRX(0, 0).RXCorrectIQPhase);
			//float gain = (float) (1.0 + 0.001 * console.dsp.GetDSPRX(0, 0).RXCorrectIQGain);
			for (int i=0; i<samples; i++)
			{
                if (vac_output_rx2)
                {
                    outbufI[i] = inbufI[i] + iq_phase2 * inbufQ[i];
                    outbufQ[i] = inbufQ[i] * iq_gain2;
                }
                else
                {
                    outbufI[i] = inbufI[i] + iq_phase * inbufQ[i];
                    outbufQ[i] = inbufQ[i] * iq_gain;
                }
			}
		}

		// returns updated phase accumulator
		unsafe public static double SineWave(float* buf, int samples, double phase, double freq)
		{
			double phase_step = freq/sample_rate1*2*Math.PI;
			double cosval = Math.Cos(phase);
			double sinval = Math.Sin(phase);
			double cosdelta = Math.Cos(phase_step);
			double sindelta = Math.Sin(phase_step);
			double tmpval;

			for(int i=0; i<samples; i++ )
			{
				tmpval = cosval*cosdelta - sinval*sindelta;
				sinval = cosval*sindelta + sinval*cosdelta;
				cosval = tmpval;
						
				buf[i] = (float)(sinval);
											 
				phase += phase_step;
			}

			return phase;
		}

		// returns updated phase accumulator
		unsafe public static double CosineWave(float* buf, int samples, double phase, double freq)
		{
			double phase_step = freq/sample_rate1*2*Math.PI;
			double cosval = Math.Cos(phase);
			double sinval = Math.Sin(phase);
			double cosdelta = Math.Cos(phase_step);
			double sindelta = Math.Sin(phase_step);
			double tmpval;

			for(int i=0; i<samples; i++ )
			{
				tmpval = cosval*cosdelta - sinval*sindelta;
				sinval = cosval*sindelta + sinval*cosdelta;
				cosval = tmpval;
						
				buf[i] = (float)(cosval);
											 
				phase += phase_step;
			}

			return phase;
		}

		unsafe public static void SineWave2Tone(float* buf, int samples, 
			double phase1, double phase2, 
			double freq1, double freq2,
			out double updated_phase1, out double updated_phase2)
		{
			double phase_step1 = freq1/sample_rate1*2*Math.PI;
			double cosval1 = Math.Cos(phase1);
			double sinval1 = Math.Sin(phase1);
			double cosdelta1 = Math.Cos(phase_step1);
			double sindelta1 = Math.Sin(phase_step1);

			double phase_step2 = freq2/sample_rate1*2*Math.PI;
			double cosval2 = Math.Cos(phase2);
			double sinval2 = Math.Sin(phase2);
			double cosdelta2 = Math.Cos(phase_step2);
			double sindelta2 = Math.Sin(phase_step2);
			double tmpval;

			for(int i=0; i<samples; i++ )
			{
				tmpval = cosval1*cosdelta1 - sinval1*sindelta1;
				sinval1 = cosval1*sindelta1 + sinval1*cosdelta1;
				cosval1 = tmpval;

				tmpval = cosval2*cosdelta2 - sinval2*sindelta2;
				sinval2 = cosval2*sindelta2 + sinval2*cosdelta2;
				cosval2 = tmpval;
						
				buf[i] = (float)(sinval1*0.5 + sinval2*0.5);
											 
				phase1 += phase_step1;
				phase2 += phase_step2;
			}

			updated_phase1 = phase1;
			updated_phase2 = phase2;
		}

		unsafe public static void CosineWave2Tone(float* buf, int samples, 
			double phase1, double phase2, 
			double freq1, double freq2,
			out double updated_phase1, out double updated_phase2)
		{
			double phase_step1 = freq1/sample_rate1*2*Math.PI;
			double cosval1 = Math.Cos(phase1);
			double sinval1 = Math.Sin(phase1);
			double cosdelta1 = Math.Cos(phase_step1);
			double sindelta1 = Math.Sin(phase_step1);

			double phase_step2 = freq2/sample_rate1*2*Math.PI;
			double cosval2 = Math.Cos(phase2);
			double sinval2 = Math.Sin(phase2);
			double cosdelta2 = Math.Cos(phase_step2);
			double sindelta2 = Math.Sin(phase_step2);
			double tmpval;

			for(int i=0; i<samples; i++ )
			{
				tmpval = cosval1*cosdelta1 - sinval1*sindelta1;
				sinval1 = cosval1*sindelta1 + sinval1*cosdelta1;
				cosval1 = tmpval;

				tmpval = cosval2*cosdelta2 - sinval2*sindelta2;
				sinval2 = cosval2*sindelta2 + sinval2*cosdelta2;
				cosval2 = tmpval;
						
				buf[i] = (float)(cosval1*0.5 + cosval2*0.5);
											 
				phase1 += phase_step1;
				phase2 += phase_step2;
			}

			updated_phase1 = phase1;
			updated_phase2 = phase2;
		}

		
		private static Random r = new Random();
		private static double y2=0.0;
		private static bool use_last = false;
		private static double boxmuller (double m,double s)
		{
			double x1,x2,w,y1;
			if (use_last)		        /* use value from previous call */
			{
				y1 = y2;
				use_last = false;
			}
			else
			{
				do 
				{
					x1 = (2.0 * r.NextDouble() - 1.0);
					x2 = (2.0 * r.NextDouble() - 1.0);
					w = x1 * x1 + x2 * x2;
				} while ( w >= 1.0 );

				w = Math.Sqrt( (-2.0 * Math.Log( w ) ) / w );
				y1 = x1 * w;
				y2 = x2 * w;
				use_last = true;
			}

			return( m + y1 * s );
		}
		unsafe public static void Noise(float* buf, int samples)
		{
			for(int i=0; i<samples; i++)
			{
				buf[i] = (float)boxmuller(0.0,0.2);
			}
		}

		private static double tri_val = 0.0;
		private static int tri_direction = 1;
		unsafe public static void Triangle(float* buf, int samples, double freq)
		{
			double step = freq/sample_rate1*2*tri_direction;
			for(int i=0; i<samples; i++)
			{
				buf[i] = (float)tri_val;
				tri_val += step;
				if(tri_val >= 1.0 || tri_val <= -1.0)
				{
					step = -step;
					tri_val += 2*step;
					if(step < 0) tri_direction = -1;
					else tri_direction = 1;
				}
			}
		}

		private static double saw_val = 0.0;
		private static int saw_direction = 1;
		unsafe public static void Sawtooth(float* buf, int samples, double freq)
		{
			double step = freq/sample_rate1*saw_direction;
			for(int i=0; i<samples; i++)
			{
				buf[i] = (float)saw_val;
				saw_val += step;
				if(saw_val >= 1.0) saw_val -= 2.0;
				if(saw_val <= -1.0) saw_val += 2.0;
			}
		}

		unsafe public static void AddConstant(float* buf, int samples, float val)
		{
			for(int i=0; i<samples; i++)
				buf[i] += val;
		}

        private static double pulse_duty = 0.01; // percent of total
        public static double PulseDuty
        {
            set { pulse_duty = value; }
        }
        
        private static double pulse_period = 1.0; // in sec
        public static double PulsePeriod
        {
            set { pulse_period = value; }
        }


        //==============================================================================================
        // ke9ns add
        private static double pulse_duty1 = 0.01; // percent of total
        public static double PulseDuty1
        {
            set { pulse_duty1 = value / 100.0; }
        }

        //==============================================================================================
        // ke9ns add
        private static double pulse_period1 = 15.0; // in sec
        public static double PulsePeriod1
        {
            set { pulse_period1 = 1.0 / value; }
        }


        private static double pulse_sine_phase = 0.0f;
        private static int pulse_on_count = 0;
        private static int pulse_off_count = 0;
        private static int pulse_state = 0; // for pulse state machine



        static Stopwatch F1 = new Stopwatch();
        static Stopwatch F2 = new Stopwatch();


        //=========================================================================================
        // ke9ns add pulser function
        // this routine is on a timer based on the SR and buffer size.
        // 192k SR and 2048 buffer = 10msec  (96k and 2048buffer = 20msec)

        unsafe private static void Pulser(float* buf, int samples, double freq)
        {
            double phase_step = freq / sample_rate1 * 2 * Math.PI;
       
            double cosval = Math.Cos(pulse_sine_phase);
            double sinval = Math.Sin(pulse_sine_phase);

            double cosdelta = Math.Cos(phase_step);
            double sindelta = Math.Sin(phase_step);
            double tmpval;


            //   double temp = pulse_period1 / ((double)samples / (double)sample_rate1) * (double)samples;

            double temp = pulse_period1 * (double)sample_rate1; // 20 pulse/second = .05 seconds per pulse  .05 / 192000 = 9600

            int pulse_samples = (int)(temp * pulse_duty1);   // ON Time
            int off_samples = (int)(temp * (1 - pulse_duty1));   // OFF TIME  (the sum of ON + OFF) equals the full period rate 

        
         
            for (int i = 0; i < samples; i++)
            {
                switch (pulse_state)
                {
                    case 0: // pulse on state                       
                        tmpval = cosval * cosdelta - sinval * sindelta;
                        sinval = cosval * sindelta + sinval * cosdelta;
                        cosval = tmpval;

                        buf[i] = (float)(sinval);

                        pulse_sine_phase += phase_step;

                        if (pulse_on_count++ > pulse_samples)
                        {
                           Debug.WriteLine("ON F2: " + F2.ElapsedMilliseconds);
                          //  F1.Restart();
                            // go to off state
                            pulse_state = 1;
                            pulse_off_count = 0;
                        }
                        break;
                    case 1: // pulse off state
                        buf[i] = 0.0f;

                        if (pulse_off_count++ > off_samples)
                        {
                            Debug.WriteLine("OFF F2: " + F2.ElapsedMilliseconds);
                            F2.Restart();

                            // goto on state
                            pulse_state = 0;
                            pulse_on_count = 0;

                            pulse_sine_phase = 0.0; // reset phase for max front end pulse
                        }
                        break;
                }
            }
        } // pulser function


        // this is from the setup->test screen
        unsafe private static void Pulse(float* buf, int samples)
        {
            double phase_step = sine_freq1 / sample_rate1 * 2 * Math.PI;
            double cosval = Math.Cos(pulse_sine_phase);
            double sinval = Math.Sin(pulse_sine_phase);
            double cosdelta = Math.Cos(phase_step);
            double sindelta = Math.Sin(phase_step);
            double tmpval;

            int pulse_samples = (int)(pulse_period * sample_rate1 * pulse_duty);
            int off_samples = (int)(pulse_period * sample_rate1 * (1-pulse_duty));

            for (int i = 0; i < samples; i++)
            {
                switch (pulse_state)
                {
                    case 0: // pulse on state                       
                        tmpval = cosval * cosdelta - sinval * sindelta;
                        sinval = cosval * sindelta + sinval * cosdelta;
                        cosval = tmpval;

                        buf[i] = (float)(sinval);

                        pulse_sine_phase += phase_step;

                        if (pulse_on_count++ > pulse_samples)
                        {
                            // go to off state
                            pulse_state = 1;
                            pulse_off_count = 0;
                        }
                        break;
                    case 1: // pulse off state
                        buf[i] = 0.0f;

                        if (pulse_off_count++ > off_samples)
                        {
                            // goto on state
                            pulse_state = 0;
                            pulse_on_count = 0;

                            pulse_sine_phase = 0.0; // reset phase for max front end pulse
                        }
                        break;
                }
            }
        }

#endregion

#region Misc Routines
        // ======================================================
        // Misc Routines
        // ======================================================

        public static void ResetMinMax()
        {
            min_in_l = float.MaxValue;
            min_in_r = float.MaxValue;
            min_out_l = float.MaxValue;
            min_out_r = float.MaxValue;

            max_in_l = float.MinValue;
            max_in_r = float.MinValue;
            max_out_l = float.MinValue;
            max_out_r = float.MinValue;
        }

		private static void VACDebug(string s)
		{
#if (VAC_DEBUG)
			Debug.WriteLine(s);
#endif
		}

#if (NewVAC)
#region NewInitVac
		unsafe private static void InitVAC()
		{
			
			if(rb_vacOUT_l == null) rb_vacOUT_l = new RingBufferFloat(4*block_size2);
			rb_vacOUT_l.Restart(block_size2);
			
			if(rb_vacOUT_r == null) rb_vacOUT_r = new RingBufferFloat(4*block_size2);
			rb_vacOUT_r.Restart(block_size2);
			int buf_size = rb_vacOUT_r.nblock2(4*(block_size2>block_size1 ? block_size2:block_size1)*sample_rate1/sample_rate2);
			Debug.WriteLine("Vac Bufsize = "+buf_size.ToString());
			if(rb_vacIN_l == null) rb_vacIN_l = new RingBufferFloat(buf_size);
			rb_vacIN_l.Reset();

			if(rb_vacIN_r == null) rb_vacIN_r = new RingBufferFloat(buf_size);
			rb_vacIN_r.Reset();

			if (sample_rate2 != sample_rate1) 
			{
				vac_resample = true;
				if(res_outl == null) res_outl = new float [buf_size];
				if(res_outr == null) res_outr = new float [buf_size];
				if(res_inl == null) res_inl  = new float [buf_size];
				if(res_inr == null) res_inr  = new float [buf_size];

				resampPtrIn_l  = DttSP.NewResamplerF(sample_rate2, sample_rate1);
				resampPtrIn_r  = DttSP.NewResamplerF(sample_rate2, sample_rate1);
				resampPtrOut_l = DttSP.NewResamplerF(sample_rate1, sample_rate2);
				resampPtrOut_r = DttSP.NewResamplerF(sample_rate1, sample_rate2);
			}
			else vac_resample = false;
			cs_vac = (void *)0x0;
			cs_vac = Win32.NewCriticalSection();
			if (Win32.InitializeCriticalSectionAndSpinCount(cs_vac, 0x00000080) == 0)
			{
				vac_enabled = false;
				Debug.WriteLine("CriticalSection Failed");
			}
		}
#endregion
#else
		unsafe private static void InitVAC()
		{
			if(rb_vacOUT_l == null) rb_vacOUT_l = new RingBufferFloat(2*65536);
			rb_vacOUT_l.Restart(vac_output_iq ? block_size1 : block_size_vac);
			
			if(rb_vacOUT_r == null) rb_vacOUT_r = new RingBufferFloat(2*65536);
			rb_vacOUT_r.Restart(vac_output_iq ? block_size1 : block_size_vac);

			if(rb_vacIN_l == null) rb_vacIN_l = new RingBufferFloat(4*65536);
			rb_vacIN_l.Restart(block_size_vac);

			if(rb_vacIN_r == null) rb_vacIN_r = new RingBufferFloat(4*65536);
			rb_vacIN_r.Restart(block_size_vac);

			if (sample_rate2 != sample_rate1 && !vac_output_iq) 
			{
				vac_resample = true;
				if(res_outl == null) res_outl = new float [65536];
				if(res_outr == null) res_outr = new float [65536];
				if(res_inl == null) res_inl  = new float [4*65536];
				if(res_inr == null) res_inr  = new float [4*65536];

                if (resampPtrIn_l != null)  DttSP.DelResamplerF(resampPtrIn_l);
                resampPtrIn_l = DttSP.NewResamplerF(sample_rate2, sample_rate1);

                if (resampPtrIn_r != null)   DttSP.DelResamplerF(resampPtrIn_r);
                resampPtrIn_r = DttSP.NewResamplerF(sample_rate2, sample_rate1);

                if (resampPtrOut_l != null)  DttSP.DelResamplerF(resampPtrOut_l);
                resampPtrOut_l = DttSP.NewResamplerF(sample_rate1, sample_rate2);

                if (resampPtrOut_r != null) DttSP.DelResamplerF(resampPtrOut_r);
				resampPtrOut_r = DttSP.NewResamplerF(sample_rate1,sample_rate2);

                if (resampPtrOut_T != null) DttSP.DelResamplerF(resampPtrOut_T);  // ke9ns add
                resampPtrOut_T = DttSP.NewResamplerF(sample_rate1, sample_rate2); // ke9ns add
            }
			else
			{
				vac_resample = false;
				if (vac_output_iq)
				{
					if(res_outl == null) res_outl = new float [65536];
					if(res_outr == null) res_outr = new float [65536];
				}
			}
			cs_vac = (void *)0x0;
			cs_vac = Win32.NewCriticalSection();
			if (Win32.InitializeCriticalSectionAndSpinCount(cs_vac, 0x00000080) == 0)
			{
				vac_enabled = false;
				Debug.WriteLine("CriticalSection Failed");
			}
		}

        unsafe private static void InitVAC2()
        {
            int block_size = block_size_vac2;
            if (vac2_output_iq) block_size = block_size1;

            if (rb_vac2OUT_l == null) rb_vac2OUT_l = new RingBufferFloat(2 * 65536);
            rb_vac2OUT_l.Restart(block_size);

            if (rb_vac2OUT_r == null) rb_vac2OUT_r = new RingBufferFloat(2 * 65536);
            rb_vac2OUT_r.Restart(block_size);

            if (rb_vac2IN_l == null) rb_vac2IN_l = new RingBufferFloat(4 * 65536);
            rb_vac2IN_l.Restart(block_size_vac2);

            if (rb_vac2IN_r == null) rb_vac2IN_r = new RingBufferFloat(4 * 65536);
            rb_vac2IN_r.Restart(block_size_vac2);

            if (sample_rate3 != sample_rate1 && !vac2_output_iq)
            {
                vac2_resample = true;
                if (res_vac2_outl == null) res_vac2_outl = new float[65536];
                if (res_vac2_outr == null) res_vac2_outr = new float[65536];
                if (res_vac2_inl == null) res_vac2_inl = new float[4 * 65536];
                if (res_vac2_inr == null) res_vac2_inr = new float[4 * 65536];

                if (resampVAC2PtrIn_l != null)
                    DttSP.DelResamplerF(resampVAC2PtrIn_l);
                resampVAC2PtrIn_l = DttSP.NewResamplerF(sample_rate3, sample_rate1);

                if (resampVAC2PtrIn_r != null)
                    DttSP.DelResamplerF(resampVAC2PtrIn_r);
                resampVAC2PtrIn_r = DttSP.NewResamplerF(sample_rate3, sample_rate1);

                if (resampVAC2PtrOut_l != null)
                    DttSP.DelResamplerF(resampVAC2PtrOut_l);
                resampVAC2PtrOut_l = DttSP.NewResamplerF(sample_rate1, sample_rate3);

                if (resampVAC2PtrOut_r != null)
                    DttSP.DelResamplerF(resampVAC2PtrOut_r);
                resampVAC2PtrOut_r = DttSP.NewResamplerF(sample_rate1, sample_rate3);
            }
            else
            {
                vac2_resample = false;
                if (vac2_output_iq)
                {
                    if (res_vac2_outl == null) res_vac2_outl = new float[65536];
                    if (res_vac2_outr == null) res_vac2_outr = new float[65536];
                }
            }
            cs_vac2 = (void*)0x0;
            cs_vac2 = Win32.NewCriticalSection();
            if (Win32.InitializeCriticalSectionAndSpinCount(cs_vac2, 0x00000080) == 0)
            {
                vac2_enabled = false;
                Debug.WriteLine("CriticalSection Failed");
            }
        }

#endif
		unsafe private static void CleanUpVAC()
		{
			Win32.DeleteCriticalSection(cs_vac);
			rb_vacOUT_l = null;
			rb_vacOUT_r = null;
			rb_vacIN_l = null;
			rb_vacIN_r = null;

			res_outl = null;
			res_outr = null;
			res_inl = null;
			res_inr = null;

            if (resampPtrIn_l != null)
            {
                DttSP.DelResamplerF(resampPtrIn_l);
                resampPtrIn_l = null;
            }

            if (resampPtrIn_r != null)
            {
                DttSP.DelResamplerF(resampPtrIn_r);
                resampPtrIn_r = null;
            }

            if (resampPtrOut_l != null)
            {
                DttSP.DelResamplerF(resampPtrOut_l);
                resampPtrOut_l = null;
            }

            if (resampPtrOut_r != null)
            {
                DttSP.DelResamplerF(resampPtrOut_r);
                resampPtrOut_r = null;
            }

            if (resampPtrOut_T != null)  // ke9ns add
            {
                DttSP.DelResamplerF(resampPtrOut_T);
                resampPtrOut_T = null;
            }

            Win32.DestroyCriticalSection(cs_vac);
		}

        unsafe private static void CleanUpVAC2()
        {
            Win32.DeleteCriticalSection(cs_vac2);
            rb_vac2OUT_l = null;
            rb_vac2OUT_r = null;
            rb_vac2IN_l = null;
            rb_vac2IN_r = null;

            res_vac2_outl = null;
            res_vac2_outr = null;
            res_vac2_inl = null;
            res_vac2_inr = null;

            if (resampVAC2PtrIn_l != null)
            {
                DttSP.DelResamplerF(resampVAC2PtrIn_l);
                resampVAC2PtrIn_l = null;
            }

            if (resampVAC2PtrIn_r != null)
            {
                DttSP.DelResamplerF(resampVAC2PtrIn_r);
                resampVAC2PtrIn_r = null;
            }

            if (resampVAC2PtrOut_l != null)
            {
                DttSP.DelResamplerF(resampVAC2PtrOut_l);
                resampVAC2PtrOut_l = null;
            }

            if (resampVAC2PtrOut_r != null)
            {
                DttSP.DelResamplerF(resampVAC2PtrOut_r);
                resampVAC2PtrOut_r = null;
            }

            Win32.DestroyCriticalSection(cs_vac2);
        }

		unsafe public static double GetCPULoad()
		{
			return PA19.PA_GetStreamCpuLoad(stream1);
		}

		public static ArrayList GetPAHosts() // returns a text list of driver types
		{
			ArrayList a = new ArrayList();
			for(int i=0; i<PA19.PA_GetHostApiCount(); i++)
			{
				PA19.PaHostApiInfo info = PA19.PA_GetHostApiInfo(i);
				a.Add(info.name);
			}
			return a;
		}

		public static ArrayList GetPAInputDevices(int hostIndex)
		{
			ArrayList a = new ArrayList();

			PA19.PaHostApiInfo hostInfo = PA19.PA_GetHostApiInfo(hostIndex);
			for(int i=0; i<hostInfo.deviceCount; i++)
			{
				int devIndex = PA19.PA_HostApiDeviceIndexToDeviceIndex(hostIndex, i);
				PA19.PaDeviceInfo devInfo = PA19.PA_GetDeviceInfo(devIndex);
                if (devInfo.maxInputChannels > 0)
                {
                    string name = devInfo.name;
                    int index = name.IndexOf("- "); // find case for things like "Microphone (2- FLEX-1500)"
                    if (index > 0)
                    {
                        char c = name[index - 1]; // make sure this is what we're looking for
                        if (c >= '0' && c <= '9') // it is... remove index
                        {
                            int x = name.IndexOf("(");
                            name = devInfo.name.Substring(0, x + 1); // grab first part of string including "("
                            name += devInfo.name.Substring(index + 2, devInfo.name.Length - index - 2); // add end of string;
                        }
                    }
                    a.Add(new PADeviceInfo(name, i)/* + " - " + devIndex*/);
                }
			}
			return a;
		}

        public static bool CheckPAInputDevices(int hostIndex, string name)
        {
            PA19.PaHostApiInfo hostInfo = PA19.PA_GetHostApiInfo(hostIndex);
            for (int i = 0; i < hostInfo.deviceCount; i++)
            {
                int devIndex = PA19.PA_HostApiDeviceIndexToDeviceIndex(hostIndex, i);
                PA19.PaDeviceInfo devInfo = PA19.PA_GetDeviceInfo(devIndex);
                if (devInfo.maxInputChannels > 0 && devInfo.name.Contains(name))
                    return true;
            }
            return false;
        }

        public static ArrayList GetPAOutputDevices(int hostIndex)
		{
			ArrayList a = new ArrayList();

			PA19.PaHostApiInfo hostInfo = PA19.PA_GetHostApiInfo(hostIndex);
			for(int i=0; i<hostInfo.deviceCount; i++)
			{
				int devIndex = PA19.PA_HostApiDeviceIndexToDeviceIndex(hostIndex, i);
				PA19.PaDeviceInfo devInfo = PA19.PA_GetDeviceInfo(devIndex);
                if (devInfo.maxOutputChannels > 0)
                {
                    string name = devInfo.name;
                    int index = name.IndexOf("- "); // find case for things like "Microphone (2- FLEX-1500)"
                    if (index > 0)
                    {
                        char c = name[index - 1]; // make sure this is what we're looking for
                        if (c >= '0' && c <= '9') // it is... remove index
                        {
                            int x = name.IndexOf("(");
                            name = devInfo.name.Substring(0, x + 1); // grab first part of string including "("
                            name += devInfo.name.Substring(index + 2, devInfo.name.Length - index - 2); // add end of string;
                        }
                    }
                    a.Add(new PADeviceInfo(name, i)/* + " - " + devIndex*/);
                }
			}
			return a;
		}

        public static bool CheckPAOutputDevices(int hostIndex, string name)
        {
            PA19.PaHostApiInfo hostInfo = PA19.PA_GetHostApiInfo(hostIndex);
            for (int i = 0; i < hostInfo.deviceCount; i++)
            {
                int devIndex = PA19.PA_HostApiDeviceIndexToDeviceIndex(hostIndex, i);
                PA19.PaDeviceInfo devInfo = PA19.PA_GetDeviceInfo(devIndex);
                if (devInfo.maxOutputChannels > 0 && devInfo.name.Contains(name))
                    return true;
            }
            return false;
        }


        //=============================================================================================================
        // ke9ns pick your PA routine (callback8 which is really callback2)
        //=============================================================================================================

        public static bool Start()
		{            
			bool retval = false;
			phase_buf_l = new float[block_size1];
			phase_buf_r = new float[block_size1];

			if(console.fwc_init && (console.CurrentModel == Model.FLEX5000 || console.CurrentModel == Model.FLEX3000))
			{
				switch(console.CurrentModel)
				{
					case Model.FLEX5000:
						in_rx1_l = 0;
						in_rx1_r = 1;
						break;
					case Model.FLEX3000:
						in_rx1_l = 1;
						in_rx1_r = 0;
						break;
				}

				in_rx2_l = 2;
				in_rx2_r = 3;

                /*in_tx_l = 5;
				in_tx_r = 6;*/

                Debug.WriteLine("STARTAUDIO HERE 0");
                retval = StartAudio(ref callback8, (uint)block_size1, sample_rate1, host1, input_dev1, output_dev1, 8, 0, latency1);    // ke9ns use primary input_dev1 device
/*
                unsafe
                {
                    JanusAudio.SetNRx(nr); //set number of receivers
                    JanusAudio.SetDuplex(1); // set full duplex mode

                  //  retval = StartAudio(ref callback3port, (uint)block_size1, sample_rate1);
                    retval = StartAudioJanus(ref callback8, (uint)block_size1, sample_rate1);


                }

*/

            }
            else if (console.hid_init && console.CurrentModel == Model.FLEX1500)
            {
                // do nothing -- handle audio through windriver, not PortAudio (PA)
                retval = true;
            }
            else
			{
                try
                {
                    if (num_channels == 2)
                    {
                        Debug.WriteLine("STARTAUDIO HERE 1");

                        retval = StartAudio(ref callback1, (uint)block_size1, sample_rate1, host1, input_dev1, output_dev1, num_channels, 0, latency1);  // ke9ns use primary input_dev1 device
                    }
                    else if (num_channels == 4)
                        retval = StartAudio(ref callback4port, (uint)block_size1, sample_rate1, host1, input_dev1, output_dev1, num_channels, 0, latency1);
                }
                catch (Exception)
                {
                    MessageBox.Show("The program is having trouble starting the audio streams.\n" +
                        "Please close the program, cycle power to the radio, and try again.",
                        "Audio Stream Startup Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return false;
                }
			}
			
			if(!retval) return retval;

			if(vac_enabled) // ke9ns  VAC1 only
			{
              
                int num_chan = 1;
				// ehr add for multirate iq to vac
				int sample_rate = sample_rate2;
				int block_size = block_size_vac;
				int latency = latency2;
				if (vac_output_iq)
				{
					num_chan = 2;
					sample_rate = sample_rate1;
					block_size = block_size1;
					//latency = 250;
				}
				else if(vac_stereo) num_chan = 2;
				// ehr end				
				vac_rb_reset = true;

                try   // ke9ns 	int new_input = ((PADeviceInfo)comboAudioInput2.SelectedItem).Index;
                {
                    Debug.WriteLine("VAC1 STARTING AUDIO STREAM:: INPUT: " + input_dev2 + ", Output: " + output_dev2 + 
                        ", block size: " + block_size + ", sample rate: " + sample_rate + ", host: " + host2 + ", num_chan " + num_chan + ", latency: " + latency + ",callbackVAC: " + callbackVAC);

                    // host2 = type of audio driver

                    retval = StartAudio(ref callbackVAC, (uint)block_size, sample_rate, host2, input_dev2, output_dev2, num_chan, 1, latency);  // ke9ns use VAC1 input_dev2 device (was 1)

                    Debug.WriteLine("VAC1 STARTING AUDIO STREAM:: RETVAL: " + retval);

                }
                catch (Exception)
                {
                    MessageBox.Show("The program is having trouble starting the VAC1 audio streams.\n" +
                        "Please examine the VAC1 related settings on the Setup Form -> Audio Tab and try again.",
                        "VAC Audio Stream Startup Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return false;
                }
              
            } // ke9ns VAC1 on



            if (vac2_enabled)  // ke9ns VAC2 only
            {
                int num_chan = 1;
                // ehr add for multirate iq to vac
                int sample_rate = sample_rate3;
                int block_size = block_size_vac2;
                int latency = latency3;

                if (vac2_output_iq)
                {
                    num_chan = 2;
                    sample_rate = sample_rate1;
                    block_size = block_size1;
                    //latency = 250;
                }
                else if (vac2_stereo) num_chan = 2;
                // ehr end				
                vac2_rb_reset = true;

                try
                {
                    Debug.WriteLine("VAC2 STARTING AUDIO STREAM:: INPUT: " + input_dev2 + ", Output: " + output_dev2 +
                     ", block size: " + block_size + ", sample rate: " + sample_rate + ", host: " + host2 + ", num_chan " + num_chan + ", latency: " + latency + ",callbackVAC: " + callbackVAC);

                    retval = StartAudio(ref callbackVAC2, (uint)block_size, sample_rate, host3, input_dev3, output_dev3, num_chan, 2, latency);  // ke9ns use VAC2 input_dev3 device

                    Debug.WriteLine("VAC2 STARTING AUDIO STREAM:: RETVAL: " + retval);

                }
                catch (Exception)
                {
                    MessageBox.Show("The program is having trouble starting the VAC2 audio streams.\n" +
                        "Please examine the VAC2 related settings on the Setup Form -> Audio Tab and try again.",
                        "VAC2 Audio Stream Startup Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return false;
                }
            }

			return retval;
		 } // Start()



//=================================================================================================
// ke9ns not called by flex1500
		public unsafe static bool StartAudio(ref PA19.PaStreamCallback callback, uint block_size, double sample_rate, int host_api_index, int input_dev_index,
			int output_dev_index, int num_channels, int callback_num, int latency_ms)
		{
            empty_buffers = 0;

            // input_dev_index = 2; // ke9ns test

            Debug.WriteLine("HOST INDEX " + host_api_index);
            Debug.WriteLine("in_dev index " + input_dev_index);
            Debug.WriteLine("out_dev index " + output_dev_index);

            int in_dev = PA19.PA_HostApiDeviceIndexToDeviceIndex(host_api_index, input_dev_index);
            int out_dev = PA19.PA_HostApiDeviceIndexToDeviceIndex(host_api_index, output_dev_index);

            Debug.WriteLine("in_dev " + in_dev);
            Debug.WriteLine("out_dev " + out_dev);


            var inparam = new PA19.PaStreamParameters();
			var outparam = new PA19.PaStreamParameters();
			
          
                	
			inparam.device = in_dev;
			inparam.channelCount = num_channels;
#if (INTERLEAVED)
			inparam.sampleFormat = PA19.paFloat32;
#else
			inparam.sampleFormat = PA19.paFloat32 | PA19.paNonInterleaved;
#endif
			inparam.suggestedLatency = ((float)latency_ms/1000);

			outparam.device = out_dev;
			outparam.channelCount = num_channels;
#if (INTERLEAVED)
			outparam.sampleFormat = PA19.paFloat32;
#else
			outparam.sampleFormat = PA19.paFloat32 | PA19.paNonInterleaved;
#endif
			outparam.suggestedLatency = ((float)latency_ms/1000);
			
			int error = 0;
           
            int RETRY_AUDIO_START_TIMES = (console.CurrentModel == Model.FLEX1500 ? 25 : 1);

           
            int i;
           
            for (i = 0; i < RETRY_AUDIO_START_TIMES; i++)
            {
             
                switch (callback_num) // ke9ns 1=vac1
                {
                    case 1: // VAC1
                        error = PA19.PA_OpenStream(out stream2, &inparam, &outparam, sample_rate, block_size, 0, callback, 1);
                        Debug.WriteLine("VAC1CALL===== " + error );
                        break;
                    case 2: //VAC2
                        
                        error = PA19.PA_OpenStream(out stream3, &inparam, &outparam, sample_rate, block_size, 0, callback, 2);
                        Debug.WriteLine("VAC2CALL===== " + error);
                        break;
                    default:
                        error = PA19.PA_OpenStream(out stream1, &inparam, &outparam, sample_rate, block_size, 0, callback, 0); // ke9ns this is the default stream  callback = callback8 (which is callback2)
                        Debug.WriteLine("startcallback 0===== " + error);
                        break;
                }
               
             
                if (error == 0) // ke9ns 0=good
                {
                   break; // stop if no error
                }
      

            } // for loop

        
            if (console.CurrentModel == Model.FLEX1500) Debug.WriteLine("audio start retry = "+ i +" times");

          
           
            if (error != 0)
			{
                Debug.WriteLine("startaudio1() " + error);
                PortAudioErrorMessageBox(error);
				return false;
			}
        
            switch (callback_num)
            {
                
                case 1:
                    error = PA19.PA_StartStream(stream2);                    
                    break;
                case 2:
                    error = PA19.PA_StartStream(stream3);
                    break;
                default:
                    error = PA19.PA_StartStream(stream1);
                    break;
            }
            /*
                        if(callback_num == 0)
                            error = PA19.PA_StartStream(stream1);
                        else
                            error = PA19.PA_StartStream(stream2);
            */

            if (error != 0)
			{
                Debug.WriteLine("startaudio");
                PortAudioErrorMessageBox(error);
				return false;
			}
			return true;
		} // StartAudio



       //=================================================================================
        private static void PortAudioErrorMessageBox(int error)
        {
            switch (error)
            {
                case (int)PA19.PaErrorCode.paInvalidDevice:
                    string s = "Whoops!  Looks like something has gone wrong in the Audio section.\n" +
                        "Go look in the Setup Form -> Audio Tab to verify the settings there.";

                    if (vac_enabled) s += "  Since VAC is enabled, make sure you look at those settings as well.";

                    MessageBox.Show(s, "Audio Subsystem Error: Invalid Device",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    break;

                default:
                    MessageBox.Show(PA19.PA_GetErrorText(error), "PortAudio Error: "+error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }
        } //  PortAudioErrorMessageBox



        public unsafe static void StopAudio()
		{
            int error = 0;
            Debug.WriteLine("STOPAUDIO"); // when windows kms started on vac1

            PA19.PA_AbortStream(stream1);
            error = PA19.PA_CloseStream(stream1);
            if (error != 0) PortAudioErrorMessageBox(error);
		}

		public unsafe static void StopAudioVAC()
		{
            int error = 0;
            Debug.WriteLine("StopAudioVAC");
            PA19.PA_AbortStream(stream2);
            error = PA19.PA_CloseStream(stream2);
          
            if (error != 0) PortAudioErrorMessageBox(error);
		}

        public unsafe static void StopAudioVAC2()
        {
            int error = 0;
            Debug.WriteLine("StopAudioVAC2");

            PA19.PA_AbortStream(stream3);
            error = PA19.PA_CloseStream(stream3);
            if (error != 0) PortAudioErrorMessageBox(error);
        }

#endregion

#region Scope Stuff

		private static int scope_samples_per_pixel = 512;
		public static int ScopeSamplesPerPixel
		{
			get { return scope_samples_per_pixel; }
			set { scope_samples_per_pixel = value; }
		}

		private static int scope_display_width = 704;
		public static int ScopeDisplayWidth
		{
			get { return scope_display_width; }
			set { scope_display_width = value; }
		}

		private static int scope_sample_index = 0;
		private static int scope_pixel_index = 0;
		private static float scope_pixel_min = float.MaxValue;
		private static float scope_pixel_max = float.MinValue;

        private static float[] scope_min;

		public static float[] ScopeMin
		{
			set { scope_min = value; }
		}
		public static float[] scope_max;
		public static float[] ScopeMax
		{
			set { scope_max = value; }
		}


        //======================================================================================
        // ke9ns this is for scope and Panascope only (framecount on a flex5000 = 2048)
        //======================================================================================
        unsafe private static void DoScope(float* buf, int frameCount)
		{

            //  if (Display.CurrentDisplayMode != DisplayMode.SCOPE && Display.CurrentDisplayMode != DisplayMode.PANASCOPE) return; // ke9ns add  no need to do scope if your not using it.
        //   Debug.WriteLine("DOSCOPE HERE" + frameCount);

            if (scope_min == null || scope_min.Length < scope_display_width) // ke9ns check for screen size change
            {
                
                if (Display.ScopeMin == null || Display.ScopeMin.Length < scope_display_width)
                {
                    Display.ScopeMin = new float[scope_display_width];
                }

				scope_min = Display.ScopeMin; 
			}

			if(scope_max == null || scope_max.Length < scope_display_width)  // ke9ns check for screen size change
			{
                if (Display.ScopeMax == null || Display.ScopeMax.Length < scope_display_width)
                {
                    Display.ScopeMax = new float[scope_display_width];
                }

				scope_max = Display.ScopeMax;
			}

			for(int i=0; i < frameCount; i++) // ke9ns 0 to 2048
			{
				if(buf[i] < scope_pixel_min) scope_pixel_min = buf[i]; // ke9ns find the mininum value of this frame

				if(buf[i] > scope_pixel_max) scope_pixel_max = buf[i]; // ke9ns find the max value of this frame

				scope_sample_index++;

				if(scope_sample_index >= scope_samples_per_pixel)
				{
					scope_sample_index = 0;

					scope_min[scope_pixel_index] = scope_pixel_min;  // ke9ns fill array with scope min max fa

					scope_max[scope_pixel_index] = scope_pixel_max;

					scope_pixel_min = float.MaxValue;
					scope_pixel_max = float.MinValue;

					scope_pixel_index++;

					if(scope_pixel_index >= scope_display_width) scope_pixel_index = 0;
				}
			}

		} // doscope

#endregion

	} // audio class

} // powersdr
