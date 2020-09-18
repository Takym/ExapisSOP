/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

// Do not change this source file. (Excluding: making a derived library)
// このファイルの内容は変更しないでください。 (※除：派生ライブラリを作成する場合)

using System;
using System.Reflection;
using System.Runtime.InteropServices;
using InfoVer   = System.Reflection.AssemblyInformationalVersionAttribute;
using Copyright = System.Reflection.AssemblyCopyrightAttribute;
using Authors   = System.Reflection.AssemblyCompanyAttribute;

[assembly: Guid(ExapisSOP.VersionInfo.GuidString)]

namespace ExapisSOP
{
	/// <summary>
	///  <see cref="ExapisSOP"/>のバージョン情報を取得します。
	///  このクラスは静的です。
	/// </summary>
	public static class VersionInfo
	{
		private readonly static Assembly _asm = typeof(VersionInfo).Assembly;

		/// <summary>
		///  バージョン情報を含んだ<see cref="ExapisSOP"/>の題名を取得します。
		/// </summary>
		public static readonly string Caption = $"{Name} [v{VersionString}, cn:{CodeName}]";

		/// <summary>
		///  このライブラリの名前を表す定数です。
		/// </summary>
		public const string Name = nameof(ExapisSOP);

		/// <summary>
		///  このアセンブリの一意識別子の文字列形式を取得します。
		/// </summary>
		// TODO: このライブラリを派生する場合は以下のGUIDを変更しなければなりません。
		// TODO: If you are making a derived library, you have to change the GUID below.
		public const string GuidString = "CADEBD58-D8EC-4F5B-B094-227BE98EF4C8";

		/// <summary>
		///  このアセンブリの一意識別子を取得します。
		/// </summary>
		public static Guid Guid => Guid.Parse(GuidString);

		/// <summary>
		///  現在のバージョンの文字列形式を取得します。
		/// </summary>
		public static string VersionString => Version?.ToString() ?? "?.?.?.?";

		/// <summary>
		///  現在のバージョンを表すオブジェクトを取得します。
		/// </summary>
		public static Version? Version => _asm.GetName().Version;

		/// <summary>
		///  <see cref="ExapisSOP"/>の開発コード名を取得します。
		/// </summary>
		public static string CodeName => _asm.GetCustomAttribute<InfoVer>()?.InformationalVersion ?? "unknown";

		/// <summary>
		///  <see cref="ExapisSOP"/>の著作権表記を取得します。
		/// </summary>
		public static string Copyright => _asm.GetCustomAttribute<Copyright>()?.Copyright!;

		/// <summary>
		///  <see cref="ExapisSOP"/>の開発者を取得します。
		/// </summary>
		public static string Developers => _asm.GetCustomAttribute<Authors>()?.Company!;
	}
}
