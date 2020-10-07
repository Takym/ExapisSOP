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
		internal const string Prefix = "system";

		internal SystemLogger(ILogFile logfile)              : base(Prefix,              logfile) { }
		internal SystemLogger(ILogFile logfile, string name) : base(Prefix + "_" + name, logfile) { }
	}
}
