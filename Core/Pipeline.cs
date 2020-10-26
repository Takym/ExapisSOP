/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExapisSOP.Core
{
	/// <summary>
	///  処理の流れを管理します。
	/// </summary>
	public class Pipeline : IPipeline
	{
		private readonly List<IProcess> _processes;

		/// <summary>
		///  型'<see cref="ExapisSOP.Core.Pipeline"/>'の新しいインスタンスを生成します。
		/// </summary>
		public Pipeline()
		{
			_processes = new List<IProcess>();
		}

		/// <summary>
		///  このパイプラインに追加された処理を取得します。
		/// </summary>
		/// <returns>処理を表すオブジェクトを含んだ配列です。</returns>
		public IProcess[] GetProcesses()
		{
			lock (_processes) {
				return _processes.ToArray();
			}
		}

		/// <summary>
		///  指定された処理を表すオブジェクトを末尾に追加します。
		/// </summary>
		/// <param name="process">追加する処理です。</param>
		/// <returns>連鎖呼び出しを行う為に現在のインスタンスを返します。</returns>
		/// <exception cref="System.ArgumentNullException" />
		public virtual IPipeline Append(IProcess process)
		{
			if (process == null) {
				throw new ArgumentNullException(nameof(process));
			}
			lock (_processes) {
				_processes.Add(process);
			}
			return this;
		}

		/// <summary>
		///  パイプラインの実行を非同期的に開始します。
		///  最初の処理の引数には<see langword="null"/>を渡し戻り値は破棄します。
		/// </summary>
		/// <param name="context">実行に必要な文脈情報です。</param>
		/// <returns>実行中のパイプラインを表す非同期操作です。</returns>
		/// <exception cref="System.InvalidOperationException" />
		public virtual async Task RunAsync(IContext context)
		{
			await RunAsyncInternal<object?, object?>(this.GetProcesses(), context, null);
		}

		/// <summary>
		///  パイプラインの実行を非同期的に開始します。
		/// </summary>
		/// <typeparam name="TParam">実行する処理の引数の種類です。</typeparam>
		/// <typeparam name="TResult">実行する処理の戻り値の種類です。</typeparam>
		/// <param name="context">実行に必要な文脈情報です。</param>
		/// <param name="arg">処理に必要な引数です。</param>
		/// <returns>戻り値を含む実行中のパイプラインを表す非同期操作です。</returns>
		/// <exception cref="System.InvalidOperationException" />
		public virtual async Task<TResult> RunAsync<TParam, TResult>(IContext context, TParam arg)
		{
			return await RunAsyncInternal<TParam, TResult>(this.GetProcesses(), context, arg);
		}

		internal static async Task<TResult> RunAsyncInternal<TParam, TResult>(IProcess[] procs, IContext context, TParam arg)
		{
			if (procs.Length == 0) {
				return default!;
			}
			for (int i = 0; i < procs.Length; ++i) {
				if ((i + 1) < procs.Length) {
					procs[i].NextProcess = procs[i + 1];
				} else {
					procs[i].NextProcess = TerminationProcess.Empty;
				}
				if (!procs[i].IsExecutable) {
					procs[i].Init(context);
				}
			}
			var value = await procs[0].InvokeAsync(context, arg);
			if (value is TResult result) {
				return result;
			} else {
				return default!;
			}
		}

		/// <summary>
		///  新しいパイプラインを作成します。
		/// </summary>
		/// <param name="isImmutable">パイプラインを不変にする場合は<see langword="true"/>、それ以外の場合は<see langword="false"/>です。</param>
		/// <returns>新しく生成されたパイプラインのインスタンスです。</returns>
		public static IPipeline Create(bool isImmutable = false)
		{
			if (isImmutable) {
				return new ImmutablePipeline();
			} else {
				return new Pipeline();
			}
		}
	}
}
