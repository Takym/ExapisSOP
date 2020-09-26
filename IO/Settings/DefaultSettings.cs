/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System.Globalization;
using System.Xml.Serialization;

namespace ExapisSOP.IO.Settings
{
	/// <summary>
	///  既定の環境設定を表します。
	/// </summary>
	[XmlRoot(RootElementName)]
	[XmlInclude(typeof(OptimizedSettings))]
	public class DefaultSettings : EnvironmentSettings
	{
		/// <summary>
		///  型'<see cref="ExapisSOP.IO.Settings.DefaultSettings"/>'の新しいインスタンスを生成します。
		/// </summary>
		public DefaultSettings() : base() { }

		/// <summary>
		///  設定を既定値へ戻します。
		/// </summary>
		public override void Reset()
		{
			this.OutputReadableXML = true;
			this.Locale            = CultureInfo.InstalledUICulture.Name;
			this.EnableLogging     = true;
			this.DataStore         = new DataStore();
		}
	}
}
