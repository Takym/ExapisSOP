/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

namespace ExapisSOP
{
	/// <summary>
	///  <see cref="ExapisSOP.TerminationException"/>の発生理由を表します。
	/// </summary>
	public enum TerminationReason : int
	{
		/// <summary>
		///  作業が正常に完了した事を表します。
		/// </summary>
		WorkCompleted,

		/// <summary>
		///  例外が発生し異常終了した事を表します。
		/// </summary>
		ThrewException,

		/// <summary>
		///  多重起動が禁止されロックファイルが存在している事を表します。
		/// </summary>
		ProcessLocked,

		/// <summary>
		///  設定ファイルに互換性が存在しない、または、正常に設定情報を読み込めない事を表します。
		/// </summary>
		NoCompatible,

		/// <summary>
		///  コマンド行引数が誤っている事を表します。
		/// </summary>
		InvalidCommandLine
	}
}
