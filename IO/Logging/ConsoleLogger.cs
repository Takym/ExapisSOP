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
	///  コンソール画面とログファイルの両方にログ出力を行います。
	/// </summary>
	public class ConsoleLogger : OutputStreamLogger
	{
		/// <summary>
		///  型'<see cref="ExapisSOP.IO.Logging.OutputStreamLogger"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="logfile">出力先のログファイルです。</param>
		/// <exception cref="System.ArgumentNullException"/>
		public ConsoleLogger(ILogFile logfile) : base(Console.Out, "console", logfile) { }

		/// <summary>
		///  指定されたメッセージを<see cref="ExapisSOP.IO.Logging.LogLevel.Trace"/>レベルでログファイルとコンソール画面に書き込みます。
		/// </summary>
		/// <param name="message">書き込むメッセージです。</param>
		/// <param name="additionalData">追加情報です。</param>
		public override void Trace(string message, ILoggable? additionalData = null)
		{
			var foreColor = Console.ForegroundColor;
			var backColor = Console.BackgroundColor;
			Console.ForegroundColor = ConsoleColor.Green;
			Console.BackgroundColor = ConsoleColor.Gray;
			base.Trace(message, additionalData);
			Console.ForegroundColor = foreColor;
			Console.BackgroundColor = backColor;
		}

		/// <summary>
		///  指定されたメッセージを<see cref="ExapisSOP.IO.Logging.LogLevel.Debug"/>レベルでログファイルとコンソール画面に書き込みます。
		/// </summary>
		/// <param name="message">書き込むメッセージです。</param>
		/// <param name="additionalData">追加情報です。</param>
		public override void Debug(string message, ILoggable? additionalData = null)
		{
			var foreColor = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Blue;
			base.Debug(message, additionalData);
			Console.ForegroundColor = foreColor;
		}

		/// <summary>
		///  指定されたメッセージを<see cref="ExapisSOP.IO.Logging.LogLevel.Info"/>レベルでログファイルとコンソール画面に書き込みます。
		/// </summary>
		/// <param name="message">書き込むメッセージです。</param>
		/// <param name="additionalData">追加情報です。</param>
		public override void Info(string message, ILoggable? additionalData = null)
		{
			this.Writer.WriteLine(message);
			this.WriteLog(LogLevel.Info, message, additionalData);
		}

		/// <summary>
		///  指定されたメッセージを<see cref="ExapisSOP.IO.Logging.LogLevel.Warn"/>レベルでログファイルとコンソール画面に書き込みます。
		/// </summary>
		/// <param name="message">書き込むメッセージです。</param>
		/// <param name="additionalData">追加情報です。</param>
		public override void Warn(string message, ILoggable? additionalData = null)
		{
			var foreColor = Console.ForegroundColor;
			var backColor = Console.BackgroundColor;
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.BackgroundColor = ConsoleColor.Gray;
			this.Writer.WriteLine(message);
			this.WriteLog(LogLevel.Info, message, additionalData);
			Console.ForegroundColor = foreColor;
			Console.BackgroundColor = backColor;
		}

		/// <summary>
		///  指定されたメッセージを<see cref="ExapisSOP.IO.Logging.LogLevel.Error"/>レベルでログファイルとコンソール画面に書き込みます。
		/// </summary>
		/// <param name="message">書き込むメッセージです。</param>
		/// <param name="additionalData">追加情報です。</param>
		public override void Error(string message, ILoggable? additionalData = null)
		{
			var foreColor = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Red;
			base.Error(message, additionalData);
			Console.ForegroundColor = foreColor;
		}

		/// <summary>
		///  指定されたメッセージを<see cref="ExapisSOP.IO.Logging.LogLevel.Fatal"/>レベルでログファイルとコンソール画面に書き込みます。
		/// </summary>
		/// <param name="message">書き込むメッセージです。</param>
		/// <param name="additionalData">追加情報です。</param>
		public override void Fatal(string message, ILoggable? additionalData = null)
		{
			var foreColor = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Red;
			base.Fatal(message, additionalData);
			Console.ForegroundColor = foreColor;
		}
	}
}
