/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

namespace ExapisSOP.IO.Logging
{
	/// <summary>
	///  ログファイルの種類を表します。
	/// </summary>
	public enum LogFileType
	{
		/// <summary>
		///  独自形式のログファイルを指定します。
		/// </summary>
		Custom,

		/// <summary>
		///  ログ出力を行わない事を指定します。
		/// </summary>
		Empty,

		/// <summary>
		///  直列化形式のログファイルを指定します。
		/// </summary>
		Serialized,

		/// <summary>
		///  テキスト形式のログファイルを指定します。
		/// </summary>
		Text,

		/// <summary>
		///  XML形式のログファイルを指定します。
		/// </summary>
		Xml,
	}
}
