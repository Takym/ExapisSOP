/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System;

namespace ExapisSOP.ConnectionModel
{
	/// <summary>
	///  接続可能なオブジェクトの戻り値を返さない接続子を提供します。
	/// </summary>
	public interface IConnector : IDisposable
	{
		/// <summary>
		///  処理を実行します。
		/// </summary>
		/// <param name="arg">処理の実行に必要な引数です。</param>
		void Run(object arg);
	}

	/// <summary>
	///  接続可能なオブジェクトの戻り値を返す接続子を提供します。
	/// </summary>
	/// <typeparam name="TIn">引数の型です。</typeparam>
	/// <typeparam name="TOut">戻り値の型です。</typeparam>
	public interface IConnector<in TIn, out TOut> : IConnector
	{
		/// <summary>
		///  処理を実行します。
		/// </summary>
		/// <param name="arg">処理の実行に必要な引数です。</param>
		/// <returns>実行結果を表すオブジェクトです。</returns>
		TOut Run(TIn arg);

#if NETCOREAPP3_1
		void IConnector.Run(object arg)
		{
			if (arg is TIn objIn) {
				this.Run(objIn);
			}
		}
#endif
	}
}
