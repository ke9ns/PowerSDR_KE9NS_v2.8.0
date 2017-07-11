/*****************************************************************************

        SpeedTester.hpp
        Copyright (c) 2005 Laurent de Soras

--- Legal stuff ---

This program is free software. It comes without any warranty, to
the extent permitted by applicable law. You can redistribute it
and/or modify it under the terms of the Do What The Fuck You Want
To Public License, Version 2, as published by Sam Hocevar. See
http://sam.zoy.org/wtfpl/COPYING for more details.

*Tab=3***********************************************************************/



#if defined (hiir_test_SpeedTester_CURRENT_CODEHEADER)
	#error Recursive inclusion of SpeedTester code header.
#endif
#define	hiir_test_SpeedTester_CURRENT_CODEHEADER

#if ! defined (hiir_test_SpeedTester_CODEHEADER_INCLUDED)
#define	hiir_test_SpeedTester_CODEHEADER_INCLUDED



/*\\\ INCLUDE FILES \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/

#include	"hiir/test/ClockCycleCounter.h"

#include	<cstdio>



namespace hiir
{
namespace test
{



/*\\\ PUBLIC \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/



template <class AP>
SpeedTester <AP>::SpeedTester ()
:	_tested_object ()
{
	// Nothing
}



template <class AP>
void	SpeedTester <AP>::perform_test (const char *classname_0, const char *funcname_0)
{
	assert (classname_0 != 0);
	assert (funcname_0 != 0);

	using namespace std;

	printf (
		"Speed test, %-18s::%-19s, %2d coef:",
		classname_0,
		funcname_0,
		NBR_COEFS
	);

	// Prepares filter
	const double	transition_bw = 0.1 / NBR_COEFS;
	double			coef_arr [NBR_COEFS];
	hiir::PolyphaseIir2Designer::compute_coefs_spec_order_tbw (
		&coef_arr [0],
		NBR_COEFS,
		transition_bw
	);
	_tested_object.set_coefs (&coef_arr [0]);
	_tested_object.clear_buffers ();

	// Does the speed test
	ClockCycleCounter	clk;
	const long		nbr_tests = 1000;

	for (long test_cnt = 0; test_cnt < nbr_tests; ++test_cnt)
	{
		clk.start ();
		AuxProc::process_block (*this, _tested_object);
		clk.stop ();
	}

	// Displays result
	const double	clk_per_spl = clk.get_best_score (_block_len);
	printf ("%8.3f clk/spl\n", clk_per_spl);
}



/*\\\ PROTECTED \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/



/*\\\ PRIVATE \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/



}	// namespace test
}	// namespace hiir



#endif	// hiir_test_SpeedTester_CODEHEADER_INCLUDED

#undef hiir_test_SpeedTester_CURRENT_CODEHEADER



/*\\\ EOF \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/
