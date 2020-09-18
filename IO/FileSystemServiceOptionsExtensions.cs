/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

namespace ExapisSOP.IO
{
	/// <summary>
	///  <see cref="ExapisSOP.IConfiguration"/>の機能を拡張します。
	///  このクラスは静的です。
	/// </summary>
	public static class FileSystemServiceOptionsExtensions
	{
		/// <summary>
		///  管理するデータが格納されたディレクトリを設定します。
		/// </summary>
		/// <param name="options">設定情報を格納したオブジェクトです。</param>
		/// <param name="path">既定のデータパスの種類です。</param>
		/// <returns><paramref name="options"/>を返します。</returns>
		public static FileSystemServiceOptions SetDataPath(this FileSystemServiceOptions options, DefaultPath path)
		{
			options.DataPath = Paths.GetDefaultPath(path);
			return options;
		}
	}
}
