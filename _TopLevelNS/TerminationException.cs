/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System;
using System.Runtime.Serialization;
using ExapisSOP.Properties;

namespace ExapisSOP
{
	/// <summary>
	///  処理を終了させる時に発生します。
	/// </summary>
	[Serializable()]
	public class TerminationException : OperationCanceledException
	{
		/// <summary>
		///  型'<see cref="ExapisSOP.TerminationException"/>'の新しいインスタンスを生成します。
		/// </summary>
		public TerminationException()
			: base(Resources.TerminationException) { }

		/// <summary>
		///  型'<see cref="ExapisSOP.TerminationException"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="innerException">内部例外です。</param>
		public TerminationException(Exception innerException)
			: base(Resources.TerminationException_withInnerError, innerException) { }

		/// <summary>
		///  型'<see cref="ExapisSOP.TerminationException"/>'を逆直列化します。
		/// </summary>
		/// <param name="info">直列化されたデータを含むオブジェクトです。</param>
		/// <param name="context">ストリームの転送先または転送元に関する文脈情報です。</param>
		protected TerminationException(SerializationInfo info, StreamingContext context)
			: base(info, context) { }
	}
}
