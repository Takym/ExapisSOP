/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using ExapisSOP.Utils;

namespace ExapisSOP.IO.Logging
{
	/// <summary>
	///  <see cref="ExapisSOP.IO.Logging.LogFile"/>に<see cref="ExapisSOP.IO.Logging.LogData"/>を書き込みます。
	/// </summary>
	public class Logger : ILogger
	{
		internal const string Noname = "!NONAME";

		/// <summary>
		///  このロガーの名前を取得します。
		/// </summary>
		public string Name { get; }

		/// <summary>
		///  このロガーの出力先のファイルを取得します。
		/// </summary>
		public ILogFile LogFile { get; }

		/// <summary>
		///  型'<see cref="ExapisSOP.IO.Logging.Logger"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="name">ロガーの名前です。一意である必要はありません。</param>
		/// <param name="logfile">出力先のログファイルです。</param>
		/// <exception cref="System.ArgumentNullException"/>
		public Logger(string name, ILogFile logfile)
		{
			if (logfile == null) {
				throw new ArgumentNullException(nameof(logfile));
			}
			this.Name    = name?.FitToLine() ?? Noname;
			this.LogFile = logfile;
		}

		/// <summary>
		///  指定されたメッセージを<see cref="ExapisSOP.IO.Logging.LogLevel.Trace"/>レベルで書き込みます。
		/// </summary>
		/// <param name="message">書き込むメッセージです。</param>
		/// <param name="additionalData">追加情報です。</param>
		public virtual void Trace(string message, ILoggable? additionalData = null)
		{
			this.WriteLog(LogLevel.Trace, message, additionalData);
		}

		/// <summary>
		///  指定されたメッセージを<see cref="ExapisSOP.IO.Logging.LogLevel.Debug"/>レベルで書き込みます。
		/// </summary>
		/// <param name="message">書き込むメッセージです。</param>
		/// <param name="additionalData">追加情報です。</param>
		public virtual void Debug(string message, ILoggable? additionalData = null)
		{
			this.WriteLog(LogLevel.Debug, message, additionalData);
		}

		/// <summary>
		///  指定されたメッセージを<see cref="ExapisSOP.IO.Logging.LogLevel.Info"/>レベルで書き込みます。
		/// </summary>
		/// <param name="message">書き込むメッセージです。</param>
		/// <param name="additionalData">追加情報です。</param>
		public virtual void Info(string message, ILoggable? additionalData = null)
		{
			this.WriteLog(LogLevel.Info, message, additionalData);
		}

		/// <summary>
		///  指定されたメッセージを<see cref="ExapisSOP.IO.Logging.LogLevel.Warn"/>レベルで書き込みます。
		/// </summary>
		/// <param name="message">書き込むメッセージです。</param>
		/// <param name="additionalData">追加情報です。</param>
		public virtual void Warn(string message, ILoggable? additionalData = null)
		{
			this.WriteLog(LogLevel.Warn, message, additionalData);
		}

		/// <summary>
		///  指定されたメッセージを<see cref="ExapisSOP.IO.Logging.LogLevel.Error"/>レベルで書き込みます。
		/// </summary>
		/// <param name="message">書き込むメッセージです。</param>
		/// <param name="additionalData">追加情報です。</param>
		public virtual void Error(string message, ILoggable? additionalData = null)
		{
			this.WriteLog(LogLevel.Error, message, additionalData);
		}

		/// <summary>
		///  指定されたメッセージを<see cref="ExapisSOP.IO.Logging.LogLevel.Fatal"/>レベルで書き込みます。
		/// </summary>
		/// <param name="message">書き込むメッセージです。</param>
		/// <param name="additionalData">追加情報です。</param>
		public virtual void Fatal(string message, ILoggable? additionalData = null)
		{
			this.WriteLog(LogLevel.Fatal, message, additionalData);
		}

		/// <summary>
		///  ログの書き込み処理を実行します。
		/// </summary>
		/// <param name="level">ログレベルです。</param>
		/// <param name="message">書き込むメッセージです。</param>
		/// <param name="additionalData">追加情報です。</param>
		protected void WriteLog(LogLevel level, string message, ILoggable? additionalData)
		{
			this.LogFile.AddLog(new LogData(level, this, message.FitToLine(), additionalData));
		}
	}
}
