/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System;
using System.IO;
using System.Reflection;

namespace ExapisSOP.IO
{
	/// <summary>
	///  データディレクトリのパス情報を取得します。
	///  このクラスは継承できません。
	/// </summary>
	public sealed class Paths : IPathList
	{
		private static string AsmName => Assembly.GetEntryAssembly()?.GetName()?.Name ?? VersionInfo.Name;

		/// <summary>
		///  既定のデータパスを取得します。
		/// </summary>
		/// <param name="path">既定のデータパスの種類です。</param>
		/// <returns>データパスを表すパス文字列です。</returns>
		public static PathString GetDefaultPath(DefaultPath path)
		{
			switch (path) {
			case DefaultPath.UserData:
				return new PathString(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)) + AsmName;
			case DefaultPath.SystemData:
				return new PathString(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)) + AsmName;
			case DefaultPath.Application:
				return new PathString(AppContext.BaseDirectory) + "Data";
			default:
				return new PathString() + $"{VersionInfo.Name}_Data";
			}
		}

		/// <summary>
		///  管理するデータが格納されたディレクトリを取得します。
		/// </summary>
		public PathString DataRoot { get; }

		/// <summary>
		///  ログファイルが格納されたディレクトリを取得します。
		/// </summary>
		public PathString Logs => this.DataRoot + "logs";

		/// <summary>
		///  一時ファイルが格納されたディレクトリを取得します。
		/// </summary>
		public PathString Temporary => this.DataRoot + "temp";

		/// <summary>
		///  キャッシュファイルが格納されたディレクトリを取得します。
		/// </summary>
		public PathString Caches => this.Temporary + "_caches";

		/// <summary>
		///  設定ファイルが格納されたディレクトリを取得します。
		/// </summary>
		public PathString Settings => this.DataRoot + "settings";

		/// <summary>
		///  ロックファイルが存在するかどうかを表す論理値を取得します。
		/// </summary>
		public bool ExistsLockFile { get; internal set; }

		internal Paths(PathString path)
		{
			this.DataRoot = path;
			Directory.CreateDirectory(this.DataRoot);
			Directory.CreateDirectory(this.Logs);
			Directory.CreateDirectory(this.Temporary);
			Directory.CreateDirectory(this.Caches);
			Directory.CreateDirectory(this.Settings);
		}
	}
}
