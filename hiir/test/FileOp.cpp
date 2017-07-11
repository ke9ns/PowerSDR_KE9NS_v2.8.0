/*****************************************************************************

        FileOp.cpp
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

#include	"hiir/test/conf.h"
#include	"hiir/test/FileOp.h"
#include	"hiir/test/fnc.h"
#include	"hiir/Array.h"
#include	"hiir/fnc.h"

#include	<vector>

#include	<cassert>
#include	<cstdio>



namespace hiir
{
namespace test
{



/*\\\ PUBLIC \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/



int	FileOp::save_raw_data_16 (const char *filename_0, const float src_ptr [], long nbr_spl, float scale)
{
	assert (filename_0 != 0);
	assert (filename_0 [0] != '\0');
	assert (src_ptr != 0);
	assert (nbr_spl > 0);
	assert (scale != 0);

	using namespace std;

	int				ret_val = 0;

#if defined (hiir_test_SAVE_RESULTS)

	printf ("Saving %s... ", filename_0);
	fflush (stdout);

	typedef	short	Int16;
	typedef	int	Check16 [(sizeof (Int16) == 2) ? 1 : -1];

	const float		mult = 16384.0f / scale;

	FILE *			f_ptr = fopen (filename_0, "wb");
	if (f_ptr == 0)
	{
		ret_val = -1;
	}
	else
	{
		enum {	BLOCK_LEN = 1024	};

		for (long block_pos = 0
		;	block_pos < nbr_spl && ret_val == 0
		;	block_pos += BLOCK_LEN)
		{
			hiir::Array <Int16, BLOCK_LEN>	buf;
			const long		block_size = get_min (
				nbr_spl - block_pos,
				static_cast <long> (BLOCK_LEN)
			);
			for (long pos = 0; pos < block_size; ++pos)
			{
				float				x = src_ptr [block_pos + pos] * mult;
				x = get_max (get_min (x, 32767.0f), -32768.0f);
				buf [pos] = static_cast <Int16> (round_int (x));
			}

			if (fwrite (&buf [0], sizeof (buf [0]) * block_size, 1, f_ptr) != 1)
			{
				ret_val = -2;
			}
		}
	}

	fclose (f_ptr);

	printf ("Done.\n");

#endif	// hiir_test_SAVE_RESULTS

	return (ret_val);
}



int	FileOp::save_raw_data_16_stereo (const char *filename_0, const float src_0_ptr [], const float src_1_ptr [], long nbr_spl, float scale)
{
	assert (filename_0 != 0);
	assert (filename_0 [0] != '\0');
	assert (src_0_ptr != 0);
	assert (src_1_ptr != 0);
	assert (nbr_spl > 0);
	assert (scale != 0);

	int				ret_val = 0;

#if defined (hiir_test_SAVE_RESULTS)

	std::vector <float>	stereo (nbr_spl * 2);
	for (long pos = 0; pos < nbr_spl; ++pos)
	{
		stereo [pos * 2    ] = src_0_ptr [pos];
		stereo [pos * 2 + 1] = src_1_ptr [pos];
	}

	ret_val = save_raw_data_16 (filename_0, &stereo [0], nbr_spl * 2, scale);

#endif	// hiir_test_SAVE_RESULTS

	return (ret_val);
}



/*\\\ PROTECTED \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/



/*\\\ PRIVATE \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/



}	// namespace test
}	// namespace hiir



/*\\\ EOF \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/
