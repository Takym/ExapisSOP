/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.IO;
using ExapisSOP.IO;
using ExapisSOP.IO.Logging;

namespace ExapisSOP.Tools.EncodingFixer
{
	internal static class EncFixerCore
	{

#if NETCOREAPP3_1 && !(ARM || x64 || x86)

		internal static void Fix(ILogger? logger)
		{
			var path = new PathString(AppDomain.CurrentDomain.BaseDirectory ?? Environment.CurrentDirectory)
				.GetDirectoryName()?.GetDirectoryName()?.GetDirectoryName()?.GetDirectoryName();
			if (path is null) {
				logger?.Warn("Could not load the solution path.");
				return;
			}
			logger?.Info($"The solution path is: {path}");
			LoadAndConvertFiles(logger, path,
				// Git Configurations
				".gitignore", ".gitattributes", ".gitmodules",
				// Visual Studio Files
				".sln", ".csproj", ".editorconfig",
				// C# Code Files and ResX Files
				".cs", ".resx",
				// Document Files
				".txt", ".md",
				// Batch Scripts
				".cmd",
				// Data Description Language Files
				".xsd", ".json", ".yml",
				// Others
				".partial"
			);
		}

		private static void LoadAndConvertFiles(ILogger? logger, PathString path, params string[] extensions)
		{
			var entries = path.GetEntries();
			if (entries == null) {
				logger?.Warn($"The specified path \"{path}\" was not directory.");
				return;
			}
			foreach (var entry in entries) {
				logger?.Info($"Loading {entry}...");
				if (entry.IsDirectory) {
					logger?.Info($"The entry is directory.");
					LoadAndConvertFiles(logger, entry, extensions);
				} else {
					logger?.Info($"The entry is file.");
					string? ext = entry.GetExtension();
					for (int i = 0; i < extensions.Length; ++i) {
						if (ext == extensions[i]) {
							logger?.Info("Matched.");
							ConvertFile(logger, entry);
							goto next_entry;
						}
					}
					logger?.Info("Skipped.");
next_entry:;
				}
			}
		}

		private static void ConvertFile(ILogger? logger, PathString fname)
		{
			using (var fs = new FileStream(fname, FileMode.Open, FileAccess.ReadWrite, FileShare.None)) {
				byte[] bom = new byte[3];
				int    len = fs.Read(bom, 0, 3);
				if (len == 3 && bom[0] == 0xEF && bom[1] == 0xBB && bom[2] == 0xBF) {
					logger?.Info("The encoding is: UTF-8 with BOM");
				} else {
					fs.Position = 0;
					int b;
					while ((b = fs.ReadByte()) != -1) {
						if ((0x00 <= b && b <= 0x08) ||
							(0x0E <= b && b <= 0x1F) ||
							(0xFD <= b && b <= 0xFF) ||
							(b == 0x0B) || (b == 0x0C) || (b == 0x7F)) {
							logger?.Warn("This file is maybe binary or not supported encoding.");
							break;
						} else if (0xC2 <= b && b <= 0xDF) {
							b = fs.ReadByte();
							if (0x80 <= b && b <= 0xBF) {
								// UTF-8
							}
						} else if (0xE0 <= b && b <= 0xEF) {
							b = fs.ReadByte();
							if (0x80 <= b && b <= 0xBF) {
								b = fs.ReadByte();
								if (0x80 <= b && b <= 0xBF) {
									// UTF-8
								}
							}
						} else if (0xF0 <= b && b <= 0xE4) {
							b = fs.ReadByte();
							if (0x80 <= b && b <= 0xBF) {
								b = fs.ReadByte();
								if (0x80 <= b && b <= 0xBF) {
									b = fs.ReadByte();
									if (0x80 <= b && b <= 0xBF) {
										// UTF-8
									}
								}
							}
						} else if (0xA1 <= b && 0xDF <= b) {
							// Shift-JIS
						} else if ((0x81 <= b && 0x9F <= b) || (0xE0 <= b && 0xFC <= b)) {
							b = fs.ReadByte();
							if ((0x40 <= b && 0x7E <= b) || (0x80 <= b && 0xFC <= b)) {
								// Shift-JIS
							}
						}
					}
				}
			}
		}

#else

		internal static void Fix(ILogger? logger)
		{
			logger?.Info($"Cannot run on this platform: {VersionInfo.SystemType}.");
		}

#endif

	}
}
