/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExapisSOP.Properties;

namespace ExapisSOP.Core
{
	internal sealed class DefaultHostRunner : HostRunner
	{
		internal readonly List<IService>    _services;
		private  readonly List<AppWorker>   _app_workers;
		private  readonly Configuration     _config;
		private           InitFinalContext? _initContext;

		public DefaultHostRunner(string[] cmdline) : base(cmdline)
		{
			_services    = new List<IService>();
			_app_workers = new List<AppWorker>();
			_config      = new Configuration(this);
		}

		public override async Task<int> RunAsync()
		{
			try {
				await (this.ConfigureCallBackFunc?.Invoke(_config) ?? Task.CompletedTask);
				await this.InitializationPhase();
				_initContext!.ToString(); // nullチェック // 確実に通る
				int ret = await this.MainEventLoopPhase();
				await this.FinalizationPhase();
				return ret;
			} catch (Exception e) {
				throw new Exception(Resources.DefaultHostRunner_Exception, e);
			}
		}

		private async Task InitializationPhase()
		{
			_initContext = new InitFinalContext(this, null);
			for (int i = 0; i < _services.Count; ++i) {
				await _services[i].InitializeAsync(_initContext);
				if (_services[i] is AppWorker appWorker) {
					_app_workers.Add(appWorker);
				}
			}
		}

		private async Task<int> MainEventLoopPhase()
		{
			if (_app_workers.Count == 0) return 0;
			EventLoopContext? context = null;
			try {
				int  ret  = 0;
				bool loop = true;
				context = new EventLoopContext(this, _initContext!);
				for (int i = 0; i < _app_workers.Count; ++i) {
					var task = Task.CompletedTask;
					try {
						await (task = _app_workers[i].OnStartup(context));
					} catch (TerminationException) {
						loop = false;
					} catch (Exception e) {
						if (await _app_workers[i].OnUnhandledError(task.Exception ?? e)) {
							ret = e.HResult;
							if (ret == 0) ret = -1;
							break;
						}
					}
				}
				while (loop) {
					context = new EventLoopContext(this, context);
					for (int i = 0; i < _app_workers.Count; ++i) {
						var task = Task.CompletedTask;
						try {
							await (task = _app_workers[i].OnUpdate(context));
						} catch (TerminationException) {
							loop = false;
							break;
						} catch (Exception e) {
							if (await _app_workers[i].OnUnhandledError(task.Exception ?? e)) {
								ret  = e.HResult;
								loop = false;
								if (ret == 0) ret = -1;
								break;
							}
						}
					}
				}
				context = new EventLoopContext(this, context);
				for (int i = _app_workers.Count - 1; i >= 0; --i) {
					var task = Task.CompletedTask;
					try {
						await (task = _app_workers[i].OnShutdown(context));
					} catch (TerminationException) {
						// do nothing, ignore
					} catch (Exception e) {
						if (await _app_workers[i].OnUnhandledError(task.Exception ?? e)) {
							ret = e.HResult;
							if (ret == 0) ret = -1;
							break;
						}
					}
				}
				return ret;
			} finally {
				context?.Dispose();
			}
		}

		private async Task FinalizationPhase()
		{
			var context = new InitFinalContext(this, _initContext);
			for (int i = _services.Count - 1; i >= 0; --i) {
				await _services[i].FinalizeAsync(context);
			}
		}
	}
}
