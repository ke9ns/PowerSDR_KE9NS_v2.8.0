/*****************************************************************************

        Array.hpp
        Copyright (c) 2005 Laurent de Soras

--- Legal stuff ---

This program is free software. It comes without any warranty, to
the extent permitted by applicable law. You can redistribute it
and/or modify it under the terms of the Do What The Fuck You Want
To Public License, Version 2, as published by Sam Hocevar. See
http://sam.zoy.org/wtfpl/COPYING for more details.

*Tab=3***********************************************************************/



#if defined (hiir_Array_CURRENT_CODEHEADER)
	#error Recursive inclusion of Array code header.
#endif
#define	hiir_Array_CURRENT_CODEHEADER

#if ! defined (hiir_Array_CODEHEADER_INCLUDED)
#define	hiir_Array_CODEHEADER_INCLUDED



/*\\\ INCLUDE FILES \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/

#include	<cassert>



namespace hiir
{



/*\\\ PUBLIC \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/



template <class T, long LEN>
Array <T, LEN>::Array (const Array <T, LEN> &other)
{
	for (long pos = 0; pos < LENGTH; ++pos)
	{
		_data [pos] = other._data [pos];
	}
}



template <class T, long LEN>
Array <T, LEN> &	Array <T, LEN>::operator = (const Array <T, LEN> &other)
{
	for (long pos = 0; pos < LENGTH; ++pos)
	{
		_data [pos] = other._data [pos];
	}

	return (*this);
}



template <class T, long LEN>
const typename Array <T, LEN>::Element &	Array <T, LEN>::operator [] (long pos) const
{
	assert (pos >= 0);
	assert (pos < LENGTH);

	return (_data [pos]);
}



template <class T, long LEN>
typename Array <T, LEN>::Element &	Array <T, LEN>::operator [] (long pos)
{
	assert (pos >= 0);
	assert (pos < LENGTH);

	return (_data [pos]);
}



template <class T, long LEN>
long	Array <T, LEN>::size ()
{
	return (LENGTH);
}



/*\\\ PROTECTED \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/



/*\\\ PRIVATE \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/



}	// namespace hiir



#endif	// hiir_Array_CODEHEADER_INCLUDED

#undef hiir_Array_CURRENT_CODEHEADER



/*\\\ EOF \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/
