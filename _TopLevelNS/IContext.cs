/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

namespace ExapisSOP
{
	/// <summary>
	///  実行中の処理にコンテキスト情報を提供します。
	/// </summary>
	public interface IContext
	{
		/// <summary>
		///  現在実行中の実行環境を取得します。
		/// </summary>
		/// <returns>実行環境を表すオブジェクトです。</returns>
		HostRunner GetHostRunner();

		/// <summary>
		///  指定されたサービスを取得します。
		/// </summary>
		/// <typeparam name="T">サービスの種類です。</typeparam>
		/// <returns>サービスが存在する場合はサービスオブジェクトを返し、存在しない場合は<see langword="null"/>を返します。</returns>
		T GetService<T>() where T: IService?;

		/// <summary>
		///  現在のコンテキスト情報に設定されているメッセージを取得します。
		/// </summary>
		/// <returns>メッセージを表すオブジェクトです。</returns>
		object GetMessage();

		/// <summary>
		///  指定されたオブジェクトをメッセージとして現在のコンテキスト情報に設定します。
		/// </summary>
		/// <param name="data">メッセージを表すオブジェクトです。</param>
		void SetMessage(object data);
	}
}
