/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System;
using System.Threading.Tasks;
using ExapisSOP.IO;
using ExapisSOP.IO.Settings;

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
		///  <paramref name="config"/>そのもの、または、
		///  指定された<see cref="ExapisSOP.AppWorker"/>がサービスリストに追加された新しい<paramref name="config"/>のコピーです。
		/// </returns>
		/// <exception cref="System.MissingMemberException" />
		/// <exception cref="System.ArgumentNullException" />
		public static IConfiguration AddAppWorker<TAppWorker>(this IConfiguration config) where TAppWorker : AppWorker
		{
			if (config == null) {
				throw new ArgumentNullException(nameof(config));
			}
			try {
				return config.AddService(Activator.CreateInstance<TAppWorker>());
			} catch (MissingMethodException mme) {
				throw new MissingMemberException(mme.Message, mme);
			}
		}

		/// <summary>
		///  <see cref="ExapisSOP.IO.IFileSystemService"/>をサービスとして実行環境に登録します。
		/// </summary>
		/// <param name="config">登録先の構成設定です。</param>
		/// <returns>
		///  <paramref name="config"/>そのもの、または、
		///  <see cref="ExapisSOP.IO.IFileSystemService"/>がサービスリストに追加された新しい<paramref name="config"/>のコピーです。
		/// </returns>
		/// <exception cref="System.ArgumentNullException" />
		public static IConfiguration AddFileSystem(this IConfiguration config)
		{
			if (config == null) {
				throw new ArgumentNullException(nameof(config));
			}
			return config.AddService(new FileSystemService(_ => Task.CompletedTask));
		}

		/// <summary>
		///  <see cref="ExapisSOP.IO.IFileSystemService"/>をサービスとして実行環境に登録します。
		/// </summary>
		/// <param name="config">登録先の構成設定です。</param>
		/// <param name="callBackFunc">サービスの設定を行います。</param>
		/// <returns>
		///  <paramref name="config"/>そのもの、または、
		///  <see cref="ExapisSOP.IO.IFileSystemService"/>がサービスリストに追加された新しい<paramref name="config"/>のコピーです。
		/// </returns>
		/// <exception cref="System.ArgumentNullException" />
		public static IConfiguration AddFileSystem(this IConfiguration config, Func<FileSystemServiceOptions, Task> callBackFunc)
		{
			if (config == null) {
				throw new ArgumentNullException(nameof(config));
			}
			if (callBackFunc == null) {
				throw new ArgumentNullException(nameof(callBackFunc));
			}
			return config.AddService(new FileSystemService(callBackFunc));
		}

		/// <summary>
		///  <see cref="ExapisSOP.IO.Settings.ISettingsSystemService"/>をサービスとして実行環境に登録します。
		///  <see cref="ExapisSOP.IO.IFileSystemService"/>より後に登録しなければなりません。
		/// </summary>
		/// <param name="config">登録先の構成設定です。</param>
		/// <returns>
		///  <paramref name="config"/>そのもの、または、
		///  <see cref="ExapisSOP.IO.Settings.ISettingsSystemService"/>がサービスリストに追加された新しい<paramref name="config"/>のコピーです。
		/// </returns>
		/// <exception cref="System.ArgumentNullException" />
		public static IConfiguration AddSettingsSystem(this IConfiguration config)
		{
			if (config == null) {
				throw new ArgumentNullException(nameof(config));
			}
			return config.AddService(new SettingsSystemService(_ => Task.CompletedTask));
		}

		/// <summary>
		///  <see cref="ExapisSOP.IO.Settings.ISettingsSystemService"/>をサービスとして実行環境に登録します。
		///  <see cref="ExapisSOP.IO.IFileSystemService"/>より後に登録しなければなりません。
		/// </summary>
		/// <param name="config">登録先の構成設定です。</param>
		/// <param name="callBackFunc">サービスの設定を行います。</param>
		/// <returns>
		///  <paramref name="config"/>そのもの、または、
		///  <see cref="ExapisSOP.IO.Settings.ISettingsSystemService"/>がサービスリストに追加された新しい<paramref name="config"/>のコピーです。
		/// </returns>
		/// <exception cref="System.ArgumentNullException" />
		public static IConfiguration AddSettingsSystem(this IConfiguration config, Func<SettingsSystemServiceOptions, Task> callBackFunc)
		{
			if (config == null) {
				throw new ArgumentNullException(nameof(config));
			}
			if (callBackFunc == null) {
				throw new ArgumentNullException(nameof(callBackFunc));
			}
			return config.AddService(new SettingsSystemService(callBackFunc));
		}
	}
}
