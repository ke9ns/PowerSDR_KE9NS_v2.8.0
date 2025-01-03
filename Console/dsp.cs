//=================================================================
// dsp.cs
// ke9ns:  DSP.cs is used to pass RX/TX DSP data between C# forms, and update.c
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


using EnvDTE;
using Microsoft.JScript;
using Microsoft.VisualBasic.Logging;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

namespace PowerSDR
{
    #region DSP Class 

    public sealed class DSP
    {
        private const int NUM_RX_THREADS = 2;
        private const int NUM_RX_PER_THREAD = 2;
        private DSPRX[][] dsp_rx;
        private DSPTX[] dsp_tx;

        public DSP()
        {
            Debug.WriteLine("START DSP  HERE==========================");

            CreateDSP();
            System.Threading.Thread.Sleep(100);
            DttSP.ReleaseUpdate();

            dsp_rx = new DSPRX[NUM_RX_THREADS][];
            for (int i = 0; i < NUM_RX_THREADS; i++)
            {
                dsp_rx[i] = new DSPRX[NUM_RX_PER_THREAD];

                for (int j = 0; j < NUM_RX_PER_THREAD; j++)
                    dsp_rx[i][j] = new DSPRX((uint)i * 2, (uint)j);
            }

            dsp_tx = new DSPTX[1];
            dsp_tx[0] = new DSPTX(1);

            dsp_rx[0][0].Active = true; // enable main RX
                                        //dsp_tx[0].Active = true; // enable main TX

            // set for half duplex
            DttSP.SetThreadProcessingMode(0, 2);
            DttSP.SetThreadProcessingMode(1, 1);
        }

        public DSPRX GetDSPRX(int thread, int subrx)
        {

            return dsp_rx[thread][subrx];
        }

        public DSPTX GetDSPTX(int thread)
        {
            return dsp_tx[thread];
        }

        public static void SyncStatic() // ke9ns: called from setup
        {
            DSP.SampleRate = DSP.SampleRate; //double sample_rate
        }

        public static void SyncHiRes(int hires1) // ke9ns: called from setup (hires)
        {
            hires = hires1;
            DSP.HiResPan = DSP.HiResPan;

            //   Debug.WriteLine("SyncHIRes HERE " + hires);


        }

        #region Static Properties and Routines

        public void CreateDSP()
        {
           // System.String app_data_path = "";
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            System.String version = fvi.FileVersion.Substring(0, fvi.FileVersion.LastIndexOf("."));
            System.String app_data_path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\FlexRadio Systems\\PowerSDR v" + version + "\\wisdom";

            DttSP.SetupSDR(app_data_path);
        }

        public static void DestroyDSP()
        {
            DttSP.Exit();
        }

        private static double sample_rate = 48000.0;
        public static double SampleRate
        {
            get { return sample_rate; }
            set
            {
                sample_rate = value;
                DttSP.SetSampleRate(value);
            }
        }


        // ke9ns add
        private static int hires = 4096;
        public static int HiResPan
        {
            get { return hires; }
            set
            {
                hires = value;
                DttSP.SetHiResPan(value);
            }
        }


        public static void SetThreadNumber(uint num)
        {
            DttSP.SetThreadNo(num);
            //  Debug.WriteLine("THREAD NUMBER ======================" + num);

        }

        public static void SetThreadCom(uint num)
        {
            DttSP.SetThreadCom(num);
        }

        #endregion
    }

    #endregion

    #region DSPRX Class

    public sealed class DSPRX
    {
        private uint thread;
        private uint subrx;

        //=====================================================================================
        public DSPRX(uint t, uint rx)
        {
            thread = t;
            subrx = rx;
            DttSP.SetTRX(t, false);
        }

        //=====================================================================================
        public void Copy(DSPRX rx)
        {
            this.AudioSize = rx.audio_size;
            this.DSPMode = rx.dsp_mode;
            this.SetRXFilter(rx.rx_filter_low, rx.rx_filter_high);
            this.NoiseReduction = rx.noise_reduction;
            this.SetNRVals(rx.nr_taps, rx.nr_delay, rx.nr_gain, rx.nr_leak);
            this.AutoNotchFilter = rx.auto_notch_filter;
            this.SetANFVals(rx.anf_taps, rx.anf_delay, rx.anf_gain, rx.anf_leak);
            this.RXAGCMode = rx.rx_agc_mode;
            this.RXEQNumBands = rx_eq_num_bands;
            if (this.rx_eq_num_bands == 3)
            {
                this.RXEQ10 = rx.rx_eq10;
                this.RXEQ3 = rx.rx_eq3;
            }
            else
            {
                this.RXEQ3 = rx.rx_eq3;
                this.RXEQ10 = rx.rx_eq10;
            }
            this.RXEQOn = rx.rx_eq_on;
            this.NBOn = rx.nb_on;
            this.NBThreshold = rx.nb_threshold;
            this.RXCorrectIQMu = rx.rx_correct_iq_mu;
            this.SDROM = rx.sdrom;
            this.SDROMThreshold = rx.sdrom_threshold;
            this.RXFixedAGC = rx.rx_fixed_agc;
            this.RXAGCMaxGain = rx.rx_agc_max_gain;
            this.RXAGCAttack = rx.rx_agc_attack;
            this.RXAGCDecay = rx.rx_agc_decay;
            this.RXAGCHang = rx.rx_agc_hang;
            this.RXOutputGain = rx.rx_output_gain;
            this.RXAGCSlope = rx.rx_agc_slope;
            this.RXAGCHangThreshold = rx.rx_agc_hang_threshold;
            this.CurrentWindow = rx.current_window;
            this.SpectrumPolyphase = rx.spectrum_polyphase;
            this.BinOn = rx.bin_on;
            this.RXSquelchThreshold = rx.rx_squelch_threshold;
            this.FMSquelchThreshold = rx.fm_squelch_threshold;
            this.RXSquelchOn = rx.rx_squelch_on;
            this.SpectrumPreFilter = rx.spectrum_pre_filter;
            this.Active = rx.active;
            this.Pan = rx.pan;
            this.RXOsc = rx.rx_osc;
            this.DCBlock = rx.dc_block;
            this.RXFMDeviation = rx.rx_fm_deviation;




        } // copy(DSPRX )

        //=====================================================================================
        private void SyncAll()
        {
            //BufferSize = buffer_size;
            SetRXCorrectIQW(rx_correct_iq_w_real, rx_correct_iq_w_imag);
            AudioSize = audio_size;
            DSPMode = dsp_mode;
            SetRXFilter(rx_filter_low, rx_filter_high);
            NoiseReduction = noise_reduction;
            SetNRVals(nr_taps, nr_delay, nr_gain, nr_leak);
            AutoNotchFilter = auto_notch_filter;
            SetANFVals(anf_taps, anf_delay, anf_gain, anf_leak);
            RXAGCMode = rx_agc_mode;
            if (rx_eq_num_bands == 3)
            {
                RXEQ10 = rx_eq10;
                RXEQ3 = rx_eq3;
            }
            else
            {
                RXEQ3 = rx_eq3;
                RXEQ10 = rx_eq10;
            }
            RXEQOn = rx_eq_on;
            NBOn = nb_on;
            NBThreshold = nb_threshold;
            RXCorrectIQMu = rx_correct_iq_mu;
            SDROM = sdrom;
            SDROMThreshold = sdrom_threshold;
            RXFixedAGC = rx_fixed_agc;
            RXAGCMaxGain = rx_agc_max_gain;
            RXAGCAttack = rx_agc_attack;
            RXAGCDecay = rx_agc_decay;
            RXAGCHang = rx_agc_hang;
            RXOutputGain = rx_output_gain;
            RXAGCSlope = rx_agc_slope;
            RXAGCHangThreshold = rx_agc_hang_threshold;
            CurrentWindow = current_window;
            SpectrumPolyphase = spectrum_polyphase;
            BinOn = bin_on;
            RXSquelchThreshold = rx_squelch_threshold;
            RXSquelchOn = rx_squelch_on;
            FMSquelchThreshold = fm_squelch_threshold;
            SpectrumPreFilter = spectrum_pre_filter;
            Active = active;
            Pan = pan;
            RXOsc = rx_osc;
            DCBlock = dc_block;
            RXFMDeviation = rx_fm_deviation;

            for (uint i = 0; i < 9; i++)
                SetNotchOn(i, notch_on[i]);
        }

        #region Non-Static Properties & Routines

        //=====================================================================================
        /// <summary>
        /// Controls whether updates to following properties call the DSP.  
        /// Each property uses this value and a copy of the last thing sent to
        /// the DSP to update in a minimal fashion.
        /// </summary>
        private bool update = false;
        public bool Update
        {
            get { return update; }
            set
            {
                update = value;
                if (value) SyncAll();
            }
        }

        //=====================================================================================
        /// <summary>
        /// Used to force properties to update even if the DSP copy matches the
        /// new setting.  Mainly used to resync the DSP after having to rebuild
        /// when resetting DSP Block Size or Sample Rate.
        /// </summary>
        private bool force = false;
        public bool Force
        {
            get { return force; }
            set { force = value; }
        }


        //================================================================================= DSPRX Class
        private int buffer_size_dsp = 2048;
        private int buffer_size = 2048;
        public int BufferSize
        {
            get { return buffer_size; }
            set
            {
                buffer_size = value;
                if (update)
                {
                    if (value != buffer_size_dsp || force)
                    {
                        //Debug.WriteLine("RX "+thread+":"+subrx+" "+value);
                        DttSP.ResizeSDR(thread, value);  // ke9ns calls setdspbuflen in update.c
                        buffer_size_dsp = value;
                        //SyncAll();
                    }
                }
            }
        } // buffersize


        //=====================================================================================
        private int audio_size_dsp = 2048;
        private int audio_size = 2048;
        public int AudioSize
        {
            get { return audio_size; }
            set
            {
                audio_size = value;
                if (update)
                {
                    if (value != audio_size_dsp || force)
                    {
                        DttSP.SetAudioSize(value);
                        audio_size_dsp = value;
                    }
                }
            }
        }

        //=====================================================================================
        private DSPMode dsp_mode_dsp = DSPMode.USB;
        private DSPMode dsp_mode = DSPMode.USB;
        public DSPMode DSPMode
        {
            get { return dsp_mode; }
            set
            {
                dsp_mode = value;
                if (update)
                {
                    if (value != dsp_mode_dsp || force)
                    {
                        DttSP.SetMode(thread, subrx, value);
                        dsp_mode_dsp = value;
                    }
                }
            }
        }

        //=====================================================================================
        public void SetRXFilter(int low, int high)
        {
            //Debug.WriteLine("SetRXFilter[" + thread + "][" + subrx + " ](" + low + ", " + high + ") ");
            rx_filter_low = low;
            rx_filter_high = high;
            if (update)
            {
                if (low != rx_filter_low_dsp || high != rx_filter_high_dsp || force)
                {
                    DttSP.SetRXFilter(thread, subrx, low, high);
                    rx_filter_low_dsp = low;
                    rx_filter_high_dsp = high;
                }
            }
        }

        //=====================================================================================
        private int rx_filter_low_dsp;
        private int rx_filter_low;
        public int RXFilterLow
        {
            get { return rx_filter_low; }
            set
            {
                rx_filter_low = value;
                if (update)
                {
                    if (value != rx_filter_low_dsp || force)
                    {
                        DttSP.SetRXFilter(thread, subrx, value, rx_filter_high);
                        rx_filter_low_dsp = value;
                    }
                }
            }
        }

        //=====================================================================================
        private int rx_filter_high_dsp;
        private int rx_filter_high;
        public int RXFilterHigh
        {
            get { return rx_filter_high; }
            set
            {
                rx_filter_high = value;
                if (update)
                {
                    if (value != rx_filter_high_dsp || force)
                    {
                        DttSP.SetRXFilter(thread, subrx, rx_filter_low, value);
                        rx_filter_high_dsp = value;
                    }
                }
            }
        }

        //=====================================================================================
        private bool noise_reduction_dsp = false;
        private bool noise_reduction = false;
        public bool NoiseReduction
        {
            get { return noise_reduction; }
            set
            {
                noise_reduction = value;
                if (update)
                {
                    if (value != noise_reduction_dsp || force)
                    {
                        DttSP.SetNR(thread, subrx, value);
                        noise_reduction_dsp = value;
                    }
                }
            }
        }

        //=====================================================================================
        private int nr_taps_dsp = 64;
        private int nr_taps = 64;
        private int nr_delay_dsp = 64;
        private int nr_delay = 64;
        private double nr_gain_dsp = 16e-4;
        private double nr_gain = 16e-4;
        private double nr_leak_dsp = 10e-7;
        private double nr_leak = 10e-7;
        public void SetNRVals(int taps, int delay, double gain, double leak)
        {
            nr_taps = taps;
            nr_delay = delay;
            nr_gain = gain;
            nr_leak = leak;
            if (update)
            {
                if (taps != nr_taps_dsp || delay != nr_delay_dsp ||
                    gain != nr_gain_dsp || leak != nr_leak_dsp || force)
                {
                    DttSP.SetNRvals(thread, subrx, taps, delay, gain, leak);
                    nr_taps_dsp = taps;
                    nr_delay_dsp = delay;
                    nr_gain_dsp = gain;
                    nr_leak_dsp = leak;
                }
            }
        }

        private bool auto_notch_filter_dsp = false;
        private bool auto_notch_filter = false;
        public bool AutoNotchFilter
        {
            get { return auto_notch_filter; }
            set
            {
                auto_notch_filter = value;
                if (update)
                {
                    if (value != auto_notch_filter_dsp || force)
                    {
                        DttSP.SetANF(thread, subrx, value);
                        auto_notch_filter_dsp = value;
                    }
                }
            }
        }

        //=====================================================================================
        private int anf_taps_dsp = 64;
        private int anf_taps = 64;
        private int anf_delay_dsp = 64;
        private int anf_delay = 64;
        private double anf_gain_dsp = 10e-4;
        private double anf_gain = 10e-4;
        private double anf_leak_dsp = 1e-7;
        private double anf_leak = 1e-7;
        public void SetANFVals(int taps, int delay, double gain, double leak)
        {
            anf_taps = taps;
            anf_delay = delay;
            anf_gain = gain;
            anf_leak = leak;
            if (update)
            {
                if (taps != anf_taps_dsp || delay != anf_delay_dsp ||
                    gain != anf_gain_dsp || leak != anf_leak_dsp || force)
                {
                    DttSP.SetANFvals(thread, subrx, taps, delay, gain, leak);
                    anf_taps_dsp = taps;
                    anf_delay_dsp = delay;
                    anf_gain_dsp = gain;
                    anf_leak_dsp = leak;
                }
            }
        }

        //=====================================================================================
        private AGCMode rx_agc_mode_dsp = AGCMode.MED;
        private AGCMode rx_agc_mode = AGCMode.MED;
        public AGCMode RXAGCMode
        {
            get { return rx_agc_mode; }
            set
            {
                rx_agc_mode = value;
                switch (rx_agc_mode)
                {
                    case AGCMode.LONG:
                        rx_agc_attack = rx_agc_attack_dsp = 2;
                        rx_agc_hang = rx_agc_hang_dsp = 750;
                        rx_agc_decay = rx_agc_decay_dsp = 2000;
                        break;
                    case AGCMode.SLOW:
                        rx_agc_attack = rx_agc_attack_dsp = 2;
                        rx_agc_hang = rx_agc_hang_dsp = 500;
                        rx_agc_decay = rx_agc_decay_dsp = 500;
                        break;
                    case AGCMode.MED:
                        rx_agc_attack = rx_agc_attack_dsp = 2;
                        rx_agc_hang = rx_agc_hang_dsp = 250;
                        rx_agc_decay = rx_agc_decay_dsp = 250;
                        break;
                    case AGCMode.FAST:
                        rx_agc_attack = rx_agc_attack_dsp = 2;
                        rx_agc_hang = rx_agc_hang_dsp = 100;
                        rx_agc_decay = rx_agc_decay_dsp = 100;
                        break;
                    case AGCMode.FIXD:
                    case AGCMode.FIRST:
                    case AGCMode.CUSTOM:
                    case AGCMode.LAST:

                        break;

                    default:
                       
                        break;
                }


                if (update)
                {
                    if (value != rx_agc_mode_dsp || force)
                    {
                        DttSP.SetRXAGC(thread, subrx, value);
                        rx_agc_mode_dsp = value;
                    }
                }
            }
        }

        //=====================================================================================
        private int rx_eq_num_bands = 3;
        public int RXEQNumBands
        {
            get { return rx_eq_num_bands; }
            set { rx_eq_num_bands = value; }
        }

        //=====================================================================================
        private int[] rx_eq3_dsp = new int[4];
        private int[] rx_eq3 = new int[4];
        public int[] RXEQ3
        {
            get { return rx_eq3; }
            set
            {
                for (int i = 0; i < rx_eq3.Length && i < value.Length; i++)
                    rx_eq3[i] = value[i];
                if (update)
                {

                    DttSP.SetGrphRXEQ(thread, subrx, rx_eq3);
                    for (int i = 0; i < rx_eq3_dsp.Length && i < value.Length; i++)
                        rx_eq3_dsp[i] = value[i];

                }
            }
        }

        //=====================================================================================
        private int[] rx_eq10_dsp = new int[11];
        private int[] rx_eq10 = new int[11];
        public int[] RXEQ10
        {
            get { return rx_eq10; }
            set
            {
                for (int i = 0; i < rx_eq10.Length && i < value.Length; i++)
                    rx_eq10[i] = value[i];
                if (update)
                {
                    DttSP.SetGrphRXEQ10(thread, subrx, rx_eq10);
                    for (int i = 0; i < rx_eq10_dsp.Length && i < value.Length; i++)
                        rx_eq10_dsp[i] = value[i];

                }
            }
        }

        //=====================================================================================
        private bool rx_eq_on_dsp = false;
        private bool rx_eq_on = false;
        public bool RXEQOn
        {
            get { return rx_eq_on; }
            set
            {
                rx_eq_on = value;
                if (update)
                {
                    if (value != rx_eq_on_dsp || force)
                    {
                        DttSP.SetGrphRXEQcmd(thread, subrx, value);
                        rx_eq_on_dsp = value;
                    }
                }
            }
        }

        //=====================================================================================
        private bool nb_on_dsp = false;
        private bool nb_on = false;
        public bool NBOn
        {
            get { return nb_on; }
            set
            {
                nb_on = value;
                if (update)
                {
                    if (value != nb_on_dsp || force)
                    {
                        DttSP.SetNB(thread, subrx, value);
                        nb_on_dsp = value;
                    }
                }
            }
        }

        //=====================================================================================
        private double nb_threshold_dsp = 3.3;
        private double nb_threshold = 3.3;
        public double NBThreshold
        {
            get { return nb_threshold; }
            set
            {
                nb_threshold = value;
                if (update)
                {
                    if (value != nb_threshold_dsp || force)
                    {
                        nb_threshold_dsp = value;
                        DttSP.SetNBvals(thread, subrx, nb_threshold_dsp, nbht_dsp, nbdly_dsp);

                    }
                }
            }
        } // NBThreshold

        private int nbht = 7;
        private int nbht_dsp = 7;

        public int NBHT
        {
            get
            {
                return nbht;

            }
            set
            {
                nbht = value;
                if (update)
                {
                    if (value != nbht_dsp || force)
                    {
                        // Debug.WriteLine("NBHT : " + value);

                        nbht_dsp = value;
                        DttSP.SetNBvals(thread, subrx, nb_threshold_dsp, nbht_dsp, nbdly_dsp);

                    }
                }
            }

        } // NBHT

        private int nbdly = 8;
        private int nbdly_dsp = 8;

        public int NBDLY
        {
            get
            {
                return nbdly;

            }
            set
            {
                nbdly = value;
                if (update)
                {
                    if (value != nbdly_dsp || force)
                    {
                        nbdly_dsp = value;
                        DttSP.SetNBvals(thread, subrx, nb_threshold_dsp, nbht_dsp, nbdly_dsp);

                    }
                }
            }

        } // NBDLY

        //=====================================================================================
        private float rx_correct_iq_w_real_dsp = 0.0f;
        private float rx_correct_iq_w_real = 0.0f;
        public float RXCorrectIQWReal
        {
            get { return rx_correct_iq_w_real; }
        }

        //=====================================================================================
        private float rx_correct_iq_w_imag_dsp = 0.0f;
        private float rx_correct_iq_w_imag = 0.0f;
        public float RXCorrectIQWImag
        {
            get { return rx_correct_iq_w_imag; }
        }

        //=====================================================================================
        public void SetRXCorrectIQW(float real, float imag)
        {
            //Debug.WriteLine("RX IQ Gain "+thread+" "+subrx+": "+value.ToString("f4"));
            rx_correct_iq_w_real = real;
            rx_correct_iq_w_imag = imag;

            if (update)
            {
                if ((real != rx_correct_iq_w_real_dsp) ||
                    (imag != rx_correct_iq_w_imag_dsp) || force)
                {
                    DttSP.SetCorrectRXIQw(thread, subrx, real, imag, 0);
                    rx_correct_iq_w_real_dsp = real;
                    rx_correct_iq_w_imag_dsp = imag;
                }
            }
        }

        //=====================================================================================
        private double rx_correct_iq_mu_dsp = 0.005;
        private double rx_correct_iq_mu = 0.005;
        public double RXCorrectIQMu
        {
            get { return rx_correct_iq_mu; }
            set
            {
                //Debug.WriteLine("RX WBIR Mu "+thread+" "+subrx+": "+value.ToString("f4"));
                rx_correct_iq_mu = value;
                if (update)
                {
                    if (value != rx_correct_iq_mu_dsp || force)
                    {
                        DttSP.SetCorrectIQMu(thread, subrx, value);
                        rx_correct_iq_mu_dsp = value;
                    }
                }
            }
        }

        //=====================================================================================
        private bool sdrom_dsp = false;
        private bool sdrom = false;
        public bool SDROM
        {
            get { return sdrom; }
            set
            {
                sdrom = value;
                if (update)
                {
                    if (value != sdrom_dsp || force)
                    {
                        DttSP.SetSDROM(thread, subrx, value);
                        sdrom_dsp = value;
                    }
                }
            }
        }

        //=====================================================================================
        private double sdrom_threshold_dsp = 2.475;
        private double sdrom_threshold = 2.475;
        public double SDROMThreshold
        {
            get { return sdrom_threshold; }
            set
            {
                sdrom_threshold = value;
                if (update)
                {
                    if (value != sdrom_threshold_dsp || force)
                    {
                        DttSP.SetSDROMvals(thread, subrx, value);
                        sdrom_threshold_dsp = value;
                    }
                }
            }
        }

        //=====================================================================================
        private double rx_fixed_agc_dsp = 20.0;
        private double rx_fixed_agc = 20.0;
        public double RXFixedAGC
        {
            get { return rx_fixed_agc; }
            set
            {
                rx_fixed_agc = value;
                if (update)
                {
                    if (value != rx_fixed_agc_dsp || force)
                    {
                        DttSP.SetFixedAGC(thread, subrx, value);
                        rx_fixed_agc_dsp = value;
                    }
                }
            }
        }

        //=====================================================================================
        private double rx_agc_max_gain_dsp = 90.0;
        private double rx_agc_max_gain = 90.0;
        public double RXAGCMaxGain
        {
            get { return rx_agc_max_gain; }
            set
            {
                rx_agc_max_gain = value;
                if (update)
                {
                    if (value != rx_agc_max_gain_dsp || force)
                    {
                        DttSP.SetRXAGCMaxGain(thread, subrx, value);
                        rx_agc_max_gain_dsp = value;
                    }
                }
            }
        }

        //=====================================================================================
        private int rx_agc_attack_dsp = 2;
        private int rx_agc_attack = 2;
        public int RXAGCAttack
        {
            get { return rx_agc_attack; }
            set
            {
                rx_agc_attack = value;
                if (update)
                {
                    if (value != rx_agc_attack_dsp || force)
                    {
                        DttSP.SetRXAGCAttack(thread, subrx, value);
                        rx_agc_attack_dsp = value;
                    }
                }
            }
        }

        //=====================================================================================
        private int rx_agc_decay_dsp = 250;
        private int rx_agc_decay = 250;
        public int RXAGCDecay
        {
            get { return rx_agc_decay; }
            set
            {
                rx_agc_decay = value;
                if (update)
                {
                    if (value != rx_agc_decay_dsp || force)
                    {
                        DttSP.SetRXAGCDecay(thread, subrx, value);
                        rx_agc_decay_dsp = value;
                    }
                }
            }
        }

        //=====================================================================================
        private int rx_agc_hang_dsp = 250;
        private int rx_agc_hang = 250;
        public int RXAGCHang
        {
            get { return rx_agc_hang; }
            set
            {
                rx_agc_hang = value;
                if (update)
                {
                    if (value != rx_agc_hang_dsp || force)
                    {
                        DttSP.SetRXAGCHang(thread, subrx, value);
                        rx_agc_hang_dsp = value;
                    }
                }
            }
        }

        //=====================================================================================
        private double rx_output_gain_dsp = 0.5;
        private double rx_output_gain = 0.5;
        public double RXOutputGain
        {
            get { return rx_output_gain; }
            set
            {
                rx_output_gain = value;
                if (update)
                {
                    if (value != rx_output_gain_dsp || force)
                    {
                        DttSP.SetRXOutputGain(thread, subrx, value); // value top = .5  bottom  = 0
                        rx_output_gain_dsp = value;
                    }
                }
            }
        }

        //=====================================================================================
        private int rx_agc_slope_dsp = 0;
        private int rx_agc_slope = 0;
        public int RXAGCSlope
        {
            get { return rx_agc_slope; }
            set
            {
                rx_agc_slope = value;
                if (update)
                {
                    if (value != rx_agc_slope_dsp || force)
                    {
                        DttSP.SetRXAGCSlope(thread, subrx, value);
                        rx_agc_slope_dsp = value;
                    }
                }
            }
        }

        //=====================================================================================
        private int rx_agc_hang_threshold_dsp = 0;
        private int rx_agc_hang_threshold = 0;
        public int RXAGCHangThreshold
        {
            get { return rx_agc_hang_threshold; }
            set
            {
                rx_agc_hang_threshold = value;
                if (update)
                {
                    if (value != rx_agc_hang_threshold_dsp || force)
                    {
                        DttSP.SetRXAGCHangThreshold(thread, subrx, value);
                        rx_agc_hang_threshold_dsp = value;
                    }
                }
            }
        }

        //=====================================================================================
        private Window current_window_dsp = Window.BLKHARRIS;
        private Window current_window = Window.BLKHARRIS;
        public Window CurrentWindow
        {
            get { return current_window; }
            set
            {
                current_window = value;
                if (update)
                {
                    if (value != current_window_dsp || force)
                    {
                        DttSP.SetWindow(thread, value);
                        current_window_dsp = value;
                    }
                }
            }
        }

        //=====================================================================================
        private bool spectrum_polyphase_dsp = false;
        private bool spectrum_polyphase = false;
        public bool SpectrumPolyphase
        {
            get { return spectrum_polyphase; }
            set
            {
                spectrum_polyphase = value;
                if (update)
                {
                    if (value != spectrum_polyphase_dsp || force)
                    {
                        DttSP.SetSpectrumPolyphase(thread, value);
                        spectrum_polyphase_dsp = value;
                    }
                }
            }
        }



        //=====================================================================================
        private bool bin_on_dsp = false;
        private bool bin_on = false;
        public bool BinOn
        {
            get { return bin_on; }
            set
            {
                bin_on = value;
                if (update)
                {
                    if (value != bin_on_dsp || force)
                    {
                        DttSP.SetBIN(thread, subrx, value);
                        bin_on_dsp = value;
                    }
                }
            }
        }

        //=====================================================================================
        private float rx_squelch_threshold_dsp = -150.0f;
        private float rx_squelch_threshold = -150.0f;
        public float RXSquelchThreshold
        {
            get { return rx_squelch_threshold; }
            set
            {
                //Debug.WriteLine("Squelch Threshold: "+value);
                rx_squelch_threshold = value;
                if (update)
                {
                    if (value != rx_squelch_threshold_dsp || force)
                    {
                        DttSP.SetSquelchVal(thread, subrx, value);
                        rx_squelch_threshold_dsp = value;
                    }
                }
            }
        }

        //=====================================================================================
        private float fm_squelch_threshold = 1.0f;
        private float fm_squelch_threshold_dsp = 1.0f;
        public float FMSquelchThreshold
        {
            get { return fm_squelch_threshold; }
            set
            {
                fm_squelch_threshold = value;
                if (update)
                    if (value != fm_squelch_threshold_dsp || force)
                    {
                        {
                            DttSP.SetFMSquelchThreshold(thread, subrx, value);
                            fm_squelch_threshold_dsp = value;
                        }
                    }
            }
        }



        //=====================================================================================
        private bool rx_squelch_on_dsp = false;
        private bool rx_squelch_on = false;
        public bool RXSquelchOn
        {
            get { return rx_squelch_on; }
            set
            {
                rx_squelch_on = value;
                if (update)
                {
                    if (value != rx_squelch_on_dsp || force)
                    {
                        DttSP.SetSquelchState(thread, subrx, value);
                        rx_squelch_on_dsp = value;
                    }
                }
            }
        }

        //=====================================================================================
        private bool spectrum_pre_filter_dsp = true;
        private bool spectrum_pre_filter = true;
        public bool SpectrumPreFilter
        {
            get { return spectrum_pre_filter; }
            set
            {
                spectrum_pre_filter = value;
                if (update)
                {
                    if (value != spectrum_pre_filter_dsp || force)
                    {
                        //Debug.WriteLine(thread+" "+subrx+" SpectrumPreFilter: "+value);
                        DttSP.SetPWSmode(thread, subrx, value);
                        spectrum_pre_filter_dsp = value;
                    }
                }
            }
        }

        //=====================================================================================
        private bool active_dsp = false;
        private bool active = false;
        public bool Active
        {
            get { return active; }
            set
            {
                active = value;
                if (update)
                {
                    if (value != active_dsp || force)
                    {
                        DttSP.SetRXOn(thread, subrx, value);
                        active_dsp = value;
                    }
                }
            }
        }


        //=====================================================================================
        private float pan_dsp = 0.5f;
        private float pan = 0.5f;
        /// <summary>
        /// Determines the left (0.0) to right (1.0) audio field for this RX.
        /// </summary>
		public float Pan
        {
            get { return pan; }
            set
            {
                pan = value;
                if (update)
                {
                    if (value != pan_dsp || force)
                    {
                        DttSP.SetRXPan(thread, subrx, value);
                        pan_dsp = value;
                    }
                }
            }
        }

        //=====================================================================================
        private double rx_osc_dsp = 0.011025;
        private double rx_osc = 0.011025;
        public double RXOsc
        {
            get { return rx_osc; }
            set
            {
                rx_osc = value;
                if (update)
                {
                    if (value != rx_osc_dsp || force)
                    {
                        DttSP.SetRXOsc(thread, subrx, value);
                        rx_osc_dsp = value;
                    }
                }
            }
        }

        //=====================================================================================
        private double osc_phase_dsp = 0.0;
        public double OSC_PHASE_DSP
        {
            get { return osc_phase_dsp; }
            set
            {
                if (update)
                {
                    if (value != osc_phase_dsp || force)
                    {
                        osc_phase_dsp = value;
                        DttSP.SetOscPhase(osc_phase_dsp);
                    }
                }
            }
        }

        //=====================================================================================
        private bool dc_block_dsp = false;
        private bool dc_block = false;
        public bool DCBlock
        {
            get { return dc_block; }
            set
            {
                //Debug.WriteLine("DC Block (" + thread + ", " + subrx + "): " + value +" -- "+update);
                dc_block = value;
                if (update)
                {
                    if (value != dc_block_dsp || force)
                    {
                        DttSP.SetRXDCBlock(thread, subrx, value);
                        dc_block_dsp = value;
                    }
                }
            }
        }

        //=====================================================================================
        private double rx_fm_deviation = 5000.0;
        private double rx_fm_deviation_dsp = 5000.0;
        public double RXFMDeviation // ke9ns .218 2.5khz, 5khz, 17khz dev options
        {
            get { return rx_fm_deviation; }
            set
            {
                rx_fm_deviation = value;
                if (update)
                {
                    if (value != rx_fm_deviation_dsp || force)
                    {
                        DttSP.SetRXFMDeviation(thread, subrx, value);
                        rx_fm_deviation_dsp = value;
                    }
                }
            }
        }

        //=====================================================================================
        private const int MAX_NOTCHES_IN_PASSBAND = 18;
        private bool[] notch_on = new bool[MAX_NOTCHES_IN_PASSBAND];
        private bool[] notch_on_dsp = new bool[MAX_NOTCHES_IN_PASSBAND];
        public bool GetNotchOn(int index)
        {
            return notch_on[index];
        }

        //=====================================================================================
        public void SetNotchOn(uint index, bool b)
        {
            notch_on[index] = b;
            if (update)
            {
                if (b != notch_on_dsp[index] || force)
                {
                    DttSP.SetRXManualNotchEnable(thread, subrx, index, b);
                    notch_on_dsp[index] = b;
                }
            }
        }

        //=====================================================================================
        private double[] notch_freq = new double[MAX_NOTCHES_IN_PASSBAND];
        private double[] notch_freq_dsp = new double[MAX_NOTCHES_IN_PASSBAND];
        public double GetNotchFreq(uint index)
        {
            return notch_freq[index];
        }

        //=====================================================================================
        public void SetNotchFreq(uint index, double freq)
        {
            notch_freq[index] = freq;
            if (update)
            {
                if (freq != notch_freq_dsp[index] || force)
                {
                    DttSP.SetRXManualNotchFreq(thread, subrx, index, freq);
                    notch_freq_dsp[index] = freq;
                }
            }
        }

        //=====================================================================================
        private double[] notch_bw = new double[MAX_NOTCHES_IN_PASSBAND];
        private double[] notch_bw_dsp = new double[MAX_NOTCHES_IN_PASSBAND];
        public double GetNotchBW(uint index)
        {
            return notch_bw[index];
        }

        //=====================================================================================
        /// <summary>
        /// Sets the notch bandwidth
        /// </summary>
        /// <param name="index">index of notch to set</param>
        /// <param name="bw">Bandwidth in Hz</param>
        public void SetNotchBW(uint index, double bw)
        {
            notch_bw[index] = bw;
            if (update)
            {
                if (bw != notch_bw_dsp[index] || force)
                {
                    DttSP.SetRXManualNotchBW(thread, subrx, index, bw);
                    notch_bw_dsp[index] = bw;
                }
            }
        }

        #endregion
    } //DSPRX

    #endregion

    #region DSPTX Class

    public sealed class DSPTX
    {
        private uint thread;

        public DSPTX(uint t)
        {
            thread = t;
            DttSP.SetTRX(t, true);
        }


        //============================================================================
        public void SyncAll()
        {
            //Debug.WriteLine("PEQ===  SYNCALL,= " + tx_eq_num_bands);


            //BufferSize = buffer_size;
            AudioSize = audio_size;
            CurrentDSPMode = current_dsp_mode;
            SetTXFilter(tx_filter_low, tx_filter_high);
            TXOsc = tx_osc;
            DCBlock = dc_block;

            if (tx_eq_num_bands == 3)
            {

                TXEQ3 = tx_eq3;
            }
            else if (tx_eq_num_bands == 28)
            {
                TXEQ28 = tx_eq28;  // ke9ns add
            }
            else if (tx_eq_num_bands == 9)
            {
                PEQ = peq; // ke9ns
            }
            else if (tx_eq_num_bands == 37)// 10
            {
                PEQ = peq; // ke9ns
                TXEQ28 = tx_eq28;  // ke9ns add

            }
            else
            {
                TXEQ10 = tx_eq10;
            }

            TXEQOn = tx_eq_on;
            Notch160 = notch_160;
            TXCorrectIQGain = tx_correct_iq_gain;
            TXCorrectIQPhase = tx_correct_iq_phase;
            TXCorrectIQMu = tx_correct_iq_mu;
            TXAMCarrierLevel = tx_am_carrier_level;
            TXALCBottom = tx_alc_bottom;
            TXALCAttack = tx_alc_attack;
            TXALCDecay = tx_alc_decay;
            TXALCHang = tx_alc_hang;
            TXLevelerMaxGain = tx_leveler_max_gain;
            TXLevelerAttack = tx_leveler_attack;
            TXLevelerDecay = tx_leveler_decay;
            TXLevelerHang = tx_leveler_hang;
            TXLevelerOn = tx_leveler_on;
            CurrentWindow = current_window;
            SpectrumPolyphase = spectrum_polyphase;
            TXSquelchThreshold = tx_squelch_threshold;
            TXSquelchAttenuate = tx_squelch_attenuate;
            TXSquelchOn = tx_squelch_on;
            TXCompandOn = tx_compand_on;
            TXCompandLevel = tx_compand_level;
            SpectrumPreFilter = spectrum_pre_filter;
            CTCSSFreqHz = ctcss_freq_hz;
            TXFMDeviation = tx_fm_deviation;
            CTCSSFlag = ctcss_flag;
            TXFMDataMode = fm_data_mode; // ke9ns add

        } // syncall

        #region Non-Static Properties & Routines

        //=====================================================================================
        /// <summary>
        /// Controls whether updates to following properties call the DSP.  
        /// Each property uses this value and a copy of the last thing sent to
        /// the DSP to update in a minimal fashion.
        /// </summary>
        private bool update = false;
        public bool Update
        {
            get { return update; }
            set
            {
                update = value;
                if (value) SyncAll();
            }
        }

        //=====================================================================================
        /// <summary>
        /// Used to force properties to update even if the DSP copy matches the
        /// new setting.  Mainly used to resync the DSP after having to rebuild
        /// when resetting DSP Block Size or Sample Rate.
        /// </summary>
        private bool force = false;
        public bool Force
        {
            get { return force; }
            set { force = value; }
        }

        //=====================================================================================
        private int buffer_size_dsp = 2048;
        private int buffer_size = 2048;
        public int BufferSize
        {
            get { return buffer_size; }
            set
            {
                buffer_size = value;
                if (update)
                {
                    if (value != buffer_size_dsp || force)
                    {
                        //Debug.WriteLine("TX "+thread+" "+value);
                        DttSP.ResizeSDR(thread, value);
                        buffer_size_dsp = value;
                        //SyncAll();
                    }
                }
            }
        }

        //=====================================================================================
        private int audio_size_dsp = 2048;
        private int audio_size = 2048;
        public int AudioSize
        {
            get { return audio_size; }
            set
            {
                audio_size = value;
                if (update)
                {
                    if (value != audio_size_dsp || force)
                    {
                        DttSP.SetAudioSize(value);
                        audio_size_dsp = value;
                    }
                }
            }
        }

        //=====================================================================================
        private DSPMode current_dsp_mode_dsp = DSPMode.USB;
        private DSPMode current_dsp_mode = DSPMode.USB;
        public DSPMode CurrentDSPMode
        {
            get { return current_dsp_mode; }
            set
            {
                current_dsp_mode = value;
                if (update)
                {
                    if (value != current_dsp_mode_dsp || force)
                    {
                        DttSP.SetTXMode(thread, value);
                        current_dsp_mode_dsp = value;
                    }
                }
            }
        }

        //=====================================================================================
        public void SetTXFilter(int low, int high)
        {
            tx_filter_low = low;
            tx_filter_high = high;
            if (update)
            {
                if (low != tx_filter_low_dsp || high != tx_filter_high_dsp || force)
                {
                    DttSP.SetTXFilter(thread, low, high);
                    tx_filter_low_dsp = low;
                    tx_filter_high_dsp = high;
                }
            }
        }

        //=====================================================================================
        private int tx_filter_low_dsp;
        private int tx_filter_low;
        public int TXFilterLow
        {
            get { return tx_filter_low; }
            set
            {
                tx_filter_low = value;
                if (update)
                {
                    if (value != tx_filter_low_dsp || force)
                    {
                        DttSP.SetTXFilter(thread, value, tx_filter_high);
                        tx_filter_low_dsp = value;
                    }
                }
            }
        }

        //=====================================================================================
        private int tx_filter_high_dsp;
        private int tx_filter_high;
        public int TXFilterHigh
        {
            get { return tx_filter_high; }
            set
            {
                tx_filter_high = value;
                if (update)
                {
                    if (value != tx_filter_high_dsp || force)
                    {
                        DttSP.SetTXFilter(thread, tx_filter_low, value);
                        tx_filter_high_dsp = value;
                    }
                }
            }
        }


        //=====================================================================================
        private double tx_osc_dsp = 0.0f;
        private double tx_osc = 0.0;
        public double TXOsc
        {
            get { return tx_osc; }
            set
            {
                tx_osc = value;
                if (update)
                {
                    if (value != tx_osc_dsp || force)
                    {
                        DttSP.SetTXOsc(thread, value);
                        tx_osc_dsp = value;
                    }
                }
            }
        }

        //=====================================================================================
        private bool dc_block_dsp;
        private bool dc_block;
        public bool DCBlock
        {
            get { return dc_block; }
            set
            {
                dc_block = value;
                if (update)
                {
                    if (value != dc_block_dsp || force)
                    {
                        DttSP.SetTXDCBlock(thread, value);
                        dc_block_dsp = value;
                    }
                }
            }
        }

        //=====================================================================================
        // ke9ns set to 10 or 3 or 28 or 9 PEQ
        private int tx_eq_num_bands = 3;
        public int TXEQNumBands
        {
            get { return tx_eq_num_bands; }
            set { tx_eq_num_bands = value; }
        }


        //=====================================================================================
        // ke9ns set when 3 band EQ enabled
        private int[] tx_eq3_dsp = new int[4];
        private int[] tx_eq3 = new int[4];
        public int[] TXEQ3
        {
            get { return tx_eq3; }
            set
            {
                for (int i = 0; i < tx_eq3.Length && i < value.Length; i++)
                {
                    tx_eq3[i] = value[i];
                }
                if (update)
                {

                    DttSP.SetGrphTXEQ(thread, tx_eq3);  // ke9ns this routine is in update.c code

                    for (int i = 0; i < tx_eq3_dsp.Length && i < value.Length; i++)
                    {
                        tx_eq3_dsp[i] = value[i];
                    }

                }
            }
        }

        //=====================================================================================
        // ke9ns called by eqform when you enable the 10 band EQ
        private int[] tx_eq10_dsp = new int[11];
        private int[] tx_eq10 = new int[11];
        public int[] TXEQ10
        {
            get
            {

                return tx_eq10;
            }
            set
            {
                for (int i = 0; i < tx_eq10.Length && i < value.Length; i++)
                {
                    tx_eq10[i] = value[i];
                }
                if (update)
                {

                    DttSP.SetGrphTXEQ10(thread, tx_eq10); // ke9ns this routine is in update.c code

                    for (int i = 0; i < tx_eq10_dsp.Length && i < value.Length; i++)
                    {
                        tx_eq10_dsp[i] = value[i];
                    }

                } // update
            } // set
        } // TXEQ10


        //=====================================================================================
        // ke9ns: called by eqform when you enable both  28 band EQ and 9band PEQ

        // must do a eqform.TXEQ get
        // then come here and do a TXEQ28 set

        private bool both = false;

        public bool BOTH
        {
            get
            {
                return both;
            }
            set
            {
                both = value;
                if (both == true)
                {

                    DttSP.SetGrphTXEQ37(thread, tx_eq28, peq);
                }

            }

        } // BOTH


        //=====================================================================================
        // ke9ns: called by eqform when you enable the 28 band EQ

        private int[] tx_eq28_dsp = new int[30];
        private int[] tx_eq28 = new int[30];
        public int[] TXEQ28
        {
            get
            {
                return tx_eq28;
            }
            set
            {

                for (int i = 0; i < tx_eq28.Length && i < value.Length; i++)
                {
                    tx_eq28[i] = value[i];
                }
                if (update)
                {
                    if (both == true)
                    {
                        DttSP.SetGrphTXEQ37(thread, tx_eq28, peq);
                    }
                    else
                    {
                        DttSP.SetGrphTXEQ28(thread, tx_eq28); // ke9ns this routine is in update.c code
                    }


                    for (int i = 0; i < tx_eq28_dsp.Length && i < value.Length; i++)
                    {
                        tx_eq28_dsp[i] = value[i];
                    }

                } // update
            } // set
        } // TXEQ28



        //=====================================================================================
        // ke9ns: called by eqform when you enable the 9 band PEQ 

        private int[] tx_peq_dsp = new int[28];
        private int[] peq = new int[28];
        public int[] PEQ
        {
            get
            {
                return peq;
            }
            set
            {

                for (int i = 0; (i < peq.Length) && (i < value.Length); i++)
                {
                    peq[i] = value[i];
                }

                if (update)
                {

                    //	Debug.WriteLine("+++9Band:");

                    if (both == true)
                    {
                        DttSP.SetGrphTXEQ37(thread, tx_eq28, peq);
                    }
                    else
                    {
                        DttSP.SetGrphTXPEQ(thread, peq); // ke9ns this routine is in update.c code
                    }


                    for (int i = 0; (i < tx_peq_dsp.Length) && (i < value.Length); i++)
                    {
                        tx_peq_dsp[i] = value[i];
                    }

                } // update

                //	Debug.WriteLine("ggg9Band:");
            } // set
        } // PEQ


        //======================================================================================
        // ke9ns: called by eqform when you ENABLE TX eq            
        private bool tx_eq_on_dsp = false;
        private bool tx_eq_on = false;
        public bool TXEQOn
        {
            get { return tx_eq_on; }
            set
            {
                tx_eq_on = value;
                if (update)
                {
                    if (value != tx_eq_on_dsp || force)
                    {
                        DttSP.SetGrphTXEQcmd(thread, value);  // ke9ns this routine is in update.c code
                        tx_eq_on_dsp = value;
                    }
                }
            }
        } //TXEQOn


        //=============================================================================
        private bool notch_160_dsp = false;
        private bool notch_160 = false;
        public bool Notch160
        {
            get { return notch_160; }
            set
            {
                notch_160 = value;
                if (update)
                {
                    if (value != notch_160_dsp || force)
                    {
                        DttSP.SetNotch160(thread, value);
                        notch_160_dsp = value;
                    }
                }
            }
        }

        private double tx_correct_iq_gain_dsp = 0.0;
        private double tx_correct_iq_gain = 0.0;
        public double TXCorrectIQGain
        {
            get { return tx_correct_iq_gain; }
            set
            {
                tx_correct_iq_gain = value;
                if (update)
                {
                    if (value != tx_correct_iq_gain_dsp || force)
                    {
                        DttSP.SetTXIQGain(thread, value);
                        tx_correct_iq_gain_dsp = value;
                    }
                }
            }
        }

        private double tx_fm_deviation = 5000.0;
        private double tx_fm_deviation_dsp = 5000.0;

        public double TXFMDeviation
        {
            get { return tx_fm_deviation; }
            set
            {
                tx_fm_deviation = value;
                if (update)
                {
                    if (value != tx_fm_deviation_dsp || force)
                    {
                        DttSP.SetTXFMDeviation(thread, value);
                        tx_fm_deviation_dsp = value;
                    }
                }
            }
        } // TXFMDeviation


        //========================================================================================
        // ke9ns add FMData turn off K_Preemphasis and K_Deemphasis here
        // this calls dttsp.cs, which calls update.c, which  sdr.c will read the tx[thread].fm struct 
        // console calls  dsp.GetDSPTX(0).TXFMDataMode

        private bool fm_data_mode = false; // true = no preemphasis, no deemphasis and 15k bandwidth
        private bool fm_data_mode_dsp = false; // true = no preemphasis, no deemphasis and 15k bandwidth

        public bool TXFMDataMode
        {
            get { return fm_data_mode; }
            set
            {
                fm_data_mode = value;
                if (update)
                {
                    if (value != fm_data_mode_dsp || force)
                    {
                        DttSP.SetTXFMDataMode(thread, value); //  dttsp.cs     update.c place value into tx[thread].fm.fmdata struct     sdr.c will then use it
                        fm_data_mode_dsp = value;

                        Debug.WriteLine("FMDATA== " + value);
                    }
                }
            }
        } // TXFMDataMode



        //==================================================================================


        private double ctcss_freq_hz = 100.0;
        private double ctcss_freq_hz_dsp = 100.0;
        public double CTCSSFreqHz                   // ke9ns: CTCSS tones Freq
        {
            get { return ctcss_freq_hz; }
            set
            {
                ctcss_freq_hz = value;
                if (update)
                {
                    if (value != ctcss_freq_hz_dsp || force)
                    {
                        DttSP.SetCTCSSFreq(thread, value);
                        ctcss_freq_hz_dsp = value;
                    }
                }
            }
        }

        private bool ctcss_flag = false;
        private bool ctcss_flag_dsp = false;
        public bool CTCSSFlag                   // ke9ns: CTCSS Tones ON/OFF
        {
            get { return ctcss_flag; }
            set
            {
                ctcss_flag = value;
                if (update)
                    if (value != ctcss_flag_dsp || force)
                    {
                        {
                            DttSP.SetCTCSSFlag(thread, value);
                            ctcss_flag_dsp = value;
                        }
                    }
            }
        }

        private double tx_correct_iq_phase_dsp = 0.0;
        private double tx_correct_iq_phase = 0.0;
        public double TXCorrectIQPhase
        {
            get { return tx_correct_iq_phase; }
            set
            {
                tx_correct_iq_phase = value;
                if (update)
                {
                    if (value != tx_correct_iq_phase_dsp || force)
                    {
                        DttSP.SetTXIQPhase(thread, value);
                        tx_correct_iq_phase_dsp = value;
                    }
                }
            }
        }

        private double tx_correct_iq_mu_dsp = 0.0;
        private double tx_correct_iq_mu = 0.0;
        public double TXCorrectIQMu
        {
            get { return tx_correct_iq_mu; }
            set
            {
                tx_correct_iq_mu = value;
                if (update)
                {
                    if (value != tx_correct_iq_mu_dsp || force)
                    {
                        DttSP.SetTXIQMu(thread, value);
                        tx_correct_iq_mu_dsp = value;
                    }
                }
            }
        }

        private double tx_am_carrier_level_dsp = 0.35;
        private double tx_am_carrier_level = 0.35;
        public double TXAMCarrierLevel
        {
            get { return tx_am_carrier_level; }
            set
            {
                tx_am_carrier_level = value;
                if (update)
                {
                    if (value != tx_am_carrier_level_dsp || force)
                    {
                        //Debug.WriteLine("TXAMCArrierLevel: "+tx_am_carrier_level.ToString("f3"));
                        DttSP.SetTXAMCarrierLevel(thread, value);
                        tx_am_carrier_level_dsp = value;
                    }
                }
            }
        }

        private int tx_alc_bottom_dsp = -120;
        private int tx_alc_bottom = -120;
        private int TXALCBottom
        {
           get { return tx_alc_bottom; }
            set
            {
                tx_alc_bottom = value;
                if (update)
                {
                    if (value != tx_alc_bottom_dsp || force)
                    {
                        DttSP.SetTXALCBot(thread, value);
                        tx_alc_bottom_dsp = value;
                    }
                }
            }
        }

        private int tx_alc_attack_dsp = 2;
        private int tx_alc_attack = 2;
        public int TXALCAttack
        {
            get { return tx_alc_attack; }
            set
            {
                tx_alc_attack = value;
                if (update)
                {
                    if (value != tx_alc_attack_dsp || force)
                    {
                        DttSP.SetTXALCAttack(thread, value);
                        tx_alc_attack_dsp = value;
                    }
                }
            }
        }

        private int tx_alc_decay_dsp = 10;
        private int tx_alc_decay = 10;
        public int TXALCDecay
        {
            get { return tx_alc_decay; }
            set
            {
                tx_alc_decay = value;
                if (update)
                {
                    if (value != tx_alc_decay_dsp || force)
                    {
                        DttSP.SetTXALCDecay(thread, value);
                        tx_alc_decay_dsp = value;
                    }
                }
            }
        }

        private int tx_alc_hang = 500;
        public int TXALCHang
        {
            get { return tx_alc_hang; }
            set
            {
                tx_alc_hang = value;
                DttSP.SetTXALCHang(thread, value);
            }
        }

        private double tx_leveler_max_gain_dsp = 5.0;
        private double tx_leveler_max_gain = 5.0;
        public double TXLevelerMaxGain
        {
            get { return tx_leveler_max_gain; }
            set
            {
                tx_leveler_max_gain = value;
                if (update)
                {
                    if (value != tx_leveler_max_gain_dsp || force)
                    {
                        DttSP.SetTXLevelerMaxGain(thread, value);
                        tx_leveler_max_gain_dsp = value;
                    }
                }
            }
        }

        private int tx_leveler_attack_dsp = 2;
        private int tx_leveler_attack = 2;
        public int TXLevelerAttack
        {
            get { return tx_leveler_attack; }
            set
            {
                tx_leveler_attack = value;
                if (update)
                {
                    if (value != tx_leveler_attack_dsp || force)
                    {
                        DttSP.SetTXLevelerAttack(thread, value);
                        tx_leveler_attack_dsp = value;
                    }
                }
            }
        }

        private int tx_leveler_decay_dsp = 500;
        private int tx_leveler_decay = 500;
        public int TXLevelerDecay
        {
            get { return tx_leveler_decay; }
            set
            {
                tx_leveler_decay = value;
                if (update)
                {
                    if (value != tx_leveler_decay_dsp || force)
                    {
                        DttSP.SetTXLevelerDecay(thread, value);
                        tx_leveler_decay_dsp = value;
                    }
                }
            }
        }

        private int tx_leveler_hang_dsp = 500;
        private int tx_leveler_hang = 500;
        public int TXLevelerHang
        {
            get { return tx_leveler_hang; }
            set
            {
                tx_leveler_hang = value;
                if (update)
                {
                    if (value != tx_leveler_hang_dsp || force)
                    {
                        DttSP.SetTXLevelerHang(thread, value);
                        tx_leveler_hang_dsp = value;
                    }
                }
            }
        }

        private bool tx_leveler_on_dsp = true;
        private bool tx_leveler_on = true;
        public bool TXLevelerOn
        {
            get { return tx_leveler_on; }
            set
            {
                tx_leveler_on = value;
                if (update)
                {
                    if (value != tx_leveler_on_dsp || force)
                    {
                        DttSP.SetTXLevelerSt(thread, value);
                        tx_leveler_on_dsp = value;
                    }
                }
            }
        }

        private Window current_window_dsp = Window.BLKHARRIS;
        private Window current_window = Window.BLKHARRIS;
        public Window CurrentWindow
        {
            get { return current_window; }
            set
            {
                current_window = value;
                if (update)
                {
                    if (value != current_window_dsp || force)
                    {
                        DttSP.SetWindow(thread, value);
                        current_window_dsp = value;
                    }
                }
            }
        }

        private bool spectrum_polyphase_dsp = false;
        private bool spectrum_polyphase = false;
        public bool SpectrumPolyphase
        {
            get { return spectrum_polyphase; }
            set
            {
                spectrum_polyphase = value;
                if (update)
                {
                    if (value != spectrum_polyphase_dsp || force)
                    {
                        DttSP.SetSpectrumPolyphase(thread, value);
                        spectrum_polyphase_dsp = value;
                    }
                }
            }
        }



        private float tx_squelch_threshold_dsp = -40.0f;
        private float tx_squelch_threshold = -40.0f;
        public float TXSquelchThreshold
        {
            get { return tx_squelch_threshold; }
            set
            {
                tx_squelch_threshold = value;
                if (update)
                {
                    if (value != tx_squelch_threshold_dsp || force)
                    {
                        DttSP.SetTXSquelchVal(thread, value);
                        tx_squelch_threshold_dsp = value;
                    }
                }
            }
        }

        private float tx_squelch_attenuate_dsp = 80.0f;
        private float tx_squelch_attenuate = 80.0f;
        public float TXSquelchAttenuate
        {
            get { return tx_squelch_attenuate; }
            set
            {
                tx_squelch_attenuate = value;
                if (update)
                {
                    if (value != tx_squelch_attenuate_dsp || force)
                    {
                        DttSP.SetTXSquelchAtt(thread, value);
                        tx_squelch_attenuate_dsp = value;
                    }
                }
            }
        }

        private bool tx_squelch_on_dsp = false;
        private bool tx_squelch_on = false;
        public bool TXSquelchOn
        {
            get { return tx_squelch_on; }
            set
            {
                tx_squelch_on = value;
                if (update)
                {
                    if (value != tx_squelch_on_dsp || force)
                    {
                        DttSP.SetTXSquelchState(thread, value);
                        tx_squelch_on_dsp = value;
                    }
                }
            }
        }

        private bool tx_compand_on_dsp = false;
        private bool tx_compand_on = false;
        public bool TXCompandOn
        {
            get { return tx_compand_on; }
            set
            {
                tx_compand_on = value;

                if (update)
                {
                    if (value != tx_compand_on_dsp || force)
                    {
                        DttSP.SetTXCompandSt(thread, value);
                        tx_compand_on_dsp = value;
                        //Debug.WriteLine("TXCompandOn: "+value.ToString());
                    }
                }
            }
        }

        private double tx_compand_level_dsp = 0.1;
        private double tx_compand_level = 0.1;
        public double TXCompandLevel
        {
            get { return tx_compand_level; }
            set
            {
                tx_compand_level = value;

                if (update)
                {
                    if (value != tx_compand_level_dsp || force)
                    {
                        DttSP.SetTXCompand(thread, value);
                        tx_compand_level_dsp = value;
                        //Debug.WriteLine("TXCompandLevel: "+value.ToString("f2"));
                    }
                }
            }
        }

        private bool spectrum_pre_filter_dsp = true;
        private bool spectrum_pre_filter = true;
        public bool SpectrumPreFilter
        {
            get { return spectrum_pre_filter; }
            set
            {
                spectrum_pre_filter = value;
                if (update)
                {
                    if (value != spectrum_pre_filter_dsp || force)
                    {
                        DttSP.SetTXPWSmode(thread, value);
                        spectrum_pre_filter_dsp = value;
                    }
                }
            }
        }
        /*
                private bool active_dsp = false;
                private bool active = false;
                public bool Active
                {
                    get { return active; }
                    set
                    {
                        active = value;
                        if(update)
                        {
                            if(value != active_dsp || force)
                            {
                                DttSP.SetRXOn(thread, subrx, value);
                                active_dsp = value;
                            }
                        }
                    }
                }

                private float pan_dsp = 0.5f;
                private float pan = 0.5f;
                public float Pan
                {
                    get { return pan; }
                    set
                    {
                        pan = value;
                        if(update)
                        {
                            if(value != pan_dsp || force)
                            {
                                DttSP.SetRXPan(thread, subrx, value);
                                pan_dsp = value;
                            }
                        }
                    }
                }
        */

        #endregion
    }

    #endregion
}