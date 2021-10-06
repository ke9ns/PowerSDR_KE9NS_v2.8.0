/* noiseblanker.c

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

#include <common.h>

// ke9ns:
//typedef struct _nbstate
//{
    //CXB sigbuf;			 NB1-2: nb->sigbuf     /* Signal Buffer */
    //REAL threshold;		 NB1-2: nb->threshold   /* Noise Blanker Threshold */
    //COMPLEX average_sig;   NB2: does not use hangtime or delay like NB1 does 
    //REAL average_mag;      NB1-2: nb->average_mag
    //COMPLEX delay[8];      NB1: nb->delay
    //int delayindex;        NB1: nb->delayindex
    //int sigindex;          NB1: nb->sigindex
    //int hangtime;          NB1: nb->hangtime
//} *NB, nbstate;


// ke9ns: for both NB1-2 called from sdr.c    new_noiseblanker (rx[thread][k].buf.i, rx[thread][k].nb.thresh); 
// ke9ns: rx[thread][k].buf.i  THIS IS THE SIGNAL BUFFER


NB new_noiseblanker (CXB sigbuf, REAL threshold, int hangtime, int delaytime)
{
  NB nb = (NB) safealloc (1, sizeof (nbstate), "new nbstate"); //ke9ns:  bufvec.c routine allocate nb is pointer to block of memory (containing the struct below)

  nb->sigbuf = sigbuf;              // ke9ns: signal buffer to alter when noise is detected  
  nb->threshold = threshold;        // ke9ns: user defined threshold value
  nb->average_mag = 1.0;            // mag of complex real, img data from the buffer
  nb->hangtime = 0;                 // ke9ns: NB1 only (this was the original way)
  nb->sigindex = 0;                 // ke9ns: NB1 only   
  nb->delayindex = 2;               // ke9ns: NB1 only
  memset (nb->delay, 0, 8 * sizeof (COMPLEX)); // ke9ns: NB1 only, was 8 create COMPLEX delay[8] in the struct and 0 it out
  nb->ht = hangtime; // ke9ns add: .182
  nb->dly = delaytime; // ke9ns add: .182




  //fprintf(stderr,"new_NB: = %f, %d, %d\n", nb->threshold,nb->ht, nb->dly),fflush(stderr);


  return nb;

} //  new_noiseblanker

void del_nb (NB nb)
{
  if (nb)
    {
      safefree ((char *) nb);
    }
}

REAL test = 0;
// ke9ns:  sdr.c noiseblanker (rx[thread][k].nb.gen); 


//ke9ns: NB1 works as follows:
// Loop through the DSP buffer (say 4096 data points for the RX passband)
//   Real Mag of each complex data point
//   delay[sigindex] = buffer data point   [sigindex 0,7,6,5,4,3,2,1]
//   avg real magnitude = 99.9% avg mag + .1% current mag
//   if real mag > (avg mag * thres) and no current hangtime, then start hangtime = 7
//   if hangtime > 0, then current dsp buffer data = 0,0 for 7 data points (the noise blanker) and reduce hangtime down
//     else current dsp buffer data = delay[delayindex]  =2,1,0,7,6,5,4,3
//
//   sigindex = (sigindex + 7) & 7     0,7,6,5,4,3,2,1
//   delayindex = (delayindex + 7) & 7  (starts at 2)  2,1,0,7,6,5,4,3,2

   REAL PERCENT = 0;
void noiseblanker(NB nb) // ke9ns: pointer to nb struct
{
    int i;

  //  fprintf(stderr, "noiseblanker dindex = %d , %d, %d, %d, %f\n", nb->sigindex, nb->delayindex, nb->ht, nb->dly, Cmag(CXBdata(nb->sigbuf, 1000))) , fflush(stderr);

    if (nb->dly > 20) nb->dly = 20; // ke9ns add
    if (nb->ht != 3 && nb->ht != 7 && nb->ht != 15 && nb->ht != 31 && nb->ht != 63) nb->ht = 7; // ke9ns add

    nb->delayindex = nb->dly; // ke9ns add: .182

  
    for (i = 0; i < CXBsize (nb->sigbuf); i++)        // ke9ns: for loop through the signal buffer size is the DSP RX buffer size like 2048 or 4096
    {

      REAL cmag = Cmag (CXBdata (nb->sigbuf, i));     // ke9ns: get magnitude of complexe buffer data     Cmag = sqrt((z.re)*(z.re) + (z.im)*(z.im)) of ((p)->data) [get magnitude of the signal buffer data ]
      nb->delay[nb->sigindex] = CXBdata (nb->sigbuf, i); // ke9ns: modify the delay[8] with complex buffer data  0, 7,6,5,4,3,2,1
    
      if (cmag > 0.01) cmag = 0.01; // ke9ns add: .183

          nb->average_mag = (REAL)(0.999 * (nb->average_mag) + 0.001 * cmag); // ke9ns: 99.9% avg + .1% new    0.01 for strong signals  0.002 for avg

          if ((nb->hangtime == 0) && (cmag > (nb->threshold * nb->average_mag))) // ke9ns hangtime goes up to 7 if was 0 and new mag > avg*3.3
          {
              nb->hangtime = nb->ht; // ke9ns:.182  was 7  this is the muted time period
              
          }
    
      if (nb->hangtime > 0)
	  {
         
          if (nb->ht == 3) 
          { 
              if (nb->hangtime == 3)
              {
                  CXBreal(nb->sigbuf, i) = CXBreal(nb->sigbuf, i) / 2;
                  CXBimag(nb->sigbuf, i) = CXBimag(nb->sigbuf, i) / 2;
              }
              else if (nb->hangtime == 2)
              {
                  CXBdata(nb->sigbuf, i) = Cmplx(0.0, 0.0); // ke9ns: this is the noise blanker here (for 7 data points now ht data points)

              }
              else
              {
                  CXBreal(nb->sigbuf, i) = CXBreal(nb->sigbuf, i) / 2;
                  CXBimag(nb->sigbuf, i) = CXBimag(nb->sigbuf, i) / 2;
              }
           
          } // ht = 3
          else if (nb->ht == 7)
          {
              if (nb->hangtime == 7)
              {
                  CXBreal(nb->sigbuf, i) = CXBreal(nb->sigbuf, i) * 0.6;
                  CXBimag(nb->sigbuf, i) = CXBimag(nb->sigbuf, i) * 0.6;
              }
              else if (nb->hangtime == 6)
              {
                  CXBreal(nb->sigbuf, i) = CXBreal(nb->sigbuf, i) * 0.3;
                  CXBimag(nb->sigbuf, i) = CXBimag(nb->sigbuf, i) * 0.3;
              }
              else if (nb->hangtime == 5)
              {
                  CXBdata(nb->sigbuf, i) = Cmplx(0.0, 0.0); // ke9ns: this is the noise blanker here (for 7 data points now ht data points)

              }
              else if (nb->hangtime == 4)
              {
                  CXBdata(nb->sigbuf, i) = Cmplx(0.0, 0.0); // ke9ns: this is the noise blanker here (for 7 data points now ht data points)

              }
              else if (nb->hangtime == 3)
              {
                  CXBdata(nb->sigbuf, i) = Cmplx(0.0, 0.0); // ke9ns: this is the noise blanker here (for 7 data points now ht data points)

              }
              else if (nb->hangtime == 2)
              {
                  CXBreal(nb->sigbuf, i) = CXBreal(nb->sigbuf, i) * 0.3;
                  CXBimag(nb->sigbuf, i) = CXBimag(nb->sigbuf, i) * 0.3;
              }
              else
              {
                  CXBreal(nb->sigbuf, i) = CXBreal(nb->sigbuf, i) * 0.6;
                  CXBimag(nb->sigbuf, i) = CXBimag(nb->sigbuf, i) * 0.6;
              }

          } // ht = 7
          else if (nb->ht == 15)
          {
              if (nb->hangtime == 15)
              {
                  CXBreal(nb->sigbuf, i) = CXBreal(nb->sigbuf, i) * 0.8;
                  CXBimag(nb->sigbuf, i) = CXBimag(nb->sigbuf, i) * 0.8;
              }
              else if (nb->hangtime == 14)
              {
                  CXBreal(nb->sigbuf, i) = CXBreal(nb->sigbuf, i) * 0.6;
                  CXBimag(nb->sigbuf, i) = CXBimag(nb->sigbuf, i) * 0.6;
              }
              else if (nb->hangtime == 13)
              {
                  CXBreal(nb->sigbuf, i) = CXBreal(nb->sigbuf, i) * 0.4;
                  CXBimag(nb->sigbuf, i) = CXBimag(nb->sigbuf, i) * 0.4;
              }
              else if (nb->hangtime == 12)
              {
                  CXBreal(nb->sigbuf, i) = CXBreal(nb->sigbuf, i) * 0.2;
                  CXBimag(nb->sigbuf, i) = CXBimag(nb->sigbuf, i) * 0.2;
              }
              else if (nb->hangtime == 11)
              {
                  CXBdata(nb->sigbuf, i) = Cmplx(0.0, 0.0); // ke9ns: this is the noise blanker here (for 7 data points now ht data points)

              }
             else if (nb->hangtime == 10)
              {
                  CXBdata(nb->sigbuf, i) = Cmplx(0.0, 0.0); // ke9ns: this is the noise blanker here (for 7 data points now ht data points)

              }
              else if (nb->hangtime == 9)
              {
                  CXBdata(nb->sigbuf, i) = Cmplx(0.0, 0.0); // ke9ns: this is the noise blanker here (for 7 data points now ht data points)

              }
              else if (nb->hangtime == 8)
              {
                  CXBdata(nb->sigbuf, i) = Cmplx(0.0, 0.0); // ke9ns: this is the noise blanker here (for 7 data points now ht data points)

              }
              else if (nb->hangtime == 7)
              {
                  CXBdata(nb->sigbuf, i) = Cmplx(0.0, 0.0); // ke9ns: this is the noise blanker here (for 7 data points now ht data points)

              }
              else if (nb->hangtime == 6)
              {
                  CXBdata(nb->sigbuf, i) = Cmplx(0.0, 0.0); // ke9ns: this is the noise blanker here (for 7 data points now ht data points)

              }
              else if (nb->hangtime == 5)
              {
                  CXBdata(nb->sigbuf, i) = Cmplx(0.0, 0.0); // ke9ns: this is the noise blanker here (for 7 data points now ht data points)

              }
              else if (nb->hangtime == 4)
              {
                  CXBreal(nb->sigbuf, i) = CXBreal(nb->sigbuf, i) * 0.2;
                  CXBimag(nb->sigbuf, i) = CXBimag(nb->sigbuf, i) * 0.2;
              }
              else if (nb->hangtime == 3)
              {
                  CXBreal(nb->sigbuf, i) = CXBreal(nb->sigbuf, i) * 0.4;
                  CXBimag(nb->sigbuf, i) = CXBimag(nb->sigbuf, i) * 0.4;
              }
              else if (nb->hangtime == 2)
              {
                  CXBreal(nb->sigbuf, i) = CXBreal(nb->sigbuf, i) * 0.6;
                  CXBimag(nb->sigbuf, i) = CXBimag(nb->sigbuf, i) * 0.6;
              }
              else 
              {
                  CXBreal(nb->sigbuf, i) = CXBreal(nb->sigbuf, i) * 0.8;
                  CXBimag(nb->sigbuf, i) = CXBimag(nb->sigbuf, i) * 0.8;
              }
             

          } // ht = 15

        
	     nb->hangtime--; // ke9ns: bring hangtime back to 0
	  }
      else
      {
         CXBdata(nb->sigbuf, i) = nb->delay[nb->delayindex]; // ke9ns: delay data points by 2 
      }
   
      nb->sigindex = (nb->sigindex + nb->ht) & nb->ht; //ke9ns:  .182 was 7       0, 7,6,5,4,3,2,1
      nb->delayindex = (nb->delayindex + nb->ht) & nb->ht; // ke9ns: .182 was 7   2, 1,0,7,6,5,4,3,2

    } // for loop through signal buffer

   

} // noiseblanker

// ke9ns
// for loop through the DSP RX buffer (i.e. 4096 or 2048)
// sigindex   = 0
// delayindex = 2
// i = 0 to buffer size -1
//    delay[7,6,5,4,3,2,1,0,7,6,5,4,3,2,1,0] = buffer[0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15]
//
//                                                                              
//  if no hangtime, then buffer[0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15] = delay[2,1,0,7,6,5,4,3,2,1,0,7,6,5,4,3]
//                   act buffer            
//
// sigindex   = 7,6,5,4,3,2,1,0,7,6,5,4,3,2,1,0
// delayindex = 1,0,7,6,5,4,3,2,1,0,7,6,5,4,3,2






// ke9ns: NB2
void SDROMnoiseblanker (NB nb)
{
  int i;
  for (i = 0; i < CXBsize (nb->sigbuf); i++)
    {
      REAL cmag = Cmag (CXBdata (nb->sigbuf, i));

      nb->average_sig = Cadd (Cscl (nb->average_sig, 0.75), Cscl (CXBdata (nb->sigbuf, i), 0.25));

      nb->average_mag = (REAL) (0.999 * (nb->average_mag) + 0.001 * cmag);

      if (cmag > (nb->threshold * nb->average_mag))
      {
          CXBdata(nb->sigbuf, i) = nb->average_sig;
      }
    } // for loop

} // SDROMnoiseblanker
