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

namespace ExapisSOP.IO.Settings
{
	/// <summary>
	///  システムに関する環境設定を表します。
	/// </summary>
	[XmlRoot(RootElementName)]
	[XmlSchemaProvider(nameof(GetSchema))]
	[XmlInclude(typeof(CustomSettings))]
	[XmlInclude(typeof(DefaultSettings))]
	[XmlInclude(typeof(OptimizedSettings))]
	public class EnvironmentSettings
	{
		/// <summary>
		///  可読なXMLファイルを出力する場合は<see langword="true"/>、しない場合は<see langword="false"/>を設定します。
		/// </summary>
		[XmlElement("saveReadable")]
		public bool OutputReadableXML { get; set; }

		/// <summary>
		///  現在の言語コードを取得または設定します。
		/// </summary>
		[XmlElement("lang")]
		public string? Locale { get; set; }

		/// <summary>
		///  ログ出力を有効にする場合は<see langword="true"/>、しない場合は<see langword="false"/>を設定します。
		/// </summary>
		[XmlElement("enableLog")]
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
			this.OutputReadableXML = default;
			this.Locale            = default;
			this.EnableLogging     = default;
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

		internal const string RootElementName = "envconfig";

		private static XmlSchema? _schema;

		internal static XmlQualifiedName GetSchema(XmlSchemaSet xss)
		{
			xss.XmlResolver = new XmlUrlResolver();
			xss.Add(LoadSchema());
			return new XmlQualifiedName(RootElementName, _schema!.TargetNamespace);
		}

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
