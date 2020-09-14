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
	///  文脈情報を格納したイベントデータを表します。
	/// </summary>
	public class ContextEventArgs : EventArgs
	{
		/// <summary>
		///  現在の文脈情報を取得します。
		/// </summary>
		public IContext Context { get; }

		/// <summary>
		///  型'<see cref="ExapisSOP.ContextEventArgs"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="context">文脈情報です。</param>
		public ContextEventArgs(IContext context)
		{
			this.Context = context;
		}
	}
}
