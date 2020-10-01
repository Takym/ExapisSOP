/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using ExapisSOP.Resources.Utils;

namespace ExapisSOP.Numerics
{
	/// <summary>
	///  <see cref="System.Security.Cryptography.RandomNumberGenerator"/>を<see cref="ExapisSOP.Numerics.IRandom"/>にラップします。
	///  このクラスは継承できません。
	/// </summary>
	[Serializable()]
	public sealed class CryptionRandom : SerializableRandom, IDisposable
	{
		private readonly RandomNumberGenerator _rng;

		/// <summary>
		///  このオブジェクトが破棄されている場合は<see langword="true"/>、有効な場合は<see langword="false"/>を返します。
		/// </summary>
		public bool IsDisposed { get; private set; }

		/// <summary>
		///  このクラスはシード値を取得する事ができない為、常に<see langword="false"/>を返します。
		/// </summary>
		public override bool CanGetSeed { get => true; protected set { /* do nothing */ } }

		/// <summary>
		///  <see cref="System.InvalidOperationException"/>を発生させます。
		/// </summary>
		/// <exception cref="System.InvalidOperationException"/>
		public override long Seed
		{
			get
			{
				throw new InvalidOperationException(StringRes.CryptionRandom_InvalidOperationException);
			}
			protected set { }
		}

		/// <summary>
		///  型'<see cref="ExapisSOP.Numerics.CryptionRandom"/>'の新しいインスタンスを生成します。
		/// </summary>
		public CryptionRandom()
		{
			_rng = RandomNumberGenerator.Create();
		}

		/// <summary>
		///  型'<see cref="ExapisSOP.Numerics.CryptionRandom"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="rng">ラップする<see cref="System.Random"/>オブジェクトです。</param>
		/// <exception cref="System.ArgumentNullException"/>
		public CryptionRandom(RandomNumberGenerator rng)
		{
			_rng = rng ?? throw new ArgumentNullException(nameof(rng));
		}

		private CryptionRandom(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			_rng = RandomNumberGenerator.Create();
		}

		/// <summary>
		///  型'<see cref="ExapisSOP.Numerics.CryptionRandom"/>'の現在のインスタンスを破棄します。
		/// </summary>
		~CryptionRandom()
		{
			this.Dispose(false);
		}

		/// <summary>
		///  8ビット符号無し整数値を指定された数だけ生成します。
		/// </summary>
		/// <param name="count">生成する乱数値の個数です。</param>
		/// <returns>結果の分からない値を返します。</returns>
		/// <exception cref="System.ArgumentOutOfRangeException"/>
		/// <exception cref="System.ObjectDisposedException" />
		public override byte[] NextBytes(int count)
		{
			if (this.IsDisposed) {
				throw new ObjectDisposedException(nameof(CryptionRandom));
			}
			if (count <= 0) {
				throw new ArgumentOutOfRangeException(nameof(count), count, StringRes.Random_ArgumentOutOfRangeException);
			}
			byte[] result = new byte[count];
			_rng.GetBytes(result);
			return result;
		}

		/// <summary>
		///  0を除く8ビット符号無し整数値を指定された数だけ生成します。
		/// </summary>
		/// <param name="count">生成する乱数値の個数です。</param>
		/// <returns>結果の分からない値を返します。</returns>
		/// <exception cref="System.ArgumentOutOfRangeException"/>
		/// <exception cref="System.ObjectDisposedException" />
		public byte[] NextNonZeroBytes(int count)
		{
			if (this.IsDisposed) {
				throw new ObjectDisposedException(nameof(CryptionRandom));
			}
			if (count <= 0) {
				throw new ArgumentOutOfRangeException(nameof(count), count, StringRes.Random_ArgumentOutOfRangeException);
			}
			byte[] result = new byte[count];
			_rng.GetNonZeroBytes(result);
			return result;
		}

		/// <summary>
		///  64ビット符号付き整数値を生成します。
		/// </summary>
		/// <returns>結果の分からない値を返します。</returns>
		/// <exception cref="System.ObjectDisposedException" />
		public override long NextSInt64()
		{
			if (this.IsDisposed) {
				throw new ObjectDisposedException(nameof(CryptionRandom));
			}
			return BitConverter.ToInt64(this.NextBytes(8));
		}

		/// <summary>
		///  倍精度浮動小数点数値を0～1の範囲で生成します。
		/// </summary>
		/// <returns>結果の分からない値を返します。</returns>
		/// <exception cref="System.ObjectDisposedException" />
		public override double NextDouble()
		{
			if (this.IsDisposed) {
				throw new ObjectDisposedException(nameof(CryptionRandom));
			}
			return base.NextDouble();
		}

		/// <summary>
		///  現在のオブジェクトインスタンスと利用しているリソースを破棄します。
		/// </summary>
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (!this.IsDisposed) {
				if (disposing) {
					_rng.Dispose();
				}
				this.IsDisposed = true;
			}
		}
	}
}
