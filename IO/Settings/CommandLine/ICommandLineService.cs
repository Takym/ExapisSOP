/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Collections.Generic;

namespace ExapisSOP.IO.Settings.CommandLine
{
	/// <summary>
	///  コマンド行引数を解析する機能を提供します。
	/// </summary>
	public interface ICommandLineService : IService
	{
		/// <summary>
		///  コマンド行引数に誤りがある場合に終了例外を発生させます。
		/// </summary>
		/// <exception cref="ExapisSOP.TerminationException"/>
		void TerminateWhenError();

		/// <summary>
		///  オブジェクトへ変換されたコマンド行引数を取得します。
		/// </summary>
		/// <returns>
		///  オブジェクトを格納した辞書、または、変換が実行されなかった場合は<see langword="null"/>を返します。
		/// </returns>
		IDictionary<Type, object>? GetValues();

		/// <summary>
		///  コマンド行引数説明書またはバージョン情報を標準出力ストリームへ書き込みます。
		/// </summary>
		/// <param name="context">現在の文脈情報です。</param>
		/// <param name="copyright">アプリケーションの著作権情報です。</param>
		void PrintManuals(IContext context, string copyright);
	}
}
