/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 *
 * distributed under the MIT License.
****/

namespace ExapisSOP.IO.Logging
{
	/// <summary>
	///  ログ情報として読み込みと書き込みを行える事を示します。
	/// </summary>
	/// <remarks>
	///  このインターフェースを実装する場合は、直列化可能にしてください。
	/// </remarks>
	public interface ILoggable
	{
		/// <summary>
		///  バイナリデータへ変換します。
		/// </summary>
		/// <returns>現在のオブジェクトが保持するデータと等価なバイト配列です。</returns>
		byte[] ToBinary();

		/// <summary>
		///  バイナリデータからデータを読み込みます。
		/// </summary>
		/// <param name="buffer">読み込むバイト配列です。</param>
		void FromBinary(byte[] buffer);
	}
}
