/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 *
 * distributed under the MIT License.
****/

namespace ExapisSOP.IO
{
	/// <summary>
	///  データディレクトリのパスを取得する機能を提供します。
	/// </summary>
	public interface IPathList
	{
		/// <summary>
		///  管理するデータが格納されたディレクトリを取得します。
		/// </summary>
		PathString DataRoot { get; }

		/// <summary>
		///  ログファイルが格納されたディレクトリを取得します。
		/// </summary>
		PathString Logs { get; }

		/// <summary>
		///  一時ファイルが格納されたディレクトリを取得します。
		/// </summary>
		PathString Temporary { get; }

		/// <summary>
		///  キャッシュファイルが格納されたディレクトリを取得します。
		/// </summary>
		PathString Caches { get; }

		/// <summary>
		///  設定ファイルが格納されたディレクトリを取得します。
		/// </summary>
		PathString Settings { get; }
	}
}
