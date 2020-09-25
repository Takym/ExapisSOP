/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System.Globalization;

namespace ExapisSOP.IO.Settings
{
	/// <summary>
	///  最適化された環境設定を表します。
	/// </summary>
	public class OptimizedSettings : DefaultSettings
	{
		/// <summary>
		///  型'<see cref="ExapisSOP.IO.Settings.OptimizedSettings"/>'の新しいインスタンスを生成します。
		/// </summary>
		public OptimizedSettings() : base() { }

		/// <summary>
		///  設定を既定値へ戻します。
		/// </summary>
		public override void Reset()
		{
			this.OutputReadableXML = false;
			this.Locale            = CultureInfo.InstalledUICulture.Name;
			this.EnableLogging     = false;
		}
	}
}
