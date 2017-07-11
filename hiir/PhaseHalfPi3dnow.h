/*****************************************************************************

        PhaseHalfPi3dnow.h
        Copyright (c) 2005 Laurent de Soras

From the input signal, generates two signals with a pi/2 phase shift, using
3DNow! instruction set.

Template parameters:
	- NC: number of coefficients, > 0

--- Legal stuff ---

This program is free software. It comes without any warranty, to
the extent permitted by applicable law. You can redistribute it
and/or modify it under the terms of the Do What The Fuck You Want
To Public License, Version 2, as published by Sam Hocevar. See
http://sam.zoy.org/wtfpl/COPYING for more details.

*Tab=3***********************************************************************/



#if ! defined (hiir_PhaseHalfPi3dnow_HEADER_INCLUDED)
#define	hiir_PhaseHalfPi3dnow_HEADER_INCLUDED

#if defined (_MSC_VER)
	#pragma once
	#pragma warning (4 : 4250) // "Inherits via dominance."
#endif



/*\\\ INCLUDE FILES \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/

#include	"hiir/Array.h"
#include	"hiir/def.h"
#include "hiir/StageData3dnow.h"



namespace hiir
{



template <int NC>
class PhaseHalfPi3dnow
{

	// Template parameter check, not used
	typedef	int	ChkTpl1 [(NC > 0) ? 1 : -1];

/*\\\ PUBLIC \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/

public:

	enum {			NBR_COEFS	= NC	};

						PhaseHalfPi3dnow ();

	void				set_coefs (const double coef_arr []);

	hiir_FORCEINLINE void
						process_sample (float &out_0, float &out_1, float input);
	void				process_block (float out_0_ptr [], float out_1_ptr [], const float in_ptr [], long nbr_spl);

	void				clear_buffers ();



/*\\\ PROTECTED \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/

protected:



/*\\\ PRIVATE \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/

private:

	enum {			STAGE_WIDTH	= 2	};
	enum {			NBR_STAGES	= (NBR_COEFS + STAGE_WIDTH-1) / STAGE_WIDTH	};
	enum {			NBR_PHASES	= 2	};

	typedef	Array <StageData3dnow, NBR_STAGES + 1>	Filter;	// Stage 0 contains only input memory
   typedef  Array <Filter, NBR_PHASES> FilterBiPhase;

	FilterBiPhase	_filter;		// Should be the first member (thus easier to align)
	float				_prev;
	int				_phase;		// 0 or 1



/*\\\ FORBIDDEN MEMBER FUNCTIONS \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/

private:

	bool				operator == (const PhaseHalfPi3dnow &other);
	bool				operator != (const PhaseHalfPi3dnow &other);

};	// class PhaseHalfPi3dnow



}	// namespace hiir



#include	"hiir/PhaseHalfPi3dnow.hpp"



#endif	// hiir_PhaseHalfPi3dnow_HEADER_INCLUDED



/*\\\ EOF \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/
