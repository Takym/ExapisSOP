/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System.Threading.Tasks;

namespace ExapisSOP.IO.Settings
{
	/// <summary>
	///  設定を管理する機能を提供します。
	/// </summary>
	public interface ISettingsSystemService : IService
	{
		/// <summary>
		///  現在の変更を破棄し、ファイルから設定情報を読み込みます。
		/// </summary>
		/// <remarks>
		///  非同期的に実行され正しく読み込まれない可能性があります。
		/// </remarks>
		/// <param name="context">現在の文脈情報です。</param>
		/// <returns>再読み込み処理の非同期操作です。</returns>
		Task Reload(IContext context);

		/// <summary>
		///  現在の設定情報をファイルに保存します。
		/// </summary>
		/// <remarks>
		///  非同期的に実行され正しく保存されない可能性があります。
		/// </remarks>
		/// <returns>保存処理の非同期操作です。</returns>
		Task Save();
	}
}
