/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace ExapisSOP.IO.Settings
{
	/// <summary>
	///  設定情報を保存する為のデータストアを表します。
	/// </summary>
	public class DataStore : IXmlSerializable
	{
		public void ReadXml(XmlReader reader)
		{
			throw new System.NotImplementedException();
		}

		public void WriteXml(XmlWriter writer)
		{
			throw new System.NotImplementedException();
		}

		/// <summary>
		///  XMLスキーマ情報を取得します。
		/// </summary>
		/// <returns></returns>
		public XmlSchema GetSchema()
		{
			return EnvironmentSettings.LoadSchema();
		}
	}
}
