//=================================================================
// win32.cs
//=================================================================
// PowerSDR is a C# implementation of a Software Defined Radio.
// Copyright (C) 2003-2013  FlexRadio Systems
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
//
// You may contact us via email at: gpl@flexradio.com.
// Paper mail may be sent to: 
//    FlexRadio Systems
//    4616 W. Howard Lane  Suite 1-150
//    Austin, TX 78728
//    USA
//=================================================================

using System;
using System.Runtime.InteropServices;

namespace PowerSDR
{
	unsafe class Win32
	{
		#region Dll Method Definitions

        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "memcpy")]
		public static extern void memcpy(void *destptr, void *srcptr, int n);

        [DllImport("kernel32.dll",  EntryPoint = "EnterCriticalSection")]
		public static extern void EnterCriticalSection(void *cs_ptr);

        [DllImport("kernel32.dll",  EntryPoint = "LeaveCriticalSection")]
		public static extern void LeaveCriticalSection(void *cs_ptr);

        [DllImport("kernel32.dll",  EntryPoint = "InitializeCriticalSection")]
		public static extern void InitializeCriticalSection(void *cs_ptr);

        [DllImport("kernel32.dll", EntryPoint = "InitializeCriticalSectionAndSpinCount")]
		public static extern int InitializeCriticalSectionAndSpinCount(void *cs_ptr, uint spincount);

        [DllImport("kernel32.dll",  EntryPoint = "DeleteCriticalSection")]
		public static extern byte DeleteCriticalSection(void *cs_ptr);

		[DllImport("kernel32.dll")]
		public static extern IntPtr GetCurrentThread();

        [DllImport("kernel32.dll",  SetLastError = true)] 
		public static extern int SetThreadAffinityMask(IntPtr hThread, IntPtr dwThreadAffinityMask);

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "NewCriticalSection")]
		public static extern void *NewCriticalSection();

        [DllImport("DttSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "DestroyCriticalSection")]
		public static extern void DestroyCriticalSection(void *cs_ptr);

        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "memset")]
		public static extern void memset(void *addr, byte val, int n);

        [DllImport("user32.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetWindowPos")]
		public static extern int SetWindowPos(int hwnd, int hWndInsertAfter, int x, int y, int cx, int cy, int wFlags);

        [DllImport("user32.dll",CallingConvention=CallingConvention.Cdecl)]
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

		#endregion
	}
}