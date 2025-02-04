/* sdr.c

This file is part of a program that implements a Software-Defined Radio.

Copyright (C) 2004, 2005, 2006 by Frank Brickle, AB2KT and Bob McGwier, N4HY.

This program is free software; you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation; either version 2 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

The authors can be reached by email at

ab2kt@arrl.net
or
rwmcgwier@comcast.net

or by paper mail at

The DTTS Microwave Society
6 Kathleen Place
Bridgewater, NJ 08807
*/

#include <common.h>

//========================================================================
/* initialization and termination */
int const MAX_NOTCHES_IN_PASSBAND = 18;

void reset_meters (unsigned int thread)
{
	if (uni[thread].meter.flag)
	{                           // reset metering completely
		int i, k;
		for (i = 0; i < RXMETERPTS; i++)
			for (k = 0; k < MAXRX; k++)
				uni[thread].meter.rx.val[k][i] = -200.0;
		for (i = 0; i < TXMETERPTS; i++)
			uni[thread].meter.tx.val[i] = -200.0;
	}
}

void reset_spectrum (unsigned int thread)
{
	if (uni[thread].spec.flag)
		reinit_spectrum (&uni[thread].spec);
}

void reset_counters (unsigned thread)
{
	int k;
	for (k = 0; k < uni[thread].multirx.nrx; k++)
		rx[thread][k].tick = 0;
	tx[thread].tick = 0;
}

//========================================================================

/* global and general info,
   not specifically attached to
   tx, rx, or scheduling */

PRIVATE void setup_all (REAL rate, int buflen, SDRMODE mode, char *wisdom, int specsize, int numrecv, int cpdsize, unsigned int thread)
{
	uni[thread].samplerate = rate;
	uni[thread].buflen = buflen;
	uni[thread].mode.sdr = mode;
	if (thread != 1) uni[thread].mode.trx = RX;
	else uni[thread].mode.trx = TX;

	uni[thread].wisdom.path = wisdom;
	uni[thread].wisdom.bits = FFTW_ESTIMATE;
	{
		FILE *f = fopen (uni[thread].wisdom.path, "r");
		if (f)
		{
			char wisdomstring[32768];

			fread(wisdomstring,1,32768,f);

			if (fftwf_import_wisdom_from_string (wisdomstring) != 0)
				uni[thread].wisdom.bits = FFTW_MEASURE;
			fclose (f);
		}
	}

	if (uni[thread].meter.flag)
	{
		reset_meters (thread);
	}

	uni[thread].spec.rxk = 0;
	uni[thread].spec.buflen = uni[thread].buflen;
	uni[thread].spec.scale = SPEC_PWR;
	uni[thread].spec.type = SPEC_POST_FILT;
	uni[thread].spec.size = specsize;  // ke9ns: set size of buffer 4096?
	uni[thread].spec.planbits = uni[thread].wisdom.bits;
	init_spectrum (&uni[thread].spec);
	//fprintf(stderr,"Created spectrum\n"),fflush(stderr);

	// set which receiver is listening to commands
	uni[thread].multirx.lis = 0;
	uni[thread].multirx.nrx = numrecv;

	// set mixing of input from aux ports
	uni[thread].mix.rx.flag = uni[thread].mix.tx.flag = FALSE;
	uni[thread].mix.rx.gain = uni[thread].mix.tx.gain = 1.0;

	uni[thread].cpdlen = cpdsize;

	uni[thread].tick = uni[thread].oldtick = 0;
}


/* purely rx */

PRIVATE void setup_rx (int k, unsigned int thread)
{
	int i;

	/* conditioning */
	if (thread == 0) 
	{
		diversity.gain = 1.0;
		diversity.scalar = Cmplx(1.0,0);
	}
	rx[thread][k].iqfix = newCorrectIQ (0.0, 1.0, 0.000f);
	// Remove the next line
	rx[thread][k].iqfix->wbir_state = JustSayNo;
	// Remove the previous line
	rx[thread][k].filt.coef = newFIR_Bandpass_COMPLEX (150.0, 2850.0, uni[thread].samplerate, uni[thread].buflen + 1);
	rx[thread][k].filt.ovsv = newFiltOvSv (FIRcoef (rx[thread][k].filt.coef), FIRsize (rx[thread][k].filt.coef), uni[thread].wisdom.bits);
	rx[thread][k].resample.flag = FALSE;
	normalize_vec_COMPLEX (rx[thread][k].filt.ovsv->zfvec, rx[thread][k].filt.ovsv->fftlen, rx[thread][k].filt.ovsv->scale);

	rx[thread][k].output_gain = 1.0f;

	// hack for EQ
	rx[thread][k].filt.save = newvec_COMPLEX (rx[thread][k].filt.ovsv->fftlen, "RX filter cache");
	memcpy ((char *) rx[thread][k].filt.save, (char *) rx[thread][k].filt.ovsv->zfvec, rx[thread][k].filt.ovsv->fftlen * sizeof (COMPLEX));

	/* buffers */
	/* note we overload the internal filter buffers	we just created */
	rx[thread][k].buf.i = newCXB (FiltOvSv_fetchsize (rx[thread][k].filt.ovsv), FiltOvSv_fetchpoint (rx[thread][k].filt.ovsv), "init rx[thread][k].buf.i");

	rx[thread][k].buf.o = newCXB (FiltOvSv_storesize (rx[thread][k].filt.ovsv),	FiltOvSv_storepoint (rx[thread][k].filt.ovsv), "init rx[thread][k].buf.o");

	rx[thread][k].dcb = newDCBlocker(DCB_SINGLE_POLE, rx[thread][k].buf.i);
	rx[thread][k].dcb->flag = FALSE;

	/* conversion */
	rx[thread][k].osc.freq = -9000.0;
	rx[thread][k].osc.phase = 0.0;
	rx[thread][k].osc.gen = newOSC (uni[thread].buflen,	ComplexTone, rx[thread][k].osc.freq, rx[thread][k].osc.phase, uni[thread].samplerate, "SDR RX Oscillator");

	rx[thread][k].dttspagc.gen = newDttSPAgc (
		agcMED,							// mode kept around for control reasons alone
		CXBbase (rx[thread][k].buf.o),	// buffer pointer
		CXBsize (rx[thread][k].buf.o),	// buffer size
		1.0f,							// Target output
		2.0f,							// Attack time constant in ms
		250,							// Decay time constant in ms
		1.0,							// Slope
		250,							// Hangtime in ms
		uni[thread].samplerate,			// Sample rate
		31622.8f,						// Maximum gain as a multipler, linear not dB
		0.00001f,						// Minimum gain as a multipler, linear not dB
		1.0,							// Set the current gain
		"AGC");							// Set a tag for an error message if the memory allocation fails		

	rx[thread][k].dttspagc.flag = TRUE;

	rx[thread][k].grapheq.gen = new_EQ (rx[thread][k].buf.o, uni[thread].samplerate, uni[thread].wisdom.bits);
	rx[thread][k].grapheq.flag = FALSE;

	/* demods */
	rx[thread][k].am.gen = newAMD (
		uni[thread].samplerate,			// REAL samprate
		0.0,							// REAL f_initial
		-2000.0,						// REAL f_lobound,
		2000.0,							// REAL f_hibound,
		300.0,							// REAL f_bandwid,
		CXBsize (rx[thread][k].buf.o),	// int size,
		CXBbase (rx[thread][k].buf.o),	// COMPLEX *ivec,
		CXBbase (rx[thread][k].buf.o),	// COMPLEX *ovec,
		AMdet,							// AM Mode AMdet == rectifier,
		//         SAMdet == synchronous detector
		"AM detector blew");   // char *tag

	rx[thread][k].fm.gen = newFMD (
		uni[thread].samplerate,			// REAL samprate
		0.0,							// REAL f_initial
		-8000.0,						// REAL f_lobound
		8000.0,							// REAL f_hibound
		16000.0,						// REAL f_bandwid
		CXBsize (rx[thread][k].buf.o),	// int size
		CXBbase (rx[thread][k].buf.o),	// COMPLEX *ivec
		CXBbase (rx[thread][k].buf.o),	// COMPLEX *ovec
		"New FM Demod structure");		// char *error message;

	/* auto-notch filter */
	rx[thread][k].anf.gen = new_lmsr (
		rx[thread][k].buf.o,	// CXB signal,
		60,						// int delay,
		25e-4f,					// REAL adaptation_rate (gain),
		1e-7f,					// REAL leakage,
		68,					    // int adaptive_filter_size (taps),
		LMADF_INTERFERENCE);
	rx[thread][k].anf.flag = FALSE;

	/* block auto-notch filter */
	rx[thread][k].banf.gen = new_blms(
		rx[thread][k].buf.o,    // CXB signal,
		0.01f,					// REAL adaptation_rate,
		0.00000f,				// REAL leakage,
		LMADF_INTERFERENCE,		// type
		uni->wisdom.bits);      // fftw wisdom
	rx[thread][k].banf.flag = FALSE;

	/* auto-noise filter */
	rx[thread][k].anr.gen = new_lmsr (
		rx[thread][k].buf.o,	// CXB signal,
		30,						// int delay,
		16e-4f,					// REAL adaptation_rate, -- also called gain in some circumstances
		10e-7f,					// REAL leakage,
		40,						// int adaptive_filter_size (taps),
		LMADF_NOISE);
	rx[thread][k].anr.flag = FALSE;

	/* block auto-noise filter */
	rx[thread][k].banr.gen = new_blms(
		rx[thread][k].buf.o,    // CXB signal,
		0.001f,					// REAL adaptation_rate,
		0.000001f,				// REAL leakage,
		LMADF_NOISE,			// type
		uni->wisdom.bits);      // fftw wisdom
	rx[thread][k].banr.flag = FALSE;


	rx[thread][k].nb.thresh = 3.3f; // ke9ns: setup threshold * .165 (so 20 * .165 = 3.3)
//	rx[thread][k].nb.gen = new_noiseblanker (rx[thread][k].buf.i, rx[thread][k].nb.thresh); // ke9ns original way
	rx[thread][k].nb.gen = new_noiseblanker(rx[thread][k].buf.i, rx[thread][k].nb.thresh, rx[thread][k].nb.ht, rx[thread][k].nb.dly); // ke9ns mode: .182
	rx[thread][k].nb.flag = FALSE;

	rx[thread][k].nb_sdrom.thresh = 2.5f;
	rx[thread][k].nb_sdrom.gen = new_noiseblanker (rx[thread][k].buf.i, rx[thread][k].nb_sdrom.thresh, rx[thread][k].nb.ht, rx[thread][k].nb.dly);
	rx[thread][k].nb_sdrom.flag = FALSE;

	for(i = 0; i < MAX_NOTCHES_IN_PASSBAND; i++)
	{
		rx[thread][k].notch[i].gen = new_IIR_2P2Z(
			rx[thread][k].buf.o,	// Buffer
			1.0,					// Gain
			1.0,					// Parameter value - Q in this case
			Q,						// type of parameter
			NOTCH,					// type of filter
			uni[thread].samplerate, // sample rate
			400.0);					// frequency for the notch
		rx[thread][k].notch[i].gen->doComplex = TRUE;
	}

	rx[thread][k].spot.gen = newSpotToneGen (
		-12.0,						// gain
		700.0,						// freq
		5.0,						// ms rise
		5.0,						// ms fall
		uni[thread].buflen,			// length of spot tone buffer
		uni[thread].samplerate);	// sample rate

	memset ((char *) &rx[thread][k].squelch, 0, sizeof (rx[thread][k].squelch));
	rx[thread][k].squelch.thresh = -150.0;
	rx[thread][k].squelch.power = 0.0;
	rx[thread][k].squelch.flag = rx[thread][k].squelch.running = rx[thread][k].squelch.set = FALSE;
	rx[thread][k].squelch.num = uni[thread].buflen - 48;

	rx[thread][k].cpd.gen = newWSCompander (uni[thread].cpdlen, 0.0, rx[thread][k].buf.o);
	rx[thread][k].cpd.flag = FALSE;

	rx[thread][k].mode = uni[thread].mode.sdr;
	rx[thread][k].bin.flag = FALSE;

	{
		//REAL pos = 0.5,             // 0 <= pos <= 1, left->right
		//theta = (REAL) ((1.0 - pos) * M_PI / 2.0);
		rx[thread][k].azim = Cmplx (1.0f, 1.0f);
	}

	rx[thread][k].tick = 0;

} // setup_RX


  //==================================================================================================
  //==================================================================================================
  /* purely tx */
  //=================================================================================================
  //==================================================================================================
PRIVATE void setup_tx (unsigned int thread)
{
	/* conditioning */
	tx[thread].iqfix = newCorrectIQ (0.0, 1.0, 0.0);

	tx[thread].filt.coef = newFIR_Bandpass_COMPLEX (300.0, 3000.0, uni[thread].samplerate, uni[thread].buflen + 1);

	tx[thread].filt.ovsv = newFiltOvSv (FIRcoef (tx[thread].filt.coef), FIRsize (tx[thread].filt.coef), uni[thread].wisdom.bits);

	tx[thread].filt.ovsv_pre = newFiltOvSv (FIRcoef (tx[thread].filt.coef), FIRsize (tx[thread].filt.coef), uni[thread].wisdom.bits); // ke9ns pre-emphasis?

	normalize_vec_COMPLEX (tx[thread].filt.ovsv->zfvec, tx[thread].filt.ovsv->fftlen, tx[thread].filt.ovsv->scale);

	// hack for EQ
	tx[thread].filt.save = newvec_COMPLEX (tx[thread].filt.ovsv->fftlen, "TX filter cache");

	memcpy ((char *) tx[thread].filt.save,   (char *) tx[thread].filt.ovsv->zfvec,    tx[thread].filt.ovsv->fftlen * sizeof (COMPLEX)); // D, S, SIZE

	/* buffers */
	tx[thread].buf.i = newCXB (FiltOvSv_fetchsize (tx[thread].filt.ovsv), FiltOvSv_fetchpoint (tx[thread].filt.ovsv), "init tx[thread].buf.i");

	tx[thread].buf.o = newCXB (FiltOvSv_storesize (tx[thread].filt.ovsv), FiltOvSv_storepoint (tx[thread].filt.ovsv), "init tx[thread].buf.o");

	tx[thread].buf.ic = newCXB (FiltOvSv_fetchsize (tx[thread].filt.ovsv_pre), FiltOvSv_fetchpoint (tx[thread].filt.ovsv_pre), "init tx[thread].buf.ic");

	tx[thread].buf.oc = newCXB (FiltOvSv_storesize (tx[thread].filt.ovsv_pre), FiltOvSv_storepoint (tx[thread].filt.ovsv_pre), "init tx[thread].buf.oc");


	tx[thread].dcb.flag = FALSE;
	tx[thread].dcb.gen = newDCBlocker (DCB_MED, tx[thread].buf.i);

	/* conversion */
	tx[thread].osc.freq = 0.0;
	tx[thread].osc.phase = 0.0;
	tx[thread].osc.gen = newOSC (uni[thread].buflen, ComplexTone, tx[thread].osc.freq, tx[thread].osc.phase, uni[thread].samplerate, "SDR TX Oscillator");

	tx[thread].am.carrier_level = 0.5f;

	tx[thread].fm.cvtmod2freq = (REAL) (5000.0f * TWOPI / uni[thread].samplerate);    //5 kHz deviation..used to be 3
	tx[thread].fm.phase = 0.0;

	
	// standard FM mode	
	
	tx[thread].fm.k_preemphasis = (REAL)(1.0f + uni[thread].samplerate / (TWOPI * 3000.0f));  //6.09 for 96k
	tx[thread].fm.k_deemphasis = (REAL)(1.0f + uni[thread].samplerate / (TWOPI * 250.0f));  // 62.14 for 96k

	tx[thread].fm.input_LPF1 = new_IIR_LPF_2P(tx[thread].buf.i, uni[thread].samplerate, 3500.0f, 0.25f);	//4 pole butterworth Q = 0.76537, 1.84776	
	tx[thread].fm.input_LPF2 = new_IIR_LPF_2P(tx[thread].buf.i, uni[thread].samplerate, 3500.0f, 1.75f);	//4 pole butterworth Q = 0.76537, 1.84776
	tx[thread].fm.input_HPF1 = new_IIR_HPF_2P(tx[thread].buf.i, uni[thread].samplerate, 150.0f, 0.34f);	//4 pole butterworth Q = 0.76537, 1.84776	
	tx[thread].fm.input_HPF2 = new_IIR_HPF_2P(tx[thread].buf.i, uni[thread].samplerate, 150.0f, 0.94f);	//4 pole butterworth Q = 0.76537, 1.84776	
	
	tx[thread].fm.output_LPF1 = new_IIR_LPF_2P(tx[thread].buf.i, uni[thread].samplerate, 3500.0f, 0.25f);	//4 pole butterworth Q = 0.76537, 1.84776	
	tx[thread].fm.output_LPF2 = new_IIR_LPF_2P(tx[thread].buf.i, uni[thread].samplerate, 3500.0f, 1.75f);	//4 pole butterworth Q = 0.76537, 1.84776	


    // DaTA FM mode	
	tx[thread].fm.k_preemphasis1 = FMDataPre; // ke9ns (REAL)(1.0f + uni[thread].samplerate / (TWOPI * 3000.0f));  // FMDATA MODE 5= 96k SR
	
	tx[thread].fm.k_deemphasis1 = FMDataDe;   // ke9ns (REAL)(1.0f + uni[thread].samplerate / (TWOPI * 250.0f));  // ke9ns: RX output appears to be working (will receive digital data)

	tx[thread].fm.input_LPF3 = new_IIR_LPF_2P(tx[thread].buf.i, uni[thread].samplerate, FMDataLowHigh, 0.25f);	//4 pole butterworth Q = 0.76537, 1.84776	
	tx[thread].fm.input_LPF4 = new_IIR_LPF_2P(tx[thread].buf.i, uni[thread].samplerate, FMDataLowHigh, 1.75f);	//4 pole butterworth Q = 0.76537, 1.84776
	tx[thread].fm.input_HPF3 = new_IIR_HPF_2P(tx[thread].buf.i, uni[thread].samplerate, FMDataLow, 0.34f);	//4 pole butterworth Q = 0.76537, 1.84776	
	tx[thread].fm.input_HPF4 = new_IIR_HPF_2P(tx[thread].buf.i, uni[thread].samplerate, FMDataLow, 0.94f);	//4 pole butterworth Q = 0.76537, 1.84776	

	// DATA FM MODE TX OUTPUT Filter
	tx[thread].fm.output_LPF3 = new_IIR_LPF_2P(tx[thread].buf.i, uni[thread].samplerate, FMDataLowHigh, 0.25f);	//4 pole butterworth Q = 0.76537, 1.84776	
	tx[thread].fm.output_LPF4 = new_IIR_LPF_2P(tx[thread].buf.i, uni[thread].samplerate, FMDataLowHigh, 1.75f);	//4 pole butterworth Q = 0.76537, 1.84776	

	
	//fprintf(stderr, "1FMDATA OFF\n"), fflush(stderr);

	tx[thread].fm.preemphasis_filter = 0.0f; // ke9ns: this value is just a placeholder, see  do_tx_fm() where the value is based on k_preemphasis 
	tx[thread].fm.deemphasis_out = 0.0f;

	tx[thread].fm.clip_threshold = 0.75f;

	
	
	tx[thread].fm.ctcss.flag = FALSE;
	tx[thread].fm.ctcss.amp = .13f;
	tx[thread].fm.ctcss.freq_hz = 100.0;
	tx[thread].fm.ctcss.osc = newOSC (uni[thread].buflen, ComplexTone, 100.0, 0.0, uni[thread].samplerate, "SDR TX CTTS Oscillator");



	tx[thread].leveler.gen = newDttSPAgc (
		1,							// mode kept around for control reasons
		CXBbase (tx[thread].buf.i),	// input buffer
		CXBsize (tx[thread].buf.i),	// output buffer
		1.0f,						// Target output
		2,							// Attack time constant in ms
		500,						// Decay time constant in ms
		1,							// Slope
		500,						// Hangtime in ms
		uni[thread].samplerate,		// Sample rate
		1.778f,						// Maximum gain as a multipler, linear not dB
		1.0,						// Minimum gain as a multipler, linear not dB
		1.0,						// Set the current gain
		"LVL");						// Set a tag for an error message if the memory allocation fails
	tx[thread].leveler.flag = TRUE;

	tx[thread].grapheq.gen = new_EQ (tx[thread].buf.i, uni[thread].samplerate, uni[thread].wisdom.bits);
	tx[thread].grapheq.flag = FALSE;

	memset ((char *) &tx[thread].squelch, 0, sizeof (tx[thread].squelch));
	tx[thread].squelch.thresh = -40.0;
	tx[thread].squelch.atten = 80.0;
	tx[thread].squelch.power = 0.0;
	tx[thread].squelch.flag = FALSE;
	tx[thread].squelch.running = tx[thread].squelch.set = FALSE;
	tx[thread].squelch.num = uni[thread].buflen - 48;

	tx[thread].alc.gen = newDttSPAgc (
		1,								// mode kept around for control reasons alone
		CXBbase (tx[thread].buf.o),		// input buffer
		CXBsize (tx[thread].buf.o),		// output buffer
		1.08f,							// Target output
		2,								// Attack time constant in ms
		10,								// Decay time constant in ms
		1,								// Slope
		500,							// Hangtime in ms
		uni[thread].samplerate, 1.0,	// Maximum gain as a multipler, linear not dB
		.000001f,						// Minimum gain as a multipler, linear not dB
		1.0,							// Set the current gain
		"ALC");							// Set a tag for an error message if the memory allocation fails
	tx[thread].alc.flag = TRUE;

	tx[thread].spr.gen =
		newSpeechProc (0.4f, 3.0, CXBbase (tx[thread].buf.i), CXBsize (tx[thread].buf.o));
	tx[thread].spr.flag = FALSE;

	tx[thread].cpd.gen = newWSCompander (uni[thread].cpdlen, (REAL)-0.1, tx[thread].buf.i);
	tx[thread].cpd.flag = FALSE;

	tx[thread].hlb.gen = newHilbertsim(tx[thread].buf.i, tx[thread].buf.i);
	tx[thread].hlb.flag = TRUE;

	//tx[thread].scl.dc = cxzero;

	tx[thread].mode = uni[thread].mode.sdr;

	tx[thread].tick = 0;


	/* not much else to do for TX */

} // setup_tx


  
  /* how the outside world sees it */

void setup_workspace (REAL rate, int buflen, SDRMODE mode, char *wisdom, int specsize, int numrecv, int cpdsize, unsigned int thread)
{
	int k;

	setup_all (rate, buflen, mode, wisdom, specsize, numrecv, cpdsize, thread);

	for (k = 0; k < uni[thread].multirx.nrx; k++)
	{
		setup_rx (k, thread);
		uni[thread].multirx.act[k] = FALSE;
	}
	uni[thread].multirx.act[0] = TRUE;
	uni[thread].multirx.nac = 1;

	setup_tx (thread);

} // setup_workspace

void
destroy_workspace (unsigned int thread)
{
	int i, k;


	/* TX */
	delHilsim(tx[thread].hlb.gen);
	delWSCompander (tx[thread].cpd.gen);
	delSpeechProc (tx[thread].spr.gen);
	delDttSPAgc (tx[thread].leveler.gen);
	delDttSPAgc (tx[thread].alc.gen);
	delOSC (tx[thread].osc.gen);
	delDCBlocker (tx[thread].dcb.gen);
	delvec_COMPLEX (tx[thread].filt.save);
	delFiltOvSv (tx[thread].filt.ovsv);
	delFIR_Bandpass_COMPLEX (tx[thread].filt.coef);
	delCorrectIQ (tx[thread].iqfix);
	delCXB (tx[thread].buf.o);
	delCXB (tx[thread].buf.i);

	// Delete preemphasis and pinching filters
	del_IIR_LPF_2P(tx[thread].fm.output_LPF1);
	del_IIR_LPF_2P(tx[thread].fm.output_LPF2);

	del_IIR_LPF_2P(tx[thread].fm.output_LPF3); // ke9ns add
	del_IIR_LPF_2P(tx[thread].fm.output_LPF4);

	del_IIR_LPF_2P(tx[thread].fm.input_LPF1); 	// 
	del_IIR_LPF_2P(tx[thread].fm.input_LPF2);
	del_IIR_HPF_2P(tx[thread].fm.input_HPF1);	
	del_IIR_HPF_2P(tx[thread].fm.input_HPF2);

	del_IIR_LPF_2P(tx[thread].fm.input_LPF3); // ke9ns add does a free() on the memory
	del_IIR_LPF_2P(tx[thread].fm.input_LPF4);
	del_IIR_HPF_2P(tx[thread].fm.input_HPF3);
	del_IIR_HPF_2P(tx[thread].fm.input_HPF4);

	/* RX */
	for (k = 0; k < uni[thread].multirx.nrx; k++)
	{
		delWSCompander (rx[thread][k].cpd.gen);
		delSpotToneGen (rx[thread][k].spot.gen);
		delDttSPAgc (rx[thread][k].dttspagc.gen);
		del_nb (rx[thread][k].nb_sdrom.gen);
		del_nb (rx[thread][k].nb.gen);
		del_lmsr (rx[thread][k].anf.gen);
		del_lmsr (rx[thread][k].anr.gen);
		delAMD (rx[thread][k].am.gen);
		delFMD (rx[thread][k].fm.gen);
		delOSC (rx[thread][k].osc.gen);
		delvec_COMPLEX (rx[thread][k].filt.save);
		delFiltOvSv (rx[thread][k].filt.ovsv);
		delFIR_Bandpass_COMPLEX (rx[thread][k].filt.coef);
		for(i=0; i<MAX_NOTCHES_IN_PASSBAND; i++)
			del_IIR_2P2Z(rx[thread][k].notch[i].gen);
		delCorrectIQ (rx[thread][k].iqfix);
		delCXB (rx[thread][k].buf.o);
		delCXB (rx[thread][k].buf.i);
	}

	/* all */
	finish_spectrum (&uni[thread].spec);
	//fprintf(stderr,"Destroyed spectrum\n"),fflush(stderr);
}

//////////////////////////////////////////////////////////////////////////
// execution
//////////////////////////////////////////////////////////////////////////

//========================================================================
// util

PRIVATE void
CXBscl (CXB buff, REAL scl)
{
	int i;
	for (i = 0; i < CXBhave (buff); i++)
		CXBdata (buff, i) = Cscl (CXBdata (buff, i), scl);
}

PRIVATE REAL
CXBnorm (CXB buff)
{
	int i;
	REAL sum = 0.0;
	for (i = 0; i < CXBhave (buff); i++)
		sum += Csqrmag (CXBdata (buff, i));
	return (REAL) sqrt (sum);
}

PRIVATE REAL
CXBnormsqr (CXB buff)
{
	int i;
	REAL sum = 0.0;
	for (i = 0; i < CXBhave (buff); i++)
		sum += Csqrmag (CXBdata (buff, i));
	return (REAL) (sum);
}

PRIVATE REAL
CXBpeak (CXB buff)
{
	int i;
	REAL maxsam = 0.0;
	for (i = 0; i < CXBhave (buff); i++)
		maxsam = max (Cmag (CXBdata (buff, i)), maxsam);
	return maxsam;
}

PRIVATE REAL peakl(CXB buff)
{
	int i;
	REAL maxpwr = 0.0;
	for(i=0; i<CXBhave(buff); i++)
		maxpwr = max(CXBreal(buff, i), maxpwr);
	return maxpwr;
}

PRIVATE REAL peakr(CXB buff)
{
	int i;
	REAL maxpwr = 0.0;
	for(i=0; i<CXBhave(buff); i++)
		maxpwr = max(CXBimag(buff, i), maxpwr);
	return maxpwr;
}

PRIVATE REAL
CXBpeakpwr (CXB buff)
{
	int i;
	REAL maxpwr = 0.0;
	for (i = 0; i < CXBhave (buff); i++)
		maxpwr = max (Csqrmag (CXBdata (buff, i)), maxpwr);
	return maxpwr;
}

//========================================================================
/* all */

// unfortunate duplication here, due to
// multirx vs monotx

PRIVATE void
do_rx_meter (int k, unsigned int thread, CXB buf, int tap)
{
	COMPLEX *vec = CXBbase (buf);
	int i, len = CXBhave (buf);
	REAL tmp;

	switch (tap)
	{
		case RXMETER_PRE_CONV:
			tmp = -10000.0f;
			for (i = 0; i < len; i++)
				tmp = (REAL) max (fabs (vec[i].re), tmp);
			//fprintf(stderr, "adc_r max: %f\n", uni[thread].meter.rx.val[k][ADC_REAL]), fflush(stderr);
			uni[thread].meter.rx.val[k][ADC_REAL] = (REAL) (20.0 * log10 (tmp + 1e-10));
			tmp = -10000.0f;
			for (i = 0; i < len; i++)
				tmp = (REAL) max (fabs (vec[i].im), tmp);
			uni[thread].meter.rx.val[k][ADC_IMAG] = (REAL) (20.0 * log10 (tmp + 1e-10));
			break;
		case RXMETER_POST_FILT:
			tmp = 0;
			for (i = 0; i < len; i++)
				tmp += Csqrmag (vec[i]);
			rx[thread][k].norm = tmp / (REAL) len;
			uni[thread].meter.rx.val[k][SIGNAL_STRENGTH] =
				(REAL) (10.0 * log10 (tmp + 1e-20));
			if (uni[thread].meter.rx.mode[k] == SIGNAL_STRENGTH)
				uni[thread].meter.rx.val[k][AVG_SIGNAL_STRENGTH] = uni[thread].meter.rx.val[k][SIGNAL_STRENGTH];
			tmp = uni[thread].meter.rx.val[k][AVG_SIGNAL_STRENGTH];
			uni[thread].meter.rx.val[k][AVG_SIGNAL_STRENGTH] =
				(REAL) (0.95 * tmp +
				0.05 *uni[thread].meter.rx.val[k][SIGNAL_STRENGTH]);
			break;
		case RXMETER_POST_AGC:
			uni[thread].meter.rx.val[k][AGC_GAIN] =
				(REAL) (20.0 * log10 (rx[thread][k].dttspagc.gen->gain.now + 1e-10));
			//fprintf(stdout, "rx gain: %15.12f\n", uni[thread].meter.rx.val[k][AGC_GAIN]);
			//fflush(stdout);
			break;
		default:
			break;
	}
}


PRIVATE void
do_rx_spectrum (int k, unsigned int thread, CXB buf, int type)
{
	if (uni[thread].spec.flag && k == uni[thread].spec.rxk && type == uni[thread].spec.type)
	{
		if ((uni[thread].spec.type == SPEC_POST_DET) && (!rx[thread][k].bin.flag)) 
		{
			int i;
			for (i=0; i<CXBhave(rx[thread][k].buf.o);i++)
				CXBdata(uni[thread].spec.accum, uni[thread].spec.fill+i) = Cmplx(CXBreal(rx[thread][k].buf.o, i)*1.414f, 0.0);
		}
		else
		{
			memcpy ((char *) &CXBdata (uni[thread].spec.accum, uni[thread].spec.fill),
				(char *) CXBbase (buf), CXBsize (buf) * sizeof (COMPLEX));
		}
		uni[thread].spec.fill = (uni[thread].spec.fill + CXBsize (buf)) & uni[thread].spec.mask;
	}
}

PRIVATE void
do_tx_spectrum (unsigned int thread, CXB buf)
{
	if (uni[thread].spec.type == SPEC_PREMOD) 
	{
		int i;
		for (i=0; i<CXBhave(tx[thread].buf.i);i++)
			CXBdata(uni[thread].spec.accum, uni[thread].spec.fill+i) = Cmplx(CXBreal(tx[thread].buf.i, i), 0.0);
	}
	else
	{
		memcpy ((char *) &CXBdata (uni[thread].spec.accum, uni[thread].spec.fill),
			(char *) CXBbase (buf), CXBsize (buf) * sizeof (COMPLEX));
	}
	uni[thread].spec.fill = (uni[thread].spec.fill + CXBsize (buf)) & uni[thread].spec.mask;
}

//========================================================================
/* RX processing */

PRIVATE void
should_do_rx_squelch (int k, unsigned int thread)
{
	if (rx[thread][k].squelch.flag)
	{
		int i, n = CXBhave (rx[thread][k].buf.o);
		rx[thread][k].squelch.power = 0.0;

		for (i = 0; i < n; i++)
			rx[thread][k].squelch.power += Csqrmag (CXBdata (rx[thread][k].buf.o, i));

		if(10.0 * log10 (rx[thread][k].squelch.power + 1e-17) < rx[thread][k].squelch.thresh)
			rx[thread][k].squelch.set = TRUE;
		else
			rx[thread][k].squelch.set = FALSE;
	}
	else
	{
		rx[thread][k].squelch.set = FALSE;
	}
}

PRIVATE void
should_do_tx_squelch (unsigned int thread)
{
	if (tx[thread].squelch.flag)
	{
		int i, n = CXBsize (tx[thread].buf.i);
		tx[thread].squelch.power = 0.0;

		for (i = 0; i < n; i++)
			tx[thread].squelch.power += Csqrmag (CXBdata (tx[thread].buf.i, i));

		if((-30 + 10.0 * log10 (tx[thread].squelch.power + 1e-17)) < tx[thread].squelch.thresh)
			tx[thread].squelch.set = TRUE;
		else
			tx[thread].squelch.set = FALSE;

	}
	else
	{
		tx[thread].squelch.set = FALSE;
	}
}

// apply squelch
// slew into silence first time

PRIVATE void
do_squelch (int k, unsigned int thread)
{
	if (!rx[thread][k].squelch.running)
	{
		int i, m = rx[thread][k].squelch.num, n = CXBhave (rx[thread][k].buf.o) - m;

		for (i = 0; i < m; i++)
		{
			CXBdata (rx[thread][k].buf.o, i) =
				Cscl (CXBdata (rx[thread][k].buf.o, i), (REAL) (1.0 - (REAL) i / m));
		}

		memset ((void *) (CXBbase (rx[thread][k].buf.o) + m), 0, n * sizeof (COMPLEX));
		rx[thread][k].squelch.running = TRUE;
	}
	else
	{
		memset ((void *) CXBbase (rx[thread][k].buf.o),
			0, CXBhave (rx[thread][k].buf.o) * sizeof (COMPLEX));
	}
}

PRIVATE void
do_tx_squelch (unsigned int thread)
{
	int i, m = tx[thread].squelch.num, n = CXBhave (tx[thread].buf.i);
	int l = ((int)tx[thread].squelch.atten * m) / 100;

	if (!tx[thread].squelch.running)
	{
		for (i = 0; i < n; i++)
		{
			REAL scale = (REAL) (1.0 - (REAL) (i < l ? i : l) / m);
			CXBdata (tx[thread].buf.i, i) =
				Cscl (CXBdata (tx[thread].buf.i, i), scale);
		}
		tx[thread].squelch.running = TRUE;
	}
	else if (l != m)
	{
		REAL scale = (REAL) (1.0 - (REAL) l / m);
		for (i = 0; i < n; i++)
		{
			CXBdata (tx[thread].buf.i, i) =
				Cscl (CXBdata (tx[thread].buf.i, i), scale);
		}
	}
	else
	{
		memset ((void *) CXBbase (tx[thread].buf.i),
			0, CXBhave (tx[thread].buf.i) * sizeof (COMPLEX));
	}
}

// lift squelch
// slew out from silence to full scale

PRIVATE void
no_squelch (int k, unsigned int thread)
{
	if (rx[thread][k].squelch.running)
	{
		int i, m = rx[thread][k].squelch.num;

		for (i = 0; i < m; i++)
		{
			CXBdata (rx[thread][k].buf.o, i) =
				Cscl (CXBdata (rx[thread][k].buf.o, i), (REAL) i / m);
		}
		rx[thread][k].squelch.running = FALSE;
	}
}

PRIVATE void
no_tx_squelch (unsigned int thread)
{
	int i, m = tx[thread].squelch.num, n = CXBhave (tx[thread].buf.i);
	int l = (((INT) tx[thread].squelch.atten) * m) / 100;

	if (tx[thread].squelch.running)
	{
		for (i = 0; i < m; i++)
		{
			REAL scale = (REAL) (i < l ? l : i) / m;
			CXBdata (tx[thread].buf.i, i) =
				Cscl (CXBdata (tx[thread].buf.i, i), scale);
		}
		tx[thread].squelch.running = FALSE;
	}
}
/* Routine to do the actual adding of buffers through the complex linear combination required */

#if 0
void
do_rx_diversity_combine()
{
	int i, n=CXBhave (rx[0][0].buf.i);
	for (i=0;i<n;i++)
	{
		CXBdata(rx[0][0].buf.i,i) = Cscl(Cadd(CXBdata(rx[0][0].buf.i,i),Cmul(CXBdata(rx[2][0].buf.i,i),diversity.scalar)),diversity.gain);
	}
}
#endif
/* pre-condition for (nearly) all RX modes */
PRIVATE void do_rx_pre (int k, unsigned int thread)
{
	int i, n = min (CXBhave (rx[thread][k].buf.i), uni[thread].buflen);

	// metering for uncorrected values here
	do_rx_meter (k, thread, rx[thread][k].buf.i, RXMETER_PRE_CONV);	

	if (rx[thread][k].dcb->flag) DCBlock(rx[thread][k].dcb);

	if (rx[thread][k].nb.flag)
	{
		noiseblanker(rx[thread][k].nb.gen); // ke9ns: NB if ON call 
	}
	if (rx[thread][k].nb_sdrom.flag)	SDROMnoiseblanker (rx[thread][k].nb_sdrom.gen); // ke9ns: NB2

	correctIQ (rx[thread][k].buf.i, rx[thread][k].iqfix, FALSE, k);

	/* 2nd IF conversion happens here */
	if (rx[thread][k].osc.gen->Frequency != 0.0)
	{
		ComplexOSC (rx[thread][k].osc.gen);
		for (i = 0; i < n; i++)
			CXBdata (rx[thread][k].buf.i, i) = Cmul (CXBdata (rx[thread][k].buf.i, i),
			OSCCdata (rx[thread][k].osc.gen, i));
	}

	/* filtering, metering, spectrum, squelch, & AGC */

	//do_rx_meter (k, rx[thread][k].buf.i, RXMETER_PRE_FILT);
	do_rx_spectrum (k, thread, rx[thread][k].buf.i, SPEC_PRE_FILT);
	
	if (rx[thread][k].mode != SPEC)
	{
		if (rx[thread][k].resample.flag) {
			PolyPhaseFIRF(rx[thread][k].resample.gen1r);
			PolyPhaseFIRF(rx[thread][k].resample.gen1i);
		}
		if (rx[thread][k].tick == 0)
			reset_OvSv (rx[thread][k].filt.ovsv);

		filter_OvSv (rx[thread][k].filt.ovsv);
	}
	else
	{
		memcpy (CXBbase (rx[thread][k].buf.o), CXBbase (rx[thread][k].buf.i),
			sizeof (COMPLEX) * CXBhave (rx[thread][k].buf.i));
	}
    
	CXBhave (rx[thread][k].buf.o) = CXBhave (rx[thread][k].buf.i);

	do_rx_meter (k, thread, rx[thread][k].buf.o, RXMETER_POST_FILT);
	do_rx_spectrum (k, thread, rx[thread][k].buf.o, SPEC_POST_FILT);

	if (rx[thread][k].cpd.flag)
		WSCompand (rx[thread][k].cpd.gen);

	should_do_rx_squelch (k, thread);

}

static int count = 0;

PRIVATE void
do_rx_post (int k, unsigned int thread)
{
	int i, n = CXBhave (rx[thread][k].buf.o);

	if(rx[thread][k].mode != FM)
	{
		if(rx[thread][k].squelch.set)
		{
			do_squelch (k, thread);
		}
		else no_squelch (k, thread);
	}

	if (rx[thread][k].grapheq.flag)
	{
		switch(rx[thread][k].mode)
		{
			case DRM:
			case DIGL:
			case DIGU: // do nothing in digital modes
				break;
			default:
				graphiceq (rx[thread][k].grapheq.gen);
				break;
		}
	}

	do_rx_spectrum(k, thread, rx[thread][k].buf.o, SPEC_POST_DET);
	// not binaural?
	// position in stereo field

	for(i=0; i<MAX_NOTCHES_IN_PASSBAND; i++)
	{

		if (rx[thread][k].notch[i].flag)
			do_IIR_2P2Z(rx[thread][k].notch[i].gen);
	}
			

	if (rx[thread][k].anf.flag)
	{
		switch(rx[thread][k].mode)
		{
			case DRM:
			case DIGL:
			case DIGU:
			case CWL:
			case CWU: // do nothing
				break;
			default:
				lmsr_adapt (rx[thread][k].anf.gen);
				//blms_adapt(rx[thread][k].banf.gen);
				break;
		}
	}

	if (rx[thread][k].anr.flag)
		lmsr_adapt (rx[thread][k].anr.gen);
		//blms_adapt(rx[thread][k].banr.gen);

	/*if(thread == 0 && k == 0)
		fprintf(stdout, "before: %15f12  ", CXBpeak(rx[thread][k].buf.i));*/
#if 0
	if (diversity.flag && (k==0) && (thread==2))
		for (i = 0; i < n; i++) CXBdata(rx[thread][k].buf.o,i) = cxzero;
	else 
#endif

	if(rx[thread][k].mode != FM)
		DttSPAgc (rx[thread][k].dttspagc.gen, rx[thread][k].tick);
	
	/*if(thread == 0 && k == 0 && count++%50 == 0)
	{
		fprintf(stdout, "after: %15f12\n", CXBpeak(rx[thread][k].buf.o));
		fflush(stdout);
	}*/

	/*if(thread == 0 && k == 0)
	{
		fprintf(stdout, "pll freq: %.3f\n", rx[thread][k].fm.gen->pll.freq.f);
		fflush(stdout);
	}*/

	do_rx_meter(k, thread, rx[thread][k].buf.o, RXMETER_POST_AGC);
	do_rx_spectrum (k, thread, rx[thread][k].buf.o, SPEC_POST_AGC);

	if (!rx[thread][k].bin.flag)
	{
		for (i = 0; i < CXBhave (rx[thread][k].buf.o); i++)
			CXBimag (rx[thread][k].buf.o, i) = CXBreal (rx[thread][k].buf.o, i);
	}

	if(uni[thread].multirx.nac == 1)
	{
		for (i = 0; i < n; i++)
			CXBdata(rx[thread][k].buf.o, i) = Cscl(Cmplx(rx[thread][k].azim.re*CXBreal(rx[thread][k].buf.o, i),
														 rx[thread][k].azim.im*CXBimag(rx[thread][k].buf.o, i)), 1.414f);
	}
	else
	{
		for (i = 0; i < n; i++)
			CXBdata(rx[thread][k].buf.o, i) = Cmplx(rx[thread][k].azim.re * CXBreal(rx[thread][k].buf.o, i),
													rx[thread][k].azim.im * CXBimag(rx[thread][k].buf.o, i));
	}

	if ((thread == 2) && (diversity.flag))
	{
		for (i=0;i< n; i++) 
			CXBdata(rx[thread][k].buf.o,i) = cxzero;
	}
	else
	{
		if (rx[thread][k].output_gain != 1.0)
		{
			for (i = 0; i < n; i++) 
				CXBdata(rx[thread][k].buf.o,i) = Cscl(CXBdata(rx[thread][k].buf.o,i),rx[thread][k].output_gain);
		}
	}

	if (rx[thread][k].resample.flag) 
	{
		PolyPhaseFIRF(rx[thread][k].resample.gen2r);
		PolyPhaseFIRF(rx[thread][k].resample.gen2i);
	}

}

/* demod processing */

PRIVATE void do_rx_SBCW (int k, unsigned int thread)
{

}

PRIVATE void do_rx_AM (int k, unsigned int thread)
{
	AMDemod (rx[thread][k].am.gen);
}

PRIVATE void do_rx_FM (int k, unsigned int thread)
{
	FMDemod (rx[thread][k].fm.gen);
}

PRIVATE void do_rx_DRM (int k, unsigned int thread)
{

}

PRIVATE void do_rx_SPEC (int k, unsigned int thread)
{

}

PRIVATE void do_rx_NIL (int k, unsigned int thread)
{
	int i, n = min (CXBhave (rx[thread][k].buf.i), uni[thread].buflen);
	for (i = 0; i < n; i++)
		CXBdata (rx[thread][k].buf.o, i) = cxzero;
}

/* overall dispatch for RX processing */

void dump_buf (const char* filename, CXB buf)
{
	int n = CXBsize(buf);
	int i = 0;

	unsigned int temp = 0;
	unsigned short temp2 = 0;

	FILE* file = fopen(filename, "w");
	fprintf(file, "RIFF");
	
	temp = 36 + 4*2*n;
	fwrite((void*)&temp, sizeof(unsigned int), 1, file);

	fprintf(file, "WAVE");
	fprintf(file, "fmt ");
	
	temp = 16; // size of fmt chunk
	fwrite((void*)&temp, sizeof(unsigned int), 1, file);
	
	temp2 = 3; // FormatTab -- 3 for float
	fwrite((void*)&temp2, sizeof(unsigned short), 1, file);

	temp2 = 2; // wChannels
	fwrite((void*)&temp2, sizeof(unsigned short), 1, file);

	temp = (unsigned int)uni[0].samplerate; // dwSamplesPerSec
	fwrite((void*)&temp, sizeof(unsigned int), 1, file);

	temp = 2 * (unsigned int)uni[0].samplerate * 4; // dwAvgBytesPerSec
	fwrite((void*)&temp, sizeof(unsigned int), 1, file);

	temp2 = 2 * 4; // wblockAlign
	fwrite((void*)&temp2, sizeof(unsigned short), 1, file);

	temp2 = 32; // wBitsPerSample
	fwrite((void*)&temp2, sizeof(unsigned short), 1, file);

	fprintf(file, "data");

	temp = 8*n;
	fwrite((void*)&temp, sizeof(unsigned int), 1, file);

	fwrite((void*)buf->data, sizeof(REAL), n*2, file);
	/*for(i=0; i<n; i++)
	{
		fwrite((void*)&CXBreal(buf, i), sizeof(REAL), 1, file); fflush(file);
		fwrite((void*)&CXBimag(buf, i), sizeof(REAL), 1, file); fflush(file);
	}*/

	fflush(file);
	fclose(file);

	/*writer.Write(0x46464952);								// "RIFF"		-- descriptor chunk ID
			writer.Write(data_length + 36);							// size of whole file -- 1 for now
			writer.Write(0x45564157);								// "WAVE"		-- descriptor type
			writer.Write(0x20746d66);								// "fmt "		-- format chunk ID
			writer.Write((int)16);									// size of fmt chunk
			writer.Write((short)3);									// FormatTag	-- 3 for floats
			writer.Write(channels);									// wChannels
			writer.Write(sample_rate);								// dwSamplesPerSec
			writer.Write((int)(channels*sample_rate*bit_depth/8));	// dwAvgBytesPerSec
			writer.Write((short)(channels*bit_depth/8));			// wBlockAlign
			writer.Write(bit_depth);								// wBitsPerSample
			writer.Write(0x61746164);								// "data" -- data chunk ID
			writer.Write(data_length);								// chunkSize = length of data
			writer.Flush();											// write the file*/
}

PRIVATE void do_rx (int k, unsigned int thread)
{
	do_rx_pre (k, thread);
	switch (rx[thread][k].mode)
	{
		case DIGU:
		case DIGL:
		case USB:
		case LSB:
		case CWU:
		case CWL:
		case DSB:
			do_rx_SBCW (k, thread);
			break;
		case AM:
		case SAM:
			do_rx_AM (k, thread);
			break;
		case FM:
			do_rx_FM (k, thread);
			break;
		case DRM:
			do_rx_DRM (k, thread);
			break;
		case SPEC:
		default:
			do_rx_SPEC (k, thread);
			break;
	}
	do_rx_post (k, thread);
}

//==============================================================
/* TX processing */
PRIVATE REAL mic_avg = 0.0f, mic_pk = 0.0f,
	alc_avg = 0.0f, alc_pk = 0.0f,
	lev_avg = 0.0f, lev_pk = 0.0f,
	eq_avg = 0.0f, eq_pk = 0.0f,
	comp_avg = 0.0f, comp_pk = 0.0f,
	cpdr_avg = 0.0f, cpdr_pk = 0.0f;

/* pre-condition for (nearly) all TX modes */
PRIVATE REAL peaksmooth = 0.0;
PRIVATE void
do_tx_meter (unsigned int thread, CXB buf, TXMETERTYPE mt)
{
	COMPLEX *vec = CXBbase (buf);
	int i, len = CXBhave (buf);
	REAL tmp = 0.0f;

	switch (mt)
	{
		case TX_MIC:
			for (i = 0; i < CXBhave (tx[thread].buf.i); i++) // calculate avg Mic
				mic_avg = (REAL) (0.9995 * mic_avg +
				0.0005 * Csqrmag (CXBdata (tx[thread].buf.i, i)));
			uni[thread].meter.tx.val[TX_MIC] = (REAL) (-10.0 * log10 (mic_avg + 1e-16));

			mic_pk = CXBpeak(tx[thread].buf.i);		// calculate peak mic                 
			uni[thread].meter.tx.val[TX_MIC_PK] = (REAL) (-20.0 * log10 (mic_pk + 1e-16));
			break;

		case TX_PWR:
			for (i = 0, tmp = 0.0000001f;
				i < CXBhave (tx[thread].buf.o); i++)
				tmp += Csqrmag (CXBdata (tx[thread].buf.o, i));
			uni[thread].meter.tx.val[TX_PWR] = tmp/(REAL) len;
			break;

		case TX_ALC:
			for (i = 0; i < CXBhave (tx[thread].buf.i); i++)
				alc_avg = (REAL) (0.9995 * alc_avg +
				0.0005 * Csqrmag (CXBdata (tx[thread].buf.o, i)));
			uni[thread].meter.tx.val[TX_ALC] = (REAL) (-10.0 * log10 (alc_avg + 1e-16));

			alc_pk = CXBpeak(tx[thread].buf.o);
			uni[thread].meter.tx.val[TX_ALC_PK] = (REAL) (-20.0 * log10 (alc_pk+ 1e-16));
			uni[thread].meter.tx.val[TX_ALC_G] = (REAL)(20.0*log10(tx[thread].alc.gen->gain.now+1e-16));
			//fprintf(stdout, "pk: %15.12f  comp: %15.12f\n", uni[thread].meter.tx.val[TX_ALC_PK], uni[thread].meter.tx.val[TX_ALC_G]);
			//fflush(stdout);
			break;

		case TX_EQ:
			for (i = 0; i < CXBhave (tx[thread].buf.i); i++)
				eq_avg = (REAL) (0.9995 * eq_avg +
				0.0005 * Csqrmag (CXBdata (tx[thread].buf.i, i)));
			uni[thread].meter.tx.val[TX_EQ] = (REAL) (-10.0 * log10 (eq_avg + 1e-16));

			eq_pk = CXBpeak(tx[thread].buf.i);
			uni[thread].meter.tx.val[TX_EQ_PK] = (REAL) (-20.0 * log10 (eq_pk + 1e-16));
			break;

		case TX_LVL:
			for (i = 0; i < CXBhave (tx[thread].buf.i); i++)
				lev_avg = (REAL) (0.9995 * lev_avg +
				0.0005 * Csqrmag (CXBdata (tx[thread].buf.i, i)));
			uni[thread].meter.tx.val[TX_LVL] = (REAL) (-10.0 * log10 (lev_avg + 1e-16));

			lev_pk = CXBpeak(tx[thread].buf.i);
			uni[thread].meter.tx.val[TX_LVL_PK] = (REAL) (-20.0 * log10 (lev_pk + 1e-16));
			uni[thread].meter.tx.val[TX_LVL_G] = (REAL)(20.0*log10(tx[thread].leveler.gen->gain.now + 1e-16));
			break;

		case TX_COMP:
			for (i = 0; i < CXBhave (tx[thread].buf.i); i++)
				comp_avg = (REAL) (0.9995 * comp_avg +
				0.0005 * Csqrmag (CXBdata (tx[thread].buf.i, i)));
			uni[thread].meter.tx.val[TX_COMP] = (REAL) (-10.0 * log10 (comp_avg + 1e-16));

			comp_pk = CXBpeak(tx[thread].buf.i);
			uni[thread].meter.tx.val[TX_COMP_PK] = (REAL) (-20.0 * log10 (comp_pk + 1e-16));
			break;

		case TX_CPDR:
			for (i = 0; i < CXBhave (tx[thread].buf.i); i++)
				cpdr_avg = (REAL) (0.9995 * cpdr_avg +
				0.0005 * Csqrmag (CXBdata (tx[thread].buf.i, i)));
			uni[thread].meter.tx.val[TX_CPDR] = (REAL) (-10.0 * log10 (cpdr_avg + 1e-16));

			cpdr_pk = CXBpeak(tx[thread].buf.i);
			uni[thread].meter.tx.val[TX_CPDR_PK] = (REAL) (-20.0 * log10 (cpdr_pk + 1e-16));
			break;

		default:
			break;
	}
}

PRIVATE void
do_tx_pre (unsigned int thread)
{
	int i, n = CXBhave (tx[thread].buf.i);
	for (i = 0; i < n; i++)
		CXBdata (tx[thread].buf.i, i) = Cmplx (CXBimag (tx[thread].buf.i, i), 0.0);
	//hilsim_transform(tx[thread].hlb.gen);
//	fprintf(stderr,"Peak value = %f\n",CXBpeakpwr(tx[thread].buf.i));
	if (tx[thread].dcb.flag)
		DCBlock (tx[thread].dcb.gen);

	do_tx_meter (thread, tx[thread].buf.i, TX_MIC);
	//fprintf(stderr,"[%.2f,%.2f]  ", peakl(tx[thread].buf.i), peakr(tx[thread].buf.i));

	should_do_tx_squelch(thread);
	if(tx[thread].squelch.set)
		do_tx_squelch (thread);
	else //if (!tx[thread].squelch.set)
		no_tx_squelch (thread);

	switch(tx[thread].mode)
	{
		case DIGU:
		case DIGL:
		case DRM:
			do_tx_meter (thread, tx[thread].buf.i, TX_EQ);
			do_tx_meter (thread, tx[thread].buf.i, TX_LVL);
			do_tx_meter (thread, tx[thread].buf.i, TX_COMP);

			//if (tx[thread].alc.flag)
			//	DttSPAgc (tx[thread].alc.gen, tx[thread].tick);
			//do_tx_meter (thread, tx[thread].buf.i, TX_ALC);

			do_tx_meter (thread, tx[thread].buf.i, TX_CPDR);
			break;
		default:
			if (tx[thread].grapheq.flag)
				graphiceq (tx[thread].grapheq.gen);
			do_tx_meter (thread, tx[thread].buf.i, TX_EQ);
			//fprintf(stderr,"[%.2f,%.2f]  ", peakl(tx[thread].buf.i), peakr(tx[thread].buf.i));

			if (tx[thread].leveler.flag)
				DttSPAgc (tx[thread].leveler.gen, tx[thread].tick);						
			do_tx_meter (thread, tx[thread].buf.i, TX_LVL);
			//fprintf(stderr,"[%.2f,%.2f]  ", peakl(tx[thread].buf.i), peakr(tx[thread].buf.i));

			//if (tx[thread].alc.flag)
			//	DttSPAgc (tx[thread].alc.gen, tx[thread].tick);
			//do_tx_meter (thread, tx[thread].buf.i, TX_ALC);
			//fprintf(stderr,"[%.2f,%.2f]  ", peakl(tx[thread].buf.i), peakr(tx[thread].buf.i));

			if (tx[thread].spr.flag)
				SpeechProcessor (tx[thread].spr.gen);						
			do_tx_meter (thread, tx[thread].buf.i, TX_COMP);
			//fprintf(stderr,"[%.2f,%.2f]  ", peakl(tx[thread].buf.i), peakr(tx[thread].buf.i));

			if(tx[thread].mode != FM)
			{
				if (tx[thread].cpd.flag)
					WSCompand (tx[thread].cpd.gen);		
				do_tx_meter (thread, tx[thread].buf.i, TX_CPDR);
			}
			
			//fprintf(stderr,"[%.2f,%.2f]  ", peakl(tx[thread].buf.i), peakr(tx[thread].buf.i));

			break;						
	}
}

PRIVATE void
do_tx_post (unsigned int thread)
{
	CXBhave (tx[thread].buf.o) = CXBhave (tx[thread].buf.i);

	if (tx[thread].tick == 0)
		reset_OvSv (tx[thread].filt.ovsv);

	//fprintf(stderr,"[%.2f,%.2f]  ", peakl(tx[thread].buf.i), peakr(tx[thread].buf.i));
	filter_OvSv (tx[thread].filt.ovsv);
	
	//fprintf(stderr,"[%.2f,%.2f]  ", peakl(tx[thread].buf.o), peakr(tx[thread].buf.o));

	if (tx[thread].alc.flag)
		DttSPAgc (tx[thread].alc.gen, tx[thread].tick);
	do_tx_meter (thread, tx[thread].buf.o, TX_ALC);

	if (uni[thread].spec.flag)
		do_tx_spectrum (thread, tx[thread].buf.o);
	
	if (tx[thread].osc.gen->Frequency != 0.0)
	{
		int i;
		ComplexOSC (tx[thread].osc.gen);
		for (i = 0; i < CXBhave (tx[thread].buf.o); i++)
		{
			CXBdata (tx[thread].buf.o, i) =
				Cmul (CXBdata (tx[thread].buf.o, i), OSCCdata (tx[thread].osc.gen, i));
		}
	}

	correctIQ (tx[thread].buf.o, tx[thread].iqfix, TRUE,0);

	// meter modulated signal
	do_tx_meter (thread, tx[thread].buf.o, TX_PWR);

	//fprintf(stderr,"[%.2f,%.2f]  ", peakl(tx[thread].buf.o), peakr(tx[thread].buf.o));
	//fprintf(stderr,"\n");
	//fflush(stderr);
}

/* modulator processing */

PRIVATE void
do_tx_SBCW (unsigned int thread)
{
	int n = min (CXBhave (tx[thread].buf.i), uni[thread].buflen);

	//fprintf(stderr,"[%.2f,%.2f]  ", peakl(tx[thread].buf.i), peakr(tx[thread].buf.i));
	if (tx[thread].mode != DSB)
		CXBscl (tx[thread].buf.i, 2.0f);
}

PRIVATE void do_tx_AM (unsigned int thread)
{
	int i, n = min (CXBhave (tx[thread].buf.i), uni[thread].buflen);
	//fprintf(stderr,"[%.2f,%.2f]  ", peakl(tx[thread].buf.i), peakr(tx[thread].buf.i));

	for (i = 0; i < n; i++)
	{
		CXBdata (tx[thread].buf.i, i) = Cmplx ((REAL)
			(tx[thread].am.carrier_level +
			(1.0f - tx[thread].am.carrier_level) * CXBreal (tx[thread].buf.i, i)), 0.0);
	}
} // do_tx_AM


//=======================================================================================
PRIVATE void do_tx_FM (unsigned int thread) // ke9ns called from do_tx above
{
	int i, n = min (CXBhave (tx[thread].buf.i), uni[thread].buflen);
	REAL clip = 1.0;
	REAL threshold = tx[thread].fm.clip_threshold;
	REAL mag = 0;
	REAL deemphasis_in = 0;
	REAL preemphasis_in = 0;

	REAL k_preemphasis = tx[thread].fm.k_preemphasis; // (REAL)(1.0f + uni[thread].samplerate/(TWOPI * 3000.0f));  //11.19
	REAL k_deemphasis = tx[thread].fm.k_deemphasis;

	
	if (tx[thread].fm.fmdata == TRUE) // ke9ns at this point cvtmod2freq = 10000  (see fm_demod for RX FM mode)
	{ 
		k_preemphasis = tx[thread].fm.k_preemphasis1; // (REAL)(1.0f + uni[thread].samplerate/(TWOPI * 3000.0f));  //11.19
	    k_deemphasis = tx[thread].fm.k_deemphasis1;
		
		do_IIR_LPF_2P(tx[thread].fm.input_LPF3);
		do_IIR_LPF_2P(tx[thread].fm.input_LPF4);
		do_IIR_HPF_2P(tx[thread].fm.input_HPF3);
		do_IIR_HPF_2P(tx[thread].fm.input_HPF4);
	}
	else
	{
		//fprintf(stderr, "3FMDATA OFF\n");
		//Input BPF 300-3000 Hz

		do_IIR_LPF_2P(tx[thread].fm.input_LPF1);
		do_IIR_LPF_2P(tx[thread].fm.input_LPF2);
		do_IIR_HPF_2P(tx[thread].fm.input_HPF1);
		do_IIR_HPF_2P(tx[thread].fm.input_HPF2);
	}

	//fprintf(stderr,"[%.2f,%.2f]  ", peakl(tx[thread].buf.i), peakr(tx[thread].buf.i));

	

		for (i = 0; i < n; i++) // ke9ns length of buffer 4096
		{
			//Preemphasis (high-pass filter)
			//		temp = (1/k)in + (k-1)/k * temp
			//		out = in - temp
			//		k = 1 + Fs/(2*pi*fo)

			if (tx[thread].fm.fmdata == TRUE) // ke9ns: wide data mode, so no Pre-emphasis
			{
				preemphasis_in = CXBreal(tx[thread].buf.i, i);
			//	tx[thread].fm.preemphasis_filter =  (preemphasis_in / k_preemphasis) + (k_preemphasis - 1.0f) / k_preemphasis * tx[thread].fm.preemphasis_filter;
			
			//	CXBreal(tx[thread].buf.i, i) = 3.0f * (preemphasis_in - tx[thread].fm.preemphasis_filter);

				CXBreal(tx[thread].buf.i, i) = 3.0f * preemphasis_in;


			//	if (i > 10 && i < 15) // pre: 11  -0.000016  -0.000016   0.000002
			//	{
				//	fprintf(stderr, "pre: %d  %f  %f %f\n", i, tx[thread].fm.preemphasis_filter, preemphasis_in, (3.0f * (preemphasis_in - tx[thread].fm.preemphasis_filter))), fflush(stderr);
			//	}
			}
			else // ke9ns: standard pre-emphasis
			{
				preemphasis_in = CXBreal(tx[thread].buf.i, i);
				tx[thread].fm.preemphasis_filter = (preemphasis_in / k_preemphasis) + (k_preemphasis - 1.0f) / k_preemphasis * tx[thread].fm.preemphasis_filter;
				CXBreal(tx[thread].buf.i, i) = 3.0f * (preemphasis_in - tx[thread].fm.preemphasis_filter);

			}


			////Soft Clipper
			mag = abs(CXBreal(tx[thread].buf.i, i));

			if (CXBreal(tx[thread].buf.i, i) > threshold) CXBreal(tx[thread].buf.i, i) = (1.0f - threshold) * (1.0f - expf((mag - threshold) / (threshold - 1.0f))) + threshold;
			else if (CXBreal(tx[thread].buf.i, i) < -threshold) CXBreal(tx[thread].buf.i, i) = -((1.0f - threshold) * (1.0f - expf((mag - threshold) / (threshold - 1.0f))) + threshold);

		} // for n loop

	
	
	if (tx[thread].fm.fmdata == TRUE) // cvtmod2freq
	{
		//3000 Hz LPF Output
		do_IIR_LPF_2P(tx[thread].fm.output_LPF3);
		do_IIR_LPF_2P(tx[thread].fm.output_LPF4);
	}
	else
	{
		//3000 Hz LPF Output
		do_IIR_LPF_2P(tx[thread].fm.output_LPF1);
		do_IIR_LPF_2P(tx[thread].fm.output_LPF2);
	}

	if(tx[thread].fm.ctcss.flag)	ComplexOSC(tx[thread].fm.ctcss.osc);  // ke9ns: if CTCSS TONES ON 
	
	for(i = 0; i < n; i++)
	{
		////Demphasis (low-pass filter) -- Not needed in TX
		//deemphasis_in = 4.0 * CXBreal(tx[thread].buf.i, i);
		//tx[thread].fm.deemphasis_out = (deemphasis_in/k_deemphasis) + (k_deemphasis-1.0)/k_deemphasis*tx[thread].fm.deemphasis_out;
		
		if(tx[thread].fm.ctcss.flag) CXBreal(tx[thread].buf.i, i) = CXBreal(tx[thread].buf.i, i) + tx[thread].fm.ctcss.amp*OSCCdata(tx[thread].fm.ctcss.osc, i).re;

		//FM modulator
		tx[thread].fm.phase += CXBreal(tx[thread].buf.i, i) * tx[thread].fm.cvtmod2freq; // ke9ns cvtmod2freq = (deviation * TWOPI / uni[thread].samplerate);

		//tx[thread].fm.phase += tx[thread].fm.deemphasis_out * tx[thread].fm.cvtmod2freq;
		CXBdata (tx[thread].buf.i, i) =	Cmplx ((REAL) cos (tx[thread].fm.phase), (IMAG) sin (tx[thread].fm.phase));


		//tx[thread].fm.phase += CXBreal (tx[thread].buf.i, i) * tx[thread].fm.cvtmod2freq;
		//CXBdata (tx[thread].buf.i, i) =
		//	Cmplx ((REAL) cos (tx[thread].fm.phase), (IMAG) sin (tx[thread].fm.phase));
	}

	do_tx_meter (thread, tx[thread].buf.i, TX_CPDR);

} // do_tx_FM



PRIVATE void do_tx_NIL (thread)
{
	int i, n = min (CXBhave (tx[thread].buf.i), uni[thread].buflen);
	for (i = 0; i < n; i++)
		CXBdata (tx[thread].buf.i, i) = cxzero;
}

/* general TX processing dispatch */

PRIVATE void do_tx (unsigned int thread)
{
	do_tx_pre (thread);

	switch (tx[thread].mode)
	{
		case USB:
		case LSB:
		case CWU:
		case CWL:
		case DIGU:
		case DIGL:
		case DRM:
		case DSB:
			do_tx_SBCW (thread);
			break;
		case AM:
		case SAM:
			do_tx_AM (thread);
			break;
		case FM:
		//	if (tx[thread].fm.fmdata == TRUE) fprintf(stderr,"FM DATA HERE\n"),fflush(stderr);
		//	else  fprintf(stderr, "FM  HERE\n"), fflush(stderr);
			do_tx_FM (thread);
			break;
		case SPEC:
		default:
			do_tx_NIL (thread);
			break;
	}
	do_tx_post (thread);
	//fprintf(stderr,"%f\n",Cmag(CXBdata(tx[thread].buf.o,0))),fflush(stderr);

} // do_tx

//========================================================================
/* overall buffer processing;
   come here when there are buffers to work on */

void
process_samples (float *bufl, float *bufr, float *auxl, float *auxr, int n, unsigned int thread)
{
	int i, k;

	switch (uni[thread].mode.trx)
	{
		case RX:


			// make copies of the input for all receivers
			for (k = 0; k < uni[thread].multirx.nrx; k++)
			{
				BOOLEAN kdone=FALSE;
				int kone = -1;
				if (uni[thread].multirx.act[k])
				{
					if (!kdone) 
					{
						kdone = TRUE;
						kone = k;
						for (i = 0; i < n; i++)
						{
							CXBimag (rx[thread][k].buf.i, i) =
								bufl[i], CXBreal (rx[thread][k].buf.i, i) = bufr[i];
						}
						CXBhave (rx[thread][k].buf.i) = n;
					} 
					else memcpy(rx[thread][k].buf.i,rx[thread][kone].buf.i,CXBhave(rx[thread][kdone].buf.i)*sizeof(COMPLEX));
				}
			}
			
			// prepare buffers for mixing
			memset ((char *) bufl, 0, n * sizeof (float));
			memset ((char *) bufr, 0, n * sizeof (float));

			// run all receivers
			for (k = 0; k < uni[thread].multirx.nrx; k++)
			{
                if (uni[thread].multirx.act[k])
				{
					do_rx (k, thread), rx[thread][k].tick++;
					// mix
					for (i = 0; i < n; i++)
					{
						bufl[i] += (float) CXBimag (rx[thread][k].buf.o, i);
						bufr[i] += (float) CXBreal (rx[thread][k].buf.o, i);
					}
					CXBhave (rx[thread][k].buf.o) = n;
				}
			}

			// late mixing of aux buffers
#if 0
			if (uni[thread].mix.rx.flag)
			{
				for (i = 0; i < n; i++)
				{
					bufl[i] += (float) (auxl[i] * uni[thread].mix.rx.gain),
						bufr[i] += (float) (auxr[i] * uni[thread].mix.rx.gain);
				}
			}
#endif
			break;

		case TX:
#if 0
			// early mixing of aux buffers
			if (uni[thread].mix.tx.flag)
			{
				for (i = 0; i < n; i++)
				{
					bufl[i] += (float) (auxl[i] * uni[thread].mix.tx.gain),
						bufr[i] += (float) (auxr[i] * uni[thread].mix.tx.gain);
				}
			}
#endif
			for (i = 0; i < n; i++)
			{
				CXBimag (tx[thread].buf.i, i) = bufl[i];
				CXBreal (tx[thread].buf.i, i) = bufr[i];
			}

			CXBhave (tx[thread].buf.i) = n;
			tx[thread].norm = CXBpeak (tx[thread].buf.i);

			do_tx (thread), tx[thread].tick++;

			for (i = 0; i < n; i++)
				bufl[i] = (float) CXBimag (tx[thread].buf.o, i),
				bufr[i] = (float) CXBreal (tx[thread].buf.o, i);
			CXBhave (tx[thread].buf.o) = n;

			break;
	}

	uni[thread].tick++;

} // process_samples
