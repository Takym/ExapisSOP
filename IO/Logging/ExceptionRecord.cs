/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.IO;

namespace ExapisSOP.IO.Logging
{
	/// <summary>
	///  例外情報をログ情報として扱います。
	///  このクラスは継承できません。
	/// </summary>
	[Serializable()]
	public sealed class ExceptionRecord : ILoggable
	{
		/// <summary>
		///  例外情報を取得または設定します。
		/// </summary>
		public Exception? Exception { get; set; }

		/// <summary>
		///  型'<see cref="ExapisSOP.IO.Logging.ExceptionRecord"/>'の新しいインスタンスを生成します。
		/// </summary>
		public ExceptionRecord() { }

		/// <summary>
		///  型'<see cref="ExapisSOP.IO.Logging.ExceptionRecord"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="exception">新しいインスタンスに設定する例外情報です。</param>
		public ExceptionRecord(Exception exception)
		{
			this.Exception = exception;
		}

		/// <summary>
		///  例外オブジェクトをバイナリデータへ変換します。
		/// </summary>
		/// <returns>現在のオブジェクトが保持する例外オブジェクトと等価なバイト配列です。</returns>
		public byte[] ToBinary()
		{
			using (var ms = new MemoryStream()) {
				VersionInfo._bf.Serialize(ms, this.Exception);
				return ms.ToArray();
			}
		}

		/// <summary>
		///  バイナリデータから例外オブジェクトを読み込みます。
		/// </summary>
		/// <param name="buffer">読み込むバイト配列です。</param>
		public void FromBinary(byte[] buffer)
		{
			using (var ms = new MemoryStream(buffer)) {
				this.Exception = VersionInfo._bf.Deserialize(ms) as Exception;
			}
		}
	}
}
