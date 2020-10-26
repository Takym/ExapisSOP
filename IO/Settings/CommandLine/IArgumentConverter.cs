/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 *
 * distributed under the MIT License.
****/

namespace ExapisSOP.IO.Settings.CommandLine
{
	/// <summary>
	///  コマンド行引数をオブジェクトへ変換する機能を提供します。
	/// </summary>
	public interface IArgumentConverter
	{
		/// <summary>
		///  コマンド行引数をオブジェクトへ変換します。
		/// </summary>
		/// <param name="args">コマンド行引数の全部または一部を表す文字列配列です。</param>
		/// <returns>変換結果を表す新しいオブジェクトです。</returns>
		object? Convert(params string[] args);
	}

	/// <summary>
	///  コマンド行引数を指定された型'<typeparamref name="T"/>'へ変換する機能を提供します。
	/// </summary>
	/// <typeparam name="T">変換後のオブジェクトです。</typeparam>
	public interface IArgumentConverter<out T> : IArgumentConverter
	{
		/// <summary>
		///  コマンド行引数を指定された型'<typeparamref name="T"/>'へ変換します。
		/// </summary>
		/// <param name="args">コマンド行引数の全部または一部を表す文字列配列です。</param>
		/// <returns>変換結果を表す新しい指定された型'<typeparamref name="T"/>'のオブジェクトです。</returns>
		new T Convert(params string[] args);

#if NETCOREAPP3_1
		object? IArgumentConverter.Convert(params string[] args)
		{
			return this.Convert(args);
		}
#endif
	}
}
