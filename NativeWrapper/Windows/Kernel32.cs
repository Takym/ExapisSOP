/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Runtime.InteropServices;

namespace ExapisSOP.NativeWrapper.Windows
{
	/// <summary>
	///  <see langword="kernel32.dll"/>内のプログラムを呼び出します。
	///  このクラスは静的です。
	/// </summary>
	public static class Kernel32
	{
		/// <summary>
		///  <see langword="kernel32.dll"/>への絶対パスを取得します。
		/// </summary>
		public const string Path = "C:\\Windows\\System32\\kernel32.dll";

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
		[DllImport(Path)]
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
		[DllImport(Path)]
		public static extern int GetLastError();

		// BOOL WINAPI AllocConsole(void);
		[DllImport(Path)]
		public static extern bool AllocConsole();

		// BOOL WINAPI FreeConsole(void);
		[DllImport(Path)]
		public static extern bool FreeConsole();

		// HWND WINAPI GetConsoleWindow(void);
		[DllImport(Path)]
		public static extern IntPtr GetConsoleWindow();
	}
}
