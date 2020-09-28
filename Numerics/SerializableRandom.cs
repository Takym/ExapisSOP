/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System;
using System.Runtime.Serialization;
using ExapisSOP.Resources.Utils;

namespace ExapisSOP.Numerics
{
	/// <summary>
	///  直列化可能な疑似乱数生成器を表します。
	/// </summary>
	[Serializable()]
	public abstract class SerializableRandom : IRandom, ISerializable
	{
		/// <summary>
		///  シード値を取得する事ができるかどうかを表す論理値を取得します。
		/// </summary>
		public virtual bool CanGetSeed { get; protected set; }

		/// <summary>
		///  上書きされた場合、シード値を取得します。
		/// </summary>
		/// <exception cref="System.InvalidOperationException"/>
		public abstract long Seed { get; protected set; }

		/// <summary>
		///  型'<see cref="ExapisSOP.Numerics.SerializableRandom"/>'の新しいインスタンスを生成します。
		/// </summary>
		public SerializableRandom() { }

		/// <summary>
		///  型'<see cref="ExapisSOP.Numerics.SerializableRandom"/>'を逆直列化します。
		/// </summary>
		/// <param name="info">直列化されたデータを含むオブジェクトです。</param>
		/// <param name="context">ストリームの転送先または転送元に関する文脈情報です。</param>
		protected SerializableRandom(SerializationInfo info, StreamingContext context)
		{
			this.CanGetSeed = info.GetBoolean(nameof(this.CanGetSeed));
			if (this.CanGetSeed) {
				this.Seed = info.GetInt64(nameof(this.Seed));
			}
		}

		/// <summary>
		///  上書きされた場合、現在のオブジェクトを直列化します。
		/// </summary>
		/// <param name="info">直列化されたデータを含むオブジェクトです。</param>
		/// <param name="context">ストリームの転送先または転送元に関する文脈情報です。</param>
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue(nameof(this.CanGetSeed), this.CanGetSeed);
			if (this.CanGetSeed) {
				info.AddValue(nameof(this.Seed), this.Seed);
			}
		}

		/// <summary>
		///  上書きされた場合、8ビット符号無し整数値を指定された数だけ生成します。
		/// </summary>
		/// <param name="count">生成する乱数値の個数です。</param>
		/// <returns>結果の分からない値を返します。</returns>
		/// <exception cref="System.ArgumentOutOfRangeException"/>
		public virtual byte[] NextBytes(int count)
		{
			if (count <= 0) {
				throw new ArgumentOutOfRangeException(nameof(count), count, StringRes.Random_ArgumentOutOfRangeException);
			}
			byte[] result = new byte[count];
			for (int i = 0; i < count; ++i) {
				var b = BitConverter.GetBytes(this.NextSInt64());
				for (int j = 0; j < b.Length; ++j) {
					result[i] ^= b[j];
				}
			}
			return result;
		}

		/// <summary>
		///  上書きされた場合、64ビット符号付き整数値を生成します。
		/// </summary>
		/// <returns>結果の分からない値を返します。</returns>
		public abstract long NextSInt64();

		/// <summary>
		///  上書きされた場合、倍精度浮動小数点数値を0～1の範囲で生成します。
		/// </summary>
		/// <returns>結果の分からない値を返します。</returns>
		public virtual double NextDouble()
		{
			return 1.0D / this.NextSInt64();
		}
	}
}
