/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System;
using System.Reflection;

namespace ExapisSOP.IO.Logging
{
	/// <summary>
	///  ログ出力処理を割り込ませる様に指示します。
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public sealed class InjectLoggerAttribute : Attribute
	{
		internal void PreAction(ILogger logger, MethodBase method)
		{
			logger.Trace($"{nameof(this.PreAction)} of \'{method.Name}\' at \'{method.ReflectedType?.AssemblyQualifiedName}\'");
		}

		internal void PostAction(ILogger logger, MethodBase method)
		{
			logger.Trace($"{nameof(this.PostAction)} of \'{method.Name}\' at \'{method.ReflectedType?.AssemblyQualifiedName}\'");
		}
	}
}
