//=================================================================
// dttsp.cs
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


using System.Runtime.InteropServices;

namespace PowerSDR
{
    unsafe sealed class DttSP
    {
        #region Enums

        public enum MeterType
        {
            SIGNAL_STRENGTH = 0,
            AVG_SIGNAL_STRENGTH,
            ADC_REAL,
            ADC_IMAG,
            AGC_GAIN,
            MIC,
            PWR,
            ALC,
            EQ,
            LEVELER,
            COMP,
            CPDR,
            ALC_G,
            LVL_G,
            MIC_PK,
            ALC_PK,
            EQ_PK,
            LEVELER_PK,
            COMP_PK,
            CPDR_PK,
        }

        #endregion

        #region Dll Method Definitions
        // ======================================================
        // DLL Method Definitions
        // ======================================================

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Setup_SDR")]
        /// <summary>
        /// This function sets up the SDR functions and data structures
        /// </summary>
        /// <returns></returns>
        public static extern void SetupSDR(System.String app_data_path);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetDSPBuflen")]  // ke9ns found in update.c and winmain.c
        public static extern void ResizeSDR(uint thread, int DSPsize);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Destroy_SDR")]
        public static extern void Exit();

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "process_samples_thread")]
        public static extern void ProcessSamplesThread(uint thread);

        //		DllImport("DttSP.dll", EntryPoint="AudioReset")]
        //		public static extern void AudioReset();

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetSwchFlag")]
        public static extern void SetSwchFlag(uint thread, bool val);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetSwchRiseThresh")]
        public static extern void SetSwchRiseThresh(uint thread, float val);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetThreadProcessingMode")]
        public static extern void SetThreadProcessingMode(uint thread, int runmode);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Audio_Callback")]
        public static extern void ExchangeSamples(void* input_l, void* input_r, void* output_l, void* output_r, int numsamples);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Audio_Callback2")]   // ke9ns used in the transmit / receive audio   winmain.c
        public static extern void ExchangeSamples2(void* input, void* output, int numsamples); // called from audio.cs

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetAudioSize")]///
		public static extern void SetAudioSize(int size);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetMode")]///
		public static extern int SetMode(uint thread, uint subrx, DSPMode m);

        public static int SetTXMode(uint thread, DSPMode m)
        {
            return SetMode(thread, 0, m);
        }

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetRXFilter")]///
		public static extern int SetRXFilter(uint thread, uint subrx, double low, double high);


        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetTXFilter")]///
		public static extern int SetTXFilter(uint thread, double low, double high);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetTXOsc")]///
		public static extern int SetTXOsc(uint thread, double freq);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetSampleRate")] // ke9ns: change sample rate pauses all 3 threads, then changes rate
        public static extern int SetSampleRate(double sampleRate);


        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetHiResPan")] // ke9ns: add tell process_panadapter for a larger buffer size (Calls update.c)
        public static extern void SetHiResPan(int hires);


        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetNR")]///
		public static extern void SetNR(uint thread, uint subrx, bool setit);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetNRvals")]///
		public static extern void SetNRvals(uint thread, uint subrx, int taps, int delay, double gain, double leak);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetRXManualNotchEnable")]///
        public static extern void SetRXManualNotchEnable(uint thread, uint subrx, uint index, bool flag);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetRXManualNotchFreq")]///
        public static extern void SetRXManualNotchFreq(uint thread, uint subrx, uint index, double freq);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetRXManualNotchBW")]///
        public static extern void SetRXManualNotchBW(uint thread, uint subrx, uint index, double bw);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetANF")]///
		public static extern void SetANF(uint thread, uint subrx, bool setit);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetANFvals")]///
		public static extern void SetANFvals(uint thread, uint subrx, int taps, int delay, double gain, double leak);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetRXAGC")]///
		public static extern void SetRXAGC(uint thread, uint subrx, AGCMode setit);


        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetTXAGCFF")]///
		public static extern void SetTXAGCFF(uint thread, bool setit);



        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetTXAGCFFCompression")]///
		public static extern void SetTXAGCFFCompression(uint thread, double txcompression);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetTXDCBlock")]///
		public static extern void SetTXDCBlock(uint thread, bool setit);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetRXDCBlock")]///
        public static extern void SetRXDCBlock(uint thread, uint subrx, bool setit);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetRXDCBlockGain")]///
        public static extern void SetRXDCBlockGain(uint thread, uint subrx, float gain);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetTXEQ")]
        public static extern void SetTXEQ(uint thread, int[] txeq);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetGrphTXEQcmd")]///
		public static extern void SetGrphTXEQcmd(uint thread, bool state);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetGrphTXEQ")]///
		public static extern void SetGrphTXEQ(uint thread, int[] txeq);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetGrphTXEQ10")]///
		public static extern void SetGrphTXEQ10(uint thread, int[] txeq10);


        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetGrphTXEQ37")]///
		public static extern void SetGrphTXEQ37(uint thread, int[] txeq28, int[] peq); //ke9ns add 28 band eq and 9 band peq


        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetGrphTXEQ28")]///
		public static extern void SetGrphTXEQ28(uint thread, int[] txeq28); //ke9ns add 28 band eq


        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetGrphTXPEQ")]///
		public static extern void SetGrphTXPEQ(uint thread, int[] peq); //ke9ns add: parametric EQ .162


        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetTXFMDeviation")]///
        public static extern void SetTXFMDeviation(uint thread, double deviation);


        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetTXFMDataMode")]///
        public static extern void SetTXFMDataMode(uint thread, bool fmdata); // ke9ns add for FM data mode (Calls update.c)



        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetRXFMDeviation")]///
        public static extern void SetRXFMDeviation(uint thread, uint subrx, double deviation);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetFMSquelchThreshold")]///
        public static extern void SetFMSquelchThreshold(uint thread, uint subrx, float threshold);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetFMSquelchBreak")] // ke9ns add
        public static extern void GetFMSquelchBreak(uint thread, uint subrx, bool* fmsquelchbreak);


        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetCTCSSFreq")]///
        public static extern void SetCTCSSFreq(uint thread, double freq_hz);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetCTCSSFlag")]///
        public static extern void SetCTCSSFlag(uint thread, bool flag);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetGrphRXEQcmd")]///
		public static extern void SetGrphRXEQcmd(uint thread, uint subrx, bool state);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetGrphRXEQ")]///
		public static extern void SetGrphRXEQ(uint thread, uint subrx, int[] rxeq);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetGrphRXEQ10")]///
		public static extern void SetGrphRXEQ10(uint thread, uint subrx, int[] rxeq);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetNotch160")]///
		public static extern void SetNotch160(uint thread, bool state);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetNB")]///
		public static extern void SetNB(uint thread, uint subrx, bool setit); // ke9ns: update.c

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetNBvals")]/// NB values
		public static extern void SetNBvals(uint thread, uint subrx, double threshold, int hangTime, int delayTime); // ke9ns mod: update.c

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetSAMFreq")]///
		public static extern void GetSAMFreq(uint thread, uint subrx, float* freq);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetSAMPLLvals")]///
		public static extern void GetSAMPLLvals(uint thread, uint subrx, float* alpha, float* beta);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetSAMPLLvals")]/// ke9ns: update.c
		public static extern void SetSAMPLLvals(uint thread, uint subrx, float alpha, float beta);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetCorrectIQEnable")]
        public static extern void SetCorrectIQEnable(uint setit);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetCorrectRXIQw")]
        public static extern void GetCorrectRXIQw(uint thread, uint subrx, float* real, float* imag, uint index);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetCorrectRXIQwReal")]///
		public static extern void SetCorrectRXIQwReal(uint thread, uint subrx, float setit, uint index);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetCorrectRXIQwImag")]///
		public static extern void SetCorrectRXIQwImag(uint thread, uint subrx, float setit, uint index);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetCorrectRXIQw")]///
		public static extern void SetCorrectRXIQw(uint thread, uint subrx, float real, float imag, uint index);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetCorrectRXIQMu")]///
		public static extern void SetCorrectIQMu(uint thread, uint subrx, double setit);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetCorrectRXIQMu")]///
		public static extern float GetCorrectIQMu(uint thread, uint subrx);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetCorrectTXIQGain")]///
		public static extern void SetTXIQGain(uint thread, double setit);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetCorrectTXIQPhase")]///
		public static extern void SetTXIQPhase(uint thread, double setit);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetCorrectTXIQMu")]///
		public static extern void SetTXIQMu(uint thread, double setit);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetSDROM")]///
		public static extern void SetSDROM(uint thread, uint subrx, bool setit);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetSDROMvals")]///
		public static extern void SetSDROMvals(uint thread, uint subrx, double threshold);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetFixedAGC")]///
		public static extern void SetFixedAGC(uint thread, uint subrx, double fixed_agc);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetRXAGCTop")]///
		public static extern void SetRXAGCMaxGain(uint thread, uint subrx, double max_agc);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetRXAGCAttack")]///
		public static extern void SetRXAGCAttack(uint thread, uint subrx, int attack);


        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetRXAGCDecay")]///
		public static extern void SetRXAGCDecay(uint thread, uint subrx, int decay);


        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetRXAGCHang")]///
		public static extern void SetRXAGCHang(uint thread, uint subrx, int hang);


        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetRXOutputGain")]///
		public static extern void SetRXOutputGain(uint thread, uint subrx, double g);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetRXAGCSlope")]///
		public static extern void SetRXAGCSlope(uint thread, uint subrx, int slope);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetRXAGCHangThreshold")]///
		public static extern void SetRXAGCHangThreshold(uint thread, uint subrx, int hangthreshold);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetTXAMCarrierLevel")]///
		public static extern void SetTXAMCarrierLevel(uint thread, double carrier_level);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetTXALCBot")]///
		public static extern void SetTXALCBot(uint thread, double max_agc);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetTXALCAttack")]///
		public static extern void SetTXALCAttack(uint thread, int attack);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetTXALCDecay")]///
		public static extern void SetTXALCDecay(uint thread, int attack);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetTXALCHang")]///
		public static extern void SetTXALCHang(uint thread, int hang);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetTXLevelerTop")]
        public static extern void SetTXLevelerMaxGain(uint thread, double max_agc);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetTXLevelerAttack")]///
		public static extern void SetTXLevelerAttack(uint thread, int attack);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetTXLevelerDecay")]///
		public static extern void SetTXLevelerDecay(uint thread, int attack);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetTXLevelerHang")]///
		public static extern void SetTXLevelerHang(uint thread, int hang);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetTXLevelerSt")]///
		public static extern void SetTXLevelerSt(uint thread, bool state);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetWindow")]///
		public static extern void SetWindow(uint thread, Window windowset);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetSpectrumPolyphase")]///
		public static extern void SetSpectrumPolyphase(uint thread, bool state);



        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetBIN")]///
		public static extern void SetBIN(uint thread, uint subrx, bool setit);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetSquelchVal")]///
		public static extern void SetSquelchVal(uint thread, uint subrx, float setit);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetSquelchState")]///
		public static extern void SetSquelchState(uint thread, uint subrx, bool state);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetTXSquelchVal")]///
		public static extern void SetTXSquelchVal(uint thread, float setit);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetTXSquelchAtt")]///
		public static extern void SetTXSquelchAtt(uint thread, float setit);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetTXSquelchSt")]///
		public static extern void SetTXSquelchState(uint thread, bool state);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetTXCompandSt")]///
		public static extern void SetTXCompandSt(uint thread, bool state);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetTXCompand")]///
		public static extern void SetTXCompand(uint thread, double setit);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetPWSmode")]///
		public static extern void SetPWSmode(uint thread, uint subrx, bool setit);
        public static void SetTXPWSmode(uint thread, bool setit)
        {
            SetPWSmode(thread, 0, setit);
        }

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Process_Spectrum")]
        unsafe public static extern void GetSpectrum(uint thread, float* results);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Process_ComplexSpectrum")]
        unsafe public static extern void GetComplexSpectrum(uint thread, float* results);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Process_Panadapter")]   // ke9ns   found in update.c   used on console.cs
        unsafe public static extern void GetPanadapter(uint thread, float* results);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Process_Phase")]
        unsafe public static extern void GetPhase(uint thread, float* results, int numpoints);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Process_Scope")]
        unsafe public static extern void GetScope(uint thread, float* results, int numpoints);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetTRX")]
        unsafe public static extern void SetTRX(uint thread, bool trx_on);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "FlushAllBufs")]
        unsafe public static extern void FlushAllBufs(uint thread, bool trx);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "CalculateRXMeter")]
        public static extern float CalculateRXMeter(uint thread, uint subrx, MeterType MT);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "CalculateTXMeter")]
        public static extern float CalculateTXMeter(uint thread, MeterType MT);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Release_Update")]
        unsafe public static extern void ReleaseUpdate();

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "NewResampler")]
        unsafe public static extern void* NewResampler(int sampin, int sampout);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "DoResampler")]
        unsafe public static extern void DoResampler(float* input, float* output, int numsamps, int* outsamps, void* ptr);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "DelPolyPhaseFIR")]
        unsafe public static extern void DelResampler(void* ptr);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "NewResamplerF")] // update.c ke9ns: used to resample VAC1 from the SR of the primary audio (like 192k)
        unsafe public static extern void* NewResamplerF(int sampin, int sampout);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "DoResamplerF")]
        unsafe public static extern void DoResamplerF(float* input, float* output, int numsamps, int* outsamps, void* ptr);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "DelPolyPhaseFIRF")]
        unsafe public static extern void DelResamplerF(void* ptr);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetThreadNo")]///
		unsafe public static extern void SetThreadNo(uint threadno);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetThreadCom")]///
		unsafe public static extern void SetThreadCom(uint thread_com);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetSubRXSt")]///
		unsafe public static extern void SetRXOn(uint thread, uint subrx, bool setit);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetRXPan")]///
		unsafe public static extern void SetRXPan(uint thread, uint subrx, float pan); // takes values from 0 to 1.0 for L to R.

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetRXOsc")]///
		unsafe public static extern int SetRXOsc(uint thread, uint subrx, double freq);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetOscPhase")]///
		unsafe public static extern int SetOscPhase(double phase);

        #region Diversity

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetDiversity")]
        unsafe public static extern int SetDiversity(int on);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetDiversityScalar")]
        unsafe public static extern int SetDiversityScalar(float real, float imag);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetDiversityGain")]
        unsafe public static extern int SetDiversityGain(float gain); // valid 0.0 - 1.0

        #endregion

        #endregion



    }
}
