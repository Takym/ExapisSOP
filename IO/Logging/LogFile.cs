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
using ExapisSOP.Utils;

namespace ExapisSOP.IO.Logging
{
	/// <summary>
	///  ログファイルを表します。
	/// </summary>
	public class LogFile : DisposableBase, ILogFile
	{
		/// <summary>
		///  このログファイルに追加されたログ情報の個数を取得します。
		/// </summary>
		public ulong Count { get; }

		/// <summary>
		///  型'<see cref="ExapisSOP.IO.Logging.LogFile"/>'の新しいインスタンスを生成します。
		/// </summary>
		public LogFile()
		{
			throw new NotImplementedException();
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
				var t = frame?.GetMethod()?.ReflectedType;
				if (t != null && !(t.Namespace?.StartsWith($"{nameof(ExapisSOP)}.{nameof(IO)}.{nameof(Logging)}") ?? true)) {
					return new Logger(t.Name, this);
				}
			}
			return new Logger(Logger.Noname, this);
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
			throw new NotImplementedException();
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
			throw new NotImplementedException();
		}

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
	}
}
