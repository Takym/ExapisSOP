/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 *
 * distributed under the MIT License.
****/

namespace ExapisSOP.Utils
{
	/// <summary>
	///  <see cref="ExapisSOP.IContext"/>の機能を拡張します。
	///  このクラスは静的です。
	/// </summary>
	public static class UtilityContextExtensions
	{
		/// <summary>
		///  サービスリストから最初に見つかった<see cref="ExapisSOP.Utils.IUtilityService"/>を取得します。
		/// </summary>
		/// <param name="context">現在の文脈情報です。</param>
		/// <returns>
		///  <see cref="ExapisSOP.Utils.IUtilityService"/>を実装したサービスオブジェクトです。
		/// </returns>
		public static IUtilityService? GetUtility(this IContext context)
		{
			return context?.GetService<IUtilityService>();
		}
	}
}
