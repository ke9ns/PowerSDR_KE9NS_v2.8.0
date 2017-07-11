/*****************************************************************************

        test_conf.h
        Copyright (c) 2005 Laurent de Soras

Depending on your CPU, define/undef symbols in this file.

--- Legal stuff ---

This program is free software. It comes without any warranty, to
the extent permitted by applicable law. You can redistribute it
and/or modify it under the terms of the Do What The Fuck You Want
To Public License, Version 2, as published by Sam Hocevar. See
http://sam.zoy.org/wtfpl/COPYING for more details.

*Tab=3***********************************************************************/



#if ! defined (hiir_test_conf_HEADER_INCLUDED)
#define	hiir_test_conf_HEADER_INCLUDED

#if defined (_MSC_VER)
	#pragma once
	#pragma warning (4 : 4250) // "Inherits via dominance."
#endif



/*\\\ INCLUDE FILES \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/



namespace hiir
{
namespace test
{



// CPU configuration (check and modify this, depending on your CPU)
#define	hiir_test_3DNOW
#define	hiir_test_SSE



// Removes code that isn't available for compilers/architectures
#if defined (__POWERPC__) || defined (__powerpc) || defined (_powerpc)

	#undef	hiir_test_3DNOW
	#undef	hiir_test_SSE

#elif (! defined (_MSC_VER) || defined (_WIN64))

	#undef	hiir_test_3DNOW

#endif



// Testing options
#undef	hiir_test_SAVE_RESULTS
#define	hiir_test_LONG_FUNC_TESTS
#define	hiir_test_LONG_SPEED_TESTS



}	// namespace test
}	// namespace hiir



#endif	// hiir_test_conf_HEADER_INCLUDED



/*\\\ EOF \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/
