/*****************************************************************************

        AlignedObject.h
        Copyright (c) 2005 Laurent de Soras

Template parameters:
	- T: Object to construct. Should have T::T() and T::~T()

--- Legal stuff ---

This program is free software. It comes without any warranty, to
the extent permitted by applicable law. You can redistribute it
and/or modify it under the terms of the Do What The Fuck You Want
To Public License, Version 2, as published by Sam Hocevar. See
http://sam.zoy.org/wtfpl/COPYING for more details.

*Tab=3***********************************************************************/



#if ! defined (hiir_test_AlignedObject_HEADER_INCLUDED)
#define	hiir_test_AlignedObject_HEADER_INCLUDED

#if defined (_MSC_VER)
	#pragma once
	#pragma warning (4 : 4250) // "Inherits via dominance."
#endif



/*\\\ INCLUDE FILES \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/



namespace hiir
{
namespace test
{



template <class T>
class AlignedObject
{

/*\\\ PUBLIC \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/

public:

	typedef	T	EmbeddedType;

	enum {			ALIGNMENT	= 16	};	// Must be a power of 2

						AlignedObject ();
						~AlignedObject ();

	inline EmbeddedType &
						use ();
	inline const EmbeddedType &
						use () const;



/*\\\ PROTECTED \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/

protected:



/*\\\ PRIVATE \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/

private:

	typedef	char	Zone [sizeof (EmbeddedType) + ALIGNMENT-1];

	Zone				_obj_zone;
	EmbeddedType *	_obj_ptr;



/*\\\ FORBIDDEN MEMBER FUNCTIONS \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/

private:

						AlignedObject (const AlignedObject &other);
	AlignedObject&	operator = (const AlignedObject &other);
	bool				operator == (const AlignedObject &other);
	bool				operator != (const AlignedObject &other);

};	// class AlignedObject



}	// namespace test
}	// namespace hiir



#include	"hiir/test/AlignedObject.hpp"



#endif	// hiir_test_AlignedObject_HEADER_INCLUDED



/*\\\ EOF \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/
