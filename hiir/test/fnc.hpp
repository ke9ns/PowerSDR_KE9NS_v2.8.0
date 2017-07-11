/*****************************************************************************

        test_fnc.hpp
        Copyright (c) 2005 Laurent de Soras

--- Legal stuff ---

This program is free software. It comes without any warranty, to
the extent permitted by applicable law. You can redistribute it
and/or modify it under the terms of the Do What The Fuck You Want
To Public License, Version 2, as published by Sam Hocevar. See
http://sam.zoy.org/wtfpl/COPYING for more details.

*Tab=3***********************************************************************/



#if defined (hiir_test_fnc_CURRENT_CODEHEADER)
	#error Recursive inclusion of test_fnc code header.
#endif
#define	hiir_test_fnc_CURRENT_CODEHEADER

#if ! defined (hiir_test_fnc_CODEHEADER_INCLUDED)
#define	hiir_test_fnc_CODEHEADER_INCLUDED



/*\\\ INCLUDE FILES \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/



namespace hiir
{
namespace test
{



template <typename T>
T	get_min (T a, T b)
{
	return ((a < b) ? a : b);
}



template <typename T>
T	get_max (T a, T b)
{
	return ((a < b) ? b : a);
}



}	// namespace test
}	// namespace hiir



#endif	// hiir_test_fnc_CODEHEADER_INCLUDED

#undef hiir_test_fnc_CURRENT_CODEHEADER



/*\\\ EOF \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/
