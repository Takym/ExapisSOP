/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System;
using System.Text;

namespace ExapisSOP.IO.Logging
{
	/// <summary>
	///  ログ情報に長文のメッセージを付加します。
	/// </summary>
	[Serializable()]
	public class LongMessageRecord : ILoggable
	{
		/// <summary>
		///  追加メッセージを取得または設定します。
		/// </summary>
		public string? Message { get; set; }

		/// <summary>
		///  型'<see cref="ExapisSOP.IO.Logging.LongMessageRecord"/>'の新しいインスタンスを生成します。
		/// </summary>
		public LongMessageRecord() { }

		/// <summary>
		///  型'<see cref="ExapisSOP.IO.Logging.LongMessageRecord"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="longMsg">新しいインスタンスに設定する追加メッセージです。</param>
		public LongMessageRecord(string longMsg)
		{
			this.Message = longMsg;
		}

		/// <summary>
		///  例外オブジェクトをバイナリデータへ変換します。
		/// </summary>
		/// <returns>現在のオブジェクトが保持する例外オブジェクトと等価なバイト配列です。</returns>
		public byte[] ToBinary()
		{
			return Encoding.UTF8.GetBytes(this.Message ?? string.Empty);
		}

		/// <summary>
		///  バイナリデータから例外オブジェクトを読み込みます。
		/// </summary>
		/// <param name="buffer">読み込むバイト配列です。</param>
		public void FromBinary(byte[] buffer)
		{
			this.Message = Encoding.UTF8.GetString(buffer);
		}
	}
}
