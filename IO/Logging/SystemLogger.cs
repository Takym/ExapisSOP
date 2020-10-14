/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

namespace ExapisSOP.IO.Logging
{
	internal sealed class SystemLogger : Logger
	{
		internal SystemLogger(ILogFile logfile)              : base("system",      logfile) { }
		internal SystemLogger(ILogFile logfile, string name) : base("sys_" + name, logfile) { }
	}
}
