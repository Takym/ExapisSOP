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
	///  クラス、構造体、またはインターフェースをスイッチとして扱う方法を制御します。
	///  このクラスは継承できません。
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
	public sealed class SwitchAttribute : Attribute
	{
		/// <summary>
		///  スイッチの長い名前を取得します。
		/// </summary>
		public string LongName  { get; }

		/// <summary>
		///  スイッチの短い名前を取得します。
		/// </summary>
		public string? ShortName { get; }

		/// <summary>
		///  スイッチの別の名前を取得します。
		/// </summary>
		public string? AltName { get; }

		/// <summary>
		///  型'<see cref="ExapisSOP.IO.Settings.CommandLine.SwitchAttribute"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="longName">スイッチの長い名前です。</param>
		public SwitchAttribute(string longName)
		{
			this.LongName = longName;
		}

		/// <summary>
		///  型'<see cref="ExapisSOP.IO.Settings.CommandLine.SwitchAttribute"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="longName">スイッチの長い名前です。</param>
		/// <param name="shortName">スイッチの短い名前です。</param>
		public SwitchAttribute(string longName, string shortName)
		{
			this.LongName  = longName;
			this.ShortName = shortName;
		}

		/// <summary>
		///  型'<see cref="ExapisSOP.IO.Settings.CommandLine.SwitchAttribute"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="longName">スイッチの長い名前です。</param>
		/// <param name="shortName">スイッチの短い名前です。</param>
		/// <param name="altName">スイッチの別の名前です。</param>
		public SwitchAttribute(string longName, string shortName, string altName)
		{
			this.LongName  = longName;
			this.ShortName = shortName;
			this.AltName   = altName;
		}
	}
}
