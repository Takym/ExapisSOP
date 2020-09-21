/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ExapisSOP.IO
{
	/// <summary>
	///  キャッシュされたストリームを表します。
	/// </summary>
	[Obsolete("現在、充分に動作確認がされていません。ご利用の際は注意してください。")]
	public sealed class CachedStream : Stream
	{
		private readonly string     _cache_file;
		private readonly List<byte> _data;
		private          bool       _data_available;

		/// <summary>
		///  このストリームは読み取りをサポートする為、常に<see langword="true"/>を返します。
		/// </summary>
		public override bool CanRead => true;

		/// <summary>
		///  このストリームは書き込みをサポートする為、常に<see langword="true"/>を返します。
		/// </summary>
		public override bool CanWrite => true;

		/// <summary>
		///  このストリームは位置の変更をサポートする為、常に<see langword="true"/>を返します。
		/// </summary>
		public override bool CanSeek => true;

		/// <summary>
		///  このストリームの現在位置を取得または設定します。
		/// </summary>
		public override long Position { get; set; }

		/// <summary>
		///  このストリームが確保したメモリ容量を取得します。
		/// </summary>
		public int Capacity => _data.Capacity;

		/// <summary>
		///  このストリームが格納している要素の数を符号付き32ビット整数形式で取得します。
		/// </summary>
		public int Count => _data.Count;

		/// <summary>
		///  このストリームが格納している要素の数を符号付き64ビット整数形式で取得します。
		/// </summary>
		public override long Length => _data.Count;

		/// <summary>
		///  このストリームが破棄されている場合は<see langword="true"/>、有効な場合は<see langword="false"/>を返します。
		/// </summary>
		public bool IsDisposed { get; private set; }

		/// <summary>
		///  型'<see cref="ExapisSOP.IO.CachedStream"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="cacheFile">キャッシュファイルへのパスです。</param>
		/// <param name="data">読み書きするデータです。コピーしてから利用されます。</param>
		public CachedStream(string cacheFile, IEnumerable<byte> data)
		{
			_cache_file     = cacheFile;
			_data           = new List<byte>(data ?? new byte[0]);
			_data_available = true;
			this.IsDisposed = false;
		}

		/// <summary>
		///  現在のストリームから1バイトだけ読み取ります。
		/// </summary>
		/// <returns>
		///  読み取ったデータ、または、
		///  ストリームの現在位置が末尾を超えている場合は<c>-1</c>を返します。
		/// </returns>
		/// <exception cref="System.ObjectDisposedException" />
		public override int ReadByte()
		{
			this.Refresh();
			this.ThrowOnObjectDisposed();
			return this.ReadByteCore();
		}

		/// <summary>
		///  現在のストリームからバイト配列を読み取ります。
		/// </summary>
		/// <param name="buffer">読み取ったデータを格納するバイト配列です。</param>
		/// <param name="offset">バイト配列の格納先の先頭位置です。</param>
		/// <param name="count">読み取るデータの個数です。</param>
		/// <returns>読み取る事ができたデータの個数です。</returns>
		/// <exception cref="System.ObjectDisposedException" />
		/// <exception cref="System.ArgumentNullException" />
		/// <exception cref="System.ArgumentOutOfRangeException" />
		public override int Read(byte[] buffer, int offset, int count)
		{
			this.Refresh();
			this.ValidateReadWrite(buffer, offset, count);
			if (this.Position < 0 || this.Position >= _data.Count) {
				return 0;
			}
			for (int i = offset; i < offset + count; ++i) {
				int x = this.ReadByteCore();
				if (x == -1) {
					return i - offset;
				}
				buffer[i] = unchecked((byte)(x));
			}
			return count;
		}

		private int ReadByteCore()
		{
			if (this.Position < 0 || this.Position >= _data.Count) {
				return -1;
			}
			byte result = _data[unchecked((int)(this.Position))];
			++this.Position;
			return result;
		}

		/// <summary>
		///  現在のストリームから1バイトだけ読み取ります。
		///  ただし、ストリーム位置は進めません。
		/// </summary>
		/// <returns>
		///  読み取ったデータ、または、
		///  ストリームの現在位置が末尾を超えている場合は<c>-1</c>を返します。
		/// </returns>
		/// <exception cref="System.ObjectDisposedException" />
		public int Peek()
		{
			this.Refresh();
			int result = this.ReadByteCore();
			--this.Position;
			return result;
		}

		/// <summary>
		///  現在のストリームへ1バイトだけ書き込みます。
		/// </summary>
		/// <returns>
		///  読み取ったデータ、または、
		///  ストリームの現在位置が末尾を超えている場合は<c>-1</c>を返します。
		/// </returns>
		/// <exception cref="System.ObjectDisposedException" />
		/// <exception cref="System.OutOfMemoryException" />
		/// <exception cref="System.OverflowException" />
		public override void WriteByte(byte value)
		{
			this.Refresh();
			this.WriteByteCore(value);
		}

		/// <summary>
		///  現在のストリームへバイト配列を書き込みます。
		/// </summary>
		/// <param name="buffer">読み取ったデータを格納するバイト配列です。</param>
		/// <param name="offset">バイト配列の格納先の先頭位置です。</param>
		/// <param name="count">読み取るデータの個数です。</param>
		/// <returns>読み取る事ができたデータの個数です。</returns>
		/// <exception cref="System.ObjectDisposedException" />
		/// <exception cref="System.ArgumentNullException" />
		/// <exception cref="System.ArgumentOutOfRangeException" />
		/// <exception cref="System.OutOfMemoryException" />
		/// <exception cref="System.OverflowException" />
		public override void Write(byte[] buffer, int offset, int count)
		{
			this.Refresh();
			this.ValidateReadWrite(buffer, offset, count);
			if (this.Position < 0 || this.Position >= _data.Count) {
				return;
			}
			for (int i = offset; i < offset + count; ++i) {
				this.WriteByteCore(buffer[i]);
			}
		}

		private void WriteByteCore(byte value)
		{
			if (this.Position < 0 || this.Position > int.MaxValue) {
				return;
			}
			if (_data.Count <= this.Position) {
				this.SetLength(this.Position + 1);
			}
			_data[(int)(this.Position)] = value;
			++this.Position;
		}

		/// <summary>
		///  現在のストリームの内容をキャッシュファイルへ書き込みます。
		/// </summary>
		/// <exception cref="System.ObjectDisposedException" />
		/// <exception cref="System.IO.IOException" />
		public override void Flush()
		{
			this.ThrowOnObjectDisposed();
			if (_data_available) {
				_data_available = false;
				try {
					using (var ms = new MemoryStream(_data.ToArray(), true))
					using (var fs = new FileStream(_cache_file, FileMode.Create, FileAccess.Write, FileShare.None)) {
						ms.CopyTo(fs);
					}
					_data.Clear();
					_data.Capacity = 0;
				} catch (Exception e) {
					throw new IOException(e.Message, e);
				}
			}
		}

		/// <summary>
		///  キャッシュファイルからデータを現在のストリームへ再度読み込みます。
		/// </summary>
		/// <exception cref="System.ObjectDisposedException" />
		/// <exception cref="System.IO.IOException" />
		public void Refresh()
		{
			this.ThrowOnObjectDisposed();
			if (!_data_available) {
				try {
					using (var fs = new FileStream(_cache_file, FileMode.Open, FileAccess.Read, FileShare.None)) {
						int b;
						while ((b = fs.ReadByte()) != -1) {
							_data.Add(unchecked((byte)(b)));
						}
					}
					_data_available = true;
				} catch (Exception e) {
					throw new IOException(e.Message, e);
				}
			}
		}

		/// <summary>
		///  非同期的にキャッシュファイルからデータを現在のストリームへ再度読み込みます。
		/// </summary>
		/// <returns>非同期操作です。</returns>
		public async Task RefreshAsync()
		{
			await this.RefreshAsync(CancellationToken.None);
		}

		/// <summary>
		///  非同期的にキャッシュファイルからデータを現在のストリームへ再度読み込みます。
		/// </summary>
		/// <param name="cancellationToken">操作の終了を通知するオブジェクトです。</param>
		/// <returns>非同期操作です。</returns>
		public Task RefreshAsync(CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested) {
				return Task.FromCanceled(cancellationToken);
			}
			try {
				this.Refresh();
				return Task.CompletedTask;
			} catch (Exception e) {
				return Task.FromException(e);
			}
		}

		/// <summary>
		///  ストリームの位置を変更します。
		/// </summary>
		/// <param name="offset">ストリームの新しい位置の相対値です。</param>
		/// <param name="origin">相対値を絶対値へ変換する為の方法です。</param>
		/// <returns>新しいストリームの位置の絶対値です。</returns>
		/// <exception cref="System.ObjectDisposedException" />
		/// <exception cref="System.ArgumentException" />
		public override long Seek(long offset, SeekOrigin origin)
		{
			this.ThrowOnObjectDisposed();
			switch (origin) {
			case SeekOrigin.Begin:
				return this.Position = offset;
			case SeekOrigin.Current:
				return this.Position += offset;
			case SeekOrigin.End:
				return this.Position = this.Length + offset;
			default:
				throw new ArgumentException(string.Empty, nameof(origin));
			}
		}

		/// <summary>
		///  ストリームの大きさを変更します。
		/// </summary>
		/// <param name="value">新しいストリーム長です。</param>
		/// <exception cref="System.ObjectDisposedException" />
		/// <exception cref="System.OutOfMemoryException" />
		/// <exception cref="System.OverflowException" />
		public override void SetLength(long value)
		{
			this.SetLength((int)(value));
		}

		/// <summary>
		///  ストリームの大きさを変更します。
		/// </summary>
		/// <param name="value">新しいストリーム長です。</param>
		/// <exception cref="System.ObjectDisposedException" />
		/// <exception cref="System.OutOfMemoryException" />
		public void SetLength(int value)
		{
			this.Refresh();
			if (_data.Count < value) {
				_data.AddRange(new byte[value - _data.Count]);
			} else if (_data.Count > value) {
				int count = _data.Count - value;
				_data.RemoveRange(_data.Count - count - 1, count);
			}
		}

		/// <summary>
		///  現在のオブジェクトインスタンスを破棄します。
		/// </summary>
		/// <param name="disposing">
		///  マネージドオブジェクトとアンマネージオブジェクト両方を破棄する場合は<see langword="true"/>、
		///  アンマネージオブジェクトのみを破棄する場合は<see langword="false"/>を設定します。
		/// </param>
		protected override void Dispose(bool disposing)
		{
			if (!this.IsDisposed) {
				base.Dispose(disposing);
				if (disposing) {
					if (File.Exists(_cache_file)) {
						File.Delete(_cache_file);
					}
				}
				_data.Clear();
				_data.Capacity = 0;
				this.IsDisposed = true;
			}
		}

		[DebuggerHidden()]
		[StackTraceHidden()]
		private void ValidateReadWrite(byte[] buffer, int offset, int count)
		{
			this.ThrowOnObjectDisposed();
			if (buffer == null) {
				throw new ArgumentNullException(nameof(buffer));
			}
			this.ThrowOnOutOfRange(buffer.Length, offset, count);
		}

		[DebuggerHidden()]
		[StackTraceHidden()]
		private void ThrowOnObjectDisposed()
		{
			if (this.IsDisposed) {
				throw new ObjectDisposedException(nameof(CachedStream));
			}
		}

		[DebuggerHidden()]
		[StackTraceHidden()]
		private void ThrowOnOutOfRange(int bufferLength, int offset, int count)
		{
			if (offset < 0) {
				throw new ArgumentOutOfRangeException(nameof(offset), offset, );
			}
			if (count < 0) {
				throw new ArgumentOutOfRangeException(nameof(count), count, );
			}
			if (bufferLength < offset) {
				throw new ArgumentOutOfRangeException(nameof(offset), offset, );
			}
			if (bufferLength < offset + count) {
				throw new ArgumentOutOfRangeException(nameof(count), count, );
			}
		}
	}
}
