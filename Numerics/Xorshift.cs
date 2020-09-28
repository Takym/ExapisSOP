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
	///  <see langword="Xorshift"/>アルゴリズムを利用した疑似乱数生成器を表します。
	/// </summary>
	/// <remarks>
	///  論文：http://www.jstatsoft.org/v08/i14/paper
	/// </remarks>
	[Serializable()]
	public class Xorshift : SerializableRandom
	{
		protected private ulong _seed, _x, _y, _z, _w;

		/// <summary>
		///  このクラスはシード値を取得する事ができる為、常に<see langword="true"/>を返します。
		/// </summary>
		public override bool CanGetSeed { get => true; protected set { /* do nothing */ } }

		/// <summary>
		///  シード値を取得します。
		/// </summary>
		public override long Seed
		{
			get => unchecked((long)(_seed));
			protected set
			{
				_seed = unchecked((ulong)(value));
				_x = _seed ^ 0x83F38C937DE3A4B3;
				_y = _seed ^ 0xCF2628AE4CD41B08;
				_z = ((_seed >> 32) | (_seed << 32));
				_w = _x ^ _y;
			}
		}

		/// <summary>
		///  型'<see cref="ExapisSOP.Numerics.Xorshift"/>'の新しいインスタンスを生成します。
		/// </summary>
		public Xorshift()
		{
#if NETCOREAPP3_1
			this.Seed = Environment.TickCount64;
#elif NET48
			this.Seed = Environment.TickCount;
#endif
		}

		/// <summary>
		///  型'<see cref="ExapisSOP.Numerics.Xorshift"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="seed">シード値です。</param>
		public Xorshift(long seed)
		{
			this.Seed = seed;
		}

		/// <summary>
		///  型'<see cref="ExapisSOP.Numerics.Xorshift"/>'を逆直列化します。
		/// </summary>
		/// <param name="info">直列化されたデータを含むオブジェクトです。</param>
		/// <param name="context">ストリームの転送先または転送元に関する文脈情報です。</param>
		protected Xorshift(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			_x = info.GetUInt64("xorshift_x");
			_y = info.GetUInt64("xorshift_y");
			_z = info.GetUInt64("xorshift_z");
			_w = info.GetUInt64("xorshift_w");
		}

		/// <summary>
		///  現在のオブジェクトを直列化します。
		/// </summary>
		/// <param name="info">直列化されたデータを含むオブジェクトです。</param>
		/// <param name="context">ストリームの転送先または転送元に関する文脈情報です。</param>
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("xorshift_x", _x);
			info.AddValue("xorshift_y", _y);
			info.AddValue("xorshift_z", _z);
			info.AddValue("xorshift_w", _w);
		}

		/// <summary>
		///  64ビット符号付き整数値を生成します。
		/// </summary>
		/// <returns>結果の分からない値を返します。</returns>
		public override long NextSInt64()
		{
			ulong i = _x ^ (_x << 11);
			_x = _y; _y = _z; _z = _w;
			return unchecked((long)(_w = (_w ^ (_w >> 19)) ^ (i ^ (i >> 8))));
		}
	}
}
