/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System;
using System.Threading.Tasks;

namespace ExapisSOP.ConnectionModel
{
	/// <summary>
	///  <see cref="ExapisSOP.ConnectionModel.IConnector"/>と<see cref="ExapisSOP.ConnectionModel.IConnector{TIn, TOut}"/>の機能を拡張します。
	///  このクラスは静的です。
	/// </summary>
	public static class ConnectorExtensions
	{
		/// <summary>
		///  <see cref="ExapisSOP.ConnectionModel.IConnector.Run(object)"/>を非同期で実行します。
		/// </summary>
		/// <param name="connector"><see cref="ExapisSOP.ConnectionModel.IConnector"/>インターフェースを実装したオブジェクトです。</param>
		/// <param name="arg">処理の実行に必要な引数です。</param>
		/// <returns>非同期操作を表すオブジェクトです。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		public static Task RunAsync(this IConnector connector, object arg)
		{
			if (connector == null) {
				throw new ArgumentNullException(nameof(connector));
			}
			connector.Run(arg);
			return Task.CompletedTask;
		}

		/// <summary>
		///  <see cref="ExapisSOP.ConnectionModel.IConnector{TIn, TOut}.Run(TIn)"/>を非同期で実行します。
		/// </summary>
		/// <typeparam name="TIn">接続子の引数の型です。</typeparam>
		/// <typeparam name="TOut">接続子の戻り値の型です。</typeparam>
		/// <param name="connector"><see cref="ExapisSOP.ConnectionModel.IConnector{TIn, TOut}"/>インターフェースを継承したオブジェクトです。</param>
		/// <param name="arg">処理の実行に必要な引数です。</param>
		/// <returns>戻り値を含む非同期操作を表すオブジェクトです。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		public static Task<TOut> RunAsync<TIn, TOut>(this IConnector<TIn, TOut> connector, TIn arg)
		{
			if (connector == null) {
				throw new ArgumentNullException(nameof(connector));
			}
			return Task.FromResult(connector.Run(arg));
		}
	}
}
