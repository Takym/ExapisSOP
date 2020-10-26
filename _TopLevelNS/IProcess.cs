/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 *
 * distributed under the MIT License.
****/

using System.Threading.Tasks;
using ExapisSOP.Utils;

namespace ExapisSOP
{
	/// <summary>
	///  <see cref="ExapisSOP.IPipeline"/>で実行される処理を表します。
	/// </summary>
	public interface IProcess
	{
		/// <summary>
		///  この処理が実行可能な状態かどうかを表す論理値を取得します。
		/// </summary>
		/// <remarks>
		///  この値が<see langword="false"/>になった場合は<see cref="ExapisSOP.IProcess.Init"/>を呼び出さなければなりません。
		/// </remarks>
		bool IsExecutable { get; }

		/// <summary>
		///  この処理の次に実行すべき処理を取得または設定します。
		/// </summary>
		IProcess NextProcess { get; set; }

		/// <summary>
		///  この処理を実行可能な状態に初期化します。
		///  <see cref="ExapisSOP.IProcess.IsExecutable"/>を<see langword="true"/>に設定します。
		/// </summary>
		/// <param name="context">現在の文脈情報です。</param>
		void Init(IContext context);

		/// <summary>
		///  この処理の実行を非同期的に開始します。
		/// </summary>
		/// <param name="context">実行に必要な文脈情報です。</param>
		/// <param name="arg">処理に必要な引数です。</param>
		/// <returns>戻り値を含むこの処理を表す非同期操作です。</returns>
		/// <exception cref="System.InvalidOperationException">
		///  <see cref="ExapisSOP.IProcess.IsExecutable"/>が<see langword="false"/>の時に発生します。
		/// </exception>
		Task<object?> InvokeAsync(IContext context, object? arg);
	}

	/// <summary>
	///  <see cref="ExapisSOP.IPipeline"/>で実行される処理を表します。
	/// </summary>
	public interface IProcess<in TParam, out TResult> : IProcess
	{
		/// <summary>
		///  この処理の実行を非同期的に開始します。
		/// </summary>
		/// <param name="context">実行に必要な文脈情報です。</param>
		/// <param name="arg">処理に必要な引数です。</param>
		/// <returns>戻り値を含むこの処理を表す非同期操作です。</returns>
		/// <exception cref="System.InvalidOperationException">
		///  <see cref="ExapisSOP.IProcess.IsExecutable"/>が<see langword="false"/>の時に発生します。
		/// </exception>
		IAwaitable<TResult> InvokeAsync(IContext context, TParam arg);
	}
}
