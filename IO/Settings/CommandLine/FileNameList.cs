/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 *
 * distributed under the MIT License.
****/

namespace ExapisSOP.IO.Settings.CommandLine
{
	/// <summary>
	///  ファイル名の一覧をコマンド行引数から入力します。
	/// </summary>
	[Switch("")]
	[Manual("en", "Sets file name list.")]
	[Manual("ja", "ファイル名の一覧を設定します。")]
	public class FileNameList
	{
		/// <summary>
		///  コマンド行引数から入力したファイル名を取得または設定します。
		/// </summary>
		[Option("")]
		[Manual("en", "Sets file names.")]
		[Manual("ja", "ファイル名を設定します。")]
		public string[]? FileNames { get; set; }

		/// <summary>
		///  コマンド行引数から入力した入力用(読み取り専用)のファイル名を取得または設定します。
		/// </summary>
		[Option("input", "i", "in")]
		[Manual("en", "Sets input file names. (read-only)")]
		[Manual("ja", "入力用ファイル名を設定します。(読み取り専用)")]
		public string[]? InputFiles { get; set; }

		/// <summary>
		///  コマンド行引数から入力した出力用(書き込み専用)のファイル名を取得または設定します。
		/// </summary>
		[Option("output", "o", "out")]
		[Manual("en", "Sets output file names. (write-only)")]
		[Manual("ja", "出力用ファイル名を設定します。(書き込み専用)")]
		public string[]? OutputFiles { get; set; }

		/// <summary>
		///  型'<see cref="ExapisSOP.IO.Settings.CommandLine.FileNameList"/>'の新しいインスタンスを生成します。
		/// </summary>
		public FileNameList() { }
	}
}
