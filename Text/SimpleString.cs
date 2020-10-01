/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

namespace ExapisSOP.Text
{
	/// <summary>
	///  <see cref="ExapisSOP.Text.SimpleEncoding"/>形式の文字列値を表します。
	///  このクラスは継承できません。
	/// </summary>
	public sealed class SimpleString
	{
		private static readonly SimpleEncoding _enc  = new SimpleEncoding();
		private static readonly SimpleEncoding _enc2 = new SimpleEncoding(true);

		/// <summary>
		///  <see cref="ExapisSOP.Text.SimpleEncoding"/>形式に符号化された文字列を表すバイト配列です。
		/// </summary>
		public byte[] Value { get; set; }

		/// <summary>
		///  型'<see cref="ExapisSOP.Text.SimpleString"/>'の新しいインスタンスを生成します。
		/// </summary>
		public SimpleString()
		{
			this.Value = new byte[0];
		}

		/// <summary>
		///  型'<see cref="ExapisSOP.Text.SimpleString"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="value">新しいインスタンスに設定する文字列です。</param>
		public SimpleString(string value)
		{
			this.Value = null!;
			this.FromString(value);
		}

		/// <summary>
		///  型'<see cref="ExapisSOP.Text.SimpleString"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="value">新しいインスタンスに設定する文字列です。</param>
		public SimpleString(byte[] value)
		{
			this.Value = value;
		}

		/// <summary>
		///  文字列から<see cref="ExapisSOP.Text.SimpleString.Value"/>を設定します。
		/// </summary>
		/// <param name="value">変換する文字列です。</param>
		public void FromString(string value)
		{
			this.Value = _enc.GetBytes(value);
		}

		/// <summary>
		///  <see cref="ExapisSOP.Text.SimpleString.Value"/>を文字列へ変換します。
		/// </summary>
		/// <returns>変換された文字列です。</returns>
		public override string ToString()
		{
			return _enc.GetString(this.Value);
		}

		/// <summary>
		///  <see cref="ExapisSOP.Text.SimpleString.Value"/>を文字列へ変換します。
		///  NULL文字は取り除きます。
		/// </summary>
		/// <returns>変換された文字列です。</returns>
		public string ToStringWithoutNull()
		{
			return _enc2.GetString(this.Value);
		}

		/// <summary>
		///  指定したオブジェクトと現在のオブジェクトが等価かどうか判定します。
		/// </summary>
		/// <param name="obj">判定するオブジェクトです。</param>
		/// <returns>等しい場合は<see langword="true"/>、等しくない場合は<see langword="false"/>を返します。</returns>
		public override bool Equals(object? obj)
		{
			if (obj is SimpleEncoding s) {
				return this.ToString() == s.ToString();
			}
			return false;
		}

		/// <summary>
		///  現在のオブジェクトのハッシュ値を取得します。
		/// </summary>
		/// <returns>現在のオブジェクトが格納している文字列値のハッシュ値です。</returns>
		public override int GetHashCode()
		{
			return this.ToString().GetHashCode();
		}
	}
}
