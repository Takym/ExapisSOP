/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 *
 * distributed under the MIT License.
****/

namespace ExapisSOP.Numerics
{
	partial class RandomExtension
	{
		/// <summary>
		///  最大値を指定して8ビット符号無し整数値を生成します。
		/// </summary>
		/// <param name="random">疑似乱数生成器です。</param>
		/// <param name="max">最大値です。</param>
		/// <returns>結果の分からない値を返します。</returns>
		/// <exception cref="System.ArgumentOutOfRangeException" />
		public static byte NextUInt8(this IRandom random, byte max)
		{
			if (max == 0) {
				ThrowOutOfRange(max, 0);
			}
			return ((byte)(random.NextUInt8() % max));
		}

		/// <summary>
		///  最大値を指定して16ビット符号無し整数値を生成します。
		/// </summary>
		/// <param name="random">疑似乱数生成器です。</param>
		/// <param name="max">最大値です。</param>
		/// <returns>結果の分からない値を返します。</returns>
		/// <exception cref="System.ArgumentOutOfRangeException" />
		public static ushort NextUInt16(this IRandom random, ushort max)
		{
			if (max == 0) {
				ThrowOutOfRange(max, 0);
			}
			return ((ushort)(random.NextUInt16() % max));
		}

		/// <summary>
		///  最大値を指定して32ビット符号無し整数値を生成します。
		/// </summary>
		/// <param name="random">疑似乱数生成器です。</param>
		/// <param name="max">最大値です。</param>
		/// <returns>結果の分からない値を返します。</returns>
		/// <exception cref="System.ArgumentOutOfRangeException" />
		public static uint NextUInt32(this IRandom random, uint max)
		{
			if (max == 0) {
				ThrowOutOfRange(max, 0);
			}
			return random.NextUInt32() % max;
		}

		/// <summary>
		///  最大値を指定して64ビット符号無し整数値を生成します。
		/// </summary>
		/// <param name="random">疑似乱数生成器です。</param>
		/// <param name="max">最大値です。</param>
		/// <returns>結果の分からない値を返します。</returns>
		/// <exception cref="System.ArgumentOutOfRangeException" />
		public static ulong NextUInt64(this IRandom random, ulong max)
		{
			if (max == 0) {
				ThrowOutOfRange(max, 0);
			}
			return random.NextUInt64() % max;
		}

		/// <summary>
		///  最大値を指定して8ビット符号付き整数値を生成します。
		/// </summary>
		/// <param name="random">疑似乱数生成器です。</param>
		/// <param name="max">最大値です。</param>
		/// <returns>結果の分からない値を返します。</returns>
		/// <exception cref="System.ArgumentOutOfRangeException" />
		public static sbyte NextSInt8(this IRandom random, sbyte max)
		{
			if (max <= 0) {
				ThrowOutOfRange(max, 0);
			}
			return ((sbyte)(random.NextSInt8() % max));
		}

		/// <summary>
		///  最大値を指定して16ビット符号付き整数値を生成します。
		/// </summary>
		/// <param name="random">疑似乱数生成器です。</param>
		/// <param name="max">最大値です。</param>
		/// <returns>結果の分からない値を返します。</returns>
		/// <exception cref="System.ArgumentOutOfRangeException" />
		public static short NextSInt16(this IRandom random, short max)
		{
			if (max <= 0) {
				ThrowOutOfRange(max, 0);
			}
			return ((short)(random.NextSInt16() % max));
		}

		/// <summary>
		///  最大値を指定して32ビット符号付き整数値を生成します。
		/// </summary>
		/// <param name="random">疑似乱数生成器です。</param>
		/// <param name="max">最大値です。</param>
		/// <returns>結果の分からない値を返します。</returns>
		/// <exception cref="System.ArgumentOutOfRangeException" />
		public static int NextSInt32(this IRandom random, int max)
		{
			if (max <= 0) {
				ThrowOutOfRange(max, 0);
			}
			return random.NextSInt32() % max;
		}

		/// <summary>
		///  最大値を指定して64ビット符号付き整数値を生成します。
		/// </summary>
		/// <param name="random">疑似乱数生成器です。</param>
		/// <param name="max">最大値です。</param>
		/// <returns>結果の分からない値を返します。</returns>
		/// <exception cref="System.ArgumentOutOfRangeException" />
		public static long NextSInt64(this IRandom random, long max)
		{
			if (max <= 0) {
				ThrowOutOfRange(max, 0);
			}
			return random.NextSInt64() % max;
		}

		/// <summary>
		///  最大値を指定して単精度浮動小数点数値を生成します。
		/// </summary>
		/// <param name="random">疑似乱数生成器です。</param>
		/// <param name="max">最大値です。</param>
		/// <returns>結果の分からない値を返します。</returns>
		/// <exception cref="System.ArgumentOutOfRangeException" />
		public static float NextSingle(this IRandom random, float max)
		{
			if (max <= 0) {
				ThrowOutOfRange(max, 0);
			}
			return random.NextSingle() * max;
		}

		/// <summary>
		///  最大値を指定して単精度浮動小数点数値を生成します。
		/// </summary>
		/// <param name="random">疑似乱数生成器です。</param>
		/// <param name="max">最大値です。</param>
		/// <returns>結果の分からない値を返します。</returns>
		/// <exception cref="System.ArgumentOutOfRangeException" />
		public static double NextDouble(this IRandom random, double max)
		{
			if (max <= 0) {
				ThrowOutOfRange(max, 0);
			}
			return random.NextDouble() * max;
		}

		/// <summary>
		///  最大値を指定して10進数数値を生成します。
		/// </summary>
		/// <param name="random">疑似乱数生成器です。</param>
		/// <param name="max">最大値です。</param>
		/// <returns>結果の分からない値を返します。</returns>
		/// <exception cref="System.ArgumentOutOfRangeException" />
		public static decimal NextDecimal(this IRandom random, decimal max)
		{
			if (max <= 0) {
				ThrowOutOfRange(max, 0);
			}
			return random.NextDecimal() * max;
		}
	}
}
