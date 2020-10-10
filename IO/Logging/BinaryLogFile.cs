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
	internal class BinaryLogFile : LogFile
	{ // 0x666C627800FF7F80
		private readonly BinaryWriter _bw;

		public override ulong Count => 0;

		internal BinaryLogFile(BinaryWriter bw)
		{
			if (bw == null) {
				throw new ArgumentNullException(nameof(bw));
			}
			_bw = BinaryWriter.Synchronized(bw);
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
					_bw.Flush();
					_bw.Close();
				}
				base.Dispose(disposing);
			}
		}
	}
}
