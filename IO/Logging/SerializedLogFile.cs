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
	internal class SerializedLogFile : LogFile
	{
		private readonly List<LogData> _logs;
		private readonly Stream        _stream;

		public override ulong Count => unchecked((ulong)(_logs.Count));

		internal SerializedLogFile(Stream stream)
		{
			if (stream == null) {
				throw new ArgumentNullException(nameof(stream));
			}
			if (stream.Length == 0) {
				_logs = new List<LogData>();
			} else {
				_logs = new List<LogData>(VersionInfo._bf.Deserialize(stream) as LogData[] ?? Array.Empty<LogData>());
			}
			_stream = stream;
		}

		protected override void AddLogCore(LogData data)
		{
			lock (_logs) {
				_logs.Add(data);
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
					VersionInfo._bf.Serialize(_stream, _logs.ToArray());
					_stream.Flush();
					_stream.Close();
				}
				base.Dispose(disposing);
			}
		}
	}
}
