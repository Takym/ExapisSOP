/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System.Collections;
using System.Collections.Generic;

namespace ExapisSOP.IO.Logging
{
	internal sealed class EmptyLogFile : ILogFile
	{
		internal static readonly EmptyLogFile Instance = new EmptyLogFile();
		private         readonly LogData      _emptyLogData;
		public                   ulong        Count    => 0;

		private EmptyLogFile()
		{
			_emptyLogData = new LogData(LogLevel.None, EmptyLogger.Instance, "<<<This is an empty log data created by the empty log file.>>>");
		}

		public ILogger CreateLogger()
		{
			return EmptyLogger.Instance;
		}

		public ILogger GetConsoleLogger()
		{
			return EmptyLogger.Instance;
		}

		public void AddLog(LogData data)
		{
			// do nothing
		}

		public LogData GetLog(ulong index)
		{
			return _emptyLogData;
		}

		public IEnumerator<LogData> GetEnumerator()
		{
			return EmptyLogDataEnumerator.Instance;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return EmptyLogDataEnumerator.Instance;
		}

		#region 子クラス

		private sealed class EmptyLogger : ILogger
		{
			internal static readonly EmptyLogger Instance = new EmptyLogger();

			public string   Name    => string.Empty;
			public ILogFile LogFile => EmptyLogFile.Instance;

			private EmptyLogger() { }

			public void Trace(string message, ILoggable? additionalData = null) { /* do nothing */ }
			public void Debug(string message, ILoggable? additionalData = null) { /* do nothing */ }
			public void Info (string message, ILoggable? additionalData = null) { /* do nothing */ }
			public void Warn (string message, ILoggable? additionalData = null) { /* do nothing */ }
			public void Error(string message, ILoggable? additionalData = null) { /* do nothing */ }
			public void Fatal(string message, ILoggable? additionalData = null) { /* do nothing */ }
		}

		private sealed class EmptyLogDataEnumerator : IEnumerator<LogData>
		{
			internal static readonly EmptyLogDataEnumerator Instance = new EmptyLogDataEnumerator();

			public LogData Current
			{
				get
				{
					return EmptyLogFile.Instance._emptyLogData;
				}
			}

			object? IEnumerator.Current => this.Current;

			private EmptyLogDataEnumerator() { }

			public bool MoveNext()
			{
				return false;
			}

			public void Reset()
			{
				// do nothing
			}

			public void Dispose()
			{
				// do nothing
			}
		}

		#endregion
	}
}
