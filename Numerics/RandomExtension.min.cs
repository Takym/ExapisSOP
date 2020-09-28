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
		public static byte NextUInt8(this IRandom random, byte max, byte min)
		{
			return ((byte)(random.NextUInt8((byte)(max - min)) + min));
		}

		/// <summary>
		///  最大値と最小値を指定して16ビット符号無し整数値を生成します。
		/// </summary>
		/// <param name="random">疑似乱数生成器です。</param>
		/// <param name="max">最大値です。</param>
		/// <param name="min">最小値です。</param>
		/// <returns>結果の分からない値を返します。</returns>
		public static ushort NextUInt16(this IRandom random, ushort max, ushort min)
		{
			return ((ushort)(random.NextUInt16((ushort)(max - min)) + min));
		}

		/// <summary>
		///  最大値と最小値を指定して32ビット符号無し整数値を生成します。
		/// </summary>
		/// <param name="random">疑似乱数生成器です。</param>
		/// <param name="max">最大値です。</param>
		/// <param name="min">最小値です。</param>
		/// <returns>結果の分からない値を返します。</returns>
		public static uint NextUInt32(this IRandom random, uint max, uint min)
		{
			return random.NextUInt32(max - min) + min;
		}

		/// <summary>
		///  最大値と最小値を指定して64ビット符号無し整数値を生成します。
		/// </summary>
		/// <param name="random">疑似乱数生成器です。</param>
		/// <param name="max">最大値です。</param>
		/// <param name="min">最小値です。</param>
		/// <returns>結果の分からない値を返します。</returns>
		public static ulong NextUInt64(this IRandom random, ulong max, ulong min)
		{
			return random.NextUInt64(max - min) + min;
		}

		/// <summary>
		///  最大値と最小値を指定して8ビット符号付き整数値を生成します。
		/// </summary>
		/// <param name="random">疑似乱数生成器です。</param>
		/// <param name="max">最大値です。</param>
		/// <param name="min">最小値です。</param>
		/// <returns>結果の分からない値を返します。</returns>
		public static sbyte NextSInt8(this IRandom random, sbyte max, sbyte min)
		{
			return ((sbyte)(random.NextSInt8((sbyte)(max - min)) + min));
		}

		/// <summary>
		///  最大値と最小値を指定して16ビット符号付き整数値を生成します。
		/// </summary>
		/// <param name="random">疑似乱数生成器です。</param>
		/// <param name="max">最大値です。</param>
		/// <param name="min">最小値です。</param>
		/// <returns>結果の分からない値を返します。</returns>
		public static short NextSInt16(this IRandom random, short max, short min)
		{
			return ((short)(random.NextSInt16((short)(max - min)) + min));
		}

		/// <summary>
		///  最大値と最小値を指定して32ビット符号付き整数値を生成します。
		/// </summary>
		/// <param name="random">疑似乱数生成器です。</param>
		/// <param name="max">最大値です。</param>
		/// <param name="min">最小値です。</param>
		/// <returns>結果の分からない値を返します。</returns>
		public static int NextSInt32(this IRandom random, int max, int min)
		{
			return random.NextSInt32(max - min) + min;
		}

		/// <summary>
		///  最大値と最小値を指定して64ビット符号付き整数値を生成します。
		/// </summary>
		/// <param name="random">疑似乱数生成器です。</param>
		/// <param name="max">最大値です。</param>
		/// <param name="min">最小値です。</param>
		/// <returns>結果の分からない値を返します。</returns>
		public static long NextSInt64(this IRandom random, long max, long min)
		{
			return random.NextSInt64(max - min) + min;
		}

		/// <summary>
		///  最大値と最小値を指定して単精度浮動小数点数値を生成します。
		/// </summary>
		/// <param name="random">疑似乱数生成器です。</param>
		/// <param name="max">最大値です。</param>
		/// <param name="min">最小値です。</param>
		/// <returns>結果の分からない値を返します。</returns>
		public static float NextSingle(this IRandom random, float max, float min)
		{
			return random.NextSingle(max - min) + min;
		}

		/// <summary>
		///  最大値と最小値を指定して単精度浮動小数点数値を生成します。
		/// </summary>
		/// <param name="random">疑似乱数生成器です。</param>
		/// <param name="max">最大値です。</param>
		/// <param name="min">最小値です。</param>
		/// <returns>結果の分からない値を返します。</returns>
		public static double NextDouble(this IRandom random, double max, double min)
		{
			return random.NextDouble(max - min) + min;
		}

		/// <summary>
		///  最大値と最小値を指定して10進数数値を生成します。
		/// </summary>
		/// <param name="random">疑似乱数生成器です。</param>
		/// <param name="max">最大値です。</param>
		/// <param name="min">最小値です。</param>
		/// <returns>結果の分からない値を返します。</returns>
		public static decimal NextDecimal(this IRandom random, decimal max, decimal min)
		{
			return random.NextDecimal(max - min) + min;
		}
	}
}
