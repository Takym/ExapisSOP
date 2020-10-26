/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Diagnostics;
using ExapisSOP.Resources.Utils;

namespace ExapisSOP.Numerics
{
	/// <summary>
	///  <see cref="ExapisSOP.Numerics.IRandom"/>の機能を拡張します。
	///  このクラスは静的です。
	/// </summary>
	public static partial class RandomExtension
	{
		/// <summary>
		///  8ビット符号無し整数値を生成します。
		/// </summary>
		/// <param name="random">疑似乱数生成器です。</param>
		/// <returns>結果の分からない値を返します。</returns>
		public static byte NextUInt8(this IRandom random)
		{
			return random.NextBytes(1)[0];
		}

		/// <summary>
		///  16ビット符号無し整数値を生成します。
		/// </summary>
		/// <param name="random">疑似乱数生成器です。</param>
		/// <returns>結果の分からない値を返します。</returns>
		public static ushort NextUInt16(this IRandom random)
		{
			return BitConverter.ToUInt16(random.NextBytes(2), 0);
		}

		/// <summary>
		///  32ビット符号無し整数値を生成します。
		/// </summary>
		/// <param name="random">疑似乱数生成器です。</param>
		/// <returns>結果の分からない値を返します。</returns>
		public static uint NextUInt32(this IRandom random)
		{
			return BitConverter.ToUInt32(random.NextBytes(4), 0);
		}

		/// <summary>
		///  64ビット符号無し整数値を生成します。
		/// </summary>
		/// <param name="random">疑似乱数生成器です。</param>
		/// <returns>結果の分からない値を返します。</returns>
		public static ulong NextUInt64(this IRandom random)
		{
			return unchecked((ulong)(random.NextSInt64()));
		}

		/// <summary>
		///  8ビット符号付き整数値を生成します。
		/// </summary>
		/// <param name="random">疑似乱数生成器です。</param>
		/// <returns>結果の分からない値を返します。</returns>
		public static sbyte NextSInt8(this IRandom random)
		{
			return unchecked((sbyte)(random.NextBytes(1)[0]));
		}

		/// <summary>
		///  16ビット符号付き整数値を生成します。
		/// </summary>
		/// <param name="random">疑似乱数生成器です。</param>
		/// <returns>結果の分からない値を返します。</returns>
		public static short NextSInt16(this IRandom random)
		{
			return BitConverter.ToInt16(random.NextBytes(2), 0);
		}

		/// <summary>
		///  32ビット符号付き整数値を生成します。
		/// </summary>
		/// <param name="random">疑似乱数生成器です。</param>
		/// <returns>結果の分からない値を返します。</returns>
		public static int NextSInt32(this IRandom random)
		{
			return BitConverter.ToInt32(random.NextBytes(4), 0);
		}

		/// <summary>
		///  単精度浮動小数点数値を0～1の範囲で生成します。
		/// </summary>
		/// <param name="random">疑似乱数生成器です。</param>
		/// <returns>結果の分からない値を返します。</returns>
		public static float NextSingle(this IRandom random)
		{
			//return 1.0F / random.NextSInt64();
			return Convert.ToSingle(random.NextDouble());
		}

		/// <summary>
		///  10進数数値を生成します。
		/// </summary>
		/// <param name="random">疑似乱数生成器です。</param>
		/// <returns>結果の分からない値を返します。</returns>
		public static decimal NextDecimal(this IRandom random)
		{
			//return 1.0M / random.NextSInt64() + random.NextSInt64();
			return new decimal(random.NextDouble()) + new decimal(random.NextSInt64());
		}

		[DebuggerHidden()]
		[StackTraceHidden()]
		private static void ThrowOutOfRange(object max, object min)
		{
			throw new ArgumentOutOfRangeException(
				nameof(max), max,
				string.Format(StringRes.RandomExtension_ArgumentOutOfRangeException, max, min)
			);
		}
	}
}
