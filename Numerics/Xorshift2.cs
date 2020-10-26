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
	///  内部変数が6つの<see langword="Xorshift"/>アルゴリズムを利用した疑似乱数生成器を表します。
	/// </summary>
	[Serializable()]
	public class Xorshift2 : Xorshift
	{
		protected private ulong _a, _b;

		/// <summary>
		///  シード値を取得します。
		/// </summary>
		public override long Seed
		{
			get => base.Seed;
			protected set
			{
				this.Seed = value;
				_a = _seed;
				_b = (_x | _y) ^ (_z & _w);
			}
		}

		/// <summary>
		///  型'<see cref="ExapisSOP.Numerics.Xorshift2"/>'の新しいインスタンスを生成します。
		/// </summary>
		public Xorshift2() : base() { }

		/// <summary>
		///  型'<see cref="ExapisSOP.Numerics.Xorshift2"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="seed">シード値です。</param>
		public Xorshift2(long seed) : base(seed) { }

		/// <summary>
		///  型'<see cref="ExapisSOP.Numerics.Xorshift2"/>'を逆直列化します。
		/// </summary>
		/// <param name="info">直列化されたデータを含むオブジェクトです。</param>
		/// <param name="context">ストリームの転送先または転送元に関する文脈情報です。</param>
		protected Xorshift2(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			_a = info.GetUInt64("xorshift2_a");
			_b = info.GetUInt64("xorshift2_b");
		}

		/// <summary>
		///  現在のオブジェクトを直列化します。
		/// </summary>
		/// <param name="info">直列化されたデータを含むオブジェクトです。</param>
		/// <param name="context">ストリームの転送先または転送元に関する文脈情報です。</param>
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("xorshift2_a", _a);
			info.AddValue("xorshift2_b", _b);
		}

		/// <summary>
		///  64ビット符号付き整数値を生成します。
		/// </summary>
		/// <returns>結果の分からない値を返します。</returns>
		public override long NextSInt64()
		{
			ulong i = _x ^ (_x << 11);
			_x = _y; _y = _z; _z = _w; _w = _a; _a = _b;
			return unchecked((long)(_b = (_b ^ (_b >> 19)) ^ (i ^ (i >> 8))));
		}
	}
}
