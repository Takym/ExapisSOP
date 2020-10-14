/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System;

namespace ExapisSOP
{
	/// <summary>
	///  <see cref="ExapisSOP.TerminationException"/>が格納されたイベントデータを表します。
	/// </summary>
	public class TerminationEventArgs : ContextEventArgs
	{
		/// <summary>
		///  処理を終了させた例外を取得します。
		/// </summary>
		public TerminationException Exception { get; }

		/// <summary>
		///  処理を終了させる理由を取得します。
		/// </summary>
		public TerminationReason Reason => this.Exception.Reason;

		/// <summary>
		///  型'<see cref="ExapisSOP.TerminationEventArgs"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="te">終了例外オブジェクトです。</param>
		/// <param name="context">現在の文脈情報です。</param>
		public TerminationEventArgs(TerminationException te, IContext context) : base(context)
		{
			this.Exception = te;
		}
	}
}
