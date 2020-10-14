/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ExapisSOP.IO.Logging
{
	internal sealed class LoggingSystemService : AppWorker, ILoggingSystemService
	{
		#region 動的

		private readonly Func<LoggingSystemServiceOptions, Task> _options;

		internal LoggingSystemService(Func<LoggingSystemServiceOptions, Task> callBackFunc)
		{
			_options = callBackFunc;
		}

		public override async Task InitializeAsync(IContext context)
		{
			await base.InitializeAsync(context);

			// Load the options
			var opt = new LoggingSystemServiceOptions();
			await _options(opt);
		}

		public override async Task FinalizeAsync(IContext context)
		{
			await base.FinalizeAsync(context);
		}

		#endregion

		#region 静的

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

		#endregion
	}
}
