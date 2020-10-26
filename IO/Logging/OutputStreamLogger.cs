/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.IO;

namespace ExapisSOP.IO.Logging
{
	/// <summary>
	///  出力ストリームとログファイルの両方にログ出力を行います。
	///  このクラスではストリームの解放は行いません。
	/// </summary>
	public class OutputStreamLogger : Logger
	{
		/// <summary>
		///  コンストラクタに渡されたストリームから作成したライターを取得します。
		/// </summary>
		protected TextWriter Writer { get; }

		/// <summary>
		///  型'<see cref="ExapisSOP.IO.Logging.OutputStreamLogger"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="stream">出力先のストリームです。</param>
		/// <param name="name">ロガーの名前です。一意である必要はありません。</param>
		/// <param name="logfile">出力先のログファイルです。</param>
		/// <exception cref="System.ArgumentNullException"/>
		public OutputStreamLogger(Stream stream, string name, ILogFile logfile) : base(name, logfile)
		{
			if (stream == null) {
				throw new ArgumentNullException(nameof(stream));
			}
			this.Writer = new StreamWriter(stream);
		}

		/// <summary>
		///  型'<see cref="ExapisSOP.IO.Logging.OutputStreamLogger"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="writer">出力先のライターです。</param>
		/// <param name="name">ロガーの名前です。一意である必要はありません。</param>
		/// <param name="logfile">出力先のログファイルです。</param>
		/// <exception cref="System.ArgumentNullException"/>
		public OutputStreamLogger(TextWriter writer, string name, ILogFile logfile) : base(name, logfile)
		{
			if (writer == null) {
				throw new ArgumentNullException(nameof(writer));
			}
			this.Writer = writer;
		}

		/// <summary>
		///  指定されたメッセージを<see cref="ExapisSOP.IO.Logging.LogLevel.Trace"/>レベルでログファイルとコンストラクタに渡されたストリームに書き込みます。
		/// </summary>
		/// <param name="message">書き込むメッセージです。</param>
		/// <param name="additionalData">追加情報です。</param>
		public override void Trace(string message, ILoggable? additionalData = null)
		{
			this.Writer.WriteLine($"TRACE: {message}");
			base.Trace(message, additionalData);
		}

		/// <summary>
		///  指定されたメッセージを<see cref="ExapisSOP.IO.Logging.LogLevel.Debug"/>レベルでログファイルとコンストラクタに渡されたストリームに書き込みます。
		/// </summary>
		/// <param name="message">書き込むメッセージです。</param>
		/// <param name="additionalData">追加情報です。</param>
		public override void Debug(string message, ILoggable? additionalData = null)
		{
			this.Writer.WriteLine($"DEBUG: {message}");
			base.Debug(message, additionalData);
		}

		/// <summary>
		///  指定されたメッセージを<see cref="ExapisSOP.IO.Logging.LogLevel.Info"/>レベルでログファイルとコンストラクタに渡されたストリームに書き込みます。
		/// </summary>
		/// <param name="message">書き込むメッセージです。</param>
		/// <param name="additionalData">追加情報です。</param>
		public override void Info(string message, ILoggable? additionalData = null)
		{
			this.Writer.WriteLine($"INFORMATION: {message}");
			base.Info(message, additionalData);
		}

		/// <summary>
		///  指定されたメッセージを<see cref="ExapisSOP.IO.Logging.LogLevel.Warn"/>レベルでログファイルとコンストラクタに渡されたストリームに書き込みます。
		/// </summary>
		/// <param name="message">書き込むメッセージです。</param>
		/// <param name="additionalData">追加情報です。</param>
		public override void Warn(string message, ILoggable? additionalData = null)
		{
			this.Writer.WriteLine($"WARNING: {message}");
			base.Warn(message, additionalData);
		}

		/// <summary>
		///  指定されたメッセージを<see cref="ExapisSOP.IO.Logging.LogLevel.Error"/>レベルでログファイルとコンストラクタに渡されたストリームに書き込みます。
		/// </summary>
		/// <param name="message">書き込むメッセージです。</param>
		/// <param name="additionalData">追加情報です。</param>
		public override void Error(string message, ILoggable? additionalData = null)
		{
			this.Writer.WriteLine($"ERROR: {message}");
			base.Error(message, additionalData);
		}

		/// <summary>
		///  指定されたメッセージを<see cref="ExapisSOP.IO.Logging.LogLevel.Fatal"/>レベルでログファイルとコンストラクタに渡されたストリームに書き込みます。
		/// </summary>
		/// <param name="message">書き込むメッセージです。</param>
		/// <param name="additionalData">追加情報です。</param>
		public override void Fatal(string message, ILoggable? additionalData = null)
		{
			this.Writer.WriteLine($"FATAL ERROR: {message}");
			base.Fatal(message, additionalData);
		}
	}
}
