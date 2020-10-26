/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 *
 * distributed under the MIT License.
****/

using System;

namespace ExapisSOP.IO.Logging
{
	/// <summary>
	///  <see cref="ExapisSOP.IO.Logging.ErrorReportBuilder"/>に例外の追加情報を提供します。
	/// </summary>
	public interface ICustomErrorDetailProvider
	{
		/// <summary>
		///  追加情報を可読な翻訳済みの文字列へ変換します。
		/// </summary>
		/// <param name="exception">変換するデータを保持している例外オブジェクトです。</param>
		/// <returns>翻訳済みの文字列です。</returns>
		string GetLocalizedDetail(Exception exception);
	}
}
