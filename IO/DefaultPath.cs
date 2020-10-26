/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 *
 * distributed under the MIT License.
****/

namespace ExapisSOP.IO
{
	/// <summary>
	///  既定のデータパスの種類を表します。
	/// </summary>
	public enum DefaultPath
	{
		/// <summary>
		///  現在の作業ディレクトリを表します。
		/// </summary>
		CurrentWorkspace,

		/// <summary>
		///  ユーザーの設定ディレクトリを表します。
		/// </summary>
		UserData,

		/// <summary>
		///  システムの設定ディレクトリを表します。
		///  管理者権限が必要になる可能性があります。
		/// </summary>
		SystemData,

		/// <summary>
		///  アプリケーションディレクトリを表します。
		///  管理者権限が必要になる可能性があります。
		/// </summary>
		Application
	}
}
