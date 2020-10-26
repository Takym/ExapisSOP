/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Collections.Generic;
using System.Xml;

namespace ExapisSOP.IO.Logging
{
	internal class XmlLogFile : LogFile
	{
		private readonly List<LogData> _logs;
		private readonly XmlWriter     _xw;

		public override ulong Count => unchecked((ulong)(_logs.Count));

		internal XmlLogFile(XmlReader xr, XmlWriter xw) : this(xw)
		{
			if (xr == null) {
				throw new ArgumentNullException(nameof(xr));
			}
			while (xr.Name == "log") {
				var data = new LogData(xr);
				_logs.Add(data);
				data.GetObjectXML(_xw);
			}
		}

		internal XmlLogFile(XmlWriter xw)
		{
			if (xw == null) {
				throw new ArgumentNullException(nameof(xw));
			}
			_logs = new List<LogData>();
			_xw   = xw;
			_xw.WriteStartElement("logs");
		}

		protected override void AddLogCore(LogData data)
		{
			lock (_logs) {
				_logs.Add(data);
			}
			lock (_xw) {
				data.GetObjectXML(_xw);
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
					_xw.WriteEndElement();
					_xw.Flush();
					_xw.Close();
				}
				base.Dispose(disposing);
			}
		}
	}
}
