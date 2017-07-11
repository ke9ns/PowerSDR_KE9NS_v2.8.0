/*****************************************************************************

        TestAllClassesSpeed.hpp
        Copyright (c) 2005 Laurent de Soras

--- Legal stuff ---

This program is free software. It comes without any warranty, to
the extent permitted by applicable law. You can redistribute it
and/or modify it under the terms of the Do What The Fuck You Want
To Public License, Version 2, as published by Sam Hocevar. See
http://sam.zoy.org/wtfpl/COPYING for more details.

*Tab=3***********************************************************************/



#if defined (hiir_test_TestAllClassesSpeed_CURRENT_CODEHEADER)
	#error Recursive inclusion of TestAllClassesSpeed code header.
#endif
#define	hiir_test_TestAllClassesSpeed_CURRENT_CODEHEADER

#if ! defined (hiir_test_TestAllClassesSpeed_CODEHEADER_INCLUDED)
#define	hiir_test_TestAllClassesSpeed_CODEHEADER_INCLUDED



/*\\\ INCLUDE FILES \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/

#include	"hiir/test/AlignedObject.h"
#include	"hiir/test/conf.h"
#include	"hiir/test/SpeedTester.h"
#include	"hiir/Downsampler2xFpu.h"
#include	"hiir/PhaseHalfPiFpu.h"
#include	"hiir/Upsampler2xFpu.h"

#if defined (hiir_test_3DNOW)
#include	"hiir/Downsampler2x3dnow.h"
#include	"hiir/PhaseHalfPi3dnow.h"
#include	"hiir/Upsampler2x3dnow.h"
#endif

#if defined (hiir_test_SSE)
#include	"hiir/Downsampler2xSse.h"
#include	"hiir/PhaseHalfPiSse.h"
#include	"hiir/Upsampler2xSse.h"
#endif

#include	<cstdio>

namespace std { }



namespace hiir
{
namespace test
{



template <class TO>
class AuxProc11
{
public:
	typedef	TO	TestedObject;
	static hiir_FORCEINLINE void	process_block (SpeedTesterBase &tester, TO &tested_obj)
	{
		tested_obj.process_block (
			&tester._dest_arr [0] [0],
			&tester._src_arr [0] [0],
			tester._block_len
		);
	}
};

template <class TO>
class AuxProc12
{
public:
	typedef	TO	TestedObject;
	static hiir_FORCEINLINE void	process_block (SpeedTesterBase &tester, TO &tested_obj)
	{
		tested_obj.process_block (
			&tester._dest_arr [0] [0],
			&tester._dest_arr [1] [0],
			&tester._src_arr [0] [0],
			tester._block_len
		);
	}
};

template <class TO>
class AuxProc12Split
{
public:
	typedef	TO	TestedObject;
	static hiir_FORCEINLINE void	process_block (SpeedTesterBase &tester, TO &tested_obj)
	{
		tested_obj.process_block_split (
			&tester._dest_arr [0] [0],
			&tester._dest_arr [1] [0],
			&tester._src_arr [0] [0],
			tester._block_len
		);
	}
};



/*\\\ PUBLIC \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/



template <int NC>
void	TestAllClassesSpeed <NC>::perform_test ()
{
	// Downsampler
	{
		SpeedTester <AuxProc11 <Downsampler2xFpu <NBR_COEFS> > >	st;
		st.perform_test ("Downsampler2xFpu", "process_block");
	}
#if defined (hiir_test_3DNOW)
	{
		SpeedTester <AuxProc11 <Downsampler2x3dnow <NBR_COEFS> > >	st;
		st.perform_test ("Downsampler2x3dnow", "process_block");
	}
#endif
#if defined (hiir_test_SSE)
	{
		typedef	SpeedTester <AuxProc11 <Downsampler2xSse <NBR_COEFS> > >	TestType;
		AlignedObject <TestType>	container;
		TestType &	st = container.use ();
		st.perform_test ("Downsampler2xSse", "process_block");
	}
#endif

	// Downsampler, split
	{
		SpeedTester <AuxProc12Split <Downsampler2xFpu <NBR_COEFS> > >	st;
		st.perform_test ("Downsampler2xFpu", "process_block_split");
	}
#if defined (hiir_test_3DNOW)
	{
		SpeedTester <AuxProc12Split <Downsampler2x3dnow <NBR_COEFS> > >	st;
		st.perform_test ("Downsampler2x3dnow", "process_block_split");
	}
#endif
#if defined (hiir_test_SSE)
	{
		typedef	SpeedTester <AuxProc12Split <Downsampler2xSse <NBR_COEFS> > >	TestType;
		AlignedObject <TestType>	container;
		TestType &	st = container.use ();
		st.perform_test ("Downsampler2xSse", "process_block_split");
	}
#endif

	// Upsampler
	{
		SpeedTester <AuxProc11 <Upsampler2xFpu <NBR_COEFS> > >	st;
		st.perform_test ("Upsampler2xFpu", "process_block");
	}
#if defined (hiir_test_3DNOW)
	{
		SpeedTester <AuxProc11 <Upsampler2x3dnow <NBR_COEFS> > >	st;
		st.perform_test ("Upsampler2x3dnow", "process_block");
	}
#endif
#if defined (hiir_test_SSE)
	{
		typedef	SpeedTester <AuxProc11 <Upsampler2xSse <NBR_COEFS> > >	TestType;
		AlignedObject <TestType>	container;
		TestType &	st = container.use ();
		st.perform_test ("Upsampler2xSse", "process_block");
	}
#endif

	// PhaseHalfPi
	{
		SpeedTester <AuxProc12 <PhaseHalfPiFpu <NBR_COEFS> > >	st;
		st.perform_test ("PhaseHalfPiFpu", "process_block");
	}
#if defined (hiir_test_3DNOW)
	{
		SpeedTester <AuxProc12 <PhaseHalfPi3dnow <NBR_COEFS> > >	st;
		st.perform_test ("PhaseHalfPi3dnow", "process_block");
	}
#endif
#if defined (hiir_test_SSE)
	{
		typedef	SpeedTester <AuxProc12 <PhaseHalfPiSse <NBR_COEFS> > >	TestType;
		AlignedObject <TestType>	container;
		TestType &	st = container.use ();
		st.perform_test ("PhaseHalfPiSse", "process_block");
	}
#endif

	using namespace std;

	printf ("\n");
}



/*\\\ PROTECTED \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/



/*\\\ PRIVATE \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/



}	// namespace test
}	// namespace hiir



#endif	// hiir_test_TestAllClassesSpeed_CODEHEADER_INCLUDED

#undef hiir_test_TestAllClassesSpeed_CURRENT_CODEHEADER



/*\\\ EOF \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/
