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
		private           bool              _ready_to_run;

		public DefaultHostRunner(string[] cmdline) : base(cmdline)
		{
			_services     = new List<IService>();
			_app_workers  = new List<AppWorker>();
			_config       = new Configuration(this);
			_ready_to_run = false;
		}

		public override HostRunner Build()
		{
			_ready_to_run = true;
			return base.Build();
		}

		public override async Task<int> RunAsync()
		{
			if (_ready_to_run) {
				try {
					int ret = 0;
					await (this.ConfigureCallBackFunc?.Invoke(_config) ?? Task.CompletedTask);
					try {
						await this.InitializationPhase();
						_initContext!.ToString(); // nullチェック // 確実に通る
						ret = await this.MainEventLoopPhase();
					} finally {
						await this.FinalizationPhase();
					}
					return ret;
				} catch (Exception e) {
					throw new Exception(Resources.DefaultHostRunner_Exception, e);
				}
			} else {
				return -1;
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
				try {
					context = new EventLoopContext(this, _initContext!);
					for (int i = 0; i < _app_workers.Count; ++i) {
						var task = Task.CompletedTask;
						try {
							await (task = _app_workers[i].OnStartupAsync(context));
						} catch (TerminationException te) {
							loop = false;
							var e1 = await _app_workers[i].OnTerminateAsync(te);
							var e2 = await _config        .OnTerminateAsync(te);
							if (te.HResult != 0) ret = te.HResult;
							if (e1 != null) ret = e1.HResult;
							if (e2 != null) ret = e2.HResult;
						} catch (Exception e) {
							var error = task.Exception ?? e;
							bool aw   = await _app_workers[i].OnUnhandledErrorAsync(error);
							bool cfg  = await _config        .OnUnhandledErrorAsync(error);
							if (aw || cfg) {
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
								await (task = _app_workers[i].OnUpdateAsync(context));
							} catch (TerminationException te) {
								loop = false;
								var e1 = await _app_workers[i].OnTerminateAsync(te, context);
								var e2 = await _config        .OnTerminateAsync(te, context);
								if (te.HResult != 0) ret = te.HResult;
								if (e1 != null) ret = e1.HResult;
								if (e2 != null) ret = e2.HResult;
								break;
							} catch (Exception e) {
								var error = task.Exception ?? e;
								bool aw   = await _app_workers[i].OnUnhandledErrorAsync(error, context);
								bool cfg  = await _config        .OnUnhandledErrorAsync(error, context);
								if (aw || cfg) {
									ret  = e.HResult;
									loop = false;
									if (ret == 0) ret = -1;
									break;
								}
							}
						}
					}
				} finally {
					context = new EventLoopContext(this, ((IContext?)(context)) ?? _initContext!);
					for (int i = _app_workers.Count - 1; i >= 0; --i) {
						var task = Task.CompletedTask;
						try {
							await (task = _app_workers[i].OnShutdownAsync(context));
						} catch (TerminationException te) {
							await _app_workers[i].OnTerminateAsync(te);
							await _config        .OnTerminateAsync(te);
							var e1 = await _app_workers[i].OnTerminateAsync(te);
							var e2 = await _config        .OnTerminateAsync(te);
							if (te.HResult != 0) ret = te.HResult;
							if (e1 != null) ret = e1.HResult;
							if (e2 != null) ret = e2.HResult;
						} catch (Exception e) {
							var error = task.Exception ?? e;
							bool aw   = await _app_workers[i].OnUnhandledErrorAsync(error);
							bool cfg  = await _config        .OnUnhandledErrorAsync(error);
							if (aw || cfg) {
								ret = e.HResult;
								if (ret == 0) ret = -1;
								break;
							}
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
