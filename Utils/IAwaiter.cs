/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 *
 * distributed under the MIT License.
****/

using System.Runtime.CompilerServices;

namespace ExapisSOP.Utils
{
	/// <summary>
	///  非同期操作を待機しその実行結果を提供します。
	/// </summary>
	/// <typeparam name="T">戻り値の種類です。</typeparam>
	public interface IAwaiter<out T> : INotifyCompletion
	{
		/// <summary>
		///  実行が完了したかどうか示す値を取得します。
		/// </summary>
		bool IsCompleted { get; }

		/// <summary>
		///  実行が完了するまで待機し、戻り値を取得します。
		/// </summary>
		/// <returns>実行結果を表すオブジェクトです。</returns>
		T GetResult();
	}
}
