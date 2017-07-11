/*****************************************************************************

        Upsampler2x3dnow.h
        Copyright (c) 2005 Laurent de Soras

Upsamples by a factor 2 the input signal, using 3DNow! instruction set.

Template parameters:
	- NC: number of coefficients, > 0

--- Legal stuff ---

This program is free software. It comes without any warranty, to
the extent permitted by applicable law. You can redistribute it
and/or modify it under the terms of the Do What The Fuck You Want
To Public License, Version 2, as published by Sam Hocevar. See
http://sam.zoy.org/wtfpl/COPYING for more details.

*Tab=3***********************************************************************/



#if ! defined (hiir_Upsampler2x3dnow_HEADER_INCLUDED)
#define	hiir_Upsampler2x3dnow_HEADER_INCLUDED

#if defined (_MSC_VER)
	#pragma once
	#pragma warning (4 : 4250) // "Inherits via dominance."
#endif



/*\\\ INCLUDE FILES \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/

#include	"hiir/Array.h"
#include	"hiir/StageData3dnow.h"



namespace hiir
{



template <int NC>
class Upsampler2x3dnow
{

	// Template parameter check, not used
	typedef	int	ChkTpl1 [(NC > 0) ? 1 : -1];

/*\\\ PUBLIC \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/

public:

	enum {			NBR_COEFS	= NC	};

						Upsampler2x3dnow ();

	void				set_coefs (const double coef_arr [NBR_COEFS]);
	inline void		process_sample (float &out_0, float &out_1, float input);
	void				process_block (float out_ptr [], const float in_ptr [], long nbr_spl);
	void				clear_buffers ();



/*\\\ PROTECTED \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/

protected:



/*\\\ PRIVATE \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/

private:

	enum {			STAGE_WIDTH	= 2	};
	enum {			NBR_STAGES = (NBR_COEFS + STAGE_WIDTH - 1) / STAGE_WIDTH	};

	typedef	Array <StageData3dnow, NBR_STAGES + 1>	Filter;	// Stage 0 contains only input memory

	Filter			_filter;		// Should be the first member (thus easier to align)



/*\\\ FORBIDDEN MEMBER FUNCTIONS \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/

private:

	bool				operator == (const Upsampler2x3dnow &other);
	bool				operator != (const Upsampler2x3dnow &other);

};	// class Upsampler2x3dnow



}	// namespace hiir



#include	"hiir/Upsampler2x3dnow.hpp"



#endif	// hiir_Upsampler2x3dnow_HEADER_INCLUDED



/*\\\ EOF \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/
