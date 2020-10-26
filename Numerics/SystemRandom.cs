/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using ExapisSOP.Resources.Utils;

namespace ExapisSOP.Numerics
{
	/// <summary>
	///  <see cref="System.Random"/>を<see cref="ExapisSOP.Numerics.IRandom"/>にラップします。
	///  このクラスは継承できません。
	/// </summary>
	[Serializable()]
	public sealed class SystemRandom : SerializableRandom
	{
		private Random _rnd;
		private long   _seed;

		/// <summary>
		///  シード値を取得します。
		/// </summary>
		/// <exception cref="System.InvalidOperationException"/>
		public override long Seed
		{
			get
			{
				if (this.CanGetSeed) {
					return _seed;
				} else {
					throw new InvalidOperationException(StringRes.SystemRandom_InvalidOperationException);
				}
			}

			protected set
			{
				if ((unchecked((ulong)(_seed)) & 0xFFFFFFFF00000000) == 0) {
					_rnd            = new Random(unchecked((int)(_seed)));
					_seed           = value;
					this.CanGetSeed = true;
				} else {
					_rnd            = new Random(_seed.GetHashCode());
					_seed           = value;
					this.CanGetSeed = true;
				}
			}
		}

		/// <summary>
		///  型'<see cref="ExapisSOP.Numerics.SystemRandom"/>'の新しいインスタンスを生成します。
		/// </summary>
		public SystemRandom()
		{
			_rnd = null!;
#if NETCOREAPP3_1
			this.Seed = Environment.TickCount64;
#elif NET48
			this.Seed = Environment.TickCount;
#endif
		}

		/// <summary>
		///  型'<see cref="ExapisSOP.Numerics.SystemRandom"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="seed">シード値です。</param>
		public SystemRandom(int seed)
		{
			_rnd = null!;
			this.Seed = seed;
		}

		/// <summary>
		///  型'<see cref="ExapisSOP.Numerics.SystemRandom"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="rnd">ラップする<see cref="System.Random"/>オブジェクトです。</param>
		/// <exception cref="System.ArgumentNullException"/>
		public SystemRandom(Random rnd)
		{
			_rnd            = rnd ?? throw new ArgumentNullException(nameof(rnd));
			this.CanGetSeed = false;
		}

		/// <summary>
		///  8ビット符号無し整数値を指定された数だけ生成します。
		/// </summary>
		/// <param name="count">生成する乱数値の個数です。</param>
		/// <returns>結果の分からない値を返します。</returns>
		/// <exception cref="System.ArgumentOutOfRangeException"/>
		public override byte[] NextBytes(int count)
		{
			if (count <= 0) {
				throw new ArgumentOutOfRangeException(nameof(count), count, StringRes.Random_ArgumentOutOfRangeException);
			}
			byte[] result = new byte[count];
			_rnd.NextBytes(result);
			return result;
		}

		/// <summary>
		///  64ビット符号付き整数値を生成します。
		/// </summary>
		/// <returns>結果の分からない値を返します。</returns>
		public override long NextSInt64()
		{
			(long h, long l) = (_rnd.Next(), _rnd.Next());
			return (h << 32) | l;
		}

		/// <summary>
		///  倍精度浮動小数点数値を0～1の範囲で生成します。
		/// </summary>
		/// <returns>結果の分からない値を返します。</returns>
		public override double NextDouble()
		{
			return _rnd.NextDouble();
		}
	}
}
