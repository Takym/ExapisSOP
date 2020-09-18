/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System;

namespace ExapisSOP.IO
{
	/// <summary>
	///  <see cref="ExapisSOP.IO.IFileSystemService"/>の動作方法を指定します。
	/// </summary>
	public class FileSystemServiceOptions
	{
		/// <summary>
		///  ロックファイルを作成し多重起動を制限する場合は<see langword="true"/>、
		///  それ以外の場合は<see langword="false"/>を指定します。
		/// </summary>
		public bool CreateLockFile { get; set; }

		/// <summary>
		///  管理するデータが格納されたディレクトリを取得または設定します。
		/// </summary>
		public PathString DataPath { get; set; }

		/// <summary>
		///  型'<see cref="ExapisSOP.IO.FileSystemServiceOptions"/>'の新しいインスタンスを生成します。
		/// </summary>
		public FileSystemServiceOptions()
		{
			this.DataPath = Paths.GetDefaultPath(DefaultPath.CurrentWorkspace);
		}
	}
}
