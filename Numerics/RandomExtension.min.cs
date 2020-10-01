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
		///  最大値と最小値を指定して8ビット符号無し整数値を生成します。
		/// </summary>
		/// <param name="random">疑似乱数生成器です。</param>
		/// <param name="max">最大値です。</param>
		/// <param name="min">最小値です。</param>
		/// <returns>結果の分からない値を返します。</returns>
		/// <exception cref="System.ArgumentOutOfRangeException" />
		public static byte NextUInt8(this IRandom random, byte max, byte min)
		{
			if (max <= min) {
				ThrowOutOfRange(max, min);
			}
			return ((byte)(random.NextUInt8((byte)(max - min)) + min));
		}

		/// <summary>
		///  最大値と最小値を指定して16ビット符号無し整数値を生成します。
		/// </summary>
		/// <param name="random">疑似乱数生成器です。</param>
		/// <param name="max">最大値です。</param>
		/// <param name="min">最小値です。</param>
		/// <returns>結果の分からない値を返します。</returns>
		/// <exception cref="System.ArgumentOutOfRangeException" />
		public static ushort NextUInt16(this IRandom random, ushort max, ushort min)
		{
			if (max <= min) {
				ThrowOutOfRange(max, min);
			}
			return ((ushort)(random.NextUInt16((ushort)(max - min)) + min));
		}

		/// <summary>
		///  最大値と最小値を指定して32ビット符号無し整数値を生成します。
		/// </summary>
		/// <param name="random">疑似乱数生成器です。</param>
		/// <param name="max">最大値です。</param>
		/// <param name="min">最小値です。</param>
		/// <returns>結果の分からない値を返します。</returns>
		/// <exception cref="System.ArgumentOutOfRangeException" />
		public static uint NextUInt32(this IRandom random, uint max, uint min)
		{
			if (max <= min) {
				ThrowOutOfRange(max, min);
			}
			return random.NextUInt32(max - min) + min;
		}

		/// <summary>
		///  最大値と最小値を指定して64ビット符号無し整数値を生成します。
		/// </summary>
		/// <param name="random">疑似乱数生成器です。</param>
		/// <param name="max">最大値です。</param>
		/// <param name="min">最小値です。</param>
		/// <returns>結果の分からない値を返します。</returns>
		/// <exception cref="System.ArgumentOutOfRangeException" />
		public static ulong NextUInt64(this IRandom random, ulong max, ulong min)
		{
			if (max <= min) {
				ThrowOutOfRange(max, min);
			}
			return random.NextUInt64(max - min) + min;
		}

		/// <summary>
		///  最大値と最小値を指定して8ビット符号付き整数値を生成します。
		/// </summary>
		/// <param name="random">疑似乱数生成器です。</param>
		/// <param name="max">最大値です。</param>
		/// <param name="min">最小値です。</param>
		/// <returns>結果の分からない値を返します。</returns>
		/// <exception cref="System.ArgumentOutOfRangeException" />
		public static sbyte NextSInt8(this IRandom random, sbyte max, sbyte min)
		{
			if (max <= min) {
				ThrowOutOfRange(max, min);
			}
			return ((sbyte)(random.NextSInt8((sbyte)(max - min)) + min));
		}

		/// <summary>
		///  最大値と最小値を指定して16ビット符号付き整数値を生成します。
		/// </summary>
		/// <param name="random">疑似乱数生成器です。</param>
		/// <param name="max">最大値です。</param>
		/// <param name="min">最小値です。</param>
		/// <returns>結果の分からない値を返します。</returns>
		/// <exception cref="System.ArgumentOutOfRangeException" />
		public static short NextSInt16(this IRandom random, short max, short min)
		{
			if (max <= min) {
				ThrowOutOfRange(max, min);
			}
			return ((short)(random.NextSInt16((short)(max - min)) + min));
		}

		/// <summary>
		///  最大値と最小値を指定して32ビット符号付き整数値を生成します。
		/// </summary>
		/// <param name="random">疑似乱数生成器です。</param>
		/// <param name="max">最大値です。</param>
		/// <param name="min">最小値です。</param>
		/// <returns>結果の分からない値を返します。</returns>
		/// <exception cref="System.ArgumentOutOfRangeException" />
		public static int NextUInt32(this IRandom random, int max, int min)
		{
			if (max <= min) {
				ThrowOutOfRange(max, min);
			}
			return random.NextSInt32(max - min) + min;
		}

		/// <summary>
		///  最大値と最小値を指定して64ビット符号付き整数値を生成します。
		/// </summary>
		/// <param name="random">疑似乱数生成器です。</param>
		/// <param name="max">最大値です。</param>
		/// <param name="min">最小値です。</param>
		/// <returns>結果の分からない値を返します。</returns>
		/// <exception cref="System.ArgumentOutOfRangeException" />
		public static long NextSInt64(this IRandom random, long max, long min)
		{
			if (max <= min) {
				ThrowOutOfRange(max, min);
			}
			return random.NextSInt64(max - min) + min;
		}

		/// <summary>
		///  最大値と最小値を指定して単精度浮動小数点数値を生成します。
		/// </summary>
		/// <param name="random">疑似乱数生成器です。</param>
		/// <param name="max">最大値です。</param>
		/// <param name="min">最小値です。</param>
		/// <returns>結果の分からない値を返します。</returns>
		/// <exception cref="System.ArgumentOutOfRangeException" />
		public static float NextSingle(this IRandom random, float max, float min)
		{
			if (max <= min) {
				ThrowOutOfRange(max, min);
			}
			return random.NextSingle(max - min) + min;
		}

		/// <summary>
		///  最大値と最小値を指定して単精度浮動小数点数値を生成します。
		/// </summary>
		/// <param name="random">疑似乱数生成器です。</param>
		/// <param name="max">最大値です。</param>
		/// <param name="min">最小値です。</param>
		/// <returns>結果の分からない値を返します。</returns>
		/// <exception cref="System.ArgumentOutOfRangeException" />
		public static double NextDouble(this IRandom random, double max, double min)
		{
			if (max <= min) {
				ThrowOutOfRange(max, min);
			}
			return random.NextDouble(max - min) + min;
		}

		/// <summary>
		///  最大値と最小値を指定して10進数数値を生成します。
		/// </summary>
		/// <param name="random">疑似乱数生成器です。</param>
		/// <param name="max">最大値です。</param>
		/// <param name="min">最小値です。</param>
		/// <returns>結果の分からない値を返します。</returns>
		/// <exception cref="System.ArgumentOutOfRangeException" />
		public static decimal NextDecimal(this IRandom random, decimal max, decimal min)
		{
			if (max <= min) {
				ThrowOutOfRange(max, min);
			}
			return random.NextDecimal(max - min) + min;
		}
	}
}
