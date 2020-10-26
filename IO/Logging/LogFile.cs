/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;
using ExapisSOP.Properties;
using ExapisSOP.Utils;

namespace ExapisSOP.IO.Logging
{
	/// <summary>
	///  ログファイルの基礎的な機能を表します。
	/// </summary>
	public abstract class LogFile : DisposableBase, ILogFile
	{
		#region 動的

		private readonly ConsoleLogger _console_logger;

		/// <summary>
		///  上書きされた場合、このログファイルに追加されたログ情報の個数を取得します。
		/// </summary>
		public abstract ulong Count { get; }

		/// <summary>
		///  型'<see cref="ExapisSOP.IO.Logging.LogFile"/>'の新しいインスタンスを生成します。
		/// </summary>
		protected LogFile()
		{
			_console_logger = new ConsoleLogger(this);
		}

		/// <summary>
		///  既定の名前でロガーを生成します。
		/// </summary>
		/// <returns>作成された新しいロガーです。</returns>
		/// <exception cref="System.ObjectDisposedException"/>
		public ILogger CreateLogger()
		{
			this.ThrowOnObjectDisposed();
			var trace = new StackTrace(1, false);
			for (int i = 0; i < trace.FrameCount; ++i) {
				var frame = trace.GetFrame(i);
				var t     = frame?.GetMethod()?.ReflectedType;
				if (t != null && !(t.Namespace?.StartsWith($"{nameof(ExapisSOP)}.{nameof(IO)}.{nameof(Logging)}") ?? true)) {
					var result = new Logger(t.Name, this);
					result.Info($"The new logger {result.Name} is created and set name automatically by the log file.");
					return result;
				}
			}
			var result2 = new Logger(Logger.Noname, this);
			result2.Warn($"The new logger is created but could not set a name.");
			return result2;
		}

		/// <summary>
		///  標準出力ストリームへの出力を行うロガーを取得します。
		/// </summary>
		/// <returns>既定のコンソールロガーです。</returns>
		/// <exception cref="System.ObjectDisposedException"/>
		public ILogger GetConsoleLogger()
		{
			this.ThrowOnObjectDisposed();
			return _console_logger;
		}

		/// <summary>
		///  指定されたログ情報を末尾に追加します。
		/// </summary>
		/// <param name="data">追加するログ情報です。</param>
		/// <exception cref="System.ArgumentNullException"/>
		/// <exception cref="System.ObjectDisposedException"/>
		public void AddLog(LogData data)
		{
			this.ThrowOnObjectDisposed();
			if (data == null) {
				throw new ArgumentNullException(nameof(data));
			}
			this.AddLogCore(data);
		}

		/// <summary>
		///  このログファイルから指定された位置のログ情報を取得します。
		/// </summary>
		/// <param name="index">ログ情報のインデックス番号です。</param>
		/// <returns>取得したログ情報を表すオブジェクトです。</returns>
		/// <exception cref="System.ArgumentOutOfRangeException"/>
		/// <exception cref="System.ObjectDisposedException"/>
		public LogData GetLog(ulong index)
		{
			this.ThrowOnObjectDisposed();
			if (this.Count <= index) {
				throw new ArgumentOutOfRangeException(nameof(index), index, Resources.LogFile_ArgumentOutOfRangeException);
			}
			return this.GetLogCore(index);
		}

		/// <summary>
		///  上書きされた場合、指定されたログ情報を末尾に追加します。
		/// </summary>
		/// <param name="data">追加するログ情報です。</param>
		protected abstract void AddLogCore(LogData data);

		/// <summary>
		///  上書きされた場合、このログファイルから指定された位置のログ情報を取得します。
		/// </summary>
		/// <param name="index">ログ情報のインデックス番号です。</param>
		/// <returns>取得したログ情報を表すオブジェクトです。</returns>
		protected abstract LogData GetLogCore(ulong index);

		/// <summary>
		///  このログ情報を反復処理する列挙子を取得します。
		/// </summary>
		/// <returns>このログ情報を反復処理する事ができる<see cref="System.Collections.Generic.IEnumerator{T}"/>オブジェクトです。</returns>
		/// <exception cref="System.ObjectDisposedException"/>
		public IEnumerator<LogData> GetEnumerator()
		{
			this.ThrowOnObjectDisposed();
			var result = new LogDataEnumerator(this);
			this.DisposableObjects.Add(result);
			return result;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion

		#region 静的

		/// <summary>
		///  直列化形式のログファイルを作成します。
		///  既にストリーム内にログ情報が保存されている場合はその情報を読み込みます。
		/// </summary>
		/// <param name="stream">ログ情報の書き込み先のストリームです。</param>
		/// <returns>新しく生成されたログファイルです。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		public static LogFile CreateSerializedFile(Stream stream)
		{
			if (stream == null) {
				throw new ArgumentNullException(nameof(stream));
			}
			return new SerializedLogFile(stream);
		}

		/// <summary>
		///  テキスト形式のログファイルを作成します。
		/// </summary>
		/// <param name="writer">ログ情報の書き込み先のライターです。</param>
		/// <returns>新しく生成されたログファイルです。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		public static LogFile CreateTextFile(TextWriter writer)
		{
			if (writer == null) {
				throw new ArgumentNullException(nameof(writer));
			}
			return new TextLogFile(writer);
		}

		/// <summary>
		///  テキスト形式のログファイルを作成します。
		/// </summary>
		/// <param name="stream">ログ情報の書き込み先のストリームです。</param>
		/// <returns>新しく生成されたログファイルです。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		public static LogFile CreateTextFile(Stream stream)
		{
			if (stream == null) {
				throw new ArgumentNullException(nameof(stream));
			}
			return new TextLogFile(new StreamWriter(stream));
		}

		/// <summary>
		///  XML形式のログファイルを作成します。
		/// </summary>
		/// <param name="writer">ログ情報の書き込み先のライターです。</param>
		/// <returns>新しく生成されたログファイルです。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		public static LogFile CreateXmlFile(XmlWriter writer)
		{
			if (writer == null) {
				throw new ArgumentNullException(nameof(writer));
			}
			return new XmlLogFile(writer);
		}

		/// <summary>
		///  XML形式のログファイルを作成します。
		/// </summary>
		/// <param name="writer">ログ情報の書き込み先のライターです。</param>
		/// <returns>新しく生成されたログファイルです。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		public static LogFile CreateXmlFile(TextWriter writer)
		{
			if (writer == null) {
				throw new ArgumentNullException(nameof(writer));
			}
			return new XmlLogFile(XmlWriter.Create(writer));
		}

		/// <summary>
		///  XML形式のログファイルを作成します。
		/// </summary>
		/// <param name="stream">ログ情報の書き込み先のストリームです。</param>
		/// <returns>新しく生成されたログファイルです。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		public static LogFile CreateXmlFile(Stream stream)
		{
			if (stream == null) {
				throw new ArgumentNullException(nameof(stream));
			}
			return new XmlLogFile(XmlWriter.Create(stream));
		}

		/// <summary>
		///  XML形式のログファイルを開き、指定されたライターへ情報をコピーします。
		/// </summary>
		/// <param name="reader">ログ情報の読み取り元のリーダーです。</param>
		/// <param name="writer">ログ情報の書き込み先のライターです。</param>
		/// <returns>新しく生成されたログファイルです。</returns>
		public static LogFile CreateXmlFile(XmlReader reader, XmlWriter writer)
		{
			if (reader == null) {
				throw new ArgumentNullException(nameof(reader));
			}
			if (writer == null) {
				throw new ArgumentNullException(nameof(writer));
			}
			return new XmlLogFile(reader, writer);
		}

		/// <summary>
		///  XML形式のログファイルを開き、指定されたライターへ情報をコピーします。
		/// </summary>
		/// <param name="reader">ログ情報の読み取り元のリーダーです。</param>
		/// <param name="writer">ログ情報の書き込み先のライターです。</param>
		/// <returns>新しく生成されたログファイルです。</returns>
		public static LogFile CreateXmlFile(TextReader reader, TextWriter writer)
		{
			if (reader == null) {
				throw new ArgumentNullException(nameof(reader));
			}
			if (writer == null) {
				throw new ArgumentNullException(nameof(writer));
			}
			var xr = XmlReader.Create(reader);
			xr.ReadStartElement("logs");
			return new XmlLogFile(xr, XmlWriter.Create(writer));
		}

		/// <summary>
		///  XML形式のログファイルを開き、指定されたストリームへ情報をコピーします。
		/// </summary>
		/// <param name="inputStream">ログ情報の読み取り元のストリームです。</param>
		/// <param name="outputStream">ログ情報の書き込み先のストリームです。</param>
		/// <returns>新しく生成されたログファイルです。</returns>
		public static LogFile CreateXmlFile(Stream inputStream, Stream outputStream)
		{
			if (inputStream == null) {
				throw new ArgumentNullException(nameof(inputStream));
			}
			if (outputStream == null) {
				throw new ArgumentNullException(nameof(outputStream));
			}
			var xr = XmlReader.Create(inputStream);
			xr.ReadStartElement("logs");
			return new XmlLogFile(xr, XmlWriter.Create(outputStream));
		}

		#endregion

		#region 子クラス

		private sealed class LogDataEnumerator : IEnumerator<LogData>
		{
			private readonly LogFile _logFile;
			private          bool    _init;
			private          ulong   _i;
			public           bool    IsDisposed { get; private set; }

			public LogData Current
			{
				get
				{
					if (this.IsDisposed) {
						throw new ObjectDisposedException(nameof(LogDataEnumerator));
					}
					if (_init) {
						throw new InvalidOperationException();
					}
					return _logFile.GetLog(_i);
				}
			}

			object? IEnumerator.Current => this.Current;

			internal LogDataEnumerator(LogFile logfile)
			{
				_logFile = logfile;
				this.Reset();
			}

			~LogDataEnumerator()
			{
				this.IsDisposed = true;
			}

			public bool MoveNext()
			{
				if (this.IsDisposed) {
					throw new ObjectDisposedException(nameof(LogDataEnumerator));
				}
				if (_init) {
					_init = false;
					return true;
				} else {
					++_i;
					return _i < _logFile.Count;
				}
			}

			public void Reset()
			{
				if (this.IsDisposed) {
					throw new ObjectDisposedException(nameof(LogDataEnumerator));
				}
				_init = true;
				_i    = 0UL;
			}

			public void Dispose()
			{
				this.IsDisposed = true;
				_logFile.DisposableObjects.Remove(this);
				GC.SuppressFinalize(this);
			}
		}

		#endregion
	}
}
