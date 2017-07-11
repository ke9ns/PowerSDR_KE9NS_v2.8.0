/*****************************************************************************

        SpeedTesterBase.h
        Copyright (c) 2005 Laurent de Soras

--- Legal stuff ---

This program is free software. It comes without any warranty, to
the extent permitted by applicable law. You can redistribute it
and/or modify it under the terms of the Do What The Fuck You Want
To Public License, Version 2, as published by Sam Hocevar. See
http://sam.zoy.org/wtfpl/COPYING for more details.

*Tab=3***********************************************************************/



#if ! defined (hiir_test_SpeedTesterBase_HEADER_INCLUDED)
#define	hiir_test_SpeedTesterBase_HEADER_INCLUDED

#if defined (_MSC_VER)
	#pragma once
	#pragma warning (4 : 4250) // "Inherits via dominance."
#endif



/*\\\ INCLUDE FILES \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/

#include	"hiir/Array.h"



namespace hiir
{
namespace test
{



class SpeedTesterBase
{

/*\\\ PUBLIC \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/

public:

	enum {			MAX_BLOCK_LEN	= 256	};
	enum {			MAX_BUF_LEN		= MAX_BLOCK_LEN * 2	};	// For oversampled data
	enum {			MAX_BUFFERS		= 2	};

	typedef	Array <float, MAX_BUF_LEN>	Buffer;
	typedef	Array <Buffer, MAX_BUFFERS>	BufferArr;

						SpeedTesterBase ();
	virtual			~SpeedTesterBase () {}

	BufferArr		_src_arr;
	BufferArr		_dest_arr;
	long				_block_len;				// ]0 ; MAX_BLOCK_LEN]



/*\\\ PROTECTED \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/

protected:



/*\\\ PRIVATE \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/

private:



/*\\\ FORBIDDEN MEMBER FUNCTIONS \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/

private:

						SpeedTesterBase (const SpeedTesterBase &other);
	SpeedTesterBase &
						operator = (const SpeedTesterBase &other);
	bool				operator == (const SpeedTesterBase &other);
	bool				operator != (const SpeedTesterBase &other);

};	// class SpeedTesterBase



}	// namespace test
}	// namespace hiir



#endif	// hiir_test_SpeedTesterBase_HEADER_INCLUDED



/*\\\ EOF \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/
