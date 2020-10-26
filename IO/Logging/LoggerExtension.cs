/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using ExapisSOP.Properties;

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

		/// <summary>
		///  改行を含む長いメッセージをログに書き込みます。
		/// </summary>
		/// <param name="logger">書き込み先のロガーです。</param>
		/// <param name="level">ログレベルです。<see cref="ExapisSOP.IO.Logging.LogLevel.None"/>は指定できません。</param>
		/// <param name="shortMessage">短いメッセージです。<paramref name="longMessage"/>を要約します。</param>
		/// <param name="longMessage">長いメッセージです。</param>
		/// <see cref="System.ArgumentNullException"/>
		/// <see cref="System.ArgumentException"/>
		public static void LongMessage(this ILogger logger, LogLevel level, string shortMessage, string longMessage)
		{
			if (logger == null) {
				throw new ArgumentNullException(nameof(logger));
			}
			var data = new LongMessageRecord(longMessage);
			switch (level) {
			case LogLevel.Trace:
				logger.Trace(shortMessage, data);
				break;
			case LogLevel.Debug:
				logger.Debug(shortMessage, data);
				break;
			case LogLevel.Info:
				logger.Trace(shortMessage, data);
				break;
			case LogLevel.Warn:
				logger.Debug(shortMessage, data);
				break;
			case LogLevel.Error:
				logger.Trace(shortMessage, data);
				break;
			case LogLevel.Fatal:
				logger.Debug(shortMessage, data);
				break;
			default:
				throw new ArgumentException(string.Format(Resources.LoggerExtension_ArgumentException, level), nameof(level));
			}
		}

		/// <summary>
		///  指定された例外をログに書き込みます。
		/// </summary>
		/// <param name="logger">書き込み先のロガーです。</param>
		/// <param name="exception">書き込む例外オブジェクトです。</param>
		/// <see cref="System.ArgumentNullException"/>
		public static void Exception(this ILogger logger, Exception exception)
		{
			if (logger == null) {
				throw new ArgumentNullException(nameof(logger));
			}
			exception ??= new NullReferenceException("the exception was null.");
			string msg  = "The exception occurred: " + exception.Message;
			var    data = new ExceptionRecord(exception);
			logger.Warn(msg, data);
		}

		/// <summary>
		///  指定された処理されない例外をログに書き込みます。
		/// </summary>
		/// <param name="logger">書き込み先のロガーです。</param>
		/// <param name="exception">書き込む例外オブジェクトです。</param>
		/// <param name="isFatal">例外が致命的である場合は<see langword="true"/>を指定します。</param>
		/// <see cref="System.ArgumentNullException"/>
		public static void UnhandledException(this ILogger logger, Exception exception, bool isFatal = false)
		{
			if (logger == null) {
				throw new ArgumentNullException(nameof(logger));
			}
			exception ??= new NullReferenceException("the exception was null.");
			string msg  = "The unhandled exception occurred: " + exception.Message;
			var    data = new ExceptionRecord(exception);
			if (isFatal) {
				logger.Fatal(msg, data);
			} else {
				logger.Error(msg, data);
			}
		}
	}
}
