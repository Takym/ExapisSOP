/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

namespace ExapisSOP.IO.Settings.CommandLine
{
	/// <summary>
	///  コマンド行引数のスイッチを表します。
	/// </summary>
	public class Switch
	{
		/// <summary>
		///  このスイッチの名前を取得します。
		/// </summary>
		public string Name { get; }

		/// <summary>
		///  このスイッチに登録されているオプションを取得します。
		/// </summary>
		public Option[] Options { get; }

		/// <summary>
		///  型'<see cref="ExapisSOP.IO.Settings.CommandLine.Switch"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="name">新しいスイッチの名前です。</param>
		/// <param name="options">新しいスイッチに登録するオプションです。</param>
		public Switch(string name, Option[] options)
		{
			this.Name    = name;
			this.Options = options;
		}
	}
}
