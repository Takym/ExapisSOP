/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System;
using ExapisSOP.IO;
using ExapisSOP.Properties;

namespace ExapisSOP.Core
{
	internal class EventLoopContext : IContext, IDisposable
	{
		private  readonly DefaultHostRunner _runner;
		internal readonly InitFinalContext  _init;
		private           EventLoopContext? _prev;
		private           object?           _msg;
		public            IPathList?        Paths      { get; internal set; }
		internal          bool              IsDisposed { get; private  set; }

		internal EventLoopContext(DefaultHostRunner runner, InitFinalContext initContext)
		{
			_runner    = runner;
			_init      = initContext;
			_msg       = initContext.GetMessage();
			this.Paths = initContext.Paths;
			if (initContext?.IsFinalizationPhase() ?? false) {
				throw new InvalidOperationException(Resources.EventLoopContext_InvalidOperationException);
			}
			this.IsDisposed = false;
		}

		internal EventLoopContext(DefaultHostRunner runner, EventLoopContext prevContext)
		{
			_runner         = runner;
			_init           = prevContext._init;
			_prev           = prevContext;
			_msg            = prevContext._msg;
			this.IsDisposed = false;

			// 2つ前の文脈情報を削除してメモリ節約
			prevContext._prev?.Dispose();
			prevContext._prev = null;
		}

		~EventLoopContext()
		{
			this.Dispose(false);
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

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (!this.IsDisposed) {
				if (disposing) {
					_prev?.Dispose();
					if (_msg is IDisposable disposable) {
						disposable.Dispose();
					}
				}
				_prev = null;
				_msg  = null;
				this.IsDisposed = true;
			}
		}
	}
}
