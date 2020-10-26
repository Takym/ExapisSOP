/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace ExapisSOP.IO.Logging
{
	/// <summary>
	///  ログ情報に長文のメッセージを付加します。
	/// </summary>
	[Serializable()]
	public class LongMessageRecord : ILoggable, IXmlSerializable
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
		///  追加メッセージをバイナリデータへ変換します。
		/// </summary>
		/// <returns>現在のオブジェクトが保持する追加メッセージと等価なバイト配列です。</returns>
		public byte[] ToBinary()
		{
			return Encoding.UTF8.GetBytes(this.Message ?? string.Empty);
		}

		/// <summary>
		///  バイナリデータから追加メッセージを読み込みます。
		/// </summary>
		/// <param name="buffer">読み込むバイト配列です。</param>
		public void FromBinary(byte[] buffer)
		{
			this.Message = Encoding.UTF8.GetString(buffer);
		}

		/// <summary>
		///  追加メッセージを指定されたXMLリーダーから読み取ります。
		/// </summary>
		/// <param name="reader">XMLリーダーの読み取り元です。</param>
		public void ReadXml(XmlReader reader)
		{
			reader.ReadStartElement("longmsg");
			this.Message = reader.ReadContentAsString();
			reader.ReadEndElement();
		}

		/// <summary>
		///  追加メッセージをXML表現へ変換します。
		/// </summary>
		/// <param name="writer">XMLデータの書き込み先です。</param>
		public void WriteXml(XmlWriter writer)
		{
			writer.WriteStartElement("longmsg");
			writer.WriteCData(this.Message);
			writer.WriteEndElement();
		}

		/// <summary>
		///  実装されていません。
		/// </summary>
		/// <returns>実装されていません。</returns>
		/// <exception cref="System.NotImplementedException" />
		[Obsolete("実装されていません。", true)]
		public XmlSchema GetSchema()
		{
			throw new NotImplementedException();
		}
	}
}
