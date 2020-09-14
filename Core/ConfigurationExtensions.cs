/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System;

namespace ExapisSOP.Core
{
	/// <summary>
	///  <see cref="ExapisSOP.IConfiguration"/>の機能を拡張します。
	///  このクラスは静的です。
	/// </summary>
	public static class ConfigurationExtensions
	{
		/// <summary>
		///  <see cref="ExapisSOP.AppWorker"/>をサービスとして実行環境に登録します。
		/// </summary>
		/// <typeparam name="TAppWorker">
		///  引数を必要としないコンストラクタを持つ具体的な<see cref="ExapisSOP.AppWorker"/>の種類です。
		/// </typeparam>
		/// <param name="config">登録先の構成設定です。</param>
		/// <returns>
		///  <paramref name="config"/>、または、指定された<see cref="ExapisSOP.AppWorker"/>がサービスリストに追加された新しい<paramref name="config"/>のコピーです。
		/// </returns>
		/// <exception cref="System.MissingMemberException" />
		public static IConfiguration AddAppWorker<TAppWorker>(this IConfiguration config) where TAppWorker : AppWorker
		{
			try {
				return config.AddService(Activator.CreateInstance<TAppWorker>());
			} catch (MissingMethodException mme) {
				throw new MissingMemberException(mme.Message, mme);
			}
		}
	}
}
