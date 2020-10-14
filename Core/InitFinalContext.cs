/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System;
using ExapisSOP.IO;
using ExapisSOP.IO.Logging;
using ExapisSOP.IO.Settings;
using ExapisSOP.IO.Settings.CommandLine;
using ExapisSOP.Properties;

namespace ExapisSOP.Core
{
	internal class InitFinalContext : IContext
	{
		private readonly DefaultHostRunner    _runner;
		private readonly InitFinalContext?    _initContext;
		private          object?              _msg;
		public           IPathList?           Paths     { get; internal set; }
		public           EnvironmentSettings? Settings  { get; internal set; }
		public           Switch[]?            Arguments { get; internal set; }
		public           ILogFile?            LogFile   { get; internal set; }

		internal InitFinalContext(DefaultHostRunner runner, InitFinalContext? initContext)
		{
			_runner        = runner;
			_initContext   = initContext;
			_msg           = initContext?._msg;
			this.Paths     = initContext?.Paths;
			this.Settings  = initContext?.Settings;
			this.Arguments = initContext?.Arguments;
			this.LogFile   = initContext?.LogFile;
			if (initContext?.IsFinalizationPhase() ?? false) {
				throw new InvalidOperationException(Resources.InitFinalContext_InvalidOperationException);
			}
		}

		internal bool IsInitializationPhase()
		{
			return _initContext == null;
		}

		internal bool IsFinalizationPhase()
		{
			return _initContext != null;
		}

		internal InitFinalContext GetInitContext()
		{
			return _initContext ?? this;
		}

		public HostRunner GetHostRunner()
		{
			return _runner;
		}

		public T? GetService<T>() where T : class, IService?
		{
			for (int i = 0; i < _runner._services.Count; ++i) {
				if (_runner._services[i] is T result) {
					return result;
				}
			}
			return null;
		}

		public object? GetMessage()
		{
			return _msg;
		}

		public void SetMessage(object data)
		{
			_msg = data;
		}
	}
}
