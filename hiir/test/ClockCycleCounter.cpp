/*****************************************************************************

        ClockCycleCounter.cpp
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

#include	"hiir/test/ClockCycleCounter.h"

#if defined (__MACOS__)
#include <Gestalt.h>
#endif	// __MACOS__

#include	<cassert>



namespace hiir
{
namespace test
{



/*\\\ PUBLIC \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/



#if defined (__MACOS__)

static inline double	hiir_test_ClockCycleCounter_get_time_s ()
{
	const Nanoseconds	ns = AbsoluteToNanoseconds (UpTime ());

	return (ns.hi * 4294967296e-9 + ns.lo * 1e-9);
}

#endif	// __MACOS__



ClockCycleCounter::ClockCycleCounter ()
:	_start (0)
,	_end (0)
,	_best (-1)
{
	if (! _init_flag)
	{
#if defined (__MACOS__)

		// Question: What if clock speed exceeds 2.1 GHz ???
		long				clk_speed;
		const ::OSErr	err = ::Gestalt (gestaltProcClkSpeed, &clk_speed);

		const double	start_time_s = hiir_test_ClockCycleCounter_get_time_s ();
		start ();

		const double	duration = 0.01;	// Seconds
		while (stopwatch_ClockCycleCounter_get_time_s () - start_time_s < duration)
		{
			continue;
		}

		const double	stop_time_s = hiir_test_ClockCycleCounter_get_time_s ();
		stop ();

		const double	diff_time_s = stop_time_s - start_time_s;
		const double	nbr_cycles = diff_time_s * static_cast <double> (clk_speed);

		const Int64		diff_time_c = _state - _start_time;
		const double	clk_mul = nbr_cycles / static_cast <double> (diff_time_c);

		_clk_mul = round_int (clk_mul);

#endif	// __MACOS__

		_init_flag = true;
	}
}



double	ClockCycleCounter::get_best_score (long nbr_spl) const
{
	assert (_best >= 0);
	assert (nbr_spl > 0);

	const double	val =   static_cast <double> (_best * _clk_mul)
						      / static_cast <double> (nbr_spl);

	return (val);
}



/*\\\ PROTECTED \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/



/*\\\ PRIVATE \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/



long	ClockCycleCounter::_clk_mul = 1;
bool	ClockCycleCounter::_init_flag = false;



}	// namespace test
}	// namespace hiir



/*\\\ EOF \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/
