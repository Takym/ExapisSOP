/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.IO;
using System.Xml;
using ExapisSOP.IO;
using ExapisSOP.IO.Logging;

namespace ExapisSOP.Tools.PackageDependencyFixer
{
	internal sealed class TextAndXmlLogFile : LogFile
	{
		private readonly LogFile _text_file;
		private readonly LogFile? _xml_file;

		public override ulong Count { get; }

		internal TextAndXmlLogFile(Stream stream)
		{
			if (stream == null) {
				throw new ArgumentNullException(nameof(stream));
			}
			_text_file = CreateTextFile(stream);
			if (stream is FileStream fs) {
				_xml_file = CreateXmlFile(XmlWriter.Create(((PathString)(fs.Name)).ChangeExtension("xml")));
			}
		}

		protected override void AddLogCore(LogData data)
		{
			_text_file.AddLog(data);
			_xml_file?.AddLog(data);
		}

		protected override LogData GetLogCore(ulong index)
		{
			return _text_file.GetLog(index);
		}

		protected override void Dispose(bool disposing)
		{
			if (!this.IsDisposed) {
				if (disposing) {
					_text_file.Dispose();
					_xml_file?.Dispose();
				}
				base.Dispose(disposing);
			}
		}
	}
}
