/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace ExapisSOP.IO.Settings
{
	/// <summary>
	///  設定情報を保存する為のデータストアを表します。
	/// </summary>
	[XmlRoot(RootElementName)]
	public class DataStore : ICloneable, IXmlSerializable
	{
		internal const string RootElementName = "datalist";

		/// <summary>
		///  保持されている値を取得または設定します。
		/// </summary>
		/// <remarks>
		///  <see cref="ExapisSOP.IO.Settings.DataStore.Dictionary"/>を利用する事で高度な操作を行う事ができます。
		/// </remarks>
		/// <param name="key">値に関連付けられている名前(キー名)です。</param>
		/// <returns>値を表すオブジェクト、または、存在しない場合は<see langword="null"/>を返します。</returns>
		/// <value><see langword="null"/>を設定した場合、値は削除されます。</value>
		public object? this[string key]
		{
			get
			{
				if (this.Dictionary.ContainsKey(key)) {
					return this.Dictionary[key];
				} else {
					return null;
				}
			}
			set
			{
				if (value == null) {
					this.Dictionary.Remove(key);
				} else {
					if (this.Dictionary.ContainsKey(key)) {
						this.Dictionary[key] = value;
					} else {
						this.Dictionary.Add(key, value);
					}
				}
			}
		}

		/// <summary>
		///  保持されている値を取得または設定します。
		/// </summary>
		/// <remarks>
		///  <see cref="ExapisSOP.IO.Settings.DataStore.Dictionary"/>を利用する事で高度な操作を行う事ができます。
		/// </remarks>
		/// <param name="index">値に関連付けられている名前(番号/インデックス)です。</param>
		/// <returns>値を表すオブジェクト、または、存在しない場合は<see langword="null"/>を返します。</returns>
		/// <value><see langword="null"/>を設定した場合、値は削除されます。</value>
		public object? this[int index]
		{
			get => this[index.ToString()];
			set => this[index.ToString()] = value;
		}

		/// <summary>
		///  設定情報を保持する辞書を取得します。
		/// </summary>
		public Dictionary<string, object> Dictionary { get; }

		/// <summary>
		///  型'<see cref="ExapisSOP.IO.Settings.DataStore"/>'の新しいインスタンスを生成します。
		/// </summary>
		public DataStore()
		{
			this.Dictionary = new Dictionary<string, object>();
		}

		/// <summary>
		///  型'<see cref="ExapisSOP.IO.Settings.DataStore"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="enumerable">設定情報のコピー元の列挙体です。</param>
		public DataStore(IEnumerable<KeyValuePair<string, object>> enumerable)
		{
#if NETCOREAPP3_1
			this.Dictionary = new Dictionary<string, object>(enumerable);
#elif NET48
			this.Dictionary = new Dictionary<string, object>();
			foreach (var item in enumerable) {
				this.Dictionary.Add(item.Key, item.Value);
			}
#endif
		}

		/// <summary>
		///  型'<see cref="ExapisSOP.IO.Settings.DataStore"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="dictionary">設定情報のコピー元の辞書オブジェクトです。</param>
		public DataStore(IDictionary<string, object> dictionary)
		{
			this.Dictionary = new Dictionary<string, object>(dictionary);
		}

		/// <summary>
		///  XMLリーダーからデータを読み取ります。
		/// </summary>
		/// <param name="reader">XMLリーダーです。</param>
		public void ReadXml(XmlReader reader)
		{
			this.Dictionary.Clear();
			if (reader.IsEmptyElement) return;
			while (reader.Read()) {
				if (reader.NodeType == XmlNodeType.EndElement) {
					reader.ReadEndElement();
					return;
				}
				reader.MoveToContent();
				string key = reader.GetAttribute("name");
				var    t   = Type.GetType(reader.GetAttribute("type"), false);
				reader.ReadStartElement("data");
				if (t != null) {
					object? obj;
					if (reader.IsEmptyElement) continue;
					/*if (typeof(IXmlSerializable).IsAssignableFrom(t)) {
						var serializable = Activator.CreateInstance(t) as IXmlSerializable;
						serializable?.ReadXml(reader);
						obj = serializable;
					} else*/ if (t.IsPrimitive || t == typeof(string)) {
						//reader.Read();
						obj = reader.ReadContentAsObject();
					} else {
						var xs = new XmlSerializer(t);
						obj = xs.Deserialize(reader);
					}
					if (obj != null) {
						this.Dictionary.Add(key, obj);
					}
				}
				//reader.ReadEndElement();
			}
		}

		/// <summary>
		///  XMLライターへデータを書き込みます。
		/// </summary>
		/// <param name="writer">XMLライターです。</param>
		public void WriteXml(XmlWriter writer)
		{
			foreach (var pair in this.Dictionary) {
				var t = pair.Value.GetType();
				writer.WriteStartElement("data");
				writer.WriteAttributeString("name", pair.Key);
				writer.WriteAttributeString("type", t.FullName);
				/*if (pair.Value is IXmlSerializable serializable) {
					serializable.WriteXml(writer);
				} else*/ if (t.IsPrimitive || t == typeof(string)) {
					writer.WriteValue(pair.Value);
				} else {
					var xs = new XmlSerializer(t);
					xs.Serialize(writer, pair.Value);
				}
				writer.WriteEndElement();
			}
		}

		/// <summary>
		///  XMLスキーマ情報を取得します。
		/// </summary>
		/// <returns>XMLスキーマを表すオブジェクトです。</returns>
		public XmlSchema GetSchema()
		{
			return EnvironmentSettings.LoadSchema();
		}

		/// <summary>
		///  現在のオブジェクトのコピーを作成します。
		/// </summary>
		/// <returns>作成したオブジェクトです。</returns>
		public DataStore Clone()
		{
			return new DataStore(this.Dictionary);
		}

		object ICloneable.Clone()
		{
			return this.Clone();
		}
	}
}
