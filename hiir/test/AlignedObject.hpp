/*****************************************************************************

        AlignedObject.hpp
        Copyright (c) 2005 Laurent de Soras

--- Legal stuff ---

This program is free software. It comes without any warranty, to
the extent permitted by applicable law. You can redistribute it
and/or modify it under the terms of the Do What The Fuck You Want
To Public License, Version 2, as published by Sam Hocevar. See
http://sam.zoy.org/wtfpl/COPYING for more details.

*Tab=3***********************************************************************/



#if defined (hiir_test_AlignedObject_CURRENT_CODEHEADER)
	#error Recursive inclusion of AlignedObject code header.
#endif
#define	hiir_test_AlignedObject_CURRENT_CODEHEADER

#if ! defined (hiir_test_AlignedObject_CODEHEADER_INCLUDED)
#define	hiir_test_AlignedObject_CODEHEADER_INCLUDED



/*\\\ INCLUDE FILES \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/

#include	<cassert>



namespace hiir
{
namespace test
{



/*\\\ PUBLIC \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/



template <class T>
AlignedObject <T>::AlignedObject ()
:/*	_obj_zone ()
,*/	_obj_ptr (0)
{
	// Check, not used
	typedef	int	ChkType1 [(sizeof (ptrdiff_t) >= sizeof (void *)) ? 1 : -1];

	// Pointer alignment
	ptrdiff_t		ptr = reinterpret_cast <ptrdiff_t> (&_obj_zone [0]);
	ptr = (ptr + ALIGNMENT-1) & -ALIGNMENT;
	_obj_ptr = reinterpret_cast <EmbeddedType *> (ptr);

	// Object creation
	new (_obj_ptr) EmbeddedType;
}



template <class T>
AlignedObject <T>::~AlignedObject ()
{
	if (_obj_ptr != 0)
	{
		_obj_ptr->~T ();
	}
}



template <class T>
typename AlignedObject <T>::EmbeddedType &	AlignedObject <T>::use ()
{
	assert (_obj_ptr != 0);

	return (*_obj_ptr);
}



template <class T>
const typename AlignedObject <T>::EmbeddedType &	AlignedObject <T>::use () const
{
	assert (_obj_ptr != 0);

	return (*_obj_ptr);
}



/*\\\ PROTECTED \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/



/*\\\ PRIVATE \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/



}	// namespace test
}	// namespace hiir



#endif	// hiir_test_AlignedObject_CODEHEADER_INCLUDED

#undef hiir_test_AlignedObject_CURRENT_CODEHEADER



/*\\\ EOF \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/
