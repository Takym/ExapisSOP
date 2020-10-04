/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System.Collections.Generic;

namespace ExapisSOP.IO.Logging
{
	/// <summary>
	///  ログファイルを管理する機能を提供します。
	/// </summary>
	public interface ILogFile : IEnumerable<LogData>
	{
		/// <summary>
		///  このログファイルに追加されたログ情報の個数を取得します。
		/// </summary>
		ulong Count { get; }

		/// <summary>
		///  既定の名前でロガーを生成します。
		/// </summary>
		/// <returns>作成された新しいロガーです。</returns>
		ILogger CreateLogger();

		/// <summary>
		///  指定されたログ情報を末尾に追加します。
		/// </summary>
		/// <param name="data">追加するログ情報です。</param>
		/// <exception cref="System.ArgumentNullException"/>
		void AddLog(LogData data);

		/// <summary>
		///  このログファイルから指定された位置のログ情報を取得します。
		/// </summary>
		/// <param name="index">ログ情報のインデックス番号です。</param>
		/// <returns>取得したログ情報を表すオブジェクトです。</returns>
		/// <exception cref="System.ArgumentOutOfRangeException"/>
		LogData GetLog(ulong index);
	}
}
