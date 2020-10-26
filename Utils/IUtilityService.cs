/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 *
 * distributed under the MIT License.
****/

using ExapisSOP.Binary;
using ExapisSOP.Numerics;
using ExapisSOP.Text;

namespace ExapisSOP.Utils
{
	/// <summary>
	///  アセンブリ'<see cref="ExapisSOP.Utils"/>'で追加される機能をサービスとして提供します。
	/// </summary>
	public interface IUtilityService : IService
	{
		/// <summary>
		///  既定の<see cref="ExapisSOP.Text.SimpleEncoding"/>を取得します。
		/// </summary>
		SimpleEncoding SimpleEncoding { get; }

		/// <summary>
		///  指定されたオブジェクトからデータ値を生成します。
		/// </summary>
		/// <param name="o">データ値に格納するオブジェクトです。</param>
		/// <returns>生成された新しいデータ値のインスタンスです。</returns>
		DataValue CreateDataValue(object o);

		/// <summary>
		///  指定された文字列から単純な文字列を生成します。
		/// </summary>
		/// <param name="s">単純な文字列へ変換する文字列です。</param>
		/// <returns>生成された新しい単純な文字列のインスタンスです。</returns>
		SimpleString CreateSimpleString(string s);

		/// <summary>
		///  指定されたシード値から疑似乱数生成器を生成します。
		/// </summary>
		/// <param name="seed">シード値です。</param>
		/// <returns>生成された新しい疑似乱数生成器のインスタンスです。</returns>
		IRandom CreateRandom(long seed);
	}
}
