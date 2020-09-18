/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System;
using System.Reflection;

namespace ExapisSOP.IO
{
	/// <summary>
	///  データディレクトリのパス情報を取得します。
	///  このクラスは継承できません。
	/// </summary>
	public sealed class Paths
	{
		private static string AsmName => Assembly.GetExecutingAssembly().GetName().Name ?? VersionInfo.Name;

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

		internal Paths(PathString path) { }
	}
}
