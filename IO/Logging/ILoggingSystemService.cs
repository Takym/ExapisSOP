/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 *
 * distributed under the MIT License.
****/

using System;

namespace ExapisSOP.IO.Logging
{
	/// <summary>
	///  ログ出力処理を管理する機能を提供します。
	/// </summary>
	public interface ILoggingSystemService : IService
	{
		/// <summary>
		///  エラーレポートを作成し保存します。
		/// </summary>
		/// <param name="context">現在の文脈情報です。</param>
		/// <param name="exception">作成するエラーレポートの例外です。</param>
		/// <param name="detailProviders">追加情報を翻訳するオブジェクトの配列です。</param>
		/// <returns>
		///  エラーレポートの保存先を表すファイルパスです。
		///  ファイルシステム管理サービスの取得に失敗した等の原因で、エラーレポートを保存できなかった場合は<see langword="null"/>を返します。
		/// </returns>
		/// <exception cref="System.ArgumentNullException"/>
		PathString? SaveErrorReport(IContext context, Exception exception, params ICustomErrorDetailProvider[] detailProviders);
	}
}
