/* winmain.c

This file is part of a program that implements a Software-Defined Radio.

Copyright (C) 2004, 2005, 2006, 2007 by Frank Brickle, AB2KT and Bob McGwier, N4HY

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
/////////////////////////////////////////////////////////////////////////

// elementary defaults
struct _loc loc[3];
extern unsigned int threadno;
/////////////////////////////////////////////////////////////////////////
// most of what little we know here about the inner loop,
// functionally speaking

extern void reset_meters (unsigned int);
extern void reset_spectrum (unsigned int);
extern void reset_counters (unsigned int);
extern void process_samples (float *, float *, float *, float *, int, unsigned int);
extern void setup_workspace (REAL rate, int buflen, SDRMODE mode, char *wisdom, int specsize, int numrecv, int cpdsize, unsigned int thread);
extern void destroy_workspace (unsigned int thread);


//========================================================================

//========================================================================



BOOLEAN reset_em;
char *APP_DATA_PATH;

PRIVATE BOOLEAN gethold (unsigned int proc_thread)
{
	if (ringb_float_read_space (top[proc_thread].jack.ring.i.l) >= top[proc_thread].hold.size.frames)
		{
			ringb_float_read (top[proc_thread].jack.ring.i.l,top[proc_thread].hold.buf.l, top[proc_thread].hold.size.frames);
			ringb_float_read (top[proc_thread].jack.ring.i.r,top[proc_thread].hold.buf.r, top[proc_thread].hold.size.frames);
	#ifdef USE_AUXILIARY
			ringb_float_read (top[proc_thread].jack.auxr.i.l,
				top[proc_thread].hold.aux.l, top[proc_thread].hold.size.frames);
			ringb_float_read (top[thread].jack.auxr.i.r,
				top[proc_thread].hold.aux.r, top[proc_thread].hold.size.frames);
	#else
			ringb_float_read (top[proc_thread].jack.auxr.i.l,top[proc_thread].hold.buf.l, top[proc_thread].hold.size.frames);
			ringb_float_read (top[proc_thread].jack.auxr.i.r,top[proc_thread].hold.buf.r, top[proc_thread].hold.size.frames);
	#endif
			return TRUE;
		} else return FALSE;

}

PRIVATE void puthold(unsigned int proc_thread)
{
	   if (ringb_float_write_space (top[proc_thread].jack.ring.o.l) >= top[proc_thread].hold.size.frames)
		{
			ringb_float_write (top[proc_thread].jack.ring.o.l, top[proc_thread].hold.buf.l,top[proc_thread].hold.size.frames);
			ringb_float_write (top[proc_thread].jack.ring.o.r, top[proc_thread].hold.buf.r,top[proc_thread].hold.size.frames);
	#ifdef USE_AUXILIARY
			ringb_float_write (top[proc_thread].jack.auxr.o.l, top[proc_thread].hold.aux.l,
				top[proc_thread].hold.size.frames);
			ringb_float_write (top[proc_thread].jack.auxr.o.r, top[proc_thread].hold.aux.r,
				top[proc_thread].hold.size.frames);
	#else
			ringb_float_write (top[proc_thread].jack.auxr.o.l, top[proc_thread].hold.buf.l,top[proc_thread].hold.size.frames);
			ringb_float_write (top[proc_thread].jack.auxr.o.r, top[proc_thread].hold.buf.r,top[proc_thread].hold.size.frames);
		}
	#endif

}
PRIVATE BOOLEAN
canhold (unsigned int proc_thread)
{

	return (ringb_float_read_space (top[proc_thread].jack.ring.i.l) >= (size_t) top[proc_thread].hold.size.frames);
}


//------------------------------------------------------------------------

PRIVATE void run_mute (unsigned int thread)
{
	memset ((char *) top[thread].hold.buf.l, 0, top[thread].hold.size.bytes);
	memset ((char *) top[thread].hold.buf.r, 0, top[thread].hold.size.bytes);
	memset ((char *) top[thread].hold.aux.l, 0, top[thread].hold.size.bytes);
	memset ((char *) top[thread].hold.aux.r, 0, top[thread].hold.size.bytes);
	uni[thread].tick++;
}

PRIVATE void run_pass (unsigned int thread)
{
	uni[thread].tick++;
}

PRIVATE void run_play (unsigned int thread)
{
	process_samples (top[thread].hold.buf.l, top[thread].hold.buf.r,
		top[thread].hold.aux.l, top[thread].hold.aux.r, top[thread].hold.size.frames, thread);
}

// NB do not set RUN_SWCH directly via setRunState;
// use setSWCH instead


PRIVATE void run_swch(unsigned int thread)
{
	int i, n = top[thread].hold.size.frames;
	REAL w;
//	static int count = 0;

	for (i = 0; i < n; i++)
	{
//		count++;
		switch(top[thread].swch.env.curr.type)
		{
		case SWCH_FALL:
			top[thread].swch.env.curr.val += top[thread].swch.env.fall.incr;
			w = (REAL)sin(top[thread].swch.env.curr.val * M_PI /  2.0f);
			top[thread].hold.buf.l[thread] *= w, top[thread].hold.buf.r[thread] *= w;
			top[thread].hold.aux.l[thread] *= w, top[thread].hold.aux.r[thread] *= w;
//			if (top[thread].swch.env.curr.cnt == 0) fprintf(stderr, "FALL\n"),fflush(stderr);
//			if(top[thread].swch.env.curr.cnt == 0) top[thread].hold.buf.l[thread] = top[thread].hold.buf.r[thread] = -1.0;
			if (++top[thread].swch.env.curr.cnt >= top[thread].swch.env.fall.size)
			{
				//top[thread].hold.buf.l[thread] = top[thread].hold.buf.r[thread] = -1.0;
				top[thread].swch.env.curr.type = SWCH_STDY;
				top[thread].swch.env.curr.cnt = 0;
				top[thread].swch.env.curr.val = 0.0;
//				fprintf(stderr, "Fall End: %d\n", count);
			}
			break;

		case SWCH_STDY:		
			top[thread].hold.buf.l[i]= top[thread].hold.buf.r[i] =
				top[thread].hold.aux.l[i] =  top[thread].hold.aux.r[i] = 0.0;
//			if (top[thread].swch.env.curr.cnt == 0) fprintf(stderr, "STDY\n"),fflush(stderr);
			if (++top[thread].swch.env.curr.cnt >= top[thread].swch.env.stdy.size)
			{
//				top[thread].hold.buf.l[thread] = top[thread].hold.buf.r[thread] = -1.0;
				top[thread].swch.env.curr.type = SWCH_RISE;
				top[thread].swch.env.curr.cnt = 0;
				top[thread].swch.env.curr.val = 0.0;
//				fprintf(stderr, "Stdy End: %d\n", count);
			}
			break;
		
		case SWCH_RISE:
			if(!top[thread].swch.rise_thresh.flag)
			{
				top[thread].swch.rise_thresh.count++;
				if((abs(top[thread].hold.buf.l[i]) > top[thread].swch.rise_thresh.threshold ||
					abs(top[thread].hold.buf.r[i]) > top[thread].swch.rise_thresh.threshold) ||
					top[thread].swch.rise_thresh.count == top[thread].swch.rise_thresh.count_limit)
				top[thread].swch.rise_thresh.flag = TRUE;
			}
				
			if(top[thread].swch.rise_thresh.flag)
			{
				top[thread].swch.env.curr.val += top[thread].swch.env.rise.incr;
				w = 1.0f - (REAL)cos(top[thread].swch.env.curr.val * M_PI / 2.0f);				

				top[thread].hold.buf.l[i] *= w, top[thread].hold.buf.r[i] *= w;
				top[thread].hold.aux.l[i] *= w, top[thread].hold.aux.r[i] *= w;

				/*if(top[thread].swch.env.curr.cnt == 0)
				{
					top[thread].hold.buf.l[i] = 1.0;
					top[thread].hold.buf.r[i] = 1.0;
				}*/

	//			if (top[thread].swch.env.curr.cnt == 0) fprintf(stderr, "RISE\n"),fflush(stderr);
				if (++top[thread].swch.env.curr.cnt >= top[thread].swch.env.rise.size)
				{
					//top[thread].hold.buf.l[i] = 1.0;
					//top[thread].hold.buf.r[i] = 1.0;

	//				reset_meters();
	//				reset_spectrum();
	//				reset_counters();
		
					//uni[thread].mode.trx = top[thread].swch.trx.next;
					top[thread].state = top[thread].swch.run.last;
					return;
	//				fprintf(stderr, "Rise End: %d\n", count);
				}
			}
			else
			{
				top[thread].hold.buf.l[i] = 0.0f;
				top[thread].hold.buf.r[i] = 0.0f;
			}
			break;
		}
	}

	process_samples(top[thread].hold.buf.l, top[thread].hold.buf.r,
		top[thread].hold.aux.l, top[thread].hold.aux.r,
		top[thread].hold.size.frames, thread);
}


//========================================================================

static void reset_system_audio(size_t nframes)
{
	size_t reset_size;
	unsigned int thread;
	const float zero = 0.;
	int i;

	reset_em = FALSE;
	for(thread = 0; thread < threadno; thread++) 
	{
		reset_size = max (top[thread].jack.reset_size, nframes);
		ringb_float_reset (top[thread].jack.ring.i.l);
		ringb_float_reset (top[thread].jack.ring.i.r);
		ringb_float_reset (top[thread].jack.auxr.i.l);
		ringb_float_reset (top[thread].jack.auxr.i.r);
		ringb_float_clear (top[thread].jack.ring.i.l, top[thread].jack.size * loc[thread].mult.ring-1);
		ringb_float_clear (top[thread].jack.ring.i.l, top[thread].jack.size * loc[thread].mult.ring-1);
		ringb_float_reset (top[thread].jack.ring.i.l);
		ringb_float_reset (top[thread].jack.ring.i.r);
	
		if (top[thread].offset < 0)
		{
			for(i=top[thread].offset;i<0;i++)
			{
				ringb_float_write(top[thread].jack.ring.i.l,&zero,1);
				ringb_float_write(top[thread].jack.auxr.i.l,&zero,1);
				//ringb_float_write(top[thread].jack.auxr.i.l,&zero,1);
			}
		}
		else
		{
			for(i=0;i<top[thread].offset;i++)
			{
				ringb_float_write(top[thread].jack.ring.i.r,&zero,1);
				ringb_float_write(top[thread].jack.auxr.i.r,&zero,1);
				//ringb_float_write(top[thread].jack.auxr.i.r,&zero,1);
			}
		}

		ringb_float_restart (top[thread].jack.ring.o.r, reset_size);
		ringb_float_restart (top[thread].jack.ring.o.l, reset_size);
		ringb_float_restart (top[thread].jack.auxr.o.r, reset_size);
		ringb_float_restart (top[thread].jack.auxr.o.l, reset_size);
		//ringb_float_restart (top[thread].jack.auxr.o.r, reset_size);
		//ringb_float_restart (top[thread].jack.auxr.o.l, reset_size);
	}
}


//======================================================================================
DttSP_EXP void Audio_Callback (float *input_l, float *input_r, float *output_l,	float *output_r, unsigned int nframes)
{
	BOOLEAN b = reset_em;
	int i;
	if (top[0].susp)
	{
		memset (output_l, 0, nframes * sizeof (float));
		memset (output_r, 0, nframes * sizeof (float));
		return;
	}

	if (b)
	{
		//fprintf(stdout,"Audio_Callback: call reset_system_audio\n"), fflush(stdout);
		reset_system_audio(nframes);
		memset (output_l, 0, nframes * sizeof (float));
		memset (output_r, 0, nframes * sizeof (float));
		return;
    }

	for (i=0; i<2; i++) 
	{
		if ((ringb_float_read_space (top[i].jack.ring.o.l) >= nframes)
			&& (ringb_float_read_space (top[i].jack.ring.o.r) >= nframes))
		{
			if (top[i].state == RUN_PLAY)
			{
				ringb_float_read (top[i].jack.auxr.o.l, output_l, nframes);
				ringb_float_read (top[i].jack.auxr.o.r, output_r, nframes);
				ringb_float_read (top[i].jack.ring.o.l, output_l, nframes);
				ringb_float_read (top[i].jack.ring.o.r, output_r, nframes);
			}
			else
			{
				ringb_float_read_advance (top[i].jack.auxr.o.l, nframes);
				ringb_float_read_advance (top[i].jack.auxr.o.r, nframes);
				ringb_float_read_advance (top[i].jack.ring.o.l, nframes);
				ringb_float_read_advance (top[i].jack.ring.o.r, nframes);

			}
		}
		else
		{	// rb pathology
			//fprintf(stdout,"Audio_Callback-2: rb out pathology\n"), fflush(stdout);
			reset_system_audio(nframes);
			memset (output_l, 0, nframes * sizeof (float));
			memset (output_r, 0, nframes * sizeof (float));
		}

		// input: copy from port to ring
		if ((ringb_float_write_space (top[i].jack.ring.i.l) >= nframes)
			&& (ringb_float_write_space (top[i].jack.ring.i.r) >= nframes))
		{
			ringb_float_write (top[i].jack.ring.i.l, (float *) input_l, nframes);
			ringb_float_write (top[i].jack.ring.i.r, (float *) input_r, nframes);
			ringb_float_write (top[i].jack.auxr.i.l, (float *) input_l, nframes);
			ringb_float_write (top[i].jack.auxr.i.r, (float *) input_r, nframes);
		}
		else
		{	// rb pathology
			//fprintf(stdout,"Audio_Callback-3: rb in pathology\n"), fflush(stdout);
			reset_system_audio(nframes);
			memset (output_l, 0, nframes * sizeof (float));
			memset (output_r, 0, nframes * sizeof (float));
		}

		// if enough accumulated in ring, fire dsp
		if (ringb_float_read_space (top[i].jack.ring.i.l) >= top[i].hold.size.frames)
			sem_post (&top[i].sync.buf.sem);
	}
} // Audio_Callback




//=======================================================================================================
// ke9ns  used in audio.cs routine  audio_callback2 ( DttSP.ExchangeSamples2 routine in audio.cs)
// ke9ns threadno:   1=demo/1500, 2=RX1 3000/5000,  3=RX2 5000 enabled
//                   1=Transmit/rx,  2=receive only, 3=receive only
// 
// 	 l0 = input[0]; left and right channels for Main RX1
//   r0 = input[1];
// 
//   l2 = input[4]; left and right channels for 2nd RX2
//   r2 = input[5];
//=======================================================================================================
DttSP_EXP void Audio_Callback2 (float **input, float **output, unsigned int nframes)
{
	unsigned int thread;
	BOOLEAN b = reset_em;             // ke9ns if true then reset audio stream ??
	BOOLEAN return_empty = FALSE;
	unsigned int i;
	

	for(thread=0;thread < threadno;thread++) // ke9ns check 3 possible threads
	{
		if (top[thread].susp) return_empty = TRUE;                       // ke9ns return if suspending thread operation ?
	}

	if (return_empty)
	{
		for(thread=0;thread < threadno;thread++) 
		{
			memset (output[2*thread], 0, nframes * sizeof (float));  // ke9ns clear out output and return
			memset (output[2*thread+1], 0, nframes * sizeof (float));
		}
		return;
	}

	if (b)                                 // ke9ns come here when setup routine runs (winmain)
	{
	//	fprintf(stderr, "reset_em!\n"); fflush(stderr);
	//	fprintf(stdout,"Audio_Callback2: reset_em = TRUE\n"), fflush(stdout);
		
		reset_system_audio(nframes);

		for(thread=0;thread < threadno;thread++)
		{
			memset (output[2*thread], 0, nframes * sizeof (float));  // ke9ns clear out output and return  (pointer to  dest, unsigned char value, # of bytes to set to this value)
			memset (output[2*thread+1], 0, nframes * sizeof (float));
		}

		return;
    }

// ke9ns removed some #if 0 code from this location

	for(thread=0; thread < threadno; thread++) 
	{
		int l = 2*thread, r = 2*thread+1;

		if ((ringb_float_read_space (top[thread].jack.ring.o.l) >= nframes)	&& (ringb_float_read_space (top[thread].jack.ring.o.r) >= nframes))
		{
			/*ringb_float_read (top[thread].jack.auxr.o.l, output[l], nframes);
			ringb_float_read (top[thread].jack.auxr.o.r, output[r], nframes);*/
		
			ringb_float_read (top[thread].jack.ring.o.l, output[l], nframes);
			ringb_float_read (top[thread].jack.ring.o.r, output[r], nframes);
		}
		else
		{	
			// rb pathology
			//reset_system_audio(nframes);
			for(thread=0;thread < threadno;thread++) 
			{
				memset (output[2*thread  ], 0, nframes * sizeof (float));
				memset (output[2*thread+1], 0, nframes * sizeof (float));
			}
			return;
		}

		// input: copy from port to ring
		if ((ringb_float_write_space (top[thread].jack.ring.i.l) >= nframes) && (ringb_float_write_space (top[thread].jack.ring.i.r) >= nframes))
		{
			if (diversity.flag && (thread == 0))
			{
				

				if ((ringb_float_write_space (top[2].jack.ring.i.l) >= nframes)	&& (ringb_float_write_space (top[2].jack.ring.i.r) >= nframes))
				{
					REAL *l0 = input[0];
					REAL *r0 = input[1];
					REAL *l2 = input[4];
					REAL *r2 = input[5];

					for (i=0;i < nframes;i++) 
					{
						COMPLEX A = Cmplx(l0[i],r0[i]); // ke9ns RX1
						COMPLEX B = Cmplx(l2[i],r2[i]); // ke9ns RX2

						A = Cscl(Cadd(A,Cmul(B,diversity.scalar)),diversity.gain); // ke9ns: new A = scale signal with diversity.gain = A + (B * diversity.scaler)

						ringb_float_write (top[0].jack.ring.i.l, &A.re, 1); // write the new A which is has RX2 scalared into RX1, into the "jack" struct
						ringb_float_write (top[0].jack.ring.i.r, &A.im, 1); // ringb_float_t *l, *r;  // ke9ns: ringb_float_t = float *buf; size_t wptr, rptr, size, mask;  buffer, write pointer, read pointer, size of buffer, mask???

					}

					/*ringb_float_write (top[thread].jack.auxr.i.l, input[l], nframes);
					ringb_float_write (top[thread].jack.auxr.i.r, input[r], nframes);*/
				} 
				else
				{
					// rb pathology
					//reset_system_audio(nframes);
					for(thread=0;thread < threadno;thread++) 
					{
						memset (output[2*thread  ], 0, nframes * sizeof (float));
						memset (output[2*thread+1], 0, nframes * sizeof (float));
					}
					return;
				}
			} 
			else
			{
				ringb_float_write (top[thread].jack.ring.i.l, input[l], nframes);
				ringb_float_write (top[thread].jack.ring.i.r, input[r], nframes);
				
				/*ringb_float_write (top[thread].jack.auxr.i.l, input[l], nframes);
				ringb_float_write (top[thread].jack.auxr.i.r, input[r], nframes);*/
			}
		}
		else
		{	
			// rb pathology
			//reset_system_audio(nframes);

			for(thread=0;thread < threadno;thread++) 
			{
				memset (output[2*thread  ], 0, nframes * sizeof (float));
				memset (output[2*thread+1], 0, nframes * sizeof (float));
			}
			return;
		}
		
		// if enough accumulated in ring, fire dsp
		if ((ringb_float_read_space(top[thread].jack.ring.i.l) >= top[thread].hold.size.frames) && (ringb_float_read_space(top[thread].jack.ring.i.r) >= top[thread].hold.size.frames))
		{
			sem_post(&top[thread].sync.buf.sem); // sem_t sem; sdrexport.h this contains the buffer ring data
		}


	} // for thread loop



} // audio_callback2 ( DttSP.ExchangeSamples2 routine in audio.cs)


//========================================================================


DttSP_EXP void process_samples_thread (unsigned int proc_thread)  // ke9ns: thread started in console.cs  audio_process_thread[proc_thread] = new Thread(new ThreadStart(pstc[proc_thread].ProcessSampleThread));
{
	while (top[proc_thread].running)
	{
		sem_wait (&top[proc_thread].sync.buf.sem);

		while (gethold(proc_thread)) 
		{
			sem_wait (&top[proc_thread].sync.upd.sem);
			switch (top[proc_thread].state)
			{
				case RUN_MUTE:
					run_mute (proc_thread);
					break;
				case RUN_PASS:
					run_pass (proc_thread);
					break;
				case RUN_PLAY:
					run_play (proc_thread);
					break;
				case RUN_SWCH:
					run_swch (proc_thread);
					break;
			}
			sem_post (&top[proc_thread].sync.upd.sem);
			puthold (proc_thread);
		}
	}
}



void closeup ()
{
	unsigned int thread;
	for(thread = 0; thread<3;thread++) 
	{
		top[thread].running = FALSE;
		top[thread].susp = TRUE;
		Sleep(396);										// .310 Sleep (96);	
		ringb_float_free (top[thread].jack.auxr.i.l);
		ringb_float_free (top[thread].jack.auxr.i.r);
		ringb_float_free (top[thread].jack.auxr.o.l);
		ringb_float_free (top[thread].jack.auxr.o.r);

		ringb_float_free (top[thread].jack.ring.o.r);
		ringb_float_free (top[thread].jack.ring.o.l);
		ringb_float_free (top[thread].jack.ring.i.r);
		ringb_float_free (top[thread].jack.ring.i.l);

		destroy_workspace (thread);
	}
	Sleep(400);									// .310 Sleep(100);
	//fprintf(stderr,"Done with destructor\n"),fflush(stderr);
}



PRIVATE void
setup_local_audio (unsigned int thread)
{
	top[thread].hold.size.frames = uni[thread].buflen;

	top[thread].hold.size.bytes = top[thread].hold.size.frames * sizeof (float);
	top[thread].hold.buf.l =
		(float *) safealloc (top[thread].hold.size.frames, sizeof (float),
		"main hold buffer left");
	top[thread].hold.buf.r =
		(float *) safealloc (top[thread].hold.size.frames, sizeof (float),
		"main hold buffer right");
	top[thread].hold.aux.l =
		(float *) safealloc (top[thread].hold.size.frames, sizeof (float),
		"aux hold buffer left");
	top[thread].hold.aux.r =
		(float *) safealloc (top[thread].hold.size.frames, sizeof (float),
			"aux hold buffer right");
}


PRIVATE void setup_system_audio (unsigned int thread)
{

//	sprintf (top[thread].jack.name, "sdr-%0u-%0u", top[thread].pid,thread); // .310 removed sprintf (top[thread].jack.name, "sdr-%d-%0u", top[thread].pid,thread);
																			// pid is unsigned long (4 bytes)
	top[thread].jack.size = 2048;

//	memset ((char *) &top[thread].jack.blow, 0, sizeof (top[thread].jack.blow)); //.310 remove

	top[thread].jack.ring.i.l = ringb_float_create (top[thread].jack.size * loc[thread].mult.ring);
	top[thread].jack.ring.i.r = ringb_float_create (top[thread].jack.size * loc[thread].mult.ring);
	top[thread].jack.ring.o.l = ringb_float_create (top[thread].jack.size * loc[thread].mult.ring);
	top[thread].jack.ring.o.r = ringb_float_create (top[thread].jack.size * loc[thread].mult.ring);
	top[thread].jack.auxr.i.l = ringb_float_create (top[thread].jack.size * loc[thread].mult.ring);
	top[thread].jack.auxr.i.r = ringb_float_create (top[thread].jack.size * loc[thread].mult.ring);
	top[thread].jack.auxr.o.l = ringb_float_create (top[thread].jack.size * loc[thread].mult.ring);
	top[thread].jack.auxr.o.r = ringb_float_create (top[thread].jack.size * loc[thread].mult.ring);

	ringb_float_clear (top[thread].jack.ring.o.l, top[thread].hold.size.frames);
	ringb_float_clear (top[thread].jack.ring.o.r, top[thread].hold.size.frames);
}

PRIVATE void setup_threading (unsigned int thread)
{
	top[thread].susp = FALSE;
	sem_init (&top[thread].sync.upd.sem, 0, 0);
	sem_init (&top[thread].sync.buf.sem, 0, 0);	
}

//========================================================================
// hard defaults, then environment

PRIVATE void setup_defaults (unsigned int thread)
{
	//fprintf(stderr,"I am inside setup defaults thread: %0u\n",thread),fflush(stderr);
	
	//loc[thread].name[0] = 0;		// no default name for jack client //.310 removed

//	sprintf (loc[thread].path.rcfile, "%s%0lu_%0u", RCBASE, top[thread].pid,thread); //  .DttSPrc
//	sprintf (loc[thread].path.parm, "%s%0lu_%0u", PARMPATH, top[thread].pid,thread);    //  \\\\.\\pipe\\SDRcommands    (escape sequence) \\.\SDRcommands (access to a pipe named SDRcommands)
//	sprintf (loc[thread].path.meter, "%s%0lu_%0u", METERPATH, top[thread].pid,thread);  //  \\\\.\\pipe\\SDRmeter                          \\.\SDRmeter
//	sprintf (loc[thread].path.spec, "%s%0lu_%0u", SPECPATH, top[thread].pid,thread);    //  \\\\.\\pipe\\SDRspectrum                       \\.\SDRspectrum
//	sprintf (loc[thread].path.wisdom, "%s%0lu_%0u", WISDOMPATH, top[thread].pid,thread); //  .\\wisdom                                      .\wisdom  (refers to the file wisdom in the current directory)

	loc[thread].def.rate = DEFRATE; // 48000
	loc[thread].def.size = DEFSIZE; // 4096
	loc[thread].def.nrx = MAXRX; // 2
	loc[thread].def.mode = DEFMODE; // usb
	loc[thread].def.spec = DEFSPEC; // 4096 ke9ns: this is specsize= uni[thread].spec.size;
	
	loc[thread].mult.ring = RINGMULT; // 4
	loc[thread].def.comp = DEFCOMP; //512

} // setup_defaults

//========================================================================
void setup (char *app_data_path)
{
	unsigned int thread;
	//fprintf(stderr,"I am inside setup\n"),fflush(stderr);
	diversity.gain = 1;
	diversity.flag = FALSE;
	diversity.scalar = cxzero;

	APP_DATA_PATH=app_data_path;

	for (thread=0;thread < 3;thread++) 
	{
		top[thread].pid = GetCurrentThreadId ();
		top[thread].uid = 0L;
		top[thread].start_tv = now_tv ();
		top[thread].running = TRUE;
		top[thread].verbose = FALSE;
		if (thread != 1) top[thread].state = RUN_PLAY;
		else top[thread].state = RUN_PASS;
		top[thread].offset = 0;
		top[thread].jack.reset_size = 2048;
		reset_em =TRUE;
		setup_defaults (thread);
		//strcpy(loc[thread].path.wisdom,app_data_path); //.310 removed
		
		//fprintf(stderr,"setup: defaults done thread %u\n", thread),fflush(stderr);
		uni[thread].meter.flag = TRUE;
		uni[thread].spec.flag = TRUE;
		top[thread].swch.env.fall.size = (int)(loc[thread].def.rate * 0.005);
		top[thread].swch.env.stdy.size = (int)(loc[thread].def.rate * 0.050);
		top[thread].swch.env.rise.size = (int)(loc[thread].def.rate * 0.005);


		top[thread].swch.env.curr.val = 0.0;
		top[thread].swch.env.curr.cnt = 0;
		top[thread].swch.env.rise.incr = 1.0f/(float)top[thread].swch.env.rise.size;
		top[thread].swch.env.fall.incr = 1.0f/(float)top[thread].swch.env.fall.size;
		//fprintf(stderr,"setup: switch done\n"),fflush(stderr);
		//fprintf(stderr,"setup: Entering workspace setup, thread %u\n", thread),fflush(stderr);

		setup_workspace (loc[thread].def.rate, loc[thread].def.size, loc[thread].def.mode, app_data_path, loc[thread].def.spec, loc[thread].def.nrx, loc[thread].def.comp, thread); // ke9ns: 
		
		//fprintf(stderr,"setup: workspace done thread %u\n", thread),fflush(stderr);

		setup_local_audio (thread);
		//fprintf(stderr,"setup: setup_local_audio done\n"),fflush(stderr);
		setup_system_audio (thread);
		//fprintf(stderr,"setup: setup_system_audio done\n"),fflush(stderr);

		setup_threading (thread);
		//fprintf(stderr,"setup: threading done\n"),fflush(stderr);

		// setup_switching ();
		uni[thread].spec.flag = TRUE;
		uni[thread].spec.type = SPEC_POST_FILT;
		uni[thread].spec.scale = SPEC_PWR;
		uni[thread].spec.rxk = 0;
		reset_meters (thread);
		reset_spectrum (thread);
		reset_counters (thread);
		//fprintf(stderr,"setup sdr thread %0u: done\n",thread),fflush(stderr);
	}
}


//=======================================================================================
// ke9ns called by setdspbuflen() in update.c
//BOOLEAN reset_buflen = FALSE;
int reset_for_buflen (unsigned int thread, int new_buflen)
{
	// make sure new size is power of 2
	if (popcnt (new_buflen) != 1) return -1;
//	reset_buflen = TRUE;

	uni[thread].buflen = new_buflen;

	top[thread].jack.reset_size = new_buflen;

	safefree ((char *) top[thread].hold.buf.r);
	safefree ((char *) top[thread].hold.buf.l);
	safefree ((char *) top[thread].hold.aux.r);
	safefree ((char *) top[thread].hold.aux.l);

	destroy_workspace (thread);
//	reset_buflen = FALSE;
	loc[thread].def.size = new_buflen;
	setup_workspace (loc[thread].def.rate, loc[thread].def.size, loc[thread].def.mode, APP_DATA_PATH, loc[thread].def.spec, loc[thread].def.nrx, loc[thread].def.size, thread);

	setup_local_audio (thread);

	reset_meters (thread);
	reset_spectrum (thread);
	reset_counters (thread);

	return 0;
} //  reset_for_buflen   called by resizesdr routine in dttsp.cs



int reset_for_samplerate (REAL new_samplerate)
{
	unsigned int thread;

	for(thread=0; thread<3; thread++) 
	{	
		safefree ((char *) top[thread].hold.buf.r);
		safefree ((char *) top[thread].hold.buf.l);
		safefree ((char *) top[thread].hold.aux.r);
		safefree ((char *) top[thread].hold.aux.l);
		destroy_workspace (thread);

		loc[thread].def.rate = uni[thread].samplerate = new_samplerate;
		top[thread].swch.env.fall.size = (int)(loc[thread].def.rate * 0.005);
		top[thread].swch.env.stdy.size = (int)(loc[thread].def.rate * 0.050);
		top[thread].swch.env.rise.size = (int)(loc[thread].def.rate * 0.005);
		top[thread].swch.env.curr.val = 0.0;
		top[thread].swch.env.curr.cnt = 0;
		top[thread].swch.env.fall.incr = 1.0f/(float)top[thread].swch.env.fall.size;
		top[thread].swch.env.rise.incr = 1.0f/(float)top[thread].swch.env.rise.size;
	
	//	fprintf(stderr, "1Reset for samplerate now: %d\n", loc[thread].def.spec);
	//fflush(stderr);

		setup_workspace (loc[thread].def.rate, loc[thread].def.size, loc[thread].def.mode, APP_DATA_PATH, loc[thread].def.spec, loc[thread].def.nrx, loc[thread].def.size,thread);
		
		//fprintf(stderr, "2Reset for samplerate now: %d\n", loc[thread].def.spec);
	//	fflush(stderr);

		setup_local_audio (thread);

		reset_meters (thread);
		reset_spectrum (thread);
		reset_counters (thread);
	}
	return 0;
}
