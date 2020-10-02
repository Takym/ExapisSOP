/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System;
using System.Globalization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using ExapisSOP.IO.Settings.CommandLine;

namespace ExapisSOP.IO.Settings
{
	/// <summary>
	///  システムに関する環境設定を表します。
	/// </summary>
	[XmlRoot(RootElementName)]
	[XmlInclude(typeof(CustomSettings))]
	[XmlInclude(typeof(DefaultSettings))]
	[XmlInclude(typeof(OptimizedSettings))]
	[Switch("Settings", "S")]
	[Manual("en", "Overrides the environment settings.")]
	[Manual("ja", "環境設定を上書きします。")]
	public class EnvironmentSettings
	{
		/// <summary>
		///  可読なXMLファイルを出力する場合は<see langword="true"/>、しない場合は<see langword="false"/>を設定します。
		/// </summary>
		[XmlElement("saveReadable")]
		[Option("output-readable-xml", "x")]
		[Manual("en", "Indicates to save the environment settings as a readable XML format.")]
		[Manual("ja", "環境設定を可読なXMLファイルとして出力する様に指示します。")]
		public bool OutputReadableXML { get; set; }

		/// <summary>
		///  現在の言語コードを取得または設定します。
		/// </summary>
		[XmlElement("lang")]
		[Option("lang", "l")]
		[Manual("en", "Sets a display language.")]
		[Manual("ja", "表示言語を設定します。")]
		public string? Locale { get; set; }

		/// <summary>
		///  ログ出力を有効にする場合は<see langword="true"/>、しない場合は<see langword="false"/>を設定します。
		/// </summary>
		[XmlElement("enableLog")]
		[Option("logging", "g")]
		[Manual("en", "Enable a log output. Specify \"-logging:disable\" or \"-g:disable\" to disable.")]
		[Manual("ja", "ログ出力を有効にします。「-logging:disable」または「-g:disable」で無効にします。")]
		public bool EnableLogging { get; set; }

		/// <summary>
		///  その他のユーザー定義の設定情報を取得または設定します。
		/// </summary>
		[XmlElement(DataStore.RootElementName, IsNullable = true)]
		public DataStore? DataStore { get; set; }

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
			this.OutputReadableXML = default;
			this.Locale            = default;
			this.EnableLogging     = default;
			this.DataStore         = default;
		}

		/// <summary>
		///  指定された設定情報から現在インスタンスへ設定情報をコピーします。
		/// </summary>
		/// <param name="settings">コピー元の設定情報です。</param>
		/// <param name="copyFirstBoot"><see cref="ExapisSOP.IO.Settings.EnvironmentSettings.FirstBoot"/>の値をコピーするかどうかを表す論理値です。</param>
		/// <exception cref="System.ArgumentNullException" />
		public virtual void CopyFrom(EnvironmentSettings settings, bool copyFirstBoot = true)
		{
			if (settings == null) {
				throw new ArgumentNullException(nameof(settings));
			}
			this.OutputReadableXML = settings.OutputReadableXML;
			this.Locale            = settings.Locale;
			this.EnableLogging     = settings.EnableLogging;
			this.DataStore         = settings.DataStore?.Clone();
			if (copyFirstBoot) {
				this.FirstBoot = settings.FirstBoot;
			}
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

		#region 静的
		internal const  string     RootElementName = "envconfig";
		private  static XmlSchema? _schema;
		internal static XmlSchema LoadSchema()
		{
			if (_schema == null) {
				using (var s = VersionInfo._asm.GetManifestResourceStream(
					$"{nameof(ExapisSOP)}.{nameof(IO)}.{nameof(Settings)}.{nameof(EnvironmentSettings)}.xsd"
				)) {
					_schema = XmlSchema.Read(s, (e, sender) => { /* do nothing */ });
				}
			}
			return _schema;
		}
		#endregion
	}
}
