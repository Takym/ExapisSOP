/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System;

namespace ExapisSOP.IO.Settings
{
	/// <summary>
	///  <see cref="ExapisSOP.IO.Settings.ISettingsSystemService"/>の動作方法を指定します。
	/// </summary>
	public class SettingsSystemServiceOptions
	{
		/// <summary>
		///  互換性があるかどうか判定する関数を取得または設定します。
		/// </summary>
		public ConfirmVersionInfo HasCompatibleWith { get; set; }

		/// <summary>
		///  現在のバージョン情報を取得する関数を取得または設定します。
		/// </summary>
		public GetVersionInfo GetCurrentVersion { get; set; }

		/// <summary>
		///  新しい設定情報を作成する関数を取得または設定します。
		/// </summary>
		public Func<EnvironmentSettings> CreateNewSettings { get; set; }

		/// <summary>
		///  型'<see cref="ExapisSOP.IO.Settings.SettingsSystemServiceOptions"/>'の新しいインスタンスを生成します。
		/// </summary>
		public SettingsSystemServiceOptions()
		{
			this.HasCompatibleWith = (v, cn) => true;
			this.GetCurrentVersion = ()      => ("?.?.?.?", "no app ver");
			this.CreateNewSettings = ()      => new DefaultSettings();
		}

		/// <summary>
		///  バージョン情報に互換性があるかどうか判定します。
		/// </summary>
		/// <param name="version">バージョン情報を表す文字列です。</param>
		/// <param name="codename">開発コード名を表す文字列です。</param>
		/// <returns>互換性がある場合は<see langword="true"/>、それ以外の場合は<see langword="false"/>を返します。</returns>
		public delegate bool ConfirmVersionInfo(string? version, string? codename);

		/// <summary>
		///  バージョン情報を取得します。
		/// </summary>
		/// <returns>1番目の要素にバージョン情報を表す文字列、2番目の要素に開発コード名を返します。</returns>
		public delegate (string version, string codename) GetVersionInfo();
	}
}
