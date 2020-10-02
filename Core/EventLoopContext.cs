/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System;
using ExapisSOP.IO;
using ExapisSOP.IO.Settings;
using ExapisSOP.IO.Settings.CommandLine;
using ExapisSOP.Properties;
using ExapisSOP.Utils;

namespace ExapisSOP.Core
{
	internal class EventLoopContext : DisposableBase, IContext
	{
		private  readonly DefaultHostRunner    _runner;
		internal readonly InitFinalContext     _init;
		private           EventLoopContext?    _prev;
		private           object?              _msg;
		public            IPathList?           Paths     { get; }
		public            EnvironmentSettings? Settings  { get; }
		public            Switch[]?            Arguments { get; }

		internal EventLoopContext(DefaultHostRunner runner, IContext context)
		{
			_runner = runner  ?? throw new ArgumentNullException(nameof(runner));
			context = context ?? throw new ArgumentNullException(nameof(context));
			if (context is InitFinalContext initContext) {
				_init          = initContext;
				_msg           = initContext.GetMessage();
				if (initContext?.IsFinalizationPhase() ?? false) {
					throw new InvalidOperationException(Resources.EventLoopContext_InvalidOperationException);
				}
			} else if (context is EventLoopContext prevContext) {
				_init          = prevContext._init;
				_prev          = prevContext;
				_msg           = prevContext._msg;
				// 2つ前の文脈情報を削除してメモリ節約
				prevContext._prev?.Dispose();
				prevContext._prev = null;
			} else {
				throw new ArgumentException(Resources.EventLoopContext_ArgumentException, nameof(context));
			}
			this.Paths     = context.Paths;
			this.Settings  = context.Settings;
			this.Arguments = context.Arguments;
		}

		internal EventLoopContext? GetPrev()
		{
			return _prev;
		}

		public HostRunner GetHostRunner()
		{
			if (this.IsDisposed) {
				throw new ObjectDisposedException(nameof(EventLoopContext));
			}
			return _runner;
		}

		public T? GetService<T>() where T : class, IService?
		{
			if (this.IsDisposed) {
				throw new ObjectDisposedException(nameof(EventLoopContext));
			}
			for (int i = 0; i < _runner._services.Count; ++i) {
				if (_runner._services[i] is T result) {
					return result;
				}
			}
			return null;
		}

		public object? GetMessage()
		{
			if (this.IsDisposed) {
				throw new ObjectDisposedException(nameof(EventLoopContext));
			}
			return _msg;
		}

		public void SetMessage(object data)
		{
			if (this.IsDisposed) {
				throw new ObjectDisposedException(nameof(EventLoopContext));
			}
			_msg = data;
		}

		protected override void Dispose(bool disposing)
		{
			if (!this.IsDisposed) {
				if (disposing) {
					_prev?.Dispose();
					/* // _msg の自動削除は廃止
					if (_msg is IDisposable disposable) {
						disposable.Dispose();
					}
					//*/
				}
				_prev = null;
				_msg  = null;
				base.Dispose(disposing);
			}
		}
	}
}
