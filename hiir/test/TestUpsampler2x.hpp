/*****************************************************************************

        TestUpsampler2x.hpp
        Copyright (c) 2005 Laurent de Soras

--- Legal stuff ---

This program is free software. It comes without any warranty, to
the extent permitted by applicable law. You can redistribute it
and/or modify it under the terms of the Do What The Fuck You Want
To Public License, Version 2, as published by Sam Hocevar. See
http://sam.zoy.org/wtfpl/COPYING for more details.

*Tab=3***********************************************************************/



#if defined (hiir_TestUpsampler2x_CURRENT_CODEHEADER)
	#error Recursive inclusion of TestUpsampler2x code header.
#endif
#define	hiir_TestUpsampler2x_CURRENT_CODEHEADER

#if ! defined (hiir_TestUpsampler2x_CODEHEADER_INCLUDED)
#define	hiir_TestUpsampler2x_CODEHEADER_INCLUDED



/*\\\ INCLUDE FILES \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/

#include	"hiir/test/BlockSplitter.h"
#include	"hiir/test/FileOp.h"
#include	"hiir/test/ResultCheck.h"
#include	"hiir/test/SweepingSine.h"

#include	<cstdio>



namespace hiir
{
namespace test
{



/*\\\ PUBLIC \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/



template <class TO>
int	TestUpsampler2x <TO>::perform_test (TO &uspl, const double coef_arr [NBR_COEFS], const SweepingSine &ss, const char *type_0, double transition_bw, std::vector <float> &dest, double stopband_at)
{
	assert (&uspl != 0);
	assert (coef_arr != 0);
	assert (&ss != 0);
	assert (&type_0 != 0);
	assert (transition_bw > 0);
	assert (transition_bw < 0.5);
	assert (&dest != 0);
	assert (stopband_at > 0);

	using namespace std;

	printf (
		"Test: Upsampler2x, %s implementation, %d coefficients.\n",
		type_0,
		NBR_COEFS
	);

	const long		len = ss.get_len ();
	std::vector <float>	src (len);
	printf ("Generating sweeping sine... ");
	fflush (stdout);
	ss.generate (&src [0]);
	printf ("Done.\n");

	uspl.set_coefs (coef_arr);
	uspl.clear_buffers ();

	const long		len_save = len * 2;
	dest.resize (len_save, 0);

	printf ("Upsampling... ");
	fflush (stdout);
	BlockSplitter	bs (64);
	for (bs.start (len); bs.is_continuing (); bs.set_next_block ())
	{
		const long		b_pos = bs.get_pos ();
		const long		b_len = bs.get_len ();
		uspl.process_block (&dest [b_pos * 2], &src [b_pos], b_len);
	}
	printf ("Done.\n");

	int			ret_val = ResultCheck::check_uspl (
		ss,
		transition_bw,
		stopband_at,
		&dest [0]
	);

	char			filename_0 [255+1];
	sprintf (filename_0, "uspl_%02d_%s.raw", TestedType::NBR_COEFS, type_0);
	FileOp::save_raw_data_16 (filename_0, &dest [0], len_save, 1);
	printf ("\n");

	return (ret_val);
}



/*\\\ PROTECTED \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/



/*\\\ PRIVATE \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/



}	// namespace test
}	// namespace hiir



#endif	// hiir_TestUpsampler2x_CODEHEADER_INCLUDED

#undef hiir_TestUpsampler2x_CURRENT_CODEHEADER



/*\\\ EOF \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/
