/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System;
using System.Runtime.Serialization;

namespace ExapisSOP.Numerics
{
	/// <summary>
	///  線形合同法を利用した疑似乱数生成器を表します。
	/// </summary>
	/// <remarks>
	///  Wikipedia: https://en.wikipedia.org/wiki/Linear_congruential_generator
	/// </remarks>
	[Serializable()]
	public class LinearCongruentialGenerator : SerializableRandom
	{
		protected private long _seed, _n;

		/// <summary>
		///  このクラスはシード値を取得する事ができる為、常に<see langword="true"/>を返します。
		/// </summary>
		public override bool CanGetSeed { get => true; protected set { /* do nothing */ } }

		/// <summary>
		///  シード値を取得します。
		/// </summary>
		public override long Seed
		{
			get => _seed;
			protected set
			{
				_seed = _n = value;
			}
		}

		/// <summary>
		///  型'<see cref="ExapisSOP.Numerics.LinearCongruentialGenerator"/>'の新しいインスタンスを生成します。
		/// </summary>
		public LinearCongruentialGenerator()
		{
#if NETCOREAPP3_1
			this.Seed = Environment.TickCount64;
#elif NET48
			this.Seed = Environment.TickCount;
#endif
		}

		/// <summary>
		///  型'<see cref="ExapisSOP.Numerics.LinearCongruentialGenerator"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="seed">シード値です。</param>
		public LinearCongruentialGenerator(long seed)
		{
			this.Seed = seed;
		}

		/// <summary>
		///  型'<see cref="ExapisSOP.Numerics.Xorshift"/>'を逆直列化します。
		/// </summary>
		/// <param name="info">直列化されたデータを含むオブジェクトです。</param>
		/// <param name="context">ストリームの転送先または転送元に関する文脈情報です。</param>
		protected LinearCongruentialGenerator(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			_n = info.GetInt64("lcd_n");
		}

		/// <summary>
		///  現在のオブジェクトを直列化します。
		/// </summary>
		/// <param name="info">直列化されたデータを含むオブジェクトです。</param>
		/// <param name="context">ストリームの転送先または転送元に関する文脈情報です。</param>
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("lcd_n", _n);
		}

		/// <summary>
		///  64ビット符号付き整数値を生成します。
		/// </summary>
		/// <returns>結果の分からない値を返します。</returns>
		public override long NextSInt64()
		{
			const long A = ((long)(7)) * 73 * 127 * 337 * 92737 * 649657 + 1;
			const long B = ((long)(2)) * 5 * 19 * 89 * 461 * 463 * 599 * 727;
			return _n = unchecked(A * _n + B) % long.MaxValue;
		}
	}
}
