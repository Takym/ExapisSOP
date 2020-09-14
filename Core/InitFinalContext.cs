/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System;
using ExapisSOP.Properties;

namespace ExapisSOP.Core
{
	internal class InitFinalContext : IContext
	{
		private readonly DefaultHostRunner _runner;
		private readonly InitFinalContext? _initContext;
		private          object?           _msg;

		internal InitFinalContext(DefaultHostRunner runner, InitFinalContext? initContext)
		{
			_runner      = runner;
			_initContext = initContext;
			if (initContext?.IsFinalizationPhase() ?? false) {
				throw new InvalidOperationException(Resources.InitFinalContext_InvalidOperationException);
			}
		}

		internal bool IsInitializationPhase()
		{
			return _initContext != null;
		}

		internal bool IsFinalizationPhase()
		{
			return _initContext == null;
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
