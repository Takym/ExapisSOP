/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System.Xml.Serialization;

namespace ExapisSOP.IO.Settings
{
	/// <summary>
	///  アプリケーション定義の環境設定を表します。
	/// </summary>
	[XmlRoot(RootElementName)]
	public class CustomSettings : EnvironmentSettings
	{
		/// <summary>
		///  既定の設定を取得または設定します。
		/// </summary>
		[XmlElement("default", IsNullable = true)]
		public EnvironmentSettings? Default { get; set; }

		/// <summary>
		///  型'<see cref="ExapisSOP.IO.Settings.CustomSettings"/>'の新しいインスタンスを生成します。
		/// </summary>
		public CustomSettings() : base() { }

		/// <summary>
		///  型'<see cref="ExapisSOP.IO.Settings.CustomSettings"/>'の新しいインスタンスを生成します。
		/// </summary>
		public CustomSettings(EnvironmentSettings defaultSettings)
		{
			this.Default = defaultSettings;
			this.Reset();
		}

		/// <summary>
		///  設定を既定値へ戻します。
		/// </summary>
		public override void Reset()
		{
			if (this.Default == null) return;
			this.CopyFrom(this.Default, false);
		}
	}
}
