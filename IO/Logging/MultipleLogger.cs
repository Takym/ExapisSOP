/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ExapisSOP.IO.Logging
{
	/// <summary>
	///  複数のログファイルにログ情報の出力を行います。
	///  このクラスは継承できません。
	/// </summary>
	public sealed class MultipleLogger : Logger
	{
		private readonly Logger[] _loggers;

		/// <summary>
		///  このロガーに格納されている<see cref="ExapisSOP.IO.Logging.Logger.LogFile"/>を除く全てのログファイルを取得します。
		/// </summary>
		public IReadOnlyList<ILogFile> LogFiles { get; }

		/// <summary>
		///  型'<see cref="ExapisSOP.IO.Logging.MultipleLogger"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="name">ロガーの名前です。一意である必要はありません。</param>
		/// <param name="mainLogfile">出力先のログファイルです。</param>
		/// <param name="logfiles">追加の出力先のログファイルです。</param>
		/// <exception cref="System.ArgumentNullException"/>
		public MultipleLogger(string name, ILogFile mainLogfile, params ILogFile[] logfiles) : base(name, mainLogfile)
		{
			if (logfiles == null) {
				throw new ArgumentNullException(nameof(logfiles));
			}
			this.LogFiles = new ReadOnlyCollection<ILogFile>(logfiles);
			_loggers      = new Logger[logfiles.Length];
			for (int i = 0; i < _loggers.Length; ++i) {
				_loggers[i] = new Logger(name, logfiles[i]);
			}
		}

		/// <summary>
		///  指定されたメッセージを<see cref="ExapisSOP.IO.Logging.LogLevel.Trace"/>レベルで書き込みます。
		/// </summary>
		/// <param name="message">書き込むメッセージです。</param>
		/// <param name="additionalData">追加情報です。</param>
		public override void Trace(string message, ILoggable? additionalData = null)
		{
			base.Trace(message, additionalData);
			for (int i = 0; i < _loggers.Length; ++i) {
				_loggers[i].Trace(message, additionalData);
			}
		}

		/// <summary>
		///  指定されたメッセージを<see cref="ExapisSOP.IO.Logging.LogLevel.Debug"/>レベルで書き込みます。
		/// </summary>
		/// <param name="message">書き込むメッセージです。</param>
		/// <param name="additionalData">追加情報です。</param>
		public override void Debug(string message, ILoggable? additionalData = null)
		{
			base.Debug(message, additionalData);
			for (int i = 0; i < _loggers.Length; ++i) {
				_loggers[i].Debug(message, additionalData);
			}
		}

		/// <summary>
		///  指定されたメッセージを<see cref="ExapisSOP.IO.Logging.LogLevel.Info"/>レベルで書き込みます。
		/// </summary>
		/// <param name="message">書き込むメッセージです。</param>
		/// <param name="additionalData">追加情報です。</param>
		public override void Info(string message, ILoggable? additionalData = null)
		{
			base.Info(message, additionalData);
			for (int i = 0; i < _loggers.Length; ++i) {
				_loggers[i].Info(message, additionalData);
			}
		}

		/// <summary>
		///  指定されたメッセージを<see cref="ExapisSOP.IO.Logging.LogLevel.Warn"/>レベルで書き込みます。
		/// </summary>
		/// <param name="message">書き込むメッセージです。</param>
		/// <param name="additionalData">追加情報です。</param>
		public override void Warn(string message, ILoggable? additionalData = null)
		{
			base.Warn(message, additionalData);
			for (int i = 0; i < _loggers.Length; ++i) {
				_loggers[i].Warn(message, additionalData);
			}
		}

		/// <summary>
		///  指定されたメッセージを<see cref="ExapisSOP.IO.Logging.LogLevel.Error"/>レベルで書き込みます。
		/// </summary>
		/// <param name="message">書き込むメッセージです。</param>
		/// <param name="additionalData">追加情報です。</param>
		public override void Error(string message, ILoggable? additionalData = null)
		{
			base.Error(message, additionalData);
			for (int i = 0; i < _loggers.Length; ++i) {
				_loggers[i].Error(message, additionalData);
			}
		}

		/// <summary>
		///  指定されたメッセージを<see cref="ExapisSOP.IO.Logging.LogLevel.Fatal"/>レベルで書き込みます。
		/// </summary>
		/// <param name="message">書き込むメッセージです。</param>
		/// <param name="additionalData">追加情報です。</param>
		public override void Fatal(string message, ILoggable? additionalData = null)
		{
			base.Fatal(message, additionalData);
			for (int i = 0; i < _loggers.Length; ++i) {
				_loggers[i].Fatal(message, additionalData);
			}
		}
	}
}
