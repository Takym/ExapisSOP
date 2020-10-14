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
	///  未処理の例外が格納されたイベントデータを表します。
	/// </summary>
	public class UnhandledErrorEventArgs : UnhandledExceptionEventArgs
	{
		/// <summary>
		///  例外オブジェクトを取得します。
		/// </summary>
		public Exception Exception { get; }

		/// <summary>
		///  処理を中断する場合は<see langword="true"/>を設定し、
		///  それ以外の場合は<see langword="false"/>を設定してください。
		/// </summary>
		public bool Abort { get; set; }

		/// <summary>
		///  現在の文脈情報を取得します。
		/// </summary>
		public IContext Context { get; }

		/// <summary>
		///  型'<see cref="ExapisSOP.UnhandledErrorEventArgs"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="exception">例外オブジェクトです。</param>
		/// <param name="context">現在の文脈情報です。</param>
		public UnhandledErrorEventArgs(Exception exception, IContext context) : base(exception, false)
		{
			this.Exception = exception;
			this.Context   = context;
		}
	}
}
