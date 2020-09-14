/****
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
		///  メインイベントループ内から呼び出されます。
		/// </summary>
		public event EventHandler<EventLoopEventArgs>? MainLoop;

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
		/// <param name="context">コンテキスト情報です。</param>
		/// <returns>サービスの初期化処理を格納した非同期操作です。</returns>
		public virtual async Task InitializeAsync(IContext context)
		{
			await Task.CompletedTask;
		}

		/// <summary>
		///  オーバーライドされた場合、非同期でサービスを破棄します。
		/// </summary>
		/// <param name="context">コンテキスト情報です。</param>
		/// <returns>サービスの破棄処理を格納した非同期操作です。</returns>
		public virtual async Task FinalizeAsync(IContext context)
		{
			await Task.CompletedTask;
		}

		/// <summary>
		///  <see cref="ExapisSOP.AppWorker.MainLoop"/>イベントを発生させます。
		/// </summary>
		/// <param name="e">コンテキスト情報を格納しているイベントデータです。</param>
		protected virtual void OnMainLoop(EventLoopEventArgs e)
		{
			this.MainLoop?.Invoke(this, e);
		}

		/// <summary>
		///  <see cref="ExapisSOP.AppWorker.UnhandledError"/>イベントを発生させます。
		/// </summary>
		/// <param name="e">未処理の例外オブジェクトを格納しているイベントデータです。</param>
		protected virtual void OnUnhandledError(UnhandledErrorEventArgs e)
		{
			this.UnhandledError?.Invoke(this, e);
		}

		internal async Task OnMainLoop(IContext context)
		{
			try {
				this.OnMainLoop(new EventLoopEventArgs(context));
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
