/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ExapisSOP.Properties;
using ExapisSOP.Utils;

namespace ExapisSOP.Core
{
	/// <summary>
	///  <see cref="ExapisSOP.IPipeline"/>で実行される処理を表します。
	///  このクラスは抽象クラスです。
	/// </summary>
	/// <typeparam name="TParam">この処理の引数の種類です。</typeparam>
	/// <typeparam name="TResult">この処理の戻り値の種類です。</typeparam>
	public abstract class CustomPipelineProcess<TParam, TResult> : IProcess<TParam, TResult>
	{
		/// <summary>
		///  上書きされた場合、この処理が実行可能な状態かどうかを表す論理値を取得します。
		/// </summary>
		/// <remarks>
		///  この値が<see langword="false"/>になった場合は<see cref="ExapisSOP.IProcess.Init"/>を呼び出さなければなりません。
		/// </remarks>
		public abstract bool IsExecutable { get; }

		/// <summary>
		///  この処理の次に実行すべき処理を取得または設定します。
		/// </summary>
		public IProcess NextProcess { get; set; }

		/// <summary>
		///  型'<see cref="ExapisSOP.Core.CustomPipelineProcess{TParam, TResult}"/>'の新しいインスタンスを生成します。
		/// </summary>
		protected CustomPipelineProcess()
		{
			this.NextProcess = TerminationProcess.Empty;
		}

		/// <summary>
		///  上書きされた場合、この処理を実行可能な状態に初期化します。
		///  <see cref="ExapisSOP.IProcess.IsExecutable"/>を<see langword="true"/>に設定します。
		/// </summary>
		/// <param name="context">現在の文脈情報です。</param>
		public void Init(IContext context)
		{
			// do nothing
		}

		/// <summary>
		///  この処理の実行を非同期的に開始します。
		/// </summary>
		/// <param name="context">実行に必要な文脈情報です。</param>
		/// <param name="arg">処理に必要な引数です。</param>
		/// <returns>戻り値を含むこの処理を表す非同期操作です。</returns>
		/// <exception cref="System.InvalidOperationException">
		///  <see cref="ExapisSOP.IProcess.IsExecutable"/>が<see langword="false"/>の時に発生します。
		/// </exception>
		public IAwaitable<TResult> InvokeAsync(IContext context, TParam arg)
		{
			if (!this.IsExecutable) {
				throw new InvalidOperationException(
					string.Format(Resources.CustomPipelineProcess_InvalidOperationException, this.GetType().FullName));
			}
			return new Awaitable(this.InvokeCore(context, arg));
		}

		/// <summary>
		///  この処理の実行を非同期的に開始します。
		/// </summary>
		/// <param name="context">実行に必要な文脈情報です。</param>
		/// <param name="arg">処理に必要な引数です。</param>
		/// <returns>戻り値を含むこの処理を表す非同期操作です。</returns>
		protected abstract Task<TResult> InvokeCore(IContext context, TParam arg);

		async Task<object?> IProcess.InvokeAsync(IContext context, object? arg)
		{
			if (arg is TParam obj) {
				return await this.InvokeAsync(context, obj);
			} else {
				return await Task.FromResult<object?>(null);
			}
		}

		private readonly struct Awaitable : IAwaitable<TResult>
		{
			private readonly Task<TResult> _task;

			internal Awaitable(Task<TResult> task)
			{
				_task = task;
			}

			public IAwaiter<TResult> GetAwaiter()
			{
				return new Awaiter(_task.GetAwaiter());
			}
		}

		private readonly struct Awaiter : IAwaiter<TResult>
		{
			private readonly TaskAwaiter<TResult> _awaiter;

			public bool IsCompleted => _awaiter.IsCompleted;

			internal Awaiter(TaskAwaiter<TResult> awaiter)
			{
				_awaiter = awaiter;
			}

			public TResult GetResult()
			{
				return _awaiter.GetResult();
			}

			public void OnCompleted(Action continuation)
			{
				_awaiter.OnCompleted(continuation);
			}
		}
	}
}
