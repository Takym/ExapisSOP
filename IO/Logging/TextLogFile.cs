/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System;
using System.Collections.Generic;
using System.IO;

namespace ExapisSOP.IO.Logging
{
	internal class TextLogFile : LogFile
	{
		private readonly List<LogData> _logs;
		private readonly TextWriter    _tw;
		private          int           _a;

		public override ulong Count => unchecked((ulong)(_logs.Count));

		internal TextLogFile(TextWriter tw)
		{
			if (tw == null) {
				throw new ArgumentNullException(nameof(tw));
			}
			_logs = new List<LogData>();
			_tw   = TextWriter.Synchronized(tw);
			_a    = 0;
		}

		protected override void AddLogCore(LogData data)
		{
			lock (_logs) {
				_logs.Add(data);
			}
			_tw.WriteLine(data.ToString());
			++_a;
			if (_a > 0x10) {
				_tw.Flush();
				_a = 0;
			}
		}

		protected override LogData GetLogCore(ulong index)
		{
			return _logs[unchecked((int)(index))];
		}

		protected override void Dispose(bool disposing)
		{
			if (!this.IsDisposed) {
				if (disposing) {
					_tw.Flush();
					_tw.Close();
				}
				base.Dispose(disposing);
			}
		}
	}
}
