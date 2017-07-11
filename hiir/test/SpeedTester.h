/*****************************************************************************

        SpeedTester.h
        Copyright (c) 2005 Laurent de Soras

Template parameters:
	- AP: Auxiliary class for processing. AP::TestedObject is the type of the
tested object.

--- Legal stuff ---

This program is free software. It comes without any warranty, to
the extent permitted by applicable law. You can redistribute it
and/or modify it under the terms of the Do What The Fuck You Want
To Public License, Version 2, as published by Sam Hocevar. See
http://sam.zoy.org/wtfpl/COPYING for more details.

*Tab=3***********************************************************************/



#if ! defined (hiir_test_SpeedTester_HEADER_INCLUDED)
#define	hiir_test_SpeedTester_HEADER_INCLUDED

#if defined (_MSC_VER)
	#pragma once
	#pragma warning (4 : 4250) // "Inherits via dominance."
#endif



/*\\\ INCLUDE FILES \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/

#include	"hiir/test/SpeedTesterBase.h"
#include	"hiir/def.h"



namespace hiir
{
namespace test
{



template <class AP>
class SpeedTester
:	public SpeedTesterBase
{

/*\\\ PUBLIC \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/

public:

	typedef	AP	AuxProc;
	typedef	typename AuxProc::TestedObject	TestedObject;

						SpeedTester ();

	void				perform_test (const char *classname_0, const char *funcname_0);



/*\\\ PROTECTED \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/

protected:



/*\\\ PRIVATE \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/

private:

	enum {			NBR_COEFS	= TestedObject::NBR_COEFS	};

	TestedObject	_tested_object;	// Should be the first object (for alignment)



/*\\\ FORBIDDEN MEMBER FUNCTIONS \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/

private:

						SpeedTester (const SpeedTester &other);
	SpeedTester &	operator = (const SpeedTester &other);
	bool				operator == (const SpeedTester &other);
	bool				operator != (const SpeedTester &other);

};	// class SpeedTester



}	// namespace test
}	// namespace hiir



#include	"hiir/test/SpeedTester.hpp"



#endif	// hiir_test_SpeedTester_HEADER_INCLUDED



/*\\\ EOF \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/
