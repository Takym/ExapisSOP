/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 *
 * distributed under the MIT License.
****/

using System.Threading.Tasks;

namespace ExapisSOP
{
	/// <summary>
	///  処理の流れを管理する機能を提供します。
	/// </summary>
	public interface IPipeline
	{
		/// <summary>
		///  このパイプラインに追加された処理を取得します。
		/// </summary>
		/// <returns>処理を表すオブジェクトを含んだ配列です。</returns>
		IProcess[] GetProcesses();

		/// <summary>
		///  指定された処理を表すオブジェクトを末尾に追加します。
		/// </summary>
		/// <param name="process">追加する処理です。</param>
		/// <returns>現在のインスタンス、または、現在のインスタンスに指定された処理を追加した新しいオブジェクトを返します。</returns>
		/// <exception cref="System.ArgumentNullException" />
		IPipeline Append(IProcess process);

		/// <summary>
		///  パイプラインの実行を非同期的に開始します。
		///  最初の処理の引数には<see langword="null"/>を渡し戻り値は破棄します。
		/// </summary>
		/// <param name="context">実行に必要な文脈情報です。</param>
		/// <returns>実行中のパイプラインを表す非同期操作です。</returns>
		/// <exception cref="System.InvalidOperationException" />
		Task RunAsync(IContext context);

		/// <summary>
		///  パイプラインの実行を非同期的に開始します。
		/// </summary>
		/// <typeparam name="TParam">実行する処理の引数の種類です。</typeparam>
		/// <typeparam name="TResult">実行する処理の戻り値の種類です。</typeparam>
		/// <param name="context">実行に必要な文脈情報です。</param>
		/// <param name="arg">処理に必要な引数です。</param>
		/// <returns>戻り値を含む実行中のパイプラインを表す非同期操作です。</returns>
		/// <exception cref="System.InvalidOperationException" />
		Task<TResult> RunAsync<TParam, TResult>(IContext context, TParam arg);
	}
}
