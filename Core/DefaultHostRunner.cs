/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExapisSOP.IO.Logging;
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
		private           ILogger?          _logger;
		private           bool              _loggerEnabled;
		private           bool              _logOnUpdate;

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
				await this.InitializeLogger();
				try {
					await this.PreStartup();
					context = new EventLoopContext(this, _initContext!);
					(ret, loop) = await this.RunEvent(context, ret, true, false, worker => worker.OnStartupAsync);
					await this.PostRunEvent(false);
					while (loop) {
						await this.PreUpdate();
						context = new EventLoopContext(this, context);
						(ret, loop) = await this.RunEvent(context, ret, true, true, worker => worker.OnUpdateAsync);
						await this.PostRunEvent(true);
					}
				} finally {
					await this.PreShutdown();
					context = new EventLoopContext(this, ((IContext?)(context)) ?? _initContext!);
					(ret, loop) = await this.RunEvent(context, ret, false, false, worker => worker.OnShutdownAsync);
					await this.PostRunEvent(false);
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

		private async Task<(int ret, bool continuous)> RunEvent(
			IContext context, int ret, bool forward, bool abortWhenTermination, Func<AppWorker, Func<IContext, Task>> eventFunc)
		{
			int  i          = forward ? 0 : _app_workers.Count - 1;
			bool continuous = true;
			while (0 <= i && i < _app_workers.Count) {
				var task = Task.CompletedTask;
				try {
					// abortWhenTermination is equal to updating
					await this.PreAppWorker(_app_workers[i], abortWhenTermination);
					await (task = eventFunc(_app_workers[i])(context));
					await this.PostAppWorker(_app_workers[i], abortWhenTermination);
				} catch (TerminationException te) {
					await this.PreHandleTermination();
					ret = await this.HandleTermination(_app_workers[i], te, context, ret);
					continuous = false;
					if (abortWhenTermination) {
						await this.PostHandleTermination(te);
						return (ret, continuous);
					}
				} catch (Exception e) {
					await this.PreHandleException();
					if (await this.HandleException(_app_workers[i], task.Exception ?? e, context)) {
						await this.PostHandleException(task.Exception, e);
						return (this.GetNonZeroHResult(e), false);
					}
				}
				if (forward) ++i; else --i;
			}
			return (ret, continuous);
		}

		private async Task<int> HandleTermination(AppWorker appWorker, TerminationException te, IContext context, int ret)
		{
			var e1 = await appWorker.OnTerminateAsync(te, context);
			var e2 = await _config  .OnTerminateAsync(te, context);
			if (e2 != null) return e2.HResult;
			if (e1 != null) return e1.HResult;
			if (te.HResult != 0) return te.HResult;
			return ret;
		}

		private async Task<bool> HandleException(AppWorker appWorker, Exception error, IContext context)
		{
			bool aw   = await appWorker.OnUnhandledErrorAsync(error, context);
			bool cfg  = await _config  .OnUnhandledErrorAsync(error, context);
			if (aw || cfg) {
				return true;
			} else {
				return false;
			}
		}

		private int GetNonZeroHResult(Exception e)
		{
			return e.HResult == 0 ? -1 : e.HResult;
		}

		private Task InitializeLogger()
		{
			if (_initContext?.LogFile == null) {
				_logger        = EmptyLogFile.Instance.CreateLogger();
				_loggerEnabled = false;
			} else {
				_logger        = new SystemLogger(_initContext.LogFile, "EVL");
				_loggerEnabled = true;
				_logOnUpdate   = (_initContext?.GetLoggingSystem() as LoggingSystemService)?.LogOnUpdate ?? false;
			}
			return Task.CompletedTask;
		}

		private Task PreStartup()
		{
			if (_loggerEnabled && _logger != null) {
				_logger.Trace($"executing {nameof(this.RunEvent)}...");
				_logger.Debug($"now invoking the startup events");
			}
			return Task.CompletedTask;
		}

		private Task PreUpdate()
		{
			if (_loggerEnabled && _logger != null && _logOnUpdate) {
				_logger.Trace($"executing {nameof(this.RunEvent)}...");
				_logger.Debug($"now invoking the update events");
			}
			return Task.CompletedTask;
		}

		private Task PreShutdown()
		{
			if (_loggerEnabled && _logger != null) {
				_logger.Trace($"executing {nameof(this.RunEvent)}...");
				_logger.Debug($"now invoking the shutdown events");
			}
			return Task.CompletedTask;
		}

		private Task PostRunEvent(bool updating)
		{
			if (_loggerEnabled && _logger != null && (!updating || _logOnUpdate)) {
				_logger.Trace($"completed {nameof(this.RunEvent)}");
			}
			return Task.CompletedTask;
		}

		private Task PreAppWorker(AppWorker appWorker, bool updating)
		{
			if (_loggerEnabled && _logger != null && (!updating || _logOnUpdate)) {
				_logger.Trace($"executing {appWorker.GetType().FullName}...");
			}
			return Task.CompletedTask;
		}

		private Task PostAppWorker(AppWorker appWorker, bool updating)
		{
			if (_loggerEnabled && _logger != null && (!updating || _logOnUpdate)) {
				_logger.Trace($"completed {appWorker.GetType().FullName} successfully");
			}
			return Task.CompletedTask;
		}

		private Task PreHandleTermination()
		{
			if (_loggerEnabled && _logger != null) {
				_logger.Trace("handling termination...");
			}
			return Task.CompletedTask;
		}

		private Task PostHandleTermination(TerminationException te)
		{
			if (_loggerEnabled && _logger != null) {
				_logger.Info("broke the loop", new ExceptionRecord(te));
			}
			return Task.CompletedTask;
		}

		private Task PreHandleException()
		{
			if (_loggerEnabled && _logger != null) {
				_logger.Trace("handling exception...");
			}
			return Task.CompletedTask;
		}

		private Task PostHandleException(AggregateException? e1, Exception e2)
		{
			if (_loggerEnabled && _logger != null) {
				_logger.UnhandledException(e1!, true);
				_logger.UnhandledException(e2,  true);
			}
			return Task.CompletedTask;
		}
	}
}
