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
	///  プログラム起動時に利用される構成設定を表します。
	/// </summary>
	public interface IConfiguration
	{
		/// <summary>
		///  指定されたサービスを実行環境に対して登録します。
		/// </summary>
		/// <param name="service">登録するサービスオブジェクトです。</param>
		/// <returns>現在のインスタンス、または、現在のインスタンスに指定されたサービスを追加した新しいオブジェクトを返します。</returns>
		IConfiguration AddService(IService service);

		/// <summary>
		///  現在登録されている全てのサービスを取得します。
		/// </summary>
		/// <returns>複数のサービスオブジェクトを含む配列です。</returns>
		IService[] GetServices();

		/// <summary>
		///  プログラム実行中に処理されない例外が発生した場合に呼び出されます。
		///  <see cref="ExapisSOP.AppWorker.UnhandledError"/>より後に呼び出されます。
		/// </summary>
		event EventHandler<UnhandledErrorEventArgs>? UnhandledError;

		/// <summary>
		///  プログラム実行中に処理が終了する時に呼び出されます。
		///  <see cref="ExapisSOP.AppWorker.Terminate"/>より後に呼び出されます。
		/// </summary>
		event EventHandler<TerminationEventArgs>? Terminate;
	}
}
