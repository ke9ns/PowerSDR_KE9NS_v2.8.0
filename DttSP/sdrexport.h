/* sdrexport.h

This file is part of a program that implements a Software-Defined Radio.

Copyright (C) 2004, 2005, 2006 by Frank Brickle, AB2KT and Bob McGwier, N4HY

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

#ifndef _sdrexport_h
#define _sdrexport_h

#include <fromsys.h>
#include <defs.h>
#include <banal.h>
#include <splitfields.h>
#include <datatypes.h>
#include <bufvec.h>
#include <cxops.h>
#include <ringb.h>
#include <lmadf.h>
#include <fftw3.h>
#include <fftw3_fix.h>
#include <ovsv.h>
#include <filter.h>
#include <oscillator.h>
#include <dttspagc.h>
#include <am_demod.h>
#include <fm_demod.h>
#include <noiseblanker.h>
#include <correctIQ.h>
#include <speechproc.h>
#include <spottone.h>
#include <hilbert.h>
#include <update.h>
#include <local.h>
#include <meter.h>
#include <spectrum.h>
#include <diversity.h>
//#include <filt2p.h>
#include <common.h>

//------------------------------------------------------------------------
// max no. simultaneous receivers
#ifndef MAXRX
#define MAXRX (4)
#endif
//------------------------------------------------------------------------
/* modulation types, modes */

//========================================================================
/* RX/TX both */
//------------------------------------------------------------------------

DiversityControl diversity;
//int MAX_NOTCHES_IN_PASSBAND = 18;
extern struct _uni
{
	REAL samplerate;  // ke9ns makes sense
	
	int buflen;       // ke9ns makes sense

	struct
	{
		SDRMODE sdr;
		TRXMODE trx;
	} mode;

	METERBlock meter; // ke9ns: this is the RX and TX meter struct

	SpecBlock spec; // ke9ns: spectrum (for panadapter/waterfall data)

	struct
	{
		BOOLEAN flag;
		FILE *fp;
		splitfld splt;

	} update;

	struct
	{
		char *path;
		int bits;
	} wisdom;

	struct
	{
		BOOLEAN act[MAXRX];
		int lis, nac, nrx;
	} multirx;

	struct
	{
		struct
		{
			BOOLEAN flag;
			REAL gain;
		} rx, tx;
	} mix;

	int cpdlen;
	long tick,oldtick;
	WBIR_STATE wbir_state;

} uni[3];

//------------------------------------------------------------------------
/* RX */
//------------------------------------------------------------------------

extern struct _rx
{
	struct
	{
		CXB i, o;
	} buf;

	IQ iqfix;

	struct
	{
		double freq, phase;
		OSC gen;
	} osc;

	struct
	{
		int decim;
		BOOLEAN flag;
		ResStF gen1r,gen1i,gen2r,gen2i;
	} resample;


	float output_gain;

	struct
	{
		ComplexFIR coef;
		FiltOvSv ovsv;
		COMPLEX *save;
	} filt,filt2;

	DCBlocker dcb;

	struct
	{
		int dly; // ke9ns add: .182
		int ht; // ke9ns add: .182
		REAL thresh;
		NB gen;
		BOOLEAN flag;
		
	} nb;

	struct
	{
		REAL thresh;
		NB gen;
		BOOLEAN flag;
	} nb_sdrom;

	struct
	{
		LMSR gen;
		BOOLEAN flag;
	} anr, anf;

	struct
	{
		BLMS gen;
		BOOLEAN flag;
	} banr, banf;

	struct
	{
		DTTSPAGC gen;
		BOOLEAN flag;
	} dttspagc;

	struct
	{
		AMD gen; //am_demod.h  ADDDesc 
	} am;

	struct
	{
		FMD gen;
	} fm;

	struct
	{
		BOOLEAN flag;
		SpotToneGen gen;
	} spot;

	struct
	{
		REAL thresh, atten, power;
		BOOLEAN flag, running, set;
		int num;
		int phase;
	} squelch;

	struct
	{
		BOOLEAN flag;
		WSCompander gen;
	} cpd;

	struct
	{
		EQ gen;
		BOOLEAN flag;
	} grapheq;

	struct
	{
		IIR_2P2Z gen;
		BOOLEAN flag;
	}  notch[18]; //MAX_NOTCHES_IN_PASSBAND

	SDRMODE mode;
	struct
	{
		BOOLEAN flag;
	} bin;

	REAL norm;
	COMPLEX azim;
	long tick;

} rx[3][MAXRX];       // extern struct _rx

//------------------------------------------------------------------------
/* TX */
//------------------------------------------------------------------------
extern struct _tx
{
	struct
	{
		CXB i, ic, o, oc;
	} buf;
	IQ iqfix;

	struct
	{
		BOOLEAN flag;
		DCBlocker gen;
	} dcb;

	struct
	{
		double freq, phase;
		OSC gen;
	} osc;

	struct
	{
		ComplexFIR coef;          // ke9ns typedef struct _complex_FIR found in filter.h
		FiltOvSv ovsv, ovsv_pre;   // ke9ns typedef struct _filt_ov_sav found in ovsv.h
		COMPLEX *save;             //  ke9ns float struct real, img
	} filt;

	struct
	{
		REAL carrier_level;
	} am;

	struct
	{
		REAL cvtmod2freq;     // ke9ns deviation here
		double phase;
		REAL preemphasis_filter;
		REAL deemphasis_out;
		
		REAL k_preemphasis;
		REAL k_deemphasis;	

		REAL k_preemphasis1; // ke9ns add for WFM lowpass TX input
		REAL k_deemphasis1;  // ke9ns add for WFM lowpass RX output

		REAL clip_threshold;
		IIR_BPF_2P input_BPF;

		IIR_LPF_2P output_LPF1;
		IIR_LPF_2P output_LPF2;

		IIR_LPF_2P output_LPF3; // ke9ns add for WFM lowpass output
		IIR_LPF_2P output_LPF4;

		IIR_LPF_2P input_BPF1;
		IIR_LPF_2P input_BPF2;
		IIR_LPF_2P input_BPF3;
		IIR_LPF_2P input_BPF4;

		IIR_LPF_2P input_LPF1;
		IIR_LPF_2P input_LPF2;

		IIR_HPF_2P input_HPF1;
		IIR_HPF_2P input_HPF2;

		IIR_LPF_2P input_LPF3; // ke9ns add for WFM lowpass input
		IIR_LPF_2P input_LPF4;

		IIR_HPF_2P input_HPF3; // ke9ns add for WFM highpass input
		IIR_HPF_2P input_HPF4;

		BOOLEAN fmdata;  // ke9ns add: for WFM data mode (10khz dev)
 
		struct
		{
			double freq_hz;
			OSC osc;
			REAL amp;
			BOOL flag;
		}ctcss;

	} fm;

	struct
	{
		REAL thresh, atten, power;
		BOOLEAN flag, running, set;
		int num;
	} squelch;

	struct
	{
		DTTSPAGC gen;
		BOOLEAN flag;
	} leveler, alc;

	struct
	{
		EQ gen;
		BOOLEAN flag;
	} grapheq;

	struct
	{
		SpeechProc gen;
		BOOLEAN flag;
	} spr;

	struct
	{
		BOOLEAN flag;
		WSCompander gen;
	} cpd;

	struct
	{
		BOOLEAN flag;
		Hilsim gen;
	} hlb;

	SDRMODE mode;

	long tick;
	REAL norm;
} tx[3];

//------------------------------------------------------------------------

typedef enum _runmode
{
	RUN_MUTE, RUN_PASS, RUN_PLAY, RUN_SWCH
} RUNMODE;


//============================================================================================
// ke9ns  top[3] 
//============================================================================================
extern struct _top
{
	DWORD pid;
	uid_t uid;

	struct timeval start_tv;

	BOOLEAN running, verbose;
	RUNMODE state;

	// audio io
	struct
	{
		struct
		{
			float *l, *r;
		} aux, buf;

		struct
		{
			unsigned int frames, bytes;
		} size;

	} hold;

	struct
	{
		char *path;
		HANDLE fd;
		HANDLE fp;
		char buff[4096];		// KE9NS LOOK this maybe the limiter for 4096 data points received by display ????? how about 8192

	} parm;

	struct
	{
		struct
		{
			char *path;
			HANDLE fp, fd;
		} mtr, spec;
	} meas;

	struct
	{
		char name[256];

		struct
		{
			struct
			{
				ringb_float_t *l, *r;
			} i, o;
		} ring;

		struct
		{
			struct
			{
				ringb_float_t *l, *r;
			} i, o;
		} auxr;

		size_t reset_size;
		size_t size;

		struct
		{
			int cb;
			struct
			{
				int i, o;
			} rb;
			int xr;
		} blow;

	} jack;

	// ke9ns THREADS
	// update io
	// multiprocessing & synchronization
	struct
	{
		struct
		{
			pthread_t id;                
		} trx, upd, updrx, mon, pws, mtr, scope;
	} thrd;

	// ke9ns SEMAPHORE to help sync the thread 
	struct
	{
		struct
		{
			sem_t sem;
		} buf, upd, mon, pws, mtr, scope;
	} sync;


	// TRX switching
	struct
	{
		struct
		{
			struct
			{
				SWCHSTATE type;
				int cnt;
				REAL val;
			} curr;

			struct
			{
				int size;
				REAL incr;
			} fall, rise, stdy;
		} env;

		struct 
		{
			TRXMODE next;
		} trx;

		struct 
		{
			RUNMODE last;
		} run;

		struct
		{
			BOOLEAN flag;
			int count;
			int count_limit;
			float threshold;
		} rise_thresh;

		BOOLEAN flag;
		
	} swch;

	BOOLEAN susp;
	int offset;

} top[3];  // ke9ns only 3 threads


#endif
