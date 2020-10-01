/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System;

namespace ExapisSOP.IO.Settings.CommandLine
{
	/// <summary>
	///  フィールド変数またはプロパティをオプションとして扱う方法を制御します。
	///  このクラスは継承できません。
	/// </summary>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public sealed class OptionAttribute : Attribute
	{
		/// <summary>
		///  オプションの長い名前を取得します。
		/// </summary>
		public string LongName { get; }

		/// <summary>
		///  オプションの短い名前を取得します。
		/// </summary>
		public string? ShortName { get; }

		/// <summary>
		///  値の型変換に利用する<see cref="ExapisSOP.IO.Settings.CommandLine.IArgumentConverter"/>の種類を取得または設定します。
		/// </summary>
		public Type? ArgumentConverter { get; set; }

		/// <summary>
		///  型'<see cref="ExapisSOP.IO.Settings.CommandLine.OptionAttribute"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="longName">オプションの長い名前です。</param>
		public OptionAttribute(string longName)
		{
			this.LongName = longName;
		}

		/// <summary>
		///  型'<see cref="ExapisSOP.IO.Settings.CommandLine.OptionAttribute"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="longName">オプションの長い名前です。</param>
		/// <param name="shortName">オプションの短い名前です。</param>
		public OptionAttribute(string longName, string shortName)
		{
			this.LongName  = longName;
			this.ShortName = shortName;
		}
	}
}
