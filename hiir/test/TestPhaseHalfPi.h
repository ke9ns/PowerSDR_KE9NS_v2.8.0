/*****************************************************************************

        TestPhaseHalfPi.h
        Copyright (c) 2005 Laurent de Soras

--- Legal stuff ---

This program is free software. It comes without any warranty, to
the extent permitted by applicable law. You can redistribute it
and/or modify it under the terms of the Do What The Fuck You Want
To Public License, Version 2, as published by Sam Hocevar. See
http://sam.zoy.org/wtfpl/COPYING for more details.

*Tab=3***********************************************************************/



#if ! defined (hiir_test_TestPhaseHalfPi_HEADER_INCLUDED)
#define	hiir_test_TestPhaseHalfPi_HEADER_INCLUDED

#if defined (_MSC_VER)
	#pragma once
	#pragma warning (4 : 4250) // "Inherits via dominance."
#endif



/*\\\ INCLUDE FILES \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/

#include	<vector>



namespace hiir
{
namespace test
{



class SweepingSine;

template <class TO>
class TestPhaseHalfPi
{

/*\\\ PUBLIC \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/

public:

	typedef	TO	TestedType;

	enum {			NBR_COEFS	= TestedType::NBR_COEFS	};

	static int		perform_test (TO &phaser, const double coef_arr [NBR_COEFS], const SweepingSine &ss, const char *type_0, double transition_bw, std::vector <float> &dest_0, std::vector <float> &dest_1);



/*\\\ PROTECTED \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/

protected:



/*\\\ PRIVATE \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/

private:



/*\\\ FORBIDDEN MEMBER FUNCTIONS \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/

private:

						TestPhaseHalfPi ();
						TestPhaseHalfPi (const TestPhaseHalfPi &other);
	TestPhaseHalfPi &
						operator = (const TestPhaseHalfPi &other);
	bool				operator == (const TestPhaseHalfPi &other);
	bool				operator != (const TestPhaseHalfPi &other);

};	// class TestPhaseHalfPi



}	// namespace test
}	// namespace hiir



#include	"hiir/test/TestPhaseHalfPi.hpp"



#endif	// hiir_test_TestPhaseHalfPi_HEADER_INCLUDED



/*\\\ EOF \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/
