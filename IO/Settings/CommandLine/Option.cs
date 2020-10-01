/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

namespace ExapisSOP.IO.Settings.CommandLine
{
	/// <summary>
	///  コマンド行引数のオプションを表します。
	/// </summary>
	public class Option
	{
		/// <summary>
		///  このオプションの名前を取得します。
		/// </summary>
		public string Name { get; }

		/// <summary>
		///  このオプションに登録されている値を取得します。
		/// </summary>
		public string[] Values { get; }

		/// <summary>
		///  型'<see cref="ExapisSOP.IO.Settings.CommandLine.Option"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="name">新しいオプションの名前です。</param>
		/// <param name="values">新しいオプションに登録する値です。</param>
		public Option(string name, string[] values)
		{
			this.Name   = name;
			this.Values = values;
		}
	}
}
