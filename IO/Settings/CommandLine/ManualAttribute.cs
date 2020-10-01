/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System;
using System.Globalization;

namespace ExapisSOP.IO.Settings.CommandLine
{
	/// <summary>
	///  スイッチまたはオプションの説明を表します。
	/// </summary>
	[AttributeUsage(
		AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface |
		AttributeTargets.Field | AttributeTargets.Property,
		AllowMultiple = true, Inherited = true
	)]
	public sealed class ManualAttribute : Attribute
	{
		/// <summary>
		///  コンストラクタに渡された言語コードを取得します。
		/// </summary>
		public string Locale { get; }

		/// <summary>
		///  説明の言語を取得します。
		/// </summary>
		public CultureInfo CultureInfo { get; }

		/// <summary>
		///  説明を表す文字列を取得します。
		/// </summary>
		public string Description { get; }

		/// <summary>
		///  型'<see cref="ExapisSOP.IO.Settings.CommandLine.ManualAttribute"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="locale">説明の言語を表す言語コードです。</param>
		/// <param name="description">説明を表す文字列です。</param>
		/// <exception cref="System.ArgumentNullException" />
		/// <exception cref="System.Globalization.CultureNotFoundException" />
		public ManualAttribute(string locale, string description)
		{
			this.Locale      = locale;
			this.CultureInfo = CultureInfo.GetCultureInfo(locale);
			this.Description = description;
		}
	}
}
