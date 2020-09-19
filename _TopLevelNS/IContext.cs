/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using ExapisSOP.IO;

namespace ExapisSOP
{
	/// <summary>
	///  実行中の処理に文脈情報を提供します。
	/// </summary>
	public interface IContext
	{
		/// <summary>
		///  データディレクトリへのパスを格納したオブジェクトを取得します。
		///  <see cref="ExapisSOP.IO.IFileSystemService"/>が初期化されていない場合は<see langword="null"/>を返します。
		/// </summary>
		IPathList? Paths { get; }

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
		T? GetService<T>() where T: class, IService?;

		/// <summary>
		///  現在の文脈情報に設定されているメッセージを取得します。
		/// </summary>
		/// <returns>メッセージを表すオブジェクトです。</returns>
		object? GetMessage();

		/// <summary>
		///  指定されたオブジェクトをメッセージとして現在の文脈情報に設定します。
		/// </summary>
		/// <param name="data">メッセージを表すオブジェクトです。</param>
		void SetMessage(object data);
	}
}
