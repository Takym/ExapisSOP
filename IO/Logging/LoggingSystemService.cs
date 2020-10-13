/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System;
using System.Diagnostics;

namespace ExapisSOP.IO.Logging
{
	internal class LoggingSystemService
	{
		internal static string CreateFileName(string? tag = null)
		{
			return CreateFileName(DateTime.Now, Process.GetCurrentProcess(), tag);
		}

		internal static string CreateFileName(DateTime dt, string? tag = null)
		{
			return CreateFileName(dt, Process.GetCurrentProcess(), tag);
		}

		internal static string CreateFileName(Process proc, string? tag = null)
		{
			return CreateFileName(DateTime.Now, proc, tag);
		}

		internal static string CreateFileName(DateTime dt, Process proc, string? tag = null)
		{
			if (string.IsNullOrEmpty(tag)) {
				return $"{dt:yyyy-MM-dd_HH-mm-ss+fffffff}.[{proc.Id}].log";
			} else {
				return $"{dt:yyyy-MM-dd_HH-mm-ss+fffffff}.[{proc.Id}].{tag}.log";
			}
		}
	}
}
