/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System;
using System.ComponentModel;
using ExapisSOP.NativeWrapper;

namespace ExapisSOP.IO.Logging
{
	/// <summary>
	///  <see cref="ExapisSOP.IO.Logging.ErrorReportBuilder"/>にH-RESULT情報を提供します。
	///  このクラスは継承できません。
	/// </summary>
	public sealed class HResultDetailProvider : ICustomErrorDetailProvider
	{
		/// <summary>
		///  型'<see cref="ExapisSOP.IO.Logging.HResultDetailProvider"/>'の新しいインスタンスを生成します。
		/// </summary>
		public HResultDetailProvider() { }

		/// <summary>
		///  追加情報を可読な翻訳済みの文字列へ変換します。
		/// </summary>
		/// <param name="exception">変換するデータを保持している例外オブジェクトです。</param>
		/// <returns>翻訳済みの文字列です。</returns>
		public string GetLocalizedDetail(Exception exception)
		{
			if (WinAPI.IsSupported()) {
				if (exception is Win32Exception w32e) {
					return $"H-RESULT: {WinAPI.GetErrorMessage(w32e.HResult)}"
						+ Environment.NewLine + $"H-RESULT (Win32): {WinAPI.GetErrorMessage(w32e.ErrorCode)}";
				} else {
					return $"H-RESULT: {WinAPI.GetErrorMessage(exception.HResult)}";
				}
			} else {
				return "The platform does not support to format-message the H-RESULT error code.";
			}
		}
	}
}
