/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 *
 * distributed under the MIT License.
****/

namespace ExapisSOP.IO.Logging
{
	/// <summary>
	///  ログ出力を行う機能を提供します。
	/// </summary>
	public interface ILogger
	{
		/// <summary>
		///  このロガーの名前を取得します。
		/// </summary>
		string Name { get; }

		/// <summary>
		///  このロガーの出力先のファイルを取得します。
		/// </summary>
		ILogFile LogFile { get; }

		/// <summary>
		///  指定されたメッセージを<see cref="ExapisSOP.IO.Logging.LogLevel.Trace"/>レベルで書き込みます。
		/// </summary>
		/// <param name="message">書き込むメッセージです。</param>
		/// <param name="additionalData">追加情報です。</param>
		void Trace(string message, ILoggable? additionalData = null);

		/// <summary>
		///  指定されたメッセージを<see cref="ExapisSOP.IO.Logging.LogLevel.Debug"/>レベルで書き込みます。
		/// </summary>
		/// <param name="message">書き込むメッセージです。</param>
		/// <param name="additionalData">追加情報です。</param>
		void Debug(string message, ILoggable? additionalData = null);

		/// <summary>
		///  指定されたメッセージを<see cref="ExapisSOP.IO.Logging.LogLevel.Info"/>レベルで書き込みます。
		/// </summary>
		/// <param name="message">書き込むメッセージです。</param>
		/// <param name="additionalData">追加情報です。</param>
		void Info(string message, ILoggable? additionalData = null);

		/// <summary>
		///  指定されたメッセージを<see cref="ExapisSOP.IO.Logging.LogLevel.Warn"/>レベルで書き込みます。
		/// </summary>
		/// <param name="message">書き込むメッセージです。</param>
		/// <param name="additionalData">追加情報です。</param>
		void Warn(string message, ILoggable? additionalData = null);

		/// <summary>
		///  指定されたメッセージを<see cref="ExapisSOP.IO.Logging.LogLevel.Error"/>レベルで書き込みます。
		/// </summary>
		/// <param name="message">書き込むメッセージです。</param>
		/// <param name="additionalData">追加情報です。</param>
		void Error(string message, ILoggable? additionalData = null);

		/// <summary>
		///  指定されたメッセージを<see cref="ExapisSOP.IO.Logging.LogLevel.Fatal"/>レベルで書き込みます。
		/// </summary>
		/// <param name="message">書き込むメッセージです。</param>
		/// <param name="additionalData">追加情報です。</param>
		void Fatal(string message, ILoggable? additionalData = null);
	}
}
