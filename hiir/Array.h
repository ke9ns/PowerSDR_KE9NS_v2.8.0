/*****************************************************************************

        Array.h
        Copyright (c) 2005 Laurent de Soras

Simple class wrapping static C arrays with bound check.

Template parameters:
   - T: Contained class. Should have T::T() and T::~T()
   - LENGTH: Number of contained elements. > 0.

--- Legal stuff ---

This program is free software. It comes without any warranty, to
the extent permitted by applicable law. You can redistribute it
and/or modify it under the terms of the Do What The Fuck You Want
To Public License, Version 2, as published by Sam Hocevar. See
http://sam.zoy.org/wtfpl/COPYING for more details.

*Tab=3***********************************************************************/



#if ! defined (hiir_Array_HEADER_INCLUDED)
#define	hiir_Array_HEADER_INCLUDED

#if defined (_MSC_VER)
	#pragma once
	#pragma warning (4 : 4250) // "Inherits via dominance."
#endif



/*\\\ INCLUDE FILES \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/



namespace hiir
{



template <class T, long LEN>
class Array
{

	// Template parameter check, not used
	typedef	int	ChkTpl1 [(LEN > 0) ? 1 : -1];

/*\\\ PUBLIC \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/

public:

	typedef	T	Element;

   enum {         LENGTH   = LEN };


						Array () {};
						Array (const Array <T, LEN> &other);
	Array <T, LEN> &
						operator = (const Array <T, LEN> &other);

	const Element&	operator [] (long pos) const;
	Element &		operator [] (long pos);

	static long		size ();



/*\\\ PROTECTED \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/

protected:



/*\\\ PRIVATE \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/

private:

	Element			_data [LENGTH];



/*\\\ FORBIDDEN MEMBER FUNCTIONS \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/

private:

	bool				operator == (const Array &other);
	bool				operator != (const Array &other);

};	// class Array



}	// namespace hiir



#include	"hiir/Array.hpp"



#endif	// hiir_Array_HEADER_INCLUDED



/*\\\ EOF \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/
