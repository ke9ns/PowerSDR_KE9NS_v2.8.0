/*****************************************************************************

        ClockCycleCounter.h
        Copyright (c) 2005 Laurent de Soras

--- Legal stuff ---

This program is free software. It comes without any warranty, to
the extent permitted by applicable law. You can redistribute it
and/or modify it under the terms of the Do What The Fuck You Want
To Public License, Version 2, as published by Sam Hocevar. See
http://sam.zoy.org/wtfpl/COPYING for more details.

*Tab=3***********************************************************************/



#if ! defined (hiir_test_ClockCycleCounter_HEADER_INCLUDED)
#define	hiir_test_ClockCycleCounter_HEADER_INCLUDED

#if defined (_MSC_VER)
	#pragma once
	#pragma warning (4 : 4250) // "Inherits via dominance."
#endif



/*\\\ INCLUDE FILES \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/

#include	"hiir/def.h"

#if defined (__GNUC__)
#include <stdint.h>
#endif



namespace hiir
{
namespace test
{



class ClockCycleCounter
{

/*\\\ PUBLIC \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/

public:

#if defined (_MSC_VER)

	typedef	__int64	Int64;

#elif defined (__GNUC__)

	typedef	int64_t	Int64;

#elif defined (__MWERKS__)

	typedef	long long	Int64;

#elif defined (__BEOS__)

	typedef	int64	Int64;

#else

	#error No 64-bit integer type defined for this compiler !

#endif

						ClockCycleCounter ();
	virtual			~ClockCycleCounter () {}

	hiir_FORCEINLINE void
						start ();
	hiir_FORCEINLINE void
						stop ();
	double			get_best_score (long nbr_spl) const;



/*\\\ PROTECTED \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/

protected:



/*\\\ PRIVATE \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/

private:

	static hiir_FORCEINLINE Int64
						read_clock_counter ();

	Int64				_start;
	Int64				_end;
	Int64				_best;

	static long		_clk_mul;
	static bool		_init_flag;



/*\\\ FORBIDDEN MEMBER FUNCTIONS \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/

private:

						ClockCycleCounter (const ClockCycleCounter &other);
	ClockCycleCounter &
						operator = (const ClockCycleCounter &other);
	bool				operator == (const ClockCycleCounter &other);
	bool				operator != (const ClockCycleCounter &other);

};	// class ClockCycleCounter



}	// namespace test
}	// namespace hiir



#include	"hiir/test/ClockCycleCounter.hpp"



#endif	// hiir_test_ClockCycleCounter_HEADER_INCLUDED



/*\\\ EOF \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/
