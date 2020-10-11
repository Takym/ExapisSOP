/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System;
using System.IO;
using System.Text;

namespace ExapisSOP.IO.Logging
{
	internal partial class BinaryLogFile : LogFile
	{
		private readonly Stream       _stream;
		private readonly BinaryReader _br;
		private readonly BinaryWriter _bw;

		public override ulong Count => this.CountLogs();

		internal BinaryLogFile(Stream stream)
		{
			if (stream == null) {
				throw new ArgumentNullException(nameof(stream));
			}
			_stream = Stream.Synchronized(stream);
			_br     = new BinaryReader(stream, Encoding.UTF8);
			_bw     = new BinaryWriter(stream, Encoding.UTF8);
			this.Init();
		}

		protected override void AddLogCore(LogData data)
		{
		}

		protected override LogData GetLogCore(ulong index)
		{
			return null!;
		}

		protected override void Dispose(bool disposing)
		{
			if (!this.IsDisposed) {
				if (disposing) {
					_stream.Flush();
					_stream.Dispose();
				}
				base.Dispose(disposing);
			}
		}
	}
}
