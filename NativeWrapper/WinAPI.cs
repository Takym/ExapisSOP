/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using ExapisSOP.NativeWrapper.Windows;
using ExapisSOP.Resources.NativeWrapper.Windows;

namespace ExapisSOP.NativeWrapper
{
	/// <summary>
	///  Windows API を簡単に呼び出します。
	///  動的関数ではなく静的関数を利用してください。
	///  このクラスは継承できません。
	/// </summary>
	public sealed class WinAPI : INativeCaller
	{
		private WinAPI() { }

		/// <summary>
		///  Windows API の呼び出しが実行できる環境かどうか判定します。
		/// </summary>
		/// <param name="reason">サポートされない場合、その理由を表す例外オブジェクトを返します。</param>
		/// <returns>サポートされる場合は<see langword="true"/>、それ以外の場合は<see langword="false"/>を返します。</returns>
		public bool IsSupported(out PlatformNotSupportedException? reason)
		{
			try {
				EnsurePlatform();
				reason = null;
				return true;
			} catch (PlatformNotSupportedException pnse) {
				reason = pnse;
				return false;
			}
		}

		/// <summary>
		///  既定のインスタンスを取得します。
		/// </summary>
		public static readonly INativeCaller Instance = new WinAPI();

		/// <summary>
		///  Windows API の呼び出しが実行できる環境かどうか判定します。
		/// </summary>
		/// <returns>サポートされる場合は<see langword="true"/>、それ以外の場合は<see langword="false"/>を返します。</returns>
		public static bool IsSupported()
		{
			return Instance.IsSupported();
		}

		[Conditional("NETCOREAPP3_1")]
		private static void EnsurePlatform()
		{
#if NETCOREAPP3_1
			if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ||
				Environment.OSVersion.Platform != PlatformID.Win32NT) {
				throw new PlatformNotSupportedException(WinRes.PlatformNotSupportedException);
			}
#endif
		}

		/// <summary>
		///  前回発生したエラーの翻訳されたエラーメッセージを取得します。
		/// </summary>
		/// <returns>エラーメッセージを表す文字列です。</returns>
		/// <exception cref="System.PlatformNotSupportedException" />
		public static string? GetErrorMessage()
		{
			return GetErrorMessage(Kernel32.GetLastError());
		}

		/// <summary>
		///  指定したエラーコードの翻訳されたエラーメッセージを取得します。
		/// </summary>
		/// <param name="hResult">エラーコードです。</param>
		/// <returns>エラーメッセージを表す文字列です。</returns>
		/// <exception cref="System.PlatformNotSupportedException" />
		public static unsafe string? GetErrorMessage(int hResult)
		{
			EnsurePlatform();
			var lpBuf = IntPtr.Zero;
			Kernel32.FormatMessageW(
				Kernel32.FORMAT_MESSAGE_ALLOCATE_BUFFER |
				Kernel32.FORMAT_MESSAGE_FROM_SYSTEM     |
				Kernel32.FORMAT_MESSAGE_IGNORE_INSERTS,
				IntPtr.Zero,
				unchecked((uint)(hResult)),
				Kernel32.MAKELANGID(Kernel32.LANG_NEUTRAL, Kernel32.SUBLANG_DEFAULT),
				new IntPtr(&lpBuf),
				0, IntPtr.Zero);
			return Marshal.PtrToStringUni(lpBuf)?.Trim();
		}
	}
}
