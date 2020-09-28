/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

namespace ExapisSOP.Numerics
{
	/// <summary>
	///  疑似乱数を生成する機能を提供します。
	/// </summary>
	public interface IRandom
	{
		/// <summary>
		///  シード値を取得します。
		/// </summary>
		/// <exception cref="System.InvalidOperationException"/>
		long Seed { get; }

		/// <summary>
		///  8ビット符号無し整数値を指定された数だけ生成します。
		/// </summary>
		/// <param name="count">生成する乱数値の個数です。</param>
		/// <returns>結果の分からない値を返します。</returns>
		/// <exception cref="System.ArgumentOutOfRangeException"/>
		byte[] NextBytes(int count);

		/// <summary>
		///  64ビット符号付き整数値を生成します。
		/// </summary>
		/// <returns>結果の分からない値を返します。</returns>
		long NextSInt64();

		/// <summary>
		///  倍精度浮動小数点数値を0～1の範囲で生成します。
		/// </summary>
		/// <returns>結果の分からない値を返します。</returns>
		double NextDouble();
	}
}
