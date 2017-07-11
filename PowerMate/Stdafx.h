// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently,
// but are changed infrequently

#pragma once

#define UNICODE

#define WIN32_LEAN_AND_MEAN
#include <windows.h>

//#include <Windowsx.h>
//#include <commctrl.h>

#include <shellapi.h>
//#include <shlwapi.h>

#include <vcclr.h>

extern "C"
{
	#include "setupapi.h"
	#include "hidsdi.h"
}

#pragma comment(lib,"setupapi.lib")
#pragma comment(lib,"hid.lib")

#pragma comment(lib, "user32.lib")

#pragma comment(lib,"shell32.lib")

#undef MessageBox