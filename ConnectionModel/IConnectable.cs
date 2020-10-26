/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 *
 * distributed under the MIT License.
****/

namespace ExapisSOP.ConnectionModel
{
	/// <summary>
	///  接続可能なオブジェクトを表します。
	/// </summary>
	public interface IConnectable
	{
		/// <summary>
		///  他のオブジェクトを現在のオブジェクトに接続します。
		/// </summary>
		/// <param name="connectableObject">接続するオブジェクトです。</param>
		void ConnectWith(IConnectable connectableObject);

		/// <summary>
		///  他のオブジェクトとの接続を解除します。
		/// </summary>
		/// <param name="connectableObject">接続を解除するオブジェクトです。</param>
		/// <returns>接続の解除に成功した場合は<see langword="true"/>、失敗した場合は<see langword="false"/>を返します。</returns>
		bool Disconnect(IConnectable connectableObject);

		/// <summary>
		///  接続子を取得します。
		/// </summary>
		/// <returns>現在のオブジェクトの接続子を提供するオブジェクトです。</returns>
		IConnector GetConnector();
	}

	/// <summary>
	///  戻り値を返す接続子を利用した接続可能なオブジェクトを表します。
	/// </summary>
	/// <typeparam name="TIn">接続子の引数の型です。</typeparam>
	/// <typeparam name="TOut">接続子の戻り値の型です。</typeparam>
	public interface IConnectable<in TIn, out TOut> : IConnectable
	{
		/// <summary>
		///  接続子を取得します。
		/// </summary>
		/// <returns>現在のオブジェクトの接続子を提供するオブジェクトです。</returns>
		new IConnector<TIn, TOut> GetConnector();

#if NETCOREAPP3_1
		IConnector IConnectable.GetConnector()
		{
			return this.GetConnector();
		}
#endif
	}
}
