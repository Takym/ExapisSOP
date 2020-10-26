/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 *
 * distributed under the MIT License.
****/

using System;

namespace ExapisSOP.Utils
{
	/// <summary>
	///  <see cref="ExapisSOP.IConfiguration"/>の機能を拡張します。
	///  このクラスは静的です。
	/// </summary>
	public static class UtilityConfigurationExtensions
	{
		/// <summary>
		///  <see cref="ExapisSOP.Utils.IUtilityService"/>をサービスとして実行環境に登録します。
		/// </summary>
		/// <param name="config">登録先の構成設定です。</param>
		/// <returns>
		///  <paramref name="config"/>そのもの、または、
		///  <see cref="ExapisSOP.Utils.IUtilityService"/>がサービスリストに追加された新しい<paramref name="config"/>のコピーです。
		/// </returns>
		/// <exception cref="System.ArgumentNullException" />
		public static IConfiguration AddUtility(this IConfiguration config)
		{
			if (config == null) {
				throw new ArgumentNullException(nameof(config));
			}
			return config.AddService(new UtilityService());
		}
	}
}
