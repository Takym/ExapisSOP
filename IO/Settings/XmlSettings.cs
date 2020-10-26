/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Text;
using System.Xml;

namespace ExapisSOP.IO.Settings
{
	internal static class XmlSettings
	{
		internal readonly static Encoding         Encoding = new UTF8Encoding(false, false);
		internal readonly static Encoding ReadableEncoding = new UTF8Encoding(true,  false);

		internal readonly static XmlReaderSettings Reader = new XmlReaderSettings() {
			IgnoreWhitespace = true,
			IgnoreComments   = true,
		};

		internal readonly static XmlWriterSettings Writer = new XmlWriterSettings() {
			NewLineChars = string.Empty,
			Indent       = false,
			IndentChars  = string.Empty
		};

		internal readonly static XmlWriterSettings ReadableWriter = new XmlWriterSettings() {
			NewLineChars = Environment.NewLine,
			Indent       = true,
			IndentChars  = "\t"
		};
	}
}
