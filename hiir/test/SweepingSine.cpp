/*****************************************************************************

        SweepingSine.cpp
        Copyright (c) 2005 Laurent de Soras

--- Legal stuff ---

This program is free software. It comes without any warranty, to
the extent permitted by applicable law. You can redistribute it
and/or modify it under the terms of the Do What The Fuck You Want
To Public License, Version 2, as published by Sam Hocevar. See
http://sam.zoy.org/wtfpl/COPYING for more details.

*Tab=3***********************************************************************/



#if defined (_MSC_VER)
	#pragma warning (1 : 4130) // "'operator' : logical operation on address of string constant"
	#pragma warning (1 : 4223) // "nonstandard extension used : non-lvalue array converted to pointer"
	#pragma warning (1 : 4705) // "statement has no effect"
	#pragma warning (1 : 4706) // "assignment within conditional expression"
	#pragma warning (4 : 4786) // "identifier was truncated to '255' characters in the debug information"
	#pragma warning (4 : 4800) // "forcing value to bool 'true' or 'false' (performance warning)"
	#pragma warning (4 : 4355) // "'this' : used in base member initializer list"
#endif



/*\\\ INCLUDE FILES \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/

#include	"hiir/test/SweepingSine.h"
#include	"hiir/def.h"

#include	<cassert>
#include	<cmath>

namespace std {}


namespace hiir
{
namespace test
{



/*\\\ PUBLIC \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/



SweepingSine::SweepingSine (float sample_freq, float freq_beg, float freq_end, long nbr_spl)
:	_sample_freq (sample_freq)
,	_freq_beg (freq_beg)
,	_freq_end (freq_end)
,	_nbr_spl (nbr_spl)
{
	assert (sample_freq > 0);
	assert (freq_beg > 0);
	assert (freq_beg <= sample_freq * 0.5f);
	assert (freq_end > 0);
	assert (freq_end <= sample_freq * 0.5f);
	assert (nbr_spl > 0);
}



void	SweepingSine::generate (float dest_ptr []) const
{
	assert (dest_ptr != 0);

	using namespace std;

	const long double	freq_mult_l = log (_freq_end / _freq_beg) / _nbr_spl;
	const long double	freq_mult = exp (freq_mult_l);
	long double		freq_base = _freq_beg / _sample_freq * (2 * hiir::PI);
	long double		freq = freq_base;
	long double		phase = 0;
	long				pos = 0;
	do
	{
		if ((pos & 0x0FF) == 0)
		{
			freq = freq_base * exp (pos * freq_mult_l);
		}

		dest_ptr [pos] = static_cast <float> (sin (phase));
		phase += freq;
		if (phase > hiir::PI)
		{
			phase -= 2 * hiir::PI;
		}
		freq *= freq_mult;
		++ pos;
	}
	while (pos < _nbr_spl);
}



long	SweepingSine::get_len () const
{
	return (_nbr_spl);
}



float	SweepingSine::get_sample_freq () const
{
	return (_sample_freq);
}



float	SweepingSine::get_freq_at_sample (long pos) const
{
	assert (pos >= 0);
	assert (pos <= _nbr_spl);

	using namespace std;

	const double	freq =
		_freq_beg * exp (log (_freq_end / _freq_beg) * pos / _nbr_spl);

	return (static_cast <float> (freq));
}



long	SweepingSine::get_sample_pos_for (float freq) const
{
	assert (freq > 0);
	assert (freq <= _sample_freq * 0.5f);

	using namespace std;

	const double		pos_dbl =
		_nbr_spl * log (freq / _freq_beg) / log (_freq_end / _freq_beg);
	long					pos = static_cast <long> (pos_dbl);
	if (pos < 0)
	{
		pos = 0;
	}
	else if (pos > _nbr_spl)
	{
		pos = _nbr_spl;
	}

	return (pos);
}



/*\\\ PROTECTED \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/



/*\\\ PRIVATE \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/



}	// namespace test
}	// namespace hiir



/*\\\ EOF \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/
