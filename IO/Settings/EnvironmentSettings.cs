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
	///  システムに関する環境設定を表します。
	/// </summary>
	[XmlRoot(nameof(EnvironmentSettings))]
	[XmlInclude(typeof(DefaultSettings))]
	public class EnvironmentSettings
	{
		/// <summary>
		///  現在の言語コードを取得または設定します。
		/// </summary>
		[XmlElement(nameof(Locale))]
		public string? Locale { get; set; }

		/// <summary>
		///  ログ出力を有効にする場合は<see langword="true"/>、しない場合は<see langword="false"/>を設定します。
		/// </summary>
		[XmlElement(nameof(EnableLogging))]
		public bool EnableLogging { get; set; }

		/// <summary>
		///  プログラムが初回起動の場合は<see langword="true"/>、それ以外の場合は<see langword="false"/>を返します。
		/// </summary>
		[XmlIgnore()]
		public bool FirstBoot { get; internal set; }

		/// <summary>
		///  型'<see cref="ExapisSOP.IO.Settings.EnvironmentSettings"/>'の新しいインスタンスを生成します。
		/// </summary>
		public EnvironmentSettings()
		{
			this.Reset();
		}

		/// <summary>
		///  設定を既定値へ戻します。
		/// </summary>
		public virtual void Reset()
		{
			this.Locale        = default;
			this.EnableLogging = default;
		}

		/// <summary>
		///  現在のインスタンスに設定されているカルチャ情報を取得します。
		/// </summary>
		/// <returns>カルチャ情報を表すオブジェクトです。</returns>
		/// <exception cref="System.Globalization.CultureNotFoundException" />
		public CultureInfo GetCulture()
		{
			return CultureInfo.GetCultureInfo(this.Locale ?? CultureInfo.InstalledUICulture.Name);
		}

		/// <summary>
		///  現在のインスタンスにカルチャ情報を設定します。
		///  または、空値を指定してOSの既定値を設定します。
		/// </summary>
		/// <param name="culture">設定するカルチャ情報を表すオブジェクトです。</param>
		public void SetCulture(CultureInfo? culture)
		{
			this.Locale = culture?.Name ?? CultureInfo.InstalledUICulture.Name;
		}
	}
}
