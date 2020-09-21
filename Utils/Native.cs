/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

#if NET48
using System;
using System.Runtime.InteropServices;

namespace ExapisSOP.Utils
{
	/// <summary>
	///  ネイティブコードを呼び出します。
	/// </summary>
	internal unsafe static class Native
	{
		public const string Kernel32 = "C:\\Windows\\System32\\kernel32.dll";

		public static string? GetErrorMessage(int hResult)
		{
			var lpBuf = IntPtr.Zero;
			FormatMessageW(
				FORMAT_MESSAGE_ALLOCATE_BUFFER |
				FORMAT_MESSAGE_FROM_SYSTEM |
				FORMAT_MESSAGE_IGNORE_INSERTS,
				IntPtr.Zero,
				unchecked((uint)(hResult)),
				MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT),
				new IntPtr(&lpBuf),
				0, IntPtr.Zero);
			return Marshal.PtrToStringUni(lpBuf)?.Trim();
		}

		public const ushort LANG_NEUTRAL    = 0x00;
		public const ushort SUBLANG_DEFAULT = 0x01;
		public static uint MAKELANGID(ushort p, ushort s)
		{
			return ((uint)((s << 10) | p));
		}

		public const uint FORMAT_MESSAGE_ALLOCATE_BUFFER = 0x00000100;
		public const uint FORMAT_MESSAGE_IGNORE_INSERTS  = 0x00000200;
		public const uint FORMAT_MESSAGE_FROM_SYSTEM     = 0x00001000;

		// DWORD FormatMessageW(
		//   DWORD   dwFlags,
		//   LPCVOID lpSource,
		//   DWORD   dwMessageId,
		//   DWORD   dwLanguageId,
		//   LPWSTR  lpBuffer,
		//   DWORD   nSize,
		//   va_list *Arguments
		// );
		[DllImport(Kernel32)]
		public static extern uint FormatMessageW(
			uint   dwFlags,
			IntPtr lpSource,
			uint   dwMessageId,
			uint   dwLanguageId,
			IntPtr lpBuffer,
			uint   nSize,
			IntPtr Arguments
		);

		// _Post_equals_last_error_ DWORD GetLastError();
		[DllImport(Kernel32)]
		public static extern int GetLastError();

		// BOOL WINAPI AllocConsole(void);
		[DllImport(Kernel32)]
		public static extern bool AllocConsole();

		// BOOL WINAPI FreeConsole(void);
		[DllImport(Kernel32)]
		public static extern bool FreeConsole();

		// HWND WINAPI GetConsoleWindow(void);
		[DllImport(Kernel32)]
		public static extern IntPtr GetConsoleWindow();
	}
}
#endif
