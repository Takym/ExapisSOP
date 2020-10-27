/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 *
 * distributed under the MIT License.
****/

using System;
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
			LoadFiles(logger, path, "cs");
		}

		private static void LoadFiles(ILogger? logger, PathString path, params string[] extensions)
		{
			//
		}

#else

		internal static void Fix(ILogger? logger)
		{
			logger?.Info($"Cannot run on this platform: {VersionInfo.SystemType}.");
		}

#endif

	}
}
