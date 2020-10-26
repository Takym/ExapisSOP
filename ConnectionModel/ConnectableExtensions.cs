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
	///  <see cref="ExapisSOP.ConnectionModel.IConnectable"/>と<see cref="ExapisSOP.ConnectionModel.IConnectable{TIn, TOut}"/>の機能を拡張します。
	///  このクラスは静的です。
	/// </summary>
	public static class ConnectableExtensions
	{
		/// <summary>
		///  処理を実行します。
		/// </summary>
		/// <param name="connectable"><see cref="ExapisSOP.ConnectionModel.IConnectable"/>インターフェースを実装したオブジェクトです。</param>
		/// <param name="arg">処理の実行に必要な引数です。</param>
		/// <exception cref="System.ArgumentNullException"/>
		public static void Run(this IConnectable connectable, object arg)
		{
			if (connectable == null) {
				throw new ArgumentNullException(nameof(connectable));
			}
			using (var connector = connectable.GetConnector()) {
				connector.Run(arg);
			}
		}

		/// <summary>
		///  処理を実行します。
		/// </summary>
		/// <typeparam name="TIn">接続子の引数の型です。</typeparam>
		/// <typeparam name="TOut">接続子の戻り値の型です。</typeparam>
		/// <param name="connectable"><see cref="ExapisSOP.ConnectionModel.IConnectable{TIn, TOut}"/>インターフェースを実装したオブジェクトです。</param>
		/// <param name="arg">処理の実行に必要な引数です。</param>
		/// <returns>実行結果を表すオブジェクトです。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		public static TOut Run<TIn, TOut>(this IConnectable<TIn, TOut> connectable, TIn arg)
		{
			if (connectable == null) {
				throw new ArgumentNullException(nameof(connectable));
			}
			using (var connector = connectable.GetConnector()) {
				return connector.Run(arg);
			}
		}

		/// <summary>
		///  非同期で処理を実行します。
		/// </summary>
		/// <param name="connectable"><see cref="ExapisSOP.ConnectionModel.IConnectable"/>インターフェースを実装したオブジェクトです。</param>
		/// <param name="arg">処理の実行に必要な引数です。</param>
		/// <returns>非同期操作を表すオブジェクトです。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		public static async Task RunAsync(this IConnectable connectable, object arg)
		{
			if (connectable == null) {
				throw new ArgumentNullException(nameof(connectable));
			}
			using (var connector = connectable.GetConnector()) {
				await connector.RunAsync(arg);
			}
		}

		/// <summary>
		///  非同期で処理を実行します。
		/// </summary>
		/// <typeparam name="TIn">接続子の引数の型です。</typeparam>
		/// <typeparam name="TOut">接続子の戻り値の型です。</typeparam>
		/// <param name="connectable"><see cref="ExapisSOP.ConnectionModel.IConnectable{TIn, TOut}"/>インターフェースを実装したオブジェクトです。</param>
		/// <param name="arg">処理の実行に必要な引数です。</param>
		/// <returns>戻り値を含む非同期操作を表すオブジェクトです。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		public static async Task<TOut> RunAsync<TIn, TOut>(this IConnectable<TIn, TOut> connectable, TIn arg)
		{
			if (connectable == null) {
				throw new ArgumentNullException(nameof(connectable));
			}
			using (var connector = connectable.GetConnector()) {
				return await connector.RunAsync(arg);
			}
		}
	}
}
