/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System;
using System.IO;
using System.Threading.Tasks;

namespace ExapisSOP.IO
{
	/// <summary>
	///  ファイルシステムを管理する機能を提供します。
	/// </summary>
	public interface IFileSystemService : IService
	{
		/// <summary>
		///  データファイルを開きます。
		/// </summary>
		/// <remarks>
		///  閉じる時は<see cref="ExapisSOP.IO.IFileSystemService.CloseStream(Stream)"/>を呼び出してください。
		/// </remarks>
		/// <param name="name">開くデータファイルの名前です。</param>
		/// <returns>開いたファイルの読み書きを行うファイルストリームです。</returns>
		/// <exception cref="System.IO.IOException" />
		FileStream OpenDataFile(string name);

		/// <summary>
		///  ログファイルを開きます。
		/// </summary>
		/// <remarks>
		///  閉じる時は<see cref="ExapisSOP.IO.IFileSystemService.CloseStream(Stream)"/>を呼び出してください。
		/// </remarks>
		/// <param name="name">開くログファイルの名前です。</param>
		/// <returns>開いたファイルの読み書きを行うファイルストリームです。</returns>
		/// <exception cref="System.IO.IOException" />
		FileStream OpenLogFile(string name);

		/// <summary>
		///  一時ファイルを開きます。
		/// </summary>
		/// <remarks>
		///  閉じる時は<see cref="ExapisSOP.IO.IFileSystemService.CloseStream(Stream)"/>を呼び出してください。
		/// </remarks>
		/// <param name="name">開く一時ファイルの名前です。</param>
		/// <returns>開いたファイルの読み書きを行うファイルストリームです。</returns>
		/// <exception cref="System.IO.IOException" />
		FileStream OpenTempFile(string name);

		/// <summary>
		///  設定ファイルを開きます。
		/// </summary>
		/// <remarks>
		///  閉じる時は<see cref="ExapisSOP.IO.IFileSystemService.CloseStream(Stream)"/>を呼び出してください。
		/// </remarks>
		/// <param name="name">開く設定ファイルの名前です。</param>
		/// <returns>開いたファイルの読み書きを行うファイルストリームです。</returns>
		/// <exception cref="System.IO.IOException" />
		FileStream OpenSettingFile(string name);

		/// <summary>
		///  メモリを開きます。
		/// </summary>
		/// <remarks>
		///  閉じる時は<see cref="ExapisSOP.IO.IFileSystemService.CloseStream(Stream)"/>を呼び出してください。
		/// </remarks>
		/// <param name="bin">開くメモリにコピーされる初期データです。</param>
		/// <returns>開いたメモリの読み書きを行うメモリストリームです。</returns>
		/// <exception cref="System.IO.IOException" />
		MemoryStream OpenMemory(params byte[] bin);

		/// <summary>
		///  指定したストリームにバッファリングレイヤーを追加します。
		/// </summary>
		/// <remarks>
		///  閉じる時は<see cref="ExapisSOP.IO.IFileSystemService.CloseStream(Stream)"/>を呼び出してください。
		/// </remarks>
		/// <param name="s">バッファリングレイヤーを追加するストリームです。</param>
		/// <returns>バッファストリームです。</returns>
		/// <exception cref="System.IO.IOException" />
		BufferedStream AddBufferingLayer(Stream s);

		/// <summary>
		///  キャッシュされたメモリを開きます。
		/// </summary>
		/// <remarks>
		///  閉じる時は<see cref="ExapisSOP.IO.IFileSystemService.CloseStream(Stream)"/>を呼び出してください。
		/// </remarks>
		/// <returns>キャッシュストリームです。</returns>
		/// <exception cref="System.IO.IOException" />
		[Obsolete("現在、充分に動作確認がされていません。ご利用の際は注意してください。")]
		CachedStream OpenCachedMemory();

		/// <summary>
		///  指定したストリームを閉じます。
		/// </summary>
		/// <param name="s">閉じるストリームです。</param>
		/// <returns>閉じる事ができた場合は<see langword="true"/>、それ以外の場合は<see langword="false"/>を返します。</returns>
		/// <exception cref="System.IO.IOException" />
		bool CloseStream(Stream s);

		/// <summary>
		///  指定したストリームを非同期で閉じます。
		/// </summary>
		/// <param name="s">閉じるストリームです。</param>
		/// <returns>
		///  非同期操作を表す<see cref="System.Threading.Tasks.Task{TResult}"/>オブジェクトです。
		///  閉じる事ができた場合は<see langword="true"/>、それ以外の場合は<see langword="false"/>を返します。
		/// </returns>
		/// <exception cref="System.IO.IOException" />
		Task<bool> CloseStreamAsync(Stream s);
	}
}
