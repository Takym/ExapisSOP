/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System;
using System.Collections.Generic;
using ExapisSOP.IO;
using ExapisSOP.IO.Logging;
using ExapisSOP.IO.Settings;
using ExapisSOP.IO.Settings.CommandLine;

namespace ExapisSOP.Core
{
	/// <summary>
	///  <see cref="ExapisSOP.IContext"/>の機能を拡張します。
	///  このクラスは静的です。
	/// </summary>
	public static class ContextExtensions
	{
		/// <summary>
		///  <see cref="ExapisSOP.Core.ContextExtensions.GetData(IContext, string)"/>及び
		///  <see cref="ExapisSOP.Core.ContextExtensions.SetData(IContext, string, object?)"/>で
		///  利用される既定のキー名を取得します。
		/// </summary>
		public const string DefaultKeyName = "_(*)";

		/// <summary>
		///  データディレクトリへのパスを格納したオブジェクトを取得します。
		/// </summary>
		/// <param name="context">現在の文脈情報です。</param>
		/// <returns>
		///  <see cref="ExapisSOP.IContext.Paths"/>を<see cref="ExapisSOP.IO.Paths"/>へ変換して返します。
		///  変換に失敗した場合は<see langword="null"/>が返ります。
		/// </returns>
		public static Paths? GetPaths(this IContext context)
		{
			return context?.Paths as Paths;
		}

		/// <summary>
		///  プログラムが初回起動かどうか判定します。
		/// </summary>
		/// <param name="context">現在の文脈情報です。</param>
		/// <returns>
		///  初回起動である場合は<see langword="true"/>、
		///  初回起動でない場合は<see langword="false"/>、
		///  判定できなかった場合は<see langword="null"/>を返します。
		/// </returns>
		public static bool? IsFirstBoot(this IContext context)
		{
			return context?.Settings?.FirstBoot;
		}

		/// <summary>
		///  プログラムが多重起動しているかどうか判定します。
		/// </summary>
		/// <param name="context">現在の文脈情報です。</param>
		/// <returns>
		///  多重起動している場合は<see langword="true"/>、
		///  多重起動していない場合は<see langword="false"/>、
		///  判定できなかった場合は<see langword="null"/>を返します。
		/// </returns>
		public static bool? IsMultipleBoot(this IContext context)
		{
			return context?.GetPaths()?.ExistsLockFile;
		}

		/// <summary>
		///  設定ファイルと現在のバージョンに互換性がないかどうか判定します。
		/// </summary>
		/// <param name="context">現在の文脈情報です。</param>
		/// <returns>
		///  互換性がない場合は<see langword="true"/>、
		///  互換性がある場合は<see langword="false"/>、
		///  判定できなかった場合は<see langword="null"/>を返します。
		/// </returns>
		public static bool? HasNoCompatible(this IContext context)
		{
			return context?.Settings?.NoCompatible;
		}

		/// <summary>
		///  プログラム初期化時に利用された文脈情報を取得します。
		/// </summary>
		/// <remarks>
		///  カスタム文脈情報からは取得する事はできません。
		/// </remarks>
		/// <param name="context">現在の文脈情報です。</param>
		/// <returns>
		///  初期化文脈情報です。
		///  <see cref="ExapisSOP.IContext"/>が初期化文脈情報の取得に対応していない場合は<see langword="null"/>です。
		/// </returns>
		public static IContext? GetInitialContext(this IContext context)
		{
			if (context is InitFinalContext initFinalContext) {
				return initFinalContext.GetInitContext();
			} else if (context is EventLoopContext eventLoopContext) {
				return eventLoopContext._init;
			} else {
				return null;
			}
		}

		/// <summary>
		///  前回のイベントループに利用された文脈情報を取得します。
		/// </summary>
		/// <remarks>
		///  カスタム文脈情報からは取得する事はできません。
		/// </remarks>
		/// <param name="context">現在の文脈情報です。</param>
		/// <returns>
		///  イベントループ文脈情報です。
		///  今回のループが最初の場合は<see langword="null"/>を返します。
		///  <see cref="ExapisSOP.IContext"/>がイベントループ文脈情報の取得に対応していない場合は<see langword="null"/>です。
		/// </returns>
		public static IContext? GetPreviousContext(this IContext context)
		{
			if (context is EventLoopContext eventLoopContext) {
				return eventLoopContext.GetPrev();
			} else {
				return null;
			}
		}

		/// <summary>
		///  文脈情報からメッセージデータを取得します。
		/// </summary>
		/// <param name="context">現在の文脈情報です。</param>
		/// <param name="key">メッセージデータに関連付けられている名前です。</param>
		/// <returns>文脈情報に登録されているオブジェクト、または、存在しない場合は<see langword="null"/>を返します。</returns>
		/// <exception cref="System.ArgumentNullException" />
		public static object? GetData(this IContext context, string key)
		{
			if (context == null) {
				throw new ArgumentNullException(nameof(context));
			}
			if (string.IsNullOrEmpty(key)) {
				key = DefaultKeyName;
			}
			if (context.GetMessage() is IDictionary<string, object> dict) {
				dict.TryGetValue(key, out object? result);
				return result;
			} else {
				return null;
			}
		}

		/// <summary>
		///  文脈情報に指定されたメッセージデータを設定します。
		///  辞書以外のオブジェクトが設定されている場合は上書きされます。
		///  <paramref name="value"/>が<see langword="null"/>の場合、値は削除されます。
		/// </summary>
		/// <param name="context">現在の文脈情報です。</param>
		/// <param name="key">メッセージデータに関連付ける名前です。</param>
		/// <param name="value">メッセージデータの値です。</param>
		/// <exception cref="System.ArgumentNullException" />
		public static void SetData(this IContext context, string key, object? value)
		{
			if (context == null) {
				throw new ArgumentNullException(nameof(context));
			}
			if (string.IsNullOrEmpty(key)) {
				key = DefaultKeyName;
			}
			if (context.GetMessage() is IDictionary<string, object> dict) {
				if (value == null) {
					dict.Remove(key);
				} else {
					if (dict.ContainsKey(key)) {
						dict[key] = value;
					} else {
						dict.Add(key, value);
					}
				}
			} else if (value != null) {
				var newdict = new Dictionary<string, object>();
				newdict.Add(key, value);
				context.SetMessage(newdict);
			}
		}

		/// <summary>
		///  サービスリストから最初に見つかった<see cref="ExapisSOP.IO.IFileSystemService"/>を取得します。
		/// </summary>
		/// <param name="context">現在の文脈情報です。</param>
		/// <returns>
		///  <see cref="ExapisSOP.IO.IFileSystemService"/>を実装したサービスオブジェクトです。
		/// </returns>
		public static IFileSystemService? GetFileSystem(this IContext context)
		{
			return context?.GetService<IFileSystemService>();
		}

		/// <summary>
		///  サービスリストから最初に見つかった<see cref="ExapisSOP.IO.Settings.ISettingsSystemService"/>を取得します。
		/// </summary>
		/// <param name="context">現在の文脈情報です。</param>
		/// <returns>
		///  <see cref="ExapisSOP.IO.Settings.ISettingsSystemService"/>を実装したサービスオブジェクトです。
		/// </returns>
		public static ISettingsSystemService? GetSettingsSystem(this IContext context)
		{
			return context?.GetService<ISettingsSystemService>();
		}

		/// <summary>
		///  サービスリストから最初に見つかった<see cref="ExapisSOP.IO.Settings.CommandLine.ICommandLineService"/>を取得します。
		/// </summary>
		/// <param name="context">現在の文脈情報です。</param>
		/// <returns>
		///  <see cref="ExapisSOP.IO.Settings.CommandLine.ICommandLineService"/>を実装したサービスオブジェクトです。
		/// </returns>
		public static ICommandLineService? GetCommandLine(this IContext context)
		{
			return context?.GetService<ICommandLineService>();
		}

		/// <summary>
		///  サービスリストから最初に見つかった<see cref="ExapisSOP.IO.Logging.ILoggingSystemService"/>を取得します。
		/// </summary>
		/// <param name="context">現在の文脈情報です。</param>
		/// <returns>
		///  <see cref="ExapisSOP.IO.Logging.ILoggingSystemService"/>を実装したサービスオブジェクトです。
		/// </returns>
		public static ILoggingSystemService? GetLoggingSystem(this IContext context)
		{
			return context?.GetService<ILoggingSystemService>();
		}
	}
}
