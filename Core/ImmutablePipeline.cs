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
	/// <summary>
	///  処理リストが不変なパイプラインを表します。
	///  このクラスは継承できません。
	/// </summary>
	public sealed class ImmutablePipeline : IPipeline
	{
		private readonly IProcess[] _processes;

		/// <summary>
		///  型'<see cref="ExapisSOP.Core.ImmutablePipeline"/>'の新しいインスタンスを生成します。
		/// </summary>
		public ImmutablePipeline()
		{
			_processes = Array.Empty<IProcess>();
		}

		/// <summary>
		///  型'<see cref="ExapisSOP.Core.ImmutablePipeline"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="processes">実行する処理を格納した配列です。</param>
		public ImmutablePipeline(params IProcess[] processes)
		{
			_processes = ((IProcess[])(processes.Clone()));
		}

		/// <summary>
		///  このパイプラインに追加された処理を取得します。
		/// </summary>
		/// <returns>処理を表すオブジェクトを含んだ配列です。</returns>
		public IProcess[] GetProcesses()
		{
			return ((IProcess[])(_processes.Clone()));
		}

		/// <summary>
		///  指定された処理を表すオブジェクトを末尾に追加します。
		/// </summary>
		/// <param name="process">追加する処理です。</param>
		/// <returns>現在のインスタンスに指定された処理を追加した新しいオブジェクトを返します。</returns>
		/// <exception cref="System.ArgumentNullException" />
		public IPipeline Append(IProcess process)
		{
			if (process == null) {
				throw new ArgumentNullException(nameof(process));
			}
			var newProcs = new IProcess[_processes.Length + 1];
			Array.Copy(_processes, newProcs, _processes.Length);
			newProcs[_processes.Length] = process;
			return new ImmutablePipeline(newProcs);
		}

		/// <summary>
		///  パイプラインの実行を非同期的に開始します。
		///  最初の処理の引数には<see langword="null"/>を渡し戻り値は破棄します。
		/// </summary>
		/// <param name="context">実行に必要な文脈情報です。</param>
		/// <returns>実行中のパイプラインを表す非同期操作です。</returns>
		/// <exception cref="System.InvalidOperationException" />
		public async Task RunAsync(IContext context)
		{
			await Pipeline.RunAsyncInternal<object?, object?>(_processes, context, null);
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
		public async Task<TResult> RunAsync<TParam, TResult>(IContext context, TParam arg)
		{
			return await Pipeline.RunAsyncInternal<TParam, TResult>(_processes, context, arg);
		}
	}
}
