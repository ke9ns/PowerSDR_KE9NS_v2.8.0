/*****************************************************************************

        TestDownsampler2x.hpp
        Copyright (c) 2005 Laurent de Soras

--- Legal stuff ---

This program is free software. It comes without any warranty, to
the extent permitted by applicable law. You can redistribute it
and/or modify it under the terms of the Do What The Fuck You Want
To Public License, Version 2, as published by Sam Hocevar. See
http://sam.zoy.org/wtfpl/COPYING for more details.

*Tab=3***********************************************************************/



#if defined (hiir_test_TestDownsampler2x_CURRENT_CODEHEADER)
	#error Recursive inclusion of TestDownsampler2x code header.
#endif
#define	hiir_test_TestDownsampler2x_CURRENT_CODEHEADER

#if ! defined (hiir_test_TestDownsampler2x_CODEHEADER_INCLUDED)
#define	hiir_test_TestDownsampler2x_CODEHEADER_INCLUDED



/*\\\ INCLUDE FILES \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/

#include	"hiir/test/BlockSplitter.h"
#include	"hiir/test/FileOp.h"
#include	"hiir/test/ResultCheck.h"
#include	"hiir/test/SweepingSine.h"

#include	<cassert>
#include	<cstdio>



namespace hiir
{
namespace test
{



/*\\\ PUBLIC \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/



template <class TO>
int	TestDownsampler2x <TO>::perform_test (TO &dspl, const double coef_arr [NBR_COEFS], const SweepingSine &ss, const char *type_0, double transition_bw, std::vector <float> &dest, double stopband_at)
{
	assert (&dspl != 0);
	assert (coef_arr != 0);
	assert (&ss != 0);
	assert (&type_0 != 0);
	assert (transition_bw > 0);
	assert (transition_bw < 0.5);
	assert (&dest != 0);
	assert (stopband_at > 0);

	using namespace std;

	printf (
		"Test: Downsampler2x, %s implementation, %d coefficients.\n",
		type_0,
		NBR_COEFS
	);

	const long		len = ss.get_len ();
	std::vector <float>	src (len);
	printf ("Generating sweeping sine... ");
	fflush (stdout);
	ss.generate (&src [0]);
	printf ("Done.\n");

	dspl.set_coefs (coef_arr);
	dspl.clear_buffers ();

	const long		len_proc = len / 2;
	dest.resize (len_proc, 0);

	printf ("Downsampling... ");
	fflush (stdout);
	BlockSplitter	bs (64);
	for (bs.start (len_proc); bs.is_continuing (); bs.set_next_block ())
	{
		const long		b_pos = bs.get_pos ();
		const long		b_len = bs.get_len ();
		dspl.process_block (&dest [b_pos], &src [b_pos * 2], b_len);
	}
	printf ("Done.\n");

	int			ret_val = ResultCheck::check_dspl (
		ss,
		transition_bw,
		stopband_at,
		&dest [0]
	);

	char			filename_0 [255+1];
	sprintf (filename_0, "dspl_%02d_%s.raw", TestedType::NBR_COEFS, type_0);
	FileOp::save_raw_data_16 (filename_0, &dest [0], len_proc, 1);
	printf ("\n");

	return (ret_val);
}



/*\\\ PROTECTED \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/



/*\\\ PRIVATE \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/



}	// namespace test
}	// namespace hiir



#endif	// hiir_test_TestDownsampler2x_CODEHEADER_INCLUDED

#undef hiir_test_TestDownsampler2x_CURRENT_CODEHEADER



/*\\\ EOF \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/
