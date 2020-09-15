﻿/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System;
using System.Threading.Tasks;

namespace ExapisSOP
{
	/// <summary>
	///  アプリケーションに必要な機能を提供します。
	///  このクラスは抽象クラスです。
	/// </summary>
	public abstract class AppWorker : IService
	{
		/// <summary>
		///  プログラム開始時に<see cref="ExapisSOP.AppWorker.InitializeAsync(IContext)"/>より後に呼び出されます。
		/// </summary>
		public event EventHandler<ContextEventArgs>? Startup;

		/// <summary>
		///  メインイベントループ内から更新時に呼び出されます。
		/// </summary>
		public event EventHandler<ContextEventArgs>? Update;

		/// <summary>
		///  プログラム終了時に<see cref="ExapisSOP.AppWorker.FinalizeAsync(IContext)"/>より前に呼び出されます。
		/// </summary>
		public event EventHandler<ContextEventArgs>? Shutdown;

		/// <summary>
		///  処理されない例外が発生した場合に呼び出されます。
		/// </summary>
		public event EventHandler<UnhandledErrorEventArgs>? UnhandledError;

		/// <summary>
		///  型'<see cref="ExapisSOP.AppWorker"/>'の新しいインスタンスを生成します。
		/// </summary>
		protected AppWorker() { }

		/// <summary>
		///  オーバーライドされた場合、非同期でサービスを初期化します。
		/// </summary>
		/// <param name="context">文脈情報です。</param>
		/// <returns>サービスの初期化処理を格納した非同期操作です。</returns>
		public virtual async Task InitializeAsync(IContext context)
		{
			await Task.CompletedTask;
		}

		/// <summary>
		///  オーバーライドされた場合、非同期でサービスを破棄します。
		/// </summary>
		/// <param name="context">文脈情報です。</param>
		/// <returns>サービスの破棄処理を格納した非同期操作です。</returns>
		public virtual async Task FinalizeAsync(IContext context)
		{
			await Task.CompletedTask;
		}

		/// <summary>
		///  <see cref="ExapisSOP.AppWorker.Startup"/>イベントを発生させます。
		/// </summary>
		/// <param name="e">文脈情報を格納しているイベントデータです。</param>
		protected virtual void OnStartup(ContextEventArgs e)
		{
			this.Startup?.Invoke(this, e);
		}

		/// <summary>
		///  <see cref="ExapisSOP.AppWorker.Update"/>イベントを発生させます。
		/// </summary>
		/// <param name="e">文脈情報を格納しているイベントデータです。</param>
		protected virtual void OnUpdate(ContextEventArgs e)
		{
			this.Update?.Invoke(this, e);
		}

		/// <summary>
		///  <see cref="ExapisSOP.AppWorker.Shutdown"/>イベントを発生させます。
		/// </summary>
		/// <param name="e">文脈情報を格納しているイベントデータです。</param>
		protected virtual void OnShutdown(ContextEventArgs e)
		{
			this.Shutdown?.Invoke(this, e);
		}

		/// <summary>
		///  <see cref="ExapisSOP.AppWorker.UnhandledError"/>イベントを発生させます。
		/// </summary>
		/// <param name="e">未処理の例外オブジェクトを格納しているイベントデータです。</param>
		protected virtual void OnUnhandledError(UnhandledErrorEventArgs e)
		{
			this.UnhandledError?.Invoke(this, e);
		}

		internal async Task OnStartup(IContext context)
		{
			try {
				this.OnStartup(new ContextEventArgs(context));
				await Task.CompletedTask;
			} catch (TerminationException e) {
				await Task.FromException(e);
			}
		}

		internal async Task OnUpdate(IContext context)
		{
			try {
				this.OnUpdate(new ContextEventArgs(context));
				await Task.CompletedTask;
			} catch (TerminationException e) {
				await Task.FromException(e);
			}
		}

		internal async Task OnShutdown(IContext context)
		{
			try {
				this.OnShutdown(new ContextEventArgs(context));
				await Task.CompletedTask;
			} catch (TerminationException e) {
				await Task.FromException(e);
			}
		}

		internal async Task<bool> OnUnhandledError(Exception e)
		{
			var args = new UnhandledErrorEventArgs(e);
			this.OnUnhandledError(args);
			return await Task.FromResult(args.Abort);
		}
	}
}
