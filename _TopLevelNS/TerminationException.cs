/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Runtime.Serialization;
using System.Threading;
using ExapisSOP.Properties;

namespace ExapisSOP
{
	/// <summary>
	///  処理を終了させる時に発生させます。
	/// </summary>
	[Serializable()]
	public class TerminationException : OperationCanceledException
	{
		private const string PREFIX = "Termination";

		/// <summary>
		///  この例外が発生した理由を表す列挙体を取得します。
		/// </summary>
		public TerminationReason Reason { get; }

		/// <summary>
		///  型'<see cref="ExapisSOP.TerminationException"/>'の新しいインスタンスを生成します。
		/// </summary>
		public TerminationException()
			: base(Resources.TerminationException)
		{
			this.Reason  = TerminationReason.WorkCompleted;
			this.HResult = 0; // この操作を正しく終了しました。
		}

		/// <summary>
		///  型'<see cref="ExapisSOP.TerminationException"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="innerException">内部例外です。</param>
		public TerminationException(Exception innerException)
			: base(Resources.TerminationException_withInnerError, innerException)
		{
			this.Reason  = TerminationReason.ThrewException;
			this.HResult = innerException.HResult;
		}

		/// <summary>
		///  型'<see cref="ExapisSOP.TerminationException"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="message">この例外を説明する翻訳済みのエラーメッセージです。</param>
		/// <param name="reason">この例外の発生理由です。</param>
		/// <param name="token">処理を終了させた操作に関連付けられているトークンです。</param>
		public TerminationException(string message, TerminationReason reason, CancellationToken token)
			: base(message, token)
		{
			this.Reason = reason;
			switch (reason) {
			case TerminationReason.ProcessLocked:
				this.HResult = 5; // アクセスが拒否されました。
				break;
			case TerminationReason.NoCompatible:
				this.HResult = 13; // データが無効です。
				break;
			case TerminationReason.InvalidCommandLine:
				this.HResult = 1; // ファンクションが間違っています。
				break;
			}
		}

		/// <summary>
		///  型'<see cref="ExapisSOP.TerminationException"/>'を逆直列化します。
		/// </summary>
		/// <param name="info">直列化されたデータを含むオブジェクトです。</param>
		/// <param name="context">ストリームの転送先または転送元に関する文脈情報です。</param>
		protected TerminationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			this.Reason = ((TerminationReason)(info.GetInt32($"{PREFIX}_{nameof(this.Reason)}")));
		}

		/// <summary>
		///  現在の例外を直列化します。
		/// </summary>
		/// <param name="info">直列化されたデータを含むオブジェクトです。</param>
		/// <param name="context">ストリームの転送先または転送元に関する文脈情報です。</param>
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue($"{PREFIX}_{nameof(this.Reason)}", ((int)(this.Reason)));
		}
	}
}
