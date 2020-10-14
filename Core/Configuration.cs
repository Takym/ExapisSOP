/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System;
using System.Threading.Tasks;

namespace ExapisSOP.Core
{
	internal class Configuration : IConfiguration
	{
		private readonly DefaultHostRunner _runner;

		public event EventHandler<UnhandledErrorEventArgs>? UnhandledError;
		public event EventHandler<TerminationEventArgs>?    Terminate;

		internal Configuration(DefaultHostRunner runner)
		{
			_runner = runner;
		}

		public IConfiguration AddService(IService service)
		{
			_runner._services.Add(service);
			return this;
		}

		public IService[] GetServices()
		{
			return _runner._services.ToArray();
		}

		internal async Task<bool> OnUnhandledErrorAsync(Exception e, IContext context)
		{
			var args = new UnhandledErrorEventArgs(e, context);
			this.UnhandledError?.Invoke(this, args);
			return await Task.FromResult(args.Abort);
		}

		internal async Task<Exception?> OnTerminateAsync(TerminationException te, IContext context)
		{
			try {
				this.Terminate?.Invoke(this, new TerminationEventArgs(te, context));
				return null;
			} catch (Exception e) {
				await this.OnUnhandledErrorAsync(e, context);
				return e;
			}
		}
	}
}
