/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ExapisSOP.IO
{
	/// <summary>
	///  キャッシュされたストリームを表します。
	/// </summary>
	public sealed class CachedStream : MemoryStream
	{
		private readonly string _cache_file;

		/// <summary>
		///  型'<see cref="ExapisSOP.IO.CachedStream"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="cacheFile">キャッシュファイルへのパスです。</param>
		/// <param name="data">読み書きするデータです。コピーしてから利用されます。</param>
		public CachedStream(string cacheFile, IEnumerable<byte> data)
		{
			_cache_file = cacheFile;
			switch (data) {
			case byte[] bytes:
				if (bytes.Length == 0) break;
				this.Write(bytes, 0, bytes.Length);
				break;
			case IList<byte> byteList:
				if (byteList.Count == 0) break;
				for (int i = 0; i < byteList.Count; ++i) {
					this.WriteByte(byteList[i]);
				}
				break;
			default:
				foreach (var b in data) {
					this.WriteByte(b);
				}
				break;
			}
			this.Flush();
		}

		/// <summary>
		///  データをキャッシュファイルへ書き込みます。
		/// </summary>
		public override void Flush()
		{
			using (var ms = new MemoryStream(this.ToArray(), false))
			using (var fs = new FileStream(_cache_file, FileMode.Create, FileAccess.Write, FileShare.None)) {
				ms.CopyTo(fs);
			}
		}

		/// <summary>
		///  データをキャッシュファイルへ書き込みます。
		/// </summary>
		/// <param name="cancellationToken">操作の取り消しを通知します。</param>
		/// <returns>非同期操作です。</returns>
		public override async Task FlushAsync(CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested) {
				await Task.FromCanceled(cancellationToken);
				return;
			}
			try {
				using (var ms = new MemoryStream(this.ToArray(), false))
				using (var fs = new FileStream(_cache_file, FileMode.Create, FileAccess.Write, FileShare.None)) {
					await ms.CopyToAsync(fs);
				}
			} catch (Exception e) {
				await Task.FromException(e);
			}
		}
	}
}
