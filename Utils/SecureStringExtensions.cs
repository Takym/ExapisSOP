/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System;
using System.Runtime.InteropServices;
using System.Security;
using ExapisSOP.Resources.Utils;

namespace ExapisSOP.Utils
{
	/// <summary>
	///  <see cref="System.Security.SecureString"/>クラスを拡張します。
	///  このクラスは静的です。
	/// </summary>
	public static class SecureStringExtensions
	{
		/// <summary>
		///  二つのセキュリティで保護された文字列が等しいかどうか判定します。
		/// </summary>
		/// <param name="sstr1">判定対象のオブジェクトです。</param>
		/// <param name="sstr2">判定対象のオブジェクトです。</param>
		/// <returns>等しい場合は<see langword="true"/>、等しくない場合は<see langword="false"/>を返します。</returns>
		/// <exception cref="System.ArgumentNullException" />
		/// <exception cref="System.Security.SecureString" />
		public static bool IsEqualWith(this SecureString sstr1, SecureString sstr2)
		{
			sstr1 = sstr1 ?? throw new ArgumentNullException(nameof(sstr1));
			sstr2 = sstr2 ?? throw new ArgumentNullException(nameof(sstr2));
			try {
				if (sstr1 == sstr2) {
					return true;
				}
				if (sstr1.Length != sstr2.Length) {
					return false;
				}
				var p1 = Marshal.SecureStringToBSTR(sstr1);
				var p2 = Marshal.SecureStringToBSTR(sstr2);
				try {
					for (int i = 0; i < sstr1.Length * 2; ++i) {
						byte b1 = Marshal.ReadByte(p1, i);
						byte b2 = Marshal.ReadByte(p2, i);
						if (b1 != b2) {
							return false;
						}
					}
					return true;
				} finally {
					Marshal.ZeroFreeBSTR(p1);
					Marshal.ZeroFreeBSTR(p2);
				}
			} catch (Exception e) {
				throw new SecurityException(StringRes.SecureStringExtensions_SecurityException, e);
			}
		}
	}
}
