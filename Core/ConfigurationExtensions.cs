/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System;
using System.Threading.Tasks;
using ExapisSOP.IO;
using ExapisSOP.IO.Logging;
using ExapisSOP.IO.Settings;
using ExapisSOP.IO.Settings.CommandLine;

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
			return config.AddService(new FileSystemService(DefaultOptions));
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
			return config.AddService(new SettingsSystemService(DefaultOptions));
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

		/// <summary>
		///  <see cref="ExapisSOP.IO.Settings.CommandLine.ICommandLineService"/>をサービスとして実行環境に登録します。
		///  <see cref="ExapisSOP.IO.Settings.ISettingsSystemService"/>より後に登録しなければなりません。
		/// </summary>
		/// <param name="config">登録先の構成設定です。</param>
		/// <returns>
		///  <paramref name="config"/>そのもの、または、
		///  <see cref="ExapisSOP.IO.Settings.CommandLine.ICommandLineService"/>がサービスリストに追加された新しい<paramref name="config"/>のコピーです。
		/// </returns>
		/// <exception cref="System.ArgumentNullException" />
		public static IConfiguration AddCommandLine(this IConfiguration config)
		{
			if (config == null) {
				throw new ArgumentNullException(nameof(config));
			}
			return config.AddService(new CommandLineService(DefaultOptions));
		}

		/// <summary>
		///  <see cref="ExapisSOP.IO.Settings.CommandLine.ICommandLineService"/>をサービスとして実行環境に登録します。
		///  <see cref="ExapisSOP.IO.Settings.ISettingsSystemService"/>より後に登録しなければなりません。
		/// </summary>
		/// <param name="config">登録先の構成設定です。</param>
		/// <param name="callBackFunc">サービスの設定を行います。</param>
		/// <returns>
		///  <paramref name="config"/>そのもの、または、
		///  <see cref="ExapisSOP.IO.Settings.CommandLine.ICommandLineService"/>がサービスリストに追加された新しい<paramref name="config"/>のコピーです。
		/// </returns>
		/// <exception cref="System.ArgumentNullException" />
		public static IConfiguration AddCommandLine(this IConfiguration config, Func<CommandLineServiceOptions, Task> callBackFunc)
		{
			if (config == null) {
				throw new ArgumentNullException(nameof(config));
			}
			if (callBackFunc == null) {
				throw new ArgumentNullException(nameof(callBackFunc));
			}
			return config.AddService(new CommandLineService(callBackFunc));
		}

		/// <summary>
		///  <see cref="ExapisSOP.IO.Logging.ILoggingSystemService"/>をサービスとして実行環境に登録します。
		///  <see cref="ExapisSOP.IO.Settings.ISettingsSystemService"/>より後に登録しなければなりません。
		///  また、<see cref="ExapisSOP.IO.Settings.CommandLine.ICommandLineService"/>を登録する場合、
		///  このサービスはその後に登録しなければなりません。
		/// </summary>
		/// <param name="config">登録先の構成設定です。</param>
		/// <returns>
		///  <paramref name="config"/>そのもの、または、
		///  <see cref="ExapisSOP.IO.Logging.ILoggingSystemService"/>がサービスリストに追加された新しい<paramref name="config"/>のコピーです。
		/// </returns>
		/// <exception cref="System.ArgumentNullException" />
		public static IConfiguration AddLoggingSystem(this IConfiguration config)
		{
			if (config == null) {
				throw new ArgumentNullException(nameof(config));
			}
			return config.AddService(new LoggingSystemService(DefaultOptions));
		}

		/// <summary>
		///  <see cref="ExapisSOP.IO.Logging.ILoggingSystemService"/>をサービスとして実行環境に登録します。
		///  <see cref="ExapisSOP.IO.Settings.ISettingsSystemService"/>より後に登録しなければなりません。
		///  また、<see cref="ExapisSOP.IO.Settings.CommandLine.ICommandLineService"/>を登録する場合、
		///  このサービスはその後に登録しなければなりません。
		/// </summary>
		/// <param name="config">登録先の構成設定です。</param>
		/// <param name="callBackFunc">サービスの設定を行います。</param>
		/// <returns>
		///  <paramref name="config"/>そのもの、または、
		///  <see cref="ExapisSOP.IO.Logging.ILoggingSystemService"/>がサービスリストに追加された新しい<paramref name="config"/>のコピーです。
		/// </returns>
		/// <exception cref="System.ArgumentNullException" />
		public static IConfiguration AddLoggingSystem(this IConfiguration config, Func<LoggingSystemServiceOptions, Task> callBackFunc)
		{
			if (config == null) {
				throw new ArgumentNullException(nameof(config));
			}
			if (callBackFunc == null) {
				throw new ArgumentNullException(nameof(callBackFunc));
			}
			return config.AddService(new LoggingSystemService(callBackFunc));
		}

		/// <summary>
		///  全てのシステムサービスを既定の設定で実行環境に登録します。
		/// </summary>
		/// <param name="config">登録先の構成設定です。</param>
		/// <returns>
		///  <paramref name="config"/>そのもの、または、
		///  下記のサービスがサービスリストに追加された新しい<paramref name="config"/>のコピーです。
		///  <list type="number">
		///   <item><see cref="ExapisSOP.IO.IFileSystemService"/></item>
		///   <item><see cref="ExapisSOP.IO.Settings.ISettingsSystemService"/></item>
		///   <item><see cref="ExapisSOP.IO.Settings.CommandLine.ICommandLineService"/></item>
		///   <item><see cref="ExapisSOP.IO.Logging.ILoggingSystemService"/></item>
		///  </list>
		/// </returns>
		public static IConfiguration AddSystemServices(this IConfiguration config)
		{
			if (config == null) {
				throw new ArgumentNullException(nameof(config));
			}
			return config
				.AddFileSystem()
				.AddSettingsSystem()
				.AddCommandLine()
				.AddLoggingSystem();
		}

		/// <summary>
		///  全てのシステムサービスを既定の設定で実行環境に登録します。
		/// </summary>
		/// <param name="config">登録先の構成設定です。</param>
		/// <param name="fileSystemOptions"><see cref="ExapisSOP.IO.IFileSystemService"/>の設定を行います。</param>
		/// <param name="settingsSystemOptions"><see cref="ExapisSOP.IO.Settings.ISettingsSystemService"/>の設定を行います。</param>
		/// <param name="commandLineOptions"><see cref="ExapisSOP.IO.Settings.CommandLine.ICommandLineService"/>の設定を行います。</param>
		/// <param name="loggingSystemOptions"><see cref="ExapisSOP.IO.Logging.ILoggingSystemService"/>の設定を行います。</param>
		/// <returns>
		///  <paramref name="config"/>そのもの、または、
		///  下記のサービスがサービスリストに追加された新しい<paramref name="config"/>のコピーです。
		///  <list type="number">
		///   <item><see cref="ExapisSOP.IO.IFileSystemService"/></item>
		///   <item><see cref="ExapisSOP.IO.Settings.ISettingsSystemService"/></item>
		///   <item><see cref="ExapisSOP.IO.Settings.CommandLine.ICommandLineService"/></item>
		///   <item><see cref="ExapisSOP.IO.Logging.ILoggingSystemService"/></item>
		///  </list>
		/// </returns>
		public static IConfiguration AddSystemServices(this IConfiguration config,
			Func<FileSystemServiceOptions,     Task>? fileSystemOptions     = null,
			Func<SettingsSystemServiceOptions, Task>? settingsSystemOptions = null,
			Func<CommandLineServiceOptions,    Task>? commandLineOptions    = null,
			Func<LoggingSystemServiceOptions,  Task>? loggingSystemOptions  = null)
		{
			if (config == null) {
				throw new ArgumentNullException(nameof(config));
			}
			fileSystemOptions     ??= DefaultOptions;
			settingsSystemOptions ??= DefaultOptions;
			commandLineOptions    ??= DefaultOptions;
			loggingSystemOptions  ??= DefaultOptions;
			return config
				.AddFileSystem    (fileSystemOptions)
				.AddSettingsSystem(settingsSystemOptions)
				.AddCommandLine   (commandLineOptions)
				.AddLoggingSystem (loggingSystemOptions);
		}

		private static Task DefaultOptions(FileSystemServiceOptions _)
		{
			return Task.CompletedTask;
		}

		private static Task DefaultOptions(SettingsSystemServiceOptions _)
		{
			return Task.CompletedTask;
		}

		private static Task DefaultOptions(CommandLineServiceOptions _)
		{
			return Task.CompletedTask;
		}

		private static Task DefaultOptions(LoggingSystemServiceOptions _)
		{
			return Task.CompletedTask;
		}
	}
}
