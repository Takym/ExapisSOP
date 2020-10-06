/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

namespace ExapisSOP.Utils
{
	/// <summary>
	///  待機可能な関数の戻り値を提供します。
	///  このインターフェースを実装したクラスは非同期操作を表します。
	/// </summary>
	/// <typeparam name="T">戻り値の種類です。</typeparam>
	public interface IAwaitable<out T>
	{
		/// <summary>
		///  この非同期操作を待機する為に使用するオブジェクトを取得します。
		/// </summary>
		/// <returns><see cref="ExapisSOP.Utils.IAwaiter{T}"/>を実装するオブジェクトです。</returns>
		IAwaiter<T> GetAwaiter();
	}
}
