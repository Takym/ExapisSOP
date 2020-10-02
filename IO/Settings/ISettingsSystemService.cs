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
		///  この操作による変更は適用されません。
		/// </summary>
		/// <remarks>
		///  非同期的に実行され正しく読み込まれない可能性があります。
		///  この操作による変更を適用させる場合は<see cref="ExapisSOP.IO.Settings.ISettingsSystemService.Apply()"/>関数を呼び出します。
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

		/// <summary>
		///  現在の変更を適用します。
		/// </summary>
		/// <remarks>
		///  非同期的に実行され正しく反映されない可能性があります。
		/// </remarks>
		/// <returns>適用処理の非同期操作です。</returns>
		Task Apply();

		/// <summary>
		///  現在の変更を破棄し、指定された設定情報を現在の設定情報へコピーし、変更を適用します。
		/// </summary>
		/// <remarks>
		///  非同期的に実行され正しく反映されない可能性があります。
		/// </remarks>
		/// <param name="settings">コピー元の設定情報です。</param>
		/// <returns>適用処理の非同期操作です。</returns>
		Task Apply(EnvironmentSettings settings);
	}
}
