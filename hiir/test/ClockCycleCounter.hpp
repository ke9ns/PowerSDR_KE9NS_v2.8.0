/*****************************************************************************

        ClockCycleCounter.hpp
        Copyright (c) 2005 Laurent de Soras

--- Legal stuff ---

This program is free software. It comes without any warranty, to
the extent permitted by applicable law. You can redistribute it
and/or modify it under the terms of the Do What The Fuck You Want
To Public License, Version 2, as published by Sam Hocevar. See
http://sam.zoy.org/wtfpl/COPYING for more details.

*Tab=3***********************************************************************/



#if defined (hiir_test_ClockCycleCounter_CURRENT_CODEHEADER)
	#error Recursive inclusion of ClockCycleCounter code header.
#endif
#define	hiir_test_ClockCycleCounter_CURRENT_CODEHEADER

#if ! defined (hiir_test_ClockCycleCounter_CODEHEADER_INCLUDED)
#define	hiir_test_ClockCycleCounter_CODEHEADER_INCLUDED



/*\\\ INCLUDE FILES \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/

#include	<cassert>

#if (defined (_MSC_VER) && defined (_WIN64))
#include	<intrin.h>
#endif


namespace hiir
{
namespace test
{



/*\\\ PUBLIC \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/



void	ClockCycleCounter::start ()
{
	_start = read_clock_counter ();
}



void	ClockCycleCounter::stop ()
{
	_end = read_clock_counter ();
	const Int64		duration = _end - _start;
	assert (duration >= 0);

	if (_best < 0)
	{
		_best = duration;
	}
	else if (duration < _best)
	{
		_best = duration;
	}
}



/*\\\ PROTECTED \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/



/*\\\ PRIVATE \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/



ClockCycleCounter::Int64	ClockCycleCounter::read_clock_counter ()
{
	register Int64		clock_cnt;

#if defined (_MSC_VER)

	#if defined (_WIN64)

	clock_cnt = __rdtsc ();

	#else

	__asm
	{
		lea				edi, clock_cnt
		rdtsc
		mov				[edi    ], eax
		mov				[edi + 4], edx
	}

	#endif

#elif defined (__GNUC__) && defined (__i386__)

	__asm__ __volatile__ ("rdtsc" : "=A" (clock_cnt));

#elif defined (__GNUC__) && defined (__x86_64__)
	
	clock_cnt = __rdtsc ();
	
#elif (__MWERKS__) && defined (__POWERPC__) 
	
	asm
	{
	loop:
		mftbu			clock_cnt@hiword
		mftb			clock_cnt@loword
		mftbu			r5
		cmpw			clock_cnt@hiword,r5
		bne			loop
	}
	
#endif

	return (clock_cnt);
}



}	// namespace test
}	// namespace hiir



#endif	// hiir_test_ClockCycleCounter_CODEHEADER_INCLUDED

#undef hiir_test_ClockCycleCounter_CURRENT_CODEHEADER



/*\\\ EOF \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/
