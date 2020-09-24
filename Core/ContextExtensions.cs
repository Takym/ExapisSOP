/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using ExapisSOP.IO;
using ExapisSOP.IO.Settings;

namespace ExapisSOP.Core
{
	/// <summary>
	///  <see cref="ExapisSOP.IContext"/>の機能を拡張します。
	///  このクラスは静的です。
	/// </summary>
	public static class ContextExtensions
	{
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
		///  サービスリストから最初に見つかった<see cref="ExapisSOP.IO.IFileSystemService"/>を取得します。
		/// </summary>
		/// <param name="context">現在の文脈情報です。</param>
		/// <returns>
		///  <see cref="ExapisSOP.IO.IFileSystemService"/>を実装したサービスオブジェクトです。
		/// </returns>
		public static IFileSystemService? GetFileSystem(this IContext context)
		{
			return context.GetService<IFileSystemService>();
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
			return context.GetService<ISettingsSystemService>();
		}
	}
}
