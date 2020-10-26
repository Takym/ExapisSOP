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
	///  ログレベルを表します。
	/// </summary>
	[Serializable()]
	public enum LogLevel : byte
	{
		/// <summary>
		///  未指定のログレベルを表します。
		///  通常は利用されません。
		/// </summary>
		None,

		/// <summary>
		///  追跡(トレース)レベルのログを表します。
		/// </summary>
		Trace,

		/// <summary>
		///  デバッグレベルのログを表します。
		/// </summary>
		Debug,

		/// <summary>
		///  情報レベルのログを表します。
		/// </summary>
		Info,

		/// <summary>
		///  警告レベルのログを表します。
		/// </summary>
		Warn,

		/// <summary>
		///  失敗(エラー)レベルのログを表します。
		/// </summary>
		Error,

		/// <summary>
		///  致命的な失敗(エラー)レベルのログを表します。
		/// </summary>
		Fatal
	}
}
