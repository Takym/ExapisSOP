/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.IO;
using System.Text;
using ExapisSOP.IO;
using ExapisSOP.IO.Logging;
using ExapisSOP.Utils;

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
			logger?.Info($"Ready to run.");
			ConsoleUtil.Pause();
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
					logger?.Info($"The entry is a directory.");
					LoadAndConvertFiles(logger, entry, extensions);
				} else {
					logger?.Info($"The entry is a file.");
					string? ext = entry.GetExtension();
					for (int i = 0; i < extensions.Length; ++i) {
						if (ext == extensions[i]) {
							logger?.Info("Matched.");
							DetectAndConvertFileEncoding(logger, entry);
							goto next_entry;
						}
					}
					logger?.Info("Skipped.");
next_entry:;
				}
			}
		}

		private static void DetectAndConvertFileEncoding(ILogger? logger, PathString fname)
		{
			using (var fs = new FileStream(fname, FileMode.Open, FileAccess.ReadWrite, FileShare.None)) {
				byte[] bom = new byte[3];
				int    len = fs.Read(bom, 0, 3);
				if (len == 3 && bom[0] == 0xEF && bom[1] == 0xBB && bom[2] == 0xBF) {
					logger?.Info("The encoding is: UTF-8 with BOM");
					ConvertFileEncoding(logger, fs, Encoding.UTF8);
				} else {
					fs.Position = 0;
					int b;
					int scoreSJIS = 0, scoreUTF8 = 0;
					while ((b = fs.ReadByte()) != -1) {
						if ((0x00 <= b && b <= 0x08) ||
							(0x0E <= b && b <= 0x1F) ||
							(0xFD <= b && b <= 0xFF) ||
							(b == 0x0B) || (b == 0x0C) || (b == 0x7F)) {
							logger?.Warn("This file is maybe binary or not supported encoding.");
							logger?.Info("Skipped.");
							return;
						} else if (0xA1 <= b && b <= 0xDF) {
							if (0xC2 <= b && b <= 0xDF) {
								b = fs.ReadByte();
								if (0x80 <= b && b <= 0xBF) {
									++scoreUTF8;
								} else {
									--scoreUTF8;
									fs.Position -= 1;
								}
							} else {
								++scoreSJIS;
							}
						} else if (0x81 <= b && b <= 0x9F) {
							b = fs.ReadByte();
							if ((0x40 <= b && b <= 0x7E) || (0x80 <= b && b <= 0xFC)) {
								++scoreSJIS;
							} else {
								--scoreSJIS;
							}
						} else if (0xE0 <= b && b <= 0xFC) {
							if (0xE0 <= b && b <= 0xEF) {
								b = fs.ReadByte();
								if (0x80 <= b && b <= 0xBF) {
									b = fs.ReadByte();
									if (0x80 <= b && b <= 0xBF) {
										scoreUTF8 += 2;
									} else {
										scoreUTF8 -= 2;
										fs.Position -= 2;
									}
								} else {
									scoreUTF8 -= 1;
									fs.Position -= 1;
								}
							} else if (0xF0 <= b && b <= 0xF4) {
								b = fs.ReadByte();
								if (0x80 <= b && b <= 0xBF) {
									b = fs.ReadByte();
									if (0x80 <= b && b <= 0xBF) {
										b = fs.ReadByte();
										if (0x80 <= b && b <= 0xBF) {
											scoreUTF8 += 2;
										} else {
											scoreUTF8 -= 3;
											fs.Position -= 3;
										}
									} else {
										scoreUTF8 -= 2;
										fs.Position -= 2;
									}
								} else {
									scoreUTF8 -= 1;
									fs.Position -= 1;
								}
							} else {
								b = fs.ReadByte();
								if ((0x40 <= b && b <= 0x7E) || (0x80 <= b && b <= 0xFC)) {
									++scoreSJIS;
								} else {
									--scoreSJIS;
								}
							}
						}
					}
					if (scoreUTF8 < scoreSJIS) {
						logger?.Info("The encoding is: Shift-JIS");
						ConvertFileEncoding(logger, fs, Encoding.GetEncoding(932));
					} else if (scoreUTF8 == scoreSJIS) {
						logger?.Warn("Could not detect the encoding.");
						logger?.Info("Skipped.");
					} else {
						logger?.Info("The encoding is: UTF-8");
						logger?.Info("Skipped.");
					}
				}
			}
		}

		private static void ConvertFileEncoding(ILogger? logger, FileStream fs, Encoding enc)
		{
			try {
				fs.Position = 0;
				string text;
				using (var sr = new StreamReader(fs, enc, true, -1, true)) {
					text = sr.ReadToEnd();
				}
				fs.SetLength(0);
				using (var sw = new StreamWriter(fs, new UTF8Encoding(false), -1, true)) {
					sw.Write(text);
				}
				logger?.Info("Converted successfully.");
			} catch (Exception e) {
				logger?.Exception(e);
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
