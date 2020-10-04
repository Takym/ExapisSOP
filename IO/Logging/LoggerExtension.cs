/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System;

namespace ExapisSOP.IO.Logging
{
	/// <summary>
	///  <see cref="ExapisSOP.IO.Logging.ILogger"/>の機能を拡張します。
	///  このクラスは静的です。
	/// </summary>
	public static class LoggerExtension
	{
		/// <summary>
		///  指定されたメッセージを<see cref="ExapisSOP.IO.Logging.LogLevel.Trace"/>レベルで書き込みます。
		/// </summary>
		/// <param name="logger">書き込み先のロガーです。</param>
		/// <param name="format">複合書式設定文字列です。</param>
		/// <param name="args">書式設定対象のオブジェクトを含む配列です。</param>
		/// <see cref="System.ArgumentNullException"/>
		public static void Trace(this ILogger logger, string format, params object?[] args)
		{
			if (logger == null) {
				throw new ArgumentNullException(nameof(logger));
			}
			logger.Trace(string.Format(format, args));
		}

		/// <summary>
		///  指定されたメッセージを<see cref="ExapisSOP.IO.Logging.LogLevel.Debug"/>レベルで書き込みます。
		/// </summary>
		/// <param name="logger">書き込み先のロガーです。</param>
		/// <param name="format">複合書式設定文字列です。</param>
		/// <param name="args">書式設定対象のオブジェクトを含む配列です。</param>
		/// <see cref="System.ArgumentNullException"/>
		public static void Debug(this ILogger logger, string format, params object?[] args)
		{
			if (logger == null) {
				throw new ArgumentNullException(nameof(logger));
			}
			logger.Debug(string.Format(format, args));
		}

		/// <summary>
		///  指定されたメッセージを<see cref="ExapisSOP.IO.Logging.LogLevel.Info"/>レベルで書き込みます。
		/// </summary>
		/// <param name="logger">書き込み先のロガーです。</param>
		/// <param name="format">複合書式設定文字列です。</param>
		/// <param name="args">書式設定対象のオブジェクトを含む配列です。</param>
		/// <see cref="System.ArgumentNullException"/>
		public static void Info(this ILogger logger, string format, params object?[] args)
		{
			if (logger == null) {
				throw new ArgumentNullException(nameof(logger));
			}
			logger.Info(string.Format(format, args));
		}

		/// <summary>
		///  指定されたメッセージを<see cref="ExapisSOP.IO.Logging.LogLevel.Warn"/>レベルで書き込みます。
		/// </summary>
		/// <param name="logger">書き込み先のロガーです。</param>
		/// <param name="format">複合書式設定文字列です。</param>
		/// <param name="args">書式設定対象のオブジェクトを含む配列です。</param>
		/// <see cref="System.ArgumentNullException"/>
		public static void Warn(this ILogger logger, string format, params object?[] args)
		{
			if (logger == null) {
				throw new ArgumentNullException(nameof(logger));
			}
			logger.Warn(string.Format(format, args));
		}

		/// <summary>
		///  指定されたメッセージを<see cref="ExapisSOP.IO.Logging.LogLevel.Error"/>レベルで書き込みます。
		/// </summary>
		/// <param name="logger">書き込み先のロガーです。</param>
		/// <param name="format">複合書式設定文字列です。</param>
		/// <param name="args">書式設定対象のオブジェクトを含む配列です。</param>
		/// <see cref="System.ArgumentNullException"/>
		public static void Error(this ILogger logger, string format, params object?[] args)
		{
			if (logger == null) {
				throw new ArgumentNullException(nameof(logger));
			}
			logger.Error(string.Format(format, args));
		}

		/// <summary>
		///  指定されたメッセージを<see cref="ExapisSOP.IO.Logging.LogLevel.Fatal"/>レベルで書き込みます。
		/// </summary>
		/// <param name="logger">書き込み先のロガーです。</param>
		/// <param name="format">複合書式設定文字列です。</param>
		/// <param name="args">書式設定対象のオブジェクトを含む配列です。</param>
		/// <see cref="System.ArgumentNullException"/>
		public static void Fatal(this ILogger logger, string format, params object?[] args)
		{
			if (logger == null) {
				throw new ArgumentNullException(nameof(logger));
			}
			logger.Fatal(string.Format(format, args));
		}
	}
}
