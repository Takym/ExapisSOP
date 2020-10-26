/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using ExapisSOP.Utils;

namespace ExapisSOP.IO.Logging
{
	/// <summary>
	///  ログ情報を表します。
	///  このクラスは継承できません。
	/// </summary>
	[Serializable()]
	public sealed class LogData : ISerializable
	{
		/// <summary>
		///  このログ情報を作成した日時を取得します。
		/// </summary>
		public DateTime Created { get; }

		/// <summary>
		///  このログ情報のログレベルを取得します。
		/// </summary>
		public LogLevel Level { get; }

		/// <summary>
		///  このログ情報を作成したロガーオブジェクトを取得します。
		///  別のプロセスで作成された場合は利用できません。
		/// </summary>
		public ILogger? Logger { get; }

		/// <summary>
		///  このログ情報を作成したロガーの名前を取得します。
		/// </summary>
		public string LoggerName { get; }

		/// <summary>
		///  このログ情報が保持しているメッセージを取得します。
		/// </summary>
		public string Message { get; }

		/// <summary>
		///  このログ情報が保持している追加データを取得します。
		/// </summary>
		public ILoggable? Data { get; }

		/// <summary>
		///  型'<see cref="ExapisSOP.IO.Logging.LogData"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="level">新しいログ情報のレベルです。</param>
		/// <param name="logger">新しいログ情報を作成するロガーです。</param>
		/// <param name="msg">新しいログ情報が保持すべきメッセージです。</param>
		/// <param name="data">新しいログ情報が保持すべき追加データです。</param>
		/// <exception cref="System.ArgumentNullException"/>
		public LogData(LogLevel level, ILogger logger, string msg, ILoggable? data = null)
			: this(DateTime.Now, level, logger, logger?.Name!, msg ?? string.Empty, data)
		{
			if (logger == null) {
				throw new ArgumentNullException(nameof(logger));
			}
		}

		private LogData(DateTime created, LogLevel level, ILogger? logger, string loggerName, string msg, ILoggable? data)
		{
			this.Created    = created;
			this.Level      = level;
			this.Logger     = logger;
			this.LoggerName = loggerName;
			this.Message    = msg;
			this.Data       = data;
		}

		private LogData(SerializationInfo info, StreamingContext context)
		{
			this.Created    = info.GetDateTime        (nameof(this.Created));
			this.Level      = info.GetValue<LogLevel> (nameof(this.Level));
			this.LoggerName = info.GetString          (nameof(this.LoggerName)) ?? string.Empty;
			this.Message    = info.GetString          (nameof(this.Message))    ?? string.Empty;
			this.Data       = info.GetValue<ILoggable>(nameof(this.Data));
		}

		internal LogData(XmlReader reader)
		{
			reader.ReadStartElement("log");
			reader.ReadStartElement("created");
			this.Created = reader.ReadContentAsDateTime();
			reader.ReadEndElement();
			reader.ReadStartElement("level");
			Enum.TryParse<LogLevel>(reader.ReadContentAsString(), out var level);
			this.Level = level;
			reader.ReadEndElement();
			reader.ReadStartElement("logger");
			this.LoggerName = reader.ReadContentAsString();
			reader.ReadEndElement();
			reader.ReadStartElement("message");
			this.Message = reader.ReadContentAsString();
			reader.ReadEndElement();
			if (reader.Name == "data") {
				reader.MoveToContent();
				int.TryParse(reader.GetAttribute("size"), out int size);
				string type = reader.GetAttribute("type");
				reader.ReadStartElement();
				byte[] buf = new byte[size];
				if (type == "!base64_serialized") {
					reader.ReadContentAsBase64(buf, 0, buf.Length);
					using (var ms = new MemoryStream(buf)) {
						this.Data = VersionInfo._bf.Deserialize(ms) as ILoggable;
					}
				} else {
					reader.ReadContentAsBinHex(buf, 0, buf.Length);
					var t = Type.GetType(type);
					if (t != null) {
						this.Data = Activator.CreateInstance(t) as ILoggable;
						this.Data?.FromBinary(buf);
					}
				}
				reader.ReadEndElement();
			}
			reader.ReadEndElement();
		}

		internal LogData(BinaryReader reader)
		{
			this.Created    = new DateTime(reader.ReadInt64());
			this.Level      = ((LogLevel)(reader.ReadByte()));
			this.LoggerName = reader.ReadString();
			this.Message    = reader.ReadString();
			byte x = reader.ReadByte();
			if (x != 0) {
				var t = Type.GetType(reader.ReadString());
				if (t != null) {
					this.Data = Activator.CreateInstance(t) as ILoggable;
					this.Data?.FromBinary(reader.ReadBytes(reader.ReadInt32()));
				}
			}
		}

		/// <summary>
		///  現在のログ情報を直列化します。
		/// </summary>
		/// <param name="info">直列化されたデータを含むオブジェクトです。</param>
		/// <param name="context">ストリームの転送先または転送元に関する文脈情報です。</param>
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue(nameof(this.Created),    this.Created);
			info.AddValue(nameof(this.Level),      this.Level);
			info.AddValue(nameof(this.LoggerName), this.LoggerName);
			info.AddValue(nameof(this.Message),    this.Message);
			info.AddValue(nameof(this.Data),       this.Data);
		}

		/// <summary>
		///  現在のログ情報をXML形式で直列化します。
		/// </summary>
		/// <param name="writer">ログ情報の書き込み先のXMLライターです。</param>
		public void GetObjectXML(XmlWriter writer)
		{
			writer.WriteStartElement("log");
			writer.WriteStartElement("created");
			writer.WriteValue(this.Created);
			writer.WriteEndElement();
			writer.WriteStartElement("level");
			writer.WriteString(this.Level.ToString());
			writer.WriteEndElement();
			writer.WriteStartElement("logger");
			writer.WriteString(this.LoggerName);
			writer.WriteEndElement();
			writer.WriteStartElement("message");
			writer.WriteString(this.Message);
			writer.WriteEndElement();
			if (this.Data != null) {
				writer.WriteStartElement("data");
				byte[] buf;
				if (this.Data.GetType().IsSerializable) {
					using (var ms = new MemoryStream()) {
						VersionInfo._bf.Serialize(ms, this.Data);
						buf = ms.ToArray();
					}
					writer.WriteAttributeString("size", buf.Length.ToString());
					writer.WriteAttributeString("type", "!base64_serialized");
					writer.WriteBase64(buf, 0, buf.Length);
				} else {
					buf = this.Data.ToBinary();
					writer.WriteAttributeString("size", buf.Length.ToString());
					writer.WriteAttributeString("type", this.Data.GetType().AssemblyQualifiedName);
					writer.WriteBinHex(buf, 0, buf.Length);
				}
				writer.WriteEndElement();
			}
			writer.WriteEndElement();
		}

		/// <summary>
		///  現在のログ情報をバイナリ形式で直列化します。
		/// </summary>
		/// <param name="writer">ログ情報の書き込み先のバイナリライターです。</param>
		public void GetObjectBinary(BinaryWriter writer)
		{
			writer.Write(this.Created.Ticks);
			writer.Write((byte)(this.Level));
			writer.Write(this.LoggerName);
			writer.Write(this.Message);
			if (this.Data == null) {
				writer.Write((byte)(0x00));
			} else {
				var data = this.Data.ToBinary() ?? Array.Empty<byte>();
				writer.Write((byte)(0xFF));
				writer.Write(this.Data.GetType().AssemblyQualifiedName!);
				writer.Write(data.Length);
				writer.Write(data);
			}
		}

		/// <summary>
		///  指定されたオブジェクトと現在のログ情報が等価かどうか判定します。
		/// </summary>
		/// <param name="obj">判定対象のオブジェクトです。</param>
		/// <returns>等しい場合は<see langword="true"/>、等しくない場合は<see langword="false"/>を返します。</returns>
		public override bool Equals(object? obj)
		{
			if (this == obj) {
				return true;
			} else if (obj is LogData other) {
				return this.Created    == other.Created
					&& this.Level      == other.Level
					&& this.LoggerName == other.LoggerName
					&& this.Message    == other.Message
					&& (this.Data?.Equals(other) ?? this.Data == other.Data);
			} else {
				return false;
			}
		}

		/// <summary>
		///  現在のログ情報のハッシュ値を取得します。
		/// </summary>
		/// <returns>現在のログ情報のハッシュ値を表す32ビット符号付き整数値です。</returns>
		public override int GetHashCode()
		{
			return this.Created.GetHashCode() ^ this.Level.GetHashCode() ^ this.LoggerName.GetHashCode() ^ this.Message.GetHashCode();
		}

		/// <summary>
		///  現在のログ情報を可読な文字列へ変換します。
		/// </summary>
		/// <returns>作成日時、ログレベル、ロガーの簡易名、メッセージを含むログ情報の文字列表現です。</returns>
		public override string ToString()
		{
			return $"{this.Created:yyyy/MM/dd HH:mm:ss.fffffff} [{this.Level.ToString().ToUpper(),-5}] {this.LoggerName.Abridge(24)}\t\t{this.Message}"; ;
		}
	}
}
