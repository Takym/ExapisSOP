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
	///  イベントループ時に利用されるコンテキスト情報を格納したイベントデータを表します。
	/// </summary>
	public class EventLoopEventArgs : EventArgs
	{
		/// <summary>
		///  現在のコンテキスト情報を取得します。
		/// </summary>
		public IContext Context { get; }

		/// <summary>
		///  型'<see cref="ExapisSOP.EventLoopEventArgs"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="context">コンテキスト情報です。</param>
		public EventLoopEventArgs(IContext context)
		{
			this.Context = context;
		}
	}
}
